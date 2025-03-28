using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sales_Report_SalesOrder : BasePage
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
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());

        StrUserId = Session["UserId"].ToString();
        if (!IsPostBack)
        {
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["DtFilter"] = null;
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtFromDate.Focus();
            txtFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

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


    protected void rbtnheader_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnheader.Checked == true)
        {
            txtFromDate.Focus();
            txtProductName.Visible = false;
            lblProductName.Visible = false;
            //lblcolon.Visible = false;



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
        Session["Parameter"] = null;
        Session["DtFilter"] = null;
        if (rbtnheader.Checked == false && RbtnDetail.Checked == false)
        {
            DisplayMessage("Select the Report Type(Header or Detail)");
            rbtnheader.Focus();
            return;
        }

        DataTable DtFilter = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;

        if (rbtnheader.Checked == true)
        {
            Session["ReportType"] = "0";

            SalesDataSetTableAdapters.sp_Inv_SalesOrderHeader_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesOrderHeader_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjSalesDataset.sp_Inv_SalesOrderHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjSalesDataset.sp_Inv_SalesOrderHeader_SelectRow_Report;


            Session["ReportHeader"] = "Sales Order Header Report By Order Date";

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "SalesOrderDate>='" + txtFromDate.Text + "' and SalesOrderDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "FilterBy:From " + txtFromDate.Text + "  To " + txtToDate.Text;


            }
            if (ddlOrderType.SelectedIndex > 0)
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SOfromTransType ='" + ddlOrderType.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    if (ddlOrderType.SelectedIndex == 1)
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
                    if (ddlOrderType.SelectedIndex == 1)
                    {
                        Session["Parameter"] = Session["Parameter"].ToString() + " , DirectWise";

                    }
                    else
                    {
                        Session["Parameter"] = Session["Parameter"].ToString() + " , InDirectWise";

                    }


                }

            }


            //for sales perosn filter

            if (txtSalesPerson.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "SalespersonID='" + HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString()) + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: Sales Person - " + txtSalesPerson.Text.Split('/')[0].ToString();
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , Sales Person - " + txtSalesPerson.Text.Split('/')[0].ToString();

                }
            }

            if (txtOrderNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SalesOrderNo='" + txtOrderNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: SalesOrderNo. Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , SalesOrderNo. Wise";
                }

            }


            if (txtQuotationNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SOfromTransType='Q' and  RefNo ='" + txtQuotationNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: QuotationNo.Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , QuotationNo.Wise";
                }
                Session["ReportType"] = "1";

            }

            if (txtCustomerName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "CustomerId =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
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
                Session["ReportType"] = "2";

            }









            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SOrderHeaderReport.aspx','window','width=1024');", true);

                //Response.Redirect("../Sales_Report/SOrderHeaderReport.aspx");
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

            Session["ReportType"] = "1";
            SalesDataSetTableAdapters.sp_Inv_SalesOrderDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesOrderDetail_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjSalesDataset.sp_Inv_SalesOrderDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjSalesDataset.sp_Inv_SalesOrderDetail_SelectRow_Report;

            Session["ReportHeader"] = "Sales Order Detail Report By Order Date";


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "SalesOrderDate>='" + txtFromDate.Text + "' and SalesOrderDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "FilterBy:From " + txtFromDate.Text + "  To " + txtToDate.Text;


            }
            if (ddlOrderType.SelectedIndex > 0)
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SOfromTransType ='" + ddlOrderType.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    if (ddlOrderType.SelectedIndex == 1)
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
                    if (ddlOrderType.SelectedIndex == 1)
                    {
                        Session["Parameter"] = Session["Parameter"].ToString() + " , DirectWise";

                    }
                    else
                    {
                        Session["Parameter"] = Session["Parameter"].ToString() + " , InDirectWise";

                    }


                }

            }
            if (txtOrderNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SalesOrderNo='" + txtOrderNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: SalesOrderNo. Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , SalesOrderNo. Wise";
                }

            }


            if (txtQuotationNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SOfromTransType='Q' and  RefNo ='" + txtQuotationNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: QuotationNo.Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , QuotationNo.Wise";
                }

            }

            if (txtCustomerName.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "CustomerId =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
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




            if (txtProductName.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
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



                Session["DtFilter"] = DtFilter;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SOrderDetailReport.aspx','window','width=1024');", true);


                //Response.Redirect("../Sales_Report/SOrderDetailReport.aspx");
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
        txtQuotationNo.Text = "";
        txtCustomerName.Text = "";
        txtOrderNo.Text = "";
        ddlOrderType.SelectedIndex = 0;




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
    public static string[] GetCompletionListQuotationNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesQuotationHeader ObjSquotationHeader = new Inv_SalesQuotationHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjSquotationHeader.GetQuotationHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["SQuotation_No"].ToString();
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
                dt = ObjSquotationHeader.GetQuotationHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["SQuotation_No"].ToString();
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





    protected void txtQuotationNo_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = objSQuotationHeader.GetQuotationHeaderAllBySQuotationNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtQuotationNo.Text);
        if (Dt.Rows.Count > 0)
        {
        }
        else
        {
            DisplayMessage("Select Quotation No. in suggestion Only");
            txtQuotationNo.Text = "";
            txtQuotationNo.Focus();

            return;
        }
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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

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

}
