using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ServiceManagement_JobPlan : System.Web.UI.Page
{
    Common cmn = null;
    SystemParameter objSys = null;
    SM_JobPlan_Header objjobPlanHeader = null;
    SM_JobPlan_Detail objjobPlanDetail = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objjobPlanHeader = new SM_JobPlan_Header(Session["DBConnection"].ToString());
        objjobPlanDetail = new SM_JobPlan_Detail(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        //AllPageCode();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ServiceManagement/JobPlan.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();
            Session["dtVisitTaskList"] = null;
            FillGrid();
           
            LoadTask();
        }
        gvJobPlan.DataSource = Session["dtFilter_Jobplan"] as DataTable;
        gvJobPlan.DataBind();
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        BtnExportExcel.Visible = clsPagePermission.bDownload;
        BtnExportPDF.Visible = clsPagePermission.bDownload;
        btnSave.Visible = clsPagePermission.bEdit;
        gvJobPlan.Columns[0].Visible = clsPagePermission.bEdit;
        gvJobPlan.Columns[1].Visible = clsPagePermission.bDelete;
    }
   

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
    }



    protected void btnbind_Click(object sender, ImageClickEventArgs e)
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
            DataTable dtCust = (DataTable)Session["Country"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Jobplan"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvJobPlan, view.ToTable(), "", "");

            //AllPageCode();
        }
        txtValue.Focus();
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;

        if (editid.Value == "")
        {
            b = objjobPlanHeader.InsertRecord(txtjobPlanId.Text.Trim(), txtjobPlanName.Text.Trim());
            if (b != 0)
            {

                //here we insert record in detail table

                if (Session["dtVisitTaskList"] != null)
                {
                    int i = 0;
                    foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                    {
                        i++;
                        objjobPlanDetail.InsertRecord("JOB", b.ToString(), dr["Work"].ToString(), dr["Minute"].ToString());
                    }
                }
                DisplayMessage("Record Saved","green");
                FillGrid();
                Reset();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            objjobPlanHeader.UpdateRecord(editid.Value, txtjobPlanId.Text.Trim(), txtjobPlanName.Text.Trim());
            //here we insert record in detail table
            //first we dete and reinsert
            //here we insert record in detail table
            objjobPlanDetail.DeleteRecord_BY_RefTypeandRefId("JOB", editid.Value);
            if (Session["dtVisitTaskList"] != null)
            {
                int i = 0;
                foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                {
                    i++;
                    objjobPlanDetail.InsertRecord("JOB", editid.Value, dr["Work"].ToString(), dr["Minute"].ToString());
                }
            }
            DisplayMessage("Record Updated", "green");
            Reset();
            FillGrid();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        editid.Value = e.CommandArgument.ToString();

        DataTable dt = objjobPlanHeader.GetRecord_By_TransId(editid.Value);
        if (dt.Rows.Count > 0)
        {
            txtjobPlanId.Text = dt.Rows[0]["JobPlanId"].ToString();
            txtjobPlanName.Text = dt.Rows[0]["JobPlanName"].ToString();
            //get record of plan list
            if (objjobPlanDetail.GetRecord_By_RefType_and_RefId("JOB", editid.Value).Rows.Count > 0)
            {
                Session["dtVisitTaskList"] = objjobPlanDetail.GetRecord_By_RefType_and_RefId("JOB", editid.Value).DefaultView.ToTable(true, "Trans_Id", "Work", "Minute");
                LoadTask();
            }
            else
            {
                Session["dtVisitTaskList"] = null;
            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            txtjobPlanId.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objjobPlanHeader.DeleteRecord(e.CommandArgument.ToString());
        objjobPlanDetail.DeleteRecord_BY_RefTypeandRefId("JOB", e.CommandArgument.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");

            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
        try
        {
            //gvJobPlan.Rows[gvRow.RowIndex].Cells[1].Focus();
        }
        catch
        {
        }
    }
    protected void gvJobPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvJobPlan.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Jobplan"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvJobPlan, dt, "", "");
        //AllPageCode();
        //gvJobPlan.HeaderRow.Focus();

    }
    protected void gvJobPlan_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Jobplan"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Jobplan"] = dt;
        gvJobPlan.DataSource = dt;
        gvJobPlan.DataBind();
        //AllPageCode();
        //gvJobPlan.HeaderRow.Focus();
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
                ArebicMessage = EnglishMessage;
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void FillGrid()
    {
        DataTable dt = objjobPlanHeader.GetAllRecord();
        //Common Function add By Lokesh on 11-05-2015
        //cmn.FillData((object)gvJobPlan, dt, "", "");
        gvJobPlan.DataSource = dt;
        gvJobPlan.DataBind();

        //AllPageCode();
        Session["dtFilter_Jobplan"] = dt;
        Session["Country"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }

    public void Reset()
    {
        txtjobPlanId.Text = "";
        txtjobPlanName.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        Session["dtVisitTaskList"] = null;
        LoadTask();

    }


    #region workList
    public void LoadTask()
    {
        DataTable dt = new DataTable();
        if (Session["dtVisitTaskList"] != null)
        {


            dt = new DataTable();
            dt = (DataTable)Session["dtVisitTaskList"];



            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvVisitTask, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Trans_Id", typeof(int));
                contacts.Columns.Add("Work", typeof(string));
                contacts.Columns.Add("Minute", typeof(string));

                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvVisitTask, contacts, "", "");
                int TotalColumns = gvVisitTask.Rows[0].Cells.Count;
                gvVisitTask.Rows[0].Cells.Clear();
                gvVisitTask.Rows[0].Cells.Add(new TableCell());
                gvVisitTask.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvVisitTask.Rows[0].Visible = false;
            }

        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Trans_Id", typeof(int));
            contacts.Columns.Add("Work", typeof(string));
            contacts.Columns.Add("Minute", typeof(string));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvVisitTask, contacts, "", "");
            int TotalColumns = gvVisitTask.Rows[0].Cells.Count;
            gvVisitTask.Rows[0].Cells.Clear();
            gvVisitTask.Rows[0].Cells.Add(new TableCell());
            gvVisitTask.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvVisitTask.Rows[0].Visible = false;
        }
    }
    protected void gvVisitTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
        DataTable dt = new DataTable();
        string EmpId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Text.Trim() == "")
            {
                DisplayMessage("Enter Description");
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Focus();
                return;
            }



            if (Session["dtVisitTaskList"] == null)
            {
                dt.Columns.Add("Trans_Id", typeof(int));
                dt.Columns.Add("Work", typeof(string));
                dt.Columns.Add("Minute", typeof(string));
                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Text.Trim();
                dr[2] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtMinutes")).Text;
                dt.Rows.Add(dr);
            }
            else
            {
                string strTransid = string.Empty;
                dt = (DataTable)Session["dtVisitTaskList"];
                if (dt.Rows.Count > 0)
                {
                    DataTable dtCopy = dt.Copy();
                    dtCopy = new DataView(dtCopy, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
                    strTransid = (float.Parse(dtCopy.Rows[0]["Trans_Id"].ToString()) + 1).ToString();
                }
                else
                {
                    strTransid = "1";
                }
                DataRow dr = dt.NewRow();
                dr[0] = strTransid;
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Text.Trim();
                dr[2] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtMinutes")).Text;
                dt.Rows.Add(dr);
            }
            Session["dtVisitTaskList"] = dt;
            gvVisitTask.EditIndex = -1;
            LoadTask();
        }
        if (e.CommandName.Equals("Delete"))
        {

            if (Session["dtVisitTaskList"] != null)
            {
                dt = (DataTable)Session["dtVisitTaskList"];
                dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtVisitTaskList"] = dt;
            }
            gvVisitTask.EditIndex = -1;
            LoadTask();
        }

        ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Focus();

    }
    protected void gvVisitTask_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvVisitTask.EditIndex = e.NewEditIndex;
        LoadTask();
    }
    protected void gvVisitTask_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvVisitTask.EditIndex = -1;
        LoadTask();
    }
    protected void gvVisitTask_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtVisitTaskList"];

        GridViewRow row = gvVisitTask.Rows[e.RowIndex];

        dt.Rows[row.DataItemIndex]["Work"] = ((TextBox)row.FindControl("txteditTask")).Text;
        dt.Rows[row.DataItemIndex]["Minute"] = ((TextBox)row.FindControl("txEdittMinutes")).Text;
        Session["dtVisitTaskList"] = dt;
        gvVisitTask.EditIndex = -1;
        LoadTask();
    }
    protected void gvVisitTask_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion

    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WritePdfToResponse();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }

    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WriteCsvToResponse();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }
}