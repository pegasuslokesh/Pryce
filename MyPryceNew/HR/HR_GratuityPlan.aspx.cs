using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;

public partial class HR_HR_GratuityPlan : System.Web.UI.Page
{
    Common cmn = null;
    SystemParameter objSys = null;
    hr_gratuity_plan objGratuityPlan = null;
    hr_gratuity_days_detail objGratuityPlanDetail = null;
    Att_Employee_Leave objEmpLeave = null;
    IT_ObjectEntry objObjectEntry = null;
    LeaveMaster_deduction ObjLeavededuction = null;
    Set_Allowance ObjAllowance = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }


        // btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
        //AllPageCode();

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGratuityPlan = new hr_gratuity_plan(Session["DBConnection"].ToString());
        objGratuityPlanDetail = new hr_gratuity_days_detail(Session["DBConnection"].ToString());
        objEmpLeave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());
        ObjAllowance = new Set_Allowance(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/HR_GratuityPlan.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            txtValue.Focus();
            FillGridBin();
            FillGrid();
            rbtnserviceCalc_Nearestinteger.Checked = true;
            rbtnserviceCalc_proratebasis.Checked = false;
            btnList_Click(null, null);
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnSave.Visible = clsPagePermission.bEdit;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
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
        txtPlanName.Focus();
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
            //AllPageCode();
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
                imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
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
        //AllPageCode();
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
        dt = objGratuityPlan.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void txtBenefitAmountLimit_OnTextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).ID == "txtBenefitAmountLimit")
        {
            txtbenefitwagemonth.Text = "0";

        }
        else
        {
            txtBenefitAmountLimit.Text = "0";
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strApplicableAllowance = string.Empty;

        DataTable dtGratuity = objGratuityPlan.GetAllTrueRecord(Session["CompId"].ToString(), "0", "0");



        if (txtApplicableAllowance.Text != "")
        {
            foreach (string str in txtApplicableAllowance.Text.Split(','))
            {

                if (str.Trim() == "")
                {
                    continue;
                }

                if (!txtApplicableAllowance.Text.Trim().Split(',').Contains(ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), str).Rows[0]["Allowance_Id"].ToString()))
                {
                    strApplicableAllowance += ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), str).Rows[0]["Allowance_Id"].ToString() + ",";
                }

            }
        }
        else
        {
            strApplicableAllowance = "0";
        }

        int b = 0;


        //here we are checking that gratuity plan is already exist or not 

        if (editid.Value == "")
        {
            dtGratuity = new DataView(dtGratuity, "Plan_Name='" + txtPlanName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }
        else
        {
            dtGratuity = new DataView(dtGratuity, "Plan_Name='" + txtPlanName.Text.Trim() + "' and Trans_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();


        }

        if (dtGratuity.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtGratuity.Rows[0]["IsActive"].ToString()))
            {
                DisplayMessage("Plan Name is already exist");
                txtPlanName.Focus();
                return;

            }
            else
            {
                DisplayMessage("Plan Name is already exist in bin section");
                txtPlanName.Focus();
                return;
            }
        }


        if (GvDeductionDetail.Rows.Count == 0)
        {
            DisplayMessage("Enter Gratuity days detail");
            return;
        }



        if (editid.Value == "")
        {
            b = objGratuityPlan.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtPlanName.Text, txtEligibleMonth.Text, txtBenefitAmountLimit.Text, txtbenefitwagemonth.Text, chkIsTaxFree.Checked.ToString(), rbtnserviceCalc_Nearestinteger.Checked ? "0" : "1", strApplicableAllowance, chkIsforefeitprovision.Checked.ToString(), txtMonthDaysCount.Text, txtbenefitperonTermination.Text, txtbenefitperonresign.Text, txtbenefitperonretirement.Text, txtbenefitperondeath.Text, txtbenefitperonother.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkisabsent.Checked.ToString(), chkisunpaidLeave.Checked.ToString(), chkispaidleave.Checked.ToString(), chkisholiday.Checked.ToString(), chkisweekoff.Checked.ToString());
            //b = objLeave.InsertLeaveMaster(Session["CompId"].ToString(), txtLeaveName.Text, txtLeaveNameL.Text, Session["BrandId"].ToString(), false.ToString(), chkIsLeaveSalaryGiven.Checked.ToString(), txtRequiredservicedays.Text, txtMaxAttempt.Text, txtMaxLeaveBalance.Text, "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            //here we inserting record in deduction table if exist

            foreach (GridViewRow gvr in GvDeductionDetail.Rows)
            {

                objGratuityPlanDetail.InsertRecord(b.ToString(), ((Label)gvr.FindControl("lblDaysFrom")).Text, ((Label)gvr.FindControl("lblDaysTo")).Text, ((Label)gvr.FindControl("lbldeductionpercentage")).Text, false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }


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
            b = objGratuityPlan.updateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtPlanName.Text, txtEligibleMonth.Text, txtBenefitAmountLimit.Text, txtbenefitwagemonth.Text, chkIsTaxFree.Checked.ToString(), rbtnserviceCalc_Nearestinteger.Checked ? "0" : "1", strApplicableAllowance, chkIsforefeitprovision.Checked.ToString(), txtMonthDaysCount.Text, txtbenefitperonTermination.Text, txtbenefitperonresign.Text, txtbenefitperonretirement.Text, txtbenefitperondeath.Text, txtbenefitperonother.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkisabsent.Checked.ToString(), chkisunpaidLeave.Checked.ToString(), chkispaidleave.Checked.ToString(), chkisholiday.Checked.ToString(), chkisweekoff.Checked.ToString());

            //b = objLeave.UpdateLeaveMaster(editid.Value, Session["CompId"].ToString(), txtLeaveName.Text, txtLeaveNameL.Text, Session["BrandId"].ToString(), false.ToString(), chkIsLeaveSalaryGiven.Checked.ToString(), txtRequiredservicedays.Text, txtMaxAttempt.Text, txtMaxLeaveBalance.Text, "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            //here we inserting record in deduction table if exist

            objGratuityPlanDetail.Deleterecord(editid.Value);
            foreach (GridViewRow gvr in GvDeductionDetail.Rows)
            {

                objGratuityPlanDetail.InsertRecord(editid.Value, ((Label)gvr.FindControl("lblDaysFrom")).Text, ((Label)gvr.FindControl("lblDaysTo")).Text, ((Label)gvr.FindControl("lbldeductionpercentage")).Text, false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }


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
    public string getAllowanceNamebyId(string strRefId)
    {
        string strAllowancename = string.Empty;

        if (strRefId != "0")
        {
            foreach (string str in strRefId.Split(','))
            {
                if (str.Trim() == "")
                {
                    continue;
                }

                strAllowancename += ObjAllowance.GetAllowanceTruebyId(Session["CompId"].ToString(), str).Rows[0]["Allowance"].ToString() + ",";

            }
        }
        return strAllowancename;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        string IsLeaveSalaryGiven = string.Empty;

        DataTable dt = objGratuityPlan.GetAllRecord_BY_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {
            txtPlanName.Text = dt.Rows[0]["Plan_Name"].ToString();
            txtEligibleMonth.Text = dt.Rows[0]["eligibility_month"].ToString();
            txtBenefitAmountLimit.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_amount_limit"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitwagemonth.Text = dt.Rows[0]["benefit_wage_month_limit"].ToString();
            chkIsTaxFree.Checked = Convert.ToBoolean(dt.Rows[0]["is_tax_free"].ToString());
            chkIsforefeitprovision.Checked = Convert.ToBoolean(dt.Rows[0]["is_forefeit_provision"].ToString());
            chkisabsent.Checked = Convert.ToBoolean(dt.Rows[0]["is_absent_day"].ToString());
            chkisunpaidLeave.Checked = Convert.ToBoolean(dt.Rows[0]["is_unpaid_leave"].ToString());
            chkispaidleave.Checked = Convert.ToBoolean(dt.Rows[0]["is_paid_leave"].ToString());
            chkisholiday.Checked = Convert.ToBoolean(dt.Rows[0]["is_holiday"].ToString());
            chkisweekoff.Checked = Convert.ToBoolean(dt.Rows[0]["is_weekoff"].ToString());

            if (dt.Rows[0]["calc_service_period"].ToString() == "0")
            {
                rbtnserviceCalc_Nearestinteger.Checked = true;
                rbtnserviceCalc_proratebasis.Checked = false;

            }
            else
            {
                rbtnserviceCalc_Nearestinteger.Checked = false;
                rbtnserviceCalc_proratebasis.Checked = true;

            }


            txtApplicableAllowance.Text = getAllowanceNamebyId(dt.Rows[0]["applicable_allowances"].ToString());
            txtMonthDaysCount.Text = dt.Rows[0]["month_days_count"].ToString();
            txtbenefitperonTermination.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_termination"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitperonresign.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_resign"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitperonretirement.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_retirement"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitperondeath.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_death"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitperonother.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_other"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            //here we are getting deduction slab if exists

            DataTable dtdeduction = objGratuityPlanDetail.GetRecordBy_GratuiryPlanId(editid.Value).DefaultView.ToTable(true, "Trans_Id", "From_Year", "To_Year", "Remuneration_days");

            objPageCmn.FillData((object)GvDeductionDetail, dtdeduction, "", "");

            txtFromYear.Text = GetExceedFromValue();
            dtdeduction.Dispose();

            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);
        }



    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        //code end
        int b = 0;
        b = objGratuityPlan.Restorerecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        //AllPageCode();
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
        //AllPageCode();
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
    protected void txtLsApplicableAllowances_OnTextChanged(object sender, EventArgs e)
    {
        if (txtApplicableAllowance.Text != "")
        {

            foreach (string str in txtApplicableAllowance.Text.Split(','))
            {

                if (str.Trim() == "")
                {
                    continue;
                }

                if (ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), str).Rows.Count == 0)
                {
                    DisplayMessage("select allowance in suggestion only");
                    txtApplicableAllowance.Text = "";
                    txtApplicableAllowance.Focus();
                    break;
                }


            }

            txtApplicableAllowance.Focus();

        }


    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
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
        DataTable dt = objGratuityPlan.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_LeaveType_Mstr"] = dt;
        Session["Leave"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objGratuityPlan.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinLeave"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
            //ImgbtnSelectAll.Visible = false;
        }
        else
        {

            //AllPageCode();
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
                            b = objGratuityPlan.Restorerecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

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
        txtPlanName.Text = "";
        txtEligibleMonth.Text = "";
        txtBenefitAmountLimit.Text = "";
        txtbenefitwagemonth.Text = "";
        txtApplicableAllowance.Text = "";
        chkIsTaxFree.Checked = false;
        chkIsforefeitprovision.Checked = false;
        txtMonthDaysCount.Text = "";
        txtbenefitperonTermination.Text = "";
        txtbenefitperonresign.Text = "";
        txtbenefitperonretirement.Text = "";
        txtbenefitperondeath.Text = "";
        txtbenefitperonother.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtFromYear.Text = "0";
        txttoYear.Text = "";
        txtremunerationDays.Text = "";
        hdndeductionTransId.Value = "";
        objPageCmn.FillData((object)GvDeductionDetail, null, "", "");
        rbtnserviceCalc_Nearestinteger.Checked = true;
        rbtnserviceCalc_proratebasis.Checked = false;
        chkisabsent.Checked = false;
        chkisunpaidLeave.Checked = false;
        chkispaidleave.Checked = false;
        chkisholiday.Checked = false;
        chkisweekoff.Checked = false;


    }
    #region DeductionSlab



    //delete
    protected void imgBtnEmpoloyeeDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dtemp = GetDeductionList();


        dtemp = new DataView(dtemp, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        objPageCmn.FillData((object)GvDeductionDetail, dtemp, "", "");


        txtFromYear.Text = GetExceedFromValue();
        dtemp.Dispose();

    }

    //edit
    protected void imgBtnEmployeeEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = GetDeductionList();


        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        txtFromYear.Text = dt.Rows[0]["From_Year"].ToString();
        txttoYear.Text = dt.Rows[0]["To_Year"].ToString();
        txttoYear.Enabled = false;
        txtremunerationDays.Text = dt.Rows[0]["Remuneration_days"].ToString();
        hdndeductionTransId.Value = e.CommandArgument.ToString();

        dt.Dispose();
    }


    protected void btndeduction_Click(object sender, EventArgs e)
    {


        if (txtFromYear.Text == "")
        {
            DisplayMessage("Enter From Year");
            txtFromYear.Focus();
            return;
        }

        if (txttoYear.Text == "")
        {
            DisplayMessage("Enter To year");
            txttoYear.Focus();
            return;
        }


        if (float.Parse(txtFromYear.Text) > float.Parse(txttoYear.Text))
        {
            DisplayMessage("Year days to value should be greater or equal to Year days from value");
            txttoYear.Focus();
            return;
        }

        if (txtremunerationDays.Text == "")
        {
            DisplayMessage("Enter deduction percentage");
            txtremunerationDays.Focus();
            return;
        }

        DataTable dt = GetDeductionList();

        if (hdndeductionTransId.Value == "" || hdndeductionTransId.Value == "0")
        {
            dt.Rows.Add();

            dt.Rows[dt.Rows.Count - 1][0] = dt.Rows.Count + 1;
            dt.Rows[dt.Rows.Count - 1][1] = txtFromYear.Text;
            dt.Rows[dt.Rows.Count - 1][2] = txttoYear.Text;
            dt.Rows[dt.Rows.Count - 1][3] = txtremunerationDays.Text;
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == hdndeductionTransId.Value)
                {
                    dt.Rows[i][1] = txtFromYear.Text;
                    dt.Rows[i][2] = txttoYear.Text;
                    dt.Rows[i][3] = txtremunerationDays.Text;

                    break;
                }
            }

        }

        objPageCmn.FillData((object)GvDeductionDetail, dt, "", "");

        btndeductionCancel_Click(null, null);
    }



    public DataTable GetDeductionList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("From_Year", typeof(float));
        dt.Columns.Add("To_Year", typeof(float));
        dt.Columns.Add("Remuneration_days", typeof(float));


        foreach (GridViewRow gvrow in GvDeductionDetail.Rows)
        {
            DataRow dr = dt.NewRow();

            dr[0] = ((Label)gvrow.FindControl("lblTransId")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblDaysFrom")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblDaysTo")).Text;
            dr[3] = ((Label)gvrow.FindControl("lbldeductionpercentage")).Text;

            dt.Rows.Add(dr);
        }



        return dt;
    }


    protected void btndeductionCancel_Click(object sender, EventArgs e)
    {
        txtFromYear.Text = "";
        txttoYear.Text = "";
        txtremunerationDays.Text = "";
        txtFromYear.Focus();
        hdndeductionTransId.Value = "";
        txtFromYear.Text = GetExceedFromValue();
        txttoYear.Focus();
        txttoYear.Enabled = true;
    }



    public string GetExceedFromValue()
    {
        string strvalue = "0";

        DataTable dt = GetDeductionList();

        if (dt.Rows.Count > 0)
        {
            strvalue = (float.Parse(new DataView(dt, "", "To_Year desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["To_Year"].ToString()) + 1).ToString();

        }

        return strvalue;

    }

    #endregion
}