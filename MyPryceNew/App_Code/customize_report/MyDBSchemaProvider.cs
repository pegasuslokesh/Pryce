using DevExpress.DataAccess.Sql;
using DevExpress.Xpo.DB;
using System.Linq;
// ... 

public class MyDBSchemaProvider : IDBSchemaProviderEx
{
    DBSchemaProviderEx provider;
    public MyDBSchemaProvider()
    {
        this.provider = new DBSchemaProviderEx();
    }

    public DBTable[] GetTables(SqlDataConnection connection, params string[] tableList)
    {
        return provider.GetTables(connection, tableList)
            //.Where(table => table.Name.StartsWith("C"))
            .ToArray();
    }

    public DBTable[] GetViews(SqlDataConnection connection, params string[] viewList)
    {
        return provider.GetViews(connection, viewList)
           // .Where(view => view.Name.StartsWith("Order"))
            .ToArray();
    }

    public DBStoredProcedure[] GetProcedures(SqlDataConnection connection, params string[] procedureList)
    {
        return provider.GetProcedures(connection, procedureList)
            //.Where(storedProcedure => storedProcedure.Arguments.Count == 0)
            .ToArray();
    }

    public void LoadColumns(SqlDataConnection connection, params DBTable[] tables)
    {
        provider.LoadColumns(connection, tables);
    }
}