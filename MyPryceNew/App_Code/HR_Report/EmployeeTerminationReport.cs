using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Web;

/// <summary>
/// Summary description for Employee_Pay_Slip_Report
/// </summary>
public class EmployeeTerminationReport : DevExpress.XtraReports.UI.XtraReport
{
    SystemParameter objSysParam = null;
	private DevExpress.XtraReports.UI.DetailBand Detail;
	private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private EmployeePaySlipDataSet employeePaySlipDataSet1;
    private EmployeeTerminationDataset employeeterminationDataSet1;
    private PageFooterBand PageFooter;
    private XRPanel xrPanel2;
    private XRLabel xrLabel3;
    private XRLabel xrLabel4;
    private XRLabel xrLabel2;
    private XRPictureBox xrPictureBox1;
    private XRLabel xrLabel1;
    private GroupHeaderBand GroupHeader4;
    private GroupHeaderBand GroupHeader2;
    private XRLabel xrLabel8;
    private XRLabel xrLabel9;
    private XRPanel xrPanel1;
    private XRPictureBox xrPictureBox2;
    private XRLabel xrLabel5;
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
    private XRTable xrTable11;
    private XRTableRow xrTableRow23;
    private XRTableCell xrTableCell51;
    private XRTableRow xrTableRow24;
    private XRTableCell xrTableCell53;
    private XRTableCell xrTableCell73;
    private XRTableCell xrTableCell54;
    private XRTableRow xrTableRow25;
    private XRTableCell xrTableCell55;
    private XRTableCell xrTableCell74;
    private XRTableCell xrTableCell56;
    private XRTableRow xrTableRow26;
    private XRTableCell xrTableCell57;
    private XRTableCell xrTableCell75;
    private XRTableCell xrTableCell58;
    private XRTableRow xrTableRow27;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell76;
    private XRTableCell xrTableCell60;
    private XRTableRow xrTableRow28;
    private XRTableCell xrTableCell61;
    private XRTableCell xrTableCell77;
    private XRTableCell xrTableCell62;
    private XRPageInfo xrPageInfo2;
    private XRPanel xrPanel3;
    private XRLabel xrLabel11;
    private XRLabel xrLabel12;
    private XRLabel xrLabel13;
    private XRLabel xrLabel14;
    private XRLabel xrLabel15;
    private XRPictureBox xrPictureBox3;
    private XRLabel xrLabel16;
    private XRPanel xrPanel4;
    private XRPictureBox xrPictureBox4;
    private XRLabel xrLabel18;
    private XRTable xrTable5;
    private XRTableRow xrTableRow22;
    private XRTableCell xrTableCell66;
    private XRTableCell xrTableCell69;
    private XRTableCell xrTableCell79;
    private XRTableRow xrTableRow29;
    private XRTableCell xrTableCell80;
    private XRTableCell xrTableCell83;
    private XRTableCell xrTableCell84;
    private XRTableRow xrTableRow30;
    private XRTableCell xrTableCell86;
    private XRTableCell xrTableCell87;
    private XRTableCell xrTableCell88;
    private XRTableRow xrTableRow31;
    private XRTableCell xrTableCell90;
    private XRTableCell xrTableCell91;
    private XRTableCell xrTableCell92;
    private XRTableRow xrTableRow32;
    private XRTableCell xrTableCell94;
    private XRTableCell xrTableCell95;
    private XRTableCell xrTableCell96;
    private XRTableRow xrTableRow33;
    private XRTableCell xrTableCell98;
    private XRTableCell xrTableCell99;
    private XRTableCell xrTableCell100;
    private XRTable xrTable8;
    private XRTableRow xrTableRow34;
    private XRTableCell xrTableCell106;
    private XRTableRow xrTableRow37;
    private XRTableCell xrTableCell108;
    private XRTableCell xrTableCell114;
    private XRTableCell xrTableCell115;
    private XRTableRow xrTableRow38;
    private XRTableCell xrTableCell116;
    private XRTableCell xrTableCell117;
    private XRTableCell xrTableCell125;
    private XRTableRow xrTableRow39;
    private XRTableCell xrTableCell126;
    private XRTableCell xrTableCell127;
    private XRTableCell xrTableCell128;
    private XRTableRow xrTableRow40;
    private XRTableCell xrTableCell129;
    private XRTableCell xrTableCell130;
    private XRTableCell xrTableCell131;
    private XRTableRow xrTableRow41;
    private XRTableCell xrTableCell132;
    private XRTableCell xrTableCell133;
    private XRTableCell xrTableCell134;
    private XRPageInfo xrPageInfo4;
    private XRLabel xrLabel21;
    private XRLabel xrLabel22;
    private XRLabel xrLabel24;
    private XRLabel xrLabel23;
    private XRPageInfo xrPageInfo3;
    private XRLabel xrLabel26;
    private XRLabel xrLabel25;
    private XRPageInfo xrPageInfo1;
    private XRLabel xrLabel30;
    private XRLabel xrLabel31;
    private XRLabel xrLabel32;
    private XRLabel xrLabel27;
    private XRLabel xrLabel28;
    private XRLabel xrLabel29;
    private XRLabel xrLabel36;
    private XRLabel xrLabel37;
    private XRLabel xrLabel38;
    private XRLabel xrLabel33;
    private XRLabel xrLabel34;
    private XRLabel xrLabel35;
    private XRTableRow xrTableRow61;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell70;
    private XRTableCell xrTableCell72;
    private XRTableRow xrTableRow62;
    private XRTableCell xrTableCell78;
    private XRTableCell xrTableCell189;
    private XRTableCell xrTableCell192;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell19;
    private XRTableCell xrTableCell20;
    private XRTableCell xrTableCell21;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell23;
    private XRTableCell xrTableCell24;
    private XRTableRow xrTableRow9;
    private XRTableCell xrTableCell25;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell27;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell30;
    private XRLabel xrLabel6;
    private XRLabel xrLabel7;
    private XRLabel xrLabel17;
    private XRLabel xrLabel10;
    private XRLabel xrLabel19;
    private XRLabel xrLabel20;
    private XRLabel xrLabel40;
    private XRLabel xrLabel39;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell39;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell41;
    private XRTableCell xrTableCell42;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell33;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell36;
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

    public EmployeeTerminationReport(string strConString)
	{
		InitializeComponent();
        objSysParam = new SystemParameter(strConString);
        //
        // TODO: Add constructor logic here
        //
    }
    public void setimage(string url)
    {
        xrPictureBox1.ImageUrl = url;
       xrPictureBox3.ImageUrl = url;
    }
    public void setempimage(string urlemp)
    {
        xrPictureBox2.ImageUrl = urlemp;
        xrPictureBox4.ImageUrl = urlemp;
    }


    public void setcompanyname(string companyname)
    {
        xrLabel1.Text = companyname;
        xrLabel16.Text = companyname;
     
    }
    public void setcompanyAddress(string Address)
    {
        xrLabel2.Text = Address;
        xrLabel15.Text = Address;
        
      
    }
    public void setcontact(string Contact)
    {
        xrLabel4.Text = Contact;
        xrLabel14.Text = Contact;
    }

    public void setwebsite(string web)
    {
        xrLabel8.Text = web;
        xrLabel12.Text = web;
    }
    public void setmailid(string mailid)
    {
         xrLabel9.Text = mailid;
         xrLabel11.Text = mailid;
        
    }
    
    public void setnetamount(string netamt)
    {
       
        //xrLabel10.Text = netamt;
      
    }
    public void Setheader(string CreatedBy,string EmployeeCode,string EmployeeName,string Designation,string Department,string Doj,string BankAc,string Attandance,string Attendence,string DayPresent,string WeekOff,string AbsentDays,string Holidays,string Leaves,string BasicSalary,string Attandancesalary,string OvertimeSalary,string Penalty,string WorkDays,string NormalOt,string WeekOffOt,string holidaysOt,string Late,string Early,string Partial,string Absent,string Total,string GrossPay,string PF,string Esic,string Netsalary,string HrSign,string EmployeeSign,string PayslipForthemonth,string EmployeeCopy,string Employercopy,string TotalDays)
    {
        xrLabel24.Text = CreatedBy;
        xrLabel26.Text = CreatedBy;
        xrTableCell1.Text = EmployeeCode;
        xrTableCell66.Text = EmployeeCode;
        xrTableCell80.Text=EmployeeName;
        xrTableCell4.Text = EmployeeName;
        xrTableCell86.Text=Designation;
        xrTableCell10.Text = Designation;
        xrTableCell90.Text = Department;
        xrTableCell7.Text = Department;
xrTableCell13.Text = Doj;
xrTableCell94.Text = Doj;
this.xrTableCell98.Text = BankAc;
this.xrTableCell16.Text = BankAc;
   this.xrTableCell106.Text = Attandance;
 this.xrTableCell51.Text = Attandance;
 this.xrTableCell53.Text = DayPresent;
 this.xrTableCell108.Text = DayPresent;
 this.xrTableCell116.Text = WeekOff;//
 
 this.xrTableCell55.Text = WeekOff;//
 
       
 this.xrTableCell57.Text = AbsentDays;
 this.xrTableCell126.Text = AbsentDays;
 this.xrTableCell129.Text = Holidays;
 
 xrTableCell59.Text = Holidays;
 
 this.xrTableCell132.Text = Leaves;
this.xrTableCell61.Text = Leaves;

   
 xrLabel5.Text = PayslipForthemonth;
 xrLabel18.Text = PayslipForthemonth;
 xrLabel21.Text = Employercopy;
         xrLabel22.Text = EmployeeCopy;
     
        xrLabel32.Text = Resources.Attendance.Brand;
        xrLabel35.Text = Resources.Attendance.Brand;
        xrLabel27.Text = Resources.Attendance.Location;
        xrLabel36.Text = Resources.Attendance.Location;

        xrTableCell51.Text = Resources.Attendance.Attendance_Detail;
        xrTableCell106.Text = Resources.Attendance.Attendance_Detail;
      
    }

    public void setBrandName(string BrandName)
    {
        xrLabel30.Text = BrandName;
        xrLabel33.Text = BrandName;

    }
    public void setLocationName(string LocationName)
    {
        xrLabel29.Text = LocationName;
        xrLabel38.Text = LocationName;

    }




    //public void setattendance(string dayspresent,string weekoffdays,string daysabsent, string holidays, string leavedays, string totaldays, string weekdayssal, string weekofsal, string holidayssal, string leavesal, string totalattendsal, string normalOTsal, string weekoffOTsal, string holidaysOTsal, string totalOTsal, string late, string early, string partial, string absentpean, string totalpenalty, string grossamt)
    //{

    //    lbldaypresent.Text = dayspresent;
    //    lblweekdays.Text = weekoffdays;
    //    xrLabel10.Text = daysabsent;
    //    xrLabel11.Text=holidays;
    //        xrLabel12.Text=leavedays;
    //        xrLabel13.Text=totaldays;
            
    //    xrLabel14.Text=weekdayssal;
    //        xrLabel15.Text=weekofsal;
    //        xrLabel16.Text=holidayssal;
    //        xrLabel17.Text=leavesal;
    //        xrLabel18.Text=totalattendsal;

    //        xrLabel19.Text=normalOTsal;
    //        xrLabel20.Text=weekoffOTsal;
    //        xrLabel21.Text=holidaysOTsal;
    //        xrLabel22.Text=totalOTsal;

    //        xrLabel23.Text=late;
    //        xrLabel24.Text=early;
    //        xrLabel25.Text=partial;
    //        xrLabel26.Text=absentpean;
    //        xrLabel27.Text= totalpenalty;

    //        xrLabel28.Text = grossamt;



    
    //}
  

    
   
   
	
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
        string resourceFileName = "EmployeeTerminationReport.resx";
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
        this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox3 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
        this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
        this.employeePaySlipDataSet1 = new EmployeePaySlipDataSet();
        this.employeeterminationDataSet1 = new EmployeeTerminationDataset();
        this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
        this.xrPageInfo4 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.xrPageInfo2 = new DevExpress.XtraReports.UI.XRPageInfo();
        this.GroupHeader4 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrPictureBox4 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow29 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow30 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow31 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow32 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell94 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell96 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow33 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell98 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell99 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell100 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
        this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
        this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
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
        this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
        this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
        this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
        this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow34 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell106 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow37 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell108 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell114 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell115 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow38 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell116 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell117 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell125 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow39 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell126 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell127 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell128 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow40 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell129 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell130 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell131 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow41 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell132 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell133 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell134 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTable11 = new DevExpress.XtraReports.UI.XRTable();
        this.xrTableRow23 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow24 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell54 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow25 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow26 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow27 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow28 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell62 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow61 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow62 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell189 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell192 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
        this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
        this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
        ((System.ComponentModel.ISupportInitialize)(this.employeePaySlipDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.employeeterminationDataSet1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.Detail.Expanded = false;
        this.Detail.HeightF = 0F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.StylePriority.UseBorders = false;
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // TopMargin
        // 
        this.TopMargin.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel26,
            this.xrLabel25,
            this.xrPageInfo1,
            this.xrLabel24,
            this.xrLabel23,
            this.xrPageInfo3,
            this.xrPanel3,
            this.xrPanel2});
        this.TopMargin.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.TopMargin.HeightF = 113F;
        this.TopMargin.Name = "TopMargin";
        this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.TopMargin.StylePriority.UseBorders = false;
        this.TopMargin.StylePriority.UseFont = false;
        this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel26
        // 
        this.xrLabel26.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel26.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(595.1605F, 0F);
        this.xrLabel26.Name = "xrLabel26";
        this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel26.SizeF = new System.Drawing.SizeF(91.95319F, 15.70834F);
        this.xrLabel26.StylePriority.UseBorders = false;
        this.xrLabel26.StylePriority.UseFont = false;
        this.xrLabel26.Text = "Created By:";
        // 
        // xrLabel25
        // 
        this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel25.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(687.1137F, 0F);
        this.xrLabel25.Name = "xrLabel25";
        this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel25.SizeF = new System.Drawing.SizeF(186.4217F, 15.70834F);
        this.xrLabel25.StylePriority.UseBorders = false;
        this.xrLabel25.StylePriority.UseFont = false;
        this.xrLabel25.StylePriority.UseTextAlignment = false;
        this.xrLabel25.Text = "xrLabel10";
        this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPageInfo1
        // 
        this.xrPageInfo1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo1.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
        this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(896.2433F, 0F);
        this.xrPageInfo1.Name = "xrPageInfo1";
        this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo1.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo1.SizeF = new System.Drawing.SizeF(242.7571F, 15.70834F);
        this.xrPageInfo1.StylePriority.UseBorders = false;
        this.xrPageInfo1.StylePriority.UseTextAlignment = false;
        this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrLabel24
        // 
        this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel24.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(0.7791519F, 0F);
        this.xrLabel24.Name = "xrLabel24";
        this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel24.SizeF = new System.Drawing.SizeF(91.95319F, 15.70834F);
        this.xrLabel24.StylePriority.UseBorders = false;
        this.xrLabel24.StylePriority.UseFont = false;
        this.xrLabel24.Text = "Created By:";
        // 
        // xrLabel23
        // 
        this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel23.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(92.73235F, 0F);
        this.xrLabel23.Name = "xrLabel23";
        this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel23.SizeF = new System.Drawing.SizeF(186.4217F, 15.70834F);
        this.xrLabel23.StylePriority.UseBorders = false;
        this.xrLabel23.StylePriority.UseFont = false;
        this.xrLabel23.StylePriority.UseTextAlignment = false;
        this.xrLabel23.Text = "xrLabel10";
        this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPageInfo3
        // 
        this.xrPageInfo3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo3.Format = "{0:dddd, MMMM dd, yyyy h:mm tt}";
        this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(302.353F, 0F);
        this.xrPageInfo3.Name = "xrPageInfo3";
        this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo3.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.xrPageInfo3.SizeF = new System.Drawing.SizeF(242.7568F, 15.70834F);
        this.xrPageInfo3.StylePriority.UseBorders = false;
        this.xrPageInfo3.StylePriority.UseTextAlignment = false;
        this.xrPageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrPanel3
        // 
        this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel36,
            this.xrLabel37,
            this.xrLabel38,
            this.xrLabel33,
            this.xrLabel34,
            this.xrLabel35,
            this.xrLabel22,
            this.xrLabel11,
            this.xrLabel12,
            this.xrLabel13,
            this.xrLabel14,
            this.xrLabel15,
            this.xrPictureBox3,
            this.xrLabel16});
        this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(594.6693F, 15.70833F);
        this.xrPanel3.Name = "xrPanel3";
        this.xrPanel3.SizeF = new System.Drawing.SizeF(544.3307F, 97.12501F);
        // 
        // xrLabel36
        // 
        this.xrLabel36.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel36.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel36.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(221.9465F, 64.41668F);
        this.xrLabel36.Name = "xrLabel36";
        this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel36.SizeF = new System.Drawing.SizeF(62.5F, 16.75F);
        this.xrLabel36.StylePriority.UseBorderColor = false;
        this.xrLabel36.StylePriority.UseBorders = false;
        this.xrLabel36.StylePriority.UseFont = false;
        this.xrLabel36.Text = "Location";
        // 
        // xrLabel37
        // 
        this.xrLabel37.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel37.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel37.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(284.4465F, 64.41668F);
        this.xrLabel37.Name = "xrLabel37";
        this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel37.SizeF = new System.Drawing.SizeF(11.83331F, 16.75F);
        this.xrLabel37.StylePriority.UseBorderColor = false;
        this.xrLabel37.StylePriority.UseBorders = false;
        this.xrLabel37.StylePriority.UseFont = false;
        this.xrLabel37.Text = ":";
        // 
        // xrLabel38
        // 
        this.xrLabel38.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel38.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel38.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(296.9465F, 64.41668F);
        this.xrLabel38.Name = "xrLabel38";
        this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel38.SizeF = new System.Drawing.SizeF(246F, 16.75F);
        this.xrLabel38.StylePriority.UseBorderColor = false;
        this.xrLabel38.StylePriority.UseBorders = false;
        this.xrLabel38.StylePriority.UseFont = false;
        this.xrLabel38.Text = "xrLabel10";
        // 
        // xrLabel33
        // 
        this.xrLabel33.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel33.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel33.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(96.94647F, 64.41668F);
        this.xrLabel33.Name = "xrLabel33";
        this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel33.SizeF = new System.Drawing.SizeF(125F, 16.75F);
        this.xrLabel33.StylePriority.UseBorderColor = false;
        this.xrLabel33.StylePriority.UseBorders = false;
        this.xrLabel33.StylePriority.UseFont = false;
        this.xrLabel33.Text = "xrLabel10";
        // 
        // xrLabel34
        // 
        this.xrLabel34.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel34.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel34.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(84.44647F, 64.41668F);
        this.xrLabel34.Name = "xrLabel34";
        this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel34.SizeF = new System.Drawing.SizeF(12.5F, 16.75F);
        this.xrLabel34.StylePriority.UseBorderColor = false;
        this.xrLabel34.StylePriority.UseBorders = false;
        this.xrLabel34.StylePriority.UseFont = false;
        this.xrLabel34.Text = ":";
        // 
        // xrLabel35
        // 
        this.xrLabel35.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel35.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel35.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(1.535278F, 64.41671F);
        this.xrLabel35.Name = "xrLabel35";
        this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel35.SizeF = new System.Drawing.SizeF(82.2113F, 16.74997F);
        this.xrLabel35.StylePriority.UseBorderColor = false;
        this.xrLabel35.StylePriority.UseBorders = false;
        this.xrLabel35.StylePriority.UseFont = false;
        this.xrLabel35.Text = "Brand";
        // 
        // xrLabel22
        // 
        this.xrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel22.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(422.1744F, 45.83335F);
        this.xrLabel22.Name = "xrLabel22";
        this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel22.SizeF = new System.Drawing.SizeF(118.6933F, 14.58332F);
        this.xrLabel22.StylePriority.UseBorders = false;
        this.xrLabel22.StylePriority.UseFont = false;
        this.xrLabel22.StylePriority.UseTextAlignment = false;
        this.xrLabel22.Text = "Employee Copy";
        this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrLabel11
        // 
        this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel11.Font = new System.Drawing.Font("Times New Roman", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(3.535217F, 52.37505F);
        this.xrLabel11.Name = "xrLabel11";
        this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel11.SizeF = new System.Drawing.SizeF(177.9321F, 10.49997F);
        this.xrLabel11.StylePriority.UseBorders = false;
        this.xrLabel11.StylePriority.UseFont = false;
        this.xrLabel11.Text = "xrLabel9";
        // 
        // xrLabel12
        // 
        this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel12.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(3.535217F, 41.83339F);
        this.xrLabel12.Name = "xrLabel12";
        this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel12.SizeF = new System.Drawing.SizeF(177.932F, 10.49997F);
        this.xrLabel12.StylePriority.UseBorders = false;
        this.xrLabel12.StylePriority.UseFont = false;
        this.xrLabel12.Text = "xrLabel8";
        // 
        // xrLabel13
        // 
        this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(3.535217F, 27.16676F);
        this.xrLabel13.Name = "xrLabel13";
        this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel13.SizeF = new System.Drawing.SizeF(56.67975F, 14.66665F);
        this.xrLabel13.StylePriority.UseBorders = false;
        this.xrLabel13.StylePriority.UseFont = false;
        this.xrLabel13.StylePriority.UseTextAlignment = false;
        this.xrLabel13.Text = "Phone No :";
        this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel14
        // 
        this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel14.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(60.2149F, 27.16675F);
        this.xrLabel14.Name = "xrLabel14";
        this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel14.SizeF = new System.Drawing.SizeF(121.2523F, 14.66667F);
        this.xrLabel14.StylePriority.UseBorders = false;
        this.xrLabel14.StylePriority.UseFont = false;
        this.xrLabel14.Text = "xrLabel4";
        // 
        // xrLabel15
        // 
        this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel15.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(3.535278F, 16.75006F);
        this.xrLabel15.Name = "xrLabel15";
        this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel15.SizeF = new System.Drawing.SizeF(177.932F, 10.41668F);
        this.xrLabel15.StylePriority.UseBorders = false;
        this.xrLabel15.StylePriority.UseFont = false;
        this.xrLabel15.StylePriority.UseTextAlignment = false;
        this.xrLabel15.Text = "xrLabel2";
        this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPictureBox3
        // 
        this.xrPictureBox3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(471.4138F, 6.00001F);
        this.xrPictureBox3.Name = "xrPictureBox3";
        this.xrPictureBox3.SizeF = new System.Drawing.SizeF(69.45386F, 35.83337F);
        this.xrPictureBox3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox3.StylePriority.UseBorders = false;
        // 
        // xrLabel16
        // 
        this.xrLabel16.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel16.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(3.535278F, 2.916797F);
        this.xrLabel16.Name = "xrLabel16";
        this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel16.SizeF = new System.Drawing.SizeF(370.6541F, 12.83328F);
        this.xrLabel16.StylePriority.UseBorders = false;
        this.xrLabel16.StylePriority.UseFont = false;
        this.xrLabel16.StylePriority.UseTextAlignment = false;
        this.xrLabel16.Text = "xrLabel1";
        this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPanel2
        // 
        this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel30,
            this.xrLabel31,
            this.xrLabel32,
            this.xrLabel27,
            this.xrLabel28,
            this.xrLabel29,
            this.xrLabel21,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel3,
            this.xrLabel4,
            this.xrLabel2,
            this.xrPictureBox1,
            this.xrLabel1});
        this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0.7790725F, 15.70833F);
        this.xrPanel2.Name = "xrPanel2";
        this.xrPanel2.SizeF = new System.Drawing.SizeF(544.3305F, 97.12501F);
        // 
        // xrLabel30
        // 
        this.xrLabel30.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel30.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel30.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(96.94613F, 64.41667F);
        this.xrLabel30.Name = "xrLabel30";
        this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel30.SizeF = new System.Drawing.SizeF(125F, 16.75F);
        this.xrLabel30.StylePriority.UseBorderColor = false;
        this.xrLabel30.StylePriority.UseBorders = false;
        this.xrLabel30.StylePriority.UseFont = false;
        this.xrLabel30.Text = "xrLabel10";
        // 
        // xrLabel31
        // 
        this.xrLabel31.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel31.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel31.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(83.74657F, 64.41669F);
        this.xrLabel31.Name = "xrLabel31";
        this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel31.SizeF = new System.Drawing.SizeF(13.19955F, 16.75F);
        this.xrLabel31.StylePriority.UseBorderColor = false;
        this.xrLabel31.StylePriority.UseBorders = false;
        this.xrLabel31.StylePriority.UseFont = false;
        this.xrLabel31.Text = ":";
        // 
        // xrLabel32
        // 
        this.xrLabel32.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel32.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel32.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(2.176814F, 64.41669F);
        this.xrLabel32.Name = "xrLabel32";
        this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel32.SizeF = new System.Drawing.SizeF(81.56977F, 16.75F);
        this.xrLabel32.StylePriority.UseBorderColor = false;
        this.xrLabel32.StylePriority.UseBorders = false;
        this.xrLabel32.StylePriority.UseFont = false;
        this.xrLabel32.Text = "Brand";
        // 
        // xrLabel27
        // 
        this.xrLabel27.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel27.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(221.9461F, 64.41667F);
        this.xrLabel27.Name = "xrLabel27";
        this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel27.SizeF = new System.Drawing.SizeF(62.50002F, 16.75F);
        this.xrLabel27.StylePriority.UseBorderColor = false;
        this.xrLabel27.StylePriority.UseBorders = false;
        this.xrLabel27.StylePriority.UseFont = false;
        this.xrLabel27.Text = "Location";
        // 
        // xrLabel28
        // 
        this.xrLabel28.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel28.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel28.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(284.4462F, 64.41669F);
        this.xrLabel28.Name = "xrLabel28";
        this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel28.SizeF = new System.Drawing.SizeF(12.49997F, 16.75F);
        this.xrLabel28.StylePriority.UseBorderColor = false;
        this.xrLabel28.StylePriority.UseBorders = false;
        this.xrLabel28.StylePriority.UseFont = false;
        this.xrLabel28.Text = ":";
        // 
        // xrLabel29
        // 
        this.xrLabel29.BorderColor = System.Drawing.Color.WhiteSmoke;
        this.xrLabel29.Borders = DevExpress.XtraPrinting.BorderSide.Top;
        this.xrLabel29.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(296.9462F, 64.41665F);
        this.xrLabel29.Name = "xrLabel29";
        this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel29.SizeF = new System.Drawing.SizeF(246F, 16.75F);
        this.xrLabel29.StylePriority.UseBorderColor = false;
        this.xrLabel29.StylePriority.UseBorders = false;
        this.xrLabel29.StylePriority.UseFont = false;
        this.xrLabel29.Text = "xrLabel10";
        // 
        // xrLabel21
        // 
        this.xrLabel21.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel21.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(411.3153F, 45.83335F);
        this.xrLabel21.Name = "xrLabel21";
        this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel21.SizeF = new System.Drawing.SizeF(128.0151F, 14.58332F);
        this.xrLabel21.StylePriority.UseBorders = false;
        this.xrLabel21.StylePriority.UseFont = false;
        this.xrLabel21.StylePriority.UseTextAlignment = false;
        this.xrLabel21.Text = "Employer Copy";
        this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrLabel9
        // 
        this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel9.Font = new System.Drawing.Font("Times New Roman", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(2.17678F, 52.37505F);
        this.xrLabel9.Name = "xrLabel9";
        this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel9.SizeF = new System.Drawing.SizeF(178.9763F, 10.49997F);
        this.xrLabel9.StylePriority.UseBorders = false;
        this.xrLabel9.StylePriority.UseFont = false;
        this.xrLabel9.Text = "xrLabel9";
        // 
        // xrLabel8
        // 
        this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(2.176812F, 41.83338F);
        this.xrLabel8.Name = "xrLabel8";
        this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel8.SizeF = new System.Drawing.SizeF(172.3732F, 10.49998F);
        this.xrLabel8.StylePriority.UseBorders = false;
        this.xrLabel8.StylePriority.UseFont = false;
        this.xrLabel8.Text = "xrLabel8";
        // 
        // xrLabel3
        // 
        this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(2.176811F, 27.16677F);
        this.xrLabel3.Name = "xrLabel3";
        this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel3.SizeF = new System.Drawing.SizeF(57.72382F, 14.66664F);
        this.xrLabel3.StylePriority.UseBorders = false;
        this.xrLabel3.StylePriority.UseFont = false;
        this.xrLabel3.StylePriority.UseTextAlignment = false;
        this.xrLabel3.Text = "Phone No :";
        this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrLabel4
        // 
        this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(59.90063F, 27.16675F);
        this.xrLabel4.Name = "xrLabel4";
        this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel4.SizeF = new System.Drawing.SizeF(114.6494F, 14.66664F);
        this.xrLabel4.StylePriority.UseBorders = false;
        this.xrLabel4.StylePriority.UseFont = false;
        this.xrLabel4.Text = "xrLabel4";
        // 
        // xrLabel2
        // 
        this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(2.176809F, 16.75006F);
        this.xrLabel2.Name = "xrLabel2";
        this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel2.SizeF = new System.Drawing.SizeF(311.7722F, 10.41668F);
        this.xrLabel2.StylePriority.UseBorders = false;
        this.xrLabel2.StylePriority.UseFont = false;
        this.xrLabel2.StylePriority.UseTextAlignment = false;
        this.xrLabel2.Text = "xrLabel2";
        this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // xrPictureBox1
        // 
        this.xrPictureBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(471.4137F, 6.000015F);
        this.xrPictureBox1.Name = "xrPictureBox1";
        this.xrPictureBox1.SizeF = new System.Drawing.SizeF(67.91672F, 35.33335F);
        this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox1.StylePriority.UseBorders = false;
        // 
        // xrLabel1
        // 
        this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(2.176817F, 2.916797F);
        this.xrLabel1.Name = "xrLabel1";
        this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel1.SizeF = new System.Drawing.SizeF(328.456F, 12.83328F);
        this.xrLabel1.StylePriority.UseBorders = false;
        this.xrLabel1.StylePriority.UseFont = false;
        this.xrLabel1.StylePriority.UseTextAlignment = false;
        this.xrLabel1.Text = "xrLabel1";
        this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // BottomMargin
        // 
        this.BottomMargin.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.BottomMargin.HeightF = 40.16673F;
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
        // employeeterminationDataSet1
        // 
        this.employeeterminationDataSet1.DataSetName = "EmployeeTerminationDataset";
        this.employeeterminationDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        // 
        // PageFooter
        // 
        this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo4,
            this.xrPageInfo2});
        this.PageFooter.HeightF = 23.33323F;
        this.PageFooter.Name = "PageFooter";
        // 
        // xrPageInfo4
        // 
        this.xrPageInfo4.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo4.Format = "Page{0}";
        this.xrPageInfo4.LocationFloat = new DevExpress.Utils.PointFloat(594.6693F, 0F);
        this.xrPageInfo4.Name = "xrPageInfo4";
        this.xrPageInfo4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo4.SizeF = new System.Drawing.SizeF(540.8677F, 22.99995F);
        this.xrPageInfo4.StylePriority.UseBorders = false;
        this.xrPageInfo4.StylePriority.UseTextAlignment = false;
        this.xrPageInfo4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // xrPageInfo2
        // 
        this.xrPageInfo2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPageInfo2.Format = "Page{0}";
        this.xrPageInfo2.LocationFloat = new DevExpress.Utils.PointFloat(0.7791519F, 0F);
        this.xrPageInfo2.Name = "xrPageInfo2";
        this.xrPageInfo2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrPageInfo2.SizeF = new System.Drawing.SizeF(544.3304F, 22.99995F);
        this.xrPageInfo2.StylePriority.UseBorders = false;
        this.xrPageInfo2.StylePriority.UseTextAlignment = false;
        this.xrPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // GroupHeader4
        // 
        this.GroupHeader4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel4,
            this.xrPanel1});
        this.GroupHeader4.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("EmpCode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader4.HeightF = 171.5695F;
        this.GroupHeader4.Level = 1;
        this.GroupHeader4.Name = "GroupHeader4";
        // 
        // xrPanel4
        // 
        this.xrPanel4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox4,
            this.xrLabel18,
            this.xrTable5});
        this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(594.6693F, 0F);
        this.xrPanel4.Name = "xrPanel4";
        this.xrPanel4.SizeF = new System.Drawing.SizeF(544.3307F, 171.5695F);
        this.xrPanel4.StylePriority.UseBorders = false;
        // 
        // xrPictureBox4
        // 
        this.xrPictureBox4.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox4.LocationFloat = new DevExpress.Utils.PointFloat(453.1237F, 25.1667F);
        this.xrPictureBox4.Name = "xrPictureBox4";
        this.xrPictureBox4.SizeF = new System.Drawing.SizeF(75.7F, 70.83F);
        this.xrPictureBox4.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox4.StylePriority.UseBorders = false;
        this.xrPictureBox4.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPictureBox4_BeforePrint);
        // 
        // xrLabel18
        // 
        this.xrLabel18.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel18.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(93.88422F, 6.569486F);
        this.xrLabel18.Name = "xrLabel18";
        this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel18.SizeF = new System.Drawing.SizeF(323.2393F, 12.58F);
        this.xrLabel18.StylePriority.UseBorders = false;
        this.xrLabel18.StylePriority.UseFont = false;
        this.xrLabel18.StylePriority.UseTextAlignment = false;
        this.xrLabel18.Text = "TERMINATION REPORT";
        this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrTable5
        // 
        this.xrTable5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(10F, 25.1667F);
        this.xrTable5.Name = "xrTable5";
        this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow22,
            this.xrTableRow29,
            this.xrTableRow30,
            this.xrTableRow31,
            this.xrTableRow32,
            this.xrTableRow33,
            this.xrTableRow10});
        this.xrTable5.SizeF = new System.Drawing.SizeF(364.189F, 134.9445F);
        this.xrTable5.StylePriority.UseBorders = false;
        // 
        // xrTableRow22
        // 
        this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell66,
            this.xrTableCell69,
            this.xrTableCell79});
        this.xrTableRow22.Name = "xrTableRow22";
        this.xrTableRow22.Weight = 1D;
        // 
        // xrTableCell66
        // 
        this.xrTableCell66.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell66.Name = "xrTableCell66";
        this.xrTableCell66.StylePriority.UseFont = false;
        this.xrTableCell66.Text = "EMPLOYEE CODE";
        this.xrTableCell66.Weight = 0.25680544945779782D;
        // 
        // xrTableCell69
        // 
        this.xrTableCell69.Name = "xrTableCell69";
        this.xrTableCell69.Text = ":";
        this.xrTableCell69.Weight = 0.026470594643385491D;
        // 
        // xrTableCell79
        // 
        this.xrTableCell79.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.EmpCode")});
        this.xrTableCell79.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell79.Name = "xrTableCell79";
        this.xrTableCell79.StylePriority.UseFont = false;
        this.xrTableCell79.Weight = 0.24427512303055635D;
        // 
        // xrTableRow29
        // 
        this.xrTableRow29.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell80,
            this.xrTableCell83,
            this.xrTableCell84});
        this.xrTableRow29.Name = "xrTableRow29";
        this.xrTableRow29.Weight = 1D;
        // 
        // xrTableCell80
        // 
        this.xrTableCell80.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell80.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell80.Name = "xrTableCell80";
        this.xrTableCell80.StylePriority.UseBorders = false;
        this.xrTableCell80.StylePriority.UseFont = false;
        this.xrTableCell80.Text = "EMPLOYEE NAME";
        this.xrTableCell80.Weight = 0.2568054280944474D;
        // 
        // xrTableCell83
        // 
        this.xrTableCell83.Name = "xrTableCell83";
        this.xrTableCell83.Text = ":";
        this.xrTableCell83.Weight = 0.026470597701588664D;
        // 
        // xrTableCell84
        // 
        this.xrTableCell84.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.EmpName")});
        this.xrTableCell84.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell84.Name = "xrTableCell84";
        this.xrTableCell84.StylePriority.UseFont = false;
        this.xrTableCell84.Weight = 0.24427514133570366D;
        // 
        // xrTableRow30
        // 
        this.xrTableRow30.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell86,
            this.xrTableCell87,
            this.xrTableCell88});
        this.xrTableRow30.Name = "xrTableRow30";
        this.xrTableRow30.Weight = 1D;
        // 
        // xrTableCell86
        // 
        this.xrTableCell86.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell86.Name = "xrTableCell86";
        this.xrTableCell86.StylePriority.UseFont = false;
        this.xrTableCell86.Text = "DESIGNATION";
        this.xrTableCell86.Weight = 0.25680544743486455D;
        // 
        // xrTableCell87
        // 
        this.xrTableCell87.Name = "xrTableCell87";
        this.xrTableCell87.Text = ":";
        this.xrTableCell87.Weight = 0.026470578361171504D;
        // 
        // xrTableCell88
        // 
        this.xrTableCell88.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.DesignationName")});
        this.xrTableCell88.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell88.Name = "xrTableCell88";
        this.xrTableCell88.StylePriority.UseFont = false;
        this.xrTableCell88.Weight = 0.24427514133570366D;
        // 
        // xrTableRow31
        // 
        this.xrTableRow31.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell90,
            this.xrTableCell91,
            this.xrTableCell92});
        this.xrTableRow31.Name = "xrTableRow31";
        this.xrTableRow31.Weight = 1D;
        // 
        // xrTableCell90
        // 
        this.xrTableCell90.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell90.Name = "xrTableCell90";
        this.xrTableCell90.StylePriority.UseFont = false;
        this.xrTableCell90.Text = "DEPARTMENT";
        this.xrTableCell90.Weight = 0.25680544743486455D;
        // 
        // xrTableCell91
        // 
        this.xrTableCell91.Name = "xrTableCell91";
        this.xrTableCell91.Text = ":";
        this.xrTableCell91.Weight = 0.026470578361171504D;
        // 
        // xrTableCell92
        // 
        this.xrTableCell92.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.DepartmentName")});
        this.xrTableCell92.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell92.Name = "xrTableCell92";
        this.xrTableCell92.StylePriority.UseFont = false;
        this.xrTableCell92.Weight = 0.24427514133570366D;
        // 
        // xrTableRow32
        // 
        this.xrTableRow32.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell94,
            this.xrTableCell95,
            this.xrTableCell96});
        this.xrTableRow32.Name = "xrTableRow32";
        this.xrTableRow32.Weight = 1D;
        // 
        // xrTableCell94
        // 
        this.xrTableCell94.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell94.Name = "xrTableCell94";
        this.xrTableCell94.StylePriority.UseFont = false;
        this.xrTableCell94.Text = "DOJ";
        this.xrTableCell94.Weight = 0.25680544743486455D;
        // 
        // xrTableCell95
        // 
        this.xrTableCell95.Name = "xrTableCell95";
        this.xrTableCell95.Text = ":";
        this.xrTableCell95.Weight = 0.026470578361171504D;
        // 
        // xrTableCell96
        // 
        this.xrTableCell96.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Date_Of_Joining")});
        this.xrTableCell96.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell96.Name = "xrTableCell96";
        this.xrTableCell96.StylePriority.UseFont = false;
        this.xrTableCell96.Weight = 0.24427514133570366D;
        this.xrTableCell96.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell96_BeforePrint);
        // 
        // xrTableRow33
        // 
        this.xrTableRow33.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell98,
            this.xrTableCell99,
            this.xrTableCell100});
        this.xrTableRow33.Name = "xrTableRow33";
        this.xrTableRow33.Weight = 1D;
        // 
        // xrTableCell98
        // 
        this.xrTableCell98.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell98.Name = "xrTableCell98";
        this.xrTableCell98.StylePriority.UseFont = false;
        this.xrTableCell98.Text = "TERMINATION DATE";
        this.xrTableCell98.Weight = 0.25680544743486455D;
        // 
        // xrTableCell99
        // 
        this.xrTableCell99.Name = "xrTableCell99";
        this.xrTableCell99.Text = ":";
        this.xrTableCell99.Weight = 0.026470578361171504D;
        // 
        // xrTableCell100
        // 
        this.xrTableCell100.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Termination_Date")});
        this.xrTableCell100.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell100.Name = "xrTableCell100";
        this.xrTableCell100.StylePriority.UseFont = false;
        this.xrTableCell100.Weight = 0.24427514133570366D;
        this.xrTableCell100.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell100_BeforePrint);
        // 
        // xrTableRow10
        // 
        this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell28,
            this.xrTableCell29,
            this.xrTableCell30});
        this.xrTableRow10.Name = "xrTableRow10";
        this.xrTableRow10.Weight = 1D;
        // 
        // xrTableCell28
        // 
        this.xrTableCell28.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell28.Name = "xrTableCell28";
        this.xrTableCell28.StylePriority.UseFont = false;
        this.xrTableCell28.Text = "TERMINATION TYPE";
        this.xrTableCell28.Weight = 0.25680544743486455D;
        // 
        // xrTableCell29
        // 
        this.xrTableCell29.Name = "xrTableCell29";
        this.xrTableCell29.Text = ":";
        this.xrTableCell29.Weight = 0.026470578361171504D;
        // 
        // xrTableCell30
        // 
        this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.TerminationType")});
        this.xrTableCell30.Name = "xrTableCell30";
        this.xrTableCell30.Text = "xrTableCell30";
        this.xrTableCell30.Weight = 0.24427514133570366D;
        // 
        // xrPanel1
        // 
        this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2,
            this.xrLabel5,
            this.xrTable1});
        this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0.7791519F, 0F);
        this.xrPanel1.Name = "xrPanel1";
        this.xrPanel1.SizeF = new System.Drawing.SizeF(544.3306F, 171.5695F);
        this.xrPanel1.StylePriority.UseBorders = false;
        // 
        // xrPictureBox2
        // 
        this.xrPictureBox2.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(445.6902F, 25.1667F);
        this.xrPictureBox2.Name = "xrPictureBox2";
        this.xrPictureBox2.SizeF = new System.Drawing.SizeF(75.70123F, 70.83332F);
        this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
        this.xrPictureBox2.StylePriority.UseBorders = false;
        this.xrPictureBox2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrPictureBox2_BeforePrint);
        // 
        // xrLabel5
        // 
        this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
        this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(91.95322F, 6.569486F);
        this.xrLabel5.Name = "xrLabel5";
        this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel5.SizeF = new System.Drawing.SizeF(306.4203F, 12.58F);
        this.xrLabel5.StylePriority.UseBorders = false;
        this.xrLabel5.StylePriority.UseFont = false;
        this.xrLabel5.StylePriority.UseTextAlignment = false;
        this.xrLabel5.Text = "TERMINATION REPORT";
        this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrTable1
        // 
        this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 25.1667F);
        this.xrTable1.Name = "xrTable1";
        this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2,
            this.xrTableRow4,
            this.xrTableRow3,
            this.xrTableRow5,
            this.xrTableRow6,
            this.xrTableRow9});
        this.xrTable1.SizeF = new System.Drawing.SizeF(364.189F, 134.9445F);
        this.xrTable1.StylePriority.UseBorders = false;
        this.xrTable1.StylePriority.UseFont = false;
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
        this.xrTableCell1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell1.Name = "xrTableCell1";
        this.xrTableCell1.StylePriority.UseFont = false;
        this.xrTableCell1.Text = "EMPLOYEE CODE";
        this.xrTableCell1.Weight = 0.25680544945779782D;
        // 
        // xrTableCell2
        // 
        this.xrTableCell2.Name = "xrTableCell2";
        this.xrTableCell2.Text = ":";
        this.xrTableCell2.Weight = 0.026470594643385491D;
        // 
        // xrTableCell3
        // 
        this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.EmpCode")});
        this.xrTableCell3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell3.Name = "xrTableCell3";
        this.xrTableCell3.StylePriority.UseFont = false;
        this.xrTableCell3.Weight = 0.24427512303055635D;
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
        this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell4.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell4.Name = "xrTableCell4";
        this.xrTableCell4.StylePriority.UseBorders = false;
        this.xrTableCell4.StylePriority.UseFont = false;
        this.xrTableCell4.Text = "EMPLOYEE NAME";
        this.xrTableCell4.Weight = 0.2568054280944474D;
        // 
        // xrTableCell5
        // 
        this.xrTableCell5.Name = "xrTableCell5";
        this.xrTableCell5.Text = ":";
        this.xrTableCell5.Weight = 0.026470597701588664D;
        // 
        // xrTableCell6
        // 
        this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.EmpName")});
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
        this.xrTableCell10.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell10.Name = "xrTableCell10";
        this.xrTableCell10.StylePriority.UseFont = false;
        this.xrTableCell10.Text = "DESIGNATION";
        this.xrTableCell10.Weight = 0.25680544743486455D;
        // 
        // xrTableCell11
        // 
        this.xrTableCell11.Name = "xrTableCell11";
        this.xrTableCell11.Text = ":";
        this.xrTableCell11.Weight = 0.026470578361171504D;
        // 
        // xrTableCell9
        // 
        this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.DesignationName")});
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
        this.xrTableCell7.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell7.Name = "xrTableCell7";
        this.xrTableCell7.StylePriority.UseFont = false;
        this.xrTableCell7.Text = "DEPARTMENT";
        this.xrTableCell7.Weight = 0.25680544743486455D;
        // 
        // xrTableCell8
        // 
        this.xrTableCell8.Name = "xrTableCell8";
        this.xrTableCell8.Text = ":";
        this.xrTableCell8.Weight = 0.026470578361171504D;
        // 
        // xrTableCell12
        // 
        this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.DepartmentName")});
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
        this.xrTableCell13.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell13.Name = "xrTableCell13";
        this.xrTableCell13.StylePriority.UseFont = false;
        this.xrTableCell13.Text = "DOJ";
        this.xrTableCell13.Weight = 0.25680544743486455D;
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
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Date_Of_Joining")});
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
        this.xrTableCell16.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell16.Name = "xrTableCell16";
        this.xrTableCell16.StylePriority.UseFont = false;
        this.xrTableCell16.Text = "TERMINATION DATE";
        this.xrTableCell16.Weight = 0.25680544743486455D;
        // 
        // xrTableCell17
        // 
        this.xrTableCell17.Name = "xrTableCell17";
        this.xrTableCell17.Text = ":";
        this.xrTableCell17.Weight = 0.026470578361171504D;
        // 
        // xrTableCell18
        // 
        this.xrTableCell18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Termination_Date")});
        this.xrTableCell18.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell18.Name = "xrTableCell18";
        this.xrTableCell18.StylePriority.UseFont = false;
        this.xrTableCell18.Weight = 0.24427514133570366D;
        this.xrTableCell18.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell18_BeforePrint);
        // 
        // xrTableRow9
        // 
        this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27});
        this.xrTableRow9.Name = "xrTableRow9";
        this.xrTableRow9.Weight = 1D;
        // 
        // xrTableCell25
        // 
        this.xrTableCell25.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrTableCell25.Name = "xrTableCell25";
        this.xrTableCell25.StylePriority.UseFont = false;
        this.xrTableCell25.Text = "TERMINATION TYPE";
        this.xrTableCell25.Weight = 0.25680544743486455D;
        // 
        // xrTableCell26
        // 
        this.xrTableCell26.Name = "xrTableCell26";
        this.xrTableCell26.Text = ":";
        this.xrTableCell26.Weight = 0.026470578361171504D;
        // 
        // xrTableCell27
        // 
        this.xrTableCell27.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.TerminationType")});
        this.xrTableCell27.Name = "xrTableCell27";
        this.xrTableCell27.Text = "xrTableCell27";
        this.xrTableCell27.Weight = 0.24427514133570366D;
        // 
        // GroupHeader2
        // 
        this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel40,
            this.xrLabel39,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLabel17,
            this.xrLabel10,
            this.xrLabel7,
            this.xrLabel6,
            this.xrTable8,
            this.xrTable11});
        this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("EmpCode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
        this.GroupHeader2.HeightF = 479.1864F;
        this.GroupHeader2.Name = "GroupHeader2";
        // 
        // xrLabel40
        // 
        this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(879.1158F, 404.7937F);
        this.xrLabel40.Name = "xrLabel40";
        this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 50, 0, 100F);
        this.xrLabel40.SizeF = new System.Drawing.SizeF(260.8841F, 74.04163F);
        this.xrLabel40.StylePriority.UsePadding = false;
        this.xrLabel40.StylePriority.UseTextAlignment = false;
        this.xrLabel40.Text = "EMPLOYEE SIGNATURE";
        this.xrLabel40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel39
        // 
        this.xrLabel39.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(594.6693F, 404.7937F);
        this.xrLabel39.Name = "xrLabel39";
        this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 50, 0, 100F);
        this.xrLabel39.SizeF = new System.Drawing.SizeF(284.4465F, 74.04163F);
        this.xrLabel39.StylePriority.UseBorders = false;
        this.xrLabel39.StylePriority.UsePadding = false;
        this.xrLabel39.StylePriority.UseTextAlignment = false;
        this.xrLabel39.Text = "HR SIGNATURE";
        this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel20
        // 
        this.xrLabel20.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(279.1541F, 404.7937F);
        this.xrLabel20.Name = "xrLabel20";
        this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 50, 0, 100F);
        this.xrLabel20.SizeF = new System.Drawing.SizeF(265.9557F, 74.04163F);
        this.xrLabel20.StylePriority.UseFont = false;
        this.xrLabel20.StylePriority.UsePadding = false;
        this.xrLabel20.StylePriority.UseTextAlignment = false;
        this.xrLabel20.Text = "EMPLOYEE SIGNATURE";
        this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel19
        // 
        this.xrLabel19.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel19.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(0.7791519F, 404.7937F);
        this.xrLabel19.Name = "xrLabel19";
        this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 50, 0, 100F);
        this.xrLabel19.SizeF = new System.Drawing.SizeF(278.3749F, 74.04163F);
        this.xrLabel19.StylePriority.UseBorders = false;
        this.xrLabel19.StylePriority.UseFont = false;
        this.xrLabel19.StylePriority.UsePadding = false;
        this.xrLabel19.StylePriority.UseTextAlignment = false;
        this.xrLabel19.Text = "HR SIGNATURE";
        this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
        // 
        // xrLabel17
        // 
        this.xrLabel17.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Reason")});
        this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(594.6693F, 290.127F);
        this.xrLabel17.Name = "xrLabel17";
        this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel17.SizeF = new System.Drawing.SizeF(545.3307F, 114.6667F);
        this.xrLabel17.StylePriority.UseBorders = false;
        this.xrLabel17.Text = "xrLabel7";
        // 
        // xrLabel10
        // 
        this.xrLabel10.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(594.6693F, 267.127F);
        this.xrLabel10.Name = "xrLabel10";
        this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel10.SizeF = new System.Drawing.SizeF(545.3306F, 23.00002F);
        this.xrLabel10.StylePriority.UseBorders = false;
        this.xrLabel10.StylePriority.UseFont = false;
        this.xrLabel10.Text = "REASON";
        // 
        // xrLabel7
        // 
        this.xrLabel7.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Reason")});
        this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0.7791519F, 290.127F);
        this.xrLabel7.Name = "xrLabel7";
        this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel7.SizeF = new System.Drawing.SizeF(544.3304F, 114.6667F);
        this.xrLabel7.StylePriority.UseBorders = false;
        this.xrLabel7.Text = "xrLabel7";
        // 
        // xrLabel6
        // 
        this.xrLabel6.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
        this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0.7791519F, 267.127F);
        this.xrLabel6.Name = "xrLabel6";
        this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.xrLabel6.SizeF = new System.Drawing.SizeF(544.3306F, 23.00002F);
        this.xrLabel6.StylePriority.UseBorders = false;
        this.xrLabel6.StylePriority.UseFont = false;
        this.xrLabel6.Text = "REASON";
        // 
        // xrTable8
        // 
        this.xrTable8.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTable8.LocationFloat = new DevExpress.Utils.PointFloat(594.6693F, 0F);
        this.xrTable8.Name = "xrTable8";
        this.xrTable8.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow34,
            this.xrTableRow37,
            this.xrTableRow38,
            this.xrTableRow39,
            this.xrTableRow13,
            this.xrTableRow14,
            this.xrTableRow40,
            this.xrTableRow41,
            this.xrTableRow7,
            this.xrTableRow8});
        this.xrTable8.SizeF = new System.Drawing.SizeF(545.3306F, 266.4088F);
        this.xrTable8.StylePriority.UseBorders = false;
        this.xrTable8.StylePriority.UseFont = false;
        // 
        // xrTableRow34
        // 
        this.xrTableRow34.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell106});
        this.xrTableRow34.Name = "xrTableRow34";
        this.xrTableRow34.Weight = 1D;
        // 
        // xrTableCell106
        // 
        this.xrTableCell106.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell106.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell106.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
        this.xrTableCell106.Name = "xrTableCell106";
        this.xrTableCell106.StylePriority.UseBackColor = false;
        this.xrTableCell106.StylePriority.UseBorders = false;
        this.xrTableCell106.StylePriority.UseFont = false;
        this.xrTableCell106.Text = "TERMINATION DETAIL";
        this.xrTableCell106.Weight = 3.7024978141204112D;
        // 
        // xrTableRow37
        // 
        this.xrTableRow37.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell108,
            this.xrTableCell114,
            this.xrTableCell115});
        this.xrTableRow37.Name = "xrTableRow37";
        this.xrTableRow37.Weight = 1D;
        // 
        // xrTableCell108
        // 
        this.xrTableCell108.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell108.Name = "xrTableCell108";
        this.xrTableCell108.StylePriority.UseFont = false;
        this.xrTableCell108.Text = "Month Salary";
        this.xrTableCell108.Weight = 1.5068957282933293D;
        // 
        // xrTableCell114
        // 
        this.xrTableCell114.Name = "xrTableCell114";
        this.xrTableCell114.Text = ":";
        this.xrTableCell114.Weight = 0.17081755059383175D;
        // 
        // xrTableCell115
        // 
        this.xrTableCell115.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Month_Salary")});
        this.xrTableCell115.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell115.Name = "xrTableCell115";
        this.xrTableCell115.StylePriority.UseFont = false;
        this.xrTableCell115.StylePriority.UseTextAlignment = false;
        this.xrTableCell115.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell115.Weight = 2.0247845352332505D;
        this.xrTableCell115.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell115_BeforePrint);
        // 
        // xrTableRow38
        // 
        this.xrTableRow38.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell116,
            this.xrTableCell117,
            this.xrTableCell125});
        this.xrTableRow38.Name = "xrTableRow38";
        this.xrTableRow38.Weight = 1D;
        // 
        // xrTableCell116
        // 
        this.xrTableCell116.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell116.Name = "xrTableCell116";
        this.xrTableCell116.StylePriority.UseFont = false;
        this.xrTableCell116.Text = "Next Month Adjustment";
        this.xrTableCell116.Weight = 1.5068953138977836D;
        // 
        // xrTableCell117
        // 
        this.xrTableCell117.Name = "xrTableCell117";
        this.xrTableCell117.Text = ":";
        this.xrTableCell117.Weight = 0.1708179587696339D;
        // 
        // xrTableCell125
        // 
        this.xrTableCell125.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.NextMonth_Amount")});
        this.xrTableCell125.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell125.Name = "xrTableCell125";
        this.xrTableCell125.StylePriority.UseFont = false;
        this.xrTableCell125.StylePriority.UseTextAlignment = false;
        this.xrTableCell125.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell125.Weight = 2.0247845414529944D;
        this.xrTableCell125.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell125_BeforePrint);
        // 
        // xrTableRow39
        // 
        this.xrTableRow39.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell126,
            this.xrTableCell127,
            this.xrTableCell128});
        this.xrTableRow39.Name = "xrTableRow39";
        this.xrTableRow39.Weight = 1D;
        // 
        // xrTableCell126
        // 
        this.xrTableCell126.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell126.Name = "xrTableCell126";
        this.xrTableCell126.StylePriority.UseFont = false;
        this.xrTableCell126.Text = "Loan Amount";
        this.xrTableCell126.Weight = 1.5068953138978296D;
        // 
        // xrTableCell127
        // 
        this.xrTableCell127.Name = "xrTableCell127";
        this.xrTableCell127.Text = ":";
        this.xrTableCell127.Weight = 0.17081796498933133D;
        // 
        // xrTableCell128
        // 
        this.xrTableCell128.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.LoanFinal_Amount")});
        this.xrTableCell128.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell128.Name = "xrTableCell128";
        this.xrTableCell128.StylePriority.UseFont = false;
        this.xrTableCell128.StylePriority.UseTextAlignment = false;
        this.xrTableCell128.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell128.Weight = 2.02478453523325D;
        this.xrTableCell128.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell128_BeforePrint);
        // 
        // xrTableRow40
        // 
        this.xrTableRow40.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell129,
            this.xrTableCell130,
            this.xrTableCell131});
        this.xrTableRow40.Name = "xrTableRow40";
        this.xrTableRow40.Weight = 1D;
        // 
        // xrTableCell129
        // 
        this.xrTableCell129.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell129.Name = "xrTableCell129";
        this.xrTableCell129.StylePriority.UseFont = false;
        this.xrTableCell129.Text = "Total Amount (In Company Currency)";
        this.xrTableCell129.Weight = 1.5068953138978296D;
        // 
        // xrTableCell130
        // 
        this.xrTableCell130.Name = "xrTableCell130";
        this.xrTableCell130.Text = ":";
        this.xrTableCell130.Weight = 0.17081796498933133D;
        // 
        // xrTableCell131
        // 
        this.xrTableCell131.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Company_TotalAmount")});
        this.xrTableCell131.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell131.Name = "xrTableCell131";
        this.xrTableCell131.StylePriority.UseFont = false;
        this.xrTableCell131.StylePriority.UseTextAlignment = false;
        this.xrTableCell131.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell131.Weight = 2.02478453523325D;
        this.xrTableCell131.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell131_BeforePrint);
        // 
        // xrTableRow41
        // 
        this.xrTableRow41.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell132,
            this.xrTableCell133,
            this.xrTableCell134});
        this.xrTableRow41.Name = "xrTableRow41";
        this.xrTableRow41.Weight = 1D;
        // 
        // xrTableCell132
        // 
        this.xrTableCell132.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell132.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell132.Name = "xrTableCell132";
        this.xrTableCell132.StylePriority.UseBorders = false;
        this.xrTableCell132.StylePriority.UseFont = false;
        this.xrTableCell132.Text = "Total Amount (In Employee Currency)";
        this.xrTableCell132.Weight = 1.5068953138978296D;
        // 
        // xrTableCell133
        // 
        this.xrTableCell133.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell133.Name = "xrTableCell133";
        this.xrTableCell133.StylePriority.UseBorders = false;
        this.xrTableCell133.Text = ":";
        this.xrTableCell133.Weight = 0.17081834736347346D;
        // 
        // xrTableCell134
        // 
        this.xrTableCell134.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell134.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Employee_TotalAmount")});
        this.xrTableCell134.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell134.Name = "xrTableCell134";
        this.xrTableCell134.StylePriority.UseBorders = false;
        this.xrTableCell134.StylePriority.UseFont = false;
        this.xrTableCell134.StylePriority.UseTextAlignment = false;
        this.xrTableCell134.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell134.Weight = 2.024784152859108D;
        this.xrTableCell134.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell134_BeforePrint);
        // 
        // xrTableRow7
        // 
        this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21});
        this.xrTableRow7.Name = "xrTableRow7";
        this.xrTableRow7.Weight = 1D;
        // 
        // xrTableCell19
        // 
        this.xrTableCell19.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell19.Name = "xrTableCell19";
        this.xrTableCell19.StylePriority.UseFont = false;
        this.xrTableCell19.Text = "Paid Amount (In Company Currency)";
        this.xrTableCell19.Weight = 1.5068953138978296D;
        // 
        // xrTableCell20
        // 
        this.xrTableCell20.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell20.Name = "xrTableCell20";
        this.xrTableCell20.StylePriority.UseFont = false;
        this.xrTableCell20.Text = ":";
        this.xrTableCell20.Weight = 0.17081834736347323D;
        // 
        // xrTableCell21
        // 
        this.xrTableCell21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Company_PaidAmount")});
        this.xrTableCell21.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell21.Name = "xrTableCell21";
        this.xrTableCell21.StylePriority.UseFont = false;
        this.xrTableCell21.StylePriority.UseTextAlignment = false;
        this.xrTableCell21.Text = "xrTableCell21";
        this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell21.Weight = 2.024784152859108D;
        this.xrTableCell21.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell21_BeforePrint);
        // 
        // xrTableRow8
        // 
        this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell24});
        this.xrTableRow8.Name = "xrTableRow8";
        this.xrTableRow8.Weight = 1D;
        // 
        // xrTableCell22
        // 
        this.xrTableCell22.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell22.Name = "xrTableCell22";
        this.xrTableCell22.StylePriority.UseFont = false;
        this.xrTableCell22.Text = "Paid Amoint (In Employee Currency)";
        this.xrTableCell22.Weight = 1.5068953138978296D;
        // 
        // xrTableCell23
        // 
        this.xrTableCell23.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell23.Name = "xrTableCell23";
        this.xrTableCell23.StylePriority.UseFont = false;
        this.xrTableCell23.Text = ":";
        this.xrTableCell23.Weight = 0.17081834736347346D;
        // 
        // xrTableCell24
        // 
        this.xrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Employee_PaidAmount")});
        this.xrTableCell24.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell24.Name = "xrTableCell24";
        this.xrTableCell24.StylePriority.UseFont = false;
        this.xrTableCell24.StylePriority.UseTextAlignment = false;
        this.xrTableCell24.Text = "xrTableCell24";
        this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell24.Weight = 2.024784152859108D;
        this.xrTableCell24.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell24_BeforePrint);
        // 
        // xrTable11
        // 
        this.xrTable11.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTable11.LocationFloat = new DevExpress.Utils.PointFloat(0.7790883F, 0F);
        this.xrTable11.Name = "xrTable11";
        this.xrTable11.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow23,
            this.xrTableRow24,
            this.xrTableRow25,
            this.xrTableRow26,
            this.xrTableRow11,
            this.xrTableRow12,
            this.xrTableRow27,
            this.xrTableRow28,
            this.xrTableRow61,
            this.xrTableRow62});
        this.xrTable11.SizeF = new System.Drawing.SizeF(544.3306F, 266.4088F);
        this.xrTable11.StylePriority.UseBorders = false;
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
        this.xrTableCell51.BackColor = System.Drawing.Color.Empty;
        this.xrTableCell51.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell51.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold);
        this.xrTableCell51.Name = "xrTableCell51";
        this.xrTableCell51.StylePriority.UseBackColor = false;
        this.xrTableCell51.StylePriority.UseBorders = false;
        this.xrTableCell51.StylePriority.UseFont = false;
        this.xrTableCell51.Text = "TERMINATION DETAIL";
        this.xrTableCell51.Weight = 3.6957091870433487D;
        // 
        // xrTableRow24
        // 
        this.xrTableRow24.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell53,
            this.xrTableCell73,
            this.xrTableCell54});
        this.xrTableRow24.Name = "xrTableRow24";
        this.xrTableRow24.Weight = 1D;
        // 
        // xrTableCell53
        // 
        this.xrTableCell53.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell53.Name = "xrTableCell53";
        this.xrTableCell53.StylePriority.UseFont = false;
        this.xrTableCell53.Text = "Month Salary";
        this.xrTableCell53.Weight = 1.6330549563686232D;
        // 
        // xrTableCell73
        // 
        this.xrTableCell73.Name = "xrTableCell73";
        this.xrTableCell73.Text = ":";
        this.xrTableCell73.Weight = 0.25696004274322426D;
        // 
        // xrTableCell54
        // 
        this.xrTableCell54.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Month_Salary")});
        this.xrTableCell54.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell54.Name = "xrTableCell54";
        this.xrTableCell54.StylePriority.UseFont = false;
        this.xrTableCell54.StylePriority.UseTextAlignment = false;
        this.xrTableCell54.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell54.Weight = 1.8056941879315014D;
        this.xrTableCell54.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell54_BeforePrint);
        // 
        // xrTableRow25
        // 
        this.xrTableRow25.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell55,
            this.xrTableCell74,
            this.xrTableCell56});
        this.xrTableRow25.Name = "xrTableRow25";
        this.xrTableRow25.Weight = 1D;
        // 
        // xrTableCell55
        // 
        this.xrTableCell55.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell55.Name = "xrTableCell55";
        this.xrTableCell55.StylePriority.UseFont = false;
        this.xrTableCell55.Text = "Next Month Adjustment";
        this.xrTableCell55.Weight = 1.6330549563686234D;
        // 
        // xrTableCell74
        // 
        this.xrTableCell74.Name = "xrTableCell74";
        this.xrTableCell74.Text = ":";
        this.xrTableCell74.Weight = 0.25696003652355037D;
        // 
        // xrTableCell56
        // 
        this.xrTableCell56.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.NextMonth_Amount")});
        this.xrTableCell56.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell56.Name = "xrTableCell56";
        this.xrTableCell56.StylePriority.UseFont = false;
        this.xrTableCell56.StylePriority.UseTextAlignment = false;
        this.xrTableCell56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell56.Weight = 1.8056941941511759D;
        this.xrTableCell56.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell56_BeforePrint);
        // 
        // xrTableRow26
        // 
        this.xrTableRow26.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell57,
            this.xrTableCell75,
            this.xrTableCell58});
        this.xrTableRow26.Name = "xrTableRow26";
        this.xrTableRow26.Weight = 1D;
        // 
        // xrTableCell57
        // 
        this.xrTableCell57.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell57.Name = "xrTableCell57";
        this.xrTableCell57.StylePriority.UseFont = false;
        this.xrTableCell57.Text = "Loan Amount";
        this.xrTableCell57.Weight = 1.6330547491708503D;
        // 
        // xrTableCell75
        // 
        this.xrTableCell75.Name = "xrTableCell75";
        this.xrTableCell75.Text = ":";
        this.xrTableCell75.Weight = 0.25696024994099731D;
        // 
        // xrTableCell58
        // 
        this.xrTableCell58.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.LoanFinal_Amount")});
        this.xrTableCell58.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell58.Name = "xrTableCell58";
        this.xrTableCell58.StylePriority.UseFont = false;
        this.xrTableCell58.StylePriority.UseTextAlignment = false;
        this.xrTableCell58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell58.Weight = 1.8056941879315016D;
        this.xrTableCell58.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell58_BeforePrint);
        // 
        // xrTableRow27
        // 
        this.xrTableRow27.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell59,
            this.xrTableCell76,
            this.xrTableCell60});
        this.xrTableRow27.Name = "xrTableRow27";
        this.xrTableRow27.Weight = 1D;
        // 
        // xrTableCell59
        // 
        this.xrTableCell59.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell59.Name = "xrTableCell59";
        this.xrTableCell59.StylePriority.UseFont = false;
        this.xrTableCell59.Text = "Total Amount (In Company Currency)";
        this.xrTableCell59.Weight = 1.6330547491708503D;
        // 
        // xrTableCell76
        // 
        this.xrTableCell76.Name = "xrTableCell76";
        this.xrTableCell76.Text = ":";
        this.xrTableCell76.Weight = 0.25696024994099742D;
        // 
        // xrTableCell60
        // 
        this.xrTableCell60.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Company_TotalAmount")});
        this.xrTableCell60.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell60.Name = "xrTableCell60";
        this.xrTableCell60.StylePriority.UseFont = false;
        this.xrTableCell60.StylePriority.UseTextAlignment = false;
        this.xrTableCell60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell60.Weight = 1.8056941879315016D;
        this.xrTableCell60.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell60_BeforePrint);
        // 
        // xrTableRow28
        // 
        this.xrTableRow28.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell61,
            this.xrTableCell77,
            this.xrTableCell62});
        this.xrTableRow28.Name = "xrTableRow28";
        this.xrTableRow28.Weight = 1D;
        // 
        // xrTableCell61
        // 
        this.xrTableCell61.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell61.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell61.Name = "xrTableCell61";
        this.xrTableCell61.StylePriority.UseBorders = false;
        this.xrTableCell61.StylePriority.UseFont = false;
        this.xrTableCell61.Text = "Total Amount (In Employee Currency)";
        this.xrTableCell61.Weight = 1.6330549563686232D;
        // 
        // xrTableCell77
        // 
        this.xrTableCell77.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell77.Name = "xrTableCell77";
        this.xrTableCell77.StylePriority.UseBorders = false;
        this.xrTableCell77.Text = ":";
        this.xrTableCell77.Weight = 0.25696021791965129D;
        // 
        // xrTableCell62
        // 
        this.xrTableCell62.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right)
                    | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.xrTableCell62.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Employee_TotalAmount")});
        this.xrTableCell62.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell62.Name = "xrTableCell62";
        this.xrTableCell62.StylePriority.UseBorders = false;
        this.xrTableCell62.StylePriority.UseFont = false;
        this.xrTableCell62.StylePriority.UseTextAlignment = false;
        this.xrTableCell62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell62.Weight = 1.805694012755074D;
        this.xrTableCell62.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell62_BeforePrint);
        // 
        // xrTableRow61
        // 
        this.xrTableRow61.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell48,
            this.xrTableCell70,
            this.xrTableCell72});
        this.xrTableRow61.Name = "xrTableRow61";
        this.xrTableRow61.Weight = 1D;
        // 
        // xrTableCell48
        // 
        this.xrTableCell48.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell48.Name = "xrTableCell48";
        this.xrTableCell48.StylePriority.UseFont = false;
        this.xrTableCell48.Text = "Paid Amount (In Company Currency)";
        this.xrTableCell48.Weight = 1.6330549563686232D;
        // 
        // xrTableCell70
        // 
        this.xrTableCell70.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell70.Name = "xrTableCell70";
        this.xrTableCell70.StylePriority.UseFont = false;
        this.xrTableCell70.Text = ":";
        this.xrTableCell70.Weight = 0.25696001072187813D;
        // 
        // xrTableCell72
        // 
        this.xrTableCell72.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Company_PaidAmount")});
        this.xrTableCell72.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell72.Name = "xrTableCell72";
        this.xrTableCell72.StylePriority.UseFont = false;
        this.xrTableCell72.StylePriority.UseTextAlignment = false;
        this.xrTableCell72.Text = "xrTableCell72";
        this.xrTableCell72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell72.Weight = 1.8056942199528472D;
        this.xrTableCell72.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell72_BeforePrint);
        // 
        // xrTableRow62
        // 
        this.xrTableRow62.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell78,
            this.xrTableCell189,
            this.xrTableCell192});
        this.xrTableRow62.Name = "xrTableRow62";
        this.xrTableRow62.Weight = 1D;
        // 
        // xrTableCell78
        // 
        this.xrTableCell78.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell78.Name = "xrTableCell78";
        this.xrTableCell78.StylePriority.UseFont = false;
        this.xrTableCell78.Text = "Paid Amoint (In Employee Currency)";
        this.xrTableCell78.Weight = 1.6330549563686232D;
        // 
        // xrTableCell189
        // 
        this.xrTableCell189.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell189.Name = "xrTableCell189";
        this.xrTableCell189.StylePriority.UseFont = false;
        this.xrTableCell189.Text = ":";
        this.xrTableCell189.Weight = 0.25696001072187813D;
        // 
        // xrTableCell192
        // 
        this.xrTableCell192.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Employee_PaidAmount")});
        this.xrTableCell192.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.xrTableCell192.Name = "xrTableCell192";
        this.xrTableCell192.StylePriority.UseFont = false;
        this.xrTableCell192.StylePriority.UseTextAlignment = false;
        this.xrTableCell192.Text = "xrTableCell192";
        this.xrTableCell192.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell192.Weight = 1.8056942199528472D;
        this.xrTableCell192.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell192_BeforePrint);
        // 
        // xrTableRow11
        // 
        this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell31,
            this.xrTableCell32,
            this.xrTableCell33});
        this.xrTableRow11.Name = "xrTableRow11";
        this.xrTableRow11.Weight = 1D;
        // 
        // xrTableCell31
        // 
        this.xrTableCell31.Name = "xrTableCell31";
        this.xrTableCell31.Text = "Leave Salary";
        this.xrTableCell31.Weight = 1.6330547491708503D;
        // 
        // xrTableCell32
        // 
        this.xrTableCell32.Name = "xrTableCell32";
        this.xrTableCell32.Text = ":";
        this.xrTableCell32.Weight = 0.25696024994099731D;
        // 
        // xrTableCell33
        // 
        this.xrTableCell33.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Field2")});
        this.xrTableCell33.Name = "xrTableCell33";
        this.xrTableCell33.StylePriority.UseTextAlignment = false;
        this.xrTableCell33.Text = "xrTableCell33";
        this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell33.Weight = 1.8056941879315016D;
        this.xrTableCell33.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell33_BeforePrint);
        // 
        // xrTableRow12
        // 
        this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell34,
            this.xrTableCell35,
            this.xrTableCell36});
        this.xrTableRow12.Name = "xrTableRow12";
        this.xrTableRow12.Weight = 1D;
        // 
        // xrTableCell34
        // 
        this.xrTableCell34.Name = "xrTableCell34";
        this.xrTableCell34.Text = "Indemnity Salary";
        this.xrTableCell34.Weight = 1.6330547491708503D;
        // 
        // xrTableCell35
        // 
        this.xrTableCell35.Name = "xrTableCell35";
        this.xrTableCell35.Text = ":";
        this.xrTableCell35.Weight = 0.25696024994099731D;
        // 
        // xrTableCell36
        // 
        this.xrTableCell36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Field1")});
        this.xrTableCell36.Name = "xrTableCell36";
        this.xrTableCell36.StylePriority.UseTextAlignment = false;
        this.xrTableCell36.Text = "xrTableCell36";
        this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell36.Weight = 1.8056941879315016D;
        this.xrTableCell36.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell36_BeforePrint);
        // 
        // xrTableRow13
        // 
        this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell39});
        this.xrTableRow13.Name = "xrTableRow13";
        this.xrTableRow13.Weight = 1D;
        // 
        // xrTableCell37
        // 
        this.xrTableCell37.Name = "xrTableCell37";
        this.xrTableCell37.Text = "Leave Salary";
        this.xrTableCell37.Weight = 1.5068953138978296D;
        // 
        // xrTableCell38
        // 
        this.xrTableCell38.Name = "xrTableCell38";
        this.xrTableCell38.Text = ":";
        this.xrTableCell38.Weight = 0.17081796498933133D;
        // 
        // xrTableCell39
        // 
        this.xrTableCell39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Field2")});
        this.xrTableCell39.Name = "xrTableCell39";
        this.xrTableCell39.StylePriority.UseTextAlignment = false;
        this.xrTableCell39.Text = "xrTableCell39";
        this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell39.Weight = 2.02478453523325D;
        this.xrTableCell39.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell39_BeforePrint);
        // 
        // xrTableRow14
        // 
        this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell40,
            this.xrTableCell41,
            this.xrTableCell42});
        this.xrTableRow14.Name = "xrTableRow14";
        this.xrTableRow14.Weight = 1D;
        // 
        // xrTableCell40
        // 
        this.xrTableCell40.Name = "xrTableCell40";
        this.xrTableCell40.Text = "Indemnity Salary";
        this.xrTableCell40.Weight = 1.5068953138978296D;
        // 
        // xrTableCell41
        // 
        this.xrTableCell41.Name = "xrTableCell41";
        this.xrTableCell41.Text = ":";
        this.xrTableCell41.Weight = 0.17081796498933133D;
        // 
        // xrTableCell42
        // 
        this.xrTableCell42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Pay_Termination_selectRow.Field1")});
        this.xrTableCell42.Name = "xrTableCell42";
        this.xrTableCell42.StylePriority.UseTextAlignment = false;
        this.xrTableCell42.Text = "xrTableCell42";
        this.xrTableCell42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        this.xrTableCell42.Weight = 2.02478453523325D;
        this.xrTableCell42.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell42_BeforePrint);
        // 
        // EmployeeTerminationReport
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.GroupHeader4,
            this.GroupHeader2});
        this.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
        this.DataMember = "sp_Pay_Termination_selectRow";
        this.DataSource = this.employeeterminationDataSet1;
        this.Font = new System.Drawing.Font("Times New Roman", 9.75F);
        this.Margins = new System.Drawing.Printing.Margins(33, 27, 113, 40);
        this.PageHeight = 1169;
        this.PageWidth = 1200;
        this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
        this.Version = "14.1";
        ((System.ComponentModel.ISupportInitialize)(this.employeePaySlipDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.employeeterminationDataSet1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).EndInit();
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


    public void setUserName(string UserName)
    {
        xrLabel23.Text = UserName;
    }
    public void setUserName1(string UserName)
    {
        xrLabel25.Text = UserName;
    }
    public void setImageUrl(string Url)
    {
        try
        {
            xrPictureBox2.ImageUrl = Url;
            xrPictureBox4.ImageUrl = Url;
        }
        catch
        {
        }
    }
    





   
 
  
    







    private void xrTableCell15_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell15.Text != "")
        {
            xrTableCell15.Text = Convert.ToDateTime(xrTableCell15.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }

    }

    private void xrTableCell96_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        if (xrTableCell96.Text != "")
        {
            xrTableCell96.Text = Convert.ToDateTime(xrTableCell96.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }

    }

    private void xrPictureBox2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            if (GetCurrentColumnValue("EmpImageUrl").ToString() != null)
            {
                xrPictureBox2.ImageUrl = GetCurrentColumnValue("EmpImageUrl").ToString();
            }
        }
        catch
        {
        }
    }

    private void xrPictureBox4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            if (GetCurrentColumnValue("EmpImageUrl").ToString() != null)
            {
                xrPictureBox4.ImageUrl = GetCurrentColumnValue("EmpImageUrl").ToString();
            }
        }
        catch
        {
        }

    }






   





   



  

   
    

  

  
    private void xrTableCell18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell18.Text = Convert.ToDateTime(xrTableCell18.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        catch
        {
        }
    }

    private void xrTableCell100_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell100.Text = Convert.ToDateTime(xrTableCell100.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        catch
        {
        }

    }

    private void xrTableCell60_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell60.Text = objSysParam.GetCurencySymbol(GetCurrentColumnValue("Employee_Id").ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell60.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell62_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell62.Text =objSysParam.GetCurencySymbol_For_EmployeeCurrency(GetCurrentColumnValue("Employee_Id").ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) +objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("EmpCurrencyId").ToString(), xrTableCell62.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell72_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell72.Text =objSysParam.GetCurencySymbol(GetCurrentColumnValue("Employee_Id").ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) +objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell72.Text);
        }
        catch
        {
        }

    }

    private void xrTableCell192_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell192.Text =objSysParam.GetCurencySymbol_For_EmployeeCurrency(GetCurrentColumnValue("Employee_Id").ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("EmpCurrencyId").ToString(), xrTableCell192.Text);
        }
        catch
        {
        }


    }

    private void xrTableCell131_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell131.Text =objSysParam.GetCurencySymbol(GetCurrentColumnValue("Employee_Id").ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell131.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell134_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell134.Text =objSysParam.GetCurencySymbol_For_EmployeeCurrency(GetCurrentColumnValue("Employee_Id").ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("EmpCurrencyId").ToString(), xrTableCell134.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell21_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell21.Text =objSysParam.GetCurencySymbol(GetCurrentColumnValue("Employee_Id").ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) +objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell21.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell24_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell24.Text =objSysParam.GetCurencySymbol_For_EmployeeCurrency(GetCurrentColumnValue("Employee_Id").ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("EmpCurrencyId").ToString(), xrTableCell24.Text);
        }
        catch
        {
        }

    }

    private void xrTableCell54_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell54.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell54.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell56_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell56.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell56.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell58_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell58.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell58.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell115_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell115.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell115.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell125_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell125.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell125.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell128_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell128.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell128.Text);
        }
        catch
        {
        }

    }

    private void xrTableCell33_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell33.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell33.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell36_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell36.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell36.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell39_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell39.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell39.Text);
        }
        catch
        {
        }
    }

    private void xrTableCell42_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        try
        {
            xrTableCell42.Text = objSysParam.GetCurencyConversionForInv(GetCurrentColumnValue("CompanyCurrencyId").ToString(), xrTableCell42.Text);
        }
        catch
        {
        }
    }

}
