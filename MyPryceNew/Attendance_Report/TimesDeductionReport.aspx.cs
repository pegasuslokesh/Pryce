using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;

public partial class Attendance_Report_PendingHolidayReport : System.Web.UI.Page
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



    public Table GetReport1(string strCompanyId, string strBrandId, string strLocationId, string strEmpIdList, string strFromDate, string strToDate, bool isDownload, string struserId = "")
    {
        Table Table1 = new Table();

        Table1.Style["Text-align"] = "Left";
        Table1.Style["Text-align"] = "Left";

        // Table1.GridLines = GridLines.Both;
        Table1.Font.Size = 10;


        TableRow trHeaderReportName = new TableRow();


        TableCell tcHeaderReportName = new TableCell();
        tcHeaderReportName.Text = "Report Name";
        tcHeaderReportName.Font.Bold = true;

        tcHeaderReportName.Font.Size = 12;
        tcHeaderReportName.ColumnSpan = 3;
        tcHeaderReportName.Style["Text-align"] = "Left";
        trHeaderReportName.Cells.Add(tcHeaderReportName);

        TableCell tcHeaderReportvalue = new TableCell();
        tcHeaderReportvalue.Text = "TSC Times Deductions";
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



        TableRow TrTopBlankRow = new TableRow();
        TableCell TcTopBlankcell = new TableCell();
        TcTopBlankcell.Text = "-";
        TcTopBlankcell.ColumnSpan = 12;
        TrTopBlankRow.Cells.Add(TcTopBlankcell);
        Table1.Rows.Add(TrTopBlankRow);


        TableCell tcSNOEmp = new TableCell();
        tcSNOEmp.Text = "SNO";
        tcSNOEmp.Wrap = false;
        tcSNOEmp.Style["border"] = "dotted";
        tcSNOEmp.Style["border-width"] = "1px";
        tcSNOEmp.Font.Bold = true;
        TableCell tcEmpCode = new TableCell();
        tcEmpCode.Text = "Employee#";
        tcEmpCode.Wrap = false;
        tcEmpCode.Style["border"] = "dotted";
        tcEmpCode.Style["border-width"] = "1px";
        tcEmpCode.Font.Bold = true;
        TableCell tcEmpName = new TableCell();
        tcEmpName.Text = "Name";
        tcEmpName.Wrap = false;
        tcEmpName.Style["border"] = "dotted";
        tcEmpName.Style["border-width"] = "1px";
        tcEmpName.Font.Bold = true;
        TableCell tcEmpStatus = new TableCell();
        tcEmpStatus.Text = "Status";
        tcEmpStatus.Wrap = false;
        tcEmpStatus.Style["border"] = "dotted";
        tcEmpStatus.Style["border-width"] = "1px";
        tcEmpStatus.Font.Bold = true;
        TableCell tcdeviceGroupValue = new TableCell();
        tcdeviceGroupValue.Text = "Device Group";
        tcdeviceGroupValue.Wrap = false;
        tcdeviceGroupValue.Style["border"] = "dotted";
        tcdeviceGroupValue.Style["border-width"] = "1px";
        tcdeviceGroupValue.Font.Bold = true;
        TableCell tcLOcValue = new TableCell();
        tcLOcValue.Text = "Location";
        tcLOcValue.Wrap = false;
        tcLOcValue.Style["border"] = "dotted";
        tcLOcValue.Style["border-width"] = "1px";
        tcLOcValue.Font.Bold = true;
        TableCell tcdeptValue = new TableCell();
        tcdeptValue.Text = "Department";
        tcdeptValue.Wrap = false;
        tcdeptValue.Style["border"] = "dotted";
        tcdeptValue.Style["border-width"] = "1px";
        tcdeptValue.Font.Bold = true;
        TableCell tcdesgValue = new TableCell();
        tcdesgValue.Text = "Designation";
        tcdesgValue.Wrap = false;
        tcdesgValue.Style["border"] = "dotted";
        tcdesgValue.Style["border-width"] = "1px";
        tcdesgValue.Font.Bold = true;
        TableCell tcDeductedDays = new TableCell();
        tcDeductedDays.Text = "Deducted Days";
        tcDeductedDays.Wrap = false;
        tcDeductedDays.Style["border"] = "dotted";
        tcDeductedDays.Style["border-width"] = "1px";
        tcDeductedDays.Font.Bold = true;
        TableCell tcDeductedMinutes = new TableCell();
        tcDeductedMinutes.Text = "Deducted Minutes";
        tcDeductedMinutes.Wrap = false;
        tcDeductedMinutes.Style["border"] = "dotted";
        tcDeductedMinutes.Style["border-width"] = "1px";
        tcDeductedMinutes.Font.Bold = true;
        TableRow TrHeaderRow = new TableRow();
        TrHeaderRow.Cells.AddAt(0, tcSNOEmp);
        TrHeaderRow.Cells.AddAt(1, tcEmpCode);
        TrHeaderRow.Cells.AddAt(2, tcEmpName);
        TrHeaderRow.Cells.AddAt(3, tcEmpStatus);
        TrHeaderRow.Cells.AddAt(4, tcdeviceGroupValue);
        TrHeaderRow.Cells.AddAt(5, tcLOcValue);
        TrHeaderRow.Cells.AddAt(6, tcdeptValue);
        TrHeaderRow.Cells.AddAt(7, tcdesgValue);
        TrHeaderRow.Cells.AddAt(8, tcDeductedDays);
        TrHeaderRow.Cells.AddAt(9, tcDeductedMinutes);
        Table1.Rows.Add(TrHeaderRow);

        if (Convert.ToDateTime(strFromDate).Date > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
        {
            strFromDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString("dd-MMM-yyyy");
        }

        if (Convert.ToDateTime(strToDate).Date > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
        {
            strToDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString("dd-MMM-yyyy");
        }

        DataTable dtFilter = ObjRegister.GetAttendanceReport(strEmpIdList, Convert.ToDateTime(strFromDate).ToString(), Convert.ToDateTime(strToDate).ToString(), "14");

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
            TableCell tcEmpStatusDetail = new TableCell();
            tcEmpStatusDetail.Text = dr["emp_type"].ToString();
            tcEmpStatusDetail.Wrap = false;
            tcEmpStatusDetail.Style["border"] = "dotted";
            tcEmpStatusDetail.Style["border-width"] = "1px";
            TableCell tcdeviceGroupValueDetail = new TableCell();
            tcdeviceGroupValueDetail.Text = dr["group_name"].ToString();
            tcdeviceGroupValueDetail.Wrap = false;
            tcdeviceGroupValueDetail.Style["border"] = "dotted";
            tcdeviceGroupValueDetail.Style["border-width"] = "1px";
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
            TableCell tcDeductedDaysDetail = new TableCell();
            tcDeductedDaysDetail.Text = dr["AbsentDay"].ToString();
            tcDeductedDaysDetail.Wrap = false;
            tcDeductedDaysDetail.Style["Text-align"] = "Center";
            tcDeductedDaysDetail.Style["border"] = "dotted";
            tcDeductedDaysDetail.Style["border-width"] = "1px";
            TableCell tcDeductedMinutesDetail = new TableCell();
            tcDeductedMinutesDetail.Text = dr["ShortMinute"].ToString();
            tcDeductedMinutesDetail.Wrap = false;
            tcDeductedMinutesDetail.Style["Text-align"] = "Center";
            tcDeductedMinutesDetail.Style["border"] = "dotted";
            tcDeductedMinutesDetail.Style["border-width"] = "1px";
            trDetailRow.Cells.AddAt(0, tcSNOEmpdetail);
            trDetailRow.Cells.AddAt(1, tcEmpCodeDetail);
            trDetailRow.Cells.AddAt(2, tcEmpNameDetail);
            trDetailRow.Cells.AddAt(3, tcEmpStatusDetail);
            trDetailRow.Cells.AddAt(4, tcdeviceGroupValueDetail);
            trDetailRow.Cells.AddAt(5, tcLOcValueDetail);
            trDetailRow.Cells.AddAt(6, tcdeptValueDetail);
            trDetailRow.Cells.AddAt(7, tcdesgValueDetail);
            trDetailRow.Cells.AddAt(8, tcDeductedDaysDetail);
            trDetailRow.Cells.AddAt(9, tcDeductedMinutesDetail);
            Table1.Rows.Add(trDetailRow);
        }

        return Table1;

    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename = TSC Times Deductions " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("ddMMyyyy_HHmmss") + ".xls");
        Response.ContentEncoding = Encoding.UTF8;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        divexport.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }
}