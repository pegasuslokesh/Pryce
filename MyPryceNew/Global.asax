<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application["TotalUser"] = 0;

        // Code that runs on application startup
        DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState =
        System.Web.SessionState.SessionStateBehavior.Required;

        DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState =
            System.Web.SessionState.SessionStateBehavior.Required;

        DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState =
            System.Web.SessionState.SessionStateBehavior.Required;
        //    // Code that runs on application startup

        DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new CustomReportStorageWebExtension(""));
        DevExpress.XtraReports.Web.ReportDesigner.DefaultReportDesignerContainer.EnableCustomSql();
        DevExpress.XtraReports.Web.ReportDesigner.DefaultReportDesignerContainer.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
        DevExpress.XtraReports.Security.ScriptPermissionManager.GlobalInstance = new DevExpress.XtraReports.Security.ScriptPermissionManager(DevExpress.XtraReports.Security.ExecutionMode.Unrestricted);
        // ...                           
        // Register a connection strings provider. 
        // ... 
        // Register the custom provider factory. 
        DevExpress.XtraReports.Web.ReportDesigner.DefaultReportDesignerContainer.RegisterDataSourceWizardDBSchemaProviderExFactory<MyDataSourceWizardDBSchemaProviderFactory>();

        DevExpress.Utils.UrlAccessSecurityLevelSetting.SecurityLevel = DevExpress.Utils.UrlAccessSecurityLevel.Unrestricted;
        // DevExpress.Utils.UrlAccessSecurityLevelSetting.SecurityLevel = DevExpress.Utils.UrlAccessSecurityLevel.FilesFromBaseDirectory;
        // Set the resource directory
        string contentPath = Server.MapPath("~/");
        string dataPath = Server.MapPath("CompanyResources");
        //AccessSettings.StaticResources.TrySetRules(DirectoryAccessRule.Allow(dataPath));
        AppDomain.CurrentDomain.SetData("DXResourceDirectory", contentPath);
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown


    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e)
    {
       Application["TotalUser"] = (int)Application["TotalUser"] + 1;
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        Application["TotalUser"] = (int)Application["TotalUser"] - 1;
    }

</script>
