using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Web.Services;
using net.webservicex.www;
using DevExpress.XtraReports.UI;
using PegasusDataAccess;


public partial class Inventory_Report_PriceListReport : System.Web.UI.Page
{

    PriceList objReport = null;
    Common cmn = null;
    PageControlCommon pageCmn = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    CurrencyMaster objCurrency = null;
    Inv_ModelMaster ObjInvModelMaster = null;
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    DataAccessClass objda = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objReport = new PriceList(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        pageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
                    ViewState["ExchangeRate"] = "1";

            if (Request.QueryString["ModelId"] != null)
            {
                hdnmodelid.Value = Request.QueryString["ModelId"].ToString();



                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), "True"), "Trans_Id=" + hdnmodelid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count != 0)
                {
                    txtModelNo.Text = dt.Rows[0]["Model_No"].ToString();
                }


            }
            else
            {
                hdnmodelid.Value = "0";
            }
            hdncurrencyId.Value = Session["CurrencyId"].ToString();
            FillCurrency();
            FillModelCategory();
            //ddlCurrency.SelectedValue = Session["CurrencyId"].ToString();
        }
        btnshow_Click(null, null);

    }
    private void FillModelCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["Compid"].ToString().ToString());
        if (dsCategory.Rows.Count > 0)
        {
            ddlcategory.Items.Clear();

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            pageCmn.FillData((object)ddlcategory, dsCategory, "Category_Name", "Category_Id");

        }
        else
        {
            ddlcategory.Items.Clear();
        }
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {


        if (ddlCurrency.SelectedIndex == 0)
        {
            DisplayMessage("Select Currency");
            ddlCurrency.Focus();
            return;
        }
        hdncurrencyId.Value = ddlCurrency.SelectedValue;


        if (ddlCurrency.SelectedValue == Session["CurrencyId"].ToString())
        {
            ViewState["ExchangeRate"] = "1";
        }
        else
        {


            //try
            //{
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), hdncurrencyId.Value, Session["DBConnection"].ToString());
            //    ViewState["ExchangeRate"] = (obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(Session["CurrencyId"].ToString()).Rows[0]["Currency_Code"].ToString()), (Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(hdncurrencyId.Value).Rows[0]["Currency_Code"].ToString())).ToString());
            //}
            //catch
            //{
            //    DataTable dtcurr = new DataView(objCurrency.GetCurrencyMaster(), "Currency_ID='" + hdncurrencyId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    if (dtcurr.Rows.Count != 0)
            //    {
            //        ViewState["ExchangeRate"] = dtcurr.Rows[0]["Currency_Value"].ToString();
            //    }
            //}
        }


        string sql = "";

        if (ddlcategory.SelectedIndex != 0)
        {
            string ModelId = string.Empty;

            sql = "select Trans_Id from Inv_ModelMaster where Trans_Id in (select distinct(modelid) from Inv_Model_Category where CategoryId=" + ddlcategory.SelectedValue + ") and Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + "";

            DataTable dt = objda.return_DataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (ModelId == "")
                {
                    ModelId = dt.Rows[i]["Trans_Id"].ToString();
                }
                else
                {
                    ModelId = ModelId + "," + dt.Rows[i]["Trans_Id"].ToString();
                }

            }

            if (ModelId != "")
            {
                GetReport(ModelId);
            }
            else
            {
                DisplayMessage("Record Not Found");
                return;
            }

        }
        else if (txtModelNo.Text != "")
        {
            GetReport(hdnmodelid.Value);
        }

        else
        {
            GetReport("0");

        }
    }



    public void GetReport(string ModelId)
    {

        string GroupBy = string.Empty;
        DataTable dtFilter = new DataTable();

        InventoryDataSet rptdata = new InventoryDataSet();

        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Inv_BOM_SelectRowTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_BOM_SelectRowTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Inv_BOM_SelectRow, Convert.ToInt32(Session["CompId"].ToString()), 0, Convert.ToInt32(hdnmodelid.Value), 4);



        dtFilter = rptdata.sp_Inv_BOM_SelectRow;

        if (ModelId != "0")
        {
            dtFilter = new DataView(dtFilter, "ModelId in (" + ModelId + ")", "", DataViewRowState.CurrentRows).ToTable();
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
            Title = "PRICE LIST";
        else
            Title = Resources.Attendance.Price_List;


        DataTable dt = new DataTable();

        string defaultPartNo = string.Empty;
        string option = string.Empty;
        string subtitle = string.Empty;


        for (int j = 0; j < dtFilter.Rows.Count; j++)
        {


            defaultPartNo = "";
            option = "";
            dt = dtFilter.Copy();

            dt = new DataView(dt, "ModelId=" + dtFilter.Rows[j]["ModelId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable(true, "Field1");



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtnew = new DataView(dtFilter, "Field1=" + dt.Rows[i]["Field1"].ToString() + " and ModelId=" + dtFilter.Rows[j]["ModelId"].ToString() + " and PDefault='True'", "", DataViewRowState.CurrentRows).ToTable();



                if (i == 0)
                {
                    if (dtnew.Rows.Count > 0)
                    {
                        defaultPartNo = dtnew.Rows[0]["ModelNo"].ToString() + "-" + dtnew.Rows[0]["OptionID"].ToString();
                        option = dtnew.Rows[0]["OptionID"].ToString();
                    }
                    else
                    {
                        defaultPartNo = dtFilter.Rows[j]["ModelNo"].ToString() + "-0";
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

            string STRING = string.Empty;
            try
            {


                STRING = option.Remove(Convert.ToInt32(dtFilter.Rows[j]["Field1"].ToString()) - 1, 1).ToString();


                dtFilter.Rows[j]["DefaultPartNo"] = dtFilter.Rows[j]["ModelNo"].ToString() + "-" + STRING.Insert(Convert.ToInt32(dtFilter.Rows[j]["Field1"].ToString()) - 1, "<b><U>" + dtFilter.Rows[j]["OptionId"].ToString() + "</U></b>");

                dtFilter.Rows[j]["ModelPartNo"] = defaultPartNo;
            }
            catch
            {
            }
        }

        objReport.setcompanyAddress(CompanyAddress);
        objReport.setcompanyname(CompanyName, Session["CompId"].ToString());

        objReport.SetImage(Imageurl);

        //set title according condition

        if (ddlcategory.SelectedIndex != 0)
        {
            Title = ddlcategory.SelectedItem.Text.Trim().ToUpper();
            subtitle = "PRICE LIST";
        }

        else
            if (txtModelNo.Text != "")
        {
            try
            {
                Title = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), ModelId, "True").Rows[0]["Model_Name"].ToString().Trim().ToUpper();
                subtitle = "PRICE LIST";
            }
            catch
            {
            }
        }
        else
        {
            Title = "ALL CATEGORIES";
            subtitle = "PRICE LIST";
        }


        objReport.setTitelName(Title);
        objReport.setSubTitleName(subtitle);

        objReport.setCompanyArebicName(CompanyName_L);
        objReport.setCompanyTelNo(Companytelno);
        objReport.setCompanyFaxNo(CompanyFaxno);
        objReport.setCompanyWebsite(CompanyWebsite);
        objReport.setGroupByValue(GroupBy);
        try
        {
            objReport.setCurrency(hdncurrencyId.Value, Request.QueryString["ModelId"].ToString());
        }
        catch
        {
        }

        objReport.setUserName(Session["UserId"].ToString());
        objReport.DataSource = dtFilter;
        objReport.DataMember = "sp_Inv_BOM_SelectRow";
        rptViewer.Report = objReport;
        objReport.setSuggestedPrice("Suggested Price(" + objCurrency.GetCurrencyMasterById(hdncurrencyId.Value).Rows[0]["Currency_Code"].ToString() + ")");
        System.Globalization.DateTimeFormatInfo mfi = new DateTimeFormatInfo();
        string value = mfi.GetMonthName(DateTime.Now.Month).ToString() + " " + DateTime.Now.Year + " | " + objCurrency.GetCurrencyMasterById(hdncurrencyId.Value).Rows[0]["Currency_Code"].ToString();
        objReport.setCurrencyandMonthName(value);
        rptToolBar.ReportViewer = rptViewer;
        objReport.setExchangeRate(ViewState["ExchangeRate"].ToString());
        string PriceListLogo = string.Empty;
        PriceListLogo = "~/Images/erp_3.jpg";
        objReport.setPriceListLogo(PriceListLogo);
        string PageaHeader = string.Empty;
        PageaHeader = objCurrency.GetCurrencyMasterById(hdncurrencyId.Value).Rows[0]["Currency_Code"].ToString() + " Price List - " + mfi.GetMonthName(DateTime.Now.Month).ToString() + " " + DateTime.Now.Year;
        objReport.setPageHeader(PageaHeader);
    }
    protected void txtModelNo_TextChanged(object sender, EventArgs e)
    {
        if (txtModelNo.Text != "")
        {

            DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), "True"), "Model_No='" + txtModelNo.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                hdnmodelid.Value = dt.Rows[0]["Trans_Id"].ToString();
            }
            else
            {
                txtModelNo.Text = "";
                DisplayMessage("Select Model No");
                txtModelNo.Focus();
                hdnmodelid.Value = "";

            }
        }


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
        DataTable dtres = (DataTable)ViewState["MessageDt"];
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
    public void FillCurrency()
    {
        try
        {
            DataTable dt = objCurrency.GetCurrencyMaster();
            ddlCurrency.DataSource = dt;
            ddlCurrency.DataTextField = "Currency_Name";
            ddlCurrency.DataValueField = "Currency_Id";
            ddlCurrency.DataBind();
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;
        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;

        }
        if (ddlCurrency.Items.Count > 0)
        {
            try
            {
                ddlCurrency.SelectedValue = Session["CurrencyId"].ToString();

            }
            catch
            {
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModelNo(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, "Model_No like '%" + prefixText.ToString() + "%'", "Model_No Asc", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            //dt = dtTemp;
        }

        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_No"].ToString();
        }


        return txt;
    }
}
