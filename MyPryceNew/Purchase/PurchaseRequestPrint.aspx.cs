using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Purchase_PurchaseRequestPrint : BasePage
{
    EmployeeMaster objEmployee = null;

    PurchaseRequestPrint objProductRequestPrint = null;
    PurchaseRequestPrint_Internal objProductRequestPrint_Internal = null;

    PurchaseRequestHeader InvPr = null;
    PurchaseDataSet ObjPdataset = new PurchaseDataSet();
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;

    string strCompId = string.Empty;
    string strBrandId = string.Empty;
    string strLocationId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());

        objProductRequestPrint = new PurchaseRequestPrint(Session["DBConnection"].ToString());
        objProductRequestPrint_Internal = new PurchaseRequestPrint_Internal(Session["DBConnection"].ToString());

        InvPr = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());


        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        ObjPdataset.EnforceConstraints = false;
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string CompanyName_L = string.Empty;
        string Companytelno = string.Empty;
        string CompanyFaxno = string.Empty;
        string CompanyWebsite = string.Empty;

        
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




        if (Request.QueryString["RId"] != null)
        {
            DataTable dt = new DataTable();

            try
            {
                string Id = new DataView(InvPr.GetPurchaseRequestHeaderTrueAll(Session["CompId"].ToString(), strBrandId.ToString(), strLocationId.ToString()), "RequestNo='" + Request.QueryString["RId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString();

                PurchaseDataSetTableAdapters.Inv_PurchaseRequestPrint1TableAdapter ObjPrintAd = new PurchaseDataSetTableAdapters.Inv_PurchaseRequestPrint1TableAdapter();
                ObjPrintAd.Connection.ConnectionString = Session["DBConnection"].ToString();
                ObjPrintAd.Fill(ObjPdataset.Inv_PurchaseRequestPrint1, Convert.ToInt32(strCompId.ToString()), Convert.ToInt32(strBrandId.ToString()), Convert.ToInt32(strLocationId), Convert.ToInt32(Id.ToString()));
                dt = ObjPdataset.Inv_PurchaseRequestPrint1;

                if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseRequestReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
                {
                    objProductRequestPrint.DataSource = dt;
                    objProductRequestPrint.DataMember = "Inv_PurchaseRequestPrint1";
                    ReportViewer1.Report = objProductRequestPrint;
                    ReportToolbar1.ReportViewer = ReportViewer1;
                    objProductRequestPrint.setcompanyname(CompanyName);
                    objProductRequestPrint.setCompanyArebicName(CompanyName_L);
                    objProductRequestPrint.setCompanyTelNo(Companytelno);
                    objProductRequestPrint.setCompanyFaxNo(CompanyFaxno);
                    objProductRequestPrint.setCompanyWebsite(CompanyWebsite);
                    objProductRequestPrint.setSignature(signatureurl);
                    objProductRequestPrint.setcompanyAddress(CompanyAddress);
                    objProductRequestPrint.SetImage(Imageurl);
                }
                if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseRequestReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "Internal")
                {
                    objProductRequestPrint_Internal.DataSource = dt;
                    objProductRequestPrint_Internal.DataMember = "Inv_PurchaseRequestPrint1";
                    ReportViewer1.Report = objProductRequestPrint_Internal;
                    ReportToolbar1.ReportViewer = ReportViewer1;
                    objProductRequestPrint_Internal.setcompanyname(CompanyName);
                    objProductRequestPrint_Internal.setCompanyArebicName(CompanyName_L);
                    objProductRequestPrint_Internal.setSignature(signatureurl);
                    objProductRequestPrint_Internal.SetImage(Imageurl);
                }
                ViewState["Id"] = Id.ToString();
                // objProductRequestPrint.ExportToPdf(Server.MapPath("~/Temp/Purchase Request -" + Request.QueryString["RId"].ToString() + ".pdf"));
                //ViewState["Path"] = "Purchase Request -" + Request.QueryString["RId"].ToString() + ".pdf";

            }
            catch
            {

            }

            ES_MailSubject esmailsub = new ES_MailSubject(HttpContext.Current.Session["DBConnection_ES"].ToString());
            string StringSubject = string.Empty;
            StringSubject = esmailsub.GetEmailSubjectAll(Session["CompId"].ToString(), true, "12", "44", "", Request.QueryString["RId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (StringSubject == "")
            {
                StringSubject = "Purchase Request -" + Request.QueryString["RId"].ToString();
                Session["MailSubject"] = StringSubject;

            }
            else
            {
                try
                {


                    StringSubject = StringSubject.Replace("/", "--").Trim();
                    StringSubject = StringSubject.Replace("<", "--").Trim();
                    StringSubject = StringSubject.Replace(">", "--").Trim();
                    StringSubject = StringSubject.Replace(":", "--").Trim();
                    StringSubject = StringSubject.Replace("*", "--").Trim();
                    StringSubject = StringSubject.Replace("?", "--").Trim();
                    StringSubject = StringSubject.Replace("|", "--").Trim();
                    StringSubject = StringSubject.Replace(",", "--").Trim();
                    StringSubject = StringSubject.Replace("&", "").Trim();

                    Session["MailSubject"] = StringSubject;
                    //objProductRequestPrint.ExportToPdf(Server.MapPath("~/Temp/Purchase Request -" + Request.QueryString["RId"].ToString() + ".pdf"));
                    //ViewState["Path"] = "Purchase Request -" + Request.QueryString["RId"].ToString() + ".pdf";

                }
                catch
                {

                }
            }
            if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseRequestReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
            {
                objProductRequestPrint.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
            }
            else
            {
                objProductRequestPrint_Internal.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
            }
            ViewState["Path"] = StringSubject.Trim() + ".pdf";


        }

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {

        Response.Redirect("../EmailSystem/SendMail.aspx?Page=PR&&URL=" + ViewState["Path"].ToString() + "&&PageRefType=PR&&PageRefId=" + ViewState["Id"].ToString());


    }

}
