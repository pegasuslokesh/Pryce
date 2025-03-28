using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;

public partial class Production_Report_ProductionFinishReport : System.Web.UI.Page
{
    //SalesInvoicePrint objReport1 = new SalesInvoicePrint();

    ProductionJobOrder objReport = null;
    ProductionVoucher ObjVoucher = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;
    Inv_ParameterMaster objParam = null;
    Inv_ProductionRequestDetail objReqDetail = null;
    Production_Process_Detail objProcessdetail = null;
    Production_BOM objProductionBom = null;
    Production_Employee objProductionEmployee = null;
    Inv_StockBatchMaster objstockBatchMaster = null;
    Inv_ShipExpDetail ObjShipExpDetail = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new ProductionJobOrder(Session["DBConnection"].ToString());
        ObjVoucher = new ProductionVoucher(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objReqDetail = new Inv_ProductionRequestDetail(Session["DBConnection"].ToString());
        objProcessdetail = new Production_Process_Detail(Session["DBConnection"].ToString());
        objProductionBom = new Production_BOM(Session["DBConnection"].ToString());
        objProductionEmployee = new Production_Employee(Session["DBConnection"].ToString());
        objstockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjShipExpDetail = new Inv_ShipExpDetail(Session["DBConnection"].ToString());

        Session["AccordianId"] = "168";
        Session["HeaderText"] = "Production Report";
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
        ProductionDataset ObjProductionDataset = new ProductionDataset();
        ObjProductionDataset.EnforceConstraints = false;

        ProductionDatasetTableAdapters.sp_Inv_Production_Process_SelectRowTableAdapter adp = new ProductionDatasetTableAdapters.sp_Inv_Production_Process_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjProductionDataset.sp_Inv_Production_Process_SelectRow, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()), 5);
        Dt = ObjProductionDataset.sp_Inv_Production_Process_SelectRow;

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
            DetailReportBand ObjOrderDetail = new DetailReportBand();
            DetailReportBand ObjBomdetail = new DetailReportBand();
            DetailReportBand ObjIssuedetail = new DetailReportBand();
            DetailReportBand ObjExpensesdetail = new DetailReportBand();
            DetailReportBand ObjLabourdetail = new DetailReportBand();

            //here we find detail band and set datasource
            if (Request.QueryString["Type"].ToString() == "ORDER")
            {

                ObjOrderDetail = (DetailReportBand)objReport.FindControl("DetailReport", true);
                ObjBomdetail = (DetailReportBand)objReport.FindControl("DetailReport1", true);
                ObjIssuedetail = (DetailReportBand)objReport.FindControl("DetailReport2", true);
                ObjExpensesdetail = (DetailReportBand)objReport.FindControl("DetailReport3", true);
                ObjLabourdetail = (DetailReportBand)objReport.FindControl("DetailReport4", true);
            }
            else
            {
                ObjOrderDetail = (DetailReportBand)ObjVoucher.FindControl("DetailReport", true);
                ObjBomdetail = (DetailReportBand)ObjVoucher.FindControl("DetailReport1", true);
                ObjIssuedetail = (DetailReportBand)ObjVoucher.FindControl("DetailReport2", true);
                ObjExpensesdetail = (DetailReportBand)ObjVoucher.FindControl("DetailReport3", true);
                ObjLabourdetail = (DetailReportBand)ObjVoucher.FindControl("DetailReport4", true);
                ObjExpensesdetail.Visible = false;
            }


            //for order detail
            ObjOrderDetail.DataSource = objProcessdetail.GetRecord_By_RefJobNo(Dt.Rows[0]["Id"].ToString());
            ObjOrderDetail.DataMember = "sp_Inv_Production_Process_Detail_SelectRow";

            //for Bom
            ObjBomdetail.DataSource = objProductionBom.GetRecord_By_RefJobNo(Dt.Rows[0]["Id"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ObjBomdetail.DataMember = "sp_Inv_Production_BOM_SelectRow";

            //for issue detail

            ObjIssuedetail.DataSource = new DataView(objstockBatchMaster.GetStockBatchMasterAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()), "TransType='FO' and TransTypeId=" + Dt.Rows[0]["Id"].ToString().Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
            ObjIssuedetail.DataMember = "sp_Inv_StockBatchMaster_SelectRow";

            //for expenses detail


            ObjExpensesdetail.DataSource = ObjShipExpDetail.Get_ShipExpDetailByInvoiceId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Dt.Rows[0]["Id"].ToString().Trim(), "PF");
            ObjExpensesdetail.DataMember = "sp_Inv_ShipExpDetail_SelectRow";

            //for labour detail

            ObjLabourdetail.DataSource = objProductionEmployee.GetRecord_By_RefJobNo(Dt.Rows[0]["Id"].ToString());
            ObjLabourdetail.DataMember = "sp_Inv_Production_Employee_SelectRow";


            if (Request.QueryString["Type"].ToString() == "ORDER")
            {
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
                objReport.DataMember = "sp_Inv_Production_Process_SelectRow";
                rptViewer.Report = objReport;
            }
            else
            {
                ObjVoucher.setSignature("");
                ObjVoucher.setcompanyAddress(CompanyAddress);
                ObjVoucher.setcompanyname(CompanyName);
                ObjVoucher.SetImage(Imageurl);
                ObjVoucher.setUserName(createdby);
                ObjVoucher.setCompanyArebicName(CompanyName_L);
                ObjVoucher.setCompanyTelNo(Companytelno);
                ObjVoucher.setCompanyFaxNo(CompanyFaxno);
                ObjVoucher.setCompanyWebsite(CompanyWebsite);
                ObjVoucher.DataSource = Dt;
                ObjVoucher.DataMember = "sp_Inv_Production_Process_SelectRow";
                rptViewer.Report = ObjVoucher;

            }
        }
    }
}