using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraReports.UI;

public partial class ProjectManagement_Report_Final_Acceptance : System.Web.UI.Page
{
    Prj_ProjectTask objProjecttask = null;
    Final_Acceptance_Report objReport = null;
    Training_Print objTrainingReport = null;
    Prj_ProjectMaster objProjctMaster = null;
    SM_WorkOrder ObjWorkOrder = null;
    SystemParameter Objsys = null;
    EmployeeMaster ObjEmployee = null;
    Set_ApplicationParameter objAppParam = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objProjecttask = new Prj_ProjectTask(Session["DBConnection"].ToString());
        objReport = new Final_Acceptance_Report(Session["DBConnection"].ToString());
        objTrainingReport = new Training_Print(Session["DBConnection"].ToString());
        objProjctMaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        ObjWorkOrder = new SM_WorkOrder(Session["DBConnection"].ToString());
        Objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());

        Getreport();


    }

    public void Getreport()
    {
        DataTable Dt = new DataTable();

        Dt.Columns.Add("Company_Name");
        Dt.Columns.Add("Contact_name ");
        Dt.Columns.Add("Designation");
        Dt.Columns.Add("MobileNo");
        Dt.Columns.Add("Project");
        Dt.Columns.Add("RefNo");
        Dt.Columns.Add("InvoiceNo");
        Dt.Columns.Add("Invoicedate");
        Dt.Columns.Add("ProjectLeadname");
        Dt.Columns.Add("Remarks");
        Dt.Columns.Add("CompanyAddress");

        DataTable dtReport = new DataTable();


        if (Request.QueryString["Type"].ToString().Trim() == "P")
        {

            DataTable dtTask = objProjecttask.GetDataProjectId(Request.QueryString["Id"].ToString());

            dtTask = new DataView(dtTask, "ParentTaskId='0' and Status<>'Cancelled'", "", DataViewRowState.CurrentRows).ToTable();

            Session["DtprojectTask"] = dtTask;

            dtReport = objProjctMaster.GetAllProjectMasteer();
            dtReport = new DataView(dtReport, "Project_Id=" + Request.QueryString["Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


            DataRow dr = Dt.NewRow();

            dr[0] = dtReport.Rows[0]["Name"].ToString();
            dr[1] = dtReport.Rows[0]["ContactName"].ToString();
            dr[2] = dtReport.Rows[0]["Designation"].ToString();
            dr[3] = dtReport.Rows[0]["MobileNo"].ToString();
            dr[4] = dtReport.Rows[0]["Project_Name"].ToString();
            dr[5] = dtReport.Rows[0]["PoRef"].ToString();

            if (dtReport.Rows[0]["InvoiceNo"].ToString().Trim() == "")
            {
                dr[6] = dtReport.Rows[0]["OrderNo"].ToString();
            }
            else
            {
                dr[6] = dtReport.Rows[0]["InvoiceNo"].ToString();
            }
            try
            {
                dr[7] = Convert.ToDateTime(dtReport.Rows[0]["Invoice_date"].ToString()).ToString(Objsys.SetDateFormat());
            }
            catch
            {
                dr[7] = "";
            }
            dr[8] = dtReport.Rows[0]["TeamLeader"].ToString();
            try
            {
                dr[9] = objAppParam.GetApplicationParameterByParamName("Final_acceptance_Terms", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Description"].ToString();
            }
            catch
            {
                dr[9] = "";
            }
            dr[10] = dtReport.Rows[0]["Customer_Id"].ToString();

            Dt.Rows.Add(dr);
        }
        else
        {
            Session["DtprojectTask"] = null;


            dtReport = ObjWorkOrder.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Request.QueryString["Id"].ToString());

            DataRow dr = Dt.NewRow();

            dr[0] = dtReport.Rows[0]["CustomerName"].ToString();
            dr[1] = dtReport.Rows[0]["ContactPersonName"].ToString();
            dr[2] = dtReport.Rows[0]["Designation"].ToString();
            dr[3] = dtReport.Rows[0]["MobileNo"].ToString();
            dr[4] = dtReport.Rows[0]["Remarks"].ToString();
            dr[5] = dtReport.Rows[0]["work_order_No"].ToString();
            dr[6] = dtReport.Rows[0]["Invoice_No"].ToString();
            try
            {
                dr[7] = Convert.ToDateTime(dtReport.Rows[0]["Invoice_date"].ToString()).ToString(Objsys.SetDateFormat());
            }
            catch
            {
                dr[7] = "";
            }
            dr[8] = dtReport.Rows[0]["EmployeeName"].ToString();
            try
            {
                dr[9] = objAppParam.GetApplicationParameterByParamName("Final_acceptance_Terms", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Description"].ToString();
            }
            catch
            {
                dr[9] = "";
            }

            dr[10] = dtReport.Rows[0]["Customer_Id"].ToString();

            Dt.Rows.Add(dr);


        }

        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string Title = "";
        string CompanyName_L = "";
        string Companytelno = string.Empty;
        string CompanyFaxno = string.Empty;
        string CompanyWebsite = string.Empty;

        string[] strParam = Common.ReportHeaderSetup("Company", Session["CompId"].ToString(), Session["DBConnection"].ToString());


        CompanyName = strParam[0].ToString();
        CompanyName_L = strParam[1].ToString();
        CompanyAddress = strParam[2].ToString();
        Companytelno = strParam[3].ToString();
        CompanyFaxno = strParam[4].ToString();
        CompanyWebsite = strParam[5].ToString();
        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();


        string createdby = string.Empty;
        if (Session["EmpId"].ToString().Trim() == "0")
        {
            createdby = "SuperAdmin";
        }
        else
        {
            try
            {
                createdby = ObjEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()).Rows[0]["Emp_Name"].ToString();
            }
            catch
            {
            }

        }


        if (Request.QueryString["Type"].ToString().Trim() == "P" || Request.QueryString["Type"].ToString().Trim() == "W")
        {
            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName);
            objReport.settitle("FINAL ACCEPTANCE REPORT");
            objReport.SetImage(Imageurl);
            objReport.setUserName(createdby);
            objReport.setCompanyArebicName(CompanyName_L);
            objReport.setCompanyTelNo(Companytelno);
            objReport.setCompanyFaxNo(CompanyFaxno);
            objReport.setCompanyWebsite(CompanyWebsite);
            objReport.DataSource = Dt;
            objReport.DataMember = "DtFinalAccceptance";
            rptViewer.Report = objReport;
            rptToolBar.ReportViewer = rptViewer;
        }
        else
        {
            objTrainingReport.setcompanyAddress(CompanyAddress);
            objTrainingReport.setcompanyname(CompanyName);
            objTrainingReport.settitle("TRAINING FORM");
            objTrainingReport.SetImage(Imageurl);
            objTrainingReport.setUserName(createdby);
            objTrainingReport.setCompanyArebicName(CompanyName_L);
            objTrainingReport.setCompanyTelNo(Companytelno);
            objTrainingReport.setCompanyFaxNo(CompanyFaxno);
            objTrainingReport.setCompanyWebsite(CompanyWebsite);
            objTrainingReport.DataSource = Dt;
            objTrainingReport.DataMember = "DtFinalAccceptance";
            rptViewer.Report = objTrainingReport;
            rptToolBar.ReportViewer = rptViewer;
        }

    }
}