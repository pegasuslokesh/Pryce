using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class HR_HR_Indemnity_Master : System.Web.UI.Page
{
    HR_Indemnity_Master objIndemnity = null;
    SystemParameter objSys = null;
    IT_ObjectEntry objObjectEntry = null;
    Common cmn = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        objIndemnity = new HR_Indemnity_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "247", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Session["dtFilter_HR_IM"] = "";
            dvNew.Visible = false;
            dvList.Visible = true;
            FillGrid();
            AllPageCode();
        }
    }
    public void AllPageCode()
    {
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("247", (DataTable)Session["ModuleName"]);
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


        if (Session["EmpId"].ToString() == "0")
        {
            btnIndemnityUpdate.Visible = true;
            foreach (GridViewRow Row in gvIndemnityMaster.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;

            }


        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "247",Session["CompId"].ToString());

            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

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
                            btnIndemnityUpdate.Visible = true;
                        }
                        foreach (GridViewRow Row in gvIndemnityMaster.Rows)
                        {
                            if (DtRow["Op_Id"].ToString() == "2")
                            {
                                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                            }

                        }

                    }
                }
            }
        }
    }
    private void FillGrid()
    {
        DataTable dt = objIndemnity.GetIndemnityEmployee();
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvIndemnityMaster, dt, "", "");
        Session["dtFilter_HR_IM"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        AllPageCode();
    }

    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }
    protected void gvIndemnityMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_HR_IM"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_HR_IM"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvIndemnityMaster, dt, "", "");
        AllPageCode();
    }

    protected void gvIndemnityMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvIndemnityMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_HR_IM"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvIndemnityMaster, dt, "", "");
        AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        ViewState["Indemnity_Id"] = Convert.ToInt32(e.CommandArgument.ToString());
        dvNew.Visible = true;
        dvList.Visible = false;
        DataTable dt = objIndemnity.GetIndemnityEmployeeById(e.CommandArgument.ToString());
        txtIndemnityDate.Text = dt.Rows[0]["Indemnity_Date"].ToString();
        AllPageCode();
        txtIndemnityDate.Focus();
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        dvNew.Visible = false;
        dvList.Visible = true;
        txtValue.Focus();
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //  pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //  pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        AllPageCode();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //  pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        // pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        AllPageCode();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {

        // pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        // pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlList.Visible = false;
        AllPageCode();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {

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
            DataTable dtCust = (DataTable)Session["dtFilter_HR_IM"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_HR_IM"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvIndemnityMaster, view.ToTable(), "", "");
            
        }
        AllPageCode();
        txtValue.Focus();
    }
    protected void btnIndemnityUpdate_Click(object sender, EventArgs e)
    {
        if (txtIndemnityDate.Text == "")
        {
            txtIndemnityDate.Focus();
            DisplayMessage("Select Indemnity Date");
            return;
        }
        int Indemnity = objIndemnity.InsertIndemnityRecord(ViewState["Indemnity_Id"].ToString(), Session["CompId"].ToString(), "0", txtIndemnityDate.Text, "Pending", "", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        DisplayMessage("Record Updated", "green");
        dvList.Visible = true;
        dvNew.Visible = false;
        txtIndemnityDate.Text = "";
        FillGrid();
        AllPageCode();
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
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
    }

}
