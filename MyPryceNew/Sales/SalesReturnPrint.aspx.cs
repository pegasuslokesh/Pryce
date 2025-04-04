﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sales_SalesReturnPrint : System.Web.UI.Page
{
    //SalesInvoicePrint objReport1 = new SalesInvoicePrint();
    Sales_Return_Print objReport = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;
    Inv_ParameterMaster objParam = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new Sales_Return_Print(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());

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

        SalesDataSetTableAdapters.sp_Inv_SalesReturnHeader_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesReturnHeader_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjSalesDataset.sp_Inv_SalesReturnHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
        Dt = ObjSalesDataset.sp_Inv_SalesReturnHeader_SelectRow_Report;
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

        objReport.setSignature(signatureurl);

        objReport.setcompanyAddress(CompanyAddress);
        objReport.setcompanyname(CompanyName);
        objReport.settitle("SALES RETURN");
        objReport.SetImage(Imageurl);
        objReport.setUserName(createdby);
        objReport.setCompanyArebicName(CompanyName_L);
        objReport.setCompanyTelNo(Companytelno);
        objReport.setCompanyFaxNo(CompanyFaxno);
        objReport.setCompanyWebsite(CompanyWebsite);

        objReport.DataSource = Dt;
        objReport.DataMember = "sp_Inv_SalesReturnHeader_SelectRow_Report";
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;



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

            }

        }
        catch
        {

        }
        ES_MailSubject esmailsub = new ES_MailSubject(HttpContext.Current.Session["DBConnection_ES"].ToString());
        string StringSubject = string.Empty;
        StringSubject = esmailsub.GetEmailSubjectAll(Session["CompId"].ToString(), true, "13", "92", customercode.ToString(), Dt.Rows[0]["Invoice_No"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (StringSubject == "")
        {
            StringSubject = "Sales Invoice-" + Dt.Rows[0]["Invoice_No"].ToString();
            Session["MailSubject"] = StringSubject;

        }

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


        objReport.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));

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