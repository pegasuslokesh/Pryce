using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net;
using PegasusDataAccess;

public partial class WebUserControl_EmailConfiguration : System.Web.UI.UserControl
{
    Common cmn = null;
    DataAccessClass objda = null;
    UserDetail objUserDetail = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmn = new Common(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objUserDetail = new UserDetail(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            try
            {
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../EmailSystem/Mailbox.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());

                FillUserList(clsPagePermission.bViewAllUserRecord);
                ResetEmail();
            }
            catch
            {

            }
        }
    }




    public void FillUserList(bool isviewalluserpemission)
    {
        DataTable dt = new DataTable();
        if (isviewalluserpemission)
        {
            dt = objda.return_DataTable("select distinct set_employeemaster.emp_name+'/'+set_employeemaster.emp_code as UserName,set_employeemaster.emp_id  from   set_employeemaster inner join  set_usermaster US on   set_employeemaster.emp_id = US.emp_id    where US.isactive='True' and set_employeemaster.isactive='True' and set_employeemaster.field2='False' and set_employeemaster.Location_id='" + Session["LocId"].ToString() + "' ");
        }
        else
        {
            dt = objda.return_DataTable("select distinct set_employeemaster.emp_name+'/'+ set_employeemaster.emp_code as UserName,set_employeemaster.emp_id  from   set_employeemaster inner join  set_usermaster US on   set_employeemaster.emp_id = US.emp_id    where US.isactive='True' and set_employeemaster.isactive='True' and set_employeemaster.field2='False' and set_employeemaster.emp_id=" + Session["EmpId"].ToString() + " ");
        }
        ddluserList.DataSource = dt;
        ddluserList.DataTextField = "UserName";
        ddluserList.DataValueField = "emp_id";
        ddluserList.DataBind();
        if (isviewalluserpemission)
            ddluserList.Items.Insert(0, new ListItem("--All User--", "0"));
        if (Session["EmpId"].ToString() != "0")
        {
            ddluserList.SelectedValue = Session["EmpId"].ToString();
        }

    }


    protected void ResetEmail()
    {
        string strsql = string.Empty;
        if (ddluserList.Visible)
        {
            if (ddluserList.SelectedValue.Trim() == "0")
            {
                strsql = "select set_employeemaster.Emp_Name,Set_UserDetail.*,Ems.LastSynchdate From Set_UserDetail left join (select Field1 as Email,max(createddate) as LastSynchdate from [" + Session["CloudDB_ES"].ToString() + "].dbo.ES_MailInboxHeader group by Field1)Ems on Set_UserDetail.Email=ems.Email inner join set_usermaster on Set_UserDetail.user_id =set_usermaster.user_id inner join set_employeemaster on set_usermaster.emp_id =set_employeemaster.emp_id where (case when " + ddluserList.SelectedValue + "='0' then '0' else set_employeemaster.emp_id end)=" + ddluserList.SelectedValue + " and Set_UserDetail.IsActive='True'  and Set_UserDetail.Company_Id=" + Session["CompId"].ToString() + " and set_employeemaster.Location_Id='" + Session["LocId"].ToString() + "' Order By set_employeemaster.Emp_Name";
            }
            else
            {
                strsql = "select set_employeemaster.Emp_Name,Set_UserDetail.*,Ems.LastSynchdate From Set_UserDetail left join (select Field1 as Email,max(createddate) as LastSynchdate from [" + Session["CloudDB_ES"].ToString() + "].dbo.ES_MailInboxHeader group by Field1)Ems on Set_UserDetail.Email=ems.Email inner join set_usermaster on Set_UserDetail.user_id =set_usermaster.user_id inner join set_employeemaster on set_usermaster.emp_id =set_employeemaster.emp_id where (case when " + ddluserList.SelectedValue + "='0' then '0' else set_employeemaster.emp_id end)=" + ddluserList.SelectedValue + " and Set_UserDetail.IsActive='True'  and Set_UserDetail.Company_Id=" + Session["CompId"].ToString() + " Order By set_employeemaster.Emp_Name";
            }
        }
        else
        {
            strsql = "select set_employeemaster.Emp_Name,Set_UserDetail.*,Ems.LastSynchdate From Set_UserDetail left join (select Field1 as Email,max(createddate) as LastSynchdate from [" + Session["CloudDB_ES"].ToString() + "].dbo.ES_MailInboxHeader group by Field1)Ems on Set_UserDetail.Email=ems.Email inner join set_usermaster on Set_UserDetail.user_id =set_usermaster.user_id inner join set_employeemaster on set_usermaster.emp_id =set_employeemaster.emp_id where (case when " + hdnUserId.Value + "='0' then '0' else set_employeemaster.emp_id end)=" + hdnEmpId.Value + " and Set_UserDetail.IsActive='True' and Set_UserDetail.Company_Id=" + Session["CompId"].ToString() + " Order By set_employeemaster.Emp_Name";
        }
        txtPasswordEmail.Attributes.Add("Value", "");
        txtEmail.Text = "";
        txtEmailSignature.Content = "";
        txtPop3.Text = "";
        txtpopport.Text = "";
        ViewState["Trans_Id"] = null;
        txtPort.Text = "";
        txtSMTP.Text = "";
        chkEnableSSL.Checked = false;
        chkDefault.Checked = false;
        gvEmailPassuser.DataSource = objda.return_DataTable(strsql);
        gvEmailPassuser.DataBind();
        lbltotalrecord.Text = "Total Record : " + gvEmailPassuser.Rows.Count.ToString();
    }

    protected void IbtnDeleteEmail_Command(object sender, CommandEventArgs e)
    {
        objUserDetail.delete(e.CommandArgument.ToString(), Session["CompId"].ToString());
        ResetEmail();

    }

    public void setUserID(string strEmpId, string strUserId, bool Val)
    {
        hdnUserId.Value = strUserId;
        hdnEmpId.Value = strEmpId;
        ddluserList.Visible = Val;
        ResetEmail();
    }


    protected void IbtnEditEmail_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objUserDetail.GetbyTransId(e.CommandArgument.ToString(), Session["CompId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ViewState["Trans_Id"] = dt.Rows[0]["Trans_Id"].ToString();
            ViewState["User_Id"] = dt.Rows[0]["User_Id"].ToString();
            txtEmail.Text = dt.Rows[0]["Email"].ToString();

            txtPasswordEmail.Attributes.Add("Value", dt.Rows[0]["Password"].ToString());
            txtSMTP.Text = dt.Rows[0]["Field1"].ToString();
            txtPort.Text = dt.Rows[0]["Field2"].ToString();
            txtPop3.Text = dt.Rows[0]["Field3"].ToString();
            txtpopport.Text = dt.Rows[0]["Field4"].ToString();
            chkEnableSSL.Checked = Convert.ToBoolean(dt.Rows[0]["Field5"].ToString());
            txtEmailSignature.Content = dt.Rows[0]["Signature"].ToString();
            chkDefault.Checked = Convert.ToBoolean(dt.Rows[0]["IsDefault"].ToString());
        }
    }

    protected void ImgLogoAdd_Click(object sender, EventArgs e)
    {

        if (FileUploadImage.HasFile == false)
        {
            DisplayMessage("Upload The File");
            FileUploadImage.Focus();
            return;
        }

        string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Signature/" + Session["UserId"].ToString());
        if (!Directory.Exists(path))
        {
            CheckDirectory(path);
        }
        string filepath = "../CompanyResource/" + Session["CompId"].ToString() + "/Signature/" + Session["UserId"].ToString() + "/" + Guid.NewGuid() + FileUploadImage.FileName;
        FileUploadImage.SaveAs(Server.MapPath(filepath));

        txtEmailSignature.Content = txtEmailSignature.Content + "<img src='" + filepath + "' />";

    }

    public void CheckDirectory(string path)
    {
        if (path != "")
        {
            Directory.CreateDirectory(path);
        }
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "');", true);
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

    protected void btnSaveSMSEmail_Click(object sender, EventArgs e)
    {
        if (ddluserList.SelectedValue.Trim() == "0" && hdnUserId.Value.Trim() == "")
        {
            DisplayMessage("Select User in user list");
            ddluserList.Focus();
            return;
        }

        if (txtEmail.Text == "")
        {
            DisplayMessage("Enter EmailId");
            txtEmail.Focus();
            return;
        }
        if (txtPasswordEmail.Text == "")
        {
            DisplayMessage("Enter Password");
            txtPasswordEmail.Focus();
            return;
        }
        else
        {
            txtPasswordEmail.Attributes.Add("Value", txtPasswordEmail.Text);

        }
        if (txtSMTP.Text == "")
        {
            DisplayMessage("Enter (SMTP)Outgoing Mail Server");
            txtSMTP.Focus();
            return;
        }
        if (txtPort.Text == "")
        {
            DisplayMessage("Enter SMTP Port");
            txtPort.Focus();
            return;
        }
        if (txtPop3.Text == "")
        {
            DisplayMessage("Enter (POP3) Incoming Mail Server");
            txtPop3.Focus();
            return;
        }
        if (txtpopport.Text == "")
        {
            DisplayMessage("Enter POP3 Port");
            txtpopport.Focus();
            return;
        }
        if (txtpopport.Text == "")
        {
            DisplayMessage("Enter POP3 Port");
            txtpopport.Focus();
            return;
        }



        string isdefault = false.ToString();
        if (chkDefault.Checked)
        {
            isdefault = true.ToString();
        }
        if (ViewState["Trans_Id"] == null)
        {
            string strUserId = string.Empty;
            string strEmpId = string.Empty;

            if (hdnUserId.Value == "")
            {
                strUserId = ddluserList.SelectedItem.Text.Split('/')[1];
                strEmpId = ddluserList.SelectedValue;
            }
            else
            {
                strUserId = hdnUserId.Value;
                strEmpId = hdnEmpId.Value;
            }

            objUserDetail.insert(Session["CompId"].ToString(), strUserId, txtEmail.Text, txtPasswordEmail.Text.Trim(), txtEmailSignature.Content.ToString(), isdefault.ToString(), txtSMTP.Text.Trim(), txtPort.Text.Trim(), txtPop3.Text.Trim(), txtpopport.Text.Trim(), chkEnableSSL.Checked.ToString(), strEmpId, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            objUserDetail.Update(ViewState["Trans_Id"].ToString(), Session["CompId"].ToString(), ViewState["User_Id"].ToString(), txtEmail.Text, txtPasswordEmail.Text.Trim(), txtEmailSignature.Content.ToString(), isdefault.ToString(), txtSMTP.Text.Trim(), txtPort.Text.Trim(), txtPop3.Text.Trim(), txtpopport.Text.Trim(), chkEnableSSL.Checked.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }

        ResetEmail();
    }

    protected void btnCancelSMSEmail_Click(object sender, EventArgs e)
    {
        ResetEmail();
    }
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FileUploadImage.HasFile)
        {
            string ext = FileUploadImage.FileName.Substring(FileUploadImage.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Signature/" + Session["UserId"].ToString());
                if (!Directory.Exists(path))
                {
                    CheckDirectory(path);
                }
                string filepath = "../CompanyResource/" + Session["CompId"].ToString() + "/Signature/" + Session["UserId"].ToString() + "/" + Guid.NewGuid() + FileUploadImage.FileName;
                FileUploadImage.SaveAs(Server.MapPath(filepath));
                txtEmailSignature.Content = txtEmailSignature.Content + "<img src='" + filepath + "' />";
            }
        }
    }

    protected void lnkTestEmail_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        DisplayMessage(SendEmail(((Label)gvrow.FindControl("lblEmailId")).Text.Trim(), "", "", ((Label)gvrow.FindControl("lblEmailId")).Text.Trim(), ((Label)gvrow.FindControl("lblpassword")).Text.Trim(), ((Label)gvrow.FindControl("lblsmtp")).Text.Trim(), ((Label)gvrow.FindControl("lblsmtpPort")).Text.Trim(), ((Label)gvrow.FindControl("lblgvEnableSSL")).Text.Trim(), "This is an e-mail message sent automatically by Pryce application while testing the settings for your account. ", "Pryce Test Message"));
    }


    //public string SendEmail(string strTo, string strCC, string BCC, string strSenderEmail, string strSenderEmailPassword, string SenderHost, string Senderport, string strSenderSSL, string strBody, string strSubject)

    //{


    //    //try
    //    //{
    //    //    MailMessage mail = new MailMessage();
    //    //    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

    //    //    mail.From = new MailAddress("lokeshrawalsd@gmail.com");
    //    //    mail.To.Add("lokeshrawalsd@gmail.com");
    //    //    mail.Subject = "Test Mail";
    //    //    mail.Body = "This is for testing SMTP mail from GMAIL";

    //    //    SmtpServer.Port = 587;
    //    //    SmtpServer.Credentials = new System.Net.NetworkCredential("lokeshrawalsd@gmail.com", "retokxeevoaxtzxh");
    //    //    SmtpServer.EnableSsl = true;

    //    //    SmtpServer.Send(mail);
    //    //    return "Email tests completed successfully";
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    return ex.Message.ToString();
    //    //}




    //    //string MySubject = "Email Subject Line";
    //    //string MyMessageBody = "This is the email body.";
    //    //string RecipientEmail = "lokeshrawalsd@gmail.com";
    //    //string SenderEmail = "lokeshrawalsd@gmail.com";
    //    ////string SenderDisplayName = "Lokesh Rawal";
    //    //string SenderEmailPassword = "123456";

    //    //string HOST = "smtp.gmail.com";
    //    //string PORT = "465";


    //    //System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
    //    //mail.Subject = MySubject;
    //    //mail.Body = MyMessageBody;
    //    //mail.To.Add(RecipientEmail);
    //    //mail.From = new System.Net.Mail.MailAddress(SenderEmail, "Pryce Email Testing");

    //    //SmtpClient SMTP = new SmtpClient(HOST);
    //    //SMTP.EnableSsl = true;
    //    //SMTP.Credentials = new NetworkCredential(SenderEmail.Trim(), SenderEmailPassword.Trim());
    //    //SMTP.DeliveryMethod = SmtpDeliveryMethod.Network;
    //    //SMTP.Port = Convert.ToInt32(PORT);

    //    //try
    //    //{
    //    //    SMTP.Send(mail);
    //    //    return "Email tests completed successfully";
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    return ex.Message.ToString();
    //    //}







    //    bool IsEmail = false;

    //    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();


    //    foreach (string str in strTo.Split(';'))
    //    {
    //        if (str == "")
    //        {
    //            continue;
    //        }
    //        message.To.Add(str);
    //    }
    //    foreach (string str in strCC.Split(';'))
    //    {
    //        if (str == "")
    //        {
    //            continue;
    //        }
    //        message.CC.Add(str);
    //    }
    //    foreach (string str in BCC.Split(';'))
    //    {
    //        if (str == "")
    //        {
    //            continue;
    //        }
    //        message.Bcc.Add(str);
    //    }

    //    message.From = new System.Net.Mail.MailAddress(strSenderEmail, "Pryce Email Testing");
    //    message.Subject = strSubject;
    //    message.IsBodyHtml = true;
    //    message.Body = strBody;
    //    SmtpClient smtp = new SmtpClient(SenderHost);
    //    NetworkCredential basiccr = new NetworkCredential(strSenderEmail, strSenderEmailPassword);
    //    smtp.UseDefaultCredentials = false;
    //    smtp.Credentials = basiccr;
    //    smtp.Port = Convert.ToInt32(Senderport);
    //    smtp.EnableSsl = Convert.ToBoolean(strSenderSSL);
    //    try
    //    {
    //        smtp.Send(message);
    //        return "Email tests completed successfully";
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message.ToString();

    //    }

    //}


    public string SendEmail(string strTo, string strCC, string BCC, string strSenderEmail, string strSenderEmailPassword, string SenderHost, string Senderport, string strSenderSSL, string strBody, string strSubject)
    {

        try
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(strSenderEmail);
            // message.To.Add(new MailAddress(strTo));
            foreach (string str in strTo.Split(';'))
            {
                if (str == "")
                {
                    continue;
                }
                message.To.Add(new MailAddress(strTo));
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
            message.Subject = strSubject;
            message.Body = strBody;
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient(SenderHost, Convert.ToInt32(Senderport)); // Hostinger's SMTP server and port
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(strSenderEmail, strSenderEmailPassword);
            smtp.EnableSsl = true; // Hostinger's SMTP server requires SSL/TLS

            smtp.Send(message);
            return "Email sent successfully!";
        }
        catch (Exception ex)
        {
            return "Error sending email: " + ex.Message;
        }
    }

    protected void ddluserList_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetEmail();
    }
}