using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;
using System.Data;

public partial class ServiceManagementReport_WorkOrderReport : System.Web.UI.Page
{
    //SalesInvoicePrint objReport1 = new SalesInvoicePrint();

    WorkOrderPrint objReport = null;

    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;

    Prj_Project_Tools objProjecttools = null;
    Prj_VisitMaster objVisitMaster = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        Session["AccordianId"] = "168";
        Session["HeaderText"] = "Production Report";

        objReport = new WorkOrderPrint(Session["DBConnection"].ToString());

        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());

        objProjecttools = new Prj_Project_Tools(Session["DBConnection"].ToString());
        objVisitMaster = new Prj_VisitMaster(Session["DBConnection"].ToString());

        GetReport();
        AllPageCode();

    }
    public void AllPageCode()
    {

        //Page.Title = ObjSysParam.GetSysTitle();
        //Session["AccordianId"] = "13";
        //Session["HeaderText"] = "Production";
    }
    void GetReport()
    {
        DataTable Dt = new DataTable();
        ServiceManagementDataset ObjServiceManagementDataset = new ServiceManagementDataset();
        ObjServiceManagementDataset.EnforceConstraints = false;

        ServiceManagementDatasetTableAdapters.sp_SM_WorkOrder_Select_Row_ReportTableAdapter adp = new ServiceManagementDatasetTableAdapters.sp_SM_WorkOrder_Select_Row_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjServiceManagementDataset.sp_SM_WorkOrder_Select_Row_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
        Dt = ObjServiceManagementDataset.sp_SM_WorkOrder_Select_Row_Report;

        if (Dt.Rows.Count > 0)
        {

            string CompanyName = "";
            string CompanyAddress = "";
            string Imageurl = "";

            string CompanyName_L = "";
            string Companytelno = string.Empty;
            string CompanyFaxno = string.Empty;
            string CompanyWebsite = string.Empty;
            Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
            string ParameterType = string.Empty;
            string ParameterValue = string.Empty;
            string[] strParam = Common.ReportHeaderSetup("Location", Session["LocId"].ToString(), Session["DBConnection"].ToString());
            CompanyName = strParam[0].ToString();
            CompanyName_L = strParam[1].ToString();
            CompanyAddress = strParam[2].ToString();
            Companytelno = strParam[3].ToString();
            CompanyFaxno = strParam[4].ToString();
            CompanyWebsite = strParam[5].ToString();
            Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();
            DataTable dtemployee = objEmployee.GetEmployeeMasterAllData(Session["CompId"].ToString());

            try
            {
                dtemployee = new DataView(dtemployee, "Emp_Id=" + Session["EmpId"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            EmployeeMaster ObjEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
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
            DetailReportBand ObjItemdetail = new DetailReportBand();
            DetailReportBand objVisitDetail = new DetailReportBand();


            //here we find detail band and set datasource


            ObjItemdetail = (DetailReportBand)objReport.FindControl("DetailReport1", true);
            objVisitDetail = (DetailReportBand)objReport.FindControl("DetailReport2", true);


            DataTable dtTools = objProjecttools.GetRecordByProjectId(Request.QueryString["Id"].ToString());
            try
            {
                dtTools = new DataView(dtTools, "Field1='WORK'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }


            if (dtTools.Rows.Count > 0)
            {
                ObjItemdetail.Visible = true;
            }
            else
            {
                ObjItemdetail.Visible = false;
            }

            //for Item detail
            ObjItemdetail.DataSource = dtTools;
            ObjItemdetail.DataMember = "sp_Prj_Project_Tools_Select_Row";



            //for visit
            objVisitDetail.DataSource = objVisitMaster.GetRecord_By_RefType_and_RefId("WORK", Request.QueryString["Id"].ToString());
            objVisitDetail.DataMember = "sp_Prj_VisitMaster_Select_Row";




            objReport.setSignature("");
            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName);
            objReport.SetImage(Imageurl);
            objReport.setUserName(createdby);
            objReport.setCompanyArebicName(CompanyName_L);
            objReport.setCompanyTelNo(Companytelno);
            objReport.setCompanyFaxNo(CompanyFaxno);
            objReport.setCompanyWebsite(CompanyWebsite);
            objReport.DataSource = Dt;
            objReport.DataMember = "sp_SM_WorkOrder_Select_Row_Report";
            rptViewer.Report = objReport;



        }
    }
}