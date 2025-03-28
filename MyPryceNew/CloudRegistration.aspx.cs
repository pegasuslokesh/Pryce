using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Configuration;
using System.IO;

public partial class CloudRegistration : System.Web.UI.Page
{
    MasterDataAccess Objda = null;
    static Random random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MasterDataAccess Objda = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
            Fillcountry();
        }

    }

    public void Fillcountry()
    {

        DataTable dt = new DataTable();
        dt = Objda.return_DataTable("select country_name, country_code from pr_countrymaster order by country_name");
        ddlCountry.DataSource = dt;
        ddlCountry.DataTextField = "country_name";
        ddlCountry.DataValueField = "country_code";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, "--Select Country--");

    }









    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCountry.SelectedIndex > 0)
        {
            txtCountryCode.Text = "+" + ddlCountry.SelectedValue;
        }
        else
        {
            txtCountryCode.Text = "";
        }
    }


    private bool IsValidEmailId(string InputEmail)
    {
        //Regex To validate Email Address
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(InputEmail);
        if (match.Success)
            return true;
        else
            return false;
    }


    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string strCompany = txtCompanyName.Text.Trim();
        string strContactPerson = txtContactPerson.Text.Trim();
        string strCountry = ddlCountry.SelectedItem.Text;
        string strMobileNo = txtCountryCode.Text + "-" + txtMobileNo.Text;
        string strEmail = string.Empty;
        DataTable dtEmail = Objda.return_DataTable("SELECT [Email] ,[Password] ,[Email_Smtp] ,[Email_Smtp_Port] ,[EnableSSL] ,[TO_Email] ,[CC_Email] ,[BCC_Email] FROM [pr_EmailConfiguration]");


      

        if (!IsValidEmailId(txtEmailId.Text.Trim()))
        {
            DisplayMessage("Invalid email , Please try again");
            txtEmailId.Focus();
            return;
        }


        if (Objda.return_DataTable("select _id from pr_company where email='" + txtEmailId.Text.Trim() + "'").Rows.Count > 0)
        {
            DisplayMessage("There is already a registration with that email!");
            txtEmailId.Focus();
            return;
        }
        //generating password randomly
        string strPassword =Common.Encrypt((Convert.ToString(random.Next(10, 20000))));

        strEmail = txtEmailId.Text.Trim();

        string strNewRegistration = string.Empty;
        string strCompanyRegistration = String.Empty;

        try
        {
            strCompanyRegistration = Objda.return_DataTable("select top 1 Company_code from pr_company order by _id desc").Rows[0][0].ToString();
        }
        catch
        {

        }
        if (strCompanyRegistration == "")
        {
            strNewRegistration = "PR-1";
        }
        else
        {
            strNewRegistration = "PR-" + (Convert.ToInt32(strCompanyRegistration.Split('-')[1].ToString()) + 1).ToString();
        }

        Objda.execute_Command("INSERT INTO [PryceMaster].[dbo].[pr_company] ([company_code] ,[company_name] ,[contact_person] ,[email] ,[country_code] ,[mobile] ,[registration_date] ,[expiry_date],[User_Password]) VALUES ('" + strNewRegistration + "' ,'" + txtCompanyName.Text.Trim() + "' , '" + txtContactPerson.Text.Trim() + "','" + txtEmailId.Text.Trim() + "' , '" + txtCountryCode.Text.Trim() + "','" + txtMobileNo.Text.Trim() + "' ,'" + DateTime.Now.ToString() + "' ,'" + DateTime.Now.AddDays(30).ToString() + "','"+strPassword+"')");

        DisplayMessage("Registered successfully, check email for login details");

        //for customer
        string strbody = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4>Hi,</h4> <hr /> Thanks for registration. <br /><br /> Your Registration Number Is " + strNewRegistration + " , Please keep your registration number for login in demo application.<br /><br /><a href="+ ConfigurationManager.AppSettings["CloudLoginURL"].ToString() + ">Click here to login</a> <br /><br />Registration Code:" + strNewRegistration + "<br />UserName:Admin<br />Password:" + Common.Decrypt(strPassword)+ "<br /><h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3>  <h2> Pryce Cloud Solution</h2> </div> </body> </html>";

        ThreadStart ts = delegate ()
        {
            SendEmail(strEmail, "", "", dtEmail.Rows[0]["Email"].ToString().Trim(), dtEmail.Rows[0]["Password"].ToString().Trim(), dtEmail.Rows[0]["Email_Smtp"].ToString().Trim(), dtEmail.Rows[0]["Email_Smtp_Port"].ToString().Trim(), strbody, "Registration Successful");
            strbody = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Hi,</h4> <hr /> Find below the new registration  <br /><br /> Company Name : " + strCompany + "<br /> Contact Person : " + strContactPerson + "<br /> Country : " + strCountry + "<br /> Contact No. :" + strMobileNo + "<br />Email-Id : "+strEmail+ "<br /><h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h2> Pryce Cloud Solution<h2></div> </body> </html>";
            SendEmail(dtEmail.Rows[0]["TO_Email"].ToString().Trim(), dtEmail.Rows[0]["CC_Email"].ToString().Trim(), dtEmail.Rows[0]["BCC_Email"].ToString().Trim(), dtEmail.Rows[0]["Email"].ToString().Trim(), dtEmail.Rows[0]["Password"].ToString().Trim(), dtEmail.Rows[0]["Email_Smtp"].ToString().Trim(), dtEmail.Rows[0]["Email_Smtp_Port"].ToString().Trim(), strbody, "New Registration");
        };

        // The thread.
        Thread t = new Thread(ts);

        // Run the thread.
        t.Start();

        txtCompanyName.Text = "";
        txtEmailId.Text = "";
        txtContactPerson.Text = "";
        txtCountryCode.Text = "";
        txtEmailId.Text = "";
        txtMobileNo.Text = "";
        ddlCountry.SelectedIndex = 0;
        txtCompanyName.Focus();
    }



    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }


    public void SendEmail(string strTo, string strCC, string BCC, string strSenderEmail, string strSenderEmailPassword, string SenderHost, string Senderport, string strBody, string strSubject)
    {

        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();


        foreach (string str in strTo.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.To.Add(str);
        }
        foreach (string str in strCC.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.CC.Add(str);
        }
        foreach (string str in BCC.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.Bcc.Add(str);
        }

        message.From = new System.Net.Mail.MailAddress(strSenderEmail, "Pryce Cloud Solution");
        message.Subject = strSubject;
        message.IsBodyHtml = true;
        message.Body = strBody;
        SmtpClient smtp = new SmtpClient(SenderHost);
        NetworkCredential basiccr = new NetworkCredential(strSenderEmail, strSenderEmailPassword);
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = basiccr;
        smtp.Port = Convert.ToInt32(Senderport);
        try
        {
            smtp.Send(message);

        }
        catch (Exception ex)
        {
            ExportToFile("Cloud Email Error Is:" + ex.Message.ToString(), "C:\\PegasusSQL\\MailError.txt", FileMode.Append);
           
        }

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





}