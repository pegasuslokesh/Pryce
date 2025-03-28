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

public partial class HR_HR_IndemnityLeaveRequest : System.Web.UI.Page
{
    HR_Indemnity_Master objIndemnity = null;
    SystemParameter objSys = null;
    Set_ApplicationParameter objAppParam = null;
    Set_Approval_Employee objApproalEmp = null;
    Att_Leave_Request objleaveReq = null;
    Att_Employee_Leave objEmpleave = null;
    PageControlCommon objPageCmn = null;
    Common cmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objIndemnity = new HR_Indemnity_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "253", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillEmployeeIndemnityLeave();
            FillEmployeeIndemnityLeavePending();
            btnNew_Click(null, null);
        }
        AllPageCode();
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("253", (DataTable)Session["ModuleName"]);
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
        Page.Title = objSys.GetSysTitle();

        if (Session["EmpId"].ToString() == "0")
        {
            btnApply.Visible = true;
        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "253",Session["CompId"].ToString());

            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {


                    foreach (GridViewRow Row in gvLeaveStatus.Rows)
                    {
                        //((ImageButton)Row.FindControl("IbtnReject")).Visible = true;
                        //((ImageButton)Row.FindControl("IbtnApprove")).Visible = true;
                        //  ((ImageButton)Row.FindControl("IbtnEdit")).Visible = true;
                    }
                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            btnApply.Visible = true;
                        }
                    }

                }


            }
        }


    }

    private void FillEmployeeIndemnityLeavePending()
    {

    }
    protected void btnApply_Click(object sender, EventArgs e)
    {

        // Approval For Indemnity Leave
        DataTable dt1 = new DataTable();
        string EmpPermission = string.Empty;
        EmpPermission = objSys.GetSysParameterByParamName("Approval Setup").Rows[0]["Param_Value"].ToString();

        dt1 = objApproalEmp.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(),"253", Session["EmpId"].ToString());
        if (dt1.Rows.Count == 0)
        {
            DisplayMessage("Approval setup issue , please contact to your admin");
            return;
        }



        // Indemnity Leave Request
        int b = 0;
        b = objleaveReq.InsertLeaveRequest(Session["CompId"].ToString(), "1", Session["EmpId"].ToString(), DateTime.Now.ToString(), txtFromDate.Text, txtToDate.Text, true.ToString(), false.ToString(), false.ToString(), txtDescription.Text, "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


        // Insert In Approval Transaction Table...............
        if (dt1.Rows.Count > 0)
        {

            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                string PriorityEmpId = dt1.Rows[j]["Emp_Id"].ToString();
                string IsPriority = dt1.Rows[j]["Priority"].ToString();
                if (EmpPermission == "1")
                {
                    objApproalEmp.InsertApprovalTransaciton("1", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                }
                else if (EmpPermission == "2")
                {
                    objApproalEmp.InsertApprovalTransaciton("1", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                }
                else if (EmpPermission == "3")
                {
                    objApproalEmp.InsertApprovalTransaciton("1", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                }
                else
                {
                    objApproalEmp.InsertApprovalTransaciton("1", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                }
            }
        }
        string TransNo = string.Empty;

        // objEmpleave.UpdateEmployeeLeaveTransactionByTransNo(TransNo, Session["CompId"].ToString(), Session["EmpId"].ToString(), "1", "2014", "0", "0", "0", "0", "0", "0", lblDays.Text, "0", Session["UserId"].ToString(), DateTime.Now.ToString());

        DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
        while (FromDate <= ToDate)
        {
            bool IsPaid = true;
            objleaveReq.InsertLeaveRequestChild(b.ToString(), "1", FromDate.ToString(), IsPaid.ToString(), "1", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            FromDate = FromDate.AddDays(1);
            txtDescription.Focus();
        }
        DisplayMessage("Indemnity Leave Submited");
        return;
    }
    protected void txtFromDate_textChanged(object sender, EventArgs e)
    {
        DataTable IndLeave = (DataTable)Session["IndemnityLeave"];
        if (IndLeave.Rows.Count > 0)
        {
            if (txtFromDate.Text == "")
            {
                txtFromDate.Focus();
                DisplayMessage("Enter From Date");
                return;
            }
            int IndemnityLeaveDays = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityLeave", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            txtToDate.Text = Convert.ToDateTime(txtFromDate.Text).AddDays(IndemnityLeaveDays - 1).ToString();
            lblDays.Text = Convert.ToString((Convert.ToDateTime(txtToDate.Text) - Convert.ToDateTime(txtFromDate.Text)).TotalDays);
            lblDays.Text = Convert.ToString(Convert.ToInt32(lblDays.Text) + 1);
            //.............................
        }

        else
        {
            DisplayMessage("You have Already Used or do not having Indemnity Leave");
            return;
        }
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        dvList.Visible = false;
        dvNew.Visible = true;
    }
    protected void btnList_Click(object sender, EventArgs e)
    {

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        dvList.Visible = true;
        dvNew.Visible = false;
    }
    public string GetDate(object obj)
    {

        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());
        return Date.ToString(objSys.SetDateFormat());
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        lblDays.Text = "";
        txtDescription.Text = "";
        txtFromDate.Focus();
        Session["dtLeaveStatus"] = "";
        return;
    }
    public string GetLeaveStatus(object TransId)
    {
        string status = string.Empty;
        //DataTable dt = objleaveReq.GetLeaveRequest(Session["CompId"].ToString());
        //dt = new DataView(dt, "Trans_Id='" + TransId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //if (dt.Rows.Count > 0)
        //{
        //    if (Convert.ToBoolean(dt.Rows[0]["Is_Pending"].ToString()))
        //    {
        //        status = "Pending";
        //    }
        //    else if (Convert.ToBoolean(dt.Rows[0]["Is_Approved"].ToString()))
        //    {
        //        status = "Approved";
        //    }
        //    else if (Convert.ToBoolean(dt.Rows[0]["Is_Canceled"].ToString()))
        //    {
        //        status = "Rejected";
        //    }

        //}

        return status;

    }

    private void FillEmployeeIndemnityLeave()
    {
        DataTable IndemnityLeave = objIndemnity.GetIndemnityLeaveByEmpId(Session["EmpId"].ToString());
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeave, IndemnityLeave, "", "");
        Session["IndemnityLeave"] = IndemnityLeave;
    }
}
