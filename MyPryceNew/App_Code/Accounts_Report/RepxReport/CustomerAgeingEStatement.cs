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
public class CustomerAgeingEstatement : DevExpress.XtraReports.UI.XtraReport
{
    CompanyMaster ObjCompany = null;
    LocationMaster objLocation = null;
    SystemParameter objsys = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;

    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private ReportHeaderBand ReportHeader;
    private PageHeaderBand PageHeader;
    private PageFooterBand PageFooter;
    private PurchaseDataSet purchaseDataSet1;
    private InventoryDataSet InventoryDataSet1;
    private XRPageInfo xrPageInfo2;
    private XRPictureBox xrPictureBox1;
    private XRLabel lblCompanyName;
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
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell16;
    private XRTable xrTable5;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell18;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell19;
    private XRTableRow xrTableRow15;
    private XRTableCell xrTableCell25;
    private XRTableCell lblCustomerAddress;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell21;
    private XRTableCell lblCustomerContactDetail;
    private XRTable xrTable4;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTblCellRptTitle;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell26;
    private XRTableCell lblDateOfStatement;
    private XRTableRow xrTableRow11;
    private XRTableCell lblTotalInvoiceLable;
    private XRTableCell lblTotalBalance;
    private XRRichText lblNote;
    private XRTable xrTblAging;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell37;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell29;
    private XRTableRow xrTableRow8;
    private XRTableCell lblCurrentAmt;
    private XRTableCell lbl1to5DaysAmt;
    private XRTableCell lbl6to30DaysAmt;
    private XRTableCell lblOver30DaysAmt;
    private XRTableCell lblTotalPastDuesAmt;
    private XRTableCell lblTotalInvoiceBalance;
    private XRTableRow xrTableRow12;
    private XRTableCell lblCurrentPercentage;
    private XRTableCell lbl1to5DaysPercentage;
    private XRTableCell lbl6to30DaysPercentage;
    private XRTableCell lblOver30DaysPercentage;
    private XRTableCell lblTotalPastDuesPercentage;
    private XRTableCell xrTableCell46;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell30;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public CustomerAgeingEstatement(string strConString)
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
        ObjCompany = new CompanyMaster(strConString);
        objLocation = new LocationMaster(strConString);
        ObjAgeingDetail = new Ac_Ageing_Detail(strConString);
        objsys = new SystemParameter(strConString);
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
    public void setAgeingDaysDetail(Ac_Ageing_Detail.clsAgeingDaysDetail clsAging)
    {
        try
        {
            lblCurrentAmt.Text = clsAging.amtCurrent ;
            lblCurrentPercentage.Text= clsAging.perCurrent ;
            lbl1to5DaysAmt.Text =clsAging.amt1to5Days ;
            lbl1to5DaysPercentage.Text = clsAging.per1to5Days;
            lbl6to30DaysAmt.Text= clsAging.amt6to30Days;
            lbl6to30DaysPercentage.Text =clsAging.per6to30Days;
            lblOver30DaysAmt.Text = clsAging.amtOver30Days ;
            lblOver30DaysPercentage.Text =clsAging.perOver30Days;
            lblTotalPastDuesAmt.Text= clsAging.amtTotalPastDue ;
            lblTotalPastDuesPercentage.Text = clsAging.perTotalPastDue ;
            lblTotalInvoiceBalance.Text= clsAging.amtTotalBalance ;
        }
        catch
        {

        }
    }
    public void setcompanyname(string companyname)
    {
        lblCompanyName.Text = companyname;
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
       
    }
    public void setStatementDate(string strStatementDate)
    {
        lblDateOfStatement.Text = strStatementDate;
    }

    public void setContactPersonName(string strpersonName)
    {
        //xrTableCell23.Text = strpersonName;
    }

    public void setFooterNote(string strFooterNote)
    {
        lblNote.Html = strFooterNote;
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
       
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            string resourceFileName = "CustomerAgeingEStatement.resx";
            System.Resources.ResourceManager resources = global::Resources.CustomerAgeingEStatement.ResourceManager;
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.lblCompanyName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblCustomerAddress = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblCustomerContactDetail = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTblCellRptTitle = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblDateOfStatement = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.lblTotalInvoiceLable = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblTotalBalance = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.purchaseDataSet1 = new PurchaseDataSet();
            this.InventoryDataSet1 = new InventoryDataSet();
            this.accountsDataset1 = new AccountsDataset();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.lblNote = new DevExpress.XtraReports.UI.XRRichText();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTblAging = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.lblCurrentAmt = new DevExpress.XtraReports.UI.XRTableCell();
            this.lbl1to5DaysAmt = new DevExpress.XtraReports.UI.XRTableCell();
            this.lbl6to30DaysAmt = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblOver30DaysAmt = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblTotalPastDuesAmt = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblTotalInvoiceBalance = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.lblCurrentPercentage = new DevExpress.XtraReports.UI.XRTableCell();
            this.lbl1to5DaysPercentage = new DevExpress.XtraReports.UI.XRTableCell();
            this.lbl6to30DaysPercentage = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblOver30DaysPercentage = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblTotalPastDuesPercentage = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTblAging)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 28.12665F;
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
            this.xrTable2.SizeF = new System.Drawing.SizeF(816.9999F, 28.12665F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell33,
            this.xrTableCell31,
            this.xrTableCell6,
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell17,
            this.xrTableCell10,
            this.xrTableCell35});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.Invoice_No")});
            this.xrTableCell4.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell4.StylePriority.UseBorderColor = false;
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Ac/No.";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.4287826263726715D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.Po_No")});
            this.xrTableCell5.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell5.StylePriority.UseBorderColor = false;
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UsePadding = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Account Name";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell5.Weight = 0.37220082087614D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell33.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.Invoice_Date")});
            this.xrTableCell33.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell33.StylePriority.UseBorderColor = false;
            this.xrTableCell33.StylePriority.UseBorders = false;
            this.xrTableCell33.StylePriority.UseFont = false;
            this.xrTableCell33.StylePriority.UsePadding = false;
            this.xrTableCell33.StylePriority.UseTextAlignment = false;
            this.xrTableCell33.Text = "xrTableCell33";
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell33.Weight = 0.33131421256317206D;
            this.xrTableCell33.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell33_BeforePrint);
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.paymentDate")});
            this.xrTableCell6.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell6.StylePriority.UseBorderColor = false;
            this.xrTableCell6.StylePriority.UseBorders = false;
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UsePadding = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Naration";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell6.Weight = 0.32726215462719288D;
            this.xrTableCell6.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell6_BeforePrint);
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.Payment_Terms")});
            this.xrTableCell8.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell8.StylePriority.UseBorderColor = false;
            this.xrTableCell8.StylePriority.UseBorders = false;
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.StylePriority.UsePadding = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "xrTableCell8";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell8.Weight = 0.32536344392211825D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.Due_Days")});
            this.xrTableCell9.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell9.StylePriority.UseBorderColor = false;
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UsePadding = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Debit Amount";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.27675820680239532D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.Currency_Name")});
            this.xrTableCell17.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell17.StylePriority.UseBorderColor = false;
            this.xrTableCell17.StylePriority.UseBorders = false;
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.StylePriority.UsePadding = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "xrTableCell17";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell17.Weight = 0.27349057695834417D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.actual_Invoice_amt")});
            this.xrTableCell10.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell10.StylePriority.UseBorderColor = false;
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UsePadding = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Credit Amount";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell10.TextFormatString = "{0:#.000}";
            this.xrTableCell10.Weight = 0.34482796769506263D;
            this.xrTableCell10.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell10_BeforePrint);
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell35.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.actual_balance_amt")});
            this.xrTableCell35.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell35.StylePriority.UseBorderColor = false;
            this.xrTableCell35.StylePriority.UseBorders = false;
            this.xrTableCell35.StylePriority.UseFont = false;
            this.xrTableCell35.StylePriority.UsePadding = false;
            this.xrTableCell35.StylePriority.UseTextAlignment = false;
            this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell35.TextFormatString = "{0:#.000}";
            this.xrTableCell35.Weight = 0.3199999901829032D;
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
            this.BottomMargin.HeightF = 19.49997F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
            this.ReportHeader.HeightF = 204.7918F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrPanel1
            // 
            this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblCompanyName,
            this.xrPictureBox1,
            this.xrTable5,
            this.xrTable4});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0.999999F, 10.00001F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(816.0001F, 188.8751F);
            this.xrPanel1.StylePriority.UseBorders = false;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblCompanyName.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyName.LocationFloat = new DevExpress.Utils.PointFloat(97.69508F, 0F);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCompanyName.SizeF = new System.Drawing.SizeF(403.1976F, 21.95834F);
            this.lblCompanyName.StylePriority.UseBorders = false;
            this.lblCompanyName.StylePriority.UseFont = false;
            this.lblCompanyName.StylePriority.UseTextAlignment = false;
            this.lblCompanyName.Text = "lblCompanyName";
            this.lblCompanyName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(1.861898F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(95.83319F, 45.95832F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox1.StylePriority.UseBorders = false;
            // 
            // xrTable5
            // 
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(5.861898F, 79.85024F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4,
            this.xrTableRow14,
            this.xrTableRow15,
            this.xrTableRow5});
            this.xrTable5.SizeF = new System.Drawing.SizeF(455.182F, 85.29994F);
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "To";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell18.Weight = 1D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell24,
            this.xrTableCell19});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseFont = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell24.Weight = 0.076634531655998039D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingStatement.Name")});
            this.xrTableCell19.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell19.Multiline = true;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell19.Weight = 0.923365468344002D;
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25,
            this.lblCustomerAddress});
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.Weight = 1D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseFont = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell25.Weight = 0.076634565178388736D;
            // 
            // lblCustomerAddress
            // 
            this.lblCustomerAddress.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerAddress.Multiline = true;
            this.lblCustomerAddress.Name = "lblCustomerAddress";
            this.lblCustomerAddress.StylePriority.UseFont = false;
            this.lblCustomerAddress.StylePriority.UseTextAlignment = false;
            this.lblCustomerAddress.Text = "lblCustomerAddress";
            this.lblCustomerAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lblCustomerAddress.Weight = 0.92336543482161126D;
            this.lblCustomerAddress.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblCustomerAddress_BeforePrint);
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell21,
            this.lblCustomerContactDetail});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell21.Weight = 0.076634565178388736D;
            // 
            // lblCustomerContactDetail
            // 
            this.lblCustomerContactDetail.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.lblCustomerContactDetail.Multiline = true;
            this.lblCustomerContactDetail.Name = "lblCustomerContactDetail";
            this.lblCustomerContactDetail.StylePriority.UseFont = false;
            this.lblCustomerContactDetail.StylePriority.UseTextAlignment = false;
            this.lblCustomerContactDetail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lblCustomerContactDetail.Weight = 0.92336543482161126D;
            // 
            // xrTable4
            // 
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(503.068F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow9,
            this.xrTableRow10,
            this.xrTableRow11});
            this.xrTable4.SizeF = new System.Drawing.SizeF(298.6404F, 63.97495F);
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTblCellRptTitle});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTblCellRptTitle
            // 
            this.xrTblCellRptTitle.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.xrTblCellRptTitle.Name = "xrTblCellRptTitle";
            this.xrTblCellRptTitle.StylePriority.UseFont = false;
            this.xrTblCellRptTitle.StylePriority.UseTextAlignment = false;
            this.xrTblCellRptTitle.Text = "Invoice Statement";
            this.xrTblCellRptTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTblCellRptTitle.Weight = 1D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell26,
            this.lblDateOfStatement});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = "Date Of Statement : ";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell26.Weight = 0.70473196703220387D;
            // 
            // lblDateOfStatement
            // 
            this.lblDateOfStatement.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.lblDateOfStatement.Name = "lblDateOfStatement";
            this.lblDateOfStatement.StylePriority.UseFont = false;
            this.lblDateOfStatement.StylePriority.UseTextAlignment = false;
            this.lblDateOfStatement.Text = "xrTableCell15";
            this.lblDateOfStatement.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblDateOfStatement.Weight = 0.29526803296779636D;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.lblTotalInvoiceLable,
            this.lblTotalBalance});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // lblTotalInvoiceLable
            // 
            this.lblTotalInvoiceLable.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.lblTotalInvoiceLable.Multiline = true;
            this.lblTotalInvoiceLable.Name = "lblTotalInvoiceLable";
            this.lblTotalInvoiceLable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalInvoiceLable.StylePriority.UseFont = false;
            this.lblTotalInvoiceLable.StylePriority.UseTextAlignment = false;
            this.lblTotalInvoiceLable.Text = "Total Invoiced Balance";
            this.lblTotalInvoiceLable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalInvoiceLable.Weight = 0.70473196703220387D;
            // 
            // lblTotalBalance
            // 
            this.lblTotalBalance.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingStatement.actual_balance_amt")});
            this.lblTotalBalance.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.lblTotalBalance.Multiline = true;
            this.lblTotalBalance.Name = "lblTotalBalance";
            this.lblTotalBalance.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalBalance.StylePriority.UseFont = false;
            this.lblTotalBalance.StylePriority.UseTextAlignment = false;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.lblTotalBalance.Summary = xrSummary1;
            this.lblTotalBalance.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalBalance.TextFormatString = "{0:#.000}";
            this.lblTotalBalance.Weight = 0.29526803296779636D;
            this.lblTotalBalance.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblTotalBalance_BeforePrint);
            this.lblTotalBalance.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.lblTotalBalance_PrintOnPage);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.HeightF = 50.20835F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.Silver;
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(7.947286E-06F, 10.00001F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(816.9999F, 40.20834F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell11,
            this.xrTableCell2,
            this.xrTableCell30,
            this.xrTableCell12,
            this.xrTableCell32,
            this.xrTableCell3,
            this.xrTableCell16,
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
            this.xrTableCell1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseBorderColor = false;
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseForeColor = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Invoice No.";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.42865152532865763D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell11.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell11.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell11.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseBackColor = false;
            this.xrTableCell11.StylePriority.UseBorderColor = false;
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.StylePriority.UseForeColor = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "PO No.";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 0.37220077492288894D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell2.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UseBorderColor = false;
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseForeColor = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Invoice Date\r\n";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.33131422944576511D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell12.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell12.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseBackColor = false;
            this.xrTableCell12.StylePriority.UseBorders = false;
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseForeColor = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "Due Date";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell12.Weight = 0.32726201241162928D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell32.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell32.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell32.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell32.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell32.Multiline = true;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.StylePriority.UseBackColor = false;
            this.xrTableCell32.StylePriority.UseBorderColor = false;
            this.xrTableCell32.StylePriority.UseBorders = false;
            this.xrTableCell32.StylePriority.UseFont = false;
            this.xrTableCell32.StylePriority.UseForeColor = false;
            this.xrTableCell32.StylePriority.UseTextAlignment = false;
            this.xrTableCell32.Text = "Payment \r\nTerms";
            this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell32.Weight = 0.32536330270943309D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell3.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell3.StylePriority.UseBackColor = false;
            this.xrTableCell3.StylePriority.UseBorderColor = false;
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseForeColor = false;
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Days \r\nOverdue";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 0.27675846614968319D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell16.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell16.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell16.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseBackColor = false;
            this.xrTableCell16.StylePriority.UseBorders = false;
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UseForeColor = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "Currency";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell16.Weight = 0.2734903170486444D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell7.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell7.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrTableCell7.StylePriority.UseBackColor = false;
            this.xrTableCell7.StylePriority.UseBorderColor = false;
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UseForeColor = false;
            this.xrTableCell7.StylePriority.UsePadding = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Inv. Amount";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 0.34482818220030076D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell34.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell34.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell34.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell34.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.StylePriority.UseBackColor = false;
            this.xrTableCell34.StylePriority.UseBorderColor = false;
            this.xrTableCell34.StylePriority.UseBorders = false;
            this.xrTableCell34.StylePriority.UseFont = false;
            this.xrTableCell34.StylePriority.UseForeColor = false;
            this.xrTableCell34.StylePriority.UseTextAlignment = false;
            this.xrTableCell34.Text = "Remain Balance";
            this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell34.Weight = 0.32698359314608683D;
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
            this.xrPageInfo2.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(816.9999F, 18.12496F);
            this.xrPageInfo2.StylePriority.UseBorders = false;
            this.xrPageInfo2.StylePriority.UseFont = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrPageInfo2.TextFormatString = "Page{0}Of {1}";
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
            this.lblNote,
            this.xrTable3,
            this.xrTblAging});
            this.ReportFooter.HeightF = 267.0832F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // lblNote
            // 
            this.lblNote.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.LocationFloat = new DevExpress.Utils.PointFloat(0F, 201.2917F);
            this.lblNote.Name = "lblNote";
            this.lblNote.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNote.SerializableRtfString = resources.GetString("lblNote.SerializableRtfString");
            this.lblNote.SizeF = new System.Drawing.SizeF(816.9999F, 55.04157F);
            this.lblNote.StylePriority.UseFont = false;
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(499.8462F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable3.SizeF = new System.Drawing.SizeF(317.1537F, 25.83326F);
            this.xrTable3.StylePriority.UseBorders = false;
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
            this.xrTableCell13.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UsePadding = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "TOTAL:  ";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell13.Weight = 1.2733326255913753D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.actual_Invoice_amt")});
            this.xrTableCell14.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UsePadding = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell14.Summary = xrSummary2;
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell14.TextFormatString = "{0:#.000}";
            this.xrTableCell14.Weight = 0.902336598119979D;
            this.xrTableCell14.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell14_BeforePrint);
            this.xrTableCell14.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell14_PrintOnPage);
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingReport.actual_balance_amt")});
            this.xrTableCell15.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UsePadding = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell15.Summary = xrSummary3;
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell15.TextFormatString = "{0:#.000}";
            this.xrTableCell15.Weight = 0.82433077628864582D;
            this.xrTableCell15.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell15_BeforePrint);
            this.xrTableCell15.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell15_PrintOnPage);
            // 
            // xrTblAging
            // 
            this.xrTblAging.BackColor = System.Drawing.Color.Black;
            this.xrTblAging.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTblAging.BorderWidth = 0.5F;
            this.xrTblAging.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTblAging.LocationFloat = new DevExpress.Utils.PointFloat(1.000007F, 49.79165F);
            this.xrTblAging.Name = "xrTblAging";
            this.xrTblAging.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow6,
            this.xrTableRow8,
            this.xrTableRow12});
            this.xrTblAging.SizeF = new System.Drawing.SizeF(817.0001F, 124.3751F);
            this.xrTblAging.StylePriority.UseBackColor = false;
            this.xrTblAging.StylePriority.UseBorders = false;
            this.xrTblAging.StylePriority.UseBorderWidth = false;
            this.xrTblAging.StylePriority.UseFont = false;
            this.xrTblAging.StylePriority.UseTextAlignment = false;
            this.xrTblAging.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleJustify;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell37});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell37.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell37.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell37.BorderWidth = 0.5F;
            this.xrTableCell37.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell37.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell37.Multiline = true;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell37.StylePriority.UseBackColor = false;
            this.xrTableCell37.StylePriority.UseBorderColor = false;
            this.xrTableCell37.StylePriority.UseBorders = false;
            this.xrTableCell37.StylePriority.UseBorderWidth = false;
            this.xrTableCell37.StylePriority.UseFont = false;
            this.xrTableCell37.StylePriority.UseForeColor = false;
            this.xrTableCell37.StylePriority.UsePadding = false;
            this.xrTableCell37.StylePriority.UseTextAlignment = false;
            this.xrTableCell37.Text = "Account Aging";
            this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell37.Weight = 3.0068524033630895D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell20,
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell27,
            this.xrTableCell28,
            this.xrTableCell29});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell20.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell20.BorderWidth = 0.5F;
            this.xrTableCell20.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell20.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell20.StylePriority.UseBackColor = false;
            this.xrTableCell20.StylePriority.UseBorderColor = false;
            this.xrTableCell20.StylePriority.UseBorders = false;
            this.xrTableCell20.StylePriority.UseBorderWidth = false;
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UseForeColor = false;
            this.xrTableCell20.StylePriority.UsePadding = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "Current";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell20.Weight = 0.42865152532865763D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell22.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell22.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell22.BorderWidth = 0.5F;
            this.xrTableCell22.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell22.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseBackColor = false;
            this.xrTableCell22.StylePriority.UseBorderColor = false;
            this.xrTableCell22.StylePriority.UseBorders = false;
            this.xrTableCell22.StylePriority.UseBorderWidth = false;
            this.xrTableCell22.StylePriority.UseFont = false;
            this.xrTableCell22.StylePriority.UseForeColor = false;
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "1-5 Days";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell22.Weight = 0.37220077492288894D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell23.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell23.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell23.BorderWidth = 0.5F;
            this.xrTableCell23.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell23.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell23.Multiline = true;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell23.StylePriority.UseBackColor = false;
            this.xrTableCell23.StylePriority.UseBorderColor = false;
            this.xrTableCell23.StylePriority.UseBorders = false;
            this.xrTableCell23.StylePriority.UseBorderWidth = false;
            this.xrTableCell23.StylePriority.UseFont = false;
            this.xrTableCell23.StylePriority.UseForeColor = false;
            this.xrTableCell23.StylePriority.UsePadding = false;
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "6-30 Days";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell23.Weight = 0.41565595182050119D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell27.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell27.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell27.BorderWidth = 0.5F;
            this.xrTableCell27.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell27.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell27.Multiline = true;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.StylePriority.UseBackColor = false;
            this.xrTableCell27.StylePriority.UseBorderColor = false;
            this.xrTableCell27.StylePriority.UseBorders = false;
            this.xrTableCell27.StylePriority.UseBorderWidth = false;
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.StylePriority.UseForeColor = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.Text = "Over 30 Days";
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell27.Weight = 0.51511329527136884D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell28.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell28.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell28.BorderWidth = 0.5F;
            this.xrTableCell28.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell28.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell28.Multiline = true;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseBackColor = false;
            this.xrTableCell28.StylePriority.UseBorderColor = false;
            this.xrTableCell28.StylePriority.UseBorders = false;
            this.xrTableCell28.StylePriority.UseBorderWidth = false;
            this.xrTableCell28.StylePriority.UseFont = false;
            this.xrTableCell28.StylePriority.UseForeColor = false;
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "Total Past Due";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell28.Weight = 0.54005105272115594D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell29.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell29.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell29.BorderWidth = 0.5F;
            this.xrTableCell29.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell29.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell29.Multiline = true;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell29.StylePriority.UseBackColor = false;
            this.xrTableCell29.StylePriority.UseBorderColor = false;
            this.xrTableCell29.StylePriority.UseBorders = false;
            this.xrTableCell29.StylePriority.UseBorderWidth = false;
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.StylePriority.UseForeColor = false;
            this.xrTableCell29.StylePriority.UsePadding = false;
            this.xrTableCell29.StylePriority.UseTextAlignment = false;
            this.xrTableCell29.Text = "Total Invoiced Balance";
            this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell29.Weight = 0.73517980329851684D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.lblCurrentAmt,
            this.lbl1to5DaysAmt,
            this.lbl6to30DaysAmt,
            this.lblOver30DaysAmt,
            this.lblTotalPastDuesAmt,
            this.lblTotalInvoiceBalance});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // lblCurrentAmt
            // 
            this.lblCurrentAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentAmt.BorderColor = System.Drawing.Color.Black;
            this.lblCurrentAmt.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblCurrentAmt.BorderWidth = 0.5F;
            this.lblCurrentAmt.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentAmt.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentAmt.Multiline = true;
            this.lblCurrentAmt.Name = "lblCurrentAmt";
            this.lblCurrentAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCurrentAmt.StylePriority.UseBackColor = false;
            this.lblCurrentAmt.StylePriority.UseBorderColor = false;
            this.lblCurrentAmt.StylePriority.UseBorders = false;
            this.lblCurrentAmt.StylePriority.UseBorderWidth = false;
            this.lblCurrentAmt.StylePriority.UseFont = false;
            this.lblCurrentAmt.StylePriority.UseForeColor = false;
            this.lblCurrentAmt.StylePriority.UsePadding = false;
            this.lblCurrentAmt.StylePriority.UseTextAlignment = false;
            this.lblCurrentAmt.Text = "lblCurrentAmt";
            this.lblCurrentAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblCurrentAmt.Weight = 0.42865152532865763D;
            // 
            // lbl1to5DaysAmt
            // 
            this.lbl1to5DaysAmt.BackColor = System.Drawing.Color.Transparent;
            this.lbl1to5DaysAmt.BorderColor = System.Drawing.Color.Black;
            this.lbl1to5DaysAmt.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lbl1to5DaysAmt.BorderWidth = 0.5F;
            this.lbl1to5DaysAmt.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1to5DaysAmt.ForeColor = System.Drawing.Color.Black;
            this.lbl1to5DaysAmt.Multiline = true;
            this.lbl1to5DaysAmt.Name = "lbl1to5DaysAmt";
            this.lbl1to5DaysAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl1to5DaysAmt.StylePriority.UseBackColor = false;
            this.lbl1to5DaysAmt.StylePriority.UseBorderColor = false;
            this.lbl1to5DaysAmt.StylePriority.UseBorders = false;
            this.lbl1to5DaysAmt.StylePriority.UseBorderWidth = false;
            this.lbl1to5DaysAmt.StylePriority.UseFont = false;
            this.lbl1to5DaysAmt.StylePriority.UseForeColor = false;
            this.lbl1to5DaysAmt.StylePriority.UseTextAlignment = false;
            this.lbl1to5DaysAmt.Text = "lbl1to5DaysAmt";
            this.lbl1to5DaysAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lbl1to5DaysAmt.Weight = 0.37220077492288894D;
            // 
            // lbl6to30DaysAmt
            // 
            this.lbl6to30DaysAmt.BackColor = System.Drawing.Color.Transparent;
            this.lbl6to30DaysAmt.BorderColor = System.Drawing.Color.Black;
            this.lbl6to30DaysAmt.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lbl6to30DaysAmt.BorderWidth = 0.5F;
            this.lbl6to30DaysAmt.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl6to30DaysAmt.ForeColor = System.Drawing.Color.Black;
            this.lbl6to30DaysAmt.Multiline = true;
            this.lbl6to30DaysAmt.Name = "lbl6to30DaysAmt";
            this.lbl6to30DaysAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl6to30DaysAmt.StylePriority.UseBackColor = false;
            this.lbl6to30DaysAmt.StylePriority.UseBorderColor = false;
            this.lbl6to30DaysAmt.StylePriority.UseBorders = false;
            this.lbl6to30DaysAmt.StylePriority.UseBorderWidth = false;
            this.lbl6to30DaysAmt.StylePriority.UseFont = false;
            this.lbl6to30DaysAmt.StylePriority.UseForeColor = false;
            this.lbl6to30DaysAmt.StylePriority.UsePadding = false;
            this.lbl6to30DaysAmt.StylePriority.UseTextAlignment = false;
            this.lbl6to30DaysAmt.Text = "lbl6to30DaysAmt";
            this.lbl6to30DaysAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lbl6to30DaysAmt.Weight = 0.41565595182050119D;
            // 
            // lblOver30DaysAmt
            // 
            this.lblOver30DaysAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblOver30DaysAmt.BorderColor = System.Drawing.Color.Black;
            this.lblOver30DaysAmt.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblOver30DaysAmt.BorderWidth = 0.5F;
            this.lblOver30DaysAmt.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOver30DaysAmt.ForeColor = System.Drawing.Color.Black;
            this.lblOver30DaysAmt.Multiline = true;
            this.lblOver30DaysAmt.Name = "lblOver30DaysAmt";
            this.lblOver30DaysAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblOver30DaysAmt.StylePriority.UseBackColor = false;
            this.lblOver30DaysAmt.StylePriority.UseBorderColor = false;
            this.lblOver30DaysAmt.StylePriority.UseBorders = false;
            this.lblOver30DaysAmt.StylePriority.UseBorderWidth = false;
            this.lblOver30DaysAmt.StylePriority.UseFont = false;
            this.lblOver30DaysAmt.StylePriority.UseForeColor = false;
            this.lblOver30DaysAmt.StylePriority.UseTextAlignment = false;
            this.lblOver30DaysAmt.Text = "lblOver30DaysAmt";
            this.lblOver30DaysAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblOver30DaysAmt.Weight = 0.51511329527136884D;
            // 
            // lblTotalPastDuesAmt
            // 
            this.lblTotalPastDuesAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalPastDuesAmt.BorderColor = System.Drawing.Color.Black;
            this.lblTotalPastDuesAmt.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblTotalPastDuesAmt.BorderWidth = 0.5F;
            this.lblTotalPastDuesAmt.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPastDuesAmt.ForeColor = System.Drawing.Color.Black;
            this.lblTotalPastDuesAmt.Multiline = true;
            this.lblTotalPastDuesAmt.Name = "lblTotalPastDuesAmt";
            this.lblTotalPastDuesAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalPastDuesAmt.StylePriority.UseBackColor = false;
            this.lblTotalPastDuesAmt.StylePriority.UseBorderColor = false;
            this.lblTotalPastDuesAmt.StylePriority.UseBorders = false;
            this.lblTotalPastDuesAmt.StylePriority.UseBorderWidth = false;
            this.lblTotalPastDuesAmt.StylePriority.UseFont = false;
            this.lblTotalPastDuesAmt.StylePriority.UseForeColor = false;
            this.lblTotalPastDuesAmt.StylePriority.UseTextAlignment = false;
            this.lblTotalPastDuesAmt.Text = "lblTotalPastDuesAmt";
            this.lblTotalPastDuesAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblTotalPastDuesAmt.Weight = 0.54005105272115594D;
            // 
            // lblTotalInvoiceBalance
            // 
            this.lblTotalInvoiceBalance.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalInvoiceBalance.BorderColor = System.Drawing.Color.Black;
            this.lblTotalInvoiceBalance.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblTotalInvoiceBalance.BorderWidth = 0.5F;
            this.lblTotalInvoiceBalance.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalInvoiceBalance.ForeColor = System.Drawing.Color.Black;
            this.lblTotalInvoiceBalance.Multiline = true;
            this.lblTotalInvoiceBalance.Name = "lblTotalInvoiceBalance";
            this.lblTotalInvoiceBalance.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalInvoiceBalance.StylePriority.UseBackColor = false;
            this.lblTotalInvoiceBalance.StylePriority.UseBorderColor = false;
            this.lblTotalInvoiceBalance.StylePriority.UseBorders = false;
            this.lblTotalInvoiceBalance.StylePriority.UseBorderWidth = false;
            this.lblTotalInvoiceBalance.StylePriority.UseFont = false;
            this.lblTotalInvoiceBalance.StylePriority.UseForeColor = false;
            this.lblTotalInvoiceBalance.StylePriority.UsePadding = false;
            this.lblTotalInvoiceBalance.StylePriority.UseTextAlignment = false;
            this.lblTotalInvoiceBalance.Text = "lblTotalInvoiceBalance";
            this.lblTotalInvoiceBalance.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblTotalInvoiceBalance.Weight = 0.73517980329851684D;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.lblCurrentPercentage,
            this.lbl1to5DaysPercentage,
            this.lbl6to30DaysPercentage,
            this.lblOver30DaysPercentage,
            this.lblTotalPastDuesPercentage,
            this.xrTableCell46});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // lblCurrentPercentage
            // 
            this.lblCurrentPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentPercentage.BorderColor = System.Drawing.Color.Black;
            this.lblCurrentPercentage.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblCurrentPercentage.BorderWidth = 0.5F;
            this.lblCurrentPercentage.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPercentage.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentPercentage.Multiline = true;
            this.lblCurrentPercentage.Name = "lblCurrentPercentage";
            this.lblCurrentPercentage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCurrentPercentage.StylePriority.UseBackColor = false;
            this.lblCurrentPercentage.StylePriority.UseBorderColor = false;
            this.lblCurrentPercentage.StylePriority.UseBorders = false;
            this.lblCurrentPercentage.StylePriority.UseBorderWidth = false;
            this.lblCurrentPercentage.StylePriority.UseFont = false;
            this.lblCurrentPercentage.StylePriority.UseForeColor = false;
            this.lblCurrentPercentage.StylePriority.UsePadding = false;
            this.lblCurrentPercentage.StylePriority.UseTextAlignment = false;
            this.lblCurrentPercentage.Text = "lblCurrentPercentage";
            this.lblCurrentPercentage.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblCurrentPercentage.Weight = 0.42865152532865763D;
            // 
            // lbl1to5DaysPercentage
            // 
            this.lbl1to5DaysPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lbl1to5DaysPercentage.BorderColor = System.Drawing.Color.Black;
            this.lbl1to5DaysPercentage.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lbl1to5DaysPercentage.BorderWidth = 0.5F;
            this.lbl1to5DaysPercentage.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1to5DaysPercentage.ForeColor = System.Drawing.Color.Black;
            this.lbl1to5DaysPercentage.Multiline = true;
            this.lbl1to5DaysPercentage.Name = "lbl1to5DaysPercentage";
            this.lbl1to5DaysPercentage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl1to5DaysPercentage.StylePriority.UseBackColor = false;
            this.lbl1to5DaysPercentage.StylePriority.UseBorderColor = false;
            this.lbl1to5DaysPercentage.StylePriority.UseBorders = false;
            this.lbl1to5DaysPercentage.StylePriority.UseBorderWidth = false;
            this.lbl1to5DaysPercentage.StylePriority.UseFont = false;
            this.lbl1to5DaysPercentage.StylePriority.UseForeColor = false;
            this.lbl1to5DaysPercentage.StylePriority.UsePadding = false;
            this.lbl1to5DaysPercentage.StylePriority.UseTextAlignment = false;
            this.lbl1to5DaysPercentage.Text = "lbl1to5DaysPercentage";
            this.lbl1to5DaysPercentage.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lbl1to5DaysPercentage.Weight = 0.37220077492288894D;
            // 
            // lbl6to30DaysPercentage
            // 
            this.lbl6to30DaysPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lbl6to30DaysPercentage.BorderColor = System.Drawing.Color.Black;
            this.lbl6to30DaysPercentage.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lbl6to30DaysPercentage.BorderWidth = 0.5F;
            this.lbl6to30DaysPercentage.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl6to30DaysPercentage.ForeColor = System.Drawing.Color.Black;
            this.lbl6to30DaysPercentage.Multiline = true;
            this.lbl6to30DaysPercentage.Name = "lbl6to30DaysPercentage";
            this.lbl6to30DaysPercentage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl6to30DaysPercentage.StylePriority.UseBackColor = false;
            this.lbl6to30DaysPercentage.StylePriority.UseBorderColor = false;
            this.lbl6to30DaysPercentage.StylePriority.UseBorders = false;
            this.lbl6to30DaysPercentage.StylePriority.UseBorderWidth = false;
            this.lbl6to30DaysPercentage.StylePriority.UseFont = false;
            this.lbl6to30DaysPercentage.StylePriority.UseForeColor = false;
            this.lbl6to30DaysPercentage.StylePriority.UsePadding = false;
            this.lbl6to30DaysPercentage.StylePriority.UseTextAlignment = false;
            this.lbl6to30DaysPercentage.Text = "lbl6to30DaysPercentage";
            this.lbl6to30DaysPercentage.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lbl6to30DaysPercentage.Weight = 0.41565595182050119D;
            // 
            // lblOver30DaysPercentage
            // 
            this.lblOver30DaysPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lblOver30DaysPercentage.BorderColor = System.Drawing.Color.Black;
            this.lblOver30DaysPercentage.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblOver30DaysPercentage.BorderWidth = 0.5F;
            this.lblOver30DaysPercentage.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOver30DaysPercentage.ForeColor = System.Drawing.Color.Black;
            this.lblOver30DaysPercentage.Multiline = true;
            this.lblOver30DaysPercentage.Name = "lblOver30DaysPercentage";
            this.lblOver30DaysPercentage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblOver30DaysPercentage.StylePriority.UseBackColor = false;
            this.lblOver30DaysPercentage.StylePriority.UseBorderColor = false;
            this.lblOver30DaysPercentage.StylePriority.UseBorders = false;
            this.lblOver30DaysPercentage.StylePriority.UseBorderWidth = false;
            this.lblOver30DaysPercentage.StylePriority.UseFont = false;
            this.lblOver30DaysPercentage.StylePriority.UseForeColor = false;
            this.lblOver30DaysPercentage.StylePriority.UsePadding = false;
            this.lblOver30DaysPercentage.StylePriority.UseTextAlignment = false;
            this.lblOver30DaysPercentage.Text = "lblOver30DaysPercentage";
            this.lblOver30DaysPercentage.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblOver30DaysPercentage.Weight = 0.51511329527136884D;
            // 
            // lblTotalPastDuesPercentage
            // 
            this.lblTotalPastDuesPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalPastDuesPercentage.BorderColor = System.Drawing.Color.Black;
            this.lblTotalPastDuesPercentage.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblTotalPastDuesPercentage.BorderWidth = 0.5F;
            this.lblTotalPastDuesPercentage.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPastDuesPercentage.ForeColor = System.Drawing.Color.Black;
            this.lblTotalPastDuesPercentage.Multiline = true;
            this.lblTotalPastDuesPercentage.Name = "lblTotalPastDuesPercentage";
            this.lblTotalPastDuesPercentage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalPastDuesPercentage.StylePriority.UseBackColor = false;
            this.lblTotalPastDuesPercentage.StylePriority.UseBorderColor = false;
            this.lblTotalPastDuesPercentage.StylePriority.UseBorders = false;
            this.lblTotalPastDuesPercentage.StylePriority.UseBorderWidth = false;
            this.lblTotalPastDuesPercentage.StylePriority.UseFont = false;
            this.lblTotalPastDuesPercentage.StylePriority.UseForeColor = false;
            this.lblTotalPastDuesPercentage.StylePriority.UsePadding = false;
            this.lblTotalPastDuesPercentage.StylePriority.UseTextAlignment = false;
            this.lblTotalPastDuesPercentage.Text = "lblTotalPastDuesPercentage";
            this.lblTotalPastDuesPercentage.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblTotalPastDuesPercentage.Weight = 0.54005105272115594D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell46.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell46.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell46.BorderWidth = 0.5F;
            this.xrTableCell46.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell46.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell46.Multiline = true;
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell46.StylePriority.UseBackColor = false;
            this.xrTableCell46.StylePriority.UseBorderColor = false;
            this.xrTableCell46.StylePriority.UseBorders = false;
            this.xrTableCell46.StylePriority.UseBorderWidth = false;
            this.xrTableCell46.StylePriority.UseFont = false;
            this.xrTableCell46.StylePriority.UseForeColor = false;
            this.xrTableCell46.StylePriority.UsePadding = false;
            this.xrTableCell46.StylePriority.UseTextAlignment = false;
            this.xrTableCell46.Text = "100%";
            this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell46.Weight = 0.73517980329851684D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell30.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell30.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell30.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell30.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell30.Multiline = true;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell30.StylePriority.UseBackColor = false;
            this.xrTableCell30.StylePriority.UseBorderColor = false;
            this.xrTableCell30.StylePriority.UseBorders = false;
            this.xrTableCell30.StylePriority.UseFont = false;
            this.xrTableCell30.StylePriority.UseForeColor = false;
            this.xrTableCell30.StylePriority.UsePadding = false;
            this.xrTableCell30.StylePriority.UseTextAlignment = false;
            this.xrTableCell30.Text = "Sales Person";
            this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell30.Weight = 0.33131422944576511D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.BorderColor = System.Drawing.Color.Silver;
            this.xrTableCell31.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Sp_Ac_InvoiceAgeingStatement.SalesPersonName")});
            this.xrTableCell31.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell31.Multiline = true;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2, 100F);
            this.xrTableCell31.StylePriority.UseBorderColor = false;
            this.xrTableCell31.StylePriority.UseBorders = false;
            this.xrTableCell31.StylePriority.UseFont = false;
            this.xrTableCell31.StylePriority.UsePadding = false;
            this.xrTableCell31.StylePriority.UseTextAlignment = false;
            this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell31.Weight = 0.33131421256317206D;
            // 
            // CustomerAgeingEstatement
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageHeader,
            this.PageFooter,
            this.ReportFooter});
            this.DataMember = "Sp_Ac_InvoiceAgeingStatement";
            this.DataSource = this.accountsDataset1;
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(23, 0, 0, 19);
            this.ScriptsSource = "\r\nprivate void calculatedField1_GetValue(object sender, DevExpress.XtraReports.UI" +
    ".GetValueEventArgs e) {\r\n\r\n}\r\n";
            this.Version = "18.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTblAging)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        
    }
    public void  setCustomerAddress(string strAddress)
    {
        lblCustomerAddress.Text = strAddress;
    }

    private void lblTotalBalance_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (lblTotalBalance.Text != "")
        {
            try
            {
                lblTotalBalance.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(),lblTotalBalance.Text);
            }
            catch
            {
                //xrTableCell10.Text = "0";

            }
        }
        else
        {
            xrTableCell10.Text = "0";
        }
    }


    private void xrTableCell10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell10.Text != "")
        {
            try
            {
                xrTableCell10.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), xrTableCell10.Text);
            }
            catch
            {
                //xrTableCell10.Text = "0";

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
                xrTableCell14.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), xrTableCell14.Text);
            }
            catch
            {
                //xrTableCell14.Text = "0";
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
                xrTableCell15.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), xrTableCell15.Text);
            }
            catch
            {
                //xrTableCell15.Text = "0";
            }
        }
        else
        {
            xrTableCell15.Text = "0";
        }
    }
    private void lblCustomerAddress_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        
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
                xrTableCell35.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), xrTableCell35.Text);
            }
            catch
            {
               // xrTableCell35.Text = "0";

            }
        }
        else
        {
            xrTableCell35.Text = "0";
        }
    }

    private void xrTableCell14_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell14.Text != "")
        {
            try
            {
                xrTableCell14.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), xrTableCell14.Text);
            }
            catch
            {
                //xrTableCell14.Text = "0";
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
                xrTableCell15.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), xrTableCell15.Text);
            }
            catch
            {
                //xrTableCell15.Text = "0";
            }
        }
        else
        {
            xrTableCell15.Text = "0";
        }
    }

    private void xrTableCell19_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell19.Text != "")
        {
            try
            {
                xrTableCell19.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), xrTableCell19.Text);
            }
            catch
            {
                //xrTableCell19.Text = "0";
            }
        }
        else
        {
            xrTableCell19.Text = "0";
        }
    }

    private void xrTableCell20_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

        if (xrTblCellRptTitle.Text != "")
        {
            try
            {
                xrTblCellRptTitle.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), xrTblCellRptTitle.Text);
            }
            catch
            {
                //xrTableCell20.Text = "0";
            }
        }
        else
        {
            xrTblCellRptTitle.Text = "0";
        }
    
    }

       private void xrTableCell23_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (lblCustomerContactDetail.Text != "")
        {
            try
            {
                lblCustomerContactDetail.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), lblCustomerContactDetail.Text);
            }
            catch
            {
                //lblCustomerContactDetail.Text = 0;
            }
        }
        else
        {
            lblCustomerContactDetail.Text = "0";
        }
    }

    private void lblTotalBalance_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (lblTotalBalance.Text != "")
        {
            try
            {
                lblTotalBalance.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString(), lblTotalBalance.Text);
            }
            catch
            {
                //xrTableCell10.Text = "0";

            }
        }
        else
        {
            xrTableCell10.Text = "0";
        }
    }
}
