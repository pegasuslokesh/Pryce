using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class Sales_Report_SalesInquiry : BasePage
{
    HR_EmployeeDetail HR_EmployeeDetail = null;
    Common cmn = null;
    Inv_ProductMaster ObjProductMaster = null;
    LocationMaster objLocation = null;
    SystemParameter ObjSysParam = null;
    PurchaseRequestHeader objPRequestHeader = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_SalesInquiryHeader objSInquiryHeader = null;
    Inv_PurchaseInquiry_Supplier ObjPISupplier = null;
    DataAccessClass objDa = null;
    EmployeeMaster objEmployee = null;
    Inv_ProductBrandMaster ObjProductBrandMaster = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    string StrUserId = string.Empty;
    UserMaster objUser = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objPRequestHeader = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objSInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        ObjPISupplier = new Inv_PurchaseInquiry_Supplier(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjProductBrandMaster = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());

        StrUserId = Session["UserId"].ToString();
        if (!IsPostBack)
        {
            Session["ReportHeader"] = null;
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["DtFilter"] = null;
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtFromDate.Focus();
            txtFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            FillLocation();
            FillddlBrandSearch();
            FillProductCategorySerch();
        }
        AllPageCode();

    }
    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("190", (DataTable)Session["ModuleName"]);
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
        panelFollowupReport.Visible = false;
        if (rbtnheader.Checked == true || rbtnFollowupReport.Checked == true)
        {
            txtFromDate.Focus();
            txtProductName.Visible = false;
            lblProductName.Visible = false;
            //lblcolon.Visible = false;
        }


        if (rbtnFollowupReport.Checked)
        {
            panelFollowupReport.Visible = true;
        }


    }
    protected void RbtnDetail_CheckedChanged(object sender, EventArgs e)
    {
        panelFollowupReport.Visible = false;
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

    public void FillddlBrandSearch()
    {
        ddlbrandsearch.Items.Clear();

        ListItem Li = new ListItem();

        Li.Text = "--Select--";
        Li.Value = "0";

        DataTable dt = ObjProductBrandMaster.GetProductBrandTrueAllData(Session["CompId"].ToString());
        try
        {

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            ddlbrandsearch.DataSource = dt;
            ddlbrandsearch.DataTextField = "Brand_Name";
            ddlbrandsearch.DataValueField = "PBrandId";
            ddlbrandsearch.DataBind();

            ddlbrandsearch.Items.Insert(0, Li);
        }
        catch
        {
            ddlbrandsearch.Items.Insert(0, Li);
        }
    }

    private void FillProductCategorySerch()
    {

        ddlcategorysearch.Items.Clear();

        ListItem Li = new ListItem();

        Li.Text = "--Select--";
        Li.Value = "0";
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());
        if (dsCategory.Rows.Count > 0)
        {

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            ddlcategorysearch.DataSource = dsCategory;
            ddlcategorysearch.DataTextField = "Category_Name";
            ddlcategorysearch.DataValueField = "Category_Id";
            ddlcategorysearch.DataBind();
            ddlcategorysearch.Items.Insert(0, Li);

        }
        else
        {
            ddlcategorysearch.Items.Insert(0, Li);
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {

        Session["ReportHeader"] = null;
        Session["Parameter"] = null;
        Session["DtFilter"] = null;
        if (rbtnheader.Checked == false && RbtnDetail.Checked == false && rbtnFollowupReport.Checked == false)
        {
            DisplayMessage("Select the Report Type(Header or Detail or Follow Up)");
            rbtnheader.Focus();
            return;
        }





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
        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }





        DataTable DtFilter = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;

        if (rbtnheader.Checked == true)
        {
            Session["ReportType"] = "0";

            SalesDataSetTableAdapters.sp_Inv_SalesInquiryHeader_SelectRowTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInquiryHeader_SelectRowTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjSalesDataset.sp_Inv_SalesInquiryHeader_SelectRow, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0, "", 0, 2);
            DtFilter = ObjSalesDataset.sp_Inv_SalesInquiryHeader_SelectRow;


            Session["ReportHeader"] = "Sales Inquiry Header Report By Inquiry Date";

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "IDate>='" + txtFromDate.Text + "' and IDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;


            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "IDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["IDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;
                txtFromDate.Text = "";
                txtToDate.Text = "";


            }


            //for sales perosn filter

            if (txtSalesPerson.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "HandledEmpID='" + HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString()) + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


                Session["ReportType"] = "2";
                Session["ReportHeader"] = "Sales Inquiry Header Report By Sales Person";

            }



            if (txtInquiryNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SInquiryNo='" + txtInquiryNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                //if (Session["Parameter"] == null)
                //{
                //    Session["Parameter"] = "FilterBy: InquiryNo. Wise";
                //}
                //else
                //{
                //    Session["Parameter"] = Session["Parameter"].ToString() + " , InquiryNo. Wise";
                //}

            }
            if (txtCustomerName.Text != "")
            {
                Session["ReportType"] = "1";
                try
                {
                    DtFilter = new DataView(DtFilter, "Customer_Id =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                //if (Session["Parameter"] == null)
                //{
                //    Session["Parameter"] = "FilterBy: CustomerWise";
                //}
                //else
                //{
                //    Session["Parameter"] = Session["Parameter"].ToString() + " , CustomerWise";
                //}

            }

            if (txtTenderNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "TenderNo ='" + txtTenderNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                //if (Session["Parameter"] == null)
                //{
                //    Session["Parameter"] = "FilterBy: TenderNo.Wise";
                //}
                //else
                //{
                //    Session["Parameter"] = Session["Parameter"].ToString() + " , TenderNo.Wise";
                //}

            }
            if (ddlStatus.SelectedIndex == 1)
            {
                {
                    try
                    {
                        DtFilter = new DataView(DtFilter, "Status ='Direct'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                }
                //if (Session["Parameter"] == null)
                //{
                //    Session["Parameter"] = "FilterBy: StatusWise";
                //}
                //else
                //{
                //    Session["Parameter"] = Session["Parameter"].ToString() + " , StatusWise";
                //}
            }
            if (ddlStatus.SelectedIndex == 2)
            {
                {
                    try
                    {
                        DtFilter = new DataView(DtFilter, "Status <>'Direct'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                }
                //if (Session["Parameter"] == null)
                //{
                //    Session["Parameter"] = "FilterBy: StatusWise";
                //}
                //else
                //{
                //    Session["Parameter"] = Session["Parameter"].ToString() + " , StatusWise";
                //}


            }







            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SInquiryHeaderReport.aspx','window','width=1024');", true);

                // Response.Redirect("../Sales_Report/SInquiryHeaderReport.aspx");
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
            SalesDataSetTableAdapters.sp_Inv_SalesInqDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInqDetail_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjSalesDataset.sp_Inv_SalesInqDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjSalesDataset.sp_Inv_SalesInqDetail_SelectRow_Report;
            Session["ReportType"] = "0";

            Session["ReportHeader"] = "Sales Inquiry Detail Report By Inquiry Date";

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "IDate>='" + txtFromDate.Text + "' and IDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "FilterBy:From " + txtFromDate.Text + "  To " + txtToDate.Text;


            }
            if (txtInquiryNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SInquiryNo='" + txtInquiryNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: InquiryNo. Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , InquiryNo. Wise";
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

            if (txtTenderNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "TenderNo ='" + txtTenderNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: TenderNo.Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , TenderNo.Wise";
                }

            }
            if (ddlStatus.SelectedIndex == 1)
            {
                {
                    try
                    {
                        DtFilter = new DataView(DtFilter, "Status ='Direct'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: StatusWise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , StatusWise";
                }
            }
            if (ddlStatus.SelectedIndex == 2)
            {
                {
                    try
                    {
                        DtFilter = new DataView(DtFilter, "Status <>'Direct'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy: StatusWise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , StatusWise";
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
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SInquiryDetailReport.aspx','window','width=1024');", true);


                //Response.Redirect("../Sales_Report/SInquiryDetailReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");
                RbtnDetail.Focus();
                return;
            }
        }

        if (rbtnFollowupReport.Checked)
        {
            string Type = string.Empty;





            if (chkMonthlyReport.Checked)
            {
                DtFilter = GetMonthLyFollowupReport(strLocationId);
                Type = "M";
            }
            else
            {
                DtFilter = GetFollowupReport(strLocationId);
                Type = "N";
            }
            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SalesFollowupReport.aspx?Type=" + Type + "','window','width=1024');", true);
            }
            else
            {
                DisplayMessage("Record Not Found");
                rbtnheader.Focus();
                return;
            }


            //DtFilter = new DataTable();

            //DtFilter.Columns.Add("Sr_No", typeof(float));
            //DtFilter.Columns.Add("Sales_Person");
            //DtFilter.Columns.Add("Inquiry_Value");
            //DtFilter.Columns.Add("Inquiry_Amount");
            //DtFilter.Columns.Add("Quotation_Value");
            //DtFilter.Columns.Add("Quotation_Amount");
            //DtFilter.Columns.Add("Order_Value");
            //DtFilter.Columns.Add("Order_Amount");
            //DtFilter.Columns.Add("Invoice_Value");
            //DtFilter.Columns.Add("Invoice_Amount");
            //DtFilter.Columns.Add("Quotation_Lost_Amount");
            //DtFilter.Columns.Add("Order_Forecast_Amount");



            //string strsql = "select distinct Inv_SalesInquiryHeader.HandledEmpID,Set_EmployeeMaster.Emp_Name  from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  inner join Set_EmployeeMaster on  Inv_SalesInquiryHeader.HandledEmpID=Set_EmployeeMaster.Emp_Id  where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' group by Set_EmployeeMaster.Emp_Name ,Inv_SalesInquiryHeader.HandledEmpID";

            //DataTable dt = objDa.return_DataTable(strsql);

            //if (txtSalesPerson.Text != "")
            //{

            //    try
            //    {
            //        dt = new DataView(dt, "HandledEmpID=" + txtSalesPerson.Text.Split('/')[1].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            //    }
            //    catch
            //    {
            //        dt = new DataTable();
            //    }
            //}

            //float SNo = 0;

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{


            //    SNo++;

            //    DataRow dr = DtFilter.NewRow();

            //    dr[0] = SNo;
            //    dr[1] = dt.Rows[i]["Emp_Name"].ToString();


            //    //get sinquiry info for currenct sales person

            //    DataTable dtTemp = new DataTable();
            //    if (txtFromDate.Text != "" && txtToDate.Text != "")
            //    {
            //        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            //        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);


            //        dtTemp = objDa.return_DataTable("select COUNT( distinct   Inv_SalesInquiryHeader.SInquiryID) as InqNo,SUM(Inv_SalesInqDetail.EstimatedUnitPrice*Inv_SalesInqDetail.Quantity) as InqAmount  from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "'");


            //        Session["Parameter"] = "From : " + txtFromDate.Text + "  To : " + txtToDate.Text;


            //    }
            //    else
            //    {

            //        dtTemp = objDa.return_DataTable("select COUNT( distinct   Inv_SalesInquiryHeader.SInquiryID) as InqNo,SUM(Inv_SalesInqDetail.EstimatedUnitPrice*Inv_SalesInqDetail.Quantity) as InqAmount  from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + "");

            //        Session["Parameter"] = "";
            //    }
            //    dr[2] = dtTemp.Rows[0]["InqNo"].ToString();
            //    dr[3] = dtTemp.Rows[0]["InqAmount"].ToString();

            //    //for quotation Amount 
            //    if (txtFromDate.Text != "" && txtToDate.Text != "")
            //    {
            //        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            //        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);


            //        dtTemp = objDa.return_DataTable("select COUNT( distinct Inv_SalesQuotationHeader.SInquiry_No) as Quote_No ,SUM(Inv_SalesQuotationHeader.Amount-Inv_SalesQuotationHeader.DiscountValue+Inv_SalesQuotationHeader.TaxValue) as Quote_Amount from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.Status<>'Lost' and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "')");


            //        Session["Parameter"] = "From : " + txtFromDate.Text + "  To : " + txtToDate.Text;


            //    }
            //    else
            //    {

            //        dtTemp = objDa.return_DataTable("select COUNT( distinct Inv_SalesQuotationHeader.SInquiry_No) as Quote_No ,SUM(Inv_SalesQuotationHeader.Amount-Inv_SalesQuotationHeader.DiscountValue+Inv_SalesQuotationHeader.TaxValue) as Quote_Amount from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.Status<>'Lost' and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + ")");

            //        Session["Parameter"] = "";
            //    }
            //    dr[4] = dtTemp.Rows[0]["Quote_No"].ToString();
            //    dr[5] = dtTemp.Rows[0]["Quote_Amount"].ToString();

            //    //for get lost quotation 


            //    //for lost quotation Amount 
            //    if (txtFromDate.Text != "" && txtToDate.Text != "")
            //    {
            //        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            //        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);


            //        dtTemp = objDa.return_DataTable("select SUM(Inv_SalesQuotationHeader.Amount-Inv_SalesQuotationHeader.DiscountValue+Inv_SalesQuotationHeader.TaxValue) as Quote_Amount from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.Status='Lost' and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "')");


            //        Session["Parameter"] = "From : " + txtFromDate.Text + "  To : " + txtToDate.Text;


            //    }
            //    else
            //    {

            //        dtTemp = objDa.return_DataTable("select SUM(Inv_SalesQuotationHeader.Amount-Inv_SalesQuotationHeader.DiscountValue+Inv_SalesQuotationHeader.TaxValue) as Quote_Amount from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.Status='Lost' and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + ")");

            //        Session["Parameter"] = "";
            //    }
            //    dr[10] = dtTemp.Rows[0]["Quote_Amount"].ToString();



            //    //for order 
            //    if (txtFromDate.Text != "" && txtToDate.Text != "")
            //    {
            //        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            //        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);


            //        dtTemp = objDa.return_DataTable("select COUNT(inv_salesorderheader.trans_id) as Order_No,sum(Inv_SalesOrderHeader.NetAmount) as Order_Amount from Inv_SalesOrderHeader where Inv_SalesOrderHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesOrderHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.IsActive='True' and (Inv_SalesOrderHeader.SOfromTransType='Q' and Inv_SalesOrderHeader.SOfromTransNo in (select  distinct Inv_SalesQuotationHeader.SQuotation_Id  from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "')))  ");

            //        Session["Parameter"] = "From : " + txtFromDate.Text + "  To : " + txtToDate.Text;
            //    }
            //    else
            //    {
            //        dtTemp = objDa.return_DataTable("select COUNT(inv_salesorderheader.trans_id) as Order_No,sum(Inv_SalesOrderHeader.NetAmount) as Order_Amount from Inv_SalesOrderHeader where Inv_SalesOrderHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesOrderHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.IsActive='True' and (Inv_SalesOrderHeader.SOfromTransType='Q' and Inv_SalesOrderHeader.SOfromTransNo in (select  distinct Inv_SalesQuotationHeader.SQuotation_Id  from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + "))) ");

            //        Session["Parameter"] = "";
            //    }
            //    dr[6] = dtTemp.Rows[0]["Order_No"].ToString();
            //    dr[7] = dtTemp.Rows[0]["Order_Amount"].ToString();



            //    //for get forecast order amount


            //    if (txtFromDate.Text != "" && txtToDate.Text != "")
            //    {
            //        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            //        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);


            //        dtTemp = objDa.return_DataTable("select SUM(Inv_SalesQuotationHeader.Amount-Inv_SalesQuotationHeader.DiscountValue+Inv_SalesQuotationHeader.TaxValue) as Forecast_Amount from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.Status='Open'  and Inv_SalesQuotationHeader.SQuotation_Id  not in (select  distinct Inv_SalesOrderHeader.SOfromTransNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' )  and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "')");


            //        Session["Parameter"] = "From : " + txtFromDate.Text + "  To : " + txtToDate.Text;


            //    }
            //    else
            //    {

            //        dtTemp = objDa.return_DataTable("select SUM(Inv_SalesQuotationHeader.Amount-Inv_SalesQuotationHeader.DiscountValue+Inv_SalesQuotationHeader.TaxValue) as Forecast_Amount from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.Status='Open' and Inv_SalesQuotationHeader.SQuotation_Id  not in (select  distinct Inv_SalesOrderHeader.SOfromTransNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' ) and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + ")");

            //        Session["Parameter"] = "";
            //    }

            //    dr[11] = dtTemp.Rows[0]["Forecast_Amount"].ToString();

            //    //for invoice 
            //    if (txtFromDate.Text != "" && txtToDate.Text != "")
            //    {
            //        Session["Parameter"] = "From : " + txtFromDate.Text + "  To : " + txtToDate.Text;
            //        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            //        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

            //        dtTemp = objDa.return_DataTable("select count( distinct Inv_SalesInvoiceHeader.Trans_Id) as Invoice_No,SUM((Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxV)*Quantity) as Invoice_Amount from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No where Inv_SalesInvoiceHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInvoiceHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.IsActive='True' and Inv_SalesInvoiceDetail.SIFromTransType='S' and Inv_SalesInvoiceDetail.SIFromTransNo in (select Inv_SalesOrderHeader.Trans_Id from Inv_SalesOrderHeader where Inv_SalesOrderHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesOrderHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.IsActive='True' and (Inv_SalesOrderHeader.SOfromTransType='Q' and Inv_SalesOrderHeader.SOfromTransNo in (select  distinct Inv_SalesQuotationHeader.SQuotation_Id  from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "'))) or Inv_SalesOrderHeader.Trans_Id in (select SH.Trans_Id from Inv_SalesOrderHeader as SH where  sh.Company_Id=" + Session["CompId"].ToString() + " and sh.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and sh.IsActive='True' and sh.SalesOrderDate>='" + txtFromDate.Text + "' and sh.SalesOrderDate<='" + ToDate.ToString() + "' and  sh.CreatedBy=(select Set_UserMaster.User_Id from Set_UserMaster where Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + ") ))");
            //    }
            //    else
            //    {
            //        dtTemp = objDa.return_DataTable("select count( distinct Inv_SalesInvoiceHeader.Trans_Id) as Invoice_No,SUM((Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxV)*Quantity) as Invoice_Amount from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No where Inv_SalesInvoiceHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInvoiceHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.IsActive='True' and Inv_SalesInvoiceDetail.SIFromTransType='S' and Inv_SalesInvoiceDetail.SIFromTransNo in (select Inv_SalesOrderHeader.Trans_Id from Inv_SalesOrderHeader where Inv_SalesOrderHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesOrderHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.IsActive='True' and (Inv_SalesOrderHeader.SOfromTransType='Q' and Inv_SalesOrderHeader.SOfromTransNo in (select  distinct Inv_SalesQuotationHeader.SQuotation_Id  from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader .Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.SInquiry_No in (select distinct Inv_SalesInquiryHeader.SInquiryID from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " ))) or Inv_SalesOrderHeader.Trans_Id in (select SH.Trans_Id from Inv_SalesOrderHeader as SH where  sh.Company_Id=" + Session["CompId"].ToString() + " and sh.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and sh.IsActive='True' and  sh.CreatedBy=(select Set_UserMaster.User_Id from Set_UserMaster where Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + ") ))");
            //        Session["Parameter"] = "";
            //    }

            //    //                or Inv_SalesOrderHeader.Trans_Id in (select SH.Trans_Id from Inv_SalesOrderHeader as SH where  sh.Company_Id=" + Session["CompId"].ToString() + " and sh.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and sh.IsActive='True' and sh.SalesOrderDate>='" + txtFromDate.Text + "' and sh.SalesOrderDate<='" + ToDate.ToString() + "' and  sh.CreatedBy=(select Set_UserMaster.User_Id from Set_UserMaster where Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + ")) 



            //    //or Inv_SalesOrderHeader.Trans_Id in (select SH.Trans_Id from Inv_SalesOrderHeader as SH where  sh.Company_Id=" + Session["CompId"].ToString() + " and sh.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and sh.IsActive='True'  and  sh.CreatedBy=(select Set_UserMaster.User_Id from Set_UserMaster where Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + ") )
            //    dr[8] = dtTemp.Rows[0]["Invoice_No"].ToString();
            //    dr[9] = dtTemp.Rows[0]["Invoice_Amount"].ToString();

            //    DtFilter.Rows.Add(dr);
        }



    }

    public DataTable GetFollowupReport(string strLocationId)
    {
        DataTable DtFilter = new DataTable();

        DtFilter.Columns.Add("Sales_Person");
        DtFilter.Columns.Add("TotalSalesInquiry", typeof(double));
        DtFilter.Columns.Add("PendingSalesInquiry", typeof(double));
        DtFilter.Columns.Add("CloseSalesInquiry", typeof(double));

        DtFilter.Columns.Add("TotalSalesQuotation", typeof(double));
        DtFilter.Columns.Add("OpenSalesQuotation", typeof(double));
        DtFilter.Columns.Add("CloseSalesQuotation", typeof(double));
        DtFilter.Columns.Add("LostSalesQuotation", typeof(double));

        DtFilter.Columns.Add("TotalSalesOrder", typeof(double));
        DtFilter.Columns.Add("PendingSalesOrder", typeof(double));
        DtFilter.Columns.Add("CloseSalesOrder", typeof(double));


        DtFilter.Columns.Add("TotalSalesInvoice", typeof(double));
        DtFilter.Columns.Add("PendingPaymentInvoice", typeof(double));
        DtFilter.Columns.Add("ReceivedPaymentInvoice", typeof(double));
        DtFilter.Columns.Add("TotalReturnAmount", typeof(double));




        double TotalSalesInquiry = 0;
        double PendingSalesInquiry = 0;
        double CloseSalesInquiry = 0;
        double TotalSalesQuotation = 0;
        double OpenSalesQuotation = 0;
        double CloseSalesQuotation = 0;
        double LostSalesQuotation = 0;
        double TotalSalesOrder = 0;
        double PendingSalesOrder = 0;
        double CloseSalesOrder = 0;
        double TotalSalesInvoice = 0;
        double PendingPaymentInvoice = 0;


        string strsql = "select Set_UserMaster.User_Id,Set_EmployeeMaster.Emp_Id as HandledEmpID,Set_EmployeeMaster.Emp_Name from Set_UserMaster inner join Set_EmployeeMaster on Set_UserMaster.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Company_Id=" + Session["CompId"].ToString() + " and Set_EmployeeMaster.Brand_Id=" + Session["BrandId"].ToString() + " and Set_EmployeeMaster.Location_Id in (" + strLocationId + ") and Set_EmployeeMaster.IsActive='True' and Set_EmployeeMaster.Field2='False'  and Set_UserMaster.Emp_Id in (select Inv_SalesInquiryHeader.HandledEmpID from Inv_SalesInquiryHeader)  order by Emp_Name";

        DataTable dt = objDa.return_DataTable(strsql);

        if (txtSalesPerson.Text != "")
        {

            try
            {
                dt = new DataView(dt, "HandledEmpID=" + HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString()) + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
                dt = new DataTable();
            }
        }
        string SqlConn = string.Empty;






        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = DtFilter.NewRow();

            dr["Sales_Person"] = dt.Rows[i]["Emp_Name"].ToString();


            //get all sales inquiry

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

                SqlConn = "select count ( distinct Inv_SalesInquiryHeader.SInquiryID) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "' and Inv_SalesInquiryHeader.IsActive='True' ";

            }
            else
            {
                SqlConn = "select count ( distinct Inv_SalesInquiryHeader.SInquiryID) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + "  and Inv_SalesInquiryHeader.IsActive='True' ";

            }

            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }

            TotalSalesInquiry = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());


            dr["TotalSalesInquiry"] = SetDecimal(TotalSalesInquiry.ToString());




            //get Pending sales imquiry %

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                SqlConn = "select count ( distinct Inv_SalesInquiryHeader.SInquiryID) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "' and Inv_SalesInquiryHeader.IsActive='True'  and inv_salesinquiryheader.SInquiryID not in ((select Inv_SalesQuotationHeader.SInquiry_No from Inv_SalesQuotationHeader))";

            }
            else
            {
                SqlConn = "select count ( distinct Inv_SalesInquiryHeader.SInquiryID) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + "  and Inv_SalesInquiryHeader.IsActive='True'  and inv_salesinquiryheader.SInquiryID not in ((select Inv_SalesQuotationHeader.SInquiry_No from Inv_SalesQuotationHeader))";

            }

            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }


            PendingSalesInquiry = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());


            if (TotalSalesInquiry == 0)
            {
                dr["PendingSalesInquiry"] = SetDecimal("0");

            }
            else
            {
                dr["PendingSalesInquiry"] = SetDecimal(((PendingSalesInquiry * 100) / TotalSalesInquiry).ToString());
            }



            //close sales inquiry detail 


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                SqlConn = "select count ( distinct Inv_SalesInquiryHeader.SInquiryID) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "' and Inv_SalesInquiryHeader.IsActive='True'  and inv_salesinquiryheader.SInquiryID  in ((select Inv_SalesQuotationHeader.SInquiry_No from Inv_SalesQuotationHeader))";

            }
            else
            {
                SqlConn = "select count ( distinct Inv_SalesInquiryHeader.SInquiryID) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + "  and Inv_SalesInquiryHeader.IsActive='True'  and inv_salesinquiryheader.SInquiryID  in ((select Inv_SalesQuotationHeader.SInquiry_No from Inv_SalesQuotationHeader))";

            }


            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }


            CloseSalesInquiry = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());



            if (TotalSalesInquiry == 0)
            {
                dr["CloseSalesInquiry"] = SetDecimal("0");
            }
            else
            {

                dr["CloseSalesInquiry"] = SetDecimal(((CloseSalesInquiry * 100) / TotalSalesInquiry).ToString());
            }


            //get all sales quotation 

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);



                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesQuotationHeader.Quotation_Date>='" + txtFromDate.Text + "' and Inv_SalesQuotationHeader.Quotation_Date<='" + ToDate.ToString() + "' and Inv_SalesQuotationHeader.IsActive='True' ";


            }
            else
            {
                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + "  and Inv_SalesQuotationHeader.IsActive='True' ";

            }


            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }



            TotalSalesQuotation = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());



            dr["TotalSalesQuotation"] = SetDecimal(TotalSalesQuotation.ToString());



            //get open sales quotation 


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesQuotationHeader.Quotation_Date>='" + txtFromDate.Text + "' and Inv_SalesQuotationHeader.Quotation_Date<='" + ToDate.ToString() + "' and Inv_SalesQuotationHeader.IsActive='True' and   Inv_SalesQuotationHeader.SQuotation_Id not in (select Inv_SalesOrderHeader.SOfromTransNo from Inv_SalesOrderHeader where SOfromTransType='Q') and Inv_SalesQuotationHeader.Status<>'Lost'";

            }
            else
            {

                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + "  and Inv_SalesQuotationHeader.IsActive='True' and   Inv_SalesQuotationHeader.SQuotation_Id not in (select Inv_SalesOrderHeader.SOfromTransNo from Inv_SalesOrderHeader where SOfromTransType='Q') and Inv_SalesQuotationHeader.Status<>'Lost' ";


            }

            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }

            OpenSalesQuotation = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());



            if (TotalSalesQuotation == 0)
            {
                dr["OpenSalesQuotation"] = SetDecimal("0");
            }
            else
            {

                dr["OpenSalesQuotation"] = SetDecimal(((OpenSalesQuotation * 100) / TotalSalesQuotation).ToString());
            }



            //close sales quotation 

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesQuotationHeader.Quotation_Date>='" + txtFromDate.Text + "' and Inv_SalesQuotationHeader.Quotation_Date<='" + ToDate.ToString() + "' and Inv_SalesQuotationHeader.IsActive='True' and   Inv_SalesQuotationHeader.SQuotation_Id  in (select Inv_SalesOrderHeader.SOfromTransNo from Inv_SalesOrderHeader where SOfromTransType='Q') ";

            }
            else
            {
                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + "  and Inv_SalesQuotationHeader.IsActive='True' and   Inv_SalesQuotationHeader.SQuotation_Id  in (select Inv_SalesOrderHeader.SOfromTransNo from Inv_SalesOrderHeader where SOfromTransType='Q')  ";

            }


            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }

            CloseSalesQuotation = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());




            if (TotalSalesQuotation == 0)
            {
                dr["CloseSalesQuotation"] = SetDecimal("0");
            }
            else
            {

                dr["CloseSalesQuotation"] = SetDecimal(((CloseSalesQuotation * 100) / TotalSalesQuotation).ToString());
            }

            //lost sales quotation 


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesQuotationHeader.Quotation_Date>='" + txtFromDate.Text + "' and Inv_SalesQuotationHeader.Quotation_Date<='" + ToDate.ToString() + "' and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.Status='Lost' ";
            }
            else
            {
                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + "  and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.Status='Lost'  ";

            }

            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }

            LostSalesQuotation = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());

            if (TotalSalesQuotation == 0)
            {
                dr["LostSalesQuotation"] = SetDecimal("0");
            }
            else
            {

                dr["LostSalesQuotation"] = SetDecimal(((LostSalesQuotation * 100) / TotalSalesQuotation).ToString());
            }


            //sales order detail 






            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                SqlConn = "select isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0) from Inv_SalesOrderHeader inner join Inv_SalesOrderDetail on Inv_SalesOrderHeader.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo  where Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.CreatedBy='" + dt.Rows[i]["User_Id"].ToString() + "' and Inv_SalesOrderHeader.SalesOrderDate>='" + txtFromDate.Text + "' and Inv_SalesOrderHeader.SalesOrderDate<='" + ToDate.ToString() + "' and Inv_SalesOrderHeader.IsActive='True' ";

            }
            else
            {
                SqlConn = "select isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0) from Inv_SalesOrderHeader inner join Inv_SalesOrderDetail on Inv_SalesOrderHeader.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo  where Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.CreatedBy='" + dt.Rows[i]["User_Id"].ToString() + "'  and Inv_SalesOrderHeader.IsActive='True' ";


            }

            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }


            TotalSalesOrder = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());


            dr["TotalSalesOrder"] = SetDecimal(TotalSalesOrder.ToString());



            //pending sales order detail
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

                SqlConn = "select isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0) from Inv_SalesOrderHeader inner join Inv_SalesOrderDetail on Inv_SalesOrderHeader.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo  where Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.CreatedBy='" + dt.Rows[i]["User_Id"].ToString() + "' and Inv_SalesOrderHeader.SalesOrderDate>='" + txtFromDate.Text + "' and Inv_SalesOrderHeader.SalesOrderDate<='" + ToDate.ToString() + "' and Inv_SalesOrderHeader.IsActive='True' and Inv_SalesOrderHeader.Trans_Id not in (select Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail) ";


            }
            else
            {
                SqlConn = "select isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0) from Inv_SalesOrderHeader inner join Inv_SalesOrderDetail on Inv_SalesOrderHeader.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo  where Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.CreatedBy='" + dt.Rows[i]["User_Id"].ToString() + "'  and Inv_SalesOrderHeader.IsActive='True' and Inv_SalesOrderHeader.Trans_Id not in (select Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail) ";

            }

            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }


            PendingSalesOrder = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());



            if (TotalSalesOrder == 0)
            {
                dr["PendingSalesOrder"] = SetDecimal("0");
            }
            else
            {

                dr["PendingSalesOrder"] = SetDecimal(((PendingSalesOrder * 100) / TotalSalesOrder).ToString());
            }

            //close sales order detail

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

                SqlConn = "select isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0) from Inv_SalesOrderHeader inner join Inv_SalesOrderDetail on Inv_SalesOrderHeader.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo  where Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.CreatedBy='" + dt.Rows[i]["User_Id"].ToString() + "' and Inv_SalesOrderHeader.SalesOrderDate>='" + txtFromDate.Text + "' and Inv_SalesOrderHeader.SalesOrderDate<='" + ToDate.ToString() + "' and Inv_SalesOrderHeader.IsActive='True' and Inv_SalesOrderHeader.Trans_Id  in (select Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail) ";


            }
            else
            {
                SqlConn = "select isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0) from Inv_SalesOrderHeader inner join Inv_SalesOrderDetail on Inv_SalesOrderHeader.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo  where Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.CreatedBy='" + dt.Rows[i]["User_Id"].ToString() + "'  and Inv_SalesOrderHeader.IsActive='True' and Inv_SalesOrderHeader.Trans_Id  in (select Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail) ";

            }

            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }


            CloseSalesOrder = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());

            if (TotalSalesOrder == 0)
            {
                dr["CloseSalesOrder"] = SetDecimal("0");
            }
            else
            {

                dr["CloseSalesOrder"] = SetDecimal(((CloseSalesOrder * 100) / TotalSalesOrder).ToString());
            }

            //sales invoice


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

                SqlConn = "select isnull( sum( (Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxV)*Inv_SalesInvoiceDetail.Quantity),0) from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No  where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.SalesPerson_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInvoiceHeader.Invoice_Date>='" + txtFromDate.Text + "' and Inv_SalesInvoiceHeader.Invoice_Date<='" + ToDate.ToString() + "' and Inv_SalesInvoiceHeader.IsActive='True' ";

            }
            else
            {
                SqlConn = "select isnull( sum( (Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxV)*Inv_SalesInvoiceDetail.Quantity),0) from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No  where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.SalesPerson_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + "  and Inv_SalesInvoiceHeader.IsActive='True' ";

            }
            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInvoiceDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInvoiceDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }

            TotalSalesInvoice = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());

            dr["TotalSalesInvoice"] = SetDecimal(TotalSalesInvoice.ToString());

            //pending payment invoice

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                strsql = "select ISNULL( SUM( dueamount),0) as DueAmount from (select ( MAX(invoice_amount)-SUM(paid_receive_amount)) as DueAmount from ac_ageing_detail where Ref_Type='SINV' and Ref_Id in (select Inv_SalesInvoiceHeader.Trans_Id from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No left join Inv_Product_Category on Inv_Product_Category.ProductId=Inv_SalesInvoiceDetail.Product_Id left join Inv_Product_Brand on Inv_Product_Brand.ProductId= Inv_SalesInvoiceDetail.Product_Id where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.SalesPerson_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInvoiceHeader.Invoice_Date>='" + txtFromDate.Text + "' and Inv_SalesInvoiceHeader.Invoice_Date<='" + ToDate.ToString() + "' and Inv_SalesInvoiceHeader.IsActive='True' and Inv_SalesInvoiceHeader.Post='True' and (CASE WHEN " + ddlcategorysearch.SelectedValue + " <> 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + ddlcategorysearch.SelectedValue + ") and (CASE WHEN " + ddlbrandsearch.SelectedValue + " <> 0 THEN Inv_Product_Brand.Brand_Id ELSE 0 END = " + ddlbrandsearch.SelectedValue + ")) group by Ac_Ageing_Detail.Ref_Id ) ab";
                PendingPaymentInvoice = Convert.ToDouble(objDa.return_DataTable(strsql).Rows[0][0].ToString());
            }
            else
            {
                strsql = "select ISNULL( SUM( dueamount),0) as DueAmount from (select ( MAX(invoice_amount)-SUM(paid_receive_amount)) as DueAmount from ac_ageing_detail where Ref_Type='SINV' and Ref_Id in (select Inv_SalesInvoiceHeader.Trans_Id from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No left join Inv_Product_Category on Inv_Product_Category.ProductId=Inv_SalesInvoiceDetail.Product_Id left join Inv_Product_Brand on Inv_Product_Brand.ProductId= Inv_SalesInvoiceDetail.Product_Id where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.SalesPerson_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInvoiceHeader.Invoice_Date>='" + txtFromDate.Text + "'  and Inv_SalesInvoiceHeader.Post='True' and (CASE WHEN " + ddlcategorysearch.SelectedValue + " <> 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + ddlcategorysearch.SelectedValue + ") and (CASE WHEN " + ddlbrandsearch.SelectedValue + " <> 0 THEN Inv_Product_Brand.Brand_Id ELSE 0 END = " + ddlbrandsearch.SelectedValue + ")) group by Ac_Ageing_Detail.Ref_Id ) ab";
                PendingPaymentInvoice = Convert.ToDouble(objDa.return_DataTable(strsql).Rows[0][0].ToString());

            }



            if (TotalSalesInvoice == 0 || ddlbrandsearch.SelectedIndex > 0 || ddlcategorysearch.SelectedIndex > 0)
            {
                dr["PendingPaymentInvoice"] = SetDecimal("0");
            }
            else
            {

                dr["PendingPaymentInvoice"] = SetDecimal(((PendingPaymentInvoice * 100) / TotalSalesInvoice).ToString());
            }
            //received payment invoice

            if (TotalSalesInvoice == 0 || ddlbrandsearch.SelectedIndex > 0 || ddlcategorysearch.SelectedIndex > 0)
            {

                dr["ReceivedPaymentInvoice"] = SetDecimal("0");
            }
            else
            {
                dr["ReceivedPaymentInvoice"] = SetDecimal((((TotalSalesInvoice - PendingPaymentInvoice) * 100) / TotalSalesInvoice).ToString());
            }

            //sales return

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                SqlConn = "select isnull( sum( (Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxV)* isnull(Inv_SalesInvoiceDetail.ReturnQty,0)),0) from  Inv_SalesReturnHeader as    Inv_SalesInvoiceHeader  inner join    Inv_SalesReturnDetail as  Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Return_No  where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.SalesPerson_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInvoiceHeader.Return_Date>='" + txtFromDate.Text.ToString() + "' and Inv_SalesInvoiceHeader.Return_Date<='" + ToDate.ToString() + "' and Inv_SalesInvoiceHeader.IsActive='True' ";
            }
            else
            {
                SqlConn = "select isnull( sum( (Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxV)* isnull(Inv_SalesInvoiceDetail.ReturnQty,0)),0) from  Inv_SalesReturnHeader as    Inv_SalesInvoiceHeader  inner join    Inv_SalesReturnDetail as  Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Return_No  where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.SalesPerson_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + "  and Inv_SalesInvoiceHeader.IsActive='True' ";
            }


            if (ddlcategorysearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInvoiceDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
            }
            if (ddlbrandsearch.SelectedIndex > 0)
            {
                SqlConn = SqlConn + " and Inv_SalesInvoiceDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
            }
            try
            {

                TotalSalesInvoice = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());
            }
            catch
            {

            }

            dr["TotalReturnAmount"] = SetDecimal(TotalSalesInvoice.ToString());



            DtFilter.Rows.Add(dr);
        }

        return DtFilter;
    }

    public string GetMonthname(int MonthNumber)
    {
        System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
        string strMonthName = mfi.GetMonthName(MonthNumber).ToString();
        return strMonthName;
    }



    public DataTable GetMonthLyFollowupReport(string strLocationId)
    {
        DataTable DtFilter = new DataTable();

        DtFilter.Columns.Add("Sales_Person");
        DtFilter.Columns.Add("MonthName");
        DtFilter.Columns.Add("TotalSalesInquiry", typeof(double));
        DtFilter.Columns.Add("PendingSalesInquiry", typeof(double));
        DtFilter.Columns.Add("CloseSalesInquiry", typeof(double));

        DtFilter.Columns.Add("TotalSalesQuotation", typeof(double));
        DtFilter.Columns.Add("OpenSalesQuotation", typeof(double));
        DtFilter.Columns.Add("CloseSalesQuotation", typeof(double));
        DtFilter.Columns.Add("LostSalesQuotation", typeof(double));

        DtFilter.Columns.Add("TotalSalesOrder", typeof(double));
        DtFilter.Columns.Add("PendingSalesOrder", typeof(double));
        DtFilter.Columns.Add("CloseSalesOrder", typeof(double));


        DtFilter.Columns.Add("TotalSalesInvoice", typeof(double));
        DtFilter.Columns.Add("PendingPaymentInvoice", typeof(double));
        DtFilter.Columns.Add("ReceivedPaymentInvoice", typeof(double));
        DtFilter.Columns.Add("TotalReturnAmount", typeof(double));




        double TotalSalesInquiry = 0;
        double PendingSalesInquiry = 0;
        double CloseSalesInquiry = 0;
        double TotalSalesQuotation = 0;
        double OpenSalesQuotation = 0;
        double CloseSalesQuotation = 0;
        double LostSalesQuotation = 0;
        double TotalSalesOrder = 0;
        double PendingSalesOrder = 0;
        double CloseSalesOrder = 0;
        double TotalSalesInvoice = 0;
        double PendingPaymentInvoice = 0;


        string strsql = "select Set_UserMaster.User_Id,Set_EmployeeMaster.Emp_Id as HandledEmpID,Set_EmployeeMaster.Emp_Name from Set_UserMaster inner join Set_EmployeeMaster on Set_UserMaster.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Company_Id=" + Session["CompId"].ToString() + " and Set_EmployeeMaster.Brand_Id=" + Session["BrandId"].ToString() + " and Set_EmployeeMaster.Location_Id in (" + strLocationId + ") and Set_EmployeeMaster.IsActive='True' and Set_EmployeeMaster.Field2='False'  and Set_UserMaster.Emp_Id in (select Inv_SalesInquiryHeader.HandledEmpID from Inv_SalesInquiryHeader)  order by Emp_Name";

        DataTable dt = objDa.return_DataTable(strsql);

        if (txtSalesPerson.Text != "")
        {

            try
            {
                dt = new DataView(dt, "HandledEmpID=" + HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString()) + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
                dt = new DataTable();
            }
        }
        string SqlConn = string.Empty;


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DateTime FinanceHeaderStartDate = Convert.ToDateTime(Session["FinanceFromdate"].ToString());
            DateTime FinanceStartDate = Convert.ToDateTime(Session["FinanceFromdate"].ToString());
            DateTime FinanceEndDate = Convert.ToDateTime(Session["FinanceTodate"].ToString());

            while (FinanceStartDate <= FinanceEndDate)
            {
                int Month = FinanceStartDate.Month;
                int Year = FinanceStartDate.Year;
                int dayOfMonth = DateTime.DaysInMonth(Year, Month);

                DateTime StartDate = new DateTime(Year, Month, 1);
                DateTime EndDate = new DateTime(Year, Month, dayOfMonth, 23, 59, 1);


                DataRow dr = DtFilter.NewRow();

                if (FinanceHeaderStartDate == FinanceStartDate)
                {

                    dr["Sales_Person"] = dt.Rows[i]["Emp_Name"].ToString();
                }
                dr["MonthName"] = GetMonthname(Month) + "-" + Year.ToString();


                //get all sales inquiry



                SqlConn = "select count ( distinct Inv_SalesInquiryHeader.SInquiryID) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + StartDate.ToString() + "' and Inv_SalesInquiryHeader.IDate<='" + EndDate.ToString() + "' and Inv_SalesInquiryHeader.IsActive='True' ";



                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }

                TotalSalesInquiry = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());


                dr["TotalSalesInquiry"] = SetDecimal(TotalSalesInquiry.ToString());




                //get Pending sales imquiry %


                SqlConn = "select count ( distinct Inv_SalesInquiryHeader.SInquiryID) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + StartDate.ToString() + "' and Inv_SalesInquiryHeader.IDate<='" + EndDate.ToString() + "' and Inv_SalesInquiryHeader.IsActive='True'  and inv_salesinquiryheader.SInquiryID not in ((select Inv_SalesQuotationHeader.SInquiry_No from Inv_SalesQuotationHeader))";



                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }


                PendingSalesInquiry = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());


                if (TotalSalesInquiry == 0)
                {
                    dr["PendingSalesInquiry"] = SetDecimal("0");

                }
                else
                {
                    dr["PendingSalesInquiry"] = SetDecimal(((PendingSalesInquiry * 100) / TotalSalesInquiry).ToString());
                }



                //close sales inquiry detail 



                SqlConn = "select count ( distinct Inv_SalesInquiryHeader.SInquiryID) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.HandledEmpID=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + StartDate.ToString() + "' and Inv_SalesInquiryHeader.IDate<='" + EndDate.ToString() + "' and Inv_SalesInquiryHeader.IsActive='True'  and inv_salesinquiryheader.SInquiryID  in ((select Inv_SalesQuotationHeader.SInquiry_No from Inv_SalesQuotationHeader))";




                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInqDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }


                CloseSalesInquiry = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());



                if (TotalSalesInquiry == 0)
                {
                    dr["CloseSalesInquiry"] = SetDecimal("0");
                }
                else
                {

                    dr["CloseSalesInquiry"] = SetDecimal(((CloseSalesInquiry * 100) / TotalSalesInquiry).ToString());
                }


                //get all sales quotation 




                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesQuotationHeader.Quotation_Date>='" + StartDate.ToString() + "' and Inv_SalesQuotationHeader.Quotation_Date<='" + EndDate.ToString() + "' and Inv_SalesQuotationHeader.IsActive='True' ";




                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }



                TotalSalesQuotation = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());



                dr["TotalSalesQuotation"] = SetDecimal(TotalSalesQuotation.ToString());



                //get open sales quotation 




                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesQuotationHeader.Quotation_Date>='" + StartDate.ToString() + "' and Inv_SalesQuotationHeader.Quotation_Date<='" + EndDate.ToString() + "' and Inv_SalesQuotationHeader.IsActive='True' and   Inv_SalesQuotationHeader.SQuotation_Id not in (select Inv_SalesOrderHeader.SOfromTransNo from Inv_SalesOrderHeader where SOfromTransType='Q') and Inv_SalesQuotationHeader.Status<>'Lost' ";



                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }

                OpenSalesQuotation = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());



                if (TotalSalesQuotation == 0)
                {
                    dr["OpenSalesQuotation"] = SetDecimal("0");
                }
                else
                {

                    dr["OpenSalesQuotation"] = SetDecimal(((OpenSalesQuotation * 100) / TotalSalesQuotation).ToString());
                }



                //close sales quotation 


                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesQuotationHeader.Quotation_Date>='" + StartDate.ToString() + "' and Inv_SalesQuotationHeader.Quotation_Date<='" + EndDate.ToString() + "' and Inv_SalesQuotationHeader.IsActive='True' and   Inv_SalesQuotationHeader.SQuotation_Id  in (select Inv_SalesOrderHeader.SOfromTransNo from Inv_SalesOrderHeader where SOfromTransType='Q') ";




                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }

                CloseSalesQuotation = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());




                if (TotalSalesQuotation == 0)
                {
                    dr["CloseSalesQuotation"] = SetDecimal("0");
                }
                else
                {

                    dr["CloseSalesQuotation"] = SetDecimal(((CloseSalesQuotation * 100) / TotalSalesQuotation).ToString());
                }

                //lost sales quotation 



                SqlConn = "select isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0) from Inv_SalesQuotationHeader inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id  where Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.Emp_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesQuotationHeader.Quotation_Date>='" + StartDate.ToString() + "' and Inv_SalesQuotationHeader.Quotation_Date<='" + EndDate.ToString() + "' and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesQuotationHeader.Status='Lost' ";


                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesQuotationDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }

                LostSalesQuotation = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());

                if (TotalSalesQuotation == 0)
                {
                    dr["LostSalesQuotation"] = SetDecimal("0");
                }
                else
                {

                    dr["LostSalesQuotation"] = SetDecimal(((LostSalesQuotation * 100) / TotalSalesQuotation).ToString());
                }


                //sales order detail 





                SqlConn = "select isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0) from Inv_SalesOrderHeader inner join Inv_SalesOrderDetail on Inv_SalesOrderHeader.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo  where Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.CreatedBy='" + dt.Rows[i]["User_Id"].ToString() + "' and Inv_SalesOrderHeader.SalesOrderDate>='" + StartDate.ToString() + "' and Inv_SalesOrderHeader.SalesOrderDate<='" + EndDate.ToString() + "' and Inv_SalesOrderHeader.IsActive='True' ";



                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }


                TotalSalesOrder = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());


                dr["TotalSalesOrder"] = SetDecimal(TotalSalesOrder.ToString());



                //pending sales order detail


                SqlConn = "select isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0) from Inv_SalesOrderHeader inner join Inv_SalesOrderDetail on Inv_SalesOrderHeader.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo  where Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.CreatedBy='" + dt.Rows[i]["User_Id"].ToString() + "' and Inv_SalesOrderHeader.SalesOrderDate>='" + StartDate.ToString() + "' and Inv_SalesOrderHeader.SalesOrderDate<='" + EndDate.ToString() + "' and Inv_SalesOrderHeader.IsActive='True' and Inv_SalesOrderHeader.Trans_Id not in (select Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail) ";




                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }


                PendingSalesOrder = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());



                if (TotalSalesOrder == 0)
                {
                    dr["PendingSalesOrder"] = SetDecimal("0");
                }
                else
                {

                    dr["PendingSalesOrder"] = SetDecimal(((PendingSalesOrder * 100) / TotalSalesOrder).ToString());
                }

                //close sales order detail



                SqlConn = "select isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0) from Inv_SalesOrderHeader inner join Inv_SalesOrderDetail on Inv_SalesOrderHeader.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo  where Inv_SalesOrderHeader.Location_Id in (" + strLocationId + ") and Inv_SalesOrderHeader.CreatedBy='" + dt.Rows[i]["User_Id"].ToString() + "' and Inv_SalesOrderHeader.SalesOrderDate>='" + StartDate.ToString() + "' and Inv_SalesOrderHeader.SalesOrderDate<='" + EndDate.ToString() + "' and Inv_SalesOrderHeader.IsActive='True' and Inv_SalesOrderHeader.Trans_Id  in (select Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail) ";




                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesOrderDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }


                CloseSalesOrder = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());

                if (TotalSalesOrder == 0)
                {
                    dr["CloseSalesOrder"] = SetDecimal("0");
                }
                else
                {

                    dr["CloseSalesOrder"] = SetDecimal(((CloseSalesOrder * 100) / TotalSalesOrder).ToString());
                }

                //sales invoice




                SqlConn = "select isnull( sum( (Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxV)*Inv_SalesInvoiceDetail.Quantity),0) from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No  where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.SalesPerson_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInvoiceHeader.Invoice_Date>='" + StartDate.ToString() + "' and Inv_SalesInvoiceHeader.Invoice_Date<='" + EndDate.ToString() + "' and Inv_SalesInvoiceHeader.IsActive='True' ";


                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInvoiceDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInvoiceDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }

                TotalSalesInvoice = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());

                dr["TotalSalesInvoice"] = SetDecimal(TotalSalesInvoice.ToString());

                //pending payment invoice


                strsql = "select ISNULL( SUM( dueamount),0) as DueAmount from (select ( MAX(invoice_amount)-SUM(paid_receive_amount)) as DueAmount from ac_ageing_detail where Ref_Type='SINV' and Ref_Id in (select Inv_SalesInvoiceHeader.Trans_Id from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No left join Inv_Product_Category on Inv_Product_Category.ProductId=Inv_SalesInvoiceDetail.Product_Id left join Inv_Product_Brand on Inv_Product_Brand.ProductId= Inv_SalesInvoiceDetail.Product_Id where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.SalesPerson_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInvoiceHeader.Invoice_Date>='" + StartDate.ToString() + "' and Inv_SalesInvoiceHeader.Invoice_Date<='" + EndDate.ToString() + "' and Inv_SalesInvoiceHeader.IsActive='True' and Inv_SalesInvoiceHeader.Post='True' and (CASE WHEN " + ddlcategorysearch.SelectedValue + " <> 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + ddlcategorysearch.SelectedValue + ") and (CASE WHEN " + ddlbrandsearch.SelectedValue + " <> 0 THEN Inv_Product_Brand.Brand_Id ELSE 0 END = " + ddlbrandsearch.SelectedValue + ")) group by Ac_Ageing_Detail.Ref_Id ) ab";
                PendingPaymentInvoice = Convert.ToDouble(objDa.return_DataTable(strsql).Rows[0][0].ToString());




                if (TotalSalesInvoice == 0 || ddlbrandsearch.SelectedIndex > 0 || ddlcategorysearch.SelectedIndex > 0)
                {
                    dr["PendingPaymentInvoice"] = SetDecimal("0");
                }
                else
                {

                    dr["PendingPaymentInvoice"] = SetDecimal(((PendingPaymentInvoice * 100) / TotalSalesInvoice).ToString());
                }
                //received payment invoice

                if (TotalSalesInvoice == 0 || ddlbrandsearch.SelectedIndex > 0 || ddlcategorysearch.SelectedIndex > 0)
                {

                    dr["ReceivedPaymentInvoice"] = SetDecimal("0");
                }
                else
                {
                    dr["ReceivedPaymentInvoice"] = SetDecimal((((TotalSalesInvoice - PendingPaymentInvoice) * 100) / TotalSalesInvoice).ToString());
                }


                //sales return 



                SqlConn = "select isnull( sum( (Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxV)* isnull(Inv_SalesInvoiceDetail.ReturnQty,0)),0) from  Inv_SalesReturnHeader as    Inv_SalesInvoiceHeader  inner join    Inv_SalesReturnDetail as  Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Return_No  where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.SalesPerson_Id=" + dt.Rows[i]["HandledEmpID"].ToString() + " and Inv_SalesInvoiceHeader.Return_Date>='" + StartDate.ToString() + "' and Inv_SalesInvoiceHeader.Return_Date<='" + EndDate.ToString() + "' and Inv_SalesInvoiceHeader.IsActive='True' ";


                if (ddlcategorysearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInvoiceDetail.Product_Id in (select Inv_Product_Category.productid from Inv_Product_Category where Inv_Product_Category.CategoryId= " + ddlcategorysearch.SelectedValue + ")";
                }
                if (ddlbrandsearch.SelectedIndex > 0)
                {
                    SqlConn = SqlConn + " and Inv_SalesInvoiceDetail.Product_Id in (select Inv_Product_Brand.ProductId from Inv_Product_Brand where Inv_Product_Brand.Brand_Id=" + ddlbrandsearch.SelectedValue + ")";
                }
                try
                {

                    TotalSalesInvoice = Convert.ToDouble(objDa.return_DataTable(SqlConn).Rows[0][0].ToString());
                }
                catch
                {

                }

                dr["TotalReturnAmount"] = SetDecimal(TotalSalesInvoice.ToString());


                DtFilter.Rows.Add(dr);
                FinanceStartDate = FinanceStartDate.AddMonths(1);
            }

        }
        return DtFilter;
    }

    public string SetDecimal(string amount)
    {

        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        rbtnheader.Checked = true;
        RbtnDetail.Checked = false;
        txtFromDate.Focus();
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductName.Text = "";
        txtTenderNo.Text = "";
        txtCustomerName.Text = "";
        txtInquiryNo.Text = "";
        ddlStatus.SelectedIndex = 0;
        FillLocation();
        lstLocationSelect.Items.Clear();
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
    public static string[] GetCompletionListInquiryNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesInquiryHeader ObjSinquiryHeader = new Inv_SalesInquiryHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjSinquiryHeader.GetSIHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["SInquiryNo"].ToString();
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
                dt = ObjSinquiryHeader.GetSIHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["SInquiryNo"].ToString();
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




    protected void txtInquiryNo_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = objSInquiryHeader.GetSIHeaderAllBySInquiryNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInquiryNo.Text);
        if (Dt.Rows.Count > 0)
        {
        }
        else
        {
            DisplayMessage("Select Inquiry No. in suggestion Only");
            txtInquiryNo.Text = "";
            txtInquiryNo.Focus();

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



    #region LocationWork
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

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            new PageControlCommon(Session["DBConnection"].ToString()).FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
    }
    public bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        bool Result = false;
        if (Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId,HttpContext.Current.Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
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
    #endregion

}
