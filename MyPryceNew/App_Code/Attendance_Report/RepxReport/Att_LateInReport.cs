using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for Att_TourReport
/// </summary>
public class Att_LateInReport : DevExpress.XtraReports.UI.XtraReport
{
   
	private DevExpress.XtraReports.UI.DetailBand Detail;
	private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
	private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private XRPanel xrPanel1;
    private XRLabel xrCompAddress;
    private XRLabel xrTitle;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrCompName;
    private PageFooterBand PageFooter;
    private XRPageInfo xrPageInfo2;
    private AttendanceDataSet attendanceDataSet1;
    private AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_TourReportTableAdapter sp_Att_AttendanceLog_TourReportTableAdapter1;
    private AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_LateReportTableAdapter sp_Att_AttendanceRegister_LateReportTableAdapter1;
    private XRLabel xrLabel16;
    private XRLabel xrLabel15;
    private XRPageInfo xrPageInfo3;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private PageHeaderBand PageHeader;
    private XRTable xrTable3;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTable xrTable5;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTable xrTable6;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell43;
    private XRTableCell xrTableCell44;
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	public Att_LateInReport()
	{
		InitializeComponent();
		//
		// TODO: Add constructor logic here
		//
	}
	
	/// <summary> 
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing) {
		if (disposing && (components != null)) {
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	#region Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent() {
        string resourceFileName = "Att_LateInReport.resx";
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrCompAddress = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTitle = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrCompName = new DevExpress.XtraReports.UI.XRLabel();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.attendanceDataSet1 = new AttendanceDataSet();
        this.sp_Att_AttendanceLog_TourReportTableAdapter1 = new AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_TourReportTableAdapter();
        this.sp_Att_AttendanceRegister_LateReportTableAdapter1 = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_LateReportTableAdapter();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.attendanceDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
        this.Detail.HeightF = 15.625F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrTable2
        // 
        this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable2.Font = new System.Drawing.Font("Times New Roman", 8.25F);
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
        this.xrTable2.SizeF = new System.Drawing.SizeF(933F, 15.625F);
        this.xrTable2.StylePriority.UseBorders = false;
        this.xrTable2.StylePriority.UseFont = false;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell8,
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell14});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_LateReport.Emp_Code")});
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.Text = "Employee Code";
        this.xrTableCell8.Weight = 1.1249999165113458D;
        this.xrTableCell8.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell8_BeforePrint);
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_LateReport.Emp_Name")});
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.Text = "Employee Name";
        this.xrTableCell9.Weight = 1.6587490465565695D;
        this.xrTableCell9.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell9_BeforePrint);
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_LateReport.Dep_Name")});
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.Text = "Department ";
        this.xrTableCell10.Weight = 1.7896371870328478D;
        this.xrTableCell10.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell10_BeforePrint);
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_LateReport.Designation")});
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.Text = "Designation";
        this.xrTableCell11.Weight = 1.39286433238872D;
        this.xrTableCell11.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell11_BeforePrint);
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_LateReport.Phone_No")});
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.Text = "Phone No.";
        this.xrTableCell12.Weight = 0.99874884292219646D;
        this.xrTableCell12.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell12_BeforePrint);
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_LateReport.Email_Id")});
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.Text = "Email Id";
        this.xrTableCell13.Weight = 1.5647913065492749D;
        this.xrTableCell13.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell13_BeforePrint);
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_LateReport.LateCount")});
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseBorders = false;
        this.xrTableCell14.Text = "Late Count";
        this.xrTableCell14.Weight = 0.80020883102564178D;
        this.xrTableCell14.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell14_BeforePrint);
        // 
        // TopMargin
        // 
        this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel16,
            this.xrLabel15,
            this.xrPageInfo3,
            this.xrPanel1});
        this.TopMargin.HeightF = 132.7917F;
        this.TopMargin.Name = "TopMargin";
        this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel16
        // 
        this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(91.95319F, 15.70834F);
        this.xrLabel16.StylePriority.UseBorders = false;
        this.xrLabel16.StylePriority.UseFont = false;
        this.xrLabel16.Text = "Created By:";
        // 
        // xrLabel15
        // 
        this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(91.95319F, 0F);
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(186.4217F, 15.70834F);
        this.xrLabel15.StylePriority.UseBorders = false;
        this.xrLabel15.StylePriority.UseFont = false;
        this.xrLabel15.StylePriority.UseTextAlignment = false;
        this.xrLabel15.Text = "xrLabel10";
        this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPageInfo3
        // 
        this.xrPageInfo3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo3.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
        this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(722.1667F, 0F);
        this.xrPageInfo3.Name = "xrPageInfo3";
        this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo3.SizeF = new System.Drawing.SizeF(210.8333F, 15.70834F);
        this.xrPageInfo3.StylePriority.UseBorders = false;
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable5,
            this.xrTable6,
            this.xrCompAddress,
            this.xrTitle,
            this.xrPictureBox1,
            this.xrCompName});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 15.70833F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(933F, 117.0833F);
        this.xrPanel1.StylePriority.UseBorders = false;
        // 
        // xrTable5
        // 
        this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(3.000006F, 48.04167F);
        this.xrTable5.Name = "xrTable5";
        this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
        this.xrTable5.SizeF = new System.Drawing.SizeF(920F, 20F);
        // 
        // xrTableRow6
        // 
        this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell31,
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell40});
        this.xrTableRow6.Name = "xrTableRow6";
        this.xrTableRow6.Weight = 1D;
        // 
        // xrTableCell31
        // 
        this.xrTableCell31.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell31.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
        this.xrTableCell31.Name = "xrTableCell31";
        this.xrTableCell31.StylePriority.UseBorders = false;
        this.xrTableCell31.StylePriority.UseFont = false;
        this.xrTableCell31.Weight = 1.5527376766560708D;
        // 
        // xrTableCell38
        // 
        this.xrTableCell38.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell38.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell38.Name = "xrTableCell38";
        this.xrTableCell38.StylePriority.UseBorders = false;
        this.xrTableCell38.StylePriority.UseFont = false;
        this.xrTableCell38.Weight = 4.589366808434284D;
        // 
        // xrTableCell39
        // 
        this.xrTableCell39.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell39.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
        this.xrTableCell39.Name = "xrTableCell39";
        this.xrTableCell39.StylePriority.UseBorders = false;
        this.xrTableCell39.StylePriority.UseFont = false;
        this.xrTableCell39.Weight = 1.6022137976687303D;
        // 
        // xrTableCell40
        // 
        this.xrTableCell40.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell40.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell40.Name = "xrTableCell40";
        this.xrTableCell40.StylePriority.UseBorders = false;
        this.xrTableCell40.StylePriority.UseFont = false;
        this.xrTableCell40.Text = "xrTableCell40";
        this.xrTableCell40.Weight = 4.694806604763202D;
        // 
        // xrTable6
        // 
        this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(3.000006F, 68.04167F);
        this.xrTable6.Name = "xrTable6";
        this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
        this.xrTable6.SizeF = new System.Drawing.SizeF(572.7712F, 20F);
        // 
        // xrTableRow7
        // 
        this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell43,
            this.xrTableCell44});
        this.xrTableRow7.Name = "xrTableRow7";
        this.xrTableRow7.Weight = 1D;
        // 
        // xrTableCell43
        // 
        this.xrTableCell43.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell43.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
        this.xrTableCell43.Name = "xrTableCell43";
        this.xrTableCell43.StylePriority.UseBorders = false;
        this.xrTableCell43.StylePriority.UseFont = false;
        this.xrTableCell43.Text = "xrTableCell43";
        this.xrTableCell43.Weight = 2.8900798687987059D;
        // 
        // xrTableCell44
        // 
        this.xrTableCell44.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell44.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell44.Name = "xrTableCell44";
        this.xrTableCell44.StylePriority.UseBorders = false;
        this.xrTableCell44.StylePriority.UseFont = false;
        this.xrTableCell44.Weight = 11.524266415913102D;
        // 
        // xrCompAddress
        // 
        this.xrCompAddress.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrCompAddress.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrCompAddress.LocationFloat = new DevExpress.Utils.PointFloat(3.000005F, 23.99999F);
        this.xrCompAddress.Name = "xrCompAddress";
        this.xrCompAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrCompAddress.SizeF = new System.Drawing.SizeF(755.2083F, 20F);
        this.xrCompAddress.StylePriority.UseBorders = false;
        this.xrCompAddress.StylePriority.UseFont = false;
        this.xrCompAddress.Text = "xrCompAddress";
        // 
        // xrTitle
        // 
        this.xrTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTitle.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTitle.LocationFloat = new DevExpress.Utils.PointFloat(191.4376F, 92.04168F);
        this.xrTitle.Name = "xrTitle";
        this.xrTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrTitle.SizeF = new System.Drawing.SizeF(583.4271F, 20F);
        this.xrTitle.StylePriority.UseBorders = false;
        this.xrTitle.StylePriority.UseFont = false;
        this.xrTitle.StylePriority.UseTextAlignment = false;
        this.xrTitle.Text = "xrTitle";
        this.xrTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(831F, 3.000005F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(100F, 45.04167F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrCompName
        // 
        this.xrCompName.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrCompName.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrCompName.LocationFloat = new DevExpress.Utils.PointFloat(3.000005F, 2.999994F);
        this.xrCompName.Name = "xrCompName";
        this.xrCompName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrCompName.SizeF = new System.Drawing.SizeF(755.2083F, 20F);
        this.xrCompName.StylePriority.UseBorders = false;
        this.xrCompName.StylePriority.UseFont = false;
        this.xrCompName.Text = "xrCompName";
        // 
        // BottomMargin
        // 
        this.BottomMargin.HeightF = 0F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2});
        this.PageFooter.HeightF = 23F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Format = "Page{0}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(933F, 23F);
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // attendanceDataSet1
        // 
        this.attendanceDataSet1.DataSetName = "AttendanceDataSet";
        this.attendanceDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // sp_Att_AttendanceLog_TourReportTableAdapter1
        // 
        this.sp_Att_AttendanceLog_TourReportTableAdapter1.ClearBeforeFill = true;
        // 
        // sp_Att_AttendanceRegister_LateReportTableAdapter1
        // 
        this.sp_Att_AttendanceRegister_LateReportTableAdapter1.ClearBeforeFill = true;
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
        this.PageHeader.HeightF = 15.625F;
        this.PageHeader.Name = "PageHeader";
        // 
        // xrTable3
        // 
        this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable3.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
        this.xrTable3.Name = "xrTable3";
        this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
        this.xrTable3.SizeF = new System.Drawing.SizeF(933F, 15.625F);
        this.xrTable3.StylePriority.UseBorders = false;
        this.xrTable3.StylePriority.UseFont = false;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.Weight = 1.1249999165113458D;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.Weight = 1.6587490465565695D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.Weight = 1.7896371870328478D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.Weight = 1.39286433238872D;
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.Weight = 0.99874884292219646D;
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.Weight = 1.5647913065492749D;
        // 
        // xrTableCell21
        // 
        this.xrTableCell21.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell21.Name = "xrTableCell21";
        this.xrTableCell21.StylePriority.UseBorders = false;
        this.xrTableCell21.Weight = 0.80020883102564178D;
        // 
        // Att_LateInReport
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.PageHeader});
        this.DataMember = "sp_Att_AttendanceRegister_LateReport";
        this.DataSource = this.attendanceDataSet1;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.Margins = new System.Drawing.Printing.Margins(28, 6, 133, 0);
        this.PageWidth = 967;
        this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.attendanceDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

	}

	#endregion

    public void setheaderName(string EmployeeCode, string EmpName, string Department, string Designation, string PhoneNo, string EmailId, string LateCount)
    {
        xrTableCell15.Text = EmployeeCode;
        xrTableCell16.Text = EmpName;
        xrTableCell17.Text = Department;
        xrTableCell18.Text = Designation;
        xrTableCell19.Text = PhoneNo;
        xrTableCell20.Text = EmailId;
        xrTableCell21.Text = LateCount;

        xrTableCell31.Text = Resources.Attendance.Brand;
        xrTableCell39.Text = Resources.Attendance.Location;
        xrTableCell43.Text = Resources.Attendance.Department;
    }
    public void SetBrandName(string BrandName)
    {
        xrTableCell38.Text = BrandName;
    }
    public void SetLocationName(string LocationName)
    {
        xrTableCell40.Text = LocationName;
    }
    public void SetDepartmentName(string DepartmentName)
    {
        xrTableCell44.Text = DepartmentName;
    }

    public void SetImage(string Url)
    {
        xrPictureBox1.ImageUrl = Url;
    }
    public void setTitleName(string Title)
    {
        xrTitle.Text = Title;

    }
    public void setaddress(string address)
    {
        xrCompAddress.Text = address;
    }
    public void setcompanyname(string companyname)
    {
        xrCompName.Text = companyname;
    }

    //private void xrLabel7_AfterPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    //{
    //    if (xrLabel7.Text == "")
    //    {
    //        xrLabel7.Text = "-";

    //    }

    //    string latecount = string.Empty;

    //    string lateparam = string.Empty;
    //    try
    //    {
    //        latecount = this.GetCurrentColumnValue("LateCount").ToString();
    //        lateparam = this.GetCurrentColumnValue("LateParam").ToString();
    //    }
    //    catch
    //    {
    //    }

    //    if (latecount != "")
    //    {
    //        if (int.Parse(latecount) > int.Parse(lateparam))
    //        { xrLabel7.BackColor = Color.Yellow;

    //        }
    //        else
    //        {
    //            xrLabel7.BackColor = Color.White;

    //        }

    //    }
        
    //}

   

   

    
    //private void xrLabel6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    //{
    //    if (xrLabel6.Text == "")
    //    {
    //        xrLabel6.Text = "-";

    //    }
    //    string latecount = string.Empty;

    //    string lateparam = string.Empty;
    //    try
    //    {
    //        latecount = this.GetCurrentColumnValue("LateCount").ToString();
    //        lateparam = this.GetCurrentColumnValue("LateParam").ToString();
    //    }
    //    catch
    //    {
    //    }
    //    if (latecount != "")
    //    {
    //        if (int.Parse(latecount) > int.Parse(lateparam))
    //        {
    //            xrLabel6.BackColor = Color.Yellow;

    //        }
    //        else
    //        {
    //            xrLabel6.BackColor = Color.White;

    //        }

    //    }

    //}

    //private void xrLabel7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    //{
    //    if (xrLabel7.Text == "")
    //    {
    //        xrLabel7.Text = "-";

    //    }
    //    string latecount = string.Empty;

    //    string lateparam = string.Empty;
    //    try
    //    {
    //        latecount = this.GetCurrentColumnValue("LateCount").ToString();
    //        lateparam = this.GetCurrentColumnValue("LateParam").ToString();
    //    }
    //    catch
    //    {
    //    }
    //    if (latecount != "")
    //    {
    //        if (int.Parse(latecount) > int.Parse(lateparam))
    //        {
    //            xrLabel7.BackColor = Color.Yellow;

    //        }
    //        else
    //        {
    //            xrLabel7.BackColor = Color.White;

    //        }

    //    }

    //}

   
    public void setUserName(string UserName)
    {
        xrLabel15.Text = UserName;
    }
   
    private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell8.Text == "")
        {
            xrTableCell8.Text = "-";

        }
        string latecount = string.Empty;

        string lateparam = string.Empty;
        try
        {
            latecount = this.GetCurrentColumnValue("LateCount").ToString();
            lateparam = this.GetCurrentColumnValue("LateParam").ToString();
            if (latecount != "")
            {
                if (int.Parse(latecount) > int.Parse(lateparam))
                {
                    xrTableCell8.BackColor = Color.Yellow;

                }
                else
                {
                    xrTableCell8.BackColor = Color.White;

                }

            }
        }
        catch
        {
        }

        

    }

    private void xrTableCell9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell9.Text == "")
        {
            xrTableCell9.Text = "-";

        }
        string latecount = string.Empty;

        string lateparam = string.Empty;
        try
        {
            latecount = this.GetCurrentColumnValue("LateCount").ToString();
            lateparam = this.GetCurrentColumnValue("LateParam").ToString();
            if (latecount != "")
            {
                if (int.Parse(latecount) > int.Parse(lateparam))
                {
                    xrTableCell9.BackColor = Color.Yellow;

                }
                else
                {
                    xrTableCell9.BackColor = Color.White;

                }

            }
        }
        catch
        {
        }
       

    }

    private void xrTableCell10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell10.Text == "")
        {
            xrTableCell10.Text = "-";

        }
        string latecount = string.Empty;

        string lateparam = string.Empty;
        try
        {
            latecount = this.GetCurrentColumnValue("LateCount").ToString();
            lateparam = this.GetCurrentColumnValue("LateParam").ToString();
            if (latecount != "")
            {
                if (int.Parse(latecount) > int.Parse(lateparam))
                {
                    xrTableCell10.BackColor = Color.Yellow;

                }
                else
                {
                    xrTableCell10.BackColor = Color.White;

                }

            }
        }
        catch
        {
        }
       
    }

    private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell11.Text == "")
        {
            xrTableCell11.Text = "-";

        }
        string latecount = string.Empty;

        string lateparam = string.Empty;
        try
        {
            latecount = this.GetCurrentColumnValue("LateCount").ToString();
            lateparam = this.GetCurrentColumnValue("LateParam").ToString();
            if (latecount != "")
            {
                if (int.Parse(latecount) > int.Parse(lateparam))
                {
                    xrTableCell11.BackColor = Color.Yellow;

                }
                else
                {
                    xrTableCell11.BackColor = Color.White;

                }

            }
        }
        catch
        {
        }
      

    }

    private void xrTableCell12_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell12.Text == "")
        {
            xrTableCell12.Text = "-";

        }
        string latecount = string.Empty;

        string lateparam = string.Empty;
        try
        {
            latecount = this.GetCurrentColumnValue("LateCount").ToString();
            lateparam = this.GetCurrentColumnValue("LateParam").ToString();
            if (latecount != "")
            {
                if (int.Parse(latecount) > int.Parse(lateparam))
                {
                    xrTableCell12.BackColor = Color.Yellow;

                }
                else
                {
                    xrTableCell12.BackColor = Color.White;

                }

            }
        }
        catch
        {
        }
       
    }

    private void xrTableCell13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell13.Text == "")
        {
            xrTableCell13.Text = "-";

        }
        string latecount = string.Empty;

        string lateparam = string.Empty;
        try
        {
            latecount = this.GetCurrentColumnValue("LateCount").ToString();
            lateparam = this.GetCurrentColumnValue("LateParam").ToString();
            if (latecount != "")
            {
                if (int.Parse(latecount) > int.Parse(lateparam))
                {
                    xrTableCell13.BackColor = Color.Yellow;

                }
                else
                {
                    xrTableCell13.BackColor = Color.White;

                }

            }
        }
        catch
        {
        }
      

    }

    private void xrTableCell14_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell14.Text == "")
        {
            xrTableCell14.Text = "-";

        }
        string latecount = string.Empty;

        string lateparam = string.Empty;
        try
        {
            latecount = this.GetCurrentColumnValue("LateCount").ToString();
            lateparam = this.GetCurrentColumnValue("LateParam").ToString();
            if (latecount != "")
            {
                if (int.Parse(latecount) > int.Parse(lateparam))
                {
                    xrTableCell14.BackColor = Color.Yellow;

                }
                else
                {
                    xrTableCell14.BackColor = Color.White;

                }

            }
        }
        catch
        {
        }
        
    }

  

    

   
}
