using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;



public partial class Purchase_CustomerHistory : System.Web.UI.Page
{
    SystemParameter ObjSysParam = null;
    DataAccessClass objDa = null;
    LocationMaster objLocation = null;
    Inv_ParameterMaster objInvParam = null;
    Common cmn = null;
    Inv_ProductMaster objProductM = null;
    Ems_ContactMaster objContact = null;
    PageControlCommon objPageCmn = null;
    public static string strLocationId = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        if(Request.QueryString["LocId"]==null)
        {
            strLocationId = Session["LocId"].ToString();
        }
        else
        {
            strLocationId = Request.QueryString["LocId"].ToString();
        }

        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            try
            {
                lblContactname.Text = objContact.GetContactTrueById(Request.QueryString["ContactId"].ToString().Trim()).Rows[0]["Name"].ToString();
            }
            catch
            {

            }
            GetHeaderRecord(Request.QueryString["Page"].ToString().Trim(),false);
          
        }
    }



    public void GetHeaderRecord(string pageType,bool IsProductFilter)
    {
        //for sales inquiry
        if (pageType == "SINQ")
        {
            imgObject.Src = "../Images/sales_inquiry.png";

            lblHeader.Text = Resources.Attendance.Sales_Inquiry;


            if (IsProductFilter)
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable("select Inv_SalesInquiryHeader.SInquiryID as Trans_Id,inv_salesinquiryheader.SInquiryNo as VoucherNo ,CONVERT(VARCHAR(11),inv_salesinquiryheader.IDate,106) as VoucherDate,inv_salesinquiryheader.Remark,(select CAST( cast(sum(Inv_SalesInqDetail.EstimatedUnitPrice*Inv_SalesInqDetail.Quantity) as numeric(18,3))as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code  from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=inv_salesinquiryheader.Currency_Id)   from Inv_SalesInqDetail  where Inv_SalesInqDetail.SInquiryID=Inv_SalesInquiryHeader.SInquiryID)  as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,case  when  (select top 1 Inv_SalesInvoiceDetail.Invoice_No from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo in (select  top 1  Inv_SalesQuotationHeader.SQuotation_Id from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID))) IS not null then 'Invoice Created'   when (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo in (select  top 1  Inv_SalesQuotationHeader.SQuotation_Id from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID))) IS not null then 'Delivery Voucher Created'     when  (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and inv_salesorderheader.SOfromTransNo in (select  top 1  Inv_SalesQuotationHeader.SQuotation_Id from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID)) IS not null then 'Order Created'  when (select  top 1  Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID) IS not null then 'Quotation Created'   when (select top 1 Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID) IS null then 'Open'  end as Status from inv_salesinquiryheader left join Set_UserMaster on inv_salesinquiryheader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id=" + strLocationId + " and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + Request.QueryString["ContactId"].ToString() + " and Inv_SalesInquiryHeader.SInquiryID in (select distinct SInquiryID from Inv_SalesInqDetail where Inv_SalesInqDetail.Product_Id in ("+hdnProductId.Value+"))  order by Inv_SalesInquiryHeader.SInquiryID desc");
    
            }
            else
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable("select Inv_SalesInquiryHeader.SInquiryID as Trans_Id,inv_salesinquiryheader.SInquiryNo as VoucherNo ,CONVERT(VARCHAR(11),inv_salesinquiryheader.IDate,106) as VoucherDate,inv_salesinquiryheader.Remark,(select CAST( cast(sum(Inv_SalesInqDetail.EstimatedUnitPrice*Inv_SalesInqDetail.Quantity) as numeric(18,3))as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code  from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=inv_salesinquiryheader.Currency_Id)   from Inv_SalesInqDetail  where Inv_SalesInqDetail.SInquiryID=Inv_SalesInquiryHeader.SInquiryID)  as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,case  when  (select top 1 Inv_SalesInvoiceDetail.Invoice_No from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo in (select  top 1  Inv_SalesQuotationHeader.SQuotation_Id from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID))) IS not null then 'Invoice Created'   when (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo in (select  top 1  Inv_SalesQuotationHeader.SQuotation_Id from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID))) IS not null then 'Delivery Voucher Created'     when  (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and inv_salesorderheader.SOfromTransNo in (select  top 1  Inv_SalesQuotationHeader.SQuotation_Id from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID)) IS not null then 'Order Created'  when (select  top 1  Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID) IS not null then 'Quotation Created'   when (select top 1 Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID) IS null then 'Open'  end as Status from inv_salesinquiryheader left join Set_UserMaster on inv_salesinquiryheader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id=" + strLocationId + " and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + Request.QueryString["ContactId"].ToString() + " order by Inv_SalesInquiryHeader.SInquiryID desc");
            }
            gvPurchaseOrder.DataBind();

        }

        //for sales quotation 

        if (pageType == "SQ")
        {
            imgObject.Src = "../Images/sales_quotation.png";

            lblHeader.Text = Resources.Attendance.Sales_Quotation;

            //here we chekc user permisison to view all user record or not

            if (IsViewAllUser("144", "57"))
            {
                if (IsProductFilter)
                {
                    gvPurchaseOrder.DataSource = objDa.return_DataTable("select inv_salesquotationheader.SQuotation_Id as Trans_Id,Inv_SalesQuotationHeader.SQuotation_No as VoucherNo,CONVERT(VARCHAR(11),Inv_SalesQuotationHeader.Quotation_Date,106) as VoucherDate,Inv_SalesInquiryHeader.SInquiryNo,CAST(cast((inv_salesquotationheader.Amount+inv_salesquotationheader.TaxValue-inv_salesquotationheader.DiscountValue) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesQuotationHeader.Currency_Id) as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,case  when  (select top 1 Inv_SalesInvoiceDetail.Invoice_No from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id)) IS not null then 'Invoice Created'    when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id  in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id)) IS not null then 'Delivery Voucher Created'               when (select top 1 Inv_SalesOrderHeader.SalesOrderNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' And Inv_SalesOrderHeader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id) IS not null then 'Order Created'        when (select top 1 Inv_SalesOrderHeader.SalesOrderNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' And Inv_SalesOrderHeader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id) IS null then 'Open'               end as Status  from inv_salesquotationheader inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id left join Set_UserMaster on Inv_SalesQuotationHeader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id=" + strLocationId + " and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + Request.QueryString["ContactId"].ToString() + " and inv_salesquotationheader.SQuotation_Id in (select distinct SQuotation_Id from Inv_SalesQuotationDetail where Inv_SalesQuotationDetail.Product_Id in ("+hdnProductId.Value+")) order by inv_salesquotationheader.SQuotation_Id desc");

                }
                else
                {
                    gvPurchaseOrder.DataSource = objDa.return_DataTable("select inv_salesquotationheader.SQuotation_Id as Trans_Id,Inv_SalesQuotationHeader.SQuotation_No as VoucherNo,CONVERT(VARCHAR(11),Inv_SalesQuotationHeader.Quotation_Date,106) as VoucherDate,Inv_SalesInquiryHeader.SInquiryNo,CAST(cast((inv_salesquotationheader.Amount+inv_salesquotationheader.TaxValue-inv_salesquotationheader.DiscountValue) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesQuotationHeader.Currency_Id) as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,case  when  (select top 1 Inv_SalesInvoiceDetail.Invoice_No from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id)) IS not null then 'Invoice Created'    when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id  in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id)) IS not null then 'Delivery Voucher Created'               when (select top 1 Inv_SalesOrderHeader.SalesOrderNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' And Inv_SalesOrderHeader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id) IS not null then 'Order Created'        when (select top 1 Inv_SalesOrderHeader.SalesOrderNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' And Inv_SalesOrderHeader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id) IS null then 'Open'               end as Status  from inv_salesquotationheader inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id left join Set_UserMaster on Inv_SalesQuotationHeader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id=" + strLocationId + " and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + Request.QueryString["ContactId"].ToString() + " order by inv_salesquotationheader.SQuotation_Id desc");

   
                }
            }
            else
            {
                if (IsProductFilter)
                {

                    gvPurchaseOrder.DataSource = objDa.return_DataTable("select inv_salesquotationheader.SQuotation_Id as Trans_Id,Inv_SalesQuotationHeader.SQuotation_No as VoucherNo,CONVERT(VARCHAR(11),Inv_SalesQuotationHeader.Quotation_Date,106) as VoucherDate,Inv_SalesInquiryHeader.SInquiryNo,CAST(cast((inv_salesquotationheader.Amount+inv_salesquotationheader.TaxValue-inv_salesquotationheader.DiscountValue) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesQuotationHeader.Currency_Id) as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,case  when  (select top 1 Inv_SalesInvoiceDetail.Invoice_No from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id)) IS not null then 'Invoice Created'  when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id  in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id)) IS not null then 'Delivery Voucher Created'              when (select top 1 Inv_SalesOrderHeader.SalesOrderNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' And Inv_SalesOrderHeader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id) IS not null then 'Order Created'        when (select top 1 Inv_SalesOrderHeader.SalesOrderNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' And Inv_SalesOrderHeader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id) IS null then 'Open'               end as Status  from inv_salesquotationheader inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id left join Set_UserMaster on Inv_SalesQuotationHeader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id=" + strLocationId + " and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + Request.QueryString["ContactId"].ToString() + " and Inv_SalesQuotationHeader.CreatedBy='" + Session["UserId"].ToString() + "' and and inv_salesquotationheader.SQuotation_Id in (select distinct SQuotation_Id from Inv_SalesQuotationDetail where Inv_SalesQuotationDetail.Product_Id in (" + hdnProductId.Value + "))  order by inv_salesquotationheader.SQuotation_Id desc");

                }
                else
                {
                    gvPurchaseOrder.DataSource = objDa.return_DataTable("select inv_salesquotationheader.SQuotation_Id as Trans_Id,Inv_SalesQuotationHeader.SQuotation_No as VoucherNo,CONVERT(VARCHAR(11),Inv_SalesQuotationHeader.Quotation_Date,106) as VoucherDate,Inv_SalesInquiryHeader.SInquiryNo,CAST(cast((inv_salesquotationheader.Amount+inv_salesquotationheader.TaxValue-inv_salesquotationheader.DiscountValue) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesQuotationHeader.Currency_Id) as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,case  when  (select top 1 Inv_SalesInvoiceDetail.Invoice_No from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id)) IS not null then 'Invoice Created'  when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id  in (select top 1 inv_salesorderheader.Trans_Id from inv_salesorderheader where Inv_SalesOrderHeader.SOfromTransType='Q' and    inv_salesorderheader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id)) IS not null then 'Delivery Voucher Created'              when (select top 1 Inv_SalesOrderHeader.SalesOrderNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' And Inv_SalesOrderHeader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id) IS not null then 'Order Created'        when (select top 1 Inv_SalesOrderHeader.SalesOrderNo from Inv_SalesOrderHeader where Inv_SalesOrderHeader.SOfromTransType='Q' And Inv_SalesOrderHeader.SOfromTransNo =Inv_SalesQuotationHeader.SQuotation_Id) IS null then 'Open'               end as Status  from inv_salesquotationheader inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id left join Set_UserMaster on Inv_SalesQuotationHeader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id=" + strLocationId + " and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + Request.QueryString["ContactId"].ToString() + " and Inv_SalesQuotationHeader.CreatedBy='" + Session["UserId"].ToString() + "'  order by inv_salesquotationheader.SQuotation_Id desc");


                }
            }
            gvPurchaseOrder.DataBind();

        }

        //for sales order

        if (pageType == "SO")
        {
            imgObject.Src = "../Images/sales_order.png";

            lblHeader.Text = Resources.Attendance.Sales_Order;


            //here we chekc user permisison to view all user record or not

            if (IsViewAllUser("144", "67"))
            {
                if (IsProductFilter)
                {
                    gvPurchaseOrder.DataSource = objDa.return_DataTable(" SELECT  SH.[Trans_Id] ,SH.[SalesOrderNo]  as VoucherNo  , convert(varchar(11), SH.[SalesOrderDate] ,106) as Voucherdate       ,                        case when SH.SOfromTransType='Q' then 'Sales Quotation'                           when SH.SOfromTransType='D' then 'Direct' end as Transfer_Type,                                      case when SH.SOfromTransNo<>' ' and SH.SOfromTransNo<>0 then (select Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SQuotation_Id=SH.SOfromTransNo)                           end as Transfer_No                         ,SH.[Remark]                      ,cast(cast(SH.[NetAmount] as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=SH.Currency_Id) as       [NetAmount]                         ,Set_EmployeeMaster.Emp_Name as CreatedBy ,case       when  (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS not null then 'Invoice Created'     when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id =SH.Trans_Id) IS not null then 'Delivery Voucher Created'        when (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS null then 'Open'               end as Status         FROM Inv_SalesOrderHeader as SH      left join Set_UserMaster on SH.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id  Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id=" + strLocationId + " and SH.IsActive='True' and sh.CustomerId=" + Request.QueryString["ContactId"].ToString() + " and SH.[Trans_Id] in (select distinct SalesOrderNo from Inv_SalesOrderDetail where Product_Id in ("+hdnProductId.Value+")) order by SH.Trans_Id desc");

                }
                else
                {

                    gvPurchaseOrder.DataSource = objDa.return_DataTable(" SELECT  SH.[Trans_Id] ,SH.[SalesOrderNo]  as VoucherNo  , convert(varchar(11), SH.[SalesOrderDate] ,106) as Voucherdate       ,                        case when SH.SOfromTransType='Q' then 'Sales Quotation'                           when SH.SOfromTransType='D' then 'Direct' end as Transfer_Type,                                      case when SH.SOfromTransNo<>' ' and SH.SOfromTransNo<>0 then (select Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SQuotation_Id=SH.SOfromTransNo)                           end as Transfer_No                         ,SH.[Remark]                      ,cast(cast(SH.[NetAmount] as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=SH.Currency_Id) as       [NetAmount]                         ,Set_EmployeeMaster.Emp_Name as CreatedBy ,case       when  (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS not null then 'Invoice Created'     when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id =SH.Trans_Id) IS not null then 'Delivery Voucher Created'        when (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS null then 'Open'               end as Status         FROM Inv_SalesOrderHeader as SH      left join Set_UserMaster on SH.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id  Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id=" + strLocationId + " and SH.IsActive='True' and sh.CustomerId=" + Request.QueryString["ContactId"].ToString() + " order by SH.Trans_Id desc");

                }
            }
            else
            {

                if (IsProductFilter)
                {
                    gvPurchaseOrder.DataSource = objDa.return_DataTable(" SELECT  SH.[Trans_Id] ,SH.[SalesOrderNo]  as VoucherNo  , convert(varchar(11), SH.[SalesOrderDate] ,106) as Voucherdate       ,                        case when SH.SOfromTransType='Q' then 'Sales Quotation'                           when SH.SOfromTransType='D' then 'Direct' end as Transfer_Type,                                      case when SH.SOfromTransNo<>' ' and SH.SOfromTransNo<>0 then (select Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SQuotation_Id=SH.SOfromTransNo)                           end as Transfer_No                         ,SH.[Remark]                      ,cast(cast(SH.[NetAmount] as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=SH.Currency_Id) as       [NetAmount]                          ,Set_EmployeeMaster.Emp_Name as CreatedBy   ,case       when  (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS not null then 'Invoice Created'     when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id =SH.Trans_Id) IS not null then 'Delivery Voucher Created'        when (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS null then 'Open'               end as Status                                                                                                                                   FROM Inv_SalesOrderHeader as SH      left join Set_UserMaster on SH.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id  Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id=" + strLocationId + " and SH.IsActive='True' and sh.CustomerId=" + Request.QueryString["ContactId"].ToString() + " and sh.CreatedBy='" + Session["UserId"].ToString() + "' and SH.[Trans_Id] in (select distinct SalesOrderNo from Inv_SalesOrderDetail where Product_Id in (" + hdnProductId.Value + ")) order by SH.Trans_Id desc");

                }
                else
                {

                    gvPurchaseOrder.DataSource = objDa.return_DataTable(" SELECT  SH.[Trans_Id] ,SH.[SalesOrderNo]  as VoucherNo  , convert(varchar(11), SH.[SalesOrderDate] ,106) as Voucherdate       ,                        case when SH.SOfromTransType='Q' then 'Sales Quotation'                           when SH.SOfromTransType='D' then 'Direct' end as Transfer_Type,                                      case when SH.SOfromTransNo<>' ' and SH.SOfromTransNo<>0 then (select Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SQuotation_Id=SH.SOfromTransNo)                           end as Transfer_No                         ,SH.[Remark]                      ,cast(cast(SH.[NetAmount] as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=SH.Currency_Id) as       [NetAmount]                          ,Set_EmployeeMaster.Emp_Name as CreatedBy   ,case       when  (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS not null then 'Invoice Created'     when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id =SH.Trans_Id) IS not null then 'Delivery Voucher Created'        when (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS null then 'Open'               end as Status                                                                                                                                   FROM Inv_SalesOrderHeader as SH      left join Set_UserMaster on SH.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id  Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id=" + strLocationId + " and SH.IsActive='True' and sh.CustomerId=" + Request.QueryString["ContactId"].ToString() + " and sh.CreatedBy='" + Session["UserId"].ToString() + "' order by SH.Trans_Id desc");

                }

            }
            gvPurchaseOrder.DataBind();


            
        }

        //for sales invoice 

        if (pageType == "SINV")
        {
            imgObject.Src = "../Images/sales_invoice.png";

            lblHeader.Text = Resources.Attendance.Sales_Invoice;

            if (IsProductFilter)
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable(" select Inv_SalesInvoiceHeader.Trans_Id,Inv_SalesInvoiceHeader.Invoice_No as VoucherNo,convert(varchar(11),Inv_SalesInvoiceHeader.Invoice_Date,106) as Voucherdate,(SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_salesOrderHeader.SalesOrderNo)                    FROM Inv_salesOrderHeader where Inv_salesOrderHeader.Trans_id in (select Distinct Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.Invoice_No=Inv_SalesInvoiceHeader.Trans_Id) FOR XML PATH('')),1,1,'') ) as   SalesOrder_No,Set_EmployeeMaster.Emp_Name as SalesPerson,cast(cast(Inv_SalesInvoiceHeader.GrandTotal as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesInvoiceHeader.Currency_Id) as InvoiceAmount     ,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesInvoiceHeader.CreatedBy)=0 then Inv_SalesInvoiceHeader.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesInvoiceHeader.CreatedBy)) end as CreatedBy          from Inv_SalesInvoiceHeader inner join Set_EmployeeMaster on Set_EmployeeMaster.emp_id=Inv_SalesInvoiceHeader.SalesPerson_Id inner join Ems_ContactMaster on Inv_SalesInvoiceHeader.Supplier_Id=Ems_ContactMaster.Trans_Id   where Inv_SalesInvoiceHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInvoiceHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInvoiceHeader.Location_Id=" + strLocationId + " and Inv_SalesInvoiceHeader.IsActive='True' and Inv_SalesInvoiceHeader.Supplier_Id=" + Request.QueryString["ContactId"].ToString() + " and Inv_SalesInvoiceHeader.Trans_Id in (select distinct Invoice_No from Inv_SalesInvoiceDetail where Product_Id in ("+hdnProductId.Value+"))  order by Inv_SalesInvoiceHeader.Trans_Id desc");

            }
            else
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable(" select Inv_SalesInvoiceHeader.Trans_Id,Inv_SalesInvoiceHeader.Invoice_No as VoucherNo,convert(varchar(11),Inv_SalesInvoiceHeader.Invoice_Date,106) as Voucherdate,(SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_salesOrderHeader.SalesOrderNo)                    FROM Inv_salesOrderHeader where Inv_salesOrderHeader.Trans_id in (select Distinct Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.Invoice_No=Inv_SalesInvoiceHeader.Trans_Id) FOR XML PATH('')),1,1,'') ) as   SalesOrder_No,Set_EmployeeMaster.Emp_Name as SalesPerson,cast(cast(Inv_SalesInvoiceHeader.GrandTotal as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesInvoiceHeader.Currency_Id) as InvoiceAmount     ,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesInvoiceHeader.CreatedBy)=0 then Inv_SalesInvoiceHeader.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesInvoiceHeader.CreatedBy)) end as CreatedBy          from Inv_SalesInvoiceHeader inner join Set_EmployeeMaster on Set_EmployeeMaster.emp_id=Inv_SalesInvoiceHeader.SalesPerson_Id inner join Ems_ContactMaster on Inv_SalesInvoiceHeader.Supplier_Id=Ems_ContactMaster.Trans_Id   where Inv_SalesInvoiceHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInvoiceHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInvoiceHeader.Location_Id=" + strLocationId + " and Inv_SalesInvoiceHeader.IsActive='True' and Inv_SalesInvoiceHeader.Supplier_Id=" + Request.QueryString["ContactId"].ToString() + "  order by Inv_SalesInvoiceHeader.Trans_Id desc");


            }
            gvPurchaseOrder.DataBind();

        }


        //for purchase quotation 


        if (pageType == "PQ")
        {
            imgObject.Src = "../Images/purchase_quotation.png";

            lblHeader.Text = Resources.Attendance.Purchase_Quotation;

            if (IsProductFilter)
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable(" select Inv_PurchaseQuoteHeader.Trans_Id,Inv_PurchaseQuoteHeader.RPQ_No as VoucherNo,CONVERT(VARCHAR(11),Inv_PurchaseQuoteHeader.RPQ_Date,106) as VoucherDate, (select Inv_PurchaseInquiryHeader.PI_No from Inv_PurchaseInquiryHeader where Inv_PurchaseInquiryHeader.Trans_Id=Inv_PurchaseQuoteHeader.PI_No) as Inquiry_No ,cast(cast((select sum(Inv_PurchaseQuoteDetail.Amount) from Inv_PurchaseQuoteDetail where Inv_PurchaseQuoteDetail.Supplier_Id=" + Request.QueryString["ContactId"].ToString() + " and Inv_PurchaseQuoteDetail.RPQ_No=Inv_PurchaseQuoteHeader.Trans_Id and Inv_PurchaseQuoteDetail.Product_Id in (" + hdnProductId.Value + ")) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_PurchaseQuoteHeader.Field1) as QuotationAmount, case when (select distinct Inv_PurchaseOrderHeader.TransID from Inv_PurchaseOrderHeader where Inv_PurchaseOrderHeader.ReferenceVoucherType='PQ'  and   Inv_PurchaseOrderHeader.ReferenceID=Inv_PurchaseQuoteHeader.Trans_Id) Is null then 'Open'                                           else 'Close' end as QuotationStatus      ,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_PurchaseQuoteHeader.CreatedBy)=0 then Inv_PurchaseQuoteHeader.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_PurchaseQuoteHeader.CreatedBy)) end as CreatedBy    from Inv_PurchaseQuoteHeader where Inv_PurchaseQuoteHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_PurchaseQuoteHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_PurchaseQuoteHeader.Location_Id=" + strLocationId + "  and Inv_PurchaseQuoteHeader.IsActive='True' and Inv_PurchaseQuoteHeader.Trans_Id in (select Inv_PurchaseQuoteDetail.RPQ_No from Inv_PurchaseQuoteDetail where Inv_PurchaseQuoteDetail.Supplier_Id=" + Request.QueryString["ContactId"].ToString() + " and Inv_PurchaseQuoteDetail.Product_Id in (" + hdnProductId.Value + "))  order by Inv_PurchaseQuoteHeader.Trans_Id desc");

            }
            else
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable(" select Inv_PurchaseQuoteHeader.Trans_Id,Inv_PurchaseQuoteHeader.RPQ_No as VoucherNo,CONVERT(VARCHAR(11),Inv_PurchaseQuoteHeader.RPQ_Date,106) as VoucherDate, (select Inv_PurchaseInquiryHeader.PI_No from Inv_PurchaseInquiryHeader where Inv_PurchaseInquiryHeader.Trans_Id=Inv_PurchaseQuoteHeader.PI_No) as Inquiry_No ,cast(cast((select sum(Inv_PurchaseQuoteDetail.Amount) from Inv_PurchaseQuoteDetail where Inv_PurchaseQuoteDetail.Supplier_Id=" + Request.QueryString["ContactId"].ToString() + " and Inv_PurchaseQuoteDetail.RPQ_No=Inv_PurchaseQuoteHeader.Trans_Id ) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_PurchaseQuoteHeader.Field1) as QuotationAmount, case when (select distinct Inv_PurchaseOrderHeader.TransID from Inv_PurchaseOrderHeader where Inv_PurchaseOrderHeader.ReferenceVoucherType='PQ'  and   Inv_PurchaseOrderHeader.ReferenceID=Inv_PurchaseQuoteHeader.Trans_Id) Is null then 'Open'                                           else 'Close' end as QuotationStatus      ,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_PurchaseQuoteHeader.CreatedBy)=0 then Inv_PurchaseQuoteHeader.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_PurchaseQuoteHeader.CreatedBy)) end as CreatedBy    from Inv_PurchaseQuoteHeader where Inv_PurchaseQuoteHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_PurchaseQuoteHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_PurchaseQuoteHeader.Location_Id=" + strLocationId + "  and Inv_PurchaseQuoteHeader.IsActive='True' and Inv_PurchaseQuoteHeader.Trans_Id in (select Inv_PurchaseQuoteDetail.RPQ_No from Inv_PurchaseQuoteDetail where Inv_PurchaseQuoteDetail.Supplier_Id=" + Request.QueryString["ContactId"].ToString() + ")  order by Inv_PurchaseQuoteHeader.Trans_Id desc");


            }
            gvPurchaseOrder.DataBind();

        }
        //for purchase order page 

        if (pageType == "PO")
        {
            imgObject.Src = "../Images/purchase_order.png";

            lblHeader.Text = Resources.Attendance.Purchase_Order;

            if (IsProductFilter)
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable(" select po.TransID as Trans_Id,PO.PoNO as VoucherNo,CONVERT(VARCHAR(11),PO.PODate,106) as VoucherDate,CONVERT(VARCHAR(11),PO.DeliveryDate,106) as DeliveryDate ,case          when PO.OrderType = 'D' then 'Direct'          when po.OrderType='R' And po.SalesOrderID=0 then 'By Quotation'          else 'By Salesorder'           end as Order_Type,           cast(cast(((select sum(Inv_PurchaseOrderDetail.Amount) from Inv_PurchaseOrderDetail where Inv_PurchaseOrderDetail.PoNO=PO.TransID)+CAST(PO.Field3 as numeric(18,6))) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Symbol from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=PO.CurrencyID) as PoAmount,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=PO.CreatedBy)=0 then PO.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=PO.CreatedBy)) end as CreatedBy  from Inv_PurchaseOrderHeader as PO  where PO.Company_Id=" + Session["CompId"].ToString() + " and PO.Brand_Id=" + Session["BrandId"].ToString() + " and PO.Location_ID=" + strLocationId + " and  PO.IsActive='True'    and po.SupplierId=" + Request.QueryString["ContactId"].ToString() + "   and po.TransID in (select Inv_PurchaseOrderDetail.PoNO from Inv_PurchaseOrderDetail where Inv_PurchaseOrderDetail.Product_ID in (" + hdnProductId.Value + ")) order by po.TransID desc");

            }
            else
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable("select po.TransID as Trans_Id,PO.PoNO as VoucherNo,CONVERT(VARCHAR(11),PO.PODate,106) as VoucherDate,CONVERT(VARCHAR(11),PO.DeliveryDate,106) as DeliveryDate ,case          when PO.OrderType = 'D' then 'Direct'          when po.OrderType='R' And po.SalesOrderID=0 then 'By Quotation'          else 'By Salesorder'           end as Order_Type,           cast(cast(((select sum(Inv_PurchaseOrderDetail.Amount) from Inv_PurchaseOrderDetail where Inv_PurchaseOrderDetail.PoNO=PO.TransID)+CAST(PO.Field3 as numeric(18,6))) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Symbol from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=PO.CurrencyID) as PoAmount,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=PO.CreatedBy)=0 then PO.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=PO.CreatedBy)) end as CreatedBy from Inv_PurchaseOrderHeader as PO  where PO.Company_Id=" + Session["CompId"].ToString() + " and PO.Brand_Id=" + Session["BrandId"].ToString() + " and PO.Location_ID=" + strLocationId + " and  PO.IsActive='True'    and po.SupplierId=" + Request.QueryString["ContactId"].ToString() + "    order by po.TransID desc");


            }
            gvPurchaseOrder.DataBind();

        }
        //for purchase invoice page 

        if (pageType == "PINV")
        {
            imgObject.Src = "../Images/purchase_invoice.png";

            lblHeader.Text = Resources.Attendance.Purchase_Invoice;

            if (IsProductFilter)
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable(" select Ph.TransID as Trans_Id,ph.InvoiceNo as Voucher_No,CONVERT(VARCHAR(11),ph.InvoiceDate,106) as VoucherDate, cast(CAST(ph.GrandTotal as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Symbol from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=ph.CurrencyID) as Amount,ph.CostingRate ,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Ph.CreatedBy)=0 then Ph.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Ph.CreatedBy)) end as CreatedBy from Inv_PurchaseInvoiceHeader as Ph where ph.Company_Id=" + Session["CompId"].ToString() + " and ph.Brand_Id=" + Session["BrandId"].ToString() + " and ph.Location_ID=" + strLocationId + " and ph.IsActive='True' and ph.SupplierId=" + Request.QueryString["ContactId"].ToString() + " and ph.TransID in (select distinct Inv_PurchaseInvoiceDetail.InvoiceNo from Inv_PurchaseInvoiceDetail where Inv_PurchaseInvoiceDetail.ProductId in ("+hdnProductId.Value+")) order by ph.TransID desc");

            }
            else
            {
                gvPurchaseOrder.DataSource = objDa.return_DataTable("select Ph.TransID as Trans_Id,ph.InvoiceNo as Voucher_No,CONVERT(VARCHAR(11),ph.InvoiceDate,106) as VoucherDate, cast(CAST(ph.GrandTotal as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Symbol from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=ph.CurrencyID) as Amount,ph.CostingRate ,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Ph.CreatedBy)=0 then Ph.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Ph.CreatedBy)) end as CreatedBy from Inv_PurchaseInvoiceHeader as Ph where ph.Company_Id=" + Session["CompId"].ToString() + " and ph.Brand_Id=" + Session["BrandId"].ToString() + " and ph.Location_ID=" + strLocationId + " and ph.IsActive='True' and ph.SupplierId=" + Request.QueryString["ContactId"].ToString() + "  order by ph.TransID desc");


            }
            gvPurchaseOrder.DataBind();

        }




       
    }


    

   

    public string SetDecimal(string amount)
    {
        if (amount == "")
        {
            amount = "0";
        }
        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), strLocationId).Rows[0]["Field1"].ToString(), amount);

    }

    public bool IsViewAllUser(string strModuleId,string ObjectId)
    {

        bool IsAllow = false;

        if (Session["EmpId"].ToString().Trim() == "0")
        {

            IsAllow = true;
        }
        else
        {
            if (new DataView(cmn.GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), strModuleId, ObjectId, HttpContext.Current.Session["CompId"].ToString()), "Op_Id=14", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                IsAllow = true;
            }
        }

        return IsAllow;
    }

    protected void gvPurchaseOrder_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string TransId = gvPurchaseOrder.DataKeys[e.Row.RowIndex].Value.ToString();
            GridView gvDetail = (GridView)e.Row.FindControl("gvDetail");

            //for inquiry
            if (Request.QueryString["Page"].ToString() == "SINQ")
            {


                if (txtProductId.Text != "" || txtSearchProductName.Text != "")
                {
                    gvDetail.DataSource = objDa.return_DataTable("select Inv_SalesInqDetail.Serial_No, Inv_ProductMaster.ProductCode,Inv_SalesInqDetail.SuggestedProductName,Inv_UnitMaster.Unit_Name,cast(Inv_SalesInqDetail.EstimatedUnitPrice as numeric(18,3)) as EstimatedUnitPrice,cast(Inv_SalesInqDetail.Quantity as numeric(18,3)) as Quantity,cast((Inv_SalesInqDetail.EstimatedUnitPrice*Inv_SalesInqDetail.Quantity) as numeric(18,3)) as LineTotal from Inv_SalesInqDetail  left join Inv_ProductMaster on Inv_SalesInqDetail.Product_Id=Inv_ProductMaster.ProductId left join Inv_UnitMaster on Inv_SalesInqDetail.UnitId=Inv_UnitMaster.Unit_Id where Inv_SalesInqDetail.SInquiryID=" + TransId + " and Inv_SalesInqDetail.Product_Id=" + hdnProductId.Value + " order by Inv_SalesInqDetail.Trans_Id");

                }
                else
                {
                    gvDetail.DataSource = objDa.return_DataTable("select Inv_SalesInqDetail.Serial_No, Inv_ProductMaster.ProductCode,Inv_SalesInqDetail.SuggestedProductName,Inv_UnitMaster.Unit_Name,cast(Inv_SalesInqDetail.EstimatedUnitPrice as numeric(18,3)) as EstimatedUnitPrice,cast(Inv_SalesInqDetail.Quantity as numeric(18,3)) as Quantity,cast((Inv_SalesInqDetail.EstimatedUnitPrice*Inv_SalesInqDetail.Quantity) as numeric(18,3)) as LineTotal from Inv_SalesInqDetail  left join Inv_ProductMaster on Inv_SalesInqDetail.Product_Id=Inv_ProductMaster.ProductId left join Inv_UnitMaster on Inv_SalesInqDetail.UnitId=Inv_UnitMaster.Unit_Id where Inv_SalesInqDetail.SInquiryID=" + TransId + " order by Inv_SalesInqDetail.Trans_Id");

                }
                gvDetail.DataBind();
            }

            //for quotation 
         

            if (Request.QueryString["Page"].ToString() == "SQ")
            {


                string strsql = string.Empty;

                strsql = "select Inv_SalesQuotationDetail.Serial_No,Inv_ProductMaster.ProductCode,case when Inv_SalesQuotationDetail.Product_Id=0 then Inv_SalesQuotationDetail.field3 else Inv_ProductMaster.EProductName end as ProductName,Inv_UnitMaster.Unit_Name as UnitName,cast(Inv_SalesQuotationDetail.UnitPrice as numeric(18,3)) as UnitPrice,cast(Inv_SalesQuotationDetail.Quantity as numeric(18,3)) as Quantity,cast((Inv_SalesQuotationDetail.UnitPrice*Inv_SalesQuotationDetail.Quantity) as numeric(18,3)) as QuantityPrice";

                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(Inv_SalesQuotationDetail.DiscountValue as numeric(18,3)) as DiscountValue";
                }
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(Inv_SalesQuotationDetail.TaxValue as numeric(18,3)) as TaxValue ";
                }
                if (txtProductId.Text != "" || txtSearchProductName.Text != "")
                {

                    strsql = strsql + " , cast( ((Inv_SalesQuotationDetail.UnitPrice+Inv_SalesQuotationDetail.TaxValue-Inv_SalesQuotationDetail.DiscountValue)*Inv_SalesQuotationDetail.Quantity) as numeric(18,3)) as LineTotal from Inv_SalesQuotationDetail left join Inv_ProductMaster on Inv_SalesQuotationDetail.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on Inv_SalesQuotationDetail.Field1=Inv_UnitMaster.Unit_Id where Inv_SalesQuotationDetail.SQuotation_Id=" + TransId + " and Inv_SalesQuotationDetail.Product_Id=" + hdnProductId.Value + "";

                }
                else
                {
                    strsql = strsql + " , cast( ((Inv_SalesQuotationDetail.UnitPrice+Inv_SalesQuotationDetail.TaxValue-Inv_SalesQuotationDetail.DiscountValue)*Inv_SalesQuotationDetail.Quantity) as numeric(18,3)) as LineTotal from Inv_SalesQuotationDetail left join Inv_ProductMaster on Inv_SalesQuotationDetail.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on Inv_SalesQuotationDetail.Field1=Inv_UnitMaster.Unit_Id where Inv_SalesQuotationDetail.SQuotation_Id=" + TransId + "";


                }
                    
                    
                    gvDetail.DataSource = objDa.return_DataTable(strsql);

                gvDetail.DataBind();
            }

            //for order

            if (Request.QueryString["Page"].ToString() == "SO")
            {


                string strsql = string.Empty;

                strsql = "select SD.Serial_No,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName as ProductName,Inv_UnitMaster.Unit_Name as UnitName,cast(SD.UnitPrice as numeric(18,3)) as UnitPrice,cast(SD.Quantity as numeric(18,3)) as Quantity,cast((SD.UnitPrice*SD.Quantity) as numeric(18,3)) as QuantityPrice";

                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(SD.DiscountV as numeric(18,3)) as DiscountValue";
                }
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(SD.TaxV as numeric(18,3)) as TaxValue ";
                }

                if (txtProductId.Text != "" || txtSearchProductName.Text != "")
                {
                    strsql = strsql + " , cast( ((SD.UnitPrice+SD.TaxV-SD.DiscountV)*SD.Quantity) as numeric(18,3)) as LineTotal from Inv_SalesOrderDetail as SD inner join Inv_ProductMaster on sd.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on sd.UnitId=Inv_UnitMaster.Unit_Id where sd.SalesOrderNo=" + TransId + " and sd.Product_Id=" + hdnProductId.Value + "";
                }
                else
                {
                    strsql = strsql + " , cast( ((SD.UnitPrice+SD.TaxV-SD.DiscountV)*SD.Quantity) as numeric(18,3)) as LineTotal from Inv_SalesOrderDetail as SD inner join Inv_ProductMaster on sd.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on sd.UnitId=Inv_UnitMaster.Unit_Id where sd.SalesOrderNo=" + TransId + "";
    
                }
                gvDetail.DataSource = objDa.return_DataTable(strsql);
                gvDetail.DataBind();
            }

            //for invoice

            if (Request.QueryString["Page"].ToString() == "SINV")
            {


                string strsql = string.Empty;

                strsql = "select SD.Serial_No,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName as ProductName,Inv_UnitMaster.Unit_Name ,cast(SD.UnitPrice as numeric(18,3)) as UnitPrice,cast(SD.Quantity as numeric(18,3)) as Quantity, cast((SD.UnitPrice*SD.Quantity) as numeric(18,3)) as QuantityPrice";

                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(SD.DiscountV as numeric(18,3)) as DiscountValue";
                }
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(SD.TaxV as numeric(18,3)) as TaxValue ";
                }

                if (txtProductId.Text != "" || txtSearchProductName.Text != "")
                {
                    strsql = strsql + " , cast( ((SD.UnitPrice+SD.TaxV-SD.DiscountV)*SD.Quantity) as numeric(18,3)) as LineTotal from Inv_SalesInvoiceDetail as SD inner join Inv_ProductMaster on SD.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster  on SD.Unit_Id=Inv_UnitMaster.Unit_Id where sd.Invoice_No=" + TransId + " and SD.Product_Id=" + hdnProductId.Value + "";

                }
                else
                {

                    strsql = strsql + " , cast( ((SD.UnitPrice+SD.TaxV-SD.DiscountV)*SD.Quantity) as numeric(18,3)) as LineTotal from Inv_SalesInvoiceDetail as SD inner join Inv_ProductMaster on SD.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster  on SD.Unit_Id=Inv_UnitMaster.Unit_Id where sd.Invoice_No=" + TransId + "";

                }
                    gvDetail.DataSource = objDa.return_DataTable(strsql);
                gvDetail.DataBind();
            }


            //for purchase quotation detail


  


            if (Request.QueryString["Page"].ToString() == "PQ")
            {


                string strsql = string.Empty;

                strsql = "select Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName as ProductName,Inv_UnitMaster.Unit_Name,cast(Inv_PurchaseQuoteDetail.UnitPrice as numeric(18,3)) as UnitPrice,cast(Inv_PurchaseQuoteDetail.ReqQty as numeric(18,3)) as Quantity, cast((Inv_PurchaseQuoteDetail.UnitPrice*Inv_PurchaseQuoteDetail.ReqQty) as numeric(18,3)) as QuantityPrice";

                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(Inv_PurchaseQuoteDetail.DiscountValue as numeric(18,3)) as DiscountValue";
                }
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(Inv_PurchaseQuoteDetail.TaxValue as numeric(18,3)) as TaxValue ";
                }


                if (txtProductId.Text != "" || txtSearchProductName.Text != "")
                {
                    strsql = strsql + " , cast( ((Inv_PurchaseQuoteDetail.UnitPrice+Inv_PurchaseQuoteDetail.TaxValue-Inv_PurchaseQuoteDetail.DiscountValue)*Inv_PurchaseQuoteDetail.ReqQty) as numeric(18,3)) as LineTotal from Inv_PurchaseQuoteDetail  left  join Inv_ProductMaster on Inv_PurchaseQuoteDetail.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on Inv_PurchaseQuoteDetail.UnitId=Inv_UnitMaster.Unit_Id where Inv_PurchaseQuoteDetail.RPQ_No=" + TransId + " and Inv_PurchaseQuoteDetail.Supplier_Id=" + Request.QueryString["ContactId"].ToString() + "  and  Inv_PurchaseQuoteDetail.Product_Id in (" + hdnProductId.Value + ")";
    
                }
                else
                {

                    strsql = strsql + " , cast( ((Inv_PurchaseQuoteDetail.UnitPrice+Inv_PurchaseQuoteDetail.TaxValue-Inv_PurchaseQuoteDetail.DiscountValue)*Inv_PurchaseQuoteDetail.ReqQty) as numeric(18,3)) as LineTotal from Inv_PurchaseQuoteDetail  left  join Inv_ProductMaster on Inv_PurchaseQuoteDetail.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on Inv_PurchaseQuoteDetail.UnitId=Inv_UnitMaster.Unit_Id where Inv_PurchaseQuoteDetail.RPQ_No=" + TransId + " and Inv_PurchaseQuoteDetail.Supplier_Id=" + Request.QueryString["ContactId"].ToString() + "";
    
                }
                gvDetail.DataSource = objDa.return_DataTable(strsql);
                gvDetail.DataBind();
            }

            //for purchase order detail
            if (Request.QueryString["Page"].ToString() == "PO")
            {


                string strsql = string.Empty;

                strsql = "select Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName  as ProductName,Inv_UnitMaster.Unit_Name,cast(Inv_PurchaseOrderDetail.UnitCost as numeric(18,3)) as UnitPrice,cast(Inv_PurchaseOrderDetail.OrderQty as numeric(18,3)) as Quantity,cast((Inv_PurchaseOrderDetail.UnitCost*Inv_PurchaseOrderDetail.OrderQty) as numeric(18,3)) as QuantityPrice";

                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " ,  cast(Inv_PurchaseOrderDetail.DiscountValue as numeric(18,3)) as DiscountValue";
                }
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(Inv_PurchaseOrderDetail.TaxValue as numeric(18,3)) as TaxValue ";
                }


                if (txtProductId.Text != "" || txtSearchProductName.Text != "")
                {
                    strsql = strsql + " , cast( ((Inv_PurchaseOrderDetail.UnitCost+Inv_PurchaseOrderDetail.TaxValue-Inv_PurchaseOrderDetail.DiscountValue)*Inv_PurchaseOrderDetail.OrderQty) as numeric(18,3)) as LineTotal from Inv_PurchaseOrderDetail inner join Inv_ProductMaster on Inv_PurchaseOrderDetail.Product_ID=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on Inv_PurchaseOrderDetail.UnitID=Inv_UnitMaster.Unit_Id   where Inv_PurchaseOrderDetail.PoNO="+TransId+" and Inv_PurchaseOrderDetail.Product_ID in ("+hdnProductId.Value+")";

                }
                else
                {

                    strsql = strsql + " ,  cast( ((Inv_PurchaseOrderDetail.UnitCost+Inv_PurchaseOrderDetail.TaxValue-Inv_PurchaseOrderDetail.DiscountValue)*Inv_PurchaseOrderDetail.OrderQty) as numeric(18,3)) as LineTotal from Inv_PurchaseOrderDetail inner join Inv_ProductMaster on Inv_PurchaseOrderDetail.Product_ID=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on Inv_PurchaseOrderDetail.UnitID=Inv_UnitMaster.Unit_Id   where Inv_PurchaseOrderDetail.PoNO="+TransId+"";

                }
                gvDetail.DataSource = objDa.return_DataTable(strsql);
                gvDetail.DataBind();
            }

            //for purchase invoice detail 

            if (Request.QueryString["Page"].ToString() == "PINV")
            {


                string strsql = string.Empty;

                strsql = "select Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName,Inv_UnitMaster.Unit_Name,pd.UnitCost,pd.InvoiceQty,CAST((pd.UnitCost*pd.InvoiceQty) as numeric(18,3)) as QuantityPrice";

                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " ,  cast(pd.DiscountV as numeric(18,3)) as DiscountValue";
                }
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
                {
                    strsql = strsql + " , cast(pd.TaxV as numeric(18,3)) as TaxValue";
                }


                if (txtProductId.Text != "" || txtSearchProductName.Text != "")
                {
                    strsql = strsql + " , cast(((pd.UnitCost-pd.DiscountV+pd.TaxV)*pd.InvoiceQty) as numeric(18,3)) as LineTotal from Inv_PurchaseInvoiceDetail as Pd inner join  Inv_ProductMaster on pd.ProductId=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on pd.UnitId=Inv_UnitMaster.Unit_Id where pd.InvoiceNo="+TransId+" and pd.ProductId="+hdnProductId.Value+"";

                }
                else
                {

                    strsql = strsql + " ,  cast(((pd.UnitCost-pd.DiscountV+pd.TaxV)*pd.InvoiceQty) as numeric(18,3)) as LineTotal from Inv_PurchaseInvoiceDetail as Pd inner join  Inv_ProductMaster on pd.ProductId=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on pd.UnitId=Inv_UnitMaster.Unit_Id where pd.InvoiceNo="+TransId+"";

                }
                gvDetail.DataSource = objDa.return_DataTable(strsql);
                gvDetail.DataBind();
            }



           
        }
    }




    #region ProductFilter
    protected void ddlProductSerach_SelectedIndexChanged(object sender, EventArgs e)
    {
        
            txtSearchProductName.Text = "";
            txtProductId.Text = "";
            if (ddlProductSerach.SelectedIndex == 0)
            {
                txtProductId.Visible = true;
                txtSearchProductName.Visible = false;
            }
            else
            {
                txtProductId.Visible = false;
                txtSearchProductName.Visible = true;
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
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }


        return txt;
    }



    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductId.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, txtProductId.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {

                    hdnProductId.Value = dt.Rows[0]["ProductId"].ToString();
                }
                else
                {
                    DisplayMessage("Product Not Found");
                    txtProductId.Text = "";
                    txtProductId.Focus();
                    return;
                }
            }
            catch
            {
            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
    }

    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtSearchProductName.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, txtSearchProductName.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {

                    hdnProductId.Value = dt.Rows[0]["ProductId"].ToString();
                }
                else
                {
                    DisplayMessage("Product Not Found");
                    txtProductId.Text = "";
                    txtProductId.Focus();
                    return;
                }
            }
            catch
            {
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
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




    protected void imgbtnsearch_Click(object sender, ImageClickEventArgs e)
    {

        if (txtProductId.Text.Trim() == "" && txtSearchProductName.Text.Trim() == "")
        {
            DisplayMessage("Enter Product Id or Name");
            txtProductId.Focus();
            return;
        }

        GetHeaderRecord(Request.QueryString["Page"].ToString().Trim(),true);

    }

    protected void ImgbtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        ddlProductSerach.SelectedIndex = 0;
        ddlProductSerach_SelectedIndexChanged(null, null);
        txtProductId.Focus();
        GetHeaderRecord(Request.QueryString["Page"].ToString().Trim(), false);
    }

    #endregion

}