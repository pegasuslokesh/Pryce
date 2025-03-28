using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Accounts_Report_TrialBalanceReport : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    TrialBalancePrint objTrialBalanceprint = null;
    CompanyMaster Objcompany = null;
    string strCompId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null || Session["dtTrialBalance"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objTrialBalanceprint = new TrialBalancePrint(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());

        GetReport();
    }
    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        string CompanyName = "";
        string Imageurl = "";

        DataTable dt = (DataTable)Session["dtTrialBalance"];
        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();
        }
        objTrialBalanceprint.DataSource = dt;
        objTrialBalanceprint.DataMember = "sp_Ac_ChartOfAccount_TrialBalance_Report";
        ReportViewer1.Report = objTrialBalanceprint;
        ReportToolbar1.ReportViewer = ReportViewer1;
        objTrialBalanceprint.setcompanyname(CompanyName);
        objTrialBalanceprint.SetImage(Imageurl);
    }
}