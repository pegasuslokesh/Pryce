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

public partial class MasterSetUp_ChangePassword : System.Web.UI.Page
{
    #region defind Class Objectpa

    UserMaster objUser = null;
    SystemParameter objSys = null;
    Set_UserReminder ObjUserReminder = null;
    Set_ApplicationParameter objAppParam = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["User_Id"] != null && Request.QueryString["Comp_Id"] != null)
        {
            if (IsValiduser(Common.Decrypt(Request.QueryString["Comp_Id"].ToString()), Common.Decrypt(Request.QueryString["User_Id"].ToString())))
            {
                Session["UserId"] = Common.Decrypt(Request.QueryString["User_Id"].ToString());
                Session["LoginCompany"] = Common.Decrypt(Request.QueryString["Comp_Id"].ToString());
                Session["CompId"] = Common.Decrypt(Request.QueryString["Comp_Id"].ToString());
            }
        }

        if (Request.QueryString["User_Id"] == null)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
        Session["AccordianId"] = "8";
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjUserReminder = new Set_UserReminder(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        //Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            if (Request.QueryString["ResetPassword"] != null)
            {
                DisplayMessage("Your Password has been expired , Please reset your password");
                txtOldPassword.Focus();
            }
            //lblLoginSec.Visible = true;
            //lblEmailSec.Visible = false;
        }
    }


    public bool IsValiduser(string strCompanyId, string struserId)
    {
        if (objDa.return_DataTable("select set_usermaster.user_id from set_usermaster inner join Set_EmployeeMaster on set_usermaster.Emp_Id= Set_EmployeeMaster.Emp_Id where set_usermaster.company_id='" + strCompanyId + "' and  set_usermaster.user_id='" + struserId + "'  and Set_EmployeeMaster.IsActive='True' and Set_EmployeeMaster.Field2='False'").Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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
        if (txtOldPassword.Text == "")
        {
            DisplayMessage("Enter old password");
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
        string strIsModified = string.Empty;

        if (Session["UserId"].ToString().ToLower().Trim() != "superadmin" && Session["UserId"] != null)
        {
            DataTable dt = new DataTable();
            dt = objUser.GetUserMasterByUserId(Session["UserId"].ToString().Trim(), Session["CompId"].ToString());

            if (dt.Rows.Count > 0)
            {
                strIsModified = dt.Rows[0]["Is_Modified"].ToString();

                if (txtOldPassword.Text.Trim() != Common.Decrypt(dt.Rows[0]["Password"].ToString().Trim()))
                {
                    DisplayMessage("Enter Correct Old Password");

                    return;
                }
                else if (txtOldPassword.Text.Trim() == txtReEnterPass.Text.Trim())
                {
                    DisplayMessage("Old password and new password can not be same");
                    return;
                }
                else
                {
                    objUser.UpdateUserMaster(Session["UserId"].ToString(), Session["LoginCompany"].ToString(), Session["UserId"].ToString(), Common.Encrypt(txtNewPassword.Text), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Role_Id"].ToString(), strIsModified, dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), "", "", dt.Rows[0]["Field6"].ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["IsGlobalAccess"].ToString());

                    int ReminderDays = 0;

                    try
                    {


                        ReminderDays = Convert.ToInt32(objDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Password Reminder(In Days)' and Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + dt.Rows[0]["Brand_Id"].ToString() + "' and Location_Id='" + dt.Rows[0]["Location_Id"].ToString() + "'").Rows[0][0].ToString());
                    }
                    catch
                    {
                        ReminderDays = 30;
                    }


                    if (ReminderDays > 0)
                    {

                        ObjUserReminder.updateRecord(Session["UserId"].ToString(), DateTime.Now.AddDays(ReminderDays).ToString(), "1", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        ObjUserReminder.InsertRecord(Session["UserId"].ToString(), DateTime.Now.AddDays(ReminderDays).ToString(), "0", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }

                    DisplayMessage("Login Password has been changed");
                    btnReset_Click(null, null);
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
            }
        }
        else if (Session["UserId"].ToString().Trim() == "superadmin")
        {
            DataTable dt = new DataTable();
            dt = objUser.GetUserMasterByUserId(Session["UserId"].ToString().Trim(), Session["CompId"].ToString());

            if (dt.Rows.Count > 0)
            {
                strIsModified = dt.Rows[0]["Is_Modified"].ToString();

                if (txtOldPassword.Text.Trim() != Common.Decrypt(dt.Rows[0]["Password"].ToString().Trim()))
                {
                    DisplayMessage("Enter Correct Old Password");
                    return;
                }
                else if (txtOldPassword.Text.Trim() == txtReEnterPass.Text.Trim())
                {
                    DisplayMessage("Old password and new password can not be same");
                    return;
                }
                else
                {
                    objUser.UpdateUserMaster(Session["UserId"].ToString(), "0", Session["UserId"].ToString(), Common.Encrypt(txtNewPassword.Text), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Role_Id"].ToString(), strIsModified, dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["IsGlobalAccess"].ToString());


                    DisplayMessage("Login Password has been changed");
                    btnReset_Click(null, null);
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
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

    protected void lnkDashboard_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Dashboard/AttendanceDashboard.aspx");
    }
}
