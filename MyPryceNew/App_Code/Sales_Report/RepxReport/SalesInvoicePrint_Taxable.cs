using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Drawing.Printing;
using PegasusDataAccess;

/// <summary>
/// Summary description for PobySupplier
/// </summary>
public class SalesInvoicePrint_Taxable : DevExpress.XtraReports.UI.XtraReport
{
    Inv_ShipExpDetail objshipdetail = null;
    Inv_ParameterMaster objParam = null;
    SystemParameter objsys = null;
    Set_AddressMaster Objaddress = null;
    LocationMaster ObjLocationMaster = null;
    CurrencyMaster objCurrency = null;
    Inv_TaxRefDetail objTaxRefDetail = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    DataAccessClass objDa = null;
    private string _strConString = string.Empty;
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    //private InventoryDataSet inventoryDataSet1;
    //private SalesDataSet SalesDataSet1;
    private PageFooterBand PageFooter;
    private XRPageInfo xrPageInfo2;
    private ReportHeaderBand ReportHeader;
    //private PurchaseDataSet purchaseDataSet1;
    private GroupHeaderBand GroupHeader1;
    private XRTable xrTable6;
    private XRTableRow xrTableRow8;
    private XRTableCell xrCellProductCode;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell61;
    private GroupFooterBand GroupFooter1;
    private XRTableCell xrTableCell66;
    private XRPanel xrPanel1;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel2;
    private XRLabel xrLabel16;
    private XRLabel xrLabel18;
    private XRLabel xrLabel19;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell4;
    private XRLabel xrLabel8;
    private XRRichText xrRichText1;
    private XRPanel xrPanel3;
    private XRLabel xrLabel15;
    private XRLabel xrLabel42;
    private XRLabel xrLabel41;
    private XRLabel xrLabel44;
    private XRLabel xrLabel43;
    private XRLabel xrLabel30;
    private XRLabel xrLabel31;
    private XRLabel xrLabel32;
    private XRLabel xrLabel14;
    private XRLabel xrLabel12;
    private XRLabel xrLabel29;
    private ReportFooterBand ReportFooter;
    private XRRichText xrRichText2;
    private XRLabel xrLabel45;
    private XRLabel xrLblCmpGstNo;
    private GroupFooterBand GroupFooter2;
    private XRTableCell xrCellDiscount;
    private XRTableCell xrCellTaxType;
    private XRTableCell xrCellTax;
    private XRLabel xrLabel6;
    private XRLabel xrLblCmpSign;
    private XRPanel xrPanelH;
    private XRTable xrTable2;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell26;
    private XRCrossBandLine xrCrossBandLine4;
    private XRTable xrTableExpenses;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCellExpensesName;
    private XRTableCell xrTableCellExpensesAmount;
    private XRTableCell xrTableCellExpensesDiscount;
    private XRTableCell xrTableCellTaxType;
    private XRTableCell xrTableCellExpensesTaxAmount;
    private XRTableCell xrTableCellExpensesTotalAmount;
    private XRTable xrTableTax;
    private XRTableRow xrTblTaxRow;
    private XRTableCell xrTblCellTaxType;
    private XRTableCell xrTblCellTaxPer;
    private XRTableCell xrTblCellTaxableAmount;
    private XRTableCell xrTblCellTaxAmount;
    private XRLabel xrLabel4;
    private XRLabel xrLblAmountInWords;
    private XRLabel xrLabel5;
    private XRLabel xrLabel10;
    private XRLabel xrLblTotalTax;
    private XRLabel xrLabel11;
    private XRTableCell xrCellAmount;
    private XRPanel xrPanel4;
    private XRLabel xrLabel35;
    private XRLabel xrLabel36;
    private XRLabel xrLabel34;
    private XRLabel xrLabel37;
    private XRLabel xrLabel40;
    private XRLabel xrLabel39;
    private XRLabel xrLabel38;
    private XRPanel xrPanel2;
    private XRLabel xrLabel27;
    private XRLabel xrLabel3;
    private XRLabel xrLabel25;
    private XRLabel xrLabel33;
    private XRLabel xrLabel24;
    private XRLabel xrLabel22;
    private XRLabel xrLabel23;
    private XRLabel xrLabel26;
    private XRLabel xrLabel17;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public SalesInvoicePrint_Taxable(string strConString)
    {
        InitializeComponent();
        objshipdetail = new Inv_ShipExpDetail(strConString);
        objParam = new Inv_ParameterMaster(strConString);
        objsys = new SystemParameter(strConString);
        Objaddress = new Set_AddressMaster(strConString);
        ObjLocationMaster = new LocationMaster(strConString);
        objCurrency = new CurrencyMaster(strConString);
        objTaxRefDetail = new Inv_TaxRefDetail(strConString);
        ObjStockBatchMaster = new Inv_StockBatchMaster(strConString);
        objDa = new DataAccessClass(strConString);
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
    public void settitle(string TitleName)
    {
        xrLabel16.Text = TitleName;
    }

    public void setcompanyname(string companyname)
    {
        xrLabel2.Text = companyname;

    }
    public void SetImage(string Url)
    {
        xrPictureBox1.ImageUrl = Url;
    }
    public void setCompanyArebicName(string ArebicName)
    {


        // xrPictureBox3.ImageUrl = "~/Images/Arabic.png";

    }
    public void setcompanyAddress(string Address)
    {
        xrLabel15.Text = Address;
    }

    public void setCompanyTelNo(string TelNo)
    {
        xrLabel12.Text = TelNo;
    }
    public void setCompanyFaxNo(string FaxNo)
    {
        xrLabel29.Text = FaxNo;
    }
    public void setCompanyWebsite(string WebSite)
    {
        xrLabel32.Text = WebSite;
    }
    public void setCompanyGSTIN(string GSTIN)
    {
        xrLblCmpGstNo.Text = GSTIN;
    }
    public void setCustomerGSTIN(string GSTIN)
    {
        xrLabel3.Text = GSTIN;
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            string resourceFileName = "SalesInvoicePrint_Taxable.resx";
            System.Resources.ResourceManager resources = global::Resources.SalesInvoicePrint_Taxable.ResourceManager;
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCellProductCode = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCellAmount = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCellDiscount = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCellTaxType = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCellTax = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblCmpSign = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblCmpGstNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPanelH = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrTableExpenses = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCellExpensesName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellExpensesAmount = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellExpensesDiscount = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellTaxType = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellExpensesTaxAmount = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCellExpensesTotalAmount = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrRichText2 = new DevExpress.XtraReports.UI.XRRichText();
            this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrTableTax = new DevExpress.XtraReports.UI.XRTable();
            this.xrTblTaxRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTblCellTaxType = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTblCellTaxPer = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTblCellTaxableAmount = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTblCellTaxAmount = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblAmountInWords = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblTotalTax = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableExpenses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
            this.Detail.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.Detail.HeightF = 18.75F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseFont = false;
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrTable6
            // 
            this.xrTable6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.xrTable6.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable6.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(0.4994546F, 0F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 3, 0, 100F);
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8});
            this.xrTable6.SizeF = new System.Drawing.SizeF(816.525F, 18.75F);
            this.xrTable6.StylePriority.UseBorderColor = false;
            this.xrTable6.StylePriority.UseBorders = false;
            this.xrTable6.StylePriority.UseFont = false;
            this.xrTable6.StylePriority.UsePadding = false;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell4,
            this.xrCellProductCode,
            this.xrTableCell57,
            this.xrTableCell59,
            this.xrTableCell61,
            this.xrCellAmount,
            this.xrCellDiscount,
            this.xrCellTaxType,
            this.xrCellTax,
            this.xrTableCell66});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProductSerialNumber]")});
            this.xrTableCell2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBorderColor = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.26805550193868055D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProductRefNo]")});
            this.xrTableCell4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBorderColor = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.46065021963704617D;
            // 
            // xrCellProductCode
            // 
            this.xrCellProductCode.BorderColor = System.Drawing.Color.Black;
            this.xrCellProductCode.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProductCode]")});
            this.xrCellProductCode.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCellProductCode.Multiline = true;
            this.xrCellProductCode.Name = "xrCellProductCode";
            this.xrCellProductCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 3, 0, 100F);
            this.xrCellProductCode.StylePriority.UseBorderColor = false;
            this.xrCellProductCode.StylePriority.UseFont = false;
            this.xrCellProductCode.StylePriority.UsePadding = false;
            this.xrCellProductCode.StylePriority.UseTextAlignment = false;
            this.xrCellProductCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrCellProductCode.Weight = 1.5392012153079098D;
            this.xrCellProductCode.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrCellProductCode_BeforePrint);
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell57.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[UnitName]")});
            this.xrTableCell57.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.StylePriority.UseBorderColor = false;
            this.xrTableCell57.StylePriority.UseFont = false;
            this.xrTableCell57.StylePriority.UseTextAlignment = false;
            this.xrTableCell57.Text = "[Unit_Name]";
            this.xrTableCell57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell57.Weight = 0.29094984096137827D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell59.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Quantity]")});
            this.xrTableCell59.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.StylePriority.UseBorderColor = false;
            this.xrTableCell59.StylePriority.UseFont = false;
            this.xrTableCell59.StylePriority.UseTextAlignment = false;
            this.xrTableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell59.Weight = 0.27344696187267631D;
            this.xrTableCell59.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell59_BeforePrint);
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell61.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[UnitPrice]")});
            this.xrTableCell61.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.StylePriority.UseBorderColor = false;
            this.xrTableCell61.StylePriority.UseFont = false;
            this.xrTableCell61.StylePriority.UseTextAlignment = false;
            this.xrTableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell61.Weight = 0.39133097308005926D;
            this.xrTableCell61.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell61_BeforePrint);
            // 
            // xrCellAmount
            // 
            this.xrCellAmount.BorderColor = System.Drawing.Color.Black;
            this.xrCellAmount.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ItemUnitPriceTotal]")});
            this.xrCellAmount.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCellAmount.Name = "xrCellAmount";
            this.xrCellAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 3, 0, 100F);
            this.xrCellAmount.StylePriority.UseBorderColor = false;
            this.xrCellAmount.StylePriority.UseFont = false;
            this.xrCellAmount.StylePriority.UsePadding = false;
            this.xrCellAmount.StylePriority.UseTextAlignment = false;
            this.xrCellAmount.Text = "Amount";
            this.xrCellAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrCellAmount.Weight = 0.418776036768223D;
            this.xrCellAmount.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrCellAmount_BeforePrint);
            // 
            // xrCellDiscount
            // 
            this.xrCellDiscount.BorderColor = System.Drawing.Color.Black;
            this.xrCellDiscount.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DiscountV]")});
            this.xrCellDiscount.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCellDiscount.Name = "xrCellDiscount";
            this.xrCellDiscount.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 3, 0, 100F);
            this.xrCellDiscount.StylePriority.UseBorderColor = false;
            this.xrCellDiscount.StylePriority.UseFont = false;
            this.xrCellDiscount.StylePriority.UsePadding = false;
            this.xrCellDiscount.StylePriority.UseTextAlignment = false;
            this.xrCellDiscount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrCellDiscount.Weight = 0.37051295733177458D;
            this.xrCellDiscount.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrCellDiscount_BeforePrint);
            // 
            // xrCellTaxType
            // 
            this.xrCellTaxType.BorderColor = System.Drawing.Color.Black;
            this.xrCellTaxType.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCellTaxType.Multiline = true;
            this.xrCellTaxType.Name = "xrCellTaxType";
            this.xrCellTaxType.StylePriority.UseBorderColor = false;
            this.xrCellTaxType.StylePriority.UseFont = false;
            this.xrCellTaxType.StylePriority.UseTextAlignment = false;
            this.xrCellTaxType.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrCellTaxType.Weight = 0.56902331495760516D;
            this.xrCellTaxType.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrCellTaxType_BeforePrint);
            // 
            // xrCellTax
            // 
            this.xrCellTax.BorderColor = System.Drawing.Color.Black;
            this.xrCellTax.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrCellTax.Name = "xrCellTax";
            this.xrCellTax.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 3, 0, 100F);
            this.xrCellTax.StylePriority.UseBorderColor = false;
            this.xrCellTax.StylePriority.UseFont = false;
            this.xrCellTax.StylePriority.UsePadding = false;
            this.xrCellTax.StylePriority.UseTextAlignment = false;
            this.xrCellTax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrCellTax.Weight = 0.48271768165695117D;
            this.xrCellTax.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrCellTax_BeforePrint);
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell66.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ProductTotalAmount]")});
            this.xrTableCell66.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 3, 0, 100F);
            this.xrTableCell66.StylePriority.UseBorderColor = false;
            this.xrTableCell66.StylePriority.UseFont = false;
            this.xrTableCell66.StylePriority.UsePadding = false;
            this.xrTableCell66.StylePriority.UseTextAlignment = false;
            this.xrTableCell66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell66.Weight = 0.57118338350206743D;
            this.xrTableCell66.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell66_BeforePrint);
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
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2});
            this.BottomMargin.HeightF = 18.12496F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Font = new System.Drawing.Font("Calibri", 6.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(762.8632F, 0F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(53.13672F, 18.12496F);
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrPageInfo2.TextFormatString = "Page{0}Of {1}";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLblCmpSign,
            this.xrRichText1,
            this.xrLabel8});
            this.PageFooter.HeightF = 121.0835F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel6
            // 
            this.xrLabel6.BorderColor = System.Drawing.Color.Black;
            this.xrLabel6.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(557.038F, 66.70835F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(258.9618F, 19.70834F);
            this.xrLabel6.StylePriority.UseBorderColor = false;
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Authorised Signatory";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLblCmpSign
            // 
            this.xrLblCmpSign.BorderColor = System.Drawing.Color.Black;
            this.xrLblCmpSign.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblCmpSign.LocationFloat = new DevExpress.Utils.PointFloat(557.038F, 9.999977F);
            this.xrLblCmpSign.Name = "xrLblCmpSign";
            this.xrLblCmpSign.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblCmpSign.SizeF = new System.Drawing.SizeF(258.9619F, 19.70834F);
            this.xrLblCmpSign.StylePriority.UseBorderColor = false;
            this.xrLblCmpSign.StylePriority.UseBorders = false;
            this.xrLblCmpSign.StylePriority.UseFont = false;
            this.xrLblCmpSign.StylePriority.UseTextAlignment = false;
            this.xrLblCmpSign.Text = "For Pegasus Trading Company";
            this.xrLblCmpSign.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLblCmpSign.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLblCmpSign_BeforePrint);
            // 
            // xrRichText1
            // 
            this.xrRichText1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Html", "[Remark]")});
            this.xrRichText1.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(28.43285F, 98.08346F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(539.3646F, 18.12495F);
            this.xrRichText1.StylePriority.UseFont = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(1.499454F, 98.08346F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(25.43396F, 18.12495F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "**";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
            this.ReportHeader.HeightF = 129.1667F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrPanel1
            // 
            this.xrPanel1.BorderColor = System.Drawing.Color.Black;
            this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPanel1.BorderWidth = 2F;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel45,
            this.xrLblCmpGstNo,
            this.xrLabel30,
            this.xrLabel31,
            this.xrLabel32,
            this.xrLabel14,
            this.xrLabel12,
            this.xrLabel29,
            this.xrPictureBox1,
            this.xrLabel2,
            this.xrLabel15});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(1.499451F, 27F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(814.5007F, 102.1667F);
            this.xrPanel1.StylePriority.UseBorderColor = false;
            this.xrPanel1.StylePriority.UseBorders = false;
            this.xrPanel1.StylePriority.UseBorderWidth = false;
            // 
            // xrLabel45
            // 
            this.xrLabel45.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(575.0026F, 81.83338F);
            this.xrLabel45.Name = "xrLabel45";
            this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel45.SizeF = new System.Drawing.SizeF(65.66211F, 18.12497F);
            this.xrLabel45.StylePriority.UseFont = false;
            this.xrLabel45.StylePriority.UseTextAlignment = false;
            this.xrLabel45.Text = "GSTIN No :";
            this.xrLabel45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLblCmpGstNo
            // 
            this.xrLblCmpGstNo.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblCmpGstNo.LocationFloat = new DevExpress.Utils.PointFloat(642.6646F, 81.83337F);
            this.xrLblCmpGstNo.Name = "xrLblCmpGstNo";
            this.xrLblCmpGstNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblCmpGstNo.SizeF = new System.Drawing.SizeF(167.567F, 18.12495F);
            this.xrLblCmpGstNo.StylePriority.UseFont = false;
            this.xrLblCmpGstNo.Text = "[Company GSTIN]";
            // 
            // xrLabel30
            // 
            this.xrLabel30.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel30.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(0F, 64.375F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(66.7812F, 17.7917F);
            this.xrLabel30.StylePriority.UseBorders = false;
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.Text = "Fax No.    : ";
            // 
            // xrLabel31
            // 
            this.xrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel31.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(0.05708265F, 82.16668F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(66.66714F, 17.79169F);
            this.xrLabel31.StylePriority.UseBorders = false;
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.Text = "Web Site   :";
            // 
            // xrLabel32
            // 
            this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel32.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(66.78117F, 82.16666F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(381.303F, 17.79169F);
            this.xrLabel32.StylePriority.UseBorders = false;
            this.xrLabel32.StylePriority.UseFont = false;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel14.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(0F, 46.58332F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(66.7812F, 17.79167F);
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.Text = "Tel No      :";
            // 
            // xrLabel12
            // 
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel12.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(66.78117F, 46.5833F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(381.3031F, 17.79167F);
            this.xrLabel12.StylePriority.UseBorders = false;
            this.xrLabel12.StylePriority.UseFont = false;
            // 
            // xrLabel29
            // 
            this.xrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel29.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(66.78117F, 64.37497F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(381.3031F, 17.7917F);
            this.xrLabel29.StylePriority.UseBorders = false;
            this.xrLabel29.StylePriority.UseFont = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox1.ImageUrl = "~\\Images\\PegasusLogo.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(635.4221F, 1.999996F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(174.8477F, 62.37497F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox1.StylePriority.UseBorders = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1.999998F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(566.298F, 21.95834F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "xrLabel1";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel15.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(0F, 23.96002F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(566.298F, 22.04F);
            this.xrLabel15.StylePriority.UseBorders = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanelH,
            this.xrLabel16,
            this.xrTable2,
            this.xrPanel3});
            this.GroupHeader1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Invoice_Id", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 226.3083F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.StylePriority.UseFont = false;
            this.GroupHeader1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader1_BeforePrint);
            // 
            // xrPanelH
            // 
            this.xrPanelH.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrPanelH.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel4,
            this.xrPanel2,
            this.xrLabel19,
            this.xrLabel20,
            this.xrLabel21,
            this.xrLabel18,
            this.xrLabel41,
            this.xrLabel42,
            this.xrLabel43,
            this.xrLabel44});
            this.xrPanelH.LocationFloat = new DevExpress.Utils.PointFloat(0.4994539F, 39.91091F);
            this.xrPanelH.Name = "xrPanelH";
            this.xrPanelH.SizeF = new System.Drawing.SizeF(816.52F, 156.15F);
            this.xrPanelH.StylePriority.UseBorders = false;
            this.xrPanelH.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrPanelH_PrintOnPage);
            // 
            // xrPanel4
            // 
            this.xrPanel4.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel35,
            this.xrLabel36,
            this.xrLabel34,
            this.xrLabel37,
            this.xrLabel40,
            this.xrLabel39,
            this.xrLabel38});
            this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(275.6F, 1.1F);
            this.xrPanel4.Name = "xrPanel4";
            this.xrPanel4.SizeF = new System.Drawing.SizeF(252.02F, 155.04F);
            this.xrPanel4.StylePriority.UseBorders = false;
            // 
            // xrLabel35
            // 
            this.xrLabel35.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel35.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShipAddressName]")});
            this.xrLabel35.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(1.468445F, 36.26334F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(249.2369F, 17.79164F);
            this.xrLabel35.StylePriority.UseBorders = false;
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.Text = "xrLabel23";
            // 
            // xrLabel36
            // 
            this.xrLabel36.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel36.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShipAddress]")});
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(1.468445F, 54.05497F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(249.2369F, 49.99998F);
            this.xrLabel36.StylePriority.UseBorders = false;
            this.xrLabel36.Text = "0";
            this.xrLabel36.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel36_BeforePrint);
            // 
            // xrLabel34
            // 
            this.xrLabel34.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel34.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(1.464661F, 1.104996F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(249.2369F, 35.15833F);
            this.xrLabel34.StylePriority.UseBorders = false;
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.Text = "Ship To";
            // 
            // xrLabel37
            // 
            this.xrLabel37.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel37.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShipAddress]")});
            this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(66.24924F, 122.055F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(184.4561F, 31.88F);
            this.xrLabel37.StylePriority.UseBorders = false;
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.Text = "0";
            this.xrLabel37.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel37_BeforePrint);
            // 
            // xrLabel40
            // 
            this.xrLabel40.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel40.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ShipAddress]")});
            this.xrLabel40.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(67.24924F, 104.055F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(183.6F, 18F);
            this.xrLabel40.StylePriority.UseBorders = false;
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.Text = "0";
            this.xrLabel40.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel40_BeforePrint);
            // 
            // xrLabel39
            // 
            this.xrLabel39.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(2F, 104.055F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(64.7807F, 17.79169F);
            this.xrLabel39.StylePriority.UseBorders = false;
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.Text = "Tel No      :";
            // 
            // xrLabel38
            // 
            this.xrLabel38.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(1.468445F, 122.055F);
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(64.7807F, 17.79164F);
            this.xrLabel38.StylePriority.UseBorders = false;
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.Text = "Fax No.    : ";
            // 
            // xrPanel2
            // 
            this.xrPanel2.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel27,
            this.xrLabel3,
            this.xrLabel25,
            this.xrLabel33,
            this.xrLabel24,
            this.xrLabel22,
            this.xrLabel23,
            this.xrLabel26,
            this.xrLabel17});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(1.099998F, 1.099998F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(252.02F, 155.04F);
            this.xrPanel2.StylePriority.UseBorders = false;
            // 
            // xrLabel27
            // 
            this.xrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel27.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CustomerAddress]")});
            this.xrLabel27.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(77.52567F, 104.9492F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(172.5F, 17.79F);
            this.xrLabel27.StylePriority.UseBorders = false;
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.Text = "0";
            this.xrLabel27.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel27_BeforePrint);
            // 
            // xrLabel3
            // 
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Cust_TAX_No]")});
            this.xrLabel3.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(77.52653F, 139.7492F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(172.5F, 15.08F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "[Customer GSTIN]";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(1.206528F, 122.7492F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(76.31435F, 17F);
            this.xrLabel25.StylePriority.UseBorders = false;
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.Text = "Fax No.    : ";
            // 
            // xrLabel33
            // 
            this.xrLabel33.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(1.21132F, 104.9492F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(76.31437F, 17.79169F);
            this.xrLabel33.StylePriority.UseBorders = false;
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.Text = "Tel No      :";
            // 
            // xrLabel24
            // 
            this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel24.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CustomerAddress]")});
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(1.211315F, 54.94915F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(249.5F, 50F);
            this.xrLabel24.StylePriority.UseBorders = false;
            this.xrLabel24.Text = "0";
            this.xrLabel24.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel24_BeforePrint);
            // 
            // xrLabel22
            // 
            this.xrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel22.Font = new System.Drawing.Font("Calibri", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(1.099998F, 1.099998F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(249.5F, 35.16F);
            this.xrLabel22.StylePriority.UseBorders = false;
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.Text = "Sold To";
            // 
            // xrLabel23
            // 
            this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Supplier_Name]")});
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(1.211319F, 37.1575F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(249.5F, 17.79F);
            this.xrLabel23.StylePriority.UseBorders = false;
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.Text = "xrLabel23";
            // 
            // xrLabel26
            // 
            this.xrLabel26.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CustomerAddress]")});
            this.xrLabel26.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(77.52653F, 122.7492F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(172.5F, 17F);
            this.xrLabel26.StylePriority.UseBorders = false;
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.Text = "0";
            this.xrLabel26.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel26_BeforePrint);
            // 
            // xrLabel17
            // 
            this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel17.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(1.206528F, 139.7492F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(76.31435F, 15.08002F);
            this.xrLabel17.StylePriority.UseBorders = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "GSTIN No   :";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel19
            // 
            this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel19.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Invoice_No]")});
            this.xrLabel19.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(661.2889F, 11.04167F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(150.9239F, 17.79F);
            this.xrLabel19.StylePriority.UseBorders = false;
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.Text = "xrLabel19";
            // 
            // xrLabel20
            // 
            this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel20.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(562.2888F, 32.20835F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(79.99994F, 17.79166F);
            this.xrLabel20.StylePriority.UseBorders = false;
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Date             :";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel21
            // 
            this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Invoice_Date]")});
            this.xrLabel21.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(661.2889F, 32.20835F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(150.9238F, 17.79167F);
            this.xrLabel21.StylePriority.UseBorders = false;
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "xrLabel19";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel21.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel21_BeforePrint);
            // 
            // xrLabel18
            // 
            this.xrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel18.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(562.2888F, 11.04167F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(80F, 17.79F);
            this.xrLabel18.StylePriority.UseBorders = false;
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.Text = "Invoice No. :";
            // 
            // xrLabel41
            // 
            this.xrLabel41.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel41.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(562.2888F, 52.20826F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(80F, 17.79163F);
            this.xrLabel41.StylePriority.UseBorders = false;
            this.xrLabel41.StylePriority.UseFont = false;
            this.xrLabel41.StylePriority.UseTextAlignment = false;
            this.xrLabel41.Text = "Ref. No.       :";
            this.xrLabel41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel42
            // 
            this.xrLabel42.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel42.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Invoice_Ref_No]")});
            this.xrLabel42.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(661.2889F, 52.20826F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(150.9238F, 17.79163F);
            this.xrLabel42.StylePriority.UseBorders = false;
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.StylePriority.UseTextAlignment = false;
            this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel43
            // 
            this.xrLabel43.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel43.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(561.2509F, 73.08337F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(80F, 17.79163F);
            this.xrLabel43.StylePriority.UseBorders = false;
            this.xrLabel43.StylePriority.UseFont = false;
            this.xrLabel43.StylePriority.UseTextAlignment = false;
            this.xrLabel43.Text = "Merchant    :";
            this.xrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel44
            // 
            this.xrLabel44.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel44.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[InvoiceMerchant]")});
            this.xrLabel44.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(660.2508F, 73.08337F);
            this.xrLabel44.Name = "xrLabel44";
            this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel44.SizeF = new System.Drawing.SizeF(150.9238F, 17.79163F);
            this.xrLabel44.StylePriority.UseBorders = false;
            this.xrLabel44.StylePriority.UseFont = false;
            this.xrLabel44.StylePriority.UseTextAlignment = false;
            this.xrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel16.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(0.5920706F, 12.00001F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(815.4081F, 24.04165F);
            this.xrLabel16.StylePriority.UseBorders = false;
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "xrLabel3";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0.5F, 196.1F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable2.SizeF = new System.Drawing.SizeF(816.525F, 30.20833F);
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell3,
            this.xrTableCell14,
            this.xrTableCell18,
            this.xrTableCell22,
            this.xrTableCell24,
            this.xrTableCell6,
            this.xrTableCell5,
            this.xrTableCell12,
            this.xrTableCell8,
            this.xrTableCell26});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorderColor = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "S. No.";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.29022087431605792D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorderColor = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "So No#";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 0.49874126223788995D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell14.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell14.StylePriority.UseBorderColor = false;
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UsePadding = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "Product";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell14.Weight = 1.6664770958682427D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell18.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell18.Multiline = true;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseBorderColor = false;
            this.xrTableCell18.StylePriority.UseBorders = false;
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "Unit \r\n";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell18.Weight = 0.31500861099525546D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell22.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell22.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseBorderColor = false;
            this.xrTableCell22.StylePriority.UseBorders = false;
            this.xrTableCell22.StylePriority.UseFont = false;
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "Qty";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell22.Weight = 0.29605774072031621D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell24.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell24.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseBorderColor = false;
            this.xrTableCell24.StylePriority.UseBorders = false;
            this.xrTableCell24.StylePriority.UseFont = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "Rate";
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell24.Weight = 0.42369018694456095D;
            this.xrTableCell24.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell24_BeforePrint);
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseBorderColor = false;
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Amount";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell6.Weight = 0.45340466613093627D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell5.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell5.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseBackColor = false;
            this.xrTableCell5.StylePriority.UseBorderColor = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Discount";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell5.Weight = 0.4022249904729549D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell12.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseBorderColor = false;
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "Tax Type";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell12.Weight = 0.61569702867323428D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell8.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseBorderColor = false;
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "Tax Amount";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell8.Weight = 0.52231265318430342D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell26.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseBorderColor = false;
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = "Total Amount";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell26.Weight = 0.61803909151703917D;
            this.xrTableCell26.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell26_BeforePrint);
            // 
            // xrPanel3
            // 
            this.xrPanel3.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrPanel3.BorderWidth = 2F;
            this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(0.5920728F, 0F);
            this.xrPanel3.Name = "xrPanel3";
            this.xrPanel3.SizeF = new System.Drawing.SizeF(815.408F, 2.333339F);
            this.xrPanel3.StylePriority.UseBorders = false;
            this.xrPanel3.StylePriority.UseBorderWidth = false;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableExpenses});
            this.GroupFooter1.HeightF = 2.083333F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupFooter1_BeforePrint);
            // 
            // xrTableExpenses
            // 
            this.xrTableExpenses.BorderColor = System.Drawing.Color.White;
            this.xrTableExpenses.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableExpenses.BorderWidth = 1F;
            this.xrTableExpenses.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold);
            this.xrTableExpenses.KeepTogether = true;
            this.xrTableExpenses.LocationFloat = new DevExpress.Utils.PointFloat(0.4994551F, 0F);
            this.xrTableExpenses.Name = "xrTableExpenses";
            this.xrTableExpenses.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTableExpenses.SizeF = new System.Drawing.SizeF(816.53F, 2.083333F);
            this.xrTableExpenses.StylePriority.UseBorderColor = false;
            this.xrTableExpenses.StylePriority.UseBorders = false;
            this.xrTableExpenses.StylePriority.UseBorderWidth = false;
            this.xrTableExpenses.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCellExpensesName,
            this.xrTableCellExpensesAmount,
            this.xrTableCellExpensesDiscount,
            this.xrTableCellTaxType,
            this.xrTableCellExpensesTaxAmount,
            this.xrTableCellExpensesTotalAmount});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCellExpensesName
            // 
            this.xrTableCellExpensesName.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCellExpensesName.Font = new System.Drawing.Font("Calibri", 9F);
            this.xrTableCellExpensesName.Name = "xrTableCellExpensesName";
            this.xrTableCellExpensesName.StylePriority.UseBorders = false;
            this.xrTableCellExpensesName.StylePriority.UseFont = false;
            this.xrTableCellExpensesName.StylePriority.UseTextAlignment = false;
            this.xrTableCellExpensesName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCellExpensesName.Weight = 2.8323581065167796D;
            // 
            // xrTableCellExpensesAmount
            // 
            this.xrTableCellExpensesAmount.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCellExpensesAmount.Font = new System.Drawing.Font("Calibri", 9F);
            this.xrTableCellExpensesAmount.Name = "xrTableCellExpensesAmount";
            this.xrTableCellExpensesAmount.StylePriority.UseBorders = false;
            this.xrTableCellExpensesAmount.StylePriority.UseFont = false;
            this.xrTableCellExpensesAmount.StylePriority.UseTextAlignment = false;
            this.xrTableCellExpensesAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCellExpensesAmount.Weight = 0.3679461876069301D;
            // 
            // xrTableCellExpensesDiscount
            // 
            this.xrTableCellExpensesDiscount.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCellExpensesDiscount.Font = new System.Drawing.Font("Calibri", 9F);
            this.xrTableCellExpensesDiscount.Name = "xrTableCellExpensesDiscount";
            this.xrTableCellExpensesDiscount.StylePriority.UseBorders = false;
            this.xrTableCellExpensesDiscount.StylePriority.UseFont = false;
            this.xrTableCellExpensesDiscount.Weight = 0.32641353896991343D;
            // 
            // xrTableCellTaxType
            // 
            this.xrTableCellTaxType.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCellTaxType.Font = new System.Drawing.Font("Calibri", 9F);
            this.xrTableCellTaxType.Name = "xrTableCellTaxType";
            this.xrTableCellTaxType.StylePriority.UseBorders = false;
            this.xrTableCellTaxType.StylePriority.UseFont = false;
            this.xrTableCellTaxType.Weight = 0.49964985951990304D;
            this.xrTableCellTaxType.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCellTaxType_BeforePrint);
            // 
            // xrTableCellExpensesTaxAmount
            // 
            this.xrTableCellExpensesTaxAmount.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCellExpensesTaxAmount.Font = new System.Drawing.Font("Calibri", 9F);
            this.xrTableCellExpensesTaxAmount.Name = "xrTableCellExpensesTaxAmount";
            this.xrTableCellExpensesTaxAmount.StylePriority.UseBorders = false;
            this.xrTableCellExpensesTaxAmount.StylePriority.UseFont = false;
            this.xrTableCellExpensesTaxAmount.StylePriority.UseTextAlignment = false;
            this.xrTableCellExpensesTaxAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCellExpensesTaxAmount.Weight = 0.42356094059160032D;
            // 
            // xrTableCellExpensesTotalAmount
            // 
            this.xrTableCellExpensesTotalAmount.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCellExpensesTotalAmount.Font = new System.Drawing.Font("Calibri", 9F);
            this.xrTableCellExpensesTotalAmount.Name = "xrTableCellExpensesTotalAmount";
            this.xrTableCellExpensesTotalAmount.StylePriority.UseBorders = false;
            this.xrTableCellExpensesTotalAmount.StylePriority.UseFont = false;
            this.xrTableCellExpensesTotalAmount.StylePriority.UseTextAlignment = false;
            this.xrTableCellExpensesTotalAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCellExpensesTotalAmount.Weight = 0.50185534041316648D;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText2});
            this.ReportFooter.HeightF = 77.16667F;
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.PrintAtBottom = true;
            // 
            // xrRichText2
            // 
            this.xrRichText2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Rtf", "[condition1]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Html", "[condition1]")});
            this.xrRichText2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrRichText2.LocationFloat = new DevExpress.Utils.PointFloat(1.499452F, 0F);
            this.xrRichText2.Name = "xrRichText2";
            this.xrRichText2.SerializableRtfString = resources.GetString("xrRichText2.SerializableRtfString");
            this.xrRichText2.SizeF = new System.Drawing.SizeF(814.5007F, 77.16666F);
            this.xrRichText2.StylePriority.UseFont = false;
            // 
            // GroupFooter2
            // 
            this.GroupFooter2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableTax,
            this.xrLabel4,
            this.xrLblAmountInWords,
            this.xrLabel5,
            this.xrLabel10,
            this.xrLblTotalTax,
            this.xrLabel11});
            this.GroupFooter2.HeightF = 143.899F;
            this.GroupFooter2.Level = 1;
            this.GroupFooter2.Name = "GroupFooter2";
            this.GroupFooter2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupFooter2_BeforePrint);
            // 
            // xrTableTax
            // 
            this.xrTableTax.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableTax.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableTax.KeepTogether = true;
            this.xrTableTax.LocationFloat = new DevExpress.Utils.PointFloat(3.615386F, 120.5828F);
            this.xrTableTax.Name = "xrTableTax";
            this.xrTableTax.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTblTaxRow});
            this.xrTableTax.SizeF = new System.Drawing.SizeF(306.9299F, 16F);
            this.xrTableTax.StylePriority.UseBorders = false;
            this.xrTableTax.StylePriority.UseFont = false;
            // 
            // xrTblTaxRow
            // 
            this.xrTblTaxRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTblCellTaxType,
            this.xrTblCellTaxPer,
            this.xrTblCellTaxableAmount,
            this.xrTblCellTaxAmount});
            this.xrTblTaxRow.Name = "xrTblTaxRow";
            this.xrTblTaxRow.Weight = 1D;
            // 
            // xrTblCellTaxType
            // 
            this.xrTblCellTaxType.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTblCellTaxType.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTblCellTaxType.Name = "xrTblCellTaxType";
            this.xrTblCellTaxType.StylePriority.UseBorders = false;
            this.xrTblCellTaxType.StylePriority.UseFont = false;
            this.xrTblCellTaxType.StylePriority.UseTextAlignment = false;
            this.xrTblCellTaxType.Text = "Tax Type";
            this.xrTblCellTaxType.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTblCellTaxType.Weight = 0.660974691585507D;
            // 
            // xrTblCellTaxPer
            // 
            this.xrTblCellTaxPer.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTblCellTaxPer.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTblCellTaxPer.Name = "xrTblCellTaxPer";
            this.xrTblCellTaxPer.StylePriority.UseBorders = false;
            this.xrTblCellTaxPer.StylePriority.UseFont = false;
            this.xrTblCellTaxPer.StylePriority.UseTextAlignment = false;
            this.xrTblCellTaxPer.Text = "Tax Rate(%)";
            this.xrTblCellTaxPer.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTblCellTaxPer.Weight = 0.5650782085098851D;
            // 
            // xrTblCellTaxableAmount
            // 
            this.xrTblCellTaxableAmount.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTblCellTaxableAmount.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTblCellTaxableAmount.Name = "xrTblCellTaxableAmount";
            this.xrTblCellTaxableAmount.StylePriority.UseBorders = false;
            this.xrTblCellTaxableAmount.StylePriority.UseFont = false;
            this.xrTblCellTaxableAmount.StylePriority.UseTextAlignment = false;
            this.xrTblCellTaxableAmount.Text = "Taxable Amount";
            this.xrTblCellTaxableAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTblCellTaxableAmount.Weight = 0.69209069367884579D;
            // 
            // xrTblCellTaxAmount
            // 
            this.xrTblCellTaxAmount.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTblCellTaxAmount.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTblCellTaxAmount.Name = "xrTblCellTaxAmount";
            this.xrTblCellTaxAmount.StylePriority.UseBorders = false;
            this.xrTblCellTaxAmount.StylePriority.UseFont = false;
            this.xrTblCellTaxAmount.StylePriority.UseTextAlignment = false;
            this.xrTblCellTaxAmount.Text = "Tax Amount";
            this.xrTblCellTaxAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTblCellTaxAmount.Weight = 0.5657397326456D;
            // 
            // xrLabel4
            // 
            this.xrLabel4.BorderColor = System.Drawing.Color.Black;
            this.xrLabel4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(2.615387F, 52.54111F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(140.8112F, 19.70834F);
            this.xrLabel4.StylePriority.UseBorderColor = false;
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Amount In Words :";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLblAmountInWords
            // 
            this.xrLblAmountInWords.BorderColor = System.Drawing.Color.Black;
            this.xrLblAmountInWords.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLblAmountInWords.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblAmountInWords.LocationFloat = new DevExpress.Utils.PointFloat(2.615387F, 72.33276F);
            this.xrLblAmountInWords.Name = "xrLblAmountInWords";
            this.xrLblAmountInWords.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblAmountInWords.SizeF = new System.Drawing.SizeF(812.7692F, 19.70834F);
            this.xrLblAmountInWords.StylePriority.UseBorderColor = false;
            this.xrLblAmountInWords.StylePriority.UseBorders = false;
            this.xrLblAmountInWords.StylePriority.UseFont = false;
            this.xrLblAmountInWords.StylePriority.UseTextAlignment = false;
            this.xrLblAmountInWords.Text = "Amount In Words";
            this.xrLblAmountInWords.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLblAmountInWords.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLblAmountInWords_BeforePrint);
            // 
            // xrLabel5
            // 
            this.xrLabel5.BorderColor = System.Drawing.Color.Black;
            this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel5.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(3.615386F, 100.8744F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(306.9299F, 19.70835F);
            this.xrLabel5.StylePriority.UseBorderColor = false;
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Tax Summary - ";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.BorderColor = System.Drawing.Color.Black;
            this.xrLabel10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel10.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(527.215F, 0F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(137.1196F, 19.70834F);
            this.xrLabel10.StylePriority.UseBorderColor = false;
            this.xrLabel10.StylePriority.UseBorders = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Total";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLblTotalTax
            // 
            this.xrLblTotalTax.BorderColor = System.Drawing.Color.Black;
            this.xrLblTotalTax.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLblTotalTax.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NetTaxV]")});
            this.xrLblTotalTax.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLblTotalTax.LocationFloat = new DevExpress.Utils.PointFloat(664.4279F, 0F);
            this.xrLblTotalTax.Name = "xrLblTotalTax";
            this.xrLblTotalTax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 5, 0, 0, 100F);
            this.xrLblTotalTax.SizeF = new System.Drawing.SizeF(69.84314F, 19.70834F);
            this.xrLblTotalTax.StylePriority.UseBorderColor = false;
            this.xrLblTotalTax.StylePriority.UseBorders = false;
            this.xrLblTotalTax.StylePriority.UseFont = false;
            this.xrLblTotalTax.StylePriority.UsePadding = false;
            this.xrLblTotalTax.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            this.xrLblTotalTax.Summary = xrSummary1;
            this.xrLblTotalTax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLblTotalTax.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLblTotalTax_BeforePrint_1);
            // 
            // xrLabel11
            // 
            this.xrLabel11.BorderColor = System.Drawing.Color.Black;
            this.xrLabel11.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[GrandTotal]")});
            this.xrLabel11.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(734.2711F, 0F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 5, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(82.75836F, 19.70834F);
            this.xrLabel11.StylePriority.UseBorderColor = false;
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UsePadding = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            this.xrLabel11.Summary = xrSummary2;
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel11.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel11_BeforePrint);
            // 
            // SalesInvoicePrint_Taxable
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.ReportHeader,
            this.GroupHeader1,
            this.GroupFooter1,
            this.ReportFooter,
            this.GroupFooter2});
            this.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
            this.Margins = new System.Drawing.Printing.Margins(3, 6, 0, 18);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Version = "18.1";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel36_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableExpenses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }


    #endregion



    double TotalGrossAmount = 0;
    double TotalProductAmount = 0;
    double TotalPriceaftertax = 0;
    double TotalTaxvalue = 0;
    double NetTaxper = 0;
    double NetDiscountper = 0;
    double Totaldiscountvalue = 0;
    string CurrencyId = string.Empty;
    double TotalNetAmount = 0;
    double PriceAfterDiscount = 0;
    double totamCustomTaxValue = 0;
    double totamCustomDiscountValue = 0;
    double NetUnitPrice = 0;
    double NetDetailTax = 0;
    double NetdetailTotal = 0;
    private void xrLabel24_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(_strConString);
        string CompanyAddress = string.Empty;
        DataTable DtAddress = objAddMaster.GetAddressDataByTransId(xrLabel24.Text, "0");
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
        xrLabel24.Text = CompanyAddress;
    }


    private void xrLabel36_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(_strConString);
        string CompanyAddress = string.Empty;
        DataTable DtAddress = objAddMaster.GetAddressDataByTransId(xrLabel36.Text, "0");
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
        xrLabel36.Text = CompanyAddress;
    }

    private void xrLabel27_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        string Companytelno = string.Empty;
        DataTable DtAddress = Objaddress.GetAddressDataByTransId(xrLabel27.Text, System.Web.HttpContext.Current.Session["CompId"].ToString());
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
        xrLabel27.Text = Companytelno;

    }

    private void xrLabel26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        string CompanyFaxno = string.Empty;
        DataTable DtAddress = Objaddress.GetAddressDataByTransId(xrLabel26.Text, System.Web.HttpContext.Current.Session["CompId"].ToString());
        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            {
                CompanyFaxno = DtAddress.Rows[0]["FaxNo"].ToString();
            }
        }
        xrLabel26.Text = CompanyFaxno;
    }

    private void xrLabel40_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        string Companytelno = string.Empty;
        DataTable DtAddress = Objaddress.GetAddressDataByTransId(xrLabel40.Text, System.Web.HttpContext.Current.Session["CompId"].ToString());

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
        xrLabel40.Text = Companytelno;

    }

    private void xrLabel37_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

        string CompanyFaxno = string.Empty;
        DataTable DtAddress = Objaddress.GetAddressDataByTransId(xrLabel37.Text, System.Web.HttpContext.Current.Session["CompId"].ToString());

        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            {
                CompanyFaxno = DtAddress.Rows[0]["FaxNo"].ToString();
            }
        }
        xrLabel37.Text = CompanyFaxno;
    }

    private void xrLabel21_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrLabel21.Text = Convert.ToDateTime(xrLabel21.Text).ToString(objsys.SetDateFormat());
        }
        catch
        {
        }
    }

    //private void xrLabel41_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    //{
    //    try
    //    {
    //        xrLabel41.Text = Convert.ToDateTime(xrLabel41.Text).ToString(objsys.SetDateFormat());
    //    }
    //    catch
    //    {
    //    }

    //}

    private void xrCellTaxType_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            double taxAmount = 0;
            double.TryParse(GetCurrentColumnValue("NetTaxV").ToString(), out taxAmount);
            if (taxAmount > 0)
            {
                string strInvoiceId = GetCurrentColumnValue("Invoice_Id").ToString();
                string strProductId = GetCurrentColumnValue("Product_Id").ToString();
                string sql = "Select Sys_TaxMaster.Tax_Name,Inv_TaxRefDetail.Tax_Per from Inv_TaxRefDetail left join Sys_TaxMaster on Inv_TaxRefDetail.Tax_Id=Sys_TaxMaster.Trans_Id where Inv_TaxRefDetail.Ref_Type='SINV' and Inv_TaxRefDetail.Ref_Id='" + strInvoiceId + "' and Inv_TaxRefDetail.ProductId='" + strProductId + "' and cast(Inv_TaxRefDetail.tax_value as decimal)>0 order by Inv_TaxRefDetail.tax_id";
                DataTable dtProductTax = objDa.return_DataTable(sql);
                string strTaxDetail = string.Empty;
                double taxPer = 0;
                for (int i = 0; i < dtProductTax.Rows.Count; i++)
                {
                    double.TryParse(dtProductTax.Rows[i]["Tax_Per"].ToString(), out taxPer);
                    if (strTaxDetail == string.Empty)
                    {
                        strTaxDetail = dtProductTax.Rows[i]["Tax_Name"].ToString() + "-" + taxPer.ToString("0.00") + "%";
                    }
                    else
                    {
                        strTaxDetail = strTaxDetail + Environment.NewLine + dtProductTax.Rows[i]["Tax_Name"].ToString() + "-" + taxPer.ToString("0.00") + "%";
                    }
                }
                xrCellTaxType.Multiline = true;
                xrCellTaxType.Text = strTaxDetail;
                //xrRichText3.Text = strTaxDetail;
            }
        }
        catch
        {
        }

    }
    private void xrLblAmountInWords_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        CurrencyMaster Objcurrency = new CurrencyMaster(_strConString);

        DataTable dtCurrency = Objcurrency.GetCurrencyMasterById(GetCurrentColumnValue("CurrencyID").ToString());
        string CurrencyName = dtCurrency.Rows[0]["Currency_Name"].ToString();


        string Amount = string.Empty;
        try
        {
            Amount = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), GetCurrentColumnValue("GrandTotalWithExpenses").ToString());
        }
        catch
        {
        }


        string[] str = Amount.Split('.');

        string amountinwords = string.Empty;
        string amountinwordsAfterDecimal = string.Empty;
        try
        {
            amountinwords += AmountToWord(Convert.ToInt32(str[0].ToString()));

        }
        catch
        {
        }
        try
        {
            amountinwordsAfterDecimal += AmountToWord(Convert.ToInt32(str[1].ToString()));
        }
        catch
        {
        }

        if (amountinwordsAfterDecimal.Trim() == "" || amountinwordsAfterDecimal.Trim() == "Zero")
        {

            xrLblAmountInWords.Text = CurrencyName.ToUpper() + " " + amountinwords.ToString().ToUpper() + " ONLY";
        }
        else
        {

            xrLblAmountInWords.Text = CurrencyName.ToUpper() + " " + amountinwords.ToString().ToUpper() + "AND " + amountinwordsAfterDecimal.ToUpper() + " " + dtCurrency.Rows[0]["Field1"].ToString().ToUpper() + " ONLY";

        }


    }
    public static string AmountToWord(int number)
    {
        if (number == 0) return "Zero";

        if (number == -2147483648)
            return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";

        int[] num = new int[4];
        int first = 0;
        int u, h, t;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (number < 0)
        {
            sb.Append("Minus ");
            number = -number;
        }

        string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
"Five " ,"Six ", "Seven ", "Eight ", "Nine "};

        string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
"Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};

        string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
"Seventy ","Eighty ", "Ninety "};

        string[] words3 = { "Thousand ", "Lakh ", "Crore " };

        num[0] = number % 1000; // units
        num[1] = number / 1000;
        num[2] = number / 100000;
        num[1] = num[1] - 100 * num[2]; // thousands
        num[3] = number / 10000000; // crores
        num[2] = num[2] - 100 * num[3]; // lakhs

        //You can increase as per your need.

        for (int i = 3; i > 0; i--)
        {
            if (num[i] != 0)
            {
                first = i;
                break;
            }
        }


        for (int i = first; i >= 0; i--)
        {
            if (num[i] == 0) continue;

            u = num[i] % 10; // ones
            t = num[i] / 10;
            h = num[i] / 100; // hundreds
            t = t - 10 * h; // tens

            if (h > 0) sb.Append(words0[h] + "Hundred ");

            if (u > 0 || t > 0)
            {
                if (h == 0)
                    sb.Append("");
                else
                    if (h > 0 || i == 0) sb.Append("");


                if (t == 0)
                    sb.Append(words0[u]);
                else if (t == 1)
                    sb.Append(words1[u]);
                else
                    sb.Append(words2[t - 2] + words0[u]);
            }

            if (i != 0) sb.Append(words3[i - 1]);

        }
        return sb.ToString().TrimEnd() + " ";
    }
    private void xrTableCell59_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell59.Text = Math.Round(Convert.ToDouble(xrTableCell59.Text), 0).ToString();
        }
        catch
        {
        }

    }

    private void xrTableCell61_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell61.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrTableCell61.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell66_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell66.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrTableCell66.Text);

        }
        catch
        {
        }
    }

    private void xrLabel5_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {

    }

    private void xrLabel5_SummaryReset(object sender, EventArgs e)
    {



    }

    private void xrLabel5_SummaryRowChanged(object sender, EventArgs e)
    {


    }

    private void xrLabel51_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {

        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, NetTaxper.ToString());
        e.Handled = true;
    }

    private void xrLabel51_SummaryReset(object sender, EventArgs e)
    {

    }



    private void xrLabel51_SummaryRowChanged_1(object sender, EventArgs e)
    {
        NetTaxper = Convert.ToDouble(GetCurrentColumnValue("NetTaxP").ToString());

        CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        xrLabel11.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel11.Text);


    }

    private void xrLabel7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            //xrLabel7.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel7.Text);

        }
        catch
        {
        }

    }

    private void xrLabel53_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            //xrLabel53.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel53.Text);

        }
        catch
        {
        }
    }

    private void xrLabel55_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {


        //try
        //{
        //xrLabel55.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), GetCurrentColumnValue("GrandTotalWithExpenses").ToString());

        //}
        //catch
        //{
        //}

    }
    private void xrLblFreightCharges_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {


        //try
        //{
        //xrLblFreightCharges.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), GetCurrentColumnValue("ExpensesAmount").ToString());

        //}
        //catch
        //{
        //}

    }

    private void xrLabel11_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, TotalGrossAmount.ToString());
        e.Handled = true;
    }

    private void xrLabel11_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel11_SummaryRowChanged(object sender, EventArgs e)
    {
        try
        {
            TotalGrossAmount += Convert.ToDouble(GetCurrentColumnValue("ProductAmount").ToString());
        }
        catch
        {
            TotalGrossAmount += 0;
        }
        CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel55_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {

    }

    private void xrLabel55_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel55_SummaryRowChanged(object sender, EventArgs e)
    {


    }



    private void xrLabel57_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, PriceAfterDiscount.ToString());
        e.Handled = true;
    }

    private void xrLabel57_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel57_SummaryRowChanged(object sender, EventArgs e)
    {
        PriceAfterDiscount = Convert.ToDouble(GetCurrentColumnValue("PriceAfterDiscount").ToString());
        CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel7_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, totamCustomDiscountValue.ToString());
        e.Handled = true;

    }

    private void xrLabel7_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel7_SummaryRowChanged(object sender, EventArgs e)
    {
        totamCustomDiscountValue = Convert.ToDouble(GetCurrentColumnValue("NetDiscountV").ToString());
        CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel53_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, totamCustomTaxValue.ToString());
        e.Handled = true;
    }

    private void xrLabel53_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel53_SummaryRowChanged(object sender, EventArgs e)
    {
        totamCustomTaxValue = Convert.ToDouble(GetCurrentColumnValue("NetTaxV").ToString());
        CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();

    }

    private void xrLabel5_SummaryGetResult_1(object sender, SummaryGetResultEventArgs e)
    {

        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, NetDiscountper.ToString());
        e.Handled = true;
    }

    private void xrLabel5_SummaryReset_1(object sender, EventArgs e)
    {

    }

    private void xrLabel5_SummaryRowChanged_1(object sender, EventArgs e)
    {
        NetDiscountper = Convert.ToDouble(GetCurrentColumnValue("NetDiscountP").ToString());

        CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrTableCell24_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //DataTable dt = objCurrency.GetCurrencyMasterById(GetCurrentColumnValue("CurrencyID").ToString());
        //if (dt.Rows.Count > 0)
        //{
        //    xrTableCell24.Text = xrTableCell24.Text + "(" + dt.Rows[0]["Currency_Symbol"].ToString() + ")";

        //}
    }

    private void xrTableCell26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }

    private void xrLabel54_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        DataTable dt = objCurrency.GetCurrencyMasterById(GetCurrentColumnValue("CurrencyID").ToString());
        if (dt.Rows.Count > 0)
        {
           // xrLabel54.Text = xrLabel54.Text + "(" + dt.Rows[0]["Currency_Symbol"].ToString() + ")";

        }

    }

    private void xrLabel35_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }



    private void xrLabel63_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //xrLabel63.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel63.Text.ToString());
    }

    private void xrTableCell6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //xrTableCell6.Text = "0.00";
        //DataTable dtRefDetail = objTaxRefDetail.GetRecord_By_RefType_and_RefId("SINV", GetCurrentColumnValue("Invoice_Id").ToString());
        //try
        //{
        //    dtRefDetail = new DataView(dtRefDetail, "Field6='False' and ProductId=" + GetCurrentColumnValue("Product_Id").ToString() + " and TaxSelected='True'", "", DataViewRowState.CurrentRows).ToTable();



        //    dtRefDetail = dtRefDetail.DefaultView.ToTable(true, "ProductId", "ProductCategoryId", "CategoryName", "TaxName", "Tax_Per", "Tax_value", "TaxSelected", "Tax_Id", "SO_Id");
        //    if (dtRefDetail.Rows.Count > 0)
        //    {
        //        xrTableCell6.Text = "";

        //        for (int i = 0; i < dtRefDetail.Rows.Count; i++)
        //        {


        //            if (dtRefDetail.Rows[i]["Tax_value"].ToString() == "0")
        //            {
        //                continue;
        //            }




        //            if (xrTableCell6.Text == "")
        //            {
        //                xrTableCell6.Text = dtRefDetail.Rows[i]["TaxName"].ToString() + "(" + dtRefDetail.Rows[i]["Tax_Per"].ToString() + "%) = " + dtRefDetail.Rows[i]["Tax_value"].ToString();

        //            }
        //            else
        //            {
        //                xrTableCell6.Text += Environment.NewLine + dtRefDetail.Rows[i]["TaxName"].ToString() + "(" + dtRefDetail.Rows[i]["Tax_Per"].ToString() + "%) = " + dtRefDetail.Rows[i]["Tax_value"].ToString();
        //            }

        //        }


        //    }
        //    else
        //    {
        //        xrTableCell6.Text= objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), GetCurrentColumnValue("TaxV").ToString());
        //    }
        //}
        //catch
        //{

        //}
    }

    private void xrLabel13_SummaryRowChanged(object sender, EventArgs e)
    {
        try
        {
            NetUnitPrice += Convert.ToDouble(GetCurrentColumnValue("ItemUnitPriceTotal").ToString());
        }
        catch
        {
            NetUnitPrice += 0;
        }
        CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel13_SummaryReset(object sender, EventArgs e)
    {
        NetUnitPrice = 0;
    }

    private void xrLabel13_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, NetUnitPrice.ToString());
        e.Handled = true;
    }

    private void xrLabel3_SummaryRowChanged(object sender, EventArgs e)
    {
        try
        {
            NetDetailTax += Convert.ToDouble(GetCurrentColumnValue("TaxV").ToString());
        }
        catch
        {
            NetDetailTax += 0;
        }
        CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel3_SummaryReset(object sender, EventArgs e)
    {
        NetDetailTax = 0;
    }

    private void xrLabel3_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, NetDetailTax.ToString());
        e.Handled = true;
    }

    private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {



    }

    private void xrLabel55_SummaryRowChanged_1(object sender, EventArgs e)
    {
        TotalNetAmount = Convert.ToDouble(GetCurrentColumnValue("GrandTotal").ToString());
        CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel55_SummaryReset_1(object sender, EventArgs e)
    {

    }

    private void xrLabel55_SummaryGetResult_1(object sender, SummaryGetResultEventArgs e)
    {

        try
        {
            TotalNetAmount = Math.Round(TotalNetAmount, 0);
        }
        catch
        {
        }

        e.Result = objsys.GetCurencyConversionForInv(CurrencyId, TotalNetAmount.ToString());
        e.Handled = true;
    }

    private void xrCellProductCode_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        string SerialNumber = string.Empty;
        try
        {

            DataTable dtStock = ObjStockBatchMaster.GetStockBatchMasterAll(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["BrandId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString());

            dtStock = new DataView(dtStock, "TransType='SI' and TransTypeId=" + GetCurrentColumnValue("Invoice_Id").ToString() + " and ProductId='" + GetCurrentColumnValue("Product_Id").ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


            for (int i = 0; i < dtStock.Rows.Count; i++)
            {

                if (SerialNumber == "")
                {
                    SerialNumber = "SN.- " + dtStock.Rows[i]["SerialNo"].ToString();
                }
                else
                {
                    SerialNumber = SerialNumber + "," + dtStock.Rows[i]["SerialNo"].ToString();
                }
            }

        }
        catch
        {


        }

        xrCellProductCode.Text += Environment.NewLine + GetCurrentColumnValue("ProductName").ToString();

        string sql = "select HsCode from inv_productmaster where productId='" + GetCurrentColumnValue("Product_Id").ToString() + "'";
        string HsnCode = objDa.get_SingleValue(sql);
        if (HsnCode != string.Empty && HsnCode != "Null")
        {
            xrCellProductCode.Text += Environment.NewLine + Environment.NewLine + "HSN-" + HsnCode;
        }
        if (SerialNumber != "")
        {
            xrCellProductCode.Text += Environment.NewLine + Environment.NewLine + SerialNumber;
        }




    }


    private void xrCellAmount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        double rate = 0;
        double qty = 0;
        double.TryParse(GetCurrentColumnValue("Quantity").ToString(), out rate);
        double.TryParse(GetCurrentColumnValue("UnitPrice").ToString(), out qty);
        xrCellAmount.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), (rate * qty).ToString());
    }
    private void xrCellDiscount_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //string strDiscount = GetCurrentColumnValue("DiscountV").ToString();
        if (xrCellDiscount.Text != "0" && xrCellDiscount.Text != string.Empty)
        {
            xrCellDiscount.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrCellDiscount.Text);
        }
    }

    private void xrCellTax_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            double taxAmount = 0;
            double.TryParse(GetCurrentColumnValue("NetTaxV").ToString(), out taxAmount);
            if (taxAmount > 0)
            {
                string strInvoiceId = GetCurrentColumnValue("Invoice_Id").ToString();
                string strProductId = GetCurrentColumnValue("Product_Id").ToString();
                string sql = "Select sum(Convert(Float, tax_value)) as taxAmount from Inv_TaxRefDetail where Inv_TaxRefDetail.Ref_Type='SINV' and Inv_TaxRefDetail.Ref_Id='" + strInvoiceId + "' and Inv_TaxRefDetail.ProductId='" + strProductId + "' and cast(Inv_TaxRefDetail.tax_value as decimal)>0 group by tax_id order by Inv_TaxRefDetail.tax_id";
                DataTable dtProductTax = objDa.return_DataTable(sql);
                string strTaxDetail = string.Empty;
                double taxVal = 0;
                for (int i = 0; i < dtProductTax.Rows.Count; i++)
                {
                    double.TryParse(dtProductTax.Rows[i]["taxAmount"].ToString(), out taxVal);
                    if (strTaxDetail == string.Empty)
                    {
                        strTaxDetail = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), taxVal.ToString());
                    }
                    else
                    {
                        strTaxDetail = strTaxDetail + Environment.NewLine + objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), taxVal.ToString());
                    }
                }
                xrCellTax.Multiline = true;
                xrCellTax.Text = strTaxDetail;
                //xrRichText3.Text = strTaxDetail;
            }
        }
        catch
        {
        }




        //double qty = 0;
        //double UnitTax = 0;
        //double.TryParse(GetCurrentColumnValue("Quantity").ToString(), out qty);
        //double.TryParse(GetCurrentColumnValue("TaxV").ToString(), out UnitTax);

        ////string strTax = GetCurrentColumnValue("TaxV").ToString();
        //if (xrCellTax.Text != "0" && xrCellTax.Text != string.Empty)
        //{
        //    xrCellTax.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), (qty*UnitTax).ToString());
        //}
    }

    private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        
    }
    private void xrLblCmpSign_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        xrLblCmpSign.Text = "For " + xrLabel2.Text;
    }

    private void xrPanelH_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

    }

    private void xrLblTotalTax_BeforePrint(object sender, PrintEventArgs e)
    {
        xrLblTotalTax.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLblTotalTax.Text);
    }

    private void GroupFooter2_BeforePrint(object sender, PrintEventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            double taxAmount = 0;
            double.TryParse(GetCurrentColumnValue("NetTaxV").ToString(), out taxAmount);
            if (taxAmount > 0)
            {
                string strInvoiceId = GetCurrentColumnValue("Invoice_Id").ToString();
                //string strProductId = GetCurrentColumnValue("Product_Id").ToString();
                //string sql = "Select Sys_TaxMaster.Tax_Name,Inv_TaxRefDetail.Tax_Per,sum(cast(Inv_TaxRefDetail.field1 as decimal))as taxable_amount,sum(cast(Inv_TaxRefDetail.tax_value as decimal)) as tax_amount from Inv_TaxRefDetail left join Sys_TaxMaster on Inv_TaxRefDetail.Tax_Id=Sys_TaxMaster.Trans_Id where Inv_TaxRefDetail.Ref_Type='SINV' and Inv_TaxRefDetail.Ref_Id='" + strInvoiceId + "' and cast(Inv_TaxRefDetail.tax_value as decimal)>0 and (Expenses_Id is null or Expenses_Id='') group by Inv_TaxRefDetail.tax_id,Sys_TaxMaster.Tax_Name,Inv_TaxRefDetail.Tax_Per order by Inv_TaxRefDetail.tax_id";
                string sql = "Select Sys_TaxMaster.Tax_Name, Avg(CONVERT(float, Inv_TaxRefDetail.Tax_Per)) as Tax_Per,SUM(convert(float,Inv_TaxRefDetail.Tax_value)) as tax_amount,SUM(convert(float,Inv_TaxRefDetail.Field1)) as taxable_amount From Inv_TaxRefDetail Left join Sys_TaxMaster on Inv_TaxRefDetail.Tax_Id=Sys_TaxMaster.Trans_Id where Inv_TaxRefDetail.Ref_Type='SINV' and Inv_TaxRefDetail.Ref_Id='" + strInvoiceId + "' and (Inv_TaxRefDetail.Expenses_Id is null or Inv_TaxRefDetail.Expenses_Id ='') Group by Inv_TaxRefDetail.Tax_Per,Sys_TaxMaster.Tax_Name order by Inv_TaxRefDetail.Tax_Per,Sys_TaxMaster.Tax_Name";
                dt = objDa.return_DataTable(sql);
                string strTaxDetail = string.Empty;
                double _taxPer = 0;
                double _taxableAmount = 0;
                double _taxAmount = 0;

                // XRTableRow xrTblRow = new XRTableRow();
                //xrTableTax.BeginInit();
                //xrtableTax.Rows.Add(dr);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    XRTableRow xrTblRow = new XRTableRow();


                    //xrTblRow = xrTblTaxRow;

                    double.TryParse(dt.Rows[i]["Tax_Per"].ToString(), out _taxPer);
                    double.TryParse(dt.Rows[i]["taxable_amount"].ToString(), out _taxableAmount);
                    double.TryParse(dt.Rows[i]["tax_amount"].ToString(), out _taxAmount);

                    //xrTblRow.Cells[0].Text = dt.Rows[i]["Tax_Name"].ToString();
                    //xrTblRow.Cells[1].Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxPer.ToString());
                    //xrTblRow.Cells[2].Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxableAmount.ToString());
                    //xrTblRow.Cells[3].Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxAmount.ToString()); ;
                    XRTableCell _xrCellTaxType = new XRTableCell();
                    _xrCellTaxType.Font = xrTblCellTaxType.Font;
                    _xrCellTaxType.Borders = xrTblCellTaxType.Borders;
                    _xrCellTaxType.SizeF = xrTblCellTaxType.SizeF;
                    _xrCellTaxType.TextAlignment = xrTblCellTaxType.TextAlignment;
                    _xrCellTaxType.Text = dt.Rows[i]["Tax_Name"].ToString();

                    XRTableCell _xrCellTaxPer = new XRTableCell();
                    _xrCellTaxPer.Font = xrTblCellTaxPer.Font;
                    _xrCellTaxPer.Borders = xrTblCellTaxPer.Borders;
                    _xrCellTaxPer.SizeF = xrTblCellTaxPer.SizeF;
                    _xrCellTaxPer.TextAlignment = xrTblCellTaxPer.TextAlignment;
                    _xrCellTaxPer.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxPer.ToString());

                    XRTableCell _xrCellTaxableAmount = new XRTableCell();
                    _xrCellTaxableAmount.Font = xrTblCellTaxableAmount.Font;
                    _xrCellTaxableAmount.Borders = xrTblCellTaxableAmount.Borders;
                    _xrCellTaxableAmount.SizeF = xrTblCellTaxableAmount.SizeF;
                    _xrCellTaxableAmount.TextAlignment = xrTblCellTaxableAmount.TextAlignment;
                    _xrCellTaxableAmount.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxableAmount.ToString());

                    XRTableCell _xrCellTaxAmount = new XRTableCell();
                    _xrCellTaxAmount.Font = xrTblCellTaxAmount.Font;
                    _xrCellTaxAmount.Borders = xrTblCellTaxAmount.Borders;
                    _xrCellTaxAmount.SizeF = xrTblCellTaxAmount.SizeF;
                    _xrCellTaxAmount.TextAlignment = xrTblCellTaxAmount.TextAlignment;
                    _xrCellTaxAmount.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxAmount.ToString());

                    xrTblRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
                    _xrCellTaxType,_xrCellTaxPer, _xrCellTaxableAmount, _xrCellTaxAmount});
                    xrTableTax.Rows.Add(xrTblRow);
                }
            }

            DataTable dt_Expenses = new DataTable();
            double ExpensestaxAmount = 0;
            double.TryParse(GetCurrentColumnValue("NetTaxV").ToString(), out ExpensestaxAmount);
            if (ExpensestaxAmount > 0)
            {
                string strInvoiceId = GetCurrentColumnValue("Invoice_Id").ToString();
                //string strProductId = GetCurrentColumnValue("Product_Id").ToString();
                //string sql = "Select Sys_TaxMaster.Tax_Name,Inv_TaxRefDetail.Tax_Per,sum(cast(Inv_TaxRefDetail.field1 as decimal))as taxable_amount,sum(cast(Inv_TaxRefDetail.tax_value as decimal)) as tax_amount from Inv_TaxRefDetail left join Sys_TaxMaster on Inv_TaxRefDetail.Tax_Id=Sys_TaxMaster.Trans_Id where Inv_TaxRefDetail.Ref_Type='SINV' and Inv_TaxRefDetail.Ref_Id='" + strInvoiceId + "' and cast(Inv_TaxRefDetail.tax_value as decimal)>0 and (Expenses_Id is null or Expenses_Id='') group by Inv_TaxRefDetail.tax_id,Sys_TaxMaster.Tax_Name,Inv_TaxRefDetail.Tax_Per order by Inv_TaxRefDetail.tax_id";
                string sql = "Select Sys_TaxMaster.Tax_Name, Avg(CONVERT(float, Inv_TaxRefDetail.Tax_Per)) as Tax_Per,SUM(convert(float,Inv_TaxRefDetail.Tax_value)) as tax_amount,SUM(convert(float,Inv_TaxRefDetail.Field1)) as taxable_amount From Inv_TaxRefDetail Left join Sys_TaxMaster on Inv_TaxRefDetail.Tax_Id=Sys_TaxMaster.Trans_Id where Inv_TaxRefDetail.Ref_Type='SINV' and Inv_TaxRefDetail.Ref_Id='" + strInvoiceId + "' and (Inv_TaxRefDetail.Expenses_Id is not null or Inv_TaxRefDetail.Expenses_Id !='') Group by Inv_TaxRefDetail.Tax_Per,Sys_TaxMaster.Tax_Name order by Inv_TaxRefDetail.Tax_Per,Sys_TaxMaster.Tax_Name";
                dt_Expenses = objDa.return_DataTable(sql);
                string strTaxDetail = string.Empty;
                double _taxPer = 0;
                double _taxableAmount = 0;
                double _taxAmount = 0;

                // XRTableRow xrTblRow = new XRTableRow();
                //xrTableTax.BeginInit();
                //xrtableTax.Rows.Add(dr);
                for (int i = 0; i < dt_Expenses.Rows.Count; i++)
                {
                    XRTableRow xrTblRow = new XRTableRow();


                    //xrTblRow = xrTblTaxRow;

                    double.TryParse(dt_Expenses.Rows[i]["Tax_Per"].ToString(), out _taxPer);
                    double.TryParse(dt_Expenses.Rows[i]["taxable_amount"].ToString(), out _taxableAmount);
                    double.TryParse(dt_Expenses.Rows[i]["tax_amount"].ToString(), out _taxAmount);

                    //xrTblRow.Cells[0].Text = dt_Expenses.Rows[i]["Tax_Name"].ToString();
                    //xrTblRow.Cells[1].Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxPer.ToString());
                    //xrTblRow.Cells[2].Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxableAmount.ToString());
                    //xrTblRow.Cells[3].Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxAmount.ToString()); ;
                    XRTableCell _xrCellTaxType = new XRTableCell();
                    _xrCellTaxType.Font = xrTblCellTaxType.Font;
                    _xrCellTaxType.Borders = xrTblCellTaxType.Borders;
                    _xrCellTaxType.SizeF = xrTblCellTaxType.SizeF;
                    _xrCellTaxType.TextAlignment = xrTblCellTaxType.TextAlignment;
                    _xrCellTaxType.Text = dt_Expenses.Rows[i]["Tax_Name"].ToString();

                    XRTableCell _xrCellTaxPer = new XRTableCell();
                    _xrCellTaxPer.Font = xrTblCellTaxPer.Font;
                    _xrCellTaxPer.Borders = xrTblCellTaxPer.Borders;
                    _xrCellTaxPer.SizeF = xrTblCellTaxPer.SizeF;
                    _xrCellTaxPer.TextAlignment = xrTblCellTaxPer.TextAlignment;
                    _xrCellTaxPer.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxPer.ToString());

                    XRTableCell _xrCellTaxableAmount = new XRTableCell();
                    _xrCellTaxableAmount.Font = xrTblCellTaxableAmount.Font;
                    _xrCellTaxableAmount.Borders = xrTblCellTaxableAmount.Borders;
                    _xrCellTaxableAmount.SizeF = xrTblCellTaxableAmount.SizeF;
                    _xrCellTaxableAmount.TextAlignment = xrTblCellTaxableAmount.TextAlignment;
                    _xrCellTaxableAmount.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxableAmount.ToString());

                    XRTableCell _xrCellTaxAmount = new XRTableCell();
                    _xrCellTaxAmount.Font = xrTblCellTaxAmount.Font;
                    _xrCellTaxAmount.Borders = xrTblCellTaxAmount.Borders;
                    _xrCellTaxAmount.SizeF = xrTblCellTaxAmount.SizeF;
                    _xrCellTaxAmount.TextAlignment = xrTblCellTaxAmount.TextAlignment;
                    _xrCellTaxAmount.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _taxAmount.ToString());

                    xrTblRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
                    _xrCellTaxType,_xrCellTaxPer, _xrCellTaxableAmount, _xrCellTaxAmount});
                    xrTableTax.Rows.Add(xrTblRow);
                }
            }
        }
        catch
        {
        }

        xrTableTax.KeepTogether = true;
    }

    private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        DataTable Dt_Expenses = new DataTable();
        try
        {
            string strInvoiceId = GetCurrentColumnValue("Invoice_Id").ToString();
            string sql = "Select ISEM.Exp_Name, ISEM.Expense_Id AS Expenses_Id, '0' AS Discount, ISED.FCExpAmount AS Amount, SUM(CONVERT(float, (CASE WHEN ITRD.Tax_value IS NOT NULL THEN ITRD.Tax_value ELSE '0' END))) AS Tax_Amount, (SUM(Convert(Float,(Case When ITRD.Tax_value IS Not Null Then ITRD.Tax_value else '0' end)))) + (SUM(DISTINCT Case When ISED.FCExpAmount IS Not Null Then ISED.FCExpAmount else '0' end)) as Total_Amount From Inv_ShipExpDetail ISED Inner join Inv_ShipExpMaster ISEM on ISEM.Expense_Id=ISED.Expense_Id Left Join Inv_TaxRefDetail ITRD on ITRD.Expenses_Id=ISED.Expense_Id and ITRD.Ref_Id='" + strInvoiceId + "' where ISED.Field1='SI' and ISED.InvoiceNo='" + strInvoiceId + "' and ISED.Company_Id='" + System.Web.HttpContext.Current.Session["CompId"].ToString() + "' and ISED.Brand_Id='" + System.Web.HttpContext.Current.Session["BrandId"].ToString() + "' and ISED.Location_Id='" + System.Web.HttpContext.Current.Session["LocId"].ToString() + "' Group by ISEM.Exp_Name,ISEM.Expense_Id,ISED.FCExpAmount";
            Dt_Expenses = objDa.return_DataTable(sql);
            string strTaxDetail = string.Empty;
            double _Amount = 0;
            double _Tax_Amount = 0;
            double _Total_Amount = 0;
            double _Discount = 0;
            for (int i = 0; i < Dt_Expenses.Rows.Count; i++)
            {
                XRTableRow xrTblRow = new XRTableRow();
                double.TryParse(Dt_Expenses.Rows[i]["Amount"].ToString(), out _Amount);                
                double.TryParse(Dt_Expenses.Rows[i]["Total_Amount"].ToString(), out _Total_Amount);
                double.TryParse(Dt_Expenses.Rows[i]["Discount"].ToString(), out _Discount);

                XRTableCell _xrTableCellExpensesName = new XRTableCell();
                _xrTableCellExpensesName.BorderColor = System.Drawing.Color.Black;
                _xrTableCellExpensesName.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _xrTableCellExpensesName.Borders = xrTblCellTaxPer.Borders;

                _xrTableCellExpensesName.StylePriority.UseFont = false;
                _xrTableCellExpensesName.StylePriority.UseTextAlignment = false;
                _xrTableCellExpensesName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                _xrTableCellExpensesName.WidthF = DevExpress.Office.Utils.Units.DocumentsToPointsF(467.04F);
                _xrTableCellExpensesName.Text = Dt_Expenses.Rows[i]["Exp_Name"].ToString();



                //_xrTableCellExpensesName.Font = xrTableCellExpensesName.Font;
                //_xrTableCellExpensesName.Borders = xrTblCellTaxType.Borders;
                //_xrTableCellExpensesName.WidthF = DevExpress.Office.Utils.Units.DocumentsToPointsF(467.04F);
                //_xrTableCellExpensesName.SizeF = xrTblCellTaxType.SizeF;
                //_xrTableCellExpensesName.TextAlignment = xrTblCellTaxType.TextAlignment;
                //_xrTableCellExpensesName.Text = Dt_Expenses.Rows[i]["Exp_Name"].ToString();

                XRTableCell _xrTableCellExpensesAmount = new XRTableCell();
                _xrTableCellExpensesAmount.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _xrTableCellExpensesAmount.Borders = xrTblCellTaxPer.Borders;
                _xrTableCellExpensesAmount.WidthF = DevExpress.Office.Utils.Units.DocumentsToPointsF(60.67F);
                //_xrTableCellExpensesAmount.SizeF = xrTblCellTaxPer.SizeF;
                _xrTableCellExpensesAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                _xrTableCellExpensesAmount.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _Amount.ToString());

                XRTableCell _xrTableCellExpensesDiscount = new XRTableCell();
                _xrTableCellExpensesDiscount.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _xrTableCellExpensesDiscount.Borders = xrTblCellTaxableAmount.Borders;
                _xrTableCellExpensesDiscount.WidthF = DevExpress.Office.Utils.Units.DocumentsToPointsF(53.82F);
                //_xrTableCellExpensesDiscount.SizeF = xrTblCellTaxableAmount.SizeF;
                _xrTableCellExpensesDiscount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                _xrTableCellExpensesDiscount.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _Discount.ToString());

                DataTable dtProductTax = new DataTable();
                string sql_Sub = "Select Sys_TaxMaster.Tax_Name,Inv_TaxRefDetail.Tax_value,Inv_TaxRefDetail.Tax_Per from Inv_TaxRefDetail left join Sys_TaxMaster on Inv_TaxRefDetail.Tax_Id=Sys_TaxMaster.Trans_Id where Inv_TaxRefDetail.Ref_Type='SINV' and Inv_TaxRefDetail.Ref_Id='" + strInvoiceId + "' and Inv_TaxRefDetail.Expenses_Id='" + Dt_Expenses.Rows[i]["Expenses_Id"].ToString() + "' and cast(Inv_TaxRefDetail.tax_value as decimal)>0 order by Inv_TaxRefDetail.tax_id";
                dtProductTax = objDa.return_DataTable(sql_Sub);
                string strTaxDetail_Sub = string.Empty;
                string strTaxAmount_Sub = string.Empty;
                double taxPer = 0;
                for (int J = 0; J < dtProductTax.Rows.Count; J++)
                {
                    double.TryParse(dtProductTax.Rows[J]["Tax_Per"].ToString(), out taxPer);
                    double.TryParse(dtProductTax.Rows[J]["Tax_value"].ToString(), out _Tax_Amount);
                    if (strTaxDetail_Sub == string.Empty)
                    {
                        strTaxDetail_Sub = dtProductTax.Rows[J]["Tax_Name"].ToString() + "-" + taxPer.ToString("0.00") + "%";
                        strTaxAmount_Sub = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _Tax_Amount.ToString());
                    }
                    else
                    {
                        strTaxDetail_Sub = strTaxDetail_Sub + Environment.NewLine + dtProductTax.Rows[J]["Tax_Name"].ToString() + "-" + taxPer.ToString("0.00") + "%";
                        strTaxAmount_Sub = strTaxAmount_Sub + Environment.NewLine + objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _Tax_Amount.ToString());
                    }
                }                
                XRTableCell _xrTableCellExpensesTaxType = new XRTableCell();
                _xrTableCellExpensesTaxType.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _xrTableCellExpensesTaxType.Borders = xrTblCellTaxType.Borders;
                _xrTableCellExpensesTaxType.WidthF = DevExpress.Office.Utils.Units.DocumentsToPointsF(82.39F);
                //_xrTableCellExpensesTaxType.SizeF = xrTblCellTaxType.SizeF;
                _xrTableCellExpensesTaxType.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                _xrTableCellExpensesTaxType.Multiline = true;
                _xrTableCellExpensesTaxType.Text = strTaxDetail_Sub;




                XRTableCell _xrTableCellExpensesTaxAmount = new XRTableCell();
                _xrTableCellExpensesTaxAmount.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _xrTableCellExpensesTaxAmount.Borders = xrTblCellTaxAmount.Borders;
                _xrTableCellExpensesTaxAmount.WidthF = DevExpress.Office.Utils.Units.DocumentsToPointsF(69.84F);
                //_xrTableCellExpensesTaxAmount.SizeF = xrTblCellTaxAmount.SizeF;
                _xrTableCellExpensesTaxAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                _xrTableCellExpensesTaxAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 3, 0);
                _xrTableCellExpensesTaxAmount.Multiline = true;
                _xrTableCellExpensesTaxAmount.Text = strTaxAmount_Sub;

                XRTableCell _xrTableCellExpensesTotalAmount = new XRTableCell();
                _xrTableCellExpensesTotalAmount.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                _xrTableCellExpensesTotalAmount.Borders = xrTblCellTaxableAmount.Borders;
                _xrTableCellExpensesTotalAmount.WidthF = DevExpress.Office.Utils.Units.DocumentsToPointsF(82.75F);
                //_xrTableCellExpensesTotalAmount.SizeF = xrTblCellTaxableAmount.SizeF;
                _xrTableCellExpensesTotalAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                _xrTableCellExpensesTotalAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 3, 0);
                _xrTableCellExpensesTotalAmount.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), _Total_Amount.ToString());
                

                xrTblRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
                    _xrTableCellExpensesName,_xrTableCellExpensesAmount, _xrTableCellExpensesDiscount,_xrTableCellExpensesTaxType, _xrTableCellExpensesTaxAmount, _xrTableCellExpensesTotalAmount});


                xrTblRow.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;

                xrTblRow.BorderColor = System.Drawing.Color.Black;
                
                xrTblRow.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                xrTableExpenses.Rows.Add(xrTblRow);
            }
        }
        catch
        {
        }

        xrTableExpenses.KeepTogether = true;
    }

    private void xrTableCellTaxType_BeforePrint(object sender, PrintEventArgs e)
    {
        //try
        //{
        //    double taxAmount = 0;
        //    double.TryParse(GetCurrentColumnValue("NetTaxV").ToString(), out taxAmount);
        //    if (taxAmount > 0)
        //    {
        //        string strInvoiceId = GetCurrentColumnValue("Invoice_Id").ToString();
        //        string sql_main = "Select ISED.Exp_Name,ITRD.Expenses_Id,ITRD.Field1 As Amount,Sum(CONVERT(float, ITRD.Tax_value)) as Tax_Amount,Convert(Float,ITRD.Field1)+ Sum(CONVERT(float, ITRD.Tax_value)) as Total_Amount From Inv_TaxRefDetail ITRD Inner join Inv_ShipExpMaster ISED on ITRD.Expenses_Id=ISED.Expense_Id where ITRD.Ref_Type='SINV' and ITRD.Ref_Id='" + strInvoiceId + "' and (ITRD.Expenses_Id is not null or ITRD.Expenses_Id!='') Group By ISED.Exp_Name,ITRD.Field1,ITRD.Expenses_Id";
        //        DataTable dtProductTax_Main = objDa.return_DataTable(sql_main);
        //        if (dtProductTax_Main != null && dtProductTax_Main.Rows.Count > 0)
        //        {
        //            foreach (DataRow DT_Row in dtProductTax_Main.Rows)
        //            {
        //                string sql = "Select Sys_TaxMaster.Tax_Name,Inv_TaxRefDetail.Tax_Per from Inv_TaxRefDetail left join Sys_TaxMaster on Inv_TaxRefDetail.Tax_Id=Sys_TaxMaster.Trans_Id where Inv_TaxRefDetail.Ref_Type='SINV' and Inv_TaxRefDetail.Ref_Id='" + strInvoiceId + "' and Inv_TaxRefDetail.Expenses_Id='" + DT_Row["Expenses_Id"].ToString() + "' and cast(Inv_TaxRefDetail.tax_value as decimal)>0 order by Inv_TaxRefDetail.tax_id";
        //                DataTable dtProductTax = objDa.return_DataTable(sql);
        //                string strTaxDetail = string.Empty;
        //                double taxPer = 0;
        //                for (int i = 0; i < dtProductTax.Rows.Count; i++)
        //                {
        //                    double.TryParse(dtProductTax.Rows[i]["Tax_Per"].ToString(), out taxPer);
        //                    if (strTaxDetail == string.Empty)
        //                    {
        //                        strTaxDetail = dtProductTax.Rows[i]["Tax_Name"].ToString() + "-" + taxPer.ToString("0.00") + "%";
        //                    }
        //                    else
        //                    {
        //                        strTaxDetail = strTaxDetail + Environment.NewLine + dtProductTax.Rows[i]["Tax_Name"].ToString() + "-" + taxPer.ToString("0.00") + "%";
        //                    }
        //                }
        //                xrTableCellTaxType.Multiline = true;
        //                xrTableCellTaxType.Text = strTaxDetail;
        //                //xrRichText3.Text = strTaxDetail;
        //            }
        //        }
        //    }
        //}
        //catch
        //{
        //}
    }

    private void xrLblTotalTax_BeforePrint_1(object sender, PrintEventArgs e)
    {
        xrLblTotalTax.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLblTotalTax.Text);
    }
}