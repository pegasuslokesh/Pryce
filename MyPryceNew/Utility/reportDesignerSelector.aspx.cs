using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CatalogDataTableAdapters;


public partial class reportDesignerSelector : System.Web.UI.Page
{
    private CatalogData catalogDataSet;
    private DataTable reportsTable;
    private CatalogDataTableAdapters.sys_reportsTableAdapter reportsTableAdapter;
    protected void Page_Load(object sender, EventArgs e)
    {
        catalogDataSet = new CatalogData();
        reportsTableAdapter = new sys_reportsTableAdapter();
        reportsTableAdapter.Connection.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
        reportsTableAdapter.Fill(catalogDataSet.sys_reports);
        reportsTable = catalogDataSet.Tables["Reports"];
        if (!IsPostBack)
        {
            reportsList.DataSource = catalogDataSet;
            reportsList.DataMember = "Reports";
            reportsList.DataTextField = "DisplayName";
            reportsList.DataValueField = "ReportID";
            this.DataBind();
        }
    }
    protected void NewReportButton_Click(object sender, EventArgs e)
    {
        Session["DesignerTask"] = new DesignerTask
        {
            mode = ReportEdditingMode.NewReport,
        };
        Response.Redirect("reportDesigner.aspx");
    }


    protected void EditButton_Click(object sender, EventArgs e)
    {
        ListItem selected = reportsList.SelectedItem;
        if (selected != null)
        {
            Session["DesignerTask"] = new DesignerTask
            {
                mode = ReportEdditingMode.ModifyReport,
                reportID = selected.Value
            };
            Session["ReportID"] = selected.Value;
            Response.Redirect("reportDesigner.aspx");
        }
    }


    protected void DeleteButton_Click(object sender, EventArgs e)
    {
        ListItem selected = reportsList.SelectedItem;

        if (selected != null)
        {
            DataRow row = reportsTable.Rows.Find(int.Parse(selected.Value));
            if (row != null)
            {
                row.Delete();
                reportsTableAdapter.Update(catalogDataSet);
                catalogDataSet.AcceptChanges();
            }
            reportsList.Items.Remove(reportsList.SelectedItem);
        }
    }
}