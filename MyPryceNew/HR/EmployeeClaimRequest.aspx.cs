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
using PegasusDataAccess;

public partial class HR_EmployeeClaimRequest : BasePage
{
    Set_Approval_Employee objEmpApproval = null;
    Pay_Employee_claim ObjClaim = null;
    EmployeeMaster ObjEmp = null;
    Common ObjComman = null;
    UserMaster objUser = null;
    SystemParameter objSys = null;
    Pay_Employee_Month objPayEmpMonth = null;
    DataAccessClass DataAccessClass = null;
    NotificationMaster Obj_Notifiacation = null;
    PageControlCommon objPageCmn = null;

    string strCompId = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        strCompId = Session["CompId"].ToString();
        btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        ObjClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        ObjEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        DataAccessClass = new DataAccessClass(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "84", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
            TxtYear.Text = DateTime.Now.Year.ToString();
        }
        TxtEmployeeId.Text = GetEmpName();
        TxtClaimName.Focus();
        AllPageCode();
        Page.Title = objSys.GetSysTitle();

    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("84", (DataTable)Session["ModuleName"]);
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

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {
            btnSave.Visible = true;

        }
        else
        {
            DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "84",Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {


                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            btnSave.Visible = true;


                        }


                    }
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
    }
    public string GetEmpName()
    {
        string empname = string.Empty;
        DataTable dtEmp = new DataTable();

        DataTable dt = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        if (dt.Rows.Count > 0)
        {
            dtEmp = ObjEmp.GetEmployee_InPayroll(Session["CompId"].ToString());

            try
            {
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "' and emp_id=" + dt.Rows[0]["Emp_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            //try
            //{
            //    if (HttpContext.Current.Session["SessionDepId"] != null)
            //    {
            //        dtEmp = new DataView(dtEmp, "Department_Id in(" + HttpContext.Current.Session["SessionDepId"].ToString().Substring(0, HttpContext.Current.Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            //    }
            //}
            //catch
            //{
            //}
            if (dtEmp.Rows.Count > 0)
            {

                empname = dt.Rows[0]["EmpName"].ToString();
                HidEmpId.Value = dt.Rows[0]["Emp_Id"].ToString();
            }


        }

        return empname;


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
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    void Reset()
    {
        TxtClaimName.Text = "";
        TxtClaimDiscription.Text = "";
        ddlMonth.SelectedIndex = 0;
        txtCalValue.Text = "";
        DdlValueType.SelectedIndex = 0;
        TxtYear.Text = "";
        TxtClaimName.Focus();
        TxtYear.Text = DateTime.Now.Year.ToString();
        int CurrentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());


        ddlMonth.SelectedValue = (CurrentMonth).ToString();
        HidEmpId.Value = "";
    }
    protected void btnSaveClaim_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (TxtEmployeeId.Text == "")
        {
            DisplayMessage("Super admin Cannot Request For Claim Or Login User not in Payroll");
            TxtEmployeeId.Focus();
            btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            return;

        }
        if (TxtClaimName.Text.Trim() == "")
        {
            DisplayMessage("Enter Claim Name");
            TxtClaimName.Focus();
            btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            return;
        }
        if (TxtClaimName.Text.Trim() == ".")
        {
            DisplayMessage("Claim Name not accept only decimal point");
            TxtClaimName.Focus();
            btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            return;
        }
        if (DdlValueType.SelectedIndex == 0)
        {
            DisplayMessage("Select Value Type");
            DdlValueType.Focus();
            btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            return;

        }
        if (txtCalValue.Text == "")
        {
            DisplayMessage("Enter Claim Value");
            txtCalValue.Focus();
            btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            return;
        }
        if (ddlMonth.SelectedIndex == 0)
        {
            DisplayMessage("Select Month");
            ddlMonth.Focus();
            btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            return;
        }
        if (TxtYear.Text == "")
        {
            DisplayMessage("Enter Year");
            TxtYear.Focus();
            btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            return;
        }
        else
        {


        }

        HidEmpId.Value = ValidPayRoll(HidEmpId.Value);
        if (HidEmpId.Value != "")
        {
            DataTable dtempmonth = new DataTable();
            dtempmonth = objPayEmpMonth.GetAllRecordPostedEmpMonth(HidEmpId.Value, ddlMonth.SelectedIndex.ToString(), TxtYear.Text.ToString(),Session["CompId"].ToString());
            if (dtempmonth.Rows.Count > 0)
            {
                DisplayMessage("Payroll Posted For This Month and Year");
                return;
            }

            DataTable dt = new DataTable();
            string EmpPermission = string.Empty;
            EmpPermission = objSys.Get_Approval_Parameter_By_Name("Claim").Rows[0]["Approval_Level"].ToString();


            dt = objEmpApproval.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(),"84", HidEmpId.Value);

            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Approval setup issue , please contact to your admin");
                return;
            }




            b = ObjClaim.Insert_In_Pay_Employee_ClaimRequest(Session["CompId"].ToString(), HidEmpId.Value, TxtClaimName.Text.Trim(), TxtClaimDiscription.Text, DdlValueType.SelectedValue, txtCalValue.Text, DateTime.Now.ToString(), "Pending", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "84", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {

                DateTime Date = new DateTime(int.Parse(TxtYear.Text), int.Parse(ddlMonth.SelectedValue), 1, 0, 0, 0);

                if (dt.Rows.Count > 0)
                {

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        int cur_trans_id = 0;
                        string PriorityEmpId = dt.Rows[j]["Emp_Id"].ToString();
                        string IsPriority = dt.Rows[j]["Priority"].ToString();
                        if (EmpPermission == "1")
                        {
                            cur_trans_id=objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                        }
                        else if (EmpPermission == "2")
                        {
                            cur_trans_id=objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                        }
                        else if (EmpPermission == "3")
                        {
                            cur_trans_id=objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                        }
                        else
                        {
                            cur_trans_id=objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                        }

                        // Insert Notification For Leave by  ghanshyam suthar
                        Session["PriorityEmpId"] = PriorityEmpId;
                        Session["cur_trans_id"] = cur_trans_id;
                        Set_Notification();

                    }

                }

                //
                DisplayMessage("Record Saved", "green");
                Reset();
            }
        }
        else
        {
            ddlMonth.Focus();
        }

        btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
    }

    public string GetEmployeeName(string EmployeeId)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = ObjEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
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

    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/hr"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, HidEmpId.Value, Session["PriorityEmpId"].ToString());
        GetEmployeeName(HidEmpId.Value);
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = TxtEmployeeId.Text.Trim() + " applied Claim Request for " + TxtClaimName.Text + " for " + ddlMonth.SelectedItem.ToString() + " " + TxtYear.Text + "";
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), HidEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "0");
    }

    private string ValidPayRoll(string StrAllEmpId)
    {
        DataAccessClass objDA = new DataAccessClass(Session["DBConnection"].ToString());
        string strMessage = string.Empty;
        string strEmpId = string.Empty;


        foreach (string str in StrAllEmpId.Split(','))
        {
            if ((str != ""))
            {


                DataTable dtPayPostedInfo = objDA.return_DataTable("select * from Pay_Employe_Month  where Emp_Id = '" + str + "' and Year = (Select MAX(Year) From Pay_Employe_Month  where Emp_Id = '" + str + "') order by MONTH desc");

                if (dtPayPostedInfo.Rows.Count > 0)
                {
                    DateTime tTemp = new DateTime(Convert.ToInt16(dtPayPostedInfo.Rows[0]["Year"].ToString()), Convert.ToInt16(dtPayPostedInfo.Rows[0]["Month"].ToString()), 1);
                    DateTime tCurrent = new DateTime(Convert.ToInt16(TxtYear.Text), Convert.ToInt16(ddlMonth.SelectedValue), 1);



                    if (tTemp < tCurrent)
                    {
                        strEmpId += str;
                    }
                    else
                    {

                        DisplayMessage("Claim Month and Year Should be Greater than Last Posted Month And Year");


                    }


                }
                else
                {



                    DateTime tEDOJ = Convert.ToDateTime(ObjEmp.GetEmployeeMasterById(Session["CompId"].ToString(), str).Rows[0]["DOJ"].ToString());



                    if (tEDOJ.Year <= Convert.ToInt16(TxtYear.Text))
                    {
                        if (tEDOJ.Year < Convert.ToInt16(TxtYear.Text))
                        {
                            strEmpId += str;
                        }
                        else
                        {
                            if (tEDOJ.Month <= Convert.ToInt16(ddlMonth.SelectedValue.ToString()))
                            {
                                strEmpId += str;
                            }
                            else
                            {

                                DisplayMessage("Claim Month and year should be greater than or equal to Date Of Joining");


                            }
                        }

                    }
                    else
                    {
                        DisplayMessage("Claim Month and year should be greater than or equal to Date Of Joining");

                    }


                }




            }
        }



        return strEmpId;


    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetClaimName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Pay_Employee_claim.GetClaimName("", HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["DBConnection"].ToString());

        dt = new DataView(dt, "Claim_Name lIKE '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();


        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i][0].ToString();
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }
    protected void TxtClaimName_textChanged(object sender, EventArgs e)
    {
        if (TxtClaimName.Text != "")
        {
            if (TxtClaimName.Text.Trim() == "Past Leave Settlement" || TxtClaimName.Text.Trim() == "Leave Settlement" || TxtClaimName.Text.Trim() == "Indemnity Claim" || TxtClaimName.Text.Trim() == "Leave Salary")
            {
                DisplayMessage("You cannot Insert this Claim Name");
                TxtClaimName.Text = "";
                TxtClaimName.Focus();
            }
        }

    }

}
