using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Duty_Master_Duty_Feedback : System.Web.UI.Page
{
    NotificationMaster Obj_Notifiacation = null;
    EmployeeMaster objEmp = null;
    Common cmn = null;
    TaskMaster TaskMaster = null;
    DutyMaster DutyMaster = null;
    IT_ObjectEntry objObjectEntry = null;
    SystemParameter objSys = null;
    DesignationMaster objDesg = null;
    Common ObjComman = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/ERPLogin.aspx");
            }

            Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
            objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            TaskMaster = new TaskMaster(Session["DBConnection"].ToString());
            DutyMaster = new DutyMaster(Session["DBConnection"].ToString());
            objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
            objSys = new SystemParameter(Session["DBConnection"].ToString());
            objDesg = new DesignationMaster(Session["DBConnection"].ToString());
            ObjComman = new Common(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
            Page.Title = objSys.GetSysTitle();
          
            if (!IsPostBack)
            {
               
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Duty_Master/Duty_Feedback.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
                if (clsPagePermission.bHavePermission == false)
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                Hdn_Contact_Id.Value = "0";
                try
                {
                    //Ddl_Employee_New.Items.Insert(0, "--Select--");
                    //Ddl_Employee_New.SelectedIndex = 0;
                    //Txt_Contact.Text = "";
                    //Txt_Start_Date.Text = "";
                    //Txt_Due_Date.Text = "";
                    //Txt_Title.Text = "";
                    //Editor_Description.Content = "";
                }
                catch
                {
                    //Ddl_Employee_New.Items.Insert(0, "--Select--");
                    //Ddl_Employee_New.SelectedIndex = 0;
                    //Txt_Contact.Text = "";
                    //Txt_Start_Date.Text = "";
                    //Txt_Due_Date.Text = "";
                    //Txt_Title.Text = "";
                    //Editor_Description.Content = "";
                }
                Get_Employee();
                Fill_Grid_List();
            }
            CalendarExtender1.Format = objSys.SetDateFormat();
            CalendarExtender2.Format = objSys.SetDateFormat();
            //CalendarExtender4.Format = objSys.SetDateFormat();
            //CalendarExtender5.Format = objSys.SetDateFormat();
            //CalendarExtender6.Format = objSys.SetDateFormat();
        }
        catch
        {
        }
    }

   

    public void DisplayMessage(string str)
    {
        try
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
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + GetArebicMessage(str) + "');", true);
            }
        }
        catch
        {
        }
    }

    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
                ArebicMessage = EnglishMessage;
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }

    protected void Get_Employee()
    {
        try
        {
            DataTable Dt_Duty_Employee = DutyMaster.Get_Hr_EmpDutyTrans("0", Session["UserID"].ToString(), Session["EmpID"].ToString(), "0", DateTime.Now.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
            if (Dt_Duty_Employee.Rows.Count > 0)
            {
                Fill_DDL_Employee(Dt_Duty_Employee);
                Ddl_Employee_List.SelectedValue = Session["EmpId"].ToString();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Fill_DDL_Employee(DataTable Dt_Employee)
    {
        try
        {
            //Ddl_Employee_New.Items.Clear();
            Ddl_Employee_List.Items.Clear();
            if (Dt_Employee.Rows.Count > 0)
            {
                Ddl_Employee_List.DataSource = null;
                Ddl_Employee_List.DataBind();
                Ddl_Employee_List.DataSource = Dt_Employee;
                Ddl_Employee_List.DataTextField = "Emp_Name";
                Ddl_Employee_List.DataValueField = "Employee_ID";
                Ddl_Employee_List.DataBind();
                Ddl_Employee_List.Items.Insert(0, "--Select--");



                //DataTable Dt = Dt_Employee = new DataView(Dt_Employee, "Field5=" + Session["EmpID"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                //if (Dt.Rows.Count > 0)
                //{
                //    Session["Feed_Tl_Team_Emp"] = Dt;
                //    Gv_Feedback_List.Columns[0].Visible = true;
                //    Gv_Feedback_List.Columns[1].Visible = true;
                //    Gv_Feedback_List.Columns[2].Visible = true;
                //    //Li_New.Visible = true;
                //    //Li_Bin.Visible = true;
                //    ratingControl.Visible = true;
                //    RequiredFieldValidator5.ValidationGroup = "Send";
                //    DDL_Status_Conversation.Visible = true;
                //    //Ddl_Employee_New.DataSource = null;
                //    //Ddl_Employee_New.DataBind();
                //    //Ddl_Employee_New.DataSource = Dt_Employee;
                //    //Ddl_Employee_New.DataTextField = "Emp_Name";
                //    //Ddl_Employee_New.DataValueField = "Emp_Id";
                //    //Ddl_Employee_New.DataBind();
                //    //Ddl_Employee_New.Items.Insert(0, "--Select--");
                //}
                //else
                //{
                //    Gv_Feedback_List.Columns[0].Visible = false;
                //    Gv_Feedback_List.Columns[1].Visible = false;
                //    Gv_Feedback_List.Columns[2].Visible = false;
                //    //Li_Bin.Visible = false;
                //    //Li_New.Visible = false;
                //    ratingControl.Visible = false;
                //    RequiredFieldValidator5.ValidationGroup = "False";
                //    DDL_Status_Conversation.Visible = false;
                //    //Ddl_Employee_New.Items.Insert(0, "--Select--");
                //}
                //if (Session["EmpID"].ToString() != "0")
                //{
                //    Ddl_Employee_List.SelectedValue = Session["EmpID"].ToString();
                //    Emp_List_ID.Value = Session["EmpID"].ToString();
                //}
                //else
                //{
                //    Emp_List_ID.Value = Ddl_Employee_List.SelectedValue.ToString();
                //}
            }
            else
            {
                try
                {
                    Emp_List_ID.Value = "0";

                    //Ddl_Employee_New.Items.Insert(0, "--Select--");
                    //Ddl_Employee_New.SelectedIndex = 0;

                    Ddl_Employee_List.Items.Insert(0, "--Select--");
                    Ddl_Employee_List.SelectedIndex = 0;
                }
                catch
                {
                    Emp_List_ID.Value = "0";

                    //Ddl_Employee_New.Items.Insert(0, "--Select--");
                    //Ddl_Employee_New.SelectedIndex = 0;

                    Ddl_Employee_List.Items.Insert(0, "--Select--");
                    Ddl_Employee_List.SelectedIndex = 0;
                }
            }
        }
        catch
        {
        }
    }

    protected void Fill_Grid_List()
    {
        try
        {
            DataTable Dt_Emp_Duties = DutyMaster.Get_Hr_EmpDutyTrans("0", Session["UserID"].ToString(), Session["EmpID"].ToString(), "0", DateTime.Now.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
            if (Dt_Emp_Duties.Rows.Count > 0)
            {
                Session["Feed_Dt_Task_List_Active"] = Dt_Emp_Duties;
                //Fill_Gv_Feedback(Dt_Emp_Duties);
                Ddl_Duty_Status_SelectedIndexChanged(null, null);
                //if (Ddl_Employee_List.SelectedValue.ToString() == Session["EmpID"].ToString())
                //{
                //    Ddl_Duty_Status.Visible = false;
                //    Lbl_Duty_Status.Visible = false;
                //}
                //else
                //{
                //    Ddl_Duty_Status.Visible = true;
                //    Lbl_Duty_Status.Visible = true;
                //}

            }
            else
            {
                Session["Feed_Dt_Task_List_Active"] = null;
                Fill_Gv_Feedback(null);
            }
        }
        catch
        {
        }
    }

    protected void Fill_Gv_Feedback(DataTable Dt_Grid)
    {
        Lbl_TotalRecords.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();
        if (Session["EmpID"].ToString() == Ddl_Employee_List.SelectedValue.ToString())
        {
            if (Dt_Grid.Rows.Count > 0)
            {
                //Gv_Feedback_List.Columns[0].Visible = false;
                //Gv_Feedback_List.Columns[1].Visible = false;
                //Gv_Feedback_List.Columns[2].Visible = false;
               
                Gv_Feedback_List.DataSource = Dt_Grid;
                Gv_Feedback_List.DataBind();
                
                // change by ghanshyam
                //Img_Emp_List_Select_All.Visible = false;
                //Img_Emp_List_Delete_All.Visible = false;
                //ratingControl.Visible = false;
                //DDL_Status_Conversation.Visible = false;
                // change by ghanshyam

                RequiredFieldValidator5.ValidationGroup = "False";
            }
            else
            {
                Gv_Feedback_List.DataSource = null;
                Gv_Feedback_List.DataBind();

                //// change by ghanshyam
                //Img_Emp_List_Select_All.Visible = false;
                //Img_Emp_List_Delete_All.Visible = false;
                //ratingControl.Visible = false;
                //DDL_Status_Conversation.Visible = false;
                //// change by ghanshyam

                RequiredFieldValidator5.ValidationGroup = "False";
            }
        }
        else
        {
            if (Dt_Grid.Rows.Count > 0)
            {
                //Gv_Feedback_List.Columns[0].Visible = true;
                //Gv_Feedback_List.Columns[1].Visible = true;
                //Gv_Feedback_List.Columns[2].Visible = true;
             
                Gv_Feedback_List.DataSource = Dt_Grid;
                Gv_Feedback_List.DataBind();
             
              //  Img_Emp_List_Select_All.Visible = true;
              //  Img_Emp_List_Delete_All.Visible = true;
                ratingControl.Visible = true;
                DDL_Status_Conversation.Visible = true;
                RequiredFieldValidator5.ValidationGroup = "Send";
            }
            else
            {
                Gv_Feedback_List.DataSource = Dt_Grid;
                Gv_Feedback_List.DataBind();
                //// change by ghanshyam
                //Img_Emp_List_Select_All.Visible = false;
                //Img_Emp_List_Delete_All.Visible = false;
                //ratingControl.Visible = false;
                //DDL_Status_Conversation.Visible = false;
                //// change by ghanshyam
                RequiredFieldValidator5.ValidationGroup = "False";
            }
        }
    }

    protected void Btn_Conversation_Command(object sender, CommandEventArgs e)
    {
        try
        {
            ratingControl.CurrentRating = 0;
            Txt_FeedBack.Text = "";
            DDL_Status_Conversation.SelectedIndex = 0;
            Hdn_Conversation_ID.Value = "";
            Hdn_Feedback_Trans.Value = "";
            Hdn_Emp_Status.Value = "";
            Hdn_Duty_ID.Value = "";
            Hdn_TL_Status.Value = "";
            Hdn_Trans_Date.Value = "";
            Hdn_Status.Value = "";
            Hdn_Effect_Date_From.Value = "";
            Hdn_Effect_Date_To.Value = "";
            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            Label Lbl_ID_List = gvRow.FindControl("Lbl_ID_List") as Label;
            Label Lbl_Task_List = gvRow.FindControl("Lbl_Task_List") as Label;
            Label Lbl_Duty_ID_List = gvRow.FindControl("Lbl_Duty_ID_List") as Label;
            Label Lbl_Task_Description_List = gvRow.FindControl("Lbl_Task_Description_List") as Label;
            Label Lbl_Effect_Date_From_List = gvRow.FindControl("Lbl_Effect_Date_From_List") as Label;
            Label Lbl_Effect_Date_To_List = gvRow.FindControl("Lbl_Effect_Date_To_List") as Label;
            Label Lbl_Report_To_List = gvRow.FindControl("Lbl_Report_To_List") as Label;

            Label Lbl_Emp_Status_List = gvRow.FindControl("Lbl_Emp_Status_List") as Label;
            Label Lbl_TL_Status_List = gvRow.FindControl("Lbl_TL_Status_List") as Label;
            Label Lbl_Trans_Date_List = gvRow.FindControl("Lbl_Trans_Date_List") as Label;

            Hdn_Effect_Date_From.Value = Lbl_Effect_Date_From_List.Text;
            Hdn_Effect_Date_To.Value = Lbl_Effect_Date_To_List.Text;


            Hdn_Conversation_ID.Value = e.CommandArgument.ToString();
            
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Conversation()", true);
            Lbl_C_Task.Text = Lbl_Task_List.Text;
            Lbl_C_Date.Text = Lbl_Effect_Date_From_List.Text;
            Ltr_C_Description.Text = Lbl_Task_Description_List.Text;
            Hdn_Duty_ID.Value = Lbl_Duty_ID_List.Text;

            Hdn_Emp_Status.Value = Lbl_Emp_Status_List.Text;
            Hdn_TL_Status.Value = Lbl_TL_Status_List.Text;
            Hdn_Trans_Date.Value = Lbl_Trans_Date_List.Text;

            if (Ddl_Employee_List.SelectedValue.ToString() == Session["EmpID"].ToString())
            {
                if (Lbl_Emp_Status_List.Text == "")
                    DDL_Status_Conversation.SelectedIndex = 0;
                else
                {
                    if (Lbl_Emp_Status_List.Text == "Select")
                        DDL_Status_Conversation.SelectedValue = "0";
                    else if (Lbl_Emp_Status_List.Text == "Not Started")
                        DDL_Status_Conversation.SelectedValue = "1";
                    else if (Lbl_Emp_Status_List.Text == "Deferred")
                        DDL_Status_Conversation.SelectedValue = "2";
                    else if (Lbl_Emp_Status_List.Text == "In-Progress")
                        DDL_Status_Conversation.SelectedValue = "3";
                    else if (Lbl_Emp_Status_List.Text == "Completed")
                        DDL_Status_Conversation.SelectedValue = "4";
                    else if (Lbl_Emp_Status_List.Text == "Waiting For Input")
                        DDL_Status_Conversation.SelectedValue = "5";
                }
            }
            else
            {
                if (Lbl_TL_Status_List.Text == "")
                    DDL_Status_Conversation.SelectedIndex = 0;
                else
                {
                    if (Lbl_TL_Status_List.Text == "Select")
                        DDL_Status_Conversation.SelectedValue = "0";
                    else if (Lbl_TL_Status_List.Text == "Not Started")
                        DDL_Status_Conversation.SelectedValue = "1";
                    else if (Lbl_TL_Status_List.Text == "Deferred")
                        DDL_Status_Conversation.SelectedValue = "2";
                    else if (Lbl_TL_Status_List.Text == "In-Progress")
                        DDL_Status_Conversation.SelectedValue = "3";
                    else if (Lbl_TL_Status_List.Text == "Completed")
                        DDL_Status_Conversation.SelectedValue = "4";
                    else if (Lbl_TL_Status_List.Text == "Waiting For Input")
                        DDL_Status_Conversation.SelectedValue = "5";
                }


                if (DDL_Status_Conversation.SelectedValue.ToString() == "4" && Ddl_Duty_Status.SelectedValue.ToString() == "1")
                {
                    DDL_Status_Conversation.Enabled = false;
                    Txt_FeedBack.Enabled = false;
                    Btn_Send_Feedback.Enabled = false;
                }
                else
                {
                    DDL_Status_Conversation.Enabled = true;
                    Txt_FeedBack.Enabled = true;
                    Btn_Send_Feedback.Enabled = true;
                }
            }
            string Conversation = string.Empty;
            if (Lbl_Emp_Status_List.Text != "" || Lbl_TL_Status_List.Text != "" || Lbl_Trans_Date_List.Text != "")
            {                
                DataTable Dt_Conversion = DutyMaster.Get_Sys_Feedback("0", "0", "0", "0", Hdn_Conversation_ID.Value, "0", "", "0", "", "", "", "", "", "True", "0", "0", "1");
                if (Dt_Conversion.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt_Conversion.Rows.Count; i++)
                    {
                        ratingControl.CurrentRating = Convert.ToInt32(Dt_Conversion.Rows[i]["Rating"]);
                        if (Dt_Conversion.Rows[i]["Emp_Name"].ToString() == "")
                            Dt_Conversion.Rows[i]["Emp_Name"] = "Superadmin";
                        if (Dt_Conversion.Rows[i]["Emp_ID"].ToString() == Session["EmpID"].ToString())
                        {
                            if (Conversation == "")
                                Conversation = "<div class='item'><img src='../Images/default_user_icon.png' alt='user image' class='online'><p class='message'><a href='#' class='name'><small class='text-muted pull-right'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + Convert.ToDateTime(Dt_Conversion.Rows[i]["Modification_Date"]).ToString("dd-MMM-yy HH:MM:ss") + "</small>" + Dt_Conversion.Rows[i]["Emp_Name"].ToString() + Get_Star(Dt_Conversion.Rows[i]["Conversation_Ranking"].ToString()) + "</a>" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "<a id='Btn_Delete_Feedback" + i + "' runat='server' style='margin-left:10px;margin-right:10px;' onclick='Delete_Feedback(" + Dt_Conversion.Rows[i]["Trans_ID"].ToString() + "," + Dt_Conversion.Rows[i]["User_ID"].ToString() + "," + Dt_Conversion.Rows[i]["Emp_ID"].ToString() + "," + Dt_Conversion.Rows[i]["RefTblPk"].ToString() + ")' tooltip='Delete' class='text-muted pull-right'><i class='fa fa-trash-o' style='color:red;'></i></a><a id='Btn_Edit_Feedback_" + i + "' runat='server' onclick='Edit_Feedback(" + Dt_Conversion.Rows[i]["Trans_ID"].ToString() + "," + Dt_Conversion.Rows[i]["User_ID"].ToString() + "," + Dt_Conversion.Rows[i]["Emp_ID"].ToString() + "," + Dt_Conversion.Rows[i]["RefTblPk"].ToString() + ",\"" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "\")' tooltip='Edit' class='text-muted pull-right'><i class='fa fa-edit' style='color:red;'></i></a></p></div>";

                            else
                                Conversation = Conversation + "<div class='item'><img src='../Images/default_user_icon.png' alt='user image' class='online'><p class='message'><a href='#' class='name'><small class='text-muted pull-right'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + Convert.ToDateTime(Dt_Conversion.Rows[i]["Modification_Date"]).ToString("dd-MMM-yy HH:MM:ss") + "</small>" + Dt_Conversion.Rows[i]["Emp_Name"].ToString() + Get_Star(Dt_Conversion.Rows[i]["Conversation_Ranking"].ToString()) + "</a>" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "<a id='Btn_Delete_Feedback" + i + "' runat='server' style='margin-left:10px;margin-right:10px;' onclick='Delete_Feedback(" + Dt_Conversion.Rows[i]["Trans_ID"].ToString() + "," + Dt_Conversion.Rows[i]["User_ID"].ToString() + "," + Dt_Conversion.Rows[i]["Emp_ID"].ToString() + "," + Dt_Conversion.Rows[i]["RefTblPk"].ToString() + ")' tooltip='Delete' class='text-muted pull-right'><i class='fa fa-trash-o' style='color:red;'></i></a><a id='Btn_Edit_Feedback_" + i + "' runat='server' onclick='Edit_Feedback(" + Dt_Conversion.Rows[i]["Trans_ID"].ToString() + "," + Dt_Conversion.Rows[i]["User_ID"].ToString() + "," + Dt_Conversion.Rows[i]["Emp_ID"].ToString() + "," + Dt_Conversion.Rows[i]["RefTblPk"].ToString() + ",\"" + Dt_Conversion.Rows[i].ToString().Replace("\n", "<br />") + "\")' tooltip='Edit' class='text-muted pull-right'><i class='fa fa-edit' style='color:red;'></i></a></p></div>";
                        }
                        else
                        {
                            if (Conversation == "")
                                Conversation = "<div class='item'><img src='../Images/default_user_icon.png' alt='user image' class='offline'><p class='message'><a href='#' class='name'><small class='text-muted pull-right'><i class='fa fa-clock-o'></i>" + Convert.ToDateTime(Dt_Conversion.Rows[i]["Modification_Date"]).ToString("dd-MMM-yy HH:MM:ss") + "</small>" + Dt_Conversion.Rows[i]["Emp_Name"].ToString() + Get_Star(Dt_Conversion.Rows[i]["Conversation_Ranking"].ToString()) + "</a>" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "</p></div>";

                            else
                                Conversation = Conversation + "<div class='item'><img src='../Images/default_user_icon.png' alt='user image' class='offline'><p class='message'><a href='#' class='name'><small class='text-muted pull-right'><i class='fa fa-clock-o'></i>" + Convert.ToDateTime(Dt_Conversion.Rows[i]["Modification_Date"]).ToString("dd-MMM-yy HH:MM:ss") + "</small>" + Dt_Conversion.Rows[i]["Emp_Name"].ToString() + Get_Star(Dt_Conversion.Rows[i]["Conversation_Ranking"].ToString()) + "</a>" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "</p></div>";
                        }
                    }
                }
            }
            else
            {
                ratingControl.CurrentRating = 0;
                Conversation = "";
            }
            
            Ltr_Conversion.Text = Conversation;
        }
        catch
        {
        }
    }

    protected void Btn_Send_Feedback_Click(object sender, EventArgs e)
    {
        try
        {
            if (Txt_FeedBack.Text == "")
            {
                DisplayMessage("Enter Feedback");
                return;
            }
            else if (DDL_Status_Conversation.Visible == true && DDL_Status_Conversation.SelectedItem.ToString() == "Status")
            {
                DisplayMessage("Select Status");
                return;
            }
            else
            {
                if (Session["EmpID"].ToString() != Ddl_Employee_List.SelectedValue.ToString())                
                {
                    int c = 0;
                    int b = 0;
                    if (Hdn_Emp_Status.Value == "" && Hdn_TL_Status.Value == "" && Hdn_Trans_Date.Value == "")
                    {
                        b = DutyMaster.Set_Hr_EmpDutyTrans("0", Hdn_Conversation_ID.Value, Session["EmpId"].ToString(), Hdn_Duty_ID.Value, DateTime.Now.ToString("dd-MMM-yyyy"),"", "", Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), "", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "1", Convert.ToDateTime(Hdn_Effect_Date_From.Value).ToString(), Convert.ToDateTime(Hdn_Effect_Date_To.Value).ToString());
                        if (b != 0)
                        {
                            c = DutyMaster.Set_Sys_Feedback("0", Session["UserID"].ToString(), Session["EmpID"].ToString(), "Hr_EmpDutyTrans", b.ToString(), Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");

                            if (c != 0)
                            {
                                Hdn_Feedback_Trans.Value = "";
                                Hdn_Conversation_ID.Value = b.ToString();
                                Hdn_Emp_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                                Hdn_TL_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                                Hdn_Trans_Date.Value = DateTime.Now.ToString();
                              //  DDL_Status_Conversation.SelectedIndex = 0;
                                Txt_FeedBack.Text = "";
                                ratingControl.CurrentRating = 0;
                                Get_Conversation(Lbl_C_Task.Text, Lbl_C_Date.Text, Ltr_C_Description.Text);
                                Txt_FeedBack.Focus();
                            }
                            else
                            {
                                DisplayMessage("Feedback Not Saved");
                            }
                        }
                    }
                    else
                    {
                        b = DutyMaster.Set_Hr_EmpDutyTrans(Hdn_Conversation_ID.Value, "0", Session["EmpId"].ToString(), Hdn_Duty_ID.Value, DateTime.Now.ToString("dd-MMM-yyyy"),"", "", Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), "", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "2", Convert.ToDateTime(Hdn_Effect_Date_From.Value).ToString(), Convert.ToDateTime(Hdn_Effect_Date_To.Value).ToString());
                        if (b != 0)
                        {
                            if (Hdn_Feedback_Trans.Value == "")
                            {
                                c = DutyMaster.Set_Sys_Feedback("0", Session["UserID"].ToString(), Session["EmpID"].ToString(), "Hr_EmpDutyTrans", Hdn_Conversation_ID.Value, Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
                            }
                            else if (Hdn_Feedback_Trans.Value != "" && Hdn_Status.Value == "Edit")
                            {
                                c = DutyMaster.Set_Sys_Feedback(Hdn_Feedback_Trans.Value, Session["UserID"].ToString(), Session["EmpID"].ToString(), "Hr_EmpDutyTrans", Hdn_Conversation_ID.Value, Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                            }
                            if (c != 0)
                            {
                                Hdn_Feedback_Trans.Value = "";
                                //Hdn_Conversation_ID.Value = b.ToString();
                                Hdn_Emp_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                                Hdn_TL_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                                Hdn_Trans_Date.Value = DateTime.Now.ToString();
                               // DDL_Status_Conversation.SelectedIndex = 0;
                                Txt_FeedBack.Text = "";
                                ratingControl.CurrentRating = 0;
                                Get_Conversation(Lbl_C_Task.Text, Lbl_C_Date.Text, Ltr_C_Description.Text);
                                Txt_FeedBack.Focus();
                            }
                            else
                            {
                                DisplayMessage("Feedback Not Saved");
                            }
                        }
                    }
                }
                else if (Session["EmpID"].ToString() == Ddl_Employee_List.SelectedValue.ToString())
                {
                    int c = 0;
                    int b = 0;
                    if (Hdn_Emp_Status.Value == "" && Hdn_TL_Status.Value == "" && Hdn_Trans_Date.Value == "")
                    {
                        b = DutyMaster.Set_Hr_EmpDutyTrans("0", Hdn_Conversation_ID.Value, Ddl_Employee_List.SelectedValue.ToString(), Hdn_Duty_ID.Value, DateTime.Now.ToString("dd-MMM-yyyy"),  Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), "", "", Session["EmpID"].ToString(), "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "1", Convert.ToDateTime(Hdn_Effect_Date_From.Value).ToString(), Convert.ToDateTime(Hdn_Effect_Date_To.Value).ToString());
                        if (b != 0)
                        {
                            c = DutyMaster.Set_Sys_Feedback("0", Session["UserID"].ToString(), Session["EmpId"].ToString(), "Hr_EmpDutyTrans", b.ToString(), Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");

                            if (c != 0)
                            {
                                if (Ddl_Duty_Status.SelectedValue.ToString() == "0")
                                {
                                    Send_Notification_Task(Lbl_C_Task.Text, DDL_Status_Conversation.SelectedItem.ToString(), DateTime.Now.ToString());
                                    Ddl_Duty_Status.SelectedValue = "1";
                                }
                                else
                                {
                                    if (DDL_Status_Conversation.SelectedItem.ToString() == "4")
                                    {
                                        Send_Notification_Task_Competed(Lbl_C_Task.Text, DDL_Status_Conversation.SelectedItem.ToString(), DateTime.Now.ToString());
                                        Ddl_Duty_Status.SelectedValue = "1";
                                    }
                                }
                                Hdn_Feedback_Trans.Value = "";
                                Hdn_Conversation_ID.Value = b.ToString();
                                Hdn_Emp_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                                Hdn_TL_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                                Hdn_Trans_Date.Value = DateTime.Now.ToString();
                               // DDL_Status_Conversation.SelectedIndex = 0;
                                Txt_FeedBack.Text = "";
                                ratingControl.CurrentRating = 0;
                                Get_Conversation(Lbl_C_Task.Text, Lbl_C_Date.Text, Ltr_C_Description.Text);
                                Txt_FeedBack.Focus();
                                if (DDL_Status_Conversation.SelectedValue.ToString() == "4")
                                    Btn_Send_Feedback.Enabled = false;
                            }
                            else
                            {
                                DisplayMessage("Feedback Not Saved");
                            }
                        }
                    }
                    else
                    {
                        b = DutyMaster.Set_Hr_EmpDutyTrans(Hdn_Conversation_ID.Value, "0", Ddl_Employee_List.SelectedValue.ToString(), Hdn_Duty_ID.Value, DateTime.Now.ToString("dd-MMM-yyyy"), Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), "", "", Session["EmpID"].ToString(), "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "3", Convert.ToDateTime(Hdn_Effect_Date_From.Value).ToString(), Convert.ToDateTime(Hdn_Effect_Date_To.Value).ToString());
                        if (b != 0)
                        {
                            if (Hdn_Feedback_Trans.Value == "")
                            {
                                c = DutyMaster.Set_Sys_Feedback("0", Session["UserID"].ToString(), Session["EmpId"].ToString(), "Hr_EmpDutyTrans", Hdn_Conversation_ID.Value, Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
                            }
                            else if (Hdn_Feedback_Trans.Value != "" && Hdn_Status.Value == "Edit")
                            {
                                c = DutyMaster.Set_Sys_Feedback(Hdn_Feedback_Trans.Value, Session["UserID"].ToString(), Session["EmpId"].ToString(), "Hr_EmpDutyTrans", Hdn_Conversation_ID.Value, Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                            }
                            if (c != 0)
                            {
                                if (Ddl_Duty_Status.SelectedValue.ToString() == "0")
                                {
                                    Send_Notification_Task(Lbl_C_Task.Text, DDL_Status_Conversation.SelectedItem.ToString(), DateTime.Now.ToString());
                                    Ddl_Duty_Status.SelectedValue = "1";
                                }
                                else
                                {
                                    if (DDL_Status_Conversation.SelectedItem.ToString() == "4")
                                    {
                                        Send_Notification_Task_Competed(Lbl_C_Task.Text, DDL_Status_Conversation.SelectedItem.ToString(), DateTime.Now.ToString());
                                        Ddl_Duty_Status.SelectedValue = "1";
                                    }
                                }
                                Hdn_Feedback_Trans.Value = "";
                                //Hdn_Conversation_ID.Value = b.ToString();
                                Hdn_Emp_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                                Hdn_TL_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                                Hdn_Trans_Date.Value = DateTime.Now.ToString();
                                //DDL_Status_Conversation.SelectedIndex = 0;
                                Txt_FeedBack.Text = "";
                                ratingControl.CurrentRating = 0;
                                Get_Conversation(Lbl_C_Task.Text, Lbl_C_Date.Text, Ltr_C_Description.Text);
                                Txt_FeedBack.Focus();
                                if (DDL_Status_Conversation.SelectedValue.ToString() == "4")
                                    Btn_Send_Feedback.Enabled = false;
                            }
                            else
                            {
                                DisplayMessage("Feedback Not Saved");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
       
    }
    
    protected void Get_Conversation(string C_Task, string C_Date, string C_Description)
    {
        try
        {
            if(Ddl_Employee_List.SelectedValue.ToString()!=Session["EmpID"].ToString())
            {
                if (DDL_Status_Conversation.SelectedValue.ToString() == "4" && Ddl_Duty_Status.SelectedValue.ToString() == "1")
                {
                    DDL_Status_Conversation.Enabled = false;
                    Txt_FeedBack.Enabled = false;
                    Btn_Send_Feedback.Enabled = false;
                }
                else
                {
                    DDL_Status_Conversation.Enabled = true;
                    Txt_FeedBack.Enabled = true;
                    Btn_Send_Feedback.Enabled = true;
                }
            }

            Lbl_C_Task.Text = C_Task;
            Lbl_C_Date.Text = C_Date;
            Ltr_C_Description.Text = C_Description;
            string Conversation = string.Empty;
            DataTable Dt_Conversion = DutyMaster.Get_Sys_Feedback("0", "0", "0", "0", Hdn_Conversation_ID.Value, "0", "", "0", "", "", "", "", "", "True", "0", "0", "1");
            if (Dt_Conversion.Rows.Count > 0)
            {
                for (int i = 0; i < Dt_Conversion.Rows.Count; i++)
                {
                    if (Dt_Conversion.Rows[i]["Emp_Name"].ToString() == "")
                        Dt_Conversion.Rows[i]["Emp_Name"] = "Superadmin";

                    if (Dt_Conversion.Rows[i]["Emp_ID"].ToString() == Session["EmpID"].ToString())
                    {
                        if (Conversation == "")
                            Conversation = "<div class='item'><img src='../Images/default_user_icon.png' alt='user image' class='online'><p class='message'><a href='#' class='name'><small class='text-muted pull-right'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + Convert.ToDateTime(Dt_Conversion.Rows[i]["Modification_Date"]).ToString("dd-MMM-yy HH:MM:ss") + "</small>" + Dt_Conversion.Rows[i]["Emp_Name"].ToString() + Get_Star(Dt_Conversion.Rows[i]["Conversation_Ranking"].ToString()) + "</a>" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "<a id='Btn_Delete_Feedback" + i + "' runat='server' style='margin-left:10px;margin-right:10px;' onclick='Delete_Feedback(" + Dt_Conversion.Rows[i]["Trans_ID"].ToString() + "," + Dt_Conversion.Rows[i]["User_ID"].ToString() + "," + Dt_Conversion.Rows[i]["Emp_ID"].ToString() + "," + Dt_Conversion.Rows[i]["RefTblPk"].ToString() + ")' tooltip='Delete' class='text-muted pull-right'><i class='fa fa-trash-o' style='color:red;'></i></a><a id='Btn_Edit_Feedback_" + i + "' runat='server' onclick='Edit_Feedback(" + Dt_Conversion.Rows[i]["Trans_ID"].ToString() + "," + Dt_Conversion.Rows[i]["User_ID"].ToString() + "," + Dt_Conversion.Rows[i]["Emp_ID"].ToString() + "," + Dt_Conversion.Rows[i]["RefTblPk"].ToString() + ",\"" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "\")' tooltip='Edit' class='text-muted pull-right'><i class='fa fa-edit' style='color:red;'></i></a></p></div>";

                        else
                            Conversation = Conversation + "<div class='item'><img src='../Images/default_user_icon.png' alt='user image' class='online'><p class='message'><a href='#' class='name'><small class='text-muted pull-right'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + Convert.ToDateTime(Dt_Conversion.Rows[i]["Modification_Date"]).ToString("dd-MMM-yy HH:MM:ss") + "</small>" + Dt_Conversion.Rows[i]["Emp_Name"].ToString() + Get_Star(Dt_Conversion.Rows[i]["Conversation_Ranking"].ToString()) + "</a>" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "<a id='Btn_Delete_Feedback" + i + "' runat='server' style='margin-left:10px;margin-right:10px;' onclick='Delete_Feedback(" + Dt_Conversion.Rows[i]["Trans_ID"].ToString() + "," + Dt_Conversion.Rows[i]["User_ID"].ToString() + "," + Dt_Conversion.Rows[i]["Emp_ID"].ToString() + "," + Dt_Conversion.Rows[i]["RefTblPk"].ToString() + ")' tooltip='Delete' class='text-muted pull-right'><i class='fa fa-trash-o' style='color:red;'></i></a><a id='Btn_Edit_Feedback_" + i + "' runat='server' onclick='Edit_Feedback(" + Dt_Conversion.Rows[i]["Trans_ID"].ToString() + "," + Dt_Conversion.Rows[i]["User_ID"].ToString() + "," + Dt_Conversion.Rows[i]["Emp_ID"].ToString() + "," + Dt_Conversion.Rows[i]["RefTblPk"].ToString() + ",\"" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "\")' tooltip='Edit' class='text-muted pull-right'><i class='fa fa-edit' style='color:red;'></i></a></p></div>";
                    }
                    else
                    {
                        if (Conversation == "")
                            Conversation = "<div class='item'><img src='../Images/default_user_icon.png' alt='user image' class='offline'><p class='message'><a href='#' class='name'><small class='text-muted pull-right'><i class='fa fa-clock-o'></i>" + Convert.ToDateTime(Dt_Conversion.Rows[i]["Modification_Date"]).ToString("dd-MMM-yy HH:MM:ss") + "</small>" + Dt_Conversion.Rows[i]["Emp_Name"].ToString() + Get_Star(Dt_Conversion.Rows[i]["Conversation_Ranking"].ToString()) + "</a>" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "</p></div>";
                        else
                            Conversation = Conversation + "<div class='item'><img src='../Images/default_user_icon.png' alt='user image' class='offline'><p class='message'><a href='#' class='name'><small class='text-muted pull-right'><i class='fa fa-clock-o'></i>" + Convert.ToDateTime(Dt_Conversion.Rows[i]["Modification_Date"]).ToString("dd-MMM-yy HH:MM:ss") + "</small>" + Dt_Conversion.Rows[i]["Emp_Name"].ToString() + Get_Star(Dt_Conversion.Rows[i]["Conversation_Ranking"].ToString()) + "</a>" + Dt_Conversion.Rows[i]["Feedback"].ToString().Replace("\n", "<br />") + "</p></div>";
                    }
                }
            }
            Ltr_Conversion.Text = Conversation;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "On_Load_Modal()", true);
        }
        catch
        {
        }
    }

    protected void Img_Search_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            Hdn_Select_Emp_ID.Value = Ddl_Employee_List.SelectedValue.ToString();
            Session["Feed_Dt_Task_List_Active"] = null;
            DataTable Dt_Emp_Duties_Filter = DutyMaster.Get_Hr_EmpDutyTrans("0", Session["UserID"].ToString(), Ddl_Employee_List.SelectedValue.ToString(), "0", DateTime.Now.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
            string Condition = string.Empty;
            
            if(Session["EmpID"].ToString()== Hdn_Select_Emp_ID.Value)
            {
                if (Ddl_Employee_List.SelectedItem.ToString() != "--Select--" && Txt_Start_Date_List.Text != "" && Txt_Due_Date_List.Text != "")
                {
                    Condition = "Employee_ID = '" + Ddl_Employee_List.SelectedValue.ToString() + "' and ((Effect_Date_From >= '" + Convert.ToDateTime(Txt_Start_Date_List.Text).ToString("dd MMM yyyy") + "' and Effect_Date_From <= '" + Convert.ToDateTime(Txt_Due_Date_List.Text).ToString("dd MMM yyyy") + "') or (Effect_Date_To >= '" + Convert.ToDateTime(Txt_Start_Date_List.Text).ToString("dd MMM yyyy") + "' and Effect_Date_To <= '" + Convert.ToDateTime(Txt_Due_Date_List.Text).ToString("dd MMM yyyy") + "'))";
                }
                else if (Ddl_Employee_List.SelectedItem.ToString() != "--Select--" && Txt_Start_Date_List.Text == "" && Txt_Due_Date_List.Text == "")
                {
                    Condition = "Employee_ID = '" + Ddl_Employee_List.SelectedValue.ToString() + "'";
                }
            }
            else if (Session["EmpID"].ToString() != Hdn_Select_Emp_ID.Value)
            {
                if (Ddl_Employee_List.SelectedItem.ToString() != "--Select--" && Txt_Start_Date_List.Text != "" && Txt_Due_Date_List.Text != "")
            {
                Condition = "Employee_ID = '" + Ddl_Employee_List.SelectedValue.ToString() + "' and Report_To LIKE '%" + Session["UserID"].ToString() + "%' and ((Effect_Date_From >= '" + Convert.ToDateTime(Txt_Start_Date_List.Text).ToString("dd MMM yyyy") + "' and Effect_Date_From <= '" + Convert.ToDateTime(Txt_Due_Date_List.Text).ToString("dd MMM yyyy") + "') or (Effect_Date_To >= '" + Convert.ToDateTime(Txt_Start_Date_List.Text).ToString("dd MMM yyyy") + "' and Effect_Date_To <= '" + Convert.ToDateTime(Txt_Due_Date_List.Text).ToString("dd MMM yyyy") + "'))";
            }
            else if (Ddl_Employee_List.SelectedItem.ToString() != "--Select--" && Txt_Start_Date_List.Text == "" && Txt_Due_Date_List.Text == "")
            {
                Condition = "Employee_ID = '" + Ddl_Employee_List.SelectedValue.ToString() + "' and Report_To LIKE '%" + Session["UserID"].ToString() + "%'";
            }
            }

            Dt_Emp_Duties_Filter = new DataView(Dt_Emp_Duties_Filter, Condition, "", DataViewRowState.CurrentRows).ToTable();
            Session["Feed_Dt_Task_List_Active"] = Dt_Emp_Duties_Filter;
            Ddl_Duty_Status_SelectedIndexChanged(null, null);
            if (Ddl_Employee_List.SelectedValue.ToString() == Session["EmpID"].ToString())
            {
                Ddl_Duty_Status.Visible = false;
                Lbl_Duty_Status.Visible = false;
            }
            else
            {
                Ddl_Duty_Status.Visible = true;
                Lbl_Duty_Status.Visible = true;
            }
            //Fill_Gv_Feedback(Dt_Emp_Duties_Filter);
        }
        catch
        {
        }
    }
    
    public string GetCycle(object obj)
    {
        string Cycle = string.Empty;
        if (obj.ToString() == "")
            Cycle = "";
        else if (obj.ToString() == "1")
            Cycle = "Daily";
        else if (obj.ToString() == "2")
            Cycle = "Weekly";
        else if (obj.ToString() == "3")
            Cycle = "Biweekly";
        else if (obj.ToString() == "4")
            Cycle = "Monthly";
        else if (obj.ToString() == "5")
            Cycle = "Quarterly";
        else if (obj.ToString() == "6")
            Cycle = "Half Yearly";
        else if (obj.ToString() == "7")
            Cycle = "Yearly";
        return Cycle;
    }
    
    protected void Btn_Delete_Feedback_Click(object sender, EventArgs e)
    {
        try
        {
            int c = 0;
            if (Hdn_Feedback_Trans.Value != "" && Hdn_Status.Value == "Delete")
            {
                c = DutyMaster.Set_Sys_Feedback(Hdn_Feedback_Trans.Value, Session["UserID"].ToString(), Session["EmpID"].ToString(), "Hr_EmpDutyTrans", "0", Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");
            }
            if (c != 0)
            {
                //Hdn_Feedback_Trans.Value = "";
                //Hdn_Status.Value = "";
                ////DDL_Status_Conversation.SelectedIndex = 0;
                //Txt_FeedBack.Text = "";
                //ratingControl.CurrentRating = 0;
                //DisplayMessage("Feedback Deleted");
                //Get_Conversation(Lbl_C_Task.Text, Lbl_C_Date.Text, Ltr_C_Description.Text);


                Hdn_Feedback_Trans.Value = "";
                //Hdn_Conversation_ID.Value = b.ToString();
                Hdn_Emp_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                Hdn_TL_Status.Value = DDL_Status_Conversation.SelectedItem.ToString();
                Hdn_Trans_Date.Value = DateTime.Now.ToString();
                // DDL_Status_Conversation.SelectedIndex = 0;
                Txt_FeedBack.Text = "";
                ratingControl.CurrentRating = 0;
                Get_Conversation(Lbl_C_Task.Text, Lbl_C_Date.Text, Ltr_C_Description.Text);
                Txt_FeedBack.Focus();
                DisplayMessage("Feedback Deleted");
            }
            else
            {
                DisplayMessage("Feedback Not Deleted");
            }
        }
        catch (Exception ex)
        {
        }
    }
    
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlFieldName.SelectedValue.ToString() == "Duty_Cycle")
            {
                txtValue.Text = "";
                Ddl_Duty_Cycle.SelectedValue = "0";

                txtValue.Visible = false;
                btnbind.Visible = false;

                Ddl_Duty_Cycle.Visible = true;
                Btn_Bind_Duty_Cycle.Visible = true;
            }
            else
            {
                txtValue.Text = "";
                Ddl_Duty_Cycle.SelectedValue = "0";

                txtValue.Visible = true;
                btnbind.Visible = true;

                Ddl_Duty_Cycle.Visible = false;
                Btn_Bind_Duty_Cycle.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
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
                DataTable Dt_Task_List = new DataTable();
                if (Session["EmpID"].ToString() == Ddl_Employee_List.SelectedValue.ToString())
                {
                    if (Session["Feed_Dt_Task_List_Pending"] != null)
                        Dt_Task_List = Session["Feed_Dt_Task_List_Pending"] as DataTable;
                }
                else if (Session["EmpID"].ToString() != Ddl_Employee_List.SelectedValue.ToString())
                {
                    if (Session["Feed_Dt_Task_List_Completed"] != null)
                        Dt_Task_List = Session["Feed_Dt_Task_List_Completed"] as DataTable;
                    else if (Session["Feed_Dt_Task_List_Pending"] != null)
                        Dt_Task_List = Session["Feed_Dt_Task_List_Pending"] as DataTable;
                }

                DataView view = new DataView(Dt_Task_List, condition, "", DataViewRowState.CurrentRows);
                Fill_Gv_Feedback(view.ToTable());
                txtValue.Focus();
            }
        }
        catch
        {
        }
    }

    protected void Btn_Bind_Duty_Cycle_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            if (ddlOption.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + Ddl_Duty_Cycle.SelectedValue.ToString() + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + Ddl_Duty_Cycle.SelectedValue.ToString() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + Ddl_Duty_Cycle.SelectedValue.ToString() + "%'";
                }
                DataTable Dt_Task_List = new DataTable();
                if (Session["EmpID"].ToString() == Ddl_Employee_List.SelectedValue.ToString())
                {
                    if (Session["Feed_Dt_Task_List_Pending"] != null)
                        Dt_Task_List = Session["Feed_Dt_Task_List_Pending"] as DataTable;
                }
                else if (Session["EmpID"].ToString() != Ddl_Employee_List.SelectedValue.ToString())
                {
                    if (Session["Feed_Dt_Task_List_Completed"] != null)
                        Dt_Task_List = Session["Feed_Dt_Task_List_Completed"] as DataTable;
                    else if (Session["Feed_Dt_Task_List_Pending"] != null)
                        Dt_Task_List = Session["Feed_Dt_Task_List_Pending"] as DataTable;
                }

                DataView view = new DataView(Dt_Task_List, condition, "", DataViewRowState.CurrentRows);
                Fill_Gv_Feedback(view.ToTable());
                Ddl_Duty_Cycle.Focus();
            }
        }
        catch
        {
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            txtValue.Text = "";
            Ddl_Duty_Cycle.SelectedValue = "0";

            txtValue.Visible = true;
            btnbind.Visible = true;

            Ddl_Duty_Cycle.Visible = false;
            Btn_Bind_Duty_Cycle.Visible = false;

            ddlFieldName.SelectedIndex = 0;
            ddlOption.SelectedIndex = 2;

            if(Session["EmpID"].ToString()==Ddl_Employee_List.SelectedValue.ToString())
            {
                if (Session["Feed_Dt_Task_List_Pending"] != null)
                    Fill_Gv_Feedback(Session["Feed_Dt_Task_List_Pending"] as DataTable);
            }
            else if (Session["EmpID"].ToString() != Ddl_Employee_List.SelectedValue.ToString())
            {
                if (Session["Feed_Dt_Task_List_Completed"] != null)
                    Fill_Gv_Feedback(Session["Feed_Dt_Task_List_Completed"] as DataTable);
                else if (Session["Feed_Dt_Task_List_Pending"] != null)
                    Fill_Gv_Feedback(Session["Feed_Dt_Task_List_Pending"] as DataTable);
            }

            
        }
        catch
        {
        }
    }

    public string GetEmployeeName(string EmployeeId)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
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

    protected void Send_Notification_Task(string Title, string Status, string Date)
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        Dt_Request_Type = Obj_Notifiacation.Get_Notification_ID("2");
        string Request_URL = "../Duty_Master/Duty_Feedback.aspx";
        string Message = string.Empty;
        Message = "Duty Status Update By '" + GetEmployeeName(Session["EmpId"].ToString()) + "'  of Title - '" + Title + "' And Current Status is '" + Status + "' On Date '" + Date + "'";
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Hdn_Select_Emp_ID.Value, Hdn_Select_Emp_ID.Value, Message, Dt_Request_Type.Rows[0]["Trans_ID"].ToString(), Request_URL, "", "0", "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "17");
    }

    protected void Send_Notification_Task_Competed(string Title, string Status, string Date)
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        Dt_Request_Type = Obj_Notifiacation.Get_Notification_ID("2");
        string Request_URL = "../Duty_Master/Duty_Feedback.aspx";
        string Message = string.Empty;
        Message = "Duty is Competed By '" + GetEmployeeName(Session["EmpId"].ToString()) + "'  of Title - '" + Title + "' And Current Status is '" + Status + "' On Date '" + Date + "'";
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Hdn_Select_Emp_ID.Value, Hdn_Select_Emp_ID.Value, Message, Dt_Request_Type.Rows[0]["Trans_ID"].ToString(), Request_URL, "", "0", "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "17");
    }

    protected void Gv_Feedback_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Gv_Feedback_List.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["EmpID"].ToString() == Ddl_Employee_List.SelectedValue.ToString())
            {
                if (Session["Feed_Dt_Task_List_Pending"] != null)
                    dt = Session["Feed_Dt_Task_List_Pending"] as DataTable;
            }
            else if (Session["EmpID"].ToString() != Ddl_Employee_List.SelectedValue.ToString())
            {
                if (Session["Feed_Dt_Task_List_Completed"] != null)
                    dt = Session["Feed_Dt_Task_List_Completed"] as DataTable;
                else if (Session["Feed_Dt_Task_List_Pending"] != null)
                    dt = Session["Feed_Dt_Task_List_Pending"] as DataTable;
            }

            objPageCmn.FillData((object)Gv_Feedback_List, dt, "", "");
          
            Gv_Feedback_List.HeaderRow.Focus();
        }
        catch
        {
        }
    }
    
    protected void Gv_Feedback_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            if (Session["EmpID"].ToString() == Ddl_Employee_List.SelectedValue.ToString())
            {
                if (Session["Feed_Dt_Task_List_Pending"] != null)
                    dt = Session["Feed_Dt_Task_List_Pending"] as DataTable;
            }
            else if (Session["EmpID"].ToString() != Ddl_Employee_List.SelectedValue.ToString())
            {
                if (Session["Feed_Dt_Task_List_Completed"] != null)
                    dt = Session["Feed_Dt_Task_List_Completed"] as DataTable;
                else if (Session["Feed_Dt_Task_List_Pending"] != null)
                    dt = Session["Feed_Dt_Task_List_Pending"] as DataTable;
            }

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
            objPageCmn.FillData((object)Gv_Feedback_List, dt, "", "");
         
            Gv_Feedback_List.HeaderRow.Focus();
        }
        catch
        {
        }
    }
    
    protected string Get_Star(string rank)
    {
        string star = string.Empty;
        if (rank == "0")
            star = "&emsp;<span class='fa fa-star'></span><span class='fa fa-star'></span><span class='fa fa-star'></span><span class='fa fa-star'></span><span class='fa fa-star'></span>";
        else if (rank == "1")
            star = "&emsp;<span class='fa fa-star Selected'></span><span class='fa fa-star'></span><span class='fa fa-star'></span><span class='fa fa-star'></span><span class='fa fa-star'></span>";
        else if (rank == "2")
            star = "&emsp;<span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span><span class='fa fa-star'></span><span class='fa fa-star'></span><span class='fa fa-star'></span>";
        else if (rank == "3")
            star = "&emsp;<span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span><span class='fa fa-star'></span><span class='fa fa-star'></span>";
        else if (rank == "4")
            star = "&emsp;<span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span><span class='fa fa-star'></span>";
        else if (rank == "5")
            star = "&emsp;<span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span><span class='fa fa-star Selected'></span>";
        return star;
    }

    protected void ratingControl_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "On_Load_Modal()", true);
    }

    protected void Ddl_Duty_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["EmpID"].ToString() == Ddl_Employee_List.SelectedValue.ToString())
        {
            DataTable Dt_Pending = Session["Feed_Dt_Task_List_Active"] as DataTable;
            if (Dt_Pending != null)
            {
                if (Dt_Pending.Rows.Count > 0)
                {
                    string Condition = "Emp_Status is null or Emp_Status = '' or Emp_Status = 'Not Started' or Emp_Status = 'Deferred' or Emp_Status = 'In-Progress' or Emp_Status = 'Waiting For Input'";
                    Dt_Pending = new DataView(Dt_Pending, Condition, "", DataViewRowState.CurrentRows).ToTable();
                    Session["Feed_Dt_Task_List_Pending"] = Dt_Pending;
                    Fill_Gv_Feedback(Dt_Pending);
                }
                else
                {
                    Session["Feed_Dt_Task_List_Pending"] = null;
                    Fill_Gv_Feedback(Session["Feed_Dt_Task_List_Pending"] as DataTable);
                }
            }
            else
            {
                Session["Feed_Dt_Task_List_Pending"] = null;
                Fill_Gv_Feedback(Session["Feed_Dt_Task_List_Pending"] as DataTable);
            }
        }
        else if (Session["EmpID"].ToString() != Ddl_Employee_List.SelectedValue.ToString())
        {
            if (Ddl_Duty_Status.SelectedValue.ToString() == "0")
            {
                //Completed
                DataTable Dt_Completed = Session["Feed_Dt_Task_List_Active"] as DataTable;
                if (Dt_Completed != null)
                {
                    if (Dt_Completed.Rows.Count > 0)
                    {
                        string Condition = "TL_Status = 'Completed'";
                        Dt_Completed = new DataView(Dt_Completed, Condition, "", DataViewRowState.CurrentRows).ToTable();
                        Session["Feed_Dt_Task_List_Completed"] = Dt_Completed;
                        Fill_Gv_Feedback(Dt_Completed);
                    }
                    else
                    {
                        Session["Feed_Dt_Task_List_Completed"] = null;
                        Fill_Gv_Feedback(Session["Feed_Dt_Task_List_Completed"] as DataTable);
                    }
                }
                else
                {
                    Session["Feed_Dt_Task_List_Completed"] = null;
                    Fill_Gv_Feedback(Session["Feed_Dt_Task_List_Completed"] as DataTable);
                }
            }
            else if (Ddl_Duty_Status.SelectedValue.ToString() == "1")
            {
                //Pending
                DataTable Dt_Pending = Session["Feed_Dt_Task_List_Active"] as DataTable;
                if (Dt_Pending != null)
                {
                    if (Dt_Pending.Rows.Count > 0)
                    {
                        string Condition = "TL_Status is null or TL_Status = '' or TL_Status = 'Not Started' or TL_Status = 'Deferred' or TL_Status = 'In-Progress' or TL_Status = 'Waiting For Input'";
                        Dt_Pending = new DataView(Dt_Pending, Condition, "", DataViewRowState.CurrentRows).ToTable();
                        Session["Feed_Dt_Task_List_Pending"] = Dt_Pending;
                        Fill_Gv_Feedback(Dt_Pending);
                    }
                    else
                    {
                        Session["Feed_Dt_Task_List_Pending"] = null;
                        Fill_Gv_Feedback(Session["Feed_Dt_Task_List_Pending"] as DataTable);
                    }
                }
                else
                {
                    Session["Feed_Dt_Task_List_Pending"] = null;
                    Fill_Gv_Feedback(Session["Feed_Dt_Task_List_Pending"] as DataTable);
                }
            }
        }
    }

    //protected void Gv_Feedback_List_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
        //if (Gv_Feedback_List != null)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        System.Data.DataRow row = ((System.Data.DataRowView)e.Row.DataItem).Row;
        //        if (row["Emp_Status"].ToString() == "Completed" && row["TL_Status"].ToString() == "")
        //            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA8072");
        //    }
        //}
    //}
}