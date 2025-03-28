using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using PegasusDataAccess;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

public partial class Inventory_PhysicalInventory1 : BasePage
{

    #region Class Object

    Inv_PhysicalHeader ObjPhysicalheader = null;
    Inv_PhysicalDetail ObjPhysicalDetail = null;
    Inv_StockDetail objStockDetail = null;
    Inv_StockBatchMaster objStockHeader = null;
    Inv_UnitMaster objUnit = null;
    Inv_ProductMaster ObjProductMaster = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    IT_ObjectEntry objObjectEntry = null;
    Common ObjComman = null;
    Common cmn = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_ProductLedger ObjProductLadger = null;
    DataAccessClass objDa = null;
    Inv_RackMaster ObjRackMaster = null;
    Inv_RackDetail ObjRackDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    LocationMaster objLocation = null;
    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjPhysicalheader = new Inv_PhysicalHeader(Session["DBConnection"].ToString());
        ObjPhysicalDetail = new Inv_PhysicalDetail(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objStockHeader = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjRackMaster = new Inv_RackMaster(Session["DBConnection"].ToString());
        ObjRackDetail = new Inv_RackDetail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/PhysicalInventory.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }


            AllPageCode(clsPagePermission);



            FillScanLocationList();
            fillRackList();
            ddlScanLocation.SelectedValue = Session["LocId"].ToString();
            try
            {
                using (DataTable _dt = ObjPhysicalheader.GetOpenPhysicalHeadersVoucher(Session["CompId"].ToString(), Session["LocId"].ToString()))
                {
                    if (_dt.Rows.Count > 0)
                    {
                        txtScanVoucherNo.Text = _dt.Rows[0]["VoucherNo"].ToString();
                        hdnScanVoucherId.Value = _dt.Rows[0]["trans_id"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            txtVoucherNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtVoucherNo.Text;
            txtVoucherdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtCalenderExtender.Format = Session["DateFormat"].ToString();
            deleteBlankVoucher();
            Fillgrid();
            btnReset_Click(null, null);
            txtValue.Focus();
        }
        Common.clsPagePermission PagePermission = cmn.getPagePermission("../Inventory/PhysicalInventory.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
        if (PagePermission.bHavePermission == false)
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }


        AllPageCode(PagePermission);
        I1.Attributes.Add("Class", "fa fa-plus");
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    public string GetDocumentNumber()
    {

        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "11", "128", "0",
 HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;

    }

    #region System Function :-

    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtbinValue.Focus();
        txtbinValue.Text = "";
    }
    protected void btnPost_Click(object sender, EventArgs e)
    {
        btnSave_Click(sender, e);
    }

    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }

    private string GetLocationCode(string strLocId)
    {
        string strLocationCode = string.Empty;
        if (strLocId != "0" && strLocId != "")
        {
            DataTable dtLocation = objLocation.GetLocationMasterByLocationId(strLocId);
            if (dtLocation.Rows.Count > 0)
            {
                strLocationCode = dtLocation.Rows[0]["Location_Code"].ToString();
            }
        }
        return strLocationCode;
    }

    //protected void btnSave_Click(object sender, EventArgs e)
    //{
    //    btnSave.Enabled = false;
    //    SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


    //    bool Post = false;
    //    Post = ((Button)sender).ID.Trim() == "btnPost" ? true : false;




    //    if (txtVoucherdate.Text == "")
    //    {

    //        DisplayMessage("Enter Voucher Date");
    //        ViewState["Post"] = null;
    //        txtVoucherdate.Focus();
    //        btnSave.Enabled = true;
    //        return;
    //    }
    //    else
    //    {
    //        try
    //        {
    //            ObjSysParam.getDateForInput(txtVoucherdate.Text);

    //        }
    //        catch
    //        {
    //            DisplayMessage("Enter Voucher Date in Format " + Session["DateFormat"].ToString() + "");
    //            txtVoucherdate.Text = "";
    //            ViewState["Post"] = null;
    //            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherdate);
    //            btnSave.Enabled = true;
    //            return;
    //        }
    //    }





    //    //code added by jitendra upadhyay on 09-12-2016
    //    //for insert record according the log in financial year

    //    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtVoucherdate.Text), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
    //    {
    //        DisplayMessage("Log In Financial year not allowing to perform this action");
    //        btnSave.Enabled = true;
    //        return;
    //    }

    //    if (txtVoucherNo.Text == "")
    //    {

    //        DisplayMessage("Enter Voucher No.");
    //        txtVoucherNo.Focus();
    //        ViewState["Post"] = null;
    //        btnSave.Enabled = true;
    //        return;
    //    }



    //    if (txtRemarks.Text == "")
    //    {

    //        DisplayMessage("Enter Remarks");
    //        ViewState["Post"] = null;
    //        txtRemarks.Focus();
    //        btnSave.Enabled = true;
    //        return;
    //    }


    //    if (Post)
    //    {
    //        if (txtpaymentdebitaccount.Text == "")
    //        {
    //            DisplayMessage("select debit account");
    //            txtpaymentdebitaccount.Focus();
    //            btnSave.Enabled = true;
    //            return;

    //        }
    //        if (txtpaymentCreditaccount.Text == "")
    //        {
    //            DisplayMessage("select credit account");
    //            txtpaymentCreditaccount.Focus();
    //            btnSave.Enabled = true;
    //            return;
    //        }
    //    }

    //    if (txtNetAmount.Text == "")
    //    {
    //        txtNetAmount.Text = "0";
    //    }


    //    if (txtPhysical.Text == "")
    //    {
    //        txtPhysical.Text = "0";
    //    }


    //    double PaidTotalCommmissionamount = 0;


    //    if (Convert.ToDouble(txtNetAmount.Text) > Convert.ToDouble(txtPhysical.Text))
    //    {
    //        PaidTotalCommmissionamount = Convert.ToDouble(txtNetAmount.Text) - Convert.ToDouble(txtPhysical.Text);
    //    }
    //    else if (Convert.ToDouble(txtNetAmount.Text) < Convert.ToDouble(txtPhysical.Text))
    //    {

    //        PaidTotalCommmissionamount = Convert.ToDouble(txtPhysical.Text) - Convert.ToDouble(txtNetAmount.Text);
    //    }


    //    //if (ViewState["Post"] != null)
    //    //{
    //    //    Post = true;
    //    //}


    //    con.Open();
    //    SqlTransaction trns;
    //    trns = con.BeginTransaction();        
    //    try
    //    {
    //        int b = 0;
    //        if (editid.Value == "")
    //        {

    //            b = ObjPhysicalheader.InsertPhysicalHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), "0", txtVoucherNo.Text, ObjSysParam.getDateForInput(txtVoucherdate.Text).ToString(), txtRemarks.Text, txtNetAmount.Text, Post.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
    //            //for update voucher number according document number set in system setup module 
    //            //code created by jitendra upadhyay on 04-07-2016

    //            string strInvoiceNo = string.Empty;

    //            if (txtVoucherNo.Text.Trim() == ViewState["DocNo"].ToString().Trim())
    //            {
    //                DataTable dtCount = ObjPhysicalheader.GetPhysicalHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);

    //                if (dtCount.Rows.Count == 0)
    //                {
    //                    ObjPhysicalheader.Updatecode(b.ToString(), txtVoucherNo.Text + "1", ref trns);
    //                    txtVoucherNo.Text = txtVoucherNo.Text + "1";
    //                }
    //                else
    //                {
    //                    DataTable dtCount1 = new DataView(dtCount, "VoucherNo='" + txtVoucherNo.Text + dtCount.Rows.Count + "'", "", DataViewRowState.CurrentRows).ToTable();
    //                    int NoRow = dtCount.Rows.Count;
    //                    if (dtCount1.Rows.Count > 0)
    //                    {
    //                        bool bCodeFlag = true;
    //                        while (bCodeFlag)
    //                        {
    //                            NoRow += 1;
    //                            DataTable dtTemp = new DataView(dtCount, "VoucherNo='" + txtVoucherNo.Text + NoRow + "'", "", DataViewRowState.CurrentRows).ToTable();
    //                            if (dtTemp.Rows.Count == 0)
    //                            {
    //                                bCodeFlag = false;
    //                            }
    //                        }
    //                    }


    //                    ObjPhysicalheader.Updatecode(b.ToString(), txtVoucherNo.Text + NoRow.ToString(), ref trns);
    //                    txtVoucherNo.Text = txtVoucherNo.Text + NoRow.ToString();

    //                }
    //            }


    //            DataTable DtDetail = (DataTable)Session["Dtproduct"];
    //            int j = 1;
    //            //foreach (GridViewRow gvrow in gvProduct.Rows)
    //            //{
    //            //    Label lblProductId = (Label)gvrow.FindControl("lblPID");
    //            //    Label lblsystemQty = (Label)gvrow.FindControl("lblSystemQuantity");


    //            //    TextBox txtPhysicalQty = (TextBox)gvrow.FindControl("txtPhysicalQuantity");
    //            //    TextBox txtAverageCost = (TextBox)gvrow.FindControl("lblUnitCost");
    //            //    Label lblUnitCost = (Label)gvrow.FindControl("lblUnitCost12");

    //            //    Label lblUnitId = (Label)gvrow.FindControl("lblUnitId");

    //            //    if (txtPhysicalQty.Text == "")
    //            //    {
    //            //        txtPhysicalQty.Text = "0";
    //            //    }
    //            //    if (lblsystemQty.Text == "")
    //            //    {
    //            //        lblsystemQty.Text = "0";
    //            //    }
    //            //    if (txtAverageCost.Text == "")
    //            //    {
    //            //        txtAverageCost.Text = "0";
    //            //    }
    //            //    if (lblUnitCost.Text == "")
    //            //    {
    //            //        lblUnitCost.Text = "0";
    //            //    }


    //            //    ObjPhysicalDetail.InsertPhysicalDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString(), b.ToString(), "0", j.ToString(), lblProductId.Text, DateTime.Now.ToString(), "0", lblUnitId.Text.Trim(), lblsystemQty.Text, txtPhysicalQty.Text, lblUnitCost.Text, ((TextBox)gvrow.FindControl("txtRackname")).Text, txtAverageCost.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

    //            //    if (Post)
    //            //    {
    //            //        string InOut = "I";

    //            //        string InQty = "0";

    //            //        string OutQty = "0";
    //            //        if (Convert.ToDouble(lblsystemQty.Text.Trim()) != Convert.ToDouble(txtPhysicalQty.Text.Trim()))
    //            //        {


    //            //            if (Convert.ToDouble(lblsystemQty.Text.Trim()) < Convert.ToDouble(txtPhysicalQty.Text.Trim()))
    //            //            {
    //            //                InOut = "I";
    //            //                InQty = (Convert.ToDouble(txtPhysicalQty.Text.Trim()) - (Convert.ToDouble(lblsystemQty.Text.Trim()))).ToString();
    //            //            }
    //            //            else
    //            //            {
    //            //                InOut = "O";
    //            //                OutQty = (Convert.ToDouble(lblsystemQty.Text.Trim()) - Convert.ToDouble(txtPhysicalQty.Text.Trim())).ToString();

    //            //            }


    //            //            ObjProductLadger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), "Physical Inventory", b.ToString(), "0", lblProductId.Text, lblUnitId.Text.Trim(), InOut, "0", "0", InQty, OutQty, "1/1/1800", txtAverageCost.Text.ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(Convert.ToDateTime(txtVoucherdate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString())).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);


    //            //            // You need to check serial also here 
    //            //            //




    //            //            //


    //            //        }


    //            //        string strsql = "update inv_stockdetail set Field2='" + txtAverageCost.Text + "' where ProductId=" + lblProductId.Text + " and Location_Id=" + Session["LocId"].ToString() + " and Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "";
    //            //        objDa.execute_Command(strsql, ref trns);

    //            //    }

    //            //    j++;


    //            //    //here we update rack in rack master rack detail table if rack  exists  or not exists.

    //            //    if (((TextBox)gvrow.FindControl("txtRackname")).Text.Trim() != "")
    //            //    {
    //            //        int RackHeaderId = 0;
    //            //        DataTable dtRack = ObjRackMaster.GetRackMasterByRackName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((TextBox)gvrow.FindControl("txtRackname")).Text, ref trns);

    //            //        if (dtRack.Rows.Count == 0)
    //            //        {
    //            //            RackHeaderId = ObjRackMaster.InsertRackMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", ((TextBox)gvrow.FindControl("txtRackname")).Text, ((TextBox)gvrow.FindControl("txtRackname")).Text, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


    //            //        }
    //            //        else
    //            //        {
    //            //            RackHeaderId = Convert.ToInt32(dtRack.Rows[0]["Rack_ID"].ToString());
    //            //        }


    //            //        ObjRackDetail.DeleteRackDetail_By_LocationId_and_ProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblProductId.Text, ref trns);


    //            //        ObjRackDetail.InsertRackDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), RackHeaderId.ToString(), lblProductId.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


    //            //    }


    //            //}


    //            if (chkDeleteLog.Checked & !Post)
    //            {
    //                string strsql1 = "SELECT DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.eproductname AS EProductName, Inv_UnitMaster.Unit_Name, ISNULL(Inv_StockDetail.Field1, 0) AS UnitCost, ISNULL(Inv_StockDetail.Field2, 0) AS AverageCost, ISNULL(Inv_StockDetail.Quantity, 0) AS SystemQuantity, 0 AS PhysicalQuantity, ISNULL(Inv_StockBatchMaster.SerialNo, ' ') AS SerialNo, '' as RackName, Inv_ProductMaster.AlternateId1, Inv_ProductMaster.AlternateId2, Inv_ProductMaster.AlternateId3 FROM Inv_ProductMaster LEFT JOIN (select sbm.ProductId,sbm.SerialNo from Inv_StockBatchMaster sbm where sbm.SerialNo<>'' and sbm.SerialNo<>'0' and sbm.Location_Id='" + Session["LocId"].ToString() + "' and sbm.isactive='true' group by sbm.ProductId,sbm.SerialNo having sum(case when sbm.InOut='I' then sbm.quantity else -sbm.Quantity end)>=1) Inv_StockBatchMaster ON Inv_ProductMaster.ProductId = Inv_StockBatchMaster.ProductId LEFT JOIN (SELECT * FROM Inv_StockDetail WHERE Inv_StockDetail.Company_Id ='" + Session["CompId"].ToString() + "' AND Inv_StockDetail.Brand_Id ='" + Session["BrandId"].ToString() + "' AND Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' AND inv_stockdetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id WHERE inv_productmaster.isactive = 'True' AND Inv_ProductMaster.Field1 = ' ' AND Inv_ProductMaster.Company_Id = '" + Session["CompId"].ToString() + "' AND Inv_ProductMaster.Brand_Id = '" + Session["BrandId"].ToString() + "' and Inv_ProductMaster.ItemType='S'";
    //                DataTable dtProduct = new DataTable();
    //                dtProduct = objDa.return_DataTable(strsql1);
    //                for (int count = 0; count < dtProduct.Rows.Count; count++)
    //                {
    //                    if (count == 0)
    //                    {
    //                        strsql1 = "Delete From  Inv_PhysicalDetailLogs where  ref_iph_id = '" + b + "'";
    //                        objDa.execute_Command(strsql1);
    //                    }
    //                    strsql1 = "INSERT INTO Inv_PhysicalDetailLogs(ref_iph_id,productid,productcode,eproductname,unit_name,unitcost,averagecost,systemqty,physicalqty,serialno,rackname,alternateid1,alternateid2,alternateid3) Values (";
    //                    strsql1 += "'" + b + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["productid"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["productcode"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["eproductname"].ToString().Replace("'", "") + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["unit_name"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["unitcost"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["averagecost"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["systemquantity"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["physicalquantity"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["serialno"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["rackname"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["alternateid1"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["alternateid2"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["alternateid3"].ToString() + "'";
    //                    strsql1 += ")";
    //                    objDa.execute_Command(strsql1);
    //                }




    //            }


    //            if (Post)
    //            {


    //                if (PaidTotalCommmissionamount != 0)
    //                {
    //                    //For Bank Account
    //                    string strAccountId = string.Empty;
    //                    DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", ref trns);
    //                    if (dtAccount.Rows.Count > 0)
    //                    {
    //                        for (int i = 0; i < dtAccount.Rows.Count; i++)
    //                        {
    //                            if (strAccountId == "")
    //                            {
    //                                strAccountId = dtAccount.Rows[i]["Param_Value"].ToString();
    //                            }
    //                            else
    //                            {
    //                                strAccountId = strAccountId + "," + dtAccount.Rows[i]["Param_Value"].ToString();
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        strAccountId = "0";
    //                    }

    //                    string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();


    //                    //for Voucher Number
    //                    string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
    //                    if (strVoucherNumber != "")
    //                    {
    //                        DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
    //                        if (dtCount.Rows.Count > 0)
    //                        {
    //                            dtCount = new DataView(dtCount, "Voucher_Type='JV'", "", DataViewRowState.CurrentRows).ToTable();
    //                        }
    //                        if (dtCount.Rows.Count == 0)
    //                        {
    //                            strVoucherNumber = strVoucherNumber + "1";
    //                        }
    //                        else
    //                        {
    //                            strVoucherNumber = strVoucherNumber + (dtCount.Rows.Count + 1);
    //                        }
    //                    }

    //                    objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "physical Inventory Difference for Voucher No. '" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "physical Inventory Difference for Voucher No. '" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", false.ToString(), false.ToString(), false.ToString(), "PV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    string strVMaxId = string.Empty;
    //                    DataTable dtVMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
    //                    if (dtVMaxId.Rows.Count > 0)
    //                    {
    //                        strVMaxId = dtVMaxId.Rows[0][0].ToString();
    //                    }

    //                    //str for Employee Id
    //                    //For Debit
    //                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), PaidTotalCommmissionamount.ToString());
    //                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
    //                    if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
    //                    {
    //                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", PaidTotalCommmissionamount.ToString(), "0.00", "physical Inventory Difference for Voucher No. '" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                    else
    //                    {
    //                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", PaidTotalCommmissionamount.ToString(), "0.00", "physical Inventory Difference for Voucher No.'" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }

    //                    //For Credit
    //                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), PaidTotalCommmissionamount.ToString());
    //                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
    //                    if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
    //                    {
    //                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalCommmissionamount.ToString(), "physical Inventory Difference for Voucher No. '" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                    else
    //                    {
    //                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalCommmissionamount.ToString(), "physical Inventory Difference for Voucher No.'" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                }
    //            }


    //            if (b != 0)
    //            {

    //                if (Post)
    //                {
    //                    DisplayMessage("Record posted successfully");
    //                }
    //                else
    //                {
    //                    DisplayMessage("Record Saved", "green");
    //                }

    //            }
    //        }
    //        else
    //        {

    //            b = ObjPhysicalheader.UpdatePhysicalHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), editid.Value, "0", txtVoucherNo.Text, ObjSysParam.getDateForInput(txtVoucherdate.Text).ToString(), txtRemarks.Text, txtNetAmount.Text, Post.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
    //            ObjPhysicalDetail.DeletePhysicalDetailByHeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", editid.Value, false.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
    //            int j = 1;

    //            string strsql2 = "update inv_stockdetail set Quantity = '0' where Location_Id=" + Session["LocId"].ToString() + " and Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "";
    //            objDa.execute_Command(strsql2, ref trns);

    //            //foreach (GridViewRow gvrow in gvProduct.Rows)
    //            //{
    //            //    Label lblProductId = (Label)gvrow.FindControl("lblPID");
    //            //    Label lblsystemQty = (Label)gvrow.FindControl("lblSystemQuantity");

    //            //    TextBox txtPhysicalQty = (TextBox)gvrow.FindControl("txtPhysicalQuantity");

    //            //    TextBox txtAverageCost = (TextBox)gvrow.FindControl("lblUnitCost");
    //            //    Label lblUnitCost = (Label)gvrow.FindControl("lblUnitCost12");
    //            //    Label lblUnitId = (Label)gvrow.FindControl("lblUnitId");

    //            //    if (txtPhysicalQty.Text == "")
    //            //    {
    //            //        txtPhysicalQty.Text = "0";
    //            //    }
    //            //    if (lblsystemQty.Text == "")
    //            //    {
    //            //        lblsystemQty.Text = "0";
    //            //    }

    //            //    if (txtAverageCost.Text == "")
    //            //    {
    //            //        txtAverageCost.Text = "0";
    //            //    }
    //            //    if (lblUnitCost.Text == "")
    //            //    {
    //            //        lblUnitCost.Text = "0";
    //            //    }
    //            //    string strsql = "";

    //            //    ObjPhysicalDetail.InsertPhysicalDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString(), editid.Value.ToString(), "0", j.ToString(), lblProductId.Text, DateTime.Now.ToString(), "0", lblUnitId.Text.Trim(), lblsystemQty.Text, txtPhysicalQty.Text, lblUnitCost.Text, ((TextBox)gvrow.FindControl("txtRackname")).Text, txtAverageCost.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);





    //            //    if (Post)
    //            //    {
    //            //        string InOut = "I";

    //            //        string InQty = "0";

    //            //        string OutQty = "0";
    //            //        if (Convert.ToDouble(lblsystemQty.Text.Trim()) != Convert.ToDouble(txtPhysicalQty.Text.Trim()))
    //            //        {


    //            //            if (Convert.ToDouble(lblsystemQty.Text.Trim()) < Convert.ToDouble(txtPhysicalQty.Text.Trim()))
    //            //            {
    //            //                InOut = "I";
    //            //                InQty = (Convert.ToDouble(txtPhysicalQty.Text.Trim()) - (Convert.ToDouble(lblsystemQty.Text.Trim()))).ToString();
    //            //            }
    //            //            else
    //            //            {
    //            //                InOut = "O";
    //            //                OutQty = (Convert.ToDouble(lblsystemQty.Text.Trim()) - Convert.ToDouble(txtPhysicalQty.Text.Trim())).ToString();

    //            //            }

    //            //            ObjProductLadger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), "Physical Inventory", b.ToString(), "0", lblProductId.Text, lblUnitId.Text.Trim(), InOut, "0", "0", InQty, OutQty, "1/1/1800", txtAverageCost.Text, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(Convert.ToDateTime(txtVoucherdate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString())).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);






    //            //        }
    //            //        strsql = "update inv_stockdetail set Quantity = '" + Convert.ToDouble(txtPhysicalQty.Text.Trim()) + "', Field2='" + txtAverageCost.Text + "' where ProductId=" + lblProductId.Text + " and Location_Id=" + Session["LocId"].ToString() + " and Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "";

    //            //        objDa.execute_Command(strsql, ref trns);


    //            //        strsql = "Select * From Inv_ProductMaster where  ProductId =  '" + lblProductId.Text + "'";
    //            //        DataTable dtProductMaster = new DataTable();
    //            //        dtProductMaster = objDa.return_DataTable(strsql, ref trns);

    //            //        if (dtProductMaster.Rows[0]["MaintainStock"].ToString() == "SNO")
    //            //        {

    //            //            DataTable dtStockBatch = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductIdForSalesInvoice(lblProductId.Text, Session["LocId"].ToString(), ref trns);

    //            //            System.Data.DataColumn newColumn = new System.Data.DataColumn("Flag", typeof(System.String));
    //            //            newColumn.DefaultValue = "N";
    //            //            dtStockBatch.Columns.Add(newColumn);

    //            //            strsql = "Select * From   Inv_PhysicalDetailLogs Where ref_iph_id =    '" + editid.Value.ToString() + "'  and productcode IN ( '" + dtProductMaster.Rows[0]["ProductCode"].ToString() + "')";
    //            //            //strsql = "Select * From   Inv_PhysicalDetailLogs Where ref_iph_id =    '" + editid.Value.ToString() + "'  and productid IN ( '" + lblProductId.Text + "')";



    //            //            DataTable dtPhyLog = objDa.return_DataTable(strsql, ref trns);

    //            //            for (int count = 0; count < dtPhyLog.Rows.Count; count++)
    //            //            {
    //            //                try
    //            //                {
    //            //                    if (dtPhyLog.Rows[count]["physicalqty"].ToString() == "0")
    //            //                    {
    //            //                        // need to out from stock batch
    //            //                        strsql = "INSERT INTO Inv_StockBatchMaster Values('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + lblProductId.Text + "','Ph','" + editid.Value.ToString() + "','" + dtProductMaster.Rows[0]["UnitId"].ToString() + "','O','1','0','0','" + DateTime.Now + "','" + dtPhyLog.Rows[count]["serialno"].ToString() + "','" + DateTime.Now + "','','','','','','','','','','1','1800-01-01 00:00:00.000','1','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "')";
    //            //                        objDa.execute_Command(strsql, ref trns);
    //            //                    }
    //            //                    else
    //            //                    {
    //            //                        // need to check if not found then insert
    //            //                        DataTable dtTempCheck = new DataView(dtStockBatch, "SerialNo = '" + dtPhyLog.Rows[count]["serialno"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //            //                        if (dtTempCheck.Rows.Count == 0)
    //            //                        {
    //            //                            strsql = "INSERT INTO Inv_StockBatchMaster Values('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + lblProductId.Text + "','Ph','" + editid.Value.ToString() + "','" + dtProductMaster.Rows[0]["UnitId"].ToString() + "','I','1','0','0','" + DateTime.Now + "','" + dtPhyLog.Rows[count]["serialno"].ToString() + "','" + DateTime.Now + "','','','','','','','','','','1','1800-01-01 00:00:00.000','1','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "')";
    //            //                            objDa.execute_Command(strsql, ref trns);
    //            //                        }


    //            //                    }
    //            //                }
    //            //                catch (Exception ex)
    //            //                {
    //            //                    string str = ex.Message.ToString();
    //            //                }

    //            //            }


    //            //            for (int count = 0; count < dtStockBatch.Rows.Count; count++)
    //            //            {
    //            //                try
    //            //                {
    //            //                    DataTable dtTempCheck = new DataView(dtPhyLog, "SerialNo = '" + dtStockBatch.Rows[count]["serialno"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //            //                    if (dtTempCheck.Rows.Count == 0)
    //            //                    {
    //            //                        strsql = "INSERT INTO Inv_StockBatchMaster Values('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + lblProductId.Text + "','Ph','" + editid.Value.ToString() + "','" + dtProductMaster.Rows[0]["UnitId"].ToString() + "','O','1','0','0','" + DateTime.Now + "','" + dtStockBatch.Rows[count]["serialno"].ToString() + "','" + DateTime.Now + "','','','','','','','','','','1','1800-01-01 00:00:00.000','1','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "')";
    //            //                        objDa.execute_Command(strsql, ref trns);

    //            //                    }
    //            //                }
    //            //                catch (Exception ex)
    //            //                {
    //            //                    string str = ex.Message.ToString();
    //            //                }

    //            //            }

    //            //            //if (dtPhyLog.Rows.Count == 0)
    //            //            //{
    //            //            //    if (dtStockBatch.Rows.Count > 0)
    //            //            //    {
    //            //            //        for (int count = 0; count < dtStockBatch.Rows.Count; count++)
    //            //            //        {
    //            //            //            strsql = "INSERT INTO Inv_StockBatchMaster Values('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + lblProductId.Text + "','Ph','" + editid.Value.ToString() + "','" + dtProductMaster.Rows[0]["UnitId"].ToString() + "','O','1','0','0','" + DateTime.Now + "','" + dtStockBatch.Rows[count]["SerialNo"].ToString() + "','" + DateTime.Now + "','','','','','','','','','','1','1800-01-01 00:00:00.000','1','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "')";

    //            //            //            objDa.execute_Command(strsql, ref trns);
    //            //            //        }
    //            //            //    }
    //            //            //}
    //            //            //else
    //            //            //{

    //            //            //}
    //            //        }




    //            //    }

    //            //    j++;




    //            //    //here we update rack in rack master rack detail table if rack  exists  or not exists.

    //            //    if (((TextBox)gvrow.FindControl("txtRackname")).Text.Trim() != "")
    //            //    {
    //            //        int RackHeaderId = 0;
    //            //        DataTable dtRack = ObjRackMaster.GetRackMasterByRackName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((TextBox)gvrow.FindControl("txtRackname")).Text, ref trns);

    //            //        if (dtRack.Rows.Count == 0)
    //            //        {
    //            //            RackHeaderId = ObjRackMaster.InsertRackMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", ((TextBox)gvrow.FindControl("txtRackname")).Text, ((TextBox)gvrow.FindControl("txtRackname")).Text, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


    //            //        }
    //            //        else
    //            //        {
    //            //            RackHeaderId = Convert.ToInt32(dtRack.Rows[0]["Rack_ID"].ToString());
    //            //        }


    //            //        ObjRackDetail.DeleteRackDetail_By_LocationId_and_ProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblProductId.Text, ref trns);


    //            //        ObjRackDetail.InsertRackDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), RackHeaderId.ToString(), lblProductId.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


    //            //    }


    //            //}


    //            if (chkDeleteLog.Checked && !Post)
    //            {
    //                string strsql1 = "SELECT DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.eproductname AS EProductName, Inv_UnitMaster.Unit_Name, ISNULL(Inv_StockDetail.Field1, 0) AS UnitCost, ISNULL(Inv_StockDetail.Field2, 0) AS AverageCost, ISNULL(Inv_StockDetail.Quantity, 0) AS SystemQuantity, 0 AS PhysicalQuantity, ISNULL(Inv_StockBatchMaster.SerialNo, ' ') AS SerialNo, '' as RackName, Inv_ProductMaster.AlternateId1, Inv_ProductMaster.AlternateId2, Inv_ProductMaster.AlternateId3 FROM Inv_ProductMaster LEFT JOIN (select sbm.ProductId,sbm.SerialNo from Inv_StockBatchMaster sbm where sbm.SerialNo<>'' and sbm.SerialNo<>'0' and sbm.Location_Id='" + Session["LocId"].ToString() + "' and sbm.isactive='true' group by sbm.ProductId,sbm.SerialNo having sum(case when sbm.InOut='I' then sbm.quantity else -sbm.Quantity end)>=1) Inv_StockBatchMaster ON Inv_ProductMaster.ProductId = Inv_StockBatchMaster.ProductId LEFT JOIN (SELECT * FROM Inv_StockDetail WHERE Inv_StockDetail.Company_Id ='" + Session["CompId"].ToString() + "' AND Inv_StockDetail.Brand_Id ='" + Session["BrandId"].ToString() + "' AND Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' AND inv_stockdetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id WHERE inv_productmaster.isactive = 'True' AND Inv_ProductMaster.Field1 = ' ' AND Inv_ProductMaster.Company_Id = '" + Session["CompId"].ToString() + "' AND Inv_ProductMaster.Brand_Id = '" + Session["BrandId"].ToString() + "' and Inv_ProductMaster.ItemType='S'";
    //                DataTable dtProduct = new DataTable();
    //                dtProduct = objDa.return_DataTable(strsql1);
    //                for (int count = 0; count < dtProduct.Rows.Count; count++)
    //                {
    //                    if (count == 0)
    //                    {
    //                        strsql1 = "Delete From  Inv_PhysicalDetailLogs where  ref_iph_id = '" + editid.Value.ToString() + "'";
    //                        objDa.execute_Command(strsql1);
    //                    }
    //                    strsql1 = "INSERT INTO Inv_PhysicalDetailLogs(ref_iph_id,productid,productcode,eproductname,unit_name,unitcost,averagecost,systemqty,physicalqty,serialno,rackname,alternateid1,alternateid2,alternateid3) Values (";
    //                    strsql1 += "'" + editid.Value.ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["productid"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["productcode"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["eproductname"].ToString().Replace("'", "") + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["unit_name"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["unitcost"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["averagecost"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["systemquantity"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["physicalquantity"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["serialno"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["rackname"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["alternateid1"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["alternateid2"].ToString() + "',";
    //                    strsql1 += "'" + dtProduct.Rows[count]["alternateid3"].ToString() + "'";
    //                    strsql1 += ")";
    //                    objDa.execute_Command(strsql1);
    //                }
    //            }


    //            if (Post)
    //            {


    //                if (PaidTotalCommmissionamount != 0)
    //                {
    //                    //For Bank Account
    //                    string strAccountId = string.Empty;
    //                    DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", ref trns);
    //                    if (dtAccount.Rows.Count > 0)
    //                    {
    //                        for (int i = 0; i < dtAccount.Rows.Count; i++)
    //                        {
    //                            if (strAccountId == "")
    //                            {
    //                                strAccountId = dtAccount.Rows[i]["Param_Value"].ToString();
    //                            }
    //                            else
    //                            {
    //                                strAccountId = strAccountId + "," + dtAccount.Rows[i]["Param_Value"].ToString();
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        strAccountId = "0";
    //                    }

    //                    string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();


    //                    //for Voucher Number
    //                    string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
    //                    if (strVoucherNumber != "")
    //                    {
    //                        DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
    //                        if (dtCount.Rows.Count > 0)
    //                        {
    //                            dtCount = new DataView(dtCount, "Voucher_Type='JV'", "", DataViewRowState.CurrentRows).ToTable();
    //                        }
    //                        if (dtCount.Rows.Count == 0)
    //                        {
    //                            strVoucherNumber = strVoucherNumber + "1";
    //                        }
    //                        else
    //                        {
    //                            strVoucherNumber = strVoucherNumber + (dtCount.Rows.Count + 1);
    //                        }
    //                    }

    //                    objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "physical Inventory Difference for Voucher No. '" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "physical Inventory Difference for Voucher No. '" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", false.ToString(), false.ToString(), false.ToString(), "PV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    string strVMaxId = string.Empty;
    //                    DataTable dtVMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
    //                    if (dtVMaxId.Rows.Count > 0)
    //                    {
    //                        strVMaxId = dtVMaxId.Rows[0][0].ToString();
    //                    }

    //                    //str for Employee Id
    //                    //For Debit
    //                    string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), PaidTotalCommmissionamount.ToString());
    //                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
    //                    if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
    //                    {
    //                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", PaidTotalCommmissionamount.ToString(), "0.00", "physical Inventory Difference for Voucher No. '" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                    else
    //                    {
    //                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "SC", "1/1/1800", "1/1/1800", "", PaidTotalCommmissionamount.ToString(), "0.00", "physical Inventory Difference for Voucher No.'" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }

    //                    //For Credit
    //                    string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), PaidTotalCommmissionamount.ToString());
    //                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
    //                    if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
    //                    {
    //                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalCommmissionamount.ToString(), "physical Inventory Difference for Voucher No. '" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                    else
    //                    {
    //                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", PaidTotalCommmissionamount.ToString(), "physical Inventory Difference for Voucher No.'" + txtVoucherNo.Text + "'  On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
    //                    }
    //                }
    //            }



    //            if (b != 0)
    //            {
    //                if (Post)
    //                {
    //                    DisplayMessage("Record posted successfully");
    //                }
    //                else
    //                {

    //                    DisplayMessage("Record Updated Successfully", "green");

    //                }
    //                Lbl_Tab_New.Text = Resources.Attendance.New;
    //                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    //            }

    //        }
    //        trns.Commit();
    //        if (con.State == System.Data.ConnectionState.Open)
    //        {
    //            con.Close();
    //        }

    //        trns.Dispose();
    //        con.Dispose();
    //        Fillgrid();
    //        Reset();
    //        txtVoucherdate.Focus();
    //        txtValue.Focus();
    //      //  gvProduct.DataSource = null;
    //        ///gvProduct.DataBind();
    //    }
    //    catch (Exception ex)
    //    {
    //        DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

    //        trns.Rollback();
    //        if (con.State == System.Data.ConnectionState.Open)
    //        {

    //            con.Close();
    //        }
    //        trns.Dispose();
    //        con.Dispose();
    //        btnSave.Enabled = true;
    //        return;
    //    }
    //    btnSave.Enabled = true;
    //}

    protected void btnSave_Click(object sender,EventArgs e)
    {
        bool Post = false;
        Post = ((Button)sender).ID.Trim() == "btnPost" ? true : false;
        if (editid.Value == "")
        {
           // b = ObjPhysicalheader.InsertPhysicalHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), "0", txtVoucherNo.Text, ObjSysParam.getDateForInput(txtVoucherdate.Text).ToString(), txtRemarks.Text, txtNetAmount.Text, Post.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
         
            string sql ="Update Inv_PhysicalHeader Set VoucherNo='"+txtVoucherNo.Text +"',Remark='"+ txtRemarks.Text + "',NetAmount='" + txtNetAmount.Text + "',Vdate='"+ ObjSysParam.getDateForInput(txtVoucherdate.Text).ToString() + "' where Trans_Id='"+hdncanPhyHeader.Value+"'";
            objDa.execute_Command(sql, HttpContext.Current.Session["DBConnection"].ToString());           
            Fillgrid();
            Reset();
            txtVoucherdate.Focus();
            txtValue.Focus();
        }
        else if (Post)
        {
            using (SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString()))
            {
                con.Open();
                SqlTransaction trns = con.BeginTransaction();
                if (Post)
                {
                    try
                    { 
                    string InOut = "I";

                    string InQty = "0";

                    string OutQty = "0";
                    string sql = "Update Inv_PhysicalHeader Set VoucherNo='" + txtVoucherNo.Text + "',Post='1',Remark='" + txtRemarks.Text + "',NetAmount='" + txtNetAmount.Text + "',Vdate='" + ObjSysParam.getDateForInput(txtVoucherdate.Text).ToString() + "' where Trans_Id='" + hdncanPhyHeader.Value + "'";
                    objDa.execute_Command(sql, HttpContext.Current.Session["DBConnection"].ToString());
                    int j = 1;
                    string strsql2 = "update inv_stockdetail set Quantity = '0' where Location_Id=" + Session["LocId"].ToString() + " and Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "";
                    objDa.execute_Command(strsql2, ref trns);
                    DataTable dtStock = (DataTable)Session["FinalStockSheet"];
                    for (int i = 0; i < dtStock.Rows.Count; i++)
                    {

                        if (Convert.ToDouble(dtStock.Rows[i]["SystemQuantity"].ToString()) != Convert.ToDouble(dtStock.Rows[i]["PhysicalQuantity"].ToString()))
                        {


                            if (Convert.ToDouble(dtStock.Rows[i]["SystemQuantity"].ToString()) < Convert.ToDouble(dtStock.Rows[i]["PhysicalQuantity"].ToString()))
                            {
                                InOut = "I";
                                InQty = (Convert.ToDouble(dtStock.Rows[i]["PhysicalQuantity"].ToString()) - (Convert.ToDouble(dtStock.Rows[i]["SystemQuantity"].ToString()))).ToString();
                            }
                            else
                            {
                                InOut = "O";
                                OutQty = (Convert.ToDouble(dtStock.Rows[i]["SystemQuantity"].ToString()) - Convert.ToDouble(dtStock.Rows[i]["PhysicalQuantity"].ToString())).ToString();

                            }
                            DataTable dtUnit = objDa.return_DataTable("Select Unit_Id From Inv_UnitMaster Where Unit_Name='" + dtStock.Rows[i]["Unit_Name"].ToString() + "'");
                            string UnitID = dtUnit.Rows[0]["Unit_Id"].ToString();

                            ObjProductLadger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), "Physical Inventory", "1", "0", dtStock.Rows[i]["ProductId"].ToString(), UnitID, InOut, "0", "0", InQty, OutQty, "1/1/1800", dtStock.Rows[i]["AverageCost"].ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(Convert.ToDateTime(txtVoucherdate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString())).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                            string strsql = "update inv_stockdetail set Quantity = '" + Convert.ToDouble(dtStock.Rows[i]["PhysicalQuantity"].ToString()) + "', Field2='" + dtStock.Rows[i]["AverageCost"].ToString() + "' where ProductId=" + dtStock.Rows[i]["ProductId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "";

                            objDa.execute_Command(strsql, ref trns);

                            strsql = "Select * From Inv_ProductMaster where  ProductId =  '" + dtStock.Rows[i]["ProductId"].ToString() + "'";
                            DataTable dtProductMaster = new DataTable();
                            dtProductMaster = objDa.return_DataTable(strsql, ref trns);
                            if (dtProductMaster.Rows[0]["MaintainStock"].ToString() == "SNO")
                            {
                                DataTable dtStockBatch = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductIdForSalesInvoice(dtStock.Rows[i]["ProductId"].ToString(), Session["LocId"].ToString(), ref trns);

                                System.Data.DataColumn newColumn = new System.Data.DataColumn("Flag", typeof(System.String));
                                newColumn.DefaultValue = "N";
                                dtStockBatch.Columns.Add(newColumn);


                                strsql = "Select * From   Inv_PhysicalDetailLogs Where ref_iph_id =    '" + editid.Value.ToString() + "'  and productcode IN ( '" + dtProductMaster.Rows[0]["ProductCode"].ToString() + "')";
                                DataTable dtPhyLog = objDa.return_DataTable(strsql, ref trns);
                                for (int count = 0; count < dtPhyLog.Rows.Count; count++)
                                {
                                    try
                                    {
                                        if (dtPhyLog.Rows[count]["physicalqty"].ToString() == "0")
                                        {
                                            // need to out from stock batch
                                            strsql = "INSERT INTO Inv_StockBatchMaster Values('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + dtStock.Rows[i]["ProductId"].ToString() + "','Ph','" + editid.Value.ToString() + "','" + dtProductMaster.Rows[0]["UnitId"].ToString() + "','O','1','0','0','" + DateTime.Now + "','" + dtPhyLog.Rows[count]["serialno"].ToString() + "','" + DateTime.Now + "','','','','','','','','','','1','1800-01-01 00:00:00.000','1','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "')";
                                            objDa.execute_Command(strsql, ref trns);
                                        }
                                        else
                                        {
                                            // need to check if not found then insert
                                            DataTable dtTempCheck = new DataView(dtStockBatch, "SerialNo = '" + dtPhyLog.Rows[count]["serialno"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                            if (dtTempCheck.Rows.Count == 0)
                                            {
                                                strsql = "INSERT INTO Inv_StockBatchMaster Values('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + dtStock.Rows[i]["ProductId"].ToString() + "','Ph','" + editid.Value.ToString() + "','" + dtProductMaster.Rows[0]["UnitId"].ToString() + "','I','1','0','0','" + DateTime.Now + "','" + dtPhyLog.Rows[count]["serialno"].ToString() + "','" + DateTime.Now + "','','','','','','','','','','1','1800-01-01 00:00:00.000','1','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "')";
                                                objDa.execute_Command(strsql, ref trns);
                                            }


                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string str = ex.Message.ToString();
                                    }

                                }

                                for (int count = 0; count < dtStockBatch.Rows.Count; count++)
                                {
                                    try
                                    {
                                        DataTable dtTempCheck = new DataView(dtPhyLog, "SerialNo = '" + dtStockBatch.Rows[count]["serialno"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dtTempCheck.Rows.Count == 0)
                                        {
                                            strsql = "INSERT INTO Inv_StockBatchMaster Values('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + dtStock.Rows[i]["ProductId"].ToString() + "','Ph','" + editid.Value.ToString() + "','" + dtProductMaster.Rows[0]["UnitId"].ToString() + "','O','1','0','0','" + DateTime.Now + "','" + dtStockBatch.Rows[count]["serialno"].ToString() + "','" + DateTime.Now + "','','','','','','','','','','1','1800-01-01 00:00:00.000','1','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "','" + Session["UserId"].ToString().ToString() + "','" + DateTime.Now + "')";
                                            objDa.execute_Command(strsql, ref trns);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string str = ex.Message.ToString();
                                    }

                                }



                            }


                        }



                    }
                    Fillgrid();
                    Reset();
                    txtVoucherdate.Focus();
                    txtValue.Focus();
                    DisplayMessage("Record Posted");
                }
                catch(Exception ex)
                {
                   DisplayMessage("No Record Posted");
                        return;
                }
                }
            }           
           
        }
        else
        {
            string sql = "Update Inv_PhysicalHeader Set VoucherNo='" + txtVoucherNo.Text + "',Remark='" + txtRemarks.Text + "',NetAmount='" + txtNetAmount.Text + "',Vdate='" + ObjSysParam.getDateForInput(txtVoucherdate.Text).ToString() + "' where Trans_Id='" + hdncanPhyHeader.Value + "'";
            objDa.execute_Command(sql, HttpContext.Current.Session["DBConnection"].ToString());
            Fillgrid();
            Reset();
            txtVoucherdate.Focus();
            txtValue.Focus();
        }

    }
    protected void lblUnitCost_TextChanged(object sender, EventArgs e)
    {
       // GetGridTotal();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            int b = 0;
            int c = 0;
            DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string STRSQL = "Delete Inv_PhysicalDetail where Header_Id='" + hdncanPhyHeader.Value + "'";
            b = ObjDa.execute_Command(STRSQL, HttpContext.Current.Session["DBConnection"].ToString());
            string STRQRL2 = "Delete Inv_PhysicalHeader where Trans_Id='" + hdncanPhyHeader.Value + "'";
            c = ObjDa.execute_Command(STRQRL2, HttpContext.Current.Session["DBConnection"].ToString());
        }       
        Reset();
        txtVoucherdate.Focus();
    }

    public string GetUnitIdByName(string strUnitName)
    {
        string strUnitId = string.Empty;

        strUnitId = objUnit.GetUnitMasterByUnitName(Session["CompId"].ToString(), strUnitName).Rows[0]["Unit_Id"].ToString();

        return strUnitId;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            int b = 0;
            int c = 0;
            DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string STRSQL = "Delete Inv_PhysicalDetail where Header_Id='" + hdncanPhyHeader.Value + "'";
            b = ObjDa.execute_Command(STRSQL, HttpContext.Current.Session["DBConnection"].ToString());
            string STRQRL2 = "Delete Inv_PhysicalHeader where Trans_Id='" + hdncanPhyHeader.Value + "'";
            c = ObjDa.execute_Command(STRQRL2, HttpContext.Current.Session["DBConnection"].ToString());
        }
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnView_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, (CommandEventArgs)e);
        btnSave.Visible = false;
        btnPost.Visible = false;
        btnDownload.Enabled = false;
        btnConsolidatedDownload.Enabled = false;
        btnUpload.Enabled = false;
        Lbl_Tab_New.Text = Resources.Attendance.View;
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = ObjPhysicalheader.GetPhysicalHeaderByTransId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {

            if (((LinkButton)sender).ID != "btnView")
            {


                if (Convert.ToBoolean(dt.Rows[0]["Post"].ToString()))
                {
                    DisplayMessage("This Record in use,can not be Update");
                    return;
                }
            }

            if (((LinkButton)sender).ID == "btnView")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;

            }
            else
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
            }

            editid.Value = e.CommandArgument.ToString();


            txtVoucherdate.Text = Convert.ToDateTime(dt.Rows[0]["Vdate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtRemarks.Text = dt.Rows[0]["Remark"].ToString();

            txtVoucherNo.Text = dt.Rows[0]["VoucherNo"].ToString();
            txtNetAmount.Text = SetDecimal(dt.Rows[0]["NetAmount"].ToString());
            DataTable dtDetail = ObjPhysicalDetail.GetPhysicalDetailByHeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
            dtDetail.Columns["Field2"].ColumnName = "AverageCost";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //  objPageCmn.FillData((object)gvProduct, dtDetail, "", "");

            dtDetail = dtDetail.DefaultView.ToTable(true,"ProductId" ,"ProductCode", "Trans_Id", "Header_Id", "EProductName", "RackName", "Unit_Name", "UnitCost", "AverageCost", "SystemQuantity", "PhysicalQuantity");

            List<object> lstSno = new List<object> { };
            foreach (DataRow row in dtDetail.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in dtDetail.Columns)
                {
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                lstSno.Add(dict);
            }
            // ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:EditRecord("+ JsonConvert.SerializeObject(lstSno) + "); ", true);
            Session["FinalStockSheet"] = dtDetail;
            //  GetGridTotal();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "EditRecord(" + JsonConvert.SerializeObject(lstSno) + ")", true);
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);        
    }
        //fillgridDetail();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Focus();
        Fillgrid();
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = ObjPhysicalheader.GetPhysicalHeaderByTransId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {

            if (Convert.ToBoolean(dt.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("This Record in use,can not be Update");
                return;
            }
        }


        ObjPhysicalheader.DeletePhysicalHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
        Fillgrid();
        DisplayMessage("Record Deleted");
        try
        {
            int i = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            ((LinkButton)gvPhysical.Rows[i].FindControl("IbtnDelete")).Focus();
        }
        catch
        {
            txtValue.Focus();
        }


    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

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
            DataTable dtCust = (DataTable)Session["DtPhysical"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Physical_Inv"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvPhysical, view.ToTable(), "", "");
            //AllPageCode();
        }
        txtValue.Focus();
    }


    protected void gvPhysical_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Physical_Inv"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Physical_Inv"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvPhysical, dt, "", "");


        //AllPageCode();
        gvPhysical.HeaderRow.Focus();

    }
    protected void gvPhysical_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPhysical.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Physical_Inv"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvPhysical, dt, "", "");


        //AllPageCode();
    }
    #endregion

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        btnPost.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        btnDownload.Visible = clsPagePermission.bDownload;
        btnConsolidatedDownload.Visible = clsPagePermission.bDownload;
        btnUpload.Visible = clsPagePermission.bUpload;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
    }
    #endregion

    #region Auto Complete Method

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster ObjInvProductMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvProductMaster.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, "EProductName like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = dtTemp.Copy();

        }

        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["EProductName"].ToString();
        }


        return txt;
    }
    #endregion

    #region User Defined Function



    public void Fillgrid()
    {

        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            PostStatus = "Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            PostStatus = "Post='False'";
        }
        DataTable dt = new DataView(ObjPhysicalheader.GetPhysicalHeaderAllTrue(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString()), PostStatus, "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvPhysical, dt, "", "");


        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count;
        Session["DtPhysical"] = dt;
        Session["dtFilter_Physical_Inv"] = dt;
        //AllPageCode();
        I1.Attributes.Add("Class", "fa fa-plus");
    }
    public void deleteBlankVoucher()
    {
        string PostStatus = string.Empty;      
        
        PostStatus = "Post='False'";
        
        DataTable dt = new DataView(ObjPhysicalheader.GetPhysicalHeaderAllTrue(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString()), PostStatus, "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["VoucherNo"].ToString() == "")
                {
                    objDa.execute_Command("Delete From Inv_PhysicalDetail where Header_Id='"+dt.Rows[i]["Trans_Id"].ToString()+"'");
                    objDa.execute_Command("Delete From Inv_PhysicalHeader where Trans_Id='"+ dt.Rows[i]["Trans_Id"].ToString() + "'");
                }
            }
        }

    }

    public void GridProductNetAmount()
    {
        double NetAmount = 0;
        //foreach (GridViewRow gvrow in gvProduct.Rows)
        //{
        //    Label lblUnitCost = (Label)gvrow.FindControl("lblUnitCost");
        //    TextBox txtPhysicalQty = (TextBox)gvrow.FindControl("txtPhysicalQuantity");

        //    try
        //    {

        //        NetAmount = NetAmount + (float.Parse(lblUnitCost.Text) * float.Parse(txtPhysicalQty.Text));
        //    }
        //    catch
        //    {

        //    }



        //}
        txtNetAmount.Text = NetAmount.ToString();
    }
    public void Reset()
    {
        try
        {
            using (DataTable _dtAdjustmentAc = objCOA.getAccountDetailsByPreText("adjustment", Session["CompId"].ToString()))
            {
                if (_dtAdjustmentAc.Rows.Count > 0)
                {
                    txtpaymentdebitaccount.Text = _dtAdjustmentAc.Rows[0]["FilterText"].ToString();
                }
            }
            using (DataTable _dtAdjustmentAc = objCOA.getAccountDetailsByPreText("purchase", Session["CompId"].ToString()))
            {
                if (_dtAdjustmentAc.Rows.Count > 0)
                {
                    txtpaymentCreditaccount.Text = _dtAdjustmentAc.Rows[0]["FilterText"].ToString();
                }
            }
        }
        catch (Exception ex)
        {

        }

        txtVoucherNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = GetDocumentNumber();
        txtVoucherdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
       // gvProduct.DataSource = null;
       // gvProduct.DataBind();
        editid.Value = "";
        txtNetAmount.Text = "0";
        txtRemarks.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtValue.Focus();
        Session["Dtproduct"] = null;
        btnDownload.Enabled = true;
        btnConsolidatedDownload.Enabled = true;
        btnUpload.Enabled = true;
        if (Session["EmpId"].ToString() == "0")
        {
            btnSave.Visible = true;
            btnPost.Visible = true;
        }
        else
        {
            DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), "11", "128", HttpContext.Current.Session["CompId"].ToString());
            try
            {
                dtAllPageCode = new DataView(dtAllPageCode, "Op_Id=1", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {
            }
            if (dtAllPageCode.Rows.Count > 0)
            {
                btnSave.Visible = true;
                btnPost.Visible = true;
            }


        }
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
    #endregion

    #region Bin Section
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
       I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");


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
            DataTable dtCust = (DataTable)Session["DtBinPhysical"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvBinPhysical, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void gvBinPhysical_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBinPhysical.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvBinPhysical, dt, "", "");

        }
        //AllPageCode();

        string temp = string.Empty;


        for (int i = 0; i < gvBinPhysical.Rows.Count; i++)
        {
            Label lblconid = (Label)gvBinPhysical.Rows[i].FindControl("lbltransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvBinPhysical.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

        gvBinPhysical.BottomPagerRow.Focus();

    }
    protected void gvBinPhysical_Sorting(object sender, GridViewSortEventArgs e)
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
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvBinPhysical, dt, "", "");

        //AllPageCode();
        gvBinPhysical.HeaderRow.Focus();

    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvBinPhysical.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvBinPhysical.Rows.Count; i++)
        {
            ((CheckBox)gvBinPhysical.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvBinPhysical.Rows[i].FindControl("lbltransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvBinPhysical.Rows[i].FindControl("lbltransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvBinPhysical.Rows[i].FindControl("lbltransId"))).Text.Trim().ToString())
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
        ((CheckBox)gvBinPhysical.HeaderRow.FindControl("chkgvSelectAll")).Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvBinPhysical.Rows[index].FindControl("lbltransId");
        if (((CheckBox)gvBinPhysical.Rows[index].FindControl("chkgvSelect")).Checked)
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
        ((CheckBox)gvBinPhysical.Rows[index].FindControl("chkgvSelect")).Focus();
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
            for (int i = 0; i < gvBinPhysical.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvBinPhysical.Rows[i].FindControl("lbltransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvBinPhysical.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtPr1 = (DataTable)Session["dtBinFilter"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvBinPhysical, dtPr1, "", "");

            //AllPageCode();
            ViewState["Select"] = null;
        }
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
                    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(ObjPhysicalheader.GetPhysicalHeaderByTransId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString()).Rows[0]["Vdate"].ToString()), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
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
                    b = ObjPhysicalheader.DeletePhysicalHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

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
            foreach (GridViewRow Gvr in gvBinPhysical.Rows)
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

        DataTable dt = ObjPhysicalheader.GetPhysicalHeaderAllFalse(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvBinPhysical, dt, "", "");



        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count;
        Session["DtBinPhysical"] = dt;
        Session["DtBinFilter"] = dt;
        if (dt.Rows.Count != 0)
        {
            //AllPageCode();
        }
        else
        {
            imgBtnRestore.Visible = false;
        }
    }
    #endregion
    public string SetDateFormat(string Date)
    {
        string DateFormat = string.Empty;
        DateFormat = Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        return DateFormat;
    }


    protected void btnDownload_Click(object sender, EventArgs e)
    {

        //string strsql = "SELECT  DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.eproductname AS EProductName, Inv_UnitMaster.Unit_Name,isnull( Inv_StockDetail.Field1,0) AS UnitCost,ISNULL( Inv_StockDetail.Field2,0) AS AverageCost,ISNULL( Inv_StockDetail.Quantity,0) AS SystemQuantity, 0 AS PhysicalQuantity,isnull( Inv_StockBatchMaster.SerialNo,' ') as SerialNo,ISNULL( Inv_RackDetail.Rack_Name,' ') AS RackName,Inv_ProductMaster.AlternateId1,Inv_ProductMaster.AlternateId2,Inv_ProductMaster.AlternateId3  FROM Inv_ProductMaster LEFT JOIN (select * from Inv_StockBatchMaster where inv_stockbatchmaster.Inout='I' and Inv_StockBatchMaster.location_id=" + Session["LocId"].ToString() + " )Inv_StockBatchMaster ON Inv_ProductMaster.ProductId = Inv_StockBatchMaster.ProductId Left JOIN (select * from Inv_StockDetail WHERE Inv_StockDetail.Company_Id = " + Session["CompId"].ToString() + " AND Inv_StockDetail.Brand_Id =" + Session["BrandId"].ToString() + " AND Inv_StockDetail.Location_Id =" + Session["LocId"].ToString() + " AND inv_stockdetail.Finance_Year_Id = " + HttpContext.Current.Session["FinanceYearId"].ToString() + " )Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id left join (SELECT Inv_RackDetail.Product_Id,Inv_RackMaster.Rack_Name FROM Inv_RackMaster INNER JOIN Inv_RackDetail ON Inv_RackMaster.Rack_ID = Inv_RackDetail.Rack_Id WHERE inv_rackmaster.location_id=" + Session["LocId"].ToString() + ")Inv_RackDetail on Inv_RackDetail.Product_Id=Inv_ProductMaster.ProductId where inv_productmaster.isactive = 'True' and Inv_ProductMaster.Field1=' ' AND Inv_ProductMaster.Company_Id =" + Session["CompId"].ToString() + " AND Inv_ProductMaster.Brand_Id = " + Session["BrandId"].ToString() + " ORDER BY Inv_ProductMaster.Productid";

        //STUFF(( SELECT ',' + Inv_RackMaster.Rack_Name FROM Inv_RackMaster INNER JOIN Inv_RackDetail ON Inv_RackMaster.Rack_ID = Inv_RackDetail.Rack_Id where inv_rackmaster.Location_Id = '" + Session["LocId"].ToString() + "' and Inv_RackDetail.Product_Id=Inv_ProductMaster.ProductId FOR XML PATH('') ), 1, 1, '')
        //string strsql = "SELECT DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.eproductname AS EProductName, Inv_UnitMaster.Unit_Name, ISNULL(Inv_StockDetail.Field1, 0) AS UnitCost, ISNULL(Inv_StockDetail.Field2, 0) AS AverageCost, ISNULL(Inv_StockDetail.Quantity, 0) AS SystemQuantity, 0 AS PhysicalQuantity, ISNULL(Inv_StockBatchMaster.SerialNo, ' ') AS SerialNo, '' as RackName, Inv_ProductMaster.AlternateId1, Inv_ProductMaster.AlternateId2, Inv_ProductMaster.AlternateId3 FROM Inv_ProductMaster LEFT JOIN (select sbm.ProductId,sbm.SerialNo from Inv_StockBatchMaster sbm where sbm.SerialNo<>'' and sbm.SerialNo<>'0' and sbm.Location_Id='" + Session["LocId"].ToString() + "' and sbm.isactive='true' group by sbm.ProductId,sbm.SerialNo having sum(case when sbm.InOut='I' then sbm.quantity else -sbm.Quantity end)>=1) Inv_StockBatchMaster ON Inv_ProductMaster.ProductId = Inv_StockBatchMaster.ProductId LEFT JOIN (SELECT * FROM Inv_StockDetail WHERE Inv_StockDetail.Company_Id ='" + Session["CompId"].ToString() + "' AND Inv_StockDetail.Brand_Id ='" + Session["BrandId"].ToString() + "' AND Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' AND inv_stockdetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id WHERE inv_productmaster.isactive = 'True' AND Inv_ProductMaster.Field1 = ' ' AND Inv_ProductMaster.Company_Id = '" + Session["CompId"].ToString() + "' AND Inv_ProductMaster.Brand_Id = '" + Session["BrandId"].ToString() + "' and Inv_ProductMaster.ItemType='S'";

        DataTable dtProduct = new DataTable();
        //dtProduct = objDa.return_DataTable(strsql);
        if (ddlPhysical.SelectedIndex == 0)
        {
            dtProduct = ObjProductMaster.GetProductMasterTrueAllwithStock(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        if (ddlPhysical.SelectedIndex == 1)
        {
            dtProduct = ObjProductMaster.GetProductMasterTrueAllCategorywithStock(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        if (ddlPhysical.SelectedIndex == 2)
        {
            dtProduct = ObjProductMaster.GetProductMasterTrueAllManifacturingBrandwithStock(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        if (ddlPhysical.SelectedIndex == 3)
        {
            dtProduct = ObjProductMaster.GetProductMasterTrueAllRackwithStock(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }

        dtProduct = dtProduct.DefaultView.ToTable(true, "ProductId", "ProductCode", "EProductName", "Unit_Name", "UnitCost", "AverageCost", "SystemQuantity", "PhysicalQuantity");


        ExportTableData(dtProduct, "Product Stock List");
    }

    protected void btnConsolidatedDownload_Click(object sender, EventArgs e)
    {
        DataTable dtProduct = new DataTable();
        if (ddlPhysical.SelectedIndex == 0)
        {
            string strsql = "SELECT DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.eproductname AS EProductName, Inv_UnitMaster.Unit_Name, ISNULL(Inv_StockDetail.Field1, 0) AS UnitCost, ISNULL(Inv_StockDetail.Field2, 0) AS AverageCost, ISNULL(Inv_StockDetail.Quantity, 0) AS SystemQuantity, 0 AS PhysicalQuantity, '' as RackName, Inv_ProductMaster.AlternateId1, Inv_ProductMaster.AlternateId2, Inv_ProductMaster.AlternateId3 FROM Inv_ProductMaster LEFT JOIN (SELECT * FROM Inv_StockDetail WHERE Inv_StockDetail.Company_Id ='" + Session["CompId"].ToString() + "' AND Inv_StockDetail.Brand_Id ='" + Session["BrandId"].ToString() + "' AND Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' AND inv_stockdetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id WHERE inv_productmaster.isactive = 'True' AND Inv_ProductMaster.Field1 = ' ' AND Inv_ProductMaster.Company_Id = '" + Session["CompId"].ToString() + "' AND Inv_ProductMaster.Brand_Id = '" + Session["BrandId"].ToString() + "' and Inv_ProductMaster.ItemType='S' ORDER BY inv_productmaster.ProductId ASC";

            dtProduct = objDa.return_DataTable(strsql);
            /// dtProduct = ObjProductMaster.GetProductMasterTrueAllwithStock(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        if (ddlPhysical.SelectedIndex == 1)
        {
            string strsql = "SELECT DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode,Inv_Product_CategoryMaster.Category_Name, Inv_ProductMaster.eproductname AS EProductName, Inv_UnitMaster.Unit_Name, ISNULL(Inv_StockDetail.Field1, 0) AS UnitCost, ISNULL(Inv_StockDetail.Field2, 0) AS AverageCost, ISNULL(Inv_StockDetail.Quantity, 0) AS SystemQuantity, 0 AS PhysicalQuantity, '' as RackName, Inv_ProductMaster.AlternateId1, Inv_ProductMaster.AlternateId2, Inv_ProductMaster.AlternateId3 FROM Inv_ProductMaster LEFT JOIN (SELECT * FROM Inv_StockDetail WHERE Inv_StockDetail.Company_Id ='" + Session["CompId"].ToString() + "' AND Inv_StockDetail.Brand_Id ='" + Session["BrandId"].ToString() + "' AND Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' AND inv_stockdetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id INNER JOIN Inv_Product_Category ON inv_productmaster.ProductId = Inv_Product_Category.ProductId INNER JOIN Inv_Product_CategoryMaster ON Inv_Product_CategoryMaster.Category_Id = Inv_Product_Category.CategoryId         WHERE inv_productmaster.isactive = 'True' AND Inv_ProductMaster.Field1 = ' ' AND Inv_ProductMaster.Company_Id = '" + Session["CompId"].ToString() + "' AND Inv_ProductMaster.Brand_Id = '" + Session["BrandId"].ToString() + "' and Inv_ProductMaster.ItemType='S' ORDER BY inv_productmaster.ProductId ASC";

            dtProduct = objDa.return_DataTable(strsql);
        }
        if (ddlPhysical.SelectedIndex == 2)
        {
            string strsql = "SELECT DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.eproductname AS EProductName, Inv_UnitMaster.Unit_Name, ISNULL(Inv_StockDetail.Field1, 0) AS UnitCost, ISNULL(Inv_StockDetail.Field2, 0) AS AverageCost, ISNULL(Inv_StockDetail.Quantity, 0) AS SystemQuantity, 0 AS PhysicalQuantity, '' as RackName, Inv_ProductMaster.AlternateId1, Inv_ProductMaster.AlternateId2, Inv_ProductMaster.AlternateId3 FROM Inv_ProductMaster LEFT JOIN (SELECT * FROM Inv_StockDetail WHERE Inv_StockDetail.Company_Id ='" + Session["CompId"].ToString() + "' AND Inv_StockDetail.Brand_Id ='" + Session["BrandId"].ToString() + "' AND Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' AND inv_stockdetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id WHERE inv_productmaster.isactive = 'True' AND Inv_ProductMaster.Field1 = ' ' AND Inv_ProductMaster.Company_Id = '" + Session["CompId"].ToString() + "' AND Inv_ProductMaster.Brand_Id = '" + Session["BrandId"].ToString() + "' and Inv_ProductMaster.ItemType='S' ORDER BY inv_productmaster.ProductId ASC";
            dtProduct = objDa.return_DataTable(strsql);
            ////dtProduct = ObjProductMaster.GetProductMasterTrueAllManifacturingBrandwithStock(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        if (ddlPhysical.SelectedIndex == 3)
        {
            dtProduct = ObjProductMaster.GetProductMasterTrueAllRackwithStock(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());

            ///dtProduct = ObjProductMaster.GetProductMasterTrueAllRackwithStock(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }



        ExportTableData(dtProduct, "Product Stock List1");
    }


    protected void btnGetRecord_Click(object sender, EventArgs e)
    {

        //if (GetUnpostedRecord().Rows.Count > 0)
        //{
        //    DisplayMessage("UnPosted Record Found");
        //    return;
        //}

        // string strsql = "SELECT DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.eproductname AS EProductName, Inv_UnitMaster.Unit_Name, ISNULL(Inv_StockDetail.Field1, 0) AS UnitCost, ISNULL(Inv_StockDetail.Field2, 0) AS AverageCost, ISNULL(Inv_StockDetail.Quantity, 0) AS SystemQuantity, 0 AS PhysicalQuantity, ISNULL(Inv_StockBatchMaster.SerialNo, ' ') AS SerialNo, '' as RackName, Inv_ProductMaster.AlternateId1, Inv_ProductMaster.AlternateId2, Inv_ProductMaster.AlternateId3 FROM Inv_ProductMaster LEFT JOIN (select sbm.ProductId,sbm.SerialNo from Inv_StockBatchMaster sbm where sbm.SerialNo<>'' and sbm.SerialNo<>'0' and sbm.Location_Id='" + Session["LocId"].ToString() + "' and sbm.isactive='true' group by sbm.ProductId,sbm.SerialNo having sum(case when sbm.InOut='I' then sbm.quantity else -sbm.Quantity end)>=1) Inv_StockBatchMaster ON Inv_ProductMaster.ProductId = Inv_StockBatchMaster.ProductId LEFT JOIN (SELECT * FROM Inv_StockDetail WHERE Inv_StockDetail.Company_Id ='" + Session["CompId"].ToString() + "' AND Inv_StockDetail.Brand_Id ='" + Session["BrandId"].ToString() + "' AND Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' AND inv_stockdetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id WHERE inv_productmaster.isactive = 'True' AND Inv_ProductMaster.Field1 = ' ' AND Inv_ProductMaster.Company_Id = '" + Session["CompId"].ToString() + "' AND Inv_ProductMaster.Brand_Id = '" + Session["BrandId"].ToString() + "' and Inv_ProductMaster.ItemType='S'";
        string strsql = "SELECT DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.eproductname AS EProductName, Inv_UnitMaster.Unit_Name, ISNULL(Inv_StockDetail.Field1, 0) AS UnitCost, ISNULL(Inv_StockDetail.Field2, 0) AS AverageCost, ISNULL(Inv_StockDetail.Quantity, 0) AS SystemQuantity, 0 AS PhysicalQuantity, ISNULL(Inv_StockBatchMaster.SerialNo, ' ') AS SerialNo, '' as RackName, Inv_ProductMaster.AlternateId1, Inv_ProductMaster.AlternateId2, Inv_ProductMaster.AlternateId3 FROM Inv_ProductMaster LEFT JOIN (SELECT sbm.ProductId,    sbm.SerialNo     FROM Inv_StockBatchMaster sbm    WHERE sbm.Trans_Id IN (SELECT       MAX(SB.trans_id)     FROM Inv_StockBatchMaster AS SB     WHERE SB.ProductId = sbm.ProductId AND SB.Location_Id = '" + Session["LocId"].ToString() + "'     GROUP BY SB.SerialNo)     AND sbm.InOut = 'I') Inv_StockBatchMaster ON Inv_ProductMaster.ProductId = Inv_StockBatchMaster.ProductId LEFT JOIN (SELECT * FROM Inv_StockDetail WHERE Inv_StockDetail.Company_Id ='" + Session["CompId"].ToString() + "' AND Inv_StockDetail.Brand_Id ='" + Session["BrandId"].ToString() + "' AND Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' AND inv_stockdetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id WHERE inv_productmaster.isactive = 'True' AND Inv_ProductMaster.Field1 = ' ' AND Inv_ProductMaster.Company_Id = '" + Session["CompId"].ToString() + "' AND Inv_ProductMaster.Brand_Id = '" + Session["BrandId"].ToString() + "' and Inv_ProductMaster.ItemType='S'";

        DataTable dtProduct = new DataTable();
        dtProduct = objDa.return_DataTable(strsql);
        if (dtProduct == null || dtProduct.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }
        else
        {
            dtProduct = dtProduct.DefaultView.ToTable(true, "ProductId", "ProductCode", "EProductName", "RackName", "Unit_Name", "UnitCost", "AverageCost", "SystemQuantity", "PhysicalQuantity");
           // objPageCmn.FillData((object)gvProduct, dtProduct, "", "");

            Session["FinalStockSheet"] = dtProduct;
            //GetGridTotal();
        }        
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string rackNameChange(string trandId,string RackName)
    {
        int b = 0;
        DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjDa.return_DataTable(" Select Rack_ID From Inv_RackMaster where Rack_Name='"+RackName+"'");
        if (dt.Rows.Count != 0)
        {
            string Rack_Id = dt.Rows[0]["Rack_ID"].ToString();
            string STRSQL = "Update Inv_PhysicalDetail set RackId='" + Rack_Id + "' where Trans_Id='" + trandId + "'";
            b = ObjDa.execute_Command(STRSQL, HttpContext.Current.Session["DBConnection"].ToString());
            if (b != 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        return "0";       
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string  avarageCostChange(string transId,string avarageCost)
    {
        int b = 0;
        DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string STRSQL = "Update Inv_PhysicalDetail set Field2='" + avarageCost + "' where Trans_Id='" + transId + "'";
        b = ObjDa.execute_Command(STRSQL, HttpContext.Current.Session["DBConnection"].ToString());
        if (b != 0)
        {
            return "1";
        }
        else
        {
            return "0";
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ResetClick(string TransId)
    {
        int b = 0;
        int c = 0;
        DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string STRSQL = "Delete Inv_PhysicalDetail where Header_Id='"+ TransId + "'";
        b = ObjDa.execute_Command(STRSQL, HttpContext.Current.Session["DBConnection"].ToString());
        string STRQRL2 = "Delete Inv_PhysicalHeader where Trans_Id='" + TransId + "'";
        c=ObjDa.execute_Command(STRQRL2, HttpContext.Current.Session["DBConnection"].ToString());
        if (b != 0)
        {
            return "1";
        }
        else
        {
            return "0";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string PhysicalCostChange(string transId,string PhyQty)
    {
        int b = 0;
        DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string STRSQL = "Update Inv_PhysicalDetail set PhysicalQuantity='" + PhyQty + "' where Trans_Id='" + transId + "'";
        b = ObjDa.execute_Command(STRSQL, HttpContext.Current.Session["DBConnection"].ToString());
        if (b != 0)
        {
            return "1";
        }
        else
        {
            return "0";
        }
    }
    public void FillHeaderId(string HeaderId)
    {
        hdncanPhyHeader.Value = HeaderId;
       
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string fillTblRecord(string editid,string VoucherNo,string Voucherdate,string txtRemarks,string txtNetAmount)
    {
        try {
            Inv_PhysicalDetail ObjPhysicalDetail = new Inv_PhysicalDetail(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_PhysicalHeader ObjPhysicalheader = new Inv_PhysicalHeader(HttpContext.Current.Session["DBConnection"].ToString());
            SystemParameter ObjSysParam = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString()); 
            DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            Inventory_PhysicalInventory1 phy = new Inventory_PhysicalInventory1();
           
                int b = 0;
                int j = 1;
            if (editid == "")
                {
                string Sql = "Insert Into Inv_PhysicalHeader values('"+HttpContext.Current.Session["CompId"].ToString().ToString()+"','"+ HttpContext.Current.Session["BrandId"].ToString().ToString()+"','"+ HttpContext.Current.Session["LocId"].ToString().ToString()+"','0','"+ VoucherNo+"', '"+ObjSysParam.getDateForInput(Voucherdate).ToString()+"', '"+txtRemarks+"', '"+txtNetAmount+"', '"+false.ToString()+"','','','','','','"+ true.ToString()+"','"+ DateTime.Now.ToString()+"', '"+true.ToString()+"', '"+HttpContext.Current.Session["UserId"].ToString().ToString()+"', '"+DateTime.Now.ToString()+ "','" + HttpContext.Current.Session["UserId"].ToString().ToString() + "', '" + DateTime.Now.ToString() + "') SELECT SCOPE_IDENTITY()";
               // b = ObjDa.execute_Command(Sql,HttpContext.Current.Session["DBConnection"].ToString()); 
                   DataTable dtPhysical = ObjDa.return_DataTable(Sql);
                //  b = ObjPhysicalheader.InsertPhysicalHeader(HttpContext.Current.Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString().ToString(), HttpContext.Current.Session["LocId"].ToString().ToString(), "0", VoucherNo, ObjSysParam.getDateForInput(Voucherdate).ToString(), txtRemarks, txtNetAmount,false.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
                b = Convert.ToInt32(dtPhysical.Rows[0]["Column1"]);
                try
                {
                   // phy.FillHeaderId(b.ToString());
                 
                }
                catch
                {
                    
                }               
                if (b != 0)
                    {
                        List<object> lstSno = new List<object> { };
                        //string[] strSerialNos = JsonConvert.DeserializeObject<string[]>(strSerial);                       
                        DataTable dtProduct = new DataTable();
                        string strsql = "SELECT DISTINCT Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.eproductname AS EProductName,Inv_UnitMaster.Unit_Id,Inv_UnitMaster.Unit_Name, ISNULL(Inv_StockDetail.Field1, 0) AS UnitCost, ISNULL(Inv_StockDetail.Field2, 0) AS AverageCost, ISNULL(Inv_StockDetail.Quantity, 0) AS SystemQuantity, 0 AS PhysicalQuantity, ISNULL(Inv_StockBatchMaster.SerialNo, ' ') AS SerialNo, '' as RackName, Inv_ProductMaster.AlternateId1, Inv_ProductMaster.AlternateId2, Inv_ProductMaster.AlternateId3 FROM Inv_ProductMaster LEFT JOIN (SELECT sbm.ProductId,    sbm.SerialNo     FROM Inv_StockBatchMaster sbm    WHERE sbm.Trans_Id IN (SELECT       MAX(SB.trans_id)     FROM Inv_StockBatchMaster AS SB     WHERE SB.ProductId = sbm.ProductId AND SB.Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "'     GROUP BY SB.SerialNo)     AND sbm.InOut = 'I') Inv_StockBatchMaster ON Inv_ProductMaster.ProductId = Inv_StockBatchMaster.ProductId LEFT JOIN (SELECT * FROM Inv_StockDetail WHERE Inv_StockDetail.Company_Id ='" + HttpContext.Current.Session["CompId"].ToString() + "' AND Inv_StockDetail.Brand_Id ='" + HttpContext.Current.Session["BrandId"].ToString() + "' AND Inv_StockDetail.Location_Id = '" + HttpContext.Current.Session["LocId"].ToString() + "' AND inv_stockdetail.Finance_Year_Id = '" + HttpContext.Current.Session["FinanceYearId"].ToString() + "') Inv_StockDetail ON Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_ProductMaster.UnitId = Inv_UnitMaster.Unit_Id WHERE inv_productmaster.isactive = 'True' AND Inv_ProductMaster.Field1 = ' ' AND Inv_ProductMaster.Company_Id = '" + HttpContext.Current.Session["CompId"].ToString() + "' AND Inv_ProductMaster.Brand_Id = '" + HttpContext.Current.Session["BrandId"].ToString() + "' and Inv_ProductMaster.ItemType='S'";
                        dtProduct = ObjDa.return_DataTable(strsql);                       
                        if(dtProduct!=null && dtProduct.Rows.Count.ToString()!= "")
                        {

                        for(int i = 0; i < dtProduct.Rows.Count; i++)
                        {

                            string Sql2 = "	insert into Inv_PhysicalDetail ( Company_Id,    Brand_Id,    Location_Id,    Header_Id,    VoucherNo,    SerialNo,    ProductId, [Date], RackId,UnitID,SystemQuantity, PhysicalQuantity, UnitCost,    Field1,    Field2,    Field3,    Field4,    Field5,    Field6,    Field7,  IsActive,    CreatedBy,    CreatedDate  )  values('"+HttpContext.Current.Session["CompId"].ToString().ToString()+"','"+HttpContext.Current.Session["BrandId"].ToString().ToString()+"', '"+HttpContext.Current.Session["LocId"].ToString()+"','"+b.ToString()+"', '0', '"+j.ToString()+"','"+dtProduct.Rows[i]["ProductId"].ToString()+"','"+ DateTime.Now.ToString()+"', '0','"+dtProduct.Rows[i]["Unit_Id"].ToString()+"','"+dtProduct.Rows[i]["SystemQuantity"].ToString()+"','"+dtProduct.Rows[i]["PhysicalQuantity"].ToString()+"','"+dtProduct.Rows[i]["UnitCost"].ToString()+"', '"+dtProduct.Rows[i]["RackName"].ToString()+"','"+ dtProduct.Rows[i]["AverageCost"].ToString()+"', '','','','"+true.ToString()+"','"+ DateTime.Now.ToString()+"','"+ true.ToString()+"','"+ HttpContext.Current.Session["UserId"].ToString().ToString()+"','"+ DateTime.Now.ToString()+ "');";
                             //ObjPhysicalDetail.InsertPhysicalDetail(HttpContext.Current.Session["CompId"].ToString().ToString(),HttpContext.Current.Session["BrandId"].ToString().ToString(), HttpContext.Current.Session["LocId"].ToString(), b.ToString(), "0", j.ToString(),dtProduct.Rows[i]["ProductId"].ToString(), DateTime.Now.ToString(), "0",dtProduct.Rows[i]["Unit_Id"].ToString(),dtProduct.Rows[i]["SystemQuantity"].ToString(),dtProduct.Rows[i]["PhysicalQuantity"].ToString(),dtProduct.Rows[i]["UnitCost"].ToString(), dtProduct.Rows[i]["RackName"].ToString(), dtProduct.Rows[i]["AverageCost"].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                            ObjDa.execute_Command(Sql2, HttpContext.Current.Session["DBConnection"].ToString());                           
                        }                  
                    }
                    DataTable dtDetail = new DataTable();
                    string strsql2 = "Select  Inv_PhysicalDetail.Trans_Id,Inv_PhysicalDetail.Header_Id,Inv_PhysicalDetail.SerialNo,Inv_ProductMaster.EProductName,Inv_PhysicalDetail.ProductId,Inv_ProductMaster.ProductCode,(Inv_PhysicalDetail.RackId)as RackName,Inv_UnitMaster.Unit_Name,Inv_PhysicalDetail.UnitCost , (Inv_PhysicalDetail.Field2)as AverageCost ,Inv_PhysicalDetail.SystemQuantity,Inv_PhysicalDetail.PhysicalQuantity From  Inv_PhysicalDetail   inner join Inv_UnitMaster on Inv_UnitMaster.Unit_Id=Inv_PhysicalDetail.UnitID inner join Inv_ProductMaster on  Inv_ProductMaster.ProductId= Inv_PhysicalDetail.ProductId where Inv_PhysicalDetail.Header_Id='" + b.ToString()+ "'";
                    dtDetail = ObjDa.return_DataTable(strsql2);
                    dtDetail = dtDetail.DefaultView.ToTable(true, "ProductId", "ProductCode","Trans_Id","Header_Id", "EProductName", "RackName", "Unit_Name", "UnitCost", "AverageCost", "SystemQuantity", "PhysicalQuantity");
                   HttpContext.Current.Session["FinalStockSheet"] = dtDetail;
                    foreach (DataRow row in dtDetail.Rows)
                        {
                            var dict = new Dictionary<string, object>();

                            foreach (DataColumn col in dtDetail.Columns)
                            {
                                dict[col.ColumnName] = (Convert.ToString(row[col]));
                            }
                            lstSno.Add(dict);
                        }                    
                        return JsonConvert.SerializeObject(lstSno);
                    }

                return null;
                }
            return null;
            }
            catch(Exception ex)
            {
                return null;
            }     
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (!rbtnAppend.Checked && !rbtnoverwrite.Checked)
        {
            DisplayMessage("Select Action");
            rbtnoverwrite.Focus();
            return;
        }
        if (fileLoad.HasFile)
        {
            fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
            string Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
            DataTable dt = ConvetExcelToDataTable(Path);
            List<object> lstSno = new List<object> { };
            DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            try
            {
                //dt = dt.DefaultView.ToTable(true, "ProductId", "ProductCode", "EProductName", "RackName", "Unit_Name", "UnitCost", "AverageCost", "SystemQuantity", "PhysicalQuantity", "DifferenceQty");
                dt = dt.DefaultView.ToTable(true, "ProductId", "ProductCode", "EProductName", "RackName", "Unit_Name", "UnitCost", "AverageCost", "SystemQuantity", "PhysicalQuantity");
                int b = 0;
                int j = 1;

                string Sql = "Insert Into Inv_PhysicalHeader values('" + HttpContext.Current.Session["CompId"].ToString().ToString() + "','" + HttpContext.Current.Session["BrandId"].ToString().ToString() + "','" + HttpContext.Current.Session["LocId"].ToString().ToString() + "','0','" + txtVoucherNo.Text + "', '" + ObjSysParam.getDateForInput(txtVoucherdate.Text).ToString() + "', '" + txtRemarks.Text + "', '" + txtNetAmount.Text + "', '" + false.ToString() + "','','','','','','" + true.ToString() + "','" + DateTime.Now.ToString() + "', '" + true.ToString() + "', '" + HttpContext.Current.Session["UserId"].ToString().ToString() + "', '" + DateTime.Now.ToString() + "','" + HttpContext.Current.Session["UserId"].ToString().ToString() + "', '" + DateTime.Now.ToString() + "') SELECT SCOPE_IDENTITY()";
                // b = ObjDa.execute_Command(Sql,HttpContext.Current.Session["DBConnection"].ToString()); 
                DataTable dtPhysical = ObjDa.return_DataTable(Sql);
                //  b = ObjPhysicalheader.InsertPhysicalHeader(HttpContext.Current.Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString().ToString(), HttpContext.Current.Session["LocId"].ToString().ToString(), "0", VoucherNo, ObjSysParam.getDateForInput(Voucherdate).ToString(), txtRemarks, txtNetAmount,false.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
                b = Convert.ToInt32(dtPhysical.Rows[0]["Column1"]);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable dtunit = objDa.return_DataTable("Select Unit_Id From Inv_UnitMaster Where Unit_Name='"+dt.Rows[i]["unit_Name"].ToString()+"'");

                    string Sql2 = "	insert into Inv_PhysicalDetail ( Company_Id,    Brand_Id,    Location_Id,    Header_Id,    VoucherNo,    SerialNo,    ProductId, [Date], RackId,UnitID,SystemQuantity, PhysicalQuantity, UnitCost,    Field1,    Field2,    Field3,    Field4,    Field5,    Field6,    Field7,  IsActive,    CreatedBy,    CreatedDate  )  values('" + HttpContext.Current.Session["CompId"].ToString().ToString() + "','" + HttpContext.Current.Session["BrandId"].ToString().ToString() + "', '" + HttpContext.Current.Session["LocId"].ToString() + "','" + b.ToString() + "', '0', '" + j.ToString() + "','" + dt.Rows[i]["ProductId"].ToString() + "','" + DateTime.Now.ToString() + "', '0','" + dtunit.Rows[0]["Unit_Id"].ToString() + "','" + dt.Rows[i]["SystemQuantity"].ToString() + "','" + dt.Rows[i]["PhysicalQuantity"].ToString() + "','" + dt.Rows[i]["UnitCost"].ToString() + "', '" + dt.Rows[i]["RackName"].ToString() + "','" + dt.Rows[i]["AverageCost"].ToString() + "', '','','','" + true.ToString() + "','" + DateTime.Now.ToString() + "','" + true.ToString() + "','" + HttpContext.Current.Session["UserId"].ToString().ToString() + "','" + DateTime.Now.ToString() + "');";
                    //ObjPhysicalDetail.InsertPhysicalDetail(HttpContext.Current.Session["CompId"].ToString().ToString(),HttpContext.Current.Session["BrandId"].ToString().ToString(), HttpContext.Current.Session["LocId"].ToString(), b.ToString(), "0", j.ToString(),dtProduct.Rows[i]["ProductId"].ToString(), DateTime.Now.ToString(), "0",dtProduct.Rows[i]["Unit_Id"].ToString(),dtProduct.Rows[i]["SystemQuantity"].ToString(),dtProduct.Rows[i]["PhysicalQuantity"].ToString(),dtProduct.Rows[i]["UnitCost"].ToString(), dtProduct.Rows[i]["RackName"].ToString(), dtProduct.Rows[i]["AverageCost"].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                    ObjDa.execute_Command(Sql2, HttpContext.Current.Session["DBConnection"].ToString());
                }
                string strsql2 = "Select  Inv_PhysicalDetail.Trans_Id,Inv_PhysicalDetail.Header_Id,Inv_PhysicalDetail.SerialNo,Inv_ProductMaster.EProductName,Inv_PhysicalDetail.ProductId,Inv_ProductMaster.ProductCode,(Inv_PhysicalDetail.RackId)as RackName,Inv_UnitMaster.Unit_Name,Inv_PhysicalDetail.UnitCost , (Inv_PhysicalDetail.Field2)as AverageCost ,Inv_PhysicalDetail.SystemQuantity,Inv_PhysicalDetail.PhysicalQuantity From  Inv_PhysicalDetail   inner join Inv_UnitMaster on Inv_UnitMaster.Unit_Id=Inv_PhysicalDetail.UnitID inner join Inv_ProductMaster on  Inv_ProductMaster.ProductId= Inv_PhysicalDetail.ProductId where Inv_PhysicalDetail.Header_Id='" + b.ToString() + "'";
               DataTable dtDetail = ObjDa.return_DataTable(strsql2);
                dtDetail = dtDetail.DefaultView.ToTable(true, "ProductId", "ProductCode", "Trans_Id", "Header_Id", "EProductName", "RackName", "Unit_Name", "UnitCost", "AverageCost", "SystemQuantity", "PhysicalQuantity");
                 HttpContext.Current.Session["FinalStockSheet"] = dtDetail;
                foreach (DataRow row in dtDetail.Rows)
                {
                    var dict = new Dictionary<string, object>();

                    foreach (DataColumn col in dtDetail.Columns)
                    {
                        dict[col.ColumnName] = (Convert.ToString(row[col]));
                    }
                    lstSno.Add(dict);
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "EditRecord(" + JsonConvert.SerializeObject(lstSno) + ")", true);
                //return JsonConvert.SerializeObject(lstSno);
            }            
            catch (Exception ex)
            {
                DisplayMessage("Error Found");
               // objPageCmn.FillData((object)gvProduct, null, "", "");
                return;
            }//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015


            if (rbtnoverwrite.Checked)
            {

               // objPageCmn.FillData((object)gvProduct, dt, "", "");

            }
            else if (rbtnAppend.Checked)
            {

                //if (gvProduct.Rows.Count == 0)
                //{

                // //   objPageCmn.FillData((object)gvProduct, dt, "", "");
                //}
                //else
                //{
                //    //foreach (GridViewRow gvrow in gvProduct.Rows)
                //    //{
                //    //    Label lblProductId = (Label)gvrow.FindControl("lblPID");
                //    //    Label lblsystemQty = (Label)gvrow.FindControl("lblSystemQuantity");
                //    //    TextBox txtRackname = (TextBox)gvrow.FindControl("txtRackname");

                //    //    TextBox txtPhysicalQty = (TextBox)gvrow.FindControl("txtPhysicalQuantity");
                //    //    TextBox txtAverageCost = (TextBox)gvrow.FindControl("lblUnitCost");
                //    //    Label lblUnitCost = (Label)gvrow.FindControl("lblUnitCost12");

                //    //    Label lblUnitId = (Label)gvrow.FindControl("lblUnitId");


                //    //    if (txtPhysicalQty.Text == "")
                //    //    {
                //    //        txtPhysicalQty.Text = "0";
                //    //    }


                //    //    DataTable dtTemp = new DataView(dt, "ProductId=" + lblProductId.Text + "", "", DataViewRowState.CurrentRows).ToTable();


                //    //    if (dtTemp.Rows.Count > 0)
                //    //    {

                //    //        if (dtTemp.Rows[0]["RackName"].ToString().Trim() != "")
                //    //        {

                //    //            txtRackname.Text = dtTemp.Rows[0]["RackName"].ToString().Trim();
                //    //        }

                //    //        txtPhysicalQty.Text = SetDecimal((float.Parse(txtPhysicalQty.Text) + float.Parse(dtTemp.Rows[0]["PhysicalQuantity"].ToString().Trim())).ToString());


                //    //    }
                //    //}
                //}
            }

            //GetGridTotal();
            //CollapsiblePanelExtender.Collapsed = true;

            Session["FinalStockSheet"] = dt;

        }
        else
        {
            DisplayMessage("File not Found");
           // objPageCmn.FillData((object)gvProduct, null, "", "");
            return;
        }

    }

    //public void GetGridTotal()
    //{
    //    double SystemAmount = 0;
    //    double PhysicalAmount = 0;
    //    foreach (GridViewRow row in gvProduct.Rows)
    //    {

    //        if (((TextBox)row.FindControl("lblUnitCost")).Text.Trim() == "")
    //        {
    //            ((TextBox)row.FindControl("lblUnitCost")).Text = "0";
    //        }

    //        if (((Label)row.FindControl("lblSystemQuantity")).Text.Trim() == "")
    //        {
    //            ((Label)row.FindControl("lblSystemQuantity")).Text = "0";
    //        }
    //        if (((TextBox)row.FindControl("txtPhysicalQuantity")).Text.Trim() == "")
    //        {
    //            ((TextBox)row.FindControl("txtPhysicalQuantity")).Text = "0";
    //        }

    //        ((Label)row.FindControl("txtDifferenceQuantity")).Text = SetDecimal((float.Parse(((TextBox)row.FindControl("txtPhysicalQuantity")).Text) - float.Parse(((Label)row.FindControl("lblSystemQuantity")).Text)).ToString());


    //        SystemAmount += Convert.ToDouble(((Label)row.FindControl("lblSystemQuantity")).Text) * Convert.ToDouble(((TextBox)row.FindControl("lblUnitCost")).Text);
    //        PhysicalAmount += Convert.ToDouble(((TextBox)row.FindControl("txtPhysicalQuantity")).Text) * Convert.ToDouble(((TextBox)row.FindControl("lblUnitCost")).Text);


    //    }
    //    txtNetAmount.Text = SetDecimal(SystemAmount.ToString());
    //    txtPhysical.Text = SetDecimal(PhysicalAmount.ToString());
    //}



    public DataTable GetUnpostedRecord()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("PageName");
        dt.Columns.Add("UnpostRecord", typeof(Int32));

        DataRow dr;
        //for transfer
        dr = dt.NewRow();

        dr[0] = "Transfer Voucher";
        dr[1] = objDa.return_DataTable("select COUNT(*) from Inv_TransferHeader where FromLocationID=" + Session["LocId"].ToString() + " and Post='N' and IsActive='True'").Rows[0][0].ToString();

        dt.Rows.Add(dr);

        //for adjustment
        dr = dt.NewRow();

        dr[0] = "Stock Adjustment";
        dr[1] = objDa.return_DataTable("select COUNT(*) from Inv_AdjustHeader where FromLocationID=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False'").Rows[0][0].ToString();

        dt.Rows.Add(dr);



        ////for adjustment
        //dr = dt.NewRow();

        //dr[0] = "Physical Inventory";
        //dr[1] = objDa.return_DataTable("select COUNT( *) from Inv_PhysicalHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False'").Rows[0][0].ToString();

        //dt.Rows.Add(dr);



        //purchase Invoice
        dr = dt.NewRow();

        dr[0] = "Purchase Invoice";
        dr[1] = objDa.return_DataTable("select COUNT ( *) from Inv_PurchaseInvoiceHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False'").Rows[0][0].ToString();

        dt.Rows.Add(dr);



        //purchase Return
        dr = dt.NewRow();

        dr[0] = "Purchase Return";
        dr[1] = objDa.return_DataTable("select COUNT(*) from Inv_PurchaseReturnHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Field1='False'").Rows[0][0].ToString();

        dt.Rows.Add(dr);


        //Sales Invoice
        dr = dt.NewRow();

        dr[0] = "Sales Invoice";
        dr[1] = objDa.return_DataTable("select count(*) from Inv_SalesInvoiceHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False'").Rows[0][0].ToString();

        dt.Rows.Add(dr);



        //Delivery Voucher
        dr = dt.NewRow();

        dr[0] = "Delivery Voucher";
        dr[1] = objDa.return_DataTable("select  COUNT(*) from Inv_SalesDeliveryVoucher_Header where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='false'").Rows[0][0].ToString();

        dt.Rows.Add(dr);



        //Sales Return
        dr = dt.NewRow();

        dr[0] = "Sales Return";
        dr[1] = objDa.return_DataTable("select COUNT(*) from Inv_SalesReturnHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False'").Rows[0][0].ToString();

        dt.Rows.Add(dr);


        //Sales Return
        dr = dt.NewRow();

        dr[0] = "Production Voucher";
        dr[1] = objDa.return_DataTable("select COUNT( *) from Inv_Production_Process where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Is_Post='False'").Rows[0][0].ToString();

        dt.Rows.Add(dr);




        dt = new DataView(dt, "UnpostRecord<>0", "", DataViewRowState.CurrentRows).ToTable();

        return dt;

    }
    public string SetDecimal(string amount)
    {
        if (amount == "")
        {
            amount = "0";
        }
        return ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), amount);
    }

    public void ExportTableData(DataTable Dt_Data, string FName)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Dt_Data, FName);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FName + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                try
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //Response.End();
                    Response.Close();
                }
                catch (Exception ex)
                {
                    DisplayMessage(ex.ToString());
                }
            }
        }
    }


    public DataTable ConvetExcelToDataTable(string path)
    {
        DataTable dt = new DataTable();
        string strcon = string.Empty;
        if (Path.GetExtension(path) == ".xls")
        {
            strcon = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + path + "; Extended Properties =\"Excel 8.0;HDR=YES;\"";
        }
        else if (Path.GetExtension(path) == ".xlsx")
        {
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        try
        {
            OleDbConnection oledbConn = new OleDbConnection(strcon);
            oledbConn.Open();
            DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string strquery = "select * from [" + Sheets.Rows[0]["Table_Name"].ToString() + "] ";
            OleDbCommand com = new OleDbCommand(strquery, oledbConn);
            DataSet ds = new DataSet();
            OleDbDataAdapter oledbda = new OleDbDataAdapter(com);
            oledbda.Fill(ds, Sheets.Rows[0]["Table_Name"].ToString());
            oledbConn.Close();
            dt = ds.Tables[0];
        }
        catch
        {
            DisplayMessage("Excel file should in correct format");
        }
        return dt;
    }

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Fillgrid();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {

        //if (gvProduct.Rows.Count > 0)
        //{

        //}
        //else
        //{
        //    DisplayMessage("Record Not Available");
        //    return;
        //}

        if (Session["FinalStockSheet"] == null)
        {

        }
        else
        {
            ExportTableData((DataTable)Session["FinalStockSheet"], "Product Stock List");
        }

    }


    #region Print


    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Inventory_Report/PhysicalReport.aspx?Id=" + e.CommandArgument.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }


    #endregion


    #region ScanningSolution


    protected void txtScanProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtScanProductCode.Text != "")
        {
            DataTable dtProduct = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtScanProductCode.Text);
            if (dtProduct == null)
            {
                DisplayMessage("Product not found");
                txtScanProductCode.Focus();
                txtScanProductCode.Text = "";
                return;
            }
            // dtProduct = new DataView(dtProduct, "ProductCode ='" + txtProductcode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                lblProductCode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
                lblProuctName.Text = dtProduct.Rows[0]["EProductName"].ToString();

                //foreach (GridViewRow gvrow in gvProduct.Rows)
                //{
                //    if (dtProduct.Rows[0]["ProductId"].ToString().Trim() == ((Label)gvrow.FindControl("lblPID")).Text.Trim())
                //    {
                //        lblSystemQty.Text = SetDecimal(((Label)gvrow.FindControl("lblSystemQuantity")).Text);
                //        lblPhysicalQuantity.Text = SetDecimal(((TextBox)gvrow.FindControl("txtPhysicalQuantity")).Text);

                //        ViewState["RowIndex"] = gvrow.RowIndex;


                //        break;

                //    }
                //}

            }
            else
            {
                txtScanProductCode.Text = "";

                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtScanProductCode);
                return;
            }
        }
        else
        {

            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtScanProductCode);
        }

        txtNewqty.Focus();
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


    protected void btnadd_OnClick(object sender, EventArgs e)
    {

        if (txtScanProductCode.Text.Trim() == "")
        {
            DisplayMessage("Product Not Found");
            txtScanProductCode.Focus();
            return;

        }



        if (lblPhysicalQuantity.Text == "")
        {
            lblPhysicalQuantity.Text = "0";
        }

        if (txtNewqty.Text == "")
        {
            txtNewqty.Text = "0";
        }


        //if (((TextBox)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblUnitCost")).Text == "")
        //{
        //    ((TextBox)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblUnitCost")).Text = "0";
        //}


        //if (rbtnDetailAppend.Checked)
        //{
        //    ((TextBox)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtPhysicalQuantity")).Text = SetDecimal((float.Parse(lblPhysicalQuantity.Text) + float.Parse(txtNewqty.Text)).ToString());
        //}
        //else if (rbtnDetailOverwrite.Checked)
        //{
        //    ((TextBox)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtPhysicalQuantity")).Text = SetDecimal(float.Parse(txtNewqty.Text).ToString());

        //}

        //txtPhysical.Text = SetDecimal((float.Parse(txtPhysical.Text) - (float.Parse(lblPhysicalQuantity.Text) * float.Parse(((TextBox)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblUnitCost")).Text)) + (float.Parse(((TextBox)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtPhysicalQuantity")).Text) * float.Parse(((TextBox)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblUnitCost")).Text))).ToString());


        //lblPhysicalQuantity.Text = ((TextBox)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtPhysicalQuantity")).Text;


        //((Label)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtDifferenceQuantity")).Text = SetDecimal((float.Parse(((TextBox)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtPhysicalQuantity")).Text) - float.Parse(((Label)gvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("lblSystemQuantity")).Text)).ToString());

        txtNewqty.Text = "0";
        DisplayMessage("Record Updated", "green");
        txtScanProductCode.Focus();
        txtScanProductCode.Text = "";
        lblProductCode.Text = "";
        lblProuctName.Text = "";
        lblSystemQty.Text = "";
        lblPhysicalQuantity.Text = "";
    }




    #endregion


    public class clsResult
    {
        public bool bResult { get; set; }
        public List<object> objResultLst { get; set; }
        public string msg { get; set; }
    }

    public class clsSearch
    {
        public string searchfieldName { get; set; }
        public string searchOperator { get; set; }
        public string searchText { get; set; }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        string filtertext = "AccountName like '%" + prefixText + "%'";
        dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }

        return txt;
    }


    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True'";
                DataTable dtCOA = objDa.return_DataTable(sql);

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose Account in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {
                DisplayMessage("Choose Account in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }
    }

    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {
        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                string Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
            }
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetScanProductDetail(string strVoucherId, string strScanText)
    {
        clsResult _clsResult = new clsResult();
        Inv_ProductMaster ObjProductMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        if (!string.IsNullOrEmpty(strScanText))
        {
            try
            {
                using (DataTable _dt = ObjProductMaster.GetProductIdByScanning(HttpContext.Current.Session["CompId"].ToString(), strVoucherId, strScanText))
                {
                    if (_dt.Rows.Count > 0)
                    {
                        clsProductDetail _obj = new clsProductDetail();
                        _obj.productId = _dt.Rows[0]["ProductId"].ToString();
                        _obj.productCode = _dt.Rows[0]["ProductCode"].ToString();
                        _obj.productName = _dt.Rows[0]["EProductName"].ToString();
                        _obj.RackNo = _dt.Rows[0]["rackId"].ToString();
                        _obj.sysQty = double.Parse(_dt.Rows[0]["SystemQuantity"].ToString()).ToString("0.00");
                        _obj.serialNo = _dt.Rows[0]["serial"].ToString();
                        _obj.phyQty = "0";
                        _clsResult.bResult = true;
                        _clsResult.objResultLst = new List<object> { };
                        _clsResult.objResultLst.Add(_obj);
                        return new JavaScriptSerializer().Serialize(_clsResult);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        return null;
    }

    protected void GetVoucherID(object sender, EventArgs e)
    {
        string voucherId = txtScanVoucherNo.Text;
        DataTable dt = new DataTable();
        dt = objDa.return_DataTable("Select*from ");

    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string UpdateScanProductDetail(clsProductDetail _cls)
    {
        if (_cls == null)
        {
            return null;
        }
        clsResult _clsResult = new clsResult();
        Inv_PhysicalHeader objPhyHLogs = new Inv_PhysicalHeader(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            int _id = objPhyHLogs.InsertPhysicalHeaderLogs(_cls.voucherId, _cls.productId, _cls.serialNo, _cls.phyQty, string.IsNullOrEmpty(_cls.RackNo) || _cls.RackNo == "--Select--" ? "0" : _cls.RackNo, HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
            if (_id == 0)
            {
                _clsResult.bResult = false;
            }
            else
            {
                _clsResult.bResult = true;
            }
        }
        catch (Exception ex)
        {
            _clsResult.bResult = false;
            _clsResult.msg = ex.Message;
        }
        return new JavaScriptSerializer().Serialize(_clsResult);
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string PostScanLogs(string strVoucherId)
    {
        if (string.IsNullOrEmpty(strVoucherId) || strVoucherId == "0")
        {
            return null;
        }
        clsResult _clsResult = new clsResult();
        Inv_PhysicalHeader objPhyHLogs = new Inv_PhysicalHeader(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            int b = objPhyHLogs.PostPhysicalHeaderLogs(strVoucherId, HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
            _clsResult.bResult = true;
            _clsResult.msg = b.ToString() + " Records have updated";
        }
        catch (Exception ex)
        {
            _clsResult.bResult = false;
            _clsResult.msg = ex.Message;
        }
        return new JavaScriptSerializer().Serialize(_clsResult);
    }

    public class lstPhyOpenVoucher
    {
        public string trans_id { get; set; }
        public string voucher_no { get; set; }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetOpenVoucherList(string strLocationId)
    {
        Inv_PhysicalHeader objPhysicalHeader = new Inv_PhysicalHeader(HttpContext.Current.Session["DBConnection"].ToString());
        clsResult _clsResult = new clsResult();
        _clsResult.objResultLst = new List<object> { };
        try
        {
            if (!string.IsNullOrEmpty(strLocationId))
            {
                using (DataTable _dt = objPhysicalHeader.GetOpenPhysicalHeadersVoucher(HttpContext.Current.Session["CompId"].ToString(), strLocationId))
                {
                    if (_dt.Rows.Count > 0)
                    {
                        lstPhyOpenVoucher _cls = new lstPhyOpenVoucher();
                        _cls.trans_id = _dt.Rows[0]["trans_id"].ToString();
                        _cls.voucher_no = _dt.Rows[0]["VoucherNo"].ToString();
                        _clsResult.bResult = true;
                        _clsResult.objResultLst.Add(_cls);
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }

        return new JavaScriptSerializer().Serialize(_clsResult);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetScanProductList(string strVoucherId, clsSearch objSearch = null)
    {
        clsResult _clsResult = new clsResult();
        Inv_PhysicalHeader objPh = new Inv_PhysicalHeader(HttpContext.Current.Session["DBConnection"].ToString());
        if (!string.IsNullOrEmpty(strVoucherId))
        {
            try
            {
                using (DataTable _dt = objPh.GetPhyHeaderLogsByVoucherId(strVoucherId))
                {
                    if (_dt.Rows.Count > 0)
                    {
                        DataTable _dtNew = new DataTable();
                        if (objSearch != null)
                        {
                            string strWhereClause = "1=1";
                            if (objSearch.searchOperator == "Equal")
                            {
                                strWhereClause += " and " + objSearch.searchfieldName + " = '" + objSearch.searchOperator + "'";
                            }
                            else if (objSearch.searchOperator == "Like")
                            {
                                strWhereClause += " and " + objSearch.searchfieldName + " Like '%" + objSearch.searchText + "%'";
                            }
                            _dtNew = new DataView(_dt, strWhereClause, "", DataViewRowState.CurrentRows).ToTable();
                        }
                        else
                        {
                            _dtNew = _dt;
                        }

                        if (_dtNew.Rows.Count > 0)
                        {
                            _clsResult.bResult = true;
                            _clsResult.objResultLst = new List<object> { };
                            foreach (DataRow _dr in _dtNew.Rows)
                            {
                                clsProductDetail _obj = new clsProductDetail();
                                _obj.productId = _dr["Product_Id"].ToString();
                                _obj.productCode = _dr["ProductCode"].ToString();
                                _obj.productName = _dr["EProductName"].ToString();
                                _obj.RackNo = _dr["rack_Id"].ToString();
                                _obj.sysQty = double.Parse(_dr["SystemQuantity"].ToString()).ToString("0.00");
                                _obj.serialNo = _dr["serial_no"].ToString();
                                _obj.phyQty = double.Parse(_dr["phy_qty"].ToString()).ToString("0.00");
                                _clsResult.objResultLst.Add(_obj);
                            }
                            return new JavaScriptSerializer().Serialize(_clsResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        return null;
    }

    public class clsProductDetail
    {
        public string productId { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string sysQty { get; set; }
        public string RackNo { get; set; }
        public string serialNo { get; set; }
        public string phyQty { get; set; }
        public string voucherId { get; set; }
    }


    public void FillScanLocationList()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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
            objPageCmn.FillData((object)ddlScanLocation, dtLoc, "Location_Name", "Location_Id");
            ddlScanLocation.Items.RemoveAt(0);
            ddlScanLocation.SelectedValue = Session["LocId"].ToString();
        }
        else
        {
            ddlScanLocation.Items.Insert(0, "--Select--");
            ddlScanLocation.SelectedIndex = 0;
        }
    }

    public void fillRackList()
    {
        using (DataTable _dt = ObjRackMaster.GetRackMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()))
        {
            if (_dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlScanRackNo, _dt, "Rack_Name", "Rack_ID");
                //ddlScanLocation.Items.RemoveAt(0);
            }
            else
            {
                ddlScanLocation.Items.Insert(0, "--Select--");
                ddlScanLocation.SelectedIndex = 0;
            }
        }
    }
}
