<%@ WebService Language="C#" Class="reportSystem" %>

using System.Web;
using System.Web.Services;
using PegasusDataAccess;
using Newtonsoft.Json;
using System.Data;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class reportSystem : System.Web.Services.WebService
{
    [WebMethod(enableSession: true)]
    public string getReportData(string moduleId, string objectId)
    {
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        if (moduleId == "144" || moduleId == "143" || moduleId == "142")
        {
            dt = objDa.return_DataTable("select distinct sys_reports.ReportId,sys_reports.DisplayName,sys_reports.ModuleId,sys_reports.objectId,case when sys_reportsdetail.isdefault is null then '' else case when sys_reportsdetail.isdefault='true' then 'checked' else '' end end as isdefault from sys_reports left join sys_reportsdetail on sys_reportsdetail.reportid=sys_reports.ReportId and sys_reportsdetail.location_id='" + Session["LocID"].ToString() + "' where (sys_reports.moduleid='" + moduleId + "' and sys_reports.objectid='" + objectId + "') or sys_reports.moduleid='176'  and sys_reports.isactive='true'");
        }
        else
        {
            dt = objDa.return_DataTable("select distinct sys_reports.ReportId,sys_reports.DisplayName,sys_reports.ModuleId,sys_reports.objectId,case when sys_reportsdetail.isdefault is null then '' else case when sys_reportsdetail.isdefault='true' then 'checked' else '' end end as isdefault from sys_reports left join sys_reportsdetail on sys_reportsdetail.reportid=sys_reports.ReportId and sys_reportsdetail.location_id='" + Session["LocID"].ToString() + "' where sys_reports.moduleid='" + moduleId + "' and sys_reports.objectid='" + objectId + "' and sys_reports.isactive='true'");
        }

        string jsonData = "";

        if (dt.Rows.Count > 0)
        {
            jsonData = JsonConvert.SerializeObject(dt);
        }

        return jsonData;
    }

    [WebMethod(enableSession: true)]
    public string setDefaultReport(string check, string reportId, string moduleId, string objectId, string removeDefault)
    {
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());

        if (check.ToLower() == "true")
        {
            objDa.execute_Command("insert into sys_reportsDetail values ('" + HttpContext.Current.Session["LocId"].ToString() + "','" + reportId + "','" + moduleId + "','" + objectId + "','" + check + "')");
        }

        if (removeDefault != "")
        {
            objDa.execute_Command("delete from sys_reportsDetail where location_id = '" + HttpContext.Current.Session["LocId"].ToString() + "' and reportid = '" + removeDefault + "' and moduleid = '" + moduleId + "' and objectid = '" + objectId + "'");
        }
        return "true";
    }

}