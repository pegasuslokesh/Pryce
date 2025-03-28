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

public partial class HR_EmployeeLoanRequest : BasePage
{
    Pay_Employee_Loan ObjLoan = null;
    EmployeeMaster ObjEmp = null;
    SystemParameter objSys = null;
    UserMaster objUser = null;
    Common ObjComman = null;
    PageControlCommon objPageCmn = null;

    Set_Approval_Employee objApproalEmp = null;
    string strCompId = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        strCompId = Session["CompId"].ToString();

        ObjLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        ObjEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "86", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            txtEmpName.Text = GetEmpName();
            txtLoanName.Focus();
            Calender.Format = objSys.SetDateFormat();
        }
        //Page.Title = objSys.GetSysTitle();
        AllPageCode();
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
            try
            {
                if (HttpContext.Current.Session["SessionDepId"] != null)
                {
                    dtEmp = new DataView(dtEmp, "Department_Id in(" + HttpContext.Current.Session["SessionDepId"].ToString().Substring(0, HttpContext.Current.Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            catch
            {
            }
            if (dtEmp.Rows.Count > 0)
            {
                empname = dt.Rows[0]["EmpName"].ToString();
                HidEmpId.Value = dt.Rows[0]["Emp_Id"].ToString();
            }
        }
        return empname;
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("86", (DataTable)Session["ModuleName"]);
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
            BtnSave.Visible = true;
        }
        else
        {
            DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "86", Session["CompId"].ToString());
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
                            BtnSave.Visible = true;
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
    public void DisplayMessage(string str, string color = "orange")
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        if (txtEmpName.Text == "")
        {
            DisplayMessage("Super admin Cannot Request For Loan Or Login User not in Payroll");
            txtEmpName.Focus();
            return;
        }



        try
        {
            Convert.ToDateTime(txtRequestDate.Text);
        }
        catch
        {
            DisplayMessage("Enter valid date");
            txtRequestDate.Focus();
            return;
        }




        if (txtLoanName.Text == "")
        {
            DisplayMessage("Enter Loan Name");
            txtLoanName.Focus();
            return;
        }
        if (txtLoanAmount.Text == "")
        {
            DisplayMessage("Enter Loan Amount");
            txtLoanAmount.Focus();
            return;
        }
        DataTable dt = new DataTable();
        string EmpPermission = string.Empty;
        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Loan").Rows[0]["Approval_Level"].ToString();
        dt = objApproalEmp.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "86", HidEmpId.Value);
        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Approval setup issue , please contact to your admin");
            return;
        }
        int CheckInsertion = 0;
        CheckInsertion = ObjLoan.Insert_In_Pay_Employee_Loan_Request(strCompId, HidEmpId.Value, txtLoanName.Text, objSys.getDateForInput(txtRequestDate.Text).ToString(), txtLoanAmount.Text, "Pending", "86", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (CheckInsertion != 0)
        {
            DisplayMessage("Record Saved", "green");
            txtEmpName.Focus();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Saved");
        }
        txtLoanName.Focus();
    }
    void Reset()
    {
        txtLoanName.Text = "";
        txtLoanAmount.Text = "";
        txtEmpName.Focus();
        HidEmpId.Value = "";
        txtRequestDate.Text = "";
    }
    protected void TxtEmpName_TextChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;
        if (txtEmpName.Text != "")
        {
            empid = txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1];
            DataTable dtEmp = ObjEmp.GetEmployeeMasterOnRole(strCompId);
            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                HidEmpId.Value = empid;
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtEmpName.Text = "";
                txtEmpName.Focus();
                HidEmpId.Value = "";
                return;
            }
        }
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetLoanName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Pay_Employee_Loan.GetLoanName("", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        dt = new DataView(dt, "Loan_Name lIKE '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i][0].ToString();
        }
        return str;
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        txtLoanName.Focus();
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        txtLoanName.Focus();
    }
}