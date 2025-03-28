using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Attendance_TimeTable : BasePage
{
    Common cmn = null;
    Att_AttendanceLog objattlog = null;
    SystemParameter objSys = null;
    Att_TimeTable objTimeTable = null;
    Att_ShiftDescription objShiftDesc = null;
    Att_ScheduleMaster objSch = null;
    Set_ApplicationParameter objAppParam = null;
    PageControlCommon objPageCmn = null;
    int BreakMin = 0;
    int RelexationMin = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        //AllPageCode();

        cmn = new Common(Session["DBConnection"].ToString());
        objattlog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objTimeTable = new Att_TimeTable(Session["DBConnection"].ToString());
        objShiftDesc = new Att_ShiftDescription(Session["DBConnection"].ToString());
        objSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

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
            objPageCmn.fillLocation(ddlLocNew);
            // fillLocation();
            txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;
            FillGridBin();
            FillGrid();
            btnList_Click(null, null);
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        //imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    protected void txtTimeTableName_OnTextChanged(object sender, EventArgs e)
    {

        if (editid.Value == "")
        {
            DataTable dt = objTimeTable.GetTimeTableMasterByTimeTableName(Session["CompId"].ToString(), txtTimeTableName.Text.Trim());
            // Modified By Nitin On 10/11/2014 : Filter Time Table On Brand Bases
            dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                txtTimeTableName.Text = "";
                DisplayMessage("TimeTable Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTimeTableName);
                return;
            }
            DataTable dt1 = objTimeTable.GetTimeTableMasterInactive(Session["CompId"].ToString().ToString());
            // Modified By Nitin On 10/11/2014 : FIlter TIme Table On Brand Bases
            dt1 = new DataView(dt1, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            dt1 = new DataView(dt1, "TimeTable_Name='" + txtTimeTableName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtTimeTableName.Text = "";
                DisplayMessage("TimeTable Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTimeTableName);
                return;
            }
            txtTimeTableNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString().ToString(), editid.Value);
            // Modified By Nitin On 10/11/2014 : FIlter TIme Table On Brand Bases
            dtTemp = new DataView(dtTemp, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["TimeTable_Name"].ToString() != txtTimeTableName.Text)
                {
                    DataTable dt = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString().ToString());
                    // Modified By Nitin On 10/11/2014 : Filter Time Table On Brand Bases
                    dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    dt = new DataView(dt, "TimeTable_Name='" + txtTimeTableName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtTimeTableName.Text = "";
                        DisplayMessage("TimeTable Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTimeTableName);
                        return;
                    }
                    DataTable dt1 = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString().ToString());
                    // Modified By Nitin On 10/11/2014 : FIlter TIme Table On Brand Bases
                    dt1 = new DataView(dt1, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    dt1 = new DataView(dt1, "TimeTable_Name='" + txtTimeTableName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtTimeTableName.Text = "";
                        DisplayMessage("TimeTable Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTimeTableName);
                        return;
                    }
                }
            }
            txtTimeTableNameL.Focus();
        }
    }

    protected void txtBreakMin_OnTextChanged(object sender, EventArgs e)
    {
        if (txtOffDutyTime.Text == "" || txtOffDutyTime.Text == "__:__:__" || txtOffDutyTime.Text == "00:00:00")
        {
            txtWorkMinute.Text = "0";
            return;
        }

        if (txtBreakMinute.Text == "")
        {
            txtBreakMinute.Text = "0";
            BreakMin = 0;
        }
        else
        {
            BreakMin = Convert.ToInt32(txtBreakMinute.Text);
        }
        if (txtRelaxMin.Text == "")
        {
            txtRelaxMin.Text = "0";
            RelexationMin = 0;
        }
        else
        {
            RelexationMin = Convert.ToInt32(txtRelaxMin.Text);
        }

        if (int.Parse(txtBreakMinute.Text) >= GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) && GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) > 0)
        {
            DisplayMessage("Break Minutes Cannot be Greater than or Equal to Work Minutes");
            txtBreakMinute.Text = "0";
            txtBreakMinute.Focus();
            txtWorkMinute.Text = (GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) - int.Parse(txtBreakMinute.Text) - int.Parse(txtRelaxMin.Text)).ToString();

            return;
        }
        else
        {

            txtWorkMinute.Text = (GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) - int.Parse(txtBreakMinute.Text) - int.Parse(txtRelaxMin.Text)).ToString();
            if (Convert.ToInt32(txtWorkMinute.Text) <= 0 && GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) > 0)
            {
                DisplayMessage("Break Minutes Cannot be Greater than or Equal to Work Minutes");
                txtBreakMinute.Text = "0";
                txtBreakMinute.Focus();
                txtWorkMinute.Text = (GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) - int.Parse(txtBreakMinute.Text) - int.Parse(txtRelaxMin.Text)).ToString();
                return;
            }
            else
            {
                // txtWorkMinute.Text = Convert.ToString(Convert.ToInt32(txtWorkMinute.Text) - BreakMin + RelexationMin);
            }
            // ViewState["TotalWorkMin"] = txtWorkMinute.Text;
        }
        txtWorkMinute.Focus();
    }

    protected void txtRelaxmin_OnTextChanged(object sender, EventArgs e)
    {
        if (txtOffDutyTime.Text == "" || txtOffDutyTime.Text == "__:__:__")
        {
            txtWorkMinute.Text = "";
            return;

        }
        if (txtBreakMinute.Text == "")
        {
            txtBreakMinute.Text = "0";
            BreakMin = 0;
        }
        else
        {
            BreakMin = Convert.ToInt32(txtBreakMinute.Text);
        }
        if (txtRelaxMin.Text == "")
        {
            txtRelaxMin.Text = "0";
            RelexationMin = 0;
        }
        else
        {
            RelexationMin = Convert.ToInt32(txtRelaxMin.Text);
        }

        if (int.Parse(txtBreakMinute.Text) >= GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text))
        {
            DisplayMessage("Break Minutes Cannot be Greater than or Equal to Work Minutes");
            txtBreakMinute.Text = "0";
            txtWorkMinute.Text = (GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) - int.Parse(txtBreakMinute.Text) - int.Parse(txtRelaxMin.Text)).ToString();

            return;
        }
        else
        {
            txtWorkMinute.Text = (GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) - int.Parse(txtBreakMinute.Text) - int.Parse(txtRelaxMin.Text)).ToString();
            if (Convert.ToInt32(txtWorkMinute.Text) <= 0)
            {
                txtWorkMinute.Text = Convert.ToString(BreakMin + RelexationMin);
                DisplayMessage("Break Minutes Cannot be Greater than or Equal to Work Minutes");
                txtRelaxMin.Text = "0";
                txtWorkMinute.Text = (GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) - int.Parse(txtBreakMinute.Text) - int.Parse(txtRelaxMin.Text)).ToString();
                return;
            }
        }
    }

    protected void txtOnDutyTime_OnTextChanged(object sender, EventArgs e)
    {
        if (txtOnDutyTime.Text == "")
        {
            txtOnDutyTime.Text = "00:00:00";
        }
        txtBreakMin_OnTextChanged(null, null);
        txtOffDutyTime.Text = "00:00:00";
        txtWorkMinute.Text = "0";
        txtOffDutyTime.Focus();
    }

    protected void txtOffDutyTime_OnTextChanged(object sender, EventArgs e)
    {

        if (txtOffDutyTime.Text == "")
        {
            txtOffDutyTime.Text = "00:00:00";
        }
        if (txtOnDutyTime.Text == "")
        {
            txtOnDutyTime.Text = "00:00:00";
        }
        if (txtRelaxMin.Text.Trim().ToString() == "")
        {
            txtRelaxMin.Text = "0";
        }
        if (txtBreakMinute.Text.Trim().ToString() == "0" || txtBreakMinute.Text.Trim().ToString() == "")
        {
            txtRelaxMin.Text = "0";
        }
        txtWorkMinute.Text = (GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) - int.Parse(txtBreakMinute.Text == "" ? "0" : txtBreakMinute.Text) - int.Parse(txtRelaxMin.Text)).ToString();
        ViewState["TotalWorkMin"] = txtWorkMinute.Text;
        txtBreakMin_OnTextChanged(null, null);

    }
    private int GetMinuteDiff(string greatertime, string lesstime)
    {

        if (greatertime == "__:__:__" || greatertime == "" || greatertime == "00:00:00")
        {
            return 0;
        }
        if (lesstime == "__:__:__" || lesstime == "" || lesstime == "00:00:00")
        {
            return 0;
        }
        int retval = 0;
        int actTimeHour = Convert.ToInt32(greatertime.Split(':')[0]);



        int ondutyhour = Convert.ToInt32(lesstime.Split(':')[0]);


        int actTimeMinute = Convert.ToInt32(greatertime.Split(':')[1]);
        int ondutyMinute = Convert.ToInt32(lesstime.Split(':')[1]);
        int totalActTimeMinute = actTimeHour * 60 + actTimeMinute;
        int totalOnDutyTimeMinute = ondutyhour * 60 + ondutyMinute;
        if (totalActTimeMinute - totalOnDutyTimeMinute < 0)
        {
            retval = 1440 + (totalActTimeMinute - totalOnDutyTimeMinute);
            //retval = 0;
        }
        else
        {
            retval = (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        return retval;
    }

    public bool CheckValid(TextBox txt, string Error_messagevalue)
    {

        if (txt.Text == "")
        {

            DisplayMessage(Error_messagevalue);
            txt.Focus();
            return false;
        }
        else if (txt.Text == "__:__:__")
        {
            DisplayMessage(Error_messagevalue);
            txt.Focus();
            return false;
        }
        else if (txt.Text == "00:00:00")
        {
            DisplayMessage(Error_messagevalue);
            txt.Focus();
            return false;
        }

        else
        {
            return true;
        }

    }
    public bool Check24Format(TextBox txt, string ErrorMessage)
    {

        if (!System.Text.RegularExpressions.Regex.IsMatch(txt.Text.ToString(), "^((0?[1-9]|1[012])(:[0-5]\\d){0,2}(\\ [AP]M))$|^([01]\\d|2[0-3])(:[0-5]\\d){0,2}$"))
        {
            DisplayMessage(ErrorMessage);
            txt.Focus();
            return false;
        }
        else
        {
            return true; ;
        }



    }
    protected void btnView_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();

        DataTable dt = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {

            txtTimeTableName.Text = dt.Rows[0]["TimeTable_Name"].ToString();

            txtTimeTableNameL.Text = dt.Rows[0]["TimeTable_Name_L"].ToString();


            txtOnDutyTime.Text = dt.Rows[0]["OnDuty_Time"].ToString();


            txtOffDutyTime.Text = dt.Rows[0]["OffDuty_Time"].ToString();
            txtBeginingIn.Text = dt.Rows[0]["Beginning_In"].ToString();
            txtBeginingOut.Text = dt.Rows[0]["Beginning_Out"].ToString();
            txtEndingIn.Text = dt.Rows[0]["Ending_In"].ToString();
            txtEndingOut.Text = dt.Rows[0]["Ending_Out"].ToString();
            txtWorkMinute.Text = dt.Rows[0]["Work_Minute"].ToString();
            txtBreakMinute.Text = dt.Rows[0]["Break_Min"].ToString();

            txtRelaxMin.Text = dt.Rows[0]["Field1"].ToString();
            txtRelaxMin.ReadOnly = true;

            txtTimeTableName.ReadOnly = true;

            txtTimeTableNameL.ReadOnly = true;


            txtOnDutyTime.ReadOnly = true;


            txtOffDutyTime.ReadOnly = true;
            txtBeginingIn.ReadOnly = true;
            txtBeginingOut.ReadOnly = true;
            txtEndingIn.ReadOnly = true;
            txtEndingOut.ReadOnly = true;
            txtWorkMinute.ReadOnly = true;
            txtBreakMinute.ReadOnly = true;

            btnSave.Visible = false;
            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.View;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        ddlFieldName.Focus();
        Session["CHECKED_ITEMS"] = null;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtTimeTableName.Focus();
        Session["CHECKED_ITEMS"] = null;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        Session["CHECKED_ITEMS"] = null;
        FillGridBin();
        ddlbinFieldName.Focus();
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
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
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
            DataTable dtCust = (DataTable)Session["TimeTable"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Time_T"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvTimeTable, view.ToTable(), "", "");
            //AllPageCode();
            txtValue.Focus();
        }
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
            DataTable dtCust = (DataTable)Session["dtbinTimeTable"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvTimeTableBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        ddlbinFieldName.Focus();
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvTimeTableBin.Rows)
        {
            index = (int)gvTimeTableBin.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvTimeTableBin.Rows)
            {
                int index = (int)gvTimeTableBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvTimeTableBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvTimeTableBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];

        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvTimeTableBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValues();
    }
    protected void gvTimeTableBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objTimeTable.GetTimeTableMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvTimeTableBin, dt, "", "");
        //AllPageCode();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (txtTimeTableName.Text == "")
        {
            DisplayMessage("Enter TimeTable Name");
            txtTimeTableName.Focus();
            return;
        }

        if (!CheckValid(txtOnDutyTime, "On Duty Time Required"))
        {
            txtOnDutyTime.Focus();
            return;
        }

        if (txtOTMin.Text == "")
        {
            txtOTMin.Text = "0";
        }


        if (!CheckValid(txtOffDutyTime, "Off Duty Time Required"))
        {
            txtOffDutyTime.Focus();
            return;
        }
        if (!CheckValid(txtBreakMinute, "Break Minute Required"))
        {
            txtBreakMinute.Focus();
            return;
        }
        if (!CheckValid(txtBeginingIn, "Beginning In Required"))
        {
            txtBeginingIn.Focus();
            return;
        }
        if (!CheckValid(txtEndingIn, "Ending In Required"))
        {
            txtEndingIn.Focus();
            return;
        }
        if (!CheckValid(txtBeginingOut, "Beginning Out Required"))
        {
            txtBeginingOut.Focus();
            return;
        }
        if (!CheckValid(txtEndingOut, "Ending Out Required"))
        {
            txtEndingOut.Focus();
            return;
        }
        if (!Check24Format(txtOnDutyTime, "Invalid Time ! Please Read * Note"))
        {
            txtOnDutyTime.Focus();
            return;
        }
        if (!Check24Format(txtOffDutyTime, "Invalid Time ! Please Read * Note"))
        {
            txtOffDutyTime.Focus();
            return;
        }

        if (!Check24Format(txtBeginingIn, "Invalid Time ! Please Read * Note"))
        {
            txtBeginingIn.Focus();
            return;
        }

        if (!Check24Format(txtEndingIn, "Invalid Time ! Please Read * Note"))
        {
            txtEndingIn.Focus();
            return;
        }

        if (!Check24Format(txtBeginingOut, "Invalid Time ! Please Read * Note"))
        {
            txtBeginingOut.Focus();
            return;
        }

        if (!Check24Format(txtEndingOut, "Invalid Time ! Please Read * Note"))
        {
            txtEndingOut.Focus();
            return;
        }

        DateTime beginingin = Convert.ToDateTime(txtBeginingIn.Text);
        DateTime ondutytime = Convert.ToDateTime(txtOnDutyTime.Text);

        DateTime endingin = Convert.ToDateTime(txtEndingIn.Text);
        DateTime endingout = Convert.ToDateTime(txtEndingOut.Text);

        DateTime beginingout = Convert.ToDateTime(txtBeginingOut.Text);
        DateTime offdutytime = Convert.ToDateTime(txtOffDutyTime.Text);

        DateTime offduty = Convert.ToDateTime(txtOffDutyTime.Text);

        int MinuteDiff = 0;
        string mindiff = (GetMinuteDiff(txtOffDutyTime.Text, txtOnDutyTime.Text) - int.Parse(txtBreakMinute.Text)).ToString();
        MinuteDiff = Convert.ToInt32(mindiff);

        string ShortestTime = objAppParam.GetApplicationParameterValueByParamName("Shortest Time Table", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int shorttime = int.Parse(ShortestTime);

        if (MinuteDiff < shorttime)
        {
            DisplayMessage("Work Minutes Cannot be Less than Shortest Time Table Duration (" + shorttime.ToString() + " Minutes)");
            return;
        }
        if (txtRelaxMin.Text == "")
        {
            txtRelaxMin.Text = "0";
        }

        if (txtLateRelaxation.Text == "")
        {
            txtLateRelaxation.Text = "0";
        }

        if (txtEarlyRelaxation.Text == "")
        {
            txtEarlyRelaxation.Text = "0";
        }

        if (editid.Value == "")
        {
            DataTable dt1 = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());

            dt1 = new DataView(dt1, "TimeTable_Name='" + txtTimeTableName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("TimeTable Name Already Exists");
                txtTimeTableName.Focus();
                return;

            }

            b = objTimeTable.InsertTimeTableMaster(Session["CompId"].ToString(), txtTimeTableName.Text, txtTimeTableNameL.Text, Session["BrandId"].ToString(), Convert.ToDateTime(txtOnDutyTime.Text), Convert.ToDateTime(txtOffDutyTime.Text), txtLateRelaxation.Text, txtEarlyRelaxation.Text, txtBeginingIn.Text, txtBeginingOut.Text, txtEndingIn.Text, txtEndingOut.Text, txtWorkMinute.Text.Trim(), txtBreakMinute.Text, false.ToString(), txtRelaxMin.Text, txtOTMin.Text, "", "",ddlLocNew.SelectedValue.Trim(), true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                FillGrid();
                Reset();
                btnList_Click(null, null);
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            string TimeTableTypeName = string.Empty;
            DataTable dt1 = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());
            try
            {
                TimeTableTypeName = new DataView(dt1, "TimeTable_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["TimeTable_Name"].ToString();
            }
            catch
            {
                TimeTableTypeName = "";
            }
            dt1 = new DataView(dt1, "TimeTable_Name='" + txtTimeTableName.Text + "' and TimeTable_Name<>'" + TimeTableTypeName + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("TimeTable Name Already Exists");
                txtTimeTableName.Focus();
                return;

            }
            b = objTimeTable.UpdateTimeTableMaster(editid.Value, Session["CompId"].ToString(), txtTimeTableName.Text, txtTimeTableNameL.Text, Session["BrandId"].ToString(), Convert.ToDateTime(txtOnDutyTime.Text).ToString(), Convert.ToDateTime(txtOffDutyTime.Text).ToString(), txtLateRelaxation.Text, txtEarlyRelaxation.Text, txtBeginingIn.Text, txtBeginingOut.Text, txtEndingIn.Text, txtEndingOut.Text, txtWorkMinute.Text.Trim(), txtBreakMinute.Text, false.ToString(), txtRelaxMin.Text, txtOTMin.Text, "", "", ddlLocNew.SelectedValue.Trim(), true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

            if (b != 0)
            {
                btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

            }
            else
            {
                DisplayMessage("Record Not Updated");
            }

        }
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Att_ScheduleMaster objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        editid.Value = e.CommandArgument.ToString();
        DataTable dtShift = objShiftDesc.GetShift_TimeTable();
        DataTable dt = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), editid.Value);
        // Modified By Nitin On 10/11/2014 : Filter Time Table On Brand Bases
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {

            txtTimeTableName.Text = dt.Rows[0]["TimeTable_Name"].ToString();
            txtTimeTableNameL.Text = dt.Rows[0]["TimeTable_Name_L"].ToString();
            txtOnDutyTime.Text = dt.Rows[0]["OnDuty_Time"].ToString();
            txtOTMin.Text = dt.Rows[0]["Field2"].ToString();
            txtOffDutyTime.Text = dt.Rows[0]["OffDuty_Time"].ToString();
            txtBeginingIn.Text = dt.Rows[0]["Beginning_In"].ToString();
            txtBeginingOut.Text = dt.Rows[0]["Beginning_Out"].ToString();
            txtEndingIn.Text = dt.Rows[0]["Ending_In"].ToString();
            txtEndingOut.Text = dt.Rows[0]["Ending_Out"].ToString();
            txtWorkMinute.Text = dt.Rows[0]["Work_Minute"].ToString();
            ViewState["TotalWorkMin"] = dt.Rows[0]["Work_Minute"].ToString();
            txtBreakMinute.Text = dt.Rows[0]["Break_Min"].ToString();
            ddlLocNew.SelectedValue = dt.Rows[0]["Field5"].ToString();
            hdnLoc_Id.Value = dt.Rows[0]["Field5"].ToString();
            txtLateRelaxation.Text = dt.Rows[0]["Late_Min"].ToString();
            txtEarlyRelaxation.Text = dt.Rows[0]["Early_Min"].ToString();
            txtRelaxMin.Text = dt.Rows[0]["Field1"].ToString();





            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }

    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        //this code is created by jitendra upadhyay on 10-09-2014
        //this code for check the table is used in log post record or not
        //code start

        Att_ScheduleMaster objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());

        DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription();
        dtShiftAllDate = new DataView(dtShiftAllDate, "TimeTable_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        if (dtShiftAllDate.Rows.Count > 0)
        {
            DisplayMessage("Time Table is in used ,You Can Not Delete");
            dtShiftAllDate.Dispose();
            return;
        }
        DataTable dtShift = objShiftDesc.GetShift_TimeTable();

        dtShift = new DataView(dtShift, "TimeTable_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtShift.Rows.Count == 0)
        {

            //dtShift = null;
            //dtShift = objSch.GetSheduleDescription();
            //dtShift = new DataView(dtShift, "TimeTable_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            int b = 0;
            b = objTimeTable.DeleteTimeTableMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
            if (b != 0)
            {
                DisplayMessage("Record Deleted");

                FillGridBin();
                FillGrid();
                Reset();
            }
            else
            {
                DisplayMessage("Record Not Deleted");
            }

        }
        else
        {
            DisplayMessage("Time table used in shift , you can not delete");
        }
    }
    protected void gvTimeTable_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTimeTable.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Time_T"];
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvTimeTable, dt, "", "");
        //AllPageCode();
    }
    protected void gvTimeTable_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Time_T"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Time_T"] = dt;
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvTimeTable, dt, "", "");
        //AllPageCode();
        gvTimeTable.HeaderRow.Focus();
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
        Reset();
        txtTimeTableName.Focus();
        return;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    public void FillGrid()
    {
        DataTable dt = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString(), ddlLocation.SelectedValue);
        // Modified By Nitin On 10/11/2014 for filter on brand Level
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvTimeTable, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Time_T"] = dt;
        Session["TimeTable"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objTimeTable.GetTimeTableMasterInactive(Session["CompId"].ToString());
        // Modified By Nitin On 10/11/2014 for filter on brand Level
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvTimeTableBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinTimeTable"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
        }

    }


    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chksell = (CheckBox)gvTimeTableBin.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gr in gvTimeTableBin.Rows)
        {
            if (chksell.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }


    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        ArrayList timetabledetails = new ArrayList();
        DataTable dttimetable = (DataTable)Session["dtbinFilter"];
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["select"] == null)
        {
            foreach (DataRow dr in dttimetable.Rows)
            {
                ViewState["select"] = 1;

                if (Session["CHECKED_ITEMS"] != null)
                {
                    timetabledetails = (ArrayList)Session["CHECKED_ITEMS"];
                }


                if (!timetabledetails.Contains(dr["TimeTable_Id"]))
                {
                    timetabledetails.Add(dr["TimeTable_Id"]);


                }

            }

            foreach (GridViewRow gr in gvTimeTableBin.Rows)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            if (timetabledetails != null && timetabledetails.Count > 0)
            {
                Session["CHECKED_ITEMS"] = timetabledetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtbinTimeTable"];
            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvTimeTableBin, dtUnit1, "", "");
            ViewState["select"] = null;
        }
    }



    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (gvTimeTableBin.Rows.Count > 0)
        {
            SaveCheckedValues();
            ArrayList tablerestore = new ArrayList();
            if (Session["CHECKED_ITEMS"] != null)
            {
                tablerestore = (ArrayList)Session["CHECKED_ITEMS"];
                if (tablerestore.Count > 0)
                {
                    foreach (int item in tablerestore)
                    {

                        b = objTimeTable.DeleteTimeTableMaster(Session["CompId"].ToString(), item.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                    }
                }



                if (b != 0)
                {

                    FillGrid();
                    FillGridBin();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CHECKED_ITEMS"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in gvTimeTableBin.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
                        if (chk.Checked)
                        {
                            fleg = 1;
                        }
                        else
                        {
                            fleg = 0;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
                    }
                    else
                    {
                        DisplayMessage("Record Not Activated");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                gvTimeTableBin.Focus();
                return;
            }
        }
    }
    public void Reset()
    {
        txtTimeTableName.ReadOnly = false;
        txtTimeTableNameL.ReadOnly = false;
        txtRelaxMin.ReadOnly = false;
        txtOnDutyTime.ReadOnly = false;
        txtRelaxMin.Text = "";
        txtOffDutyTime.ReadOnly = false;
        txtBeginingIn.ReadOnly = false;
        txtBeginingOut.ReadOnly = false;
        txtEndingIn.ReadOnly = false;
        txtEndingOut.ReadOnly = false;
        txtWorkMinute.ReadOnly = false;
        txtBreakMinute.ReadOnly = false;
        Session["CHECKED_ITEMS"] = null;
        btnSave.Visible = true;
        txtOTMin.Text = "0";
        txtTimeTableName.Text = "";
        txtTimeTableNameL.Text = "";
        txtOnDutyTime.Text = "";
        txtOffDutyTime.Text = "";
        txtBeginingIn.Text = "";
        txtBeginingOut.Text = "";
        txtWorkMinute.Text = "";
        txtBreakMinute.Text = "";
        txtEndingIn.Text = "";
        txtEndingOut.Text = "";
        // ViewState["TotalWorkMin"].ToString().Equals("0"); 
        txtLateRelaxation.Text = "0";
        txtEarlyRelaxation.Text = "0";
        hdnLoc_Id.Value = "";


        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        ddlLocNew.SelectedValue = Session["LocId"].ToString();
    }



    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
    }
}