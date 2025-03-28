using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using PegasusDataAccess;
public partial class Attendance_Report_Shift_Schedule_Report : System.Web.UI.Page
{
    Att_ShiftDescription objShift = null;
    DataAccessClass objDa = null;
    SystemParameter objSys = null;
    Att_Leave_Request ObjLeaveReq = null;
    Set_Employee_Holiday objEmpHoli = null;
    Set_ApplicationParameter objAppParam = null;
    Att_ShiftManagement objShiftManagement = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objShift = new Att_ShiftDescription(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLeaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objEmpHoli = new Set_Employee_Holiday(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objShiftManagement = new Att_ShiftManagement(Session["DBConnection"].ToString());

        if (Session["EmpList"] == null)
        {
            Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
        }
        else
        {
            GetReport1();

        }
    }
    public void GetReport()
    {
        DataTable dtTime = new DataTable();
        dtTime.Columns.Add("EmpCode");
        dtTime.Columns.Add("EmpName");
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        string Emplist = string.Empty;
        FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
        ToDate = objSys.getDateForInput(Session["ToDate"].ToString());
        Emplist = Session["EmpList"].ToString();
        //if (lblSelectRecord.Text == "")
        //{
        //    if (Session["EmpList1"] != null)
        //    {
        //        Emplist = Session["EmpList1"].ToString();
        //    }
        //}
        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Att_ScheduleDescription_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Emplist);
        if (Emplist != "")
        {
            dtFilter = new DataView(rptdata.sp_Att_ScheduleDescription_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") ", "Att_Date_1", DataViewRowState.CurrentRows).ToTable();
        }



        int dayscount = 0;
        dayscount = Convert.ToDateTime(ToDate).Subtract(Convert.ToDateTime(FromDate)).Days;

        for (int r = 1; r <= dayscount + 1; r++)
        {
            dtTime.Columns.Add("Date" + r.ToString());
        }
        DataTable dtEmpCode = new DataTable();
        try
        {
            dtEmpCode = dtFilter.DefaultView.ToTable(true, "Emp_Id");
        }
        catch
        {
        }
        DataTable dtPerDate = new DataTable();
        dtPerDate.Columns.Add("Att_Date_1");
        DataTable dtDate = new DataTable();

        while (FromDate <= ToDate)
        {
            dtPerDate.Rows.Add(FromDate.ToString(objSys.SetDateFormat()));
            FromDate = FromDate.AddDays(1);
        }

        //try
        //{
        //    dtPerDate = dtFilter.DefaultView.ToTable(true, "Att_Date_1");
        //}
        //catch
        //{
        //}
        int j = 0;
        DataRow dr = dtTime.NewRow();
        //while (j < dtPerDate.Rows.Count)
        //{
        //    //dr["Date" + (j + 1).ToString()] = Convert.ToDateTime(dtPerDate.Rows[j]["Att_Date_1"].ToString()).ToString("dd-MMM-yyyy") + "  " + Convert.ToDateTime(dtPerDate.Rows[j]["Att_Date_1"].ToString()).DayOfWeek.ToString().Substring(0, 3);
        //    dr["Date" + (j + 1).ToString()] = Convert.ToDateTime(dtPerDate.Rows[j]["Att_Date_1"].ToString()).Day.ToString();
        //    j++;
        //}


        FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
        while (j < dtTime.Columns.Count - 2)
        {

            //dr["Date" + (j + 1).ToString()] = Convert.ToDateTime(dtPerDate.Rows[j]["Att_Date_1"].ToString()).ToString("dd-MMM-yyyy") + "  " + Convert.ToDateTime(dtPerDate.Rows[j]["Att_Date_1"].ToString()).DayOfWeek.ToString().Substring(0, 3);
            //if (j == 0)
            //{
            //    dr["Date" + (j + 1).ToString()] = Convert.ToDateTime(FromDate).Day.ToString();
            //    j++;
            //}
            //else
            //{
            dr["Date" + (j + 1).ToString()] = Convert.ToDateTime(FromDate).AddDays(j).Day.ToString();
            j++;
            //}
        }
        dr["EmpCode"] = "Code";
        dr["EmpName"] = "Name";
        dtTime.Rows.Add(dr);
        int j1 = 0;
        int k1 = 2;
        if (dtEmpCode.Rows.Count > 0)
        {
            for (int i = 0; i < dtEmpCode.Rows.Count; i++)
            {
                j1 = 0;
                k1 = 2;
                DataTable dtDate1 = new DataTable();
                dtDate = new DataView(dtFilter, "Emp_Id='" + dtEmpCode.Rows[i]["Emp_Id"].ToString() + "'", "Att_Date_1 desc", DataViewRowState.CurrentRows).ToTable();
                DataRow dr1 = dtTime.NewRow();
                if (dtDate.Rows.Count > 0)
                {
                    dr1["EmpCode"] = dtDate.Rows[0]["Emp_Code"].ToString();
                    dr1["EmpName"] = dtDate.Rows[0]["Emp_Name"].ToString();
                    while (j1 < dtPerDate.Rows.Count)
                    {
                        string s1 = dtTime.Rows[0][k1].ToString();
                        if (s1.Length == 1)
                            s1 = "0" + s1;
                        string ss = dtPerDate.Rows[j1]["Att_Date_1"].ToString().Substring(0, 2);
                        if (ss == s1)
                        {
                            dtDate1 = new DataView(dtDate, "Att_Date_1='" + dtPerDate.Rows[j1]["Att_Date_1"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (objEmpHoli.GetEmployeeHolidayOnDateAndEmpId(Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date_1"].ToString()).ToString(), dtEmpCode.Rows[i]["Emp_Id"].ToString()))
                            {
                                dr1["Date" + (k1 - 1).ToString()] = "Holiday";
                                j++;
                            }
                            else if (ObjLeaveReq.IsLeaveOnDate(Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date_1"].ToString()).ToString(), dtEmpCode.Rows[i]["Emp_Id"].ToString()))
                            {
                                string strLeaveName = string.Empty;
                                strLeaveName = ObjLeaveReq.GetLeavetypeName(Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date_1"].ToString()).ToString(), dtEmpCode.Rows[i]["Emp_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                                //if (!strLeaveName.ToUpper().ToString().Contains("LEAVE"))
                                //{
                                //    strLeaveName += " Leave";
                                //}
                                dr1["Date" + (k1 - 1).ToString()] = strLeaveName;
                                j++;
                            }
                            else if (dtDate1.Rows.Count > 0)
                            {
                                if (dtDate1.Rows.Count > 1)
                                {
                                    string Time = string.Empty;
                                    dtDate1 = new DataView(dtDate1, "", "Shift_Name", DataViewRowState.CurrentRows).ToTable();
                                    for (int t = 0; t < dtDate1.Rows.Count; t++)
                                    {
                                        try
                                        {
                                            Time = dtDate1.Rows[t]["Shift_Name"].ToString();
                                            dr1["Date" + (k1 - 1).ToString()] = Time;
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToBoolean(dtDate1.Rows[0]["Is_Off"].ToString()))
                                    {
                                        dr1["Date" + (k1 - 1).ToString()] = "Week Off";
                                    }
                                    else
                                    {
                                        dr1["Date" + (k1 - 1).ToString()] = dtDate1.Rows[0]["Shift_Name"].ToString();
                                    }
                                }
                            }
                            j1++;
                        }
                        k1++;
                    }
                }

                dtTime.Rows.Add(dr1);
            }
        }
        Map(dtTime);
        lblTitle.Text = "Employee Shift Schedule Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat());

    }



    public void GetReport1()
    {
        DataTable dtTime = new DataTable();
        dtTime.Columns.Add("EmpCode");
        dtTime.Columns.Add("EmpName");
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        string Emplist = string.Empty;
        FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
        ToDate = objSys.getDateForInput(Session["ToDate"].ToString());
        Emplist = Session["EmpList"].ToString();

        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Att_ScheduleDescription_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Emplist);
        if (Emplist != "")
        {
            dtFilter = new DataView(rptdata.sp_Att_ScheduleDescription_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") ", "Att_Date_1", DataViewRowState.CurrentRows).ToTable();
        }



        int dayscount = 0;
        dayscount = Convert.ToDateTime(ToDate).Subtract(Convert.ToDateTime(FromDate)).Days;

        for (int r = 1; r <= dayscount + 1; r++)
        {
            dtTime.Columns.Add("Date" + r.ToString());
        }
        DataTable dtEmpCode = new DataTable();
        try
        {
            dtEmpCode = objDa.return_DataTable("select Emp_id, Location_Id,Emp_Code,Emp_Name from set_employeemaster where Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") order by CAST(emp_code as int)");
        }
        catch
        {
        }
        DataTable dtPerDate = new DataTable();
        dtPerDate.Columns.Add("Att_Date_1");
        DataTable dtDate = new DataTable();

        while (FromDate <= ToDate)
        {
            dtPerDate.Rows.Add(FromDate.ToString(objSys.SetDateFormat()));
            FromDate = FromDate.AddDays(1);
        }


        int j = 0;
        DataRow dr = dtTime.NewRow();


        FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
        while (j < dtTime.Columns.Count - 2)
        {


            dr["Date" + (j + 1).ToString()] = Convert.ToDateTime(FromDate).AddDays(j).Day.ToString();
            j++;

        }
        dr["EmpCode"] = "Code";
        dr["EmpName"] = "Name";
        dtTime.Rows.Add(dr);
        int j1 = 0;
        int k1 = 2;
        string DefaultShiftId = string.Empty;
        bool IsDataFound = false;
        if (dtEmpCode.Rows.Count > 0)
        {
            for (int i = 0; i < dtEmpCode.Rows.Count; i++)
            {
                IsDataFound = false;
                j1 = 0;
                k1 = 2;
                DataRow dr1 = dtTime.NewRow();
                dr1["EmpCode"] = dtEmpCode.Rows[i]["Emp_Code"].ToString();
                dr1["EmpName"] = dtEmpCode.Rows[i]["Emp_Name"].ToString();
                DefaultShiftId = objAppParam.GetApplicationParameterValueByParamName("Default_Shift", Session["CompId"].ToString(), Session["BrandId"].ToString(), dtEmpCode.Rows[i]["Location_Id"].ToString());
                while (j1 < dtPerDate.Rows.Count)
                {
                    string s1 = dtTime.Rows[0][k1].ToString();
                    if (s1.Length == 1)
                        s1 = "0" + s1;
                    string ss = dtPerDate.Rows[j1]["Att_Date_1"].ToString().Substring(0, 2);
                    if (ss == s1)
                    {
                        dtDate = new DataView(dtFilter, "Att_Date_1='" + dtPerDate.Rows[j1]["Att_Date_1"].ToString() + "' and emp_id=" + dtEmpCode.Rows[i]["Emp_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (objEmpHoli.GetEmployeeHolidayOnDateAndEmpId(Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date_1"].ToString()).ToString(), dtEmpCode.Rows[i]["Emp_Id"].ToString()))
                        {
                            dr1["Date" + (k1 - 1).ToString()] = "Holiday";
                            j++;
                            IsDataFound = true;
                        }
                        else if (ObjLeaveReq.IsLeaveOnDate(Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date_1"].ToString()).ToString(), dtEmpCode.Rows[i]["Emp_Id"].ToString()))
                        {
                            string strLeaveName = string.Empty;
                            strLeaveName = ObjLeaveReq.GetLeavetypeName(Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date_1"].ToString()).ToString(), dtEmpCode.Rows[i]["Emp_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                            dr1["Date" + (k1 - 1).ToString()] = strLeaveName;
                            j++;
                            IsDataFound = true;
                        }
                        else if (dtDate.Rows.Count > 0)
                        {
                            if (dtDate.Rows.Count > 1)
                            {
                                string Time = string.Empty;
                                dtDate = new DataView(dtDate, "", "Shift_Name", DataViewRowState.CurrentRows).ToTable();
                                for (int t = 0; t < dtDate.Rows.Count; t++)
                                {
                                    try
                                    {
                                        Time = dtDate.Rows[t]["Shift_Name"].ToString();
                                        dr1["Date" + (k1 - 1).ToString()] = Time;
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            else
                            {
                                if (dtDate.Rows[0]["Is_Off"].ToString().Trim() == "")
                                {
                                    dr1["Date" + (k1 - 1).ToString()] = dtDate.Rows[0]["Shift_Name"].ToString();
                                }
                                else if (Convert.ToBoolean(dtDate.Rows[0]["Is_Off"].ToString()))
                                {
                                    dr1["Date" + (k1 - 1).ToString()] = "Week Off";
                                }
                                else
                                {
                                    dr1["Date" + (k1 - 1).ToString()] = dtDate.Rows[0]["Shift_Name"].ToString();
                                }
                            }
                            IsDataFound = true;
                        }
                        //here we can write code for deafult shift
                        else if (DefaultShiftId != "" && DefaultShiftId != "0")
                        {

                            DataTable defaultShift = objShift.GetShiftDescriptionByShiftId(DefaultShiftId);

                            if (defaultShift.Rows.Count > 0)
                            {
                                string strCycleType = defaultShift.Rows[0]["Cycle_Type"].ToString().Split('-')[0].ToString();

                                switch (strCycleType)
                                {
                                    case "Week":
                                        string DayofWeek = Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date_1"].ToString()).DayOfWeek.ToString();
                                        switch (DayofWeek)
                                        {
                                            case "Monday":
                                                DayofWeek = "1";
                                                break;
                                            case "Tuesday":
                                                DayofWeek = "2";
                                                break;
                                            case "Wednesday":
                                                DayofWeek = "3";
                                                break;
                                            case "Thursday":
                                                DayofWeek = "4";
                                                break;
                                            case "Friday":
                                                DayofWeek = "5";
                                                break;
                                            case "Saturday":
                                                DayofWeek = "6";
                                                break;
                                            case "Sunday":
                                                DayofWeek = "7";
                                                break;

                                        }
                                        defaultShift = new DataView(defaultShift, "Cycle_Day='" + DayofWeek.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                        break;
                                    case "Month":
                                        string strMonthDay = Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date_1"].ToString()).Day.ToString();

                                        defaultShift = new DataView(defaultShift, "Cycle_Day='" + strMonthDay.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


                                        break;
                                    case "Day":
                                        defaultShift = new DataView(defaultShift, "Cycle_Day='1'", "", DataViewRowState.CurrentRows).ToTable();

                                        break;
                                }

                            }

                            if (defaultShift.Rows.Count > 0)
                            {
                                DataTable DtDefaultTime = objShiftManagement.GetShifByTansId(defaultShift.Rows[0]["Trans_Id"].ToString());
                                if (DtDefaultTime.Rows.Count > 0)
                                {
                                    dr1["Date" + (k1 - 1).ToString()] = DtDefaultTime.Rows[DtDefaultTime.Rows.Count - 1]["Shift_Name"].ToString();
                                }
                                else
                                {
                                    dr1["Date" + (k1 - 1).ToString()] = "";
                                }
                            }
                            else
                            {
                                dr1["Date" + (k1 - 1).ToString()] = "Week Off";
                            }

                            IsDataFound = true;

                        }
                        else
                        {
                            dr1["Date" + (k1 - 1).ToString()] = "";
                        }

                        j1++;
                    }
                    k1++;
                }

                if (IsDataFound)
                {
                    dtTime.Rows.Add(dr1);
                }

            }
        }
        Map(dtTime);
        lblTitle.Text = "Employee Shift Schedule Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat());


    }



    public void Map(System.Data.DataTable
     dataTable)
    {
        int i = 0;
        foreach (System.Data.DataRow dataRow in dataTable.Rows)
        {
            TableRow webRow = new TableRow();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                System.Web.UI.WebControls.TableCell webCell = new
                System.Web.UI.WebControls.TableCell();
                Label label = new Label();
                string colname = dataColumn.ColumnName;
                label.Text = dataRow[colname].ToString();
                //if (colname == "EmpName")
                //{
                label.Style.Add(HtmlTextWriterStyle.OverflowX, "hidden");
                //}
                //else
                //{
                //    label.Width = 90;
                //}
                if (label.Text == "Week Off")
                {
                    webCell.BackColor = System.Drawing.Color.SeaGreen;
                    label.Width = 25;
                }
                if (label.Text == "Holiday")
                {
                    webCell.BackColor = System.Drawing.Color.Red;
                }
                if (label.Text == "Leave")
                {
                    webCell.BackColor = System.Drawing.Color.Yellow;
                }
                if (i == 0)
                {
                    webCell.BackColor = System.Drawing.Color.SkyBlue;
                }
                webCell.Controls.Add(label);
                webRow.Cells.Add(webCell);
            }
            i++;
            Table1.Rows.Add(webRow);
        }
    }
    protected void btnExportPdf_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandArgument.ToString() == "1")
        {
            //GetReport();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=ShiftScheduleReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            ExportPanel1.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            //Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
            Document pdfDoc = new Document(PageSize.A2, 0f, 0f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();

            //btnExportPdf.Visible = false;
            //btnExportToExcel.Visible = false;
            //ExportPanel1.OpenInBrowser = true;
            //ExportPanel1.FileName = "Shiftschedulereport";
            //ExportPanel1.ExportType = ControlFreak.ExportPanel.AppType.Word;
        }
        else if (e.CommandArgument.ToString() == "2")
        {
            //GetReport();
            btnExportPdf.Visible = false;
            btnExportToExcel.Visible = false;
            ExportPanel1.OpenInBrowser = true;
            ExportPanel1.FileName = "Shiftschedulereport";
            ExportPanel1.ExportType = ControlFreak.ExportPanel.AppType.Excel;
        }
    }
}