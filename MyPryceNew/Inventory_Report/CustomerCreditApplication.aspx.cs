using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_CustomerCreditApplication : System.Web.UI.Page
{
    CreditApplication objReport = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objReport = new CreditApplication(Session["DBConnection"].ToString());

        GetReport();

    }

    void GetReport()
    {
        DataTable Dt = new DataTable();
        InventoryDataSet ObjInventoryDataset = new InventoryDataSet();
        ObjInventoryDataset.EnforceConstraints = false;

        InventoryDataSetTableAdapters.sp_Set_CustomerMaster_CreditApplication_SelectRowTableAdapter adp = new InventoryDataSetTableAdapters.sp_Set_CustomerMaster_CreditApplication_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjInventoryDataset.sp_Set_CustomerMaster_CreditApplication_SelectRow, Convert.ToInt32(Request.QueryString["Id"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), 0);
        Dt = ObjInventoryDataset.sp_Set_CustomerMaster_CreditApplication_SelectRow;


        string[] strParam = Common.ReportHeaderSetup("Company", Session["CompId"].ToString(), Session["DBConnection"].ToString());



        string AuthorozedSignature = string.Empty;
        try
        {
            AuthorozedSignature = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + Dt.Rows[0]["Signature"].ToString();
        }
        catch
        {


        }
        objReport.setcompanyname(strParam[0].ToString().Trim().ToUpper());
        objReport.SetImage("~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString());
        objReport.setSignature(AuthorozedSignature);
        objReport.DataSource = Dt;
        objReport.DataMember = "sp_Set_CustomerMaster_CreditApplication_SelectRow";
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;

    }
}