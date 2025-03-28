using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

/// <summary>
/// Summary description for PurchaseRequestPrint
/// </summary>
public class SalesCommissionHeaderVoucherReport : DevExpress.XtraReports.UI.XtraReport
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
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell8;
    private PageFooterBand PageFooter;
    private PurchaseDataSet purchaseDataSet1;
    private SalesDataSet SalesDataSet1;
    private XRPageInfo xrPageInfo2;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell16;
    private XRLabel xrLabel14;
    private XRLabel xrLabelUser;
    private XRPageInfo xrPageInfo1;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell7;
    private XRTableRow xrTableRow4;
    private XRTable xrTable4;
    private XRLabel xrLabel4;
    private ReportFooterBand ReportFooter;
    private PageHeaderBand PageHeader;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel5;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell30;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell32;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell33;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel1;
    private XRPanel xrPanel1;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public SalesCommissionHeaderVoucherReport(string strConString)
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
    public void setdateCriteria(string strDateCriteria)
    {
        xrLabel5.Text = strDateCriteria;
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

    public void setReportTitle(string strpersonheader)
    {
        //xrLabel4.Text = strpersonheader;
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
        string resourceFileName = "SalesCommissionHeaderVoucherReport.resx";
        DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary6 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary7 = new DevExpress.XtraReports.UI.XRSummary();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
        this.purchaseDataSet1 = new PurchaseDataSet();
        this.SalesDataSet1 = new SalesDataSet();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabelUser = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.SalesDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
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
        this.xrTable3.SizeF = new System.Drawing.SizeF(1047.208F, 19F);
        this.xrTable3.StylePriority.UseBackColor = false;
        this.xrTable3.StylePriority.UseBorderColor = false;
        this.xrTable3.StylePriority.UseBorders = false;
        this.xrTable3.StylePriority.UseFont = false;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell14,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell26,
            this.xrTableCell8,
            this.xrTableCell16,
            this.xrTableCell29,
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21,
            this.xrTableCell31,
            this.xrTableCell32});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell14.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Voucher_No")});
        this.xrTableCell14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseBorderColor = false;
        this.xrTableCell14.StylePriority.UseBorders = false;
        this.xrTableCell14.StylePriority.UseFont = false;
        this.xrTableCell14.StylePriority.UseTextAlignment = false;
        this.xrTableCell14.Text = "xrTableCell14";
        this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell14.Weight = 0.93213676334907558D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Voucher_Date")});
        this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorderColor = false;
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.Text = "xrTableCell4";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell4.Weight = 0.94674099770922315D;
        this.xrTableCell4.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell4_BeforePrint);
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.From_Date")});
        this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseBorderColor = false;
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "xrTableCell5";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell5.Weight = 0.863697433830029D;
        this.xrTableCell5.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell5_BeforePrint_1);
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.To_Date")});
        this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell6.StylePriority.UseBorderColor = false;
        this.xrTableCell6.StylePriority.UseBorders = false;
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UsePadding = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "xrTableCell6";
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell6.Weight = 1.0246522607617432D;
        this.xrTableCell6.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell6_BeforePrint);
        // 
        // xrTableCell26
        // 
        this.xrTableCell26.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell26.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.CommissionType")});
        this.xrTableCell26.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell26.Name = "xrTableCell26";
        this.xrTableCell26.StylePriority.UseBorderColor = false;
        this.xrTableCell26.StylePriority.UseBorders = false;
        this.xrTableCell26.StylePriority.UseFont = false;
        this.xrTableCell26.StylePriority.UseTextAlignment = false;
        this.xrTableCell26.Text = "xrTableCell26";
        this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell26.Weight = 0.64197745515195181D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell8.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.CommissionPerson")});
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
        this.xrTableCell8.Weight = 1.1492783096511383D;
        this.xrTableCell8.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell8_BeforePrint);
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell16.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Total_Sales_Amount")});
        this.xrTableCell16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseBorderColor = false;
        this.xrTableCell16.StylePriority.UseBorders = false;
        this.xrTableCell16.StylePriority.UseFont = false;
        this.xrTableCell16.StylePriority.UseTextAlignment = false;
        this.xrTableCell16.Text = "xrTableCell16";
        this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell16.Weight = 1.0135433017763109D;
        this.xrTableCell16.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell16_BeforePrint);
        // 
        // xrTableCell29
        // 
        this.xrTableCell29.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell29.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Total_Comission_Amount")});
        this.xrTableCell29.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell29.Name = "xrTableCell29";
        this.xrTableCell29.StylePriority.UseBorderColor = false;
        this.xrTableCell29.StylePriority.UseBorders = false;
        this.xrTableCell29.StylePriority.UseFont = false;
        this.xrTableCell29.StylePriority.UseTextAlignment = false;
        this.xrTableCell29.Text = "xrTableCell29";
        this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell29.Weight = 1.0214859746548306D;
        this.xrTableCell29.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell29_BeforePrint);
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell19.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.TotalReturnCommission")});
        this.xrTableCell19.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.StylePriority.UseBorderColor = false;
        this.xrTableCell19.StylePriority.UseBorders = false;
        this.xrTableCell19.StylePriority.UseFont = false;
        this.xrTableCell19.StylePriority.UseTextAlignment = false;
        this.xrTableCell19.Text = "xrTableCell19";
        this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell19.Weight = 1.1425920551057716D;
        this.xrTableCell19.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell19_BeforePrint);
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell20.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.TotalActualCommission")});
        this.xrTableCell20.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.StylePriority.UseBorderColor = false;
        this.xrTableCell20.StylePriority.UseBorders = false;
        this.xrTableCell20.StylePriority.UseFont = false;
        this.xrTableCell20.StylePriority.UseTextAlignment = false;
        this.xrTableCell20.Text = "xrTableCell20";
        this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell20.Weight = 0.95690583978293553D;
        this.xrTableCell20.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell20_BeforePrint);
        // 
        // xrTableCell21
        // 
        this.xrTableCell21.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell21.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Total_Paid_Amount")});
        this.xrTableCell21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell21.Name = "xrTableCell21";
        this.xrTableCell21.StylePriority.UseBorderColor = false;
        this.xrTableCell21.StylePriority.UseBorders = false;
        this.xrTableCell21.StylePriority.UseFont = false;
        this.xrTableCell21.StylePriority.UseTextAlignment = false;
        this.xrTableCell21.Text = "xrTableCell21";
        this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell21.Weight = 0.68794580174516651D;
        this.xrTableCell21.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell21_BeforePrint);
        // 
        // xrTableCell31
        // 
        this.xrTableCell31.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell31.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Total_Remain_Amount")});
        this.xrTableCell31.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell31.Name = "xrTableCell31";
        this.xrTableCell31.StylePriority.UseBorderColor = false;
        this.xrTableCell31.StylePriority.UseBorders = false;
        this.xrTableCell31.StylePriority.UseFont = false;
        this.xrTableCell31.StylePriority.UseTextAlignment = false;
        this.xrTableCell31.Text = "xrTableCell31";
        this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell31.Weight = 0.7289861848722492D;
        this.xrTableCell31.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell31_BeforePrint);
        // 
        // xrTableCell32
        // 
        this.xrTableCell32.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell32.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.TotalUnpaidBalace")});
        this.xrTableCell32.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTableCell32.Name = "xrTableCell32";
        this.xrTableCell32.StylePriority.UseBorderColor = false;
        this.xrTableCell32.StylePriority.UseBorders = false;
        this.xrTableCell32.StylePriority.UseFont = false;
        this.xrTableCell32.StylePriority.UseTextAlignment = false;
        this.xrTableCell32.Text = "xrTableCell32";
        this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell32.Weight = 0.89498410618777569D;
        this.xrTableCell32.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell32_BeforePrint);
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
        this.PageFooter.HeightF = 32.12502F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo1
        // 
        this.xrPageInfo1.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
        this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(393.3127F, 14.00007F);
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
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(899.3297F, 13.91665F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(147.8783F, 18.12499F);
        this.xrPageInfo2.StylePriority.UseBorders = false;
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrTableCell28
        // 
        this.xrTableCell28.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell28.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell28.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell28.Multiline = true;
        this.xrTableCell28.Name = "xrTableCell28";
        this.xrTableCell28.StylePriority.UseBackColor = false;
        this.xrTableCell28.StylePriority.UseBorders = false;
        this.xrTableCell28.StylePriority.UseFont = false;
        this.xrTableCell28.StylePriority.UseTextAlignment = false;
        this.xrTableCell28.Text = "Total Comm.";
        this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell28.Weight = 1.0195304549750719D;
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell15.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell15.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.StylePriority.UseBackColor = false;
        this.xrTableCell15.StylePriority.UseBorderColor = false;
        this.xrTableCell15.StylePriority.UseBorders = false;
        this.xrTableCell15.StylePriority.UseFont = false;
        this.xrTableCell15.StylePriority.UseTextAlignment = false;
        this.xrTableCell15.Text = "Total Sales Amount";
        this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell15.Weight = 1.0582737754280818D;
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
        this.xrTableCell13.Text = "Comm. Person";
        this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell13.Weight = 1.1004047900811909D;
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
        this.xrTableCell12.Text = "To Date";
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell12.Weight = 1.0226904440353395D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell10.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell10.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.StylePriority.UseBackColor = false;
        this.xrTableCell10.StylePriority.UseBorderColor = false;
        this.xrTableCell10.StylePriority.UseBorders = false;
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        this.xrTableCell10.Text = "From Date";
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell10.Weight = 0.8620430845826127D;
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
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell9.Weight = 0.94492756527398547D;
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
        this.xrTableCell7.Weight = 0.93035140792921867D;
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell12,
            this.xrTableCell25,
            this.xrTableCell13,
            this.xrTableCell15,
            this.xrTableCell28,
            this.xrTableCell11,
            this.xrTableCell27,
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell30});
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
        this.xrTableCell25.Text = "Type";
        this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell25.Weight = 0.64074758512695884D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell11.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.StylePriority.UseBackColor = false;
        this.xrTableCell11.StylePriority.UseBorders = false;
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.StylePriority.UseTextAlignment = false;
        this.xrTableCell11.Text = "Total Return Comm.";
        this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell11.Weight = 1.1404034492875328D;
        // 
        // xrTableCell27
        // 
        this.xrTableCell27.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell27.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell27.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell27.Name = "xrTableCell27";
        this.xrTableCell27.StylePriority.UseBackColor = false;
        this.xrTableCell27.StylePriority.UseBorders = false;
        this.xrTableCell27.StylePriority.UseFont = false;
        this.xrTableCell27.StylePriority.UseTextAlignment = false;
        this.xrTableCell27.Text = "Actual Comm.";
        this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell27.Weight = 0.955072647449502D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell17.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell17.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseBackColor = false;
        this.xrTableCell17.StylePriority.UseBorders = false;
        this.xrTableCell17.StylePriority.UseFont = false;
        this.xrTableCell17.StylePriority.UseTextAlignment = false;
        this.xrTableCell17.Text = "Paid ";
        this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell17.Weight = 0.68662901076775618D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell18.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseBackColor = false;
        this.xrTableCell18.StylePriority.UseBorders = false;
        this.xrTableCell18.StylePriority.UseFont = false;
        this.xrTableCell18.StylePriority.UseTextAlignment = false;
        this.xrTableCell18.Text = "Remain";
        this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell18.Weight = 0.72758965488138638D;
        // 
        // xrTableCell30
        // 
        this.xrTableCell30.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell30.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell30.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell30.Name = "xrTableCell30";
        this.xrTableCell30.StylePriority.UseBackColor = false;
        this.xrTableCell30.StylePriority.UseBorders = false;
        this.xrTableCell30.StylePriority.UseFont = false;
        this.xrTableCell30.StylePriority.UseTextAlignment = false;
        this.xrTableCell30.Text = "Unpaid Balance";
        this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell30.Weight = 0.85894489416268538D;
        // 
        // xrTable4
        // 
        this.xrTable4.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable4.Font = new System.Drawing.Font("Verdana", 8F);
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 12.00002F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
        this.xrTable4.SizeF = new System.Drawing.SizeF(1044.208F, 18F);
        this.xrTable4.StylePriority.UseBackColor = false;
        this.xrTable4.StylePriority.UseBorderColor = false;
        this.xrTable4.StylePriority.UseBorders = false;
        this.xrTable4.StylePriority.UseFont = false;
        // 
        // xrLabel4
        // 
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1.000023F, 65.31255F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(457.8176F, 19.875F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "xrLabel4";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // ReportFooter
        // 
        this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
        this.ReportFooter.HeightF = 109.2292F;
        this.ReportFooter.Name = "ReportFooter";
        // 
        // xrTable1
        // 
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
        this.xrTable1.SizeF = new System.Drawing.SizeF(1043.208F, 25F);
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell35,
            this.xrTableCell34,
            this.xrTableCell33,
            this.xrTableCell2,
            this.xrTableCell22,
            this.xrTableCell23,
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
        this.xrTableCell1.Text = "TOTAL :";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell1.Weight = 12.867239079312141D;
        // 
        // xrTableCell35
        // 
        this.xrTableCell35.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Total_Sales_Amount")});
        this.xrTableCell35.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell35.Name = "xrTableCell35";
        this.xrTableCell35.StylePriority.UseBorders = false;
        this.xrTableCell35.StylePriority.UseFont = false;
        this.xrTableCell35.StylePriority.UseTextAlignment = false;
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell35.Summary = xrSummary1;
        this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell35.Weight = 2.3510854062278135D;
        this.xrTableCell35.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell35_PrintOnPage);
        // 
        // xrTableCell34
        // 
        this.xrTableCell34.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Total_Comission_Amount")});
        this.xrTableCell34.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell34.Name = "xrTableCell34";
        this.xrTableCell34.StylePriority.UseBorders = false;
        this.xrTableCell34.StylePriority.UseFont = false;
        this.xrTableCell34.StylePriority.UseTextAlignment = false;
        xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell34.Summary = xrSummary2;
        this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell34.Weight = 2.3695078156772715D;
        this.xrTableCell34.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell34_PrintOnPage);
        // 
        // xrTableCell33
        // 
        this.xrTableCell33.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.TotalReturnCommission")});
        this.xrTableCell33.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell33.Name = "xrTableCell33";
        this.xrTableCell33.StylePriority.UseBorders = false;
        this.xrTableCell33.StylePriority.UseFont = false;
        this.xrTableCell33.StylePriority.UseTextAlignment = false;
        xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell33.Summary = xrSummary3;
        this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell33.Weight = 2.6504337152307271D;
        this.xrTableCell33.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell33_PrintOnPage);
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.TotalActualCommission")});
        this.xrTableCell2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.StylePriority.UseBorders = false;
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UseTextAlignment = false;
        xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell2.Summary = xrSummary4;
        this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell2.Weight = 2.2197007887577698D;
        this.xrTableCell2.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell2_PrintOnPage);
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Total_Paid_Amount")});
        this.xrTableCell22.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.StylePriority.UseBorders = false;
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.StylePriority.UseTextAlignment = false;
        xrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell22.Summary = xrSummary5;
        this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell22.Weight = 1.5958086698445153D;
        this.xrTableCell22.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell22_PrintOnPage);
        // 
        // xrTableCell23
        // 
        this.xrTableCell23.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.Total_Remain_Amount")});
        this.xrTableCell23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell23.Name = "xrTableCell23";
        this.xrTableCell23.StylePriority.UseBorders = false;
        this.xrTableCell23.StylePriority.UseFont = false;
        this.xrTableCell23.StylePriority.UseTextAlignment = false;
        xrSummary6.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell23.Summary = xrSummary6;
        this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell23.Weight = 1.6910072634598636D;
        this.xrTableCell23.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell23_PrintOnPage);
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesCommission_Header_VoucherReport.TotalUnpaidBalace")});
        this.xrTableCell24.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.StylePriority.UseBorders = false;
        this.xrTableCell24.StylePriority.UseFont = false;
        this.xrTableCell24.StylePriority.UseTextAlignment = false;
        xrSummary7.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell24.Summary = xrSummary7;
        this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell24.Weight = 1.9962878319657675D;
        this.xrTableCell24.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell24_PrintOnPage);
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
        this.PageHeader.HeightF = 30.00002F;
        this.PageHeader.Name = "PageHeader";
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1,
            this.xrLabel1,
            this.xrPictureBox1,
            this.xrLabel5,
            this.xrLabel4});
        this.ReportHeader.HeightF = 117.0625F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrPanel1.BorderWidth = 2F;
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(1.000023F, 111.0626F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(1043.208F, 2.249969F);
        this.xrPanel1.StylePriority.UseBorders = false;
        this.xrPanel1.StylePriority.UseBorderWidth = false;
        // 
        // xrLabel1
        // 
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(1.000023F, 42.31254F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(457.8176F, 23F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.Text = "Sales Commission Header Report By Voucher Wise";
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(940F, 10.00001F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(95.83F, 45.96F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        // 
        // xrLabel5
        // 
        this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(1F, 10.00001F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(457.8176F, 23F);
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.Text = "xrLabel5";
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // SalesCommissionHeaderVoucherReport
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.ReportFooter,
            this.PageHeader,
            this.ReportHeader});
        this.DataMember = "DtSalesCommission";
        this.DataSource = this.SalesDataSet1;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.Landscape = true;
        this.Margins = new System.Drawing.Printing.Margins(27, 23, 0, 11);
        this.PageHeight = 850;
        this.PageWidth = 1100;
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.SalesDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
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
        try
        {
            xrTableCell6.Text = Convert.ToDateTime(xrTableCell6.Text).ToString(objSys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrTableCell29_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell29.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell29.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell19.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell19.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell35_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrTableCell35.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell35.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell2_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrTableCell2.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell2.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell22_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrTableCell22.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell22.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell23_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrTableCell23.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell23.Text);
        }
        catch
        {
        }
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
        try
        {
            xrTableCell34.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell34.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell33_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrTableCell33.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell33.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell20_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell20.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell20.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell21_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell21.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell21.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell31_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell31.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell31.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell32_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell32.Text = objSys.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell32.Text);
        }
        catch
        {
        }
    }

   


}
