using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json.Converters;
using System.Configuration;
using Newtonsoft.Json.Serialization;

using PegasusDataAccess;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public partial class Email_Api : System.Web.UI.Page
{
    DataAccessClass objDA = null;
    Ems_MailMarketing objMailMarket = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        objDA = new DataAccessClass(Session["DBConnection"].ToString());
        objMailMarket = new Ems_MailMarketing(Session["DBConnection"].ToString());

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string registration_code;
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add(new DataColumn("result"));
        string s = string.Empty;
        DataRow row = dtResult.NewRow();
        string strSQL = string.Empty;

        try
        {
            registration_code = Request.Form["Query"].ToString();


            JObject results = JObject.Parse(registration_code);


            foreach (var result in results["result"])
            {
                int b = 0;

                string Display_text = (string)result["Display_text"];
                string Subject = (string)result["Subject"];
                string Template_Content = (string)result["Template_Content"];



                b = objMailMarket.CRUDHeaderRecord("2", "2", "2", "0", "0", Template_Content, Subject,"7", Display_text,"0", "", "", "", true.ToString(), "3000", DateTime.Now.ToString(), "3000", DateTime.Now.ToString(), "1");
     

               
                // this can be a string or array, how can we tell which it is
                JToken EmailList = result["emails"];


                //save header record and get id 

                string Email = "";
                if (EmailList is JValue)
                {
                    Email = (string)EmailList;
                }
                else if (EmailList is JArray)
                {
                    // can pick one, or flatten array to a string

                    //string[] nums = (string[])supervisor.ToArray(typeof(string)); 
                    //supervisorName = (string)((JArray)supervisor).First;

                    foreach (var item in EmailList)
                    {
                        objMailMarket.CRUDDetailRecord("0", b.ToString(),"0", "",item.ToString(), " ", "", "", "", "", true.ToString(), "3000", DateTime.Now.ToString(), "3000", DateTime.Now.ToString(), "1");
               
                    }
                }
              // Based on Value u need to isnert or else 


                row[0] = b.ToString();
            }
        }
        catch(Exception Ex)
        {
            row[0] = Ex.ToString();
        }
        dtResult.Rows.Add(row);

        Response.Write(DataTableToJSONWithJSONNet(dtResult));

    }
    public string DataTableToJSONWithJSONNet(DataTable table)
    {
       
        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
        return json;
    }
    
}