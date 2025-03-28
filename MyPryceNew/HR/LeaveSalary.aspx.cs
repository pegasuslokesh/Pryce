using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class HR_LeaveSalary : System.Web.UI.Page
{
    Common cmn = null;
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    IT_ObjectEntry objObjectEntry = null;
    HR_Leave_Salary objLeaveSalary = null;
    Pay_Employee_claim objPayEmpClaim = null;
    Attendance objAtt = null;
    Pay_Employee_claim ObjClaim = null;
    Attendance objAttendance = null;
    CurrencyMaster objcurrency = null;
    Set_ApplicationParameter objAppParam = null;
    PageControlCommon objPageCmn = null;

    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objLeaveSalary = new HR_Leave_Salary(Session["DBConnection"].ToString());
        objPayEmpClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        objAtt = new Attendance(Session["DBConnection"].ToString());
        ObjClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objcurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "257", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            txtApplyLeaveCount.Text = "0";
            txtApplyLeaveSalarytotal.Text = "0";
            txtEmpName.Focus();
            lblClaimTotal.Text = "0";
            lblFinalTotal.Text = "0";
            //txtUse.Text = "0";
            //FinalTotal.Text = "0";
            txtLeaveCount.Text = "0";
            string symbol = objcurrency.GetCurrencyMasterById(Session["Currencyid"].ToString()).Rows[0]["Currency_symbol"].ToString();
            try
            {
                ViewState["Symbol"] = symbol.ToString();
            }
            catch
            {
                ViewState["Symbol"] = "";
            }
        }
        AllPageCode();
    }

    public void AllPageCode()
    {
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("257", (DataTable)Session["ModuleName"]);
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
        StrUserId = Session["UserId"].ToString();

        if (Session["EmpId"].ToString() == "0")
        {

            btnSave.Visible = true;
        }
        else
        {

            DataTable dtAllPageCode = cmn.GetAllPagePermission(StrUserId.ToString(), strModuleId, "257",Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ErpLogin.aspx");
            }

            foreach (DataRow DtRow in dtAllPageCode.Rows)
            {
                if (DtRow["Op_Id"].ToString() == "1")
                {
                    btnSave.Visible = true;
                    break;
                }
            }
        }
    }
    protected void btnLeaveSalary_Click(object sender, EventArgs e)
    {
        DataTable dtEmp = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dtEmp = new DataView(dtEmp, "Emp_Id='" + hdnEmpId.Value + "' and Field2='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtEmp.Rows.Count > 0)
        {
            DisplayMessage("Employee is Terminated You can see his report only");
            return;
        }

        RefreshData();
        lblClaimTotal.Text = "0";
        lblFinalTotal.Text = "0";
        //txtUse.Text = "0";

        //FinalTotal.Text = "0";

        if (txtEmpName.Text != "")
        {

            //here we are checking the leave maturity 









            //DataTable dt = objLeaveSalary.GetAllLeaveSalaryByEmpID(hdnEmpId.Value);
            //if (dt.Rows.Count > 0)
            //{
            //if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString())) == true)
            //{
            ViewState["Count"] = 0;
            FillGridLeaveDetail();
            FillGrid();
            FillGrid_LeaveSalaryTaken();
            //FillGridClaim();
            //if (Convert.ToInt32(ViewState["Count"]) > 0)
            //{


            //    //if (lblFinalTotal.Text != "")
            //    //{
            //    //    if (lblClaimTotal.Text != "")
            //    //    {
            //    //        FinalTotal.Text = (Convert.ToDouble(lblFinalTotal.Text) - Convert.ToDouble(lblClaimTotal.Text)).ToString();

            //    //    }
            //    //    else
            //    //    {
            //    //        FinalTotal.Text = (Convert.ToDouble(lblFinalTotal.Text)).ToString();
            //    //    }
            //    //}
            //    //FinalTotal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), FinalTotal.Text);
            //    //lblFinalTotal2Curr.Text = "(" + ViewState["Symbol"].ToString() + ")";
            //    //divPending.Visible = true;
            //}
            //else
            //{
            //    DisplayMessage("No Records Available");
            //    return;
            //}
            //}
            //else
            //{
            //    DisplayMessage("You have no record");
            //    txtEmpName.Text = "";
            //    txtEmpName.Focus();
            //    return;
            //}
        }
        else
        {
            DisplayMessage("Select Employee");
            txtEmpName.Text = "";
            txtEmpName.Focus();
            return;
        }
    }
    private void FillGridLeaveDetail()
    {
        DataTable dt = objAtt.GetLeaveRequestByEmpId(Session["CompId"].ToString(), hdnEmpId.Value);
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveDetail, dt, "", "");
        Session["dtFilterLeave"] = dt;
        if (dt.Rows.Count > 0)
        {
            divLeave.Visible = true;
            ViewState["Count"] = 1;
        }
        else
        {
            divLeave.Visible = false;
            ViewState["Count"] = 0;
        }
    }
    private void FillGrid()
    {
        DataTable dt = objLeaveSalary.GetAllLeaveSalaryByEmpID(hdnEmpId.Value);

        if (dt.Rows.Count > 0)
        {
            lblFinalTotal.Text = dt.Rows[0]["FinalTotal"].ToString();
            txtLeaveCount.Text = dt.Rows[0]["TotalLeaveCount"].ToString();
        }

        lblFinalTotal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), lblFinalTotal.Text);
        lblFinalTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";
        lblApplyFinalTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveSalary, dt, "", "");
        TotalLeaveSum();

        Session["dtFilter_Leave_Sal"] = dt;
        if (dt.Rows.Count > 0)
        {
            divLeaveSalary.Visible = true;
            ViewState["Count"] = Convert.ToInt32(ViewState["Count"]) + 1;
            divPending.Visible = true;
        }
        else
        {
            divLeaveSalary.Visible = false;
            divPending.Visible = false;
        }


    }
    private void FillGrid_LeaveSalaryTaken()
    {
        DataTable dt = objLeaveSalary.GetAllLeaveSalaryByEmpID_LeaveTakenNew(hdnEmpId.Value);

        //lblFinalTotal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), lblFinalTotal.Text);
        //lblFinalTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";
        lblClaimTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployeeTakenLeave, dt, "", "");
        TotalLeaveSum_SalaryTaken();

        //Session["dtFilter_Leave_Sal"] = dt;
        if (dt.Rows.Count > 0)
        {
            divClaim.Visible = true;
            //ViewState["Count"] = Convert.ToInt32(ViewState["Count"]) + 1;
        }
        else
        {
            divClaim.Visible = false;
        }


    }
    protected void gvLeaveDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveDetail.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterLeave"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveDetail, dt, "", "");
    }
    protected void gvLeaveDetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilterLeave"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDirLeave"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDirLeave"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDirLeave"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDirLeave"] = "DESC";
        }

        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDirLeave"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtFilterLeave"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveDetail, dt, "", "");
        // AllPageCode();
    }
    protected void gvLeaveSalary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveSalary.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Leave_Sal"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveSalary, dt, "", "");
    }
    protected void gvLeaveSalary_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Leave_Sal"];
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
        Session["dtFilter_Leave_Sal"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveSalary, dt, "", "");
        //AllPageCode();
        try
        {
            gvLeaveSalary.HeaderRow.Focus();
        }
        catch
        {
        }
    }
    private void FillGridClaim()
    {
        //DataTable dt = objPayEmpClaim.Get_PayEmployeeClaim_By_EmpId_LeaveSalary(Session["CompId"].ToString(), hdnEmpId.Value);

        //if (dt != null && dt.Rows.Count > 0)
        //{
        //    lblClaimTotal.Text = dt.Rows[0]["FinalTotal"].ToString();
        //}
        //lblClaimTotal.Text = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), lblClaimTotal.Text);
        //lblClaimTotalCurr.Text = "(" + ViewState["Symbol"].ToString() + ")";

        //gvEmployeeClaim.DataSource = dt;
        //gvEmployeeClaim.DataBind();
        //if (dt.Rows.Count > 0)
        //{

        //    lblClaimTotal.Visible =true ;
        //    ViewState["Count"] = Convert.ToInt32(ViewState["Count"]) + 1;
        //}
        //else
        //{
        //    lblClaimTotal.Visible = false;
        //}
        //Session["dtFilterClaim"] = dt;
        //if (dt.Rows.Count > 0)
        //{
        //    divClaim.Visible = true;
        //}
        //else
        //{
        //    divClaim.Visible = false;
        //}

    }
    protected void gvEmployeeClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gvEmployeeClaim.PageIndex = e.NewPageIndex;
        //DataTable dt = (DataTable)Session["dtFilterClaim"];
        //gvEmployeeClaim.DataSource = dt;
        //gvEmployeeClaim.DataBind();

    }
    protected void gvEmployeeClaim_Sorting(object sender, GridViewSortEventArgs e)
    {
        //    DataTable dt = (DataTable)Session["dtFilterClaim"];
        //    string sortdirclaim = "DESC";
        //    if (ViewState["SortDirClaim"] != null)
        //    {
        //        sortdirclaim = ViewState["SortDirClaim"].ToString();
        //        if (sortdirclaim == "ASC")
        //        {
        //            e.SortDirection = SortDirection.Descending;
        //            ViewState["SortDirClaim"] = "DESC";
        //        }
        //        else
        //        {
        //            e.SortDirection = SortDirection.Ascending;
        //            ViewState["SortDirClaim"] = "ASC";
        //        }
        //    }
        //    else
        //    {
        //        ViewState["SortDirClaim"] = "DESC";
        //    }

        //    dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDirClaim"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        //    Session["dtFilterClaim"] = dt;
        //    gvEmployeeClaim.DataSource = dt;
        //    gvEmployeeClaim.DataBind();
        //    //AllPageCode();

    }
    protected void txtUse_OnTextChanged(object sender, EventArgs e)
    {
        //if (txtUse.Text != "")
        //{
        //    if (Convert.ToDouble(txtUse.Text) > Convert.ToDouble(FinalTotal.Text))
        //    {
        //        DisplayMessage("Used Cannot be greater than pending");
        //        txtUse.Text = "";
        //        txtUse.Focus();
        //        return;

        //    }
        //}
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;

        if (txtEmpName.Text != "")
        {
            empid = txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hdnEmpId.Value = empid;
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtEmpName.Text = "";
                txtEmpName.Focus();
                return;
            }
        }
        else
        {
            RefreshData();
        }
    }

    public void RefreshData()
    {
        gvEmployeeTakenLeave.DataSource = null;
        gvEmployeeTakenLeave.DataBind();
        //gvEmployeeClaim.DataSource = null;
        //gvEmployeeClaim.DataBind();
        gvLeaveDetail.DataSource = null;
        gvLeaveDetail.DataBind();
        gvLeaveSalary.DataSource = null;
        gvLeaveSalary.DataBind();
        divLeave.Visible = false;
        lblClaimTotal.Text = "0";
        lblFinalTotal.Text = "0";
        //txtUse.Text = "0";
        //FinalTotal.Text = "0";
        txtApplyLeaveCount.Text = "0";
        txtApplyLeaveSalarytotal.Text = "0";
        txtLeaveCount.Text = "0";
        divPending.Visible = false;
        divLeaveSalary.Visible = false;
        divLeave.Visible = false;
        divClaim.Visible = false;
        //txtEmpName.Text = "";
        //txtEmpName.Focus();
        Session["dtFilterClaim"] = null;
        Session["dtFilter_Leave_Sal"] = null;
        Session["dtFilterLeave"] = null;
        ViewState["SortDirClaim"] = null;
        ViewState["SortDir"] = null;
        ViewState["SortDirLeave"] = null;
        //ViewState["Symbol"] = "";
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, "0",HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (gvLeaveSalary.Rows.Count > 0)
        {

            //here  are checking that employee finished leave maturity or not , if maturity assigned on leave level 

            //code start


            foreach (GridViewRow gvrow in gvLeaveSalary.Rows)
            {

                if (((CheckBox)gvrow.FindControl("chkselect")).Checked)
                {

                    if (!objAttendance.IsLeaveMaturity(hdnEmpId.Value, ((Label)gvrow.FindControl("lblLeaveTypeId")).Text,Session["CompId"].ToString(),Session["TimeZoneId"].ToString()))
                    {
                        DisplayMessage("Employee not eligible");
                        return;
                    }
                }
            }



            //code end




            objAttendance.DeleteLeaveSalaryClaim(hdnEmpId.Value, DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString());
            string EmpPayrollPosted = objAttendance.GePayrollPostedByEmpId(Session["CompId"].ToString(), hdnEmpId.Value, DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString());
            if (EmpPayrollPosted.ToString() != string.Empty)
            {
                DisplayMessage("Payroll Already Posted For Employee :-" + EmpPayrollPosted);
                return;
            }

            HR_Leave_Salary objLeaveSal = new HR_Leave_Salary(Session["DBConnection"].ToString());
            foreach (GridViewRow gvrow in gvLeaveSalary.Rows)
            {

                if (((CheckBox)gvrow.FindControl("chkselect")).Checked)
                {


                    b = objLeaveSal.UpdateLeaveSalaryStatus(((Label)gvrow.FindControl("lblTransId")).Text, DateTime.Now.ToString());
                }
            }


            //b = ObjClaim.Insert_In_Pay_Employee_ClaimRequest(Session["CompId"].ToString(), hdnEmpId.Value, "Leave Salary", "Assigned Leave Salary", "1", txtUse.Text, DateTime.Now.ToString(), "Approved", DateTime.Now.ToString(), DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }
        else
        {
            DisplayMessage("Record Not Found");

            return;
        }

        if (b != 0)
        {
            DisplayMessage("Record Saved","green");
            RefreshData();
            return;

        }
        else
        {
            DisplayMessage("Record Not Saved");
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        RefreshData();
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

    public string GetMonthName(int monthNumber)
    {
        string strMonthName = "";
        return strMonthName;
        //CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthNumber);

    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }

    #region Report

    protected void btnReport_Click(object sender, EventArgs e)
    {
        if (txtEmpName.Text != "")
        {
            //if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString())) == false)
            //{
            //DataTable dt = objLeaveSalary.GetAllLeaveSalaryByEmpID(hdnEmpId.Value);
            //if (dt.Rows.Count > 0)
            //{
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/EmployeeLeaveSalary.aspx?EmpId=" + hdnEmpId.Value + "','window','width=1024');", true);
            //}
            //else
            //{
            //    DisplayMessage("Record Not available");
            //    return;
            //}

            //}
            //else
            //{
            //    DisplayMessage("You are Not Applicable");
            //    txtEmpName.Text = "";
            //    txtEmpName.Focus();
            //    return;
            //}
        }
        else
        {
            DisplayMessage("Select Employee");
            txtEmpName.Text = "";
            txtEmpName.Focus();
            return;
        }
    }
    #endregion

    public void TotalLeaveSum_SalaryTaken()
    {

        float TotalLeaveSalary = float.Parse(txtApplyLeaveSalarytotal.Text);
        foreach (GridViewRow gvrow in gvEmployeeTakenLeave.Rows)
        {

            try
            {
                TotalLeaveSalary += float.Parse(((Label)gvrow.FindControl("lblTotal")).Text);
            }
            catch
            {
                TotalLeaveSalary += 0;
            }

        }
        lblClaimTotal.Text = TotalLeaveSalary.ToString();

    }
    public void TotalLeaveSum()
    {
        float TotalLeaveCount = float.Parse(txtApplyLeaveCount.Text);
        float TotalLeaveSalary = float.Parse(txtApplyLeaveSalarytotal.Text);
        foreach (GridViewRow gvrow in gvLeaveSalary.Rows)
        {
            try
            {
                TotalLeaveCount += float.Parse(((Label)gvrow.FindControl("lblLeaveCount")).Text);
            }
            catch
            {
                TotalLeaveCount += 0;
            }
            try
            {
                TotalLeaveSalary += float.Parse(((Label)gvrow.FindControl("lblTotal")).Text);
            }
            catch
            {
                TotalLeaveSalary += 0;
            }

        }
        txtLeaveCount.Text = TotalLeaveCount.ToString();
        lblFinalTotal.Text = TotalLeaveSalary.ToString();
    }

    protected void chkSelect_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;

        float TotalLeaveCount = float.Parse(txtApplyLeaveCount.Text);
        float TotalLeaveSalary = float.Parse(txtApplyLeaveSalarytotal.Text);



        if (((CheckBox)gvrow.FindControl("chkselect")).Checked)
        {
            try
            {
                TotalLeaveCount += float.Parse(((Label)gvrow.FindControl("lblLeaveCount")).Text);
            }
            catch
            {
                TotalLeaveCount += 0;
            }
            try
            {
                TotalLeaveSalary += float.Parse(((Label)gvrow.FindControl("lblTotal")).Text);
            }
            catch
            {
                TotalLeaveSalary += 0;
            }
        }
        else
        {
            try
            {
                TotalLeaveCount -= float.Parse(((Label)gvrow.FindControl("lblLeaveCount")).Text);
            }
            catch
            {
                TotalLeaveCount -= 0;
            }
            try
            {
                TotalLeaveSalary -= float.Parse(((Label)gvrow.FindControl("lblTotal")).Text);
            }
            catch
            {
                TotalLeaveSalary -= 0;
            }

        }
        txtApplyLeaveCount.Text = TotalLeaveCount.ToString();
        txtApplyLeaveSalarytotal.Text = TotalLeaveSalary.ToString();

    }

}