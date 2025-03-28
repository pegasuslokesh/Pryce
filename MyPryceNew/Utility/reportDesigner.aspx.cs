using System;
using System.Web;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using System.Data.SqlClient;

public partial class reportDesigner : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DesignerTask task = (DesignerTask)Session["DesignerTask"];
        if (task != null)
        {
            InitDesignerPage(task);

        }
        else if (!Page.IsCallback)
        {
            Response.Redirect("reportDesignerSelector.aspx");
        }

        if (!IsPostBack)
        {
            DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new CustomReportStorageWebExtension(Session["DBConnection"].ToString()));
        }

    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="task"></param>
    private void InitDesignerPage(DesignerTask task)
    {
        BindToData();

        switch (task.mode)
        {
            case ReportEdditingMode.NewReport:
                // Create a new report from the template.
                // DevExpress.XtraReports.UI.XtraReport newXtrareport = new DevExpress.XtraReports.UI.XtraReport();
                ReportTemplate rptNew = new ReportTemplate();
                ASPxReportDesigner1.OpenReport(rptNew);
                break;
            case ReportEdditingMode.ModifyReport:
                // Load an existing report from the report storage.
                try
                {
                    ASPxReportDesigner1.OpenReport(task.reportID);
                    
                }
                catch (Exception ex)
                {
                }
                break;
        }
    }


    #region #BindToData
    private void BindToData()
    {
        // Create a SQL data source with the specified connection parameters.
        //Access97ConnectionParameters connectionParameters =
        //    new Access97ConnectionParameters(HttpRuntime.AppDomainAppPath + "App_Data\\nwind.mdb", "", "");
        //SqlDataSource ds = new SqlDataSource(connectionParameters);

        SqlConnectionStringBuilder SSB = new SqlConnectionStringBuilder(HttpContext.Current.Session["DBConnection"].ToString());


        //MsSqlConnectionParameters sqlConParam = new MsSqlConnectionParameters("PRYCE-TL", "pryce_230218", "sa", "123", MsSqlAuthorizationType.SqlServer);
        MsSqlConnectionParameters sqlConParam = new MsSqlConnectionParameters(SSB.DataSource, SSB.InitialCatalog, SSB.UserID, SSB.Password, MsSqlAuthorizationType.SqlServer);
        // SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        SqlDataSource ds1 = new SqlDataSource(sqlConParam);


        // Create a custom SQL query to access the Products data table.
        CustomSqlQuery query = new CustomSqlQuery();
        //query.Name = "Products";
        //query.Sql = "SELECT * FROM Products";
        //ds.Queries.Add(query);
        //ds.RebuildResultSchema();


        // CustomSqlQuery query = new CustomSqlQuery();
        // query.Name = "ProductMaster";
        // query.Sql = "SELECT * FROM Inv_ProductMaster";
        // //QueryParameter qp1= new QueryParameter()
        //// query.Parameters.Add()
        // ds1.Queries.Add(query);
        // ds1.RebuildResultSchema();

        query = new CustomSqlQuery();
        query.Name = "EmployeeMaster";
        query.Sql = "SELECT emp_id,emp_name,location_id from Set_EmployeeMaster where Set_EmployeeMaster.IsActive = 'True' and Set_EmployeeMaster.field2 = 'false'";
        ds1.Queries.Add(query);
        ds1.RebuildResultSchema();


        query = new CustomSqlQuery();
        query.Name = "AccessibleLocation";
        if (HttpContext.Current.Session["UserId"].ToString() == "superadmin")
        {
            query.Sql = "select set_locationMaster.* from set_locationMaster where set_locationMaster.company_id=" + Session["CompId"].ToString() + " and IsActive='true'";
        }
        else
        {
            query.Sql = "select set_locationMaster.* from set_locationMaster inner join (Select distinct Record_Id as Location_id From Set_UserDataPermission where Set_UserDataPermission.Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and  Set_UserDataPermission.User_Id ='" + HttpContext.Current.Session["UserId"].ToString() + "' and Set_UserDataPermission.IsActive='True' and Record_Type='L') Acc_Loc on Acc_Loc.Location_id=set_locationMaster.Location_id where set_locationMaster.company_id=" + Session["CompId"].ToString() + " and IsActive='true'";
        }
        ds1.Queries.Add(query);
        ds1.RebuildResultSchema();


        query = new CustomSqlQuery();
        query.Name = "CompanyDtls";
        query.Sql = "select Set_CompanyMaster.Company_Name as HeaderName,Set_CompanyMaster.Company_Name_L as HeaderName_L, '~/CompanyResource/" + Session["Compid"].ToString() + "/'+Set_CompanyMaster.Logo_Path as Imageurl,Set_AddressMaster.Address,Set_AddressMaster.Street,Set_AddressMaster.Block,Set_AddressMaster.Avenue,Set_AddressMaster.CityId,Set_AddressMaster.StateId,Set_AddressMaster.CountryId,Set_AddressMaster.PinCode,Set_AddressMaster.PhoneNo1,Set_AddressMaster.PhoneNo2,Set_AddressMaster.MobileNo1,Set_AddressMaster.MobileNo2,Set_AddressMaster.FaxNo,Set_AddressMaster.WebSite from Set_CompanyMaster  full outer join Set_AddressChild on Set_CompanyMaster.Company_Id=Set_AddressChild.Add_Ref_Id and  Set_AddressChild.Add_Type='Company' full outer join Set_AddressMaster on Set_AddressChild.Ref_Id=Set_AddressMaster.Trans_Id  where Set_CompanyMaster.Company_Id='" + Session["CompId"].ToString() + "'  and Set_CompanyMaster.IsActive='True'";
        ds1.Queries.Add(query);
        ds1.RebuildResultSchema();


        query = new CustomSqlQuery();
        query.Name = "EmployeeDetails";
        query.Sql = "select Set_UserMaster.User_Id,Set_UserMaster.Emp_Id,Set_EmployeeMaster.Emp_Name from Set_UserMaster inner join Set_EmployeeMaster on Set_EmployeeMaster.emp_id = Set_UserMaster.Emp_Id where Set_UserMaster.User_Id='" + Session["UserId"].ToString() + "'";
        ds1.Queries.Add(query);
        ds1.RebuildResultSchema();



        // Add the created data source to the list of default data sources. 
        //ASPxReportDesigner1.DataSources.Add("Northwind", ds);

        //ASPxReportDesigner1.AvailableDataSources.Add("Pegasus", ds1);

        ASPxReportDesigner1.DataSources.Add("Pegasus", ds1);

    }
    #endregion #BindToData
}
