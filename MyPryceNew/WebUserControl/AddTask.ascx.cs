using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using PegasusDataAccess;

//created by divya parakh 
public partial class WebUserControl_AddTask : System.Web.UI.UserControl
{
    NotificationMaster Obj_Notifiacation = null;
    EmployeeMaster objEmp = null;
    Common cmn = null;
    TaskMaster TaskMaster = null;
    IT_ObjectEntry objObjectEntry =null;
    SystemParameter objSys = null;
    DesignationMaster objDesg = null;
    Common ObjComman = null;
    DataAccessClass objDa = null;
    SystemParameter ObjSysParam = null;
    SalesLeadClass SLCalss = null;
    PageControlCommon objPageCmn = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        tblName = (HiddenField)Parent.FindControl("TableName");

        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        TaskMaster = new TaskMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objDesg = new DesignationMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        SLCalss = new SalesLeadClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            fillDate();

            //string where = "Company_Id=" + Session["CompID"].ToString() + " and Field2='False' and IsActive='true' order by Emp_Name asc";
            //DataTable EmpName_Id = objDa.return_DataTable("select Emp_Name, cast(Emp_Code as int) as Emp_Code,Emp_Id from Set_EmployeeMaster where " + where + "");

            //Ddl_Employee_New.DataSource = EmpName_Id;

            //try
            //{
            //    for (int i = 0; i < EmpName_Id.Rows.Count; i++)
            //    {
            //        Ddl_Employee_New.Items.Add(EmpName_Id.Rows[i][0].ToString() + "/" + EmpName_Id.Rows[i][1].ToString());
            //    }
            //}
            //catch (Exception ee)
            //{

            //}
        }

    }

    public void fillDate()
    {
        Txt_Start_Date.Text = GetDate(System.DateTime.Now.ToString());
        Txt_Due_Date.Text = GetDate(System.DateTime.Now.ToString());
        CalendarExtender4.Format = objSys.SetDateFormat();
        CalendarExtender5.Format = objSys.SetDateFormat();
    }

    private void fillTaskSession()
    {
        DataTable dt1 = new DataTable();
        if (tblName.Value == "Inv_SalesLead")
        {
            dt1 = SLCalss.TaskGridData(tblName.Value, HttpContext.Current.Session["SalesLeadData"].ToString());
            Session["SalesLeadGrid"] = dt1;
        }
        else
        {
            if (tblName.Value == "crm_campaign")
            {
                dt1 = SLCalss.TaskGridData(tblName.Value, HttpContext.Current.Session["CampaignData"].ToString());
                Session["CRM_CampaignGrid"] = dt1;
            }
        }
    }

    private void fillTaskGrid()
    {
        DataTable dt1 = new DataTable();

        if (tblName.Value == "Inv_SalesLead")
        {
            dt1 = Session["SalesLeadGrid"] as DataTable;
            GvExistingData.DataSource = dt1;
            GvExistingData.DataBind();
            AllPageCodeSalesLead();
        }
        else
        {
            if (tblName.Value == "crm_campaign")
            {
                dt1 = Session["CRM_CampaignGrid"] as DataTable;
                GvExistingData.DataSource = dt1;
                GvExistingData.DataBind();
                AllPageCodeCampaign();
            }
        }
    }

    public void AllPageCodeCampaign()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("35", (DataTable)Session["ModuleName"]);
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
        //End Code

        Page.Title = objSys.GetSysTitle();
        if (Session["EmpId"].ToString() == "0")
        {
            //Btn_SaveT.Visible = true;
            foreach (GridViewRow Row in GvExistingData.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
            }
            //imgBtnRestore.Visible = true;
            //ImgbtnSelectAll.Visible = false;
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "35",Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
            {

            }
            else
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {

                    foreach (GridViewRow Row in GvExistingData.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "2")
                        {
                            ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                        }
                        if (DtRow["Op_Id"].ToString() == "3")
                        {
                            ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                        }
                    }
                }
            }

        }


    }

    public void AllPageCodeSalesLead()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());


        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("393", (DataTable)Session["ModuleName"]);
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
        //End Code



        Page.Title = ObjSysParam.GetSysTitle();

        if (Session["EmpId"].ToString() == "0")
        {

            foreach (GridViewRow Row in GvExistingData.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                //((ImageButton)Row.FindControl("lnkViewDetail")).Visible = true;
            }



            //imgBtnRestore.Visible = true;
            //ImgbtnSelectAll.Visible = false;
        }
        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "393",Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {

            foreach (GridViewRow Row in GvExistingData.Rows)
            {
                if (DtRow["Op_Id"].ToString() == "2")
                {
                    ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                }
                if (DtRow["Op_Id"].ToString() == "3")
                {
                    ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                }
            }
        }
    }

    public void DisplayMessage(string str,string color="orange")
    {
        try
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
            Txt_Start_Date.Text = "";
            Txt_Due_Date.Text = "";
            Txt_Title.Text = "";
            Editor_Description.Content = "";
            //Ddl_Employee_New.SelectedIndex = 0;
            Edit_ID.Value = "";
            hdnEmpId.Value = "";
            fillDate();
        }
        catch
        {
        }
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

    protected void Btn_SaveT_Click(object sender, EventArgs e)
    {

        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myTimer()", true);

        try
        {
            if (hdnEmpId.Value== "")
            {
                DisplayMessage("Select Employee");
                txtEmployeeList.Text = "";
                txtEmployeeList.Focus();
                Btn_SaveT.Enabled = true;
                return;
            }



            if (Txt_Start_Date.Text == "")
            {
                DisplayMessage("Enter Start date");
                Txt_Start_Date.Focus();
                Btn_SaveT.Enabled = true;
                return;
            }

            if (Txt_Due_Date.Text == "")
            {
                DisplayMessage("Enter End date");
                Txt_Due_Date.Focus();
                Btn_SaveT.Enabled = true;
                return;
            }
            if (ObjSysParam.getDateForInput(Txt_Start_Date.Text) > ObjSysParam.getDateForInput(Txt_Due_Date.Text))
            {
                Txt_Due_Date.Text = "";
                Txt_Due_Date.Focus();
                Btn_SaveT.Enabled = true;
                return;
            }
            if (Txt_Title.Text == "")
            {
                DisplayMessage("Enter Task Title");
                Txt_Title.Focus();
                Btn_SaveT.Enabled = true;
                return;
            }
            else if (Editor_Description.Content.Trim() == "")
            {
                DisplayMessage("Enter Task Description");
                Editor_Description.Focus();
                Btn_SaveT.Enabled = true;
                return;
            }
            else
            {
                int b = 0;
                string EmpId = hdnEmpId.Value;//Ddl_Employee_New.SelectedValue.ToString().Split('/')[1].ToString();

                if (tblName.Value == "Inv_SalesLead")
                {
                    PrimaryId.Value = (Parent.FindControl("lblId") as Label).Text;
                }
                else
                {
                    if (tblName.Value == "crm_campaign")
                    {
                        PrimaryId.Value = (Parent.FindControl("lblCampId") as Label).Text;
                    }
                }

                if (EditCheck.Text == "0")
                {
                    b = TaskMaster.Set_Hr_EmpTask("0", Session["CompId"].ToString(), EmpId, Txt_Due_Date.Text.Trim(), Txt_Start_Date.Text.Trim(), "", Txt_Title.Text.Trim(), Editor_Description.Content.Trim(), "0", "0", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1", tblName.Value, PrimaryId.Value);
                    if (b != 0)
                    {
                        Send_Notification_Task(Txt_Title.Text, Txt_Start_Date.Text, EmpId, Txt_Due_Date.Text);

                        DisplayMessage("Task has been Assigned");
                        Reset();

                        if (tblName.Value == "Inv_SalesLead")
                        {
                            DataTable dtAllData = SLCalss.getActiveLeadDataById(PrimaryId.Value);
                            string date1 = GetDate(dtAllData.Rows[0]["Lead_date"].ToString());
                            string leadStatus = "";

                            if (dtAllData.Rows[0]["Lead_status"].ToString() == "")
                            {
                                leadStatus = "In Process";
                                SLCalss.UpdateLeadData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtAllData.Rows[0]["Lead_no"].ToString(), ObjSysParam.getDateForInput(date1).ToString(), dtAllData.Rows[0]["Customer_Id"].ToString(), dtAllData.Rows[0]["Contact_Id"].ToString(), dtAllData.Rows[0]["Title"].ToString(), dtAllData.Rows[0]["Lead_source"].ToString(), leadStatus, dtAllData.Rows[0]["Currency_ID"].ToString(), dtAllData.Rows[0]["Opportunity_amount"].ToString(), dtAllData.Rows[0]["Source_description"].ToString(), dtAllData.Rows[0]["Status_description"].ToString(), dtAllData.Rows[0]["Remark"].ToString(), dtAllData.Rows[0]["Generated_by"].ToString(), dtAllData.Rows[0]["Assign_to"].ToString(), dtAllData.Rows[0]["Campaign_Id"].ToString(), dtAllData.Rows[0]["Refered_by"].ToString(), Session["EmpID"].ToString(), Session["EmpID"].ToString());
                                //DisplayMessage("Record Updated Successfully", "green");

                            }
                        }


                        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Close()", true);
                        requestData();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);
                    }
                    else
                    {
                        DisplayMessage("Record Not Saved");
                    }
                }
                else
                {
                    //Edit button case
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);
                    b = TaskMaster.Set_Hr_EmpTask(Trans_ID.Text, Session["CompId"].ToString(), EmpId, Txt_Due_Date.Text.Trim(), Txt_Start_Date.Text.Trim(), "", Txt_Title.Text.Trim(), Editor_Description.Content.Trim(), "0", "0", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), tblName.Value, PrimaryId.Value);
                    if (b != 0)
                    {
                        Send_Notification_Task(Txt_Title.Text, Txt_Start_Date.Text, EmpId, Txt_Due_Date.Text);

                        DisplayMessage("Task has been Updated");
                        Reset();

                        requestData();


                    }
                    else
                    {
                        DisplayMessage("Record Not Saved");
                    }
                    EditCheck.Text = "0";
                }

            }
        }
        catch (Exception error)
        {
        }


    }

    protected void Btn_CancelT_Click(object sender, EventArgs e)
    {
        Reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Close()", true);
        try
        {
            Response.Redirect(Request.RawUrl);
        }
        catch (Exception ee)
        {

        }

    }

    protected void GvExistingData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add.Attributes.Add("Class", "box box-primary");
        GvExistingData.PageIndex = e.NewPageIndex;
        DataTable dt = new DataTable();

        if (tblName.Value == "Inv_SalesLead")
        {
            dt = Session["SalesLeadGrid"] as DataTable; ;
        }
        else
        {
            if (tblName.Value == "crm_campaign")
            {
                dt = Session["CRM_CampaignGrid"] as DataTable;
            }
        }

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvExistingData, dt, "", "");
        PermissionFunction();


    }

    protected void GvExistingData_Sorting(object sender, GridViewSortEventArgs e)
    {
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add.Attributes.Add("Class", "box box-primary");

        DataTable dt = new DataTable();
        if (tblName.Value == "Inv_SalesLead")
        {
            dt = Session["SalesLeadGrid"] as DataTable;
        }
        else
        {
            if (tblName.Value == "crm_campaign")
            {
                dt = Session["CRM_CampaignGrid"] as DataTable;
            }
        }

        if (dt != null)
        {

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

            if (tblName.Value == "Inv_SalesLead")
            {
                Session["SalesLeadGrid"] = dt;
            }
            else
            {
                if (tblName.Value == "crm_campaign")
                {
                    Session["CRM_CampaignGrid"] = dt;
                }
            }

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvExistingData, dt, "", "");

        }

        PermissionFunction();
    }

    public void PermissionFunction()
    {
        if (tblName.Value == "Inv_SalesLead")
        {
            AllPageCodeSalesLead();
        }
        else
        {
            if (tblName.Value == "crm_campaign")
            {
                AllPageCodeCampaign();
            }
        }
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);
        string Trans_ID = e.CommandArgument.ToString();
        string PKID = e.CommandName.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = TaskMaster.Set_HR_EmpTask_UpdateTask("false", Session["UserID"].ToString(), Trans_ID, tblName.Value, PKID);
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            requestData();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }


    }

    public void requestData()
    {

        DataTable dt1 = new DataTable();
        try
        {
            fillTaskSession();
            fillTaskGrid();
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnEdit_Command1(object sender, CommandEventArgs e)
    {

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);
        DataTable dt = new DataTable();
        if (tblName.Value == "Inv_SalesLead")
        {
            dt = Session["SalesLeadGrid"] as DataTable;
        }
        else
        {
            if (tblName.Value == "crm_campaign")
            {
                dt = Session["CRM_CampaignGrid"] as DataTable;
            }
        }

        string PKid = e.CommandArgument.ToString();

        string condition = "Trans_Id=" + PKid + "";
        Trans_ID.Text = PKid;
        dt = new DataView(dt, condition, "", DataViewRowState.CurrentRows).ToTable();

        //Ddl_Employee_New.SelectedValue = dt.Rows[0]["AssignedToName"].ToString() + "/" + dt.Rows[0]["Assign_To"].ToString();
        Txt_Start_Date.Text = GetDate(dt.Rows[0]["Start_Date"].ToString());
        Txt_Due_Date.Text = GetDate(dt.Rows[0]["Due_Date"].ToString());
        Txt_Title.Text = dt.Rows[0]["Title"].ToString();
        Editor_Description.Content = dt.Rows[0]["Description"].ToString();
        EditCheck.Text = "1";
    }

    protected void txtEmployeeList_TextChanged(object sender, EventArgs e)
    {
        int start_pos = txtEmployeeList.Text.LastIndexOf("/") + 1;
        int last_pos = txtEmployeeList.Text.Length;
        string id = txtEmployeeList.Text.Substring(start_pos, last_pos - start_pos);
        if (txtEmployeeList.Text != "")
        {
            hdnEmpId.Value = GetContactId();
            if (hdnEmpId.Value != "" && hdnEmpId.Value != "0")
            {
                txtEmployeeList.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtEmployeeList.Text = "";
                txtEmployeeList.Focus();
            }
        }
        else
        {
            txtEmployeeList.Text = "";
            txtEmployeeList.Focus();
        }
    }

    

    private string GetContactId()
    {
        string retval = "";
        try
        {
            if (txtEmployeeList.Text != "")
            {
                int start_pos = txtEmployeeList.Text.LastIndexOf("/") + 1;
                int last_pos = txtEmployeeList.Text.Length;
                string id = txtEmployeeList.Text.Substring(start_pos, last_pos - start_pos);
                if (start_pos != 0)
                {
                    //DataTable dtSupp = ObjContactMaster.GetContactTrueAllData();
                    DataTable Dt_EmpInfo = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(),id);

                    int Last_pos_name = txtEmployeeList.Text.LastIndexOf("/");
                    string nameNcode = txtEmployeeList.Text.Substring(0, Last_pos_name - 0);



                    int start_pos_code = nameNcode.LastIndexOf("/") + 1;
                    int last_pos_code = nameNcode.Length;
                    string code = nameNcode.Substring(start_pos_code, last_pos_code - start_pos_code);

                    txtEmployeeList.Text = nameNcode;

                    int Last_pos_code1 = nameNcode.LastIndexOf("/");
                    string name = txtEmployeeList.Text.Substring(0, Last_pos_code1 - 0);


                    //string name = txt_contactName.Text.Trim().Split('/')[0].ToString().Trim();
                    Dt_EmpInfo = new DataView(Dt_EmpInfo, "Emp_Name='" + name + "' ", "", DataViewRowState.CurrentRows).ToTable();

                    if (Dt_EmpInfo.Rows.Count > 0)
                    {
                        retval = id;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        return retval;
    }
}