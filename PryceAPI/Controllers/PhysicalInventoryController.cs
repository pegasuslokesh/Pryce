using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PryceAPI.Controllers
{
    public class PhysicalInventoryController : Controller
    {
        // GET: PhysicalInventory
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(string UserCode, string Password)
        {
            DataTable dt = new DataTable("ResultTable");
            DataSet ds = new DataSet("ResultSet");
            try
            {
                string sql = "Select  Set_EmployeeMaster.Emp_Id ,Set_EmployeeMaster.Emp_Code , Set_EmployeeMaster.Emp_Name , Set_LocationMaster.Location_Id ,Set_LocationMaster.Location_Name From Set_UserMaster INNER JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Code = Set_UserMaster.User_Id  INNER JOIN  Set_LocationMaster ON Set_LocationMaster.Location_Id = Set_EmployeeMaster.Location_Id WHERE Set_UserMaster.User_Id ='' and  Set_UserMaster.Password  ='' ";



            }
            catch
            {

            }
           

            string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
            json = json.Replace("\r", "").Replace("\n", "");
            return Json(json, "text/x-json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            
        }
    }
}