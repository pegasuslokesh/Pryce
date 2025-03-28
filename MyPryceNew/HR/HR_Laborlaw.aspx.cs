using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class HR_HR_Laborlaw : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    hr_gratuity_plan objGratuityPlanMaster = null;

    Att_Employee_Leave objEmpLeave = null;
    IT_ObjectEntry objObjectEntry = null;
    LeaveMaster_deduction ObjLeavededuction = null;
    Set_Allowance ObjAllowance = null;
    LeaveMaster objLeaveType = null;
    hr_laborLaw_config ObjLabourLaw = null;
    hr_laborLaw_leave ObjLabourLawLeave = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }


        // btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGratuityPlanMaster = new hr_gratuity_plan(Session["DBConnection"].ToString());

        objEmpLeave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());
        ObjAllowance = new Set_Allowance(Session["DBConnection"].ToString());
        objLeaveType = new LeaveMaster(Session["DBConnection"].ToString());
        ObjLabourLaw = new hr_laborLaw_config(Session["DBConnection"].ToString());
        ObjLabourLawLeave = new hr_laborLaw_leave(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            if (Session["Application_Id"].ToString().Trim() == "1")
            {
                //for time man application , it should be false
                Div_Indemnity.Visible = false;
            }
            else
            {
                Div_Indemnity.Visible = true;
            }

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../hr/hr_laborLaw.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            FillPlan();
            txtValue.Focus();
            FillGridBin();
            FillGrid();
            FillLeaveTypeDDL();
            btnList_Click(null, null);
        }
    }
    
    public void FillPlan()
    {
        ddlPlanName.Items.Clear();

        DataTable dt = objGratuityPlanMaster.GetAllTrueRecord(Session["CompId"].ToString(), Session["brandid"].ToString(), Session["locid"].ToString());

        ddlPlanName.DataSource = dt;
        ddlPlanName.DataTextField = "Plan_Name";
        ddlPlanName.DataValueField = "Trans_Id";
        ddlPlanName.DataBind();

        ddlPlanName.Items.Insert(0, "--select--");

        if(dt.Rows.Count>0)
        {
            ddlPlanName.SelectedIndex = 1;
        }
        else
        {
            ddlPlanName.SelectedIndex = 0;
        }
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }



    protected void btnList_Click(object sender, EventArgs e)
    {
        txtValue.Focus();
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //txtPlanName.Focus();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

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
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
    
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();

        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
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
            DataTable dtCust = (DataTable)Session["Leave"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_LeaveType_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)gvLeaveMaster, view.ToTable(), "", "");
            
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
            DataTable dtCust = (DataTable)Session["dtbinLeave"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)gvLeaveMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
               
            }
            else
            {
            
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvLeaveMasterBin.Rows)
        {
            index = (int)gvLeaveMasterBin.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValuesemplog()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvLeaveMasterBin.Rows)
            {
                int index = (int)gvLeaveMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void gvLeaveMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvLeaveMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMasterBin, dt, "", "");
   
        PopulateCheckedValuesemplog();
    }
    //protected void gvLeaveMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{

    //    gvLeaveMasterBin.PageIndex = e.NewPageIndex;
    //    if (HDFSortbin.Value == "")
    //        FillGridBin();
    //    else
    //    {
    //        DataTable dt = (DataTable)Session["dtbinFilter"];
    //        gvLeaveMasterBin.DataSource = dt;
    //        gvLeaveMasterBin.DataBind();
    //        AllPageCode();
    //    }
    //    string temp = string.Empty;
    //    bool isselcted;

    //    for (int i = 0; i < gvLeaveMasterBin.Rows.Count; i++)
    //    {
    //        Label lblconid = (Label)gvLeaveMasterBin.Rows[i].FindControl("lblLeaveId");
    //        string[] split = lblSelectedRecord.Text.Split(',');

    //        for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
    //        {
    //            if (lblSelectedRecord.Text.Split(',')[j] != "")
    //            {
    //                if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
    //                {
    //                    ((CheckBox)gvLeaveMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
    //                }
    //            }
    //        }
    //    }

    //}
    protected void gvLeaveMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjLabourLaw.GetAllFalseRecord(Session["CompId"].ToString());

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMasterBin, dt, "", "");
     
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        string strIndemnityPlanId = "0";
        string WeekOffDays = string.Empty;


        if (ddlPlanName.SelectedIndex > 0)
        {
            strIndemnityPlanId = ddlPlanName.SelectedValue;
        }



        for (int i = 0; i < ChkWeekOffList.Items.Count; i++)
        {
            if (ChkWeekOffList.Items[i].Selected == true)
            {
                if (WeekOffDays == "")
                {
                    WeekOffDays = ChkWeekOffList.Items[i].Text;
                }
                else
                {
                    WeekOffDays = WeekOffDays + "," + ChkWeekOffList.Items[i].Text;
                }
            }
        }


        DataTable dtTemp = ObjLabourLaw.GetAllRecord(Session["CompId"].ToString());



        if (editid.Value == "")
        {
            dtTemp = new DataView(dtTemp, "Laborlaw_Name='" + txtLabourLawname.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtTemp = new DataView(dtTemp, "Laborlaw_Name='" + txtLabourLawname.Text.Trim() + "' and Trans_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        }


        if (dtTemp.Rows.Count > 0)
        {


            if (Convert.ToBoolean(dtTemp.Rows[0]["IsActive"].ToString()))
            {
                DisplayMessage("Labour law name is already exists");
                txtLabourLawname.Focus();
                return;
            }
            else
            {
                DisplayMessage("Labour law name is already exists in bin section");
                txtLabourLawname.Focus();
                return;
            }
        }


        int b = 0;

        if (editid.Value == "")
        {

            b = ObjLabourLaw.Insertrecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtLabourLawname.Text, txtDescription.Text, ddlFinancestartMonth.SelectedValue, strIndemnityPlanId, txtWorkdayMinute.Text, WeekOffDays, txtyearlyHalfDay.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            DataTable dtLeave = GetLeaveDatatable();

            foreach (DataRow dr in dtLeave.Rows)
            {
                ObjLabourLawLeave.Insertrecord(b.ToString(), dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["schedule_type"].ToString(), dr["is_yearcarry"].ToString(), dr["is_auto"].ToString(), dr["is_rule"].ToString(), dr["Gender"].ToString());
            }



            if (b != 0)
            {
                DisplayMessage("Record Saved","green");
                FillGrid();
                Reset();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }

        }
        else
        {

            b = ObjLabourLaw.Updaterecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtLabourLawname.Text, txtDescription.Text, ddlFinancestartMonth.SelectedValue, strIndemnityPlanId, txtWorkdayMinute.Text, WeekOffDays, txtyearlyHalfDay.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            ObjLabourLawLeave.DeleteRecord(editid.Value);

            DataTable dtLeave = GetLeaveDatatable();

            foreach (DataRow dr in dtLeave.Rows)
            {
                ObjLabourLawLeave.Insertrecord(editid.Value, dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["schedule_type"].ToString(), dr["is_yearcarry"].ToString(), dr["is_auto"].ToString(), dr["is_rule"].ToString(), dr["Gender"].ToString());
            }


            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                FillGrid();
                Reset();
                btnList_Click(null, null);
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not updated");
            }
        }
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        string IsLeaveSalaryGiven = string.Empty;

        DataTable dt = ObjLabourLaw.GetRecordbyTRans_Id(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {

            txtLabourLawname.Text = dt.Rows[0]["Laborlaw_Name"].ToString();
            txtDescription.Text = dt.Rows[0]["Description"].ToString();
            ddlFinancestartMonth.SelectedValue = dt.Rows[0]["fy_start_month"].ToString();
            try
            {
                ddlPlanName.SelectedValue = dt.Rows[0]["indemnity_plan_id"].ToString();
            }
            catch
            {
                ddlPlanName.SelectedIndex = 0;
            }
            txtWorkdayMinute.Text = dt.Rows[0]["work_day_minutes"].ToString();
            txtyearlyHalfDay.Text = dt.Rows[0]["yearly_halfday"].ToString();
            ChkWeekOffList.ClearSelection();

            try
            {
                string[] WeekOffDays = dt.Rows[0]["week_off_day"].ToString().Split(',');
                for (int j = 0; j < WeekOffDays.Length; j++)
                {
                    for (int i = 0; i < ChkWeekOffList.Items.Count; i++)
                    {
                        if (ChkWeekOffList.Items[i].Text == WeekOffDays[j].ToString())
                        {
                            ChkWeekOffList.Items[i].Selected = true;
                            break;
                        }
                    }
                }
            }
            catch
            {
            }


            DataTable dtleavedetail = ObjLabourLawLeave.GetRecord_By_LaborLawId(editid.Value);

            dtleavedetail = dtleavedetail.DefaultView.ToTable(true, "Gender", "Leave_Type_Id", "Total_Leave_days", "Paid_Leave_days", "schedule_type", "is_yearcarry", "is_auto", "is_rule", "Trans_Id");


            objPageCmn.FillData((object)GvLeaveDetail, dtleavedetail, "", "");

            //txtPlanName.Text = dt.Rows[0]["Plan_Name"].ToString();
            //txtEligibleMonth.Text = dt.Rows[0]["eligibility_month"].ToString();
            //txtBenefitAmountLimit.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_amount_limit"].ToString());
            //txtbenefitwagemonth.Text = dt.Rows[0]["benefit_wage_month_limit"].ToString();
            //chkIsTaxFree.Checked = Convert.ToBoolean(dt.Rows[0]["is_tax_free"].ToString());
            //chkIsforefeitprovision.Checked = Convert.ToBoolean(dt.Rows[0]["is_forefeit_provision"].ToString());

            //if (dt.Rows[0]["calc_service_period"].ToString() == "0")
            //{
            //    rbtnserviceCalc_Nearestinteger.Checked = true;
            //    rbtnserviceCalc_proratebasis.Checked = false;

            //}
            //else
            //{
            //    rbtnserviceCalc_Nearestinteger.Checked = false;
            //    rbtnserviceCalc_proratebasis.Checked = true;

            //}


            //txtApplicableAllowance.Text = getAllowanceNamebyId(dt.Rows[0]["applicable_allowances"].ToString());
            //txtMonthDaysCount.Text = dt.Rows[0]["month_days_count"].ToString();
            //txtbenefitperonTermination.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_termination"].ToString());
            //txtbenefitperonresign.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_resign"].ToString());
            //txtbenefitperonretirement.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_retirement"].ToString());
            //txtbenefitperondeath.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_death"].ToString());
            //txtbenefitperonother.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_other"].ToString());
            ////here we are getting deduction slab if exists

            //DataTable dtdeduction = objGratuityPlanDetail.GetRecordBy_GratuiryPlanId(editid.Value).DefaultView.ToTable(true, "Trans_Id", "From_Year", "To_Year", "Remuneration_days");

            //cmn.FillData((object)GvDeductionDetail, dtdeduction, "", "");

            //txtFromYear.Text = GetExceedFromValue();
            //dtdeduction.Dispose();

            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);
        }



    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        //code end
        int b = 0;
        b = ObjLabourLaw.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
    protected void gvLeaveMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_LeaveType_Mstr"];
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMaster, dt, "", "");
   
    }
    protected void gvLeaveMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_LeaveType_Mstr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_LeaveType_Mstr"] = dt;
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMaster, dt, "", "");
     
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListLeaveName(string prefixText, int count, string contextKey)
    {
        LeaveMaster objLeaveMaster = new LeaveMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = new DataView(objLeaveMaster.GetLeaveMaster(HttpContext.Current.Session["CompId"].ToString()), "Leave_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Leave_Name"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Applicable_Allowance(string prefixText, int count, string contextKey)
    {
        Set_Allowance obj = new Set_Allowance(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = obj.GetDistinctAllowance(HttpContext.Current.Session["CompId"].ToString(), prefixText);
        string[] str = new string[dt.Rows.Count];

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] += dt.Rows[i][0].ToString() + ",";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = obj.GetAllowanceTrueAll("1");
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] += dt.Rows[i]["Allowance"].ToString() + ",";
                    }
                }
            }
        }
        return str;
    }

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
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
        DataTable dt = ObjLabourLaw.GetAllTrueRecord(Session["CompId"].ToString());

        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMaster, dt, "", "");
    
        Session["dtFilter_LeaveType_Mstr"] = dt;
        Session["Leave"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = ObjLabourLaw.GetAllFalseRecord(Session["CompId"].ToString());

        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinLeave"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
           
        }
        else
        {

       
        }

    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvLeaveMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvLeaveMasterBin.Rows)
        {

            if (chkSelAll.Checked == true)
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
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Trans_Id"]))
                {
                    userdetails.Add(dr["Trans_Id"]);
                }
            }
            foreach (GridViewRow GR in gvLeaveMasterBin.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)gvLeaveMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvLeaveMasterBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = ObjLabourLaw.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

                        }
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
                    foreach (GridViewRow Gvr in gvLeaveMasterBin.Rows)
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
                gvLeaveMasterBin.Focus();
                return;

            }
        }
    }
    public void Reset()
    {

        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        objPageCmn.FillData((object)GvLeaveDetail, null, "", "");
        btnCancelLabour_Click(null, null);
        txtLabourLawname.Focus();
        txtLabourLawname.Text = "";
        ddlFinancestartMonth.SelectedIndex = 0;
        txtDescription.Text = "";
        txtWorkdayMinute.Text = "";
        ddlPlanName.SelectedIndex = 0;
        txtyearlyHalfDay.Text = "";
        ChkWeekOffList.ClearSelection();

    }



    #region Leavedetail

    protected void imgBtnEmpoloyeeDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = GetLeaveDatatable();

        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;

        dt = new DataView(dt, "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


        objPageCmn.FillData((object)GvLeaveDetail, dt, "", "");


    }
    public void FillLeaveTypeDDL()
    {
        DataTable dt = objLeaveType.GetLeaveMaster(Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            ddlLeaveType.DataSource = null;
            ddlLeaveType.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlLeaveType, dt, "Leave_Name", "Leave_Id");
        }
        else
        {
            ddlLeaveType.Items.Insert(0, "--Select--");
            ddlLeaveType.SelectedIndex = 0;
        }
    }
    protected void btnAddLabour_Click(object sender, EventArgs e)
    {
        string strMaxId = "1";



        DataTable dt = GetLeaveDatatable();


        DataTable dtTemp = dt.Copy();

        dtTemp = new DataView(dt, "Leave_Type_Id='" + ddlLeaveType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtTemp.Rows.Count > 0)
        {
            if(dtTemp.Rows[0]["Gender"].ToString()=="Both" || ddlGender.SelectedValue.Trim() == "Both")
            {
                DisplayMessage("Leave type already exists");
                ddlLeaveType.Focus();
                return;
            }


            dtTemp = new DataView(dt, "Leave_Type_Id='" + ddlLeaveType.SelectedValue + "' and Gender='" + ddlGender.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtTemp.Rows.Count > 0)
            {
                DisplayMessage("Leave type already exists");
                ddlLeaveType.Focus();
                return;
            }

        }
      


        if (dt.Rows.Count > 0)
        {
            strMaxId = (float.Parse(new DataView(dt, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) + 1).ToString();
        }


        dt.Rows.Add(ddlGender.SelectedValue, ddlLeaveType.SelectedValue, txtTotalLeave.Text, txtPaidLeave.Text, ddlSchType.SelectedValue, chkYearCarry.Checked, chkIsAuto.Checked, ChkIsRule.Checked, strMaxId);


        objPageCmn.FillData((object)GvLeaveDetail, dt, "", "");

        btnCancelLabour_Click(null, null);


    }

    protected void btnCancelLabour_Click(object sender, EventArgs e)
    {
        ddlLeaveType.SelectedIndex = 0;
        txtTotalLeave.Text = "";
        txtPaidLeave.Text = "";
        chkYearCarry.Checked = false;
        chkIsAuto.Checked = false;
        ChkIsRule.Checked = false;
        ddlLeaveType.Focus();

    }

    public DataTable GetLeaveDatatable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Gender");
        dt.Columns.Add("Leave_Type_Id");
        dt.Columns.Add("Total_Leave_days");
        dt.Columns.Add("Paid_Leave_days");
        dt.Columns.Add("schedule_type");
        dt.Columns.Add("is_yearcarry");
        dt.Columns.Add("is_auto");
        dt.Columns.Add("is_rule");
        dt.Columns.Add("Trans_Id", typeof(float));

        foreach (GridViewRow gvrow in GvLeaveDetail.Rows)
        {
            DataRow dr = dt.NewRow();
            dr[0] = ((Label)gvrow.FindControl("lblGender")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblLeave_Type_Id")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblTotalleaveDay")).Text;
            dr[3] = ((Label)gvrow.FindControl("lblTotalPaidDay")).Text;
            dr[4] = ((Label)gvrow.FindControl("lblscheduleType")).Text;
            dr[5] = ((Label)gvrow.FindControl("lblisYearcarry")).Text;
            dr[6] = ((Label)gvrow.FindControl("lblisauto")).Text;
            dr[7] = ((Label)gvrow.FindControl("lblisRule")).Text;
            dr[8] = dt.Rows.Count + 1;
            dt.Rows.Add(dr);
        }

        return dt;

    }


    #endregion



}