using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class ServiceManagementReport_RMAReport : System.Web.UI.Page
{
    //SalesInvoicePrint objReport1 = new SalesInvoicePrint();

    RMAPrint objReport = null;

    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;
    Inv_ParameterMaster objParam = null;
    Inv_ProductionRequestDetail objReqDetail = null;
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

        RMAPrint objReport = new RMAPrint(Session["DBConnection"].ToString());

        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objReqDetail = new Inv_ProductionRequestDetail(Session["DBConnection"].ToString());
        objProductionBom = new Production_BOM(Session["DBConnection"].ToString());
        objProductionEmployee = new Production_Employee(Session["DBConnection"].ToString());
        objstockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjShipExpDetail = new Inv_ShipExpDetail(Session["DBConnection"].ToString());

        GetReport();


    }

    void GetReport()
    {
        DataTable Dt = new DataTable();
        ServiceManagementDataset ObjServiceDataset = new ServiceManagementDataset();
        ObjServiceDataset.EnforceConstraints = false;

        ServiceManagementDatasetTableAdapters.sp_SM_GetPass_Header_SelectRow_ReportTableAdapter adp = new ServiceManagementDatasetTableAdapters.sp_SM_GetPass_Header_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjServiceDataset.sp_SM_GetPass_Header_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()),0);
        Dt = ObjServiceDataset.sp_SM_GetPass_Header_SelectRow_Report;

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
            objReport.DataMember = "sp_SM_GetPass_Header_SelectRow_Report";
            rptViewer.Report = objReport;


        }
    }

}