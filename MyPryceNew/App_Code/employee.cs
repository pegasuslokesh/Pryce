using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PegasusDataAccess;
using System.Data;

/// <summary>
/// Summary description for employee
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class employee : System.Web.Services.WebService
{
    DataAccessClass objDA = null;

    public employee(string strConString)
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
        objDA = new DataAccessClass(strConString);
    }

    
    [WebMethod]
    public bool employeeValidation(string empName,string empCode)
    {        
        string count = objDA.get_SingleValue("select count(emp_id) from set_employeemaster where emp_name='"+ empName + "' and emp_code='"+ empCode + "'");
        if (count != "0" || count == "@NOTFOUND@")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
