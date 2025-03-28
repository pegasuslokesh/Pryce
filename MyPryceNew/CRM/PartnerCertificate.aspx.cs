using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CRM_PartnerCertificate : System.Web.UI.Page
{
    CRM_Agreements obj_agreement = null;
    CRM_Agreements_Product obj_agreement_product = null;
    string strAgreementNo = "";
    XtraReport obj = new XtraReport();
    LocationMaster objLoc = null;
    Set_AddressChild objAddChild = null;
    UserMaster objUserMaser = null;
    EmployeeMaster objEM = null;
    protected void Page_Load(object sender, EventArgs e)
    {


        string strPath = Server.MapPath("~");
        obj_agreement = new CRM_Agreements(Session["DBConnection"].ToString());
        obj_agreement_product = new CRM_Agreements_Product(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objUserMaser = new UserMaster(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());

        objEM = new EmployeeMaster(Session["DBConnection"].ToString());
        obj.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "/partner_certificate.repx");
        if (!IsPostBack)
        {

            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "98", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

        }
        strAgreementNo = Request.QueryString["Id"].ToString();
        GetReport();
    }

    private void GetReport()
    {
        DataTable dt_advanceSearch = obj_agreement.getAdvanceSearchData("", strAgreementNo, "", "", "", "", "", "", "");
        string strProductCategory = "";
        for (int count = 0; count < dt_advanceSearch.Rows.Count; count++)
        {
            if (count == 0)
            {
                strProductCategory = strProductCategory + " Hardwares : " + dt_advanceSearch.Rows[count]["product_category"].ToString();
            }
            else
            {
                strProductCategory = strProductCategory + "," + dt_advanceSearch.Rows[count]["product_category"].ToString();
            }
        }

        XRLabel lblHardwares = (XRLabel)obj.FindControl("lblHardwares", true);
        lblHardwares.Text = strProductCategory;

        XRLabel lblCustomer = (XRLabel)obj.FindControl("lblCustomer", true);
        lblCustomer.Text = dt_advanceSearch.Rows[0]["CustomerName"].ToString();

        string strForLocation = "";
        XRLabel lblForLocation = (XRLabel)obj.FindControl("lblForLocation", true);
        if (dt_advanceSearch.Rows[0]["group_name"].ToString().Length > 0)
        {

            strForLocation = "IS IN " + dt_advanceSearch.Rows[0]["group_name"].ToString() + " ";

            if (dt_advanceSearch.Rows[0]["city_name"].ToString().Length > 0)
            {
                strForLocation = strForLocation + " FOR " + dt_advanceSearch.Rows[0]["city_name"].ToString();
            }
            else if (dt_advanceSearch.Rows[0]["state_Name"].ToString().Length > 0)
            {
                strForLocation = strForLocation + " FOR " + dt_advanceSearch.Rows[0]["state_Name"].ToString();
            }
            else if (dt_advanceSearch.Rows[0]["country_name"].ToString().Length > 0)
            {
                strForLocation = strForLocation + " FOR " + dt_advanceSearch.Rows[0]["country_name"].ToString();
            }
        }
        lblForLocation.Text = strForLocation;

        XRLabel lblForYear = (XRLabel)obj.FindControl("lblForYear", true);
        lblForYear.Text = "For The Year " + GetMonthName(Convert.ToDateTime(dt_advanceSearch.Rows[0]["from_date"].ToString()).Month) + " " + Convert.ToDateTime(dt_advanceSearch.Rows[0]["from_date"].ToString()).Year + " to " + GetMonthName(Convert.ToDateTime(dt_advanceSearch.Rows[0]["to_date"].ToString()).Month) + " " + Convert.ToDateTime(dt_advanceSearch.Rows[0]["to_date"].ToString()).Year;

        XRLabel lblValidity = (XRLabel)obj.FindControl("lblValidity", true);
        lblValidity.Text = "Validity : " + Convert.ToDateTime(dt_advanceSearch.Rows[0]["from_date"].ToString()).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dt_advanceSearch.Rows[0]["to_date"].ToString()).ToString("dd/MM/yyyy");


        DataTable DtLocation = objLoc.GetLocationMasterById(dt_advanceSearch.Rows[0]["Company_Id"].ToString(), dt_advanceSearch.Rows[0]["Location_Id"].ToString());


        XRLabel lblLocation = (XRLabel)obj.FindControl("lblLocation", true);
        lblLocation.Text = DtLocation.Rows[0]["Location_Name"].ToString();

        string strCountryName = "";
        DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Location", dt_advanceSearch.Rows[0]["Location_Id"].ToString());
        XRLabel lblLocationAddress = (XRLabel)obj.FindControl("lblLocationAddress", true);
        if (dtChild.Rows.Count > 0)
        {
            lblLocationAddress.Text = dtChild.Rows[0]["Address_Name"].ToString();
            try
            {
                string[] arrCountryName = dtChild.Rows[0]["FullAddress"].ToString().Split(new Char[] { ',' });

                strCountryName = arrCountryName[arrCountryName.Length - 1];
            }
            catch
            {
                strCountryName = "";
            }
        }


        DataTable dtEmp = objEM.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), Session["UserId"].ToString());


        XRLabel lblCountry = (XRLabel)obj.FindControl("lblCountry", true);
        lblCountry.Text = strCountryName;
        string Imageurl = "";
        XRPictureBox pBoxLogo = (XRPictureBox)obj.FindControl("pBoxLogo", true);
        try
        {

            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtLocation.Rows[0]["Field2"].ToString();
            pBoxLogo.ImageUrl = Imageurl;
        }
        catch
        {

        }


        string Imageurl1 = "";

        if (dtEmp.Rows.Count > 0)
        {
            try
            {
                if (dtEmp.Rows[0]["Field3"].ToString().Length > 0)
                {
                    Imageurl1 = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmp.Rows[0]["Field3"].ToString();
                }
            }
            catch
            {

            }
        }


      
        XRPictureBox pBoxAuthSign = (XRPictureBox)obj.FindControl("pBoxAuthSign", true);
        try
        {

            if(Imageurl1.Length > 0)
            {
                pBoxAuthSign.ImageUrl = Imageurl1;
            }
            
        }
        catch
        {

        }


        DataTable dtUser = objUserMaser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        XRLabel lblAuthroizedBy = (XRLabel)obj.FindControl("lblAuthroizedBy", true);
        if (dtUser.Rows.Count > 0)
        {
            lblAuthroizedBy.Text = dtUser.Rows[0]["EmpName"].ToString() + "(" + dtUser.Rows[0]["Emp_Designation"].ToString() + ")";
        }



        obj.CreateDocument(true);
        rptViewer.Report = obj;
        rptToolBar.ReportViewer = rptViewer;
    }
    public string GetMonthName(int i)
    {
        string strMonth = "";
        switch (i)
        {
            case 1:
                strMonth = "January"; break;
            case 2:
                strMonth = "February"; break;
            case 3:
                strMonth = "March"; break;
            case 4:
                strMonth = "April"; break;
            case 5:
                strMonth = "May"; break;
            case 6:
                strMonth = "June"; break;
            case 7:
                strMonth = "July"; break;
            case 8:
                strMonth = "August"; break;
            case 9:
                strMonth = "September"; break;
            case 10:
                strMonth = "October"; break;
            case 11:
                strMonth = "November"; break;
            case 12:
                strMonth = "December"; break;


        }
        return strMonth;
    }


}