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

public partial class MasterSetUp_ForgetPassword : System.Web.UI.Page
{
    #region defind Class Objectpa

    UserMaster objUser = null;
    SystemParameter objSys = null;
    Set_UserReminder ObjUserReminder = null;
    Set_ApplicationParameter objAppParam = null;
    DataAccessClass objDa = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.QueryString["User_Id"] == null)
        {

            Response.Redirect("~/ERPLogin.aspx");

        }
        Session["AccordianId"] = "8";
        //Page.Title = objSys.GetSysTitle();

        //objUser = new UserMaster(Session["DBConnection"].ToString());
        //objSys = new SystemParameter(Session["DBConnection"].ToString());
        //ObjUserReminder = new Set_UserReminder(Session["DBConnection"].ToString());
        //objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        //objDa = new DataAccessClass(Session["DBConnection"].ToString());

        objUser = new UserMaster(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
        objSys = new SystemParameter(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
        ObjUserReminder = new Set_UserReminder(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
        objAppParam = new Set_ApplicationParameter(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
        objDa = new DataAccessClass(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);

    }

    #region System defind Function
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtReEnterPass.Text = "";
        txtOldPassword.Text = "";
        txtNewPassword.Text = "";
        txtOldPassword.Attributes.Add("Value", "");
        txtNewPassword.Attributes.Add("Value", "");
        txtReEnterPass.Attributes.Add("Value", "");
        //lblLoginSec.Visible = true;
    }
    //protected void rbtnLoginOnCheckedChanged(object sender, EventArgs e)
    //{
    //    txtReEnterPass.Text = "";
    //    txtOldPassword.Text = "";
    //    txtNewPassword.Text = "";

    //    txtOldPassword.Attributes.Add("Value", "");
    //    txtNewPassword.Attributes.Add("Value", "");
    //    txtReEnterPass.Attributes.Add("Value", "");
    //    if (rbtnLogin.Checked)
    //    {
    //        lblLoginSec.Visible = true;
    //        lblEmailSec.Visible = false;

    //        lblemailsignature.Visible = false;
    //        lblcolon.Visible = false;
    //        txtEmailSignature.Visible = false;

    //    }
    //    else
    //    {
    //        DataTable dt = objUser.GetUserMasterByUserId(Session["UserId"].ToString().Trim(), Session["LoginCompany"].ToString());

    //        if (dt.Rows.Count > 0)
    //        {
    //            try
    //            {
    //                txtEmailSignature.Content = dt.Rows[0]["Field5"].ToString();
    //            }
    //            catch
    //            {

    //            }
    //        }

    //        lblemailsignature.Visible = true;
    //        lblcolon.Visible = true;
    //        txtEmailSignature.Visible = true;


    //        lblLoginSec.Visible = false;
    //        lblEmailSec.Visible = true;
    //    }
    //}
    protected void btnSave_Click(object sender, EventArgs e)
    {
        MasterDataAccess objMDa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
        if (txtOldPassword.Text.Trim() == "")
        {
            DisplayMessage("Enter Verification Code");
            txtOldPassword.Focus();
            return;
        }
        else
        {
            txtOldPassword.Attributes.Add("Value", txtOldPassword.Text);

        }

        if (txtNewPassword.Text.Trim() == "")
        {
            DisplayMessage("Enter new password");
            txtNewPassword.Focus();
            return;
        }
        else
        {
            txtNewPassword.Attributes.Add("Value", txtNewPassword.Text);
        }

        if (txtReEnterPass.Text.Trim() == "")
        {
            DisplayMessage("Re-Enter new password");
            txtReEnterPass.Focus();
            return;
        }
        else
        {
            txtReEnterPass.Attributes.Add("Value", txtReEnterPass.Text);
        }

        if (txtNewPassword.Text != txtReEnterPass.Text)
        {
            DisplayMessage("New password and Re-Enter password does not match");
            return;
        }


        if(Request.QueryString["RegCode"]==null)
        {
            Session["DBConnection"] = ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString;
        }
        else
        {
            //string strserverName = objMDa.get_SingleValue("select @@servername");
            string strserverName = "74.208.235.72";
            MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(Common.Decrypt(Request.QueryString["RegCode"].ToString().Trim()), ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());
            Session["DBConnection"] = "Data Source=" + strserverName + ";Initial Catalog=" + clsMasterCmp.database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
        }



        string strIsModified = string.Empty;


        DataTable dt = new DataTable();
        dt = objDa.return_DataTable("SELECT dbo.Set_UserMaster.*, Set_EmployeeMaster.Email_Id, set_employeemaster.Emp_Name FROM dbo.Set_UserMaster  INNER JOIN dbo.Set_EmployeeMaster ON dbo.Set_UserMaster.Emp_Id = dbo.Set_EmployeeMaster.Emp_Id WHERE Set_UserMaster.Emp_Id = '" + Common.Decrypt(Request.QueryString["Employee_Id"].ToString()) + "' AND Set_UserMaster.IsActive = 'True' AND Set_EmployeeMaster.IsActive = 'True' AND Set_EmployeeMaster.Field2 = 'False'");


        if (dt.Rows.Count > 0)
        {


            if (txtOldPassword.Text.Trim() != dt.Rows[0]["Field5"].ToString().Trim())
            {
                DisplayMessage("Enter Correct Verfication Code");

                return;
            }
            else
            {
                objUser.UpdateUserMaster(dt.Rows[0]["User_Id"].ToString(), dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["User_Id"].ToString(), Common.Encrypt(txtNewPassword.Text), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Role_Id"].ToString(), dt.Rows[0]["Is_Modified"].ToString(), dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), dt.Rows[0]["User_Id"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["IsGlobalAccess"].ToString());

                int ReminderDays = 0;

                try
                {
                    ReminderDays = Convert.ToInt32(objDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Password Reminder(In Days)' and Company_Id='" + dt.Rows[0]["Company_Id"].ToString() + "'").Rows[0][0].ToString());
                }
                catch
                {
                    ReminderDays = 0;
                }


                if (ReminderDays > 0)
                {

                    ObjUserReminder.updateRecord(dt.Rows[0]["User_Id"].ToString(), DateTime.Now.AddDays(ReminderDays).ToString(), "1", dt.Rows[0]["User_Id"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["User_Id"].ToString(), DateTime.Now.ToString());
                    ObjUserReminder.InsertRecord(dt.Rows[0]["User_Id"].ToString(), DateTime.Now.AddDays(ReminderDays).ToString(), "0", dt.Rows[0]["User_Id"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["User_Id"].ToString(), DateTime.Now.ToString());
                }

                DisplayMessage("Login Password has been reset successfully");
                btnReset_Click(null, null);
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }


    }
    #endregion

    #region User defind Function

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    #endregion
}