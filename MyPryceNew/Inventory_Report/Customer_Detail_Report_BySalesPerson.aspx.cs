using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_Customer_Detail_Report_BySalesPerson : System.Web.UI.Page
{
    CustomerDetailBySalesPerson objReport = new CustomerDetailBySalesPerson();

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
        Session["AccordianId"] = "145";
        Session["HeaderText"] = "Inventory Report";

    }
    void GetReport()
    {
        DataTable Dt = new DataTable();

        if (Session["dtFilter_Cus_DtBs"] != null)
        {
            Dt = (DataTable)Session["dtFilter_Cus_DtBs"];
        }
        DataTable DtCustomer = new DataTable();

        DtCustomer.Columns.Add("Customer_Name");
        DtCustomer.Columns.Add("Address");
        DtCustomer.Columns.Add("Contact_No");
        DtCustomer.Columns.Add("Email_Id");
        DtCustomer.Columns.Add("Company");
        DtCustomer.Columns.Add("Customer_code");
        DtCustomer.Columns.Add("Customer_Id");
        DtCustomer.Columns.Add("Sales_Person");

        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            string ContactNo = "";
            string EmailId = "";
            string Address = "";
            DataRow dr = DtCustomer.NewRow();
            dr["Customer_Id"] = Dt.Rows[i]["Contact_Id"].ToString();
            dr["Customer_code"] = Dt.Rows[i]["Contact_Code"].ToString();
            dr["Customer_Name"] = Dt.Rows[i]["Contact_Name"].ToString();

            if (Dt.Rows[i]["Address"].ToString() != "")
            {
                Address = Dt.Rows[i]["Address"].ToString();
            }
            if (Dt.Rows[i]["Street"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + Dt.Rows[i]["Street"].ToString();
                }
                else
                {
                    Address = Dt.Rows[i]["Street"].ToString();
                }
            }
            if (Dt.Rows[i]["Block"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + Dt.Rows[i]["Block"].ToString();
                }
                else
                {
                    Address = Dt.Rows[i]["Block"].ToString();
                }

            }
            if (Dt.Rows[i]["Avenue"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + Dt.Rows[i]["Avenue"].ToString();
                }
                else
                {
                    Address = Dt.Rows[i]["Avenue"].ToString();
                }
            }

            if (Dt.Rows[i]["CityId"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + Dt.Rows[i]["CityId"].ToString();
                }
                else
                {
                    Address = Dt.Rows[i]["CityId"].ToString();
                }

            }
            if (Dt.Rows[i]["StateId"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + Dt.Rows[i]["StateId"].ToString();
                }
                else
                {
                    Address = Dt.Rows[i]["StateId"].ToString();
                }

            }
            if (Dt.Rows[i]["Country_Name"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + Dt.Rows[i]["Country_Name"].ToString();
                }
                else
                {
                    Address = Dt.Rows[i]["Country_Name"].ToString();
                }

            }
            if (Dt.Rows[i]["PinCode"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + Dt.Rows[i]["PinCode"].ToString();
                }
                else
                {
                    Address = Dt.Rows[i]["PinCode"].ToString();
                }

            }
            dr["Address"] = Address;
            dr["Sales_Person"] = Dt.Rows[i]["Sales_Person"].ToString();

            if (Dt.Rows[i]["PhoneNo1"].ToString() != "")
            {
                ContactNo = Dt.Rows[i]["PhoneNo1"].ToString();
            }
            if (Dt.Rows[i]["PhoneNo2"].ToString() != "")
            {
                if (ContactNo != "")
                {
                    ContactNo = ContactNo + "," + Dt.Rows[i]["PhoneNo2"].ToString();
                }
                else
                {
                    ContactNo = Dt.Rows[i]["PhoneNo2"].ToString();
                }
            }

            dr["Contact_No"] = ContactNo;
            if (Dt.Rows[i]["EmailId1"].ToString() != "")
            {
                EmailId = Dt.Rows[i]["EmailId1"].ToString();
            }
            if (Dt.Rows[i]["EmailId2"].ToString() != "")
            {
                if (EmailId.Trim() != "")
                {
                    EmailId = EmailId + "," + Dt.Rows[i]["EmailId2"].ToString();
                }
                else
                {
                    EmailId = Dt.Rows[i]["EmailId2"].ToString();
                }
            }
            dr["Email_Id"] = EmailId;
            dr["Company"] = Dt.Rows[i]["Company_Name"].ToString();
            DtCustomer.Rows.Add(dr);

        }





        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string FromDate = "";
        string Todate = "";
        string Title = "";
        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();


        }
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
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


        }
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
        }



        Title = "Customer Detail Report";
        objReport.setcompanyAddress(CompanyAddress);
        objReport.setcompanyname(CompanyName);
        objReport.setBrandName(BrandName);
        objReport.setLocationName(LocationName);
        objReport.settitle(Title);
        objReport.SetImage(Imageurl);
        objReport.SetUserName(Session["UserId"].ToString());



        objReport.DataSource = DtCustomer;
        objReport.DataMember = "Customer_Detail";
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;

    }
}
