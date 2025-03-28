using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PegasusDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.Expressions;

public partial class Inventory_ProductPriceList : System.Web.UI.Page
{
    DataAccessClass ObjDa = null;
    Inv_ProductMaster objProductM = null;
    PageControlCommon objPageCmn = null;
    Inv_ProductBrandMaster ObjProductBrandMaster = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    Common cmn = null;
    Inv_SalesPriceList ObjSalePrice=null;
    SystemParameter ObjSysParam = null;
    //This Page is Created By Rahul Sharma on Date 02-01-2024
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        ObjProductBrandMaster = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjDa =new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        ObjSalePrice =new Inv_SalesPriceList(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/ProductPriceList.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            //btnbindrpt_Click(null, null);
            lblSalesPrice1.Text = GetCurrencyName("Sales Price 1");
            lblSalesPrice2.Text= GetCurrencyName("Sales Price 2");
            lblSalesPrice3.Text= GetCurrencyName("Sales Price 3");
            FillddlBrandSearch(Chkbrandsearch);
            FillProductCategorySerch(Chkcategorysearch);
            btnbindrpt_Click(null, null);
        }
        if (gvProduct.Rows.Count > 0)
        {
            divButtons.Visible = true;
        }
        else
        {
            divButtons.Visible = false;
        }
    }
    private void FillProductCategorySerch(CheckBoxList ddl)
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString().ToString());
        if (dsCategory.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddl, dsCategory, "Category_Name", "Category_Id");
        }
        else
        {
            ddl.Items.Insert(0, "--Select One--");
            ddl.SelectedIndex = 0;
        }
    }
    public void FillddlBrandSearch(CheckBoxList ddl)
    {
        DataTable dt = ObjProductBrandMaster.GetProductBrandTrueAllData(Session["CompId"].ToString().ToString());
        try
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddl, dt, "Brand_Name", "PBrandId"); 
        }
        catch
        {
            ddl.Items.Insert(0, "--Select One--");
            ddl.SelectedIndex = 0;
        }
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        
        if (clsPagePermission.bVerify == true)
        {
            gvProductList.Columns[4].Visible = true;
            Update_New.Visible = true;
            Update_Upload.Visible = true;
            Update_Li.Visible = true;
            Upload_li.Visible = true;
        }
        else
        {
            Upload_li.Visible = false;
            Update_Li.Visible = false;
            gvProductList.Columns[4].Visible = false;
            Update_New.Visible = false;
            Update_Upload.Visible = false;
        }
     
    }
    #endregion
    #region List Section
    //grid Seacrch binding function
    protected void btnbindrpt_Click(object sender,EventArgs e)
    {
        string strcategoryId = "0";
        string strBrandId = "0";       
        DataTable dtProduct = new DataTable();

        foreach (System.Web.UI.WebControls.ListItem item in Chkcategorysearch.Items)
        {
            if (item.Selected)
            {
                if (strcategoryId == "0")
                {
                    strcategoryId = item.Value;
                }
                else
                {
                    strcategoryId+= ","+item.Value;
                }
                // Do something with the checked value
            }
        }

        foreach (System.Web.UI.WebControls.ListItem item in Chkbrandsearch.Items)
        {
            if (item.Selected)
            {
                if (strBrandId == "0")
                {
                    strBrandId = item.Value;
                }
                else
                {
                    strBrandId += "," + item.Value;
                }
                // Do something with the checked value
            }
        }


        dtProduct = ObjSalePrice.GetAllSalesPrice(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strBrandId, strcategoryId, Session["FinanceYearId"].ToString());
        
        if (dtProduct.Rows.Count > 0)
        {
            string condition = string.Empty;
          
            if (txtValue.Text != "")
            {
               
                    string SearchType = string.Empty;
                    if (ddlOption.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                        SearchType = "Equal";
                    }
                    else if (ddlOption.SelectedIndex == 2)
                    {
                        condition = "" + ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
                        SearchType = "Contains";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
                        SearchType = "Like";
                    }
               
                dtProduct = new DataView(dtProduct,condition, "", DataViewRowState.CurrentRows).ToTable();
                if (dtProduct.Rows.Count > 0)
                {
                    if (chkStockZero.Checked == true)
                    {
                        string con = "Stock IS NOT NULL AND Stock <> '0'";
                        try
                        {
                            dtProduct = new DataView(dtProduct, con, "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                    }

                    lblTotalRecords.Text = "Total Records:"+ dtProduct.Rows.Count + "";
                    gvProductList.DataSource = dtProduct;
                    gvProductList.DataBind();
                    Session["GvPriceList"] = dtProduct;
                }
                else
                {
                    lblTotalRecords.Text = "Total Records: 0";
                    gvProductList.DataSource = null;
                    gvProductList.DataBind();
                    Session["GvPriceList"] = null; ;
                }

            }
            else if (ddlFieldName.SelectedIndex == 3)
            {
                        
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + ddlitemtypeserach.SelectedValue + "'";
                dtProduct = new DataView(dtProduct, condition, "", DataViewRowState.CurrentRows).ToTable();
                if (dtProduct.Rows.Count > 0)
                {
                    if (chkStockZero.Checked == true)
                    {
                        string con = "Stock IS NOT NULL AND Stock <> '0'";
                        try
                        {
                            dtProduct = new DataView(dtProduct, con, "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                    }
                    lblTotalRecords.Text = "Total Records:" + dtProduct.Rows.Count + "";
                    gvProductList.DataSource = dtProduct;
                    gvProductList.DataBind();
                    Session["GvPriceList"] = dtProduct;
                }
                else
                {
                    lblTotalRecords.Text = "Total Records: 0";
                    gvProductList.DataSource = null;
                    gvProductList.DataBind();
                    Session["GvPriceList"] = null; ;
                }
            }
            else
            {
                if (chkStockZero.Checked == true)
                {
                    string con = "Stock IS NOT NULL AND Stock <> '0'";
                    try
                    {
                        dtProduct = new DataView(dtProduct, con, "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }
                }
                lblTotalRecords.Text = "Total Records:" + dtProduct.Rows.Count + "";
                gvProductList.DataSource = dtProduct;
                gvProductList.DataBind();
                Session["GvPriceList"] = dtProduct;
            }
        }
        else
        {
            gvProductList.DataSource = null;
            gvProductList.DataBind();
        }

    }
    //static void ExportDataTableToPdf(DataTable dataTable)
    //{
    //    using (MemoryStream ms = new MemoryStream())
    //    {
    //        Document document = new Document(PageSize.A4.Rotate()); // Set landscape orientation
    //        PdfWriter.GetInstance(document, ms);
    //        document.Open();

    //        PdfPTable table = new PdfPTable(dataTable.Columns.Count);
    //        table.WidthPercentage = 100; // Set table width to 100% of page width
    //        table.LockedWidth = true; // Lock the table width

    //        // Add table headers
    //        foreach (DataColumn column in dataTable.Columns)
    //        {
    //            PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName));
    //            headerCell.BackgroundColor = BaseColor.LIGHT_GRAY; // Example: Set background color for header
    //                                                               // Set additional properties for header cell (if needed)
    //            table.AddCell(headerCell);
    //        }

    //        // Add table rows
    //        foreach (DataRow row in dataTable.Rows)
    //        {
    //            foreach (object item in row.ItemArray)
    //            {
    //                PdfPCell cell = new PdfPCell(new Phrase(item.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8))); // Set font size to 8
    //                                                                                                                                                   // Set additional properties for cells (if needed)
    //                table.AddCell(cell);
    //            }
    //        }

    //        // Set total width of the table according to the content
    //        table.SetTotalWidth(new float[] { 60f,80f,40f,40f,40f,50f,50f,60f,60f,70f,50f,70f,70f,70f }); // Example: Set initial widths for testing

    //        // Add the table to the document
    //        document.Add(table);
    //        document.Close();

    //        // Send the PDF content to the client's browser for download
    //        HttpContext.Current.Response.ContentType = "application/pdf";
    //        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=ProductPriceList.pdf");
    //        HttpContext.Current.Response.Buffer = true;
    //        HttpContext.Current.Response.Clear();
    //        HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
    //        HttpContext.Current.Response.OutputStream.Flush();
    //        HttpContext.Current.Response.End();
    //    }
    //}

    static void ExportDataTableToPdf(DataTable dataTable)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            Document document = new Document(PageSize.A4.Rotate()); // Set landscape orientation
            PdfWriter.GetInstance(document, ms);
            document.Open();

            // Add header
            PdfPTable headerTable = new PdfPTable(2); // 2 columns for date and name
            headerTable.WidthPercentage = 100;
            headerTable.DefaultCell.Border = PdfPCell.NO_BORDER; // Remove border around cells

            // Add date
            PdfPCell dateCell = new PdfPCell(new Phrase("Date:"+DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss")+"    "+"Created By :" + HttpContext.Current.Session["UserId"].ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12)));
            dateCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            dateCell.Border = PdfPCell.NO_BORDER; // Remove border around date cell
            headerTable.AddCell(dateCell);

            // Add name
            PdfPCell nameCell = new PdfPCell(new Phrase("Sales Price List", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD)));
            nameCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            nameCell.Border = PdfPCell.NO_BORDER; // Remove border around name cell
            headerTable.AddCell(nameCell);

            document.Add(headerTable);

            PdfPTable table = new PdfPTable(dataTable.Columns.Count);
            table.WidthPercentage = 100; // Set table width to 100% of page width
            table.LockedWidth = true; // Lock the table width

            // Add table headers
            foreach (DataColumn column in dataTable.Columns)
            {
                PdfPCell header = new PdfPCell(new Phrase(column.ColumnName));
                header.BackgroundColor = BaseColor.LIGHT_GRAY; // Example: Set background color for header
                                                                   // Set additional properties for header cell (if needed)
                table.AddCell(header);
            }

            // Add table rows
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (object item in row.ItemArray)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(item.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8))); // Set font size to 8
                                                                                                                                                       // Set additional properties for cells (if needed)
                    table.AddCell(cell);
                }
            }

            // Set total width of the table according to the content
            table.SetTotalWidth(new float[] { 60f, 80f, 40f, 40f, 40f, 50f, 50f, 60f, 60f, 70f, 50f, 70f, 70f, 70f }); // Example: Set initial widths for testing

            // Add the table to the document
            document.Add(table);
            document.Close();

            // Send the PDF content to the client's browser for download
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=SalesPriceList.pdf");
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            HttpContext.Current.Response.OutputStream.Flush();
            HttpContext.Current.Response.End();
        }
    }








    #region LOcationStock
    protected string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        CurrencyId = ObjDa.get_SingleValue("Select Currency_Id from Set_LocationMaster inner join Sys_CurrencyMaster on Sys_CurrencyMaster.Currency_ID=Set_LocationMaster.Field1 where Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'");


        return SystemParameter.GetCurrencySmbol(CurrencyId, SetDecimal(Amount.ToString()), Session["DBConnection"].ToString());
    }
    public string SetDecimal(string amount)
    {
        string strCurrencyId = "";
        strCurrencyId = ObjDa.get_SingleValue("Select Currency_Id from Set_LocationMaster inner join Sys_CurrencyMaster on Sys_CurrencyMaster.Currency_ID=Set_LocationMaster.Field1 where Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'");

        return ObjSysParam.GetCurencyConversionForInv(strCurrencyId, amount);
    }
    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        modelSA.getProductDetail(e.CommandArgument.ToString(), "", "");
    }
    #endregion
    public string  GetCurrencyName(string SalesName)
    {
        string CurrencyCode = "";
        try
        {
            CurrencyCode = ObjDa.get_SingleValue("Select Currency_Code from Set_LocationMaster inner join Sys_CurrencyMaster on Sys_CurrencyMaster.Currency_ID=Set_LocationMaster.Field1 where Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'");

            return (CurrencyCode + " " + SalesName);
        }
        catch (Exception ex)
        {
            return SalesName;
        }
    }
    //Refresh Grid On List
    protected void btnRefreshReport_Click(object sender,EventArgs e)
    {
        txtValue.Text = "";

        FillddlBrandSearch(Chkbrandsearch);
        FillProductCategorySerch(Chkcategorysearch);
        ddlFieldName.SelectedValue = "ProductId";
        gvProductList.DataSource = null;
        gvProductList.DataBind();
        btnbindrpt_Click(null, null);
        

    }
    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["GvPriceList"];
        if (dt != null)
        {
             
            ExportDataTableToPdf(dt);
        }
    }
    protected void btnExportPriceList_Click(object sender,EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["GvPriceList"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Remove("CostPrice");
                    dt.Columns.Add("Discontinue", typeof(string));
                    ExportTableData(dt);

                }
                else
                {
                    DisplayMessage("No Record Found");
                }

            }
            else
            {
                DisplayMessage("No Record Found");
            }

            dt.Dispose();
        }
        catch (Exception ex)
        {
            DisplayMessage("No Record Found");
        }

    }
    //Export function  for Excel Download
    protected void btnExportExcel_Click(object sender,EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["GvPriceList"];            
            if (dt!=null)
            {
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("Discontinue", typeof(string));
                    ExportTableData(dt);
                }
                else
                {
                    DisplayMessage("No Record Found");
                }
              
            }
            else
            {
                DisplayMessage("No Record Found");
            }
        }
        catch(Exception ex)
        {
            DisplayMessage("No Record Found");
        }

    }
    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {

    }
    //Upload Excel for Upload Section
    protected void btnUploadExcel_Click(object sender, EventArgs e)
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
    //In this function we Check Product Code is valid or not in Excel 
    public void Import(String path, int fileType)
    {
        try
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

            using (OleDbConnection oledbConn = new OleDbConnection(strcon))
            {
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [ProductPriceList$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataTable dt = new DataTable();
                oleda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("IsValid", typeof(string));
                    dt.Columns.Add("ProductId_Id", typeof(string));
                    dt.AcceptChanges();
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        string ProductCode = dt.Rows[i]["ProductId"].ToString();

                        DataTable dtProduct = ObjDa.return_DataTable("select ProductId from Inv_ProductMaster where ProductCode='"+ ProductCode + "'");
                        if (dtProduct.Rows.Count > 0)
                        {
                            dt.Rows[i]["ProductId_Id"] = dtProduct.Rows[0]["ProductId"].ToString();
                            dt.Rows[i]["IsValid"] = "True";
                        }
                        else
                        {
                            dt.Rows[i]["IsValid"] = "False";
                        }
                        if(dt.Rows[i]["SalesPrice1"].ToString()=="")
                        {
                            dt.Rows[i]["SalesPrice1"] = "0";
                        }
                        if (dt.Rows[i]["SalesPrice2"].ToString() == "")
                        {
                            dt.Rows[i]["SalesPrice2"] = "0";
                        }
                        if (dt.Rows[i]["SalesPrice3"].ToString() == "")
                        {
                            dt.Rows[i]["SalesPrice3"] = "0";
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        Session["GvImport"] = dt;
                        objPageCmn.FillData((object)gvImport, dt, "", "");
                        GetLocationForSameCurrency();
                    }
                    
                }
                else
                {
                    uploadOb.Visible = false;
                }
                
       
            }




        }
        catch (Exception ex)
        {
            //hdnInvalidExcelRecords.Value = "0";
            DisplayMessage("Error in excel uploading");
        }
        finally
        {
            if (gvImport.Rows.Count == 0)
            {
                uploadOb.Visible = false;
            }
            else
            {
                uploadOb.Visible = true;
            }

        }
    }
    //this is text changed function for filter valid or invalid record
    protected void rbtnupd_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["GvImport"];
        if (dt != null)
        {
            if (rbtnupdValid.Checked)
            {
                DataTable _dtTemp = new DataView(dt, "IsValid='True'", "", DataViewRowState.CurrentRows).ToTable();
                objPageCmn.FillData((object)gvImport, _dtTemp, "", "");
            }
            else if (rbtnupdInValid.Checked)
            {
                DataTable _dtTemp = new DataView(dt, "IsValid='False'", "", DataViewRowState.CurrentRows).ToTable();
                objPageCmn.FillData((object)gvImport, _dtTemp, "", "");
            }
            else
            {
                objPageCmn.FillData((object)gvImport, dt, "", "");
            }
        }
    }
   //Comman function for Export Datatble in excel 
    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "ProductPriceList";
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, strFname);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFname + ".xlsx");
            using(MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    //Excel upload Data Save function
    protected void btnUploadSave_Click(object sender,EventArgs e)
    {
        DataTable dt = (DataTable)Session["GvImport"];
        if (dt != null)
        {
            DataTable dtProduct = new DataView(dt, "IsValid='True'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtProduct.Rows.Count > 0)
            {
                for(int i = 0; i < dtProduct.Rows.Count; i++)
                {
                    if (dtProduct.Rows[i]["ProductId_Id"].ToString() != "")
                    {
                        DataTable dtStock = ObjDa.return_DataTable("Select* from Inv_StockDetail where Company_Id='" + Session["CompId"].ToString() + "' And Location_Id='" + Session["LocId"].ToString() + "' And ProductId='" + dtProduct.Rows[i]["ProductId_Id"].ToString() + "' And IsActive='1' And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "'");
                        if (dtStock.Rows.Count > 0)
                        {
                            ObjDa.execute_Command("Update Inv_StockDetail Set SalesPrice1='" + dtProduct.Rows[i]["SalesPrice1"].ToString() + "',SalesPrice2='" + dtProduct.Rows[i]["SalesPrice2"].ToString() + "',SalesPrice3='" + dtProduct.Rows[i]["SalesPrice3"].ToString() + "' where Company_Id='" + Session["CompId"].ToString() + "' And Location_Id='" + Session["LocId"].ToString() + "' And ProductId='" + dtProduct.Rows[i]["ProductId_Id"].ToString() + "' And IsActive='1' And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "'");

                        }
                        else
                        {
                            ObjDa.execute_Command("Insert Into Inv_StockDetail ([Company_Id],[Brand_Id],[Location_Id],[ProductId],[OpeningBalance],[RackID],[Quantity],[Minimum_Qty],[Maximum_Qty],[ReserveQty],[DamageQty],[BlockedQty],[OrderQty],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate], Finance_Year_Id, SalesPrice1, SalesPrice2, SalesPrice3) Values ('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + dtProduct.Rows[i]["ProductId_Id"].ToString() + "','0','0','0','0','0','0','0','0','0', '0','0','','','','1','" + DateTime.Now.ToString("yyyy-MM-dd") + "','1','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["FinanceYearId"].ToString() + "','" + dtProduct.Rows[i]["SalesPrice1"].ToString() + "', '" + dtProduct.Rows[i]["SalesPrice2"].ToString() + "', '" + dtProduct.Rows[i]["SalesPrice3"].ToString() + "')");
                        }
                        foreach (System.Web.UI.WebControls.ListItem li in ChkSubLocation.Items)
                        {
                            if (li.Selected)
                            {
                                DataTable dtListStock = ObjDa.return_DataTable("Select* from Inv_StockDetail where Company_Id='" + Session["CompId"].ToString() + "' And Location_Id='" + li.Value + "' And ProductId='" + dtProduct.Rows[i]["ProductId_Id"].ToString() + "' And IsActive='1' And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "'");
                                if (dtListStock.Rows.Count > 0)
                                {
                                    ObjDa.execute_Command("Update Inv_StockDetail Set SalesPrice1='" + dtProduct.Rows[i]["SalesPrice1"].ToString() + "',SalesPrice2='" + dtProduct.Rows[i]["SalesPrice2"].ToString() + "',SalesPrice3='" + dtProduct.Rows[i]["SalesPrice3"].ToString() + "' where Company_Id='" + Session["CompId"].ToString() + "' And Location_Id='" + li.Value.ToString() + "' And ProductId='" + dtProduct.Rows[i]["ProductId_Id"].ToString() + "' And IsActive='1' And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "'");

                                }
                                else
                                {
                                    ObjDa.execute_Command("Insert Into Inv_StockDetail ([Company_Id],[Brand_Id],[Location_Id],[ProductId],[OpeningBalance],[RackID],[Quantity],[Minimum_Qty],[Maximum_Qty],[ReserveQty],[DamageQty],[BlockedQty],[OrderQty],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate], Finance_Year_Id, SalesPrice1, SalesPrice2, SalesPrice3) Values ('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + li.Value.ToString() + "','" + dtProduct.Rows[i]["ProductId_Id"].ToString() + "','0','0','0','0','0','0','0','0','0', '0','0','','','','1','" + DateTime.Now.ToString("yyyy-MM-dd") + "','1','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["FinanceYearId"].ToString() + "','" + dtProduct.Rows[i]["SalesPrice1"].ToString() + "', '" + dtProduct.Rows[i]["SalesPrice2"].ToString() + "', '" + dtProduct.Rows[i]["SalesPrice3"].ToString() + "')");
                                }
                            }
                        }

                        if (dtProduct.Rows[i]["Discontinue"].ToString()!="" && dtProduct.Rows[i]["Discontinue"].ToString() != null)
                        {
                            ObjDa.execute_Command("Update  Inv_ProductMaster Set Field1='" + dtProduct.Rows[i]["Discontinue"].ToString() + "' where ProductId='" + dtProduct.Rows[i]["ProductId_Id"].ToString() + "' And Company_Id='"+ Session["CompId"].ToString() + "' And Brand_Id='" + Session["BrandId"].ToString() + "' ");
                        }

                    }
                }
                btnUploadReset_Click(null,null);
                DisplayMessage("Data Save Successfully");
            }
            else
            {
                DisplayMessage("there is no Valid record found");
            }
        }
    }
    //reset upload Section
    protected void btnUploadReset_Click(object sender,EventArgs e)
    {
        gvImport.DataSource = null;
        gvImport.DataBind();
        uploadOb.Visible = false;
        Session["GvImport"] = null;
    }
    #endregion



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductCode_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["ProductCode"].ToString();
            }
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductName_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["EProductName"].ToString();
            }
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
    }

    protected void ddlFieldName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedIndex == 3)
        {
            ddlitemtypeserach.Visible = true;
            txtValue.Visible = false;           
            ddlOption.SelectedIndex = 1;
            ddlOption.Enabled = false;
        }
        else
        {
            ddlitemtypeserach.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            ddlOption.SelectedIndex = 2;
            ddlOption.Enabled = true;
        }
    }

    //Add data in Table 
    protected void btnAdd_Click(object sender,EventArgs e)
    {
        DataTable dt = new DataTable();
         dt= FillProductDataTable();
            if (dt.Rows.Count > 0)
            {
                 Session["dtPriceList"] = dt;
                 objPageCmn.FillData((object)gvProduct, dt, "", "");
                 divButtons.Visible = true;
                 Reset();
            }
        

    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;
        if (txtProductcode.Text != "")
        {
            try
            {
                DataTable dtProduct = ObjDa.return_DataTable("Select EProductName,(Select SalesPrice1 from Inv_StockDetail where Inv_StockDetail.ProductId=Inv_ProductMaster.ProductId And Inv_StockDetail.Company_Id='" + Session["CompId"].ToString() + "' And Inv_StockDetail.Brand_Id = '" + Session["BrandId"].ToString() + "' And Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' And Inv_StockDetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') as SalesPrice1,(Select SalesPrice2 from Inv_StockDetail where Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId And Inv_StockDetail.Company_Id = '" + Session["CompId"].ToString() + "' And Inv_StockDetail.Brand_Id = '" + Session["BrandId"].ToString() + "' And Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' And Inv_StockDetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "')as SalesPrice2,(Select SalesPrice3 from Inv_StockDetail where Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId And Inv_StockDetail.Company_Id = '" + Session["CompId"].ToString() + "' And Inv_StockDetail.Brand_Id = '" + Session["BrandId"].ToString() + "' And Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' And Inv_StockDetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "')as SalesPrice3 from Inv_ProductMaster where ProductCode = '" + txtProductcode.Text + "' And IsActive = '1'");
                if (dtProduct.Rows.Count > 0)
                {
                    txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                    txtSalesPrice1.Text= dtProduct.Rows[0]["SalesPrice1"].ToString();
                    txtSalesPrice2.Text= dtProduct.Rows[0]["SalesPrice2"].ToString();
                    txtSalesPrice3.Text= dtProduct.Rows[0]["SalesPrice3"].ToString();
                }                
                else
                {
                    txtProductcode.Text = "";
                    DisplayMessage("Please Choose In suggestion");
                }


            }
            catch(Exception ex)
            {

            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }
        //AllPageCode();
    }
    protected void txtProductName_TextChanged(object sender,EventArgs e)
    {
        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;
        if (txtProductName.Text != "")
        {
            try
            {
                // DataTable dtProduct = ObjDa.return_DataTable("select ProductCode, Inv_StockDetail.SalesPrice1,Inv_StockDetail.SalesPrice2,Inv_StockDetail.SalesPrice3  from  Inv_StockDetail inner join Inv_ProductMaster on Inv_ProductMaster.ProductId = Inv_StockDetail.ProductId where EProductName = '" + txtProductName.Text + "' And  Inv_StockDetail.IsActive = '1' And Inv_StockDetail.Company_Id='" + Session["CompId"].ToString() + "' And Inv_StockDetail.Brand_Id='" + Session["BrandId"].ToString() + "' And Inv_StockDetail.Location_Id='" + Session["LocId"].ToString() + "' And Inv_StockDetail.Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "'");
                DataTable dtProduct = ObjDa.return_DataTable("Select ProductCode,(Select SalesPrice1 from Inv_StockDetail where Inv_StockDetail.ProductId=Inv_ProductMaster.ProductId And Inv_StockDetail.Company_Id='" + Session["CompId"].ToString() + "' And Inv_StockDetail.Brand_Id = '" + Session["BrandId"].ToString() + "' And Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' And Inv_StockDetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "') as SalesPrice1,(Select SalesPrice2 from Inv_StockDetail where Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId And Inv_StockDetail.Company_Id = '" + Session["CompId"].ToString() + "' And Inv_StockDetail.Brand_Id = '" + Session["BrandId"].ToString() + "' And Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' And Inv_StockDetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "')as SalesPrice2,(Select SalesPrice3 from Inv_StockDetail where Inv_StockDetail.ProductId = Inv_ProductMaster.ProductId And Inv_StockDetail.Company_Id = '" + Session["CompId"].ToString() + "' And Inv_StockDetail.Brand_Id = '" + Session["BrandId"].ToString() + "' And Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "' And Inv_StockDetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "')as SalesPrice3 from Inv_ProductMaster where EProductName  = '" + txtProductName.Text + "' And IsActive = '1'");

                if (dtProduct.Rows.Count > 0)
                {
                    txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
                    txtSalesPrice1.Text = dtProduct.Rows[0]["SalesPrice1"].ToString();
                    txtSalesPrice2.Text = dtProduct.Rows[0]["SalesPrice2"].ToString();
                    txtSalesPrice3.Text = dtProduct.Rows[0]["SalesPrice3"].ToString();
                }
                else
                {
                    txtProductName.Text = "";
                    DisplayMessage("Please Choose In suggestion");
                }

            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
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
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    //Delete data in grid after add
    protected void imgBtnDetailDelete_Command(object sender,CommandEventArgs e)
    {
        DataTable dt = FillProductDataTabelDelete(e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Serial_No"] = Convert.ToString(i + 1);
            }


            Session["dtPriceList"] = dt;
            objPageCmn.FillData((object)gvProduct, dt, "", "");
            divButtons.Visible = true;
            Reset();
        }
    }
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Serial_No");
        dt.Columns.Add("ProductId");       
        dt.Columns.Add("SalesPrice1");
        dt.Columns.Add("SalesPrice2");
        dt.Columns.Add("SalesPrice3");        
        return dt;
    }
    public DataTable FillProductDataTable()
    {
        DataTable dt = CreateProductDataTable();

        if (gvProduct.Rows.Count > 0)
        {
            for (int i = 0; i < gvProduct.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != gvProduct.Rows.Count)
                {
                    DataRow row = dt.NewRow();

                    Label lblgvSNo = (Label)gvProduct.Rows[i].FindControl("lblSNo");
                    HiddenField hdngvProductId = (HiddenField)gvProduct.Rows[i].FindControl("hdngvProductId");
                    Label gvSalesPrice1 = (Label)gvProduct.Rows[i].FindControl("gvSalesPrice1");
                    Label gvSalesPrice2 = (Label)gvProduct.Rows[i].FindControl("gvSalesPrice2");
                    Label gvSalesPrice3 = (Label)gvProduct.Rows[i].FindControl("gvSalesPrice3");

                    row["Serial_No"] = lblgvSNo.Text;
                    row["ProductId"] = hdngvProductId.Value;
                    row["SalesPrice1"] = gvSalesPrice1.Text;
                    row["SalesPrice2"] = gvSalesPrice2.Text;
                    row["SalesPrice3"] = gvSalesPrice3.Text;

                    dt.Rows.Add(row);
                }
                else
                {
                    DataRow row = dt.NewRow();                    
                    row["Serial_No"] = (i+1).ToString();
                    row["ProductId"] = GetProductId(txtProductcode.Text);
                    row["SalesPrice1"] = txtSalesPrice1.Text;
                    row["SalesPrice2"] = txtSalesPrice2.Text;
                    row["SalesPrice3"] = txtSalesPrice3.Text;
                    dt.Rows.Add(row);
                }
            }
        }
        else
        {
            // Add a default row if GridView has no rows
            DataRow row = dt.NewRow();
            row["Serial_No"] = "1";
            row["ProductId"] = GetProductId(txtProductcode.Text);
            row["SalesPrice1"] = txtSalesPrice1.Text;
            row["SalesPrice2"] = txtSalesPrice2.Text;
            row["SalesPrice3"] = txtSalesPrice3.Text;

            dt.Rows.Add(row);
        }

        return dt;
    }

    public DataTable FillProductDataTabelDelete( string Serial_No)
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < gvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);

            Label lblgvSNo = (Label)gvProduct.Rows[i].FindControl("lblSNo");
            HiddenField hdngvProductId = (HiddenField)gvProduct.Rows[i].FindControl("hdngvProductId");
            Label gvSalesPrice1 = (Label)gvProduct.Rows[i].FindControl("gvSalesPrice1");
            Label gvSalesPrice2 = (Label)gvProduct.Rows[i].FindControl("gvSalesPrice2");
            Label gvSalesPrice3 = (Label)gvProduct.Rows[i].FindControl("gvSalesPrice3");

            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["ProductId"] = hdngvProductId.Value;
            dt.Rows[i]["SalesPrice1"] = gvSalesPrice1.Text;
            dt.Rows[i]["SalesPrice2"] = gvSalesPrice2.Text;
            dt.Rows[i]["SalesPrice3"] = gvSalesPrice3.Text;
         
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Serial_No<>'" + Serial_No + "'";
        dt = (DataTable)dv.ToTable();
        return dt;
    }
    public string GetProductId(string ProductCode)
    {
        string ProductId = ObjDa.get_SingleValue("Select ProductId from Inv_ProductMaster where ProductCode='"+ ProductCode + "'");
        return ProductId;
    }
    protected void btnReset_Click(object sender,EventArgs e)
    {
        Reset();
    }
    public void Reset()
    {
        txtProductcode.Text = "";
        txtProductName.Text = "";
        txtSalesPrice1.Text = "";
        txtSalesPrice2.Text = "";
        txtSalesPrice3.Text = "";
    }

    public void FillProduct()
    {
        DataTable dt = new DataTable();
        dt = ObjDa.return_DataTable("");
        if(dt.Rows.Count>0)
        {

        }
    }
  
    protected void gvProductList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductList.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["GvPriceList"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductList, dt, "", "");

        //AllPageCode();
        gvProductList.BottomPagerRow.Focus();
    }
    protected void gvProductList_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["GvPriceList"];
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
        Session["GvPriceList"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductList, dt, "", "");


        //AllPageCode();
        gvProductList.HeaderRow.Focus();
    }

    //Save Code for new and old entry
    protected void btnSave_Click(object sender,EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["dtPriceList"];
            if (dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable dtProduct = ObjDa.return_DataTable("Select* from Inv_StockDetail where Company_Id='"+ Session["CompId"].ToString() + "' And Location_Id='"+ Session["LocId"].ToString() + "' And ProductId='"+ dt.Rows[i]["ProductId"].ToString()+ "' And IsActive='1' And Finance_Year_Id='"+ Session["FinanceYearId"].ToString() + "'");
                    if (dtProduct.Rows.Count > 0)
                    {
                        ObjDa.execute_Command("Update Inv_StockDetail Set SalesPrice1='" + dt.Rows[i]["SalesPrice1"].ToString() + "',SalesPrice2='" + dt.Rows[i]["SalesPrice2"].ToString() + "',SalesPrice3='" + dt.Rows[i]["SalesPrice3"].ToString() + "' where Company_Id='" + Session["CompId"].ToString() + "' And Location_Id='" + Session["LocId"].ToString() + "' And ProductId='" + dt.Rows[i]["ProductId"].ToString() + "' And IsActive='1' And Finance_Year_Id='"+ Session["FinanceYearId"].ToString() + "'");
                    }
                    else
                    {
                        ObjDa.execute_Command("Insert Into Inv_StockDetail ([Company_Id],[Brand_Id],[Location_Id],[ProductId],[OpeningBalance],[RackID],[Quantity],[Minimum_Qty],[Maximum_Qty],[ReserveQty],[DamageQty],[BlockedQty],[OrderQty],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate], Finance_Year_Id, SalesPrice1, SalesPrice2, SalesPrice3) Values ('"+ Session["CompId"].ToString() + "','"+ Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','"+ dt.Rows[i]["ProductId"].ToString() + "','0','0','0','0','0','0','0','0','0', '0','0','','','','1','"+DateTime.Now.ToString("yyyy-MM-dd")+"','1','"+ Session["UserId"].ToString() + "','"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+ Session["UserId"].ToString() + "','"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+ Session["FinanceYearId"].ToString() + "','"+ dt.Rows[i]["SalesPrice1"].ToString() + "', '"+ dt.Rows[i]["SalesPrice2"].ToString() + "', '"+ dt.Rows[i]["SalesPrice3"].ToString() + "')");
                    }
                }
                btnCancel_Click(null, null);
                DisplayMessage("Save Successfully");
            }
            else
            {
                DisplayMessage("Please Add Product");
            }
        }
        catch(Exception ex)
        {
            DisplayMessage("Please Add Product");
        }

    }
    //reset function
    protected void btnCancel_Click(object sender,EventArgs e)
    {
        Reset();
        gvProduct.DataSource = null;
        gvProduct.DataBind();
        divButtons.Visible = false;
    }
    public void GetLocationForSameCurrency()
    {
        DataTable dt = new DataTable();
        dt = ObjDa.return_DataTable("Select Location_Id,Location_Name from  Set_LocationMaster where Field1 In (Select Field1 from Set_LocationMaster Where Location_Id='"+ Session["LocId"].ToString() + "') And Location_Id!='"+ Session["LocId"].ToString() + "'");
        if (dt.Rows.Count > 0)
        {
            try
            {
                objPageCmn.FillData((object)ChkSubLocation, dt, "Location_Name", "Location_Id");
            }
            catch
            {

            }
        }
    }

}