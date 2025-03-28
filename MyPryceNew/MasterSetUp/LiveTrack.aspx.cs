using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class MasterSetUp_LiveTrack : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = this.GetData();
            rptMarkers.DataSource = dt;
            rptMarkers.DataBind();
        }
    }
    private DataTable GetData()
    {
        ArrayList objarr = (ArrayList)Session["LiveTrackParam"];


        PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        DataTable dt = new DataTable();


        //string strsql = "SELECT (select set_employeemaster.Emp_Name from set_employeemaster where set_employeemaster.emp_id=  (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=gw_geocoords.User_code)) as Name,  id ,latitute as lat ,longitute as lng , user_code as title , address as description FROM  GeoWork.dbo.gw_geocoords Where user_code ='" + objar[0].ToString() + "' and ((CONVERT(Date,t_date)>=CONVERT(Date,'" + objar[1].ToString() + "')) and (CONVERT(Date,t_date)<=CONVERT(Date,'" + objar[2].ToString() + "') ))order by id Desc";
        //DataTable dtRecord = objDA.return_DataTable(strsql);
        //rptMarkers.DataSource = dtRecord;


        //dt = objDA.return_DataTable("SELECT  p.user_code  as Name, latitute as Latitude ,longitute as Longitude , '' as Description FROM   GeoWork.dbo.gw_geocoords p INNER JOIN (SELECT user_code,MAX(t_date) AS MAXDATE FROM GeoWork.dbo.gw_geocoords GROUP BY user_code) tp ON p.user_code = tp.user_code AND p.t_date = tp.MAXDATE");

        dt = objDA.return_DataTable("SELECT p.t_date,p.User_code, (select set_employeemaster.Emp_Name from Pryce.dbo.set_employeemaster where set_employeemaster.emp_code=p.User_code)  as Name, latitute as Latitude ,longitute as Longitude , ' ' as Description FROM   GeoWork.dbo.gw_geocoords p INNER JOIN (SELECT user_code,MAX(t_date) AS MAXDATE FROM GeoWork.dbo.gw_geocoords GROUP BY user_code) tp ON p.user_code = tp.user_code AND p.t_date = tp.MAXDATE");


        //for particular user

        //code start

        if (objarr[0].ToString() != "")
        {

            dt = new DataView(dt, "User_code='" + objarr[0].ToString().Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        //code end

        DataTable dtRecord = new DataTable();
        dtRecord.Columns.Add(new DataColumn("Name"));
        dtRecord.Columns.Add(new DataColumn("Latitude"));
        dtRecord.Columns.Add(new DataColumn("Longitude"));
        dtRecord.Columns.Add(new DataColumn("Description"));

        //DataRow row1 = dtRecord.NewRow();
        //row1[1] = "24.606";
        //row1[2] = "73.6841";
        //row1[0] = "3002";
        //row1[3] = "Saheli Nagar Udaipur, Rajasthan 313001 Udaipur India";

        //dtRecord.Rows.Add(row1);


        //DataRow row2 = dtRecord.NewRow();
        //row2[1] = "24.606102";
        //row2[2] = "73.684074";
        //row2[0] = "3001";
        //row2[3] = "address";


        //dtRecord.Rows.Add(row2);


        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    DataRow row2 = dtRecord.NewRow();
        //    row2[1] = dt.Rows[i][1].ToString();
        //    row2[2] = dt.Rows[i][2].ToString();
        //    row2[0] = dt.Rows[i][0].ToString();
        //    row2[3] = dt.Rows[i][3].ToString();

        //    dtRecord.Rows.Add(row2);
        //}


        //dtRecord.AcceptChanges();
            return dt;
    }
}