using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Inventory_TransferIn : BasePage
{
    Inv_UnitMaster objUnit = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_TransferRequestHeader ObjTransReq = null;
    Inv_TransferRequestDetail ObjTransferReqDetail = null;
    Inv_TransferHeader ObjTransferHeader = null;
    Inv_TransferDetail ObjTransferDetail = null;
    LocationMaster objLocation = null;
    SystemParameter ObjSysParam = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_ProductLedger ObjProductledger = null;
    Common cmn = null;
    DataAccessClass objDa = null;
    Inv_ParameterMaster objInvParam = null;
    PageControlCommon objPageCmn = null;
    string strCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocId = string.Empty;
    string UserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }


        StrBrandId = Session["BrandId"].ToString();
        StrLocId = Session["LocId"].ToString();
        strCompId = Session["CompId"].ToString();

        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjTransReq = new Inv_TransferRequestHeader(Session["DBConnection"].ToString());
        ObjTransferReqDetail = new Inv_TransferRequestDetail(Session["DBConnection"].ToString());
        ObjTransferHeader = new Inv_TransferHeader(Session["DBConnection"].ToString());
        ObjTransferDetail = new Inv_TransferDetail(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjProductledger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        //btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
        //btnPostSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPostSave, "").ToString());
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "118", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            txtTransferDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            Calender.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            FillGrid();
            Lbl_Tab_New.Enabled = false;
            btnList_Click(null, null);

            DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTransferInScanning");
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    pnlScanProduct.Visible = true;
                }
            }
        }
        AllPageCode();
    }
    protected void btnPostSave_Click(object sender, EventArgs e)
    {
        if (Inventory_Common.GetPhyscialInventoryStatus(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Physical Inventory work is under processing ,you can not post.");
            return;
        }



        foreach (GridViewRow gvrow in gvEditProduct.Rows)
        {
            TextBox txt = (TextBox)gvrow.FindControl("txtInQty");
            Label lb = (Label)gvrow.FindControl("lbloutqty");
            if (txt.Text == "")
            {
                txt.Text = "0.000";
            }
            if (lb.Text == "")
            {
                lb.Text = "0.000";
            }
            if (float.Parse(txt.Text.ToString()) > float.Parse(lb.Text.ToString()))
            {
                DisplayMessage("In quantity should not be greater than Out quantity");
                txt.Text = "0";
                txt.Focus();
                return;
            }

            if (float.Parse(txt.Text.ToString()) != float.Parse(lb.Text.ToString()))
            {
                DisplayMessage("In quantity should  be equal to Out quantity");
                txt.Text = "0";
                txt.Focus();
                return;
            }
        }
        btnSave_Click(sender, e);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        DataTable dTranfer = ObjTransferHeader.GetTransferHeaderForTransferIn(strCompId, StrBrandId, StrLocId);

        dTranfer = new DataView(dTranfer, "Trans_Id=" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dTranfer.Rows.Count != 0)
        {
            if (Common.DtPhysical != null)
            {
                DataTable dt = new DataTable();
                dt = Common.DtPhysical;
                if (dt.Rows.Count > 0)
                {
                    dt = new DataView(dt, "CompanyId='" + Session["CompId"].ToString() + "' and BrandId='" + Session["BrandId"].ToString() + "' and LocationId='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    string CurrentLocation = objLocation.GetLocationMasterById(strCompId, Session["LocId"].ToString()).Rows[0]["Location_Name"].ToString();
                    txtLocationName.Text = objLocation.GetLocationMasterById(strCompId, dTranfer.Rows[0]["FromLocationId"].ToString()).Rows[0]["Location_Name"].ToString();
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["TransType"].ToString() == "1")
                        {
                            DisplayMessage("Do not transaction ,Stock Work is going On in " + CurrentLocation + " Location");
                            return;
                        }
                    }
                    DataTable dtFromLocation = Common.DtPhysical;
                    dtFromLocation = new DataView(dtFromLocation, "CompanyId='" + Session["CompId"].ToString() + "' and BrandId='" + Session["BrandId"].ToString() + "' and LocationId='" + dTranfer.Rows[0]["FromLocationId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtFromLocation.Rows.Count > 0)
                    {
                        if (dtFromLocation.Rows[0]["TransType"].ToString() == "1")
                        {
                            DisplayMessage("Do not transaction ,Stock Work is going On in " + txtLocationName.Text + " Location");
                            return;
                        }
                    }
                }
            }
        }

        if (txtTransferDate.Text == "")
        {
            DisplayMessage("Enter Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTransferDate);
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtTransferDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Start Date in format " + Session["DateFormat"].ToString() + "");
                txtTransferDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTransferDate);
                return;
            }
        }


        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtTransferDate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }




        //here we checking post status
        //this line is updated by jitendra upadhyay was coming on server on 28-07-2016

        string Post = ((Button)sender).ID.Trim() == "btnPostSave" ? "Y" : "N";


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {

            ObjTransferHeader.UpdateTransferHeaderForTransferIn(strCompId, StrBrandId, StrLocId, editid.Value, ObjSysParam.getDateForInput(txtTransferDate.Text).ToString(), Post, Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            DataTable dtTransHeader = ObjTransferHeader.GetTransferHeaderForTransferIn(strCompId, StrBrandId, StrLocId, editid.Value, ref trns);



            //here we get exchnage rate for convert product cost in location currenty

            string strFromCurrencyId = string.Empty;
            string strToCurrencyId = string.Empty;

            strFromCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), dtTransHeader.Rows[0]["FromLocationId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
            strToCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();


            string ExchnageRate = SystemParameter.GetExchageRate(strFromCurrencyId, strToCurrencyId,Session["DBConnection"].ToString());

            if (ExchnageRate == "")
            {
                ExchnageRate = "1";
            }



            string TransId = editid.Value;
            foreach (GridViewRow Row in gvEditProduct.Rows)
            {
                string strItemType = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString();


                if (((TextBox)Row.FindControl("txtInQty")).Text == "")
                {
                    ((TextBox)Row.FindControl("txtInQty")).Text = "0";
                }

                ObjTransferDetail.UpdateTransferDetailForTransferIn(strCompId, StrBrandId, StrLocId, ((Label)Row.FindControl("lblTransId")).Text.Trim(), ((TextBox)Row.FindControl("txtInqty")).Text.Trim(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                //updated by jitendra upadhyay to insert the serial number in stock batch master table
                //updated on 24-12-2013
                ObjStockBatchMaster.DeleteStockBatchMaster(strCompId, StrBrandId, StrLocId, "TI", TransId, ((Label)Row.FindControl("lblPId")).Text.Trim(), ref trns);
                ObjStockBatchMaster.DeleteStockBatchMaster(strCompId, StrBrandId, ViewState["FromLocationId"].ToString(), "TO", TransId, ((Label)Row.FindControl("lblPId")).Text.Trim(), ref trns);

                int LedgerRefId = 0;
                if (Post.Trim() == "Y")
                {



                    if (((Label)Row.FindControl("lblunitcost")).Text == "")
                    {
                        ((Label)Row.FindControl("lblunitcost")).Text = "0";
                    }

                    string CostPrice = SetDecimal((Convert.ToDouble(((Label)Row.FindControl("lblunitcost")).Text) * Convert.ToDouble(ExchnageRate)).ToString());

                    LedgerRefId = ObjProductledger.InsertProductLedger(strCompId.ToString(), StrBrandId.ToString(), dtTransHeader.Rows[0]["FromLocationId"].ToString(), "TO", editid.Value.ToString(), StrLocId, ((Label)Row.FindControl("lblPId")).Text.Trim(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), "O", "0", "0", "0", ((Label)Row.FindControl("lbloutqty")).Text.Trim(), "1/1/1800", ((Label)Row.FindControl("lblunitcost")).Text, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(txtTransferDate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);

                    ObjProductledger.InsertProductLedger(strCompId.ToString(), StrBrandId.ToString(), StrLocId.ToString(), "TI", editid.Value.ToString(), dtTransHeader.Rows[0]["FromLocationId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), "I", "0", "0", ((TextBox)Row.FindControl("txtInQty")).Text.Trim(), "0", "1/1/1800", CostPrice, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(txtTransferDate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                }

                //if (strItemType == "FE" || strItemType == "LE" || strItemType == "FM" || strItemType == "LM")
                //{
                //    UpdateRecordForStckableItem(((Label)Row.FindControl("lblPId")).Text.Trim(), ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), ref trns).Rows[0]["MaintainStock"].ToString(), float.Parse(((TextBox)Row.FindControl("txtInqty")).Text), TransId, ((Label)Row.FindControl("lblUnitId")).Text.Trim(), LedgerRefId.ToString(), ViewState["FromLocationId"].ToString(), ref trns);

                //}

                if (Post.Trim() == "Y")
                {
                    if (dtTransHeader.Rows[0]["RequestNo"].ToString() != "0")
                    {
                        ObjTransferReqDetail.UpdateTransferRequestDetailForTransferIn(strCompId, StrBrandId, StrLocId, dtTransHeader.Rows[0]["RequestNo"].ToString(), ((TextBox)Row.FindControl("txtInqty")).Text.Trim(), ((Label)Row.FindControl("lblPId")).Text.Trim(), UserId.ToString(), DateTime.Now.ToString(), ref trns);

                        //here we update receieve quantity in bom table 
                        //if request from production
                        //code start
                        if (ObjTransReq.GetRecordUsingTransId(strCompId, StrBrandId, Session["LOcId"].ToString(), dtTransHeader.Rows[0]["RequestNo"].ToString(), ref trns).Rows[0]["Field3"].ToString() != "0")
                        {
                            string sql = string.Empty;
                            sql = "update Inv_Production_BOM set rec_qty=" + ((TextBox)Row.FindControl("txtInQty")).Text.Trim() + " where Ref_Job_No=" + ObjTransReq.GetRecordUsingTransId(strCompId, StrBrandId, Session["LOcId"].ToString(), dtTransHeader.Rows[0]["RequestNo"].ToString()).Rows[0]["Field3"].ToString() + " and Item_Id=" + ((Label)Row.FindControl("lblPId")).Text.Trim() + "";
                            objDa.execute_Command(sql, ref trns);
                        }
                        //code end
                    }
                }
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            DisplayMessage("Record Saved","green");
            btnList_Click(null, null);
            Reset();
            ViewState["RequestId"] = null;
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_New()", true);
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
        txtVoucherNo.Text = "";
        editid.Value = "";
        txtTransferDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtTransNo.Text = "";
        txtTransReqDate.Text = "";
        txtLocationName.Text = "";
        pnlTrans.Visible = false;
        FillGrid();
        editid.Value = "";
        gvEditProduct.DataSource = null;
        gvEditProduct.DataBind();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        chkPost.Checked = false;
        AllPageCode();
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_New()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Focus();
        FillGrid();
    }

    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "TDate" || ddlFieldName.SelectedItem.Value == "Request_Date" || ddlFieldName.SelectedItem.Value == "OutDate")
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
            DataTable dtCust = (DataTable)ViewState["dtTransfer"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Trans_In"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)GvTransfer, view.ToTable(), "", "");

            AllPageCode();

            //btnbind.Focus();

        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        FillGrid();
        txtValue.Focus();
    }
  
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = objUnit.GetUnitMasterById(strCompId.ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
    }
    public string ProductName(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = ObjProductMaster.GetProductMasterById(strCompId.ToString(), StrBrandId, ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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

        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), StrBrandId, ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
    protected void GvTransfer_Sorting(object sender, GridViewSortEventArgs e)
    {
        hdnSort.Value = hdnSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Trans_In"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + hdnSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Trans_In"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)GvTransfer, dt, "", "");

        AllPageCode();
        GvTransfer.HeaderRow.Focus();
    }

    protected void GvTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvTransfer.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Trans_In"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)GvTransfer, dt, "", "");

        AllPageCode();
    }
    public void FillGrid()
    {
        DataTable dt = ObjTransferHeader.GetTransferHeaderForTransferIn(strCompId, StrBrandId, StrLocId);

        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            PostStatus = " Post='Y'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            PostStatus = " Post='N'";
        }

        dt = new DataView(dt, PostStatus, "", DataViewRowState.CurrentRows).ToTable(); ;
        dt.Columns.Add("Request_No");
        dt.Columns.Add("Request_Date");


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            try
            {
                DataTable dtRequest = ObjTransReq.GetRecordUsingTransId(strCompId, StrBrandId, dt.Rows[i]["ToLocationId"].ToString(), dt.Rows[i]["RequestNo"].ToString());
                dt.Rows[i]["Request_No"] = dtRequest.Rows[0]["RequestNo"].ToString();
                dt.Rows[i]["Request_Date"] = dtRequest.Rows[0]["TDate"].ToString();
            }
            catch
            {

            }
        }
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)GvTransfer, dt, "", "");

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count;
        ViewState["dtTransfer"] = dt;
        Session["dtFilter_Trans_In"] = dt;
        AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString().Trim() == "Y")
        {
            DisplayMessage("Voucher Posted , you can not edit !");
            return;
        }

        DataTable dTranfer = ObjTransferHeader.GetTransferHeaderForTransferIn(strCompId, StrBrandId, StrLocId);

        dTranfer = new DataView(dTranfer, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dTranfer.Rows.Count != 0)
        {

            editid.Value = e.CommandArgument.ToString();

            //added by jitendra upadhyay to retreive the serial number in editable mode
            //added on 24-12-2013
            DataTable dtSerial = new DataTable();
            dtSerial.Columns.Add("Product_Id");
            dtSerial.Columns.Add("SerialNo");
            dtSerial.Columns.Add("SOrderNo");
            dtSerial.Columns.Add("BarcodeNo");
            dtSerial.Columns.Add("BatchNo");
            dtSerial.Columns.Add("LotNo");
            dtSerial.Columns.Add("ExpiryDate");
            dtSerial.Columns.Add("ManufacturerDate");


            DataTable dtStock = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(strCompId, StrBrandId, StrLocId, "TI", editid.Value);
            if (dtStock.Rows.Count > 0)
            {

                for (int i = 0; i < dtStock.Rows.Count; i++)
                {
                    DataRow dr = dtSerial.NewRow();
                    dr["Product_Id"] = dtStock.Rows[i]["ProductId"].ToString();
                    dr["SerialNo"] = dtStock.Rows[i]["SerialNo"].ToString();
                    dr["SOrderNo"] = dtStock.Rows[i]["Field1"].ToString();
                    dr["BarcodeNo"] = dtStock.Rows[i]["Barcode"].ToString();
                    dr["BatchNo"] = dtStock.Rows[i]["BatchNo"].ToString();
                    dr["LotNo"] = dtStock.Rows[i]["LotNo"].ToString();
                    dr["ExpiryDate"] = dtStock.Rows[i]["ExpiryDate"].ToString();
                    dr["ManufacturerDate"] = dtStock.Rows[i]["ManufacturerDate"].ToString();
                    dtSerial.Rows.Add(dr);
                }

            }
            ViewState["dtFinal"] = dtSerial;


            ViewState["FromLocationId"] = dTranfer.Rows[0]["FromLocationID"].ToString();
            txtTransferDate.Text = Convert.ToDateTime(dTranfer.Rows[0]["TDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtVoucherNo.Text = dTranfer.Rows[0]["VoucherNo"].ToString();


            DataTable dtTranReqHeader = ObjTransReq.GetRecordUsingTransId(strCompId.ToString(), StrBrandId.ToString(), StrLocId.ToString(), dTranfer.Rows[0]["RequestNo"].ToString());
            if (dtTranReqHeader.Rows.Count != 0)
            {
                ViewState["RequestId"] = dTranfer.Rows[0]["RequestNo"].ToString();
                pnlTrans.Visible = true;
                txtTransReqDate.Text = Convert.ToDateTime(dtTranReqHeader.Rows[0]["TDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtTransNo.Text = dtTranReqHeader.Rows[0]["RequestNo"].ToString();

            }
            try
            {
                txtLocationName.ReadOnly = true;
                txtLocationName.Text = objLocation.GetLocationMasterById(strCompId, dTranfer.Rows[0]["FromLocationId"].ToString()).Rows[0]["Location_Name"].ToString();
            }
            catch
            {
                txtLocationName.Text = "";
            }


            DataTable dtTrasDetail = ObjTransferDetail.GetTransferDetailbyTransferId(strCompId.ToString(), StrBrandId.ToString(), dTranfer.Rows[0]["FromLocationId"].ToString(), e.CommandArgument.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtTrasDetail.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
                dtTrasDetail = new DataView(dtTrasDetail, "", "Serial_No", DataViewRowState.CurrentRows).ToTable();
                objPageCmn.FillData((object)gvEditProduct, dtTrasDetail, "", "");
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_New()", true);
            }
            getGridTotal();

        }

        txtscanProduct.Focus();
    }
    #region AllPageCode
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());

        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("118", (DataTable)Session["ModuleName"]);
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
        Page.Title = ObjSysParam.GetSysTitle();

        StrBrandId = Session["BrandId"].ToString();
        StrLocId = Session["LocId"].ToString();

        UserId = Session["UserId"].ToString();
        strCompId = Session["CompId"].ToString();

        if (Session["EmpId"].ToString() == "0")
        {
            btnSave.Visible = true;

            foreach (GridViewRow Row in GvTransfer.Rows)
            {

                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                ((ImageButton)Row.FindControl("IbtnPrint")).Visible = true;
            }

        }
        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "118", HttpContext.Current.Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {

            if (DtRow["Op_Id"].ToString() == "1")
            {
                btnSave.Visible = true;

            }
            foreach (GridViewRow Row in GvTransfer.Rows)
            {
                if (DtRow["Op_Id"].ToString() == "2")
                {
                    ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                }
                if (DtRow["Op_Id"].ToString() == "6")
                {
                    ((ImageButton)Row.FindControl("IbtnPrint")).Visible = true;
                }
            }
        }


    }
    #endregion

    protected void txtTransferDate_TextChanged(object sender, EventArgs e)
    {
        //if (txtTransferDate.Text == "")
        //{
        //    txtTransferDate.Text = DateTime.Now.ToString(ObjSysParam.GetSysParameterByParamName("Date_Format").Rows[0]["Param_Value"].ToString());
        //    txtVoucherNo.Focus();
        //}
    }

    protected void txtInQty_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lb = (Label)row.FindControl("lbloutqty");
        if (txt.Text == "")
        {
            txt.Text = "0.000";
        }
        if (lb.Text == "")
        {
            lb.Text = "0.000";
        }
        if (float.Parse(txt.Text.ToString()) > float.Parse(lb.Text.ToString()))
        {
            DisplayMessage("In quantity should not be greater than Out quantity");
            txt.Text = "0";
            txt.Focus();
        }

        if (float.Parse(txt.Text.ToString()) != float.Parse(lb.Text.ToString()))
        {
            DisplayMessage("In quantity should  be equal to Out quantity");
            txt.Text = "0";
            txt.Focus();
        }
        getGridTotal();
    }
    public string SetDateFormat(string Date)
    {
        string DateFormat = string.Empty;
        if (Date != "")
        {
            DateFormat = Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }

        return DateFormat;

    }
 

    #region stockable with serial number
    protected void lnkAddSerial_Command(object sender, EventArgs e)
    {
        lblCount.Text = Resources.Attendance.Serial_No + ":-";
        txtCount.Text = "0";

        Session["RdoType"] = "SerialNo";

        GridViewRow Row = (GridViewRow)((LinkButton)sender).Parent.Parent;
        ViewState["RowIndex"] = Row.RowIndex;

        lblProductIdvalue.Text = ((Label)Row.FindControl("lblGvProductCode")).Text;
        lblProductNameValue.Text = ((Label)Row.FindControl("lblProductId")).Text;
        lblOutQtyValue.Text = ((Label)Row.FindControl("lbloutqty")).Text;
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        txtSerialNo.Text = "";
        ViewState["PID"] = ((Label)Row.FindControl("lblPId")).Text;
        if (ViewState["dtFinal"] == null)
        {
        }
        else
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["dtFinal"];


            dt = new DataView(dt, "Product_Id='" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            ViewState["Tempdt"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

            ViewState["dtSerial"] = dt;
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();

        }

        txtSerialNo.Focus();

    }
    protected void BtnSerialSave_Click(object sender, EventArgs e)
    {

        string DuplicateserialNo = string.Empty;
        string serialNoExists = string.Empty;
        string alreadyout = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();


        if (txtSerialNo.Text.Trim() != "")
        {
            //  ViewState["dtSerial"] = null;

            string[] txt = txtSerialNo.Text.Split('\n');
            int counter = 0;

            if (ViewState["dtSerial"] == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Product_Id");
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("SOrderNo");
                dt.Columns.Add("BarcodeNo");
                dt.Columns.Add("BatchNo");
                dt.Columns.Add("LotNo");
                dt.Columns.Add("ExpiryDate");
                dt.Columns.Add("ManufacturerDate");

                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        string[] result = isSerialNumberValid(txt[i].ToString().Trim(), ViewState["PID"].ToString(), editid.Value, gvSerialNumber, ViewState["FromLocationId"].ToString());
                        if (result[0] == "VALID")
                        {

                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = ViewState["PID"].ToString();
                            dr[1] = txt[i].ToString();
                            dr[2] = "0";
                            //for batch number
                            dr[4] = result[4].ToString();

                            dr[5] = "0";
                            //for expiry date
                            dr[6] = DateTime.Now.ToString();
                            //for Manufacturer date
                            dr[7] = DateTime.Now.ToString();
                            counter++;

                        }
                        else if (result[0].ToString() == "DUPLICATE")
                        {
                            DuplicateserialNo += txt[i].ToString() + ",";
                        }
                        else if (result[0].ToString() == "NOT EXISTS")
                        {
                            serialNoExists += txt[i].ToString() + ",";
                        }


                    }
                }

            }
            else
            {
                dt = (DataTable)ViewState["dtSerial"];
                dtTemp = dt.Copy();

                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {

                        string[] result = isSerialNumberValid(txt[i].ToString().Trim(), ViewState["PID"].ToString(), editid.Value, gvSerialNumber, ViewState["FromLocationId"].ToString());
                        if (result[0] == "VALID")
                        {

                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = ViewState["PID"].ToString();
                            dr[1] = txt[i].ToString();
                            dr[2] = "0";
                            //for batch number
                            dr[4] = "0";

                            dr[5] = "0";
                            //for expiry date
                            dr[6] = DateTime.Now.ToString();
                            //for Manufacturer date
                            dr[7] = DateTime.Now.ToString();
                            counter++;

                        }
                        else if (result[0].ToString() == "DUPLICATE")
                        {
                            DuplicateserialNo += txt[i].ToString() + ",";
                        }
                        else if (result[0].ToString() == "NOT EXISTS")
                        {
                            serialNoExists += txt[i].ToString() + ",";
                        }

                    }
                }
            }

        }
        else
        {
            dt = (DataTable)ViewState["dtSerial"];
        }
        string Message = string.Empty;
        if (DuplicateserialNo != "" || serialNoExists != "")
        {
            if (DuplicateserialNo != "")
            {
                Message += "Following serial Number is duplicate=" + DuplicateserialNo;
            }
            if (serialNoExists != "")
            {
                if (Message == "")
                {
                    Message += "Following serial number not match with source location (Transfer Out)=" + serialNoExists;
                }
                else
                {
                    Message += Environment.NewLine + "Following serial number not match with source location (Transfer Out)=" + serialNoExists;
                }
            }

            DisplayMessage(Message);
        }
        //here we check that sales quantity should be less than system quantity
        //this validation is add by jitendra upadhyay on 22-05-2015
        //code start
        if (float.Parse(((TextBox)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtInQty")).Text) > float.Parse(((Label)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lbloutqty")).Text))
        {

            DisplayMessage("In Quantity Should be less than or equal to the Out quantity");
            return;

        }
        //code end

        ViewState["dtSerial"] = dt;
        if (ViewState["dtFinal"] == null)
        {
            if (ViewState["dtSerial"] != null)
            {
                ViewState["dtFinal"] = (DataTable)ViewState["dtSerial"];
            }
        }
        else
        {
            dt = new DataTable();
            DataTable Dtfinal = (DataTable)ViewState["dtFinal"];

            Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


            if (ViewState["dtSerial"] != null)
            {
                dt = (DataTable)ViewState["dtSerial"];
            }
            Dtfinal.Merge(dt);
            ViewState["dtFinal"] = Dtfinal;
        }


        if (ViewState["dtSerial"] != null)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, (DataTable)ViewState["dtSerial"], "", "");


            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        }


        ((TextBox)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtInQty")).Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();
        getGridTotal();
        txtSerialNo.Text = "";
        txtCount.Text = "0";
        txtSerialNo.Focus();
    }
    protected void btnResetSerial_Click(object sender, EventArgs e)
    {
        ViewState["dtSerial"] = null;
        txtSerialNo.Text = "";
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        txtCount.Text = "0";
        txtselectedSerialNumber.Text = "0";
        if (ViewState["dtFinal"] != null)
        {
            DataTable Dtfinal = (DataTable)ViewState["dtFinal"];

            Dtfinal = new DataView(Dtfinal, "Product_Id<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            ViewState["dtFinal"] = Dtfinal;
        }
        ((TextBox)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtInQty")).Text = "0";
        txtSerialNo.Focus();
    }
    protected void btncloseserial_Click(object sender, EventArgs e)
    {
        ViewState["dtSerial"] = null;
    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {

        ViewState["dtSerial"] = null;
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
        if (ViewState["dtSerial"] != null)
        {
            DataTable dt = (DataTable)ViewState["dtSerial"];

            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "SerialNo<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            ViewState["dtSerial"] = dt;
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
            if (ViewState["dtSerial"] != null)
            {
                dt = (DataTable)ViewState["dtSerial"];

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
        if (ViewState["dtSerial"] != null)
        {
            dt = (DataTable)ViewState["dtSerial"];

        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

        txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        txtserachserialnumber.Text = "";
        txtserachserialnumber.Focus();

    }
    protected void gvSerialNumber_OnSorting(object sender, GridViewSortEventArgs e)
    {
        hdnSort.Value = hdnSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dtSerial"];

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + hdnSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["dtSerial"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

        AllPageCode();
    }
    protected void btnexecute_Click(object sender, EventArgs e)
    {
        int counter = 0;
        txtSerialNo.Text = "";
        DataTable dtserial = ObjStockBatchMaster.GetStockBatchMasterAll_By_CompanyId_BrandId_LocationId_ProductId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ViewState["FromLocationId"].ToString(), ViewState["PID"].ToString());

        try
        {
            dtserial = new DataView(dtserial, "TransType='TO' and TransTypeId=" + editid.Value + " ", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        for (int i = 0; i < dtserial.Rows.Count; i++)
        {
            if (txtSerialNo.Text == "")
            {
                txtSerialNo.Text = dtserial.Rows[i]["SerialNo"].ToString();

            }
            else
            {
                txtSerialNo.Text += Environment.NewLine + dtserial.Rows[i]["SerialNo"].ToString();

            }
            counter++;
        }
        txtCount.Text = counter.ToString();
        if (counter == 0)
        {
            DisplayMessage("Serial Number Not Found");
        }

    }
    public static string[] isSerialNumberValid(string serialNumber, string ProductId, string TransId, GridView gvSerialNumber, string FromLocationId)
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



            DataTable dtserial = ObjStockBatchMaster.GetStockBatchMasterAll_By_CompanyId_BrandId_LocationId_ProductId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), FromLocationId, ProductId);



            try
            {
                dtserial = new DataView(dtserial, "SerialNo='" + serialNumber + "' and ProductId=" + ProductId + " and TransType='TO' and TransTypeId=" + TransId + " ", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            if (dtserial.Rows.Count > 0)
            {
                Result[0] = "VALID";

            }
            else
            {
                Result[0] = "NOT EXISTS";
            }
        }
        else
        {
            Result[0] = "DUPLICATE";
        }

        return Result;
    }
    public void UpdateRecordForStckableItem(string ProductId, string ItemType, float Quantity, string TransId, string UnitId, string LedgerRefid, string FromLocationId, ref SqlTransaction trns)
    {

        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        float Currencyquantity = 0;

        DataTable dt = new DataTable();
        dt = ObjStockBatchMaster.GetStockBatchMasterAll_By_CompanyId_BrandId_LocationId_ProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["FromLocationId"].ToString(), ProductId, ref trns);
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
            Currencyquantity = float.Parse(dt.Rows[i]["Quantity"].ToString());

            if (Quantity == 0)
            {
                break;
            }
            string sql = "select SUM(quantity) from Inv_StockBatchMaster where Field3='" + dt.Rows[i]["Trans_Id"].ToString() + "' and InOut='O'";

            DataTable dtTrns = da.return_DataTable(sql, ref trns);


            if (dtTrns.Rows.Count > 0)
            {

                try
                {
                    if (Currencyquantity == float.Parse(dtTrns.Rows[0][0].ToString()))
                    {
                        continue;
                    }
                    else
                    {
                        float Remqty = 0;

                        Remqty = Currencyquantity - float.Parse(dtTrns.Rows[0][0].ToString());
                        if (Remqty > Quantity)
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), FromLocationId, "TO", TransId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), LedgerRefid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), StrLocId.ToString(), "TI", TransId, ProductId, UnitId, "I", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            Quantity = 0;
                        }
                        else
                        {

                            ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), FromLocationId, "TO", TransId, ProductId, UnitId, "O", "0", "0", Remqty.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), LedgerRefid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), StrLocId.ToString(), "TI", TransId, ProductId, UnitId, "I", "0", "0", Remqty.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            Quantity = Quantity - Remqty;

                        }

                    }
                }
                catch
                {
                    if (Currencyquantity > Quantity)
                    {

                        ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), FromLocationId, "TO", TransId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), LedgerRefid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), StrLocId.ToString(), "TI", TransId, ProductId, UnitId, "I", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                        Quantity = 0;
                    }
                    else
                    {
                        ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), FromLocationId, "TO", TransId, ProductId, UnitId, "O", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), LedgerRefid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), StrLocId.ToString(), "TI", TransId, ProductId, UnitId, "I", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                        Quantity = Quantity - Currencyquantity;

                    }

                }

            }
            else
            {
                if (Currencyquantity > Quantity)
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), FromLocationId, "TO", TransId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), LedgerRefid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), StrLocId.ToString(), "TI", TransId, ProductId, UnitId, "I", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    Quantity = 0;
                }
                else
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), FromLocationId, "TO", TransId, ProductId, UnitId, "O", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), LedgerRefid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    ObjStockBatchMaster.InsertStockBatchMaster(strCompId.ToString(), StrBrandId.ToString(), StrLocId.ToString(), "TI", TransId, ProductId, UnitId, "I", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    Quantity = Quantity - Currencyquantity;

                }
            }
        }

    }

    #endregion

    #region printreport

    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + e.CommandArgument.ToString() + "&&Type=TI','window','width=1024');", true);
    }
    #endregion


    #region RowBoundFunction
    //protected void gvEditProduct_OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {


    //        string ProductID = ((Label)e.Row.FindControl("lblPID")).Text;

    //        if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID).Rows[0]["MaintainStock"].ToString() == "SNO")
    //        {
    //            if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Serial on Transfer Voucher").Rows[0]["ParameterValue"].ToString()))
    //            {
    //                //((TextBox)e.Row.FindControl("txtInQty")).Enabled = false;

    //                ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
    //            }
    //            else
    //            {
    //                //((TextBox)e.Row.FindControl("txtInQty")).Enabled = true;

    //                ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
    //            }
    //        }
    //        else
    //        {
    //            //((TextBox)e.Row.FindControl("txtInQty")).Enabled = true;

    //            ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
    //        }
    //    }
    //}


    #endregion

    #region Post
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        AllPageCode();

    }
    #endregion


    #region getGridCalculation
    public void getGridTotal()
    {
        float fRequest = 0;
        float fout = 0;
        float fIn = 0;

        foreach (GridViewRow gvrow in gvEditProduct.Rows)
        {

            try
            {
                fRequest += float.Parse(((Label)gvrow.FindControl("lblReqQty")).Text);
            }
            catch
            {
                fRequest += 0;
            }
            try
            {
                fout += float.Parse(((Label)gvrow.FindControl("lbloutqty")).Text);
            }
            catch
            {
                fout += 0;
            }
            try
            {
                fIn += float.Parse(((TextBox)gvrow.FindControl("txtInQty")).Text);
            }
            catch
            {
                fIn += 0;
            }
        }


        try
        {
            ((Label)gvEditProduct.FooterRow.FindControl("txttotReqqtyShow")).Text = fRequest.ToString();
            ((Label)gvEditProduct.FooterRow.FindControl("txttotoutqtyShow")).Text = fout.ToString();
            ((Label)gvEditProduct.FooterRow.FindControl("txtInoutqtyShow")).Text = fIn.ToString();
        }
        catch
        {
        }
    }
    #endregion

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

    public string SetDecimal(string Amount)
    {

        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Amount);



    }



    #region Scanningsolution

    protected void btnscanserial_OnClick(object sender, EventArgs e)
    {
        if (txtscanProduct.Text == "")
        {
            txtscanProduct.Focus();
            return;
        }

        int counter = 0;

        DataTable dt = new DataTable();
        dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtscanProduct.Text.ToString());
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEditProduct.Rows)
            {
                if (((Label)gvrow.FindControl("lblGvProductCode")).Text == dt.Rows[0]["ProductCode"].ToString())
                {
                    if (float.Parse(((TextBox)gvrow.FindControl("txtInQty")).Text) + 1 <= float.Parse(((Label)gvrow.FindControl("lbloutqty")).Text))
                    {
                        ((TextBox)gvrow.FindControl("txtInQty")).Text = SetDecimal((float.Parse(((TextBox)gvrow.FindControl("txtInQty")).Text) + 1).ToString());
                    }
                    ((TextBox)gvrow.FindControl("txtInQty")).Enabled = true;
                    counter = 1;
                }
            }
        }

        if (counter == 0)
        {
            DisplayMessage("Product Not Found");

        }
        txtscanProduct.Text = "";
        txtscanProduct.Focus();
    }
    #endregion

    #region RowDataBound
    protected void GvProduct_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //string strMaintainStock = string.Empty;

            string ProductID = ((Label)e.Row.FindControl("lblPID")).Text;
            DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTransferInScanning");
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    ((TextBox)e.Row.FindControl("txtInQty")).Enabled = false;

                }
            }

            if (editid.Value != "")
            {

                if (((TextBox)e.Row.FindControl("txtInQty")).Text != "")
                {
                    if (float.Parse(((TextBox)e.Row.FindControl("txtInQty")).Text) > 0)
                    {
                        ((TextBox)e.Row.FindControl("txtInQty")).Enabled = true;

                    }
                }

            }
        }
    }

    #endregion

    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
        }
    }
}
