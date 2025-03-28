using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class Inventory_Report_StockAdjustment_Print : System.Web.UI.Page
{
    StockAdjustmentPrint objReport = null;
    CompanyMaster Objcompany = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objReport = new StockAdjustmentPrint(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        GetReport();

    }

    public void GetReport()
    {
        string strCompanyName = string.Empty;

        string strCompanyLogo = string.Empty;

        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());

        if (DtCompany.Rows.Count > 0)
        {
            strCompanyName = DtCompany.Rows[0]["Company_Name"].ToString();

            strCompanyLogo = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();
        }

        string strReportTitle = string.Empty;

        ArrayList objarr = (ArrayList)Session["dtAdjustmentReport"];
        DataTable dt = (DataTable)objarr[2];

        objReport.DataSource = dt;
        objReport.DataMember = "sp_Inv_StockAdjustment_SelectRow_Report";
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;
        objReport.setReportTitle((string)objarr[0]);
        objReport.setDateCriteria((string)objarr[1]);
        objReport.setcompanyname(strCompanyName);
        objReport.SetImage(strCompanyLogo);

    }
}