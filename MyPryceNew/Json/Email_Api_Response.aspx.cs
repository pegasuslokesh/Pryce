using System;
using System.Collections.Generic;
using System.Linq;
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

public partial class Json_Email_Api_Response : System.Web.UI.Page
{
    Ems_MailMarketing objMailMarket = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objMailMarket = new Ems_MailMarketing(Session["DBConnection"].ToString());

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string registration_code;

        Dictionary<string, string[]> udemy = new Dictionary<string, string[]>();

        DataTable jsonresult = new DataTable();
        string strSQL = string.Empty;
        string reftype = string.Empty;
        string json2 = string.Empty;
        try
        {
            registration_code = Request.Form["Query"].ToString();


            JObject results = JObject.Parse(registration_code);


            foreach (var result in results["result"])
            {
                int b = 0;

                string RefId = (string)result["Ref_Id"];
                reftype = (string)result["Ref_Type"];
                string refValue = (string)result["Ref_Value"];
                DataTable dt = objMailMarket.GetRecordHeader(RefId.Trim(), "5");
                //dt = new DataView(dt, "Trans_id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dt.Rows.Count > 0)
                {

                    DataRow dr;
                    if (reftype.ToUpper() != "LIST")
                    {
                        jsonresult.Columns.Add("Ref_Id");
                        jsonresult.Columns.Add("TotalMail");
                        jsonresult.Columns.Add("SentMail");
                        jsonresult.Columns.Add("FailureMail");
                        jsonresult.Columns.Add("PendingMail");


                        dr = jsonresult.NewRow();

                        dr[0] = RefId.ToString();
                        dr[1] = dt.Rows[0]["TotalMail"].ToString();
                        dr[2] = dt.Rows[0]["SentMail"].ToString();
                        dr[3] = dt.Rows[0]["FailureMail"].ToString();
                        dr[4] = dt.Rows[0]["PendingMail"].ToString();

                        jsonresult.Rows.Add(dr);
                        json2 = DataTableToJSONWithJSONNet(jsonresult);
                    }
                    else
                    {
                        int count = 0;
                        DataTable dtdetail = objMailMarket.GetRecordDetail(RefId, "4");
                        string strMailList = string.Empty;
                        if (refValue.ToUpper() == "ALL")
                        {
                            //for total mail
                            string[] arr_TotalMail = new string[dtdetail.Rows.Count];

                            foreach (DataRow drrow in dtdetail.Rows)
                            {
                                arr_TotalMail[count] = drrow["EmailId"].ToString();

                                count++;
                            }


                            //here we get sent Mail List 

                            DataTable dtsentmail = new DataView(dtdetail, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable(true, "EmailId");

                            strMailList = "";

                             count = 0;

                             string[] arr_SentMail = new string[dtsentmail.Rows.Count];
                             
                            foreach (DataRow drrow in dtsentmail.Rows)
                            {
                                arr_SentMail[count] = drrow["EmailId"].ToString();

                                count++;
                            }






                            //for get failure mail
                            DataTable dtFailuremail = new DataView(dtdetail, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable(true, "EmailId");
                            strMailList = "";

                            string[] arr_FailureMail = new string[dtFailuremail.Rows.Count];
                            count = 0;
                           
                            foreach (DataRow drrow in dtFailuremail.Rows)
                            {
                                arr_FailureMail[count] = drrow["EmailId"].ToString();

                                count++;
                               

                            }




                            //for get pending mail
                            DataTable dtPendingemail = new DataView(dtdetail, "Field1=' '", "", DataViewRowState.CurrentRows).ToTable(true, "EmailId");

                            strMailList = "";

                            //for pending mail
                            string[] arr_PendingMail = new string[dtPendingemail.Rows.Count];

                            count = 0;
                            foreach (DataRow drrow in dtPendingemail.Rows)
                            {
                                arr_PendingMail[count] = drrow["EmailId"].ToString();
                                count++;
                            }


                            udemy.Add("TotalMail", arr_TotalMail);
                            udemy.Add("SentMail", arr_SentMail);
                            udemy.Add("FailureMail", arr_FailureMail);
                            udemy.Add("PendingMail", arr_PendingMail);


                            json2 = JsonConvert.SerializeObject(udemy, Newtonsoft.Json.Formatting.Indented);

                           
                        }
                        else
                            if (refValue.ToUpper() == "PENDING")
                            {
                                //for get pending mail
                                DataTable dtPendingemail = new DataView(dtdetail, "Field1=' '", "", DataViewRowState.CurrentRows).ToTable(true, "EmailId");
                                strMailList = "";

                                string[] arr_PendingMail = new string[dtPendingemail.Rows.Count];
                                count = 0;
                                foreach (DataRow drrow in dtPendingemail.Rows)
                                {
                                    arr_PendingMail[count] = drrow["EmailId"].ToString();
                                    count++;
                                }
                                udemy.Add("PendingMail", arr_PendingMail);
                                json2 = JsonConvert.SerializeObject(udemy, Newtonsoft.Json.Formatting.Indented);
                             
                            }
                            else if (refValue.ToUpper() == "SENT")
                            {

                                DataTable dtsentmail = new DataView(dtdetail, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable(true, "EmailId");

                                strMailList = "";

                                string[] arr_SentMail = new string[dtsentmail.Rows.Count];

                                count = 0;
                                foreach (DataRow drrow in dtsentmail.Rows)
                                {
                                    arr_SentMail[count] = drrow["EmailId"].ToString();
                                    count++;
                                }

                                udemy.Add("SentMail", arr_SentMail);
                                json2 = JsonConvert.SerializeObject(udemy, Newtonsoft.Json.Formatting.Indented);

                            }
                            else if (refValue.ToUpper() == "FAILED")
                            {
                                //for get failure mail
                                DataTable dtFailuremail = new DataView(dtdetail, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable(true, "EmailId");

                                strMailList = "";

                                string[] arr_FailureMail = new string[dtFailuremail.Rows.Count];
                                count = 0;
                                foreach (DataRow drrow in dtFailuremail.Rows)
                                {
                                    arr_FailureMail[count] = drrow["EmailId"].ToString();
                                    count++;
                                }

                                udemy.Add("FailureMail", arr_FailureMail);
                                json2 = JsonConvert.SerializeObject(udemy, Newtonsoft.Json.Formatting.Indented);

                            }
                    }

                }
            }
        }
        catch (Exception Ex)
        {

           

            string[] strError = new string[1];

            strError[0] = Ex.ToString();

            udemy.Add("Error", strError);
            json2 = JsonConvert.SerializeObject(udemy, Newtonsoft.Json.Formatting.Indented);

        }

       

       

       

        Response.Write(json2);
    }

    public string DataTableToJSONWithJSONNet(DataTable table)
    {

        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
        return json;
    }

}