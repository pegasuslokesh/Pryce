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
using System.Linq;
using System.Globalization;

public partial class WorkOrder_api : System.Web.UI.Page
{
    DataAccessClass objDA = null;
    SM_WorkOrder objWorkorder = null;
    TaskMaster objTaskMaster = null;
    Ems_ContactMaster objContactMaster = null;
    ContactNoMaster objContactnoMaster = null;
    Set_AddressMaster objAddressMaster = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        objDA = new DataAccessClass(Session["DBConnection"].ToString());
        objWorkorder = new SM_WorkOrder(Session["DBConnection"].ToString());
        objTaskMaster = new TaskMaster(Session["DBConnection"].ToString());
        objContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objAddressMaster = new Set_AddressMaster(Session["DBConnection"].ToString());

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string WorkOrder_TransID;
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add(new DataColumn("result"));
        string s = string.Empty;
        DataRow row = dtResult.NewRow();
        string strSQL = string.Empty;

        try
        {

            WorkOrder_TransID = Request.Form["Query"].ToString();
            JObject results = JObject.Parse(WorkOrder_TransID);

            if (results.Last.Path.ToString() == "UpdateVisit")
            {
                foreach (var result in results["UpdateVisit"])
                {
                    int b = 0;

                    string trans_id = (string)result["trans_id"];
                    string latitude = (string)result["Current_latitude"];
                    string longitude = (string)result["Current_longitude"];
                    string remark = (string)result["remark"];
                    string emp_comment = (string)result["emp_comment"];
                    string cust_comment = (string)result["cust_comment"];
                    string task_status = (string)result["task_status"];
                    string modifiedBy = (string)result["modifiedBy"];
                    string campaignid = (string)result["campaignName"];
                    string workorder_status = (string)result["workorder_status"];
                    string visit_date = (string)result["visit_date"];
                    string end_time = (string)result["end_time"];
                    string address_id = (string)result["address_id"];

                    string w3Time = "";
                    w3Time = visit_date.Split('-')[2] + "-" + visit_date.Split('-')[1] + "-" + visit_date.Split('-')[0];
                    DateTime time = DateTime.Parse(w3Time);
                    //used to update the location and remarks of work order
                    b = objWorkorder.UpdateLat_Long_RemarkByTransId(trans_id, latitude, longitude, remark, modifiedBy, emp_comment, cust_comment, time.ToString(), end_time, address_id);

                    //this try block is used to update the status of task assigned to employee from campaign
                    try
                    {
                        if (latitude != "" || latitude != "0.000000")
                        {
                            objAddressMaster.UpdateGioCoordinatedIfNotPresent(address_id, latitude, longitude);
                        }

                        if (campaignid != "")
                        {
                            string custId = objWorkorder.getCustIDFromTransID(trans_id);

                            DataTable dt_taskData = objTaskMaster.getCampaignTaskByUserID_PKID(modifiedBy, campaignid, custId);

                            if (dt_taskData.Rows.Count > 0)
                            {
                                objTaskMaster.updateTaskStatusByTransID(task_status, dt_taskData.Rows[0]["Trans_ID"].ToString(), modifiedBy);
                            }
                        }

                    }
                    catch
                    {

                    }
                    //this is used to update the visit closed date for customer in campaign
                    try
                    {

                        if (campaignid != "")
                        {
                            objWorkorder.Update_CampaignCustomerVisitCloseDateFromWorkorderTransId_CampaignID(trans_id, campaignid, modifiedBy);
                        }
                    }
                    catch
                    {

                    }


                    if (b != 0)
                    {
                        row[0] = "Updated Successfully";
                    }
                    else
                    {
                        row[0] = "Error";
                    }
                }
            }


            if (results.Last.Path.ToString() == "SelectVisit")
            {
                foreach (var result in results["SelectVisit"])
                {
                    string user_id = (string)result["user_id"];
                    string fromDate = (string)result["fromDate"];
                    string toDate = (string)result["toDate"];
                    string WOstatus = (string)result["workorder_status"];

                    string frmDt = "";
                    frmDt = fromDate.Split('-')[2] + "-" + getMonth(fromDate.Split('-')[1]) + "-" + fromDate.Split('-')[0];

                    string toDt = "";
                    toDt = toDate.Split('-')[2] + "-" + getMonth( toDate.Split('-')[1]) + "-" + toDate.Split('-')[0];

                    DataTable dt_workorder = objWorkorder.GetRecordByVisitPersonUserId(user_id, frmDt, toDt, WOstatus);

                    DataTable distinctContact = dt_workorder.DefaultView.ToTable(true, "Customer_Id");
                    string ContactIDs = "";

                    for (int i = 0; i < distinctContact.Rows.Count; i++)
                    {
                        ContactIDs += distinctContact.Rows[i]["Customer_Id"].ToString() + ",";
                    }

                    DataTable dt_ContactName = objContactMaster.GetAllContactNameAndNoByCompanyID(ContactIDs);


                    //DataTable dt_ContactName = objContactMaster.GetAllContactNameByCompanyID(ContactIDs);

                    //DataTable distinctContactPKID = dt_ContactName.DefaultView.ToTable(true, "Trans_Id");
                    //string ContactPKID = "";

                    //for (int i = 0; i < distinctContactPKID.Rows.Count; i++)
                    //{
                    //    ContactPKID += distinctContactPKID.Rows[i]["Trans_Id"].ToString() + ",";
                    //}
                    //DataTable dt_ContactNo = objContactnoMaster.GetAllContactNoByPKID(ContactPKID);
                    DataSet ds = new DataSet();

                    DataTable dtCopy1 = dt_workorder.Copy();
                    ds.Tables.Add(dtCopy1);

                    DataTable dtCopy2 = dt_ContactName.Copy();
                    ds.Tables.Add(dtCopy2);

                    //DataTable dtCopy3 = dt_ContactNo.Copy();
                    //ds.Tables.Add(dtCopy3);

                    Response.Write(ds2json(ds));
                    return;
                }

            }

            if (results.Last.Path.ToString() == "SelectContact")
            {
                foreach (var result in results["SelectContact"])
                {
                    string Contact_Id = (string)result["Contact_Id"];
                    DataTable dt_ContactName = objContactMaster.GetAllContactNameByCompanyID(Contact_Id);
                    Response.Write(DataTableToJSONWithJSONNet(dt_ContactName));
                    return;
                }
            }

            if (results.Last.Path.ToString() == "SelectMobile")
            {
                foreach (var result in results["SelectMobile"])
                {
                    string Contact_Id = (string)result["Contact_Id"];
                    string Contact_name = (string)result["Contact_name"];

                    DataTable dt_ContactName = objContactMaster.GetAllContactNameAndNoByCompanyID(Contact_Id);
                    dt_ContactName = new DataView(dt_ContactName, "Name='" + Contact_name + "'", "", DataViewRowState.CurrentRows).ToTable();
                    Response.Write(DataTableToJSONWithJSONNet(dt_ContactName));
                    return;
                }
            }
        }
        catch (Exception Ex)
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

    public static string ds2json(DataSet ds)
    {
        return JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);
    }

    public string getMonth(string mm)
    {
        string mmm = "";
        if (mm == "01")
        {
            mmm = "Jan";
        }
        else if (mm == "02")
        {
            mmm = "Feb";
        }
        else if (mm == "03")
        {
            mmm = "Mar";
        }
        else if (mm == "04")
        {
            mmm = "Apr";
        }
        else if (mm == "05")
        {
            mmm = "May";
        }
        else if (mm == "06")
        {
            mmm = "Jun";
        }
        else if (mm == "07")
        {
            mmm = "Jul";
        }
        else if (mm == "08")
        {
            mmm = "Aug";
        }
        else if (mm == "09")
        {
            mmm = "Sep";
        }
        else if (mm == "10")
        {
            mmm = "Oct";
        }
        else if (mm == "11")
        {
            mmm = "Nov";
        }
        else if (mm == "12")
        {
            mmm = "Dec";
        }
        return mmm;
    }

}