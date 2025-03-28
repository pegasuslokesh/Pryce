using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class SystemSetUp_SystemParameter : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    Set_Approval_Employee objApprovalEmp = null;
    Set_ApplicationParameter objParam = null;
    CurrencyMaster objCurrency = null;
    IT_ObjectEntry objObjectEntry = null;
    Inv_ParameterMaster objInvParam = null;
    PageControlCommon objPageCmn = null;

    string StrUserId = string.Empty;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;


    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objApprovalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        if (txtParameterName.Text == "Tax System")
        {
            Btn_Tax_Save.Visible = true;
            btnCSave.Visible = false;
        }
        else
        {
            Btn_Tax_Save.Visible = false;
            btnCSave.Visible = true;
        }
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SystemSetup/SystemParameter.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            ddlOption.SelectedIndex = 2;
            FillGrid();
            FillCurrencyDDL();
            ddlFieldName.SelectedIndex = 0;
            txtParameterName.Focus();
            Session["SP_Dt_Pending_Approval"] = null;
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnCSave.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
       
    }
    #region User Defined Funcation
    public void FillCurrencyDDL()
    {
        DataTable dt = objCurrency.GetCurrencyMaster();

        if (dt.Rows.Count > 0)
        {
            ddlCurrency.DataSource = null;
            ddlCurrency.DataBind();
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
        }
        else
        {
            try
            {
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
            }
            catch
            {
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
            }
        }

    }
    private void FillGrid()
    {
        DataTable dtParam = objSys.GetSysParameter_For_UserDisplay();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtParam.Rows.Count + "";
        Session["dtParameter"] = dtParam;
        Session["dtFilter_System_Para"] = dtParam;
        if (dtParam != null && dtParam.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvParameter, dtParam, "", "");
          
            showCurrencyandApprovalName();
        }
        else
        {
            GvParameter.DataSource = null;
            GvParameter.DataBind();
        }
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        DataTable dtCurrency = objCurrency.GetCurrencyMasterById(strCurrencyId);
        if (dtCurrency.Rows.Count > 0)
        {
            strCurrencyName = dtCurrency.Rows[0]["Currency_Name"].ToString();
        }
        else
        {
            strCurrencyName = "";
        }

        return strCurrencyName;
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
        Hdn_Product_Count.Value = "";
        Btn_Tax_Save.Visible = false;
        btnCSave.Visible = true;
        Hdn_Edit_Value.Value = "";
        txtParameterValue.ReadOnly = true;
        txtParameterName.Text = "";
        txtParameterValue.Text = "";
        editid.Value = "";
        //btnNew.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        ddlCurrency.Visible = false;
        ddlApprovalType.Visible = false;
        DDL_Tax_System.Visible = false;
        txtParameterValue.Visible = true;

        try
        {
            ddlCurrency.SelectedIndex = 0;
        }
        catch
        {

        }
       
    }
    #endregion

    #region System Defined Funcation



    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        if (txtParameterValue.Visible == true)
        {
            if (txtParameterValue.Text == "" || txtParameterValue.Text == null)
            {
                DisplayMessage("Enter Parameter Value");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtParameterValue);
                return;
            }
        }
        if (ddlCurrency.Visible == true)
        {
            if (ddlCurrency.SelectedIndex == 0)
            {
                DisplayMessage("Select Currency");
                return;
            }
        }


        int b = 0;
        if (editid.Value != "")
        {
            if (txtParameterValue.Visible == true)
            {
                b = objSys.UpdateSysParameterMaster(editid.Value, txtParameterName.Text, txtParameterValue.Text.Trim().ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
            }

            if (ddlCurrency.Visible == true)
            {
                b = objSys.UpdateSysParameterMaster(editid.Value, txtParameterName.Text, ddlCurrency.SelectedValue, "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                DataTable dtCurrency = new DataTable();
                dtCurrency = objCurrency.GetCurrencyMaster();
                if (dtCurrency.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrency.Rows.Count; i++)
                    {
                        if (dtCurrency.Rows[i]["Currency_ID"].ToString() == ddlCurrency.SelectedValue)
                        {
                            objCurrency.UpdateCurrencyMaster(dtCurrency.Rows[i]["Currency_ID"].ToString(), dtCurrency.Rows[i]["Currency_Name"].ToString(), dtCurrency.Rows[i]["Currency_Name_L"].ToString(), dtCurrency.Rows[i]["Currency_Code"].ToString(), dtCurrency.Rows[i]["Currency_Symbol"].ToString(), "1", true.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                        else
                        {
                            objCurrency.UpdateCurrencyMaster(dtCurrency.Rows[i]["Currency_ID"].ToString(), dtCurrency.Rows[i]["Currency_Name"].ToString(), dtCurrency.Rows[i]["Currency_Name_L"].ToString(), dtCurrency.Rows[i]["Currency_Code"].ToString(), dtCurrency.Rows[i]["Currency_Symbol"].ToString(), "1", false.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }
            }

            if (ddlApprovalType.Visible == true)
            {
                b = objSys.UpdateSysParameterMaster(editid.Value, txtParameterName.Text, ddlApprovalType.SelectedValue.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
            }

            if (DDL_Tax_System.Visible == true)
            {
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Confirm()", true);               

                string confirmValue = Request.Form["confirm_value"];
                if (confirmValue != null)
                {
                    if (confirmValue.Length > 3)
                        confirmValue = confirmValue.Substring(confirmValue.LastIndexOf(',') + 1);
                }
                if (confirmValue == "Yes")
                {
                    b = objSys.UpdateSysParameterMaster(editid.Value, txtParameterName.Text, DDL_Tax_System.SelectedValue.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else
                {
                    Reset();
                }
            }

            editid.Value = "";
            Hdn_Edit_Value.Value = "";
            if (b != 0)
            {
                if (txtParameterName.Text == "Grid_Size")
                {
                    Session["GridSize"] = txtParameterValue.Text;
                }
                Reset();
                FillGrid();

                DisplayMessage("Record Updated", "green");
            }
            else
            {
                DisplayMessage("Record  Not Updated");
            }
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        int counter = 0;
        editid.Value = e.CommandArgument.ToString();
        txtParameterValue.ReadOnly = false;

        DataTable dtParameter = objSys.GetSysParameterByTransId(editid.Value);
        if (dtParameter.Rows.Count > 0)
        {
            txtParameterName.Text = dtParameter.Rows[0]["Param_Name"].ToString();
            if (txtParameterName.Text.Trim() == "Base Currency")
            {
                counter = 1;
                txtParameterValue.Visible = false;
                ddlCurrency.Visible = true;
                ddlApprovalType.Visible = false;
                DDL_Tax_System.Visible = false;
                try
                {
                    ddlCurrency.SelectedValue = dtParameter.Rows[0]["Param_Value"].ToString();
                }
                catch
                {
                }
            }
            if (txtParameterName.Text.Trim() == "Approval Setup")
            {
                DataTable Dt_Pending_Approval = objApprovalEmp.GetApprovalTransation(Session["CompId"].ToString());
                if (Dt_Pending_Approval != null && Dt_Pending_Approval.Rows.Count > 0)
                    Session["SP_Dt_Pending_Approval"] = Dt_Pending_Approval;
                counter = 1;
                DataTable DT_Approval_Parameter = objSys.Get_Approval_Parameter();
                if (DT_Approval_Parameter != null && DT_Approval_Parameter.Rows.Count > 0)
                {
                    //Session["SP_DT_Approval_Parameter"] = DT_Approval_Parameter;
                    Grv_Approval.DataSource = DT_Approval_Parameter;
                    Grv_Approval.DataBind();
                    foreach (GridViewRow GVR in Grv_Approval.Rows)
                    {
                        HiddenField Hdn_Approval_ID = (HiddenField)GVR.FindControl("Hdn_Approval_ID");
                        HiddenField Hdn_Approval_Level = (HiddenField)GVR.FindControl("Hdn_Approval_Level");
                        DropDownList Ddl_Approval_Type = (DropDownList)GVR.FindControl("Ddl_Approval_Type");
                        if (Hdn_Approval_ID.Value.ToString() == "14" || Hdn_Approval_ID.Value.ToString() == "16")
                        {
                            Ddl_Approval_Type.Items.Insert(0, new ListItem("Brand", "2"));
                        }
                        else
                        {
                            Ddl_Approval_Type.Items.Insert(0, new ListItem("Department", "4"));
                            Ddl_Approval_Type.Items.Insert(0, new ListItem("Location", "3"));
                            Ddl_Approval_Type.Items.Insert(0, new ListItem("Brand", "2"));
                            Ddl_Approval_Type.Items.Insert(0, new ListItem("Company", "1"));
                        }

                        if (Hdn_Approval_Level.Value != "")
                        {
                            Ddl_Approval_Type.SelectedValue = Hdn_Approval_Level.Value;
                        }
                    }
                }
                else
                {
                    Grv_Approval.DataSource = null;
                    Grv_Approval.DataBind();
                }
                editid.Value = "";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Popup()", true);
            }

            if (txtParameterName.Text.Trim() == "Tax System")
            {
                DataTable dt_count = objSys.GET_Inv_ProductTaxMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "True", "0");
                if (dt_count != null && dt_count.Rows.Count > 0)
                    Hdn_Product_Count.Value = dt_count.Rows[0]["Product_Count"].ToString();
                else
                    Hdn_Product_Count.Value = "0";
                counter = 1;
                txtParameterValue.Visible = false;
                ddlApprovalType.Visible = false;
                DDL_Tax_System.Visible = true;
                ddlCurrency.Visible = false;
                Btn_Tax_Save.Visible = true;
                btnCSave.Visible = false;
                try
                {
                    DDL_Tax_System.SelectedValue = dtParameter.Rows[0]["Param_Value"].ToString();
                    Hdn_Edit_Value.Value = dtParameter.Rows[0]["Param_Value"].ToString();
                }
                catch
                {
                }
            }

            if (counter == 0)
            {
                txtParameterValue.Visible = true;
                ddlCurrency.Visible = false;
                ddlApprovalType.Visible = false;
                DDL_Tax_System.Visible = false;
                try
                {
                    ddlCurrency.SelectedIndex = 0;
                }
                catch
                {
                }
                txtParameterValue.Text = dtParameter.Rows[0]["Param_Value"].ToString();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtParameterValue);
        }
    }
    protected void GvParameter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvParameter.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_System_Para"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvParameter, dt, "", "");
        
        showCurrencyandApprovalName();
    }
    public void showCurrencyandApprovalName()
    {
        foreach (GridViewRow gvr in GvParameter.Rows)
        {
            Label lblParameterName = (Label)gvr.FindControl("lblParameterName");
            Label lblgvParameterId = (Label)gvr.FindControl("lblParameterId");
            Label lblgvParameterValue = (Label)gvr.FindControl("lblParameterValue");
            string strCurrency = string.Empty;

            if (lblParameterName.Text.Trim() == "Base Currency")
            {
                if (lblgvParameterValue.Text != "")
                {
                    strCurrency = GetCurrencyName(lblgvParameterValue.Text);
                    if (strCurrency != "")
                    {
                        lblgvParameterValue.Text = strCurrency;
                    }
                }

            }
            if (lblParameterName.Text == "Approval Setup")
            {
                if (lblgvParameterValue.Text == "1")
                {
                    lblgvParameterValue.Text = "Company";
                }
                if (lblgvParameterValue.Text == "2")
                {
                    lblgvParameterValue.Text = "Brand";
                }
                if (lblgvParameterValue.Text == "3")
                {
                    lblgvParameterValue.Text = "Location";
                }
                if (lblgvParameterValue.Text == "4")
                {
                    lblgvParameterValue.Text = "Department";
                }

            }
        }
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
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text + "%'";
            }

            DataTable dtCurrency = (DataTable)Session["dtParameter"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvParameter, view.ToTable(), "", "");
            Session["dtFilter_System_Para"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        
            showCurrencyandApprovalName();
        }
        txtValue.Focus();
    }
    protected void GvParameter_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_System_Para"];
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
        Session["dtFilter_System_Para"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvParameter, dt, "", "");
     
        showCurrencyandApprovalName();
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
    #endregion

    protected void Btn_Tax_Save_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (txtParameterValue.Visible == true)
            {
                if (txtParameterValue.Text == "" || txtParameterValue.Text == null)
                {
                    DisplayMessage("Enter Parameter Value");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtParameterValue);
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

            int b = 0;
            int c = 0;
            if (editid.Value != "")
            {
                if (DDL_Tax_System.Visible == true)
                {
                    string confirmValue = Request.Form["confirm_value"];
                    if (confirmValue != null)
                    {
                        if (confirmValue.Length > 3)
                            confirmValue = confirmValue.Substring(confirmValue.LastIndexOf(',') + 1);
                    }
                    if (confirmValue == "Yes")
                    {
                        b = objSys.UpdateSysParameterMaster(editid.Value, txtParameterName.Text, DDL_Tax_System.SelectedValue.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        if (b != 0)
                            c = objSys.SET_Inv_ProductTaxMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "False", Session["UserId"].ToString(), "0", ref trns);

                        if (DDL_Tax_System.SelectedValue == "")
                        {
                            objInvParam.UpdateParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "IsTax", "False", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            objInvParam.UpdateParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "IsTaxSales", "False", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        editid.Value = "";
                        Hdn_Edit_Value.Value = "";
                        if (b != 0)
                        {
                            if (txtParameterName.Text == "Grid_Size")
                            {
                                Session["GridSize"] = txtParameterValue.Text;
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

                            DisplayMessage("Record Updated", "green");
                        }
                        else
                        {
                            DisplayMessage("Record  Not Updated");
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
                    else if (confirmValue == null)
                    {
                        trns.Commit();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();

                        DisplayMessage("Record Updated", "green");
                        Reset();
                    }
                    else
                    {
                        DisplayMessage("Record Not Updated");
                        Reset();
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

    protected void Btn_Approval_Save_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow GV_Row in Grv_Approval.Rows)
            {
                HiddenField Hdn_Approval_ID = (HiddenField)GV_Row.FindControl("Hdn_Approval_ID");
                HiddenField Hdn_Approval_Level = (HiddenField)GV_Row.FindControl("Hdn_Approval_Level");
                DropDownList Ddl_Approval_Type = (DropDownList)GV_Row.FindControl("Ddl_Approval_Type");
                DataTable dtTrans = Session["SP_Dt_Pending_Approval"] as DataTable;
                if (Hdn_Approval_Level.Value != Ddl_Approval_Type.SelectedValue)
                {
                    if (dtTrans != null && dtTrans.Rows.Count > 0)
                    {
                        string EmpAccessPermission = objSys.Get_Approval_Parameter_By_ID(Hdn_Approval_ID.Value).Rows[0]["Approval_Level"].ToString();
                        //if (EmpAccessPermission == "2")
                        //{
                        //    dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        //}
                        //else if (EmpAccessPermission == "3")
                        //{
                        //    dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        //}
                        //else if (EmpAccessPermission == "4")
                        //{
                        //    dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        //}
                        dtTrans = new DataView(dtTrans, "Approval_Id='" + Hdn_Approval_ID.Value + "' and Status='Pending' and  Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtTrans.Rows.Count > 0)
                        {
                            if (dtTrans.Rows[0]["Approval_Type"].ToString() == "Priority")
                            {
                                DisplayMessage("Request is in under processing , You cannot edit this approval type");
                                dtTrans.Dispose();
                                Ddl_Approval_Type.SelectedValue = EmpAccessPermission;
                                return;
                            }
                        }
                    }
                }
            }




            int b = 0;
            int c = 0;
            foreach (GridViewRow GVR in Grv_Approval.Rows)
            {
                HiddenField Hdn_Approval_ID = (HiddenField)GVR.FindControl("Hdn_Approval_ID");
                Label Lbl_Approval_Name = (Label)GVR.FindControl("Lbl_Approval_Name");
                HiddenField Hdn_Approval_Level = (HiddenField)GVR.FindControl("Hdn_Approval_Level");
                DropDownList Ddl_Approval_Type = (DropDownList)GVR.FindControl("Ddl_Approval_Type");
                if (Hdn_Approval_Level.Value != Ddl_Approval_Type.SelectedValue)
                {
                    b = objSys.Update_Approval_Parameter_Master(Hdn_Approval_ID.Value, Lbl_Approval_Name.Text, Ddl_Approval_Type.SelectedValue.ToString().Trim(), Session["UserId"].ToString());
                    if (b != 0)
                    {
                        DataTable dt = objApprovalEmp.GetApprovalToEmployee(Hdn_Approval_ID.Value, Session["CompId"].ToString(), "0", "0", "0", "1");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (Ddl_Approval_Type.SelectedValue.ToString() == "1")
                                    c = objSys.Set_Approval_Master_Employee_Update(dt.Rows[i]["C_Company_Id"].ToString(), "0", "0", "0", Hdn_Approval_ID.Value, dt.Rows[i]["Emp_Id"].ToString(), Session["UserId"].ToString());
                                else if (Ddl_Approval_Type.SelectedValue.ToString() == "2")
                                    c = objSys.Set_Approval_Master_Employee_Update(dt.Rows[i]["C_Company_Id"].ToString(), dt.Rows[i]["C_Brand_Id"].ToString(), "0", "0", Hdn_Approval_ID.Value, dt.Rows[i]["Emp_Id"].ToString(), Session["UserId"].ToString());
                                else if (Ddl_Approval_Type.SelectedValue.ToString() == "3")
                                    c = objSys.Set_Approval_Master_Employee_Update(dt.Rows[i]["C_Company_Id"].ToString(), dt.Rows[i]["C_Brand_Id"].ToString(), dt.Rows[i]["C_Location_Id"].ToString(), "0", Hdn_Approval_ID.Value, dt.Rows[i]["Emp_Id"].ToString(), Session["UserId"].ToString());
                                else if (Ddl_Approval_Type.SelectedValue.ToString() == "4")
                                    c = objSys.Set_Approval_Master_Employee_Update(dt.Rows[i]["C_Company_Id"].ToString(), dt.Rows[i]["C_Brand_Id"].ToString(), dt.Rows[i]["C_Location_Id"].ToString(), dt.Rows[i]["C_Department_Id"].ToString(), Hdn_Approval_ID.Value, dt.Rows[i]["Emp_Id"].ToString(), Session["UserId"].ToString());
                            }
                        }
                    }
                    objApprovalEmp.DeleteDuplicate_Record(Ddl_Approval_Type.SelectedValue, Hdn_Approval_ID.Value);
                }
            }
            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Popup()", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void Ddl_Approval_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddl.NamingContainer;
        HiddenField Hdn_Approval_ID = (HiddenField)row.FindControl("Hdn_Approval_ID");
        DropDownList Ddl_Approval_Type = (DropDownList)row.FindControl("Ddl_Approval_Type");

        DataTable dtTrans = Session["SP_Dt_Pending_Approval"] as DataTable;
        if (dtTrans != null && dtTrans.Rows.Count > 0)
        {
            string EmpAccessPermission = objSys.Get_Approval_Parameter_By_ID(Hdn_Approval_ID.Value).Rows[0]["Approval_Level"].ToString();
            //if (EmpAccessPermission == "2")
            //{
            //dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //else if (EmpAccessPermission == "3")
            //{
            //    dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //else if (EmpAccessPermission == "4")
            //{
            //    dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            //}
            dtTrans = new DataView(dtTrans, "Approval_Id='" + Hdn_Approval_ID.Value + "' and Status='Pending' and  Priority='True'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtTrans.Rows.Count > 0)
            {
                if (dtTrans.Rows[0]["Approval_Type"].ToString() == "Priority")
                {
                    DisplayMessage("Request is in under processing , You cannot edit this approval type");
                    dtTrans.Dispose();
                    Ddl_Approval_Type.SelectedValue = EmpAccessPermission;
                    return;
                }
            }
        }
    }
}