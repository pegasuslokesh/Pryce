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
public class ReconciledReportByVoucher : DevExpress.XtraReports.UI.XtraReport
{
    CompanyMaster ObjCompany = null;
    LocationMaster objLocation = null;
    SystemParameter objsys = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    Set_CustomerMaster ObjCustomerMaster = null;
    EmployeeMaster ObjEmployee = null;
    Ac_ChartOfAccount ObjCOA = null;
    Set_Suppliers ObjSuppliers = null;

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
    private XRLabel xrLabel2;
    private XRPanel xrPanel1;
    private AccountsDataset accountsDataset1;
    private ReportFooterBand ReportFooter;
    private XRLabel xrLabel1;
    private XRPageInfo xrPageInfo1;
    private XRLabel xrLabel3;
    private XRLabel xrLabel6;
    private XRLabel xrLabel4;
    private GroupHeaderBand GroupHeader1;
    private XRTable xrTable3;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell19;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell9;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell16;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell30;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private GroupFooterBand GroupFooter1;
    private XRTable xrTable1;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell3;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell11;
    private XRTable xrTable4;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTable xrTable5;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ReconciledReportByVoucher(string strConString)
    {
        InitializeComponent();
        ObjCompany = new CompanyMaster(strConString);
        objLocation = new LocationMaster(strConString);
        ObjAgeingDetail = new Ac_Ageing_Detail(strConString);
        objsys = new SystemParameter(strConString);
        ObjCustomerMaster = new Set_CustomerMaster(strConString);
        ObjEmployee = new EmployeeMaster(strConString);
        ObjCOA = new Ac_ChartOfAccount(strConString);
        ObjSuppliers = new Set_Suppliers(strConString);
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
        //xrPictureBox1.ImageUrl = Url;
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
        //xrTableCell27.Text = stropBalance;
    }
    public void setLastBalance(string setLastBalance)
    {
        // xrTableCell60.Text = setLastBalance;
    }
    public void setDateFilter(string strDatefiltertext)
    {
        xrLabel1.Text = strDatefiltertext;
    }
    public void setReconciledBy(string strReconciledBy)
    {
        
        
        if (strReconciledBy != "0")
        {
            xrLabel4.Visible = true;
            DataTable dtEmp = ObjEmployee.GetEmployeeMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), strReconciledBy);
            if (dtEmp.Rows.Count > 0)
            {
                xrLabel4.Text = "Reconciled By :" + dtEmp.Rows[0]["Emp_Name"].ToString();
            }
            else
            {
                xrLabel4.Text = "Reconciled By :";
            }
        }
        else
        {
            xrLabel4.Visible = false;
        }

    }
    public void setAccount(string strAccountName)
    {
        
        if (strAccountName != "0")
        {
            xrLabel6.Visible = true;
            DataTable dtCOA= ObjCOA.GetCOAByTransId(System.Web.HttpContext.Current.Session["CompId"].ToString(), strAccountName);
            if (dtCOA.Rows.Count > 0)
            {
                xrLabel6.Text = "Account Name :" + dtCOA.Rows[0]["AccountName"].ToString();
            }
            else
            {
                xrLabel6.Text = "Account Name :";
            }
        }
        else
        {
            xrLabel6.Visible = false;
        }
    }

    #region Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string resourceFileName = "ReconciledReportByVoucher.resx";
        DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
        DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrReportTitle = new DevExpress.XtraReports.UI.XRLabel();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.purchaseDataSet1 = new PurchaseDataSet();
        this.InventoryDataSet1 = new InventoryDataSet();
        this.accountsDataset1 = new AccountsDataset();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
        this.Detail.HeightF = 25F;
        this.Detail.KeepTogether = true;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrTable4
        // 
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(10.00028F, 0F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow9});
        this.xrTable4.SizeF = new System.Drawing.SizeF(1016.375F, 25F);
        // 
        // xrTableRow9
        // 
        this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell35,
            this.xrTableCell36,
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell40});
        this.xrTableRow9.Name = "xrTableRow9";
        this.xrTableRow9.Weight = 1D;
        // 
        // xrTableCell35
        // 
        this.xrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Voucher_No")});
        this.xrTableCell35.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell35.Name = "xrTableCell35";
        this.xrTableCell35.StylePriority.UseFont = false;
        this.xrTableCell35.StylePriority.UseTextAlignment = false;
        this.xrTableCell35.Text = "xrTableCell23";
        this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell35.Weight = 0.38505206391986052D;
        // 
        // xrTableCell36
        // 
        this.xrTableCell36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Voucher_Date")});
        this.xrTableCell36.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell36.Name = "xrTableCell36";
        this.xrTableCell36.StylePriority.UseFont = false;
        this.xrTableCell36.StylePriority.UseTextAlignment = false;
        this.xrTableCell36.Text = "xrTableCell24";
        this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell36.Weight = 0.37056253129066563D;
        // 
        // xrTableCell37
        // 
        this.xrTableCell37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Debit_Amount")});
        this.xrTableCell37.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell37.Name = "xrTableCell37";
        this.xrTableCell37.StylePriority.UseFont = false;
        this.xrTableCell37.StylePriority.UseTextAlignment = false;
        this.xrTableCell37.Text = "xrTableCell25";
        this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell37.Weight = 0.35598004438090758D;
        this.xrTableCell37.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell37_BeforePrint);
        // 
        // xrTableCell38
        // 
        this.xrTableCell38.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Credit_Amount")});
        this.xrTableCell38.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell38.Name = "xrTableCell38";
        this.xrTableCell38.StylePriority.UseBackColor = false;
        this.xrTableCell38.StylePriority.UseFont = false;
        this.xrTableCell38.StylePriority.UseTextAlignment = false;
        this.xrTableCell38.Text = "xrTableCell26";
        this.xrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell38.Weight = 0.38840536040856632D;
        this.xrTableCell38.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell38_BeforePrint);
        // 
        // xrTableCell39
        // 
        this.xrTableCell39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Is_Reconciled_Value")});
        this.xrTableCell39.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell39.Name = "xrTableCell39";
        this.xrTableCell39.StylePriority.UseFont = false;
        this.xrTableCell39.StylePriority.UseTextAlignment = false;
        this.xrTableCell39.Text = "xrTableCell27";
        this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell39.Weight = 0.5D;
        // 
        // xrTableCell40
        // 
        this.xrTableCell40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Remarks")});
        this.xrTableCell40.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell40.Name = "xrTableCell40";
        this.xrTableCell40.StylePriority.UseFont = false;
        this.xrTableCell40.StylePriority.UseTextAlignment = false;
        this.xrTableCell40.Text = "xrTableCell28";
        this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell40.Weight = 1D;
        // 
        // xrTable3
        // 
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(10.00025F, 119.0833F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
        this.xrTable3.SizeF = new System.Drawing.SizeF(1016.375F, 25F);
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17,
            this.xrTableCell22,
            this.xrTableCell18,
            this.xrTableCell21,
            this.xrTableCell20,
            this.xrTableCell19});
        this.xrTableRow4.Name = "xrTableRow4";
        this.xrTableRow4.Weight = 1D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell17.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.StylePriority.UseBackColor = false;
        this.xrTableCell17.StylePriority.UseBorders = false;
        this.xrTableCell17.StylePriority.UseFont = false;
        this.xrTableCell17.StylePriority.UseTextAlignment = false;
        this.xrTableCell17.Text = "Voucher No.\t";
        this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell17.Weight = 0.38505206391986052D;
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell22.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell22.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.StylePriority.UseBackColor = false;
        this.xrTableCell22.StylePriority.UseBorders = false;
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.StylePriority.UseTextAlignment = false;
        this.xrTableCell22.Text = "Date";
        this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell22.Weight = 0.37056253129066563D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell18.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseBackColor = false;
        this.xrTableCell18.StylePriority.UseBorders = false;
        this.xrTableCell18.StylePriority.UseFont = false;
        this.xrTableCell18.StylePriority.UseTextAlignment = false;
        this.xrTableCell18.Text = "Debit";
        this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell18.Weight = 0.35598004438090758D;
        // 
        // xrTableCell21
        // 
        this.xrTableCell21.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell21.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell21.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell21.Name = "xrTableCell21";
        this.xrTableCell21.StylePriority.UseBackColor = false;
        this.xrTableCell21.StylePriority.UseBorders = false;
        this.xrTableCell21.StylePriority.UseFont = false;
        this.xrTableCell21.StylePriority.UseTextAlignment = false;
        this.xrTableCell21.Text = "Credit";
        this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell21.Weight = 0.38840536040856632D;
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell20.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.StylePriority.UseBackColor = false;
        this.xrTableCell20.StylePriority.UseBorders = false;
        this.xrTableCell20.StylePriority.UseFont = false;
        this.xrTableCell20.StylePriority.UseTextAlignment = false;
        this.xrTableCell20.Text = "Reconciled Status";
        this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell20.Weight = 0.5D;
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell19.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell19.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.StylePriority.UseBackColor = false;
        this.xrTableCell19.StylePriority.UseBorders = false;
        this.xrTableCell19.StylePriority.UseFont = false;
        this.xrTableCell19.StylePriority.UseTextAlignment = false;
        this.xrTableCell19.Text = "Remarks";
        this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell19.Weight = 1D;
        // 
        // xrTable2
        // 
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 0F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2,
            this.xrTableRow3,
            this.xrTableRow6});
        this.xrTable2.SizeF = new System.Drawing.SizeF(1016.375F, 117.7083F);
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBackColor = false;
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "Reconciled By";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell1.Weight = 0.38505197631489474D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Reconciled_By_Name")});
        this.xrTableCell2.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UseTextAlignment = false;
        this.xrTableCell2.Text = "xrTableCell2";
        this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell2.Weight = 2.6149480236851055D;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell9});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBackColor = false;
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.Text = "Reconcilation No";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell4.Weight = 0.38505197631489474D;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.ReconcilationNo")});
        this.xrTableCell5.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseFont = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "xrTableCell5";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell5.Weight = 1.5100034715549611D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseBackColor = false;
        this.xrTableCell6.StylePriority.UseBorders = false;
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "Total Reconciled Voucher";
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell6.Weight = 0.596920695525921D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Total_Reconciled_Record")});
        this.xrTableCell9.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.Text = "xrTableCell9";
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell9.Weight = 0.50802385660422333D;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrTableCell12,
            this.xrTableCell14,
            this.xrTableCell16});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell10.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.StylePriority.UseBackColor = false;
        this.xrTableCell10.StylePriority.UseBorders = false;
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.StylePriority.UseTextAlignment = false;
        this.xrTableCell10.Text = "Reconcilation Date";
        this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell10.Weight = 0.38505197631489474D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.ReconcileDate")});
        this.xrTableCell12.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.StylePriority.UseTextAlignment = false;
        this.xrTableCell12.Text = "xrTableCell12";
        this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell12.Weight = 1.5100034715549611D;
        this.xrTableCell12.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell12_BeforePrint);
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell14.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseBackColor = false;
        this.xrTableCell14.StylePriority.UseBorders = false;
        this.xrTableCell14.StylePriority.UseFont = false;
        this.xrTableCell14.StylePriority.UseTextAlignment = false;
        this.xrTableCell14.Text = "Conflicted Voucher";
        this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell14.Weight = 0.596920695525921D;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Total_Not_Reconciled_Record")});
        this.xrTableCell16.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseFont = false;
        this.xrTableCell16.StylePriority.UseTextAlignment = false;
        this.xrTableCell16.Text = "xrTableCell16";
        this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell16.Weight = 0.50802385660422333D;
        // 
        // xrTableRow6
        // 
        this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell29,
            this.xrTableCell30});
        this.xrTableRow6.Name = "xrTableRow6";
        this.xrTableRow6.Weight = 1D;
        // 
        // xrTableCell29
        // 
        this.xrTableCell29.BackColor = System.Drawing.Color.Silver;
        this.xrTableCell29.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell29.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell29.Name = "xrTableCell29";
        this.xrTableCell29.StylePriority.UseBackColor = false;
        this.xrTableCell29.StylePriority.UseBorders = false;
        this.xrTableCell29.StylePriority.UseFont = false;
        this.xrTableCell29.StylePriority.UseTextAlignment = false;
        this.xrTableCell29.Text = "Remark";
        this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell29.Weight = 0.38505197631489474D;
        // 
        // xrTableCell30
        // 
        this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.HeaderRemark")});
        this.xrTableCell30.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell30.Name = "xrTableCell30";
        this.xrTableCell30.StylePriority.UseFont = false;
        this.xrTableCell30.StylePriority.UseTextAlignment = false;
        this.xrTableCell30.Text = "xrTableCell30";
        this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        this.xrTableCell30.Weight = 2.6149480236851055D;
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
        this.BottomMargin.HeightF = 32F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel4,
            this.xrLabel1,
            this.xrPanel1});
        this.ReportHeader.HeightF = 91.54835F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrLabel6
        // 
        this.xrLabel6.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(735.5842F, 62.25007F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(290.7908F, 27.00003F);
        this.xrLabel6.StylePriority.UseFont = false;
        // 
        // xrLabel4
        // 
        this.xrLabel4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic);
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(735.5845F, 39.25006F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(290.7907F, 23.00001F);
        this.xrLabel4.StylePriority.UseFont = false;
        // 
        // xrLabel1
        // 
        this.xrLabel1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 66.25007F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(491.9295F, 23.00002F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrReportTitle});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(596.1666F, 56.25008F);
        this.xrPanel1.StylePriority.UseBorders = false;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 1.333329F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(579.4344F, 21.95834F);
        this.xrLabel2.StylePriority.UseBorders = false;
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "xrLabel1";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrReportTitle
        // 
        this.xrReportTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrReportTitle.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 33.25008F);
        this.xrReportTitle.Name = "xrReportTitle";
        this.xrReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrReportTitle.SizeF = new System.Drawing.SizeF(491.9295F, 23F);
        this.xrReportTitle.StylePriority.UseBorders = false;
        this.xrReportTitle.StylePriority.UseFont = false;
        this.xrReportTitle.StylePriority.UseTextAlignment = false;
        this.xrReportTitle.Text = "RECONCILED REPORT BY VOUCHER";
        this.xrReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // PageHeader
        // 
        this.PageHeader.HeightF = 11F;
        this.PageHeader.Name = "PageHeader";
        // 
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrPageInfo1,
            this.xrPageInfo2});
        this.PageFooter.HeightF = 23F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrLabel3
        // 
        this.xrLabel3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 0F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(70.83333F, 18.12F);
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.Text = "Print On :";
        // 
        // xrPageInfo1
        // 
        this.xrPageInfo1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrPageInfo1.Format = "{0:dd-MMM-yy}";
        this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(80.83333F, 0F);
        this.xrPageInfo1.Name = "xrPageInfo1";
        this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo1.SizeF = new System.Drawing.SizeF(474.0706F, 18.12496F);
        this.xrPageInfo1.StylePriority.UseBorders = false;
        this.xrPageInfo1.StylePriority.UseFont = false;
        this.xrPageInfo1.StylePriority.UseTextAlignment = false;
        this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrPageInfo2.Format = "Page{0}Of {1}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(554.9039F, 0F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(501.3751F, 18.12496F);
        this.xrPageInfo2.StylePriority.UseBorders = false;
        this.xrPageInfo2.StylePriority.UseFont = false;
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
            this.xrTable5});
        this.ReportFooter.HeightF = 84.375F;
        this.ReportFooter.KeepTogether = true;
        this.ReportFooter.Name = "ReportFooter";
        // 
        // xrTable5
        // 
        this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(10.00028F, 3.999964F);
        this.xrTable5.Name = "xrTable5";
        this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
        this.xrTable5.SizeF = new System.Drawing.SizeF(1020.375F, 25F);
        // 
        // xrTableRow5
        // 
        this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13,
            this.xrTableCell15,
            this.xrTableCell23,
            this.xrTableCell24});
        this.xrTableRow5.Name = "xrTableRow5";
        this.xrTableRow5.Weight = 1D;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.StylePriority.UseTextAlignment = false;
        this.xrTableCell13.Text = "Total :";
        this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell13.Weight = 0.75561457380511321D;
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Debit_Amount")});
        this.xrTableCell15.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.StylePriority.UseFont = false;
        this.xrTableCell15.StylePriority.UseTextAlignment = false;
        xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell15.Summary = xrSummary1;
        this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell15.Weight = 0.35598003109242404D;
        this.xrTableCell15.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell15_BeforePrint);
        this.xrTableCell15.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell15_PrintOnPage);
        // 
        // xrTableCell23
        // 
        this.xrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Credit_Amount")});
        this.xrTableCell23.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell23.Name = "xrTableCell23";
        this.xrTableCell23.StylePriority.UseFont = false;
        this.xrTableCell23.StylePriority.UseTextAlignment = false;
        xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
        this.xrTableCell23.Summary = xrSummary2;
        this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell23.Weight = 0.38840515489523142D;
        this.xrTableCell23.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell23_BeforePrint);
        this.xrTableCell23.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell23_PrintOnPage);
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.Weight = 1.5000002402072314D;
        // 
        // GroupHeader1
        // 
        this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xrTable3});
        this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ReconcilationNo", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader1.HeightF = 144.7917F;
        this.GroupHeader1.KeepTogether = true;
        this.GroupHeader1.Name = "GroupHeader1";
        // 
        // GroupFooter1
        // 
        this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
        this.GroupFooter1.HeightF = 25F;
        this.GroupFooter1.Name = "GroupFooter1";
        // 
        // xrTable1
        // 
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 0F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
        this.xrTable1.SizeF = new System.Drawing.SizeF(1016.375F, 25F);
        // 
        // xrTableRow7
        // 
        this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell11});
        this.xrTableRow7.Name = "xrTableRow7";
        this.xrTableRow7.Weight = 1D;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.StylePriority.UseTextAlignment = false;
        this.xrTableCell3.Text = "Total :";
        this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell3.Weight = 0.75561457380511321D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Debit_Amount")});
        this.xrTableCell7.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UseTextAlignment = false;
        xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrTableCell7.Summary = xrSummary3;
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell7.Weight = 0.35598003109242404D;
        this.xrTableCell7.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell7_BeforePrint);
        this.xrTableCell7.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell7_PrintOnPage);
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Ac_Reconcile_Detail_Report.Credit_Amount")});
        this.xrTableCell8.Font = new System.Drawing.Font("Calibri", 9F);
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.StylePriority.UseTextAlignment = false;
        xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
        this.xrTableCell8.Summary = xrSummary4;
        this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
        this.xrTableCell8.Weight = 0.38840515489523142D;
        this.xrTableCell8.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell8_BeforePrint);
        this.xrTableCell8.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell8_PrintOnPage);
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.Weight = 1.5000002402072314D;
        // 
        // ReconciledReportByVoucher
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageHeader,
            this.PageFooter,
            this.ReportFooter,
            this.GroupHeader1,
            this.GroupFooter1});
        this.DataMember = "sp_Ac_Reconcile_Detail_Report";
        this.DataSource = this.accountsDataset1;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.Landscape = true;
        this.Margins = new System.Drawing.Printing.Margins(23, 0, 0, 32);
        this.PageHeight = 850;
        this.PageWidth = 1100;
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.purchaseDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.InventoryDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.accountsDataset1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
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
        if (xrTableCell5.Text != "")
        {

            try
            {
                xrTableCell5.Text = objsys.GetCurencyConversionForInv(ObjCompany.GetCompanyMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString(), xrTableCell5.Text);
            }
            catch
            {

            }
        }
    }  
    private void xrTableCell16_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell16.Text != "")
        {
            try
            {
                xrTableCell16.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell16.Text);
            }
            catch
            {
                xrTableCell16.Text = "0";
            }
        }
        else
        {
            xrTableCell16.Text = "0";
        }
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

    private void xrTableCell9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell9.Text != "")
        {
            try
            {
                xrTableCell9.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell9.Text);
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
    private void xrTableCell37_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell37.Text != "")
        {
            try
            {
                xrTableCell37.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell37.Text);
            }
            catch
            {
                xrTableCell37.Text = "0";
            }
        }
        else
        {
            xrTableCell37.Text = "0";
        }
    }
    private void xrTableCell38_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell38.Text != "")
        {
            try
            {
                xrTableCell38.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell38.Text);
            }
            catch
            {
                xrTableCell38.Text = "0";
            }
        }
        else
        {
            xrTableCell38.Text = "0";
        }
    }  
    private void xrTableCell12_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell12.Text != "")
        //{
        //    try
        //    {
        //        xrTableCell12.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell12.Text);
        //    }
        //    catch
        //    {
        //        xrTableCell12.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrTableCell12.Text = "0";
        //}
    }

    private void xrTableCell13_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell13.Text != "")
        //{
        //    try
        //    {
        //        xrTableCell13.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell13.Text);
        //    }
        //    catch
        //    {
        //        xrTableCell13.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrTableCell13.Text = "0";
        //}
    }       
    private void xrTableCell36_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell36.Text != "")
        //{
        //    try
        //    {
        //        xrTableCell36.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell36.Text);
        //    }
        //    catch
        //    {
        //        xrTableCell36.Text = "0";

        //    }
        //}
        //else
        //{
        //    xrTableCell36.Text = "0";
        //}
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

   
    public void setCurrencySymbol(string CurrencyId)
    {
       // xrTableCell34.Text = SystemParameter.GetCurrencySmbol(CurrencyId, "Balance");
        //xrAgeingTable.Text = SystemParameter.GetCurrencySmbol(CurrencyId, "Ageing Analysis");
        //DataTable dtSupplier = ObjSupplierMaster.GetSupplierAllDataBySupplierId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), GetCurrentColumnValue("Other_Account_No").ToString());
        //if (dtSupplier.Rows.Count > 0)
        //{
        //    string strSupCurr = dtSupplier.Rows[0]["Field3"].ToString();
        //    xrTableCell18.Text = SystemParameter.GetCurrencySmbol(strSupCurr, "Foreign Amount");
        //}
        //xrTableCell4.Text = SystemParameter.GetCurrencySmbol(CurrencyId, "Ageing Balance");
    }

    //For Ageing Detail
    public static string[] ForGetAgeingDetail(string strCustomerId,string strConString)
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
    private void xrTableCell12_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell12.Text = Convert.ToDateTime(xrTableCell12.Text).ToString(objsys.SetDateFormat());
        }
        catch
        {

        }
    }   
    //For Opening Balance
    public string GetOpeningBalance(string strSupplierId)
    {
        string strOpening = string.Empty;

        DataTable dtSup = ObjSuppliers.GetSupplierAllDataBySupplierId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), strSupplierId);
        if (dtSup.Rows.Count > 0)
        {
            double dtDebit = Convert.ToDouble(dtSup.Rows[0]["O_Db_Amount"].ToString());
            double dtCredit = Convert.ToDouble(dtSup.Rows[0]["O_Cr_Amount"].ToString());

            if (dtDebit != 0)
            {
                strOpening = dtDebit.ToString();
            }
            else if (dtCredit != 0)
            {
                strOpening = dtCredit.ToString();
            }
            else
            {
                strOpening = "0";
            }
        }
        else
        {
            strOpening = "0.00";
        }
        return strOpening;
    }


    private void xrTableCell51_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //xrTableCell51.Text = GetOpeningBalance(GetCurrentColumnValue("Other_Account_No").ToString());
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
    //private void xrTableCell20_PrintOnPage(object sender, PrintOnPageEventArgs e)
    //{
    //    if (xrTableCell20.Text != "")
    //    {
    //        try
    //        {
    //            xrTableCell20.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell20.Text);
    //        }
    //        catch
    //        {
    //            xrTableCell20.Text = "0";
    //        }
    //    }
    //    else
    //    {
    //        xrTableCell20.Text = "0";
    //    }
    //}

    //private void xrTableCell11_PrintOnPage(object sender, PrintOnPageEventArgs e)
    //{
    //    if (xrTableCell11.Text != "")
    //    {
    //        try
    //        {
    //            xrTableCell11.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell11.Text);
    //        }
    //        catch
    //        {
    //            xrTableCell11.Text = "0";
    //        }
    //    }
    //    else
    //    {
    //        xrTableCell11.Text = "0";
    //    }
    //}   

    private void xrTableCell7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell7.Text != "")
        {
            try
            {
                xrTableCell7.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell7.Text);
            }
            catch
            {
                xrTableCell7.Text = "0";
            }
        }
        else
        {
            xrTableCell7.Text = "0";
        }
    }
    private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell8.Text != "")
        {
            try
            {
                xrTableCell8.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell8.Text);
            }
            catch
            {
                xrTableCell8.Text = "0";
            }
        }
        else
        {
            xrTableCell8.Text = "0";
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
    private void xrTableCell23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell23.Text != "")
        {
            try
            {
                xrTableCell23.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell23.Text);
            }
            catch
            {
                xrTableCell23.Text = "0";
            }
        }
        else
        {
            xrTableCell23.Text = "0";
        }
    }
    private void xrTableCell7_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell7.Text != "")
        {
            try
            {
                xrTableCell7.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell7.Text);
            }
            catch
            {
                xrTableCell7.Text = "0";
            }
        }
        else
        {
            xrTableCell7.Text = "0";
        }
    }
    private void xrTableCell8_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell8.Text != "")
        {
            try
            {
                xrTableCell8.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell8.Text);
            }
            catch
            {
                xrTableCell8.Text = "0";
            }
        }
        else
        {
            xrTableCell8.Text = "0";
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
    private void xrTableCell23_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell23.Text != "")
        {
            try
            {
                xrTableCell23.Text = objsys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(System.Web.HttpContext.Current.Session["CompId"].ToString(), System.Web.HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), xrTableCell23.Text);
            }
            catch
            {
                xrTableCell23.Text = "0";
            }
        }
        else
        {
            xrTableCell23.Text = "0";
        }
    }
}
