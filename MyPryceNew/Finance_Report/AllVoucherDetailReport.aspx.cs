using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using DevExpress.XtraReports.UI;
using PegasusDataAccess;
using System.Configuration;
using DevExpress.Web;
using System.Collections.Generic;

public partial class Finance_Report_AllVoucherDetailReport : System.Web.UI.Page
{
    XtraReport objRepxReport = null;


    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    SystemParameter ObjSysParam = null;
    Ems_GroupMaster ObjGroupMaster = null;
    Inv_ProductBrandMaster ObjProductBrandMaster = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    DataAccessClass da = null;
    Inv_ProductMaster ObjProductMaster = null;

    Common cmn = null;

    LocationMaster objLocation = null;
    PurchaseRequestHeader objPRequestHeader = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_SalesInquiryHeader objSInquiryHeader = null;
    Inv_SalesQuotationHeader objSQuotationHeader = null;
    Inv_PurchaseInquiry_Supplier ObjPISupplier = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_SalesInvoiceHeader objsInvoiceHeader = null;
    MerchantMaster objMerchantMaster = null;
    EmployeeMaster objEmployee = null;
    HR_EmployeeDetail HR_EmployeeDetail = null;
    string StrUserId = string.Empty;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objRepxReport = new XtraReport();
        //objRepxReport.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "LocationWiseItemSales.repx");

        StrCompId = Session["CompId"].ToString();
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());

        ObjGroupMaster = new Ems_GroupMaster(Session["DBConnection"].ToString());
        ObjProductBrandMaster = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        cmn = new Common(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objPRequestHeader = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objSInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        objSQuotationHeader = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        ObjPISupplier = new Inv_PurchaseInquiry_Supplier(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        objsInvoiceHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objMerchantMaster = new MerchantMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());

        StrUserId = Session["UserId"].ToString();
        if (!IsPostBack)
        {
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["DtFilter"] = null;
            Session["gvExportData"] = null;

            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            FillLocation();
            //ddlLocation.SelectedValue = Session["locId"].ToString();
            txtFromDate.Focus();
            Div1.Attributes.Add("Class", "box box-primary");
            ddlReportType_SelectedIndexChanged(null, null);
        }


        if (Session["gvExportData"] != null)
        {
            try
            {
                gvExportData.DataSource = Session["gvExportData"] as DataTable;
                gvExportData.DataBind();
            }
            catch
            {

            }
        }
    }


    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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

    protected void btnReset_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");


        txtFromDate.Focus();

        txtFromDate.Text = "";
        txtToDate.Text = "";

        ListItem LiDate = new ListItem();
        LiDate.Text = "Invoice Date";
        LiDate.Value = "0";
        ListItem LiCustomerName = new ListItem();
        LiCustomerName.Text = "Customer Name";
        LiCustomerName.Value = "1";
        ListItem LiSalesPerson = new ListItem();
        LiSalesPerson.Text = "Sales Person";
        LiSalesPerson.Value = "2";

        gvExportData.DataSource = null;
        gvExportData.DataBind();
        Session["gvExportData"] = null;

    }

    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objAcParamMaster.GetSupplierAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_id"].ToString();
            }
        }
        return filterlist;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtCon = objAcParamMaster.GetCustomerAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText, contextKey == "True" ? true : false))
        {
            //dtCon = objAcParamMaster.GetCustomerAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_id"].ToString();
                }
            }
            return filterlist;
        }
    }

    #endregion
    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (Session["gvExportData"] != null)
        {
            if (ddlReportType.SelectedValue == "SPWSR")
            {
                ExportTableData(Session["gvExportData"] as DataTable, "SummaryProductWiseSalesReport");
            }

            if (ddlReportType.SelectedValue == "SCWSR")
            {
                ExportTableData(Session["gvExportData"] as DataTable, "SummaryCategoryWiseSalesReport");
            }
            if (ddlReportType.SelectedValue == "SDSR")
            {
                ExportTableData(Session["gvExportData"] as DataTable, "SummaryDailySalesReport");
            }
            if (ddlReportType.SelectedValue == "SSOADBC")
            {
                ExportTableData(Session["gvExportData"] as DataTable, "SummarySalesOrderAndDeleviryByCustomer");
            }
            if (ddlReportType.SelectedValue == "SSRBE")
            {
                ExportTableData(Session["gvExportData"] as DataTable, "SummarySalesReportByEmployee");
            }
            if (ddlReportType.SelectedValue == "SRBB")
            {
                ExportTableData(Session["gvExportData"] as DataTable, "SummaryReportByBrand");
            }
        }
    }

    protected void btnFillgrid_Click(object sender, EventArgs e)
    {
        //lblTotalProfit.Text = "";
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        Session["gvExportData"] = null;
        gvExportData.Columns.Clear();
        gvExportData.DataSource = null;
        gvExportData.ClearSort();
        gvExportData.DataBind();

        if (txtFromDate.Text == "")
        {
            DisplayMessage("Please Fill From Date");
            return;
        }
        if (txtToDate.Text == "")
        {
            DisplayMessage("Please Fill To Date");
            return;
        }

        //for Selected Location
        string strLocationId = string.Empty;
        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
        {
            if (strLocationId == "")
            {
                strLocationId = lstLocationSelect.Items[i].Value;
            }
            else
            {
                strLocationId = strLocationId + "','" + lstLocationSelect.Items[i].Value;
            }
        }

        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }

        string strHandledEmployee = string.Empty;
        if (txtHandledEmployee.Text != "")
        {
            if (GetEmployeeId(txtHandledEmployee.Text) == "")
            {
                strHandledEmployee = "0";
            }
            else
            {
                strHandledEmployee = GetEmployeeId(txtHandledEmployee.Text);
            }
        }
        else
        {
            strHandledEmployee = "0";
        }

        DataTable dt = new DataTable();
        if (ddlReportType.SelectedValue == "JV" || ddlReportType.SelectedValue == "PV" || ddlReportType.SelectedValue == "SPV")
        {
            if (txtSupplierName.Text != "")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select LM.Location_Name,AH.Voucher_Type, AH.Voucher_No, AH.Voucher_Date, AD.Debit_Amount as VoucherAmount, (select EC.Name from Ac_AccountMaster as AM inner join Ems_ContactMaster as EC on EC.Trans_Id=AM.Ref_Id where AM.Trans_Id = AD.Other_Account_No) as SupplierName, AH.Narration from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AD.Debit_Amount != '0' and AH.Location_Id in ('" + strLocationId + "') and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "' and AD.Other_Account_No='" + txtSupplierName.Text.Split('/')[1].ToString() + "'");
            }
            else if (txtCustomerName.Text != "" && txtHandledEmployee.Text != "" && ddlReportType.SelectedValue == "JV")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select distinct LM.Location_Name,AH.Voucher_Type, SEM.Emp_Name as [Sales Person] ,  AH.Voucher_No, AH.Voucher_Date, AD.Credit_Amount as VoucherAmount, EM.Name as CustomerName, AH.Narration from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id inner join Ac_AccountMaster as AM on AM.Trans_Id = AD.Other_Account_No inner join Inv_SalesInvoiceHeader as SI on SI.Supplier_Id = AM.Ref_Id inner join Ems_ContactMaster as EM on EM.Trans_Id = Am.Ref_Id left join Set_EmployeeMaster as SEM on SEM.Emp_Id = CAST(SI.SalesPerson_Id as int) where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AH.Location_Id in ('" + strLocationId + "') and AD.Credit_Amount != '0' and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "' and SI.SalesPerson_Id = '" + strHandledEmployee + "' and AD.Other_Account_No = '" + txtCustomerName.Text.Split('/')[1].ToString() + "'");
            }
            else if (txtHandledEmployee.Text != "" && ddlReportType.SelectedValue == "JV")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select distinct LM.Location_Name,AH.Voucher_Type, SEM.Emp_Name as [Sales Person] ,  AH.Voucher_No, AH.Voucher_Date, AD.Credit_Amount as VoucherAmount, EM.Name as CustomerName, AH.Narration from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id inner join Ac_AccountMaster as AM on AM.Trans_Id = AD.Other_Account_No inner join Inv_SalesInvoiceHeader as SI on SI.Supplier_Id = AM.Ref_Id inner join Ems_ContactMaster as EM on EM.Trans_Id = Am.Ref_Id left join Set_EmployeeMaster as SEM on SEM.Emp_Id = CAST(SI.SalesPerson_Id as int) where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AH.Location_Id in ('" + strLocationId + "') and AD.Credit_Amount != '0' and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "' and SI.SalesPerson_Id = '" + strHandledEmployee + "'");
            }
            else if (txtCustomerName.Text != "" && ddlReportType.SelectedValue == "JV")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select distinct LM.Location_Name,AH.Voucher_Type, SEM.Emp_Name as [Sales Person] ,  AH.Voucher_No, AH.Voucher_Date, AD.Credit_Amount as VoucherAmount, EM.Name as CustomerName, AH.Narration from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id inner join Ac_AccountMaster as AM on AM.Trans_Id = AD.Other_Account_No inner join Inv_SalesInvoiceHeader as SI on SI.Supplier_Id = AM.Ref_Id inner join Ems_ContactMaster as EM on EM.Trans_Id = Am.Ref_Id left join Set_EmployeeMaster as SEM on SEM.Emp_Id = CAST(SI.SalesPerson_Id as int) where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AH.Location_Id in ('" + strLocationId + "') and AD.Credit_Amount != '0' and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "' and AD.Other_Account_No = '" + txtCustomerName.Text.Split('/')[1].ToString() + "'");
            }
            else if (txtHandledEmployee.Text != "" && ddlReportType.SelectedValue == "JV")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select distinct LM.Location_Name,AH.Voucher_Type, SEM.Emp_Name as [Sales Person] ,  AH.Voucher_No, AH.Voucher_Date, AD.Credit_Amount as VoucherAmount, EM.Name as CustomerName, AH.Narration from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id inner join Ac_AccountMaster as AM on AM.Trans_Id = AD.Other_Account_No inner join Inv_SalesInvoiceHeader as SI on SI.Supplier_Id = AM.Ref_Id inner join Ems_ContactMaster as EM on EM.Trans_Id = Am.Ref_Id left join Set_EmployeeMaster as SEM on SEM.Emp_Id = CAST(SI.SalesPerson_Id as int) where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AH.Location_Id in ('" + strLocationId + "') and AD.Credit_Amount != '0' and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "' and SI.SalesPerson_Id = '" + strHandledEmployee + "'");
            }
            else
            {
                dt = ObjProductMaster.daClass.return_DataTable("select LM.Location_Name,AH.Voucher_Type, AH.Voucher_No, AH.Voucher_Date, AD.Debit_Amount as VoucherAmount, (select EC.Name from Ac_AccountMaster as AM inner join Ems_ContactMaster as EC on EC.Trans_Id=AM.Ref_Id where AM.Trans_Id = AD.Other_Account_No) as SupplierName , AH.Narration  from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AD.Debit_Amount != '0' and AH.Location_Id in ('" + strLocationId + "') and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "'");
            }
        }

        if (ddlReportType.SelectedValue == "RV" || ddlReportType.SelectedValue == "CCN" || ddlReportType.SelectedValue == "CRV")
        {
            if (txtCustomerName.Text != "" && txtHandledEmployee.Text != "")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select distinct LM.Location_Name,AH.Voucher_Type, SEM.Emp_Name as [Sales Person] ,  AH.Voucher_No, AH.Voucher_Date, AD.Credit_Amount as VoucherAmount, EM.Name as CustomerName, AH.Narration from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id inner join Ac_AccountMaster as AM on AM.Trans_Id = AD.Other_Account_No inner join Inv_SalesInvoiceHeader as SI on SI.Supplier_Id = AM.Ref_Id inner join Ems_ContactMaster as EM on EM.Trans_Id = Am.Ref_Id left join Set_EmployeeMaster as SEM on SEM.Emp_Id = CAST(SI.SalesPerson_Id as int) where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AH.Location_Id in ('" + strLocationId + "') and AD.Credit_Amount != '0' and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "' and SI.SalesPerson_Id = '" + strHandledEmployee + "' and AD.Other_Account_No = '" + txtCustomerName.Text.Split('/')[1].ToString() + "'");
            }          
            else if (txtHandledEmployee.Text != "" && txtCustomerName.Text == "")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select distinct LM.Location_Name,AH.Voucher_Type, SEM.Emp_Name as [Sales Person] ,  AH.Voucher_No, AH.Voucher_Date, AD.Credit_Amount as VoucherAmount, EM.Name as CustomerName, AH.Narration from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id inner join Ac_AccountMaster as AM on AM.Trans_Id = AD.Other_Account_No inner join Inv_SalesInvoiceHeader as SI on SI.Supplier_Id = AM.Ref_Id inner join Ems_ContactMaster as EM on EM.Trans_Id = Am.Ref_Id left join Set_EmployeeMaster as SEM on SEM.Emp_Id = CAST(SI.SalesPerson_Id as int) where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AH.Location_Id in ('" + strLocationId + "') and AD.Credit_Amount != '0' and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "' and SI.SalesPerson_Id = '" + strHandledEmployee + "'");
            }
            else if (txtCustomerName.Text != "" && txtHandledEmployee.Text == "")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select distinct LM.Location_Name,AH.Voucher_Type, SEM.Emp_Name as [Sales Person] ,  AH.Voucher_No, AH.Voucher_Date, AD.Credit_Amount as VoucherAmount, EM.Name as CustomerName, AH.Narration from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id inner join Ac_AccountMaster as AM on AM.Trans_Id = AD.Other_Account_No inner join Inv_SalesInvoiceHeader as SI on SI.Supplier_Id = AM.Ref_Id inner join Ems_ContactMaster as EM on EM.Trans_Id = Am.Ref_Id left join Set_EmployeeMaster as SEM on SEM.Emp_Id = CAST(SI.SalesPerson_Id as int) where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AH.Location_Id in ('" + strLocationId + "') and AD.Credit_Amount != '0' and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "' and AD.Other_Account_No = '" + txtCustomerName.Text.Split('/')[1].ToString() + "'");
            }
            else if (txtCustomerName.Text == "" && txtHandledEmployee.Text == "")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select distinct LM.Location_Name,AH.Voucher_Type, SEM.Emp_Name as [Sales Person] ,  AH.Voucher_No, AH.Voucher_Date, AD.Credit_Amount as VoucherAmount, EM.Name as CustomerName, AH.Narration from Ac_Voucher_Header as AH inner join Ac_Voucher_Detail as AD on AH.Trans_Id = AD.Voucher_No inner join Set_LocationMaster as LM on LM.Location_Id = AH.Location_Id inner join Ac_AccountMaster as AM on AM.Trans_Id = AD.Other_Account_No inner join Inv_SalesInvoiceHeader as SI on SI.Supplier_Id = AM.Ref_Id inner join Ems_ContactMaster as EM on EM.Trans_Id = Am.Ref_Id left join Set_EmployeeMaster as SEM on SEM.Emp_Id = CAST(SI.SalesPerson_Id as int) where AH.IsActive = 'True' and AH.Voucher_Type = '" + ddlReportType.SelectedValue + "' and AH.Location_Id in ('" + strLocationId + "') and AD.Credit_Amount != '0' and cast(AH.Voucher_Date as date) >= '" + txtFromDate.Text + "' and cast(AH.Voucher_Date as date) <= '" + txtToDate.Text + "'");
            }
        }


        if (dt.Rows.Count > 0)
        {
            Session["gvExportData"] = dt;
            gvExportData.DataSource = dt;
            gvExportData.AutoGenerateColumns = true;
            gvExportData.DataBind();

            try
            {
                lblTotalRecords.Text = "Total Records : " + dt.Rows.Count;
            }
            catch
            {
                lblTotalRecords.Text = "Total Records : " + 0;
            }
        }
        else
        {
            DisplayMessage("You have no record.");
            lblTotalRecords.Text = "Total Records : " + 0;
        }





    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);

        //Inv_ProductMaster ObjProductMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        // DataTable dt = ObjProductMaster.daClass.return_DataTable("");
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }

        return txt;
    }

    protected void txtHandledEmployee_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtHandledEmployee.Text != "")
        {
            strEmployeeId = GetEmployeeId(txtHandledEmployee.Text);
            if (strEmployeeId != "" && strEmployeeId != "0")
            {
                //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDebitAmount);
            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtHandledEmployee.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHandledEmployee);
            }
        }
    }

    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(StrCompId, strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(StrCompId, retval);
                if (dtEmp.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }


    public void ExportTableData(DataTable dtdata, string filename)
    {
        string strFname = filename;
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, strFname);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFname + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    public void FillLocation()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();
        //DataTable dtloc = new DataTable();
        //dtloc = ObjLocation.GetAllLocationMaster();

        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
    }

    //public void fillLocation()
    //{
    //    DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());

    //    try
    //    {
    //        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //    }
    //    catch
    //    {
    //    }
    //    string LocIds = "";
    //    if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
    //    {
    //        LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
    //        if (LocIds != "")
    //        {
    //            dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
    //            if (dtLoc.Rows.Count > 1)
    //            {
    //                ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
    //            }
    //        }
    //        else
    //        {
    //            ddlLocation.Items.Insert(0, new ListItem("All", Session["LocId"].ToString()));
    //        }
    //    }
    //    if (dtLoc.Rows.Count > 0)
    //    {
    //        ddlLocation.DataSource = dtLoc;
    //        ddlLocation.DataTextField = "Location_name";
    //        ddlLocation.DataValueField = "Location_Id";
    //        ddlLocation.DataBind();
    //        //if (dtLoc.Rows.Count > 1 && LocIds != "")
    //        //{
    //        //    ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
    //        //}
    //        ddlLocation.Items.Insert(0, new ListItem("All", "0"));
    //    }
    //    else
    //    {
    //        ddlLocation.Items.Clear();
    //    }

    //    dtLoc = null;
    //}

    protected void btnPushDept_Click(object sender, EventArgs e)
    {
        if (lstLocation.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocationSelect.Items)
            {
                lstLocation.Items.Remove(li);
            }
            lstLocationSelect.SelectedIndex = -1;
        }
        btnPushDept.Focus();
    }
    protected void btnPullDept_Click(object sender, EventArgs e)
    {
        if (lstLocationSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    lstLocation.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Remove(li);
                }
            }
            lstLocation.SelectedIndex = -1;
        }
        btnPullDept.Focus();
    }
    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllDept.Focus();
    }
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //I1.Attributes.Add("Class", "fa fa-minus");
        //Div1.Attributes.Add("Class", "box box-primary");

        if (ddlReportType.SelectedValue == "PV" || ddlReportType.SelectedValue == "SPV")
        {
            divSalesPerson.Visible = false;
            divCustomer.Visible = false;
            divSupplier.Visible = true;
        }
        else if (ddlReportType.SelectedValue == "CCN" || ddlReportType.SelectedValue == "CRV")
        {
            divSalesPerson.Visible = true;
            divCustomer.Visible = true;
            divSupplier.Visible = false;
        }
        else if (ddlReportType.SelectedValue == "JV")
        {
            divSalesPerson.Visible = true;
            divCustomer.Visible = true;
            divSupplier.Visible = false;
        }
        else
        {
            divSalesPerson.Visible = false;
            divCustomer.Visible = false;
            divSupplier.Visible = false;
        }

        txtCustomerName.Text = "";
        txtSupplierName.Text = "";
        txtHandledEmployee.Text = "";

        Session["gvExportData"] = null;
        gvExportData.Columns.Clear();
        gvExportData.DataSource = null;
        gvExportData.ClearSort();
        gvExportData.DataBind();

    }

    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {
        try
        {
            //DataTable dt = new DataTable();

            //ASPxGridViewExporter1.PaperKind = System.Drawing.Printing.PaperKind.A3;
            //ASPxGridViewExporter1.Landscape = true;
            //ASPxGridViewExporter1.WritePdfToResponse();

            ASPxGridViewExporter1.FileName = "ExportedData.pdf"; // Customize filename
            ASPxGridViewExporter1.WritePdfToResponse();
            Response.End();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }

    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedValue == "SICR")
        {
            DataTable dt = new DataTable();
            try
            {
                dt = (DataTable)Session["gvExportData"];
            }
            catch (Exception ex)
            {
                DisplayMessage("No Data Found");
                return;
            }
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ExportTableData(dt, "SalesInvoiceCancelReport");

                }
                else
                {
                    DisplayMessage("No Data Found");
                    return;
                }
            }
            else
            {
                DisplayMessage("No Data Found");
                return;
            }

        }
        if (ddlReportType.SelectedValue == "SIDR")
        {
            DataTable dt = new DataTable();
            try
            {
                dt = (DataTable)Session["gvExportData"];
            }
            catch (Exception ex)
            {
                DisplayMessage("No Data Found");
                return;
            }
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ExportTableData(dt, "SalesInvoiceDetailReport");

                }
                else
                {
                    DisplayMessage("No Data Found");
                    return;
                }
            }
            else
            {
                DisplayMessage("No Data Found");
                return;
            }

        }
        try
        {
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WriteCsvToResponse();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }


    }







}