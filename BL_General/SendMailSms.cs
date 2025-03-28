using System;
using System.Data;
using System.Configuration;
//
using System.Web;

using System.Net.Mail;
using System.Text;
using System.IO;
using System.Net;
using PegasusDataAccess;
using System.Data.SqlClient;

public class SendMailSms
{
    DataAccessClass daClass = null;
    Set_ApplicationParameter objAppParam = null;
    UserDetail objuserDetail = null;
    private string _strConString = string.Empty;
    public SendMailSms(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objAppParam = new Set_ApplicationParameter(strConString);
        objuserDetail = new UserDetail(strConString);
        _strConString = strConString;
    }
    public void ExportToFile(string LogFormat, string Filepath, System.IO.FileMode Mode)
    {
        FileStream fs = new FileStream(Filepath, Mode, FileAccess.Write);
        StreamWriter s = new StreamWriter(fs);
        s.BaseStream.Seek(0, SeekOrigin.End);
        s.WriteLine(LogFormat);
        s.Close();
        fs.Close();
    }
    public bool SendMail(string strTo, string strSubject, string strMsg, string compid, string Attachment, string strAttachpath, string strBrandId, string strLocId)
    {
        try
        {
            //using gmail
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            if (Attachment != "")
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(strAttachpath);
                message.Attachments.Add(attachment);
            }
            message.To.Add(strTo);
            message.Subject = strSubject;
            message.From = new System.Net.Mail.MailAddress(GetSystemMailId(compid, strBrandId, strLocId));
            message.IsBodyHtml = true;
            message.Body = strMsg;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(GetSystemMailSmtpHost(compid, strBrandId, strLocId));

            NetworkCredential basiccr = new NetworkCredential(GetSystemMailId(compid, strBrandId, strLocId), getSystemMailPwd(compid, strBrandId, strLocId));
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = basiccr;

            smtp.EnableSsl = IsSSLEnabled(compid, strBrandId, strLocId);
            smtp.Port = GetSystemMailPort(compid, strBrandId, strLocId);

            try
            {
                smtp.Send(message);
                return true;
            }
            catch (Exception e)
            {
                ExportToFile("Error Is:" + e.ToString(), "C:\\PegasusSQL\\MailError.txt", FileMode.Append);
                return false;
            }

            //end using gmail

            //MailMessage objMailMsg = new MailMessage(GetSystemMailId(), strTo);

            //objMailMsg.BodyEncoding = Encoding.UTF8;
            //objMailMsg.Subject = strSubject;
            //objMailMsg.Body = strMsg;

            //objMailMsg.Priority = System.Net.Mail.MailPriority.High;
            //objMailMsg.IsBodyHtml = true;

            ////prepare to send mail via SMTP transport
            //SmtpClient objSMTPClient = newp SmtpClient();

            //objSMTPClient.Timeout = 1000000;
            //objSMTPClient.Send(objMailMsg);
            //return true;
        }
        catch (Exception ex)
        {
            // Write in File
            WriteSettingsFile(GetSystemMailId(compid, strBrandId, strLocId), strTo, strMsg, GetSystemMailSmtpHost(compid, strBrandId, strLocId), getSystemMailPwd(compid, strBrandId, strLocId), GetSystemMailPort(compid, strBrandId, strLocId), IsSSLEnabled(compid, strBrandId, strLocId));
            return false;
        }
    }


    public bool SendMail_TicketInfo(string strTo, string strCc, string StrBcc, string strSubject, string strMsg, string compid, string strAttachment, string UserName, string Password, string DisplayName, string Master_Email_SMTP, string Master_Email_Port, string strFilePathLink, string strBrandId, string strLocId)
    {
        try
        {
            //using gmail


            string strsql = "select * from Set_UserDetail where Email='" + UserName.Trim() + "'";
            DataTable dt = daClass.return_DataTable(strsql);
            //DataTable dt = new DataView(objuserDetail.GetbyUserId(HttpContext.Current.Session["UserId"].ToString(), compid), "Email='" + UserName.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            string IncomingMailServer = string.Empty;
            int port = 0;
            bool Enablessl = false;

            IncomingMailServer = Master_Email_SMTP;
            port = Convert.ToInt32(Master_Email_Port);
            Enablessl = false;

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            if (strAttachment != "")
            {
                foreach (string ToEMailId in strAttachment.Split(','))
                {
                    try
                    {
                        message.Attachments.Add(new Attachment(ToEMailId));

                    }
                    catch
                    {

                    }
                }
            }
            foreach (string ToEMailId in strTo.Split(';'))
            {
                try
                {
                    message.To.Add(new MailAddress(ToEMailId));
                }
                catch
                {


                }
            }
            if (strCc.ToString() != "")
            {
                foreach (string ToEMailId in strCc.Split(';'))
                {

                    try
                    {
                        message.CC.Add(new MailAddress(ToEMailId));
                    }
                    catch
                    {


                    }
                }
            }
            if (StrBcc.ToString() != "")
            {

                foreach (string ToEMailId in StrBcc.Split(';'))
                {

                    try
                    {
                        message.Bcc.Add(new MailAddress(ToEMailId));
                    }
                    catch
                    {


                    }
                }
            }
            message.Subject = strSubject;
            message.From = new System.Net.Mail.MailAddress(UserName, DisplayName);
            message.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(IncomingMailServer, port);
            NetworkCredential basiccr = new NetworkCredential(UserName, Password);

            try
            {
                if (!string.IsNullOrEmpty(strFilePathLink))
                {

                    string StrfilePathlink = strFilePathLink;

                    foreach (string str in StrfilePathlink.Split(','))
                    {
                        try
                        {
                            if (str != "")
                            {
                                string[] straa = str.Split('/');
                                //  Add image to HTML version
                                try
                                {
                                    strMsg = strMsg.Trim().Replace(str, "cid:" + straa[straa.Length - 1].ToString());
                                }
                                catch
                                { }

                            }

                        }
                        catch
                        {

                        }

                    }
                    System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(strMsg.Trim(), null, "text/html");

                    //foreach (string str in StrfilePathlink.Split(','))
                    //{
                    //    try
                    //    {
                    //        string[] straa = str.Split('/');
                    //        //  Add image to HTML version
                    //        System.Net.Mail.LinkedResource imageResource = new System.Net.Mail.LinkedResource((HttpContext.Current.Server.MapPath(str)), "image/jpeg");
                    //        try
                    //        {
                    //            imageResource.ContentId = straa[straa.Length - 1].ToString();

                    //        }
                    //        catch
                    //        { }

                    //        htmlView.LinkedResources.Add(imageResource);
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}
                    message.AlternateViews.Add(htmlView);
                }
                else
                {
                    message.Body = strMsg.ToString();
                }

            }
            catch
            {

            }
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = basiccr;
            smtp.EnableSsl = Enablessl;
            try
            {
                //SendMailThread(message, smtp);
                smtp.Send(message);
                message.Attachments.Dispose();
                // return true;
            }
            catch (Exception e)
            {
                message.Attachments.Dispose();

                ExportToFile("Error Is:" + e.ToString(), "C:\\PegasusSQL\\MailError.txt", FileMode.Append);
                // return false;
            }
            //end using gmail

            //MailMessage objMailMsg = new MailMessage(GetSystemMailId(), strTo);

            //objMailMsg.BodyEncoding = Encoding.UTF8;
            //objMailMsg.Subject = strSubject;
            //objMailMsg.Body = strMsg;

            //objMailMsg.Priority = System.Net.Mail.MailPriority.High;
            //objMailMsg.IsBodyHtml = true;

            ////prepare to send mail via SMTP transport
            //SmtpClient objSMTPClient = new SmtpClient();

            //objSMTPClient.Timeout = 1000000;
            //objSMTPClient.Send(objMailMsg);
            //return true;
        }
        catch (Exception ex)
        {
            // Write in File
            WriteSettingsFile(GetSystemMailId(compid, strBrandId, strLocId), strTo, strMsg, GetSystemMailSmtpHost(compid, strBrandId, strLocId), getSystemMailPwd(compid, strBrandId, strLocId), GetSystemMailPort(compid, strBrandId, strLocId), IsSSLEnabled(compid, strBrandId, strLocId));
            // return false;
        }

        return true;
    }

    private static void SendMailThread(MailMessage message, System.Net.Mail.SmtpClient smtp)
    {
        using (var server = smtp)
        {
            server.Credentials = smtp.Credentials;
            server.SendAsync(message, null);
        }
    }

    public bool SendMail(string strTo, string strCc, string StrBcc, string strSubject, string strMsg, string compid, string strAttachment, string UserName, string Password, string DisplayName, string struserid, string strFilePathLink, string strBrandId, string strLocId)
    {
        try
        {
            //using gmail

            UserDetail objuserDetail = new UserDetail(_strConString);
            DataTable dt = new DataView(objuserDetail.GetbyUserId(struserid, compid), "Email='" + UserName.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            string IncomingMailServer = string.Empty;
            int port = 0;
            bool Enablessl = false;
            if (dt.Rows.Count != 0)
            {
                IncomingMailServer = dt.Rows[0]["Field1"].ToString();
                port = Convert.ToInt32(dt.Rows[0]["Field2"].ToString());
                Enablessl = Convert.ToBoolean(dt.Rows[0]["Field5"].ToString());
            }
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            if (strAttachment != "")
            {
                foreach (string ToEMailId in strAttachment.Split(','))
                {
                    try
                    {
                        message.Attachments.Add(new Attachment(ToEMailId));

                    }
                    catch
                    {

                    }
                }
            }
            foreach (string ToEMailId in strTo.Split(';'))
            {
                try
                {
                    message.To.Add(new MailAddress(ToEMailId));
                }
                catch
                {


                }
            }
            if (strCc.ToString() != "")
            {
                foreach (string ToEMailId in strCc.Split(';'))
                {

                    try
                    {
                        message.CC.Add(new MailAddress(ToEMailId));
                    }
                    catch
                    {


                    }
                }
            }
            if (StrBcc.ToString() != "")
            {

                foreach (string ToEMailId in StrBcc.Split(';'))
                {

                    try
                    {
                        message.Bcc.Add(new MailAddress(ToEMailId));
                    }
                    catch
                    {


                    }
                }
            }
            message.Subject = strSubject;
            message.From = new System.Net.Mail.MailAddress(UserName, DisplayName);
            message.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(IncomingMailServer, port);
            NetworkCredential basiccr = new NetworkCredential(UserName, Password);

            try
            {
                if (!string.IsNullOrEmpty(strFilePathLink))
                {

                    string StrfilePathlink = strFilePathLink;

                    foreach (string str in StrfilePathlink.Split(','))
                    {
                        try
                        {
                            if (str != "")
                            {
                                string[] straa = str.Split('/');
                                //  Add image to HTML version
                                try
                                {
                                    strMsg = strMsg.Trim().Replace(str, "cid:" + straa[straa.Length - 1].ToString());
                                }
                                catch
                                { }

                            }

                        }
                        catch
                        {

                        }

                    }
                    System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(strMsg.Trim(), null, "text/html");

                    //foreach (string str in StrfilePathlink.Split(','))
                    //{
                    //    try
                    //    {
                    //        string[] straa = str.Split('/');
                    //        Add image to HTML version
                    //        System.Net.Mail.LinkedResource imageResource = new System.Net.Mail.LinkedResource((HttpContext.Current.Server.MapPath(str)), "image/jpeg");
                    //        try
                    //        {
                    //            imageResource.ContentId = straa[straa.Length - 1].ToString();

                    //        }
                    //        catch
                    //        { }

                    //        htmlView.LinkedResources.Add(imageResource);
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}
                    message.AlternateViews.Add(htmlView);
                }
                else
                {
                    message.Body = strMsg.ToString();
                }

            }
            catch
            {

            }
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = basiccr;
            smtp.EnableSsl = Enablessl;

            try
            {
                //HttpContext.Current.Session["FilePathLink"] = null;
                smtp.Send(message);
                message.Attachments.Dispose();
                return true;
            }
            catch (SmtpException e)
            {
                message.Attachments.Dispose();

                ExportToFile("Error Is:" + e.ToString(), "C:\\PegasusSQL\\MailError.txt", FileMode.Append);


                if (e.Message.Contains("Mailbox unavailable"))
                {
                    return true;

                }
                else
                {
                    return false;
                }

            }
            //end using gmail

            //MailMessage objMailMsg = new MailMessage(GetSystemMailId(), strTo);

            //objMailMsg.BodyEncoding = Encoding.UTF8;
            //objMailMsg.Subject = strSubject;
            //objMailMsg.Body = strMsg;

            //objMailMsg.Priority = System.Net.Mail.MailPriority.High;
            //objMailMsg.IsBodyHtml = true;

            ////prepare to send mail via SMTP transport
            //SmtpClient objSMTPClient = new SmtpClient();

            //objSMTPClient.Timeout = 1000000;
            //objSMTPClient.Send(objMailMsg);
            //return true;
        }
        catch (Exception ex)
        {
            // Write in File
            WriteSettingsFile(GetSystemMailId(compid, strBrandId, strLocId), strTo, strMsg, GetSystemMailSmtpHost(compid, strBrandId, strLocId), getSystemMailPwd(compid, strBrandId, strLocId), GetSystemMailPort(compid, strBrandId, strLocId), IsSSLEnabled(compid, strBrandId, strLocId));
            return false;
        }
    }

    private string GetSystemMailSmtpHost(string compid, string strBrandId, string strLocId)
    {
        string retval = string.Empty;

        retval = objAppParam.GetApplicationParameterValueByParamName("Master_Email_SMTP", compid, strBrandId, strLocId);


        return retval;
    }

    private string GetSystemMailSmtpHost(string compid, ref SqlTransaction trns, string strBrandId, string strLocId)
    {
        string retval = string.Empty;

        retval = objAppParam.GetApplicationParameterValueByParamName("Master_Email_SMTP", compid, strBrandId, strLocId, ref trns);


        return retval;
    }

    private int GetSystemMailPort(string compid, string strBrandId, string strLocId)
    {

        int retval = 0;

        retval = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Master_Email_Port", compid, strBrandId, strLocId));


        return retval;


        //return 587;
    }

    private int GetSystemMailPort(string compid, ref SqlTransaction trns, string strBrandId, string strLocId)
    {

        int retval = 0;

        retval = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Master_Email_Port", compid, strBrandId, strLocId, ref trns));


        return retval;


        //return 587;
    }

    private string getSystemMailPwd(string compid, string strBrandId, string strLocId)
    {
        string retval = string.Empty;

        retval = Common.Decrypt(objAppParam.GetApplicationParameterValueByParamName("Master_Email_Password", compid, strBrandId, strLocId));




        return retval;
    }

    private string getSystemMailPwd(string compid, ref SqlTransaction trns, string strBrandId, string strLocId)
    {
        string retval = string.Empty;

        retval = Common.Decrypt(objAppParam.GetApplicationParameterValueByParamName("Master_Email_Password", compid, strBrandId, strLocId, ref trns));




        return retval;
    }

    private string GetSystemMailId(string compid, string strBrandId, string strLocId)
    {
        string retval = string.Empty;

        retval = objAppParam.GetApplicationParameterValueByParamName("Master_Email", compid, strBrandId, strLocId);


        return retval;
    }


    private string GetSystemMailId(string compid, ref SqlTransaction trns, string strBrandId, string strLocId)
    {
        string retval = string.Empty;

        retval = objAppParam.GetApplicationParameterValueByParamName("Master_Email", compid, strBrandId, strLocId, ref trns);


        return retval;
    }

    private bool IsSSLEnabled(string compid, string strBrandId, string strLocId)
    {
        bool retval = false;
        retval = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Master_Email_EnableSSL", compid, strBrandId, strLocId));
        return retval;
    }

    private bool IsSSLEnabled(string compid, ref SqlTransaction trns, string strBrandId, string strLocId)
    {
        bool retval = false;

        retval = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Master_Email_EnableSSL", compid, strBrandId, strLocId, ref trns));


        return retval;
    }

    public bool SendApprovalMail(string strTo, string From, string Paswd, string strSubject, string strMsg, string compid, string Attachment, string strAttachpath, string strBrand, string strLocId)
    {
        //return true;
        try
        {
            //using gmail
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            if (Attachment != "")
            {
                System.Net.Mail.Attachment attachment;
                //attachment = new System.Net.Mail.Attachment(HttpContext.Current.Server.MapPath(Attachment).ToString());
                attachment = new System.Net.Mail.Attachment(strAttachpath);
                message.Attachments.Add(attachment);
            }

            message.To.Add(strTo);
            message.Subject = strSubject;
            // message.From = new System.Net.Mail.MailAddress(GetSystemMailId(compid));
            message.From = new System.Net.Mail.MailAddress(From);
            message.IsBodyHtml = true;
            message.Body = strMsg;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(GetSystemMailSmtpHost(compid, strBrand, strLocId));

            NetworkCredential basiccr = new NetworkCredential(From.ToString(), Paswd.ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = basiccr;

            smtp.EnableSsl = IsSSLEnabled(compid, strBrand, strLocId);
            smtp.Port = GetSystemMailPort(compid, strBrand, strLocId);

            try
            {
                smtp.Send(message);
                return true;
            }
            catch (Exception e)
            {
                ExportToFile("Error Is:" + e.ToString(), "C:\\PegasusSQL\\MailError.txt", FileMode.Append);
                return false;
            }

            //end using gmail

            //MailMessage objMailMsg = new MailMessage(GetSystemMailId(), strTo);

            //objMailMsg.BodyEncoding = Encoding.UTF8;
            //objMailMsg.Subject = strSubject;
            //objMailMsg.Body = strMsg;

            //objMailMsg.Priority = System.Net.Mail.MailPriority.High;
            //objMailMsg.IsBodyHtml = true;

            ////prepare to send mail via SMTP transport
            //SmtpClient objSMTPClient = newp SmtpClient();

            //objSMTPClient.Timeout = 1000000;
            //objSMTPClient.Send(objMailMsg);
            //return true;
        }
        catch (Exception ex)
        {
            // Write in File
            WriteSettingsFile(GetSystemMailId(compid, strBrand, strLocId), strTo, strMsg, GetSystemMailSmtpHost(compid, strBrand, strLocId), getSystemMailPwd(compid, strBrand, strLocId), GetSystemMailPort(compid, strBrand, strLocId), IsSSLEnabled(compid, strBrand, strLocId));
            //return false;
        }
        return true;
    }

    public bool SendApprovalMail(string strTo, string From, string Paswd, string strSubject, string strMsg, string compid, string Attachment, ref SqlTransaction trns, string strAttachpath, string strBrandId, string strLocid)
    {
        //return true;
        try
        {
            //using gmail
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            if (Attachment != "")
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(strAttachpath);
                message.Attachments.Add(attachment);
            }

            message.To.Add(strTo);
            message.Subject = strSubject;
            // message.From = new System.Net.Mail.MailAddress(GetSystemMailId(compid));
            message.From = new System.Net.Mail.MailAddress(From);
            message.IsBodyHtml = true;
            message.Body = strMsg;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(GetSystemMailSmtpHost(compid, ref trns, strBrandId, strLocid));

            NetworkCredential basiccr = new NetworkCredential(From.ToString(), Paswd.ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = basiccr;

            smtp.EnableSsl = IsSSLEnabled(compid, ref trns, strBrandId, strLocid);
            smtp.Port = GetSystemMailPort(compid, ref trns, strBrandId, strLocid);

            try
            {
                smtp.Send(message);
                return true;
            }
            catch (Exception e)
            {
                ExportToFile("Error Is:" + e.ToString(), "C:\\PegasusSQL\\MailError.txt", FileMode.Append);
                //return false;
            }

            //end using gmail

            //MailMessage objMailMsg = new MailMessage(GetSystemMailId(), strTo);

            //objMailMsg.BodyEncoding = Encoding.UTF8;
            //objMailMsg.Subject = strSubject;
            //objMailMsg.Body = strMsg;

            //objMailMsg.Priority = System.Net.Mail.MailPriority.High;
            //objMailMsg.IsBodyHtml = true;

            ////prepare to send mail via SMTP transport
            //SmtpClient objSMTPClient = newp SmtpClient();

            //objSMTPClient.Timeout = 1000000;
            //objSMTPClient.Send(objMailMsg);
            //return true;
        }
        catch (Exception ex)
        {
            // Write in File
            WriteSettingsFile(GetSystemMailId(compid, ref trns, strBrandId, strLocid), strTo, strMsg, GetSystemMailSmtpHost(compid, ref trns, strBrandId, strLocid), getSystemMailPwd(compid, ref trns, strBrandId, strLocid), GetSystemMailPort(compid, ref trns, strBrandId, strLocid), IsSSLEnabled(compid, ref trns, strBrandId, strLocid));
            //return false;
        }
        return true;
    }
    public bool SendApprovalMail(string strTo, string From, string Paswd, string strSubject, string strMsg, string compid, string Attachment, string SmtpHost, bool SSLEnabled, string SystemMailPort, string strAttachpath)
    {
        try
        {
            //using gmail
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

            if (Attachment != "")
            {
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(strAttachpath);
                message.Attachments.Add(attachment);
            }
            message.To.Add(strTo);
            message.Subject = strSubject;
            // message.From = new System.Net.Mail.MailAddress(GetSystemMailId(compid));
            message.From = new System.Net.Mail.MailAddress(From);
            message.IsBodyHtml = true;
            message.Body = strMsg;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(SmtpHost);

            NetworkCredential basiccr = new NetworkCredential(From.ToString(), Paswd.ToString());
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = basiccr;

            smtp.EnableSsl = SSLEnabled;
            smtp.Port = Convert.ToInt32(SystemMailPort);

            try
            {
                smtp.Send(message);
                return true;
            }
            catch (Exception e)
            {
                //ExportToFile("Error Is:" + e.ToString(), "C:\\PegasusSQL\\MailError.txt", FileMode.Append);
                //return false;
            }

            //end using gmail

            //MailMessage objMailMsg = new MailMessage(GetSystemMailId(), strTo);

            //objMailMsg.BodyEncoding = Encoding.UTF8;
            //objMailMsg.Subject = strSubject;
            //objMailMsg.Body = strMsg;

            //objMailMsg.Priority = System.Net.Mail.MailPriority.High;
            //objMailMsg.IsBodyHtml = true;

            ////prepare to send mail via SMTP transport
            //SmtpClient objSMTPClient = newp SmtpClient();

            //objSMTPClient.Timeout = 1000000;
            //objSMTPClient.Send(objMailMsg);
            //return true;
        }
        catch (Exception ex)
        {
            // Write in File
            //WriteSettingsFile(GetSystemMailId(compid), strTo, strMsg, GetSystemMailSmtpHost(compid), getSystemMailPwd(compid), GetSystemMailPort(compid), IsSSLEnabled(compid));
            //return false;
        }
        return true;
    }





    public DataTable GetAccountMasterInfoByCompId(string compid, string Setupid)
    {
        //  varchar(5)
        DataTable dtMasterInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];

        paramList[0] = new PassDataToSql("@SetUpId", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@CompanyId", compid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtMasterInfo = daClass.Reuturn_Datatable_Search("AccountMaster_Select", paramList);
        dtMasterInfo = new DataView(dtMasterInfo, "SetUpId='" + Setupid + "'", "", DataViewRowState.CurrentRows).ToTable();
        return dtMasterInfo;
    }


    public string GetDeptMailId(string deptid, string compid)
    {
        //return objDept.GetMailId(deptid, compid);
        return "";
    }


    private void WriteSettingsFile(string FromMail, string ToMail, string msg, string smtphost, string pwd, int port, bool sslenable)
    {
        //GetSystemMailId(compid), strTo, strMsg, GetSystemMailSmtpHost(compid), getSystemMailPwd(compid), GetSystemMailPort(compid)
        string fileLoc = "C:\\PegasusSQL\\message_" + FromMail + ".txt";
        string mailinfo = string.Empty;
        if (!System.IO.Directory.Exists("C:\\PegasusSQL/"))
        {
            System.IO.Directory.CreateDirectory("C:\\PegasusSQL//");
        }

        if (!File.Exists(fileLoc))
        {

            {//create mail header  it will comma sepreted line sequence,it will be  -: from mail,to mail,smtphost,pwd,port

                mailinfo = FromMail + "," + ToMail + "," + smtphost + "," + pwd + "," + port + "," + sslenable.ToString();

            }
        }

        FileStream fs = new FileStream(fileLoc, FileMode.Append, FileAccess.Write);


        using (StreamWriter sw = new StreamWriter(fs))
        {
            // sw.Write("Mail Did Not Send : " + DateTime.Now.ToString());
            sw.WriteLine(mailinfo);

            sw.Write(msg);

        }

        fs.Dispose();
    }
}

