using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Summary description for Ac_EmailSend
/// </summary>
public class Ac_EmailSend
{
    DataAccessClass objDa = null;
    Ac_Parameter_Location objAcParamLocation = null;
    CompanyMaster objCompany = null;
    CustomerAgeingEstatement objCustomerAgeingStatement = null;
    SystemParameter objsys = null;
    NotificationMaster objNotification = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    private string  _strConString = string.Empty;

    public Ac_EmailSend(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        ObjAgeingDetail = new Ac_Ageing_Detail(strConString);
        objDa = new DataAccessClass(strConString);
        objAcParamLocation = new Ac_Parameter_Location(strConString);
        objCompany = new CompanyMaster(strConString);
        objCustomerAgeingStatement = new CustomerAgeingEstatement(strConString);
        objsys = new SystemParameter(strConString);
        objNotification = new NotificationMaster(strConString);
        ObjCurrencyMaster = new CurrencyMaster(strConString);
    }

    public bool sendEmail(string strCompany_Id, string strBrand_Id, string strLocation_Id, string strCompany_name, string strCompanyLogo_url, string strTempPath, string strEmp_id, string strUser_id, string strRec_Type, string strEmailLogTransId, string conString)
    {
        bool result = false;
        DataTable dtParameter = objAcParamLocation.GetParameterMasterAllTrue(strCompany_Id, strBrand_Id, strLocation_Id, conString);
        string strSenderId = string.Empty;
        string strSenderPassword = string.Empty;
        string strSMTP = string.Empty;
        string strSMTPPort = string.Empty;
        //string strPop = string.Empty;
        //string strPopPort = string.Empty;
        string strDispalytext = string.Empty;
        bool isEnableSsl = false;
        string strNotificationText = string.Empty;
        int ErrorCount = 0;
        try
        {
            strSenderId = new DataView(dtParameter, "Param_Name='Finance_Email'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
            strSenderPassword = new DataView(dtParameter, "Param_Name='Finance_Email_Password'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
            strSenderPassword = Common.Decrypt(strSenderPassword);
            strSMTP = new DataView(dtParameter, "Param_Name='Finance_Email_SMTP'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
            strSMTPPort = new DataView(dtParameter, "Param_Name='Finance_Email_Port_Out'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
            //strPop = new DataView(dtParameter, "Param_Name='Finance_Email_Pop'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
            //strPopPort = new DataView(dtParameter, "Param_Name='Finance_Email_Port_In'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
            strDispalytext = new DataView(dtParameter, "Param_Name='Finance_Email_Display_Text'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
            isEnableSsl = Convert.ToBoolean(new DataView(dtParameter, "Param_Name='Finance_Email_EnableSSL'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString());
        }
        catch (Exception ex)
        {
            strNotificationText = "Please check Email configuration in Accounts Parameter";
            updateNotification(strCompany_Id, strBrand_Id, strLocation_Id, strEmp_id, strUser_id, strNotificationText, conString);
            return false;
        }

        DataTable _dt = new DataTable();
        _dt = new Ac_EmailLog(_strConString).getAcEmailLogByStatus(strLocation_Id, strRec_Type, "Pending", conString);
        _dt = new DataView(_dt, "trans_id in (" + strEmailLogTransId + ")", "", DataViewRowState.CurrentRows).ToTable();
        if (_dt.Rows.Count > 0)
        {
            AccountsDataset ObjAccountDataset = new AccountsDataset();
            ObjAccountDataset.EnforceConstraints = false;
            AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter adp = new AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter();
            adp.Connection.ConnectionString = conString;
            string strEmailFooter = objAcParamLocation.GetParameterValue_By_ParameterName(strCompany_Id, strBrand_Id, strLocation_Id, "Finance_Email_Statement_Footer", conString).Rows[0]["Param_Value"].ToString();
            foreach (DataRow dr in _dt.Rows)
            {
                adp.Fill(ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement, strCompany_Id, strBrand_Id, dr["field1"].ToString(), "RV", dr["field3"].ToString(), (Convert.ToDateTime(dr["Statement_Date"].ToString())).Date);
                DataTable dt = ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement;
                Ac_Ageing_Detail.clsAgeingDaysDetail clsAging = new Ac_Ageing_Detail.clsAgeingDaysDetail();
                if (dt.Rows.Count > 0)
                {
                    int CurrencydecimalCount = 0;
                    int.TryParse(ObjCurrencyMaster.GetCurrencyMasterById(dt.Rows[0]["Currency_Id"].ToString(), conString).Rows[0]["decimal_count"].ToString(), out CurrencydecimalCount);

                    //set ageing days detail in footer of the print
                    clsAging = ObjAgeingDetail.getAgingDayDetail(dt, CurrencydecimalCount == 0 ? 2 : CurrencydecimalCount);
                }
                objCustomerAgeingStatement.DataSource = dt;
                //Session["DtRvInvoiceStatement"] = dt;
                objCustomerAgeingStatement.DataMember = "sp_Ac_InvoiceAgeingStatement";
                //ReportViewer1.Report = objCustomerAgeingStatement;
                //ReportToolbar1.ReportViewer = ReportViewer1;

                objCustomerAgeingStatement.setcompanyname(strCompany_name);
                objCustomerAgeingStatement.SetImage(strCompanyLogo_url);
                objCustomerAgeingStatement.setStatementDate(DateTime.Parse(dr["Statement_Date"].ToString()).ToString("dd-MMM-yyyy"));
                string strCustomerAddress = objDa.get_SingleValue("SELECT Set_AddressMaster.Address FROM Set_AddressMaster WHERE Set_AddressMaster.Trans_Id = (SELECT TOP 1 Set_AddressChild.Ref_Id FROM Set_AddressChild inner join ac_accountMaster on ac_accountMaster.ref_id=Set_AddressChild.Add_Ref_Id WHERE (Set_AddressChild.Add_Type = 'Contact' or Set_AddressChild.Add_Type = 'Customer') AND ac_accountMaster.trans_id = '" + dr["field3"].ToString() + "')", conString);
                objCustomerAgeingStatement.setCustomerAddress(strCustomerAddress);
                objCustomerAgeingStatement.setFooterNote(strEmailFooter);
                objCustomerAgeingStatement.setAgeingDaysDetail(clsAging);
                string strReportFileName = string.Empty;
                strReportFileName = "Invoice_Statement_" + DateTime.Parse(dr["Statement_Date"].ToString()).ToString("dd-MMM-yyyy") + "_" + dr["Customer_id"].ToString() + ".pdf";
                string strReportPhPath = strTempPath + "\\" + strReportFileName;
                objCustomerAgeingStatement.CreateDocument(true);
                objCustomerAgeingStatement.ExportToPdf(strReportPhPath);

                string strReceiverId = dr["Receiver_email"].ToString();
                int retryCount = 0;
                Int32.TryParse(dr["field2"].ToString(), out retryCount);
                retryCount++;

                try
                {
                    //using gmail
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                    if (strReportFileName != "")
                    {
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(strReportPhPath);
                        message.Attachments.Add(attachment);
                    }

                    message.To.Add(strReceiverId);
                    message.Subject = dr["Subject"].ToString();
                    // message.From = new System.Net.Mail.MailAddress(GetSystemMailId(compid));
                    message.From = new System.Net.Mail.MailAddress(strSenderId);
                    message.IsBodyHtml = true;
                    message.Body = "Dear Sir/Madam, <br /> Attached, please find our Statement of Account. We request you to kindly release the payment at the earliest. <br /> An early update on the payment status will be highly appreciated";
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(strSMTP);

                    NetworkCredential basiccr = new NetworkCredential(strSenderId, strSenderPassword);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = basiccr;

                    smtp.EnableSsl = isEnableSsl;
                    smtp.Port = Convert.ToInt32(strSMTPPort);

                    try
                    {
                        smtp.Send(message);
                        message.Attachments.Dispose();
                        //string sql = "update ac_emaillog set status='Sent' where trans_id=" + dr["Trans_id"].ToString();
                        objDa.execute_Command("update ac_emaillog set status='Sent',field2='" + retryCount + "',Delivered_Date='" + DateTime.Now.ToString() + "', ModifiedBy='" + strUser_id + "', ModifiedDate='" + DateTime.Now.ToString() + "' where trans_id='" + dr["trans_id"].ToString() + "'", conString);
                        //return true;
                    }
                    catch (SmtpException ex)
                    {

                        //ExportToFile("Error Is:" + e.ToString(), "C:\\PegasusSQL\\MailError.txt", FileMode.Append);
                        //return false;
                        objDa.execute_Command("update ac_emaillog set Exception_Detail='Smtp Exception Status Code - " + ex.StatusCode + "',field2='" + retryCount + "',ModifiedBy='" + strUser_id + "', ModifiedDate='" + DateTime.Now.ToString() + "' where trans_id='" + dr["trans_id"].ToString() + "'", conString);
                        ErrorCount++;

                    }


                }
                catch (Exception ex)
                {
                    // Write in File
                    //WriteSettingsFile(GetSystemMailId(compid, ref trns), strTo, strMsg, GetSystemMailSmtpHost(compid, ref trns), getSystemMailPwd(compid, ref trns), GetSystemMailPort(compid, ref trns), IsSSLEnabled(compid, ref trns));
                    //return false;
                    ErrorCount++;
                }

            }
            if (ErrorCount > 0)
            {
                strNotificationText = "Email Send process for customer statement has been completed with " + ErrorCount.ToString() + " Errors.";
            }
            else
            {
                strNotificationText = "Email Send process for customer statement has been completed.";
            }
            updateNotification(strCompany_Id, strBrand_Id, strLocation_Id, strEmp_id, strUser_id, strNotificationText, conString);
        }
        return result;
    }

    protected void updateNotification(string strCompany_id, string strBrand_id, string strLocation_id, string strEmp_id, string strUser_id, string strMessages, string conString = "")
    {
        DataTable Dt_Request_Type = new DataTable();
        Dt_Request_Type = objNotification.Get_Notification_ID_By_Type("Notification", "", "", "5", conString);
        objNotification.InsertNotificationMaster_Trans(strCompany_id, strBrand_id, strLocation_id, strEmp_id, strEmp_id, strEmp_id, strMessages, Dt_Request_Type.Rows[0]["Trans_ID"].ToString(), "", "", "0", "False", "", "", "", "", "", strUser_id, System.DateTime.Now.ToString(), strUser_id, System.DateTime.Now.ToString(), "0", "18", conString);
    }
}