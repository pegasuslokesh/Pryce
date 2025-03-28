using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CRM_Reminder : System.Web.UI.Page
{
    Reminder reminderClass = null;
    SystemParameter ObjSysParam = null;
    NotificationMaster Obj_Notifiacation = null;
    EmployeeMaster objEmployee = null;
    ReminderLogs objReminderLog = null;
    Common cmn = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }


        txtStart_Date.Attributes.Add("readonly", "readonly");
        txtValueDate.Attributes.Add("readonly", "readonly");
        txtExpiry_Date.Attributes.Add("readonly", "readonly");
        //txtDue_Date.Attributes.Add("readonly", "readonly");
        //Calender.SelectedDate = DateTime.Now;

        reminderClass = new Reminder(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objReminderLog = new ReminderLogs(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            if (Request.QueryString["ReminderID"] != null)
            {
                fillGridByReminderID(Request.QueryString["ReminderID"].ToString());
            }
            else
            {
                fillCurrentGrid();
            }
        }
    }

    public void fillCurrentGrid()
    {
        if(Request.QueryString["ReminderID"]!=null)
        {
            fillGridByReminderID(Request.QueryString["ReminderID"].ToString());
            GvReminderList.Columns[1].Visible = false;
            GvReminderList.Columns[2].Visible = true;
            GvReminderList.Columns[3].Visible = true;
            return;
        }
        DataTable Dt_activeDate =  reminderClass.AllDataByEmp_ID(Session["EmpId"].ToString());
        GvReminderList.DataSource = Dt_activeDate;
        GvReminderList.DataBind();
        GvReminderList.Columns[1].Visible = false;
        GvReminderList.Columns[2].Visible = true;
        GvReminderList.Columns[3].Visible = true;
        Session["fillReminderGrid"] = Dt_activeDate;
        lblTotalRecords.Text = "Total Records: " + Dt_activeDate.Rows.Count.ToString();

        string date = System.DateTime.Now.ToString();
       
        foreach (DataRow dr in Dt_activeDate.Rows)
        {
            if(GetDate(dr["Start_date"].ToString())== GetDate(dr["Expiry_date"].ToString()))
            {

            }
            else
            {
                if (GetDate(date) == GetDate(dr["Expiry_date"].ToString()))
                {
                    reminderClass.Set_ReminderOFF(dr["Trans_id"].ToString(), Session["UserId"].ToString());
                }
            }            
        }

    }

    
    public void fillGridByReminderID(string ReminderID)
    {
        try
        {
            DataTable Dt_activeDate = reminderClass.AllCurrentDateDataByTransID_EmpID(ReminderID, Session["EmpId"].ToString());

            objReminderLog.setIsReadTrueBytransId(Dt_activeDate.Rows[0]["LogTrans_Id"].ToString(), Session["UserId"].ToString());

            string date = System.DateTime.Now.ToString();
            if (GetDate(date) == GetDate(Dt_activeDate.Rows[0]["Expiry_date"].ToString()))
            {
                reminderClass.Set_ReminderOFF(Dt_activeDate.Rows[0]["Trans_id"].ToString(), Session["UserId"].ToString());
            }

            DataTable Dt_Date = reminderClass.AllCurrentDateDataByTransID_EmpID(ReminderID, Session["EmpId"].ToString());
            GvReminderList.DataSource = Dt_Date;
            GvReminderList.DataBind();
            GvReminderList.Columns[1].Visible = false;

            //btnRefresh.Enabled = false;

            if (Dt_Date.Rows[0]["Is_Active"].ToString() == "False")
            {
                GvReminderList.Columns[2].Visible = false;
                GvReminderList.Columns[3].Visible = false;
            }
            Session["fillReminderGrid"] = Dt_Date;
            lblTotalRecords.Text = "Total Records: " + Dt_Date.Rows.Count.ToString();
        }
        catch(Exception er)
        {

        }
    }


    public void fillUpcomingGrid(int days)
    {
        DataTable Dt_activeDate = reminderClass.AllUpcomingDataByEmp_ID(Session["EmpId"].ToString(), days.ToString());
        GvReminderList.DataSource = Dt_activeDate;
        GvReminderList.DataBind();
        GvReminderList.Columns[1].Visible = false;
        GvReminderList.Columns[2].Visible = true;
        GvReminderList.Columns[3].Visible = true;
        Session["fillReminderGrid"] = Dt_activeDate;
        lblTotalRecords.Text = "Total Records: " + Dt_activeDate.Rows.Count.ToString();
    }

    public void fillGoneGrid(int days)
    {
        DataTable Dt_activeDate = reminderClass.AllGoneDataByEmpID(Session["EmpId"].ToString(),days.ToString());
        GvReminderList.DataSource = Dt_activeDate;
        GvReminderList.DataBind();
        Session["fillReminderGrid"] = Dt_activeDate;
        GvReminderList.Columns[1].Visible = false;
        GvReminderList.Columns[2].Visible = false;
        GvReminderList.Columns[3].Visible = false;
        lblTotalRecords.Text = "Total Records: "+ Dt_activeDate.Rows.Count.ToString();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactList(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = new DataTable();

        try
        {
            dtCon = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText.ToString());
        }
        catch
        {
            dtCon = new DataTable();
        }

        string[] txt = new string[dtCon.Rows.Count];

        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                txt[i] += dtCon.Rows[i]["Emp_Name"].ToString() + "/" + dtCon.Rows[i]["Emp_Code"].ToString() + ";";
            }

        }

        return txt;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
       
        if (txtExpiry_Date.Text == "")
        {
            txtExpiry_Date.Focus();
            return;
        }
        if (txtRemindTo.Text == "")
        {
            txtRemindTo.Focus();
            return;
        }
        if (txtReminderText.Text == "")
        {
            txtReminderText.Focus();
            return;
        }
        if (!chkNotification.Checked)
        {
            chkNotification.Focus();
            return;
        }
        string date = System.DateTime.Now.ToString();
        string email = "", sms = "", notification = "";
        if (chkEmail.Checked)
            email = "true";
        else
            email = "false";

        if (chkSMS.Checked)
            sms = "true";
        else
            sms = "false";

        if (chkNotification.Checked)
            notification = "true";
        else
            notification = "false";

        string id, StrTo = "";
        int start_pos, last_pos;

        if(hdndue_date.Value == "")
        {
            hdndue_date.Value = txtExpiry_Date.Text;
        }
        if(txtRepeatCount.Text=="")
        {
            txtRepeatCount.Text = "1";
        }
        
        if(chkNotification.Checked)
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtRemindTo.Text.Split('/')[1].ToString();
            id = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

            objReminderLog.DeleteLogsByReminderID(hdntrans_id.Value);
            reminderClass.UpdateStandaloneData(hdntrans_id.Value, txtReminderText.Text, hdnURL.Value, txtStart_Date.Text, txtRepeatCount.Text, txtExpiry_Date.Text, ddlRepeatType.SelectedItem.ToString(), id, ddlStatus.SelectedItem.ToString(), email, sms, notification, Session["UserId"].ToString());

            if (Convert.ToInt32(txtRepeatCount.Text.Trim()) > 0)
            {
                for (int i = 0; i < Convert.ToInt32(txtRepeatCount.Text.Trim()); i++)
                {
                    objReminderLog.insertLogData(hdntrans_id.Value, hdndue_date.Value, "", Session["UserId"].ToString(), Session["UserId"].ToString());

                    setDueDate(hdndue_date.Value);
                }
            }
            else
            {
                objReminderLog.insertLogData(hdntrans_id.Value, hdndue_date.Value, "", Session["UserId"].ToString(), Session["UserId"].ToString());
            }
        }

        reset();
        fillCurrentGrid();

        Li_New.Visible = false;
        //New.Visible = false;

    }

    protected void Send_Notification_Task(string message, string id, string url)
    {
        int Save_Notification = 0;
        string Message = string.Empty;
        Message = message;
        GetEmployeeName(Session["EmpId"].ToString());
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), id, Message, "38", url, "", "0", "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "17");
    }

    public string GetEmployeeName(string EmployeeId)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
            ViewState["Emp_Img"] = "../CompanyResource/2/" + Dt.Rows[0]["Emp_Image"].ToString();
        }
        else
        {
            ViewState["Emp_Img"] = "";
        }

        return EmployeeName;
    }

    protected void GvReminderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvReminderList.PageIndex = e.NewPageIndex;
        DataTable dtPaging = (DataTable)Session["fillReminderGrid"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvReminderList, dtPaging, "", "");
    }

    protected void GvReminderList_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_Sorting = (DataTable)Session["fillReminderGrid"];
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

        dt_Sorting = (new DataView(dt_Sorting, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["fillReminderGrid"] = dt_Sorting;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvReminderList, dt_Sorting, "", "");

    }

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        ImageButton b = (ImageButton)sender;
        string objSenderID = b.ID;
        hdntrans_id.Value = e.CommandArgument.ToString();
        hdnlogTrans_id.Value = e.CommandName.ToString();
        DataTable DtData = reminderClass.AllDataByTransID(hdntrans_id.Value);

        string date = System.DateTime.Now.ToString();

        foreach(DataRow dr in DtData.Rows)
        {
            if (GetDate(date) == GetDate(dr["Expiry_date"].ToString()) && dr["Status"].ToString() == "On")
            {
                reminderClass.Set_ReminderOFF(dr["Trans_id"].ToString(), Session["UserId"].ToString());
            }
        }

        DataTable DtSingleRecord = reminderClass.AllDataByTransID(hdntrans_id.Value);

        DtSingleRecord = new DataView(DtSingleRecord, "LogTrans_Id=" + hdnlogTrans_id.Value, "", DataViewRowState.CurrentRows).ToTable();

        Li_New.Visible = true;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

        if (objSenderID == "btnEdit")
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            btnSave.Visible = true;           
            btnCancel.Visible = true;
        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            btnSave.Visible = false;           
            btnCancel.Visible = true;
        }

        txtStart_Date.Enabled = false;
        ddlRepeatType.Enabled = false;
        txtRemindTo.Enabled = false;
        txtExpiry_Date.Enabled = false;
        txtRepeatCount.Enabled = false;
        
        if(DtSingleRecord.Rows.Count>0)
        {
            txtStart_Date.Text = GetDate(DtSingleRecord.Rows[0]["Start_date"].ToString());
            txtExpiry_Date.Text = GetDate(DtSingleRecord.Rows[0]["Expiry_date"].ToString());
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(DtSingleRecord.Rows[0]["Remind_to"].ToString());
            txtRemindTo.Text = DtSingleRecord.Rows[0]["RemindTo_Name"].ToString() + "/" + Emp_Code;
            ddlStatus.SelectedValue = DtSingleRecord.Rows[0]["Status"].ToString();
            hdnURL.Value = DtSingleRecord.Rows[0]["Target_url"].ToString();
            txtRepeatCount.Text = DtSingleRecord.Rows[0]["Field1"].ToString();

            ddlRepeatType.SelectedValue = DtSingleRecord.Rows[0]["Repeat_type"].ToString();


            if (DtSingleRecord.Rows[0]["Notify_by_email"].ToString() == "0")
            {
                chkEmail.Checked = false;
            }
            else
            {
                chkEmail.Checked = true;
            }

            if (DtSingleRecord.Rows[0]["Notify_by_sms"].ToString() == "0")
            {
                chkSMS.Checked = false;
            }
            else
            {
                chkSMS.Checked = true;
            }

            if (DtSingleRecord.Rows[0]["Notify_by_Notification"].ToString() == "0")
            {
                chkNotification.Checked = false;
            }
            else
            {
                chkNotification.Checked = true;
            }

            txtReminderText.Text = DtSingleRecord.Rows[0]["Reminder_text"].ToString();

            if (DtSingleRecord.Rows[0]["Is_read"].ToString() == "False")
            {
                objReminderLog.setIsReadTrueBytransId(DtSingleRecord.Rows[0]["LogTrans_Id"].ToString(), Session["UserId"].ToString());
            }
        }      
    } 

    private void reset()
    {
        //Calender.SelectedDate = DateTime.Now;
        txtExpiry_Date.Text = "";
        //txtDue_Date.Text = "";
        ddlRepeatType.SelectedIndex = 0;
        txtRemindTo.Text = "";
        ddlStatus.SelectedIndex = 0;
        hdnlogTrans_id.Value = "";
        chkEmail.Checked = false;
        chkSMS.Checked = false;
        chkNotification.Checked = false;
        txtReminderText.Text = "";
        hdntrans_id.Value = "0";
        hdnURL.Value = "0";
        txtRepeatCount.Text = "";
    }
    

    protected void ddlRepeatType_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (txtStart_Date.Text.Trim() != "")
        {
            setDueDate(txtStart_Date.Text);
        }

        if(ddlRepeatType.SelectedIndex==0)
        {
            txtRepeatCount.Enabled = false;
            txtExpiry_Date.Text = "";
            txtRepeatCount.Text = "";
            lblExpiry_Date.Text = "Due Date";
        }       
        else
        {
            lblExpiry_Date.Text = "Expiry Date";
            txtRepeatCount.Enabled = true;
        }
        txtRepeatCount_TextChanged(sender, e);
    }


    private void setDueDate(string date)
    {
        DateTime dt = Convert.ToDateTime(date);
        if (ddlRepeatType.SelectedItem.ToString() == "Daily")
        {
            hdndue_date.Value = GetDate((dt.AddDays(1)).ToString());
        }

        if (ddlRepeatType.SelectedItem.ToString() == "Weekly")
        {
            hdndue_date.Value = GetDate((dt.AddDays(7)).ToString());
        }

        if (ddlRepeatType.SelectedItem.ToString() == "Monthly")
        {
            hdndue_date.Value = GetDate((dt.AddMonths(1)).ToString());
        }

        if (ddlRepeatType.SelectedItem.ToString() == "Yearly")
        {
            hdndue_date.Value = GetDate((dt.AddYears(1)).ToString());
        }
    }

    protected void txtExpiry_Date_TextChanged(object sender, EventArgs e)
    {
        if (ObjSysParam.getDateForInput(txtExpiry_Date.Text) < ObjSysParam.getDateForInput(hdndue_date.Value))
        {
            txtExpiry_Date.Focus();
            txtExpiry_Date.Text = "";
            return;
        }
        if (ObjSysParam.getDateForInput(txtExpiry_Date.Text) < ObjSysParam.getDateForInput(txtStart_Date.Text))
        {
            txtExpiry_Date.Focus();
            txtExpiry_Date.Text = "";
            DisplayMessage("Expiry Date should be greater then Start Date");
            return;
        }
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

   


    protected void ddlChangeState_SelectedIndexChanged1(object sender, EventArgs e)
    {
        int index = ((GridViewRow)((DropDownList)sender).Parent.Parent).RowIndex;
        HiddenField hdnTransID = (HiddenField)GvReminderList.Rows[index].FindControl("hdnchangeStateTransID");
        DropDownList ddl = (DropDownList)GvReminderList.Rows[index].FindControl("ddlChangeState");
        Label LblExpDt = (Label)GvReminderList.Rows[index].FindControl("lblExpiry_date");
        string date = System.DateTime.Now.ToString();
        if (ObjSysParam.getDateForInput(LblExpDt.Text) > ObjSysParam.getDateForInput(GetDate(date)))
        {
            if (ddl.SelectedItem.ToString() == "On")
            {
                reminderClass.Set_ReminderON(hdnTransID.Value, Session["UserId"].ToString());
            }
            else
            {
                reminderClass.Set_ReminderOFF(hdnTransID.Value, Session["UserId"].ToString());
            }
        }
        else
        {
            DisplayMessage("Cant Modify Expired Reminder");

            if(ddl.SelectedItem.ToString() == "On")
            {
                ddl.SelectedValue = "Off";
            }
            else
            {
                ddl.SelectedValue = "On";
            }
        }


        
    }
    protected void txtRepeatCount_TextChanged(object sender, EventArgs e)
    {
        
        if (txtRepeatCount.Text.Trim()!="" )
        {
            int count = Convert.ToInt32(txtRepeatCount.Text);
            if(count>0)
            {
                DateTime dt = Convert.ToDateTime(txtStart_Date.Text);

                if (ddlRepeatType.SelectedItem.ToString() == "Daily")
                {
                    txtExpiry_Date.Text = GetDate((dt.AddDays(count)).ToString());
                }

                if (ddlRepeatType.SelectedItem.ToString() == "Weekly")
                {
                    txtExpiry_Date.Text = GetDate((dt.AddDays(7 * count)).ToString());
                }

                if (ddlRepeatType.SelectedItem.ToString() == "Monthly")
                {
                    txtExpiry_Date.Text = GetDate((dt.AddMonths(count)).ToString());
                }

                if (ddlRepeatType.SelectedItem.ToString() == "Yearly")
                {
                    txtExpiry_Date.Text = GetDate((dt.AddYears(count)).ToString());
                }
            }      
            else
            {
                txtRepeatCount.Text = "";
                txtRepeatCount.Focus();
                DisplayMessage("Count Should Be Positive");
            }      
        }        
    }
    
 
    protected void btnReminderDelete_Command(object sender, CommandEventArgs e)
    {
        reminderClass.Set_ReminderIsActiveFalse(e.CommandArgument.ToString(), Session["UserId"].ToString());
        objReminderLog.setIsActivefalseByReminderId(e.CommandArgument.ToString(), Session["UserId"].ToString());

        if (ddlFieldName.SelectedValue == "Upcoming")
            fillUpcomingGrid(Convert.ToInt32(txtValueDays.Text));

        if (ddlFieldName.SelectedValue == "Gone")
            fillGoneGrid(Convert.ToInt32(txtValueDays.Text));

        if (ddlFieldName.SelectedValue == "Current")
            fillCurrentGrid();

    }


    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlFieldName.SelectedIndex==0)
        {
            fillCurrentGrid();
            ddlPerameter.Enabled = true;
            ddlPerameter.SelectedIndex = 0;
            txtValue.Visible = true;
            txtValueDate.Visible = false;
            txtValueDays.Visible = false;
            ddlOption.Enabled = true;
        }
        if (ddlFieldName.SelectedIndex == 1 || ddlFieldName.SelectedIndex == 2)
        {
            ddlPerameter.SelectedValue = "Days";
            ddlPerameter.Enabled = false;
            txtValueDays.Visible = true;
            txtValue.Visible = false;
            txtValueDate.Visible = false;
            ddlOption.Enabled = false;
        }
       

    }


    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        ddlFieldName.SelectedIndex = 0;
        ddlFieldName_SelectedIndexChanged(null,null);
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValueDate.Text = "";
        fillCurrentGrid();
    }


    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        //ddlFieldName_SelectedIndexChanged(null, null);
        if(ddlFieldName.SelectedValue == "Upcoming")
            fillUpcomingGrid(0);

        if (ddlFieldName.SelectedValue == "Gone")
            fillGoneGrid(0);

        if (ddlFieldName.SelectedValue == "Current")
            fillCurrentGrid();

        

        
        if (ddlPerameter.SelectedItem.Text == "Days")
        {
            int parsedValue;
            if (int.TryParse(txtValueDays.Text, out parsedValue))
            {
                if (ddlFieldName.SelectedValue == "Upcoming")
                {
                    fillUpcomingGrid(Convert.ToInt32(txtValueDays.Text));
                    return;
                }
                if (ddlFieldName.SelectedValue == "Gone")
                {
                    fillGoneGrid(Convert.ToInt32(txtValueDays.Text));
                    return;
                }
                if(ddlFieldName.SelectedValue=="Current")
                {
                    DisplayMessage("Search on Current Day by Days is not Possible");
                    return;
                }
            }
        }


        DataTable dtAdd = Session["fillReminderGrid"] as DataTable;

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlPerameter.SelectedItem.Value == "Due_date" || ddlPerameter.SelectedItem.Value == "Expiry_date")
            {
                if (txtValueDate.Text.Trim() != "")
                {
                    if (ddlOption.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlPerameter.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "'";
                    }
                    else if (ddlOption.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlPerameter.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlPerameter.SelectedValue + ",System.String) Like '%" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "%'";
                    }
                }
                else
                {
                    DisplayMessage("Enter Date");
                    txtValueDate.Focus();
                    return;
                }
            }
            else
            {
                if (txtValue.Text.Trim() != "")
                {
                    if (ddlOption.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlPerameter.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                    }
                    else if (ddlOption.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlPerameter.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlPerameter.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
                    }
                }
                else
                {
                    DisplayMessage("Enter Some Value");
                    
                    txtValueDate.Focus();
                }
                

                DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvReminderList, view.ToTable(), "", "");
                Session["fillReminderGrid"] = view.ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            }
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
        DataTable dtres = (DataTable)ViewState["MessageDt"];
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


    protected void ddlPerameter_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtValueDays.Visible = false;
        txtValueDate.Visible = false;
        txtValue.Visible = true;

        if (ddlPerameter.SelectedItem.Text=="Due Date" || ddlPerameter.SelectedItem.Text == "Expiry Date")
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValueDays.Visible = false;
        }

        if (ddlPerameter.SelectedItem.Text == "Days")
        {
            txtValueDays.Visible = true;
            txtValueDate.Visible = false;
            txtValue.Visible = false;
        }
      
        

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
}