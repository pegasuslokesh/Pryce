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
using System.Data.SqlClient;
using PegasusDataAccess;

/// <summary>
/// Summary description for Att_AttendanceLog
/// </summary>
public class Att_inOutReport_Direct
{
    Set_ApplicationParameter objAppParam = null;
    Att_InOutReport RptShift = null;
    Att_InOutReport_1_Direct RptShift_Seperate = null;
    Attendance objAttendance = null;
    SystemParameter objSys = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    BrandMaster objBrand = null;
    DepartmentMaster objDept = null;
    LocationMaster objLoc = null;
    LeaveMaster objLeave = null;
    Att_Employee_Notification objEmpNotice = null;
    Att_Leave_Request objleaveReq = null;
    //Boolean isPdfUrlOnly = false;
    string tmpCompanyId = string.Empty;
    string tmpBrandId = string.Empty;
    string tmpLocationId = string.Empty;
    string tmpFromDate = string.Empty;
    string tmpToDate = string.Empty;
    string tmpEmpList = string.Empty;
    string tmpFileName = string.Empty;

    public Att_inOutReport_Direct(string strConString)
    {
        ////isPdfUrlOnly = true;
        //tmpCompanyId = strCompanyId;
        //tmpBrandId = strBrandId;
        //tmpLocationId = strLocationId;
        //tmpFromDate = strFromDate;
        //tmpToDate = strToDate;
        //tmpEmpList = strEmpList;
        objAppParam = new Set_ApplicationParameter(strConString);
        RptShift = new Att_InOutReport(strConString);
        RptShift_Seperate = new Att_InOutReport_1_Direct(strConString);
        objAttendance = new Attendance(strConString);
        objSys = new SystemParameter(strConString);
        objComp = new CompanyMaster(strConString);
        ObjAddress = new Set_AddressChild(strConString);
        objBrand = new BrandMaster(strConString);
        objDept = new DepartmentMaster(strConString);
        objLoc = new LocationMaster(strConString);
        objLeave = new LeaveMaster(strConString);
        objEmpNotice = new Att_Employee_Notification(strConString);
        objleaveReq = new Att_Leave_Request(strConString);
    }
    public DevExpress.XtraReports.UI.XtraReport GetReport(string strCompanyId, string strBrandId, string strLocationId, string strEmpList, string strFromDate, string strToDate, string rptType, string strLang, string strUserId)
    {

        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();

        string EmpReport = string.Empty;

        FromDate = objSys.getDateForInput(strFromDate);
        ToDate = objSys.getDateForInput(strToDate);

        //this code is created by jitendra upadhyay on 19-09-2014
        //this code for employee should not be showing in repot if not exists in employee notification for in out report
        //code star
        //code update on 27-09-2014

        //DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(EmpReport, "11");

        //foreach (DataRow dr in dtEmpNF.Rows)
        //{
        //    Emplist += dr["Emp_Id"] + ",";
        //}
        //code end

        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();

        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();
        adp.Connection.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();

        try
        {
            adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), strEmpList, 17);
        }
        catch (Exception ex)
        {

        }

        dtFilter = rptdata.sp_Att_AttendanceRegister_Report;

        // dtFilter = new DataView(dtFilter, "", "Emp_Id ASC", DataViewRowState.CurrentRows).ToTable();

        //if (Emplist != "")
        //{
        //    dtFilter = new DataView(rptdata.sp_Att_AttendanceRegister_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();
        //}

        //for (int k = 0; k < dtFilter.Rows.Count; k++)
        //{


        //    if (Convert.ToBoolean(dtFilter.Rows[k]["Is_TempShift"].ToString()))
        //    {

        //        dtFilter.Rows[k]["Shift_Name"] = "Temp Shift";

        //        if (objAppParam.GetApplicationParameterValueByParamName("TempShift_Color_Code", strCompanyId, strBrandId, strLocationId) == "")
        //        {
        //            dtFilter.Rows[k]["Field2"] = "ffffff";
        //        }
        //        else
        //        {
        //            dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("TempShift_Color_Code", strCompanyId, strBrandId, strLocationId);
        //        }
        //        //dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("TempShift_Color_Code",strCompanyId);
        //    }

        //    if (Convert.ToBoolean(dtFilter.Rows[k]["Is_Week_Off"].ToString()))
        //    {
        //        dtFilter.Rows[k]["Field1"] = "Off";
        //        dtFilter.Rows[k]["OverTime_Min"] = dtFilter.Rows[k]["Week_Off_Min"];
        //        if (objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", strCompanyId, strBrandId, strLocationId) == "")
        //        {
        //            dtFilter.Rows[k]["Field2"] = "ffffff";
        //        }
        //        else
        //        {
        //            dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", strCompanyId, strBrandId, strLocationId);
        //        }
        //        // dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", strCompanyId);
        //    }
        //    else if (Convert.ToBoolean(dtFilter.Rows[k]["Is_Holiday"].ToString()))
        //    {
        //        dtFilter.Rows[k]["Field1"] = "Holiday";
        //        dtFilter.Rows[k]["OverTime_Min"] = dtFilter.Rows[k]["Holiday_Min"];
        //        if (objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", strCompanyId, strBrandId, strLocationId) == "")
        //        {
        //            dtFilter.Rows[k]["Field2"] = "ffffff";
        //        }
        //        else
        //        {
        //            dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", strCompanyId, strBrandId, strLocationId);
        //        }
        //        // dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", strCompanyId);

        //    }
        //    else if (Convert.ToBoolean(dtFilter.Rows[k]["Is_Leave"].ToString()))
        //    {
        //        DateTime dtDate = DateTime.Parse(dtFilter.Rows[k]["Att_Date"].ToString());
        //        string strEmployeeId = dtFilter.Rows[k]["Emp_Id"].ToString();

        //        DataTable DtLeave = objleaveReq.GetLeaveRequestById(strCompanyId, strEmployeeId);
        //        DtLeave = new DataView(DtLeave, "From_Date <='" + dtDate.ToString() + "' and To_Date>='" + dtDate.ToString() + "' and Is_Approved='True'", "", DataViewRowState.CurrentRows).ToTable();
        //        if (DtLeave.Rows.Count > 0)
        //        {
        //            dtFilter.Rows[k]["Field1"] = DtLeave.Rows[0]["Leave_Name"].ToString();
        //        }
        //        else
        //        {
        //            dtFilter.Rows[k]["Field1"] = "Leave";
        //        }

        //        if (objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", strCompanyId, strBrandId, strLocationId) == "")
        //        {
        //            dtFilter.Rows[k]["Field2"] = "ffffff";
        //        }
        //        else
        //        {
        //            dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", strCompanyId, strBrandId, strLocationId);
        //        }
        //        //dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", strCompanyId);


        //    }
        //    else if (Convert.ToBoolean(dtFilter.Rows[k]["Is_Absent"].ToString()))
        //    {
        //        dtFilter.Rows[k]["Field1"] = "Absent";
        //        if (objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", strCompanyId, strBrandId, strLocationId) == "")
        //        {
        //            dtFilter.Rows[k]["Field2"] = "ffffff";
        //        }
        //        else
        //        {
        //            dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", strCompanyId, strBrandId, strLocationId);
        //        }
        //        // dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", strCompanyId);


        //    }
        //    else
        //    {
        //        dtFilter.Rows[k]["Field1"] = "Present";

        //        if (objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", strCompanyId, strBrandId, strLocationId) == "")
        //        {
        //            dtFilter.Rows[k]["Field2"] = "ffffff";
        //        }
        //        else
        //        {
        //            dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", strCompanyId, strBrandId, strLocationId);
        //        }
        //        //dtFilter.Rows[k]["Field2"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", strCompanyId);
        //    }
        //}

        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string DepartmentName = "";
        // Get Company Name
        CompanyName = objAttendance.GetCompanyName(strCompanyId, strLang, "1");
        // Image Url
        //Imageurl = objAttendance.GetCompanyName(strCompanyId, strLang, "2");

        DataTable DtLocation = objLoc.GetLocationMasterById(strCompanyId, strLocationId);
        if (DtLocation.Rows.Count > 0)
        {
            if (DtLocation.Rows[0]["Field2"].ToString() != "" && DtLocation.Rows[0]["Field2"].ToString() != null)
            {
                Imageurl = "~/CompanyResource/" + strCompanyId + "/" + DtLocation.Rows[0]["Field2"].ToString();
            }
            else
            {
                Imageurl = objAttendance.GetCompanyName(strCompanyId, "2", "2");
            }
        }
        else
        {
            Imageurl = objAttendance.GetCompanyName(strCompanyId, "2", "2");
        }



        // Get Brand Name
        BrandName = objAttendance.GetBrandName(strCompanyId, strBrandId, strLang);
        // Get Location Name

        LocationName = objAttendance.GetLocationName(strCompanyId, strBrandId, strLocationId, strLang);



        DepartmentName = "All";

        // Get Company Address
        DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", strCompanyId);
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
        }

        if (rptType == "0")
        {

            RptShift.SetCompanyId(strCompanyId);
            RptShift.SetImage(Imageurl);
            RptShift.SetBrandName(BrandName);
            RptShift.SetLocationName(LocationName);
            RptShift.SetDepartmentName(DepartmentName);
            RptShift.setTitleName("In Out Report" + " From " + FromDate.ToString("dd-MMM-yyyy") + " To " + ToDate.ToString("dd-MMM-yyyy"));
            RptShift.setcompanyname(CompanyName);
            RptShift.setaddress(CompanyAddress);
            RptShift.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.Shift_Name, Resources.Attendance.On_Duty_Time, Resources.Attendance.Off_Duty_Time, Resources.Attendance.In_Time, Resources.Attendance.Out_Time, Resources.Attendance.Late, Resources.Attendance.Early, Resources.Attendance.Work, Resources.Attendance.Over_Time, Resources.Attendance.Status, Resources.Attendance.Total, Resources.Attendance.Brand, Resources.Attendance.Location, Resources.Attendance.Department);
            RptShift.setUserName(strUserId);
            RptShift.DataSource = dtFilter;
            RptShift.DataMember = "sp_Att_AttendanceRegister_Report";
            return RptShift;
        }
        else
        {

            DataTable dtTemp = new DataTable();

            //here we are making temporary table for show all leave type and count for selected date anbd emnployee ]

            DataTable dtLeave = objLeave.GetLeaveMaster(strCompanyId);

            for (int k = 0; k < dtLeave.Rows.Count; k++)
            {
                dtTemp.Columns.Add(dtLeave.Rows[k]["Leave_Name"].ToString());
            }

            dtTemp.Columns.Add("Holidays");

            RptShift_Seperate.SetDateCriteria(FromDate, ToDate);
            RptShift_Seperate.SetLeaveDetail(dtTemp, dtFilter);
            RptShift_Seperate.SetCompanyId(strCompanyId);
            RptShift_Seperate.SetImage(Imageurl);
            RptShift_Seperate.SetBrandName(BrandName);
            RptShift_Seperate.SetLocationName(LocationName);
            RptShift_Seperate.SetDepartmentName(DepartmentName);
            RptShift_Seperate.setTitleName("In Out Report" + " From " + FromDate.ToString("dd-MMM-yyyy") + " To " + ToDate.ToString("dd-MMM-yyyy"));
            RptShift_Seperate.setcompanyname(CompanyName);
            RptShift_Seperate.setaddress(CompanyAddress);
            RptShift_Seperate.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.Shift_Name, Resources.Attendance.On_Duty_Time, Resources.Attendance.Off_Duty_Time, Resources.Attendance.In_Time, Resources.Attendance.Out_Time, Resources.Attendance.Late, Resources.Attendance.Early, Resources.Attendance.Work, Resources.Attendance.Over_Time, Resources.Attendance.Status, Resources.Attendance.Total, Resources.Attendance.Brand, Resources.Attendance.Location, Resources.Attendance.Department);
            RptShift_Seperate.setUserName(strUserId);
            RptShift_Seperate.DataSource = dtFilter;
            RptShift_Seperate.DataMember = "sp_Att_AttendanceRegister_Report";
            return RptShift_Seperate;
        }
        //if (isPdfUrlOnly)
        //{
        //    RptShift.ExportToPdf("~/temp/" + tmpFileName);
        //}
        //else
        //{
        //    rptToolBar.ReportViewer = rptViewer;
        //}


    }
}
