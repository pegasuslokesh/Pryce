using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Web;
// ... 

public class MyDataSourceWizardDBSchemaProviderFactory : IDataSourceWizardDBSchemaProviderExFactory
{
    public IDBSchemaProviderEx Create()
    {
        return new MyDBSchemaProvider();
    }
}