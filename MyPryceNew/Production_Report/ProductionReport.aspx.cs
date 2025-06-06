﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Production_Report_ProductionReport : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;

    ProductionPrint objReport = null;
    DailyProductionPrint objDailyProductionPrint = null;
    PurchaseRequestHeader InvPr = null;
    InventoryDataSet ObjInvdataset = null;
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

        objReport = new ProductionPrint(Session["DBConnection"].ToString());
        objDailyProductionPrint = new DailyProductionPrint(Session["DBConnection"].ToString());
        InvPr = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());

        GetReport();
        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "168";
        Session["HeaderText"] = "Production Report";
    }
    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        //ObjInvdataset.EnforceConstraints = false;
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
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtLocation.Rows[0]["Field2"].ToString();


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


        if (Request.QueryString["Type"].ToString() == "PR" || Request.QueryString["Type"].ToString() == "PP" || Request.QueryString["Type"].ToString() == "PF" || Request.QueryString["Type"].ToString() == "ALL")
        {
            DataTable dt = (DataTable)Session["DtProduction"];

            objReport.DataSource = dt;
            objReport.DataMember = "sp_Inv_ProductionRequestHeader_SelectRow_Report";
            ReportViewer1.Report = objReport;
            ReportToolbar1.ReportViewer = ReportViewer1;
            objReport.setcompanyname(LocationName);
            objReport.setSignature(signatureurl);
            objReport.setCompanyArebicName(CompanyName_L);
            objReport.setCompanyTelNo(Companytelno);
            objReport.setCompanyFaxNo(CompanyFaxno);
            objReport.setCompanyWebsite(CompanyWebsite);
            objReport.setcompanyAddress(CompanyAddress);
            objReport.SetImage(Imageurl);

            if (Request.QueryString["Type"].ToString() == "PR")
            {
                objReport.setReportTitle("PRODUCTION REQUEST REPORT");
            }
            else
                if (Request.QueryString["Type"].ToString() == "PP")
                {
                    objReport.setReportTitle("PRODUCTION PROCESS REPORT");
                }
                else
                    if (Request.QueryString["Type"].ToString() == "PF")
                    {
                        objReport.setReportTitle("PRODUCTION FINISH REPORT");
                    }
                    else
                        if (Request.QueryString["Type"].ToString() == "All")
                        {
                            objReport.setReportTitle("PRODUCTION REPORT");
                        }
        }
        else
        {

            DataTable dt = (DataTable)Session["DtDailyProduction"];

            objDailyProductionPrint.DataSource = dt;
            objDailyProductionPrint.DataMember = "DtDailyProductionReport";
            ReportViewer1.Report = objDailyProductionPrint;
            ReportToolbar1.ReportViewer = ReportViewer1;
            objDailyProductionPrint.setcompanyname(LocationName);
            objDailyProductionPrint.setSignature(signatureurl);
            objDailyProductionPrint.setCompanyArebicName(CompanyName_L);
            objDailyProductionPrint.setCompanyTelNo(Companytelno);
            objDailyProductionPrint.setCompanyFaxNo(CompanyFaxno);
            objDailyProductionPrint.setCompanyWebsite(CompanyWebsite);

            if (Session["DtFromDailyProduction"] != null)
            {
                objDailyProductionPrint.setDateCriteria("From : " + Session["DtFromDailyProduction"].ToString() + "  To : " + Session["DtToDailyProduction"].ToString());
            }
            objDailyProductionPrint.SetImage(Imageurl);
            objReport.setReportTitle("DAILY PRODUCTION REPORT");
        }
        
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        Response.Redirect("../EmailSystem/SendMail.aspx?Page=PS&&URL=" + ViewState["Path"].ToString() + "'");
    }
}