using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using PegasusDataAccess;
using Newtonsoft.Json;

public partial class Sales_SalesQuatationJScript : BasePage
{
    #region defined Class Object
    Common cmn = null;
    Inv_TaxRefDetail objTaxRefDetail = null;
    DataAccessClass objDa = null;
    IT_ObjectEntry objObjectEntry = null;
    Set_Approval_Employee objEmpApproval = null;
    Inv_SalesQuotationHeader objSQuoteHeader = null;
    Inv_SalesQuotationDetail ObjSQuoteDetail = null;
    Inv_SalesInquiryHeader objSIHeader = null;
    Inv_SalesInquiryDetail ObjSIDetail = null;
    Contact_PriceList objCustomerPriceList = null;
    Inv_ProductMaster objProductM = null;
    Ems_ContactMaster objContact = null;
    CurrencyMaster objCurrency = null;
    Inv_UnitMaster UM = null;
    EmployeeMaster objEmployee = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_ParameterMaster objInvParam = null;
    FollowUp ObjFollowupClass = null;
    Inv_ReferenceMailContact objMailContact = null;
    Set_AddressMaster objAddMaster = null;
    Inv_Product_RelProduct objRelProduct = null;
    LocationMaster objLocation = null;
    Inv_StockDetail objStockDetail = null;
    Inv_SalesQuotation_FollowUp objFollowup = null;
    NotificationMaster Obj_Notifiacation = null;
    Set_CustomerMaster objCustomer = null;
    PageControlCommon objPageCmn = null;


    string ProductIds = string.Empty;
    string Parameter_Id = string.Empty;
    string ParameterValue = string.Empty;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objSQuoteHeader = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        ObjSQuoteDetail = new Inv_SalesQuotationDetail(Session["DBConnection"].ToString());
        objSIHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        ObjSIDetail = new Inv_SalesInquiryDetail(Session["DBConnection"].ToString());
        objCustomerPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjFollowupClass = new FollowUp(Session["DBConnection"].ToString());
        objMailContact = new Inv_ReferenceMailContact(HttpContext.Current.Session["DBConnection_ES"].ToString());
        objAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());  
        objRelProduct = new Inv_Product_RelProduct(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objFollowup = new Inv_SalesQuotation_FollowUp(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        txtProductcode.Focus();
        //txtFollowupHeader.Attributes.Add("readonly", "true");
        txtSQDate.Attributes.Add("readonly", "true");
        //btnSQuoteSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSQuoteSave, "").ToString());
        if (!IsPostBack)
        {

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Sales/SalesQuotation.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            hdnDecimalCount.Value = Session["LoginLocDecimalCount"].ToString();
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();

            FillUser();
            FillUnit();

            Session["isSalesTaxEnabled"] = null;
            Session["IsSalesDiscountEnabled"] = null;
            ddlTransType.Enabled = true;
            Session["Temp_Product_Tax_SQ"] = null;
            bool Tax_Apply = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            Session["AddCtrl_State_Id"] = "";
            Session["AddCtrl_Country_Id"] = "";
            Session["Is_Tax_Apply"] = Tax_Apply.ToString();
            //cmn.FillUser(Session["CompId"].ToString(), Session["UserId"].ToString(), ddlUser, objObjectEntry.GetModuleIdAndName("57").Rows[0]["Module_Id"].ToString(), "57");
            ddlOption.SelectedIndex = 2;
            if (Request.QueryString["ReminderID"] != null)
            {
                //fillGrid(Request.QueryString["ReminderID"].ToString());
            }
            else
            {
                //FillGrid(1);
            }
          
            FillCurrency();
            FillTax();
            ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), Session["LocCurrencyId"].ToString(), Session["DBConnection"].ToString());
            txtSQNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtSQNo.Text;
            Calender.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtSQDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();

            //CalendarExtendertxtFollowupHeader.Format = Session["DateFormat"].ToString();
            rbtnFormView.Visible = false;
            rbtnAdvancesearchView.Visible = false;
            btnAddNewProduct.Visible = false;
            btnAddProductScreen.Visible = false;
            btnAddtoList.Visible = false;
            //this code is created by jitendra upadhyay when we open direct quotation page throug inquiry
            Session["DtSearchProduct"] = null;
            Session["dtQuotationDetail"] = null;
            //CalendarExtender1.Format = Session["DateFormat"].ToString();
            Session["dtFollowup"] = null;
            //txtFollowupDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            //CalendarExtendertxtFollowupHeader.Format = Session["DateFormat"].ToString();

            //used to create new quotation
            if (Request.QueryString["InquiryId"] != null)
            {
                hdnLocationId.Value = objSIHeader.getInquiryLocationById(Request.QueryString["InquiryId"].ToString());
                string quoteno = objSQuoteHeader.getOpenQuoteNoByInqId(Request.QueryString["InquiryId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value);
                FillTransactionType();
                if (quoteno != "")
                {
                    DisplayMessage("Please Make Disposal of Old Open Quotation: " + quoteno + "");
                    return;
                }
                fillInquiryRecordOnQuatation(Request.QueryString["InquiryId"].ToString(), hdnLocationId.Value);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSQNo);
                if (!ClientScript.IsStartupScriptRegistered("alert"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alert", "alertMe();", true);
                }
            }

            // by divya parakh 05-mar-2018
            if (Request.QueryString["FollowupID"] != null)
            {
                string strRequestId = Request.QueryString["FollowupID"].ToString();
                DataTable dtFollowupData = ObjFollowupClass.getDataByTransID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strRequestId);
                fillOppoRecordOnQuatation(strRequestId);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSQNo);
                if (!ClientScript.IsStartupScriptRegistered("alert"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alert", "alertMe();", true);
                }
                dtFollowupData.Dispose();
            }
            TaxandDiscountParameter();
            FillTransactionType();
            DataTable Dt_Individual = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Allow TAX edit on individual transactions Sales");
            if (Dt_Individual.Rows.Count > 0)
            {
                //if (Convert.ToBoolean(Dt_Individual.Rows[0]["ParameterValue"]) == true)
                //    Btn_Update_Tax.Visible = true;
                //else
                //    Btn_Update_Tax.Visible = false;
            }
            else
            {
                //Btn_Update_Tax.Visible = false;
            }
            Dt_Individual.Dispose();
        
        }

        if (Session["Is_Tax_Apply"] != null && Session["Is_Tax_Apply"].ToString() == "False")
            Trans_Div.Visible = false;

      /* Page.MaintainScrollPositionOnPostBack = true;
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
           
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }*/
       
    }
    

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetProductTax(string ProductId)
    {
        try
        {
            if (HttpContext.Current.Session["Temp_Product_Tax_SQ"].ToString() != null)
            {
                DataTable Dt_Cal = HttpContext.Current.Session["Temp_Product_Tax_SQ"] as DataTable;
                if (Dt_Cal.Rows.Count > 0)
                {
                    Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (Dt_Cal.Rows.Count > 0)
                    {
                        var Json = JsonConvert.SerializeObject(Dt_Cal);

                        return Json;
                    }
                }
            }
            else
            {
                return "";

            }
        }
        catch
        {
            return "";
        }
        return "";

    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtProduct_TextChanged(string product)
    {
        string[] ParameterValue = new string[7];
        DataTable dtProduct = new DataTable();
        try
        {
            using(dtProduct = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString()).return_DataTable("select top 1 case when Inv_StockDetail.Quantity IS null then '0' else Inv_StockDetail.Quantity end as Quantity , Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.EProductName, (select dbo.StripHTML(Inv_ProductMaster.Description)) as Description, Inv_ProductMaster.UnitId, case when relatedProducts IS null then 0 else relatedProducts end as relatedProducts from Inv_ProductMaster left join Inv_StockDetail on Inv_StockDetail.productid=inv_productmaster.ProductId and Inv_StockDetail.Finance_Year_Id='" + HttpContext.Current.Session["FinanceYearId"].ToString() + "' and Inv_StockDetail.Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Inv_StockDetail.Brand_Id ='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Inv_StockDetail.company_id='" + HttpContext.Current.Session["CompId"].ToString() + "' left join (select count(Product_Id) relatedProducts,Product_Id from Inv_Product_RelProduct group by Product_Id) relatedP on relatedP.Product_Id=Inv_ProductMaster.ProductId where Inv_ProductMaster.ProductCode ='" + product.Trim() + "' or Inv_ProductMaster.EProductName = '" + product.Trim() + "'"))
            {
                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    ParameterValue[0] = dtProduct.Rows[0]["EProductName"].ToString();
                    ParameterValue[1] = dtProduct.Rows[0]["ProductCode"].ToString();
                    ParameterValue[2] = "0";
                    if (dtProduct.Rows[0]["UnitId"].ToString() != "0" && dtProduct.Rows[0]["UnitId"].ToString() != "")
                    {
                        ParameterValue[2] = dtProduct.Rows[0]["UnitId"].ToString();
                    }
                    ParameterValue[3] = dtProduct.Rows[0]["Description"].ToString();
                    ParameterValue[4] = dtProduct.Rows[0]["ProductId"].ToString();
                    ParameterValue[5] = dtProduct.Rows[0]["relatedProducts"].ToString();
                    ParameterValue[6] = dtProduct.Rows[0]["Quantity"].ToString();
                }
                else
                {
                    return null;
                }
                string json = "";
                json = JsonConvert.SerializeObject(ParameterValue);
                return json;
            }
        }
        catch
        {
            return null;
        }
        finally
        {
            dtProduct = null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetGeoLocation(string ShippingAddress)
    {
        string[] result = new string[3];
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtAM = objAddMaster.GetAddressDataByAddressName(ShippingAddress.Split('/')[0].ToString());
        if (dtAM.Rows.Count > 0)
        {
            result[0] = dtAM.Rows[0]["Longitude"].ToString();
            result[1] = dtAM.Rows[0]["Latitude"].ToString();

            return result;
        }
        else
        {
            return result;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetInquiryDetail(string Id,string Customer_Id,string ExchangeRate)
    {
     
        Inv_ProductMaster objProductM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_UnitMaster UM = new Inv_UnitMaster(HttpContext.Current.Session["DBConnection"].ToString());
        CurrencyMaster objCurrency = new CurrencyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtDetail = (DataTable)HttpContext.Current.Session["dtQuotationDetail"];
        Sales_SalesQuatationJScript obj = new Sales_SalesQuatationJScript();
        if (dtDetail.Rows.Count > 0)
        {
            List<clsQtoHeader> ObjList = new List<clsQtoHeader>();
            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                clsQtoHeader objq = new clsQtoHeader();               
                DataTable dtProductCode = objProductM.GetProductMasterById(HttpContext.Current.Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), dtDetail.Rows[i]["Product_Id"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());                
                if (dtProductCode.Rows.Count != 0)
                {
                objq.ProductId =dtProductCode.Rows[0]["ProductCode"].ToString();
                }
                else
                {
                    objq.ProductId = "0";
                }
                objq.hdnNewProductId = dtDetail.Rows[i]["Product_Id"].ToString();
                objq.ProductName = dtDetail.Rows[i]["SuggestedProductName"].ToString();
                objq.RequiredQuantity = dtDetail.Rows[i]["Quantity"].ToString();
                objq.UnitId = dtDetail.Rows[i]["UnitId"].ToString();
              
                DataTable dtUName = UM.GetUnitMasterById(HttpContext.Current.Session["CompId"].ToString(), dtDetail.Rows[i]["UnitId"].ToString());
           
                if (dtUName.Rows.Count > 0)
                {
                    objq.UnitName = dtUName.Rows[0]["Unit_Name"].ToString();
                }
                else
                {
                    objq.UnitName = "0";
                }
                objq.UnitPrice = dtDetail.Rows[i]["EstimatedUnitPrice"].ToString();
                objq.GrossPrice= dtDetail.Rows[i]["EstimatedUnitPrice"].ToString();
                objq.TotalAmount= dtDetail.Rows[i]["EstimatedUnitPrice"].ToString();
                objq.Discount = dtDetail.Rows[i]["DiscountPercent"].ToString();
                objq.DiscountValue = dtDetail.Rows[i]["DiscountValue"].ToString();
                objq.TaxP = dtDetail.Rows[i]["TaxPercent"].ToString();
                objq.TaxV = dtDetail.Rows[i]["TaxValue"].ToString();
                objq.TotalAmount = dtDetail.Rows[i]["EstimatedUnitPrice"].ToString();
                objq.CurrencyId = dtDetail.Rows[i]["Currency_Id"].ToString();               
                DataTable dtCName = objCurrency.GetCurrencyMasterById(dtDetail.Rows[i]["Currency_Id"].ToString());
          
                if (dtCName.Rows.Count > 0)
                {
                    objq.CurrencyName = dtCName.Rows[0]["Currency_Code"].ToString();
                }
                else
                {
                    objq.CurrencyName = "0";
                }
                ///obj.Add_Tax_In_Session(objq.TotalAmount, objq.hdnNewProductId,i.ToString());
                objq.txtgvAgentCommission = "0.000";
                objq.Description = dtDetail.Rows[i]["ProductDescription"].ToString();
                ObjList.Add(objq);

            }
            var json = JsonConvert.SerializeObject(ObjList);
            return json;
        }
        return "";
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string FillSalesQuatationList()
    {
        try
        {
            DataTable dt = new DataTable();
            DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            Inv_SalesQuotationHeader objSQuoteHeader = new Inv_SalesQuotationHeader(HttpContext.Current.Session["DBConnection"].ToString());
            //dt = objSQuoteHeader.GetQuotationHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dt = objda.return_DataTable("SELECT TOP 100 SI.[Company_Id], SI.[Brand_Id], SI.[Location_Id], SI.[SQuotation_Id], SI.[SQuotation_No], SI.[Quotation_Date], SI.[SInquiry_No], SI.[Currency_Id],SI.[Amount], SI.[Emp_Id], SI.[TaxPercent], SI.[TaxValue], SI.[DiscountPercent],SI.[DiscountValue], SI.[Status], SI.[Header], SI.[Footer], SI.[Condition1],SI.[Condition2], SI.[Condition3], SI.[Condition4], SI.[Condition5], SI.[Post],SI.[Reason], SI.[Field1], SI.[Field2], SI.[Field3], SI.[Field4], SI.[Field5],SI.[Field6], SI.[Field7], SI.[CreatedBy], SI.[CreatedDate], SI.[ModifiedBy],SI.[ModifiedDate], SI.[IsActive], (SI.Amount + SI.TaxValue - SI.DiscountValue) AS TotalAmount,dbo.Set_EmployeeMaster.Emp_Name AS EmployeeName, Ems_ContactMaster.Name AS Customer_Name,Inv_SalesInquiryHeader.SInquiryNo AS InquiryNo, Inv_SalesInquiryHeader.OrderCompletionDate AS OrderCompletionDate,(SELECT STUFF((SELECT DISTINCT ',' + CAST(Inv_ProductMaster.EProductName AS char(14)) + '...' FROM Inv_ProductMaster WHERE Inv_ProductMaster.ProductId IN(SELECT Product_Id FROM dbo.Inv_SalesQuotationDetail WHERE SQuotation_Id = SI.SQuotation_Id) FOR xml PATH('')), 1, 1, '')) AS ShortProductList, (SELECT STUFF((SELECT DISTINCT ',' + RTRIM(Inv_ProductMaster.EProductName) FROM Inv_ProductMaster WHERE Inv_ProductMaster.ProductId IN (SELECT Product_Id FROM dbo.Inv_SalesQuotationDetail WHERE SQuotation_Id = SI.SQuotation_Id) FOR xml PATH('')), 1, 1, '')) AS FullProductList,(SELECT Inv_Product_CategoryMaster.Category_Name FROM Inv_Product_CategoryMaster WHERE Inv_Product_CategoryMaster.Category_Id = SI.Field5) AS CategoryName FROM Inv_SalesQuotationHeader AS SI LEFT JOIN Set_EmployeeMaster ON SI.Emp_Id = dbo.Set_EmployeeMaster.Emp_Id LEFT JOIN Inv_SalesInquiryHeader ON SI.SInquiry_No = Inv_SalesInquiryHeader.SInquiryID LEFT JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = Inv_SalesInquiryHeader.Customer_Id WHERE SI.Company_Id = " + HttpContext.Current.Session["CompId"].ToString() + " AND SI.Brand_Id = " + HttpContext.Current.Session["BrandId"].ToString() + " AND SI.Location_Id = " + HttpContext.Current.Session["LocId"].ToString() + " AND SI.IsActive = 'True' And SI.Status='Open' ORDER BY SI.SQuotation_Id Desc ");
            HttpContext.Current.Session["dtSQuotation"] = dt;
            HttpContext.Current.Session["dtFilterSQuotation"] = dt;
            // Sort the data based on "SQuotation_Id"
            DataView dataView = new DataView(dt);
            dataView.Sort = "SQuotation_Id";

            // Get the top 100 records
            int topN = 100;
            DataTable top100Records = dataView.ToTable().AsEnumerable().OrderByDescending(row => row.Field<int>("SQuotation_Id")).Take(topN).CopyToDataTable();

            if (top100Records != null && top100Records.Rows.Count > 0)
            {
                // Convert the top 100 records to JSON
                string JSONresult = JsonConvert.SerializeObject(top100Records);
                return JSONresult;
            }
            else
            {
                return "";
            }
        }
        catch (Exception ex)
        {
            return "";
        }

    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetProductPrice(string ProductId, string CustomerID)
    {
        string UnitPrice = "0";
        Inv_ProductMaster objProductM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        
            try
            {
                if (ProductId != null)
                {
                    UnitPrice = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", CustomerID, ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString())).ToString();
                    return UnitPrice;
                }
                else
                {
                    return UnitPrice;

                }
            }
            catch(Exception ex)
            {
                return UnitPrice;
            }        
       
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] ShipTo_TextChanged(string ShipTo)
    {
        string[] txt1 = new string[0];
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        if (ShipTo != "")
        {
            using (DataTable dtAM = objAddMaster.GetAddressDataByAddressName(ShipTo))
            {
                if (dtAM.Rows.Count > 0)
                {
                    string name = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());
                    string[] txt = { name };
                    return txt;
                }
            }
        }
        return txt1;
    }



    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSQuoteSave.Visible = clsPagePermission.bAdd;
        btnSQuoteSaveandPrint.Visible = clsPagePermission.bAdd;
        //GvSalesQuote.Columns[2].Visible = clsPagePermission.bEdit;
        //GvSalesQuote.Columns[0].Visible = clsPagePermission.bPrint;
        //GvDetail.Columns[1].Visible = clsPagePermission.bDelete;
        //GvSalesQuote.Columns[3].Visible = clsPagePermission.bDelete;
        //GvSalesQuote.Columns[1].Visible = clsPagePermission.bView;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        txtAgentName.Enabled = clsPagePermission.bPayCommission;
        //ddlUser.Visible = clsPagePermission.bViewAllUserRecord;

        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
        //Calender.Enabled = clsPagePermission.bModifyDate;
    }
    public void fillOppoRecordOnQuatation(string strRequestId)
    {
        DataTable dtPRequest = ObjFollowupClass.getDataByTransID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strRequestId);
        //DataTable dtDetail = objFollowupDetails.getProductDetailByTransID(strRequestId);
        DataTable dtDetail = ObjSIDetail.GetSIDetailBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strRequestId, Session["FinanceYearId"].ToString());
        if (dtPRequest.Rows.Count > 0)
        {
            txtCustomer.Text = dtPRequest.Rows[0]["PartyName"].ToString();
            txtContactName.Text = dtPRequest.Rows[0]["ContactName"].ToString() + "/" + dtPRequest.Rows[0]["Contact_Id"].ToString();
            txtInquiryNo.Text = dtPRequest.Rows[0]["SInquiryNo"].ToString();
            txtInquiryDate.Text = GetDate(dtPRequest.Rows[0]["IDate"].ToString());
            txtOrderCompletionDate.Text = GetDate(dtPRequest.Rows[0]["OrderCompletionDate"].ToString());
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dtPRequest.Rows[0]["Followup_by"].ToString());
            txtEmployee.Text = dtPRequest.Rows[0]["FollowupByName"].ToString() + "/" + Emp_Code;
            //txtFollowupDate.Text = GetDate(dtPRequest.Rows[0]["Followup_date"].ToString());
            //CalendarExtender1.SelectedDate = Convert.ToDateTime(dtPRequest.Rows[0]["Followup_date"].ToString());
            ddlCurrency.SelectedValue = dtDetail.Rows[0]["Currency_Id"].ToString();
            ddlPCurrency.SelectedValue = ddlCurrency.SelectedValue;

            hdnSalesInquiryId.Value = dtPRequest.Rows[0]["Ref_table_pk"].ToString();
            lblAmount.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Gross Total", Session["DBConnection"].ToString());
            Label3.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value", Session["DBConnection"].ToString());

            lblTotalAmount.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Net Total", Session["DBConnection"].ToString());

            dtDetail.Columns.Add("SalesPrice", typeof(string));
            ViewState["Customer_Id"] = dtPRequest.Rows[0]["Party_Id"].ToString();

            dtDetail.Columns.Add("Serial_No");
            dtDetail.Columns.Add("TaxPercent");
            dtDetail.Columns.Add("PurchaseProductPrice");
            dtDetail.Columns.Add("PurchaseProductDescription");
            dtDetail.Columns.Add("TaxValue");
            dtDetail.Columns.Add("PriceAfterTax");
            dtDetail.Columns.Add("DiscountPercent");
            dtDetail.Columns.Add("DiscountValue");
            dtDetail.Columns.Add("PriceAfterDiscount");
            dtDetail.Columns.Add("AgentCommission");
            dtDetail.Columns.Add("Sysqty");


            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                dtDetail.Rows[i]["Serial_No"] = (i + 1).ToString();
                dtDetail.Rows[i]["PurchaseProductPrice"] = dtDetail.Rows[i]["SalesPrice1"].ToString();
                dtDetail.Rows[i]["PurchaseProductDescription"] = dtDetail.Rows[i]["ProductDescription"].ToString();
                dtDetail.Rows[i]["TaxPercent"] = Get_Tax_Percentage(dtDetail.Rows[i]["Product_Id"].ToString(), dtDetail.Rows[i]["Serial_No"].ToString());
                dtDetail.Rows[i]["TaxValue"] = Get_Tax_Amount(dtDetail.Rows[i]["EstimatedUnitPrice"].ToString(), dtDetail.Rows[i]["Product_Id"].ToString(), dtDetail.Rows[i]["Serial_No"].ToString());
                dtDetail.Rows[i]["PriceAfterTax"] = "0";
                dtDetail.Rows[i]["DiscountPercent"] = "0";
                dtDetail.Rows[i]["DiscountValue"] = "0";
                dtDetail.Rows[i]["PriceAfterDiscount"] = "0";
                dtDetail.Rows[i]["AgentCommission"] = "0";
                dtDetail.Rows[i]["Sysqty"] = dtDetail.Rows[i]["Quantity"].ToString();

                ProductIds = dtDetail.Rows[i]["Product_Id"].ToString();

                try
                {
                    dtDetail.Rows[i]["SalesPrice"] = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", ViewState["Customer_Id"].ToString(), ProductIds, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();

                    dtDetail.Rows[i]["SalesPrice"] = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), dtDetail.Rows[i]["SalesPrice"].ToString());

                }
                catch
                {
                    dtDetail.Rows[i]["SalesPrice"] = "0";
                }

            }

            Session["dtQuotationDetail"] = dtDetail;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //objPageCmn.FillData((object)GvDetail, dtDetail, "", "");

            //btnAddNewProduct.Visible = true;
            //GvDetail.Columns[14].Visible = false;
            //GvDetail.Columns[17].Visible = false;
            //try
            //{
            //    GvDetail.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Estimated", Session["DBConnection"].ToString());
            //    GvDetail.HeaderRow.Cells[10].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Price", Session["DBConnection"].ToString());


            //    GvDetail.HeaderRow.Cells[11].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "", Session["DBConnection"].ToString());

            //    GvDetail.HeaderRow.Cells[13].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value", Session["DBConnection"].ToString());
            //    GvDetail.HeaderRow.Cells[16].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value", Session["DBConnection"].ToString());

            //    GvDetail.HeaderRow.Cells[18].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "", Session["DBConnection"].ToString());

            //}
            //catch (Exception err)
            //{

            //}

            //GvDetail.Columns[12].Visible = false;
            //foreach (GridViewRow Row in GvDetail.Rows)
            //{
            //    try
            //    {
            //        DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //        if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
            //        {
            //            ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
            //            ((Panel)Row.FindControl("PopupMenu")).Visible = false;
            //        }
            //    }
            //    catch
            //    {
            //    }

            //    // txtgvUnitPrice_TextChanged(((object)((TextBox)Row.FindControl("txtgvUnitPrice"))), null);

            //}
            rbtnFormView.Visible = true;
            rbtnAdvancesearchView.Visible = true;
            rbtnFormView.Checked = true;
            rbtnAdvancesearchView.Checked = false;
            rbtnFormView_OnCheckedChanged(null, null);
        }
        dtPRequest.Dispose();
        dtDetail.Dispose();
    }
    public void FillTransactionType()
    {
        ddlTransType.DataSource = Enum.GetValues(typeof(Common.TransactionType)).Cast<Common.TransactionType>().Select(s => new KeyValuePair<int, string>((int)s, s.ToString())).ToList();
        ddlTransType.DataValueField = "Key";
        ddlTransType.DataTextField = "Value";
        ddlTransType.DataBind();
        ddlTransType.Items.Insert(0, new ListItem("--Select--", "-1"));
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    public void TaxandDiscountParameter()
    {
        bool isTaxApplicable = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //for set tax visibility accoding inventory parameter
        //GvDetail.Columns[15].Visible = isTaxApplicable;
        //GvDetail.Columns[16].Visible = isTaxApplicable;
        lblTaxP.Visible = isTaxApplicable;
        txtTaxP.Visible = isTaxApplicable;
        Label21.Visible = isTaxApplicable;
        txtTaxV.Visible = isTaxApplicable;
        txtPriceAfterTax.Visible = false;

        bool isdiscApplicable = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //for set discount visibility accoding inventory parameter
        //GvDetail.Columns[12].Visible = isdiscApplicable;
        //GvDetail.Columns[13].Visible = isdiscApplicable;
        lblDiscountP.Visible = isdiscApplicable;
        txtDiscountP.Visible = isdiscApplicable;
        Label3.Visible = isdiscApplicable;
        txtDiscountV.Visible = isdiscApplicable;

    }
    public string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "13", "57", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    #region System defined Function
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        bool isTaxApplicable = Inventory_Common.IsSalesTaxEnabled(e.CommandName.ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //if (Convert.ToBoolean(Session["isSalesTaxEnabled"].ToString()) != isTaxApplicable)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Tax is not enabled on this location do you want to continue ?');", true);
        //    return;
        //}

        Hdn_Edit_ID.Value = e.CommandArgument.ToString();
        hdnLocationId.Value = e.CommandName.ToString();
        ddlTransType.Enabled = false;
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;
        if (objSenderID != "lnkViewDetail")
        {
            string isApproved = objInvParam.getParameterValueByParameterName("SalesQuotationApproval", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (Convert.ToBoolean(isApproved) == true)
            {
                string st = GetApprovalStatus(Convert.ToInt32(e.CommandArgument.ToString()));
                if (st == "Approved")
                {
                    DisplayMessage("Cannot Edit, Quotation has Approved");
                    return;
                }
            }
        }

        editid.Value = e.CommandArgument.ToString();
        edit();
        if (objSenderID == "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSQNo);

        if (objSenderID == "lnkViewDetail")
        {
            btnSQuoteSave.Enabled = false;
            BtnReset.Visible = false;
            btnSQuoteSaveandPrint.Enabled = false;
        }
        else
        {
            rbtnFormView.Visible = true;
            rbtnAdvancesearchView.Visible = true;
            rbtnFormView.Checked = true;
            rbtnFormView_OnCheckedChanged(null, null);
            btnSQuoteSave.Enabled = true;
            BtnReset.Visible = true;
            btnSQuoteSaveandPrint.Enabled = true;
        }

        txtShipingAddress_TextChanged(null, null);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GetShipToAddress(" + txtShipCustomerName.Text + ");", true);

        //Update_New.Update();
    }
    public void editOppo()
    {
        string CustomerId = string.Empty;
        CustomerId = "0";
        string ContactId = string.Empty;
        ContactId = "0";
        using (DataTable dtQuoteEdit = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value))
        {
            if (dtQuoteEdit.Rows.Count > 0)
            {

                txtSQNo.Text = dtQuoteEdit.Rows[0]["SQuotation_No"].ToString();
                ViewState["TimeStamp"] = dtQuoteEdit.Rows[0]["Row_Lock_Id"].ToString();
                txtSQNo.ReadOnly = true;
                txtSQDate.Text = Convert.ToDateTime(dtQuoteEdit.Rows[0]["Quotation_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());//updated by jitendra on 07-Nov-2013
                txtShipCustomerName.Text = "";
                txtShipingAddress.Text = "";
                string strCustomerId = dtQuoteEdit.Rows[0]["Field2"].ToString();
                if (strCustomerId != "" && strCustomerId != "0")
                {
                    txtShipCustomerName.Text = GetCustomerName(strCustomerId) + "/" + strCustomerId;

                }
                string strAddressId = dtQuoteEdit.Rows[0]["Field3"].ToString();

                using (DataTable dtAddName = objAddMaster.GetAddressDataByTransId(strAddressId, Session["CompId"].ToString()))
                {
                    if (dtAddName.Rows.Count > 0)
                    {
                        txtShipingAddress.Text = dtAddName.Rows[0]["Address_Name"].ToString();
                    }
                    else
                    {
                        txtShipingAddress.Text = "";
                    }
                }
                try
                {
                    ddlStatus.SelectedValue = dtQuoteEdit.Rows[0]["Status"].ToString().Trim();
                }
                catch
                {
                    ddlStatus.SelectedIndex = 0;
                }
                if (ddlStatus.SelectedValue.Trim() == "Close")
                {
                    //ddlStatus.Enabled = false;
                }

                txtReason.Text = dtQuoteEdit.Rows[0]["Reason"].ToString();
                //for get agent name according condition 

                string strAgentId = dtQuoteEdit.Rows[0]["Agent_Id"].ToString();
                if (strAgentId != "" && strAgentId != "0")
                {
                    txtAgentName.Text = dtQuoteEdit.Rows[0]["Agent_Name"].ToString() + "/" + strAgentId;
                    //GvDetail.Columns[GvDetail.Columns.Count - 1].Visible = true;
                }

                if (dtQuoteEdit.Rows[0]["Condition4"].ToString() != "" && dtQuoteEdit.Rows[0]["Condition4"].ToString() != "0")
                {
                    try
                    {
                        txtTemplateName.Text = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId("0", "0", "0", dtQuoteEdit.Rows[0]["Condition4"].ToString()).Rows[0]["Condition2"].ToString() + "/" + dtQuoteEdit.Rows[0]["Condition4"].ToString();
                    }
                    catch
                    {
                        txtTemplateName.Text = "";

                    }
                }
                //get follow up record

                //DataTable dtFollowup = objFollowup.GetRecord_By_SquotationId(editid.Value);


                //if (dtFollowup.Rows.Count > 0)
                //{
                //    dtFollowup = dtFollowup.DefaultView.ToTable(true, "Trans_Id", "Follow_Up_Date", "Follow_Up_Person", "Description");
                //    Session["dtFollowup"] = dtFollowup;
                //    GvFollowup.DataSource = dtFollowup;
                //    GvFollowup.DataBind();
                //}
                //else
                //{
                //    GvFollowup.DataSource = null;
                //    GvFollowup.DataBind();
                //}
                //dtFollowup.Dispose();

                //For Approval Status
                ViewState["ApprovalStatus"] = dtQuoteEdit.Rows[0]["Field4"].ToString();

                //Add Inquiry Data
                string strSalesInquiryId = dtQuoteEdit.Rows[0]["SInquiry_No"].ToString();
                hdnSalesInquiryId.Value = strSalesInquiryId;
                if (strSalesInquiryId != "" && strSalesInquiryId != "0")
                {
                    using (DataTable dtInquiry = objSIHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnSalesInquiryId.Value))
                    {
                        if (dtInquiry.Rows.Count > 0)
                        {
                            txtInquiryNo.Text = dtInquiry.Rows[0]["SInquiryNo"].ToString();
                            txtInquiryDate.Text = Convert.ToDateTime(dtInquiry.Rows[0]["IDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                            ddlCurrency.SelectedValue = dtInquiry.Rows[0]["Currency_Id"].ToString();
                            string strEmployeeId = dtInquiry.Rows[0]["HandledEmpID"].ToString();

                            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(strEmployeeId);

                            txtEmployee.Text = GetEmployeeName(strEmployeeId) + "/" + Emp_Code;
                            txtCustomer.Text = GetCustomerName(dtInquiry.Rows[0]["Customer_Id"].ToString());
                            txtOrderCompletionDate.Text = Convert.ToDateTime(dtInquiry.Rows[0]["OrderCompletiondate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                            //get contact number according individual contact 
                            txtContactNo.Text = GetCustomerContactNo(dtInquiry.Rows[0]["Field2"].ToString());
                            txtContactName.Text = GetCustomerName(dtInquiry.Rows[0]["Field2"].ToString()) + "/" + dtInquiry.Rows[0]["Field2"].ToString();
                            CustomerId = dtInquiry.Rows[0]["Customer_Id"].ToString();
                            ContactId = dtInquiry.Rows[0]["Field2"].ToString();
                            //if contact number not exist for individual then we get according selected customer 
                            if (txtContactNo.Text.Trim() == "")
                            {
                                txtContactNo.Text = GetCustomerContactNo(CustomerId);
                            }
                            ViewState["Customer_Id"] = CustomerId;
                        }
                    }
                }
                HeadearCalculateGrid();
                txtHeader.Content = dtQuoteEdit.Rows[0]["Header"].ToString();
                txtFooter.Content = dtQuoteEdit.Rows[0]["Footer"].ToString();
                txtCondition1.Content = dtQuoteEdit.Rows[0]["Condition1"].ToString();
                //GvDetail.Columns[14].Visible = false;
                //Gv//Detail.Columns[17].Visible = false;
            }
        }

    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetShipToAddress(string ShipToAddress)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtAM = objAddMaster.GetAddressDataByTransId(ShipToAddress.Split('/')[1].ToString(),HttpContext.Current.Session["CompId"].ToString());
        string Address = "0";
        if (dtAM.Rows.Count > 0)
        {
            Address = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["City_Name"].ToString(), dtAM.Rows[0]["State_Name"].ToString(), dtAM.Rows[0]["PhoneNo1"].ToString());

            return Address;

        }
        else
        {
            return Address;
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetShippingAddress(string ShippingAddress)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtAM = objAddMaster.GetAddressDataByTransId(ShippingAddress.Split('/')[1].ToString(), HttpContext.Current.Session["CompId"].ToString());
        string Address = "0";
        if (dtAM.Rows.Count > 0)
        {
            Address = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["City_Name"].ToString(), dtAM.Rows[0]["State_Name"].ToString(), dtAM.Rows[0]["PhoneNo1"].ToString());

            return Address;

        }
        else
        {
            return Address;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] editQuatation(string Id)
     {
        Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesQuotationHeader objSQuoteHeader = new Inv_SalesQuotationHeader(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesQuotationDetail ObjSQuoteDetail = new Inv_SalesQuotationDetail(HttpContext.Current.Session["DBConnection"].ToString());
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Set_Approval_Employee objEmpApproval = new Set_Approval_Employee(HttpContext.Current.Session["DBConnection"].ToString());
        SystemParameter ObjSysParam = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_TaxRefDetail objTaxRefDetail = new Inv_TaxRefDetail(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesQuotation_FollowUp objFollowup = new Inv_SalesQuotation_FollowUp(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesInquiryHeader objSIHeader = new Inv_SalesInquiryHeader(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_ReferenceMailContact objMailContact = new Inv_ReferenceMailContact(HttpContext.Current.Session["DBConnection_ES"].ToString());
        Sales_SalesQuatationJScript obj = new Sales_SalesQuatationJScript();
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string[] result = new string[3];
        bool isTaxApplicable = Inventory_Common.IsSalesTaxEnabled(Id, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string isApproved = objInvParam.getParameterValueByParameterName("SalesQuotationApproval", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (Convert.ToBoolean(isApproved) == true)
        {
            string st = obj.GetApprovalStatus(Convert.ToInt32(Id));
            if (st == "Approved")
            {
                result[0] = "Cannot Edit, Quotation has Approved";
                // DisplayMessage("Cannot Edit, Quotation has Approved");
                return result;
            }
        }
        string CustomerId = string.Empty;
        CustomerId = "0";
        string ContactId = string.Empty;
        ContactId = "0";
        DataTable dtQuoteEdit = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Id);
        if (dtQuoteEdit.Rows.Count > 0)
        {
            objSalesQta objqta = new objSalesQta();
            if (dtQuoteEdit.Rows[0]["Trans_Type"].ToString() != "")
            {
                objqta.ddlTransType = dtQuoteEdit.Rows[0]["Trans_Type"].ToString();
            }           
            else
            {
                objqta.ddlTransType = "0";
            }
            //Get_Tax_From_DB();
            objqta.editId= dtQuoteEdit.Rows[0]["SQuotation_Id"].ToString();
            objqta.Quotation_No = dtQuoteEdit.Rows[0]["SQuotation_No"].ToString();
            objqta.TimeStamp = dtQuoteEdit.Rows[0]["Row_Lock_Id"].ToString();
            objqta.Quotation_Date = Convert.ToDateTime(dtQuoteEdit.Rows[0]["Quotation_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());//updated by jitendra on 07-Nov-2013
            objqta.txtOrderCompletionDate = Convert.ToDateTime(dtQuoteEdit.Rows[0]["field7"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());//updated by on 12-Feb-2020
            objqta.txtShipCustomerName = "";
            objqta.txtShipingAddress = "";
            objqta.txtHeader = dtQuoteEdit.Rows[0]["Header"].ToString(); 
            objqta.txtFooter= dtQuoteEdit.Rows[0]["Footer"].ToString();
            objqta.txtCondition1 = dtQuoteEdit.Rows[0]["Condition1"].ToString();
            string strCustomerId = dtQuoteEdit.Rows[0]["Field2"].ToString();
            if (strCustomerId != "" && strCustomerId != "0")
            {
                objqta.txtShipCustomerName = obj.GetCustomerName(strCustomerId) + "/" + strCustomerId;

            }
            string strAddressId = dtQuoteEdit.Rows[0]["Field3"].ToString();

            using (DataTable dtAddName = objAddMaster.GetAddressDataByTransId(strAddressId, HttpContext.Current.Session["CompId"].ToString()))
            {
                if (dtAddName.Rows.Count > 0)
                {
                    objqta.txtShipingAddress = dtAddName.Rows[0]["Address_Name"].ToString()+"/"+ strAddressId;
                }
                else
                {
                    objqta.txtShipingAddress = "";
                }
            }
            try
            {
                objqta.ddlStatus = dtQuoteEdit.Rows[0]["Status"].ToString().Trim();
            }
            catch
            {
                objqta.ddlStatus = "0";
            }
            objqta.txtReason = dtQuoteEdit.Rows[0]["Reason"].ToString();
            //for get agent name according condition 

            string strAgentId = dtQuoteEdit.Rows[0]["Agent_Id"].ToString();
            if (strAgentId != "" && strAgentId != "0")
            {
                objqta.txtAgentName = dtQuoteEdit.Rows[0]["Agent_Name"].ToString() + "/" + strAgentId;
                //GvDetail.Columns[GvDetail.Columns.Count - 1].Visible = true;
            }
            if (dtQuoteEdit.Rows[0]["Condition4"].ToString() != "" && dtQuoteEdit.Rows[0]["Condition4"].ToString() != "0")
            {
                try
                {
                    objqta.txtTemplateName = objSQuoteHeader.getTemplateName(dtQuoteEdit.Rows[0]["Condition4"].ToString());
                }
                catch
                {
                    objqta.txtTemplateName = "";

                }
            }
            objqta.ApprovalStatus = dtQuoteEdit.Rows[0]["Field4"].ToString();

            //Add Inquiry Data
            string strSalesInquiryId = dtQuoteEdit.Rows[0]["SInquiry_No"].ToString();
            objqta.hdnSalesInquiryId = strSalesInquiryId;
            if (strSalesInquiryId != "" && strSalesInquiryId != "0")
            {
                using (DataTable dtInquiry = objSIHeader.GetSIHeaderAllBySInquiryId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objqta.hdnSalesInquiryId))
                {
                    if (dtInquiry.Rows.Count > 0)
                    {
                        objqta.txtInquiryNo = dtInquiry.Rows[0]["SInquiryNo"].ToString();
                        objqta.txtInquiryDate = Convert.ToDateTime(dtInquiry.Rows[0]["IDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                        objqta.Currency = dtInquiry.Rows[0]["Currency_Id"].ToString();
                        objqta.CurrencyId= dtInquiry.Rows[0]["Currency_Id"].ToString();
                        //objqta.hdnCustomerId.Value = dtInquiry.Rows[0]["Customer_Id"].ToString();
                        HttpContext.Current.Session["ContactID"] = dtInquiry.Rows[0]["Customer_Id"].ToString();
                        string strEmployeeId = dtInquiry.Rows[0]["HandledEmpID"].ToString();
                        HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
                        string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(strEmployeeId);
                        objqta.txtEmployee = obj.GetEmployeeName(strEmployeeId) + "/" + Emp_Code;
                        objqta.txtCustomer = obj.GetCustomerName(dtInquiry.Rows[0]["Customer_Id"].ToString());
                        ////txtOrderCompletionDate.Text = Convert.ToDateTime(dtInquiry.Rows[0]["OrderCompletiondate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                        //get contact number according individual contact 
                        objqta.txtContactNo = obj.GetCustomerContactNo(dtInquiry.Rows[0]["Field2"].ToString());
                        objqta.txtContactName = obj.GetCustomerName(dtInquiry.Rows[0]["Field2"].ToString()) + "/" + dtInquiry.Rows[0]["Field2"].ToString();

                        CustomerId = dtInquiry.Rows[0]["Customer_Id"].ToString();
                        ContactId = dtInquiry.Rows[0]["Field2"].ToString();
                        //if contact number not exist for individual then we get according selected customer 
                        if (objqta.txtContactNo.Trim() == "")
                        {
                            objqta.txtContactNo = obj.GetCustomerContactNo(CustomerId);
                        }
                        //ViewState["Customer_Id"] = CustomerId;
                    }
                }

            }
            using (DataTable dtDetail = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Id, HttpContext.Current.Session["FinanceYearId"].ToString()))
            {
                if (dtDetail.Rows.Count > 0)
                {
                    dtDetail.Columns["Field1"].ColumnName = "UnitId";
                    dtDetail.Columns["UnitPrice"].ColumnName = "SalesPrice";
                    dtDetail.Columns["Field2"].ColumnName = "EstimatedUnitPrice";
                    dtDetail.Columns["Field4"].ColumnName = "PurchaseProductPrice";
                    dtDetail.Columns["Field5"].ColumnName = "PurchaseProductDescription";

                    dtDetail.DefaultView.Sort = "Serial_No Asc";
                    HttpContext.Current.Session["dtQuotationDetail"] = dtDetail;
                    if (dtDetail != null && dtDetail.Rows.Count > 0)
                    {
                       List<clsQtoHeader> objDetail = new List<clsQtoHeader>();
                        foreach (DataRow DT_Row in dtDetail.Rows)
                        {
                            clsQtoHeader objdt = new clsQtoHeader();
                            if (DT_Row["TaxPercent"].ToString() == "0.000000" || DT_Row["TaxPercent"].ToString() == "0.00000" || DT_Row["TaxPercent"].ToString() == "0.0000" || DT_Row["TaxPercent"].ToString() == "0.000" || DT_Row["TaxPercent"].ToString() == "0.00" || DT_Row["TaxPercent"].ToString() == "0.0" || DT_Row["TaxPercent"].ToString() == "0")
                            {
                                DT_Row["TaxPercent"] = obj.Get_Tax_Percentage(DT_Row["Product_Id"].ToString(), DT_Row["Serial_No"].ToString());
                                DT_Row["TaxValue"] =obj.Get_Tax_Amount(DT_Row["SalesPrice"].ToString(), DT_Row["Product_Id"].ToString(), DT_Row["Serial_No"].ToString());
                            }
                            objdt.CurrencyId = DT_Row["Currency_Id"].ToString();
                            objdt.CurrencyName = obj.GetCurrencyName(DT_Row["Currency_Id"].ToString());
                            objdt.hdnNewProductId = DT_Row["Product_Id"].ToString();
                            objdt.ProductId = objDa.get_SingleValue("Select ProductCode from Inv_ProductMaster where ProductId='" + objdt.hdnNewProductId.ToString() + "'");
                            objdt.ProductName = DT_Row["SuggestedProductName"].ToString();
                            if (objdt.ProductName == "0")
                            {
                                objdt.ProductName = objDa.get_SingleValue("Select EProductName from Inv_ProductMaster where ProductId='" + objdt.hdnNewProductId.ToString() + "'");
                            }
                            objdt.RequiredQuantity = DT_Row["Quantity"].ToString();
                            objdt.TaxP = DT_Row["TaxPercent"].ToString();
                            objdt.TaxV = DT_Row["TaxValue"].ToString();
                            objdt.TotalAmount = DT_Row["SalesPrice"].ToString();
                            objdt.UnitId = DT_Row["UnitId"].ToString();
                            objdt.UnitName = obj.GetUnitName(DT_Row["UnitId"].ToString());
                            objdt.UnitPrice = DT_Row["SalesPrice"].ToString(); 
                            objdt.txtgvAgentCommission = DT_Row["AgentCommission"].ToString();
                            objdt.GrossPrice = DT_Row["SalesPrice"].ToString();
                            objdt.Discount = DT_Row["DiscountPercent"].ToString();
                            objdt.DiscountValue = DT_Row["DiscountValue"].ToString();
                            objdt.PriceAfterTax= DT_Row["PriceAfterTax"].ToString();
                            objDetail.Add(objdt);
                        }
                        objqta.Products = objDetail;
                        var json = JsonConvert.SerializeObject(objqta);
                        result[1] = json;
                    }
                }
            }








         }

        return result;

    }




    public void edit()
    {
        string CustomerId = string.Empty;
        CustomerId = "0";
        string ContactId = string.Empty;
        ContactId = "0";
        DataTable dtQuoteEdit = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, editid.Value);

        if (dtQuoteEdit.Rows.Count > 0)
        {
            if (dtQuoteEdit.Rows[0]["Trans_Type"].ToString() != "")
                ddlTransType.SelectedValue = dtQuoteEdit.Rows[0]["Trans_Type"].ToString();
            else
                ddlTransType.SelectedIndex = 0;

            Get_Tax_From_DB();
            txtSQNo.Text = dtQuoteEdit.Rows[0]["SQuotation_No"].ToString();
            ViewState["TimeStamp"] = dtQuoteEdit.Rows[0]["Row_Lock_Id"].ToString();
            txtSQNo.ReadOnly = true;
            txtSQDate.Text = Convert.ToDateTime(dtQuoteEdit.Rows[0]["Quotation_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());//updated by jitendra on 07-Nov-2013
            txtOrderCompletionDate.Text = Convert.ToDateTime(dtQuoteEdit.Rows[0]["field7"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());//updated by on 12-Feb-2020
            txtShipCustomerName.Text = "";
            txtShipingAddress.Text = "";
            string strCustomerId = dtQuoteEdit.Rows[0]["Field2"].ToString();
            if (strCustomerId != "" && strCustomerId != "0")
            {
                txtShipCustomerName.Text = GetCustomerName(strCustomerId) + "/" + strCustomerId;

            }
            string strAddressId = dtQuoteEdit.Rows[0]["Field3"].ToString();

            using (DataTable dtAddName = objAddMaster.GetAddressDataByTransId(strAddressId, Session["CompId"].ToString()))
            {
                if (dtAddName.Rows.Count > 0)
                {
                    txtShipingAddress.Text = dtAddName.Rows[0]["Address_Name"].ToString();
                }
                else
                {
                    txtShipingAddress.Text = "";
                }
            }
            try
            {
                ddlStatus.SelectedValue = dtQuoteEdit.Rows[0]["Status"].ToString().Trim();
            }
            catch
            {
                ddlStatus.SelectedIndex = 0;
            }



            if (ddlStatus.SelectedValue.Trim() == "Close")
            {
                //ddlStatus.Enabled = false;
            }


            txtReason.Text = dtQuoteEdit.Rows[0]["Reason"].ToString();
            //for get agent name according condition 

            string strAgentId = dtQuoteEdit.Rows[0]["Agent_Id"].ToString();
            if (strAgentId != "" && strAgentId != "0")
            {
                txtAgentName.Text = dtQuoteEdit.Rows[0]["Agent_Name"].ToString() + "/" + strAgentId;
                //GvDetail.Columns[GvDetail.Columns.Count - 1].Visible = true;
            }

            if (dtQuoteEdit.Rows[0]["Condition4"].ToString() != "" && dtQuoteEdit.Rows[0]["Condition4"].ToString() != "0")
            {
                try
                {
                    txtTemplateName.Text = objSQuoteHeader.getTemplateName(dtQuoteEdit.Rows[0]["Condition4"].ToString());
                }
                catch
                {
                    txtTemplateName.Text = "";

                }
            }
            //For Approval Status
            ViewState["ApprovalStatus"] = dtQuoteEdit.Rows[0]["Field4"].ToString();

            //Add Inquiry Data
            string strSalesInquiryId = dtQuoteEdit.Rows[0]["SInquiry_No"].ToString();
            hdnSalesInquiryId.Value = strSalesInquiryId;
            if (strSalesInquiryId != "" && strSalesInquiryId != "0")
            {
                using (DataTable dtInquiry = objSIHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, hdnSalesInquiryId.Value))
                {
                    if (dtInquiry.Rows.Count > 0)
                    {
                        txtInquiryNo.Text = dtInquiry.Rows[0]["SInquiryNo"].ToString();
                        txtInquiryDate.Text = Convert.ToDateTime(dtInquiry.Rows[0]["IDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                        ddlCurrency.SelectedValue = dtInquiry.Rows[0]["Currency_Id"].ToString();

                        hdnCustomerId.Value = dtInquiry.Rows[0]["Customer_Id"].ToString();
                        Session["ContactID"] = dtInquiry.Rows[0]["Customer_Id"].ToString();
                        string strEmployeeId = dtInquiry.Rows[0]["HandledEmpID"].ToString();
                        HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                        string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(strEmployeeId);
                        txtEmployee.Text = GetEmployeeName(strEmployeeId) + "/" + Emp_Code;
                        txtCustomer.Text = GetCustomerName(dtInquiry.Rows[0]["Customer_Id"].ToString());
                        ////txtOrderCompletionDate.Text = Convert.ToDateTime(dtInquiry.Rows[0]["OrderCompletiondate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                        //get contact number according individual contact 
                        txtContactNo.Text = GetCustomerContactNo(dtInquiry.Rows[0]["Field2"].ToString());
                        txtContactName.Text = GetCustomerName(dtInquiry.Rows[0]["Field2"].ToString()) + "/" + dtInquiry.Rows[0]["Field2"].ToString();

                        CustomerId = dtInquiry.Rows[0]["Customer_Id"].ToString();
                        ContactId = dtInquiry.Rows[0]["Field2"].ToString();
                        //if contact number not exist for individual then we get according selected customer 
                        if (txtContactNo.Text.Trim() == "")
                        {
                            txtContactNo.Text = GetCustomerContactNo(CustomerId);
                        }
                        ViewState["Customer_Id"] = CustomerId;
                    }
                }

            }

            //Add Detail Grid For Edit

            //Comment by ghanshyam suthar on 27-03-2018
            using (DataTable dtDetail = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, editid.Value, Session["FinanceYearId"].ToString()))
            {
                if (dtDetail.Rows.Count > 0)
                {
                    dtDetail.Columns["Field1"].ColumnName = "UnitId";
                    dtDetail.Columns["UnitPrice"].ColumnName = "SalesPrice";
                    dtDetail.Columns["Field2"].ColumnName = "EstimatedUnitPrice";
                    dtDetail.Columns["Field4"].ColumnName = "PurchaseProductPrice";
                    dtDetail.Columns["Field5"].ColumnName = "PurchaseProductDescription";

                    dtDetail.DefaultView.Sort = "Serial_No Asc";
                    Session["dtQuotationDetail"] = dtDetail;

                    if (dtDetail != null && dtDetail.Rows.Count > 0)
                    {
                        foreach (DataRow DT_Row in dtDetail.Rows)
                        {
                            if (DT_Row["TaxPercent"].ToString() == "0.000000" || DT_Row["TaxPercent"].ToString() == "0.00000" || DT_Row["TaxPercent"].ToString() == "0.0000" || DT_Row["TaxPercent"].ToString() == "0.000" || DT_Row["TaxPercent"].ToString() == "0.00" || DT_Row["TaxPercent"].ToString() == "0.0" || DT_Row["TaxPercent"].ToString() == "0")
                            {
                                DT_Row["TaxPercent"] = Get_Tax_Percentage(DT_Row["Product_Id"].ToString(), DT_Row["Serial_No"].ToString());
                                DT_Row["TaxValue"] = Get_Tax_Amount(DT_Row["SalesPrice"].ToString(), DT_Row["Product_Id"].ToString(), DT_Row["Serial_No"].ToString());
                            }
                        }
                    }


                    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    //objPageCmn.FillData((object)GvDetail, dtDetail, "", "");



                    //GvDetail.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Estimated", Session["DBConnection"].ToString());
                    ////GvDetail.HeaderRow.Cells[7].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Quanti");

                    //GvDetail.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Price", Session["DBConnection"].ToString());
                    //GvDetail.HeaderRow.Cells[10].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "", Session["DBConnection"].ToString());

                    //GvDetail.HeaderRow.Cells[12].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value", Session["DBConnection"].ToString());
                    //GvDetail.HeaderRow.Cells[15].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value", Session["DBConnection"].ToString());

                    //GvDetail.HeaderRow.Cells[17].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Net Amount", Session["DBConnection"].ToString());

                    //foreach (GridViewRow Row in GvDetail.Rows)
                    //{
                    //    try
                    //    {
                    //        DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //        if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
                    //        {
                    //            ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
                    //            ((Panel)Row.FindControl("PopupMenu")).Visible = false;

                    //        }
                    //    }
                    //    catch
                    //    {

                    //    }
                    //    HiddenField Product_ID = (HiddenField)Row.FindControl("hdnProductId");
                    //    Label lblgvEstimatedUnitPrice = (Label)Row.FindControl("lblgvEstimatedUnitPrice");
                    //    TextBox lblgvQuantity = (TextBox)Row.FindControl("lblgvQuantity");
                    //    TextBox txtgvUnitPrice = (TextBox)Row.FindControl("txtgvUnitPrice");
                    //    TextBox txtgvQuantityPrice = (TextBox)Row.FindControl("txtgvQuantityPrice");
                    //    TextBox txtgvDiscountP = (TextBox)Row.FindControl("txtgvDiscountP");
                    //    TextBox txtgvDiscountV = (TextBox)Row.FindControl("txtgvDiscountV");
                    //    TextBox txtgvPriceAfterDiscount = (TextBox)Row.FindControl("txtgvPriceAfterDiscount");
                    //    TextBox txtgvTaxP = (TextBox)Row.FindControl("txtgvTaxP");
                    //    TextBox txtgvTaxV = (TextBox)Row.FindControl("txtgvTaxV");
                    //    TextBox txtgvPriceAfterTax = (TextBox)Row.FindControl("txtgvPriceAfterTax");
                    //    TextBox txtgvTotal = (TextBox)Row.FindControl("txtgvTotal");
                    //    TextBox txtgvAgentCommission = (TextBox)Row.FindControl("txtgvAgentCommission");
                    //    Label lblgvSerialNo = (Label)Row.FindControl("lblgvSerialNo");
                    //    if (lblgvQuantity.Text == "")
                    //    {
                    //        lblgvQuantity.Text = "0";
                    //    }
                    //    if (txtgvUnitPrice.Text == "")
                    //    {
                    //        txtgvUnitPrice.Text = "0";
                    //    }
                    //    if (txtgvAgentCommission.Text == "")
                    //    {
                    //        txtgvAgentCommission.Text = "0";
                    //    }
                    //    txtgvQuantityPrice.Text = GetAmountDecimal((Convert.ToDouble(lblgvQuantity.Text) * Convert.ToDouble(txtgvUnitPrice.Text)).ToString());
                    //    lblgvEstimatedUnitPrice.Text = GetAmountDecimal(lblgvEstimatedUnitPrice.Text);
                    //    lblgvQuantity.Text = GetAmountDecimal(lblgvQuantity.Text);
                    //    txtgvUnitPrice.Text = GetAmountDecimal(txtgvUnitPrice.Text);
                    //    txtgvDiscountP.Text = GetAmountDecimal(txtgvDiscountP.Text);
                    //    txtgvDiscountV.Text = GetAmountDecimal(txtgvDiscountV.Text);
                    //    txtgvTaxP.Text = GetAmountDecimal(txtgvTaxP.Text);
                    //    txtgvTaxV.Text = GetAmountDecimal(txtgvTaxV.Text);
                    //    txtgvTotal.Text = GetAmountDecimal(txtgvTotal.Text);
                    //    txtgvAgentCommission.Text = GetAmountDecimal(txtgvAgentCommission.Text);

                    //    double PriceValue = double.Parse(txtgvUnitPrice.Text);
                    //    double QtyValue = double.Parse(lblgvQuantity.Text);
                    //    double AmountValue = PriceValue * QtyValue;
                    //    double AmntAfterDiscnt = AmountValue - (AmountValue * double.Parse(txtgvDiscountP.Text)) / 100;
                    //    bool IsValidDiscount = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                    //    if (AmountValue != AmntAfterDiscnt & IsValidDiscount)
                    //        txtgvDiscountV.Text = (AmountValue - AmntAfterDiscnt).ToString();
                    //    if (!IsValidDiscount)
                    //        AmntAfterDiscnt = AmountValue;
                    //    double TotalTax = Get_Tax_Amount(AmntAfterDiscnt.ToString(), Product_ID.Value, lblgvSerialNo.Text);
                    //    TotalTax = TotalTax / QtyValue;
                    //    string[] strvalue = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, "", txtgvDiscountP.Text, "", true, ddlCurrency.SelectedValue, false, Session["DBConnection"].ToString());
                    //    txtgvDiscountP.Text = strvalue[1].ToString();
                    //    txtgvDiscountV.Text = strvalue[2].ToString();
                    //    txtgvTaxP.Text = strvalue[3].ToString();
                    //    txtgvTaxV.Text = strvalue[4].ToString();
                    //    txtgvTotal.Text = strvalue[5].ToString();
                    //}
                    txtAmount.Text = GetRoundValue(GetAmountDecimal(dtQuoteEdit.Rows[0]["Amount"].ToString()));
                    lblAmount.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Gross Amount", Session["DBConnection"].ToString());
                    lblTotalAmount.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Net Amount", Session["DBConnection"].ToString());

                }
            }

            txtTaxP.Text = GetAmountDecimal(dtQuoteEdit.Rows[0]["TaxPercent"].ToString());
            txtTaxV.Text = GetAmountDecimal(dtQuoteEdit.Rows[0]["TaxValue"].ToString());
            HeadearCalculateGrid();
            txtHeader.Content = dtQuoteEdit.Rows[0]["Header"].ToString();
            txtFooter.Content = dtQuoteEdit.Rows[0]["Footer"].ToString();
            txtCondition1.Content = dtQuoteEdit.Rows[0]["Condition1"].ToString();
            // GvDetail.Columns[14].Visible = false;
            // GvDetail.Columns[17].Visible = false;

        }
        dtQuoteEdit.Dispose();

    }
    //protected void btnbindrpt_Click(object sender, EventArgs e)
    //{
    //    I1.Attributes.Add("Class", "fa fa-minus");
    //    Div1.Attributes.Add("Class", "box box-primary");
    //    if (ddlFieldName.SelectedItem.Value == "Quotation_Date" || ddlFieldName.SelectedItem.Value == "Field7" || ddlFieldName.SelectedItem.Value == "OrderCompletionDate")
    //    {
    //        if (txtValueDate.Text != "")
    //        {
    //            try
    //            {
    //                ObjSysParam.getDateForInput(txtValueDate.Text);
    //                txtValue.Text = Convert.ToDateTime(txtValueDate.Text).ToString();
    //            }
    //            catch
    //            {
    //                DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
    //                txtValueDate.Text = "";
    //                txtValue.Text = "";
    //                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            DisplayMessage("Enter Date");
    //            txtValueDate.Focus();
    //            return;
    //        }
    //    }
    //    //FillGrid(1);
    //    if (txtValue.Text != "")
    //        txtValue.Focus();
    //    else if (txtValueDate.Text != "")
    //        txtValueDate.Focus();
    //}
    ////protected void GvSalesQuote_Sorting(object sender, GridViewSortEventArgs e)
    ////{
    ////    string sortField = e.SortExpression;
    ////    SortDirection sortDirection = e.SortDirection;

    ////    if (GvSalesQuote.Attributes["CurrentSortField"] != null &&
    ////        GvSalesQuote.Attributes["CurrentSortDirection"] != null)
    ////    {
    ////        if (sortField == GvSalesQuote.Attributes["CurrentSortField"])
    ////        {
    ////            if (GvSalesQuote.Attributes["CurrentSortDirection"] == "ASC")
    ////            {
    ////                sortDirection = SortDirection.Descending;
    ////            }
    ////            else
    ////            {
    ////                sortDirection = SortDirection.Ascending;
    ////            }
    ////        }
    ////    }
    ////    GvSalesQuote.Attributes["CurrentSortField"] = sortField;
    ////    GvSalesQuote.Attributes["CurrentSortDirection"] =
    ////            (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
    ////    FillGrid(Int32.Parse(hdnGvSalesQuotationCurrentPageIndex.Value));
    ////}
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = 0;
        using (DataTable dtSoHeader = objSOrderHeader.GetSOHeaderAllByFromTransType(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), "Q", e.CommandArgument.ToString()))
        {

            if (dtSoHeader.Rows.Count > 0)
            {
                DisplayMessage("Sales Quotation is used in Sales order");
                return;
            }
        }

        //Code for Approval
        string quoteApproval = objInvParam.getParameterValueByParameterName("SalesQuotationApproval", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (Convert.ToBoolean(quoteApproval) == true)
        {
            string st = GetApprovalStatus(Convert.ToInt32(e.CommandArgument.ToString()));
            if (st == "Approved")
            {
                DisplayMessage("Cannot Delete, Quotation has Approved");
                return;
            }
        }


        editid.Value = e.CommandArgument.ToString();
        b = objSQuoteHeader.DeleteQuotationHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), editid.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            objEmpApproval.Delete_Approval_Transaction("8", Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), "0", e.CommandArgument.ToString());
            DisplayMessage("Record Delete");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        //FillGrid(1);
        //FillGridBin(); //Update grid view in bin tab
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        //FillGrid(1);
        //AllPageCode();
    }
   
    protected void btnSQuoteSaveandPrint_Click(object sender, EventArgs e)
    {
      
    }
    protected string GetEmployeeCode(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            using (DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), strEmployeeId))
            {
                if (dtEName.Rows.Count > 0)
                {
                    strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
                    ViewState["Emp_Img"] = "../CompanyResource/2/" + dtEName.Rows[0]["Emp_Image"].ToString();
                }
                else
                {
                    ViewState["Emp_Img"] = "";
                }
            }
        }
        else
        {
            strEmployeeName = "";
            ViewState["Emp_Img"] = "";
        }
        return strEmployeeName;
    }
    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/sales"));
        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = GetEmployeeCode(Session["UserId"].ToString()) + " request for Sales Quotation. on " + System.DateTime.Now.ToString();
        if (Hdn_Edit_ID.Value == "")
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["Ref_ID"].ToString(), "1");
        else
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Hdn_Edit_ID.Value, "15");
        Dt_Request_Type.Dispose();
    }    
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] SaveQuotation(objSalesQta objquo)
    {
        Inv_SalesQuotationHeader objSQuoteHeader = new Inv_SalesQuotationHeader(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesQuotationDetail ObjSQuoteDetail = new Inv_SalesQuotationDetail(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Set_AddressMaster objAddMaster = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Set_Approval_Employee objEmpApproval = new Set_Approval_Employee(HttpContext.Current.Session["DBConnection"].ToString());
        SystemParameter ObjSysParam = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_TaxRefDetail objTaxRefDetail = new Inv_TaxRefDetail(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesQuotation_FollowUp objFollowup = new Inv_SalesQuotation_FollowUp(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesInquiryHeader objSIHeader = new Inv_SalesInquiryHeader(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_ReferenceMailContact objMailContact = new Inv_ReferenceMailContact(HttpContext.Current.Session["DBConnection_ES"].ToString());
        Ems_ContactMaster objContact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string[] result = new string[3];
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        //Code Start
        if (objquo.editId != "")
        {
            string rowLock = objSQuoteHeader.getRowLocakNo(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objquo.editId);
            if (rowLock != "")
            {
                //if (objquo.TimeStamp != rowLock)
                //{
                //    //DisplayMessage("Another User update Information reload and try again");
                //    result[0] = "Another User update Information reload and try again";
                //    return result;
                //}
            }
        }

        Sales_SalesQuatationJScript obj = new Sales_SalesQuatationJScript();
        string strContactId = string.Empty;
        string Quotationid = string.Empty;
        strContactId = obj.JsContactId(objquo.txtContactName);

        if (objquo.Quotation_No == "")
        {
           // DisplayMessage("Enter Sales Quotation No.");
            result[0] = "Enter Sales Quotation No.";
            return result;
        }

        else
        {
            if (objquo .editId== "")
            {
                using (DataTable dtSQNo = objSQuoteHeader.GetQuotationHeaderAllBySQuotationNo(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objquo.Quotation_No))
                {
                    if (dtSQNo.Rows.Count > 0)
                    {
                        // DisplayMessage("Sales Quotation No. Already Exits");
                        //txtSQNo.Text = "";
                        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSQNo);
                        result[0] = "Sales Quotation No. Already Exits";
                        return result;
                    }
                }
            }
        }

        if (objquo.txtShipCustomerName != "")
        {
            if (Set_CustomerMaster.GetCustomerIdByCustomerName(objquo.txtShipCustomerName, HttpContext.Current.Session["DBConnection"].ToString()) == "")
            {
                objquo.txtShipCustomerName = "";
                // DisplayMessage("Ship To Choose In Suggestion Only");
                result[0] = "Ship To Choose In Suggestion Only";
                return result;
            }
        }
        string strAddressId = string.Empty;
        if (objquo.txtShipingAddress != "")
        {
            using (DataTable dtAddId = objAddMaster.GetAddressDataByTransId(objquo.txtShipingAddress.Split('/')[1].ToString(), HttpContext.Current.Session["CompId"].ToString()))
            {
                if (dtAddId.Rows.Count > 0)
                {
                    strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                }
            }
        }
        else
        {
            strAddressId = "0";
        }


        if (objquo.hdnSalesInquiryId != "0")
        {

        }
        else if (objquo.hdnSalesInquiryId == "0")
        {
            //DisplayMessage("Choose Record In Inquiry Section For Create Quotation");
            //return;
            result[0] = "Choose Record In Inquiry Section For Create Quotation";
            return result;
        }


        if (objquo.GrossAmount == "")
        {
            objquo.GrossAmount = "0";
        }
        //if (txtTaxP.Text == "")
        //{
        //    txtTaxP.Text = "0";
        //}
        //if (txtTaxV.Text == "")
        //{
        //    txtTaxV.Text = "0";
        //}
        if (objquo.txtDiscountP == "")
        {
            objquo.txtDiscountP = "0";
        }
        if (objquo.txtDiscountV == "")
        {
            objquo.txtDiscountV = "0";
        }
        if (objquo.txtTotalAmount == "")
        {
            objquo.txtTotalAmount = "0";
        }
        if (objquo.ddlStatus == "" || objquo.ddlStatus == null||objquo.ddlStatus== "--Select--")
        {
            objquo.ddlStatus = "Open";
        }
        string strTemplateId = string.Empty;

        if (objquo.txtTemplateName == "")
        {
            strTemplateId = "0";
        }
        else
        {
            try
            {
                strTemplateId = objquo.txtTemplateName.Split('/')[1].ToString();
            }
            catch
            {
                strTemplateId = "0";
            }
        }

        if (objquo.ddlStatus.Trim().ToUpper() == "LOST")
        {
            if (objquo.txtReason == "")
            {
                //DisplayMessage("Enter Lost Reason");
                //txtReason.Text = "";
                //txtReason.Focus();
                result[0] = "Enter Lost Reason";
                return result;
            }
            else
            {
                if (objquo.txtReason.Length < 50)
                {
                    //DisplayMessage("At least 50 character must be required in reason");
                    result[0] = "At least 50 character must be required in reason";
                    return result;
                }
            }
        }

        string strAgentId = string.Empty;
        if (objquo.txtAgentName != "")
        {
            strAgentId = objquo.txtAgentName.Split('/')[1].ToString();
        }
        else
        {
            strAgentId = "0";
        }
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            if (objquo.editId != "")
            {
                Quotationid = objquo.editId;

                if (objquo.Currency == "")
                {
                    objquo.Currency = "0";
                }
                if (objquo.txtTaxP == null || objquo.txtTaxP == "")
                {
                    objquo.txtTaxP = "0";
                }

                if (objquo.txtTaxV == null || objquo.txtTaxV == "")
                {
                    objquo.txtTaxV = "0";
                }
                if (objquo.ddlTransType == null || objquo.ddlTransType == "")
                {
                    objquo.ddlTransType = "0";
                }
                string SalesQuotationApproval = objInvParam.getParameterValueByParameterName("SalesQuotationApproval", ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (SalesQuotationApproval != "")
                {
                    if (Convert.ToBoolean(SalesQuotationApproval) == true)
                    {
                        if (objquo.ApprovalStatus == "Rejected")
                        {
                            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
                            string Emp_Code = objquo.txtEmployee.Split('/')[1].ToString();
                            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

                            b = objSQuoteHeader.UpdateQuotationHeader(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objquo.editId, objquo.Quotation_No, ObjSysParam.getDateForInput(objquo.Quotation_Date).ToString(), objquo.hdnSalesInquiryId, objquo.Currency, Emp_ID, objquo.GrossAmount, objquo.txtTaxP, objquo.txtTaxV, objquo.txtDiscountP, objquo.txtDiscountV, objquo.ddlStatus, objquo.txtHeader, objquo.txtFooter, objquo.txtCondition1, "", "", strTemplateId, "", "False", strAgentId, objquo.txtReason, strContactId, Set_CustomerMaster.GetCustomerIdByCustomerName(objquo.txtShipCustomerName, ref trns), strAddressId, "Pending", "", "True", objquo.txtOrderCompletionDate, "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), objquo.ddlTransType,"0", "0", ref trns);
                            DataTable dtEmp = objEmpApproval.getApprovalTransByRef_IDandApprovalId(objquo.editId.ToString(), "8", ref trns);

                            if (dtEmp.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtEmp.Rows.Count; i++)
                                {
                                    objEmpApproval.UpdateApprovalTransaciton("SalesQuotation", objquo.editId.ToString(), "57", dtEmp.Rows[i]["Emp_Id"].ToString(), "Pending", dtEmp.Rows[i]["Description"].ToString(), dtEmp.Rows[i]["Approval_Id"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString());
                                }
                            }
                        }
                        else
                        {
                            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
                            string Emp_Code = objquo.txtEmployee.Split('/')[1].ToString();
                            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                            b = objSQuoteHeader.UpdateQuotationHeader(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objquo.editId, objquo.Quotation_No, ObjSysParam.getDateForInput(objquo.Quotation_Date).ToString(), objquo.hdnSalesInquiryId, objquo.Currency, Emp_ID, objquo.GrossAmount, objquo.txtTaxP, objquo.txtTaxV, objquo.txtDiscountP, objquo.txtDiscountV, objquo.ddlStatus, objquo.txtHeader, objquo.txtFooter, objquo.txtCondition1, "", "", strTemplateId, "", "False", strAgentId, objquo.txtReason, strContactId, Set_CustomerMaster.GetCustomerIdByCustomerName(objquo.txtShipCustomerName, ref trns), strAddressId, objquo.ApprovalStatus, "", "True", objquo.txtOrderCompletionDate, "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), objquo.ddlTransType,"0", "0", ref trns);
                        }
                    }
                    else
                    {
                        HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
                        string Emp_Code = objquo.txtEmployee.Split('/')[1].ToString();
                        string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                        b = objSQuoteHeader.UpdateQuotationHeader(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objquo.editId, objquo.Quotation_No, ObjSysParam.getDateForInput(objquo.Quotation_Date).ToString(), objquo.hdnSalesInquiryId, objquo.Currency, Emp_ID, objquo.GrossAmount, objquo.txtTaxP, objquo.txtTaxV, objquo.txtDiscountP, objquo.txtDiscountV, objquo.ddlStatus, objquo.txtHeader, objquo.txtFooter, objquo.txtCondition1, "", "", strTemplateId, "", "False", strAgentId, objquo.txtReason, strContactId, Set_CustomerMaster.GetCustomerIdByCustomerName(objquo.txtShipCustomerName, ref trns), strAddressId, objquo.ApprovalStatus, "", "True", objquo.txtOrderCompletionDate, "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), objquo.ddlTransType.ToString(),"0","0",ref trns);
                    }

                }

                if (b != 0)
                {
                    ObjSQuoteDetail.DeleteQuotationDetail(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objquo.editId, ref trns);
                    objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SQ", objquo.editId, ref trns);

                    foreach(var gvr in objquo.Products)
                    {
                         if (gvr.UnitPrice == "" || gvr.UnitPrice == null)
                        {
                            gvr.UnitPrice = "0";
                        }
                        if (gvr.TaxP == ""|| gvr.TaxP==null)
                        {
                            gvr.TaxP = "0";
                        }
                        if (gvr.TaxV == ""|| gvr.TaxV == null)
                        {
                            gvr.TaxV = "0";
                        }
                        if (gvr.PriceAfterTax == ""|| gvr.PriceAfterTax ==null)
                        {
                            gvr.PriceAfterTax = "0";
                        }
                        if (gvr.Discount == ""|| gvr.Discount==null)
                        {
                            gvr.Discount = "0";
                        }
                        if (gvr.DiscountValue == ""||gvr.DiscountValue ==null )
                        {
                            gvr.DiscountValue = "0";
                        }
                        if (gvr.TotalAmount == ""|| gvr.TotalAmount == null)
                        {
                            gvr.TotalAmount = "0";
                        }

                        if (gvr.txtgvAgentCommission == "" || gvr.txtgvAgentCommission == null)
                        {
                            gvr.txtgvAgentCommission = "0";
                        }
                        if(gvr.Description==""|| gvr.Description == null)
                        {
                            gvr.Description = "0";
                        }

                        string IsProductNameShow = objInvParam.getParameterValueByParameterName("IsProductNameShow", ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        if (Convert.ToBoolean(IsProductNameShow))
                        {
                            //when showing product name
                            int Detail_ID = ObjSQuoteDetail.InsertQuotationDetail(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objquo.editId,"0",gvr.hdnNewProductId,gvr.Description,gvr.CurrencyId,gvr.UnitPrice, gvr.RequiredQuantity, gvr.TaxP, gvr.TaxV, gvr.PriceAfterTax, gvr.Discount,gvr.DiscountValue,gvr.TotalAmount, "False",gvr.txtgvAgentCommission,gvr.UnitId,gvr.UnitPrice, gvr.Description,gvr.UnitPrice,gvr.Description, "False", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            //Insert_Tax("GvDetail", objquo.editId, Detail_ID.ToString(), gvr, ref trns);
                        }
                        else
                        {
                            //when showing description
                            int Detail_ID = ObjSQuoteDetail.InsertQuotationDetail(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), objquo.editId, "0", gvr.hdnNewProductId,gvr.Description,gvr.Discount,gvr.UnitPrice,gvr.RequiredQuantity,gvr.TaxP,gvr.TaxV, gvr.PriceAfterTax,gvr.Discount,gvr.DiscountValue,gvr.TotalAmount, "False",gvr.txtgvAgentCommission,gvr.UnitId,gvr.UnitPrice,gvr.UnitId,gvr.UnitPrice,gvr.Description, "False", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                           // Insert_Tax("GvDetail", objquo.editId, Detail_ID.ToString(), gvr, ref trns);
                        }
                    }

                    //for insert record in folow up table

                    objFollowup.DeleteRecord_By_SquotationId(objquo.editId, ref trns);

                    DataTable dtFollowup = (DataTable)HttpContext.Current.Session["dtFollowup"];
                    if (dtFollowup != null)
                    {
                        foreach (DataRow dr in dtFollowup.Rows)
                        {
                            objFollowup.InsertRecord(objquo.editId, dr["Follow_Up_Date"].ToString(), dr["Follow_Up_Person"].ToString(), dr["Description"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        }
                    }
                   // DisplayMessage("Record Updated", "green");
                    result[0] = "Record Updated";
                    result[1] = objquo.editId;

                }
                else
                {
                    //DisplayMessage("Record  Not Updated");
                    result[0] = "Record  Not Updated";
                }
            }
            else
            {
                string IsApproved = "Pending";
                string SalesQuotationApproval = objInvParam.getParameterValueByParameterName("SalesQuotationApproval", ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                DataTable dt1 = new DataTable();
                string EmpPermission = string.Empty;
                if (SalesQuotationApproval != "")
                {
                    if (Convert.ToBoolean(SalesQuotationApproval) == true)
                    {
                        EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("SalesQuotation").Rows[0]["Approval_Level"].ToString();
                        dt1 = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "57", HttpContext.Current.Session["EmpId"].ToString());
                        if (dt1.Rows.Count == 0)
                        {
                            result[0] = "Approval setup issue , please contact to your admin";
                            //DisplayMessage("Approval setup issue , please contact to your admin");
                            trns.Rollback();
                            if (con.State == System.Data.ConnectionState.Open)
                            {
                                con.Close();
                            }
                            trns.Dispose();
                            con.Dispose();
                            return result;
                        }
                    }
                    else
                    {
                        IsApproved = "Approved";
                    }
                }
                if (objquo.Currency == "")
                {
                    objquo.Currency = "0";
                }
                if(objquo.txtTaxP==null|| objquo.txtTaxP == "")
                {
                    objquo.txtTaxP = "0";
                }
             
                if(objquo.txtTaxV==null|| objquo.txtTaxV == "")
                {
                    objquo.txtTaxV = "0";
                }
                if(objquo.ddlTransType==null|| objquo.ddlTransType == "")
                {
                    objquo.ddlTransType = "0";
                }
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
                string Emp_Code = objquo.txtEmployee.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                b = objSQuoteHeader.InsertQuotationHeader(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(),objquo.Quotation_No, ObjSysParam.getDateForInput(objquo.Quotation_Date).ToString(),objquo.hdnSalesInquiryId, objquo.Currency, Emp_ID, objquo.GrossAmount, objquo.txtTaxP, objquo.txtTaxV, objquo.txtDiscountP, objquo.txtDiscountV, objquo.ddlStatus, objquo.txtHeader, objquo.txtFooter, objquo.txtCondition1, "", "", strTemplateId, "", "False", strAgentId, objquo.txtReason, strContactId, Set_CustomerMaster.GetCustomerIdByCustomerName(objquo.txtShipCustomerName, ref trns), strAddressId, IsApproved.Trim(), "", "False", objquo.txtOrderCompletionDate, "True",HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), objquo.ddlTransType,"0", "0", ref trns);
                objquo.editId = b.ToString();
                if (b != 0)
                {
                    string strMaxId = string.Empty;
                    strMaxId = b.ToString();
                    if (objquo.Quotation_No == objquo.DocNo)
                    {
                        int dtCount = objSQuoteHeader.GetQuotationCount(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);

                        if (dtCount == 0)
                        {
                            objSQuoteHeader.Updatecode(b.ToString(), objquo.Quotation_No + "1", ref trns);
                            objquo.Quotation_No = objquo.Quotation_No + "1";
                        }
                        else
                        {
                            objSQuoteHeader.Updatecode(b.ToString(), objquo.Quotation_No + dtCount, ref trns);
                            objquo.Quotation_No = objquo.Quotation_No + dtCount;
                        }
                    }
                    objMailContact.InsertRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "SQ", strMaxId, strContactId, "TO", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());

                    if (strMaxId != "" && strMaxId != "0")
                    {

                        Quotationid = strMaxId;
                        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("SQ", strMaxId, ref trns);
                        foreach (var gvr in objquo.Products)
                        {
                            if (gvr.UnitPrice == "")
                            {
                                gvr.UnitPrice = "0";
                            }
                            if (gvr.TaxP == ""|| gvr.TaxP==null)
                            {
                                gvr.TaxP = "0";
                            }
                            if (gvr.TaxV == ""|| gvr.TaxV==null)
                            {
                                gvr.TaxV = "0";
                            }
                            if (gvr.PriceAfterTax == ""|| gvr.PriceAfterTax==null)
                            {
                                gvr.PriceAfterTax = "0";
                            }
                            if (gvr.Discount == ""|| gvr.Discount==null)
                            {
                                gvr.Discount = "0";
                            }
                            if (gvr.DiscountValue == ""|| gvr.DiscountValue==null)
                            {
                                gvr.DiscountValue = "0";
                            }
                            if (gvr.TotalAmount == ""|| gvr.TotalAmount==null)
                            {
                                gvr.TotalAmount = "0";
                            }

                            if (gvr.txtgvAgentCommission == ""|| gvr.txtgvAgentCommission==null)
                            {
                                gvr.txtgvAgentCommission = "0";
                            }

                            string IsProductNameShow = objInvParam.getParameterValueByParameterName("IsProductNameShow", ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                            if (Convert.ToBoolean(IsProductNameShow))
                            {
                                int Detail_ID = ObjSQuoteDetail.InsertQuotationDetail(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), strMaxId,"0",gvr.hdnNewProductId,gvr.Description,gvr.CurrencyId,gvr.UnitPrice,gvr.RequiredQuantity,gvr.TaxP,gvr.TaxV,gvr.GrossPrice,gvr.Discount,gvr.DiscountValue,gvr.TotalAmount, "False",gvr.txtgvAgentCommission,gvr.UnitId,gvr.UnitPrice,gvr.Description,gvr.UnitPrice,gvr.Description, "False", DateTime.Now.ToString(), "True",HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                               // Insert_Tax("GvDetail", strMaxId, Detail_ID.ToString(), gvr, ref trns);
                            }
                            else
                            {
                                int Detail_ID = ObjSQuoteDetail.InsertQuotationDetail(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(),HttpContext.Current.Session["LocId"].ToString(), strMaxId, "0",gvr.hdnNewProductId,gvr.Description,gvr.CurrencyId, gvr.UnitPrice, gvr.RequiredQuantity,gvr.TaxP, gvr.TaxV, gvr.GrossPrice, gvr.Discount, gvr.DiscountValue, gvr.TotalAmount, "False", gvr.txtgvAgentCommission, gvr.UnitId, gvr.UnitPrice, gvr.Description, gvr.UnitPrice, gvr.Description, "False", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                               // Insert_Tax("GvDetail", strMaxId, Detail_ID.ToString(), gvr, ref trns);
                            }

                        }

                        DataTable dtFollowup = (DataTable)HttpContext.Current.Session["dtFollowup"];
                        if (dtFollowup != null)
                        {

                            foreach (DataRow dr in dtFollowup.Rows)
                            {
                                objFollowup.InsertRecord(strMaxId, dr["Follow_Up_Date"].ToString(), dr["Follow_Up_Person"].ToString(), dr["Description"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            }
                        }



                        if (SalesQuotationApproval != "")
                        {
                            if (Convert.ToBoolean(SalesQuotationApproval) == true)
                            {

                                if (dt1.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt1.Rows.Count; j++)
                                    {
                                        string PriorityEmpId = dt1.Rows[j]["Emp_Id"].ToString();
                                        string IsPriority = dt1.Rows[j]["Priority"].ToString();
                                        int cur_trans_id = 0;
                                        if (EmpPermission == "1")
                                        {
                                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton("8", HttpContext.Current.Session["CompId"].ToString(), "0", "0", "0", HttpContext.Current.Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                        }
                                        else if (EmpPermission == "2")
                                        {
                                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton("8", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", "0", HttpContext.Current.Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                        }
                                        else if (EmpPermission == "3")
                                        {
                                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton("8", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                        }
                                        else
                                        {
                                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton("8", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), strMaxId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                        }

                                        // Insert Notification For Leave by  ghanshyam suthar
                                        HttpContext.Current.Session["PriorityEmpId"] = PriorityEmpId;
                                        HttpContext.Current.Session["cur_trans_id"] = cur_trans_id;
                                        HttpContext.Current.Session["Ref_ID"] = strMaxId.ToString();
                                        //Set_Notification();

                                    }
                                }

                            }
                        }//end approval
                    }
                   
                    result[0] = "Record Saved";
                    result[1] = strMaxId;
                }
                else
                {
                    result[0] = "Record  Not Saved";
                }
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            // used to update the sales stage of opportunity/ inquiry
            try
            {
                using (DataTable dt_inqno = objSIHeader.getInqIDFromQuotationID(objquo.editId))
                {
                    if (dt_inqno.Rows.Count > 0)
                        objSIHeader.UpdateSalesStageFromQuotation(dt_inqno.Rows[0][0].ToString(), objquo.editId);
                }
            }
            catch
            {

            }
           // obj.Reset();
          // obj.ResetProduct();
            //if (((Button)sender).ID == "btnSQuoteSaveandPrint")
            //{
            //    QuotationPrint(Quotationid);
            //}
            //AllPageCode();
        }
        catch (Exception ex)
        {
            //DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
            result[0] = (Common.ConvertErrorMessage(ex.Message.ToString(), ex));
            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return result;
        }

        return result;

    }
    
    public string GetApprovalStatus(int TransID)
    {
        objSQuoteHeader = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());

        return ((DataTable)(objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), TransID.ToString()))).Rows[0]["Field4"].ToString();
    }
    protected void ddlCurrency_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCurrency.SelectedIndex != 0)
        {
            ddlPCurrency.SelectedValue = ddlCurrency.SelectedValue;
        }

        using (DataTable dt = objDa.return_DataTable("select Sys_CurrencyMaster.Currency_Code,Sys_CurrencyMaster.field2 as smallestDenomiation,case when Sys_Country_Currency.field1 is null or Sys_Country_Currency.field1 ='' then '0' else Sys_Country_Currency.field1 end as decimal from Sys_CurrencyMaster left join Sys_Country_Currency on Sys_Country_Currency.Currency_Id = Sys_CurrencyMaster.Currency_ID where Sys_CurrencyMaster.Currency_Id ='" + ddlCurrency.SelectedValue + "'"))
        {
            hdnDecimalCount.Value = dt.Rows[0]["decimal"].ToString();
            if (hdnDecimalCount.Value == "")
            {
                hdnDecimalCount.Value = "2";
            }
        }
    }
    protected void txtAgentName_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtAgentName.Text != "")
        {
            strCustomerId = Set_CustomerMaster.GetCustomerIdByCustomerName(txtAgentName.Text, Session["DBConnection"].ToString());
            if (strCustomerId != "" && strCustomerId != "0")
            {
               // GvDetail.DataSource = GetProductDetailinDatatable();
               // GvDetail.DataBind();
                //GvDetail.Columns[GvDetail.Columns.Count - 1].Visible = true;
                // SetDecimal();
                //foreach (GridViewRow Row in GvDetail.Rows)
                //{
                //    TextBox txtgvAgentCommission = (TextBox)Row.FindControl("txtgvAgentCommission");

                //    txtgvAgentCommission.Text = GetAmountDecimal(txtgvAgentCommission.Text);

                //    try
                //    {
                //        DataTable dt = new DataView(GetProductDetailinDatatable(), "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                //        if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
                //        {
                //            ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
                //            ((Panel)Row.FindControl("PopupMenu")).Visible = false;
                //        }
                //    }
                //    catch
                //    {
                //    }
                //    // txtgvUnitPrice_TextChanged(((object)((TextBox)Row.FindControl("txtgvUnitPrice"))), e);

                //}
                HeadearCalculateGrid();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAgentName.Text = "";
                //GvDetail.DataSource = GetProductDetailinDatatable();
                //GvDetail.DataBind();
                //GvDetail.Columns[GvDetail.Columns.Count - 1].Visible = false;
                //SetDecimal();
                //foreach (GridViewRow Row in GvDetail.Rows)
                //{
                //    TextBox txtgvAgentCommission = (TextBox)Row.FindControl("txtgvAgentCommission");

                //    txtgvAgentCommission.Text = GetAmountDecimal(txtgvAgentCommission.Text);

                //    try
                //    {
                //        DataTable dt = new DataView(GetProductDetailinDatatable(), "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                //        if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
                //        {
                //            ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
                //            ((Panel)Row.FindControl("PopupMenu")).Visible = false;
                //        }
                //    }
                //    catch
                //    {
                //    }
                //    //txtgvUnitPrice_TextChanged(((object)((TextBox)Row.FindControl("txtgvUnitPrice"))), e);

                //}
                HeadearCalculateGrid();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAgentName);
                return;
            }
        }
        else
        {
            //GvDetail.DataSource = GetProductDetailinDatatable();
            //GvDetail.DataBind();
           // GvDetail.Columns[GvDetail.Columns.Count - 1].Visible = false;
            //SetDecimal();
            //foreach (GridViewRow Row in GvDetail.Rows)
            //{
            //    TextBox txtgvAgentCommission = (TextBox)Row.FindControl("txtgvAgentCommission");

            //    txtgvAgentCommission.Text = GetAmountDecimal(txtgvAgentCommission.Text);

            //    try
            //    {
            //        DataTable dt = new DataView(GetProductDetailinDatatable(), "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //        if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
            //        {
            //            ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
            //            ((Panel)Row.FindControl("PopupMenu")).Visible = false;
            //        }
            //    }
            //    catch
            //    {
            //    }
            //    //txtgvUnitPrice_TextChanged(((object)((TextBox)Row.FindControl("txtgvUnitPrice"))), e);

            //}
            HeadearCalculateGrid();
            txtAgentName.Text = "";
            txtAgentName.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAgentName);
        }
    }

    #region Bin Section
    protected void GvSalesQuoteBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesQuoteBin.PageIndex = e.NewPageIndex;

        using (DataTable dt = (DataTable)Session["dtFilterSQuotationBin"])
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesQuoteBin, dt, "", "");
        }
        string temp = string.Empty;

        for (int i = 0; i < GvSalesQuoteBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvSalesQuoteBin.Rows[i].FindControl("hdnTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvSalesQuoteBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        //AllPageCode();
    }
    protected void GvSalesQuoteBin_OnSorting(object sender, GridViewSortEventArgs e)
    {

        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;

        if (GvSalesQuoteBin.Attributes["CurrentSortField"] != null &&
            GvSalesQuoteBin.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvSalesQuoteBin.Attributes["CurrentSortField"])
            {
                if (GvSalesQuoteBin.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvSalesQuoteBin.Attributes["CurrentSortField"] = sortField;
        GvSalesQuoteBin.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGridBin(Int32.Parse(hdnGvSalesQuotationCurrentPageIndexBin.Value));

    }
    public void FillGridBin(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0 && txtValueBin.Text != string.Empty)
        {
            if (ddlOptionBin.SelectedIndex == 1)
            {
                strSearchCondition = ddlFieldNameBin.SelectedValue + "='" + txtValueBin.Text.Trim() + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                strSearchCondition = ddlFieldNameBin.SelectedValue + " like '%" + txtValueBin.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlFieldNameBin.SelectedValue + " Like '" + txtValueBin.Text.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "isActive='false' ";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }

        if (ddlLocation.SelectedIndex > 0)
        {
            strWhereClause = strWhereClause + " and location_id='" + ddlLocation.SelectedValue.Trim() + "'";
        }
        else
        {
            strWhereClause = strWhereClause + " and location_id in(" + ddlLocation.SelectedValue.Trim() + ")";
        }



        int totalRows = 0;
        using (DataTable dt = objSQuoteHeader.getQuotationList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvSalesQuoteBin.Attributes["CurrentSortField"], GvSalesQuoteBin.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvSalesQuoteBin, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                GvSalesQuoteBin.DataSource = null;
                GvSalesQuoteBin.DataBind();
                lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : 0";
            }

            PageControlCommon.PopulatePager(rptPager_BIN, totalRows, currentPageIndex);
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "Quotation_Date")
        {
            if (txtValueBinDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueBinDate.Text);
                    txtValueBin.Text = Convert.ToDateTime(txtValueBinDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueBinDate.Text = "";
                    txtValueBin.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBinDate);
                    return;
                }

            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueBinDate.Focus();
                return;
            }
        }

        FillGridBin(1);
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        if (GvSalesQuoteBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        b = objSQuoteHeader.DeleteQuotationHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                //FillGrid(1);
                FillGridBin();
                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activated");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvSalesQuoteBin.Rows)
                {
                    CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                    if (chk.Checked)
                    {
                        fleg = 1;
                    }
                    else
                    {
                        fleg = 0;
                    }
                }
                if (fleg == 0)
                {
                    DisplayMessage("Please Select Record");
                }
                else
                {
                    DisplayMessage("Record Not Activated");
                }
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        //AllPageCode();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvSalesQuoteBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvSalesQuoteBin.Rows.Count; i++)
        {
            ((CheckBox)GvSalesQuoteBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvSalesQuoteBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvSalesQuoteBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvSalesQuoteBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        HiddenField lb = (HiddenField)GvSalesQuoteBin.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)GvSalesQuoteBin.Rows[index].FindControl("chkSelect")).Checked)
        {
            empidlist += lb.Value.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Value.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectedRecord.Text = temp;
        }
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;
        FillGridBin(1);
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        //AllPageCode();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtFilterSQuotationBin"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["SQuotation_Id"]))
                {
                    lblSelectedRecord.Text += dr["SQuotation_Id"] + ",";
                }
            }
            for (int i = 0; i < GvSalesQuoteBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvSalesQuoteBin.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvSalesQuoteBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtFilterSQuotationBin"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesQuoteBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlLocation.SelectedItem.ToString() == "All")
        {
            DisplayMessage("Please Select One Location");
            return;
        }
        //in use
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objSQuoteHeader.DeleteQuotationHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                }
            }
        }

        if (b != 0)
        {
            //FillGrid(1);
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvSalesQuoteBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                if (chk.Checked)
                {
                    fleg = 1;
                }
                else
                {
                    fleg = 0;
                }
            }
            if (fleg == 0)
            {
                DisplayMessage("Please Select Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }
    }
    #endregion
    #endregion
    #region User defined Function



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] btnbindrpt_Click(string ddlLocation,string ddlUser,string ddlPosted,string ddlFieldName,string ddlOption,string txtValue)
    {
        UserMaster ObjUser = new UserMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string []result = new string[2];
        Inv_SalesQuotationHeader objSQuoteHeader = new Inv_SalesQuotationHeader(HttpContext.Current.Session["DBConnection"].ToString());
        int currentPageIndex = 1;
        string strSearchCondition = string.Empty;
        if (ddlOption != "0" && txtValue != string.Empty)
        {
            if (ddlOption == "Contains")
            {
                strSearchCondition = ddlFieldName + "='" + txtValue.Trim() + "'";
            }
            else if (ddlOption == "Like")
            {
                strSearchCondition = ddlFieldName + " like '%" + txtValue.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlFieldName + " Like '" + txtValue.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "isActive='true' ";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }

        if (ddlUser != "--Select User--")
        {
            strWhereClause = strWhereClause + " and CreatedBy='" + ddlUser.ToString() + "'";
        }
        else
        {
            bool isSingleUser = false;
            if (HttpContext.Current.Session["EmpId"].ToString() != "0")
            {
                isSingleUser = bool.Parse(ObjUser.CheckIsSingleUser(HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString()));
            }

            if (isSingleUser == true)
            {
                string strAllUser = string.Empty;
                //foreach (ListItem item in ddlUser.Items)
                //{
                //    if (item.Value == "--Select User--")
                //    {
                //        continue;
                //    }
                //    strAllUser += "," + item.Value.ToString();
                //}
                if (!string.IsNullOrEmpty(ddlUser))
                {
                    strAllUser = strAllUser.Substring(1, strAllUser.Length - 1);
                    strWhereClause = strWhereClause + " and  case when CreatedBy = 'superadmin' then 0 else CreatedBy end in(" + strAllUser + ")";
                }
            }
        }

        if (ddlPosted != "0")
        {
            strWhereClause = strWhereClause + " and Status='" + ddlPosted.Trim() + "'";
        }

        if (int.Parse(ddlLocation) >0)
        {
            strWhereClause = strWhereClause + " and location_id='" + ddlLocation.Trim() + "'";
        }
        else
        {
            strWhereClause = strWhereClause + " and location_id in(" + ddlLocation.Trim() + ")";
        }

        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = objSQuoteHeader.getQuotationList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), "Quotation_Date", "DESC", strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {

                string JSONresult = JsonConvert.SerializeObject(dt);
                result[0] = JSONresult;
                return result;
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
               

            }
            else
            {

               
            }
            
        }
        return result;

    }
    //protected void Page_Changed(object sender, EventArgs e)
    //{
    //    int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
    //    hdnGvSalesQuotationCurrentPageIndex.Value = pageIndex.ToString();
    //    FillGrid(pageIndex);
    //}
    protected void PageBin_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvSalesQuotationCurrentPageIndexBin.Value = pageIndex.ToString();
        FillGridBin(pageIndex);
    }
    private void FillGrid()
    {
        if (Request.QueryString["ReminderID"] != null)
        {
           // fillGrid(Request.QueryString["ReminderID"].ToString());
            return;
        }

        DataTable dt = objSQuoteHeader.GetQuotationHeaderAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (ddlUser.SelectedValue != "--Select User--")
        {
            dt = new DataView(dt, "CreatedBy='" + ddlUser.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (ddlPosted.SelectedIndex != 0)
        {
            try
            {
                dt = new DataView(dt, "Status='" + ddlPosted.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }

        Session["dtSQuotation"] = dt;
        Session["dtFilterSQuotation"] = dt;

        //if (dt != null && dt.Rows.Count > 0)
        //{//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //    objPageCmn.FillData((object)GvSalesQuote, dt, "", "");

        //}
        //else
        //{
        //    GvSalesQuote.DataSource = null;
        //    GvSalesQuote.DataBind();
        //}
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

        object sumObject;
        sumObject = dt.Compute("Sum(TotalAmount)", "");




        lblTotal.Text = "Total Amount = " + GetCurrencySymbol(GetAmountDecimal(sumObject.ToString()), Session["LocCurrencyId"].ToString());

        //AllPageCode();
    }
    //private void fillGrid(string id)
    ////{
    ////    //DataTable dt = objSQuoteHeader.GetQuotationHeaderAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
    ////    DataTable dt = objSQuoteHeader.GetQuotationDtlsForReminderByID(id);
    ////    //dt = new DataView(dt, "SQuotation_Id="+id, "", DataViewRowState.CurrentRows).ToTable();

    ////    Session["dtSQuotation"] = dt;
    ////    Session["dtFilterSQuotation"] = dt;

    ////    if (dt != null && dt.Rows.Count > 0)
    ////    {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
    ////        objPageCmn.FillData((object)GvSalesQuote, dt, "", "");

    ////    }
    ////    else
    ////    {
    ////        GvSalesQuote.DataSource = null;
    ////        GvSalesQuote.DataBind();
    ////    }
    ////    lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
    ////    ddlPosted.Enabled = false;
    ////    btnRefresh.Enabled = false;

    //} 



    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    private string GetContactId()
    {
        string retval = string.Empty;
        if (txtContactName.Text != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txtContactName.Text.Trim().Split('/')[0].ToString().Trim());
            if (dtSupp.Rows.Count > 0)
            {
                retval = (txtContactName.Text.Split('/'))[txtContactName.Text.Split('/').Length - 1];

                DataTable dtCompany = objContact.GetContactTrueById(retval);
                if (dtCompany.Rows.Count > 0)
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

    private string JsContactId( string txtContactName)
    {
        string retval = string.Empty;
        Ems_ContactMaster objContact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        if (txtContactName != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txtContactName.Trim().Split('/')[0].ToString().Trim());
            if (dtSupp.Rows.Count > 0)
            {
                retval = (txtContactName.Split('/'))[txtContactName.Split('/').Length - 1];

                DataTable dtCompany = objContact.GetContactTrueById(retval);
                if (dtCompany.Rows.Count > 0)
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




    private string GetContactId(ref SqlTransaction trns)
    {
        string retval = string.Empty;
        if (txtContactName.Text != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txtContactName.Text.Trim().Split('/')[0].ToString().Trim(), ref trns);
            if (dtSupp.Rows.Count > 0)
            {
                retval = (txtContactName.Text.Split('/'))[txtContactName.Text.Split('/').Length - 1];

                DataTable dtCompany = objContact.GetContactTrueById(retval, ref trns);
                if (dtCompany.Rows.Count > 0)
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
 
    #endregion
    #region Add Product Concept
    protected string GetProductName(string strProductId)
    {
        Inv_ProductMaster objProductM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string strProductName = string.Empty;
        if (strProductId != "0" && strProductId != "")
        {
            using (DataTable dtPName = objProductM.GetProductMasterById(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()))
            {
                if (dtPName.Rows.Count > 0)
                {
                    strProductName = dtPName.Rows[0]["EProductName"].ToString();
                }
            }
        }
        else
        {
            strProductName = "";
        }
        return strProductName;
    }
    //updated by varsha 
    public string SuggestedProductName(string ProductId, string SerialNo)
    {

        string Product_Name = string.Empty;

        if (editid.Value == "")
        {

            DataTable dtDetail = ((DataTable)Session["dtQuotationDetail"]);


            try
            {
                dtDetail = new DataView(dtDetail, "Product_Id=" + ProductId + " and Serial_No=" + SerialNo + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {


            }


            if (!Convert.ToBoolean(objInvParam.getParameterValueByParameterName("IsProductNameShow", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString())))
            {
                Product_Name = dtDetail.Rows[0]["ProductDescription"].ToString();
            }
            else
            {
                Product_Name = dtDetail.Rows[0]["SuggestedProductName"].ToString();
            }

        }
        else
        {
            DataTable dtSquotationDetail = ((DataTable)Session["dtQuotationDetail"]);

            try
            {
                dtSquotationDetail = new DataView(dtSquotationDetail, "Product_Id=" + ProductId + "  and Serial_No=" + SerialNo + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            if (!Convert.ToBoolean(objInvParam.getParameterValueByParameterName("IsProductNameShow", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString())))
            {
                Product_Name = dtSquotationDetail.Rows[0]["ProductDescription"].ToString();
            }
            else
            {
                Product_Name = dtSquotationDetail.Rows[0]["SuggestedProductName"].ToString();
            }
        }

        return Product_Name;
    }

    public string getShortSuggestedProductName(string ProductId, string SerialNo)
    {
        string Product_Name = string.Empty;
        Product_Name = SuggestedProductName(ProductId, SerialNo);

        if (Product_Name.Length > 16)
        {
            Product_Name = Product_Name.Substring(0, 15) + "...";
        }
        return Product_Name;
    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;

        using (DataTable dt = objProductM.GetProductMasterById(HttpContext.Current.Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()))
        {
            if (dt.Rows.Count != 0)
            {
                ProductName = dt.Rows[0]["ProductCode"].ToString();
            }
            else
            {
                ProductName = "0";
            }
        }

        return ProductName;

    }
    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string CurrencyAmount = string.Empty;
        try
        {
            CurrencyAmount = SystemParameter.GetCurrencySmbol(CurrencyId, Amount, Session["DBConnection"].ToString());
        }
        catch
        {
        }
        return CurrencyAmount;
    }
    protected string GetSalesInquiryNo(string strSalesInquiryId)
    {
        string strSalesInquiryNo = string.Empty;
        if (strSalesInquiryId != "0" && strSalesInquiryId != "")
        {
            using (DataTable dtSINo = objSIHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strSalesInquiryId))
            {
                if (dtSINo.Rows.Count > 0)
                {
                    strSalesInquiryNo = dtSINo.Rows[0]["SInquiryNo"].ToString();
                }
            }
        }
        else
        {
            strSalesInquiryNo = "";
        }
        return strSalesInquiryNo;
    }
    protected string GetUnitName(string strUnitId)
    {
        Inv_UnitMaster UM = new Inv_UnitMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string strUnitName = string.Empty;
        if (strUnitId != "0" && strUnitId != "")
        {
            using (DataTable dtUName = UM.GetUnitMasterById(Session["CompId"].ToString(), strUnitId))
            {
                if (dtUName.Rows.Count > 0)
                {
                    strUnitName = dtUName.Rows[0]["Unit_Name"].ToString();
                }
            }
        }
        else
        {
            strUnitName = "";
        }
        return strUnitName;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetCurrencyNames(string CurrencyId)
    {
        if (CurrencyId == "--Select--")
        {
            CurrencyId = "0";
        }
        CurrencyMaster objCurrency = new CurrencyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string strCurrencyName = string.Empty;
        if (CurrencyId != "0" && CurrencyId != "")
        {
            using (DataTable dtCName = objCurrency.GetCurrencyMasterById(CurrencyId))
            {
                if (dtCName.Rows.Count > 0)
                {
                    strCurrencyName = dtCName.Rows[0]["Currency_Code"].ToString();
                }
                else
                {
                    strCurrencyName = "";
                }
            }
        }
        else
        {
            using (DataTable dtCName = objCurrency.GetCurrencyMasterById(HttpContext.Current.Session["LocCurrencyId"].ToString()))
            {
                if (dtCName.Rows.Count > 0)
                {
                    strCurrencyName = dtCName.Rows[0]["Currency_Code"].ToString();
                }
                else
                {
                    strCurrencyName = "";
                }
            }
        }
        return strCurrencyName;
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        CurrencyMaster objCurrency = new CurrencyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            using (DataTable dtCName = objCurrency.GetCurrencyMasterById(strCurrencyId))
            {
                if (dtCName.Rows.Count > 0)
                {
                    strCurrencyName = dtCName.Rows[0]["Currency_Code"].ToString();
                }
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    #endregion
    #region Add Request Section
    private void FillCurrency()
    {
        using (DataTable dsCurrency = objCurrency.GetCurrencyMaster())
        {
            if (dsCurrency.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");
                objPageCmn.FillData((object)ddlPCurrency, dsCurrency, "Currency_Name", "Currency_ID");
            }
            else
            {
                ddlCurrency.Items.Add("--Select--");
                ddlCurrency.SelectedValue = "--Select--";
                ddlPCurrency.Items.Add("--Select--");
                ddlPCurrency.SelectedValue = "--Select--";
            }
        }
    }
    protected string GetEmployeeName(string strEmployeeId)
    {
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            strEmployeeName = objEmployee.GetEmployeeNameByEmployeeId(strEmployeeId, Session["CompId"].ToString(), Session["BrandId"].ToString());
        }
        else
        {
            strEmployeeName = "";
        }
        return strEmployeeName;
    }
    protected string GetCustomerName(string strCustomerId)
    {
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        string strCustomerName = string.Empty;
        if (strCustomerId != "0" && strCustomerId != "")
        {
            strCustomerName = objContact.GetContactNameByContactiD(strCustomerId);
        }
        else
        {
            strCustomerName = "";
        }
        return strCustomerName;
    }
    protected string GetCustomerContactNo(string strCustomerId)
    {
        string strCustomerContactNo = string.Empty;
        if (strCustomerId != "0" && strCustomerId != "")
        {
            using (DataTable dtCName = objContact.GetContactTrueById(strCustomerId))
            {
                if (dtCName.Rows.Count > 0)
                {
                    strCustomerContactNo = dtCName.Rows[0]["Field2"].ToString();
                }
            }
        }
        else
        {
            strCustomerContactNo = "";
        }
        return strCustomerContactNo;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public string GetCustomerAdderss(string strCustomerId)
    {
        string strCustomerContactNo = string.Empty;
        using (DataTable dtCName = objContact.GetContactTrueById(strCustomerId))
        {
            if (dtCName.Rows.Count > 0)
            {
                strCustomerContactNo = dtCName.Rows[0]["Field2"].ToString();
            }
        }
        return strCustomerContactNo;
    }
    public class clsQtoHeader
    {
        public string ProductId { get; set; }
        public string hdnNewProductId { get; set; }
        public string ChkQuatationDetail { get; set; }
        public string ProductName { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string RequiredQuantity { get; set; }
        public string UnitPrice { get; set; }
        public string GrossPrice { get; set; }
        public string Discount { get; set; }
        public string DiscountValue { get; set; }
        public string Description { get; set; }
        public string TotalAmount { get; set; }
        public string TaxP { get; set; }
        public string TaxV { get; set; }
        public string PriceAfterTax { get; set; }
        public string txtgvAgentCommission { get; set; }
    }
    public class objSalesQta
    {
        public string editId { get; set; }
        public string hdnSalesInquiryId { get; set; }
        public string TimeStamp { get; set; }
        public string ddlTransType { get; set; }
        public string  txtTaxV { get; set; }
        public string txtTaxP { get; set; }
        public string ApprovalStatus { get; set; }
        public string DocNo { get; set; }

        public string Quotation_Date { get; set; }
        public string Quotation_No { get; set; }
        public string txtCustomer { get; set; }
        public string txtContactName { get; set; }
        public string txtContactNo { get; set; }
        public string txtShipCustomerName { get; set; }
        public string txtShipingAddress { get; set; }
        public string txtInquiryNo { get; set; }
        public string txtInquiryDate { get; set; }
        public string txtOrderCompletionDate { get; set; }
        public string Currency { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyText { get; set; }
        public string txtEmployee { get; set; }
        public string ddlStatus { get; set; }
        public string txtReason { get; set; }
        public string txtAgentName { get; set; }
        public string txtTemplateName { get; set; }
        public string GrossAmount { get; set; }
        public string txtDiscountP { get; set; }
        public string txtDiscountV { get; set; }
        public string txtTotalAmount { get; set; }
        public string txtHeader { get; set; }
        public string txtFooter { get; set; }
        public string txtCondition1 { get; set; }
        public List<clsQtoHeader> Products { get; set; }
      
    }
    public void fillInquiryRecordOnQuatation(string strRequestId, string locationid)
    {
        Hdn_Get_Inquity.Value = strRequestId;
        DataTable dtPRequest = objSIHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, strRequestId);
        //FillTransactionType();
        if (dtPRequest.Rows.Count > 0)
        {
            txtInquiryNo.Text = dtPRequest.Rows[0]["SInquiryNo"].ToString();
            txtInquiryDate.Text = Convert.ToDateTime(dtPRequest.Rows[0]["IDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            ddlCurrency.SelectedValue = dtPRequest.Rows[0]["Currency_Id"].ToString();
            ddlPCurrency.SelectedValue = ddlCurrency.SelectedValue;
            string strEmployeeId = dtPRequest.Rows[0]["HandledEmpID"].ToString();
            txtOrderCompletionDate.Text = Convert.ToDateTime(dtPRequest.Rows[0]["OrderCompletiondate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(strEmployeeId);
            txtEmployee.Text = GetEmployeeName(strEmployeeId) + "/" + Emp_Code;
            txtCustomer.Text = GetCustomerName(dtPRequest.Rows[0]["Customer_Id"].ToString());
            hdnCustomerId.Value = dtPRequest.Rows[0]["Customer_Id"].ToString();
            Session["ContactID"] = dtPRequest.Rows[0]["Customer_Id"].ToString();
            txtContactName.Text = GetCustomerName(dtPRequest.Rows[0]["Field2"].ToString()) + "/" + dtPRequest.Rows[0]["Field2"].ToString();
            txtContactNo.Text = GetCustomerContactNo(dtPRequest.Rows[0]["Field2"].ToString());
            if (txtContactNo.Text.Trim() == "")
            {
                txtContactNo.Text = GetCustomerContactNo(dtPRequest.Rows[0]["Customer_Id"].ToString());
            }
            DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "Terms & Condition(Sales Quotation)");
            if (Dt.Rows.Count > 0)
            {
                 txtCondition1.Content  = Dt.Rows[0]["Field1"].ToString();
            }
            else
            {
                 txtCondition1.Content  = dtPRequest.Rows[0]["Condition1"].ToString();
            }

            DataTable DtSQHeaderPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "SalesQuotationHeader");
            if (DtSQHeaderPara.Rows.Count > 0)
            {
                 txtHeader.Content  = DtSQHeaderPara.Rows[0]["Field1"].ToString();

            }
            DtSQHeaderPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, "Sales Quotation Footer");
            if (DtSQHeaderPara.Rows.Count > 0)
            {
               txtFooter.Content = DtSQHeaderPara.Rows[0]["Field1"].ToString();
            }
            DtSQHeaderPara.Dispose();
            hdnSalesInquiryId.Value = dtPRequest.Rows[0]["SInquiryID"].ToString();
            lblAmount.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Gross Total", Session["DBConnection"].ToString());
            //  Label2.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value");
            //Label3.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Value");

            lblTotalAmount.Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Net Total", Session["DBConnection"].ToString());
            //Add Detail Grid
            DataTable dtDetail = ObjSIDetail.GetSIDetailBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), locationid, strRequestId, Session["FinanceYearId"].ToString());
            dtDetail.Columns.Add("SalesPrice", typeof(string));
            ViewState["Customer_Id"] = dtPRequest.Rows[0]["Customer_Id"].ToString();

            dtDetail.Columns.Add("TaxPercent");
            dtDetail.Columns.Add("TaxValue");
            dtDetail.Columns.Add("PriceAfterTax");
            dtDetail.Columns.Add("DiscountPercent");
            dtDetail.Columns.Add("DiscountValue");
            dtDetail.Columns.Add("PriceAfterDiscount");
            dtDetail.Columns.Add("AgentCommission");
            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                try
                {
                    ProductIds = dtDetail.Rows[i]["Product_Id"].ToString();
                    dtDetail.Rows[i]["SalesPrice"] = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), locationid, "C", ViewState["Customer_Id"].ToString(), ProductIds, Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();

                    dtDetail.Rows[i]["SalesPrice"] = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), dtDetail.Rows[i]["SalesPrice"].ToString());

                }
                catch
                {
                    dtDetail.Rows[i]["SalesPrice"] = "0";
                }
                Add_Tax_In_Session(dtDetail.Rows[i]["SalesPrice"].ToString(), dtDetail.Rows[i]["Product_Id"].ToString(), dtDetail.Rows[i]["Serial_No"].ToString());
                dtDetail.Rows[i]["TaxPercent"] = Get_Tax_Percentage(dtDetail.Rows[i]["Product_Id"].ToString(), dtDetail.Rows[i]["Serial_No"].ToString());
                dtDetail.Rows[i]["TaxValue"] = Get_Tax_Amount(dtDetail.Rows[i]["SalesPrice"].ToString(), dtDetail.Rows[i]["Product_Id"].ToString(), dtDetail.Rows[i]["Serial_No"].ToString());
                dtDetail.Rows[i]["PriceAfterTax"] = "0";
                dtDetail.Rows[i]["DiscountPercent"] = "0";
                dtDetail.Rows[i]["DiscountValue"] = "0";
                dtDetail.Rows[i]["PriceAfterDiscount"] = "0";
                dtDetail.Rows[i]["AgentCommission"] = "0";

            }

            //if (dtDetail.Rows.Count > 0)
            //{
            //    List<clsQtoHeader> ObjList = new List<clsQtoHeader>();
            //    for(int i = 0; i < dtDetail.Rows.Count; i++)
            //    {
            //        clsQtoHeader obj = new clsQtoHeader();
            //        obj.ProductId = ProductCode(dtDetail.Rows[i]["Product_Id"].ToString());
            //        obj.hdnNewProductId= dtDetail.Rows[i]["Product_Id"].ToString();                    
            //        obj.ProductName = dtDetail.Rows[i]["SuggestedProductName"].ToString();
            //        obj.RequiredQuantity = dtDetail.Rows[i]["Quantity"].ToString();
            //        obj.UnitId = dtDetail.Rows[i]["UnitId"].ToString();
            //        obj.UnitName = GetUnitName(dtDetail.Rows[i]["UnitId"].ToString());
            //        obj.UnitPrice = dtDetail.Rows[i]["SalesPrice"].ToString();
            //        obj.Discount = dtDetail.Rows[i]["DiscountPercent"].ToString();
            //        obj.DiscountValue = dtDetail.Rows[i]["DiscountValue"].ToString();
            //        obj.TotalAmount = dtDetail.Rows[i]["SalesPrice"].ToString();
            //        obj.CurrencyId = dtDetail.Rows[i]["Currency_Id"].ToString();
            //        obj.CurrencyName = GetCurrencyName(dtDetail.Rows[i]["Currency_Id"].ToString());
            //        obj.Description = dtDetail.Rows[i]["ProductDescription"].ToString();

            //        ObjList.Add(obj);
            //    }
            //    var json = JsonConvert.SerializeObject(ObjList);
            //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "QuationByInquiry(" + json + ")", true);
            //}
            Session["dtQuotationDetail"] = dtDetail;          
            dtDetail.Dispose();
            HeadearCalculateGrid();
            rbtnFormView.Visible = true;
            rbtnAdvancesearchView.Visible = true;
            rbtnFormView.Checked = true;
            rbtnAdvancesearchView.Checked = false;
            rbtnFormView_OnCheckedChanged(null, null);
        }
        dtPRequest.Dispose();
    }
  
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = Session["LocCurrencyId"].ToString();
        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (Convert.ToDouble(strExchangeRate) * Convert.ToDouble(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    protected void Get_Tax_Parameter()
    {
        DataTable Dt_Parameter = ObjSysParam.GetSysParameterByParamName("Tax System");
        if (Dt_Parameter != null && Dt_Parameter.Rows.Count > 0)
        {
            if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Company")
            {
                Hdn_Tax_By.Value = "Company";
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Location")
            {
                Hdn_Tax_By.Value = "Location";
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "System")
            {
                Hdn_Tax_By.Value = "System";
            }
            else
            {
                Hdn_Tax_By.Value = "Select";
            }
        }
    }
    protected void GvDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //AllPageCode();
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Edit;
            cell.ColumnSpan = 1;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Delete;
            cell.ColumnSpan = 1;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.S_No_;
            cell.ColumnSpan = 1;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 5;
            cell.Text = Resources.Attendance.Product_Detail;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Unit;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Gross_Price;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Discount;

            if (Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
            {
                cell.ColumnSpan = 0;
            }
            else
            {
                row.Controls.Add(cell);
            }


            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Tax;

            if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
            {
                cell.ColumnSpan = 0;
            }
            else
            {
                row.Controls.Add(cell);
            }

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Line_total;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = "Stock";
            row.Controls.Add(cell);


            if (txtAgentName.Text.Trim() != "")
            {
                cell = new TableHeaderCell();
                cell.ColumnSpan = 1;
                cell.Text = "Commission";
                row.Controls.Add(cell);
            }

            gvProduct.Controls[0].Controls.Add(row);


        }
    }
    #endregion
    #region Calculations
    protected void txtTaxP_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }



        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", "", txtDiscountV.Text, false, Session["LocCurrencyId"].ToString(), true, Session["DBConnection"].ToString());
        txtTaxV.Text = str[4].ToString();
        txtTotalAmount.Text = str[5].ToString();
        txtPriceAfterTax.Text = str[5].ToString();
        TaxDiscountFromHeader(false);


    }

    protected void txtTaxV_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", "", txtTaxV.Text, "", txtDiscountV.Text, false, Session["LocCurrencyId"].ToString(), true, Session["DBConnection"].ToString());
        txtTaxP.Text = str[3].ToString();
        txtTotalAmount.Text = str[5].ToString();
        txtPriceAfterTax.Text = str[5].ToString();


        TaxDiscountFromHeader(false);
    }
    protected void txtDiscountP_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", txtDiscountP.Text, "", false, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());
        txtTaxV.Text = str[4].ToString();

        txtDiscountV.Text = str[2].ToString();

        txtTotalAmount.Text = str[5].ToString();
        txtPriceAfterTax.Text = str[5].ToString();

        TaxDiscountFromHeader(true);

    }
    protected void txtDiscountV_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }


        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", "", txtDiscountV.Text, false, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());
        txtTaxV.Text = str[4].ToString();
        txtDiscountP.Text = str[1].ToString();

        txtTotalAmount.Text = str[5].ToString();
        txtPriceAfterTax.Text = str[5].ToString();

        TaxDiscountFromHeader(true);
    }

    #endregion
    #region Grid Calculations
    public void TaxDiscountFromHeader(bool IsDiscount)
    {


        double netPrice = 0;
        double netTax = 0;
        double netDiscount = 0;


        //foreach (GridViewRow Row in GvDetail.Rows)
        //{
        //    TextBox lblgvQuantity = (TextBox)Row.FindControl("lblgvQuantity");
        //    string[] str;
        //    if (IsDiscount)
        //    {


        //        str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("txtgvUnitPrice")).Text, lblgvQuantity.Text, ((TextBox)Row.FindControl("txtgvTaxP")).Text, "", txtDiscountP.Text, "", true, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());

        //        ((TextBox)Row.FindControl("txtgvTaxV")).Text = str[4].ToString();
        //        ((TextBox)Row.FindControl("txtgvDiscountV")).Text = str[2].ToString();
        //        ((TextBox)Row.FindControl("txtgvDiscountP")).Text = str[1].ToString();
        //        ((TextBox)Row.FindControl("txtgvPriceAfterTax")).Text = str[5].ToString();
        //        ((TextBox)Row.FindControl("txtgvTotal")).Text = str[5].ToString();
        //    }
        //    else
        //    {

        //        str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("txtgvUnitPrice")).Text, lblgvQuantity.Text, txtTaxP.Text, "", "", ((TextBox)Row.FindControl("txtgvDiscountV")).Text, true, Session["LocCurrencyId"].ToString(), true, Session["DBConnection"].ToString());


        //        ((TextBox)Row.FindControl("txtgvTaxV")).Text = str[4].ToString();
        //        ((TextBox)Row.FindControl("txtgvTaxP")).Text = str[3].ToString();
        //        ((TextBox)Row.FindControl("txtgvPriceAfterTax")).Text = str[5].ToString();
        //        ((TextBox)Row.FindControl("txtgvTotal")).Text = str[5].ToString();

        //    }

        //}





    }
    protected void lblgvQuantity_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)row.FindControl("lblgvQuantity");
        lblgvQuantity.Text = ((TextBox)sender).Text;
        HeadearCalculateGrid();
    }
    protected void txtgvUnitPrice_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;
        TextBox txtgvUnitPrice = (TextBox)row.FindControl("txtgvUnitPrice");
        txtgvUnitPrice.Text = ((TextBox)sender).Text;
        HeadearCalculateGrid();
    }
    protected void txtgvTaxP_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtgvTaxV_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txtgvDiscountP_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;
        TextBox txtgvDiscountP = (TextBox)row.FindControl("txtgvDiscountP");
        txtgvDiscountP.Text = ((TextBox)sender).Text;
        HeadearCalculateGrid();

    }
    protected void txtgvDiscountV_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;
        TextBox txtgvUnitPrice = (TextBox)row.FindControl("txtgvUnitPrice");
        TextBox txtgvDiscountV = (TextBox)row.FindControl("txtgvDiscountV");
        TextBox txtgvDiscountP = (TextBox)row.FindControl("txtgvDiscountP");

        txtgvDiscountV.Text = ((TextBox)sender).Text;
        if (txtgvUnitPrice.Text == "")
            txtgvUnitPrice.Text = "0";
        if (txtgvDiscountV.Text == "")
            txtgvDiscountV.Text = "0";
        txtgvDiscountP.Text = Get_Discount_Percentage(txtgvUnitPrice.Text, txtgvDiscountV.Text).ToString();
        HeadearCalculateGrid();

    }
    public void SetDecimal()
    {
        //foreach (GridViewRow gvr in GvDetail.Rows)
        //{


        //    Label lblgvEstimatedUnitPrice = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");
        //    TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
        //    TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
        //    TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        //    TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
        //    TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
        //    TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
        //    TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
        //    TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
        //    TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
        //    TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");
        //    if (lblgvQuantity.Text == "")
        //    {
        //        lblgvQuantity.Text = "0";
        //    }
        //    if (txtgvUnitPrice.Text == "")
        //    {
        //        txtgvUnitPrice.Text = "0";
        //    }
        //    txtgvQuantityPrice.Text = GetAmountDecimal((Convert.ToDouble(lblgvQuantity.Text) * Convert.ToDouble(txtgvUnitPrice.Text)).ToString());
        //    lblgvEstimatedUnitPrice.Text = GetAmountDecimal(lblgvEstimatedUnitPrice.Text);
        //    lblgvQuantity.Text = GetAmountDecimal(lblgvQuantity.Text);
        //    txtgvUnitPrice.Text = GetAmountDecimal(txtgvUnitPrice.Text);
        //    txtgvDiscountP.Text = GetAmountDecimal(txtgvDiscountP.Text);
        //    txtgvDiscountV.Text = GetAmountDecimal(txtgvDiscountV.Text);
        //    txtgvTaxP.Text = GetAmountDecimal(txtgvTaxP.Text);
        //    txtgvTaxV.Text = GetAmountDecimal(txtgvTaxV.Text);
        //    txtgvTotal.Text = GetAmountDecimal(txtgvTotal.Text);


        //}
        txtAmount.Text = GetRoundValue(GetAmountDecimal(txtAmount.Text));
        txtTaxP.Text = GetAmountDecimal(txtTaxP.Text);
        txtDiscountP.Text = GetAmountDecimal(txtDiscountP.Text);
        txtDiscountV.Text = GetRoundValue(GetAmountDecimal(txtDiscountV.Text));
        txtTaxV.Text = GetRoundValue(GetAmountDecimal(txtTaxV.Text));
        txtTotalAmount.Text = GetRoundValue(GetAmountDecimal(txtTotalAmount.Text));
    }
    public string GetAmountDecimal(string Amount)
    {
        return SystemParameter.GetAmountWithDecimal(Amount, hdnDecimalCount.Value);

    }
    #endregion
    public string getStatus(string Status)
    {
        if (Status == "FWInPur")
        {
            Status = "Forward In Purchase";
        }
        return Status;
    }
    #region View

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);

    }
    #endregion
    #region QuotationPrint
    public void QuotationPrint(string QuotationId)
    {

        using (DataTable DtPara = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesQuotationApproval"))
        {

            if (DtPara.Rows.Count > 0)
            {
                if (Convert.ToBoolean(DtPara.Rows[0]["ParameterValue"]) == true)
                {
                    string st = GetApprovalStatus(Convert.ToInt32(QuotationId));
                    if (st == "Approved")
                    {
                        string strCmd = string.Format("window.open('../Sales/SalesQuotationPrint.aspx?Id=" + QuotationId.ToString() + "','window','width=1024, ');");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

                    }
                    else
                    {
                        DisplayMessage("Cannot Print,Quotation Not Approved");
                        return;
                    }
                }
                else
                {
                    string strCmd = string.Format("window.open('../Sales/SalesQuotationPrint.aspx?Id=" + QuotationId.ToString() + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

                }
            }
        }


    }
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        if (ddlLocation.SelectedValue.ToString() != Session["LocId"].ToString())
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('To Print this quotation you have to change your current location, do you want to continue ?');", true);
            return;
        }

        QuotationPrint(e.CommandArgument.ToString());
    }
    #endregion
    protected void ddlUser_Click(object sender, EventArgs e)
    {
        //FillGrid(1);
    }
    #region Shipping  Address
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtContact = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);
        string[] filterlist = new string[dtContact.Rows.Count];
        if (dtContact.Rows.Count > 0)
        {
            for (int i = 0; i < dtContact.Rows.Count; i++)
            {
                filterlist[i] = dtContact.Rows[i]["Filtertext"].ToString();
            }
        }
        dtContact = null;
        return filterlist;
    }
    protected void txtShipCustomerName_TextChanged(object sender, EventArgs e)
    {
        if (txtShipCustomerName.Text != "")
        {
            string[] ShipName = txtShipCustomerName.Text.Split('/');
            DataTable DtCustomer = objContact.GetContactByContactName(ShipName[0].ToString().Trim());

            if (DtCustomer.Rows.Count > 0)
            {
                DataTable dtAddress = objContact.GetAddressByRefType_Id("Contact", ShipName[1].ToString().Trim());
                if (dtAddress != null && dtAddress.Rows.Count > 0)
                {
                    txtShipingAddress.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                }
                else
                {
                    txtShipingAddress.Text = "";
                }
            }
            else
            {
                DisplayMessage("Select Ship to in Suggestion Only");
                txtShipCustomerName.Text = "";
                txtShipCustomerName.Focus();
                return;
            }

        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        try
        {
            Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = AddressN.GetAddress_PreText(prefixText);
            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["filterText"].ToString();
                }
            }
            dt = null;
            return str;
        }
        catch
        {
            return null;
        }
    }
    protected void txtShipingAddress_TextChanged(object sender, EventArgs e)
    {
        Set_AddressMaster objAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        if (txtShipingAddress.Text != "")
        {
            using (DataTable dtAM = objAddMaster.GetAddressDataByAddressName(txtShipingAddress.Text))
            {
                if (dtAM.Rows.Count > 0)
                {
                    string name = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());
                    ShippingAddress.Text = name;
                    GetShipToAddres();
                }
                else
                {
                    txtShipingAddress.Text = "";
                    DisplayMessage("Choose In Suggestions Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShipingAddress);
                    return;
                }
            }
        }
        else
        {
            ShippingAddress.Text = "";
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShipingAddress);
        }
        //AllPageCode();
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "script", "GetShipToAddress(" + txtShipCustomerName.Text + ");", true);
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "GetShipToAddress("+ txtShipCustomerName.Text+ ");", true);
    }
    public void GetShipToAddres()
    {
        string a = "";
        if (txtShipCustomerName.Text != "")
        {
            string[] ShipName = txtShipCustomerName.Text.Split('/');




            /// DataTable DtCustomer = objContact.GetContactByContactName(ShipName[0].ToString().Trim());
            DataTable dtAM = objAddMaster.GetAddressDataByAddressName(txtShipingAddress.Text);
            // DataTable dtAddress = objContact.GetAddressByRefType_Id("Contact", ShipName[1].ToString().Trim());
            if (dtAM != null && dtAM.Rows.Count > 0)
            {
                //txtShipingAddress.Text = dtAM.Rows[0]["Address_Name"].ToString();
                string name = string.Concat(dtAM.Rows[0]["Address"].ToString(), ",", dtAM.Rows[0]["EmailId1"].ToString(), ",", dtAM.Rows[0]["FullAddress"].ToString());
                ShipAddress.Text = name;
            }
            else
            {
                DisplayMessage("Select Ship to in Suggestion Only");
                txtShipCustomerName.Text = "";
                txtShipCustomerName.Focus();

            }

        }
        else
        {
            ShipAddress.Text = "";
        }
    }
    #endregion//round Function
    public String GetRoundValue(string Amount)
    {
        string strretrun = Amount;
        try
        {
            string DotValue = string.Empty;
            int strDeCount = Amount.Split('.')[1].ToString().Length;
            if (strDeCount > 0)
            {
                strretrun = Math.Round(Convert.ToDouble(Amount), strDeCount - 1).ToString();


                for (int i = 0; i < strDeCount; i++)
                {
                    DotValue = DotValue + "0";

                }


            }
            return Convert.ToDouble(strretrun).ToString("0." + DotValue);
        }
        catch
        {
            return strretrun;
        }

    }
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Quotation_Date" || ddlFieldName.SelectedItem.Value == "Field7" || ddlFieldName.SelectedItem.Value == "OrderCompletionDate")
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
        else
        {
            txtValueDate.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
    }
    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "Quotation_Date")
        {
            txtValueBinDate.Visible = true;
            txtValueBin.Visible = false;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
        else
        {
            txtValueBinDate.Visible = false;
            txtValueBin.Visible = true;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
    }
    #endregion
    #region Add Product Concept
    private void FillUnit()
    {

        using (DataTable dsUnit = UM.GetUnitListforDDl(Session["CompId"].ToString()))
        {
            if (dsUnit.Rows.Count > 0)
            {   //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)ddlUnit, dsUnit, "Unit_Name", "Unit_Id");
            }
        }
    }
    private void FillProductCurrency()
    {

        using (DataTable dsPCurrency = objCurrency.GetCurrencyListForDDL())
        {
            if (dsPCurrency.Rows.Count > 0)
            {   //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)ddlPCurrency, dsPCurrency, "Currency_Name", "Currency_ID");
            }
            else
            {
                ddlPCurrency.Items.Add("--Select--");
                ddlPCurrency.SelectedValue = "--Select--";
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductName_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["EProductName"].ToString();
            }
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductCode_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["ProductCode"].ToString();
            }
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
    }
    protected void btnAddNewProduct_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = true;
      
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        //AllPageCode();
        Session["DtSearchProduct"] = null;
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        string ParameterValue = string.Empty;
        if (txtProductName.Text != "")
        {
            if (Session["dtQuotationDetail"] != null)
            {

                if (new DataView(((DataTable)Session["dtQuotationDetail"]), "SuggestedProductName='" + txtProductcode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0)
                {

                    DisplayMessage("Product is already exists!");
                    txtProductName.Text = "";
                    txtProductcode.Text = "";
                    txtProductcode.Focus();
                    return;

                }
            }


            DataTable dtProduct = new DataTable();
            try
            {
                dtProduct = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductName.Text.ToString());

            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {


                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();







                string strUnitId = dtProduct.Rows[0]["UnitId"].ToString();
                if (strUnitId != "0" && strUnitId != "")
                {
                    ddlUnit.SelectedValue = strUnitId;
                }
                else
                {
                    FillUnit();
                }
                txtPDescription.Text = dtProduct.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtPDesc.Visible = false;
                pnlPDescription.Visible = true;
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRequiredQty);
                FillRelatedProduct(hdnNewProductId.Value);

            }
            else
            {
                GvRelatedProduct.DataSource = null;
                GvRelatedProduct.DataBind();

                hdnNewProductId.Value = "0";
                txtPDesc.Visible = true;
                pnlPDescription.Visible = false;

            }

            dtProduct.Dispose();
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
            GvRelatedProduct.DataSource = null;
            GvRelatedProduct.DataBind();
        }
        //AllPageCode();
    }
    public void FillRelatedProduct(string ProductId)
    {

        DataTable dsPCurrency = objCurrency.GetCurrencyMaster();
        DataTable dsUnit = UM.GetUnitMaster(Session["CompId"].ToString());
        using (DataTable dtRelatedProduct = objRelProduct.GetRelatedProductByProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId))
        {
            if (dtRelatedProduct.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvRelatedProduct, dtRelatedProduct, "", "");

                foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
                {

                    DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
                    DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
                    HiddenField hdnUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
                    TextBox txtquantity = (TextBox)gvRow.FindControl("txtquantity");

                    txtquantity.Text = "1";

                    if (dsUnit.Rows.Count > 0)
                    {
                        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

                        objPageCmn.FillData((object)ddlgvUnit, dsUnit, "Unit_Name", "Unit_Id");

                        try
                        {
                            ddlgvUnit.SelectedValue = hdnUnitId.Value;
                        }
                        catch
                        {
                        }
                    }

                    if (dsPCurrency.Rows.Count > 0)
                    {
                        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

                        objPageCmn.FillData((object)ddlgvCurrency, dsPCurrency, "Currency_Name", "Currency_ID");

                        try
                        {
                            ddlgvCurrency.SelectedValue = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        ddlgvCurrency.Items.Insert(0, "--Select--");

                    }

                }

            }
        }
        dsPCurrency.Dispose();
        dsUnit.Dispose();

    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        DataTable dtProduct = new DataTable();
        string ParameterValue = string.Empty;
        if (txtProductcode.Text != "")
        {

            try
            {
                dtProduct = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductcode.Text.ToString());
                if (new DataView(((DataTable)Session["dtQuotationDetail"]), "Product_Id='" + dtProduct.Rows[0]["ProductId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0)
                {

                    DisplayMessage("Product is already exists!");
                    txtProductName.Text = "";
                    txtProductcode.Text = "";
                    txtProductcode.Focus();
                    return;

                }
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {

                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();


                string strUnitId = dtProduct.Rows[0]["UnitId"].ToString();
                if (strUnitId != "0" && strUnitId != "")
                {
                    ddlUnit.SelectedValue = strUnitId;
                }
                else
                {
                    FillUnit();
                }
                txtPDescription.Text = dtProduct.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtPDesc.Visible = false;
                pnlPDescription.Visible = true;
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRequiredQty);
                FillRelatedProduct(hdnNewProductId.Value);
            }
            else
            {
                GvRelatedProduct.DataSource = null;
                GvRelatedProduct.DataBind();
                FillUnit();

                hdnNewProductId.Value = "0";
                txtPDesc.Visible = true;
                pnlPDescription.Visible = false;
            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
            GvRelatedProduct.DataSource = null;
            GvRelatedProduct.DataBind();
        }
        dtProduct.Dispose();

    }
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        string SuggestedProductName = string.Empty;

        //if (txtProductcode.Text == "")
        //{
        //    DisplayMessage("Enter Product Code");
        //    txtProductName.Focus();
        //    return;
        //}

        txtProductcode.Text = txtProductcode.Text == "" ? "0" : txtProductcode.Text;

        if (txtProductcode.Text != "")
        {
            hdnNewProductId.Value = objProductM.GetProductIdbyProductCode(txtProductcode.Text.Trim(), Session["BrandId"].ToString());

            if (hdnNewProductId.Value.Trim() == "@NOTFOUND@")
            {
                hdnNewProductId.Value = "0";
                SuggestedProductName = txtProductName.Text;
                if (txtPDesc.Text.Trim() == "")
                {
                    DisplayMessage("Enter Product Description");
                    txtPDesc.Focus();
                    return;
                }
            }
        }


        if (ddlUnit.SelectedValue != "--Select--")
        {
            hdnUnitId.Value = ddlUnit.SelectedValue;
        }
        else
        {
            DisplayMessage("Select Unit Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlUnit);
            return;
        }
        if (txtRequiredQty.Text == "")
        {

            DisplayMessage("Enter Required Quantity");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRequiredQty);
            return;
        }

        if (ddlPCurrency.SelectedValue != "--Select--")
        {
            hdnCurrencyId.Value = ddlPCurrency.SelectedValue;
        }
        else
        {
            DisplayMessage("Currency Required On Company Level");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlPCurrency);
            return;
        }

        if (txtEstimatedUnitPrice.Text == "")
        {

            txtEstimatedUnitPrice.Text = "0";

        }



        foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
        {
            CheckBox chk = (CheckBox)gvRow.FindControl("chk");
            DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
            DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
            TextBox txtgvquantity = (TextBox)gvRow.FindControl("txtquantity");
            Label txtgvProductdesc = (Label)gvRow.FindControl("Description");
            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            TextBox txtGvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtEstimatedUnitPrice");
            if (chk.Checked == true)
            {


                //foreach (GridViewRow gvDetailRow in GvDetail.Rows)
                //{
                //    HiddenField hdnCheckProductId = (HiddenField)gvDetailRow.FindControl("hdnProductId");
                //    if (lblgvProductId.Text == hdnCheckProductId.Value)
                //    {
                //        DisplayMessage("Related Product(" + lblgvProductName.Text + ") is Already Exists");
                //        chk.Checked = false;
                //        return;

                //    }


                //}


                if (ddlgvUnit.SelectedIndex == 0)
                {
                    DisplayMessage("Select Unit In Related Product List for product name=" + lblgvProductName.Text);
                    ddlgvUnit.Focus();
                    return;
                }
                if (txtgvquantity.Text.Trim() == "")
                {
                    DisplayMessage("Enter Required Quantity In Related Product List for product name=" + lblgvProductName.Text);
                    ddlgvUnit.Focus();
                    return;

                }


                if (txtGvEstimatedUnitPrice.Text != "")
                {
                    double flTemp = 0;
                    if (double.TryParse(txtGvEstimatedUnitPrice.Text, out flTemp))
                    {
                    }
                    else
                    {
                        txtGvEstimatedUnitPrice.Text = "";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Estimated Unit Price Should be numeric In Related Product List for product name=" + lblgvProductName.Text + "');", true);
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGvEstimatedUnitPrice);
                        return;
                    }
                }
                else
                {
                    txtGvEstimatedUnitPrice.Text = "0";
                }


                if (ddlgvCurrency.SelectedIndex == 0)
                {
                    DisplayMessage("Select Currency In Related Product List for product name=" + lblgvProductName.Text);
                    ddlgvUnit.Focus();
                    return;
                }

            }

        }

        DataTable dtDetail = ((DataTable)Session["dtQuotationDetail"]).Clone();
        int i = 0;


        //foreach (GridViewRow gvr in GvDetail.Rows)
        //{
        //    HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdnProductId");
        //    HiddenField hdnCurrencyId = (HiddenField)gvr.FindControl("hdnCurrencyId");
        //    HiddenField hdngvUnitId = (HiddenField)gvr.FindControl("hdnUnitId");

        //    //

        //    AjaxControlToolkit.HTMLEditor.Editor lblgvProductDescription = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("hdnSuggestedProductdesc");
        //    AjaxControlToolkit.HTMLEditor.Editor lblSuggestedProductName = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("lblgvProductDescription");

        //    Label lblSerialNo = (Label)gvr.FindControl("lblSerialNo");
        //    Label lblgvProductName = (Label)gvr.FindControl("lblgvProductName");
        //    TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
        //    TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
        //    TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        //    TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
        //    TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
        //    TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
        //    TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
        //    TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
        //    TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
        //    Label lblgvEstimatedUnitPrice = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");

        //    Literal lblProductPurchaseDiscription = (Literal)gvr.FindControl("txtdesc");
        //    Label lblProductPurchasePrice = (Label)gvr.FindControl("Label8");
        //    TextBox txtgvAgentCommission = (TextBox)gvr.FindControl("txtgvAgentCommission");
        //    if (Session["dtQuotationDetail"] != null)
        //    {


        //        if (hdnEditProductId.Value.Trim() == lblSerialNo.Text.Trim())
        //        {

        //            dtDetail.Rows.Add();
        //            i = dtDetail.Rows.Count - 1;

        //            //dtDetail.Rows[i]["TaxPercent"] = Tax_Per_Calculation(dtDetail.Rows[i]["UnitCost"].ToString(), dtDetail.Rows[i]["Product_Id"].ToString());
        //            //dtDetail.Rows[i]["TaxValue"] = Tax_Value_Calculation(dtDetail.Rows[i]["UnitCost"].ToString(), dtDetail.Rows[i]["Product_Id"].ToString());

        //            dtDetail.Rows[i]["TaxPercent"] = txtgvTaxP.Text;
        //            dtDetail.Rows[i]["TaxValue"] = txtgvTaxV.Text;

        //            dtDetail.Rows[i]["PriceAfterTax"] = txtgvPriceAfterTax.Text.Trim();
        //            dtDetail.Rows[i]["DiscountPercent"] = txtgvDiscountP.Text.Trim();
        //            dtDetail.Rows[i]["DiscountValue"] = txtgvDiscountV.Text.Trim();
        //            dtDetail.Rows[i]["PriceAfterDiscount"] = txtgvPriceAfterDiscount.Text;
        //            dtDetail.Rows[i]["AgentCommission"] = txtgvAgentCommission.Text;

        //            try
        //            {
        //                dtDetail.Rows[i]["Serial_No"] = Convert.ToDouble(new DataView(dtDetail, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Serial_No"].ToString()) + 1;
        //            }
        //            catch
        //            {
        //                dtDetail.Rows[i]["Serial_No"] = 1;

        //            }
        //            dtDetail.Rows[i]["Quantity"] = txtRequiredQty.Text;

        //            if (hdnNewProductId.Value == "0")
        //            {
        //                dtDetail.Rows[i]["ProductDescription"] = txtPDesc.Text;
        //                dtDetail.Rows[i]["SuggestedProductName"] = txtProductName.Text;
        //            }
        //            else
        //            {
        //                dtDetail.Rows[i]["ProductDescription"] = txtPDescription.Text;
        //                dtDetail.Rows[i]["SuggestedProductName"] = txtProductName.Text;

        //            }
        //            dtDetail.Rows[i]["Product_Id"] = hdnNewProductId.Value;
        //            dtDetail.Rows[i]["UnitId"] = hdnUnitId.Value;
        //            dtDetail.Rows[i]["Currency_Id"] = ddlCurrency.SelectedValue;
        //            dtDetail.Rows[i]["EstimatedUnitPrice"] = txtEstimatedUnitPrice.Text;
        //            dtDetail.Rows[i]["PurchaseProductDescription"] = "";
        //            try
        //            {
        //                dtDetail.Rows[i]["PurchaseProductPrice"] = "";
        //            }
        //            catch
        //            {
        //            }

        //            try
        //            {
        //                dtDetail.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), hdnNewProductId.Value).Rows[0]["Quantity"].ToString();
        //            }
        //            catch
        //            {
        //                dtDetail.Rows[i]["Sysqty"] = "0";

        //            }





        //            //this code is created by jitendra upadhyay on 15-12-2014
        //            //this code for get the sales price according the inventory parameter

        //            //code start

        //            try
        //            {

        //                dtDetail.Rows[i]["SalesPrice"] = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", ViewState["Customer_Id"].ToString(), hdnNewProductId.Value).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();

        //                dtDetail.Rows[i]["SalesPrice"] = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), dtDetail.Rows[i]["SalesPrice"].ToString());

        //            }
        //            catch
        //            {
        //                dtDetail.Rows[i]["SalesPrice"] = "0";
        //            }
        //        }
        //        else
        //        {

        //            dtDetail.Rows.Add();

        //            i = dtDetail.Rows.Count - 1;
        //            dtDetail.Rows[i]["TaxPercent"] = txtgvTaxP.Text;
        //            dtDetail.Rows[i]["TaxValue"] = txtgvTaxV.Text;
        //            dtDetail.Rows[i]["PriceAfterTax"] = txtgvPriceAfterTax.Text.Trim();
        //            dtDetail.Rows[i]["DiscountPercent"] = txtgvDiscountP.Text.Trim();
        //            dtDetail.Rows[i]["DiscountValue"] = txtgvDiscountV.Text.Trim();
        //            dtDetail.Rows[i]["PriceAfterDiscount"] = txtgvPriceAfterDiscount.Text;
        //            dtDetail.Rows[i]["Serial_No"] = lblSerialNo.Text;
        //            dtDetail.Rows[i]["Quantity"] = lblgvQuantity.Text;
        //            dtDetail.Rows[i]["Product_Id"] = hdngvProductId.Value;
        //            dtDetail.Rows[i]["SuggestedProductName"] = lblgvProductName.Text;
        //            dtDetail.Rows[i]["UnitId"] = hdngvUnitId.Value;
        //            dtDetail.Rows[i]["ProductDescription"] = lblgvProductDescription.Content;
        //            dtDetail.Rows[i]["Currency_Id"] = hdnCurrencyId.Value.Trim();
        //            dtDetail.Rows[i]["EstimatedUnitPrice"] = lblgvEstimatedUnitPrice.Text;
        //            dtDetail.Rows[i]["SalesPrice"] = txtgvUnitPrice.Text.Trim();
        //            dtDetail.Rows[i]["PurchaseProductDescription"] = lblProductPurchaseDiscription.Text.Trim();
        //            dtDetail.Rows[i]["AgentCommission"] = txtgvAgentCommission.Text;
        //            try
        //            {
        //                dtDetail.Rows[i]["PurchaseProductPrice"] = lblProductPurchasePrice.Text.Trim();
        //            }
        //            catch
        //            {
        //            }

        //            try
        //            {
        //                dtDetail.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), hdngvProductId.Value).Rows[0]["Quantity"].ToString();
        //            }
        //            catch
        //            {
        //                dtDetail.Rows[i]["Sysqty"] = "0";

        //            }
        //        }


        //    }
        //}

        if (hdnEditProductId.Value.Trim() == "")
        {


            dtDetail.Rows.Add();
            i = dtDetail.Rows.Count - 1;
            //dtDetail.Rows[i]["TaxPercent"] = "0";
            //dtDetail.Rows[i]["TaxValue"] = "0";
            //dtDetail.Rows[i]["TaxPercent"] = Get_Tax_Percentage(hdnNewProductId.Value, dtDetail.Rows[i]["Serial_No"].ToString());
            dtDetail.Rows[i]["TaxPercent"] = Get_Tax_Percentage(hdnNewProductId.Value, dtDetail.Rows.Count.ToString());
            dtDetail.Rows[i]["TaxValue"] = "0";

            dtDetail.Rows[i]["PriceAfterTax"] = "0";
            dtDetail.Rows[i]["DiscountPercent"] = "0";
            dtDetail.Rows[i]["DiscountValue"] = "0";
            dtDetail.Rows[i]["PriceAfterDiscount"] = "0";
            dtDetail.Rows[i]["AgentCommission"] = "0";

            try
            {
                dtDetail.Rows[i]["Serial_No"] = Convert.ToDouble(new DataView(dtDetail, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Serial_No"].ToString()) + 1;
            }
            catch
            {
                dtDetail.Rows[i]["Serial_No"] = 1;

            }
            dtDetail.Rows[i]["Quantity"] = txtRequiredQty.Text;

            if (hdnNewProductId.Value == "0")
            {
                dtDetail.Rows[i]["ProductDescription"] = txtPDesc.Text;
                dtDetail.Rows[i]["SuggestedProductName"] = txtProductName.Text;
            }
            else
            {
                dtDetail.Rows[i]["ProductDescription"] = txtPDescription.Text;
                dtDetail.Rows[i]["SuggestedProductName"] = txtProductName.Text;

            }
            dtDetail.Rows[i]["Product_Id"] = hdnNewProductId.Value;
            dtDetail.Rows[i]["UnitId"] = hdnUnitId.Value;
            dtDetail.Rows[i]["Currency_Id"] = ddlCurrency.SelectedValue;
            dtDetail.Rows[i]["EstimatedUnitPrice"] = txtEstimatedUnitPrice.Text;
            dtDetail.Rows[i]["PurchaseProductDescription"] = "";
            try
            {
                dtDetail.Rows[i]["PurchaseProductPrice"] = "";
            }
            catch
            {
            }
            try
            {
                dtDetail.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), hdnNewProductId.Value).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dtDetail.Rows[i]["Sysqty"] = "0";

            }

            //this code is created by jitendra upadhyay on 15-12-2014
            //this code for get the sales price according the inventory parameter

            //code start

            try
            {

                dtDetail.Rows[i]["SalesPrice"] = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", ViewState["Customer_Id"].ToString(), hdnNewProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();

                dtDetail.Rows[i]["SalesPrice"] = GetAmountDecimal(dtDetail.Rows[i]["SalesPrice"].ToString());

            }
            catch
            {
                dtDetail.Rows[i]["SalesPrice"] = "0";
            }
        }




        foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
        {
            CheckBox chk = (CheckBox)gvRow.FindControl("chk");
            DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
            DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
            TextBox txtgvquantity = (TextBox)gvRow.FindControl("txtquantity");
            Label txtgvProductdesc = (Label)gvRow.FindControl("lblgvProductDescription");
            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            TextBox txtGvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtEstimatedUnitPrice");


            if (chk.Checked == true)
            {

                dtDetail.Rows.Add();
                i = dtDetail.Rows.Count - 1;
                dtDetail.Rows[i]["TaxPercent"] = "0";
                dtDetail.Rows[i]["TaxValue"] = "0";
                dtDetail.Rows[i]["PriceAfterTax"] = "0";
                dtDetail.Rows[i]["DiscountPercent"] = "0";
                dtDetail.Rows[i]["DiscountValue"] = "0";
                dtDetail.Rows[i]["PriceAfterDiscount"] = "0";
                dtDetail.Rows[i]["AgentCommission"] = "0";
                try
                {
                    dtDetail.Rows[i]["Serial_No"] = Convert.ToDouble(new DataView(dtDetail, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Serial_No"].ToString()) + 1;
                }
                catch
                {
                    dtDetail.Rows[i]["Serial_No"] = 1;

                }
                //dtDetail.Rows[i]["Serial_No"] = Convert.ToInt32(dtDetail.Rows[i - 1]["Serial_No"].ToString()) + 1;
                dtDetail.Rows[i]["Quantity"] = txtgvquantity.Text;


                dtDetail.Rows[i]["ProductDescription"] = txtgvProductdesc.Text;
                dtDetail.Rows[i]["SuggestedProductName"] = lblgvProductName.Text;


                dtDetail.Rows[i]["Product_Id"] = lblgvProductId.Text;
                dtDetail.Rows[i]["UnitId"] = ddlgvUnit.SelectedValue;
                dtDetail.Rows[i]["Currency_Id"] = ddlgvCurrency.SelectedValue;
                dtDetail.Rows[i]["EstimatedUnitPrice"] = txtGvEstimatedUnitPrice.Text;
                dtDetail.Rows[i]["PurchaseProductDescription"] = "";
                try
                {
                    dtDetail.Rows[i]["PurchaseProductPrice"] = "";
                }
                catch
                {
                }
                //this code is created by jitendra upadhyay on 15-12-2014
                //this code for get the sales price according the inventory parameter

                //code start
                try
                {
                    dtDetail.Rows[i]["SalesPrice"] = GetAmountDecimal(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", ViewState["Customer_Id"].ToString(), lblgvProductId.Text, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString());
                }
                catch
                {
                    dtDetail.Rows[i]["SalesPrice"] = "0";
                }
                //code end

                try
                {
                    dtDetail.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
                }
                catch
                {
                    dtDetail.Rows[i]["Sysqty"] = "0";

                }
            }
        }

        Session["dtQuotationDetail"] = dtDetail;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //objPageCmn.FillData((object)GvDetail, dtDetail, "", "");
        HeadearCalculateGrid();
    
        dtDetail.Dispose();
    }
  
 
    protected void IbtnDetailDelete_Command(object sender, CommandEventArgs e)
    {
        // Session["dtQuotationDetail"] = GetProductDetailinDatatable();
        DataTable dtDetail = new DataView((DataTable)Session["dtQuotationDetail"], "Serial_No<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


        DataTable Dt_Cal = new DataTable();
        if (Session["Temp_Product_Tax_SQ"] != null)
        {
            Dt_Cal = Session["Temp_Product_Tax_SQ"] as DataTable;
            Dt_Cal = new DataView(Dt_Cal, "serial_no<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    Session["Temp_Product_Tax_SQ"] = Dt_Cal;
            //DataTable Dt_Cal = Session["Temp_Product_Tax_SQ"] as DataTable;
            DataTable dt1 = new DataTable();

            int serial_no = Convert.ToInt32(e.CommandArgument) - 1;
            int x = 0;
            //for (int i = serial_no; i < GvDetail.Rows.Count; i++)
            //{
            //    x = i + 2;
            //    //DataTable dtj = new DataView(Dt_Cal,"serial_no='"+ x + "'","",DataViewRowState.CurrentRows).ToTable(); 
            //    for (int j = 0; j < Dt_Cal.Rows.Count; j++)
            //    {
            //        if (Dt_Cal.Rows[j]["serial_no"].ToString() == x.ToString())
            //        {
            //            Dt_Cal.Rows[j]["serial_no"] = x - 1;
            //        }
            //    }
            //    try
            //    {
            //        if (dtDetail.Rows[i]["serial_no"].ToString() == x.ToString())
            //        {
            //            dtDetail.Rows[i]["serial_no"] = x - 1;
            //        }
            //    }
            //    catch
            //    {

            //    }
            //}
            Session["Temp_Product_Tax_SQ"] = Dt_Cal;
        }

        Session["dtQuotationDetail"] = dtDetail;


        if (dtDetail.Rows.Count != 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
           // objPageCmn.FillData((object)GvDetail, dtDetail, "", "");

            //foreach (GridViewRow Row in GvDetail.Rows)
            //{

            //    TextBox lblgvQuantity = (TextBox)Row.FindControl("lblgvQuantity");

            //    TextBox txtgvQuantityPrice = (TextBox)Row.FindControl("txtgvQuantityPrice");

            //    TextBox txtgvUnitPrice = (TextBox)Row.FindControl("txtgvUnitPrice");

            //    if (lblgvQuantity.Text == "")
            //    {
            //        lblgvQuantity.Text = "0";
            //    }
            //    if (txtgvUnitPrice.Text == "")
            //    {
            //        txtgvUnitPrice.Text = "0";
            //    }
            //    txtgvQuantityPrice.Text = (Convert.ToDouble(lblgvQuantity.Text) * Convert.ToDouble(txtgvUnitPrice.Text)).ToString();

            //    try
            //    {
            //        DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //        if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
            //        {
            //            ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
            //            ((Panel)Row.FindControl("PopupMenu")).Visible = false;
            //        }
            //    }
            //    catch
            //    {
            //    }
            //}
            HeadearCalculateGrid();
            //SetDecimal();
        }
        else
        {
           // GvDetail.DataSource = null;
            //GvDetail.DataBind();
            txtAmount.Text = "0";
            txtTaxP.Text = "0";
            txtTaxV.Text = "0";
            txtPriceAfterTax.Text = "0";
            txtDiscountP.Text = "0";
            txtDiscountV.Text = "0";
            txtTotalAmount.Text = "0";

        }
        dtDetail.Dispose();
        //AllPageCode();
    }
    protected void IbtnDetailEdit_Command(object sender, CommandEventArgs e)
    {
  
        //GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;

        DataTable dtDetail = new DataView((DataTable)Session["dtQuotationDetail"], "Serial_No='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtDetail.Rows.Count > 0)
        {
            txtRequiredQty.Text = dtDetail.Rows[0]["Quantity"].ToString();
            if (dtDetail.Rows[0]["Product_Id"].ToString() == "0")
            {
                txtPDesc.Text = dtDetail.Rows[0]["ProductDescription"].ToString();
                txtProductName.Text = dtDetail.Rows[0]["SuggestedProductName"].ToString();
                txtPDesc.Visible = true;
                pnlPDescription.Visible = false;
            }
            else
            {
                txtProductcode.Text = ((Label)gvRow.FindControl("lblgvProductcode")).Text;
                txtPDescription.Text = dtDetail.Rows[0]["ProductDescription"].ToString();
                txtProductName.Text = dtDetail.Rows[0]["SuggestedProductName"].ToString();
                txtPDesc.Visible = false;
                pnlPDescription.Visible = true;
            }


            hdnEditProductId.Value = e.CommandArgument.ToString();
            hdnNewProductId.Value = dtDetail.Rows[0]["Product_Id"].ToString();
            ddlUnit.SelectedValue = dtDetail.Rows[0]["UnitId"].ToString();
            hdnUnitId.Value = dtDetail.Rows[0]["UnitId"].ToString();
            ddlCurrency.SelectedValue = dtDetail.Rows[0]["Currency_Id"].ToString();
            txtEstimatedUnitPrice.Text = dtDetail.Rows[0]["EstimatedUnitPrice"].ToString();
        }
        dtDetail.Dispose();
    }
    #endregion
    #region Advancesearch
    public DataTable CreateProductDataTableForSalesQuotation()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("TaxPercent");
        dt.Columns.Add("TaxValue");
        dt.Columns.Add("PriceAfterTax");
        dt.Columns.Add("DiscountPercent");
        dt.Columns.Add("DiscountValue");
        dt.Columns.Add("PriceAfterDiscount");
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Quantity");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("SuggestedProductName");
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("PurchaseProductDescription");
        dt.Columns.Add("PurchaseProductPrice");
        dt.Columns.Add("SalesPrice");
        dt.Columns.Add("Sysqty");
        dt.Columns.Add("AgentCommission");
        return dt;
    }
    protected void btnAddProductScreen_Click(object sender, EventArgs e)
    {
        Session["dtQuotationDetail"] = GetProductDetailinDatatable();

        Session["DtSearchProduct"] = Session["dtQuotationDetail"];

        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=SQ&&CustomerId=" + ViewState["Customer_Id"].ToString() + "&&CurId=" + ddlCurrency.SelectedValue + "','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    public DataTable GetProductDetailinDatatable()
    {
        DataTable dtDetail = CreateProductDataTableForSalesQuotation();
        int i = 0;

        //foreach (GridViewRow gvr in GvDetail.Rows)
        //{
        //    HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdnProductId");
        //    HiddenField hdnCurrencyId = (HiddenField)gvr.FindControl("hdnCurrencyId");
        //    HiddenField hdngvUnitId = (HiddenField)gvr.FindControl("hdnUnitId");

        //    //

        //    AjaxControlToolkit.HTMLEditor.Editor lblgvProductDescription = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("hdnSuggestedProductdesc");
        //    AjaxControlToolkit.HTMLEditor.Editor lblSuggestedProductName = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("lblgvProductDescription");

        //    Label lblSerialNo = (Label)gvr.FindControl("lblSerialNo");
        //    Label lblgvProductName = (Label)gvr.FindControl("lblgvProductName");
        //    TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
        //    TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
        //    TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        //    TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
        //    TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
        //    TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
        //    TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
        //    TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
        //    TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
        //    Label lblgvEstimatedUnitPrice = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");
        //    TextBox txtgvAgentCommission = (TextBox)gvr.FindControl("txtgvAgentCommission");
        //    Literal lblProductPurchaseDiscription = (Literal)gvr.FindControl("txtdesc");
        //    Label lblProductPurchasePrice = (Label)gvr.FindControl("Label8");

        //    if (Session["dtQuotationDetail"] != null)
        //    {
        //        dtDetail.Rows.Add();

        //        i = dtDetail.Rows.Count - 1;
        //        dtDetail.Rows[i]["TaxPercent"] = txtgvTaxP.Text;
        //        dtDetail.Rows[i]["TaxValue"] = txtgvTaxV.Text;
        //        dtDetail.Rows[i]["PriceAfterTax"] = txtgvPriceAfterTax.Text.Trim();
        //        dtDetail.Rows[i]["DiscountPercent"] = txtgvDiscountP.Text.Trim();
        //        dtDetail.Rows[i]["DiscountValue"] = txtgvDiscountV.Text.Trim();
        //        dtDetail.Rows[i]["PriceAfterDiscount"] = txtgvPriceAfterDiscount.Text;
        //        dtDetail.Rows[i]["Serial_No"] = lblSerialNo.Text;
        //        dtDetail.Rows[i]["Quantity"] = lblgvQuantity.Text;
        //        dtDetail.Rows[i]["Product_Id"] = hdngvProductId.Value;
        //        dtDetail.Rows[i]["SuggestedProductName"] = lblgvProductName.Text;
        //        dtDetail.Rows[i]["UnitId"] = hdngvUnitId.Value;
        //        dtDetail.Rows[i]["ProductDescription"] = lblgvProductDescription.Content;
        //        dtDetail.Rows[i]["Currency_Id"] = hdnCurrencyId.Value.Trim();
        //        dtDetail.Rows[i]["EstimatedUnitPrice"] = lblgvEstimatedUnitPrice.Text;
        //        dtDetail.Rows[i]["SalesPrice"] = txtgvUnitPrice.Text.Trim();
        //        dtDetail.Rows[i]["PurchaseProductDescription"] = lblProductPurchaseDiscription.Text.Trim();
        //        dtDetail.Rows[i]["AgentCommission"] = txtgvAgentCommission.Text;
        //        try
        //        {
        //            dtDetail.Rows[i]["PurchaseProductPrice"] = lblProductPurchasePrice.Text.Trim();
        //        }
        //        catch
        //        {
        //        }

        //        try
        //        {
        //            dtDetail.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), hdngvProductId.Value).Rows[0]["Quantity"].ToString();
        //        }
        //        catch
        //        {
        //            dtDetail.Rows[i]["Sysqty"] = "0";

        //        }
        //    }
        //}

        return dtDetail;
    }
    protected void btnAddtoList_Click(object sender, EventArgs e)
    {

        DataTable dtDetail = new DataTable();
        if (Session["DtSearchProduct"] != null)
        {
            Session["dtQuotationDetail"] = Session["DtSearchProduct"];
            if (Session["dtQuotationDetail"] != null)
            {
                dtDetail = (DataTable)Session["dtQuotationDetail"];
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                //objPageCmn.FillData((object)GvDetail, (DataTable)Session["dtQuotationDetail"], "", "");

            }
            //foreach (GridViewRow Row in GvDetail.Rows)
            //{
            //    TextBox txtgvAgentCommission = (TextBox)Row.FindControl("txtgvAgentCommission");

            //    TextBox Tax_Percent = (TextBox)Row.FindControl("txtgvTaxP");

            //    HiddenField Product_ID = (HiddenField)Row.FindControl("hdnProductId");
            //    TextBox Unit_Cost = (TextBox)Row.FindControl("txtgvUnitPrice");
            //    Label lblgvSerialNo = (Label)Row.FindControl("lblgvSerialNo");


            //    string cost = string.Empty;
            //    if (Unit_Cost.Text == "0.00" || Unit_Cost.Text == "0" || Unit_Cost.Text == "")
            //        cost = "1";
            //    else
            //        cost = Unit_Cost.Text;

            //    if (Tax_Percent.Text == "0.00" || Tax_Percent.Text == "0" || Tax_Percent.Text == "")
            //        Tax_Percent.Text = Get_Tax_Percentage(Product_ID.Value, lblgvSerialNo.Text).ToString();

            //    txtgvAgentCommission.Text = GetAmountDecimal(txtgvAgentCommission.Text);

            //    try
            //    {
            //        DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //        if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
            //        {
            //            ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
            //            ((Panel)Row.FindControl("PopupMenu")).Visible = false;
            //        }
            //    }
            //    catch
            //    {
            //    }
            //    //txtgvUnitPrice_TextChanged(((object)((TextBox)Row.FindControl("txtgvUnitPrice"))), e);

            //}
            HeadearCalculateGrid();
            Session["DtSearchProduct"] = null;
        }
        else
        {
            if (Session["dtQuotationDetail"] != null)
            {
                dtDetail = (DataTable)Session["dtQuotationDetail"];
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                //objPageCmn.FillData((object)GvDetail, (DataTable)Session["dtQuotationDetail"], "", "");
            }


            //foreach (GridViewRow Row in GvDetail.Rows)
            //{
            //    TextBox txtgvAgentCommission = (TextBox)Row.FindControl("txtgvAgentCommission");

            //    txtgvAgentCommission.Text = GetAmountDecimal(txtgvAgentCommission.Text);

            //    try
            //    {
            //        DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //        if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
            //        {
            //            ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
            //            ((Panel)Row.FindControl("PopupMenu")).Visible = false;
            //        }
            //    }
            //    catch
            //    {
            //    }
            //    //txtgvUnitPrice_TextChanged(((object)((TextBox)Row.FindControl("txtgvUnitPrice"))), e);

            //}
            HeadearCalculateGrid();
            DisplayMessage("Product Not Found");
            return;

        }
        //GvDetail.Columns[14].Visible = false;
        //GvDetail.Columns[17].Visible = false;
        //dtDetail.Dispose();

    }
    protected void rbtnFormView_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnFormView.Checked == true)
        {
            btnAddNewProduct.Visible = true;
            btnAddProductScreen.Visible = false;
            btnAddtoList.Visible = false;
            btnAddNewProduct_Click(null, null);
        }
        if (rbtnAdvancesearchView.Checked == true)
        {
            btnAddNewProduct.Visible = false;
            btnAddProductScreen.Visible = true;
            btnAddtoList.Visible = true;
          
        }

    }
    #endregion
    #region PendingSalesQuotation
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        //FillGrid(1);
        //AllPageCode();
    }
    #endregion
    #region LOcationStock

    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {

        string CustomerName = string.Empty;

        try
        {
            CustomerName = txtCustomer.Text;
        }
        catch
        {

        }
        string strCmd = "window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=" + e.CommandArgument.ToString() + "&&Type=SALES&&Contact=" + CustomerName + "')";

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);





        //DataTable dt = objStockDetail.GetStockDetail(Session["CompId"].ToString(), e.CommandArgument.ToString());
        //if (dt.Rows.Count == 0)
        //{
        //    DisplayMessage("Stock Not Found");
        //    return;
        //}
        //try
        //{
        //    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //    objPageCmn.FillData((object)gvStockInfo, dt, "", "");
        //}
        //catch
        //{
        //}
        //pnlStock1.Visible = true;
        //pnlStock2.Visible = true;
    }


    #endregion
    #region PreDesignQuotation

    protected void txtTemplateName_TextChanged(object sender, EventArgs e)
    {
        if (txtTemplateName.Text.Trim() != "")
        {

            if (txtCustomer.Text == "")
            {
                DisplayMessage("First Select Inquiry !");
                txtTemplateName.Text = "";
                return;
            }

            //here we check that template is selected from suggestion or not 


            string strquotationid = string.Empty;

            try
            {
                strquotationid = txtTemplateName.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Template Not Found");
                txtTemplateName.Text = "";
                return;
            }

            string SuggestedProductName = string.Empty;
            DataTable dtSQuoteDetail = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id("0", "0", "0", strquotationid, Session["FinanceYearId"].ToString());
            if (dtSQuoteDetail.Rows.Count == 0)
            {
                DisplayMessage("Product Not Found");
                txtTemplateName.Focus();
                txtTemplateName.Text = "";
                return;
            }
            dtSQuoteDetail.Columns["Field1"].ColumnName = "UnitId";
            dtSQuoteDetail.Columns["UnitPrice"].ColumnName = "SalesPrice";
            dtSQuoteDetail.Columns["Field2"].ColumnName = "EstimatedUnitPrice";
            dtSQuoteDetail.Columns["Field4"].ColumnName = "PurchaseProductPrice";
            dtSQuoteDetail.Columns["Field5"].ColumnName = "PurchaseProductDescription";
            DataTable dtQuoteEdit = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId("0", "0", "0", strquotationid);
            if (dtQuoteEdit.Rows.Count > 0)
            {
                txtHeader.Content = dtQuoteEdit.Rows[0]["Header"].ToString();
                txtFooter.Content = dtQuoteEdit.Rows[0]["Footer"].ToString();
                txtCondition1.Content = dtQuoteEdit.Rows[0]["Condition1"].ToString();
            }
            DataTable dtDetail = ((DataTable)Session["dtQuotationDetail"]).Clone();
            int i = 0;
            //foreach (GridViewRow gvr in GvDetail.Rows)
            //{
            //    HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdnProductId");
            //    HiddenField hdnCurrencyId = (HiddenField)gvr.FindControl("hdnCurrencyId");
            //    HiddenField hdngvUnitId = (HiddenField)gvr.FindControl("hdnUnitId");

            //    //

            //    AjaxControlToolkit.HTMLEditor.Editor lblgvProductDescription = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("hdnSuggestedProductdesc");
            //    AjaxControlToolkit.HTMLEditor.Editor lblSuggestedProductName = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("lblgvProductDescription");

            //    Label lblSerialNo = (Label)gvr.FindControl("lblSerialNo");
            //    TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
            //    TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
            //    TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
            //    TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
            //    TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
            //    TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
            //    TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
            //    TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
            //    TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
            //    Label lblgvEstimatedUnitPrice = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");
            //    TextBox txtgvAgentCommission = (TextBox)gvr.FindControl("txtgvAgentCommission");
            //    Literal lblProductPurchaseDiscription = (Literal)gvr.FindControl("txtdesc");
            //    Label lblProductPurchasePrice = (Label)gvr.FindControl("Label8");

            //    if (Session["dtQuotationDetail"] != null)
            //    {
            //        dtDetail.Rows.Add();

            //        i = dtDetail.Rows.Count - 1;
            //        dtDetail.Rows[i]["TaxPercent"] = txtgvTaxP.Text;
            //        dtDetail.Rows[i]["TaxValue"] = txtgvTaxV.Text;
            //        dtDetail.Rows[i]["PriceAfterTax"] = txtgvPriceAfterTax.Text.Trim();
            //        dtDetail.Rows[i]["DiscountPercent"] = txtgvDiscountP.Text.Trim();
            //        dtDetail.Rows[i]["DiscountValue"] = txtgvDiscountV.Text.Trim();
            //        dtDetail.Rows[i]["PriceAfterDiscount"] = txtgvPriceAfterDiscount.Text;
            //        dtDetail.Rows[i]["Serial_No"] = lblSerialNo.Text;
            //        dtDetail.Rows[i]["Quantity"] = lblgvQuantity.Text;
            //        dtDetail.Rows[i]["Product_Id"] = hdngvProductId.Value;
            //        dtDetail.Rows[i]["SuggestedProductName"] = lblSuggestedProductName.Content;
            //        dtDetail.Rows[i]["UnitId"] = hdngvUnitId.Value;
            //        dtDetail.Rows[i]["ProductDescription"] = lblgvProductDescription.Content;
            //        dtDetail.Rows[i]["Currency_Id"] = hdnCurrencyId.Value.Trim();
            //        dtDetail.Rows[i]["EstimatedUnitPrice"] = lblgvEstimatedUnitPrice.Text;
            //        dtDetail.Rows[i]["SalesPrice"] = txtgvUnitPrice.Text.Trim();
            //        dtDetail.Rows[i]["PurchaseProductDescription"] = lblProductPurchaseDiscription.Text.Trim();
            //        dtDetail.Rows[i]["AgentCommission"] = txtgvAgentCommission.Text;
            //        try
            //        {
            //            dtDetail.Rows[i]["PurchaseProductPrice"] = lblProductPurchasePrice.Text.Trim();
            //        }
            //        catch
            //        {
            //        }

            //        try
            //        {
            //            dtDetail.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), hdngvProductId.Value).Rows[0]["Quantity"].ToString();
            //        }
            //        catch
            //        {
            //            dtDetail.Rows[i]["Sysqty"] = "0";

            //        }
            //    }
            //}

            foreach (DataRow dr in dtSQuoteDetail.Rows)
            {
                if (dr["Product_Id"].ToString().Trim() != "0")
                {
                    if (new DataView(dtDetail, "Product_Id=" + dr["Product_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        continue;
                    }
                }

                dtDetail.Rows.Add();
                i = dtDetail.Rows.Count - 1;
                dtDetail.Rows[i]["TaxPercent"] = dr["TaxPercent"].ToString();
                dtDetail.Rows[i]["TaxValue"] = dr["TaxValue"].ToString();
                dtDetail.Rows[i]["PriceAfterTax"] = dr["PriceAfterTax"].ToString();
                dtDetail.Rows[i]["DiscountPercent"] = dr["DiscountPercent"].ToString();
                dtDetail.Rows[i]["DiscountValue"] = dr["DiscountValue"].ToString();
                dtDetail.Rows[i]["PriceAfterDiscount"] = dr["PriceAfterDiscount"].ToString();
                dtDetail.Rows[i]["AgentCommission"] = "0";
                try
                {
                    dtDetail.Rows[i]["Serial_No"] = Convert.ToDouble(new DataView(dtDetail, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Serial_No"].ToString()) + 1;
                }
                catch
                {
                    dtDetail.Rows[i]["Serial_No"] = 1;

                }
                //dtDetail.Rows[i]["Serial_No"] = Convert.ToInt32(dtDetail.Rows[i - 1]["Serial_No"].ToString()) + 1;
                dtDetail.Rows[i]["Quantity"] = dr["Quantity"].ToString();
                dtDetail.Rows[i]["ProductDescription"] = dr["ProductDescription"].ToString();
                dtDetail.Rows[i]["SuggestedProductName"] = dr["SuggestedProductName"].ToString();
                dtDetail.Rows[i]["Product_Id"] = dr["Product_Id"].ToString();
                dtDetail.Rows[i]["UnitId"] = dr["UnitId"].ToString();
                dtDetail.Rows[i]["Currency_Id"] = dr["Currency_Id"].ToString();
                dtDetail.Rows[i]["EstimatedUnitPrice"] = getLocalAmount(dr["EstimatedUnitPrice"].ToString(), dr["Currency_Id"].ToString());
                dtDetail.Rows[i]["PurchaseProductDescription"] = dr["PurchaseProductDescription"].ToString();
                try
                {
                    dtDetail.Rows[i]["PurchaseProductPrice"] = dr["PurchaseProductPrice"].ToString();
                }
                catch
                {
                }
                try
                {
                    dtDetail.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), dr["Product_Id"].ToString()).Rows[0]["Quantity"].ToString();
                }
                catch
                {
                    dtDetail.Rows[i]["Sysqty"] = "0";

                }

                //this code is created by jitendra upadhyay on 15-12-2014
                //this code for get the sales price according the inventory parameter

                //code start

                try
                {

                    dtDetail.Rows[i]["SalesPrice"] = getLocalAmount(dr["SalesPrice"].ToString(), dr["Currency_Id"].ToString());

                }
                catch
                {
                    dtDetail.Rows[i]["SalesPrice"] = "0";
                }
            }

            Session["dtQuotationDetail"] = dtDetail;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //objPageCmn.FillData((object)GvDetail, dtDetail, "", "");

            //foreach (GridViewRow Row in GvDetail.Rows)
            //{
            //    try
            //    {
            //        DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //        if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
            //        {
            //            ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
            //            ((Panel)Row.FindControl("PopupMenu")).Visible = false;
            //        }
            //    }
            //    catch
            //    {
            //    }
            //    // txtgvUnitPrice_TextChanged(((object)((TextBox)Row.FindControl("txtgvUnitPrice"))), e);

            //}
            //SetDecimal();
            HeadearCalculateGrid();
        
            dtSQuoteDetail.Dispose();
            dtQuoteEdit.Dispose();
            dtDetail.Dispose();
        }
    }

    public string getLocalAmount(string strForeignAmount, string strforeignCurrency)
    {
        string strlocalAmount = string.Empty;

        string strExchnagerate = string.Empty;

        try
        {
            strExchnagerate = SystemParameter.GetExchageRate(strforeignCurrency, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());
        }
        catch
        {
            strExchnagerate = "1";
        }

        if (strExchnagerate == "")
        {
            strExchnagerate = "1";
        }

        strlocalAmount = GetAmountDecimal((Convert.ToDouble(strForeignAmount) * Convert.ToDouble(strExchnagerate)).ToString());

        return strlocalAmount;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] DeleteQuotation(string QuatationId,string Location_Id)
    {
        string[] txt = new string[2];

        Inv_SalesOrderHeader objSOrderHeader = new Inv_SalesOrderHeader(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_ParameterMaster objInvParam = new Inv_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesQuotationHeader objSQuoteHeader = new Inv_SalesQuotationHeader(HttpContext.Current.Session["DBConnection"].ToString());
        Set_Approval_Employee objEmpApproval = new Set_Approval_Employee(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtSoHeader = objSOrderHeader.GetSOHeaderAllByFromTransType(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(),Location_Id, "Q",QuatationId))
        {

            if (dtSoHeader.Rows.Count > 0)
            {
                txt[0]= "Sales Quotation is used in Sales order";
                return txt;
            }
        }

        string quoteApproval = objInvParam.getParameterValueByParameterName("SalesQuotationApproval", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (Convert.ToBoolean(quoteApproval) == true)
        {
            //string st = GetApprovalStatus(Convert.ToInt32(QuatationId));
            string st= ((DataTable)(objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), QuatationId.ToString()))).Rows[0]["Field4"].ToString();
            if (st == "Approved")
            {
                txt[0] = "Cannot Delete, Quotation has Approved";
                return txt;
            }
        }       
      int  b = objSQuoteHeader.DeleteQuotationHeader(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(),Location_Id, QuatationId, "false", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            objEmpApproval.Delete_Approval_Transaction("8", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(),Location_Id, "0",QuatationId);
            txt[0] ="Record Delete";
          
        }
        else
        {
            txt[0]="Record  Not Deleted";
            return txt;
        }
       
        return txt;
    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListtemplateName(string prefixText, int count, string contextKey)
    {
        Inv_SalesQuotationHeader objSalesquotation = new Inv_SalesQuotationHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtContact = objSalesquotation.GetQuotationHeaderAllTrue("0", "0", "0");
        DataTable dtMain = new DataTable();
        dtMain = dtContact.Copy();
        string filtertext = "Condition2 like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "Condition2 Asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Condition2"].ToString() + "/" + dtCon.Rows[i]["SQuotation_Id"].ToString();
            }
        }
        dtContact.Dispose();
        return filterlist;
    }


    #endregion
    #region Followup


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListFollowupperson(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtContact = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);
        string[] filterlist = new string[dtContact.Rows.Count];
        if (dtContact.Rows.Count > 0)
        {
            for (int i = 0; i < dtContact.Rows.Count; i++)
            {
                filterlist[i] = dtContact.Rows[i]["Name"].ToString();
            }
        }
        dtContact = null;
        return filterlist;
    }

    protected void IbtnAddProductSupplierCode_Click(object sender, ImageClickEventArgs e)
    {
        //if (txtFollowupDate.Text == "")
        //{
        //    DisplayMessage("Enter Followup date");
        //    txtFollowupDate.Focus();
        //    return;
        //}
        //else
        //{
        //    try
        //    {
        //        Convert.ToDateTime(txtFollowupDate.Text);
        //    }
        //    catch
        //    {
        //        DisplayMessage("Enter Followup date in correct format");
        //        txtFollowupDate.Focus();
        //        return;
        //    }
        //}

        //if (txtFollowup_Person.Text == "")
        //{
        //    DisplayMessage("Enter Followup person Name");
        //    txtFollowup_Person.Focus();
        //    return;
        //}

        //if (txtFollowupRemarks.Text == "")
        //{
        //    DisplayMessage("Enter Followup Description");
        //    txtFollowupRemarks.Focus();
        //    return;
        //}

        //string strTransId = string.Empty;



        //DataTable dt = (DataTable)Session["dtFollowup"];
        //if (dt == null)
        //{
        //    dt = new DataTable();
        //    dt.Columns.Add("Trans_Id", typeof(double));
        //    dt.Columns.Add("Follow_Up_Date", typeof(DateTime));
        //    dt.Columns.Add("Follow_Up_Person");
        //    dt.Columns.Add("Description");
        //    strTransId = "1";
        //}
        //else
        //{

        //    if (dt.Rows.Count > 0)
        //    {
        //        strTransId = (Convert.ToDouble(new DataView(dt, "", "Trans_Id Desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) + 1).ToString();
        //    }
        //    else
        //    {
        //        strTransId = "1";
        //    }

        //}

        //DateTime dtdate = new DateTime(Convert.ToDateTime(txtFollowupDate.Text).Year, Convert.ToDateTime(txtFollowupDate.Text).Month, Convert.ToDateTime(txtFollowupDate.Text).Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);



        //dt.Rows.Add(strTransId, dtdate, txtFollowup_Person.Text, txtFollowupRemarks.Text);
        ////this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //objPageCmn.FillData((object)GvFollowup, dt, "", "");

        //Session["dtFollowup"] = dt;
        //txtFollowupDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        //txtFollowup_Person.Text = "";
        //txtFollowupRemarks.Text = "";
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFollowupDate);

    }
    protected void IbtnDeleteFollowup_Command(object sender, CommandEventArgs e)
    {
        //DataTable dt = (DataTable)Session["dtFollowup"];
        //if (dt != null)
        //{
        //    if (dt.Rows.Count > 0)
        //    {
        //        var rows = dt.Select("Trans_Id ='" + e.CommandArgument.ToString() + "'");
        //        foreach (var row in rows)
        //            row.Delete();
        //        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //        objPageCmn.FillData((object)GvFollowup, dt, "", "");
        //        Session["dtFollowup"] = dt;
        //    }
        //}

    }
    #endregion
    #region serviceMethod
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactList(string prefixText, int count, string contextKey)
    {

        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtContact = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);
        string[] filterlist = new string[dtContact.Rows.Count];
        if (dtContact.Rows.Count > 0)
        {
            for (int i = 0; i < dtContact.Rows.Count; i++)
            {
                filterlist[i] = dtContact.Rows[i]["Filtertext"].ToString();
            }
        }
        dtContact = null;
        return filterlist;
    }
    #endregion
    public bool IsAddAgentCommission(string str)
    {
        Common ObjComman = new Common(Session["DBConnection"].ToString());
        bool IsAllow = false;

        if (Session["EmpId"].ToString() == "0")
        {
            IsAllow = true;
        }
        else
        {

            using (DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), "144", "57", Session["CompId"].ToString()))
            {

                if (new DataView(dtAllPageCode, "Op_Id=16", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    IsAllow = true;

                }
            }
        }

        return IsAllow;
    }
    protected void lnkcustomerHistory_OnClick(object sender, EventArgs e)
    {
        string CustomerId = string.Empty;


        if (txtCustomer.Text != "")
        {
            try
            {
                CustomerId = ViewState["Customer_Id"].ToString();
            }
            catch
            {
                CustomerId = "0";
            }
        }
        else
        {
            CustomerId = "0";
        }

        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + CustomerId + "&&Page=SQ','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    //created by jitendra upadhyay for ask reason in case of quotation will be lost 


    //created on 04-06-2016



    //code created by jitendra upadhyay fro show ordre detail from quotation page if order created against the quotation

    //code created on 04-06-2016
    #region SalesOrderDetail


    protected void btnOrderDetail_Click(object sender, EventArgs e)
    {

        using (DataTable dt = objSOrderHeader.GetSOHeaderAllByFromTransType(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Q", editid.Value))
        {

            if (dt.Rows.Count > 0)
            {

                if (!isSalesOrderPermission())
                {
                    DisplayMessage("User have no permission to view sales order ");
                    return;
                }

                string strCmd = string.Format("window.open('../Sales/SalesOrderView.aspx?OrderId=" + dt.Rows[0]["Trans_Id"].ToString() + "','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
            else
            {
                DisplayMessage("Order Not found");
                return;
            }
        }

    }

    public bool isSalesOrderPermission()
    {
        bool isAllow = false;
        //here we checking user permission for view sales order info 
        if (Session["EmpId"].ToString() == "0")
        {
            isAllow = true;
        }
        else
        {
            using (DataTable dtAllpagecode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "144", "67", Session["CompId"].ToString()))
            {
                if (dtAllpagecode.Rows.Count > 0)
                {
                    isAllow = true;
                }
            }
        }

        return isAllow;
    }
    #endregion
    //product builder concept
    //added by jirtendra upadhyay on 09-11-2016
    protected void lnkProductBuilder_OnClick(object sender, EventArgs e)
    {
        Session["dtQuotationDetail"] = GetProductDetailinDatatable();

        Session["DtSearchProduct"] = Session["dtQuotationDetail"];

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory/ProductBuilder.aspx?Type=SQ')", true);


    }
    protected void lnkLabelBuilder_OnClick(object sender, EventArgs e)
    {
        Session["dtQuotationDetail"] = GetProductDetailinDatatable();

        Session["DtSearchProduct"] = Session["dtQuotationDetail"];

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory/LabelBuilder.aspx?Type=SQ')", true);


    }
    protected void lbkGetProductList_OnClick(object sender, EventArgs e)
    {

        btnAddtoList_Click(null, null);

    }
    public DataTable TemporaryProductWiseTaxes()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Product_Id", typeof(float));
        dt.Columns.Add("Tax_Id", typeof(float));
        dt.Columns.Add("Tax_Name", typeof(string));
        dt.Columns.Add("Tax_Value", typeof(float));
        dt.Columns.Add("TaxAmount", typeof(float));
        dt.Columns.Add("Amount", typeof(float));
        dt.Columns.Add("Serial_No");
        return dt;
    }
    protected void IBtnFollowup_Command(object sender, CommandEventArgs e)
    {
        Session["Quote_SQuotation_Id"] = "0";
        Session["Quote_SQuotation_Id"] = e.CommandArgument.ToString();
        FollowupUC.setLocationId(e.CommandName.ToString());
        FollowupUC.newBtnCall();
        //FollowupUC.fillFollowupListSession(hdnFollowupTableName.Value);
        //FollowupUC.fillFollowupList(hdnFollowupTableName.Value);
        //FollowupUC.fillFollowupBinSession(hdnFollowupTableName.Value);
        //FollowupUC.fillFollowupBin(hdnFollowupTableName.Value);
        FollowupUC.GetFollowupDocumentNumber();
        FollowupUC.SetGeneratedByName();
        FollowupUC.ResetFollowupType();
        DataTable dt = ObjFollowupClass.getQuoteHeaderDtlByID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        FollowupUC.fillHeader(dt, hdnFollowupTableName.Value);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open();showNewTab();", true);
    }
    // used for autocomplete list used on the followup web user control page
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        try
        {
            Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtCustomer = objcustomer.GetCustomerAllTrueDataForSearch(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] filterlist = new string[dtCustomer.Rows.Count];
            for (int i = 0; i < dtCustomer.Rows.Count; i++)
            {
                filterlist[i] = dtCustomer.Rows[i]["Name"].ToString() + "/" + dtCustomer.Rows[i]["Customer_Id"].ToString();
            }
            return filterlist;
        }
        catch
        {
            return null;
        }
    }
    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    //{
    //    try
    //    {
    //        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster();
    //        string id = string.Empty;

    //        id = WebUserControl_Followup.getCustid();
    //        DataTable dtCon = ObjContactMaster.GetContactTrueAllData(id, "Individual");
    //        string filtertext = "Name like '%" + prefixText + "%'";
    //        DataTable dt_Contact = new DataView(dtCon, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable(true, "Name", "Trans_Id");
    //        //DataTable DtCompany = ObjContactMaster.GetContactTrueById(id).DefaultView.ToTable(true, "Name", "Trans_Id");
    //        //dt.Merge(DtCompany);
    //        string[] filterlist = new string[dt_Contact.Rows.Count];
    //        if (dt_Contact.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt_Contact.Rows.Count; i++)
    //            {
    //                filterlist[i] = dt_Contact.Rows[i]["Name"].ToString() + "/" + dt_Contact.Rows[i]["Trans_Id"].ToString();
    //            }

    //            return filterlist;
    //        }
    //        else
    //        {
    //            DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
    //            string[] filterlistcon = new string[dtcon.Rows.Count];
    //            for (int i = 0; i < dtcon.Rows.Count; i++)
    //            {
    //                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
    //            }
    //            return filterlistcon;
    //        }
    //    }
    //    catch (Exception error)
    //    {

    //    }
    //    return null;
    //}
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText.ToString());
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString();
            }
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCallLogs(string prefixText, int count, string contextKey)
    {
        CallLogs call = new CallLogs(HttpContext.Current.Session["DBConnection"].ToString());

        string id = WebUserControl_Followup.getCustid();
        DataTable dt1 = call.GetCallLogsFor_CustomerID(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), id, prefixText);
        string[] txt = new string[dt1.Rows.Count];
        if (dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                txt[i] = dt1.Rows[i]["filteredText"].ToString();
            }
        }
        dt1.Dispose();
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVisitLogs(string prefixText, int count, string contextKey)
    {
        SM_WorkOrder visit = new SM_WorkOrder(HttpContext.Current.Session["DBConnection"].ToString());
        string id = WebUserControl_Followup.getCustid();
        DataTable dt1 = visit.GetWorkOrderNoPreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), id, prefixText);
        string[] txt = new string[dt1.Rows.Count];
        if (dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {

                txt[i] = dt1.Rows[i]["Work_Order_No"].ToString() + "/" + dt1.Rows[i]["Trans_Id"].ToString();
            }
        }
        dt1.Dispose();
        return txt;
    }
    protected void BtnAddTax_Command(object sender, CommandEventArgs e)
    {
        if ((ddlTransType.SelectedItem.ToString().ToLower() == "--select--" || ddlTransType.SelectedItem.ToString().ToLower() == "select") && ddlTransType.Visible == true)
        {
            DisplayMessage("Please Select Tax Transaction Type");
            return;
        }
        //if (e.CommandArgument.ToString() != "0")
        //{
        //    divTaxForNewProduct.Visible = false;
        //    gvTaxCalculation.Columns[0].Visible = false;
        //}
        //else
        //{
        //    divTaxForNewProduct.Visible = true;
        //    gvTaxCalculation.Columns[0].Visible = true;
        //}
        if (Session["Temp_Product_Tax_SQ"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_SQ"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {

                string F_Serial_No = string.Empty;

                if (e.CommandName.ToString() == "GvDetail")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("lblgvSerialNo");
                    F_Serial_No = Serial_No.Text;
                }

                //Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "' and Serial_No='" + F_Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Cal.Rows.Count > 0)
                {
                    //gvTaxCalculation.DataSource = Dt_Cal;
                    //gvTaxCalculation.DataBind();
                    int Row_Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
                    string Grid_Name = e.CommandName.ToString();
                    if (Grid_Name == "GvDetail")
                    {
                        //TextBox Unit_Price = (TextBox)GvDetail.Rows[Row_Index].FindControl("txtgvUnitPrice");
                        //TextBox Discount_Price = (TextBox)GvDetail.Rows[Row_Index].FindControl("txtgvDiscountV");
                       // Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        //Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    //Hdn_Serial_No_Tax.Value = F_Serial_No;
                    //hdnSerialNo.Value = F_Serial_No;
                    //Hdn_Product_Id_Tax.Value = e.CommandArgument.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
                }
                else
                {
                    //gvTaxCalculation.DataSource = null;
                    //gvTaxCalculation.DataBind();
                    ImageButton btn = (ImageButton)sender;
                    GridViewRow row = (GridViewRow)btn.NamingContainer;
                    ImageButton imagebutton = (ImageButton)row.FindControl("IbtnDelete");
                    TextBox price = (TextBox)row.FindControl("txtgvUnitPrice");
                    TextBox taxValue = (TextBox)row.FindControl("txtgvTaxV");
                    //hdnSerialNo.Value = imagebutton.CommandArgument;
                    //hdnAmount.Value = price.Text;
                    //hdnTaxAmount.Value = taxValue.Text;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
                }
            }
            Dt_Cal.Dispose();
        }
        else
        {
            //gvTaxCalculation.DataSource = null;
            //gvTaxCalculation.DataBind();
            ImageButton btn = (ImageButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            ImageButton imagebutton = (ImageButton)row.FindControl("IbtnDelete");
            TextBox price = (TextBox)row.FindControl("txtgvUnitPrice");
            TextBox taxValue = (TextBox)row.FindControl("txtgvTaxV");
            //hdnAmount.Value = price.Text;
            //hdnTaxAmount.Value = taxValue.Text;
            //hdnSerialNo.Value = imagebutton.CommandArgument;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
        }
    }
    protected void FillTax()
    {
        string TaxQuery = "Select Trans_Id as Id, Tax_Name as Name from Sys_TaxMaster where isActive='true'";
        DataTable Taxdt = objDa.return_DataTable(TaxQuery);
        if (Taxdt != null && Taxdt.Rows.Count > 0)
        {
            //ddltaxList.DataTextField = "Name";
            //ddltaxList.DataValueField = "Id";
            //ddltaxList.DataSource = Taxdt;
            //ddltaxList.DataBind();
            //ddltaxList.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }
    public void Get_Tax_From_DB()
    {
        try
        {
            string TaxQuery = string.Empty;
            bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (IsTax == true)
            {
                if (ddlTransType.SelectedIndex > 0)
                {
                    if (editid.Value != "")
                    {
                        DataTable DT_Db_Details = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, Session["FinanceYearId"].ToString());
                        if (DT_Db_Details.Rows.Count > 0)
                        {
                            TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesQuotationDetail IPID on IPID.Trans_Id=TRD.Field2  where TRD.Ref_Id='" + editid.Value + "' and TRD.Ref_Type='SQ' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                            DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                            Session["Temp_Product_Tax_SQ"] = null;
                            DataTable Dt_Temp = new DataTable();
                            Dt_Temp = TemporaryProductWiseTaxes();
                            Dt_Temp = Dt_Inv_TaxRefDetail;
                            if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                            {
                                Session["Temp_Product_Tax_SQ"] = Dt_Temp;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public double Get_Tax_Percentage(string ProductId, string Serial_No)
    {
        double TotalTax_Percentage = 0;
        bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_SQ"] != null)
            {
                DataTable Dt_Session_Tax = Session["Temp_Product_Tax_SQ"] as DataTable;
                if (Dt_Session_Tax.Rows.Count > 0)
                {
                    //Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
                    Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "' and Serial_No='" + Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow DRT in Dt_Session_Tax.Rows)
                    {
                        TotalTax_Percentage = TotalTax_Percentage + Convert.ToDouble(DRT["Tax_Value"].ToString());
                    }
                }
            }
        }
        return TotalTax_Percentage;
    }
    public double Get_Tax_Amount(string Amount, string ProductId, string Serial_No)
    {
        double TotalTax_Amount = 0;
        //if (hdnIsTaxApplicable.Value == "true")
        //{
            if (Session["Temp_Product_Tax_SQ"] != null)
            {
                double Tax_Value = Get_Tax_Percentage(ProductId, Serial_No);
                double Temp_Amount = Convert.ToDouble(Amount);
                TotalTax_Amount = Convert.ToDouble(GetAmountDecimal(((Tax_Value * Temp_Amount) / 100).ToString()));
            }
        //}
        return TotalTax_Amount;
    }
    public void Add_Tax_In_Session(string Amount, string ProductId, string Serial_No)
    {
        string TaxQuery = string.Empty;
        Amount = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
       
            Get_Tax_Parameter();
            String Condition = string.Empty;
            if (Hdn_Tax_By.Value == Resources.Attendance.Company)
                Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Company_ID = " + Session["CompId"].ToString() + "";
            else if (Hdn_Tax_By.Value == Resources.Attendance.Location)
                Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Location_ID = " + Session["LocId"].ToString() + "";
            if (ddlTransType.SelectedIndex > 0)
            {
                TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage as Tax_Value,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
                            where ((',' + RTRIM(STM.Field2) + ',') LIKE '%,' + CONVERT(nvarchar(100)," + ddlTransType.SelectedValue + ") + ',%') and IPTM.Product_Id = " + ProductId + "" + Condition + "";
                DataTable dtTax = objDa.return_DataTable(TaxQuery);
                double TotalPriceBeforeDiscount = double.Parse(Amount);
                DataTable dt = new DataTable();
                if (Session["Temp_Product_Tax_SQ"] == null)
                    dt = TemporaryProductWiseTaxes();
                else
                    dt = (DataTable)Session["Temp_Product_Tax_SQ"];
                if (dtTax.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTax.Rows)
                    {
                        double taxvalue = double.Parse(dr["Tax_Value"].ToString());
                        double taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
                        DataRow Newdr = dt.NewRow();
                        Newdr["Product_Id"] = ProductId;
                        Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
                        Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
                        Newdr["Tax_Value"] = dr["Tax_Value"].ToString();
                        Newdr["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                        Newdr["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
                        Newdr["Serial_No"] = Serial_No;
                        DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + " AND Serial_No = '" + Serial_No + "'");
                        //DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
                        if (SRow.Length == 0)
                        {
                            try
                            {
                                dt.Rows.Add(Newdr);
                            }
                            catch (Exception err)
                            {

                            }

                        }
                        else
                        {
                            taxvalue = double.Parse(SRow[0]["Tax_Value"].ToString());
                            taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
                            SRow[0]["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Serial_No"] = Serial_No;
                        }
                    }
                    Session["Temp_Product_Tax_SQ"] = dt;
                }
            
        }
    }    
    protected void Btn_Update_Tax_Click(object sender, EventArgs e)
    {
        //string Product_ID = Hdn_Product_Id_Tax.Value;
        //string Unit_Price = Hdn_unit_Price_Tax.Value;
        //string Unit_Discount = Hdn_Discount_Tax.Value;
        //string Net_Unit_Price = (Convert.ToDouble(Unit_Price) - Convert.ToDouble(Unit_Discount)).ToString();
        //string Serial_No = Hdn_Serial_No_Tax.Value;

        if (Session["Temp_Product_Tax_SQ"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_SQ"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {
                //foreach (DataRow DR_Tax in Dt_Cal.Rows)
                //{
                //    foreach (GridViewRow GVR in gvTaxCalculation.Rows)
                //    {
                //        TextBox Tax_Percentage = (TextBox)GVR.FindControl("txtTaxValueInPer");
                //        HiddenField TaxId = (HiddenField)GVR.FindControl("lblgvTaxId");
                //        HiddenField ProductId = (HiddenField)GVR.FindControl("lblgvProductId");
                //        if (Tax_Percentage.Text == "")
                //            Tax_Percentage.Text = "0.00";
                //        //if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value)
                //        if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value && DR_Tax["Serial_No"].ToString() == Serial_No)
                //        {
                //            DR_Tax["Tax_Value"] = Tax_Percentage.Text;
                //            DR_Tax["TaxAmount"] = (Convert.ToDouble(Net_Unit_Price) * Convert.ToDouble(Tax_Percentage.Text)) / 100;
                //            DR_Tax["Amount"] = Net_Unit_Price;
                //        }
                //    }
                //}
            }
            Session["Temp_Product_Tax_SQ"] = Dt_Cal;
            //foreach (GridViewRow dl in GvDetail.Rows)
            //{
            //    if (Dt_Cal.Rows.Count > 0)
            //    {
            //        //DataTable Dt_Cal_Temp = new DataView(Dt_Cal, "Product_Id='" + Product_ID + "'", "", DataViewRowState.CurrentRows).ToTable();
            //        DataTable Dt_Cal_Temp = new DataView(Dt_Cal, "Product_Id='" + Product_ID + "' And Serial_No='" + Serial_No + "' ", "", DataViewRowState.CurrentRows).ToTable();
            //        if (Dt_Cal_Temp.Rows.Count > 0)
            //        {
            //            foreach (DataRow Dr in Dt_Cal_Temp.Rows)
            //            {
            //                DataTable Dt_Cal_Temp2 = Dt_Cal_Temp.DefaultView.ToTable(true, "Tax_Id", "Tax_Value");
            //                double Tax_Val = 0;
            //                foreach (DataRow DRR in Dt_Cal_Temp2.Rows)
            //                {
            //                    Tax_Val = Tax_Val + Convert.ToDouble(DRR["Tax_Value"].ToString());
            //                }
            //                TextBox Tax_Percent = (TextBox)dl.FindControl("txtgvTaxP");
            //                HiddenField hdnProductId = (HiddenField)dl.FindControl("hdnProductId");
            //                Label lblgvSerialNo = (Label)dl.FindControl("lblgvSerialNo");
            //                if (Product_ID == hdnProductId.Value && Serial_No == lblgvSerialNo.Text)
            //                {
            //                    Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
            //                }
            //            }
            //        }
            //    }
            //}
            HeadearCalculateGrid();
            //Hdn_Product_Id_Tax.Value = "";
            //Hdn_Serial_No_Tax.Value = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
        }
    }
    public void Insert_Tax(string Grid_Name, string PQ_Header_ID, string Detail_ID, GridViewRow Gv_Row, ref SqlTransaction trns)
    {
        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("SQ", PQ_Header_ID.ToString(), Detail_ID.ToString(), ref trns);
        string R_Product_ID = string.Empty;
        string R_Order_Req_Qty = string.Empty;
        string R_Unit_Price = string.Empty;
        string R_Discount_Value = string.Empty;
        string R_Serial_No = string.Empty;
        string Grid = string.Empty;
        if (Grid_Name == "GvDetail")
        {
            Grid = "GvDetail";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdnProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("lblgvQuantity");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtgvUnitPrice");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("txtgvDiscountV");
            Label lblSerialNO = (Label)Gv_Row.FindControl("lblgvSerialNo");
            R_Serial_No = lblSerialNO.Text;
            R_Product_ID = Product_ID.Value;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
        }
        if (Grid != "")
        {
            double A_Unit_Cost = Convert.ToDouble(R_Unit_Price) * Convert.ToDouble(R_Order_Req_Qty);
            double A_Unit_Discount = Convert.ToDouble(R_Discount_Value) * Convert.ToDouble(R_Order_Req_Qty);
            double Net_Amount = A_Unit_Cost - A_Unit_Discount;
            //Get_Tax_Insert(R_Product_ID);
            DataTable ProductTax = new DataTable();
            if (Session["Temp_Product_Tax_SQ"] == null)
                ProductTax = TemporaryProductWiseTaxes();
            else
                ProductTax = (DataTable)Session["Temp_Product_Tax_SQ"];
            string ProductId = string.Empty;
            string TaxId = string.Empty;
            string TaxValue = string.Empty;
            string TaxAmount = string.Empty;
            string Amount = string.Empty;
            if (ProductTax != null && ProductTax.Rows.Count > 0)
            {
                foreach (DataRow dr in ProductTax.Rows)
                {
                    //if (dr["Product_Id"].ToString() == R_Product_ID)
                    if (dr["Product_Id"].ToString() == R_Product_ID && dr["Serial_No"].ToString() == R_Serial_No)
                    {
                        ProductId = dr["Product_Id"].ToString();
                        TaxId = dr["Tax_Id"].ToString();
                        TaxValue = GetAmountDecimal(dr["Tax_Value"].ToString());
                        TaxAmount = dr["TaxAmount"].ToString();
                        Amount = Net_Amount.ToString();
                        //if (Convert.ToDouble(Amount) != 0)
                        objTaxRefDetail.InsertRecord("SQ", PQ_Header_ID, "0", "0", ProductId, TaxId, TaxValue, TaxAmount, false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
            }
        }
    }
    public double Get_Discount_Percentage(string Unit_Price, string Discount_Amount)
    {
        try
        {
            double Discount_Percent = 0;
            if (hdnIsDiscountApplicable.Value == "true")
            {
                double D_Unit_Price = Convert.ToDouble(Unit_Price);
                double D_Discount_Amount = Convert.ToDouble(Discount_Amount);
                Discount_Percent = (D_Discount_Amount / D_Unit_Price) * 100;
            }
            return Discount_Percent;
        }
        catch
        {
            return 0;
        }
    }
    public double Get_Total_Tax_Percentage(string Unit_Price, string Tax_Amount)
    {
        try
        {
            double Tax_Percent = 0;
            if (hdnIsTaxApplicable.Value == "true")
            {
                double D_Unit_Price = Convert.ToDouble(Unit_Price);
                double D_Tax_Amount = Convert.ToDouble(Tax_Amount);
                Tax_Percent = (D_Tax_Amount / D_Unit_Price) * 100;
            }
            return Tax_Percent;
        }
        catch
        {
            return 0;
        }
    }
    public double Get_Discount_Amount(string Unit_Price, string Discount_Percent)
    {
        try
        {
            double Discount_Amount = 0;
            if (hdnIsDiscountApplicable.Value == "true")
            {
                double D_Unit_Price = Convert.ToDouble(Unit_Price);
                double D_Discount_Percent = Convert.ToDouble(Discount_Percent);
                Discount_Amount = (D_Unit_Price * D_Discount_Percent) / 100;
            }
            return Discount_Amount;
        }
        catch
        {
            return 0;
        }
    }
    public double Get_Net_Amount(string Unit_Price, string Discount_Percent, string Product_Id, string Serial_No)
    {
        try
        {
            double Net_Amount = 0;
            double D_Unit_Price = Convert.ToDouble(Unit_Price);
            double D_Discount_Percent = Convert.ToDouble(Discount_Percent);
            double Discount_Amount = (D_Unit_Price * D_Discount_Percent) / 100;
            double Tax_Amount = Get_Tax_Amount((D_Unit_Price - Discount_Amount).ToString(), Product_Id, Serial_No);
            Net_Amount = (D_Unit_Price - Discount_Amount) + Tax_Amount;
            return Net_Amount;
        }
        catch
        {
            return 0;
        }
    }
    public void HeadearCalculateGrid()
    {
        double F_Gross_Total = 0;
        double F_Discount_Per = 0;
        double F_Discount_Value = 0;
        double F_Tax_Per = 0;
        double F_Tax_Value = 0;
        double F_Net_Total = 0;


        double Gross_Unit_Price = 0;
        double Gross_Discount_Amount = 0;
        double Gross_Tax_Amount = 0;
        double Gross_Line_Total = 0;

        int totalRowsWithTax = 0;
        double totalTaxPercentage = 0;

        //foreach (GridViewRow gvr in GvDetail.Rows)
        //{
        //    Label lblgvSerialNo = (Label)gvr.FindControl("lblgvSerialNo");
        //    Label Product_Code = (Label)gvr.FindControl("lblgvProductcode");
        //    HiddenField Product_Id = (HiddenField)gvr.FindControl("hdnProductId");
        //    HiddenField Unit_Id = (HiddenField)gvr.FindControl("hdnUnitId");
        //    Label Unit_Name = (Label)gvr.FindControl("lblgvUnit");
        //    HiddenField Currency_ID = (HiddenField)gvr.FindControl("hdnCurrencyId");
        //    Label Currency_Name = (Label)gvr.FindControl("lblgvCurrency");
        //    TextBox Quantity = (TextBox)gvr.FindControl("lblgvQuantity");
        //    Label Estimated_Price = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");
        //    TextBox Unit_Price = (TextBox)gvr.FindControl("txtgvUnitPrice");
        //    TextBox Total_Quantity_Price = (TextBox)gvr.FindControl("txtgvQuantityPrice");
        //    TextBox Discount_Percentage = (TextBox)gvr.FindControl("txtgvDiscountP");
        //    TextBox Discount_Amount = (TextBox)gvr.FindControl("txtgvDiscountV");
        //    TextBox Price_After_Discount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
        //    TextBox Tax_Percentage = (TextBox)gvr.FindControl("txtgvTaxP");
        //    TextBox Tax_Amount = (TextBox)gvr.FindControl("txtgvTaxV");
        //    TextBox Amount_After_Tax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
        //    TextBox Line_Total = (TextBox)gvr.FindControl("txtgvTotal");
        //    TextBox Agent_Commission = (TextBox)gvr.FindControl("txtgvAgentCommission");


        //    if (Unit_Price.Text == "")
        //        Unit_Price.Text = "0";

        //    if (Quantity.Text == "")
        //        Quantity.Text = "0";

        //    if (Discount_Percentage.Text == "")
        //        Discount_Percentage.Text = "0";

        //    double F_Unit_Price = double.Parse(Unit_Price.Text);
        //    double F_Order_Quantity = double.Parse(Quantity.Text);
        //    double F_Discount_Percentage = double.Parse(Discount_Percentage.Text);
        //    double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
        //    //if (Hdn_Get_Inquity.Value != "")
        //  //  Add_Tax_In_Session((F_Unit_Price - F_Discount_Amount).ToString(), Product_Id.Value, lblgvSerialNo.Text);
        //    double F_Tax_Percentage = Get_Tax_Percentage(Product_Id.Value, lblgvSerialNo.Text);
        //    double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), Product_Id.Value, lblgvSerialNo.Text);
        //    double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
        //    double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;

        //    Discount_Percentage.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
        //    Discount_Amount.Text = GetAmountDecimal(F_Discount_Amount.ToString());
        //    Tax_Percentage.Text = GetAmountDecimal(F_Tax_Percentage.ToString());


        //    if (Tax_Percentage.Text != "" && Tax_Percentage.Text != "0" && Tax_Percentage.Text != "0.00")
        //    {
        //        totalTaxPercentage += Convert.ToDouble(Tax_Percentage.Text);
        //        totalRowsWithTax++;
        //    }

        //    Tax_Amount.Text = GetAmountDecimal(F_Tax_Amount.ToString());
        //    Line_Total.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
        //    Total_Quantity_Price.Text = (F_Unit_Price * F_Order_Quantity).ToString();

        //    Gross_Unit_Price = Gross_Unit_Price + (F_Unit_Price * F_Order_Quantity);
        //    Gross_Discount_Amount = Gross_Discount_Amount + (F_Discount_Amount * F_Order_Quantity);
        //    Gross_Tax_Amount = Gross_Tax_Amount + (F_Tax_Amount * F_Order_Quantity);
        //    Gross_Line_Total = Gross_Line_Total + F_Row_Total_Amount;
        //}
        txtAmount.Text = GetAmountDecimal(Gross_Unit_Price.ToString());
        txtDiscountV.Text = GetAmountDecimal(Gross_Discount_Amount.ToString());
        txtTaxV.Text = GetAmountDecimal(Gross_Tax_Amount.ToString());
        txtPriceAfterTax.Text = GetAmountDecimal(Gross_Line_Total.ToString());
        txtTotalAmount.Text = GetAmountDecimal(Gross_Line_Total.ToString());
        txtDiscountP.Text = GetAmountDecimal(Get_Discount_Percentage(Gross_Unit_Price.ToString(), Gross_Discount_Amount.ToString()).ToString());
        //txtTaxP.Text = GetAmountDecimal(Get_Total_Tax_Percentage((Gross_Unit_Price - Gross_Discount_Amount).ToString(), Gross_Tax_Amount.ToString()).ToString());
        if (totalRowsWithTax != 0)
        {
            txtTaxP.Text = GetAmountDecimal((totalTaxPercentage / totalRowsWithTax).ToString());
        }

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetDistinctEmailId(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        dt.Dispose();
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        //DataTable dt = new EmployeeMaster().GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentMaster();

        dt = new DataView(dt, "Dep_Name like '%" + prefixText + "%'", "Dep_Name asc", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
        }
        dt.Dispose();
        return str;

    }
    protected void btnNewaddress_Click(object sender, EventArgs e)
    {
        if (hdnCustomerId.Value == "")
        {
            DisplayMessage("Please Select a Customer To Add New Address");
            return;
        }
        addaddress.Reset();

        Country_Currency objCountryCurrency = new Country_Currency(HttpContext.Current.Session["DBConnection"].ToString());

        ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(Session["LocCurrencyId"].ToString(), "2").Rows[0]["Country_Id"].ToString();

        if (ViewState["Country_Id"] != null)
        {
            addaddress.BtnNew_click(ViewState["Country_Id"].ToString());
            Session["AddCtrl_Country_Id"] = ViewState["Country_Id"].ToString();
        }

        addaddress.fillGridAdd(hdnCustomerId.Value);
        addaddress.setCustomerID(hdnCustomerId.Value);
        addaddress.fillHeader(txtCustomer.Text);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_NewAddress_Open();displayList()", true);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListStateName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["AddCtrl_Country_Id"].ToString() == "")
        {
            return null;
        }
        StateMaster objStateMaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objStateMaster.GetAllStateByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_Country_Id"].ToString());
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["State_Name"].ToString();
        }
        dt.Dispose();
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCityName(string prefixText, int count, string contextKey)
    {
        try
        {
            if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "" || HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "0")
            {
                return null;
            }
            CityMaster objCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objCityMaster.GetAllCityByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_State_Id"].ToString());
            string[] txt = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["City_Name"].ToString();
            }
            return txt;
        }
        catch
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void resetAddress()
    {
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ddlCountry_IndexChanged(string CountryId)
    {
        CountryMaster ObjSysCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
        return "+" + ObjSysCountryMaster.GetCountryMasterById(CountryId).Rows[0]["Country_Code"].ToString();
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCity_TextChanged(string stateId, string cityName)
    {
        CityMaster ObjCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());

        string City_id = ObjCityMaster.GetCityIdFromStateIdNCityName(stateId, cityName);

        if (City_id != "")
        {
            return City_id;
        }
        else
        {
            return "0";
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtState_TextChanged(string CountryId, string StateName)
    {
        StateMaster ObjStatemaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string stateId = ObjStatemaster.GetStateIdFromCountryIdNStateName(CountryId, StateName);
        if (stateId != "")
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = stateId;
            return stateId;
        }
        else
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
            return "0";
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static int txtAddressNameNew_TextChanged(string AddressName, string addressId)
    {
        // return  1 when 'Address Name Already Exists' and 0 when not present
        Set_AddressMaster AM = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string data = AM.GetAddressDataExistOrNot(AddressName, addressId);
        if (data == "0")
        {
            return 0;
        }
        else
        {
            return 1;
        }

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    {
        ContactNoMaster objContactNumMaster = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Phone_no"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] lblgvSQNo_Command(string quotationId, string locationid)
    {
        string[] result = new string[2];
        string orderId = "0";
        orderId = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString()).get_SingleValue("SELECT Trans_Id FROM Inv_SalesOrderHeader WHERE Company_Id = '" + HttpContext.Current.Session["CompId"].ToString() + "'   AND Brand_Id = '" + HttpContext.Current.Session["BrandId"].ToString() + "' AND Location_Id = '" + locationid + "' AND IsActive = 'True'    AND SOfromTransType = 'Q'   AND SOfromTransNo = '" + quotationId + "'");
        orderId = orderId == "@NOTFOUND@" ? "0" : orderId;
        if (orderId != "0")
        {
            if (!hasSalesOrderPermission())
            {
                result[0] = "false";
                result[1] = "User have no permission to view sales order ";
                return result;
            }
            else
            {
                result[0] = "true";
                result[1] = orderId;
                return result;
            }
        }
        else
        {
            result[0] = "false";
            result[1] = "Order Not found";
            return result;
        }
    }
    public static bool hasSalesOrderPermission()
    {
        bool isAllow = false;
        //here we checking user permission for view sales order info 
        if (HttpContext.Current.Session["EmpId"].ToString() == "0")
        {
            isAllow = true;
        }
        else
        {
            using (DataTable dtAllpagecode = new Common(HttpContext.Current.Session["DBConnection"].ToString()).GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), "144", "67", HttpContext.Current.Session["CompId"].ToString()))
            {
                if (dtAllpagecode.Rows.Count > 0)
                {
                    isAllow = true;
                }
            }
        }

        return isAllow;
    }


    protected void btnFillRelatedProducts_Click(object sender, EventArgs e)
    {
        FillRelatedProduct(hdnNewProductId.Value);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static bool isDiscountApplicable()
    {
        return Inventory_Common.IsSalesDiscountEnabled(HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static bool isTaxApplicable()
    {
        return Inventory_Common.IsSalesTaxEnabled(HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetTaxPerameter()
    {
        try
        {
            bool IsTax = Inventory_Common.IsSalesTaxEnabled(HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            return IsTax.ToString();
        }
        catch (Exception ex)
        {
            return "";
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string GetAddTaxPercentage(string ProductId,string transType)
    {
        Add_Tax_To_Session("0", ProductId, "", transType);
        double TotalTax_Percentage = 0;
        bool IsTax = Inventory_Common.IsSalesTaxEnabled(HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (HttpContext.Current.Session["Temp_Product_Tax_SQ"] != null)
            {
                DataTable Dt_Session_Tax = HttpContext.Current.Session["Temp_Product_Tax_SQ"] as DataTable;
                if (Dt_Session_Tax.Rows.Count > 0)
                {
                    //Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
                    Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow DRT in Dt_Session_Tax.Rows)
                    {
                        TotalTax_Percentage = TotalTax_Percentage + Convert.ToDouble(DRT["Tax_Value"].ToString());
                    }
                }
            }
        }
        return TotalTax_Percentage.ToString();        
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static void Add_Tax_To_Session(string Amount, string ProductId, string Serial_No, string transType)
    {
        string TaxQuery = string.Empty;
        bool IsTax = Inventory_Common.IsSalesTaxEnabled(HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = HttpContext.Current.Session["LocCurrencyId"].ToString();
        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            strForienAmount = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString()).GetCurencyConversionForInv(HttpContext.Current.Session["LocCurrencyId"].ToString(), (Convert.ToDouble(strExchangeRate) * Convert.ToDouble(Amount.ToString())).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0/0";
        }

        Amount = strForienAmount.Split('/')[0].ToString();
        if (IsTax)
        {
            string taxBy = getTaxParameter();
            String Condition = string.Empty;
            if (taxBy == Resources.Attendance.Company)
                Condition = "AND IPTM.Field1='" + taxBy + "' AND IPTM.Company_ID = " + HttpContext.Current.Session["CompId"].ToString() + "";
            else if (taxBy == Resources.Attendance.Location)
                Condition = "AND IPTM.Field1='" + taxBy + "' AND IPTM.Location_ID = " + HttpContext.Current.Session["LocId"].ToString() + "";
            if (transType != "0")
            {
                TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage as Tax_Value,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
                            where ((',' + RTRIM(STM.Field2) + ',') LIKE '%,' + CONVERT(nvarchar(100)," + transType + ") + ',%') and IPTM.Product_Id = " + ProductId + "" + Condition + "";
                DataTable dtTax = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString()).return_DataTable(TaxQuery);
                double TotalPriceBeforeDiscount = double.Parse(Amount);
                DataTable dt = new DataTable();
                if (HttpContext.Current.Session["Temp_Product_Tax_SQ"] == null)
                {
                    dt.Columns.Add("Product_Id", typeof(float));
                    dt.Columns.Add("Tax_Id", typeof(float));
                    dt.Columns.Add("Tax_Name", typeof(string));
                    dt.Columns.Add("Tax_Value", typeof(float));
                    dt.Columns.Add("TaxAmount", typeof(float));
                    dt.Columns.Add("Amount", typeof(float));                                    
                }
                else
                {
                    dt = (DataTable)HttpContext.Current.Session["Temp_Product_Tax_SQ"];
                }                   
                if (dtTax.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTax.Rows)
                    {
                        double taxvalue = double.Parse(dr["Tax_Value"].ToString());
                        double taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
                        DataRow Newdr = dt.NewRow();
                        Newdr["Product_Id"] = ProductId;
                        Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
                        Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
                        Newdr["Tax_Value"] = dr["Tax_Value"].ToString();
                        Newdr["TaxAmount"] = GetCurrencyAmt(HttpContext.Current.Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                        Newdr["Amount"] = GetCurrencyAmt(HttpContext.Current.Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
                        DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
                        //DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
                        if (SRow.Length == 0)
                        {
                            dt.Rows.Add(Newdr);
                        }
                        else
                        {
                            taxvalue = double.Parse(SRow[0]["Tax_Value"].ToString());
                            taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
                            SRow[0]["TaxAmount"] = GetCurrencyAmt(HttpContext.Current.Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Amount"] = GetCurrencyAmt(HttpContext.Current.Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
                            
                        }
                    }
                    HttpContext.Current.Session["Temp_Product_Tax_SQ"] = dt;
                }
            }
        }
    }
    public static string getTaxParameter()
    {
        DataTable Dt_Parameter = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString()).GetSysParameterByParamName("Tax System");
        if (Dt_Parameter != null && Dt_Parameter.Rows.Count > 0)
        {
            if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Company")
            {
                return "Company";
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Location")
            {
                return "Location";
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "System")
            {
                return "System";
            }
            else
            {
                return "Select";
            }
        }
        return "select";
    }
    public static string GetCurrencyAmt(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = HttpContext.Current.Session["LocCurrencyId"].ToString();
        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            strForienAmount = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString()).GetCurencyConversionForInv(strToCurrency, (Convert.ToDouble(strExchangeRate) * Convert.ToDouble(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string getTaxAmount(string Amount, string ProductId, string Serial_No)
    {
        double TotalTax_Amount = 0;
        if (HttpContext.Current.Session["Temp_Product_Tax_SQ"] != null)
        {
            double Tax_Value = Convert.ToDouble(getTaxPercentage(ProductId, Serial_No));
            double Temp_Amount = Convert.ToDouble(Amount);
            TotalTax_Amount = ((Tax_Value * Temp_Amount) / 100);
        }
        return TotalTax_Amount.ToString();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string getTaxPercentage(string ProductId, string Serial_No)
    {
        double TotalTax_Percentage = 0;
        if (HttpContext.Current.Session["Temp_Product_Tax_SQ"] != null)
        {
            DataTable Dt_Session_Tax = HttpContext.Current.Session["Temp_Product_Tax_SQ"] as DataTable;
            if (Dt_Session_Tax.Rows.Count > 0)
            {
                //Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
                Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "' and Serial_No='" + Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                foreach (DataRow DRT in Dt_Session_Tax.Rows)
                {
                    TotalTax_Percentage = TotalTax_Percentage + Convert.ToDouble(DRT["Tax_Value"].ToString());
                }
            }
        }
        return TotalTax_Percentage.ToString();
    }

    protected void btn_bin_Click(object sender, EventArgs e)
    {
        FillGridBin();
    }

    //protected void btnAddTax_Click(object sender, EventArgs e)
    //{
    //    if (ddltaxList.SelectedIndex == 0)
    //    {
    //        DisplayMessage("Please Select A Tax");
    //        txtTaxValue.Focus();
    //        return;
    //    }
    //    if (txtTaxValue.Text == "")
    //    {
    //        DisplayMessage("Please Enter Tax Value");
    //        txtTaxValue.Focus();
    //        return;
    //    }

    //    DataTable dt = Session["Temp_Product_Tax_SQ"] as DataTable;

    //    if (Session["Temp_Product_Tax_SQ"] == null)
    //    {
    //        dt = TemporaryProductWiseTaxes();
    //        dt.Rows.Add("0", ddltaxList.SelectedValue, ddltaxList.SelectedItem, txtTaxValue.Text, hdnTaxAmount.Value, hdnAmount.Value, hdnSerialNo.Value);
    //        gvTaxCalculation.DataSource = dt;
    //        gvTaxCalculation.DataBind();
    //        Session["Temp_Product_Tax_SQ"] = dt;
    //        ddltaxList.SelectedIndex = 0;
    //        txtTaxValue.Text = "";
    //    }
    //    else
    //    {
    //        using (DataTable dtTaxName = new DataView(dt, "tax_name ='" + ddltaxList.SelectedItem + "' and serial_no ='" + hdnSerialNo.Value + "'", "", DataViewRowState.CurrentRows).ToTable())
    //        {
    //            if (dtTaxName.Rows.Count > 0)
    //            {
    //                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "alert('Selected Tax Has been applied already')", true);
    //                return;
    //            }
    //        }

    //        DataTable dt1 = new DataView(dt, "serial_no =" + hdnSerialNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
    //        decimal totalTax = 0;

    //        //if (dt1.Rows.Count==0)
    //        //{
    //        totalTax = Convert.ToDecimal(txtTaxValue.Text);
    //        //}
    //        //else
    //        //{
    //        //    for (int i = 0; i < dt1.Rows.Count; i++)
    //        //    {
    //        //        totalTax += Convert.ToDecimal(dt1.Rows[i]["tax_value"].ToString());
    //        //    }
    //        //    totalTax += Convert.ToDecimal(txtTaxValue.Text);
    //        //}

    //        int index = Convert.ToInt32(hdnSerialNo.Value) - 1;
    //       // TextBox unitPrice = GvDetail.Rows[index].FindControl("txtgvUnitPrice") as TextBox;
    //       // unitPrice.Text = unitPrice.Text == "" ? "0" : unitPrice.Text;
    //        //hdnTaxAmount.Value = (Convert.ToDecimal(unitPrice.Text) * totalTax / 100).ToString();


    //        //dt1.Rows.Add("0", ddltaxList.SelectedValue, ddltaxList.SelectedItem, txtTaxValue.Text, hdnTaxAmount.Value, unitPrice.Text, hdnSerialNo.Value);
    //        gvTaxCalculation.DataSource = dt1;
    //        gvTaxCalculation.DataBind();

    //        //if(Lbl_Tab_New.Text.ToLower()=="new")
    //        //{
    //        DataTable alldata = Session["Temp_Product_Tax_SQ"] as DataTable;
    //        alldata = new DataView(alldata, "serial_no <>" + hdnSerialNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
    //        alldata.Merge(dt1);
    //        Session["Temp_Product_Tax_SQ"] = alldata;
    //        //}
    //        //else
    //        //{
    //        //    DataTable alldata = Session["Temp_Product_Tax_SQ"] as DataTable;
    //        //    alldata.Merge(dt1);
    //        //    Session["Temp_Product_Tax_SQ"] = alldata;
    //        //    alldata = null;
    //        //}

    //        ddltaxList.SelectedIndex = 0;
    //        txtTaxValue.Text = "";
    //        dt1 = null;
    //    }
    //    btnSaveTax_Click(dt);
    //    dt = null;
    //}

    public void btnSaveTax_Click(DataTable dt)
    {
        if (Session["Temp_Product_Tax_SQ"] != null)
        {
            //decimal totalTax = 0;
            ////DataTable dt = Session["Temp_Product_Tax_SQ"] as DataTable;
            //dt = new DataView(dt, "serial_no ='" + hdnSerialNo.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    totalTax += Convert.ToDecimal(dt.Rows[i]["tax_value"].ToString());
            //}
            //int index = Convert.ToInt32(hdnSerialNo.Value) - 1;
            //(GvDetail.Rows[index].FindControl("txtgvTaxP") as TextBox).Text = totalTax.ToString();
          //  Label taxName = GvDetail.Rows[index].FindControl("lblTaxName") as Label;
           // taxName.Text = getTaxName(hdnSerialNo.Value);
            //TextBox unitPrice = GvDetail.Rows[index].FindControl("txtgvUnitPrice") as TextBox;
            //unitPrice.Text = unitPrice.Text == "" ? "0" : unitPrice.Text;
            //(GvDetail.Rows[index].FindControl("txtgvTaxV") as TextBox).Text = (Convert.ToDecimal(unitPrice.Text) * totalTax / 100).ToString();
            HeadearCalculateGrid();
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
        }
    }

    protected void gvtaxDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = Session["Temp_Product_Tax_SQ"] as DataTable;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["tax_id"].ToString() == e.CommandName.ToString() && dt.Rows[i]["serial_no"].ToString() == e.CommandArgument.ToString())
            {
                dt.Rows.RemoveAt(i);
                break;
            }
        }
        Session["Temp_Product_Tax_SQ"] = dt;
        DataTable dt1 = new DataView(dt, "serial_no ='" + e.CommandArgument + "' ", "", DataViewRowState.CurrentRows).ToTable();
        //gvTaxCalculation.DataSource = dt1;
        //gvTaxCalculation.DataBind();
        btnSaveTax_Click(dt);
        dt = null;
        dt1 = null;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = string.Empty;
        if (HttpContext.Current.Session["ContactID"] != null)
        {
            id = HttpContext.Current.Session["ContactID"].ToString();
        }
        else
        {
            id = "0";
        }
        DataTable dt = ObjContactMaster.GetContactAsPerFilterText(prefixText, id);
        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Filtertext"].ToString();
            }
            dt = null;
            return filterlist;
        }
        else
        {
            DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
            string[] filterlistcon = new string[dtcon.Rows.Count];
            for (int i = 0; i < dtcon.Rows.Count; i++)
            {
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            dtcon = null;
            return filterlistcon;
        }
    }
    public void setTaxNameFromDetailTable()
    {
        if (Trans_Div.Visible == true && Session["Temp_Product_Tax_SQ"] != null)
        {
            //for (int i = 0; i < GvDetail.Rows.Count; i++)
            //{
            //    (GvDetail.Rows[i].FindControl("lblTaxName") as Label).Text = getTaxName((i + 1).ToString());
            //}
        }
    }

    public string getTaxName(string serialNo)
    {
        string taxName = "";

        if (Trans_Div.Visible == true && Session["Temp_Product_Tax_SQ"] != null)
        {
            DataTable dt = Session["Temp_Product_Tax_SQ"] as DataTable;
            dt = new DataView(dt, "serial_no='" + serialNo + "'", "", DataViewRowState.CurrentRows).ToTable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((dt.Rows.Count - 1) == i)
                {
                    taxName = taxName + dt.Rows[i]["tax_name"].ToString() + " (" + dt.Rows[i]["tax_value"].ToString() + ")";
                }
                else
                {
                    taxName = taxName + dt.Rows[i]["tax_name"].ToString() + " (" + dt.Rows[i]["tax_value"].ToString() + "), ";
                }
            }
            dt = null;
        }
        return taxName;
    }

    public void FillUser()
    {
        try
        {
            string strEmpId = string.Empty;
            string strLocationDept = string.Empty;
            string strLocId = Session["LocId"].ToString();

            strLocId = ddlLocation.SelectedValue;
            strLocationDept = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (strLocationDept == "")
            {
                strLocationDept = "0,";
            }
            strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept);


            DataTable dtEmp = new DataTable();

            string isSingle = ObjUser.CheckIsSingleUser(Session["UserId"].ToString(), Session["CompId"].ToString());
            bool IsSingleUser = false;
            try
            {
                IsSingleUser = Convert.ToBoolean(isSingle);
            }
            catch
            {
                IsSingleUser = false;
            }

            string sharedSalesPersons = string.Empty;
            if (Session["EmpId"].ToString() != "0" && IsSingleUser == true)
            {
                sharedSalesPersons = objDa.get_SingleValue("select top 1 Param_Value from set_employee_parameter_new where Param_Name='SharedSalesPersons' and Company_Id='" + Session["CompId"].ToString() + "' and EmpId='" + Session["EmpId"].ToString() + "' and IsActive='true'");
            }
            // can see multiple employee data
            if (IsSingleUser == false)
            {
                //for normal user
                if (Session["EmpId"].ToString() != "0")
                {
                    dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString(), strEmpId);
                    //dtEmp = new DataView(dtEmp, "Emp_Id in (" + strEmpId.Substring(0, strEmpId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    //for super admin
                    if (ddlLocation.SelectedIndex > 0)
                    {
                        dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                        dtEmp = new DataView(dtEmp, "Location_Id=" + ddlLocation.SelectedValue.Trim() + "", "emp_name asc", DataViewRowState.CurrentRows).ToTable();
                    }
                }
            }
            else
            {
                string strNewEmpList = Session["EmpId"].ToString();
                if (!string.IsNullOrEmpty(sharedSalesPersons) && sharedSalesPersons != "@NOTFOUND@")
                {
                    strNewEmpList = sharedSalesPersons;
                    strNewEmpList += "," + Session["EmpId"].ToString();
                }

                //dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                dtEmp = objEmployee.GetEmployeeMasterAll(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "emp_id in(" + strNewEmpList + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (ddlLocation.SelectedIndex > 0)
                {
                    dtEmp = new DataView(dtEmp, "location_id='" + ddlLocation.SelectedValue + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                //dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString(), Session["EmpId"].ToString());

            }

            ddlUser.DataSource = dtEmp;
            ddlUser.DataTextField = "Emp_name";
            ddlUser.DataValueField = "user_id";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("--Select User--", "--Select User--"));
        }
        catch
        {

        }
    }

    public string getEmpList(string strBrandId, string strLocationId, string strdepartmentId)
    {
        string strEmpList = "0";

        DataTable dtEmp = objDa.return_DataTable(" SELECt STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Brand_Id=" + strBrandId + " and  location_id in (" + strLocationId + ") and Department_Id in (" + strdepartmentId.Substring(0, strdepartmentId.Length - 1) + ") and IsActive='True' FOR xml PATH ('')), 1, 1, '')");

        if (dtEmp.Rows[0][0] != null)
        {
            strEmpList = dtEmp.Rows[0][0].ToString();
        }

        if (strEmpList == "")
        {
            strEmpList = "0";
        }
        return strEmpList;

    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUser();
        //FillGrid(1);
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
            if (dtLoc.Rows.Count > 1 && LocIds != "")
            {
                ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        dtLoc = null;
    }

    protected void lnkAddNewContact_Click(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Please Enter Customer Name");
            txtCustomer.Focus();
            return;
        }
        DataTable dt = objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["ContactID"].ToString());
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(Session["ContactID"].ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
    }
    protected void btnAddCustomer_Click(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Please Enter Customer Name");
            txtCustomer.Focus();
            return;
        }

        DataTable dt = objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["ContactID"].ToString());
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(Session["ContactID"].ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDesignationMaster(string prefixText, int count, string contextKey)
    {

        DataTable dt = new DesignationMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDesignationDataPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Designation"].ToString() + "/" + dt.Rows[i]["Designation_Id"].ToString();
        }
        return str;
    }

    protected void chkShortProductName_CheckedChanged(object sender, EventArgs e)
    {
        //foreach (GridViewRow gvRow in GvDetail.Rows)
        //{
        //    Label lblDetailName = (Label)gvRow.FindControl("lblgvProductName");
        //    Label lblShortName = (Label)gvRow.FindControl("lblShortProductName1");
        //    if (((CheckBox)sender).Checked)
        //    {
        //        lblDetailName.Visible = true;
        //        lblShortName.Visible = false;
        //        ((CheckBox)sender).ToolTip = "Display short name";
        //    }
        //    else
        //    {
        //        lblDetailName.Visible = false;
        //        lblShortName.Visible = true;
        //        ((CheckBox)sender).ToolTip = "Display detail name";
        //    }
        //}
    }
    #region ArchivingModule
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/SQ", "Sales", "sales Quotation", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    #endregion
    public string getShortProductName(string ProductId)
    {
        string strProductName = "";
        strProductName = objProductM.GetProductNamebyProductId(ProductId.ToString());
        if (strProductName.Length > 16)
        {
            strProductName = strProductName.Substring(0, 15) + "...";
        }
        return strProductName;
    }
}