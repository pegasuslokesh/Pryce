using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

/// <summary>
/// Summary description for Employee_Total_Sal_Report
/// </summary>
public class Leave_Salary_Report : DevExpress.XtraReports.UI.XtraReport
{
    HR_Leave_Salary objLeaveSalary = null;
    Pay_Employee_claim objPayEmpClaim = null;
    Attendance objAtt = null;
    SystemParameter objsys = null;
    CurrencyMaster objCurrency = null;
	private DevExpress.XtraReports.UI.DetailBand Detail;
	private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
	private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private GroupHeaderBand GroupHeader1;
    private EmployeePaySlipDataSet employeePaySlipDataSet1;
    private GroupHeaderBand GroupHeader2;
    private XRPanel xrPanel1;
    private XRPictureBox xrPictureBox2;
    private XRTable xrTable1;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell1;
    private XRTableCell xrTableCell2;
    private XRTableCell xrTableCell3;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell4;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell6;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell10;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell9;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell8;
    private XRTableCell xrTableCell12;
    private XRTableRow xrTableRow5;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell xrTableCell15;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell16;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell18;
    private PageFooterBand PageFooter;
    private XRPageInfo xrPageInfo2;
    private ReportHeaderBand ReportHeader;
    private XRLabel xrLabel9;
    private XRPageInfo xrPageInfo3;
    private XRLabel xrLabel8;
    private XRPanel xrPanel2;
    private XRLabel xrLabel1;
    private XRLabel xrLabel2;
    private XRLabel xrLabel3;
    private XRLabel xrLabel4;
    private XRLabel xrLabel10;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel11;
    private XRLabel xrLabel15;
    private XRLabel xrLabel14;
    private XRLabel xrLabel20;
    private XRLabel xrLabel19;
    private XRLabel xrLabel12;
    private XRLabel xrLabel13;
    private XRTable XrLeaveDetail;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell19;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTable XrLeaveSalary;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell21;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell27;
    private XRTableCell xrTableCell24;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell30;
    private XRTable xrEmployeeClaim;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell29;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell33;
    private XRLabel xrLabel6;
    private XRLabel xrLabel5;
    private XRLabel xrLabel7;
    private XRTable xrTable2;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell32;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRLabel xrLabel16;
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

    public Leave_Salary_Report(string strConString)
    {
        InitializeComponent();
        objLeaveSalary = new HR_Leave_Salary(strConString);
        objPayEmpClaim = new Pay_Employee_claim(strConString);
        objAtt = new Attendance(strConString);
        objsys = new SystemParameter(strConString);
        objCurrency = new CurrencyMaster(strConString);
        //
        // TODO: Add constructor logic here
        //



    }
    public void setUserName(string UserName)
    {
        xrLabel8.Text = UserName;
    }
    //public void setimage(string url)
    //{
    //    xrPictureBox1.ImageUrl =      url;
    //    //xrPictureBox2.ImageUrl = url;
    //}
    public void setempimage(string urlemp)
    {
        xrPictureBox2.ImageUrl = urlemp;
        //xrPictureBox2.ImageUrl = url;
    }


    public void setcompanyname(string companyname)
    {
        xrLabel11.Text = companyname;
        // xrLabel13.Text = companyname;
    }
    public void setcompanyAddress(string Address)
    {
        xrLabel10.Text = Address;
        // xrLabel12.Text = Address;
    }
    public void setcontact(string Contact)
    {
        xrLabel4.Text = Contact;
        // xrLabel11.Text = Contact;
    }

    public void setwebsite(string web)
    {
        xrLabel2.Text = web;

    }
    public void setmailid(string mailid)
    {
        xrLabel1.Text = mailid;

    }
    public void setheader(string CreatedBy,string PayslipForthemonth,string employeecode,string employeename,string designation,string Department,string Doj,string bankAc,string Salary,string allowances,string deduction,string penalty,string Claim,string Loan,string Attandance,string Grosspay,string Previousmonthadjustment )
    {
        //xrLabel16.Text = Resources.Attendance.Department;


    }

    public void set_Designation_name(string str_desg)
    {
        xrTableCell9.Text = str_desg;
    }
    public void set_Department_name(string str_depart)
    {
        xrTableCell12.Text = str_depart;
    }
    public void set_Basic_Salary(string str_Basic)
    {
        xrTableCell18.Text = str_Basic;
    }
    public void setBrand(string BrandName)
    {
        xrLabel15.Text = BrandName;
    }
    public void setLocation(string LocationName)
    {
        xrLabel14.Text = LocationName;
    }
    public void setDepartment(string DepartmentName)
    {
        //xrLabel18.Text = DepartmentName;
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
            string resourceFileName = "LeaveSalaryReport.resx";
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrEmployeeClaim = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrLeaveSalary = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.XrLeaveDetail = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.employeePaySlipDataSet1 = new EmployeePaySlipDataSet();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrEmployeeClaim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XrLeaveSalary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XrLeaveDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeePaySlipDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseFont = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 1F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xrLabel6,
            this.xrLabel5,
            this.xrEmployeeClaim,
            this.XrLeaveSalary,
            this.XrLeaveDetail});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("EmpCode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 331.6429F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader1_BeforePrint);
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(10F, 223F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow13,
            this.xrTableRow14});
            this.xrTable2.SizeF = new System.Drawing.SizeF(786.0002F, 50F);
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell32});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.BackColor = System.Drawing.Color.Black;
            this.xrTableCell32.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrTableCell32.ForeColor = System.Drawing.Color.White;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.StylePriority.UseBackColor = false;
            this.xrTableCell32.StylePriority.UseFont = false;
            this.xrTableCell32.StylePriority.UseForeColor = false;
            this.xrTableCell32.StylePriority.UseTextAlignment = false;
            this.xrTableCell32.Text = "LAST LEAVE SALARY DETAIL";
            this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell32.Weight = 3D;
            this.xrTableCell32.WordWrap = false;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell34,
            this.xrTableCell35,
            this.xrTableCell36,
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell39});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.StylePriority.UseBackColor = false;
            this.xrTableCell34.StylePriority.UseTextAlignment = false;
            this.xrTableCell34.Text = "Month";
            this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell34.Weight = 0.45036203507004718D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.StylePriority.UseBackColor = false;
            this.xrTableCell35.StylePriority.UseTextAlignment = false;
            this.xrTableCell35.Text = "Year";
            this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell35.Weight = 0.49574477899105179D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.StylePriority.UseBackColor = false;
            this.xrTableCell36.StylePriority.UseTextAlignment = false;
            this.xrTableCell36.Text = "Leave Type";
            this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell36.Weight = 0.56473746502418443D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.StylePriority.UseBackColor = false;
            this.xrTableCell37.StylePriority.UseTextAlignment = false;
            this.xrTableCell37.Text = "Leave Count";
            this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell37.Weight = 0.530248125324249D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.StylePriority.UseBackColor = false;
            this.xrTableCell38.StylePriority.UseTextAlignment = false;
            this.xrTableCell38.Text = "Per Day Salary";
            this.xrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell38.Weight = 0.5324127907992553D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.StylePriority.UseBackColor = false;
            this.xrTableCell39.StylePriority.UseTextAlignment = false;
            this.xrTableCell39.Text = "Total";
            this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell39.Weight = 0.426494804791212D;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(644.4168F, 297.8096F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(149.8749F, 23F);
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrLabel6.Visible = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(476.4052F, 297.8096F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(168.0116F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "Total Pending Claim   :";
            this.xrLabel5.Visible = false;
            // 
            // xrEmployeeClaim
            // 
            this.xrEmployeeClaim.LocationFloat = new DevExpress.Utils.PointFloat(9.999879F, 149.0833F);
            this.xrEmployeeClaim.Name = "xrEmployeeClaim";
            this.xrEmployeeClaim.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow11,
            this.xrTableRow12});
            this.xrEmployeeClaim.SizeF = new System.Drawing.SizeF(786.0002F, 50F);
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell29});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.BackColor = System.Drawing.Color.Black;
            this.xrTableCell29.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrTableCell29.ForeColor = System.Drawing.Color.White;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseBackColor = false;
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.StylePriority.UseForeColor = false;
            this.xrTableCell29.StylePriority.UseTextAlignment = false;
            this.xrTableCell29.Text = "TAKEN LEAVE SALARY";
            this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell29.Weight = 8.0374016924616889D;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell31,
            this.xrTableCell33});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.StylePriority.UseBackColor = false;
            this.xrTableCell31.StylePriority.UseTextAlignment = false;
            this.xrTableCell31.Text = "DATE";
            this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell31.Weight = 4.0477542779538318D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.StylePriority.UseBackColor = false;
            this.xrTableCell33.StylePriority.UseTextAlignment = false;
            this.xrTableCell33.Text = "AMOUNT";
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell33.Weight = 3.9896474145078571D;
            // 
            // XrLeaveSalary
            // 
            this.XrLeaveSalary.LocationFloat = new DevExpress.Utils.PointFloat(9.999895F, 78.0179F);
            this.XrLeaveSalary.Name = "XrLeaveSalary";
            this.XrLeaveSalary.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow9,
            this.xrTableRow10});
            this.XrLeaveSalary.SizeF = new System.Drawing.SizeF(786.0002F, 50F);
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell21});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.BackColor = System.Drawing.Color.Black;
            this.xrTableCell21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrTableCell21.ForeColor = System.Drawing.Color.White;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseBackColor = false;
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UseForeColor = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "PENDING LEAVE SALARY";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell21.Weight = 3D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCell24,
            this.xrTableCell28,
            this.xrTableCell30});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseBackColor = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "Month";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell25.Weight = 0.45036203507004718D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseBackColor = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = "Year";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell26.Weight = 0.49574477899105179D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.StylePriority.UseBackColor = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.Text = "Leave Type";
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell27.Weight = 0.56473746502418443D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseBackColor = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "Leave Count";
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell24.Weight = 0.530248125324249D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseBackColor = false;
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "Per Day Salary";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell28.Weight = 0.5324127907992553D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.StylePriority.UseBackColor = false;
            this.xrTableCell30.StylePriority.UseTextAlignment = false;
            this.xrTableCell30.Text = "Total";
            this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell30.Weight = 0.426494804791212D;
            // 
            // XrLeaveDetail
            // 
            this.XrLeaveDetail.LocationFloat = new DevExpress.Utils.PointFloat(9.999887F, 10.00001F);
            this.XrLeaveDetail.Name = "XrLeaveDetail";
            this.XrLeaveDetail.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow8});
            this.XrLeaveDetail.SizeF = new System.Drawing.SizeF(786.0002F, 50F);
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.BackColor = System.Drawing.Color.Black;
            this.xrTableCell19.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrTableCell19.ForeColor = System.Drawing.Color.White;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseBackColor = false;
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseForeColor = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.Text = "LEAVE DETAIL";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell19.Weight = 3D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell20,
            this.xrTableCell22,
            this.xrTableCell23});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseBackColor = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "From Date";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell20.Weight = 1.2133072217851662D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseBackColor = false;
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "To Date";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell22.Weight = 1.0366927782148339D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.BackColor = System.Drawing.Color.Silver;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseBackColor = false;
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "Rejoin Date";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell23.Weight = 0.75D;
            // 
            // employeePaySlipDataSet1
            // 
            this.employeePaySlipDataSet1.DataSetName = "EmployeePaySlipDataSet";
            this.employeePaySlipDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel1});
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("EmpCode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.HeightF = 118.9584F;
            this.GroupHeader2.Level = 1;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // xrPanel1
            // 
            this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2,
            this.xrTable1});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 0F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(786F, 118.9584F);
            this.xrPanel1.StylePriority.UseBorders = false;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(700.3F, 9.337807F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(75.7F, 70.83F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox2.StylePriority.UseBorders = false;
            this.xrPictureBox2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPictureBox2_BeforePrint);
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(9.916742F, 10.00001F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2,
            this.xrTableRow4,
            this.xrTableRow3,
            this.xrTableRow5,
            this.xrTableRow6});
            this.xrTable1.SizeF = new System.Drawing.SizeF(632.5046F, 98.95838F);
            this.xrTable1.StylePriority.UseBorders = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.Text = "EMPLOYEE CODE";
            this.xrTableCell1.Weight = 0.25692606735088941D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Text = ":";
            this.xrTableCell2.Weight = 0.025509254301800344D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_EmployeeMaster_SelectRow.Emp_Code")});
            this.xrTableCell3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.Weight = 0.2452364633721415D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.Text = "EMPLOYEE NAME";
            this.xrTableCell4.Weight = 0.25692606532795614D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = ":";
            this.xrTableCell5.Weight = 0.026470578361171504D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_EmployeeMaster_SelectRow.Emp_Name")});
            this.xrTableCell6.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.Weight = 0.24427514133570366D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell9});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.Text = "DESIGNATION";
            this.xrTableCell10.Weight = 0.25692606532795614D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = ":";
            this.xrTableCell11.Weight = 0.026470578361171504D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.Weight = 0.24427514133570366D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell8,
            this.xrTableCell12});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.Text = "DEPARTMENT";
            this.xrTableCell7.Weight = 0.25692606532795614D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = ":";
            this.xrTableCell8.Weight = 0.026470578361171504D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.Weight = 0.24427514133570366D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell15});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.Text = "DOJ";
            this.xrTableCell13.Weight = 0.25692606532795614D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = ":";
            this.xrTableCell14.Weight = 0.026470578361171504D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Set_EmployeeMaster_SelectRow.DOJ")});
            this.xrTableCell15.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.Weight = 0.24427514133570366D;
            this.xrTableCell15.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell15_BeforePrint);
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell16,
            this.xrTableCell17,
            this.xrTableCell18});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.Text = "BASIC SALARY";
            this.xrTableCell16.Weight = 0.25692606532795614D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Text = ":";
            this.xrTableCell17.Weight = 0.026470578361171504D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.Weight = 0.24427514133570366D;
            this.xrTableCell18.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell18_BeforePrint);
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel16,
            this.xrPageInfo2});
            this.PageFooter.HeightF = 85.50002F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel16
            // 
            this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(9.999879F, 38.50002F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(784.2918F, 23F);
            this.xrLabel16.StylePriority.UseBorders = false;
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "EMPLOYEE  SIGNATURE                                          ACCOUNTANT SIGNATURE" +
    "                                        MANAGER SIGNATURE";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo2
            // 
            this.xrPageInfo2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPageInfo2.Format = "Page{0}";
            this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(311.9583F, 66.66666F);
            this.xrPageInfo2.Name = "xrPageInfo2";
            this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo2.SizeF = new System.Drawing.SizeF(163.875F, 18.83335F);
            this.xrPageInfo2.StylePriority.UseBorders = false;
            this.xrPageInfo2.StylePriority.UseTextAlignment = false;
            this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.xrPageInfo3,
            this.xrLabel8,
            this.xrPanel2});
            this.ReportHeader.HeightF = 87.66663F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(9.999886F, 0F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(81.07728F, 15.70833F);
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Created By:";
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPageInfo3.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(558.4583F, 0F);
            this.xrPageInfo3.Name = "xrPageInfo3";
            this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
            this.xrPageInfo3.SizeF = new System.Drawing.SizeF(235.3911F, 15.70833F);
            this.xrPageInfo3.StylePriority.UseBorders = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            this.xrPageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(91.07713F, 0F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(166.8028F, 15.70833F);
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "xrLabel10";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel2
            // 
            this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel7,
            this.xrLabel15,
            this.xrLabel14,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel12,
            this.xrLabel13,
            this.xrLabel1,
            this.xrLabel2,
            this.xrLabel3,
            this.xrLabel4,
            this.xrLabel10,
            this.xrPictureBox1,
            this.xrLabel11});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 15.70835F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(786F, 71.95827F);
            this.xrPanel2.StylePriority.UseBorders = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(295.2949F, 26.99995F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(223.6635F, 23F);
            this.xrLabel7.StylePriority.UseBorders = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UsePadding = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "LEAVE SALARY REPORT";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel15
            // 
            this.xrLabel15.BorderColor = System.Drawing.Color.Empty;
            this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(91.95311F, 53.99995F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(135.0051F, 14.24998F);
            this.xrLabel15.StylePriority.UseBorderColor = false;
            this.xrLabel15.StylePriority.UseBorders = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.Text = "xrLabel10";
            // 
            // xrLabel14
            // 
            this.xrLabel14.BorderColor = System.Drawing.Color.Empty;
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(301.9583F, 53.9999F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(217F, 14.24999F);
            this.xrLabel14.StylePriority.UseBorderColor = false;
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.Text = "xrLabel10";
            // 
            // xrLabel20
            // 
            this.xrLabel20.BorderColor = System.Drawing.Color.Empty;
            this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(2F, 53.99992F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(77.44597F, 14.24998F);
            this.xrLabel20.StylePriority.UseBorderColor = false;
            this.xrLabel20.StylePriority.UseBorders = false;
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.Text = "Brand";
            // 
            // xrLabel19
            // 
            this.xrLabel19.BorderColor = System.Drawing.Color.Empty;
            this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(79.44597F, 53.99995F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(12.50714F, 14.24998F);
            this.xrLabel19.StylePriority.UseBorderColor = false;
            this.xrLabel19.StylePriority.UseBorders = false;
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.Text = ":";
            // 
            // xrLabel12
            // 
            this.xrLabel12.BorderColor = System.Drawing.Color.Empty;
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(289.4583F, 53.99992F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(11.83331F, 14.24998F);
            this.xrLabel12.StylePriority.UseBorderColor = false;
            this.xrLabel12.StylePriority.UseBorders = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = ":";
            // 
            // xrLabel13
            // 
            this.xrLabel13.BorderColor = System.Drawing.Color.Empty;
            this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(226.9582F, 53.99992F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(62.50003F, 14.24998F);
            this.xrLabel13.StylePriority.UseBorderColor = false;
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.Text = "Location";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(2.916618F, 41.66667F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(204.2499F, 10.49997F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(570.8334F, 57.74992F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(205.1666F, 10.49998F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(2.916618F, 26.99995F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(59.99496F, 14.66664F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Phone No :";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(63.25172F, 27.00002F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(131.9565F, 14.66664F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UseFont = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(2.916615F, 16.58335F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(314.9698F, 10.41668F);
            this.xrLabel10.StylePriority.UseBorders = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(703.2081F, 7.50005F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(72.79193F, 37.66659F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            this.xrPictureBox1.StylePriority.UseBorders = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(2.916622F, 3.999997F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(314.9698F, 12.58334F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // Leave_Salary_Report
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.GroupHeader2,
            this.PageFooter,
            this.ReportHeader});
            this.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.DataMember = "sp_Set_EmployeeMaster_SelectRow";
            this.DataSource = this.employeePaySlipDataSet1;
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(24, 30, 0, 1);
            this.Version = "14.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrEmployeeClaim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XrLeaveSalary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XrLeaveDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeePaySlipDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

	}

	#endregion

    private void xrPictureBox2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            if (GetCurrentColumnValue("Emp_Image").ToString() != null)
            {
                xrPictureBox2.ImageUrl = "~/CompanyResource/" + GetCurrentColumnValue("Company_Id").ToString() + "/" + GetCurrentColumnValue("Emp_Image").ToString();
            }
        }
        catch
        {
        }
    }

    private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
      


        DataTable dt = objAtt.GetLeaveRequestByEmpId(GetCurrentColumnValue("Company_Id").ToString(), GetCurrentColumnValue("Emp_Id").ToString());
        if (dt.Rows.Count > 0)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {


                XRTableRow dr = new XRTableRow();
                XrLeaveDetail.Rows.Add(dr);
                XRTableCell FromDate = new XRTableCell();
                XRTableCell ToDate = new XRTableCell();
                XRTableCell RejoinDate = new XRTableCell();

                FromDate.Text = Convert.ToDateTime(dt.Rows[i]["From_Date"].ToString()).ToString(objsys.SetDateFormat());




                FromDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    FromDate.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }

                ToDate.Text = Convert.ToDateTime(dt.Rows[i]["To_Date"].ToString()).ToString(objsys.SetDateFormat());




                ToDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    ToDate.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }


                RejoinDate.Text = Convert.ToDateTime(dt.Rows[i]["RejoiningDate"].ToString()).ToString(objsys.SetDateFormat());




                RejoinDate.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    RejoinDate.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }



                dr.Cells.Add(FromDate);
                dr.Cells.Add(ToDate);
                dr.Cells.Add(RejoinDate);

                XrLeaveDetail.Rows[dr.Index].Cells[0].SizeF = new SizeF(317.89F, 25F);
                XrLeaveDetail.Rows[dr.Index].Cells[1].SizeF = new SizeF(271.61F, 25F);
                XrLeaveDetail.Rows[dr.Index].Cells[2].SizeF = new SizeF(196.5F, 25F);

                XrLeaveDetail.Rows[dr.Index].Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                XrLeaveDetail.Rows[dr.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                XrLeaveDetail.Rows[dr.Index].Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;

            }
        }
        else
        {
            XrLeaveDetail.Visible = false;
        }

        //this code for retrieve leave salary

        //code start
        float TotalSum = 0;
        float LeaveSum = 0;
        float NetLeave = 0;
        float NetSalary = 0;
       
        dt = objLeaveSalary.GetAllLeaveSalaryByEmpID(GetCurrentColumnValue("Emp_Id").ToString());
       
        if (dt.Rows.Count > 0)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {



               

                   

                    try
                    {


                        if (i > 0)
                        {

                            if (dt.Rows[i]["Per_Day_Salary"].ToString() != dt.Rows[i - 1]["Per_Day_Salary"].ToString())
                            {

                                XRTableRow drGroup = new XRTableRow();
                                XrLeaveSalary.Rows.Add(drGroup);
                                XRTableCell TotalName = new XRTableCell();
                                XRTableCell TotalLeave = new XRTableCell();
                                XRTableCell sumLeaveSalary = new XRTableCell();


                                TotalName.Text = "Total";




                                TotalName.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                                try
                                {
                                    TotalName.Borders = DevExpress.XtraPrinting.BorderSide.All;
                                }
                                catch
                                {
                                }
                                TotalName.Font = new System.Drawing.Font("Times New Roman",10F, System.Drawing.FontStyle.Bold);

                                TotalLeave.Text = LeaveSum.ToString();




                                TotalLeave.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                                try
                                {
                                    TotalLeave.Borders = DevExpress.XtraPrinting.BorderSide.All;
                                }
                                catch
                                {
                                }
                                TotalLeave.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);


                                sumLeaveSalary.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), TotalSum.ToString());




                                sumLeaveSalary.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                                try
                                {
                                    sumLeaveSalary.Borders = DevExpress.XtraPrinting.BorderSide.All;
                                }
                                catch
                                {
                                }
                                sumLeaveSalary.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);


                                drGroup.Cells.Add(TotalName);
                                drGroup.Cells.Add(TotalLeave);
                                drGroup.Cells.Add(sumLeaveSalary);

                                XrLeaveSalary.Rows[drGroup.Index].Cells[0].SizeF = new SizeF(395.84F, 25F);
                                XrLeaveSalary.Rows[drGroup.Index].Cells[1].SizeF = new SizeF(138.93F, 25F);
                                XrLeaveSalary.Rows[drGroup.Index].Cells[2].SizeF = new SizeF(251.23F, 25F);


                                XrLeaveSalary.Rows[drGroup.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                                XrLeaveSalary.Rows[drGroup.Index].Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                                LeaveSum = 0;
                                TotalSum = 0;


                            }
                        }
                    }
                    catch
                    {


                    }
                













                XRTableRow dr = new XRTableRow();
                XrLeaveSalary.Rows.Add(dr);
                XRTableCell Month = new XRTableCell();
                XRTableCell Year = new XRTableCell();
                XRTableCell LeaveType = new XRTableCell();
                XRTableCell LeaveCount = new XRTableCell();
                XRTableCell PerDaySalary = new XRTableCell();
                XRTableCell Total = new XRTableCell();

                Month.Text = dt.Rows[i]["Month_Name"].ToString();

                Month.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    Month.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }




                Year.Text = dt.Rows[i]["L_Year"].ToString();

                Year.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    Year.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }



                LeaveType.Text = dt.Rows[i]["LeaveTypeName"].ToString();
                LeaveType.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    LeaveType.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }
                NetLeave+= float.Parse(dt.Rows[i]["F4"].ToString());
                LeaveSum += float.Parse(dt.Rows[i]["F4"].ToString());
                LeaveCount.Text = dt.Rows[i]["F4"].ToString();
                LeaveCount.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    LeaveCount.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }
                
                PerDaySalary.Text =    objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(),dt.Rows[i]["Per_Day_Salary"].ToString());
                PerDaySalary.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    PerDaySalary.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }



                NetSalary += float.Parse(objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), dt.Rows[i]["Total"].ToString()));
                TotalSum += float.Parse(objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), dt.Rows[i]["Total"].ToString()));
                Total.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(),dt.Rows[i]["Total"].ToString());
                Total.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    Total.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }




                dr.Cells.Add(Month);
                dr.Cells.Add(Year);
                dr.Cells.Add(LeaveType);
                dr.Cells.Add(LeaveCount);
                dr.Cells.Add(PerDaySalary);
                dr.Cells.Add(Total);

                XrLeaveSalary.Rows[dr.Index].Cells[0].SizeF = new SizeF(117.99F, 25F);
                XrLeaveSalary.Rows[dr.Index].Cells[1].SizeF = new SizeF(129.89F, 25F);
                XrLeaveSalary.Rows[dr.Index].Cells[2].SizeF = new SizeF(147.96F, 25F);
                XrLeaveSalary.Rows[dr.Index].Cells[3].SizeF = new SizeF(138.93F, 25F);
                XrLeaveSalary.Rows[dr.Index].Cells[4].SizeF = new SizeF(139.49F, 25F);
                XrLeaveSalary.Rows[dr.Index].Cells[5].SizeF = new SizeF(111.74F, 25F);

                XrLeaveSalary.Rows[dr.Index].Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                XrLeaveSalary.Rows[dr.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                XrLeaveSalary.Rows[dr.Index].Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                XrLeaveSalary.Rows[dr.Index].Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                XrLeaveSalary.Rows[dr.Index].Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                XrLeaveSalary.Rows[dr.Index].Cells[5].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

               


            }


            XRTableRow drFooter = new XRTableRow();
            XrLeaveSalary.Rows.Add(drFooter);
            XRTableCell TotalFooter = new XRTableCell();
            XRTableCell TotalFooterLeave = new XRTableCell();
            XRTableCell TotalFooterSalary = new XRTableCell();


            TotalFooter.Text = "Net Total";




            TotalFooter.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            try
            {
                TotalFooter.Borders = DevExpress.XtraPrinting.BorderSide.All;
            }
            catch
            {
            }


            TotalFooter.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);






            TotalFooterLeave.Text = NetLeave.ToString();




            TotalFooterLeave.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            try
            {
                TotalFooterLeave.Borders = DevExpress.XtraPrinting.BorderSide.All;
            }
            catch
            {
            }
            TotalFooterLeave.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);



            TotalFooterSalary.Text =  objCurrency.GetCurrencyMasterById(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString()).Rows[0]["Currency_Symbol"].ToString()+" "+objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), NetSalary.ToString());




            TotalFooterSalary.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            try
            {
                TotalFooterSalary.Borders = DevExpress.XtraPrinting.BorderSide.All;
            }
            catch
            {
            }
            TotalFooterSalary.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);



            drFooter.Cells.Add(TotalFooter);
            drFooter.Cells.Add(TotalFooterLeave);
            drFooter.Cells.Add(TotalFooterSalary);

            XrLeaveSalary.Rows[drFooter.Index].Cells[0].SizeF = new SizeF(395.84F, 25F);
            XrLeaveSalary.Rows[drFooter.Index].Cells[1].SizeF = new SizeF(138.93F, 25F);
            XrLeaveSalary.Rows[drFooter.Index].Cells[2].SizeF = new SizeF(251.23F, 25F);


            XrLeaveSalary.Rows[drFooter.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            XrLeaveSalary.Rows[drFooter.Index].Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;


        }
        else
        {
            XrLeaveSalary.Visible = false;
        }
        
        //code end

        //this code for showing employee claim
        float NetClaim = 0;
        dt = objLeaveSalary.GetAllLeaveSalaryByEmpID_LeaveTakenNew(GetCurrentColumnValue("Emp_Id").ToString());

       

        if (dt.Rows.Count > 0)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {


                XRTableRow dr = new XRTableRow();
                xrEmployeeClaim.Rows.Add(dr);
                XRTableCell Date = new XRTableCell();
               
                XRTableCell Value = new XRTableCell();

                Date.Text = Convert.ToDateTime(dt.Rows[i]["F1"].ToString()).ToString(objsys.SetDateFormat()) ;




                Date.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    Date.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }

               




                Value.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                try
                {
                    Value.Borders = DevExpress.XtraPrinting.BorderSide.All;
                }
                catch
                {
                }


                Value.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), dt.Rows[i]["Total"].ToString());


                NetClaim += float.Parse(objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), dt.Rows[i]["Total"].ToString()));





                dr.Cells.Add(Date);
              
                dr.Cells.Add(Value);

                xrEmployeeClaim.Rows[dr.Index].Cells[0].SizeF = new SizeF(395.84F, 25F);
                xrEmployeeClaim.Rows[dr.Index].Cells[1].SizeF = new SizeF(390.16F, 25F);
               

                xrEmployeeClaim.Rows[dr.Index].Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                xrEmployeeClaim.Rows[dr.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                if (i == 0)
                {
                    DateTime dtDate = Convert.ToDateTime(Date.Text);
                 
                  
                }

            }

            TakenLeaveSalaryDetail(DateTime.Now, NetClaim.ToString());



            XRTableRow drFooter = new XRTableRow();
            xrEmployeeClaim.Rows.Add(drFooter);
            XRTableCell TotalFooter = new XRTableCell();
            XRTableCell TotalFooterClaim = new XRTableCell();


            TotalFooter.Text = "Total";




            TotalFooter.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            try
            {
                TotalFooter.Borders = DevExpress.XtraPrinting.BorderSide.All;
            }
            catch
            {
            }


            TotalFooter.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);


            TotalFooterClaim.Text =objCurrency.GetCurrencyMasterById(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString()).Rows[0]["Currency_Symbol"].ToString()+" "+objsys.GetCurencyConversionForInv( System.Web.HttpContext.Current.Session["CurrencyId"].ToString(),NetClaim.ToString());




            TotalFooterClaim.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
            try
            {
                TotalFooterClaim.Borders = DevExpress.XtraPrinting.BorderSide.All;
            }
            catch
            {
            }

            TotalFooterClaim.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);
           



            drFooter.Cells.Add(TotalFooter);
            drFooter.Cells.Add(TotalFooterClaim);


            xrEmployeeClaim.Rows[drFooter.Index].Cells[0].SizeF = new SizeF(395.84F, 25F);
            xrEmployeeClaim.Rows[drFooter.Index].Cells[1].SizeF = new SizeF(390.16F, 25F);
         


            xrEmployeeClaim.Rows[drFooter.Index].Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            xrEmployeeClaim.Rows[drFooter.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;



        }
        else
        {
            xrEmployeeClaim.Visible = false;
            xrTable2.Visible = false;
        }
     
        
        //xrLabel6.Text = objCurrency.GetCurrencyMasterById(System.Web.HttpContext.Current.Session["CurrencyId"].ToString()).Rows[0]["Currency_Symbol"].ToString() +" "+ objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), (NetSalary - NetClaim).ToString());
    
    }
    public void TakenLeaveSalaryDetail(DateTime dt,string NetGivenAmount)
    {

        double NetSalary = 0;
        xrTableCell32.Text = "LAST LEAVE SALARY DETAIL";
         DataTable dtTakenLeave = objLeaveSalary.GetAllLeaveSalaryAllRecord_ByEmpID(GetCurrentColumnValue("Emp_Id").ToString());
         //////try
         //////{
         //////    dtTakenLeave = new DataView(dtTakenLeave, "F1='" + dt.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
         //////}
         //////catch
         //////{
         //////}

         if (dtTakenLeave.Rows.Count > 0)
          {
              xrTable2.Visible = true;
              for (int i = 0; i < dtTakenLeave.Rows.Count; i++)
              {
                  XRTableRow dr = new XRTableRow();
                  xrTable2.Rows.Add(dr);
                  XRTableCell Month = new XRTableCell();
                  XRTableCell Year = new XRTableCell();
                  XRTableCell LeaveType = new XRTableCell();
                  XRTableCell LeaveCount = new XRTableCell();
                  XRTableCell PerDaySalary = new XRTableCell();
                  XRTableCell Total = new XRTableCell();

                  Month.Text = dtTakenLeave.Rows[i]["Month_Name"].ToString();

                  Month.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                  try
                  {
                      Month.Borders = DevExpress.XtraPrinting.BorderSide.All;
                  }
                  catch
                  {
                  }




                  Year.Text = dtTakenLeave.Rows[i]["L_Year"].ToString();

                  Year.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                  try
                  {
                      Year.Borders = DevExpress.XtraPrinting.BorderSide.All;
                  }
                  catch
                  {
                  }



                  LeaveType.Text = dtTakenLeave.Rows[i]["LeaveTypeName"].ToString();
                  LeaveType.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                  try
                  {
                      LeaveType.Borders = DevExpress.XtraPrinting.BorderSide.All;
                  }
                  catch
                  {
                  }
                  LeaveCount.Text = dtTakenLeave.Rows[i]["F4"].ToString();
                  LeaveCount.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                  try
                  {
                      LeaveCount.Borders = DevExpress.XtraPrinting.BorderSide.All;
                  }
                  catch
                  {
                  }

                  PerDaySalary.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), dtTakenLeave.Rows[i]["Per_Day_Salary"].ToString());
                  PerDaySalary.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                  try
                  {
                      PerDaySalary.Borders = DevExpress.XtraPrinting.BorderSide.All;
                  }
                  catch
                  {
                  }



                  Total.Text = objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), dtTakenLeave.Rows[i]["Total"].ToString());
                  Total.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
                  try
                  {
                      Total.Borders = DevExpress.XtraPrinting.BorderSide.All;
                  }
                  catch
                  {
                  }

                  NetSalary+= Convert.ToDouble(Total.Text);


                  dr.Cells.Add(Month);
                  dr.Cells.Add(Year);
                  dr.Cells.Add(LeaveType);
                  dr.Cells.Add(LeaveCount);
                  dr.Cells.Add(PerDaySalary);
                  dr.Cells.Add(Total);

                  xrTable2.Rows[dr.Index].Cells[0].SizeF = new SizeF(117.99F, 25F);
                  xrTable2.Rows[dr.Index].Cells[1].SizeF = new SizeF(129.89F, 25F);
                  xrTable2.Rows[dr.Index].Cells[2].SizeF = new SizeF(147.96F, 25F);
                  xrTable2.Rows[dr.Index].Cells[3].SizeF = new SizeF(138.93F, 25F);
                  xrTable2.Rows[dr.Index].Cells[4].SizeF = new SizeF(139.49F, 25F);
                  xrTable2.Rows[dr.Index].Cells[5].SizeF = new SizeF(111.74F, 25F);

                  xrTable2.Rows[dr.Index].Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                  xrTable2.Rows[dr.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                  xrTable2.Rows[dr.Index].Cells[2].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                  xrTable2.Rows[dr.Index].Cells[3].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                  xrTable2.Rows[dr.Index].Cells[4].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
                  xrTable2.Rows[dr.Index].Cells[5].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

              }

              XRTableRow drFooter = new XRTableRow();
              xrTable2.Rows.Add(drFooter);
            
              XRTableCell TotalFooterLeave = new XRTableCell();
              XRTableCell TotalFooterSalary = new XRTableCell();








              TotalFooterLeave.Text = "Total";




              TotalFooterLeave.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
              try
              {
                  TotalFooterLeave.Borders = DevExpress.XtraPrinting.BorderSide.All;
              }
              catch
              {
              }
              TotalFooterLeave.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);



              TotalFooterSalary.Text = objCurrency.GetCurrencyMasterById(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString()).Rows[0]["Currency_Symbol"].ToString() + " " + objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), NetSalary.ToString());




              TotalFooterSalary.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
              try
              {
                  TotalFooterSalary.Borders = DevExpress.XtraPrinting.BorderSide.All;
              }
              catch
              {
              }
              TotalFooterSalary.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);



           
              drFooter.Cells.Add(TotalFooterLeave);
              drFooter.Cells.Add(TotalFooterSalary);


              xrTable2.Rows[drFooter.Index].Cells[0].SizeF = new SizeF(395.84F, 25F);
              xrTable2.Rows[drFooter.Index].Cells[1].SizeF = new SizeF(390.16F, 25F);



              xrTable2.Rows[drFooter.Index].Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
              xrTable2.Rows[drFooter.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;


            //this row add for showing give amount row
              XRTableRow drgiveamount = new XRTableRow();
              xrTable2.Rows.Add(drgiveamount);

              XRTableCell TotalgivenLeave = new XRTableCell();
              XRTableCell TotalgivenSalary = new XRTableCell();


              TotalgivenLeave.Text = "Given Amount";




              TotalgivenLeave.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
              try
              {
                  TotalgivenLeave.Borders = DevExpress.XtraPrinting.BorderSide.All;
              }
              catch
              {
              }
              TotalgivenLeave.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);



              TotalgivenSalary.Text = objCurrency.GetCurrencyMasterById(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString()).Rows[0]["Currency_Symbol"].ToString() + " " + NetGivenAmount.ToString();




              TotalgivenSalary.BorderColor = System.Drawing.ColorTranslator.FromHtml("#000000");
              try
              {
                  TotalgivenSalary.Borders = DevExpress.XtraPrinting.BorderSide.All;
              }
              catch
              {
              }
              TotalgivenSalary.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Bold);




              drgiveamount.Cells.Add(TotalgivenLeave);
              drgiveamount.Cells.Add(TotalgivenSalary);


              xrTable2.Rows[drgiveamount.Index].Cells[0].SizeF = new SizeF(395.84F, 25F);
              xrTable2.Rows[drgiveamount.Index].Cells[1].SizeF = new SizeF(390.16F, 25F);



              xrTable2.Rows[drgiveamount.Index].Cells[0].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
              xrTable2.Rows[drgiveamount.Index].Cells[1].TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;

              

          }


    }

    private void xrTableCell15_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell15.Text = Convert.ToDateTime(xrTableCell15.Text).ToString(objsys.SetDateFormat());
        }
        catch
        {
        }
    }

    private void xrTableCell18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell18.Text = objCurrency.GetCurrencyMasterById(System.Web.HttpContext.Current.Session["LocCurrencyId"].ToString()).Rows[0]["Currency_Symbol"].ToString() + " " + objsys.GetCurencyConversionForInv(System.Web.HttpContext.Current.Session["CurrencyId"].ToString(), xrTableCell18.Text);

        }

        catch
        {
        }
        }
}
