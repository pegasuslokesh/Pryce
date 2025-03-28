<%@ WebService Language="C#" Class="projectManagement" %>

using System;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Web.Services.Protocols;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class projectManagement : System.Web.Services.WebService
{



    [WebMethod(enableSession: true)]
    public string saveNewBug(Prj_ProjectTask.ProjectBug pbData)
    {
        int task_id = new Prj_ProjectTask(HttpContext.Current.Session["DBConnection"].ToString()).InsertProjectTask(pbData.projectId, "0", System.DateTime.Now.ToString("dd-MMM-yyyy"), "1/1/1900", System.DateTime.Now.ToString("dd-MMM-yyyy"), "1/1/1900", "1/1/1900", "1/1/1900", pbData.bugName, pbData.description, "0", "Assigned", "", "", "", pbData.priority, "0", "2", "0.00", true.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), "Internal", "0", "0", "0.00", "", pbData.assignedBy);
        return task_id.ToString();
    }

    [WebMethod(enableSession: true)]
    public string[] GetCompletionListProject(string prefixText, int count, string contextKey)
    {
        DataTable dtCon = new DataTable();
        try
        {
            using (dtCon = new Prj_ProjectTeam(HttpContext.Current.Session["DBConnection"].ToString()).GetAllProjectNamePreText("0", "", prefixText))
            {
                string[] filterlist = new string[dtCon.Rows.Count];
                if (dtCon.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCon.Rows.Count; i++)
                    {
                        filterlist[i] = dtCon.Rows[i]["Project_Name"].ToString();
                    }
                }
                return filterlist;
            }

        }
        catch
        {
            return null;
        }
    }
    [WebMethod(enableSession: true)]
    public string[] GetCompletionListAssignBy(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DataTable();
        EmployeeMaster em = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        dt = em.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), "0", prefixText);
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Emp_name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString();
            }
        }
        return str;
    }
    [WebMethod(enableSession: true)]
    public string txtProjectName_TextChanged(string projectName)
    {
        string projectId = new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetProjectIdByName(projectName);
        if (projectId == "")
        {
            return "0";
        }
        else
        {
            return projectId;
        }
    }
}