using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Purchase_PurchaseInquiryPrint : System.Web.UI.Page
{
    PurchaseInquiryPrint_Internal objPIPrint_Internal = null;
    PurchaseInquiryPrint objPIPrint = null;
    PurchaseDataSet ObjPdataset = new PurchaseDataSet();
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    Inv_PurchaseInquiry_Supplier ObjPISupplier = null;
    Inv_ParameterMaster objInvParam = null;

    string strCompId = string.Empty;
    string strBrandId = string.Empty;
    string strLocationId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        objPIPrint_Internal = new PurchaseInquiryPrint_Internal(Session["DBConnection"].ToString());
        objPIPrint = new PurchaseInquiryPrint(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjPISupplier = new Inv_PurchaseInquiry_Supplier(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());

        ObjPdataset.EnforceConstraints = false;
        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        string CompanyName = "";
       
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string CompanyName_L = "";
        string CompanyAddress = "";
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
       
        if (Request.QueryString["RId"] != null)
        {
            DataTable dt = new DataTable();
            try
            {
               
                    string ParameterType=string.Empty;
                    string ParameterValue=string.Empty;

                 if(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(),Session["BrandId"].ToString(),Session["LocId"].ToString(),"Purchase Report Header").Rows[0]["ParameterValue"].ToString()=="Company Level")
                 {
                     ParameterType="Company";
                     ParameterValue=Session["CompId"].ToString();
                 }
                 if(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(),Session["BrandId"].ToString(),Session["LocId"].ToString(),"Purchase Report Header").Rows[0]["ParameterValue"].ToString()=="Brand Level")
                 {
                      ParameterType="Brand";
                     ParameterValue=Session["BrandId"].ToString();
                 }
                 if(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(),Session["BrandId"].ToString(),Session["LocId"].ToString(),"Purchase Report Header").Rows[0]["ParameterValue"].ToString()=="Location Level")
                 {
                       ParameterType="Location";
                     ParameterValue=Session["LocId"].ToString();
                 }



                 string[] strParam = Common.ReportHeaderSetup(ParameterType, ParameterValue, Session["DBConnection"].ToString());


                 CompanyName = strParam[0].ToString();
                 CompanyName_L = strParam[1].ToString();
                 CompanyAddress = strParam[2].ToString();
                 Companytelno = strParam[3].ToString();
                 CompanyFaxno = strParam[4].ToString();
                 CompanyWebsite = strParam[5].ToString();
                 Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();

                PurchaseDataSetTableAdapters.Inv_PurchaseInquiryPrintTableAdapter ObjPrintAd = new PurchaseDataSetTableAdapters.Inv_PurchaseInquiryPrintTableAdapter();
                ObjPrintAd.Connection.ConnectionString = Session["DBConnection"].ToString();
                ObjPrintAd.Fill(ObjPdataset.Inv_PurchaseInquiryPrint, Convert.ToInt32(strCompId.ToString()), Convert.ToInt32(strBrandId.ToString()), Convert.ToInt32(strLocationId), Convert.ToInt32(Request.QueryString["RId"].ToString()));
                dt = ObjPdataset.Inv_PurchaseInquiryPrint;
                if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseInquiryReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
                {
                    objPIPrint.DataSource = dt;
                    objPIPrint.DataMember = "Inv_PurchaseInquiryPrint";
                    ReportViewer1.Report = objPIPrint;
                    ReportToolbar2.ReportViewer = ReportViewer1;
                    objPIPrint.setcompanyname(CompanyName);
                    objPIPrint.setcompanyAddress(CompanyAddress);
                    objPIPrint.setCompanyArebicName(CompanyName_L);
                    objPIPrint.setCompanyTelNo(Companytelno);
                    objPIPrint.setCompanyFaxNo(CompanyFaxno);
                    objPIPrint.setCompanyWebsite(CompanyWebsite);
                    objPIPrint.SetImage(Imageurl);
                    objPIPrint.setSignature(signatureurl);
                }
                else
                {
                    objPIPrint_Internal.DataSource = dt;
                    objPIPrint_Internal.DataMember = "Inv_PurchaseInquiryPrint";
                    ReportViewer1.Report = objPIPrint_Internal;
                    ReportToolbar2.ReportViewer = ReportViewer1;
                    objPIPrint_Internal.setcompanyname(CompanyName);
                    objPIPrint_Internal.setCompanyArebicName(CompanyName_L);
                    objPIPrint.SetImage(Imageurl);
                    objPIPrint.setSignature(signatureurl);

                }
                //objPIPrint.ExportToPdf(Server.MapPath("~/Temp/Purchase Inquiry -" + dt.Rows[0]["PI_No1"].ToString() + ".pdf"));
               // ViewState["Path"] = "Purchase Inquiry -" + dt.Rows[0]["PI_No1"].ToString() + ".pdf";
                ViewState["Id"] = Request.QueryString["RId"].ToString();
                string str = string.Empty;
                DataTable dtSupplier = ObjPISupplier.GetAllPISupplierWithPI_No(strCompId, strBrandId, Session["LocId"].ToString(), Request.QueryString["RId"].ToString());
                int i = 0;
                foreach (DataRow dr in dtSupplier.Rows)
                {
                    i++;
                    if (i < dtSupplier.Rows.Count)
                    {
                        str += dr["Supplier_Id"].ToString() + ",";
                    }
                    else
                    {
                        str += dr["Supplier_Id"].ToString();

                    }

                }
                ViewState["SupId"] = str.ToString();

                ES_MailSubject esmailsub = new ES_MailSubject(HttpContext.Current.Session["DBConnection_ES"].ToString());
                string StringSubject = string.Empty;
                StringSubject = esmailsub.GetEmailSubjectAll(Session["CompId"].ToString(), true, "12", "49", "", dt.Rows[0]["PI_No1"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (StringSubject == "")
                {
                    StringSubject = "Purchase Inquiry -" + dt.Rows[0]["PI_No1"].ToString();
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
                if (objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PurchaseInquiryReportHeaderLevel").Rows[0]["ParameterValue"].ToString() == "External")
                {
                    objPIPrint.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
                }
                else
                {
                    objPIPrint_Internal.ExportToPdf(Server.MapPath("~/Temp/" + StringSubject.ToString().Trim() + ".pdf"));
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
            catch
            {

            }

        }

    }



    protected void btnSend_Click(object sender, EventArgs e)
    {
        Response.Redirect("../EmailSystem/ContactMailReference.aspx?Page=PI&&Url=" + ViewState["Path"].ToString() + "&&Id=" + ViewState["Id"].ToString() + "&&ConId=" + ViewState["SupId"].ToString() + "&&ProductId=" + ViewState["ProductId"].ToString() + "&&PageRefType=PI&&PageRefId=" + Request.QueryString["RId"].ToString());

    }
}
