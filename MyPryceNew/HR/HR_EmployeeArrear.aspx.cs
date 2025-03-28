using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

public partial class HR_HR_EmployeeArrear : System.Web.UI.Page
{

    Pay_EmployeeArrear ObjEmpArrear = null;
    Common ObjComman = null;
    SystemParameter ObjSys = null;
    EmployeeMaster objEmp = null;
    EmployeeParameter objempparam = null;
    Pay_Employee_Month objPayMonth = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjEmpArrear = new Pay_EmployeeArrear(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objPayMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../hr/hr_employeearrear.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            FillddlDeaprtment();
          
            TxtFromYear.Text = DateTime.Now.Year.ToString();
            txttoYear.Text = DateTime.Now.Year.ToString();
            FillGrid();
        }
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Save.Visible = clsPagePermission.bAdd;
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }


    #region List

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void FillGrid()
    {
        DataTable dtBrand = ObjEmpArrear.GetTrueAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString());



        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtDeduction"] = dtBrand;
        Session["dtFilter_HR_Em_Arr"] = dtBrand;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dtBrand, "", "");
      
    }


    protected void GvsalaryPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvsalaryPlan.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_HR_Em_Arr"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
   
        GvsalaryPlan.HeaderRow.Focus();
    }

    protected void GvsalaryPlan_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_HR_Em_Arr"];
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
        Session["dtFilter_HR_Em_Arr"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
      
        GvsalaryPlan.HeaderRow.Focus();
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
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text + "%'";
            }
            DataTable dtDeduction = (DataTable)Session["dtDeduction"];
            DataView view = new DataView(dtDeduction, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvsalaryPlan, view.ToTable(), "", "");
            Session["dtFilter_HR_Em_Arr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        
        }
        txtValue.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        FillGrid();
        FillGridBin();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }


    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {


        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        //Txt_Plan_Name.Focus();

    }




    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = 0;

        DataTable dt = new DataTable();
        dt = ObjEmpArrear.GetRecordByTrans_Id(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {


            if (dt.Rows[0]["Is_Adjusted"].ToString() == "True")
            {
                DisplayMessage("Arrear adjusted , you can not delete");
                dt.Dispose();
                return;

            }

        }


        b = ObjEmpArrear.Deletrecord(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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

    #endregion


    #region Bin


    public void FillGridBin()
    {

        DataTable dt = new DataTable();
        dt = ObjEmpArrear.GetFalseAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString());
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");
        Session["dtBinDeduction"] = dt;
        Session["dtBinFilter"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        Session["CHECKED_ITEMS"] = null;
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {
           
        }
    }


    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");


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
            DataTable dtCust = (DataTable)Session["dtBinDeduction"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvsalaryPlanBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                ImgbtnSelectAll.Visible = false;
            }
            else
            {
               
            }
        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");


        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");


        int Msg = 0;
        if (GvsalaryPlanBin.Rows.Count != 0)
        {
            SaveCheckedValues();
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Please Select Record");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        Msg = ObjEmpArrear.Deletrecord(Session["CompId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }

                    if (Msg != 0)
                    {
                        FillGrid();
                        FillGridBin();
                        ViewState["Select"] = null;
                        lblSelectedRecord.Text = "";
                        DisplayMessage("Record Activated");
                        Session["CHECKED_ITEMS"] = null;
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
                return;
            }
        }
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
            {
                int index = (int)GvsalaryPlanBin.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
        {
            index = (int)GvsalaryPlanBin.DataKeys[gvrow.RowIndex].Value;
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
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");


        ArrayList userdetails = new ArrayList();
        DataTable dtDEPARTMENT = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtDEPARTMENT.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Trans_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Trans_Id"]));

            }
            foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvsalaryPlanBin, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvsalaryPlanBin.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
        {
            index = (int)GvsalaryPlanBin.DataKeys[gvrow.RowIndex].Value;
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
    protected void GvsalaryPlanBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        GvsalaryPlanBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");

        PopulateCheckedValues();
      

    }
    protected void GvsalaryPlanBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        SaveCheckedValues();
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtbinFilter"];
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");

      
        PopulateCheckedValues();
    }
    #endregion



    #region Operation

    public void Reset()
    {

        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        Session["CHECKED_ITEMS"] = null;
        Btn_New.Text = Resources.Attendance.New;

        Lbl_Tab_New.Text = Resources.Attendance.New;
    }
    protected void Btn_List_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void Btn_New_Click(object sender, EventArgs e)
    {
        FillEmployee();
    }

    protected void Btn_Bin_Click(object sender, EventArgs e)
    {
        FillGridBin();
    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {



        double Arrear = 0;


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        DateTime dtFrom = new DateTime();
        DateTime dtTo = new DateTime();

        try
        {
            dtFrom = new DateTime(Convert.ToInt32(TxtFromYear.Text), Convert.ToInt32(ddlFromMonth.SelectedValue), 1);
        }
        catch
        {
            DisplayMessage("From month or year is invalid");
            ddlFromMonth.Focus();
            return;

        }





        try
        {

            int dayInMonth = DateTime.DaysInMonth(Convert.ToInt32(txttoYear.Text), Convert.ToInt32(ddlToMonth.SelectedValue));
            dtTo = new DateTime(Convert.ToInt32(txttoYear.Text), Convert.ToInt32(ddlToMonth.SelectedValue), dayInMonth);
        }
        catch
        {
            DisplayMessage("to month or year is invalid");
            ddlToMonth.Focus();
            return;

        }


        if (dtFrom > dtTo)
        {
            DisplayMessage("selected month and year criteria is invalid");
            ddlFromMonth.Focus();
            return;
        }


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {

            int counter = 0;
            foreach (GridViewRow gvrow in gvEmpSalary.Rows)
            {

                try
                {
                    Arrear = Convert.ToDouble(((Label)gvrow.FindControl("lblarrear")).Text);
                }
                catch
                {
                    Arrear = 0;

                }

                if (((CheckBox)gvrow.FindControl("chkgvSelect")).Checked && Arrear > 0)
                {

                    if (CheckEmployeeArearExist(((Label)gvrow.FindControl("lblEmpId")).Text, dtFrom, dtTo, ref trns))
                    {
                        ObjEmpArrear.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((Label)gvrow.FindControl("lblEmpId")).Text, dtFrom.ToString(), dtTo.ToString(), Arrear.ToString(), Session["CurrencyId"].ToString(), false.ToString(), "0", true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);
                        counter++;
                    }
                    else
                    {
                        DisplayMessage("Record already exist between selected period for employee code =" + ((Label)gvrow.FindControl("lblEmpCode")).Text + "");

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
            }


            DisplayMessage(counter.ToString() + " record inserted ");

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            reset();
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


    public bool CheckEmployeeArearExist(string strempId, DateTime dtFromDate, DateTime dtTodate, ref SqlTransaction trns)
    {
        bool Result = true;


        DataTable dt = ObjEmpArrear.GetRecordByTransactionDate(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Locid"].ToString(), strempId, dtFromDate.ToString(), dtTodate.ToString(), ref trns);


        if (dt.Rows.Count > 0)
        {

            Result = false;


        }


        dt.Dispose();
        return Result;


    }

    protected void Btn_Reset_Click(object sender, EventArgs e)
    {
        reset();
    }


    public void reset()
    {
        btnAllRefreshSalary_Click(null, null);
        ddlFromMonth.Enabled = true;
        ddlToMonth.Enabled = true;
        btnGetArrear.Enabled = true;
        TxtFromYear.Enabled = true;
        txttoYear.Enabled = true;
        ddlFromMonth.SelectedIndex = 0;
        ddlToMonth.SelectedIndex = 0;
        TxtFromYear.Text = DateTime.Now.Year.ToString();
        txttoYear.Text = DateTime.Now.Year.ToString();
        hdnEditId.Value = "";
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
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

    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
        {
            strNewDate = DateTime.Parse(strDate).ToString(ObjSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }


    protected void btnGetArrear_Click(object sender, EventArgs e)
    {
        DateTime dtFrom = new DateTime();
        DateTime dtTo = new DateTime();

        try
        {
            dtFrom = new DateTime(Convert.ToInt32(TxtFromYear.Text), Convert.ToInt32(ddlFromMonth.SelectedValue), 1);
        }
        catch
        {
            DisplayMessage("From month or year is invalid");
            ddlFromMonth.Focus();
            return;

        }



        try
        {
            dtTo = new DateTime(Convert.ToInt32(txttoYear.Text), Convert.ToInt32(ddlToMonth.SelectedValue), 1);
        }
        catch
        {
            DisplayMessage("to month or year is invalid");
            ddlToMonth.Focus();
            return;

        }


        if (dtFrom > dtTo)
        {
            DisplayMessage("selected month and year criteria is invalid");
            ddlFromMonth.Focus();
            return;
        }



        ddlFromMonth.Enabled = false;
        ddlToMonth.Enabled = false;
        TxtFromYear.Enabled = false;
        txttoYear.Enabled = false;
        btnGetArrear.Enabled = false;


        int affectedrecord = GetEmployeeeArrearInformation(dtFrom, dtTo);

        DisplayMessage(affectedrecord + " record affected");


    }




    public int GetEmployeeeArrearInformation(DateTime dtFromDate, DateTime DtToYear)
    {


        double EmpGrossSalary = 0;
        double EmployeeArrear = 0;
        double EmpMonthGrossSalary = 0;

        int counter = 0;

        foreach (GridViewRow gvrow in gvEmpSalary.Rows)
        {

            if (((CheckBox)gvrow.FindControl("chkgvSelect")).Checked)
            {
                EmployeeArrear = 0;
                EmpGrossSalary = GetEmployeeGrossSalary(((Label)gvrow.FindControl("lblEmpId")).Text);

                while (dtFromDate <= DtToYear)
                {

                    EmpMonthGrossSalary = GetEmployeeMonthGrossSalary(((Label)gvrow.FindControl("lblEmpId")).Text, dtFromDate.Month.ToString(), dtFromDate.Year.ToString());

                    if (EmpMonthGrossSalary > 0)
                    {
                        EmployeeArrear += EmpMonthGrossSalary;

                    }


                    dtFromDate = dtFromDate.AddMonths(1);
                }


                if (EmployeeArrear > 0)
                {
                    counter++;
                    gvrow.BackColor = System.Drawing.Color.Red;

                }
                 ((Label)gvrow.FindControl("lblarrear")).Text = ObjSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), EmployeeArrear.ToString());
            }



        }



        return counter;


    }


    public double GetEmployeeGrossSalary(string strEmpId)
    {

        double GrossSalary = 0;
        DataTable dtparam = objempparam.GetEmployeeParameterByEmpId(strEmpId, Session["CompId"].ToString());

        if (dtparam.Rows.Count > 0)
        {

            GrossSalary = Convert.ToDouble(dtparam.Rows[0]["Gross_Salary"].ToString());

        }

        return GrossSalary;

    }

    public double GetEmployeeMonthGrossSalary(string strEmpId, string strMonth, string strYear)
    {
        double PreviousMasterGrossSalaary = 0;
        double ActualGrossSalary = 0;
        double CurrentMasterGrossSalary = 0;
        double Arrrearamount = 0;
        double previoussalarypercentage = 0;



        DataTable dtMonth = objPayMonth.GetAllRecordPostedEmpMonth(strEmpId, strMonth, strYear,Session["CompId"].ToString());

        if (dtMonth.Rows.Count > 0)
        {

            PreviousMasterGrossSalaary = Convert.ToDouble(dtMonth.Rows[0]["Field5"].ToString());
            ActualGrossSalary = Convert.ToDouble(dtMonth.Rows[0]["Field8"].ToString());

            if (ActualGrossSalary > 0)
            {
                CurrentMasterGrossSalary = GetEmployeeGrossSalary(strEmpId);
                previoussalarypercentage = (ActualGrossSalary * 100) / PreviousMasterGrossSalaary;

                Arrrearamount = Math.Round(((CurrentMasterGrossSalary - PreviousMasterGrossSalaary) * previoussalarypercentage) / 100);

            }



        }

        return Arrrearamount;

    }

    #endregion




    #region FillEmployee

    protected void ddlDeptSalary_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillEmployee();
    }


    protected void btnAllRefreshSalary_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ddlDeptSalary.SelectedIndex = 0;
        FillEmployee();
      
    }



    public void FillEmployee()
    {
        DataTable dtEmp = Common.GetEmployeeListbyLocationIdandDepartmentValue(Session["LocId"].ToString(),ddlDeptSalary.SelectedIndex>0?ddlDeptSalary.SelectedValue:"0", true, Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["RoleId"].ToString(), Session["UserId"].ToString(), Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString());
        

        Session["dtEmpList"] = dtEmp;
        Session["dtEmpFilterList"] = dtEmp;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpSalary, dtEmp, "", "");
        lblTotalRecordSal.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        dtEmp.Dispose();

    }


    private void FillddlDeaprtment()
    {
        DataTable dtDepartment = PageControlCommon.GetEmployeeDepartmentByLocationValue(Session["LocId"].ToString(), ddlDeptSalary, Session["DBConnection"].ToString());
        objPageCmn.FillData((object)ddlDeptSalary, dtDepartment, "Dep_Name", "Dep_Id");
        
    }



    protected void btnSalarybind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (txtValueSal.Text.Trim() == "")
        {
            txtValueSal.Focus();
            DisplayMessage("Please Fill Value");
            return;
        }
        if (ddlOptionSal.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionSal.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldSal.SelectedValue + ",System.String)='" + txtValueSal.Text.Trim() + "'";
            }
            else if (ddlOptionSal.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldSal.SelectedValue + ",System.String) like '%" + txtValueSal.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldSal.SelectedValue + ",System.String) Like '" + txtValueSal.Text.Trim() + "%'";

            }
            DataTable dtEmp = (DataTable)Session["dtEmpFilterList"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpSalary, view.ToTable(), "", "");

            Session["dtEmpList"] = view.ToTable();
            Session["CHECKED_ITEMS"] = null;

            lblTotalRecordSal.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";

        }
       
        txtValueSal.Focus();
    }
    protected void btnSalaryRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        lblSelectRecordSal.Text = "";
        ddlFieldSal.SelectedIndex = 1;
        ddlOptionSal.SelectedIndex = 2;
        txtValueSal.Text = "";

        DataTable dtEmp = (DataTable)Session["dtEmpFilterList"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpSalary, dtEmp, "", "");
    
        Session["CHECKEDEMP_ITEMS"] = null;

    }

    protected void ImgbtnSelectAll_ClickSalary(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        ArrayList userdetails = new ArrayList();
        DataTable dtEmp = (DataTable)Session["dtEmpFilterList"];

        if (ViewState["Select"] == null)
        {
            //Session["CHECKEDEMP_ITEMS"] = null;
            //ViewState["Select"] = 1;
            //foreach (DataRow dr in dtEmp.Rows)
            //{
            //    //Allowance_Id

            //    // Check in the Session
            //    if (Session["CHECKEDEMP_ITEMS"] != null)
            //        userdetails = (ArrayList)Session["CHECKEDEMP_ITEMS"];

            //    if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
            //        userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));

            //}
            foreach (GridViewRow gvrow in gvEmpSalary.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            //if (userdetails != null && userdetails.Count > 0)
            //    Session["CHECKEDEMP_ITEMS"] = userdetails;

        }
        else
        {
            //Session["CHECKEDEMP_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpFilterList"];
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpSalary, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
       
    }


    protected void chkgvSelectAll_CheckedChangedSal(object sender, EventArgs e)
    {
        CheckBox ChkHeader = (CheckBox)gvEmpSalary.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gvrow in gvEmpSalary.Rows)
        {
            if (ChkHeader.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }

        }
    

    }

    protected void gvEmpSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues(gvEmpSalary);
        gvEmpSalary.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpList"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpSalary, dtEmp, "", "");
        PopulateCheckedValues(gvEmpSalary);
        
    }

    private void SaveCheckedValues(GridView GvCheckedEmployee)
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvCheckedEmployee.Rows)
        {
            index = (int)GvCheckedEmployee.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValues(GridView GvCheckedEmployee)
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvCheckedEmployee.Rows)
            {
                int index = (int)GvCheckedEmployee.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }



    #endregion




    public string GetAmountDecimal(string strAmount)
    {
        string strAmt = string.Empty;


        if (strAmount == "")
        {
            strAmount = "0";

        }
        strAmt = ObjSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), strAmount);


        return strAmt;



    }
}