using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Inventory_Transfer : BasePage
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
    Common cmn = null;
    Inv_StockBatchMaster objstockbatchMaster = null;
    Inv_StockDetail objstock = null;
    Set_DocNumber objDocNo = null;
    Inv_ParameterMaster objInvParam = null;
    DataAccessClass ObjDa = null;
    RoleMaster objRole = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;
    string StrLocId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        if (Request.QueryString["Id"] != null)
        {

            StrLocId = Request.QueryString["LocId"].ToString();
        }
        else
        {
            StrLocId = Session["LocId"].ToString();
        }


        //Lbl_Tab_New.Enabled = false;
        //btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjTransReq = new Inv_TransferRequestHeader(Session["DBConnection"].ToString());
        ObjTransferReqDetail = new Inv_TransferRequestDetail(Session["DBConnection"].ToString());
        ObjTransferHeader = new Inv_TransferHeader(Session["DBConnection"].ToString());
        ObjTransferDetail = new Inv_TransferDetail(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objstockbatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        objstock = new Inv_StockDetail(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/Transfer.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            FillTransferRequestgrid();
            Calender.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtTransferDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            FillGrid();
            txtFromDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
            txtToDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            btnList_Click(null, null);
            ViewState["dtSerial"] = null;
            ViewState["ProductId"] = null;
            ViewState["dtFinal"] = null;
            txtVoucherNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = GetDocumentNumber();
            FillddlLocation();
            txtFromDate_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtToDate_CalendarExtender.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            //this code for when we redirect from the producte ledger page 
            //this code created on 22-07-2015
            //code start

            if (Request.QueryString["Id"] != null)
            {
                LinkButton imgeditbutton = new LinkButton();
                imgeditbutton.ID = "lnkViewDetail";
                btnEdit_Command(imgeditbutton, new CommandEventArgs("commandName", Request.QueryString["Id"].ToString()));
                //btnList.Visible = false;
                //btnBin.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_Bin()", true);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_List()", true);
                btnPICancel.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_Request()", true);
                StrLocId = Request.QueryString["LocId"].ToString();
                //  ((Panel)Master.FindControl("pnlaccordian")).Visible = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Bin()", true);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_List()", true);
                btnPICancel.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Request()", true);
                //((Panel)Master.FindControl("pnlaccordian")).Visible = true;
            }
            //code end

            DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTransferOutScanning");
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    pnlScanProduct.Visible = true;
                }
            }
        }
        //AllPageCode();

    }
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "11", "94", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        if (txtTransferDate.Text == "")
        {
            DisplayMessage("Select Date");

            txtTransferDate.Focus();
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
                DisplayMessage("Enter Transfer Date in format " + Session["DateFormat"].ToString() + "");
                txtTransferDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTransferDate);
                return;
            }
        }


        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtTransferDate.Text), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        if (txtVoucherNo.Text == "")
        {
            DisplayMessage("Voucher No. Not Found , You Can set in document Page !");
            txtVoucherNo.Focus();

            return;
        }
        else
        {

            DataTable dt = ObjTransferHeader.GetTransferHeader(Session["CompId"].ToString());

            if (editid.Value == "")
            {
                dt = new DataView(dt, "Brand_Id=" + Session["BrandId"].ToString() + " and FromLocationID=" + Session["LocId"].ToString() + " and VoucherNo='" + txtVoucherNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dt = new DataView(dt, "Brand_Id=" + Session["BrandId"].ToString() + " and FromLocationID=" + Session["LocId"].ToString() + " and VoucherNo='" + txtVoucherNo.Text + "' and Trans_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();

            }

            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Voucher Number is already exists");
                txtVoucherNo.Focus();

                return;
            }

        }
        string LocationId = string.Empty;
        if (ViewState["RequestId"] == null)
        {



            DataTable Dtlocation = objLocation.GetLocationMasterByLocationName(Session["CompId"].ToString(), txtLocationNameOut.Text);
            if (Dtlocation.Rows.Count != 0)
            {
                try
                {
                    LocationId = Dtlocation.Rows[0]["Location_Id"].ToString();
                }
                catch
                {
                    LocationId = "0";
                }
            }
            else
            {
                LocationId = "0";

            }
            if (LocationId == "0")
            {
                DisplayMessage("Select Location");
                txtLocationNameOut.Text = "";
                txtLocationNameOut.Focus();

                return;
            }
        }
        if (ViewState["RequestId"] != null)
        {
            //here we check that Tranfer out record already exist or not for this request


            if (editid.Value == "")
            {
                string strsql = "select * from Inv_TransferHeader where RequestNo =" + ViewState["RequestId"].ToString() + "";
                if (ObjDa.return_DataTable(strsql).Rows.Count > 0)
                {
                    //DisplayMessage("Transfer out already exists for this request number");

                    Reset();
                    ViewState["RequestId"] = null;
                    return;
                }
            }

            DataTable Dtlocation = objLocation.GetLocationMasterByLocationName(Session["CompId"].ToString(), txtLocationName.Text);
            if (Dtlocation.Rows.Count != 0)
            {
                LocationId = Dtlocation.Rows[0]["Location_Id"].ToString();
            }
            else
            {
                LocationId = "0";

            }
        }
        bool b = false;
        string StrReqId = "";
        if (ViewState["RequestId"] != null)
        {
            StrReqId = ViewState["RequestId"].ToString();
        }
        else
        {
            DisplayMessage("Select from Request Tab");
            StrReqId = "0";
            return;

        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            if (editid.Value == "")
            {

                //here we begin transaction

                if (ViewState["RequestId"] != null)
                {
                    ObjTransferHeader.InsertTransferHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, ObjSysParam.getDateForInput(txtTransferDate.Text).ToString(), StrReqId.Trim(), LocationId.Trim(), txtDescription.Text.Trim(), "N", DateTime.Now.ToString(), null, txtVoucherNo.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                    string TransId = ObjTransferHeader.getAutoId(ref trns);

                    if (txtVoucherNo.Text == ViewState["DocNo"].ToString())
                    {
                        DataTable dtCount = ObjTransferHeader.GetTransferHeader(Session["CompId"].ToString(), ref trns);
                        try
                        {
                            dtCount = new DataView(dtCount, "Brand_Id=" + Session["BrandId"].ToString() + " and FromLocationID=" + Session["LocId"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                        if (dtCount.Rows.Count == 0)
                        {
                            ObjTransferHeader.Updatecode(TransId, txtVoucherNo.Text + "1", ref trns);
                            txtVoucherNo.Text = txtVoucherNo.Text + "1";
                        }
                        else
                        {
                            ObjTransferHeader.Updatecode(TransId, txtVoucherNo.Text + dtCount.Rows.Count, ref trns);
                            txtVoucherNo.Text = txtVoucherNo.Text + dtCount.Rows.Count;
                        }
                    }



                    foreach (GridViewRow Row in GvProduct.Rows)
                    {

                        string strItemType = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString();
                        if (((TextBox)Row.FindControl("txtOutQty")).Text.Trim() == "")
                        {
                            ((TextBox)Row.FindControl("txtOutQty")).Text = "0";

                        }

                        ObjTransferDetail.InsertTransferDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, TransId, ((Label)Row.FindControl("lblSerialNO")).Text.Trim(), ((Label)Row.FindControl("lblPId")).Text.Trim(), ((Label)Row.FindControl("lblunitcost")).Text.Trim(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), ((Label)Row.FindControl("lblReqQty")).Text.Trim(), ((TextBox)Row.FindControl("txtOutQty")).Text.Trim(), "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        //updated by jitendra upadhyay to insert the serial number in stock batch master table
                        //updated on 24-12-2013
                        if (strItemType == "SNO")
                        {
                            if (ViewState["dtFinal"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["dtFinal"];
                                dt = new DataView(dt, "Product_Id='" + ((Label)Row.FindControl("lblPId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    string strsql = "update Inv_StockBatchMaster set Location_Id='" + LocationId.Trim() + "' where ProductId='" + ((Label)Row.FindControl("lblPId")).Text + "' and SerialNo='" + dr["SerialNo"].ToString().Trim() + "' and InOut='I'";
                                    ObjDa.execute_Command(strsql, ref trns);
                                    //ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", TransId, ((Label)Row.FindControl("lblPId")).Text.Trim(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(),"","","", "", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                                }
                            }
                        }
                        else if (strItemType == "FE" || strItemType == "LE" || strItemType == "FM" || strItemType == "LM")
                        {
                            UpdateRecordForStckableItem(((Label)Row.FindControl("lblPId")).Text.Trim(), strItemType, float.Parse(((TextBox)Row.FindControl("txtOutQty")).Text), TransId, ((Label)Row.FindControl("lblUnitId")).Text.Trim(), ref trns);

                        }
                    }
                    DisplayMessage("Record Saved", "green");
                    int insertintransReq = 0;
                    insertintransReq = ObjTransReq.UpdateStatusInTransferRequestHeader(StrReqId, Session["CompId"].ToString(), Session["BrandId"].ToString(), LocationId.Trim(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), "2", ref trns);

                }
                else
                {

                    int maxid = 0;
                    maxid = ObjTransferHeader.InsertTransferHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, ObjSysParam.getDateForInput(txtTransferDate.Text).ToString(), StrReqId.Trim(), LocationId.Trim(), txtDescription.Text.Trim(), "N", DateTime.Now.ToString(), null, txtVoucherNo.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                    if (txtVoucherNo.Text == ViewState["DocNo"].ToString())
                    {
                        DataTable dtCount = ObjTransferHeader.GetTransferHeader(Session["CompId"].ToString(), ref trns);
                        try
                        {
                            dtCount = new DataView(dtCount, "Brand_Id=" + Session["BrandId"].ToString() + " and FromLocationID=" + Session["LocId"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                        if (dtCount.Rows.Count == 0)
                        {
                            ObjTransferHeader.Updatecode(maxid.ToString(), txtVoucherNo.Text + "1", ref trns);
                            txtVoucherNo.Text = txtVoucherNo.Text + "1";
                        }
                        else
                        {
                            ObjTransferHeader.Updatecode(maxid.ToString(), txtVoucherNo.Text + dtCount.Rows.Count, ref trns);
                            txtVoucherNo.Text = txtVoucherNo.Text + dtCount.Rows.Count;
                        }
                    }



                    foreach (GridViewRow Row in gvProductRequest.Rows)
                    {
                        string strItemType = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString();


                        ObjTransferDetail.InsertTransferDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, maxid.ToString(), ((Label)Row.FindControl("lblSerialNum")).Text.Trim(), ((Label)Row.FindControl("lblPID")).Text.Trim(), "0", ((Label)Row.FindControl("lblUnitId")).Text.Trim(), ((TextBox)Row.FindControl("lblReqQty")).Text.Trim(), ((TextBox)Row.FindControl("lblReqQty")).Text.Trim(), "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);

                        if (strItemType == "SNO")
                        {

                            if (ViewState["dtFinal"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["dtFinal"];
                                dt = new DataView(dt, "Product_Id='" + ((Label)Row.FindControl("lblPId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    string strsql = "update Inv_StockBatchMaster set Location_Id='" + LocationId.Trim() + "' where ProductId='" + ((Label)Row.FindControl("lblPId")).Text + "' and SerialNo='" + dr["SerialNo"].ToString().Trim() + "' and InOut='I'";
                                    ObjDa.execute_Command(strsql, ref trns);

                                    //ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", maxid.ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(),"","","", "", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                                }
                            }
                        }
                        else if (strItemType == "FE" || strItemType == "LE" || strItemType == "FM" || strItemType == "LM")
                        {
                            UpdateRecordForStckableItem(((Label)Row.FindControl("lblPId")).Text.Trim(), ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString(), float.Parse(((TextBox)Row.FindControl("lblReqQty")).Text.Trim()), maxid.ToString(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), ref trns);

                        }
                    }
                    DisplayMessage("Record Saved", "green");

                }

            }
            else
            {

                ObjTransferHeader.UpdateTransferHeader(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, ObjSysParam.getDateForInput(txtTransferDate.Text).ToString(), StrReqId.Trim(), LocationId.Trim(), txtDescription.Text.Trim(), "N", DateTime.Now.ToString(), null, txtVoucherNo.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                ObjTransferDetail.DeleteTransferDetailbyHeaderTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, editid.Value, ref trns);
                string TransId = editid.Value;
                if (ViewState["RequestId"] != null)
                {
                    foreach (GridViewRow Row in gvEditProduct.Rows)
                    {
                        string strItemType = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString();

                        if (((TextBox)Row.FindControl("txtOutQty")).Text.Trim() == "")
                        {
                            ((TextBox)Row.FindControl("txtOutQty")).Text = "0";

                        }

                        ObjTransferDetail.InsertTransferDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, TransId, ((Label)Row.FindControl("lblSerialNO")).Text.Trim(), ((Label)Row.FindControl("lblPId")).Text.Trim(), ((Label)Row.FindControl("lblunitcost")).Text.Trim(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), ((Label)Row.FindControl("lblReqQty")).Text.Trim(), ((TextBox)Row.FindControl("txtOutQty")).Text.Trim(), "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);

                        //updated by jitendra upadhyay to update the serial number in stock batch master table
                        //updated on 24-12-2013
                        ObjStockBatchMaster.DeleteStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, "TO", TransId, ((Label)Row.FindControl("lblPId")).Text.Trim(), ref trns);

                        if (strItemType == "SNO")
                        {
                            if (ViewState["dtFinal"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["dtFinal"];
                                dt = new DataView(dt, "Product_Id='" + ((Label)Row.FindControl("lblPId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    string strsql = "update Inv_StockBatchMaster set Location_Id='" + LocationId.Trim() + "', TransType='TO', TransTypeId='" + editid.Value + "' where ProductId='" + ((Label)Row.FindControl("lblPId")).Text + "' and SerialNo='" + dr["SerialNo"].ToString().Trim() + "' and InOut='I'";
                                    ObjDa.execute_Command(strsql, ref trns);
                                    //ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", TransId, ((Label)Row.FindControl("lblPId")).Text.Trim(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(),"","","", "", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                                }
                            }
                        }
                        else if (strItemType == "FE" || strItemType == "LE" || strItemType == "FM" || strItemType == "LM")
                        {
                            UpdateRecordForStckableItem(((Label)Row.FindControl("lblPId")).Text.Trim(), ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString(), float.Parse(((TextBox)Row.FindControl("txtOutQty")).Text.Trim()), TransId.ToString(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), ref trns);

                        }

                    }
                }
                else
                {

                    foreach (GridViewRow Row in gvProductRequest.Rows)
                    {

                        string strItemType = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString();

                        ObjTransferDetail.InsertTransferDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, TransId.ToString(), ((Label)Row.FindControl("lblSerialNum")).Text.Trim(), ((Label)Row.FindControl("lblPID")).Text.Trim(), "0", ((Label)Row.FindControl("lblUnitId")).Text.Trim(), ((TextBox)Row.FindControl("lblReqQty")).Text.Trim(), ((TextBox)Row.FindControl("lblReqQty")).Text.Trim(), "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        ObjStockBatchMaster.DeleteStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, "TO", TransId, ((Label)Row.FindControl("lblPId")).Text.Trim(), ref trns);
                        if (strItemType == "SNO")
                        {


                            if (ViewState["dtFinal"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["dtFinal"];
                                dt = new DataView(dt, "Product_Id='" + ((Label)Row.FindControl("lblPId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", TransId, ((Label)Row.FindControl("lblPId")).Text.Trim(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(), "", "", "", "", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                }
                            }
                        }
                        else if (strItemType == "FE" || strItemType == "LE" || strItemType == "FM" || strItemType == "LM")
                        {
                            UpdateRecordForStckableItem(((Label)Row.FindControl("lblPId")).Text.Trim(), ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)Row.FindControl("lblPId")).Text.Trim(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString(), float.Parse(((TextBox)Row.FindControl("lblReqQty")).Text.Trim()), TransId.ToString(), ((Label)Row.FindControl("lblUnitId")).Text.Trim(), ref trns);

                        }

                    }
                }
                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
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
        Reset();
        ViewState["RequestId"] = null;
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
        txtDescription.Text = "";
        GvProduct.DataSource = null;
        GvProduct.DataBind();
        FillGrid();
        editid.Value = "";
        gvEditProduct.DataSource = null;
        gvEditProduct.DataBind();

        txtLocationNameOut.Text = "";
        gvProductRequest.DataSource = null;
        gvProductRequest.DataBind();
        ViewState["RequestId"] = null;
        if (ViewState["RequestId"] != null)
        {
            btnAddProduct.Visible = false;
            txtLocationNameOut.Visible = false;
            txtLocationName.Visible = true;
            lblReqLocationColon.Visible = false;
            lblReqLocation.Visible = false;
            txtLocationName.ReadOnly = false;
            gvProductRequest.DataSource = null;
            gvProductRequest.DataBind();
        }
        else
        {
            btnAddProduct.Visible = true;
            txtLocationNameOut.Visible = true;
            txtLocationName.Visible = false;
            lblReqLocationColon.Visible = true;
            lblReqLocation.Visible = true;
            txtLocationNameOut.ReadOnly = false;

        }

        ViewState["Dtproduct"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;

        FillTransferRequestgrid();
        ViewState["dtSerial"] = null;
        ViewState["ProductId"] = null;
        ViewState["dtFinal"] = null;
        txtVoucherNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = GetDocumentNumber();
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
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
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Focus();
        FillGrid();
        //AllPageCode();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "TDate")
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
            DataTable dtCust = (DataTable)Session["DtTransfer"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Trans"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)GvTransfer, view.ToTable(), "", "");

            //AllPageCode();

            // btnbind.Focus();

        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlTransferRequest.Visible = false;
        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        txtValue.Focus();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        PnlTransferRequest.Visible = false;

        if (ViewState["RequestId"] != null)
        {
            btnAddProduct.Visible = false;
            txtLocationNameOut.Visible = false;
            txtLocationName.Visible = true;
            lblReqLocationColon.Visible = false;
            lblReqLocation.Visible = false;
            ViewState["Dtproduct"] = null;
            gvProductRequest.DataSource = null;
            gvProductRequest.DataBind();
            if (editid.Value == "")
            {
                txtLocationName.ReadOnly = false;
                txtLocationNameOut.ReadOnly = false;
            }
            else
            {
                txtLocationName.ReadOnly = true;
                txtLocationNameOut.ReadOnly = true;
            }
        }
        else
        {
            btnAddProduct.Visible = true;
            txtLocationNameOut.Visible = true;
            txtLocationNameOut.ReadOnly = false;
            txtLocationName.Visible = false;
            lblReqLocationColon.Visible = true;
            lblReqLocation.Visible = true;
            if (editid.Value == "")
            {
                txtLocationName.ReadOnly = false;
                txtLocationNameOut.ReadOnly = false;
            }
            else
            {
                txtLocationName.ReadOnly = true;
                txtLocationNameOut.ReadOnly = true;
            }
        }

    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlTransferRequest.Visible = false;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
        txtValueBin.Focus();
    }
    protected void btnTransRequest_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlList.Visible = false;
        PnlTransferRequest.Visible = true;
        FillTransferRequestgrid();


    }
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = objUnit.GetUnitMasterById(Session["CompId"].ToString().ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
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
        return ProductName;

    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
    public void FillTransferRequestgrid()
    {
        DataTable dt = new DataView(ObjTransReq.GetAllRecord_TrueByRequestLocation(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "0"), "Post='Y' and Status='0' and RequestLocationId='" + StrLocId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvTransferRequest, dt, "", "");

        btnTRTotal.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        Session["DtTransferRequest"] = dt;
        Session["DtTRFilter"] = dt;
        //AllPageCode();

    }
    protected void btnTransferRequest_Command(object sender, CommandEventArgs e)
    {
        DataTable dtTranReqHeader = ObjTransReq.GetRecordUsingTransIdByRequestLocation(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), e.CommandArgument.ToString());
        if (dtTranReqHeader.Rows.Count != 0)
        {
            ViewState["RequestId"] = e.CommandArgument.ToString();
            //editid.Value = e.CommandArgument.ToString();
            pnlTrans.Visible = true;
            txtTransReqDate.Text = Convert.ToDateTime(dtTranReqHeader.Rows[0]["TDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtTransNo.Text = dtTranReqHeader.Rows[0]["RequestNo"].ToString();
            txtDescription.Text = dtTranReqHeader.Rows[0]["Remark"].ToString();
            try
            {
                txtLocationName.ReadOnly = true;
                txtLocationName.Text = objLocation.GetLocationMasterById(Session["CompId"].ToString(), dtTranReqHeader.Rows[0]["Location_ID"].ToString()).Rows[0]["Location_Name"].ToString();
            }
            catch
            {
                txtLocationName.Text = "";
            }
            DataTable dtTrasDetail = ObjTransferReqDetail.GetTransferRequestDetailbyRequestId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), dtTranReqHeader.Rows[0]["Location_ID"].ToString(), e.CommandArgument.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015

            dtTrasDetail = new DataView(dtTrasDetail, "ProductId<>'0'", "", DataViewRowState.CurrentRows).ToTable();
            objPageCmn.FillData((object)GvProduct, dtTrasDetail, "", "");
            foreach (GridViewRow gvr in GvProduct.Rows)
            {
                Label lblGvProductCode = (Label)gvr.FindControl("lblGvProductCode");
                LinkButton lnkAddSerial = (LinkButton)gvr.FindControl("lnkAddSerial");

                string serialno = objDa.get_SingleValue("Select MaintainStock from Inv_ProductMaster where ProductCode='" + lblGvProductCode.Text + "'");
                if (serialno == "SNO")
                {
                    lnkAddSerial.Visible = true;
                }
                else
                {
                    lnkAddSerial.Visible = false;
                }
            }
            gvEditProduct.DataSource = null;
            gvEditProduct.DataBind();
            //foreach (GridViewRow row in GvProduct.Rows)
            //{
            //    DataTable dt = new DataView(objstock.GetStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId), "ProductId='" + ((Label)row.FindControl("lblPId")).Text.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    if (dt.Rows.Count == 0 || Convert.ToDouble(dt.Rows[0]["Quantity"].ToString()) <= 0)
            //    {
            //        row.Enabled = false;

            //    }
            //}
            btnNew_Click(null, null);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active_Request()", true);
        }
        getGridTotal();
        btnAddProduct.Visible = false;
        txtLocationNameOut.Visible = false;
        txtLocationName.Visible = true;
        lblReqLocationColon.Visible = false;
        lblReqLocation.Visible = false;
        ViewState["Dtproduct"] = null;
        gvProductRequest.DataSource = null;
        gvProductRequest.DataBind();
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtscanProduct.Focus();
    }

    protected void IbtnUpdateRequestStatus_Command(object sender, CommandEventArgs e)
    {

        ObjTransReq.UpdateStatusInTransferRequestHeader(e.CommandArgument.ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
        FillTransferRequestgrid();
        DisplayMessage("Transfer Request Rejected Successfully");
        //AllPageCode();

    }
    protected void gvTransferRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransferRequest.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["DtTRFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvTransferRequest, dt, "", "");

        //AllPageCode();
    }
    protected void GvTransfer_Sorting(object sender, GridViewSortEventArgs e)
    {
        hdnSort.Value = hdnSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Trans"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + hdnSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Trans"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)GvTransfer, dt, "", "");

        //AllPageCode();
        GvTransfer.HeaderRow.Focus();
    }

    protected void gvTransferRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        hdnTransferRequest.Value = hdnTransferRequest.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["DtTRFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + hdnTransferRequest.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["DtTRFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvTransferRequest, dt, "", "");

        //AllPageCode();
        gvTransferRequest.HeaderRow.Focus();
    }
    protected void GvTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvTransfer.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Trans"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)GvTransfer, dt, "", "");

        //AllPageCode();
    }
    public void FillGrid()
    {
        DataTable dt = ObjTransferHeader.GetTransferHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId);

        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            PostStatus = " Post='Y'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            PostStatus = " Post='N'";
        }

        DataTable dtBrand = new DataView(ObjTransferHeader.GetTransferHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId), PostStatus, "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)GvTransfer, dtBrand, "", "");

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count;
        Session["dtTransfer"] = dtBrand;
        Session["dtFilter_Trans"] = dtBrand;


        //AllPageCode();
    }
    public string GetPost(string Post)
    {
        string Result = string.Empty;
        if (Post == "Y")
        {
            Result = "Yes";
        }
        else
        {
            Result = "No";
        }
        return Result;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dTranfer = ObjTransferHeader.GetTransferHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, e.CommandArgument.ToString());
        if (dTranfer.Rows.Count > 0)
        {
            editid.Value = e.CommandArgument.ToString();
            if (dTranfer.Rows[0]["Post"].ToString() == "Y")
            {
                DisplayMessage("Record cannot be Updated");
                return;
            }
            //updated by jitendra upadhyay to retreive the serial number in editable mode from stock batch master table
            //updated on 24-12-2013
            DataTable dtSerial = new DataTable();
            dtSerial.Columns.Add("Product_Id");
            dtSerial.Columns.Add("SerialNo");
            dtSerial.Columns.Add("SOrderNo");
            dtSerial.Columns.Add("BarcodeNo");
            dtSerial.Columns.Add("BatchNo");
            dtSerial.Columns.Add("LotNo");
            dtSerial.Columns.Add("ExpiryDate");
            dtSerial.Columns.Add("ManufacturerDate");
            dtSerial.Columns.Add("Product_Index");


            DataTable dtStock = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), dTranfer.Rows[0]["TOLocationId"].ToString(), "TO", editid.Value);
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

            txtTransferDate.Text = Convert.ToDateTime(dTranfer.Rows[0]["TDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtVoucherNo.Text = dTranfer.Rows[0]["VoucherNo"].ToString();
            txtDescription.Text = dTranfer.Rows[0]["Remark"].ToString();

            DataTable Dtlocation = objLocation.GetLocationMasterById(Session["CompId"].ToString(), dTranfer.Rows[0]["TOLocationId"].ToString());
            if (Dtlocation.Rows.Count > 0)
            {
                txtLocationNameOut.Text = Dtlocation.Rows[0]["Location_Name"].ToString();
                txtLocationNameOut.ReadOnly = true;
            }

            DataTable dtTranReqHeader = ObjTransReq.GetRecordUsingTransIdByRequestLocation(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), dTranfer.Rows[0]["RequestNo"].ToString());
            if (dtTranReqHeader.Rows.Count != 0)
            {
                ViewState["RequestId"] = dTranfer.Rows[0]["RequestNo"].ToString();
                pnlTrans.Visible = true;
                txtTransReqDate.Text = Convert.ToDateTime(dtTranReqHeader.Rows[0]["TDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtTransNo.Text = dtTranReqHeader.Rows[0]["RequestNo"].ToString();

                try
                {
                    txtLocationName.ReadOnly = true;
                    txtLocationName.Text = objLocation.GetLocationMasterById(Session["CompId"].ToString(), dtTranReqHeader.Rows[0]["Location_ID"].ToString()).Rows[0]["Location_Name"].ToString();
                }
                catch
                {
                    txtLocationName.Text = "";
                }

                DataTable dtTrasDetail = ObjTransferDetail.GetTransferDetailbyTransferId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), e.CommandArgument.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
                //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015


                dtTrasDetail = new DataView(dtTrasDetail, "", "Serial_No", DataViewRowState.CurrentRows).ToTable();

                objPageCmn.FillData((object)gvEditProduct, dtTrasDetail, "", "");

                foreach (GridViewRow gvr in gvEditProduct.Rows)
                {
                    Label lblGvProductCode = (Label)gvr.FindControl("lblGvProductCode");
                    LinkButton lnkAddSerial = (LinkButton)gvr.FindControl("lnkAddSerial");

                    string serialno = objDa.get_SingleValue("Select MaintainStock from Inv_ProductMaster where ProductCode='" + lblGvProductCode.Text + "'");
                    if (serialno == "SNO")
                    {
                        lnkAddSerial.Visible = true;
                    }
                    else
                    {
                        lnkAddSerial.Visible = false;
                    }
                }
                GvProduct.DataSource = null;
                GvProduct.DataBind();
                btnNew_Click(null, null);
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

            }
            else
            {

                DataTable dtTrasDetail = ObjTransferDetail.GetTransferDetailbyTransferId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), e.CommandArgument.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
                ViewState["Dtproduct"] = dtTrasDetail;
                //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
                objPageCmn.FillData((object)gvProductRequest, dtTrasDetail, "", "");

                btnNew_Click(null, null);
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                //AllPageCode();

            }

        }
        getGridTotal();

        txtscanProduct.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        ObjTransferHeader.DeleteTransferHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
        FillGrid();
        DisplayMessage("Record Deleted");
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        if (Request.QueryString["Id"] == null)
        {
            btnSave.Visible = clsPagePermission.bAdd;
        }
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        IbtnReport.Visible = clsPagePermission.bPrint;
    }
    #endregion
    #region Bin Section
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (ddlFieldNameBin.SelectedItem.Value == "TDate")
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


        if (ddlOptionBin.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text + "%'";
            }
            DataTable dtTrans = (DataTable)Session["DtBinTransfer"];


            DataView view = new DataView(dtTrans, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvTransferBin, view.ToTable(), "", "");


            if (view.ToTable().Rows.Count == 0)
            {
                //imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
            // btnbindBin.Focus();

        }
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        txtValueBin.Text = "";
        txtValueBinDate.Text = "";
        txtValueBin.Visible = true;
        txtValueBinDate.Visible = false;
        lblSelectedRecord.Text = "";
        txtValueBin.Focus();
    }
    protected void gvTransferBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTransferBin.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvTransferBin, dt, "", "");

        }
        //AllPageCode();

        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvTransferBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvTransferBin.Rows[i].FindControl("lblTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvTransferBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

        gvTransferBin.BottomPagerRow.Focus();

    }
    protected void gvTransferBin_Sorting(object sender, GridViewSortEventArgs e)
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
        objPageCmn.FillData((object)gvTransferBin, dt, "", "");

        //AllPageCode();
        gvTransferBin.HeaderRow.Focus();

    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvTransferBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvTransferBin.Rows.Count; i++)
        {
            ((CheckBox)gvTransferBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvTransferBin.Rows[i].FindControl("lblTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvTransferBin.Rows[i].FindControl("lblTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvTransferBin.Rows[i].FindControl("lblTransId"))).Text.Trim().ToString())
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
        ((CheckBox)gvTransferBin.HeaderRow.FindControl("chkgvSelectAll")).Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvTransferBin.Rows[index].FindControl("lblTransId");
        if (((CheckBox)gvTransferBin.Rows[index].FindControl("chkgvSelect")).Checked)
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
        ((CheckBox)gvTransferBin.Rows[index].FindControl("chkgvSelect")).Focus();
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
            for (int i = 0; i < gvTransferBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvTransferBin.Rows[i].FindControl("lblTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvTransferBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
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
            objPageCmn.FillData((object)gvTransferBin, dtPr1, "", "");

            //AllPageCode();
            ViewState["Select"] = null;
        }


    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        bool Result = true;
        int b = 0;



        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(ObjTransferHeader.GetTransferHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, lblSelectedRecord.Text.Split(',')[j].Trim()).Rows[0]["TDate"].ToString()), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
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



        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {


                    b = ObjTransferHeader.DeleteTransferHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

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
            foreach (GridViewRow Gvr in gvTransferBin.Rows)
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
        txtValueBin.Focus();
    }


    public void FillGridBin()
    {

        DataTable dt = ObjTransferHeader.GetTransferHeaderFalse(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString());




        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvTransferBin, dt, "", "");


        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count;
        Session["DtBinTransfer"] = dt;
        Session["DtBinFilter"] = dt;
        if (dt.Rows.Count != 0)
        {
            //AllPageCode();
        }
        else
        {
            //imgBtnRestore.Visible = false;
            //ImgbtnSelectAll.Visible = false;
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
    protected void txtOutQty_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lb = (Label)row.FindControl("lblReqQty");

        LinkButton lnkAddSerial = (LinkButton)row.FindControl("lnkAddSerial");
        if (float.Parse(txt.Text.ToString()) > float.Parse(lb.Text.ToString()))
        {
            DisplayMessage("Out quantity  should not be greater than request quantity");
            txt.Text = "0";
            lnkAddSerial.Focus();
        }
        DataTable dt = objstock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, HttpContext.Current.Session["FinanceYearId"].ToString(), ((Label)row.FindControl("lblPId")).Text.ToString());
        if (dt.Rows.Count != 0)
        {
            if (float.Parse(txt.Text.ToString()) > float.Parse(dt.Rows[0]["Quantity"].ToString()))
            {

                DisplayMessage("Out quantity  should not be greater than System quantity,You have only " + dt.Rows[0]["Quantity"].ToString());
                txt.Text = "0";
                lnkAddSerial.Focus();
            }

            //here we check that if out qty is already reserver to anoth location than we can not allow here 
            //code start
            string strsql = string.Empty;

            strsql = "select Inv_TransferDetail.ProductId,Inv_TransferDetail.OutQty from Inv_TransferHeader inner join Inv_TransferDetail on Inv_TransferHeader.Trans_Id=Inv_TransferDetail.Transfer_Id where Inv_TransferHeader.Post='N' and Inv_TransferDetail.ProductId='" + ((Label)row.FindControl("lblPId")).Text.ToString() + "' and Inv_TransferDetail.FromLocationId=" + Session["LocId"].ToString() + " and Inv_TransferHeader.RequestNo<>" + ViewState["RequestId"].ToString() + " and Inv_TransferHeader.isActive='True'";

            ObjDa.return_DataTable(strsql);

            if (ObjDa.return_DataTable(strsql).Rows.Count > 0)
            {
                float totalreserverqty = 0;

                foreach (DataRow dr in ObjDa.return_DataTable(strsql).Rows)
                {
                    try
                    {
                        totalreserverqty += float.Parse(dr["OutQty"].ToString());
                    }
                    catch
                    {

                    }
                }

                if (float.Parse(txt.Text.ToString()) > (float.Parse(dt.Rows[0]["Quantity"].ToString()) - totalreserverqty))
                {

                    DisplayMessage(totalreserverqty.ToString() + " quantity already reserved to another reqeust ,now You have only " + (float.Parse(dt.Rows[0]["Quantity"].ToString()) - totalreserverqty));
                    txt.Text = "0";
                    lnkAddSerial.Focus();
                    return;
                }
            }

            //code end

        }
        getGridTotal();


    }
    public string SetDateFormat(string Date)
    {
        string DateFormat = string.Empty;
        DateFormat = Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString());

        return DateFormat;

    }

    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("Trans_Id");

        DtProduct.Columns.Add("Serial_No");
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("Unit_Id");
        DtProduct.Columns.Add("OutQty");
        DtProduct.Columns.Add("UnitCost");
        DtProduct.Columns.Add("ProductDescription");

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
            DisplayMessage("Select Unit");
            ddlUnit.SelectedIndex = 0;
            ddlUnit.Focus();
            return;

        }

        if (txtRequestQty.Text == "")
        {
            DisplayMessage("Enter Out Quantity");
            txtRequestQty.Text = "";
            txtRequestQty.Focus();
            return;
            // txtRequestQty.Text = "1";
        }
        else
        {
            if (txtRequestQty.Text == "0")
            {
                DisplayMessage("Enter Out Quantity");
                txtRequestQty.Text = "";
                txtRequestQty.Focus();
                return;

            }
        }


        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;

        if (editid.Value == "")
        {
            ReqId = ObjTransferHeader.getAutoId();
        }
        else
        {
            ReqId = editid.Value.ToString();
        }

        int serialNo = 0;
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
            DataTable dt = new DataView(ObjProductMaster.GetProductMasterAll(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductCode='" + txtProductcode.Text.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                ProductId = dt.Rows[0]["ProductId"].ToString();
            }

        }
        if (txtProductName.Text != "")
        {
            DataTable dt = new DataView(ObjProductMaster.GetProductMasterAll(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "EProductName='" + txtProductName.Text.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                ProductId = dt.Rows[0]["ProductId"].ToString();
            }

        }

        UnitId = ddlUnit.SelectedValue.ToString();

        if (hidProduct.Value == "" || hidProduct.Value == "0")
        {
            if (ViewState["Dtproduct"] == null)
            {

            }
            else
            {
                DtProduct = (DataTable)ViewState["Dtproduct"];
            }
            if (DtProduct.Rows.Count > 0)
            {
                DataTable Dt = new DataView(DtProduct, "", "Serial_No Desc", DataViewRowState.CurrentRows).ToTable();
                serialNo = Convert.ToInt32(Dt.Rows[0]["Serial_No"].ToString());
                serialNo += 1;
            }
            else
            {
                serialNo = 1;
            }
            if (ViewState["Dtproduct"] == null)
            {
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                dr["Serial_No"] = serialNo.ToString();
                dr["ProductId"] = ProductId.ToString();
                dr["Unit_Id"] = UnitId.ToString();
                dr["OutQty"] = txtRequestQty.Text.ToString();
                dr["ProductDescription"] = txtPDescription.Text;

                DtProduct.Rows.Add(dr);
                ViewState["Dtproduct"] = (DataTable)DtProduct;
            }
            else
            {
                DtProduct = (DataTable)ViewState["Dtproduct"];
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                dr["Serial_No"] = serialNo.ToString();
                dr["ProductId"] = ProductId.ToString();
                dr["Unit_Id"] = UnitId.ToString();
                dr["OutQty"] = txtRequestQty.Text.ToString();
                // dr["ProductDescription"] = txtPDescription.Text;

                DtProduct.Rows.Add(dr);
                ViewState["Dtproduct"] = (DataTable)DtProduct;

            }
        }
        else
        {
            serialNo = Convert.ToInt32(ViewState["SerialNo"].ToString());

            DataTable dt = (DataTable)ViewState["Dtproduct"];


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = DtProduct.NewRow();
                if (dt.Rows[i]["Trans_Id"].ToString() == hidProduct.Value)
                {
                    dr["Trans_Id"] = hidProduct.Value;
                    dr["Serial_No"] = serialNo.ToString();
                    dr["ProductId"] = ProductId.ToString();
                    dr["Unit_Id"] = UnitId.ToString();
                    dr["OutQty"] = txtRequestQty.Text.ToString();
                    dr["ProductDescription"] = txtPDescription.Text;

                    DtProduct.Rows.Add(dr);

                }
                else
                {
                    dr["Trans_Id"] = dt.Rows[i]["Trans_Id"].ToString();
                    dr["Serial_No"] = dt.Rows[i]["Serial_No"].ToString();
                    dr["ProductId"] = dt.Rows[i]["ProductId"].ToString();
                    dr["Unit_Id"] = dt.Rows[i]["Unit_Id"].ToString();
                    dr["OutQty"] = dt.Rows[i]["OutQty"].ToString();
                    //dr["ProductDescription"] = dt.Rows[i]["ProductDescription"].ToString();

                    DtProduct.Rows.Add(dr);
                }

            }
            PanelProduct4.Visible = false;
            PanelProduct5.Visible = false;
        }

        ViewState["Dtproduct"] = (DataTable)DtProduct;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvProductRequest, DtProduct, "", "");

        //AllPageCode();
        ResetDetail();
        txtProductcode.Focus();
        getGridTotal();
    }
    protected void btnproductReset_Click(object sender, EventArgs e)
    {
        ResetDetail();
    }
    protected void txtLocationName_TextChanged(object sender, EventArgs e)
    {

        if (txtLocationNameOut.Text != "")
        {
            DataTable Dtlocation = objLocation.GetLocationMasterByLocationName(Session["CompId"].ToString(), txtLocationNameOut.Text);
            if (Dtlocation.Rows.Count == 0)
            {
                DisplayMessage("Select Location");
                txtLocationNameOut.Focus();
                txtLocationNameOut.Text = "";
                return;
            }

        }

    }

    #region stockable with serial number
    public void UpdateRecordForStckableItem(string ProductId, string ItemType, float Quantity, string TransId, string UnitId, ref SqlTransaction trns)
    {

        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        float Currencyquantity = 0;

        DataTable dt = new DataTable();
        dt = ObjStockBatchMaster.GetStockBatchMasterAll_By_CompanyId_BrandId_LocationId_ProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ProductId, ref trns);
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
            string sql = "select SUM(quantity) from Inv_StockBatchMaster where Field3='" + dt.Rows[i]["Trans_Id"].ToString() + "' and  ProductId=" + ProductId + " and InOut='O'";


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
                            ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", TransId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            Quantity = 0;
                        }
                        else
                        {

                            ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", TransId, ProductId, UnitId, "O", "0", "0", Remqty.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            Quantity = Quantity - Remqty;

                        }

                    }
                }
                catch
                {
                    if (Currencyquantity > Quantity)
                    {

                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", TransId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                        Quantity = 0;
                    }
                    else
                    {
                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", TransId, ProductId, UnitId, "O", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                        Quantity = Quantity - Currencyquantity;

                    }

                }

            }
            else
            {
                if (Currencyquantity > Quantity)
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", TransId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                    Quantity = 0;
                }
                else
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), StrLocId.ToString(), "TO", TransId, ProductId, UnitId, "O", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", "0", "", dt.Rows[i]["Trans_Id"].ToString(), "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    Quantity = Quantity - Currencyquantity;

                }
            }
        }
    }

    protected void lnkAddSerial_OnClick(object sender, EventArgs e)
    {
        lblCount.Text = Resources.Attendance.Serial_No + ":-";
        txtCount.Text = "0";
        ViewState["RowIndex"] = null;
        lblProductIdvalue.Text = txtProductcode.Text;
        lblProductNameValue.Text = txtProductName.Text;
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        txtSerialNo.Text = "";
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

        pnlProduct1.Visible = true;
        pnlProduct2.Visible = true;
        PanelProduct4.Visible = false;
        PanelProduct5.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Popup()", true);

    }
    protected void lnkAddSerial_Command(object sender, EventArgs e)
    {
        lblCount.Text = Resources.Attendance.Serial_No + ":-";
        txtCount.Text = "0";

        GridViewRow Row = (GridViewRow)((LinkButton)sender).Parent.Parent;
        ViewState["RowIndex"] = Row.RowIndex;
        lblProductIdvalue.Text = ((Label)Row.FindControl("lblGvProductCode")).Text;
        lblProductNameValue.Text = ((Label)Row.FindControl("lblProductId")).Text;
        lblREquestQtyValue.Text = ((Label)Row.FindControl("lblReqQty")).Text;
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        txtSerialNo.Text = "";
        ViewState["PID"] = ((Label)Row.FindControl("lblPID")).Text;
        if (ViewState["dtFinal"] == null)
        {

        }
        else
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["dtFinal"];


            dt = new DataView(dt, "Product_Id='" + ViewState["PID"].ToString() + "' and LotNo='" + ViewState["RowIndex"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
            ViewState["Tempdt"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

            ViewState["dtSerial"] = dt;
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        }

        txtSerialNo.Focus();

        pnlProduct1.Visible = true;
        pnlProduct2.Visible = true;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Popup()", true);

    }
    protected void BtnSerialSave_Click(object sender, EventArgs e)
    {
        string TransId = string.Empty;
        TransId = "0";

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
                        string[] result = isSerialNumberValid(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                        if (result[0] == "VALID")
                        {
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = ViewState["PID"].ToString();
                            dr[1] = txt[i].ToString();
                            dr[2] = "0";
                            //for batch number
                            dr[4] = result[4].ToString();

                            dr[5] = ViewState["RowIndex"].ToString();
                            //for expiry date
                            dr[6] = result[3].ToString();
                            //for Manufacturer date
                            dr[7] = result[2].ToString();

                            counter++;

                        }
                        else if (result[0].ToString() == "ALREADY OUT")
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

                        string[] result = isSerialNumberValid(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                        if (result[0] == "VALID")
                        {
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = ViewState["PID"].ToString();
                            dr[1] = txt[i].ToString();
                            dr[2] = "0";
                            //for batch number
                            dr[4] = result[4].ToString();

                            dr[5] = ViewState["RowIndex"].ToString();
                            //for expiry date
                            dr[6] = result[3].ToString();
                            //for Manufacturer date
                            dr[7] = result[2].ToString();

                            counter++;
                        }
                        else if (result[0].ToString() == "ALREADY OUT")
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
                Message += "Following serial Number is Already Out from the stock=" + DuplicateserialNo;
            }
            if (serialNoExists != "")
            {


                if (Message == "")
                {
                    Message += "Following serial number not exists in stock=" + serialNoExists;
                }
                else
                {
                    Message += Environment.NewLine + "Following serial number not exists in stock=" + serialNoExists;
                }
            }

            DisplayMessage(Message);
        }
        //here we check that sales quantity should be less than system quantity
        //this validation is add by jitendra upadhyay on 22-05-2015
        //code start



        if (ViewState["RowIndex"] != null)
        {
            if (GvProduct.Rows.Count > 0)
            {
                TextBox txtoutqty = (TextBox)GvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtOutQty");
                Label lb = (Label)GvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblReqQty");
                txtoutqty.Text = dt.Rows.Count.ToString();
                if (float.Parse(txtoutqty.Text.ToString()) > float.Parse(lb.Text.ToString()))
                {
                    DisplayMessage("Out quantity  should not be greater than request quantity");
                    txtoutqty.Text = "0";
                    ViewState["dtSerial"] = null;
                    //  lnkAddSerial.Focus();
                    return;
                }
                DataTable dtCheck = objstock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, HttpContext.Current.Session["FinanceYearId"].ToString(), ((Label)GvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblPId")).Text.ToString());

                if (dtCheck.Rows.Count != 0)
                {
                    if (float.Parse(txtoutqty.Text.ToString()) > float.Parse(dtCheck.Rows[0]["Quantity"].ToString()))
                    {

                        DisplayMessage("Out quantity  should not be greater than System quantity,You have only " + dtCheck.Rows[0]["Quantity"].ToString());
                        txtoutqty.Text = "0";
                        ViewState["dtSerial"] = null;

                        //    lnkAddSerial.Focus();
                        return;
                    }

                    //here we check that if out qty is already reserver to anoth location than we can not allow here 
                    //code start
                    string strsql = string.Empty;

                    strsql = "select Inv_TransferDetail.ProductId,Inv_TransferDetail.OutQty from Inv_TransferHeader inner join Inv_TransferDetail on Inv_TransferHeader.Trans_Id=Inv_TransferDetail.Transfer_Id where Inv_TransferHeader.Post='N' and Inv_TransferDetail.ProductId='" + ((Label)GvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblPId")).Text.ToString() + "' and Inv_TransferHeader.Fromlocationid=" + Session["LocId"].ToString() + " and Inv_TransferHeader.RequestNo<>" + ViewState["RequestId"].ToString() + " and  Inv_TransferHeader.isactive='True'";
                    ObjDa.return_DataTable(strsql);

                    if (ObjDa.return_DataTable(strsql).Rows.Count > 0)
                    {
                        float totalreserverqty = 0;

                        foreach (DataRow dr in ObjDa.return_DataTable(strsql).Rows)
                        {
                            try
                            {
                                totalreserverqty += float.Parse(dr["OutQty"].ToString());
                            }
                            catch
                            {

                            }
                        }


                        if (float.Parse(txtoutqty.Text.ToString()) > (float.Parse(dtCheck.Rows[0]["Quantity"].ToString()) - totalreserverqty))
                        {

                            DisplayMessage(totalreserverqty.ToString() + " quantity already reserved to another reqeust ,now You have only " + (float.Parse(dtCheck.Rows[0]["Quantity"].ToString()) - totalreserverqty));
                            txtoutqty.Text = "0";
                            ViewState["dtSerial"] = null;
                            return;
                        }
                    }

                    //code end

                }

            }
            if (gvEditProduct.Rows.Count > 0)
            {
                TextBox txtEDItoutqty = (TextBox)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtOutQty");
                Label EDItlb = (Label)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblReqQty");
                txtEDItoutqty.Text = dt.Rows.Count.ToString();
                if (float.Parse(txtEDItoutqty.Text.ToString()) > float.Parse(EDItlb.Text.ToString()))
                {
                    DisplayMessage("Out quantity  should not be greater than request quantity");
                    txtEDItoutqty.Text = "0";
                    ViewState["dtSerial"] = null;

                    //  lnkAddSerial.Focus();
                    return;
                }

                DataTable dtCheck = objstock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, HttpContext.Current.Session["FinanceYearId"].ToString(), ((Label)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblPId")).Text.ToString());

                if (dtCheck.Rows.Count != 0)
                {
                    if (float.Parse(txtEDItoutqty.Text.ToString()) > float.Parse(dtCheck.Rows[0]["Quantity"].ToString()))
                    {

                        DisplayMessage("Out quantity  should not be greater than System quantity,You have only " + dtCheck.Rows[0]["Quantity"].ToString());
                        txtEDItoutqty.Text = "0";
                        ViewState["dtSerial"] = null;
                        return;
                    }
                    //here we check that if out qty is already reserver to anoth location than we can not allow here 
                    //code start
                    string strsql = string.Empty;

                    strsql = "select Inv_TransferDetail.ProductId,Inv_TransferDetail.OutQty from Inv_TransferHeader inner join Inv_TransferDetail on Inv_TransferHeader.Trans_Id=Inv_TransferDetail.Transfer_Id where Inv_TransferHeader.Post='N' and Inv_TransferDetail.ProductId='" + ((Label)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblPId")).Text.ToString() + "'  and Inv_TransferHeader.Fromlocationid=" + Session["LocId"].ToString() + " and Inv_TransferHeader.RequestNo<>" + ViewState["RequestId"].ToString() + " and  Inv_TransferHeader.isactive='True'";
                    ObjDa.return_DataTable(strsql);

                    if (ObjDa.return_DataTable(strsql).Rows.Count > 0)
                    {
                        float totalreserverqty = 0;

                        foreach (DataRow dr in ObjDa.return_DataTable(strsql).Rows)
                        {
                            try
                            {
                                totalreserverqty += float.Parse(dr["OutQty"].ToString());
                            }
                            catch
                            {

                            }
                        }
                        if (float.Parse(txtEDItoutqty.Text.ToString()) > (float.Parse(dtCheck.Rows[0]["Quantity"].ToString()) - totalreserverqty))
                        {

                            DisplayMessage(totalreserverqty.ToString() + " quantity already reserved to another reqeust ,now You have only " + (float.Parse(dtCheck.Rows[0]["Quantity"].ToString()) - totalreserverqty));
                            txtEDItoutqty.Text = "0";
                            ViewState["dtSerial"] = null;
                            return;
                        }
                    }
                    //code end
                }

            }

            if (gvProductRequest.Rows.Count > 0)
            {
                //Label txtoutqty = (Label)gvProductRequest.Rows[Row].FindControl("lblReqQty");

                TextBox lb = (TextBox)gvProductRequest.Rows[(int)ViewState["RowIndex"]].FindControl("lblReqQty");

                lb.Text = dt.Rows.Count.ToString();

                DataTable dtCheck = objstock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, HttpContext.Current.Session["FinanceYearId"].ToString(), ((Label)GvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblPId")).Text.ToString());



                if (dtCheck.Rows.Count != 0)
                {
                    if (float.Parse(lb.Text.ToString()) > float.Parse(dtCheck.Rows[0]["Quantity"].ToString()))
                    {

                        DisplayMessage("Out quantity  should not be greater than System quantity,You have only " + dtCheck.Rows[0]["Quantity"].ToString());
                        lb.Text = "0";
                        ViewState["dtSerial"] = null;

                        //    lnkAddSerial.Focus();
                        return;
                    }
                }

            }
        }
        else
        {//use this function when we add in product panelk
            DataTable dtCheck = objstock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, HttpContext.Current.Session["FinanceYearId"].ToString(), ((Label)GvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblPId")).Text.ToString());

            if (dtCheck.Rows.Count != 0)
            {

                if (float.Parse(dt.Rows.Count.ToString()) > float.Parse(dtCheck.Rows[0]["Quantity"].ToString()))
                {
                    DisplayMessage("Out quantity  should not be greater than System quantity,You have only " + dtCheck.Rows[0]["Quantity"].ToString());
                    txtRequestQty.Text = "0";
                    ViewState["dtSerial"] = null;
                    return;
                }

            }
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

            Dtfinal = new DataView(Dtfinal, "LotNo<>'" + ViewState["RowIndex"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();


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

        if (ViewState["RowIndex"] != null)
        {
            if (GvProduct.Rows.Count > 0)
            {
                ((TextBox)GvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtOutQty")).Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();
            }
            if (gvEditProduct.Rows.Count > 0)
            {
                ((TextBox)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtOutQty")).Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();
            }
            if (gvProductRequest.Rows.Count > 0)
            {
                ((TextBox)gvProductRequest.Rows[(int)ViewState["RowIndex"]].FindControl("lblReqQty")).Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();
            }
        }
        else
        {
            txtRequestQty.Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();

        }
        txtSerialNo.Text = "";
        txtCount.Text = "0";
        txtSerialNo.Focus();
        getGridTotal();
    }

    public static string[] isSerialNumberValid(string serialNumber, string ProductId, string TransId, GridView gvSerialNumber)
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

            if (dtserial.Rows.Count > 0)
            {

                if (dtserial.Rows[0]["InOut"].ToString() == "O")
                {
                    Result[0] = "ALREADY OUT";
                }
                else if (dtserial.Rows[0]["InOut"].ToString() == "I")
                {
                    Result[0] = "VALID";
                    Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                    Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[4] = dtserial.Rows[0]["BatchNo"].ToString();
                }
                else
                {
                    //DataTable dtCopy = dtserial.Copy();

                    //dtserial = new DataView(dtserial, "InOut='O' and (TransType='SI' or TransType='DV')  and TransTypeId<>" + TransId + "", "", DataViewRowState.CurrentRows).ToTable();
                    //if (dtserial.Rows.Count > 0)
                    //{

                    Result[0] = "ALREADY OUT";

                    //}

                    //else
                    //{
                    //    dtserial = dtCopy.Copy();
                    //    dtserial = new DataView(dtserial, "InOut='I'", "", DataViewRowState.CurrentRows).ToTable();
                    //    if (dtserial.Rows.Count > 0)
                    //    {

                    //        Result[0] = "VALID";
                    //        Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                    //        Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    //        Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    //        Result[4] = dtserial.Rows[0]["BatchNo"].ToString();





                    //    }
                    //}
                }
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
        if (GvProduct.Rows.Count > 0)
        {
            ((TextBox)GvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtOutQty")).Text = "0";
        }
        if (gvEditProduct.Rows.Count > 0)
        {
            ((TextBox)gvEditProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtOutQty")).Text = "0";
        }
        if (gvProductRequest.Rows.Count > 0)
        {
            ((TextBox)gvProductRequest.Rows[(int)ViewState["RowIndex"]].FindControl("lblReqQty")).Text = "0";
        }

        txtSerialNo.Focus();
    }
    protected void btncloseserial_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = false;
        pnlProduct2.Visible = false;

        if (ViewState["RowIndex"] == null)
        {
            PanelProduct4.Visible = true;
            PanelProduct5.Visible = true;
        }
        //lblDuplicateserialNo.Text = "";
        ViewState["dtSerial"] = null;

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Close_Modal_Popup()", true);

    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = false;
        pnlProduct2.Visible = false;
        if (ViewState["RowIndex"] == null)
        {
            PanelProduct4.Visible = true;
            PanelProduct5.Visible = true;
        }
        ViewState["dtSerial"] = null;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Close_Modal_Popup()", true);
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
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dtSerial"];

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["dtSerial"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

        lblSelectedRecord.Text = "";
        //AllPageCode();
    }

    protected void btnexecute_Click(object sender, EventArgs e)
    {
        string TransId = string.Empty;
        if (editid.Value == "")
        {
            TransId = "0";
        }
        else
        {
            TransId = editid.Value;
        }

        int counter = 0;
        txtSerialNo.Text = "";


        DataTable dtSockCopy = new DataTable();

        DataTable dtstock = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductId(ViewState["PID"].ToString());

        //DataTable dtstock = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductIdForSalesInvoice(ViewState["PID"].ToString(), Session["LocId"].ToString());


        try
        {




            try
            {
                dtstock = new DataView(dtstock, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            dtstock = dtstock.DefaultView.ToTable(true, "SerialNo");


            for (int i = 0; i < dtstock.Rows.Count; i++)
            {

                string[] result = isSerialNumberValid(dtstock.Rows[i]["SerialNo"].ToString(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                if (result[0] == "VALID")
                {
                    if (txtSerialNo.Text == "")
                    {
                        txtSerialNo.Text = dtstock.Rows[i]["SerialNo"].ToString();

                    }
                    else
                    {
                        txtSerialNo.Text += Environment.NewLine + dtstock.Rows[i]["SerialNo"].ToString();


                    }
                    counter++;

                }



            }

        }
        catch
        {
        }



        txtCount.Text = counter.ToString();
        if (counter == 0)
        {
            DisplayMessage("Serial Number Not Found");
        }




    }

    #endregion

    protected void txtRequestQty_OnTextChanged(object sender, EventArgs e)
    {
        if (txtProductcode.Text == "" || txtProductName.Text == "")
        {
            DisplayMessage("Product Not Found");
            txtProductcode.Focus();
            return;
        }


        if (txtRequestQty.Text != "")
        {

            DataTable dtCheck = objstock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, HttpContext.Current.Session["FinanceYearId"].ToString(), ViewState["PID"].ToString());


            if (dtCheck.Rows.Count != 0)
            {
                if (float.Parse(txtRequestQty.Text.ToString()) > float.Parse(dtCheck.Rows[0]["Quantity"].ToString()))
                {

                    DisplayMessage("Out quantity  should not be greater than System quantity,You have only " + dtCheck.Rows[0]["Quantity"].ToString());
                    txtRequestQty.Text = "0";
                    return;
                }
            }
        }

    }
    protected void btnTRRefresh_Click(object sender, EventArgs e)
    {
        txtTrValue.Text = "";
        ddlTRFieldName.SelectedIndex = 0;
        ddlTROption.SelectedIndex = 3;
        txtTrValue.Focus();
        FillTransferRequestgrid();
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
    }

    protected void btnTRbind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        if (ddlTROption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlTROption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlTRFieldName.SelectedValue + ",System.String)='" + txtTrValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlTRFieldName.SelectedValue + ",System.String) like '%" + txtTrValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlTRFieldName.SelectedValue + ",System.String) Like '" + txtTrValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["DtTransferRequest"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["DtTRFilter"] = view.ToTable();
            btnTRTotal.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvTransferRequest, view.ToTable(), "", "");

            //AllPageCode();

            btnbind.Focus();

        }

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListLocationName(string prefixText, int count, string contextKey)
    {
        LocationMaster objLocationMaster = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = new DataView(objLocationMaster.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString()), "Location_Name like '%" + prefixText.ToString() + "%'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Location_Name"].ToString();
        }

        return txt;
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductcode.Text != "")
        {
            DataTable dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, txtProductcode.Text.ToString());
            if (dt == null)
            {
                ResetDetail();
                return;
            }

            //DataTable dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocationId, txtProductcode.Text.ToString());
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
                    lnkAddSerial.Visible = false;
                    txtRequestQty.Enabled = false;
                    return;
                }

                if (ViewState["Dtproduct"] != null)
                {
                    DataTable Dt = (DataTable)ViewState["Dtproduct"];
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
                        DisplayMessage("Product already exists!");
                        txtProductcode.Text = "";
                        txtProductcode.Focus();
                        txtProductName.Text = "";
                        lnkAddSerial.Visible = false;
                        txtRequestQty.Enabled = false;
                        return;

                    }
                }
                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();

                FillUnit(dt.Rows[0]["ProductId"].ToString());
                ViewState["PID"] = dt.Rows[0]["ProductId"].ToString();
                txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                if (dt.Rows[0]["MaintainStock"].ToString().Trim().ToUpper() == "SNO")
                {
                    lnkAddSerial.Visible = true;
                    txtRequestQty.Enabled = false;
                }
                else
                {
                    lnkAddSerial.Visible = false;
                    txtRequestQty.Enabled = true;
                }
                txtUnitCost.Focus();

            }
            else
            {
                DisplayMessage("Select Product");
                ddlUnit.Items.Clear();
                txtProductName.Text = "";
                txtProductcode.Text = "";
                txtPDescription.Text = "";
                txtProductcode.Focus();
                lnkAddSerial.Visible = false;
                txtRequestQty.Enabled = false;
                return;

            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            ddlUnit.Items.Clear();
            lnkAddSerial.Visible = false;
            txtRequestQty.Enabled = false;
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }



    }
    public void ResetDetail()
    {
        txtProductName.Text = "";
        txtUnitCost.Text = "0";
        ddlUnit.Items.Clear();
        txtRequestQty.Text = "0";
        hidProduct.Value = "";
        txtPDescription.Text = "";
        txtProductcode.Text = "";
        txtProductcode.Focus();
        lnkAddSerial.Visible = false;
        txtRequestQty.Enabled = false;

    }

    public void FillUnit(string ProductId)
    {
        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());
    }

    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        //btnClosePanel_Click(null, null);
        ImgProductClose_Click(null, null);
        ResetDetail();

    }
    protected void ImgProductClose_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = false;
        PanelProduct4.Visible = false;
        PanelProduct5.Visible = false;
        lnkAddSerial.Visible = false;
        ddlUnit.Items.Clear();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Close_Modal_Popup()", true);
    }
    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        PanelProduct4.Visible = true;
        PanelProduct5.Visible = true;
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
                dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, txtProductName.Text.ToString());
            }
            catch
            {
            }

            if (dt == null)
            {
                ResetDetail();
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
                    lnkAddSerial.Visible = false;
                    txtRequestQty.Enabled = false;
                    return;

                }

                if (ViewState["Dtproduct"] != null)
                {
                    DataTable Dt = (DataTable)ViewState["Dtproduct"];
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
                        DisplayMessage("Product already exists!");
                        txtProductName.Text = "";
                        txtProductName.Focus();
                        txtProductName.Text = "";
                        lnkAddSerial.Visible = false;
                        txtRequestQty.Enabled = false;
                        return;

                    }
                }
                if (dt.Rows[0]["MaintainStock"].ToString().Trim().ToUpper() == "SNO")
                {
                    lnkAddSerial.Visible = true;
                    txtRequestQty.Enabled = false;
                }
                else
                {
                    lnkAddSerial.Visible = false;
                    txtRequestQty.Enabled = true;
                }

                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();

                FillUnit(dt.Rows[0]["ProductId"].ToString());
                ViewState["PID"] = dt.Rows[0]["ProductId"].ToString();

                txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                txtUnitCost.Focus();

            }
            else
            {
                DisplayMessage("Select Product");
                ddlUnit.Items.Clear();
                txtProductName.Text = "";
                txtProductcode.Text = "";
                txtPDescription.Text = "";
                txtProductName.Focus();
                lnkAddSerial.Visible = false;
                txtRequestQty.Enabled = false;
                return;
                //FillUnit();
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            ddlUnit.Items.Clear();
            lnkAddSerial.Visible = false;
            txtRequestQty.Enabled = false;
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
    }

    protected void btnEditProduct_Command(object sender, CommandEventArgs e)
    {
        PanelProduct4.Visible = true;
        PanelProduct5.Visible = true;
        hidProduct.Value = e.CommandArgument.ToString();

        DataTable dtproduct = new DataTable();
        dtproduct = (DataTable)ViewState["Dtproduct"];

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

            ddlUnit.SelectedValue = dt.Rows[0]["Unit_Id"].ToString();
            //txtPDescription.Text = dt.Rows[0]["ProductDescription"].ToString();
            txtRequestQty.Text = dt.Rows[0]["OutQty"].ToString();
            txtUnitCost.Text = dt.Rows[0]["UnitCost"].ToString();
            ViewState["SerialNo"] = dt.Rows[0]["Serial_No"].ToString();
        }
        txtProductcode.Focus();
    }
    protected void btnDeleteProduct_Command(object sender, CommandEventArgs e)
    {

        DataTable dtproduct = (DataTable)ViewState["Dtproduct"];
        DataTable dt = new DataView(dtproduct, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvProductRequest, dt, "", "");

        ViewState["Dtproduct"] = (DataTable)dt;
        //AllPageCode();
        getGridTotal();
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
        return ProductName;
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

    #region printreport

    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + e.CommandArgument.ToString() + "&&Type=TO','window','width=1024');", true);
    }
    #endregion


    #region Post

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        //AllPageCode();
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



            DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTransferOutScanning");
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    ((TextBox)e.Row.FindControl("txtOutQty")).Enabled = false;
                    ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
                }
            }


            if (editid.Value != "")
            {

                if (((TextBox)e.Row.FindControl("txtOutQty")).Text != "")
                {
                    if (float.Parse(((TextBox)e.Row.FindControl("txtOutQty")).Text) > 0)
                    {
                        ((TextBox)e.Row.FindControl("txtOutQty")).Enabled = true;
                        ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
                    }
                }

            }



            //try
            //{
            //    strMaintainStock = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString();
            //}
            //catch
            //{
            //}

            //if (strMaintainStock.Trim() == "SNO")
            //{

            //    if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Serial on Transfer Voucher").Rows[0]["ParameterValue"].ToString()))
            //    {
            //        ((TextBox)e.Row.FindControl("txtOutQty")).Enabled = false;

            //        ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
            //    }
            //    else
            //    {
            //        ((TextBox)e.Row.FindControl("txtOutQty")).Enabled = true;

            //        ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
            //    }

            //}
            //else
            //{
            //    ((TextBox)e.Row.FindControl("txtOutQty")).Enabled = true;

            //    ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
            //}


            DataTable dt = objstock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), StrLocId, HttpContext.Current.Session["FinanceYearId"].ToString(), ProductID);

            if (dt.Rows.Count == 0 || Convert.ToDouble(dt.Rows[0]["Quantity"].ToString()) <= 0)
            {
                e.Row.Enabled = false;

            }
            else
            {
                e.Row.Enabled = true;
            }
        }
    }
    protected void gvProductRequest_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            string ProductID = ((Label)e.Row.FindControl("lblPID")).Text;


            if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
            {
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Serial on Transfer Voucher").Rows[0]["ParameterValue"].ToString()))
                {
                    ((TextBox)e.Row.FindControl("lblReqQty")).Enabled = false;

                    ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
                }
                else
                {
                    ((TextBox)e.Row.FindControl("lblReqQty")).Enabled = true;

                    ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
                }
            }
            else
            {
                ((TextBox)e.Row.FindControl("lblReqQty")).Enabled = true;

                ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
            }
        }
    }
    #endregion

    #region getGridCalculation
    public void getGridTotal()
    {
        float fRequest = 0;
        float fout = 0;
        foreach (GridViewRow gvrow in gvProductRequest.Rows)
        {

            try
            {
                fout += float.Parse(((TextBox)gvrow.FindControl("lblReqQty")).Text);
            }
            catch
            {
                fout += 0;
            }
        }

        foreach (GridViewRow gvrow in GvProduct.Rows)
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
                fout += float.Parse(((TextBox)gvrow.FindControl("txtOutQty")).Text);
            }
            catch
            {
                fout += 0;
            }
        }
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
                fout += float.Parse(((TextBox)gvrow.FindControl("txtOutQty")).Text);
            }
            catch
            {
                fout += 0;
            }
        }



        try
        {
            ((Label)gvProductRequest.FooterRow.FindControl("txttotqtyShow")).Text = fout.ToString();
        }
        catch
        {
        }

        try
        {
            ((Label)GvProduct.FooterRow.FindControl("txttotReqqtyShow")).Text = fRequest.ToString();
            ((Label)GvProduct.FooterRow.FindControl("txttotoutqtyShow")).Text = fout.ToString();
        }
        catch
        {
        }

        try
        {
            ((Label)gvEditProduct.FooterRow.FindControl("txttotReqqtyShow")).Text = fRequest.ToString();
            ((Label)gvEditProduct.FooterRow.FindControl("txttotoutqtyShow")).Text = fout.ToString();
        }
        catch
        {
        }
    }

    protected void txtscanProduct_TextChanged(object sender, EventArgs e)
    {

        btnscanserial_OnClick(null, null);
    }
    #endregion

    #region DecimlaFormat
    public string GetAmountDecimal(string Amount)
    {

        return ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Amount);

    }
    #endregion

    #region LOcationStock
    protected void btnCloseStockPanel_Click(object sender, EventArgs e)
    {
        pnlStock1.Visible = false;
        pnlStock2.Visible = false;
    }
    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        modelSA.getProductDetail(e.CommandArgument.ToString(), "", "");
    }

    public string GetStock(string strProductId)
    {

        string sysqty = string.Empty;
        try
        {
            sysqty = objstock.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), strProductId).Rows[0]["Quantity"].ToString();

        }
        catch
        {
            sysqty = "0";
        }

        if (sysqty == "")
        {
            sysqty = "0.000";
        }

        return GetAmountDecimal(sysqty);
    }



    #endregion

    #region TransferOutreport

    protected void IbtnReport_Command(object sender, EventArgs e)
    {
        Session["DateCrediteria"] = "";
        string strsql = string.Empty;
        strsql = "select th.Trans_Id,th.VoucherNo,th.TDate as VoucherDate,th.Remark,LM.Location_Name as Location_To,lm.Field1 as Currency_Id,pm.ProductCode,pm.EProductName,um.Unit_Name,TD.RequestQty,TD.OutQty,TD.ReceivedQty,Inv_StockDetail.Field1 as UnitCost,(TD.ReceivedQty*cast(Inv_StockDetail.Field1 as numeric(18,6))) as LineTotal  from Inv_TransferHeader as TH  inner join Inv_TransferDetail as TD on TH.Trans_Id=TD.Transfer_Id inner join Inv_ProductMaster as PM on td.ProductId=PM.ProductId inner join Inv_UnitMaster as UM on td.Unit_Id=UM.Unit_Id left join Inv_StockDetail  on td.ProductId=Inv_StockDetail.ProductId and Inv_StockDetail.Location_Id=td.FromLocationID   and Inv_StockDetail.Finance_Year_Id=" + Session["FinanceYearId"].ToString() + " inner join Set_LocationMaster as LM on th.ToLocationID=LM.Location_Id where TH.IsActive='True' and TH.Post='Y' and TH.FromLocationID=" + Session["LocId"].ToString() + "";
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
        strsql = strsql + " order by th.TDate ";
        DataTable dt = objDa.return_DataTable(strsql);
        Session["dtTranferOut"] = dt;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=0&&Type=TO','window','width=1024');", true);
    }

    private void FillddlLocation()
    {

        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ") and Location_Id<>" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtLoc = new DataView(dtLoc, "Location_Id<>" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

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


    #endregion

    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedItem.Value == "TDate")
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
    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "TDate")
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
    }
    #endregion



    #region Scanningsolution

    protected void btnscanserial_OnClick(object sender, EventArgs e)
    {
        bool IsSerial = false;
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
            if (dt.Rows[0]["ProductId"].ToString() != "")
            {
                DataTable dtStock = objDa.return_DataTable("Select  Quantity from  Inv_StockDetail where ProductId='" + dt.Rows[0]["ProductId"].ToString() + "' And Company_Id='" + Session["CompId"].ToString() + "' And Brand_Id='" + Session["BrandId"].ToString() + "' And Location_Id='" + Session["LocId"].ToString() + "' And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "' ");
                if (dtStock.Rows.Count > 0)
                {
                    try
                    {
                        float Quantity = float.Parse(dtStock.Rows[0]["Quantity"].ToString());
                        if (Quantity > 0)
                        {

                        }
                        else
                        {
                            DisplayMessage("Stock Not available");
                            txtscanProduct.Focus();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Stock Not available");
                        txtscanProduct.Focus();
                        return;
                    }
                }
                else
                {
                    DisplayMessage("Stock Not available");
                    txtscanProduct.Focus();
                    return;
                }
            }

            if (dt.Rows[0]["Type"].ToString().Trim() == "2")
            {
                IsSerial = true;
                if (!Inventory_Common.CheckValidSerialForSalesinvoice(dt.Rows[0]["ProductId"].ToString(), txtscanProduct.Text.ToString()))
                {
                    DisplayMessage("Serial number is invalid");
                    txtscanProduct.Text = "";
                    txtscanProduct.Focus();
                    return;
                }

                if (ViewState["dtFinal"] != null)
                {
                    if (new DataView((DataTable)ViewState["dtFinal"], "SerialNo='" + txtscanProduct.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        DisplayMessage("Serial number is already exists");
                        txtscanProduct.Text = "";
                        txtscanProduct.Focus();
                        return;

                    }
                }
            }




            if (editid.Value == "")
            {
                foreach (GridViewRow gvrow in GvProduct.Rows)
                {

                    if (((TextBox)gvrow.FindControl("txtOutQty")).Text == "")
                    {
                        ((TextBox)gvrow.FindControl("txtOutQty")).Text = "0";
                    }
                    if (((Label)gvrow.FindControl("lblGvProductCode")).Text == dt.Rows[0]["ProductCode"].ToString())
                    {
                        ((TextBox)gvrow.FindControl("txtOutQty")).Enabled = true;

                        if (float.Parse(((TextBox)gvrow.FindControl("txtOutQty")).Text) + 1 <= float.Parse(((Label)gvrow.FindControl("lblReqQty")).Text))
                        {
                            ((TextBox)gvrow.FindControl("txtOutQty")).Text = GetAmountDecimal((float.Parse(((TextBox)gvrow.FindControl("txtOutQty")).Text) + 1).ToString());
                        }


                        if (IsSerial)
                        {
                            addSerialFnc(dt.Rows[0]["ProductId"].ToString(), txtscanProduct.Text.Trim(), gvrow.RowIndex);
                        }

                        string serialno = objDa.get_SingleValue("Select MaintainStock from Inv_ProductMaster where ProductId='" + dt.Rows[0]["ProductId"].ToString() + "'");
                        if (serialno == "SNO")
                        {
                            ((LinkButton)gvrow.FindControl("lnkAddSerial")).Visible = true;
                        }
                        else
                        {

                            ((LinkButton)gvrow.FindControl("lnkAddSerial")).Visible = false;
                        }
                        counter = 1;
                    }
                }
            }
            else
            {

                foreach (GridViewRow gvrow in gvEditProduct.Rows)
                {
                    if (((TextBox)gvrow.FindControl("txtOutQty")).Text == "")
                    {
                        ((TextBox)gvrow.FindControl("txtOutQty")).Text = "0";
                    }
                    if (((Label)gvrow.FindControl("lblGvProductCode")).Text == dt.Rows[0]["ProductCode"].ToString())
                    {
                        if (float.Parse(((TextBox)gvrow.FindControl("txtOutQty")).Text) + 1 <= float.Parse(((Label)gvrow.FindControl("lblReqQty")).Text))
                        {
                            ((TextBox)gvrow.FindControl("txtOutQty")).Text = GetAmountDecimal((float.Parse(((TextBox)gvrow.FindControl("txtOutQty")).Text) + 1).ToString());
                        }

                        if (IsSerial)
                        {
                            addSerialFnc(dt.Rows[0]["ProductId"].ToString(), txtscanProduct.Text.Trim(), gvrow.RowIndex);
                        }
                        string serialno = objDa.get_SingleValue("Select MaintainStock from Inv_ProductMaster where ProductCode='" + dt.Rows[0]["ProductId"].ToString() + "'");
                        if (serialno == "SNO")
                        {
                            ((LinkButton)gvrow.FindControl("lnkAddSerial")).Visible = true;
                        }
                        else
                        {

                            ((LinkButton)gvrow.FindControl("lnkAddSerial")).Visible = false;
                        }
                        ((TextBox)gvrow.FindControl("txtOutQty")).Enabled = true;
                        //((LinkButton)gvrow.FindControl("lnkAddSerial")).Visible = true;
                        counter = 1;
                    }
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


    public void addSerialFnc(string strProductId, string strSerialNo, int RowIndex)
    {

        DataTable dt = new DataTable();

        if (ViewState["dtFinal"] == null)
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
            dt = (DataTable)ViewState["dtFinal"];
        }

        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        dr[0] = strProductId;
        dr[1] = strSerialNo;
        dr[2] = "0";
        //for batch number
        dr[4] = "";

        dr[5] = RowIndex.ToString();
        //for expiry date
        dr[6] = DateTime.Now.ToString();
        //for Manufacturer date
        dr[7] = DateTime.Now.ToString();

        ViewState["dtFinal"] = dt;

    }
    #endregion

    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        Common cmn = new Common(HttpContext.Current.Session["DBConnection"].ToString());
        bool Result = false;

        if (HttpContext.Current.Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), ModuelId, ObjectId, HttpContext.Current.Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }
}

