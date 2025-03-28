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
using System.Text;
using System.Data.SqlClient;
using PegasusDataAccess;

public partial class HR_DeductionMaster : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    Set_Deduction ObjAddDed = null;
    SystemParameter ObjSysParam = null;
    Set_Pay_Employee_Allow_Deduc ObjAllDeduc = null;
    Set_DeductionDetail ObjDeductiondetail = null;
    PageControlCommon objPageCmn = null;
    Common ObjComman = null;
    string StrCompId = string.Empty;
    string StrUserId = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();

        cmn = new Common(Session["DBConnection"].ToString());
        ObjAddDed = new Set_Deduction(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjAllDeduc = new Set_Pay_Employee_Allow_Deduc(Session["DBConnection"].ToString());
        ObjDeductiondetail = new Set_DeductionDetail(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/DeductionMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);


            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();

            ddlOption.SelectedIndex = 2;
            FillGridBin();
            FillGrid();
            //pnlBin.Visible = false;
            btnNew_Click(null, null);
            Session["CHECKED_ITEMS"] = null;
        }

        Page.Title = ObjSysParam.GetSysTitle();

    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnCSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    #region User Defined Funcation
    public void FillGridBin()
    {

        DataTable dt = new DataTable();
        dt = ObjAddDed.GetDeductionFalseAll(StrCompId.ToString());
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvDeductionBin, dt, "", "");
        Session["dtBinDeduction"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        Session["CHECKED_ITEMS"] = null;

    }
    private void FillGrid()
    {
        DataTable dtBrand = ObjAddDed.GetDeductionTrueAll(StrCompId.ToString(), ddlLocation.SelectedValue);
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtDeduction"] = dtBrand;
        Session["dtFilter_Deud_Master"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvDeduction, dtBrand, "", "");
        }
        else
        {
            GvDeduction.DataSource = null;
            GvDeduction.DataBind();
        }


    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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
    public void Reset()
    {
        chkAddToAllLocation.Enabled = true;
        chkAddToAllLocation.Checked = false;
        txtDeductionName.Text = "";
        editid.Value = "";
        Lbl_New_tab.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        Session["CHECKED_ITEMS"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtDeductionNameL.Text = "";
        objPageCmn.FillData((object)GvDeductionDetail, null, "", "");
        ddlType.SelectedIndex = 0;
        txtAccountNo.Text = "";
        ResetDetail();
        hdnBransID.Value = "";
        hdnLocationId.Value = "";
        ddlOptionType.SelectedIndex = 0;
        ddlCalculationType_TextChanged(null, null);
    }
    #endregion
    #region Auto Complete Method/Funcation

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Set_Deduction obj = new Set_Deduction(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = obj.GetDistinctDeduction(HttpContext.Current.Session["CompId"].ToString(), prefixText);
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
                dt = obj.GetDeductionTrueAll("1");
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Deduction"].ToString();
                    }
                }
            }
        }
        return str;
    }
    #endregion

    #region System Defined Funcation

    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");


        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlList.Visible = true;

        // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_Bin_tab_position()", true);

        txtDeductionName.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtAllowdeduction = ObjAllDeduc.GetPayAllowDeducAll(Session["CompId"].ToString());
        if (dtAllowdeduction.Rows.Count > 0)
        {
            try
            {
                dtAllowdeduction = new DataView(dtAllowdeduction, "Type=2 and Ref_Id=" + e.CommandArgument.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        if (dtAllowdeduction.Rows.Count > 0)
        {
            DisplayMessage("This Deduction In Use,You Can Not Delete");
            return;
        }
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = StrCompId.ToString();
        String UserId = StrUserId.ToString();
        b = ObjAddDed.DeleteDeduction(StrCompId.ToString(), editid.Value, "false", StrUserId.ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGridBin();
        FillGrid();
        Reset();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlList.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_Bin_tab_position()", true);

        FillGridBin();
        txtValueBin.Focus();
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
    }

    protected void btnCSave_Click(object sender, EventArgs e)
    {

        string strIncludeDay = string.Empty;

        if (ddlOptionType.SelectedIndex == 1)
        {
            //present day
            if (chkPresent.Checked)
            {
                strIncludeDay = "0" + ",";
            }
            if (chkweekoff.Checked)
            {
                strIncludeDay += "1" + ",";
            }
            if (chkHoliday.Checked)
            {
                strIncludeDay += "2" + ",";
            }
            if (chkabsent.Checked)
            {
                strIncludeDay += "3" + ",";
            }
            if (chkPaidLeave.Checked)
            {
                strIncludeDay += "4" + ",";
            }
            if (chkUnpaidLeave.Checked)
            {
                strIncludeDay += "5" + ",";
            }
            if (chkHalfday.Checked)
            {
                strIncludeDay += "6" + ",";
            }


            if (strIncludeDay.Trim() == "")
            {
                DisplayMessage("select day option for day basis calculation");
                chkPresent.Focus();
                return;
            }

        }

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        if (txtDeductionName.Text.Trim() == "" || txtDeductionName.Text.Trim() == null)
        {
            DisplayMessage("Enter Deduction Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
            return;
        }


        if (editid.Value != "")
        {
            //here we check that record is exist or not in true mode
            //if exists than showing message and return the function
            //code start
            DataTable dtCate = ObjAddDed.GetDeductionTrueAll(StrCompId.ToString());
            dtCate = new DataView(dtCate, "Deduction='" + txtDeductionName.Text.Trim() + "' and Deduction_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {


                DisplayMessage("Deduction Already Exists");
                dtCate.Dispose();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;

            }
            //code end


            //here we check that record is exist or not in false mode
            //if exists than showing message and return the function

            //code start
            dtCate = ObjAddDed.GetDeductionFalseAll(StrCompId.ToString());
            dtCate = new DataView(dtCate, "Deduction='" + txtDeductionName.Text.Trim() + "' and Deduction_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {


                DisplayMessage("Deduction Already Exists-Go to Bin Tab");
                dtCate.Dispose();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;

            }
        }
        else
        {

            DataTable dtPro = ObjAddDed.GetDeductionTrueAll(StrCompId.ToString());
            dtPro = new DataView(dtPro, "Deduction='" + txtDeductionName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {

                DisplayMessage("Deduction Name Already Exists");
                dtPro.Dispose();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;
            }

            //code end

            //here we check that record is exist or not in False mode
            //if exists than showing message and return the function
            //code start
            dtPro = ObjAddDed.GetDeductionFalseAll(StrCompId.ToString());
            dtPro = new DataView(dtPro, "Deduction='" + txtDeductionName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {

                DisplayMessage("Deduction Already Exists-Go to Bin Tab");
                dtPro.Dispose();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;
            }

        }


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            int b = 0;
            if (editid.Value != "")
            {


                //this code for update the record in deductionmaster table

                //code start

                b = ObjAddDed.UpdateDeduction(StrCompId.ToString(), editid.Value, txtDeductionName.Text.Trim(), txtDeductionNameL.Text.Trim().ToString(), ddlType.SelectedValue, txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[1].ToString(), "", hdnBransID.Value, hdnLocationId.Value, true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ddlOptionType.SelectedValue, strIncludeDay, ref trns);



                ObjDeductiondetail.DeleteDeduction_By_headerId(editid.Value, ref trns);

                DataTable dtDeductionList = GetDeductionList();

                foreach (DataRow dr in dtDeductionList.Rows)
                {

                    ObjDeductiondetail.InsertDeduction(editid.Value, dr["Calculation_Type"].ToString(), dr["From_Amount"].ToString(), dr["To_Amount"].ToString(), dr["Value"].ToString(), dr["Is_Senior_Citizen"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                }


                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                }
                else
                {
                    DisplayMessage("Record  Not Updated");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                }

                //code end
            }

            else
            {
                //code start
                if (chkAddToAllLocation.Checked)
                {
                    string currentValue = ddlLocation.SelectedValue;
                    ddlLocation.SelectedIndex = 0;
                    string allLocations = ddlLocation.SelectedValue;
                    ddlLocation.SelectedValue = currentValue;

                    foreach (string LocId in allLocations.Split(','))
                    {
                        if (LocId != "")
                        {                          
                            b = ObjAddDed.InsertDeduction(StrCompId.ToString(), txtDeductionName.Text.Trim(), txtDeductionNameL.Text.Trim(), ddlType.SelectedValue, txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[1].ToString(), "", Session["BrandId"].ToString(), LocId, true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlOptionType.SelectedValue, strIncludeDay, ref trns);
                            DataTable dtDeductionList = GetDeductionList();
                            foreach (DataRow dr in dtDeductionList.Rows)
                            {
                                ObjDeductiondetail.InsertDeduction(b.ToString(), dr["Calculation_Type"].ToString(), dr["From_Amount"].ToString(), dr["To_Amount"].ToString(), dr["Value"].ToString(), dr["Is_Senior_Citizen"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                    }
                }
                else
                {
                    b = ObjAddDed.InsertDeduction(StrCompId.ToString(), txtDeductionName.Text.Trim(), txtDeductionNameL.Text.Trim(), ddlType.SelectedValue, txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[1].ToString(), "", Session["BrandId"].ToString(), Session["LocId"].ToString(), true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlOptionType.SelectedValue, strIncludeDay, ref trns);

                    DataTable dtDeductionList = GetDeductionList();

                    foreach (DataRow dr in dtDeductionList.Rows)
                    {
                        ObjDeductiondetail.InsertDeduction(b.ToString(), dr["Calculation_Type"].ToString(), dr["From_Amount"].ToString(), dr["To_Amount"].ToString(), dr["Value"].ToString(), dr["Is_Senior_Citizen"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }


                if (b != 0)
                {
                    DisplayMessage("Record Saved", "green");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
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

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        chkAddToAllLocation.Enabled = false;
        editid.Value = e.CommandArgument.ToString();

        txtAccountNo.Text = "";

        DataTable dtTax = ObjAddDed.GetDeductionTruebyId(StrCompId.ToString(), editid.Value);

        Lbl_New_tab.Text = Resources.Attendance.Edit;

        txtDeductionName.Text = dtTax.Rows[0]["Deduction"].ToString();
        txtDeductionNameL.Text = dtTax.Rows[0]["Deduction_L"].ToString();
        hdnLocationId.Value = dtTax.Rows[0]["Field5"].ToString();
        hdnBransID.Value = dtTax.Rows[0]["Field4"].ToString();

        try
        {
            ddlType.SelectedValue = dtTax.Rows[0]["Field1"].ToString();
        }
        catch
        {
            ddlType.SelectedIndex = 0;
        }

        txtAccountNo.Text = Ac_ParameterMaster.GetAccountNameByTransId(dtTax.Rows[0]["Field2"].ToString(), Session["DBConnection"].ToString(),Session["CompId"].ToString());

        ddlOptionType.SelectedValue = dtTax.Rows[0]["Calculation_Type"].ToString();
        ddlCalculationType_TextChanged(null, null);
        if (ddlOptionType.SelectedIndex == 1)
        {
            string strCalculationValue = dtTax.Rows[0]["Calculation_Value"].ToString();

            foreach (string str in strCalculationValue.Split(','))
            {
                if (str == "")
                {
                    continue;
                }

                if (str == "0")
                {
                    chkPresent.Checked = true;
                }
                if (str == "1")
                {
                    chkweekoff.Checked = true;
                }
                if (str == "2")
                {
                    chkHoliday.Checked = true;
                }
                if (str == "3")
                {
                    chkabsent.Checked = true;
                }
                if (str == "4")
                {
                    chkPaidLeave.Checked = true;
                }
                if (str == "5")
                {
                    chkUnpaidLeave.Checked = true;
                }
                if (str == "6")
                {
                    chkHalfday.Checked = true;
                }

            }

        }


        DataTable dt = ObjDeductiondetail.GetDeduction_By_headerId(editid.Value);

        dt = dt.DefaultView.ToTable(true, "Trans_Id", "Calculation_Type", "From_Amount", "To_Amount", "Value", "Is_Senior_Citizen");

        objPageCmn.FillData((object)GvDeductionDetail, dt, "", "");


        if (GvDeductionDetail.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
        }
        ResetDetail();

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
    }



    protected void GvDeduction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvDeduction.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Deud_Master"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvDeduction, dt, "", "");

        GvDeduction.HeaderRow.Focus();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
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
            objPageCmn.FillData((object)GvDeduction, view.ToTable(), "", "");
            Session["dtFilter_Deud_Master"] = view.ToTable();
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
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }


    protected void GvDeduction_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Deud_Master"];
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
        Session["dtFilter_Deud_Master"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvDeduction, dt, "", "");

        GvDeduction.HeaderRow.Focus();
    }
    protected void GvDeductionBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjAddDed.GetDeductionFalseAll(StrCompId.ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvDeductionBin, dt, "", "");
        Session["CHECKED_ITEMS"] = null;

        GvDeductionBin.HeaderRow.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        Session["CHECKED_ITEMS"] = null;
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        txtValueBin.Focus();
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {

        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {


            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text + "%'";
            }

            DataTable dtCust = (DataTable)Session["dtBinDeduction"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvDeductionBin, view.ToTable(), "", "");

            Session["CHECKED_ITEMS"] = null;

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        int Msg = 0;
        DataTable dt = ObjAddDed.GetDeductionFalseAll(StrCompId.ToString());

        if (GvDeductionBin.Rows.Count != 0)
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
                        Msg = ObjAddDed.DeleteDeduction(StrCompId.ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }

                    if (Msg != 0)
                    {
                        FillGrid();
                        FillGridBin();
                        ViewState["Select"] = null;
                        Session["CHECKED_ITEMS"] = null;
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }

    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvDeductionBin.Rows)
            {
                int index = (int)GvDeductionBin.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in GvDeductionBin.Rows)
        {
            index = (int)GvDeductionBin.DataKeys[gvrow.RowIndex].Value;
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
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        CheckBox chkSelAll = ((CheckBox)GvDeductionBin.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in GvDeductionBin.Rows)
        {
            index = (int)GvDeductionBin.DataKeys[gvrow.RowIndex].Value;
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
    protected void GvDeductionBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        GvDeductionBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvDeductionBin, dt, "", "");
        PopulateCheckedValues();

    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {

        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtbinFilter"];

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

                if (!userdetails.Contains(Convert.ToInt32(dr["Deduction_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Deduction_Id"]));

            }
            foreach (GridViewRow gvrow in GvDeductionBin.Rows)
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
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvDeductionBin, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void txtDeductionName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = ObjAddDed.GetDeductionByDeduction(StrCompId.ToString(), txtDeductionName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtDeductionName.Text = "";
                DisplayMessage("Deduction Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;
            }
            DataTable dt1 = ObjAddDed.GetDeductionFalseAll(StrCompId.ToString());
            dt1 = new DataView(dt1, "Deduction='" + txtDeductionName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtDeductionName.Text = "";
                DisplayMessage("Deduction Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;
            }
        }
        else
        {
            DataTable dtTemp = ObjAddDed.GetDeductionTruebyId(StrCompId.ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Deduction"].ToString() != txtDeductionName.Text)
                {
                    DataTable dt = ObjAddDed.GetDeductionTrueAll(StrCompId.ToString());
                    dt = new DataView(dt, "Deduction='" + txtDeductionName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtDeductionName.Text = "";
                        DisplayMessage("Deduction Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                        return;
                    }
                    DataTable dt1 = ObjAddDed.GetDeductionFalseAll(StrCompId.ToString());
                    dt1 = new DataView(dt1, "Deduction='" + txtDeductionName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtDeductionName.Text = "";
                        DisplayMessage("Deduction Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                        return;
                    }
                }
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionNameL);
    }
    #endregion


    #region LeaveDeductionSlab



    //delete
    protected void imgBtnEmpoloyeeDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dtemp = GetDeductionList();


        dtemp = new DataView(dtemp, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        objPageCmn.FillData((object)GvDeductionDetail, dtemp, "", "");

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
        // txtFromAmount.Text = GetExceedFromValue();
        dtemp.Dispose();

    }

    //edit
    protected void imgBtnEmployeeEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = GetDeductionList();


        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        txtFromAmount.Text = dt.Rows[0]["From_Amount"].ToString();
        txtToAmount.Text = dt.Rows[0]["To_Amount"].ToString();
        ddlcalculationType.SelectedValue = dt.Rows[0]["Calculation_Type"].ToString();
        txtdeductionValue.Text = dt.Rows[0]["Value"].ToString();
        chkseniorcitizen.Checked = Convert.ToBoolean(dt.Rows[0]["Is_Senior_Citizen"].ToString());
        //txtToAmount.Enabled = false;

        hdndeductionTransId.Value = e.CommandArgument.ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
        dt.Dispose();
    }


    protected void btndeduction_Click(object sender, EventArgs e)
    {



        if (float.Parse(txtFromAmount.Text) >= float.Parse(txtToAmount.Text))
        {
            DisplayMessage("From Amount should be less then To amount");
            txtFromAmount.Focus();
            return;
        }

        DataTable dt = GetDeductionList();




        if (hdndeductionTransId.Value == "" || hdndeductionTransId.Value == "0")
        {
            dt.Rows.Add();


            dt.Rows[dt.Rows.Count - 1][0] = dt.Rows.Count + 1;
            dt.Rows[dt.Rows.Count - 1][1] = ddlcalculationType.SelectedValue;
            dt.Rows[dt.Rows.Count - 1][2] = txtFromAmount.Text;
            dt.Rows[dt.Rows.Count - 1][3] = txtToAmount.Text;
            dt.Rows[dt.Rows.Count - 1][4] = txtdeductionValue.Text;
            dt.Rows[dt.Rows.Count - 1][5] = chkseniorcitizen.Checked;

        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == hdndeductionTransId.Value)
                {

                    dt.Rows[i][1] = ddlcalculationType.SelectedValue;
                    dt.Rows[i][2] = txtFromAmount.Text;
                    dt.Rows[i][3] = txtToAmount.Text;
                    dt.Rows[i][4] = txtdeductionValue.Text;
                    dt.Rows[i][5] = chkseniorcitizen.Checked;
                    break;
                }

            }

        }

        objPageCmn.FillData((object)GvDeductionDetail, dt, "", "");

        ResetDetail();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
    }



    public DataTable GetDeductionList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id", typeof(float));
        dt.Columns.Add("Calculation_Type");
        dt.Columns.Add("From_Amount", typeof(float));
        dt.Columns.Add("To_Amount", typeof(float));
        dt.Columns.Add("Value", typeof(float));
        dt.Columns.Add("Is_Senior_Citizen", typeof(bool));

        foreach (GridViewRow gvrow in GvDeductionDetail.Rows)
        {
            DataRow dr = dt.NewRow();

            dr[0] = ((Label)gvrow.FindControl("lblTransId")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblcalculationType")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblDaysFrom")).Text;
            dr[3] = ((Label)gvrow.FindControl("lblDaysTo")).Text;
            dr[4] = ((Label)gvrow.FindControl("lblValue")).Text;
            dr[5] = ((Label)gvrow.FindControl("lblSeniorcitizen")).Text;
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
        txtFromAmount.Text = "";
        txtToAmount.Text = "";
        txtdeductionValue.Text = "";
        chkseniorcitizen.Checked = false;

        txtFromAmount.Focus();
        hdndeductionTransId.Value = "";
    }



    //public string GetExceedFromValue()
    //{
    //    string strvalue = "0";

    //    DataTable dt = GetDeductionList();

    //    if (dt.Rows.Count > 0)
    //    {
    //        strvalue = (float.Parse(new DataView(dt, "", "DaysTo desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["DaysTo"].ToString()) + 1).ToString();

    //    }

    //    return strvalue;

    //}

    #endregion

    #region Finance

    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        string cls = Btnadd.Attributes["Class"].ToString();

        if (cls == "fa fa-minus")
        {
            Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
            Div_Box_Add.Attributes.Add("Class", "box box-primary");
        }



        if (((TextBox)sender).Text != "")
        {
            try
            {
                if (Ac_ParameterMaster.GetAccountNameByTransId(((TextBox)sender).Text.Split('/')[1].ToString(), Session["DBConnection"].ToString(),Session["CompId"].ToString()) == "")
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

    public string GetAccountNamebyTransId(string strTransId)
    {
        return Ac_ParameterMaster.GetAccountNameByTransId(strTransId, Session["DBConnection"].ToString(),Session["CompId"].ToString()).Split('/')[0].ToString();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Field1='False'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        string filtertext = "AccountName like '%" + prefixText + "%'";
        try
        {
            dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }


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
    #endregion

    public void fillLocation()
    {
        string location_id = "";
        ddlLocation.Items.Clear();

        DataTable dtLoc = new LocationMaster(Session["DBConnection"].ToString()).GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (!Common.GetStatus(Session["CompId"].ToString()))
        {
            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {

            for (int i = 0; i < dtLoc.Rows.Count; i++)
            {
                ddlLocation.Items.Add(new ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));

                if (i == dtLoc.Rows.Count - 1)
                {
                    location_id = location_id + dtLoc.Rows[i]["Location_Id"].ToString();
                }
                else
                {
                    location_id = location_id + dtLoc.Rows[i]["Location_Id"].ToString() + ",";
                }


            }
            ddlLocation.Items.Insert(0, new ListItem("All", location_id));
        }
        else
        {
            ddlLocation.Items.Clear();
        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void ddlCalculationType_TextChanged(object sender, EventArgs e)
    {

        chkPresent.Checked = false;
        chkweekoff.Checked = false;
        chkHoliday.Checked = false;
        chkabsent.Checked = false;
        chkPaidLeave.Checked = false;
        chkUnpaidLeave.Checked = false;
        chkHalfday.Checked = false;

        if (ddlOptionType.SelectedIndex == 0)
        {
            Div_IncludeDay.Visible = false;
        }
        else
        {
            Div_IncludeDay.Visible = true;
        }
    }


}
