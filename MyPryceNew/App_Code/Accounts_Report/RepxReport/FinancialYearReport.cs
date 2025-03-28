using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

/// <summary>
/// Summary description for PurchaseRequestPrint
/// </summary>
public class FinancialYearReport : DevExpress.XtraReports.UI.XtraReport
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
    private PageFooterBand PageFooter;
    private PurchaseDataSet purchaseDataSet1;
    private SalesDataSet SalesDataSet1;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrLabel14;
    private XRLabel xrLabelUser;
    private XRPageInfo xrPageInfo1;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell9;
    private XRTableRow xrTableRow4;
    private XRTable xrTable4;
    private XRLabel xrLabel4;
    private PageHeaderBand PageHeader;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel5;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell25;
    private InventoryDataSet inventoryDataSet1;
    private AccountsDataset accountsDataset1;
    private GroupHeaderBand GroupHeader1;
    private XRLabel xrLabel2;
    private GroupHeaderBand GroupHeader2;
    private XRLabel xrLabel1;
    private GroupFooterBand GroupFooter1;
    private GroupFooterBand GroupFooter2;
    private XRLabel xrLabel3;
    private XRPanel xrPanel1;
    private XRTableCell xrTableCell1;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public FinancialYearReport(string strConString)
    {
        InitializeComponent();
        ObjLocationMaster = new LocationMaster(strConString);
        objSys = new SystemParameter(strConString);
        objCurrency = new CurrencyMaster(strConString);
        Objaddress = new Set_AddressChild(strConString);
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
        //xrPictureBox1.ImageUrl = Url;
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

    public void setReportTitle(string strpersonheader)
    {
        xrLabel4.Text = strpersonheader;
    }

    public void setDateCriteria(string strTitle)
    {
        //xrLabel4.Text = strTitle;
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
        string resourceFileName = "FinancialYearReport.resx";
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
        this.purchaseDataSet1 = new PurchaseDataSet();
        this.SalesDataSet1 = new SalesDataSet();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabelUser = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.inventoryDataSet1 = new InventoryDataSet();
        this.accountsDataset1 = new AccountsDataset();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.SalesDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.inventoryDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
        this.Detail.HeightF = 20.08333F;
        this.Detail.KeepTogether = true;
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
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell1,
            this.xrTableCell26});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_FinancialYear_Report.AccountName")});
        this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorderColor = false;
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.Text = "xrTableCell4";
        this.xrTableCell4.Weight = 4.4780257897622118D;
        this.xrTableCell4.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell4_BeforePrint);
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_FinancialYear_Report.ContactName")});
        this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseBorderColor = false;
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "xrTableCell5";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell5.Weight = 5.5867609481992311D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_FinancialYear_Report.Closing_Amount")});
        this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell6.StylePriority.UseBorderColor = false;
        this.xrTableCell6.StylePriority.UseBorders = false;
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UsePadding = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "xrTableCell6";
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell6.Weight = 1.4811530008416254D;
        this.xrTableCell6.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell6_BeforePrint);
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBorderColor = false;
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell1.Weight = 0.11463691283496824D;
        // 
        // xrTableCell26
        // 
        this.xrTableCell26.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell26.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_FinancialYear_Report.Type")});
        this.xrTableCell26.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell26.Name = "xrTableCell26";
        this.xrTableCell26.StylePriority.UseBorderColor = false;
        this.xrTableCell26.StylePriority.UseBorders = false;
        this.xrTableCell26.StylePriority.UseFont = false;
        this.xrTableCell26.StylePriority.UseTextAlignment = false;
        this.xrTableCell26.Text = "CR ";
        this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell26.Weight = 0.3443484273395509D;
        this.xrTableCell26.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell26_BeforePrint);
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
        this.xrTableCell10.Text = "Refference";
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell10.Weight = 5.6362172251297578D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell9.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell9.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.StylePriority.UseBackColor = false;
        this.xrTableCell9.StylePriority.UseBorderColor = false;
        this.xrTableCell9.StylePriority.UseBorders = false;
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.Text = "Account Name";
        this.xrTableCell9.Weight = 4.4945351528990471D;
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell25});
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
        this.xrTableCell25.Text = "Closing Amount";
        this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell25.Weight = 1.9573135420405277D;
        // 
        // xrTable4
        // 
        this.xrTable4.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable4.Font = new System.Drawing.Font("Verdana", 8F);
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(2.000028F, 12F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
        this.xrTable4.SizeF = new System.Drawing.SizeF(1045.208F, 18F);
        this.xrTable4.StylePriority.UseBackColor = false;
        this.xrTable4.StylePriority.UseBorderColor = false;
        this.xrTable4.StylePriority.UseBorders = false;
        this.xrTable4.StylePriority.UseFont = false;
        // 
        // xrLabel4
        // 
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(4.000028F, 44.45834F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(1043.208F, 19.875F);
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "PHYSICAL INVENTORY VOUCHER";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
        this.PageHeader.HeightF = 30F;
        this.PageHeader.Name = "PageHeader";
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel4});
        this.ReportHeader.HeightF = 71.47916F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrLabel5
        // 
        this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(4.000139F, 10.00001F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(1043.208F, 23F);
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.Text = "xrLabel5";
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // inventoryDataSet1
        // 
        this.inventoryDataSet1.DataSetName = "InventoryDataSet";
        this.inventoryDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // accountsDataset1
        // 
        this.accountsDataset1.DataSetName = "AccountsDataset";
        this.accountsDataset1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrLabel3});
        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Finance_Code", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Location_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Detail_Trans_Id", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader1.HeightF = 38.54167F;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // xrLabel2
        // 
        this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_FinancialYear_Report.Location_Name")});
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(390.6249F, 23F);
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.Text = "xrLabel2";
        // 
        // xrLabel3
        // 
        this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_FinancialYear_Report.CreatedDate")});
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(417.7083F, 10.00001F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(176.0416F, 23F);
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.Text = "xrLabel3";
        this.xrLabel3.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel3_BeforePrint);
        // 
        // GroupHeader2
        // 
        this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
        this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Finance_Code", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader2.HeightF = 40.625F;
        this.GroupHeader2.Level = 1;
        this.GroupHeader2.Name = "GroupHeader2";
        // 
        // xrLabel1
        // 
        this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_FinancialYear_Report.Finance_Code")});
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(273.9584F, 23F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.Text = "xrLabel1";
        // 
        // GroupFooter1
        // 
        this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
        this.GroupFooter1.HeightF = 35.41667F;
        this.GroupFooter1.Name = "GroupFooter1";
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel1.BorderWidth = 3F;
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(1047.208F, 2.083334F);
        this.xrPanel1.StylePriority.UseBorders = false;
        this.xrPanel1.StylePriority.UseBorderWidth = false;
        // 
        // GroupFooter2
        // 
        this.GroupFooter2.HeightF = 0F;
        this.GroupFooter2.Level = 1;
        this.GroupFooter2.Name = "GroupFooter2";
        this.GroupFooter2.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
        // 
        // FinancialYearReport
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.PageHeader,
            this.ReportHeader,
            this.GroupHeader1,
            this.GroupHeader2,
            this.GroupFooter1,
            this.GroupFooter2});
        this.DataMember = "sp_Ac_FinancialYear_Report";
        this.DataSource = this.accountsDataset1;
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
        ((System.ComponentModel.ISupportInitialize)(this.inventoryDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).EndInit();
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
        //    xrTableCell8.Text = objSys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell8.Text);
        //}
        //catch
        //{


        //}
    }

    private void xrTableCell16_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell16.Text = objSys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell16.Text);
        //}
        //catch
        //{
        //}
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
        //try
        //{
        //    xrTableCell4.Text = Convert.ToDateTime(xrTableCell4.Text).ToString(objSys.SetDateFormat());
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
       
    }

    private void xrTableCell29_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell29.Text = objSys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell29.Text);
        //}
        //catch
        //{


        //}
    }

    private void xrTableCell19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell19.Text = objSys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell19.Text);
        //}
        //catch
        //{


        //}
    }

    private void xrTableCell20_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell20.Text = objSys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell20.Text);
        //}
        //catch
        //{


        //}
    }

    private void xrTableCell21_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
       
    }

    private void xrTableCell18_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
       
    }

    private void xrTableCell2_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell2.Text = objSys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell2.Text);
        //}
        //catch
        //{


        //}
    }

    private void xrTableCell22_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell22.Text = objSys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell22.Text);
        //}
        //catch
        //{


        //}
    }

    private void xrTableCell23_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrTableCell23.Text = objSys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell23.Text);
        //}
        //catch
        //{


        //}
    }

    private void xrLabel12_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //try
        //{
        //    xrLabel12.Text = Convert.ToDateTime(xrLabel12.Text).ToString(objSys.SetDateFormat());
        //}
        //catch
        //{

        //}
    }

    private void xrLabel22_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        
    }

    private void xrLabel25_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
    }

    private void xrLabel3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrLabel3.Text = Convert.ToDateTime(xrLabel3.Text).ToString(objSys.SetDateFormat());
        }
        catch
        {

        }
    }

    private void xrTableCell6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell6.Text = objSys.GetCurencyConversionForInv(GetCurrentColumnValue("Currency_Id").ToString(), xrTableCell6.Text);
        }
        catch
        {


        }
    }


}
