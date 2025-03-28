<%@ WebService Language="C#" Class="employee" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using PegasusDataAccess;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class employee : System.Web.Services.WebService
{
    
    [WebMethod(enableSession: true)]
    public bool employeeValidation(string empName, string empCode)
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string count = objDA.get_SingleValue("select count(emp_id) from set_employeemaster where emp_name='" + empName + "' and emp_code='" + empCode + "'");
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
    public bool employeeValidationWithEmpId(string empName, string empId)
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string count = objDA.get_SingleValue("select count(emp_id) from set_employeemaster where emp_name='" + empName + "' and emp_Id='" + empId + "'");
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
    public string ValidationEmployeeGetEmpID(string empName, string empCode)
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string empID = "";
        empID = objDA.get_SingleValue("select emp_id from set_employeemaster where emp_name='" + empName + "' and emp_Code='" + empCode + "'");
        empID = empID == "@NOTFOUND@" ? "" : empID;
        return empID;
    }
         [WebMethod(enableSession: true)]
    public string getDefaultUserName()
    {
        DataAccessClass objDA = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string empName = "";
        empName = objDA.get_SingleValue("select Emp_Name+'/'+emp_code as empName from set_employeemaster where emp_id='"+HttpContext.Current.Session["EmpId"].ToString()+"'");
        empName = empName == "@NOTFOUND@" ? "" : empName;
        return empName;
    }
}