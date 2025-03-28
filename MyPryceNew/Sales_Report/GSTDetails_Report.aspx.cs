using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class GSTDetails_Report : System.Web.UI.Page
{
    Common cmn = null;
    DataAccessClass objDa = null;
    GSTDetailReport rptTaxTransaction = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        rptTaxTransaction = new GSTDetailReport(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());

        StrUserId = Session["UserId"].ToString();


        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "193", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["dtFilter_Sales_Orders"] = null;

            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtFromDate.Focus();
            txtFromDate.Text = "";
            txtToDate.Text = "";
            FillTaxCategory();
            //pnlReport.Visible = false;
            //pnlFilterRecords.Visible = true;
        }


        if (IsPostBack)
        {
            getReport();
        }
        AllPageCode();

    }
    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("193", (DataTable)Session["ModuleName"]);
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

        getReport();

    }

    protected void getReport()
    {

       

        if (txtFromDate.Text == "" || txtToDate.Text == "")
        {
            DisplayMessage("Please select valid date");
            return;
        }
        //txtFromDate.Text = "01-Sep-2017";
        DateTime dtFromDate = DateTime.Parse(txtFromDate.Text);
        DateTime dtToDate = DateTime.Parse(txtToDate.Text);
        if (dtFromDate > dtToDate)
        {
            DisplayMessage("From date can not be grater then To date, Please select valid date");
            return;
        }



        bool isPostedOnly = false;
        if (rbPosted.Checked)
        {
            isPostedOnly = true;
        }
        else
        {
            isPostedOnly = false;
        }
        //ddlTaxCategory.SelectedValue = "2";

        //if (ddlTaxCategory.SelectedValue == "0" && ddlTaxCategory.SelectedValue == "")
        //{
        //    DisplayMessage("Please select tax type");
        //    ddlTaxCategory.Focus();
        //    return;
        //}
        if (ddlTaxCategory.SelectedValue == "")
        {
            DisplayMessage("Please select transaction");
            ddlTaxCategory.Focus();
            return;
        }

        DataTable DtFilter = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;
        Session["ReportType"] = "0";
        SalesDataSetTableAdapters.sp_Sales_GSTWithInvoiceTableAdapter adp = new SalesDataSetTableAdapters.sp_Sales_GSTWithInvoiceTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        DtFilter = ObjSalesDataset.sp_Sales_GSTWithInvoice;
        adp.Fill(ObjSalesDataset.sp_Sales_GSTWithInvoice, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(ddlTaxCategory.SelectedValue), isPostedOnly, dtFromDate, dtToDate, ddlTransaction.SelectedValue);

        Session["ReportHeader"] = "TAX TRANSACTION DETAIL";


        DevExpress.XtraReports.UI.XRTableCell ObjXrCell = (DevExpress.XtraReports.UI.XRTableCell)rptTaxTransaction.FindControl("xrTableCell10", true);

        if (DtFilter.Rows.Count > 0)
        {
            // get basic detail from system 
            string CompanyName = "";
            string CompanyAddress = "";
            string Imageurl = "";
            string BrandName = "";
            string LocationName = "";
            string strDateRange = txtFromDate.Text + " To " + txtToDate.Text;
            //string FromDate = "";
            //string Todate = "";
            //string Title = "";

            string[] strParam = Common.ReportHeaderSetup("Location", Session["LocId"].ToString(), Session["DBConnection"].ToString());
            CompanyName = strParam[0].ToString();
            //CompanyName_L = strParam[1].ToString();
            CompanyAddress = strParam[2].ToString();
            string Companytelno = strParam[3].ToString();
            string CompanyFaxno = strParam[4].ToString();
            string CompanyWebsite = strParam[5].ToString();
            //string CompanyGSTIN = strParam[7].ToString();
            Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();

            string sqlQuery = "select top 1 field1 from set_companymaster where isactive='true' and company_id=" + Session["CompId"].ToString();
            string CompanyGSTIN = string.Empty;
            CompanyGSTIN = objDa.get_SingleValue(sqlQuery);


            DataTable Dt_Location = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
            if (Dt_Location.Rows.Count > 0)
            {
                if (Dt_Location.Rows[0]["Field4"].ToString() != "")
                {
                    rptTaxTransaction.setCompanyGstinNo(Dt_Location.Rows[0]["Field4"].ToString());
                    rptTaxTransaction.setCompanyTAX_Name("GSTIN No. -");

                    ObjXrCell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Sales_GSTWithInvoice.GSTIN_No")});

                }
                else if (Dt_Location.Rows[0]["Field5"].ToString() != "")
                {
                    rptTaxTransaction.setCompanyGstinNo(Dt_Location.Rows[0]["Field5"].ToString());
                    rptTaxTransaction.setCompanyTAX_Name("TRN No. -");

                    ObjXrCell.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "sp_Sales_GSTWithInvoice.TRN_No")});
                }
            }



            rptTaxTransaction.SetImage(Imageurl);

            //rptTaxTransaction.setBrandName(BrandName);
            rptTaxTransaction.setCmpTelephoneNo(Companytelno);
            rptTaxTransaction.setCmpWebAddress(CompanyWebsite);
            rptTaxTransaction.setCmpFaxNo(CompanyFaxno);
            //rptTaxTransaction.setCompanyGstinNo(CompanyGSTIN);

            rptTaxTransaction.setCompanyName(CompanyName);

            rptTaxTransaction.setCompanyAddress(CompanyAddress);
            rptTaxTransaction.setDateRange(strDateRange);

            rptTaxTransaction.setPrintedBy(Session["UserId"].ToString());
            rptTaxTransaction.setReportTitle(ddlTaxCategory.SelectedItem.Text + " TRANSACTIONS REPORT");
            //rptTaxTransaction.setheaderName(Resources.Attendance.Id, Resources.Attendance.Name, Resources.Attendance.Date, Resources.Attendance.Shift_Name, Resources.Attendance.On_Duty_Time, Resources.Attendance.Off_Duty_Time, Resources.Attendance.In_Time, Resources.Attendance.Out_Time, Resources.Attendance.Absent_Count);
            rptTaxTransaction.DataSource = DtFilter;
            rptTaxTransaction.DataMember = "sp_Sales_GSTWithInvoice";
            rptViewer.Report = rptTaxTransaction;
            rptToolBar.ReportViewer = rptViewer;

            


        }
        else
        {
            DisplayMessage("Record Not Found");
            //rbtnheader.Focus();
            return;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        rbPosted.Checked = true;
        txtFromDate.Focus();
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddlTransaction.SelectedIndex = 0;
        ddlTaxCategory.SelectedIndex = 0;
    }
    protected void FillTaxCategory()
    {
        string CategoryQuery = "Select Trans_Id as Id, Category_Name as Name from Sys_TaxCategoryMaster where IsActive = 'true'";
        DataTable dt = objDa.return_DataTable(CategoryQuery);

        if (dt.Rows.Count > 0)
        {            
            ddlTaxCategory.DataTextField = "Name";
            ddlTaxCategory.DataValueField = "Id";
            ddlTaxCategory.DataSource = dt;
            ddlTaxCategory.DataBind();
        }
        //ddlTaxType.Items.Insert(0, new ListItem("--Select--", "0"));
    }
}