#region #CustomReportStorage
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DevExpress.XtraReports.UI;
using CatalogDataTableAdapters;
using System.Web;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.ConnectionParameters;
// ...


public class CustomReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
{
    private CatalogData catalogDataSet;
    private DataTable reportsTable;
    private sys_reportsTableAdapter reportsTableAdapter;
    ObjectMaster objObject = null;
    IT_App_Op_Permission obj_OP_Permission = null;
    public CustomReportStorageWebExtension(string strConString)
    {
        try
        {
            ObjectMaster objObject = new ObjectMaster(strConString);
            IT_App_Op_Permission obj_OP_Permission = new IT_App_Op_Permission(strConString);
            setAdapterconnection();

        }
        catch (Exception ex)
        {

        }
    }

    public void setAdapterconnection()
    {
        catalogDataSet = new CatalogData();
        reportsTableAdapter = new sys_reportsTableAdapter();
        if (HttpContext.Current.Session["DBConnection"] != null)
        {
            string strconn = HttpContext.Current.Session["DBConnection"].ToString();
            reportsTableAdapter.Connection.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
            reportsTableAdapter.Fill(catalogDataSet.sys_reports);
            reportsTable = catalogDataSet.Tables["sys_reports"];
        }
    }

    public override bool CanSetData(string url)
    {
        // Check if the URL is available in the report storage.
        return reportsTable.Rows.Find(int.Parse(url)) != null;
    }


    public override byte[] GetData(string url)
    {
        // Get the report data from the storage.

        string CatlogName = string.Empty;
        string DataSource = string.Empty;
        string UserName = string.Empty;
        string Password = string.Empty;
        foreach (string str in HttpContext.Current.Session["DBConnection"].ToString().Split(';'))
        {
            if (str.Contains("Initial Catalog"))
            {
                CatlogName = str.Split('=')[1];
            }
            if (str.Contains("Data Source"))
            {
                DataSource = str.Split('=')[1];
            }
            if (str.Contains("User ID"))
            {
                UserName = str.Split('=')[1];
            }
            if (str.Contains("Password"))
            {
                Password = str.Split('=')[1];
            }

        }


        setAdapterconnection();

        DataRow row = reportsTable.Rows.Find(int.Parse(url));
        if (row == null) return null;
        byte[] reportData = (Byte[])row["LayoutData"];
        XtraReport objXr = new XtraReport();
        using (MemoryStream ms = new MemoryStream(reportData))
        {
            objXr.LoadLayout(ms);
            try
            {
                SqlDataSource dataSource = objXr.DataSource as SqlDataSource;
                MsSqlConnectionParameters sqlConParam = new MsSqlConnectionParameters(DataSource, CatlogName, UserName, Password, MsSqlAuthorizationType.SqlServer);
                dataSource.ConnectionParameters = sqlConParam;
            }
            catch
            {

            }

            using (MemoryStream ms1 = new MemoryStream())
            {
                objXr.SaveLayoutToXml(ms1);
                reportData = ms1.GetBuffer();
            }
        }

        return reportData;

    }


    public override Dictionary<string, string> GetUrls()
    {
        // Get URLs and display names for all reports available in the storage.
        return reportsTable.AsEnumerable()
              .ToDictionary<DataRow, string, string>(dataRow => ((Int32)dataRow["ReportID"]).ToString(),
                                                     dataRow => (string)dataRow["DisplayName"]);
    }


    public override bool IsValidUrl(string url)
    {
        // Check if the specified URL is valid for the current report storage.
        // In this example, a URL should be a string containing a numeric value that is used as a data row primary key.
        int n;
        return int.TryParse(url, out n);
    }


    public override void SetData(XtraReport report, string url)
    {
        //used to get the updated report table data
        setAdapterconnection();

        // Write a report to the storage under the specified URL.
        DataRow row = reportsTable.Rows.Find(int.Parse(url));

        if (row != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);
                byte[] data = ms.GetBuffer();
                row["LayoutData"] = ms.GetBuffer();
            }
            row["ModifiedBy"] = HttpContext.Current.Session["UserId"].ToString();
            row["ModifiedDate"] = DateTime.Now.ToString();
            reportsTableAdapter.Update(catalogDataSet);
            catalogDataSet.AcceptChanges();
        }
    }


    public override string SetNewData(XtraReport report, string defaultUrl)
    {
        setAdapterconnection();
        // Save a report to the storage under a new URL. 
        // The defaultUrl parameter contains the report display name specified by a user.
        DataRow row = reportsTable.NewRow();

        row["DisplayName"] = defaultUrl;

        using (MemoryStream ms = new MemoryStream())
        {
            report.SaveLayoutToXml(ms);
            row["LayoutData"] = ms.GetBuffer();
        }


        row["ModuleId"] = HttpContext.Current.Session["ReportModuleId"].ToString();
        row["ObjectId"] = HttpContext.Current.Session["ReportObjectIDOfReport"].ToString();
        row["ReportType"] = HttpContext.Current.Session["ReportType"].ToString();
        row["CreatedBy"] = HttpContext.Current.Session["UserId"].ToString();
        row["CreatedDate"] = DateTime.Now.ToString();
        row["ModifiedBy"] = HttpContext.Current.Session["UserId"].ToString();
        row["ModifiedDate"] = DateTime.Now.ToString();
        row["IsActive"] = "true";
        reportsTable.Rows.Add(row);
        reportsTableAdapter.Update(catalogDataSet);
        catalogDataSet.AcceptChanges();

        // Refill the dataset to obtain the actual value of the new row's autoincrement key field.
        reportsTableAdapter.Fill(catalogDataSet.sys_reports);
        string key = catalogDataSet.sys_reports.FirstOrDefault(x => x.DisplayName == defaultUrl).ReportId.ToString();

        //here we added code for insert new object entry 

        //start
        if (HttpContext.Current.Session["ReportType"].ToString() != "Sub Report")
        {
            int b = objObject.InsertObjectMaster(defaultUrl, defaultUrl, "../Utility/reportviewer.aspx?ReportId=" + key + "", "0", "F", "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), "0", "0", "false");
            //Session["Application_Id"]
            objObject.InsertObjectModuleMaster(HttpContext.Current.Session["Application_Id"].ToString(), HttpContext.Current.Session["ReportModuleId"].ToString(), b.ToString());
            objObject.InsertObjectModuleMaster("0", HttpContext.Current.Session["ReportModuleId"].ToString(), b.ToString());
            obj_OP_Permission.insertRecord(b.ToString(), "5");
        }
        //end

        return key;

    }
}

#endregion #CustomReportStorage