using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using PegasusDataAccess;

/// <summary>
/// Summary description for ContactReport
/// </summary>
public class PriceList : DevExpress.XtraReports.UI.XtraReport
{
    Inv_Model_Category_Product objInvModelCategoryProduct = null;
    Inv_ParameterMaster objParam = null;
    SystemParameter objsys = null;
    Set_AddressChild Objaddress = null;
    LocationMaster ObjLocationMaster = null;
    CurrencyMaster objCurrency = null;
    DataAccessClass objda = null;
    private string _strConString = string.Empty;
    Inv_StockDetail objStockDetail = null;
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private InventoryDataSet inventoryDataSet1;
    private InventoryDataSetTableAdapters.sp_Ems_ContactMaster_SelectRowTableAdapter sp_Ems_ContactMaster_SelectRowTableAdapter1;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private PageFooterBand PageFooter;
    private XRPageInfo xrPageInfo1;
    private XRPageInfo xrPageInfo2;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell14;
    private GroupHeaderBand GroupHeader1;
    private XRLabel xrLabel5;
    private XRRichText xrRichText2;
    private XRTable xrTable5;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell20;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell21;
    private XRRichText xrRichText3;
    private ReportHeaderBand ReportHeader;
    private XRPictureBox xrPictureBox2;
    private XRLabel xrLabel2;
    private XRLabel xrLabel1;
    private XRPictureBox xrPictureBox1;
    private XRRichText xrRichText1;
    private XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell10;
    private XRTable xrTable4;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell15;
    private PageHeaderBand PageHeader;
    private XRLabel xrLabel6;
    private XRLabel xrLabel3;
    private XRTable xrTable6;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell11;
    private GroupHeaderBand GroupHeader2;
    private GroupFooterBand GroupFooter2;
    private XRPanel xrPanel1;
    private XRLabel xrLabel15;
    private XRLabel xrLabel4;
    private XRTableCell xrTableCell1;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public PriceList(string strConString)
    {
        InitializeComponent();
        objInvModelCategoryProduct = new Inv_Model_Category_Product(strConString);
        objParam = new Inv_ParameterMaster(strConString);
        objda = new DataAccessClass(strConString);
        objStockDetail = new Inv_StockDetail(strConString);
        objsys = new SystemParameter(strConString);
        Objaddress = new Set_AddressChild(strConString);
        ObjLocationMaster = new LocationMaster(strConString);
        objCurrency = new CurrencyMaster(strConString);
        _strConString = strConString;
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }


    string GroupBy = string.Empty;
    string ModelCurrencyId = string.Empty;
    string ModelId = string.Empty;
    double TotalPrice = 0;
    string Curr_ExchangeRate = "1";
    bool isReportLoad = false;

    public void setcompanyAddress(string Address)
    {
        //xrLabel3.Text = Address;
    }
    public void setcompanyname(string companyname, string CompanyId)
    {
        //xrLabel2.Text = companyname;


    }
    public void setCurrencyandMonthName(string Value)
    {
        xrLabel2.Text = Value;
    }
    public void setGroupByValue(string Value)
    {
        GroupBy = Value;
    }
    public void SetImage(string Url)
    {
        xrPictureBox1.ImageUrl = Url;
    }
    public void setCompanyArebicName(string ArebicName)
    {
        //xrLabel1.Text = ArebicName;

    }
    public void setTitelName(string Title)
    {
        xrLabel1.Text = Title;
    }
    public void setSubTitleName(string subtitle)
    {
        xrLabel4.Text = subtitle;
    }
    public void setCompanyTelNo(string TelNo)
    {
        //xrLabel28.Text = TelNo;
    }
    public void setCompanyFaxNo(string FaxNo)
    {
        //xrLabel29.Text = FaxNo;
    }
    public void setCompanyWebsite(string WebSite)
    {
        //xrLabel32.Text = WebSite;

    }
    public void setUserName(string UserName)
    {
        // xrLabel15.Text = UserName;
    }
    public void setSuggestedPrice(string Value)
    {
        xrTableCell10.Text = Value;
    }
    public void setCurrency(string CurrencyId, string ModelTransId)
    {
        ModelCurrencyId = CurrencyId;
        ModelId = ModelTransId;

    }
    public void setExchangeRate(string ExchangeRate)
    {
        Curr_ExchangeRate = ExchangeRate;
    }
    public void setPriceListLogo(string url)
    {
        xrPictureBox2.ImageUrl = url;

    }
    public void setPageHeader(string HeaderValue)
    {
        xrLabel6.Text = HeaderValue;

    }


    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string resourceFileName = "PriceList.resx";
        System.Resources.ResourceManager resources = global::Resources.PriceList.ResourceManager;
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrRichText2 = new DevExpress.XtraReports.UI.XRRichText();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.inventoryDataSet1 = new InventoryDataSet();
        this.sp_Ems_ContactMaster_SelectRowTableAdapter1 = new InventoryDataSetTableAdapters.sp_Ems_ContactMaster_SelectRowTableAdapter();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrRichText3 = new DevExpress.XtraReports.UI.XRRichText();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.inventoryDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
        this.Detail.HeightF = 29.37495F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("TransID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrTable2
        // 
        this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 0F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
        this.xrTable2.SizeF = new System.Drawing.SizeF(835.9998F, 29.37495F);
        this.xrTable2.StylePriority.UseBorders = false;
        this.xrTable2.StylePriority.UseFont = false;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6,
            this.xrTableCell19,
            this.xrTableCell14});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.Borders = DevExpress.XtraPrinting.BorderSide.Left;
        this.xrTableCell6.BorderWidth = 2F;
        this.xrTableCell6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText2});
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 5, 0, 100F);
        this.xrTableCell6.StylePriority.UseBorders = false;
        this.xrTableCell6.StylePriority.UseBorderWidth = false;
        this.xrTableCell6.StylePriority.UsePadding = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell6.Weight = 2.1533176628855846D;
        // 
        // xrRichText2
        // 
        this.xrRichText2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrRichText2.BorderWidth = 2F;
        this.xrRichText2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Rtf", null, "sp_Inv_BOM_SelectRow.DefaultPartNo"),
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "sp_Inv_BOM_SelectRow.DefaultPartNo")});
        this.xrRichText2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrRichText2.LocationFloat = new DevExpress.Utils.PointFloat(5.499998F, 0F);
        this.xrRichText2.Name = "xrRichText2";
        this.xrRichText2.SerializableRtfString = resources.GetString("xrRichText2.SerializableRtfString");
        this.xrRichText2.SizeF = new System.Drawing.SizeF(199.7499F, 29.37495F);
        this.xrRichText2.StylePriority.UseBorders = false;
        this.xrRichText2.StylePriority.UseBorderWidth = false;
        this.xrRichText2.StylePriority.UseFont = false;
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell19.BorderWidth = 1F;
        this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_BOM_SelectRow.ShortDescription")});
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 5, 0, 100F);
        this.xrTableCell19.StylePriority.UseBorders = false;
        this.xrTableCell19.StylePriority.UseBorderWidth = false;
        this.xrTableCell19.StylePriority.UsePadding = false;
        this.xrTableCell19.Weight = 4.5314540992930423D;
        this.xrTableCell19.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell19_BeforePrint);
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.Borders = DevExpress.XtraPrinting.BorderSide.Right;
        this.xrTableCell14.BorderWidth = 2F;
        this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_BOM_SelectRow.LocalUnitPrice")});
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0, 100F);
        this.xrTableCell14.StylePriority.UseBorders = false;
        this.xrTableCell14.StylePriority.UseBorderWidth = false;
        this.xrTableCell14.StylePriority.UsePadding = false;
        this.xrTableCell14.StylePriority.UseTextAlignment = false;
        this.xrTableCell14.Text = "xrTableCell14";
        this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell14.Weight = 1.678405751435796D;
        this.xrTableCell14.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell14_BeforePrint);
        // 
        // TopMargin
        // 
        this.TopMargin.HeightF = 1.041603F;
        this.TopMargin.Name = "TopMargin";
        this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // BottomMargin
        // 
        this.BottomMargin.HeightF = 1.749929F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // inventoryDataSet1
        // 
        this.inventoryDataSet1.DataSetName = "InventoryDataSet";
        this.inventoryDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // sp_Ems_ContactMaster_SelectRowTableAdapter1
        // 
        this.sp_Ems_ContactMaster_SelectRowTableAdapter1.ClearBeforeFill = true;
        // 
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLabel15,
            this.xrPageInfo2});
        this.PageFooter.HeightF = 51.37507F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo1
        // 
        this.xrPageInfo1.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
        this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 12.04179F);
        this.xrPageInfo1.Name = "xrPageInfo1";
        this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo1.SizeF = new System.Drawing.SizeF(215.25F, 18.04161F);
        this.xrPageInfo1.StylePriority.UseTextAlignment = false;
        this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel15
        // 
        this.xrLabel15.ForeColor = System.Drawing.Color.Gray;
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(219.75F, 12.04179F);
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(415.3409F, 17.62505F);
        this.xrLabel15.StylePriority.UseForeColor = false;
        this.xrLabel15.StylePriority.UseTextAlignment = false;
        this.xrLabel15.Text = "Specifications subject to change without notice";
        this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Format = "Page{0}Of {1}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(748.9997F, 12.04179F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(89.99997F, 18.04161F);
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrRichText3
        // 
        this.xrRichText3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "sp_Inv_BOM_SelectRow.Footer")});
        this.xrRichText3.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 63.41666F);
        this.xrRichText3.Name = "xrRichText3";
        this.xrRichText3.SizeF = new System.Drawing.SizeF(835.9998F, 23.00002F);
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5});
        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Field1", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ModelNo", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader1.HeightF = 22.99999F;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // xrLabel5
        // 
        this.xrLabel5.BackColor = System.Drawing.Color.Silver;
        this.xrLabel5.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel5.BorderWidth = 2F;
        this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_BOM_SelectRow.OptionCategoryName")});
        this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 0F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(835.9998F, 22.99999F);
        this.xrLabel5.StylePriority.UseBackColor = false;
        this.xrLabel5.StylePriority.UseBorders = false;
        this.xrLabel5.StylePriority.UseBorderWidth = false;
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UsePadding = false;
        this.xrLabel5.Text = "xrLabel5";
        // 
        // xrTable5
        // 
        this.xrTable5.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrTable5.BorderWidth = 2F;
        this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 3F);
        this.xrTable5.Name = "xrTable5";
        this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow6});
        this.xrTable5.SizeF = new System.Drawing.SizeF(835.9998F, 50F);
        this.xrTable5.StylePriority.UseBorders = false;
        this.xrTable5.StylePriority.UseBorderWidth = false;
        this.xrTable5.Visible = false;
        // 
        // xrTableRow7
        // 
        this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell21});
        this.xrTableRow7.Name = "xrTableRow7";
        this.xrTableRow7.Weight = 1D;
        // 
        // xrTableCell21
        // 
        this.xrTableCell21.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrTableCell21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell21.Name = "xrTableCell21";
        this.xrTableCell21.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
        this.xrTableCell21.StylePriority.UseBorders = false;
        this.xrTableCell21.StylePriority.UseFont = false;
        this.xrTableCell21.StylePriority.UsePadding = false;
        this.xrTableCell21.Text = "RELATED PRODUCT";
        this.xrTableCell21.Weight = 8.3599978731325884D;
        // 
        // xrTableRow6
        // 
        this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell1,
            this.xrTableCell18,
            this.xrTableCell20});
        this.xrTableRow6.Name = "xrTableRow6";
        this.xrTableRow6.Weight = 1D;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseBorders = false;
        this.xrTableCell16.StylePriority.UseFont = false;
        this.xrTableCell16.Text = "Product Id";
        this.xrTableCell16.Weight = 1.6212494598457365D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell17.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseBorders = false;
        this.xrTableCell17.StylePriority.UseFont = false;
        this.xrTableCell17.StylePriority.UseTextAlignment = false;
        this.xrTableCell17.Text = "Product Name";
        this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell17.Weight = 4.25838875912625D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseBorders = false;
        this.xrTableCell18.StylePriority.UseFont = false;
        this.xrTableCell18.StylePriority.UseTextAlignment = false;
        this.xrTableCell18.Text = "Stock";
        this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell18.Weight = 0.722880201429469D;
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell20.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.StylePriority.UseBorders = false;
        this.xrTableCell20.StylePriority.UseFont = false;
        this.xrTableCell20.StylePriority.UseTextAlignment = false;
        this.xrTableCell20.Text = "Price";
        this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell20.Weight = 1.1111379138175603D;
        this.xrTableCell20.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell20_BeforePrint);
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrPictureBox2,
            this.xrLabel2,
            this.xrLabel1,
            this.xrPictureBox1});
        this.ReportHeader.HeightF = 1094.834F;
        this.ReportHeader.Name = "ReportHeader";
        this.ReportHeader.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ReportHeader_BeforePrint);
        // 
        // xrLabel4
        // 
        this.xrLabel4.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel4.ForeColor = System.Drawing.Color.Gray;
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(20.55269F, 192.5F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(807.6448F, 37.99982F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseForeColor = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "PARTNER PRICE LIST";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrPictureBox2
        // 
        this.xrPictureBox2.BackColor = System.Drawing.Color.Empty;
        this.xrPictureBox2.ImageUrl = "~\\Images\\erp_3.jpg";
        this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(20.55269F, 279.7083F);
        this.xrPictureBox2.Name = "xrPictureBox2";
        this.xrPictureBox2.SizeF = new System.Drawing.SizeF(813.6142F, 771.5421F);
        this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox2.StylePriority.UseBackColor = false;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Bold);
        this.xrLabel2.ForeColor = System.Drawing.Color.Silver;
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(20.55269F, 232.5416F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(807.6448F, 36.54166F);
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseForeColor = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "MARCH 2015  |   USD";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel1
        // 
        this.xrLabel1.Font = new System.Drawing.Font("Verdana", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel1.ForeColor = System.Drawing.Color.Gray;
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(20.55268F, 54.25F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(807.6448F, 138.25F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseForeColor = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "PARTNER PRICE LIST";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(20.55268F, 0F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(231.606F, 54.25F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        // 
        // xrRichText1
        // 
        this.xrRichText1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrRichText1.BorderWidth = 2F;
        this.xrRichText1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "sp_Inv_BOM_SelectRow.Header")});
        this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 23.42434F);
        this.xrRichText1.Name = "xrRichText1";
        this.xrRichText1.SizeF = new System.Drawing.SizeF(835.9999F, 22.99999F);
        this.xrRichText1.StylePriority.UseBorders = false;
        this.xrRichText1.StylePriority.UseBorderWidth = false;
        // 
        // xrTable3
        // 
        this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 46.87506F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
        this.xrTable3.SizeF = new System.Drawing.SizeF(835.9998F, 43.12494F);
        this.xrTable3.StylePriority.UseBorders = false;
        this.xrTable3.StylePriority.UseFont = false;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell8,
            this.xrTableCell10});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.BackColor = System.Drawing.Color.White;
        this.xrTableCell5.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell5.BorderWidth = 2F;
        this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell5.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrTableCell5.StylePriority.UseBackColor = false;
        this.xrTableCell5.StylePriority.UseBorderColor = false;
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.StylePriority.UseBorderWidth = false;
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.StylePriority.UseForeColor = false;
        this.xrTableCell5.StylePriority.UsePadding = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "Part Number";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell5.Weight = 2.1533178436892397D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.BackColor = System.Drawing.Color.White;
        this.xrTableCell8.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell8.BorderWidth = 2F;
        this.xrTableCell8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell8.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
        this.xrTableCell8.StylePriority.UseBackColor = false;
        this.xrTableCell8.StylePriority.UseBorderColor = false;
        this.xrTableCell8.StylePriority.UseBorders = false;
        this.xrTableCell8.StylePriority.UseBorderWidth = false;
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UseForeColor = false;
        this.xrTableCell8.StylePriority.UsePadding = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        this.xrTableCell8.Text = "Description";
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell8.Weight = 4.5314517992394521D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.BackColor = System.Drawing.Color.White;
        this.xrTableCell10.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell10.BorderWidth = 2F;
        this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell10.ForeColor = System.Drawing.Color.Black;
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0, 100F);
        this.xrTableCell10.StylePriority.UseBackColor = false;
        this.xrTableCell10.StylePriority.UseBorderColor = false;
        this.xrTableCell10.StylePriority.UseBorders = false;
        this.xrTableCell10.StylePriority.UseBorderWidth = false;
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.StylePriority.UseForeColor = false;
        this.xrTableCell10.StylePriority.UsePadding = false;
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        this.xrTableCell10.Text = "Suggested Price List";
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell10.Weight = 1.678406100049755D;
        // 
        // xrTable4
        // 
        this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 89.99996F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 5, 0, 100F);
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
        this.xrTable4.SizeF = new System.Drawing.SizeF(836F, 29.37F);
        this.xrTable4.StylePriority.UseBorders = false;
        this.xrTable4.StylePriority.UsePadding = false;
        // 
        // xrTableRow5
        // 
        this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell12,
            this.xrTableCell15});
        this.xrTableRow5.Name = "xrTableRow5";
        this.xrTableRow5.Weight = 1D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.Left;
        this.xrTableCell7.BorderWidth = 2F;
        this.xrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_BOM_SelectRow.ModelPartNo")});
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseBorders = false;
        this.xrTableCell7.StylePriority.UseBorderWidth = false;
        this.xrTableCell7.Weight = 2.152499695402196D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrTableCell12.BorderWidth = 2F;
        this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_BOM_SelectRow.Modeldesc")});
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 5, 0, 100F);
        this.xrTableCell12.StylePriority.UseBorders = false;
        this.xrTableCell12.StylePriority.UseBorderWidth = false;
        this.xrTableCell12.StylePriority.UsePadding = false;
        this.xrTableCell12.Weight = 4.5297309638112209D;
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrTableCell15.BorderWidth = 2F;
        this.xrTableCell15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_BOM_SelectRow.ModelLocalPrice")});
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0, 100F);
        this.xrTableCell15.StylePriority.UseBorders = false;
        this.xrTableCell15.StylePriority.UseBorderWidth = false;
        this.xrTableCell15.StylePriority.UsePadding = false;
        this.xrTableCell15.StylePriority.UseTextAlignment = false;
        this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell15.Weight = 1.677769645962365D;
        this.xrTableCell15.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell15_BeforePrint_1);
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel3});
        this.PageHeader.HeightF = 40.23991F;
        this.PageHeader.Name = "PageHeader";
        // 
        // xrLabel6
        // 
        this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 12F);
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(525F, 9.999974F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(313.9998F, 23F);
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.StylePriority.UseTextAlignment = false;
        this.xrLabel6.Text = "xrLabel6";
        this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrLabel3
        // 
        this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Bookmark", null, "sp_Inv_BOM_SelectRow.ModelNo")});
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 12F);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 9.999974F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(197.6973F, 23F);
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "Pegasus Technologies";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrLabel3.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrLabel3_PrintOnPage);
        // 
        // xrTable6
        // 
        this.xrTable6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable6.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
        this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 3.632673F);
        this.xrTable6.Name = "xrTable6";
        this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
        this.xrTable6.SizeF = new System.Drawing.SizeF(835.9998F, 19.79166F);
        this.xrTable6.StylePriority.UseBorders = false;
        this.xrTable6.StylePriority.UseFont = false;
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell11});
        this.xrTableRow4.Name = "xrTableRow4";
        this.xrTableRow4.Weight = 1D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.BackColor = System.Drawing.Color.Black;
        this.xrTableCell4.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_BOM_SelectRow.ModelNo")});
        this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell4.ForeColor = System.Drawing.Color.White;
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrTableCell4.StylePriority.UseBackColor = false;
        this.xrTableCell4.StylePriority.UseBorderColor = false;
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UseForeColor = false;
        this.xrTableCell4.StylePriority.UsePadding = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.Text = "Part Number";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell4.Weight = 2.1533178436892397D;
        this.xrTableCell4.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell4_BeforePrint_1);
        this.xrTableCell4.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell4_PrintOnPage);
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.BackColor = System.Drawing.Color.Black;
        this.xrTableCell11.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_BOM_SelectRow.ModelName")});
        this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell11.ForeColor = System.Drawing.Color.White;
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseBackColor = false;
        this.xrTableCell11.StylePriority.UseBorderColor = false;
        this.xrTableCell11.StylePriority.UseBorders = false;
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UseForeColor = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "Price";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell11.Weight = 6.2098578992892062D;
        // 
        // GroupHeader2
        // 
        this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3,
            this.xrTable4,
            this.xrRichText1,
            this.xrTable6});
        this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ModelNo", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader2.HeightF = 119.37F;
        this.GroupHeader2.Level = 1;
        this.GroupHeader2.Name = "GroupHeader2";
        this.GroupHeader2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader2_BeforePrint_1);
        // 
        // GroupFooter2
        // 
        this.GroupFooter2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1,
            this.xrTable5,
            this.xrRichText3});
        this.GroupFooter2.HeightF = 88.41667F;
        this.GroupFooter2.Level = 1;
        this.GroupFooter2.Name = "GroupFooter2";
        this.GroupFooter2.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
        this.GroupFooter2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupFooter2_BeforePrint);
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrPanel1.BorderWidth = 2F;
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(4.5F, 0F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(836F, 2.083333F);
        this.xrPanel1.StylePriority.UseBorders = false;
        this.xrPanel1.StylePriority.UseBorderWidth = false;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "Unit";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell1.Weight = 0.64634153891357382D;
        // 
        // PriceList
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.GroupHeader1,
            this.ReportHeader,
            this.PageHeader,
            this.GroupHeader2,
            this.GroupFooter2});
        this.Bookmark = "Table of Contents";
        this.DataMember = "sp_Inv_BOM_SelectRow";
        this.DataSource = this.inventoryDataSet1;
        this.Margins = new System.Drawing.Printing.Margins(5, 0, 1, 2);
        this.Version = "14.1";
        this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Productreport_BeforePrint);
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.inventoryDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion






    private void xrTableCell14_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell14.Text = (float.Parse(xrTableCell14.Text) * float.Parse(Curr_ExchangeRate)).ToString();

            //  xrTableCell14.Text = objsys.GetCurencyConversionForInv(ModelCurrencyId, (float.Parse(xrTableCell14.Text) * float.Parse(Curr_ExchangeRate)).ToString());
        }
        catch
        {
            xrTableCell14.Text = "0";
        }
    }

    private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {



    }

    private void Productreport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {




        //if (GroupBy == "NoGroup")
        //{
        //    GroupBy_Brand.Visible = false;
        //    GroupBy_Category.Visible = false;
        //}

        //if (GroupBy == "Brand")
        //{
        //    GroupBy_Brand.Visible = true;
        //    GroupBy_Category.Visible = false;
        //}

        //if (GroupBy == "Category")
        //{
        //    GroupBy_Brand.Visible = false;
        //    GroupBy_Category.Visible = true;
        //}

        //if (GroupBy == "Both")
        //{
        //    GroupBy_Brand.Visible = true;
        //    GroupBy_Category.Visible = true;
        //}

    }



    private void xrPictureBox4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            //xrPictureBox4.ImageUrl = "~/CompanyResource/" + GetCurrentColumnValue("Company_Id").ToString() + "/Model/" + GetCurrentColumnValue("ModelImage").ToString();

        }
        catch
        {
        }
    }

    private void xrTableCell10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell10.Text = SystemParameter.GetCurrencySmbol(GetCurrentColumnValue("CurrencyId").ToString(), Resources.Attendance.Unit_Price,_strConString);
        }
        catch
        {
        }

    }

    private void xrLabel5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrLabel5.Text = objsys.GetCurencyConversionForInv(ModelCurrencyId, xrLabel5.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrLabel4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrLabel4.Text = SystemParameter.GetCurrencySmbol(ModelCurrencyId, Resources.Attendance.Total_Price);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }

    private void xrLabel4_SummaryRowChanged(object sender, EventArgs e)
    {

    }

    private void xrLabel5_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(ModelCurrencyId, TotalPrice.ToString());
        e.Handled = true;
    }

    private void xrLabel5_SummaryRowChanged(object sender, EventArgs e)
    {
        TotalPrice += Convert.ToDouble(GetCurrentColumnValue("LineTotal").ToString());
    }

    private void xrTableCell3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        // xrTableCell3.Text = SystemParameter.GetCurrencySmbol(GetCurrentColumnValue("CurrencyId").ToString(), Resources.Attendance.Line_total);

    }

    private void xrTableCell4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            // xrTableCell4.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyId").ToString(), xrTableCell4.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell12_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell12.Text = objsys.GetCurencyConversionForInv(ModelCurrencyId, xrTableCell12.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell7.Text = SystemParameter.GetCurrencySmbol(ModelCurrencyId, "Model Price",_strConString);
        }
        catch
        {
        }
    }

    private void xrTableCell13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell13.Text = objsys.GetCurencyConversionForInv(ModelCurrencyId, xrTableCell13.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell2.Text = objsys.GetCurencyConversionForInv(ModelCurrencyId, xrTableCell2.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell15_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell15.Text = objsys.GetCurencyConversionForInv(ModelCurrencyId, xrTableCell15.Text);
        }
        catch
        {
        }
    }

    private void ReportFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }

    private void xrTableCell15_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {

            xrTableCell15.Text = Math.Round(float.Parse(xrTableCell15.Text) * float.Parse(Curr_ExchangeRate), 0).ToString();

            // xrTableCell15.Text = objsys.GetCurencyConversionForInv(ModelCurrencyId, (float.Parse(xrTableCell15.Text) * float.Parse(Curr_ExchangeRate)).ToString());
        }
        catch
        {
            xrTableCell15.Text = "0";
        }
    }

    private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }

    private void GroupFooter2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        xrTable5.Rows.Clear();




        if (GetCurrentColumnValue("ModelId") != null)
        {
            DataTable DtMain = objInvModelCategoryProduct.SelectModelCategoryProductRow(GetCurrentColumnValue("ModelId").ToString());

            if (DtMain.Rows.Count > 0)
            {
                //THIS CODE FOR SET THE FIRST ROW OF RELATED PRODUCT
                XRTableRow drCategoryheader = new XRTableRow();
                xrTable5.Rows.Add(drCategoryheader);
                XRTableCell xrCategoryCellheader = new XRTableCell();

                xrCategoryCellheader.Text = "RELATED PRODUCT";
                xrCategoryCellheader.Font = new Font("Times New Roman", 10, FontStyle.Bold);
                xrCategoryCellheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {


                    xrCategoryCellheader.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right;


                }
                catch
                {
                }
                drCategoryheader.Cells.Add(xrCategoryCellheader);
                xrTable5.Rows[drCategoryheader.Index].Cells[0].SizeF = new SizeF(836.00F, 25F);

                //THIS CODE FOR INSRET THE SECOND LINE IN RELATED PRODUCT

                //CODE START
                XRTableRow dritemheader = new XRTableRow();
                xrTable5.Rows.Add(dritemheader);
                XRTableCell xritemidheader = new XRTableCell();
                XRTableCell xritemnameheader = new XRTableCell();
                XRTableCell xrunitnameheader = new XRTableCell();
                XRTableCell xrStockViewheader = new XRTableCell();
                XRTableCell xrUnitPriceheader = new XRTableCell();

                xritemidheader.Text = "Product Id";




                xritemidheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {


                    xritemidheader.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Top;


                }
                catch
                {
                }


                xritemnameheader.Text = "Product Name";

                xritemnameheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    xritemnameheader.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Top;

                }
                catch
                {
                }




                xrunitnameheader.Text = "Unit";

                xrunitnameheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {

                    xrunitnameheader.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Top;

                }
                catch
                {
                }


                xrStockViewheader.Text = "Stock";

                xrStockViewheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {

                    xrStockViewheader.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Top;

                }
                catch
                {
                }



                xrUnitPriceheader.Text = SystemParameter.GetCurrencySmbol(ModelCurrencyId, Resources.Attendance.Price,_strConString);

                xrUnitPriceheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {

                    xrUnitPriceheader.Borders = DevExpress.XtraPrinting.BorderSide.All;

                }
                catch
                {
                }



                dritemheader.Cells.Add(xritemidheader);
                dritemheader.Cells.Add(xritemnameheader);
                dritemheader.Cells.Add(xrunitnameheader);
                dritemheader.Cells.Add(xrStockViewheader);
                dritemheader.Cells.Add(xrUnitPriceheader);
                xrTable5.Rows[dritemheader.Index].Cells[0].SizeF = new SizeF(162.12F, 25F);
                xrTable5.Rows[dritemheader.Index].Cells[1].SizeF = new SizeF(425.84F, 25F);
                xrTable5.Rows[dritemheader.Index].Cells[2].SizeF = new SizeF(64.63F, 25F);
                xrTable5.Rows[dritemheader.Index].Cells[3].SizeF = new SizeF(72.29F, 25F);
                xrTable5.Rows[dritemheader.Index].Cells[4].SizeF = new SizeF(111.11F, 25F);

                xrTable5.Rows[dritemheader.Index].Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                xrTable5.Rows[dritemheader.Index].Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

                //CODE END
                xrTable5.Visible = true;
                xrTableCell20.Text = SystemParameter.GetCurrencySmbol(ModelCurrencyId, Resources.Attendance.Price,_strConString);

                DataTable dtDistinct = new DataView(DtMain, "", "", DataViewRowState.CurrentRows).ToTable(true, "CategoryId");
                for (int j = 0; j < dtDistinct.Rows.Count; j++)
                {
                    XRTableRow drCategory = new XRTableRow();
                    xrTable5.Rows.Add(drCategory);
                    XRTableCell xrCategoryCell = new XRTableCell();

                    DataTable dt = new DataTable();

                    dt = new DataView(DtMain, "CategoryId=" + dtDistinct.Rows[j]["CategoryId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                    xrCategoryCell.Text = dt.Rows[0]["CategoryName"].ToString();
                    xrCategoryCell.Font = new Font("Times New Roman", 10, FontStyle.Bold);
                    xrCategoryCell.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    try
                    {


                        xrCategoryCell.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Right;


                    }
                    catch
                    {
                    }
                    drCategory.Cells.Add(xrCategoryCell);
                    xrTable5.Rows[drCategory.Index].Cells[0].SizeF = new SizeF(835.99F, 25F);
                    xrTable5.Rows[drCategory.Index].Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {




                        XRTableRow dr = new XRTableRow();
                        xrTable5.Rows.Add(dr);
                        XRTableCell ProductId = new XRTableCell();
                        XRTableCell ProductName = new XRTableCell();
                        XRTableCell Unit = new XRTableCell();
                        XRTableCell Stock = new XRTableCell();
                        XRTableCell UnitPrice = new XRTableCell();
                        ProductId.Text = dt.Rows[i]["ProductCode"].ToString();
                        ProductId.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        try
                        {
                            ProductId.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;
                        }
                        catch
                        {
                        }

                        ProductName.Text = dt.Rows[i]["ProductName"].ToString();
                        ProductName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        try
                        {

                            ProductName.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;


                        }
                        catch
                        {
                        }

                        Unit.Text = dt.Rows[i]["UnitName"].ToString();
                        Unit.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        try
                        {
                            Unit.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;



                        }
                        catch
                        {
                        }
                        try
                        {
                            Stock.Text = GetAmountDecimal(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["BrandId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString(), System.Web.HttpContext.Current.Session["FinanceYearId"].ToString(), dt.Rows[i]["ProductId"].ToString()).Rows[0]["Quantity"].ToString());
                        }
                        catch
                        {
                            Stock.Text = "0.000";
                        }
                        Stock.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        try
                        {
                            Stock.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;

                        }
                        catch
                        {
                        }



                        string ProductUnitPrice = Math.Round(float.Parse(dt.Rows[i]["UnitPrice"].ToString()) * float.Parse(Curr_ExchangeRate), 0).ToString();

                        UnitPrice.Text = GetAmountDecimal(ProductUnitPrice);
                        UnitPrice.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                        try
                        {

                            UnitPrice.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;

                        }
                        catch
                        {
                        }


                        // FCExpAmount.GetEffectiveTextAlignment() = DevExpress.XtraPrinting.TextAlignment.MiddleRight;




                        dr.Cells.Add(ProductId);
                        dr.Cells.Add(ProductName);
                        dr.Cells.Add(Unit);
                        dr.Cells.Add(Stock);
                        dr.Cells.Add(UnitPrice);
                        xrTable5.Rows[dr.Index].Cells[0].SizeF = new SizeF(162.12F, 25F);
                        xrTable5.Rows[dr.Index].Cells[1].SizeF = new SizeF(425.84F, 25F);
                        xrTable5.Rows[dr.Index].Cells[2].SizeF = new SizeF(64.63F, 25F);
                        xrTable5.Rows[dr.Index].Cells[3].SizeF = new SizeF(72.29F, 25F);
                        xrTable5.Rows[dr.Index].Cells[4].SizeF = new SizeF(111.11F, 25F);


                        xrTable5.Rows[dr.Index].Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                        xrTable5.Rows[dr.Index].Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

                        //xrTableCell12.Text = dt.Rows[i]["Currency_Name"].ToString();
                        //xrTable1.Rows[dr.Index].Cells[0].Text = dt.Rows[i]["Currency_Name"].ToString();
                    }

                }



                int size = 70;




            }
            else
            {
                xrTable5.Visible = false;
                try
                {
                    xrTable5.DataBindings.Clear();
                }
                catch
                {
                }
            }
        }
        else
        {
            xrTable5.Visible = false;
            try
            {
                xrTable5.DataBindings.Clear();
            }
            catch
            {
            }
        }



    }

    public string GetAmountDecimal(string Amount)
    {
        return objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), Amount);

    }

    private void GroupHeader2_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
    {




    }

    private void xrTableCell4_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (!isReportLoad)
        //{
        //    System.Web.HttpContext.Current.Session["Pagemodel"] = null;
        //}

        //DataTable dt = new DataTable();


        //if (System.Web.HttpContext.Current.Session["Pagemodel"] == null)
        //{

        //    dt = new DataTable();
        //    dt.Columns.Add("modelNo");
        //    dt.Columns.Add("PageNumber");
        //    DataRow dr = dt.NewRow();
        //    dr[0] = xrTableCell4.Text;
        //    dr[1] = "3";
        //    dt.Rows.Add(dr);
        //    System.Web.HttpContext.Current.Session["Pagemodel"] = dt;
        //}
        //else
        //{
        //    dt = (DataTable)System.Web.HttpContext.Current.Session["Pagemodel"];
        //    DataRow dr = dt.NewRow();
        //    dr[0] = xrTableCell4.Text;
        //    dr[1] = (e.PageIndex + 1).ToString();
        //    dt.Rows.Add(dr);
        //    System.Web.HttpContext.Current.Session["Pagemodel"] = dt;

        //}
        //isReportLoad = true;
    }
    private void ReportHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

        //if (System.Web.HttpContext.Current.Session["Pagemodel"] != null)
        //{

        //    DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["Pagemodel"];

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        XRTableRow dr = new XRTableRow();
        //        xrTablePageIndex.Rows.Add(dr);
        //        XRTableCell ModelNumber = new XRTableCell();
        //        XRTableCell PageNumber = new XRTableCell();
        //        xrTablePageIndex.Visible = true;


        //        string sql = "select Inv_Product_CategoryMaster.Category_Name,Inv_Model_Category.ModelId,Inv_Model_Category.CategoryId from Inv_ModelMaster inner join Inv_Model_Category on Inv_ModelMaster.Trans_Id=Inv_Model_Category.ModelId inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.Category_Id=Inv_Model_Category.CategoryId and Inv_ModelMaster.Model_No='" + dt.Rows[i]["modelNo"].ToString().Trim()+ "'";
        //        try
        //        {
        //            ModelNumber.Text = objda.return_DataTable(sql).Rows[0]["Category_Name"].ToString() + " - " + dt.Rows[i]["modelNo"].ToString();
        //        }
        //        catch
        //        {
        //            ModelNumber.Text = dt.Rows[i]["modelNo"].ToString();
        //        }

        //        ModelNumber.Font = new Font("Times New Roman", 14, FontStyle.Regular);

        //        ModelNumber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
        //        ModelNumber.Borders = DevExpress.XtraPrinting.BorderSide.None;
        //        //ModelNumber.Target = "_self";
        //        //ModelNumber.NavigateUrl = xrLabel2.Name;
        //        //ModelNumber.Bookmark = dt.Rows[i]["modelNo"].ToString();


        //        //try
        //        //{


        //        //    ModelNumber.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom;


        //        //}
        //        //catch
        //        //{
        //        //}
        //        ModelNumber.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0423ff");

        //        PageNumber.Text = dt.Rows[i]["PageNumber"].ToString();
        //        PageNumber.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
        //        PageNumber.Borders = DevExpress.XtraPrinting.BorderSide.None;
        //        //try
        //        //{

        //        //    PageNumber.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom | DevExpress.XtraPrinting.BorderSide.Right;


        //        //}
        //        //catch
        //        //{
        //        //}
        //        PageNumber.Font = new Font("Times New Roman", 14, FontStyle.Regular);

        //        dr.Cells.Add(ModelNumber);
        //        dr.Cells.Add(PageNumber);

        //        xrTablePageIndex.Rows[dr.Index].Cells[0].SizeF = new SizeF(332.99F,50F);
        //        xrTablePageIndex.Rows[dr.Index].Cells[1].SizeF = new SizeF(109.11F, 50F);



        //        xrTablePageIndex.Rows[dr.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;



        //    }

        //}

        //this.ReportHeader.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;

    }

    private void xrLabel4_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

    }

    private void xrTableCell4_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }

    private void GroupHeader3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {


    }

    private void xrTableCell1_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    string s = (xrTableCell2.Text).ToString();

        //    ((XRTableCell)sender).Bookmark += s;
        //}
        //catch
        //{

        //}


    }

    private void xrLabel3_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //string s = (e.PageIndex + 1).ToString();
        //// Set the label's text and bookmark (this label is located on the PageHeader
        //// band, so it will show a bookmark for every report page
        //if (e.PageIndex == 1)
        //{
        //    ((XRLabel)sender).Bookmark = "Page Index";

        //}
    }

    private void xrTableCell20_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }





}
