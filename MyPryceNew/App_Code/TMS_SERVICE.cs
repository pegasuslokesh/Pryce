using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using PegasusDataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Web.Script.Serialization;

/// <summary>
/// Summary description for TMS_SERVICE
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class TMS_SERVICE : System.Web.Services.WebService
{
    DataAccessClass ObjDa = null;
    Att_AttendanceRegister ObjRegister = null;
    public TMS_SERVICE(string strConString)
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
        ObjDa = new DataAccessClass(strConString);
        ObjRegister = new Att_AttendanceRegister(strConString);
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }



    [WebMethod]

    public void GET_TMS_RECORD(string strEmpCodelist, string strLocationName, string strDepartmentName, string strFromDate, string strToDate)
    {
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add(new DataColumn("Error Code"));
        dtResult.Columns.Add(new DataColumn("Error Detail"));


        try
        {

            if (strEmpCodelist == "" && strLocationName == "")
            {
                dtResult.Rows.Add("1", "Enter Employee code or Location");
            }

          


            string strEmpList = "0";
            string strLocationList = string.Empty;
            string strDepartmentList = string.Empty;

            foreach (string str in strLocationName.Split(','))
            {
                if (str.Trim() == "")
                {
                    continue;
                }

                strLocationList += "'" + str.Trim() + "'" + ",";

            }

            foreach (string str in strDepartmentName.Split(','))
            {
                if (str.Trim() == "")
                {
                    continue;
                }

                strDepartmentList += "'" + str.Trim() + "'" + ",";

            }



            if (strEmpCodelist != "")
            {
                strEmpList = ObjDa.get_SingleValue("SELECT  STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Set_EmployeeMaster.Emp_Type='On Role' and Set_EmployeeMaster.Field2='False' and Set_EmployeeMaster.Emp_code     in  (" + strEmpCodelist + ") FOR xml PATH ('')), 1, 1, '') as Value");
            }
            else
            {
                if (strDepartmentList == "")
                {
                    strEmpList = ObjDa.get_SingleValue("SELECT  STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Set_EmployeeMaster.Emp_Type='On Role' and Set_EmployeeMaster.Field2='False' and Set_EmployeeMaster.Location_Id  in   (select set_locationmaster.location_id from set_locationmaster where location_name in  (" + strLocationList.Substring(0, strLocationList.Length - 1) + ")) FOR xml PATH ('')), 1, 1, '') as Value");
                }
                else
                {
                    strEmpList = ObjDa.get_SingleValue("SELECT  STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Set_EmployeeMaster.Emp_Type='On Role' and Set_EmployeeMaster.Field2='False' and Set_EmployeeMaster.Location_Id  in   (select set_locationmaster.location_id from set_locationmaster where location_name in  (" + strLocationList.Substring(0, strLocationList.Length - 1) + ")) and  Set_EmployeeMaster.department_Id  in   (select Set_DepartmentMaster.Dep_Id from Set_DepartmentMaster where Set_DepartmentMaster.dep_name in  (" + strDepartmentList.Substring(0, strDepartmentList.Length - 1) + "))  FOR xml PATH ('')), 1, 1, '') as Value");
                }
            }

            //if (dtEmpList != null)
            //{
            //    strEmpList = dtEmpList.Rows[0][0].ToString();
            //}
            DataTable dtFilter = new DataTable();



            dtFilter = ObjRegister.GetAttendanceReport(strEmpList, Convert.ToDateTime(strFromDate).ToString(), Convert.ToDateTime(strToDate).ToString(), "12");

            HttpContext.Current.Response.Write(DataTableToJSONWithJSONNet(dtFilter));
        }
        catch (Exception ex)
        {
            dtResult.Rows.Add(ex.Message.ToString());
            HttpContext.Current.Response.Write(DataTableToJSONWithJSONNet(dtResult));
        }


    }


    public string DataTableToJSONWithJSONNet(DataTable table)
    {

        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
        return json;
    }




}
