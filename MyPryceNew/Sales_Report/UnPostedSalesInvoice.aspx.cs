using System;
using System.Web.UI;
using System.Data;

public partial class Sales_Report_UnPostedSalesInvoice : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    UnPostedInvoicePrint objProductStockPrint = null;
    PurchaseRequestHeader InvPr = null;
    InventoryDataSet ObjInvdataset = new InventoryDataSet();
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

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objProductStockPrint = new UnPostedInvoicePrint(Session["DBConnection"].ToString());
        InvPr = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());

        if (Request.QueryString["LID"]!=null)
        {
            GetReport(Request.QueryString["LID"].ToString());
        }
        else
        {
            GetReport();
        }
        
        Page.Title = ObjSysParam.GetSysTitle();
       
    }
    public void GetReport(string LocationId = "")
    {
        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        if (LocationId == "")
        {
            strLocationId = Session["LocId"].ToString();
        }
        else
        {
            strLocationId = LocationId;
        }
        ObjInvdataset.EnforceConstraints = false;
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string CompanyName_L = string.Empty;
        string Companytelno = string.Empty;
        string CompanyFaxno = string.Empty;
        string CompanyWebsite = string.Empty;

        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            CompanyName_L = DtCompany.Rows[0]["Company_Name_L"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();


        }
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), strLocationId);
        if (DtLocation.Rows.Count > 0)
        {
            LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
        }
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            if (DtAddress.Rows[0]["Address"].ToString() != "")
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }
            if (DtAddress.Rows[0]["Street"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Street"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Street"].ToString();
                }
            }
            if (DtAddress.Rows[0]["Block"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Block"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Block"].ToString();
                }

            }
            if (DtAddress.Rows[0]["Avenue"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Avenue"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Avenue"].ToString();
                }
            }

            if (DtAddress.Rows[0]["CityId"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["CityId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["CityId"].ToString();
                }

            }
            if (DtAddress.Rows[0]["StateId"].ToString() != "")
            {


                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["StateId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["StateId"].ToString();
                }

            }
            if (DtAddress.Rows[0]["CountryId"].ToString() != "")
            {

                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + LocationName;
                }
                else
                {
                    CompanyAddress = LocationName;
                }

            }
            if (DtAddress.Rows[0]["PinCode"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["PinCode"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["PinCode"].ToString();
                }

            }
            Companytelno = DtAddress.Rows[0]["PhoneNo1"].ToString();

            if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
            {

                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["PhoneNo2"].ToString();
                }
            }
            if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
            {
                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo1"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["MobileNo1"].ToString();
                }
            }
            if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
            {
                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["MobileNo2"].ToString();
                }
            }
            if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            {
                CompanyFaxno = DtAddress.Rows[0]["FaxNo"].ToString();
            }
            if (DtAddress.Rows[0]["WebSite"].ToString() != "")
            {
                CompanyWebsite = DtAddress.Rows[0]["WebSite"].ToString();
            }



        }
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
        }
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
            signatureurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtemployee.Rows[0]["Field2"].ToString();
        }
        try
        {
            DataTable dt = (DataTable)Session["UnPostedStock"];
            objProductStockPrint.DataSource = dt;
            objProductStockPrint.DataMember = "DtUnPostedInvoice";
            ReportViewer1.Report = objProductStockPrint;
            ReportToolbar1.ReportViewer = ReportViewer1;
            objProductStockPrint.setcompanyname(CompanyName);
            objProductStockPrint.setSignature(signatureurl);
            objProductStockPrint.setCompanyArebicName(CompanyName_L);
            objProductStockPrint.setCompanyTelNo(Companytelno);
            objProductStockPrint.setCompanyFaxNo(CompanyFaxno);
            objProductStockPrint.setCompanyWebsite(CompanyWebsite);
            objProductStockPrint.setcompanyAddress(CompanyAddress);
            objProductStockPrint.SetImage(Imageurl);
            //objProductStockPrint.ExportToPdf(Server.MapPath("~/Temp/Product Stock -" + dt.Rows[0]["ProductId"].ToString() + ".pdf"));
            //ViewState["Path"] = "Product Ledger -" + dt.Rows[0]["ProductId"].ToString() + ".pdf";
            dt = null;
        }
        catch
        {

        }
    }


    protected void btnSend_Click(object sender, EventArgs e)
    {
        Response.Redirect("../EmailSystem/SendMail.aspx?Page=PS&&URL=" + ViewState["Path"].ToString() + "'");
    }
}