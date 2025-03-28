using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetUp_LeaveTypeMaster : BasePage
{

    Common cmn = null;
    SystemParameter objSys = null;
    LeaveMaster objLeave = null;
    Att_Employee_Leave objEmpLeave = null;
    IT_ObjectEntry objObjectEntry = null;
    LeaveMaster_deduction ObjLeavededuction = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objLeave = new LeaveMaster(Session["DBConnection"].ToString());
        objEmpLeave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        //AllPageCode();
        if (!IsPostBack)
        {
          
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/LeaveTypeMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();
            FillGridBin();
            FillGrid();
            CloudSetup();
        }
    }
    public void CloudSetup()
    {
        if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
        {

            div_maxleavebal.Visible = false;
            div_LeaveSalaryGiven.Visible = false;
            div_NegativeBalance.Visible = false;
            div_LeaveTransaction.Visible = false;

        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    protected void txtLeaveName_OnTextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objLeave.GetLeaveMasterByLeaveName(Session["CompId"].ToString().ToString(), txtLeaveName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtLeaveName.Text = "";
                DisplayMessage("Leave Type Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLeaveName);
                return;
            }
            DataTable dt1 = objLeave.GetLeaveMasterInactive(Session["CompId"].ToString().ToString());
            dt1 = new DataView(dt1, "Leave_Name='" + txtLeaveName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtLeaveName.Text = "";
                DisplayMessage("Leave Type Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLeaveName);
                return;
            }
            txtLeaveNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objLeave.GetLeaveMasterById(Session["CompId"].ToString().ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Leave_Name"].ToString() != txtLeaveName.Text)
                {
                    DataTable dt = objLeave.GetLeaveMaster(Session["CompId"].ToString().ToString());
                    dt = new DataView(dt, "Leave_Name='" + txtLeaveName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtLeaveName.Text = "";
                        DisplayMessage("Leave Type Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLeaveName);
                        return;
                    }
                    DataTable dt1 = objLeave.GetLeaveMaster(Session["CompId"].ToString().ToString());
                    dt1 = new DataView(dt1, "Leave_Name='" + txtLeaveName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtLeaveName.Text = "";
                        DisplayMessage("Leave Type Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLeaveName);
                        return;
                    }
                }
            }
            txtLeaveNameL.Focus();
        }
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
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

    protected void gvLeaveMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objLeave.GetLeaveMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
        {
            txtMaxLeaveBalance.Text = "0";
            chkIsLeaveSalaryGiven.Checked = true;
            chkIsNegativeBalance.Checked = false;
        }


        if (txtLeaveName.Text == "")
        {
            DisplayMessage("Enter Leave Type Name");
            txtLeaveName.Focus();
            return;
        }

        if (txtRequiredservicedays.Text.Trim() == "")
        {
            txtRequiredservicedays.Text = "0";
        }


        if (txtMaxAttempt.Text == "")
        {
            txtMaxAttempt.Text = "0";
        }

        if (txtMaxLeaveBalance.Text == "")
        {
            txtMaxLeaveBalance.Text = "0";
        }



        if (editid.Value == "")
        {

            DataTable dt1 = objLeave.GetLeaveMaster(Session["CompId"].ToString());

            dt1 = new DataView(dt1, "Leave_Name='" + txtLeaveName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Leave Type Name Already Exists");
                txtLeaveName.Focus();
                return;

            }




            b = objLeave.InsertLeaveMaster(Session["CompId"].ToString(), txtLeaveName.Text, txtLeaveNameL.Text, Session["BrandId"].ToString(), chkIsNegativeBalance.Checked.ToString(), chkIsLeaveSalaryGiven.Checked.ToString(), txtRequiredservicedays.Text, txtMaxAttempt.Text, txtMaxLeaveBalance.Text, chkLeaveApproval.Checked.ToString(), chkLeaveTransaction.Checked.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
            //here we inserting record in deduction table if exist

            foreach (GridViewRow gvr in GvDeductionDetail.Rows)
            {

                ObjLeavededuction.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), b.ToString(), "0", ((Label)gvr.FindControl("lblDaysFrom")).Text, ((Label)gvr.FindControl("lblDaysTo")).Text, ((Label)gvr.FindControl("lbldeductionpercentage")).Text, "", "", "", "", "", false.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

            }


            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                FillGrid();
                Reset();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            string leaveTypeName = string.Empty;
            DataTable dt1 = objLeave.GetLeaveMaster(Session["CompId"].ToString());
            try
            {
                leaveTypeName = new DataView(dt1, "Leave_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Leave_Name"].ToString();
            }
            catch
            {
                leaveTypeName = "";
            }
            dt1 = new DataView(dt1, "Leave_Name='" + txtLeaveName.Text + "' and Leave_Name<>'" + leaveTypeName + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Leave Type Name Already Exists");
                txtLeaveName.Focus();
                return;

            }
            b = objLeave.UpdateLeaveMaster(editid.Value, Session["CompId"].ToString(), txtLeaveName.Text, txtLeaveNameL.Text, Session["BrandId"].ToString(), chkIsNegativeBalance.Checked.ToString(), chkIsLeaveSalaryGiven.Checked.ToString(), txtRequiredservicedays.Text, txtMaxAttempt.Text, txtMaxLeaveBalance.Text, chkLeaveApproval.Checked.ToString(), chkLeaveTransaction.Checked.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

            //here we inserting record in deduction table if exist

            ObjLeavededuction.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, "0");
            foreach (GridViewRow gvr in GvDeductionDetail.Rows)
            {

                ObjLeavededuction.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, "0", ((Label)gvr.FindControl("lblDaysFrom")).Text, ((Label)gvr.FindControl("lblDaysTo")).Text, ((Label)gvr.FindControl("lbldeductionpercentage")).Text, "", "", "", "", "", false.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

            }


            if (b != 0)
            {
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
        editid.Value = e.CommandArgument.ToString();
        string IsLeaveSalaryGiven = string.Empty;

        DataTable dt = objLeave.GetLeaveMasterById(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {
            chkIsNegativeBalance.Checked = false;
            chkIsNegativeBalance.Checked = Convert.ToBoolean(dt.Rows[0]["Is_Partial"].ToString());
            txtLeaveName.Text = dt.Rows[0]["Leave_Name"].ToString();
            txtLeaveNameL.Text = dt.Rows[0]["Leave_Name_L"].ToString();
            IsLeaveSalaryGiven = dt.Rows[0]["Field1"].ToString();
            txtRequiredservicedays.Text = dt.Rows[0]["Field2"].ToString();
            txtMaxAttempt.Text = dt.Rows[0]["Field3"].ToString();
            txtMaxLeaveBalance.Text = dt.Rows[0]["Field4"].ToString();
            if (IsLeaveSalaryGiven == "True")
            {
                chkIsLeaveSalaryGiven.Checked = true;
            }
            else
            {
                chkIsLeaveSalaryGiven.Checked = false;
            }

            chkLeaveApproval.Checked = Convert.ToBoolean(dt.Rows[0]["Field5"].ToString());
            chkLeaveTransaction.Checked = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());





            //here we are getting deduction slab if exists

            DataTable dtdeduction = ObjLeavededuction.GetRecordbyLeaveTypeId(editid.Value).DefaultView.ToTable(true, "Trans_Id", "DaysFrom", "Daysto", "Deduction_Percentage");

            objPageCmn.FillData((object)GvDeductionDetail, dtdeduction, "", "");

            txtexceedDays.Text = GetExceedFromValue();
            dtdeduction.Dispose();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);
        }



    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        //this code is created by jitendra upadhyay on 02-09-2014
        //this code for validation that user can not delete Record when Assigned to any employee
        //code start
        DataTable DtLeave = objEmpLeave.GetEmployeeLeaveByCompanyId(Session["CompId"].ToString());
        try
        {
            DtLeave = new DataView(DtLeave, "LeaveType_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (DtLeave.Rows.Count > 0)
        {
            DisplayMessage("Leave Type is assigned to employee, You can not delete");
            return;
        }

        //code end
        int b = 0;
        b = objLeave.DeleteLeaveMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
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
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    public void FillGrid()
    {
        DataTable dt = objLeave.GetLeaveMaster(Session["CompId"].ToString());

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
        dt = objLeave.GetLeaveMasterInactive(Session["CompId"].ToString());

        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinLeave"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
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

                if (!userdetails.Contains(dr["Leave_Id"]))
                {
                    userdetails.Add(dr["Leave_Id"]);
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
                            b = objLeave.DeleteLeaveMaster(Session["CompId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

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


        chkIsNegativeBalance.Checked = false;
        chkIsLeaveSalaryGiven.Checked = false;
        txtLeaveName.Text = "";
        txtLeaveNameL.Text = "";
        txtRequiredservicedays.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtexceedDays.Text = "0";
        txtexceedDaysto.Text = "";
        txtdeduction.Text = "";
        hdndeductionTransId.Value = "";
        objPageCmn.FillData((object)GvDeductionDetail, null, "", "");
        txtMaxAttempt.Text = "0";
        txtMaxLeaveBalance.Text = "0";
        chkLeaveApproval.Checked = false;
        chkLeaveTransaction.Checked = false;

    }
    #region DeductionSlab



    //delete
    protected void imgBtnEmpoloyeeDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dtemp = GetDeductionList();


        dtemp = new DataView(dtemp, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        objPageCmn.FillData((object)GvDeductionDetail, dtemp, "", "");


        txtexceedDays.Text = GetExceedFromValue();
        dtemp.Dispose();

    }

    //edit
    protected void imgBtnEmployeeEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = GetDeductionList();


        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        txtexceedDays.Text = dt.Rows[0]["DaysFrom"].ToString();
        txtexceedDaysto.Text = dt.Rows[0]["DaysTo"].ToString();
        txtexceedDaysto.Enabled = false;
        txtdeduction.Text = dt.Rows[0]["Deduction_Percentage"].ToString();
        hdndeductionTransId.Value = e.CommandArgument.ToString();

        dt.Dispose();
    }


    protected void btndeduction_Click(object sender, EventArgs e)
    {


        if (txtexceedDays.Text == "")
        {
            DisplayMessage("Enter Exceed From days");
            txtexceedDays.Focus();
            return;
        }

        if (txtexceedDaysto.Text == "")
        {
            DisplayMessage("Enter Exceed to days");
            txtexceedDaysto.Focus();
            return;
        }


        if (float.Parse(txtexceedDays.Text) > float.Parse(txtexceedDaysto.Text))
        {
            DisplayMessage("exceed days to value should be greater or equal to exceed days from value");
            txtexceedDaysto.Focus();
            return;
        }

        if (txtdeduction.Text == "")
        {
            DisplayMessage("Enter deduction percentage");
            txtdeduction.Focus();
            return;
        }




        DataTable dt = GetDeductionList();




        if (hdndeductionTransId.Value == "" || hdndeductionTransId.Value == "0")
        {
            dt.Rows.Add();


            dt.Rows[dt.Rows.Count - 1][0] = dt.Rows.Count + 1;
            dt.Rows[dt.Rows.Count - 1][1] = txtexceedDays.Text;
            dt.Rows[dt.Rows.Count - 1][2] = txtexceedDaysto.Text;
            dt.Rows[dt.Rows.Count - 1][3] = txtdeduction.Text;

        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == hdndeductionTransId.Value)
                {

                    dt.Rows[i][1] = txtexceedDays.Text;
                    dt.Rows[i][2] = txtexceedDaysto.Text;
                    dt.Rows[i][3] = txtdeduction.Text;

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
        dt.Columns.Add("DaysFrom");
        dt.Columns.Add("DaysTo", typeof(float));
        dt.Columns.Add("Deduction_Percentage");


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
        txtexceedDays.Text = "";
        txtexceedDaysto.Text = "";
        txtdeduction.Text = "";
        txtexceedDays.Focus();
        hdndeductionTransId.Value = "";
        txtexceedDays.Text = GetExceedFromValue();
        txtexceedDaysto.Focus();
        txtexceedDaysto.Enabled = true;
    }



    public string GetExceedFromValue()
    {
        string strvalue = "0";

        DataTable dt = GetDeductionList();

        if (dt.Rows.Count > 0)
        {
            strvalue = (float.Parse(new DataView(dt, "", "DaysTo desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["DaysTo"].ToString()) + 1).ToString();

        }

        return strvalue;

    }

    #endregion



}
