using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Utility_ContactReplacement : System.Web.UI.Page
{
    DataAccessClass objda = null;
    Ems_ContactMaster objContact = null;
    Ac_SubChartOfAccount objAcSubChartOfAccount = null;
    Common objCommon = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objAcSubChartOfAccount = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
        objCommon = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "331", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            fillDdlCurrecy();
        }
        txtContactFrom.Focus();


    }

    protected void btnReplace_Click(object sender, EventArgs e)
    {

        string strFromContactId = string.Empty;
        string strToContactId = string.Empty;
        int strFromAcId = 0;
        int strToAcId = 0;
        if (txtContactFrom.Text == "")
        {
            DisplayMessage("Enter Contact From");
            txtContactFrom.Focus();
            return;
        }
        else
        {
            if (GetCustomerId(txtContactFrom) == "")
            {
                DisplayMessage("Enter Valid contact");
                txtContactFrom.Focus();
                return;
            }
            else
            {
                strFromContactId = GetCustomerId(txtContactFrom).ToString();
            }
        }

        if (txtContactto.Text == "")
        {
            DisplayMessage("Enter Contact To");
            txtContactto.Focus();
            return;
        }
        else
        {
            if (GetCustomerId(txtContactto) == "")
            {
                DisplayMessage("Enter Valid contact");
                txtContactto.Focus();
                return;
            }
            else
            {
                strToContactId = GetCustomerId(txtContactto).ToString();
            }
        }

        if (ddlCurrency.SelectedValue == "0")
        {

        }
        else
        {
            Ac_AccountMaster objAcMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
            strFromAcId = objAcMaster.GetCustomerAccountByCurrency(strFromContactId, ddlCurrency.SelectedValue);
            strToAcId = objAcMaster.GetCustomerAccountByCurrency(strToContactId, ddlCurrency.SelectedValue);
        }

        if (strFromAcId != strToAcId)
        {
            if (strFromAcId > 0 && strToAcId == 0)
            {
                DisplayMessage("you can not proceed this with it because account mismatch are there");
                return;
            }
        }

        string strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());



        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());



        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {


            string message = string.Empty;
            int counter = 0;

            int b = 0;

            //for update in sales inquiry header
            string strsql = "update Inv_SalesInquiryHeader set Customer_Id=" + strToContactId + " where Customer_Id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += " Sales Inquiry(Customer)-" + b.ToString() + " , ";
            }

            //for update in sales inquiry header
            strsql = "update Inv_SalesInquiryHeader set Field2=" + strToContactId + " where Field2=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += " Sales Inquiry(Contact)-" + b.ToString() + " , ";
            }

            //for update in sales Quotation header
            strsql = "update Inv_SalesQuotationHeader set Field1=" + strToContactId + " where Field1=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += " Sales Quotation(Contact)-" + b.ToString() + " , ";
            }

            //for update in sales Quotation header
            strsql = "update Inv_SalesQuotationHeader set Field2=" + strToContactId + " where Field2=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += " Sales Quotation(Ship Customer)-" + b.ToString() + " , ";
            }

            //for update in sales order header
            strsql = "update Inv_SalesOrderHeader set Field2=" + strToContactId + " where Field2=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += " Sales order(Ship Customer)-" + b.ToString() + " , ";
            }

            //for update in sales order header
            strsql = "update Inv_SalesOrderHeader set CustomerId=" + strToContactId + " where CustomerId=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += " Sales order(Customer)-" + b.ToString() + " , ";
            }

            //for update in sales invoice header
            strsql = "update Inv_SalesInvoiceHeader set Field2=" + strToContactId + " where Field2=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += " Sales Invoice(Ship Customer)-" + b.ToString() + " , ";
            }

            //for update in sales invoice header
            strsql = "update Inv_SalesInvoiceHeader set Supplier_Id=" + strToContactId + " where Supplier_Id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += " Sales Invoice(Customer)-" + b.ToString() + " , ";
            }

            //for update in sales delivery voucher header
            strsql = "update Inv_SalesDeliveryVoucher_Header set Customer_Id=" + strToContactId + " where Customer_Id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += " Delivery Voucher(Customer)-" + b.ToString() + " , ";
            }

            //for update in sales commission detail
            strsql = "update Inv_SalesCommission_Detail set Customer_Id=" + strToContactId + " where Customer_Id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Sales Commission(Customer)-" + b.ToString() + " , ";
            }

            //for update in purchase inquiry supplier 
            strsql = "update Inv_PurchaseInquiry_Supplier set Supplier_Id=" + strToContactId + " where Supplier_Id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Purchase Inquiry(Supplier)-" + b.ToString() + " , ";
            }

            //for update in purchase quotation detail 
            strsql = "update Inv_PurchaseQuoteDetail set Supplier_Id=" + strToContactId + " where Supplier_Id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Purchase Quotation(Supplier)-" + b.ToString() + " , ";
            }

            //for update in purchase order header 
            strsql = "update Inv_PurchaseOrderHeader set SupplierId=" + strToContactId + " where SupplierId=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Purchase Order(Supplier)-" + b.ToString() + " , ";
            }

            //for update in purchase order header 
            strsql = "update Inv_PurchaseOrderHeader set ShippingLine=" + strToContactId + " where ShippingLine=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Purchase Order(Shipping Line)-" + b.ToString() + " , ";
            }


            //for update in purchase invoice header 
            strsql = "update Inv_PurchaseInvoiceHeader set SupplierId=" + strToContactId + " where SupplierId=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Purchase Invoice(Customer)-" + b.ToString() + " , ";
            }

            //for update in Inv_InvoiceShippingHeader
            strsql = "update Inv_InvoiceShippingHeader set Shipping_Line=" + strToContactId + " where Shipping_Line=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Purchase Invoice(Shipping Line)-" + b.ToString() + " , ";
            }

            //for update in Inv_InvoiceShippingHeader
            strsql = "update Inv_ProductionRequestHeader set Customer_Id=" + strToContactId + " where Customer_Id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Production Request(Customer)-" + b.ToString() + " , ";
            }



            ////for update in Lic Table
            //strsql = "update Lic_SoftwareInquiry set Contact_Id=0 where Contact_Id=0";
            //b = objda.execute_Command(strsql);

            //if (b != 0)
            //{
            //    counter++;
            //}

            //for update in Ac_Ageing_Detail
            strsql = "update Ac_Ageing_Detail set Other_Account_No=" + strToAcId + " where Other_Account_No=" + strFromAcId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Account Ageing(Customer)-" + b.ToString() + " , ";
            }

            //for update in Ac_Voucher_Detail
            strsql = "update Ac_Voucher_Detail set Other_Account_No=" + strToAcId + " where Other_Account_No=" + strFromAcId + " and account_no in (" + strPaymentVoucherAcc + "," + strReceiveVoucherAcc + ")";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Account Voucher(Customer)-" + b.ToString() + " , ";
            }



            //update contact company name 

            strsql = "update Ems_ContactMaster set Company_Id = " + strToContactId + " where Company_Id = " + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            if (b != 0)
            {
                counter++;
                message += "Contact(Company name)-" + b.ToString() + " , ";

            }



            //set is active false to old contact 

            strsql = "update Ems_ContactMaster set IsActive='False' ,ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            //set is active false in customer table
            strsql = "update Set_CustomerMaster set IsActive='False' ,ModifiedDate='" + DateTime.Now.ToString() + "' where customer_id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);

            //set is active false in supplier table
            strsql = "update Set_Suppliers set IsActive='False' ,ModifiedDate='" + DateTime.Now.ToString() + "' where supplier_id=" + strFromContactId + "";
            b = objda.execute_Command(strsql, ref trns);


            if (b != 0)
            {
                counter++;
                message += "Contact Deleted-" + b.ToString() + " , ";

            }

            //------------update opening balance - neelkanth purohit 20-07-18---------------
            int totalRecordAffected = 0;
            strsql = "select * from ac_subChartOfAccount where IsActive='true' and company_id='" + Session["CompId"].ToString() + "' and other_account_no in (" + strFromAcId + "," + strToAcId + ") and accTransId in (" + strPaymentVoucherAcc + "," + strReceiveVoucherAcc + ")";
            DataTable dtOpening = objda.return_DataTable(strsql);
            if (dtOpening.Rows.Count > 0)
            {
                DataTable dtLocation = dtOpening.DefaultView.ToTable(true, "location_id");
                DataTable dtYear = dtOpening.DefaultView.ToTable(true, "FinancialYearId");
                foreach (DataRow rowLocation in dtLocation.Rows)
                {
                    foreach (DataRow rowYear in dtYear.Rows)
                    {
                        DataRow[] drData = dtOpening.Select("Location_id='" + rowLocation["location_id"].ToString() + "' and FinancialYearId='" + rowYear["FinancialYearId"].ToString() + "'");
                        if (drData.Count() > 0)
                        {
                            double drAmount = 0;
                            double crAmount = 0;
                            double fDrAmount = 0;
                            double fCrAmount = 0;
                            double cmpDrAmount = 0;
                            double cmpCrAmount = 0;
                            if (drData.Count() == 1 && drData[0]["other_account_no"].ToString() == strFromAcId.ToString())
                            {
                                b = objda.execute_Command("Update ac_subChartOfAccount set other_account_no=" + strToAcId + ",ModifiedDate='" + DateTime.Now.ToString() + "', Modifiedby='" + Session["UserId"].ToString() + "'  where trans_id=" + drData[0]["trans_id"].ToString(), ref trns);
                                if (b != 0)
                                {
                                    totalRecordAffected = totalRecordAffected + b;
                                }
                                continue;
                            }

                            foreach (DataRow drOpenYear in drData)
                            {
                                drAmount = drAmount + Convert.ToDouble(drOpenYear["LDr_Amount"].ToString());
                                crAmount = crAmount + Convert.ToDouble(drOpenYear["LCr_Amount"].ToString());
                                cmpDrAmount = cmpDrAmount + Convert.ToDouble(drOpenYear["CompanyCurrDebit"].ToString());
                                cmpCrAmount = cmpCrAmount + Convert.ToDouble(drOpenYear["CompanyCurrCredit"].ToString());
                                fDrAmount = fDrAmount + Convert.ToDouble(drOpenYear["FDr_Amount"].ToString());
                                fCrAmount = fCrAmount + Convert.ToDouble(drOpenYear["FCr_Amount"].ToString());
                            }

                            strsql = "update ac_subchartofAccount set LDr_Amount=" + (drAmount - crAmount > 0 ? (drAmount - crAmount) : 0).ToString() + ", LCr_Amount=" + (crAmount - drAmount > 0 ? (crAmount - drAmount) : 0).ToString() + ",FDr_Amount=" + (fDrAmount - fCrAmount > 0 ? (fDrAmount - fCrAmount) : 0).ToString() + ", fCr_Amount=" + (fCrAmount - fDrAmount > 0 ? (fCrAmount - fDrAmount) : 0).ToString() + ",CompanyCurrDebit=" + (cmpDrAmount - cmpCrAmount > 0 ? (cmpDrAmount - cmpCrAmount) : 0).ToString() + ",CompanyCurrCredit=" + (cmpCrAmount - cmpDrAmount > 0 ? (cmpCrAmount - cmpDrAmount) : 0).ToString() + ",ModifiedDate='" + DateTime.Now.ToString() + "', Modifiedby='" + Session["UserId"].ToString() + "',other_account_no=" + strToAcId + "  where trans_id=" + drData[0]["trans_id"].ToString();
                            objda.execute_Command(strsql, ref trns);
                            try
                            {
                                strsql = "update ac_subchartofAccount set isActive='false',ModifiedDate='" + DateTime.Now.ToString() + "', Modifiedby='" + Session["UserId"].ToString() + "' where trans_id=" + drData[1]["trans_id"].ToString();
                                objda.execute_Command(strsql, ref trns);
                            }
                            catch (Exception ex)
                            {

                            }
                            totalRecordAffected = totalRecordAffected + 2;

                        }
                    }
                }

            }

            //------------end-----------------------------------




            if (counter != 0)
            {
                DisplayMessage(message + " Record affected");
            }
            else
            {
                DisplayMessage("Record not found");
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }

    }
    private string GetCustomerId(TextBox txtCustomerName)
    {
        string retval = string.Empty;
        if (txtCustomerName.Text != "")
        {
            DataTable dtSupp = objContact.GetContactByContactName(txtCustomerName.Text.Trim().Split('/')[0].ToString());
            if (dtSupp.Rows.Count > 0)
            {
                try
                {
                    retval = txtCustomerName.Text.Split('/')[1].ToString();
                }
                catch
                {
                    retval = "0";

                }
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
    protected void fillDdlCurrecy()
    {
        DataTable _dtCurrency = new CurrencyMaster(Session["DBConnection"].ToString()).GetCurrencyMaster();
        objPageCmn.FillData((object)ddlCurrency, _dtCurrency, "Currency_Name", "Currency_Id");
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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = ObjContactMaster.GetContactTrueAllData();
        try
        {
            dtCustomer = new DataView(dtCustomer, "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();

        }
        catch
        {
            dtCustomer = ObjContactMaster.GetContactTrueAllData();
        }
        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }
}