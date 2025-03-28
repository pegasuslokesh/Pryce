using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sales_Report_SalesCommission : System.Web.UI.Page
{
    SalesCommissionReport objReport = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new SalesCommissionReport(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());

        if (Session["DtCommissionReport"] != null)
        {

            GetReport();
        }


    }

    void GetReport()
    {
        DataTable Dt = new DataTable();


        Dt = (DataTable)Session["DtCommissionReport"];


        //SalesDataSet ObjSalesDataset = new SalesDataSet();
        //ObjSalesDataset.EnforceConstraints = false;


        //SalesDataSetTableAdapters.sp_Inv_SalesDeliveryVoucher_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesDeliveryVoucher_SelectRow_ReportTableAdapter();


        //adp.Fill(ObjSalesDataset.sp_Inv_SalesDeliveryVoucher_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
        //Dt = ObjSalesDataset.sp_Inv_SalesDeliveryVoucher_SelectRow_Report;

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

        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Report Header").Rows[0]["ParameterValue"].ToString() == "Company Level")
        {
            ParameterType = "Company";
            ParameterValue = Session["CompId"].ToString();
        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Report Header").Rows[0]["ParameterValue"].ToString() == "Brand Level")
        {
            ParameterType = "Brand";
            ParameterValue = Session["BrandId"].ToString();
        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Sales Report Header").Rows[0]["ParameterValue"].ToString() == "Location Level")
        {
            ParameterType = "Location";
            ParameterValue = Session["LocId"].ToString();
        }



        string[] strParam = Common.ReportHeaderSetup(ParameterType, ParameterValue, Session["DBConnection"].ToString());


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
        string signatureurl = string.Empty;
        if (dtemployee.Rows.Count > 0)
        {

            signatureurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtemployee.Rows[0]["Field3"].ToString();
            objReport.setUserName(dtemployee.Rows[0]["Emp_Name"].ToString());
        }
        else
        {
            objReport.setUserName("Superadmin");
        }
        Title = "COMMISSION REPORT";


        //DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objReport.FindControl("GroupHeader2", true);
        //if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DeliveryVoucherReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
        //{
        //    Gh.Visible = true;
        //}
        //else
        //{
        //    Gh.Visible = false;
        //}


        if (Request.QueryString["Type"].ToString().Trim() =="Agent")
        {
            objReport.setPersontype("Agent Name");
            objReport.setReportTitle("AGENT COMMISSION REPORT");
        }
        else
        {
            objReport.setPersontype("Employee Name");
            objReport.setReportTitle("EMPLOYEE COMMISSION REPORT");
        }
        objReport.setSignature(signatureurl);
        objReport.setcompanyAddress(CompanyAddress);
        objReport.setcompanyname(CompanyName.ToUpper());
        objReport.SetImage(Imageurl);
        objReport.setCompanyArebicName(CompanyName_L);
        objReport.setCompanyTelNo(Companytelno);
        objReport.setCompanyFaxNo(CompanyFaxno);
        objReport.setCompanyWebsite(CompanyWebsite);
        //objReport.setDateCriteria(Session["DtCommissionReport_Range"].ToString());
        objReport.DataSource = Dt;
        objReport.DataMember = "DtSalesCommission";
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;
    }
}