using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;

public partial class ProjectManagement_ProjectDetail : System.Web.UI.Page
{
    UserMaster objUser = null;
    SystemParameter ObjSysParam = null;
    Prj_ProjectTeam objProjectteam = null;
    Common cmn = null;
    Prj_ProjectTask objProjectTask = null;
    Prj_ProjectMaster objProjctMaster = null;
    EmployeeMaster ObjEmp = null;
    Prj_Project_TaskType ObjTaskType = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
    

        objUser = new UserMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objProjectteam = new Prj_ProjectTeam(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objProjectTask = new Prj_ProjectTask(Session["DBConnection"].ToString());
        objProjctMaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        ObjEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjTaskType = new Prj_Project_TaskType(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Fillddlprojectname();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "355", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtToDate_CalendarExtender.Format = Session["DateFormat"].ToString();
            Session["dtTeamRecord"] = null;

            FillprojectTaskType();
            if (Request.QueryString["Project_Id"] != null)
            {
                if (Request.QueryString["Project_Id"].ToString() != "0")
                {
                    ddlprojectname.Value = Request.QueryString["Project_Id"].ToString();
                }

            }

        }
        FillProjectTeam();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }

    public void FillprojectTaskType()
    {
        ddlTaskType.Items.Clear();
        DataTable dt = ObjTaskType.GetAllTrueRecord();

        ddlTaskType.DataSource = dt;
        ddlTaskType.DataTextField = "TaskType_Name";
        ddlTaskType.DataValueField = "Trans_Id";
        ddlTaskType.DataBind();
        ddlTaskType.Items.Insert(0, "--Select--");


        dt.Dispose();
    }

    public void Fillddlprojectname()
    {
        string Prjectid = string.Empty;


        DataTable dt = objProjctMaster.GetAllProjectMasteer();

        dt = new DataView(dt, "IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();

        objPageCmn.FillData((object)ddlprojectname, dt, "Project_Name", "");

        dt.Dispose();
    }


    public void AllPageCode()
    {



        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());


        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        //356
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("355", (DataTable)Session["ModuleName"]);
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

        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        //356

        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "355", HttpContext.Current.Session["CompId"].ToString());


        if (dtAllPageCode.Rows.Count == 0)
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }

    }

    public void FillProjectTeam()
    {
        DataTable dt = new DataTable();

        if (ddlprojectname.SelectedIndex <= 0)
        {
            dt = ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        }
        else
        {
            dt = objProjectteam.GetRecordByProjectId("", ddlprojectname.Value.ToString());
        }



        objPageCmn.FillData((object)ddlEmployeeName, dt, "Emp_Name", "Emp_Id");

        dt.Dispose();
    }


    protected void gvrstatus_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtTeamRecord"];
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
        Session["dtTeamRecord"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvrstatus, dt, "", "");
        AllPageCode();
    }
    protected void gvrstatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvrstatus.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtTeamRecord"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvrstatus, dt, "", "");
        AllPageCode();
    }

    public string GetAssignBy(object UserId)
    {
        string EmpName = string.Empty;
        DataTable dt = new DataTable();
        dt = objUser.GetUserMasterByUserId(UserId.ToString(), Session["CompId"].ToString());

        if (dt.Rows.Count > 0)
        {
            EmpName = dt.Rows[0]["EmpName"].ToString();

        }

        return EmpName;


    }

    public string FormatTime(object input)
    {
        return Convert.ToDateTime(input).ToString("HH:mm");
    }

    public string Formatdate(object Date)
    {
        string strDate = string.Empty;
        if (Date.ToString() != "" && Convert.ToDateTime(Date.ToString()).ToString("dd/MM/yyyy") != "01/01/1900" && Convert.ToDateTime(Date.ToString()).ToString("dd/MM/yyyy") != "01-01-2000")
        {
            strDate = Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }

        return strDate;
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


    protected void btnReport_Click(object sender, EventArgs e)
    {

        if (gvrstatus.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        lblEmpName.Visible = true;
        lblEmpValue.Visible = true;
        Div_EmbValue.Visible = true;
        if (lblAssigndateNameValue.Text != "")
        {
            lblAssigndateNameValue.Visible = true;
            lblAssigndateName.Visible = true;
            Div_AssigndateNameValue.Visible = true;
        }
        else
        {
            lblAssigndateNameValue.Visible = false;
            lblAssigndateName.Visible = false;
            Div_AssigndateNameValue.Visible = false;
        }
        gvrstatus.Columns[6].Visible = false;
        ExportGridToPDF();
        lblEmpName.Visible = false;
        lblEmpValue.Visible = false;
        Div_EmbValue.Visible = false;
        lblAssigndateName.Visible = false;
        lblAssigndateNameValue.Visible = false;
        Div_AssigndateNameValue.Visible = false;
    }
    private void ExportGridToPDF()
    {
        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter hw = new HtmlTextWriter(sw))
            {
                //To Export all pages
                gvrstatus.AllowPaging = false;
                // this.BindGrid();
                btngo_Click(null, null);
                gvrstatus.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
            }
        }



        //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition", "attachment;filename=" + ddlEmployeeName.SelectedItem.Text + ".pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //pnllist.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End();

    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        objPageCmn.FillData((object)gvrstatus, null, "", "");

        Session["dtTeamRecord"] = null;


        if (ddlprojectname.Items == null)
        {
            DisplayMessage("Project Not Found");
            return;
        }
        if (ddlEmployeeName.SelectedIndex <= 0)
        {
            DisplayMessage("Select Employee Name");
            ddlEmployeeName.Focus();
            return;
        }
        else
        {
            lblEmpValue.Text = ddlEmployeeName.SelectedItem.Text.Trim();
        }

        if (txtFromDate.Text != "")
        {

            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct From Date Format dd-MMM-yyyy");
                txtFromDate.Focus();
                return;
            }
        }

        if (txtToDate.Text != "")
        {

            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct To Date Format dd-MMM-yyyy");
                txtToDate.Focus();
                return;
            }
        }


        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            if (ObjSysParam.getDateForInput(txtFromDate.Text) > ObjSysParam.getDateForInput(txtToDate.Text))
            {
                DisplayMessage("From Date cannot be greater than To Date");
                //txtFromDate.Text = "";
                //txtToDate.Text = "";
                txtFromDate.Focus();
                //rbtnMonthly.Checked = false;
                //rbtnYearly.Checked = false;
                return;
            }


            lblAssigndateNameValue.Text = "From  " + txtFromDate.Text + " To " + txtToDate.Text;


        }
        else
        {
            lblAssigndateNameValue.Text = "";
        }


        DataTable dtProjecttask = new DataTable();

        if (ddlprojectname.SelectedIndex <= 0)
        {
            dtProjecttask = objProjectTask.GetRecordTaskVisibilityandProjectId(ddlEmployeeName.Value.ToString(), "0");
        }
        else
        {
            dtProjecttask = objProjectTask.GetRecordTaskVisibilityandProjectId(ddlEmployeeName.Value.ToString(), ddlprojectname.Value.ToString());
        }


        if (ddlstatus.SelectedIndex != 0)
        {
            dtProjecttask = new DataView(dtProjecttask, "Status='" + ddlstatus.SelectedItem.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

            try
            {
                dtProjecttask = new DataView(dtProjecttask, "Assign_Date>='" + txtFromDate.Text + "' and Assign_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

        }


        if (ddlTaskType.SelectedIndex != 0)
        {

            dtProjecttask = new DataView(dtProjecttask, "TaskTypeId='" + ddlTaskType.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }

        if (ddlPriority.SelectedIndex != 0)
        {

            dtProjecttask = new DataView(dtProjecttask, "Field41='" + ddlPriority.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (dtProjecttask == null || dtProjecttask.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }


        objPageCmn.FillData((object)gvrstatus, dtProjecttask, "", "");

        Session["dtTeamRecord"] = dtProjecttask;

        try
        {
            gvrstatus.Columns[6].Visible = true;
        }
        catch
        {

        }

        dtProjecttask.Dispose();

    }



    protected void ddlempname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtProjecrtteam = new DataTable();

        if (ddlEmployeeName.SelectedIndex > 0)
        {
            //DataTable dtProjecttask = new DataTable();

            //dtProjecttask = objProjectTask.GetRecordTaskVisibilityandProjectId(ddlEmployeeName.Value.ToString(), "0");


            dtProjecrtteam = objProjectteam.GetAllProjectTeam();
            try
            {
                dtProjecrtteam = new DataView(dtProjecrtteam, "Emp_Id='" + ddlEmployeeName.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }


            objPageCmn.FillData((object)ddlprojectname, dtProjecrtteam, "Project_Name", "Project_Id");


        }
        else
        {

            Fillddlprojectname();
        }
        dtProjecrtteam.Dispose();

    }

    protected void ddlprojectname_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillProjectTeam();
    }
}