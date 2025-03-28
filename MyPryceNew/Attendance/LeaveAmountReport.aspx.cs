using PegasusDataAccess;
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Attendance_LeaveAmountReport : System.Web.UI.Page
{
    Common cmn = null;
    SystemParameter objSys = null;
    LeaveAmountReport objAmountReport = null;
    PageControlCommon objPageCmn = null;
    DataAccessClass ObjDa = null;
    int BreakMin = 0;
    int RelexationMin = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        //AllPageCode();

        objAmountReport = new LeaveAmountReport(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Attendance/TimeTable.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);

            FillYear(ddlYear);

            try
            {
                string year = DateTime.Now.Year.ToString();
                ddlYear.SelectedValue = year;
            }
            catch (Exception ex)
            {

            }

            // fillLocation();
            txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;

            FillGrid();
            btnList_Click(null, null);
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        //imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    private void FillYear(DropDownList ddl)
    {
        DataTable dt = new DataTable();
        dt = ObjDa.return_DataTable("SELECT DISTINCT YEAR AS Unique_Years FROM LeaveAmountReport");
        if (dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddl, dt, "Unique_Years", "Unique_Years");
        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        ddlFieldName.Focus();
        Session["CHECKED_ITEMS"] = null;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        ddlFieldName.Focus();
        ddlYear.SelectedValue = DateTime.Now.Year.ToString();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        if (ddlYear.SelectedItem.Text == "--Select--" || ddlYear.SelectedItem.Text == "")
        {
            DisplayMessage("Please Get Select The Year Value");
            return;
        }

        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (txtValue.Text.Trim().ToString() == "")
        {
            DisplayMessage("Please Fill Value");
            txtValue.Focus();
            return;
        }
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
            DataTable dtCust = (DataTable)Session["LeaveAmount"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_LeaveAmount"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvLeaveAmountReport, view.ToTable(), "", "");

            if (gvLeaveAmountReport.Rows.Count > 0)
            {
                gvLeaveAmountReport.HeaderRow.Cells[6].Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }

            //AllPageCode();
            txtValue.Focus();
        }
    }

    //protected void gvLeaveAmountReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvLeaveAmountReport.PageIndex = e.NewPageIndex;
    //    DataTable dt = (DataTable)Session["dtFilter_LeaveAmount"];
    //    //Common Function add By Lokesh on 21-05-2015
    //    objPageCmn.FillData((object)gvLeaveAmountReport, dt, "", "");

    //    if (gvLeaveAmountReport.Rows.Count > 0)
    //    {
    //        gvLeaveAmountReport.HeaderRow.Cells[5].Text = DateTime.Now.ToString("dd-MMM-yyyy");
    //    }
    //    //AllPageCode();
    //}
    protected void gvLeaveAmountReport_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_LeaveAmount"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_LeaveAmount"] = dt;
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvLeaveAmountReport, dt, "", "");
        //AllPageCode();
        gvLeaveAmountReport.HeaderRow.Focus();

        if (gvLeaveAmountReport.Rows.Count > 0)
        {
            gvLeaveAmountReport.HeaderRow.Cells[6].Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListTimeTableName(string prefixText, int count, string contextKey)
    {
        Att_TimeTable objAtt_TimeTable = new Att_TimeTable(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objAtt_TimeTable.GetTimeTableMaster(HttpContext.Current.Session["CompId"].ToString()), "TimeTable_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        // Modified By Nitin On 10/11/2014 : Filter Time Table On Brand Bases
        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["TimeTable_Name"].ToString();
        }
        return txt;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        return;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        btnList_Click(null, null);
        //Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    public void FillGrid()
    {
      

        DataTable dt = new DataTable();
        dt = objAmountReport.GetFirstDataforEmployee(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, "1");

        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvLeaveAmountReport, dt, "", "");
            //AllPageCode();
            Session["dtFilter_LeaveAmount"] = dt;
            Session["LeaveAmount"] = dt;

            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

            if (gvLeaveAmountReport.Rows.Count > 0)
            {
                gvLeaveAmountReport.HeaderRow.Cells[6].Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }


        //if (ddlYear.SelectedValue == DateTime.Now.Year.ToString())
        //{
            GetUpdateData();
       // }
        dt = objAmountReport.GetFirstDataforEmployee(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlYear.SelectedValue, "2");

        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvLeaveAmountReport, dt, "", "");
            //AllPageCode();
            Session["dtFilter_LeaveAmount"] = dt;
            Session["LeaveAmount"] = dt;

            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

            if (gvLeaveAmountReport.Rows.Count > 0)
            {
                gvLeaveAmountReport.HeaderRow.Cells[6].Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
    }

    public void GetUpdateData()
    {
        DateTime Today = DateTime.Now;
        if (gvLeaveAmountReport.Rows.Count > 0)
        {
            try
            {
                foreach (GridViewRow gvr in gvLeaveAmountReport.Rows)
                {
                    Label lblEmpName = (Label)gvr.FindControl("lblEmployees");
                    Label lblDoj = (Label)gvr.FindControl("lblDOJ");
                    Label lblBasicSalary = (Label)gvr.FindControl("lblBasicSalary");
                    Label lblRate = (Label)gvr.FindControl("lblRate");
                    TextBox lblDays = (TextBox)gvr.FindControl("lblDays");
                    TextBox lblAbsentDays = (TextBox)gvr.FindControl("lblAbsentDays");
                    TextBox lblNetLeaveDays = (TextBox)gvr.FindControl("lblNetLeaveDays");
                    TextBox lblLeaveYears = (TextBox)gvr.FindControl("lblLeaveYears");
                    TextBox lblLDG = (TextBox)gvr.FindControl("lblLDG");
                    TextBox lblLeaveDays = (TextBox)gvr.FindControl("lblLeaveDays");
                    TextBox lblOTLeaveDays = (TextBox)gvr.FindControl("lblOTLeaveDays");
                    TextBox lblTotalLeaveDays = (TextBox)gvr.FindControl("lblTotalLeaveDays");
                    TextBox lblDaysPreviousYearsPaid = (TextBox)gvr.FindControl("lblDaysPreviousYearsPaid");
                    TextBox lblDueDaysBeforeTakingLeaveThisYear = (TextBox)gvr.FindControl("lblDueDaysBeforeTakingLeaveThisYear");
                    TextBox lblCurrentYearDaysPaid = (TextBox)gvr.FindControl("lblCurrentYearDaysPaid");
                    TextBox lblDueDays = (TextBox)gvr.FindControl("lblDueDays");
                    TextBox lblLeaveDuesPaidinCurrentYear = (TextBox)gvr.FindControl("lblLeaveDuesPaidinCurrentYear");
                    TextBox lblLeavesAMTPayable = (TextBox)gvr.FindControl("lblLeavesAMTPayable");
                    TextBox lblLeaveTillLastYear = (TextBox)gvr.FindControl("lblLeaveTillLastYear");
                    TextBox lblCurrentYearLeave = (TextBox)gvr.FindControl("lblCurrentYearLeave");

                    int i = 0;
                    string Emp_Id = ObjDa.get_SingleValue("Select Emp_Id from Set_EmployeeMaster Where Emp_Name = '" + lblEmpName.Text + "' And IsActive = '1'");

                    DataTable dtData = ObjDa.return_DataTable("Select * from LeaveAmountReport where Emp_Id='" + Emp_Id + "' and Year='" + Today.Year.ToString() + "'");
                    if (dtData.Rows.Count > 0)
                    {
                        i = objAmountReport.UpdateLeaveAmountReport(Emp_Id, lblDoj.Text, lblBasicSalary.Text, lblRate.Text, lblDays.Text, lblAbsentDays.Text, lblNetLeaveDays.Text, lblLeaveYears.Text, lblLDG.Text, lblTotalLeaveDays.Text, lblDaysPreviousYearsPaid.Text, lblDueDaysBeforeTakingLeaveThisYear.Text, lblDueDays.Text, lblLeaveDuesPaidinCurrentYear.Text, lblLeavesAMTPayable.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), lblLeaveDays.Text, lblOTLeaveDays.Text, Today.Year.ToString(), lblLeaveTillLastYear.Text, lblCurrentYearLeave.Text);
                    }
                    else
                    {
                        i = objAmountReport.InserLeaveAmountReport(Emp_Id, lblDoj.Text, lblBasicSalary.Text, lblRate.Text, lblDays.Text, lblAbsentDays.Text, lblNetLeaveDays.Text, lblLeaveYears.Text, lblLDG.Text, lblTotalLeaveDays.Text, lblDaysPreviousYearsPaid.Text, lblDueDaysBeforeTakingLeaveThisYear.Text, lblCurrentYearDaysPaid.Text, lblDueDays.Text, lblLeaveDuesPaidinCurrentYear.Text, lblLeavesAMTPayable.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), lblLeaveDays.Text, lblOTLeaveDays.Text, Today.Year.ToString(), lblLeaveTillLastYear.Text, lblCurrentYearLeave.Text);
                    }
                }
                // DisplayMessage("Records Update");
            }
            catch (Exception ex)
            {
                // DisplayMessage("Records not Update");
            }
        }
    }
    //protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    FillGrid();
    //   // I1.Attributes.Add("Class", "fa fa-minus");
    //    Div1.Attributes.Add("Class", "box box-primary");
    //}

    //Code Created By Rahul Sharma Date 06-11-2023 
    #region Save & Cancel or Print Report
    protected void btnSaveReport_Click(object sender, EventArgs e)
    {
        DateTime Today = DateTime.Now;
        if (gvLeaveAmountReport.Rows.Count > 0)
        {
            try
            {
                foreach (GridViewRow gvr in gvLeaveAmountReport.Rows)
                {
                    Label lblEmpName = (Label)gvr.FindControl("lblEmployees");
                    TextBox lblCurrentYearDaysPaid = (TextBox)gvr.FindControl("lblCurrentYearDaysPaid");
                    TextBox lblDueDays = (TextBox)gvr.FindControl("lblDueDays");
                    TextBox lblLeaveDuesPaidinCurrentYear = (TextBox)gvr.FindControl("lblLeaveDuesPaidinCurrentYear");
                    TextBox lblLeavesAMTPayable = (TextBox)gvr.FindControl("lblLeavesAMTPayable");

                    int i = 0;
                    string Emp_Id = ObjDa.get_SingleValue("Select Emp_Id from Set_EmployeeMaster Where Emp_Name = '" + lblEmpName.Text + "' And IsActive = '1'");

                    DataTable dtData = ObjDa.return_DataTable("Select * from LeaveAmountReport where Emp_Id='" + Emp_Id + "' and Year='" + Today.Year.ToString() + "'");
                    if (dtData.Rows.Count > 0)
                    {
                        ObjDa.execute_Command("update LeaveAmountReport set CurrentYearDaysPaid='" + lblCurrentYearDaysPaid.Text + "',DueDays='" + lblDueDays.Text + "', LeaveDuesPaidInCurrentYear='" + lblLeaveDuesPaidinCurrentYear.Text + "', LeaveAMTPayable='" + lblLeavesAMTPayable.Text + "' where Emp_Id='" + Emp_Id + "' and Year='" + Today.Year.ToString() + "'");
                    }
                }
                DisplayMessage("Records Update");    
            }
            catch (Exception ex)
            {
                // DisplayMessage("Records not Update");
            }
        }
    }

    protected void btnCancelReport_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnPrintReporrt_Click(object sender, EventArgs e)
    {
        btnSaveReport_Click(null, null);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/LeaveAmountReport_Print.aspx?Trans_Id=0','','height=650,width=950,scrollbars=Yes')", true);
    }


    #endregion

    //Code Created By Rahul Sharma Date 07-11-2023
    #region Grid Text Change Calculation
    protected void txtAmount_Change(object sender, EventArgs e)
    {
        //if (gvLeaveAmountReport.Rows.Count > 0)
        //{
        //    foreach (GridViewRow gvr in gvLeaveAmountReport.Rows)
        //    {
        //        TextBox TotalDays = (TextBox)gvr.FindControl("lblDays");
        //        TextBox AbsentDays = (TextBox)gvr.FindControl("lblAbsentDays");
        //        TextBox lblNetLeaveDays = (TextBox)gvr.FindControl("lblNetLeaveDays");
        //        TextBox lblLeaveYears = (TextBox)gvr.FindControl("lblLeaveYears");
        //        TextBox lblLDG = (TextBox)gvr.FindControl("lblLDG");
        //        TextBox lblTotalLeaveDays = (TextBox)gvr.FindControl("lblTotalLeaveDays");
        //        TextBox lblDaysPreviousYearsPaid = (TextBox)gvr.FindControl("lblDaysPreviousYearsPaid");
        //        TextBox lblDueDaysBeforeTakingLeaveThisYear = (TextBox)gvr.FindControl("lblDueDaysBeforeTakingLeaveThisYear");

        //        Label lblRate = (Label)gvr.FindControl("lblRate");
        //        TextBox lblCurrentYearDaysPaid = (TextBox)gvr.FindControl("lblCurrentYearDaysPaid");
        //        TextBox lblLeaveDuesPaidinCurrentYear = (TextBox)gvr.FindControl("lblLeaveDuesPaidinCurrentYear");



        //        float netdays = float.Parse(TotalDays.Text) - float.Parse(AbsentDays.Text);
        //        lblNetLeaveDays.Text = netdays.ToString();

        //        float totalleavedays = float.Parse(lblLeaveYears.Text) * float.Parse(lblLDG.Text);
        //        lblTotalLeaveDays.Text = totalleavedays.ToString();

        //        float takingLeavethisyear = float.Parse(lblTotalLeaveDays.Text) - float.Parse(lblDaysPreviousYearsPaid.Text);
        //        lblDueDaysBeforeTakingLeaveThisYear.Text = takingLeavethisyear.ToString();


        //        float DuePaidInCurrentYear = float.Parse(lblRate.Text) * float.Parse(lblCurrentYearDaysPaid.Text);
        //        lblLeaveDuesPaidinCurrentYear.Text = DuePaidInCurrentYear.ToString();

        //    }
        //}


        if (gvLeaveAmountReport.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in gvLeaveAmountReport.Rows)
            {
                TextBox lblCurrentYearDaysPaid = (TextBox)gvr.FindControl("lblCurrentYearDaysPaid");
                TextBox lblDueDaysBeforeTakingLeaveThisYear = (TextBox)gvr.FindControl("lblDueDaysBeforeTakingLeaveThisYear");
                TextBox lblDueDays = (TextBox)gvr.FindControl("lblDueDays");
                TextBox lblLeaveDuesPaidinCurrentYear = (TextBox)gvr.FindControl("lblLeaveDuesPaidinCurrentYear");
                Label lblRate = (Label)gvr.FindControl("lblRate");
                TextBox lblLeavesAMTPayable = (TextBox)gvr.FindControl("lblLeavesAMTPayable");


                float duedays = float.Parse(lblDueDaysBeforeTakingLeaveThisYear.Text) - float.Parse(lblCurrentYearDaysPaid.Text);
                lblDueDays.Text = duedays.ToString();

                float paidincurrentyear = float.Parse(lblCurrentYearDaysPaid.Text) * float.Parse(lblRate.Text);
                lblLeaveDuesPaidinCurrentYear.Text = paidincurrentyear.ToString();

                float amountpayable = float.Parse(lblDueDays.Text) * float.Parse(lblRate.Text);
                lblLeavesAMTPayable.Text = amountpayable.ToString();
            }
        }

        if (gvLeaveAmountReport.Rows.Count > 0)
        {
            gvLeaveAmountReport.HeaderRow.Cells[6].Text = DateTime.Now.ToString("dd-MMM-yyyy");
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "SetScrollPosition", "setScrollPositionAfterPostback();", true);
    }
    #endregion


}