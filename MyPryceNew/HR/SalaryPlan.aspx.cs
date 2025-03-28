using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

public partial class HR_SalaryPlan : System.Web.UI.Page
{
    CurrencyMaster ObjCurreny = null;
    SystemParameter ObjSys = null;
    Set_Allowance ObjAllowance = null;
    Set_Deduction ObjDeduction = null;
    Common ObjComman = null;
    Pay_SalaryPlanHeader ObjSalaryPlanHeader = null;
    Pay_SalaryPlanDetail Objsalaryplandetail = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjCurreny = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjAllowance = new Set_Allowance(Session["DBConnection"].ToString());
        ObjDeduction = new Set_Deduction(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjSalaryPlanHeader = new Pay_SalaryPlanHeader(Session["DBConnection"].ToString());
        Objsalaryplandetail = new Pay_SalaryPlanDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        //this code for "when user click on edit button in list grid then tab redirect on "new tab""
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../HR/SalaryPlan.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            Session["CHECKED_ITEMS"] = null;
            FillGrid();
            FillGridBin();
        }

        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        //Lbl_Tab_New.Text = Resources.Attendance.New;
    }

    #region AllPageCode

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        Btn_Save.Visible = clsPagePermission.bEdit;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    #endregion




    #region List
    private void FillGrid()
    {
        DataTable dtBrand = ObjSalaryPlanHeader.GetRecordTrueAll(Session["CompId"].ToString());
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtDeduction"] = dtBrand;
        Session["dtFilter_Sale_Plan"] = dtBrand;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dtBrand, "", "");
        //AllPageCode();

    }


    protected void GvsalaryPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvsalaryPlan.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Sale_Plan"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
        //AllPageCode();
        GvsalaryPlan.HeaderRow.Focus();
    }

    protected void GvsalaryPlan_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Sale_Plan"];
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
        Session["dtFilter_Sale_Plan"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
        //AllPageCode();
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
            Session["dtFilter_Sale_Plan"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
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
        hdnEditId.Value = e.CommandArgument.ToString();
        DataTable dt = ObjSalaryPlanHeader.GetRecordTruebyId(Session["CompId"].ToString(), hdnEditId.Value);

        if (dt.Rows.Count > 0)
        {

            try
            {
                Txt_Gross_Salary.Text = dt.Rows[0]["Gross_Salary"].ToString();
                Txt_Plan_Name.Text = dt.Rows[0]["Plan_Name"].ToString();
                Txt_Basic_Salary.Text = dt.Rows[0]["Basic_Salary_Percentage"].ToString();
                Txt_Basic_Sal_Amt.Text = dt.Rows[0]["Basic_Salary_Value"].ToString();
            }
            catch
            {

            }

            DataTable dttDetail = Objsalaryplandetail.GetDeduction_By_headerId(hdnEditId.Value);

            dttDetail = dttDetail.DefaultView.ToTable(true, "Trans_Id", "Ref_Id", "Calculation_Method", "Value", "Amount", "Deduction_Applicable", "Field1");

            objPageCmn.FillData((object)GvSalaryPlanDetail, dttDetail, "", "");

            //TabContainer1.ActiveTabIndex = 1;
            // Btn_New_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            Txt_Plan_Name.Focus();
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = 0;

        DataTable dt = new DataTable();
        //dt = ObjComman.GetCheckEsistenceId(e.CommandArgument.ToString(), "3");

        //if (dt.Rows.Count > 0)
        //{
        //    DisplayMessage("Record is in Use ,you cannot delete this");
        //    editid.Value = "";
        //    return;
        //}


        b = ObjSalaryPlanHeader.DeleteRecord(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        dt = ObjSalaryPlanHeader.GetDeductionFalseAll(Session["CompId"].ToString());
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");
        Session["dtBinDeduction"] = dt;
        Session["dtBinFilter"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        Session["CHECKED_ITEMS"] = null;
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
            
        }
        else
        {
            //AllPageCode();
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
            DataTable dtCust = (DataTable)Session["dtBinDeduction"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvsalaryPlanBin, view.ToTable(), "", "");

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
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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
                        Msg = ObjSalaryPlanHeader.DeleteRecord(Session["CompId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
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
        //AllPageCode();

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

        //AllPageCode();
        PopulateCheckedValues();
    }
    #endregion




    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());




        if (!checkSalaryPlanStatus())
        {
            DisplayMessage("Salary plan is invalid");
            return;
        }

        DataTable dtsalaryPlandetail = GetPlanList();
        if (dtsalaryPlandetail != null && dtsalaryPlandetail.Rows.Count > 0)
        {
            int Adjust_Remaining_Amount = 0;
            foreach (DataRow DR in dtsalaryPlandetail.Rows)
            {
                if (DR["Field1"].ToString() == "True")
                {
                    Adjust_Remaining_Amount++;
                }
            }
            if (Adjust_Remaining_Amount != 1)
            {
                DisplayMessage("Adjust Remaining Amount flag should be true with one fixed allowance");
                return;
            }
        }


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (hdnEditId.Value == "")
            {

                //insert record

                int b = ObjSalaryPlanHeader.InsertRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Txt_Plan_Name.Text, Txt_Gross_Salary.Text, Txt_Basic_Salary.Text, Txt_Basic_Sal_Amt.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), ref trns);



                InsertdetailRecord(b.ToString(), ref trns);


            }
            else
            {
                //update record

                ObjSalaryPlanHeader.UpdateRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEditId.Value, Txt_Plan_Name.Text, Txt_Gross_Salary.Text, Txt_Basic_Salary.Text, Txt_Basic_Sal_Amt.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), ref trns);



                Objsalaryplandetail.DeleteDeduction_By_headerId(hdnEditId.Value, ref trns);

                InsertdetailRecord(hdnEditId.Value, ref trns);
            }


            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();


            if (hdnEditId.Value == "")
            {
                DisplayMessage("Record Saved Successfully","green");
            }
            else
            {
                DisplayMessage("Record Updated Successfully", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);


                Btn_List_Click(null, null);
            }
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



    public bool checkSalaryPlanStatus()
    {
        bool Result = true;

        double BasicSalary = 0;
        double GrossAmt = 0;
        double AllowanceSum = 0;
        try
        {
            BasicSalary = Convert.ToDouble(Txt_Basic_Sal_Amt.Text);
        }
        catch
        {
        }
        try
        {
            GrossAmt = Convert.ToDouble(Txt_Gross_Salary.Text);
        }
        catch
        {

        }

        DataTable dt = GetPlanList();


        foreach (DataRow dr in dt.Rows)
        {
            AllowanceSum += Convert.ToDouble(dr["Amount"].ToString());
        }

        if (GrossAmt != (BasicSalary + AllowanceSum))
        {
            Result = false;
        }

        return Result;
    }
    public void InsertdetailRecord(string strHeaderId, ref SqlTransaction trns)
    {
        DataTable dtsalaryPlandetail = GetPlanList();


        foreach (DataRow dr in dtsalaryPlandetail.Rows)
        {


            Objsalaryplandetail.InsertRecord(strHeaderId, "Allowance", dr["Ref_Id"].ToString(), dr["Calculation_Method"].ToString(), dr["Value"].ToString(), dr["Amount"].ToString(), dr["Deduction_Applicable"].ToString(), dr["Field1"].ToString(), "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

        }


    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }


    #region Auto Complete Method/Funcation
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Allowance(string prefixText, int count, string contextKey)
    {
        Set_Allowance obj = new Set_Allowance(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = obj.GetDistinctAllowance(HttpContext.Current.Session["CompId"].ToString(), prefixText);
        string[] str = new string[dt.Rows.Count];

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i][0].ToString();
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
                        str[i] = dt.Rows[i]["Allowance"].ToString();
                    }
                }
            }
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Applicable_Deduction(string prefixText, int count, string contextKey)
    {
        Set_Deduction obj = new Set_Deduction(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = obj.GetDistinctDeduction(HttpContext.Current.Session["CompId"].ToString(), prefixText);
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
                dt = obj.GetDeductionTrueAll("1");
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] += dt.Rows[i]["Deduction"].ToString() + ",";
                    }
                }
            }
        }
        return str;
    }
    #endregion

    protected void Btn_List_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void Btn_New_Click(object sender, EventArgs e)
    {
        Txt_Plan_Name.Focus();
    }

    protected void Btn_Bin_Click(object sender, EventArgs e)
    {
        FillGridBin();
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

    #region calculation


    protected void Txt_Gross_Salary_OnTextChanged(object sender, EventArgs e)
    {
        double Grossamount = 0;
        double BasicSalarypercentage = 0;

        try
        {
            Grossamount = Convert.ToDouble(Txt_Gross_Salary.Text);
        }
        catch
        {

        }

        try
        {
            BasicSalarypercentage = Convert.ToDouble(Txt_Basic_Salary.Text);
        }
        catch
        {

        }


        Txt_Basic_Sal_Amt.Text = ObjSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), ((Grossamount * BasicSalarypercentage) / 100).ToString());

        Txt_Allowance.Focus();


        DataTable dtPlandetail = GetPlanList();




        for (int i = 0; i < dtPlandetail.Rows.Count; i++)
        {
            if (dtPlandetail.Rows[i]["Calculation_Method"].ToString() == "Percent")
            {

                dtPlandetail.Rows[i]["Amount"] = ObjSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), ((Convert.ToDouble(Txt_Basic_Sal_Amt.Text) * Convert.ToDouble(dtPlandetail.Rows[i]["Value"].ToString())) / 100).ToString());

            }
        }

        objPageCmn.FillData((object)GvSalaryPlanDetail, dtPlandetail, "", "");
    }


    protected void Ddl_Calculation_Method_OnTextChanged(object sender, EventArgs e)
    {
        double BasicSalary = 0;
        double calcPercentage = 0;


        try
        {
            BasicSalary = Convert.ToDouble(Txt_Basic_Sal_Amt.Text);
        }
        catch
        {

        }

        try
        {
            calcPercentage = Convert.ToDouble(Txt_Value.Text);
        }
        catch
        {

        }
        if (Ddl_Calculation_Method.SelectedValue == "Percent")
        {
            Txt_Amount.Text = ObjSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), ((BasicSalary * calcPercentage) / 100).ToString());

        }
        else
        {
            Txt_Amount.Text = ObjSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), calcPercentage.ToString());
        }

        Txt_Applicable_Deduction.Focus();
    }

    protected void Txt_Applicable_Deduction_OnTextChanged(object sender, EventArgs e)
    {
        if (Txt_Applicable_Deduction.Text != "")
        {

            foreach (string str in Txt_Applicable_Deduction.Text.Split(','))
            {

                if (str.Trim() == "")
                {
                    continue;
                }

                if (ObjDeduction.GetDeductionByDeduction(Session["CompId"].ToString(), str).Rows.Count == 0)
                {
                    DisplayMessage("select deduction in suggestion only");
                    Txt_Applicable_Deduction.Text = "";
                    Txt_Applicable_Deduction.Focus();
                    break;
                }


            }

            Txt_Applicable_Deduction.Focus();

        }


    }
    #endregion


    #region Detailsection

    protected void Txt_Allowance_OnTextChanged(object sender, EventArgs e)
    {

        if (Txt_Allowance.Text.Trim() != "")
        {
            if (ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), Txt_Allowance.Text).Rows.Count == 0)
            {
                DisplayMessage("Select allowance in suggestion only");
                Txt_Allowance.Text = "";
                Txt_Allowance.Focus();
                return;

            }


            DataTable dt = GetPlanList();


            if (hdndeductionTransId.Value == "" || hdndeductionTransId.Value == "0")
            {
                if (new DataView(dt, "Ref_Id=" + ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), Txt_Allowance.Text.Trim()).Rows[0]["Allowance_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    DisplayMessage("Allowance already Exists");
                    Txt_Allowance.Text = "";
                    Txt_Allowance.Focus();
                    return;
                }
            }
            else
            {
                if (new DataView(dt, "Ref_Id=" + ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), Txt_Allowance.Text.Trim()).Rows[0]["Allowance_Id"].ToString() + " and Trans_id<>" + hdndeductionTransId.Value + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    DisplayMessage("Allowance already Exists");
                    Txt_Allowance.Text = "";
                    Txt_Allowance.Focus();
                    return;
                }
            }

        }


        Ddl_Calculation_Method.Focus();

    }


    //delete
    protected void imgBtnEmpoloyeeDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dtemp = GetPlanList();


        dtemp = new DataView(dtemp, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        objPageCmn.FillData((object)GvSalaryPlanDetail, dtemp, "", "");

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
        // txtFromAmount.Text = GetExceedFromValue();
        dtemp.Dispose();

    }

    //edit
    protected void imgBtnEmployeeEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = GetPlanList();


        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        Txt_Allowance.Text = getAllowanceNamebyId(dt.Rows[0]["ref_id"].ToString());
        Ddl_Calculation_Method.SelectedValue = dt.Rows[0]["Calculation_Method"].ToString();
        Txt_Value.Text = dt.Rows[0]["Value"].ToString();
        Txt_Amount.Text = dt.Rows[0]["Amount"].ToString();
        Txt_Applicable_Deduction.Text = getDeductionNamebyId(dt.Rows[0]["Deduction_Applicable"].ToString());
        hdndeductionTransId.Value = e.CommandArgument.ToString();
        chkAdjustRemainingAmount.Checked = Convert.ToBoolean(dt.Rows[0]["Field1"].ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
        dt.Dispose();
    }


    protected void Btn_Add_Click(object sender, EventArgs e)
    {


        string strApplicableDeduction = string.Empty;
        string strAllowanceId = string.Empty;


        strAllowanceId = ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), Txt_Allowance.Text.Trim()).Rows[0]["Allowance_Id"].ToString();





        if (Txt_Applicable_Deduction.Text != "")
        {
            foreach (string str in Txt_Applicable_Deduction.Text.Split(','))
            {

                if (str.Trim() == "")
                {
                    continue;
                }

                if (!strApplicableDeduction.Trim().Split(',').Contains(ObjDeduction.GetDeductionByDeduction(Session["CompId"].ToString(), str).Rows[0]["Deduction_Id"].ToString()))
                {
                    strApplicableDeduction += ObjDeduction.GetDeductionByDeduction(Session["CompId"].ToString(), str).Rows[0]["Deduction_Id"].ToString() + ",";
                }

            }

        }





        DataTable dt = GetPlanList();






        if (hdndeductionTransId.Value == "" || hdndeductionTransId.Value == "0")
        {




            dt.Rows.Add();


            dt.Rows[dt.Rows.Count - 1][0] = dt.Rows.Count + 1;
            dt.Rows[dt.Rows.Count - 1][1] = strAllowanceId;
            dt.Rows[dt.Rows.Count - 1][2] = Ddl_Calculation_Method.SelectedValue;
            dt.Rows[dt.Rows.Count - 1][3] = Txt_Value.Text;
            dt.Rows[dt.Rows.Count - 1][4] = Txt_Amount.Text;
            dt.Rows[dt.Rows.Count - 1][5] = strApplicableDeduction;
            dt.Rows[dt.Rows.Count - 1][6] = chkAdjustRemainingAmount.Checked.ToString();
        }
        else
        {


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == hdndeductionTransId.Value)
                {
                    dt.Rows[i][1] = strAllowanceId;
                    dt.Rows[i][2] = Ddl_Calculation_Method.SelectedValue;
                    dt.Rows[i][3] = Txt_Value.Text;
                    dt.Rows[i][4] = Txt_Amount.Text;
                    dt.Rows[i][5] = strApplicableDeduction;
                    dt.Rows[i][6] = chkAdjustRemainingAmount.Checked.ToString();
                    break;
                }

            }

        }

        objPageCmn.FillData((object)GvSalaryPlanDetail, dt, "", "");

        ResetDetail();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
    }



    public DataTable GetPlanList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id", typeof(float));
        dt.Columns.Add("Ref_Id");
        dt.Columns.Add("Calculation_Method");
        dt.Columns.Add("Value", typeof(float));
        dt.Columns.Add("Amount", typeof(float));
        dt.Columns.Add("Deduction_Applicable");
        dt.Columns.Add("Field1");
        foreach (GridViewRow gvrow in GvSalaryPlanDetail.Rows)
        {
            DataRow dr = dt.NewRow();

            dr[0] = ((Label)gvrow.FindControl("lblTransId")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblAllowanceId")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblcalcMethod")).Text;
            dr[3] = ((Label)gvrow.FindControl("lblValue")).Text;
            dr[4] = ((Label)gvrow.FindControl("lblCalculatedAmount")).Text;
            dr[5] = ((Label)gvrow.FindControl("lbldeductionApplicable")).Text;
            dr[6] = ((Label)gvrow.FindControl("lblisadjustRemaining")).Text;
            dt.Rows.Add(dr);
        }

        return dt;
    }


    protected void btndeductionCancel_Click(object sender, EventArgs e)
    {
        ResetDetail();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
    }


    public void ResetDetail()
    {
        Txt_Allowance.Text = "";
        Txt_Value.Text = "0";
        Txt_Amount.Text = "0";
        Txt_Applicable_Deduction.Text = "";
        hdndeductionTransId.Value = "";
        Txt_Allowance.Focus();
    }

    public string getAllowanceNamebyId(string strRefId)
    {

        return ObjAllowance.GetAllowanceTruebyId(Session["CompId"].ToString(), strRefId).Rows[0]["Allowance"].ToString();


    }

    public string getDeductionNamebyId(string strRefId)
    {
        string strDeductionname = string.Empty;

        foreach (string str in strRefId.Split(','))
        {
            if (str.Trim() == "")
            {
                continue;
            }

            strDeductionname += ObjDeduction.GetDeductionTruebyId(Session["CompId"].ToString(), str).Rows[0]["Deduction"].ToString() + ",";

        }



        return strDeductionname;


    }

    #endregion




    public void Reset()
    {
        Txt_Gross_Salary.Text = "";
        Txt_Plan_Name.Text = "";
        Txt_Basic_Salary.Text = "";
        Txt_Basic_Sal_Amt.Text = "";
        hdnEditId.Value = "";
        ResetDetail();
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        Session["CHECKED_ITEMS"] = null;
        Btn_New.Text = Resources.Attendance.New;
        objPageCmn.FillData((object)GvSalaryPlanDetail, null, "", "");

    }


    protected void chkAdjustRemainingAmount_OnCheckedChanged(object sender, EventArgs e)
    {

        if (chkAdjustRemainingAmount.Checked)
        {
            Ddl_Calculation_Method.SelectedIndex = 0;

            double BasicSalary = 0;
            double GrossAmt = 0;
            double AllowanceSum = 0;
            try
            {
                BasicSalary = Convert.ToDouble(Txt_Basic_Sal_Amt.Text);
            }
            catch
            {
            }
            try
            {
                GrossAmt = Convert.ToDouble(Txt_Gross_Salary.Text);
            }
            catch
            {

            }

            DataTable dt = GetPlanList();


            if (hdndeductionTransId.Value != "0" && hdndeductionTransId.Value != "")
            {

                dt = new DataView(dt, "Trans_id<>" + hdndeductionTransId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            foreach (DataRow dr in dt.Rows)
            {
                AllowanceSum += Convert.ToDouble(dr["Amount"].ToString());
            }


            Txt_Value.Text = ObjSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), (GrossAmt - (BasicSalary + AllowanceSum)).ToString());
            Txt_Amount.Text = Txt_Value.Text;
        }
        else
        {
            Txt_Value.Text = "0";
            Txt_Amount.Text = "0";
        }

    }
}