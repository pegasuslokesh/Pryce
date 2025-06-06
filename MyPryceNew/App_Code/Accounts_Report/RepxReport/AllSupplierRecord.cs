using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for PurchaseRequestPrint
/// </summary>
public class AllSupplierRecord : DevExpress.XtraReports.UI.XtraReport
{
    CompanyMaster ObjCompany = null;
    SystemParameter objsys = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    Set_Suppliers ObjSupplier = null;
    private string _strConString = string.Empty;
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private ReportHeaderBand ReportHeader;
    private PageHeaderBand PageHeader;
    private PageFooterBand PageFooter;
    private PurchaseDataSet purchaseDataSet1;
    private InventoryDataSet InventoryDataSet1;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrReportTitle;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel2;
    private XRPanel xrPanel1;
    private AccountsDataset accountsDataset1;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private ReportFooterBand ReportFooter;
    private XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell36;
    private XRLabel xrLabel1;
    private GroupFooterBand GroupFooter2;
    private XRTable xrTable8;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell58;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell60;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell2;
    private XRLabel xrLabel8;
    private XRLabel xrLabel3;
    private XRLabel xrLabel9;
    private XRLabel xrLabel7;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRLabel xrLabel6;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public AllSupplierRecord(string strConString)
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
        ObjCompany = new CompanyMaster(strConString);
        ObjAgeingDetail = new Ac_Ageing_Detail(strConString);
        objsys = new SystemParameter(strConString);
        ObjSupplier = new Set_Suppliers(strConString);
        _strConString = strConString;
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
    public void setcompanyAddress(string Address)
    {
        //xrLabel1.Text = Address;
    }
    public void setcompanyname(string companyname)
    {
        xrLabel2.Text = companyname;
    }
    public void setCurrencySymbol(string CurrencyId)
    {
        xrTableCell8.Text = SystemParameter.GetCurrencySmbol(CurrencyId, "Balance",_strConString);
        xrTableCell4.Text = SystemParameter.GetCurrencySmbol(CurrencyId, "Ageing Balance",_strConString);
    }
    public void SetImage(string Url)
    {
        xrPictureBox1.ImageUrl = Url;
    }
    public void setCompanyArebicName(string ArebicName)
    {
        //xrLabel7.Text = ArebicName;

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
    public void setSignature(string Url)
    {
        //try
        //{
        //    xrPictureBox2.ImageUrl = Url;
        //}
        //catch
        //{
        //}
    }
    public void setReportTitle(string strReportTile)
    {
        xrReportTitle.Text = strReportTile;
    }

    public void setContactPersonName(string strpersonName)
    {
        //xrTableCell23.Text = strpersonName;
    }
    public void setAddress(string strAddress)
    {
        //xrTableCell21.Text = strAddress;
    }
    public void setTelephoneno(string strTelephoneNo)
    {
        //xrTableCell16.Text = strTelephoneNo;
    }
    public void setOPeningBalance(string stropBalance)
    {
        //xrTableCell25.Text = stropBalance;
    }
    public void setDateFilter(string strDatefiltertext)
    {
        xrLabel1.Text = strDatefiltertext;
    }
    public void set0_30(string lbl0_30)
    {
        //xrcell0_30.Text = lbl0_30;

    }
    public void set31_60(string lbl31_60)
    {
        //xrcell31_60.Text = lbl31_60;
    }

    public void set61_90(string lbl61_90)
    {
        // xrcell61_90.Text = lbl61_90;
    }

    public void set91_180(string lbl91_180)
    {
        //xrcell91_180.Text = lbl91_180;
    }

    public void set181_365(string lbl181_365)
    {
        //xrcell181_365.Text = lbl181_365;
    }

    public void setabove365(string lblabove_365)
    {
        //xrcellabove_365.Text = lblabove_365;
    }



    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string resourceFileName = "AllSupplierRecord.resx";
        DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary6 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary7 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary8 = new DevExpress.XtraReports.UI.XRSummary();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrReportTitle = new DevExpress.XtraReports.UI.XRLabel();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.purchaseDataSet1 = new PurchaseDataSet();
        this.InventoryDataSet1 = new InventoryDataSet();
        this.accountsDataset1 = new AccountsDataset();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
        this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel9,
            this.xrLabel7,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel8,
            this.xrLabel3});
        this.Detail.HeightF = 23F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel6
        // 
        this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Credit_Final")});
        this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(506.8059F, 0F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(136.1128F, 23F);
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.StylePriority.UseTextAlignment = false;
        this.xrLabel6.Text = "xrLabel6";
        this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrLabel6.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel6_BeforePrint);
        // 
        // xrLabel9
        // 
        this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.AgeingBalance_Final")});
        this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(881.5931F, 0F);
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(185.4065F, 23F);
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.StylePriority.UseTextAlignment = false;
        this.xrLabel9.Text = "xrLabel9";
        this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrLabel9.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel9_BeforePrint);
        // 
        // xrLabel7
        // 
        this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Closing_Final")});
        this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(642.9188F, 0F);
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(105.2085F, 23F);
        this.xrLabel7.StylePriority.UseFont = false;
        this.xrLabel7.StylePriority.UseTextAlignment = false;
        this.xrLabel7.Text = "xrLabel7";
        this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrLabel7.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel7_BeforePrint);
        // 
        // xrLabel5
        // 
        this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Debit_Final")});
        this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(398.2315F, 0F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(108.5744F, 23F);
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.Text = "xrLabel5";
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrLabel5.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel5_BeforePrint);
        // 
        // xrLabel4
        // 
        this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Opening_Final")});
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(264.7057F, 0F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(133.5258F, 23F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "xrLabel4";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrLabel4.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel4_BeforePrint);
        // 
        // xrLabel8
        // 
        this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.ForeignClosing_Balance")});
        this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(748.1273F, 0F);
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(133.4658F, 23F);
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.StylePriority.UseTextAlignment = false;
        this.xrLabel8.Text = "xrLabel8";
        this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrLabel8.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel8_BeforePrint);
        // 
        // xrLabel3
        // 
        this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Name")});
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(264.7058F, 23F);
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "xrLabel3";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // TopMargin
        // 
        this.TopMargin.HeightF = 0F;
        this.TopMargin.Name = "TopMargin";
        this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // BottomMargin
        // 
        this.BottomMargin.HeightF = 2F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrPanel1});
        this.ReportHeader.HeightF = 101.6668F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrLabel1
        // 
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0.4887024F, 73.66681F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(425.0672F, 18F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrPictureBox1,
            this.xrReportTitle});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(1067F, 56.25008F);
        this.xrPanel1.StylePriority.UseBorders = false;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(1.861898F, 1.333329F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(603.1976F, 21.95834F);
        this.xrLabel2.StylePriority.UseBorders = false;
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "xrLabel1";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(949.0285F, 1.333332F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(95.83319F, 45.95832F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrReportTitle
        // 
        this.xrReportTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrReportTitle.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
        this.xrReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(398.2315F, 24.29166F);
        this.xrReportTitle.Name = "xrReportTitle";
        this.xrReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrReportTitle.SizeF = new System.Drawing.SizeF(281.2782F, 23F);
        this.xrReportTitle.StylePriority.UseBorders = false;
        this.xrReportTitle.StylePriority.UseFont = false;
        this.xrReportTitle.StylePriority.UseTextAlignment = false;
        this.xrReportTitle.Text = "SUPPLIER ACCOUNTS";
        this.xrReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
        this.PageHeader.HeightF = 31.04165F;
        this.PageHeader.Name = "PageHeader";
        // 
        // xrTable1
        // 
        this.xrTable1.BackColor = System.Drawing.Color.Silver;
        this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(1.861898F, 10.00001F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
        this.xrTable1.SizeF = new System.Drawing.SizeF(1065.138F, 20F);
        this.xrTable1.StylePriority.UseBackColor = false;
        this.xrTable1.StylePriority.UseBorders = false;
        this.xrTable1.StylePriority.UseFont = false;
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell32,
            this.xrTableCell3,
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell2,
            this.xrTableCell4});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell1.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
        this.xrTableCell1.StylePriority.UseBackColor = false;
        this.xrTableCell1.StylePriority.UseBorderColor = false;
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UsePadding = false;
        this.xrTableCell1.Text = "Supplier Name";
        this.xrTableCell1.Weight = 0.5582983809311296D;
        // 
        // xrTableCell32
        // 
        this.xrTableCell32.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell32.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell32.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell32.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell32.Name = "xrTableCell32";
        this.xrTableCell32.StylePriority.UseBackColor = false;
        this.xrTableCell32.StylePriority.UseBorderColor = false;
        this.xrTableCell32.StylePriority.UseBorders = false;
        this.xrTableCell32.StylePriority.UseFont = false;
        this.xrTableCell32.StylePriority.UseTextAlignment = false;
        this.xrTableCell32.Text = "Opening Balance";
        this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell32.Weight = 0.28361788804935351D;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell3.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
        this.xrTableCell3.StylePriority.UseBackColor = false;
        this.xrTableCell3.StylePriority.UseBorderColor = false;
        this.xrTableCell3.StylePriority.UseBorders = false;
        this.xrTableCell3.StylePriority.UseFont = false;
        this.xrTableCell3.StylePriority.UsePadding = false;
        this.xrTableCell3.StylePriority.UseTextAlignment = false;
        this.xrTableCell3.Text = "Debit";
        this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell3.Weight = 0.23061955721413727D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell7.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrTableCell7.StylePriority.UseBackColor = false;
        this.xrTableCell7.StylePriority.UseBorderColor = false;
        this.xrTableCell7.StylePriority.UseBorders = false;
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UsePadding = false;
        this.xrTableCell7.StylePriority.UseTextAlignment = false;
        this.xrTableCell7.Text = "Credit";
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell7.Weight = 0.289113002642333D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell8.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell8.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell8.Multiline = true;
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrTableCell8.StylePriority.UseBackColor = false;
        this.xrTableCell8.StylePriority.UseBorderColor = false;
        this.xrTableCell8.StylePriority.UseBorders = false;
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UsePadding = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell8.Weight = 0.22347006636667388D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UseTextAlignment = false;
        this.xrTableCell2.Text = "Foreign";
        this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell2.Weight = 0.28349071142216775D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell4.Weight = 0.39381660104563126D;
        // 
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2});
        this.PageFooter.HeightF = 18.12502F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo2.Format = "Page{0}Of {1}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(801.7084F, 18.12496F);
        this.xrPageInfo2.StylePriority.UseBorders = false;
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // purchaseDataSet1
        // 
        this.purchaseDataSet1.DataSetName = "PurchaseDataSet";
        this.purchaseDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // InventoryDataSet1
        // 
        this.InventoryDataSet1.DataSetName = "InventoryDataSet";
        this.InventoryDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // accountsDataset1
        // 
        this.accountsDataset1.DataSetName = "AccountsDataset";
        this.accountsDataset1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // ReportFooter
        // 
        this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
        this.ReportFooter.HeightF = 22.91667F;
        this.ReportFooter.KeepTogether = true;
        this.ReportFooter.Name = "ReportFooter";
        this.ReportFooter.Visible = false;
        // 
        // xrTable3
        // 
        this.xrTable3.BorderColor = System.Drawing.Color.Silver;
        this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(1.862121F, 0F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
        this.xrTable3.SizeF = new System.Drawing.SizeF(641.0568F, 20.83333F);
        this.xrTable3.StylePriority.UseBorderColor = false;
        this.xrTable3.StylePriority.UseBorders = false;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell36});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 10, 0, 0, 100F);
        this.xrTableCell11.StylePriority.UseBorders = false;
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UsePadding = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "Report Total :";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell11.Weight = 1.1331139094187823D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Debit_Final")});
        this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0, 100F);
        this.xrTableCell12.StylePriority.UseBorders = false;
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.StylePriority.UsePadding = false;
        this.xrTableCell12.StylePriority.UseTextAlignment = false;
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell12.Summary = xrSummary1;
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell12.Weight = 0.25412523540646237D;
        this.xrTableCell12.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell12_PrintOnPage);
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Credit_Final")});
        this.xrTableCell13.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0, 100F);
        this.xrTableCell13.StylePriority.UseBorders = false;
        this.xrTableCell13.StylePriority.UseFont = false;
        this.xrTableCell13.StylePriority.UsePadding = false;
        this.xrTableCell13.StylePriority.UseTextAlignment = false;
        xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell13.Summary = xrSummary2;
        this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell13.Weight = 0.36039040103040343D;
        this.xrTableCell13.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell13_PrintOnPage);
        // 
        // xrTableCell36
        // 
        this.xrTableCell36.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Closing_Final")});
        this.xrTableCell36.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell36.Name = "xrTableCell36";
        this.xrTableCell36.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0, 100F);
        this.xrTableCell36.StylePriority.UseBorders = false;
        this.xrTableCell36.StylePriority.UseFont = false;
        this.xrTableCell36.StylePriority.UsePadding = false;
        this.xrTableCell36.StylePriority.UseTextAlignment = false;
        xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell36.Summary = xrSummary3;
        this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell36.Weight = 0.35128933874159135D;
        this.xrTableCell36.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell36_PrintOnPage);
        // 
        // GroupFooter2
        // 
        this.GroupFooter2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable8});
        this.GroupFooter2.HeightF = 31.25F;
        this.GroupFooter2.Name = "GroupFooter2";
        // 
        // xrTable8
        // 
        this.xrTable8.LocationFloat = new DevExpress.Utils.PointFloat(264.7057F, 10.00001F);
        this.xrTable8.Name = "xrTable8";
        this.xrTable8.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow13});
        this.xrTable8.SizeF = new System.Drawing.SizeF(802.2938F, 20F);
        // 
        // xrTableRow13
        // 
        this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell58,
            this.xrTableCell59,
            this.xrTableCell60,
            this.xrTableCell5,
            this.xrTableCell6});
        this.xrTableRow13.Name = "xrTableRow13";
        this.xrTableRow13.Weight = 1D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Opening_Final")});
        this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.StylePriority.UseBorderColor = false;
        this.xrTableCell9.StylePriority.UseBorders = false;
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrTableCell9.Summary = xrSummary4;
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell9.Weight = 0.31700702731946268D;
        this.xrTableCell9.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell9_PrintOnPage);
        // 
        // xrTableCell58
        // 
        this.xrTableCell58.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell58.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell58.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Debit_Final")});
        this.xrTableCell58.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell58.Name = "xrTableCell58";
        this.xrTableCell58.StylePriority.UseBorderColor = false;
        this.xrTableCell58.StylePriority.UseBorders = false;
        this.xrTableCell58.StylePriority.UseFont = false;
        this.xrTableCell58.StylePriority.UseTextAlignment = false;
        xrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrTableCell58.Summary = xrSummary5;
        this.xrTableCell58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell58.Weight = 0.25776931125365049D;
        this.xrTableCell58.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell58_BeforePrint);
        this.xrTableCell58.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell58_PrintOnPage);
        // 
        // xrTableCell59
        // 
        this.xrTableCell59.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell59.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell59.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Credit_Final")});
        this.xrTableCell59.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell59.Name = "xrTableCell59";
        this.xrTableCell59.StylePriority.UseBorderColor = false;
        this.xrTableCell59.StylePriority.UseBorders = false;
        this.xrTableCell59.StylePriority.UseFont = false;
        this.xrTableCell59.StylePriority.UseTextAlignment = false;
        xrSummary6.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrTableCell59.Summary = xrSummary6;
        this.xrTableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell59.Weight = 0.32314878615279913D;
        this.xrTableCell59.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell59_BeforePrint);
        this.xrTableCell59.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell59_PrintOnPage);
        // 
        // xrTableCell60
        // 
        this.xrTableCell60.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell60.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell60.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.Closing_Final")});
        this.xrTableCell60.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell60.Name = "xrTableCell60";
        this.xrTableCell60.StylePriority.UseBorderColor = false;
        this.xrTableCell60.StylePriority.UseBorders = false;
        this.xrTableCell60.StylePriority.UseFont = false;
        this.xrTableCell60.StylePriority.UseTextAlignment = false;
        xrSummary7.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrTableCell60.Summary = xrSummary7;
        this.xrTableCell60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell60.Weight = 0.24977846285826405D;
        this.xrTableCell60.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell60_BeforePrint);
        this.xrTableCell60.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell60_PrintOnPage);
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseBorderColor = false;
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.Weight = 0.31686444770378269D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_AllSupplierDetail_SelectRow.AgeingBalance_Final")});
        this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseBorderColor = false;
        this.xrTableCell6.StylePriority.UseBorders = false;
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        xrSummary8.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrTableCell6.Summary = xrSummary8;
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell6.Weight = 0.4401780440949189D;
        this.xrTableCell6.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell6_PrintOnPage);
        // 
        // AllSupplierRecord
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageHeader,
            this.PageFooter,
            this.ReportFooter,
            this.GroupFooter2});
        this.DataMember = "sp_Ac_AllSupplierDetail_SelectRow";
        this.DataSource = this.accountsDataset1;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.Landscape = true;
        this.Margins = new System.Drawing.Printing.Margins(23, 0, 0, 2);
        this.PageHeight = 850;
        this.PageWidth = 1100;
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
    private void xrTableCell12_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell12.Text != "")
        {
            try
            {
                xrTableCell12.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell12.Text);
            }
            catch
            {
                xrTableCell12.Text = "0";

            }
        }
        else
        {
            xrTableCell12.Text = "0";
        }
    }

    private void xrTableCell13_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell13.Text != "")
        {
            try
            {
                xrTableCell13.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell13.Text);
            }
            catch
            {
                xrTableCell13.Text = "0";

            }
        }
        else
        {
            xrTableCell13.Text = "0";
        }
    }

    private void xrTableCell33_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            // xrTableCell33.Text = Convert.ToDateTime(xrTableCell33.Text).ToString(objsys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrTableCell35_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //if (xrTableCell35.Text != "")
        //{
        //    try
        //    {
        //        xrTableCell35.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell35.Text);
        //    }
        //    catch
        //    {
        //        xrTableCell35.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrTableCell35.Text = "0";
        //}
    }

    private void xrTableCell36_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell36.Text != "")
        {
            try
            {
                xrTableCell36.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell36.Text);
            }
            catch
            {
                xrTableCell36.Text = "0";

            }
        }
        else
        {
            xrTableCell36.Text = "0";
        }
    }

    private void xrLabel5_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

    }

    private void xrLabel6_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

    }

    private void xrLabel7_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

    }

    private void xrTableCell37_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell37.Text != "")
        //{
        //    try
        //    {
        //        xrTableCell37.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell37.Text);
        //    }
        //    catch
        //    {
        //        xrTableCell37.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrTableCell37.Text = "0";
        //}
    }

    private void xrTableCell38_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell38.Text != "")
        //{
        //    try
        //    {
        //        xrTableCell38.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell38.Text);
        //    }
        //    catch
        //    {
        //        xrTableCell38.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrTableCell38.Text = "0";
        //}
    }

    private void xrTableCell39_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

    }

    private void xrTableCell25_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell25.Text != "")
        //{
        //    try
        //    {
        //        xrTableCell25.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell25.Text);
        //    }
        //    catch
        //    {
        //        xrTableCell25.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrTableCell25.Text = "0";
        //}
    }

    private void xrTableCell60_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

        if (xrTableCell60.Text != "")
        {
            try
            {
                xrTableCell60.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell60.Text);
            }
            catch
            {
                xrTableCell60.Text = "0";

            }
        }
        else
        {
            xrTableCell60.Text = "0";
        }
    }

    //For Ageing Detail
    public static string[] ForGetAgeingDetail(string strSupplierId, string strConString)
    {
        string[] Objarr = new string[6];


        string xrcell0_30 = string.Empty;
        string xrcell31_60 = string.Empty;
        string xrcell61_90 = string.Empty;
        string xrcell91_180 = string.Empty;
        string xrcell181_365 = string.Empty;
        string xrcellabove_365 = string.Empty;

        Ac_Ageing_Detail ObjAgeingDetail = new Ac_Ageing_Detail(strConString);


        if (strSupplierId != "" && strSupplierId != "0")
        {
            //string sql = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id from ac_ageing_detail group by Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,Field2  having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0 and other_account_no='" + txtCustomerName.Text.Split('/')[1].ToString() + "' and Field2='RV'";
            //DataTable dtAgeing = da.return_DataTable(sql);

            DataTable dtAgeingDetail = ObjAgeingDetail.GetAgeingDetailAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtAgeingDetail.Rows.Count > 0)
            {
                dtAgeingDetail = new DataView(dtAgeingDetail, "other_account_no='" + strSupplierId + "' and Field2='RV'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //For 0-30 Days
            try
            {
                DateTime dt30DaysDate = new DateTime();
                if (DateTime.Now.Month == 1)
                {
                    dt30DaysDate = new DateTime(DateTime.Now.Year - 1, 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else
                {
                    dt30DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }

                DataTable dt30Days = new DataView(dtAgeingDetail, "Invoice_Date <= '" + DateTime.Now.ToString() + "' and  Invoice_Date >= '" + dt30DaysDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt30Days.Rows.Count > 0)
                {
                    double InvoiceAmt = 0;
                    double PaidAmt = 0;

                    for (int i = 0; i < dt30Days.Rows.Count; i++)
                    {
                        InvoiceAmt = InvoiceAmt + float.Parse(dt30Days.Rows[i]["Due_Amount"].ToString());
                        PaidAmt = PaidAmt + float.Parse(dt30Days.Rows[i]["Paid_Receive_Amount"].ToString());
                    }
                    xrcell0_30 = (float.Parse(InvoiceAmt.ToString()) - float.Parse(PaidAmt.ToString())).ToString();
                }
                else
                {
                    xrcell0_30 = "0.00";
                }
            }
            catch
            {
                xrcell0_30 = "0.00";
            }


            //For 31-60 Days
            try
            {
                DateTime dt31DaysDate = new DateTime();
                DateTime dt60DaysDate = new DateTime();
                if (DateTime.Now.Month == 1)
                {
                    dt31DaysDate = new DateTime(DateTime.Now.Year - 1, 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt60DaysDate = new DateTime(DateTime.Now.Year - 1, 11, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 2)
                {
                    dt31DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt60DaysDate = new DateTime(DateTime.Now.Year - 1, 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else
                {
                    dt31DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt60DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 2, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }

                DataTable dt60Days = new DataView(dtAgeingDetail, "Invoice_Date <= '" + dt31DaysDate.ToString() + "' and  Invoice_Date >= '" + dt60DaysDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt60Days.Rows.Count > 0)
                {
                    double InvoiceAmt60 = 0;
                    double PaidAmt60 = 0;
                    for (int i = 0; i < dt60Days.Rows.Count; i++)
                    {
                        InvoiceAmt60 = InvoiceAmt60 + float.Parse(dt60Days.Rows[i]["Due_Amount"].ToString());
                        PaidAmt60 = PaidAmt60 + float.Parse(dt60Days.Rows[i]["Paid_Receive_Amount"].ToString());

                    }
                    xrcell31_60 = (float.Parse(InvoiceAmt60.ToString()) - float.Parse(PaidAmt60.ToString())).ToString();
                }
                else
                {
                    xrcell31_60 = "0.00";
                }
            }
            catch
            {
                xrcell31_60 = "0.00";
            }

            //For 61-90 Days
            try
            {
                DateTime dt61DaysDate = new DateTime();
                DateTime dt90DaysDate = new DateTime();
                if (DateTime.Now.Month == 1)
                {
                    dt61DaysDate = new DateTime(DateTime.Now.Year - 1, 11, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt90DaysDate = new DateTime(DateTime.Now.Year - 1, 10, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 2)
                {
                    dt61DaysDate = new DateTime(DateTime.Now.Year - 1, 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt90DaysDate = new DateTime(DateTime.Now.Year - 1, 11, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 3)
                {
                    dt61DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 2, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt90DaysDate = new DateTime(DateTime.Now.Year - 1, 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else
                {
                    dt61DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 2, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt90DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 3, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }

                DataTable dt90Days = new DataView(dtAgeingDetail, "Invoice_Date <= '" + dt61DaysDate.ToString() + "' and  Invoice_Date >= '" + dt90DaysDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt90Days.Rows.Count > 0)
                {
                    double InvoiceAmt90 = 0;
                    double PaidAmt90 = 0;
                    for (int i = 0; i < dt90Days.Rows.Count; i++)
                    {
                        InvoiceAmt90 = InvoiceAmt90 + float.Parse(dt90Days.Rows[i]["Due_Amount"].ToString());
                        PaidAmt90 = PaidAmt90 + float.Parse(dt90Days.Rows[i]["Paid_Receive_Amount"].ToString());
                    }

                    xrcell61_90 = (float.Parse(InvoiceAmt90.ToString()) - float.Parse(PaidAmt90.ToString())).ToString();
                }
                else
                {
                    xrcell61_90 = "0.00";
                }
            }
            catch
            {
                xrcell61_90 = "0.00";
            }

            //For 91-180 Days
            try
            {
                DateTime dt91DaysDate = new DateTime();
                DateTime dt180DaysDate = new DateTime();
                if (DateTime.Now.Month == 1)
                {
                    dt91DaysDate = new DateTime(DateTime.Now.Year - 1, 10, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt180DaysDate = new DateTime(DateTime.Now.Year - 1, 7, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 2)
                {
                    dt91DaysDate = new DateTime(DateTime.Now.Year - 1, 11, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt180DaysDate = new DateTime(DateTime.Now.Year - 1, 8, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 3)
                {
                    dt91DaysDate = new DateTime(DateTime.Now.Year - 1, 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt180DaysDate = new DateTime(DateTime.Now.Year - 1, 9, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 4)
                {
                    dt91DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 3, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt180DaysDate = new DateTime(DateTime.Now.Year - 1, 10, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 5)
                {
                    dt91DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 3, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt180DaysDate = new DateTime(DateTime.Now.Year - 1, 11, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 6)
                {
                    dt91DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 3, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt180DaysDate = new DateTime(DateTime.Now.Year - 1, 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else
                {
                    dt91DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 3, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt180DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 6, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }

                DataTable dt180Days = new DataView(dtAgeingDetail, "Invoice_Date <= '" + dt91DaysDate.ToString() + "' and  Invoice_Date >= '" + dt180DaysDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt180Days.Rows.Count > 0)
                {
                    double InvoiceAmt180 = 0;
                    double PaidAmt180 = 0;
                    for (int i = 0; i < dt180Days.Rows.Count; i++)
                    {
                        InvoiceAmt180 = InvoiceAmt180 + float.Parse(dt180Days.Rows[i]["Due_Amount"].ToString());
                        PaidAmt180 = PaidAmt180 + float.Parse(dt180Days.Rows[i]["Paid_Receive_Amount"].ToString());
                    }
                    xrcell91_180 = (float.Parse(InvoiceAmt180.ToString()) - float.Parse(PaidAmt180.ToString())).ToString();
                }
                else
                {
                    xrcell91_180 = "0.00";
                }
            }
            catch
            {
                xrcell91_180 = "0.00";
            }

            //For 181-365 Days
            try
            {
                DateTime dt181DaysDate = new DateTime();
                DateTime dt365DaysDate = new DateTime();
                if (DateTime.Now.Month == 1)
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year - 1, 4, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year - 1, 1, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 2)
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year - 1, 5, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year - 1, 2, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 3)
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year - 1, 6, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year - 1, 3, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 4)
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year - 1, 7, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year - 1, 4, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 5)
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year - 1, 8, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year - 1, 3, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 6)
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year - 1, 9, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year - 1, 4, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 7)
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year - 1, 10, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year - 1, 5, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 8)
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year - 1, 11, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year - 1, 6, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else if (DateTime.Now.Month == 9)
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year - 1, 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year - 1, 7, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                else
                {
                    dt181DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 9, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    dt365DaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                }
                DataTable dt365Days = new DataView(dtAgeingDetail, "Invoice_Date <= '" + dt181DaysDate.ToString() + "' and  Invoice_Date >= '" + dt365DaysDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt365Days.Rows.Count > 0)
                {
                    double InvoiceAmt365 = 0;
                    double PaidAmt365 = 0;
                    for (int i = 0; i < dt365Days.Rows.Count; i++)
                    {
                        InvoiceAmt365 = InvoiceAmt365 + float.Parse(dt365Days.Rows[i]["Due_Amount"].ToString());
                        PaidAmt365 = PaidAmt365 + float.Parse(dt365Days.Rows[i]["Paid_Receive_Amount"].ToString());
                    }
                    xrcell181_365 = (float.Parse(InvoiceAmt365.ToString()) - float.Parse(PaidAmt365.ToString())).ToString();
                }
                else
                {
                    xrcell181_365 = "0.00";
                }
            }
            catch
            {
                xrcell181_365 = "0.00";
            }

            //For 181-365 Days
            try
            {
                DateTime dt365DaysAboveDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 12, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                DataTable dt365DaysAbove = new DataView(dtAgeingDetail, "Invoice_Date <= '" + dt365DaysAboveDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt365DaysAbove.Rows.Count > 0)
                {
                    double InvoiceAmtAbove365 = 0;
                    double PaidAmtAbove365 = 0;
                    for (int i = 0; i < dt365DaysAbove.Rows.Count; i++)
                    {
                        InvoiceAmtAbove365 = InvoiceAmtAbove365 + float.Parse(dt365DaysAbove.Rows[i]["Due_Amount"].ToString());
                        PaidAmtAbove365 = PaidAmtAbove365 + float.Parse(dt365DaysAbove.Rows[i]["Paid_Receive_Amount"].ToString());
                    }
                    xrcellabove_365 = (float.Parse(InvoiceAmtAbove365.ToString()) - float.Parse(PaidAmtAbove365.ToString())).ToString();
                }
                else
                {
                    xrcellabove_365 = "0.00";
                }
            }
            catch
            {
                xrcellabove_365 = "0.00";
            }

            Objarr[0] = xrcell0_30;
            Objarr[1] = xrcell31_60;
            Objarr[2] = xrcell61_90;
            Objarr[3] = xrcell91_180;
            Objarr[4] = xrcell181_365;
            Objarr[5] = xrcellabove_365;

        }

        return Objarr;
    }

    private void xrcell0_30_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //xrcell0_30.Text = ForGetAgeingDetail(GetCurrentColumnValue("Other_Account_No").ToString())[0].ToString();
    }

    private void xrcell31_60_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //xrcell31_60.Text = ForGetAgeingDetail(GetCurrentColumnValue("Other_Account_No").ToString())[1].ToString();
    }

    private void xrcell61_90_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //xrcell61_90.Text = ForGetAgeingDetail(GetCurrentColumnValue("Other_Account_No").ToString())[2].ToString();
    }

    private void xrcell91_180_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //xrcell91_180.Text = ForGetAgeingDetail(GetCurrentColumnValue("Other_Account_No").ToString())[3].ToString();
    }

    private void xrcell181_365_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //xrcell181_365.Text = ForGetAgeingDetail(GetCurrentColumnValue("Other_Account_No").ToString())[4].ToString();
    }

    private void xrcellabove_365_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //xrcellabove_365.Text = ForGetAgeingDetail(GetCurrentColumnValue("Other_Account_No").ToString())[5].ToString();
    }

    private void xrTableCell51_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //xrTableCell51.Text = GetOpeningBalance(GetCurrentColumnValue("Other_Account_No").ToString());
    }

    //For Opening Balance
    public string GetOpeningBalance(string strSupplierId)
    {
        
        string strOpening = string.Empty;

        DataTable dtCus = ObjSupplier.GetSupplierAllDataBySupplierId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), strSupplierId);
        if (dtCus.Rows.Count > 0)
        {
            strOpening = dtCus.Rows[0]["O_Db_Amount"].ToString();
        }
        else
        {
            strOpening = "0.00";
        }
        return strOpening;
    }

    private void xrLabel7_PrintOnPage_1(object sender, PrintOnPageEventArgs e)
    {
        if (xrLabel7.Text != "")
        {
            try
            {
                xrLabel7.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel7.Text);
            }
            catch
            {
                xrLabel7.Text = "0";
            }
        }
        else
        {
            xrLabel7.Text = "0";
        }
    }



    private void xrLabel9_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrLabel9.Text != "")
        {
            try
            {
                xrLabel9.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel9.Text);
            }
            catch
            {
                xrLabel9.Text = "0";

            }
        }
        else
        {
            xrLabel9.Text = "0";
        }
    }
    private void xrTableCell9_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell9.Text != "")
        {
            try
            {
                xrTableCell9.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell9.Text);
            }
            catch
            {
                xrTableCell9.Text = "0";
            }
        }
        else
        {
            xrTableCell9.Text = "0";
        }
    }
    private void xrTableCell6_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell6.Text != "")
        {
            try
            {
                xrTableCell6.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell6.Text);
            }
            catch
            {
                xrTableCell6.Text = "0";
            }
        }
        else
        {
            xrTableCell6.Text = "0";
        }
    }

    private void xrTableCell58_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell58.Text != "")
        {
            try
            {
                xrTableCell58.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell58.Text);
            }
            catch
            {
                xrTableCell58.Text = "0";

            }
        }
        else
        {
            xrTableCell58.Text = "0";
        }
    }

    private void xrTableCell59_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell59.Text != "")
        {
            try
            {
                xrTableCell59.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell59.Text);
            }
            catch
            {
                xrTableCell59.Text = "0";

            }
        }
        else
        {
            xrTableCell59.Text = "0";
        }
    }
    private void xrLabel4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrLabel4.Text != "")
        {
            try
            {
                xrLabel4.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel4.Text);
            }
            catch
            {
                xrLabel4.Text = "0";
                xrLabel4.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel4.Text);
            }
        }
        else
        {
            xrLabel4.Text = "0";
            xrLabel4.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel4.Text);
        }
    }

    private void xrLabel5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrLabel5.Text != "")
        {
            try
            {
                xrLabel5.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel5.Text);
            }
            catch
            {
                xrLabel5.Text = "0";
                xrLabel5.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel5.Text);
            }
        }
        else
        {
            xrLabel5.Text = "0";
            xrLabel5.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel5.Text);
        }
    }
    private void xrLabel6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrLabel6.Text != "")
        {
            try
            {
                xrLabel6.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel6.Text);
            }
            catch
            {
                xrLabel6.Text = "0";
                xrLabel6.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel6.Text);
            }
        }
        else
        {
            xrLabel6.Text = "0";
            xrLabel6.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel6.Text);
        }
    }
    private void xrLabel7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrLabel7.Text != "")
        {
            try
            {
                xrLabel7.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel7.Text);
            }
            catch
            {
                xrLabel7.Text = "0";
                xrLabel7.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel7.Text);
            }
        }
        else
        {
            xrLabel7.Text = "0";
            xrLabel7.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel7.Text);
        }
    }

    private void xrLabel8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        string strCurrencyId = string.Empty;
        if (xrLabel8.Text != "")
        {
            try
            {
                if (GetCurrentColumnValue("Supplier_Id").ToString() != "" && GetCurrentColumnValue("Supplier_Id").ToString() != "0")
                {
                    DataTable dtSupplier = ObjSupplier.GetSupplierAllDataBySupplierId(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["BrandId"].ToString(), GetCurrentColumnValue("Supplier_Id").ToString());
                    if (dtSupplier.Rows.Count > 0)
                    {
                        strCurrencyId = dtSupplier.Rows[0]["Field3"].ToString();
                    }
                }

                xrLabel8.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel8.Text);
                if (strCurrencyId != "")
                {
                    xrLabel8.Text = objsys.GetCurencyConversionForInv(strCurrencyId, xrLabel8.Text);
                    xrLabel8.Text = SystemParameter.GetCurrencySmbol(strCurrencyId, xrLabel8.Text,_strConString);
                }
            }
            catch
            {
                xrLabel8.Text = "0";
                //xrLabel8.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel8.Text);
            }
        }
        else
        {
            xrLabel8.Text = "0";
            xrLabel8.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel8.Text);
        }
    }

    private void xrLabel9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrLabel9.Text != "")
        {
            try
            {
                xrLabel9.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel9.Text);
            }
            catch
            {
                xrLabel9.Text = "0";
                xrLabel9.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel9.Text);
            }
        }
        else
        {
            xrLabel9.Text = "0";
            xrLabel9.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel9.Text);
        }
    }







    private void xrTableCell58_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell58.Text != "")
        {
            try
            {
                xrTableCell58.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell58.Text);
            }
            catch
            {
                xrTableCell58.Text = "0";
                xrTableCell58.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell58.Text);
            }
        }
        else
        {
            xrTableCell58.Text = "0";
            xrTableCell58.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell58.Text);
        }
    }

    private void xrTableCell59_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell59.Text != "")
        {
            try
            {
                xrTableCell59.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell59.Text);
            }
            catch
            {
                xrTableCell59.Text = "0";
                xrTableCell59.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell59.Text);
            }
        }
        else
        {
            xrTableCell59.Text = "0";
            xrTableCell59.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell59.Text);
        }
    }

    private void xrTableCell60_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell60.Text != "")
        {
            try
            {
                xrTableCell60.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell60.Text);
            }
            catch
            {
                xrTableCell60.Text = "0";
                xrTableCell60.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell60.Text);
            }
        }
        else
        {
            xrTableCell60.Text = "0";
            xrTableCell60.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell60.Text);
        }
    }
}
