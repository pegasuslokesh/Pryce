using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Duty_Master_Task : System.Web.UI.Page
{
    NotificationMaster Obj_Notifiacation = null;
    EmployeeMaster objEmp = null;
    Common cmn = null;
    TaskMaster TaskMaster = null;
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
            objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
            objSys = new SystemParameter(Session["DBConnection"].ToString());
            objDesg = new DesignationMaster(Session["DBConnection"].ToString());
            ObjComman = new Common(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

            Page.Title = objSys.GetSysTitle();
            AllPageCode();
            if (!IsPostBack)
            {
                Hdn_Contact_Id.Value = "0";
                Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
                if (Chk_Page_.CheckPagePermission(Session["UserID"].ToString(), "375", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                try
                {
                    Ddl_Employee_New.Items.Insert(0, "--Select--");
                    Ddl_Employee_New.SelectedIndex = 0;
                    Txt_Contact.Text = "";
                    Txt_Start_Date.Text = "";
                    Txt_Due_Date.Text = "";
                    Txt_Title.Text = "";
                    Editor_Description.Content = "";
                }
                catch
                {
                    Ddl_Employee_New.Items.Insert(0, "--Select--");
                    Ddl_Employee_New.SelectedIndex = 0;
                    Txt_Contact.Text = "";
                    Txt_Start_Date.Text = "";
                    Txt_Due_Date.Text = "";
                    Txt_Title.Text = "";
                    Editor_Description.Content = "";
                }
                Get_Employee();
                Fill_Grid_List();
            }
            CalendarExtender1.Format = objSys.SetDateFormat();
            CalendarExtender2.Format = objSys.SetDateFormat();
            CalendarExtender3.Format = objSys.SetDateFormat();
            CalendarExtender4.Format = objSys.SetDateFormat();
            CalendarExtender5.Format = objSys.SetDateFormat();
            CalendarExtender6.Format = objSys.SetDateFormat();
        }
        catch
        {
        }
    }

    public void AllPageCode()
    {
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("375", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        Page.Title = objSys.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpID"].ToString() == "0")
        {
            Btn_Save.Visible = true;
            Img_Emp_List_Select_All.Visible = true;
            Img_Emp_List_Delete_All.Visible = true;
            ratingControl.Visible = true;
            DDL_Status_Conversation.Visible = true;
            RequiredFieldValidator5.ValidationGroup = "Send";
            hdnCanEdit.Value = "true";
            hdnCanDelete.Value = "true";
            Img_Emp_List_Active.Visible = true;
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserID"].ToString(), strModuleId, "375",Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() != "SuperAdmin")
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (DtRow["Op_Id"].ToString() == "1")
                    {
                        Btn_Save.Visible = true;
                        continue;
                    }

                    if (DtRow["Op_Id"].ToString() == "2")
                    {
                        if (Session["EmpID"].ToString() != Ddl_Employee_List.SelectedValue.ToString())
                        {
                            hdnCanEdit.Value = "true";
                        }
                        continue;
                    }
                    if (DtRow["Op_Id"].ToString() == "3")
                    {
                        if (Session["EmpID"].ToString() != Ddl_Employee_List.SelectedValue.ToString())
                        {
                            hdnCanDelete.Value = "true";
                            Img_Emp_List_Select_All.Visible = true;
                            Img_Emp_List_Delete_All.Visible = true;
                            ratingControl.Visible = true;
                            DDL_Status_Conversation.Visible = true;
                            RequiredFieldValidator5.ValidationGroup = "Send";
                        }
                        continue;
                    }
                    if (DtRow["Op_Id"].ToString() == "4")
                    {
                        Img_Emp_List_Active.Visible = true;
                        continue;
                    }
                }
            }
        }
    }

    public void DisplayMessage(string str, string color = "orange")
    {
        try
        {
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
            }
            if (Session["lang"].ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
            }
            else if (Session["lang"].ToString() == "2")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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
            DataTable Dt_Employee = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());
            Dt_Employee = new DataView(Dt_Employee, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["EmpID"].ToString() != "0")
            {
                Dt_Employee = new DataView(Dt_Employee, "Emp_Id in (" + GetTlList() + ")", "Emp_Name", DataViewRowState.CurrentRows).ToTable();
            }
            Fill_DDL_Employee(Dt_Employee);
        }
        catch (Exception ex)
        {
        }
    }

    public string GetTlList()
    {
        DataTable dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_id='" + Session["EmpID"].ToString() + "' or Field5='" + Session["EmpID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            if (Session["T_EmpList"] == null)
            {
                Session["T_EmpList"] = dt.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                Session["T_EmpList"] = Session["T_EmpList"].ToString() + "," + dt.Rows[i]["Emp_Id"].ToString();
            }
            FillChild(dt.Rows[i]["Emp_Id"].ToString());
            i++;
        }
        return Session["T_EmpList"].ToString();
    }

    private void FillChild(string index)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());
            dt = new DataView(dt, "Field5=" + index + "", "", DataViewRowState.CurrentRows).ToTable();
            int i = 0;
            while (i < dt.Rows.Count)
            {
                if (Session["T_EmpList"] == null)
                {
                    Session["T_EmpList"] = dt.Rows[i]["Emp_Id"].ToString();
                }
                else
                {
                    Session["T_EmpList"] = Session["T_EmpList"].ToString() + "," + dt.Rows[i]["Emp_Id"].ToString();
                }
                FillChild(dt.Rows[i]["Emp_Id"].ToString());
                i++;
            }
        }
        catch
        {
        }
    }

    protected void Fill_DDL_Employee(DataTable Dt_Employee)
    {
        try
        {
            Ddl_Employee_New.Items.Clear();
            Ddl_Employee_List.Items.Clear();
            if (Dt_Employee.Rows.Count > 0)
            {
                Ddl_Employee_List.DataSource = null;
                Ddl_Employee_List.DataBind();
                Ddl_Employee_List.DataSource = Dt_Employee;
                Ddl_Employee_List.DataTextField = "Emp_Name";
                Ddl_Employee_List.DataValueField = "Emp_Id";
                Ddl_Employee_List.DataBind();
                Ddl_Employee_List.Items.Insert(0, "--Select--");

                DataTable Dt = Dt_Employee = new DataView(Dt_Employee, "Field5=" + Session["EmpID"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {
                    Session["T_Tl_Team_Emp"] = Dt;
                    Gv_Feedback_List.Columns[0].Visible = true;
                    Gv_Feedback_List.Columns[1].Visible = true;
                    Gv_Feedback_List.Columns[2].Visible = true;
                    Li_New.Visible = true;
                    Li_Bin.Visible = true;
                    ratingControl.Visible = true;
                    RequiredFieldValidator5.ValidationGroup = "Send";
                    DDL_Status_Conversation.Visible = true;
                    Ddl_Employee_New.DataSource = null;
                    Ddl_Employee_New.DataBind();
                    Ddl_Employee_New.DataSource = Dt_Employee;
                    Ddl_Employee_New.DataTextField = "Emp_Name";
                    Ddl_Employee_New.DataValueField = "Emp_Id";
                    Ddl_Employee_New.DataBind();
                    Ddl_Employee_New.Items.Insert(0, "--Select--");
                }
                else
                {
                    //Gv_Feedback_List.Columns[0].Visible = false;
                    //Gv_Feedback_List.Columns[1].Visible = false;
                    Gv_Feedback_List.Columns[2].Visible = false;
                    Li_Bin.Visible = false;
                    Li_New.Visible = false;
                    ratingControl.Visible = false;
                    RequiredFieldValidator5.ValidationGroup = "False";
                    DDL_Status_Conversation.Visible = false;
                    Ddl_Employee_New.Items.Insert(0, "--Select--");
                }
                if (Session["EmpID"].ToString() != "0")
                {
                    //Ddl_Employee_New.SelectedValue = Session["EmpID"].ToString();
                    Ddl_Employee_List.SelectedValue = Session["EmpID"].ToString();
                    Emp_List_ID.Value = Session["EmpID"].ToString();
                }
                else
                {
                    Emp_List_ID.Value = Ddl_Employee_List.SelectedValue.ToString();
                }
            }
            else
            {
                try
                {
                    //Img_Emp_List_Select_All.Visible = false;
                    //Img_Emp_List_Delete_All.Visible = false;
                    Emp_List_ID.Value = "0";

                    Ddl_Employee_New.Items.Insert(0, "--Select--");
                    Ddl_Employee_New.SelectedIndex = 0;

                    Ddl_Employee_List.Items.Insert(0, "--Select--");
                    Ddl_Employee_List.SelectedIndex = 0;
                }
                catch
                {
                    //Img_Emp_List_Select_All.Visible = false;
                    //Img_Emp_List_Delete_All.Visible = false;
                    Emp_List_ID.Value = "0";

                    Ddl_Employee_New.Items.Insert(0, "--Select--");
                    Ddl_Employee_New.SelectedIndex = 0;

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
            DataTable Dt_Task_List = TaskMaster.Get_Hr_EmpTask("0", Session["CompId"].ToString(), "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "", "", "", "0", "0", "True", "", "", "1");
            Session["T_Dt_Task_List"] = Dt_Task_List;
            Dt_Task_List = new DataView(Dt_Task_List, "Assign_To = '" + Session["EmpID"].ToString() + "' And Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();
            Session["T_Dt_Task_List_Active"] = Dt_Task_List;
            if (Dt_Task_List.Rows.Count > 0)
            {
                Fill_Gv_Feedback(Dt_Task_List);
            }
            else
            {
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
               // Gv_Feedback_List.Columns[0].Visible = false;
                //Gv_Feedback_List.Columns[1].Visible = false;
                Gv_Feedback_List.Columns[2].Visible = false;
                AllPageCode();
                Gv_Feedback_List.DataSource = Dt_Grid;
                Gv_Feedback_List.DataBind();
                Img_Emp_List_Select_All.Visible = false;
                Img_Emp_List_Delete_All.Visible = false;
                ratingControl.Visible = false;
                DDL_Status_Conversation.Visible = false;
                RequiredFieldValidator5.ValidationGroup = "False";
            }
            else
            {
                Gv_Feedback_List.DataSource = Dt_Grid;
                Gv_Feedback_List.DataBind();
                Img_Emp_List_Select_All.Visible = false;
                Img_Emp_List_Delete_All.Visible = false;
                ratingControl.Visible = false;
                DDL_Status_Conversation.Visible = false;
                RequiredFieldValidator5.ValidationGroup = "False";
            }
        }
        else
        {
            if (Dt_Grid.Rows.Count > 0)
            {
                Gv_Feedback_List.Columns[0].Visible = true;
                Gv_Feedback_List.Columns[1].Visible = true;
                Gv_Feedback_List.Columns[2].Visible = true;
                AllPageCode();
                Gv_Feedback_List.DataSource = Dt_Grid;
                Gv_Feedback_List.DataBind();
                AllPageCode();
                Img_Emp_List_Select_All.Visible = true;
                Img_Emp_List_Delete_All.Visible = true;
                ratingControl.Visible = true;
                DDL_Status_Conversation.Visible = true;
                RequiredFieldValidator5.ValidationGroup = "Send";
            }
            else
            {
                Gv_Feedback_List.DataSource = Dt_Grid;
                Gv_Feedback_List.DataBind();
                Img_Emp_List_Select_All.Visible = false;
                Img_Emp_List_Delete_All.Visible = false;
                ratingControl.Visible = false;
                DDL_Status_Conversation.Visible = false;
                RequiredFieldValidator5.ValidationGroup = "False";
            }
        }
    }

    protected void Img_Search_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            Hdn_Select_Emp_ID.Value = Ddl_Employee_List.SelectedValue.ToString();
            Session["T_Dt_Filter_List"] = null;
            DataTable Dt_Task_List = Session["T_Dt_Task_List"] as DataTable;
            string Condition = string.Empty;
            if (Ddl_Employee_List.SelectedItem.ToString() != "--Select--" && Txt_Start_Date_List.Text != "" && Txt_Due_Date_List.Text != "")
            {
                Condition = "Is_Active='True' And Assign_To = '" + Ddl_Employee_List.SelectedValue.ToString() + "' and ((Start_Date >= '" + Convert.ToDateTime(Txt_Start_Date_List.Text).ToString("dd-MMM-yyyy") + "' and Start_Date <= '" + Convert.ToDateTime(Txt_Due_Date_List.Text).ToString("dd-MMM-yyyy") + "') or (Due_Date >= '" + Convert.ToDateTime(Txt_Start_Date_List.Text).ToString("dd-MMM-yyyy") + "' and Due_Date <= '" + Convert.ToDateTime(Txt_Due_Date_List.Text).ToString("dd-MMM-yyyy") + "'))";
            }
            else if (Ddl_Employee_List.SelectedItem.ToString() != "--Select--" && Txt_Start_Date_List.Text == "" && Txt_Due_Date_List.Text == "")
            {
                Condition = "Is_Active='True' And Assign_To = '" + Ddl_Employee_List.SelectedValue.ToString() + "'";
            }
            Dt_Task_List = new DataView(Dt_Task_List, Condition, "", DataViewRowState.CurrentRows).ToTable();
            Session["T_Dt_Task_List_Active"] = Dt_Task_List;
            Fill_Gv_Feedback(Dt_Task_List);
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
            Save_Checked_Task_Master();
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
                DataTable Dt_Task_List = (DataTable)Session["T_Dt_Task_List_Active"];
                DataView view = new DataView(Dt_Task_List, condition, "", DataViewRowState.CurrentRows);
                Session["T_Dt_Filter_Chart"] = view.ToTable();
                Fill_Gv_Feedback(view.ToTable());
                txtValue.Focus();
            }
            Populate_Checked_Task_Master();
        }
        catch
        {
        }
        txtValue.Focus();
    }

    protected void BtnBindDate_Click(object sender, EventArgs e)
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
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + Convert.ToDateTime(TxtValueDate.Text.Trim()).ToString() + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(TxtValueDate.Text.Trim()).ToString() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + Convert.ToDateTime(TxtValueDate.Text.Trim()).ToString() + "%'";
                }
                DataTable Dt_Task_List = (DataTable)Session["T_Dt_Task_List_Active"];
                DataView view = new DataView(Dt_Task_List, condition, "", DataViewRowState.CurrentRows);
                Session["T_Dt_Filter_Chart"] = view.ToTable();
                Fill_Gv_Feedback(view.ToTable());
                btnRefresh.Focus();
            }
        }
        catch
        {
        }
        TxtValueDate.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            txtValue.Text = "";
            TxtValueDate.Text = "";

            txtValue.Visible = true;
            btnbind.Visible = true;

            TxtValueDate.Visible = false;
            BtnBindDate.Visible = false;

            ddlFieldName.SelectedIndex = 0;
            ddlOption.SelectedIndex = 2;
            Fill_Gv_Feedback(Session["T_Dt_Task_List_Active"] as DataTable);
        }
        catch
        {
        }
    }

    protected void Img_Emp_List_Select_All_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        DataTable dtUnit = (DataTable)Session["T_Dt_Task_List_Active"];
        ArrayList userdetails = new ArrayList();
        Session["T_CHECKED_ITEMS_T"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["T_CHECKED_ITEMS_T"] != null)
                {
                    userdetails = (ArrayList)Session["T_CHECKED_ITEMS_T"];
                }
                if (!userdetails.Contains(dr["Trans_ID"]))
                {
                    userdetails.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Feedback_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["T_CHECKED_ITEMS_T"] = userdetails;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["T_Dt_Task_List_Active"];
            objPageCmn.FillData((object)Gv_Feedback_List, dt, "", "");
            AllPageCode();
            ViewState["Select"] = null;
            if (dt.Rows.Count > 0)
            {
                Img_Emp_List_Select_All.Visible = true;
                Img_Emp_List_Delete_All.Visible = true;
                ratingControl.Visible = true;
                DDL_Status_Conversation.Visible = true;
                RequiredFieldValidator5.ValidationGroup = "Send";
            }
            else
            {
                Img_Emp_List_Select_All.Visible = false;
                Img_Emp_List_Delete_All.Visible = false;
                ratingControl.Visible = false;
                DDL_Status_Conversation.Visible = false;
                RequiredFieldValidator5.ValidationGroup = "False";
            }
        }
    }

    protected void Img_Emp_List_Delete_All_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["T_CHECKED_ITEMS_T"] = null;
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (Gv_Feedback_List.Rows.Count > 0)
        {
            Save_Checked_Task_Master();
            if (Session["T_CHECKED_ITEMS_T"] != null)
            {
                userdetail = (ArrayList)Session["T_CHECKED_ITEMS_T"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = TaskMaster.Set_Hr_EmpTask(userdetail[j].ToString(), Session["CompId"].ToString(), "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "", "", "", Hdn_Contact_Id.Value, "0", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");
                        }
                    }
                }
                if (b != 0)
                {
                    if (Hdn_Select_Emp_ID.Value != "")
                    {
                        Fill_Grid_List();
                        Ddl_Employee_List.SelectedValue = Hdn_Select_Emp_ID.Value;
                        Img_Search_Click(null, null);
                    }
                    else
                    {
                        Fill_Grid_List();
                        Ddl_Employee_List.SelectedValue = Emp_List_ID.Value;
                        Img_Search_Click(null, null);
                    }
                    ViewState["Select"] = null;
                    DisplayMessage("Record Deleted");
                    Session["T_CHECKED_ITEMS_T"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Feedback_List.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("Chk_Gv_Select");
                        if (chk.Checked)
                        {
                            fleg = 1;
                        }
                        else
                        {
                            fleg = 0;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
                    }
                    else
                    {
                        DisplayMessage("Record Not Activated");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                Gv_Feedback_List.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    private void Save_Checked_Task_Master()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Feedback_List.Rows)
        {
            index = (int)Gv_Feedback_List.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select")).Checked;
            if (Session["T_CHECKED_ITEMS_T"] != null)
                userdetails = (ArrayList)Session["T_CHECKED_ITEMS_T"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["T_CHECKED_ITEMS_T"] = userdetails;
    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (Ddl_Employee_New.SelectedItem.ToString() == "--Select--")
            {
                DisplayMessage("Select Employee");
                return;
            }
            else if (Txt_Start_Date.Text == "")
            {
                DisplayMessage("Enter Start Date");
                return;
            }
            else if (Txt_Due_Date.Text == "")
            {
                DisplayMessage("Enter Due Date");
                return;
            }
            else if (Txt_Title.Text == "")
            {
                DisplayMessage("Enter Task Title");
                return;
            }
            else if (Editor_Description.Content.Trim() == "")
            {
                DisplayMessage("Enter Task Description");
                return;
            }
            else
            {
                if (Edit_ID.Value == "")
                {
                    int b = 0;
                    b = TaskMaster.Set_Hr_EmpTask("0", Session["CompId"].ToString(), Ddl_Employee_New.SelectedValue.ToString(), Txt_Due_Date.Text.Trim(), Txt_Start_Date.Text.Trim(), "", Txt_Title.Text.Trim(), Editor_Description.Content.Trim(), Hdn_Contact_Id.Value, "0", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
                    if (b != 0)
                    {
                        Send_Notification_Task(Txt_Title.Text, Txt_Start_Date.Text, Ddl_Employee_New.SelectedValue.ToString(), Txt_Due_Date.Text);
                        DisplayMessage("Record Saved", "green");
                        Reset();
                        Fill_Grid_List();
                        Ddl_Employee_List.SelectedValue = Hdn_Select_Emp_ID.Value;
                        Img_Search_Click(null, null);
                        Lbl_Tab_New.Text = Resources.Attendance.New;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    }
                    else
                    {
                        DisplayMessage("Record Not Saved");
                    }
                }
                else
                {
                    int b = 0;
                    b = TaskMaster.Set_Hr_EmpTask(Edit_ID.Value, Session["CompId"].ToString(), Ddl_Employee_New.SelectedValue.ToString(), Txt_Due_Date.Text.Trim(), Txt_Start_Date.Text.Trim(), "", Txt_Title.Text.Trim(), Editor_Description.Content.Trim(), Hdn_Contact_Id.Value, "0", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                    if (b != 0)
                    {
                        DisplayMessage("Record Updated", "green");
                        Reset();
                        Fill_Grid_List();
                        Ddl_Employee_List.SelectedValue = Hdn_Select_Emp_ID.Value;
                        Img_Search_Click(null, null);
                        Lbl_Tab_New.Text = Resources.Attendance.New;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    }
                    else
                    {
                        DisplayMessage("Record Not Updated");
                    }
                }
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

    protected void Send_Notification_Task(string Title, string Start_Date, string Assign_To, string Due_Date)
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        Dt_Request_Type = Obj_Notifiacation.Get_Notification_Type_Request("4");
        string Request_URL = "../Duty_Master/Task.aspx";
        string Message = string.Empty;
        Message = "Task " + Title + " assign for " + GetEmployeeName(Assign_To) + ". Task Start Date  " + Start_Date + " and Due Date is " + Due_Date;
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Assign_To, Assign_To, Message, Dt_Request_Type.Rows[0]["Trans_ID"].ToString(), Request_URL, "", "0", "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "17");
    }

    protected void Reset()
    {
        try
        {
            Txt_Contact.Text = "";
            Txt_Start_Date.Text = "";
            Txt_Due_Date.Text = "";
            Txt_Title.Text = "";
            Editor_Description.Content = "";
            Ddl_Employee_New.SelectedIndex = 0;
            Edit_ID.Value = "";
            Lbl_Tab_New.Text = Resources.Attendance.New;
        }
        catch
        {
        }
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            Reset();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
        catch
        {
        }
    }

    protected void Btn_Reset_Click(object sender, EventArgs e)
    {
        try
        {
            Reset();
        }
        catch
        {
        }
    }


    // Bin
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        try
        {
            Save_Checked_Task_Master_Bin();
            if (ddlOptionBin.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOptionBin.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
                }
                else if (ddlOptionBin.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text.Trim() + "%'";
                }
                DataTable Dt_Task_List = (DataTable)Session["T_Dt_Task_Bin_InActive"];
                DataView view = new DataView(Dt_Task_List, condition, "", DataViewRowState.CurrentRows);
                Session["T_Dt_Filter_Chart_Bin"] = view.ToTable();
                Fill_Gv_Bin(view.ToTable());

            }
            Populate_Checked_Task_Master_Bin();
        }
        catch (Exception ex)
        {
        }
        txtValueBin.Focus();
    }

    protected void Fill_Grid_Bin()
    {
        try
        {
            DataTable Dt_Task_Bin = Session["T_Dt_Task_List"] as DataTable;
            DataTable Dt_Tl_Team_Emp = Session["T_Tl_Team_Emp"] as DataTable;
            DataTable Dt_Bin = new DataTable();
            Dt_Bin.Columns.AddRange(new DataColumn[13] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Company_ID"), new DataColumn("Assign_To"), new DataColumn("Start_Date"), new DataColumn("Due_Date"), new DataColumn("Status"), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Contact_ID"), new DataColumn("Rating"), new DataColumn("Is_Active"), new DataColumn("Emp_Name"), new DataColumn("Contact_Name") });
            for (int i = 0; i < Dt_Tl_Team_Emp.Rows.Count; i++)
            {
                DataRow[] Dr_Bin = Dt_Task_Bin.Select("Assign_To = " + Dt_Tl_Team_Emp.Rows[i]["Emp_Id"].ToString() + " and Is_Active='False'");
                foreach (DataRow row in Dr_Bin)
                {
                    Dt_Bin.ImportRow(row);
                }
            }
            Session["T_Dt_Task_Bin_InActive"] = Dt_Bin;
            if (Dt_Task_Bin.Rows.Count > 0)
            {
                Fill_Gv_Bin(Dt_Bin);
            }
            else
            {
                Fill_Gv_Bin(null);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Fill_Gv_Bin(DataTable Dt_Grid)
    {
        Lbl_TotalRecords_Bin.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();
        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Task_Bin.DataSource = Dt_Grid;
            Gv_Task_Bin.DataBind();
            AllPageCode();
            Img_Emp_List_Active.Visible = true;
        }
        else
        {
            Gv_Task_Bin.DataSource = Dt_Grid;
            Gv_Task_Bin.DataBind();
            AllPageCode();
            Img_Emp_List_Active.Visible = false;
        }
    }

    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        try
        {
            txtValueBin.Text = "";
            TxtValueDateBin.Text = "";

            txtValueBin.Visible = true;
            btnbindBin.Visible = true;

            TxtValueDateBin.Visible = false;
            BtnBindDateBin.Visible = false;

            ddlFieldNameBin.SelectedIndex = 0;
            ddlOptionBin.SelectedIndex = 2;
            Fill_Gv_Bin(Session["T_Dt_Task_Bin_InActive"] as DataTable);
        }
        catch
        {
        }
    }

    protected void Img_Emp_Bin_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["T_Dt_Task_Bin_InActive"];
        ArrayList userdetails = new ArrayList();
        Session["T_CHECKED_ITEMS_T_BIN"] = null;
        if (ViewState["Select_Bin"] == null)
        {
            ViewState["Select_Bin"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["T_CHECKED_ITEMS_T_BIN"] != null)
                {
                    userdetails = (ArrayList)Session["T_CHECKED_ITEMS_T_BIN"];
                }
                if (!userdetails.Contains(dr["Trans_ID"]))
                {
                    userdetails.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Task_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["T_CHECKED_ITEMS_T_BIN"] = userdetails;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["T_Dt_Task_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Task_Bin, dt, "", "");
            AllPageCode();
            ViewState["Select_Bin"] = null;
            if (dt.Rows.Count > 0)
            {
                Img_Emp_List_Active.Visible = true;
            }
            else
            {
                Img_Emp_List_Active.Visible = false;
            }
        }
    }

    protected void Img_Emp_List_Active_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        Session["T_CHECKED_ITEMS_T_BIN"] = null;
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (Gv_Task_Bin.Rows.Count > 0)
        {
            Save_Checked_Task_Master_Bin();
            if (Session["T_CHECKED_ITEMS_T_BIN"] != null)
            {
                userdetail = (ArrayList)Session["T_CHECKED_ITEMS_T_BIN"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = TaskMaster.Set_Hr_EmpTask(userdetail[j].ToString(), Session["CompId"].ToString(), "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "", "", "", Hdn_Contact_Id.Value, "0", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");
                        }
                    }
                }
                if (b != 0)
                {
                    Fill_Grid_List();
                    Btn_Bin_Click(null, null);
                    ViewState["Select_Bin"] = null;
                    DisplayMessage("Record Activated");
                    Session["T_CHECKED_ITEMS_T_BIN"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Task_Bin.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("Chk_Gv_Select_Bin");
                        if (chk.Checked)
                        {
                            fleg = 1;
                        }
                        else
                        {
                            fleg = 0;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
                    }
                    else
                    {
                        DisplayMessage("Record Not Activated");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                Gv_Task_Bin.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    private void Save_Checked_Task_Master_Bin()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Task_Bin.Rows)
        {
            index = (int)Gv_Task_Bin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin")).Checked;
            if (Session["T_CHECKED_ITEMS_T_BIN"] != null)
                userdetails = (ArrayList)Session["T_CHECKED_ITEMS_T_BIN"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["T_CHECKED_ITEMS_T_BIN"] = userdetails;
    }
    protected void Populate_Checked_Task_Master_Bin()
    {
        ArrayList userdetails = (ArrayList)Session["T_CHECKED_ITEMS_T_BIN"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Task_Bin.Rows)
            {
                int index = (int)Gv_Task_Bin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            if (ddlFieldName.SelectedValue.ToString() == "Start_Date" || ddlFieldName.SelectedValue.ToString() == "Due_Date")
            {
                txtValue.Text = "";
                TxtValueDate.Text = "";

                txtValue.Visible = false;
                btnbind.Visible = false;

                TxtValueDate.Visible = true;
                BtnBindDate.Visible = true;
            }
            else
            {
                txtValue.Text = "";
                TxtValueDate.Text = "";

                txtValue.Visible = true;
                btnbind.Visible = true;

                TxtValueDate.Visible = false;
                BtnBindDate.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void Btn_Edit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        string Trans_ID = e.CommandArgument.ToString();
        DataTable dt = TaskMaster.Get_Hr_EmpTask(Trans_ID, Session["CompId"].ToString(), Session["EmpID"].ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), "", "", "", "0", "0", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
        if (dt.Rows.Count > 0)
        {
            Edit_ID.Value = Trans_ID;
            try
            {
                Ddl_Employee_New.SelectedValue = dt.Rows[0]["Assign_To"].ToString();
            }
            catch { }
            Txt_Start_Date.Text = Convert.ToDateTime(dt.Rows[0]["Start_Date"]).ToString("dd-MMM-yyyy");
            Txt_Due_Date.Text = Convert.ToDateTime(dt.Rows[0]["Due_Date"]).ToString("dd-MMM-yyyy");
            Txt_Title.Text = dt.Rows[0]["Title"].ToString();
            Editor_Description.Content = dt.Rows[0]["Description"].ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            Lbl_Tab_New.CssClass = "active";
            Txt_Start_Date.Focus();
        }
    }

    protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    {
        string Trans_ID = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = TaskMaster.Set_Hr_EmpTask(Trans_ID, Session["CompId"].ToString(), "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "", "", "", Hdn_Contact_Id.Value, "0", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");
        if (b != 0)
        {
            if (Hdn_Select_Emp_ID.Value != "")
            {
                Fill_Grid_List();
                Ddl_Employee_List.SelectedValue = Hdn_Select_Emp_ID.Value;
                Img_Search_Click(null, null);
            }
            else
            {
                Fill_Grid_List();
                Ddl_Employee_List.SelectedValue = Emp_List_ID.Value;
                Img_Search_Click(null, null);
            }
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }

    protected void IBtn_Active_Command(object sender, CommandEventArgs e)
    {
        string Trans_ID = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = TaskMaster.Set_Hr_EmpTask(Trans_ID, Session["CompId"].ToString(), "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "", "", "", Hdn_Contact_Id.Value, "0", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");
        if (b != 0)
        {
            Fill_Grid_List();
            Btn_Bin_Click(null, null);
            DisplayMessage("Record Active");
        }
        else
        {
            DisplayMessage("Record Not Active");
        }
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Feedback_List.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow gr in Gv_Feedback_List.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select")).Checked = false;
            }
        }
    }

    protected void Chk_Gv_Select_All_Bin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Task_Bin.HeaderRow.FindControl("Chk_Gv_Select_All_Bin"));
        foreach (GridViewRow gr in Gv_Task_Bin.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
        }
    }

    protected void Gv_Feedback_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Task_Master();
            Gv_Feedback_List.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["T_Dt_Filter_Chart"] != null)
                dt = (DataTable)Session["T_Dt_Filter_Chart"];
            else if (Session["T_Dt_Task_List_Active"] != null)
                dt = (DataTable)Session["T_Dt_Task_List_Active"];
            objPageCmn.FillData((object)Gv_Feedback_List, dt, "", "");
            AllPageCode();
            Gv_Feedback_List.HeaderRow.Focus();
            Populate_Checked_Task_Master();
        }
        catch
        {
        }
    }

    protected void Populate_Checked_Task_Master()
    {
        ArrayList userdetails = (ArrayList)Session["T_CHECKED_ITEMS_T"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Feedback_List.Rows)
            {
                int index = (int)Gv_Feedback_List.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("Chk_Gv_Select");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void Gv_Feedback_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            if (Session["T_Dt_Filter_Chart"] != null)
                dt = (DataTable)Session["T_Dt_Filter_Chart"];
            else if (Session["T_Dt_Task_List_Active"] != null)
                dt = (DataTable)Session["T_Dt_Task_List_Active"];

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
            Session["T_Dt_Filter_Chart"] = dt;
            objPageCmn.FillData((object)Gv_Feedback_List, dt, "", "");
            AllPageCode();
            Gv_Feedback_List.HeaderRow.Focus();
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
                int c = 0;
                int b = 0;
                b = TaskMaster.Set_Hr_EmpTask(Hdn_Conversation_ID.Value, Session["CompId"].ToString(), Ddl_Employee_List.SelectedValue.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), DDL_Status_Conversation.SelectedItem.ToString(), Lbl_C_Task.Text, "", "0", ratingControl.CurrentRating.ToString(), "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "4");
                if (b != 0)
                {
                    if (Hdn_Feedback_Trans.Value == "")
                    {
                        c = TaskMaster.Set_Sys_Feedback("0", Session["UserID"].ToString(), Session["EmpID"].ToString(), "Hr_EmpTask", Hdn_Conversation_ID.Value, Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
                    }
                    else if (Hdn_Feedback_Trans.Value != "" && Hdn_Status.Value == "Edit")
                    {
                        c = TaskMaster.Set_Sys_Feedback(Hdn_Feedback_Trans.Value, Session["UserID"].ToString(), Session["EmpID"].ToString(), "Hr_EmpTask", "0", Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                    }
                    if (c != 0)
                    {
                        Hdn_Feedback_Trans.Value = "";
                        Hdn_Status.Value = "";
                        DDL_Status_Conversation.SelectedIndex = 0;
                        Txt_FeedBack.Text = "";
                        ratingControl.CurrentRating = 0;
                        //DisplayMessage("Feedback Saved");
                        Get_Conversation(Lbl_C_Task.Text, Lbl_C_Date.Text, Ltr_C_Description.Text);
                        Txt_FeedBack.Focus();
                        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Conversation()", true);
                    }
                    else
                    {
                        DisplayMessage("Feedback Not Saved");
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Btn_Delete_Feedback_Click(object sender, EventArgs e)
    {
        try
        {
            int c = 0;
            if (Hdn_Feedback_Trans.Value != "" && Hdn_Status.Value == "Delete")
            {
                c = TaskMaster.Set_Sys_Feedback(Hdn_Feedback_Trans.Value, Session["UserID"].ToString(), Session["EmpID"].ToString(), "Hr_EmpTask", "0", Txt_FeedBack.Text, DDL_Status_Conversation.SelectedItem.ToString(), ratingControl.CurrentRating.ToString(), "", "", "", "", "", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");
            }
            if (c != 0)
            {
                Hdn_Feedback_Trans.Value = "";
                Hdn_Status.Value = "";
                DDL_Status_Conversation.SelectedIndex = 0;
                Txt_FeedBack.Text = "";
                ratingControl.CurrentRating = 0;
                DisplayMessage("Feedback Deleted");
                Get_Conversation(Lbl_C_Task.Text, Lbl_C_Date.Text, Ltr_C_Description.Text);
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Conversation()", true);
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

    protected void Btn_Bin_Click(object sender, EventArgs e)
    {
        Fill_Grid_Bin();
    }

    protected void Gv_Task_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Task_Master_Bin();
            Gv_Task_Bin.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["T_Dt_Filter_Chart_Bin"] != null)
                dt = (DataTable)Session["T_Dt_Filter_Chart_Bin"];
            else if (Session["T_Dt_Task_Bin_InActive"] != null)
                dt = (DataTable)Session["T_Dt_Task_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Task_Bin, dt, "", "");
            AllPageCode();
            Gv_Task_Bin.HeaderRow.Focus();
            Populate_Checked_Task_Master_Bin();
        }
        catch
        {
        }
    }

    protected void Gv_Task_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            if (Session["T_Dt_Filter_Chart_Bin"] != null)
                dt = (DataTable)Session["T_Dt_Filter_Chart_Bin"];
            else if (Session["T_Dt_Task_Bin_InActive"] != null)
                dt = (DataTable)Session["T_Dt_Task_Bin_InActive"];

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
            Session["T_Dt_Filter_Chart_Bin"] = dt;
            objPageCmn.FillData((object)Gv_Task_Bin, dt, "", "");
            AllPageCode();
            Gv_Task_Bin.HeaderRow.Focus();
        }
        catch
        {
        }
    }

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        try
        {
            if (ddlFieldNameBin.SelectedValue.ToString() == "Start_Date" || ddlFieldNameBin.SelectedValue.ToString() == "Due_Date")
            {
                txtValueBin.Text = "";
                TxtValueDateBin.Text = "";

                txtValueBin.Visible = false;
                btnbindBin.Visible = false;

                TxtValueDateBin.Visible = true;
                BtnBindDateBin.Visible = true;
            }
            else
            {
                txtValueBin.Text = "";
                TxtValueDateBin.Text = "";

                txtValueBin.Visible = true;
                btnbindBin.Visible = true;

                TxtValueDateBin.Visible = false;
                BtnBindDateBin.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void BtnBindDateBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        try
        {
            if (ddlOptionBin.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (TxtValueDateBin.Text != "")
                {
                    if (ddlOptionBin.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + Convert.ToDateTime(TxtValueDateBin.Text.Trim()).ToString() + "'";
                    }
                    else if (ddlOptionBin.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(TxtValueDateBin.Text.Trim()).ToString() + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + Convert.ToDateTime(TxtValueDateBin.Text.Trim()).ToString() + "%'";
                    }
                }
                DataTable Dt_Task_List = (DataTable)Session["T_Dt_Task_Bin_InActive"];
                DataView view = new DataView(Dt_Task_List, condition, "", DataViewRowState.CurrentRows);
                Session["T_Dt_Filter_Chart_Bin"] = view.ToTable();
                Fill_Gv_Bin(view.ToTable());
                btnRefresh.Focus();
            }
        }
        catch
        {
        }
        TxtValueDateBin.Focus();
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
            Hdn_Status.Value = "";
            //Txt_FeedBack.Text = "1st round- Telephonic \n2nd round- face to face interviewing, (if shortlisted in 1st round) \n3rd round- HR and Operations testing then selecting the best available candidate.";
            GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
            Label Lbl_ID_List = gvRow.FindControl("Lbl_ID_List") as Label;
            Label Lbl_Task_List = gvRow.FindControl("Lbl_Task_List") as Label;
            Label Lbl_Task_Description_List = gvRow.FindControl("Lbl_Task_Description_List") as Label;
            Label Lbl_Start_Date_List = gvRow.FindControl("Lbl_Start_Date_List") as Label;
            Label Lbl_Due_Date_List = gvRow.FindControl("Lbl_Due_Date_List") as Label;
            Label Lbl_Assign_To_ID_List = gvRow.FindControl("Lbl_Assign_To_ID_List") as Label;
            Hdn_Conversation_ID.Value = e.CommandArgument.ToString();

            //Hdn_Emp_ID.Value = Lbl_Assign_To_ID_List.Text;
            //Hdn_Trans_ID.Value = Lbl_ID_List.Text;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Conversation()", true);
            Lbl_C_Task.Text = Lbl_Task_List.Text;
            Lbl_C_Date.Text = Lbl_Start_Date_List.Text;
            Ltr_C_Description.Text = Lbl_Task_Description_List.Text;
            string Conversation = string.Empty;
            DataTable Dt_Conversion = TaskMaster.Get_Sys_Feedback("0", "0", "0", "0", Hdn_Conversation_ID.Value, "0", "", "0", "", "", "", "", "", "True", "0", "0", "2");
            if (Dt_Conversion.Rows.Count > 0)
            {
                for (int i = 0; i < Dt_Conversion.Rows.Count; i++)
                {
                    //LinkButton lbOneMore = new LinkButton();
                    //lbOneMore.Text = "One more click";
                    //lbOneMore.CommandArgument = "cArg";
                    //lbOneMore.CommandName = "CName";
                    //lbOneMore.Click += new EventHandler(Edit_Feedback);
                    //DivControl.Controls.Add(lbOneMore);

                    ratingControl.CurrentRating = Convert.ToInt32(Dt_Conversion.Rows[i]["Rating"]);
                    if (Dt_Conversion.Rows[i]["Status"].ToString() == "")
                        DDL_Status_Conversation.SelectedIndex = 0;
                    else
                    {
                        if (Dt_Conversion.Rows[i]["Status"].ToString() == "Select")
                            DDL_Status_Conversation.SelectedValue = "0";
                        else if (Dt_Conversion.Rows[i]["Status"].ToString() == "Not Started")
                            DDL_Status_Conversation.SelectedValue = "1";
                        else if (Dt_Conversion.Rows[i]["Status"].ToString() == "Deferred")
                            DDL_Status_Conversation.SelectedValue = "2";
                        else if (Dt_Conversion.Rows[i]["Status"].ToString() == "In-Progress")
                            DDL_Status_Conversation.SelectedValue = "3";
                        else if (Dt_Conversion.Rows[i]["Status"].ToString() == "Completed")
                            DDL_Status_Conversation.SelectedValue = "4";
                        else if (Dt_Conversion.Rows[i]["Status"].ToString() == "Waiting For Input")
                            DDL_Status_Conversation.SelectedValue = "5";
                        //DDL_Status_Conversation.Items.FindByText(Dt_Conversion.Rows[i]["Status"].ToString()).Selected = true;
                    }
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
            Ltr_Conversion.Text = Conversation;
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

    protected void Get_Conversation(string C_Task, string C_Date, string C_Description)
    {
        try
        {
            //GridViewRow gvRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
            //Label Lbl_ID_List = gvRow.FindControl("Lbl_ID_List") as Label;
            //Label Lbl_Task_List = gvRow.FindControl("Lbl_Task_List") as Label;
            //Label Lbl_Task_Description_List = gvRow.FindControl("Lbl_Task_Description_List") as Label;
            //Label Lbl_Start_Date_List = gvRow.FindControl("Lbl_Start_Date_List") as Label;
            //Label Lbl_Due_Date_List = gvRow.FindControl("Lbl_Due_Date_List") as Label;
            //Label Lbl_Assign_To_ID_List = gvRow.FindControl("Lbl_Assign_To_ID_List") as Label;
            //Hdn_Conversation_ID.Value = e.CommandArgument.ToString();
            ////Hdn_Emp_ID.Value = Lbl_Assign_To_ID_List.Text;
            ////Hdn_Trans_ID.Value = Lbl_ID_List.Text;
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Conversation()", true);
            Lbl_C_Task.Text = C_Task;
            Lbl_C_Date.Text = C_Date;
            Ltr_C_Description.Text = C_Description;
            string Conversation = string.Empty;
            DataTable Dt_Conversion = TaskMaster.Get_Sys_Feedback("0", "0", "0", "0", Hdn_Conversation_ID.Value, "0", "", "0", "", "", "", "", "", "True", "0", "0", "2");
            if (Dt_Conversion.Rows.Count > 0)
            {
                for (int i = 0; i < Dt_Conversion.Rows.Count; i++)
                {
                    //LinkButton lbOneMore = new LinkButton();
                    //lbOneMore.Text = "One more click";
                    //lbOneMore.CommandArgument = "cArg";
                    //lbOneMore.CommandName = "CName";
                    //lbOneMore.Click += new EventHandler(Edit_Feedback);
                    //DivControl.Controls.Add(lbOneMore);

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

    protected void ratingControl_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "On_Load_Modal()", true);
    }
}