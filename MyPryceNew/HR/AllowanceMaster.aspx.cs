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
using PegasusDataAccess;

public partial class HR_AllowanceMaster : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    Set_Allowance ObjAddAll = null;
    SystemParameter ObjSysParam = null;
    Set_Pay_Employee_Allow_Deduc ObjAllDeduc = null;
    DataAccessClass ObjDa = null;
    PageControlCommon objPageCmn = null;
   
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        ObjAddAll = new Set_Allowance(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjAllDeduc = new Set_Pay_Employee_Allow_Deduc(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/AllowanceMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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

            try
            {
                GvAllowance.PageSize = int.Parse(Session["GridSize"].ToString());
                GvAllowanceBin.PageSize = int.Parse(Session["GridSize"].ToString());
            }
            catch
            {

            }

            //pnlBin.Visible = false;

            btnNew_Click(null, null);
            Session["CHECKED_ITEMS"] = null;

        }
        Page.Title = ObjSysParam.GetSysTitle();
      

    }

    #region AllPageCode

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnCSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }


    #endregion

    #region User Defined Funcation
    public void FillGridBin()
    {

        DataTable dt = new DataTable();
        dt = ObjAddAll.GetAllowanceFalseAll(Session["CompId"].ToString());
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAllowanceBin, dt, "", "");
        Session["dtBinAllowance"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        lblSelectedRecord.Text = "";
      
    }
    private void FillGrid()
    {
        DataTable dtAllowance = ObjAddAll.GetAllowanceTrueAll(Session["CompId"].ToString(), ddlLocation.SelectedValue);
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtAllowance.Rows.Count + "";
        Session["dtAllowance"] = dtAllowance;
        Session["dtFilter_All_Master"] = dtAllowance;
        if (dtAllowance != null && dtAllowance.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvAllowance, dtAllowance, "", "");
        }
        else
        {
            GvAllowance.DataSource = null;
            GvAllowance.DataBind();
        }
       
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
    public void Reset()
    {

        txtAllowanceName.Text = "";
        editid.Value = "";
        Lbl_New_tab.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtAllowanceNameL.Text = "";
        txtAccountNo.Text = "";
        hdnBrand_id.Value = "";
        hdnLocation_id.Value = "";
        ddlCalculationType.SelectedIndex = 0;
        ddlCalculationType_TextChanged(null, null);


    }
    #endregion

    #region Auto Complete Method/Funcation
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
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
    #endregion
    #region System Defined Funcation

    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");


        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlList.Visible = true;

        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_Bin_tab_position()", true);
        txtAllowanceName.Focus();
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtAllowdeduction = ObjAllDeduc.GetPayAllowDeducAll(Session["CompId"].ToString());
        if (dtAllowdeduction.Rows.Count > 0)
        {
            try
            {
                dtAllowdeduction = new DataView(dtAllowdeduction, "Type=1 and Ref_Id=" + e.CommandArgument.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }

        if (dtAllowdeduction.Rows.Count > 0)
        {
            DisplayMessage("This Allowances In Use,You Can Not Delete");
            return;
        }

        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString();
        String UserId = Session["UserId"].ToString();
        b = ObjAddAll.DeleteAllowance(CompanyId.ToString(), editid.Value, "false", UserId.ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record Not Deleted");
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
        ddlFieldNameBin.Focus();
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
    }

    protected void btnCSave_Click(object sender, EventArgs e)
    {
        string strIncludeDay = string.Empty;

        if (ddlCalculationType.SelectedIndex == 1)
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


        string strAccTransId = txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[0].ToString();

        if (txtAllowanceName.Text.Trim() == "" || txtAllowanceName.Text.Trim() == null)
        {
            DisplayMessage("Enter Allowance Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
            txtAllowanceName.Text = "";
            return;
        }
        int b = 0;
        if (editid.Value != "")
        {
            //here we check that record is exist or not in true mode
            //if exists than showing message and return the function
            //code start
            DataTable dtCate = ObjAddAll.GetAllowanceTrueAll(Session["CompId"].ToString());
            dtCate = new DataView(dtCate, "Allowance='" + txtAllowanceName.Text.Trim() + "' and Allowance_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {
                DisplayMessage("Allowance Already Exists");
                txtAllowanceName.Text = "";
                txtAllowanceNameL.Text = "";

                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
                return;

            }

            //code end


            //here we check that record is exist or not in False mode
            //if exists than showing message and return the function

            //code start
            DataTable dt1 = ObjAddAll.GetAllowanceFalseAll(Session["CompId"].ToString());
            dt1 = new DataView(dt1, "Allowance='" + txtAllowanceName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtAllowanceName.Text = "";
                txtAllowanceNameL.Text = "";
                DisplayMessage("Allowance Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
                return;
            }

            //code end

            //this code for update the record in allowancemaster table
            //code start
            b = ObjAddAll.UpdateAllowance(Session["CompId"].ToString(), editid.Value, txtAllowanceName.Text.Trim(), txtAllowanceNameL.Text.Trim().ToString(), txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[1].ToString(), "", "", hdnBrand_id.Value, hdnLocation_id.Value, true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ddlCalculationType.SelectedValue.Trim(), strIncludeDay.Trim());
            editid.Value = "";
            if (b != 0)
            {
                Reset();
                FillGrid();
                DisplayMessage("Record Updated", "green");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);

            }
            else
            {
                DisplayMessage("Record  Not Updated");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
            }
            //code end
        }
        else
        {
            //here we check that record is exist or not in true mode
            //if exists than showing message and return the function
            //code start
            DataTable dtPro = ObjAddAll.GetAllowanceTrueAll(Session["CompId"].ToString());
            dtPro = new DataView(dtPro, "Allowance='" + txtAllowanceName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {
                txtAllowanceName.Text = "";
                txtAllowanceNameL.Text = "";
                DisplayMessage("Allowance Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
                return;
            }
            //code end


            //here we check that record is exist or not in true mode
            //if exists than showing message and return the function
            //code start
            DataTable dt2 = ObjAddAll.GetAllowanceFalseAll(Session["CompId"].ToString());
            dt2 = new DataView(dt2, "Allowance='" + txtAllowanceName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt2.Rows.Count > 0)
            {
                txtAllowanceName.Text = "";
                txtAllowanceNameL.Text = "";
                DisplayMessage("Allowance Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
                return;
            }

            //code end


            //here we insert the record in allowancemaster table
            //code start
            b = ObjAddAll.InsertAllowance(Session["CompId"].ToString(), txtAllowanceName.Text.Trim(), txtAllowanceNameL.Text.Trim(), txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[1].ToString(), "", "", Session["BrandId"].ToString(), Session["LocId"].ToString(), true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(),ddlCalculationType.SelectedValue.Trim(),strIncludeDay.Trim());
            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                Reset();
                FillGrid();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
            }
            else
            {
                DisplayMessage("Record Not Saved");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
            }
            //code end
        }
    }



    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        string strCalculationValue = string.Empty;
        DataTable dtTax = ObjAddAll.GetAllowanceTruebyId(Session["CompId"].ToString(), editid.Value);

        Lbl_New_tab.Text = Resources.Attendance.Edit;

        txtAllowanceName.Text = dtTax.Rows[0]["Allowance"].ToString();
        txtAllowanceNameL.Text = dtTax.Rows[0]["Allowance_L"].ToString();
        hdnLocation_id.Value = dtTax.Rows[0]["Field5"].ToString();
        hdnBrand_id.Value = dtTax.Rows[0]["Field4"].ToString();
        ddlCalculationType.SelectedValue = dtTax.Rows[0]["Calculation_Type"].ToString();
        ddlCalculationType_TextChanged(null, null);
        if(ddlCalculationType.SelectedIndex==1)
        {
            strCalculationValue = dtTax.Rows[0]["Calculation_Value"].ToString();

            foreach(string str in strCalculationValue.Split(','))
            {
                if(str=="")
                {
                    continue;
                }

                if(str=="0")
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



        txtAccountNo.Text = Ac_ParameterMaster.GetAccountNameByTransId(dtTax.Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString(),Session["CompId"].ToString());

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
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
            DataTable dtAllowance = (DataTable)Session["dtAllowance"];
            DataView view = new DataView(dtAllowance, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvAllowance, view.ToTable(), "", "");
            Session["dtFilter_All_Master"] = view.ToTable();
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



    protected void GvAllowance_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_All_Master"];
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
        Session["dtFilter_All_Master"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAllowance, dt, "", "");
       
        GvAllowance.HeaderRow.Focus();
    }

    protected void GvAllowanceBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjAddAll.GetAllowanceFalseAll(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAllowanceBin, dt, "", "");
        Session["CHECKED_ITEMS"] = null;
    
        GvAllowanceBin.HeaderRow.Focus();
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
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
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

            DataTable dtCust = (DataTable)Session["dtBinAllowance"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvAllowanceBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
           
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }


    //this code created on 20-aug-2014 by jitendra upadhyay

    //code start
    protected void GvAllowance_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvAllowance.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_All_Master"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAllowance, dt, "", "");
   
        GvAllowance.HeaderRow.Focus();
    }
    protected void GvAllowanceBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        GvAllowanceBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAllowanceBin, dt, "", "");
        PopulateCheckedValues();
   
    }
    protected void btnRestoreSelected_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        DataTable dt = ObjAddAll.GetAllowanceFalseAll(Session["CompId"].ToString());

        if (GvAllowanceBin.Rows.Count != 0)
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
                        Msg = ObjAddAll.DeleteAllowance(Session["CompId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
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

                if (!userdetails.Contains(Convert.ToInt32(dr["Allowance_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Allowance_Id"]));

            }
            foreach (GridViewRow gvrow in GvAllowanceBin.Rows)
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
            objPageCmn.FillData((object)GvAllowanceBin, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvAllowanceBin.Rows)
            {
                int index = (int)GvAllowanceBin.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in GvAllowanceBin.Rows)
        {
            index = (int)GvAllowanceBin.DataKeys[gvrow.RowIndex].Value;
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

        CheckBox chkSelAll = ((CheckBox)GvAllowanceBin.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in GvAllowanceBin.Rows)
        {
            index = (int)GvAllowanceBin.DataKeys[gvrow.RowIndex].Value;
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

    //code end

    #endregion
    #region Finance
    public string GetAccountNamebyTransId(string strTransId)
    {
        return Ac_ParameterMaster.GetAccountNameByTransId(strTransId, Session["DBConnection"].ToString(),Session["CompId"].ToString()).Split('/')[0].ToString();
    }


    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();
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

        if (!Common.GetStatus(Session["EmpId"].ToString()))
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
        if (ddlCalculationType.SelectedIndex == 0)
        {
            Div_IncludeDay.Visible = false;
        }
        else
        {
            Div_IncludeDay.Visible = true;
        }
    }
}
