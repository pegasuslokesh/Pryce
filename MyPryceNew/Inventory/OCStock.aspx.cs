using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;
using ClosedXML.Excel;
using PegasusDataAccess;

public partial class Inventory_OCStock : BasePage
{
    #region Defined Class object

    Inv_StockDetail objstockDetail = null;
    SystemParameter ObjSysParam = null;
    Inv_ProductLedger ObjProductLadger = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    IT_ObjectEntry objObjectEntry = null;
    LocationMaster objLocation = null;
    Inv_ProductMaster objProductMaster = null;
    Inv_ProductBrandMaster ObjProductBrandMaster = null;
    Common cmn = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    Inv_Product_Brand ObjProductBrand = null;
    Inv_Product_Category ObjProductCate = null;
    DataAccessClass objDa = null;

    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string UserId = string.Empty;
    #endregion
    protected string FuLogo_UploadFolderPath = "~/Temp/";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        UserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();

        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;


        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        //End Code

        objstockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjProductBrandMaster = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        ObjProductBrand = new Inv_Product_Brand(Session["DBConnection"].ToString());
        ObjProductCate = new Inv_Product_Category(Session["DBConnection"].ToString());
        Page.Title = ObjSysParam.GetSysTitle();
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("226", (DataTable)Session["ModuleName"]);
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
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "226", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            btnList_Click(null, null);
            txtSerialNo.Attributes["onkeydown"] = String.Format("count('{0}')", txtSerialNo.ClientID);
            LoadStores();
            // string Path = Server.MapPath("~/Temp/test.csv");
            Session["dtFilter_OC_Stock"] = null;
            Session["dtFinal"] = null;
            Session["dtSerial"] = null;
            FillProductBrand();
            FillProductCategory();

            //here we modify code for show cost price according the login user permission
            //code created by jitendra on 13-08-2016

        }


        try
        {
            gvProductStock.Columns[4].HeaderText = SystemParameter.GetCurrencySmbol(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Resources.Attendance.Unit_Cost, Session["DBConnection"].ToString());
            gvProductStock.Columns[4].Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
        }
        catch
        {

        }
        //Use this datatable and read its contents into TextBox
    }


    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }

    #region ProductBrand/category

    public void FillProductBrand()
    {
        DataTable dt = ObjProductBrandMaster.GetProductBrandTrueAllData(StrCompId.ToString());
        try
        {
            objPageCmn.FillData((object)ddlbrandsearch, dt, "Brand_Name", "PBrandId");
        }
        catch
        {
            ddlbrandsearch.Items.Insert(0, "--Select One--");
            ddlbrandsearch.SelectedIndex = 0;
        }
    }

    private void FillProductCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(StrCompId.ToString());
        if (dsCategory.Rows.Count > 0)
        {

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlcategorysearch, dsCategory, "Category_Name", "Category_Id");

        }
        else
        {
            ddlcategorysearch.Items.Insert(0, "--Select One--");
            ddlcategorysearch.SelectedIndex = 0;
        }
    }


    protected void btngo_Click(object sender, EventArgs e)
    {

        DataTable dt = objstockDetail.GetOpeningStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on  05-05-2015



        string strProductId = string.Empty;

        if (ddlbrandsearch.SelectedIndex != 0)
        {

            DataTable dtProductBrand = ObjProductBrand.GetDataBrandId(StrCompId.ToString(), Session["BrandId"].ToString(), ddlbrandsearch.SelectedValue);
            for (int i = 0; i < dtProductBrand.Rows.Count; i++)
            {
                if (!strProductId.Split(',').ToString().Contains(dtProductBrand.Rows[i]["ProductId"].ToString()))
                {

                    strProductId += dtProductBrand.Rows[i]["ProductId"].ToString() + ",";
                }
            }
        }

        if (ddlcategorysearch.SelectedIndex != 0)
        {

            DataTable dtProductCate = ObjProductCate.GetProductByCategoryId(StrCompId.ToString(), Session["BrandId"].ToString(), ddlcategorysearch.SelectedValue);
            for (int i = 0; i < dtProductCate.Rows.Count; i++)
            {

                if (!strProductId.Split(',').ToString().Contains(dtProductCate.Rows[i]["ProductId"].ToString()))
                {
                    strProductId += dtProductCate.Rows[i]["ProductId"].ToString() + ",";
                }
            }

        }

        if (strProductId != "")
        {
            dt = new DataView(dt, "ProductId in (" + strProductId.Substring(0, strProductId.Length - 1).ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        objPageCmn.FillData((object)gvProductStock, dt, "", "");
        Session["DtOC"] = dt;
        Session["dtFilter_OC_Stock"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
    }


    protected void btnResetSreach_Click(object sender, EventArgs e)
    {
        ddlbrandsearch.SelectedIndex = 0;
        ddlcategorysearch.SelectedIndex = 0;
        btnRefresh_Click(null, null);
        btngo_Click(null, null);
    }

    #endregion
    public void fillgrid()
    {

        DataTable dt = objstockDetail.GetOpeningStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on  05-05-2015

        //11-03-2023
        // objPageCmn.FillData((object)gvProductStock, dt, "", "");

        Session["DtOC"] = dt;
        Session["dtFilter_OC_Stock"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";

    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        fillgrid();
    }

    public float findRemainingserialnumber(int rowno)
    {

        float Remainingseruialnumber = 0;

        DataTable dtstockbatchmaster = ObjStockBatchMaster.GetStockBatchMasterAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        dtstockbatchmaster = new DataView(dtstockbatchmaster, "TransType='OP' and ProductId='" + ((HiddenField)gvProductStock.Rows[rowno].FindControl("HdnProductId")).Value + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (float.Parse(((Label)gvProductStock.Rows[rowno].FindControl("lblQuantity")).Text) != dtstockbatchmaster.Rows.Count)
        {
            Remainingseruialnumber = float.Parse(((Label)gvProductStock.Rows[rowno].FindControl("lblQuantity")).Text) - dtstockbatchmaster.Rows.Count;
        }


        return Remainingseruialnumber;
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
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
            DataTable dtAdd = (DataTable)Session["DtOC"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);

            Session["dtFilter_OC_Stock"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvProductStock, view.ToTable(), "", "");

        }


        try
        {

            gvProductStock.HeaderRow.Cells[4].Text = SystemParameter.GetCurrencySmbol(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Resources.Attendance.Unit_Cost, Session["DBConnection"].ToString());


        }
        catch
        {

        }
        txtValue.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        if (ddlcategorysearch.SelectedIndex != 0 || ddlbrandsearch.SelectedIndex != 0)
        {
            btngo_Click(null, null);
        }
        else
        {
            fillgrid();
        }
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        try
        {

            gvProductStock.HeaderRow.Cells[4].Text = SystemParameter.GetCurrencySmbol(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Resources.Attendance.Unit_Cost, Session["DBConnection"].ToString());


        }
        catch
        {

        }
    }
    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (Inventory_Common.GetPhyscialInventoryStatus(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Physical Inventory work is under processing ,you can not post.");
            return;
        }


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            string Qty = string.Empty;

            foreach (GridViewRow Row in gvProductStock.Rows)
            {
                string ProductId = ((HiddenField)Row.FindControl("HdnProductId")).Value;
                Qty = ((TextBox)Row.FindControl("txtQuantity")).Text.Trim();

                if (Qty != "" && Qty != "0")
                {
                    DataTable dtstock = objstockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ProductId);
                    if (dtstock.Rows.Count == 0)
                    {
                        string Unit_Price = string.Empty;
                        if (((TextBox)Row.FindControl("txtUnitPrice")).Text != "")
                        {
                            Unit_Price = ((TextBox)Row.FindControl("txtUnitPrice")).Text;
                        }
                        else
                        {
                            Unit_Price = "0";
                        }

                        b = objstockDetail.InsertStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ProductId.ToString(), Qty, "0", "0", "0", "0", "0", "0", "0", "0", Unit_Price, Unit_Price, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);


                        b = ObjProductLadger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "OP", "0", "0", ProductId, ((HiddenField)Row.FindControl("HdnUnitId")).Value, "I", "0", "0", Qty, "0", "1/1/1800", Unit_Price, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);


                        if (Session["dtFinal"] != null)
                        {
                            DataTable dt = (DataTable)Session["dtFinal"];
                            dt = new DataView(dt, "ProductId=" + ProductId, "", DataViewRowState.CurrentRows).ToTable();
                            foreach (DataRow dr in dt.Rows)
                            {
                                ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "OP", "0", ProductId, ((HiddenField)Row.FindControl("HdnUnitId")).Value, "I", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), dr["ManufacturerDate"].ToString(), dr["BarcodeNo"].ToString(), dr["Width"].ToString(), dr["Length"].ToString(), dr["Pallet_ID"].ToString(), "", "", "", b.ToString(), "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);

                            }
                        }
                    }
                    else
                    {

                        if (Session["dtFinal"] != null)
                        {
                            DataTable dt = (DataTable)Session["dtFinal"];
                            dt = new DataView(dt, "ProductId=" + ProductId, "", DataViewRowState.CurrentRows).ToTable();
                            foreach (DataRow dr in dt.Rows)
                            {
                                ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "OP", "0", ProductId, ((HiddenField)Row.FindControl("HdnUnitId")).Value, "I", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), dr["ManufacturerDate"].ToString(), dr["BarcodeNo"].ToString(), dr["Width"].ToString(), dr["Length"].ToString(), dr["Pallet_ID"].ToString(), "", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);

                            }
                        }

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
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString().Replace("'", " ") + " Line Number : " + Common.GetLineNumber(ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }

        //fillgrid();
        //if (b != 0)
        //{

        DisplayMessage("Record Posted Successfully");
        //}
        Session["dtFinal"] = null;
    }
    //protected void gvProductStock_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {


    //        DataTable dtstock = new DataView(objstockDetail.GetStockDetail(StrCompId, StrBrandId, StrLocationId), "ProductId='" + ((HiddenField)e.Row.FindControl("HdnProductId")).Value + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        if (dtstock.Rows.Count == 0)
    //        {
    //            ((TextBox)e.Row.FindControl("txtQuantity")).Enabled = false;
    //            ((TextBox)e.Row.FindControl("txtQuantity")).Visible = true;
    //            ((Label)e.Row.FindControl("lblQuantity")).Visible = false;
    //            ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
    //            ((TextBox)e.Row.FindControl("txtUnitPrice")).Visible = true;
    //            ((Label)e.Row.FindControl("lblUnitPrice")).Visible = false;

    //            if (objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)e.Row.FindControl("HdnProductId")).Value).Rows[0]["itemtype"].ToString() == "S" && objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)e.Row.FindControl("HdnProductId")).Value).Rows[0]["MaintainStock"].ToString() == "NM")
    //            {
    //                ((TextBox)e.Row.FindControl("txtQuantity")).Enabled = true;
    //                ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
    //            }
    //             btnPost.Visible = true;

    //             //float QtyCount = 0;
    //             //if (Session["dtFinal"] != null)
    //             //{
    //             //    DataTable Dtfinal = new DataView((DataTable)Session["dtFinal"], "ProductId='" +((HiddenField)e.Row.FindControl("HdnProductId")).Value+ "'", "", DataViewRowState.CurrentRows).ToTable();

    //             //    foreach (DataRow dr in Dtfinal.Rows)
    //             //    {
    //             //        try
    //             //        {
    //             //            QtyCount += float.Parse(dr["Quantity"].ToString());
    //             //        }
    //             //        catch
    //             //        {
    //             //            QtyCount += 0;
    //             //        }
    //             //    }
    //             //}
    //             //if (QtyCount > 0)
    //             //{
    //             //    ((TextBox)e.Row.FindControl("txtQuantity")).Text = QtyCount.ToString();
    //             //}
    //        }
    //        else
    //        {
    //            ((TextBox)e.Row.FindControl("txtQuantity")).Visible = false;
    //            ((Label)e.Row.FindControl("lblQuantity")).Visible = true;
    //            ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
    //            ((TextBox)e.Row.FindControl("txtUnitPrice")).Visible = false;
    //            ((Label)e.Row.FindControl("lblUnitPrice")).Visible = true;


    //        }
    //    }



    //}
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
    public string SetDecimal(string amount)
    {
        if (amount == "")
        {
            amount = "0";
        }
        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
    }
    #region upload
    protected void btnExport_Click(object sender, EventArgs e)
    {

        DataTable dt = objstockDetail.GetOpeningStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());

        if (dt.Rows.Count > 0)
        {

            dt = dt.DefaultView.ToTable(true, "ProductId", "ProductCode", "EProductName", "UnitPrice", "Quantity");

            ExportTableData(dt, "OpeningStock");
        }
        else
        {
            DisplayMessage("Record Not Found");
            return;
        }


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
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    //protected void btnConnect_Click(object sender, EventArgs e)
    //{

    //    if (fileLoad.HasFile)
    //    {
    //        fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
    //        string Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
    //        DataTable dt = ConvetExcelToDataTable(Path);

    //        if (dt != null)
    //        {
    //            try
    //            {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
    //                objPageCmn.FillData((object)gvProductStock, dt, "", "");
    //                Session["DtOC"] = dt;
    //                Session["dtFilter_OC_Stock"] = dt;
    //                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
    //            }
    //            catch
    //            {
    //            }

    //            fileLoad = null;


    //            //    int b = 0;

    //            //    for (int i = 0; i < dt.Rows.Count; i++)
    //            //    {

    //            //        if (dt.Rows[i]["Quantity"].ToString() != "" && dt.Rows[i]["Quantity"].ToString() != "0")
    //            //        {
    //            //            DataTable dtstock = new DataView(objstockDetail.GetStockDetail(StrCompId, StrBrandId, StrLocationId), "ProductId='" + dt.Rows[i]["ProductId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //            //            if (dtstock.Rows.Count == 0)
    //            //            {
    //            //                b = objstockDetail.InsertStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[i]["ProductId"].ToString(), dt.Rows[i]["Quantity"].ToString(), "0", "0", "0", "0", "0", "0", "0", "0", dt.Rows[i]["UnitPrice"].ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

    //            //                b = ObjProductLadger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "OP", "0", "0", dt.Rows[i]["ProductId"].ToString(), "0", "I", "0", "0", dt.Rows[i]["Quantity"].ToString(), "0", "1/1/1800", dt.Rows[i]["UnitPrice"].ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

    //            //            }


    //            //        }




    //            //    }
    //            //    if (b != 0)
    //            //    {

    //            //        DisplayMessage("Record Updated Successfully", "green");

    //            //    }
    //            //}
    //            //else
    //            //{
    //            //    DisplayMessage("Record Not Found");
    //            //    return;
    //            //}
    //        }
    //    }
    //    else
    //    {
    //        DisplayMessage("File Not Found");
    //        return;
    //    }

    //}
    //public DataTable ConvetExcelToDataTable(string path)
    //{
    //    DataTable dt = new DataTable();
    //    string strcon = string.Empty;
    //    if (Path.GetExtension(path) == ".xls")
    //    {
    //        strcon = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + path + "; Extended Properties =\"Excel 8.0;HDR=YES;\"";
    //    }
    //    else if (Path.GetExtension(path) == ".xlsx")
    //    {
    //        strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
    //    }
    //    try
    //    {
    //        OleDbConnection oledbConn = new OleDbConnection(strcon);
    //        oledbConn.Open();
    //        DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
    //        string strquery = "select * from [" + Sheets.Rows[0]["Table_Name"].ToString() + "] ";
    //        OleDbCommand com = new OleDbCommand(strquery, oledbConn);
    //        DataSet ds = new DataSet();
    //        OleDbDataAdapter oledbda = new OleDbDataAdapter(com);
    //        oledbda.Fill(ds, Sheets.Rows[0]["Table_Name"].ToString());
    //        oledbConn.Close();
    //        dt = ds.Tables[0];
    //    }
    //    catch
    //    {
    //        DisplayMessage("Excel file should in correct format");
    //    }
    //    return dt;
    //}
    #endregion
    #region Serial Number
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
                dt = CreateProductDatatable();
                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        if ((new DataView(dt, "SerialNo='" + txt[i].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) == 0)
                        {
                            string[] result = isSerialNumberValid_Openingstock(txt[i].ToString().Trim(), Session["PID"].ToString(), TransId, gvSerialNumber);
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
                            string[] result = isSerialNumberValid_Openingstock(txt[i].ToString().Trim(), Session["PID"].ToString(), TransId, gvSerialNumber);
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
            ((TextBox)gvProductStock.Rows[(int)Session["RowIndex"]].FindControl("txtQuantity")).Text = QtyCount.ToString();
        }
        else
        {
            ((TextBox)gvProductStock.Rows[(int)Session["RowIndex"]].FindControl("txtQuantity")).Text = "0";
        }

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



            if (dtserial.Rows.Count == 0)
            {
                //here we are checking that serial already exist or not with another product in current final table

                DataTable dtStockBatch = ObjStockBatchMaster.GetStockBatchMaster_By_SerialNo(serialNumber);


                if (dtStockBatch.Rows.Count == 0)
                {

                    if (HttpContext.Current.Session["dtFinal"] != null)
                    {
                        if (new DataView((DataTable)HttpContext.Current.Session["dtFinal"], "SerialNo='" + serialNumber + "' and ProductId<>" + ProductId + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                        {
                            Result[0] = "VALID";
                        }
                        else
                        {
                            Result[0] = "SERIAL_MISSMATCH";
                        }
                    }
                    else
                    {
                        Result[0] = "VALID";
                    }
                }
                else
                {
                    Result[0] = "SERIAL_MISSMATCH";

                }

                //if we not found in database with thsi product id that we are allow this serial number

            }
            else
            {
                Result[0] = "DUPLICATE";

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
        Session["RowIndex"] = Row.RowIndex;
        lblProductIdvalue.Text = ((Label)Row.FindControl("lblproductcode")).Text;
        lblProductNameValue.Text = ((Label)Row.FindControl("lblProductName")).Text;
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
            try
            {
                dt = new DataView(dt, "ProductId='" + Session["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            if (dt != null)
            {
                Session["dtSerial"] = dt;
            }
        }
        if (objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "SNO")
        {
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            pnlSerialNumber.Visible = true;
            pnlexp_and_Manf.Visible = false;
        }
        else if (objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LE" || objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FE")
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
        else if (objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LM" || objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FM")
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
        txtSerialNo.Focus();
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "text", "fnShowSerialPopup()", true);
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

        ((TextBox)gvProductStock.Rows[(int)Session["RowIndex"]].FindControl("txtQuantity")).Text = "0";

        txtSerialNo.Focus();
    }
    protected void btncloseserial_Click(object sender, EventArgs e)
    {
        pnlSerialNumber.Visible = false;
        pnlexp_and_Manf.Visible = false;
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
                ((TextBox)gvProductStock.Rows[(int)Session["RowIndex"]].FindControl("txtQuantity")).Text = recqty.ToString();
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
        lblSelectedRecord.Text = "";

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




    #region uploadEmployee
    protected void FileUploadComplete(object sender, EventArgs e)
    {
        int fileType = 0;
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
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }
                string path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                fileLoad.SaveAs(path);
                Import(path, fileType);
            }
        }
    }
    protected void btnGetSheet_Click(object sender, EventArgs e)
    {

        int fileType = 0;
        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }
                Import(Path, fileType);
            }
        }
    }

    public void Import(String path, int fileType)
    {
        string strcon = string.Empty;
        if (fileType == 1)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        else if (fileType == 0)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=YES;iMEX=1\"";
        }
        else
        {
            Session["filetype"] = "access";
            //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:/abc.mdb;Persist Security Info=False
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
        }
        Session["cnn"] = strcon;
        OleDbConnection conn = new OleDbConnection(strcon);
        conn.Open();
        DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        ddlTables.DataSource = tables;
        ddlTables.DataTextField = "TABLE_NAME";
        ddlTables.DataValueField = "TABLE_NAME";
        ddlTables.DataBind();
        conn.Close();
    }

    public DataTable ConvetExcelToDataTable(string path, string strtableName)
    {
        DataTable dt = new DataTable();

        try
        {
            OleDbConnection cnn = new OleDbConnection(Session["cnn"].ToString());
            OleDbDataAdapter adp = new OleDbDataAdapter("", "");
            adp.SelectCommand.CommandText = "Select *  From [" + strtableName.ToString() + "]";
            adp.SelectCommand.Connection = cnn;
            try
            {
                adp.Fill(dt);
            }
            catch (Exception)
            {
            }

        }
        catch
        {
            DisplayMessage("Excel file should in correct format");
        }
        return dt;
    }



    public bool CheckSheetValidation(DataTable dt)
    {
        bool Result = true;
        if (dt.Columns.Contains("ProductCode") && dt.Columns.Contains("UnitPrice") && dt.Columns.Contains("Quantity"))
        {

        }
        else
        {
            Result = false;
        }
        return Result;
    }


    public DataTable AddColumn(DataTable dt)
    {
        dt.Columns.Add("ProductCode_Id");
        return dt;
    }

    public string GetcolumnValue(string strtablename, string strKeyfieldname, string strKeyfieldvalue, string strKeyFieldResult)
    {
        string strResult = "0";
        strKeyfieldvalue = strKeyfieldvalue.Replace("'", "");
        DataTable dt = objDa.return_DataTable("select " + strKeyFieldResult + " from " + strtablename + " where " + strKeyfieldname + "='" + strKeyfieldvalue + "'");
        if (dt.Rows.Count > 0)
        {
            strResult = dt.Rows[0][0].ToString();
        }
        return strResult;
    }

    protected void btnConnect_Click(object sender, EventArgs e)
    {

        string strResult = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        string strDateVal = string.Empty;
        if (ddlTables == null)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        else if (ddlTables.Items.Count == 0)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        string strItemId = string.Empty;
        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                dt = ConvetExcelToDataTable(Path, ddlTables.SelectedValue.Trim());
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Record not found");
                    return;
                }
                if (!CheckSheetValidation(dt))
                {
                    DisplayMessage("Upload valid excel sheet");
                    return;
                }
                if (!dt.Columns.Contains("IsValid"))
                {
                    dt.Columns.Add("IsValid");
                }
                dt = AddColumn(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["IsValid"] = "True";

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.Trim() == "ProductCode")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(ProductCode - Enter Value)";
                                break;
                            }
                            strResult = GetcolumnValue("Inv_ProductMaster", "ProductCode", dt.Rows[i][j].ToString(), "ProductId");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            strItemId = strResult;

                            try
                            {
                                if (Convert.ToInt32(strItemId) == 0)
                                {
                                    dt.Rows[i]["IsValid"] = "False(Invalid ProductCode)";
                                    break;
                                }
                            }
                            catch
                            {

                            }
                        }

                        if (dt.Columns[j].ColumnName.Trim() == "Quantity")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(Quantity - Enter Value)";
                                break;
                            }

                            if (dt.Rows[i][j].ToString().Trim() == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(Quantity - Enter Value)";
                                break;
                            }
                        }

                    }
                }
                uploadEmpdetail.Visible = true;
                dtTemp = dt.DefaultView.ToTable(true, "ProductCode", "UnitPrice", "Quantity", "IsValid");
                gvSelected.DataSource = dtTemp;
                gvSelected.DataBind();
                //lbltotaluploadRecord.Text = "Total Record : " + (dtTemp.Rows.Count - 1).ToString();
                lbltotaluploadRecord.Text = "Total Record : " + (dtTemp.Rows.Count).ToString();
                Session["UploadEmpDtAll"] = dt;
                Session["UploadEmpDt"] = dtTemp;
                rbtnupdall.Checked = true;
                rbtnupdInValid.Checked = false;
                rbtnupdValid.Checked = false;
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
        dt.Dispose();
    }

    protected void rbtnupdall_OnCheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["UploadEmpDt"];
        if (rbtnupdValid.Checked)
        {
            dt = new DataView(dt, "IsValid='True' or IsValid=''", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (rbtnupdInValid.Checked)
        {
            dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        gvSelected.DataSource = dt;
        gvSelected.DataBind();
        //lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count - 1).ToString();
        lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count).ToString();
    }
    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "EmployeeInformation";
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
    protected void btnUploaditemInfo_Click(object sender, EventArgs e)
    {
        string strEmpType = string.Empty;
        if (gvSelected.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }
        int newhirecount = 0;
        string strItemId = string.Empty;
        int counter = 0;
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        DataTable dtProduct = new DataTable();
        SqlTransaction trns;
        int b = 0;
        trns = con.BeginTransaction();
        try
        {
            dt = (DataTable)Session["UploadEmpDtAll"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["IsValid"].ToString().Trim() != "True")
                {
                    continue;
                }

                if (dt.Rows[i]["ProductCode_Id"].ToString().Trim() != "0")
                {
                    string strUnitPrice = "0.00";
                    if (dt.Rows[i]["UnitPrice"].ToString() == "" || dt.Rows[i]["UnitPrice"].ToString() == null)
                    {
                        strUnitPrice = "0.00";
                    }
                    else
                    {
                        strUnitPrice = dt.Rows[i]["UnitPrice"].ToString();
                    }

                    strItemId = dt.Rows[i]["ProductCode"].ToString().Trim();

                    DataTable dtStockDetail = objDa.return_DataTable("Select * from Inv_StockDetail where ProductId='" + dt.Rows[i]["ProductCode_Id"].ToString() + "' and Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Finance_Year_Id='" + HttpContext.Current.Session["FinanceYearId"].ToString() + "' ");
                    if (dtStockDetail.Rows.Count > 0)
                    {
                        float fFinalQty = 0;
                        string strQuantity = dtStockDetail.Rows[0]["Quantity"].ToString();
                        if (strQuantity != "" && strQuantity != "0")
                        {
                            fFinalQty = float.Parse(strQuantity) + float.Parse(dt.Rows[i]["Quantity"].ToString());
                        }
                        else
                        {
                            fFinalQty = float.Parse(dt.Rows[i]["Quantity"].ToString());
                        }


                        //string strsql = "update Inv_StockDetail set Quantity='"+ fFinalQty + "' where ProductId='" + dt.Rows[i]["ProductCode_Id"].ToString() + "' and Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Finance_Year_Id='" + HttpContext.Current.Session["FinanceYearId"].ToString() + "' ";
                        //objDa.execute_Command(strsql, ref trns);

                        b = ObjProductLadger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "OP", "0", "0", dt.Rows[i]["ProductCode_Id"].ToString(), "0", "I", "0", "0", float.Parse(dt.Rows[i]["Quantity"].ToString()).ToString(), "0", "1/1/1800", strUnitPrice, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                    }
                    else
                    {
                        try
                        {
                            b = objstockDetail.InsertStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[i]["ProductCode_Id"].ToString(), dt.Rows[i]["Quantity"].ToString(), "0", "0", "0", "0", "0", "0", "0", "0", strUnitPrice, "0", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
                            b = ObjProductLadger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "OP", "0", "0", dt.Rows[i]["ProductCode_Id"].ToString(), "0", "I", "0", "0", float.Parse(dt.Rows[i]["Quantity"].ToString()).ToString(), "0", "1/1/1800", strUnitPrice, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        }
                        catch (Exception ex)
                        {
                            //lblBrandsearch.Text = "error " + dt.Rows[i]["ProductCode_Id"].ToString();
                        }
                    }










                    //DataTable dtProduct = objDa.return_DataTable("select * from inv_productmaster where productid=" + strItemId + "", ref trns);
                    //objstockDetail.DeleteStockDetailNew(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[i]["ProductCode_Id"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), "0", "0", DateTime.Now.ToString());
                    //ObjProductLadger.DeleteProductLedger_New(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "OP", "0", dt.Rows[i]["ProductCode_Id"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());



                    counter++;
                }
                else
                {

                }
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            if (newhirecount > 0)
            {
                DisplayMessage(newhirecount.ToString() + " new Product inserted and " + counter.ToString() + " Product information updated");
            }
            else
            {
                DisplayMessage(counter.ToString() + " Product information updated");
            }



            //btnResetitemInfo_Click(null, null);

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

    protected void btnResetitemInfo_Click(object sender, EventArgs e)
    {

    }

    protected void btndownloadInvalid_Click(object sender, EventArgs e)
    {
        //this event for download inavid record excel sheet 
        if (Session["UploadEmpDt"] == null)
        {
            DisplayMessage("Record Not found");
            return;
        }
        DataTable dt = (DataTable)(Session["UploadEmpDt"]);
        dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        ExportTableData(dt);
    }
    #endregion
}
