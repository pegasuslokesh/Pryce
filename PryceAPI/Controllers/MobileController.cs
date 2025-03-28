using PryceAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PryceAPI.Controllers
{
    public class MobileController : Controller
    {
        // GET: Mobile
        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult RegisterDevice(string EmpCode,string Code1,string Code2,string Code3,string Code4)
        //{
        //    MobileResult mr = new MobileResult();
        //    string JSON result = JsonConvert.SerializeObject(emp);
        //}
    }
}