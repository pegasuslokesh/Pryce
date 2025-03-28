using System;
using System.Web.UI;
using System.Data;

public partial class Sales_SalesOrder_Print : System.Web.UI.Page
{
    SalesOrderPrint objReport = null;
    SalesOrderPrint_Internal objReport_Internal = null;

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

        objReport = new SalesOrderPrint(Session["DBConnection"].ToString());
        objReport_Internal = new SalesOrderPrint_Internal(Session["DBConnection"].ToString());

        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());

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
        string locationid = new Inv_SalesOrderHeader(Session["DBConnection"].ToString()).getOrderLocationById(Request.QueryString["Id"].ToString());
        SalesDataSet objSalesdataset = new SalesDataSet();
        objSalesdataset.EnforceConstraints = false;
        SalesDataSetTableAdapters.sp_Inv_SalesOrderDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesOrderDetail_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(objSalesdataset.sp_Inv_SalesOrderDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(locationid), Convert.ToInt32(Request.QueryString["Id"].ToString()));
        DataTable Dt = objSalesdataset.sp_Inv_SalesOrderDetail_SelectRow_Report;
        //try
        //{
        //    Dt = new DataView(Dt, "Trans_Id=" + + "", "", DataViewRowState.CurrentRows).ToTable();
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

        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "Sales Report Header").Rows[0]["ParameterValue"].ToString() == "Company Level")
        {
            ParameterType = "Company";
            ParameterValue = Session["CompId"].ToString();
        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "Sales Report Header").Rows[0]["ParameterValue"].ToString() == "Brand Level")
        {
            ParameterType = "Brand";
            ParameterValue = Session["BrandId"].ToString();
        }
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "Sales Report Header").Rows[0]["ParameterValue"].ToString() == "Location Level")
        {
            ParameterType = "Location";
            ParameterValue = locationid;
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
        


        Title = "SALES ORDER";

        //if (Session["Parameter"] != null)
        //{
        //    objReport.SetDateCriteria(Session["Parameter"].ToString());
        //}
        //else
        //{
        //    objReport.SetDateCriteria("");
        //}
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
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "SalesOrderReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
        {
            if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "Invoice Report Format").Rows[0]["ParameterValue"].ToString() == "Customer Format 1")
                objReport.setTax_Title("VAT ");
            else
                objReport.setTax_Title("TAX ");
            DataTable Tax_Dt = objInvParam.Get_Location_GST_TRN_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "0");
            if (Tax_Dt != null && Tax_Dt.Rows.Count > 0)
            {
                objReport.setLoc_TRN_Title(Tax_Dt.Rows[0]["Registration_Type"].ToString() + "         : ", Tax_Dt.Rows[0]["Registration_No"].ToString());
            }
            else
            {
                objReport.setLoc_TRN_Title("", "");
            }

            DataTable Cust_Reg_Dt = objInvParam.Get_Customer_GST_TRN_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Dt.Rows[0]["CustomerId"].ToString(), "0");
            if (Cust_Reg_Dt != null && Cust_Reg_Dt.Rows.Count > 0)
            {
                objReport.setCust_TRN_Title(Cust_Reg_Dt.Rows[0]["Registration_Type"].ToString() + "         : ", Cust_Reg_Dt.Rows[0]["Registration_No"].ToString());
            }
            else
            {
                objReport.setCust_TRN_Title("", "");
            }

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
            objReport.setUserName(createdby);
            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Inv_SalesOrderDetail_SelectRow_Report";
            rptViewer.Report = objReport;
        }
        else
        {

            objReport_Internal.setSignature(signatureurl);

            objReport_Internal.setcompanyname(CompanyName);
            //objReport.setBrandName(BrandName);
            objReport_Internal.setLocationName(LocationName);
            objReport_Internal.settitle(Title);
            objReport_Internal.SetImage(Imageurl);
            objReport_Internal.setCompanyArebicName(CompanyName_L);

            objReport_Internal.setUserName(createdby);
            objReport_Internal.DataSource = Dt;
            objReport_Internal.DataMember = "sp_Inv_SalesOrderDetail_SelectRow_Report";
            rptViewer.Report = objReport_Internal;
        }



        rptToolBar.ReportViewer = rptViewer;
       // objReport.ExportToPdf(Server.MapPath("~/Temp/Sales Order-" + Dt.Rows[0]["SalesOrderNo"].ToString() + ".pdf"));
        //ViewState["Path"] = "Sales Order-" + Dt.Rows[0]["SalesOrderNo"].ToString() + ".pdf";
        ViewState["Id"] = Dt.Rows[0]["Trans_Id"].ToString();
        ViewState["CustId"] = Dt.Rows[0]["CustomerId"].ToString();

        ////Code for Subject 

        //Set_CustomerMaster objCustomer = new Set_CustomerMaster();
        //Ems_ContactMaster objContact = new Ems_ContactMaster();

        //string customercode = string.Empty;

        //try
        //{
        //    DataTable dtCust = objCustomer.GetCustomerAllDataByCustomerIdWithOutBrand(Session["CompId"].ToString(), Dt.Rows[0]["CustomerId"].ToString());
        //    if (dtCust.Rows.Count > 0)
        //    {
        //        if ((dtCust.Rows[0]["Status"].ToString() == "Individual") && (dtCust.Rows[0]["Company_Id1"].ToString() != "0"))
        //        {
        //            DataTable dtCon = objContact.GetContactTrueById(dtCust.Rows[0]["Company_Id1"].ToString());
        //            customercode = dtCust.Rows[0]["Customer_Code"].ToString() + "/" + dtCon.Rows[0]["Name"].ToString();
        //        }
        //        else
        //        {
        //            customercode = dtCust.Rows[0]["Customer_Code"].ToString() + "/" + dtCust.Rows[0]["Name"].ToString();
        //        }

        //    }

        //}


        //catch
        //{

        //}
        string StringSubject = ES_MailSubject.GetMailSubject("Sales Order", Dt.Rows[0]["SalesOrderNo"].ToString(), ViewState["CustId"].ToString(), "13", "67", Session["DBConnection_ES"].ToString(), Session["EmpId"].ToString());
        Session["MailSubject"] = StringSubject;


        //ES_MailSubject esmailsub = new ES_MailSubject();
        //string StringSubject = string.Empty;
        //StringSubject = esmailsub.GetEmailSubjectAll(Session["CompId"].ToString(), true, "13", "67", customercode.ToString(), Dt.Rows[0]["SalesOrderNo"].ToString());
        //if (StringSubject == "")
        //{
        //    StringSubject = "Sales Order-" + Dt.Rows[0]["SalesOrderNo"].ToString();
        //    Session["MailSubject"] = StringSubject;

        //}
        //else
        //{
        //    try
        //    {



        //        StringSubject = StringSubject.Replace("/", "").Trim();
        //        StringSubject = StringSubject.Replace("<", "").Trim();
        //        StringSubject = StringSubject.Replace(">", "").Trim();
        //        StringSubject = StringSubject.Replace(":", "").Trim();
        //        StringSubject = StringSubject.Replace("*", "").Trim();
        //        StringSubject = StringSubject.Replace("?", "").Trim();
        //        StringSubject = StringSubject.Replace("|", "").Trim();
        //        StringSubject = StringSubject.Replace(",", "").Trim();
        //        StringSubject = StringSubject.Replace("&", "").Trim();
                
        //        Session["MailSubject"] = StringSubject;

                
        //    }
        //    catch
        //    {

        //    }
        //}
        if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "SalesOrderReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
        {
            objReport.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
        }
        else
        {
            objReport_Internal.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));

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
        Response.Redirect("../EmailSystem/ContactMailReference.aspx?Page=SO&&Url=" + ViewState["Path"].ToString() + "&&Id=" + ViewState["Id"].ToString() + "&&ConId=" + ViewState["CustId"].ToString() + "&&ProductId=" + ViewState["ProductId"].ToString() + "&&PageRefType=SO&&PageRefId=" + ViewState["Id"].ToString());

    }
}
