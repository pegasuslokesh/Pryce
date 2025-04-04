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
public class SupplierAgeingPrint : DevExpress.XtraReports.UI.XtraReport
{
    CompanyMaster ObjCompany = null;
    LocationMaster objLocation = null;
    SystemParameter objsys = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    Set_CustomerMaster ObjCoustmer = null;

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
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell7;
    private ReportFooterBand ReportFooter;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell34;
    private XRLabel xrLabel1;
    private XRLabel xrLabel4;
    private XRLabel xrLabel3;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public SupplierAgeingPrint(string strConString)
    {
        InitializeComponent();
        ObjCompany = new CompanyMaster(strConString);
        objLocation = new LocationMaster(strConString);
        objsys = new SystemParameter(strConString);
        ObjAgeingDetail = new Ac_Ageing_Detail(strConString);
        ObjCoustmer = new Set_CustomerMaster(strConString);
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
    public void setcompanyAddress(string Address)
    {
        //xrLabel1.Text = Address;
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

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string resourceFileName = "SupplierAgeingPrint.resx";
        DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
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
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.purchaseDataSet1 = new PurchaseDataSet();
        this.InventoryDataSet1 = new InventoryDataSet();
        this.accountsDataset1 = new AccountsDataset();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
        this.Detail.HeightF = 19.79332F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrTable2
        // 
        this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
        this.xrTable2.SizeF = new System.Drawing.SizeF(799.8463F, 17.71F);
        this.xrTable2.StylePriority.UseBorders = false;
        this.xrTable2.StylePriority.UseFont = false;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell33,
            this.xrTableCell6,
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell35});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Invoice_No")});
        this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
        this.xrTableCell4.StylePriority.UseBorderColor = false;
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UsePadding = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.Text = "Ac/No.";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell4.Weight = 0.3906999872823067D;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Po_No")});
        this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
        this.xrTableCell5.StylePriority.UseBorderColor = false;
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.StylePriority.UsePadding = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "Account Name";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell5.Weight = 0.35167853731366966D;
        // 
        // xrTableCell33
        // 
        this.xrTableCell33.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell33.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Invoice_Date")});
        this.xrTableCell33.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell33.Name = "xrTableCell33";
        this.xrTableCell33.StylePriority.UseBorderColor = false;
        this.xrTableCell33.StylePriority.UseBorders = false;
        this.xrTableCell33.StylePriority.UseFont = false;
        this.xrTableCell33.StylePriority.UseTextAlignment = false;
        this.xrTableCell33.Text = "xrTableCell33";
        this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell33.Weight = 0.33912773676772739D;
        this.xrTableCell33.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell33_BeforePrint);
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Due_Date")});
        this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
        this.xrTableCell6.StylePriority.UseBorderColor = false;
        this.xrTableCell6.StylePriority.UseBorders = false;
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UsePadding = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "Naration";
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell6.Weight = 0.37414644758447335D;
        this.xrTableCell6.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell6_BeforePrint);
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell8.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Payment_Terms")});
        this.xrTableCell8.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.StylePriority.UseBorderColor = false;
        this.xrTableCell8.StylePriority.UseBorders = false;
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        this.xrTableCell8.Text = "xrTableCell8";
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell8.Weight = 0.41913111413342136D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell9.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Days_Overdue")});
        this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrTableCell9.StylePriority.UseBorderColor = false;
        this.xrTableCell9.StylePriority.UseBorders = false;
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UsePadding = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell9.Weight = 0.35099169762923321D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell10.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Invoice_Amount")});
        this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrTableCell10.StylePriority.UseBorderColor = false;
        this.xrTableCell10.StylePriority.UseBorders = false;
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.StylePriority.UsePadding = false;
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        this.xrTableCell10.Text = "Credit Amount";
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell10.Weight = 0.36482875825220862D;
        this.xrTableCell10.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell10_BeforePrint);
        // 
        // xrTableCell35
        // 
        this.xrTableCell35.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell35.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Due_Amount")});
        this.xrTableCell35.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell35.Name = "xrTableCell35";
        this.xrTableCell35.StylePriority.UseBorderColor = false;
        this.xrTableCell35.StylePriority.UseBorders = false;
        this.xrTableCell35.StylePriority.UseFont = false;
        this.xrTableCell35.StylePriority.UseTextAlignment = false;
        this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell35.Weight = 0.40939572103695981D;
        this.xrTableCell35.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell35_BeforePrint);
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
        this.BottomMargin.HeightF = 17.83333F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrPanel1});
        this.ReportHeader.HeightF = 93.33344F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrLabel1
        // 
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(376.6411F, 71.25009F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(425.0672F, 18F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
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
        this.xrPanel1.SizeF = new System.Drawing.SizeF(801.7084F, 56.25008F);
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
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(690.6952F, 1.333329F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(95.83319F, 45.95832F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrReportTitle
        // 
        this.xrReportTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrReportTitle.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
        this.xrReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(298.4034F, 29.08336F);
        this.xrReportTitle.Name = "xrReportTitle";
        this.xrReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrReportTitle.SizeF = new System.Drawing.SizeF(281.2782F, 23F);
        this.xrReportTitle.StylePriority.UseBorders = false;
        this.xrReportTitle.StylePriority.UseFont = false;
        this.xrReportTitle.StylePriority.UseTextAlignment = false;
        this.xrReportTitle.Text = "SUPPLIER AGEING";
        this.xrReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
        this.PageHeader.HeightF = 30.00001F;
        this.PageHeader.Name = "PageHeader";
        // 
        // xrTable1
        // 
        this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(1.861898F, 10.00001F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
        this.xrTable1.SizeF = new System.Drawing.SizeF(799.8463F, 20F);
        this.xrTable1.StylePriority.UseBorders = false;
        this.xrTable1.StylePriority.UseFont = false;
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell11,
            this.xrTableCell2,
            this.xrTableCell12,
            this.xrTableCell32,
            this.xrTableCell3,
            this.xrTableCell7,
            this.xrTableCell34});
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
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "Invoice No.";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell1.Weight = 0.38371654808102634D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell11.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell11.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseBackColor = false;
        this.xrTableCell11.StylePriority.UseBorderColor = false;
        this.xrTableCell11.StylePriority.UseBorders = false;
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "Refrence Order/Invoice Number";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell11.Weight = 0.35167849590782857D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell2.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell2.Multiline = true;
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
        this.xrTableCell2.StylePriority.UseBackColor = false;
        this.xrTableCell2.StylePriority.UseBorderColor = false;
        this.xrTableCell2.StylePriority.UseBorders = false;
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UsePadding = false;
        this.xrTableCell2.StylePriority.UseTextAlignment = false;
        this.xrTableCell2.Text = "Invoice Date\r\n";
        this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell2.Weight = 0.3391276102735799D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell12.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell12.Multiline = true;
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.StylePriority.UseBackColor = false;
        this.xrTableCell12.StylePriority.UseBorders = false;
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.StylePriority.UseTextAlignment = false;
        this.xrTableCell12.Text = "Due Date";
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell12.Weight = 0.37414650657339094D;
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
        this.xrTableCell32.Text = "Payment Terms";
        this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell32.Weight = 0.4191313425400775D;
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
        this.xrTableCell3.Text = "Days Overdue";
        this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell3.Weight = 0.35099191525963791D;
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
        this.xrTableCell7.Text = "Invoice Amount";
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell7.Weight = 0.36482854616931992D;
        // 
        // xrTableCell34
        // 
        this.xrTableCell34.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell34.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell34.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell34.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell34.Name = "xrTableCell34";
        this.xrTableCell34.StylePriority.UseBackColor = false;
        this.xrTableCell34.StylePriority.UseBorderColor = false;
        this.xrTableCell34.StylePriority.UseBorders = false;
        this.xrTableCell34.StylePriority.UseFont = false;
        this.xrTableCell34.StylePriority.UseTextAlignment = false;
        this.xrTableCell34.Text = "Balance Remaining";
        this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell34.Weight = 0.4163790351951393D;
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
            this.xrTable3,
            this.xrLabel4,
            this.xrLabel3});
        this.ReportFooter.HeightF = 72.29156F;
        this.ReportFooter.KeepTogether = true;
        this.ReportFooter.Name = "ReportFooter";
        // 
        // xrTable3
        // 
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(499.8463F, 0F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
        this.xrTable3.SizeF = new System.Drawing.SizeF(300F, 25F);
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell15});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.Text = "TOTAL :";
        this.xrTableCell13.Weight = 0.935799560546875D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Invoice_Amount")});
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseTextAlignment = false;
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell14.Summary = xrSummary1;
        this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell14.Weight = 0.97268859863281243D;
        this.xrTableCell14.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell14_BeforePrint);
        this.xrTableCell14.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell14_PrintOnPage);
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_SupplierAgeing_SelectRow.Due_Amount")});
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.StylePriority.UseTextAlignment = false;
        xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell15.Summary = xrSummary2;
        this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell15.Weight = 1.0915118408203126D;
        this.xrTableCell15.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell15_BeforePrint);
        this.xrTableCell15.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell15_PrintOnPage);
        // 
        // xrLabel4
        // 
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(572.5928F, 53.125F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(150.8411F, 18.4167F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "Manager Signature";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel3
        // 
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(32.51286F, 53.125F);
        this.xrLabel3.Multiline = true;
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(165.4167F, 18.4167F);
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "Authorised Signature";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // SupplierAgeingPrint
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageHeader,
            this.PageFooter,
            this.ReportFooter});
        this.DataMember = "sp_Ac_SupplierAgeing_SelectRow";
        this.DataSource = this.accountsDataset1;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.Margins = new System.Drawing.Printing.Margins(23, 0, 0, 18);
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    this.xrTableCell11.Text = Convert.ToDateTime(xrTableCell11.Text).ToString(objsys.SetDateFormat());
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //if (xrTableCell5.Text != "")
        //{

        //    try
        //    {
        //        xrTableCell5.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell5.Text);
        //    }
        //    catch
        //    {

        //    }
        //}

    }

    private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {



        //xrTableCell8.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyId").ToString(), xrTableCell8.Text);



    }

    private void xrTableCell22_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {



        //try
        //{
        //    this.xrTableCell22.Text = Convert.ToDateTime(xrTableCell22.Text).ToString(objsys.SetDateFormat());
        //}
        //catch
        //{
        //}
    }
    private void xrLabel3_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrLabel3.Text != "")
        //{
        //    try
        //    {
        //        xrLabel3.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel3.Text);
        //    }
        //    catch
        //    {
        //        xrLabel3.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrLabel3.Text = "0";
        //}
    }

    private void xrLabel4_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrLabel4.Text != "")
        //{
        //    try
        //    {
        //        xrLabel4.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrLabel4.Text);
        //    }
        //    catch
        //    {
        //        xrLabel4.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrLabel4.Text = "0";
        //}
    }
    private void xrLabel5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrLabel5.Text = Convert.ToDateTime(xrLabel5.Text).ToString(objsys.SetDateFormat());
        //}
        //catch
        //{
        //}
    }
    private void xrTableCell10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell10.Text != "")
        {
            try
            {
                xrTableCell10.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell10.Text);
            }
            catch
            {
                xrTableCell10.Text = "0";

            }
        }
        else
        {
            xrTableCell10.Text = "0";
        }
    }
    private void xrTableCell14_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell14.Text != "")
        {
            try
            {
                xrTableCell14.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell14.Text);
            }
            catch
            {
                xrTableCell14.Text = "0";
            }
        }
        else
        {
            xrTableCell14.Text = "0";
        }
    }
    private void xrTableCell15_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell15.Text != "")
        {
            try
            {
                xrTableCell15.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell15.Text);
            }
            catch
            {
                xrTableCell15.Text = "0";
            }
        }
        else
        {
            xrTableCell15.Text = "0";
        }
    }
    private void xrTableCell33_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell33.Text = Convert.ToDateTime(xrTableCell33.Text).ToString(objsys.SetDateFormat());
        }
        catch
        {

        }
    }
    private void xrTableCell6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell6.Text = Convert.ToDateTime(xrTableCell6.Text).ToString(objsys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrTableCell35_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell35.Text != "")
        {
            try
            {
                xrTableCell35.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell35.Text);
            }
            catch
            {
                xrTableCell35.Text = "0";

            }
        }
        else
        {
            xrTableCell35.Text = "0";
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
        //if (xrTableCell39.Text != "")
        //{

        //    try
        //    {
        //        if (HttpContext.Current.Session["DtReportStatement"] != null)
        //        {


        //            DataTable dtStatementRecord = (DataTable)HttpContext.Current.Session["DtReportStatement"];


        //            dtStatementRecord = new DataView(dtStatementRecord, "Location_Id =" + GetCurrentColumnValue("Location_Id").ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        //            if (dtStatementRecord.Rows.Count > 0)
        //            {
        //                xrTableCell39.Text = dtStatementRecord.Rows[dtStatementRecord.Rows.Count - 1]["BalanceAmount"].ToString();
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }







        //    try
        //    {
        //        xrTableCell39.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell39.Text);
        //    }
        //    catch
        //    {
        //        xrTableCell39.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrTableCell39.Text = "0";
        //}
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

    //For Ageing Detail
    public static string[] ForGetAgeingDetail(string strCustomerId, string strConString)
    {
        string[] Objarr = new string[6];


        string xrcell0_30 = string.Empty;
        string xrcell31_60 = string.Empty;
        string xrcell61_90 = string.Empty;
        string xrcell91_180 = string.Empty;
        string xrcell181_365 = string.Empty;
        string xrcellabove_365 = string.Empty;

        Ac_Ageing_Detail ObjAgeingDetail = new Ac_Ageing_Detail(strConString);


        if (strCustomerId != "" && strCustomerId != "0")
        {
            //string sql = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id from ac_ageing_detail group by Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,Field2  having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0 and other_account_no='" + txtCustomerName.Text.Split('/')[1].ToString() + "' and Field2='RV'";
            //DataTable dtAgeing = da.return_DataTable(sql);

            DataTable dtAgeingDetail = ObjAgeingDetail.GetAgeingDetailAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtAgeingDetail.Rows.Count > 0)
            {
                dtAgeingDetail = new DataView(dtAgeingDetail, "other_account_no='" + strCustomerId + "' and Field2='RV'", "", DataViewRowState.CurrentRows).ToTable();
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
    //For Opening Balance
    public string GetOpeningBalance(string strCustomerId)
    {
        string strOpening = string.Empty;

        DataTable dtCus = ObjCoustmer.GetCustomerAllDataByCustomerId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), strCustomerId);
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

    private void xrTableCell14_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell14.Text != "")
        {
            try
            {
                xrTableCell14.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell14.Text);
            }
            catch
            {
                xrTableCell14.Text = "0";
            }
        }
        else
        {
            xrTableCell14.Text = "0";
        }
    }
    private void xrTableCell15_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell15.Text != "")
        {
            try
            {
                xrTableCell15.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell15.Text);
            }
            catch
            {
                xrTableCell15.Text = "0";
            }
        }
        else
        {
            xrTableCell15.Text = "0";
        }
    }
}
