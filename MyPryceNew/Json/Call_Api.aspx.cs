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
using System.Configuration;
using PegasusDataAccess;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


public partial class Call_Api : System.Web.UI.Page
{
    DataAccessClass objDA = null;
    CallLogs ObjCallLogs = null;
    Set_DocNumber ObjDoc = null;
    SystemParameter objSysParam = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string registration_code;
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add(new DataColumn("result"));
        string s = string.Empty;
        DataRow row = dtResult.NewRow();
        string strSQL = string.Empty;
Session["DBConnection"] = ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString;
        objDA = new DataAccessClass(Session["DBConnection"].ToString());
        ObjCallLogs = new CallLogs(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());

        try
        {
            registration_code = Request.Form["Query"].ToString();


            JObject results = JObject.Parse(registration_code);


            foreach (var result in results["result"])
            {

                string CustomerName = (string)result["name"];
                string EmailId = (string)result["email"];
                string CountryId = (string)result["country"];
                string Message = (string)result["message"];
                string ContactNo = (string)result["mobile"];
                // this can be a string or array, how can we tell which it is
                JToken ProductList = result["products"];

                string ProductName = "";
                if (ProductList is JValue)
                {
                    ProductName = (string)ProductList;
                }
                else if (ProductList is JArray)
                {
                    // can pick one, or flatten array to a string

                    //string[] nums = (string[])supervisor.ToArray(typeof(string)); 
                    //supervisorName = (string)((JArray)supervisor).First;

                    foreach (var item in ProductList)
                    {
                        if (ProductName == "")
                        {
                            ProductName = item.ToString();
                        }
                        else
                        {
                            ProductName = ProductName + "," + item.ToString();
                        }
                    }
                }
              // Based on Value u need to isnert or else 

                int b = 0;
                b = ObjCallLogs.InsertCallLogs("2", "2", "8", "",DateTime.Now.ToString(), CustomerName, CustomerName,ContactNo, EmailId, "Service", Message+" - Call Registered By Pegasus Website", "0", "High", "Open", ProductName, "Not Generated", "313", "", "", false.ToString(), false.ToString(), DateTime.Now.ToString(), "True", "Superadmin", DateTime.Now.ToString(),"Superadmin", DateTime.Now.ToString());

                DataTable dtCount = ObjCallLogs.GetAllRecord("2", "2", "8");
                if (dtCount.Rows.Count == 0)
                {
                    ObjCallLogs.Updatecode(b.ToString(), GetDocumentNumber() + "1");
                }
                else
                {
                    ObjCallLogs.Updatecode(b.ToString(), GetDocumentNumber() + dtCount.Rows.Count);
                }
                row[0] = true;
            }
        }
        catch(Exception Ex)
        {
            row[0] = Ex.Message.ToString();
        }
        dtResult.Rows.Add(row);

        Response.Write(DataTableToJSONWithJSONNet(dtResult));

    }
    public string DataTableToJSONWithJSONNet(DataTable table)
    {
       
        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
        return json;
    }

    protected string GetDocumentNumber()
    {
        string s = GetDocumentNo(true,"2", true, "158", "270", "0");
        return s;
    }




    public DataTable GetDocumentNumberAll(string StrCompanyId, string strBrandId, string strLocationId, string ModuleId, string ObjectId, string OpType)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", StrCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Object_Id", ObjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Module_Id", ModuleId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = objDA.Reuturn_Datatable_Search("sp_Set_DocNumber_SelectRow", paramList, Session["DBConnection"].ToString());
        return dtInfo;
    }




    public string GetDocumentNo(bool IsFormateOnly, string CompanyId, bool IsUseCompIdInWhere, string ModuleId, string ObjectId, string LastTransId)
    {
        string StrDocument = string.Empty;
        DataTable dt = new DataTable();
        if (IsUseCompIdInWhere)
        {
            dt =GetDocumentNumberAll(CompanyId, "2","8", ModuleId, ObjectId, "2");
        }
        else
        {
            dt = GetDocumentNumberAll("2", "2", "8", ModuleId, ObjectId, "2");
        }

        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];

            if (dr["Prefix"].ToString() != "")
            {
                StrDocument += dr["Prefix"].ToString();
            }
            if (Convert.ToBoolean(dr["CompId"].ToString()))
            {
                StrDocument += "2";
            }

            if (Convert.ToBoolean(dr["BrandId"].ToString()))
            {
                StrDocument += "2";
            }

            if (Convert.ToBoolean(dr["LocationId"].ToString()))
            {
                StrDocument += "2";
            }

            if (Convert.ToBoolean(dr["DeptId"].ToString()))
            {
                StrDocument += "0";
            }

            if (Convert.ToBoolean(dr["EmpId"].ToString()))
            {
                StrDocument += "0";
            }


            if (Convert.ToBoolean(dr["Day"].ToString()))
            {
                StrDocument += DateTime.Now.Day.ToString();
            }

            if (Convert.ToBoolean(dr["Month"].ToString()))
            {
                StrDocument += DateTime.Now.Month.ToString();
            }
            if (Convert.ToBoolean(dr["Year"].ToString()))
            {
                StrDocument += DateTime.Now.Year.ToString();
            }

            if (dr["Suffix"].ToString() != "")
            {
                StrDocument += dr["Suffix"].ToString();
            }

            if (!IsFormateOnly)
            {
                if (StrDocument != "")
                {
                    StrDocument += (LastTransId + 1).ToString();
                }
                else
                {
                    StrDocument += (LastTransId + 1).ToString();

                }
            }
            else
            {
                StrDocument += "";
            }
        }
        return StrDocument.Trim();
    }
}