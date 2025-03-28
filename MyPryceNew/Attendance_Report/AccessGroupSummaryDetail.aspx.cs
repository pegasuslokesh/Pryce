using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;

public partial class Attendance_Report_AccessGroupSummaryDetail : BasePage
{
    Att_Leave_Request objleaveReq = null;
    Set_ApplicationParameter objAppParam = null;
    SystemParameter ObjSys = null;
    Att_AttendanceRegister ObjRegister = null;
    Attendance objAttendance = null;
    AccessGroupSummary ObjAcessgroupsummary = null;
    //Att_AccessGroupSummary objReport = new Att_AccessGroupSummary();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjRegister = new Att_AttendanceRegister(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAcessgroupsummary = new AccessGroupSummary(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
           
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../Attendance_Report/AccessGroupSummaryDetail.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            if (Session["EmpList"] == null)
            {
                Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
            }

            if (Session["Report_EmpList"] != null)
            {
                Session["SelectedEmpId"] = Session["Report_EmpList"];
            }
            else
            {
                return;
            }
        }
        DataTable dtEmp = (DataTable)Session["dtEmpDetails"];
        string struserName = string.Empty;
        if (dtEmp != null && dtEmp.Rows.Count > 0)
        {
            struserName = dtEmp.Rows[0]["Emp_Id"].ToString() == "0" ? "Superadmin" : dtEmp.Rows[0]["EmpName"].ToString();
        }
        Table tResult = new Table();
        //tResult = ObjAcessgroupsummary.GetReport1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "1564", Convert.ToDateTime(Session["Report_FromDate"].ToString()).ToString(), Convert.ToDateTime(Session["Report_ToDate"].ToString()).ToString(), false, struserName);
        tResult = ObjAcessgroupsummary.GetReport1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["SelectedEmpId"].ToString(), Convert.ToDateTime(Session["Report_FromDate"].ToString()).ToString(), Convert.ToDateTime(Session["Report_ToDate"].ToString()).ToString(), false, struserName);
       // tResult = ObjAcessgroupsummary.GetReport1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["SelectedEmpId"].ToString(), Convert.ToDateTime("2021-08-01").ToString(), Convert.ToDateTime("2021-08-08").ToString(), false, struserName);
        mDiv.Controls.Add(tResult);
    }

    public Table GetReport1(string strCompanyId, string strBrandId, string strLocationId, string strEmpIdList, string strFromDate, string strToDate, bool isDownload, string struserId = "")
    {

        DataTable dt = new DataTable();

        Table Table1 = new Table();

        Table1.Style["Text-align"] = "Left";
        Table1.Style["Text-align"] = "Left";

        // Table1.GridLines = GridLines.Both;
        Table1.Font.Size = 10;
        int Counter = 0;


        Table1.Rows.Clear();


        //adding header here 

        //report name

        TableRow trHeaderReportName = new TableRow();


        TableCell tcHeaderReportName = new TableCell();
        tcHeaderReportName.Text = "Report Name";
        tcHeaderReportName.Font.Bold = true;

        tcHeaderReportName.Font.Size = 12;
        tcHeaderReportName.ColumnSpan = 3;
        tcHeaderReportName.Style["Text-align"] = "Left";
        trHeaderReportName.Cells.Add(tcHeaderReportName);

        TableCell tcHeaderReportvalue = new TableCell();
        tcHeaderReportvalue.Text = "TSC Time Attendance Details Report";
        tcHeaderReportvalue.ColumnSpan = 9;
        tcHeaderReportvalue.Font.Size = 12;
        tcHeaderReportvalue.Style["Text-align"] = "Left";
        trHeaderReportName.Cells.Add(tcHeaderReportvalue);
        Table1.Rows.Add(trHeaderReportName);


        //from date

        TableRow trHeaderFromDate = new TableRow();

        TableCell tcHeaderFromDate = new TableCell();
        tcHeaderFromDate.Text = "From Date";
        tcHeaderFromDate.ColumnSpan = 3;
        tcHeaderFromDate.Font.Size = 12;
        tcHeaderFromDate.Font.Bold = true;
        tcHeaderFromDate.Style["Text-align"] = "Left";
        trHeaderFromDate.Cells.Add(tcHeaderFromDate);

        var span = new LiteralControl("<span style='font-size:1px;'>.</span>" + Convert.ToDateTime(strFromDate).ToString("dd-MMM-yyyy"));
        Literal LitFromdate = new Literal();
        LitFromdate.Text = "";
        TableCell tcHeaderFromDateValue = new TableCell();
        tcHeaderFromDateValue.Text = " " + Convert.ToDateTime(strFromDate).ToString("dd-MMM-yyyy");
        tcHeaderFromDateValue.ColumnSpan = 9;
        tcHeaderFromDateValue.Font.Size = 12;
        tcHeaderFromDateValue.Style["Text-align"] = "Left";
        tcHeaderFromDateValue.Controls.Add(span);
        trHeaderFromDate.Cells.Add(tcHeaderFromDateValue);
        Table1.Rows.Add(trHeaderFromDate);

        //to date

        TableRow trHeaderToDate = new TableRow();

        TableCell tcHeaderToDate = new TableCell();
        tcHeaderToDate.Text = "To Date";
        tcHeaderToDate.ColumnSpan = 3;
        tcHeaderToDate.Font.Bold = true;
        tcHeaderToDate.Font.Size = 12;
        tcHeaderToDate.Style["Text-align"] = "Left";
        trHeaderToDate.Cells.Add(tcHeaderToDate);

        TableCell tcHeaderToDateValue = new TableCell();
        span = new LiteralControl("<span style='font-size:1px;'>.</span>" + Convert.ToDateTime(strToDate).ToString("dd-MMM-yyyy"));
        tcHeaderToDateValue.Text = "" + Convert.ToDateTime(strToDate).ToString("dd-MMM-yyyy");
        tcHeaderToDateValue.ColumnSpan = 9;
        tcHeaderToDateValue.Font.Size = 12;
        tcHeaderToDateValue.Style["Text-align"] = "Left";
        tcHeaderToDateValue.Controls.Add(span);
        trHeaderToDate.Cells.Add(tcHeaderToDateValue);
        Table1.Rows.Add(trHeaderToDate);

        //date time(created date)
        TableRow trHeaderDateTime = new TableRow();

        TableCell tcHeaderDateTime = new TableCell();
        tcHeaderDateTime.Text = "Created Date";
        tcHeaderDateTime.ColumnSpan = 3;
        tcHeaderDateTime.Font.Bold = true;
        tcHeaderDateTime.Font.Size = 12;
        tcHeaderDateTime.Style["Text-align"] = "Left";
        trHeaderDateTime.Cells.Add(tcHeaderDateTime);

        TableCell tcHeaderDateTimeValue = new TableCell();
        span = new LiteralControl("<span style='font-size:1px;'>.</span>" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss"));
        string strdateVal = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString("dd-MMM-yyyy");
        string strdatetime = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("HH:mm:ss");
        //+ " " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("HH:mm:ss");
        tcHeaderDateTimeValue.Text = "'" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss");
        tcHeaderDateTimeValue.ColumnSpan = 9;
        tcHeaderDateTimeValue.Font.Size = 12;
        tcHeaderDateTimeValue.Style["Text-align"] = "Left";
        tcHeaderDateTimeValue.Controls.Add(span);
        trHeaderDateTime.Cells.Add(tcHeaderDateTimeValue);
        Table1.Rows.Add(trHeaderDateTime);


        //date time(created by)
        TableRow trHeaderCreatedBy = new TableRow();

        TableCell tcHeaderCreatedBy = new TableCell();
        tcHeaderCreatedBy.Text = "Created By";
        tcHeaderCreatedBy.ColumnSpan = 3;
        tcHeaderCreatedBy.Font.Size = 12;
        tcHeaderCreatedBy.Font.Bold = true;
        tcHeaderCreatedBy.Style["Text-align"] = "Left";
        trHeaderCreatedBy.Cells.Add(tcHeaderCreatedBy);

        TableCell tcHeaderCreatedByValue = new TableCell();
        tcHeaderCreatedByValue.Text = struserId;
        tcHeaderCreatedByValue.ColumnSpan = 9;
        tcHeaderCreatedByValue.Font.Size = 12;
        tcHeaderCreatedByValue.Style["Text-align"] = "Left";
        trHeaderCreatedBy.Cells.Add(tcHeaderCreatedByValue);
        Table1.Rows.Add(trHeaderCreatedBy);



        DataTable dtColorCode = new DataTable();
        dtColorCode.Columns.Add("Type");
        dtColorCode.Columns.Add("ShortId");
        dtColorCode.Columns.Add("ColorCode");

        DataRow dr1 = dtColorCode.NewRow();
        dr1["Type"] = "Absent";
        dr1["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", strCompanyId, strBrandId, strLocationId);

        //  lblabsent.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr1["ColorCode"].ToString()).ToString());


        dr1["ShortId"] = "AB";
        dtColorCode.Rows.Add(dr1);




        DataRow dr2 = dtColorCode.NewRow();
        dr2["Type"] = "Normal";
        dr2["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", strCompanyId, strBrandId, strLocationId);
        //lblPresent.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr2["ColorCode"].ToString()).ToString());
        dr2["ShortId"] = "N";
        dtColorCode.Rows.Add(dr2);


        DataRow dr3 = dtColorCode.NewRow();
        dr3["Type"] = "Holiday";
        dr3["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", strCompanyId, strBrandId, strLocationId);
        // lblHoliday.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr3["ColorCode"].ToString()).ToString());
        dr3["ShortId"] = "PH";
        dtColorCode.Rows.Add(dr3);

        DataRow dr4 = dtColorCode.NewRow();
        dr4["Type"] = "Leave";
        dr4["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", strCompanyId, strBrandId, strLocationId);
        // lblleave.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr4["ColorCode"].ToString()).ToString());
        dr4["ShortId"] = "L";
        dtColorCode.Rows.Add(dr4);


        DataRow dr5 = dtColorCode.NewRow();

        dr5["Type"] = "WeekOff";
        dr5["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", strCompanyId, strBrandId, strLocationId);
        // lblweekoff.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr5["ColorCode"].ToString()).ToString());
        dr5["ShortId"] = "WO";
        dtColorCode.Rows.Add(dr5);


        DataRow dr6 = dtColorCode.NewRow();
        dr6["Type"] = "Missed Out";
        dr6["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("MO_Color_Code", strCompanyId, strBrandId, strLocationId);
        // lblMissedout.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr6["ColorCode"].ToString()).ToString());
        dr6["ShortId"] = "MO";
        dtColorCode.Rows.Add(dr6);



        DataRow dr7 = dtColorCode.NewRow();
        dr7["Type"] = "Missed In";
        dr7["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("MI_Color_Code", strCompanyId, strBrandId, strLocationId);
        // lblMissedIn.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr7["ColorCode"].ToString()).ToString());
        dr7["ShortId"] = "MI";
        dtColorCode.Rows.Add(dr7);



        DataRow dr8 = dtColorCode.NewRow();
        dr8["Type"] = "Late Check IN";
        dr8["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Late_Color_Code", strCompanyId, strBrandId, strLocationId);
        // lbllatein.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr8["ColorCode"].ToString()).ToString());
        dr8["ShortId"] = "";
        dtColorCode.Rows.Add(dr8);


        DataRow dr9 = dtColorCode.NewRow();
        dr9["Type"] = "Early Check Out";
        dr9["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Early_Color_Code", strCompanyId, strBrandId, strLocationId);
        //  lblearlyout.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr9["ColorCode"].ToString()).ToString());
        dr9["ShortId"] = "";
        dtColorCode.Rows.Add(dr9);

        DataRow dr10 = dtColorCode.NewRow();
        dr10["Type"] = "NR_Color_Code";
        dr10["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("NR_Color_Code", strCompanyId, strBrandId, strLocationId);
        //  lblearlyout.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr9["ColorCode"].ToString()).ToString());
        dr10["ShortId"] = "";
        dtColorCode.Rows.Add(dr10);

        DataRow dr11 = dtColorCode.NewRow();
        dr11["Type"] = "NS_Color_Code";
        dr11["ColorCode"] = "fce9da";
        dr11["ShortId"] = "";
        dtColorCode.Rows.Add(dr11);

        DataRow dr12 = dtColorCode.NewRow();
        dr12["Type"] = "OT_Color_Code";
        dr12["ColorCode"] = "ffd800";
        //  lblearlyout.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr9["ColorCode"].ToString()).ToString());
        dr12["ShortId"] = "";
        dtColorCode.Rows.Add(dr12);


        //set color and short code
        string Normal = "P";
        string Absent = (new DataView(dtColorCode, "Type='Absent'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();

        string WeekOff = (new DataView(dtColorCode, "Type='WeekOff'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string Holiday = (new DataView(dtColorCode, "Type='Holiday'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string MIssedIn = (new DataView(dtColorCode, "Type='Missed In'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string MIssedout = (new DataView(dtColorCode, "Type='Missed Out'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string LateIn = (new DataView(dtColorCode, "Type='Late Check IN'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string Earlyout = (new DataView(dtColorCode, "Type='Early Check Out'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();

        string Leave = "L";
        string Normalcol = (new DataView(dtColorCode, "Type='Normal'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string Absentcol = (new DataView(dtColorCode, "Type='Absent'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();

        string Leavecol = (new DataView(dtColorCode, "Type='Leave'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string WeekOffcol = (new DataView(dtColorCode, "Type='WeekOff'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string Holidaycol = (new DataView(dtColorCode, "Type='Holiday'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string MIssedIncol = (new DataView(dtColorCode, "Type='Missed In'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string MIssedoutcol = (new DataView(dtColorCode, "Type='Missed Out'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string LateIncol = (new DataView(dtColorCode, "Type='Late Check IN'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string Earlyoutcol = (new DataView(dtColorCode, "Type='Early Check Out'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string NRcol = (new DataView(dtColorCode, "Type='NR_Color_Code'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string NScol = (new DataView(dtColorCode, "Type='NS_Color_Code'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string OTcol = (new DataView(dtColorCode, "Type='OT_Color_Code'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();


        TableRow TrTopBlankRow = new TableRow();
        TableCell TcTopBlankcell = new TableCell();
        TcTopBlankcell.Text = "-";
        TcTopBlankcell.ColumnSpan = 12;
        TrTopBlankRow.Cells.Add(TcTopBlankcell);
        Table1.Rows.Add(TrTopBlankRow);


        //here we are added code for show color code



        TableRow trcolor = new TableRow();

        TableCell tcNR = new TableCell();
        tcNR.Text = "NR-Not Registered";
        tcNR.ColumnSpan = 2;
        tcNR.Font.Size = 12;
        tcNR.Wrap = false;
        tcNR.Style["Text-align"] = "Center";
        trcolor.Cells.Add(tcNR);
        try
        {
            tcNR.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(NRcol).ToString());
        }
        catch
        {
            tcNR.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }




        TableCell tcNS = new TableCell();
        tcNS.Text = "NS-No Shift";
        tcNS.ColumnSpan = 1;
        tcNS.Font.Size = 12;
        tcNS.Wrap = false;
        tcNS.Style["Text-align"] = "Center";
        tcNS.ForeColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ff0000").ToString());
        trcolor.Cells.Add(tcNS);
        try
        {
            tcNS.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(NScol).ToString());
        }
        catch
        {
            tcNS.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }


        TableCell tcPresent = new TableCell();
        tcPresent.Text = "Present";
        tcPresent.Wrap = false;
        tcPresent.Font.Size = 12;
        tcPresent.Style["Text-align"] = "Center";
        tcPresent.ColumnSpan = 1;
        trcolor.Cells.Add(tcPresent);
        try
        {
            tcPresent.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Normalcol).ToString());
        }
        catch
        {
            tcPresent.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }


        TableCell tcWo = new TableCell();
        tcWo.Text = "WO-Week OFF";
        tcWo.ColumnSpan = 2;
        tcWo.Font.Size = 12;
        tcWo.Wrap = false;
        tcWo.Style["Text-align"] = "Center";
        trcolor.Cells.Add(tcWo);
        try
        {
            tcWo.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(WeekOffcol).ToString());
        }
        catch
        {
            tcWo.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }





        TableCell tcPH = new TableCell();
        tcPH.Text = "PH-Public Holiday";
        tcPH.Style["Text-align"] = "Center";
        tcPH.Wrap = false;
        tcPH.Font.Size = 12;
        tcPH.ColumnSpan = 1;
        trcolor.Cells.Add(tcPH);
        try
        {
            tcPH.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Holidaycol).ToString());
        }
        catch
        {
            tcPH.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }

        TableCell tcLate = new TableCell();
        tcLate.Text = "Late Check IN";
        tcLate.Wrap = false;
        tcLate.Font.Size = 12;
        tcLate.Style["Text-align"] = "Center";
        tcLate.ColumnSpan = 1;
        trcolor.Cells.Add(tcLate);
        try
        {
            tcLate.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(LateIncol).ToString());
        }
        catch
        {
            tcLate.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }


        TableCell tcearly = new TableCell();
        tcearly.Text = "Early Check OUT";
        tcearly.Wrap = false;
        tcearly.Font.Size = 12;
        tcearly.Style["Text-align"] = "Center";
        tcearly.ColumnSpan = 1;
        trcolor.Cells.Add(tcearly);
        try
        {
            tcearly.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Earlyoutcol).ToString());
        }
        catch
        {
            tcearly.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }

        TableCell tcMI = new TableCell();
        tcMI.Text = "MI-Missed IN";
        tcMI.Wrap = false;
        tcMI.Font.Size = 12;
        tcMI.Style["Text-align"] = "Center";
        tcMI.ColumnSpan = 1;
        trcolor.Cells.Add(tcMI);

        try
        {
            tcMI.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(MIssedIncol).ToString());
        }
        catch
        {
            tcMI.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }



        TableCell tcMO = new TableCell();
        tcMO.Text = "MO-Missed OUT";
        tcMO.Wrap = false;
        tcMO.Font.Size = 12;
        tcMO.Style["Text-align"] = "Center";
        tcMO.ColumnSpan = 1;
        trcolor.Cells.Add(tcMO);

        try
        {
            tcMO.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(MIssedoutcol).ToString());
        }
        catch
        {
            tcMO.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }



        TableCell tcLeave = new TableCell();
        tcLeave.Text = "Leave";
        tcLeave.Wrap = false;
        tcLeave.Font.Size = 12;
        tcLeave.Style["Text-align"] = "Center";
        tcLeave.ColumnSpan = 1;
        trcolor.Cells.Add(tcLeave);
        try
        {
            tcLeave.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Leavecol).ToString());
        }
        catch
        {
            tcLeave.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }

        TableCell tcAB = new TableCell();
        tcAB.Text = "AB-Absent";
        tcAB.Wrap = false;
        tcAB.Font.Size = 12;
        tcAB.Style["Text-align"] = "Center";
        tcAB.ColumnSpan = 1;
        trcolor.Cells.Add(tcAB);
        try
        {
            tcAB.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Absentcol).ToString());
        }
        catch
        {
            tcAB.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }


        TableCell tcOT = new TableCell();
        tcOT.Text = "> 16 Hours";
        tcOT.Wrap = false;
        tcOT.Font.Size = 12;
        tcOT.Style["Text-align"] = "Center";
        tcOT.ColumnSpan = 1;
        tcOT.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(OTcol).ToString());
        tcOT.ForeColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ff0000").ToString());
        trcolor.Cells.Add(tcOT);




        Table1.Rows.Add(trcolor);



        TableRow TrBlankRow = new TableRow();
        TableCell TcBlankcell = new TableCell();
        TcBlankcell.Text = "-";
        TcBlankcell.ColumnSpan = 12;
        TrBlankRow.Cells.Add(TcBlankcell);
        Table1.Rows.Add(TrBlankRow);



        //AttendanceDataSet rptdata = new AttendanceDataSet();
        //rptdata.EnforceConstraints = false;
        //AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();

        //try
        //{
        //    adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(Session["Report_FromDate"].ToString()), Convert.ToDateTime(Session["Report_ToDate"].ToString()), Session["SelectedEmpId"].ToString(), 11);
        //}
        //catch
        //{

        //}
        DataTable dtFilter = ObjRegister.GetAttendanceReport(strEmpIdList, Convert.ToDateTime(strFromDate).ToString(), Convert.ToDateTime(strToDate).ToString(), "11");


        TableRow tr = new TableRow();



        TableCell tcSNO = new TableCell();
        tcSNO.Text = "SNO";
        tcSNO.Style["border"] = "dotted";
        tcSNO.Style["border-width"] = "1px";
        tcSNO.Style["Text-align"] = "Center";
        tcSNO.VerticalAlign = VerticalAlign.Middle;
        tcSNO.Font.Bold = true;
        tr.Cells.Add(tcSNO);
        TableCell tcId = new TableCell();
        tcId.Wrap = false;
        tcId.Text = "Employee#";
        tcId.Style["border"] = "dotted";
        tcId.Style["border-width"] = "1px";
        tcId.Font.Bold = true;
        tr.Cells.Add(tcId);
        TableCell tcName = new TableCell();
        tcName.Wrap = false;
        tcName.Text = "Name";
        tcName.Style["border"] = "dotted";
        tcName.Style["border-width"] = "1px";
        tcName.Font.Bold = true;
        tr.Cells.Add(tcName);

        //for device group
        TableCell tcdeviceGroup = new TableCell();
        tcdeviceGroup.Wrap = false;
        tcdeviceGroup.Text = "Device Group";
        tcdeviceGroup.Style["border"] = "dotted";
        tcdeviceGroup.Style["border-width"] = "1px";
        tcdeviceGroup.Font.Bold = true;
        tr.Cells.Add(tcdeviceGroup);

        TableCell tcLoc = new TableCell();
        tcLoc.Wrap = false;
        tcLoc.Text = "Location";
        tcLoc.Style["border"] = "dotted";
        tcLoc.Style["border-width"] = "1px";
        tcLoc.Font.Bold = true;
        tr.Cells.Add(tcLoc);
        TableCell tcDept = new TableCell();
        tcDept.Wrap = false;
        tcDept.Text = "Department";
        tcDept.Style["border"] = "dotted";
        tcDept.Style["border-width"] = "1px";
        tcDept.Font.Bold = true;
        tr.Cells.Add(tcDept);
        TableCell tcDesg = new TableCell();
        tcDesg.Wrap = false;
        tcDesg.Text = "Designation";
        tcDesg.Style["border"] = "dotted";
        tcDesg.Style["border-width"] = "1px";
        tcDesg.Font.Bold = true;
        tr.Cells.Add(tcDesg);
        TableCell tcShiftcount = new TableCell();
        tcShiftcount.Wrap = false;
        tcShiftcount.Text = "Number of shifts";
        tcShiftcount.Style["border"] = "dotted";
        tcShiftcount.Style["border-width"] = "1px";
        tcShiftcount.Font.Bold = true;
        tr.Cells.Add(tcShiftcount);


        int count = 1;


        DateTime dtFromdate = Convert.ToDateTime(strFromDate);
        DateTime dtTodate = Convert.ToDateTime(strToDate);




        while (dtFromdate <= dtTodate)
        {
            TableCell tcDay = new TableCell();
            tcDay.Text = "&nbsp;" + dtFromdate.ToString("dd-MMM-yyyy") + "&nbsp;";
            //tcDay.Text = dtFromdate.ToString("dd-MMM-yyyy");
            tcDay.Wrap = false;
            tcDay.Style["border"] = "dotted";
            tcDay.Style["border-width"] = "1px";
            tcDay.Style["Text-align"] = "Center";
            tcDay.Font.Bold = true;
            tr.Cells.Add(tcDay);
            dtFromdate = dtFromdate.AddDays(1);
        }

        Table1.Rows.Add(tr);
        DataTable dtEmpList = dtFilter.DefaultView.ToTable(true, "Emp_Id");
        int empCounter = 0;
        if (dtEmpList.Rows.Count > 0)
        {

            while (empCounter < dtEmpList.Rows.Count)
            {

                DataTable dtAttbyEmpId = new DataView(dtFilter, "Emp_Id = '" + dtEmpList.Rows[empCounter][0].ToString() + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

                DateTime dtFromTemp = Convert.ToDateTime(strFromDate);
                DateTime dtToTemp = Convert.ToDateTime(strToDate);


                int maxshift = 0;
                while (dtFromTemp < dtToTemp)
                {


                    DataTable dtTempDateRecordEmp = new DataView(dtAttbyEmpId, "Att_Date = '" + dtFromTemp.ToString("dd-MMM-yyyy") + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

                    if (maxshift < dtTempDateRecordEmp.Rows.Count)
                    {
                        maxshift = dtTempDateRecordEmp.Rows.Count;
                    }
                    dtFromTemp = dtFromTemp.AddDays(1);
                }

                TableRow[] trEmp = new TableRow[maxshift];
                int[] absent = new int[maxshift];
                int[] present = new int[maxshift];
                int[] early = new int[maxshift];
                int[] late = new int[maxshift];
                int[] total = new int[maxshift];


                try
                {
                    present[0] = 0;
                    early[0] = 0;
                    late[0] = 0;
                    total[0] = 0;
                }
                catch
                {

                }
                int tempcounter = 0;

                while (tempcounter < maxshift)
                {
                    trEmp[tempcounter] = new TableRow();
                    tempcounter++;
                }

                TableCell tcSNOEmp = new TableCell();
                TableCell tcNameEmp1 = new TableCell();
                TableCell tcNameEmp = new TableCell();
                TableCell tcdeviceGroupValue = new TableCell();
                TableCell tcLOcValue = new TableCell();
                TableCell tcdeptValue = new TableCell();
                TableCell tcdesgValue = new TableCell();
                TableCell tcshiftvalue = new TableCell();
                tcSNOEmp.Text = (empCounter + 1).ToString();
                //tcSNOEmp.VerticalAlign = VerticalAlign.Top;
                if (maxshift > 0)
                {
                    tcSNOEmp.Style["border"] = "dotted";
                    tcSNOEmp.Style["border-width"] = "1px";
                    tcSNOEmp.Style["Text-align"] = "Center";
                    tcSNOEmp.VerticalAlign = VerticalAlign.NotSet;
                    trEmp[0].Cells.Add(tcSNOEmp);


                    tcNameEmp1.Style["border"] = "dotted";
                    tcNameEmp1.Style["border-width"] = "1px";
                    tcNameEmp1.Style["Text-align"] = "Center";
                    tcNameEmp1.Style["VerticalAlign-align"] = "Middle";
                    tcNameEmp1.Text = dtAttbyEmpId.Rows[1]["Emp_Code"].ToString();
                    tcNameEmp1.VerticalAlign = VerticalAlign.Middle;
                    trEmp[0].Cells.Add(tcNameEmp1);




                    tcNameEmp.Style["border"] = "dotted";
                    tcNameEmp.Style["border-width"] = "1px";
                    tcNameEmp.Style["Text-align"] = "Center";
                    tcNameEmp.Text = dtAttbyEmpId.Rows[0]["Emp_Name"].ToString();
                    tcNameEmp.Wrap = false;
                    tcNameEmp.VerticalAlign = VerticalAlign.Middle;
                    trEmp[0].Cells.Add(tcNameEmp);

                    //for device group
                    tcdeviceGroupValue.Style["border"] = "dotted";
                    tcdeviceGroupValue.Style["border-width"] = "1px";
                    tcdeviceGroupValue.Style["Text-align"] = "Center";
                    tcdeviceGroupValue.Text = dtAttbyEmpId.Rows[0]["Group_Name"].ToString();
                    tcdeviceGroupValue.Wrap = false;
                    tcdeviceGroupValue.VerticalAlign = VerticalAlign.Middle;
                    trEmp[0].Cells.Add(tcdeviceGroupValue);

                    //for location


                    tcLOcValue.Style["border"] = "dotted";
                    tcLOcValue.Style["border-width"] = "1px";
                    tcLOcValue.Style["Text-align"] = "Center";
                    tcLOcValue.Text = dtAttbyEmpId.Rows[0]["Location_Name"].ToString();
                    tcLOcValue.Wrap = false;
                    tcLOcValue.VerticalAlign = VerticalAlign.Middle;
                    trEmp[0].Cells.Add(tcLOcValue);
                    //for department

                    tcdeptValue.Style["border"] = "dotted";
                    tcdeptValue.Style["border-width"] = "1px";
                    tcdeptValue.Style["Text-align"] = "Center";
                    tcdeptValue.Text = dtAttbyEmpId.Rows[0]["dep_name"].ToString();
                    tcdeptValue.Wrap = false;
                    tcdeptValue.VerticalAlign = VerticalAlign.Middle;
                    trEmp[0].Cells.Add(tcdeptValue);
                    //for designation
                    tcdesgValue.Style["border"] = "dotted";
                    tcdesgValue.Style["border-width"] = "1px";

                    tcdesgValue.Text = dtAttbyEmpId.Rows[0]["Designation"].ToString();
                    tcdesgValue.VerticalAlign = VerticalAlign.Middle;
                    tcdesgValue.Style["Text-align"] = "Center";
                    tcdesgValue.Wrap = false;
                    trEmp[0].Cells.Add(tcdesgValue);


                    tcshiftvalue.Style["border"] = "dotted";
                    tcshiftvalue.Style["border-width"] = "1px";
                    tcshiftvalue.Style["Text-align"] = "Center";
                    if (maxshift > 0)
                    {
                        tcshiftvalue.Text = maxshift == 1 ? "One Shift" : "Split Shift";
                    }
                    else
                    {
                        tcshiftvalue.Text = "";
                    }

                    tcshiftvalue.Wrap = false;
                    tcshiftvalue.VerticalAlign = VerticalAlign.Middle;
                    trEmp[0].Cells.Add(tcshiftvalue);

                }
                int presentcount = 0;

                dtFromdate = Convert.ToDateTime(strFromDate);
                dtTodate = Convert.ToDateTime(strToDate);
                string strlogdetail = string.Empty;
                int Logcount = 0;
                string strMidLine = string.Empty;
                string stBottomLine = string.Empty;
                while (dtFromdate <= dtTodate)
                {

                    int shiftCounter = 0;
                    DataTable dtTempDateRecordEmp = new DataView(dtAttbyEmpId, "Att_Date = '" + dtFromdate.ToString("dd-MMM-yyyy") + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

                    while (shiftCounter < maxshift)
                    {
                        TableCell tcDay = new TableCell();


                        tcDay.Style["border"] = "dotted";
                        tcDay.Style["border-width"] = "1px";
                        tcDay.VerticalAlign = VerticalAlign.Middle;
                        Literal lit = new Literal();

                        lit.Text = "";


                        tcDay.Wrap = true;
                        tcDay.Style["Text-align"] = "Center";
                        //tcDay.Style["VerticalAlign"] = "Top";
                        string attEmp = string.Empty;

                        if (shiftCounter < dtTempDateRecordEmp.Rows.Count)
                        {
                            DateTime OndutyTime = new DateTime();
                            DateTime OffdutyTime = new DateTime();


                            if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                            {
                                OndutyTime = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day, Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OnDuty_Time"].ToString()).Hour, Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OnDuty_Time"].ToString()).Minute, Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OnDuty_Time"].ToString()).Second);

                                if (Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OnDuty_Time"].ToString()) <= Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OffDuty_Time"].ToString()))
                                {
                                    OffdutyTime = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day, Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OffDuty_Time"].ToString()).Hour, Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OffDuty_Time"].ToString()).Minute, Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OffDuty_Time"].ToString()).Second);
                                }
                                else
                                {
                                    OffdutyTime = new DateTime(dtFromdate.AddDays(1).Year, dtFromdate.AddDays(1).Month, dtFromdate.AddDays(1).Day, Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OffDuty_Time"].ToString()).Hour, Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OffDuty_Time"].ToString()).Minute, Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["OffDuty_Time"].ToString()).Second);
                                }
                            }





                            strlogdetail = dtTempDateRecordEmp.Rows[shiftCounter]["log_detail"].ToString();

                            if (dtTempDateRecordEmp.Rows[shiftCounter]["UserStatus"].ToString() == "0")
                            {

                                if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                {
                                    lit.Text = "NR<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + "Shift " + dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() + "-" + dtTempDateRecordEmp.Rows[shiftCounter]["OFF_Time"].ToString() + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />I/O:--/--";
                                }
                                else
                                {
                                    lit.Text = "NR<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + "Shift: None<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />I/O:--/--";
                                }


                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(NRcol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }
                            else if (strlogdetail.Trim() == "")
                            {
                                //if 0 log found then 
                                if (Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Leave"].ToString()))
                                {
                                    int workMinute = 0;
                                    if (dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString().Trim() != "")
                                    {

                                        if (strlogdetail.Trim() == "")
                                        {
                                            attEmp = attEmp + dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString();
                                        }
                                        else if (strlogdetail.Split(',').Length == 2)
                                        {
                                            attEmp = attEmp + dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString() + "(" + strlogdetail.Substring(0, strlogdetail.Length - 1) + ")";
                                        }
                                        if (strlogdetail.Split(',').Length > 2)
                                        {
                                            workMinute = GetMinuteDiff(strlogdetail.Split(',')[strlogdetail.Split(',').Length - 2], strlogdetail.Split(',')[0]);

                                            if (workMinute > 0)
                                            {
                                                string strTime = GetHours(workMinute.ToString());
                                                attEmp = attEmp + dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString() + "(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                            }
                                            else
                                            {
                                                attEmp = attEmp + dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString() + "(" + strlogdetail.Substring(0, strlogdetail.Length - 1) + ")";
                                            }
                                        }

                                    }
                                    else
                                    {
                                        attEmp = attEmp + "Leave";
                                    }


                                    if (workMinute > 960)
                                    {
                                        tcDay = GetTableCellcolor("ff0000", OTcol);
                                    }
                                    else
                                    {
                                        tcDay = GetTableCellcolor("", Leavecol);

                                    }
                                }
                                else if ((Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Holiday"].ToString())))
                                {

                                    if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Holiday_Min"].ToString()) > 0)
                                    {
                                        string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Holiday_Min"].ToString());
                                        attEmp = attEmp + Holiday + "(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                    }
                                    else
                                    {
                                        attEmp = attEmp + Holiday;
                                    }

                                    if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Holiday_Min"].ToString()) > 960)
                                    {
                                        tcDay = GetTableCellcolor("ff0000", OTcol);
                                    }
                                    else
                                    {
                                        tcDay = GetTableCellcolor("", Holidaycol);

                                    }
                                }
                                else if ((Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Week_Off"].ToString())))
                                {

                                    if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Week_Off_Min"].ToString()) > 0)
                                    {
                                        string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Week_Off_Min"].ToString());
                                        attEmp = attEmp + "WO(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                    }
                                    else
                                    {
                                        attEmp = attEmp + WeekOff;
                                    }

                                    if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Week_Off_Min"].ToString()) > 960)
                                    {
                                        tcDay = GetTableCellcolor("ff0000", OTcol);
                                    }
                                    else
                                    {
                                        tcDay = GetTableCellcolor("", WeekOffcol);
                                    }
                                }
                                else if (Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["att_date"].ToString()).Date == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                {

                                    if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                    {
                                        if (OndutyTime.TimeOfDay > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).TimeOfDay)
                                        {
                                            tcDay.Text = "";
                                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                        }
                                        else
                                        {
                                            if (OffdutyTime.TimeOfDay > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).TimeOfDay)
                                            {
                                                if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                                {
                                                    attEmp = attEmp + MIssedIn;
                                                    tcDay = GetTableCellcolor("", MIssedIncol);
                                                }
                                                else
                                                {
                                                    attEmp = attEmp + "NS";
                                                    tcDay = GetTableCellcolor("ff0000", NScol);
                                                }
                                            }
                                            else
                                            {

                                                attEmp = attEmp + Absent;

                                                if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                                {
                                                    tcDay = GetTableCellcolor("", Absentcol);
                                                }
                                                else
                                                {
                                                    tcDay = GetTableCellcolor("ff0000", NScol);
                                                }
                                            }

                                            if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                            {

                                                strMidLine = "Shift " + dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() + "-" + dtTempDateRecordEmp.Rows[shiftCounter]["OFF_Time"].ToString();
                                                //attEmp = attEmp + Environment.NewLine + "" + dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() + "-" + dtTempDateRecordEmp.Rows[shiftCounter]["OFF_Time"].ToString();
                                            }
                                            else
                                            {
                                                strMidLine = "Shift: None";
                                                // attEmp = attEmp + Environment.NewLine + "Shift: None";
                                            }

                                            if (attEmp == "AB")
                                            {
                                                lit.Text = attEmp + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + strMidLine + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + "I/O:--/--";
                                            }
                                            else
                                            {
                                                lit.Text = attEmp + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + strMidLine + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />";
                                            }


                                        }
                                    }
                                    else
                                    {
                                        attEmp = attEmp + "";

                                        try
                                        {
                                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                        }
                                        catch
                                        {
                                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    attEmp = attEmp + Absent;

                                    if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                    {
                                        strMidLine = "Shift " + dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() + "-" + dtTempDateRecordEmp.Rows[shiftCounter]["OFF_Time"].ToString();
                                        tcDay = GetTableCellcolor("", Absentcol);
                                    }
                                    else
                                    {
                                        strMidLine = "Shift: None";
                                        tcDay = GetTableCellcolor("ff0000", NScol);
                                    }
                                    lit.Text = attEmp + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + strMidLine + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />I/O:--/--";


                                }
                            }
                            else if (Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Leave"].ToString()))
                            {
                                int workMinute = 0;
                                if (dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString().Trim() != "")
                                {
                                    if (strlogdetail.Trim() == "")
                                    {
                                        attEmp = attEmp + dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString();
                                    }
                                    else if (strlogdetail.Split(',').Length == 2)
                                    {
                                        attEmp = attEmp + dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString() + "(" + strlogdetail.Substring(0, strlogdetail.Length - 1) + ")";
                                    }
                                    if (strlogdetail.Split(',').Length > 2)
                                    {
                                        workMinute = GetMinuteDiff(strlogdetail.Split(',')[strlogdetail.Split(',').Length - 2], strlogdetail.Split(',')[0]);

                                        if (workMinute > 0)
                                        {
                                            string strTime = GetHours(workMinute.ToString());
                                            attEmp = attEmp + dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString() + "(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                        }
                                        else
                                        {
                                            attEmp = attEmp + dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString() + "(" + strlogdetail.Substring(0, strlogdetail.Length - 1) + ")";
                                        }
                                    }


                                }
                                else
                                {
                                    attEmp = attEmp + "Leave";
                                }

                                if (workMinute > 960)
                                {
                                    tcDay = GetTableCellcolor("ff0000", OTcol);
                                }
                                else
                                {
                                    tcDay = GetTableCellcolor("", Leavecol);

                                }
                            }
                            else if ((Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Holiday"].ToString())))
                            {

                                if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Holiday_Min"].ToString()) > 0)
                                {
                                    string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Holiday_Min"].ToString());
                                    attEmp = attEmp + Holiday + "(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                }
                                else
                                {
                                    attEmp = attEmp + Holiday;
                                }

                                if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Holiday_Min"].ToString()) > 960)
                                {
                                    tcDay = GetTableCellcolor("ff0000", OTcol);
                                }
                                else
                                {
                                    tcDay = GetTableCellcolor("", Holidaycol);
                                }
                            }
                            else if ((Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Week_Off"].ToString())))
                            {
                                if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Week_Off_Min"].ToString()) > 0)
                                {
                                    string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Week_Off_Min"].ToString());
                                    attEmp = attEmp + "WO(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                }
                                else
                                {
                                    attEmp = attEmp + WeekOff;
                                }

                                if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Week_Off_Min"].ToString()) > 960)
                                {
                                    tcDay = GetTableCellcolor("ff0000", OTcol);
                                }
                                else
                                {
                                    tcDay = GetTableCellcolor("", WeekOffcol);

                                }
                            }
                            //updated 08-02-2019
                            else if (dtTempDateRecordEmp.Rows[shiftCounter]["log_detail"].ToString().Split(',').Length == 1)
                            //else if (dtTempDateRecordEmp.Rows[shiftCounter]["log_detail"].ToString().Split(',').Length == 1)
                            {
                                //if single log found then 

                                if (Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["att_date"].ToString()).Date == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                {

                                    if (OffdutyTime > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()))
                                    {
                                        //if (OndutyTime.TimeOfDay < Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["In_Time1"].ToString()).TimeOfDay)
                                        //{

                                        if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["LateMin"].ToString()) > 0)
                                        {


                                            attEmp = attEmp + "<B>LI</B>";

                                            try
                                            {
                                                tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(LateIncol).ToString());
                                            }
                                            catch
                                            {
                                                tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                            }
                                        }
                                        else
                                        {
                                            attEmp = attEmp + "";
                                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());


                                        }
                                    }
                                    else if (dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() == "00:00" && dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() != "00:00")
                                    {
                                        // attEmp = attEmp + MIssedIn;

                                        if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                        {
                                            attEmp = attEmp + MIssedIn;
                                            tcDay = GetTableCellcolor("", MIssedIncol);
                                        }
                                        else
                                        {
                                            attEmp = attEmp + "NS";
                                            tcDay = GetTableCellcolor("ff0000", NScol);
                                        }
                                    }
                                    else if (dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() != "00:00" && dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() == "00:00")
                                    {


                                        if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                        {
                                            attEmp = attEmp + MIssedout;
                                            tcDay = GetTableCellcolor("", MIssedoutcol);
                                        }
                                        else
                                        {
                                            attEmp = attEmp + "NS";
                                            tcDay = GetTableCellcolor("ff0000", NScol);
                                        }

                                    }
                                }
                                else if (dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() == "00:00" && dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() != "00:00")
                                {
                                    // attEmp = attEmp + MIssedIn;


                                    if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                    {
                                        attEmp = attEmp + MIssedIn;
                                        tcDay = GetTableCellcolor("", MIssedIncol);
                                    }
                                    else
                                    {
                                        attEmp = attEmp + "NS";
                                        tcDay = GetTableCellcolor("ff0000", NScol);
                                    }

                                }
                                else if (dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() != "00:00" && dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() == "00:00")
                                {

                                    if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                    {
                                        attEmp = attEmp + MIssedout;
                                        tcDay = GetTableCellcolor("", MIssedoutcol);
                                    }
                                    else
                                    {
                                        attEmp = attEmp + "NS";
                                        tcDay = GetTableCellcolor("ff0000", NScol);
                                    }
                                }
                                else
                                {

                                    attEmp = attEmp + Absent;

                                    if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                    {
                                        strMidLine = "Shift " + dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() + "-" + dtTempDateRecordEmp.Rows[shiftCounter]["OFF_Time"].ToString();
                                        tcDay = GetTableCellcolor("", Absentcol);
                                    }
                                    else
                                    {
                                        strMidLine = "Shift: None";
                                        tcDay = GetTableCellcolor("ff0000", NScol);
                                    }

                                    lit.Text = attEmp + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + strMidLine + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />I/O:--/--";

                                }






                            }
                            //here we will check missed in and missed out conditions

                            else if (Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Absent"].ToString()))
                            {

                                attEmp = attEmp + Absent;



                                if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                {

                                    strMidLine = "Shift " + dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() + "-" + dtTempDateRecordEmp.Rows[shiftCounter]["OFF_Time"].ToString();
                                    tcDay = GetTableCellcolor("", Absentcol);
                                }
                                else
                                {
                                    strMidLine = "Shift: None";
                                    tcDay = GetTableCellcolor("ff0000", NScol);
                                }


                                lit.Text = attEmp + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + strMidLine + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />I/O:--/--";

                            }
                            else if (dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() == "00:00" && dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() != "00:00")
                            {
                                // attEmp = attEmp + MIssedIn;
                                int workMinute = 0;


                                if (strlogdetail.Split(',').Length > 2)
                                {
                                    workMinute = GetMinuteDiff(strlogdetail.Split(',')[strlogdetail.Split(',').Length - 2], strlogdetail.Split(',')[0]);
                                }

                                if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                {

                                    if (workMinute > 0)
                                    {
                                        string strTime = GetHours(workMinute.ToString());
                                        attEmp = attEmp + MIssedIn + "(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                    }
                                    else
                                    {
                                        attEmp = attEmp + MIssedIn;
                                    }


                                    if (workMinute > 960)
                                    {
                                        tcDay = GetTableCellcolor("ff0000", OTcol);
                                    }
                                    else
                                    {
                                        tcDay = GetTableCellcolor("", MIssedIncol);
                                    }
                                }
                                else
                                {
                                    attEmp = attEmp + "NS";
                                    tcDay = GetTableCellcolor("ff0000", NScol);
                                }



                            }
                            else if (dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() != "00:00" && dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() == "00:00")
                            {
                                int workMinute = 0;
                                if (strlogdetail.Split(',').Length > 2)
                                {
                                    workMinute = GetMinuteDiff(strlogdetail.Split(',')[strlogdetail.Split(',').Length - 2], strlogdetail.Split(',')[0]);
                                }

                                if (Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["att_date"].ToString()).Date == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                {

                                    if (OffdutyTime > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()))
                                    {
                                        //if (OndutyTime.TimeOfDay < Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["In_Time1"].ToString()).TimeOfDay)
                                        //{

                                        if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["LateMin"].ToString()) > 0)
                                        {
                                            attEmp = attEmp + "<B>LI</B>";

                                            try
                                            {
                                                tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(LateIncol).ToString());
                                            }
                                            catch
                                            {
                                                tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                            }
                                        }
                                        else
                                        {
                                            if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                            {

                                                if (workMinute > 0)
                                                {
                                                    string strTime = GetHours(workMinute.ToString());
                                                    attEmp = attEmp + "" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M";
                                                }
                                                else
                                                {
                                                    attEmp = attEmp + "";
                                                }
                                                tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                            }
                                            else
                                            {
                                                attEmp = attEmp + "NS";
                                                tcDay = GetTableCellcolor("ff0000", NScol);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                        {
                                            if (workMinute > 0)
                                            {
                                                string strTime = GetHours(workMinute.ToString());
                                                attEmp = attEmp + MIssedout + "(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                            }
                                            else
                                            {
                                                attEmp = attEmp + MIssedout;
                                            }
                                            tcDay = GetTableCellcolor("", MIssedoutcol);
                                        }
                                        else
                                        {
                                            attEmp = attEmp + "NS";
                                            tcDay = GetTableCellcolor("ff0000", NScol);
                                        }
                                    }
                                }
                                else
                                {
                                    if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                    {

                                        if (workMinute > 0)
                                        {
                                            string strTime = GetHours(workMinute.ToString());
                                            attEmp = attEmp + MIssedout + "(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                        }
                                        else
                                        {
                                            attEmp = attEmp + MIssedout;
                                        }

                                        tcDay = GetTableCellcolor("", MIssedoutcol);
                                    }
                                    else
                                    {
                                        attEmp = attEmp + "NS";
                                        tcDay = GetTableCellcolor("ff0000", NScol);
                                    }
                                }

                                if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                {
                                    if (workMinute > 960)
                                    {
                                        tcDay = GetTableCellcolor("ff0000", OTcol);

                                    }
                                }

                            }
                            else if ((Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["LateMin"].ToString())) > 0 && Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["LateMin"].ToString()) >= Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["EarlyMin"].ToString()))
                            {
                                int workMinute = 0;

                                if (strlogdetail.Split(',').Length > 2)
                                {
                                    workMinute = GetMinuteDiff(strlogdetail.Split(',')[strlogdetail.Split(',').Length - 2], strlogdetail.Split(',')[0]);
                                }


                                if (Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["Att_date"].ToString()).Date == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                {
                                    if (OffdutyTime > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()))
                                    {

                                        if (workMinute > 0)
                                        {
                                            string strTime = GetHours(workMinute.ToString());
                                            attEmp = attEmp + "" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M";
                                        }
                                        else
                                        {
                                            attEmp = attEmp + "";
                                        }

                                        //tcDay.Text = "";
                                        tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());

                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString()) > 0)
                                        {
                                            string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString());
                                            attEmp = attEmp + "LI(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                        }
                                        else
                                        {
                                            attEmp = attEmp + "";
                                        }

                                        //attEmp = attEmp + "LI(" + dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() + ")";
                                        //attEmp = attEmp + LateIn;

                                        tcDay = GetTableCellcolor("", LateIncol);

                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString()) > 0)
                                    {
                                        string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString());
                                        attEmp = attEmp + "LI(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                    }
                                    else
                                    {
                                        attEmp = attEmp + "";
                                    }
                                    //attEmp = attEmp + "LI(" + dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() + ")";

                                    //attEmp = attEmp + LateIn;

                                    tcDay = GetTableCellcolor("", LateIncol);

                                }
                                presentcount++;

                                if (workMinute > 960)
                                {
                                    tcDay = GetTableCellcolor("ff0000", OTcol);

                                }


                            }
                            else if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["EarlyMin"].ToString()) > 0)
                            {
                                int workMinute = 0;
                                if (strlogdetail.Split(',').Length > 2)
                                {
                                    workMinute = GetMinuteDiff(strlogdetail.Split(',')[strlogdetail.Split(',').Length - 2], strlogdetail.Split(',')[0]);
                                }
                                if (Convert.ToDateTime(dtTempDateRecordEmp.Rows[shiftCounter]["Att_date"].ToString()).Date == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                                {
                                    if (OffdutyTime > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()))
                                    {

                                        if (workMinute > 0)
                                        {
                                            string strTime = GetHours(workMinute.ToString());
                                            attEmp = attEmp + "" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M";
                                        }
                                        else
                                        {
                                            attEmp = attEmp + "";
                                        }
                                        tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());

                                    }
                                    else
                                    {

                                        // attEmp = attEmp + "EO(" + dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() + ")";

                                        if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString()) > 0)
                                        {

                                            string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString());
                                            attEmp = attEmp + "EO(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                        }
                                        else
                                        {
                                            attEmp = attEmp + "";
                                        }

                                        //attEmp = attEmp + Earlyout;
                                        try
                                        {
                                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Earlyoutcol).ToString());
                                        }
                                        catch
                                        {
                                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                        }
                                    }
                                }
                                else
                                {

                                    //attEmp = attEmp + "EO(" + dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() + ")";
                                    if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString()) > 0)
                                    {

                                        string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString());
                                        attEmp = attEmp + "EO(" + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M)";
                                    }
                                    else
                                    {
                                        attEmp = attEmp + "";
                                    }

                                    //attEmp = attEmp + Earlyout;
                                    try
                                    {
                                        tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Earlyoutcol).ToString());
                                    }
                                    catch
                                    {
                                        tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                    }
                                }
                                presentcount++;
                                if (workMinute > 960)
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffd800").ToString());
                                    tcDay.ForeColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ff0000").ToString());
                                }
                            }
                            else
                            {
                                if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                {

                                    if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString()) > 0)
                                    {
                                        string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString());
                                        attEmp = attEmp + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M";
                                    }
                                    else
                                    {
                                        attEmp = attEmp + "";
                                    }
                                    if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Effectivework_Min"].ToString()) > 960)
                                    {
                                        tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffd800").ToString());
                                        tcDay.ForeColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ff0000").ToString());
                                    }
                                    else
                                    {

                                        try
                                        {
                                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Normalcol).ToString());
                                        }
                                        catch
                                        {
                                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    attEmp = attEmp + "NS";
                                    tcDay = GetTableCellcolor("ff0000", NScol);
                                }


                                presentcount++;
                            }
                            absent[shiftCounter] = absent[shiftCounter] + 1;




                            string strSeperator1 = string.Empty;
                            string strSeperator2 = string.Empty;
                            if (lit.Text.Trim() == "")
                            {

                                if (dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() != "00:00" || dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() != "00:00")
                                {

                                    if (dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() != "00:00")
                                    {

                                        strMidLine = "Shift " + dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() + "-" + dtTempDateRecordEmp.Rows[shiftCounter]["OFF_Time"].ToString();
                                        //attEmp = attEmp + Environment.NewLine + "" + dtTempDateRecordEmp.Rows[shiftCounter]["On_Time"].ToString() + "-" + dtTempDateRecordEmp.Rows[shiftCounter]["OFF_Time"].ToString();
                                    }
                                    else
                                    {
                                        strMidLine = "Shift: None";
                                        // attEmp = attEmp + Environment.NewLine + "Shift: None";
                                    }



                                    stBottomLine = "I/O: " + dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() + " / " + dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString();



                                    //=attEmp = attEmp + Environment.NewLine + dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() + "-" + dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString();


                                    //attEmp = String.Format("{0}<br style="mso-data-placement:same-cell;" />" + Environment.NewLine+ "{1}<br style="mso-data-placement:same-cell;" />" + Environment.NewLine + "{2}", attEmp, strMidLine, stBottomLine);


                                    //attEmp = SplitLineToMultiline(attEmp, 3);



                                    if (attEmp.Contains("("))
                                    {
                                        strSeperator1 = attEmp.Split('(')[0];
                                        strSeperator2 = attEmp.Split('(')[1];

                                        lit.Text = "<B>" + strSeperator2.Replace(")", "") + "</B> (<B>" + strSeperator1 + "</B>)<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + strMidLine + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + stBottomLine;
                                    }
                                    else
                                    {
                                        if (attEmp == "")
                                        {
                                            lit.Text = strMidLine;
                                        }
                                        else
                                        {
                                            lit.Text = "<B>" + attEmp + "</B><br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + strMidLine + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />" + stBottomLine;
                                        }
                                    }


                                }
                                else
                                {
                                    if (attEmp.Contains("("))
                                    {
                                        strSeperator1 = attEmp.Split('(')[0];
                                        strSeperator2 = attEmp.Split('(')[1];

                                        lit.Text = "<B>" + strSeperator1 + "</B> (<B>" + strSeperator2.Replace(")", "") + "</B>)";
                                    }
                                    else
                                    {
                                        lit.Text = attEmp;
                                    }
                                }
                            }

                            //tcDay.Text = attEmp;
                        }
                        else
                        {
                            attEmp = "-";
                            lit.Text = attEmp;
                            //tcDay.Text = "<B>" + attEmp + "</B>";
                            //tcDay.VerticalAlign = VerticalAlign.Top;
                        }


                        tcDay.Controls.Add(lit);
                        //tcDay.Text = lit.Text;
                        //for future date it will not show as absent

                        if (dtFromdate.Date > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                        {
                            tcDay.Text = "";
                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                        }

                        trEmp[shiftCounter].Cells.Add(tcDay);
                        //trEmp[shiftCounter].Cells.Add(tcDay);

                        //

                        shiftCounter = shiftCounter + 1;
                        // Modified By Nitin jain On 23/10/2014
                        if (shiftCounter == maxshift)
                        {
                            Counter = 0;
                        }

                    }


                    dtFromdate = dtFromdate.AddDays(1);

                }



                for (int maxcounter = 1; maxcounter <= maxshift; maxcounter++)
                {
                    if (maxcounter < maxshift)
                    {
                        TableCell BlankCell = new TableCell();
                        BlankCell.Text = tcSNOEmp.Text;

                        TableCell EmPCode = new TableCell();
                        EmPCode.Text = tcNameEmp1.Text;
                        TableCell EmPName = new TableCell();
                        EmPName.Text = tcNameEmp.Text;
                        TableCell GroupName = new TableCell();
                        GroupName.Text = tcdeviceGroupValue.Text;
                        TableCell LocName = new TableCell();
                        LocName.Text = tcLOcValue.Text;
                        TableCell DeptName = new TableCell();
                        DeptName.Text = tcdeptValue.Text;
                        TableCell DesgName = new TableCell();
                        DesgName.Text = tcdesgValue.Text;
                        TableCell shiftCount = new TableCell();
                        shiftCount.Text = tcshiftvalue.Text;

                        BlankCell.Style["border"] = "dotted";
                        BlankCell.Style["border-width"] = "1px";
                        BlankCell.VerticalAlign = VerticalAlign.Middle;
                        BlankCell.Style["Text-align"] = "Center";


                        EmPCode.Style["border"] = "dotted";
                        EmPCode.Style["border-width"] = "1px";
                        EmPCode.VerticalAlign = VerticalAlign.Middle;
                        EmPCode.Style["Text-align"] = "Center";

                        EmPName.Style["border"] = "dotted";
                        EmPName.Style["border-width"] = "1px";
                        EmPName.VerticalAlign = VerticalAlign.Middle;
                        EmPName.Style["Text-align"] = "Center";



                        GroupName.Style["border"] = "dotted";
                        GroupName.Style["border-width"] = "1px";
                        GroupName.VerticalAlign = VerticalAlign.Middle;
                        GroupName.Style["Text-align"] = "Center";


                        LocName.Style["border"] = "dotted";
                        LocName.Style["border-width"] = "1px";
                        LocName.VerticalAlign = VerticalAlign.Middle;
                        LocName.Style["Text-align"] = "Center";


                        DeptName.Style["border"] = "dotted";
                        DeptName.Style["border-width"] = "1px";
                        DeptName.VerticalAlign = VerticalAlign.Middle;
                        DeptName.Style["Text-align"] = "Center";


                        DesgName.Style["border"] = "dotted";
                        DesgName.Style["border-width"] = "1px";
                        DesgName.VerticalAlign = VerticalAlign.Middle;
                        DesgName.Style["Text-align"] = "Center";


                        shiftCount.Style["border"] = "dotted";
                        shiftCount.Style["border-width"] = "1px";
                        shiftCount.VerticalAlign = VerticalAlign.Middle;
                        shiftCount.Style["Text-align"] = "Center";

                        trEmp[maxcounter].Cells.AddAt(0, BlankCell);
                        trEmp[maxcounter].Cells.AddAt(1, EmPCode);
                        trEmp[maxcounter].Cells.AddAt(2, EmPName);
                        trEmp[maxcounter].Cells.AddAt(3, GroupName);
                        trEmp[maxcounter].Cells.AddAt(4, LocName);
                        trEmp[maxcounter].Cells.AddAt(5, DeptName);
                        trEmp[maxcounter].Cells.AddAt(6, DesgName);
                        trEmp[maxcounter].Cells.AddAt(7, shiftCount);
                    }

                    Table1.Rows.Add(trEmp[maxcounter - 1]);
                }




                empCounter++;
            }

        }
        //lblmonthname.Text = "Month " + ": " + ddlMonth.SelectedItem.Text;



        return Table1;
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


    public int hextoint(string hexValue)
    {

        if (hexValue == "")
        {
            hexValue = "ffffff";
        }

        return int.Parse(hexValue, NumberStyles.AllowHexSpecifier);
    }

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    protected void lnkback_Click(object sender, EventArgs e)
    {

        Session["SelectedEmpId"] = null;
        Response.Redirect("../attendance_report/attendancereport.aspx");
    }


    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename = Time Attendance Details Report " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("ddMMyyyy_HHmmss") + ".xls");
        Response.ContentEncoding = Encoding.UTF8;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        divexport.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }

    public Literal GetTableCellText(string Cellheader, string cellmiddle, string cellBottom)
    {
        Literal Ltr_col_Val = new Literal();

        Ltr_col_Val.Text = Cellheader + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />";
        if (cellmiddle != "")
        {
            Ltr_col_Val.Text += cellmiddle + "<br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' />";
        }
        if (cellBottom != "")
        {
            Ltr_col_Val.Text += cellBottom;
        }
        return Ltr_col_Val;
    }

    public TableCell GetTableCellcolor(string strForecolor, string strbackcolor)
    {
        TableCell Reportcell = new TableCell();

        Reportcell.Style["border"] = "dotted";
        Reportcell.Style["border-width"] = "1px";
        Reportcell.VerticalAlign = VerticalAlign.Middle;
        Reportcell.Wrap = true;
        Reportcell.Style["Text-align"] = "Center";
        Reportcell.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(strbackcolor).ToString());
        if (strForecolor != "")
        {
            Reportcell.ForeColor = System.Drawing.ColorTranslator.FromHtml(hextoint(strForecolor).ToString());
        }

        return Reportcell;
    }


    public int GetMinuteDiff(string greatertime, string lesstime)
    {
        int retval = 0;
        int actTimeHour = Convert.ToInt32(greatertime.Split(':')[0]);
        int ondutyhour = Convert.ToInt32(lesstime.Split(':')[0]);
        int actTimeMinute = Convert.ToInt32(greatertime.Split(':')[1]);
        int ondutyMinute = Convert.ToInt32(lesstime.Split(':')[1]);
        int totalActTimeMinute = actTimeHour * 60 + actTimeMinute;
        int totalOnDutyTimeMinute = ondutyhour * 60 + ondutyMinute;
        if (totalActTimeMinute - totalOnDutyTimeMinute < 0)
        {
            retval = 1440 + (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        else
        {
            retval = (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        return retval;
    }




}