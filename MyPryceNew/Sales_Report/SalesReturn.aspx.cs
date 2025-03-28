using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sales_Report_SalesReturn : BasePage
{
    Common cmn = null;
    Inv_ProductMaster ObjProductMaster = null;
    LocationMaster objLocation = null;
    SystemParameter ObjSysParam = null;
    PurchaseRequestHeader objPRequestHeader = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_SalesInquiryHeader objSInquiryHeader = null;
    Inv_SalesQuotationHeader objSQuotationHeader = null;
    Inv_PurchaseInquiry_Supplier ObjPISupplier = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_SalesInvoiceHeader objsInvoiceHeader = null;
    PageControlCommon objPageCmn = null;

    EmployeeMaster objEmployee = null;
    HR_EmployeeDetail HR_EmployeeDetail = null;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

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
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        StrUserId = Session["UserId"].ToString();
        if (!IsPostBack)
        {
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["DtFilter"] = null;
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            CalendarExtender3.Format = Session["DateFormat"].ToString();
            txtFromDate.Focus();
            txtFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            FillLocation();
            rbReturnDate.Checked = true;
            rbInvoiceDate_CheckedChanged(null, null);
        }
        AllPageCode();

    }
    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("170", (DataTable)Session["ModuleName"]);
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
    protected void btngo_Click(object sender, EventArgs e)
    {
        string strType = "3";

        if (txtFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text).ToString();
                strType = "2";
            }
            catch (Exception ex)
            {
                DisplayMessage("Enter Valid date");
                return;

            }
        }
        if (txtToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text).ToString();
            }
            catch (Exception ex)
            {
                DisplayMessage("Enter Valid date");
                return;

            }
        }

        if (txtFromDate.Text != "" && txtToDate.Text == "")
        {
            DisplayMessage("Enter to date");
            return;

        }
        if (txtFromDate.Text == "" && txtToDate.Text != "")
        {
            DisplayMessage("Enter from date");
            return;

        }
        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {

            if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            {
                DisplayMessage("To date should be greater then from date");
                return;
            }
        }


        if (txtInvoiceFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtInvoiceFromDate.Text).ToString();
                if (strType == "2")
                {
                    strType = "0";
                }
                else
                {
                    strType = "1";
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("Enter Valid date");
                return;

            }
        }
        if (txtInvoiceToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtInvoiceToDate.Text).ToString();
            }
            catch (Exception ex)
            {
                DisplayMessage("Enter Valid date");
                return;

            }
        }

        if (txtInvoiceFromDate.Text != "" && txtInvoiceToDate.Text == "")
        {
            DisplayMessage("Enter to date");
            return;

        }
        if (txtInvoiceFromDate.Text == "" && txtInvoiceToDate.Text != "")
        {
            DisplayMessage("Enter from date");
            return;

        }
        if (txtInvoiceFromDate.Text != "" && txtInvoiceToDate.Text != "")
        {

            if (Convert.ToDateTime(txtInvoiceFromDate.Text) > Convert.ToDateTime(txtInvoiceToDate.Text))
            {
                DisplayMessage("To date should be greater then from date");
                return;
            }
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
                strLocationId = strLocationId + "," + lstLocationSelect.Items[i].Value;
            }
        }



        if (strLocationId == "")
        {
            strLocationId = Session["LocId"].ToString();
        }
        


        Session["Parameter"] = null;
        Session["DtFilter"] = null;


        DataTable DtFilter = new DataTable();
        //SalesDataSet ObjSalesDataset = new SalesDataSet();
        //ObjSalesDataset.EnforceConstraints = false;



        //SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter();

        //adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        //adp.Fill(ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
        //   DtFilter = ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report;

        if (strType == "3")
        {
            if (txtProductName.Text != "")
            {
                DtFilter = objsInvoiceHeader.GetReturnReportData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", "1990-01-01", "1990-01-01", "1990-01-01", "1990-01-01", "2");
            }
            else
            {
                DtFilter = objsInvoiceHeader.GetReturnReportData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", "1990-01-01", "1990-01-01", "1990-01-01", "1990-01-01", "1");
            }
        }
        else if (strType == "2")
        {
            if (txtProductName.Text != "")
            {
                DtFilter = objsInvoiceHeader.GetReturnReportData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", "1990-01-01", "1990-01-01", Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd"), "2");
            }
            else
            {
                DtFilter = objsInvoiceHeader.GetReturnReportData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", "1990-01-01", "1990-01-01", Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd"), "1");
            }
        }
        else if (strType == "1")
        {
            if (txtProductName.Text != "")
            {
                DtFilter = objsInvoiceHeader.GetReturnReportData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", Convert.ToDateTime(txtInvoiceFromDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtInvoiceToDate.Text).ToString("yyyy-MM-dd"), "1990-01-01", "1990-01-01", "2");
            }
            else
            {
                DtFilter = objsInvoiceHeader.GetReturnReportData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", Convert.ToDateTime(txtInvoiceFromDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtInvoiceToDate.Text).ToString("yyyy-MM-dd"), "1990-01-01", "1990-01-01", "1");
            }
        }
        else if (strType == "0")
        {
            if (txtProductName.Text != "")
            {
                DtFilter = objsInvoiceHeader.GetReturnReportData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", Convert.ToDateTime(txtInvoiceFromDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtInvoiceToDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd"), "2");
            }
            else
            {
                DtFilter = objsInvoiceHeader.GetReturnReportData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "0", Convert.ToDateTime(txtInvoiceFromDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtInvoiceToDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd"), Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd"), "1");
            }
        }

        if (txtInvoiceFromDate.Text != "" && txtInvoiceToDate.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtInvoiceToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

            DateTime FromDate = Convert.ToDateTime(txtInvoiceFromDate.Text);
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 23, 59, 1);

            try
            {
                DtFilter = new DataView(DtFilter, "date2>='" + FromDate.ToString("dd-MMM-yyyy") + "' and date2<='" + ToDate.ToString("dd-MMM-yyyy") + "' and Invoice_Date<>'0' ", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception ex)
            {

            }

            Session["Parameter"] = "FilterBy:From " + txtInvoiceFromDate.Text + "  To " + txtInvoiceToDate.Text;
        }

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
            try
            {
                DtFilter = new DataView(DtFilter, "ReturnDate>='" + txtFromDate.Text + "' and ReturnDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception ex)
            {

            }
            Session["Parameter"] = "FilterBy:From " + txtFromDate.Text + "  To " + txtToDate.Text;
        }

        if (ddlInvoiceType.SelectedIndex > 0)
        {

            try
            {
                DtFilter = new DataView(DtFilter, "SIFromTransType ='" + ddlInvoiceType.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception ex)
            {
            }
            if (Session["Parameter"] == null)
            {
                if (ddlInvoiceType.SelectedIndex == 1)
                {
                    Session["Parameter"] = "FilterBy:DirectWise";
                }
                else
                {
                    Session["Parameter"] = "FilterBy:InDirectWise";

                }
            }
            else
            {
                if (ddlInvoiceType.SelectedIndex == 1)
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , DirectWise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , InDirectWise";
                }
            }
        }
        if (txtInvoiceNo.Text != "")
        {

            try
            {
                DtFilter = new DataView(DtFilter, "Invoice_No='" + txtInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception ex)
            {

            }
            if (Session["Parameter"] == null)
            {
                Session["Parameter"] = "FilterBy: InvoiceNo. Wise";
            }
            else
            {
                Session["Parameter"] = Session["Parameter"].ToString() + " , InvoiceNo. Wise";
            }

        }


        if (txtOrderNo.Text != "")
        {

            DataTable Dt = new DataTable();
            DataTable DtFinal = new DataTable();
            try
            {
                DtFilter = new DataView(DtFilter, "SIFromTransType='S' and ProductRefNo='" + txtOrderNo.Text + "' ", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception ex)
            {
            }



            if (Session["Parameter"] == null)
            {
                Session["Parameter"] = "FilterBy: OrderNo.Wise";
            }
            else
            {
                Session["Parameter"] = Session["Parameter"].ToString() + " , OrderNo.Wise";
            }

        }

        if (txtCustomerName.Text != "")
        {

            try
            {
                DtFilter = new DataView(DtFilter, "Customer_Id =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception ex)
            {

            }
            if (Session["Parameter"] == null)
            {
                Session["Parameter"] = "FilterBy: CustomerWise";
            }
            else
            {
                Session["Parameter"] = Session["Parameter"].ToString() + " , CustomerWise";
            }

        }

        if (txtSalesPerson.Text.Trim() != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "SalesPersonName='" + txtSalesPerson.Text.Trim().Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception ex)
            {

            }
        }


        if (txtProductName.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception ex)
            {
            }

            if (Session["Parameter"] == null)
            {
                Session["Parameter"] = "FilterBy: ProductWise";
            }
            else
            {
                Session["Parameter"] = Session["Parameter"].ToString() + " , ProductWise";
            }
        }

        if (DtFilter.Rows.Count > 0)
        {
            //try
            //{
            //    DtFilter = new DataView(DtFilter, "ReturnQty>0", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //catch (Exception ex)
            //{

            //}

            Session["DtFilter"] = DtFilter;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SalesReturn_Report.aspx?IsGroup=" + chkGroupBy.Checked.ToString() + "','window','width=1024');", true);


            // Response.Redirect("../Sales_Report/SReturnDetailReport.aspx");
        }
        else
        {
            DisplayMessage("Record Not Found");

            return;
        }
    }

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

    protected void btnReset_Click(object sender, EventArgs e)
    {

        txtFromDate.Focus();

        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductName.Text = "";

        txtCustomerName.Text = "";
        txtOrderNo.Text = "";
        ddlInvoiceType.SelectedIndex = 0;




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
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomerName.Text != "")
        {
            DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerName.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnCustomerId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("select Customer in suggestion only");
                txtCustomerName.Text = "";
                txtCustomerName.Focus();
                return;
            }


        }


    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
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
                dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
    public static string[] GetCompletionListOrderNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesOrderHeader objSOrderHeader = new Inv_SalesOrderHeader(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objSOrderHeader.GetSOHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["SalesOrderNo"].ToString();
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
                dt = objSOrderHeader.GetSOHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["SalesOrderNo"].ToString();
                    }
                }
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInvoiceNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesInvoiceHeader objSinvoiceHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objSinvoiceHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Invoice_No"].ToString();
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
                dt = objSinvoiceHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Invoice_No"].ToString();
                    }
                }
            }
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["CompId"].ToString());


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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        //dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "";
            }
        }

        return txt;
    }

    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(Session["CompId"].ToString(), strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), retval);
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


    protected void txtSalesPerson_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtSalesPerson.Text != "")
        {
            strEmployeeId = HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString());
            if (strEmployeeId != "" && strEmployeeId != "0")
            {

            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtSalesPerson.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesPerson);
            }
        }
        AllPageCode();
    }


    protected void txtOrderNo_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = objSOrderHeader.GetSOHeaderAllBySalesOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtOrderNo.Text);
        if (Dt.Rows.Count > 0)
        {

        }
        else
        {
            DisplayMessage("Select Order No. in suggestion Only");
            txtOrderNo.Text = "";
            txtOrderNo.Focus();

            return;
        }
    }
    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = objsInvoiceHeader.GetSInvHeaderAllByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInvoiceNo.Text);
        if (Dt.Rows.Count > 0)
        {

        }
        else
        {
            DisplayMessage("Select Invoice No. in suggestion Only");
            txtInvoiceNo.Text = "";
            txtInvoiceNo.Focus();

            return;
        }
    }


    protected void rbInvoiceDate_CheckedChanged(object sender, EventArgs e)
    {
        if (rbInvoiceDate.Checked == true)
        {
            InvoiceFromDate.Visible = true;
            InvoiceToDate.Visible = true;
            Returndate.Visible = false;
            txtInvoiceFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtInvoiceToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtFromDate.Text = "";
            txtToDate.Text = "";
        }
        else if (rbReturnDate.Checked == true)
        {
            InvoiceFromDate.Visible = false;
            InvoiceToDate.Visible = false;
            Returndate.Visible = true;
            txtFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtInvoiceFromDate.Text = "";
            txtInvoiceToDate.Text = "";
        }
        else
        {
            InvoiceFromDate.Visible = false;
            InvoiceToDate.Visible = false;
            Returndate.Visible = false;
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtInvoiceFromDate.Text = "";
            txtInvoiceToDate.Text = "";
        }
    }
}
