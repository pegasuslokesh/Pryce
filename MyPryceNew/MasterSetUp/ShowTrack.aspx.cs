using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Globalization;
using PegasusDataAccess;
using System.Xml;
using System.Net;
using System.Text;
public partial class Google_ShowTrack : BasePage
{
    DataAccessClass objDA = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
            //Session["CompId"] = Session["CompId"].ToString();

            //string strcompanyId = Session["CompId"].ToString();
            body1.Style[HtmlTextWriterStyle.Direction] = "ltr";
        }
        else if (Session["lang"].ToString() == "2")
        {
            body1.Style[HtmlTextWriterStyle.Direction] = "rtl";
        }
        else if (Session["lang"].ToString() == "1")
        {
            body1.Style[HtmlTextWriterStyle.Direction] = "ltr";
        }

        objDA = new DataAccessClass(Session["DBConnection"].ToString());

        if (!this.IsPostBack)
        {
            ArrayList objar = (ArrayList)Session["TrackParam"];



            try
            {
                //string strsql = "SELECT   _id ,latitute as lat ,longitute as lng ,  User_code as title , '' as description FROM  GeoWork.dbo.gw_geocoords Where user_code ='" + objar[0].ToString() + "' and ((CONVERT(Date,t_date)>=CONVERT(Date,'" + objar[1].ToString() + "')) and (CONVERT(Date,t_date)<=CONVERT(Date,'" + objar[2].ToString() + "') ))order by _id Desc";

                string strsql = "SELECT t_date,  _id ,latitute as lat ,longitute as lng ,   (select set_employeemaster.Emp_Name from Pryce.dbo.set_employeemaster where set_employeemaster.emp_code=User_code) as title , ' ' as description FROM  GeoWork.dbo.gw_geocoords Where user_code ='" + objar[0].ToString() + "' and ((CONVERT(Date,t_date)>=CONVERT(Date,'" + objar[1].ToString() + "')) and (CONVERT(Date,t_date)<=CONVERT(Date,'" + objar[2].ToString() + "') ))order by _id Desc";
                DataTable dtRecord = objDA.return_DataTable(strsql);

                //DataTable dtRecord = new DataTable();
                //dtRecord.Columns.Add(new DataColumn("lat"));
                //dtRecord.Columns.Add(new DataColumn("lng"));
                //dtRecord.Columns.Add(new DataColumn("title"));
                //dtRecord.Columns.Add(new DataColumn("description"));

                //DataRow row1 = dtRecord.NewRow();
                //row1[0] = "24.6110188";
                //row1[1] = "73.684673";
                //row1[2] = "Office Address";
                //row1[3] = "Saheli Nagar";

                //dtRecord.Rows.Add(row1);

                //DataRow row2 = dtRecord.NewRow();
                //row2[0] = "24.5383905";
                //row2[1] = "73.6844444";
                //row2[2] = "Home Address";
                //row2[3] = "Sector -14";


                //dtRecord.Rows.Add(row2);

                //DataRow row3 = dtRecord.NewRow();
                //row3[0] = "24.5383905";
                //row3[1] = "73.6844444";
                //row3[2] = "Home Address";
                //row3[3] = "Sector -14";


                //dtRecord.Rows.Add(row3);


                //dtRecord.AcceptChanges();
               
                rptMarkers.DataSource = dtRecord;
                rptMarkers.DataBind();
            }
            catch
            {
            }






        }
    }

}