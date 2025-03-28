using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.IO;
using DocumentFormat.OpenXml.Office2010.Excel;

public partial class HR_HR_EmployeeGratuity : System.Web.UI.Page
{
    Common cmn = null;
    SystemParameter objSys = null;
    Set_ApplicationParameter objAppParam = null;
    hr_gratuity_plan objGratuityPlan = null;
    hr_gratuity_days_detail objGratuityPlanDetail = null;
    Att_Employee_Leave objEmpLeave = null;
    IT_ObjectEntry objObjectEntry = null;
    LeaveMaster_deduction ObjLeavededuction = null;
    Set_Allowance ObjAllowance = null;
    EmployeeMaster objEmp = null;
    Pay_Employee_Month objPayEmpMonth = null;
    DataAccessClass ObjDa = null;
    Attendance objAttendance = null;
    hr_empgratuity objempgratuity = null;
    LocationMaster objLocation = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    hr_empgratuityDetail objempgratuitydetail = null;
    Set_DocNumber objDocNo = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Att_AttendanceRegister Objattregister = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGratuityPlan = new hr_gratuity_plan(Session["DBConnection"].ToString());
        objGratuityPlanDetail = new hr_gratuity_days_detail(Session["DBConnection"].ToString());
        objEmpLeave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());
        ObjAllowance = new Set_Allowance(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objempgratuity = new hr_empgratuity(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objempgratuitydetail = new hr_empgratuityDetail(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        Objattregister = new Att_AttendanceRegister(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/HR_EmployeeGratuity.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();
            FillGridBin();
            FillGrid();
            btnList_Click(null, null);
            CalendarExtender1.Format = objSys.SetDateFormat();
            FillPlan();
            txttRansdate.Text = DateTime.Now.ToString(objSys.SetDateFormat());

            try
            {
                txtpaymentCreditaccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account").Rows[0]["Param_Value"].ToString());

            }
            catch
            {

            }


            try
            {
                ddlPlanName.SelectedValue = GetIndemnityPlan_accordingLabourLaw();
            }
            catch
            {

            }

        }
    }

    public string GetIndemnityPlan_accordingLabourLaw()
    {

        string strIndemnityPlanId = "0";
        string strLabourLaw = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field3"].ToString();

        if (strLabourLaw != "" && strLabourLaw != "0")
        {
            strIndemnityPlanId = ObjDa.return_DataTable("select indemnity_plan_id from hr_laborLaw_config where Trans_id='" + strLabourLaw + "'").Rows[0]["indemnity_plan_id"].ToString();
        }
        return strIndemnityPlanId;
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bEdit;
        btnPost.Visible = clsPagePermission.bEdit;
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
        txttRansdate.Focus();
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

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnPost_Click(object sender, EventArgs e)
    {
        btnSave_Click(sender, e);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool Post = false;
        double PaidTotaamount = 0;
        if (((Button)sender).ID == "btnPost")
        {
            Post = true;


            if (txtpaymentCreditaccount.Text == "")
            {
                DisplayMessage("Credit account not found");
                return;
            }

            if (txtpaymentdebitaccount.Text == "")
            {
                DisplayMessage("debit account not found");
                return;
            }


            DataTable dtTerminationData = ObjDa.return_DataTable("select * from Pay_Termination where Employee_Id=" + hdnEmpId.Value + "");
            if (dtTerminationData.Rows.Count > 0)
            {

            }
            else
            {
                DisplayMessage("Employee has been Terminated After that you can post it.");
                return;
            }
        }


        //here we are checking that gratuity generated or not fopr sleected employee

        DataTable dtEmpGratuity = objempgratuity.GetRecord_By_EmployeeId(hdnEmpId.Value);


        if (dtEmpGratuity.Rows.Count > 0)
        {
            if (editid.Value == "")
            {
                DisplayMessage("Gratuity genrated for selected employee");
                return;

            }
            else
            {
                dtEmpGratuity = new DataView(dtEmpGratuity, "Trans_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();


                if (dtEmpGratuity.Rows.Count > 0)
                {
                    DisplayMessage("Gratuity generated for selected employee");
                    return;
                }

            }

        }






        PaidTotaamount = Convert.ToDouble(txtFinalAmount.Text);


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        if (GvDeductionDetail.Rows.Count == 0)
        {
            DisplayMessage("gratuity Detail not found");
            return;
        }

        int b = 0;


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {

            if (editid.Value == "")
            {
                b = objempgratuity.InsertRecord(hdnEmpId.Value, ddlPlanName.SelectedValue, ddlReason.SelectedValue, objSys.getDateForInput(txttRansdate.Text).ToString(), txtGratuityDays.Text, txtGratuitywage.Text, txtgratuityYears.Text, txtgratuityAmount.Text, txtBenefitamountlimit.Text, txtApplicablePercentage.Text, txtFinalAmount.Text, Post.ToString(), "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                foreach (GridViewRow gvrow in GvDeductionDetail.Rows)
                {
                    if (((TextBox)gvrow.FindControl("txrGratuitydays")).Text == "")
                    {
                        ((TextBox)gvrow.FindControl("txrGratuitydays")).Text = "0";
                    }
                    objempgratuitydetail.InsertRecord(b.ToString(), ((Label)gvrow.FindControl("lblyearCount")).Text, ((Label)gvrow.FindControl("lblYearName")).Text, ((Label)gvrow.FindControl("lblgratuitydays")).Text, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ((TextBox)gvrow.FindControl("txrGratuitydays")).Text, ref trns);
                }

                DisplayMessage("Record Saved", "green");
            }
            else
            {
                objempgratuity.UpdateRecord(editid.Value, hdnEmpId.Value, ddlPlanName.SelectedValue, ddlReason.SelectedValue, objSys.getDateForInput(txttRansdate.Text).ToString(), txtGratuityDays.Text, txtGratuitywage.Text, txtgratuityYears.Text, txtgratuityAmount.Text, txtBenefitamountlimit.Text, txtApplicablePercentage.Text, txtFinalAmount.Text, Post.ToString(), "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                objempgratuitydetail.Deleterecord(editid.Value, ref trns);

                foreach (GridViewRow gvrow in GvDeductionDetail.Rows)
                {
                    if (((TextBox)gvrow.FindControl("txrGratuitydays")).Text == "")
                    {
                        ((TextBox)gvrow.FindControl("txrGratuitydays")).Text = "0";
                    }
                    objempgratuitydetail.InsertRecord(editid.Value, ((Label)gvrow.FindControl("lblyearCount")).Text, ((Label)gvrow.FindControl("lblYearName")).Text, ((Label)gvrow.FindControl("lblgratuitydays")).Text, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ((TextBox)gvrow.FindControl("txrGratuitydays")).Text, ref trns);
                }


                btnList_Click(null, null);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                DisplayMessage("Record Updated", "green");
            }


            if (PaidTotaamount != 0 && Post)
            {


                string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();
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
                    int counter = objAcParameter.GetCounterforVoucherNumber1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "JV", Session["FinanceYearId"].ToString(), ref trns);


                    if (counter == 0)
                    {
                        strVoucherNumber = strVoucherNumber + "1";
                    }
                    else
                    {
                        strVoucherNumber = strVoucherNumber + (counter + 1);
                    }
                }

                b = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", DateTime.Now.ToString(), strVoucherNumber, DateTime.Now.ToString(), "JV", "1/1/1800", "1/1/1800", "", "Gratuity/indemnity Amount For '" + txtEmpName.Text.Split('/')[0].ToString() + "'", strCurrencyId, "1", "Gratuity/indemnity Amount For '" + txtEmpName.Text.Split('/')[0].ToString() + "'", false.ToString(), false.ToString(), false.ToString(), "JV", "", "", hdnEmpId.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                string strVMaxId = b.ToString();



                double strExchnagerate = Convert.ToDouble(SystemParameter.GetExchageRate(strCurrencyId, Session["CurrencyId"].ToString(), Session["DBConnection"].ToString()));

                string strCompAmount = (PaidTotaamount * strExchnagerate).ToString();
                string strLocAmount = PaidTotaamount.ToString();
                string strForeignAmount = PaidTotaamount.ToString();
                string strForeignExchangerate = "1";



                //str for Employee Id
                //For Debit

                if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "GS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Gratuity/indemnity Amount For '" + txtEmpName.Text.Split('/')[0].ToString() + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "GS", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Gratuity/indemnity Amount For '" + txtEmpName.Text.Split('/')[0].ToString() + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //For Credit
                if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "GS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Gratuity/indemnity Amount For '" + txtEmpName.Text.Split('/')[0].ToString() + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), hdnEmpId.Value, "0", "GS", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, "Gratuity/indemnity Amount For '" + txtEmpName.Text.Split('/')[0].ToString() + "'", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", strCompAmount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Reset();
            FillGrid();
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

    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
        {
            strNewDate = DateTime.Parse(strDate).ToString(objSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    protected void btnExportToExcel_Command(object sender, CommandEventArgs e)
    {

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/EmployeeGratuityReport.aspx?Trans_Id=" + e.CommandArgument + "','','height=650,width=950,scrollbars=Yes')", true);
        //DataTable dt = objempgratuity.GetRecord_ForExcel_ByEmp_Id(e.CommandArgument.ToString());
        //if (dt.Rows.Count > 0 && dt != null)
        //{
        //    //Common Function add By Lokesh on 25-08-2023
        //    objPageCmn.FillData((object)GvHeader, dt, "", "");

        //    //foreach (GridViewRow gvr in GvHeader.Rows)
        //    //{
        //    //    GridView gvDetail = (GridView)gvr.FindControl("GvDetail");

        //    //    DataTable dtDetail = objempgratuity.GetRecord_ForExcel_ByGratuity_Id(e.CommandArgument.ToString());
        //    //    if (dtDetail.Rows.Count > 0 && dtDetail != null)
        //    //    {
        //    //        //Common Function add By Lokesh on 25-08-2023
        //    //        objPageCmn.FillData((object)gvDetail, dtDetail, "", "");
        //    //    }
        //    //}


        //    string filename = String.Format("Results_{0}_{1}.xls", DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString());
        //    if (!string.IsNullOrEmpty(GvHeader.Page.Title))
        //        filename = GvHeader.Page.Title + ".xls";

        //    HttpContext.Current.Response.Clear();

        //    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);


        //    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        //    HttpContext.Current.Response.Charset = "";

        //    System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        //    System.Web.UI.HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);



        //    System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
        //    GvHeader.Parent.Controls.Add(form);
        //    form.Controls.Add(GvHeader);
        //    form.RenderControl(htmlWriter);

        //    HttpContext.Current.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
        //    HttpContext.Current.Response.Write(stringWriter.ToString());
        //    HttpContext.Current.Response.End();
        //}
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objempgratuity.GetRecord_By_TransId(e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Field4"].ToString() == "True")
            {
                DisplayMessage("Record posted , you can not edit");
                return;
            }

            editid.Value = e.CommandArgument.ToString();
            txtEmpName.Text = "" + dt.Rows[0]["Emp_Name"].ToString() + "/(" + dt.Rows[0]["Designation"].ToString() + ")/ " + dt.Rows[0]["emp_code"].ToString() + "";
            hdnEmpId.Value = dt.Rows[0]["Emp_Id"].ToString();
            ddlPlanName.SelectedValue = dt.Rows[0]["gratuity_plan_id"].ToString();
            ddlReason.SelectedValue = dt.Rows[0]["reason"].ToString();
            txtGratuityDays.Text = Common.GetAmountDecimal(dt.Rows[0]["gratuity_days"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtGratuitywage.Text = Common.GetAmountDecimal(dt.Rows[0]["gratuity_wage"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtgratuityYears.Text = Common.GetAmountDecimal(dt.Rows[0]["gratuity_years"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtgratuityAmount.Text = Common.GetAmountDecimal(dt.Rows[0]["gratuity_amount"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtBenefitamountlimit.Text = Common.GetAmountDecimal(dt.Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtApplicablePercentage.Text = Common.GetAmountDecimal(dt.Rows[0]["Field2"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtFinalAmount.Text = Common.GetAmountDecimal(dt.Rows[0]["Field3"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            //here we are getting deduction slab if exists

            DataTable dtdeduction = objempgratuitydetail.GetRecordBy_GratuiryPlanId(editid.Value).DefaultView.ToTable(true, "year_count", "year_name", "gratuity_days", "gratuity_days_physical");

            objPageCmn.FillData((object)GvDeductionDetail, dtdeduction, "", "");


            dtdeduction.Dispose();

            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = objempgratuity.GetRecord_By_TransId(e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Field4"].ToString() == "True")
            {
                DisplayMessage("Record posted , you can not delete");
                return;
            }
        }

        //code end
        int b = 0;
        b = objempgratuity.Restorerecord(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        DataTable dt = objempgratuity.GetAllTrueRecord();

        dt = new DataView(dt, "Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            dt = new DataView(dt, "Field4='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            dt = new DataView(dt, "Field4='False'", "", DataViewRowState.CurrentRows).ToTable();
        }


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
        dt = objempgratuity.GetAllFalseRecord();

        dt = new DataView(dt, "Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
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
                            b = objempgratuity.Restorerecord(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

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
        hdndeductionTransId.Value = "";
        objPageCmn.FillData((object)GvDeductionDetail, null, "", "");
        txttRansdate.Text = "";
        txtEmpName.Text = "";
        ddlPlanName.SelectedIndex = 0;
        ddlReason.SelectedIndex = 0;
        txtGratuityDays.Text = "0";
        txtgratuityAmount.Text = "0";
        txtGratuitywage.Text = "0";
        txtgratuityYears.Text = "0";
        txtApplicablePercentage.Text = "0";
        txtBenefitamountlimit.Text = "0";
        txtFinalAmount.Text = "0";
        txttRansdate.Focus();
        txttRansdate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
        try
        {
            ddlPlanName.SelectedValue = GetIndemnityPlan_accordingLabourLaw();
        }
        catch
        {

        }
    }

    #region employeegratuity

    public void FillPlan()
    {
        ddlPlanName.Items.Clear();

        DataTable dt = objGratuityPlan.GetAllTrueRecord(Session["CompId"].ToString(), Session["brandid"].ToString(), Session["locid"].ToString());

        ddlPlanName.DataSource = dt;
        ddlPlanName.DataTextField = "Plan_Name";
        ddlPlanName.DataValueField = "Trans_Id";
        ddlPlanName.DataBind();

        ddlPlanName.Items.Insert(0, "--select--");
    }

    protected void btndeduction_Click(object sender, EventArgs e)
    {

        bool AllowAllEmployeeOnGratuity = false;
        AllowAllEmployeeOnGratuity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("AllowAllEmployeeOnGratuity", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));


        DataTable dt = new DataTable();
        DataTable dtpayMonth = new DataTable();
        int eligibility_month = 0;
        double benefit_amount_limit = 0;
        int benefit_wagemonth_limit = 0;
        string calc_service_period = string.Empty;
        string applicable_allowances = string.Empty;
        int month_days_count = 0;
        double benefit_per_on_termination = 0;
        double benefit_per_on_resign = 0;
        double benefit_per_on_retirement = 0;
        double benefit_per_on_death = 0;
        double benefit_per_on_other = 0;
        double Basicsalary = 0;
        double Allowancevalue = 0;
        bool Isabsentdays = false;
        bool IsPaidLeavedays = false;
        bool IsUnPaidLeavedays = false;
        bool Isweekoffdays = false;
        bool Isholidays = false;
        int Presentday = 0;
        int weekoffday = 0;
        int holiday = 0;
        int paidLeave = 0;
        int unpaidLeave = 0;
        int absentday = 0;
        DataTable dtAttendanceregister = new DataTable();
        DataTable dttemp = new DataTable();

        DateTime dtTerminationdate = new DateTime();
        if (AllowAllEmployeeOnGratuity)
        {
            DataTable dtTerminationData = ObjDa.return_DataTable("select * from Pay_Termination where Employee_Id=" + hdnEmpId.Value + "");
            if (dtTerminationData.Rows.Count > 0)
            {
                dtTerminationdate = Convert.ToDateTime(ObjDa.return_DataTable("select MAX(Termination_Date) from Pay_Termination where Employee_Id=" + hdnEmpId.Value + "").Rows[0][0].ToString());
            }
            else
            {
                dtTerminationdate = DateTime.Now;
            }
        }
        else
        {
            dtTerminationdate = Convert.ToDateTime(ObjDa.return_DataTable("select MAX(Termination_Date) from Pay_Termination where Employee_Id=" + hdnEmpId.Value + "").Rows[0][0].ToString());
        }

        DataTable DTDOJ = objAttendance.GetEmployeeDOJ(Session["CompId"].ToString(), hdnEmpId.Value);
        DateTime DOJ = Convert.ToDateTime(DTDOJ.Rows[0]["DOJ"].ToString());
        DataTable dtDetail = new DataTable();
        dtDetail.Columns.Add("year_count", typeof(float));
        dtDetail.Columns.Add("year_name");
        dtDetail.Columns.Add("gratuity_days", typeof(float));
        dtDetail.Columns.Add("gratuity_days_physical", typeof(float));
        try
        {
            Basicsalary = Convert.ToDouble(ObjDa.return_DataTable("select basic_salary from Set_Employee_Parameter where Emp_Id=" + hdnEmpId.Value + "").Rows[0][0].ToString());
        }
        catch
        {
            Basicsalary = 0;
        }

        //here are getting all parameter according selected gratuity plan 

        dt = objGratuityPlan.GetAllRecord_BY_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlPlanName.SelectedValue);

        if (dt.Rows.Count > 0)
        {
            eligibility_month = Convert.ToInt32(dt.Rows[0]["eligibility_month"].ToString());
            benefit_amount_limit = Convert.ToDouble(dt.Rows[0]["benefit_amount_limit"].ToString());
            benefit_wagemonth_limit = Convert.ToInt32(dt.Rows[0]["benefit_wage_month_limit"].ToString());
            calc_service_period = dt.Rows[0]["calc_service_period"].ToString();
            applicable_allowances = dt.Rows[0]["applicable_allowances"].ToString();
            month_days_count = Convert.ToInt32(dt.Rows[0]["month_days_count"].ToString());
            benefit_per_on_termination = Convert.ToDouble(dt.Rows[0]["benefit_per_on_termination"].ToString());
            benefit_per_on_resign = Convert.ToDouble(dt.Rows[0]["benefit_per_on_resign"].ToString());
            benefit_per_on_retirement = Convert.ToDouble(dt.Rows[0]["benefit_per_on_retirement"].ToString());
            benefit_per_on_death = Convert.ToDouble(dt.Rows[0]["benefit_per_on_death"].ToString());
            benefit_per_on_other = Convert.ToDouble(dt.Rows[0]["benefit_per_on_other"].ToString());
            Isabsentdays = Convert.ToBoolean(dt.Rows[0]["is_absent_day"].ToString());
            IsPaidLeavedays = Convert.ToBoolean(dt.Rows[0]["is_paid_leave"].ToString());
            IsUnPaidLeavedays = Convert.ToBoolean(dt.Rows[0]["is_unpaid_leave"].ToString());
            Isholidays = Convert.ToBoolean(dt.Rows[0]["is_holiday"].ToString());
            Isweekoffdays = Convert.ToBoolean(dt.Rows[0]["is_weekoff"].ToString());

        }

        //here we getting that number of month worked by employee

        //dtpayMonth = objPayEmpMonth.GetAllRecordPostedByCompanyId(Session["CompId"].ToString());

        //dtpayMonth = new DataView(dtpayMonth, "Emp_Id=" + hdnEmpId.Value + "", "", DataViewRowState.CurrentRows).ToTable();

        if ((dtTerminationdate - DOJ).TotalDays < (eligibility_month * 30))
        {
            DisplayMessage("Employee Not eligible for gratuity");
            return;
        }


        if (applicable_allowances != "0")
        {

            DataTable dtallowance = ObjDa.return_DataTable("select Value_type,value from Pay_Employee_Allow_Deduction where Emp_Id = " + hdnEmpId.Value + " and Type ='1' and  Ref_Id in (" + applicable_allowances.Substring(0, applicable_allowances.Length - 1) + ")");

            foreach (DataRow dr in dtallowance.Rows)
            {
                if (dr["Value_type"].ToString() == "1")
                {

                    Allowancevalue += Convert.ToDouble(dr["value"].ToString());

                }
                else
                {
                    try
                    {
                        Allowancevalue += (Basicsalary * Convert.ToDouble(dr["value"].ToString())) / 100;
                    }
                    catch
                    {
                        Allowancevalue = 0;
                    }

                }

            }
        }

        int dojYear = DOJ.Year;
        int YearCount = 0;
        double gratuityDaysPerDay = 0;
        string strNarration = string.Empty;
        double TotalGratuityDays = 0;
        DateTime dtFromdate = DOJ;
        DateTime dtTodate = DOJ.AddYears(1).AddDays(-1);
        int Totalday = 0;
        while (dojYear <= dtTerminationdate.Year)
        {
            if (dtTodate > dtTerminationdate)
            {
                dtTodate = dtTerminationdate;
            }



            DataRow dr = dtDetail.NewRow();

            try
            {
                gratuityDaysPerDay = Convert.ToDouble(ObjDa.return_DataTable("select isnull(Remuneration_days,0) from hr_gratuity_days_detail where From_Year <= '" + (YearCount + 1).ToString() + "' and To_Year >= '" + (YearCount + 1).ToString() + "' and Gratuity_plan_id=" + ddlPlanName.SelectedValue + "").Rows[0][0].ToString());


                gratuityDaysPerDay = gratuityDaysPerDay / 365;
            }
            catch
            {
                gratuityDaysPerDay = 0;
            }


            dtAttendanceregister = Objattregister.GetAttendanceRegDataByDate_EmpId(hdnEmpId.Value, dtFromdate.ToString(), dtTodate.ToString());

            if (dtAttendanceregister.Rows.Count > 0)
            {
                if (Isweekoffdays)
                {
                    dttemp = new DataView(dtAttendanceregister, "Is_Week_Off='" + Isweekoffdays.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    weekoffday = dttemp.Rows.Count;
                }

                if (Isholidays)
                {

                    dttemp = new DataView(dtAttendanceregister, "Is_Holiday='" + Isholidays.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    holiday = dttemp.Rows.Count;
                }

                if (Isabsentdays)
                {

                    dttemp = new DataView(dtAttendanceregister, "Is_Absent='" + Isabsentdays.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    absentday = dttemp.Rows.Count;
                }


                dttemp = new DataView(dtAttendanceregister, "Is_Week_Off='False' and Is_Holiday='False' and Is_Absent='False' and Is_Leave='False'", "", DataViewRowState.CurrentRows).ToTable();

                Presentday = dttemp.Rows.Count;


                if (IsPaidLeavedays)
                {
                    paidLeave = ObjDa.return_DataTable("SELECT Att_Leave_Request.Emp_Id FROM Att_Leave_Request_Child Inner JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE (Att_Leave_Request_Child.Is_Paid = 'True') AND (Att_Leave_Request_Child.IsActive = 'True') and (Att_Leave_Request.Is_Approved='True') AND (Att_Leave_Request_Child.Leave_Date between '" + dtFromdate + "' and '" + dtTodate + "')  and Att_Leave_Request.Emp_Id=" + hdnEmpId.Value + "").Rows.Count;
                }

                if (IsUnPaidLeavedays)
                {
                    unpaidLeave = ObjDa.return_DataTable("SELECT Att_Leave_Request.Emp_Id FROM Att_Leave_Request_Child Inner JOIN Att_Leave_Request ON Att_Leave_Request_Child.Ref_Id = Att_Leave_Request.Trans_Id WHERE (Att_Leave_Request_Child.Is_Paid = 'False') AND (Att_Leave_Request_Child.IsActive = 'True') and (Att_Leave_Request.Is_Approved='True') AND (Att_Leave_Request_Child.Leave_Date between '" + dtFromdate + "' and '" + dtTodate + "')  and Att_Leave_Request.Emp_Id=" + hdnEmpId.Value + "").Rows.Count;
                }
            }

            Totalday = (Presentday + absentday + weekoffday + holiday + paidLeave + unpaidLeave);

            if (dtTodate == dtTerminationdate)
            {
                if (calc_service_period == "0")
                {
                    if (Totalday > 180)
                    {
                        Totalday = 365;
                    }
                    else
                    {
                        Totalday = 0;
                    }
                }
            }

            dr[0] = dojYear.ToString();
            dr[1] = dtFromdate.ToString(objSys.SetDateFormat()) + "-" + dtTodate.ToString(objSys.SetDateFormat());
            dr[2] = gratuityDaysPerDay * Totalday;
            dr[3] = gratuityDaysPerDay * Totalday;
            YearCount++;
            TotalGratuityDays += gratuityDaysPerDay * Totalday;
            dtDetail.Rows.Add(dr);

            //here we are checking that service caluclation is pro data basis or nearest round of integer

            dojYear++;

            if (dtTodate == dtTerminationdate)
            {
                break;
            }
            dtFromdate = dtTodate.AddDays(1);
            dtTodate = dtFromdate.AddYears(1).AddDays(-1);

        }

        objPageCmn.FillData((object)GvDeductionDetail, dtDetail, "", "");


        txtGratuityDays.Text = Common.GetAmountDecimal(TotalGratuityDays.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        txtGratuitywage.Text = Common.GetAmountDecimal(((Basicsalary + Allowancevalue) / month_days_count).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        txtgratuityYears.Text = YearCount.ToString();
        txtgratuityAmount.Text = Common.GetAmountDecimal((((Basicsalary + Allowancevalue) / month_days_count) * TotalGratuityDays).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());

        if (benefit_amount_limit > 0)
        {
            txtBenefitamountlimit.Text = Common.GetAmountDecimal(benefit_amount_limit.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        else if (benefit_wagemonth_limit > 0)
        {
            txtBenefitamountlimit.Text = Common.GetAmountDecimal((benefit_wagemonth_limit * Basicsalary).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }

        if (ddlReason.SelectedValue == "Termination")
        {
            txtApplicablePercentage.Text = Common.GetAmountDecimal(benefit_per_on_termination.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        else if (ddlReason.SelectedValue == "Resign")
        {
            txtApplicablePercentage.Text = Common.GetAmountDecimal(benefit_per_on_resign.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        else if (ddlReason.SelectedValue == "Retirement")
        {
            txtApplicablePercentage.Text = Common.GetAmountDecimal(benefit_per_on_retirement.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        else if (ddlReason.SelectedValue == "Death")
        {
            txtApplicablePercentage.Text = Common.GetAmountDecimal(benefit_per_on_death.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        else if (ddlReason.SelectedValue == "Other")
        {
            txtApplicablePercentage.Text = Common.GetAmountDecimal(benefit_per_on_other.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }

        if (Convert.ToDouble(txtgratuityAmount.Text) > Convert.ToDouble(txtBenefitamountlimit.Text))
        {
            txtFinalAmount.Text = Common.GetAmountDecimal(((Convert.ToDouble(txtBenefitamountlimit.Text) * Convert.ToDouble(txtApplicablePercentage.Text)) / 100).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        else
        {
            txtFinalAmount.Text = Common.GetAmountDecimal(((Convert.ToDouble(txtgratuityAmount.Text) * Convert.ToDouble(txtApplicablePercentage.Text)) / 100).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }


        Session["dtGratuityDetail"] = dtDetail;



    }

    protected void btndeductionCancel_Click(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        bool AllowAllEmployeeOnGratuity = false;
        DataAccessClass Objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCheckParameter = Objda.return_DataTable("select * from Set_ApplicationParameter where Param_Name='AllowAllEmployeeOnGratuity' and Company_Id='" + HttpContext.Current.Session["CompId"].ToString() + "' and Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'");
        if (dtCheckParameter.Rows.Count > 0)
        {
            AllowAllEmployeeOnGratuity = Convert.ToBoolean(dtCheckParameter.Rows[0]["Param_Value"].ToString());
        }

        DataTable dt = null;
        if (AllowAllEmployeeOnGratuity == true)
        {
            dt = Objda.return_DataTable("select top 10 set_employeemaster.Emp_Name,set_employeemaster.Emp_Code,Set_DesignationMaster.Designation from set_employeemaster left join set_designationmaster on set_employeemaster.designation_id= set_designationmaster.Designation_Id  where set_employeemaster.Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and (set_employeemaster.Emp_Name like '%" + prefixText + "%' OR set_employeemaster.Emp_Code like '%" + prefixText + "%')");
        }
        else
        {
            dt = Objda.return_DataTable("select top 10 set_employeemaster.Emp_Name,set_employeemaster.Emp_Code,Set_DesignationMaster.Designation from set_employeemaster left join set_designationmaster on set_employeemaster.designation_id= set_designationmaster.Designation_Id  where set_employeemaster.Field2='True' and   set_employeemaster.Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and (set_employeemaster.Emp_Name like '%" + prefixText + "%' OR set_employeemaster.Emp_Code like '%" + prefixText + "%')");
        }



        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][0].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][1].ToString() + "";
        }
        return str;

    }

    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        bool AllowAllEmployeeOnGratuity = false;
        AllowAllEmployeeOnGratuity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("AllowAllEmployeeOnGratuity", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));

        hdnEmpId.Value = "";
        string empid = string.Empty;


        if (((TextBox)sender).Text != "")
        {
            empid = ((TextBox)sender).Text.Split('/')[((TextBox)sender).Text.Split('/').Length - 1];

            DataTable dtEmp = null;
            if (AllowAllEmployeeOnGratuity == true)
            {
                dtEmp = ObjDa.return_DataTable("select emp_id from set_employeemaster where Emp_Code='" + empid + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'");
            }
            else
            {
                dtEmp = ObjDa.return_DataTable("select emp_id from set_employeemaster where Field2='True' and  Emp_Code='" + empid + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'");
            }

            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();

                if (((TextBox)sender).ID.Trim() == "txtEmpName")
                {
                    hdnEmpId.Value = empid;

                }
            }
            else
            {
                DisplayMessage("Employee Not Exists");

                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }
        }
    }

    #region Account
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
                DataTable dtCOA = ObjDa.return_DataTable(sql);

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


    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id=" + strAccountNo + " and IsActive='True'";

            DataTable dtAccName = ObjDa.return_DataTable(sql);
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
    private string GetLocationCode(string strLocationId)
    {
        string strLocationCode = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocation = objLocation.GetLocationMasterByLocationId(strLocationId);
            if (dtLocation.Rows.Count > 0)
            {
                strLocationCode = dtLocation.Rows[0]["Location_Code"].ToString();
            }
        }
        return strLocationCode;
    }
    #endregion



    #endregion

    protected void txrGratuitydays_TextChanged(object sender, EventArgs e)
    {
        double TotalGratuitydays = 0;
        double gratuityDays = 0;

        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

        try
        {
            gratuityDays = Convert.ToDouble(ObjDa.return_DataTable("select isnull(Remuneration_days,0) from hr_gratuity_days_detail where From_Year <= '" + (gvrow.RowIndex + 1).ToString() + "' and To_Year >= '" + (gvrow.RowIndex + 1).ToString() + "' and Gratuity_plan_id=" + ddlPlanName.SelectedValue + "").Rows[0][0].ToString());
        }
        catch
        {

        }

        if (((TextBox)gvrow.FindControl("txrGratuitydays")).Text == "")
        {
            ((TextBox)gvrow.FindControl("txrGratuitydays")).Text = "0";
        }

        if (Convert.ToDouble(((TextBox)gvrow.FindControl("txrGratuitydays")).Text) > gratuityDays)
        {
            DisplayMessage("gratuity days is exceeded for selected year");
            ((TextBox)gvrow.FindControl("txrGratuitydays")).Text = "0";
        }

        foreach (GridViewRow gvrow1 in GvDeductionDetail.Rows)
        {
            TotalGratuitydays += Convert.ToDouble(((TextBox)gvrow1.FindControl("txrGratuitydays")).Text);
        }

        txtGratuityDays.Text = Common.GetAmountDecimal(TotalGratuitydays.ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        txtgratuityAmount.Text = Common.GetAmountDecimal((TotalGratuitydays * Convert.ToDouble(txtGratuitywage.Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());

        if (Convert.ToDouble(txtgratuityAmount.Text) > Convert.ToDouble(txtBenefitamountlimit.Text))
        {
            txtFinalAmount.Text = Common.GetAmountDecimal(((Convert.ToDouble(txtBenefitamountlimit.Text) * Convert.ToDouble(txtApplicablePercentage.Text)) / 100).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        else
        {
            txtFinalAmount.Text = Common.GetAmountDecimal(((Convert.ToDouble(txtgratuityAmount.Text) * Convert.ToDouble(txtApplicablePercentage.Text)) / 100).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }


    }
}