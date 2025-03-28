using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_BillOfMaterial_Report : System.Web.UI.Page
{
    Bill_Of_Material objReport = null;
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

        objReport = new Bill_Of_Material(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());

        GetReport();


    }



    void GetReport()
    {
        string GroupBy = string.Empty;
        DataTable dtFilter = new DataTable();

        InventoryDataSet rptdata = new InventoryDataSet();

        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Inv_BOM_SelectRowTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_BOM_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();

        adp.Fill(rptdata.sp_Inv_BOM_SelectRow, Convert.ToInt32(Session["CompId"].ToString()), 0, Convert.ToInt32(Request.QueryString["ModelId"].ToString()), 4);

        dtFilter = rptdata.sp_Inv_BOM_SelectRow;

        try
        {
            dtFilter = new DataView(dtFilter, "ModelId=" + Request.QueryString["ModelId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }


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
        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            CompanyName_L = DtCompany.Rows[0]["Company_Name_L"].ToString();

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
            {

                Companytelno = DtAddress.Rows[0]["PhoneNo1"].ToString();
            }
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


        if (Session["Lang"].ToString() == "1")
            Title = "BILL OF MATERIAL";
        else
            Title = Resources.Attendance.Bill_Of_Material;


        DataTable dt = new DataTable();

        dt = dtFilter.Copy();

        dt = new DataView(dt, "", "", DataViewRowState.CurrentRows).ToTable(true, "Field1");

        string defaultPartNo = string.Empty;
        string option = string.Empty;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataTable dtnew = new DataView(dtFilter, "Field1='" + dt.Rows[i]["Field1"].ToString() + "' and PDefault='True'", "", DataViewRowState.CurrentRows).ToTable();



            if (i == 0)
            {
                if (dtnew.Rows.Count > 0)
                {
                    defaultPartNo = dtnew.Rows[0]["ModelNo"].ToString() + "-" + dtnew.Rows[0]["OptionID"].ToString();
                    option = dtnew.Rows[0]["OptionID"].ToString();
                }
                else
                {
                    defaultPartNo = dtFilter.Rows[0]["ModelNo"].ToString() + "-0";
                    option = "0";

                }
            }
            else
            {
                if (dtnew.Rows.Count > 0)
                {
                    defaultPartNo = defaultPartNo + dtnew.Rows[0]["OptionID"].ToString();
                    option = option + dtnew.Rows[0]["OptionID"].ToString();

                }
                else
                {
                    defaultPartNo = defaultPartNo + "0";
                    option = option + "0";
                }

            }

        }

        for (int j = 0; j < dtFilter.Rows.Count; j++)
        {
            //string STRING = option.Remove(Convert.ToInt32(dtFilter.Rows[j]["Field1"].ToString()) - 1, 1).ToString();

            dtFilter.Rows[j]["DefaultPartNo"] = defaultPartNo;

            //dtFilter.Rows[j]["ModelPartNo"] = defaultPartNo;
        }

        objReport.setcompanyAddress(CompanyAddress);
        objReport.setcompanyname(CompanyName, Session["CompId"].ToString());

        objReport.SetImage(Imageurl);
        objReport.setTitelName(Title);
        objReport.setCompanyArebicName(CompanyName_L);
        objReport.setCompanyTelNo(Companytelno);
        objReport.setCompanyFaxNo(CompanyFaxno);
        objReport.setCompanyWebsite(CompanyWebsite);
        objReport.setGroupByValue(GroupBy);
        try
        {
            objReport.setCurrency(dtFilter.Rows[0]["CurrencyId"].ToString());
        }
        catch
        {
        }

        objReport.setUserName(Session["UserId"].ToString());
        objReport.DataSource = dtFilter;
        objReport.DataMember = "sp_Inv_BOM_SelectRow";
        rptViewer.Report = objReport;

        rptToolBar.ReportViewer = rptViewer;
        //objReport.ExportToPdf(Server.MapPath("~/Temp/Contact-" + dtFilter.Rows[0]["Code"].ToString() + ".pdf"));
        //ViewState["Path"] = "Contact-" + dtFilter.Rows[0]["Code"].ToString() + ".pdf";
        //ViewState["Id"] = dtFilter.Rows[0]["Trans_Id"].ToString();

    }
}
