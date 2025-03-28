using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraEditors.Controls.Rtf;
public partial class ProjectManagement_Report_PrjScrumStaticsReport : System.Web.UI.Page
{
    Prj_ScrumStaticsReport Obj = null;
    Prj_ScrumMaster ObjClass = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        Obj = new Prj_ScrumStaticsReport(Session["DBConnection"].ToString());
        ObjClass = new Prj_ScrumMaster(Session["DBConnection"].ToString());
        GetReport();
    }
    public void GetReport()
    {
        if (Request.QueryString["Id"].ToString() != "All")
        {
            string MonthValue = Request.QueryString["Id"].ToString();
            DataTable dtTask = ObjClass.GetScrumStaticsReport(HttpContext.Current.Session["LocId"].ToString());           
            dtTask = new DataView(dtTask, "Month='" + MonthValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            Obj.DataSource = dtTask;
            Obj.DataMember = "sp_EmployeeScrum_Statics_Report";
            rptViewer.Report = Obj;
            rptToolBar.ReportViewer = rptViewer;
        }
        else
        {
            DataTable dtTask = ObjClass.GetScrumStaticsReport(HttpContext.Current.Session["LocId"].ToString());            
            Obj.DataSource = dtTask;
            Obj.DataMember = "sp_EmployeeScrum_Statics_Report";
            rptViewer.Report = Obj;
            rptToolBar.ReportViewer = rptViewer;
        }        
    }
}