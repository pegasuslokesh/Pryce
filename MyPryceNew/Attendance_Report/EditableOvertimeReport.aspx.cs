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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;

public partial class Attendance_Report_EditableOvertimeReport : BasePage
{
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    IT_ObjectEntry objObjectEntry = null;
    Att_AttendanceRegister objAttRegister = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("113", (DataTable)Session["ModuleName"]);
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

        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objAttRegister = new Att_AttendanceRegister(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();
       
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;

        if (txtEmpName.Text != "")
        {
            empid = txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hdnEmpId.Value = empid;
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtEmpName.Text = "";
                hdnEmpId.Value = "";
                return;
            }
        }
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
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
    protected void btnReport_Click(object sender, EventArgs e)
    {
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        if (txtEmpName.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtEmpName.Focus();
            return;
        }
        if (txtFromDate.Text == "")
        {
            DisplayMessage("From Date Required");
            txtFromDate.Focus();
            return;
        }
        if (txtToDate.Text == "")
        {
            DisplayMessage("To Date Required");
            txtToDate.Focus();
            return;
        }


        FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
        ToDate = objSys.getDateForInput(Session["ToDate"].ToString());

        DataTable dtdata = objAttRegister.GetAttendanceReport(hdnEmpId.Value + ",", FromDate.ToString(), ToDate.ToString(), "5");
        if (dtdata.Rows.Count > 0)
        {
            gvrecord.DataSource = dtdata;
            gvrecord.DataBind();
        }
    }
}