using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using PegasusDataAccess;
/// <summary>
/// Summary description for CustomizedReport
/// </summary>
public class CustomizedReport
{
    DataAccessClass daClass = null;

    public CustomizedReport(string strConString)
    {
        daClass = new DataAccessClass(strConString);
    }

    public DataTable getAllActiveReports()
    {
        DataTable dt_report = daClass.return_DataTable("select * from sys_reports where isactive='true'");
        return dt_report;
    }
    public DataTable getReportsById(string ReportId)
    {
        DataTable dt_report = daClass.return_DataTable("select * from sys_reports where isactive='true' and reportid='"+ReportId+"'");
        return dt_report;
    }
    public void setisActiveFalse(string reportId)
    {
        daClass.execute_Command("update sys_reports set isactive='false' where reportid='" + reportId + "'");
    }
}