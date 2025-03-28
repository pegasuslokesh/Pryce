using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_TransferOutReport : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;

    TransferOutPrint objTransferOutPrint = null;
    TransferInPrint objTransferInPrint = null;

    TransferDetailReport objTransferdetailReport = null;
    PurchaseRequestHeader InvPr = null;
    InventoryDataSet ObjInvdataset = new InventoryDataSet();
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    CurrencyMaster objCurrency = null;
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
        objTransferOutPrint = new TransferOutPrint(Session["DBConnection"].ToString());
        objTransferInPrint = new TransferInPrint(Session["DBConnection"].ToString());
        objTransferdetailReport = new TransferDetailReport(Session["DBConnection"].ToString());
        InvPr = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());


        btnSend.Visible = false;
        GetReport();

        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "145";
        Session["HeaderText"] = "Inventory Report";


    }

    public void GetReport()
    {
        strCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        if (Request.QueryString["LocId"] != null)
        {
            strLocationId = Request.QueryString["LocId"].ToString();
        }
        else
        {
            strLocationId = Session["LocId"].ToString();
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

        DataTable dt = new DataTable();


        if (Request.QueryString["TransId"].ToString() != "0")
        {
            try
            {


                InventoryDataSetTableAdapters.sp_Inv_TransferInOut_SelectRow_ReportTableAdapter ObjPrintAd = new InventoryDataSetTableAdapters.sp_Inv_TransferInOut_SelectRow_ReportTableAdapter();
                ObjPrintAd.Connection.ConnectionString = Session["DBConnection"].ToString();

                if (Request.QueryString["Type"].ToString() == "TO")
                {
                    ObjPrintAd.Fill(ObjInvdataset.sp_Inv_TransferInOut_SelectRow_Report, Convert.ToInt32(strCompId.ToString()), Convert.ToInt32(strBrandId.ToString()), Convert.ToInt32(strLocationId), Convert.ToInt32(Request.QueryString["TransId"].ToString()), 1);
                    dt = ObjInvdataset.sp_Inv_TransferInOut_SelectRow_Report;

                    if (dt.Rows.Count == 0)
                    {
                        return;
                    }


                    objTransferOutPrint.DataSource = dt;
                    objTransferOutPrint.DataMember = "sp_Inv_TransferInOut_SelectRow_Report";
                    rptViewer.Report = objTransferOutPrint;
                    rptToolBar.ReportViewer = rptViewer;
                    objTransferOutPrint.setcompanyname(CompanyName);
                    objTransferOutPrint.setSignature(signatureurl);
                    objTransferOutPrint.setCompanyArebicName(CompanyName_L);
                    objTransferOutPrint.setCompanyTelNo(Companytelno);
                    objTransferOutPrint.setCompanyFaxNo(CompanyFaxno);
                    objTransferOutPrint.setCompanyWebsite(CompanyWebsite);

                    objTransferOutPrint.setcompanyAddress(CompanyAddress);
                    objTransferOutPrint.SetImage(Imageurl);
                    objTransferOutPrint.ExportToPdf(Server.MapPath("~/Temp/Transfer Out -" + dt.Rows[0]["VoucherNo"].ToString() + ".pdf"));
                    ViewState["Path"] = "Transfer Out -" + dt.Rows[0]["VoucherNo"].ToString() + ".pdf";
                    btnSend.Visible = true;
                }
                else
                {
                    ObjPrintAd.Fill(ObjInvdataset.sp_Inv_TransferInOut_SelectRow_Report, Convert.ToInt32(strCompId.ToString()), Convert.ToInt32(strBrandId.ToString()), Convert.ToInt32(strLocationId), Convert.ToInt32(Request.QueryString["TransId"].ToString()), 2);
                    dt = ObjInvdataset.sp_Inv_TransferInOut_SelectRow_Report;


                    if (dt.Rows.Count == 0)
                    {
                        return;
                    }

                    objTransferInPrint.DataSource = dt;
                    objTransferInPrint.DataMember = "sp_Inv_TransferInOut_SelectRow_Report";
                    rptViewer.Report = objTransferInPrint;
                    rptToolBar.ReportViewer = rptViewer;
                    objTransferInPrint.setcompanyname(CompanyName);
                    objTransferInPrint.setSignature(signatureurl);
                    objTransferInPrint.setCompanyArebicName(CompanyName_L);
                    objTransferInPrint.setCompanyTelNo(Companytelno);
                    objTransferInPrint.setCompanyFaxNo(CompanyFaxno);
                    objTransferInPrint.setCompanyWebsite(CompanyWebsite);

                    objTransferInPrint.setcompanyAddress(CompanyAddress);
                    objTransferInPrint.SetImage(Imageurl);
                    objTransferInPrint.ExportToPdf(Server.MapPath("~/Temp/Transfer In -" + dt.Rows[0]["VoucherNo"].ToString() + ".pdf"));
                    ViewState["Path"] = "Transfer In -" + dt.Rows[0]["VoucherNo"].ToString() + ".pdf";
                    btnSend.Visible = true;
                }

            }
            catch
            {
            }
        }
        else
        {
            dt = (DataTable)Session["dtTranferOut"];

            string[] strParam = Common.ReportHeaderSetup("Location", Session["LocId"].ToString(), Session["DBConnection"].ToString());
            objTransferdetailReport.setcompanyname(strParam[0].ToString());
            objTransferdetailReport.setCompanyArebicName(strParam[1].ToString());
            objTransferdetailReport.setReportTitle("TRANSFER OUT REPORT");
            objTransferdetailReport.setDateCriteria(Session["DateCrediteria"].ToString());
            objTransferdetailReport.setCurrencySymbol("Total(" + objCurrency.GetCurrencyMasterById(Session["CurrencyId"].ToString()).Rows[0]["Currency_Symbol"].ToString() + ")");
            objTransferdetailReport.SetImage("~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString());
            objTransferdetailReport.DataSource = dt;
            objTransferdetailReport.DataMember = "DtTransferOutReport";
            rptViewer.Report = objTransferdetailReport;
            rptToolBar.ReportViewer = rptViewer;
        }
    }


    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Type"].ToString() == "TO")
        {
            Response.Redirect("../EmailSystem/SendMail.aspx?Page=TO&&URL=" + ViewState["Path"].ToString() + "'");
        }
        else
        {
            Response.Redirect("../EmailSystem/SendMail.aspx?Page=TI&&URL=" + ViewState["Path"].ToString() + "'");

        }


    }
}
