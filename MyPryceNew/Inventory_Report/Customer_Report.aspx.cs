using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_Customer_Report : BasePage
{
    Set_Suppliers objSupplier = null;
    Set_CustomerMaster objCustomer = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster ObjContactMaster = null;

    CustomerDetailBySalesPerson objReport = new CustomerDetailBySalesPerson();
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;

    CustomerDetail objReport_Detail = new CustomerDetail();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());

        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
           
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "167", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Session["dtFilter_Cus_Rpt"] = null;
        }
        else
        {
            if (ChkSalesPerson.Checked == true)
            {
                GetReport_BySalesPerson();
            }
            else
            {
                GetReport();
            }            
        }
        AllPageCode();
    }
    public void AllPageCode()
    {

        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "145";
        Session["HeaderText"] = "Inventory Report";

    }

    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        Session["dtFilter_Cus_Rpt"] = null;

        DataTable DtFilter = new DataTable();
        InventoryDataSet ObjInvDataset = new InventoryDataSet();
        ObjInvDataset.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Set_Customer_Detail_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Set_Customer_Detail_ReportTableAdapter();

        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjInvDataset.sp_Set_Customer_Detail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()));
        DtFilter = ObjInvDataset.sp_Set_Customer_Detail_Report;
        if (txtCustomer.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "Contact_Id=" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }
        if (DtFilter.Rows.Count > 0)
        {
            Session["dtFilter_Cus_Rpt"] = DtFilter;


            if (ChkSalesPerson.Checked == true)
            {
                //Response.Redirect("../Inventory_Report/Customer_Detail_Report_BySalesPerson.aspx");
                GetReport_BySalesPerson();
            }
            else
            {
                //Response.Redirect("../Inventory_Report/Customer_Detail_Report.aspx");
                GetReport();
            }
        }
        else
        {
            DisplayMessage("Record Not Found");
            txtCustomer.Focus();
            return;
        }


    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Name like '" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            dtCon = dtCustomer;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Customer_Id"].ToString();
            }
        }
        return filterlist;

    }
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomer.Text != "")
        {
            DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomer.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {
                hdnCustomerId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("Select Customer In Suggestion only");
                txtCustomer.Text = "";
                txtCustomer.Focus();
                return;
            }

        }


    }

    void GetReport_BySalesPerson()
    {
        DataTable Dt = new DataTable();

        if (Session["dtFilter_Cus_Rpt"] != null)
        {
            Dt = (DataTable)Session["dtFilter_Cus_Rpt"];
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

    void GetReport()
    {
        DataTable Dt = new DataTable();

        if (Session["dtFilter_Cus_Rpt"] != null)
        {
            Dt = (DataTable)Session["dtFilter_Cus_Rpt"];
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
        objReport_Detail.setcompanyAddress(CompanyAddress);
        objReport_Detail.setcompanyname(CompanyName);
        objReport_Detail.setBrandName(BrandName);
        objReport_Detail.setLocationName(LocationName);
        objReport_Detail.settitle(Title);
        objReport_Detail.SetImage(Imageurl);
        objReport_Detail.setUserName(Session["UserId"].ToString());



        objReport_Detail.DataSource = DtCustomer;
        objReport_Detail.DataMember = "Customer_Detail";
        rptViewer.Report = objReport_Detail;
        rptToolBar.ReportViewer = rptViewer;

    }
}
