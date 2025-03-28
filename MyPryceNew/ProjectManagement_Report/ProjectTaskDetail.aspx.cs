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

public partial class ProjectManagement_Report_ProjectTaskDetailaspx : System.Web.UI.Page
{
    Common cmn = null;
    Prj_ProjectTaskDetail RptShift = null;
  
    Prj_ProjectMaster objProj = null;
    SystemParameter ObjSysParam = null;
    Prj_ProjectTask objProjTask = null;
    UserMaster objUser = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        cmn = new Common(Session["DBConnection"].ToString());
        RptShift = new Prj_ProjectTaskDetail(Session["DBConnection"].ToString());

        objProj = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objProjTask = new Prj_ProjectTask(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());

        ScriptManager sm = (ScriptManager)Master.FindControl("SM1");
        sm.RegisterPostBackControl(btnsave);
        StrUserId = Session["UserId"].ToString();
        if (!IsPostBack)
        {
            pnlReport.Visible = false;
            FillddlProjectname();
            btnreset.Visible = false;
            // btnsave.Visible = false;

        }

        GetReport();
        ObjSysParam.GetSysTitle();



        AllPageCode();
    }

    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());


        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("350", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            //Session.Abandon();
            //Response.Redirect("~/ERPLogin.aspx");
        }

        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;

        btnsave.Visible = true;
    }
    public void GetReport()
    {
        try
        {

            if (ddlprojectname.SelectedIndex == 0)
            {
                return;
            }
        }
        catch
        {

        }


        if (lblSelectRecord.Text == "")
        {
            return;
        }

        DataTable dtFilter = new DataTable();

        AttendanceDataSet rptdata = new AttendanceDataSet();

        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Prj_ProjectHistory_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Prj_ProjectHistory_ReportTableAdapter();

        adp.Fill(rptdata.sp_Prj_ProjectHistory_Report, int.Parse(ddlprojectname.SelectedValue),1);



        if (lblSelectRecord.Text != "")
        {
            dtFilter = new DataView(rptdata.sp_Prj_ProjectHistory_Report, "Task_Id in (" + lblSelectRecord.Text.Substring(0, lblSelectRecord.Text.Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();
        }

        // dtFilter = new DataView(rptdata.sp_Prj_ProjectHistory_Report, "", "", DataViewRowState.CurrentRows).ToTable();






        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";

        DataTable DtCompany = objComp.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();


        }
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
        }
        RptShift.SetImage(Imageurl);
        RptShift.setTitleName("Project Task detail");
        RptShift.setcompanyname(CompanyName);
        RptShift.setaddress(CompanyAddress);


        RptShift.DataSource = dtFilter;
        RptShift.DataMember = "sp_Prj_ProjectHistory_Report";
        rptViewer.Report = RptShift;
        rptToolBar.ReportViewer = rptViewer;



    }



    protected void lnkback_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = false;
        pnlProjectfilter.Visible = true;

    }
    public string GetAssignBy(string strUserId)
    {

        string strUsername = string.Empty;

        DataTable dt = objUser.GetUserMasterByUserId(strUserId, "");

        if (dt.Rows.Count > 0)
        {

            strUsername = dt.Rows[0]["EmpName"].ToString();

        }

        return strUsername;
    }


    protected void GvrProjecttask_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        AllPageCode();
        GvrProjecttask.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Project_TaskD"];
        GvrProjecttask.DataSource = dt;
        GvrProjecttask.DataBind();


        string temp = string.Empty;


        for (int i = 0; i < GvrProjecttask.Rows.Count; i++)
        {
            Label lblconid = (Label)GvrProjecttask.Rows[i].FindControl("lblProjectId");
            string[] split = lblSelectRecord.Text.Split(',');

            for (int j = 0; j < lblSelectRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvrProjecttask.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
    }

    protected void GvrProjecttask_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Project_TaskD"];
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
        Session["dtFilter_Project_TaskD"] = dt;
        GvrProjecttask.DataSource = dt;
        GvrProjecttask.DataBind();


    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvrProjecttask.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < GvrProjecttask.Rows.Count; i++)
        {
            ((CheckBox)GvrProjecttask.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectRecord.Text.Split(',').Contains(((Label)(GvrProjecttask.Rows[i].FindControl("lblProjectId"))).Text.Trim().ToString()))
                {
                    lblSelectRecord.Text += ((Label)(GvrProjecttask.Rows[i].FindControl("lblProjectId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvrProjecttask.Rows[i].FindControl("lblProjectId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectRecord.Text = temp;
            }
        }


    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvrProjecttask.Rows[index].FindControl("lblProjectId");
        if (((CheckBox)GvrProjecttask.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectRecord.Text += empidlist;

        }

        else
        {

            empidlist += lb.Text.ToString().Trim();
            lblSelectRecord.Text += empidlist;
            string[] split = lblSelectRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectRecord.Text = temp;
        }
    }

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {


        ViewState["Id"] = "1";
        if (lblSelectRecord.Text == "")
        {
            DisplayMessage("Please select task");

        }
        else
        {
            pnlProjectfilter.Visible = false;
            pnlReport.Visible = true;
            GetReport();

        }
    }



    protected void btnreset_Click(object sender, EventArgs e)
    {
        lblSelectRecord.Text = "";
        ddlprojectname.SelectedIndex = 0;
        ddlprojectname_SelectedIndexChanged(null, null);
        AllPageCode();
    }

    protected void ddlprojectname_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSelectRecord.Text = "";
        if (ddlprojectname.SelectedIndex != 0)
        {
            DataTable dt = new DataTable();
            dt = objProjTask.GetDataProjectId(ddlprojectname.SelectedValue);

            Session["dtFilter_Project_TaskD"] = dt;
            GvrProjecttask.DataSource = dt;
            GvrProjecttask.DataBind();
            if (dt.Rows.Count > 0)
            {
                btnsave.Visible = true;
                btnreset.Visible = true;
            }
            else
            {
                btnsave.Visible = false;
                btnreset.Visible = false;
            }
        }
        else
        {
            Session["dtFilter_Project_TaskD"] = null;
            GvrProjecttask.DataSource = null;
            GvrProjecttask.DataBind();
            btnsave.Visible = false;
            btnreset.Visible = false;
        }

    }
    public string FormatTime(object input)
    {
        try
        {
            return Convert.ToDateTime(input).ToString("HH:mm");
        }
        catch
        {
            return "";
        }
    }

    public string Formatdate(object Date)
    {
        string strDate = string.Empty;
        if (Date.ToString() != "" && Convert.ToDateTime(Date.ToString()).ToString("dd/MM/yyyy") != "01/01/1900")
        {
            strDate = Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }

        return strDate;
    }

    public void FillddlProjectname()
    {
        DataTable dtProject = new DataTable();
        dtProject = objProj.GetAllRecordProjectMasteer();

        try
        {
            dtProject = new DataView(dtProject, "Field4='" + Session["CompId"].ToString() + "' and Field5='" + Session["BrandId"].ToString() + "' and Field6='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }


        if (dtProject.Rows.Count > 0)
        {

            ddlprojectname.DataSource = dtProject;
            ddlprojectname.DataTextField = "Project_Name";
            ddlprojectname.DataValueField = "Project_Id";
            ddlprojectname.DataBind();



            ListItem li = new ListItem("--Select--", "-Select--");
            ddlprojectname.Items.Insert(0, li);
            ddlprojectname.SelectedIndex = 0;
        }
        else
        {
            ListItem li = new ListItem("--Select--", "-Select--");
            ddlprojectname.Items.Insert(0, li);
            ddlprojectname.SelectedIndex = 0;
        }

    }
}