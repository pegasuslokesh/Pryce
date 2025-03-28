using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Web;
using PegasusDataAccess;

/// <summary>
/// Summary description for Att_ShiftReport
/// </summary>
public class Att_InOutReport_1_Direct : DevExpress.XtraReports.UI.XtraReport
{
    DataAccessClass Objda = null;
    Set_ApplicationParameter objAppParam = null;
    SystemParameter objsys = null;
    Set_Employee_Holiday objEmpHoli = null;
    Att_AttendanceRegister objAttReg = null;
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private PageFooterBand PageFooter;
    private AttendanceDataSet attendanceDataSet1;
    private AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter sp_Att_ScheduleDescription_ReportTableAdapter1;
    private ReportHeaderBand ReportHeader;
    private XRPageInfo xrPageInfo2;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell30;
    private AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter sp_Att_AttendanceRegister_ReportTableAdapter1;
    private XRLabel xrLabel9;
    private XRLabel xrLabel10;
    private XRPageInfo xrPageInfo3;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell22;
    private PageHeaderBand PageHeader;
    private XRTableCell xrTableCell53;
    private XRPanel xrPanel1;
    private XRTable xrTable4;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRLabel xrTitle;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrCompName;
    private XRLabel xrLabel1;
    private XRLabel xrLabel2;
    private XRPanel xrPanel3;
    private XRPanel xrPanel4;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private XRLabel xrLabel4;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell28;
    private XRLabel xrLabel3;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public Att_InOutReport_1_Direct(string strConString)
    {

        InitializeComponent();
        Objda = new DataAccessClass(strConString);
        objAttReg = new Att_AttendanceRegister(strConString);
        objAppParam = new Set_ApplicationParameter(strConString);
        objsys = new SystemParameter(strConString);
        objEmpHoli = new Set_Employee_Holiday(strConString);
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

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string resourceFileName = "Att_InOutReport_1_Direct.resx";
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.attendanceDataSet1 = new AttendanceDataSet();
        this.sp_Att_ScheduleDescription_ReportTableAdapter1 = new AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter();
        this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTitle = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrCompName = new DevExpress.XtraReports.UI.XRLabel();
        this.sp_Att_AttendanceRegister_ReportTableAdapter1 = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.attendanceDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xrPanel3,
            this.xrPanel4});
        this.Detail.HeightF = 13.50002F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
        // 
        // xrTable2
        // 
        this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable2.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(4.999733F, 0F);
        this.xrTable2.Name = "xrTable2";
        this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
        this.xrTable2.SizeF = new System.Drawing.SizeF(1155.542F, 13F);
        this.xrTable2.StylePriority.UseBorders = false;
        this.xrTable2.StylePriority.UseFont = false;
        // 
        // xrTableRow2
        // 
        this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell20,
            this.xrTableCell29,
            this.xrTableCell28,
            this.xrTableCell30,
            this.xrTableCell19,
            this.xrTableCell5,
            this.xrTableCell9});
        this.xrTableRow2.Name = "xrTableRow2";
        this.xrTableRow2.Weight = 1D;
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell20.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell20.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrTableCell20.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_Report.Emp_Code")});
        this.xrTableCell20.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell20.Multiline = true;
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell20.StylePriority.UseBackColor = false;
        this.xrTableCell20.StylePriority.UseBorderColor = false;
        this.xrTableCell20.StylePriority.UseBorderDashStyle = false;
        this.xrTableCell20.StylePriority.UseBorders = false;
        this.xrTableCell20.StylePriority.UseFont = false;
        this.xrTableCell20.StylePriority.UsePadding = false;
        this.xrTableCell20.StylePriority.UseTextAlignment = false;
        this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell20.Weight = 0.70352608739721134D;
        // 
        // xrTableCell29
        // 
        this.xrTableCell29.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell29.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell29.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrTableCell29.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_Report.Emp_Name")});
        this.xrTableCell29.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell29.Multiline = true;
        this.xrTableCell29.Name = "xrTableCell29";
        this.xrTableCell29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell29.StylePriority.UseBackColor = false;
        this.xrTableCell29.StylePriority.UseBorderColor = false;
        this.xrTableCell29.StylePriority.UseBorderDashStyle = false;
        this.xrTableCell29.StylePriority.UseBorders = false;
        this.xrTableCell29.StylePriority.UseFont = false;
        this.xrTableCell29.StylePriority.UsePadding = false;
        this.xrTableCell29.StylePriority.UseTextAlignment = false;
        this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell29.Weight = 1.1608565344330437D;
        // 
        // xrTableCell28
        // 
        this.xrTableCell28.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell28.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell28.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrTableCell28.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_Report.Dep_Name")});
        this.xrTableCell28.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell28.Multiline = true;
        this.xrTableCell28.Name = "xrTableCell28";
        this.xrTableCell28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell28.StylePriority.UseBackColor = false;
        this.xrTableCell28.StylePriority.UseBorderColor = false;
        this.xrTableCell28.StylePriority.UseBorderDashStyle = false;
        this.xrTableCell28.StylePriority.UseBorders = false;
        this.xrTableCell28.StylePriority.UseFont = false;
        this.xrTableCell28.StylePriority.UsePadding = false;
        this.xrTableCell28.StylePriority.UseTextAlignment = false;
        this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell28.Weight = 0.65660547179107243D;
        // 
        // xrTableCell30
        // 
        this.xrTableCell30.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell30.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell30.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrTableCell30.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_Report.Att_Date")});
        this.xrTableCell30.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell30.Name = "xrTableCell30";
        this.xrTableCell30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
        this.xrTableCell30.StylePriority.UseBackColor = false;
        this.xrTableCell30.StylePriority.UseBorderColor = false;
        this.xrTableCell30.StylePriority.UseBorderDashStyle = false;
        this.xrTableCell30.StylePriority.UseBorders = false;
        this.xrTableCell30.StylePriority.UseFont = false;
        this.xrTableCell30.StylePriority.UsePadding = false;
        this.xrTableCell30.StylePriority.UseTextAlignment = false;
        this.xrTableCell30.Text = "xrTableCell30";
        this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell30.Weight = 0.63239141005381239D;
        this.xrTableCell30.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell30_PrintOnPage);
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell19.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell19.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrTableCell19.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_Report.In_Time")});
        this.xrTableCell19.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.StylePriority.UseBackColor = false;
        this.xrTableCell19.StylePriority.UseBorderColor = false;
        this.xrTableCell19.StylePriority.UseBorderDashStyle = false;
        this.xrTableCell19.StylePriority.UseBorders = false;
        this.xrTableCell19.StylePriority.UseTextAlignment = false;
        this.xrTableCell19.Text = "xrTableCell19";
        this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell19.Weight = 0.70034149859730488D;
        this.xrTableCell19.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell19_PrintOnPage);
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell5.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell5.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_Report.Out_Time")});
        this.xrTableCell5.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.StylePriority.UseBackColor = false;
        this.xrTableCell5.StylePriority.UseBorderColor = false;
        this.xrTableCell5.StylePriority.UseBorderDashStyle = false;
        this.xrTableCell5.StylePriority.UseBorders = false;
        this.xrTableCell5.StylePriority.UseTextAlignment = false;
        this.xrTableCell5.Text = "xrTableCell5";
        this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell5.Weight = 0.630698131149194D;
        this.xrTableCell5.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell5_PrintOnPage);
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell9.BorderColor = System.Drawing.Color.Silver;
        this.xrTableCell9.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
        this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_Report.EffectiveWork_Min_hhmm")});
        this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 8F);
        this.xrTableCell9.Name = "xrTableCell9";
        this.xrTableCell9.StylePriority.UseBackColor = false;
        this.xrTableCell9.StylePriority.UseBorderColor = false;
        this.xrTableCell9.StylePriority.UseBorderDashStyle = false;
        this.xrTableCell9.StylePriority.UseTextAlignment = false;
        this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell9.Weight = 1.3300133854826552D;
        this.xrTableCell9.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTableCell9_PrintOnPage);
        // 
        // xrPanel3
        // 
        this.xrPanel3.Borders = DevExpress.XtraPrinting.BorderSide.Left;
        this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(5F, 0F);
        this.xrPanel3.Name = "xrPanel3";
        this.xrPanel3.SizeF = new System.Drawing.SizeF(2.083343F, 13F);
        this.xrPanel3.StylePriority.UseBorders = false;
        // 
        // xrPanel4
        // 
        this.xrPanel4.Borders = DevExpress.XtraPrinting.BorderSide.Right;
        this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(1158.375F, 0F);
        this.xrPanel4.Name = "xrPanel4";
        this.xrPanel4.SizeF = new System.Drawing.SizeF(2.166748F, 13F);
        this.xrPanel4.StylePriority.UseBorders = false;
        // 
        // xrTable1
        // 
        this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(4.999733F, 119.2917F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
        this.xrTable1.SizeF = new System.Drawing.SizeF(1155.542F, 13F);
        this.xrTable1.StylePriority.UseBorders = false;
        this.xrTable1.StylePriority.UseFont = false;
        // 
        // xrTableRow1
        // 
        this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell6,
            this.xrTableCell2,
            this.xrTableCell4,
            this.xrTableCell1,
            this.xrTableCell22,
            this.xrTableCell53});
        this.xrTableRow1.Name = "xrTableRow1";
        this.xrTableRow1.Weight = 1D;
        // 
        // xrTableCell7
        // 
        this.xrTableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseBorders = false;
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.StylePriority.UseTextAlignment = false;
        this.xrTableCell7.Text = "Employee ID";
        this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell7.Weight = 0.48393832501721756D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell6.Name = "xrTableCell6";
        this.xrTableCell6.StylePriority.UseBorders = false;
        this.xrTableCell6.StylePriority.UseFont = false;
        this.xrTableCell6.StylePriority.UseTextAlignment = false;
        this.xrTableCell6.Text = "Employee Name";
        this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell6.Weight = 0.79852467865243681D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.StylePriority.UseBorders = false;
        this.xrTableCell2.StylePriority.UseFont = false;
        this.xrTableCell2.StylePriority.UseTextAlignment = false;
        this.xrTableCell2.Text = "Department";
        this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell2.Weight = 0.45166310381171343D;
        // 
        // xrTableCell4
        // 
        this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.StylePriority.UseTextAlignment = false;
        this.xrTableCell4.Text = "Date";
        this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell4.Weight = 0.43500625771177864D;
        // 
        // xrTableCell1
        // 
        this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseBorders = false;
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.StylePriority.UseTextAlignment = false;
        this.xrTableCell1.Text = "In Time";
        this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell1.Weight = 0.48174808424170623D;
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell22.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.StylePriority.UseBorders = false;
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.StylePriority.UseTextAlignment = false;
        this.xrTableCell22.Text = "Out Time";
        this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell22.Weight = 0.4338415647431102D;
        // 
        // xrTableCell53
        // 
        this.xrTableCell53.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        | DevExpress.XtraPrinting.BorderSide.Right)
        | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell53.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell53.Name = "xrTableCell53";
        this.xrTableCell53.StylePriority.UseBorders = false;
        this.xrTableCell53.StylePriority.UseFont = false;
        this.xrTableCell53.StylePriority.UseTextAlignment = false;
        this.xrTableCell53.Text = "Work Time";
        this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        this.xrTableCell53.Weight = 0.91488341356571612D;
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
        this.BottomMargin.HeightF = 8.332984F;
        this.BottomMargin.Name = "BottomMargin";
        this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel1,
            this.xrLabel5,
            this.xrLabel6,
            this.xrLabel2,
            this.xrPageInfo2,
            this.xrLabel9,
            this.xrLabel10,
            this.xrPageInfo3,
            this.xrLabel3});
        this.PageFooter.HeightF = 83.91676F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrLabel4
        // 
        this.xrLabel4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(902.6102F, 6.833267F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(229.0637F, 4.166667F);
        this.xrLabel4.StylePriority.UseBorderDashStyle = false;
        this.xrLabel4.StylePriority.UseBorders = false;
        this.xrLabel4.StylePriority.UseTextAlignment = false;
        this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel1
        // 
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(6.965677F, 22.50001F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(271.5903F, 18.62498F);
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "HR Signature";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel5
        // 
        this.xrLabel5.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(473.5153F, 6.833267F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(229.0637F, 4.166667F);
        this.xrLabel5.StylePriority.UseBorderDashStyle = false;
        this.xrLabel5.StylePriority.UseBorders = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel6
        // 
        this.xrLabel6.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
        this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(44.73967F, 6.833267F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(229.0637F, 4.166667F);
        this.xrLabel6.StylePriority.UseBorderDashStyle = false;
        this.xrLabel6.StylePriority.UseBorders = false;
        this.xrLabel6.StylePriority.UseTextAlignment = false;
        this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        // 
        // xrLabel2
        // 
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(435.3474F, 22.50001F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(330.5291F, 18.62498F);
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "Supervisor/TL Signature";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(974.1915F, 68.20844F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(175.3502F, 15.70832F);
        this.xrPageInfo2.StylePriority.UseFont = false;
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
        this.xrPageInfo2.TextFormatString = "Page{0}Of {1}";
        // 
        // xrLabel9
        // 
        this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 68.20837F);
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(67.46136F, 14.41668F);
        this.xrLabel9.StylePriority.UseBorders = false;
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.StylePriority.UseTextAlignment = false;
        this.xrLabel9.Text = "Printed By:";
        this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrLabel10
        // 
        this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(67.46143F, 66.91672F);
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(267.8177F, 15.70834F);
        this.xrLabel10.StylePriority.UseBorders = false;
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.StylePriority.UseTextAlignment = false;
        this.xrLabel10.Text = "xrLabel10";
        this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        // 
        // xrPageInfo3
        // 
        this.xrPageInfo3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo3.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(499.1587F, 66.91672F);
        this.xrPageInfo3.Name = "xrPageInfo3";
        this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo3.SizeF = new System.Drawing.SizeF(199.6612F, 15.70834F);
        this.xrPageInfo3.StylePriority.UseBorders = false;
        this.xrPageInfo3.StylePriority.UseFont = false;
        this.xrPageInfo3.StylePriority.UseTextAlignment = false;
        this.xrPageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
        this.xrPageInfo3.TextFormatString = "{0:dddd, MMMM dd, yyyy h:mm tt}";
        // 
        // xrLabel3
        // 
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(819.0126F, 22.50001F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(330.5291F, 18.62498F);
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "Employee Signature";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // attendanceDataSet1
        // 
        this.attendanceDataSet1.DataSetName = "AttendanceDataSet";
        this.attendanceDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // sp_Att_ScheduleDescription_ReportTableAdapter1
        // 
        this.sp_Att_ScheduleDescription_ReportTableAdapter1.ClearBeforeFill = true;
        // 
        // ReportHeader
        // 
        this.ReportHeader.HeightF = 0F;
        this.ReportHeader.Name = "ReportHeader";
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrTitle,
            this.xrPictureBox1,
            this.xrCompName});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 0F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(1146F, 97.33332F);
        this.xrPanel1.StylePriority.UseBorders = false;
        // 
        // xrTable4
        // 
        this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(4.04168F, 37.66666F);
        this.xrTable4.Name = "xrTable4";
        this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
        this.xrTable4.SizeF = new System.Drawing.SizeF(844.7669F, 19.08337F);
        // 
        // xrTableRow4
        // 
        this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell31,
            this.xrTableCell38,
            this.xrTableCell39,
            this.xrTableCell40,
            this.xrTableCell41,
            this.xrTableCell42});
        this.xrTableRow4.Name = "xrTableRow4";
        this.xrTableRow4.Weight = 1D;
        // 
        // xrTableCell31
        // 
        this.xrTableCell31.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell31.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell31.Name = "xrTableCell31";
        this.xrTableCell31.StylePriority.UseBorders = false;
        this.xrTableCell31.StylePriority.UseFont = false;
        this.xrTableCell31.Weight = 1.4553366828930381D;
        // 
        // xrTableCell38
        // 
        this.xrTableCell38.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell38.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell38.Name = "xrTableCell38";
        this.xrTableCell38.StylePriority.UseBorders = false;
        this.xrTableCell38.StylePriority.UseFont = false;
        this.xrTableCell38.Weight = 1.4372902022334819D;
        // 
        // xrTableCell39
        // 
        this.xrTableCell39.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell39.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell39.Name = "xrTableCell39";
        this.xrTableCell39.StylePriority.UseBorders = false;
        this.xrTableCell39.StylePriority.UseFont = false;
        this.xrTableCell39.Weight = 1.0007234865413803D;
        // 
        // xrTableCell40
        // 
        this.xrTableCell40.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_AttendanceRegister_Report.Location_Name")});
        this.xrTableCell40.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell40.Name = "xrTableCell40";
        this.xrTableCell40.StylePriority.UseBorders = false;
        this.xrTableCell40.StylePriority.UseFont = false;
        this.xrTableCell40.Text = "xrTableCell40";
        this.xrTableCell40.Weight = 1.6260486453991851D;
        // 
        // xrTableCell41
        // 
        this.xrTableCell41.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell41.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell41.Name = "xrTableCell41";
        this.xrTableCell41.StylePriority.UseBorders = false;
        this.xrTableCell41.StylePriority.UseFont = false;
        this.xrTableCell41.Weight = 0.90625563380149332D;
        // 
        // xrTableCell42
        // 
        this.xrTableCell42.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTableCell42.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell42.Name = "xrTableCell42";
        this.xrTableCell42.StylePriority.UseBorders = false;
        this.xrTableCell42.StylePriority.UseFont = false;
        this.xrTableCell42.Weight = 2.9754052646597571D;
        // 
        // xrTitle
        // 
        this.xrTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrTitle.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTitle.LocationFloat = new DevExpress.Utils.PointFloat(129.2854F, 56.75003F);
        this.xrTitle.Name = "xrTitle";
        this.xrTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrTitle.SizeF = new System.Drawing.SizeF(774.2604F, 15.99999F);
        this.xrTitle.StylePriority.UseBorders = false;
        this.xrTitle.StylePriority.UseFont = false;
        this.xrTitle.StylePriority.UseTextAlignment = false;
        this.xrTitle.Text = "xrTitle";
        this.xrTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(906.0171F, 0F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(244.5248F, 97.33332F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrCompName
        // 
        this.xrCompName.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrCompName.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrCompName.LocationFloat = new DevExpress.Utils.PointFloat(4.041672F, 1.999989F);
        this.xrCompName.Name = "xrCompName";
        this.xrCompName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrCompName.SizeF = new System.Drawing.SizeF(752.0833F, 20F);
        this.xrCompName.StylePriority.UseBorders = false;
        this.xrCompName.StylePriority.UseFont = false;
        this.xrCompName.Text = "xrCompName";
        // 
        // sp_Att_AttendanceRegister_ReportTableAdapter1
        // 
        this.sp_Att_AttendanceRegister_ReportTableAdapter1.ClearBeforeFill = true;
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.xrPanel1});
        this.PageHeader.HeightF = 132.2917F;
        this.PageHeader.Name = "PageHeader";
        // 
        // Att_InOutReport_1_Direct
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.ReportHeader,
            this.PageHeader});
        this.DataMember = "sp_Att_AttendanceRegister_Report";
        this.DataSource = this.attendanceDataSet1;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.Landscape = true;
        this.Margins = new System.Drawing.Printing.Margins(3, 0, 0, 8);
        this.PageHeight = 827;
        this.PageWidth = 1169;
        this.PaperKind = System.Drawing.Printing.PaperKind.A4;
        this.Version = "18.1";
        ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.attendanceDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
    string CompanyId = "0";
    DataTable dtleaveMaster = new DataTable();
    DataTable dtInOutReport = new DataTable();
    DateTime FromDate = new DateTime();
    DateTime ToDate = new DateTime();

    public void SetCompanyId(string Company_Id)
    {
        CompanyId = Company_Id;
    }


    public void SetDateCriteria(DateTime dtFromdate, DateTime dttoDate)
    {
        FromDate = dtFromdate;
        ToDate = dttoDate;
    }

    public void setheaderName(string Id, string Name, string Date, string ShiftName, string OnDutyTime, string OffDutyTime, string InTime, string OutTime, string Late, string Early, string Work, string OverTime, string Status, string Total, string Brand, string Location, string Department)
    {
        //xrTableCell13.Text = "ID";
        //xrTableCell17.Text = Name;

        //xrTableCell7.Text = Date;
        //xrTableCell6.Text = ShiftName;
        //xrTableCell2.Text = OnDutyTime;
        //xrTableCell4.Text = OffDutyTime;
        //xrTableCell1.Text = InTime;
        //xrTableCell22.Text = OutTime;
        //xrTableCell24.Text = Resources.Attendance.Late_In;
        //xrTableCell25.Text = Resources.Attendance.Early_Out;
        //xrTableCell23.Text = Work;
        //xrTableCell26.Text = OverTime;
        //xrTableCell89.Text = Status;
        xrTableCell31.Text = "Company Name :";
        xrTableCell39.Text = "Location :";
        //xrTableCell43.Text = Department;

        //xrTableCell48.Text = Resources.Attendance.Designation;
        //xrTableCell53.Text = "Short";
    }
    public void SetBrandName(string BrandName)
    {
        xrTableCell38.Text = BrandName;
    }
    public void SetLocationName(string LocationName)
    {
        //xrTableCell40.Text = LocationName;
    }
    public void SetDepartmentName(string DepartmentName)
    {
        //xrTableCell44.Text = DepartmentName;
    }

    public void SetLeaveDetail(DataTable dt1, DataTable dt2)
    {
        dtleaveMaster = dt1;
        dtInOutReport = dt2;
        //xrTableCell44.Text = DepartmentName;
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
        // xrCompAddress.Text = address;
    }
    public void setcompanyname(string companyname)
    {
        xrCompName.Text = companyname;
    }
    public string GetHours(object obj)
    {
        if (obj.ToString() == "")
        {
            return "";
        }
        string retval = string.Empty;
        retval = ((Convert.ToInt32(obj) / 60) < 10) ? "0" + (Convert.ToInt32(obj) / 60).ToString() : ((Convert.ToInt32(obj) / 60)).ToString();
        retval += ":" + (((Convert.ToInt32(obj) % 60) < 10) ? "0" + (Convert.ToInt32(obj) % 60) : (Convert.ToInt32(obj) % 60).ToString());

        return retval;
    }








    private void xrTableCell32_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //int sum = 0;
        //DateTime fromdate = objsys.getDateForInput(System.Web.HttpContext.Current.Session["FromDate"].ToString());
        //DateTime ToDate = objsys.getDateForInput(System.Web.HttpContext.Current.Session["ToDate"].ToString());


        //DataTable dt = objAttReg.GetAttendanceRegDataByEmpId(GetCurrentColumnValue("Emp_Id").ToString(), fromdate.ToString(), ToDate.ToString());
        //dt = new DataView(dt,"LateMin<>0","",DataViewRowState.CurrentRows).ToTable();
        //if(dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dt.Rows.Count;i++ )
        //    {

        //        sum += int.Parse(dt.Rows[i]["LateMin"].ToString());
        //    }

        //}
        //xrTableCell32.Text = GetHours(sum.ToString());

    }

    private void xrTableCell33_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //int sum = 0;
        //DateTime fromdate = objsys.getDateForInput(System.Web.HttpContext.Current.Session["FromDate"].ToString());
        //DateTime ToDate = objsys.getDateForInput(System.Web.HttpContext.Current.Session["ToDate"].ToString());


        //DataTable dt = objAttReg.GetAttendanceRegDataByEmpId(GetCurrentColumnValue("Emp_Id").ToString(), fromdate.ToString(), ToDate.ToString());
        //dt = new DataView(dt, "EarlyMin<>0", "", DataViewRowState.CurrentRows).ToTable();
        //if (dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {

        //        sum += int.Parse(dt.Rows[i]["EarlyMin"].ToString());
        //    }

        //}
        //xrTableCell33.Text = GetHours(sum.ToString());
    }

    private void xrTableCell34_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //int sum = 0;
        //DateTime fromdate = objsys.getDateForInput(System.Web.HttpContext.Current.Session["FromDate"].ToString());
        //DateTime ToDate = objsys.getDateForInput(System.Web.HttpContext.Current.Session["ToDate"].ToString());


        //DataTable dt = objAttReg.GetAttendanceRegDataByEmpId(GetCurrentColumnValue("Emp_Id").ToString(), fromdate.ToString(), ToDate.ToString());
        //dt = new DataView(dt, "EffectiveWork_Min<>0", "", DataViewRowState.CurrentRows).ToTable();
        //if (dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {

        //        sum += int.Parse(dt.Rows[i]["EffectiveWork_Min"].ToString());
        //    }

        //}
        //xrTableCell34.Text = GetHours(sum.ToString());

    }

    private void xrTableCell35_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //int sum = 0;
        //DateTime fromdate = objsys.getDateForInput(System.Web.HttpContext.Current.Session["FromDate"].ToString());
        //DateTime ToDate = objsys.getDateForInput(System.Web.HttpContext.Current.Session["ToDate"].ToString());


        //DataTable dt = objAttReg.GetAttendanceRegDataByEmpId(GetCurrentColumnValue("Emp_Id").ToString(), fromdate.ToString(), ToDate.ToString());

        //if (dt.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {

        //        sum += int.Parse(dt.Rows[i]["OverTime_Min"].ToString()) + int.Parse(dt.Rows[i]["Week_Off_Min"].ToString()) + int.Parse(dt.Rows[i]["Holiday_Min"].ToString());
        //    }

        //}
        //xrTableCell35.Text = GetHours(sum.ToString());
    }




    public void setUserName(string UserName)
    {
        xrLabel10.Text = UserName;
    }

    private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        int Counter = 0;
        foreach (Band b in Report.Bands)
        {

            foreach (XRControl c in b.Controls)
            {
                if (c is XRTable)
                {
                    XRTable t = (XRTable)c;
                    if (t.Name.Trim() == "xrTable2")
                    {
                        Counter = 1;

                        foreach (XRControl row in t.Controls)
                        {
                            //row.BackColor = System.Drawing.ColorTranslator.FromHtml("#" + GetCurrentColumnValue("Field2").ToString());
                            foreach (XRControl cell in row.Controls)
                            {
                                // binding logic
                                try
                                {
                                    //if (xrTableCell19.Text != "" && xrTableCell5.Text != "")
                                    //{
                                    //    cell.BackColor = System.Drawing.Color.Black;
                                    //}
                                    //cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#" + GetCurrentColumnValue("Field2").ToString());
                                }
                                catch
                                {

                                }
                            }
                        }
                        //break;
                    }
                }
            }
            //if (Counter == 1)
            //{
            //    break;
            //}
        }
    }
    private void xrTableCell70_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }

    private void xrTableCell70_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell70.Text == "0:00" || xrTableCell70.Text == "00:00")
        //{
        //    xrTableCell70.Text = "";
        //}
    }

    private void xrTableCell72_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell72.Text == "0:00" || xrTableCell72.Text == "00:00")
        //{
        //    xrTableCell72.Text = "";
        //}
    }

    private void xrTableCell66_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell66.Text == "0:00" || xrTableCell66.Text == "00:00")
        //{
        //    xrTableCell66.Text = "";
        //}
    }

    private void xrTableCell67_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell67.Text == "0:00" || xrTableCell67.Text == "00:00")
        //{
        //    xrTableCell67.Text = "";
        //}
    }

    private void xrTableCell68_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell68.Text == "0:00" || xrTableCell68.Text == "00:00")
        //{
        //    xrTableCell68.Text = "";
        //}
    }

    private void xrTableCell60_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell60.Text.Trim() == "0")
        //{
        //    xrTableCell60.Text = "";
        //}
    }

    private void xrTableCell61_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell61.Text.Trim() == "0")
        //{
        //    xrTableCell61.Text = "";
        //}
    }

    private void xrTableCell62_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell62.Text.Trim() == "0")
        //{
        //    xrTableCell62.Text = "";
        //}
    }

    private void xrTableCell63_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell63.Text.Trim() == "0")
        //{
        //    xrTableCell63.Text = "";
        //}
    }

    private void xrTableCell64_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell64.Text.Trim() == "0")
        //{
        //    xrTableCell64.Text = "";
        //}
    }

    private void xrTableCell65_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell65.Text.Trim() == "0")
        //{
        //    xrTableCell65.Text = "";
        //}
    }

    private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        // xrTable5.Rows.Clear();



        //if (GetCurrentColumnValue("Emp_Id") != null)
        //{
        //    string strEmpId = GetCurrentColumnValue("Emp_Id").ToString(); ;
        //    string strHoliday = GetCurrentColumnValue("Holidaycount").ToString(); ;



        //    XRTableRow dr = new XRTableRow();
        //   // xrTable5.Rows.Add(dr);


        //    //here we are generating table header 
        //    DataTable dtleavedetail = dtInOutReport;
        //    DataTable dtLeaveHeader = dtleaveMaster;

        //    for (int i = 0; i < dtLeaveHeader.Columns.Count; i++)
        //    {
        //        XRTableCell xr = new XRTableCell();

        //        xr.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        xr.Name = dtLeaveHeader.Columns[i].ColumnName.ToString();
        //        xr.StylePriority.UseFont = false;
        //        xr.StylePriority.UseTextAlignment = false;
        //        xr.Text = dtLeaveHeader.Columns[i].ColumnName.ToString();
        //        xr.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        //        xr.Weight = 0.62036202421487674D;

        //        //float widthF = (float.Parse("878.96") / dtLeaveHeader.Columns.Count);

        //        //xr.SizeF = new SizeF(xr.SizeF.Width+ widthF, 25F);
        //        dr.Cells.Add(xr);
        //    }


        //    XRTableRow dr1 = new XRTableRow();
        //   // xrTable5.Rows.Add(dr1);


        //    for (int i = 0; i < dtLeaveHeader.Columns.Count; i++)
        //    {
        //        XRTableCell xr = new XRTableCell();

        //        xr.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        xr.Name = dtLeaveHeader.Columns[i].ColumnName.ToString() + i.ToString();
        //        xr.StylePriority.UseFont = false;
        //        xr.StylePriority.UseTextAlignment = false;
        //        if (dtLeaveHeader.Columns[i].ColumnName == "Holidays")
        //        {
        //            xr.Text = strHoliday;
        //        }
        //        else
        //        {
        //            xr.Text = new DataView(dtleavedetail, "Emp_Id="+strEmpId+ " and Is_Leave='True' and Field1='"+ dtLeaveHeader.Columns[i].ColumnName.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable(true,"Att_Date").Rows.Count.ToString();
        //        }

        //        if(xr.Text=="" || xr.Text == "0")
        //        {
        //            xr.Text = "";
        //        }

        //        xr.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        //        xr.Weight = 0.62036202421487674D;

        //        //float widthF = (float.Parse("878.96") / dtLeaveHeader.Columns.Count);

        //        //xr.SizeF = new SizeF(xr.SizeF.Width+ widthF, 25F);
        //        dr1.Cells.Add(xr);
        //    }
        //}

    }

    private void xrTableCell28_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell28.Text == "0:00" || xrTableCell28.Text == "00:00")
        {
            xrTableCell28.Text = "";
        }
    }

    private void xrTableCell20_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell20.Text == "0:00" || xrTableCell20.Text == "00:00")
        {
            xrTableCell20.Text = "";
        }
    }

    private void xrTableCell19_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell19.Text == "0:00" || xrTableCell19.Text == "00:00")
        {
            xrTableCell19.Text = "";
        }


    }

    private void xrTableCell5_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell5.Text == "0:00" || xrTableCell5.Text == "00:00")
        {
            xrTableCell5.Text = "";
        }
    }

    private void xrTableCell86_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell86.Text == "0:00" || xrTableCell86.Text == "00:00")
        //{
        //    xrTableCell86.Text = "";
        //}
    }

    private void xrTableCell88_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell88.Text == "0:00" || xrTableCell88.Text == "00:00")
        //{
        //    xrTableCell88.Text = "";
        //}
    }

    private void xrTableCell8_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell8.Text == "0:00" || xrTableCell8.Text == "00:00")
        //{
        //    xrTableCell8.Text = "";
        //}
    }

    private void xrTableCell10_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell10.Text == "0:00" || xrTableCell10.Text == "00:00")
        //{
        //    xrTableCell10.Text = "";
        //}
    }

    private void xrTableCell9_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (xrTableCell9.Text == "0:00" || xrTableCell9.Text == "00:00")
        {
            xrTableCell9.Text = "";
        }
    }

    private void xrTableCell11_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell11.Text == "0:00" || xrTableCell11.Text == "00:00")
        //{
        //    xrTableCell11.Text = "";
        //}
    }

    private void xrTableCell54_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        //if (xrTableCell54.Text == "0:00" || xrTableCell54.Text == "00:00")
        //{
        //    xrTableCell54.Text = "";
        //}
    }

    private void xrTableCell30_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {

        if (xrTableCell30.Text != "")
        {
            xrTableCell30.Text = Convert.ToDateTime(xrTableCell30.Text).ToString("dd-MMM-yyyy") + " (" + Convert.ToDateTime(xrTableCell30.Text).DayOfWeek.ToString().Substring(0, 3) + ")";

        }

    }
}
