using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

/// <summary>
/// Summary description for PobySupplier
/// </summary>
public class Sales_Return_Print : DevExpress.XtraReports.UI.XtraReport
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
    private SalesDataSet SalesDataSet1;
    private PageFooterBand PageFooter;
    private XRPageInfo xrPageInfo2;
    private ReportHeaderBand ReportHeader;
    private PurchaseDataSet purchaseDataSet1;
    private GroupHeaderBand GroupHeader1;
    private XRTable xrTable2;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell24;
    private XRTable xrTable6;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell61;
    private GroupFooterBand GroupFooter1;
    private XRLabel xrLabel10;
    private XRLabel xrLabel11;
    private XRLabel xrLabel15;
    private XRLabel xrLabel14;
    private XRPageInfo xrPageInfo1;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell66;
    private FormattingRule formattingRule1;
    private XRLabel xrLabel9;
    private XRPanel xrPanel1;
    private XRLabel xrLabel32;
    private XRLabel xrLabel31;
    private XRLabel xrLabel30;
    private XRLabel xrLabel29;
    private XRLabel xrLabel28;
    private XRLabel xrLabel17;
    private XRLabel xrLabel1;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel2;
    private XRLabel xrLabel3;
    private XRLabel xrLabel16;
    private XRLabel xrLabel18;
    private XRLabel xrLabel19;
    private XRLabel xrLabel21;
    private XRLabel xrLabel20;
    private XRLabel xrLabel22;
    private XRLabel xrLabel23;
    private XRLabel xrLabel24;
    private XRLabel xrLabel27;
    private XRLabel xrLabel33;
    private XRLabel xrLabel25;
    private XRLabel xrLabel26;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRLabel xrLabel51;
    private XRLabel xrLabel50;
    private XRLabel xrLabel53;
    private XRLabel xrLabel52;
    private XRLabel xrLabel55;
    private XRLabel xrLabel54;
    private XRLabel xrLabel7;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRPanel pnlDiscount;
    private XRLabel xrLabel57;
    private XRLabel xrLabel56;
    private XRPanel pnlTax;
    private XRPanel xrPanel2;
    private XRPanel xrPanel5;
    private ReportFooterBand ReportFooter;
    private XRLabel xrLabel46;
    private XRLabel xrLabel43;
    private XRLabel xrLabel44;
    private XRLabel xrLabel8;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell4;
    private XRLabel xrLabel48;
    private XRPictureBox xrPictureBox2;
    private XRLabel xrLabel35;
    private XRLabel xrLabel36;
    private XRLabel xrLabel13;
    private XRLabel xrLabel34;
    private XRLabel xrLabel12;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public Sales_Return_Print(string strConString)
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
    public void settitle(string TitleName)
    {
        xrLabel16.Text = TitleName;
    }
    public void setcompanyAddress(string Address)
    {
        xrLabel3.Text = Address;
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
       xrLabel1.Text = ArebicName;
      
        // xrPictureBox3.ImageUrl = "~/Images/Arabic.png";

    }
    public void setCompanyTelNo(string TelNo)
    {
        xrLabel28.Text = TelNo;
    }
    public void setCompanyFaxNo(string FaxNo)
    {
        xrLabel29.Text = FaxNo;
    }
    public void setCompanyWebsite(string WebSite)
    {
        xrLabel32.Text = WebSite;

    }
    //public void setBrandName(string BrandName)
    //{
    //    xrLabel6.Text = BrandName;
    //}
    public void setLocationName(string LocationName)
    {
        //xrLabel35.Text = LocationName;
    }
    //public void SetDateCriteria(string DateCriteria)
    //{
    //    xrLabel4.Text = DateCriteria;
    //}
    public void setUserName(string UserName)
    {
        xrLabel15.Text = UserName;
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
        string resourceFileName = "Sales_Return_Print.resx";
        DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary6 = new DevExpress.XtraReports.UI.XRSummary();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
        this.purchaseDataSet1 = new PurchaseDataSet();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.inventoryDataSet1 = new InventoryDataSet();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.SalesDataSet1 = new SalesDataSet();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
        this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel5 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
        this.pnlTax = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
        this.pnlDiscount = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.inventoryDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.SalesDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
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
        // 
        // xrTable6
        // 
        this.xrTable6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrTable6.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable6.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable6.Name = "xrTable6";
        this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8});
        this.xrTable6.SizeF = new System.Drawing.SizeF(835.0002F, 18.75F);
        this.xrTable6.StylePriority.UseBorderColor = false;
        this.xrTable6.StylePriority.UseBorders = false;
        this.xrTable6.StylePriority.UseFont = false;
        // 
        // xrTableRow8
        // 
        this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell4,
            this.xrTableCell56,
            this.xrTableCell21,
            this.xrTableCell57,
            this.xrTableCell59,
            this.xrTableCell61,
            this.xrTableCell66});
        this.xrTableRow8.Name = "xrTableRow8";
        this.xrTableRow8.Weight = 1D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.Serial_No")});
        this.xrTableCell2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.StylePriority.UseBorderColor = false;
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UseTextAlignment = false;
        this.xrTableCell2.Text = "xrTableCell2";
        this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell2.Weight = 0.33670552857284847D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.ProductRefNo")});
        this.xrTableCell4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorderColor = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.Text = "xrTableCell4";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell4.Weight = 0.61008602435952719D;
        // 
        // xrTableCell56
        // 
        this.xrTableCell56.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell56.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.ProductCode")});
        this.xrTableCell56.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell56.Name = "xrTableCell56";
        this.xrTableCell56.StylePriority.UseBorderColor = false;
        this.xrTableCell56.StylePriority.UseFont = false;
        this.xrTableCell56.StylePriority.UseTextAlignment = false;
        this.xrTableCell56.Text = "xrTableCell56";
        this.xrTableCell56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell56.Weight = 0.83680505092787161D;
        // 
        // xrTableCell21
        // 
        this.xrTableCell21.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.ProductName")});
        this.xrTableCell21.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell21.Name = "xrTableCell21";
        this.xrTableCell21.StylePriority.UseBorderColor = false;
        this.xrTableCell21.StylePriority.UseFont = false;
        this.xrTableCell21.StylePriority.UseTextAlignment = false;
        this.xrTableCell21.Text = "xrTableCell21";
        this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell21.Weight = 1.5006491769227275D;
        // 
        // xrTableCell57
        // 
        this.xrTableCell57.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell57.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell57.Name = "xrTableCell57";
        this.xrTableCell57.StylePriority.UseBorderColor = false;
        this.xrTableCell57.StylePriority.UseFont = false;
        this.xrTableCell57.StylePriority.UseTextAlignment = false;
        this.xrTableCell57.Text = "[Unit_Name]";
        this.xrTableCell57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell57.Weight = 0.345683620098077D;
        // 
        // xrTableCell59
        // 
        this.xrTableCell59.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell59.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.ReturnQty")});
        this.xrTableCell59.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell59.Name = "xrTableCell59";
        this.xrTableCell59.StylePriority.UseBorderColor = false;
        this.xrTableCell59.StylePriority.UseFont = false;
        this.xrTableCell59.StylePriority.UseTextAlignment = false;
        this.xrTableCell59.Text = "xrTableCell59";
        this.xrTableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell59.Weight = 0.5453060803420291D;
        this.xrTableCell59.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell59_BeforePrint);
        // 
        // xrTableCell61
        // 
        this.xrTableCell61.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell61.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.UnitPrice")});
        this.xrTableCell61.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell61.Name = "xrTableCell61";
        this.xrTableCell61.StylePriority.UseBorderColor = false;
        this.xrTableCell61.StylePriority.UseFont = false;
        this.xrTableCell61.StylePriority.UseTextAlignment = false;
        this.xrTableCell61.Text = "xrTableCell61";
        this.xrTableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell61.Weight = 0.612855631318985D;
        this.xrTableCell61.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell61_BeforePrint);
        // 
        // xrTableCell66
        // 
        this.xrTableCell66.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell66.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.LineTotal")});
        this.xrTableCell66.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell66.Name = "xrTableCell66";
        this.xrTableCell66.StylePriority.UseBorderColor = false;
        this.xrTableCell66.StylePriority.UseFont = false;
        this.xrTableCell66.StylePriority.UseTextAlignment = false;
        this.xrTableCell66.Text = "xrTableCell66";
        this.xrTableCell66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell66.Weight = 0.97881662663836111D;
        this.xrTableCell66.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell66_BeforePrint);
        // 
        // purchaseDataSet1
        // 
        this.purchaseDataSet1.DataSetName = "PurchaseDataSet";
        this.purchaseDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
        this.BottomMargin.HeightF = 69F;
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
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLabel14,
            this.xrLabel9,
            this.xrPageInfo2,
            this.xrLabel15});
        this.PageFooter.HeightF = 31.12494F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo1
        // 
        this.xrPageInfo1.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
        this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(497.4586F, 12.58335F);
        this.xrPageInfo1.Name = "xrPageInfo1";
        this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo1.SizeF = new System.Drawing.SizeF(337.5415F, 18.04161F);
        this.xrPageInfo1.StylePriority.UseTextAlignment = false;
        this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrLabel14
        // 
        this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(2.000014F, 12.99998F);
        this.xrLabel14.Name = "xrLabel14";
        this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel14.SizeF = new System.Drawing.SizeF(79.16667F, 17.62505F);
        this.xrLabel14.StylePriority.UseFont = false;
        this.xrLabel14.Text = "Created By:";
        // 
        // xrLabel9
        // 
        this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(835.0001F, 2.083333F);
        this.xrLabel9.StylePriority.UseBorders = false;
        this.xrLabel9.StylePriority.UseFont = false;
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Format = "Page{0}Of {1}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(303.9833F, 12.99998F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(118.5655F, 18.12496F);
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel15
        // 
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(81.16668F, 12.99998F);
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(177.0833F, 17.62505F);
        this.xrLabel15.Text = "xrLabel15";
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
        this.ReportHeader.HeightF = 151F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrPanel1
        // 
        this.xrPanel1.BorderColor = System.Drawing.Color.Black;
        this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrPanel1.BorderWidth = 2F;
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel32,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel29,
            this.xrLabel28,
            this.xrLabel17,
            this.xrLabel1,
            this.xrPictureBox1,
            this.xrLabel2,
            this.xrLabel3});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(835.1252F, 126F);
        this.xrPanel1.StylePriority.UseBorderColor = false;
        this.xrPanel1.StylePriority.UseBorders = false;
        this.xrPanel1.StylePriority.UseBorderWidth = false;
        // 
        // xrLabel32
        // 
        this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(70.49999F, 105.5417F);
        this.xrLabel32.Name = "xrLabel32";
        this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel32.SizeF = new System.Drawing.SizeF(649.2808F, 17.79169F);
        this.xrLabel32.StylePriority.UseBorders = false;
        this.xrLabel32.StylePriority.UseFont = false;
        // 
        // xrLabel31
        // 
        this.xrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(2.000014F, 105.5417F);
        this.xrLabel31.Name = "xrLabel31";
        this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel31.SizeF = new System.Drawing.SizeF(68.49998F, 17.79169F);
        this.xrLabel31.StylePriority.UseBorders = false;
        this.xrLabel31.StylePriority.UseFont = false;
        this.xrLabel31.Text = "Web Site   :";
        // 
        // xrLabel30
        // 
        this.xrLabel30.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(1.885954F, 87.75001F);
        this.xrLabel30.Name = "xrLabel30";
        this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel30.SizeF = new System.Drawing.SizeF(68.61404F, 17.79169F);
        this.xrLabel30.StylePriority.UseBorders = false;
        this.xrLabel30.StylePriority.UseFont = false;
        this.xrLabel30.Text = "Fax No.    : ";
        // 
        // xrLabel29
        // 
        this.xrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(70.49999F, 87.75001F);
        this.xrLabel29.Name = "xrLabel29";
        this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel29.SizeF = new System.Drawing.SizeF(649.2808F, 17.79169F);
        this.xrLabel29.StylePriority.UseBorders = false;
        this.xrLabel29.StylePriority.UseFont = false;
        // 
        // xrLabel28
        // 
        this.xrLabel28.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(70.49999F, 69.95832F);
        this.xrLabel28.Name = "xrLabel28";
        this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel28.SizeF = new System.Drawing.SizeF(649.2808F, 17.79167F);
        this.xrLabel28.StylePriority.UseBorders = false;
        this.xrLabel28.StylePriority.UseFont = false;
        // 
        // xrLabel17
        // 
        this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(1.885954F, 69.95832F);
        this.xrLabel17.Name = "xrLabel17";
        this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel17.SizeF = new System.Drawing.SizeF(68.61404F, 17.79167F);
        this.xrLabel17.StylePriority.UseBorders = false;
        this.xrLabel17.StylePriority.UseFont = false;
        this.xrLabel17.Text = "Tel No      :";
        // 
        // xrLabel1
        // 
        this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(1.885954F, 23.95833F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(717.895F, 23F);
        this.xrLabel1.StylePriority.UseBorders = false;
        this.xrLabel1.StylePriority.UseFont = false;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(731.0001F, 1.999998F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(94.99982F, 67.95834F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(2.000014F, 1.999998F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(717.7809F, 21.95834F);
        this.xrLabel2.StylePriority.UseBorders = false;
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "xrLabel1";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel3
        // 
        this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(2.000014F, 47.91835F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(717.7808F, 22.04F);
        this.xrLabel3.StylePriority.UseBorders = false;
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // SalesDataSet1
        // 
        this.SalesDataSet1.DataSetName = "SalesDataSet";
        this.SalesDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel35,
            this.xrLabel36,
            this.xrLabel13,
            this.xrLabel34,
            this.xrLabel27,
            this.xrLabel33,
            this.xrLabel25,
            this.xrLabel26,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel16,
            this.xrTable2});
        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("TransID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader1.HeightF = 205.375F;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // xrLabel35
        // 
        this.xrLabel35.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(601.4841F, 117.2916F);
        this.xrLabel35.Name = "xrLabel35";
        this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel35.SizeF = new System.Drawing.SizeF(105.4583F, 17.79166F);
        this.xrLabel35.StylePriority.UseFont = false;
        this.xrLabel35.StylePriority.UseTextAlignment = false;
        this.xrLabel35.Text = "Date                    :";
        this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel36
        // 
        this.xrLabel36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.Invoice_Date")});
        this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(706.9424F, 117.2916F);
        this.xrLabel36.Name = "xrLabel36";
        this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel36.SizeF = new System.Drawing.SizeF(134.0576F, 17.79166F);
        this.xrLabel36.StylePriority.UseFont = false;
        this.xrLabel36.StylePriority.UseTextAlignment = false;
        this.xrLabel36.Text = "xrLabel19";
        this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrLabel36.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel36_BeforePrint_1);
        // 
        // xrLabel13
        // 
        this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(601.4841F, 96.12497F);
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(104.4583F, 17.79F);
        this.xrLabel13.StylePriority.UseFont = false;
        this.xrLabel13.Text = "Sales Invoice#   :";
        // 
        // xrLabel34
        // 
        this.xrLabel34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.Invoice_No")});
        this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(706.9424F, 96.12497F);
        this.xrLabel34.Name = "xrLabel34";
        this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel34.SizeF = new System.Drawing.SizeF(134.0576F, 17.79F);
        this.xrLabel34.StylePriority.UseFont = false;
        this.xrLabel34.Text = "xrLabel19";
        // 
        // xrLabel27
        // 
        this.xrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel27.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.CustomerAddress")});
        this.xrLabel27.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(66.37281F, 133.7083F);
        this.xrLabel27.Name = "xrLabel27";
        this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel27.SizeF = new System.Drawing.SizeF(212.5775F, 17.79169F);
        this.xrLabel27.StylePriority.UseBorders = false;
        this.xrLabel27.StylePriority.UseFont = false;
        this.xrLabel27.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel27_BeforePrint);
        // 
        // xrLabel33
        // 
        this.xrLabel33.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(0.5920728F, 133.7083F);
        this.xrLabel33.Name = "xrLabel33";
        this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel33.SizeF = new System.Drawing.SizeF(65.78071F, 17.79169F);
        this.xrLabel33.StylePriority.UseBorders = false;
        this.xrLabel33.StylePriority.UseFont = false;
        this.xrLabel33.Text = "Tel No      :";
        // 
        // xrLabel25
        // 
        this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(0.5920728F, 151.7083F);
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(65.78072F, 17.79161F);
        this.xrLabel25.StylePriority.UseBorders = false;
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.Text = "Fax No.    : ";
        // 
        // xrLabel26
        // 
        this.xrLabel26.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.CustomerAddress")});
        this.xrLabel26.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(66.37281F, 151.7083F);
        this.xrLabel26.Name = "xrLabel26";
        this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel26.SizeF = new System.Drawing.SizeF(212.5776F, 17.79161F);
        this.xrLabel26.StylePriority.UseBorders = false;
        this.xrLabel26.StylePriority.UseFont = false;
        this.xrLabel26.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel26_BeforePrint);
        // 
        // xrLabel24
        // 
        this.xrLabel24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.CustomerAddress")});
        this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(0.5920728F, 96.12497F);
        this.xrLabel24.Name = "xrLabel24";
        this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel24.SizeF = new System.Drawing.SizeF(278.3583F, 37.58331F);
        this.xrLabel24.Text = "xrLabel24";
        this.xrLabel24.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel24_BeforePrint);
        // 
        // xrLabel23
        // 
        this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.CustomerName")});
        this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(0.5920728F, 65.91669F);
        this.xrLabel23.Name = "xrLabel23";
        this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel23.SizeF = new System.Drawing.SizeF(278.3582F, 17.79164F);
        this.xrLabel23.StylePriority.UseFont = false;
        this.xrLabel23.Text = "xrLabel23";
        // 
        // xrLabel22
        // 
        this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(0.5920728F, 35.74998F);
        this.xrLabel22.Name = "xrLabel22";
        this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel22.SizeF = new System.Drawing.SizeF(164.5834F, 17.79165F);
        this.xrLabel22.StylePriority.UseFont = false;
        this.xrLabel22.Text = "Return To";
        // 
        // xrLabel21
        // 
        this.xrLabel21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.Return_Date")});
        this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(707.5345F, 63.91669F);
        this.xrLabel21.Name = "xrLabel21";
        this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel21.SizeF = new System.Drawing.SizeF(134.0576F, 17.79166F);
        this.xrLabel21.StylePriority.UseFont = false;
        this.xrLabel21.StylePriority.UseTextAlignment = false;
        this.xrLabel21.Text = "xrLabel19";
        this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrLabel21.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel21_BeforePrint);
        // 
        // xrLabel20
        // 
        this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(602.0762F, 63.91669F);
        this.xrLabel20.Name = "xrLabel20";
        this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel20.SizeF = new System.Drawing.SizeF(105.4583F, 17.79166F);
        this.xrLabel20.StylePriority.UseFont = false;
        this.xrLabel20.StylePriority.UseTextAlignment = false;
        this.xrLabel20.Text = "Date                    :";
        this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel19
        // 
        this.xrLabel19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.Return_No")});
        this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(707.5345F, 42.75001F);
        this.xrLabel19.Name = "xrLabel19";
        this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel19.SizeF = new System.Drawing.SizeF(134.0576F, 17.79F);
        this.xrLabel19.StylePriority.UseFont = false;
        this.xrLabel19.Text = "xrLabel19";
        // 
        // xrLabel18
        // 
        this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(602.0761F, 42.75001F);
        this.xrLabel18.Name = "xrLabel18";
        this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel18.SizeF = new System.Drawing.SizeF(104.4583F, 17.79F);
        this.xrLabel18.StylePriority.UseFont = false;
        this.xrLabel18.Text = "Sales  Return#   :";
        // 
        // xrLabel16
        // 
        this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(602.0762F, 5F);
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(232.9239F, 17.79165F);
        this.xrLabel16.StylePriority.UseBorders = false;
        this.xrLabel16.StylePriority.UseFont = false;
        this.xrLabel16.StylePriority.UseTextAlignment = false;
        this.xrLabel16.Text = "xrLabel3";
        this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrTable2
        // 
        this.xrTable2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable2.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 183.5F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
        this.xrTable2.SizeF = new System.Drawing.SizeF(835.0001F, 21.875F);
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
            this.xrTableCell19,
            this.xrTableCell18,
            this.xrTableCell22,
            this.xrTableCell24,
            this.xrTableCell26});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBorderColor = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "S. No.";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell1.Weight = 0.36716352915552386D;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.StylePriority.UseBorderColor = false;
        this.xrTableCell3.StylePriority.UseFont = false;
        this.xrTableCell3.Text = "So No#";
        this.xrTableCell3.Weight = 0.66527398518328762D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell14.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseBorderColor = false;
        this.xrTableCell14.StylePriority.UseBorders = false;
        this.xrTableCell14.StylePriority.UseFont = false;
        this.xrTableCell14.Text = "Product Id";
        this.xrTableCell14.Weight = 0.91250152340830781D;
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell19.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.StylePriority.UseBorderColor = false;
        this.xrTableCell19.StylePriority.UseFont = false;
        this.xrTableCell19.Text = "Description";
        this.xrTableCell19.Weight = 1.6363960568694678D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell18.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell18.Multiline = true;
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseBorderColor = false;
        this.xrTableCell18.StylePriority.UseBorders = false;
        this.xrTableCell18.StylePriority.UseFont = false;
        this.xrTableCell18.StylePriority.UseTextAlignment = false;
        this.xrTableCell18.Text = "Unit \r\n";
        this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.xrTableCell18.Weight = 0.37695458505596358D;
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell22.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell22.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.StylePriority.UseBorderColor = false;
        this.xrTableCell22.StylePriority.UseBorders = false;
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.StylePriority.UseTextAlignment = false;
        this.xrTableCell22.Text = "Quantity";
        this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell22.Weight = 0.59463367266337042D;
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell24.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell24.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.StylePriority.UseBorderColor = false;
        this.xrTableCell24.StylePriority.UseBorders = false;
        this.xrTableCell24.StylePriority.UseFont = false;
        this.xrTableCell24.StylePriority.UseTextAlignment = false;
        this.xrTableCell24.Text = "Unit Price";
        this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell24.Weight = 0.66829343213617309D;
        this.xrTableCell24.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell24_BeforePrint);
        // 
        // xrTableCell26
        // 
        this.xrTableCell26.BorderColor = System.Drawing.Color.Black;
        this.xrTableCell26.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell26.Name = "xrTableCell26";
        this.xrTableCell26.StylePriority.UseBorderColor = false;
        this.xrTableCell26.StylePriority.UseFont = false;
        this.xrTableCell26.StylePriority.UseTextAlignment = false;
        this.xrTableCell26.Text = "Line Total";
        this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell26.Weight = 1.0673586834656166D;
        this.xrTableCell26.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell26_BeforePrint);
        // 
        // GroupFooter1
        // 
        this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel2,
            this.pnlTax,
            this.pnlDiscount,
            this.xrLabel11,
            this.xrLabel10});
        this.GroupFooter1.HeightF = 299.4584F;
        this.GroupFooter1.KeepTogether = true;
        this.GroupFooter1.Name = "GroupFooter1";
        this.GroupFooter1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupFooter1_BeforePrint);
        // 
        // xrPanel2
        // 
        this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel12,
            this.xrPictureBox2,
            this.xrLabel48,
            this.xrLabel55,
            this.xrLabel54,
            this.xrPanel5,
            this.xrLabel8,
            this.xrLabel46,
            this.xrLabel43,
            this.xrLabel44});
        this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 121.0834F);
        this.xrPanel2.Name = "xrPanel2";
        this.xrPanel2.SizeF = new System.Drawing.SizeF(839F, 178.375F);
        // 
        // xrLabel12
        // 
        this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.Remark")});
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(103.645F, 38.33344F);
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(731.3552F, 23F);
        this.xrLabel12.Text = "xrLabel12";
        // 
        // xrPictureBox2
        // 
        this.xrPictureBox2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(6.265863F, 116.2083F);
        this.xrPictureBox2.Name = "xrPictureBox2";
        this.xrPictureBox2.SizeF = new System.Drawing.SizeF(290.625F, 55.29175F);
        this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox2.StylePriority.UseBorders = false;
        // 
        // xrLabel48
        // 
        this.xrLabel48.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(6.265863F, 69.3334F);
        this.xrLabel48.Name = "xrLabel48";
        this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel48.SizeF = new System.Drawing.SizeF(290.625F, 23F);
        this.xrLabel48.StylePriority.UseFont = false;
        this.xrLabel48.Text = "Signature & Approvals";
        // 
        // xrLabel55
        // 
        this.xrLabel55.BorderColor = System.Drawing.Color.Black;
        this.xrLabel55.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel55.BorderWidth = 2F;
        this.xrLabel55.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.GrandTotal")});
        this.xrLabel55.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(695.0346F, 2.000019F);
        this.xrLabel55.Name = "xrLabel55";
        this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel55.SizeF = new System.Drawing.SizeF(141.1323F, 19.70831F);
        this.xrLabel55.StylePriority.UseBorderColor = false;
        this.xrLabel55.StylePriority.UseBorders = false;
        this.xrLabel55.StylePriority.UseBorderWidth = false;
        this.xrLabel55.StylePriority.UseFont = false;
        this.xrLabel55.StylePriority.UseTextAlignment = false;
        xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        this.xrLabel55.Summary = xrSummary1;
        this.xrLabel55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel55.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel55_SummaryGetResult);
        this.xrLabel55.SummaryReset += new System.EventHandler(this.xrLabel55_SummaryReset);
        this.xrLabel55.SummaryRowChanged += new System.EventHandler(this.xrLabel55_SummaryRowChanged);
        this.xrLabel55.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel55_BeforePrint);
        this.xrLabel55.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrLabel55_PrintOnPage);
        // 
        // xrLabel54
        // 
        this.xrLabel54.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel54.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel54.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(567.5417F, 2.000019F);
        this.xrLabel54.Name = "xrLabel54";
        this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel54.SizeF = new System.Drawing.SizeF(127.4929F, 19.70831F);
        this.xrLabel54.StylePriority.UseBorderColor = false;
        this.xrLabel54.StylePriority.UseBorders = false;
        this.xrLabel54.StylePriority.UseFont = false;
        this.xrLabel54.StylePriority.UseTextAlignment = false;
        this.xrLabel54.Text = "Net Amount";
        this.xrLabel54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrLabel54.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel54_BeforePrint);
        // 
        // xrPanel5
        // 
        this.xrPanel5.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrPanel5.BorderWidth = 2F;
        this.xrPanel5.LocationFloat = new DevExpress.Utils.PointFloat(4.265944F, 25.33341F);
        this.xrPanel5.Name = "xrPanel5";
        this.xrPanel5.SizeF = new System.Drawing.SizeF(831.9008F, 2.083344F);
        this.xrPanel5.StylePriority.UseBorders = false;
        this.xrPanel5.StylePriority.UseBorderWidth = false;
        // 
        // xrLabel8
        // 
        this.xrLabel8.Font = new System.Drawing.Font("Verdana", 8.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(6.265863F, 38.33341F);
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(97.37911F, 22.99995F);
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.StylePriority.UseTextAlignment = false;
        this.xrLabel8.Text = "Reference   :";
        this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel46
        // 
        this.xrLabel46.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel46.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(296.8909F, 116.2083F);
        this.xrLabel46.Name = "xrLabel46";
        this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel46.SizeF = new System.Drawing.SizeF(539.134F, 55.29176F);
        this.xrLabel46.StylePriority.UseBorders = false;
        this.xrLabel46.StylePriority.UseFont = false;
        this.xrLabel46.StylePriority.UseTextAlignment = false;
        this.xrLabel46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel43
        // 
        this.xrLabel43.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel43.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(6.265863F, 93.2082F);
        this.xrLabel43.Name = "xrLabel43";
        this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel43.SizeF = new System.Drawing.SizeF(290.625F, 23.00002F);
        this.xrLabel43.StylePriority.UseBorders = false;
        this.xrLabel43.StylePriority.UseFont = false;
        this.xrLabel43.StylePriority.UseTextAlignment = false;
        this.xrLabel43.Text = "Sales Person";
        this.xrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel44
        // 
        this.xrLabel44.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel44.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(296.8909F, 93.2082F);
        this.xrLabel44.Name = "xrLabel44";
        this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel44.SizeF = new System.Drawing.SizeF(539.134F, 23.00008F);
        this.xrLabel44.StylePriority.UseBorders = false;
        this.xrLabel44.StylePriority.UseFont = false;
        this.xrLabel44.StylePriority.UseTextAlignment = false;
        this.xrLabel44.Text = "Manager";
        this.xrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // pnlTax
        // 
        this.pnlTax.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel51,
            this.xrLabel50,
            this.xrLabel52,
            this.xrLabel53});
        this.pnlTax.LocationFloat = new DevExpress.Utils.PointFloat(535.7342F, 80.37504F);
        this.pnlTax.Name = "pnlTax";
        this.pnlTax.SizeF = new System.Drawing.SizeF(305.2657F, 40F);
        // 
        // xrLabel51
        // 
        this.xrLabel51.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel51.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel51.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.NetTaxP")});
        this.xrLabel51.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(158.7083F, 0F);
        this.xrLabel51.Name = "xrLabel51";
        this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel51.SizeF = new System.Drawing.SizeF(141.7244F, 19.70832F);
        this.xrLabel51.StylePriority.UseBorderColor = false;
        this.xrLabel51.StylePriority.UseBorders = false;
        this.xrLabel51.StylePriority.UseFont = false;
        this.xrLabel51.StylePriority.UseTextAlignment = false;
        xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        this.xrLabel51.Summary = xrSummary2;
        this.xrLabel51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel51.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel51_SummaryGetResult);
        this.xrLabel51.SummaryReset += new System.EventHandler(this.xrLabel51_SummaryReset);
        this.xrLabel51.SummaryRowChanged += new System.EventHandler(this.xrLabel51_SummaryRowChanged_1);
        this.xrLabel51.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrLabel51_PrintOnPage);
        // 
        // xrLabel50
        // 
        this.xrLabel50.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel50.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel50.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(32.97418F, 6.357829E-05F);
        this.xrLabel50.Name = "xrLabel50";
        this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel50.SizeF = new System.Drawing.SizeF(125.5674F, 19.70832F);
        this.xrLabel50.StylePriority.UseBorderColor = false;
        this.xrLabel50.StylePriority.UseBorders = false;
        this.xrLabel50.StylePriority.UseFont = false;
        this.xrLabel50.StylePriority.UseTextAlignment = false;
        this.xrLabel50.Text = "Tax %(+)";
        this.xrLabel50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel52
        // 
        this.xrLabel52.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel52.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel52.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(31.974F, 19.70834F);
        this.xrLabel52.Name = "xrLabel52";
        this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel52.SizeF = new System.Drawing.SizeF(127.3264F, 19.70831F);
        this.xrLabel52.StylePriority.UseBorderColor = false;
        this.xrLabel52.StylePriority.UseBorders = false;
        this.xrLabel52.StylePriority.UseFont = false;
        this.xrLabel52.StylePriority.UseTextAlignment = false;
        this.xrLabel52.Text = "Tax";
        this.xrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel53
        // 
        this.xrLabel53.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel53.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel53.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.NetTaxV")});
        this.xrLabel53.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(159.3004F, 19.70834F);
        this.xrLabel53.Name = "xrLabel53";
        this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel53.SizeF = new System.Drawing.SizeF(141.1321F, 19.70831F);
        this.xrLabel53.StylePriority.UseBorderColor = false;
        this.xrLabel53.StylePriority.UseBorders = false;
        this.xrLabel53.StylePriority.UseFont = false;
        this.xrLabel53.StylePriority.UseTextAlignment = false;
        xrSummary3.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        this.xrLabel53.Summary = xrSummary3;
        this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel53.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel53_SummaryGetResult);
        this.xrLabel53.SummaryReset += new System.EventHandler(this.xrLabel53_SummaryReset);
        this.xrLabel53.SummaryRowChanged += new System.EventHandler(this.xrLabel53_SummaryRowChanged);
        this.xrLabel53.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel53_BeforePrint);
        this.xrLabel53.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrLabel53_PrintOnPage);
        // 
        // pnlDiscount
        // 
        this.pnlDiscount.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel5,
            this.xrLabel6,
            this.xrLabel7,
            this.xrLabel56,
            this.xrLabel57});
        this.pnlDiscount.LocationFloat = new DevExpress.Utils.PointFloat(535.7342F, 19.70838F);
        this.pnlDiscount.Name = "pnlDiscount";
        this.pnlDiscount.SizeF = new System.Drawing.SizeF(305.2658F, 60.66666F);
        // 
        // xrLabel4
        // 
        this.xrLabel4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(32.97418F, 0.9999924F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(125.5673F, 19.70831F);
        this.xrLabel4.StylePriority.UseBorderColor = false;
        this.xrLabel4.StylePriority.UseBorders = false;
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "Discount %(-)";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel5
        // 
        this.xrLabel5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.NetDiscountP")});
        this.xrLabel5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(158.7081F, 1.000007F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(141.7244F, 19.70832F);
        this.xrLabel5.StylePriority.UseBorderColor = false;
        this.xrLabel5.StylePriority.UseBorders = false;
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        xrSummary4.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        this.xrLabel5.Summary = xrSummary4;
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel5.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel5_SummaryGetResult_1);
        this.xrLabel5.SummaryReset += new System.EventHandler(this.xrLabel5_SummaryReset_1);
        this.xrLabel5.SummaryRowChanged += new System.EventHandler(this.xrLabel5_SummaryRowChanged_1);
        this.xrLabel5.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrLabel5_PrintOnPage);
        // 
        // xrLabel6
        // 
        this.xrLabel6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(31.97412F, 20.70834F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(126.7341F, 19.37504F);
        this.xrLabel6.StylePriority.UseBorderColor = false;
        this.xrLabel6.StylePriority.UseBorders = false;
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.StylePriority.UseTextAlignment = false;
        this.xrLabel6.Text = "Discount";
        this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel7
        // 
        this.xrLabel7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.NetDiscountV")});
        this.xrLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(158.7082F, 20.70832F);
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(141.7243F, 19.37504F);
        this.xrLabel7.StylePriority.UseBorderColor = false;
        this.xrLabel7.StylePriority.UseBorders = false;
        this.xrLabel7.StylePriority.UseFont = false;
        this.xrLabel7.StylePriority.UseTextAlignment = false;
        xrSummary5.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        this.xrLabel7.Summary = xrSummary5;
        this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel7.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel7_SummaryGetResult);
        this.xrLabel7.SummaryReset += new System.EventHandler(this.xrLabel7_SummaryReset);
        this.xrLabel7.SummaryRowChanged += new System.EventHandler(this.xrLabel7_SummaryRowChanged);
        this.xrLabel7.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrLabel7_PrintOnPage);
        // 
        // xrLabel56
        // 
        this.xrLabel56.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel56.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel56.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(31.97406F, 40.08337F);
        this.xrLabel56.Name = "xrLabel56";
        this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel56.SizeF = new System.Drawing.SizeF(126.7342F, 19.37504F);
        this.xrLabel56.StylePriority.UseBorderColor = false;
        this.xrLabel56.StylePriority.UseBorders = false;
        this.xrLabel56.StylePriority.UseFont = false;
        this.xrLabel56.StylePriority.UseTextAlignment = false;
        this.xrLabel56.Text = "After Discount";
        this.xrLabel56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrLabel57
        // 
        this.xrLabel57.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel57.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel57.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.PriceAfterDiscount")});
        this.xrLabel57.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(158.7082F, 40.08336F);
        this.xrLabel57.Name = "xrLabel57";
        this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel57.SizeF = new System.Drawing.SizeF(141.7243F, 19.37504F);
        this.xrLabel57.StylePriority.UseBorderColor = false;
        this.xrLabel57.StylePriority.UseBorders = false;
        this.xrLabel57.StylePriority.UseFont = false;
        this.xrLabel57.StylePriority.UseTextAlignment = false;
        xrSummary6.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
        this.xrLabel57.Summary = xrSummary6;
        this.xrLabel57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel57.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel57_SummaryGetResult);
        this.xrLabel57.SummaryReset += new System.EventHandler(this.xrLabel57_SummaryReset);
        this.xrLabel57.SummaryRowChanged += new System.EventHandler(this.xrLabel57_SummaryRowChanged);
        this.xrLabel57.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrLabel57_PrintOnPage);
        // 
        // xrLabel11
        // 
        this.xrLabel11.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Inv_SalesReturnHeader_SelectRow_Report.TotalAmount")});
        this.xrLabel11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(693.2757F, 0F);
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(141.7245F, 19.70834F);
        this.xrLabel11.StylePriority.UseBorderColor = false;
        this.xrLabel11.StylePriority.UseBorders = false;
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.StylePriority.UseTextAlignment = false;
        this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrLabel11.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel11_SummaryGetResult);
        this.xrLabel11.SummaryReset += new System.EventHandler(this.xrLabel11_SummaryReset);
        this.xrLabel11.SummaryRowChanged += new System.EventHandler(this.xrLabel11_SummaryRowChanged);
        this.xrLabel11.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel11_BeforePrint);
        this.xrLabel11.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrLabel11_PrintOnPage);
        // 
        // xrLabel10
        // 
        this.xrLabel10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(206)))), ((int)(((byte)(206)))));
        this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(567.7083F, 0F);
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(125.5674F, 19.70834F);
        this.xrLabel10.StylePriority.UseBorderColor = false;
        this.xrLabel10.StylePriority.UseBorders = false;
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.StylePriority.UseTextAlignment = false;
        this.xrLabel10.Text = "Gross Amount";
        this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // formattingRule1
        // 
        this.formattingRule1.Name = "formattingRule1";
        // 
        // ReportFooter
        // 
        this.ReportFooter.HeightF = 0F;
        this.ReportFooter.Name = "ReportFooter";
        // 
        // Sales_Return_Print
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.ReportHeader,
            this.GroupHeader1,
            this.GroupFooter1,
            this.ReportFooter});
        this.DataMember = "sp_Inv_SalesReturnHeader_SelectRow_Report";
        this.DataSource = this.SalesDataSet1;
        this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
        this.Margins = new System.Drawing.Printing.Margins(7, 0, 0, 69);
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.inventoryDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.SalesDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
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
        //Set_AddressMaster objAddMaster = new Set_AddressMaster();
        //string CompanyAddress = string.Empty;
        //DataTable DtAddress = objAddMaster.GetAddressDataByTransId(xrLabel36.Text, "0");
        //if (DtAddress.Rows.Count > 0)
        //{
        //    CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
        //    if (DtAddress.Rows[0]["Address"].ToString() != "")
        //    {
        //        CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
        //    }
        //    if (DtAddress.Rows[0]["Street"].ToString() != "")
        //    {
        //        if (CompanyAddress != "")
        //        {
        //            CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Street"].ToString();
        //        }
        //        else
        //        {
        //            CompanyAddress = DtAddress.Rows[0]["Street"].ToString();
        //        }
        //    }
        //    if (DtAddress.Rows[0]["Block"].ToString() != "")
        //    {
        //        if (CompanyAddress != "")
        //        {
        //            CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Block"].ToString();
        //        }
        //        else
        //        {
        //            CompanyAddress = DtAddress.Rows[0]["Block"].ToString();
        //        }
        //    }
        //    if (DtAddress.Rows[0]["Avenue"].ToString() != "")
        //    {
        //        if (CompanyAddress != "")
        //        {
        //            CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Avenue"].ToString();
        //        }
        //        else
        //        {
        //            CompanyAddress = DtAddress.Rows[0]["Avenue"].ToString();
        //        }
        //    }

        //    if (DtAddress.Rows[0]["CityId"].ToString() != "")
        //    {
        //        if (CompanyAddress != "")
        //        {
        //            CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["CityId"].ToString();
        //        }
        //        else
        //        {
        //            CompanyAddress = DtAddress.Rows[0]["CityId"].ToString();
        //        }

        //    }
        //    if (DtAddress.Rows[0]["StateId"].ToString() != "")
        //    {


        //        if (CompanyAddress != "")
        //        {
        //            CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["StateId"].ToString();
        //        }
        //        else
        //        {
        //            CompanyAddress = DtAddress.Rows[0]["StateId"].ToString();
        //        }
        //    }
        //    if (DtAddress.Rows[0]["CountryId"].ToString() != "")
        //    {
        //        string LocationName = string.Empty;
        //        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(GetCurrentColumnValue("Company_Id").ToString(), DtAddress.Rows[0]["CountryId"].ToString());
        //        if (DtLocation.Rows.Count > 0)
        //        {


        //            if (CompanyAddress != "")
        //            {
        //                CompanyAddress = CompanyAddress + "," + LocationName;
        //            }
        //            else
        //            {
        //                CompanyAddress = LocationName;
        //            }
        //        }

        //    }
        //    if (DtAddress.Rows[0]["PinCode"].ToString() != "")
        //    {
        //        if (CompanyAddress != "")
        //        {
        //            CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["PinCode"].ToString();
        //        }
        //        else
        //        {
        //            CompanyAddress = DtAddress.Rows[0]["PinCode"].ToString();
        //        }

        //    }


        //}
        //xrLabel36.Text = CompanyAddress;
    }

    private void xrLabel27_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        string Companytelno = string.Empty;
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Customer", xrLabel27.Text);
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
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Customer", xrLabel26.Text);
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
        //string Companytelno = string.Empty;
        //DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Contact", xrLabel40.Text);
        //if (DtAddress.Rows.Count > 0)
        //{
        //    if (DtAddress.Rows[0]["PhoneNo1"].ToString() != "")
        //    {

        //        Companytelno = DtAddress.Rows[0]["PhoneNo1"].ToString();
        //    }
        //    //if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
        //    //{
        //    //    if (Companytelno != "")
        //    //    {
        //    //        Companytelno = Companytelno + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
        //    //    }
        //    //    else
        //    //    {
        //    //        Companytelno = DtAddress.Rows[0]["PhoneNo2"].ToString();
        //    //    }
        //    //}
        //    if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
        //    {
        //        if (Companytelno != "")
        //        {
        //            Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo1"].ToString();
        //        }
        //        else
        //        {
        //            Companytelno = DtAddress.Rows[0]["MobileNo1"].ToString();
        //        }
        //    }
        //    //if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
        //    //{
        //    //    if (Companytelno != "")
        //    //    {
        //    //        Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
        //    //    }
        //    //    else
        //    //    {
        //    //        Companytelno = DtAddress.Rows[0]["MobileNo2"].ToString();
        //    //    }
        //    //}
        //}
        //xrLabel40.Text = Companytelno;

    }

    private void xrLabel37_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

        //string CompanyFaxno = string.Empty;
        //DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Contact", xrLabel37.Text);
        //if (DtAddress.Rows.Count > 0)
        //{
        //    if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
        //    {
        //        CompanyFaxno = DtAddress.Rows[0]["FaxNo"].ToString();
        //    }
        //}
        //xrLabel37.Text = CompanyFaxno;
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

        //e.Result = objsys.GetCurencyConversionForInv(CurrencyId, NetTaxper.ToString());
        //e.Handled = true;
    }

    private void xrLabel51_SummaryReset(object sender, EventArgs e)
    {

    }



    private void xrLabel51_SummaryRowChanged_1(object sender, EventArgs e)
    {
        //NetTaxper = Convert.ToDouble(GetCurrentColumnValue("NetTaxP").ToString());
      
        //CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        
    }

    private void xrLabel7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrLabel7.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel7.Text);

        }
        catch
        {
        }

    }

    private void xrLabel53_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrLabel53.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel53.Text);

        }
        catch
        {
        }
    }

    private void xrLabel55_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrLabel55.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel55.Text);

        //}
        //catch
        //{
        //}

    }

    private void xrLabel11_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        //e.Result = objsys.GetCurencyConversionForInv(CurrencyId, TotalGrossAmount.ToString());
        //e.Handled = true;
    }

    private void xrLabel11_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel11_SummaryRowChanged(object sender, EventArgs e)
    {
        //TotalGrossAmount = Convert.ToDouble(GetCurrentColumnValue("TotalAmount").ToString());
        //CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel55_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        //e.Result = objsys.GetCurencyConversionForInv(CurrencyId, TotalNetAmount.ToString());
        //e.Handled = true;
    }

    private void xrLabel55_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel55_SummaryRowChanged(object sender, EventArgs e)
    {

        //TotalNetAmount = Convert.ToDouble(GetCurrentColumnValue("GrandTotal").ToString());
        //CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (!Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()) && !Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
        {
            pnlDiscount.Visible = false;
            pnlTax.Visible = false;
            //xrLabel10.Visible = false;
            //xrLabel11.Visible = false;
            pnlDiscount.LocationF = new PointF(200F, 19.71F);
            pnlTax.LocationF = new PointF(200F, 19.71F);

            //xrLabel55.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            xrPanel2.LocationF = new PointF(0F, 19.71F);

            //xrLabel54.LocationF = new PointF(569.71F, 19.71F);
            //xrLabel55.LocationF = new PointF(694.44F, 19.71F);
            //xrPanel3.LocationF = new PointF(200,12);


        }
        if (Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()) && !Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
        {

            pnlDiscount.Visible = false;
            pnlTax.Visible = true;
            pnlTax.LocationF = new PointF(535.73F, 19.71F);
            pnlDiscount.LocationF = new PointF(200F, 19.71F);
            xrPanel2.LocationF = new PointF(0F, 80.38F);
            xrLabel56.Visible = true;
            xrLabel57.Visible = true;
            //pnlTax.Visible = false;
            //pnlDiscount.Visible = true;
            //pnlDiscount.LocationF = new PointF(535.73F, 19.71F);
            //pnlTax.LocationF = new PointF(200F, 19.71F);
            //xrPanel2.LocationF = new PointF(0F, 80.38F);
        }
        if (!Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()) && Convert.ToBoolean(objParam.GetParameterValue_By_ParameterName(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Brand_Id").ToString(), GetCurrentColumnValue("Location_Id").ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
        {
            pnlTax.Visible = false;
            pnlDiscount.Visible = true;
            pnlTax.LocationF = new PointF(200F, 19.71F);
            pnlDiscount.LocationF = new PointF(535.73F, 19.71F);
            xrPanel2.LocationF = new PointF(0F,80.38F);
            xrLabel56.Visible = false;
            xrLabel57.Visible = false;
            //pnlDiscount.Visible = false;
            //pnlTax.Visible = true;
            //pnlDiscount.LocationF = new PointF(200F, 19.71F);
            //pnlTax.LocationF = new PointF(535.73F, 19.71F);
            //xrPanel2.LocationF = new PointF(0F, 65F);
        }
    }

    private void xrLabel57_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        //e.Result = objsys.GetCurencyConversionForInv(CurrencyId, PriceAfterDiscount.ToString());
        //e.Handled = true;
    }

    private void xrLabel57_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel57_SummaryRowChanged(object sender, EventArgs e)
    {
        //PriceAfterDiscount = Convert.ToDouble(GetCurrentColumnValue("PriceAfterDiscount").ToString());
        //CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel7_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        //e.Result = objsys.GetCurencyConversionForInv(CurrencyId, totamCustomDiscountValue.ToString());
        //e.Handled = true;

    }

    private void xrLabel7_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel7_SummaryRowChanged(object sender, EventArgs e)
    {
        //totamCustomDiscountValue = Convert.ToDouble(GetCurrentColumnValue("NetDiscountV").ToString());
        //CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
    }

    private void xrLabel53_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        //e.Result = objsys.GetCurencyConversionForInv(CurrencyId, totamCustomTaxValue.ToString());
        //e.Handled = true;
    }

    private void xrLabel53_SummaryReset(object sender, EventArgs e)
    {

    }

    private void xrLabel53_SummaryRowChanged(object sender, EventArgs e)
    {
        //totamCustomTaxValue = Convert.ToDouble(GetCurrentColumnValue("NetTaxV").ToString());
        //CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();

    }

    private void xrLabel5_SummaryGetResult_1(object sender, SummaryGetResultEventArgs e)
    {

        //e.Result = objsys.GetCurencyConversionForInv(CurrencyId, NetDiscountper.ToString());
        //e.Handled = true;
    }

    private void xrLabel5_SummaryReset_1(object sender, EventArgs e)
    {

    }

    private void xrLabel5_SummaryRowChanged_1(object sender, EventArgs e)
    {
        //NetDiscountper = Convert.ToDouble(GetCurrentColumnValue("NetDiscountP").ToString());
     
        //CurrencyId = GetCurrentColumnValue("CurrencyID").ToString();
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
        DataTable dt = objCurrency.GetCurrencyMasterById(GetCurrentColumnValue("CurrencyID").ToString());
        if (dt.Rows.Count > 0)
        {
            xrTableCell26.Text = xrTableCell26.Text + "(" + dt.Rows[0]["Currency_Symbol"].ToString() + ")";

        }
    }

    private void xrLabel54_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        DataTable dt = objCurrency.GetCurrencyMasterById(GetCurrentColumnValue("CurrencyID").ToString());
        if (dt.Rows.Count > 0)
        {
            xrLabel54.Text = xrLabel54.Text + "(" + dt.Rows[0]["Currency_Symbol"].ToString() + ")";

        }

    }

    private void xrLabel35_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }

    private void xrLabel63_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //xrLabel63.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel63.Text.ToString());
    }

    private void xrLabel11_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrLabel11.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel11.Text.ToString());
        }
        catch
        {
        }
    }

    private void xrLabel5_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrLabel5.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel5.Text.ToString());
        }
        catch
        {
        }
    }

    private void xrLabel7_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrLabel7.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel7.Text.ToString());
        }
        catch
        {
        }
    }

    private void xrLabel57_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrLabel57.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel57.Text.ToString());
        }
        catch
        {
        }
    }

    private void xrLabel51_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrLabel51.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel51.Text.ToString());
        }
        catch
        {
        }
    }

    private void xrLabel53_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrLabel53.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel53.Text.ToString());
        }
        catch
        {
        }
    }

    private void xrLabel55_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        try
        {
            xrLabel55.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrLabel55.Text.ToString());
        }
        catch
        {
        }
    }

    private void xrLabel36_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrLabel36.Text = Convert.ToDateTime(xrLabel36.Text).ToString(objsys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrTableCell6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell6.Text = objsys.GetCurencyConversionForInv(GetCurrentColumnValue("CurrencyID").ToString(), xrTableCell6.Text);
        //}
        //catch
        //{
        //}
    }

}
