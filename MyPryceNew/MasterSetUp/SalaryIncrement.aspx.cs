using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetUp_SalaryIncrement : BasePage
{
    Set_Emp_SalaryIncrement objEmpSalInc = null;
    NotificationMaster Obj_Notifiacation = null;
    EmployeeMaster objEmployee = null;
    Set_ApplicationParameter objAppParam = null;
    EmployeeParameter objEmpParam = null;
    EmployeeMaster objEmp = null;
    SystemParameter objSys = null;
    Set_Approval_Employee objApproalEmp = null;
    PageControlCommon objPageCmn = null;
    int Fr_IncrementDuration = 0;
    int Exp_IncrementDuration = 0;

    Common cmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmpSalInc = new Set_Emp_SalaryIncrement(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "172", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            // BindddlIncrementPercent();
            FillGrid();
        }
        Page.Title = objSys.GetSysTitle();
        AllPageCode();
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("172", (DataTable)Session["ModuleName"]);
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

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {
            gvEmp.Columns[0].Visible = true;
            foreach (GridViewRow Row in gvEmp.Rows)
            {
                ((ImageButton)Row.FindControl("imgBtnApprove")).Visible = true;
                ((TextBox)Row.FindControl("txtIncrementValue")).Visible = true;
            }
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "172", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
            {
            }
            else
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (DtRow["Op_Id"].ToString() == "6")
                    {
                        gvEmp.Columns[0].Visible = true;
                    }
                    foreach (GridViewRow Row in gvEmp.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "2")
                        {
                            ((ImageButton)Row.FindControl("imgBtnApprove")).Visible = true;
                        }
                        
                        if (DtRow["Op_Id"].ToString() == "9")
                        {
                            ((TextBox)Row.FindControl("txtIncrementValue")).Visible = true;
                        }
                    }
                }
            }
        }
    }
    protected void chkEmpInc_CheckedChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        AllPageCode();
    }

    private DataTable BindddlIncrementPercent(string ExpLevel)
    {
        DataTable dtIncrement = new DataTable();


        int IncrementFrom = 0;
        int IncrementTo = 0;
        if (ExpLevel != "Experience")
        {
            IncrementFrom = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            IncrementTo = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        else
        {
            IncrementFrom = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_From", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            IncrementTo = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        dtIncrement.Columns.Add("IncrementPer", typeof(string));
        DataRow dtrow = dtIncrement.NewRow();
        for (int i = IncrementFrom; i <= IncrementTo; i++)
        {
            dtrow["IncrementPer"] = i;
            dtIncrement.Rows.Add(dtrow);
            dtrow = dtIncrement.NewRow();
        }
        return dtIncrement;

    }
   
    protected void gvEmp_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmp.PageIndex = e.NewPageIndex;
        FillGrid();
        AllPageCode();
    }
    public void FillGrid()
    {
        string ExpLevel = string.Empty;
        // Modified By Nitin jain On 20/11/2014 Get Salary Increment Parameter For Fresher 
        Fr_IncrementDuration = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        // Experienced 
        Exp_IncrementDuration = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration For Experience", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        //.................................................................................

        DataTable dt = objEmp.GetEmployeeIncrement(Fr_IncrementDuration.ToString(), System.DateTime.Now.ToString(), "", "0");

        if (chkEmpInc.Checked)
        {
            dt = objEmpSalInc.GetAllEmpSalaryIncrement(Session["CompId"].ToString());
        }
        else
        {
            dt = objEmpSalInc.GetEmpSalaryIncrementByMonthYear(Session["CompId"].ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString());
        }

        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        try
        {
            if (Session["SessionDepId"] != null)
            {
                dt = new DataView(dt, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        catch (Exception Ex)
        {

        }

        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmp, dt, "", "");
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["dtSalaryIncr"] = dt;
       
        DataTable dtFresher = BindddlIncrementPercent("Fresher");
        DataTable dtExp = BindddlIncrementPercent("Experience");
        

        foreach (GridViewRow gvr in gvEmp.Rows)
        {
            DropDownList ddlIncrPer = (DropDownList)gvr.FindControl("ddlIncrPer");


            if (((HiddenField)gvr.FindControl("hdncategory")).Value.Trim()== "Fresher")
            {
                ddlIncrPer.DataSource = dtFresher;
            }
            else
            {
                ddlIncrPer.DataSource = dtExp;
            }
          
            ddlIncrPer.DataTextField = "IncrementPer";
            ddlIncrPer.DataValueField = "IncrementPer";
            ddlIncrPer.DataBind();
            ((TextBox)gvr.FindControl("txtIncrementValue")).Text = ddlIncrPer.SelectedValue;

        }
        
    }
    public string GetDuration(object EmpId)
    {
        string Duration = "0";
        DataTable dt = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmpId.ToString());

        if (dt.Rows.Count > 0)
        {
            DateTime JoininingDate = new DateTime(1900, 1, 1);
            try
            {
                JoininingDate = Convert.ToDateTime(dt.Rows[0]["DOJ"].ToString());
            }
            catch
            {
                Duration = "0";
            }
            if (JoininingDate.ToString("dd/mm/yyyy") != "1/1/1900")
            {
                TimeSpan ts = DateTime.Now - JoininingDate;
                double month = ts.TotalDays / 30;
                Duration = System.Math.Round(month, 0).ToString();
            }
        }
        return Duration;
    }
    
    public string GetMonth(object Month)
    {
        string[] month = new string[13];
        month[0] = "--Select--";
        month[1] = "JAN";
        month[2] = "FEB";
        month[3] = "MAR";
        month[4] = "APR";
        month[5] = "MAY";
        month[6] = "JUN";
        month[7] = "JUL";
        month[8] = "AUG";
        month[9] = "SEP";
        month[10] = "OCT";
        month[11] = "NOV";
        month[12] = "DEC";

        return month[int.Parse(Month.ToString())].ToString();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex =2;
        txtValue.Text = "";
        AllPageCode();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOption.SelectedIndex == 1)
        {
            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
        }
        else if (ddlOption.SelectedIndex == 2)
        {
            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
        }
        else
        {
            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '" + txtValue.Text.Trim() + "%'";
        }

        DataTable dtEmp = (DataTable)Session["dtSalaryIncr"];
        DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);

        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";

        Session["dtSalaryIncr"] = view.ToTable();

        dtEmp = view.ToTable();
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmp, dtEmp, "", "");

        DataTable dtFresher = BindddlIncrementPercent("Fresher");
        DataTable dtExp = BindddlIncrementPercent("Experience");


        foreach (GridViewRow gvr in gvEmp.Rows)
        {
            DropDownList ddlIncrPer = (DropDownList)gvr.FindControl("ddlIncrPer");


            if (((HiddenField)gvr.FindControl("hdncategory")).Value.Trim() == "Fresher")
            {
                ddlIncrPer.DataSource = dtFresher;
            }
            else
            {
                ddlIncrPer.DataSource = dtExp;
            }

            ddlIncrPer.DataTextField = "IncrementPer";
            ddlIncrPer.DataValueField = "IncrementPer";
            ddlIncrPer.DataBind();
            ((TextBox)gvr.FindControl("txtIncrementValue")).Text = ddlIncrPer.SelectedValue;

        }
        AllPageCode();
        //ddlIncrPer_OnSelectedIndexChanged(sender, e);
        txtValue.Focus();
    }

    protected string GetEmployeeCode(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
                ViewState["Emp_Img"] = "../CompanyResource/2/" + dtEName.Rows[0]["Emp_Image"].ToString();
            }
            else
            {
                ViewState["Emp_Img"] = "";
            }
        }
        else
        {
            strEmployeeName = "";
        }
        return strEmployeeName;
    }

    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();

        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/mastersetup"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, Session["Req_Emp_ID"].ToString(), Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = GetEmployeeCode(Session["Req_Emp_ID"].ToString()) + " request for Salary Increment. on " + System.DateTime.Now.ToString();
        if (Hdn_Edit_ID.Value == "")
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["Req_Emp_ID"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["Ref_ID"].ToString(), "1");
        else
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["Req_Emp_ID"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Hdn_Edit_ID.Value, "15");
    }

    protected void Approve_Command(object sender, CommandEventArgs e)
    {
        PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        

        string sql = "select * from dbo.HR_Salary_Increment where Employee_Id=" + e.CommandName.ToString() + " and Field1='Pending'";
        DataTable dtSalary = objDA.return_DataTable(sql);

        if (dtSalary.Rows.Count > 0)
        {
            DisplayMessage("You Cannot Request,Your Previous Request is Under Processing...");
            return;
        }

        DataTable dt1 = new DataTable();
        string EmpPermission = string.Empty;
        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Salary Increment").Rows[0]["Approval_Level"].ToString();

        dt1 = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(),"172", e.CommandName.ToString());


        if (dt1.Rows.Count == 0)
        {
            DisplayMessage("Approval setup issue , please contact to your admin");

            return;
        }



        GridViewRow row = (GridViewRow)((ImageButton)sender).Parent.Parent;
        string Id = e.CommandName.ToString();
        decimal IncPer = 0;
        decimal IncrementValue;
        decimal IncrementSalary;
        DataTable dtEmpList = (DataTable)Session["dtSalaryIncr"];
        dtEmpList = new DataView(dtEmpList, "Emp_Id='" + e.CommandName.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        HiddenField HdnEmpId = (HiddenField)row.FindControl("HdnEmpId");
        DropDownList ddlIncrPer = (DropDownList)row.FindControl("ddlIncrPer");
        TextBox txtIncrPer = (TextBox)row.FindControl("txtIncrementValue");
        TextBox txtIncAmount = (TextBox)row.FindControl("Txt_Invrement_Amount");
        

        if (txtIncrPer.Visible == true)
        {
            if (txtIncrPer.Text != "")
            {
                IncPer = Convert.ToDecimal(txtIncrPer.Text);
            }
            else
            {
                DisplayMessage("Fill Increment Value in TextBox");
                return;
            }
        }
        else if (txtIncrPer.Visible == false)
        {
            if (ddlIncrPer.SelectedValue == "--Select--")
            {
                DisplayMessage("Select Increment Value in DropDownList");
                return;
            }
            else
            {
                IncPer = Convert.ToDecimal(ddlIncrPer.SelectedValue);
            }
        }


        Label lblBasicSal = (Label)row.FindControl("lblBasicSal");
        if (!string.IsNullOrEmpty(txtIncAmount.Text))
        {
            IncrementValue = decimal.Parse(txtIncAmount.Text);
        }
        else
        {
            IncrementValue = (Convert.ToDecimal(lblBasicSal.Text) * IncPer) / 100;
        }
        IncrementSalary = Convert.ToDecimal(lblBasicSal.Text) + IncrementValue;

        // string NextIncrementMonth = 

        //IncrementPer = double.Parse(ddlIncrPer.SelectedValue);
        string ExpLevel = string.Empty;
        DateTime FinalDate = new DateTime();
        if (dtEmpList.Rows.Count > 0)
        {
            if (dtEmpList.Rows[0]["Category"].ToString() == "Fresher")
            {
                Fr_IncrementDuration = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                FinalDate = Convert.ToDateTime(DateTime.Today.AddMonths(Fr_IncrementDuration).ToString());
            }
            else
            {
                Exp_IncrementDuration = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration For Experience", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                FinalDate = Convert.ToDateTime(DateTime.Today.AddMonths(Exp_IncrementDuration).ToString());
            }
        }
        int b = 0;

        // Experienced 


        //PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass();
        //string strQuery = "Update Set_Emp_SalaryIncrement SET Month='"+FinalDate.Month.ToString()+"',Year ='"+FinalDate.Year.ToString()+"',Basic_Salary='"+IncrementSalary.ToString()+"' where Emp_Id='"+e.CommandName.ToString()+"'";
        //int a = objDA.execute_Command(strQuery);
        b = objEmpSalInc.Insert_SalaryIncrement(Session["CompId"].ToString(), e.CommandName.ToString(), lblBasicSal.Text.ToString(), IncPer.ToString(), IncrementValue.ToString(), IncrementSalary.ToString(), "Pending", FinalDate.ToString(), "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        //this code is created by jitendra upadhyay on 03-03-2015
        //this code for insert salary increment request in approval employee table 

        //code start



        for (int j = 0; j < dt1.Rows.Count; j++)
        {
            int cur_trans_id = 0;
            string PriorityEmpId = dt1.Rows[j]["Emp_Id"].ToString();
            string IsPriority = dt1.Rows[j]["Priority"].ToString();
            if (EmpPermission == "1")
            {
                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("12", Session["CompId"].ToString(), "0", "0", "0", e.CommandName.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
            }
            else if (EmpPermission == "2")
            {
                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("12", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", e.CommandName.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
            }
            else if (EmpPermission == "3")
            {
                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("12", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", e.CommandName.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
            }
            else
            {
                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("12", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", e.CommandName.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
            }
            // Insert Notification For Leave by  ghanshyam suthar
            Session["PriorityEmpId"] = PriorityEmpId;
            Session["cur_trans_id"] = cur_trans_id;
            Session["Req_Emp_ID"] = e.CommandName.ToString();
            Session["Ref_ID"] = b.ToString();
            Set_Notification();
        }

        //code end

        DisplayMessage("Request Applied");
        FillGrid();
        AllPageCode();
    }


    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    public DataTable GetSalaryPercentage(object Category, object TransId)
    {
        int FromPer = 0;
        int ToPer = 0;

        DataTable dt1 = objEmpSalInc.GetEmpSalaryIncrement(Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt1.Rows.Count > 0)
        {
            FromPer = int.Parse(dt1.Rows[0]["Field1"].ToString());
            ToPer = int.Parse(dt1.Rows[0]["Field2"].ToString());

        }
        else
        {
            if (Category.ToString() == "Fresher")
            {
                FromPer = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                ToPer = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            }
            else
            {
                FromPer = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_From", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                ToPer = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

            }
        }


        DataTable dt = new DataTable();
        dt.Columns.Add("IncrementPer");

        for (int i = FromPer; i <= ToPer; i++)
        {
            DataRow dr = dt.NewRow();
            dr["IncrementPer"] = i.ToString();

            dt.Rows.Add(dr);

        }
        return dt;

    }

    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/SalaryIncrementReport.aspx?Emp_Id=" + e.CommandArgument.ToString() + "','window','width=1024');", true);
    }



    protected void txtIncrementValue_TextChanged(object sender, EventArgs e)
    {
        GridViewRow GV_Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label Basic_Salary = (Label)GV_Row.FindControl("lblBasicSal");
        TextBox Increment_Percentage = (TextBox)GV_Row.FindControl("txtIncrementValue");
        TextBox Increment_Amount = (TextBox)GV_Row.FindControl("Txt_Invrement_Amount");

        int parsedValue;
        float parseddecimal;
        if (!int.TryParse(Increment_Percentage.Text, out parsedValue))
        {
            if (!float.TryParse(Increment_Percentage.Text, out parseddecimal))
            {
                DisplayMessage("Amount entered was not in correct format");
                ((TextBox)GV_Row.FindControl("txtIncrementValue")).Text = "";
                ((TextBox)GV_Row.FindControl("Txt_Invrement_Amount")).Text = "";
                return;
            }
        }

        if (Increment_Percentage.Text == "")
            Increment_Percentage.Text = "0";

        if (Increment_Amount.Text == "")
            Increment_Amount.Text = "0";


        decimal D_Basic_Salary = Convert.ToDecimal(Basic_Salary.Text);
        decimal D_Increment_Percentage = Convert.ToDecimal(Increment_Percentage.Text);

        Increment_Amount.Text = ((D_Basic_Salary * D_Increment_Percentage) / 100).ToString("0.00");
    }

    protected void Txt_Invrement_Amount_TextChanged(object sender, EventArgs e)
    {
        GridViewRow GV_Row = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label Basic_Salary = (Label)GV_Row.FindControl("lblBasicSal");
        TextBox Increment_Percentage = (TextBox)GV_Row.FindControl("txtIncrementValue");
        TextBox Increment_Amount = (TextBox)GV_Row.FindControl("Txt_Invrement_Amount");

        int parsedValue;
        float parseddecimal;

        if (!int.TryParse(Increment_Amount.Text, out parsedValue))
        {
            if (!float.TryParse(Increment_Amount.Text, out parseddecimal))
            {
                DisplayMessage("Amount entered was not in correct format");
                ((TextBox)GV_Row.FindControl("Txt_Invrement_Amount")).Text = "";
                return ;
            }
        }

        if (Increment_Percentage.Text == "")
            Increment_Percentage.Text = "0";

        if (Increment_Amount.Text == "")
            Increment_Amount.Text = "0";

        decimal D_Basic_Salary = Convert.ToDecimal(Basic_Salary.Text);
        decimal D_Increment_Amount = Convert.ToDecimal(Increment_Amount.Text);

        Increment_Percentage.Text = ((D_Increment_Amount * 100) / D_Basic_Salary).ToString("0.00");
    }
    protected void ddlIncrPer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in gvEmp.Rows)
        {
            DropDownList ddlValue = (DropDownList)gvr.FindControl("ddlIncrPer");
            TextBox txtIncrementValue = (TextBox)gvr.FindControl("txtIncrementValue");

            if (txtIncrementValue.Visible == true)
            {
                txtIncrementValue.Text = ddlValue.SelectedValue;                
            }
            else
            {
                txtIncrementValue.Text = "";
            }
        }

        //Old Code
        double IncrementPer = int.Parse(((DropDownList)(sender)).SelectedValue);
        int index = ((GridViewRow)((DropDownList)sender).Parent.Parent).RowIndex;
        HiddenField hdnTransId = (HiddenField)gvEmp.Rows[index].FindControl("HdnEmpId");
      
    }

}
