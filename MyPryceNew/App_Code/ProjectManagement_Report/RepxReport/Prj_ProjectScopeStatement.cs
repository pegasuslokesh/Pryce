using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for Att_ShiftReport
/// </summary>
public class Prj_ProjectScopeStatement : DevExpress.XtraReports.UI.XtraReport
{
    //Att_AttendanceRegister objAttReg = null;
    //Set_Employee_Holiday objEmpHoli = null;
    SystemParameter objsys = null;
    //Prj_ProjectTask objProjectTask = null;
    //UserMaster objUser = null;
	private DevExpress.XtraReports.UI.DetailBand Detail;
	private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private PageFooterBand PageFooter;
  //  private AttendanceDataSet attendanceDataSet1;
   // private AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter sp_Att_ScheduleDescription_ReportTableAdapter1;
    private ReportHeaderBand ReportHeader;
    private XRPageInfo xrPageInfo2;
    private XRPageInfo xrPageInfo1;
   // private AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter sp_Att_AttendanceRegister_ReportTableAdapter1;
    private XRPanel xrPanel1;
    private XRLabel xrCompAddress;
    private XRLabel xrTitle;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrCompName;
    //private AttendanceDataSetTableAdapters.sp_Prj_ProjectHistory_ReportTableAdapter sp_Prj_ProjectHistory_ReportTableAdapter1;
    //  private AttendanceDataSetTableAdapters.sp_Prj_ProjectHistory_ReportTableAdapter sp_Prj_ProjectHistory_ReportTableAdapter2;
    private XRLabel xrLabel1;
    private XRTable xrTable5;
    private XRTableRow xrTableRow16;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell70;
    private XRTableRow xrTableRow17;
    private XRTableCell xrTableCell74;
    private XRTableCell xrTableCell75;
    private XRTableRow xrTableRow18;
    private XRTableCell xrTableCell76;
    private XRTableCell xrTableCell78;
    private XRTableRow xrTableRow19;
    private XRTableCell xrTableCell79;
    private XRTableCell xrTableCell80;
    private XRRichText xrRichText1;
    private XRTable xrTable7;
    private XRTableRow xrTableRow27;
    private XRTableCell xrTableCell98;
    private XRTableCell xrTableCell99;
    private XRTableCell xrTableCell100;
    private XRTableCell xrTableCell101;
    private XRTableCell xrTableCell102;
    private DevExpress.XtraReports.Parameters.Parameter Parent_Task_Id;
    private PageHeaderBand PageHeader;
    private XRLabel xrLabel3;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell15;
    private XRTableCell xrTableCell25;
    private AttendanceDataSet attendanceDataSet1;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private XRTableCell xrTableCell19;
    private ReportFooterBand ReportFooter;
    private XRLabel xrLabel11;
    private XRLabel xrLabel10;
    private XRLabel xrLabel9;
    private XRLabel xrLabel8;
    private XRLabel xrLabel7;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRTableCell xrTableCell2;
    private XRTable xrTable2;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell14;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell9;
    private XRTable xrTable6;
    private XRTableRow xrTableRow26;
    private XRTableCell xrTableCell93;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell94;
    private XRTableCell xrTableCell95;
    private XRTableCell xrTableCell96;
    private XRTableCell xrTableCell97;
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

    public Prj_ProjectScopeStatement(string strConString)
	{
		InitializeComponent();
        //objAttReg = new Att_AttendanceRegister(strConString);
        //objEmpHoli = new Set_Employee_Holiday(strConString);
        objsys = new SystemParameter(strConString);
        //objProjectTask = new Prj_ProjectTask(strConString);
        //objUser = new UserMaster(strConString);
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
        string resourceFileName = "Prj_ProjectScopeStatement.resx";
        System.Resources.ResourceManager resources = global::Resources.Prj_ProjectScopeStatement.ResourceManager;
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow27 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell98 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell99 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell100 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell101 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell102 = new DevExpress.XtraReports.UI.XRTableCell();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow16 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrCompAddress = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTitle = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrCompName = new DevExpress.XtraReports.UI.XRLabel();
        this.attendanceDataSet1 = new AttendanceDataSet();
        this.Parent_Task_Id = new DevExpress.XtraReports.Parameters.Parameter();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow26 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell94 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell96 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell97 = new DevExpress.XtraReports.UI.XRTableCell();
        this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.attendanceDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable7});
        this.Detail.HeightF = 23F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrTable7
        // 
        this.xrTable7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(7.000001F, 0F);
        this.xrTable7.Name = "xrTable7";
        this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow27});
        this.xrTable7.SizeF = new System.Drawing.SizeF(1125F, 23F);
        this.xrTable7.StylePriority.UseBorders = false;
        this.xrTable7.StylePriority.UseFont = false;
        // 
        // xrTableRow27
        // 
        this.xrTableRow27.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell98,
            this.xrTableCell2,
            this.xrTableCell99,
            this.xrTableCell100,
            this.xrTableCell101,
            this.xrTableCell102});
        this.xrTableRow27.Name = "xrTableRow27";
        this.xrTableRow27.Weight = 1D;
        // 
        // xrTableCell98
        // 
        this.xrTableCell98.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell98.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.Expr15")});
        this.xrTableCell98.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell98.Name = "xrTableCell98";
        this.xrTableCell98.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell98.StylePriority.UseBorders = false;
        this.xrTableCell98.StylePriority.UseFont = false;
        this.xrTableCell98.StylePriority.UsePadding = false;
        this.xrTableCell98.Weight = 3.9572273228766353D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.Task_Type")});
        this.xrTableCell2.Font = new System.Drawing.Font("Calibri", 9.75F);
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell2.StylePriority.UseBorders = false;
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UsePadding = false;
        this.xrTableCell2.Text = "xrTableCell2";
        this.xrTableCell2.Weight = 0.90424620012174772D;
        // 
        // xrTableCell99
        // 
        this.xrTableCell99.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell99.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.Assign_Date1")});
        this.xrTableCell99.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell99.Name = "xrTableCell99";
        this.xrTableCell99.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.xrTableCell99.StylePriority.UseBorders = false;
        this.xrTableCell99.StylePriority.UseFont = false;
        this.xrTableCell99.StylePriority.UsePadding = false;
        this.xrTableCell99.StylePriority.UseTextAlignment = false;
        this.xrTableCell99.Text = "Assign date";
        this.xrTableCell99.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell99.Weight = 1.1079881016874484D;
        this.xrTableCell99.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell99_BeforePrint);
        // 
        // xrTableCell100
        // 
        this.xrTableCell100.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell100.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.AssignTo")});
        this.xrTableCell100.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell100.Name = "xrTableCell100";
        this.xrTableCell100.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell100.StylePriority.UseBorders = false;
        this.xrTableCell100.StylePriority.UseFont = false;
        this.xrTableCell100.StylePriority.UsePadding = false;
        this.xrTableCell100.Text = "Support Team";
        this.xrTableCell100.Weight = 1.5350712073322281D;
        // 
        // xrTableCell101
        // 
        this.xrTableCell101.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell101.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.ClientTeam")});
        this.xrTableCell101.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell101.Name = "xrTableCell101";
        this.xrTableCell101.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell101.StylePriority.UseBorders = false;
        this.xrTableCell101.StylePriority.UseFont = false;
        this.xrTableCell101.StylePriority.UsePadding = false;
        this.xrTableCell101.Text = "Client\'s Team";
        this.xrTableCell101.Weight = 1.8628282302572072D;
        // 
        // xrTableCell102
        // 
        this.xrTableCell102.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell102.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.Address")});
        this.xrTableCell102.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell102.Name = "xrTableCell102";
        this.xrTableCell102.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell102.StylePriority.UseBorders = false;
        this.xrTableCell102.StylePriority.UseFont = false;
        this.xrTableCell102.StylePriority.UsePadding = false;
        this.xrTableCell102.Text = "Place";
        this.xrTableCell102.Weight = 1.7478699797189696D;
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
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrPageInfo1});
        this.PageFooter.HeightF = 23.95833F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo2.Font = new System.Drawing.Font("Calibri", 9.75F);
        this.xrPageInfo2.Format = "Page{0}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(660.7918F, 0F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(295.2082F, 23F);
        this.xrPageInfo2.StylePriority.UseBorders = false;
        this.xrPageInfo2.StylePriority.UseFont = false;
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrPageInfo1
        // 
        this.xrPageInfo1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo1.Font = new System.Drawing.Font("Calibri", 9.75F);
        this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0.9583156F);
        this.xrPageInfo1.Name = "xrPageInfo1";
        this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo1.SizeF = new System.Drawing.SizeF(344.7917F, 23F);
        this.xrPageInfo1.StylePriority.UseBorders = false;
        this.xrPageInfo1.StylePriority.UseFont = false;
        // 
        // ReportHeader
        // 
        this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xrLabel1,
            this.xrTable5,
            this.xrRichText1,
            this.xrPanel1});
        this.ReportHeader.HeightF = 418.4792F;
        this.ReportHeader.Name = "ReportHeader";
        this.ReportHeader.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ReportHeader_BeforePrint);
        // 
        // xrTable2
        // 
        this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(6.999874F, 366.5F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4,
            this.xrTableRow2});
        this.xrTable2.SizeF = new System.Drawing.SizeF(1125F, 50F);
        this.xrTable2.StylePriority.UseBorders = false;
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell14});
        this.xrTableRow4.Name = "xrTableRow4";
        this.xrTableRow4.Weight = 1D;
        // 
        // xrTableCell14
        // 
        this.xrTableCell14.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrTableCell14.Name = "xrTableCell14";
        this.xrTableCell14.StylePriority.UseFont = false;
        this.xrTableCell14.Text = " Main Activity";
        this.xrTableCell14.Weight = 9.4699987792968763D;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6,
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell9});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseBackColor = false;
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.Text = "Activity";
        this.xrTableCell6.Weight = 5.0858857125062888D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell7.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseBackColor = false;
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.Text = "Schedule date";
        this.xrTableCell7.Weight = 1.1237172285483281D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.StylePriority.UseBackColor = false;
        this.xrTableCell8.StylePriority.UseFont = false;
        this.xrTableCell8.Text = "Place";
        this.xrTableCell8.Weight = 2.4154811799814029D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell9.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.StylePriority.UseBackColor = false;
        this.xrTableCell9.StylePriority.UseFont = false;
        this.xrTableCell9.Text = "Status";
        this.xrTableCell9.Weight = 0.844914658260855D;
        // 
        // xrLabel1
        // 
        this.xrLabel1.BackColor = System.Drawing.Color.Empty;
        this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(10.50006F, 290.1875F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
        this.xrLabel1.StylePriority.UseBackColor = false;
        this.xrLabel1.StylePriority.UseBorders = false;
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UsePadding = false;
        this.xrLabel1.Text = "Overview";
        // 
        // xrTable5
        // 
        this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(6.999874F, 122.7709F);
        this.xrTable5.Name = "xrTable5";
        this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3,
            this.xrTableRow16,
            this.xrTableRow17,
            this.xrTableRow18,
            this.xrTableRow19});
        this.xrTable5.SizeF = new System.Drawing.SizeF(1125F, 134.7222F);
        this.xrTable5.StylePriority.UseFont = false;
        // 
        // xrTableRow3
        // 
        this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell19});
        this.xrTableRow3.Name = "xrTableRow3";
        this.xrTableRow3.Weight = 1D;
        // 
        // xrTableCell16
        // 
        this.xrTableCell16.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell16.StylePriority.UseFont = false;
        this.xrTableCell16.Text = "Customer Name";
        this.xrTableCell16.Weight = 1.690758385007274D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.Name")});
        this.xrTableCell17.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell17.StylePriority.UseFont = false;
        this.xrTableCell17.Text = "xrTableCell17";
        this.xrTableCell17.Weight = 2.1729491033708532D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell18.StylePriority.UseFont = false;
        this.xrTableCell18.Text = "Customer Representative";
        this.xrTableCell18.Weight = 1.5828202347804554D;
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.ContactPerson")});
        this.xrTableCell19.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell19.StylePriority.UseFont = false;
        this.xrTableCell19.Text = "xrTableCell19";
        this.xrTableCell19.Weight = 2.0981679256269126D;
        // 
        // xrTableRow16
        // 
        this.xrTableRow16.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12,
            this.xrTableCell70,
            this.xrTableCell10,
            this.xrTableCell22});
        this.xrTableRow16.Name = "xrTableRow16";
        this.xrTableRow16.Weight = 1D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell12.Name = "xrTableCell12";
        this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell12.StylePriority.UseFont = false;
        this.xrTableCell12.StylePriority.UsePadding = false;
        this.xrTableCell12.Text = "Project #";
        this.xrTableCell12.Weight = 1.690758385007274D;
        // 
        // xrTableCell70
        // 
        this.xrTableCell70.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.Expr7")});
        this.xrTableCell70.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell70.Name = "xrTableCell70";
        this.xrTableCell70.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell70.StylePriority.UseFont = false;
        this.xrTableCell70.StylePriority.UsePadding = false;
        this.xrTableCell70.Weight = 2.1729491033708532D;
        // 
        // xrTableCell10
        // 
        this.xrTableCell10.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.Text = "Project Start Date";
        this.xrTableCell10.Weight = 1.5828202347804554D;
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.Start_Date")});
        this.xrTableCell22.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.Text = "xrTableCell22";
        this.xrTableCell22.Weight = 2.0981679256269126D;
        this.xrTableCell22.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell22_BeforePrint);
        // 
        // xrTableRow17
        // 
        this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell74,
            this.xrTableCell75,
            this.xrTableCell11,
            this.xrTableCell23});
        this.xrTableRow17.Name = "xrTableRow17";
        this.xrTableRow17.Weight = 1D;
        // 
        // xrTableCell74
        // 
        this.xrTableCell74.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell74.Name = "xrTableCell74";
        this.xrTableCell74.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell74.StylePriority.UseFont = false;
        this.xrTableCell74.StylePriority.UsePadding = false;
        this.xrTableCell74.Text = "Project Name";
        this.xrTableCell74.Weight = 1.690758385007274D;
        // 
        // xrTableCell75
        // 
        this.xrTableCell75.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.Project_Name")});
        this.xrTableCell75.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell75.Name = "xrTableCell75";
        this.xrTableCell75.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell75.StylePriority.UseFont = false;
        this.xrTableCell75.StylePriority.UsePadding = false;
        this.xrTableCell75.Text = "xrTableCell75";
        this.xrTableCell75.Weight = 2.1729491033708532D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell11.StylePriority.UseFont = false;
        this.xrTableCell11.Text = "Expected End Date";
        this.xrTableCell11.Weight = 1.5828202347804554D;
        // 
        // xrTableCell23
        // 
        this.xrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.Exp_End_Date")});
        this.xrTableCell23.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell23.Name = "xrTableCell23";
        this.xrTableCell23.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell23.StylePriority.UseFont = false;
        this.xrTableCell23.Text = "xrTableCell23";
        this.xrTableCell23.Weight = 2.0981679256269126D;
        this.xrTableCell23.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell23_BeforePrint);
        // 
        // xrTableRow18
        // 
        this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell76,
            this.xrTableCell78,
            this.xrTableCell13,
            this.xrTableCell24});
        this.xrTableRow18.Name = "xrTableRow18";
        this.xrTableRow18.Weight = 1D;
        // 
        // xrTableCell76
        // 
        this.xrTableCell76.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell76.Name = "xrTableCell76";
        this.xrTableCell76.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell76.StylePriority.UseFont = false;
        this.xrTableCell76.StylePriority.UsePadding = false;
        this.xrTableCell76.Text = "Po Ref.";
        this.xrTableCell76.Weight = 1.690758385007274D;
        // 
        // xrTableCell78
        // 
        this.xrTableCell78.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.PoRef")});
        this.xrTableCell78.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell78.Name = "xrTableCell78";
        this.xrTableCell78.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell78.StylePriority.UseFont = false;
        this.xrTableCell78.StylePriority.UsePadding = false;
        this.xrTableCell78.Text = "xrTableCell78";
        this.xrTableCell78.Weight = 2.1729491033708532D;
        // 
        // xrTableCell13
        // 
        this.xrTableCell13.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell13.StylePriority.UseFont = false;
        this.xrTableCell13.Text = "Project Close date";
        this.xrTableCell13.Weight = 1.5828202347804554D;
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.End_Date")});
        this.xrTableCell24.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell24.StylePriority.UseFont = false;
        this.xrTableCell24.Text = "xrTableCell24";
        this.xrTableCell24.Weight = 2.0981679256269126D;
        this.xrTableCell24.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell24_BeforePrint);
        // 
        // xrTableRow19
        // 
        this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell79,
            this.xrTableCell80,
            this.xrTableCell15,
            this.xrTableCell25});
        this.xrTableRow19.Name = "xrTableRow19";
        this.xrTableRow19.Weight = 1D;
        // 
        // xrTableCell79
        // 
        this.xrTableCell79.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell79.Name = "xrTableCell79";
        this.xrTableCell79.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell79.StylePriority.UseFont = false;
        this.xrTableCell79.StylePriority.UsePadding = false;
        this.xrTableCell79.Text = "Invoice No. & Date";
        this.xrTableCell79.Weight = 1.690758385007274D;
        // 
        // xrTableCell80
        // 
        this.xrTableCell80.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.InvoiceRef")});
        this.xrTableCell80.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell80.Name = "xrTableCell80";
        this.xrTableCell80.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell80.StylePriority.UseFont = false;
        this.xrTableCell80.StylePriority.UsePadding = false;
        this.xrTableCell80.Text = "xrTableCell80";
        this.xrTableCell80.Weight = 2.1729491033708532D;
        // 
        // xrTableCell15
        // 
        this.xrTableCell15.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell15.Name = "xrTableCell15";
        this.xrTableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell15.StylePriority.UseFont = false;
        this.xrTableCell15.Text = "Project Manager/Team Lead";
        this.xrTableCell15.Weight = 1.5828202347804554D;
        // 
        // xrTableCell25
        // 
        this.xrTableCell25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.TeamLeader")});
        this.xrTableCell25.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrTableCell25.Name = "xrTableCell25";
        this.xrTableCell25.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
        this.xrTableCell25.StylePriority.UseFont = false;
        this.xrTableCell25.Text = "xrTableCell25";
        this.xrTableCell25.Weight = 2.0981679256269126D;
        // 
        // xrRichText1
        // 
        this.xrRichText1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrRichText1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Html", null, "sp_Prj_ProjectHistory_Report.Project_Description")});
        this.xrRichText1.Font = new System.Drawing.Font("Verdana", 8F);
        this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(6.999874F, 313.1875F);
        this.xrRichText1.Name = "xrRichText1";
        this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
        this.xrRichText1.SizeF = new System.Drawing.SizeF(1125F, 33.41669F);
        this.xrRichText1.StylePriority.UseBorders = false;
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrCompAddress,
            this.xrTitle,
            this.xrPictureBox1,
            this.xrCompName});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(7.000001F, 4.166667F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(1125F, 83.33334F);
        this.xrPanel1.StylePriority.UseBorders = false;
        // 
        // xrLabel3
        // 
        this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(1023.937F, 54.04169F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(97.06274F, 15.99998F);
        this.xrLabel3.StylePriority.UseBorders = false;
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "xrLabel3";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrCompAddress
        // 
        this.xrCompAddress.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrCompAddress.Font = new System.Drawing.Font("Calibri", 11F);
        this.xrCompAddress.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 28F);
        this.xrCompAddress.Name = "xrCompAddress";
        this.xrCompAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrCompAddress.SizeF = new System.Drawing.SizeF(856.5105F, 22.04167F);
        this.xrCompAddress.StylePriority.UseBorders = false;
        this.xrCompAddress.StylePriority.UseFont = false;
        this.xrCompAddress.Text = "xrCompAddress";
        // 
        // xrTitle
        // 
        this.xrTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTitle.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold);
        this.xrTitle.LocationFloat = new DevExpress.Utils.PointFloat(287.5F, 54.04167F);
        this.xrTitle.Name = "xrTitle";
        this.xrTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrTitle.SizeF = new System.Drawing.SizeF(571.0104F, 19.29165F);
        this.xrTitle.StylePriority.UseBorders = false;
        this.xrTitle.StylePriority.UseFont = false;
        this.xrTitle.StylePriority.UseTextAlignment = false;
        this.xrTitle.Text = "xrTitle";
        this.xrTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(1019.937F, 3.000005F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(97.06268F, 45.04167F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrCompName
        // 
        this.xrCompName.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrCompName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrCompName.LocationFloat = new DevExpress.Utils.PointFloat(1.999998F, 3.000005F);
        this.xrCompName.Name = "xrCompName";
        this.xrCompName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrCompName.SizeF = new System.Drawing.SizeF(856.5104F, 23F);
        this.xrCompName.StylePriority.UseBorders = false;
        this.xrCompName.StylePriority.UseFont = false;
        this.xrCompName.Text = "xrCompName";
        // 
        // attendanceDataSet1
        // 
        this.attendanceDataSet1.DataSetName = "AttendanceDataSet";
        this.attendanceDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // Parent_Task_Id
        // 
        this.Parent_Task_Id.Description = "Parameter1";
        this.Parent_Task_Id.Name = "Parent_Task_Id";
        this.Parent_Task_Id.Type = typeof(int);
        this.Parent_Task_Id.ValueInfo = "0";
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
        this.PageHeader.HeightF = 32.70832F;
        this.PageHeader.Name = "PageHeader";
        // 
        // xrTable6
        // 
        this.xrTable6.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(6.999874F, 10.49999F);
        this.xrTable6.Name = "xrTable6";
        this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow26});
        this.xrTable6.SizeF = new System.Drawing.SizeF(1125F, 22.20834F);
        this.xrTable6.StylePriority.UseBorders = false;
        // 
        // xrTableRow26
        // 
        this.xrTableRow26.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell93,
            this.xrTableCell1,
            this.xrTableCell94,
            this.xrTableCell95,
            this.xrTableCell96,
            this.xrTableCell97});
        this.xrTableRow26.Name = "xrTableRow26";
        this.xrTableRow26.Weight = 1D;
        // 
        // xrTableCell93
        // 
        this.xrTableCell93.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell93.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell93.BorderWidth = 1F;
        this.xrTableCell93.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell93.Name = "xrTableCell93";
        this.xrTableCell93.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
        this.xrTableCell93.StylePriority.UseBackColor = false;
        this.xrTableCell93.StylePriority.UseBorders = false;
        this.xrTableCell93.StylePriority.UseBorderWidth = false;
        this.xrTableCell93.StylePriority.UseFont = false;
        this.xrTableCell93.StylePriority.UsePadding = false;
        this.xrTableCell93.StylePriority.UseTextAlignment = false;
        this.xrTableCell93.Text = "Activity";
        this.xrTableCell93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell93.Weight = 3.9904207648367471D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell1.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "Task Nature";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell1.Weight = 0.91183023237012162D;
        // 
        // xrTableCell94
        // 
        this.xrTableCell94.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell94.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell94.BorderWidth = 1F;
        this.xrTableCell94.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell94.Multiline = true;
        this.xrTableCell94.Name = "xrTableCell94";
        this.xrTableCell94.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell94.StylePriority.UseBackColor = false;
        this.xrTableCell94.StylePriority.UseBorders = false;
        this.xrTableCell94.StylePriority.UseBorderWidth = false;
        this.xrTableCell94.StylePriority.UseFont = false;
        this.xrTableCell94.StylePriority.UsePadding = false;
        this.xrTableCell94.StylePriority.UseTextAlignment = false;
        this.xrTableCell94.Text = "Schedule date";
        this.xrTableCell94.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell94.Weight = 1.1172815377922674D;
        // 
        // xrTableCell95
        // 
        this.xrTableCell95.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell95.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell95.BorderWidth = 1F;
        this.xrTableCell95.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell95.Name = "xrTableCell95";
        this.xrTableCell95.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell95.StylePriority.UseBackColor = false;
        this.xrTableCell95.StylePriority.UseBorders = false;
        this.xrTableCell95.StylePriority.UseBorderWidth = false;
        this.xrTableCell95.StylePriority.UseFont = false;
        this.xrTableCell95.StylePriority.UsePadding = false;
        this.xrTableCell95.StylePriority.UseTextAlignment = false;
        this.xrTableCell95.Text = "Support Team";
        this.xrTableCell95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell95.Weight = 1.5479467697997835D;
        // 
        // xrTableCell96
        // 
        this.xrTableCell96.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell96.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell96.BorderWidth = 1F;
        this.xrTableCell96.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell96.Multiline = true;
        this.xrTableCell96.Name = "xrTableCell96";
        this.xrTableCell96.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell96.StylePriority.UseBackColor = false;
        this.xrTableCell96.StylePriority.UseBorders = false;
        this.xrTableCell96.StylePriority.UseBorderWidth = false;
        this.xrTableCell96.StylePriority.UseFont = false;
        this.xrTableCell96.StylePriority.UsePadding = false;
        this.xrTableCell96.StylePriority.UseTextAlignment = false;
        this.xrTableCell96.Text = "Customer\'s  Representative";
        this.xrTableCell96.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell96.Weight = 1.878452912763928D;
        // 
        // xrTableCell97
        // 
        this.xrTableCell97.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell97.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell97.BorderWidth = 1F;
        this.xrTableCell97.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
        this.xrTableCell97.Name = "xrTableCell97";
        this.xrTableCell97.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell97.StylePriority.UseBackColor = false;
        this.xrTableCell97.StylePriority.UseBorders = false;
        this.xrTableCell97.StylePriority.UseBorderWidth = false;
        this.xrTableCell97.StylePriority.UseFont = false;
        this.xrTableCell97.StylePriority.UsePadding = false;
        this.xrTableCell97.StylePriority.UseTextAlignment = false;
        this.xrTableCell97.Text = "Place";
        this.xrTableCell97.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        this.xrTableCell97.Weight = 1.7625307820632949D;
        // 
        // ReportFooter
        // 
        this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4});
        this.ReportFooter.HeightF = 140.625F;
        this.ReportFooter.Name = "ReportFooter";
        this.ReportFooter.PrintAtBottom = true;
        // 
        // xrLabel11
        // 
        this.xrLabel11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.TeamLeader")});
        this.xrLabel11.Font = new System.Drawing.Font("Calibri", 12F);
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(583.1227F, 103.1667F);
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(548.8773F, 23.00001F);
        this.xrLabel11.StylePriority.UseBorders = false;
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.StylePriority.UseTextAlignment = false;
        this.xrLabel11.Text = "Signature";
        this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel10
        // 
        this.xrLabel10.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Prj_ProjectHistory_Report.ContactPerson")});
        this.xrLabel10.Font = new System.Drawing.Font("Calibri", 12F);
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(10.50007F, 103.1667F);
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(572.6226F, 23.00001F);
        this.xrLabel10.StylePriority.UseBorders = false;
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.StylePriority.UseTextAlignment = false;
        this.xrLabel10.Text = "Signature";
        this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel9
        // 
        this.xrLabel9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel9.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(583.1227F, 80.16663F);
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(548.8773F, 23.00001F);
        this.xrLabel9.StylePriority.UseBorders = false;
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.StylePriority.UseTextAlignment = false;
        this.xrLabel9.Text = "Signature";
        this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel8
        // 
        this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.Left;
        this.xrLabel8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(10.50007F, 80.16663F);
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(572.6226F, 23.00001F);
        this.xrLabel8.StylePriority.UseBorders = false;
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.StylePriority.UseTextAlignment = false;
        this.xrLabel8.Text = "Signature";
        this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel7
        // 
        this.xrLabel7.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(583.1226F, 34.25F);
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(548.8773F, 45.91669F);
        this.xrLabel7.StylePriority.UseBorders = false;
        // 
        // xrLabel6
        // 
        this.xrLabel6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(10.50007F, 34.25F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(572.6226F, 45.91668F);
        this.xrLabel6.StylePriority.UseBorders = false;
        // 
        // xrLabel5
        // 
        this.xrLabel5.BackColor = System.Drawing.Color.Empty;
        this.xrLabel5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)));
        this.xrLabel5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(583.1226F, 11.24999F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(548.8774F, 23F);
        this.xrLabel5.StylePriority.UseBackColor = false;
        this.xrLabel5.StylePriority.UseBorders = false;
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.Text = "Project Manager/Team lead";
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel4
        // 
        this.xrLabel4.BackColor = System.Drawing.Color.Empty;
        this.xrLabel4.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
        this.xrLabel4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(10.50007F, 11.24999F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(572.6226F, 23F);
        this.xrLabel4.StylePriority.UseBackColor = false;
        this.xrLabel4.StylePriority.UseBorders = false;
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.Text = "Customer Representative";
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // Prj_ProjectScopeStatement
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.ReportHeader,
            this.PageHeader,
            this.ReportFooter});
        this.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.DataMember = "sp_Prj_ProjectHistory_Report";
        this.DataSource = this.attendanceDataSet1;
        this.Font = new System.Drawing.Font("Times New Roman", 11F);
        this.Landscape = true;
        this.Margins = new System.Drawing.Printing.Margins(17, 15, 0, 0);
        this.PageHeight = 827;
        this.PageWidth = 1169;
        this.PaperKind = System.Drawing.Printing.PaperKind.A4;
        this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.Parent_Task_Id});
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.attendanceDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

	}

	#endregion

    double RowNo = 0;
    string strProjectid = string.Empty;
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
    public void setreportCode(string strreportcode)
    {
        xrLabel3.Text = strreportcode;
    }
    public void setcompanyname(string companyname)
    {
        xrCompName.Text = companyname;
    }

    private void xrTableCell86_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
       
    }

    private void xrTableCell88_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //if (xrTableCell88.Text != "")
        //{
        //    xrTableCell88.Text = Convert.ToDateTime(xrTableCell88.Text).ToString(objsys.SetDateFormat());

        //}
    }

    private void xrTableCell90_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
       
    }

    private void xrTableCell99_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell99.Text != "")
        {
            xrTableCell99.Text = Convert.ToDateTime(xrTableCell99.Text).ToString(objsys.SetDateFormat());

        }
    }

    private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //if (xrTableCell2.Text != "")
        //{
        //    xrTableCell2.Text = Convert.ToDateTime(xrTableCell2.Text).ToString(objsys.SetDateFormat());

        //}
    }

    private void GroupHeader3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //Parameters["Parent_Task_Id"].Value = GetCurrentColumnValue("Task_Id");


        //DataTable dt = objProjectTask.GetDataProjectId(GetCurrentColumnValue("Project_Id").ToString());


        //dt = new DataView(dt, "ParentTaskId=" + GetCurrentColumnValue("Task_Id") + "", "", DataViewRowState.CurrentRows).ToTable();

        //if (dt == null || dt.Rows.Count == 0)
        //{
        //    DetailReport.Visible = false;
        //    GroupFooter1.Visible = false;
        //}
        //else
        //{
        //    DetailReport.Visible = true;
        //    GroupFooter1.Visible = true;
        //}

        //dt.Dispose();
    }

    private void DetailReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }

    private void xrTableCell11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //if (xrTableCell11.Text != "")
        //{
        //    xrTableCell11.Text = Convert.ToDateTime(xrTableCell11.Text).ToString(objsys.SetDateFormat());

        //}
    }

    private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
       
    }

    private void xrTableCell22_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell22.Text != "")
        {
            xrTableCell22.Text = Convert.ToDateTime(xrTableCell22.Text).ToString(objsys.SetDateFormat());

        }
    }

    private void xrTableCell23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell23.Text != "")
        {
            xrTableCell23.Text = Convert.ToDateTime(xrTableCell23.Text).ToString(objsys.SetDateFormat());

        }
    }

    private void xrTableCell24_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell24.Text != "")
        {
            if (Convert.ToDateTime(xrTableCell24.Text).ToString(objsys.SetDateFormat()) != "01-Jan-1990")
            {
                xrTableCell24.Text = Convert.ToDateTime(xrTableCell24.Text).ToString(objsys.SetDateFormat());

            }
            else
            {
                xrTableCell24.Text = "";
            }
        }
        else
        {
            xrTableCell24.Text = "";
        }
    }

    private void xrTableCell20_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {


        //RowNo = RowNo+1;
        //xrTableCell20.Text = RowNo.ToString();
    }

    private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        RowNo = 0;
    }

    private void ReportHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        xrTable2.Rows.Clear();


        if (System.Web.HttpContext.Current.Session["DtprojectTask"] != null)
        {
            DataTable dt = (DataTable)System.Web.HttpContext.Current.Session["DtprojectTask"];


            if (dt.Rows.Count > 0)
            {



                XRTableRow drCategoryheader = new XRTableRow();
                xrTable2.Rows.Add(drCategoryheader);
                XRTableCell xrCategoryCellheader = new XRTableCell();

                xrCategoryCellheader.Text = " Main Activity";
                xrCategoryCellheader.Font = new Font("Calibri", 12, FontStyle.Bold);
                xrCategoryCellheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {


                    xrCategoryCellheader.Borders = DevExpress.XtraPrinting.BorderSide.All;


                }
                catch
                {
                }
                drCategoryheader.Cells.Add(xrCategoryCellheader);
                xrTable2.Rows[drCategoryheader.Index].Cells[0].SizeF = new SizeF(1125F, 25F);


                ///header row
                ///
                XRTableRow dritemheader = new XRTableRow();
                xrTable2.Rows.Add(dritemheader);
                XRTableCell xrActivityheader = new XRTableCell();
                XRTableCell xrScheduledateheader = new XRTableCell();
                XRTableCell xrPlaceheader = new XRTableCell();
                XRTableCell xrStatusheader = new XRTableCell();



                xrActivityheader.Text = " Activity";

                xrActivityheader.Font = new Font("Calibri", 11, FontStyle.Bold);
                xrScheduledateheader.Font = new Font("Calibri", 11, FontStyle.Bold);
                xrPlaceheader.Font = new Font("Calibri", 11, FontStyle.Bold);
                xrStatusheader.Font = new Font("Calibri", 11, FontStyle.Bold);


                xrActivityheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {


                    xrActivityheader.Borders = DevExpress.XtraPrinting.BorderSide.All;


                }
                catch
                {
                }

                xrActivityheader.BackColor = System.Drawing.Color.Empty;


                xrScheduledateheader.Text = " Schedule date";

                xrScheduledateheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    xrScheduledateheader.Borders = DevExpress.XtraPrinting.BorderSide.All;

                }
                catch
                {
                }

                xrScheduledateheader.BackColor = System.Drawing.Color.Empty;




                xrPlaceheader.Text = " Place";

                xrPlaceheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {

                    xrPlaceheader.Borders = DevExpress.XtraPrinting.BorderSide.All;

                }
                catch
                {
                }
                xrPlaceheader.BackColor = System.Drawing.Color.Empty;


                xrStatusheader.Text = " Status";

                xrStatusheader.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {

                    xrStatusheader.Borders = DevExpress.XtraPrinting.BorderSide.All;

                }
                catch
                {
                }

                //for silver color 
                // xrStatusheader.BackColor = System.Drawing.ColorTranslator.FromHtml("#C0C0C0");

                xrStatusheader.BackColor = System.Drawing.Color.Empty;






                dritemheader.Cells.Add(xrActivityheader);
                dritemheader.Cells.Add(xrScheduledateheader);
                dritemheader.Cells.Add(xrPlaceheader);
                dritemheader.Cells.Add(xrStatusheader);

                xrTable2.Rows[dritemheader.Index].Cells[0].SizeF = new SizeF(604.18F, 25F);
                xrTable2.Rows[dritemheader.Index].Cells[1].SizeF = new SizeF(133.49F, 25F);
                xrTable2.Rows[dritemheader.Index].Cells[2].SizeF = new SizeF(286.95F, 25F);
                xrTable2.Rows[dritemheader.Index].Cells[3].SizeF = new SizeF(100.37F, 25F);


                //   xrTable2.Rows[dritemheader.Index].Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                //xrTable2.Rows[dritemheader.Index].Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;



                for (int i = 0; i < dt.Rows.Count; i++)
                {




                    XRTableRow dr = new XRTableRow();
                    xrTable2.Rows.Add(dr);
                    XRTableCell Activity = new XRTableCell();
                    XRTableCell Scheduledate = new XRTableCell();
                    XRTableCell place = new XRTableCell();
                    XRTableCell Status = new XRTableCell();

                    Activity.Font = new Font("Calibri", 11, FontStyle.Regular);
                    Scheduledate.Font = new Font("Calibri", 11, FontStyle.Regular);
                    place.Font = new Font("Calibri", 11, FontStyle.Regular);
                    Status.Font = new Font("Calibri", 11, FontStyle.Regular);


                    Activity.Text = " " + dt.Rows[i]["Subject"].ToString();
                    Activity.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    try
                    {
                        Activity.Borders = DevExpress.XtraPrinting.BorderSide.All;
                    }
                    catch
                    {
                    }

                    Scheduledate.Text = " " + Convert.ToDateTime(dt.Rows[i]["Assign_date"].ToString()).ToString(objsys.SetDateFormat());
                    Scheduledate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    try
                    {

                        Scheduledate.Borders = DevExpress.XtraPrinting.BorderSide.All;


                    }
                    catch
                    {
                    }

                    place.Text = " " + dt.Rows[i]["Address"].ToString();
                    place.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    try
                    {
                        place.Borders = DevExpress.XtraPrinting.BorderSide.All;



                    }
                    catch
                    {
                    }
                    Status.Text = " " + dt.Rows[i]["Status"].ToString();


                    Status.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                    try
                    {
                        Status.Borders = DevExpress.XtraPrinting.BorderSide.All;

                    }
                    catch
                    {
                    }

                    // FCExpAmount.GetEffectiveTextAlignment() = DevExpress.XtraPrinting.TextAlignment.MiddleRight;




                    dr.Cells.Add(Activity);
                    dr.Cells.Add(Scheduledate);
                    dr.Cells.Add(place);
                    dr.Cells.Add(Status);
                    xrTable2.Rows[dr.Index].Cells[0].SizeF = new SizeF(604.18F, 25F);
                    xrTable2.Rows[dr.Index].Cells[1].SizeF = new SizeF(133.49F, 25F);
                    xrTable2.Rows[dr.Index].Cells[2].SizeF = new SizeF(286.95F, 25F);
                    xrTable2.Rows[dr.Index].Cells[3].SizeF = new SizeF(100.37F, 25F);


                    // xrTable5.Rows[dr.Index].Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                    //  xrTable5.Rows[dr.Index].Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

                    //xrTableCell12.Text = dt.Rows[i]["Currency_Name"].ToString();
                    //xrTable1.Rows[dr.Index].Cells[0].Text = dt.Rows[i]["Currency_Name"].ToString();
                }



            }

        }
    }
}
