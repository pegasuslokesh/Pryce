using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class HR_EmployeeRejoining : System.Web.UI.Page
{
    Common cmn = null;
    SystemParameter objSys = null;
    Attendance objAtt = null;
    EmployeeMaster objEmp = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAtt = new Attendance(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
           
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "255", HttpContext.Current.Session["CompId"].ToString(),  HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            dvNew.Visible = true;
            dvList.Visible = false;
            //FillGrid();
            AllPageCode();
            txtEmpName.Focus();
            txtRejoinDates.Format = objSys.SetDateFormat();
        }
        
    }
    public void AllPageCode()
    {
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("255", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code

        Page.Title = objSys.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        StrUserId = Session["UserId"].ToString();

        if (Session["EmpId"].ToString() == "0")
        {
            //btnCSave.Visible = true;
            foreach (GridViewRow Row in gvRejoining.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;

            }
            //foreach (GridViewRow Row in GvCurrency.Rows)
            //{

            //    ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
            //}
            //ImgbtnSelectAll.Visible = false;
            //imgBtnRestore.Visible = true;


        }
        else
        {

            DataTable dtAllPageCode = cmn.GetAllPagePermission(StrUserId.ToString(), strModuleId, "255",Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {

                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            //btn.Visible = true;
                        }
                        if (DtRow["Op_Id"].ToString() == "2")
                        {

                            foreach (GridViewRow Row in gvRejoining.Rows)
                            {
                                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;

                            }


                        }
                        if (DtRow["Op_Id"].ToString() == "3")
                        {


                        }
                        if (DtRow["Op_Id"].ToString() == "4")
                        {


                        }
                    }
                }
            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
    }
    protected void gvRejoining_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Emp_Rejoin"];
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
        Session["dtFilter_Emp_Rejoin"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvRejoining, dt, "", "");
        AllPageCode();
        try
        {
            gvRejoining.HeaderRow.Focus();
        }
        catch
        {
        }
    }
    private void FillGrid()
    {
        DataTable dt = objAtt.GetLeaveRequestByEmpId(Session["CompId"].ToString(), hdnEmpId.Value);
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvRejoining, dt, "", "");

        Session["dtFilter_Emp_Rejoin"] = dt;
        if (dt.Rows.Count > 0)
        {
            dvList.Visible = true;
        }
        //lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;

        if (txtEmpName.Text != "")
        {
            empid = txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hdnEmpId.Value = empid;
                FillGrid();
                AllPageCode();
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtEmpName.Text = "";
                txtEmpName.Focus();
                hdnEmpId.Value = "";
                return;
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(),"0",prefixText); //Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }
    //protected void gvRejoining_OnSorting(object sender, GridViewSortEventArgs e)
    //{
    //    HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
    //    DataTable dt = new DataTable();
    //    dt = (DataTable)Session["dtFilter_Emp_Rejoin"];
    //    DataView dv = new DataView(dt);
    //    string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
    //    dv.Sort = Query;
    //    dt = dv.ToTable();
    //    Session["dtFilter_Emp_Rejoin"] = dt;
    //    gvRejoining.DataSource = dt;
    //    gvRejoining.DataBind();
    //}
    protected void gvRejoining_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRejoining.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Emp_Rejoin"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvRejoining, dt, "", "");
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        ViewState["Trans_Id"] = Convert.ToInt32(e.CommandArgument.ToString());
        dvNew.Visible = true;
        dvList.Visible = true;
        DataTable dt = (DataTable)Session["dtFilter_Emp_Rejoin"];
        dt = new DataView(dt, "Trans_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            btnRejoinUpdate.Visible = true;
            txtRejoinDate.Text = GetDate(dt.Rows[0]["RejoiningDate"].ToString());
            txtRejoinDate.Enabled = true;

        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        dvNew.Visible = true;
        dvList.Visible = true;
        //txtValue.Focus();
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //  pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //  pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //  pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        // pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {

        // pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        // pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlList.Visible = false;
    }
    //protected void btnbind_Click(object sender, ImageClickEventArgs e)
    //{

    //    if (ddlOption.SelectedIndex != 0)
    //    {
    //        string condition = string.Empty;


    //        if (ddlOption.SelectedIndex == 1)
    //        {
    //            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
    //        }
    //        else if (ddlOption.SelectedIndex == 2)
    //        {
    //            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
    //        }
    //        else
    //        {
    //            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
    //        }
    //        DataTable dtCust = (DataTable)Session["dtFilter_Emp_Rejoin"];
    //        DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
    //        Session["dtFilter_Emp_Rejoin"] = view.ToTable();
    //        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
    //        gvRejoining.DataSource = view.ToTable();
    //        gvRejoining.DataBind();
    //        btnRefresh.Focus();

    //    }
    //}
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtRejoinDate.Text = "";
        txtRejoinDate.Enabled = false;
        txtEmpName.Text = "";
        btnRejoinUpdate.Visible = false;
        gvRejoining.DataSource = null;
        gvRejoining.DataBind();
        txtEmpName.Focus();
    }
    protected void btnRejoinUpdate_Click(object sender, EventArgs e)
    {
        if (txtRejoinDate.Text == "")
        {
            txtRejoinDate.Focus();
            DisplayMessage("Select Indemnity Date");
            return;
        }
        string Rejoin = objAtt.UpdateRejoinDate(txtRejoinDate.Text, ViewState["Trans_Id"].ToString());
        // string Rejoin = objIndemnity.InsertIndemnityRecord(ViewState["Indemnity_Id"].ToString(), Session["CompId"].ToString(), "0", txtIndemnityDate.Text, "Pending", "", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        DisplayMessage("Record Updated", "green");

        btnCancel_Click(null, null);
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
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
    }
}