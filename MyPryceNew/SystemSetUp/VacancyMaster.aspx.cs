using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class SystemSetUp_VacancyMaster : BasePage
{
    string strCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocId = string.Empty;
    Common cmn = null;
    Set_Vacancies_Master objVacancy = null;
    SystemParameter objSys = null;
    IT_ObjectEntry objObjectEntry = null;
    EmployeeMaster objEmp = null;
    DesignationMaster objDesg = null;
    Pay_FollowUp_Master objFollowup = new Pay_FollowUp_Master();
    PageControlCommon objPageCmn = null;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        strCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocId = Session["LocId"].ToString();

        cmn = new Common(Session["DBConnection"].ToString());
        objVacancy = new Set_Vacancies_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objDesg = new DesignationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SystemSetup/VacancyMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            txtValue.Focus();
            FillGridBin();
            FillGrid();
            btnList_Click(null, null);

        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    public void FillGrid()
    {
        DataTable dt = objVacancy.GetAllTrueRecord();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvVacancyMaster, dt, "", "");
      
        Session["dtFilter_Vehicle_Mstr"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objVacancy.GetAllFalseRecord();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvVacancyMasterBin, dt, "", "");
        Session["dtbinFilter"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        
    }
    protected void gvVacancyMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVacancyMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Vehicle_Mstr"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvVacancyMaster, dt, "", "");
       
    }
    protected void gvVacancyMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Vehicle_Mstr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Vehicle_Mstr"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvVacancyMaster, dt, "", "");
      
    }
    protected void gvVacancyMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvVacancyMasterBin.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvVacancyMasterBin, dt, "", "");
          
        }
        string temp = string.Empty;

        for (int i = 0; i < gvVacancyMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvVacancyMasterBin.Rows[i].FindControl("lblFileId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvVacancyMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

    }
    protected void gvVacancyMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objVacancy.GetAllFalseRecord();

        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvVacancyMasterBin, dt, "", "");
        
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)gvVacancyMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvVacancyMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvVacancyMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvVacancyMasterBin.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvVacancyMasterBin.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvVacancyMasterBin.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvVacancyMasterBin.Rows[index].FindControl("lblFileId");
        if (((CheckBox)gvVacancyMasterBin.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
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
            lblSelectedRecord.Text = temp;
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = objVacancy.GetRecord_By_VacancyId(e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            txtVacancyName.Text = dt.Rows[0]["Vacancy"].ToString();
            txtVacancyL.Text = dt.Rows[0]["Vacancy_L"].ToString();
            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objVacancy.DeleteRecord(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
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
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public void Reset()
    {
        txtVacancyName.Text = "";
        txtVacancyL.Text = "";

        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        
        txtValue.Focus();
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlList.Visible = true;
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtVacancyName.Focus();
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlList.Visible = false;
        //PnlNewEdit.Visible = true;
        //PnlBin.Visible = false;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        txtbinValue.Focus();
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = true;
        //PnlList.Visible = false;
        FillGridBin();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 1;
        txtValue.Text = "";
        txtValue.Focus();
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
            DataTable dtCust = (DataTable)Session["dtFilter_Vehicle_Mstr"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Vehicle_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvVacancyMaster, view.ToTable(), "", "");

                
        }
        txtValue.Focus();
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
            DataTable dtCust = (DataTable)Session["dtbinFilter"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvVacancyMasterBin, view.ToTable(), "", "");

            
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 1;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvVacancyMasterBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
            }
        }
        if (userdetail.Count > 0)
        {
            for (int j = 0; j < userdetail.Count; j++)
            {
                if (userdetail[j].ToString() != "")
                {
                    b = objVacancy.DeleteRecord(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
            foreach (GridViewRow Gvr in gvVacancyMasterBin.Rows)
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
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvVacancyMasterBin.Rows)
        {
            index = (int)gvVacancyMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvVacancyMasterBin.Rows)
            {
                int index = (int)gvVacancyMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {

        DataTable dtUnit = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Vacancy_Id"]))
                {
                    lblSelectedRecord.Text += dr["Vacancy_Id"] + ",";
                }
            }
            for (int i = 0; i < gvVacancyMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvVacancyMasterBin.Rows[i].FindControl("lblFileId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvVacancyMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvVacancyMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtVacancyName.Text.Trim() == "")
        {
            DisplayMessage("Enter Vacancy Name");
            txtVacancyName.Focus();
            return;
        }

        int b = 0;

        if (editid.Value == "")
        {

            b = objVacancy.InsertRecord(txtVacancyName.Text, txtVacancyL.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                DisplayMessage("Record Saved","green");
                FillGrid();
                Reset();
                btnList_Click(null, null);
                Lbl_Tab_New.Text = Resources.Attendance.New;

            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {



            b = objVacancy.UpdateRecord(editid.Value, txtVacancyName.Text, txtVacancyL.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
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
        
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        Reset();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void txtVacancyName_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (txtVacancyName.Text != "")
        {
            dt = objVacancy.GetAllTrueRecord();
            dt = new DataView(dt, "Vacancy='" + txtVacancyName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Vacancy with Same Name is Already Exists");
                txtVacancyName.Text = "";
                txtVacancyName.Focus();
                return;
            }
            dt = objVacancy.GetAllFalseRecord();
            dt = new DataView(dt, "Vacancy='" + txtVacancyName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Vacancy with Same Name is Already Exists in Bin Section");
                txtVacancyName.Text = "";
                txtVacancyName.Focus();
                return;
            }

        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVacancyName(string prefixText, int count, string contextKey)
    {
        Set_Vacancies_Master objVacancy = new Set_Vacancies_Master(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = objVacancy.GetAllTrueRecord();


        dt = new DataView(dt, "Vacancy like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Vacancy"].ToString();
        }
        return txt;
    }

}
