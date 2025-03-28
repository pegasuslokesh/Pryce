using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SystemSetUp_NotificationType : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    NotificationMaster objNotification = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objNotification = new NotificationMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SystemSetup/NotificationType.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;
            FillGrid();
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }

    protected void txtNotificationType_TextChanged(object sender, EventArgs e)
    {
        int counter = 0;
        if (txtNotificationType.Text.Trim() != "")
        {
            if (editid.Value == "")
            {
                DataTable dt = objNotification.GetNotificationMasterByType("0", StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString(), txtNotificationType.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Notification Type is Already Exists");
                    txtNotificationType.Text = "";
                    txtNotificationType.Focus();
                }
                DataTable dt1 = objNotification.GetNotificationMasterByStatus(StrCompId, StrBrandId, strLocationId, "False", DateTime.Now.ToString());
                dt1 = new DataView(dt1, "Type='" + txtNotificationType.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Notification Type Already Exists in Bin Section");
                    txtNotificationType.Text = "";
                    txtNotificationType.Focus();
                }
            }
            else
            {
                DataTable dtTemp = objNotification.GetNotificationMasterById(editid.Value, StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString());
                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["Type"].ToString() != txtNotificationType.Text)
                    {
                        DataTable dt = objNotification.GetNotificationMasterByType("0", StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString(), txtNotificationType.Text.Trim());
                        if (dt.Rows.Count > 0)
                        {
                            counter = 1;
                            DisplayMessage("Notification Type is Already Exists");
                            txtNotificationType.Text = "";
                            txtNotificationType.Focus();
                        }
                        DataTable dt1 = objNotification.GetNotificationMasterByStatus(StrCompId, StrBrandId, strLocationId, "False", DateTime.Now.ToString());
                        dt1 = new DataView(dt1, "Type='" + txtNotificationType.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                            counter = 1;
                            DisplayMessage("Notification Type Already Exists in Bin Section");
                            txtNotificationType.Text = "";
                            txtNotificationType.Focus();
                        }
                    }
                }
            }
        }

        if (counter == 0)
            txtTitle.Focus();
    }

    protected void txtTitle_TextChanged(object sender, EventArgs e)
    {
        int counter = 0;
        if (txtTitle.Text.Trim() != "")
        {
            if (editid.Value == "")
            {
                DataTable dt = objNotification.GetNotificationMasterByTitle("0", StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString(), txtTitle.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Notification Title is Already Exists");
                    txtTitle.Text = "";
                    txtTitle.Focus();
                }
                DataTable dt1 = objNotification.GetNotificationMasterByStatus(StrCompId, StrBrandId, strLocationId, "False", DateTime.Now.ToString());
                dt1 = new DataView(dt1, "Title='" + txtTitle.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Notification Title Already Exists in Bin Section");
                    txtTitle.Text = "";
                    txtTitle.Focus();
                }
            }
            else
            {
                DataTable dtTemp = objNotification.GetNotificationMasterById(editid.Value, StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString());
                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["Title"].ToString() != txtTitle.Text)
                    {
                        DataTable dt = objNotification.GetNotificationMasterByTitle("0", StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString(), txtTitle.Text.Trim());
                        if (dt.Rows.Count > 0)
                        {
                            counter = 1;
                            DisplayMessage("Notification Title is Already Exists");
                            txtTitle.Text = "";
                            txtTitle.Focus();
                        }
                        DataTable dt1 = objNotification.GetNotificationMasterByStatus(StrCompId, StrBrandId, strLocationId, "False", DateTime.Now.ToString());
                        dt1 = new DataView(dt1, "Title='" + txtTitle.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                            counter = 1;
                            DisplayMessage("Notification Title Already Exists in Bin Section");
                            txtTitle.Text = "";
                            txtTitle.Focus();
                        }
                    }
                }
            }
        }

        if (counter == 0)
            txtMessageLength.Focus();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetNotificationList(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        NotificationMaster objNotificationMaster = new NotificationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objNotificationMaster.GetNotificationMaster_List(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "True", DateTime.Now.ToString()), "Type like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Type"].ToString();
        }
        return txt;
    }

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetTitleList(string prefixText, int count, string contextKey)
    //{
    //    if (HttpContext.Current.Session["UserId"] == null)
    //    {
    //        HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
    //    }
    //    NotificationMaster objNotificationMaster = new NotificationMaster();
    //    DataTable dt = new DataView(objNotificationMaster.GetNotificationMaster_List(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "True", DateTime.Now.ToString()), "Title like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
    //    string[] txt = new string[dt.Rows.Count];

    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        txt[i] = dt.Rows[i]["Title"].ToString();
    //    }
    //    return txt;
    //}

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (txtNotificationType.Text.Trim() == "")
        {
            DisplayMessage("Enter Notification Type");
            txtNotificationType.Focus();
            return;
        }
        if (txtTitle.Text.Trim() == "")
        {
            DisplayMessage("Enter Notification Title");
            txtTitle.Focus();
            return;
        }
        if (txtMessageLength.Text.Trim() == "")
        {
            DisplayMessage("Enter Message Length");
            txtMessageLength.Focus();
            return;
        }
        if (editid.Value == "")
        {
            b = objNotification.InsertNotificationMaster("0", StrCompId, StrBrandId, strLocationId, txtNotificationType.Text.Trim(), txtTitle.Text.Trim(), txtTemplate.Text.Trim(), "", "", "", "", "", txtMessageLength.Text.Trim(), Session["UserId"].ToString(), Session["UserId"].ToString(), "True", DateTime.Now.ToString());
            if (b != 0)
            {
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
            b = objNotification.UpdateNotificationMaster(editid.Value, StrCompId, StrBrandId, strLocationId, txtNotificationType.Text.Trim(), txtTitle.Text.Trim(), txtTemplate.Text.Trim(), "", "", "", "", "", txtMessageLength.Text.Trim(), Session["UserId"].ToString(), Session["UserId"].ToString(), "True", DateTime.Now.ToString());
            if (b != 0)
            {

                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        Reset();
    }

    public void Reset()
    {
        Session["CHECKED_ITEMS"] = null;
        txtNotificationType.Text = "";
        txtTitle.Text = "";
        txtMessageLength.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtNotificationType.Focus();
    }
    public void FillGrid()
    {
        DataTable dt = objNotification.GetNotificationMaster(StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString());
        objPageCmn.FillData((object)gvNotificationMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Notification_T"] = dt;
        Session["Notification"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objNotification.GetNotificationMaster(StrCompId, StrBrandId, strLocationId, "False", DateTime.Now.ToString());
        objPageCmn.FillData((object)gvNotificationMasterBin, dt, "", "");
        Session["dtbinFilter"] = dt;
        Session["dtbinCountry"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            // imgBtnRestore.Visible = false;
            // ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }

    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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
            DataTable dtCust = (DataTable)Session["Notification"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Notification_T"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvNotificationMaster, view.ToTable(), "", "");

            //AllPageCode();
        }
        txtValue.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
    }

    protected void gvNotificationMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNotificationMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Notification_T"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvNotificationMaster, dt, "", "");
        //AllPageCode();
        gvNotificationMaster.HeaderRow.Focus();
    }

    protected void gvNotificationMaster_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Notification_T"];
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
        Session["dtFilter_Notification_T"] = dt;
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvNotificationMaster, dt, "", "");
        //AllPageCode();
        gvNotificationMaster.HeaderRow.Focus();
    }




    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvNotificationMasterBin.Rows)
        {
            index = (int)gvNotificationMasterBin.DataKeys[gvrow.RowIndex].Value;
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
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvNotificationMasterBin.Rows.Count > 0)
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
                            b = objNotification.DeleteNotificationMaster(userdetail[j].ToString(), StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString(), Session["UserId"].ToString());
                        }
                    }
                }

                if (b != 0)
                {
                    FillGrid();
                    FillGridBin();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CHECKED_ITEMS"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in gvNotificationMasterBin.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
                        if (chk.Checked)
                        {
                            fleg = 1;
                        }
                        else
                        {
                            fleg = 0;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
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
                gvNotificationMasterBin.Focus();
                return;
            }
        }
    }

    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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

                if (!userdetails.Contains(dr["Trans_id"]))
                {
                    userdetails.Add(dr["Trans_id"]);
                }
            }
            foreach (GridViewRow GR in gvNotificationMasterBin.Rows)
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
            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)gvNotificationMasterBin, dtUnit1, "", "");

            ViewState["Select"] = null;
        }
    }

    protected void gvNotificationMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvNotificationMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        objPageCmn.FillData((object)gvNotificationMasterBin, (DataTable)Session["dtbinFilter"], "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }

    private void PopulateCheckedValuesemplog()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvNotificationMasterBin.Rows)
            {
                int index = (int)gvNotificationMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvNotificationMasterBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtbinFilter"];
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
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvNotificationMasterBin, dt, "", "");
        //AllPageCode();
        gvNotificationMasterBin.HeaderRow.Focus();
    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvNotificationMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvNotificationMasterBin.Rows)
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

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        editid.Value = e.CommandArgument.ToString();

        DataTable dt = objNotification.GetNotificationMasterById(editid.Value, StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString());
        if (dt.Rows.Count > 0)
        {
            txtNotificationType.Text = dt.Rows[0]["Type"].ToString();
            txtTitle.Text = dt.Rows[0]["Title"].ToString();
            txtMessageLength.Text = dt.Rows[0]["message_length"].ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            txtNotificationType.Focus();
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
                ArebicMessage = EnglishMessage;
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }

    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        editid.Value = e.CommandArgument.ToString();
        int b = 0;

        b = objNotification.DeleteNotificationMaster(editid.Value, StrCompId, StrBrandId, strLocationId, "False", DateTime.Now.ToString(), Session["UserId"].ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            //FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
        try
        {
            gvNotificationMaster.Rows[gvRow.RowIndex].Cells[1].Focus();
        }
        catch
        {
        }
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
            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)gvNotificationMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                // imgBtnRestore.Visible = false;
                // ImgbtnSelectAll.Visible = false;
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
        //FillGrid();
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
}