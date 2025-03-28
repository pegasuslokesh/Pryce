using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;

public partial class WebUserControl_Reminder : System.Web.UI.UserControl
{
    Reminder reminderClass = null;
    SystemParameter ObjSysParam = null;
    NotificationMaster Obj_Notifiacation = null;
    EmployeeMaster objEmployee = null;
    ReminderLogs objReminderLog = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        txtStart_Date.Attributes.Add("readonly", "readonly");
        txtExpiry_Date.Attributes.Add("readonly", "readonly");

        reminderClass = new Reminder(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objReminderLog = new ReminderLogs(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            fillRemindTo();
        }
    }

    public void fillRemindTo()
    {
        txtStart_Date.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        DataTable dtEmpDtls = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
        if (dtEmpDtls != null)
        {
            if (dtEmpDtls.Rows.Count > 0)
            {
                txtRemindTo.Text = dtEmpDtls.Rows[0]["Emp_Name"].ToString() + "/" + dtEmpDtls.Rows[0]["Emp_Code"].ToString() + ";";
            }
        }
    }

    //public void fillReminder()
    //{
    //    RemID = (HiddenField)Parent.FindControl("hdnid");


    //    if (RemID != null)
    //    {
    //        //Li_ReminderList1.Style.Add("display", "block");
    //        if (RemID.Value.Trim() != "")
    //        {
    //            //DataTable dtLogdataByTransId = objReminderLog.getAllCurrentDayLogsByReminderID(RemID.Value, Session["EmpId"].ToString());
    //            DataTable dt = objReminderLog.getAllLogsByEmpid(Session["EmpId"].ToString());
    //            DataTable dtLogdataByTransId = new DataView(dt, "Reminder_Id=" + RemID.Value + "", "", DataViewRowState.CurrentRows).ToTable();
    //            //DataTable dtLogdataByTransId = objReminderLog.getLogDataByTransID(Request.QueryString["ReminderLodID"].ToString());
    //            if (dtLogdataByTransId.Rows.Count > 0)
    //            {
    //                if (dtLogdataByTransId.Rows[0]["Is_read"].ToString() == "0")
    //                {
    //                    objReminderLog.setIsReadfalseBytransId(dtLogdataByTransId.Rows[0]["Trans_Id"].ToString(), Session["UserId"].ToString());

    //                }

    //                labelStartDt.Text = GetDate(dtLogdataByTransId.Rows[0]["Start_date"].ToString());
    //                labelDueDt.Text = GetDate(dtLogdataByTransId.Rows[0]["Due_date"].ToString());
    //                labelExpiryDt.Text = GetDate(dtLogdataByTransId.Rows[0]["Expiry_date"].ToString());
    //                labelRepeatType.Text = dtLogdataByTransId.Rows[0]["Repeat_type"].ToString();
    //                dropdownStatus.SelectedValue = dtLogdataByTransId.Rows[0]["Status"].ToString();
    //                labelReminderText.Text = dtLogdataByTransId.Rows[0]["Reminder_text"].ToString();
    //            }
    //        }

    //    }
    //}


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
        if (txtStart_Date.Text == "")
        {
            txtStart_Date.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtExpiry_Date.Text == "")
        {
            txtExpiry_Date.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtRemindTo.Text == "")
        {
            txtRemindTo.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtReminderText.Text == "")
        {
            txtReminderText.Focus();
            btnSave.Enabled = true;
            return;
        }
       
        if (!chkNotification.Checked )
        {
            chkNotification.Focus();
            btnSave.Enabled = true;
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

        if (ddlRepeatType.SelectedIndex == 0)
        {
            Session["due_date"]  = txtExpiry_Date.Text;
        }

        if (txtRepeatCount.Text.Trim() == "")
        {
            txtRepeatCount.Text = "1";
        }

        int reminder_id = 0;

        if (chkNotification.Checked)
        {
            foreach (string name in txtRemindTo.Text.Split(';'))
            {
                start_pos = name.LastIndexOf("/") + 1;
                last_pos = name.Length;
                string Emp_Code = name.Substring(start_pos, last_pos - start_pos);


                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());                
                id = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

                if (id != "0" && id != "")
                {
                    if (!StrTo.Split(';').Contains(id))
                    {
                        reminder_id = reminderClass.insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", "0", txtReminderText.Text, "#", txtStart_Date.Text, txtRepeatCount.Text, txtExpiry_Date.Text, ddlRepeatType.SelectedItem.ToString(), id, ddlStatus.SelectedItem.ToString(), email, sms, notification, Session["UserId"].ToString(), Session["UserId"].ToString());
                        reminderClass.Set_Url(reminder_id.ToString(), "../CRM/Reminder.aspx?ReminderID=" + reminder_id + "");
                        StrTo = StrTo + id + ";";
                        if (Convert.ToInt32(txtRepeatCount.Text.Trim()) > 0)
                        {
                            for (int i = 0; i < Convert.ToInt32(txtRepeatCount.Text.Trim()); i++)
                            {
                                objReminderLog.insertLogData(reminder_id.ToString(), Session["due_date"].ToString(), "", Session["UserId"].ToString(), Session["UserId"].ToString());
                                setDueDate(Session["due_date"].ToString());
                            }
                        }
                        else
                        {
                            objReminderLog.insertLogData(reminder_id.ToString(), Session["due_date"].ToString(), "", Session["UserId"].ToString(), Session["UserId"].ToString());
                        }
                    }
                }
            }
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "recordSave()", true);
        
        reset();
        btnSave.Enabled = true;
    }

    private void setDueDate(string date)
    {
        DateTime dt = Convert.ToDateTime(date);
        if (ddlRepeatType.SelectedItem.ToString() == "Daily")
        {
            Session["due_date"] = GetDate((dt.AddDays(1)).ToString());
        }

        if (ddlRepeatType.SelectedItem.ToString() == "Weekly")
        {
            Session["due_date"] = GetDate((dt.AddDays(7)).ToString());
        }

        if (ddlRepeatType.SelectedItem.ToString() == "Monthly")
        {
            Session["due_date"] = GetDate((dt.AddMonths(1)).ToString());
        }

        if (ddlRepeatType.SelectedItem.ToString() == "Yearly")
        {
            Session["due_date"] = GetDate((dt.AddYears(1)).ToString());
        }
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
        Dt = null;
        return EmployeeName;
    }


    private void reset()
    {
        Calender.SelectedDate = DateTime.Now;
        txtExpiry_Date.Text = "";
        ddlRepeatType.SelectedIndex = 0;
        txtRepeatCount.Enabled = true;
        txtRemindTo.Text = "";
        ddlStatus.SelectedIndex = 0;
        txtRepeatCount.Text = "";
        chkEmail.Checked = false;
        chkSMS.Checked = false;
        chkNotification.Checked = false;
        txtReminderText.Text = "";
        Session["due_date"] = "";
        fillRemindTo();
    }

    protected void ddlRepeatType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRepeatType.SelectedIndex != 0)
        {
            txtExpiry_Date.Enabled = false;
        }
        else
        {
            txtExpiry_Date.Enabled = true;
        }

        if (txtStart_Date.Text != "")
        {
            setDueDate(txtStart_Date.Text);

            if (ddlRepeatType.SelectedIndex == 0)
            {
                Session["due_date"] = txtExpiry_Date.Text;
            }
        }

        if (ddlRepeatType.SelectedIndex != 0)
        {
            txtRepeatCount.Enabled = true;
            lblExpiry_Date.Text = "Expiry Date";
            txtExpiry_Date.Text = "";
        }
        else
        {
            lblExpiry_Date.Text = "Due Date";
            txtRepeatCount.Enabled = false;
            txtExpiry_Date.Text = "";
            txtRepeatCount.Text = "";
        }
        txtRepeatCount_TextChanged(sender, e);
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }


    protected void txtExpiry_Date_TextChanged(object sender, EventArgs e)
    {
        if (ddlRepeatType.SelectedIndex != 0)
        {
            if (ObjSysParam.getDateForInput(txtExpiry_Date.Text) < ObjSysParam.getDateForInput(Session["due_date"].ToString()))
            {
                txtExpiry_Date.Focus();
                txtExpiry_Date.Text = "";
                return;
            }
        }
        else
        {
            if (ObjSysParam.getDateForInput(txtStart_Date.Text) > ObjSysParam.getDateForInput(txtExpiry_Date.Text))
            {
                txtExpiry_Date.Focus();
                txtExpiry_Date.Text = "";
                return;
            }
        }
    }

    protected void txtRepeatCount_TextChanged(object sender, EventArgs e)
    {
        if (txtRepeatCount.Text != "" && Convert.ToInt32(txtRepeatCount.Text.Trim()) > 0)
        {
            DateTime dt = Convert.ToDateTime(txtStart_Date.Text);
            int count = Convert.ToInt32(txtRepeatCount.Text);
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
        }
    }

}