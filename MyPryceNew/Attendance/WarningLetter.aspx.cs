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

public partial class Attendance_WarningLetter : System.Web.UI.Page
{
    EmployeeMaster objEmp = null;

    protected void Page_Load(object sender, EventArgs e)
    {
       
            objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
       
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
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
                //hdnEmpId.Value = empid;

                //FillLeaveSummary(empid);
                //FillApproveLeave(empid);
                //FillPendingLeave(empid);
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtEmpName.Text = "";
                txtEmpName.Focus();
                //hdnEmpId.Value = "";
                return;
            }
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {

    }
    protected void btnApply_Click(object sender, EventArgs e)
    {

    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
}