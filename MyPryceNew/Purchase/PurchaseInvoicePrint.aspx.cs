using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Purchase_PurchaseInvoicePrint : System.Web.UI.Page
{
    PurchaseInvoicePrint objReport = null;
    PurchaseInvoicePrint_Internal objReport_Internal = null;

    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objReport = new PurchaseInvoicePrint(Session["DBConnection"].ToString());
        objReport_Internal = new PurchaseInvoicePrint_Internal(Session["DBConnection"].ToString());

        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());

        GetReport();
        AllPageCode();
    }
    public void AllPageCode()
    {

        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "12";
        Session["HeaderText"] = "Purchase";
    }
    void GetReport()
    {

        PurchaseDataSet objPurchasedataset = new PurchaseDataSet();
        objPurchasedataset.EnforceConstraints = false;
        PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(objPurchasedataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
        DataTable Dt = objPurchasedataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report;
       

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
       
        EmployeeMaster objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
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

        }
       

        Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());

        string ParameterType = string.Empty;
        string ParameterValue = string.Empty;

        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Purchase Report Header").Rows[0]["ParameterValue"].ToString() == "Company Level")
        {
            ParameterType = "Company";
            ParameterValue = Session["CompId"].ToString();
        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Purchase Report Header").Rows[0]["ParameterValue"].ToString() == "Brand Level")
        {
            ParameterType = "Brand";
            ParameterValue = Session["BrandId"].ToString();
        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Purchase Report Header").Rows[0]["ParameterValue"].ToString() == "Location Level")
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



        Title = "PURCHASE INVOICE";
       

      

        string createdby = string.Empty;
        if (Session["EmpId"].ToString().Trim() == "0")
        {
            createdby = "SuperAdmin";
        }
        else
        {
            try
            {
                createdby = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()).Rows[0]["Emp_Name"].ToString();
            }
            catch
            {
            }

        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseInvoiceReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
        {
            if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString() == "Customer Format 1")
                objReport.setTax_Title("VAT ");
            else
                objReport.setTax_Title("TAX ");
            DataTable Tax_Dt = objInvParam.Get_Location_GST_TRN_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0");
            if (Tax_Dt != null && Tax_Dt.Rows.Count > 0)
            {
                objReport.setLoc_TRN_Title(Tax_Dt.Rows[0]["Registration_Type"].ToString() + "         : ", Tax_Dt.Rows[0]["Registration_No"].ToString());
            }
            else
            {
                objReport.setLoc_TRN_Title("", "");
            }

            DataTable Cust_Reg_Dt = objInvParam.Get_Supplier_GST_TRN_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Dt.Rows[0]["SupplierId"].ToString(), "0");
            if (Cust_Reg_Dt != null && Cust_Reg_Dt.Rows.Count > 0)
            {
                objReport.setCust_TRN_Title(Cust_Reg_Dt.Rows[0]["Registration_Type"].ToString() + "         : ", Cust_Reg_Dt.Rows[0]["Registration_No"].ToString());
            }
            else
            {
                objReport.setCust_TRN_Title("", "");
            }

            objReport.setUserName(createdby);
            objReport.setSignature(signatureurl);
            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName);
            //objReport.setBrandName(BrandName);
            objReport.setLocationName(LocationName);
            objReport.settitle(Title);
            objReport.SetImage(Imageurl);
            objReport.setCompanyArebicName(CompanyName_L);
            objReport.setCompanyTelNo(Companytelno);
            objReport.setCompanyFaxNo(CompanyFaxno);
            objReport.setCompanyWebsite(CompanyWebsite);
            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Inv_PurchaseInvoiceHeader_SelectRow_Report";
            rptViewer.Report = objReport;
        }
        else
        {
            objReport_Internal.setUserName(createdby);
            objReport_Internal.setSignature(signatureurl);
            
            objReport_Internal.setcompanyname(CompanyName);
            //objReport.setBrandName(BrandName);
            objReport_Internal.setLocationName(LocationName);
            objReport_Internal.settitle(Title);
            objReport_Internal.SetImage(Imageurl);
            objReport_Internal.setCompanyArebicName(CompanyName_L);

            objReport_Internal.DataSource = Dt;
            objReport_Internal.DataMember = "sp_Inv_PurchaseInvoiceHeader_SelectRow_Report";
            rptViewer.Report = objReport_Internal;


        }



        rptToolBar.ReportViewer = rptViewer;
        //objReport.ExportToPdf(Server.MapPath("~/Temp/Purchase Invoice-" + Dt.Rows[0]["InvoiceNo"].ToString() + ".pdf"));
        //ViewState["Path"] = "Purchase Invoice-" + Dt.Rows[0]["InvoiceNo"].ToString() + ".pdf";
        ViewState["Id"] = Request.QueryString["Id"].ToString();
        ViewState["SupId"] = Dt.Rows[0]["SupplierId"].ToString();

        Ems_ContactMaster objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());

        Set_Suppliers objSup = new Set_Suppliers(Session["DBConnection"].ToString());

        string SupplierCode = string.Empty;

        try
        {
            DataTable dtSup = objSup.GetSupplierAllDataBySupplierIdWithoutBrand(Session["CompId"].ToString(), Dt.Rows[0]["SupplierId"].ToString());
            if (dtSup.Rows.Count > 0)
            {
                if ((dtSup.Rows[0]["Status"].ToString() == "Individual") && (dtSup.Rows[0]["Company_Id1"].ToString() != "0"))
                {
                    DataTable dtCon = objContact.GetContactTrueById(dtSup.Rows[0]["Company_Id1"].ToString());
                    SupplierCode = dtSup.Rows[0]["Supplier_Code"].ToString() + "/" + dtCon.Rows[0]["Name"].ToString();
                }
                else
                {
                    SupplierCode = dtSup.Rows[0]["Supplier_Code"].ToString() + "/" + dtSup.Rows[0]["Name"].ToString();
                }

            }

        }


        catch
        {

        }



        ES_MailSubject esmailsub = new ES_MailSubject(HttpContext.Current.Session["DBConnection_ES"].ToString());
        string StringSubject = string.Empty;
        StringSubject = esmailsub.GetEmailSubjectAll(Session["CompId"].ToString(), true, "12", "48", SupplierCode.ToString(), Dt.Rows[0]["InvoiceNo"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (StringSubject == "")
        {
            StringSubject = "Purchase Invoice -" + Dt.Rows[0]["InvoiceNo"].ToString();
            Session["MailSubject"] = StringSubject;

        }
        else
        {
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
        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseInvoiceReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
        {
            objReport.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
        }
        else
        {
            objReport_Internal.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
       
        }
        ViewState["Path"] = StringSubject.Trim() + ".pdf";



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

        Response.Redirect("../EmailSystem/ContactMailReference.aspx?Page=PINV&&Url=" + ViewState["Path"].ToString() + "&&Id=" + ViewState["Id"].ToString() + "&&ConId=" + ViewState["SupId"].ToString() + "&&ProductId=" + ViewState["ProductId"].ToString() + "&&PageRefType=PINV&&PageRefId=" + Request.QueryString["Id"].ToString());

    }
}
