using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Parameters;
using PegasusDataAccess;
using System.Configuration;

public partial class Sales_SalesInvoicePrint : System.Web.UI.Page
{
    //SalesInvoicePrint objReport1 = new SalesInvoicePrint();
    Sales_Invoice_Print objReport = null;
    Sales_Invoice_Print_Internal objReport_Internal = null;
    //SalesInvoicePrint2 objReportCustFormat1 = null;
    SalesInvoicePrint_Taxable_PosScan objReportCustFormat1 = null;
    SalesInvoicePrint_Taxable objReportTaxable = null;
    iLabelSalesInvoicePrint ObjIlabelReport = null;
    KuwaitSalesInvoicePrint ObjKuwaitFormat = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;
    Inv_ParameterMaster objParam = null;
    DataAccessClass objDa = null;
     
    protected void Page_Load(object sender, EventArgs e)
    {
        objReport = new Sales_Invoice_Print(Session["DBConnection"].ToString());
        objReport_Internal = new Sales_Invoice_Print_Internal(Session["DBConnection"].ToString());
        //objReportCustFormat1 = new SalesInvoicePrint2(Session["DBConnection"].ToString());
        objReportCustFormat1 = new SalesInvoicePrint_Taxable_PosScan(Session["DBConnection"].ToString());
        objReportTaxable = new SalesInvoicePrint_Taxable(Session["DBConnection"].ToString());
        ObjIlabelReport = new iLabelSalesInvoicePrint(Session["DBConnection"].ToString());
        ObjKuwaitFormat = new KuwaitSalesInvoicePrint(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());

        // ObjKuwaitFormat.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "KuwaitSalesInvoicePrint.repx");
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        
        GetReport();
        AllPageCode();

    }
    public void AllPageCode()
    {

        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "13";
        Session["HeaderText"] = "Sales";
    }
    void GetReport()
    {
        DataTable Dt = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;

        SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
        Dt = ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report;
        //try
        //{
        //    Dt = new DataView(Dt, "Invoice_Id=" + Request.QueryString["Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //}
        //catch
        //{
        //}

        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string FromDate = "";
        string Todate = "";
        string Title = "";
        string CompanyName_L = "";
        string Companytelno = string.Empty;
        string CompanyFaxno = string.Empty;
        string CompanyWebsite = string.Empty;
        string CompanyGSTIN = string.Empty;
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
        //CompanyGSTIN = strParam[7].ToString();
        Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();
        DataTable dtemployee = objEmployee.GetEmployeeMasterAllData(Session["CompId"].ToString());

        try
        {
            dtemployee = new DataView(dtemployee, "Emp_Id=" + Session["EmpId"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        string sqlQuery = "select top 1 field4 from set_locationmaster where location_id=" + Session["LocId"].ToString();
        //string CompanyGSTIN = string.Empty;
        
        CompanyGSTIN = objDa.get_SingleValue(sqlQuery);
        //CompanyGSTIN = strParam[7].ToString();

        string signatureurl = string.Empty;
        if (dtemployee.Rows.Count > 0)
        {


            signatureurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtemployee.Rows[0]["Field3"].ToString();

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
        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString().Trim()=="System Format")
        {
            Title = "SALES INVOICE";
           
           

            if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesInvoiceReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
            {
               objReport.setSignature(signatureurl);

                objReport.setcompanyAddress(CompanyAddress);
                objReport.setcompanyname(CompanyName);
                objReport.settitle(Title);
                objReport.SetImage(Imageurl);
                objReport.setUserName(createdby);
                objReport.setCompanyArebicName(CompanyName_L);
                objReport.setCompanyTelNo(Companytelno);
                objReport.setCompanyFaxNo(CompanyFaxno);
                objReport.setCompanyWebsite(CompanyWebsite);

                objReport.DataSource = Dt;
                objReport.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
                objReport.CreateDocument();
                rptViewer.OpenReport(objReport);

                //rptViewer.Report = objReport;
            }
            else
            {
                objReport_Internal.setSignature(signatureurl);





                objReport_Internal.setcompanyname(CompanyName);

                objReport_Internal.settitle(Title);
                objReport_Internal.SetImage(Imageurl);

                objReport_Internal.setUserName(createdby);

                //objReport.setUserName(Session["UserId"].ToString());
                objReport_Internal.setCompanyArebicName(CompanyName_L);


                objReport_Internal.DataSource = Dt;
                objReport_Internal.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
                objReport_Internal.CreateDocument();
                rptViewer.OpenReport( objReport_Internal);

            }
            //rptToolBar.ReportViewer = rptViewer;
        }

        else if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString().Trim() == "Taxable Report")
        {

            Title = "TAX INVOICE";
            objReportTaxable.setcompanyname(CompanyName);
            objReportTaxable.settitle(Title);
            objReportTaxable.SetImage(Imageurl);
            objReportTaxable.setcompanyAddress(CompanyAddress);
            objReportTaxable.setCompanyTelNo(Companytelno);
            objReportTaxable.setCompanyFaxNo(CompanyFaxno);
            objReportTaxable.setCompanyWebsite(CompanyWebsite);
            objReportTaxable.setCompanyArebicName(CompanyName_L);
            objReportTaxable.setCompanyGSTIN(CompanyGSTIN);
            objReportTaxable.DataSource = Dt;
            objReportTaxable.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
            objReportTaxable.CreateDocument();
            rptViewer.OpenReport(objReportTaxable);
            //rptToolBar.ReportViewer = rptViewer;
           

        }
        else if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString() == "Customer Format 1")
        {
            objReportCustFormat1.DataSource = Dt;
            objReportCustFormat1.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
           
            //rptViewer.Report = objReportCustFormat1;
            //rptToolBar.ReportViewer = rptViewer;
            if (Dt.Rows.Count > 0)
            {
                //objReportCustFormat1.setReportTitle(Dt.Rows[0]["InvoiceTitle"].ToString().ToUpper());
                //if (Dt.Rows[0]["Cust_TAX_No"].ToString() != "")
                //    objReportCustFormat1.setCust_TRN_Title("TRN : " + Dt.Rows[0]["Cust_TAX_No"].ToString().ToUpper());
                //else
                //    objReportCustFormat1.setCust_TRN_Title("");
            }
            else
            {
                //objReportCustFormat1.setReportTitle("CREDIT");
                //if (Dt.Rows[0]["Cust_TAX_No"].ToString() != "")
                //    objReportCustFormat1.setCust_TRN_Title("TRN : " + Dt.Rows[0]["Cust_TAX_No"].ToString().ToUpper());
                //else
                //    objReportCustFormat1.setCust_TRN_Title("");
            }

            objReportCustFormat1.CreateDocument();
            rptViewer.OpenReport(objReportCustFormat1);
        }
        else if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString() == "iLabel Format")
        {
            ObjIlabelReport.DataSource = Dt;
            ObjIlabelReport.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
            

            //rptViewer.Report = ObjIlabelReport;
            //rptToolBar.ReportViewer = rptViewer;

            if (Dt.Rows.Count > 0)
            {
                ObjIlabelReport.setReportTitle(Dt.Rows[0]["InvoiceTitle"].ToString().ToUpper() + " INVOICE");
            }
            else
            {
                ObjIlabelReport.setReportTitle("CREDIT INVOICE");
            }
            ObjIlabelReport.CreateDocument();
            rptViewer.OpenReport(ObjIlabelReport);
        }
        else if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString() == "Kuwait Format")
        {
            ObjKuwaitFormat.DataSource = Dt;
            ObjKuwaitFormat.DataMember = "sp_Inv_SalesInvoiceDetail_SelectRow_Report";
            if (Dt.Rows.Count > 0)
            {
                ObjKuwaitFormat.setReportTitle(Dt.Rows[0]["InvoiceTitle"].ToString().ToUpper() + " INVOICE");
            }
            else
            {
                ObjKuwaitFormat.setReportTitle("CREDIT INVOICE");
            }
            ObjKuwaitFormat.CreateDocument();
            rptViewer.OpenReport(ObjKuwaitFormat);
            //rptViewer.Report = ObjKuwaitFormat;
            //rptToolBar.ReportViewer = rptViewer;

            
        }

       // objReport.ExportToPdf(Server.MapPath("~/Temp/Sales Invoice-" + Dt.Rows[0]["Invoice_No"].ToString() + ".pdf"));
       // ViewState["Path"] = "Sales Invoice-" + Dt.Rows[0]["Invoice_No"].ToString() + ".pdf";
        ViewState["Id"] = Dt.Rows[0]["Invoice_Id"].ToString();
        ViewState["CustId"] = Dt.Rows[0]["Customer_Id"].ToString();

        //Code for Subject 

        Set_CustomerMaster objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        Ems_ContactMaster objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());

        string customercode = string.Empty;

        try
        {
            DataTable dtCust = objCustomer.GetCustomerAllDataByCustomerIdWithOutBrand(Session["CompId"].ToString(), Dt.Rows[0]["Customer_Id"].ToString());
            if (dtCust.Rows.Count > 0)
            {
                if ((dtCust.Rows[0]["Status"].ToString() == "Individual") && (dtCust.Rows[0]["Company_Id1"].ToString() != "0"))
                {
                    DataTable dtCon = objContact.GetContactTrueById(dtCust.Rows[0]["Company_Id1"].ToString());
                    customercode = dtCust.Rows[0]["Customer_Code"].ToString() + "/" + dtCon.Rows[0]["Name"].ToString();
                }
                else
                {
                    customercode = dtCust.Rows[0]["Customer_Code"].ToString() + "/" + dtCust.Rows[0]["Name"].ToString();
                }

                string CustomerGSTIN = dtCust.Rows[0]["GSTIN_NO"].ToString();
                objReportTaxable.setCustomerGSTIN(CustomerGSTIN);
            }

        }
        catch
        {

        }
        //ES_MailSubject esmailsub = new ES_MailSubject(Session["DBConnection_ES"].ToString());
        //string StringSubject = string.Empty;
        //StringSubject = esmailsub.GetEmailSubjectAll(Session["CompId"].ToString(), true, "13", "92", customercode.ToString(), Dt.Rows[0]["Invoice_No"].ToString(),Session["EmpId"].ToString());
        //if (StringSubject == "")
        //{
        //    StringSubject = "Sales Invoice-" + Dt.Rows[0]["Invoice_No"].ToString();
        //    Session["MailSubject"] = StringSubject;

        //}

        string StringSubject = string.Empty;
        Session["MailSubject"] = StringSubject;

        
        try
        {


                StringSubject = StringSubject.Replace("/", "").Trim();
                StringSubject = StringSubject.Replace("<", "").Trim();
                StringSubject = StringSubject.Replace(">", "").Trim();
                StringSubject = StringSubject.Replace(":", "").Trim();
                StringSubject = StringSubject.Replace("*", "").Trim();
                StringSubject = StringSubject.Replace("?", "").Trim();
                StringSubject = StringSubject.Replace("|", "").Trim();
                StringSubject = StringSubject.Replace(",", "").Trim();
                StringSubject = StringSubject.Replace("&", "").Trim();
                
                Session["MailSubject"] = StringSubject;

                
            }
            catch
            {

            }
        
        if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString().Trim() == "System Format")
        {
            if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesInvoiceReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
            {
                objReport.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
            }
            else
            {
                objReport_Internal.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
            }
        }
        else if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString().Trim() == "Taxable Report")
        {
            objReportTaxable.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
    
        }
        else if (objParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString().Trim() == "Customer Format 1")
        {
            objReportCustFormat1.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
        }
        else if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString().Trim() == "iLabel Format")
        {
            ObjIlabelReport.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
        }
        ViewState["Path"] = StringSubject.Trim() + ".pdf";
        //End Code
        string ProductId = string.Empty;
        foreach (DataRow dr in Dt.Rows)
        {
            if (ProductId == "")
            {
                ProductId = dr["ProductCode"].ToString();
            }
            else
            {
                ProductId = ProductId + "," + dr["ProductCode"].ToString();

            }

        }
        ViewState["ProductId"] = ProductId;

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {

        Response.Redirect("../EmailSystem/ContactMailReference.aspx?Page=SINV&&Url=" + ViewState["Path"].ToString() + "&&Id=" + ViewState["Id"].ToString() + "&&ConId=" + ViewState["CustId"].ToString() + "&&ProductId=" + ViewState["ProductId"].ToString() + "&&PageRefType=SINV&&PageRefId=" + ViewState["Id"].ToString());
    
    }
}
