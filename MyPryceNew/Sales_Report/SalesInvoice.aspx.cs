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

public partial class Sales_Report_SalesInvoice : BasePage
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


    Common cmn = null;
    Inv_ProductMaster ObjProductMaster = null;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        //objRepxReport = new XtraReport();
        //objRepxReport.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "LocationWiseItemSales.repx");


        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjGroupMaster = new Ems_GroupMaster(Session["DBConnection"].ToString());
        ObjProductBrandMaster = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());



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
            fillLocation();
            ddlLocation.SelectedValue = Session["locId"].ToString();
            txtFromDate.Focus();
            FillMerchant();
            rbtnheader_CheckedChanged(null, null);

        }

        if (rbtnSummary.Checked)
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
    public void FillMerchant()
    {
        new PageControlCommon(Session["DBConnection"].ToString()).FillData((object)ddlMerchant, objMerchantMaster.GetMerchantMasterTrueAll(), "Merchant_Name", "Trans_Id");
    }



    protected void rbtnheader_CheckedChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        divReports.Visible = true;
        trProduct.Visible = true;
        divGrid.Visible = false;

        btngo.Visible = true;
        btnFillgrid.Visible = false;
        BtnExportExcel.Visible = false;
        BtnExportPDF.Visible = false;


        txtFromDate.Focus();
        txtProductName.Visible = false;
        lblProductName.Visible = false;
        ddlMerchant.SelectedIndex = 0;
        ddlGroupBy.Items.Clear();

        if (rbtnheader.Checked)
        {
            if (rbtnheaderDailySales.Checked)
            {
                ListItem LiDate = new ListItem();
                LiDate.Text = "Invoice Date";
                LiDate.Value = "0";
                ListItem LiCustomerName = new ListItem();
                LiCustomerName.Text = "Customer Name";
                LiCustomerName.Value = "1";
                ListItem LiSalesPerson = new ListItem();
                LiSalesPerson.Text = "Sales Person";
                LiSalesPerson.Value = "2";
                ddlGroupBy.Items.Add(LiDate);
                ddlGroupBy.Items.Add(LiCustomerName);
                ddlGroupBy.Items.Add(LiSalesPerson);
                Location.Visible = false;
                ReportType.Visible = false;
                InvoiceType.Visible = false;
                InvoiceNo.Visible = false;
                OrderNo.Visible = false;
                GroupBy.Visible = false;
                Merchent.Visible = false;
                OrderBy.Visible = false;
                State.Visible = false;
                Group.Visible = false;
            }
            else
            {
                ListItem LiDate = new ListItem();
                LiDate.Text = "Invoice Date";
                LiDate.Value = "0";
                ListItem LiCustomerName = new ListItem();
                LiCustomerName.Text = "Customer Name";
                LiCustomerName.Value = "1";
                ListItem LiSalesPerson = new ListItem();
                LiSalesPerson.Text = "Sales Person";
                LiSalesPerson.Value = "2";
                ddlGroupBy.Items.Add(LiDate);
                ddlGroupBy.Items.Add(LiCustomerName);
                ddlGroupBy.Items.Add(LiSalesPerson);
                Location.Visible = true;
                ReportType.Visible = true;
                InvoiceType.Visible = true;
                InvoiceNo.Visible = true;
                OrderNo.Visible = true;
                GroupBy.Visible = true;
                Merchent.Visible = true;
                OrderBy.Visible = true;
                State.Visible = true;
                Group.Visible = true;
            }

        }
        else
        {
            if (rbtnSummary.Checked)
            {
                divGrid.Visible = true;
                divReports.Visible = false;
                trProduct.Visible = false;
                btngo.Visible = false;
                btnFillgrid.Visible = true;
                BtnExportExcel.Visible = true;
                BtnExportPDF.Visible = true;
                Location.Visible = true;
                ReportType.Visible = true;
                InvoiceType.Visible = true;
                InvoiceNo.Visible = true;
                OrderNo.Visible = true;
                GroupBy.Visible = true;
                Merchent.Visible = true;
                OrderBy.Visible = true;
                State.Visible = true;
                Group.Visible = true;
            }
            else
            {
                ListItem liMerchant = new ListItem();
                liMerchant.Text = "Merchant Name";
                liMerchant.Value = "3";
                ddlGroupBy.Items.Add(liMerchant);
                Location.Visible = false;
                ReportType.Visible = false;
                InvoiceType.Visible = false;
                InvoiceNo.Visible = false;
                OrderNo.Visible = false;
                GroupBy.Visible = false;
                Merchent.Visible = true;
                OrderBy.Visible = false;
                State.Visible = false;
                Group.Visible = false;
            }
        }

        Session["gvExportData"] = null;
        gvExportData.Columns.Clear();
        gvExportData.DataSource = null;
        gvExportData.ClearSort();
        gvExportData.DataBind();

    }
    protected void RbtnDetail_CheckedChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (RbtnDetail.Checked == true)
        {
            ddlMerchant.SelectedIndex = 0;
            txtFromDate.Focus();
            txtProductName.Visible = true;
            lblProductName.Visible = true;
            //lblcolon.Visible = true;

            ddlGroupBy.Items.Clear();
            ListItem LiDate = new ListItem();
            LiDate.Text = "Invoice Date";
            LiDate.Value = "0";
            ListItem LiInvoiceNo = new ListItem();
            LiInvoiceNo.Text = "Invoice No.";
            LiInvoiceNo.Value = "1";
            ListItem LiCustomerName = new ListItem();
            LiCustomerName.Text = "Customer Name";
            LiCustomerName.Value = "2";
            ddlGroupBy.Items.Add(LiDate);
            ddlGroupBy.Items.Add(LiInvoiceNo);
            ddlGroupBy.Items.Add(LiCustomerName);
            Location.Visible = true;
            ReportType.Visible = true;
            InvoiceType.Visible = true;
            InvoiceNo.Visible = true;
            OrderNo.Visible = true;
            GroupBy.Visible = true;
            Merchent.Visible = true;
            OrderBy.Visible = true;
            State.Visible = true;
            Group.Visible = true;
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
    protected void btngo_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        Session["Parameter"] = null;
        Session["DtFilter"] = null;
        if (rbtnheader.Checked == false && RbtnDetail.Checked == false && RbtnVatHeader.Checked == false && rbtnheaderDailySales.Checked == false)
        {
            DisplayMessage("Select the Report Type(Header or Detail or Vat)");
            rbtnheader.Focus();
            return;
        }


        DataTable DtFilter = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;

        if (rbtnheader.Checked == true || RbtnVatHeader.Checked == true)
        {

            SalesDataSetTableAdapters.sp_Inv_SalesInvoiceHeader_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInvoiceHeader_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjSalesDataset.sp_Inv_SalesInvoiceHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjSalesDataset.sp_Inv_SalesInvoiceHeader_SelectRow_Report;

            Session["ReportHeader"] = "Sales Invoice Header Report By Invoice Date";
            Session["ReportType"] = "0";

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "Invoice_Date>='" + txtFromDate.Text + "' and Invoice_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
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
                catch
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




            //for sales perosn filter

            if (txtSalesPerson.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "SalesPerson_Id='" + HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString()) + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }



            }


            if (txtInvoiceNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Invoice_No like '" + txtInvoiceNo.Text + "%'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
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
            if (ddlMerchant.SelectedIndex != 0)
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Invoice_Merchant_Id = " + ddlMerchant.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: Merchant Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , Merchant Wise";
                }

            }


            if (txtOrderNo.Text != "")
            {

                DataTable Dt = new DataTable();
                DataTable DtFinal = new DataTable();
                try
                {
                    DtFilter = new DataView(DtFilter, "SIFromTransType='S' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                for (int i = 0; i < DtFilter.Rows.Count; i++)
                {
                    Dt = DtFilter.Copy();
                    string[] PoNO = DtFilter.Rows[i]["RefNo"].ToString().Split(',');

                    for (int j = 0; j < PoNO.Length; j++)
                    {
                        if (PoNO[j].ToString() == txtOrderNo.Text)
                        {
                            Dt = new DataView(Dt, "Trans_Id=" + DtFilter.Rows[i]["Trans_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                            DtFinal.Merge(Dt);
                            break;
                        }
                    }
                }

                DtFilter = DtFinal.Copy();
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
                    DtFilter = new DataView(DtFilter, "Supplier_Id =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
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
                Session["ReportType"] = "1";
                Session["ReportHeader"] = "Sales Invoice Header Report By Customer";
            }
            Session["ReportType"] = ddlGroupBy.SelectedValue;
            if (rbtnheader.Checked)
            {
                if (chkGroupBy.Checked)
                {
                    Session["ReportHeader"] = "Sales Invoice Header Report By " + ddlGroupBy.SelectedItem.Text;
                }
                else
                {
                    Session["ReportHeader"] = "Sales Invoice Report";
                }
            }
            else
            {
                if (txtStateName.Text != "")
                {
                    try
                    {
                        DtFilter = new DataView(DtFilter, "StateName like'%" + txtStateName.Text + "%'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }
                }
                Session["ReportHeader"] = "Vat Report";
            }

            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SInvoiceHeaderReport.aspx?orderby=" + ddlorderby.SelectedValue + "&&IsGroup=" + chkGroupBy.Checked.ToString() + "','window','width=1024');", true);

                // Response.Redirect("../Sales_Report/SInvoiceHeaderReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");
                rbtnheader.Focus();
                return;
            }
        }

        if (rbtnheaderDailySales.Checked)
        {
            SalesDataSetTableAdapters.sp_Inv_DailySales_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_DailySales_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjSalesDataset.sp_Inv_DailySales_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), null, null, 0);
            DtFilter = ObjSalesDataset.sp_Inv_DailySales_Report;

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

                adp.Fill(ObjSalesDataset.sp_Inv_DailySales_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToDateTime(txtFromDate.Text), ToDate, 1);
                DtFilter = ObjSalesDataset.sp_Inv_DailySales_Report;

                Session["Parameter"] = "FilterBy : From " + txtFromDate.Text + "  To " + txtToDate.Text;
            }
            else
            {
                Session["Parameter"] = null;
            }

            if (txtCustomerName.Text != "")
            {
                //string strCustomerName = txtCustomerName.Text.Trim().Split('/')[0].ToString();
                string strCustomerName = txtCustomerName.Text.Split('/')[0].ToString();
                try
                {
                    DtFilter = new DataView(DtFilter, "CustomerName ='" + strCustomerName + "'", "", DataViewRowState.CurrentRows).ToTable();
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
            if (txtSalesPerson.Text != "")
            {
                string strSalesPersonName = txtSalesPerson.Text.Trim().Split('/')[0].ToString();
                DataTable dtEmp = da.return_DataTable("Select Emp_Id From Set_EmployeeMaster where Emp_Name='" + strSalesPersonName + "'");
                if (dtEmp.Rows.Count > 0)
                {
                    string EmpId = dtEmp.Rows[0]["Emp_Id"].ToString();
                    DtFilter = new DataView(DtFilter, "SalesPerson_Id ='" + EmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                }
            }

            if (ddlMerchant.SelectedValue != "--Select--")
            {
                DtFilter = new DataView(DtFilter, "Invoice_Merchant_Id ='" + ddlMerchant.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (rBtnTypePosted.Checked)
            {
                DtFilter = new DataView(DtFilter, "Post ='" + true + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (rBtnTypeUnPosted.Checked)
            {
                DtFilter = new DataView(DtFilter, "Post ='" + false + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            //dtpendingSpv = new DataView(dtpendingSpv, "Field1='SPV' and Field3='Pending' and Other_Account_No='" + txtSupplierName.Text.Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/DailySalesReport.aspx','window','width=1024');", true);

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
            SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report;


            Session["ReportHeader"] = "Sales Invoice Detail Report By Invoice Date";

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "Invoice_Date>='" + txtFromDate.Text + "' and Invoice_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "FilterBy:From " + txtFromDate.Text + "  To " + txtToDate.Text;
            }
            if (ddlInvoiceType.SelectedIndex > 0)
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "RefType ='" + ddlInvoiceType.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
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
                catch
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
            if (ddlMerchant.SelectedIndex != 0)
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Invoice_Merchant_Id = " + ddlMerchant.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: Merchant Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , Merchant Wise";
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
                catch
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

            Session["ReportType"] = ddlGroupBy.SelectedValue;
            Session["ReportHeader"] = "Sales Invoice Detail Report By " + ddlGroupBy.SelectedItem.Text;

            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SInvoiceDetailReport.aspx?orderby=" + ddlorderby.SelectedValue + "','window','width=1024');", true);
                //Response.Redirect("../Sales_Report/SInvoiceDetailReport.aspx");
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
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        rbtnheader.Checked = true;
        RbtnDetail.Checked = false;
        txtFromDate.Focus();
        ddlMerchant.SelectedIndex = 0;
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductName.Text = "";
        txtStateName.Text = "";
        txtCustomerName.Text = "";
        txtOrderNo.Text = "";
        ddlInvoiceType.SelectedIndex = 0;
        ddlGroupBy.Items.Clear();
        ListItem LiDate = new ListItem();
        LiDate.Text = "Invoice Date";
        LiDate.Value = "0";
        ListItem LiCustomerName = new ListItem();
        LiCustomerName.Text = "Customer Name";
        LiCustomerName.Value = "1";
        ListItem LiSalesPerson = new ListItem();
        LiSalesPerson.Text = "Sales Person";
        LiSalesPerson.Value = "2";
        ddlGroupBy.Items.Add(LiDate);
        ddlGroupBy.Items.Add(LiCustomerName);
        ddlGroupBy.Items.Add(LiSalesPerson);
        gvExportData.DataSource = null;
        gvExportData.DataBind();
        Session["gvExportData"] = null;

    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

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
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        TextBox tb = (TextBox)sender;

        if (tb.Text != "")
        {
            string customerId = "";
            customerId = ObjContactMaster.GetContactIdByContactNameNID(tb.Text.Trim().Split('/')[0].ToString(), tb.Text.Trim().Split('/')[1].ToString());
            if (customerId != "")
            {
                hdnCustomerId.Value = customerId;
            }
            else
            {
                DisplayMessage("select Customer in suggestion only");
                tb.Text = "";
                tb.Focus();
                return;
            }
        }
        else
        {
            hdnCustomerId.Value = "";
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
        DataTable dtCon = objcustomer.GetCustomerRecAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["trans_id"].ToString();
            }
        }
        return filterlist;
    }






    protected void txtOrderNo_TextChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

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
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

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


    public DataTable GetSalesInvoiceDetailReport(string strCompanyId, string strBrandId, string strLocationId, string strAccountNo, string strOther_Account_No, string strFromDate, string strToDate, string strCurrency_Type, string strChildCustomers, string strParentCustomerName)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", strLocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Account_No", strAccountNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Other_Account_No", strOther_Account_No, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@From_Date", strFromDate, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@To_Date", strToDate, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Currency_Type", strCurrency_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[8] = new PassDataToSql("@ChildCustomers", strChildCustomers, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ParentCustomer", strParentCustomerName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = da.Reuturn_Datatable_Search("sp_Ac_AllStatements_SelectRow_MultipleCustomer", paramList);
        return dtInfo;
    }

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
        lblTotalProfit.Text = "";
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        Session["gvExportData"] = null;
        gvExportData.Columns.Clear();
        gvExportData.DataSource = null;
        gvExportData.ClearSort();
        gvExportData.DataBind();

        DataTable dt = new DataTable();
        if (ddlReportType.SelectedValue == "SPWSR")
        {
            //dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
            if (ddlLocation.SelectedIndex == 0)
            {

                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true'  and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='false'   and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");

            }
            else
            {
                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoicedetail.Post='false' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
            }
        }
        if (ddlReportType.SelectedValue == "SPWSRC")
        {
            //dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
            if (ddlLocation.SelectedIndex == 0)
            {
                dt = ObjProductMaster.daClass.return_DataTable("SELECT Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1  left   join   Ems_ContactMaster on Ems_ContactMaster.Trans_Id =  Inv_SalesInvoiceHeader.Supplier_Id  where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
            }
            else
            {
                dt = ObjProductMaster.daClass.return_DataTable("SELECT Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1  left   join   Ems_ContactMaster on Ems_ContactMaster.Trans_Id =  Inv_SalesInvoiceHeader.Supplier_Id  where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
            }
        }

        if (ddlReportType.SelectedValue == "PER")
        {
            if (ddlLocation.SelectedIndex == 0)
            {
                if (txtProductName.Text == "")
                {
                    dt = ObjProductMaster.daClass.return_DataTable("select (Select Location_Name from Set_LocationMaster where Location_Id=ab.Location_Id) as Location, (Select EProductName from Inv_ProductMaster where ProductId = ab.ProductId) as ProductName,  ab.TotalIn, ab.TotalOut, (ab.TotalIn - ab.TotalOut) as Balance, ab.BatchNo, ab.ExpiryDate, ab.Barcode from (SELECT SUM(CASE WHEN InOut = 'I' THEN Quantity ELSE 0 END) AS TotalIn, SUM(CASE WHEN InOut = 'O' THEN Quantity ELSE 0 END) AS TotalOut, Company_Id, Brand_Id, Location_Id, ProductId, UnitId, BatchNo, ExpiryDate, Barcode FROM Inv_StockBatchMaster GROUP BY Company_Id, Brand_Id, Location_Id, ProductId, UnitId, BatchNo, ExpiryDate, Barcode) as ab where ab.ExpiryDate >= '" + txtFromDate.Text + "' and ab.ExpiryDate <= '" + txtToDate.Text + "' and (ab.TotalIn - ab.TotalOut) != '0' order by ab.ExpiryDate ASC");
                }
                else
                {
                    dt = ObjProductMaster.daClass.return_DataTable("select (Select Location_Name from Set_LocationMaster where Location_Id=ab.Location_Id) as Location, (Select EProductName from Inv_ProductMaster where ProductId = ab.ProductId) as ProductName,  ab.TotalIn, ab.TotalOut, (ab.TotalIn - ab.TotalOut) as Balance, ab.BatchNo, ab.ExpiryDate, ab.Barcode from (SELECT SUM(CASE WHEN InOut = 'I' THEN Quantity ELSE 0 END) AS TotalIn, SUM(CASE WHEN InOut = 'O' THEN Quantity ELSE 0 END) AS TotalOut, Company_Id, Brand_Id, Location_Id, ProductId, UnitId, BatchNo, ExpiryDate, Barcode FROM Inv_StockBatchMaster where ProductId = '" + hdnProductId.Value + "' GROUP BY Company_Id, Brand_Id, Location_Id, ProductId, UnitId, BatchNo, ExpiryDate, Barcode) as ab where ab.ExpiryDate >= '" + txtFromDate.Text + "' and ab.ExpiryDate <= '" + txtToDate.Text + "' and (ab.TotalIn - ab.TotalOut) != '0' order by ab.ExpiryDate ASC");
                }
            }
            else
            {
                if (txtProductName.Text == "")
                {
                    dt = ObjProductMaster.daClass.return_DataTable("select (Select Location_Name from Set_LocationMaster where Location_Id=ab.Location_Id) as Location, (Select EProductName from Inv_ProductMaster where ProductId = ab.ProductId) as ProductName,  ab.TotalIn, ab.TotalOut, (ab.TotalIn - ab.TotalOut) as Balance, ab.BatchNo, ab.ExpiryDate, ab.Barcode from (SELECT SUM(CASE WHEN InOut = 'I' THEN Quantity ELSE 0 END) AS TotalIn, SUM(CASE WHEN InOut = 'O' THEN Quantity ELSE 0 END) AS TotalOut, Company_Id, Brand_Id, Location_Id, ProductId, UnitId, BatchNo, ExpiryDate, Barcode FROM Inv_StockBatchMaster WHERE Location_Id = '" + ddlLocation.SelectedValue + "' GROUP BY Company_Id, Brand_Id, Location_Id, ProductId, UnitId, BatchNo, ExpiryDate, Barcode) as ab where ab.ExpiryDate >= '" + txtFromDate.Text + "' and ab.ExpiryDate <= '" + txtToDate.Text + "' and (ab.TotalIn - ab.TotalOut) != '0' order by ab.ExpiryDate ASC");
                }
                else
                {
                    dt = ObjProductMaster.daClass.return_DataTable("select (Select Location_Name from Set_LocationMaster where Location_Id=ab.Location_Id) as Location, (Select EProductName from Inv_ProductMaster where ProductId = ab.ProductId) as ProductName,  ab.TotalIn, ab.TotalOut, (ab.TotalIn - ab.TotalOut) as Balance, ab.BatchNo, ab.ExpiryDate, ab.Barcode from (SELECT SUM(CASE WHEN InOut = 'I' THEN Quantity ELSE 0 END) AS TotalIn, SUM(CASE WHEN InOut = 'O' THEN Quantity ELSE 0 END) AS TotalOut, Company_Id, Brand_Id, Location_Id, ProductId, UnitId, BatchNo, ExpiryDate, Barcode FROM Inv_StockBatchMaster WHERE ProductId = '" + hdnProductId.Value + "' AND Location_Id = '" + ddlLocation.SelectedValue + "' GROUP BY Company_Id, Brand_Id, Location_Id, ProductId, UnitId, BatchNo, ExpiryDate, Barcode) as ab where ab.ExpiryDate >= '" + txtFromDate.Text + "' and ab.ExpiryDate <= '" + txtToDate.Text + "' and (ab.TotalIn - ab.TotalOut) != '0' order by ab.ExpiryDate ASC");
                }
            }
        }

        //if (ddlReportType.SelectedValue == "SIDR")
        //{
        //    if (ddlLocation.SelectedIndex == 0)
        //    {
        //        if (rBtnTypeAll.Checked)
        //            dt = ObjProductMaster.daClass.return_DataTable("SELECT (Select Location_Name from Set_LocationMaster where Location_Id=SH.Location_Id) as [Location Name],SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],(Select Top 1 CategoryName from Inv_CustomerCategory as ICC inner join Inv_CustomerCategory_Customer as ICCC on ICCC.Customer_Category_Id = ICC.Trans_Id where ICCC.Customer_Id = SH.Supplier_Id and ICCC.IsActive = 'True' and ICC.IsActive = 'True') as [Customer Category], (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS [Customer Name], (Select Emp_Name from Set_EmployeeMaster where Emp_Id=SH.SalesPerson_Id) as [Sales Person], (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Code], (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Name], SD.Quantity,SH.GrandTotal as [Total Amount], (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id=SH.Trans_Id) AS ReturnNetAmount,       Sd.ReturnQty as [Return Quantity],(Select Unit_Name+'/'+ Unit_Code from Inv_UnitMaster where Unit_Id=SD.Unit_Id) as [Sales Unit],SD.UnitPrice as [Unit Price],SD.Field2 as [Average Cost], cast((SD.Field2/(Select Coversion_Qty from Inv_UnitMaster where Unit_Id=SD.Unit_Id)) as numeric(36,5)) [Average Cost by unit], (( ISNULL(SD.UnitPrice, 0)    *  ISNULL(SD.Quantity, 0) )- ((  ISNULL(SD.UnitPrice, 0)    * ISNULL(SD.Quantity, 0))* ISNULL(SD.DiscountP, 0) /100)) AS [Selling Price],  SH.ReturnNo AS [Return No], SH.Field7 AS [Return Date],cast(round((Sd.ReturnQty * SD.UnitPrice),2) as numeric(36,4)) as [Return Amount]   FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) LEFT JOIN (SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl,  '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\\' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block, Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END) AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1, Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE  cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.IsCancel='False'");
        //        else if (rBtnTypePosted.Checked)
        //            dt = ObjProductMaster.daClass.return_DataTable("SELECT (Select Location_Name from Set_LocationMaster where Location_Id=SH.Location_Id) as [Location Name],SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No], (Select Top 1 CategoryName from Inv_CustomerCategory as ICC inner join Inv_CustomerCategory_Customer as ICCC on ICCC.Customer_Category_Id = ICC.Trans_Id where ICCC.Customer_Id = SH.Supplier_Id and ICCC.IsActive = 'True' and ICC.IsActive = 'True') as [Customer Category], (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS [Customer Name], (Select Emp_Name from Set_EmployeeMaster where Emp_Id=SH.SalesPerson_Id) as [Sales Person], (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Code], (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Name], SD.Quantity,SH.GrandTotal as [Total Amount], (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id=SH.Trans_Id) AS ReturnNetAmount,         Sd.ReturnQty as [Return Quantity], (Select Unit_Name+'/'+ Unit_Code from Inv_UnitMaster where Unit_Id=SD.Unit_Id) as [Sales Unit],SD.UnitPrice as [Unit Price],SD.Field2 as [Average Cost], cast((SD.Field2/(Select Coversion_Qty from Inv_UnitMaster where Unit_Id=SD.Unit_Id)) as numeric(36,5)) [Average Cost by unit], (( ISNULL(SD.UnitPrice, 0)    *  ISNULL(SD.Quantity, 0) )- ((  ISNULL(SD.UnitPrice, 0)    * ISNULL(SD.Quantity, 0))* ISNULL(SD.DiscountP, 0) /100)) AS [Selling Price],  SH.ReturnNo AS [Return No], SH.Field7 AS [Return Date],cast(round((Sd.ReturnQty * SD.UnitPrice),2) as numeric(36,4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) LEFT JOIN (SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl,  '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\\' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block, Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END) AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1, Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE  cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Post=1 AND SH.IsActive = 'True' and SH.IsCancel='False'");
        //        else if (rBtnTypeUnPosted.Checked)
        //            dt = ObjProductMaster.daClass.return_DataTable("SELECT (Select Location_Name from Set_LocationMaster where Location_Id=SH.Location_Id) as [Location Name],SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No], (Select Top 1 CategoryName from Inv_CustomerCategory as ICC inner join Inv_CustomerCategory_Customer as ICCC on ICCC.Customer_Category_Id = ICC.Trans_Id where ICCC.Customer_Id = SH.Supplier_Id and ICCC.IsActive = 'True' and ICC.IsActive = 'True') as [Customer Category], (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS [Customer Name], (Select Emp_Name from Set_EmployeeMaster where Emp_Id=SH.SalesPerson_Id) as [Sales Person], (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Code], (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Name], SD.Quantity,SH.GrandTotal as [Total Amount], (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id=SH.Trans_Id) AS ReturnNetAmount,         Sd.ReturnQty as [Return Quantity],(Select Unit_Name+'/'+ Unit_Code from Inv_UnitMaster where Unit_Id=SD.Unit_Id) as [Sales Unit],SD.UnitPrice as [Unit Price],SD.Field2 as [Average Cost], cast((SD.Field2/(Select Coversion_Qty from Inv_UnitMaster where Unit_Id=SD.Unit_Id)) as numeric(36,5)) [Average Cost by unit], (( ISNULL(SD.UnitPrice, 0)    *  ISNULL(SD.Quantity, 0) )- ((  ISNULL(SD.UnitPrice, 0)    * ISNULL(SD.Quantity, 0))* ISNULL(SD.DiscountP, 0) /100)) AS [Selling Price],  SH.ReturnNo AS [Return No], SH.Field7 AS [Return Date],cast(round((Sd.ReturnQty * SD.UnitPrice),2) as numeric(36,4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) LEFT JOIN (SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl,  '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\\' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block, Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END) AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1, Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE  cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Post=0 AND SH.IsActive = 'True' and SH.IsCancel='False'");
        //    }
        //    else
        //    {
        //        if (rBtnTypeAll.Checked)
        //            dt = ObjProductMaster.daClass.return_DataTable("SELECT (Select Location_Name from Set_LocationMaster where Location_Id=SH.Location_Id) as [Location Name],SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No], (Select Top 1 CategoryName from Inv_CustomerCategory as ICC inner join Inv_CustomerCategory_Customer as ICCC on ICCC.Customer_Category_Id = ICC.Trans_Id where ICCC.Customer_Id = SH.Supplier_Id and ICCC.IsActive = 'True' and ICC.IsActive = 'True') as [Customer Category], (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS [Customer Name], (Select Emp_Name from Set_EmployeeMaster where Emp_Id=SH.SalesPerson_Id) as [Sales Person], (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Code], (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Name], SD.Quantity,SH.GrandTotal as [Total Amount], (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id=SH.Trans_Id) AS ReturnNetAmount,       Sd.ReturnQty as [Return Quantity],(Select Unit_Name+'/'+ Unit_Code from Inv_UnitMaster where Unit_Id=SD.Unit_Id) as [Sales Unit],SD.UnitPrice as [Unit Price],SD.Field2 as [Average Cost], cast((SD.Field2/(Select Coversion_Qty from Inv_UnitMaster where Unit_Id=SD.Unit_Id)) as numeric(36,5)) [Average Cost by unit], (( ISNULL(SD.UnitPrice, 0)    *  ISNULL(SD.Quantity, 0) )- ((  ISNULL(SD.UnitPrice, 0)    * ISNULL(SD.Quantity, 0))* ISNULL(SD.DiscountP, 0) /100)) AS [Selling Price],  SH.ReturnNo AS [Return No], SH.Field7 AS [Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice),2) as numeric(36,4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) LEFT JOIN (SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl,  '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\\' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block, Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END) AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1, Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE  cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.IsActive = 'True' and SH.IsCancel='False'");
        //        else if (rBtnTypePosted.Checked)
        //            dt = ObjProductMaster.daClass.return_DataTable("SELECT (Select Location_Name from Set_LocationMaster where Location_Id=SH.Location_Id) as [Location Name],SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No], (Select Top 1 CategoryName from Inv_CustomerCategory as ICC inner join Inv_CustomerCategory_Customer as ICCC on ICCC.Customer_Category_Id = ICC.Trans_Id where ICCC.Customer_Id = SH.Supplier_Id and ICCC.IsActive = 'True' and ICC.IsActive = 'True') as [Customer Category], (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS [Customer Name], (Select Emp_Name from Set_EmployeeMaster where Emp_Id=SH.SalesPerson_Id) as [Sales Person], (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Code], (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Name], SD.Quantity,SH.GrandTotal as [Total Amount],(Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id=SH.Trans_Id) AS ReturnNetAmount,          Sd.ReturnQty as [Return Quantity], (Select Unit_Name+'/'+ Unit_Code from Inv_UnitMaster where Unit_Id=SD.Unit_Id) as [Sales Unit],SD.UnitPrice as [Unit Price],SD.Field2 as [Average Cost], cast((SD.Field2/(Select Coversion_Qty from Inv_UnitMaster where Unit_Id=SD.Unit_Id)) as numeric(36,5)) [Average Cost by unit], (( ISNULL(SD.UnitPrice, 0)    *  ISNULL(SD.Quantity, 0) )- ((  ISNULL(SD.UnitPrice, 0)    * ISNULL(SD.Quantity, 0))* ISNULL(SD.DiscountP, 0) /100)) AS [Selling Price],  SH.ReturnNo AS [Return No], SH.Field7 AS [Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice),2) as numeric(36,4)) as [Return Amount]   FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) LEFT JOIN (SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl,  '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\\' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block, Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END) AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1, Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE  cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Post=1 and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.IsActive = 'True' and SH.IsCancel='False'");
        //        else if (rBtnTypeUnPosted.Checked)
        //            dt = ObjProductMaster.daClass.return_DataTable("SELECT (Select Location_Name from Set_LocationMaster where Location_Id=SH.Location_Id) as [Location Name],SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No], (Select Top 1 CategoryName from Inv_CustomerCategory as ICC inner join Inv_CustomerCategory_Customer as ICCC on ICCC.Customer_Category_Id = ICC.Trans_Id where ICCC.Customer_Id = SH.Supplier_Id and ICCC.IsActive = 'True' and ICC.IsActive = 'True') as [Customer Category], (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS [Customer Name], (Select Emp_Name from Set_EmployeeMaster where Emp_Id=SH.SalesPerson_Id) as [Sales Person], (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Code], (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS [Product Name], SD.Quantity,SH.GrandTotal as [Total Amount],  (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id=SH.Trans_Id) AS ReturnNetAmount, Sd.ReturnQty as [Return Quantity], (Select Unit_Name+'/'+ Unit_Code from Inv_UnitMaster where Unit_Id=SD.Unit_Id) as [Sales Unit],SD.UnitPrice as [Unit Price],SD.Field2 as [Average Cost], cast((SD.Field2/(Select Coversion_Qty from Inv_UnitMaster where Unit_Id=SD.Unit_Id)) as numeric(36,5)) [Average Cost by unit],     (( ISNULL(SD.UnitPrice, 0)    *  ISNULL(SD.Quantity, 0) )- ((  ISNULL(SD.UnitPrice, 0)    * ISNULL(SD.Quantity, 0))* ISNULL(SD.DiscountP, 0) /100)) AS [Selling Price],  SH.ReturnNo AS [Return No], SH.Field7 AS [Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice),2) as numeric(36,4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) LEFT JOIN (SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl,  '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\\' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block, Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END) AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1, Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE  cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Post=0 and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.IsActive = 'True' and SH.IsCancel='False'");
        //    }
        //}


        if (ddlReportType.SelectedValue == "SIDR")
        {
            if (ddlLocation.SelectedIndex == 0)
            {
                if (rBtnTypeAll.Checked)
                    //dt = ObjProductMaster.daClass.return_DataTable("SELECT 'Sales' as Type, (Select Location_Name from Set_LocationMaster where Location_Id=SH.Location_Id) as [Location Name],  case when SH.Post=1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No], (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code],  (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],  (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name],  SD.Quantity, SD.Field3 as [Free Qty],   SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount,   Sd.ReturnQty as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], SD.UnitPrice as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]   FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\' + Set_LocationMaster.Field2 AS headerLogoPath,  Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' union all  SELECT 'Return' as Type, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],  case when SRH.Post=1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100) as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],  (Select Top 1 CategoryName from Inv_CustomerCategory as ICC inner join Inv_CustomerCategory_Customer as ICCC on ICCC.Customer_Category_Id = ICC.Trans_Id where ICCC.Customer_Id = SRH.Customer_Id and ICCC.IsActive = 'True' and ICC.IsActive = 'True') as [Customer Category],  (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code],  (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person],  (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name],  SRD.Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], SRD.UnitPrice as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]     FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\' + Set_LocationMaster.Field2 AS headerLogoPath,  Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True'");
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT 'Sales' as Type, 'Parent' as CustomerStatus, SH.Supplier_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],   (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name],  cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty],  SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3)) as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.IsActive = 'True' and EM.ParentContactId = '0'  union all SELECT 'Sales' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],   case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],   (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name], cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty], SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3)) as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.IsActive = 'True' and EM.ParentContactId != '0' union all  SELECT 'Return' as Type, 'Parent' as CustomerStatus, SRH.Customer_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],  case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.IsActive = 'True' and EM.ParentContactId = '0'  union all  SELECT 'Return' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],   case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person], (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.IsActive = 'True' and EM.ParentContactId != '0'");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT 'Sales' as Type, 'Parent' as CustomerStatus, SH.Supplier_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],  (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],   (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name], cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty],  SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3)) as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Post=1  AND SH.IsActive = 'True' and EM.ParentContactId = '0'  union all SELECT 'Sales' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],   case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],   (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name], cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty], SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3)) as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Post=1  AND SH.IsActive = 'True' and  EM.ParentContactId != '0' union all  SELECT 'Return' as Type, 'Parent' as CustomerStatus, SRH.Customer_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],  case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],(Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and  SRH.Post=1  AND SRH.IsActive = 'True' and EM.ParentContactId = '0'  union all  SELECT 'Return' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],   case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],    (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person], (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.Post=1  AND SRH.IsActive = 'True' and EM.ParentContactId != '0'");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT 'Sales' as Type, 'Parent' as CustomerStatus, SH.Supplier_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],  (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],   (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name], cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty],  SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3)) as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Post=0  AND SH.IsActive = 'True' and EM.ParentContactId = '0'  union all SELECT 'Sales' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],   case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],   (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name], cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty], SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3)) as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Post=0  AND SH.IsActive = 'True' and EM.ParentContactId != '0' union all  SELECT 'Return' as Type, 'Parent' as CustomerStatus, SRH.Customer_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],  case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],(Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.Post=0  AND SRH.IsActive = 'True' and EM.ParentContactId = '0'  union all  SELECT 'Return' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],   case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person], (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.Post=0  AND SRH.IsActive = 'True' and EM.ParentContactId != '0'");
            }
            else
            {
                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT 'Sales' as Type, 'Parent' as CustomerStatus, SH.Supplier_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],   (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name], cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty],  SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3)) as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.IsActive = 'True' and  EM.ParentContactId = '0'  union all SELECT 'Sales' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],   case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],   (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name], cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty], SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3)) as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.IsActive = 'True' and EM.ParentContactId != '0' union all  SELECT 'Return' as Type, 'Parent' as CustomerStatus, SRH.Customer_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],  case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],  (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SRH.IsActive = 'True' and EM.ParentContactId = '0'  union all  SELECT 'Return' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],   case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],    (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person], (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity ,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SRH.IsActive = 'True' and EM.ParentContactId != '0'");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT 'Sales' as Type, 'Parent' as CustomerStatus, SH.Supplier_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],   (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name], cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty],  SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount,cast(Sd.ReturnQty as decimal(18,3))  as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.Post=1  AND SH.IsActive = 'True' and  EM.ParentContactId = '0'  union all SELECT 'Sales' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],   case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],   (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name], cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty], SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount,cast(Sd.ReturnQty as decimal(18,3))  as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.Post=1  AND SH.IsActive = 'True' and EM.ParentContactId != '0' union all  SELECT 'Return' as Type, 'Parent' as CustomerStatus, SRH.Customer_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],  case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],    (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SRH.Post=1  AND SRH.IsActive = 'True' and EM.ParentContactId = '0'  union all  SELECT 'Return' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],   case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],    (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person], (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SRH.Post=1  AND SRH.IsActive = 'True' and EM.ParentContactId != '0'");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("SELECT 'Sales' as Type, 'Parent' as CustomerStatus, SH.Supplier_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],   (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name],  cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty],  SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3))  as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.Post=0  AND SH.IsActive = 'True' and   EM.ParentContactId = '0'  union all SELECT 'Sales' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SH.Location_Id) as [Location Name],   case when SH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as   [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No],   (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],   (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name],  cast(SD.Quantity as decimal(18,3)) as Quantity, SD.Field3 as [Free Qty], SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount, cast(Sd.ReturnQty as decimal(18,3)) as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], cast(SD.UnitPrice as decimal(18,3)) as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]  FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SH.Supplier_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' +  CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath, Set_AddressMaster.Address,  Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode,  Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo,  Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.Post=0  AND SH.IsActive = 'True' and EM.ParentContactId != '0' union all  SELECT 'Return' as Type, 'Parent' as CustomerStatus, SRH.Customer_Id as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],  case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],    (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person],  (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name], cast(SRD.Quantity as decimal(18,3)) as Quantity,'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SRH.Post=0  AND SRH.IsActive = 'True' and EM.ParentContactId = '0'  union all  SELECT 'Return' as Type, 'Child' as CustomerStatus,  EM.ParentContactId as CustomerCode, (Select Location_Name from Set_LocationMaster where Location_Id = SRH.Location_Id) as [Location Name],   case when SRH.Post = 1 then 'Posted' else 'Un Posted' end as Post,  SRH.Return_Date as [Invoice Date],  CONVERT(varchar(3), SRH.Return_Date, 100)   as [Month], Year(SRH.Return_Date) as [Year], SRH.Return_No as [Invoice No],  (Select Field1 from Ems_Contact_Group where Contact_Id = SRH.Customer_Id and Group_Id = '1') as [Customer Code],  CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SRH.Customer_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SRH.SalesPerson_Id) as [Sales Person], (select Top 1 PBM.Brand_Name from Inv_Product_Brand as PB inner join Inv_ProductBrandMaster as PBM on PBM.PBrandId = PB.PBrandId where PB.ProductId = SRD.Product_Id) as ProductBrandName, (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SRD.Product_Id) AS[Product Name],  cast(SRD.Quantity as decimal(18,3)),'0' as [Free Qty],SRH.GrandTotal as [Total Amount],  '0' AS ReturnNetAmount, '0' as [Return Quantity], (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SRD.Unit_Id) as [Sales Unit], cast(SRD.UnitPrice as decimal(18,3)) as [Unit Price],SRD.DiscountV as [Discount Value], SRD.Field2 as [Average Cost],  '0' as [Average Cost by unit], ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) - ((ISNULL(SRD.UnitPrice, 0) * ISNULL(SRD.Quantity, 0)) * ISNULL(SRD.DiscountP, 0) / 100)) AS[Selling Price],  '0' AS[Return No], '' AS[Return Date],  ISNULL(cast(round((SRD.Quantity * SRD.UnitPrice), 2) as numeric(36, 4)), 0) as [Return Amount]  FROM Inv_SalesReturnDetail AS SRD INNER JOIN Inv_SalesReturnHeader AS SRH ON(SRH.Company_Id = SRD.Company_Id AND SRH.Brand_Id = SRD.Brand_Id AND SRH.Location_ID = SRD.Location_ID AND SRH.Trans_Id = SRD.Return_No) Inner join Ems_ContactMaster as EM on EM.Trans_Id = SRH.Customer_Id LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '' + Set_LocationMaster.Field2 AS headerLogoPath,   Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,   Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON SRH.Location_Id = rpt_header.l_id WHERE cast(SRH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SRH.Post=0  AND SRH.IsActive = 'True' and EM.ParentContactId != '0'");
            }

            //dt = ObjProductMaster.daClass.return_DataTable("SELECT 'Sales' as Type, SH.Supplier_Id, (Select Location_Name from Set_LocationMaster where Location_Id=SH.Location_Id) as [Location Name],  case when SH.Post=1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No], (Select Top 1 CategoryName from Inv_CustomerCategory as ICC inner join Inv_CustomerCategory_Customer as ICCC on ICCC.Customer_Category_Id = ICC.Trans_Id where ICCC.Customer_Id = SH.Supplier_Id and ICCC.IsActive = 'True' and ICC.IsActive = 'True') as [Customer Category],  (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],  (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name],  SD.Quantity, SD.Field3 as [Free Qty],   SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount,   Sd.ReturnQty as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], SD.UnitPrice as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]   FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\' + Set_LocationMaster.Field2 AS headerLogoPath,  Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Location_Id in (" + ddlLocation.SelectedValue + ") AND SH.IsActive = 'True' ");
            //gvSalesData.DataSource = dt;
            //gvSalesData.DataBind();

            //foreach (GridViewRow gvr in gvSalesData.Rows)
            //{
            //    HiddenField hdnSupplierId = (HiddenField)gvr.FindControl("hdnSupplierId");
            //    GridView gvChild = (GridView)gvr.FindControl("gvChildData");

            //    DataTable dtContactData = ObjProductMaster.daClass.return_DataTable("Select * from Ems_ContactMaster where ParentContactId='" + hdnSupplierId.Value + "'");
            //    if (dtContactData.Rows.Count > 0)
            //    {
            //        string SupplierId = string.Empty;
            //        for (int i = 0; i < dtContactData.Rows.Count; i++)
            //        {

            //            if (SupplierId == "")
            //            {
            //                SupplierId = dtContactData.Rows[i]["Trans_Id"].ToString();
            //            }
            //            else
            //            {
            //                SupplierId = SupplierId + "','" + dtContactData.Rows[i]["Trans_Id"].ToString();
            //            }
            //        }

            //        if (SupplierId != "")
            //        {
            //            DataTable dtChild = ObjProductMaster.daClass.return_DataTable("SELECT 'Sales' as Type, SH.Supplier_Id, (Select Location_Name from Set_LocationMaster where Location_Id=SH.Location_Id) as [Location Name],  case when SH.Post=1 then 'Posted' else 'Un Posted' end as Post,  SH.Invoice_Date as [Invoice Date], CONVERT(varchar(3), SH.Invoice_Date, 100) as [Month], Year(SH.Invoice_Date) as [Year], SH.Invoice_No as [Invoice No], (Select Top 1 CategoryName from Inv_CustomerCategory as ICC inner join Inv_CustomerCategory_Customer as ICCC on ICCC.Customer_Category_Id = ICC.Trans_Id where ICCC.Customer_Id = SH.Supplier_Id and ICCC.IsActive = 'True' and ICC.IsActive = 'True') as [Customer Category],  (Select Field1 from Ems_Contact_Group where Contact_Id = SH.Supplier_Id and Group_Id = '1') as [Customer Code], CASE WHEN(SELECT status FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) = 'Company' THEN(SELECT Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) ELSE(SELECT Field3 + ' ' + Name FROM Ems_ContactMaster WHERE Trans_Id = SH.Supplier_Id) END AS[Customer Name], (Select Emp_Code from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person Code], (Select Emp_Name from Set_EmployeeMaster where Emp_Id = SH.SalesPerson_Id) as [Sales Person],  (SELECT ProductCode FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Code],  (SELECT EProductName FROM Inv_ProductMaster WHERE ProductId = SD.Product_Id) AS[Product Name],  SD.Quantity, SD.Field3 as [Free Qty],   SH.GrandTotal as [Total Amount],   (Select top 1 GrandTotal from Inv_SalesReturnHeader where Invoice_Id = SH.Trans_Id) AS ReturnNetAmount,   Sd.ReturnQty as [Return Quantity],  (Select Unit_Name + '/' + Unit_Code from Inv_UnitMaster where Unit_Id = SD.Unit_Id) as [Sales Unit], SD.UnitPrice as [Unit Price], SD.DiscountV as [Discount Value],  SD.Field2 as [Average Cost], cast((SD.Field2 / (Select Coversion_Qty from Inv_UnitMaster where Unit_Id = SD.Unit_Id)) as numeric(36, 5)) [Average Cost by unit], ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) - ((ISNULL(SD.UnitPrice, 0) * ISNULL(SD.Quantity, 0)) * ISNULL(SD.DiscountP, 0) / 100)) AS[Selling Price],  SH.ReturnNo AS[Return No], SH.Field7 AS[Return Date], cast(round((Sd.ReturnQty * SD.UnitPrice), 2) as numeric(36, 4)) as [Return Amount]   FROM Inv_SalesInvoiceDetail AS SD INNER JOIN Inv_SalesInvoiceHeader AS SH ON(SH.Company_Id = SD.Company_Id AND SH.Brand_Id = SD.Brand_Id AND SH.Location_ID = SD.Location_ID AND SH.Trans_Id = SD.Invoice_No) LEFT JOIN(SELECT Set_LocationMaster.location_id AS l_id, Set_LocationMaster.Location_Name AS HeaderName, Set_LocationMaster.Location_Name_L AS HeaderName_L, Set_LocationMaster.Field2 AS Imageurl, '~\\CompanyResource\\' + CAST(Set_LocationMaster.Location_Id AS varchar) + '\' + Set_LocationMaster.Field2 AS headerLogoPath,  Set_AddressMaster.Address, Set_AddressMaster.Street, Set_AddressMaster.Block,  Set_AddressMaster.Avenue, (CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Sys_StateMaster.State_Name ELSE Set_AddressMaster.StateId END)  AS StateId, (CASE WHEN ISNUMERIC(Set_AddressMaster.cityid) = 1 THEN Sys_CityMaster.City_Name ELSE Set_AddressMaster.CityId END) AS CityId, Set_AddressMaster.CountryId, Set_AddressMaster.PinCode, Set_AddressMaster.PhoneNo1, Set_AddressMaster.PhoneNo2, Set_AddressMaster.MobileNo1,  Set_AddressMaster.MobileNo2, Set_AddressMaster.FaxNo, Set_AddressMaster.WebSite FROM Set_LocationMaster FULL OUTER JOIN Set_AddressChild ON Set_LocationMaster.Location_Id = Set_AddressChild.Add_Ref_Id AND Set_AddressChild.Add_Type = 'Location' FULL OUTER JOIN Set_AddressMaster ON Set_AddressChild.Ref_Id = Set_AddressMaster.Trans_Id LEFT JOIN Sys_StateMaster ON Sys_StateMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.StateId) = 1 THEN Set_AddressMaster.StateId ELSE 0 END LEFT JOIN Sys_CityMaster ON Sys_CityMaster.Trans_Id = CASE WHEN ISNUMERIC(Set_AddressMaster.CityId) = 1 THEN Set_AddressMaster.CityId ELSE 0 END) rpt_header ON sh.Location_Id = rpt_header.l_id WHERE cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Location_Id in (" + ddlLocation.SelectedValue + ") and SH.IsActive = 'True' and SH.Supplier_Id in ('" + SupplierId +"')  ");
            //            if (dtChild.Rows.Count > 0)
            //            {
            //                gvChild.DataSource = dtChild;
            //                gvChild.DataBind();
            //            }
            //        }
            //    }
            //}
        }

        if (ddlReportType.SelectedValue == "DSPP")
        {

            //dt = ObjProductMaster.daClass.return_DataTable("select * from (select PM.ProductCode, (Select Top 1 CM.Name  from Inv_Product_Suppliers as PS inner join Ems_ContactMaster as CM on PS.Supplier_Id = CM.Trans_Id where Product_Id = PM.ProductId) as SupplierName, PM.EProductName, (Select Unit_Name from Inv_UnitMaster where Unit_Id = PM.UnitId) as ProductUnit,  ISNULL((Select(SUM(SD.Quantity)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '"+txtFromDate.Text+ "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' ),0) as SalesQty,  ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' ),0) as SalesAmt,  ISNULL((Select(SUM(SRD.ReturnQty)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True'),0) as ReturnQty, ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True'),0) as ReturnAmt,  (ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' ), 0) - ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '"+ txtToDate.Text +"' AND SRH.IsActive = 'True'),0)) as NetSales,   '' as [Net Cost],  '' as GrossProfit, '' as [% Profit], '' as [Total Profit %],  '' as [Moving %]  from Inv_ProductMaster as PM ) as NewData  where SalesQty!= '0'  ");
            if (ddlLocation.SelectedIndex == 0)
            {
                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable(" select ProductCode, SupplierName,[Product Name],Unit, SalesQty,SalesAmt,ReturnQty,ReturnAmt,NetSales,[Net Cost],GrossProfit,[% Profit],   [Total Profit %], [Moving %] from(select ProductCode, SupplierName, EProductName as [Product Name], ProductUnit as Unit, SalesQty, SalesAmt, ReturnQty, ReturnAmt, NetSales, [Net Cost], (NetSales -[Net Cost]) as GrossProfit, SUM(NetSales -[Net Cost]) over() as [GrossTotal], SUM(NetSales) over() as TotalSale, ISNULL(CAST(((NetSales / SUM(NetSales) over()) * 100) as Numeric(18,3)),0) as [Moving %], ISNULL(CAST((((NetSales -[Net Cost]) / (SUM(NetSales -[Net Cost]) over())) * 100) as Numeric(18,3)),0) as [Total Profit %],   case when(NetSales -[Net Cost]) > 0 and NetSales > 0 then((NetSales -[Net Cost]) * 100 / NetSales) else '0' end as [% Profit]  from (select PM.ProductCode, (Select Top 1 CM.Name  from Inv_Product_Suppliers as PS inner join Ems_ContactMaster as CM on PS.Supplier_Id = CM.Trans_Id where Product_Id = PM.ProductId) as SupplierName, PM.EProductName, (Select Unit_Name from Inv_UnitMaster where Unit_Id = PM.UnitId) as ProductUnit,  ISNULL((Select(SUM(SD.Quantity)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True'),0) as SalesQty,  ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' ),0) as SalesAmt,  ISNULL((Select(SUM(SRD.ReturnQty)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True'),0) as ReturnQty,  ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True'),0) as ReturnAmt,  (ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' ), 0) - ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "'  AND SRH.IsActive = 'True'),0)) as NetSales,   ISNULL((Select((SUM(SD.Quantity)) * (SUM(CAST(SD.Field2 as numeric))))  from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' ),0) as [Net Cost]  from Inv_ProductMaster as PM ) as NewData  where SalesQty!= '0'  )   as FinalData group by FinalData.ProductCode, FinalData.SupplierName, FinalData.[Product Name], FinalData.Unit, FinalData.SalesQty, FinalData.SalesAmt, FinalData.ReturnQty, FinalData.ReturnAmt, FinalData.NetSales, FinalData.[Net Cost], FinalData.GrossProfit, FinalData.[% Profit], FinalData.GrossTotal, FinalData.[Total Profit %],FinalData.[Moving %]");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable(" select ProductCode, SupplierName,[Product Name],Unit, SalesQty,SalesAmt,ReturnQty,ReturnAmt,NetSales,[Net Cost],GrossProfit,[% Profit],   [Total Profit %], [Moving %] from(select ProductCode, SupplierName, EProductName as [Product Name], ProductUnit as Unit, SalesQty, SalesAmt, ReturnQty, ReturnAmt, NetSales, [Net Cost], (NetSales -[Net Cost]) as GrossProfit, SUM(NetSales -[Net Cost]) over() as [GrossTotal], SUM(NetSales) over() as TotalSale,  ISNULL(CAST(((NetSales / SUM(NetSales) over()) * 100) as Numeric(18,3)),0) as [Moving %], ISNULL(CAST((((NetSales -[Net Cost]) / (SUM(NetSales -[Net Cost]) over())) * 100) as Numeric(18,3)),0) as [Total Profit %],  case when(NetSales -[Net Cost]) > 0 and NetSales > 0 then((NetSales -[Net Cost]) * 100 / NetSales) else '0' end as [% Profit]  from (select PM.ProductCode, (Select Top 1 CM.Name  from Inv_Product_Suppliers as PS inner join Ems_ContactMaster as CM on PS.Supplier_Id = CM.Trans_Id where Product_Id = PM.ProductId) as SupplierName, PM.EProductName, (Select Unit_Name from Inv_UnitMaster where Unit_Id = PM.UnitId) as ProductUnit,  ISNULL((Select(SUM(SD.Quantity)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'True'),0) as SalesQty,  ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'True'),0) as SalesAmt,  ISNULL((Select(SUM(SRD.ReturnQty)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'True'),0) as ReturnQty,  ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'True'),0) as ReturnAmt,  (ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'True'), 0) - ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'True'),0)) as NetSales,   ISNULL((Select((SUM(SD.Quantity)) * (SUM(CAST(SD.Field2 as numeric))))  from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'True'),0) as [Net Cost]  from Inv_ProductMaster as PM ) as NewData  where SalesQty!= '0'  )   as FinalData group by FinalData.ProductCode, FinalData.SupplierName, FinalData.[Product Name], FinalData.Unit, FinalData.SalesQty, FinalData.SalesAmt, FinalData.ReturnQty, FinalData.ReturnAmt, FinalData.NetSales, FinalData.[Net Cost], FinalData.GrossProfit, FinalData.[% Profit], FinalData.GrossTotal, FinalData.[Total Profit %],FinalData.[Moving %]");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable(" select ProductCode, SupplierName,[Product Name],Unit, SalesQty,SalesAmt,ReturnQty,ReturnAmt,NetSales,[Net Cost],GrossProfit,[% Profit],   [Total Profit %], [Moving %] from(select ProductCode, SupplierName, EProductName as [Product Name], ProductUnit as Unit, SalesQty, SalesAmt, ReturnQty, ReturnAmt, NetSales, [Net Cost], (NetSales -[Net Cost]) as GrossProfit, SUM(NetSales -[Net Cost]) over() as [GrossTotal], SUM(NetSales) over() as TotalSale,  ISNULL(CAST(((NetSales / SUM(NetSales) over()) * 100) as Numeric(18,3)),0) as [Moving %], ISNULL(CAST((((NetSales -[Net Cost]) / (SUM(NetSales -[Net Cost]) over())) * 100) as Numeric(18,3)),0) as [Total Profit %],  case when(NetSales -[Net Cost]) > 0 and NetSales > 0 then((NetSales -[Net Cost]) * 100 / NetSales) else '0' end as [% Profit]  from (select PM.ProductCode, (Select Top 1 CM.Name  from Inv_Product_Suppliers as PS inner join Ems_ContactMaster as CM on PS.Supplier_Id = CM.Trans_Id where Product_Id = PM.ProductId) as SupplierName, PM.EProductName, (Select Unit_Name from Inv_UnitMaster where Unit_Id = PM.UnitId) as ProductUnit,  ISNULL((Select(SUM(SD.Quantity)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'False'),0) as SalesQty,  ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'False'),0) as SalesAmt,  ISNULL((Select(SUM(SRD.ReturnQty)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'False'),0) as ReturnQty,  ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'False'),0) as ReturnAmt,  (ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'False'), 0) - ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'False'),0)) as NetSales,   ISNULL((Select((SUM(SD.Quantity)) * (SUM(CAST(SD.Field2 as numeric))))  from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'False'),0) as [Net Cost]  from Inv_ProductMaster as PM ) as NewData  where SalesQty!= '0'  )   as FinalData group by FinalData.ProductCode, FinalData.SupplierName, FinalData.[Product Name], FinalData.Unit, FinalData.SalesQty, FinalData.SalesAmt, FinalData.ReturnQty, FinalData.ReturnAmt, FinalData.NetSales, FinalData.[Net Cost], FinalData.GrossProfit, FinalData.[% Profit], FinalData.GrossTotal, FinalData.[Total Profit %],FinalData.[Moving %]");
            }
            else
            {
                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable(" select ProductCode, SupplierName,[Product Name],Unit, SalesQty,SalesAmt,ReturnQty,ReturnAmt,NetSales,[Net Cost],GrossProfit,[% Profit],   [Total Profit %], [Moving %] from(select ProductCode, SupplierName, EProductName as [Product Name], ProductUnit as Unit, SalesQty, SalesAmt, ReturnQty, ReturnAmt, NetSales, [Net Cost], (NetSales -[Net Cost]) as GrossProfit, SUM(NetSales -[Net Cost]) over() as [GrossTotal], SUM(NetSales) over() as TotalSale,  ISNULL(CAST(((NetSales / SUM(NetSales) over()) * 100) as Numeric(18,3)),0) as [Moving %], ISNULL(CAST((((NetSales -[Net Cost]) / (SUM(NetSales -[Net Cost]) over())) * 100) as Numeric(18,3)),0) as [Total Profit %],  case when(NetSales -[Net Cost]) > 0 and NetSales > 0 then((NetSales -[Net Cost]) * 100 / NetSales) else '0' end as [% Profit]  from (select PM.ProductCode, (Select Top 1 CM.Name  from Inv_Product_Suppliers as PS inner join Ems_ContactMaster as CM on PS.Supplier_Id = CM.Trans_Id where Product_Id = PM.ProductId) as SupplierName, PM.EProductName, (Select Unit_Name from Inv_UnitMaster where Unit_Id = PM.UnitId) as ProductUnit,  ISNULL((Select(SUM(SD.Quantity)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as SalesQty,  ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as SalesAmt,  ISNULL((Select(SUM(SRD.ReturnQty)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True'    and SRH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as ReturnQty,  ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True'    and SRH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as ReturnAmt,  (ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True'  and SH.Location_Id in (" + ddlLocation.SelectedValue + ")), 0) - ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True'   and SRH.Location_Id in (" + ddlLocation.SelectedValue + ")),0)) as NetSales,   ISNULL((Select((SUM(SD.Quantity)) * (SUM(CAST(SD.Field2 as numeric))))  from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as [Net Cost]  from Inv_ProductMaster as PM ) as NewData  where SalesQty!= '0'  )   as FinalData group by FinalData.ProductCode, FinalData.SupplierName, FinalData.[Product Name], FinalData.Unit, FinalData.SalesQty, FinalData.SalesAmt, FinalData.ReturnQty, FinalData.ReturnAmt, FinalData.NetSales, FinalData.[Net Cost], FinalData.GrossProfit, FinalData.[% Profit], FinalData.GrossTotal, FinalData.[Total Profit %],FinalData.[Moving %]");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable(" select ProductCode, SupplierName,[Product Name],Unit, SalesQty,SalesAmt,ReturnQty,ReturnAmt,NetSales,[Net Cost],GrossProfit,[% Profit],   [Total Profit %], [Moving %] from(select ProductCode, SupplierName, EProductName as [Product Name], ProductUnit as Unit, SalesQty, SalesAmt, ReturnQty, ReturnAmt, NetSales, [Net Cost], (NetSales -[Net Cost]) as GrossProfit, SUM(NetSales -[Net Cost]) over() as [GrossTotal], SUM(NetSales) over() as TotalSale,  ISNULL(CAST(((NetSales / SUM(NetSales) over()) * 100) as Numeric(18,3)),0) as [Moving %], ISNULL(CAST((((NetSales -[Net Cost]) / (SUM(NetSales -[Net Cost]) over())) * 100) as Numeric(18,3)),0) as [Total Profit %],  case when(NetSales -[Net Cost]) > 0 and NetSales > 0 then((NetSales -[Net Cost]) * 100 / NetSales) else '0' end as [% Profit]  from (select PM.ProductCode, (Select Top 1 CM.Name  from Inv_Product_Suppliers as PS inner join Ems_ContactMaster as CM on PS.Supplier_Id = CM.Trans_Id where Product_Id = PM.ProductId) as SupplierName, PM.EProductName, (Select Unit_Name from Inv_UnitMaster where Unit_Id = PM.UnitId) as ProductUnit,  ISNULL((Select(SUM(SD.Quantity)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'True' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as SalesQty,  ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'True' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as SalesAmt,  ISNULL((Select(SUM(SRD.ReturnQty)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'True' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as ReturnQty,  ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'True' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as ReturnAmt,  (ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'True' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")), 0) - ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'True' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ")),0)) as NetSales,   ISNULL((Select((SUM(SD.Quantity)) * (SUM(CAST(SD.Field2 as numeric))))  from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'True' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as [Net Cost]  from Inv_ProductMaster as PM ) as NewData  where SalesQty!= '0'  )   as FinalData group by FinalData.ProductCode, FinalData.SupplierName, FinalData.[Product Name], FinalData.Unit, FinalData.SalesQty, FinalData.SalesAmt, FinalData.ReturnQty, FinalData.ReturnAmt, FinalData.NetSales, FinalData.[Net Cost], FinalData.GrossProfit, FinalData.[% Profit], FinalData.GrossTotal, FinalData.[Total Profit %],FinalData.[Moving %]");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable(" select ProductCode, SupplierName,[Product Name],Unit, SalesQty,SalesAmt,ReturnQty,ReturnAmt,NetSales,[Net Cost],GrossProfit,[% Profit],   [Total Profit %], [Moving %] from(select ProductCode, SupplierName, EProductName as [Product Name], ProductUnit as Unit, SalesQty, SalesAmt, ReturnQty, ReturnAmt, NetSales, [Net Cost], (NetSales -[Net Cost]) as GrossProfit, SUM(NetSales -[Net Cost]) over() as [GrossTotal], SUM(NetSales) over() as TotalSale,  ISNULL(CAST(((NetSales / SUM(NetSales) over()) * 100) as Numeric(18,3)),0) as [Moving %], ISNULL(CAST((((NetSales -[Net Cost]) / (SUM(NetSales -[Net Cost]) over())) * 100) as Numeric(18,3)),0) as [Total Profit %],  case when(NetSales -[Net Cost]) > 0 and NetSales > 0 then((NetSales -[Net Cost]) * 100 / NetSales) else '0' end as [% Profit]  from (select PM.ProductCode, (Select Top 1 CM.Name  from Inv_Product_Suppliers as PS inner join Ems_ContactMaster as CM on PS.Supplier_Id = CM.Trans_Id where Product_Id = PM.ProductId) as SupplierName, PM.EProductName, (Select Unit_Name from Inv_UnitMaster where Unit_Id = PM.UnitId) as ProductUnit,  ISNULL((Select(SUM(SD.Quantity)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'False' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as SalesQty,  ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'False' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as SalesAmt,  ISNULL((Select(SUM(SRD.ReturnQty)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'False' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as ReturnQty,  ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'False' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as ReturnAmt,  (ISNULL((Select CAST((SUM(SD.Quantity * SD.UnitPrice)) as numeric(18, 3)) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'False' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")), 0) - ISNULL((Select CAST((SUM(SRD.ReturnQty * SRD.UnitPrice)) as numeric(18, 3)) from Inv_SalesReturnDetail as SRD inner join Inv_SalesReturnHeader as SRH on SRH.Trans_Id = SRD.Return_No where SRD.Product_Id = PM.ProductId and cast(SRH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(SRH.Return_Date as date) <= '" + txtToDate.Text + "' AND SRH.IsActive = 'True' and SRH.Post = 'False' and SRH.Location_Id in (" + ddlLocation.SelectedValue + ")),0)) as NetSales,   ISNULL((Select((SUM(SD.Quantity)) * (SUM(CAST(SD.Field2 as numeric))))  from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' AND SH.IsActive = 'True' and SH.Post = 'False' and SH.Location_Id in (" + ddlLocation.SelectedValue + ")),0) as [Net Cost]  from Inv_ProductMaster as PM ) as NewData  where SalesQty!= '0'  )   as FinalData group by FinalData.ProductCode, FinalData.SupplierName, FinalData.[Product Name], FinalData.Unit, FinalData.SalesQty, FinalData.SalesAmt, FinalData.ReturnQty, FinalData.ReturnAmt, FinalData.NetSales, FinalData.[Net Cost], FinalData.GrossProfit, FinalData.[% Profit], FinalData.GrossTotal, FinalData.[Total Profit %],FinalData.[Moving %]");
            }

            float Total = 0;
            foreach (DataRow dr in dt.Rows) // search whole table
            {
                if (dr["GrossProfit"].ToString() != "")
                {
                    if (Total == 0)
                    {
                        Total = float.Parse(dr["GrossProfit"].ToString());
                    }
                    else
                    {
                        Total = Total + float.Parse(dr["GrossProfit"].ToString());
                    }
                }
            }

            if (Total != 0)
            {
                lblTotalProfit.Text = "Total Profit : " + Total.ToString();
            }
        }

        if (ddlReportType.SelectedValue == "IRPR")
        {
            CustomerName.Visible = true;
            SalesPerson.Visible = true;

            rBtnTypeAll.Visible = false;
            rBtnTypePosted.Visible = false;
            rBtnTypeUnPosted.Visible = false;

            string strEmployeeId = string.Empty;
            if (txtSummarySalesPerson.Text != "")
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtSummarySalesPerson.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                strEmployeeId = Emp_ID;
            }

            if (ddlLocation.SelectedIndex == 0)
            {
                if (txtCustomer.Text != "" && strEmployeeId != "")
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select SalesReturn.* from(Select PM.ProductCode, PM.EProductName,ISNULL((Select CAST(SUM(Quantity) as numeric) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and SH.SalesPerson_Id='" + strEmployeeId + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'), 0.00) as TotalSaleQty, ISNULL((Select SUM((Cast(SD.Field3 as numeric))) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True'   and SD.Product_Id = PM.ProductId and SH.SalesPerson_Id='" + strEmployeeId + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'), 0.00) as TotalFreeQty, ISNULL((Select CAST(SUM(SD.DiscountV) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True'  and SD.Product_Id = PM.ProductId and SH.SalesPerson_Id='" + strEmployeeId + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalDiscountAmt,  ISNULL((Select CAST((SUM(SD.Quantity) * SUM(SD.UnitPrice)) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and SH.SalesPerson_Id='" + strEmployeeId + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalNetAmt, ISNULL((Select CAST(SUM(RD.Quantity) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and RH.SalesPerson_Id='" + strEmployeeId + "' and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalReturnQty,  ISNULL((Select CAST(SUM(RD.DiscountV) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and RH.SalesPerson_Id='" + strEmployeeId + "' and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalReturnDiscount,  ISNULL((Select CAST((SUM(RD.Quantity) * SUM(RD.UnitPrice)) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and RH.SalesPerson_Id='" + strEmployeeId + "' and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and  cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalNetReturn from Inv_ProductMaster as PM where PM.IsActive = 'True') SalesReturn   where TotalSaleQty != '0' or TotalFreeQty!= '0' or TotalDiscountAmt!= '0' or TotalNetAmt!= '0' or TotalReturnQty!= '0' or TotalReturnDiscount!= '0' or TotalNetReturn != '0' ");
                }
                else if (txtCustomer.Text != "")
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select SalesReturn.* from(Select PM.ProductCode, PM.EProductName,ISNULL((Select CAST(SUM(Quantity) as numeric) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'), 0.00) as TotalSaleQty, ISNULL((Select SUM((Cast(SD.Field3 as numeric))) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True'   and SD.Product_Id = PM.ProductId and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'), 0.00) as TotalFreeQty, ISNULL((Select CAST(SUM(SD.DiscountV) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalDiscountAmt,  ISNULL((Select CAST((SUM(SD.Quantity) * SUM(SD.UnitPrice)) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalNetAmt, ISNULL((Select CAST(SUM(RD.Quantity) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalReturnQty,  ISNULL((Select CAST(SUM(RD.DiscountV) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalReturnDiscount,  ISNULL((Select CAST((SUM(RD.Quantity) * SUM(RD.UnitPrice)) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "' and  cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalNetReturn from Inv_ProductMaster as PM where PM.IsActive = 'True') SalesReturn   where TotalSaleQty != '0' or TotalFreeQty!= '0' or TotalDiscountAmt!= '0' or TotalNetAmt!= '0' or TotalReturnQty!= '0' or TotalReturnDiscount!= '0' or TotalNetReturn != '0' ");
                }
                else if (strEmployeeId != "")
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select SalesReturn.* from(Select PM.ProductCode, PM.EProductName,ISNULL((Select CAST(SUM(Quantity) as numeric) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and SH.SalesPerson_Id='" + strEmployeeId + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'), 0.00) as TotalSaleQty, ISNULL((Select SUM((Cast(SD.Field3 as numeric))) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and SH.SalesPerson_Id='" + strEmployeeId + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'), 0.00) as TotalFreeQty, ISNULL((Select CAST(SUM(SD.DiscountV) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and SH.SalesPerson_Id='" + strEmployeeId + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalDiscountAmt,  ISNULL((Select CAST((SUM(SD.Quantity) * SUM(SD.UnitPrice)) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and SH.SalesPerson_Id='" + strEmployeeId + "' and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalNetAmt, ISNULL((Select CAST(SUM(RD.Quantity) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and RH.SalesPerson_Id='" + strEmployeeId + "' and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalReturnQty,  ISNULL((Select CAST(SUM(RD.DiscountV) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and RH.SalesPerson_Id='" + strEmployeeId + "' and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalReturnDiscount,  ISNULL((Select CAST((SUM(RD.Quantity) * SUM(RD.UnitPrice)) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and RH.SalesPerson_Id='" + strEmployeeId + "' and  cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalNetReturn from Inv_ProductMaster as PM where PM.IsActive = 'True') SalesReturn   where TotalSaleQty != '0' or TotalFreeQty!= '0' or TotalDiscountAmt!= '0' or TotalNetAmt!= '0' or TotalReturnQty!= '0' or TotalReturnDiscount!= '0' or TotalNetReturn != '0' ");
                }
                else
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select SalesReturn.* from(Select  PM.ProductCode, PM.EProductName,ISNULL((Select CAST(SUM(Quantity) as numeric) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True'   and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'), 0.00) as TotalSaleQty, ISNULL((Select SUM((Cast(SD.Field3 as numeric))) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'), 0.00) as TotalFreeQty, ISNULL((Select CAST(SUM(SD.DiscountV) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalDiscountAmt,  ISNULL((Select CAST((SUM(SD.Quantity) * SUM(SD.UnitPrice)) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalNetAmt, ISNULL((Select CAST(SUM(RD.Quantity) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalReturnQty,  ISNULL((Select CAST(SUM(RD.DiscountV) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalReturnDiscount,  ISNULL((Select CAST((SUM(RD.Quantity) * SUM(RD.UnitPrice)) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "'),0.00) as TotalNetReturn from Inv_ProductMaster as PM where PM.IsActive = 'True') SalesReturn   where TotalSaleQty != '0' or TotalFreeQty!= '0' or TotalDiscountAmt!= '0' or TotalNetAmt!= '0' or TotalReturnQty!= '0' or TotalReturnDiscount!= '0' or TotalNetReturn != '0' ");
                }
            }
            else
            {
                if (txtCustomer.Text != "" && strEmployeeId != "")
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select SalesReturn.* from(Select  PM.ProductCode, PM.EProductName,ISNULL((Select CAST(SUM(Quantity) as numeric) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True'  and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.SalesPerson_Id='" + strEmployeeId + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and SD.Location_Id in (" + ddlLocation.SelectedValue + ") ), 0.00) as TotalSaleQty, ISNULL((Select SUM((Cast(SD.Field3 as numeric))) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.SalesPerson_Id='" + strEmployeeId + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and SD.Location_Id in (" + ddlLocation.SelectedValue + ")), 0.00) as TotalFreeQty, ISNULL((Select CAST(SUM(SD.DiscountV) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.SalesPerson_Id='" + strEmployeeId + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and SD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalDiscountAmt,  ISNULL((Select CAST((SUM(SD.Quantity) * SUM(SD.UnitPrice)) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.SalesPerson_Id='" + strEmployeeId + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and SD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalNetAmt, ISNULL((Select CAST(SUM(RD.Quantity) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RH.SalesPerson_Id='" + strEmployeeId + "' and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalReturnQty,  ISNULL((Select CAST(SUM(RD.DiscountV) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RH.SalesPerson_Id='" + strEmployeeId + "' and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalReturnDiscount,  ISNULL((Select CAST((SUM(RD.Quantity) * SUM(RD.UnitPrice)) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RH.SalesPerson_Id='" + strEmployeeId + "' and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalNetReturn from Inv_ProductMaster as PM where PM.IsActive = 'True') SalesReturn   where TotalSaleQty != '0' or TotalFreeQty!= '0' or TotalDiscountAmt!= '0' or TotalNetAmt!= '0' or TotalReturnQty!= '0' or TotalReturnDiscount!= '0' or TotalNetReturn != '0' ");
                }
                else if (txtCustomer.Text != "")
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select SalesReturn.* from(Select  PM.ProductCode, PM.EProductName,ISNULL((Select CAST(SUM(Quantity) as numeric) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True'  and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and SD.Location_Id in (" + ddlLocation.SelectedValue + ") ), 0.00) as TotalSaleQty, ISNULL((Select SUM((Cast(SD.Field3 as numeric))) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and SD.Location_Id in (" + ddlLocation.SelectedValue + ")), 0.00) as TotalFreeQty, ISNULL((Select CAST(SUM(SD.DiscountV) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and SD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalDiscountAmt,  ISNULL((Select CAST((SUM(SD.Quantity) * SUM(SD.UnitPrice)) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.Supplier_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and SD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalNetAmt, ISNULL((Select CAST(SUM(RD.Quantity) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalReturnQty,  ISNULL((Select CAST(SUM(RD.DiscountV) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalReturnDiscount,  ISNULL((Select CAST((SUM(RD.Quantity) * SUM(RD.UnitPrice)) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RH.Customer_Id='" + txtCustomer.Text.Split('/')[1].ToString() + "'  and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalNetReturn from Inv_ProductMaster as PM where PM.IsActive = 'True') SalesReturn   where TotalSaleQty != '0' or TotalFreeQty!= '0' or TotalDiscountAmt!= '0' or TotalNetAmt!= '0' or TotalReturnQty!= '0' or TotalReturnDiscount!= '0' or TotalNetReturn != '0' ");
                }
                else if (strEmployeeId != "")
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select SalesReturn.* from(Select  PM.ProductCode, PM.EProductName,ISNULL((Select CAST(SUM(Quantity) as numeric) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True'  and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.SalesPerson_Id='" + strEmployeeId + "' and SD.Location_Id in (" + ddlLocation.SelectedValue + ") ), 0.00) as TotalSaleQty, ISNULL((Select SUM((Cast(SD.Field3 as numeric))) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.SalesPerson_Id='" + strEmployeeId + "' and SD.Location_Id in (" + ddlLocation.SelectedValue + ")), 0.00) as TotalFreeQty, ISNULL((Select CAST(SUM(SD.DiscountV) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.SalesPerson_Id='" + strEmployeeId + "' and SD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalDiscountAmt,  ISNULL((Select CAST((SUM(SD.Quantity) * SUM(SD.UnitPrice)) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SH.SalesPerson_Id='" + strEmployeeId + "' and SD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalNetAmt, ISNULL((Select CAST(SUM(RD.Quantity) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RH.SalesPerson_Id='" + strEmployeeId + "' and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalReturnQty,  ISNULL((Select CAST(SUM(RD.DiscountV) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RH.SalesPerson_Id='" + strEmployeeId + "' and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalReturnDiscount,  ISNULL((Select CAST((SUM(RD.Quantity) * SUM(RD.UnitPrice)) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RH.SalesPerson_Id='" + strEmployeeId + "' and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalNetReturn from Inv_ProductMaster as PM where PM.IsActive = 'True') SalesReturn   where TotalSaleQty != '0' or TotalFreeQty!= '0' or TotalDiscountAmt!= '0' or TotalNetAmt!= '0' or TotalReturnQty!= '0' or TotalReturnDiscount!= '0' or TotalNetReturn != '0' ");
                }
                else
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select SalesReturn.* from(Select  PM.ProductCode, PM.EProductName,ISNULL((Select CAST(SUM(Quantity) as numeric) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True'  and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SD.Location_Id in (" + ddlLocation.SelectedValue + ") ), 0.00) as TotalSaleQty, ISNULL((Select SUM((Cast(SD.Field3 as numeric))) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SD.Location_Id in (" + ddlLocation.SelectedValue + ")), 0.00) as TotalFreeQty, ISNULL((Select CAST(SUM(SD.DiscountV) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalDiscountAmt,  ISNULL((Select CAST((SUM(SD.Quantity) * SUM(SD.UnitPrice)) as float) from Inv_SalesInvoiceDetail as SD inner join Inv_SalesInvoiceHeader as SH on SH.Trans_Id = SD.Invoice_No where SH.IsActive = 'True' and SH.Post = 'True' and SD.Product_Id = PM.ProductId and cast(SH.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(SH.Invoice_Date as date) <= '" + txtToDate.Text + "' and SD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalNetAmt, ISNULL((Select CAST(SUM(RD.Quantity) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalReturnQty,  ISNULL((Select CAST(SUM(RD.DiscountV) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalReturnDiscount,  ISNULL((Select CAST((SUM(RD.Quantity) * SUM(RD.UnitPrice)) as float) from Inv_SalesReturnDetail as RD inner join Inv_SalesReturnHeader as RH on RH.Trans_Id = RD.Return_No where RH.IsActive = 'True' and RH.Post = 'True' and RD.Product_Id = PM.ProductId and cast(RH.Return_Date as date) >= '" + txtFromDate.Text + "' and cast(RH.Return_Date as date) <= '" + txtToDate.Text + "' and RD.Location_Id in (" + ddlLocation.SelectedValue + ")),0.00) as TotalNetReturn from Inv_ProductMaster as PM where PM.IsActive = 'True') SalesReturn   where TotalSaleQty != '0' or TotalFreeQty!= '0' or TotalDiscountAmt!= '0' or TotalNetAmt!= '0' or TotalReturnQty!= '0' or TotalReturnDiscount!= '0' or TotalNetReturn != '0' ");
                }
            }
        }

        if (ddlReportType.SelectedValue == "SCWSR")
        {
            if (ddlLocation.SelectedIndex == 0)
            {


                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3))as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and  cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3))as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3))as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='false' and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");


            }
            else
            {
                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3))as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3))as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3))as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='false' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");

            }


            //dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3))as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");
        }
        if (ddlReportType.SelectedValue == "SDSR")
        {



            if (ddlLocation.SelectedIndex == 0)
            {


                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'    and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'  and inv_salesinvoiceheader.Post ='true'  and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'  and inv_salesinvoiceheader.Post ='false'  and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");


            }
            else
            {
                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'   and  set_locationmaster.location_id in (" + ddlLocation.SelectedValue + ") and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'  and inv_salesinvoiceheader.Post ='true' and  set_locationmaster.location_id in (" + ddlLocation.SelectedValue + ") and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'  and inv_salesinvoiceheader.Post ='false' and  set_locationmaster.location_id in (" + ddlLocation.SelectedValue + ") and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");

            }

            //dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.Post='true' and inv_salesinvoiceheader.IsActive='true' and  set_locationmaster.location_id in (" + ddlLocation.SelectedValue + ") and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code");
        }
        if (ddlReportType.SelectedValue == "SSOADBC")
        {
            if (txtCustomerNameSummery.Text.Trim() != "" && hdnCustomerId.Value.Trim() != "")
            {
                dt = ObjProductMaster.daClass.return_DataTable("select Ems_ContactMaster.Name as CustomerName, SalesOrderNo as OrderNo ,REPLACE(CONVERT(CHAR(11), SalesOrderDate, 106),' ','-') as OrderDate,cast(sum(Inv_SalesInvoiceDetail.OrderQty) as decimal(18,3)) as OrderQty, cast(sum(((Inv_SalesInvoiceDetail.UnitPrice + Inv_SalesInvoiceDetail.TaxV - Inv_SalesInvoiceDetail.DiscountV) * Inv_SalesDeliveryVoucher_Detail.Order_Qty)) as decimal(18,3)) as OrderAmount ,cast(sum(Inv_SalesDeliveryVoucher_Detail.Delievered_Qty)as decimal(18,3)) as DeliveredQty,cast(sum(((Inv_SalesInvoiceDetail.UnitPrice + Inv_SalesInvoiceDetail.TaxV - Inv_SalesInvoiceDetail.DiscountV) * Inv_SalesDeliveryVoucher_Detail.Delievered_Qty)) as decimal(18,3)) as DeliveredAmount ,cast(sum(Inv_SalesInvoiceDetail.Quantity)as decimal(18,3)) as InvoiceQty,cast(sum(((Inv_SalesInvoiceDetail.UnitPrice + Inv_SalesInvoiceDetail.TaxV - Inv_SalesInvoiceDetail.DiscountV) * Inv_SalesInvoiceDetail.Quantity)) as decimal(18,3)) as InvoiceAmount   from inv_salesorderheader inner join Ems_ContactMaster on Inv_SalesOrderHeader.CustomerId = Ems_ContactMaster.Trans_Id inner join Inv_SalesDeliveryVoucher_Header on Inv_SalesDeliveryVoucher_Header.SalesOrder_Id = Inv_SalesOrderHeader.Trans_Id inner join Inv_SalesDeliveryVoucher_Detail on Inv_SalesDeliveryVoucher_Detail.Voucher_No = Inv_SalesDeliveryVoucher_Header.Trans_Id inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderHeader.Trans_Id and Inv_SalesInvoiceDetail.SIFromTransType = 'S' and Inv_SalesDeliveryVoucher_Detail.Product_Id = Inv_SalesInvoiceDetail.Product_Id inner join Inv_SalesInvoiceheader on Inv_SalesInvoiceheader.trans_id = Inv_SalesInvoiceDetail.invoice_no where Inv_SalesOrderHeader.IsActive = 'True' and Inv_SalesInvoiceheader.isactive = 'true'  and Inv_SalesOrderHeader.Location_Id in (" + ddlLocation.SelectedValue + ") and Inv_SalesOrderHeader.CustomerId = '" + hdnCustomerId.Value + "' and cast(SalesOrderDate as date) >= '" + txtFromDate.Text + "' and cast(SalesOrderDate as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Name, SalesOrderNo ,SalesOrderDate order by SalesOrderDate desc");
            }
            else
            {
                dt = ObjProductMaster.daClass.return_DataTable("select Ems_ContactMaster.Name as CustomerName, SalesOrderNo as OrderNo ,REPLACE(CONVERT(CHAR(11), SalesOrderDate, 106),' ','-') as OrderDate,cast(sum(Inv_SalesInvoiceDetail.OrderQty) as decimal(18,3)) as OrderQty, cast(sum(((Inv_SalesInvoiceDetail.UnitPrice + Inv_SalesInvoiceDetail.TaxV - Inv_SalesInvoiceDetail.DiscountV) * Inv_SalesDeliveryVoucher_Detail.Order_Qty)) as decimal(18,3)) as OrderAmount ,cast(sum(Inv_SalesDeliveryVoucher_Detail.Delievered_Qty)as decimal(18,3)) as DeliveredQty,cast(sum(((Inv_SalesInvoiceDetail.UnitPrice + Inv_SalesInvoiceDetail.TaxV - Inv_SalesInvoiceDetail.DiscountV) * Inv_SalesDeliveryVoucher_Detail.Delievered_Qty)) as decimal(18,3)) as DeliveredAmount ,cast(sum(Inv_SalesInvoiceDetail.Quantity)as decimal(18,3)) as InvoiceQty,cast(sum(((Inv_SalesInvoiceDetail.UnitPrice + Inv_SalesInvoiceDetail.TaxV - Inv_SalesInvoiceDetail.DiscountV) * Inv_SalesInvoiceDetail.Quantity)) as decimal(18,3)) as InvoiceAmount  from inv_salesorderheader inner join Ems_ContactMaster on Inv_SalesOrderHeader.CustomerId = Ems_ContactMaster.Trans_Id inner join Inv_SalesDeliveryVoucher_Header on Inv_SalesDeliveryVoucher_Header.SalesOrder_Id = Inv_SalesOrderHeader.Trans_Id inner join Inv_SalesDeliveryVoucher_Detail on Inv_SalesDeliveryVoucher_Detail.Voucher_No = Inv_SalesDeliveryVoucher_Header.Trans_Id inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderHeader.Trans_Id and Inv_SalesInvoiceDetail.SIFromTransType = 'S' and Inv_SalesDeliveryVoucher_Detail.Product_Id = Inv_SalesInvoiceDetail.Product_Id inner join Inv_SalesInvoiceheader on Inv_SalesInvoiceheader.trans_id = Inv_SalesInvoiceDetail.invoice_no where Inv_SalesOrderHeader.IsActive = 'True' and Inv_SalesInvoiceheader.isactive = 'true'  and Inv_SalesOrderHeader.Location_Id in (" + ddlLocation.SelectedValue + ") and cast(SalesOrderDate as date) >= '" + txtFromDate.Text + "' and cast(SalesOrderDate as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Name, SalesOrderNo ,SalesOrderDate order by SalesOrderDate desc");
            }
        }
        if (ddlReportType.SelectedValue == "SSRBE")
        {
            if (ddlLocation.SelectedIndex == 0)
            {
                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true'  and set_employeemaster.isactive = 'true'                                              and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true' and inv_salesinvoiceheader.post = 'true'  and set_employeemaster.isactive = 'true'    and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true' and inv_salesinvoiceheader.post = 'false' and set_employeemaster.isactive = 'true'    and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");
            }
            else
            {
                if (rBtnTypeAll.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true'  and set_employeemaster.isactive = 'true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");
                else if (rBtnTypePosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true' and inv_salesinvoiceheader.post = 'true' and set_employeemaster.isactive = 'true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");
                else if (rBtnTypeUnPosted.Checked)
                    dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true' and inv_salesinvoiceheader.post = 'false' and set_employeemaster.isactive = 'true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");

            }
        }

        if (ddlReportType.SelectedValue == "SRBB")
        {
            dt = ObjProductMaster.daClass.return_DataTable("SELECT set_locationmaster.location_name AS LocationName, Inv_ProductBrandMaster.Brand_Name AS BrandName, SUM(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity, SUM(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue, CAST((SUM(inv_salesinvoicedetail.unitprice) + SUM(inv_salesinvoicedetail.TaxV) - SUM(inv_salesinvoicedetail.discountv)) * SUM(inv_salesinvoicedetail.Quantity) AS decimal(18, 3)) AS LineTotal, sys_currencymaster.currency_code as currency FROM inv_salesinvoicedetail INNER JOIN inv_salesinvoiceheader ON inv_salesinvoiceheader.Trans_Id = inv_salesinvoicedetail.Invoice_No INNER JOIN Inv_Product_Brand ON Inv_Product_Brand.productid = inv_salesinvoicedetail.product_id INNER JOIN Inv_ProductBrandMaster ON Inv_ProductBrandMaster.pbrandid = Inv_Product_Brand.pbrandid INNER JOIN set_locationmaster ON set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 WHERE inv_salesinvoicedetail.isactive = 'true' AND inv_salesinvoiceheader.Post = 'true' AND inv_salesinvoiceheader.IsActive = 'true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' GROUP BY set_locationmaster.location_name, Inv_ProductBrandMaster.Brand_Name,sys_currencymaster.currency_code");
        }

        if (ddlReportType.SelectedValue == "SICR")
        {
            bool IsPost = false;
            if (rBtnTypePosted.Checked == true)
            {
                IsPost = true;
                if (ddlLocation.SelectedIndex == 0)
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select Invoice_No,Invoice_Date,(Select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id )as SalesPerson ,(Case when Post='1' then 'True' else 'False' End)as IsPost,(Select Name From Ems_ContactMaster where Trans_Id= Inv_SalesInvoiceHeader.Supplier_Id) As Customer,Invoice_Ref_No,(GrandTotal)as InvoiceAmount,CancelRemark,Case When CancelBy='superadmin' then '' else (Select Emp_name from Set_EmployeeMaster where Emp_Code=CancelBy) end as Cancel_By from Inv_SalesInvoiceHeader where  IsCancel='1' And IsActive='1' And Post='" + IsPost + "' And  cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "'");
                    //dt = ObjProductMaster.daClass.return_DataTable("Select Invoice_No,Invoice_Date,(Select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id )as SalesPerson ,(Case when Post='1' then 'True' else 'False' End)as IsPost,(Select Name From Ems_ContactMaster where Trans_Id= Inv_SalesInvoiceHeader.Supplier_Id) As Customer,Invoice_Ref_No,(GrandTotal)as InvoiceAmount,CancelRemark,(Field4) as Is_Approved from Inv_SalesInvoiceHeader where  IsCancel='1' And IsActive='1' And Post='" + IsPost + "' And Location_Id In ('" + ddlLocation.SelectedValue.ToString() + "') And cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "'");

                }
                else
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select Invoice_No,Invoice_Date,(Select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id )as SalesPerson ,(Case when Post='1' then 'True' else 'False' End)as IsPost,(Select Name From Ems_ContactMaster where Trans_Id= Inv_SalesInvoiceHeader.Supplier_Id) As Customer,Invoice_Ref_No,(GrandTotal)as InvoiceAmount,CancelRemark,Case When CancelBy='superadmin' then '' else (Select Emp_name from Set_EmployeeMaster where Emp_Code=CancelBy) end as Cancel_By from Inv_SalesInvoiceHeader where  IsCancel='1' And IsActive='1' And Post='" + IsPost + "' And Location_Id In ('" + ddlLocation.SelectedValue.ToString() + "') And cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "'");

                }

            }
            else if (rBtnTypeUnPosted.Checked == true)
            {
                if (ddlLocation.SelectedIndex == 0)
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select Invoice_No,Invoice_Date,(Select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id )as SalesPerson ,(Case when Post='1' then 'True' else 'False' End)as IsPost,(Select Name From Ems_ContactMaster where Trans_Id= Inv_SalesInvoiceHeader.Supplier_Id) As Customer,Invoice_Ref_No,(GrandTotal)as InvoiceAmount,CancelRemark,Case When CancelBy='superadmin' then '' else (Select Emp_name from Set_EmployeeMaster where Emp_Code=CancelBy) end as Cancel_By from Inv_SalesInvoiceHeader where  IsCancel='1' And IsActive='1' And  Post='" + IsPost + "'  And cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "'");
                }
                else
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select Invoice_No,Invoice_Date,(Select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id )as SalesPerson ,(Case when Post='1' then 'True' else 'False' End)as IsPost,(Select Name From Ems_ContactMaster where Trans_Id= Inv_SalesInvoiceHeader.Supplier_Id) As Customer,Invoice_Ref_No,(GrandTotal)as InvoiceAmount,CancelRemark,Case When CancelBy='superadmin' then '' else (Select Emp_name from Set_EmployeeMaster where Emp_Code=CancelBy) end as Cancel_By from Inv_SalesInvoiceHeader where  IsCancel='1' And IsActive='1' And  Post='" + IsPost + "'  And Location_Id In ('" + ddlLocation.SelectedValue.ToString() + "') And cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "'");
                }
            }
            else if (rBtnTypeAll.Checked == true)
            {
                if (ddlLocation.SelectedIndex == 0)
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select Invoice_No,Invoice_Date,(Select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id )as SalesPerson ,(Case when Post='1' then 'True' else 'False' End)as IsPost,(Select Name From Ems_ContactMaster where Trans_Id= Inv_SalesInvoiceHeader.Supplier_Id) As Customer,Invoice_Ref_No,(GrandTotal)as InvoiceAmount,CancelRemark,Case When CancelBy='superadmin' then '' else (Select Emp_name from Set_EmployeeMaster where Emp_Code=CancelBy) end as Cancel_By from Inv_SalesInvoiceHeader where  IsCancel='1' And IsActive='1' And cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "'");

                }
                else
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select Invoice_No,Invoice_Date,(Select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id )as SalesPerson ,(Case when Post='1' then 'True' else 'False' End)as IsPost,(Select Name From Ems_ContactMaster where Trans_Id= Inv_SalesInvoiceHeader.Supplier_Id) As Customer,Invoice_Ref_No,(GrandTotal)as InvoiceAmount,CancelRemark,Case When CancelBy='superadmin' then '' else (Select Emp_name from Set_EmployeeMaster where Emp_Code=CancelBy) end as Cancel_By from Inv_SalesInvoiceHeader where  IsCancel='1' And IsActive='1' And Location_Id In( '" + ddlLocation.SelectedValue.ToString() + "') And cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "'");
                }

            }
            else
            {
                if (ddlLocation.SelectedIndex == 0)
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select Invoice_No,Invoice_Date,(Select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id )as SalesPerson ,(Case when Post='1' then 'True' else 'False' End)as IsPost,(Select Name From Ems_ContactMaster where Trans_Id= Inv_SalesInvoiceHeader.Supplier_Id) As Customer,Invoice_Ref_No,(GrandTotal)as InvoiceAmount,CancelRemark,Case When CancelBy='superadmin' then '' else (Select Emp_name from Set_EmployeeMaster where Emp_Code=CancelBy) end as Cancel_By from Inv_SalesInvoiceHeader where  IsCancel='1' And IsActive='1'  And cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "'");

                }
                else
                {
                    dt = ObjProductMaster.daClass.return_DataTable("Select Invoice_No,Invoice_Date,(Select Emp_Name from Set_EmployeeMaster where Emp_Id=Inv_SalesInvoiceHeader.SalesPerson_Id )as SalesPerson ,(Case when Post='1' then 'True' else 'False' End)as IsPost,(Select Name From Ems_ContactMaster where Trans_Id= Inv_SalesInvoiceHeader.Supplier_Id) As Customer,Invoice_Ref_No,(GrandTotal)as InvoiceAmount,CancelRemark,Case When CancelBy='superadmin' then '' else (Select Emp_name from Set_EmployeeMaster where Emp_Code=CancelBy) end as Cancel_By from Inv_SalesInvoiceHeader where  IsCancel='1' And IsActive='1' And Location_Id In ('" + ddlLocation.SelectedValue.ToString() + "') And cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "'");
                }
            }

        }

        if (dt.Rows.Count > 0)
        {
            Session["gvExportData"] = dt;
            gvExportData.DataSource = dt;
            gvExportData.AutoGenerateColumns = true;
            gvExportData.DataBind();
        }
        else
        {
            DisplayMessage("No Data Found According to Given Criteria");
        }

        try
        {
            lblTotalRecords.Text = "Total Records : " + dt.Rows.Count;
        }
        catch
        {
            lblTotalRecords.Text = "Total Records : " + 0;
        }
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

    public void fillLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 1)
                {
                    ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
                }
            }
            else
            {
                ddlLocation.Items.Insert(0, new ListItem("All", Session["LocId"].ToString()));
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            //if (dtLoc.Rows.Count > 1 && LocIds != "")
            //{
            //    ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
            //}
            ddlLocation.Items.Insert(0, new ListItem("All", "0"));
        }
        else
        {
            ddlLocation.Items.Clear();
        }

        dtLoc = null;
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlReportType.SelectedValue == "SSOADBC")
        {
            DivCustomerName.Visible = true;
        }
        else
        {
            DivCustomerName.Visible = false;
        }

        if (ddlReportType.SelectedValue == "IRPR")
        {
            CustomerName.Visible = true;
            SalesPerson.Visible = true;

            txtCustomer.Text = "";
            txtSummarySalesPerson.Text = "";

            rBtnTypeAll.Visible = false;
            rBtnTypePosted.Visible = false;
            rBtnTypeUnPosted.Visible = false;
            trProduct.Visible = false;
            txtProductName.Visible = false;
            lblProductName.Visible = false;
        }
        else if (ddlReportType.SelectedValue == "PER")
        {
            CustomerName.Visible = false;
            SalesPerson.Visible = false;

            rBtnTypeAll.Visible = false;
            rBtnTypePosted.Visible = false;
            rBtnTypeUnPosted.Visible = false;
            trProduct.Visible = true;
            txtProductName.Visible = true;
            lblProductName.Visible = true;
        }
        else
        {
            CustomerName.Visible = false;
            SalesPerson.Visible = false;

            rBtnTypeAll.Visible = true;
            rBtnTypePosted.Visible = true;
            rBtnTypeUnPosted.Visible = true;
            trProduct.Visible = false;
            txtProductName.Visible = false;
            lblProductName.Visible = false;
        }
    }
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {

    }



    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            if (ddlReportType.SelectedValue == "SPWSR")
            {
                //dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
                if (ddlLocation.SelectedIndex == 0)
                {

                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true'  and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='false'   and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");

                }
                else
                {
                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoicedetail.Post='false' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");

                }


                Session["LocationProductSales"] = dt;
                Response.Redirect("LocationProductSales.aspx?id=1");




            }
            else if (ddlReportType.SelectedValue == "SPWSRC")
            {
                //dt = ObjProductMaster.daClass.return_DataTable("SELECT  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code");
                if (ddlLocation.SelectedIndex == 0)
                {


                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1  left   join   Ems_ContactMaster on Ems_ContactMaster.Trans_Id =  Inv_SalesInvoiceHeader.Supplier_Id  where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1  left   join   Ems_ContactMaster on Ems_ContactMaster.Trans_Id =  Inv_SalesInvoiceHeader.Supplier_Id  where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName, inv_productmaster.LProductName as ProductNameL, sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1  left   join   Ems_ContactMaster on Ems_ContactMaster.Trans_Id =  Inv_SalesInvoiceHeader.Supplier_Id  where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");




                }
                else
                {

                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1  left   join   Ems_ContactMaster on Ems_ContactMaster.Trans_Id =  Inv_SalesInvoiceHeader.Supplier_Id  where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1  left   join   Ems_ContactMaster on Ems_ContactMaster.Trans_Id =  Inv_SalesInvoiceHeader.Supplier_Id  where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("SELECT Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,  set_locationmaster.location_name AS LocationName,  inv_productmaster.ProductCode,  inv_productmaster.eproductname AS ProductName,inv_productmaster.LProductName as ProductNameL,  sum(CAST(inv_salesinvoicedetail.quantity AS decimal(10, 3))) AS Quantity,   sum(CAST(inv_salesinvoicedetail.DiscountV AS decimal(10, 3))) AS DiscountValue,  CAST(SUM((inv_salesinvoicedetail.unitprice + inv_salesinvoicedetail.TaxV -inv_salesinvoicedetail.discountv)* (inv_salesinvoicedetail.Quantity)) AS decimal(10, 3)) AS LineTotal,sys_currencymaster.currency_Code as Currency FROM inv_salesinvoicedetail left join inv_productmaster on inv_productmaster.productid=inv_salesinvoicedetail.product_id inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1  left   join   Ems_ContactMaster on Ems_ContactMaster.Trans_Id =  Inv_SalesInvoiceHeader.Supplier_Id  where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by  Ems_ContactMaster.Code ,Ems_ContactMaster.Name ,set_locationmaster.location_name, inv_productmaster.ProductCode,inv_productmaster.eproductname,sys_currencymaster.currency_Code,inv_productmaster.LProductName");




                }

                Session["LocationProductSales"] = dt;
                Response.Redirect("LocationProductSales.aspx?id=2");


            }
            else if (ddlReportType.SelectedValue == "SCWSR")
            {
                if (ddlLocation.SelectedIndex == 0)
                {


                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3)) as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and  cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3)) as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3)) as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='false' and inv_salesinvoiceheader.IsActive='true'  and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");


                }
                else
                {
                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3)) as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true'  and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3)) as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='true' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, Inv_Product_CategoryMaster.category_Name as CategoryName,sum(cast(inv_salesinvoicedetail.quantity as decimal(10,3))) as Quantity,sum(cast(inv_salesinvoicedetail.taxv as decimal(10,3))) as TaxValue,sum(cast(inv_salesinvoicedetail.DiscountV as decimal(10,3))) as DiscountValue, cast((sum((inv_salesinvoicedetail.unitprice  + inv_salesinvoicedetail.TaxV - inv_salesinvoicedetail.discountv)*(inv_salesinvoicedetail.Quantity))) as decimal(18,3)) as LineTotal ,sys_currencymaster.currency_Code as Currency from inv_salesinvoicedetail inner join inv_salesinvoiceheader on inv_salesinvoiceheader.Trans_Id= inv_salesinvoicedetail.Invoice_No inner join Inv_Product_Category on Inv_Product_Category.productid = inv_salesinvoicedetail.product_id inner join Inv_Product_CategoryMaster on Inv_Product_CategoryMaster.category_id = Inv_Product_Category.categoryId  inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoicedetail.isactive='true' and inv_salesinvoicedetail.Post='false' and inv_salesinvoiceheader.IsActive='true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) >= '" + txtFromDate.Text + "' and cast(Inv_SalesInvoiceHeader.Invoice_Date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name,Inv_Product_CategoryMaster.category_Name,sys_currencymaster.currency_Code");

                }


                Session["LocationProductSales"] = dt;
                Response.Redirect("LocationProductSales.aspx?id=3");
            }
            else if (ddlReportType.SelectedValue == "SDSR")
            {
                if (ddlLocation.SelectedIndex == 0)
                {


                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'    and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'  and inv_salesinvoiceheader.Post ='true'  and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'  and inv_salesinvoiceheader.Post ='false'  and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");


                }
                else
                {
                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'   and  set_locationmaster.location_id in (" + ddlLocation.SelectedValue + ") and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'  and inv_salesinvoiceheader.Post ='true' and  set_locationmaster.location_id in (" + ddlLocation.SelectedValue + ") and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select cast(inv_salesinvoiceheader.Invoice_Date as date) as inv_date,set_locationmaster.location_name as LocationName,Set_Payment_Mode_Master.Pay_Mod_Name, cast(sum(inv_salesinvoiceheader.grandtotal )as decimal(18,3)) as GrandTotal,sys_currencymaster.Currency_Code as Currency from inv_salesinvoiceheader inner join Inv_PaymentTrn on Inv_PaymentTrn.TransNo = inv_salesinvoiceheader.trans_id and Inv_PaymentTrn.TypeTrans='SI' inner join Set_Payment_Mode_Master on Set_Payment_Mode_Master.Pay_Mode_Id=Inv_PaymentTrn.PaymentModeId inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id  left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.IsActive='true'  and inv_salesinvoiceheader.Post ='false' and  set_locationmaster.location_id in (" + ddlLocation.SelectedValue + ") and cast(inv_salesinvoiceheader.invoice_date as date) >= '" + txtFromDate.Text + "' and cast(inv_salesinvoiceheader.invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, Set_Payment_Mode_Master.Pay_Mod_Name,sys_currencymaster.Currency_Code,cast(inv_salesinvoiceheader.Invoice_Date as date) order by Location_Name ,cast(inv_salesinvoiceheader.Invoice_Date as date)");

                }

                Session["LocationProductSales"] = dt;
                Response.Redirect("LocationProductSales.aspx?id=4");
            }
            else if (ddlReportType.SelectedValue == "SSRBE")
            {
                if (ddlLocation.SelectedIndex == 0)
                {
                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true'  and set_employeemaster.isactive = 'true'                                              and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true' and inv_salesinvoiceheader.post = 'true'  and set_employeemaster.isactive = 'true'    and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true' and inv_salesinvoiceheader.post = 'false' and set_employeemaster.isactive = 'true'    and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");


                }
                else
                {
                    if (rBtnTypeAll.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true'  and set_employeemaster.isactive = 'true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");
                    else if (rBtnTypePosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true' and inv_salesinvoiceheader.post = 'true' and set_employeemaster.isactive = 'true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");
                    else if (rBtnTypeUnPosted.Checked)
                        dt = ObjProductMaster.daClass.return_DataTable("select set_locationmaster.location_name as LocationName, set_employeemaster.emp_name as EmployeeName,cast(sum(inv_salesinvoiceheader.grandtotal) as decimal(18,3)) as TotalAmt,sys_currencymaster.currency_Code as Currency from inv_salesinvoiceheader inner join set_employeemaster on set_employeemaster.emp_id = inv_salesinvoiceheader.salesperson_id inner join set_locationmaster on set_locationmaster.location_id = inv_salesinvoiceheader.location_id left join sys_currencymaster on sys_currencymaster.currency_id = set_locationmaster.field1 where inv_salesinvoiceheader.isactive = 'true' and inv_salesinvoiceheader.post = 'false' and set_employeemaster.isactive = 'true' and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' group by set_locationmaster.location_name, set_employeemaster.emp_name,sys_currencymaster.currency_Code");

                }

                Session["LocationProductSales"] = dt;
                Response.Redirect("LocationProductSales.aspx?id=5");
            }
            else if (ddlReportType.SelectedValue == "CER")
            {

                DataTable dtSales = new DataTable();
                DataTable dtRetrun = new DataTable();

                if (ddlLocation.SelectedIndex == 0)
                {
                    if (rBtnTypeAll.Checked)
                        dtSales = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal)as TotalSales, Supplier_Id as Customer_Id ,Ems_ContactMaster.Code ,Ems_ContactMaster.Name  From Inv_SalesInvoiceHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesInvoiceHeader.Supplier_Id   Where 1=1     and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' and Inv_SalesInvoiceHeader.IsActive ='1' Group By Supplier_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name ");
                    else if (rBtnTypePosted.Checked)
                        dtSales = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal)as TotalSales, Supplier_Id as Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name   From Inv_SalesInvoiceHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesInvoiceHeader.Supplier_Id   Where 1=1  and inv_salesinvoiceheader.post = 'true'   and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' and Inv_SalesInvoiceHeader.IsActive ='1' Group By Supplier_Id ,Ems_ContactMaster.Code ,Ems_ContactMaster.Name");
                    else if (rBtnTypeUnPosted.Checked)
                        dtSales = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal)as TotalSales, Supplier_Id as Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name   From Inv_SalesInvoiceHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesInvoiceHeader.Supplier_Id   Where 1=1  and inv_salesinvoiceheader.post = 'false'   and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' and Inv_SalesInvoiceHeader.IsActive ='1' Group By Supplier_Id ,Ems_ContactMaster.Code ,Ems_ContactMaster.Name");


                    //Select Sum(GrandTotal) as TotalReturn, Customer_Id  From Inv_SalesReturnHeader Where 1=1 and    cast(Return_Date as date) >= '" + txtFromDate.Text + "' and cast(Return_Date as date) <= '" + txtToDate.Text + "' and Inv_SalesReturnHeader.IsActive ='1' Group By Customer_Id 

                    if (rBtnTypeAll.Checked)
                        dtRetrun = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal) as TotalReturn, Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name  From Inv_SalesReturnHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesReturnHeader.Customer_Id Where 1=1 and    cast(Return_Date as date) >= '" + txtFromDate.Text + "' and cast(Return_Date as date) <= '" + txtToDate.Text + "' and Inv_SalesReturnHeader.IsActive ='1' Group By Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name ");
                    else if (rBtnTypePosted.Checked)
                        dtRetrun = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal) as TotalReturn, Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name  From Inv_SalesReturnHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesReturnHeader.Customer_Id Where 1=1  and Inv_SalesReturnHeader.post = 'true' and    cast(Return_Date as date) >= '" + txtFromDate.Text + "' and cast(Return_Date as date) <= '" + txtToDate.Text + "' and Inv_SalesReturnHeader.IsActive ='1' Group By Customer_Id ,Ems_ContactMaster.Code ,Ems_ContactMaster.Name");
                    else if (rBtnTypeUnPosted.Checked)
                        dtRetrun = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal) as TotalReturn, Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name  From Inv_SalesReturnHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesReturnHeader.Customer_Id    Where 1=1  and Inv_SalesReturnHeader.post = 'false' and    cast(Return_Date as date) >= '" + txtFromDate.Text + "' and cast(Return_Date as date) <= '" + txtToDate.Text + "' and Inv_SalesReturnHeader.IsActive ='1' Group By Customer_Id ,Ems_ContactMaster.Code ,Ems_ContactMaster.Name");

                }
                else
                {

                    if (rBtnTypeAll.Checked)
                        dtSales = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal)as TotalSales, Supplier_Id as Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name   From Inv_SalesInvoiceHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesInvoiceHeader.Supplier_Id   Where 1=1   and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ")   and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' and Inv_SalesInvoiceHeader.IsActive ='1' Group By Supplier_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name ");
                    else if (rBtnTypePosted.Checked)
                        dtSales = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal)as TotalSales, Supplier_Id as Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name   From Inv_SalesInvoiceHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesInvoiceHeader.Supplier_Id   Where 1=1  and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and inv_salesinvoiceheader.post = 'true'   and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' and Inv_SalesInvoiceHeader.IsActive ='1' Group By Supplier_Id ,Ems_ContactMaster.Code ,Ems_ContactMaster.Name");
                    else if (rBtnTypeUnPosted.Checked)
                        dtSales = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal)as TotalSales, Supplier_Id as ,Ems_ContactMaster.Code ,Ems_ContactMaster.Name   From Inv_SalesInvoiceHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesInvoiceHeader.Supplier_Id    Where 1=1  and inv_salesinvoiceheader.location_id in (" + ddlLocation.SelectedValue + ") and inv_salesinvoiceheader.post = 'false'   and cast(invoice_date as date) >= '" + txtFromDate.Text + "' and cast(invoice_date as date) <= '" + txtToDate.Text + "' and Inv_SalesInvoiceHeader.IsActive ='1' Group By Supplier_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name ");


                    if (rBtnTypeAll.Checked)
                        dtRetrun = ObjProductMaster.daClass.return_DataTable("Select  Sum(GrandTotal) as TotalReturn, Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name  From Inv_SalesReturnHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesReturnHeader.Customer_Id Where 1=1  and Inv_SalesReturnHeader.location_id in (" + ddlLocation.SelectedValue + ") and    cast(Return_Date as date) >= '" + txtFromDate.Text + "' and cast(Return_Date as date) <= '" + txtToDate.Text + "' and Inv_SalesReturnHeader.IsActive ='1' Group By Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name ");
                    else if (rBtnTypePosted.Checked)
                        dtRetrun = ObjProductMaster.daClass.return_DataTable("Select Sum(GrandTotal) as TotalReturn, Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.NameFrom Inv_SalesReturnHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesReturnHeader.Customer_Id Where 1=1  and Inv_SalesReturnHeader.post = 'true'  and Inv_SalesReturnHeader.location_id in (" + ddlLocation.SelectedValue + ") and    cast(Return_Date as date) >= '" + txtFromDate.Text + "' and cast(Return_Date as date) <= '" + txtToDate.Text + "' and Inv_SalesReturnHeader.IsActive ='1' Group By Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name ");
                    else if (rBtnTypeUnPosted.Checked)
                        dtRetrun = ObjProductMaster.daClass.return_DataTable("Select  Sum(GrandTotal) as TotalReturn, Customer_Id ,Ems_ContactMaster.Code ,Ems_ContactMaster.Name From Inv_SalesReturnHeader INNER JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesReturnHeader.Customer_Id Where 1=1  and Inv_SalesReturnHeader.post = 'false'  and Inv_SalesReturnHeader.location_id in (" + ddlLocation.SelectedValue + ") and    cast(Return_Date as date) >= '" + txtFromDate.Text + "' and cast(Return_Date as date) <= '" + txtToDate.Text + "' and Inv_SalesReturnHeader.IsActive ='1' Group By Customer_Id,Ems_ContactMaster.Code ,Ems_ContactMaster.Name ");

                }

                DataTable dtCER = new DataTable();
                dtCER.Columns.Add(new DataColumn("Code"));
                dtCER.Columns.Add(new DataColumn("Name"));
                dtCER.Columns.Add(new DataColumn("TotalSales"));
                dtCER.Columns.Add(new DataColumn("TotalReturn"));
                dtCER.Columns.Add(new DataColumn("Customer_Id"));

                for (int i = 0; i < dtSales.Rows.Count; i++)
                {
                    DataRow row = dtCER.NewRow();
                    row[0] = dtSales.Rows[i][2].ToString();
                    row[1] = dtSales.Rows[i][3].ToString();
                    row[2] = Convert.ToDecimal(dtSales.Rows[i][0]).ToString("0.000");

                    try
                    {
                        DataTable dtTemp = new DataView(dtRetrun, "Customer_Id =  '" + dtSales.Rows[i][1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                        if (dtTemp.Rows.Count > 0)
                        {
                            row[3] = Convert.ToDecimal(dtTemp.Rows[0][0]).ToString("0.000");

                        }
                        else
                        {
                            row[3] = "0.000";
                        }
                    }
                    catch
                    {
                        row[3] = "0.000";
                    }

                    row[4] = dtSales.Rows[i][1].ToString();
                    dtCER.Rows.Add(row);
                    dtCER.AcceptChanges();
                }
                for (int i = 0; i < dtRetrun.Rows.Count; i++)
                {

                    DataTable dtTemp = new DataView(dtCER, "Customer_Id =  '" + dtRetrun.Rows[i][1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();



                    if (dtTemp.Rows.Count == 0)
                    {
                        DataRow row = dtCER.NewRow();
                        row[0] = dtRetrun.Rows[i][2].ToString();
                        row[1] = dtRetrun.Rows[i][3].ToString();
                        row[2] = "0.000";
                        row[3] = Convert.ToDecimal(dtRetrun.Rows[i][0]).ToString("0.000");
                        row[4] = dtRetrun.Rows[i][1].ToString();
                        dtCER.Rows.Add(row);
                        dtCER.AcceptChanges();
                    }

                }



                Session["LocationProductSales"] = dtCER;
                Response.Redirect("LocationProductSales.aspx?id=6");
            }
            else if (ddlReportType.SelectedValue == "PSR")
            {

                DataTable dtResult = new DataTable();

                if (ddlLocation.SelectedIndex == 0)
                {
                    for (int i = 1; i < ddlLocation.Items.Count; i++)
                    {
                        DataTable dtTemp = new DataTable();
                        dtTemp = da.return_DataTable("Select * From dbo.fn_Product_Balance(" + ddlLocation.Items[i].Value.ToString() + ",'" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "')");
                        if (dtResult.Rows.Count > 0)
                        {
                            dtResult.Merge(dtTemp);
                        }
                        else
                        {
                            dtResult = dtTemp.Copy();
                        }
                        dtResult.AcceptChanges();
                    }
                }
                else
                {
                    dtResult = da.return_DataTable("Select * From dbo.fn_Product_Balance(" + ddlLocation.SelectedValue.ToString() + ",'" + Convert.ToDateTime(txtFromDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtToDate.Text).ToString("yyyy-MM-dd") + "')");
                }

                Session["LocationProductSales"] = dtResult;
                Response.Redirect("LocationProductSales.aspx?id=7");
            }
            else
            {


                ASPxGridViewExporter1.PaperKind = System.Drawing.Printing.PaperKind.A3;
                ASPxGridViewExporter1.Landscape = true;
                ASPxGridViewExporter1.WritePdfToResponse();
            }
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
