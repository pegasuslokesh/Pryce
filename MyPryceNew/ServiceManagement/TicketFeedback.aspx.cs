using System;
using System.Web;
using System.Web.UI;
using System.Data;
using PegasusDataAccess;

public partial class ServiceManagement_TicketFeedback : System.Web.UI.Page
{
    Set_CustomerMaster ObjCustomer = null;
    SystemParameter objSysParam = null;
    Sm_TicketFeedback ObjTicketFeedback = null;
    Set_DocNumber ObjDoc = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmpMaster = null;
    SM_Ticket_Master objticketmaster = null;
    SM_TicketEmployee objTicketEmployee = null;
    DataAccessClass objDa = null;
    Common cmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;

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
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            if (Request.QueryString["Ticket_Id"] != null)
            {
                txtticketno_OnTextChanged(null, null);
            }
        }
    }
  
    
    protected void btnInquirySave_Click(object sender, EventArgs e)
    {

        if (txtAction.Text == "")
        {
            DisplayMessage("Enter Feedback");
            txtAction.Focus();
            return;
        }


        string sql = "INSERT INTO [SM_Ticket_Description]   ([Ticket_No]           ,[Emp_Id]           ,[Date]           ,[Action]           ,[Customer_Id]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES  ("+Request.QueryString["Ticket_Id"].ToString()+" ,'0','"+DateTime.Now.ToString()+"','"+txtAction.Text+"','0',' ',' ',' ',' ',' ','"+false.ToString()+"','"+DateTime.Now.ToString()+"','"+true.ToString()+"','Customer','"+DateTime.Now.ToString()+"','Customer','"+DateTime.Now.ToString()+"')";

        objDa.execute_Command(sql);

        txtticketno_OnTextChanged(null, null);
    }


  

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnInquiryCancel_Click(object sender, EventArgs e)
    {
        Reset();
      
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
    public void Reset()
    {
        txtAction.Text = "";
        txtticketno.Text = "";
        pnlTicket.Visible = false;
        txtticketno.Enabled = true;
      
        txtticketno.Focus();
    }
    

   
    protected void txtticketno_OnTextChanged(object sendre, EventArgs e)
    {

        string sql = "select *,case when SM_Ticket_Master.Field6='True' then (select Name from EMS_ContactMaster where Trans_Id=SM_Ticket_Master.Customer_Name)    when SM_Ticket_Master.Field6='False' then SM_Ticket_Master.Customer_Name end as  customerName from SM_Ticket_Master where Trans_Id=" + Request.QueryString["Ticket_Id"].ToString() + "";
            DataTable dtTicket = objDa.return_DataTable(sql);

           
            if (dtTicket.Rows.Count > 0)
            {
                hdnTicketid.Value = dtTicket.Rows[0]["Trans_Id"].ToString();
                //DataTable dtTicketEmp = objTicketEmployee.GetAllRecord_ByTicketIdandEmployeeId(hdnTicketid.Value, Session["EmpId"].ToString());
                //if (dtTicketEmp.Rows.Count > 0)
                //{
                txtticketno.Text = dtTicket.Rows[0]["Ticket_No"].ToString();
                    lblTickeDate.Text = GetDate(dtTicket.Rows[0]["Ticket_Date"].ToString());
                    lblStatus.Text = dtTicket.Rows[0]["Status"].ToString();
                    lblTaskType.Text = dtTicket.Rows[0]["Task_Type"].ToString();
                    lblCustomerNameValue.Text = dtTicket.Rows[0]["CustomerName"].ToString();
                    lblScheduledate.Text = GetDate(dtTicket.Rows[0]["Schedule_Date"].ToString());
                    lblDescriptionvalue.Text = dtTicket.Rows[0]["Description"].ToString();

                    DataTable dt = ObjTicketFeedback.GetAllRecord();

                    try
                    {
                        //TicketId

                        dt = new DataView(dt, "Ticket_No=" + Request.QueryString["Ticket_Id"].ToString()+ " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    new PageControlCommon("DBConnection").FillData((object)GvViewFeedback, dt, "", "");
                
            }
            else
            {
                DisplayMessage("Ticket Not Found");
                txtticketno.Text = "";
                txtticketno.Focus();
                pnlTicket.Visible = false;
                return;
            }

        
      
       

    }


  








}