using iTextSharp.text;
using iTextSharp.text.pdf;
using PegasusDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Globalization;

public partial class ProjectManagement_ScrumDetail : System.Web.UI.Page
{
    PageControlCommon objPageCmn = null;
    Prj_ScrumMaster objScrumMaster = null;
    Set_DocNumber objDocNo = null;
    DataAccessClass objDa = null;
    Common cmn = null;
    UserMaster objUser = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objScrumMaster = new Prj_ScrumMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objUser= new UserMaster(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ProjectManagement/ScrumDetail.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);


            btnsave.Visible = true;
            hdnCanEdit.Value = "true";
            Session["dtTaskList"] = null;
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
            LoadProductRecord();
            BindGrid();
            txtSrmNo.Text = GetDocumentNumber();
            txtSrmdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewState["SDocNo"] = GetDocumentNumber();
            Session["ScrumReportData"] = null;




            ///string InvoiceCount = get_SingleValue("SELECT count(*) FROM Inv_SalesInvoiceHeader where  Company_Id='" + SalesObj.CompanyId + "' And Brand_Id='" + SalesObj.BrandId + "' And Location_Id='" + SalesObj.LocId + "'", ref trns);
            //InvoiceNo = SalesObj.InvoiceNo + Convert.ToString(int.Parse(InvoiceCount) + 1);
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnsave.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
    }

    protected void ddlFieldName_Changed(object sender,EventArgs e)
    {
        if (ddlFieldName.SelectedValue == "Status")
        {
            Panel1.Visible = false;
            pnlStatus.Visible = true;
        }
        else
        {
            Panel1.Visible = true;
            pnlStatus.Visible = false;
        }
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

    }

 
    protected void GvrScrumIteam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvrScrumDetail.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["ScrumList"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvrScrumDetail, dt, "", "");
        //AllPageCode();
        OnRowDataBound();
    }

    protected void ddlNewStatus_Changed(object sender,EventArgs e)
    {
        //if (ddlStatus.SelectedValue ==) ;
    }
    public void OnRowDataBound()
    {     

            foreach (GridViewRow gv in GvrScrumDetail.Rows)
            {
                Label Status = (Label)gv.FindControl("gvListStatus");

                if (Status.Text == "Pass")
                {
                    gv.BackColor = Color.LawnGreen;
                }
                if (Status.Text == "Failed")
                {
                    gv.BackColor = Color.Yellow;
                }
                if (Status.Text == "Pending")
                {
                    gv.BackColor = Color.Orange;
                }
                if(Status.Text == "Delayed")
                {
                    gv.BackColor = Color.Red;
                }
            }
       
    }
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, HttpContext.Current.Session["CompId"].ToString(), true, "156", "617", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    protected void btList_Click(object sender, EventArgs e)
    {

    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        DataTable dt = new DataTable();
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
            DataTable dtProjectteam = (DataTable)Session["ScrumList"];
            if (ddlListStatus.SelectedValue != "")
            {
                dtProjectteam = new DataView(dtProjectteam, "Status='"+ddlListStatus.SelectedItem.Text+"'", "", DataViewRowState.CurrentRows).ToTable();
                //dtProjectteam = new DataView(dtProjectteam, "Status=" + ddlListStatus.SelectedItem.Text + "", "", DataViewRowState.CurrentRows).ToTable();
            }           
            if (dtProjectteam != null)
            {
                DataView view = new DataView(dtProjectteam, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Rahul Date 18-07-2023              
                objPageCmn.FillData((object)GvrScrumDetail, view.ToTable(), "", "");
                Session["ScrumList_Filter"] = view.ToTable();
                OnRowDataBound();
                dt = view.ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
                //AllPageCode();
            }
        }
        dt.Dispose();
        txtValue.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Panel1.Visible = true;
        pnlStatus.Visible = false;
        ddlFieldName.SelectedValue = "ScrumNumber";
        DataTable dtProjectteam = (DataTable)Session["ScrumList"];
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtProjectteam.Rows.Count + "";
        objPageCmn.FillData((object)GvrScrumDetail, dtProjectteam, "", "");
        OnRowDataBound();

    }
    protected void gvproduct_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        // string ScrumId = e.CommandArgument.ToString();
        string strCmd = string.Format("window.open('../ProjectManagement_Report/PrjScrumReport.aspx?Id=" + e.CommandArgument.ToString() + "&&Type=P','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }


    protected void chkStaticsReport_Click(object sender,EventArgs e)
    {
        StaticsPanel.Visible = true;
        reportpnl.Visible = false;
    }
    protected void btnbackReport_Click(object sender,EventArgs e)
    {
        chkStaticsReport.Checked = false;
        StaticsPanel.Visible = false;
        reportpnl.Visible = true;
    }
    protected void btnStaticsReport_Click(object sender,EventArgs e)
    {
        string strCmd = string.Format("window.open('../ProjectManagement_Report/PrjScrumStaticsReport.aspx?ID="+ddlMonth.SelectedItem.Text+"&&Type=P','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    protected void GvTaskList_PageIndexChanging(object sender,EventArgs e)
    {

    }
    protected void btnListView_Click(object sender,EventArgs e)
    {
        GvListTreeView.Visible = false;
        GvrScrumDetail.Visible = true;
        btnListView.Visible = false;
        btnTreeView.Visible = true;
        BindGrid();
    }
    protected void btnTreeView_Click(object sender,EventArgs e)
    {
        btnTreeView.Visible = false;
        GvListTreeView.Nodes.Clear();
        DataTable dt = objScrumMaster.GetScrumRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        DataTable dtTemp = dt;
        if (dtTemp.Rows.Count == 0)
        {
            dtTemp = dt.Copy();
        }
        int i = 0;
        while (i < dtTemp.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dtTemp.Rows[i]["ScrumName"].ToString();
            tn.Value = dtTemp.Rows[i]["ScrumId"].ToString();
            GvListTreeView.Nodes.Add(tn);
            FillChild((dtTemp.Rows[i]["ScrumId"].ToString()), tn, dt);
            i++;
        }
        GvListTreeView.DataBind();
        dtTemp.Dispose();
        dt.Dispose();
        GvListTreeView.Visible = true;
        GvrScrumDetail.Visible = false;
        btnListView.Visible = true;
    }
    //private void FillChild(string index, TreeNode tn, DataTable dt)//fill up child nodes and respective child nodes of them 
    //{
    //    try
    //    {
    //        DataTable dtTemp = dt;
    //        int i = 0;

    //        DataTable dt1 = objScrumMaster.GetScrumDetailByScrumId(index);
    //        for (int j = 0; j < dt1.Rows.Count; j++)
    //        {
    //            TreeNode tn1 = new TreeNode();
    //            tn1.Text = "'" + dt1.Rows[j]["Task"].ToString() + "::" + dt1.Rows[j]["Project"].ToString() + "'";
    //            tn1.Value = dt1.Rows[j]["ScrumId"].ToString();
    //            tn.ChildNodes.Add(tn1);
    //        }
    //        //FillChild((dtTemp.Rows[i]["ScrumId"].ToString()), tn1, dt);
    //        i++;

    //        GvListTreeView.DataBind();
    //        dtTemp.Dispose();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    private void FillChild(string index, TreeNode tn, DataTable dt)
    {
        try
        {
            DataTable dtTemp = dt;
            int i = 0;

            DataTable dt1 = objScrumMaster.GetScrumDetailByScrumId(index);
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                TreeNode tn1 = new TreeNode();

                string task = dt1.Rows[j]["Task"].ToString();
                string project = dt1.Rows[j]["Project"].ToString();

                // Modify the text to display in a different color
                tn1.Text = "'" + task + "::" + "<span style='color: red;'>" + project + "</span>" + "";

                tn1.Value = dt1.Rows[j]["ScrumId"].ToString();
                tn.ChildNodes.Add(tn1);
            }

            //FillChild((dtTemp.Rows[i]["ScrumId"].ToString()), tn1, dt);
            i++;

            GvListTreeView.DataBind();
            dtTemp.Dispose();
        }
        catch (Exception ex)
        {
            // Handle the exception if needed
        }
    }

    protected void All_TaskClose(object sender,EventArgs e)
    {
      
        CheckBox ChkBoxHeader = (CheckBox)GvTaskList.HeaderRow.FindControl("AllTaskClose");

        if (ChkBoxHeader.Checked)
        {
            foreach (GridViewRow gv in GvTaskList.Rows)
            {
                CheckBox chkbox = (CheckBox)gv.FindControl("gvTaskChk");
                chkbox.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow gv in GvTaskList.Rows)
            {
                CheckBox chkbox = (CheckBox)gv.FindControl("gvTaskChk");
                chkbox.Checked = false;
            }
        }


    }
    protected void btnSaveTask_Click(object sender,EventArgs e)
    {
        try
        {
            foreach (GridViewRow gv in GvTaskList.Rows)
            {
                CheckBox chkbox = (CheckBox)gv.FindControl("gvTaskChk");
                HiddenField hid = (HiddenField)gv.FindControl("RowTransId");
                if (chkbox.Checked == true)
                {
                    DataTable dttask = new DataTable();
                    dttask = objDa.return_DataTable(" select * from  Prj_ScrumDetail where Trans_Id='" + hid.Value + "'");
                    if (dttask.Rows.Count > 0)
                    {
                        objDa.execute_Command("Update Prj_Project_Task Set Status='Closed' where Task_Id='"+dttask.Rows[0]["TaskId"].ToString()+"'");
                    }
                    objDa.execute_Command("Update Prj_ScrumDetail Set TaskStatus='3' where Trans_Id='" + hid.Value + "'");
                }
            }
            DataTable dt = new DataTable();
            dt = objDa.return_DataTable("Select * From Prj_ScrumDetail where TaskStatus !='3' And ScrumId=" + RowScrumId.Value + " ");
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Task Close Successfully");
                BindGrid();
            }
            else
            {
                int i = 0;
                i = objDa.execute_Command("Update  Prj_ScrumMaster Set Status='3' where ScrumId=" + RowScrumId.Value + "");              
                if (i != 0)
                {
                    DisplayMessage("Scrum Close Sccessfully");
                    BindGrid();
                }
            }
        }
        catch(Exception ex)
        {

        }
    }
    protected void IbtnClose_Command(object sender, CommandEventArgs e)
    {
        string ScrumId = e.CommandArgument.ToString();
        DataTable dt = new DataTable();
        DataTable  dtHeader = objScrumMaster.GetScrumRecordByScrumId(ScrumId);
        if (dtHeader.Rows.Count > 0)
        {
            if (dtHeader.Rows[0]["Status"].ToString() == "2")
            {
                DisplayMessage("This Scrum Is Failed you Cant Close");
                return;
            }
        }
        dt = objScrumMaster.GetScrumDetailByScrumId(ScrumId);       
        dt = new DataView(dt, "TaskStatus <> 3", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            Session["DtgvTaskList"] = dt;
            objPageCmn.FillData((object)GvTaskList, dt, "", "");
            RowScrumId.Value = ScrumId;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "<script>$(function () { $('#modelViewDetail').modal('show');});</script>", false);
        }
        else
        {
            DisplayMessage("This Scrum is already Close");
            return;
        }
    }
    //protected void ddlPageSize_OnSelectedIndexChanged1(object sender, EventArgs e)
    //{
    //    DropDownList ddl = sender as DropDownList;
    //    //int currentpagesize = int.Parse(TaskPageNo.SelectedItem.Text.ToString());
    //    // int _TotalRowCount = 0;     
    //    if (ddlOption.SelectedIndex != 0)
    //    {
    //        int Page = currentpagesize;
    //        DataTable dt = (DataTable)Session["DtgvTaskList"];
    //        GvTaskList.PageSize = Page;
    //        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0][0].ToString() + "";
    //        GvTaskList.DataSource = dt;
    //        GvTaskList.DataBind();
    //    }
    //}
    protected void GvPresent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvTaskList.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["ScrumList"];
        GvTaskList.DataSource = dt;
        GvTaskList.DataBind();
        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Modal_ViewInfo_Open();", true);
    }
    protected void IbtnEdit_Command(object sender, CommandEventArgs e)
    {
        
        editid.Value = e.CommandArgument.ToString();
        DataTable dtHeader = new DataTable();
        txtSrmNo.ReadOnly = true;
        txtstartdate.ReadOnly = true;
        CalendarExtender1.Dispose();
        CalendarExtender2.Dispose();
        txtexpenddate.ReadOnly = true;
        dtHeader = objScrumMaster.GetScrumRecordByScrumId(editid.Value);
        if (dtHeader.Rows[0]["Status"].ToString() == "3")
        {
            DisplayMessage("This Scrum is Close You Cant Edit");

        }
        else
        {
            if (dtHeader.Rows.Count > 0)
            {
                txtSrmNo.Text = dtHeader.Rows[0]["ScrumNumber"].ToString();
                txtSrmName.Text = dtHeader.Rows[0]["ScrumName"].ToString();
                txtSrmdate.Text = dtHeader.Rows[0]["ScrumDate"].ToString();
                txtstartdate.Text = dtHeader.Rows[0]["StartDate"].ToString();
                txtexpenddate.Text = dtHeader.Rows[0]["EndDate"].ToString();
                ddlStatus.SelectedValue = dtHeader.Rows[0]["Status"].ToString();
                txtAssignTo.Text = dtHeader.Rows[0]["AssignTo"].ToString();
                txtAssignDate.Text = dtHeader.Rows[0]["AssignDate"].ToString();
                txtNoOfHrs.Text = dtHeader.Rows[0]["NumberOfHours"].ToString();
                txtRemark.Text = dtHeader.Rows[0]["Remark"].ToString();
                ddlPriority.SelectedValue = dtHeader.Rows[0]["Priority"].ToString();
                if (dtHeader.Rows[0]["ParentScrumId"].ToString() != "0")
                {
                    string ParentName = objDa.get_SingleValue("Select ScrumName from Prj_ScrumMaster where ScrumId='" + dtHeader.Rows[0]["ParentScrumId"].ToString() + "'");
                    txtPerScrum.Text = ParentName + '/' + dtHeader.Rows[0]["ParentScrumId"].ToString();
                }
                txtReleaseDate.Text = dtHeader.Rows[0]["ReleaseDate"].ToString();
                txtPercentage.Text = dtHeader.Rows[0]["Percentage"].ToString();
            }
            DataTable dtScrumDetail = objScrumMaster.GetScrumDetailByScrumId(editid.Value);
            Session["dtTaskList"] = dtScrumDetail;
            LoadProductRecord();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
            txtstartdate.ReadOnly = true;
            txtexpenddate.ReadOnly = true;
        }

    }
    protected void IbtnView_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dtHeader = new DataTable();
        txtSrmNo.ReadOnly = true;
        dtHeader = objScrumMaster.GetScrumRecordByScrumId(editid.Value);
        if (dtHeader.Rows.Count > 0)
        {
            txtSrmNo.Text = dtHeader.Rows[0]["ScrumNumber"].ToString();
            txtSrmName.Text = dtHeader.Rows[0]["ScrumName"].ToString();
            txtSrmdate.Text = dtHeader.Rows[0]["ScrumDate"].ToString();
            txtstartdate.Text = dtHeader.Rows[0]["StartDate"].ToString();
            txtexpenddate.Text = dtHeader.Rows[0]["EndDate"].ToString();
            ddlStatus.SelectedValue = dtHeader.Rows[0]["Status"].ToString();
            txtAssignTo.Text = dtHeader.Rows[0]["AssignTo"].ToString();
            txtAssignDate.Text = dtHeader.Rows[0]["AssignDate"].ToString();
            txtNoOfHrs.Text = dtHeader.Rows[0]["NumberOfHours"].ToString();
            txtRemark.Text = dtHeader.Rows[0]["Remark"].ToString();
            ddlPriority.SelectedValue = dtHeader.Rows[0]["Priority"].ToString();
            if (dtHeader.Rows[0]["ParentScrumId"].ToString() != "0")
            {
                string ParentName = objDa.get_SingleValue("Select ScrumName from Prj_ScrumMaster where ScrumId='" + dtHeader.Rows[0]["ParentScrumId"].ToString() + "'");
                txtPerScrum.Text = ParentName + '/' + dtHeader.Rows[0]["ParentScrumId"].ToString();
            }
            txtReleaseDate.Text = dtHeader.Rows[0]["ReleaseDate"].ToString();
            txtPercentage.Text = dtHeader.Rows[0]["Percentage"].ToString();
        }
        DataTable dtScrumDetail = objScrumMaster.GetScrumDetailByScrumId(editid.Value);
        Session["dtTaskList"] = dtScrumDetail;
        LoadProductRecord();
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
        btnsave.Visible = false;

    }

    protected string GetUserId(string UserId)
    {
        return objUser.GetNameByUserId(UserId.ToString(), Session["CompId"].ToString());
    }
    public void BindGrid()
    {
        DataTable dt = objScrumMaster.GetScrumRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (dt.Rows.Count > 0)
        {
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Status"].ToString() == "Running")
                {
                    //DateTime date = DateTime.Now;
                    //DateTime Todate = Convert.ToDateTime(dt.Rows[i]["EndDate"].ToString());
                    //if (date > Todate)
                    //{
                    //    objDa.execute_Command("Update Prj_ScrumMaster Set Status='2' where ScrumId='"+ dt.Rows[i]["ScrumId"].ToString() + "'");
                    //}
                    DateTime date = DateTime.Now;
                    DateTime Todate = Convert.ToDateTime(dt.Rows[i]["EndDate"].ToString());

                    string formattedDate = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string formattedTodate = Todate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    // Extract the date part from formattedDate and formattedTodate
                    DateTime dateOnly = DateTime.ParseExact(formattedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime TodateOnly = DateTime.ParseExact(formattedTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    // Check if dateOnly is greater than TodateOnly
                    if (dateOnly > TodateOnly)
                    {
                        // If the condition is true, execute the update command
                        string scrumId = dt.Rows[i]["ScrumId"].ToString();
                        objDa.execute_Command("Update Prj_ScrumMaster Set Status='2' where ScrumId='" + scrumId + "'");
                    }


                }             

            }
        }
        dt.Dispose();
        dt = objScrumMaster.GetScrumRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
        objPageCmn.FillData((object)GvrScrumDetail, dt, "", "");      
        Session["ScrumList"] = dt;
        OnRowDataBound();
    }

    protected void btnFindRecord_Click(object sender,EventArgs e)
    {
        try
        {
            Session["ScrumReportData"] = null;
            string Emp_Code = "";
            if (txtReportEmp.Text != "")
            {
                Emp_Code = txtReportEmp.Text.Trim().Split('/')[1].ToString();
            }
            DataTable dt = objScrumMaster.GetScrumReport(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Emp_Code, txtFromDate.Text, txtToDate.Text, ddlReportStatus.SelectedValue);

            if (dt.Rows.Count > 0)
            {
                Session["ScrumReportData"] = dt;
                objPageCmn.FillData((object)GvReport, dt, "", "");
               
            }
            else
            {
                GvReport.DataSource = null;
                GvReport.DataBind();
                DisplayMessage("Record Not Found");
            }
        }
        catch(Exception ex)
        {
            Session["ScrumReportData"] = null;
        }
    }
    protected void btnCancelReport_Click(object sender,EventArgs e)
    {
        GvReport.DataSource = null;
        GvReport.DataBind();
        Session["ScrumReportData"] = null;
    }
    public void btnsave_Click(object sender, EventArgs e)
    {
        if (txtSrmNo.Text == ViewState["SDocNo"].ToString())
        {
            string Count = objDa.get_SingleValue("SELECT count(*) FROM Prj_ScrumMaster where  Company_Id='" + HttpContext.Current.Session["CompId"].ToString() + "' And Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' And Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'");
            txtSrmNo.Text = txtSrmNo.Text + Convert.ToString(int.Parse(Count) + 1);
        }
        if (txtSrmName.Text == "")
        {
            DisplayMessage("Please enter Scrum Name");
            return;
        }

        if (txtNoOfHrs.Text == "00:00" || txtNoOfHrs.Text =="")
        {
            DisplayMessage("Please enter No. of hours");
            return;
        }


        if (txtstartdate.Text == "" && txtexpenddate.Text == "")
        {
            DisplayMessage("Please enter Start & End Date");
        }
        if (Convert.ToDateTime(txtstartdate.Text) > Convert.ToDateTime(txtexpenddate.Text))
        {
            DisplayMessage("End Date should be greater than Start Date");
        }

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string ParentScrumId = "";
        try
        {
            if (txtPerScrum.Text == "" || txtPerScrum.Text == "0")
            {
                ParentScrumId = "0";
            }
            else
            {
                try
                {
                    ParentScrumId = txtPerScrum.Text.Split('/')[1].ToString();
                }
                catch (Exception ex)
                {
                    DisplayMessage("Select Parent Scrum in suggestion only");
                    txtPerScrum.Text = "";
                    return;
                }
            }
            string ReleaseDate = "";
            if (txtReleaseDate.Text == "")
            {
                ReleaseDate = DateTime.Now.ToString();
            }
            else
            {
                ReleaseDate = txtReleaseDate.Text;
            }
            string Percentage = "";
            if (txtPercentage.Text == "")
            {
                Percentage = "0";
            }
            else
            {
                Percentage = txtPercentage.Text;
            }
            string AssignID = "";
            if (txtAssignTo.Text != "")
            {
                try
                {
                    AssignID = txtAssignTo.Text.Split('/')[1].ToString();
                }
                catch (Exception ex)
                {
                    txtAssignTo.Text = "";
                    DisplayMessage("Select Assign To in suggestion only");
                    return;
                }
            }


            if (editid.Value != "")
            {
                int i = objScrumMaster.UpdateScrumMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtSrmNo.Text, txtSrmName.Text, txtSrmdate.Text, txtstartdate.Text, txtexpenddate.Text, ddlStatus.SelectedValue, AssignID, txtAssignDate.Text, txtNoOfHrs.Text, txtRemark.Text, ddlPriority.SelectedValue, ParentScrumId, ReleaseDate, Percentage, "", "", "", "true", "true", DateTime.Now.ToString(), "true", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), editid.Value, ref trns);
                if (i != 0)
                {
                    try
                    {
                        objScrumMaster.DeleteScrumDetail(editid.Value, ref trns);
                        DataTable dt = (DataTable)Session["dtTaskList"];
                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                string Task = dt.Rows[j]["Task"].ToString();
                                //string TaskId = Task.Split('/')[1].ToString();
                                DataTable dtProject = new DataTable();
                                string TaskPercentage = "";
                                dtProject = objScrumMaster.GetProjectId(dt.Rows[j]["Project"].ToString());

                                DataTable dtTask = objScrumMaster.GetTaskIdBySubject(dt.Rows[j]["Task"].ToString(), dtProject.Rows[0]["Project_Id"].ToString());

                                if (dt.Rows[j]["TaskPercentage"].ToString() == "")
                                {
                                    TaskPercentage = "0";
                                }
                                else
                                {
                                    TaskPercentage = dt.Rows[j]["TaskPercentage"].ToString();
                                }
                                if (ddlStatus.SelectedValue != "3")
                                {
                                    if (dt.Rows[j]["TaskStatus"].ToString() == "3")
                                    {
                                        objScrumMaster.InsertScrumDetail(editid.Value, dtProject.Rows[0]["Project_Id"].ToString(), dtTask.Rows[0]["Task_Id"].ToString(), dt.Rows[j]["TaskStatus"].ToString(), TaskPercentage, dt.Rows[j]["Remark"].ToString(), dt.Rows[j]["TaskHour"].ToString(), "", "", "", "True", DateTime.Now.ToString(), "true", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        objDa.execute_Command("Update Prj_Project_Task Set Status='Closed' where Task_Id='"+ dtTask.Rows[0]["Task_Id"].ToString() + "'");
                                    }
                                    else
                                    {
                                        objScrumMaster.InsertScrumDetail(editid.Value, dtProject.Rows[0]["Project_Id"].ToString(), dtTask.Rows[0]["Task_Id"].ToString(), dt.Rows[j]["TaskStatus"].ToString(), TaskPercentage, dt.Rows[j]["Remark"].ToString(), dt.Rows[j]["TaskHour"].ToString(), "", "", "", "True", DateTime.Now.ToString(), "true", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                    }

                                }
                                else
                                {
                                    objScrumMaster.InsertScrumDetail(editid.Value, dtProject.Rows[0]["Project_Id"].ToString(), dtTask.Rows[0]["Task_Id"].ToString(),"3", TaskPercentage, dt.Rows[j]["Remark"].ToString(),dt.Rows[j]["TaskHour"].ToString(), "", "", "", "True", DateTime.Now.ToString(), "true", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    objDa.execute_Command("Update Prj_Project_Task Set Status='Closed' where Task_Id='" + dtTask.Rows[0]["Task_Id"].ToString() + "'");

                                }

                            }

                        }
                        else
                        {
                            DisplayMessage("Please Add Scrum Detail");
                            trns.Rollback();
                            return;
                        }


                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Please Add Scrum Detail");
                        trns.Rollback();
                        return;
                    }


                }
                else
                {
                    trns.Rollback();
                }
                trns.Commit();
                trns.Dispose();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                DisplayMessage("Data Save Successfully");
                Reset();
            }
            else
            {
               
                int i = objScrumMaster.InsertScrumMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtSrmNo.Text, txtSrmName.Text, txtSrmdate.Text, txtstartdate.Text, txtexpenddate.Text, ddlStatus.SelectedValue, AssignID, txtAssignDate.Text, txtNoOfHrs.Text, txtRemark.Text, ddlPriority.SelectedValue, ParentScrumId, ReleaseDate, Percentage, "", "", "", "true", "true", DateTime.Now.ToString(), "true", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (i != 0)
                {
                    try
                    {
                        DataTable dt = (DataTable)Session["dtTaskList"];
                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                string Task = dt.Rows[j]["Task"].ToString();
                                //string TaskId = Task.Split('/')[1].ToString();
                                string TaskPercentage = "";
                                DataTable dtProject = new DataTable();
                                dtProject = objScrumMaster.GetProjectId(dt.Rows[j]["Project"].ToString());

                                DataTable dtTask = objScrumMaster.GetTaskIdBySubject(dt.Rows[j]["Task"].ToString(), dtProject.Rows[0]["Project_Id"].ToString());

                                if (dt.Rows[j]["TaskPercentage"].ToString() == "")
                                {
                                    TaskPercentage = "0";
                                }
                                else
                                {
                                    TaskPercentage = dt.Rows[j]["TaskPercentage"].ToString();
                                }
                                if (ddlStatus.SelectedValue != "3")
                                {
                                    if (dt.Rows[j]["TaskStatus"].ToString() == "3")
                                    {
                                        objScrumMaster.InsertScrumDetail(i.ToString(), dtProject.Rows[0]["Project_Id"].ToString(), dtTask.Rows[0]["Task_Id"].ToString(), dt.Rows[j]["TaskStatus"].ToString(), TaskPercentage, dt.Rows[j]["Remark"].ToString(), dt.Rows[j]["TaskHour"].ToString(), "", "", "", "True", DateTime.Now.ToString(), "true", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        objDa.execute_Command("Update Prj_Project_Task Set Status='Closed' where Task_Id='" + dtTask.Rows[0]["Task_Id"].ToString() + "'");
                                    }
                                    else
                                    {
                                        objScrumMaster.InsertScrumDetail(i.ToString(), dtProject.Rows[0]["Project_Id"].ToString(), dtTask.Rows[0]["Task_Id"].ToString(), dt.Rows[j]["TaskStatus"].ToString(), TaskPercentage, dt.Rows[j]["Remark"].ToString(), dt.Rows[j]["TaskHour"].ToString(), "", "", "", "True", DateTime.Now.ToString(), "true", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }

                                }
                                else
                                {
                                    objScrumMaster.InsertScrumDetail(i.ToString(), dtProject.Rows[0]["Project_Id"].ToString(), dtTask.Rows[0]["Task_Id"].ToString(), "3", TaskPercentage, dt.Rows[j]["Remark"].ToString(), dt.Rows[j]["TaskHour"].ToString(), "", "", "", "True", DateTime.Now.ToString(), "true", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    objDa.execute_Command("Update Prj_Project_Task Set Status='Closed' where Task_Id='" + dtTask.Rows[0]["Task_Id"].ToString() + "'");

                                }
                            }
                        }
                        else
                        {
                            DisplayMessage("Please Add Scrum Detail");
                            trns.Rollback();
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Please Add Scrum Detail");
                        trns.Rollback();
                        return;
                    }

                }
                else
                {
                    trns.Rollback();
                    return;
                }
            }
            DisplayMessage("Data Save Successfully");
            trns.Commit();
            trns.Dispose();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            Reset();
        }
        catch (Exception Ex)
        {

        }

    }

    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {

        try
        {
            
                DataTable dt = Session["ScrumReportData"] as DataTable;
            if (dt!=null && dt.Rows.Count>0)
            {
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                pdfDoc.AddHeader("Header", "Header Text");
               iTextSharp.text.Font font13 = FontFactory.GetFont("ARIAL", 11);
                iTextSharp.text.Font font18 = FontFactory.GetFont("ARIAL", 12);
                iTextSharp.text.Font font21 = FontFactory.GetFont("ARIAL", 21);
                try
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                    pdfDoc.Open();

                    if (dt.Rows.Count > 0)
                    {
                        PdfPTable PdfTable = new PdfPTable(1);

                        PdfTable.TotalWidth = 700f;
                        PdfTable.LockedWidth = true;
                        PdfPCell PdfPCell = new PdfPCell(new Phrase(new Chunk("Scrum Report", font21))) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                        PdfPCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        PdfTable.AddCell(PdfPCell);
                        //DrawLine(writer, 25f, pdfDoc.Top - 30f, pdfDoc.PageSize.Width - 25f, pdfDoc.Top - 30f, new BaseColor(System.Drawing.Color.Red));                   
                        PdfTable.HorizontalAlignment = 0;
                        PdfTable.SpacingAfter = 10;
                        // STEP 2: Set the widths of the table columns                   
                        // STEP 3: Set the table width to not resize                   
                        pdfDoc.Add(PdfTable);

                        PdfTable = new PdfPTable(dt.Columns.Count);
                        PdfTable.SpacingBefore = 20f;
                        for (int columns = 0; columns <= dt.Columns.Count - 1; columns++)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[columns].ColumnName, font18)));
                            PdfTable.AddCell(PdfPCell);
                        }

                        for (int rows = 0; rows <= dt.Rows.Count - 1; rows++)
                        {
                            for (int column = 0; column <= dt.Columns.Count - 1; column++)
                            {
                                PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font13)));
                                PdfTable.AddCell(PdfPCell);
                            }
                        }
                        pdfDoc.Add(PdfTable);
                    }
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=ScrumReport_" + DateTime.Now.Date.Day.ToString() + DateTime.Now.Date.Month.ToString() + DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Hour.ToString() + DateTime.Now.Date.Minute.ToString() + DateTime.Now.Date.Second.ToString() + DateTime.Now.Date.Millisecond.ToString() + ".pdf");
                    System.Web.HttpContext.Current.Response.Write(pdfDoc);
                    Response.Flush();
                    Response.End();
                }
                catch (DocumentException de)
                {
                }
                // System.Web.HttpContext.Current.Response.Write(de.Message)
                catch (IOException ioEx)
                {
                }
                // System.Web.HttpContext.Current.Response.Write(ioEx.Message)
                catch (Exception ex)
                {
                }
            }
            else
            {
                DisplayMessage("Record Not Found");
            }
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    
    }



    public void btnreset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    public void btncencel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void Reset()
    {
        txtPerScrum.Text = "";
        txtNoOfHrs.Text = "";
        txtRemark.Text = "";
        txtstartdate.Text = "";
        txtexpenddate.Text = "";
        txtPercentage.Text = "";
        txtAssignTo.Text = "";
        txtAssignDate.Text = "";
        txtSrmName.Text = "";
        txtValue.Text = "";
        txtReleaseDate.Text = "";
        Session["dtTaskList"] = null;
        btnsave.Visible = true;
        LoadProductRecord();
        BindGrid();
        txtSrmNo.ReadOnly = false;
        txtSrmNo.Text = GetDocumentNumber();
        txtSrmdate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        ViewState["SDocNo"] = GetDocumentNumber();


    }


    protected void gvproduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        string EmpId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvproduct.FooterRow.FindControl("txtTask")).Text == "")
            {
                DisplayMessage("Choose Task in suggestion");
                ((TextBox)gvproduct.FooterRow.FindControl("txtTask")).Focus();
                return;
            }
            else
            {
                string TaskId = ((TextBox)gvproduct.FooterRow.FindControl("txtTask")).Text;
                string ProjectId = ((TextBox)gvproduct.FooterRow.FindControl("txtProject")).Text;
                DataTable dtProject = objScrumMaster.GetProjectId(ProjectId);
                try
                {
                    DataTable dtTaskId = new DataTable();
                    dtTaskId = objScrumMaster.GetTaskIdBySubject(TaskId, dtProject.Rows[0]["Project_Id"].ToString());
                    if (dtTaskId.Rows.Count == 0)
                    {
                        DisplayMessage("Choose txtTask in suggestion");
                        ((TextBox)gvproduct.FooterRow.FindControl("txtTask")).Text = "";
                        ((TextBox)gvproduct.FooterRow.FindControl("txtTask")).Focus();
                        return;
                    }
                }
                catch
                {
                    DisplayMessage("Choose txtTask in suggestion");
                    ((TextBox)gvproduct.FooterRow.FindControl("txtTask")).Text = "";
                    ((TextBox)gvproduct.FooterRow.FindControl("txtTask")).Focus();
                    return;
                }

            }
            if (((DropDownList)gvproduct.FooterRow.FindControl("txtTaskStatus")).Text == "")
            {
                DisplayMessage("Enter Quantity");
                ((DropDownList)gvproduct.FooterRow.FindControl("txtTaskStatus")).Focus();
                return;
            }
            if (Session["dtTaskList"] == null)
            {
                dt.Columns.Add("Trans_Id", typeof(int));
                dt.Columns.Add("TaskHour", typeof(string));
                dt.Columns.Add("Task", typeof(string));
                dt.Columns.Add("Project", typeof(string));
                dt.Columns.Add("ScrumId", typeof(int));
                dt.Columns.Add("TaskStatus", typeof(string));
                dt.Columns.Add("StatusId", typeof(string));
                dt.Columns.Add("TaskPercentage", typeof(string));
                dt.Columns.Add("Cost", typeof(string));
                dt.Columns.Add("Date", typeof(string));
                dt.Columns.Add("Remark", typeof(string));
                DataRow dr = dt.NewRow();
                dr[0] = 0;
                dr[1] = ((TextBox)gvproduct.FooterRow.FindControl("txtTaskHours")).Text;
                dr[2] = ((TextBox)gvproduct.FooterRow.FindControl("txtTask")).Text;
                dr[3] = ((TextBox)gvproduct.FooterRow.FindControl("txtProject")).Text;
                dr[4] = "1";
                dr[5] = ((DropDownList)gvproduct.FooterRow.FindControl("txtTaskStatus")).SelectedValue;
                dr[6] = ((DropDownList)gvproduct.FooterRow.FindControl("txtTaskStatus")).SelectedItem.Text;
                //dr[5] = ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).SelectedItem.Text;
                if (((TextBox)gvproduct.FooterRow.FindControl("txtTaskPercentage")).Text != "")
                {
                    dr[7] = ((TextBox)gvproduct.FooterRow.FindControl("txtTaskPercentage")).Text;
                }
                else
                {
                    dr[7] = "0";
                }
                dr[8] = ((TextBox)gvproduct.FooterRow.FindControl("txtGvCost")).Text;
                dr[9] = ((TextBox)gvproduct.FooterRow.FindControl("txtTaskDate")).Text;
                dr[10] = ((TextBox)gvproduct.FooterRow.FindControl("txtgvRemark")).Text;
                dt.Rows.Add(dr);
            }
            else
            {
                try
                {
                    string strTransid = string.Empty;
                    dt = (DataTable)Session["dtTaskList"];
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtCopy = dt.Copy();
                        dtCopy = new DataView(dtCopy, "", "ScrumId desc", DataViewRowState.CurrentRows).ToTable();
                        strTransid = (float.Parse(dtCopy.Rows[0]["ScrumId"].ToString()) + 1).ToString();
                    }
                    else
                    {
                        strTransid = "1";
                    }

                    DataRow dr = dt.NewRow();
                    dr[0] = 0;
                    dr[1] = ((TextBox)gvproduct.FooterRow.FindControl("txtTaskHours")).Text;
                    dr[2] = ((TextBox)gvproduct.FooterRow.FindControl("txtTask")).Text;
                    dr[3] = ((TextBox)gvproduct.FooterRow.FindControl("txtProject")).Text;
                    dr[4] = strTransid;
                    dr[5] = ((DropDownList)gvproduct.FooterRow.FindControl("txtTaskStatus")).SelectedValue;
                    dr[6] = ((DropDownList)gvproduct.FooterRow.FindControl("txtTaskStatus")).SelectedItem.Text;
                    //dr[5] = ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).SelectedItem.Text;
                    if (((TextBox)gvproduct.FooterRow.FindControl("txtTaskPercentage")).Text != "")
                    {
                        dr[7] = ((TextBox)gvproduct.FooterRow.FindControl("txtTaskPercentage")).Text;
                    }
                    else
                    {
                        dr[7] = "0";
                    }
                    dr[8] = ((TextBox)gvproduct.FooterRow.FindControl("txtGvCost")).Text;
                    dr[9] = ((TextBox)gvproduct.FooterRow.FindControl("txtTaskDate")).Text;
                    dr[10] = ((TextBox)gvproduct.FooterRow.FindControl("txtgvRemark")).Text;
                    dt.Rows.Add(dr);     //dr[5] = ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).SelectedItem.Text;


                }
                catch (Exception ex)
                {

                }
            }
            Session["dtTaskList"] = dt;
        }
        if (e.CommandName.Equals("Delete"))
        {
            if (Session["dtTaskList"] != null)
            {
                dt = (DataTable)Session["dtTaskList"];
                //if (dt.Rows.Count > 0)
                //{
                //    int i = int.Parse(e.CommandArgument.ToString());
                //    TimeSpan t1 = TimeSpan.Parse(txtNoOfHrs.Text);
                //    TimeSpan t2 = TimeSpan.Parse(dt.Rows[i-1]["TaskHour"].ToString());
                //    TimeSpan t3 = t1.Subtract(t2);
                //    string timeString = t3.ToString();
                //    string modifiedTimeString = timeString.Substring(0, 5);
                //    txtNoOfHrs.Text = modifiedTimeString;
                //}
                dt = new DataView(dt, "ScrumId<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();                
                Session["dtTaskList"] = dt;
            }
        }
        gvproduct.EditIndex = -1;
        LoadProductRecord();
    }
    public void GetProjectId(object sender, EventArgs e)
    {
        GridViewRow gv = gvproduct.FooterRow;
        if (((TextBox)gv.FindControl("txtTask")).Text != "" || ((TextBox)gv.FindControl("txtTask")).Text != null)
        {
            ((TextBox)gv.FindControl("txtTask")).Text = "";
        }


        try
        {

            DataTable dt = new DataTable();
            TextBox textBox = sender as TextBox;
            dt = objScrumMaster.GetProjectId(textBox.Text);
            if (dt.Rows.Count > 0)
            {
                Session["PrjScrumId"] = dt.Rows[0]["Project_Id"].ToString();
            }
            else
            {
                textBox.Text = "";

                // TextBox ss = sender.row.FindControl("TextBox1") as TextBox;

                DisplayMessage("Select Project in suggestion only");
            }
        }
        catch (Exception ex)
        {
            DisplayMessage("Select Project in suggestion only");
        }
        
    }
    protected void txtTask_Changed(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        TextBox Task = sender as TextBox;
        try
        {

            dt = objDa.return_DataTable("Select (Concat('AssignDate :',' ',REPLACE(CONVERT(NVARCHAR,Prj_Project_Task.Assign_Date, 106), ' ', '-'),' ','CloseDate:',' ',REPLACE(CONVERT(NVARCHAR,Prj_Project_Task.Emp_Close_Date, 106), ' ', '-')))as Date,Field5,RequiredHours,Expected_Cost from Prj_Project_Task where Subject = '" + Task.Text + "' And Status='Assigned'");
            if (dt.Rows.Count > 0)
            {
                GridViewRow gv = gvproduct.FooterRow;
                ((TextBox)gv.FindControl("txtTaskPercentage")).Text = dt.Rows[0]["Field5"].ToString();
                ((TextBox)gv.FindControl("txtTaskDate")).Text = dt.Rows[0]["Date"].ToString();
                ((TextBox)gv.FindControl("txtGvCost")).Text = dt.Rows[0]["Expected_Cost"].ToString();
                if (dt.Rows[0]["RequiredHours"].ToString() == "")
                {
                    ((TextBox)gv.FindControl("txtTaskHours")).Text = "00:00";
                }
                else
                {
                    ((TextBox)gv.FindControl("txtTaskHours")).Text = dt.Rows[0]["RequiredHours"].ToString();

                }
               
            }
        }
        catch (Exception ex)
        {

        }

    }
    //public void GetTaskId(object sender, EventArgs e)
    //{
    //    DataTable dt = new DataTable();
    //    TextBox Task = sender as TextBox;
    //    try
    //    {
    //        string TaskId = Task.Text.Split('/')[1].ToString();
    //        dt = objScrumMaster.GetTaskId(TaskId);
    //        if (dt.Rows.Count == 0)
    //        {
    //            Task.Text = "";
    //            DisplayMessage("Select Task in suggestion only");
    //        }
    //    }
    //    catch
    //    {
    //        Task.Text = "";
    //        DisplayMessage("Select Task in suggestion only");

    //    }

    //}

    public void GetEmployeeId(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
            dt = new DataView(dt1, "Emp_Code=" + txtAssignTo.Text.Split('/')[1].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {

            }
            else
            {
                DisplayMessage("Select Assign To  in suggestion only");
                txtAssignTo.Text = "";
            }


        }
        catch (Exception ex)
        {
            DisplayMessage("Select Assign To  in suggestion only");
            txtAssignTo.Text = "";
        }

    }
    public void GetParentScrumId(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            Prj_ScrumMaster objScrumMaster = new Prj_ScrumMaster(HttpContext.Current.Session["DBConnection"].ToString());
            dt = objScrumMaster.GetScrumRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dt = new DataView(dt, "ScrumId=" + txtPerScrum.Text.Split('/')[1].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Perent Scrum in suggestion only");
                txtPerScrum.Text = "";
            }

        }
        catch (Exception ex)
        {
            DisplayMessage("Select Perent Scrum in suggestion only");
            txtPerScrum.Text = "";

        }

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "";
            }
        }
        return txt;

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetParentScrumList(string prefixText, int count, string contextKey)
    {
        Prj_ScrumMaster objScrumMaster = new Prj_ScrumMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objScrumMaster.GetScrumRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        dt = new DataView(dt, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = new DataView(dt1, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                txt[i] = dt.Rows[i]["ScrumName"].ToString() + "/" + dt.Rows[i]["ScrumId"].ToString() + "";
            }
        }

        return txt;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetTaskList(string prefixText, int count, string contextKey)
    {
        Prj_ScrumMaster ObprjMaster = new Prj_ScrumMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            if (HttpContext.Current.Session["PrjScrumId"].ToString() != "")
            {                
                DataTable dt1 = ObprjMaster.GetEmployeeTaskList(HttpContext.Current.Session["PrjScrumId"].ToString());
                dt1 = new DataView(dt1, "Status='Assigned'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    DataTable dt = new DataView(dt1, "(Subject like '" + prefixText + "%')", "Subject Asc", DataViewRowState.CurrentRows).ToTable();
                    string[] txt = new string[dt.Rows.Count];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            //txt[i] = dt.Rows[i]["Subject"].ToString() + "/" + dt.Rows[i]["Task_Id"].ToString() + "";
                            txt[i] = dt.Rows[i]["Subject"].ToString();
                        }
                    }
                    return txt;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        catch (Exception Ex)
        {

            return null;
        }
    }

    protected void voucherPrint(object sender,EventArgs e)
    {
        string strCmd = string.Format("window.open('../utility/reportViewer.aspx?reportId=223&ScrumId=" + 1 + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }

    [WebMethod(), ScriptMethod()]
    public static string[] GetCompletionListProject(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["DT_ddlProjectName_Followup"] == null)
        {
            HttpContext.Current.Session["DT_ddlProjectName_Followup"] = new Prj_ProjectTeam(HttpContext.Current.Session["DBConnection"].ToString()).GetAllProjectNamePreText(HttpContext.Current.Session["EmpId"].ToString(), "");
        }

        DataTable dtCon = HttpContext.Current.Session["DT_ddlProjectName_Followup"] as DataTable;
        using (dtCon = new DataView(dtCon, "Project_Name like '%" + prefixText + "%'", "Project_Name asc", DataViewRowState.CurrentRows).ToTable())
        {

            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Project_Name"].ToString();
                }
            }
            return filterlist;
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
    public void LoadProductRecord()
    {

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();        
        if (Session["dtTaskList"] != null)
        {
            dt = new DataTable();
            dt = (DataTable)Session["dtTaskList"];
            if (dt.Rows.Count > 0)
            {
                dt1 = dt;
                dt1 = new DataView(dt, "TaskStatus=3", "", DataViewRowState.CurrentRows).ToTable();

                int a = dt1.Rows.Count;
                int b = dt.Rows.Count;
                txtPercentage.Text = Convert.ToString(a * 100 / b);
                // Assuming you have already populated the DataTable with the relevant data

                int totalHours = 0;
                int totalMinutes = 0;
                int hoursResult, minutesResult;

                foreach (DataRow row in dt.Rows)
                {
                    string hourString = row["TaskHour"].ToString();
                    string[] parts = hourString.Split(':');

                    if (parts.Length == 2 && int.TryParse(parts[0], out hoursResult) && int.TryParse(parts[1], out minutesResult))
                    {
                        totalHours += hoursResult;
                        totalMinutes += minutesResult;

                        // Adjust totalMinutes if it exceeds 59
                        totalHours += totalMinutes / 60;
                        totalMinutes %= 60;
                    }
                    else
                    {
                        // Handle the parsing error for invalid hourString
                       // Console.WriteLine($"Invalid hour format: {hourString}");
                    }
                }

                txtNoOfHrs.Text = totalHours.ToString()+':'+totalMinutes.ToString();


                //TimeSpan t1 = TimeSpan.Zero; // Initialize t1 to zero

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    TimeSpan t2 = TimeSpan.Parse(dt.Rows[i]["TaskHour"].ToString());
                //    t1 = t1.Add(t2); // Add the current TimeSpan t2 to t1
                //}

                //string timeString = t1.TotalHours.ToString();
                ////string modifiedTimeString = timeString.Substring(0, 5);
                // txtNoOfHrs.Text = timeString;
                ////string timeString = "48";
                ////string modifiedTimeString = timeString.PadRight(5, '0');
                ////modifiedTimeString = modifiedTimeString.Insert(2, ":");
                //string modifiedTimeString = timeString.PadLeft(2, '0');
                //modifiedTimeString = modifiedTimeString.PadRight(5, '0');
                //modifiedTimeString = modifiedTimeString.Insert(2, ":");
                //txtNoOfHrs.Text = modifiedTimeString;

                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvproduct, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Trans_Id", typeof(int));
                contacts.Columns.Add("TaskHour", typeof(string));
                contacts.Columns.Add("ScrumId", typeof(int));
                contacts.Columns.Add("Task", typeof(string));
                contacts.Columns.Add("Project", typeof(int));
                contacts.Columns.Add("TaskStatus", typeof(string));
                contacts.Columns.Add("StatusId", typeof(string));
                contacts.Columns.Add("TaskPercentage", typeof(string));
                contacts.Columns.Add("Cost", typeof(string));
                contacts.Columns.Add("Date", typeof(string));
                contacts.Columns.Add("Remark", typeof(string));
                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvproduct, contacts, "", "");
                int TotalColumns = gvproduct.Rows[0].Cells.Count;
                gvproduct.Rows[0].Cells.Clear();
                gvproduct.Rows[0].Cells.Add(new TableCell());
                gvproduct.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvproduct.Rows[0].Visible = false;
            }
        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Trans_Id", typeof(int));
            contacts.Columns.Add("TaskHour", typeof(string));
            contacts.Columns.Add("ScrumId", typeof(int));
            contacts.Columns.Add("Task", typeof(string));
            contacts.Columns.Add("Project", typeof(int));
            contacts.Columns.Add("TaskStatus", typeof(string));
            contacts.Columns.Add("StatusId", typeof(string));
            contacts.Columns.Add("TaskPercentage", typeof(string));
            contacts.Columns.Add("Cost", typeof(string));
            contacts.Columns.Add("Date", typeof(string));
            contacts.Columns.Add("Remark", typeof(string));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvproduct, contacts, "", "");
            int TotalColumns = gvproduct.Rows[0].Cells.Count;
            gvproduct.Rows[0].Cells.Clear();
            gvproduct.Rows[0].Cells.Add(new TableCell());
            gvproduct.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvproduct.Rows[0].Visible = false;
        }
    }
}