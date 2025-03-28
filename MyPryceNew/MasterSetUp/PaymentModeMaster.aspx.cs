using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetUp_PaymentModeMaster : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    Set_Payment_Mode_Master objPM = null;
    Ac_ChartOfAccount objCOA = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objPM = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/PaymentModeMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            ddlOption.SelectedIndex = 2;
            FillGridBin();
            FillGrid();
            ddlFieldName.SelectedIndex = 0;
            txtPaymentModeName.Focus();
        }
        Page.Title = objSys.GetSysTitle();
        //AllPageCode();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnCSave.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    #region User Defined Funcation
    public void FillGridBin()
    {

        DataTable dt = new DataTable();
        dt = objPM.GetPaymentModeMasterInactive(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvPaymentModeBin, dt, "", "");
        Session["dtBinPaymentMode"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        Session["CHECKED_ITEMS"] = null;

    }
    private void FillGrid()
    {
        DataTable dtBrand = objPM.GetPaymentModeMaster(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtPaymentMode"] = dtBrand;
        Session["dtFilter_Payment_MMstr"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvPaymentMode, dtBrand, "", "");
            //AllPageCode();
        }
        else
        {
            GvPaymentMode.DataSource = null;
            GvPaymentMode.DataBind();
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
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public void Reset()
    {
        txtPaymentModeName.Text = "";
        txtPaymentModeNameL.Text = "";
        txtAccountName.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        Session["CHECKED_ITEMS"] = null;
        txtValue.Focus();
        txtPaymentModeNameL.Text = "";
        ddltype.SelectedIndex = 0;
    }
    #endregion

    #region System Defined Funcation


    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        //AllPageCode();
        Session["CHECKED_ITEMS"] = null;

    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentModeName);
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        if (txtPaymentModeName.Text == "" || txtPaymentModeName.Text == null)
        {
            DisplayMessage("Enter Payment Mode Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentModeName);
            return;
        }

        if (ddltype.SelectedIndex == 0)
        {
            DisplayMessage("Select Payment Type");
            ddltype.Focus();
            return;
        }


        if (txtAccountName.Text == "")
        {
            DisplayMessage("Fill Account Name");
            txtAccountName.Focus();
            return;
        }
        else
        {
            if (GetAccountId() == "")
            {
                DisplayMessage("Please Choose Account Name In Suggestions Only");
                txtAccountName.Text = "";
                txtAccountName.Focus();
                return;
            }
        }



        int b = 0;
        if (editid.Value != "")
        {

            DataTable dtCate = objPM.GetPaymentModeMaster(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dtCate = new DataView(dtCate, "Pay_Mod_Name='" + txtPaymentModeName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {
                if (dtCate.Rows[0]["Pay_Mode_Id"].ToString() != editid.Value)
                {
                    txtPaymentModeName.Text = "";
                    DisplayMessage("Payment Mode Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentModeName);
                    return;
                }
            }


            b = objPM.UpdatePaymentModeMaster(Session["CompId"].ToString().ToString(), editid.Value, txtPaymentModeName.Text, txtPaymentModeNameL.Text.Trim().ToString(), GetAccountId(), ddltype.SelectedValue, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            editid.Value = "";
            if (b != 0)
            {
                Reset();
                FillGrid();
                DisplayMessage("Record Updated", "green");

            }
            else
            {
                DisplayMessage("Record  Not Updated");
            }
        }
        else
        {
            DataTable dtPro = objPM.GetPaymentModeMaster(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dtPro = new DataView(dtPro, "Pay_Mod_Name='" + txtPaymentModeName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {
                txtPaymentModeName.Text = "";
                DisplayMessage("Payment Mode Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentModeName);
                return;
            }

            b = objPM.InsertPaymentModeMaster(Session["CompId"].ToString().ToString(), txtPaymentModeName.Text, txtPaymentModeNameL.Text.Trim(), GetAccountId(), ddltype.SelectedValue, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                Reset();
                FillGrid();
            }
            else
            {
                DisplayMessage("Record  Not Saved");
            }
        }
    }
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString();
        String UserId = Session["UserId"].ToString();
        b = objPM.DeletePaymentModeMaster(Session["CompId"].ToString().ToString(), editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Restored");
        }
        else
        {
            DisplayMessage("Record  Restore Fail");
        }
        FillGrid();
        FillGridBin();
        Reset();
    }
    protected void GvPaymentModeBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues(GvPaymentModeBin);
        GvPaymentModeBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvPaymentModeBin, dt, "", "");
        PopulateCheckedValues(GvPaymentModeBin);
        //AllPageCode();
    }
    protected void GvPaymentModeBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objPM.GetPaymentModeMasterInactive(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvPaymentModeBin, dt, "", "");
        //AllPageCode();
        Session["CHECKED_ITEMS"] = null;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dtTax = objPM.GetPaymentModeMasterById(Session["CompId"].ToString().ToString(), editid.Value, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        txtPaymentModeName.Text = dtTax.Rows[0]["Pay_Mod_Name"].ToString();
        txtPaymentModeNameL.Text = dtTax.Rows[0]["Pay_Mod_Name_L"].ToString();
        string strAccountId = dtTax.Rows[0]["Account_No"].ToString();
        DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
        if (dtAccount.Rows.Count > 0)
        {
            txtAccountName.Text = dtAccount.Rows[0]["AccountName"].ToString() + "/" + strAccountId;
        }
        ddltype.SelectedValue = dtTax.Rows[0]["Field1"].ToString();

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentModeName);
    }
    protected void GvPaymentMode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPaymentMode.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Payment_MMstr"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvPaymentMode, dt, "", "");
        //AllPageCode();
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
            DataTable dtCurrency = (DataTable)Session["dtPaymentMode"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvPaymentMode, view.ToTable(), "", "");
            Session["dtFilter_Payment_MMstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void GvPaymentMode_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Payment_MMstr"];
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
        Session["dtFilter_Payment_MMstr"] = dt;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvPaymentMode, dt, "", "");
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        String UserId = Session["UserId"].ToString().ToString();
        b = objPM.DeletePaymentModeMaster(Session["CompId"].ToString().ToString(), editid.Value, "false", Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListPaymentMode(string prefixText, int count, string contextKey)
    {
        Set_Payment_Mode_Master objPaymentMode = new Set_Payment_Mode_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objPaymentMode.GetPaymentModeMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Pay_Mod_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Pay_Mod_Name"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount objCOA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

        string filtertext = "AccountName like '" + prefixText + "%'";
        dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                txt = null;
            }
            else
            {
                dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dtCOA.Rows.Count > 0)
                {
                    txt = new string[dtCOA.Rows.Count];
                    for (int i = 0; i < dtCOA.Rows.Count; i++)
                    {
                        txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return txt;
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

            DataTable dtCust = (DataTable)Session["dtBinPaymentMode"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvPaymentModeBin, view.ToTable(), "", "");

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
        ArrayList userdetails = new ArrayList();

        SaveCheckedValues(GvPaymentModeBin);

        if (Session["CHECKED_ITEMS"] == null)
        {
            DisplayMessage("Please Select Record");
            return;
        }
        else
        {
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];

            if (userdetails.Count == 0)
            {
                DisplayMessage("Please Select Record");
                return;
            }

        }
        for (int i = 0; i < userdetails.Count; i++)
        {
            Msg = objPM.DeletePaymentModeMaster(Session["CompId"].ToString().ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }

        if (Msg != 0)
        {
            FillGrid();
            FillGridBin();
            ViewState["Select"] = null;
            Session["CHECKED_ITEMS"] = null;
            DisplayMessage("Record Activated");

        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
        //AllPageCode();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {

        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
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

                if (!userdetails.Contains(Convert.ToInt32(dr["Pay_Mode_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Pay_Mode_Id"]));

            }
            foreach (GridViewRow gvrow in GvPaymentModeBin.Rows)
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
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvPaymentModeBin, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        //AllPageCode();
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

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {


        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        CheckBox ChkHeader = (CheckBox)GvPaymentModeBin.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gvrow in GvPaymentModeBin.Rows)
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
        //AllPageCode();


    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        Session["CHECKED_ITEMS"] = null;
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void txtPaymentModeName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objPM.GetPaymentModeMasterByPaymentModeName(Session["CompId"].ToString().ToString(), txtPaymentModeName.Text.Trim(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dt.Rows.Count > 0)
            {
                txtPaymentModeName.Text = "";
                DisplayMessage("Payment Mode Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentModeName);
            }
            DataTable dt1 = objPM.GetPaymentModeMasterInactive(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dt1 = new DataView(dt1, "Pay_Mod_Name='" + txtPaymentModeName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtPaymentModeName.Text = "";
                DisplayMessage("Payment Mode Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentModeName);

            }
            txtPaymentModeNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objPM.GetPaymentModeMasterById(Session["CompId"].ToString().ToString(), editid.Value, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Pay_Mod_Name"].ToString() != txtPaymentModeName.Text)
                {
                    DataTable dt = objPM.GetPaymentModeMaster(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    dt = new DataView(dt, "Pay_Mod_Name='" + txtPaymentModeName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtPaymentModeName.Text = "";
                        DisplayMessage("Payment Mode Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentModeName);

                    }
                    DataTable dt1 = objPM.GetPaymentModeMasterInactive(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    dt1 = new DataView(dt1, "Pay_Mod_Name='" + txtPaymentModeName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtPaymentModeName.Text = "";
                        DisplayMessage("Payment Mode Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPaymentModeName);
                    }
                }
            }
            txtPaymentModeNameL.Focus();
        }
    }
    protected void txtAccountName_TextChanged(object sender, EventArgs e)
    {
        if (txtAccountName.Text != "")
        {
            DataTable dtAccountName = objCOA.GetCOAAll(Session["CompId"].ToString());
            string retval = string.Empty;
            if (txtAccountName.Text != "")
            {
                string strAccountName = txtAccountName.Text.Trim().Split('/')[0].ToString();
                dtAccountName = new DataView(dtAccountName, "AccountName='" + strAccountName + "' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccountName.Rows.Count > 0)
                {
                    retval = (txtAccountName.Text.Split('/'))[txtAccountName.Text.Split('/').Length - 1];
                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }

            if (retval != "0" && retval != "")
            {
                string strTransId = GetAccountId();
                DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strTransId);
                if (dtAccount.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnCSave);
                }
                else
                {
                    txtAccountName.Text = "";
                    DisplayMessage("Choose In Suggestions Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
                    return;
                }
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAccountName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            }
        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
        }
    }
    public string GetAccountNameByAccountId(string strAccountId)
    {
        string strAccountName = string.Empty;
        DataTable dtAccName = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
        if (dtAccName.Rows.Count > 0)
        {
            try
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString();
            }
            catch
            {

            }
        }
        return strAccountName;
    }
    private string GetAccountId()
    {
        string retval = string.Empty;
        if (txtAccountName.Text != "")
        {
            retval = (txtAccountName.Text.Split('/'))[txtAccountName.Text.Split('/').Length - 1];

            DataTable dtCOA = objCOA.GetCOAByTransId(Session["CompId"].ToString(), retval);
            if (dtCOA.Rows.Count > 0)
            {

            }
            else
            {
                retval = "0";
            }
        }
        else
        {
            retval = "0";
        }
        return retval;
    }


    #endregion
}
