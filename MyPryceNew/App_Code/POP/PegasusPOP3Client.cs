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
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.IO;


/// <summary>
/// Summary description for PegasusPOP3Client
/// </summary>
/// 
namespace PegasusPOP3
{
    public class PegasusPOP3Client
    {
        TcpClient TCpClient;
        Stream POP3Stream;
        StreamReader POP3SR;
        string strDataIn, strNumMains;

        int intNoEmails;
        public PegasusPOP3Client(string strServer, int StrPort, string strUserName, string strPassword)
        {
            try
            {
                TCpClient = new TcpClient();
                TCpClient.Connect(strServer, StrPort); ;
               
                //create stream into the ip
                POP3Stream = TCpClient.GetStream();
                POP3SR = new StreamReader(POP3Stream);

                //Make sure we get the ok back from the server
                if (!WaitFor("+OK"))
                {
                    POPErrors("Unexpected Response from mail server when connecting" + "\r\n" + strDataIn);
                }
                //  'send the email down 
                SendData("USER " + strUserName);
                if (!WaitFor("+OK"))
                {
                    POPErrors("Unexpected Response from mail server when sending user" + "\r\n" + strDataIn);
                }
                SendData("PASS " + strPassword);

                if (!WaitFor("+OK"))
                {
                    POPErrors("Unexpected Response from mail server when sending Password" + "\r\n" + strDataIn);
                }
            }
            catch
            {

            }

        }

        public void POPErrors(string s)
        {

        }

        public void SendData(string strCommand)
        {

            byte[] outBuff = null;
            outBuff = ConvertStringToByteArray(strCommand + "\r\n");
            POP3Stream.Write(outBuff, 0, strCommand.Length + 2);
        }

        public byte[] ConvertStringToByteArray(string Str)
        {
            System.Text.ASCIIEncoding ObjASCIIEncoding = new System.Text.ASCIIEncoding();
            return ObjASCIIEncoding.GetBytes(Str);

        }

        public bool WaitFor(string strTarget)
        {
            strDataIn = POP3SR.ReadLine();
            if (Convert.ToBoolean(string.Compare(strDataIn, strTarget)))
            {

                return true;
            }
            else
            {
                return false;

            }

        }



        public int GetMailStat()
        {


            // 'send the stat command and make sure it returns as expected
            try
            {
                SendData("STAT");

                if (!WaitFor("+OK"))
                {
                    POPErrors("Unexpected Response from mail server when Getting No of Messages" + "\n" + strDataIn);

                }
                else
                {
                    //'split up the response. It should be +OK Num of emails size of emails
                    strNumMains = strDataIn.Split(' ')[1].ToString();


                }

                return Convert.ToInt32(strNumMains);
            }
            catch
            {
                return 0;
            }
        }


        public string GetMailMessage(int intNum)
        {


            string strTemp;
            string strEmailMess = string.Empty;
            try
            {
                //'send the command to the server to return that email back. Command is RETR and the email no ie RETR 1
                SendData("RETR " + intNum.ToString());
                // 'make sure we get a proper response back
                if (!WaitFor("+OK"))
                {
                    POPErrors("Unexpected Response from mail server getting email body" + "\n" + strDataIn);

                }
                //'Should be ok at this point to read in the tcp stream. We read it in until the end of the email
                //'whitch is signified by a line containing only a fullpoint(chr46)
                strTemp = POP3SR.ReadLine();

                while (strTemp != ".")
                {
                    strEmailMess = strEmailMess + strTemp + "\r\n";
                    strTemp = POP3SR.ReadLine();
                }

                return strEmailMess.ToString();

            }
            catch
            {

                return "No Email was Retrived";

            }

        }
        public string GetMailUniqueId(int intNum)
        {


            string strTemp;
            string strEmailMess = string.Empty;
            try
            {
                //'send the command to the server to return that email back. Command is RETR and the email no ie RETR 1
                SendData("UIDL " + intNum.ToString());
                // 'make sure we get a proper response back
                strTemp = POP3SR.ReadLine();
                
          
                return strTemp.ToString();

            }
            catch
            {

                return "No Email was Retrived";

            }

        }
        public void CloseConn()
        {
            try
            {
                SendData("QUIT");
                POP3SR.Close();
                POP3SR.Close();
            }
            catch
            {

            }
        }

    }
}

