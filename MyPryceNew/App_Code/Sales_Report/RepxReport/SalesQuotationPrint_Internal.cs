using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

/// <summary>
/// Summary description for PobySupplier
/// </summary>
public class SalesQuotationPrint_Internal : DevExpress.XtraReports.UI.XtraReport
{
  
    Inv_ParameterMaster objParam = null;
    SystemParameter objsys = null;
    Set_AddressChild Objaddress = null;
    LocationMaster ObjLocationMaster = null;
    CurrencyMaster objCurrency = null;
    private string _strConString = string.Empty;
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private InventoryDataSet inventoryDataSet1;
    private PageFooterBand PageFooter;
    private ReportHeaderBand ReportHeader;
    private PurchaseDataSet purchaseDataSet1;
    private GroupHeaderBand GroupHeader1;
    private SalesDataSet salesDataSet1;
    private GroupFooterBand GroupFooter1;
    private GroupFooterBand GroupFooter2;
    private ReportFooterBand ReportFooter;
    private XRPanel xrPanel1;
    private XRLabel xrLabel3;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel1;
    private XRPageInfo xrPageInfo1;
    private XRLabel xrLabel14;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrLabel5;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell40;
    private XRLabel xrLabel7;
    private SalesDataSetTableAdapters.sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter1;
    private XRTable xrTable4;
    private XRTableRow xrTableRow8;
    private XRPictureBox xrPictureBox2;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell54;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell52;
    private XRLabel xrLabel8;
    private XRLabel xrLabel9;
    private XRLabel xrLabel10;
    private XRLabel xrLabel12;
    private XRLabel xrLabel13;
    private XRLabel xrLabel16;
    private XRLabel xrLabel15;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel26;
    private XRPanel xrPanel35;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell17;
    private XRLabel xrLabel11;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell34;
    private XRPanel xrPanel2;
    private XRPanel xrPanel6;
    private XRLabel xrLabel38;
    private XRRichText xrRichText1;
    private XRLabel xrLabel55;
    private XRLabel xrLabel54;
    private XRPanel xrPanel7;
    private XRPanel xrPanel4;
    private XRLabel xrLabel51;
    private XRLabel xrLabel50;
    private XRLabel xrLabel52;
    private XRLabel xrLabel53;
    private XRPanel xrPanel3;
    private XRLabel xrLabel34;
    private XRLabel xrLabel35;
    private XRLabel xrLabel36;
    private XRLabel xrLabel37;
    private XRLabel xrLabel56;
    private XRLabel xrLabel57;
    private XRLabel xrLabel27;
    private XRLabel xrLabel33;
    private XRLabel xrLabel58;
    private XRLabel xrLabel43;
    private XRLabel xrLabel61;
    private XRLabel xrLabel60;
    private XRLabel xrLabel40;
    private XRLabel xrLabel39;
    private XRLabel xrLabel42;
    private XRLabel xrLabel41;
    private XRRichText xrRichText2;
    private XRRichText xrRichText3;
    private XRLabel xrLabel24;
    private XRLabel xrLabel25;
    private XRLabel xrLabel44;
    private XRLabel xrLabel23;
    private XRLabel xrLabel4;
    private XRLabel xrLabel6;
    private XRLabel xrLabel22;
    private XRPictureBox xrPictureBox3;
    private XRLabel xrLabel47;
    private XRLabel xrLabel48;
    private XRLabel xrLabel46;
    private XRLabel xrLabel45;
    private XRRichText xrRichText4;
    private XRRichText xrRichText5;
    private XRLabel xrLabel62;
    private XRLabel xrLabel64;
    private XRLabel xrLabel65;
    private XRLabel xrLabel59;
    private XRLabel xrLabel49;
    private XRLabel xrLabel63;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public SalesQuotationPrint_Internal(string strConString)
    {
        InitializeComponent();
        objParam = new Inv_ParameterMaster(strConString);
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

    string ProductParameter = string.Empty;
    string CompanyId = string.Empty;
    public void settitle(string TitleName)
    {
        xrLabel3.Text = TitleName;
    }
   
    public void setcompanyname(string companyname)
    {
        xrLabel1.Text = companyname;
     
       
    }
    public void SetImage(string Url)
    {
        xrPictureBox1.ImageUrl = Url;
    }
    public void setSignature(string Url)
    {
        try
        {
            xrPictureBox3.ImageUrl = Url;
        }
        catch
        {
        }
    }
    public void setCompanyArebicName(string ArebicName)
    {
        xrLabel11.Text = ArebicName;
        // xrPictureBox4.ImageUrl = "~/Images/Arabic.png";
        
    }
   
    public void setProductParameterValue(string value)
    {
        ProductParameter = value;
    }




    public void setUserName(string UserName)
    {
        xrLabel5.Text = UserName;
    }




    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string resourceFileName = "SalesQuotationPrint_Internal.resx";
        DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary6 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary7 = new DevExpress.XtraReports.UI.XRSummary();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrRichText4 = new DevExpress.XtraReports.UI.XRRichText();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.salesDataSet1 = new SalesDataSet();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.inventoryDataSet1 = new InventoryDataSet();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.purchaseDataSet1 = new PurchaseDataSet();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrRichText3 = new DevExpress.XtraReports.UI.XRRichText();
        this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel35 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel7 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel6 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
        this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrRichText5 = new DevExpress.XtraReports.UI.XRRichText();
        this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrRichText2 = new DevExpress.XtraReports.UI.XRRichText();
        this.xrPictureBox3 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter1 = new SalesDataSetTableAdapters.sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter();
        this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.salesDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.inventoryDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
        this.Detail.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.Detail.HeightF = 74.37499F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.StylePriority.UseFont = false;
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
        // 
        // xrTable4
        // 
        this.xrTable4.BorderColor = System.Drawing.Color.Black;
        this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable4.Font = new System.Drawing.Font("Verdana", 8F);
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(1.419624F, 0F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8});
        this.xrTable4.SizeF = new System.Drawing.SizeF(824.4711F, 74.37499F);
        this.xrTable4.StylePriority.UseBorderColor = false;
        this.xrTable4.StylePriority.UseBorders = false;
        this.xrTable4.StylePriority.UseFont = false;
        this.xrTable4.StylePriority.UseTextAlignment = false;
        this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrTableRow8
        // 
        this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell34,
            this.xrTableCell33,
            this.xrTableCell45,
            this.xrTableCell54,
            this.xrTableCell17,
            this.xrTableCell46,
            this.xrTableCell48,
            this.xrTableCell52});
        this.xrTableRow8.Name = "xrTableRow8";
        this.xrTableRow8.Weight = 1D;
        // 
        // xrTableCell34
        // 
        this.xrTableCell34.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.ProductSerialNumber")});
        this.xrTableCell34.Name = "xrTableCell34";
        this.xrTableCell34.StylePriority.UseBorders = false;
        this.xrTableCell34.Text = "xrTableCell34";
        this.xrTableCell34.Weight = 0.5297078969761001D;
        // 
        // xrTableCell33
        // 
        this.xrTableCell33.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell33.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2});
        this.xrTableCell33.Name = "xrTableCell33";
        this.xrTableCell33.StylePriority.UseBorders = false;
        this.xrTableCell33.Text = "xrTableCell33";
        this.xrTableCell33.Weight = 0.93477158450555708D;
        // 
        // xrPictureBox2
        // 
        this.xrPictureBox2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(4F, 5F);
        this.xrPictureBox2.Name = "xrPictureBox2";
        this.xrPictureBox2.SizeF = new System.Drawing.SizeF(82F, 65F);
        this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox2.StylePriority.UseBorders = false;
        this.xrPictureBox2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPictureBox2_BeforePrint);
        // 
        // xrTableCell45
        // 
        this.xrTableCell45.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell45.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Productcode")});
        this.xrTableCell45.Name = "xrTableCell45";
        this.xrTableCell45.StylePriority.UseBorders = false;
        this.xrTableCell45.StylePriority.UseTextAlignment = false;
        this.xrTableCell45.Text = "Product";
        this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell45.Weight = 1.2568082757438039D;
        // 
        // xrTableCell54
        // 
        this.xrTableCell54.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell54.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText4});
        this.xrTableCell54.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.ProductName")});
        this.xrTableCell54.Name = "xrTableCell54";
        this.xrTableCell54.StylePriority.UseBorders = false;
        this.xrTableCell54.StylePriority.UseTextAlignment = false;
        this.xrTableCell54.Text = "xrTableCell54";
        this.xrTableCell54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell54.Weight = 2.9501014783002044D;
        // 
        // xrRichText4
        // 
        this.xrRichText4.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrRichText4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.ProductDescription")});
        this.xrRichText4.LocationFloat = new DevExpress.Utils.PointFloat(1.999993F, 1.999996F);
        this.xrRichText4.Name = "xrRichText4";
        this.xrRichText4.SizeF = new System.Drawing.SizeF(284.5F, 70F);
        this.xrRichText4.StylePriority.UseBorders = false;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Unit_Name")});
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseBorders = false;
        this.xrTableCell17.StylePriority.UseTextAlignment = false;
        this.xrTableCell17.Text = "xrTableCell17";
        this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell17.Weight = 0.44135312942228555D;
        this.xrTableCell17.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell17_BeforePrint);
        // 
        // xrTableCell46
        // 
        this.xrTableCell46.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Quantity")});
        this.xrTableCell46.Name = "xrTableCell46";
        this.xrTableCell46.StylePriority.UseBorders = false;
        this.xrTableCell46.StylePriority.UseTextAlignment = false;
        this.xrTableCell46.Text = "Currency";
        this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell46.Weight = 0.64790729783504863D;
        this.xrTableCell46.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell46_BeforePrint);
        // 
        // xrTableCell48
        // 
        this.xrTableCell48.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell48.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.UnitPrice")});
        this.xrTableCell48.Name = "xrTableCell48";
        this.xrTableCell48.StylePriority.UseBorders = false;
        this.xrTableCell48.StylePriority.UseTextAlignment = false;
        this.xrTableCell48.Text = "Quantity";
        this.xrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell48.Weight = 0.73812727115956622D;
        this.xrTableCell48.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell48_BeforePrint);
        // 
        // xrTableCell52
        // 
        this.xrTableCell52.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell52.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.ProductAmount")});
        this.xrTableCell52.Name = "xrTableCell52";
        this.xrTableCell52.StylePriority.UseBorders = false;
        this.xrTableCell52.StylePriority.UseTextAlignment = false;
        this.xrTableCell52.Text = "xrTableCell52";
        this.xrTableCell52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell52.Weight = 0.98693459162295138D;
        this.xrTableCell52.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell52_BeforePrint);
        // 
        // xrLabel7
        // 
        this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Product_Id")});
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(340.375F, 143.2084F);
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(2.000008F, 14.58333F);
        this.xrLabel7.Text = "xrLabel15";
        this.xrLabel7.Visible = false;
        // 
        // salesDataSet1
        // 
        this.salesDataSet1.DataSetName = "SalesDataSet";
        this.salesDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // TopMargin
        // 
        this.TopMargin.HeightF = 0F;
        this.TopMargin.Name = "TopMargin";
        this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.TopMargin.StylePriority.UseTextAlignment = false;
        this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // BottomMargin
        // 
        this.BottomMargin.HeightF = 18F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // inventoryDataSet1
        // 
        this.inventoryDataSet1.DataSetName = "InventoryDataSet";
        this.inventoryDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // PageFooter
        // 
        this.PageFooter.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLabel14,
            this.xrPageInfo2,
            this.xrLabel5});
        this.PageFooter.HeightF = 25.33334F;
        this.PageFooter.Name = "PageFooter";
        this.PageFooter.StylePriority.UseBorders = false;
        // 
        // xrPageInfo1
        // 
        this.xrPageInfo1.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
        this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(500F, 0F);
        this.xrPageInfo1.Name = "xrPageInfo1";
        this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo1.SizeF = new System.Drawing.SizeF(327.0001F, 18.04161F);
        this.xrPageInfo1.StylePriority.UseTextAlignment = false;
        this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrLabel14
        // 
        this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel14.Name = "xrLabel14";
        this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel14.SizeF = new System.Drawing.SizeF(75F, 17.62505F);
        this.xrLabel14.StylePriority.UseFont = false;
        this.xrLabel14.Text = "Created By:";
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Format = "Page{0}Of {1}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(362.5F, 0F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(137.5F, 18.12496F);
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel5
        // 
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(75F, 0F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(287.5F, 17.62505F);
        this.xrLabel5.Text = "xrLabel15";
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
        this.ReportHeader.HeightF = 66.00002F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrPanel1
        // 
        this.xrPanel1.BorderColor = System.Drawing.Color.Black;
        this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrPanel1.BorderWidth = 2F;
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11,
            this.xrPictureBox1,
            this.xrLabel1});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 15.00001F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(830.0001F, 51.00001F);
        this.xrPanel1.StylePriority.UseBorderColor = false;
        this.xrPanel1.StylePriority.UseBorders = false;
        this.xrPanel1.StylePriority.UseBorderWidth = false;
        // 
        // xrLabel11
        // 
        this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(2.000014F, 23.95833F);
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(717.7808F, 23.96002F);
        this.xrLabel11.StylePriority.UseBorders = false;
        this.xrLabel11.StylePriority.UseFont = false;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(731.0001F, 1.999998F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(94.99982F, 45.91835F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrLabel1
        // 
        this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(2.000014F, 1.999998F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(717.7809F, 21.95834F);
        this.xrLabel1.StylePriority.UseBorders = false;
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "xrLabel1";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel3
        // 
        this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(582.6249F, 3.999996F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(241.3752F, 17.79165F);
        this.xrLabel3.StylePriority.UseBorders = false;
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "xrLabel3";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // purchaseDataSet1
        // 
        this.purchaseDataSet1.DataSetName = "SalesDataSet";
        this.purchaseDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel62,
            this.xrLabel64,
            this.xrLabel65,
            this.xrLabel59,
            this.xrLabel49,
            this.xrLabel63,
            this.xrLabel24,
            this.xrLabel25,
            this.xrLabel44,
            this.xrLabel23,
            this.xrLabel4,
            this.xrLabel6,
            this.xrLabel22,
            this.xrRichText3,
            this.xrLabel58,
            this.xrLabel43,
            this.xrLabel61,
            this.xrLabel60,
            this.xrLabel40,
            this.xrLabel39,
            this.xrLabel42,
            this.xrLabel41,
            this.xrPanel35,
            this.xrLabel26,
            this.xrTable1,
            this.xrLabel3,
            this.xrLabel7});
        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("SQuotation_Id", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader1.HeightF = 229.875F;
        this.GroupHeader1.Level = 1;
        this.GroupHeader1.Name = "GroupHeader1";
        this.GroupHeader1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader1_BeforePrint);
        // 
        // xrLabel24
        // 
        this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Field3")});
        this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(373.7807F, 99.99997F);
        this.xrLabel24.Name = "xrLabel24";
        this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel24.SizeF = new System.Drawing.SizeF(178.6707F, 15.00002F);
        this.xrLabel24.StylePriority.UseBorders = false;
        this.xrLabel24.StylePriority.UseFont = false;
        this.xrLabel24.Text = "xrLabel40";
        this.xrLabel24.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel24_BeforePrint);
        // 
        // xrLabel25
        // 
        this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(309F, 99.99994F);
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(64.7807F, 15.00002F);
        this.xrLabel25.StylePriority.UseBorders = false;
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.Text = "Tel No      :";
        // 
        // xrLabel44
        // 
        this.xrLabel44.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel44.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(308.9999F, 115F);
        this.xrLabel44.Name = "xrLabel44";
        this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel44.SizeF = new System.Drawing.SizeF(64.7807F, 15.00005F);
        this.xrLabel44.StylePriority.UseBorders = false;
        this.xrLabel44.StylePriority.UseFont = false;
        this.xrLabel44.Text = "Fax No.    : ";
        // 
        // xrLabel23
        // 
        this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Field3")});
        this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(373.7807F, 115F);
        this.xrLabel23.Name = "xrLabel23";
        this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel23.SizeF = new System.Drawing.SizeF(178.6707F, 14.99999F);
        this.xrLabel23.StylePriority.UseBorders = false;
        this.xrLabel23.StylePriority.UseFont = false;
        this.xrLabel23.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel23_BeforePrint);
        // 
        // xrLabel4
        // 
        this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Field3")});
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(309F, 64.87503F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(243.4514F, 35.12491F);
        this.xrLabel4.Text = "xrLabel36";
        this.xrLabel4.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel4_BeforePrint);
        // 
        // xrLabel6
        // 
        this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Delivered_Name")});
        this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(309F, 44.87502F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(243.4514F, 20.00001F);
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.Text = "xrLabel23";
        // 
        // xrLabel22
        // 
        this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(309F, 24.87501F);
        this.xrLabel22.Name = "xrLabel22";
        this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel22.SizeF = new System.Drawing.SizeF(100F, 20.00001F);
        this.xrLabel22.StylePriority.UseFont = false;
        this.xrLabel22.Text = "Delivery To :-";
        // 
        // xrRichText3
        // 
        this.xrRichText3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Header")});
        this.xrRichText3.LocationFloat = new DevExpress.Utils.PointFloat(2.419631F, 179.7917F);
        this.xrRichText3.Name = "xrRichText3";
        this.xrRichText3.SizeF = new System.Drawing.SizeF(823.471F, 22.99997F);
        // 
        // xrLabel58
        // 
        this.xrLabel58.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(582.6249F, 93.875F);
        this.xrLabel58.Name = "xrLabel58";
        this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel58.SizeF = new System.Drawing.SizeF(73.16669F, 17.79169F);
        this.xrLabel58.StylePriority.UseFont = false;
        this.xrLabel58.Text = "Date           :";
        // 
        // xrLabel43
        // 
        this.xrLabel43.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(582.6249F, 75.37498F);
        this.xrLabel43.Name = "xrLabel43";
        this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel43.SizeF = new System.Drawing.SizeF(72.16656F, 17.79166F);
        this.xrLabel43.StylePriority.UseFont = false;
        this.xrLabel43.Text = "Our Ref#  : ";
        // 
        // xrLabel61
        // 
        this.xrLabel61.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Quotation_Date")});
        this.xrLabel61.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(655.8332F, 93.875F);
        this.xrLabel61.Name = "xrLabel61";
        this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel61.SizeF = new System.Drawing.SizeF(169.0575F, 17.79162F);
        this.xrLabel61.StylePriority.UseFont = false;
        this.xrLabel61.StylePriority.UseTextAlignment = false;
        this.xrLabel61.Text = "xrLabel19";
        this.xrLabel61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrLabel61.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel61_BeforePrint);
        // 
        // xrLabel60
        // 
        this.xrLabel60.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.SQuotation_No")});
        this.xrLabel60.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(655.8332F, 75.37497F);
        this.xrLabel60.Name = "xrLabel60";
        this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel60.SizeF = new System.Drawing.SizeF(169.0576F, 17.79164F);
        this.xrLabel60.StylePriority.UseFont = false;
        this.xrLabel60.StylePriority.UseTextAlignment = false;
        this.xrLabel60.Text = "xrLabel19";
        this.xrLabel60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel40
        // 
        this.xrLabel40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Inq_No")});
        this.xrLabel40.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(655.8332F, 32.37498F);
        this.xrLabel40.Name = "xrLabel40";
        this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel40.SizeF = new System.Drawing.SizeF(169.0576F, 17.79F);
        this.xrLabel40.StylePriority.UseFont = false;
        this.xrLabel40.Text = "xrLabel19";
        // 
        // xrLabel39
        // 
        this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(582.6249F, 32.37498F);
        this.xrLabel39.Name = "xrLabel39";
        this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel39.SizeF = new System.Drawing.SizeF(72.16663F, 17.79F);
        this.xrLabel39.StylePriority.UseFont = false;
        this.xrLabel39.Text = "Your Ref#:            ";
        // 
        // xrLabel42
        // 
        this.xrLabel42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Inq_Date")});
        this.xrLabel42.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(655.8332F, 52.87498F);
        this.xrLabel42.Name = "xrLabel42";
        this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel42.SizeF = new System.Drawing.SizeF(169.0576F, 17.79166F);
        this.xrLabel42.StylePriority.UseFont = false;
        this.xrLabel42.StylePriority.UseTextAlignment = false;
        this.xrLabel42.Text = "xrLabel19";
        this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrLabel42.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel42_BeforePrint);
        // 
        // xrLabel41
        // 
        this.xrLabel41.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(582.6249F, 52.87498F);
        this.xrLabel41.Name = "xrLabel41";
        this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel41.SizeF = new System.Drawing.SizeF(73.16663F, 17.79166F);
        this.xrLabel41.StylePriority.UseFont = false;
        this.xrLabel41.StylePriority.UseTextAlignment = false;
        this.xrLabel41.Text = "Date           :";
        this.xrLabel41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPanel35
        // 
        this.xrPanel35.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPanel35.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel12,
            this.xrLabel13,
            this.xrLabel15,
            this.xrLabel16,
            this.xrLabel18,
            this.xrLabel19,
            this.xrLabel20,
            this.xrLabel21});
        this.xrPanel35.LocationFloat = new DevExpress.Utils.PointFloat(2.000014F, 23.00002F);
        this.xrPanel35.Name = "xrPanel35";
        this.xrPanel35.SizeF = new System.Drawing.SizeF(283.4167F, 125.375F);
        this.xrPanel35.StylePriority.UseBorders = false;
        // 
        // xrLabel10
        // 
        this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 41.87501F);
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(278.36F, 18.75F);
        this.xrLabel10.StylePriority.UseBorders = false;
        this.xrLabel10.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel10_BeforePrint);
        // 
        // xrLabel9
        // 
        this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Customer_Name")});
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 21.875F);
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(278.36F, 20F);
        this.xrLabel9.StylePriority.UseBorders = false;
        this.xrLabel9.Text = "xrLabel9";
        // 
        // xrLabel8
        // 
        this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 1.874989F);
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(278.36F, 20F);
        this.xrLabel8.StylePriority.UseBorders = false;
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.Text = "Quote to :-";
        // 
        // xrLabel12
        // 
        this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 61.49998F);
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(66.49998F, 15.49998F);
        this.xrLabel12.StylePriority.UseBorders = false;
        this.xrLabel12.Text = "Att.        :";
        // 
        // xrLabel13
        // 
        this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Field1")});
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(69.5F, 61.49998F);
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(210.86F, 15.49998F);
        this.xrLabel13.StylePriority.UseBorders = false;
        this.xrLabel13.Text = "xrLabel13";
        this.xrLabel13.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel13_BeforePrint);
        // 
        // xrLabel15
        // 
        this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Field1")});
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(69.08038F, 76.99995F);
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(211.2796F, 14.99999F);
        this.xrLabel15.StylePriority.UseBorders = false;
        this.xrLabel15.Text = "xrLabel13";
        this.xrLabel15.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel15_BeforePrint);
        // 
        // xrLabel16
        // 
        this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 76.99995F);
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(66.49997F, 14.99999F);
        this.xrLabel16.StylePriority.UseBorders = false;
        this.xrLabel16.Text = "Tel         : ";
        // 
        // xrLabel18
        // 
        this.xrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(69.5F, 92F);
        this.xrLabel18.Name = "xrLabel18";
        this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel18.SizeF = new System.Drawing.SizeF(210.86F, 14.99999F);
        this.xrLabel18.StylePriority.UseBorders = false;
        this.xrLabel18.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel18_BeforePrint);
        // 
        // xrLabel19
        // 
        this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 92F);
        this.xrLabel19.Name = "xrLabel19";
        this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel19.SizeF = new System.Drawing.SizeF(66.49996F, 14.99999F);
        this.xrLabel19.StylePriority.UseBorders = false;
        this.xrLabel19.Text = "Fax        :";
        // 
        // xrLabel20
        // 
        this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Field1")});
        this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(69.5F, 107F);
        this.xrLabel20.Name = "xrLabel20";
        this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel20.SizeF = new System.Drawing.SizeF(210.86F, 15F);
        this.xrLabel20.StylePriority.UseBorders = false;
        this.xrLabel20.Text = "xrLabel13";
        this.xrLabel20.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel20_BeforePrint);
        // 
        // xrLabel21
        // 
        this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 107F);
        this.xrLabel21.Name = "xrLabel21";
        this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel21.SizeF = new System.Drawing.SizeF(66.49995F, 15F);
        this.xrLabel21.StylePriority.UseBorders = false;
        this.xrLabel21.Text = "Email    :";
        // 
        // xrLabel26
        // 
        this.xrLabel26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.CustomerId")});
        this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(402.1319F, 143.2084F);
        this.xrLabel26.Name = "xrLabel26";
        this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel26.SizeF = new System.Drawing.SizeF(13.8681F, 7.374985F);
        this.xrLabel26.Text = "xrLabel26";
        this.xrLabel26.Visible = false;
        // 
        // xrTable1
        // 
        this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(1.419623F, 211.875F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
        this.xrTable1.SizeF = new System.Drawing.SizeF(824.5803F, 18F);
        this.xrTable1.StylePriority.UseBorders = false;
        this.xrTable1.StylePriority.UseFont = false;
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell40,
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell9,
            this.xrTableCell5,
            this.xrTableCell11,
            this.xrTableCell6});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "S. No.";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell1.Weight = 0.33633125713635159D;
        // 
        // xrTableCell40
        // 
        this.xrTableCell40.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell40.Name = "xrTableCell40";
        this.xrTableCell40.StylePriority.UseBorders = false;
        this.xrTableCell40.StylePriority.UseTextAlignment = false;
        this.xrTableCell40.Text = "Image";
        this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell40.Weight = 0.59352146614901247D;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.StylePriority.UseBorders = false;
        this.xrTableCell3.Text = "Product  Id \t\t";
        this.xrTableCell3.Weight = 0.79799463750492017D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.Text = "Description";
        this.xrTableCell4.Weight = 1.8731295629895628D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.StylePriority.UseBorders = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.Text = "Unit";
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell9.Weight = 0.2802311272607077D;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "Quantity";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell5.Weight = 0.41137967720530355D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "Unit Price";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell11.Weight = 0.46866516269494357D;
        this.xrTableCell11.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell11_BeforePrint);
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "Line Total";
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell6.Weight = 0.62735544151119582D;
        this.xrTableCell6.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell6_BeforePrint);
        // 
        // GroupFooter1
        // 
        this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel2,
            this.xrPanel4,
            this.xrPanel3,
            this.xrLabel27,
            this.xrLabel33});
        this.GroupFooter1.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.GroupFooter1.HeightF = 162.4167F;
        this.GroupFooter1.Name = "GroupFooter1";
        this.GroupFooter1.StylePriority.UseFont = false;
        this.GroupFooter1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupFooter1_BeforePrint);
        // 
        // xrPanel2
        // 
        this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel55,
            this.xrLabel54,
            this.xrPanel7});
        this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 125F);
        this.xrPanel2.Name = "xrPanel2";
        this.xrPanel2.SizeF = new System.Drawing.SizeF(825.0001F, 37.41672F);
        // 
        // xrLabel55
        // 
        this.xrLabel55.BorderColor = System.Drawing.Color.Black;
        this.xrLabel55.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel55.BorderWidth = 2F;
        this.xrLabel55.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.NetTotal")});
        this.xrLabel55.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(682.5F, 0F);
        this.xrLabel55.Name = "xrLabel55";
        this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel55.SizeF = new System.Drawing.SizeF(142.5001F, 19.70831F);
        this.xrLabel55.StylePriority.UseBorderColor = false;
        this.xrLabel55.StylePriority.UseBorders = false;
        this.xrLabel55.StylePriority.UseBorderWidth = false;
        this.xrLabel55.StylePriority.UseFont = false;
        this.xrLabel55.StylePriority.UseTextAlignment = false;
        xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrLabel55.Summary = xrSummary1;
        this.xrLabel55.Text = "xrLabel55";
        this.xrLabel55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel55.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel55_SummaryGetResult);
        this.xrLabel55.SummaryRowChanged += new System.EventHandler(this.xrLabel55_SummaryRowChanged);
        // 
        // xrLabel54
        // 
        this.xrLabel54.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel54.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel54.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(554.9962F, 0F);
        this.xrLabel54.Name = "xrLabel54";
        this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel54.SizeF = new System.Drawing.SizeF(127.5038F, 19.70831F);
        this.xrLabel54.StylePriority.UseBorderColor = false;
        this.xrLabel54.StylePriority.UseBorders = false;
        this.xrLabel54.StylePriority.UseFont = false;
        this.xrLabel54.StylePriority.UseTextAlignment = false;
        this.xrLabel54.Text = "Net Amount";
        this.xrLabel54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrLabel54.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel54_BeforePrint);
        // 
        // xrPanel7
        // 
        this.xrPanel7.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrPanel7.BorderWidth = 2F;
        this.xrPanel7.LocationFloat = new DevExpress.Utils.PointFloat(4.265944F, 19.70831F);
        this.xrPanel7.Name = "xrPanel7";
        this.xrPanel7.SizeF = new System.Drawing.SizeF(818.7341F, 4.708481F);
        this.xrPanel7.StylePriority.UseBorders = false;
        this.xrPanel7.StylePriority.UseBorderWidth = false;
        // 
        // xrPanel4
        // 
        this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel53,
            this.xrLabel52,
            this.xrLabel50,
            this.xrLabel51});
        this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(529.73F, 81.99997F);
        this.xrPanel4.Name = "xrPanel4";
        this.xrPanel4.SizeF = new System.Drawing.SizeF(297.2701F, 42.70834F);
        // 
        // xrLabel53
        // 
        this.xrLabel53.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel53.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel53.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.TaxValue")});
        this.xrLabel53.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(151.77F, 22.00002F);
        this.xrLabel53.Name = "xrLabel53";
        this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel53.SizeF = new System.Drawing.SizeF(142.5001F, 19.70832F);
        this.xrLabel53.StylePriority.UseBorderColor = false;
        this.xrLabel53.StylePriority.UseBorders = false;
        this.xrLabel53.StylePriority.UseFont = false;
        this.xrLabel53.StylePriority.UseTextAlignment = false;
        xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrLabel53.Summary = xrSummary2;
        this.xrLabel53.Text = "xrLabel53";
        this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel53.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel53_SummaryGetResult);
        this.xrLabel53.SummaryRowChanged += new System.EventHandler(this.xrLabel53_SummaryRowChanged);
        // 
        // xrLabel52
        // 
        this.xrLabel52.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel52.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel52.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(24.26624F, 22.00002F);
        this.xrLabel52.Name = "xrLabel52";
        this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel52.SizeF = new System.Drawing.SizeF(127.5037F, 19.70832F);
        this.xrLabel52.StylePriority.UseBorderColor = false;
        this.xrLabel52.StylePriority.UseBorders = false;
        this.xrLabel52.StylePriority.UseFont = false;
        this.xrLabel52.StylePriority.UseTextAlignment = false;
        this.xrLabel52.Text = "Tax";
        this.xrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel50
        // 
        this.xrLabel50.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel50.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel50.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(24.26611F, 2.291702F);
        this.xrLabel50.Name = "xrLabel50";
        this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel50.SizeF = new System.Drawing.SizeF(127.5038F, 19.70831F);
        this.xrLabel50.StylePriority.UseBorderColor = false;
        this.xrLabel50.StylePriority.UseBorders = false;
        this.xrLabel50.StylePriority.UseFont = false;
        this.xrLabel50.StylePriority.UseTextAlignment = false;
        this.xrLabel50.Text = "Tax %(+)";
        this.xrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel51
        // 
        this.xrLabel51.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel51.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel51.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.TaxPercent")});
        this.xrLabel51.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(151.77F, 2.291641F);
        this.xrLabel51.Name = "xrLabel51";
        this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel51.SizeF = new System.Drawing.SizeF(142.5001F, 19.70831F);
        this.xrLabel51.StylePriority.UseBorderColor = false;
        this.xrLabel51.StylePriority.UseBorders = false;
        this.xrLabel51.StylePriority.UseFont = false;
        this.xrLabel51.StylePriority.UseTextAlignment = false;
        xrSummary3.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrLabel51.Summary = xrSummary3;
        this.xrLabel51.Text = "xrLabel51";
        this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel51.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel51_SummaryGetResult);
        this.xrLabel51.SummaryRowChanged += new System.EventHandler(this.xrLabel51_SummaryRowChanged);
        // 
        // xrPanel3
        // 
        this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel57,
            this.xrLabel56,
            this.xrLabel37,
            this.xrLabel36,
            this.xrLabel35,
            this.xrLabel34});
        this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(529.73F, 19.70997F);
        this.xrPanel3.Name = "xrPanel3";
        this.xrPanel3.SizeF = new System.Drawing.SizeF(297.2701F, 61.99995F);
        // 
        // xrLabel57
        // 
        this.xrLabel57.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel57.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel57.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.PriceAfterDiscount")});
        this.xrLabel57.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(151.77F, 40.62497F);
        this.xrLabel57.Name = "xrLabel57";
        this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel57.SizeF = new System.Drawing.SizeF(141.7245F, 19.37504F);
        this.xrLabel57.StylePriority.UseBorderColor = false;
        this.xrLabel57.StylePriority.UseBorders = false;
        this.xrLabel57.StylePriority.UseFont = false;
        this.xrLabel57.StylePriority.UseTextAlignment = false;
        xrSummary4.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrLabel57.Summary = xrSummary4;
        this.xrLabel57.Text = "xrLabel57";
        this.xrLabel57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel57.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel57_SummaryGetResult);
        this.xrLabel57.SummaryRowChanged += new System.EventHandler(this.xrLabel57_SummaryRowChanged);
        // 
        // xrLabel56
        // 
        this.xrLabel56.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel56.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel56.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(24.26611F, 40.62497F);
        this.xrLabel56.Name = "xrLabel56";
        this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel56.SizeF = new System.Drawing.SizeF(127.5039F, 19.37504F);
        this.xrLabel56.StylePriority.UseBorderColor = false;
        this.xrLabel56.StylePriority.UseBorders = false;
        this.xrLabel56.StylePriority.UseFont = false;
        this.xrLabel56.StylePriority.UseTextAlignment = false;
        this.xrLabel56.Text = "After Discount";
        this.xrLabel56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel37
        // 
        this.xrLabel37.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel37.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.DiscountValue")});
        this.xrLabel37.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(151.77F, 21.24993F);
        this.xrLabel37.Name = "xrLabel37";
        this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel37.SizeF = new System.Drawing.SizeF(142.4999F, 19.37504F);
        this.xrLabel37.StylePriority.UseBorderColor = false;
        this.xrLabel37.StylePriority.UseBorders = false;
        this.xrLabel37.StylePriority.UseFont = false;
        this.xrLabel37.StylePriority.UseTextAlignment = false;
        xrSummary5.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        xrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrLabel37.Summary = xrSummary5;
        this.xrLabel37.Text = "xrLabel7";
        this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel37.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel37_SummaryGetResult);
        this.xrLabel37.SummaryRowChanged += new System.EventHandler(this.xrLabel37_SummaryRowChanged);
        // 
        // xrLabel36
        // 
        this.xrLabel36.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel36.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel36.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(24.26611F, 21.24987F);
        this.xrLabel36.Name = "xrLabel36";
        this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel36.SizeF = new System.Drawing.SizeF(127.5039F, 19.37504F);
        this.xrLabel36.StylePriority.UseBorderColor = false;
        this.xrLabel36.StylePriority.UseBorders = false;
        this.xrLabel36.StylePriority.UseFont = false;
        this.xrLabel36.StylePriority.UseTextAlignment = false;
        this.xrLabel36.Text = "Discount";
        this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel35
        // 
        this.xrLabel35.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel35.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.DiscountPercent")});
        this.xrLabel35.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(151.77F, 1.541616F);
        this.xrLabel35.Name = "xrLabel35";
        this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel35.SizeF = new System.Drawing.SizeF(141.7244F, 19.70831F);
        this.xrLabel35.StylePriority.UseBorderColor = false;
        this.xrLabel35.StylePriority.UseBorders = false;
        this.xrLabel35.StylePriority.UseFont = false;
        this.xrLabel35.StylePriority.UseTextAlignment = false;
        xrSummary6.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        xrSummary6.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrLabel35.Summary = xrSummary6;
        this.xrLabel35.Text = "xrLabel35";
        this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel35.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel35_SummaryGetResult);
        this.xrLabel35.SummaryRowChanged += new System.EventHandler(this.xrLabel35_SummaryRowChanged);
        // 
        // xrLabel34
        // 
        this.xrLabel34.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel34.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel34.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(24.26617F, 1.541616F);
        this.xrLabel34.Name = "xrLabel34";
        this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel34.SizeF = new System.Drawing.SizeF(127.5038F, 19.70831F);
        this.xrLabel34.StylePriority.UseBorderColor = false;
        this.xrLabel34.StylePriority.UseBorders = false;
        this.xrLabel34.StylePriority.UseFont = false;
        this.xrLabel34.StylePriority.UseTextAlignment = false;
        this.xrLabel34.Text = "Discount %(-)";
        this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel27
        // 
        this.xrLabel27.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel27.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(553.9962F, 0F);
        this.xrLabel27.Name = "xrLabel27";
        this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel27.SizeF = new System.Drawing.SizeF(127.5038F, 19.70831F);
        this.xrLabel27.StylePriority.UseBorderColor = false;
        this.xrLabel27.StylePriority.UseBorders = false;
        this.xrLabel27.StylePriority.UseFont = false;
        this.xrLabel27.StylePriority.UseTextAlignment = false;
        this.xrLabel27.Text = "Gross Amount";
        this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel33
        // 
        this.xrLabel33.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel33.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Amount")});
        this.xrLabel33.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(681.5F, 0F);
        this.xrLabel33.Name = "xrLabel33";
        this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel33.SizeF = new System.Drawing.SizeF(141.7245F, 19.70834F);
        this.xrLabel33.StylePriority.UseBorderColor = false;
        this.xrLabel33.StylePriority.UseBorders = false;
        this.xrLabel33.StylePriority.UseFont = false;
        this.xrLabel33.StylePriority.UseTextAlignment = false;
        xrSummary7.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        xrSummary7.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrLabel33.Summary = xrSummary7;
        this.xrLabel33.Text = "xrLabel33";
        this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel33.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel33_SummaryGetResult);
        this.xrLabel33.SummaryRowChanged += new System.EventHandler(this.xrLabel33_SummaryRowChanged);
        // 
        // xrPanel6
        // 
        this.xrPanel6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel38,
            this.xrRichText1});
        this.xrPanel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrPanel6.Name = "xrPanel6";
        this.xrPanel6.SizeF = new System.Drawing.SizeF(827.0002F, 51.12515F);
        // 
        // xrLabel38
        // 
        this.xrLabel38.Font = new System.Drawing.Font("Verdana", 8.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(2.419631F, 6.357829E-05F);
        this.xrLabel38.Name = "xrLabel38";
        this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel38.SizeF = new System.Drawing.SizeF(824.5804F, 22.99995F);
        this.xrLabel38.StylePriority.UseFont = false;
        this.xrLabel38.StylePriority.UseTextAlignment = false;
        this.xrLabel38.Text = "Term & Condition";
        this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrRichText1
        // 
        this.xrRichText1.BorderColor = System.Drawing.Color.Black;
        this.xrRichText1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrRichText1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Condition1")});
        this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(2.419631F, 23.74999F);
        this.xrRichText1.Name = "xrRichText1";
        this.xrRichText1.SizeF = new System.Drawing.SizeF(824.5804F, 21.95842F);
        this.xrRichText1.StylePriority.UseBorderColor = false;
        this.xrRichText1.StylePriority.UseBorders = false;
        // 
        // GroupFooter2
        // 
        this.GroupFooter2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText5,
            this.xrLabel45,
            this.xrLabel47,
            this.xrLabel48,
            this.xrLabel46,
            this.xrRichText2,
            this.xrPanel6,
            this.xrPictureBox3});
        this.GroupFooter2.HeightF = 219.0002F;
        this.GroupFooter2.Level = 1;
        this.GroupFooter2.Name = "GroupFooter2";
        this.GroupFooter2.StylePriority.UseTextAlignment = false;
        this.GroupFooter2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrRichText5
        // 
        this.xrRichText5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.SalesQuotationFooter")});
        this.xrRichText5.LocationFloat = new DevExpress.Utils.PointFloat(4.000012F, 75.20847F);
        this.xrRichText5.Name = "xrRichText5";
        this.xrRichText5.SizeF = new System.Drawing.SizeF(823.0001F, 17.79169F);
        // 
        // xrLabel45
        // 
        this.xrLabel45.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel45.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(3.500001F, 103.5833F);
        this.xrLabel45.Name = "xrLabel45";
        this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel45.SizeF = new System.Drawing.SizeF(412.5F, 20.91668F);
        this.xrLabel45.StylePriority.UseBorders = false;
        this.xrLabel45.StylePriority.UseFont = false;
        this.xrLabel45.StylePriority.UseTextAlignment = false;
        this.xrLabel45.Text = "Signature and Approvals";
        this.xrLabel45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel47
        // 
        this.xrLabel47.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel47.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(3.500001F, 124.5F);
        this.xrLabel47.Name = "xrLabel47";
        this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel47.SizeF = new System.Drawing.SizeF(263.3197F, 23.00002F);
        this.xrLabel47.StylePriority.UseBorders = false;
        this.xrLabel47.StylePriority.UseFont = false;
        this.xrLabel47.StylePriority.UseTextAlignment = false;
        this.xrLabel47.Text = "Sales Person";
        this.xrLabel47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel48
        // 
        this.xrLabel48.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel48.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(266.8197F, 124.5F);
        this.xrLabel48.Name = "xrLabel48";
        this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel48.SizeF = new System.Drawing.SizeF(560.1805F, 23.00008F);
        this.xrLabel48.StylePriority.UseBorders = false;
        this.xrLabel48.StylePriority.UseFont = false;
        this.xrLabel48.StylePriority.UseTextAlignment = false;
        this.xrLabel48.Text = "Manager";
        this.xrLabel48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel46
        // 
        this.xrLabel46.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel46.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(266.8197F, 148F);
        this.xrLabel46.Name = "xrLabel46";
        this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel46.SizeF = new System.Drawing.SizeF(560.1805F, 65.70842F);
        this.xrLabel46.StylePriority.UseBorders = false;
        this.xrLabel46.StylePriority.UseFont = false;
        this.xrLabel46.StylePriority.UseTextAlignment = false;
        this.xrLabel46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrRichText2
        // 
        this.xrRichText2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Footer")});
        this.xrRichText2.LocationFloat = new DevExpress.Utils.PointFloat(3.500001F, 52.4168F);
        this.xrRichText2.Name = "xrRichText2";
        this.xrRichText2.SizeF = new System.Drawing.SizeF(823.5001F, 17.79169F);
        // 
        // xrPictureBox3
        // 
        this.xrPictureBox3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(4.000012F, 148F);
        this.xrPictureBox3.Name = "xrPictureBox3";
        this.xrPictureBox3.SizeF = new System.Drawing.SizeF(262.8197F, 65.70836F);
        this.xrPictureBox3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox3.StylePriority.UseBorders = false;
        // 
        // ReportFooter
        // 
        this.ReportFooter.HeightF = 0F;
        this.ReportFooter.Name = "ReportFooter";
        // 
        // sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter1
        // 
        this.sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter1.ClearBeforeFill = true;
        // 
        // xrLabel59
        // 
        this.xrLabel59.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(585.0753F, 118.5001F);
        this.xrLabel59.Name = "xrLabel59";
        this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel59.SizeF = new System.Drawing.SizeF(72.16656F, 17.79166F);
        this.xrLabel59.StylePriority.UseFont = false;
        this.xrLabel59.Text = "From         : ";
        // 
        // xrLabel49
        // 
        this.xrLabel49.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(585.0753F, 137.0001F);
        this.xrLabel49.Name = "xrLabel49";
        this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel49.SizeF = new System.Drawing.SizeF(73.16669F, 17.79169F);
        this.xrLabel49.StylePriority.UseFont = false;
        this.xrLabel49.Text = "Email        :";
        // 
        // xrLabel63
        // 
        this.xrLabel63.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.Employee")});
        this.xrLabel63.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(658.2837F, 118.5001F);
        this.xrLabel63.Name = "xrLabel63";
        this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel63.SizeF = new System.Drawing.SizeF(165.7162F, 17.79163F);
        this.xrLabel63.StylePriority.UseFont = false;
        this.xrLabel63.StylePriority.UseTextAlignment = false;
        this.xrLabel63.Text = "xrLabel19";
        this.xrLabel63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel62
        // 
        this.xrLabel62.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.SalesPerson_EmailId")});
        this.xrLabel62.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(658.2837F, 137.0001F);
        this.xrLabel62.Name = "xrLabel62";
        this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel62.SizeF = new System.Drawing.SizeF(165.7161F, 17.79163F);
        this.xrLabel62.StylePriority.UseFont = false;
        this.xrLabel62.StylePriority.UseTextAlignment = false;
        this.xrLabel62.Text = "xrLabel19";
        this.xrLabel62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel64
        // 
        this.xrLabel64.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(585.0753F, 155.2084F);
        this.xrLabel64.Name = "xrLabel64";
        this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel64.SizeF = new System.Drawing.SizeF(73.16669F, 17.79169F);
        this.xrLabel64.StylePriority.UseFont = false;
        this.xrLabel64.Text = "Tel No.      :";
        // 
        // xrLabel65
        // 
        this.xrLabel65.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesQuotationDetail_SelectRow_Report.SalesPerson_PhoneNo")});
        this.xrLabel65.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(658.2837F, 155.2084F);
        this.xrLabel65.Name = "xrLabel65";
        this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel65.SizeF = new System.Drawing.SizeF(165.7161F, 17.79163F);
        this.xrLabel65.StylePriority.UseFont = false;
        this.xrLabel65.StylePriority.UseTextAlignment = false;
        this.xrLabel65.Text = "xrLabel19";
        this.xrLabel65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // SalesQuotationPrint_Internal
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.GroupFooter1,
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.ReportHeader,
            this.GroupHeader1,
            this.GroupFooter2,
            this.ReportFooter});
        this.DataMember = "sp_Inv_SalesQuotationDetail_SelectRow_Report";
        this.DataSource = this.salesDataSet1;
        this.Margins = new System.Drawing.Printing.Margins(6, 0, 0, 18);
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.salesDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.inventoryDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
    string CurrencySymbol = string.Empty;
    double TotalGrossAmount = 0;
    double TotalProductAmount = 0;
    double TotalPriceafterDiscount = 0;
    double TotaltaxPer = 0;
    double TotalDiscountPer = 0;
    double TotalTaxvalue = 0;
    double Totaldiscountvalue = 0;
    string CurrencyId = string.Empty;
    double TotalNetAmount = 0;
    double PriceAftertax = 0;
    double totamCustomTaxValue = 0;
    double totamCustomDiscountValue = 0;

    private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
       
            xrRichText4.Visible = true;
        
     



    }

    private void xrLabel7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }

    private void xrPictureBox2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrPictureBox2.ImageUrl = "~/Handler.ashx?ImID=" + GetCurrentColumnValue("Product_Id").ToString() + "";
        }
        catch
        {
        }


    }

    private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {




        if (!Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
        {




        }
        if (!Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
        {


        }

        //here we set width dynamic

        //when tax is false and discount is true
        if (!Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
        {
            if (Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
            {

            }

        }
        //when discount is false and tax is true
        if (Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
        {
            if (!Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
            {


            }

        }

        if (!Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(),  GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(),"IsTaxSales").Rows[0]["ParameterValue"].ToString()) && !Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
        {

            //xrTableCell8.SizeF = new SizeF(400.03F, 37.9F);
        }




    }

    private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (!Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(),  GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(),"IsTaxSales").Rows[0]["ParameterValue"].ToString()) && !Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
        {
            xrPanel3.Visible = false;
            xrPanel4.Visible = false;
            xrLabel27.Visible = false;
            xrLabel33.Visible = false;
           xrPanel3.LocationF = new PointF(200F, 19.71F);
           xrPanel4.LocationF = new PointF(200F, 82F);

            xrLabel55.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            xrPanel2.LocationF = new PointF(0F,1F);

            //xrLabel54.LocationF = new PointF(569.71F, 19.71F);
            //xrLabel55.LocationF = new PointF(694.44F, 19.71F);
            //xrPanel3.LocationF = new PointF(200,12);


        }
        if (Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()) && !Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
        {
            //xrPanel4.Visible = false;
            //xrPanel3.Visible = true;
            //xrPanel3.LocationF = new PointF(535.73F, 19.71F);
            //xrPanel4.LocationF = new PointF(200F, 19.71F);


            xrPanel3.Visible = false;
            xrPanel4.Visible = true;
            xrPanel4.LocationF = new PointF(529.73F, 19.71F);
            xrPanel3.LocationF = new PointF(200F, 19.71F);
            xrLabel56.Visible = true;
            xrLabel57.Visible = true;

            //xrLabel56.Visible = false;
            //xrLabel57.Visible = false;
            xrPanel2.LocationF = new PointF(0F, 82F);
            //xrPanel2.LocationF = new PointF(0F,65.62F);

            //xrLabel54.LocationF = new PointF(569.71F, 80.38F);
            //xrLabel55.LocationF = new PointF(694.44F, 80.38F);

        }
        if (!Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()) && Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
        {
            //xrPanel3.Visible = false;
            //xrPanel4.Visible = true;
            //xrPanel3.LocationF = new PointF(200F, 19.71F);
            //xrPanel4.LocationF = new PointF(535.73F, 19.71F);
            xrPanel4.Visible = false;
            xrPanel3.Visible = true;
            xrPanel4.LocationF = new PointF(200F, 19.71F);
            xrPanel3.LocationF = new PointF(529.73F, 19.71F);
            xrLabel56.Visible = false;
            xrLabel57.Visible = false;

            xrPanel2.LocationF = new PointF(0F, 82F);

            //xrLabel54.LocationF = new PointF(569.71F, 80.38F);
            //xrLabel55.LocationF = new PointF(694.44F, 80.38F);

        }

    }

    private void xrLabel13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            Ems_ContactMaster objcontact = new Ems_ContactMaster(_strConString);

            DataTable dt = objcontact.GetContactTrueById(xrLabel13.Text.Trim());
            string s = string.Empty;

            if (dt.Rows.Count != 0)
            {

                s = dt.Rows[0]["Name"].ToString();
            }
            xrLabel13.Text = s;
        }
        catch
        {



        }
    }

    private void xrLabel15_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

        try
        {
            Ems_ContactMaster objcontact = new Ems_ContactMaster(_strConString);

            DataTable dt = objcontact.GetContactTrueById(xrLabel15.Text.Trim());
            string s = string.Empty;

            if (dt.Rows.Count != 0)
            {

                s = dt.Rows[0]["Field2"].ToString();
            }
            xrLabel15.Text = s;
        }
        catch
        {



        }


    }

    private void xrLabel20_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            Ems_ContactMaster objcontact = new Ems_ContactMaster(_strConString);

            DataTable dt = objcontact.GetContactTrueById(xrLabel20.Text.Trim());
            string s = string.Empty;

            if (dt.Rows.Count != 0)
            {

                s = dt.Rows[0]["Field1"].ToString();
            }
            xrLabel20.Text = s;
        }
        catch
        {



        }
    }

    private void xrLabel10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        string CompanyAddress = string.Empty;
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Customer", xrLabel26.Text);
        if(DtAddress.Rows.Count>0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            if (DtAddress.Rows[0]["Address"].ToString() != "")
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }
            if (DtAddress.Rows[0]["Street"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Street"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Street"].ToString();
                }
            }
            if (DtAddress.Rows[0]["Block"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Block"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Block"].ToString();
                }
            }
            if (DtAddress.Rows[0]["Avenue"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Avenue"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Avenue"].ToString();
                }
            }

            if (DtAddress.Rows[0]["CityId"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["CityId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["CityId"].ToString();
                }

            }
            if (DtAddress.Rows[0]["StateId"].ToString() != "")
            {


                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["StateId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["StateId"].ToString();
                }
            }
            if (DtAddress.Rows[0]["CountryId"].ToString() != "")
            {
                string LocationName = string.Empty;
                DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(GetCurrentColumnValue("Company_Id").ToString(), DtAddress.Rows[0]["CountryId"].ToString());
                if (DtLocation.Rows.Count > 0)
                {


                    if (CompanyAddress != "")
                    {
                        CompanyAddress = CompanyAddress + "," + LocationName;
                    }
                    else
                    {
                        CompanyAddress = LocationName;
                    }
                }

            }
            if (DtAddress.Rows[0]["PinCode"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["PinCode"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["PinCode"].ToString();
                }

            }


        }
        xrLabel10.Text = CompanyAddress;
    }

    private void xrLabel18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Customer", xrLabel26.Text);
        if (DtAddress.Rows.Count > 0)
        {
            xrLabel18.Text = DtAddress.Rows[0]["FaxNo"].ToString();
        }
    }

  

   

    private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //xrTableCell11.Text = SystemParameter.GetCurrencySmbol(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell11.Text);


        DataTable dt = objCurrency.GetCurrencyMasterById(GetCurrentColumnValue("Currency_Id").ToString());
        if (dt.Rows.Count > 0)
        {
            xrTableCell11.Text = xrTableCell11.Text + "    " + "(" + dt.Rows[0]["Currency_Symbol"].ToString()+")";
            CurrencySymbol = dt.Rows[0]["Currency_Symbol"].ToString();
        }
    }

    private void xrTableCell6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        DataTable dt = objCurrency.GetCurrencyMasterById(GetCurrentColumnValue("Currency_Id").ToString());
        if (dt.Rows.Count > 0)
        {
            xrTableCell6.Text = xrTableCell6.Text + "    " +"("+ dt.Rows[0]["Currency_Symbol"].ToString()+")";
            CurrencySymbol = dt.Rows[0]["Currency_Symbol"].ToString();
        }



      

    }

    private void xrTableCell19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //xrTableCell19.Text = SystemParameter.GetCurrencySmbol(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell19.Text);
    }

    private void xrTableCell48_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        xrTableCell48.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell48.Text);
    }

    private void xrTableCell52_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        xrTableCell52.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell52.Text);
  
    }

    private void xrTableCell17_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
      
    }

    private void xrTableCell46_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell46.Text = Math.Round(Convert.ToDouble(xrTableCell46.Text), 0).ToString();
        }
        catch
        {
           

        }
    }

    private void xrLabel33_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, TotalGrossAmount.ToString());
        e.Handled = true;
    }

    private void xrLabel33_SummaryRowChanged(object sender, EventArgs e)
    {
        TotalGrossAmount = Convert.ToDouble(GetCurrentColumnValue("Amount").ToString());
        CurrencyId = GetCurrentColumnValue("Currency_Id").ToString();
    }

    private void xrLabel35_SummaryRowChanged(object sender, EventArgs e)
    {
        TotalDiscountPer = Convert.ToDouble(GetCurrentColumnValue("DiscountPercent").ToString());
       
        CurrencyId = GetCurrentColumnValue("Currency_Id").ToString();
    }

    private void xrLabel35_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {

        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, TotalDiscountPer.ToString());
        e.Handled = true;
    }

    private void xrLabel37_SummaryRowChanged(object sender, EventArgs e)
    {
        totamCustomDiscountValue = Convert.ToDouble(GetCurrentColumnValue("DiscountValue").ToString());
        CurrencyId = GetCurrentColumnValue("Currency_Id").ToString();
    }

    private void xrLabel37_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, totamCustomDiscountValue.ToString());
        e.Handled = true;
    }

    private void xrLabel57_SummaryRowChanged(object sender, EventArgs e)
    {
        TotalPriceafterDiscount = Convert.ToDouble(GetCurrentColumnValue("PriceAfterDiscount").ToString());
        CurrencyId = GetCurrentColumnValue("Currency_Id").ToString();
    }

    private void xrLabel57_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, TotalPriceafterDiscount.ToString());
        e.Handled = true;
    }

    private void xrLabel51_SummaryRowChanged(object sender, EventArgs e)
    {
        TotaltaxPer = Convert.ToDouble(GetCurrentColumnValue("TaxPercent").ToString());
        //Totaldiscountvalue += Convert.ToDouble(GetCurrentColumnValue("DiscountValue1").ToString());
        CurrencyId = GetCurrentColumnValue("Currency_Id").ToString();
    }

    private void xrLabel51_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {

        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, TotaltaxPer.ToString());
        e.Handled = true;
    }

    private void xrLabel53_SummaryRowChanged(object sender, EventArgs e)
    {
        totamCustomTaxValue = Convert.ToDouble(GetCurrentColumnValue("TaxValue").ToString());
        CurrencyId = GetCurrentColumnValue("Currency_Id").ToString();
    }

    private void xrLabel53_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, totamCustomTaxValue.ToString());
        e.Handled = true;
    }

    private void xrLabel55_SummaryRowChanged(object sender, EventArgs e)
    {
        TotalNetAmount = Convert.ToDouble(GetCurrentColumnValue("NetTotal").ToString());
        CurrencyId = GetCurrentColumnValue("Currency_Id").ToString();
    }

    private void xrLabel55_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, TotalNetAmount.ToString());
        e.Handled = true;
    }

    private void xrLabel54_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        DataTable dt = objCurrency.GetCurrencyMasterById(GetCurrentColumnValue("Currency_Id").ToString());
        if (dt.Rows.Count > 0)
        {
            xrLabel54.Text = xrLabel54.Text + "(" + dt.Rows[0]["Currency_Symbol"].ToString() + ")";

        }
    }

    private void xrLabel42_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrLabel42.Text = Convert.ToDateTime(xrLabel42.Text).ToString(objsys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrLabel61_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrLabel61.Text = Convert.ToDateTime(xrLabel61.Text).ToString(objsys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrLabel4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(_strConString);
        string CompanyAddress = string.Empty;
        DataTable DtAddress = objAddMaster.GetAddressDataByTransId(xrLabel4.Text, "0");
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            if (DtAddress.Rows[0]["Address"].ToString() != "")
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }
            if (DtAddress.Rows[0]["Street"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Street"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Street"].ToString();
                }
            }
            if (DtAddress.Rows[0]["Block"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Block"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Block"].ToString();
                }
            }
            if (DtAddress.Rows[0]["Avenue"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Avenue"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Avenue"].ToString();
                }
            }

            if (DtAddress.Rows[0]["CityId"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["CityId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["CityId"].ToString();
                }

            }
            if (DtAddress.Rows[0]["StateId"].ToString() != "")
            {


                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["StateId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["StateId"].ToString();
                }
            }
            if (DtAddress.Rows[0]["CountryId"].ToString() != "")
            {
                string LocationName = string.Empty;
                DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(GetCurrentColumnValue("Company_Id").ToString(), DtAddress.Rows[0]["CountryId"].ToString());
                if (DtLocation.Rows.Count > 0)
                {


                    if (CompanyAddress != "")
                    {
                        CompanyAddress = CompanyAddress + "," + LocationName;
                    }
                    else
                    {
                        CompanyAddress = LocationName;
                    }
                }

            }
            if (DtAddress.Rows[0]["PinCode"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["PinCode"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["PinCode"].ToString();
                }

            }


        }
        xrLabel4.Text = CompanyAddress;
  
    }

    private void xrLabel24_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(_strConString);
        string Companytelno = string.Empty;
        DataTable DtAddress = objAddMaster.GetAddressDataByTransId(xrLabel24.Text, "0");
        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["PhoneNo1"].ToString() != "")
            {

                Companytelno = DtAddress.Rows[0]["PhoneNo1"].ToString();
            }
            //if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
            //{
            //    if (Companytelno != "")
            //    {
            //        Companytelno = Companytelno + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
            //    }
            //    else
            //    {
            //        Companytelno = DtAddress.Rows[0]["PhoneNo2"].ToString();
            //    }
            //}
            if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
            {
                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo1"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["MobileNo1"].ToString();
                }
            }
            //if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
            //{
            //    if (Companytelno != "")
            //    {
            //        Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
            //    }
            //    else
            //    {
            //        Companytelno = DtAddress.Rows[0]["MobileNo2"].ToString();
            //    }
            //}
        }
        xrLabel24.Text = Companytelno;

    }

    private void xrLabel23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(_strConString);
        string CompanyFaxno = string.Empty;
        DataTable DtAddress = objAddMaster.GetAddressDataByTransId(xrLabel23.Text, "0");
        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            {
                CompanyFaxno = DtAddress.Rows[0]["FaxNo"].ToString();
            }
        }
        xrLabel23.Text = CompanyFaxno;

    }

    //private void xrTableCell13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    //{
    //    try
    //    {
    //        Ems_ContactMaster objcontact = new Ems_ContactMaster();

    //        DataTable dt = objcontact.GetContactTrueById(xrTableCell13.Text.Trim());
    //        string s = string.Empty;

    //        if (dt.Rows.Count != 0)
    //        {

    //            s = dt.Rows[0]["Name"].ToString();
    //        }
    //        xrTableCell13.Text = s;
    //    }
    //    catch
    //    {



    //    }
    //}

    //private void xrTableCell10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    //{
    //    try
    //    {
    //        Ems_ContactMaster objcontact = new Ems_ContactMaster();

    //        DataTable dt = objcontact.GetContactTrueById(xrTableCell10.Text.Trim());
    //        string s = string.Empty;

    //        if (dt.Rows.Count != 0)
    //        {

    //            s = "Mob.No :-" + dt.Rows[0]["Field2"].ToString();
    //        }
    //        xrTableCell10.Text = s;
    //    }
    //    catch
    //    {



    //    }

    //}

    //private void xrTableCell14_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    //{
    //    try
    //    {
    //        Ems_ContactMaster objcontact = new Ems_ContactMaster();

    //        DataTable dt = objcontact.GetContactTrueById(xrTableCell14.Text.Trim());
    //        string s = string.Empty;

    //        if (dt.Rows.Count != 0)
    //        {

    //            s = "Email Id :-" + dt.Rows[0]["Field1"].ToString();
    //        }
    //        xrTableCell14.Text = s;
    //    }
    //    catch
    //    {



    //    }
    //}

















}
