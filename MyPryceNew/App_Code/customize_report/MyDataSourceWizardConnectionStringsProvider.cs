using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Web;
using System.Collections.Generic;
using System.Web;
using System.Linq;
// ... 

public class MyDataSourceWizardConnectionStringsProvider : IDataSourceWizardConnectionStringsProvider
{
    public Dictionary<string, string> GetConnectionDescriptions()
    {
        Dictionary<string, string> connections = AppConfigHelper.GetConnections().Keys.ToDictionary(x => x, x => x);
        connections.Remove("LocalSqlServer");
        //connections.Remove("PegaConnection");
        //connections.Add("CustomSqlConnection", "Custom SQL Connection");    
        return connections;
    }

    public DataConnectionParametersBase GetDataConnectionParameters(string name)
    {
        try
        {
            // Return custom connection parameters for the custom connection(s).  
            if (name == "CustomMdbConnection")
            {
                return new Access97ConnectionParameters("|DataDirectory|nwind.mdb", "", "");
            }
            else if (name == "CustomSqlConnection")
            {
                return new MsSqlConnectionParameters("PRYCE-TL", "dummy", "sa", "123", MsSqlAuthorizationType.SqlServer);
            }
            else if (name == "PegaConnection")
            {

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



                return new MsSqlConnectionParameters(DataSource, CatlogName, UserName, Password, MsSqlAuthorizationType.SqlServer);
            }
            return AppConfigHelper.LoadConnectionParameters(name);
        }
        catch
        {
            return AppConfigHelper.LoadConnectionParameters(name);
        }

    }
}