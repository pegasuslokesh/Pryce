using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class Sales_SalesQuotationPrint : System.Web.UI.Page
{
    SalesQuotationPrint objReport =null;
    SalesQuotationDetailWithTax objReportWt = null;
    SalesQuotationPrint_Internal objReport_Internal = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;
    Ems_ContactMaster objContact = null;
    DataAccessClass objDa = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new SalesQuotationPrint(Session["DBConnection"].ToString());
        objReportWt = new SalesQuotationDetailWithTax(Session["DBConnection"].ToString());
        objReport_Internal = new SalesQuotationPrint_Internal(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());

        if (Request.QueryString["Id"] != null)
        {
            string Name = objDa.get_SingleValue("Select (case when EC.Field3='0' or EC.Field3 is null then '' else EC.Field3 end)+ ' ' +EC.Name+' -- '+SQuotation_No from Inv_SalesQuotationHeader as SQ inner join Ems_ContactMaster as EC on EC.Trans_Id = SQ.Customer_Id Where SQuotation_Id ='" + Request.QueryString["Id"].ToString() + "'");
            string stringWithoutHyphen = Name.Replace("-", "");
            objReport.SetPrintName(stringWithoutHyphen);
            GetReport();
        }
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
        DataTable dt = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;
        SalesDataSetTableAdapters.sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter();

        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjSalesDataset.sp_Inv_SalesQuotationDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
        dt = ObjSalesDataset.sp_Inv_SalesQuotationDetail_SelectRow_Report;
        
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

        }


        Title = "SALES QUOTATION";

        string ProductParameter = string.Empty;

       

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

        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesQuotationReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
        {

            if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()))
            {
                DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objReportWt.FindControl("GroupHeader1", true);

                try
                {
                    if (dt.Rows[0]["Quotationtemplate"].ToString().Trim() == "" || dt.Rows[0]["Quotationtemplate"] == null)
                    {
                        Gh.Visible = false;
                    }
                    else
                    {
                        Gh.Visible = true;
                    }
                }
                catch
                {
                    Gh.Visible = false;
                }


                string strPath = string.Empty;
                try
                {
                    dt.Columns["Quotationtemplate"].ReadOnly = false;
                }
                catch
                {
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Quotationtemplate"] != "")
                    {
                        try
                        {
                            strPath = Server.MapPath("");
                            strPath = strPath.Replace("Sales", "CompanyResource").Trim();
                            dt.Rows[i]["Quotationtemplate"] = dt.Rows[i]["Quotationtemplate"].ToString().Replace("../CompanyResource", strPath).ToString();
                        }
                        catch
                        {
                        }
                    }
                }
                if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Invoice Report Format").Rows[0]["ParameterValue"].ToString() == "Customer Format 1")
                    objReportWt.setTax_Title("VAT ");
                else
                    objReportWt.setTax_Title("TAX ");
                DataTable Tax_Dt = objInvParam.Get_Location_GST_TRN_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0");
                if (Tax_Dt != null && Tax_Dt.Rows.Count > 0)
                {
                    objReportWt.setLoc_TRN_Title(Tax_Dt.Rows[0]["Registration_Type"].ToString() + "         : ", Tax_Dt.Rows[0]["Registration_No"].ToString());
                }
                else
                {
                    objReportWt.setLoc_TRN_Title("", "");
                }

                DataTable Cust_Reg_Dt = objInvParam.Get_Customer_GST_TRN_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), dt.Rows[0]["CustomerId"].ToString(), "0");
                if (Cust_Reg_Dt != null && Cust_Reg_Dt.Rows.Count > 0)
                {
                    objReportWt.setCust_TRN_Title(Cust_Reg_Dt.Rows[0]["Registration_Type"].ToString() + "      : ", Cust_Reg_Dt.Rows[0]["Registration_No"].ToString());
                }
                else
                {
                    objReportWt.setCust_TRN_Title("", "");
                }


                objReportWt.setProductParameterValue(ProductParameter);
                objReportWt.setSignature(signatureurl);
                objReportWt.setcompanyAddress(CompanyAddress);
                objReportWt.setcompanyname(CompanyName);
                objReportWt.setCompanyArebicName(CompanyName_L);
                objReportWt.setCompanyTelNo(Companytelno);
                objReportWt.setCompanyFaxNo(CompanyFaxno);
                objReportWt.setCompanyWebsite(CompanyWebsite);
                objReportWt.settitle(Title);
                objReportWt.SetImage(Imageurl);
                objReportWt.setUserName(createdby);
                objReportWt.DataSource = dt;
                objReportWt.DataMember = "sp_Inv_SalesQuotationDetail_SelectRow_Report";

                rptViewer.Report = objReportWt;
            }
            else
            {
                DevExpress.XtraReports.UI.GroupHeaderBand Gh = (DevExpress.XtraReports.UI.GroupHeaderBand)objReport.FindControl("GroupHeader1", true);

                try
                {
                    if (dt.Rows[0]["Quotationtemplate"].ToString().Trim() == "" || dt.Rows[0]["Quotationtemplate"] == null)
                    {
                        Gh.Visible = false;
                    }
                    else
                    {
                        Gh.Visible = true;
                    }
                }
                catch
                {
                    Gh.Visible = false;
                }


                string strPath = string.Empty;
                try
                {
                    dt.Columns["Quotationtemplate"].ReadOnly = false;
                }
                catch
                {
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Quotationtemplate"] != "")
                    {
                        try
                        {
                            strPath = Server.MapPath("");
                            strPath = strPath.Replace("Sales", "CompanyResource").Trim();
                            dt.Rows[i]["Quotationtemplate"] = dt.Rows[i]["Quotationtemplate"].ToString().Replace("../CompanyResource", strPath).ToString();
                        }
                        catch
                        {
                        }
                    }
                }
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

                DataTable Cust_Reg_Dt = objInvParam.Get_Customer_GST_TRN_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), dt.Rows[0]["CustomerId"].ToString(), "0");
                if (Cust_Reg_Dt != null && Cust_Reg_Dt.Rows.Count > 0)
                {
                    objReport.setCust_TRN_Title(Cust_Reg_Dt.Rows[0]["Registration_Type"].ToString() + "      : ", Cust_Reg_Dt.Rows[0]["Registration_No"].ToString());
                }
                else
                {
                    objReport.setCust_TRN_Title("", "");
                }


                objReport.setProductParameterValue(ProductParameter);
                objReport.setSignature(signatureurl);
                objReport.setcompanyAddress(CompanyAddress);
                objReport.setcompanyname(CompanyName);
                objReport.setCompanyArebicName(CompanyName_L);
                objReport.setCompanyTelNo(Companytelno);
                objReport.setCompanyFaxNo(CompanyFaxno);
                objReport.setCompanyWebsite(CompanyWebsite);
                objReport.settitle(Title);
                objReport.SetImage(Imageurl);
                objReport.setUserName(createdby);
                objReport.DataSource = dt;
                objReport.DataMember = "sp_Inv_SalesQuotationDetail_SelectRow_Report";

                rptViewer.Report = objReport;
            }

            
        }
        else
        {
            objReport_Internal.setProductParameterValue(ProductParameter);
            objReport_Internal.setSignature(signatureurl);

            objReport_Internal.setcompanyname(CompanyName);
            objReport_Internal.setCompanyArebicName(CompanyName_L);
            objReport_Internal.settitle(Title);
            objReport_Internal.SetImage(Imageurl);
            objReport_Internal.setUserName(createdby);
            objReport_Internal.DataSource = dt;
            objReport_Internal.DataMember = "sp_Inv_SalesQuotationDetail_SelectRow_Report";
            rptViewer.Report = objReport_Internal;
        }


        rptToolBar.ReportViewer = rptViewer;
        ViewState["Id"] = dt.Rows[0]["SQuotation_Id"].ToString();
        ViewState["CustId"] = dt.Rows[0]["CustomerId"].ToString();
       
        Set_CustomerMaster objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        string customercode = string.Empty;
              
        try
        {
            DataTable dtCust = objCustomer.GetCustomerAllDataByCustomerIdWithOutBrand(Session["CompId"].ToString(), dt.Rows[0]["CustomerId"].ToString());
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

            }

        }


        catch
        {

        }
        ES_MailSubject esmailsub = new ES_MailSubject(Session["DBConnection_ES"].ToString());
        string StringSubject = string.Empty;
        StringSubject = esmailsub.GetEmailSubjectAll(Session["CompId"].ToString(), true, "13", "57", customercode.ToString(), dt.Rows[0]["SQuotation_No"].ToString(),Session["EmpId"].ToString());
        if (StringSubject == "")
        {
            StringSubject = "Sales Quotation -" + dt.Rows[0]["SQuotation_No"].ToString();
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
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesQuotationReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
        {
            if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()))
            {
                objReportWt.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
            }
            else
            {
                objReport.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
            }                
        }
        else
        {
            objReport_Internal.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
        }
        ViewState["Path"] = StringSubject.Trim() + ".pdf";

        string ProductId = string.Empty;
        foreach (DataRow dr in dt.Rows)
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
        Response.Redirect("../EmailSystem/ContactMailReference.aspx?Page=SQ&&Url=" + ViewState["Path"].ToString() + "&&Id=" + ViewState["Id"].ToString() + "&&ConId=" + ViewState["CustId"].ToString() + "&&ProductId=" + ViewState["ProductId"].ToString() + "&&PageRefType=SQ&&PageRefId=" + ViewState["Id"].ToString());
    }
}
