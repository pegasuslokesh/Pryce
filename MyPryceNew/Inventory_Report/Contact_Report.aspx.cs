using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_Contact_Report : System.Web.UI.Page
{
    ContactReport objReport = null;
    ContactReport_ByGroup objReport_ByGroup = null;

    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    Ems_GroupMaster ObjGroupMaster = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new ContactReport(Session["DBConnection"].ToString());
        objReport_ByGroup = new ContactReport_ByGroup(Session["DBConnection"].ToString());

        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjGroupMaster = new Ems_GroupMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "245", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            bindAllGroup();
        }
        if (ddlGroupBY.SelectedIndex > 0)
        {
            GetReport();
        }

        AllPageCode();
    }
    public void DisplayMessage(string Message)
    {

        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Message + "');", true); ;
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
        if (ddlGroupBY.SelectedIndex <= 0)
        {
            DisplayMessage("Select Contact Group");
            return;
        }

        GetReport();
    }
    public void bindAllGroup()
    {
        DataTable dtGroup = ObjGroupMaster.GetGroupMasterTrueAllData();
        if (dtGroup.Rows.Count > 0)
        {
            dtGroup = new DataView(dtGroup, "", "Group_Name", DataViewRowState.CurrentRows).ToTable();
            ddlGroupBY.DataSource = dtGroup;
            ddlGroupBY.DataTextField = "Group_Name";
            ddlGroupBY.DataValueField = "Group_Id";
            ddlGroupBY.DataBind();
            ddlGroupBY.Items.Insert(0, "--Select--");

        }
    }

    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("245", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code



        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;


    }
    void GetReport()
    {

        DataTable dtFilter = new DataTable();

        InventoryDataSet rptdata = new InventoryDataSet();

        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Ems_ContactMaster_SelectRowTableAdapter adp = new InventoryDataSetTableAdapters.sp_Ems_ContactMaster_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Ems_ContactMaster_SelectRow, 0, "", 7);

        dtFilter = rptdata.sp_Ems_ContactMaster_SelectRow;

        if (ddlContactType.SelectedIndex != 0)
        {
            try
            {
                dtFilter = new DataView(dtFilter, "Status='" + ddlContactType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }

        //if (ddlGroupBY.SelectedIndex == 0)
        //{
        //    DataTable dtReport = new DataTable();

        //    dtReport = dtFilter.Copy();


        //    //try
        //    //{
        //    //    dtReport = dtReport.DefaultView.ToTable(true, "Trans_Id", "Code", "Name", "Name_L", "Status", "Permanent_EmailId", "Permanent_Mobileno", "Contact_Company_Name", "DepartmentName", "DesignationName");


        //    //    dtReport.Columns.Add("Customercode");
        //    //    dtReport.Columns.Add("suppliercode");
        //    //}
        //    //catch
        //    //{
        //    //}


        //    //for (int i = 0; i < dtReport.Rows.Count; i++)
        //    //{

        //    //    DataTable dtcheckRecord = dtFilter.Copy();

        //    //    try
        //    //    {
        //    //        dtcheckRecord = new DataView(dtcheckRecord, "Trans_Id=" + dtReport.Rows[i]["Trans_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //    //    }
        //    //    catch
        //    //    {
        //    //    }

        //    //    for (int j = 0; j < dtcheckRecord.Rows.Count; j++)
        //    //    {
        //    //        if (dtcheckRecord.Rows[j]["Group_Id"].ToString() == "1")
        //    //        {
        //    //            dtReport.Rows[i]["Customercode"] = dtcheckRecord.Rows[j]["Customercode"].ToString();

        //    //        }
        //    //        if (dtcheckRecord.Rows[j]["Group_Id"].ToString() == "2")
        //    //        {
        //    //            dtReport.Rows[i]["suppliercode"] = dtcheckRecord.Rows[j]["suppliercode"].ToString();
        //    //        }
        //    //    }
        //    //}


        //    dtFilter = dtReport.Copy();



        if (ddlGroupBY.SelectedIndex > 0)
        {
            try
            {
                dtFilter = new DataView(dtFilter, "Group_id='" + ddlGroupBY.SelectedValue + "'", "Contact_Company_Name,Name", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
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
            Title = "CONTACT REPORT";
        else
            Title = Resources.Attendance.Contact_Report;

        if (ddlGroupBY.SelectedIndex == 0)
        {
            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName, Session["CompId"].ToString());

            objReport.SetImage(Imageurl);
            objReport.setTitelName(Title);
            objReport.setCompanyArebicName(CompanyName_L);
            objReport.setCompanyTelNo(Companytelno);
            objReport.setCompanyFaxNo(CompanyFaxno);
            objReport.setCompanyWebsite(CompanyWebsite);

            objReport.setUserName(Session["UserId"].ToString());
            objReport.DataSource = dtFilter;
            objReport.DataMember = "sp_Ems_ContactMaster_SelectRow";
            rptViewer.Report = objReport;
        }
        else
        {
            objReport.setTitelName(Title);
            objReport_ByGroup.setcompanyAddress(CompanyAddress);
            objReport_ByGroup.setcompanyname(CompanyName, Session["CompId"].ToString());

            objReport_ByGroup.SetImage(Imageurl);
            objReport_ByGroup.setCompanyArebicName(CompanyName_L);
            objReport_ByGroup.setCompanyTelNo(Companytelno);
            objReport_ByGroup.setCompanyFaxNo(CompanyFaxno);
            objReport_ByGroup.setCompanyWebsite(CompanyWebsite);

            objReport_ByGroup.setUserName(Session["UserId"].ToString());
            objReport_ByGroup.DataSource = dtFilter;
            objReport_ByGroup.DataMember = "sp_Ems_ContactMaster_SelectRow";
            rptViewer.Report = objReport_ByGroup;
        }
        rptToolBar.ReportViewer = rptViewer;
        //objReport.ExportToPdf(Server.MapPath("~/Temp/Contact-" + dtFilter.Rows[0]["Code"].ToString() + ".pdf"));
        //ViewState["Path"] = "Contact-" + dtFilter.Rows[0]["Code"].ToString() + ".pdf";
        //ViewState["Id"] = dtFilter.Rows[0]["Trans_Id"].ToString();

    }

    //protected void btnSend_Click(object sender, EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?Page=Contact&&Url=" + ViewState["Path"].ToString() + "&&Id=" + ViewState["Id"].ToString() + "&&Type=Contact','','height=650,width=900,scrollbars=Yes');", true);

    //}
}
