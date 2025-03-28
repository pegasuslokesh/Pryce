<%@ WebService Language="C#" Class="master" %>

using System.Web.Services;
using PegasusDataAccess;
using System.Data;
using System.Web;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class master : System.Web.Services.WebService
{

    [WebMethod(enableSession: true)]
    public bool validateDesignation(string designationName, string designationId)
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string count = objDA.get_SingleValue("select count(Designation_Id) from set_designationmaster where Designation_Id= '" + designationId + "' and Designation= '" + designationName + "' and IsActive='true'");
        if (count != "0" || count == "@NOTFOUND@")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    [WebMethod(enableSession: true)]
    public bool validateDepartment(string departmentName, string departmentId)
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string count = objDA.get_SingleValue("select count(Dep_Id) from set_departmentmaster where Dep_Id= '" + departmentId + "' and Dep_name= '" + departmentName + "' and IsActive='true'");
        if (count != "0" || count == "@NOTFOUND@")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    [WebMethod(enableSession: true)]
    public bool validateCountry(string Name)
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string count = objDA.get_SingleValue("Select count(Country_Id) as CountryCount  from Sys_CountryMaster where Country_Name = '" + Name + "'");
        if (count != "0" || count == "@NOTFOUND@")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [WebMethod(enableSession: true)]
    public string getDocumentNo(string LocationId, string PageUrl)
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        Set_DocNumber ObjDoc = new Set_DocNumber(HttpContext.Current.Session["DBConnection"].ToString());
        string sql = "select top 1 IT_App_Mod_Object.module_id,IT_App_Mod_Object.object_id from IT_App_Mod_Object inner join IT_ObjectEntry on IT_ObjectEntry.Object_Id=IT_App_Mod_Object.Object_Id where application_id=" + Session["Application_Id"].ToString() + " and IT_ObjectEntry.Page_Url='" + PageUrl + "'";
        using (DataTable dtModObject = objDA.return_DataTable(sql))
        {
            if (dtModObject.Rows.Count > 0)
            {
                string docNo = ObjDoc.GetDocumentNo(true, Session["CompId"].ToString(), LocationId, true, dtModObject.Rows[0]["module_id"].ToString(), dtModObject.Rows[0]["object_id"].ToString(), "0",HttpContext.Current.Session["LocId"].ToString(),"0", HttpContext.Current.Session["EmpId"].ToString(),HttpContext.Current.Session["TimeZoneId"].ToString());
                return docNo;
            }
            else
            {
                return "";
            }
        }
    }

    [WebMethod(enableSession: true)]
    public string getDocumentNoByModuleNObjectID(string LocationId, string moduleId, string objectId)
    {
        Set_DocNumber ObjDoc = new Set_DocNumber(HttpContext.Current.Session["DBConnection"].ToString());
        string docNo = ObjDoc.GetDocumentNo(true, Session["CompId"].ToString(), LocationId, true, moduleId, objectId, "0", HttpContext.Current.Session["LocId"].ToString(),"0", HttpContext.Current.Session["EmpId"].ToString(),HttpContext.Current.Session["TimeZoneId"].ToString());
        return docNo;
    }

    [WebMethod(enableSession: true)]
    public string[] getObjectIdNModuleIDByURL(string url)
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string[] data = new string[2];
        string sql = "select top 1 IT_App_Mod_Object.module_id,IT_App_Mod_Object.object_id from IT_App_Mod_Object inner join IT_ObjectEntry on IT_ObjectEntry.Object_Id=IT_App_Mod_Object.Object_Id where application_id=" + Session["Application_Id"].ToString() + " and IT_ObjectEntry.Page_Url='" + url + "'";
        DataTable dtModObject = objDA.return_DataTable(sql);
        if (dtModObject.Rows.Count > 0)
        {
            data[0] = dtModObject.Rows[0]["module_id"].ToString();
            data[1] = dtModObject.Rows[0]["object_id"].ToString();
        }
        else
        {
            data[0] = "0";
            data[1] = "0";
        }
        return data;
    }
}