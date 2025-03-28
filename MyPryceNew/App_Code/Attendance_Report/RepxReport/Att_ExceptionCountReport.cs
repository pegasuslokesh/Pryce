using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for Att_ShiftReport
/// </summary>
public class Att_ExceptionCountReport : DevExpress.XtraReports.UI.XtraReport
{
    Set_Employee_Holiday objEmpHoli = null;
    SystemParameter objSys = null;
	private DevExpress.XtraReports.UI.DetailBand Detail;
	private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private PageFooterBand PageFooter;
    private AttendanceDataSet attendanceDataSet1;
    private AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter sp_Att_ScheduleDescription_ReportTableAdapter1;
    private ReportHeaderBand ReportHeader;
    private GroupHeaderBand GroupHeader2;
    private XRPageInfo xrPageInfo2;
    private AttendanceDataSetTableAdapters.sp_Set_Employee_Holiday_ReportTableAdapter sp_Set_Employee_Holiday_ReportTableAdapter1;
    private AttendanceDataSetTableAdapters.sp_Set_Att_Employee_Leave_ReportTableAdapter sp_Set_Att_Employee_Leave_ReportTableAdapter1;
    private AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter sp_Att_AttendanceLog_ReportTableAdapter1;
    private AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter sp_Att_AttendanceLog_ReportTableAdapter2;
    private AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter sp_Att_AttendanceRegister_ReportTableAdapter1;
    private XRLabel xrTitle;
    private XRLabel xrCompName;
    private XRLabel xrLabel9;
    private XRLabel xrLabel10;
    private XRPageInfo xrPageInfo3;
    private PageHeaderBand PageHeader;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell fldEmpCode;
    private XRTableCell fleEmployeeName;
    private XRTableCell fldDesignation;
    private XRTableCell fldDepartment;
    private XRTableCell fldNoOfOccurance;
    private XRTableCell xrTableCell3;
    private XRLabel fldLocationname;
    private GroupFooterBand GroupFooter1;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public Att_ExceptionCountReport(string strConString)
	{
		InitializeComponent();
        objEmpHoli = new Set_Employee_Holiday(strConString);
        objSys = new SystemParameter(strConString);
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
            string resourceFileName = "Att_ExceptionCountReport.resx";
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.attendanceDataSet1 = new AttendanceDataSet();
            this.sp_Att_ScheduleDescription_ReportTableAdapter1 = new AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCompName = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.sp_Set_Employee_Holiday_ReportTableAdapter1 = new AttendanceDataSetTableAdapters.sp_Set_Employee_Holiday_ReportTableAdapter();
            this.sp_Set_Att_Employee_Leave_ReportTableAdapter1 = new AttendanceDataSetTableAdapters.sp_Set_Att_Employee_Leave_ReportTableAdapter();
            this.sp_Att_AttendanceLog_ReportTableAdapter1 = new AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter();
            this.sp_Att_AttendanceLog_ReportTableAdapter2 = new AttendanceDataSetTableAdapters.sp_Att_AttendanceLog_ReportTableAdapter();
            this.sp_Att_AttendanceRegister_ReportTableAdapter1 = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.fldEmpCode = new DevExpress.XtraReports.UI.XRTableCell();
            this.fleEmployeeName = new DevExpress.XtraReports.UI.XRTableCell();
            this.fldDesignation = new DevExpress.XtraReports.UI.XRTableCell();
            this.fldDepartment = new DevExpress.XtraReports.UI.XRTableCell();
            this.fldNoOfOccurance = new DevExpress.XtraReports.UI.XRTableCell();
            this.fldLocationname = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.attendanceDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 23.33333F;
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
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo2,
            this.xrPageInfo3,
            this.xrLabel10,
            this.xrLabel9});
            this.PageFooter.HeightF = 22.99999F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Format = "Page{0}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(706.1588F, 0F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(94.6745F, 15.70834F);
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
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
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCompName,
            this.xrTitle});
            this.ReportHeader.HeightF = 74.04175F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(1.7682F, 0F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(91.95319F, 15.70834F);
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Created By:";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(93.72139F, 0F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(186.4217F, 15.70834F);
            this.xrLabel10.StylePriority.UseBorders = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "xrLabel10";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPageInfo3.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(320.6116F, 0F);
            this.xrPageInfo3.Name = "xrPageInfo3";
            this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo3.SizeF = new System.Drawing.SizeF(259.3884F, 15.70834F);
            this.xrPageInfo3.StylePriority.UseBorders = false;
            // 
            // xrTitle
            // 
            this.xrTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTitle.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrTitle.LocationFloat = new DevExpress.Utils.PointFloat(1.7682F, 48.70834F);
            this.xrTitle.Name = "xrTitle";
            this.xrTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTitle.SizeF = new System.Drawing.SizeF(800.2318F, 16F);
            this.xrTitle.StylePriority.UseBorders = false;
            this.xrTitle.StylePriority.UseFont = false;
            this.xrTitle.StylePriority.UseTextAlignment = false;
            this.xrTitle.Text = "xrTitle";
            this.xrTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrCompName
            // 
            this.xrCompName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrCompName.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrCompName.LocationFloat = new DevExpress.Utils.PointFloat(1.7682F, 10.00001F);
            this.xrCompName.Name = "xrCompName";
            this.xrCompName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrCompName.SizeF = new System.Drawing.SizeF(799.0651F, 23F);
            this.xrCompName.StylePriority.UseBorders = false;
            this.xrCompName.StylePriority.UseFont = false;
            this.xrCompName.Text = "xrCompName";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.fldLocationname});
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Location_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.HeightF = 34.79166F;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // sp_Set_Employee_Holiday_ReportTableAdapter1
            // 
            this.sp_Set_Employee_Holiday_ReportTableAdapter1.ClearBeforeFill = true;
            // 
            // sp_Set_Att_Employee_Leave_ReportTableAdapter1
            // 
            this.sp_Set_Att_Employee_Leave_ReportTableAdapter1.ClearBeforeFill = true;
            // 
            // sp_Att_AttendanceLog_ReportTableAdapter1
            // 
            this.sp_Att_AttendanceLog_ReportTableAdapter1.ClearBeforeFill = true;
            // 
            // sp_Att_AttendanceLog_ReportTableAdapter2
            // 
            this.sp_Att_AttendanceLog_ReportTableAdapter2.ClearBeforeFill = true;
            // 
            // sp_Att_AttendanceRegister_ReportTableAdapter1
            // 
            this.sp_Att_AttendanceRegister_ReportTableAdapter1.ClearBeforeFill = true;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.HeightF = 27.49999F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(800.8333F, 27.49999F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell7,
            this.xrTableCell1,
            this.xrTableCell3,
            this.xrTableCell2});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.xrTableCell11.StylePriority.UsePadding = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "Emp Code";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell11.Weight = 0.937346047218852D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.xrTableCell7.StylePriority.UsePadding = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Employee Name";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell7.Weight = 2.2727539156308065D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Designation";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 1.2850531122810818D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "No Of Occurances";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell2.Weight = 0.93784080290349048D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Department";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 1.0142565714378002D;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Font = new System.Drawing.Font("Times New Roman", 8F, System.Drawing.FontStyle.Bold);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(800.8333F, 22.29166F);
            this.xrTable2.StylePriority.UseBorderDashStyle = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.fldEmpCode,
            this.fleEmployeeName,
            this.fldDesignation,
            this.fldDepartment,
            this.fldNoOfOccurance});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // fldEmpCode
            // 
            this.fldEmpCode.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_ExceptionCountReport.Emp_Code")});
            this.fldEmpCode.Name = "fldEmpCode";
            this.fldEmpCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.fldEmpCode.StylePriority.UsePadding = false;
            this.fldEmpCode.StylePriority.UseTextAlignment = false;
            this.fldEmpCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.fldEmpCode.Weight = 0.937346047218852D;
            // 
            // fleEmployeeName
            // 
            this.fleEmployeeName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_ExceptionCountReport.Emp_Name")});
            this.fleEmployeeName.Name = "fleEmployeeName";
            this.fleEmployeeName.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.fleEmployeeName.StylePriority.UsePadding = false;
            this.fleEmployeeName.StylePriority.UseTextAlignment = false;
            this.fleEmployeeName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.fleEmployeeName.Weight = 2.2727539156308065D;
            // 
            // fldDesignation
            // 
            this.fldDesignation.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_ExceptionCountReport.Designation")});
            this.fldDesignation.Name = "fldDesignation";
            this.fldDesignation.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.fldDesignation.StylePriority.UsePadding = false;
            this.fldDesignation.StylePriority.UseTextAlignment = false;
            this.fldDesignation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.fldDesignation.Weight = 1.2850531122810818D;
            // 
            // fldDepartment
            // 
            this.fldDepartment.Name = "fldDepartment";
            this.fldDepartment.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.fldDepartment.StylePriority.UsePadding = false;
            this.fldDepartment.StylePriority.UseTextAlignment = false;
            this.fldDepartment.Text = "Department";
            this.fldDepartment.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.fldDepartment.Weight = 1.0142565714378002D;
            // 
            // fldNoOfOccurance
            // 
            this.fldNoOfOccurance.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_ExceptionCountReport.NoOfOccurance")});
            this.fldNoOfOccurance.Name = "fldNoOfOccurance";
            this.fldNoOfOccurance.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.fldNoOfOccurance.StylePriority.UsePadding = false;
            this.fldNoOfOccurance.StylePriority.UseTextAlignment = false;
            this.fldNoOfOccurance.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.fldNoOfOccurance.Weight = 0.93784080290349048D;
            // 
            // fldLocationname
            // 
            this.fldLocationname.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.fldLocationname.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Att_ExceptionCountReport.Location_Name")});
            this.fldLocationname.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.fldLocationname.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2.749984F);
            this.fldLocationname.Name = "fldLocationname";
            this.fldLocationname.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.fldLocationname.SizeF = new System.Drawing.SizeF(398.7366F, 22.04167F);
            this.fldLocationname.StylePriority.UseBorders = false;
            this.fldLocationname.StylePriority.UseFont = false;
            this.fldLocationname.StylePriority.UsePadding = false;
            this.fldLocationname.StylePriority.UseTextAlignment = false;
            this.fldLocationname.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.HeightF = 10.41667F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // Att_ExceptionCountReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.ReportHeader,
            this.GroupHeader2,
            this.PageHeader,
            this.GroupFooter1});
            this.DataMember = "sp_Att_ExceptionCountReport";
            this.DataSource = this.attendanceDataSet1;
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(23, 25, 0, 0);
            this.Version = "14.1";
            ((System.ComponentModel.ISupportInitialize)(this.attendanceDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

	}

	#endregion

    public void setheaderName(string Id, string Name, string InOutException, string EventDate)
    {
       
        xrTableCell11.Text = Id;
        xrTableCell7.Text = Name;
        xrTableCell1.Text = EventDate;
        xrTableCell2.Text = InOutException;
        
    }
    public void SetBrandName(string BrandName)
    {
        
    }
    public void SetLocationName(string LocationName)
    {
       
    }
    public void SetDepartmentName(string DepartmentName)
    {
       
    }


    public void SetImage(string Url)
    {
        
    }
    public void setTitleName(string Title)
    {
        xrTitle.Text = Title;
        
    }
    public void setaddress(string address)
    {
        
    }
    public void setcompanyname(string companyname)
    {
        xrCompName.Text = companyname;
    }
    public void setUserName(string UserName)
    {
        xrLabel10.Text = UserName;
    }
    

   

   
}
