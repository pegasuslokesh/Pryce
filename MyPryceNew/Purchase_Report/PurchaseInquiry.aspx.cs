using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Purchase_Report_PurchaseInquiry : BasePage
{
    Inv_ProductMaster ObjProductMaster = null;
    LocationMaster objLocation = null;
    SystemParameter ObjSysParam = null;
    PurchaseRequestHeader objPRequestHeader = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_PurchaseInquiryHeader objPInquiryHeader = null;
    Inv_PurchaseInquiry_Supplier ObjPISupplier = null;

    PInquiryDetail objReport = null;
    PInquiryDetailByInquiryNo objReportByInquiryNo = null;
    PInquiryDetailBySupplier objReportBySupplier = new PInquiryDetailBySupplier();

    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;

    PInquiryHeader objInquiryHeader = new PInquiryHeader();
    PInquiryHeader_ByDate objReportByDate = new PInquiryHeader_ByDate();

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
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objPInquiryHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        ObjPISupplier = new Inv_PurchaseInquiry_Supplier(Session["DBConnection"].ToString());

        objReport = new PInquiryDetail(Session["DBConnection"].ToString());
        objReportByInquiryNo = new PInquiryDetailByInquiryNo(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {

            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["DtFilter"] = null;

            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtFromDate.Focus();
            ChkInquiryNo.Visible = false;
            txtFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

        }
        else
        {
            if (rbtnheader.Checked == true && Session["ReportType"] != null && Session["SupplierGroup"]!=null)
                Get_PInquiryHeaderReport();
            if (RbtnDetail.Checked == true && Session["ReportType"] != null)
                Get_PInquirydetailReport();
        }
        AllPageCode();

    }
    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("174", (DataTable)Session["ModuleName"]);
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

            ChkInquiryNo.Visible = false;
            chkSupplier.Visible = true;
            chkSupplier.Checked = false;

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
            ChkInquiryNo.Visible = true;
            chkSupplier.Visible = false;
            ChkInquiryNo.Checked = false;
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
        Session["ReportHeader"] = null;
        Session["SupplierGroup"] = null;
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
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseInquiryHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInquiryHeader_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            if (chkSupplier.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Inquiry Header Report By Supplier";
                adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInquiryHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 1);

            }
            else
            {
                adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInquiryHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 2);
                Session["ReportHeader"] = "Purchase Inquiry Header Report By Inquiry Date";
            }





            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseInquiryHeader_SelectRow_Report;





            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = new DateTime();
                try
                {
                    ToDate = Convert.ToDateTime(txtToDate.Text);
                    DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                }
                catch
                {
                    DisplayMessage("Invalid Date");
                    txtFromDate.Text = "";
                    txtToDate.Text = "";
                    txtFromDate.Focus();
                    return;
                }
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDate.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }
                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;

            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "PIDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["PIDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;

            }
            if (txtInquiryNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "PI_No='" + txtInquiryNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Inquiry Header Report By InquiryNo.";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , InquiryNo";
                }

            }
            if (txtSupplierName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Supplier_Id =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (chkSupplier.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Inquiry Header Report By Supplier";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier";
                    }
                }

            }
            if (txtRequestNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "RequestNo ='" + txtRequestNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Inquiry Header Report By RequestNo.";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , RequestNo.";
                }

            }
            if (ddlStatus.SelectedIndex == 1)
            {
                {
                    try
                    {
                        DtFilter = new DataView(DtFilter, "Status ='" + ddlStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Inquiry Header Report By Status(Pending)";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " ,Status(Pending)";
                }
            }
            if (ddlStatus.SelectedIndex == 2)
            {
                {
                    try
                    {
                        DtFilter = new DataView(DtFilter, "Status <>'Pending'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Inquiry Header Report By Status(Working)";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Status(Working)";
                }


            }







            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                if (chkSupplier.Checked == true)
                {
                    Session["SupplierGroup"] = "Yes";
                }
                else
                {
                    Session["SupplierGroup"] = "No";
                }
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PInquiryHeaderReport.aspx','window','width=1024');", true);
                Get_PInquiryHeaderReport();
                // Response.Redirect("../Purchase_Report/PInquiryHeaderReport.aspx");
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

            PurchaseDataSetTableAdapters.sp_Inv_PurchaseInquiryDetail_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInquiryDetail_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInquiryDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseInquiryDetail_SelectRow_Report;
            if (ChkInquiryNo.Checked == false)
            {
                Session["ReportHeader"] = "Purchase Inquiry Detail Report By Inquiry Date";
                Session["ReportType"] = "0";
            }
            //if (chkSupplier.Checked == true)
            //{
            //    Session["ReportHeader"] = "Purchase Inquiry Detail Report By Supplier";
            //    Session["ReportType"] = "1";

            //}
            if (ChkInquiryNo.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Inquiry Detail Report By Inquiry No.";
                Session["ReportType"] = "2";
            }

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = new DateTime();
                try
                {
                    ToDate = Convert.ToDateTime(txtToDate.Text);
                    DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                }
                catch
                {
                    DisplayMessage("Invalid Date");
                    txtFromDate.Text = "";
                    txtToDate.Text = "";
                    txtFromDate.Focus();
                    return;
                }
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDate.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;


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
            if (txtInquiryNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "PI_No='" + txtInquiryNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (ChkInquiryNo.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Inquiry Detail Report By InquiryNo.";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , InquiryNo";
                    }
                }

            }
            if (txtSupplierName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Supplier_Id =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Inquiry Detail Report By Supplier";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier";
                }


            }
            if (txtRequestNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "RequestNo ='" + txtRequestNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Inquiry Detail Report By RequestNo.";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , RequestNo.";
                }
            }



            if (ddlStatus.SelectedIndex == 1)
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Status ='" + ddlStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Inquiry Detail Report By Status(Pending)";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " ,Status(Pending)";
                }



            }
            if (ddlStatus.SelectedIndex == 2)
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Status <>'Pending'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Inquiry Detail Report By Status(Working)";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " ,Status(Working)";
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
                if (DtFilter.Rows.Count > 0)
                {
                    if (Session["Parameter"] == null)
                    {
                        Session["Parameter"] = "FilterBy: ProductWise";
                    }
                    else
                    {
                        Session["Parameter"] = Session["Parameter"].ToString() + " , ProductWise";
                    }

                    Session["DtFilter"] = DtFilter;

                    //Response.Redirect("../Purchase_Report/PInquirydetailReportByProduct.aspx");
                }
                else
                {
                    DisplayMessage("Record Not Found");
                    RbtnDetail.Focus();
                    return;
                }

            }

            if (DtFilter.Rows.Count > 0)
            {

                Session["DtFilter"] = DtFilter;
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PInquirydetailReport.aspx','window','width=1024');", true);
                Get_PInquirydetailReport();
                // Response.Redirect("../Purchase_Report/PInquirydetailReport.aspx");
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
        txtSupplierName.Text = "";
        txtInquiryNo.Text = "";
        ddlStatus.SelectedIndex = 0;
        chkSupplier.Checked = false;



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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInquiryNo(string prefixText, int count, string contextKey)
    {
        Inv_PurchaseInquiryHeader objPInquiryHeader = new Inv_PurchaseInquiryHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = objPInquiryHeader.GetPIHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["PI_No"].ToString();
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
                dt = objPInquiryHeader.GetPIHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["PI_No"].ToString();
                    }
                }
            }
        }
        return str;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtSupplier = ObjSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
        DataTable dtMain = new DataTable();
        dtMain = dtSupplier.Copy();


        string filtertext = "Name like '" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            dtCon = dtSupplier;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }



    protected void txtInquiryNo_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = objPInquiryHeader.GetPIHeaderAllDataByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInquiryNo.Text);
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
    protected void txtRequestNo_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = objPRequestHeader.GetPurchaseRequestHeaderTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        try
        {
            Dt = new DataView(Dt, "RequestNo='" + txtRequestNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (Dt.Rows.Count > 0)
        {
        }
        else
        {
            DisplayMessage("Select Request No. in suggestion Only");
            txtRequestNo.Text = "";
            txtRequestNo.Focus();

            return;
        }

    }

    void Get_PInquirydetailReport()
    {
        lblHeader.Text = Session["ReportHeader"].ToString();
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



        if (Session["FromDate"] != null)
        {
            FromDate = Session["FromDate"].ToString();
        }
        if (Session["ToDate"] != null)
        {
            Todate = Session["ToDate"].ToString();
        }
        Title = "Purchase Inquiry Report";

        if (Session["ReportType"].ToString() == "0")
        {
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
        if (Session["ReportType"].ToString() == "1")
        {
            objReportBySupplier.setcompanyAddress(CompanyAddress);
            objReportBySupplier.setcompanyname(CompanyName);
            objReportBySupplier.setBrandName(BrandName);
            objReportBySupplier.setLocationName(LocationName);
            objReportBySupplier.settitle(Session["ReportHeader"].ToString());
            objReportBySupplier.SetImage(Imageurl);
            if (Session["Parameter"] == null)
            {
                objReportBySupplier.SetDateCriteria("");
            }
            else
            {
                objReportBySupplier.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportBySupplier.setUserName(Session["UserId"].ToString());

            objReportBySupplier.setArabic();
            objReportBySupplier.DataSource = Dt;
            objReportBySupplier.DataMember = "sp_Inv_PurchaseRequestDetail_Report";
            rptViewer.Report = objReportBySupplier;
            objReportBySupplier.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));

        }
        if (Session["ReportType"].ToString() == "2")
        {
            objReportByInquiryNo.setcompanyAddress(CompanyAddress);
            objReportByInquiryNo.setcompanyname(CompanyName);
            objReportByInquiryNo.setBrandName(BrandName);
            objReportByInquiryNo.setLocationName(LocationName);
            objReportByInquiryNo.settitle(Session["ReportHeader"].ToString());
            objReportByInquiryNo.SetImage(Imageurl);
            objReportByInquiryNo.setArabic();
            if (Session["Parameter"] == null)
            {
                objReportByInquiryNo.SetDateCriteria("");
            }
            else
            {
                objReportByInquiryNo.SetDateCriteria(Session["Parameter"].ToString());
            }
            objReportByInquiryNo.setUserName(Session["UserId"].ToString());


            objReportByInquiryNo.DataSource = Dt;
            objReportByInquiryNo.DataMember = "sp_Inv_PurchaseRequestDetail_Report";
            rptViewer.Report = objReportByInquiryNo;
            objReportByInquiryNo.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));

        }
        ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?Page=PI&&Url=" + ViewState["Path"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);

    }
    void Get_PInquiryHeaderReport()
    {
        lblHeader.Text = Session["ReportHeader"].ToString();
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



        if (Session["FromDate"] != null)
        {
            FromDate = Session["FromDate"].ToString();
        }
        if (Session["ToDate"] != null)
        {
            Todate = Session["ToDate"].ToString();
        }

        if (Session["ReportHeader"] == null)
        {
            Title = "Purchase Inquiry Header Report";
        }
        else
        {
            Title = Session["ReportHeader"].ToString();
        }
        if (Session["SupplierGroup"].ToString() == "Yes")
        {

            objInquiryHeader.setcompanyAddress(CompanyAddress);
            objInquiryHeader.setcompanyname(CompanyName);
            objInquiryHeader.setBrandName(BrandName);
            objInquiryHeader.setLocationName(LocationName);

            objInquiryHeader.settitle(Title);
            objInquiryHeader.SetImage(Imageurl);
            objInquiryHeader.setArabic();
            if (Session["Parameter"] == null)
            {
                objInquiryHeader.SetDateCriteria("");
            }
            else
            {
                objInquiryHeader.SetDateCriteria(Session["Parameter"].ToString());
            }
            objInquiryHeader.setUserName(Session["UserId"].ToString());

            objInquiryHeader.DataSource = Dt;
            objInquiryHeader.DataMember = "sp_Inv_PurchaseInquiryHeader_SelectRow_Report";
            rptViewer.Report = objInquiryHeader;
            objInquiryHeader.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));
        }
        else
        {
            objReportByDate.setcompanyAddress(CompanyAddress);
            objReportByDate.setcompanyname(CompanyName);
            objReportByDate.setBrandName(BrandName);
            objReportByDate.setLocationName(LocationName);
            objReportByDate.SetArabic();
            objReportByDate.settitle(Title);
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

            objReportByDate.DataSource = Dt;
            objReportByDate.DataMember = "sp_Inv_PurchaseInquiryHeader_SelectRow_Report";
            rptViewer.Report = objReportByDate;
            objReportByDate.ExportToPdf(Server.MapPath("~/Temp/" + Session["ReportHeader"].ToString() + ".pdf"));

        }
        ViewState["Path"] = Session["ReportHeader"].ToString() + ".pdf";
        rptToolBar.ReportViewer = rptViewer;
    }
}
