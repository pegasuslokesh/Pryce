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

public partial class Attendance_Report_AccessGroupSummary : BasePage
{
    Att_Leave_Request objleaveReq = null;
    Set_ApplicationParameter objAppParam = null;
    SystemParameter ObjSys = null;
    Att_AttendanceRegister ObjRegister = null;
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

        if (!IsPostBack)
        {
           
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../Attendance_Report/AccessGroupSummary.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

                Session["DtShortCode_AcGroup"] = null;

                DataTable dtColorCode = new DataTable();
                dtColorCode.Columns.Add("Type");
                dtColorCode.Columns.Add("ShortId");
                dtColorCode.Columns.Add("ColorCode");

                DataRow dr1 = dtColorCode.NewRow();
                dr1["Type"] = "Absent";
                dr1["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                lblabsent.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr1["ColorCode"].ToString()).ToString());


                dr1["ShortId"] = "AB";
                dtColorCode.Rows.Add(dr1);

                DataRow dr2 = dtColorCode.NewRow();
                dr2["Type"] = "Normal";
                dr2["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                lblPresent.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr2["ColorCode"].ToString()).ToString());
                dr2["ShortId"] = "N";
                dtColorCode.Rows.Add(dr2);


                DataRow dr3 = dtColorCode.NewRow();
                dr3["Type"] = "Holiday";
                dr3["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                lblHoliday.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr3["ColorCode"].ToString()).ToString());
                dr3["ShortId"] = "PH";
                dtColorCode.Rows.Add(dr3);

                DataRow dr4 = dtColorCode.NewRow();
                dr4["Type"] = "Leave";
                dr4["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                lblleave.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr4["ColorCode"].ToString()).ToString());
                dr4["ShortId"] = "L";
                dtColorCode.Rows.Add(dr4);


                DataRow dr5 = dtColorCode.NewRow();

                dr5["Type"] = "WeekOff";
                dr5["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                lblweekoff.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr5["ColorCode"].ToString()).ToString());
                dr5["ShortId"] = "WO";
                dtColorCode.Rows.Add(dr5);


                DataRow dr6 = dtColorCode.NewRow();
                dr6["Type"] = "Missed Out";
                dr6["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("MO_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                lblMissedout.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr6["ColorCode"].ToString()).ToString());
                dr6["ShortId"] = "MO";
                dtColorCode.Rows.Add(dr6);



                DataRow dr7 = dtColorCode.NewRow();
                dr7["Type"] = "Missed In";
                dr7["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("MI_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                lblMissedIn.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr7["ColorCode"].ToString()).ToString());
                dr7["ShortId"] = "MI";
                dtColorCode.Rows.Add(dr7);



                DataRow dr8 = dtColorCode.NewRow();
                dr8["Type"] = "Late Check IN";
                dr8["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Late_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                lbllatein.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr8["ColorCode"].ToString()).ToString());
                dr8["ShortId"] = "";
                dtColorCode.Rows.Add(dr8);


                DataRow dr9 = dtColorCode.NewRow();
                dr9["Type"] = "Early Check Out";
                dr9["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Early_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                lblearlyout.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dr9["ColorCode"].ToString()).ToString());
                dr9["ShortId"] = "";
                dtColorCode.Rows.Add(dr9);



                Session["DtShortCode_AcGroup"] = dtColorCode;



            }

        }

        GetReport();

    }


    public void GetReport()
    {
        int Counter = 0;
        if (Session["SelectedEmpId"] == null)
        {
            return;
        }

        Table1.Rows.Clear();

        DataTable dtShortCode = (DataTable)Session["DtShortCode_AcGroup"];


        //set color and short code
        string Normal = "P";
        string Absent = (new DataView(dtShortCode, "Type='Absent'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();

        string WeekOff = (new DataView(dtShortCode, "Type='WeekOff'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string Holiday = (new DataView(dtShortCode, "Type='Holiday'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string MIssedIn = (new DataView(dtShortCode, "Type='Missed In'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string MIssedout = (new DataView(dtShortCode, "Type='Missed Out'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string LateIn = (new DataView(dtShortCode, "Type='Late Check IN'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string Earlyout = (new DataView(dtShortCode, "Type='Early Check Out'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();

        string Leave = "L";
        string Normalcol = (new DataView(dtShortCode, "Type='Normal'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string Absentcol = (new DataView(dtShortCode, "Type='Absent'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();

        string Leavecol = (new DataView(dtShortCode, "Type='Leave'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string WeekOffcol = (new DataView(dtShortCode, "Type='WeekOff'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string Holidaycol = (new DataView(dtShortCode, "Type='Holiday'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string MIssedIncol = (new DataView(dtShortCode, "Type='Missed In'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string MIssedoutcol = (new DataView(dtShortCode, "Type='Missed Out'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string LateIncol = (new DataView(dtShortCode, "Type='Late Check IN'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string Earlyoutcol = (new DataView(dtShortCode, "Type='Early Check Out'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();


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
        DataTable dtFilter = ObjRegister.GetAttendanceReport(Session["SelectedEmpId"].ToString(), Convert.ToDateTime(Session["Report_FromDate"].ToString()).ToString(), Convert.ToDateTime(Session["Report_ToDate"].ToString()).ToString(), "11");



        TableRow tr = new TableRow();

        TableCell tcSNO = new TableCell();
        tcSNO.Text = "SNO";
        tcSNO.Font.Bold = true;
        tr.Cells.Add(tcSNO);
        TableCell tcId = new TableCell();
        tcId.Wrap = false;
        tcId.Text = "Employee#";
        tcId.Font.Bold = true;
        tr.Cells.Add(tcId);
        TableCell tcName = new TableCell();
        tcName.Wrap = false;
        tcName.Text = "Name";
        tcName.Font.Bold = true;
        tr.Cells.Add(tcName);
        TableCell tcLoc = new TableCell();
        tcLoc.Wrap = false;
        tcLoc.Text = "Location";
        tcLoc.Font.Bold = true;
        tr.Cells.Add(tcLoc);
        TableCell tcDept = new TableCell();
        tcDept.Wrap = false;
        tcDept.Text = "Department";
        tcDept.Font.Bold = true;
        tr.Cells.Add(tcDept);
        TableCell tcDesg = new TableCell();
        tcDesg.Wrap = false;
        tcDesg.Text = "Designation";
        tcDesg.Font.Bold = true;
        tr.Cells.Add(tcDesg);


        int count = 1;


        DateTime dtFromdate = Convert.ToDateTime(Session["Report_FromDate"].ToString());
        DateTime dtTodate = Convert.ToDateTime(Session["Report_ToDate"].ToString());




        while (dtFromdate <= dtTodate)
        {
            TableCell tcDay = new TableCell();
            tcDay.Text = dtFromdate.ToString(ObjSys.SetDateFormat());
            tcDay.Wrap = false;
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

                DateTime dtFromTemp = Convert.ToDateTime(Session["Report_FromDate"].ToString());
                DateTime dtToTemp = Convert.ToDateTime(Session["Report_ToDate"].ToString());


                int maxshift = 0;
                while (dtFromTemp < dtToTemp)
                {

                    dtFromTemp = dtFromTemp.AddDays(1);
                    DataTable dtTempDateRecordEmp = new DataView(dtAttbyEmpId, "Att_Date = '" + dtFromTemp.ToString("dd-MMM-yyyy") + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

                    if (maxshift < dtTempDateRecordEmp.Rows.Count)
                    {
                        maxshift = dtTempDateRecordEmp.Rows.Count;
                    }
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
                TableCell tcLOcValue = new TableCell();
                TableCell tcdeptValue = new TableCell();
                TableCell tcdesgValue = new TableCell();
                tcSNOEmp.Text = (empCounter + 1).ToString();
                if (maxshift > 0)
                {

                    trEmp[0].Cells.Add(tcSNOEmp);


                    tcNameEmp1.Text = dtAttbyEmpId.Rows[1]["Emp_Code"].ToString();

                    trEmp[0].Cells.Add(tcNameEmp1);


                    tcNameEmp.Text = dtAttbyEmpId.Rows[0]["Emp_Name"].ToString();
                    tcNameEmp.Wrap = false;
                    trEmp[0].Cells.Add(tcNameEmp);

                    //for location


                    tcLOcValue.Text = dtAttbyEmpId.Rows[0]["Location_Name"].ToString();
                    tcLOcValue.Wrap = false;
                    trEmp[0].Cells.Add(tcLOcValue);
                    //for department

                    tcdeptValue.Text = dtAttbyEmpId.Rows[0]["dep_name"].ToString();
                    tcdeptValue.Wrap = false;
                    trEmp[0].Cells.Add(tcdeptValue);
                    //for designation

                    tcdesgValue.Text = dtAttbyEmpId.Rows[0]["Designation"].ToString();
                    tcdesgValue.Wrap = false;
                    trEmp[0].Cells.Add(tcdesgValue);
                }
                int presentcount = 0;

                dtFromdate = Convert.ToDateTime(Session["Report_FromDate"].ToString());
                dtTodate = Convert.ToDateTime(Session["Report_ToDate"].ToString());

                while (dtFromdate <= dtTodate)
                {
                    int shiftCounter = 0;
                    DataTable dtTempDateRecordEmp = new DataView(dtAttbyEmpId, "Att_Date = '" + dtFromdate.ToString("dd-MMM-yyyy") + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

                    while (shiftCounter < maxshift)
                    {
                        TableCell tcDay = new TableCell();

                        tcDay.Style["Text-align"] = "Center";
                        string attEmp = string.Empty;

                        if (shiftCounter < dtTempDateRecordEmp.Rows.Count)
                        {
                            if ((Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Week_Off"].ToString())))
                            {

                                if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Week_Off_Min"].ToString()) > 0)
                                {
                                    string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Week_Off_Min"].ToString());
                                    attEmp = attEmp + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M";
                                }
                                else
                                {
                                    attEmp = attEmp + WeekOff;
                                }


                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(WeekOffcol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }
                            else if ((Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Holiday"].ToString())))
                            {

                                if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["Holiday_Min"].ToString()) > 0)
                                {
                                    string strTime = GetHours(dtTempDateRecordEmp.Rows[shiftCounter]["Holiday_Min"].ToString());
                                    attEmp = attEmp + strTime.Split(':')[0] + "H" + ":" + strTime.Split(':')[1] + "M";
                                }
                                else
                                {
                                    attEmp = attEmp + Holiday;
                                }

                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Holidaycol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }
                            else if (Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Leave"].ToString()))
                            {
                                if (dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString().Trim() != "")
                                {
                                    //DataTable DtLeave = objleaveReq.GetLeaveRequestById(Session["CompId"].ToString(), dtTempDateRecordEmp.Rows[shiftCounter]["Emp_Id"].ToString());
                                    //DtLeave = new DataView(DtLeave, "From_Date <='" + dtFromdate.ToString() + "' and To_Date>='" + dtFromdate.ToString() + "' and Is_Approved='True'", "", DataViewRowState.CurrentRows).ToTable();
                                    //if (DtLeave.Rows.Count > 0)
                                    //{
                                    //    //if(DtLeave.Rows[0]["Leave_Name"].ToString().Length>11)
                                    //    //{
                                    //    //    dtFilter.Rows[k]["Field1"] = DtLeave.Rows[0]["Leave_Name"].ToString().Substring(0, 9) + "..";
                                    //    //}
                                    //    //else
                                    //    //{
                                    attEmp = attEmp + dtTempDateRecordEmp.Rows[shiftCounter]["Leave_Name"].ToString();
                                    //}

                                }
                                else
                                {
                                    attEmp = attEmp + "Leave";
                                }

                                //attEmp = attEmp + Leave;
                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Leavecol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }
                            else if (Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Absent"].ToString()))
                            {

                                attEmp = attEmp + Absent;

                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Absentcol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }

                            }

                            else if (dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() == "00:00" && dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() != "00:00")
                            {
                                attEmp = attEmp + MIssedIn;

                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(MIssedIncol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }
                            else if (dtTempDateRecordEmp.Rows[shiftCounter]["In_Time"].ToString() != "00:00" && dtTempDateRecordEmp.Rows[shiftCounter]["Out_Time"].ToString() == "00:00")
                            {
                                attEmp = attEmp + MIssedout;

                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(MIssedoutcol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }



                            else if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["LateMin"].ToString()) > 0)
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


                                //attEmp = attEmp + LateIn;
                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(LateIncol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                                presentcount++;
                            }
                            else if (Convert.ToInt32(dtTempDateRecordEmp.Rows[shiftCounter]["EarlyMin"].ToString()) > 0)
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

                                //attEmp = attEmp + Earlyout;
                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Earlyoutcol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                                presentcount++;
                            }
                            else
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

                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Normalcol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                                presentcount++;
                            }
                            absent[shiftCounter] = absent[shiftCounter] + 1;
                        }
                        else
                        {
                            attEmp = "-";
                        }


                        //for future date it will not show as absent

                        if (dtFromdate.Date > Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
                        {
                            attEmp = "";
                            tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                        }


                        tcDay.Text = attEmp;
                        trEmp[shiftCounter].Cells.Add(tcDay);
                        trEmp[shiftCounter].Cells.Add(tcDay);

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
                        trEmp[maxcounter].Cells.AddAt(0, new TableCell());
                        TableCell EmPCode = new TableCell();
                        EmPCode.Text = tcNameEmp1.Text;
                        TableCell EmPName = new TableCell();
                        EmPName.Text = tcNameEmp.Text;
                        TableCell LocName = new TableCell();
                        LocName.Text = tcLOcValue.Text;
                        TableCell DeptName = new TableCell();
                        DeptName.Text = tcdeptValue.Text;
                        TableCell DesgName = new TableCell();
                        DesgName.Text = tcdesgValue.Text;
                        trEmp[maxcounter].Cells.AddAt(1, EmPCode);
                        trEmp[maxcounter].Cells.AddAt(2, EmPName);
                        trEmp[maxcounter].Cells.AddAt(3, LocName);
                        trEmp[maxcounter].Cells.AddAt(4, DeptName);
                        trEmp[maxcounter].Cells.AddAt(5, DesgName);
                    }

                    Table1.Rows.Add(trEmp[maxcounter - 1]);
                }




                empCounter++;
            }

        }
        //lblmonthname.Text = "Month " + ": " + ddlMonth.SelectedItem.Text;

        //here we are added code for show color code



        TableRow trcolor = new TableRow();

        TableCell tcWo = new TableCell();
        tcWo.Text = "WO-Week OFF";
        tcWo.ColumnSpan = 2;
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
        tcPH.ColumnSpan = 2;
        trcolor.Cells.Add(tcPH);
        try
        {
            tcPH.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Holidaycol).ToString());
        }
        catch
        {
            tcPH.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }


        TableCell tcLeave = new TableCell();
        tcLeave.Text = "Leave";
        tcLeave.Style["Text-align"] = "Center";
        tcLeave.ColumnSpan = 2;
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






        TableCell tcMI = new TableCell();
        tcMI.Text = "MI-Missed IN";
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

      

        TableCell tcLate = new TableCell();
        tcLate.Text = "Late Check IN";
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
        tcearly.Style["Text-align"] = "Center";
        tcearly.ColumnSpan = 2;
        trcolor.Cells.Add(tcearly);
        try
        {
            tcearly.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Earlyoutcol).ToString());
        }
        catch
        {
            tcearly.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }


        TableCell tcPresent = new TableCell();
        tcPresent.Text = "Present";
        tcPresent.Style["Text-align"] = "Center";
        tcPresent.ColumnSpan = 2;
        trcolor.Cells.Add(tcPresent);
        try
        {
            tcPresent.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Normalcol).ToString());
        }
        catch
        {
            tcPresent.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
        }


        tblColorCode.Rows.Add(trcolor);


        

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


    private static List<string> GetContents(string input, string pattern)

    {

        MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.Singleline);

        List<string> contents = new List<string>();

        foreach (Match match in matches)

            contents.Add(match.Value);

        return contents;

    }



    protected void btnExportPdf_Command(object sender, CommandEventArgs e)
    {
        WebClient wc = new WebClient();

        string url = Request.Url.AbsoluteUri;

        string fileContent = wc.DownloadString(url);

        List<string> tableContents = GetContents(fileContent, Table1.ToString());

        string HTMLString = String.Join(" ", tableContents.ToArray());

        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

        pdfDoc.Open();

        pdfDoc.Add(new Paragraph("Welcome to dotnetfox"));

        List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(HTMLString), null);

        for (int k = 0; k < htmlarraylist.Count; k++)

        {

            pdfDoc.Add((IElement)htmlarraylist[k]);

        }

        pdfDoc.Close();

        Response.ContentType = "application/pdf";

        Response.AddHeader("content-disposition", "attachment;" +

                                       "filename=sample.pdf");

        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.Write(pdfDoc);

        Response.End();

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
        //GetReport();
        int rowcount = Table1.Rows.Count;

        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename = AccessGroupSummary " + DateTime.UtcNow.ToString() + ".xls");
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


}