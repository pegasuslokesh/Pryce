using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Purchase_Report_PurchaseRequest : BasePage
{
    Inv_ProductMaster ObjProductMaster = null;
    LocationMaster objLocation = null;
    SystemParameter ObjSysParam = null;
    PurchaseRequestHeader objPRequestHeader = null;

    PRequestDetail objReport = null;
    PRequestDetailByRequestDate objReportByDate = null;
    PRequestDetailByProduct objReporybyProduct = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    PRequestHeaderByDepartment objReportByDepartment = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objPRequestHeader = new PurchaseRequestHeader(Session["DBConnection"].ToString());

        objReport = new PRequestDetail(Session["DBConnection"].ToString());
        objReportByDate = new PRequestDetailByRequestDate(Session["DBConnection"].ToString());
        objReporybyProduct = new PRequestDetailByProduct(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objReportByDepartment = new PRequestHeaderByDepartment(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["DtFilter"] = null;

            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtFromDate.Focus();
            ChkProduct.Visible = false;
            ChkRequestNo.Visible = false;
            txtFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            if (RbtnDetail.Checked == true && Session["ReportType"]!=null)
                Get_Details_Report();
            if (rbtnheader.Checked == true && Session["ReportType"] != null)
                Get_Header_Report();
        }
        AllPageCode();

    }
    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("173", (DataTable)Session["ModuleName"]);
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


    protected void rbtnheader_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnheader.Checked == true)
        {
            txtFromDate.Focus();
            txtProductName.Visible = false;
            lblProductName.Visible = false;
            //lblcolon.Visible = false;

            chkdepartment.Visible = true;
            ChkRequestNo.Visible = false;
            ChkProduct.Visible = false;

        }


    }
    protected void RbtnDetail_CheckedChanged(object sender, EventArgs e)
    {
        if (RbtnDetail.Checked == true)
        {
            txtFromDate.Focus();
            txtProductName.Visible = true;
            lblProductName.Visible = true;
            //lblcolon.Visible = true;
            chkdepartment.Visible = false;
            ChkRequestNo.Visible = true;
            ChkProduct.Visible = false;

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
        Session["ReportType"] = null;
        Session["ReportHeader"] = null;
        Session["Parameter"] = null;
        Session["DtFilter"] = null;
        if (rbtnheader.Checked == false && RbtnDetail.Checked == false)
        {
            DisplayMessage("Select the Report Type(Header or Detail)");
            rbtnheader.Focus();
            return;
        }

        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;

        if (rbtnheader.Checked == true)
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseRequestHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseRequestHeader_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseRequestHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseRequestHeader_SelectRow_Report;

            if (chkdepartment.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Request Header Report By Department";
                Session["ReportType"] = "0";
                //here we calling the PRequestHeaderByDepartment.cs report object
            }
            else
            {
                Session["ReportHeader"] = "Purchase Request Header Report By Request date";
                Session["ReportType"] = "1";
                //here we calling the PRequestHeader.cs report object

            }


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDate.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From " + txtFromDate.Text + "  To " + txtToDate.Text;




            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "RequestDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["RequestDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;

            }
            if (txtRequestNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "RequestNo='" + txtRequestNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Request Header Report By Request No.";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + ",RequestNo.";
                }

            }
            if (ddlStatus.SelectedIndex > 0)
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Status='" + ddlStatus.SelectedItem.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Request Header Report By Status(" + ddlStatus.SelectedItem.Text + ")";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + ",Status(" + ddlStatus.SelectedItem.Text + ")";
                }
            }







            try
            {
                DtFilter = new DataView(DtFilter, "Department_Name<>' '", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;

                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PRequestHeaderReport.aspx','window','width=1024');", true);
                Get_Header_Report();


                //Response.Redirect("../Purchase_Report/PRequestHeaderReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");
                rbtnheader.Focus();
                return;
            }
        }
        if (RbtnDetail.Checked == true)
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseRequestDetail_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseRequestDetail_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseRequestDetail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseRequestDetail_Report;
            if (ChkProduct.Checked == false && ChkRequestNo.Checked == false)
            {
                Session["ReportHeader"] = "Purchase Request Detail Report By RequestDate";
                Session["ReportType"] = "0";
            }
            if (ChkRequestNo.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Request Detail Report By RequestNo.";
                Session["ReportType"] = "1";
                //here we calling the PRequestHeaderByDepartment.cs report object
            }
            if (ChkProduct.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Request Detail Report By Product";
                Session["ReportType"] = "2";
                //here we calling the PRequestHeader.cs report object

            }

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDate.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                Session["Parameter"] = "FilterBy: From " + txtFromDate.Text + "  To " + txtToDate.Text;

            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;

            }
            if (txtRequestNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "RequestNo='" + txtRequestNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (ChkRequestNo.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Request Detail Report By Request No.";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + ",RequestNo.";
                    }
                }




            }
            if (ddlStatus.SelectedIndex > 0)
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Status='" + ddlStatus.SelectedItem.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Request Detail Report By Status(" + ddlStatus.SelectedItem.Text + ")";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + ",Status(" + ddlStatus.SelectedItem.Text + ")";
                }


            }





            if (txtProductName.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (ChkProduct.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Request Detail Report By Product";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + ",Product";
                    }
                }
                Session["DtFilter"] = DtFilter;

                //Response.Redirect("../Purchase_Report/PRequestdetailReportByProduct.aspx");



            }

            if (DtFilter.Rows.Count > 0)
            {

                Session["DtFilter"] = DtFilter;
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PRequestdetailReport.aspx','window','width=1024');", true);
                Get_Details_Report();

                // Response.Redirect("../Purchase_Report/PRequestdetailReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");
                RbtnDetail.Focus();
                return;
            }
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        rbtnheader.Checked = true;
        RbtnDetail.Checked = false;
        txtFromDate.Focus();

        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductName.Text = "";
        txtRequestNo.Text = "";
        ddlStatus.SelectedIndex = 0;
        chkdepartment.Checked = false;



    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text != "")
        {
            //DataTable dtProduct = objProductM.GetProductMasterTrueAll(StrCompId);
            //dtProduct = new DataView(dtProduct, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtProduct.Rows.Count > 0)
            //{
            DataTable dtProduct = new DataTable();

            try
            {
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductName.Text.ToString());
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                hdnProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();





            }
            else
            {
                DisplayMessage("Select Product in suggestion only");
                txtProductName.Text = "";
                txtProductName.Focus();
                return;
            }
        }


    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(),HttpContext.Current.Session["LocId"].ToString());
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["EProductName"].ToString();
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["locId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["EProductName"].ToString();
                    }
                }
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRequestNo(string prefixText, int count, string contextKey)
    {
        PurchaseRequestHeader objPRequestHeader = new PurchaseRequestHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = objPRequestHeader.GetPurchaseRequestHeaderTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["RequestNo"].ToString();
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = objPRequestHeader.GetPurchaseRequestHeaderTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["RequestNo"].ToString();
                    }
                }
            }
        }
        return str;
    }
    void Get_Details_Report()
    {
        DataTable Dt = new DataTable();

        if (Session["DtFilter"] != null)
        {
            Dt = (DataTable)Session["DtFilter"];
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
            if (Session["Lang"].ToString() == "1")
            {
                CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            }
            else
            {
                CompanyName = DtCompany.Rows[0]["Company_Name_L"].ToString();

            }
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();


        }
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                LocationName = DtLocation.Rows[0]["Location_Name"].ToString();

            }
            else
            {
                LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();

            }
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
            if (Session["Lang"].ToString() == "1")
            {
                BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
            }
            else
            {
                BrandName = DtBrand.Rows[0]["Brand_Name_L"].ToString();

            }
        }




        Title = "Purchase Request Report";

        if (Session["ReportType"].ToString() == "0")
        {
            lblHeader.Text = Session["ReportHeader"].ToString();
            objReportByDate.setcompanyAddress(CompanyAddress);
            objReportByDate.setcompanyname(CompanyName);
            objReportByDate.setBrandName(BrandName);
            objReportByDate.setLocationName(LocationName);
            objReportByDate.settitle(Session["ReportHeader"].ToString());
            objReportByDate.SetImage(Imageurl);

            if (Session["Parameter"] == null)
            {
                objReportByDate.SetDateCriteria("");
            }
            else
            {
                objReportByDate.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportByDate.setUserName(Session["UserId"].ToString());

            objReportByDate.setArabic();
            objReportByDate.DataSource = Dt;
            objReportByDate.DataMember = "sp_Inv_PurchaseRequestDetail_Report";
            rptViewer.Report = objReportByDate;
            objReportByDate.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        if (Session["ReportType"].ToString() == "1")
        {
            lblHeader.Text = Session["ReportHeader"].ToString();
            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName);
            objReport.setBrandName(BrandName);
            objReport.setLocationName(LocationName);
            objReport.settitle(Session["ReportHeader"].ToString());
            objReport.SetImage(Imageurl);
            objReport.setArabic();
            if (Session["Parameter"] == null)
            {
                objReport.SetDateCriteria("");
            }
            else
            {
                objReport.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReport.setUserName(Session["UserId"].ToString());


            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Inv_PurchaseRequestDetail_Report";
            rptViewer.Report = objReport;
            objReport.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        if (Session["ReportType"].ToString() == "2")
        {
            lblHeader.Text = Session["ReportHeader"].ToString();
            objReporybyProduct.setcompanyAddress(CompanyAddress);
            objReporybyProduct.setcompanyname(CompanyName);
            objReporybyProduct.setBrandName(BrandName);
            objReporybyProduct.setLocationName(LocationName);
            objReporybyProduct.settitle(Session["ReportHeader"].ToString());
            objReporybyProduct.SetImage(Imageurl);

            if (Session["Parameter"] == null)
            {
                objReporybyProduct.SetDateCriteria("");
            }
            else
            {
                objReporybyProduct.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReporybyProduct.setUserName(Session["UserId"].ToString());


            objReporybyProduct.DataSource = Dt;
            objReporybyProduct.DataMember = "sp_Inv_PurchaseRequestDetail_Report";
            rptViewer.Report = objReporybyProduct;
            objReporybyProduct.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?Page=PR&&Url=" + ViewState["Path"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);        
    }
    void Get_Header_Report()
    {
        DataTable Dt = new DataTable();

        if (Session["DtFilter"] != null)
        {
            Dt = (DataTable)Session["DtFilter"];
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
            if (Session["Lang"].ToString() == "1")
            {
                CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            }
            else
            {
                CompanyName = DtCompany.Rows[0]["Company_Name_L"].ToString();

            }
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




        Title = "Purchase Request Report";

        if (Session["ReportType"].ToString() == "1")
        {
            lblHeader.Text = Resources.Attendance.Purchase_Request_Header_Report_By_Request_Date;
            objReport.setcompanyAddress(CompanyAddress);
            objReport.setcompanyname(CompanyName);
            objReport.setBrandName(BrandName);
            objReport.setLocationName(LocationName);
            objReport.settitle(Resources.Attendance.Purchase_Request_Header_Report_By_Request_Date);
            objReport.SetImage(Imageurl);
            if (Session["Parameter"] == null)
            {
                objReport.SetDateCriteria("");
            }
            else
            {
                objReport.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReport.setUserName(Session["UserId"].ToString());
            objReport.setArabic();
            objReport.DataSource = Dt;
            objReport.DataMember = "sp_Inv_PurchaseRequestHeader_SelectRow";
            rptViewer.Report = objReport;
            objReport.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));

        }
        else
        {
            lblHeader.Text = Resources.Attendance.Purchase_Request_Header_Report_By_Department;
            objReportByDepartment.setcompanyAddress(CompanyAddress);
            objReportByDepartment.setcompanyname(CompanyName);
            objReportByDepartment.setBrandName(BrandName);
            objReportByDepartment.setLocationName(LocationName);
            objReportByDepartment.settitle(Resources.Attendance.Purchase_Request_Header_Report_By_Department);
            objReportByDepartment.SetImage(Imageurl);
            if (Session["Parameter"] == null)
            {
                objReportByDepartment.SetDateCriteria("");
            }
            else
            {
                objReportByDepartment.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportByDepartment.setUserName(Session["UserId"].ToString());
            objReportByDepartment.SetArabic();

            objReportByDepartment.DataSource = Dt;
            objReportByDepartment.DataMember = "sp_Inv_PurchaseRequestHeader_SelectRow";
            rptViewer.Report = objReportByDepartment;
            objReportByDepartment.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));


        }
        ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;
    }
}


