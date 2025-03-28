using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

/// <summary>
/// Summary description for PegasusEmailMessage
/// </summary>
/// 
namespace PegasusPOP3
{
    public class PegasusEmailMessage
    {
        public string m_MessageSource
        {
            get;
            set;
        }
        public string From
        {
            get;
            set;


        }
        public string To
        {
            get;
            set;

        }
        public string MessageId
        {
            get;
            set;

        }

        public string Cc
        {
            get;
            set;

        }
        public string Subject
        {
            get;
            set;


        }
        public string BodyText
        {
            get;
            set;



        }
        public string BodyHtml
        {
            get;
            set;



        }
        public string Date
        {
            get;
            set;



        }
        public string BodyType
        {
            get;
            set;
        }

        public PegasusEmailMessage()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public void ParseEmail(string strMessage)
        {
            m_MessageSource = strMessage;
            From = ParseHeader("From:").Replace("<", "").Replace(">", "");
            To = ParseHeader("To:").Replace("<", "").Replace(">", "");
            Cc = ParseHeader("Cc:").Replace("<", "").Replace(">", "");
            Subject = ParseHeader("Subject:");
            MessageId = ParseHeader("Message-ID:");

            try
            {
                string[] strDate = ParseHeader("Date:").Split(',');
                Date = strDate[1].ToString().Trim().Substring(0, 20).ToString();
            }
            catch
            {
                if (ParseHeader("Date:").Trim().Length >= 20)
                {
                    Date = ParseHeader("Date:").Trim().Substring(0, 20);
                }
                else
                {
                    Date = ParseHeader("Date:").Trim();

                }
            }
            StrBody();
            AttachFile();
        }

        public string ParseHeader(string strHeader)
        {
            string[] Temp = m_MessageSource.ToString().Split('\n');
            string StrValue = string.Empty;
            bool b = false;
            foreach (string s in Temp)
            {
                if (s.StartsWith(strHeader + " "))
                {
                    b = true;
                }
                if (b)
                {
                    StrValue += s.ToString().Replace(strHeader, "");
                    if (s.ToString().Substring(s.Length - 2, 1).ToString() != ",")
                    {
                        b = false;
                        return StrValue;
                    }
                }
            }
            return StrValue;
        }
        private void StrBody()
        {
            string multipartext = string.Empty;
            string TextPlan = string.Empty;
            string TextHtml = string.Empty;

            multipartext = multipart();
            BodyHtml = GetBodyHtml();
            BodyText = GetBodyText();
        }
        private string GetBodyText()
        {
            string[] Temp = m_MessageSource.ToString().Split('\n');
            bool b = false;
            string StrValue = string.Empty;
            int i = 0;
            foreach (string s in Temp)
            {

                if (s.Contains("Content-Type: text/plain"))
                {
                    b = true;
                    i++;
                }
                if (b)
                {
                    if (s.Contains("Content-Type:"))
                    {
                        if (i >= 2)
                        {
                            b = false;
                        }
                    }
                    else
                    {
                        StrValue += s.ToString() + "\n";
                    }
                }
            }
            return StrValue;


        }
        private string GetBodyHtml()
        {
            string[] Temp = m_MessageSource.ToString().Split('\n');
            bool b = false;
            bool c = false;
            string StrValue = string.Empty;
            int i = 0;

            foreach (string s in Temp)
            {

                if (s.Contains("Content-Type: text/html"))
                {
                    b = true;

                }
                if (b)
                {


                    if (s.Contains("------=_NextPart"))
                    {

                        b = false;
                        return StrValue;

                    }

                    else
                    {
                        if (s.ToString() == "\r")
                        {
                            i = 1;
                        }
                        if (i == 1)
                        {
                            try
                            {
                                if (s.Substring(s.Length - 2, 2).Equals("=\r"))
                                {
                                    StrValue += s.ToString().Replace("=\r", "").Replace("<=", "<").Replace("==", "=").Replace("3D", "") + "\n";
                                }
                                else
                                {
                                    StrValue += s.ToString();

                                }


                            }
                            catch
                            {


                            }
                        }
                    }
                }
            }
            return StrValue;

        }
        private string multipart()
        {
            string[] Temp = m_MessageSource.ToString().Split('\n');
            bool b = false;
            string StrValue = string.Empty;
            foreach (string s in Temp)
            {

                if (s.Contains("Content-Type: multipart/alternative"))
                {
                    b = true;

                }
                if (b)
                {
                    if (s.Contains("Content-Type:"))
                    {
                        if (!s.Contains("Content-Type: multipart/alternative"))
                        {
                            b = false;
                        }
                    }
                    else
                    {
                        StrValue += s.ToString() + "\n";
                    }
                }
            }
            return StrValue;
        }
        public DataTable AttachFile()
        {
            string[] Temp = m_MessageSource.ToString().Split('\n');
            DataTable dt = new DataTable();
            dt.Columns.Add("FileId");
            dt.Columns.Add("ContentDisposition");
            dt.Columns.Add("ContentTransfer");
            dt.Columns.Add("ContantType");
            dt.Columns.Add("FileName");
            dt.Columns.Add("FileData");
            bool b = false; ;
            bool Check = false;
            bool strAllow = false;
            string str = string.Empty;
            string ContantDes = string.Empty;
            string ContantType = string.Empty;
            string ContentTransfer = string.Empty;
            string FileName = string.Empty;

            foreach (string s in Temp)
            {
                if (s.Contains("Content-Type: multipart/mixed;"))
                {
                    b = true;
                }
                if (b)
                {


                    if (s.Contains("Content-Type: "))
                    {
                        ContantType = s.ToString().Replace("Content-Type: ", "").Replace("\r", "");

                    }
                    if (s.Contains("Content-Transfer-Encoding: "))
                    {
                        ContentTransfer = s.ToString().Replace("Content-Transfer-Encoding: ", "").Replace("\r", "");
                    }
                    if (s.Trim().Contains("filename="))
                    {
                        FileName = s.ToString().Trim().Replace("filename=", "").Replace('"', '/');
                        FileName = FileName.Replace("/", "").Trim();
                        FileName = FileName.Replace("\r", "").Replace(";", "");
                    }



                    if (s.Equals("Content-Disposition: attachment;\r"))
                    {
                        ContantDes = "attachment;";
                        Check = true;
                    }

                    if (Check)
                    {

                        if (s.ToString() == "\r")
                        {
                            strAllow = true;
                        }
                        if (strAllow)
                        {



                            try
                            {
                                if (!s.Contains("------=_NextPart"))
                                {
                                    if (ContentTransfer.ToUpper().Equals("BASE64"))
                                    {
                                        if (Checkbinary(s))
                                        {
                                            str += s.ToString();
                                        }
                                    }
                                    else
                                    {
                                        str += s.ToString().Replace("=\r", "").Replace("<=", "<").Replace("==", "=").Replace("3D", "").Replace("< /", "<") + "\n";

                                    }



                                }
                                else
                                {
                                    dt.Rows.Add();
                                    dt.Rows[dt.Rows.Count - 1]["FileId"] = dt.Rows.Count;
                                    dt.Rows[dt.Rows.Count - 1]["ContentDisposition"] = ContantDes;
                                    dt.Rows[dt.Rows.Count - 1]["ContentTransfer"] = ContentTransfer;
                                    dt.Rows[dt.Rows.Count - 1]["ContantType"] = ContantType;
                                    dt.Rows[dt.Rows.Count - 1]["FileName"] = FileName;
                                    dt.Rows[dt.Rows.Count - 1]["FileData"] = str;
                                    str = string.Empty;
                                    ContantDes = string.Empty;
                                    ContentTransfer = string.Empty;
                                    FileName = string.Empty;
                                    Check = false;
                                    strAllow = false;
                                    strAllow = false;
                                }
                            }
                            catch
                            {

                            }
                        }


                    }
                }

            }
            return dt;

        }



        public void Download(string ContentDisposition, string ContentTransferEncoding, string m_data, string fileName)
        {
            // if BASE-64 data ...
            string m_contentDisposition = ContentDisposition;
            string m_contentTransferEncoding = ContentTransferEncoding;
            string FilePath = HttpContext.Current.Server.MapPath("~/Atteched");
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Atteched")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Atteched"));
            }
            byte[] m_binaryData;

            if ((m_contentDisposition.Contains("attachment;")) &&
                (m_contentTransferEncoding.ToUpper()
                .Equals("QUOTED-PRINTABLE")))
            {
                using (StreamWriter sw = File.CreateText(FilePath + @"\" + fileName))
                {

                    sw.Write(FromQuotedPrintable(m_data));
                    sw.Flush();
                    sw.Close();
                }



            }

            else
            {

                if ((m_contentDisposition.Contains("attachment;")) ||
             (m_contentTransferEncoding.ToUpper()
             .Equals("BASE64")))
                {
                    // convert attachment from BASE64 ...
                    m_binaryData =
                          Convert.FromBase64String(m_data.Replace(@"\n", ""));
                    BinaryWriter bw;

                    bw = new BinaryWriter(
                          new FileStream(FilePath + @"\" + fileName, FileMode.CreateNew));



                    bw.Write(m_binaryData);
                    bw.Flush();
                    bw.Close();



                }


            }
            try
            {
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName.ToString() + "\"");

                byte[] data = req.DownloadData(FilePath + @"\" + fileName);
                response.BinaryWrite(data);
                response.End();

                if (!File.Exists(FilePath + @"\" + fileName))
                {
                    File.Delete(FilePath + @"\" + fileName);
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(FilePath + @"\" + fileName))
                {
                    File.Delete(FilePath + @"\" + fileName);
                }
            }



        }

        public void CreateFile(string ContentDisposition, string ContentTransferEncoding, string m_data, string fileName)
        {
            // if BASE-64 data ...
            string m_contentDisposition = ContentDisposition;
            string m_contentTransferEncoding = ContentTransferEncoding;

           
            string FilePath = HttpContext.Current.Server.MapPath("~/Atteched/Temp");
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Atteched/Temp")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Atteched/Temp"));
            }
            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Atteched/Temp/" + fileName)))
            {



                byte[] m_binaryData;

                if ((m_contentDisposition.Contains("attachment;")) &&
                    (m_contentTransferEncoding.ToUpper()
                    .Equals("QUOTED-PRINTABLE")))
                {
                    using (StreamWriter sw = File.CreateText(FilePath + @"\" + fileName))
                    {

                        sw.Write(FromQuotedPrintable(m_data));
                        sw.Flush();
                        sw.Close();
                    }



                }

                else
                {

                    if ((m_contentDisposition.Contains("attachment;")) ||
                 (m_contentTransferEncoding.ToUpper()
                 .Equals("BASE64")))
                    {
                        // convert attachment from BASE64 ...
                        m_binaryData =
                              Convert.FromBase64String(m_data.Replace(@"\n", ""));
                        BinaryWriter bw;

                        bw = new BinaryWriter(
                              new FileStream(FilePath + @"\" + fileName, FileMode.CreateNew));



                        bw.Write(m_binaryData);
                        bw.Flush();
                        bw.Close();



                    }


                }

            }

        }



        private string FromQuotedPrintable(string inString)
        {
            string outputString = null;
            string Str = null;
            string inputString = inString.Replace("=\n", "");
            if (inputString.Length > 3)
            {
                outputString = "";

                for (int x = 0; x < inputString.Length; )
                {
                    string s1 = inputString.Substring(x, 1);

                    if ((s1.Equals("=")) && ((x + 2) < inputString.Length))
                    {
                        string hexString = inputString.Substring(x + 1, 2);

                        // if hexadecimal ...
                        if (Regex.Match(hexString.ToUpper()
                            , @"^[A-F|0-9]+[A-F|0-9]+$").Success)
                        {
                            // convert to string representation ...
                            outputString +=
                                System.Text
                                .Encoding.ASCII.GetString(
                                new byte[] {
									System.Convert.ToByte(hexString,16)});
                            x += 3;
                        }
                        else
                        {
                            outputString += s1;
                            ++x;
                        }
                    }
                    else
                    {
                        outputString += s1;
                        ++x;


                    }

                }
            }
            else
            {
                outputString = inputString;
            }


            return outputString.Replace("\n", "\r\n");
        }


        private bool Checkbinary(string s)
        {
            try
            {
                byte[] m_binaryData =
                         Convert.FromBase64String(s);
                return true;
            }
            catch
            {
                return false;

            }
        }

        //Send Email

        public bool SendMail(string strTo, string strCC, string strBcc, string strSubject, string strMsg, string Attachment)
        {
            try
            {
                //using gmail
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                if (Attachment != "")
                {
                    System.Net.Mail.Attachment attachment;

                    if (File.Exists(HttpContext.Current.Server.MapPath(Attachment)))
                    {
                        attachment = new System.Net.Mail.Attachment(HttpContext.Current.Server.MapPath(Attachment).ToString());
                        message.Attachments.Add(attachment);
                    }

                }
                message.To.Add(strTo);
                message.CC.Add(strCC);
                message.Bcc.Add(strBcc);
                message.Subject = strSubject;
                message.From = new System.Net.Mail.MailAddress("Lokesh@Pegasustech.net");
                message.IsBodyHtml = true;
                message.Body = strMsg;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtpout.secureserver.net");

                smtp.Credentials = new NetworkCredential("Lokesh@Pegasustech.net", "lo@1234");
                smtp.Port = 25;


                smtp.EnableSsl = false;


                try
                {
                    smtp.Send(message);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                    //  ExportToFile("Error Is:" + e.ToString(), "C:\\PegasusSQL\\MailError.txt", FileMode.Append);
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
                //   WriteSettingsFile(GetSystemMailId(compid), strTo, strMsg, GetSystemMailSmtpHost(compid), getSystemMailPwd(compid), GetSystemMailPort(compid), IsSSLEnabled(compid));
                return false;
            }
        }



    }

}
