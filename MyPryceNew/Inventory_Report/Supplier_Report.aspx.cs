using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Report_Supplier_Report : BasePage
{
    Set_Suppliers objSupplier = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster ObjContactMaster = null;

    SupplierDetail objReport = new SupplierDetail();
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "166", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Session["dtFilter_Supplier_R"] = null;
        }
        else
        {
            GetReport();
        }
        AllPageCode();
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("166", (DataTable)Session["ModuleName"]);
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
        Session["dtFilter_Supplier_R"] = null;

        DataTable DtFilter = new DataTable();
        InventoryDataSet ObjInvDataset = new InventoryDataSet();
        ObjInvDataset.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Set_Suppliers_Detail_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Set_Suppliers_Detail_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();

        adp.Fill(ObjInvDataset.sp_Set_Suppliers_Detail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()));
        DtFilter = ObjInvDataset.sp_Set_Suppliers_Detail_Report;
        if (txtSupplierName.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "Trans_Id=" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch

            {
            }

        }
        if (DtFilter.Rows.Count > 0)
        {

            Session["dtFilter_Supplier_R"] = DtFilter;
            //Response.Redirect("../Inventory_Report/Supplier_Detail_Report.aspx");
            GetReport();
        }
        else
        {
            DisplayMessage("Record Not Found");
            txtSupplierName.Focus();
            return;
        }

    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        if (txtSupplierName.Text != "")
        {
            DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierName.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnSupplierId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("select supplier in suggestion only");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
                return;
            }


        }


    }
    #region ServiceMethod
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_PurchaseQuoteDetail objPQDetail = new Inv_PurchaseQuoteDetail(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_PurchaseQuoteHeader objPQHeader = new Inv_PurchaseQuoteHeader(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = new DataTable();
        DataTable dtContAll = ObjContMaster.GetContactTrueAllData();
        if (HttpContext.Current.Session["RPQ_No"] == null)
        {
            dtSupplier = ObjSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
        }
        else
        {
            if (HttpContext.Current.Session["RPQ_No"] == "PQ")
            {
                dtSupplier = objPQDetail.GetQuoteDetailByRPQ_No(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objPQHeader.GetQuoteHeaderAllDataByRPQ_No(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["RPQ_No"].ToString()).Rows[0]["Trans_Id"].ToString());
            }
            else
            {
                dtSupplier = ObjSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
            }
        }
        DataTable dtMain = new DataTable();
        dtMain = dtSupplier;
        //for (int i = 0; i < dtSupplier.Rows.Count; i++)
        //{
        //    dtMain.Merge(new DataView(dtContAll, "Supplier_Id='" + dtSupplier.Rows[i]["Supplier_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable());
        //}


        string filtertext = "Name like '" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        //if (dtCon.Rows.Count == 0)
        //{
        //    dtCon = dtMain.Copy();
        //}
        dtCon = dtCon.DefaultView.ToTable(true, "Name", "Trans_Id");
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }

    void GetReport()
    {
        DataTable Dt = new DataTable();

        if (Session["dtFilter_Supplier_R"] != null)
        {
            Dt = (DataTable)Session["dtFilter_Supplier_R"];
        }
        DataTable DtSupplier = new DataTable();
        DtSupplier.Columns.Add("Supplier_Code");
        DtSupplier.Columns.Add("Supplier_Name");
        DtSupplier.Columns.Add("Address");
        DtSupplier.Columns.Add("Contact_No");
        DtSupplier.Columns.Add("Email_Id");
        DtSupplier.Columns.Add("Company");
        DtSupplier.Columns.Add("Supplier_Id");

        for (int i = 0; i < Dt.Rows.Count; i++)
        {
            string ContactNo = "";
            string EmailId = "";
            string Address = "";
            DataRow dr = DtSupplier.NewRow();
            dr["Supplier_Id"] = Dt.Rows[i]["Trans_Id"].ToString();
            dr["Supplier_Code"] = Dt.Rows[i]["Code"].ToString();
            dr["Supplier_Name"] = Dt.Rows[i]["Name"].ToString();

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
            DtSupplier.Rows.Add(dr);

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



        Title = "Supplier Detail Report";
        objReport.setcompanyAddress(CompanyAddress);
        objReport.setcompanyname(CompanyName);
        objReport.setBrandName(BrandName);
        objReport.setLocationName(LocationName);
        objReport.settitle(Title);
        objReport.SetImage(Imageurl);
        objReport.setUserName(Session["UserId"].ToString());


        objReport.DataSource = DtSupplier;
        objReport.DataMember = "Supplier_Detail";
        rptViewer.Report = objReport;
        rptToolBar.ReportViewer = rptViewer;

    }

    #endregion
}
