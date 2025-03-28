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
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class HR_GenerateEmpPayroll : BasePage
{
    #region defind Class Object
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Voucher_Header objVoucherHeader = null;
    DepartmentMaster objDep = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Att_Employee_Leave objEmpleave = null;
    Pay_Employee_Due_Payment objEmpDuePay = null;
    Pay_Employee_Attendance objEmpAttendance = null;
    Pay_Employee_Loan objEmpLoan = null;
    Pay_Employee_Month objPayEmpMonth = null;
    Pay_Employee_Deduction objpayrolldeduc = null;
    Pay_Employe_Allowance objpayrollall = null;
    Pay_Employee_Penalty objPEpenalty = null;
    Pay_Employee_claim objPEClaim = null;
    Common ObjComman = null;
    Set_Pay_Employee_Allow_Deduc ObjAllDeduc = null;
    Set_Allowance ObjAllow = null;
    Set_Deduction ObjDeduc = null;
    EmployeeParameter objempparam = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    SystemParameter objSys = null;
    Pay_Employee_claim ObjClaim = null;
    Att_Employee_Notification objEmpNotice = null;
    Set_ApplicationParameter objAppParam = null;
    Pay_Employee_Attendance objPayEmpAtt = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    HR_Indemnity_Master objIndemnity = null;
    RoleMaster objRole = null;
    Attendance objAttendance = null;
    Att_Employee_HalfDay objEmpHalfDay = null;
    HR_Leave_Salary objLeaveSal = null;
    HR_EmployeeDetail objEmpDetail = null;
    DataAccessClass da = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Pay_SalaryPlanDetail objsalaryPlan = null;
    Set_DeductionDetail objdeductiondetail = null;
    Pay_AdvancePayment objAdvancePayment = null;
    Pay_SalaryPlanDetail objsalaryplandetail = null;
    Pay_EmployeeArrear ObjEmpArrear = null;
    LogProcess ObjLogProcess = null;
    PageControlCommon objPageCmn = null;

    double Total_Days = 0;
    #endregion
    string Narration_Month = string.Empty;
    CurrencyMaster objCurrency = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objEmpDuePay = new Pay_Employee_Due_Payment(Session["DBConnection"].ToString());
        objEmpAttendance = new Pay_Employee_Attendance(Session["DBConnection"].ToString());
        objEmpLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objpayrolldeduc = new Pay_Employee_Deduction(Session["DBConnection"].ToString());
        objpayrollall = new Pay_Employe_Allowance(Session["DBConnection"].ToString());
        objPEpenalty = new Pay_Employee_Penalty(Session["DBConnection"].ToString());
        objPEClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjAllDeduc = new Set_Pay_Employee_Allow_Deduc(Session["DBConnection"].ToString());
        ObjAllow = new Set_Allowance(Session["DBConnection"].ToString());
        ObjDeduc = new Set_Deduction(Session["DBConnection"].ToString());
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objPayEmpAtt = new Pay_Employee_Attendance(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objIndemnity = new HR_Indemnity_Master(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objEmpHalfDay = new Att_Employee_HalfDay(Session["DBConnection"].ToString());
        objLeaveSal = new HR_Leave_Salary(Session["DBConnection"].ToString());
        objEmpDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objsalaryPlan = new Pay_SalaryPlanDetail(Session["DBConnection"].ToString());
        objdeductiondetail = new Set_DeductionDetail(Session["DBConnection"].ToString());
        objAdvancePayment = new Pay_AdvancePayment(Session["DBConnection"].ToString());
        objsalaryplandetail = new Pay_SalaryPlanDetail(Session["DBConnection"].ToString());
        ObjEmpArrear = new Pay_EmployeeArrear(Session["DBConnection"].ToString());
        ObjLogProcess = new LogProcess(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
           
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../HR/GenerateEmpPayroll.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            string amount = Math.Round(25.0, 0).ToString();
            ddlMonth.SelectedIndex = System.DateTime.Now.Month;
            TxtYear.Text = DateTime.Now.Year.ToString();
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
            }

            ViewState["CurrIndex"] = 0;
            ViewState["SubSize"] = 9;
            ViewState["CurrIndexbin"] = 0;
            ViewState["SubSizebin"] = 9;
            Session["dtPayroll"] = null;
            FillddlLocation();
            FillddlDeaprtment();
            Session["empimgpath"] = null;

            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            btnPayroll_Click(null, null);
            bool IsCompOT = false;
            bool IsPartialComp = false;
            try
            {
                IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
                IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Payroll_Enable", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            }
            catch
            {

            }

            //this code is created by jitendra upadhyay on 17-07-2014
            //this code for set the gridview page size according the system parameter
            try
            {
                gvEmployee.PageSize = int.Parse(Session["GridSize"].ToString());
                gvEmpPayroll.PageSize = int.Parse(Session["GridSize"].ToString());
            }
            catch
            {

            }
            ValidEmpList();
            ddlLocation.Focus();
            GetSetAccountDetail();
            CalendarExtendertxtValueDate.Format = objSys.SetDateFormat();
            Set_Default_Credit_Account();
            Session["TerminateEmpId"] = null;
        }

        Page.Title = objSys.GetSysTitle();

      
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnGenratePayroll.Visible = clsPagePermission.bEdit;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
    }

    public void Set_Default_Credit_Account()
    {
        txtCreditAccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "HR Section").Rows[0]["Param_Value"].ToString());
    }

    public void GetSetAccountDetail()
    {
        string strCashAccount = string.Empty;
        string strHRAccount = string.Empty;
        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());
        DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCredit.Rows.Count > 0)
        {
            string strAccountId = dtCredit.Rows[0]["Param_Value"].ToString();
            DataTable dtAcc = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
            if (dtAcc.Rows.Count > 0)
            {
                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                txtCreditAccount.Text = strAccountName + "/" + strAccountId;
            }
        }

        DataTable dtDebit = new DataView(dtAcParameter, "Param_Name='HR Section'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDebit.Rows.Count > 0)
        {
            string strAccountId = dtDebit.Rows[0]["Param_Value"].ToString();
            DataTable dtAcc = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
            if (dtAcc.Rows.Count > 0)
            {
                string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                txtDebitAccount.Text = strAccountName + "/" + strAccountId;
            }
        }
    }
    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlLocation, dtLoc, "Location_Name", "Location_Id");
        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
        }
    }


    public void FillddlDeaprtment()
    {
        DataTable dtDepartment = PageControlCommon.GetEmployeeDepartmentByLocationId(ddlLocation, dpDepartment, Session["DBConnection"].ToString());
        objPageCmn.FillData((object)dpDepartment, dtDepartment, "Dep_Name", "Dep_Id");
    }

    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillddlDeaprtment();
        FillGrid();
       
    }
    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    public void FillGrid()
    {
        string strDepId = string.Empty;
        string strLocationId = string.Empty;

        DataTable dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());
        
        try
        {

            if (Session["TerminateEmpId"].ToString() == null)
            {
                Session["TerminateEmpId"] = "";
            }

            if (Session["TerminateEmpId"].ToString() != "")
            {
                Session["CHECKED_ITEMS"] = null;
                ddlField1.SelectedIndex = 1;
                ddlOption1.SelectedIndex = 2;
                txtValue1.Text = "";
                dtEmp = new DataView(dtEmp, "Emp_Id='" + Session["TerminateEmpId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DateTime Date = Convert.ToDateTime(Session["TerminationDate"].ToString());
                ddlMonth.SelectedValue = Date.Month.ToString();
                TxtYear.Text = Date.Year.ToString();
                ddlMonth.Enabled = false;
                TxtYear.Enabled = false;
            }
            else
            {
                ddlMonth.Enabled = true;
                TxtYear.Enabled = true;
                if (ddlLocation.SelectedIndex == 0 && dpDepartment.SelectedIndex == 0)
                {

                    Session["CHECKED_ITEMS"] = null;
                    ddlField1.SelectedIndex = 1;
                    ddlOption1.SelectedIndex = 2;
                    txtValue1.Text = "";
                }


                if (dpDepartment.SelectedIndex != 0)
                {
                    dtEmp = new DataView(dtEmp, "Department_Id = " + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

                }
            }
        }
        catch (Exception Ex)
        {
        }

        string Validemplist = string.Empty;
        foreach (DataRow dr in dtEmp.Rows)
        {
            if (ValidPayRoll(dr["Emp_Id"].ToString()) != "")
            {
                Validemplist += ValidPayRoll(dr["Emp_Id"].ToString()).ToString();
            }
        }
        if (dtEmp.Rows.Count > 0)
        {
            if (Validemplist != "")
            {
                dtEmp = new DataView(dtEmp, "Emp_Id in(" + Validemplist.Substring(0, Validemplist.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                Session["dtEmpPayroll"] = dtEmp;
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmpPayroll, dtEmp, "", "");
                lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            }
            else
            {
                gvEmpPayroll.DataSource = null;
                gvEmpPayroll.DataBind();
                Session["dtEmpPayroll"] = null;
                lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : 0";
            }
        }
        else
        {
            gvEmpPayroll.DataSource = null;
            gvEmpPayroll.DataBind();
            Session["dtEmpPayroll"] = dtEmp;
            lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
      
    }
    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["dtEmpPayroll"] != null)
        {
            DataTable dtFilter = (DataTable)Session["dtEmpPayroll"];
            if (dtFilter.Rows.Count > 0)
            {
                if (ddlFilter.SelectedValue == "All")
                {

                }
                else if (ddlFilter.SelectedValue == "Not Generated")
                {

                }
                else if (ddlFilter.SelectedValue == "Generated")
                {
                    dtFilter = new DataView(dtFilter, "", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (ddlFilter.SelectedValue == "Posted")
                {

                }
            }
            else
            {

            }
        }
    }
    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid();
        Session["dtPayroll"] = null;
      
        ddlLocation.Focus();
    }
    protected void imgValidEmployee_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlMonth.SelectedIndex == 0)
        {
            DisplayMessage("Select Month");
            ddlMonth.Focus();
            return;
        }
        if (TxtYear.Text.Trim() == "" || TxtYear.Text.Trim() == "0")
        {
            DisplayMessage("Enter Year");
            TxtYear.Focus();
            return;
        }
        if (rbtnGroup.Checked)
        {
            lbxGroup_SelectedIndexChanged(null, null);
        }
        else
        {
            FillGrid();

        }

        FillVoucherReportGrid();

    }
    protected void ddlMonth_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        imgValidEmployee_Click(null, null);
        FillVoucherReportGrid();
    }
  
    protected void lbxGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroup.Items.Count; i++)
        {
            if (lbxGroup.Items[i].Selected == true)
            {
                GroupIds += lbxGroup.Items[i].Value + ",";
            }
        }
        if (GroupIds != "")
        {
            DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());
            
            DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

            dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
            {
                if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                {
                    //here we check that employee is valid or not for selected month and year'

                    if (ValidPayRoll(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()) != "")
                    {
                        EmpIds += ValidPayRoll(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()).ToString();
                    }
                }
            }



            if (EmpIds != "")
            {
                dtEmp = new DataView(dtEmp, "Emp_Id in(" + EmpIds.Substring(0, EmpIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            }
            else
            {
                dtEmp = new DataTable();
            }









            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmp1"] = dtEmp;
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            }
            else
            {
                Session["dtEmp1"] = null;
                gvEmployee.DataSource = null;
                gvEmployee.DataBind();

            }

        }
        else
        {
            gvEmployee.DataSource = null;
            gvEmployee.DataBind();

        }
    }
    protected void gvEmpPayroll_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        SaveCheckedValues();
        gvEmpPayroll.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtEmpPayroll"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpPayroll, dt, "", "");
        PopulateCheckedValues();
      
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmpPayroll.Rows)
            {
                int index = (int)gvEmpPayroll.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvEmpPayroll.Rows)
        {
            index = (int)gvEmpPayroll.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;


            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);

        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void chkgvSelectAll_CheckedChangedPayroll(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmpPayroll.HeaderRow.FindControl("chkgvSelectAll"));
        bool result = false;
        if (chkSelAll.Checked == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvEmpPayroll.Rows)
        {
            index = (int)gvEmpPayroll.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];


            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }



        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;

    }

    protected void chkgvSelect_CheckedChangedPayroll(object sender, EventArgs e)
    {
        lblPostFirst.Text = "";
        lblAlreadyPosted.Text = "";
        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvEmpPayroll.Rows[index].FindControl("lblEmpId");
        if (((CheckBox)gvEmpPayroll.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            //add employee id
            string strTempEmpId = lb.Text.Trim().ToString();

            strTempEmpId = ValidPayRoll(strTempEmpId);

            if (strTempEmpId.Length > 0)
            {
                empidlist += lb.Text.Trim().ToString() + ",";
                lblSelectRecd.Text += empidlist;
            }
            else
            {
                ((CheckBox)gvEmpPayroll.Rows[index].FindControl("chkgvSelect")).Checked = false;
            }


        }

        else
        {

            empidlist += lb.Text.ToString().Trim();
            lblSelectRecd.Text += empidlist;
            string[] split = lblSelectRecd.Text.Split(',');
            foreach (string item in split)
            {
                //add employee id
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectRecd.Text = temp;
        }
        Session["dtPayroll"] = null;
        if (lblPostFirst.Text.Trim() != "")
        {
            DisplayMessage(lblPostFirst.Text);
        }
        if (lblAlreadyPosted.Text != "")
        {
            DisplayMessage(lblAlreadyPosted.Text);
        }
        if (lblWrongSequence.Text != "")
        {
            DisplayMessage(lblWrongSequence.Text);
        }
        if (lblDojIssue.Text != "")
        {
            DisplayMessage(lblDojIssue.Text);
        }
    }
    protected void ImgbtnSelectAll_ClickPayroll(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtEmpPayroll"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAllowance.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));

            }
            foreach (GridViewRow gvrow in gvEmpPayroll.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpPayroll"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpPayroll, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";

        FillGrid();
        Session["dtPayroll"] = null;
       
        txtValue1.Focus();
        //chkYearCarry.Visible = false;
    }
    protected void gvEmpPayroll_Sorting(object sender, GridViewSortEventArgs e)
    {
        hdFSortgvEmpPayroll.Value = hdFSortgvEmpPayroll.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpPayroll"];
        DataView dv = new DataView(dt);

        string Query = "" + e.SortExpression + " " + hdFSortgvEmpPayroll.Value + "";

        dv.Sort = Query;
        dt = dv.ToTable();

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpPayroll, dt, "", "");
    
        gvEmpPayroll.HeaderRow.Focus();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (Session["dtEmpPayroll"] != null)
        {
            if (ddlOption1.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOption1.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlField1.SelectedValue + ",System.String)='" + txtValue1.Text.Trim() + "'";
                }
                else if (ddlOption1.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlField1.SelectedValue + ",System.String) like '%" + txtValue1.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlField1.SelectedValue + ",System.String) Like '" + txtValue1.Text.Trim() + "%'";

                }
                DataTable dtEmp = (DataTable)Session["dtEmpPayroll"];
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                DataTable DtRows = view.ToTable();
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmpPayroll, view.ToTable(), "", "");
                Session["dtEmpPayroll"] = view.ToTable();
                lblTotalRecordsPayroll.Text = Resources.Attendance.Total_Records + " : " + DtRows.Rows.Count.ToString() + "";
            }
        }
        Session["dtPayroll"] = null;
      
        txtValue1.Focus();
        //chkYearCarry.Visible = false;
    }
    protected void btnPayroll_Click(object sender, EventArgs e)
    {

        pnlPayroll.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");


        PnlEmployeePayroll.Visible = true;
        Session["CHECKED_ITEMS"] = null;
        gvEmpPayroll.Visible = true;
        FillGrid();
        Session["dtPayroll"] = null;


        rbtnEmp.Checked = true;
        rbtnGroup.Checked = false;
        EmpGroup_CheckedChanged(null, null);
        ddlLocation.SelectedIndex = 0;
        dpDepartment.SelectedIndex = 0;
     
    }
    protected void gvEmp1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployee, (DataTable)Session["dtEmp1"], "", "");
     
    }
    protected void EmpGroup_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroup.Checked)
        {
            Div_Group.Visible = true;
            Div_Employee.Visible = false;

            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            // Modified By Nitin Jain On 25-09-2014
            if (Session["RoleId"] != null)
            {
                dtGroup = new DataView(dtGroup, "Role_Id in (" + Session["RoleId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();

            }
            //--------------------------------------

            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)lbxGroup, dtGroup, "Group_Name", "Group_Id");
            }
            lbxGroup_SelectedIndexChanged(null, null);
        }
        else if (rbtnEmp.Checked)
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;

            lblEmp.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;

            FillGrid();
        }
      
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
    public string GetCalculation(string Value_Type)
    {
        if (Value_Type == "1")
        {
            Value_Type = "Fixed";
        }
        else if (Value_Type == "2")
        {
            Value_Type = "Percentage";

        }
        return Value_Type;
    }


    protected void btnGenratePayroll1_Click(object sender, EventArgs e)
    {

        string strEmpList_logPostpending = string.Empty;
        lblPostFirst.Text = "";
        lblAlreadyPosted.Text = "";
        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";
        string EmpIds = string.Empty;
        bool LogpostedMendatory = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLogPostMendatory", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        string SalaryCalculationMethode = (objAppParam.GetApplicationParameterValueByParamName("Salary Calculate According To", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        int DaysInMonth = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Days In Month", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));

        if (SalaryCalculationMethode == "Fixed Days")
        {
            Total_Days = DaysInMonth;
        }
        else
        {
            Total_Days = DateTime.DaysInMonth(int.Parse(TxtYear.Text), int.Parse(ddlMonth.SelectedValue));
        }

        if (rbtnGroup.Checked)
        {
            string GroupIds = string.Empty;


            for (int i = 0; i < lbxGroup.Items.Count; i++)
            {
                if (lbxGroup.Items[i].Selected)
                {
                    GroupIds += lbxGroup.Items[i].Value + ",";
                }
            }

            if (GroupIds != "")
            {
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                //this code is for filter only those employee which are allow for login  user
                try
                {
                    dtEmpInGroup = new DataView(dtEmpInGroup, "Emp_Id in (" + Session["dtEmpList"].ToString().Trim() + ") ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }

                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        if (ValidPayRoll(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()) != "")
                        {

                            if (LogpostedMendatory)
                            {
                                if (objEmpAttendance.GetRecord_Emp_Attendance(dtEmpInGroup.Rows[i]["Emp_Id"].ToString(), ddlMonth.SelectedValue, TxtYear.Text,Session["CompId"].ToString()).Rows.Count == 0)
                                {
                                    strEmpList_logPostpending += GetEmployeeCode(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()) + ",";
                                }
                                else
                                {
                                    EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                                }
                            }
                            else
                            {
                                EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                            }

                           
                        }
                    }
                }

                // Modofied By Nitin jain On 13/11/2014
                DataTable Dt = new DataTable();
                Dt = ObjClaim.GetRecord_From_PayEmployeeClaimLoanPending(EmpIds);
                if (Dt.Rows.Count > 0)
                {
                    DisplayMessage("Claim or Loan Already Pending SO Approve then Generate Payroll");
                    return;
                }

                // GeneratePayroll(EmpIds);
            }
            else
            {
                DisplayMessage("Select Group First");
                lbxGroup.Focus();
            }
        }
        else
        {
            SaveCheckedValues();
            ArrayList userdetails = new ArrayList();
            if (Session["CHECKED_ITEMS"] != null)
            {

                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }

            }
            else
            {
                DisplayMessage("Select Employee First");
                return;
            }

            string strFinalPayEmp = string.Empty;
            for (int i = 0; i < userdetails.Count; i++)
            {
                if (LogpostedMendatory)
                {
                    if (objEmpAttendance.GetRecord_Emp_Attendance(userdetails[i].ToString(), ddlMonth.SelectedValue, TxtYear.Text, Session["CompId"].ToString()).Rows.Count == 0)
                    {
                        strEmpList_logPostpending += GetEmployeeCode(userdetails[i].ToString()) + ",";
                    }
                    else
                    {
                        strFinalPayEmp += userdetails[i].ToString() + ",";
                    }
                }
                else
                {
                    strFinalPayEmp += userdetails[i].ToString() + ",";
                }

            }
            // Modofied By Nitin jain On 13/11/2014
            DataTable Dt = new DataTable();
            Dt = ObjClaim.GetRecord_From_PayEmployeeClaimLoanPending(strFinalPayEmp);
            if (Dt.Rows.Count > 0)
            {
                DisplayMessage("Claim or Loan Already Pending So Approve then Generate Payroll");
                return;
            }
            //DisplayMessage(strFinalPayEmp);
            //return;
            EmpIds = strFinalPayEmp;
            /// GeneratePayroll(strFinalPayEmp);
        }


        


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {

            ObjLogProcess.GeneratePayroll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlMonth.SelectedValue, TxtYear.Text, ddlMonth.SelectedItem.Text.Trim(), Session["UserId"].ToString(), EmpIds, false, ref trns,Session["TimeZoneId"].ToString(),Session["LocCurrencyId"].ToString());

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            //GeneratePayroll(EmpIds);
            Session["TerminateEmpId"] = null;
            Session["TerminationDate"] = null;
            FillGrid();

            if (strEmpList_logPostpending != "")
            {
                DisplayMessage("Log not Posted For Employee Code=" + strEmpList_logPostpending + "");
            }
            else
            {
                DisplayMessage("Generate Payroll Successfully");
            }
            txtValue1.Text = "";
        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }

    }


    protected void btnGenratePayroll_Click(object sender, EventArgs e)
    {
        lblPostFirst.Text = "";
        lblAlreadyPosted.Text = "";
        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";
        string SalaryCalculationMethode = (objAppParam.GetApplicationParameterValueByParamName("Salary Calculate According To", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        int DaysInMonth = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Days In Month", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));

        if (SalaryCalculationMethode == "Fixed Days")
        {
            Total_Days = DaysInMonth;
        }
        else
        {
            Total_Days = DateTime.DaysInMonth(int.Parse(TxtYear.Text), int.Parse(ddlMonth.SelectedValue));
        }

        if (rbtnGroup.Checked)
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;

            for (int i = 0; i < lbxGroup.Items.Count; i++)
            {
                if (lbxGroup.Items[i].Selected)
                {
                    GroupIds += lbxGroup.Items[i].Value + ",";
                }
            }

            if (GroupIds != "")
            {
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                //this code is for filter only those employee which are allow for login  user
                try
                {
                    dtEmpInGroup = new DataView(dtEmpInGroup, "Emp_Id in (" + Session["dtEmpList"].ToString().Trim() + ") ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }

                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        if (ValidPayRoll(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()) != "")
                        {
                            EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                        }
                    }
                }

                // Modofied By Nitin jain On 13/11/2014
                DataTable Dt = new DataTable();
                Dt = ObjClaim.GetRecord_From_PayEmployeeClaimLoanPending(EmpIds);
                if (Dt.Rows.Count > 0)
                {
                    DisplayMessage("Claim or Loan Already Pending SO Approve then Generate Payroll");
                    return;
                }

                GeneratePayroll(EmpIds);
            }
            else
            {
                DisplayMessage("Select Group First");
                lbxGroup.Focus();
            }
        }
        else
        {
            SaveCheckedValues();
            ArrayList userdetails = new ArrayList();
            if (Session["CHECKED_ITEMS"] != null)
            {

                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }

            }
            else
            {
                DisplayMessage("Select Employee First");
                return;
            }

            string strFinalPayEmp = string.Empty;
            for (int i = 0; i < userdetails.Count; i++)
            {

                strFinalPayEmp += userdetails[i].ToString() + ",";

            }
            // Modofied By Nitin jain On 13/11/2014
            DataTable Dt = new DataTable();
            Dt = ObjClaim.GetRecord_From_PayEmployeeClaimLoanPending(strFinalPayEmp);
            if (Dt.Rows.Count > 0)
            {
                DisplayMessage("Claim or Loan Already Pending So Approve then Generate Payroll");
                return;
            }
            //DisplayMessage(strFinalPayEmp);
            //return;
            GeneratePayroll(strFinalPayEmp);
        }
        Session["TerminateEmpId"] = null;
        Session["TerminationDate"] = null;
        FillGrid();
        DisplayMessage("Generate Payroll Successfully");
        txtValue1.Text = "";
    }


    #region SalaryPlanDeduction
    public string[] getEpfDeduction(string strBasicSalary, string actualbasicsalary, string strSalaryPlan, string strEmpId)
    {


        string[] str = new string[7];
        double ApplicableAmount = 0;
        double Employee_PF = 0;
        double Employer_PF = 0;
        double Employer_FPF = 0;
        double Pf_Applicable_Salary = 0;
        double PF_Inspection_Charges = 0;
        double PF_EDLI = 0;
        double PF_Admin_Charges = 0;
        double DeductionAmount = 0;


        try
        {
            ApplicableAmount = double.Parse(strBasicSalary);
        }
        catch
        {

        }

        try
        {

            Employee_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_PF", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {
        }


        try
        {
            Employer_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_PF", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            Employer_FPF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_FPF", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            Pf_Applicable_Salary = double.Parse(objAppParam.GetApplicationParameterValueByParamName("PF_Applicable_Salary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            PF_Inspection_Charges = double.Parse(objAppParam.GetApplicationParameterValueByParamName("PF_Inspection_Charges", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }
        try
        {
            PF_EDLI = double.Parse(objAppParam.GetApplicationParameterValueByParamName("PF_EDLI", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }
        try
        {
            PF_Admin_Charges = double.Parse(objAppParam.GetApplicationParameterValueByParamName("PF_Admin_Charges", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }


        string strDeductionId = GetDeductionIdbyDeductionType("PF");


        ApplicableAmount = getApplicableAmountByDeductionId(strBasicSalary, strSalaryPlan, strDeductionId, strEmpId);
        DeductionAmount = getdeductionAmountByDeductionIdandEmployeeeId(actualbasicsalary, strSalaryPlan, strDeductionId, strEmpId);


        if (ApplicableAmount <= Pf_Applicable_Salary)
        {
            str[0] = ((DeductionAmount * Employee_PF) / 100).ToString();
            str[1] = ((DeductionAmount * Employer_PF) / 100).ToString();
            str[2] = ((DeductionAmount * Employer_FPF) / 100).ToString();
            str[3] = ((DeductionAmount * PF_Inspection_Charges) / 100).ToString();
            str[4] = ((DeductionAmount * PF_EDLI) / 100).ToString();
            str[5] = ((DeductionAmount * PF_Admin_Charges) / 100).ToString();
            str[6] = DeductionAmount.ToString();

        }
        else
        {
            str[0] = "0";
            str[1] = "0";
            str[2] = "0";
            str[3] = "0";
            str[4] = "0";
            str[5] = "0";
            str[6] = DeductionAmount.ToString();

        }

        return str;
    }

    public string[] getESICDeduction(string strBasicSalary, string actualbasicsalary, string strSalaryPlan, string strEmpId)
    {



        string[] str = new string[3];
        double ApplicableAmount = 0;
        double DeductionAmount = 0;
        double ESIC_Applicable_Salary = 0;
        double Employee_ESIC = 0;
        double Employer_ESIC = 0;



        try
        {
            ApplicableAmount = double.Parse(strBasicSalary);
        }
        catch
        {

        }

        try
        {

            Employee_ESIC = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_ESIC", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {
        }
        try
        {
            Employer_ESIC = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_ESIC", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            ESIC_Applicable_Salary = double.Parse(objAppParam.GetApplicationParameterValueByParamName("ESIC_Applicable_Salary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }



        string strDeductionId = GetDeductionIdbyDeductionType("ESIC");


        ApplicableAmount = getApplicableAmountByDeductionId(strBasicSalary, strSalaryPlan, strDeductionId, strEmpId);

        DeductionAmount = getdeductionAmountByDeductionIdandEmployeeeId(actualbasicsalary, strSalaryPlan, strDeductionId, strEmpId);



        if (ApplicableAmount <= ESIC_Applicable_Salary)
        {
            str[0] = ((DeductionAmount * Employee_ESIC) / 100).ToString();
            str[1] = ((DeductionAmount * Employer_ESIC) / 100).ToString();
            str[2] = DeductionAmount.ToString();
        }
        else
        {
            str[0] = "0";
            str[1] = "0";
            str[2] = DeductionAmount.ToString();
        }


        return str;
    }



    public string[] getTDSdeduction(double GrossSalary, string strBasicSalary, string actualbasicsalary, string strEpfAmount, string strSalaryPlan, string Dob, string strEmpId, double ArrearAmt, double PreviousEmployerTotalEarning, double PreviousEmployerTotalTDS, double SeniorCitizenagelimit, DateTime FinancialYearStartDate, DateTime FinancialYearEndDate, DateTime DOJ)
    {

        string[] str = new string[2];
        string strDeductionId = GetDeductionIdbyDeductionType("TDS");
        DateTime dtCurrentMonth = new DateTime(Convert.ToInt32(TxtYear.Text), Convert.ToInt32(ddlMonth.SelectedValue), 1);
        double TDSAmt = 0;
        double ApplicableAmount = 0;
        double EpfAmount = 0;
        bool IsSeniorCitizen = false;
        double EmployerTotalTDS = 0;
        double DeductionAmount = 0;

        double EmployeeAge = (DateTime.Now - Convert.ToDateTime(Dob)).TotalDays / 365;

        DeductionAmount = getdeductionAmountByDeductionIdandEmployeeeId(actualbasicsalary, strSalaryPlan, strDeductionId, strEmpId);

        DeductionAmount = (DeductionAmount - Convert.ToDouble(strEpfAmount) + ArrearAmt);

        if (EmployeeAge > SeniorCitizenagelimit)
        {
            IsSeniorCitizen = true;
        }
        //first we need to get financial year start date and end date according set in company parameter

        //here we are checking that if this is start date month 

        if (ddlMonth.SelectedValue == FinancialYearStartDate.Month.ToString() && TxtYear.Text == FinancialYearStartDate.Year.ToString())
        {

            ApplicableAmount = (GrossSalary * 12);

            TDSAmt = getTDSAmountByApplicableAmount(ApplicableAmount, strDeductionId, IsSeniorCitizen, false);
        }
        else if (ddlMonth.SelectedValue == FinancialYearEndDate.Month.ToString() && TxtYear.Text == FinancialYearEndDate.Year.ToString())
        {
            ApplicableAmount = GetPreviousMonthTotalEarning(strEmpId, strDeductionId, FinancialYearStartDate, FinancialYearEndDate);
            if (FinancialYearStartDate < DOJ && FinancialYearEndDate >= DOJ)
            {
                ApplicableAmount += PreviousEmployerTotalEarning;
                EmployerTotalTDS = PreviousEmployerTotalTDS;
            }


            TDSAmt = getTDSAmountByApplicableAmount((ApplicableAmount + DeductionAmount), strDeductionId, IsSeniorCitizen, true) - (GetTotalTDS(strEmpId, strDeductionId, FinancialYearStartDate, FinancialYearEndDate) + EmployerTotalTDS);

        }
        else
        {
            ApplicableAmount = GetPreviousMonthTotalEarning(strEmpId, strDeductionId, FinancialYearStartDate, FinancialYearEndDate);
            if (FinancialYearStartDate < DOJ && FinancialYearEndDate >= DOJ)
            {
                ApplicableAmount += PreviousEmployerTotalEarning;
            }

            TDSAmt = getTDSAmountByApplicableAmount(((GrossSalary * getMonthDifference(dtCurrentMonth, FinancialYearEndDate)) + ApplicableAmount - EpfAmount), strDeductionId, IsSeniorCitizen, false);

        }



        if (DeductionAmount < TDSAmt || TDSAmt < 0)
        {
            TDSAmt = 0;
        }
        str[0] = TDSAmt.ToString();
        str[1] = DeductionAmount.ToString();

        //th

        return str;

    }


    public double GetTotalTDS(string strEmpId, string strdeductionId, DateTime FinanceStartDate, DateTime FinanceEndDate)
    {
        DataTable dtPreviousMonth = new DataTable();
        double PreviousMonthTotalEarning = 0;

        dtPreviousMonth = da.return_DataTable("select isnull( sum( CAST(Pay_Employe_Deduction.Act_Deduction_Value as Decimal(18,3)) ),0) as TotalEarning from Pay_Employe_Deduction where CAST( CAST(Pay_Employe_Deduction.Year AS VARCHAR(4)) + RIGHT('0' + CAST(Pay_Employe_Deduction.Month AS VARCHAR(2)), 2) + RIGHT('0' + CAST(1 AS VARCHAR(2)), 2) AS DATETIME) >='" + FinanceStartDate + "' and CAST( CAST(Pay_Employe_Deduction.Year AS VARCHAR(4)) + RIGHT('0' + CAST(Pay_Employe_Deduction.Month AS VARCHAR(2)), 2) + RIGHT('0' + CAST(1 AS VARCHAR(2)), 2) AS DATETIME) <='" + FinanceEndDate + "' and Pay_Employe_Deduction.Emp_Id=" + strEmpId + " and Pay_Employe_Deduction.Deduction_Id=" + strdeductionId + "");

        if (dtPreviousMonth.Rows.Count > 0)
        {
            PreviousMonthTotalEarning = Convert.ToDouble(dtPreviousMonth.Rows[0]["TotalEarning"].ToString());

        }
        dtPreviousMonth.Dispose();
        return PreviousMonthTotalEarning;
    }

    public int getMonthDifference(DateTime dtFromdate, DateTime dtTodate)
    {
        int TotalMonth = 0;

        while (dtFromdate <= dtTodate)
        {
            TotalMonth++;
            dtFromdate = dtFromdate.AddMonths(1);
        }

        return TotalMonth;
    }


    public double GetPreviousMonthTotalEarning(string strEmpId, string strdeductionId, DateTime FinanceStartDate, DateTime FinanceEndDate)
    {
        DataTable dtPreviousMonth = new DataTable();
        double PreviousMonthTotalEarning = 0;

        dtPreviousMonth = da.return_DataTable("select isnull( sum( CAST(Pay_Employe_Deduction.Field1 as Decimal(18,3)) ),0) as TotalEarning from Pay_Employe_Deduction where CAST( CAST(Pay_Employe_Deduction.Year AS VARCHAR(4)) + RIGHT('0' + CAST(Pay_Employe_Deduction.Month AS VARCHAR(2)), 2) + RIGHT('0' + CAST(1 AS VARCHAR(2)), 2) AS DATETIME) >='" + FinanceStartDate + "' and CAST( CAST(Pay_Employe_Deduction.Year AS VARCHAR(4)) + RIGHT('0' + CAST(Pay_Employe_Deduction.Month AS VARCHAR(2)), 2) + RIGHT('0' + CAST(1 AS VARCHAR(2)), 2) AS DATETIME) <='" + FinanceEndDate + "' and Pay_Employe_Deduction.Emp_Id=" + strEmpId + " and Pay_Employe_Deduction.Deduction_Id=" + strdeductionId + "");

        if (dtPreviousMonth.Rows.Count > 0)
        {
            PreviousMonthTotalEarning = Convert.ToDouble(dtPreviousMonth.Rows[0]["TotalEarning"].ToString());

        }
        dtPreviousMonth.Dispose();
        return PreviousMonthTotalEarning;
    }


    public string[] getProfessionalTaxdeduction(string strBasicSalary, string actualbasicsalary, string strSalaryPlan, string Dob, string strEmpId)
    {

        string[] str = new string[2];


        double ApplicableAmount = 0;

        double SeniorCitizenagelimit = 0;
        double DeductionAmount = 0;


        try
        {
            SeniorCitizenagelimit = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Senior_Citizen_Age_Limit", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }

        string strDeductionId = GetDeductionIdbyDeductionType("PT");


        ApplicableAmount = getApplicableAmountByDeductionId(strBasicSalary, strSalaryPlan, strDeductionId, strEmpId);


        DeductionAmount = getdeductionAmountByDeductionIdandEmployeeeId(actualbasicsalary, strSalaryPlan, strDeductionId, strEmpId);

        ApplicableAmount = (ApplicableAmount) * 12;


        //here we are checking that selected employee is senior citizen or general 


        double EmployeeAge = (DateTime.Now - Convert.ToDateTime(Dob)).TotalDays / 365;



        if (EmployeeAge > SeniorCitizenagelimit)
        {
            str[0] = getDeductionAmountByDeductionId(ApplicableAmount.ToString(), strDeductionId, true, DeductionAmount.ToString());
        }
        else
        {
            str[0] = getDeductionAmountByDeductionId(ApplicableAmount.ToString(), strDeductionId, false, DeductionAmount.ToString());
        }

        str[1] = ApplicableAmount.ToString();

        //th

        return str;

    }




    public string getDeductionAmountByDeductionId(string strAmount, string strDeductionId, bool IsSeniorCitizen, string strDeductionAmount)
    {
        string strdeductionAmount = "0";

        DataTable dt = new DataTable();
        //select calculation_type,value from set_deductiondetail where From_Amount <= 0 and To_Amount >= 0 and header_id=0 and Is_Senior_Citizen='True'

        if (IsSeniorCitizen)
        {
            dt = da.return_DataTable("select calculation_type,value from set_deductiondetail where From_Amount <= " + strAmount + " and To_Amount >=" + strAmount + " and header_id=" + strDeductionId + " and Is_Senior_Citizen='True'");
        }
        else
        {
            dt = da.return_DataTable("select calculation_type,value from set_deductiondetail where From_Amount <= " + strAmount + " and To_Amount >=" + strAmount + " and header_id=" + strDeductionId + " and Is_Senior_Citizen='False'");
        }


        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["calculation_type"].ToString() == "Fixed")
            {

                strdeductionAmount = dt.Rows[0]["value"].ToString();
            }
            else
            {
                strdeductionAmount = ((Convert.ToDouble(strDeductionAmount) * Convert.ToDouble(dt.Rows[0]["value"].ToString())) / 100).ToString();
            }
        }



        return strdeductionAmount;



    }

    public double getTDSAmountByApplicableAmount(double ApplicableAmount, string strDeductionId, bool IsSeniorCitizen, bool IsYearEndMonth)
    {

        double TDSAmt = 0;

        double TdsApplicableslabamt = 0;
        DataTable dt = new DataTable();
        //select calculation_type,value from set_deductiondetail where From_Amount <= 0 and To_Amount >= 0 and header_id=0 and Is_Senior_Citizen='True'


        dt = da.return_DataTable("select calculation_type,value,To_Amount,From_Amount from set_deductiondetail where  header_id=" + strDeductionId + " and Is_Senior_Citizen='" + IsSeniorCitizen.ToString() + "'");

        foreach (DataRow dr in dt.Rows)
        {
            TdsApplicableslabamt = 0;
            if (ApplicableAmount > Convert.ToDouble(dr["To_Amount"].ToString()))
            {
                TdsApplicableslabamt = Convert.ToDouble(dr["To_Amount"].ToString()) - Convert.ToDouble(dr["From_Amount"].ToString());
                TDSAmt += (TdsApplicableslabamt * Convert.ToDouble(dr["value"].ToString())) / 100;

            }
            else
            {
                TdsApplicableslabamt = ApplicableAmount - Convert.ToDouble(dr["From_Amount"].ToString());
                TDSAmt += (TdsApplicableslabamt * Convert.ToDouble(dr["value"].ToString())) / 100;
                break;
            }
        }
        if (IsYearEndMonth)
        {
            TDSAmt = Math.Round(TDSAmt, 0);

        }
        else
        {
            TDSAmt = Math.Round(TDSAmt / 12, 0);
        }

        return TDSAmt;
    }



    public string GetDeductionIdbyDeductionType(string strType)
    {
        string strDeductionId = "0";

        DataTable dt = ObjDeduc.GetDeductionTrueAll(Session["CompId"].ToString());

        dt = new DataView(dt, "Field1='" + strType + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            strDeductionId = dt.Rows[0]["Deduction_Id"].ToString();

        }
        return strDeductionId;
    }


    public double getApplicableAmountByDeductionId(string strBasicSalary, string strSalaryPlanId, string strDeductionId, string strEmpId)
    {
        double strApplicableAmount = 0;
        double BasicSalary = 0;
        DataTable dtTemp = new DataTable();
        double GrossAmount = 0;
        try
        {
            BasicSalary = Convert.ToDouble(strBasicSalary);
        }
        catch
        {

        }

        DataTable dtparam = objempparam.GetEmployeeParameterByEmpId(strEmpId, Session["CompId"].ToString());


        if (dtparam.Rows.Count > 0)
        {

            GrossAmount = Convert.ToDouble(dtparam.Rows[0]["Gross_Salary"].ToString());


        }

        strApplicableAmount += BasicSalary;

        DataTable dt = objsalaryPlan.GetApplicableAllowance_By_headerId_and_deductionId(strSalaryPlanId, strDeductionId);


        dtTemp = new DataView(dt, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
        foreach (DataRow dr in dtTemp.Rows)
        {


            if (dr["Calculation_Method"].ToString() == "Fixed")
            {
                strApplicableAmount += Convert.ToDouble(dr["Value"].ToString());
            }
            else
            {
                try
                {
                    strApplicableAmount += (BasicSalary * Convert.ToDouble(dr["Value"].ToString())) / 100;
                }
                catch
                {

                }

            }

        }

        dtTemp = new DataView(dt, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtTemp.Rows.Count > 0)
        {

            if ((GrossAmount - strApplicableAmount) > 0)
            {

                strApplicableAmount += (GrossAmount - strApplicableAmount);

            }

        }

        dtTemp.Dispose();
        dt.Dispose();
        return strApplicableAmount;

    }


    public double getdeductionAmountByDeductionIdandEmployeeeId(string strBasicSalary, string strSalaryPlanId, string strDeductionId, string strEmpId)
    {
        double strApplicableAmount = 0;
        double BasicSalary = 0;

        try
        {
            BasicSalary = Convert.ToDouble(strBasicSalary);
        }
        catch
        {

        }


        strApplicableAmount += BasicSalary;

        DataTable dt = objsalaryPlan.GetApplicableAllowance_By_headerId_and_deductionId(strSalaryPlanId, strDeductionId);

        foreach (DataRow dr in dt.Rows)
        {


            DataTable dtAlllowaance = da.return_DataTable("select Allowance_Value from Pay_Employe_Allowance_Temp where Emp_Id=" + strEmpId + " and Allowance_Id=" + dr["ref_Id"].ToString() + "");



            if (dtAlllowaance.Rows.Count > 0)
            {
                try
                {
                    strApplicableAmount += Convert.ToDouble(dtAlllowaance.Rows[0]["Allowance_Value"].ToString());
                }
                catch
                {

                }
            }


        }





        return strApplicableAmount;

    }

    public string GetAmountDecimal(string strAmount)
    {
        return objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), strAmount);
    }


    public void insertAllowance_using_SalaryPlan(string strSalaryPlanId, string strEmployeeId, string strGrossAmount, double BasicSalary)
    {
        double GrossAmt = 0;
        double RowAmount = 0;
        try
        {
            GrossAmt = Convert.ToDouble(strGrossAmount);
        }
        catch
        {

        }
        DataTable dtsalaryPlanAll = objsalaryplandetail.GetDeduction_By_headerId(strSalaryPlanId);


        DataTable dtsalaryPlan = new DataView(dtsalaryPlanAll, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
        //  dttDetail = dttDetail.DefaultView.ToTable(true, "Trans_Id", "Ref_Id", "Calculation_Method", "Value", "Amount", "Deduction_Applicable");

        foreach (DataRow dr in dtsalaryPlan.Rows)
        {

            string strCalcmethod = string.Empty;
            if (dr["Calculation_Method"].ToString() == "Fixed")
            {
                strCalcmethod = "1";


                RowAmount += Convert.ToDouble(dr["Value"].ToString());
            }
            else
            {
                strCalcmethod = "2";

                RowAmount += (BasicSalary * Convert.ToDouble(dr["Value"].ToString())) / 100;
            }


            ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), strEmployeeId, "1", dr["ref_id"].ToString(), "Monthly", strCalcmethod, dr["Value"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


        }

        if ((GrossAmt - (BasicSalary + RowAmount)) > 0)
        {

            dtsalaryPlan = new DataView(dtsalaryPlanAll, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable();


            if (dtsalaryPlan.Rows.Count > 0)
            {
                ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), strEmployeeId, "1", dtsalaryPlan.Rows[0]["Ref_Id"].ToString(), "Monthly", "1", (GrossAmt - (BasicSalary + RowAmount)).ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
        }
    }


    #endregion


    void GeneratePayroll(string EmpLIst)
    {

        EmployeeMaster objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        bool IndemnityStatus = false;
        bool IsIndemnity = false;
        int IndemnityDays = 0;
        int IndemnityGivenType = 0;
        // int IndemnitySalaryType = 0;
        //   int IndemnitySalaryValue = 0;
        // int IndemnityLeave = 0;
        double Employee_PF = 0;
        double Employer_PF = 0;
        double Employee_ESIC = 0;
        double Employer_ESIC = 0;
        double MobileLimit = 0;
        string strMobileNumber = string.Empty;
        double PreviousEmployerTotalEarning = 0;
        double PreviousEmployerTotalTDS = 0;
        double SeniorCitizenagelimit = 0;
        DateTime Doj = new DateTime();
        // Indemnity Code Here.....

        // IsIndemnity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString()));
        //  IndemnityDays = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityDayas", Session["CompId"].ToString()));

        //if (IsIndemnity == true)
        //{

        //IndemnityGivenType = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Indemnity_GivenType", Session["CompId"].ToString()));
        //if (IndemnityGivenType == 1)
        //{
        //    IndemnitySalaryType = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Indemenity_SalaryCalculationType", Session["CompId"].ToString()));
        //    IndemnitySalaryValue = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnitySalaryValue", Session["CompId"].ToString()));
        //}
        //else
        //{
        //    IndemnityLeave = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityLeave", Session["CompId"].ToString()));
        //}
        // }


        //...........................................
        try
        {

            Employee_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_PF", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {
        }
        try
        {
            Employer_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_PF", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }
        try
        {
            Employee_ESIC = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_ESIC", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }
        try
        {
            Employer_ESIC = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_ESIC", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            SeniorCitizenagelimit = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Senior_Citizen_Age_Limit", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        }
        catch
        {

        }



        DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        int FinancialYearMonth = 0;

        if (dt.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
        }

        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        if (DateTime.Now.Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }


        double sumpenalty = 0;
        double sum = 0;
        double sumallow = 0;
        double sumdeduc = 0;
        double sumloan = 0;
        int b = 0;
        int a = 0;
        int mainflag = 0;

        int monthclaim = 0;
        int yearclaim = 0;
        double totaldueamt;
        double dueamt = 0;
        double dueamtt = 0;
        bool IsLeaveSalary = false;
        double basicsal = 0;
        double Actualbasicsal = 0;
        double Grossamt = 0;
        bool IsCtc = false;

        double ArrearAmt = 0;
        DataTable dtEmpPay = GetTable();

        string TransNo = string.Empty;

        DataAccessClass objDA = new DataAccessClass(Session["DBConnection"].ToString());

        Total_Days = DateTime.DaysInMonth(Convert.ToInt32(TxtYear.Text), Convert.ToInt32(ddlMonth.SelectedValue));
        foreach (string str in EmpLIst.Split(','))
        {
            if ((str != ""))
            {


                try
                {

                    strMobileNumber = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), str).Rows[0]["Company_phone_no"].ToString();
                }
                catch
                {

                }

                bool IsPF = false;
                bool IsEsIc = false;
                double workeddaysPercentage = 0;
                double Pf_Applicable_Amount = 0;
                double Employee_Pf_Amount = 0;
                double Employer_Pf_Amount = 0;
                double Employer_FPF = 0;
                double Employer_PF_Admin_Charges = 0;
                double Employer_PF_EDLI = 0;
                double Employer_PF_Inspection_Charges = 0;


                double Esic_Applicable_Amount = 0;
                double Employee_Esic_Amount = 0;
                double Employer_Esic_Amount = 0;

                double TdsAmount = 0;
                double Tds_applicable_Amount = 0;

                double PT = 0;
                double PT_applicable_Amount = 0;

                string strSalaryPlanId = "0";

                string strDob = string.Empty;


                strDob = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), str).Rows[0]["DOB"].ToString();


                double PerDaySal = 0;
                DataTable dtemppara = new DataTable();
                double TotalLDays = 0;
                double UsedLeaveDays = 0;
                int TotalWorkedDays = 0;
                double PerDayLeave = 0;
                double WorkDaysSal = 0;
                double ValidLeaveDays = 0;
                dtemppara = objempparam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
                try
                {
                    basicsal = Convert.ToDouble(dtemppara.Rows[0]["Basic_Salary"].ToString());
                }
                catch
                {


                }

                try
                {

                    Grossamt = Convert.ToDouble(dtemppara.Rows[0]["Gross_Salary"].ToString());
                }
                catch
                {

                }


                PerDaySal = basicsal / Total_Days;
                DataTable DTDOJ = objAttendance.GetEmployeeDOJ(Session["CompId"].ToString(), str);

                if (DTDOJ.Rows.Count > 0)
                {
                    Doj = Convert.ToDateTime(DTDOJ.Rows[0]["Doj"].ToString());
                }
                if (dtemppara.Rows.Count > 0)
                {
                    strSalaryPlanId = dtemppara.Rows[0]["Salary_Plan_Id"].ToString();

                    if (strSalaryPlanId.Trim() == "")
                    {
                        strSalaryPlanId = "0";
                    }

                    IsCtc = Convert.ToBoolean(dtemppara.Rows[0]["IsCtc_Employee"].ToString());
                    try
                    {
                        MobileLimit = Convert.ToDouble(dtemppara.Rows[0]["Mobilebill_Limit"].ToString());
                    }
                    catch
                    {

                    }

                    try
                    {
                        PreviousEmployerTotalEarning = Convert.ToDouble(dtemppara.Rows[0]["Previous_Employer_Earning"].ToString());
                    }
                    catch
                    {


                    }

                    try
                    {
                        PreviousEmployerTotalTDS = Convert.ToDouble(dtemppara.Rows[0]["Previous_Employer_TDS"].ToString());
                    }
                    catch
                    {


                    }
                    // Nitin Jain On 24/01/2015................
                    // Leave Adjustment Salary..................
                    try
                    {
                        if (Session["TerminateEmpId"].ToString() == null)
                        {
                            Session["TerminateEmpId"] = "";
                        }


                        if (Session["TerminateEmpId"].ToString() != "")
                        {
                            IsLeaveSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
                            if (IsLeaveSalary == true)
                            {
                                DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(Session["TerminateEmpId"].ToString());
                                if (dtLeaveSummary.Rows.Count > 0)
                                {
                                    // Previous Year Leave Salary..
                                    string months = string.Empty;

                                    DateTime FromDate = DateTime.Now;
                                    DateTime ToDate = DateTime.Now.AddMonths(2);
                                    while (FromDate <= ToDate)
                                    {
                                        months += FromDate.Month.ToString() + ",";
                                        FromDate = FromDate.AddMonths(1);
                                    }


                                    string year = string.Empty;
                                    string year1 = string.Empty;

                                    if (FinancialYearStartDate.Year == FinancialYearEndDate.Year)
                                    {
                                        year = FinancialYearStartDate.Year.ToString();
                                        year1 = year;

                                    }
                                    else
                                    {


                                        year += FinancialYearStartDate.Year.ToString() + ",";
                                        year1 = year;
                                        year += FinancialYearEndDate.Year.ToString() + ",";
                                    }



                                    DataTable dtleave = dtLeaveSummary;
                                    dtleave = new DataView(dtleave, "month='0' and year in(" + year1 + ")", "", DataViewRowState.CurrentRows).ToTable();

                                    dtLeaveSummary = new DataView(dtLeaveSummary, "month in(" + months + ") and year in (" + year + ") ", "", DataViewRowState.CurrentRows).ToTable();

                                    dtLeaveSummary.Merge(dtleave);
                                    if (dtLeaveSummary.Rows.Count > 0)
                                    {
                                        objAttendance.DeleteClaim(str);
                                        try
                                        {
                                            dtLeaveSummary = new DataView(dtLeaveSummary, "Month=" + DateTime.Now.Month.ToString() + " or Shedule_Type='Yearly' ", "", DataViewRowState.CurrentRows).ToTable();
                                            DataTable AssignedLeave = new DataTable();
                                            AssignedLeave = new DataView(dtLeaveSummary, "IsAuto='True'", "", DataViewRowState.CurrentRows).ToTable();
                                            if (AssignedLeave.Rows.Count > 0)
                                            {
                                                for (int AL = 0; AL <= AssignedLeave.Rows.Count - 1; AL++)
                                                {
                                                    TotalLDays = Convert.ToDouble(AssignedLeave.Rows[AL]["Total_Days"].ToString());
                                                    basicsal = PerDaySal * TotalLDays;
                                                    b = ObjClaim.Insert_In_Pay_Employee_ClaimRequest(Session["CompId"].ToString(), str, "Past Leave Settlement", "Previous Pending Leave Salary", "1", basicsal.ToString(), DateTime.Now.ToString(), "Approved", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                            AssignedLeave = new DataView(dtLeaveSummary, "IsAuto='False'", "", DataViewRowState.CurrentRows).ToTable();
                                            if (AssignedLeave.Rows.Count > 0)
                                            {
                                                if (AssignedLeave.Rows.Count == 1)
                                                {
                                                    if (FinancialYearStartDate > Convert.ToDateTime(DTDOJ.Rows[0]["Doj"].ToString()))
                                                    {
                                                        TotalLDays = Convert.ToDouble(AssignedLeave.Rows[0]["Total_Days"].ToString());
                                                        UsedLeaveDays = Convert.ToDouble(AssignedLeave.Rows[0]["Used_Days"].ToString());
                                                        TotalWorkedDays = Convert.ToInt32((Convert.ToDateTime(Session["TerminationDate"].ToString()) - FinancialYearStartDate).TotalDays);
                                                        PerDayLeave = TotalLDays / 365;
                                                        ValidLeaveDays = PerDayLeave * TotalWorkedDays;
                                                        ValidLeaveDays = Math.Round(ValidLeaveDays);
                                                        if (ValidLeaveDays > UsedLeaveDays)
                                                        {
                                                            ValidLeaveDays = ValidLeaveDays - UsedLeaveDays;
                                                            basicsal = ValidLeaveDays * PerDaySal;
                                                            b = ObjClaim.Insert_In_Pay_Employee_ClaimRequest(Session["CompId"].ToString(), str, "Leave Settlement", "Assigned Remaning Leave Salary", "1", basicsal.ToString(), DateTime.Now.ToString(), "Approved", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                        }
                                                        else
                                                        {
                                                            ValidLeaveDays = UsedLeaveDays - ValidLeaveDays;
                                                            basicsal = ValidLeaveDays * PerDaySal;
                                                            b = objPEpenalty.Insert_In_Pay_Employee_Penalty(Session["CompId"].ToString(), str, "Balanced Leave Penalty", "Penalty Leave Salary", "1", basicsal.ToString(), DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                        }

                                                    }
                                                }
                                                else if (AssignedLeave.Rows.Count > 1)
                                                {
                                                    if (FinancialYearStartDate > Convert.ToDateTime(DTDOJ.Rows[0]["Doj"].ToString()))
                                                    {
                                                        for (int AL = 0; AL <= AssignedLeave.Rows.Count - 1; AL++)
                                                        {
                                                            TotalLDays = Convert.ToDouble(AssignedLeave.Rows[AL]["Total_Days"].ToString());
                                                            UsedLeaveDays = Convert.ToDouble(AssignedLeave.Rows[AL]["Used_Days"].ToString());
                                                            TotalWorkedDays = Convert.ToInt32((Convert.ToDateTime(Session["TerminationDate"].ToString()) - FinancialYearStartDate).TotalDays);
                                                            PerDayLeave = TotalLDays / 365;
                                                            ValidLeaveDays = PerDayLeave * TotalWorkedDays;
                                                            ValidLeaveDays = Math.Round(ValidLeaveDays);
                                                            if (ValidLeaveDays > UsedLeaveDays)
                                                            {
                                                                ValidLeaveDays = ValidLeaveDays - UsedLeaveDays;
                                                                basicsal = ValidLeaveDays * PerDaySal;
                                                                b = ObjClaim.Insert_In_Pay_Employee_ClaimRequest(Session["CompId"].ToString(), str, "Leave Settlement", "Assigned Remaning Leave Salary", "1", basicsal.ToString(), DateTime.Now.ToString(), "Approved", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                            }
                                                            else
                                                            {
                                                                ValidLeaveDays = UsedLeaveDays - ValidLeaveDays;
                                                                basicsal = ValidLeaveDays * PerDaySal;
                                                                b = objPEpenalty.Insert_In_Pay_Employee_Penalty(Session["CompId"].ToString(), str, "Balanced Leave Penalty", "Penalty Leave Salary", "1", basicsal.ToString(), DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            // Half Day Code Will Start Here Nitin Jain
                                            string Year = string.Empty;
                                            DataTable dtFinencialYera = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                                            FinancialYearMonth = 0;

                                            if (dtFinencialYera.Rows.Count > 0)
                                            {
                                                FinancialYearMonth = int.Parse(dtFinencialYera.Rows[0]["Param_Value"].ToString());

                                            }

                                            if (DateTime.Now.Month >= FinancialYearMonth)
                                            {
                                                Year = DateTime.Now.Year.ToString();
                                            }
                                            else
                                            {
                                                Year = (DateTime.Now.Year - 1).ToString();
                                            }
                                            DataTable dtEmpHalf = objEmpHalfDay.GetEmployeeHalfDayTransactionData(str, Year);
                                            if (dtEmpHalf.Rows.Count > 0)
                                            {
                                                ValidLeaveDays = Convert.ToInt32(dtEmpHalf.Rows[0]["Used_Days"]) - Convert.ToInt32(dtEmpHalf.Rows[0]["Remaning_Days"]);
                                                basicsal = ValidLeaveDays * PerDaySal;
                                                basicsal = basicsal / 2;
                                                b = ObjClaim.Insert_In_Pay_Employee_ClaimRequest(Session["CompId"].ToString(), str, "Leave Settlement", "Assigned Remaning Leave Salary", "1", basicsal.ToString(), DateTime.Now.ToString(), "Approved", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                            // ------------------------------------------------------------------------
                                        }

                                        catch
                                        {
                                        }

                                    }
                                }
                            }
                            else
                            {
                                string TotalLeaveSal = objLeaveSal.GetLeaveSalaryByEmpId(str);
                                string TotalClaimSal = objLeaveSal.GetClaimSalByEmpId(str);
                                if (Convert.ToDecimal(TotalClaimSal) < Convert.ToDecimal(TotalLeaveSal))
                                {
                                    TotalClaimSal = Convert.ToString(Convert.ToDecimal(TotalLeaveSal) - Convert.ToDecimal(TotalClaimSal));
                                    int c = ObjClaim.Insert_In_Pay_Employee_ClaimRequest(Session["CompId"].ToString(), str, "Leave Salary", "Assigned Leave Salary", "1", TotalClaimSal, DateTime.Now.ToString(), "Approved", DateTime.Now.ToString(), DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                            }
                            // Current Year Leave Salary...
                        }
                    }
                    catch (Exception Ex)
                    {
                    }

                    //..........................................

                    // Indemnity.................................................................
                    if (IndemnityStatus == false)
                    {
                        if (IsIndemnity == true)
                        {
                            DataTable DtIndemnityEmp = objIndemnity.GetIndemnityEmployeeForPayroll(EmpLIst, ddlMonth.SelectedValue, TxtYear.Text);
                            if (DtIndemnityEmp.Rows.Count > 0)
                            {
                                for (int k = 0; k < DtIndemnityEmp.Rows.Count; k++)
                                {
                                    dtemppara = objempparam.GetEmployeeParameterByEmpId(DtIndemnityEmp.Rows[k]["Employee_Id"].ToString(), Session["CompId"].ToString());
                                    basicsal = Convert.ToDouble(dtemppara.Rows[0]["Basic_Salary"].ToString());
                                    double MonthSal = objAttendance.IndemnitySalary(Session["CompId"].ToString(), DtIndemnityEmp.Rows[k]["Employee_Id"].ToString(), basicsal.ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                                    //// Insert Entry in Claim Table 
                                    //if (IndemnityGivenType == 1)
                                    //{
                                    //    b = ObjClaim.Insert_In_Pay_Employee_ClaimRequest(Session["CompId"].ToString(), DtIndemnityEmp.Rows[k]["Employee_Id"].ToString(), "Indemnity Claim", "This Is Indemnity Claim", "1", MonthSal.ToString(), DateTime.Now.ToString(), "Approved", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    //    // Next Indemnity Date Insert in Indemnity Table
                                    //    int Indemnity = objIndemnity.InsertIndemnityRecord(DtIndemnityEmp.Rows[k]["Indemnity_Id"].ToString(), Session["CompId"].ToString(), DtIndemnityEmp.Rows[k]["Employee_Id"].ToString(), DtIndemnityEmp.Rows[k]["Indemnity_Date"].ToString(), "Approved", "1", "Used", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    //    Indemnity = objIndemnity.InsertIndemnityRecord("0", Session["CompId"].ToString(), DtIndemnityEmp.Rows[k]["Employee_Id"].ToString(), Convert.ToDateTime(DtIndemnityEmp.Rows[k]["Indemnity_Date"].ToString()).AddYears(NextIndemnityDuration).ToString(), "Pending", "2", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    //}
                                    //else
                                    //{
                                    //    // Leave
                                    //   // b = ObjClaim.Insert_In_Pay_Employee_ClaimRequest(Session["CompId"].ToString(), DtIndemnityEmp.Rows[k]["Employee_Id"].ToString(), "Indemnity Claim", "This Is Indemnity Claim", "1", MonthSal.ToString(), DateTime.Now.ToString(), "Approved", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    //    // Next Indemnity Date Insert in Indemnity Table
                                    //    int Indemnity = objIndemnity.InsertIndemnityRecord(DtIndemnityEmp.Rows[k]["Indemnity_Id"].ToString(), Session["CompId"].ToString(), DtIndemnityEmp.Rows[k]["Employee_Id"].ToString(), DtIndemnityEmp.Rows[k]["Indemnity_Date"].ToString(), "Approved", "1", "Not Used", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    //    Indemnity = objIndemnity.InsertIndemnityRecord("0", Session["CompId"].ToString(), DtIndemnityEmp.Rows[k]["Employee_Id"].ToString(), Convert.ToDateTime(DtIndemnityEmp.Rows[k]["Indemnity_Date"].ToString()).AddYears(NextIndemnityDuration).ToString(), "Pending", "2", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    //}

                                }

                            }
                            IndemnityStatus = true;
                        }
                    }
                    // End Indemnity..........................................................
                    basicsal = Convert.ToDouble(dtemppara.Rows[0]["Basic_Salary"].ToString());
                    try
                    {
                        IsPF = Convert.ToBoolean(dtemppara.Rows[0]["Field4"].ToString());
                    }
                    catch
                    {

                    }
                    try
                    {
                        IsEsIc = Convert.ToBoolean(dtemppara.Rows[0]["Field5"].ToString());
                    }
                    catch
                    {

                    }

                }



                //here we are inserting row in employee allowance and deduction part according selected salry plan for selected employee


                if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsSalaryPlanEnable", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString())))
                {
                    //fisrt we will delete old record then insert new
                    objDA.execute_Command("delete from Pay_Employee_Allow_Deduction where Emp_Id=" + str + " and type=1");
                    insertAllowance_using_SalaryPlan(strSalaryPlanId, str, Grossamt.ToString(), basicsal);

                }
                //here we insert the row in pay_employee_attendence table with current month and current year
                //this code is update on 05-04-2014
                DataTable DtEmpAttendence = objPayEmpAtt.GetRecord_Emp_Attendance(str, ddlMonth.SelectedValue, TxtYear.Text, Session["CompId"].ToString());
                if (DtEmpAttendence.Rows.Count == 0)
                {

                    DataTable dtobjempdetail = objEmpDetail.GetAllTrueRecord();
                    try
                    {
                        dtobjempdetail = new DataView(dtobjempdetail, "Emp_Id=" + str + " and Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtobjempdetail.Rows.Count > 0)
                    {
                        objPayEmpAtt.InsertPayEmployeeAttendance(Session["CompId"].ToString(), str, ddlMonth.SelectedValue, TxtYear.Text, Math.Round(float.Parse(dtobjempdetail.Rows[0]["Total_Days"].ToString()), 0).ToString(), "0", Math.Round(float.Parse(dtobjempdetail.Rows[0]["Present_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["WeekOff_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Holiday_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Leave_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Absent_Days"].ToString()), 0).ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }
                    else
                    {
                        objPayEmpAtt.InsertPayEmployeeAttendance(Session["CompId"].ToString(), str, ddlMonth.SelectedValue, TxtYear.Text, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }
                }
                else
                {

                }

                //this code for delete the record from due payment table using empid month and year
                objEmpDuePay.DeleteEmp_Due_paymentByEmpId_MonthandYear(Session["CompId"].ToString(), str, ddlMonth.SelectedValue, TxtYear.Text, Session["CompId"].ToString(), "", "2");



                DataTable dtEmpMonth = new DataTable();

                // Pay_Employe_Month_Temp: Get Data Of Employee
                dtEmpMonth = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(str,Session["CompId"].ToString());
                dtEmpMonth = new DataView(dtEmpMonth, "Month='" + ddlMonth.SelectedValue + "' and Year='" + TxtYear.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                int m = 0;
                if (dtEmpMonth.Rows.Count > 0)
                {
                    // Here Delete From Master And Child Table Record 
                    // If Same Month And Year Record
                    m = objPayEmpMonth.DeleteEmpMonthTemp_By_EmpId_MonthandYear(Session["CompId"].ToString(), str, ddlMonth.SelectedValue, TxtYear.Text);
                    a = objpayrolldeduc.DeletePayDeductionTemp_By_EmpId_MonthandYear(str, ddlMonth.SelectedValue, TxtYear.Text);
                    b = objpayrollall.DeletePayAllowanceTemp_By_EmpId_MonthandYear(str, ddlMonth.SelectedValue, TxtYear.Text);

                }


                int WorkDays = 0;
                sumpenalty = 0;
                sum = 0;
                sumallow = 0;
                sumdeduc = 0;
                double work_wedges_Day = 0;


                int ma = objPayEmpMonth.Insert_Pay_Employee_Month_Temp(Session["CompId"].ToString(), str, ddlMonth.SelectedValue.ToString(), TxtYear.Text, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
                // Get Trans Id of thie Employee acc to EmpId,Month,Year = Ref id of allowance and Deduction Temp
                // Get Employee Allowance


                Pay_Employee_Attendance objPay = new Pay_Employee_Attendance(Session["DBConnection"].ToString());

                DataTable dtPay = new DataTable();
                try
                {
                    dtPay = objPay.GetRecord_Emp_Attendance(str, ddlMonth.SelectedValue, TxtYear.Text, Session["CompId"].ToString());

                }
                catch
                {

                }

                if (dtPay.Rows.Count > 0)
                {
                    WorkDays = int.Parse(dtPay.Rows[0]["Worked_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Week_Off_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Holiday_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Leave_Days"].ToString());


                    Actualbasicsal = PerDaySal * WorkDays;

                    Actualbasicsal = Convert.ToDouble(dtPay.Rows[0]["Basic_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Normal_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["WeekOff_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Leave_Days_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_Days_Salary"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Absent_Day_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Late_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Early_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Parital_Violation_Penalty"].ToString());

                    workeddaysPercentage = (Actualbasicsal * 100) / basicsal;
                }



                //if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsSalaryPlanEnable", Session["CompId"].ToString())))
                //{

                //    //fisrt we will delete old record then insert new
                //    objDA.execute_Command("delete from Pay_Employee_Allow_Deduction where Emp_Id=" + str + " and type=1");


                //    if (strSalaryPlanId != "0")
                //    {
                //        insertAllowance_using_SalaryPlan(strSalaryPlanId, str, Grossamt, basicsal, Actualbasicsal, Total_Days, WorkDays);
                //    }


                //}

                if (Total_Days == 0)
                {
                    Total_Days = 1;
                }


                //this code fro insert the record in allowance_temp table from pay_allow_deduction table
                //where we filter the record type=1 for allowance

                DataTable dtEmpAllowDeduc = GetEmpAllowDedu(Convert.ToInt32(str));
                sumallow = 0;
                dtEmpAllowDeduc = new DataView(dtEmpAllowDeduc, "Type = 1", "", DataViewRowState.CurrentRows).ToTable();


                for (int i = 0; i < dtEmpAllowDeduc.Rows.Count; i++)
                {

                    Actualbasicsal = Common.getAllowanceanddeductionCalculation(dtEmpAllowDeduc.Rows[i]["Calculation_Type"].ToString(), dtEmpAllowDeduc.Rows[i]["Calculation_Value"].ToString(), dtPay, PerDaySal, str, ddlMonth.SelectedValue, TxtYear.Text, Session["DBConnection"].ToString())[1];
                    workeddaysPercentage = (Actualbasicsal * 100) / basicsal;

                    string CalMethod = string.Empty;
                    DataRow row = dtEmpPay.NewRow();
                    row[0] = str;


                    row[1] = "";
                    row[2] = ddlMonth.SelectedItem.Text;
                    row[3] = TxtYear.Text;
                    row[4] = dtEmpAllowDeduc.Rows[i]["Type"].ToString();
                    row[5] = dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString();
                    row[6] = dtEmpAllowDeduc.Rows[i]["Value_Type"].ToString();

                    CalMethod = dtEmpAllowDeduc.Rows[i]["Calculation_Method"].ToString();
                    double AllowancesValue = 0;
                    if (row[6].ToString() == "1")
                    {
                        GetCalculation(dtEmpAllowDeduc.Rows[i]["Value_Type"].ToString());
                        row[7] = dtEmpAllowDeduc.Rows[i]["Value"].ToString();
                        double al = Convert.ToDouble(dtEmpAllowDeduc.Rows[i]["Value"]);

                        al = (Convert.ToDouble(dtEmpAllowDeduc.Rows[i]["Value"]) * workeddaysPercentage) / 100;

                        if (CalMethod == "Daily")
                        {
                            al = al * WorkDays;
                        }


                        AllowancesValue = al;



                        row[7] = al.ToString();

                    }
                    else
                    {
                        double val = Convert.ToDouble((Actualbasicsal * Convert.ToDouble(dtEmpAllowDeduc.Rows[i]["Value"].ToString())) / 100);
                        row[7] = val.ToString();
                        if (CalMethod == "Daily")
                        {
                            val = val * WorkDays;
                        }

                        AllowancesValue = val;
                        row[7] = val.ToString();
                    }
                    //this code is created by jitendra upadhyay on 07-05-2014
                    //here we add the last month remaining allowances
                    //code start

                    int Lastmonth = 0;
                    int LastYear = 0;
                    string AllowancesId = string.Empty;
                    AllowancesId = "Allowance Id=" + dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString() + "";
                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(str,Session["CompId"].ToString());
                    if (ddlMonth.SelectedValue.ToString() == "1")
                    {
                        LastYear = Convert.ToInt32(TxtYear.Text) - 1;
                        Lastmonth = 12;
                    }
                    else
                    {
                        Lastmonth = Convert.ToInt32(ddlMonth.SelectedValue.ToString()) - 1;
                        LastYear = Convert.ToInt32(TxtYear.Text);
                    }

                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Allowance' and Field2='" + AllowancesId + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                    if (dtDueamount.Rows.Count > 0)
                    {
                        if (dtDueamount.Rows[0]["Amount"].ToString().Trim() != "")
                        {
                            try
                            {
                                AllowancesValue = AllowancesValue + Convert.ToDouble(dtDueamount.Rows[0]["Amount"].ToString());
                            }
                            catch
                            {
                                if (row[7].ToString() != "")

                                    AllowancesValue = Convert.ToDouble(row[7].ToString());
                                else

                                    AllowancesValue = 0;
                            }
                        }

                    }


                    //code end

                    if (AllowancesValue.ToString().Trim() == "NaN" || AllowancesValue.ToString().Trim() == "Infinity")
                    {
                        AllowancesValue = 0;
                    }


                    sumallow += AllowancesValue;
                    dtEmpPay.Rows.Add(row);

                    // Insert Allowance Temp Here\\\
                    // Update Ref Id = Trans Id of this Employee  in  Pay_Employe_Month_Temp
                    int k = objpayrollall.InsertPayrollEmpAllowance(str, ddlMonth.SelectedValue.ToString(), TxtYear.Text, ma.ToString(), dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString(), dtEmpAllowDeduc.Rows[i]["Value_Type"].ToString(), AllowancesValue.ToString(), AllowancesValue.ToString());

                }







                //here we will assign allowance according selected salary plan 





                if (IsPF == true)
                {

                    //double Employee_Pf_Amount = 0;
                    //double Employer_Pf_Amount = 0;
                    //double Employer_FPF = 0;
                    //double Employer_PF_Inspection_Charges = 0;
                    //double Employer_PF_EDLI = 0;
                    //double Employer_PF_Admin_Charges = 0;



                    //double Employee_Esic_Amount = 0;
                    //double Employer_Esic_Amount = 0;

                    string[] strPF = getEpfDeduction(basicsal.ToString(), Actualbasicsal.ToString(), strSalaryPlanId, str);

                    Employee_Pf_Amount = Convert.ToDouble(GetAmountDecimal(strPF[0].ToString()));
                    Employer_Pf_Amount = Convert.ToDouble(GetAmountDecimal(strPF[1].ToString()));
                    Employer_FPF = Convert.ToDouble(GetAmountDecimal(strPF[2].ToString()));
                    Employer_PF_Inspection_Charges = Convert.ToDouble(GetAmountDecimal(strPF[3].ToString()));
                    Employer_PF_EDLI = Convert.ToDouble(GetAmountDecimal(strPF[4].ToString()));
                    Employer_PF_Admin_Charges = Convert.ToDouble(GetAmountDecimal(strPF[5].ToString()));
                    Pf_Applicable_Amount = Convert.ToDouble(GetAmountDecimal(strPF[6].ToString()));

                    //Employee_Pf_Amount = (basicsal * Employee_PF) / 100;
                    //Employer_Pf_Amount = (basicsal * Employer_PF) / 100;

                }
                if (IsEsIc == true)
                {

                    string[] strEsIc = getESICDeduction(basicsal.ToString(), Actualbasicsal.ToString(), strSalaryPlanId, str);


                    Employee_Esic_Amount = Convert.ToDouble(GetAmountDecimal(strEsIc[0].ToString()));
                    Employer_Esic_Amount = Convert.ToDouble(GetAmountDecimal(strEsIc[1].ToString()));
                    Esic_Applicable_Amount = Convert.ToDouble(GetAmountDecimal(strEsIc[2].ToString()));

                }






                //here we are checking  the any arrear exist or not for selected employee

                objDA.execute_Command("delete from Pay_Employee_Claim where Emp_Id=" + str + " and Claim_Month='" + ddlMonth.SelectedValue + "' and Claim_Year='" + TxtYear.Text + "' and Claim_Name='Arrear'");


                DataTable dtArrear = ObjEmpArrear.GetRecordByEmployeeId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), str);

                if (dtArrear.Rows.Count > 0)
                {

                    ArrearAmt = Convert.ToDouble(dtArrear.Rows[0]["Arrear_amount"].ToString());

                    ObjClaim.Insert_In_Pay_Employee_Claim(Session["CompId"].ToString(), str, "Arrear", "Arrear From " + dtArrear.Rows[0]["FromMonth"].ToString() + " to " + dtArrear.Rows[0]["ToMonth"].ToString(), "1", dtArrear.Rows[0]["Arrear_amount"].ToString(), DateTime.Now.ToString(), "Approved", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "71", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    ObjEmpArrear.updateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), dtArrear.Rows[0]["Trans_Id"].ToString(), dtArrear.Rows[0]["Emp_Id"].ToString(), dtArrear.Rows[0]["From_Date"].ToString(), dtArrear.Rows[0]["To_Date"].ToString(), dtArrear.Rows[0]["Arrear_amount"].ToString(), dtArrear.Rows[0]["Currency_Id"].ToString(), false.ToString(), "0", true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());

                }

                dtArrear.Dispose();


                //here we are checking that salary plan is enable or not if enable then  we will insert deduction in related table


                if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsSalaryPlanEnable", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString())))
                {


                    string strDeductionId = string.Empty;
                    //for epf

                    strDeductionId = GetDeductionIdbyDeductionType("PF");

                    if (strDeductionId != "0")
                    {

                        if (Employee_PF > 0)
                        {
                            objDA.execute_Command("delete from Pay_Employee_Allow_Deduction where type=2 and ref_id=" + strDeductionId + "");
                            ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), str, "2", strDeductionId, "Monthly", "1", Employee_Pf_Amount.ToString(), Pf_Applicable_Amount.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }

                    }



                    //for esc
                    strDeductionId = GetDeductionIdbyDeductionType("ESIC");




                    if (strDeductionId != "0")
                    {

                        if (Employee_ESIC > 0)
                        {
                            objDA.execute_Command("delete from Pay_Employee_Allow_Deduction where type=2 and ref_id=" + strDeductionId + "");
                            ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), str, "2", strDeductionId, "Monthly", "1", Employee_Esic_Amount.ToString(), Esic_Applicable_Amount.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }

                    }


                    //for tds
                    //for tds we need to check that parameter sertting is auto or manual if manual then ni need to insert deduction row for tds

                    strDeductionId = GetDeductionIdbyDeductionType("TDS");



                    if (objAppParam.GetApplicationParameterValueByParamName("TDS_Functionality", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()).Trim() == "Auto")
                    {

                        objDA.execute_Command("delete from Pay_Employee_Allow_Deduction where type=2 and ref_id=" + strDeductionId + "");
                        if (strDeductionId != "0")
                        {

                            string[] strTds = getTDSdeduction(Grossamt, basicsal.ToString(), Actualbasicsal.ToString(), Employee_Pf_Amount.ToString(), strSalaryPlanId, strDob, str, ArrearAmt, PreviousEmployerTotalEarning, PreviousEmployerTotalTDS, SeniorCitizenagelimit, FinancialYearStartDate, FinancialYearEndDate, Doj);

                            if (Convert.ToDouble(strTds[0].ToString()) > 0)
                            {
                                ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), str, "2", strDeductionId, "Monthly", "1", strTds[0].ToString(), strTds[1].ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            }

                        }
                    }
                    else
                    {

                    }

                    //for pt

                    strDeductionId = GetDeductionIdbyDeductionType("PT");




                    if (strDeductionId != "0")
                    {
                        objDA.execute_Command("delete from Pay_Employee_Allow_Deduction where type=2 and ref_id=" + strDeductionId + "");
                        string[] strPT = getProfessionalTaxdeduction(basicsal.ToString(), Actualbasicsal.ToString(), strSalaryPlanId, strDob, str);
                        if (Convert.ToDouble(strPT[0].ToString()) > 0)
                        {
                            ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), str, "2", strDeductionId, "Monthly", "1", strPT[0].ToString(), strPT[1].ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }

                    }


                    //penalty for ctc employee

                    ///if emploee is ctc then we will deduct employer pf contribution from emploee salary as penalty 



                    DeletePenaltyByEmployeeIdandPenaltyName(str, "General Deduction");


                    if (IsCtc)
                    {
                        if ((Employer_Pf_Amount + Employer_FPF + Employer_PF_Admin_Charges + Employer_PF_EDLI + Employer_PF_Inspection_Charges) > 0)
                        {

                            b = objPEpenalty.Insert_In_Pay_Employee_Penalty(Session["CompId"].ToString(), str, "General Deduction", "Employer Pf Contribution ", "1", (Employer_Pf_Amount + Employer_FPF + Employer_PF_Admin_Charges + Employer_PF_EDLI + Employer_PF_Inspection_Charges).ToString(), DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "0", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }




                }


                //here we are checking that mobile payment

                DeletePenaltyByEmployeeIdandPenaltyName(str, "Mobile Penalty");

                if (GetMobileExceedAmount(str, strMobileNumber, MobileLimit.ToString()) > 0)
                {
                    b = objPEpenalty.Insert_In_Pay_Employee_Penalty(Session["CompId"].ToString(), str, "Mobile Penalty", "Exceed Mobile Usage", "1", GetMobileExceedAmount(str, strMobileNumber, MobileLimit.ToString()).ToString(), DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }




                //this code for insert the deduction record in deduction temp table


                string strTDSDeductionId = GetDeductionIdbyDeductionType("TDS");
                string strEPFDeductionId = GetDeductionIdbyDeductionType("PF");
                string strESICDeductionId = GetDeductionIdbyDeductionType("ESIC");

                dtEmpAllowDeduc = GetEmpAllowDedu(Convert.ToInt32(str));

                dtEmpAllowDeduc = new DataView(dtEmpAllowDeduc, "Type = 2", "", DataViewRowState.CurrentRows).ToTable();
                sumdeduc = 0;
                for (int i = 0; i < dtEmpAllowDeduc.Rows.Count; i++)
                {

                    Actualbasicsal = Common.getAllowanceanddeductionCalculation(dtEmpAllowDeduc.Rows[i]["Calculation_Type"].ToString(), dtEmpAllowDeduc.Rows[i]["Calculation_Value"].ToString(), dtPay, PerDaySal, str, ddlMonth.SelectedValue, TxtYear.Text, Session["DBConnection"].ToString())[1];
                    workeddaysPercentage = (Actualbasicsal * 100) / basicsal;


                    string CalMethod = string.Empty;
                    DataRow row = dtEmpPay.NewRow();
                    row[0] = str;

                    row[1] = "";
                    row[2] = ddlMonth.SelectedItem.Text;
                    row[3] = TxtYear.Text;
                    row[4] = dtEmpAllowDeduc.Rows[i]["Type"].ToString();
                    if (row[4].ToString() == "1")
                    {
                        row[4] = "Allowance";
                    }
                    else
                    {
                        row[4] = "Deduction";

                    }
                    CalMethod = dtEmpAllowDeduc.Rows[i]["Calculation_Method"].ToString();
                    row[5] = dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString();
                    row[6] = dtEmpAllowDeduc.Rows[i]["Value_Type"].ToString();
                    double DeductionValue = 0;
                    if (row[6].ToString() == "1")
                    {
                        row[7] = dtEmpAllowDeduc.Rows[i]["Value"].ToString();
                        double deduc = Convert.ToDouble(dtEmpAllowDeduc.Rows[i]["Value"]);

                        if (dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString() != strTDSDeductionId && dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString() != strEPFDeductionId && dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString() != strESICDeductionId)
                        {
                            deduc = (deduc * workeddaysPercentage) / 100;
                        }

                        if (CalMethod == "Daily")
                        {
                            deduc = deduc * WorkDays;
                        }

                        DeductionValue = deduc;
                        row[7] = deduc.ToString();
                    }
                    else
                    {
                        double val = 0;
                        if (dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString() != strTDSDeductionId && dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString() != strEPFDeductionId && dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString() != strESICDeductionId)
                        {
                            val = Convert.ToDouble((Actualbasicsal * Convert.ToDouble(dtEmpAllowDeduc.Rows[i]["Value"].ToString())) / 100);
                        }
                        else
                        {
                            val = Convert.ToDouble((basicsal * Convert.ToDouble(dtEmpAllowDeduc.Rows[i]["Value"].ToString())) / 100);
                        }



                        row[7] = val.ToString();
                        if (CalMethod == "Daily")
                        {
                            val = val * WorkDays;
                        }

                        DeductionValue = val;
                        row[7] = val.ToString();
                    }
                    //this code is created by jitendra upadhyay on 07-05-2014
                    //here we add the last month remaining allowances
                    //code start

                    int Lastmonth = 0;
                    int LastYear = 0;
                    string DeductionId = string.Empty;
                    DeductionId = "Deduction Id=" + dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString() + "";
                    DataTable dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(str,Session["CompId"].ToString());
                    if (ddlMonth.SelectedValue.ToString() == "1")
                    {
                        LastYear = Convert.ToInt32(TxtYear.Text) - 1;
                        Lastmonth = 12;
                    }
                    else
                    {
                        Lastmonth = Convert.ToInt32(ddlMonth.SelectedValue.ToString()) - 1;
                        LastYear = Convert.ToInt32(TxtYear.Text);
                    }

                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Deduction' and Field2='" + DeductionId + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                    if (dtDueamount.Rows.Count > 0)
                    {
                        if (dtDueamount.Rows[0]["Amount"].ToString().Trim() != "")
                        {
                            try
                            {
                                DeductionValue = DeductionValue + Convert.ToDouble(dtDueamount.Rows[0]["Amount"].ToString());
                            }
                            catch
                            {
                                if (row[7].ToString() != "")

                                    DeductionValue = Convert.ToDouble(row[7].ToString());
                                else

                                    DeductionValue = 0;
                            }
                        }
                    }

                    //code end


                    if (DeductionValue.ToString().Trim() == "NaN" || DeductionValue.ToString().Trim() == "Infinity")
                    {
                        DeductionValue = 0;
                    }
                    sumdeduc += DeductionValue;

                    dtEmpPay.Rows.Add(row);


                    double ApplicableAmount = 0;
                    try
                    {
                        ApplicableAmount = Convert.ToDouble(dtEmpAllowDeduc.Rows[i]["Field1"].ToString());
                    }
                    catch
                    {

                    }
                    // Insert Deduction Temp Here\\\
                    // Update Ref Id = Trans Id of this Employee  in  Pay_Employe_Month_Temp
                    int j = objpayrolldeduc.InsertPayrollEmpDeduction(str, ddlMonth.SelectedValue.ToString(), TxtYear.Text, ma.ToString(), dtEmpAllowDeduc.Rows[i]["Ref_Id"].ToString(), dtEmpAllowDeduc.Rows[i]["Value_Type"].ToString(), DeductionValue.ToString(), DeductionValue.ToString(), ApplicableAmount.ToString());


                }




                // Here we will get all Penality record of this Employee acc to selected Month and Year
                // We will get data table and loop accrding to rows
                sumpenalty = 0;
                DataTable dtEmpPenalty = objPEpenalty.GetRecord_From_PayEmployeePenalty_By_EmpId_MonthandYear(Session["CompId"].ToString(), str, "", ddlMonth.SelectedIndex.ToString(), TxtYear.Text, "", "");
                for (int i = 0; i < dtEmpPenalty.Rows.Count; i++)
                {

                    DataRow row = dtEmpPay.NewRow();
                    row[0] = str;
                    row[1] = "";
                    row[2] = ddlMonth.SelectedItem.Text;
                    row[3] = TxtYear.Text;
                    row[4] = dtEmpPenalty.Rows[i]["Penalty_Name"].ToString(); ;
                    row[5] = dtEmpPenalty.Rows[i]["Penalty_Id"].ToString();
                    row[6] = dtEmpPenalty.Rows[i]["Value_Type"].ToString();
                    if (row[6].ToString() == "1")
                    {
                        row[7] = dtEmpPenalty.Rows[i]["Value"].ToString();
                        double p = Convert.ToDouble(dtEmpPenalty.Rows[i]["Value"]);

                        //p =(p * workeddaysPercentage) / 100;

                        row[7] = p.ToString();
                        sumpenalty += p;
                    }
                    else
                    {
                        double val = Convert.ToDouble((basicsal * Convert.ToDouble(dtEmpPenalty.Rows[i]["Value"].ToString())) / 100);
                        row[7] = val.ToString();
                        sumpenalty += val;
                    }
                    row[6] = GetCalculation(dtEmpPenalty.Rows[i]["Value_Type"].ToString());


                    objPEpenalty.UpdateRecord_In_Pay_Employee_PenaltyAmount(dtEmpPenalty.Rows[i]["Company_Id"].ToString(), str, dtEmpPenalty.Rows[i]["Penalty_Id"].ToString(), row[7].ToString(), row[7].ToString(), ddlMonth.SelectedValue, TxtYear.Text, Session["UserId"].ToString(), DateTime.Now.ToString());


                    dtEmpPay.Rows.Add(row);


                }





                // Here we will get all claim record of this Employee acc to selected Month and Year only IsApproved Record
                // We will get data table and loop accrding to rows
                sum = 0;
                DataTable dtEmpClaim = objPEClaim.GetRecord_From_PayEmployeeClaim_By_EmpId_MonthandYear(Session["CompId"].ToString(), str, "", "Approved", ddlMonth.SelectedIndex.ToString(), TxtYear.Text, "", "");
                for (int i = 0; i < dtEmpClaim.Rows.Count; i++)
                {

                    DataRow row = dtEmpPay.NewRow();
                    row[0] = str;
                    row[1] = "";
                    row[2] = ddlMonth.SelectedItem.Text;
                    row[3] = TxtYear.Text;
                    row[4] = dtEmpClaim.Rows[i]["Claim_Approved"].ToString();
                    row[5] = dtEmpClaim.Rows[i]["Claim_Id"].ToString();
                    row[6] = dtEmpClaim.Rows[i]["Value_Type"].ToString();
                    if (row[6].ToString() == "1")
                    {
                        Double h = Convert.ToDouble(dtEmpClaim.Rows[i]["Value"]);

                        //h = (h * workeddaysPercentage) / 100;
                        row[7] = h.ToString();
                        sum += h;
                    }
                    else
                    {

                        double val = Convert.ToDouble((basicsal * Convert.ToDouble(dtEmpClaim.Rows[i]["Value"].ToString())) / 100);
                        row[7] = val.ToString();
                        sum += val;


                    }
                    row[6] = GetCalculation(dtEmpClaim.Rows[i]["Value_Type"].ToString());
                    objPEClaim.UpdateRecord_In_Pay_Employee_ClaimAmount(dtEmpClaim.Rows[i]["Company_Id"].ToString(), dtEmpClaim.Rows[i]["Claim_Id"].ToString(), str, row[7].ToString(), row[7].ToString(), ddlMonth.SelectedValue, TxtYear.Text, Session["UserId"].ToString(), DateTime.Now.ToString());


                    dtEmpPay.Rows.Add(row);
                }
                // Get employee loan 
                //this code is updated by jitendra upadhyay on 07-05-2014
                //for update the paid amount and status field in loan detail table when we post payroll without edit payroll
                //code start
                sumloan = 0;

                //code end

                monthclaim = ddlMonth.SelectedIndex;
                yearclaim = Convert.ToInt32(TxtYear.Text);
                if (monthclaim == 1)
                {
                    monthclaim = 12;
                    yearclaim = yearclaim - 1;

                }

                totaldueamt = 0;
                monthclaim = monthclaim - 1;
                DataTable dtdueamt = new DataTable();

                dtdueamt = objEmpDuePay.GetRecord_Emp_Due_paymentType1(str, monthclaim.ToString(), yearclaim.ToString(),Session["CompId"].ToString());

                DataTable dtdueamt1 = new DataTable();
                dtdueamt1 = objEmpDuePay.GetRecord_Emp_Due_paymentByType2(str, monthclaim.ToString(), yearclaim.ToString(), Session["CompId"].ToString());


                if (dtdueamt.Rows.Count > 0 && dtdueamt != null && dtdueamt.Rows[0]["Amount"].ToString() != "")
                {
                    dueamt = Convert.ToDouble(dtdueamt.Rows[0]["Amount"]);
                }

                if (dtdueamt1.Rows.Count > 0 && dtdueamt1 != null && dtdueamt1.Rows[0]["Amount"].ToString() != "")
                {

                    dueamtt = Convert.ToDouble(dtdueamt1.Rows[0]["Amount"]);

                }
                totaldueamt = dueamt - dueamtt;
                int u = 0;
                DataTable dtemprecord = new DataTable();
                dtemprecord = objPayEmpMonth.GetRecordByEmpIdMonthYear(str, ddlMonth.SelectedValue.ToString(), TxtYear.Text.ToString(), Session["CompId"].ToString());

                if (dtemprecord.Rows.Count > 0)
                {
                    u = objPayEmpMonth.UpdateRecord_PrvBal_fields_By_TransId(dtemprecord.Rows[0]["Trans_Id"].ToString(), totaldueamt.ToString(), dtemprecord.Rows[0]["Field1"].ToString(), dtemprecord.Rows[0]["Field2"].ToString(), dtemprecord.Rows[0]["Field3"].ToString(), dtemprecord.Rows[0]["Field4"].ToString(), dtemprecord.Rows[0]["Field5"].ToString(), dtemprecord.Rows[0]["Field6"].ToString(), dtemprecord.Rows[0]["Field7"].ToString(), dtemprecord.Rows[0]["Field8"].ToString(), dtemprecord.Rows[0]["Field9"].ToString(), dtemprecord.Rows[0]["Field10"].ToString(), Session["CompId"].ToString());

                }





                int dty = Convert.ToInt32(TxtYear.Text);
                int dtm = ddlMonth.SelectedIndex;
                int days = System.DateTime.DaysInMonth(dty, dtm);

                int Id = 0;
                Id = objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), str, ddlMonth.SelectedValue.ToString(), TxtYear.Text.ToString(), sumpenalty.ToString(), sum.ToString(), sumloan.ToString(), sumallow.ToString(), sumdeduc.ToString(), sumpenalty.ToString(), sum.ToString(), "0", Employer_Pf_Amount.ToString(), "0", Employer_Esic_Amount.ToString(), Employer_FPF.ToString(), Employer_PF_Admin_Charges.ToString(), Employer_PF_EDLI.ToString(), Employer_PF_Inspection_Charges.ToString());
                DataTable dtEmpattendnce = new DataTable();
                dtEmpattendnce = objEmpAttendance.GetRecord_Emp_Attendance(str, ddlMonth.SelectedValue.ToString(), TxtYear.Text, Session["CompId"].ToString());
                if (dtEmpattendnce.Rows.Count > 0)
                {
                    int s = objPayEmpMonth.UpdateAttendenceRecord_By_EmpId_Monthandyear(str, ddlMonth.SelectedValue.ToString(), TxtYear.Text.ToString(), dtEmpattendnce.Rows[0]["Basic_Work_Salary"].ToString(), dtEmpattendnce.Rows[0]["Normal_OT_Work_Salary"].ToString(), dtEmpattendnce.Rows[0]["WeekOff_OT_Work_Salary"].ToString(), dtEmpattendnce.Rows[0]["Holiday_OT_Work_Salary"].ToString(), dtEmpattendnce.Rows[0]["Leave_Days_Salary"].ToString(), dtEmpattendnce.Rows[0]["Week_Off_Days_Salary"].ToString(), dtEmpattendnce.Rows[0]["Holiday_Days_Salary"].ToString(), dtEmpattendnce.Rows[0]["Absent_Day_Penalty"].ToString(), dtEmpattendnce.Rows[0]["Late_Min_Penalty"].ToString(), dtEmpattendnce.Rows[0]["Early_Min_Penalty"].ToString(), dtEmpattendnce.Rows[0]["Parital_Violation_Penalty"].ToString(), Session["CompId"].ToString());
                }


                //Code By Ghanshyam Suthar on 09-02-2018

                string strLoandetailId = "0";
                DataTable dtempmonthtemp = new DataTable();
                dtempmonthtemp = objPayEmpMonth.GetallTemprecords_By_EmployeeId(str, Session["CompId"].ToString());
                dtempmonthtemp = new DataView(dtempmonthtemp, "Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                double GrossSalary = 0;
                if (dtempmonthtemp != null && dtempmonthtemp.Rows.Count > 0)
                {
                    GrossSalary = (float.Parse(dtempmonthtemp.Rows[0]["Worked_Min_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Normal_OT_Min_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Week_Off_OT_Min_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Holiday_OT_Min_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Leave_Days_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Week_Off_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Holidays_Salary"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Absent_Salary"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Late_Min_Penalty"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Early_Min_Penalty"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Patial_Violation_Penalty"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Employee_Penalty"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Employee_Claim"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Emlployee_Loan"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Total_Allowance"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Total_Deduction"].ToString()));
                }
                double Deduction = sumpenalty + sumdeduc + Employer_Pf_Amount + Employer_Esic_Amount + Employer_FPF + Employer_PF_Admin_Charges + Employer_PF_EDLI + Employer_PF_Inspection_Charges;
                double Addition = GrossSalary + sum + sumallow;
                double Net_Salary = Addition - Deduction;
                string Is_Greather = "False";
                DataTable Dtloan = new DataTable();
                Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
                Dtloan = new DataView(Dtloan, " Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < Dtloan.Rows.Count; i++)
                {
                    DataTable dtloandetial = new DataTable();
                    dtloandetial = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan.Rows[i]["Loan_Id"].ToString());
                    dtloandetial = new DataView(dtloandetial, "Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + " and Is_Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtloandetial.Rows.Count > 0)
                    {
                        //if (Net_Salary >= Convert.ToDouble(dtloandetial.Rows[0]["Total_Amount"].ToString()))
                        //{
                        //Is_Greather = "True";
                        double loan = 0;
                        DataRow row = dtEmpPay.NewRow();

                        if (dtloandetial.Rows[0]["Total_Amount"].ToString() != "")
                        {
                            row[6] = dtloandetial.Rows[0]["Total_Amount"].ToString();
                            loan = Convert.ToDouble(dtloandetial.Rows[0]["Total_Amount"]);

                            sumloan += loan;
                            Net_Salary -= loan;
                        }


                        strLoandetailId = dtloandetial.Rows[0]["Trans_Id"].ToString();



                        //}
                    }

                    //if (Is_Greather == "True")
                    //{
                    int Rc_Update = 0;
                    Rc_Update = objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), str, ddlMonth.SelectedValue.ToString(), TxtYear.Text.ToString(), sumpenalty.ToString(), sum.ToString(), sumloan.ToString(), sumallow.ToString(), sumdeduc.ToString(), sumpenalty.ToString(), sum.ToString(), "0", Employer_Pf_Amount.ToString(), "0", Employer_Esic_Amount.ToString(), Employer_FPF.ToString(), Employer_PF_Admin_Charges.ToString(), Employer_PF_EDLI.ToString(), Employer_PF_Inspection_Charges.ToString());

                    //}
                }
                // Code End by Ghanshyam Suthar




                // Update In Master Table Total Allowance /Deduction/Claim/Penalty
                // Acc to Month Year And Employee Id



            }
        }


    }

    public void insertAllowance_using_SalaryPlan(string strSalaryPlanId, string strEmployeeId, double GrossAmt, double BasicSalary, double actualBasicSalary, double TotalDay, double workingday)
    {



        double RowAmount = 0;

        double ActualAllowance = 0;

        DataTable dtsalaryPlanAll = objsalaryplandetail.GetDeduction_By_headerId(strSalaryPlanId);


        DataTable dtsalaryPlan = new DataView(dtsalaryPlanAll, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
        //  dttDetail = dttDetail.DefaultView.ToTable(true, "Trans_Id", "Ref_Id", "Calculation_Method", "Value", "Amount", "Deduction_Applicable");

        foreach (DataRow dr in dtsalaryPlan.Rows)
        {

            string strCalcmethod = string.Empty;
            if (dr["Calculation_Method"].ToString() == "Fixed")
            {
                strCalcmethod = "1";


                RowAmount += Convert.ToDouble(dr["Value"].ToString());

                //ActualAllowance = (Convert.ToDouble(dr["Value"].ToString()) / TotalDay) * workingday;


                ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), strEmployeeId, "1", dr["ref_id"].ToString(), "Monthly", strCalcmethod, dr["Value"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            else
            {
                strCalcmethod = "2";

                RowAmount += (BasicSalary * Convert.ToDouble(dr["Value"].ToString())) / 100;

                ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), strEmployeeId, "1", dr["ref_id"].ToString(), "Monthly", strCalcmethod, ((BasicSalary * Convert.ToDouble(dr["Value"].ToString())) / 100).ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }




        }

        if ((GrossAmt - (BasicSalary + RowAmount)) > 0)
        {

            dtsalaryPlan = new DataView(dtsalaryPlanAll, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable();


            if (dtsalaryPlan.Rows.Count > 0)
            {

                //ActualAllowance = ((GrossAmt - (BasicSalary + RowAmount)) / TotalDay) * workingday;


                ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), strEmployeeId, "1", dtsalaryPlan.Rows[0]["Ref_Id"].ToString(), "Monthly", "1", (GrossAmt - (BasicSalary + RowAmount)).ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
        }



    }

    public double GetActualAllowance(string strSalaryPlanId, double GrossAmt, double BasicSalary)
    {


        double RowAmount = 0;



        DataTable dtsalaryPlanAll = objsalaryplandetail.GetDeduction_By_headerId(strSalaryPlanId);


        DataTable dtsalaryPlan = new DataView(dtsalaryPlanAll, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
        //  dttDetail = dttDetail.DefaultView.ToTable(true, "Trans_Id", "Ref_Id", "Calculation_Method", "Value", "Amount", "Deduction_Applicable");

        foreach (DataRow dr in dtsalaryPlan.Rows)
        {

            string strCalcmethod = string.Empty;
            if (dr["Calculation_Method"].ToString() == "Fixed")
            {



                RowAmount += Convert.ToDouble(dr["Value"].ToString());


            }
            else
            {


                RowAmount += (BasicSalary * Convert.ToDouble(dr["Value"].ToString())) / 100;

            }
        }


        RowAmount += (GrossAmt - (BasicSalary + RowAmount));


        return RowAmount;
    }

    private string ValidPayRoll(string StrAllEmpId)
    {
        if (TxtYear.Text.Trim() == "")
        {
            TxtYear.Text = "0";
        }
        DataAccessClass objDA = new DataAccessClass(Session["DBConnection"].ToString());
        string strMessage = string.Empty;
        string strEmpId = string.Empty;


        foreach (string str in StrAllEmpId.Split(','))
        {
            if ((str != ""))
            {

                //DataTable dtPayInfo = objPayEmpMonth.GetPayEmpMonth(str);
                DataTable dtPayInfo = objDA.return_DataTable("select * from dbo.Pay_Employe_Month_Temp Where Emp_Id = '" + str + "'");
                if (dtPayInfo.Rows.Count > 0)
                {
                    if (dtPayInfo.Rows[0]["Month"].ToString() == ddlMonth.SelectedValue.ToString() && dtPayInfo.Rows[0]["Year"].ToString() == TxtYear.Text)
                    {
                        //strEmpId += str + ",";
                    }
                    else
                    {
                        if (lblPostFirst.Text == "")
                        {
                            if (Session["Lang"].ToString() == "1")
                            {
                                lblPostFirst.Text = "Please Post Generated PayRoll First for EmployeeCode= " + GetEmployeeCode(str) + "";
                            }
                            else
                            {
                                lblPostFirst.Text = GetArebicMessage("Please Post Generated PayRoll First for EmployeeCode") + "=" + GetEmployeeCode(str) + "";

                            }
                        }
                        else
                        {
                            lblPostFirst.Text = lblPostFirst.Text + "," + GetEmployeeCode(str);
                        }



                    }

                }
                else
                {

                    DataTable dtempmonth1 = new DataTable();
                    dtempmonth1 = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, ddlMonth.SelectedValue.ToString(), TxtYear.Text.ToString(),Session["CompId"].ToString());
                    if (dtempmonth1.Rows.Count > 0)
                    {
                        if (lblAlreadyPosted.Text == "")
                        {
                            if (Session["Lang"].ToString() == "1")
                            {

                                lblAlreadyPosted.Text = "Payroll Already Posted Before Now You Can Not Generate Again for EmployeeCode= " + GetEmployeeCode(str) + "";
                            }
                            else
                            {
                                lblAlreadyPosted.Text = GetArebicMessage("Payroll Already Posted Before Now You Can Not Generate Again for EmployeeCode") + "=" + GetEmployeeCode(str) + "";

                            }
                        }
                        else
                        {
                            lblAlreadyPosted.Text = lblAlreadyPosted.Text + "," + GetEmployeeCode(str);
                        }



                    }
                    else
                    {
                        DataTable dtPayPostedInfo = objDA.return_DataTable("select * from Pay_Employe_Month  where Emp_Id = '" + str + "' and Year = (Select MAX(Year) From Pay_Employe_Month  where Emp_Id = '" + str + "') order by MONTH desc");
                        // If we find record means employee old otherwise new
                        if (dtPayPostedInfo.Rows.Count > 0)
                        {
                            DateTime tTemp = new DateTime(Convert.ToInt16(dtPayPostedInfo.Rows[0]["Year"].ToString()), Convert.ToInt16(dtPayPostedInfo.Rows[0]["Month"].ToString()), 1);
                            tTemp = tTemp.AddMonths(1);

                            if (tTemp.Year.ToString() == TxtYear.Text.ToString() && tTemp.Month.ToString() == ddlMonth.SelectedValue.ToString())
                            {
                                strEmpId += str + ",";
                            }
                            else
                            {
                                if (lblWrongSequence.Text == "")
                                {
                                    if (Session["Lang"].ToString() == "1")
                                    {

                                        lblWrongSequence.Text = "Cannot Generate Payroll in Wrong Sequence for EmployeeCode= " + GetEmployeeCode(str) + "";
                                    }
                                    else
                                    {
                                        lblWrongSequence.Text = GetArebicMessage("Cannot Generate Payroll in Wrong Sequence for EmployeeCode") + "=" + GetEmployeeCode(str) + "";


                                    }
                                }
                                else
                                {
                                    lblWrongSequence.Text = lblWrongSequence.Text + "," + GetEmployeeCode(str);
                                }



                            }


                        }
                        else
                        {

                            //here we generate the payroll with doj of employee
                            //New Employee Code
                            // DOJ MOnth Year 
                            DataTable dt = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), str);
                            if (dt.Rows.Count > 0)
                            {

                                DateTime tEDOJ = Convert.ToDateTime(objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), str).Rows[0]["DOJ"].ToString());

                                //if (tEDOJ.Year.ToString() == TxtYear.Text.ToString() && tEDOJ.Month.ToString() == ddlMonth.SelectedValue.ToString())
                                //{
                                //    strEmpId += str + ",";
                                //}
                                //else
                                //{
                                //    // Add in message DOJ Issue 
                                //}

                                if (tEDOJ.Year <= Convert.ToInt16(TxtYear.Text))
                                {
                                    if (tEDOJ.Year < Convert.ToInt16(TxtYear.Text))
                                    {
                                        strEmpId += str + ",";
                                    }
                                    else
                                    {
                                        if (tEDOJ.Month <= Convert.ToInt16(ddlMonth.SelectedValue.ToString()))
                                        {
                                            strEmpId += str + ",";
                                        }
                                        else
                                        {
                                            if (lblDojIssue.Text == "")
                                            {
                                                if (Session["Lang"].ToString() == "1")
                                                {

                                                    lblDojIssue.Text = "Payroll Month and year should be greater than or equal to Date Of Joining for EmployeeCode=" + GetEmployeeCode(str);
                                                }
                                                else
                                                {
                                                    lblDojIssue.Text = GetArebicMessage("Payroll Month and year should be greater than or equal to Date Of Joining for EmployeeCode") + "=" + GetEmployeeCode(str);


                                                }
                                            }
                                            else
                                            {
                                                lblDojIssue.Text = lblDojIssue.Text + "," + GetEmployeeCode(str);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    if (lblDojIssue.Text == "")
                                    {
                                        if (Session["Lang"].ToString() == "1")
                                        {

                                            lblDojIssue.Text = "Payroll Month and year should be greater than or equal to Date Of Joining for EmployeeCode=" + GetEmployeeCode(str);
                                        }
                                        else
                                        {
                                            lblDojIssue.Text = GetArebicMessage("Payroll Month and year should be greater than or equal to Date Of Joining for EmployeeCode") + "=" + GetEmployeeCode(str);


                                        }
                                    }
                                    else
                                    {
                                        lblDojIssue.Text = lblDojIssue.Text + "," + GetEmployeeCode(str);
                                    }
                                }
                            }

                        }




                    }

                }

            }
        }


        //DisplayMessage(strMessage);
        return strEmpId;


    }
    public string GetEmployeeCode(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(),empid.ToString());
       

        if (dt.Rows.Count > 0)
        {

            empname = dt.Rows[0]["Emp_Code"].ToString();

            if (empname == "")
            {
                empname = "No Code";
            }

        }
        else
        {
            empname = "No Code";

        }

        return empname;
    }

    void ValidEmpList()
    {
        DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());
        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        try
        {
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        catch
        {
        }
        string EmpLIst = string.Empty;
        for (int i = 0; i < dtEmp.Rows.Count; i++)
        {
            if (EmpLIst.Trim() == "")
            {
                EmpLIst = dtEmp.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                EmpLIst = EmpLIst + "," + dtEmp.Rows[i]["Emp_Id"].ToString();
            }

        }
        Session["dtEmpList"] = EmpLIst;
    }

    public DataTable Get_Decimal_Count(string Currency_ID, string Currency_Code, string Currency_Name, string Country_Id, string Country_Name, string Country_Code)
    {
        DataTable Dt_Decimal_Count = new DataTable();
        Dt_Decimal_Count = objCurrency.GetDecimalCount(Currency_ID, Currency_Code, Currency_Name, Country_Id, Country_Name, Country_Code, "1");
        return Dt_Decimal_Count;
    }

    protected void btnpostpayroll_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        string strLocId = ddlLocation.SelectedIndex > 0 ? ddlLocation.SelectedValue : Session["LocId"].ToString();
        try
        {
            string strMaxVoucheno = da.return_DataTable("select max(ISNULL( pay_employe_month.Voucher_No,0))+1 from pay_employe_month inner join set_employeemaster on pay_employe_month.Emp_Id= set_employeemaster.Emp_Id where set_employeemaster.Location_Id=" + strLocId + "").Rows[0][0].ToString();

            string strPendingGenPayroll = string.Empty;
            string StrPostedEmployee = string.Empty;
            lblPostFirst.Text = "";
            lblAlreadyPosted.Text = "";
            lblWrongSequence.Text = "";
            lblDojIssue.Text = "";

            DataTable dtEmpPay = GetTable();
            string TransNo = string.Empty;
            string SalaryPlanid = string.Empty;
            DataTable dtAppParam = new DataTable();
            double Employee_PF = 0;
            double Employer_PF = 0;
            double Employee_ESIC = 0;
            double Employer_ESIC = 0;

            double PF = 0;
            double ESIC = 0;
            double PF1 = 0;
            double ESIC1 = 0;

            bool IsPF = false;
            bool IsESIC = false;
            double BasicSal = 0;
            double EmpGrossSalary = 0;
            double TotalEmployeeAllowance = 0;
            bool IsEmpINPayroll = false;
            double ActualGross = 0;
            string[] EmpDetails = new string[1];
            try
            {
                Employee_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_PF", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            }
            catch
            {

            }

            try
            {
                Employer_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_PF", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            }
            catch
            {

            }

            try
            {
                Employee_ESIC = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_ESIC", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            }
            catch
            {
            }

            try
            {
                Employer_ESIC = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_ESIC", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            }
            catch
            {

            }

            if (rbtnGroup.Checked)
            {
                string GroupIds = string.Empty;
                string EmpIds = string.Empty;

                for (int i = 0; i < lbxGroup.Items.Count; i++)
                {
                    if (lbxGroup.Items[i].Selected)
                    {
                        GroupIds += lbxGroup.Items[i].Value + ",";
                    }
                }

                if (GroupIds != "")
                {
                    DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString(), ref trns);

                    dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                    //this code is for filter only those employee which are allow for login  user
                    try
                    {
                        dtEmpInGroup = new DataView(dtEmpInGroup, "Emp_Id in (" + Session["dtEmpList"].ToString().Trim() + ") ", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }

                    for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                    {
                        if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                        {
                            if (ValidPayRoll(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()) != "")
                            {
                                EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                            }
                        }
                    }
                }
                else
                {
                    DisplayMessage("Select Group First");
                }


                strPendingGenPayroll = CheckPayrollStatus(EmpIds);

                if (strPendingGenPayroll.Trim() != "")
                {
                    DisplayMessage("Payroll not generated for employee code = " + strPendingGenPayroll);
                    return;
                }





                int EmpCount = -1;
                foreach (string str in EmpIds.Split(','))
                {
                    var arr = EmpIds.Split(',');
                    if (EmpCount == -1)
                        EmpDetails = new string[arr.Length - 1];
                    if ((str != ""))
                    {
                        EmpCount++;


                        UpdateMobileAdjustedFlag(str, trns);
                        //here we insert the row in pay_employee_attendence table with current month and current year
                        //this code is update on 05-04-2014
                        DataTable DtEmpAttendence = objPayEmpAtt.GetRecord_Emp_Attendance(str, ddlMonth.SelectedValue, TxtYear.Text, ref trns,Session["CompId"].ToString());
                        if (DtEmpAttendence.Rows.Count == 0)
                        {

                            DataTable dtobjempdetail = objEmpDetail.GetAllTrueRecord(ref trns);
                            try
                            {
                                dtobjempdetail = new DataView(dtobjempdetail, "Emp_Id=" + str + " and Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            catch
                            {
                            }
                            if (dtobjempdetail.Rows.Count > 0)
                            {
                                objPayEmpAtt.InsertPayEmployeeAttendance(Session["CompId"].ToString(), str, ddlMonth.SelectedValue, TxtYear.Text, Math.Round(float.Parse(dtobjempdetail.Rows[0]["Total_Days"].ToString()), 0).ToString(), "0", Math.Round(float.Parse(dtobjempdetail.Rows[0]["Present_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["WeekOff_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Holiday_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Leave_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Absent_Days"].ToString()), 0).ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            }
                            else
                            {
                                objPayEmpAtt.InsertPayEmployeeAttendance(Session["CompId"].ToString(), str, ddlMonth.SelectedValue, TxtYear.Text, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }

                        PF = 0;
                        ESIC = 0;
                        PF1 = 0;
                        ESIC1 = 0;


                        IsPF = false;
                        IsESIC = false;
                        BasicSal = 0;
                        IsEmpINPayroll = false;
                        DataTable dtempmonthtemp = new DataTable();
                        dtempmonthtemp = objPayEmpMonth.GetallTemprecords_By_EmployeeId(str, ref trns, Session["CompId"].ToString());
                        dtempmonthtemp = new DataView(dtempmonthtemp, "Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();

                        if (dtempmonthtemp.Rows.Count > 0)
                        {
                            DataTable dt = objempparam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString(), ref trns);

                            if (dt.Rows.Count > 0)
                            {


                                SalaryPlanid = dt.Rows[0]["Salary_Plan_Id"].ToString();

                                try
                                {
                                    IsEmpINPayroll = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
                                }
                                catch
                                {

                                }


                                try
                                {
                                    IsPF = Convert.ToBoolean(dt.Rows[0]["Field4"].ToString());
                                }
                                catch
                                {

                                }
                                try
                                {
                                    IsESIC = Convert.ToBoolean(dt.Rows[0]["Field5"].ToString());
                                }
                                catch
                                {

                                }


                                try
                                {
                                    BasicSal = double.Parse(dt.Rows[0]["Basic_Salary"].ToString());
                                }
                                catch
                                {

                                }

                                try
                                {
                                    EmpGrossSalary = Convert.ToDouble(dt.Rows[0]["Gross_Salary"].ToString());
                                }
                                catch
                                {


                                }


                                try
                                {
                                    TotalEmployeeAllowance = GetActualAllowance(SalaryPlanid, EmpGrossSalary, BasicSal);
                                }
                                catch
                                {

                                }


                                if (IsEmpINPayroll)
                                {
                                    if (BasicSal.ToString() != "")
                                    {
                                        if (IsPF == true)
                                        {
                                            PF = (BasicSal * Employee_PF) / 100;
                                            PF1 = (BasicSal * Employer_PF) / 100;

                                        }


                                        if (IsESIC == true)
                                        {
                                            ESIC1 = (BasicSal * Employer_ESIC) / 100;
                                            ESIC = (BasicSal * Employee_ESIC) / 100;
                                        }
                                    }
                                }
                            }

                            int TransId = objPayEmpMonth.Insert_posted_Pay_Emp_Month(Session["CompId"].ToString(), str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), dtempmonthtemp.Rows[0]["Worked_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Normal_OT_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Week_Off_OT_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Holiday_OT_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Leave_Days_Salary"].ToString(), dtempmonthtemp.Rows[0]["Week_Off_Salary"].ToString(), dtempmonthtemp.Rows[0]["Holidays_Salary"].ToString(), dtempmonthtemp.Rows[0]["Absent_Salary"].ToString(), dtempmonthtemp.Rows[0]["Late_Min_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Early_Min_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Patial_Violation_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Employee_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Employee_Claim"].ToString(), dtempmonthtemp.Rows[0]["Emlployee_Loan"].ToString(), dtempmonthtemp.Rows[0]["Total_Allowance"].ToString(), dtempmonthtemp.Rows[0]["Total_Deduction"].ToString(), dtempmonthtemp.Rows[0]["Previous_Month_Balance"].ToString(), System.DateTime.Now.ToString(), PF.ToString(), PF1.ToString(), ESIC.ToString(), ESIC1.ToString(), EmpGrossSalary.ToString(), BasicSal.ToString(), TotalEmployeeAllowance.ToString(), "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            //here we are inserting row in employee statement table 

                            double GrossSalary = 0;

                            GrossSalary = (float.Parse(dtempmonthtemp.Rows[0]["Worked_Min_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Normal_OT_Min_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Week_Off_OT_Min_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Holiday_OT_Min_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Leave_Days_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Week_Off_Salary"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Holidays_Salary"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Absent_Salary"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Late_Min_Penalty"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Early_Min_Penalty"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Patial_Violation_Penalty"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Employee_Penalty"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Employee_Claim"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Emlployee_Loan"].ToString()) + float.Parse(dtempmonthtemp.Rows[0]["Total_Allowance"].ToString()) - float.Parse(dtempmonthtemp.Rows[0]["Total_Deduction"].ToString()));


                            objAdvancePayment.InsertEmployeeStatement(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), str, DateTime.Now.ToString(), "0", GrossSalary.ToString(), Session["CurrencyId"].ToString(), "0", GrossSalary.ToString(), "0", GrossSalary.ToString(), "Pay_Employe_Month", TransId.ToString(), "0", "Employee Salary of " + ddlMonth.SelectedItem.Text + "-" + TxtYear.Text, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), false.ToString(), ref trns);

                            string EmpDetail = str + "," + GrossSalary.ToString();
                            EmpDetails[EmpCount] = EmpDetail;

                            DataTable dtArrear = ObjEmpArrear.GetRecordByEmployeeId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), str, ref trns);

                            if (dtArrear.Rows.Count > 0)
                            {
                                ObjEmpArrear.updateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), dtArrear.Rows[0]["Trans_Id"].ToString(), dtArrear.Rows[0]["Emp_Id"].ToString(), dtArrear.Rows[0]["From_Date"].ToString(), dtArrear.Rows[0]["To_Date"].ToString(), dtArrear.Rows[0]["Arrear_amount"].ToString(), dtArrear.Rows[0]["Currency_Id"].ToString(), true.ToString(), TransId.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);


                            }




                            StrPostedEmployee += GetEmployeeCode(str) + ",";
                            DataTable dtemppayrollpost = new DataTable();
                            dtemppayrollpost = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns, Session["CompId"].ToString());


                            if (dtemppayrollpost.Rows.Count > 0)
                            {

                                ActualGross = (float.Parse(dtemppayrollpost.Rows[0]["Worked_Min_Salary"].ToString()) + float.Parse(dtemppayrollpost.Rows[0]["Leave_Days_Salary"].ToString()) + float.Parse(dtemppayrollpost.Rows[0]["Week_Off_Salary"].ToString()) + float.Parse(dtemppayrollpost.Rows[0]["Holidays_Salary"].ToString()) + float.Parse(dtemppayrollpost.Rows[0]["Total_Allowance"].ToString()));

                            }

                            UpdateEmployeeGrossSalary(str, ActualGross.ToString(), strMaxVoucheno, trns);


                            DataTable dtallowancetemp = new DataTable();
                            dtallowancetemp = objpayrollall.GetPayAllowPayaRoll(str, ref trns);

                            if (dtallowancetemp.Rows.Count > 0)
                            {
                                objpayrollall.InsertPostPayrollAllowance(str, dtallowancetemp.Rows[0]["Month"].ToString(), dtallowancetemp.Rows[0]["Year"].ToString(), dtemppayrollpost.Rows[0]["Trans_Id"].ToString(), dtallowancetemp.Rows[0]["Allowance_Id"].ToString(), dtallowancetemp.Rows[0]["Allowance_Type"].ToString(), dtallowancetemp.Rows[0]["Allowance_Value"].ToString(), dtallowancetemp.Rows[0]["Act_Allowance_Value"].ToString(), System.DateTime.Now.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                objpayrollall.DeletePayAllowanceTemp_By_EmpId_MonthandYear(str, dtallowancetemp.Rows[0]["Month"].ToString(), dtallowancetemp.Rows[0]["Year"].ToString(), ref trns);
                            }

                            DataTable dtdeductiontemp = new DataTable();
                            dtdeductiontemp = objpayrolldeduc.GetPayDeducPayaRoll(str, ref trns);

                            if (dtdeductiontemp.Rows.Count > 0)
                            {
                                objpayrolldeduc.InsertPostPayrollDeduction(str, dtdeductiontemp.Rows[0]["Month"].ToString(), dtdeductiontemp.Rows[0]["Year"].ToString(), dtemppayrollpost.Rows[0]["Trans_Id"].ToString(), dtdeductiontemp.Rows[0]["Deduction_Id"].ToString(), dtdeductiontemp.Rows[0]["Deduction_Type"].ToString(), dtdeductiontemp.Rows[0]["Deduction_Value"].ToString(), dtdeductiontemp.Rows[0]["Act_Deduction_Value"].ToString(), System.DateTime.Now.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                objpayrolldeduc.DeletePayDeductionTemp_By_EmpId_MonthandYear(str, dtdeductiontemp.Rows[0]["Month"].ToString(), dtdeductiontemp.Rows[0]["Year"].ToString(), ref trns);
                            }
                            objPayEmpMonth.DeleteEmpMonthTemp_By_EmpId_MonthandYear(Session["CompId"].ToString(), str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns);
                        }
                        else
                        {
                            //DisplayMessage("You can not payroll posted");
                        }
                    }
                }

                //for Grp Code add Finance Code
            }
            else
            {
                int TransId = 0;
                int EmpCount = -1;
                SaveCheckedValues();
                ArrayList userdetails = new ArrayList();
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                    if (userdetails.Count == 0)
                    {
                        DisplayMessage("Select Employee First");
                        return;
                    }
                }
                else
                {
                    DisplayMessage("Select Employee First");
                    return;
                }




                if (txtCreditAccount.Text != "")
                {

                }
                else
                {
                    DisplayMessage("Please fill Credit Account");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCreditAccount);
                    return;
                }
                if (txtDebitAccount.Text != "")
                {

                }
                else
                {
                    DisplayMessage("Please fill Debit Account");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDebitAccount);
                    return;
                }

                string strFinalPayEmp = string.Empty;
                for (int i = 0; i < userdetails.Count; i++)
                {
                    strFinalPayEmp += userdetails[i].ToString() + ",";
                }


                strPendingGenPayroll = CheckPayrollStatus(strFinalPayEmp);

                if (strPendingGenPayroll.Trim() != "")
                {
                    DisplayMessage("Payroll not generated for employee code = " + strPendingGenPayroll);
                    return;
                }



                string strCurrencyId = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();

                DataTable Dt_Decimal_Count = Get_Decimal_Count(strCurrencyId, "", "", "0", "", "");
                double TotalAmountPay = 0;
                foreach (string str in strFinalPayEmp.Split(','))
                {

                    var arr = strFinalPayEmp.Split(',');
                    if (EmpCount == -1)
                        EmpDetails = new string[arr.Length - 1];
                    if ((str != ""))
                    {
                        EmpCount++;
                        UpdateMobileAdjustedFlag(str, trns);

                        //here we insert the row in pay_employee_attendence table with current month and current year
                        //this code is update on 05-04-2014
                        DataTable DtEmpAttendence = objPayEmpAtt.GetRecord_Emp_Attendance(str, ddlMonth.SelectedValue, TxtYear.Text, ref trns, Session["CompId"].ToString());
                        if (DtEmpAttendence.Rows.Count == 0)
                        {

                            //this code is created by jitendra upadhyay on 01-04-2015
                            //this code for check that record exists or not in employe detail table if exisr than we insert from employee detail table
                            DataTable dtobjempdetail = objEmpDetail.GetAllTrueRecord(ref trns);
                            try
                            {
                                dtobjempdetail = new DataView(dtobjempdetail, "Emp_Id=" + str + " and Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            catch
                            {

                            }
                            if (dtobjempdetail.Rows.Count > 0)
                            {
                                objPayEmpAtt.InsertPayEmployeeAttendance(Session["CompId"].ToString(), str, ddlMonth.SelectedValue, TxtYear.Text, Math.Round(float.Parse(dtobjempdetail.Rows[0]["Total_Days"].ToString()), 0).ToString(), "0", Math.Round(float.Parse(dtobjempdetail.Rows[0]["Present_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["WeekOff_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Holiday_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Leave_Days"].ToString()), 0).ToString(), Math.Round(float.Parse(dtobjempdetail.Rows[0]["Absent_Days"].ToString()), 0).ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            else
                            {
                                objPayEmpAtt.InsertPayEmployeeAttendance(Session["CompId"].ToString(), str, ddlMonth.SelectedValue, TxtYear.Text, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", DateTime.Now.ToString(), "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }

                        PF = 0;
                        ESIC = 0;
                        PF1 = 0;
                        ESIC1 = 0;


                        IsPF = false;
                        IsESIC = false;
                        BasicSal = 0;


                        IsEmpINPayroll = false;
                        DataTable dtempmonthtemp = new DataTable();
                        dtempmonthtemp = objPayEmpMonth.GetallTemprecords_By_EmployeeId(str, ref trns, Session["CompId"].ToString());
                        dtempmonthtemp = new DataView(dtempmonthtemp, "Emp_Id=" + str + " ", "", DataViewRowState.CurrentRows).ToTable();

                        if (dtempmonthtemp.Rows.Count > 0)
                        {
                            EmployeeParameter objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
                            DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString(), ref trns);

                            if (dt.Rows.Count > 0)
                            {

                                SalaryPlanid = dt.Rows[0]["Salary_Plan_Id"].ToString();
                                try
                                {
                                    IsEmpINPayroll = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
                                }
                                catch
                                {

                                }



                                try
                                {
                                    IsPF = Convert.ToBoolean(dt.Rows[0]["Field4"].ToString());
                                }
                                catch
                                {

                                }

                                try
                                {
                                    IsESIC = Convert.ToBoolean(dt.Rows[0]["Field5"].ToString());
                                }
                                catch
                                {

                                }


                                try
                                {
                                    BasicSal = double.Parse(dt.Rows[0]["Basic_Salary"].ToString());
                                }
                                catch
                                {

                                }

                                try
                                {
                                    EmpGrossSalary = Convert.ToDouble(dt.Rows[0]["Gross_Salary"].ToString());
                                }
                                catch
                                {


                                }


                                try
                                {
                                    TotalEmployeeAllowance = GetActualAllowance(SalaryPlanid, EmpGrossSalary, BasicSal);
                                }
                                catch
                                {

                                }



                                if (IsEmpINPayroll)
                                {
                                    if (BasicSal.ToString() != "")
                                    {
                                        if (IsPF == true)
                                        {
                                            PF = (BasicSal * Employee_PF) / 100;
                                            PF1 = (BasicSal * Employer_PF) / 100;
                                        }
                                        if (IsESIC == true)
                                        {
                                            ESIC1 = (BasicSal * Employer_ESIC) / 100;
                                            ESIC = (BasicSal * Employee_ESIC) / 100;
                                        }
                                    }
                                }
                            }

                            //this code also for insert the record in payroll table(pay_employee_month) from temporary table(pay_employee_month_Temp)
                            //code start

                            TransId = objPayEmpMonth.Insert_posted_Pay_Emp_Month(Session["CompId"].ToString(), str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), dtempmonthtemp.Rows[0]["Worked_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Normal_OT_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Week_Off_OT_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Holiday_OT_Min_Salary"].ToString(), dtempmonthtemp.Rows[0]["Leave_Days_Salary"].ToString(), dtempmonthtemp.Rows[0]["Week_Off_Salary"].ToString(), dtempmonthtemp.Rows[0]["Holidays_Salary"].ToString(), dtempmonthtemp.Rows[0]["Absent_Salary"].ToString(), dtempmonthtemp.Rows[0]["Late_Min_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Early_Min_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Patial_Violation_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Employee_Penalty"].ToString(), dtempmonthtemp.Rows[0]["Employee_Claim"].ToString(), dtempmonthtemp.Rows[0]["Emlployee_Loan"].ToString(), dtempmonthtemp.Rows[0]["Total_Allowance"].ToString(), dtempmonthtemp.Rows[0]["Total_Deduction"].ToString(), dtempmonthtemp.Rows[0]["Previous_Month_Balance"].ToString(), System.DateTime.Now.ToString(), dtempmonthtemp.Rows[0]["Employee_PF"].ToString(), dtempmonthtemp.Rows[0]["Employer_PF"].ToString(), dtempmonthtemp.Rows[0]["Employee_ESIC"].ToString(), dtempmonthtemp.Rows[0]["Employer_ESIC"].ToString(), dtempmonthtemp.Rows[0]["Field3"].ToString(), dtempmonthtemp.Rows[0]["Field4"].ToString(), dtempmonthtemp.Rows[0]["Field5"].ToString(), dtempmonthtemp.Rows[0]["Field6"].ToString(), EmpGrossSalary.ToString(), BasicSal.ToString(), TotalEmployeeAllowance.ToString(), "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            string strEmpCodeOnly = GetEmployeeCode(str);
                            StrPostedEmployee += GetEmployeeCode(str) + ",";
                            //Code end



                            DataTable Dtloan = new DataTable();
                            Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved", ref trns);

                            string Narration = string.Empty;
                            DataTable dtemppayrollpost = new DataTable();
                            dtemppayrollpost = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns, Session["CompId"].ToString());
                            string strEmployeeActualSalary = string.Empty;
                            if (dtemppayrollpost.Rows.Count > 0)
                            {
                                strEmployeeActualSalary = string.Empty;
                                string WorkedSalary = dtemppayrollpost.Rows[0]["Worked_Min_Salary"].ToString();
                                string NormalOT = dtemppayrollpost.Rows[0]["Normal_OT_Min_Salary"].ToString();
                                string WeekOffOT = dtemppayrollpost.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                                string HolidayOT = dtemppayrollpost.Rows[0]["Holiday_OT_Min_Salary"].ToString();
                                string LeaveDaysSalary = dtemppayrollpost.Rows[0]["Leave_Days_Salary"].ToString();
                                string WeekOffSalary = dtemppayrollpost.Rows[0]["Week_Off_Salary"].ToString();
                                string HolidaySalary = dtemppayrollpost.Rows[0]["Holidays_Salary"].ToString();
                                string AbsentSalary = dtemppayrollpost.Rows[0]["Absent_Salary"].ToString();
                                string LateMinPenalty = dtemppayrollpost.Rows[0]["Late_Min_Penalty"].ToString();
                                string EarlyMinPenalty = dtemppayrollpost.Rows[0]["Early_Min_Penalty"].ToString();
                                string PartialViolationPenalty = dtemppayrollpost.Rows[0]["Patial_Violation_Penalty"].ToString();
                                string EmpPenalty = dtemppayrollpost.Rows[0]["Employee_Penalty"].ToString();
                                string EmpClaim = dtemppayrollpost.Rows[0]["Employee_Claim"].ToString();
                                string EmpLoan = dtemppayrollpost.Rows[0]["Emlployee_Loan"].ToString();
                                string TotalAllowance = dtemppayrollpost.Rows[0]["Total_Allowance"].ToString();
                                string TotalDeduction = dtemppayrollpost.Rows[0]["Total_Deduction"].ToString();
                                string PreviousMonthAdjust = dtemppayrollpost.Rows[0]["Previous_Month_Balance"].ToString();
                                string EmployeePF = dtemppayrollpost.Rows[0]["Employee_PF"].ToString();
                                string EmployeeESIC = dtemppayrollpost.Rows[0]["Employee_ESIC"].ToString();

                                ActualGross = (float.Parse(WorkedSalary) + float.Parse(LeaveDaysSalary) + float.Parse(WeekOffSalary) + float.Parse(HolidaySalary) + float.Parse(TotalAllowance));

                                string strAddition = (float.Parse(WorkedSalary) + float.Parse(NormalOT) + float.Parse(WeekOffOT) + float.Parse(HolidayOT) + float.Parse(LeaveDaysSalary) + float.Parse(WeekOffSalary) + float.Parse(HolidaySalary) + float.Parse(EmpClaim) + float.Parse(TotalAllowance) + float.Parse(PreviousMonthAdjust)).ToString();

                                string strDeduction = (float.Parse(AbsentSalary) + float.Parse(LateMinPenalty) + float.Parse(EarlyMinPenalty) + float.Parse(PartialViolationPenalty) + float.Parse(EmpPenalty) + float.Parse(TotalDeduction)).ToString();

                                //Code Comment by Ghanhshyam Suthar because loan Entry not save in this voucher on 09-02-2018
                                double sumloan = 0;
                                //DataTable Dtloan_Str = new DataView(Dtloan, " Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                                //for (int i = 0; i < Dtloan_Str.Rows.Count; i++)
                                //{
                                //    DataTable dtloandetial = new DataTable();
                                //    dtloandetial = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan_Str.Rows[i]["Loan_Id"].ToString(), ref trns);
                                //    dtloandetial = new DataView(dtloandetial, "Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + " and Is_Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                                //    if (dtloandetial.Rows.Count > 0)
                                //    {
                                //        double loan = 0;
                                //        DataRow row = dtEmpPay.NewRow();
                                //        if (dtloandetial.Rows[0]["Total_Amount"].ToString() != "")
                                //        {
                                //            row[6] = dtloandetial.Rows[0]["Total_Amount"].ToString();
                                //            loan = Convert.ToDouble(dtloandetial.Rows[0]["Total_Amount"]);
                                //            sumloan += loan;
                                //        }
                                //    }
                                //}
                                sumloan = Convert.ToDouble(EmpLoan);

                                //string strDeduction = (float.Parse(AbsentSalary) + float.Parse(LateMinPenalty) + float.Parse(EarlyMinPenalty) + float.Parse(PartialViolationPenalty) + float.Parse(EmpPenalty) + float.Parse(EmpLoan) + float.Parse(TotalDeduction)).ToString();
                                // Code End


                                //Narration = "Payroll For the month " + ddlMonth.SelectedItem.Text.ToString() + "-" + TxtYear.Text.ToString() + "";


                                if (strDeduction == "0")
                                    Narration = "Payroll For the month " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.ToString() + " (Gross Amount = " + (Convert.ToDouble(strAddition)).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString()) + ")";
                                else
                                    Narration = "Payroll For the month " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.ToString() + " (Gross Amount = " + (Convert.ToDouble(strAddition)).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString()) + " & Deduction Amount = " + Convert.ToDecimal(strDeduction).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString()) + ")";

                                if (sumloan != 0)
                                {
                                    if (Convert.ToDouble(strAddition) < sumloan)
                                    {
                                        Narration = Narration + " Note: Unable to deduct loan installment amount " + sumloan + " because of insufficient salary.";
                                    }
                                }

                                //if (float.Parse(strAddition) > float.Parse(strDeduction))
                                //{
                                strEmployeeActualSalary = (float.Parse(strAddition) - float.Parse(strDeduction)).ToString();
                                strEmployeeActualSalary = Common.Get_Roundoff_Amount_By_Location(strEmployeeActualSalary, Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
                                //}
                                //else
                                //{
                                //    strEmployeeActualSalary = (float.Parse(strDeduction) - float.Parse(strAddition)).ToString();
                                //}

                                TotalAmountPay += Convert.ToDouble(strEmployeeActualSalary);
                            }


                            double GrossSalary = 0;

                            //GrossSalary = TotalAmountPay;
                            //string EmpDetail = str + "," + GrossSalary.ToString();
                            string EmpDetail = str + "," + strEmployeeActualSalary.ToString() + "," + Narration;
                            EmpDetails[EmpCount] = EmpDetail;

                            //here we are updating gross salary in pay_eemployee_month table
                            //28-06-2017 by jitendra




                            UpdateEmployeeGrossSalary(str, ActualGross.ToString(), strMaxVoucheno, trns);


                            objAdvancePayment.InsertEmployeeStatement(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), str, DateTime.Now.ToString(), "0", GrossSalary.ToString(), Session["CurrencyId"].ToString(), "0", GrossSalary.ToString(), "0", GrossSalary.ToString(), "Pay_Employe_Month", TransId.ToString(), "0", "Employee Salary of " + ddlMonth.SelectedItem.Text + "-" + TxtYear.Text, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), false.ToString(), ref trns);


                            DataTable dtArrear = ObjEmpArrear.GetRecordByEmployeeId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), str, ref trns);

                            if (dtArrear.Rows.Count > 0)
                            {
                                ObjEmpArrear.updateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), dtArrear.Rows[0]["Trans_Id"].ToString(), dtArrear.Rows[0]["Emp_Id"].ToString(), dtArrear.Rows[0]["From_Date"].ToString(), dtArrear.Rows[0]["To_Date"].ToString(), dtArrear.Rows[0]["Arrear_amount"].ToString(), dtArrear.Rows[0]["Currency_Id"].ToString(), true.ToString(), TransId.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);


                            }


                            //Code End

                            //this code for insert the allowance record from temporary table to permanent table
                            //after that delete from temporary table using empid month and year

                            //code start

                            DataTable dtallowancetemp = new DataTable();
                            dtallowancetemp = objpayrollall.GetPayAllowPayaRoll(str, ref trns);

                            if (dtallowancetemp.Rows.Count > 0)
                            {
                                for (int k = 0; k < dtallowancetemp.Rows.Count; k++)
                                {
                                    objpayrollall.InsertPostPayrollAllowance(str, dtallowancetemp.Rows[k]["Month"].ToString(), dtallowancetemp.Rows[k]["Year"].ToString(), dtemppayrollpost.Rows[0]["Trans_Id"].ToString(), dtallowancetemp.Rows[k]["Allowance_Id"].ToString(), dtallowancetemp.Rows[k]["Allowance_Type"].ToString(), dtallowancetemp.Rows[k]["Allowance_Value"].ToString(), dtallowancetemp.Rows[k]["Act_Allowance_Value"].ToString(), System.DateTime.Now.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    objpayrollall.DeletePayAllowanceTemp_By_EmpId_MonthandYear(str, dtallowancetemp.Rows[k]["Month"].ToString(), dtallowancetemp.Rows[k]["Year"].ToString(), ref trns);
                                }
                            }
                            //code end

                            //this code for insert the deduction record from temporary table to permanent table
                            //after that delete from temporary table using empid month and year

                            //code start

                            DataTable dtdeductiontemp = new DataTable();
                            dtdeductiontemp = objpayrolldeduc.GetPayDeducPayaRoll(str, ref trns);

                            if (dtdeductiontemp.Rows.Count > 0)
                            {
                                for (int k1 = 0; k1 < dtdeductiontemp.Rows.Count; k1++)
                                {
                                    objpayrolldeduc.InsertPostPayrollDeduction(str, dtdeductiontemp.Rows[k1]["Month"].ToString(), dtdeductiontemp.Rows[k1]["Year"].ToString(), dtemppayrollpost.Rows[0]["Trans_Id"].ToString(), dtdeductiontemp.Rows[k1]["Deduction_Id"].ToString(), dtdeductiontemp.Rows[k1]["Deduction_Type"].ToString(), dtdeductiontemp.Rows[k1]["Deduction_Value"].ToString(), dtdeductiontemp.Rows[k1]["Act_Deduction_Value"].ToString(), System.DateTime.Now.ToString(), dtdeductiontemp.Rows[k1]["applicable_amount"].ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    objpayrolldeduc.DeletePayDeductionTemp_By_EmpId_MonthandYear(str, dtdeductiontemp.Rows[k1]["Month"].ToString(), dtdeductiontemp.Rows[k1]["Year"].ToString(), ref trns);
                                }
                            }
                            //code end

                            //this code for delete the record from pay_employee_month_temp table using employee id and month and year
                            //objPayEmpMonth.DeleteEmpMonthTemp_By_EmpId_MonthandYear(Session["CompId"].ToString(), str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns);
                        }
                        else
                        {
                            // DisplayMessage("You can not payroll posted");
                        }
                    }
                }

                //for Account Add

                //On 07-09-2015
                //Add Code for Voucher Entry for Paid Amount           

                //For Bank Account
                string strAccountId = string.Empty;
                DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", ref trns);
                if (dtAccount.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAccount.Rows.Count; i++)
                    {
                        if (strAccountId == "")
                        {
                            strAccountId = dtAccount.Rows[i]["Param_Value"].ToString();
                        }
                        else
                        {
                            strAccountId = strAccountId + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                        }
                    }
                }
                else
                {
                    strAccountId = "0";
                }




                //for Voucher Number
                string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                if (strVoucherNumber != "")
                {
                    DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
                    if (dtCount.Rows.Count > 0)
                    {
                        dtCount = new DataView(dtCount, "Voucher_Type='JV'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    if (dtCount.Rows.Count == 0)
                    {
                        strVoucherNumber = strVoucherNumber + "1";
                    }
                    else
                    {
                        double TotalCount = Convert.ToDouble(dtCount.Rows.Count) + 1;
                        strVoucherNumber = strVoucherNumber + TotalCount;
                    }
                }

                int VMaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", TransId.ToString(), "Pay_Employe_Month", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Payroll for the month " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.Trim() + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "Payroll posted for the month " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.Trim() + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strVMaxId = VMaxId.ToString();
                string strLocationCurrencyId = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
                string strLocationCurrencyValue = GetCurrency(strLocationCurrencyId, TotalAmountPay.ToString());
                string TotalAmountLocation = TotalAmountPay.ToString();
                //For Debit
                string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), TotalAmountPay.ToString());
                string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                if (strAccountId.Split(',').Contains(txtDebitAccount.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtDebitAccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", TotalAmountLocation.ToString(), "0.00", "Payroll for '" + StrPostedEmployee + "' for the month : " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.Trim() + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtDebitAccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", TotalAmountLocation.ToString(), "0.00", "Payroll for '" + StrPostedEmployee + "' for the month : " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.Trim() + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //For Credit
                if (EmpDetails.Length > 0)
                {
                    foreach (string EmpStr in EmpDetails)
                    {
                        string Emp_Id = EmpStr.Split(',')[0].ToString();
                        string Emp_Sal = EmpStr.Split(',')[1].ToString();
                        string Voucher_Narration = EmpStr.Split(',')[2].ToString();
                        string EmployeeAccountName = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account", ref trns).Rows[0]["Param_Value"].ToString());
                        string EmployeeAccountId = EmployeeAccountName.Split('/')[1].ToString();
                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), TotalAmountPay.ToString());
                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                        if (strAccountId.Split(',').Contains(txtCreditAccount.Text.Split('/')[1].ToString()))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtCreditAccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", TotalAmountLocation.ToString(), Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", EmployeeAccountId, Emp_Id, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Emp_Sal, Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        //updated on 21/11/2017
                        //updating finance voucher id in pay employee month table in case of rollback we can delete finance voucher
                        da.execute_Command("update pay_employe_month set Field10='" + strVMaxId + "'   where emp_id='" + Emp_Id + "' and month='" + ddlMonth.SelectedValue.Trim() + "' and year='" + TxtYear.Text.Trim() + "'", ref trns);

                    }
                }

                // if Radio button Employee is Select then
                Create_Loan_Voucher(strFinalPayEmp, Dt_Decimal_Count, strCurrencyId, strVMaxId.ToString(), TransId, trns);

            }

            if (StrPostedEmployee != "")
            {

                DisplayMessage("Payroll Posted Successfully For Employee Code = " + StrPostedEmployee);
                Session["TerminateEmpId"] = null;
                Session["TerminationDate"] = null;
                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                Set_Default_Credit_Account();
            }
            else
            {
                DisplayMessage("Payroll not generated");
                Session["TerminateEmpId"] = null;
                Session["TerminationDate"] = null;
                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                return;
            }

            btnPayroll_Click(null, null);
            txtValue1.Text = "";
        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }
    }

    public void Create_Loan_Voucher(string strFinalPayEmp, DataTable Dt_Decimal_Count, string strCurrencyId, string Salary_Header_ID, int Insert_posted_Pay_Emp_Month_Trans_ID, SqlTransaction trns)
    {
        string Paid_Loan_Amount = string.Empty;
        DataTable dtEmpPay = GetTable();
        double ActualGross_Loan = 0;
        string StrPostedEmployee_Loan = string.Empty;
        string[] EmpDetails_Loan = new string[1];
        int EmpCount_Loan = -1;
        double TotalAmountPay_Loan = 0;
        foreach (string str in strFinalPayEmp.Split(','))
        {
            var arr = strFinalPayEmp.Split(',');
            if (EmpCount_Loan == -1)
                EmpDetails_Loan = new string[arr.Length - 1];
            if ((str != ""))
            {
                EmpCount_Loan++;
                DataTable dtempmonthtemp = new DataTable();
                dtempmonthtemp = objPayEmpMonth.GetallTemprecords_By_EmployeeId(str, ref trns,Session["CompId"].ToString());
                dtempmonthtemp = new DataView(dtempmonthtemp, "Emp_Id=" + str + " ", "", DataViewRowState.CurrentRows).ToTable();

                if (dtempmonthtemp.Rows.Count > 0)
                {
                    string strEmpCodeOnly = GetEmployeeCode(str);
                    StrPostedEmployee_Loan += GetEmployeeCode(str) + ",";
                    DataTable Dtloan = new DataTable();
                    Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved", ref trns);
                    string Narration = string.Empty;
                    DataTable dtemppayrollpost = new DataTable();
                    dtemppayrollpost = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns, Session["CompId"].ToString());
                    string strEmployeeActualSalary = string.Empty;
                    if (dtemppayrollpost.Rows.Count > 0)
                    {
                        strEmployeeActualSalary = string.Empty;
                        string WorkedSalary = dtemppayrollpost.Rows[0]["Worked_Min_Salary"].ToString();
                        string NormalOT = dtemppayrollpost.Rows[0]["Normal_OT_Min_Salary"].ToString();
                        string WeekOffOT = dtemppayrollpost.Rows[0]["Week_Off_OT_Min_Salary"].ToString();
                        string HolidayOT = dtemppayrollpost.Rows[0]["Holiday_OT_Min_Salary"].ToString();
                        string LeaveDaysSalary = dtemppayrollpost.Rows[0]["Leave_Days_Salary"].ToString();
                        string WeekOffSalary = dtemppayrollpost.Rows[0]["Week_Off_Salary"].ToString();
                        string HolidaySalary = dtemppayrollpost.Rows[0]["Holidays_Salary"].ToString();
                        string AbsentSalary = dtemppayrollpost.Rows[0]["Absent_Salary"].ToString();
                        string LateMinPenalty = dtemppayrollpost.Rows[0]["Late_Min_Penalty"].ToString();
                        string EarlyMinPenalty = dtemppayrollpost.Rows[0]["Early_Min_Penalty"].ToString();
                        string PartialViolationPenalty = dtemppayrollpost.Rows[0]["Patial_Violation_Penalty"].ToString();
                        string EmpPenalty = dtemppayrollpost.Rows[0]["Employee_Penalty"].ToString();
                        string EmpClaim = dtemppayrollpost.Rows[0]["Employee_Claim"].ToString();
                        string Employee_Loan = dtemppayrollpost.Rows[0]["Emlployee_Loan"].ToString();
                        string TotalAllowance = dtemppayrollpost.Rows[0]["Total_Allowance"].ToString();
                        string TotalDeduction = dtemppayrollpost.Rows[0]["Total_Deduction"].ToString();
                        string PreviousMonthAdjust = dtemppayrollpost.Rows[0]["Previous_Month_Balance"].ToString();
                        string EmployeePF = dtemppayrollpost.Rows[0]["Employee_PF"].ToString();
                        string EmployeeESIC = dtemppayrollpost.Rows[0]["Employee_ESIC"].ToString();
                        ActualGross_Loan = (float.Parse(WorkedSalary) + float.Parse(LeaveDaysSalary) + float.Parse(WeekOffSalary) + float.Parse(HolidaySalary) + float.Parse(TotalAllowance));
                        string strAddition = (float.Parse(WorkedSalary) + float.Parse(NormalOT) + float.Parse(WeekOffOT) + float.Parse(HolidayOT) + float.Parse(LeaveDaysSalary) + float.Parse(WeekOffSalary) + float.Parse(HolidaySalary) + float.Parse(EmpClaim) + float.Parse(TotalAllowance) + float.Parse(PreviousMonthAdjust)).ToString();
                        string strDeduction = (float.Parse(AbsentSalary) + float.Parse(LateMinPenalty) + float.Parse(EarlyMinPenalty) + float.Parse(PartialViolationPenalty) + float.Parse(EmpPenalty) + float.Parse(TotalDeduction)).ToString();
                        double sumloan = 0;
                        //DataTable Dtloan_Str = new DataView(Dtloan, " Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                        //for (int i = 0; i < Dtloan_Str.Rows.Count; i++)
                        //{
                        //    string strLoandetailId = string.Empty;
                        //    DataTable dtloandetial = new DataTable();
                        //    dtloandetial = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan_Str.Rows[i]["Loan_Id"].ToString(), ref trns);
                        //    dtloandetial = new DataView(dtloandetial, "Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + " and Is_Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                        //    if (dtloandetial.Rows.Count > 0)
                        //    {
                        //        double loan = 0;
                        //        DataRow row = dtEmpPay.NewRow();
                        //        if (dtloandetial.Rows[0]["Total_Amount"].ToString() != "")
                        //        {
                        //            row[6] = dtloandetial.Rows[0]["Total_Amount"].ToString();
                        //            loan = Convert.ToDouble(dtloandetial.Rows[0]["Total_Amount"]);
                        //            sumloan += loan;
                        //        }
                        //        strLoandetailId = dtloandetial.Rows[0]["Trans_Id"].ToString();
                        //        strEmployeeActualSalary = sumloan.ToString();
                        //        TotalAmountPay_Loan += Convert.ToDouble(sumloan);
                        //    }
                        //}
                        Paid_Loan_Amount = Employee_Loan;
                        sumloan = Convert.ToDouble(Employee_Loan);
                        strEmployeeActualSalary = sumloan.ToString();
                        TotalAmountPay_Loan = sumloan;
                        if (sumloan != 0)
                        {
                            if (Convert.ToDouble(strAddition) >= sumloan)
                            {
                                Narration = "Loan installment deduction for the Month : " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.ToString() + " (Loan Amount = " + (Convert.ToDouble(sumloan)).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString()) + ")";
                                string EmpDetail = str + "," + strEmployeeActualSalary.ToString() + "," + Narration;
                                EmpDetails_Loan[EmpCount_Loan] = EmpDetail;
                            }
                        }
                    }
                    double GrossSalary = 0;
                    objPayEmpMonth.DeleteEmpMonthTemp_By_EmpId_MonthandYear(Session["CompId"].ToString(), str, dtempmonthtemp.Rows[0]["Month"].ToString(), dtempmonthtemp.Rows[0]["Year"].ToString(), ref trns);
                }
                else
                {

                }
            }
        }

        string strAccountId_Loan = string.Empty;
        DataTable dtAccount_Loan = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", ref trns);
        if (dtAccount_Loan.Rows.Count > 0)
        {
            for (int i = 0; i < dtAccount_Loan.Rows.Count; i++)
            {
                if (strAccountId_Loan == "")
                {
                    strAccountId_Loan = dtAccount_Loan.Rows[i]["Param_Value"].ToString();
                }
                else
                {
                    strAccountId_Loan = strAccountId_Loan + "," + dtAccount_Loan.Rows[i]["Param_Value"].ToString();
                }
            }
        }
        else
        {
            strAccountId_Loan = "0";
        }

        if (TotalAmountPay_Loan != 0)
        {
            string strVoucherNumber_Loan = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            if (strVoucherNumber_Loan != "")
            {
                DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ref trns);
                if (dtCount.Rows.Count > 0)
                {
                    dtCount = new DataView(dtCount, "Voucher_Type='JV'", "", DataViewRowState.CurrentRows).ToTable();
                }
                if (dtCount.Rows.Count == 0)
                {
                    strVoucherNumber_Loan = strVoucherNumber_Loan + "1";
                }
                else
                {
                    double TotalCount = Convert.ToDouble(dtCount.Rows.Count) + 1;
                    strVoucherNumber_Loan = strVoucherNumber_Loan + TotalCount;
                }
            }

            if (EmpDetails_Loan.Length > 0)
            {
                EmpDetails_Loan = EmpDetails_Loan.Where(str => !string.IsNullOrEmpty(str)).ToArray();
                List<string> Temp_List_Emp = new List<string>();
                foreach (string str in EmpDetails_Loan)
                {
                    if (!string.IsNullOrEmpty(str))
                        Temp_List_Emp.Add(str);
                }
                EmpDetails_Loan = Temp_List_Emp.ToArray();
            }

            if (EmpDetails_Loan.Length > 0)
            {
                btnsaveloan_Click(EmpDetails_Loan, Paid_Loan_Amount, "", ref trns);

                int VMaxId_Loan = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", Insert_posted_Pay_Emp_Month_Trans_ID.ToString(), "Loan_Deduction", "0", DateTime.Now.ToString(), strVoucherNumber_Loan, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Loan Deduction for the month : " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.Trim() + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", strCurrencyId, "0.00", "Loan installment for the month : " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.Trim() + "", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", Salary_Header_ID.ToString(), "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strVMaxId_Loan = VMaxId_Loan.ToString();
                string strLocationCurrencyId_Loan = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
                string strLocationCurrencyValue_Loan = GetCurrency(strLocationCurrencyId_Loan, TotalAmountPay_Loan.ToString());
                string TotalAmountLocation_Loan = TotalAmountPay_Loan.ToString();


                //For Credit


                foreach (string EmpStr in EmpDetails_Loan)
                {

                    string Emp_Id = EmpStr.Split(',')[0].ToString();
                    string Emp_Loan = EmpStr.Split(',')[1].ToString();
                    //For Debit
                    if (Emp_Loan.ToString() != "" && Emp_Loan.ToString() != "0")
                    {
                        string strCompanyCrrValueDr_Loan = GetCurrency(Session["CurrencyId"].ToString(), TotalAmountPay_Loan.ToString());
                        string CompanyCurrDebit_Loan = strCompanyCrrValueDr_Loan.Trim().Split('/')[0].ToString();
                        if (strAccountId_Loan.Split(',').Contains(txtDebitAccount.Text.Split('/')[1].ToString()))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId_Loan, "1", "143", "0", "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Emp_Loan, "Loan installment deposited for the Month : " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.Trim() + " On Location '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit_Loan, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId_Loan, "1", "143", Emp_Id, "0", "PP", "1/1/1800", "1/1/1800", "", "0.00", Emp_Loan, "Loan installment deposited for the Month : " + ddlMonth.SelectedItem.Text.ToString() + " - " + TxtYear.Text.Trim() + " On Location'" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", CompanyCurrDebit_Loan, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }


                        string Voucher_Narration = EmpStr.Split(',')[2].ToString();
                        string EmployeeAccountName = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account", ref trns).Rows[0]["Param_Value"].ToString());
                        string EmployeeAccountId = EmployeeAccountName.Split('/')[1].ToString();
                        string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), TotalAmountPay_Loan.ToString());
                        string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                        if (strAccountId_Loan.Split(',').Contains(txtCreditAccount.Text.Split('/')[1].ToString()))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId_Loan, "1", txtCreditAccount.Text.Split('/')[1].ToString(), "0", "0", "PP", "1/1/1800", "1/1/1800", "", Emp_Loan, "0.00", Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId_Loan, "1", EmployeeAccountId, Emp_Id, "0", "PP", "1/1/1800", "1/1/1800", "", Emp_Loan, "0.00", Voucher_Narration, "", Session["EmpId"].ToString(), strCurrencyId, "0.00", "0.00", "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }

                foreach (string EmpStr in EmpDetails_Loan)
                {
                    string Emp_Id = EmpStr.Split(',')[0].ToString();
                    //    string Emp_Loan = EmpStr.Split(',')[1].ToString();
                    //    double sumloan = 0;
                    //    DataTable Dtloan = new DataTable();
                    //    Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved", ref trns);
                    //    DataTable Dtloan_Str = new DataView(Dtloan, " Emp_Id=" + Emp_Id + "", "", DataViewRowState.CurrentRows).ToTable();
                    //    for (int i = 0; i < Dtloan_Str.Rows.Count; i++)
                    //    {
                    //        string strLoandetailId = string.Empty;
                    //        DataTable dtloandetial = new DataTable();
                    //        dtloandetial = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan_Str.Rows[i]["Loan_Id"].ToString(), ref trns);
                    //        dtloandetial = new DataView(dtloandetial, "Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + " and Is_Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                    //        if (dtloandetial.Rows.Count > 0)
                    //        {
                    //            double loan = 0;
                    //            DataRow row = dtEmpPay.NewRow();
                    //            if (dtloandetial.Rows[0]["Total_Amount"].ToString() != "")
                    //            {
                    //                row[6] = dtloandetial.Rows[0]["Total_Amount"].ToString();
                    //                loan = Convert.ToDouble(dtloandetial.Rows[0]["Total_Amount"]);
                    //                sumloan += loan;
                    //            }
                    //            strLoandetailId = dtloandetial.Rows[0]["Trans_Id"].ToString();
                    //            DataAccessClass objDA = new DataAccessClass();
                    //            objEmpLoan.UpdateRecord_loandetials_WithPaidStatusandAmount(dtloandetial.Rows[0]["Loan_Id"].ToString(), dtloandetial.Rows[0]["Trans_Id"].ToString(), loan.ToString(), "Paid", ref trns);
                    //            objDA.execute_Command("update Pay_Employee_Loan_Detail set Previous_Balance='0',Total_Amount=Montly_Installment,Employee_Paid='0',Is_Status='Pending' where Loan_Id=" + dtloandetial.Rows[0]["Loan_Id"].ToString() + " and Trans_Id>" + strLoandetailId + "  and Is_Status='Pending'", ref trns);
                    //        }
                    //    }
                    UpdateEmployeeLoanVoucherId(Emp_Id, strVMaxId_Loan, trns);
                }


            }
        }
    }

    protected void btnsaveloan_Click(string[] EmpDetails_Loan, string Paid_Loan_Amount, string Voucher_ID, ref SqlTransaction trns)
    {
        double netamt = 0;
        double pvbal = 0;
        double instllamt = 0;
        double totalamt = 0;
        double txtamount = 0;
        double currentLoanAmt = 0;
        foreach (string EmpStr in EmpDetails_Loan)
        {
            string Emp_Id = EmpStr.Split(',')[0].ToString();
            string Emp_Loan = EmpStr.Split(',')[1].ToString();
            double sumloan = 0;
            DataTable Dtloan = new DataTable();
            Dtloan = objEmpLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved", ref trns);
            DataTable Dtloan_Str = new DataView(Dtloan, " Emp_Id=" + Emp_Id + "", "", DataViewRowState.CurrentRows).ToTable();
            for (int i = 0; i < Dtloan_Str.Rows.Count; i++)
            {
                string strLoandetailId = string.Empty;
                DataTable dtloandetial = new DataTable();
                dtloandetial = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(Dtloan_Str.Rows[i]["Loan_Id"].ToString(), ref trns);
                dtloandetial = new DataView(dtloandetial, "Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + " and Is_Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                //dtloandetial = new DataView(dtloandetial, "Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + " and Is_Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtloandetial.Rows.Count > 0)
                {
                    double txtamt = Convert.ToDouble(Paid_Loan_Amount);
                    DataTable dtempedit = new DataTable();
                    dtempedit = objPayEmpMonth.GetPayEmpMonthTemp_By_EmployeeId(Emp_Id, ref trns, Session["CompId"].ToString());
                    int counter = 0;
                    instllamt = Convert.ToDouble(dtloandetial.Rows[0]["Montly_Installment"].ToString());
                    txtamount = txtamt;
                    //txtamount = Convert.ToDouble(dtloandetial.Rows[0]["Montly_Installment"].ToString());
                    currentLoanAmt = Convert.ToDouble(dtloandetial.Rows[0]["Total_Amount"].ToString());
                    int trnsid = 0;
                    trnsid = Convert.ToInt32(dtloandetial.Rows[0]["Trans_Id"].ToString());
                    if (currentLoanAmt != txtamount)
                    {
                        DataTable dtlndedetials = new DataTable();
                        dtlndedetials = objEmpLoan.GetRecord_From_PayEmployeeLoanDetailAll(ref trns);
                        dtlndedetials = new DataView(dtlndedetials, "Loan_Id=" + dtloandetial.Rows[0]["Loan_Id"].ToString() + " and Trans_Id > " + (trnsid) + " ", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtlndedetials.Rows.Count > 0)
                        {
                            if (currentLoanAmt > txtamount)
                            {
                                pvbal = currentLoanAmt - txtamount;
                                totalamt = instllamt + pvbal;
                                objEmpLoan.UpdateRecord_loandetials_Amt(Session["CompId"].ToString(), dtloandetial.Rows[0]["Loan_Id"].ToString().ToString(), (trnsid + 1).ToString(), pvbal.ToString(), totalamt.ToString(), "0");
                            }
                            else if (txtamount > currentLoanAmt)
                            {
                                double TotalPendingamount = currentLoanAmt;
                                foreach (DataRow dr in dtlndedetials.Rows)
                                {
                                    TotalPendingamount += Convert.ToDouble(dr["total_amount"].ToString());
                                }
                                if (txtamount > TotalPendingamount)
                                {
                                    DisplayMessage("Paid amount should not be greater then loan due amount");
                                    return;
                                }
                                double PreviousBalance = 0;
                                pvbal = txtamount - currentLoanAmt;
                                PreviousBalance = pvbal;
                                double currentloaninstallment = 0;
                                string strStatus = string.Empty;
                                foreach (DataRow dr in dtlndedetials.Rows)
                                {
                                    if (pvbal <= 0)
                                    {
                                        break;
                                    }
                                    currentLoanAmt = Convert.ToDouble(dr["Montly_installment"].ToString());
                                    if (PreviousBalance > currentLoanAmt)
                                    {
                                        currentloaninstallment = 0;
                                        PreviousBalance = (currentLoanAmt);
                                    }
                                    else
                                    {
                                        currentloaninstallment = currentLoanAmt - PreviousBalance;
                                    }
                                    objEmpLoan.UpdateRecord_loandetials_Amt(Session["CompId"].ToString(), dtloandetial.Rows[0]["Loan_Id"].ToString().ToString(), dr["Trans_Id"].ToString(), (0 - PreviousBalance).ToString(), currentloaninstallment.ToString(), "0", ref trns);
                                    pvbal = pvbal - PreviousBalance;
                                    if (currentloaninstallment <= 0 || currentloaninstallment <= 1)
                                    {
                                        da.execute_Command("update pay_employee_loan_detail set is_status = 'Paid'  where Trans_id = " + dr["Trans_Id"].ToString() + "", ref trns);
                                    }
                                    PreviousBalance = pvbal;
                                }
                            }
                        }
                        else
                        {
                            counter = 1;
                        }
                    }

                    string strstatus = string.Empty;
                    if (counter == 1)
                    {
                        DisplayMessage("You Can Not Adjust The Amount");
                        return;
                    }
                    else
                    {
                        if (txtamt > 0)
                        {
                            strstatus = "Paid";
                        }
                        else
                        {
                            strstatus = "Pending";
                        }
                        objEmpLoan.UpdateRecord_loandetials_WithPaidStatusandAmount(dtloandetial.Rows[0]["Loan_Id"].ToString().ToString(), trnsid.ToString(), txtamt.ToString(), strstatus, ref trns);
                    }
                    if (txtamt != 0)
                    {
                        netamt += txtamt;
                    }
                }
                DataTable dtPay = new DataTable();
                dtPay = objPayEmpMonth.GetRecordByEmpIdMonthYear(Emp_Id, Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), ref trns, Session["CompId"].ToString());
                if (dtPay.Rows.Count > 0)
                {
                    objPayEmpMonth.UpdateRecord_Pay_Employee_Month(Session["CompId"].ToString(), Emp_Id, Session["monthAllow"].ToString(), Session["YearAllow"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), netamt.ToString(), dtPay.Rows[0]["Total_Allowance"].ToString(), dtPay.Rows[0]["Total_Deduction"].ToString(), dtPay.Rows[0]["Employee_Penalty"].ToString(), dtPay.Rows[0]["Employee_Claim"].ToString(), dtPay.Rows[0]["Employee_PF"].ToString(), dtPay.Rows[0]["Employer_PF"].ToString(), dtPay.Rows[0]["Employee_ESIC"].ToString(), dtPay.Rows[0]["Employer_ESIC"].ToString(), dtPay.Rows[0]["Field3"].ToString(), dtPay.Rows[0]["Field4"].ToString(), dtPay.Rows[0]["Field5"].ToString(), dtPay.Rows[0]["Field6"].ToString(), ref trns);
                }
            }
            //UpdateEmployeeLoanVoucherId(Emp_Id, Voucher_ID, trns);
        }
    }

    public string CheckPayrollStatus(string stremp)
    {
        string strEmpList = string.Empty;

        foreach (string str in stremp.Split(','))
        {
            if (str == "")
            {
                continue;
            }
            if (da.return_DataTable("select Pay_Employe_Month_Temp.Emp_Id from Pay_Employe_Month_Temp where Emp_Id=" + str + " and MONTH='" + ddlMonth.SelectedValue + "' and YEAR='" + TxtYear.Text.Trim() + "'").Rows.Count == 0)
            {
                strEmpList += Common.GetEmployeeCode(str, Session["DBConnection"].ToString(), Session["CompId"].ToString()) + ",";
            }
        }



        return strEmpList;

    }

    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id=" + strAccountNo + " and IsActive='True'";

            DataTable dtAccName = da.return_DataTable(sql);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString() + "/" + strAccountNo;
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }

    public void UpdateEmployeeGrossSalary(string strEmpId, string strGrossSalary, string strVoucherNo, SqlTransaction trns)
    {
        da.execute_Command("update Pay_Employe_Month  set Field8='" + strGrossSalary + "',Voucher_No=" + strVoucherNo + " where Emp_Id='" + strEmpId + "' and MONTH='" + ddlMonth.SelectedValue + "' and YEAR='" + TxtYear.Text + "'", ref trns);
    }

    public void UpdateEmployeeLoanVoucherId(string strEmpId, string strVoucherNo, SqlTransaction trns)
    {
        da.execute_Command("update Pay_Employe_Month  set LOan_Voucher_No=" + strVoucherNo + " where Emp_Id='" + strEmpId + "' and MONTH='" + ddlMonth.SelectedValue + "' and YEAR='" + TxtYear.Text + "'", ref trns);
    }
    private string GetLocationCode(string strLocationId)
    {
        string strLocationCode = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocation = ObjLocationMaster.GetLocationMasterByLocationId(strLocationId);
            if (dtLocation.Rows.Count > 0)
            {
                strLocationCode = dtLocation.Rows[0]["Location_Code"].ToString();
            }
        }
        return strLocationCode;
    }
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = Session["CurrencyId"].ToString();
        //ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();


        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = objSys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }

    public string GetEmployeeName(string strEmpCode)
    {
        string strEmpName = string.Empty;
        DataTable dtEmp = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), strEmpCode);
        if (dtEmp.Rows.Count > 0)
        {
            strEmpName = dtEmp.Rows[0]["Emp_Name"].ToString();
        }
        else
        {
            strEmpName = "";
        }
        return strEmpName;
    }

    public string GetDocumentNumberVoucher()
    {
        string DocumentNo = string.Empty;

        DataTable Dt = objDocNo.GetDocumentNumberAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["Prefix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Prefix"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["CompId"].ToString()))
            {
                DocumentNo += Session["CompId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["BrandId"].ToString()))
            {
                DocumentNo += Session["BrandId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["LocationId"].ToString()))
            {
                DocumentNo += Session["LocId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["DeptId"].ToString()))
            {
                DocumentNo += (string)Session["SessionDepId"];
            }

            if (Convert.ToBoolean(Dt.Rows[0]["EmpId"].ToString()))
            {
                DataTable Dtuser = ObjUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["LoginCompany"].ToString());
                DocumentNo += Dtuser.Rows[0]["Emp_Id"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Year"].ToString()))
            {
                DocumentNo += DateTime.Now.Year.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Month"].ToString()))
            {
                DocumentNo += DateTime.Now.Month.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Day"].ToString()))
            {
                DocumentNo += DateTime.Now.Day.ToString();
            }

            if (Dt.Rows[0]["Suffix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Suffix"].ToString();
            }

            if (DocumentNo != "")
            {
                DocumentNo += "-" + (Convert.ToInt32(objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString()).Rows.Count) + 1).ToString();
            }
            else
            {
                DocumentNo += (Convert.ToInt32(objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString()).Rows.Count) + 1).ToString();
            }
        }
        else
        {
            DocumentNo += (Convert.ToInt32(objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString()).Rows.Count) + 1).ToString();
        }
        return DocumentNo;
    }
    protected void btnpayrollPostedReport_Click(object sender, EventArgs e)
    {


        string Selected_Emp = string.Empty;

        if (rbtnGroup.Checked == true)
        {
            foreach (GridViewRow GVR in gvEmployee.Rows)
            {
                Label lblEmpId = (Label)GVR.FindControl("lblEmpId");
                Selected_Emp = lblEmpId.Text + "," + Selected_Emp;
            }
        }
        else if (rbtnEmp.Checked == true)
        {
            foreach (GridViewRow GVR in gvEmpPayroll.Rows)
            {
                CheckBox Chk_Box = (CheckBox)GVR.FindControl("chkgvSelect");
                Label lblEmpId = (Label)GVR.FindControl("lblEmpId");
                if (Chk_Box.Checked == true)
                {
                    Selected_Emp = lblEmpId.Text + "," + Selected_Emp;
                }
            }
        }


        lblPostFirst.Text = "";
        lblAlreadyPosted.Text = "";
        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";
        DataAccessClass objDA = new DataAccessClass(Session["DBConnection"].ToString());
        DataTable DtpostedPayroll = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());
        if (ddlLocation.SelectedIndex == 0 || rbtnGroup.Checked == true)
        {
            Session["PostedLocationId"] = Session["LocId"].ToString();
        }
        else
        {
            Session["PostedLocationId"] = ddlLocation.SelectedValue;
            
        }

        DataTable DtReport = new DataTable();
        DtReport.Columns.Add("EmpCode");
        DtReport.Columns.Add("EmployeeName");
        DtReport.Columns.Add("EmailId");
        DtReport.Columns.Add("Month");
        DtReport.Columns.Add("Year");
        DtReport.Columns.Add("Department_Name");
        DtReport.Columns.Add("Emp_Id");

        for (int i = 0; i < DtpostedPayroll.Rows.Count; i++)
        {
            DataTable dtPayPostedInfo = objDA.return_DataTable("select * from Pay_Employe_Month  where Emp_Id = '" + DtpostedPayroll.Rows[i]["Emp_Id"].ToString() + "' and Year = (Select MAX(Year) From Pay_Employe_Month  where Emp_Id = '" + DtpostedPayroll.Rows[i]["Emp_Id"].ToString() + "') order by MONTH desc");
            if (dtPayPostedInfo.Rows.Count > 0)
            {
                DataRow Dr = DtReport.NewRow();
                Dr[0] = DtpostedPayroll.Rows[i]["Emp_Code"].ToString();
                Dr[1] = DtpostedPayroll.Rows[i]["Emp_Name"].ToString();
                Dr[2] = DtpostedPayroll.Rows[i]["Email_Id"].ToString();
                Dr[3] = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(dtPayPostedInfo.Rows[0]["Month"].ToString()));
                Dr[4] = dtPayPostedInfo.Rows[0]["Year"].ToString();
                Dr[5] = DtpostedPayroll.Rows[i]["Department"].ToString();
                Dr[6] = DtpostedPayroll.Rows[i]["Emp_Id"].ToString();
                DtReport.Rows.Add(Dr);
            }
        }

        if (DtReport.Rows.Count > 0)
        {
            Session["PostedEmployee"] = DtReport;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/EmployeePayrollPosted.aspx')", true);

        }
        else
        {
            DisplayMessage("Record Not Found");
        }

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);




        string filtertext = "AccountName like '%" + prefixText + "%'";
        dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }

        return txt;
    }
    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True'";
                DataTable dtCOA = da.return_DataTable(sql);

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose Account in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {
                DisplayMessage("Choose Account in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }
    }
    public DataTable GetTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("EmpId"));
        dt.Columns.Add(new DataColumn("EmpName"));
        dt.Columns.Add(new DataColumn("Month"));
        dt.Columns.Add(new DataColumn("Year"));
        dt.Columns.Add(new DataColumn("Type"));
        dt.Columns.Add(new DataColumn("RefId"));
        dt.Columns.Add(new DataColumn("ValueType"));
        dt.Columns.Add(new DataColumn("Value"));

        return dt;
    }
    public DataTable GetEmpAllowDedu(int EmpId)
    {

        DataTable DtAllDeduc = new DataTable();

        // Get Record all Alllowance & Deduction of this Employee

        DataTable dtEmp = new DataTable();
        dtEmp = ObjAllDeduc.GetPayAllowDeducByEmpId(Session["CompId"].ToString(), EmpId.ToString());

        return dtEmp;
    }
    public string DateFormat(string Date)
    {
        string SystemDate = string.Empty;

        try
        {
            SystemDate = Convert.ToDateTime(Date).ToString(objSys.SetDateFormat());
        }
        catch
        {
            SystemDate = Date;
        }

        return SystemDate;
    }

    public double GetMobileExceedAmount(string strEmpId, string strMobileNumber, string strMobileLimit)
    {
        double ExceedAmount = 0;
        double MobileLimit = 0;
        double MobileBill = 0;
        Pay_MobileBillPayment ObjMobilePayment = new Pay_MobileBillPayment(Session["DBConnection"].ToString());


        DataTable dt = ObjMobilePayment.GetRecordByMobileNumber(Session["CompId"].ToString(), strMobileNumber);

        dt = new DataView(dt, "Month='" + ddlMonth.SelectedValue + "' and year='" + TxtYear.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            try
            {
                MobileBill = Convert.ToDouble(dt.Rows[0]["Bill_Amount"].ToString());
            }
            catch
            {

            }

            try
            {
                MobileLimit = Convert.ToDouble(strMobileLimit);
            }
            catch
            {

            }

            if (MobileBill > MobileLimit)
            {
                ExceedAmount = MobileBill - MobileLimit;


                ObjMobilePayment.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), strEmpId, dt.Rows[0]["Mobile_Number"].ToString(), dt.Rows[0]["Month"].ToString(), dt.Rows[0]["Year"].ToString(), dt.Rows[0]["Bill_Amount"].ToString(), MobileLimit.ToString(), ExceedAmount.ToString(), dt.Rows[0]["Ref_Id"].ToString(), dt.Rows[0]["Operator"].ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), false.ToString());

            }

        }


        return ExceedAmount;
    }



    public void UpdateMobileAdjustedFlag(string strEmpId, SqlTransaction trans)
    {
        Pay_MobileBillPayment ObjMobilePayment = new Pay_MobileBillPayment(Session["DBConnection"].ToString());

        DataTable dt = ObjMobilePayment.GetRecordByEmployeeId(Session["CompId"].ToString(), strEmpId, ref trans);

        dt = new DataView(dt, "Month='" + ddlMonth.SelectedValue + "' and year='" + TxtYear.Text + "' and Exceed_Amount>0", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            ObjMobilePayment.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), strEmpId, dt.Rows[0]["Mobile_Number"].ToString(), dt.Rows[0]["Month"].ToString(), dt.Rows[0]["Year"].ToString(), dt.Rows[0]["Bill_Amount"].ToString(), dt.Rows[0]["Bill_Limit"].ToString(), dt.Rows[0]["Exceed_Amount"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), dt.Rows[0]["Operator"].ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), true.ToString(), ref trans);
        }
    }
    public void DeletePenaltyByEmployeeIdandPenaltyName(string strEmpId, string strPenaltyname)
    {

        DataTable dtPenalty = objPEpenalty.GetRecord_From_PayEmployeePenalty_By_EmpId_MonthandYear(Session["CompId"].ToString(), strEmpId, "", ddlMonth.SelectedIndex.ToString(), TxtYear.Text, "", "");
        for (int i = 0; i < dtPenalty.Rows.Count; i++)
        {


            if (dtPenalty.Rows[i]["Penalty_Name"].ToString().Trim() == strPenaltyname)
            {
                objPEpenalty.DeleteRecord_in_Pay_Employee_penalty(Session["CompId"].ToString(), dtPenalty.Rows[i]["Penalty_Id"].ToString(), "False", Session["UserId"].ToString(), DateTime.Now.ToString());
                break;
            }

        }
    }


    protected void Btn_List_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    #region Report
    protected void Btn_Report_Click(object sender, EventArgs e)
    {
        FillVoucherReportGrid();

    }
    public void FillVoucherReportGrid()
    {
        DataTable dt = new DataTable();

        if (TxtYear.Text.Trim() == "")
        {
            TxtYear.Text = DateTime.Now.Year.ToString();
        }

        if (ddlMonth.SelectedIndex > 0)
        {

            dt = da.return_DataTable("select pay_employe_month.Voucher_no,pay_employe_month.Month,pay_employe_month.Year,SUM(Worked_Min_Salary+Normal_OT_Min_Salary+Week_Off_OT_Min_Salary+Holiday_OT_Min_Salary+Leave_Days_Salary+Week_Off_Salary+Holidays_Salary-Absent_Salary-Late_Min_Penalty-Early_Min_Penalty-Patial_Violation_Penalty-Employee_Penalty+Employee_Claim-Emlployee_Loan+Total_Allowance-Total_Deduction-Employee_PF-Employee_ESIC+Previous_Month_Balance) as TotalAmount,pay_employe_month.CreatedBy,MAX( REPLACE( CONVERT(VARCHAR(11), pay_employe_month.CreatedDate, 106),' ','-')) as CreatedDate,set_Employeemaster.Location_Id from pay_employe_month inner join set_Employeemaster on pay_employe_month.Emp_Id=set_Employeemaster.Emp_Id where set_Employeemaster.Location_Id=" + Session["LocId"].ToString() + " and pay_employe_month.Voucher_no<>0 and pay_employe_month.Month=" + ddlMonth.SelectedValue + " and pay_employe_month.Year=" + TxtYear.Text + "  Group by pay_employe_month.Voucher_no,pay_employe_month.CreatedBy,pay_employe_month.Month,pay_employe_month.Year,set_Employeemaster.Location_Id");
        }
        else
        {
            dt = da.return_DataTable("select pay_employe_month.Voucher_no,pay_employe_month.Month,pay_employe_month.Year,SUM(Worked_Min_Salary+Normal_OT_Min_Salary+Week_Off_OT_Min_Salary+Holiday_OT_Min_Salary+Leave_Days_Salary+Week_Off_Salary+Holidays_Salary-Absent_Salary-Late_Min_Penalty-Early_Min_Penalty-Patial_Violation_Penalty-Employee_Penalty+Employee_Claim-Emlployee_Loan+Total_Allowance-Total_Deduction-Employee_PF-Employee_ESIC+Previous_Month_Balance) as TotalAmount,pay_employe_month.CreatedBy,MAX( REPLACE( CONVERT(VARCHAR(11), pay_employe_month.CreatedDate, 106),' ','-')) as CreatedDate,set_Employeemaster.Location_Id from pay_employe_month inner join set_Employeemaster on pay_employe_month.Emp_Id=set_Employeemaster.Emp_Id where set_Employeemaster.Location_Id=" + Session["LocId"].ToString() + " and pay_employe_month.Voucher_no<>0  Group by pay_employe_month.Voucher_no,pay_employe_month.CreatedBy,pay_employe_month.Month,pay_employe_month.Year,set_Employeemaster.Location_Id");
        }

        Session["dtEmpPayroll_Report_All"] = dt;

        Session["dtEmpPayroll_Report"] = dt;

        objPageCmn.FillData((GridView)gvEmp_Report, dt, "", "");


        lblTotalRecordsPayrollReport.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

       
    }
    protected void gvEmp_Report_Sorting(object sender, GridViewSortEventArgs e)
    {
        hdFSortgvEmpPayroll.Value = hdFSortgvEmpPayroll.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpPayroll_Report"];
        DataView dv = new DataView(dt);

        string Query = "" + e.SortExpression + " " + hdFSortgvEmpPayroll.Value + "";

        dv.Sort = Query;
        dt = dv.ToTable();

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmp_Report, dt, "", "");
     
        gvEmp_Report.HeaderRow.Focus();
    }
    protected void gvEmp_Report_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvEmp_Report.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtEmpPayroll_Report"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmp_Report, dt, "", "");
       
    }
    protected void btnbindReport_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (Session["dtEmpPayroll_Report_All"] != null)
        {

            string condition = string.Empty;
            if (ddlField1Report.SelectedValue == "Voucher_no")
            {

                if (ddlOption1Report.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlField1Report.SelectedValue + ",System.String)='" + txtValue1Report.Text.Trim() + "'";
                }
                else if (ddlOption1Report.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlField1Report.SelectedValue + ",System.String) like '%" + txtValue1Report.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlField1Report.SelectedValue + ",System.String) Like '" + txtValue1Report.Text.Trim() + "%'";

                }
            }
            else
            {
                if (ddlOption1Report.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlField1Report.SelectedValue + ",System.String)='" + txtValueDate.Text.Trim() + "'";
                }
                else if (ddlOption1Report.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlField1Report.SelectedValue + ",System.String) like '%" + txtValueDate.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlField1Report.SelectedValue + ",System.String) Like '" + txtValueDate.Text.Trim() + "%'";

                }
            }

            DataTable dtEmp = (DataTable)Session["dtEmpPayroll_Report_All"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            DataTable DtRows = view.ToTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmp_Report, view.ToTable(), "", "");
            Session["dtEmpPayroll_Report"] = view.ToTable();
            lblTotalRecordsPayrollReport.Text = Resources.Attendance.Total_Records + " : " + DtRows.Rows.Count.ToString() + "";

        }

      
        txtValue1Report.Focus();
        //chkYearCarry.Visible = false;
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        ddlField1Report.SelectedIndex = 0;
        ddlOption1Report.SelectedIndex = 2;
        txtValue1Report.Text = "";
        txtValueDate.Text = "";
        FillVoucherReportGrid();
      
        txtValue1Report.Focus();
        txtValue1Report.Visible = true;
        txtValueDate.Visible = false;

        //chkYearCarry.Visible = false;
    }
    public string GetType(string Type)
    {
        if (Type == "1")
        {
            Type = "January";
        }
        if (Type == "2")
        {
            Type = "February";
        }
        if (Type == "3")
        {
            Type = "March";

        }
        if (Type == "4")
        {
            Type = "April";

        }
        if (Type == "5")
        {
            Type = "May";

        }
        if (Type == "6")
        {
            Type = "June";

        }
        if (Type == "7")
        {
            Type = "July";

        }
        if (Type == "8")
        {
            Type = "August";

        }
        if (Type == "9")
        {
            Type = "september";

        }
        if (Type == "10")
        {
            Type = "October";

        }
        if (Type == "11")
        {
            Type = "November";

        }
        if (Type == "12")
        {
            Type = "December";

        }
        return Type;
    }
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        string strMonth = string.Empty;
        string strYear = string.Empty;
        string strEmpId = string.Empty;
        DataTable dt = da.return_DataTable("select pay_employe_month.Emp_Id,pay_employe_month.Month,pay_employe_month.Year from pay_employe_month inner join Set_EmployeeMaster on pay_employe_month.Emp_id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_id=" + ((Label)gvrow.FindControl("lblLocationId")).Text + " and pay_employe_month.voucher_no=" + e.CommandArgument.ToString() + "");

        if (dt.Rows.Count <= 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            strMonth = dr["Month"].ToString();
            strYear = dr["Year"].ToString();
            strEmpId += dr["Emp_Id"].ToString() + ",";
        }
        Session["DepartmentName"] = "All";
        Session["Month"] = strMonth;
        Session["MonthName"] = GetType(strMonth);
        Session["Year"] = strYear;
        Session["Querystring"] = strEmpId;

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/PayrollSummayReport.aspx?Value=2')", true);




    }



    protected void SetDateTextBox(object sender, EventArgs e)
    {
        if (ddlField1Report.SelectedValue == "Voucher_no")
        {
            txtValue1Report.Visible = true;
            txtValueDate.Visible = false;
        }
        else if (ddlField1Report.SelectedValue == "CreatedDate")
        {
            txtValue1Report.Visible = false;
            txtValueDate.Visible = true;
        }

        txtValue1Report.Text = "";
        txtValueDate.Text = "";

    }


    #endregion

}
