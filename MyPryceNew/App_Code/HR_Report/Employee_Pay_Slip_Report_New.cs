using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for Employee_Pay_Slip_Report_New
/// </summary>
public class Employee_Pay_Slip_Report_New : DevExpress.XtraReports.UI.XtraReport
{
    string NetSalary = "0";
    string Comp_Id = string.Empty;
    SystemParameter objSysParam = null;
    private string _strConString = string.Empty;
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private EmployeePaySlipDataSet employeePaySlipDataSet1;
    private XRPanel xrPanel2;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel5;
    private XRLabel xrLabel29;
    private XRLabel xrLabel10;
    private XRLabel xrLabel11;
    private XRLabel xrLabel12;
    private XRLabel xrLabel13;
    private XRLabel xrLabel14;
    private XRLabel xrLabel15;
    private XRLabel xrLabel4;
    private XRLabel xrLabel8;
    private XRLabel xrLabel9;
    private XRLabel xrLabel3;
    private XRLabel xrLabel2;
    private XRLabel xrLabel1;
    private GroupHeaderBand GroupHeader1;
    private GroupFooterBand GroupFooter1;
    private XRPanel xrPanel3;
    private XRTable xrTable2;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell37;
    private XRTable xrTable3;
    private XRTableRow xrTableRow31;
    private XRTableCell xrTableCell80;
    private XRTableCell xrTableCell81;
    private XRTableCell xrTableCell82;
    private XRPanel xrPanel1;
    private XRTable xrTable11;
    private XRTableRow xrTableRow23;
    private XRTableCell xrTableCell51;
    private XRTableRow xrTableRow24;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell73;
    private XRTable xrTable1;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell9;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRPanel xrPanel4;
    private XRTable xrTable4;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell22;
    private XRTable xrTable5;
    private XRTableRow xrTableRow22;
    private XRTableCell xrTableCell62;
    private XRTableCell xrTableCell64;
    private XRPageInfo xrPageInfo3;
    private XRLabel xrLabel23;
    private XRLabel xrLblAmountInWords;
    private XRLabel xrLabel21;
    private XRLabel xrLabel19;
    private XRLabel xrLabel18;
    private XRLabel xrLabel22;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public Employee_Pay_Slip_Report_New(string strConString)
    {
        InitializeComponent();
        objSysParam = new SystemParameter(strConString);
        _strConString = strConString;
        //
        // TODO: Add constructor logic here
        //
    }
    public void setimage(string url)
    {
        xrPictureBox1.ImageUrl = url;
    }
    
    public void setnetamount(string netamt)
    {

        //xrLabel10.Text = netamt;

    }
    public void Setheader(string CreatedBy, string EmployeeCode, string EmployeeName, string Designation, string Department, string Doj, string BankAc, string Attandance, string Attendence, string DayPresent, string WeekOff, string AbsentDays, string Holidays, string Leaves, string BasicSalary, string Attandancesalary, string OvertimeSalary, string Penalty, string WorkDays, string NormalOt, string WeekOffOt, string holidaysOt, string Late, string Early, string Partial, string Absent, string Total, string GrossPay, string PF, string Esic, string Netsalary, string HrSign, string EmployeeSign, string PayslipForthemonth, string EmployeeCopy, string Employercopy, string TotalDays)
    {        
        //this.xrTableCell51.Text = Attandance;
        //this.xrTableCell53.Text = DayPresent;
        xrLabel5.Text = PayslipForthemonth;
        //xrTableCell51.Text = Resources.Attendance.Attendance_Detail;
    }
    
    public void Net_Amount(string Total_Days, string Total_Present, string Tot_Allowance,string Tot_Deduction,string Net_Salary)
    {
        //Net_Salary = Common.Get_Roundoff_Amount_By_Location(Net_Salary);
        xrLabel23.Text = Net_Salary;
        NetSalary = Net_Salary;
        //xrLabel17.Text = Total_Days;
        xrLabel19.Text = Total_Present;
    }
    public void Company_ID(string Company_ID)
    {
        Comp_Id = Company_ID;
    }
    public void setLocationName(string LocationName)
    {
        xrLabel29.Text = LocationName;
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
            string resourceFileName = "Employee_Pay_Slip_Report_New.resx";
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow31 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.employeePaySlipDataSet1 = new EmployeePaySlipDataSet();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrTable11 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow23 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow24 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblAmountInWords = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeePaySlipDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel3});
            this.Detail.HeightF = 22.46F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseBorders = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel3
            // 
            this.xrPanel3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xrTable3});
            this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(13F, 0F);
            this.xrPanel3.Name = "xrPanel3";
            this.xrPanel3.SizeF = new System.Drawing.SizeF(879F, 22.46F);
            this.xrPanel3.StylePriority.UseBorders = false;
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(16.57288F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow14});
            this.xrTable2.SizeF = new System.Drawing.SizeF(419.3749F, 22.45333F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell35,
            this.xrTableCell36,
            this.xrTableCell37});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.S_No")});
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.StylePriority.UseTextAlignment = false;
            this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell35.Weight = 0.69064162037496313D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Allowance")});
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.xrTableCell36.StylePriority.UsePadding = false;
            this.xrTableCell36.StylePriority.UseTextAlignment = false;
            this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell36.Weight = 3.1503965364722575D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Act_Allowance_Value")});
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.StylePriority.UseTextAlignment = false;
            this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell37.Weight = 2.0623709635090153D;
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable3.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(453.2214F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow31});
            this.xrTable3.SizeF = new System.Drawing.SizeF(407.4283F, 22.45333F);
            this.xrTable3.StylePriority.UseBorders = false;
            this.xrTable3.StylePriority.UseFont = false;
            // 
            // xrTableRow31
            // 
            this.xrTableRow31.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell80,
            this.xrTableCell81,
            this.xrTableCell82});
            this.xrTableRow31.Name = "xrTableRow31";
            this.xrTableRow31.Weight = 1D;
            // 
            // xrTableCell80
            // 
            this.xrTableCell80.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.S_No")});
            this.xrTableCell80.Name = "xrTableCell80";
            this.xrTableCell80.StylePriority.UseTextAlignment = false;
            this.xrTableCell80.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell80.Weight = 0.43236222555723913D;
            // 
            // xrTableCell81
            // 
            this.xrTableCell81.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Deduction")});
            this.xrTableCell81.Name = "xrTableCell81";
            this.xrTableCell81.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 0, 0, 0, 100F);
            this.xrTableCell81.StylePriority.UsePadding = false;
            this.xrTableCell81.StylePriority.UseTextAlignment = false;
            this.xrTableCell81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell81.Weight = 1.9722689968066163D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Deduction_Value")});
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.StylePriority.UseTextAlignment = false;
            this.xrTableCell82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell82.Weight = 1.2911204761161308D;
            // 
            // TopMargin
            // 
            this.TopMargin.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.TopMargin.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseBorders = false;
            this.TopMargin.StylePriority.UseFont = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel2
            // 
            this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel29,
            this.xrPictureBox1,
            this.xrLabel5,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel14,
            this.xrLabel15,
            this.xrLabel4,
            this.xrLabel8,
            this.xrLabel9,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.xrLabel13,
            this.xrLabel10});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(13F, 32F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(879F, 162F);
            this.xrPanel2.StylePriority.UseBorders = false;
            // 
            // xrLabel29
            // 
            this.xrLabel29.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(19F, 57F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(842.0388F, 20.91667F);
            this.xrLabel29.StylePriority.UseBorderColor = false;
            this.xrLabel29.StylePriority.UseBorders = false;
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "xrLabel10";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(19F, 13F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(80F, 40F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox1.StylePriority.UseBorders = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(215.9023F, 17.00001F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(645.1365F, 25.97689F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Salary Slip for the month of ";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Dep_Name")});
            this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(540.653F, 88.04169F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(320.3857F, 23F);
            this.xrLabel12.StylePriority.UseBorders = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Bank_Name")});
            this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(498.2216F, 111.0417F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(362.8171F, 23F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(453.2214F, 111.0417F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(45F, 23F);
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "Bank : ";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(453.2214F, 88.04169F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(87.43146F, 23F);
            this.xrLabel15.StylePriority.UseBorders = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "Department : ";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Account_No")});
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(581.2215F, 134.0416F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(279.8173F, 23F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Designation")});
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(103.5728F, 134.0416F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(332.375F, 23F);
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Emp_Name")});
            this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(131.6978F, 88.04166F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(304.2499F, 23F);
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(453.0965F, 134.0416F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(128.125F, 23F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Bank Account No. : ";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(16.5728F, 134.0416F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(87F, 23F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Designation : ";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(16.57278F, 88.04166F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(115F, 23F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Employee Name : ";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(16.57288F, 111.0416F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(112F, 23F);
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "Employee Code : ";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Emp_Code")});
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(128.5732F, 111.0416F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(307.3746F, 23F);
            this.xrLabel10.StylePriority.UseBorders = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.BottomMargin.HeightF = 23F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseBorders = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // employeePaySlipDataSet1
            // 
            this.employeePaySlipDataSet1.DataSetName = "EmployeePaySlipDataSet";
            this.employeePaySlipDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1,
            this.xrPanel2});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Emp_Id", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WholePage;
            this.GroupHeader1.HeightF = 238.91F;
            this.GroupHeader1.KeepTogether = true;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.StylePriority.UseBorders = false;
            // 
            // xrPanel1
            // 
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable11,
            this.xrTable1});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(13F, 194F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(879F, 44.91F);
            this.xrPanel1.StylePriority.UseBorders = false;
            // 
            // xrTable11
            // 
            this.xrTable11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable11.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTable11.LocationFloat = new DevExpress.Utils.PointFloat(16.57278F, 0F);
            this.xrTable11.Name = "xrTable11";
            this.xrTable11.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow23,
            this.xrTableRow24});
            this.xrTable11.SizeF = new System.Drawing.SizeF(419.3749F, 44.90666F);
            this.xrTable11.StylePriority.UseBorders = false;
            this.xrTable11.StylePriority.UseFont = false;
            // 
            // xrTableRow23
            // 
            this.xrTableRow23.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell51});
            this.xrTableRow23.Name = "xrTableRow23";
            this.xrTableRow23.Weight = 1D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell51.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.StylePriority.UseBorders = false;
            this.xrTableCell51.StylePriority.UseFont = false;
            this.xrTableCell51.StylePriority.UseTextAlignment = false;
            this.xrTableCell51.Text = "Earnings";
            this.xrTableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell51.Weight = 3.6957091870433487D;
            // 
            // xrTableRow24
            // 
            this.xrTableRow24.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell53,
            this.xrTableCell73});
            this.xrTableRow24.Name = "xrTableRow24";
            this.xrTableRow24.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "SL";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.43236222555723913D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.StylePriority.UseBorders = false;
            this.xrTableCell53.StylePriority.UseFont = false;
            this.xrTableCell53.StylePriority.UseTextAlignment = false;
            this.xrTableCell53.Text = "Salary Head";
            this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell53.Weight = 1.9722412342357505D;
            // 
            // xrTableCell73
            // 
            this.xrTableCell73.Name = "xrTableCell73";
            this.xrTableCell73.StylePriority.UseBorders = false;
            this.xrTableCell73.StylePriority.UseTextAlignment = false;
            this.xrTableCell73.Text = "Amount (Rs.)";
            this.xrTableCell73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell73.Weight = 1.291105727250359D;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(453.2213F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3,
            this.xrTableRow4});
            this.xrTable1.SizeF = new System.Drawing.SizeF(407.4284F, 44.90666F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Deductions";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 3.6957091870433487D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrTableCell13,
            this.xrTableCell14});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "SL";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.43236222555723913D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseBorders = false;
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "Salary Head";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell13.Weight = 1.9722412342357505D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "Amount (Rs.)";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell14.Weight = 1.291105727250359D;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel4});
            this.GroupFooter1.HeightF = 171.1783F;
            this.GroupFooter1.KeepTogether = true;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.StylePriority.UseBorders = false;
            this.GroupFooter1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupFooter1_BeforePrint);
            // 
            // xrPanel4
            // 
            this.xrPanel4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrTable5,
            this.xrPageInfo3,
            this.xrLabel23,
            this.xrLblAmountInWords,
            this.xrLabel21,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel22});
            this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(13F, 0F);
            this.xrPanel4.Name = "xrPanel4";
            this.xrPanel4.SizeF = new System.Drawing.SizeF(879F, 143.75F);
            this.xrPanel4.StylePriority.UseBorders = false;
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(16.57278F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8});
            this.xrTable4.SizeF = new System.Drawing.SizeF(419.3749F, 22.45333F);
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseFont = false;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell21,
            this.xrTableCell22});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "Gross Total";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell21.Weight = 2.3864115197751281D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Act_Allowance_Value")});
            this.xrTableCell22.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseFont = false;
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell22.Summary = xrSummary1;
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell22.Weight = 1.2813358050289219D;
            // 
            // xrTable5
            // 
            this.xrTable5.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable5.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(453.2215F, 0F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow22});
            this.xrTable5.SizeF = new System.Drawing.SizeF(407.4283F, 22.45333F);
            this.xrTable5.StylePriority.UseBorders = false;
            this.xrTable5.StylePriority.UseFont = false;
            // 
            // xrTableRow22
            // 
            this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell62,
            this.xrTableCell64});
            this.xrTableRow22.Name = "xrTableRow22";
            this.xrTableRow22.Weight = 1D;
            // 
            // xrTableCell62
            // 
            this.xrTableCell62.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell62.Name = "xrTableCell62";
            this.xrTableCell62.StylePriority.UseFont = false;
            this.xrTableCell62.StylePriority.UseTextAlignment = false;
            this.xrTableCell62.Text = "Total Deductions";
            this.xrTableCell62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell62.Weight = 5.9390728387296123D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Deduction_Value")});
            this.xrTableCell64.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.StylePriority.UseFont = false;
            this.xrTableCell64.StylePriority.UseTextAlignment = false;
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell64.Summary = xrSummary2;
            this.xrTableCell64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell64.Weight = 3.1888744853374713D;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPageInfo3.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrPageInfo3.Format = "{MMM dd, yyyy}";
            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(495.8426F, 114.5372F);
            this.xrPageInfo3.Name = "xrPageInfo3";
            this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo3.SizeF = new System.Drawing.SizeF(242.7568F, 22.99997F);
            this.xrPageInfo3.StylePriority.UseBorders = false;
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            this.xrPageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel23
            // 
            this.xrLabel23.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.Net_Salary")});
            this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(96.5728F, 31.7158F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(122.0504F, 23F);
            this.xrLabel23.StylePriority.UseBorders = false;
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel23.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel23_BeforePrint);
            this.xrLabel23.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrLabel23_PrintOnPage);
            // 
            // xrLblAmountInWords
            // 
            this.xrLblAmountInWords.BorderColor = System.Drawing.Color.Black;
            this.xrLblAmountInWords.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLblAmountInWords.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLblAmountInWords.LocationFloat = new DevExpress.Utils.PointFloat(16.57277F, 59.70247F);
            this.xrLblAmountInWords.Name = "xrLblAmountInWords";
            this.xrLblAmountInWords.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLblAmountInWords.SizeF = new System.Drawing.SizeF(844.077F, 19.70834F);
            this.xrLblAmountInWords.StylePriority.UseBorderColor = false;
            this.xrLblAmountInWords.StylePriority.UseBorders = false;
            this.xrLblAmountInWords.StylePriority.UseFont = false;
            this.xrLblAmountInWords.StylePriority.UseTextAlignment = false;
            this.xrLblAmountInWords.Text = "Amount In Words";
            this.xrLblAmountInWords.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLblAmountInWords.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLblAmountInWords_BeforePrint);
            // 
            // xrLabel21
            // 
            this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(738.5993F, 114.5372F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(122.0504F, 23F);
            this.xrLabel21.StylePriority.UseBorders = false;
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "Accountant/HR";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel19
            // 
            this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Dt_Pay_Slip.dayspres")});
            this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(149.6978F, 114.5372F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(34F, 22.99998F);
            this.xrLabel19.StylePriority.UseBorders = false;
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel18
            // 
            this.xrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(16.57277F, 114.5372F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(133.1251F, 22.99999F);
            this.xrLabel18.StylePriority.UseBorders = false;
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "Total Days Present : ";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel22
            // 
            this.xrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(16.57278F, 31.71586F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(80.00001F, 23F);
            this.xrLabel22.StylePriority.UseBorders = false;
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "Net Salary : ";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // Employee_Pay_Slip_Report_New
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.GroupFooter1});
            this.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.DataMember = "dtEmpPayslip";
            this.DataSource = this.employeePaySlipDataSet1;
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(0, 6, 0, 23);
            this.PageHeight = 1169;
            this.PageWidth = 903;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "14.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeePaySlipDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    double netsalary = 0;
    double netsalary1 = 0;
    double netsalary_EmployeeCurrency = 0;
    double netsalary_EmployeeCurrency1 = 0;
    string CurrencySymbol = string.Empty;
    double GrossAmount = 0;
    string CurrencySymbol_Gross = string.Empty;
    double GrossAmount_Copy = 0;
    string CurrencySymbol_GrossCopy = string.Empty;
    string Emp_Id = string.Empty;







    private void xrTableCell122_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //double GrossPay=0;
        //try
        //{
        //    GrossPay = double.Parse(xrTableCell122.Text);
        //    GrossPay = System.Math.Round(GrossPay,3);
        //    xrTableCell122.Text = GrossPay.ToString();
        //}
        //    catch
        //{

        //    }
    }

    private void xrTableCell183_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //double GrossPay = 0;
        //try
        //{
        //    GrossPay = double.Parse(xrTableCell183.Text);
        //    GrossPay = System.Math.Round(GrossPay, 3);
        //    xrTableCell183.Text = GrossPay.ToString();
        //}
        //catch
        //{

        //}
    }

    

    private void xrTableCell23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell23.Text = GetCurrentColumnValue("Currency_Symbol").ToString() + xrTableCell23.Text;
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell22_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell22.Text = GetCurrentColumnValue("Currency_Symbol").ToString() + xrTableCell22.Text;
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell186_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell186.Text = GetCurrentColumnValue("Currency_Symbol").ToString() + xrTableCell186.Text;
        //}
        //catch
        //{

        //}
    }

    private void xrTableCell22_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {

        e.Result = objSysParam.GetCurencyConversion(Emp_Id, GrossAmount.ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
        e.Handled = true;
    }

    private void xrTableCell22_SummaryReset(object sender, EventArgs e)
    {
        GrossAmount = 0;
        CurrencySymbol_Gross = "";
    }

    private void xrTableCell22_SummaryRowChanged(object sender, EventArgs e)
    {
        Emp_Id = GetCurrentColumnValue("Emp_Id").ToString();
        GrossAmount += Convert.ToDouble(GetCurrentColumnValue("AllowanceActAmt").ToString());
        CurrencySymbol_Gross = GetCurrentColumnValue("Currency_Symbol").ToString();
    }

    private void xrTableCell188_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        e.Result = objSysParam.GetCurencyConversion(Emp_Id, GrossAmount_Copy.ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
        e.Handled = true;
    }

    private void xrTableCell188_SummaryReset(object sender, EventArgs e)
    {
        GrossAmount_Copy = 0;
        CurrencySymbol_GrossCopy = "";
    }

    private void xrTableCell188_SummaryRowChanged(object sender, EventArgs e)
    {
        Emp_Id = GetCurrentColumnValue("Emp_Id").ToString();
        GrossAmount_Copy += Convert.ToDouble(GetCurrentColumnValue("AllowanceActAmt").ToString());
        CurrencySymbol_GrossCopy = GetCurrentColumnValue("Currency_Symbol").ToString();
    }

    private void xrPF_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //xrPF.Text = GetCurrentColumnValue("Currency_Symbol").ToString() + xrPF.Text;
    }

    private void xrESIC_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //xrESIC.Text = GetCurrentColumnValue("Currency_Symbol").ToString() + xrESIC.Text;
    }

    private void xrPF1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //xrPF1.Text = GetCurrentColumnValue("Currency_Symbol").ToString() + xrPF1.Text;
    }

    private void xrEsic1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //xrEsic1.Text = GetCurrentColumnValue("Currency_Symbol").ToString() + xrEsic1.Text;
    }
    
    
    
    

    private void xrTableCell194_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell194.Text = GetCurrentColumnValue("Currency_Symbol").ToString() + xrTableCell194.Text;
        //}
        //catch
        //{
        //}
    }

    private void xrTableCell204_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //try
        //{
        //    xrTableCell204.Text = GetCurrentColumnValue("Currency_Symbol").ToString() + xrTableCell204.Text;

        //}
        //catch
        //{
        //}
    }

    private void xrTableCell24_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        try
        {
            //if (xrPF.Text != "")
            //{
            //    netsalary = netsalary - double.Parse(xrPF.Text);

            //}

            //if (xrESIC.Text != "")
            //{
            //    netsalary = netsalary - double.Parse(xrESIC.Text);

            //}
        }
        catch
        {

        }
        //xrPF.Text = CurrencySymbol + xrPF.Text;
        //xrESIC.Text = CurrencySymbol + xrESIC.Text;

        e.Result = SystemParameter.GetCurrencySmbol(Common.GetEmployeeCurreny(Emp_Id,_strConString, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), objSysParam.GetCurencyConversionForInv(Common.GetEmployeeCurreny(Emp_Id,_strConString, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), netsalary.ToString()),_strConString);
        e.Handled = true;

    }

    private void xrTableCell24_SummaryReset(object sender, EventArgs e)
    {
        netsalary = 0;

    }

    private void xrTableCell24_SummaryRowChanged(object sender, EventArgs e)
    {
        if (GetCurrentColumnValue("NetSalary").ToString() != null && GetCurrentColumnValue("NetSalary").ToString().Trim() != "")
        {

            if (Convert.ToDouble(GetCurrentColumnValue("NetSalary").ToString()) != 0)
            {
                netsalary = Convert.ToDouble(GetCurrentColumnValue("NetSalary"));
            }
        }
        CurrencySymbol = GetCurrentColumnValue("Currency_Symbol").ToString();
        Emp_Id = GetCurrentColumnValue("Emp_Id").ToString();

    }

    private void xrTableCell197_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        try
        {
            //if (xrPF1.Text.Trim() != "")
            //{
            //    netsalary1 = netsalary1 - double.Parse(xrPF1.Text);
            //}

            //if (xrEsic1.Text.Trim() != "")
            //{
            //    netsalary1 = netsalary1 - double.Parse(xrEsic1.Text);
            //}
        }
        catch
        {

        }

        //xrPF1.Text = CurrencySymbol + xrPF1.Text;
        //xrEsic1.Text = CurrencySymbol + xrEsic1.Text;
        e.Result = SystemParameter.GetCurrencySmbol(Common.GetEmployeeCurreny(Emp_Id,_strConString, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), objSysParam.GetCurencyConversionForInv(Common.GetEmployeeCurreny(Emp_Id,_strConString, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), netsalary1.ToString()),_strConString);

        e.Handled = true;
    }

    private void xrTableCell197_SummaryReset(object sender, EventArgs e)
    {
        netsalary1 = 0;
    }

    private void xrTableCell197_SummaryRowChanged(object sender, EventArgs e)
    {
        if (GetCurrentColumnValue("NetSalary").ToString() != null && GetCurrentColumnValue("NetSalary").ToString() != "")
        {
            if (Convert.ToDouble(GetCurrentColumnValue("NetSalary").ToString()) != 0)
            {
                netsalary1 = Convert.ToDouble(GetCurrentColumnValue("NetSalary"));
            }
        }
        Emp_Id = GetCurrentColumnValue("Emp_Id").ToString();
        CurrencySymbol = GetCurrentColumnValue("Currency_Symbol").ToString();
    }

    private void xrTableCell49_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        try
        {
            //if (xrPF.Text != "")
            //{
            //    netsalary_EmployeeCurrency = netsalary_EmployeeCurrency - double.Parse(xrPF.Text);

            //}

            //if (xrESIC.Text != "")
            //{
            //    netsalary_EmployeeCurrency = netsalary_EmployeeCurrency - double.Parse(xrESIC.Text);
            //}
        }
        catch
        {

        }
        //xrPF.Text = CurrencySymbol + xrPF.Text;
        //xrESIC.Text = CurrencySymbol + xrESIC.Text;

        e.Result = objSysParam.GetCurencySymbol_For_EmployeeCurrency(Emp_Id, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + objSysParam.GetCurencyConversion_For_EmployeeCurrency(Emp_Id, netsalary_EmployeeCurrency.ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString() );
        e.Handled = true;
    }

    private void xrTableCell49_SummaryReset(object sender, EventArgs e)
    {
        netsalary_EmployeeCurrency = 0;
    }

    private void xrTableCell49_SummaryRowChanged(object sender, EventArgs e)
    {
        if (GetCurrentColumnValue("NetSalary").ToString() != null && GetCurrentColumnValue("NetSalary").ToString() != "")
        {
            if (Convert.ToDouble(GetCurrentColumnValue("NetSalary").ToString()) != 0)
            {
                netsalary_EmployeeCurrency = Convert.ToDouble(GetCurrentColumnValue("NetSalary"));
            }
        }
        Emp_Id = GetCurrentColumnValue("Emp_Id").ToString();
        CurrencySymbol = GetCurrentColumnValue("Currency_Symbol").ToString();
    }

    private void xrTableCell199_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
    {
        try
        {
            //if (xrPF1.Text.Trim() != "")
            //{
            //    netsalary_EmployeeCurrency1 = netsalary_EmployeeCurrency1 - double.Parse(xrPF1.Text);
            //}

            //if (xrEsic1.Text.Trim() != "")
            //{
            //    netsalary_EmployeeCurrency1 = netsalary_EmployeeCurrency1 - double.Parse(xrEsic1.Text);
            //}
        }
        catch
        {

        }

        //xrPF1.Text = CurrencySymbol + xrPF1.Text;
        //xrEsic1.Text = CurrencySymbol + xrEsic1.Text;
        e.Result = objSysParam.GetCurencySymbol_For_EmployeeCurrency(Emp_Id, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + objSysParam.GetCurencyConversion_For_EmployeeCurrency(Emp_Id, netsalary_EmployeeCurrency1.ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
        e.Handled = true;

    }

    private void xrTableCell199_SummaryReset(object sender, EventArgs e)
    {
        netsalary_EmployeeCurrency1 = 0;
    }

    private void xrTableCell199_SummaryRowChanged(object sender, EventArgs e)
    {
        if (GetCurrentColumnValue("NetSalary").ToString() != null && GetCurrentColumnValue("NetSalary").ToString() != "")
        {
            if (Convert.ToDouble(GetCurrentColumnValue("NetSalary").ToString()) != 0)
            {
                netsalary_EmployeeCurrency1 = Convert.ToDouble(GetCurrentColumnValue("NetSalary"));
            }
        }
        Emp_Id = GetCurrentColumnValue("Emp_Id").ToString();
        CurrencySymbol = GetCurrentColumnValue("Currency_Symbol").ToString();
    }
    
    private void xrLblAmountInWords_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        string CurrencyID = HttpContext.Current.Session["LocCurrencyId"].ToString();
        CurrencyMaster Objcurrency = new CurrencyMaster(_strConString);

        DataTable dtCurrency = Objcurrency.GetCurrencyMasterById(CurrencyID);
        string CurrencyName = dtCurrency.Rows[0]["Currency_Code"].ToString();


        string Amount = string.Empty;
        try
        {
            Amount = objSysParam.GetCurencyConversionForInv(CurrencyID.ToString(), GetCurrentColumnValue("Net_Salary").ToString());
        }
        catch
        {
        }


        string[] str = Amount.Split('.');

        string amountinwords = string.Empty;
        string amountinwordsAfterDecimal = string.Empty;
        try
        {
            amountinwords += AmountToWord(Convert.ToInt32(str[0].ToString()));

        }
        catch
        {
        }
        try
        {
            amountinwordsAfterDecimal += AmountToWord(Convert.ToInt32(str[1].ToString()));
        }
        catch
        {
        }

        if (amountinwordsAfterDecimal.Trim() == "" || amountinwordsAfterDecimal.Trim() == "Zero")
        {

            xrLblAmountInWords.Text = "In Words : (" + CurrencyName.ToUpper() + " " + amountinwords.ToString().ToUpper() + " ONLY" + ")";
        }
        else
        {

            xrLblAmountInWords.Text = "In Words : (" + CurrencyName.ToUpper() + " " + amountinwords.ToString().ToUpper() + "AND " + amountinwordsAfterDecimal.ToUpper() + " " + dtCurrency.Rows[0]["Field1"].ToString().ToUpper() + " ONLY" + ")";

        }


    }
    public static string AmountToWord(int number)
    {
        if (number == 0) return "Zero";

        if (number == -2147483648)
            return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";

        int[] num = new int[4];
        int first = 0;
        int u, h, t;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (number < 0)
        {
            sb.Append("Minus ");
            number = -number;
        }

        string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
"Five " ,"Six ", "Seven ", "Eight ", "Nine "};

        string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
"Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};

        string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
"Seventy ","Eighty ", "Ninety "};

        string[] words3 = { "Thousand ", "Lakh ", "Crore " };

        num[0] = number % 1000; // units
        num[1] = number / 1000;
        num[2] = number / 100000;
        num[1] = num[1] - 100 * num[2]; // thousands
        num[3] = number / 10000000; // crores
        num[2] = num[2] - 100 * num[3]; // lakhs

        //You can increase as per your need.

        for (int i = 3; i > 0; i--)
        {
            if (num[i] != 0)
            {
                first = i;
                break;
            }
        }


        for (int i = first; i >= 0; i--)
        {
            if (num[i] == 0) continue;

            u = num[i] % 10; // ones
            t = num[i] / 10;
            h = num[i] / 100; // hundreds
            t = t - 10 * h; // tens

            if (h > 0) sb.Append(words0[h] + "Hundred ");

            if (u > 0 || t > 0)
            {
                if (h == 0)
                    sb.Append("");
                else
                    if (h > 0 || i == 0) sb.Append("");


                if (t == 0)
                    sb.Append(words0[u]);
                else if (t == 1)
                    sb.Append(words1[u]);
                else
                    sb.Append(words2[t - 2] + words0[u]);
            }

            if (i != 0) sb.Append(words3[i - 1]);

        }
        return sb.ToString().TrimEnd() + " ";
    }

    private void xrLabel23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        //Common.Get_Roundoff_Amount_By_Location(xrLabel23.Text);
    }

    private void xrLabel23_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        
    }

    private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {

    }
}
