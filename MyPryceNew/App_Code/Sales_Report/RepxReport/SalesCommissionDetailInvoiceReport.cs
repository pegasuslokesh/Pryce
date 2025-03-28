using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

/// <summary>
/// Summary description for PurchaseRequestPrint
/// </summary>
public class SalesCommissionDetailInvoiceReport : DevExpress.XtraReports.UI.XtraReport
{
    SystemParameter objSys = null;
    CurrencyMaster objCurrency = null;
    Set_AddressChild Objaddress = null;
    LocationMaster ObjLocationMaster = null;
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell8;
    private PageFooterBand PageFooter;
    private PurchaseDataSet purchaseDataSet1;
    private SalesDataSet SalesDataSet1;
    private XRPageInfo xrPageInfo2;
    private XRTableCell xrTableCell16;
    private XRLabel xrLabel14;
    private XRLabel xrLabelUser;
    private XRPageInfo xrPageInfo1;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell7;
    private XRTableRow xrTableRow4;
    private XRTable xrTable4;
    private XRLabel xrLabel4;
    private ReportFooterBand ReportFooter;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel5;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell25;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell24;
    private GroupHeaderBand GroupHeader1;
    private GroupHeaderBand GroupHeader2;
    private XRLabel xrLabel8;
    private XRLabel xrLabel10;
    private XRLabel xrLabel13;
    private XRLabel xrLabel12;
    private XRLabel xrLabel11;
    private GroupFooterBand GroupFooter1;
    private GroupFooterBand GroupFooter2;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel1;
    private XRTable xrTable5;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell14;
    private XRPanel xrPanel1;
    private XRTable xrTable6;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private PageHeaderBand PageHeader;
    private XRLabel xrLabel3;
    private XRLabel xrLabel2;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public SalesCommissionDetailInvoiceReport(string strConString)
    {
        InitializeComponent();
        objSys = new SystemParameter(strConString);
        objCurrency = new CurrencyMaster(strConString);
        Objaddress = new Set_AddressChild(strConString);
        ObjLocationMaster = new LocationMaster(strConString);
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
       
    }
    public void setcompanyname(string companyname)
    {
        xrLabel5.Text = companyname;
    }
    public void SetImage(string Url)
    {
        xrPictureBox1.ImageUrl = Url;
    }
    public void setCompanyArebicName(string ArebicName)
    {
        //        xrPictureBox3.ImageUrl = "~/Images/Arabic.png";
        //xrLabel10.Text = ArebicName;

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
    public void setPersontype(string strpersonheader)
    {
        //xrLabel1.Text = strpersonheader;
    }

    public void setdateCriteria(string strDateCriteria)
    {
        xrLabel5.Text = strDateCriteria;
    }

    public void setReportTitle(string strpersonheader)
    {
        xrLabel4.Text = strpersonheader;
    }

    public void setDateCriteria(string strTitle)
    {
        xrLabel4.Text = strTitle;
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
    public void setUserName(string UserName)
    {
        xrLabelUser.Text = UserName;
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string resourceFileName = "SalesCommissionDetailInvoiceReport.resx";
        DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.purchaseDataSet1 = new PurchaseDataSet();
        this.SalesDataSet1 = new SalesDataSet();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabelUser = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.SalesDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
        this.Detail.HeightF = 20.08333F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrTable3
        // 
        this.xrTable3.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable3.Font = new System.Drawing.Font("Verdana", 6.75F);
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
        this.xrTable3.SizeF = new System.Drawing.SizeF(800F, 19F);
        this.xrTable3.StylePriority.UseBackColor = false;
        this.xrTable3.StylePriority.UseBorderColor = false;
        this.xrTable3.StylePriority.UseBorders = false;
        this.xrTable3.StylePriority.UseFont = false;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell26,
            this.xrTableCell8,
            this.xrTableCell16});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Voucher_No")});
        this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorderColor = false;
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.Text = "xrTableCell4";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell4.Weight = 1.6069651868978658D;
        this.xrTableCell4.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell4_BeforePrint);
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Voucher_Date")});
        this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseBorderColor = false;
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "xrTableCell5";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell5.Weight = 1.982631500480323D;
        this.xrTableCell5.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell5_BeforePrint_1);
        // 
        // xrTableCell26
        // 
        this.xrTableCell26.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell26.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.ProductCode")});
        this.xrTableCell26.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell26.Name = "xrTableCell26";
        this.xrTableCell26.StylePriority.UseBorderColor = false;
        this.xrTableCell26.StylePriority.UseBorders = false;
        this.xrTableCell26.StylePriority.UseFont = false;
        this.xrTableCell26.StylePriority.UseTextAlignment = false;
        this.xrTableCell26.Text = "xrTableCell26";
        this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell26.Weight = 3.0816617660498991D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell8.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Salesperson")});
        this.xrTableCell8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrTableCell8.StylePriority.UseBorderColor = false;
        this.xrTableCell8.StylePriority.UseBorders = false;
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UsePadding = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        this.xrTableCell8.Text = "xrTableCell8";
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell8.Weight = 3.42552331448235D;
        this.xrTableCell8.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell8_BeforePrint);
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell16.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Commission_Amount")});
        this.xrTableCell16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseBorderColor = false;
        this.xrTableCell16.StylePriority.UseBorders = false;
        this.xrTableCell16.StylePriority.UseFont = false;
        this.xrTableCell16.StylePriority.UseTextAlignment = false;
        this.xrTableCell16.Text = "xrTableCell16";
        this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell16.Weight = 1.9081447166677625D;
        this.xrTableCell16.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell16_BeforePrint);
        // 
        // purchaseDataSet1
        // 
        this.purchaseDataSet1.DataSetName = "PurchaseDataSet";
        this.purchaseDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // SalesDataSet1
        // 
        this.SalesDataSet1.DataSetName = "SalesDataSet";
        this.SalesDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
        this.BottomMargin.HeightF = 11F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLabel14,
            this.xrLabelUser,
            this.xrPageInfo2});
        this.PageFooter.HeightF = 44.62503F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo1
        // 
        this.xrPageInfo1.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
        this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(335.1464F, 14.08335F);
        this.xrPageInfo1.Name = "xrPageInfo1";
        this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo1.SizeF = new System.Drawing.SizeF(259.208F, 18.04161F);
        this.xrPageInfo1.StylePriority.UseTextAlignment = false;
        this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel14
        // 
        this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(0F, 14.49994F);
        this.xrLabel14.Name = "xrLabel14";
        this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel14.SizeF = new System.Drawing.SizeF(75.78073F, 17.62505F);
        this.xrLabel14.StylePriority.UseFont = false;
        this.xrLabel14.Text = "Created By:";
        // 
        // xrLabelUser
        // 
        this.xrLabelUser.LocationFloat = new DevExpress.Utils.PointFloat(75.78074F, 14.49994F);
        this.xrLabelUser.Name = "xrLabelUser";
        this.xrLabelUser.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabelUser.SizeF = new System.Drawing.SizeF(221.125F, 17.62505F);
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo2.Format = "Page{0}Of {1}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(639.0935F, 14.49992F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(147.8783F, 18.12499F);
        this.xrPageInfo2.StylePriority.UseBorders = false;
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell13.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell13.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrTableCell13.StylePriority.UseBackColor = false;
        this.xrTableCell13.StylePriority.UseBorderColor = false;
        this.xrTableCell13.StylePriority.UseBorders = false;
        this.xrTableCell13.StylePriority.UseFont = false;
        this.xrTableCell13.StylePriority.UsePadding = false;
        this.xrTableCell13.StylePriority.UseTextAlignment = false;
        this.xrTableCell13.Text = "Comm. Amount";
        this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell13.Weight = 1.8990346730093117D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell12.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell12.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell12.StylePriority.UseBackColor = false;
        this.xrTableCell12.StylePriority.UseBorderColor = false;
        this.xrTableCell12.StylePriority.UseBorders = false;
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.StylePriority.UsePadding = false;
        this.xrTableCell12.StylePriority.UseTextAlignment = false;
        this.xrTableCell12.Text = "Product Code";
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell12.Weight = 3.0669475389063452D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell9.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell9.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell9.Multiline = true;
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.StylePriority.UseBackColor = false;
        this.xrTableCell9.StylePriority.UseBorderColor = false;
        this.xrTableCell9.StylePriority.UseBorders = false;
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.Text = "Voucher Date";
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell9.Weight = 1.9731649186443958D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell7.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseBackColor = false;
        this.xrTableCell7.StylePriority.UseBorderColor = false;
        this.xrTableCell7.StylePriority.UseBorders = false;
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UseTextAlignment = false;
        this.xrTableCell7.Text = "Voucher No.";
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell7.Weight = 1.5992930634444273D;
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell9,
            this.xrTableCell12,
            this.xrTableCell25,
            this.xrTableCell13});
        this.xrTableRow4.Name = "xrTableRow4";
        this.xrTableRow4.Weight = 1D;
        // 
        // xrTableCell25
        // 
        this.xrTableCell25.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell25.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell25.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell25.Name = "xrTableCell25";
        this.xrTableCell25.StylePriority.UseBackColor = false;
        this.xrTableCell25.StylePriority.UseBorders = false;
        this.xrTableCell25.StylePriority.UseFont = false;
        this.xrTableCell25.StylePriority.UseTextAlignment = false;
        this.xrTableCell25.Text = "Sales Person";
        this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell25.Weight = 3.409168569976841D;
        // 
        // xrTable4
        // 
        this.xrTable4.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable4.Font = new System.Drawing.Font("Verdana", 8F);
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 10.00001F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
        this.xrTable4.SizeF = new System.Drawing.SizeF(800F, 18F);
        this.xrTable4.StylePriority.UseBackColor = false;
        this.xrTable4.StylePriority.UseBorderColor = false;
        this.xrTable4.StylePriority.UseBorders = false;
        this.xrTable4.StylePriority.UseFont = false;
        // 
        // xrLabel4
        // 
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75.7291F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(471.8179F, 19.875F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "xrLabel4";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // ReportFooter
        // 
        this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable5});
        this.ReportFooter.HeightF = 38.39588F;
        this.ReportFooter.Name = "ReportFooter";
        // 
        // xrTable5
        // 
        this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable5.Name = "xrTable5";
        this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
        this.xrTable5.SizeF = new System.Drawing.SizeF(799.0001F, 25F);
        // 
        // xrTableRow5
        // 
        this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell14});
        this.xrTableRow5.Name = "xrTableRow5";
        this.xrTableRow5.Weight = 1D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseBorders = false;
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "GRAND TOTAL :";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell11.Weight = 23.360910710649929D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Commission_Amount")});
        this.xrTableCell14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseBorders = false;
        this.xrTableCell14.StylePriority.UseFont = false;
        this.xrTableCell14.StylePriority.UseTextAlignment = false;
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell14.Summary = xrSummary1;
        this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell14.Weight = 4.38015985982594D;
        this.xrTableCell14.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell14_PrintOnPage);
        // 
        // xrTable1
        // 
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
        this.xrTable1.SizeF = new System.Drawing.SizeF(799.0001F, 25F);
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell24});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "PERSON WISE TOTAL :";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell1.Weight = 23.360912829774232D;
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Commission_Amount")});
        this.xrTableCell24.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.StylePriority.UseBorders = false;
        this.xrTableCell24.StylePriority.UseFont = false;
        this.xrTableCell24.StylePriority.UseTextAlignment = false;
        xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrTableCell24.Summary = xrSummary2;
        this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell24.Weight = 4.3801577407016374D;
        this.xrTableCell24.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell24_PrintOnPage);
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1,
            this.xrLabel1,
            this.xrPictureBox1,
            this.xrLabel5,
            this.xrLabel4});
        this.ReportHeader.HeightF = 117.0624F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrPanel1.BorderWidth = 2F;
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 113.3124F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(799.0002F, 2F);
        this.xrPanel1.StylePriority.UseBorders = false;
        this.xrPanel1.StylePriority.UseBorderWidth = false;
        // 
        // xrLabel1
        // 
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 52.7291F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(471.8179F, 23F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.Text = "Sales Commission Detail Report By Invoice Wise";
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(703.1701F, 10.00001F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(95.83F, 45.96F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        // 
        // xrLabel5
        // 
        this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 10.00001F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(471.8179F, 23F);
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.Text = "xrLabel5";
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8});
        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Commission_Person", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader1.HeightF = 36.45837F;
        this.GroupHeader1.Level = 1;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // xrLabel8
        // 
        this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Commission_Person")});
        this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 10.00001F);
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(325.0001F, 23F);
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.Text = "xrLabel8";
        // 
        // GroupHeader2
        // 
        this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel13,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel10});
        this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Salesperson", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Invoice_No", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Invoice_Date", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader2.HeightF = 51.45829F;
        this.GroupHeader2.Name = "GroupHeader2";
        // 
        // xrLabel3
        // 
        this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.CustomerName")});
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(129.1666F, 0.1249631F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(400.3954F, 23F);
        this.xrLabel3.StylePriority.UseFont = false;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 0F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(129.1666F, 23F);
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.Text = "Customer Name :";
        // 
        // xrLabel13
        // 
        this.xrLabel13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Invoice_Date")});
        this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(444.568F, 28.45829F);
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(130.2808F, 23F);
        this.xrLabel13.StylePriority.UseFont = false;
        this.xrLabel13.Text = "xrLabel13";
        this.xrLabel13.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel13_BeforePrint);
        // 
        // xrLabel12
        // 
        this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(333.2345F, 28.45829F);
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(111.3335F, 23F);
        this.xrLabel12.StylePriority.UseFont = false;
        this.xrLabel12.StylePriority.UseTextAlignment = false;
        this.xrLabel12.Text = "Invoice Date :";
        this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel11
        // 
        this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Invoice_No")});
        this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(108.7086F, 28.45829F);
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(142.7807F, 23F);
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.Text = "xrLabel11";
        // 
        // xrLabel10
        // 
        this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 28.45829F);
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(107.1668F, 23F);
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.StylePriority.UseTextAlignment = false;
        this.xrLabel10.Text = "Invoice No. :";
        this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // GroupFooter1
        // 
        this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
        this.GroupFooter1.HeightF = 36.45833F;
        this.GroupFooter1.Name = "GroupFooter1";
        // 
        // xrTable6
        // 
        this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 0F);
        this.xrTable6.Name = "xrTable6";
        this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
        this.xrTable6.SizeF = new System.Drawing.SizeF(799.0002F, 25F);
        // 
        // xrTableRow6
        // 
        this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17,
            this.xrTableCell18});
        this.xrTableRow6.Name = "xrTableRow6";
        this.xrTableRow6.Weight = 1D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseFont = false;
        this.xrTableCell17.StylePriority.UseTextAlignment = false;
        this.xrTableCell17.Text = "INVOICE NO. WISE TOTAL :";
        this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell17.Weight = 13.169963783891916D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Detail_Report.Commission_Amount")});
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseTextAlignment = false;
        xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrTableCell18.Summary = xrSummary3;
        this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell18.Weight = 2.4693628125281668D;
        this.xrTableCell18.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell18_PrintOnPage);
        // 
        // GroupFooter2
        // 
        this.GroupFooter2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
        this.GroupFooter2.HeightF = 38.54167F;
        this.GroupFooter2.Level = 1;
        this.GroupFooter2.Name = "GroupFooter2";
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
        this.PageHeader.HeightF = 33.33333F;
        this.PageHeader.Name = "PageHeader";
        // 
        // SalesCommissionDetailInvoiceReport
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.ReportFooter,
            this.ReportHeader,
            this.GroupHeader1,
            this.GroupHeader2,
            this.GroupFooter1,
            this.GroupFooter2,
            this.PageHeader});
        this.DataMember = "sp_Inv_SalesCommission_Detail_Report";
        this.DataSource = this.SalesDataSet1;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.Margins = new System.Drawing.Printing.Margins(27, 23, 0, 11);
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.SalesDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    this.xrTableCell11.Text = Convert.ToDateTime(xrTableCell11.Text).ToString("dd/MM/yyyy");
        //}
        //catch
        //{


    }

    private void xrTableCell5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //if (GetCurrentColumnValue("EProductName") != null)
        //{
        //    if (GetCurrentColumnValue("EProductName").ToString().Trim() == "")
        //    {
        //        xrTableCell5.Text = GetCurrentColumnValue("SuggestedProductName").ToString();

        //    }
        //}
        //else
        //{
        //    try
        //    {
        //        xrTableCell5.Text = GetCurrentColumnValue("SuggestedProductName").ToString();
        //    }
        //    catch
        //    {
        //        xrTableCell5.Text = "";
        //    }
        //}

    }



    private void xrTableCell3_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
       

    }

    private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell8.Text = Math.Round(Convert.ToDouble(xrTableCell8.Text), 0).ToString();
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell16_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell16.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell16.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell15_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //DataTable dt = objCurrency.GetCurrencyMasterById(GetCurrentColumnValue("Currency_Id").ToString());
        //if (dt.Rows.Count > 0)
        //{
        //    xrTableCell15.Text = "Unit Price" + "(" + dt.Rows[0]["Currency_Symbol"].ToString() + ")";

        //}
    }

    private void xrLabel27_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
       
    }

    private void xrLabel26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        
    }

    private void xrLabel24_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
       
    }

    private void xrLabel24_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
    //    string CompanyAddress = string.Empty;
    //    DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Customer", xrLabel24.Text);
    //    if (DtAddress.Rows.Count > 0)
    //    {
    //        CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
    //        if (DtAddress.Rows[0]["Address"].ToString() != "")
    //        {
    //            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
    //        }
    //        if (DtAddress.Rows[0]["Street"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Street"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["Street"].ToString();
    //            }
    //        }
    //        if (DtAddress.Rows[0]["Block"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Block"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["Block"].ToString();
    //            }
    //        }
    //        if (DtAddress.Rows[0]["Avenue"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Avenue"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["Avenue"].ToString();
    //            }
    //        }

    //        if (DtAddress.Rows[0]["CityId"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["CityId"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["CityId"].ToString();
    //            }

    //        }
    //        if (DtAddress.Rows[0]["StateId"].ToString() != "")
    //        {


    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["StateId"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["StateId"].ToString();
    //            }
    //        }
    //        if (DtAddress.Rows[0]["CountryId"].ToString() != "")
    //        {
    //            string LocationName = string.Empty;
    //            DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(GetCurrentColumnValue("Company_Id").ToString(), DtAddress.Rows[0]["CountryId"].ToString());
    //            if (DtLocation.Rows.Count > 0)
    //            {


    //                if (CompanyAddress != "")
    //                {
    //                    CompanyAddress = CompanyAddress + "," + LocationName;
    //                }
    //                else
    //                {
    //                    CompanyAddress = LocationName;
    //                }
    //            }

    //        }
    //        if (DtAddress.Rows[0]["PinCode"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["PinCode"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["PinCode"].ToString();
    //            }

    //        }


    //    }
    //    xrLabel24.Text = CompanyAddress;
    //}

    //private void xrLabel27_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
    //{
    //    string Companytelno = string.Empty;
    //    DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Customer", xrLabel27.Text);
    //    if (DtAddress.Rows.Count > 0)
    //    {
    //        if (DtAddress.Rows[0]["PhoneNo1"].ToString() != "")
    //        {

    //            Companytelno = DtAddress.Rows[0]["PhoneNo1"].ToString();
    //        }
    //        //if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
    //        //{
    //        //    if (Companytelno != "")
    //        //    {
    //        //        Companytelno = Companytelno + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
    //        //    }
    //        //    else
    //        //    {
    //        //        Companytelno = DtAddress.Rows[0]["PhoneNo2"].ToString();
    //        //    }
    //        //}
    //        if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
    //        {
    //            if (Companytelno != "")
    //            {
    //                Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo1"].ToString();
    //            }
    //            else
    //            {
    //                Companytelno = DtAddress.Rows[0]["MobileNo1"].ToString();
    //            }
    //        }
    //        //if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
    //        //{
    //        //    if (Companytelno != "")
    //        //    {
    //        //        Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
    //        //    }
    //        //    else
    //        //    {
    //        //        Companytelno = DtAddress.Rows[0]["MobileNo2"].ToString();
    //        //    }
    //        //}
    //    }
    //    xrLabel27.Text = Companytelno;
    }

    private void xrLabel26_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //string CompanyFaxno = string.Empty;
        //DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Customer", xrLabel26.Text);
        //if (DtAddress.Rows.Count > 0)
        //{
        //    if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
        //    {
        //        CompanyFaxno = DtAddress.Rows[0]["FaxNo"].ToString();
        //    }
        //}
        //xrLabel26.Text = CompanyFaxno;
    }

    private void xrTableCell3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell3.Text = Convert.ToDateTime(xrTableCell3.Text).ToString(objSys.SetDateFormat());
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell18.Text = Convert.ToDateTime(xrTableCell18.Text).ToString(objSys.SetDateFormat());
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell4.Text = Convert.ToDateTime(xrTableCell4.Text).ToString(objSys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrTableCell5_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell5.Text = Convert.ToDateTime(xrTableCell5.Text).ToString(objSys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrTableCell6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell6.Text = Convert.ToDateTime(xrTableCell6.Text).ToString(objSys.SetDateFormat());
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell29_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell29.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell29.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell19.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell19.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell35_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell35.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell35.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell2_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell2.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell2.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell22_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell22.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell22.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell23_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell23.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell23.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell24_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrTableCell24.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell24.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell34_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell34.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell34.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell33_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell33.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell33.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell20_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell20.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell20.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell21_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell21.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell21.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell31_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell31.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell31.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell32_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell32.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell32.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrLabel13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrLabel13.Text = Convert.ToDateTime(xrLabel13.Text).ToString(objSys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrTableCell3_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell3.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell3.Text);
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell14_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrTableCell14.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell14.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell18_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrTableCell18.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell18.Text);
        }
        catch
        {
        }
    }

   


}
