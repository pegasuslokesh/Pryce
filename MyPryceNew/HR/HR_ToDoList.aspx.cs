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
using PegasusDataAccess;

public partial class HR_HR_ToDoList : System.Web.UI.Page
{
    Common cmn = null;
    SystemParameter objSys = null;
    IT_ObjectEntry objObjectEntry = null;
    HR_FollowUp objFollowUp = null;
    EmployeeMaster objEmp = null;
    Attendance objAtt = null;
    HR_Followup_Detail objFollowDetail = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objFollowUp = new HR_FollowUp(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objAtt = new Attendance(Session["DBConnection"].ToString());
        objFollowDetail = new HR_Followup_Detail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/HR_ToDoList.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            tr_tlHeaderFeedback.Visible = setTlPermission();
            tr_tlHeaderStatus.Visible = setTlPermission();
            tr_tlDetailFeedback.Visible = setTlPermission();

            FillteamLeader();
            Session["CHECKED_ITEMS"] = null;
            //txtFromDate.Text = System.DateTime.Now.ToString(objSys.SetDateFormat());
            //txtToDate.Text = System.DateTime.Now.ToString(objSys.SetDateFormat());
            //string EmpId = Session["EmpId"].ToString();

            txtFromDate.Text = DateTime.Now.AddDays(-1).ToString(objSys.SetDateFormat());
            txtToDate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
            FillGrid("", "", "");
            FillGridBin();
            btnList_Click(null, null);
            BindTreeView();
            Calender.Format = objSys.SetDateFormat();
            txtTaskDate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
            Session["DtTaskDetail"] = null;
            string allEmpdetail = GetTlList();
            txtFromDate_CalendarExtender1.Format = objSys.SetDateFormat();
            txtToDate_CalendarExtender2.Format = objSys.SetDateFormat();
            CalendarExtender_txtFromdateReport.Format = objSys.SetDateFormat();
            CalendarExtender_txttodatereport.Format = objSys.SetDateFormat();
        }


    }

    protected void btnGridView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewToDoList.Visible == true)
        {
            TreeViewToDoList.Visible = false;
            gvToDoList.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvToDoList.Visible = false;
            TreeViewToDoList.Visible = true;
            BindTreeView();
            FillGrid("", "", "");
        }
        btnGridView.Focus();
    }
    protected void btnTreeView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewToDoList.Visible == true)
        {
            TreeViewToDoList.Visible = false;
            gvToDoList.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvToDoList.Visible = false;
            TreeViewToDoList.Visible = true;

        }
        BindTreeView();
        btnTreeView.Focus();
    }

    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {
        TreeViewToDoList.Nodes.Clear();
        DataTable dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());



        dt = new DataView(dt, "Emp_Id=" + Session["EmpId"].ToString() + "", "Emp_Name asc", DataViewRowState.OriginalRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["Emp_Name"].ToString();
            tn.Value = dt.Rows[i]["Emp_Id"].ToString();
            TreeViewToDoList.Nodes.Add(tn);
            FillChild((dt.Rows[i]["Emp_Id"].ToString()), tn);
            i++;
        }
        TreeViewToDoList.DataBind();
    }

    private void FillChild(string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Field5='" + index.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Emp_Name"].ToString();
            tn1.Value = dt.Rows[i]["Emp_Id"].ToString();
            tn.ChildNodes.Add(tn1);
            FillChild((dt.Rows[i]["Emp_Id"].ToString()), tn1);
            i++;
        }
        TreeViewToDoList.DataBind();
    }

    public string GetTlList()
    {
        DataTable dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());


        //dt = new DataView(dt, "Emp_id=" + Session["EmpId"].ToString() + " or Field5 =" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        dt = new DataView(dt, "Emp_id='" + Session["EmpID"].ToString() + "' or Field5='" + Session["EmpID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            if (Session["To_Do_List_EmpList"] == null)
            {
                Session["To_Do_List_EmpList"] = dt.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                Session["To_Do_List_EmpList"] = Session["To_Do_List_EmpList"].ToString() + "," + dt.Rows[i]["Emp_Id"].ToString();
            }

            FillChild(dt.Rows[i]["Emp_Id"].ToString());

            i++;

        }


        return Session["To_Do_List_EmpList"].ToString();

    }


    private void FillChild(string index)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Field5='" + index + "'", "", DataViewRowState.CurrentRows).ToTable();



        int i = 0;
        while (i < dt.Rows.Count)
        {
            if (Session["To_Do_List_EmpList"] == null)
            {
                Session["To_Do_List_EmpList"] = dt.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                Session["To_Do_List_EmpList"] = Session["To_Do_List_EmpList"].ToString() + "," + dt.Rows[i]["Emp_Id"].ToString();
            }

            FillChild(dt.Rows[i]["Emp_Id"].ToString());

            i++;
        }

    }


    protected void ddlEmployeeList_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        tr_tlHeaderFeedback.Visible = false;
        tr_tlHeaderStatus.Visible = false;
        tr_tlDetailFeedback.Visible = false;

        if (ddlEmployeeList.SelectedIndex > 0)
        {

            string strTeamLeaderId = string.Empty;


            DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());

            if (dtEmp.Rows.Count > 0)
            {
                strTeamLeaderId = dtEmp.Rows[0]["Field5"].ToString();
            }

            if (strTeamLeaderId != "0")
            {


                DataTable dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());


                dt = new DataView(dt, "Emp_Id in (" + GetTlList() + ")", "", DataViewRowState.CurrentRows).ToTable();


                dt = new DataView(dt, "Emp_Id<>'" + HttpContext.Current.Session["EmpId"].ToString() + "'  and Emp_Id=" + ddlEmployeeList.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();


                if (dt.Rows.Count > 0)
                {
                    tr_tlHeaderFeedback.Visible = true;
                    tr_tlHeaderStatus.Visible = true;
                    tr_tlDetailFeedback.Visible = true;
                }
            }
            else
            {
                tr_tlHeaderFeedback.Visible = true;
                tr_tlHeaderStatus.Visible = true;
                tr_tlDetailFeedback.Visible = true;
            }

            //string strTeamLeaderId = string.Empty;


            //DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());

            //if (dtEmp.Rows.Count > 0)
            //{
            //    strTeamLeaderId = dtEmp.Rows[0]["Field5"].ToString();
            //}

            //if (strTeamLeaderId == "0")
            //{

            //    dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'  and Field5='" + Session["EmpId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            //    if (dt.Rows.Count > 0)
            //    {
            //        tr_tlHeaderFeedback.Visible = true;
            //        tr_tlHeaderStatus.Visible = true;
            //        tr_tlDetailFeedback.Visible = true;
            //    }

            //}
        }

    }

    public bool setTlPermission()
    {
        bool result = false;

        DataTable dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());
        if (Session["EmpId"].ToString() != "0")
        {

            //string strTeamLeaderId = string.Empty;


            //DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());

            //if (dtEmp.Rows.Count > 0)
            //{
            //    strTeamLeaderId = dtEmp.Rows[0]["Field5"].ToString();
            //}

            //if (strTeamLeaderId == "0")
            //{

            dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'  and Field5='" + Session["EmpId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                result = true;
            }
            //}
        }
        else
        {
            result = true;
        }

        return result;

    }

    public void FillteamLeader()
    {
        ddlEmployeeFilter.Items.Clear();
        ddlEmployeeList.Items.Clear();
        ddlReportEmployee.Items.Clear();

        DataTable dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (Session["EmpId"].ToString() != "0")
        {


            dt = new DataView(dt, "Emp_Id in (" + GetTlList() + ")", "", DataViewRowState.CurrentRows).ToTable();


        }


        objPageCmn.FillData((object)ddlEmployeeList, dt, "Emp_Name", "Emp_Id");

        ddlEmployeeFilter.DataSource = dt;
        ddlEmployeeFilter.DataTextField = "Emp_Name";
        ddlEmployeeFilter.DataValueField = "Emp_Id";
        ddlEmployeeFilter.DataBind();
        ddlEmployeeFilter.Items.Insert(0, "--Select--");

        ddlReportEmployee.DataSource = dt;
        ddlReportEmployee.DataTextField = "Emp_Name";
        ddlReportEmployee.DataValueField = "Emp_Id";
        ddlReportEmployee.DataBind();
        ddlReportEmployee.Items.Insert(0, "--Select--");



        if (Session["EmpId"].ToString() != "0")
        {

            ddlEmployeeList.SelectedValue = Session["EmpId"].ToString();


            ddlEmployeeList_OnSelectedIndexChanged(null, null);

        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bEdit;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.Visible = false;

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //txtTitle.Focus();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.Visible = false;
        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;

    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.Visible = false;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
    }


    protected void btnMenuReport_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlReport.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlList.Visible = false;

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddlEmployeeFilter.SelectedIndex = 0;
        ddlFilterStatus.SelectedIndex = 0;
        FillGrid("", "", "");
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string EmpId = ddlEmployeeFilter.SelectedValue;

        FillGrid(txtFromDate.Text, txtToDate.Text, EmpId);

    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtbinCountry"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvToDoListBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                //imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string EmpId = Session["EmpId"].ToString();
        FillGrid("", "", "");
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvToDoListBin.Rows)
        {
            index = (int)gvToDoListBin.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValuesemplog()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvToDoListBin.Rows)
            {
                int index = (int)gvToDoListBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvToDoListBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvToDoListBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvToDoListBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void gvToDoListBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        // dt = objCountry.GetCountryMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvToDoListBin, dt, "", "");
        //AllPageCode();
        gvToDoListBin.HeaderRow.Focus();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (ddlEmployeeList.SelectedIndex == 0)
        {
            DisplayMessage("Select Employee Name");
            ddlEmployeeList.Focus();
            return;
        }

        if (txtTaskDate.Text == "")
        {
            DisplayMessage("Enter Task Date");
            txtTaskDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtTaskDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Task date");
                txtTaskDate.Focus();
                return;
            }
        }



        if (txtTaskDesc.Text == "")
        {
            DisplayMessage("Enter Task Description");
            txtTaskDesc.Focus();
            return;
        }




        string strTeamLeaderId = string.Empty;


        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), ddlEmployeeList.SelectedValue);

        if (dtEmp.Rows.Count > 0)
        {
            strTeamLeaderId = dtEmp.Rows[0]["Field5"].ToString();
        }




        int b = 0;

        if (editid.Value == "")
        {
            b = objFollowUp.CRUDRecord("0", txtTaskDate.Text, "Employee", ddlEmployeeList.SelectedValue, txtTaskDesc.Text, txtTLfeedback.Text, strTeamLeaderId, ddlStatus.SelectedValue, "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
            if (b != 0)
            {

                DataTable dt = (DataTable)Session["DtTaskDetail"];

                if (dt != null)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objFollowDetail.InsertRecord(b.ToString(), dt.Rows[i]["From_Time"].ToString(), dt.Rows[i]["To_Time"].ToString(), dt.Rows[i]["Task_Description"].ToString(), dt.Rows[i]["Task_Feedback"].ToString(), dt.Rows[i]["TL_Feedback"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());

                    }
                }

                DisplayMessage("Record Saved","green");
                string EmpId = Session["EmpId"].ToString();
                FillGrid("", "", "");
                Reset();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            string strFeedbackModifiedBy = string.Empty;
            //here we get feedback modified by name

            if (ViewState["Status"].ToString().Trim() != ddlStatus.SelectedValue)
            {
                strFeedbackModifiedBy = Session["EmpId"].ToString();
            }
            // b = objCountry.UpdateCountryMaster(editid.Value, txtCountryName.Text.Trim(), txtCountryNameL.Text.Trim(), txtCountryCode.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objFollowUp.CRUDRecord(editid.Value, txtTaskDate.Text, "Employee", ddlEmployeeList.SelectedValue, txtTaskDesc.Text, txtTLfeedback.Text, strTeamLeaderId, ddlStatus.SelectedValue, strFeedbackModifiedBy, "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "2");

            objFollowDetail.DeleteRecord_By_HeaderId(editid.Value);
            DataTable dt = (DataTable)Session["DtTaskDetail"];

            if (dt != null)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objFollowDetail.InsertRecord(editid.Value, dt.Rows[i]["From_Time"].ToString(), dt.Rows[i]["To_Time"].ToString(), dt.Rows[i]["Task_Description"].ToString(), dt.Rows[i]["Task_Feedback"].ToString(), dt.Rows[i]["TL_Feedback"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());

                }
            }
            btnList_Click(null, null);
            DisplayMessage("Record Updated", "green");
            Reset();
            string EmpId = Session["EmpId"].ToString();
            FillGrid("", "", "");
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
    }

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
        //AllPageCode();

    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;

        //here we checking status for validation
        //if status is pendin gor done then we can not edit 

        if (objSenderID != "lnkViewDetail")
        {
            if (((Label)gvRow.FindControl("lblStatus")).Text.Trim().ToUpper() == "PENDING" || ((Label)gvRow.FindControl("lblStatus")).Text.Trim().ToUpper() == "DONE")
            {
                DisplayMessage("you can not edit pending or done task");
                return;
            }
        }




        DataTable dt = objDa.return_DataTable("select * from HR_FollowUp where Trans_Id=" + e.CommandArgument.ToString() + "");
        editid.Value = e.CommandArgument.ToString();
        ddlEmployeeList.SelectedValue = dt.Rows[0]["ref_Id"].ToString();


        ddlEmployeeList_OnSelectedIndexChanged(null, null);

        txtTaskDate.Text = Convert.ToDateTime(dt.Rows[0]["Follow_Date"].ToString()).ToString(objSys.SetDateFormat());
        txtTaskDesc.Text = dt.Rows[0]["Title"].ToString();
        txtTLfeedback.Text = dt.Rows[0]["Description"].ToString();
        ddlStatus.SelectedValue = dt.Rows[0]["Field1"].ToString();
        ViewState["Status"] = dt.Rows[0]["Field1"].ToString();
        //here we get detail record

        DataTable dtDetail = objFollowDetail.GetRecord_by_HeaderId(e.CommandArgument.ToString());



        dtDetail = dtDetail.DefaultView.ToTable(true, "Trans_Id", "From_Time", "To_Time", "Task_Description", "Task_Feedback", "TL_Feedback");


        Session["DtTaskDetail"] = dtDetail;

        objPageCmn.FillData((object)GvTaskDetail, dtDetail, "", "");


        btnNew_Click(null, null);
        if (objSenderID != "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        ddlEmployeeList.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;


        //here we checking status for validation
        //if status is pendin gor done then we can not edit 


        if (((Label)gvRow.FindControl("lblStatus")).Text.Trim().ToUpper() == "PENDING" || ((Label)gvRow.FindControl("lblStatus")).Text.Trim().ToUpper() == "DONE")
        {
            DisplayMessage("You can not edit pending or done task");
            return;
        }


        int b = 0;

        objDa.execute_Command("update HR_FollowUp set IsActive='False',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id=" + e.CommandArgument.ToString() + "");
        DisplayMessage("Record Deleted");
        FillGrid("", "", "");
        FillGridBin();
        Reset();

    }
    protected void gvToDoList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvToDoList.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_HR__Todo"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvToDoList, dt, "", "");
        //AllPageCode();
        gvToDoList.HeaderRow.Focus();
    }
    protected void gvToDoList_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_HR__Todo"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_HR__Todo"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvToDoList, dt, "", "");
        //AllPageCode();
        gvToDoList.HeaderRow.Focus();
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
        string EmpId = Session["EmpId"].ToString();
        FillGrid("", "", "");
        FillGridBin();
        Reset();
        btnList_Click(null, null);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void FillGrid(string FromDate, string ToDate, string EmpId)
    {


        DataTable dt = objDa.return_DataTable("Select HR_FollowUp.*,Set_EmployeeMaster.Emp_Name , (select set_employeemaster.Emp_Name from set_employeemaster where set_employeemaster.Emp_Id =(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=HR_FollowUp.CreatedBy) ) as CreatedByName, case when HR_FollowUp.Field2<>' ' then (select set_employeemaster.Emp_Name from set_employeemaster where set_employeemaster.Emp_Id =HR_FollowUp.Field2) else ' ' end as Feedback_By From HR_FollowUp Inner Join Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = HR_FollowUp.Ref_Id WHERE ISNULL(HR_FollowUp.IsActive,0)=1 ORDER BY HR_FollowUp.Trans_Id DESC");


        if (Session["EmpId"].ToString() != "0")
        {

            try
            {
                dt = new DataView(dt, "Ref_Id in (" + GetTlList() + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }

        try
        {

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                dt = new DataView(dt, "Follow_Date>='" + Convert.ToDateTime(txtFromDate.Text) + "' AND Follow_Date<='" + Convert.ToDateTime(txtToDate.Text) + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (ddlEmployeeFilter.SelectedIndex > 0)
            {

                dt = new DataView(dt, "Follow_Type='Employee' and Ref_Id=" + ddlEmployeeFilter.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (ddlFilterStatus.SelectedIndex > 0)
            {

                dt = new DataView(dt, "Field1='" + ddlFilterStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

        }
        catch
        {

        }

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvToDoList, dt, "", "");
        //AllPageCode();
        Session["dtFilter_HR__Todo"] = dt;
        Session["Country"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }

    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }
    public void FillGridBin()
    {
        DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());
        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        try
        {
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            }
        }
        catch
        {
        }

        string Validemplist = string.Empty;
        foreach (DataRow dr in dtEmp.Rows)
        {
            if ((dr["Emp_Id"].ToString()) != "")
            {
                Validemplist += (dr["Emp_Id"].ToString()).ToString() + ",";
            }
        }
        DataTable dt = objDa.return_DataTable("Select *,Set_EmployeeMaster.Emp_Name From HR_FollowUp   Inner Join Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = HR_FollowUp.Ref_Id   WHERE ISNULL(HR_FollowUp.IsActive,0)=0  ORDER BY Follow_Date DESC    ");
        try
        {
            dt = new DataView(dt, "Follow_Type='Employee'", "", DataViewRowState.CurrentRows).ToTable();
            if (Validemplist != "")
            {
                dt = new DataView(dt, "Ref_Id in(" + Validemplist.Substring(0, Validemplist.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        catch
        {

        }
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvToDoListBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinCountry"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            //imgBtnRestore.Visible = false;
            //ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvToDoListBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvToDoListBin.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                //if (!userdetails.Contains(dr["Country_Id"]))
                //{
                //    userdetails.Add(dr["Country_Id"]);
                //}
            }
            foreach (GridViewRow GR in gvToDoListBin.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvToDoListBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dt = new DataTable();
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvToDoListBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            objDa.execute_Command("update HR_FollowUp set IsActive='True',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id=" + userdetail[j].ToString() + "");

                        }
                    }
                }


                FillGrid("", "", "");
                FillGridBin();
                lblSelectedRecord.Text = "";
                ViewState["Select"] = null;
                DisplayMessage("Record Activated");
                Session["CHECKED_ITEMS"] = null;

            }
            else
            {
                DisplayMessage("Please Select Record");
                gvToDoListBin.Focus();
                return;
            }
        }
    }
    public void Reset()
    {
        ddlEmployeeList.SelectedIndex = 0;
        txtTaskDate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
        txtTaskDesc.Text = "";
        txtTLfeedback.Text = "";
        ddlStatus.SelectedIndex = 0;
        ResettaskDetail();
        GvTaskDetail.DataSource = null;
        GvTaskDetail.DataBind();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        Session["DtTaskDetail"] = null;
        editid.Value = "";
        //AllPageCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmpName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        dt = new DataView(dt, "Emp_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Name"].ToString();
        }
        return txt;
    }





    #region TaskDetail


    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Trans_Id", typeof(float));
        dt.Columns.Add("From_Time");
        dt.Columns.Add("To_Time");
        dt.Columns.Add("Task_Description");
        dt.Columns.Add("Task_Feedback");
        dt.Columns.Add("TL_Feedback");

        return dt;
    }
    protected void btnTaskSave_Click(object sender, EventArgs e)
    {
        if (txtInTime.Text == "")
        {
            DisplayMessage("Enter From Time");
            txtInTime.Focus();
            return;
        }
        if (txtOuttime.Text == "")
        {
            DisplayMessage("Enter To Time");
            txtOuttime.Focus();
            return;
        }

        if (txtTaskDetaildesc.Text == "")
        {
            DisplayMessage("Enter Task description");
            txtTaskDetaildesc.Focus();
            return;
        }

        float TransId = 0;


        DataTable dt = new DataTable();


        if (hdnTaskId.Value == "")
        {


            if (Session["DtTaskDetail"] == null)
            {
                dt = CreateProductDataTable();

                DataRow dr = dt.NewRow();


                dr[0] = 1;
                dr[1] = txtInTime.Text;
                dr[2] = txtOuttime.Text;
                dr[3] = txtTaskDetaildesc.Text;
                dr[4] = txtTaskdetailFeedback.Text;
                dr[5] = txtTlDetailFeedback.Text;

                dt.Rows.Add(dr);
            }
            else
            {

                dt = (DataTable)Session["DtTaskDetail"];

                DataRow dr = dt.NewRow();
                try
                {
                    TransId = float.Parse(new DataView(dt, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) + 1; ;
                }
                catch
                {
                    TransId = 1;
                }

                dr[0] = TransId;
                dr[1] = txtInTime.Text;
                dr[2] = txtOuttime.Text;
                dr[3] = txtTaskDetaildesc.Text;
                dr[4] = txtTaskdetailFeedback.Text;
                dr[5] = txtTlDetailFeedback.Text;

                dt.Rows.Add(dr);

            }
        }
        else
        {

            dt = (DataTable)Session["DtTaskDetail"];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Trans_Id"].ToString().Trim() == hdnTaskId.Value)
                {

                    dt.Rows[i]["From_Time"] = txtInTime.Text;
                    dt.Rows[i]["To_Time"] = txtOuttime.Text;
                    dt.Rows[i]["Task_Description"] = txtTaskDetaildesc.Text;
                    dt.Rows[i]["Task_Feedback"] = txtTaskdetailFeedback.Text;
                    dt.Rows[i]["TL_Feedback"] = txtTlDetailFeedback.Text;
                }
            }
        }

        Session["DtTaskDetail"] = dt;

        objPageCmn.FillData((object)GvTaskDetail, dt, "", "");
        ResettaskDetail();
    }


    protected void btnTaskCancel_Click(object sender, EventArgs e)
    {
        ResettaskDetail();
    }

    public void ResettaskDetail()
    {
        hdnTaskId.Value = "";
        txtInTime.Text = "";
        txtOuttime.Text = "";
        txtTaskDetaildesc.Text = "";
        txtTaskdetailFeedback.Text = "";
        txtTlDetailFeedback.Text = "";
        txtInTime.Focus();
    }

    protected void btnEdit_Command1(object sender, CommandEventArgs e)
    {

        DataTable dt = new DataView((DataTable)Session["DtTaskDetail"], "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        hdnTaskId.Value = e.CommandArgument.ToString();
        txtInTime.Text = dt.Rows[0]["From_Time"].ToString();
        txtOuttime.Text = dt.Rows[0]["To_Time"].ToString();
        txtTaskDetaildesc.Text = dt.Rows[0]["Task_Description"].ToString();
        txtTaskdetailFeedback.Text = dt.Rows[0]["Task_Feedback"].ToString();
        txtTlDetailFeedback.Text = dt.Rows[0]["TL_Feedback"].ToString();
    }

    protected void IbtnDelete_Command1(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView((DataTable)Session["DtTaskDetail"], "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        Session["DtTaskDetail"] = dt;

        objPageCmn.FillData((object)GvTaskDetail, dt, "", "");

    }


    #endregion



    #region Report
    protected void btnTransportreport_OnClick(object sender, EventArgs e)
    {
        DataTable DtFilter = new DataTable();
        HrDataset ObjHRDataset = new HrDataset();
        ObjHRDataset.EnforceConstraints = false;


        HrDatasetTableAdapters.sp_HR_FollowUp_ReportTableAdapter adp = new HrDatasetTableAdapters.sp_HR_FollowUp_ReportTableAdapter();


        adp.Fill(ObjHRDataset.sp_HR_FollowUp_Report);
        DtFilter = ObjHRDataset.sp_HR_FollowUp_Report;

        if (txtFromdateReport.Text != "" && txttodatereport.Text != "")
        {

            DtFilter = new DataView(DtFilter, "Follow_Date>='" + txtFromdateReport.Text + "' and Follow_Date<='" + txttodatereport.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

        }


        if (ddlReportEmployee.SelectedIndex > 0)
        {

            DtFilter = new DataView(DtFilter, "Employee_Id in (" + ddlReportEmployee.SelectedValue + ")", "", DataViewRowState.CurrentRows).ToTable();

        }
        else
        {
            DtFilter = new DataView(DtFilter, "Employee_Id in (" + GetTlList() + ")", "", DataViewRowState.CurrentRows).ToTable();

        }



        if (ddlreportStatus.SelectedIndex > 0)
        {

            DtFilter = new DataView(DtFilter, "TaskStatus = '" + ddlreportStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }

        Session["DtTaskdetail"] = DtFilter;



        string strCmd = string.Format("window.open('../HR_Report/ToDoListReport.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }


    #endregion



}

