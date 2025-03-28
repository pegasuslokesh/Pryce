using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Globalization;

public partial class Attendance_Report_TSC_Overtimereport : System.Web.UI.Page
{
    Att_AttendanceRegister ObjRegister = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjRegister = new Att_AttendanceRegister(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../Attendance_Report/TimesDeductionReport.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

        tResult = GetReport1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["SelectedEmpId"].ToString(), Convert.ToDateTime(Session["Report_FromDate"].ToString()).ToString(), Convert.ToDateTime(Session["Report_ToDate"].ToString()).ToString(), false, struserName);

        mDiv.Controls.Add(tResult);

    }

    public int hextoint(string hexValue)
    {

        if (hexValue == "")
        {
            hexValue = "ffffff";
        }

        return int.Parse(hexValue, NumberStyles.AllowHexSpecifier);
    }

    public Table GetReport1(string strCompanyId, string strBrandId, string strLocationId, string strEmpIdList, string strFromDate, string strToDate, bool isDownload, string struserId = "")
    {
        Table Table1 = new Table();

        Table1.Style["Text-align"] = "Left";
        Table1.Style["Text-align"] = "Left";

        // Table1.GridLines = GridLines.Both;
        Table1.Font.Size = 10;

        TableRow trTitleReportName = new TableRow();


        TableCell tcHeaderExplanationBlank = new TableCell();
        tcHeaderExplanationBlank.Text = " ";
        tcHeaderExplanationBlank.Font.Bold = true;
        tcHeaderExplanationBlank.Font.Size = 12;
        tcHeaderExplanationBlank.ColumnSpan = 4;
        tcHeaderExplanationBlank.Style["Text-align"] = "Left";
        trTitleReportName.Cells.Add(tcHeaderExplanationBlank);



        TableCell tcHeaderExplanation = new TableCell();
        tcHeaderExplanation.Text = "Explanation";
        tcHeaderExplanation.Font.Bold = true;
        tcHeaderExplanation.Font.Size = 12;
        tcHeaderExplanation.ColumnSpan = 9;
        tcHeaderExplanation.Style["Text-align"] = "Left";
        trTitleReportName.Cells.Add(tcHeaderExplanation);

        Table1.Rows.Add(trTitleReportName);


        TableRow trHeaderReportName = new TableRow();


        TableCell tcHeaderReportName = new TableCell();
        tcHeaderReportName.Text = "Report Name";
        tcHeaderReportName.Font.Bold = true;

        tcHeaderReportName.Font.Size = 12;
        tcHeaderReportName.ColumnSpan = 2;
        tcHeaderReportName.Style["Text-align"] = "Left";
        trHeaderReportName.Cells.Add(tcHeaderReportName);

        TableCell tcHeaderReportvalue = new TableCell();
        tcHeaderReportvalue.Text = "TSC Overtime Report";
        tcHeaderReportvalue.ColumnSpan = 2;
        tcHeaderReportvalue.Font.Size = 12;
        tcHeaderReportvalue.Style["Text-align"] = "Left";
        trHeaderReportName.Cells.Add(tcHeaderReportvalue);
       


        TableCell tcHeaderscheduledEXP = new TableCell();
        tcHeaderscheduledEXP.Text = "Scheduled Hours :- Approved Scheduled Hours assigned to Employee within selected period.";
        tcHeaderscheduledEXP.ColumnSpan = 9;
        tcHeaderscheduledEXP.Font.Size = 9;
        tcHeaderscheduledEXP.Style["Text-align"] = "Left";
        trHeaderReportName.Cells.Add(tcHeaderscheduledEXP);
        Table1.Rows.Add(trHeaderReportName);


        //explanation





        //from date

        TableRow trHeaderFromDate = new TableRow();

        TableCell tcHeaderFromDate = new TableCell();
        tcHeaderFromDate.Text = "From Date";
        tcHeaderFromDate.ColumnSpan = 2;
        tcHeaderFromDate.Font.Size = 12;
        tcHeaderFromDate.Font.Bold = true;
        tcHeaderFromDate.Style["Text-align"] = "Left";
        trHeaderFromDate.Cells.Add(tcHeaderFromDate);

        var span = new LiteralControl("<span style='font-size:1px;'>.</span>" + Convert.ToDateTime(strFromDate).ToString("dd-MMM-yyyy"));
        Literal LitFromdate = new Literal();
        LitFromdate.Text = "";
        TableCell tcHeaderFromDateValue = new TableCell();
        tcHeaderFromDateValue.Text = " " + Convert.ToDateTime(strFromDate).ToString("dd-MMM-yyyy");
        tcHeaderFromDateValue.ColumnSpan = 2;
        tcHeaderFromDateValue.Font.Size = 12;
        tcHeaderFromDateValue.Style["Text-align"] = "Left";
        tcHeaderFromDateValue.Controls.Add(span);
        trHeaderFromDate.Cells.Add(tcHeaderFromDateValue);



        TableCell tcHeaderMaxHourEXP = new TableCell();
        tcHeaderMaxHourEXP.Text = "Maximum Hours :- (Days within Selected - WO - AB - AL - SL - PO) X 9 hours.";
        tcHeaderMaxHourEXP.ColumnSpan = 9;
        tcHeaderMaxHourEXP.Font.Size = 9;
        tcHeaderMaxHourEXP.Style["Text-align"] = "Left";
        trHeaderFromDate.Cells.Add(tcHeaderMaxHourEXP);




        Table1.Rows.Add(trHeaderFromDate);

        //to date

        TableRow trHeaderToDate = new TableRow();

        TableCell tcHeaderToDate = new TableCell();
        tcHeaderToDate.Text = "To Date";
        tcHeaderToDate.ColumnSpan = 2;
        tcHeaderToDate.Font.Bold = true;
        tcHeaderToDate.Font.Size = 12;
        tcHeaderToDate.Style["Text-align"] = "Left";
        trHeaderToDate.Cells.Add(tcHeaderToDate);

        TableCell tcHeaderToDateValue = new TableCell();
        span = new LiteralControl("<span style='font-size:1px;'>.</span>" + Convert.ToDateTime(strToDate).ToString("dd-MMM-yyyy"));
        tcHeaderToDateValue.Text = "" + Convert.ToDateTime(strToDate).ToString("dd-MMM-yyyy");
        tcHeaderToDateValue.ColumnSpan = 2;
        tcHeaderToDateValue.Font.Size = 12;
        tcHeaderToDateValue.Style["Text-align"] = "Left";
        tcHeaderToDateValue.Controls.Add(span);
        trHeaderToDate.Cells.Add(tcHeaderToDateValue);


        TableCell tcHeaderExtraHourEXP = new TableCell();
        tcHeaderExtraHourEXP.Text = "Extra Scheduled Hours:- Scheduled Hours - Maximum.";
        tcHeaderExtraHourEXP.ColumnSpan = 9;
        tcHeaderExtraHourEXP.Font.Size = 9;
        tcHeaderExtraHourEXP.Style["Text-align"] = "Left";
        trHeaderToDate.Cells.Add(tcHeaderExtraHourEXP);



        Table1.Rows.Add(trHeaderToDate);

        //date time(created date)
        TableRow trHeaderDateTime = new TableRow();

        TableCell tcHeaderDateTime = new TableCell();
        tcHeaderDateTime.Text = "Created Date";
        tcHeaderDateTime.ColumnSpan = 2;
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
        tcHeaderDateTimeValue.ColumnSpan = 2;
        tcHeaderDateTimeValue.Font.Size = 12;
        tcHeaderDateTimeValue.Style["Text-align"] = "Left";
        tcHeaderDateTimeValue.Controls.Add(span);
        trHeaderDateTime.Cells.Add(tcHeaderDateTimeValue);



        TableCell tcHeaderACTworkingHourEXP = new TableCell();
        tcHeaderACTworkingHourEXP.Text = "Actual Working Hours:- Total number of hours based on the time log within selected period.";
        tcHeaderACTworkingHourEXP.ColumnSpan = 9;
        tcHeaderACTworkingHourEXP.Font.Size =9;
        tcHeaderACTworkingHourEXP.Style["Text-align"] = "Left";
        trHeaderDateTime.Cells.Add(tcHeaderACTworkingHourEXP);



        Table1.Rows.Add(trHeaderDateTime);


        //date time(created by)
        TableRow trHeaderCreatedBy = new TableRow();

        TableCell tcHeaderCreatedBy = new TableCell();
        tcHeaderCreatedBy.Text = "Created By";
        tcHeaderCreatedBy.ColumnSpan = 2;
        tcHeaderCreatedBy.Font.Size = 12;
        tcHeaderCreatedBy.Font.Bold = true;
        tcHeaderCreatedBy.Style["Text-align"] = "Left";
        trHeaderCreatedBy.Cells.Add(tcHeaderCreatedBy);

        TableCell tcHeaderCreatedByValue = new TableCell();
        tcHeaderCreatedByValue.Text = struserId;
        tcHeaderCreatedByValue.ColumnSpan = 2;
        tcHeaderCreatedByValue.Font.Size = 12;
        tcHeaderCreatedByValue.Style["Text-align"] = "Left";
        trHeaderCreatedBy.Cells.Add(tcHeaderCreatedByValue);


        TableCell tcHeaderEXTworkingHourEXP = new TableCell();
        tcHeaderEXTworkingHourEXP.Text = "Extra Actual Working Hours:- Actual Working Hours - Maximum Hours + PH Working Hours";
        tcHeaderEXTworkingHourEXP.ColumnSpan = 9;
        tcHeaderEXTworkingHourEXP.Font.Size = 9;
        tcHeaderEXTworkingHourEXP.Style["Text-align"] = "Left";
        trHeaderCreatedBy.Cells.Add(tcHeaderEXTworkingHourEXP);




        Table1.Rows.Add(trHeaderCreatedBy);






        TableRow TrTopBlankRow = new TableRow();
        TableCell TcTopBlankcell = new TableCell();
        TcTopBlankcell.Text = "-";
        TcTopBlankcell.ColumnSpan =8;
        TrTopBlankRow.Cells.Add(TcTopBlankcell);

        TableCell tcscheduled = new TableCell();
        tcscheduled.Text = "Scheduled";
        tcscheduled.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("c5be97").ToString());
        tcscheduled.ColumnSpan = 3;
        tcscheduled.HorizontalAlign = HorizontalAlign.Center;
        tcscheduled.VerticalAlign = VerticalAlign.Middle;
        tcscheduled.Style["border"] = "dotted";
        tcscheduled.Style["border-width"] = "1px";
        tcscheduled.Font.Size = 11;
        tcscheduled.Font.Bold = true;
        TrTopBlankRow.Cells.Add(tcscheduled);

        TableCell tcActual = new TableCell();
        tcActual.Text = "Actual";
        tcActual.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("d8d8d8").ToString());
        tcActual.ColumnSpan = 2;
        tcActual.HorizontalAlign = HorizontalAlign.Center;
        tcActual.VerticalAlign = VerticalAlign.Middle;
        tcActual.Style["border"] = "dotted";
        tcActual.Style["border-width"] = "1px";
        tcActual.Font.Size =11;
        tcActual.Font.Bold = true;
        TrTopBlankRow.Cells.Add(tcActual);


        Table1.Rows.Add(TrTopBlankRow);


        TableCell tcSNOEmp = new TableCell();
        tcSNOEmp.Text = "SNO";
        tcSNOEmp.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("d7e4bc").ToString());
        tcSNOEmp.Font.Size = 11;
        tcSNOEmp.HorizontalAlign = HorizontalAlign.Center;
        tcSNOEmp.VerticalAlign = VerticalAlign.Middle;
        tcSNOEmp.Style["border"] = "dotted";
        tcSNOEmp.Style["border-width"] = "1px";
        tcSNOEmp.Font.Bold = true;
        TableCell tcLOcValue = new TableCell();
        tcLOcValue.Text = "Location";
        tcLOcValue.Font.Size = 11;
        tcLOcValue.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("d7e4bc").ToString());
        tcLOcValue.Wrap = false;
        tcLOcValue.Style["border"] = "dotted";
        tcLOcValue.Style["border-width"] = "1px";
        tcLOcValue.Font.Bold = true;
        TableCell tcdeptValue = new TableCell();
        tcdeptValue.Text = "Department";
        tcdeptValue.Font.Size = 11;
        tcdeptValue.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("d7e4bc").ToString());
        tcdeptValue.Wrap = false;
        tcdeptValue.Style["border"] = "dotted";
        tcdeptValue.Style["border-width"] = "1px";
        tcdeptValue.Font.Bold = true;
        TableCell tcdesgValue = new TableCell();
        tcdesgValue.Text = "Designation";
        tcdesgValue.Font.Size = 11;
        tcdesgValue.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("d7e4bc").ToString());
        tcdesgValue.Wrap = false;
        tcdesgValue.Style["border"] = "dotted";
        tcdesgValue.Style["border-width"] = "1px";
        tcdesgValue.Font.Bold = true;
        TableCell tcEmpCode = new TableCell();
        tcEmpCode.Text = "Employee#";
        tcEmpCode.Font.Size = 11;
        tcEmpCode.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("b8cce4").ToString());
        tcEmpCode.Wrap = false;
        tcEmpCode.Style["border"] = "dotted";
        tcEmpCode.Style["border-width"] = "1px";
        tcEmpCode.Font.Bold = true;
        TableCell tcEmpName = new TableCell();
        tcEmpName.Text = "Name";
        tcEmpName.Font.Size = 11;
        tcEmpName.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("b8cce4").ToString());
        tcEmpName.Wrap = false;
        tcEmpName.Style["border"] = "dotted";
        tcEmpName.Style["border-width"] = "1px";
        tcEmpName.Font.Bold = true;

        TableCell tcLineManagerCode = new TableCell();
        tcLineManagerCode.Text = "Line Manager Code";
        tcLineManagerCode.Font.Size = 11;
        tcLineManagerCode.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("b8cce4").ToString());
        tcLineManagerCode.Wrap = false;
        tcLineManagerCode.Style["border"] = "dotted";
        tcLineManagerCode.Style["border-width"] = "1px";
        tcLineManagerCode.Font.Bold = true;


        TableCell tcLineManagerName = new TableCell();
        tcLineManagerName.Text = "Line Manager Name";
        tcLineManagerName.Font.Size = 11;
        tcLineManagerName.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("b8cce4").ToString());
        tcLineManagerName.Wrap = false;
        tcLineManagerName.Style["border"] = "dotted";
        tcLineManagerName.Style["border-width"] = "1px";
        tcLineManagerName.Font.Bold = true;


        TableCell tcScheduledHours = new TableCell();
        tcScheduledHours.Font.Size = 11;
        tcScheduledHours.Text = "Scheduled <br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' /> Hours";
        tcScheduledHours.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ddd9c3").ToString());
        tcScheduledHours.HorizontalAlign = HorizontalAlign.Center;
        tcScheduledHours.Style["border"] = "dotted";
        tcScheduledHours.Style["border-width"] = "1px";
        tcScheduledHours.Font.Bold = true;

        TableCell tcMaximumHours = new TableCell();
        tcMaximumHours.Font.Size = 11;
        tcMaximumHours.Text = "Maximum <br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' /> Hours";
        tcMaximumHours.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ddd9c3").ToString());
        tcMaximumHours.HorizontalAlign = HorizontalAlign.Center;
        tcMaximumHours.Wrap = true;
        tcMaximumHours.Style["border"] = "dotted";
        tcMaximumHours.Style["border-width"] = "1px";
        tcMaximumHours.Font.Bold = true;



        TableCell tcExtraHours = new TableCell();
        tcExtraHours.Font.Size = 11;
        tcExtraHours.Text = "Extra <br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' /> Hours";
        tcExtraHours.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ddd9c3").ToString());
        tcExtraHours.HorizontalAlign = HorizontalAlign.Center;
        tcExtraHours.Wrap = true;
        tcExtraHours.Style["border"] = "dotted";
        tcExtraHours.Style["border-width"] = "1px";
        tcExtraHours.Font.Bold = true;



        TableCell tcActWorkingHours = new TableCell();
        tcActWorkingHours.Font.Size = 11;
        tcActWorkingHours.Text = "Actual <br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' /> Working Hours";
        tcActWorkingHours.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("eeece1").ToString());
        tcActWorkingHours.HorizontalAlign = HorizontalAlign.Center;
        tcActWorkingHours.Wrap = true;
        tcActWorkingHours.Style["border"] = "dotted";
        tcActWorkingHours.Style["border-width"] = "1px";
        tcActWorkingHours.Font.Bold = true;


        TableCell tcExtraWorkingHours = new TableCell();
        tcExtraWorkingHours.Font.Size = 11;
        tcExtraWorkingHours.Text = "Extra <br style = 'mso-data-placement:same-cell;text-align:center;vertical-align:middle;' /> Working Hours";
        tcExtraWorkingHours.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("eeece1").ToString());
        tcExtraWorkingHours.HorizontalAlign = HorizontalAlign.Center;
        tcExtraWorkingHours.Wrap = true;
        tcExtraWorkingHours.Style["border"] = "dotted";
        tcExtraWorkingHours.Style["border-width"] = "1px";
        tcExtraWorkingHours.Font.Bold = true;


       
        TableRow TrHeaderRow = new TableRow();
        TrHeaderRow.Cells.AddAt(0, tcSNOEmp);
        TrHeaderRow.Cells.AddAt(1, tcLOcValue);
        TrHeaderRow.Cells.AddAt(2, tcdeptValue);
        TrHeaderRow.Cells.AddAt(3, tcdesgValue);
        TrHeaderRow.Cells.AddAt(4, tcEmpCode);
        TrHeaderRow.Cells.AddAt(5, tcEmpName);
        TrHeaderRow.Cells.AddAt(6, tcLineManagerCode);
        TrHeaderRow.Cells.AddAt(7, tcLineManagerName);
        TrHeaderRow.Cells.AddAt(8, tcScheduledHours);
        TrHeaderRow.Cells.AddAt(9, tcMaximumHours);
        TrHeaderRow.Cells.AddAt(10, tcExtraHours);
        TrHeaderRow.Cells.AddAt(11, tcActWorkingHours);
        TrHeaderRow.Cells.AddAt(12, tcExtraWorkingHours);
        Table1.Rows.Add(TrHeaderRow);

        if (Convert.ToDateTime(strFromDate).Date > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
        {
            strFromDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString("dd-MMM-yyyy");
        }

        if (Convert.ToDateTime(strToDate).Date > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
        {
            strToDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString("dd-MMM-yyyy");
        }

        DataTable dtFilter = ObjRegister.GetAttendanceReport(strEmpIdList, Convert.ToDateTime(strFromDate).ToString(), Convert.ToDateTime(strToDate).ToString(), "16");

        int counter = 0;
        foreach (DataRow dr in dtFilter.Rows)
        {
            counter++;
            TableRow trDetailRow = new TableRow();

            TableCell tcSNOEmpdetail = new TableCell();
            tcSNOEmpdetail.Text = counter.ToString();
            tcSNOEmpdetail.Wrap = false;
            tcSNOEmpdetail.Style["border"] = "dotted";
            tcSNOEmpdetail.Style["border-width"] = "1px";
            TableCell tcLOcValueDetail = new TableCell();
            tcLOcValueDetail.Text = dr["location_name"].ToString();
            tcLOcValueDetail.Wrap = false;
            tcLOcValueDetail.Style["border"] = "dotted";
            tcLOcValueDetail.Style["border-width"] = "1px";
            TableCell tcdeptValueDetail = new TableCell();
            tcdeptValueDetail.Text = dr["dep_name"].ToString();
            tcdeptValueDetail.Wrap = false;
            tcdeptValueDetail.Style["border"] = "dotted";
            tcdeptValueDetail.Style["border-width"] = "1px";
            TableCell tcdesgValueDetail = new TableCell();
            tcdesgValueDetail.Text = dr["designation"].ToString();
            tcdesgValueDetail.Wrap = false;
            tcdesgValueDetail.Style["border"] = "dotted";
            tcdesgValueDetail.Style["border-width"] = "1px";
            TableCell tcEmpCodeDetail = new TableCell();
            tcEmpCodeDetail.Text = dr["emp_code"].ToString();
            tcEmpCodeDetail.Wrap = false;
            tcEmpCodeDetail.Style["border"] = "dotted";
            tcEmpCodeDetail.Style["border-width"] = "1px";
            TableCell tcEmpNameDetail = new TableCell();
            tcEmpNameDetail.Text = dr["emp_name"].ToString();
            tcEmpNameDetail.Wrap = false;
            tcEmpNameDetail.Style["border"] = "dotted";
            tcEmpNameDetail.Style["border-width"] = "1px";
            TableCell tcLineManagerCodedetail = new TableCell();
            tcLineManagerCodedetail.Text = dr["LineManagerCode"].ToString();
            tcLineManagerCodedetail.Wrap = false;
            tcLineManagerCodedetail.Style["border"] = "dotted";
            tcLineManagerCodedetail.Style["border-width"] = "1px";
            TableCell tcLineManagerNamedetail = new TableCell();
            tcLineManagerNamedetail.Text = dr["LineManagerName"].ToString();
            tcLineManagerNamedetail.Wrap = false;
            tcLineManagerNamedetail.Style["border"] = "dotted";
            tcLineManagerNamedetail.Style["border-width"] = "1px";
            TableCell tcScheduledHoursDetail = new TableCell();
            tcScheduledHoursDetail.Text = dr["Total_Schedules_HHMM"].ToString();
            tcScheduledHoursDetail.Wrap = false;
            tcScheduledHoursDetail.Style["border"] = "dotted";
            tcScheduledHoursDetail.Style["border-width"] = "1px";
            TableCell tcMaximumHoursDetail = new TableCell();
            tcMaximumHoursDetail.Text = dr["Maximum_Schedules_HHMM"].ToString();
            tcMaximumHoursDetail.Wrap = false;
            tcMaximumHoursDetail.Style["border"] = "dotted";
            tcMaximumHoursDetail.Style["border-width"] = "1px";
            TableCell tcExtraHoursDetail = new TableCell();
            tcExtraHoursDetail.Text = dr["Extra_Schedules_HHMM"].ToString();
            tcExtraHoursDetail.Wrap = false;
            tcExtraHoursDetail.Style["border"] = "dotted";
            tcExtraHoursDetail.Style["border-width"] = "1px";
            TableCell tcActWorkingHoursdetail = new TableCell();
            tcActWorkingHoursdetail.Text = dr["Actual_Working_HHMM"].ToString();
            tcActWorkingHoursdetail.Wrap = false;
            tcActWorkingHoursdetail.Style["border"] = "dotted";
            tcActWorkingHoursdetail.Style["border-width"] = "1px";
            TableCell tcExtraWorkingHoursDetail = new TableCell();
            tcExtraWorkingHoursDetail.Text = dr["Extra_Working_HHMM"].ToString();
            tcExtraWorkingHoursDetail.Wrap = false;
            tcExtraWorkingHoursDetail.Style["border"] = "dotted";
            tcExtraWorkingHoursDetail.Style["border-width"] = "1px";


            trDetailRow.Cells.AddAt(0, tcSNOEmpdetail);
            trDetailRow.Cells.AddAt(1, tcLOcValueDetail);
            trDetailRow.Cells.AddAt(2, tcdeptValueDetail);
            trDetailRow.Cells.AddAt(3, tcdesgValueDetail);
            trDetailRow.Cells.AddAt(4, tcEmpCodeDetail);
            trDetailRow.Cells.AddAt(5, tcEmpNameDetail);
            trDetailRow.Cells.AddAt(6, tcLineManagerCodedetail);
            trDetailRow.Cells.AddAt(7, tcLineManagerNamedetail);
            trDetailRow.Cells.AddAt(8, tcScheduledHoursDetail);
            trDetailRow.Cells.AddAt(9, tcMaximumHoursDetail);
            trDetailRow.Cells.AddAt(10, tcExtraHoursDetail);
            trDetailRow.Cells.AddAt(11, tcActWorkingHoursdetail);
            trDetailRow.Cells.AddAt(12, tcExtraWorkingHoursDetail);
            Table1.Rows.Add(trDetailRow);
        }

        return Table1;

    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename = TSC Overtime Report " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("ddMMyyyy_HHmmss") + ".xls");
        Response.ContentEncoding = Encoding.UTF8;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        divexport.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }
}