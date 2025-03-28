using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web;

/// <summary>
/// Summary description for PurchaseRequestPrint
/// </summary>
public class CreditApplication : DevExpress.XtraReports.UI.XtraReport
{
    LocationMaster objLocation = null;
    SystemParameter objsys = null;
    CurrencyMaster objCurrency = null;
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private ReportHeaderBand ReportHeader;
    private PageFooterBand PageFooter;
    private PurchaseDataSet purchaseDataSet1;
    private InventoryDataSet InventoryDataSet1;
    private XRPageInfo xrPageInfo2;
    private XRLabel xrLabel8;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel2;
    private XRPanel xrPanel1;
    private SalesDataSet salesDataSet1;
    private GroupHeaderBand GroupHeader1;
    private XRPanel xrPanel2;
    private XRLabel xrLabel66;
    private XRLabel xrLabel77;
    private XRLabel xrLabel76;
    private XRLabel xrLabel72;
    private XRPictureBox xrPictureBox2;
    private XRLabel xrLabel71;
    private XRLabel xrLabel73;
    private XRLabel xrLabel75;
    private XRLabel xrLabel74;
    private XRLabel xrLabel70;
    private XRLabel xrLabel65;
    private XRLabel xrLabel67;
    private XRLabel xrLabel69;
    private XRLabel xrLabel68;
    private XRLabel xrLabel64;
    private XRLabel xrLabel62;
    private XRLabel xrLabel59;
    private XRLabel xrLabel51;
    private XRLabel xrLabel50;
    private XRLabel xrLabel48;
    private XRLabel xrLabel58;
    private XRLabel xrLabel57;
    private XRLabel xrLabel63;
    private XRLabel xrLabel61;
    private XRLabel xrLabel56;
    private XRLabel xrLabel49;
    private XRLabel xrLabel54;
    private XRLabel xrLabel53;
    private XRLabel xrLabel55;
    private XRLabel xrLabel60;
    private XRLabel xrLabel52;
    private XRLabel xrLabel45;
    private XRLabel xrLabel46;
    private XRLabel xrLabel47;
    private XRLabel xrLabel42;
    private XRLabel xrLabel43;
    private XRLabel xrLabel44;
    private XRLabel xrLabel39;
    private XRLabel xrLabel40;
    private XRLabel xrLabel41;
    private XRLabel xrLabel36;
    private XRLabel xrLabel37;
    private XRLabel xrLabel38;
    private XRLabel xrLabel35;
    private XRLabel xrLabel34;
    private XRLabel xrLabel33;
    private XRLabel xrLabel32;
    private XRLabel xrLabel31;
    private XRLabel xrLabel30;
    private XRLabel xrLabel29;
    private XRLabel xrLabel26;
    private XRLabel xrLabel25;
    private XRLabel xrLabel23;
    private XRLabel xrLabel27;
    private XRLabel xrLabel28;
    private XRLabel xrLabel24;
    private XRLabel xrLabel22;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel17;
    private XRLabel xrLabel16;
    private XRLabel xrLabel14;
    private XRLabel xrLabel15;
    private XRLabel xrLabel12;
    private XRLabel xrLabel13;
    private XRLabel xrLabel10;
    private XRLabel xrLabel11;
    private XRLabel xrLabel7;
    private XRLabel xrLabel9;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRLabel xrLabel3;
    private XRLabel xrLabel1;
    private XRLabel xrLabel152;
    private XRLabel xrLabel151;
    private XRLabel xrLabel145;
    private XRLabel xrLabel146;
    private XRLabel xrLabel143;
    private XRLabel xrLabel144;
    private XRLabel xrLabel149;
    private XRLabel xrLabel150;
    private XRLabel xrLabel147;
    private XRLabel xrLabel148;
    private XRLabel xrLabel137;
    private XRLabel xrLabel138;
    private XRLabel xrLabel135;
    private XRLabel xrLabel136;
    private XRLabel xrLabel141;
    private XRLabel xrLabel142;
    private XRLabel xrLabel139;
    private XRLabel xrLabel140;
    private XRLabel xrLabel134;
    private XRLabel xrLabel133;
    private XRLabel xrLabel132;
    private XRLabel xrLabel131;
    private XRLabel xrLabel130;
    private XRLabel xrLabel129;
    private XRLabel xrLabel125;
    private XRLabel xrLabel124;
    private XRLabel xrLabel123;
    private XRLabel xrLabel128;
    private XRLabel xrLabel127;
    private XRLabel xrLabel126;
    private XRLabel xrLabel119;
    private XRLabel xrLabel118;
    private XRLabel xrLabel117;
    private XRLabel xrLabel122;
    private XRLabel xrLabel121;
    private XRLabel xrLabel120;
    private XRLabel xrLabel116;
    private XRLabel xrLabel112;
    private XRLabel xrLabel111;
    private XRLabel xrLabel110;
    private XRLabel xrLabel115;
    private XRLabel xrLabel114;
    private XRLabel xrLabel113;
    private XRLabel xrLabel106;
    private XRLabel xrLabel105;
    private XRLabel xrLabel104;
    private XRLabel xrLabel109;
    private XRLabel xrLabel108;
    private XRLabel xrLabel107;
    private XRLabel xrLabel100;
    private XRLabel xrLabel99;
    private XRLabel xrLabel98;
    private XRLabel xrLabel103;
    private XRLabel xrLabel102;
    private XRLabel xrLabel101;
    private XRLabel xrLabel94;
    private XRLabel xrLabel93;
    private XRLabel xrLabel92;
    private XRLabel xrLabel97;
    private XRLabel xrLabel96;
    private XRLabel xrLabel95;
    private XRLabel xrLabel89;
    private XRLabel xrLabel90;
    private XRLabel xrLabel91;
    private XRLabel xrLabel86;
    private XRLabel xrLabel87;
    private XRLabel xrLabel88;
    private XRLabel xrLabel83;
    private XRLabel xrLabel84;
    private XRLabel xrLabel85;
    private XRLabel xrLabel80;
    private XRLabel xrLabel81;
    private XRLabel xrLabel82;
    private XRLabel xrLabel79;
    private XRLabel xrLabel78;
    private XRLabel xrLabel153;
    private XRLabel xrLabel154;
    private XRLabel xrLabel155;
    private XRLabel xrLabel156;
    private ReportFooterBand ReportFooter;
    private XRPanel xrPanel3;
    private XRPanel xrPanel4;
    private XRLabel xrLabel166;
    private XRLabel xrLabel165;
    private XRLabel xrLabel164;
    private XRLabel xrLabel163;
    private XRLabel xrLabel162;
    private XRLabel xrLabel161;
    private XRLabel xrLabel160;
    private XRLabel xrLabel159;
    private XRLabel xrLabel158;
    private XRLabel xrLabel157;
    private XRLabel xrLabel167;
    private XRLabel xrLabel169;
    private XRLabel xrLabel168;
    private XRLabel xrLabel170;
    private XRLabel xrLabel171;
    private XRLabel xrLabel172;
    private XRLabel xrLabel173;
    private XRLabel xrLabel176;
    private XRLabel xrLabel175;
    private XRLabel xrLabel174;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public CreditApplication(string strConString)
    {
        InitializeComponent();
        objLocation = new LocationMaster(strConString);
        objsys = new SystemParameter(strConString);
        objCurrency = new CurrencyMaster(strConString);
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
    public void setDatefiltertext(string strdatefilter)
    {
        //xrlbldatefilter.Text = strdatefilter;
    }
    public void setSignature(string Url)
    {
        try
        {
            xrPictureBox2.ImageUrl = Url;
        }
        catch
        {
        }
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string resourceFileName = "CreditApplication.resx";
        System.Resources.ResourceManager resources = global::Resources.CreditApplication.ResourceManager;
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.purchaseDataSet1 = new PurchaseDataSet();
        this.InventoryDataSet1 = new InventoryDataSet();
        this.salesDataSet1 = new SalesDataSet();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel152 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel151 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel145 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel146 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel143 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel144 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel149 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel150 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel147 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel148 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel137 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel138 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel135 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel136 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel141 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel142 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel139 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel140 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel134 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel133 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel132 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel131 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel130 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel129 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel125 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel124 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel123 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel128 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel127 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel126 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel119 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel118 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel117 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel122 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel121 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel120 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel116 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel112 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel111 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel110 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel115 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel114 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel113 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel106 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel105 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel104 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel109 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel108 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel107 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel100 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel99 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel98 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel103 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel102 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel101 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel94 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel93 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel92 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel97 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel96 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel95 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel89 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel90 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel91 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel86 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel87 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel88 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel83 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel84 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel85 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel80 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel81 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel82 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel79 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel78 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel66 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel77 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel76 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel72 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel71 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel73 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel75 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel74 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel70 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel67 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel69 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel68 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel153 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel154 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel155 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel156 = new DevExpress.XtraReports.UI.XRLabel();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel169 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel168 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel166 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel165 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel164 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel163 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel162 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel161 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel160 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel159 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel158 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel157 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel167 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel170 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel171 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel172 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel173 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel174 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel175 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel176 = new DevExpress.XtraReports.UI.XRLabel();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.salesDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.HeightF = 0F;
        this.Detail.KeepTogether = true;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
        this.BottomMargin.HeightF = 0F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
        this.ReportHeader.HeightF = 86.1667F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPanel1.BorderWidth = 2F;
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrPictureBox1,
            this.xrLabel8});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 17.00001F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(785F, 69.16669F);
        this.xrPanel1.StylePriority.UseBorders = false;
        this.xrPanel1.StylePriority.UseBorderWidth = false;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(1.861898F, 9.333329F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(470.9059F, 21.95834F);
        this.xrLabel2.StylePriority.UseBorders = false;
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "xrLabel1";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox1.ImageUrl = "~\\Images\\PegasusLogo.png";
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(596.2369F, 2.333367F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(173.9582F, 66.83333F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrLabel8
        // 
        this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 46.16669F);
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(469.6428F, 23F);
        this.xrLabel8.StylePriority.UseBorders = false;
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.StylePriority.UseTextAlignment = false;
        this.xrLabel8.Text = "CONFIDENTIAL CREDIT APPLICATION";
        this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2});
        this.PageFooter.HeightF = 18.12499F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrPageInfo2.Format = "Page{0}Of {1}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(785.0001F, 18.12496F);
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
        // salesDataSet1
        // 
        this.salesDataSet1.DataSetName = "SalesDataSet";
        this.salesDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel2});
        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Pay_Mod_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader1.HeightF = 1646.959F;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // xrPanel2
        // 
        this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel176,
            this.xrLabel175,
            this.xrLabel174,
            this.xrLabel152,
            this.xrLabel151,
            this.xrLabel145,
            this.xrLabel146,
            this.xrLabel143,
            this.xrLabel144,
            this.xrLabel149,
            this.xrLabel150,
            this.xrLabel147,
            this.xrLabel148,
            this.xrLabel137,
            this.xrLabel138,
            this.xrLabel135,
            this.xrLabel136,
            this.xrLabel141,
            this.xrLabel142,
            this.xrLabel139,
            this.xrLabel140,
            this.xrLabel134,
            this.xrLabel133,
            this.xrLabel132,
            this.xrLabel131,
            this.xrLabel130,
            this.xrLabel129,
            this.xrLabel125,
            this.xrLabel124,
            this.xrLabel123,
            this.xrLabel128,
            this.xrLabel127,
            this.xrLabel126,
            this.xrLabel119,
            this.xrLabel118,
            this.xrLabel117,
            this.xrLabel122,
            this.xrLabel121,
            this.xrLabel120,
            this.xrLabel116,
            this.xrLabel112,
            this.xrLabel111,
            this.xrLabel110,
            this.xrLabel115,
            this.xrLabel114,
            this.xrLabel113,
            this.xrLabel106,
            this.xrLabel105,
            this.xrLabel104,
            this.xrLabel109,
            this.xrLabel108,
            this.xrLabel107,
            this.xrLabel100,
            this.xrLabel99,
            this.xrLabel98,
            this.xrLabel103,
            this.xrLabel102,
            this.xrLabel101,
            this.xrLabel94,
            this.xrLabel93,
            this.xrLabel92,
            this.xrLabel97,
            this.xrLabel96,
            this.xrLabel95,
            this.xrLabel89,
            this.xrLabel90,
            this.xrLabel91,
            this.xrLabel86,
            this.xrLabel87,
            this.xrLabel88,
            this.xrLabel83,
            this.xrLabel84,
            this.xrLabel85,
            this.xrLabel80,
            this.xrLabel81,
            this.xrLabel82,
            this.xrLabel79,
            this.xrLabel78,
            this.xrLabel66,
            this.xrLabel77,
            this.xrLabel76,
            this.xrLabel72,
            this.xrPictureBox2,
            this.xrLabel71,
            this.xrLabel73,
            this.xrLabel75,
            this.xrLabel74,
            this.xrLabel70,
            this.xrLabel65,
            this.xrLabel67,
            this.xrLabel69,
            this.xrLabel68,
            this.xrLabel64,
            this.xrLabel62,
            this.xrLabel59,
            this.xrLabel51,
            this.xrLabel50,
            this.xrLabel48,
            this.xrLabel58,
            this.xrLabel57,
            this.xrLabel63,
            this.xrLabel61,
            this.xrLabel56,
            this.xrLabel49,
            this.xrLabel54,
            this.xrLabel53,
            this.xrLabel55,
            this.xrLabel60,
            this.xrLabel52,
            this.xrLabel45,
            this.xrLabel46,
            this.xrLabel47,
            this.xrLabel42,
            this.xrLabel43,
            this.xrLabel44,
            this.xrLabel39,
            this.xrLabel40,
            this.xrLabel41,
            this.xrLabel36,
            this.xrLabel37,
            this.xrLabel38,
            this.xrLabel35,
            this.xrLabel34,
            this.xrLabel33,
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel29,
            this.xrLabel26,
            this.xrLabel25,
            this.xrLabel23,
            this.xrLabel27,
            this.xrLabel28,
            this.xrLabel24,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel14,
            this.xrLabel15,
            this.xrLabel12,
            this.xrLabel13,
            this.xrLabel10,
            this.xrLabel11,
            this.xrLabel7,
            this.xrLabel9,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel1});
        this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0.7499695F);
        this.xrPanel2.Name = "xrPanel2";
        this.xrPanel2.SizeF = new System.Drawing.SizeF(785.0002F, 1646.209F);
        this.xrPanel2.StylePriority.UseBorders = false;
        // 
        // xrLabel152
        // 
        this.xrLabel152.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel152.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account2_Contact_No")});
        this.xrLabel152.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel152.LocationFloat = new DevExpress.Utils.PointFloat(635.6382F, 1577.688F);
        this.xrLabel152.Name = "xrLabel152";
        this.xrLabel152.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel152.SizeF = new System.Drawing.SizeF(145.9607F, 23F);
        this.xrLabel152.StylePriority.UseBorders = false;
        this.xrLabel152.StylePriority.UseFont = false;
        this.xrLabel152.StylePriority.UsePadding = false;
        this.xrLabel152.StylePriority.UseTextAlignment = false;
        this.xrLabel152.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel151
        // 
        this.xrLabel151.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel151.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel151.LocationFloat = new DevExpress.Utils.PointFloat(622.4453F, 1577.688F);
        this.xrLabel151.Name = "xrLabel151";
        this.xrLabel151.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel151.SizeF = new System.Drawing.SizeF(13.19287F, 23F);
        this.xrLabel151.StylePriority.UseBorders = false;
        this.xrLabel151.StylePriority.UseFont = false;
        this.xrLabel151.StylePriority.UseTextAlignment = false;
        this.xrLabel151.Text = ":";
        this.xrLabel151.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel145
        // 
        this.xrLabel145.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel145.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account2_Banker_Name")});
        this.xrLabel145.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel145.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1577.688F);
        this.xrLabel145.Name = "xrLabel145";
        this.xrLabel145.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel145.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel145.StylePriority.UseBorders = false;
        this.xrLabel145.StylePriority.UseFont = false;
        this.xrLabel145.StylePriority.UsePadding = false;
        this.xrLabel145.StylePriority.UseTextAlignment = false;
        this.xrLabel145.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel146
        // 
        this.xrLabel146.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel146.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel146.LocationFloat = new DevExpress.Utils.PointFloat(3.124952F, 1577.688F);
        this.xrLabel146.Name = "xrLabel146";
        this.xrLabel146.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0, 100F);
        this.xrLabel146.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel146.StylePriority.UseBorders = false;
        this.xrLabel146.StylePriority.UseFont = false;
        this.xrLabel146.StylePriority.UsePadding = false;
        this.xrLabel146.StylePriority.UseTextAlignment = false;
        this.xrLabel146.Text = "Banker\'s Name";
        this.xrLabel146.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel143
        // 
        this.xrLabel143.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel143.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel143.LocationFloat = new DevExpress.Utils.PointFloat(3.124952F, 1551.688F);
        this.xrLabel143.Name = "xrLabel143";
        this.xrLabel143.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel143.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel143.StylePriority.UseBorders = false;
        this.xrLabel143.StylePriority.UseFont = false;
        this.xrLabel143.StylePriority.UsePadding = false;
        this.xrLabel143.StylePriority.UseTextAlignment = false;
        this.xrLabel143.Text = "2. Account Name";
        this.xrLabel143.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel144
        // 
        this.xrLabel144.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel144.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account2_Account_No")});
        this.xrLabel144.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel144.LocationFloat = new DevExpress.Utils.PointFloat(499.3464F, 1551.688F);
        this.xrLabel144.Name = "xrLabel144";
        this.xrLabel144.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel144.SizeF = new System.Drawing.SizeF(280.9869F, 23F);
        this.xrLabel144.StylePriority.UseBorders = false;
        this.xrLabel144.StylePriority.UseFont = false;
        this.xrLabel144.StylePriority.UsePadding = false;
        this.xrLabel144.StylePriority.UseTextAlignment = false;
        this.xrLabel144.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel149
        // 
        this.xrLabel149.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel149.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel149.LocationFloat = new DevExpress.Utils.PointFloat(150F, 1577.688F);
        this.xrLabel149.Name = "xrLabel149";
        this.xrLabel149.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel149.SizeF = new System.Drawing.SizeF(6.333221F, 23F);
        this.xrLabel149.StylePriority.UseBorders = false;
        this.xrLabel149.StylePriority.UseFont = false;
        this.xrLabel149.StylePriority.UseTextAlignment = false;
        this.xrLabel149.Text = ":";
        this.xrLabel149.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel150
        // 
        this.xrLabel150.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel150.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel150.LocationFloat = new DevExpress.Utils.PointFloat(426.9344F, 1577.688F);
        this.xrLabel150.Name = "xrLabel150";
        this.xrLabel150.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel150.SizeF = new System.Drawing.SizeF(14.58331F, 23F);
        this.xrLabel150.StylePriority.UseBorders = false;
        this.xrLabel150.StylePriority.UseFont = false;
        this.xrLabel150.StylePriority.UseTextAlignment = false;
        this.xrLabel150.Text = ":";
        this.xrLabel150.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel147
        // 
        this.xrLabel147.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel147.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account2_Fax_No")});
        this.xrLabel147.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel147.LocationFloat = new DevExpress.Utils.PointFloat(441.5177F, 1577.688F);
        this.xrLabel147.Name = "xrLabel147";
        this.xrLabel147.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel147.SizeF = new System.Drawing.SizeF(120.1751F, 23F);
        this.xrLabel147.StylePriority.UseBorders = false;
        this.xrLabel147.StylePriority.UseFont = false;
        this.xrLabel147.StylePriority.UsePadding = false;
        this.xrLabel147.StylePriority.UseTextAlignment = false;
        this.xrLabel147.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel148
        // 
        this.xrLabel148.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel148.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel148.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1577.688F);
        this.xrLabel148.Name = "xrLabel148";
        this.xrLabel148.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel148.SizeF = new System.Drawing.SizeF(74.14285F, 23F);
        this.xrLabel148.StylePriority.UseBorders = false;
        this.xrLabel148.StylePriority.UseFont = false;
        this.xrLabel148.StylePriority.UsePadding = false;
        this.xrLabel148.StylePriority.UseTextAlignment = false;
        this.xrLabel148.Text = "Fax No.";
        this.xrLabel148.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel137
        // 
        this.xrLabel137.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel137.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account2_Banker_Address")});
        this.xrLabel137.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel137.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1607.688F);
        this.xrLabel137.Name = "xrLabel137";
        this.xrLabel137.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel137.SizeF = new System.Drawing.SizeF(625.2657F, 23F);
        this.xrLabel137.StylePriority.UseBorders = false;
        this.xrLabel137.StylePriority.UseFont = false;
        this.xrLabel137.StylePriority.UsePadding = false;
        this.xrLabel137.StylePriority.UseTextAlignment = false;
        this.xrLabel137.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel138
        // 
        this.xrLabel138.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel138.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel138.LocationFloat = new DevExpress.Utils.PointFloat(561.6929F, 1577.688F);
        this.xrLabel138.Name = "xrLabel138";
        this.xrLabel138.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel138.SizeF = new System.Drawing.SizeF(60.75232F, 23F);
        this.xrLabel138.StylePriority.UseBorders = false;
        this.xrLabel138.StylePriority.UseFont = false;
        this.xrLabel138.StylePriority.UsePadding = false;
        this.xrLabel138.StylePriority.UseTextAlignment = false;
        this.xrLabel138.Text = "Tel. No.";
        this.xrLabel138.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel135
        // 
        this.xrLabel135.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel135.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel135.LocationFloat = new DevExpress.Utils.PointFloat(150F, 1607.688F);
        this.xrLabel135.Name = "xrLabel135";
        this.xrLabel135.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel135.SizeF = new System.Drawing.SizeF(6.333206F, 23F);
        this.xrLabel135.StylePriority.UseBorders = false;
        this.xrLabel135.StylePriority.UseFont = false;
        this.xrLabel135.StylePriority.UseTextAlignment = false;
        this.xrLabel135.Text = ":";
        this.xrLabel135.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel136
        // 
        this.xrLabel136.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel136.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel136.LocationFloat = new DevExpress.Utils.PointFloat(3.124968F, 1607.688F);
        this.xrLabel136.Name = "xrLabel136";
        this.xrLabel136.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0, 100F);
        this.xrLabel136.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel136.StylePriority.UseBorders = false;
        this.xrLabel136.StylePriority.UseFont = false;
        this.xrLabel136.StylePriority.UsePadding = false;
        this.xrLabel136.StylePriority.UseTextAlignment = false;
        this.xrLabel136.Text = "Banker\'s Address";
        this.xrLabel136.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel141
        // 
        this.xrLabel141.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel141.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel141.LocationFloat = new DevExpress.Utils.PointFloat(473.3047F, 1551.688F);
        this.xrLabel141.Name = "xrLabel141";
        this.xrLabel141.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel141.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel141.StylePriority.UseBorders = false;
        this.xrLabel141.StylePriority.UseFont = false;
        this.xrLabel141.StylePriority.UseTextAlignment = false;
        this.xrLabel141.Text = ":";
        this.xrLabel141.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel142
        // 
        this.xrLabel142.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel142.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account2_Account_Name")});
        this.xrLabel142.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel142.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1551.688F);
        this.xrLabel142.Name = "xrLabel142";
        this.xrLabel142.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel142.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel142.StylePriority.UseBorders = false;
        this.xrLabel142.StylePriority.UseFont = false;
        this.xrLabel142.StylePriority.UsePadding = false;
        this.xrLabel142.StylePriority.UseTextAlignment = false;
        this.xrLabel142.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel139
        // 
        this.xrLabel139.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel139.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel139.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1551.688F);
        this.xrLabel139.Name = "xrLabel139";
        this.xrLabel139.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel139.SizeF = new System.Drawing.SizeF(119.9762F, 23F);
        this.xrLabel139.StylePriority.UseBorders = false;
        this.xrLabel139.StylePriority.UseFont = false;
        this.xrLabel139.StylePriority.UsePadding = false;
        this.xrLabel139.StylePriority.UseTextAlignment = false;
        this.xrLabel139.Text = "Account No.";
        this.xrLabel139.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel140
        // 
        this.xrLabel140.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel140.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel140.LocationFloat = new DevExpress.Utils.PointFloat(150F, 1551.688F);
        this.xrLabel140.Name = "xrLabel140";
        this.xrLabel140.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel140.SizeF = new System.Drawing.SizeF(6.333221F, 23F);
        this.xrLabel140.StylePriority.UseBorders = false;
        this.xrLabel140.StylePriority.UseFont = false;
        this.xrLabel140.StylePriority.UseTextAlignment = false;
        this.xrLabel140.Text = ":";
        this.xrLabel140.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel134
        // 
        this.xrLabel134.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel134.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel134.LocationFloat = new DevExpress.Utils.PointFloat(150F, 1502.313F);
        this.xrLabel134.Name = "xrLabel134";
        this.xrLabel134.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel134.SizeF = new System.Drawing.SizeF(6.333206F, 23F);
        this.xrLabel134.StylePriority.UseBorders = false;
        this.xrLabel134.StylePriority.UseFont = false;
        this.xrLabel134.StylePriority.UseTextAlignment = false;
        this.xrLabel134.Text = ":";
        this.xrLabel134.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel133
        // 
        this.xrLabel133.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel133.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel133.LocationFloat = new DevExpress.Utils.PointFloat(3.124952F, 1502.313F);
        this.xrLabel133.Name = "xrLabel133";
        this.xrLabel133.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0, 100F);
        this.xrLabel133.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel133.StylePriority.UseBorders = false;
        this.xrLabel133.StylePriority.UseFont = false;
        this.xrLabel133.StylePriority.UsePadding = false;
        this.xrLabel133.StylePriority.UseTextAlignment = false;
        this.xrLabel133.Text = "Banker\'s Address";
        this.xrLabel133.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel132
        // 
        this.xrLabel132.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel132.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account1_Banker_Address")});
        this.xrLabel132.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel132.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1502.313F);
        this.xrLabel132.Name = "xrLabel132";
        this.xrLabel132.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel132.SizeF = new System.Drawing.SizeF(625.2657F, 23F);
        this.xrLabel132.StylePriority.UseBorders = false;
        this.xrLabel132.StylePriority.UseFont = false;
        this.xrLabel132.StylePriority.UsePadding = false;
        this.xrLabel132.StylePriority.UseTextAlignment = false;
        this.xrLabel132.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel131
        // 
        this.xrLabel131.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel131.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel131.LocationFloat = new DevExpress.Utils.PointFloat(622.4452F, 1472.313F);
        this.xrLabel131.Name = "xrLabel131";
        this.xrLabel131.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel131.SizeF = new System.Drawing.SizeF(13.19287F, 23F);
        this.xrLabel131.StylePriority.UseBorders = false;
        this.xrLabel131.StylePriority.UseFont = false;
        this.xrLabel131.StylePriority.UseTextAlignment = false;
        this.xrLabel131.Text = ":";
        this.xrLabel131.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel130
        // 
        this.xrLabel130.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel130.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel130.LocationFloat = new DevExpress.Utils.PointFloat(561.6929F, 1472.313F);
        this.xrLabel130.Name = "xrLabel130";
        this.xrLabel130.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel130.SizeF = new System.Drawing.SizeF(60.75232F, 23F);
        this.xrLabel130.StylePriority.UseBorders = false;
        this.xrLabel130.StylePriority.UseFont = false;
        this.xrLabel130.StylePriority.UsePadding = false;
        this.xrLabel130.StylePriority.UseTextAlignment = false;
        this.xrLabel130.Text = "Tel. No.";
        this.xrLabel130.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel129
        // 
        this.xrLabel129.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel129.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account1_Contact_No")});
        this.xrLabel129.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel129.LocationFloat = new DevExpress.Utils.PointFloat(635.6381F, 1472.313F);
        this.xrLabel129.Name = "xrLabel129";
        this.xrLabel129.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel129.SizeF = new System.Drawing.SizeF(145.9607F, 23F);
        this.xrLabel129.StylePriority.UseBorders = false;
        this.xrLabel129.StylePriority.UseFont = false;
        this.xrLabel129.StylePriority.UsePadding = false;
        this.xrLabel129.StylePriority.UseTextAlignment = false;
        this.xrLabel129.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel125
        // 
        this.xrLabel125.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel125.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel125.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1446.313F);
        this.xrLabel125.Name = "xrLabel125";
        this.xrLabel125.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel125.SizeF = new System.Drawing.SizeF(119.9762F, 23F);
        this.xrLabel125.StylePriority.UseBorders = false;
        this.xrLabel125.StylePriority.UseFont = false;
        this.xrLabel125.StylePriority.UsePadding = false;
        this.xrLabel125.StylePriority.UseTextAlignment = false;
        this.xrLabel125.Text = "Account No.";
        this.xrLabel125.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel124
        // 
        this.xrLabel124.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel124.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel124.LocationFloat = new DevExpress.Utils.PointFloat(150F, 1446.313F);
        this.xrLabel124.Name = "xrLabel124";
        this.xrLabel124.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel124.SizeF = new System.Drawing.SizeF(6.333221F, 23F);
        this.xrLabel124.StylePriority.UseBorders = false;
        this.xrLabel124.StylePriority.UseFont = false;
        this.xrLabel124.StylePriority.UseTextAlignment = false;
        this.xrLabel124.Text = ":";
        this.xrLabel124.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel123
        // 
        this.xrLabel123.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel123.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel123.LocationFloat = new DevExpress.Utils.PointFloat(473.3047F, 1446.313F);
        this.xrLabel123.Name = "xrLabel123";
        this.xrLabel123.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel123.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel123.StylePriority.UseBorders = false;
        this.xrLabel123.StylePriority.UseFont = false;
        this.xrLabel123.StylePriority.UseTextAlignment = false;
        this.xrLabel123.Text = ":";
        this.xrLabel123.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel128
        // 
        this.xrLabel128.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel128.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account1_Account_Name")});
        this.xrLabel128.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel128.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1446.313F);
        this.xrLabel128.Name = "xrLabel128";
        this.xrLabel128.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel128.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel128.StylePriority.UseBorders = false;
        this.xrLabel128.StylePriority.UseFont = false;
        this.xrLabel128.StylePriority.UsePadding = false;
        this.xrLabel128.StylePriority.UseTextAlignment = false;
        this.xrLabel128.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel127
        // 
        this.xrLabel127.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel127.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel127.LocationFloat = new DevExpress.Utils.PointFloat(3.124936F, 1446.313F);
        this.xrLabel127.Name = "xrLabel127";
        this.xrLabel127.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel127.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel127.StylePriority.UseBorders = false;
        this.xrLabel127.StylePriority.UseFont = false;
        this.xrLabel127.StylePriority.UsePadding = false;
        this.xrLabel127.StylePriority.UseTextAlignment = false;
        this.xrLabel127.Text = "1. Account Name";
        this.xrLabel127.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel126
        // 
        this.xrLabel126.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel126.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account1_Account_No")});
        this.xrLabel126.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel126.LocationFloat = new DevExpress.Utils.PointFloat(499.3464F, 1446.313F);
        this.xrLabel126.Name = "xrLabel126";
        this.xrLabel126.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel126.SizeF = new System.Drawing.SizeF(280.9869F, 23F);
        this.xrLabel126.StylePriority.UseBorders = false;
        this.xrLabel126.StylePriority.UseFont = false;
        this.xrLabel126.StylePriority.UsePadding = false;
        this.xrLabel126.StylePriority.UseTextAlignment = false;
        this.xrLabel126.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel119
        // 
        this.xrLabel119.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel119.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account1_Banker_Name")});
        this.xrLabel119.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel119.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1472.313F);
        this.xrLabel119.Name = "xrLabel119";
        this.xrLabel119.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel119.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel119.StylePriority.UseBorders = false;
        this.xrLabel119.StylePriority.UseFont = false;
        this.xrLabel119.StylePriority.UsePadding = false;
        this.xrLabel119.StylePriority.UseTextAlignment = false;
        this.xrLabel119.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel118
        // 
        this.xrLabel118.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel118.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel118.LocationFloat = new DevExpress.Utils.PointFloat(3.124936F, 1472.313F);
        this.xrLabel118.Name = "xrLabel118";
        this.xrLabel118.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0, 100F);
        this.xrLabel118.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel118.StylePriority.UseBorders = false;
        this.xrLabel118.StylePriority.UseFont = false;
        this.xrLabel118.StylePriority.UsePadding = false;
        this.xrLabel118.StylePriority.UseTextAlignment = false;
        this.xrLabel118.Text = "Banker\'s Name";
        this.xrLabel118.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel117
        // 
        this.xrLabel117.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel117.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account1_Fax_No")});
        this.xrLabel117.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel117.LocationFloat = new DevExpress.Utils.PointFloat(441.5178F, 1472.313F);
        this.xrLabel117.Name = "xrLabel117";
        this.xrLabel117.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel117.SizeF = new System.Drawing.SizeF(120.1751F, 23F);
        this.xrLabel117.StylePriority.UseBorders = false;
        this.xrLabel117.StylePriority.UseFont = false;
        this.xrLabel117.StylePriority.UsePadding = false;
        this.xrLabel117.StylePriority.UseTextAlignment = false;
        this.xrLabel117.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel122
        // 
        this.xrLabel122.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel122.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel122.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1472.313F);
        this.xrLabel122.Name = "xrLabel122";
        this.xrLabel122.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel122.SizeF = new System.Drawing.SizeF(74.14285F, 23F);
        this.xrLabel122.StylePriority.UseBorders = false;
        this.xrLabel122.StylePriority.UseFont = false;
        this.xrLabel122.StylePriority.UsePadding = false;
        this.xrLabel122.StylePriority.UseTextAlignment = false;
        this.xrLabel122.Text = "Fax No.";
        this.xrLabel122.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel121
        // 
        this.xrLabel121.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel121.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel121.LocationFloat = new DevExpress.Utils.PointFloat(150F, 1472.313F);
        this.xrLabel121.Name = "xrLabel121";
        this.xrLabel121.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel121.SizeF = new System.Drawing.SizeF(6.333221F, 23F);
        this.xrLabel121.StylePriority.UseBorders = false;
        this.xrLabel121.StylePriority.UseFont = false;
        this.xrLabel121.StylePriority.UseTextAlignment = false;
        this.xrLabel121.Text = ":";
        this.xrLabel121.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel120
        // 
        this.xrLabel120.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel120.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel120.LocationFloat = new DevExpress.Utils.PointFloat(426.9344F, 1472.313F);
        this.xrLabel120.Name = "xrLabel120";
        this.xrLabel120.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel120.SizeF = new System.Drawing.SizeF(14.58331F, 23F);
        this.xrLabel120.StylePriority.UseBorders = false;
        this.xrLabel120.StylePriority.UseFont = false;
        this.xrLabel120.StylePriority.UseTextAlignment = false;
        this.xrLabel120.Text = ":";
        this.xrLabel120.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel116
        // 
        this.xrLabel116.BackColor = System.Drawing.Color.Empty;
        this.xrLabel116.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel116.BorderWidth = 2F;
        this.xrLabel116.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel116.LocationFloat = new DevExpress.Utils.PointFloat(3.125079F, 1407.583F);
        this.xrLabel116.Name = "xrLabel116";
        this.xrLabel116.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel116.SizeF = new System.Drawing.SizeF(170.8332F, 23F);
        this.xrLabel116.StylePriority.UseBackColor = false;
        this.xrLabel116.StylePriority.UseBorders = false;
        this.xrLabel116.StylePriority.UseBorderWidth = false;
        this.xrLabel116.StylePriority.UseFont = false;
        this.xrLabel116.Text = "BANK REFERENCES";
        // 
        // xrLabel112
        // 
        this.xrLabel112.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel112.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier3_Fax_No")});
        this.xrLabel112.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel112.LocationFloat = new DevExpress.Utils.PointFloat(499.3464F, 1337.021F);
        this.xrLabel112.Name = "xrLabel112";
        this.xrLabel112.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel112.SizeF = new System.Drawing.SizeF(280.9869F, 23F);
        this.xrLabel112.StylePriority.UseBorders = false;
        this.xrLabel112.StylePriority.UseFont = false;
        this.xrLabel112.StylePriority.UsePadding = false;
        this.xrLabel112.StylePriority.UseTextAlignment = false;
        this.xrLabel112.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel111
        // 
        this.xrLabel111.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel111.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel111.LocationFloat = new DevExpress.Utils.PointFloat(3.124952F, 1337.021F);
        this.xrLabel111.Name = "xrLabel111";
        this.xrLabel111.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0, 100F);
        this.xrLabel111.SizeF = new System.Drawing.SizeF(125.3048F, 23F);
        this.xrLabel111.StylePriority.UseBorders = false;
        this.xrLabel111.StylePriority.UseFont = false;
        this.xrLabel111.StylePriority.UsePadding = false;
        this.xrLabel111.StylePriority.UseTextAlignment = false;
        this.xrLabel111.Text = "Tel. No.";
        this.xrLabel111.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel110
        // 
        this.xrLabel110.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel110.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier3_Contact_No")});
        this.xrLabel110.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel110.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1337.021F);
        this.xrLabel110.Name = "xrLabel110";
        this.xrLabel110.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel110.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel110.StylePriority.UseBorders = false;
        this.xrLabel110.StylePriority.UseFont = false;
        this.xrLabel110.StylePriority.UsePadding = false;
        this.xrLabel110.StylePriority.UseTextAlignment = false;
        this.xrLabel110.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel115
        // 
        this.xrLabel115.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel115.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel115.LocationFloat = new DevExpress.Utils.PointFloat(473.3047F, 1337.021F);
        this.xrLabel115.Name = "xrLabel115";
        this.xrLabel115.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel115.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel115.StylePriority.UseBorders = false;
        this.xrLabel115.StylePriority.UseFont = false;
        this.xrLabel115.StylePriority.UseTextAlignment = false;
        this.xrLabel115.Text = ":";
        this.xrLabel115.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel114
        // 
        this.xrLabel114.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel114.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel114.LocationFloat = new DevExpress.Utils.PointFloat(128.4297F, 1337.021F);
        this.xrLabel114.Name = "xrLabel114";
        this.xrLabel114.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel114.SizeF = new System.Drawing.SizeF(27.90352F, 23F);
        this.xrLabel114.StylePriority.UseBorders = false;
        this.xrLabel114.StylePriority.UseFont = false;
        this.xrLabel114.StylePriority.UseTextAlignment = false;
        this.xrLabel114.Text = ":";
        this.xrLabel114.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel113
        // 
        this.xrLabel113.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel113.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel113.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1337.021F);
        this.xrLabel113.Name = "xrLabel113";
        this.xrLabel113.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel113.SizeF = new System.Drawing.SizeF(119.9762F, 23F);
        this.xrLabel113.StylePriority.UseBorders = false;
        this.xrLabel113.StylePriority.UseFont = false;
        this.xrLabel113.StylePriority.UsePadding = false;
        this.xrLabel113.StylePriority.UseTextAlignment = false;
        this.xrLabel113.Text = "Fax No.";
        this.xrLabel113.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel106
        // 
        this.xrLabel106.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel106.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel106.LocationFloat = new DevExpress.Utils.PointFloat(473.3047F, 1314.021F);
        this.xrLabel106.Name = "xrLabel106";
        this.xrLabel106.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel106.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel106.StylePriority.UseBorders = false;
        this.xrLabel106.StylePriority.UseFont = false;
        this.xrLabel106.StylePriority.UseTextAlignment = false;
        this.xrLabel106.Text = ":";
        this.xrLabel106.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel105
        // 
        this.xrLabel105.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel105.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel105.LocationFloat = new DevExpress.Utils.PointFloat(128.4297F, 1314.021F);
        this.xrLabel105.Name = "xrLabel105";
        this.xrLabel105.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel105.SizeF = new System.Drawing.SizeF(27.90352F, 23F);
        this.xrLabel105.StylePriority.UseBorders = false;
        this.xrLabel105.StylePriority.UseFont = false;
        this.xrLabel105.StylePriority.UseTextAlignment = false;
        this.xrLabel105.Text = ":";
        this.xrLabel105.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel104
        // 
        this.xrLabel104.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel104.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel104.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1314.021F);
        this.xrLabel104.Name = "xrLabel104";
        this.xrLabel104.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel104.SizeF = new System.Drawing.SizeF(119.9762F, 23F);
        this.xrLabel104.StylePriority.UseBorders = false;
        this.xrLabel104.StylePriority.UseFont = false;
        this.xrLabel104.StylePriority.UsePadding = false;
        this.xrLabel104.StylePriority.UseTextAlignment = false;
        this.xrLabel104.Text = "Contact Person";
        this.xrLabel104.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel109
        // 
        this.xrLabel109.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel109.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier3_Contact_Person")});
        this.xrLabel109.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel109.LocationFloat = new DevExpress.Utils.PointFloat(499.3464F, 1314.021F);
        this.xrLabel109.Name = "xrLabel109";
        this.xrLabel109.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel109.SizeF = new System.Drawing.SizeF(280.9869F, 23F);
        this.xrLabel109.StylePriority.UseBorders = false;
        this.xrLabel109.StylePriority.UseFont = false;
        this.xrLabel109.StylePriority.UsePadding = false;
        this.xrLabel109.StylePriority.UseTextAlignment = false;
        this.xrLabel109.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel108
        // 
        this.xrLabel108.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel108.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel108.LocationFloat = new DevExpress.Utils.PointFloat(3.124952F, 1314.021F);
        this.xrLabel108.Name = "xrLabel108";
        this.xrLabel108.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel108.SizeF = new System.Drawing.SizeF(125.3048F, 23F);
        this.xrLabel108.StylePriority.UseBorders = false;
        this.xrLabel108.StylePriority.UseFont = false;
        this.xrLabel108.StylePriority.UsePadding = false;
        this.xrLabel108.StylePriority.UseTextAlignment = false;
        this.xrLabel108.Text = "3. Supplier Name";
        this.xrLabel108.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel107
        // 
        this.xrLabel107.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel107.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier3_Company_Name")});
        this.xrLabel107.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel107.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1314.021F);
        this.xrLabel107.Name = "xrLabel107";
        this.xrLabel107.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel107.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel107.StylePriority.UseBorders = false;
        this.xrLabel107.StylePriority.UseFont = false;
        this.xrLabel107.StylePriority.UsePadding = false;
        this.xrLabel107.StylePriority.UseTextAlignment = false;
        this.xrLabel107.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel100
        // 
        this.xrLabel100.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel100.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel100.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1249.438F);
        this.xrLabel100.Name = "xrLabel100";
        this.xrLabel100.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel100.SizeF = new System.Drawing.SizeF(119.9762F, 23F);
        this.xrLabel100.StylePriority.UseBorders = false;
        this.xrLabel100.StylePriority.UseFont = false;
        this.xrLabel100.StylePriority.UsePadding = false;
        this.xrLabel100.StylePriority.UseTextAlignment = false;
        this.xrLabel100.Text = "Contact Person";
        this.xrLabel100.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel99
        // 
        this.xrLabel99.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel99.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel99.LocationFloat = new DevExpress.Utils.PointFloat(128.4297F, 1249.438F);
        this.xrLabel99.Name = "xrLabel99";
        this.xrLabel99.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel99.SizeF = new System.Drawing.SizeF(27.90352F, 23F);
        this.xrLabel99.StylePriority.UseBorders = false;
        this.xrLabel99.StylePriority.UseFont = false;
        this.xrLabel99.StylePriority.UseTextAlignment = false;
        this.xrLabel99.Text = ":";
        this.xrLabel99.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel98
        // 
        this.xrLabel98.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel98.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel98.LocationFloat = new DevExpress.Utils.PointFloat(473.3047F, 1249.438F);
        this.xrLabel98.Name = "xrLabel98";
        this.xrLabel98.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel98.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel98.StylePriority.UseBorders = false;
        this.xrLabel98.StylePriority.UseFont = false;
        this.xrLabel98.StylePriority.UseTextAlignment = false;
        this.xrLabel98.Text = ":";
        this.xrLabel98.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel103
        // 
        this.xrLabel103.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel103.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier2_Company_Name")});
        this.xrLabel103.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel103.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1249.438F);
        this.xrLabel103.Name = "xrLabel103";
        this.xrLabel103.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel103.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel103.StylePriority.UseBorders = false;
        this.xrLabel103.StylePriority.UseFont = false;
        this.xrLabel103.StylePriority.UsePadding = false;
        this.xrLabel103.StylePriority.UseTextAlignment = false;
        this.xrLabel103.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel102
        // 
        this.xrLabel102.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel102.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel102.LocationFloat = new DevExpress.Utils.PointFloat(3.124936F, 1249.438F);
        this.xrLabel102.Name = "xrLabel102";
        this.xrLabel102.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel102.SizeF = new System.Drawing.SizeF(125.3048F, 23F);
        this.xrLabel102.StylePriority.UseBorders = false;
        this.xrLabel102.StylePriority.UseFont = false;
        this.xrLabel102.StylePriority.UsePadding = false;
        this.xrLabel102.StylePriority.UseTextAlignment = false;
        this.xrLabel102.Text = "2. Supplier Name";
        this.xrLabel102.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel101
        // 
        this.xrLabel101.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel101.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier2_Contact_Person")});
        this.xrLabel101.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel101.LocationFloat = new DevExpress.Utils.PointFloat(499.3464F, 1249.438F);
        this.xrLabel101.Name = "xrLabel101";
        this.xrLabel101.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel101.SizeF = new System.Drawing.SizeF(280.9869F, 23F);
        this.xrLabel101.StylePriority.UseBorders = false;
        this.xrLabel101.StylePriority.UseFont = false;
        this.xrLabel101.StylePriority.UsePadding = false;
        this.xrLabel101.StylePriority.UseTextAlignment = false;
        this.xrLabel101.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel94
        // 
        this.xrLabel94.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel94.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier2_Contact_No")});
        this.xrLabel94.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel94.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1272.438F);
        this.xrLabel94.Name = "xrLabel94";
        this.xrLabel94.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel94.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel94.StylePriority.UseBorders = false;
        this.xrLabel94.StylePriority.UseFont = false;
        this.xrLabel94.StylePriority.UsePadding = false;
        this.xrLabel94.StylePriority.UseTextAlignment = false;
        this.xrLabel94.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel93
        // 
        this.xrLabel93.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel93.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel93.LocationFloat = new DevExpress.Utils.PointFloat(3.124936F, 1272.438F);
        this.xrLabel93.Name = "xrLabel93";
        this.xrLabel93.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0, 100F);
        this.xrLabel93.SizeF = new System.Drawing.SizeF(125.3048F, 23F);
        this.xrLabel93.StylePriority.UseBorders = false;
        this.xrLabel93.StylePriority.UseFont = false;
        this.xrLabel93.StylePriority.UsePadding = false;
        this.xrLabel93.StylePriority.UseTextAlignment = false;
        this.xrLabel93.Text = "Tel. No.";
        this.xrLabel93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel92
        // 
        this.xrLabel92.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel92.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier2_Fax_No")});
        this.xrLabel92.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel92.LocationFloat = new DevExpress.Utils.PointFloat(499.3464F, 1272.438F);
        this.xrLabel92.Name = "xrLabel92";
        this.xrLabel92.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel92.SizeF = new System.Drawing.SizeF(280.9869F, 23F);
        this.xrLabel92.StylePriority.UseBorders = false;
        this.xrLabel92.StylePriority.UseFont = false;
        this.xrLabel92.StylePriority.UsePadding = false;
        this.xrLabel92.StylePriority.UseTextAlignment = false;
        this.xrLabel92.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel97
        // 
        this.xrLabel97.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel97.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel97.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1272.438F);
        this.xrLabel97.Name = "xrLabel97";
        this.xrLabel97.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel97.SizeF = new System.Drawing.SizeF(119.9762F, 23F);
        this.xrLabel97.StylePriority.UseBorders = false;
        this.xrLabel97.StylePriority.UseFont = false;
        this.xrLabel97.StylePriority.UsePadding = false;
        this.xrLabel97.StylePriority.UseTextAlignment = false;
        this.xrLabel97.Text = "Fax No.";
        this.xrLabel97.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel96
        // 
        this.xrLabel96.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel96.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel96.LocationFloat = new DevExpress.Utils.PointFloat(128.4297F, 1272.438F);
        this.xrLabel96.Name = "xrLabel96";
        this.xrLabel96.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel96.SizeF = new System.Drawing.SizeF(27.90352F, 23F);
        this.xrLabel96.StylePriority.UseBorders = false;
        this.xrLabel96.StylePriority.UseFont = false;
        this.xrLabel96.StylePriority.UseTextAlignment = false;
        this.xrLabel96.Text = ":";
        this.xrLabel96.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel95
        // 
        this.xrLabel95.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel95.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel95.LocationFloat = new DevExpress.Utils.PointFloat(473.3047F, 1272.438F);
        this.xrLabel95.Name = "xrLabel95";
        this.xrLabel95.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel95.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel95.StylePriority.UseBorders = false;
        this.xrLabel95.StylePriority.UseFont = false;
        this.xrLabel95.StylePriority.UseTextAlignment = false;
        this.xrLabel95.Text = ":";
        this.xrLabel95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel89
        // 
        this.xrLabel89.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel89.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier1_Fax_No")});
        this.xrLabel89.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel89.LocationFloat = new DevExpress.Utils.PointFloat(499.3464F, 1203.729F);
        this.xrLabel89.Name = "xrLabel89";
        this.xrLabel89.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel89.SizeF = new System.Drawing.SizeF(280.9869F, 23F);
        this.xrLabel89.StylePriority.UseBorders = false;
        this.xrLabel89.StylePriority.UseFont = false;
        this.xrLabel89.StylePriority.UsePadding = false;
        this.xrLabel89.StylePriority.UseTextAlignment = false;
        this.xrLabel89.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel90
        // 
        this.xrLabel90.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel90.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel90.LocationFloat = new DevExpress.Utils.PointFloat(3.124952F, 1203.729F);
        this.xrLabel90.Name = "xrLabel90";
        this.xrLabel90.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0, 100F);
        this.xrLabel90.SizeF = new System.Drawing.SizeF(125.3048F, 23F);
        this.xrLabel90.StylePriority.UseBorders = false;
        this.xrLabel90.StylePriority.UseFont = false;
        this.xrLabel90.StylePriority.UsePadding = false;
        this.xrLabel90.StylePriority.UseTextAlignment = false;
        this.xrLabel90.Text = "Tel. No.";
        this.xrLabel90.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel91
        // 
        this.xrLabel91.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel91.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier1_Contact_No")});
        this.xrLabel91.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel91.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1203.729F);
        this.xrLabel91.Name = "xrLabel91";
        this.xrLabel91.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel91.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel91.StylePriority.UseBorders = false;
        this.xrLabel91.StylePriority.UseFont = false;
        this.xrLabel91.StylePriority.UsePadding = false;
        this.xrLabel91.StylePriority.UseTextAlignment = false;
        this.xrLabel91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel86
        // 
        this.xrLabel86.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel86.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel86.LocationFloat = new DevExpress.Utils.PointFloat(473.3047F, 1203.729F);
        this.xrLabel86.Name = "xrLabel86";
        this.xrLabel86.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel86.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel86.StylePriority.UseBorders = false;
        this.xrLabel86.StylePriority.UseFont = false;
        this.xrLabel86.StylePriority.UseTextAlignment = false;
        this.xrLabel86.Text = ":";
        this.xrLabel86.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel87
        // 
        this.xrLabel87.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel87.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel87.LocationFloat = new DevExpress.Utils.PointFloat(128.4297F, 1203.729F);
        this.xrLabel87.Name = "xrLabel87";
        this.xrLabel87.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel87.SizeF = new System.Drawing.SizeF(27.90352F, 23F);
        this.xrLabel87.StylePriority.UseBorders = false;
        this.xrLabel87.StylePriority.UseFont = false;
        this.xrLabel87.StylePriority.UseTextAlignment = false;
        this.xrLabel87.Text = ":";
        this.xrLabel87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel88
        // 
        this.xrLabel88.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel88.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel88.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1203.729F);
        this.xrLabel88.Name = "xrLabel88";
        this.xrLabel88.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel88.SizeF = new System.Drawing.SizeF(119.9762F, 23F);
        this.xrLabel88.StylePriority.UseBorders = false;
        this.xrLabel88.StylePriority.UseFont = false;
        this.xrLabel88.StylePriority.UsePadding = false;
        this.xrLabel88.StylePriority.UseTextAlignment = false;
        this.xrLabel88.Text = "Fax No.";
        this.xrLabel88.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel83
        // 
        this.xrLabel83.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel83.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel83.LocationFloat = new DevExpress.Utils.PointFloat(473.3048F, 1180.729F);
        this.xrLabel83.Name = "xrLabel83";
        this.xrLabel83.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel83.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel83.StylePriority.UseBorders = false;
        this.xrLabel83.StylePriority.UseFont = false;
        this.xrLabel83.StylePriority.UseTextAlignment = false;
        this.xrLabel83.Text = ":";
        this.xrLabel83.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel84
        // 
        this.xrLabel84.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel84.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel84.LocationFloat = new DevExpress.Utils.PointFloat(128.4297F, 1180.729F);
        this.xrLabel84.Name = "xrLabel84";
        this.xrLabel84.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel84.SizeF = new System.Drawing.SizeF(27.90352F, 23F);
        this.xrLabel84.StylePriority.UseBorders = false;
        this.xrLabel84.StylePriority.UseFont = false;
        this.xrLabel84.StylePriority.UseTextAlignment = false;
        this.xrLabel84.Text = ":";
        this.xrLabel84.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel85
        // 
        this.xrLabel85.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel85.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel85.LocationFloat = new DevExpress.Utils.PointFloat(352.7916F, 1180.729F);
        this.xrLabel85.Name = "xrLabel85";
        this.xrLabel85.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel85.SizeF = new System.Drawing.SizeF(119.9762F, 23F);
        this.xrLabel85.StylePriority.UseBorders = false;
        this.xrLabel85.StylePriority.UseFont = false;
        this.xrLabel85.StylePriority.UsePadding = false;
        this.xrLabel85.StylePriority.UseTextAlignment = false;
        this.xrLabel85.Text = "Contact Person";
        this.xrLabel85.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel80
        // 
        this.xrLabel80.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel80.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier1_Contact_Person")});
        this.xrLabel80.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel80.LocationFloat = new DevExpress.Utils.PointFloat(499.3464F, 1180.729F);
        this.xrLabel80.Name = "xrLabel80";
        this.xrLabel80.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel80.SizeF = new System.Drawing.SizeF(280.9869F, 23F);
        this.xrLabel80.StylePriority.UseBorders = false;
        this.xrLabel80.StylePriority.UseFont = false;
        this.xrLabel80.StylePriority.UsePadding = false;
        this.xrLabel80.StylePriority.UseTextAlignment = false;
        this.xrLabel80.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel81
        // 
        this.xrLabel81.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel81.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel81.LocationFloat = new DevExpress.Utils.PointFloat(3.124952F, 1180.729F);
        this.xrLabel81.Name = "xrLabel81";
        this.xrLabel81.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel81.SizeF = new System.Drawing.SizeF(125.3048F, 23F);
        this.xrLabel81.StylePriority.UseBorders = false;
        this.xrLabel81.StylePriority.UseFont = false;
        this.xrLabel81.StylePriority.UsePadding = false;
        this.xrLabel81.StylePriority.UseTextAlignment = false;
        this.xrLabel81.Text = "1. Supplier Name";
        this.xrLabel81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel82
        // 
        this.xrLabel82.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel82.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Supplier1_Company_Name")});
        this.xrLabel82.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel82.LocationFloat = new DevExpress.Utils.PointFloat(156.3332F, 1180.729F);
        this.xrLabel82.Name = "xrLabel82";
        this.xrLabel82.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel82.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel82.StylePriority.UseBorders = false;
        this.xrLabel82.StylePriority.UseFont = false;
        this.xrLabel82.StylePriority.UsePadding = false;
        this.xrLabel82.StylePriority.UseTextAlignment = false;
        this.xrLabel82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel79
        // 
        this.xrLabel79.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel79.Font = new System.Drawing.Font("Times New Roman", 11.25F);
        this.xrLabel79.LocationFloat = new DevExpress.Utils.PointFloat(3.125079F, 1100.75F);
        this.xrLabel79.Multiline = true;
        this.xrLabel79.Name = "xrLabel79";
        this.xrLabel79.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel79.SizeF = new System.Drawing.SizeF(778.4736F, 55F);
        this.xrLabel79.StylePriority.UseBorders = false;
        this.xrLabel79.StylePriority.UseFont = false;
        this.xrLabel79.StylePriority.UsePadding = false;
        this.xrLabel79.StylePriority.UseTextAlignment = false;
        this.xrLabel79.Text = "This is my / our authorization to release information to Pegasus International Co" +
            "mputer co.. For the purpose of\r\nsupporting\r\nThe Credit Application .Trade Refere" +
            "nces";
        this.xrLabel79.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel78
        // 
        this.xrLabel78.BackColor = System.Drawing.Color.Empty;
        this.xrLabel78.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel78.BorderWidth = 2F;
        this.xrLabel78.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel78.LocationFloat = new DevExpress.Utils.PointFloat(3.125064F, 1071.25F);
        this.xrLabel78.Name = "xrLabel78";
        this.xrLabel78.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel78.SizeF = new System.Drawing.SizeF(128.1249F, 23F);
        this.xrLabel78.StylePriority.UseBackColor = false;
        this.xrLabel78.StylePriority.UseBorders = false;
        this.xrLabel78.StylePriority.UseBorderWidth = false;
        this.xrLabel78.StylePriority.UseFont = false;
        this.xrLabel78.Text = "REFERENCES";
        // 
        // xrLabel66
        // 
        this.xrLabel66.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel66.Font = new System.Drawing.Font("Times New Roman", 11.25F);
        this.xrLabel66.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 980.1147F);
        this.xrLabel66.Multiline = true;
        this.xrLabel66.Name = "xrLabel66";
        this.xrLabel66.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel66.SizeF = new System.Drawing.SizeF(777.2083F, 23F);
        this.xrLabel66.StylePriority.UseBorders = false;
        this.xrLabel66.StylePriority.UseFont = false;
        this.xrLabel66.StylePriority.UsePadding = false;
        this.xrLabel66.StylePriority.UseTextAlignment = false;
        this.xrLabel66.Text = "Hereby request for a credit facility and give the power of attorney to above auth" +
            "orized personnel whose names\r\ndesignations and signatures are given thereof to a" +
            "ct on behalf my our company";
        this.xrLabel66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel77
        // 
        this.xrLabel77.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel77.Font = new System.Drawing.Font("Times New Roman", 11.25F);
        this.xrLabel77.LocationFloat = new DevExpress.Utils.PointFloat(3.124984F, 957.1147F);
        this.xrLabel77.Name = "xrLabel77";
        this.xrLabel77.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel77.SizeF = new System.Drawing.SizeF(777.2083F, 23F);
        this.xrLabel77.StylePriority.UseBorders = false;
        this.xrLabel77.StylePriority.UseFont = false;
        this.xrLabel77.StylePriority.UsePadding = false;
        this.xrLabel77.StylePriority.UseTextAlignment = false;
        this.xrLabel77.Text = "The sponsor/ Partners/Owner/s of";
        this.xrLabel77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel76
        // 
        this.xrLabel76.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel76.Font = new System.Drawing.Font("Times New Roman", 11.25F);
        this.xrLabel76.LocationFloat = new DevExpress.Utils.PointFloat(270.8334F, 920.8333F);
        this.xrLabel76.Name = "xrLabel76";
        this.xrLabel76.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel76.SizeF = new System.Drawing.SizeF(364.8047F, 23F);
        this.xrLabel76.StylePriority.UseBorders = false;
        this.xrLabel76.StylePriority.UseFont = false;
        this.xrLabel76.StylePriority.UsePadding = false;
        this.xrLabel76.StylePriority.UseTextAlignment = false;
        this.xrLabel76.Text = "I/We";
        this.xrLabel76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel72
        // 
        this.xrLabel72.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel72.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel72.LocationFloat = new DevExpress.Utils.PointFloat(1.861898F, 910.4167F);
        this.xrLabel72.Name = "xrLabel72";
        this.xrLabel72.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel72.SizeF = new System.Drawing.SizeF(202.3047F, 23F);
        this.xrLabel72.StylePriority.UseBorders = false;
        this.xrLabel72.StylePriority.UseFont = false;
        this.xrLabel72.StylePriority.UsePadding = false;
        this.xrLabel72.StylePriority.UseTextAlignment = false;
        this.xrLabel72.Text = "Power of attorney";
        this.xrLabel72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrPictureBox2
        // 
        this.xrPictureBox2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox2.ImageUrl = "~\\Images\\PegasusLogo.png";
        this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(583.3463F, 831.1564F);
        this.xrPictureBox2.Name = "xrPictureBox2";
        this.xrPictureBox2.SizeF = new System.Drawing.SizeF(198.2525F, 66.83331F);
        this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox2.StylePriority.UseBorders = false;
        // 
        // xrLabel71
        // 
        this.xrLabel71.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel71.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel71.LocationFloat = new DevExpress.Utils.PointFloat(205.4298F, 831.1564F);
        this.xrLabel71.Name = "xrLabel71";
        this.xrLabel71.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel71.SizeF = new System.Drawing.SizeF(27.90352F, 33.41681F);
        this.xrLabel71.StylePriority.UseBorders = false;
        this.xrLabel71.StylePriority.UseFont = false;
        this.xrLabel71.StylePriority.UseTextAlignment = false;
        this.xrLabel71.Text = ":";
        this.xrLabel71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel73
        // 
        this.xrLabel73.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel73.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel73.LocationFloat = new DevExpress.Utils.PointFloat(205.4298F, 864.5732F);
        this.xrLabel73.Name = "xrLabel73";
        this.xrLabel73.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel73.SizeF = new System.Drawing.SizeF(27.90353F, 33.41656F);
        this.xrLabel73.StylePriority.UseBorders = false;
        this.xrLabel73.StylePriority.UseFont = false;
        this.xrLabel73.StylePriority.UseTextAlignment = false;
        this.xrLabel73.Text = ":";
        this.xrLabel73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel75
        // 
        this.xrLabel75.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel75.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel75.LocationFloat = new DevExpress.Utils.PointFloat(3.125079F, 864.5731F);
        this.xrLabel75.Name = "xrLabel75";
        this.xrLabel75.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel75.SizeF = new System.Drawing.SizeF(202.3047F, 33.41663F);
        this.xrLabel75.StylePriority.UseBorders = false;
        this.xrLabel75.StylePriority.UseFont = false;
        this.xrLabel75.StylePriority.UsePadding = false;
        this.xrLabel75.StylePriority.UseTextAlignment = false;
        this.xrLabel75.Text = "Designation";
        this.xrLabel75.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel74
        // 
        this.xrLabel74.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel74.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.AuthorizedPersonDesignation")});
        this.xrLabel74.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel74.LocationFloat = new DevExpress.Utils.PointFloat(233.3334F, 864.5732F);
        this.xrLabel74.Name = "xrLabel74";
        this.xrLabel74.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel74.SizeF = new System.Drawing.SizeF(239.4344F, 33.41656F);
        this.xrLabel74.StylePriority.UseBorders = false;
        this.xrLabel74.StylePriority.UseFont = false;
        this.xrLabel74.StylePriority.UsePadding = false;
        this.xrLabel74.StylePriority.UseTextAlignment = false;
        this.xrLabel74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel70
        // 
        this.xrLabel70.BackColor = System.Drawing.Color.Empty;
        this.xrLabel70.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel70.BorderWidth = 2F;
        this.xrLabel70.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel70.LocationFloat = new DevExpress.Utils.PointFloat(3.125032F, 783.552F);
        this.xrLabel70.Name = "xrLabel70";
        this.xrLabel70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel70.SizeF = new System.Drawing.SizeF(657.8618F, 22.99994F);
        this.xrLabel70.StylePriority.UseBackColor = false;
        this.xrLabel70.StylePriority.UseBorders = false;
        this.xrLabel70.StylePriority.UseBorderWidth = false;
        this.xrLabel70.StylePriority.UseFont = false;
        this.xrLabel70.Text = "SPECIMAN SIGNATURE OF AUTHORIZED PERSON TO SIGN PURCHASE ORDER";
        // 
        // xrLabel65
        // 
        this.xrLabel65.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel65.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(476.0548F, 831.1564F);
        this.xrLabel65.Name = "xrLabel65";
        this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel65.SizeF = new System.Drawing.SizeF(85.63803F, 66.83344F);
        this.xrLabel65.StylePriority.UseBorders = false;
        this.xrLabel65.StylePriority.UseFont = false;
        this.xrLabel65.StylePriority.UsePadding = false;
        this.xrLabel65.StylePriority.UseTextAlignment = false;
        this.xrLabel65.Text = "Signature";
        this.xrLabel65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel67
        // 
        this.xrLabel67.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel67.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel67.LocationFloat = new DevExpress.Utils.PointFloat(561.6929F, 831.1564F);
        this.xrLabel67.Name = "xrLabel67";
        this.xrLabel67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel67.SizeF = new System.Drawing.SizeF(21.6535F, 66.83331F);
        this.xrLabel67.StylePriority.UseBorders = false;
        this.xrLabel67.StylePriority.UseFont = false;
        this.xrLabel67.StylePriority.UseTextAlignment = false;
        this.xrLabel67.Text = ":";
        this.xrLabel67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel69
        // 
        this.xrLabel69.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel69.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel69.LocationFloat = new DevExpress.Utils.PointFloat(3.125079F, 831.1564F);
        this.xrLabel69.Name = "xrLabel69";
        this.xrLabel69.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel69.SizeF = new System.Drawing.SizeF(202.3047F, 33.41669F);
        this.xrLabel69.StylePriority.UseBorders = false;
        this.xrLabel69.StylePriority.UseFont = false;
        this.xrLabel69.StylePriority.UsePadding = false;
        this.xrLabel69.StylePriority.UseTextAlignment = false;
        this.xrLabel69.Text = "Name";
        this.xrLabel69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel68
        // 
        this.xrLabel68.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel68.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.AuthorizedPersonName")});
        this.xrLabel68.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel68.LocationFloat = new DevExpress.Utils.PointFloat(233.3334F, 831.1564F);
        this.xrLabel68.Name = "xrLabel68";
        this.xrLabel68.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel68.SizeF = new System.Drawing.SizeF(239.4344F, 33.41687F);
        this.xrLabel68.StylePriority.UseBorders = false;
        this.xrLabel68.StylePriority.UseFont = false;
        this.xrLabel68.StylePriority.UsePadding = false;
        this.xrLabel68.StylePriority.UseTextAlignment = false;
        this.xrLabel68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel64
        // 
        this.xrLabel64.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel64.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(205.4298F, 633.5312F);
        this.xrLabel64.Name = "xrLabel64";
        this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel64.SizeF = new System.Drawing.SizeF(27.90353F, 23F);
        this.xrLabel64.StylePriority.UseBorders = false;
        this.xrLabel64.StylePriority.UseFont = false;
        this.xrLabel64.StylePriority.UseTextAlignment = false;
        this.xrLabel64.Text = ":";
        this.xrLabel64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel62
        // 
        this.xrLabel62.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel62.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(233.3333F, 658.5311F);
        this.xrLabel62.Multiline = true;
        this.xrLabel62.Name = "xrLabel62";
        this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel62.SizeF = new System.Drawing.SizeF(549.5287F, 97.70844F);
        this.xrLabel62.StylePriority.UseBorders = false;
        this.xrLabel62.StylePriority.UseFont = false;
        this.xrLabel62.StylePriority.UsePadding = false;
        this.xrLabel62.StylePriority.UseTextAlignment = false;
        this.xrLabel62.Text = resources.GetString("xrLabel62.Text");
        this.xrLabel62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel59
        // 
        this.xrLabel59.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel59.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(3.125048F, 633.5312F);
        this.xrLabel59.Name = "xrLabel59";
        this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel59.SizeF = new System.Drawing.SizeF(202.3047F, 23F);
        this.xrLabel59.StylePriority.UseBorders = false;
        this.xrLabel59.StylePriority.UseFont = false;
        this.xrLabel59.StylePriority.UsePadding = false;
        this.xrLabel59.StylePriority.UseTextAlignment = false;
        this.xrLabel59.Text = "Payment Undertaking";
        this.xrLabel59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel51
        // 
        this.xrLabel51.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel51.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 610.5313F);
        this.xrLabel51.Name = "xrLabel51";
        this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel51.SizeF = new System.Drawing.SizeF(202.3047F, 23F);
        this.xrLabel51.StylePriority.UseBorders = false;
        this.xrLabel51.StylePriority.UseFont = false;
        this.xrLabel51.StylePriority.UsePadding = false;
        this.xrLabel51.StylePriority.UseTextAlignment = false;
        this.xrLabel51.Text = "Terms & Condition";
        this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel50
        // 
        this.xrLabel50.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel50.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.CreditCondition")});
        this.xrLabel50.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(233.3333F, 610.5313F);
        this.xrLabel50.Name = "xrLabel50";
        this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel50.SizeF = new System.Drawing.SizeF(548.2654F, 23F);
        this.xrLabel50.StylePriority.UseBorders = false;
        this.xrLabel50.StylePriority.UseFont = false;
        this.xrLabel50.StylePriority.UsePadding = false;
        this.xrLabel50.StylePriority.UseTextAlignment = false;
        this.xrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel48
        // 
        this.xrLabel48.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel48.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(205.4297F, 610.5313F);
        this.xrLabel48.Name = "xrLabel48";
        this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel48.SizeF = new System.Drawing.SizeF(27.90353F, 23F);
        this.xrLabel48.StylePriority.UseBorders = false;
        this.xrLabel48.StylePriority.UseFont = false;
        this.xrLabel48.StylePriority.UseTextAlignment = false;
        this.xrLabel48.Text = ":";
        this.xrLabel48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel58
        // 
        this.xrLabel58.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel58.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.CreditLimit")});
        this.xrLabel58.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(233.3333F, 564.5313F);
        this.xrLabel58.Name = "xrLabel58";
        this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel58.SizeF = new System.Drawing.SizeF(155.0594F, 23F);
        this.xrLabel58.StylePriority.UseBorders = false;
        this.xrLabel58.StylePriority.UseFont = false;
        this.xrLabel58.StylePriority.UsePadding = false;
        this.xrLabel58.StylePriority.UseTextAlignment = false;
        this.xrLabel58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        this.xrLabel58.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel58_BeforePrint);
        // 
        // xrLabel57
        // 
        this.xrLabel57.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel57.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(3.125032F, 564.5313F);
        this.xrLabel57.Name = "xrLabel57";
        this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel57.SizeF = new System.Drawing.SizeF(202.3047F, 23F);
        this.xrLabel57.StylePriority.UseBorders = false;
        this.xrLabel57.StylePriority.UseFont = false;
        this.xrLabel57.StylePriority.UsePadding = false;
        this.xrLabel57.StylePriority.UseTextAlignment = false;
        this.xrLabel57.Text = "Credit Limit";
        this.xrLabel57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel63
        // 
        this.xrLabel63.BackColor = System.Drawing.Color.Empty;
        this.xrLabel63.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel63.BorderWidth = 2F;
        this.xrLabel63.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(3.124984F, 516.9269F);
        this.xrLabel63.Name = "xrLabel63";
        this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel63.SizeF = new System.Drawing.SizeF(146.875F, 22.99994F);
        this.xrLabel63.StylePriority.UseBackColor = false;
        this.xrLabel63.StylePriority.UseBorders = false;
        this.xrLabel63.StylePriority.UseBorderWidth = false;
        this.xrLabel63.StylePriority.UseFont = false;
        this.xrLabel63.Text = "CREDIT DETAILS";
        // 
        // xrLabel61
        // 
        this.xrLabel61.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel61.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(205.4298F, 564.5313F);
        this.xrLabel61.Name = "xrLabel61";
        this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel61.SizeF = new System.Drawing.SizeF(27.90352F, 23F);
        this.xrLabel61.StylePriority.UseBorders = false;
        this.xrLabel61.StylePriority.UseFont = false;
        this.xrLabel61.StylePriority.UseTextAlignment = false;
        this.xrLabel61.Text = ":";
        this.xrLabel61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel56
        // 
        this.xrLabel56.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel56.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(388.3928F, 564.5313F);
        this.xrLabel56.Name = "xrLabel56";
        this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel56.SizeF = new System.Drawing.SizeF(391.9405F, 23F);
        this.xrLabel56.StylePriority.UseBorders = false;
        this.xrLabel56.StylePriority.UseFont = false;
        this.xrLabel56.StylePriority.UsePadding = false;
        this.xrLabel56.StylePriority.UseTextAlignment = false;
        this.xrLabel56.Text = "( if over 1000 KD-Attach financial statement )";
        this.xrLabel56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
        // 
        // xrLabel49
        // 
        this.xrLabel49.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel49.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(205.4298F, 587.5313F);
        this.xrLabel49.Name = "xrLabel49";
        this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel49.SizeF = new System.Drawing.SizeF(27.90353F, 23F);
        this.xrLabel49.StylePriority.UseBorders = false;
        this.xrLabel49.StylePriority.UseFont = false;
        this.xrLabel49.StylePriority.UseTextAlignment = false;
        this.xrLabel49.Text = ":";
        this.xrLabel49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel54
        // 
        this.xrLabel54.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel54.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Credit_Days")});
        this.xrLabel54.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(233.3333F, 587.5313F);
        this.xrLabel54.Name = "xrLabel54";
        this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel54.SizeF = new System.Drawing.SizeF(548.2654F, 23F);
        this.xrLabel54.StylePriority.UseBorders = false;
        this.xrLabel54.StylePriority.UseFont = false;
        this.xrLabel54.StylePriority.UsePadding = false;
        this.xrLabel54.StylePriority.UseTextAlignment = false;
        this.xrLabel54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel53
        // 
        this.xrLabel53.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel53.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(3.125032F, 587.5313F);
        this.xrLabel53.Name = "xrLabel53";
        this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel53.SizeF = new System.Drawing.SizeF(202.3047F, 23F);
        this.xrLabel53.StylePriority.UseBorders = false;
        this.xrLabel53.StylePriority.UseFont = false;
        this.xrLabel53.StylePriority.UsePadding = false;
        this.xrLabel53.StylePriority.UseTextAlignment = false;
        this.xrLabel53.Text = "Credit Days";
        this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel55
        // 
        this.xrLabel55.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel55.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(3.125048F, 441.1563F);
        this.xrLabel55.Name = "xrLabel55";
        this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel55.SizeF = new System.Drawing.SizeF(779.7368F, 23F);
        this.xrLabel55.StylePriority.UseBorders = false;
        this.xrLabel55.StylePriority.UseFont = false;
        this.xrLabel55.StylePriority.UsePadding = false;
        this.xrLabel55.StylePriority.UseTextAlignment = false;
        this.xrLabel55.Text = "1. ";
        this.xrLabel55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel60
        // 
        this.xrLabel60.BackColor = System.Drawing.Color.Empty;
        this.xrLabel60.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel60.BorderWidth = 2F;
        this.xrLabel60.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 393.5521F);
        this.xrLabel60.Name = "xrLabel60";
        this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel60.SizeF = new System.Drawing.SizeF(697F, 23F);
        this.xrLabel60.StylePriority.UseBackColor = false;
        this.xrLabel60.StylePriority.UseBorders = false;
        this.xrLabel60.StylePriority.UseBorderWidth = false;
        this.xrLabel60.StylePriority.UseFont = false;
        this.xrLabel60.Text = "NAME AND ADDRESS OR ASSOCIATED COMPANY IN /OUTSIDE OF YOUR COUNTRY";
        // 
        // xrLabel52
        // 
        this.xrLabel52.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel52.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(3.125048F, 464.1563F);
        this.xrLabel52.Name = "xrLabel52";
        this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel52.SizeF = new System.Drawing.SizeF(779.7368F, 23F);
        this.xrLabel52.StylePriority.UseBorders = false;
        this.xrLabel52.StylePriority.UseFont = false;
        this.xrLabel52.StylePriority.UsePadding = false;
        this.xrLabel52.StylePriority.UseTextAlignment = false;
        this.xrLabel52.Text = "2.";
        this.xrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel45
        // 
        this.xrLabel45.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel45.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(510.3049F, 345.6042F);
        this.xrLabel45.Name = "xrLabel45";
        this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel45.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel45.StylePriority.UseBorders = false;
        this.xrLabel45.StylePriority.UseFont = false;
        this.xrLabel45.StylePriority.UseTextAlignment = false;
        this.xrLabel45.Text = ":";
        this.xrLabel45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel46
        // 
        this.xrLabel46.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel46.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(205.4298F, 345.6042F);
        this.xrLabel46.Name = "xrLabel46";
        this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel46.SizeF = new System.Drawing.SizeF(27.90353F, 23F);
        this.xrLabel46.StylePriority.UseBorders = false;
        this.xrLabel46.StylePriority.UseFont = false;
        this.xrLabel46.StylePriority.UseTextAlignment = false;
        this.xrLabel46.Text = ":";
        this.xrLabel46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel47
        // 
        this.xrLabel47.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel47.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(414.2501F, 345.6042F);
        this.xrLabel47.Name = "xrLabel47";
        this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel47.SizeF = new System.Drawing.SizeF(96.05481F, 23F);
        this.xrLabel47.StylePriority.UseBorders = false;
        this.xrLabel47.StylePriority.UseFont = false;
        this.xrLabel47.StylePriority.UsePadding = false;
        this.xrLabel47.StylePriority.UseTextAlignment = false;
        this.xrLabel47.Text = "Email ";
        this.xrLabel47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel42
        // 
        this.xrLabel42.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Accounts_Email_Id")});
        this.xrLabel42.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(536.3466F, 345.6042F);
        this.xrLabel42.Name = "xrLabel42";
        this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel42.SizeF = new System.Drawing.SizeF(243.9868F, 23F);
        this.xrLabel42.StylePriority.UseBorders = false;
        this.xrLabel42.StylePriority.UseFont = false;
        this.xrLabel42.StylePriority.UsePadding = false;
        this.xrLabel42.StylePriority.UseTextAlignment = false;
        this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel43
        // 
        this.xrLabel43.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel43.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(3.125048F, 345.6042F);
        this.xrLabel43.Name = "xrLabel43";
        this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel43.SizeF = new System.Drawing.SizeF(202.3047F, 23F);
        this.xrLabel43.StylePriority.UseBorders = false;
        this.xrLabel43.StylePriority.UseFont = false;
        this.xrLabel43.StylePriority.UsePadding = false;
        this.xrLabel43.StylePriority.UseTextAlignment = false;
        this.xrLabel43.Text = "Contact Person(s)-Accounts";
        this.xrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel44
        // 
        this.xrLabel44.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel44.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Account_Contact_Person")});
        this.xrLabel44.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(233.3333F, 345.6042F);
        this.xrLabel44.Name = "xrLabel44";
        this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel44.SizeF = new System.Drawing.SizeF(180.9168F, 23F);
        this.xrLabel44.StylePriority.UseBorders = false;
        this.xrLabel44.StylePriority.UseFont = false;
        this.xrLabel44.StylePriority.UsePadding = false;
        this.xrLabel44.StylePriority.UseTextAlignment = false;
        this.xrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel39
        // 
        this.xrLabel39.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Sales_Email_Id")});
        this.xrLabel39.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(536.3466F, 322.6042F);
        this.xrLabel39.Name = "xrLabel39";
        this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel39.SizeF = new System.Drawing.SizeF(243.9868F, 23F);
        this.xrLabel39.StylePriority.UseBorders = false;
        this.xrLabel39.StylePriority.UseFont = false;
        this.xrLabel39.StylePriority.UsePadding = false;
        this.xrLabel39.StylePriority.UseTextAlignment = false;
        this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel40
        // 
        this.xrLabel40.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel40.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(3.125048F, 322.6042F);
        this.xrLabel40.Name = "xrLabel40";
        this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel40.SizeF = new System.Drawing.SizeF(202.3047F, 23F);
        this.xrLabel40.StylePriority.UseBorders = false;
        this.xrLabel40.StylePriority.UseFont = false;
        this.xrLabel40.StylePriority.UsePadding = false;
        this.xrLabel40.StylePriority.UseTextAlignment = false;
        this.xrLabel40.Text = "Contact Person(s)-Sales";
        this.xrLabel40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel41
        // 
        this.xrLabel41.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel41.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Sales_Contact_Person")});
        this.xrLabel41.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(233.3333F, 322.6042F);
        this.xrLabel41.Name = "xrLabel41";
        this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel41.SizeF = new System.Drawing.SizeF(180.9168F, 23F);
        this.xrLabel41.StylePriority.UseBorders = false;
        this.xrLabel41.StylePriority.UseFont = false;
        this.xrLabel41.StylePriority.UsePadding = false;
        this.xrLabel41.StylePriority.UseTextAlignment = false;
        this.xrLabel41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel36
        // 
        this.xrLabel36.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(510.3049F, 322.6042F);
        this.xrLabel36.Name = "xrLabel36";
        this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel36.SizeF = new System.Drawing.SizeF(26.04166F, 23F);
        this.xrLabel36.StylePriority.UseBorders = false;
        this.xrLabel36.StylePriority.UseFont = false;
        this.xrLabel36.StylePriority.UseTextAlignment = false;
        this.xrLabel36.Text = ":";
        this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel37
        // 
        this.xrLabel37.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(205.4298F, 322.6042F);
        this.xrLabel37.Name = "xrLabel37";
        this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel37.SizeF = new System.Drawing.SizeF(27.90352F, 23F);
        this.xrLabel37.StylePriority.UseBorders = false;
        this.xrLabel37.StylePriority.UseFont = false;
        this.xrLabel37.StylePriority.UseTextAlignment = false;
        this.xrLabel37.Text = ":";
        this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel38
        // 
        this.xrLabel38.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(414.2501F, 322.6042F);
        this.xrLabel38.Name = "xrLabel38";
        this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel38.SizeF = new System.Drawing.SizeF(96.05481F, 23F);
        this.xrLabel38.StylePriority.UseBorders = false;
        this.xrLabel38.StylePriority.UseFont = false;
        this.xrLabel38.StylePriority.UsePadding = false;
        this.xrLabel38.StylePriority.UseTextAlignment = false;
        this.xrLabel38.Text = "Email ";
        this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel35
        // 
        this.xrLabel35.BackColor = System.Drawing.Color.Empty;
        this.xrLabel35.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel35.BorderWidth = 2F;
        this.xrLabel35.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 275F);
        this.xrLabel35.Name = "xrLabel35";
        this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel35.SizeF = new System.Drawing.SizeF(181.4715F, 23F);
        this.xrLabel35.StylePriority.UseBackColor = false;
        this.xrLabel35.StylePriority.UseBorders = false;
        this.xrLabel35.StylePriority.UseBorderWidth = false;
        this.xrLabel35.StylePriority.UseFont = false;
        this.xrLabel35.Text = "OWNER (S)/SPONSOR";
        // 
        // xrLabel34
        // 
        this.xrLabel34.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(717.8334F, 208.2917F);
        this.xrLabel34.Name = "xrLabel34";
        this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel34.SizeF = new System.Drawing.SizeF(19.79169F, 23F);
        this.xrLabel34.StylePriority.UseBorders = false;
        this.xrLabel34.StylePriority.UseFont = false;
        this.xrLabel34.StylePriority.UseTextAlignment = false;
        this.xrLabel34.Text = ":";
        this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel33
        // 
        this.xrLabel33.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(737.6251F, 208.2917F);
        this.xrLabel33.Name = "xrLabel33";
        this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel33.SizeF = new System.Drawing.SizeF(42.70828F, 22.99998F);
        this.xrLabel33.StylePriority.UseBorders = false;
        this.xrLabel33.StylePriority.UseFont = false;
        this.xrLabel33.StylePriority.UsePadding = false;
        // 
        // xrLabel32
        // 
        this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(567.5001F, 208.2917F);
        this.xrLabel32.Name = "xrLabel32";
        this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel32.SizeF = new System.Drawing.SizeF(150.3333F, 23F);
        this.xrLabel32.StylePriority.UseBorders = false;
        this.xrLabel32.StylePriority.UseFont = false;
        this.xrLabel32.StylePriority.UsePadding = false;
        this.xrLabel32.StylePriority.UseTextAlignment = false;
        this.xrLabel32.Text = "Sole Proprietorship";
        this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel31
        // 
        this.xrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(379.1798F, 208.2917F);
        this.xrLabel31.Name = "xrLabel31";
        this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel31.SizeF = new System.Drawing.SizeF(96.875F, 23F);
        this.xrLabel31.StylePriority.UseBorders = false;
        this.xrLabel31.StylePriority.UseFont = false;
        this.xrLabel31.StylePriority.UsePadding = false;
        this.xrLabel31.StylePriority.UseTextAlignment = false;
        this.xrLabel31.Text = "Partnership";
        this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel30
        // 
        this.xrLabel30.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(507.3048F, 208.2917F);
        this.xrLabel30.Name = "xrLabel30";
        this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel30.SizeF = new System.Drawing.SizeF(42.70828F, 22.99998F);
        this.xrLabel30.StylePriority.UseBorders = false;
        this.xrLabel30.StylePriority.UseFont = false;
        this.xrLabel30.StylePriority.UsePadding = false;
        // 
        // xrLabel29
        // 
        this.xrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(476.0548F, 208.2917F);
        this.xrLabel29.Name = "xrLabel29";
        this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel29.SizeF = new System.Drawing.SizeF(31.25F, 23F);
        this.xrLabel29.StylePriority.UseBorders = false;
        this.xrLabel29.StylePriority.UseFont = false;
        this.xrLabel29.StylePriority.UseTextAlignment = false;
        this.xrLabel29.Text = ":";
        this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel26
        // 
        this.xrLabel26.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel26.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(290.6668F, 208.2917F);
        this.xrLabel26.Name = "xrLabel26";
        this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel26.SizeF = new System.Drawing.SizeF(31.25F, 23F);
        this.xrLabel26.StylePriority.UseBorders = false;
        this.xrLabel26.StylePriority.UseFont = false;
        this.xrLabel26.StylePriority.UseTextAlignment = false;
        this.xrLabel26.Text = ":";
        this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel25
        // 
        this.xrLabel25.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(321.9168F, 208.2917F);
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(42.70828F, 22.99998F);
        this.xrLabel25.StylePriority.UseBorders = false;
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.StylePriority.UsePadding = false;
        // 
        // xrLabel23
        // 
        this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(193.7918F, 208.2917F);
        this.xrLabel23.Name = "xrLabel23";
        this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel23.SizeF = new System.Drawing.SizeF(96.875F, 23F);
        this.xrLabel23.StylePriority.UseBorders = false;
        this.xrLabel23.StylePriority.UseFont = false;
        this.xrLabel23.StylePriority.UsePadding = false;
        this.xrLabel23.StylePriority.UseTextAlignment = false;
        this.xrLabel23.Text = "Limited Co";
        this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel27
        // 
        this.xrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel27.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 208.2917F);
        this.xrLabel27.Name = "xrLabel27";
        this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel27.SizeF = new System.Drawing.SizeF(96.875F, 23F);
        this.xrLabel27.StylePriority.UseBorders = false;
        this.xrLabel27.StylePriority.UseFont = false;
        this.xrLabel27.StylePriority.UsePadding = false;
        this.xrLabel27.StylePriority.UseTextAlignment = false;
        this.xrLabel27.Text = "Corporation";
        this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel28
        // 
        this.xrLabel28.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(131.25F, 208.2917F);
        this.xrLabel28.Name = "xrLabel28";
        this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel28.SizeF = new System.Drawing.SizeF(42.70828F, 22.99998F);
        this.xrLabel28.StylePriority.UseBorders = false;
        this.xrLabel28.StylePriority.UseFont = false;
        this.xrLabel28.StylePriority.UsePadding = false;
        // 
        // xrLabel24
        // 
        this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(100F, 208.2917F);
        this.xrLabel24.Name = "xrLabel24";
        this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel24.SizeF = new System.Drawing.SizeF(31.25F, 23F);
        this.xrLabel24.StylePriority.UseBorders = false;
        this.xrLabel24.StylePriority.UseFont = false;
        this.xrLabel24.StylePriority.UseTextAlignment = false;
        this.xrLabel24.Text = ":";
        this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel22
        // 
        this.xrLabel22.BackColor = System.Drawing.Color.Empty;
        this.xrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel22.BorderWidth = 2F;
        this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(1.86189F, 171.875F);
        this.xrLabel22.Name = "xrLabel22";
        this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel22.SizeF = new System.Drawing.SizeF(386.5309F, 23F);
        this.xrLabel22.StylePriority.UseBackColor = false;
        this.xrLabel22.StylePriority.UseBorders = false;
        this.xrLabel22.StylePriority.UseBorderWidth = false;
        this.xrLabel22.StylePriority.UseFont = false;
        this.xrLabel22.Text = "PLEASE TICK ANY ONE OF THE FOLLOWING";
        // 
        // xrLabel21
        // 
        this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(510.3048F, 118.9583F);
        this.xrLabel21.Name = "xrLabel21";
        this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel21.SizeF = new System.Drawing.SizeF(26.04172F, 23F);
        this.xrLabel21.StylePriority.UseBorders = false;
        this.xrLabel21.StylePriority.UseFont = false;
        this.xrLabel21.StylePriority.UseTextAlignment = false;
        this.xrLabel21.Text = ":";
        this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel20
        // 
        this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(510.3048F, 95.95833F);
        this.xrLabel20.Name = "xrLabel20";
        this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel20.SizeF = new System.Drawing.SizeF(26.04172F, 23F);
        this.xrLabel20.StylePriority.UseBorders = false;
        this.xrLabel20.StylePriority.UseFont = false;
        this.xrLabel20.StylePriority.UseTextAlignment = false;
        this.xrLabel20.Text = ":";
        this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel19
        // 
        this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(150F, 118.9583F);
        this.xrLabel19.Name = "xrLabel19";
        this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel19.SizeF = new System.Drawing.SizeF(23.95834F, 23F);
        this.xrLabel19.StylePriority.UseBorders = false;
        this.xrLabel19.StylePriority.UseFont = false;
        this.xrLabel19.StylePriority.UseTextAlignment = false;
        this.xrLabel19.Text = ":";
        this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel18
        // 
        this.xrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(150F, 95.95833F);
        this.xrLabel18.Name = "xrLabel18";
        this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel18.SizeF = new System.Drawing.SizeF(23.95834F, 23F);
        this.xrLabel18.StylePriority.UseBorders = false;
        this.xrLabel18.StylePriority.UseFont = false;
        this.xrLabel18.StylePriority.UseTextAlignment = false;
        this.xrLabel18.Text = ":";
        this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel17
        // 
        this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(150F, 72.95834F);
        this.xrLabel17.Name = "xrLabel17";
        this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel17.SizeF = new System.Drawing.SizeF(23.95834F, 23F);
        this.xrLabel17.StylePriority.UseBorders = false;
        this.xrLabel17.StylePriority.UseFont = false;
        this.xrLabel17.StylePriority.UseTextAlignment = false;
        this.xrLabel17.Text = ":";
        this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel16
        // 
        this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(150F, 49.95832F);
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(23.95834F, 23F);
        this.xrLabel16.StylePriority.UseBorders = false;
        this.xrLabel16.StylePriority.UseFont = false;
        this.xrLabel16.StylePriority.UseTextAlignment = false;
        this.xrLabel16.Text = ":";
        this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel14
        // 
        this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(414.2501F, 118.9583F);
        this.xrLabel14.Name = "xrLabel14";
        this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel14.SizeF = new System.Drawing.SizeF(96.05478F, 23.00002F);
        this.xrLabel14.StylePriority.UseBorders = false;
        this.xrLabel14.StylePriority.UseFont = false;
        this.xrLabel14.StylePriority.UsePadding = false;
        this.xrLabel14.StylePriority.UseTextAlignment = false;
        this.xrLabel14.Text = "Website";
        this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel15
        // 
        this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Company_WebSite")});
        this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(536.3466F, 118.9583F);
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(245.2521F, 22.99998F);
        this.xrLabel15.StylePriority.UseBorders = false;
        this.xrLabel15.StylePriority.UseFont = false;
        this.xrLabel15.StylePriority.UsePadding = false;
        this.xrLabel15.StylePriority.UseTextAlignment = false;
        this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel12
        // 
        this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Company_Fax_No")});
        this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(536.3466F, 95.95833F);
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(246.5153F, 22.99999F);
        this.xrLabel12.StylePriority.UseBorders = false;
        this.xrLabel12.StylePriority.UseFont = false;
        this.xrLabel12.StylePriority.UsePadding = false;
        this.xrLabel12.StylePriority.UseTextAlignment = false;
        this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel13
        // 
        this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(414.2501F, 95.95833F);
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 0, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(96.05481F, 22.99999F);
        this.xrLabel13.StylePriority.UseBorders = false;
        this.xrLabel13.StylePriority.UseFont = false;
        this.xrLabel13.StylePriority.UsePadding = false;
        this.xrLabel13.StylePriority.UseTextAlignment = false;
        this.xrLabel13.Text = "Fax No.     ";
        this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel10
        // 
        this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Company_Contact_No")});
        this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(173.9583F, 95.95833F);
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(240.2917F, 23F);
        this.xrLabel10.StylePriority.UseBorders = false;
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.StylePriority.UsePadding = false;
        this.xrLabel10.StylePriority.UseTextAlignment = false;
        this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel11
        // 
        this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 95.95833F);
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel11.StylePriority.UseBorders = false;
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.StylePriority.UsePadding = false;
        this.xrLabel11.StylePriority.UseTextAlignment = false;
        this.xrLabel11.Text = "Telephone    ";
        this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel7
        // 
        this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 118.9583F);
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel7.StylePriority.UseBorders = false;
        this.xrLabel7.StylePriority.UseFont = false;
        this.xrLabel7.StylePriority.UsePadding = false;
        this.xrLabel7.StylePriority.UseTextAlignment = false;
        this.xrLabel7.Text = "Email ";
        this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel9
        // 
        this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Company_Email_Id")});
        this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(173.9583F, 118.9583F);
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(240.2918F, 22.99998F);
        this.xrLabel9.StylePriority.UseBorders = false;
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.StylePriority.UsePadding = false;
        this.xrLabel9.StylePriority.UseTextAlignment = false;
        this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel6
        // 
        this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 72.95834F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel6.StylePriority.UseBorders = false;
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.StylePriority.UsePadding = false;
        this.xrLabel6.StylePriority.UseTextAlignment = false;
        this.xrLabel6.Text = "Company Address ";
        this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel5
        // 
        this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Company_Address")});
        this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(173.9583F, 72.95834F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(607.6404F, 23F);
        this.xrLabel5.StylePriority.UseBorders = false;
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UsePadding = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel4
        // 
        this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Customer_Company_Name")});
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Underline);
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(173.9583F, 49.95832F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(608.9036F, 23F);
        this.xrLabel4.StylePriority.UseBorders = false;
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UsePadding = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel3
        // 
        this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(3.125F, 49.95832F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(146.875F, 23F);
        this.xrLabel3.StylePriority.UseBorders = false;
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UsePadding = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "Company Name     ";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel1
        // 
        this.xrLabel1.BackColor = System.Drawing.Color.Empty;
        this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel1.BorderWidth = 2F;
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(1.861898F, 10.00001F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(172.0965F, 23F);
        this.xrLabel1.StylePriority.UseBackColor = false;
        this.xrLabel1.StylePriority.UseBorders = false;
        this.xrLabel1.StylePriority.UseBorderWidth = false;
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.Text = "COMPANY DETAILS";
        // 
        // xrLabel153
        // 
        this.xrLabel153.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel153.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold);
        this.xrLabel153.LocationFloat = new DevExpress.Utils.PointFloat(3.944874F, 79F);
        this.xrLabel153.Multiline = true;
        this.xrLabel153.Name = "xrLabel153";
        this.xrLabel153.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 0, 0, 0, 100F);
        this.xrLabel153.SizeF = new System.Drawing.SizeF(777.2083F, 23F);
        this.xrLabel153.StylePriority.UseBorders = false;
        this.xrLabel153.StylePriority.UseFont = false;
        this.xrLabel153.StylePriority.UsePadding = false;
        this.xrLabel153.StylePriority.UseTextAlignment = false;
        this.xrLabel153.Text = "We understand and agree to abide by the sales terms and conditions of Pegasus.";
        this.xrLabel153.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel154
        // 
        this.xrLabel154.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel154.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel154.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 23.91726F);
        this.xrLabel154.Name = "xrLabel154";
        this.xrLabel154.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel154.SizeF = new System.Drawing.SizeF(196.4584F, 23F);
        this.xrLabel154.StylePriority.UseBorders = false;
        this.xrLabel154.StylePriority.UseFont = false;
        this.xrLabel154.StylePriority.UsePadding = false;
        this.xrLabel154.StylePriority.UseTextAlignment = false;
        this.xrLabel154.Text = "Approved By  :";
        this.xrLabel154.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel155
        // 
        this.xrLabel155.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel155.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel155.LocationFloat = new DevExpress.Utils.PointFloat(310.8464F, 202.8755F);
        this.xrLabel155.Name = "xrLabel155";
        this.xrLabel155.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel155.SizeF = new System.Drawing.SizeF(196.4584F, 20F);
        this.xrLabel155.StylePriority.UseBorders = false;
        this.xrLabel155.StylePriority.UseFont = false;
        this.xrLabel155.StylePriority.UsePadding = false;
        this.xrLabel155.StylePriority.UseTextAlignment = false;
        this.xrLabel155.Text = "Name(s)";
        this.xrLabel155.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel156
        // 
        this.xrLabel156.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel156.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel156.LocationFloat = new DevExpress.Utils.PointFloat(588.0961F, 202.8754F);
        this.xrLabel156.Name = "xrLabel156";
        this.xrLabel156.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel156.SizeF = new System.Drawing.SizeF(193.0574F, 20F);
        this.xrLabel156.StylePriority.UseBorders = false;
        this.xrLabel156.StylePriority.UseFont = false;
        this.xrLabel156.StylePriority.UsePadding = false;
        this.xrLabel156.StylePriority.UseTextAlignment = false;
        this.xrLabel156.Text = "Signature & Date";
        this.xrLabel156.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // ReportFooter
        // 
        this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel3});
        this.ReportFooter.HeightF = 491.6667F;
        this.ReportFooter.KeepTogether = true;
        this.ReportFooter.Name = "ReportFooter";
        // 
        // xrPanel3
        // 
        this.xrPanel3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel4,
            this.xrLabel166,
            this.xrLabel165,
            this.xrLabel164,
            this.xrLabel163,
            this.xrLabel162,
            this.xrLabel161,
            this.xrLabel160,
            this.xrLabel159,
            this.xrLabel158,
            this.xrLabel157,
            this.xrLabel155,
            this.xrLabel156,
            this.xrLabel153,
            this.xrLabel167,
            this.xrLabel170,
            this.xrLabel171,
            this.xrLabel172,
            this.xrLabel173});
        this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrPanel3.Name = "xrPanel3";
        this.xrPanel3.SizeF = new System.Drawing.SizeF(785F, 491.6666F);
        this.xrPanel3.StylePriority.UseBorders = false;
        // 
        // xrPanel4
        // 
        this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel169,
            this.xrLabel168,
            this.xrLabel154});
        this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(8.00001F, 370.2081F);
        this.xrPanel4.Name = "xrPanel4";
        this.xrPanel4.SizeF = new System.Drawing.SizeF(770.3332F, 102.4585F);
        // 
        // xrLabel169
        // 
        this.xrLabel169.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel169.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel169.LocationFloat = new DevExpress.Utils.PointFloat(575.0961F, 72.45866F);
        this.xrLabel169.Name = "xrLabel169";
        this.xrLabel169.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel169.SizeF = new System.Drawing.SizeF(192.2373F, 19.99997F);
        this.xrLabel169.StylePriority.UseBorders = false;
        this.xrLabel169.StylePriority.UseFont = false;
        this.xrLabel169.StylePriority.UsePadding = false;
        this.xrLabel169.StylePriority.UseTextAlignment = false;
        this.xrLabel169.Text = "Managing Director";
        this.xrLabel169.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel168
        // 
        this.xrLabel168.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel168.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel168.LocationFloat = new DevExpress.Utils.PointFloat(260.8334F, 72.45852F);
        this.xrLabel168.Name = "xrLabel168";
        this.xrLabel168.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel168.SizeF = new System.Drawing.SizeF(196.4584F, 20F);
        this.xrLabel168.StylePriority.UseBorders = false;
        this.xrLabel168.StylePriority.UseFont = false;
        this.xrLabel168.StylePriority.UsePadding = false;
        this.xrLabel168.StylePriority.UseTextAlignment = false;
        this.xrLabel168.Text = "Finance Manager";
        this.xrLabel168.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel166
        // 
        this.xrLabel166.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel166.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel166.LocationFloat = new DevExpress.Utils.PointFloat(8.000009F, 346.3333F);
        this.xrLabel166.Name = "xrLabel166";
        this.xrLabel166.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel166.SizeF = new System.Drawing.SizeF(176.5965F, 23.87491F);
        this.xrLabel166.StylePriority.UseBorders = false;
        this.xrLabel166.StylePriority.UseFont = false;
        this.xrLabel166.Text = "Notes                                             ";
        // 
        // xrLabel165
        // 
        this.xrLabel165.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel165.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel165.LocationFloat = new DevExpress.Utils.PointFloat(7.554555F, 322.4584F);
        this.xrLabel165.Name = "xrLabel165";
        this.xrLabel165.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel165.SizeF = new System.Drawing.SizeF(177.042F, 23.87497F);
        this.xrLabel165.StylePriority.UseBorders = false;
        this.xrLabel165.StylePriority.UseFont = false;
        this.xrLabel165.Text = "Credit Limit K.D.                       ";
        // 
        // xrLabel164
        // 
        this.xrLabel164.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel164.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel164.LocationFloat = new DevExpress.Utils.PointFloat(7.554555F, 299.4584F);
        this.xrLabel164.Name = "xrLabel164";
        this.xrLabel164.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel164.SizeF = new System.Drawing.SizeF(177.042F, 22.99994F);
        this.xrLabel164.StylePriority.UseBorders = false;
        this.xrLabel164.StylePriority.UseFont = false;
        this.xrLabel164.Text = "Payment Terms Granted          ";
        // 
        // xrLabel163
        // 
        this.xrLabel163.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel163.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
        this.xrLabel163.LocationFloat = new DevExpress.Utils.PointFloat(265.114F, 346.3332F);
        this.xrLabel163.Name = "xrLabel163";
        this.xrLabel163.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel163.SizeF = new System.Drawing.SizeF(513.2192F, 23.87495F);
        this.xrLabel163.StylePriority.UseBorders = false;
        this.xrLabel163.StylePriority.UseFont = false;
        // 
        // xrLabel162
        // 
        this.xrLabel162.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel162.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
        this.xrLabel162.LocationFloat = new DevExpress.Utils.PointFloat(265.114F, 322.4583F);
        this.xrLabel162.Name = "xrLabel162";
        this.xrLabel162.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel162.SizeF = new System.Drawing.SizeF(513.6646F, 23.87497F);
        this.xrLabel162.StylePriority.UseBorders = false;
        this.xrLabel162.StylePriority.UseFont = false;
        // 
        // xrLabel161
        // 
        this.xrLabel161.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel161.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
        this.xrLabel161.LocationFloat = new DevExpress.Utils.PointFloat(265.114F, 299.4583F);
        this.xrLabel161.Name = "xrLabel161";
        this.xrLabel161.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel161.SizeF = new System.Drawing.SizeF(513.6646F, 23F);
        this.xrLabel161.StylePriority.UseBorders = false;
        this.xrLabel161.StylePriority.UseFont = false;
        // 
        // xrLabel160
        // 
        this.xrLabel160.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel160.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
        this.xrLabel160.LocationFloat = new DevExpress.Utils.PointFloat(265.114F, 276.4584F);
        this.xrLabel160.Name = "xrLabel160";
        this.xrLabel160.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel160.SizeF = new System.Drawing.SizeF(513.6646F, 23F);
        this.xrLabel160.StylePriority.UseBorders = false;
        this.xrLabel160.StylePriority.UseFont = false;
        // 
        // xrLabel159
        // 
        this.xrLabel159.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel159.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel159.LocationFloat = new DevExpress.Utils.PointFloat(7.554579F, 276.4584F);
        this.xrLabel159.Name = "xrLabel159";
        this.xrLabel159.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel159.SizeF = new System.Drawing.SizeF(177.0419F, 22.99994F);
        this.xrLabel159.StylePriority.UseBorders = false;
        this.xrLabel159.StylePriority.UseFont = false;
        this.xrLabel159.Text = "Date Application Received       ";
        // 
        // xrLabel158
        // 
        this.xrLabel158.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel158.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
        this.xrLabel158.LocationFloat = new DevExpress.Utils.PointFloat(412.2501F, 253.4583F);
        this.xrLabel158.Name = "xrLabel158";
        this.xrLabel158.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel158.SizeF = new System.Drawing.SizeF(366.0831F, 23F);
        this.xrLabel158.StylePriority.UseBorders = false;
        this.xrLabel158.StylePriority.UseFont = false;
        this.xrLabel158.Text = "For Pegasus use Only";
        // 
        // xrLabel157
        // 
        this.xrLabel157.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel157.LocationFloat = new DevExpress.Utils.PointFloat(7.554577F, 253.4583F);
        this.xrLabel157.Name = "xrLabel157";
        this.xrLabel157.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel157.SizeF = new System.Drawing.SizeF(404.6954F, 23F);
        this.xrLabel157.StylePriority.UseBorders = false;
        // 
        // xrLabel167
        // 
        this.xrLabel167.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel167.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel167.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 202.8755F);
        this.xrLabel167.Name = "xrLabel167";
        this.xrLabel167.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrLabel167.SizeF = new System.Drawing.SizeF(196.4584F, 20F);
        this.xrLabel167.StylePriority.UseBorders = false;
        this.xrLabel167.StylePriority.UseFont = false;
        this.xrLabel167.StylePriority.UsePadding = false;
        this.xrLabel167.StylePriority.UseTextAlignment = false;
        this.xrLabel167.Text = "Company Stamp";
        this.xrLabel167.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel170
        // 
        this.xrLabel170.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel170.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel170.LocationFloat = new DevExpress.Utils.PointFloat(184.5965F, 276.4585F);
        this.xrLabel170.Name = "xrLabel170";
        this.xrLabel170.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel170.SizeF = new System.Drawing.SizeF(80.51744F, 22.99997F);
        this.xrLabel170.StylePriority.UseBorders = false;
        this.xrLabel170.StylePriority.UseFont = false;
        this.xrLabel170.StylePriority.UseTextAlignment = false;
        this.xrLabel170.Text = ":";
        this.xrLabel170.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel171
        // 
        this.xrLabel171.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel171.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel171.LocationFloat = new DevExpress.Utils.PointFloat(184.5965F, 299.4584F);
        this.xrLabel171.Name = "xrLabel171";
        this.xrLabel171.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel171.SizeF = new System.Drawing.SizeF(80.5175F, 22.99997F);
        this.xrLabel171.StylePriority.UseBorders = false;
        this.xrLabel171.StylePriority.UseFont = false;
        this.xrLabel171.StylePriority.UseTextAlignment = false;
        this.xrLabel171.Text = ":";
        this.xrLabel171.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel172
        // 
        this.xrLabel172.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel172.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel172.LocationFloat = new DevExpress.Utils.PointFloat(184.5965F, 322.4584F);
        this.xrLabel172.Name = "xrLabel172";
        this.xrLabel172.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel172.SizeF = new System.Drawing.SizeF(80.51744F, 23.87515F);
        this.xrLabel172.StylePriority.UseBorders = false;
        this.xrLabel172.StylePriority.UseFont = false;
        this.xrLabel172.StylePriority.UseTextAlignment = false;
        this.xrLabel172.Text = ":";
        this.xrLabel172.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel173
        // 
        this.xrLabel173.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel173.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel173.LocationFloat = new DevExpress.Utils.PointFloat(184.5965F, 346.3336F);
        this.xrLabel173.Name = "xrLabel173";
        this.xrLabel173.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel173.SizeF = new System.Drawing.SizeF(80.51744F, 23.87457F);
        this.xrLabel173.StylePriority.UseBorders = false;
        this.xrLabel173.StylePriority.UseFont = false;
        this.xrLabel173.StylePriority.UseTextAlignment = false;
        this.xrLabel173.Text = ":";
        this.xrLabel173.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel174
        // 
        this.xrLabel174.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel174.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_CustomerMaster_CreditApplication_SelectRow.Credit_Days")});
        this.xrLabel174.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel174.LocationFloat = new DevExpress.Utils.PointFloat(473.3048F, 635.5313F);
        this.xrLabel174.Name = "xrLabel174";
        this.xrLabel174.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrLabel174.SizeF = new System.Drawing.SizeF(34.00003F, 22.99988F);
        this.xrLabel174.StylePriority.UseBorders = false;
        this.xrLabel174.StylePriority.UseFont = false;
        this.xrLabel174.StylePriority.UseTextAlignment = false;
        this.xrLabel174.Text = "15";
        this.xrLabel174.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
        // 
        // xrLabel175
        // 
        this.xrLabel175.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel175.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel175.LocationFloat = new DevExpress.Utils.PointFloat(233.3334F, 635.5313F);
        this.xrLabel175.Name = "xrLabel175";
        this.xrLabel175.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
        this.xrLabel175.SizeF = new System.Drawing.SizeF(239.4344F, 23F);
        this.xrLabel175.StylePriority.UseBorders = false;
        this.xrLabel175.StylePriority.UseFont = false;
        this.xrLabel175.StylePriority.UseTextAlignment = false;
        this.xrLabel175.Text = "we undertake to settle your invoice";
        this.xrLabel175.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel176
        // 
        this.xrLabel176.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel176.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel176.LocationFloat = new DevExpress.Utils.PointFloat(507.3049F, 635.5313F);
        this.xrLabel176.Name = "xrLabel176";
        this.xrLabel176.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel176.SizeF = new System.Drawing.SizeF(274.2937F, 23F);
        this.xrLabel176.StylePriority.UseBorders = false;
        this.xrLabel176.StylePriority.UseFont = false;
        this.xrLabel176.StylePriority.UseTextAlignment = false;
        this.xrLabel176.Text = "days from the invoice date .    In the";
        this.xrLabel176.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // CreditApplication
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupHeader1,
            this.ReportFooter});
        this.DataMember = "sp_Set_CustomerMaster_CreditApplication_SelectRow";
        this.DataSource = this.InventoryDataSet1;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.Margins = new System.Drawing.Printing.Margins(25, 14, 0, 0);
        this.PageHeight = 1169;
        this.PageWidth = 827;
        this.PaperKind = System.Drawing.Printing.PaperKind.A4;
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.salesDataSet1)).EndInit();
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

        //try
        //{
        //    xrTableCell5.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrTableCell5.Text);
        //}
        //catch
        //{

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

    private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
      
    }

    private void xrLabel4_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        
    }

    private void xrLabel5_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        

    }

    private void xrTableCell5_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        
    }

    private void xrTableCell10_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
       
    }

    private void xrTableCell13_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
       
    }

    private void xrTableCell18_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

    }

    private void xrTableCell20_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
      
    }

    private void xrTableCell22_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
       
    }

    private void xrTableCell28_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
       
    }

    private void xrTableCell24_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
       
    }

    private void xrTableCell26_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        
    }

    private void xrLabel58_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrLabel58.Text = objsys.GetCurencyConversionForInv(HttpContext.Current.Session["CurrencyId"].ToString(), xrLabel58.Text) + " " + objCurrency.GetCurrencyMasterById(HttpContext.Current.Session["CurrencyId"].ToString()).Rows[0]["Currency_Symbol"].ToString();

            
        }
        catch
        {

        }
    }
}
