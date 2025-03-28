<%@ WebService Language="C#" Class="customer" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using PegasusDataAccess;
using System.Data;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]

public class customer : System.Web.Services.WebService
{
    [WebMethod(EnableSession = true)]
    public string getCustomerAddress(string strCustomerName, string strCustomerId)
    {
        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;
        string sql = "select customer_id,is_block from set_customerMaster inner join ems_contactMaster on ems_contactMaster.trans_id=set_customerMaster.customer_id where Set_CustomerMaster.customer_id='" + strCustomerId + "' and ems_contactMaster.name='" + strCustomerName + "'";
        DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        using (DataTable dtCustomer = objDa.return_DataTable(sql))
        {
            if (dtCustomer.Rows.Count == 0 || bool.Parse(dtCustomer.Rows[0]["Is_Block"].ToString()))
            {
                return "false";
            }
        }

        string strAddress = "";
        Ems_ContactMaster objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        DataTable dtAddress = objContact.GetAddressByRefType_Id("Contact", strCustomerId);
        if (dtAddress.Rows.Count > 0)
        {
            dtAddress = new DataView(dtAddress, "Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtAddress != null)
            {
                if (dtAddress.Rows.Count > 0)
                {
                    strAddress = dtAddress.Rows[0]["Address_Name"].ToString() + "/" + dtAddress.Rows[0]["Trans_Id"].ToString();
                }
            }
        }
        dtAddress = null;
        return strAddress;
    }

    [WebMethod(EnableSession = true)]
    public string getCreditInfo(string strCustomerId, string strCurrencyId)
    {
        DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        string strsql = string.Empty;
        double closingBalance = 0;
        double unPostedVoucherAmount = 0;
        try
        {
            string strCreditLimit = "0";
            string strCreditDays = "0";
            string strCreditContiditon = "";
            //Get Customer account id based on cutomer_id and currency_id
            Ac_AccountMaster objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
            string strOtherAccountId = objAcAccountMaster.GetCustomerAccountByCurrency(strCustomerId, strCurrencyId).ToString();
            if (strOtherAccountId == "0")
            {
                return null;
            }
            Set_CustomerMaster_CreditParameter objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(Session["DBConnection"].ToString());
            using (DataTable dtCreditParameter = objCustomerCreditParam.GetCustomerRecord_By_OtherAccountId(strOtherAccountId))
            {
                if (dtCreditParameter.Rows.Count > 0)
                {
                    strCreditLimit = SystemParameter.GetAmountWithDecimal(dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim(), HttpContext.Current.Session["LoginLocDecimalCount"].ToString());
                    strCreditDays = dtCreditParameter.Rows[0]["Credit_Days"].ToString().Trim();
                    if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()))
                    {
                        strCreditContiditon = "Advance Cheque Basis";
                    }
                    else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()))
                    {
                        strCreditContiditon = "Invoice to Invoice Payment";
                    }
                    else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim()))
                    {
                        strCreditContiditon = "50% advance and 50% on delivery";
                    }
                    else
                    {
                        strCreditContiditon = "None";
                    }

                    //here we are getting customer balance 

                    strsql = "select  dbo.Ac_GetBalance(" + HttpContext.Current.Session["CompId"].ToString() + "," + HttpContext.Current.Session["BrandId"].ToString() + "," + HttpContext.Current.Session["LocId"].ToString() + ",'" + DateTime.Now.ToString() + "',0," + Ac_ParameterMaster.GetCustomerAccountNo(HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["DBConnection"].ToString()) + "," + strOtherAccountId + ",3,'" + HttpContext.Current.Session["FinanceYearId"].ToString() + "')";
                    double.TryParse(objDa.get_SingleValue(strsql), out closingBalance);

                    //Get Unposted Voucher Active voucher
                    strsql = "select sum((case when Debit_Amount > 0 then Foreign_Amount else 0 end)) as amt  from ac_voucher_detail inner join ac_voucher_header on ac_voucher_header.trans_id = ac_voucher_detail.voucher_no where ac_voucher_header.Location_id='" + HttpContext.Current.Session["LocId"].ToString() + "' and ac_voucher_header.IsActive = 'true' and ac_voucher_header.ReconciledFromFinance = 'false' and Account_No = '" + Ac_ParameterMaster.GetCustomerAccountNo(HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["DBConnection"].ToString()) + "' and Other_Account_No = '" + strOtherAccountId + "'";
                    double.TryParse(objDa.get_SingleValue(strsql), out unPostedVoucherAmount);
                    closingBalance = closingBalance + unPostedVoucherAmount;


                }
            }
            return strCreditLimit + "," + strCreditDays + "," + strCreditContiditon+","+closingBalance;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    [WebMethod(enableSession: true)]
    public string customerValidation(string strCustomerName, string strCustomerId)
    {
        string sql = "select customer_id,is_block from set_customerMaster inner join ems_contactMaster on ems_contactMaster.trans_id=set_customerMaster.customer_id where Set_CustomerMaster.customer_id='" + strCustomerId + "' and ems_contactMaster.name='" + strCustomerName + "'";
        DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        using (DataTable dtCustomer = objDa.return_DataTable(sql))
        {
            if (dtCustomer.Rows.Count == 0 || bool.Parse(dtCustomer.Rows[0]["Is_Block"].ToString()))
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }
    }
    [WebMethod(EnableSession = true)]
    public string customerValidation(string strCustomerName, string strCustomerId, string sessionName)
    {
        string sql = "select customer_id,is_block from set_customerMaster inner join ems_contactMaster on ems_contactMaster.trans_id=set_customerMaster.customer_id where Set_CustomerMaster.customer_id='" + strCustomerId + "' and ems_contactMaster.name='" + strCustomerName + "'";
        DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        using (DataTable dtCustomer = objDa.return_DataTable(sql))
        {
            if (dtCustomer.Rows.Count == 0 || bool.Parse(dtCustomer.Rows[0]["Is_Block"].ToString()))
            {
                HttpContext.Current.Session[sessionName] = "0";
                return "false";
            }
            else
            {
                HttpContext.Current.Session[sessionName] = dtCustomer.Rows[0]["customer_id"].ToString();
                return "true";
            }
        }
    }
    [WebMethod(enableSession: true)]
    public bool contactPersonValidation(string name, string id)
    {
        string count = "0";
        DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        count = objDa.get_SingleValue("select COUNT(Trans_Id) as empCount from Ems_ContactMaster where Name = '" + name + "' and Trans_Id = '" + id + "'");
        if (count == "0" || count == "@NOTFOUND@")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    [WebMethod(enableSession: true)]
    public bool resetCustomerIdSessionForContact()
    {
        HttpContext.Current.Session["ContactID"] = "0";
        return true;
    }

    [WebMethod(enableSession: true)]
    public bool setContactId(string data)
    {
        HttpContext.Current.Session["ContactID"] = data;
        return true;
    }

    [WebMethod(enableSession: true)]
    public string[] getCustomerAddressName(string customerName, string customerId)
    {
            DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        string[] result = new string[2];
        string isValidCustomer = customerValidation(customerName, customerId);
        if (isValidCustomer == "true")
        {
            result[1] = objDa.get_SingleValue("SELECT Set_AddressMaster.Address_Name FROM dbo.Set_AddressMaster INNER JOIN dbo.Set_AddressChild ON dbo.Set_AddressMaster.Trans_Id = dbo.Set_AddressChild.Ref_Id WHERE Set_AddressChild.Add_Ref_Id = '" + customerId + "'  AND Set_AddressChild.Add_Type = 'contact'");
            result[1] = result[1] == "@NOTFOUND@" ? "" : result[1];
            result[0] = "true";
            return result;
        }
        else
        {
            result[0] = "false";
            result[1] = "Customer Not Valid";
            return result;
        }
    }

    [WebMethod(enableSession: true)]
    public string getContactIdFromName(string Name)
    {
            DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        string result = "";
        result = objDa.get_SingleValue("select top 1 trans_id from ems_contactmaster where isactive='true' and name = '" + Name + "'");
        result = result == "@NOTFOUND@" ? "" : result;
        return result;
    }

    [WebMethod(enableSession: true)]
    public string getContactIdFromNameNId(string Name, string id)
    {
            DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        string result = "";
        result = objDa.get_SingleValue("select top 1 trans_id from ems_contactmaster where isactive='true' and name = '" + Name + "' and trans_id='" + id + "'");
        result = result == "@NOTFOUND@" ? "" : result;
        return result;
    }

    [WebMethod(enableSession: true)]
    public string getContactIdFromNameNIdWithSetSession(string Name, string id, string sessionName)
    {
            DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        string result = "";
        result = objDa.get_SingleValue("select top 1 trans_id from ems_contactmaster where isactive='true' and name = '" + Name + "' and trans_id='" + id + "'");
        result = result == "@NOTFOUND@" ? "" : result;
        if (result != "")
        {
            Session[sessionName] = result;
        }
        else
        {
            Session[sessionName] = "";
        }
        return result;
    }

    [WebMethod(enableSession: true)]
    public string[] getSupplierAddressName(string Name, string Id)
    {
            DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        string[] result = new string[2];
        string isValidCustomer = validateSupplier(Name);
        if (isValidCustomer != "0")
        {
            result[1] = objDa.get_SingleValue("SELECT Set_AddressMaster.Address_Name FROM dbo.Set_AddressMaster INNER JOIN dbo.Set_AddressChild ON dbo.Set_AddressMaster.Trans_Id = dbo.Set_AddressChild.Ref_Id WHERE Set_AddressChild.Add_Ref_Id = '" + Id + "'  AND Set_AddressChild.Add_Type = 'supplier'");
            result[1] = result[1] == "@NOTFOUND@" ? "" : result[1];
            result[0] = "true";
            return result;
        }
        else
        {
            result[0] = "false";
            result[1] = "No Address Found";
            return result;
        }
    }
    [WebMethod(enableSession: true)]
    public string validateSupplier(string name)
    {
            DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        string result = "";
        result = objDa.get_SingleValue("SELECT count(Ems_ContactMaster.Trans_Id) as totalCount FROM Ems_ContactMaster LEFT JOIN Ems_ContactMaster e2 ON Ems_ContactMaster.Company_Id = e2.Trans_Id WHERE Ems_ContactMaster.IsActive = 'True' and (Ems_ContactMaster.Name + '/' + CAST(Ems_ContactMaster.Trans_Id AS nvarchar(50)) ) like '%" + name + "%'");
        result = result == "@NOTFOUND@" ? "0" : result;
        return result;
    }
}