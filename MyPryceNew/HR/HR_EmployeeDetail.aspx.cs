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
using System.Data.OleDb;

public partial class HR_HR_EmployeeDetail : System.Web.UI.Page
{
    EmployeeMaster objEmp = null;
    Attendance objAttendance = null;
    HR_EmployeeDetail objEmpDetail = null;
    SystemParameter objsys = null;
    Set_ApplicationParameter objAppParam = null;
    Common ObjComman = null;
    EmployeeParameter objEmpParam = null;
    HR_Indemnity_Master objIndemnity = null;
    Pay_Employee_Attendance objPayEmpAtt = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objEmpDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objIndemnity = new HR_Indemnity_Master(Session["DBConnection"].ToString());
        objPayEmpAtt = new Pay_Employee_Attendance(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../HR/HR_EmployeeDetail.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            //  FillGrid();
            FillGridBin();
            btnList_Click(null, null);
        }
        
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        btnAllPost.Visible = clsPagePermission.bRestore;
    }


    private void FillGridBin()
    {
        DataTable dtBinEmp = objEmpDetail.GetAllFalseRecord();
        if (dtBinEmp.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBinEmp.Rows.Count.ToString() + "";
            objPageCmn.FillData((object)gvEmpDetailBin, dtBinEmp, "", "");
            Session["dtbinFilter"]= dtBinEmp;
        }
        else
        {
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
            gvEmpDetailBin.DataSource = null;
            gvEmpDetailBin.DataBind();
            Session["dtbinFilter"] = "";
        }
    }

    private void FillGrid()
    {
        DataTable dtTrueEmp = objEmpDetail.GetAllTrueRecord();
        if (dtTrueEmp.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpDetailMaster, dtTrueEmp, "", "");
            Session["dtFilter_HR_Emp_Dtls"] = dtTrueEmp;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtTrueEmp.Rows.Count.ToString() + "";
            btnAllPost.Visible = true;            
        }
        else
        {
            Session["dtFilter_HR_Emp_Dtls"] = null;
            gvEmpDetailMaster.DataSource = null;
            gvEmpDetailMaster.DataBind();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ":0";
            btnAllPost.Visible = false;
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        btnReset.Text = "Post";
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = objEmpDetail.GetRecord_By_TransId(editid.Value);
        txtEmpName.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_Code"].ToString();
        ViewState["Emp_Id"] = dt.Rows[0]["Emp_Id"].ToString();
        Edit_ID.Value = dt.Rows[0]["Emp_Id"].ToString();
        ddlMonth.SelectedValue = dt.Rows[0]["Month"].ToString();
        TxtYear.Text = dt.Rows[0]["Year"].ToString();
        txtMonthDays.Text = dt.Rows[0]["Total_Days"].ToString();
        txtPresentDays.Text = dt.Rows[0]["Present_Days"].ToString();
        txtAbsentDays.Text = dt.Rows[0]["Absent_Days"].ToString();
        txtHoliday.Text = dt.Rows[0]["Holiday_Days"].ToString();
        txtLeaveDays.Text = dt.Rows[0]["Leave_Days"].ToString();
        txtWeekoffDays.Text = dt.Rows[0]["WeekOff_Days"].ToString();
        txtLatePenaltyMin.Text = dt.Rows[0]["Late_Min"].ToString();
        txtEarlyPenaltyMin.Text = dt.Rows[0]["Early_Min"].ToString();
        txtPartialPenaltyMin.Text = dt.Rows[0]["Partial_Min"].ToString();
        txtNormalOTMin.Text = dt.Rows[0]["Normal_OTMin"].ToString();
        txtWeekOffOTMin.Text = dt.Rows[0]["WeekOff_OTMin"].ToString();
        txtHolidayOTMin.Text = dt.Rows[0]["Holiday_OTMin"].ToString();
        btnNew_Click(null, null);
        txtEmpName.Enabled = false;
        Lbl_New_tab.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    protected void txtEmpName_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dt = objEmp.GetEmployeeMasterByEmpName(Session["CompId"].ToString().ToString(), txtEmpName.Text.Trim().Split('/')[0].ToString());
        if (dt.Rows.Count > 0)
        {
            txtEmpName.Text = dt.Rows[0]["Emp_Name"].ToString()+"/"+ dt.Rows[0]["Emp_Code"].ToString();
            ViewState["Emp_Id"] = dt.Rows[0]["Emp_Id"].ToString();

            // txtInterviewDate.Focus();
        }
        else
        {
            txtEmpName.Text = "";
            DisplayMessage("Select Employee From Suggested List");
            return;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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


    protected void btnPost_Click(object sender, EventArgs e)
    {
        double BasicMonthSalary = 0;
        int AssignMin = 0;
        double PerMinSal = 0;
        double PerDaySalary = 0;
        double LatePenaltySalary = 0;
        double EarlyPenaltySalary = 0;
        double PartialPenaltySalary = 0;
        double BasicWorkSalary = 0;
        double LeaveDaysSalary = 0;
        double AbsentDaysSalary = 0;
        double WeekOffSalary = 0;
        double HolidaySalary = 0;
        // Get Employee Parameter 

        DataTable dtNonPostedEmp = objEmpDetail.GetNonPostedRecord(ViewState["Emp_Id"].ToString(), ddlMonth.SelectedValue, TxtYear.Text);
        if (dtNonPostedEmp.Rows.Count > 0)
        {
            for (int i = 0; i < dtNonPostedEmp.Rows.Count; i++)
            {
                DataTable dtEmpParam = objAttendance.GetEmployeeParameter(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString());
                BasicMonthSalary = Convert.ToDouble(dtEmpParam.Rows[0]["Basic_Salary"].ToString());
                AssignMin = Convert.ToInt32(dtEmpParam.Rows[0]["Assign_Min"].ToString());
                PerMinSal = BasicMonthSalary / (30 * AssignMin);
                PerDaySalary = BasicMonthSalary / 30;
                LatePenaltySalary = Convert.ToDouble(txtLatePenaltyMin.Text) * PerMinSal;
                EarlyPenaltySalary = Convert.ToDouble(txtEarlyPenaltyMin.Text) * PerMinSal;
                PartialPenaltySalary = Convert.ToDouble(txtPartialPenaltyMin.Text) * PerMinSal;
                BasicWorkSalary = Convert.ToDouble(txtPresentDays.Text) * PerDaySalary;
                LeaveDaysSalary = Convert.ToDouble(txtLeaveDays.Text) * PerDaySalary;
                AbsentDaysSalary = Convert.ToDouble(txtAbsentDays.Text) * PerDaySalary;
                WeekOffSalary = Convert.ToDouble(txtWeekoffDays.Text) * PerDaySalary;
                HolidaySalary = Convert.ToDouble(txtHoliday.Text) * PerDaySalary;
                int b = objEmpDetail.UpdatePayrllSalaryRecord(dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), ddlMonth.SelectedValue, TxtYear.Text, BasicWorkSalary.ToString(), HolidaySalary.ToString(), WeekOffSalary.ToString(), AbsentDaysSalary.ToString(), LeaveDaysSalary.ToString(), LatePenaltySalary.ToString(), EarlyPenaltySalary.ToString(), PartialPenaltySalary.ToString(), "", "", "");
            }
            DisplayMessage("Record Posted");
            FillGrid();
            FillGridBin();
            btnList_Click(null, null);
        }
    }
    protected void TxtYear_TextChanged(object sender, EventArgs e)
    {
        if (ddlMonth.SelectedValue == "0")
        {
            DisplayMessage("Select Month");
            ddlMonth.Focus();
            return;
        }
        if (TxtYear.Text == "")
        {
            DisplayMessage("Type Year");
            TxtYear.Focus();
            return;
        }
        int day = DateTime.DaysInMonth(Convert.ToInt32(TxtYear.Text), Convert.ToInt32(ddlMonth.SelectedValue));
        txtMonthDays.Text = Convert.ToString(day);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtEmpName.Text == "")
        {
            DisplayMessage("Type Employee Name");
            txtEmpName.Focus();
            return;
        }
        if (ddlMonth.SelectedValue == "")
        {
            DisplayMessage("Select Month");
            ddlMonth.Focus();
            return;
        }
        if (TxtYear.Text == "")
        {
            DisplayMessage("Type Year");
            TxtYear.Focus();
            return;
        }
        if (txtMonthDays.Text == "")
        {
            DisplayMessage("Type Month Days");
            txtMonthDays.Focus();
            return;
        }

        if (txtPresentDays.Text == "")
        {
            DisplayMessage("Type Present Days");
            txtPresentDays.Focus();
            return;
        }

        if (txtHoliday.Text == "")
        {
            DisplayMessage("Type Holiday Days");
            txtHoliday.Focus();
            return;
        }

        if (txtWeekoffDays.Text == "")
        {
            DisplayMessage("Type Week Off Days");
            txtWeekoffDays.Focus();
            return;
        }
        if (txtAbsentDays.Text == "")
        {
            DisplayMessage("Type Absent Days");
            txtAbsentDays.Focus();
            return;
        }
        if (txtLeaveDays.Text == "")
        {
            DisplayMessage("Type Leave Days");
            txtLeaveDays.Focus();
            return;
        }
        if (txtLatePenaltyMin.Text == "")
        {
            DisplayMessage("Type Late Penalty Min");
            txtLatePenaltyMin.Focus();
            return;
        }
        if (txtEarlyPenaltyMin.Text == "")
        {
            DisplayMessage("Type Early Penalty Min");
            txtEarlyPenaltyMin.Focus();
            return;
        }
        if (txtPartialPenaltyMin.Text == "")
        {
            DisplayMessage("Type Partial Penalty Min");
            txtPartialPenaltyMin.Focus();
            return;
        }
        if (txtNormalOTMin.Text == "")
        {
            DisplayMessage("Type Normal OT Min");
            txtNormalOTMin.Focus();
            return;
        }
        if (txtWeekOffOTMin.Text == "")
        {
            DisplayMessage("Type Week Off OT Min");
            txtWeekOffOTMin.Focus();
            return;
        }
        if (txtHolidayOTMin.Text == "")
        {
            DisplayMessage("Type Holiday OT Min");
            txtHolidayOTMin.Focus();
            return;
        }
        string EmpPayrollPosted = objAttendance.GePayrollPostedByEmpId(Session["CompId"].ToString(), ViewState["Emp_Id"].ToString(), ddlMonth.SelectedValue, TxtYear.Text);
        if (EmpPayrollPosted.ToString() != string.Empty)
        {
            //lblNonPost.Text = "Payroll Already Posted For Employee :-" + EmpPayrollPosted;
            lblNonPost.Text = "";
            DisplayMessage("Payroll Already Posted For Employee :-" + EmpPayrollPosted);
            return;
        }

        string strIsPost = string.Empty;
        if (editid.Value == "")
        {
            DataTable dtAll = objEmpDetail.GetAllRecord();
            if (dtAll.Rows.Count > 0)
            {
                dtAll = new DataView(dtAll, "Emp_Id=" + ViewState["Emp_Id"].ToString() + " and Month=" + ddlMonth.SelectedValue + " and Year=" + TxtYear.Text + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAll.Rows.Count > 0)
                {
                    editid.Value = dtAll.Rows[0]["Trans_Id"].ToString();
                    int b = objEmpDetail.InsertORUpdateRecord(editid.Value, ViewState["Emp_Id"].ToString(), ddlMonth.SelectedValue, TxtYear.Text, txtMonthDays.Text, txtPresentDays.Text, txtHoliday.Text, txtWeekoffDays.Text, txtAbsentDays.Text, txtLeaveDays.Text, txtLatePenaltyMin.Text, txtEarlyPenaltyMin.Text, txtPartialPenaltyMin.Text, txtNormalOTMin.Text, txtWeekOffOTMin.Text, txtHolidayOTMin.Text, "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    DisplayMessage("Record Saved","green");
                    Reset();
                }
                else
                {
                    int b = objEmpDetail.InsertORUpdateRecord("0", ViewState["Emp_Id"].ToString(), ddlMonth.SelectedValue, TxtYear.Text, txtMonthDays.Text, txtPresentDays.Text, txtHoliday.Text, txtWeekoffDays.Text, txtAbsentDays.Text, txtLeaveDays.Text, txtLatePenaltyMin.Text, txtEarlyPenaltyMin.Text, txtPartialPenaltyMin.Text, txtNormalOTMin.Text, txtWeekOffOTMin.Text, txtHolidayOTMin.Text, "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    Reset();
                    DisplayMessage("Record Saved","green");
                }
            }
            else
            {
                int b = objEmpDetail.InsertORUpdateRecord("0", ViewState["Emp_Id"].ToString(), ddlMonth.SelectedValue, TxtYear.Text, txtMonthDays.Text, txtPresentDays.Text, txtHoliday.Text, txtWeekoffDays.Text, txtAbsentDays.Text, txtLeaveDays.Text, txtLatePenaltyMin.Text, txtEarlyPenaltyMin.Text, txtPartialPenaltyMin.Text, txtNormalOTMin.Text, txtWeekOffOTMin.Text, txtHolidayOTMin.Text, "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                Reset();
                DisplayMessage("Record Saved","green");
            }
        }
        else
        {
            int b = objEmpDetail.InsertORUpdateRecord(editid.Value, ViewState["Emp_Id"].ToString(), ddlMonth.SelectedValue, TxtYear.Text, txtMonthDays.Text, txtPresentDays.Text, txtHoliday.Text, txtWeekoffDays.Text, txtAbsentDays.Text, txtLeaveDays.Text, txtLatePenaltyMin.Text, txtEarlyPenaltyMin.Text, txtPartialPenaltyMin.Text, txtNormalOTMin.Text, txtWeekOffOTMin.Text, txtHolidayOTMin.Text, "0", "0", "0", "0", "0", "0", "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            DisplayMessage("Record Saved","green");
            Reset();
            Lbl_New_tab.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
    }
    protected void btnAllPost_Click(object sender, EventArgs e)
    {
        string EmpIdList = lblSelectedRecord.Text;
        string NonPostedList = "";
        double BasicMonthSalary = 0;
        int AssignMin = 0;
        double PerMinSal = 0;
        double PerDaySalary = 0;
        double LatePenaltySalary = 0;
        double EarlyPenaltySalary = 0;
        double PartialPenaltySalary = 0;
        double BasicWorkSalary = 0;
        double LeaveDaysSalary = 0;
        double AbsentDaysSalary = 0;
        double WeekOffSalary = 0;
        double HolidaySalary = 0;
        double Normal_OTSal = 0;
        double WeekOff_OTSal = 0;
        double Holiday_OTSal = 0;

        bool IsWeekOffSalary = false;
        bool IsHolidaySalary = false;
        bool IsAbsentSalary = false;

        int TotalMonthDays = 0;
        string SalaryCalculationMethod = "";
        DataTable dtSalCalc = objAttendance.GetSalCalculationMethod(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        SalaryCalculationMethod = dtSalCalc.Rows[0]["Param_Value"].ToString();
        bool IsLeaveSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        bool IsCompIndemnity = false;
        IsCompIndemnity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));

        //for indeminity add parameter on 19-03-2015 by lokesh
        IsWeekOffSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsWeekOffLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        IsHolidaySalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsHolidayLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
        IsAbsentSalary = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsAbsentLeaveSalary", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));



        // Get Employee Parameter 
        string EmpPayrollPosted = objAttendance.GePayrollPostedByEmpId(Session["CompId"].ToString(), EmpIdList.ToString(), DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString());
        if (EmpPayrollPosted.ToString() != string.Empty)
        {
            //lblNonPost.Text = "Payroll Already Posted For Employee :-" + EmpPayrollPosted;
            DisplayMessage("Payroll Already Posted For Employee :-" + EmpPayrollPosted);
            // return;
        }
        else
        {

            for (int j = 0; j < EmpIdList.Split(',').Length - 1; j++)
            {
                if (EmpIdList.Split(',')[j] != "")
                {

                    DataTable DTEmpList = objEmpDetail.GetRecord_By_EmpId(EmpIdList.Split(',')[j].ToString());
                    if (DTEmpList.Rows.Count > 0)
                    {
                        DataTable dtNonPostedEmp = objEmpDetail.GetNonPostedRecord(DTEmpList.Rows[0]["Emp_Id"].ToString(), DTEmpList.Rows[0]["Month"].ToString(), DTEmpList.Rows[0]["Year"].ToString());
                        if (dtNonPostedEmp.Rows.Count > 0)
                        {
                            double Basic_Salary = 0;
                            for (int i = 0; i < dtNonPostedEmp.Rows.Count; i++)
                            {

                                try
                                {
                                    Basic_Salary = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), "Basic_Salary"));
                                }
                                catch
                                {
                                    Basic_Salary = 0;
                                }
                                if (SalaryCalculationMethod == "Monthly")
                                {
                                    TotalMonthDays = DateTime.DaysInMonth(Convert.ToInt32(DTEmpList.Rows[0]["Year"].ToString()), Convert.ToInt32(DTEmpList.Rows[0]["Month"].ToString()));
                                }
                                else
                                {
                                    TotalMonthDays = Convert.ToInt32(dtSalCalc.Rows[1]["Param_Value"].ToString());
                                }
                                DataTable dtEmpParam = objAttendance.GetEmployeeParameter(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString());
                                BasicMonthSalary = Convert.ToDouble(dtEmpParam.Rows[0]["Basic_Salary"].ToString());
                                AssignMin = Convert.ToInt32(dtEmpParam.Rows[0]["Assign_Min"].ToString());
                                PerMinSal = BasicMonthSalary / (TotalMonthDays * AssignMin);
                                // Absent Penalty Salary
                                AbsentDaysSalary = objAttendance.AbsentDaysSalary(Session["CompId"].ToString(), PerMinSal, dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), AssignMin, Convert.ToDouble(DTEmpList.Rows[0]["Absent_Days"].ToString()), Session["BrandId"].ToString(), Session["LocId"].ToString());
                                // Late Penalty Salary
                                LatePenaltySalary = objAttendance.LatePenalty(Session["CompId"].ToString(), PerMinSal, dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), AssignMin, Convert.ToInt32(DTEmpList.Rows[0]["Late_Min"].ToString()), "EmpDetail", Session["BrandId"].ToString(), Session["LocId"].ToString());
                                // Early Penalty 
                                EarlyPenaltySalary = objAttendance.EarlyPenalty(Session["CompId"].ToString(), PerMinSal, dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), AssignMin, Convert.ToInt32(DTEmpList.Rows[0]["Early_Min"].ToString()), "EmpDetail", Session["BrandId"].ToString(), Session["LocId"].ToString());

                                // Normal OT Salary
                                Normal_OTSal = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), PerMinSal, Convert.ToInt32(DTEmpList.Rows[0]["Normal_OTMin"].ToString()), "Normal");

                                // Week Off Salary
                                WeekOff_OTSal = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), PerMinSal, Convert.ToInt32(DTEmpList.Rows[0]["WeekOff_OTMin"].ToString()), "WeekOff");

                                // Holiday OT Salary 
                                Holiday_OTSal = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), PerMinSal, Convert.ToInt32(DTEmpList.Rows[0]["Holiday_OTMin"].ToString()), "Holiday");

                                // Partial Penalty
                                PartialPenaltySalary = objAttendance.ParialPenalty(Session["CompId"].ToString(), PerMinSal, dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), AssignMin, Convert.ToInt32(DTEmpList.Rows[0]["Partial_Min"].ToString()), "EmpDetail", Session["BrandId"].ToString(), Session["LocId"].ToString());

                                PerDaySalary = BasicMonthSalary / TotalMonthDays;
                                WeekOffSalary = Convert.ToDouble(DTEmpList.Rows[0]["WeekOff_Days"].ToString()) * PerDaySalary;
                                HolidaySalary = Convert.ToDouble(DTEmpList.Rows[0]["Holiday_Days"].ToString()) * PerDaySalary;


                                //LatePenaltySalary = Convert.ToDouble(txtLatePenaltyMin.Text) * PerMinSal;
                                // EarlyPenaltySalary = Convert.ToDouble(txtEarlyPenaltyMin.Text) * PerMinSal;
                                // PartialPenaltySalary = Convert.ToDouble(txtPartialPenaltyMin.Text) * PerMinSal;
                                BasicWorkSalary = Convert.ToDouble(DTEmpList.Rows[0]["Present_Days"].ToString()) * PerDaySalary;



                                double PresentDays = Convert.ToDouble(DTEmpList.Rows[0]["Present_Days"].ToString());
                                double LeaveSalWeekOffCount = 0;
                                double LeaveSalHolidayCount = 0;
                                double LeaveSalAbsentCount = 0;
                                double LeaveSalWorkedDays = 0;

                                if (IsWeekOffSalary)
                                {
                                    LeaveSalWeekOffCount = Convert.ToDouble(DTEmpList.Rows[0]["WeekOff_Days"].ToString());
                                }
                                else
                                {
                                    LeaveSalWeekOffCount = 0;
                                }
                                if (IsHolidaySalary)
                                {
                                    LeaveSalHolidayCount = Convert.ToDouble(DTEmpList.Rows[0]["Holiday_Days"].ToString());
                                }
                                else
                                {
                                    LeaveSalHolidayCount = 0;
                                }
                                if (IsAbsentSalary)
                                {
                                    LeaveSalAbsentCount = Convert.ToDouble(DTEmpList.Rows[0]["Absent_Days"].ToString());
                                }
                                else
                                {
                                    LeaveSalAbsentCount = 0;
                                }
                                LeaveSalWorkedDays = PresentDays + LeaveSalWeekOffCount + LeaveSalHolidayCount + LeaveSalAbsentCount;


                                PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
                                //Leave Salary Work
                                if (IsLeaveSalary == true)
                                {
                                    LeaveDaysSalary = Convert.ToDouble(DTEmpList.Rows[0]["Leave_Days"].ToString()) * PerDaySalary;
                                }
                                else
                                {


                                    DataTable dt12 = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                                    int FinancialYearMonth = 0;

                                    if (dt12.Rows.Count > 0)
                                    {
                                        FinancialYearMonth = int.Parse(dt12.Rows[0]["Param_Value"].ToString());

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

                                    DataTable dtLeaveEmpInfo = objDA.return_DataTable("Select * From Set_Att_Employee_Leave Where  Emp_Id = '" + dtNonPostedEmp.Rows[i]["Emp_Id"].ToString() + "' AND Field5='False' ");
                                    DataTable DTDOJ = objAttendance.GetEmployeeDOJ(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString());
                                    DateTime DOJ = Convert.ToDateTime(DTDOJ.Rows[0]["DOJ"].ToString());

                                    for (int iLeave = 0; iLeave < dtLeaveEmpInfo.Rows.Count; iLeave++)
                                    {

                                        //dtLeaveEmpInfo.Rows[iLeave]["LeaveType_Id"].tostring()
                                        string strLEaveTypeId = dtLeaveEmpInfo.Rows[iLeave]["LeaveType_Id"].ToString();
                                        double PLeave = Convert.ToDouble(dtLeaveEmpInfo.Rows[iLeave]["Paid_Leave"].ToString()) / 12;
                                        //dtLeaveEmpInfo.Rows[iLeave]["LeaveType_Id"].tostring()

                                        if (FinancialYearStartDate < DOJ)
                                        {
                                            if (Convert.ToInt32(DTEmpList.Rows[0]["Month"].ToString()) == DOJ.Month)
                                            {
                                                DateTime dtEm = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                                dtEm = (dtEm.AddMonths(1)).AddDays(-1);
                                                int rDays = Convert.ToInt32((dtEm - DOJ).TotalDays);
                                                //LeaveSalWorkedDays = Convert.ToInt32((dtEm - DOJ).TotalDays);
                                                PLeave = (PLeave / 30) * rDays;

                                            }
                                        }

                                        PerDaySalary = Basic_Salary / 30;
                                        double TotalLeaveSal = PLeave * PerDaySalary;
                                        //string strDelete = "Delete from Att_LeaveSalary where Emp_Id='" + dtNonPostedEmp.Rows[i]["Emp_Id"].ToString() + "' and L_Month='" + DTEmpList.Rows[0]["Month"].ToString() + "' and L_Year='" + DTEmpList.Rows[0]["Year"].ToString() + "' and Is_Report='False'";
                                        //int c = objDA.execute_Command(strDelete);

                                        //string strSQL = "INSERT INTO Att_LeaveSalary (Emp_Id,L_Month,L_Year ,Leave_Type_Id,Leave_Count,Per_Day_Salary,Total,Is_Report,F1,F2,F3,F4,F5,F6,F7)  VALUES ('" + dtNonPostedEmp.Rows[i]["Emp_Id"].ToString() + "','" + DTEmpList.Rows[0]["Month"].ToString() + "','" + DTEmpList.Rows[0]["Year"].ToString() + "','" + strLEaveTypeId + "','" + PLeave + "','" + PerDaySalary + "','" + TotalLeaveSal + "','" + false.ToString() + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "')";
                                        //int a = objDA.execute_Command(strSQL);
                                    }
                                }
                                // AbsentDaysSalary = Convert.ToDouble(txtAbsentDays.Text) * PerDaySalary;

                                //Indeminty Code Start here On 13-03-2015 by lokesh
                                //if (IsCompIndemnity)
                                //{
                                //    bool IsEmpIndemnity = false;
                                //    double IndemnityPerDayLeave = 0;
                                //    DataTable DtIndemnityEmp = objIndemnity.GetIndemnityEmployeeForPayroll(dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), DTEmpList.Rows[0]["Month"].ToString(), DTEmpList.Rows[0]["Year"].ToString());
                                //    if (DtIndemnityEmp.Rows.Count > 0)
                                //    {
                                //        for (int IND = 0; IND < DtIndemnityEmp.Rows.Count; IND++)
                                //        {

                                //            IsEmpIndemnity = Convert.ToBoolean(objEmpParam.GetEmployeeParameterValueByParamNameNew("IsIndemnity", DtIndemnityEmp.Rows[IND]["Employee_Id"].ToString()));
                                //            if (IsEmpIndemnity)
                                //            {
                                //                IndemnityPerDayLeave = Convert.ToDouble(objEmpParam.GetEmployeeParameterValueByParamNameNew("IndemnityDayas", DtIndemnityEmp.Rows[IND]["Employee_Id"].ToString()));
                                //                IndemnityPerDayLeave = (IndemnityPerDayLeave / 12) / 30;
                                //                LeaveSalWorkedDays = (LeaveSalWorkedDays * IndemnityPerDayLeave) * PerDaySalary;

                                //                //string strDelete = "Delete from Att_IndemnitySalary where Emp_Id='" + DtIndemnityEmp.Rows[IND]["Employee_Id"].ToString() + "' and Indemnity_Month='" + DTEmpList.Rows[0]["Month"].ToString() + "' and Indemnity_Year='" + DTEmpList.Rows[0]["Year"].ToString() + "'";
                                //                //int c = objDA.execute_Command(strDelete);

                                //                //string strSQL = "INSERT INTO Att_IndemnitySalary (Emp_Id,Indemnity_Month,Indemnity_Year,Per_Day_Salary,Indemnity_Salary,Indemnity_Days,Is_Report,F1,F2,F3,F4,F5,F6,F7)  VALUES ('" + DtIndemnityEmp.Rows[IND]["Employee_Id"].ToString() + "','" + DTEmpList.Rows[0]["Month"].ToString() + "','" + DTEmpList.Rows[0]["Year"].ToString() + "','" + PerDaySalary.ToString() + "','" + LeaveSalWorkedDays.ToString() + "','" + IndemnityPerDayLeave.ToString() + "','" + true.ToString() + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "')";
                                //                //int q = objDA.execute_Command(strSQL);


                                //                //string strDeleteHRIndem = "Delete from HR_Indemnity_Master where Company_Id='" + Session["CompId"].ToString() + "' and Employee_Id='" + DtIndemnityEmp.Rows[IND]["Employee_Id"].ToString() + "' and Indemnity_Date='" + Convert.ToDateTime(DtIndemnityEmp.Rows[IND]["Indemnity_Date"].ToString()).AddMonths(1).ToString() + "'";
                                //                //int s = objDA.execute_Command(strDeleteHRIndem);

                                //                //int Indemnity = objIndemnity.InsertIndemnityRecord("0", Session["CompId"].ToString(), DtIndemnityEmp.Rows[IND]["Employee_Id"].ToString(), Convert.ToDateTime(DtIndemnityEmp.Rows[IND]["Indemnity_Date"].ToString()).AddMonths(1).ToString(), "Pending", "2", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                //            }
                                //        }
                                //    }
                                //}

                                //here we inserting data in attendance detail table 
                                //code added on 20/11/2017 by jitendra upadhyay
                                //because attendance calculation is showing 0 in edit payroll page

                                objDA.execute_Command("delete from pay_employee_attendance where emp_id='" + dtNonPostedEmp.Rows[i]["Emp_Id"].ToString() + "' and month='" + DTEmpList.Rows[0]["Month"].ToString() + "' and year='" + DTEmpList.Rows[0]["Year"].ToString() + "'");

                                objPayEmpAtt.InsertPayEmployeeAttendance(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), DTEmpList.Rows[0]["Month"].ToString(), DTEmpList.Rows[0]["Year"].ToString(), DTEmpList.Rows[0]["Total_Days"].ToString(), "0", Math.Round(Convert.ToDouble(DTEmpList.Rows[0]["Present_Days"].ToString()), 0).ToString(), DTEmpList.Rows[0]["WeekOff_Days"].ToString(), DTEmpList.Rows[0]["Holiday_Days"].ToString(), Math.Round(Convert.ToDouble(DTEmpList.Rows[0]["Leave_Days"].ToString()), 0).ToString(), Math.Round(Convert.ToDouble(DTEmpList.Rows[0]["Absent_Days"].ToString()), 0).ToString(), "0", Basic_Salary.ToString(), BasicWorkSalary.ToString(), Normal_OTSal.ToString(), WeekOff_OTSal.ToString(), Holiday_OTSal.ToString(), AbsentDaysSalary.ToString(), DTEmpList.Rows[0]["Late_Min"].ToString(), EarlyPenaltySalary.ToString(), DTEmpList.Rows[0]["Partial_Min"].ToString(), "0", DTEmpList.Rows[0]["Holiday_OTMin"].ToString(), DTEmpList.Rows[0]["WeekOff_OTMin"].ToString(), DTEmpList.Rows[0]["Normal_OTMin"].ToString(), DTEmpList.Rows[0]["Late_Min"].ToString(), DTEmpList.Rows[0]["Early_Min"].ToString(), DTEmpList.Rows[0]["Partial_Min"].ToString(), BasicWorkSalary.ToString(), Normal_OTSal.ToString(), WeekOff_OTSal.ToString(), Holiday_OTSal.ToString(), WeekOffSalary.ToString(), HolidaySalary.ToString(), LeaveDaysSalary.ToString(), AbsentDaysSalary.ToString(), LatePenaltySalary.ToString(), DTEmpList.Rows[0]["Early_Min"].ToString(), PartialPenaltySalary.ToString(), DateTime.Now.ToString(), "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                int b = objEmpDetail.UpdatePayrllSalaryRecord(dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), DTEmpList.Rows[0]["Month"].ToString(), DTEmpList.Rows[0]["Year"].ToString(), BasicWorkSalary.ToString(), HolidaySalary.ToString(), WeekOffSalary.ToString(), AbsentDaysSalary.ToString(), LeaveDaysSalary.ToString(), LatePenaltySalary.ToString(), EarlyPenaltySalary.ToString(), PartialPenaltySalary.ToString(), Normal_OTSal.ToString(), WeekOff_OTSal.ToString(), Holiday_OTSal.ToString());
                            }
                            DisplayMessage("Record Posted");
                            FillGrid();
                            FillGridBin();
                            btnList_Click(null, null);
                            btnReset.Text = "Reset";
                        }
                        else
                        {
                            NonPostedList += DTEmpList.Rows[0]["Emp_Code"].ToString() + ",";
                        }
                    }
                }
            }
        }
        if (NonPostedList.TrimEnd().ToString() != string.Empty)
        {
            //lblNonPost.Text = "Payroll Not Generate For Employee :-" + NonPostedList.TrimEnd().ToString();
            DisplayMessage("Payroll Not Generate For Employee :-" + NonPostedList.TrimEnd().ToString());
        }
        else
        {
            lblNonPost.Text = string.Empty;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        if (btnReset.Text == "Reset")
        {
            Reset();
        }
        else
        {

            string NonPostedList = "";
            double BasicMonthSalary = 0;
            int AssignMin = 0;
            double PerMinSal = 0;
            double PerDaySalary = 0;
            double LatePenaltySalary = 0;
            double EarlyPenaltySalary = 0;
            double PartialPenaltySalary = 0;
            double BasicWorkSalary = 0;
            double LeaveDaysSalary = 0;
            double AbsentDaysSalary = 0;
            double WeekOffSalary = 0;
            double HolidaySalary = 0;
            double Normal_OTSal = 0;
            double WeekOff_OTSal = 0;
            double Holiday_OTSal = 0;


            // Get Employee Parameter 

            string EmpPayrollPosted = objAttendance.GePayrollPostedByEmpId(Session["CompId"].ToString(), Edit_ID.Value.ToString(), DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString());
            if (EmpPayrollPosted.ToString() != string.Empty)
            {
                //lblNonPost.Text = "Payroll Already Posted For Employee :-" + EmpPayrollPosted;
                DisplayMessage("Payroll Already Posted For Employee :-" + EmpPayrollPosted);
                // return;
            }
            else
            {
                DataTable DTEmpList = objEmpDetail.GetRecord_By_EmpId(Edit_ID.Value.ToString());
                if (DTEmpList.Rows.Count > 0)
                {
                    DataTable dtNonPostedEmp = objEmpDetail.GetNonPostedRecord(Edit_ID.Value, ddlMonth.SelectedValue, TxtYear.Text);
                    if (dtNonPostedEmp.Rows.Count > 0)
                    {
                        int TotalMonthDays = 0;
                        string SalaryCalculationMethod = "";
                        DataTable dtSalCalc = objAttendance.GetSalCalculationMethod(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                        SalaryCalculationMethod = dtSalCalc.Rows[0]["Param_Value"].ToString();
                        if (SalaryCalculationMethod == "Monthly")
                        {
                            TotalMonthDays = DateTime.DaysInMonth(Convert.ToInt32(TxtYear.Text), Convert.ToInt32(ddlMonth.SelectedValue));
                        }
                        else
                        {
                            TotalMonthDays = Convert.ToInt32(dtSalCalc.Rows[1]["Param_Value"].ToString());
                        }

                        for (int i = 0; i < dtNonPostedEmp.Rows.Count; i++)
                        {
                            DataTable dtEmpParam = objAttendance.GetEmployeeParameter(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString());
                            BasicMonthSalary = Convert.ToDouble(dtEmpParam.Rows[0]["Basic_Salary"].ToString());
                            AssignMin = Convert.ToInt32(dtEmpParam.Rows[0]["Assign_Min"].ToString());
                            PerMinSal = BasicMonthSalary / (TotalMonthDays * AssignMin);
                            // Absent Penalty Salary
                            AbsentDaysSalary = objAttendance.AbsentDaysSalary(Session["CompId"].ToString(), PerMinSal, dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), AssignMin, Convert.ToDouble(txtAbsentDays.Text), Session["BrandId"].ToString(), Session["LocId"].ToString());
                            // Late Penalty Salary
                            LatePenaltySalary = objAttendance.LatePenalty(Session["CompId"].ToString(), PerMinSal, dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), AssignMin, Convert.ToInt32(txtLatePenaltyMin.Text), "EmpDetail", Session["BrandId"].ToString(), Session["LocId"].ToString());
                            // Early Penalty 
                            EarlyPenaltySalary = objAttendance.EarlyPenalty(Session["CompId"].ToString(), PerMinSal, dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), AssignMin, Convert.ToInt32(txtEarlyPenaltyMin.Text), "EmpDetail", Session["BrandId"].ToString(), Session["LocId"].ToString());

                            // Normal OT Salary
                            Normal_OTSal = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), PerMinSal, Convert.ToInt32(txtNormalOTMin.Text), "Normal");

                            // Week Off Salary
                            WeekOff_OTSal = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), PerMinSal, Convert.ToInt32(txtNormalOTMin.Text), "WeekOff");

                            // Holiday OT Salary 
                            Holiday_OTSal = objAttendance.GetOTSalary(Session["CompId"].ToString(), dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), PerMinSal, Convert.ToInt32(txtNormalOTMin.Text), "Holiday");

                            // Partial Penalty
                            PartialPenaltySalary = objAttendance.ParialPenalty(Session["CompId"].ToString(), PerMinSal, dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), AssignMin, Convert.ToInt32(txtPartialPenaltyMin.Text), "EmpDetail", Session["BrandId"].ToString(), Session["LocId"].ToString());

                            PerDaySalary = BasicMonthSalary / TotalMonthDays;
                            //LatePenaltySalary = Convert.ToDouble(txtLatePenaltyMin.Text) * PerMinSal;
                            // EarlyPenaltySalary = Convert.ToDouble(txtEarlyPenaltyMin.Text) * PerMinSal;
                            // PartialPenaltySalary = Convert.ToDouble(txtPartialPenaltyMin.Text) * PerMinSal;
                            BasicWorkSalary = Convert.ToDouble(txtPresentDays.Text) * PerDaySalary;
                            LeaveDaysSalary = Convert.ToDouble(txtLeaveDays.Text) * PerDaySalary;
                            // AbsentDaysSalary = Convert.ToDouble(txtAbsentDays.Text) * PerDaySalary;
                            WeekOffSalary = Convert.ToDouble(txtWeekoffDays.Text) * PerDaySalary;
                            HolidaySalary = Convert.ToDouble(txtHoliday.Text) * PerDaySalary;
                            // Normal_OTSal = Convert.ToDouble(txtNormalOTMin.Text) * PerMinSal;
                            // WeekOff_OTSal = Convert.ToDouble(txtWeekOffOTMin.Text) * PerMinSal;
                            //  Holiday_OTSal = Convert.ToDouble(txtHolidayOTMin.Text) * PerMinSal;

                            int b = objEmpDetail.UpdatePayrllSalaryRecord(dtNonPostedEmp.Rows[i]["Emp_Id"].ToString(), ddlMonth.SelectedValue, TxtYear.Text, BasicWorkSalary.ToString(), HolidaySalary.ToString(), WeekOffSalary.ToString(), AbsentDaysSalary.ToString(), LeaveDaysSalary.ToString(), LatePenaltySalary.ToString(), EarlyPenaltySalary.ToString(), PartialPenaltySalary.ToString(), Normal_OTSal.ToString(), WeekOff_OTSal.ToString(), Holiday_OTSal.ToString());
                        }
                        DisplayMessage("Record Posted");
                        FillGrid();
                        FillGridBin();                        
                        btnReset.Text = "Reset";
                        btnReset_Click(null, null);
                        btnList_Click(null, null);
                        Lbl_New_tab.Text = Resources.Attendance.New;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    }
                    else
                    {
                        NonPostedList += DTEmpList.Rows[0]["Emp_Code"].ToString() + ",";
                    }
                }
            }
            if (NonPostedList.TrimEnd().ToString() != string.Empty)
            {
                //lblNonPost.Text = "Payroll Not Generate For Employee :-" + NonPostedList.TrimEnd().ToString();
                DisplayMessage("Payroll Not Generate For Employee :-" + NonPostedList.TrimEnd().ToString());
            }
            else
            {
                lblNonPost.Text = string.Empty;
            }
        }        
    }

    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvEmpDetailMaster.Rows[index].FindControl("lblFileId");
        if (((CheckBox)gvEmpDetailMaster.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;

        }

        else
        {

            empidlist += lb.Text.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
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
            lblSelectedRecord.Text = temp;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        btnList_Click(null, null);

        
        Lbl_New_tab.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmpDetailMaster.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvEmpDetailMaster.Rows.Count; i++)
        {
            ((CheckBox)gvEmpDetailMaster.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvEmpDetailMaster.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvEmpDetailMaster.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvEmpDetailMaster.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        FillGrid();
        txtValue.Focus();
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlEmpUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlEmpUpload.Visible = false;
        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtEmpName.Focus();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlEmpUpload.Visible = false;
        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
        pnlEmpUpload.Visible = false;
    }
    #region
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        ViewState["Select"] = null;

        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlEmpUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        // pnlAttList.Visible = false;
        PnlBin.Visible = false;
        // pnlEmpAtt.Visible = false;
        pnlEmpUpload.Visible = true;
        PnlNewEdit.Visible = false;
        div_Grid.Style.Add("display", "none");
        PnlList.Visible = false;
        Div_showdata.Style.Add("display", "none");
        ddlTables.Items.Clear();
        lblMessage.Text = "";
        Session["cnn"] = null;

        FillGridBin();

    }
    protected void btnConnect_Click(object sender, EventArgs e)
    {
        int fileType = -1;

        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                Literal l4 = new Literal();
                l4.Text = @"<font size=4 color=red></font><script>alert(""Please load a excel/access file"");</script></br></br>";
                this.Controls.Add(l4);
                return;
            }

            if (ext == ".xls")
            {
                fileType = 0;
            }
            if (ext == ".xlsx")
            {
                fileType = 1;
            }
            if (ext == ".mdb")
            {
                fileType = 2;
            }
            if (ext == ".accdb")
            {
                fileType = 3;
            }
            string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + fileLoad.FileName);
            fileLoad.SaveAs(path);

            //DataTable dt//
            Import(path, fileType);
            //if (dt != null)
            //{
            //  //  gvLoadContact.DataSource = dt;
            //   // Session["dtContact"] = dt;
            //   // gvLoadContact.DataBind();
            //}
            Literal l5 = new Literal();
            l5.Text = @"<font size=4 color=red></font><script>alert(""file succesfully uploaded"");</script></br></br>";
            this.Controls.Add(l5);
        }
        else
        {
            Literal l4 = new Literal();
            l4.Text = @"<font size=4 color=red></font><script>alert(""Please load a  file"");</script></br></br>";
            this.Controls.Add(l4);
        }
        fileLoad.FileContent.Dispose();
        //tr0.Visible = true;
    }
    public void Import(String path, int fileType)
    {
        string strcon = string.Empty;
        if (fileType == 1)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=NO;iMEX=1\"";
        }
        else if (fileType == 0)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=NO;iMEX=1\"";
        }
        else
        {
            Session["filetype"] = "access";
            //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:/abc.mdb;Persist Security Info=False
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
        }
        Session["cnn"] = strcon;

        OleDbConnection conn = new OleDbConnection(strcon);
        conn.Open();

        DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        ddlTables.DataSource = tables;

        ddlTables.DataTextField = "TABLE_NAME";
        ddlTables.DataValueField = "TABLE_NAME";
        ddlTables.DataBind();
        // OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", conn);
        //create new dataset
        //  DataSet ds = new DataSet();
        // fill dataset
        // da.Fill(ds);
        //populate grid with data
        //this.gvLoadContact.DataSource = ds.Tables[0];
        ////close connection
        conn.Close();

        //return ds.Tables[0];
    }
    protected void btnviewcolumns_Click(object sender, EventArgs e)
    {
        if (Session["cnn"] != null)
        {

            OleDbConnection cnn = new OleDbConnection(Session["cnn"].ToString());
            OleDbDataAdapter adp = new OleDbDataAdapter("", "");
            adp.SelectCommand.CommandText = "Select *  From [" + ddlTables.SelectedValue.ToString() + "]";
            adp.SelectCommand.Connection = cnn;

            DataTable userTable = new DataTable();
            try
            {
                adp.Fill(userTable);
            }
            catch (Exception)
            {
                Literal l4 = new Literal();
                l4.Text = @"<font size=4 color=red></font><script>alert(""Error in Mapping File"");</script></br></br>";
                this.Controls.Add(l4);
                return;
            }
            int counter = 0;
            DataTable dtSourceData = new DataTable();
            dtSourceData = userTable.Copy();
            lblMessage.Text = "";

            Session["SourceData"] = userTable;
            DataTable dtcolumn = new DataTable();
            dtcolumn.Columns.Add("COLUMN_NAME");
            dtcolumn.Columns.Add("COLUMN");
            for (int i = 0; i < userTable.Columns.Count; i++)
            {
                dtcolumn.Rows.Add(dtcolumn.NewRow());
                if (Session["filetype"].ToString() != "excel")
                {
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN_NAME"] = userTable.Columns[i].ToString();
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN"] = userTable.Columns[i].ToString();
                }
                else
                {
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN_NAME"] = userTable.Rows[0][i].ToString();
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN"] = userTable.Columns[i].ToString();
                }
            }

            Session["SourceTbl"] = dtcolumn;
            //get destination table field 
            DataTable dtDestinationDt = objEmpDetail.GetFieldName();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvFieldMapping, dtDestinationDt, "", "");

            //get source field
            pnlUpload1.Visible = false;
            
            div_Grid.Style.Add("display", "");
            pnlUpload1.Visible = false;

        }

    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        pnlUpload1.Visible = true;
        div_Grid.Style.Add("display", "none");
        Div_showdata.Style.Add("display", "none");
        ddlTables.Items.Clear();        
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    protected void btnUpload_Click1(object sender, EventArgs e)
    {
        DataTable dtDuplicateRecords = new DataTable();
        dtDuplicateRecords.Columns.Add("Emp_Id");
        dtDuplicateRecords.Columns.Add("Month");
        dtDuplicateRecords.Columns.Add("Year");
        dtDuplicateRecords.Columns.Add("Total_Days");
        dtDuplicateRecords.Columns.Add("Present_Days");

        dtDuplicateRecords.Columns.Add("Holiday_Days");
        dtDuplicateRecords.Columns.Add("WeekOff_Days");
        dtDuplicateRecords.Columns.Add("Absent_Days");
        dtDuplicateRecords.Columns.Add("Leave_Days");
        dtDuplicateRecords.Columns.Add("Late_Min");
        dtDuplicateRecords.Columns.Add("Early_Min");
        dtDuplicateRecords.Columns.Add("Partial_Min");

        dtDuplicateRecords.Columns.Add("Normal_OTMin");
        dtDuplicateRecords.Columns.Add("WeekOff_OTMin");
        dtDuplicateRecords.Columns.Add("Holiday_OTMin");
        string empids = string.Empty;
        string empidnotexists = string.Empty;
        string Date;
        string Month;
        string Year;
        string ActualDate;
        int Insertedrowcount = 0;
        DateTime date_Of_Joining = new DateTime();
        string compid = Session["CompId"].ToString();

        double Total_Days = 0;
        double HoliDays = 0;
        double Leave_Days = 0;
        double Present_Days = 0;
        double Weekoff_Days = 0;
        double Absent_Days = 0;
        string Days_Mismatch = string.Empty;

        for (int rowcounter = 1; rowcounter < gvSelected.Rows.Count; rowcounter++)
        {
            Total_Days = 0;
            HoliDays = 0;
            Leave_Days = 0;
            Present_Days = 0;
            Weekoff_Days = 0;
            Absent_Days = 0;

            int counter = 0;

            SetAllId();
            for (int col = 0; col < gvSelected.Rows[rowcounter].Cells.Count; col++)
            {
                string colname = gvSelected.HeaderRow.Cells[col].Text;

                string colval = gvSelected.Rows[rowcounter].Cells[col].Text;

                colval = colval.Replace("&#160;", "");

                colval = colval.Replace("&nbsp;", "");
                if (colval == "")
                {
                    counter = 1;
                    break;
                }

                if (colname == "Emp_Id")
                {
                    objEmpDetail.Emp_Id = colval;
                }
                if (colname == "Month")
                {
                    objEmpDetail.Month = colval;
                }
                if (colname == "Year")
                {
                    objEmpDetail.Year = colval;
                }
                if (colname == "Total_Days")
                {
                    objEmpDetail.TotalDays = colval;
                    Total_Days = Convert.ToDouble(colval);
                }
                if (colname == "Present_Days")
                {
                    objEmpDetail.PresentDays = colval;
                    Present_Days = Convert.ToDouble(colval);
                }

                if (colname == "WeekOff_Days")
                {
                    objEmpDetail.WeekOffDays = colval;
                    Weekoff_Days = Convert.ToDouble(colval);
                }
                if (colname == "Absent_Days")
                {
                    objEmpDetail.AbsentDays = colval;
                    Absent_Days = Convert.ToDouble(colval);
                }
                if (colname == "Leave_Days")
                {
                    objEmpDetail.LeaveDays = colval;
                    Leave_Days = Convert.ToDouble(colval);
                }
                if (colname == "Holiday_Days")
                {
                    objEmpDetail.HolidayDays = colval;
                    HoliDays = Convert.ToDouble(colval);
                }
                if (colname == "Late_Min")
                {
                    objEmpDetail.LateMin = colval;
                }
                if (colname == "Early_Min")
                {
                    objEmpDetail.EarlyMin = colval;
                }
                if (colname == "Partial_Min")
                {
                    objEmpDetail.PartialMin = colval;
                }
                //
                if (colname == "Normal_OTMin")
                {
                    objEmpDetail.Normal_OTMin = colval;
                }
                if (colname == "WeekOff_OTMin")
                {
                    objEmpDetail.WeekOff_OTMin = colval;
                }
                if (colname == "Holiday_OTMin")
                {
                    objEmpDetail.Holiday_OTMin = colval;
                }
            }





            if (counter == 1)
            {
                continue;
            }
            int b = 0;

            string EmpId = objEmpDetail.GetEmployeeId(objEmpDetail.Emp_Id);
            if (EmpId != "0")
            {
                if (Total_Days == (Present_Days + Weekoff_Days + Leave_Days + HoliDays + Absent_Days))
                {
                    string EmpPayrollPosted = objAttendance.GePayrollPostedByEmpId(Session["CompId"].ToString(), EmpId, DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString());
                    if (EmpPayrollPosted.ToString() != string.Empty)
                    {
                        //lblNonPost.Text = "Payroll Already Posted For Employee :-" + EmpPayrollPosted;
                        lblNonPost.Text = "";
                        DisplayMessage("Payroll Already Posted For Employee :-" + EmpPayrollPosted);
                        return;
                    }
                    if (EmpId != string.Empty)
                    {
                        b = objEmpDetail.InsertORUpdateRecord("0", EmpId.ToString(), objEmpDetail.Month, objEmpDetail.Year, objEmpDetail.TotalDays, objEmpDetail.PresentDays, objEmpDetail.HolidayDays, objEmpDetail.WeekOffDays, objEmpDetail.AbsentDays, objEmpDetail.LeaveDays, objEmpDetail.LateMin, objEmpDetail.EarlyMin, objEmpDetail.PartialMin, objEmpDetail.Normal_OTMin, objEmpDetail.WeekOff_OTMin, objEmpDetail.Holiday_OTMin, "", "", "", "", "", "", "", true.ToString(), "Superadmin", System.DateTime.Now.ToString(), "SuperAdmin", System.DateTime.Now.ToString());
                    }

                    if (b != 0)
                    {
                        Insertedrowcount++;
                    }
                }
                else
                {
                    Days_Mismatch = objEmpDetail.Emp_Id.ToString() + "," + Days_Mismatch;
                    empidnotexists = "True";
                }
            }
            else
            {
                empidnotexists = objEmpDetail.Emp_Id.ToString() + "," + empidnotexists;
            }
        }
        if (empids != "")
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvSelected, dtDuplicateRecords, "", "");

            if (empidnotexists != "")
            {
                if (lblNonPost.Text != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Insertedrowcount + " Row Inserted and Following EmpId does not exists" + "(" + empidnotexists + ")" + " Following Records Already Exists and Payroll Posted for Employee List" + "(" + lblNonPost.Text + ")" + "')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Insertedrowcount + " Row Inserted and Following EmpId does not exists" + "(" + empidnotexists + ")" + " Following Records Already Exists')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Insertedrowcount + " Row Inserted and Following Records Already Exists')", true);

            }
        }
        else
        {
            if (empidnotexists != "")
            {
                if (lblNonPost.Text != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Insertedrowcount + " Row Inserted; and Following EmpId does not exists" + "(" + empidnotexists + ")')", true);
                }
                else if(empidnotexists!="" && Days_Mismatch=="")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Insertedrowcount + " Row Inserted; and Following EmpId does not exists" + "(" + empidnotexists + ")')", true);
                }
                else if (empidnotexists != "True" && empidnotexists != "" && Days_Mismatch != "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Insertedrowcount + " Row Inserted; and Following EmpId does not exists" + "(" + empidnotexists + "); and Following EmpId does not Match along with Total Days and sum of Present Days, WeekOff Days, Leave Days, Holidays & Absent Days" + "(" + Days_Mismatch + ")')", true);
                }
                else if (empidnotexists == "True" && Days_Mismatch != "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Insertedrowcount + " Row Inserted; and Following EmpId does not Match along with Total Days and sum of Present Days, WeekOff Days, Leave Days, Holidays & Absent Days" + "(" + Days_Mismatch + ")')", true);
                }
            }
            else
            {
                gvSelected.DataSource = null;
                gvSelected.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Insertedrowcount + " Row Inserted')", true);
                Div_showdata.Style.Add("display", "none");
                div_Grid.Style.Add("display", "none");                
            }
        }
    }

    public void SetAllId()
    {
        //objEmpDetail.IsActive = true.ToString();
        //objAttLog.CreatedBy = Session["UserId"].ToString();
        //objAttLog.CreatedDate = DateTime.Now.ToString();

    }

    protected void btnUpload_Click2(object sender, EventArgs e)
    {
        string query = "";
        //// get columns name
        DataTable dtSource = (DataTable)Session["SourceData"];

        DataTable dtDestTemp = new DataTable();
        for (int col = 0; col < gvFieldMapping.Rows.Count; col++)
        {
            if (((DropDownList)gvFieldMapping.Rows[col].FindControl("ddlExcelCol")).SelectedValue != "0")
            {
                dtDestTemp.Columns.Add(((Label)gvFieldMapping.Rows[col].FindControl("lblColName")).Text);
            }
        }

        for (int rowcountr = 0; rowcountr < dtSource.Rows.Count; rowcountr++)
        {
            dtDestTemp.Rows.Add(dtDestTemp.NewRow());

            for (int i = 0; i < gvFieldMapping.Rows.Count; i++)
            {
                if (((DropDownList)gvFieldMapping.Rows[i].FindControl("ddlExcelCol")).SelectedValue != "0")
                {
                    dtDestTemp.Rows[rowcountr][((Label)gvFieldMapping.Rows[i].FindControl("lblColName")).Text] = dtSource.Rows[rowcountr][((DropDownList)gvFieldMapping.Rows[i].FindControl("ddlExcelCol")).SelectedValue].ToString();
                }
            }
        }
        //EEmpFirstName','DOJ','DepartmentId','DesignationId','DOB','BrandId','LocationId','EmpId
        if (dtDestTemp.Columns.Contains("Emp_Id") && dtDestTemp.Columns.Contains("Month") && dtDestTemp.Columns.Contains("Year") && dtDestTemp.Columns.Contains("Late_Min") && dtDestTemp.Columns.Contains("Present_Days") && dtDestTemp.Columns.Contains("Holiday_Days") && dtDestTemp.Columns.Contains("Absent_Days") && dtDestTemp.Columns.Contains("Leave_Days") && dtDestTemp.Columns.Contains("WeekOff_Days") && dtDestTemp.Columns.Contains("Early_Min") && dtDestTemp.Columns.Contains("Partial_Min") && dtDestTemp.Columns.Contains("Normal_OTMin") && dtDestTemp.Columns.Contains("WeekOff_OTMin") && dtDestTemp.Columns.Contains("Holiday_OTMin"))
        {
            ddlFiltercol.DataSource = dtDestTemp.Columns;
        }
        else
        {
            DisplayMessage("Map all Necessary Field");
            return;
        }

        Div_showdata.Style.Add("display", "");
        pnlUpload1.Visible = false;
        div_Grid.Style.Add("display", "none");
        //ddlFiltercol.DataTextField = "Column_Name";
        //ddlFiltercol.DataValueField = "Column_Name";
        ddlFiltercol.DataBind();

        Session["dtDest"] = dtDestTemp;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvSelected, dtDestTemp, "", "");
    }
    protected void btnresetgv_Click(object sender, EventArgs e)
    {
        Div_showdata.Style.Add("display", "none");
        div_Grid.Style.Add("display", "");
        pnlUpload1.Visible = false;

        txtfiltercol.Text = "";

        //btnUpload_Click(null, null);

        // trnew.Visible = false;

    }
    protected void btnBackToMapData_Click(object sender, EventArgs e)
    {
        Div_showdata.Style.Add("display", "none");
        pnlUpload1.Visible = true;
        div_Grid.Style.Add("display", "none");
        ddlTables.Items.Clear();
        //trmap.Visible = false;
        //trnew.Visible = false;

    }
    protected void gvFieldMapping_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string nec = gvFieldMapping.DataKeys[e.Row.RowIndex]["Nec"].ToString();
            if (nec.Trim() == "1")
            {
                ((Label)e.Row.FindControl("lblCompulsery")).Text = "*";
                ((Label)e.Row.FindControl("lblCompulsery")).ForeColor = System.Drawing.Color.Red;
            }
            DropDownList ddl = ((DropDownList)e.Row.FindControl("ddlExcelCol"));
            binddropdownlist(ddl);
        }
    }
    private void binddropdownlist(DropDownList ddl)
    {
        DataTable dt = (DataTable)Session["SourceTbl"];

        string filetype = Session["filetype"].ToString();
        int startingrow = 0;
        if (filetype == "excel")
            startingrow = 1;
        ListItem lst = new ListItem("--select one--", "0");

        if (ddl != null)
        {
            ddl.Items.Insert(0, lst);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst = new ListItem(dt.Rows[i]["COLUMN_NAME"].ToString(), dt.Rows[i]["COLUMN"].ToString());
                ddl.Items.Insert(i + 1, lst);
                //lst=new ListItem()
            }
        }
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtDest"];
        dt = new DataView(dt, "" + ddlFiltercol.SelectedValue + "='" + txtfiltercol.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvSelected, dt, "", "");
    }
    # endregion

    protected void gvEmpDetailMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmpDetailMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_HR_Emp_Dtls"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpDetailMaster, dt, "", "");
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        FillGrid();
        FillGridBin();

        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 1;
        txtValue.Text = "";
        txtValue.Focus();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtFilter_HR_Emp_Dtls"];

            if (dtCust!=null && dtCust.Rows.Count > 0)
            {
                DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
                Session["dtFilter_HR_Emp_Dtls"] = view.ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmpDetailMaster, view.ToTable(), "", "");
                if (gvEmpDetailMaster.Rows.Count > 0)
                    btnAllPost.Visible = true;
                else btnAllPost.Visible = false;
            }

        }
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtbinFilter"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpDetailBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                ////imgBtnRestore.Visible = false;
                ////ImgbtnSelectAll.Visible = false;
            }
            else
            {
                // AllPageCode();
            }
            txtbinValue.Focus();
        }
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 1;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void gvEmpDetailMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_HR_Emp_Dtls"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_HR_Emp_Dtls"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpDetailMaster, dt, "", "");
    }
    protected void gvEmpDetailBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvEmpDetailBin.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpDetailBin, dt, "", "");
        }
        string temp = string.Empty;
        bool isselcted;



    }
    protected void gvEmpDetailBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        string FileTypeid = "0";
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = Session["dtbinFilter"] as DataTable;
        // dt = objInterview.GetAllFalseRecord();

        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpDetailBin, dt, "", "");
        //AllPageCode();
    }
    private void Reset()
    {


        Edit_ID.Value = "";
        btnReset.Text = "Reset";
        txtEmpName.Text = "";
        ddlMonth.SelectedValue = "0";
        TxtYear.Text = "";
        txtMonthDays.Text = "";
        txtPresentDays.Text = "";
        txtHoliday.Text = "";
        txtWeekoffDays.Text = "";
        txtAbsentDays.Text = "";
        txtLeaveDays.Text = "";
        txtLatePenaltyMin.Text = "";
        txtEarlyPenaltyMin.Text = "";
        txtPartialPenaltyMin.Text = "";
        txtNormalOTMin.Text = "";
        txtWeekOffOTMin.Text = "";
        txtHolidayOTMin.Text = "";
        btnNew_Click(null, null);
        lblNonPost.Text = string.Empty;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmpName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        //ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        //dt = new DataView(dt, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Name"].ToString()+"/"+ dt.Rows[i]["Emp_Code"].ToString();
        }
        return txt;
    }

    protected string GetMonth(string strMonthId)
    {
        string strMonth = string.Empty;
        if (strMonthId == "1")
        {
            strMonth = "January";
        }
        else if (strMonthId == "2")
        {
            strMonth = "February";
        }
        else if (strMonthId == "3")
        {
            strMonth = "March";
        }
        else if (strMonthId == "4")
        {
            strMonth = "April";
        }
        else if (strMonthId == "5")
        {
            strMonth = "May";
        }
        else if (strMonthId == "6")
        {
            strMonth = "June";
        }
        else if (strMonthId == "7")
        {
            strMonth = "July";
        }
        else if (strMonthId == "8")
        {
            strMonth = "August";
        }
        else if (strMonthId == "9")
        {
            strMonth = "September";
        }
        else if (strMonthId == "10")
        {
            strMonth = "October";
        }
        else if (strMonthId == "11")
        {
            strMonth = "November";
        }
        else if (strMonthId == "12")
        {
            strMonth = "December";
        }
        else if (strMonthId == "")
        {
            strMonth = "";
        }
        return strMonth;
    }

    protected void FileUploadComplete(object sender, EventArgs e)
    {
        //string filename = System.IO.Path.GetFileName(fileLoad.FileName);
        //fileLoad.SaveAs(Server.MapPath(this.UploadFolderPath) + filename);

        int fileType = -1;

        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }
                string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + fileLoad.FileName);
                fileLoad.SaveAs(path);
                Import(path, fileType);
            }
        }
    }
}
