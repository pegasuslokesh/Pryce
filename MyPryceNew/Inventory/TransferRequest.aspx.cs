using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
public partial class Inventory_TransferRequest : BasePage
{
    #region Class Object
    Inv_TransferRequestHeader ObjTrans = null;
    Inv_TransferRequestDetail OBjtransDetail = null;
    Inv_UnitMaster objUnit = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_SalesOrderDetail ObjSOrderDetail = null;
    Inv_StockDetail objStockDetail = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    LocationMaster objLocation = null;
    Set_DocNumber objDocNo = null;
    Production_Process objProductionProcess = null;
    Production_BOM objProductionBom = null;
    PageControlCommon objPageCmn = null;
    Common cmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjTrans = new Inv_TransferRequestHeader(Session["DBConnection"].ToString());
        OBjtransDetail = new Inv_TransferRequestDetail(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjSOrderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objProductionProcess = new Production_Process(Session["DBConnection"].ToString());
        objProductionBom = new Production_BOM(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        //btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/TransferRequest.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            AllPageCode(clsPagePermission);
            txtlRequestNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = GetDocumentNumber();
            txtRequestdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtCalenderExtender.Format = Session["DateFormat"].ToString();
            Fillgrid();
            btnReset_Click(null, null);
            rbtnFormView.Checked = true;
            rbtnFormView_OnCheckedChanged(null, null);
            hdnJobId.Value = "0";
            CalendarExtendertxtValueRequestdate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtbinValueDate.Format = Session["DateFormat"].ToString();
        }
        //AllPageCode();
    }
    #region System Function :-
    protected void btnPRequest_Click(object sender, EventArgs e)
    {
        FillRequestGrid();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtbinValue.Focus();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = false;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        if (txtRequestdate.Text == "")
        {
            DisplayMessage("Enter Request Date");
            txtRequestdate.Focus();
            btnSave.Enabled = true;
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtRequestdate.Text);
            }
            catch
            {
                DisplayMessage("Enter Request Date in format " + Session["DateFormat"].ToString() + "");
                txtRequestdate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRequestdate);
                btnSave.Enabled = true;
                return;
            }
        }
        if (txtlRequestNo.Text == "")
        {
            ViewState["Return"] = 1;
            DisplayMessage("Request No. Not Found , You Can set in document Page !");
            txtlRequestNo.Focus();
            btnSave.Enabled = true;
            return;
        }
        else
        {
            DataTable dt = ObjTrans.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0");
            try
            {
                if (editid.Value == "")
                {
                    dt = new DataView(dt, "RequestNo='" + txtlRequestNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    dt = new DataView(dt, "RequestNo='" + txtlRequestNo.Text + "' and Trans_ID<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            catch
            {
            }
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Request Number is already exists");
                txtlRequestNo.Focus();
                btnSave.Enabled = true;
                return;
            }
            dt = null;
        }
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtRequestdate.Text), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            btnSave.Enabled = true;
            return;
        }

        if (txtLocationName.Text == "")
        {
            ViewState["Return"] = 1;
            DisplayMessage("Enter Location Name");
            txtLocationName.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (gvProductRequest.Rows.Count == 0)
        {
            ViewState["Return"] = 1;
            DisplayMessage("Enter Product Details");
            btnAddProduct.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtLocationName.Text == "")
        {
            DisplayMessage("Enter Location Name");
            txtLocationName.Focus();
            btnSave.Enabled = true;
            return;
        }
        string post = "Y";
        string StrStatus = string.Empty;
        if (editid.Value != "")
        {
            if (chkReopen.Checked)
            {
                StrStatus = "0";
            }
            else
            {
                StrStatus = ViewState["Status"].ToString();
            }
        }
        string LocationId = string.Empty;
        DataTable Dtlocation = objLocation.GetLocationMasterByLocationName(Session["CompId"].ToString(), txtLocationName.Text);
        if (Dtlocation.Rows.Count == 0)
        {
            DisplayMessage("Invalid Location");
            txtLocationName.Focus();
            btnSave.Enabled = true;
            return;
        }
        else
        {
            LocationId = Dtlocation.Rows[0]["Location_Id"].ToString();
        }
        Dtlocation = null;
        int b = 0;
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (editid.Value == "")
            {
                b = ObjTrans.InsertTransferRequestHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), txtlRequestNo.Text, ObjSysParam.getDateForInput(txtRequestdate.Text).ToString(), txtTermCondition.Text, post, "0", LocationId.ToString(), false.ToString(), false.ToString(), hdnJobId.Value, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                string strMaxId = string.Empty;
                if (b != 0)
                {
                    strMaxId = b.ToString();
                    if (txtlRequestNo.Text == ViewState["DocNo"].ToString())
                    {
                        using (DataTable dtCount = ObjTrans.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", ref trns))
                        {
                            if (dtCount.Rows.Count == 0)
                            {
                                ObjTrans.Updatecode(b.ToString(), txtlRequestNo.Text + "1", ref trns);
                                txtlRequestNo.Text = txtlRequestNo.Text + "1";
                            }
                            else
                            {
                                ObjTrans.Updatecode(b.ToString(), txtlRequestNo.Text + dtCount.Rows.Count, ref trns);
                                txtlRequestNo.Text = txtlRequestNo.Text + dtCount.Rows.Count;
                            }
                        }
                    }
                    DisplayMessage("Record Saved","green");
                    foreach (GridViewRow gvr in gvProductRequest.Rows)
                    {
                        Label lblSerialNo = (Label)gvr.FindControl("lblSerialNO");
                        Label lblProductId = (Label)gvr.FindControl("lblPID");
                        Label lblUnitId = (Label)gvr.FindControl("lblUnitId");
                        Label lblReqQty = (Label)gvr.FindControl("lblReqQty");
                        Label lblUnitCost = (Label)gvr.FindControl("lblunitcost");
                        HiddenField hdnSalesOrderId = (HiddenField)gvr.FindControl("hdnSalesOrderId");
                        // Label lblProductDescription = (Label)gvr.FindControl("lblTermCondition");
                        DataTable dtProduct = new DataTable();
                        try
                        {
                            dtProduct = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), lblProductId.Text.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        }
                        catch
                        {
                            lblProductId.Text = "0";
                        }
                        OBjtransDetail.InsertTransferRequestDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), lblSerialNo.Text, lblProductId.Text, b.ToString(), lblUnitId.Text, "0", lblReqQty.Text, "0", "0", hdnSalesOrderId.Value, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        dtProduct = null;
                    }
                }
            }
            else
            {
                b = ObjTrans.UpdateTransferRequestHeader(editid.Value, Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), txtlRequestNo.Text, ObjSysParam.getDateForInput(txtRequestdate.Text).ToString(), txtTermCondition.Text, post, StrStatus, LocationId.ToString(), "", false.ToString(), hdnJobId.Value, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    OBjtransDetail.DeleteTransferRequestDetailBYReqID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, ref trns);
                    foreach (GridViewRow gvr in gvProductRequest.Rows)
                    {
                        Label lblSerialNo = (Label)gvr.FindControl("lblSerialNO");
                        Label lblProductId = (Label)gvr.FindControl("lblPID");
                        Label lblUnitId = (Label)gvr.FindControl("lblUnitId");
                        Label lblReqQty = (Label)gvr.FindControl("lblReqQty");
                        Label lblUnitCost = (Label)gvr.FindControl("lblunitcost");
                        HiddenField hdnSalesOrderId = (HiddenField)gvr.FindControl("hdnSalesOrderId");
                        // Label lblProductDescription = (Label)gvr.FindControl("lblTermCondition");
                        DataTable dtProduct = new DataTable();
                        try
                        {
                            dtProduct = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), lblProductId.Text.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        }
                        catch
                        {
                            lblProductId.Text = "0";
                        }
                        int eid = Convert.ToInt32(editid.Value.ToString());
                        OBjtransDetail.InsertTransferRequestDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), lblSerialNo.Text, lblProductId.Text, eid.ToString(), lblUnitId.Text, "0", lblReqQty.Text, "0", "0", hdnSalesOrderId.Value, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        dtProduct = null;
                    }
                    DisplayMessage("Record Update");
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
            ViewState["RequestNo"] = txtlRequestNo.Text;
            Fillgrid();
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
            btnSave.Enabled = true;
            return;
        }
        btnSave.Enabled = true;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        txtlRequestNo.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //if (editid.Value == "")
        //{
        //        string Id = ObjTrans.getAutoId();
        //        OBjtransDetail.DeleteTransferRequestDetailBYReqID(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), Id.ToString());
        //}
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = ObjTrans.GetRecordUsingTransId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            chkReopen.Visible = false;
            chkReopen.Checked = false;
            if (dt.Rows[0]["Status"].ToString() == "2")
            {
                DisplayMessage("Transfer Request in use,can not be Update");
                return;
            }
            editid.Value = e.CommandArgument.ToString();
            txtlRequestNo.Text = dt.Rows[0]["RequestNo"].ToString();
            DataTable dtlocation = objLocation.GetLocationMasterById(Session["CompId"].ToString(), dt.Rows[0]["RequestLocationID"].ToString());
            txtRequestdate.Text = Convert.ToDateTime(dt.Rows[0]["TDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtTermCondition.Text = dt.Rows[0]["Remark"].ToString();
            txtLocationName.Text = dtlocation.Rows[0]["Location_Name"].ToString();
            hdnJobId.Value = dt.Rows[0]["Field3"].ToString();
            ViewState["Status"] = dt.Rows[0]["Status"].ToString();
            if (dt.Rows[0]["Status"].ToString() == "1")
            {
                chkReopen.Checked = false;
                chkReopen.Visible = true;
            }
            string Post = dt.Rows[0]["Post"].ToString();
            fillgridDetail();
            if (hdnJobId.Value != "0")
            {
                rbtnFormView.Visible = false;
                rbtnAdvancesearchView.Visible = false;
                btnAddProduct.Visible = false;
                btnAddProductScreen.Visible = false;
                btnAddtoList.Visible = false;
            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        dt = null;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValueRequestdate.Text = "";
        txtValueRequestdate.Visible = false;
        txtValue.Visible = true;
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 3;
        txtValue.Focus();
        Fillgrid();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable DtTransHeader = ObjTrans.GetRecordUsingTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        if (DtTransHeader.Rows.Count > 0)
        {
            if (DtTransHeader.Rows[0]["Status"].ToString() == "2")
            {
                DisplayMessage("Transfer Request in use,can not be Delete");
                return;
            }
        }
        ObjTrans.DeleteTransferRequestHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
        Fillgrid();
        //FillGridBin();
        DtTransHeader = null;
        DisplayMessage("Record Deleted");
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "TDate")
        {
            if (txtValueRequestdate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueRequestdate.Text);
                    txtValue.Text = Convert.ToDateTime(txtValueRequestdate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueRequestdate.Text = "";
                    txtValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueRequestdate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueRequestdate.Focus();
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
            DataTable dtCust = (DataTable)Session["DtTransferRequest"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Trans_Req"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvTransferRequest, view.ToTable(), "", "");
            //AllPageCode();
            //btnbind.Focus();
            dtCust = null;
        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueRequestdate.Text != "")
            txtValueRequestdate.Focus();
    }
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        if (txtLocationName.Text == "")
        {
            DisplayMessage("Enter Request Location Name");
            txtLocationName.Focus();
            return;
        }
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("Trans_Id", typeof(float));
        DtProduct.Columns.Add("SerialNo", typeof(float));
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("Quantity");
        DtProduct.Columns.Add("UnitCost");
        DtProduct.Columns.Add("ProductDescription");
        DtProduct.Columns.Add("salesOrderNo");
        DtProduct.Columns.Add("SalesOrderId");
        if (txtProductcode.Text == "")
        {
            DisplayMessage("Enter Product Id");
            txtProductcode.Text = "";
            txtProductcode.Focus();
            return;
        }
        if (txtProductName.Text == "")
        {
            DisplayMessage("Enter Product Name");
            txtProductName.Text = "";
            txtProductName.Focus();
            return;
        }
        if (ddlUnit == null)
        {
            DisplayMessage("Unit Not Found");
            return;
        }
        if (txtRequestQty.Text == "")
        {
            txtRequestQty.Text = "1";
        }
        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;
        if (editid.Value == "")
        {
            ReqId = ObjTrans.getAutoId();
        }
        else
        {
            ReqId = editid.Value.ToString();
        }
        float serialNo = 0;
        string serailNo = string.Empty;
        if (hidProduct.Value == "")
        {
            int Serial = gvProductRequest.Rows.Count;
            Serial = Serial + 1;
            serailNo = Serial.ToString();
        }
        else
        {
            serailNo = ViewState["SerialNo"].ToString();
        }
        if (txtProductcode.Text != "")
        {
            DataTable dt = ObjProductMaster.GetProductMaserTrueAllByProductCode(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtProductcode.Text, HttpContext.Current.Session["FinanceYearId"].ToString());
            dt = new DataView(dt, "IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                ProductId = dt.Rows[0]["ProductId"].ToString();
            }
            dt = null;
        }
        if (ddlUnit == null)
        {
            DisplayMessage("Unit Not Found");
            return;
        }
        else
        {
            UnitId = ddlUnit.SelectedValue.ToString();
        }
        if (hidProduct.Value == "" || hidProduct.Value == "0")
        {
            if (Session["Dtproduct"] == null)
            {
            }
            else
            {
                DtProduct = (DataTable)Session["Dtproduct"];
            }
            if (DtProduct.Rows.Count > 0)
            {
                DataTable Dt = new DataView(DtProduct, "", "SerialNo Desc", DataViewRowState.CurrentRows).ToTable();
                serialNo = float.Parse(Dt.Rows[0]["SerialNo"].ToString());
                serialNo += 1;
                Dt = null;
            }
            else
            {
                serialNo = 1;
            }
            if (Session["Dtproduct"] == null)
            {
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                dr["SerialNo"] = serialNo.ToString();
                dr["ProductId"] = ProductId.ToString();
                dr["UnitId"] = UnitId.ToString();
                dr["Quantity"] = txtRequestQty.Text.ToString();
                dr["UnitCost"] = txtUnitCost.Text.ToString();
                dr["ProductDescription"] = txtPDescription.Text;
                DtProduct.Rows.Add(dr);
                Session["Dtproduct"] = (DataTable)DtProduct;
            }
            else
            {
                DtProduct = (DataTable)Session["Dtproduct"];
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                dr["SerialNo"] = serialNo.ToString();
                dr["ProductId"] = ProductId.ToString();
                dr["UnitId"] = UnitId.ToString();
                dr["Quantity"] = txtRequestQty.Text.ToString();
                dr["UnitCost"] = txtUnitCost.Text.ToString();
                // dr["ProductDescription"] = txtPDescription.Text;
                DtProduct.Rows.Add(dr);
                Session["Dtproduct"] = (DataTable)DtProduct;
            }
        }
        else
        {
            serialNo = float.Parse(ViewState["SerialNo"].ToString());
            DataTable dt = (DataTable)Session["Dtproduct"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = DtProduct.NewRow();
                if (dt.Rows[i]["Trans_Id"].ToString() == hidProduct.Value)
                {
                    dr["Trans_Id"] = hidProduct.Value;
                    dr["SerialNo"] = serialNo.ToString();
                    dr["ProductId"] = ProductId.ToString();
                    dr["UnitId"] = UnitId.ToString();
                    dr["Quantity"] = txtRequestQty.Text.ToString();
                    dr["UnitCost"] = txtUnitCost.Text.ToString();
                    dr["ProductDescription"] = txtPDescription.Text;
                    DtProduct.Rows.Add(dr);
                }
                else
                {
                    dr["Trans_Id"] = dt.Rows[i]["Trans_Id"].ToString();
                    dr["SerialNo"] = dt.Rows[i]["SerialNo"].ToString();
                    dr["ProductId"] = dt.Rows[i]["ProductId"].ToString();
                    dr["UnitId"] = dt.Rows[i]["UnitId"].ToString();
                    dr["UnitCost"] = dt.Rows[i]["UnitCost"].ToString();
                    dr["Quantity"] = dt.Rows[i]["Quantity"].ToString();
                    //dr["ProductDescription"] = dt.Rows[i]["ProductDescription"].ToString();
                    DtProduct.Rows.Add(dr);
                }
            }
            dt = null;
        }
        Session["Dtproduct"] = (DataTable)DtProduct;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvProductRequest, DtProduct, "", "");
        //AllPageCode();
        ResetDetail();
        txtProductcode.Focus();
        getGridTotal();
        DtProduct = null;
    }
    protected void btnProductSaveold_Click(object sender, EventArgs e)
    {
        if (txtProductName.Text == "")
        {
            DisplayMessage("Enter Product Name");
            txtProductName.Text = "";
            txtProductName.Focus();
            return;
        }
        if (ddlUnit.SelectedIndex == 0)
        {
            DisplayMessage("Select Unit");
            ddlUnit.SelectedIndex = 0;
            ddlUnit.Focus();
            return;
        }
        if (txtUnitCost.Text == "")
        {
            DisplayMessage("Enter Unit Cost");
            txtUnitCost.Focus();
            return;
        }
        if (txtRequestQty.Text == "")
        {
            txtRequestQty.Text = "1";
        }
        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;
        if (editid.Value == "")
        {
            ReqId = ObjTrans.getAutoId();   // confusion
        }
        else
        {
            ReqId = editid.Value.ToString();
        }
        string serailNo = string.Empty;
        if (hidProduct.Value == "")
        {
            int Serial = gvProductRequest.Rows.Count;
            Serial = Serial + 1;
            serailNo = Serial.ToString();
        }
        else
        {
            serailNo = ViewState["SerialNo"].ToString();
        }
        if (txtProductName.Text != "")
        {
            DataTable dt = new DataView(ObjProductMaster.GetProductMasterAll(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "EProductName='" + txtProductName.Text.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                ProductId = dt.Rows[0]["ProductId"].ToString();
            }
            else
            {
                ProductId = "0";
            }
            dt = null;
        }
        if (ddlUnit.SelectedIndex == 0)
        {
        }
        else
        {
            UnitId = ddlUnit.SelectedValue.ToString();
        }
        if (hidProduct.Value == "")
        {
            OBjtransDetail.InsertTransferRequestDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), serailNo.ToString(), ProductId.ToString(), ReqId.ToString(), UnitId.ToString(), txtUnitCost.Text, txtRequestQty.Text.ToString(), "0", "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
        }
        else
        {
            OBjtransDetail.UpdateTransferRequestDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), hidProduct.Value.ToString(), ReqId, serailNo.ToString(), ProductId.ToString(), UnitId.ToString(), txtUnitCost.Text, txtRequestQty.Text.ToString(), "0", "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
            pnlProduct1.Visible = false;
        }
        fillgridDetail();
        ResetDetail();
    }
    public void getGridTotal()
    {
        float f = 0;
        foreach (GridViewRow gvrow in gvProductRequest.Rows)
        {
            try
            {
                f += float.Parse(((Label)gvrow.FindControl("lblReqQty")).Text);
            }
            catch
            {
                f += 0;
            }
        }
        try
        {
            ((Label)gvProductRequest.FooterRow.FindControl("txttotqtyShow")).Text = GetAmountDecimal(f.ToString());
        }
        catch
        {
        }
    }
    protected void btnEdit_Command1(object sender, CommandEventArgs e)
    {
        pnlProduct1.Visible = true;
        hidProduct.Value = e.CommandArgument.ToString();
        //DataTable dt = OBjtransDetail.GetTransferRequestDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), "0", e.CommandArgument.ToString());
        DataTable dtproduct = new DataTable();
        dtproduct = (DataTable)Session["Dtproduct"];
        DataTable dt = new DataView(dtproduct, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["ProductID"].ToString() != "0")
            {
                txtProductName.Text = ProductName(dt.Rows[0]["ProductID"].ToString());
                txtProductcode.Text = ProductCode(dt.Rows[0]["ProductID"].ToString());
                txtPDescription.Text = GetProductDescription(dt.Rows[0]["ProductID"].ToString());
            }
            else
            {
            }
            FillUnit(dt.Rows[0]["ProductID"].ToString());
            ddlUnit.SelectedValue = dt.Rows[0]["UnitId"].ToString();
            //txtPDescription.Text = dt.Rows[0]["ProductDescription"].ToString();
            txtRequestQty.Text = dt.Rows[0]["Quantity"].ToString();
            txtUnitCost.Text = dt.Rows[0]["UnitCost"].ToString();
            ViewState["SerialNo"] = dt.Rows[0]["SerialNo"].ToString();
        }
        txtProductcode.Focus();
        dtproduct = null;
    }
    protected void IbtnDelete_Command1(object sender, CommandEventArgs e)
    {
        DataTable dtproduct = (DataTable)Session["Dtproduct"];
        DataTable dt = new DataView(dtproduct, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvProductRequest, dt, "", "");
        Session["Dtproduct"] = (DataTable)dt;
        //AllPageCode();
        getGridTotal();
        dtproduct = null;
        dt = null;
    }
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        //btnClosePanel_Click(null, null);
        ResetDetail();
    }
    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = true;
        ResetDetail();
        txtProductcode.Focus();
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text != "")
        {
            DataTable dt = new DataTable();
            try
            {
                dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductName.Text.Trim().ToString());
            }
            catch
            {
            }
            if (dt == null)
            {
                DisplayMessage("Product Not Found");
                txtProductName.Text = "";
                txtProductcode.Text = "";
                txtPDescription.Text = "";
                txtProductName.Focus();
                ddlUnit.Items.Clear();
                return;
            }
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["ItemType"].ToString() == "NS")
                {
                    DisplayMessage("Product is non-stockable");
                    txtProductName.Text = "";
                    txtProductcode.Text = "";
                    txtPDescription.Text = "";
                    txtProductName.Focus();
                    ddlUnit.Items.Clear();
                    return;
                }
                if (Session["Dtproduct"] != null)
                {
                    DataTable Dt = (DataTable)Session["Dtproduct"];
                    DataTable dtProduct = new DataTable();
                    try
                    {
                        dtProduct = new DataView(Dt, "ProductId=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtProduct.Rows.Count > 0)
                    {
                        //DisplayMessage("Product already exists!");
                        //txtProductName.Text = "";
                        //txtProductName.Focus();
                        //txtProductName.Text = "";
                        //ddlUnit.Items.Clear();
                        //return;
                    }
                }
                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                FillUnit(dt.Rows[0]["ProductId"].ToString());
                txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                txtUnitCost.Focus();
            }
            else
            {
                DisplayMessage("Select Product");
                txtProductName.Text = "";
                txtProductcode.Text = "";
                txtPDescription.Text = "";
                txtProductName.Focus();
                ddlUnit.Items.Clear();
                return;
                //FillUnit();
            }
            dt = null;
        }
        else
        {
            DisplayMessage("Enter Product Name");
            ddlUnit.Items.Clear();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductcode.Text != "")
        {
            //DataTable dt = ObjProductMaster.GetProductMaserTrueAllByProductCode(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtProductcode.Text.ToString());
            DataTable dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductcode.Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["ItemType"].ToString() == "NS")
                {
                    DisplayMessage("Product is non-stockable");
                    txtProductName.Text = "";
                    txtProductcode.Text = "";
                    txtPDescription.Text = "";
                    txtProductcode.Focus();
                    ddlUnit.Items.Clear();
                    return;
                }
                if (Session["Dtproduct"] != null)
                {
                    DataTable Dt = (DataTable)Session["Dtproduct"];
                    DataTable dtProduct = new DataTable();
                    try
                    {
                        dtProduct = new DataView(Dt, "ProductId=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtProduct.Rows.Count > 0)
                    {
                        //DisplayMessage("Product already exists!");
                        //txtProductcode.Text = "";
                        //txtProductcode.Focus();
                        //txtProductName.Text = "";
                        //ddlUnit.Items.Clear();
                        //return;
                    }
                }
                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                FillUnit(dt.Rows[0]["ProductId"].ToString());
                txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                txtUnitCost.Focus();
                dt = null;
            }
            else
            {
                DisplayMessage("Select Product");
                ddlUnit.Items.Clear();
                txtProductName.Text = "";
                txtProductcode.Text = "";
                txtPDescription.Text = "";
                txtProductcode.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            ddlUnit.Items.Clear();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }
    }
    //Function to Check Whether there is Opening Stock for Particular Product or Not
    //created by Varsha Surana
    private bool CheckOpeningStockRow(string pid)
    {
        int st1;
        st1 = objStockDetail.GetProductOpeningStockStatus(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), pid);
        if (st1 > 0)
            return true;
        else
            return false;
    }
    //End Function
    protected void gvTransferRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Trans_Req"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Trans_Req"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvTransferRequest, dt, "", "");
        //AllPageCode();
        gvTransferRequest.HeaderRow.Focus();
        dt = null;
    }
    protected void gvTransferRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransferRequest.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Trans_Req"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvTransferRequest, dt, "", "");
        //AllPageCode();
        dt = null;
    }
    #endregion
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "11", "93", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Fillgrid();
        ////AllPageCode();
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        btnProductSave.Visible = clsPagePermission.bAdd;
        gvProductRequest.Columns[0].Visible = clsPagePermission.bEdit;
        gvProductRequest.Columns[1].Visible = clsPagePermission.bDelete;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();

        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    //protected void btnPostSave_Click(object sender, EventArgs e)
    //{
    //   // ChkPost.Checked = true;
    //    btnSave_Click(sender, e);
    //}
    #endregion
    #region Auto Complete Method
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
        dt = null;
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
        dt = null;
        return txt;
    }
    #endregion
    #region User defined Function
    public void ResetDetail()
    {
        txtProductName.Text = "";
        txtUnitCost.Text = "0";
        txtRequestQty.Text = "1";
        hidProduct.Value = "";
        txtPDescription.Text = "";
        txtProductcode.Text = "";
        txtProductcode.Focus();
        ddlUnit.Items.Clear();
    }
    private void FillUnit(string ProductId)
    {
        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());
    }
    public void fillgridDetail()
    {
        string ReqId = ObjTrans.getAutoId();
        if (editid.Value == "")
        {
            ReqId = ObjTrans.getAutoId();
        }
        else
        {
            ReqId = editid.Value.ToString();
        }
        DataTable dt = OBjtransDetail.GetTransferRequestDetailbyRequestId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ReqId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        Session["Dtproduct"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvProductRequest, dt, "", "");
        getGridTotal();

        if (dt.Rows[0]["SalesOrderNo"].ToString() == "")
        {
            ddlType.SelectedIndex = 0;
            gvProductRequest.Columns[0].Visible = true;
            gvProductRequest.Columns[3].Visible = false;
            div_Product.Visible = true;
            div_radio.Visible = true;
            pnlProduct1.Visible = true;
            Div_Btn_TransferFromOrder.Visible = false;
            Div_Grid_TransferFromOrder.Visible = false;
        }
        else
        {
            ddlType.SelectedIndex = 1;
            gvProductRequest.Columns[0].Visible = false;
            gvProductRequest.Columns[3].Visible = true;
            div_Product.Visible = false;
            div_radio.Visible = false;
            pnlProduct1.Visible = false;
            Div_Btn_TransferFromOrder.Visible = true;
            Div_Grid_TransferFromOrder.Visible = true;

        }
        dt = null;
        ////AllPageCode();
    }
    public string ProductName(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["EProductName"].ToString();
        }
        else
        {
            ProductName = "0";
        }
        dt = null;
        return ProductName;
    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["ProductCode"].ToString();
        }
        else
        {
            ProductName = "0";
        }
        dt = null;
        return ProductName;
    }
    public string GetProductDescription(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["Description"].ToString();
        }
        else
        {
            ProductName = "0";
        }
        dt = null;
        return ProductName;
    }
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = objUnit.GetUnitMasterById(Session["CompId"].ToString().ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        dt = null;
        return UnitName;
    }
    public void Fillgrid()
    {
        DataTable dt = ObjTrans.GetAllRecord_True(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), "0");
        if (ddlPosted.SelectedIndex != 0)
        {
            dt = new DataView(dt, "RequestStatus='" + ddlPosted.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvTransferRequest, dt, "", "");
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count;
        Session["DtTransferRequest"] = dt;
        Session["dtFilter_Trans_Req"] = dt;
        dt = null;
        ////AllPageCode();
    }
    public void Reset()
    {
        txtTermCondition.Text = "";
        //ChkPost.Checked = false;
        //FillUnit();
        txtLocationName.Text = "";
        txtRequestdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        ResetDetail();
        txtlRequestNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = GetDocumentNumber();
        gvProductRequest.DataSource = null;
        gvProductRequest.DataBind();
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ViewState["DepartmentApproval"] = null;
        txtlRequestNo.Text = GetDocumentNumber();
        Session["Dtproduct"] = null;
        rbtnFormView.Visible = true;
        rbtnAdvancesearchView.Visible = true;
        rbtnFormView.Checked = true;
        rbtnFormView_OnCheckedChanged(null, null);
        hdnJobId.Value = "0";
        chkReopen.Visible = false;
        chkReopen.Checked = false;
        gvOrderData.DataSource = null;
        gvOrderData.DataBind();
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
        dtres = null;
        return ArebicMessage;
    }
    #endregion
    #region Bin Section
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinFieldName.SelectedItem.Value == "TDate")
        {
            if (txtbinValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtbinValueDate.Text);
                    txtbinValue.Text = Convert.ToDateTime(txtbinValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtbinValueDate.Text = "";
                    txtbinValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtbinValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtbinValueDate.Focus();
                txtbinValue.Text = "";
                return;
            }
        }
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["DtBinTransferRequest"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvBinTransferRequest, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                ////AllPageCode();
            }
            dtCust = null;
            //btnbinbind.Focus();
        }
        if (txtbinValue.Text != "")
            txtbinValue.Focus();
        else if (txtbinValueDate.Text != "")
            txtbinValueDate.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        ddlbinOption.SelectedIndex = 3;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        txtbinValueDate.Text = "";
        txtbinValueDate.Visible = false;
        txtbinValue.Visible = true;
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void gvBinTransferRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBinTransferRequest.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvBinTransferRequest, dt, "", "");
            dt = null;
        }
        ////AllPageCode();
        string temp = string.Empty;
        bool isselcted;
        for (int i = 0; i < gvBinTransferRequest.Rows.Count; i++)
        {
            Label lblconid = (Label)gvBinTransferRequest.Rows[i].FindControl("lblReqId");
            string[] split = lblSelectedRecord.Text.Split(',');
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvBinTransferRequest.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
        gvBinTransferRequest.BottomPagerRow.Focus();
    }
    protected void gvBinTransferRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtBinFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvBinTransferRequest, dt, "", "");
        ////AllPageCode();
        gvBinTransferRequest.HeaderRow.Focus();
        dt = null;
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvBinTransferRequest.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvBinTransferRequest.Rows.Count; i++)
        {
            ((CheckBox)gvBinTransferRequest.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvBinTransferRequest.Rows[i].FindControl("lblReqId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvBinTransferRequest.Rows[i].FindControl("lblReqId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvBinTransferRequest.Rows[i].FindControl("lblReqId"))).Text.Trim().ToString())
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
        ((CheckBox)gvBinTransferRequest.HeaderRow.FindControl("chkgvSelectAll")).Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvBinTransferRequest.Rows[index].FindControl("lblReqId");
        if (((CheckBox)gvBinTransferRequest.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
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
        ((CheckBox)gvBinTransferRequest.Rows[index].FindControl("chkgvSelect")).Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPr = (DataTable)Session["dtBinFilter"];
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPr.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < gvBinTransferRequest.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvBinTransferRequest.Rows[i].FindControl("lblReqId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvBinTransferRequest.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtPr1 = (DataTable)Session["dtBinFilter"];
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvBinTransferRequest, dtPr1, "", "");
            ////AllPageCode();
            ViewState["Select"] = null;
        }
        dtPr = null;
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        bool Result = true;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(ObjTrans.GetRecordUsingTransId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString()).Rows[0]["TDate"].ToString()), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                    {
                        Result = false;
                        break;
                    }
                }
            }
        }
        if (!Result)
        {
            DisplayMessage("You can not restore closed financial year record");
            return;
        }
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = ObjTrans.DeleteTransferRequestHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
                }
            }
        }
        if (b != 0)
        {
            Fillgrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activated");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvBinTransferRequest.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
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
        txtbinValue.Focus();
    }
    public void FillGridBin()
    {
        DataTable dt = ObjTrans.GetAllRecord_False(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), "0");
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvBinTransferRequest, dt, "", "");
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count;
        Session["DtBinTransferRequest"] = dt;
        Session["DtBinFilter"] = dt;
        if (dt.Rows.Count != 0)
        {
            ////AllPageCode();
        }
        else
        {
            imgBtnRestore.Visible = false;
        }
        dt = null;
    }
    #endregion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListLocationName(string prefixText, int count, string contextKey)
    {
        LocationMaster objLocationMaster = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objLocationMaster.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString()), "Location_Name like '%" + prefixText.ToString() + "%'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();
        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id<>'" + HttpContext.Current.Session["LocId"].ToString() + "'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            //dt=objLocationMaster.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());
            //       dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
        }
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Location_Name"].ToString();
        }
        dt = null;
        return txt;
    }
    protected void txtLocationName_TextChanged(object sender, EventArgs e)
    {
        if (txtLocationName.Text != "")
        {
            DataTable Dtlocation = objLocation.GetLocationMasterByLocationName(Session["CompId"].ToString(), txtLocationName.Text);
            if (Dtlocation.Rows.Count == 0)
            {
                DisplayMessage("Select Location");
                txtLocationName.Focus();
                txtLocationName.Text = "";
                return;
            }
            else
            {
                //ChkPost.Focus();
            }
            Dtlocation = null;
        }
        else
        {
        }
    }
   
    public string GetStatus(string status)
    {
        string Result = string.Empty;
        if (status.Trim() == "0")
        {
            Result = "Not Open";
        }
        if (status.Trim() == "1")
        {
            Result = "Processing";
        }
        if (status.Trim() == "2")
        {
            Result = "Transfer Out";
        }
        return Result;
    }
    public string SetDateFormat(string Date)
    {
        string DateFormat = string.Empty;
        DateFormat = Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        return DateFormat;
    }
    #region printreport
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/TransferRequestPrint.aspx?TransId=" + e.CommandArgument.ToString() + "','window','width=1024');", true);
    }
    #endregion
    #region Advance Search
    protected void btnAddProductScreen_Click(object sender, EventArgs e)
    {
        Session["DtSearchProduct"] = Session["Dtproduct"];
        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=TR','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        if (Session["DtSearchProduct"] != null)
        {
            Session["Dtproduct"] = Session["DtSearchProduct"];
            if (Session["Dtproduct"] != null)
            {
                //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvProductRequest, (DataTable)Session["Dtproduct"], "", "");
            }
            Session["DtSearchProduct"] = null;
            ////AllPageCode();
            getGridTotal();
        }
        else
        {
            if (Session["Dtproduct"] != null)
            {
                //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvProductRequest, (DataTable)Session["Dtproduct"], "", "");
            }
            getGridTotal();
            ////AllPageCode();
            DisplayMessage("Product Not Found");
            return;
        }
    }
    protected void rbtnFormView_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnFormView.Checked == true)
        {
            btnAddProduct.Visible = true;
            btnAddProductScreen.Visible = false;
            btnAddtoList.Visible = false;
        }
        if (rbtnAdvancesearchView.Checked == true)
        {
            btnAddProduct.Visible = false;
            btnAddProductScreen.Visible = true;
            btnAddtoList.Visible = true;
        }
    }
    #endregion
    #region Request
    public void FillRequestGrid()
    {
        DataTable dtPInquiry = objProductionProcess.GetRecord_For_TransferRequest(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        lblRequestTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtPInquiry.Rows.Count + "";
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
        lblRequestTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtPInquiry.Rows.Count.ToString() + "";
        dtPInquiry = null;
        ////AllPageCode();
    }
    protected void ddlRequestFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        if (ddlRequestFieldName.SelectedItem.Value == "Job_Creation_Date" || ddlRequestFieldName.SelectedItem.Value == "Request_Date")
        {
            txtValueDate.Visible = true;
            txtRequestvalue.Visible = false;
            txtRequestvalue.Text = "";
            txtValueDate.Text = "";
        }
        else
        {
            txtValueDate.Visible = false;
            txtRequestvalue.Visible = true;
            txtRequestvalue.Text = "";
            txtValueDate.Text = "";
        }
        ddlRequestFieldName.Focus();
    }
    protected void btnrequestbind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        if (ddlRequestFieldName.SelectedItem.Value == "Job_Creation_Date" || ddlRequestFieldName.SelectedItem.Value == "Request_Date")
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueDate.Text);
                    txtRequestvalue.Text = Convert.ToDateTime(txtValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDate.Text = "";
                    txtRequestvalue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDate.Focus();
                txtRequestvalue.Text = "";
                return;
            }
        }
        if (ddlrequestoption.SelectedIndex != 0 && txtRequestvalue.Text.Trim()!="")
        {
            string condition = string.Empty;
            if (ddlrequestoption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlrequestoption.SelectedValue + ",System.String)='" + txtRequestvalue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlrequestoption.SelectedValue + ",System.String) like '%" + txtRequestvalue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlrequestoption.SelectedValue + ",System.String) Like '" + txtRequestvalue.Text.Trim() + "%'";
            }
            DataTable dtPurchaseInquiry = (DataTable)Session["dtPInquiry"];
            DataView view = new DataView(dtPurchaseInquiry, condition, "", DataViewRowState.CurrentRows);
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductionProcess, view.ToTable(), "", "");
            Session["dtPInquiry"] = view.ToTable();
            lblRequestTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
            //    btnrequestrefresh.Focus();
            dtPurchaseInquiry = null;
        }
        if (txtRequestvalue.Text != "")
            txtRequestvalue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void btnrequestrefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        FillRequestGrid();
        ddlRequestFieldName.SelectedIndex = 0;
        ddlrequestoption.SelectedIndex = 2;
        txtRequestvalue.Text = "";
        txtRequestvalue.Visible = true;
        txtValueDate.Visible = false;
        txtValueDate.Text = "";
        txtRequestvalue.Focus();
    }
    protected void GvProductionProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvProductionProcess.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtPInquiry"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcess, dt, "", "");
        dt = null;
        //AllPageCode();
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
        dt = null;
        //AllPageCode();
    }
    protected void btnPREdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow GVrow = (GridViewRow)((Button)sender).Parent.Parent;
        editid.Value = "";
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("Trans_Id");
        DtProduct.Columns.Add("SerialNo");
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("Quantity");
        DtProduct.Columns.Add("UnitCost");
        DtProduct.Columns.Add("ProductDescription");
        DtProduct.Columns.Add("SalesOrderNo");
        DtProduct.Columns.Add("SalesOrderID");
        DataTable dtBom = objProductionBom.GetRecord_By_RefJobNo(e.CommandArgument.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        foreach (DataRow dr in dtBom.Rows)
        {
            DataRow dr1 = DtProduct.NewRow();
            dr1[0] = dr["Id"].ToString();
            dr1[1] = dr["S_No"].ToString();
            dr1[2] = dr["Item_Id"].ToString();
            dr1[3] = dr["Unit_Id"].ToString();
            dr1[4] = dr["Req_Qty"].ToString();
            dr1[5] = dr["Unit_Price"].ToString();
            dr1[6] = "";
            DtProduct.Rows.Add(dr1);
        }
        Session["Dtproduct"] = DtProduct;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvProductRequest, DtProduct, "", "");
        txtTermCondition.Text = "Production Job No : " + e.CommandName;
        txtLocationName.Text = objLocation.GetLocationMasterById(Session["CompId"].ToString(), ((Label)GVrow.FindControl("lblrequestlocation")).Text).Rows[0]["Location_Name"].ToString();
        hdnJobId.Value = e.CommandArgument.ToString();
        rbtnFormView.Visible = false;
        rbtnAdvancesearchView.Visible = false;
        btnAddProduct.Visible = false;
        btnAddProductScreen.Visible = false;
        btnAddtoList.Visible = false;
        //AllPageCode();
        getGridTotal();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active_Request()", true);
        DtProduct = null;
    }
    #endregion
    public string GetProductStock(string strProductId, string strType)
    {
        string SysQty = string.Empty;
        if (strType == "1")
        {
            try
            {
                SysQty = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), objLocation.GetLocationMasterByLocationName(Session["CompId"].ToString(), txtLocationName.Text).Rows[0]["Location_Id"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), strProductId).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                SysQty = "0";
            }
        }
        else
        {
            try
            {
                SysQty = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), strProductId).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                SysQty = "0";
            }
        }
        return GetAmountDecimal(SysQty);
    }
    public string GetAmountDecimal(string Amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Amount);
    }
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "TDate")
        {
            txtValueRequestdate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueRequestdate.Text = "";
        }
        else
        {
            txtValueRequestdate.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            txtValueRequestdate.Text = "";
        }
    }
    protected void ddlbinFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinFieldName.SelectedItem.Value == "TDate")
        {
            txtbinValueDate.Visible = true;
            txtbinValue.Visible = false;
            txtbinValue.Text = "";
            txtbinValueDate.Text = "";
        }
        else
        {
            txtbinValueDate.Visible = false;
            txtbinValue.Visible = true;
            txtbinValue.Text = "";
            txtbinValueDate.Text = "";
        }
    }
    #endregion
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_Product.Visible = false;
        div_radio.Visible = false;
        pnlProduct1.Visible = false;
        Div_Btn_TransferFromOrder.Visible = true;
        Div_Grid_TransferFromOrder.Visible = true;
        gvProductRequest.Columns[0].Visible = false;
        gvProductRequest.Columns[3].Visible = true;
        gvOrderData.DataSource = null;
        gvOrderData.DataBind();
        gvProductRequest.DataSource = null;
        gvProductRequest.DataBind();
        Session["Dtproduct"] = null;
        if (ddlType.SelectedValue == "Direct")
        {
            div_Product.Visible = true;
            div_radio.Visible = true;
            pnlProduct1.Visible = true;
            Div_Btn_TransferFromOrder.Visible = false;
            Div_Grid_TransferFromOrder.Visible = false;
            gvProductRequest.Columns[0].Visible = true;
            gvProductRequest.Columns[3].Visible = false;
        }
    }
    protected void btnGenerateOrder_Click(object sender, EventArgs e)
    {
        if (txtLocationName.Text == "")
        {
            DisplayMessage("Enter Request Location Name");
            txtLocationName.Focus();
            return ;
        }
        string ReqLocationId = objLocation.getLocationIDFromName(txtLocationName.Text);
        using (DataTable dt = ObjSOrderDetail.getSODetailDataForTransferRequest(Session["compid"].ToString(), Session["brandid"].ToString(), Session["locid"].ToString(), ReqLocationId, Session["FinanceYearId"].ToString()))
        {
            gvOrderData.DataSource = dt;
            gvOrderData.DataBind();
        }
    }
    public bool btnProductSave_Click(string orderID, string productCode, string requestQty, string orderNo, string productId, string unitPrice, string remove = "")
    {
        if (txtLocationName.Text == "")
        {
            DisplayMessage("Enter Request Location Name");
            txtLocationName.Focus();
            return false;
        }
        DataTable DtProduct = new DataTable();
        if (remove == "False")
        {
            DtProduct = (DataTable)Session["Dtproduct"];
            for (int i = 0; i < DtProduct.Rows.Count; i++)
            {
                if (DtProduct.Rows[i]["salesorderId"].ToString() == orderID && DtProduct.Rows[i]["productID"].ToString() == productId)
                {
                    DtProduct.Rows.RemoveAt(i);
                }
            }
            Session["Dtproduct"] = (DataTable)DtProduct;
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvProductRequest, DtProduct, "", "");
            getGridTotal();
            return true;
        }
        DtProduct.Columns.Add("Trans_Id", typeof(float));
        DtProduct.Columns.Add("SerialNo", typeof(float));
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("Quantity");
        DtProduct.Columns.Add("UnitCost");
        DtProduct.Columns.Add("ProductDescription");
        DtProduct.Columns.Add("SalesOrderNo");
        DtProduct.Columns.Add("SalesOrderID");
        if (productCode == "")
        {
            DisplayMessage("Cant find product code of selected product");
            return false;
        }
        string productName = ObjProductMaster.GetProductNamebyProductCode(productCode);
        if (productName == "")
        {
            DisplayMessage("Cant find product name of selected product");
            return false;
        }
        if (requestQty == "")
        {
            requestQty = "1";
        }
        string ReqId = string.Empty;
        string ProductId = productId;
        string UnitId = objUnit.GetUnitIdByProductCode(productCode);
        if (editid.Value == "")
        {
            ReqId = ObjTrans.getAutoId();
        }
        else
        {
            ReqId = editid.Value.ToString();
        }
        float serialNo = 0;
        string serailNo = string.Empty;
        if (hidProduct.Value == "")
        {
            int Serial = gvProductRequest.Rows.Count;
            Serial = Serial + 1;
            serailNo = Serial.ToString();
        }
        else
        {
            serailNo = ViewState["SerialNo"].ToString();
        }
        if (productCode != "")
        {
            DataTable dt = ObjProductMaster.GetProductMaserTrueAllByProductCode(Session["CompId"].ToString(), Session["BrandId"].ToString(), productCode, HttpContext.Current.Session["FinanceYearId"].ToString());
            dt = new DataView(dt, "IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                ProductId = dt.Rows[0]["ProductId"].ToString();
            }
        }
        if (hidProduct.Value == "" || hidProduct.Value == "0")
        {
            if (Session["Dtproduct"] == null)
            {
            }
            else
            {
                DtProduct = (DataTable)Session["Dtproduct"];
            }
            if (DtProduct.Rows.Count > 0)
            {
                DataTable Dt = new DataView(DtProduct, "", "SerialNo Desc", DataViewRowState.CurrentRows).ToTable();
                serialNo = float.Parse(Dt.Rows[0]["SerialNo"].ToString());
                serialNo += 1;
            }
            else
            {
                serialNo = 1;
            }
            if (Session["Dtproduct"] == null)
            {
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                dr["SerialNo"] = serialNo.ToString();
                dr["ProductId"] = ProductId.ToString();
                dr["UnitId"] = UnitId.ToString();
                dr["Quantity"] = requestQty;
                dr["UnitCost"] = unitPrice;
                dr["ProductDescription"] = "";
                dr["SalesOrderNo"] = orderNo;
                dr["SalesOrderID"] = orderID;
                DtProduct.Rows.Add(dr);
                Session["Dtproduct"] = (DataTable)DtProduct;
            }
            else
            {
                DtProduct = (DataTable)Session["Dtproduct"];
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                dr["SerialNo"] = serialNo.ToString();
                dr["ProductId"] = ProductId.ToString();
                dr["UnitId"] = UnitId.ToString();
                dr["Quantity"] = requestQty;
                dr["UnitCost"] = unitPrice;
                dr["SalesOrderNo"] = orderNo;
                dr["SalesOrderID"] = orderID;
                // dr["ProductDescription"] = txtPDescription.Text;
                DtProduct.Rows.Add(dr);
                Session["Dtproduct"] = (DataTable)DtProduct;
            }
        }
        else
        {
            serialNo = float.Parse(ViewState["SerialNo"].ToString());
            DataTable dt = (DataTable)Session["Dtproduct"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = DtProduct.NewRow();
                if (dt.Rows[i]["Trans_Id"].ToString() == hidProduct.Value)
                {
                    dr["Trans_Id"] = hidProduct.Value;
                    dr["SerialNo"] = serialNo.ToString();
                    dr["ProductId"] = ProductId.ToString();
                    dr["UnitId"] = UnitId.ToString();
                    dr["Quantity"] = requestQty;
                    dr["UnitCost"] = unitPrice;
                    dr["ProductDescription"] = "";
                    dr["SalesOrderNo"] = orderNo;
                    dr["SalesOrderID"] = orderID;
                    DtProduct.Rows.Add(dr);
                }
                else
                {
                    dr["Trans_Id"] = dt.Rows[i]["Trans_Id"].ToString();
                    dr["SerialNo"] = dt.Rows[i]["SerialNo"].ToString();
                    dr["ProductId"] = dt.Rows[i]["ProductId"].ToString();
                    dr["UnitId"] = dt.Rows[i]["UnitId"].ToString();
                    dr["UnitCost"] = dt.Rows[i]["UnitCost"].ToString();
                    dr["Quantity"] = dt.Rows[i]["Quantity"].ToString();
                    dr["SalesOrderNo"] = dt.Rows[i]["orderno"].ToString();
                    dr["SalesOrderID"] = dt.Rows[i]["orderID"].ToString();
                    //dr["ProductDescription"] = dt.Rows[i]["ProductDescription"].ToString();
                    DtProduct.Rows.Add(dr);
                }
            }
        }
        Session["Dtproduct"] = (DataTable)DtProduct;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvProductRequest, DtProduct, "", "");
        getGridTotal();
        return true;
    }
    protected void chkAddSOForTR_CheckedChanged(object sender, EventArgs e)
    {
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        CheckBox chk = (CheckBox)sender;
        Label lblOrderNo = (Label)gvOrderData.Rows[index].FindControl("lblOrderNo");
        Label lblProductCode = (Label)gvOrderData.Rows[index].FindControl("lblProductCode");
        Label lblQuantity = (Label)gvOrderData.Rows[index].FindControl("lblQuantity");
        HiddenField hdnProductId = (HiddenField)gvOrderData.Rows[index].FindControl("gvhdnProductId");
        HiddenField hdnUnitCost = (HiddenField)gvOrderData.Rows[index].FindControl("gvHdnUnitCost");
        HiddenField gvOrderId = (HiddenField)gvOrderData.Rows[index].FindControl("gvOrderId");
        if (!btnProductSave_Click(gvOrderId.Value, lblProductCode.Text, lblQuantity.Text, lblOrderNo.Text, hdnProductId.Value, hdnUnitCost.Value, chk.Checked.ToString()))
        {
            chk.Checked = false;
        }
    }
}