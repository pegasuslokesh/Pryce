using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraEditors.Controls.Rtf;

public partial class ProjectManagement_Report_PrjScrumReport : System.Web.UI.Page
{
    Prj_ScrumMasterReport Obj = null;
    Prj_ScrumMaster ObjClass = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Obj = new Prj_ScrumMasterReport(Session["DBConnection"].ToString());
        ObjClass = new Prj_ScrumMaster(Session["DBConnection"].ToString());
        GetReport();
    }
    public void GetReport()
    {
        DataTable dtTask = ObjClass.GetScrumReport(Request.QueryString["Id"].ToString());
        Obj.DataSource = dtTask;
        Obj.DataMember = "sp_Prj_ScrumMaster_SelectRow_Report";
        rptViewer.Report = Obj;
        rptToolBar.ReportViewer = rptViewer;
    }
}