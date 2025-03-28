using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class EmailSystem_MailInfo : BasePage
{
    ES_Attachment ObjAtt = null;
    ES_MailInboxHeader ObjMailBoxHeader = null;
    ES_SendMailHeader objSendMail = null;
    PegasusPOP3.PegasusEmailMessage POPMessage = new PegasusPOP3.PegasusEmailMessage();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjAtt = new ES_Attachment(HttpContext.Current.Session["DBConnection_ES"].ToString());
        ObjMailBoxHeader = new ES_MailInboxHeader(HttpContext.Current.Session["DBConnection_ES"].ToString());
        objSendMail = new ES_SendMailHeader(HttpContext.Current.Session["DBConnection_ES"].ToString());


        if (!IsPostBack)
        {
            if (Request.QueryString["MailId"] != null || Request.QueryString["RefId"] != null)
            {

                ShowMail(Request.QueryString["MailId"].ToString(), Request.QueryString["RefId"].ToString());

            }
        }
    }


    public void FillDownload(string RefType, string RefId)
    {
        string FilePathLink = string.Empty;
        DataTable dt = ObjAtt.Get_Attachment(Session["CompId"].ToString(), "0", "0", RefType, RefId);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["Field1"].ToString() != "")
            {
                POPMessage.CreateFile(dt.Rows[i]["Content_Disposition"].ToString(), dt.Rows[i]["Content_Transfer"].ToString(), dt.Rows[i]["File_Data"].ToString(), RefId + "-" + dt.Rows[i]["File_Name"].ToString());
                Editor1.Content = Server.HtmlDecode(Editor1.Content.Replace("cid:" + dt.Rows[i]["Field1"].ToString(), "../Atteched/Temp/" + RefId + "-" + dt.Rows[i]["File_Name"].ToString()));
                FilePathLink = FilePathLink + "../Atteched/Temp/" + RefId + "-" + dt.Rows[i]["File_Name"].ToString() + ",";
            }
        }

        Session["FilePathLink"] = FilePathLink;
        gvDownload.DataSource = new DataView(dt, "Field1=''", "", DataViewRowState.CurrentRows).ToTable();
        gvDownload.DataBind();
    }
    public void ShowMail(string MailId, string RefId)
    {

        DataTable dt = new DataTable();

        if (RefId.ToString() == "1")
        {
            dt = ObjMailBoxHeader.Get_MailByTransId(Session["CompId"].ToString(), "0", MailId.ToString(), Session["EmpId"].ToString());
            string StrRow = string.Empty;

            if (dt.Rows[0]["Cc"].ToString() != "")
            {
                StrRow = "<tr><td>Cc</td><td>:</td><td align='Left'> " + dt.Rows[0]["Cc"].ToString() + "</td></tr>";
            }
            if (dt.Rows[0]["Subject"].ToString() != "")
            {
                StrRow += "<tr><td>Subject</td><td>:</td><td>" + dt.Rows[0]["Subject"].ToString() + "</td></tr>";
            }
            else
            {
                StrRow += "<tr><td>Subject</td><td>:</td><td>No Subject</td></tr>";
            }
            litheader.Text = "<table><tr><td>Date</td><td>:</td><td align='Left'>" + dt.Rows[0]["Date"].ToString() + "</td></tr><tr><td>From</td><td>:</td> <td align='Left'>" + dt.Rows[0]["From"].ToString().Replace("<", " ").Replace(">", " ") + "</td></tr><tr><td>To</td><td>:</td><td align='Left'>" + dt.Rows[0]["To"].ToString() + "</td></tr>" + StrRow + "</table>";

            Editor1.Content = Server.HtmlDecode(dt.Rows[0]["Body"].ToString());

            FillDownload("R", dt.Rows[0]["Trans_Id"].ToString());
            dt.Rows[0]["Body"] = Server.HtmlDecode(Editor1.Content.ToString());
            if (dt.Rows[0]["Field6"].ToString() != "False")
            {
                ObjMailBoxHeader.MailInbox_UpdateReadStatus(MailId.ToString(), Session["CompId"].ToString(), Session["EmpId"].ToString(), "False");
            }
        }
        else
        {
            btnReply.Visible = false;
            btnReplyAll.Visible = false;
            dt = objSendMail.Get_MailbyTransId(Session["CompId"].ToString(), "0", MailId, Session["EmpId"].ToString());
            string StrRow = string.Empty;


            if (dt.Rows[0]["Cc"].ToString() != "")
            {
                StrRow = "<tr><td>Cc</td><td>:</td><td align='Left'> " + dt.Rows[0]["Cc"].ToString() + "</td></tr>";
            }
            if (dt.Rows[0]["BCc"].ToString() != "")
            {
                StrRow += "<tr><td>BCc</td><td>:</td><td align='Left'> " + dt.Rows[0]["BCc"].ToString() + "</td></tr>";
            }
            if (dt.Rows[0]["Subject"].ToString() != "")
            {
                StrRow += "<tr><td>Subject</td><td>:</td><td>" + dt.Rows[0]["Subject"].ToString() + "</td></tr>";
            }
            else
            {
                StrRow += "<tr><td>Subject</td><td>:</td><td>No Subject</td></tr>";
            }
            litheader.Text = "<table><tr><td>Date</td><td>:</td><td align='Left'>" + dt.Rows[0]["ModifiedDate"].ToString() + "</td></tr><tr><td>From</td><td>:</td><td align='Left'>" + dt.Rows[0]["Field1"].ToString() + "</td></tr><tr><td>To</td><td>:</td><td align='Left'>" + dt.Rows[0]["To"].ToString() + "</td></tr>" + StrRow.ToString() + "</table>";
            Editor1.Content = Server.HtmlDecode(dt.Rows[0]["Body"].ToString());

            FillDownload("S", dt.Rows[0]["Trans_Id"].ToString());
            if (dt.Rows[0]["Field6"].ToString() != "False")
                objSendMail.SendMail_UpdateReadStatus(MailId.ToString(), Session["CompId"].ToString(), Session["EmpId"].ToString(), "False");


        }

        Session["DtInbox"] = dt;

    }

    protected void lnlFileName_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = ObjAtt.Get_Attachment(e.CommandArgument.ToString(), Session["CompId"].ToString(), "0", "0");
        POPMessage.Download(dt.Rows[0]["Content_Disposition"].ToString(), dt.Rows[0]["Content_Transfer"].ToString(), dt.Rows[0]["File_Data"].ToString(), dt.Rows[0]["File_Name"].ToString());
    }

    protected void btnReply_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?id=" + Request.QueryString["MailId"].ToString() + "&&RT=RE&&Page=" + Request.QueryString["RefId"].ToString() + "','','height=650,width=900,scrollbars=Yes')", true);



    }
    protected void btnReplyAll_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?id=" + Request.QueryString["MailId"].ToString() + "&&RT=REA&&Page=" + Request.QueryString["RefId"].ToString() + "','','height=650,width=900,scrollbars=Yes')", true);


    }
    protected void btnForward_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx?id=" + Request.QueryString["MailId"].ToString() + "&&FW=0&&Page=" + Request.QueryString["RefId"].ToString() + "','','height=650,width=900,scrollbars=Yes')", true);


    }

}
