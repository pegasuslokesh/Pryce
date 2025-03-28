using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Threading;


public partial class ServiceManagement_ViewFeedBack : System.Web.UI.Page
{
    Set_CustomerMaster ObjCustomer = null;
    SystemParameter objSysParam = null;
    Sm_TicketFeedback ObjTicketFeedback = null;
    Set_DocNumber ObjDoc = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmpMaster = null;
    SM_Ticket_Master objticketmaster = null;
    SM_TicketEmployee objTicketEmployee = null;
    Common cmn = null;
    DataAccessClass objDa = null;
    SendMailSms ObjSendMailSms = null;
    UserMaster ObjUserMaster = null;
    SM_Ticket_Product objTicketproduct = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjTicketFeedback = new Sm_TicketFeedback(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objticketmaster = new SM_Ticket_Master(Session["DBConnection"].ToString());
        objTicketEmployee = new SM_TicketEmployee(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
        objTicketproduct = new SM_Ticket_Product(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            if (Request.QueryString["TicketId"] != null)
            {
                string sql = "select *,case when SM_Ticket_Master.Field6='True' then (select Name from EMS_ContactMaster where Trans_Id=SM_Ticket_Master.Customer_Name)    when SM_Ticket_Master.Field6='False' then SM_Ticket_Master.Customer_Name end as  customerName ,case when SM_Ticket_Master.Field6='True' then (select EMS_ContactMaster.Field2 from EMS_ContactMaster where EMS_ContactMaster.Trans_Id=SM_Ticket_Master.Customer_Name)    when SM_Ticket_Master.Field6='False' then ' ' end as  ContactNo from SM_Ticket_Master where Trans_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["TicketId"].ToString())) + "";
                DataTable dtTicket = objDa.return_DataTable(sql);
                if (dtTicket.Rows.Count > 0)
                {
                    lblticketno.Text = dtTicket.Rows[0]["Ticket_No"].ToString();
                    lblTickeDate.Text = GetDate(dtTicket.Rows[0]["Ticket_Date"].ToString());
                    lblStatus.Text = dtTicket.Rows[0]["Status"].ToString();
                    lblTaskType.Text = dtTicket.Rows[0]["Task_Type"].ToString();
                    lblCustomerNameValue.Text = dtTicket.Rows[0]["CustomerName"].ToString();
                    lblScheduledate.Text = GetDate(dtTicket.Rows[0]["Schedule_Date"].ToString());
                    lblDescriptionvalue.Text = dtTicket.Rows[0]["Description"].ToString();
                    lblCustomerEmailId.Text = dtTicket.Rows[0]["Email_Id"].ToString();
                    lblCustomerContactNo.Text = dtTicket.Rows[0]["ContactNo"].ToString();
                }


                FillGrid();
                CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            }
            if (Request.QueryString["CompId"] != null)
            {
                pnlcomments.Visible = true;
                GvFeedback.Columns[0].Visible = false;
            }
            else
            {
                pnlcomments.Visible = false;
                GvFeedback.Columns[0].Visible = true;
            }
        }


    }
    public void FillGrid()
    {
        DataTable dt = ObjTicketFeedback.GetAllRecord();

        try
        {
            //TicketId

            dt = new DataView(dt, "Ticket_No=" +Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["TicketId"].ToString())) + " and IsActive='True'", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (Request.QueryString["CompId"] != null)
        {
            dt = new DataView(dt, "Field3='CE' or Field3='C'", "Trans_Id Desc", DataViewRowState.CurrentRows).ToTable();
        }

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvFeedback, dt, "", "");


        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

        Session["dtCInquiry"] = dt;

    }
    protected void GvFeedback_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtCInquiry"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDir"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDir"] = "DESC";
        }

        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtCInquiry"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvFeedback, dt, "", "");


    }
    protected void GvFeedback_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvFeedback.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCInquiry"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvFeedback, dt, "", "");


    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {

        if (ddlFieldName.SelectedIndex == 1)
        {
            if (txtValueDate.Text != "")
            {

                try
                {
                    txtValue.Text = objSysParam.getDateForInput(txtValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDate.Text = "";
                    txtValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDate.Focus();
                txtValue.Text = "";
                return;
            }
        }

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCustomInq = (DataTable)Session["dtCInquiry"];
            DataView view = new DataView(dtCustomInq, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvFeedback, view.ToTable(), "", "");
            Session["dtCInquiry"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {

        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        txtValue.Text = "";
        txtValueDate.Text = "";

    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    protected void OnDownloadCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() == "")
        {
            DisplayMessage("File Not Found");
            return;
        }
        else
        {
            string filepath = "~/" + "ServiceManagement/Feedback/" + e.CommandArgument.ToString() + "/" + e.CommandName.ToString();

            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + e.CommandName.ToString() + "\"");
            Response.TransmitFile(Server.MapPath(filepath));
            Response.End();
        }
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
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedIndex == 1)
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
        else
        {
            txtValueDate.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
        ddlFieldName.Focus();
    }
    protected void btnInquirySave_Click(object sender, EventArgs e)
    {

        if (txtAction.Text == "")
        {
            DisplayMessage("Enter Feedback");
            txtAction.Focus();
            return;
        }
        DateTime dtGeneratedate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        string sql = "INSERT INTO [SM_Ticket_Description]   ([Ticket_No]           ,[Emp_Id]           ,[Date]           ,[Action]           ,[Customer_Id]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES  (" +Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["TicketId"].ToString())) + " ,'0','" + dtGeneratedate.ToString() + "','" + txtAction.Text + "','0','Open',' ','CE',' ',' ','" + false.ToString() + "','" + DateTime.Now.ToString() + "','" + true.ToString() + "','Customer','" + DateTime.Now.ToString() + "','Customer','" + DateTime.Now.ToString() + "')";

        objDa.execute_Command(sql);
        //for send mail

        //code start
        string strAppMailId = string.Empty;
        string strAppPassword = string.Empty;
        string smtpHost = string.Empty;
        bool SSLEnabled = false;
        string SystemMailPort = string.Empty;
        string Email_Display_Name = string.Empty;


        sql = "select Param_Value from Set_ApplicationParameter where Company_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())) + " and Brand_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["BrandId"].ToString())) + " and Location_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["LocId"].ToString())) + " and Param_Name='Support_Email'";
        strAppMailId = objDa.return_DataTable(sql).Rows[0][0].ToString();
        sql = "select Param_Value from Set_ApplicationParameter where Company_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())) + " and Brand_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["BrandId"].ToString())) + " and Location_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["LocId"].ToString())) + " and Param_Name='Support_Email_Password'";
        strAppPassword = objDa.return_DataTable(sql).Rows[0][0].ToString();
        sql = "select Param_Value from Set_ApplicationParameter where Company_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())) + " and Brand_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["BrandId"].ToString())) + " and Location_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["LocId"].ToString())) + " and Param_Name='Support_Email_SMTP'";
        smtpHost = objDa.return_DataTable(sql).Rows[0][0].ToString();
        sql = "select Param_Value from Set_ApplicationParameter where Company_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())) + " and Brand_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["BrandId"].ToString())) + " and Location_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["LocId"].ToString())) + " and Param_Name='Support_Email_EnableSSL'";
        SSLEnabled = Convert.ToBoolean(objDa.return_DataTable(sql).Rows[0][0].ToString());
        sql = "select Param_Value from Set_ApplicationParameter where Company_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())) + " and Brand_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["BrandId"].ToString())) + " and Location_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["LocId"].ToString())) + " and Param_Name='Support_Email_Port'";
        SystemMailPort = objDa.return_DataTable(sql).Rows[0][0].ToString();
        sql = "select Param_Value from Set_ApplicationParameter where Company_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())) + " and Brand_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["BrandId"].ToString())) + " and Location_Id=" + Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["LocId"].ToString())) + " and Param_Name='Support_Display_Text'";
        Email_Display_Name = objDa.return_DataTable(sql).Rows[0][0].ToString();


        string strProductName = string.Empty;
        DataTable dtTicketProduct = objTicketproduct.GetRecordByTicketId(Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["TicketId"].ToString())));
        if (dtTicketProduct.Rows.Count > 0)
        {
            foreach (DataRow dr in dtTicketProduct.Rows)
            {
                if (strProductName == "")
                {
                    strProductName = dr["ProductName"].ToString();
                }
                else
                {
                    strProductName = strProductName + "," + dr["ProductName"].ToString();
                }
            }
        }

        string MailBody = string.Empty;


        MailBody = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title>Pryce Client Support</title></head><body><div style='background:#eee; height:80px; border-bottom:solid 1px #cccccc; margin-bottom:15px;'><div style='float:left;color:#064184; margin-left:10px; line-height:40px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:22px; letter-spacing:1px; margin-top:20px;'>Customer Feedback</div>		</div>                <div>                	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'>" + txtAction.Text + " </p> <h4 style='float:left;color:#064184; margin-left:10px; line-height:20px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:18px; letter-spacing:1px; margin-left:10px;'> Customer Support Team</h4>                </div> <table width='100%' cellpadding='2' cellspacing='3' border='0' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:30px; border:solid 1px #333333;'>	    <tr style='background:#064184; color:#FFFFFF; margin-left:10px; line-height:40px; font-weight:bold; font-size:16px; letter-spacing:1px;'>    	<td style='padding-left:10px;' colspan='2'>Ticket Information</td>    </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket No.</td>        <td style='padding-left:10px;'>" + lblticketno.Text + "</td>    </tr>         <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket Date</td>        <td style='padding-left:10px;'>" + Convert.ToDateTime(lblTickeDate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString()) + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Customer Name</td>        <td style='padding-left:10px;'>" + lblCustomerNameValue.Text + "</td>    </tr>	<tr style='color:#000; background:#eeeeee;'>    	<td style='padding-left:10px;'>Email</td>        <td style=' padding-left:10px;'>" + lblCustomerEmailId.Text + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Contact No.</td>        <td style='padding-left:10px;'>" + lblCustomerContactNo.Text + "</td>    </tr>        <tr style='background:#eeeeee;color:#000;'>    	<td style='padding-left:10px;'>Product</td>        <td style='padding-left:10px;'>" + strProductName + "</td> </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Description</td>        <td style='padding-left:10px;'>" + lblDescriptionvalue.Text + "</td>    </tr></table></body></html>";
    

        //MailMessage += txtAction.Text + "(" + DateTime.Now.ToString() + ") .";
        //MailMessage += "<br /><br /> For check Ticket feedback you can <a href=http://192.168.1.102/pryce/ServiceManagement/ViewFeedBack.aspx?TicketId=@T>ClickHere</a><br />  <br />From<br />" + lblCustomerNameValue.Text;
        //MailMessage = MailMessage.Replace("@T", HttpUtility.UrlEncode(Encrypt(Decrypt(HttpUtility.UrlDecode(Request.QueryString["TicketId"].ToString())))));

        //this code for send the mail from auto log process
        //code start

        //for get cc empployee
        string strCreatedByEmailId = string.Empty;
        string strCC = string.Empty;
        string EmpId = string.Empty;




        DataTable dtTicket = objticketmaster.GetRecordByTransId(Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())), Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["BrandId"].ToString())), Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["LocId"].ToString())), Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["TicketId"].ToString())));

        try
        {
            EmpId = ObjUserMaster.GetUserMasterByUserId(dtTicket.Rows[0]["CreatedBy"].ToString(), Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString()))).Rows[0]["Emp_Id"].ToString();
        }
        catch
        {
            EmpId = "0";
        }


        if (EmpId != "0")
        {
            strCreatedByEmailId = objEmpMaster.GetEmployeeMasterById(Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())), EmpId).Rows[0]["Email_Id"].ToString();
        }

        DataTable dtTicketEmp = objTicketEmployee.GetAllRecord_ByTicketId(Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["TicketId"].ToString())));
        if (dtTicketEmp.Rows.Count > 0)
        {
           

            foreach (DataRow dr in dtTicketEmp.Rows)
            {
                if (objEmpMaster.GetEmployeeMasterById(Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString() != "")
                {
                    if (strCC == "")
                    {
                        strCC = objEmpMaster.GetEmployeeMasterById(Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString();
                    }
                    else
                    {
                        strCC = strCC + ";" + objEmpMaster.GetEmployeeMasterById(Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString();
                    }

                }
            }
        }

        string strsubject = string.Empty;
        strsubject = "Customer Feedback(" + lblticketno.Text + ")";
        DisplayMessage("Feedback Saved");
        try
        {
            //ThreadStart ts = delegate()
            // {
            ObjSendMailSms.SendMail_TicketInfo(strCreatedByEmailId, strCC, "", strsubject, MailBody, Common.Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())), "", strAppMailId, strAppPassword, Email_Display_Name, smtpHost, SystemMailPort,"", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            // };

            //Thread t = new Thread(ts);

            //// Run the thread.
            //t.Start();
        }
        catch
        {
        }
        //sql = "INSERT INTO [Ser_ReportLog]([Emp_Id]           ,[Message]           ,[Status]           ,[Type]           ,[Message_Heading]           ,[Generate_Date]           ,[Delivered_Date]           ,[Exception_Detail]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES   ('0','" + MailMessage + "','Pending','Mail','Report','" + dtGeneratedate.ToString() + "','1/1/1900','1/1/1900','" + strsubject + "','" + strAppMailId + "','" + Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())) + "','" + Decrypt(HttpUtility.UrlDecode(Request.QueryString["BrandId"].ToString())) + "','" + Decrypt(HttpUtility.UrlDecode(Request.QueryString["LocId"].ToString())) + "','" + false.ToString() + "','" + DateTime.Now.ToString() + "','" + true.ToString() + "','Customer','" + DateTime.Now.ToString() + "','Customer','" + DateTime.Now.ToString() + "')";
        //objDa.execute_Command(sql);
        ////code end

        // ObjSendMailSms.SendApprovalMail(strAppMailId.ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Customer Feedback(" + lblticketno.Text + ")", MailMessage.ToString(), Decrypt(HttpUtility.UrlDecode(Request.QueryString["CompId"].ToString())), "", smtpHost, SSLEnabled,SystemMailPort);

        //code end
        FillGrid();
        txtAction.Text = "";

    }
   
}