using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_Candidate_Master : System.Web.UI.Page
{
    HR_CandidateMaster objCandidate = null;
    EmployeeMaster objEmp = null;
    Common cmn = null;
    CandidateMaster CandidateMaster = null;
    QualificationMaster QualificationMaster = null;
    IT_ObjectEntry objObjectEntry = null;
    SystemParameter objSys = null;
    DesignationMaster objDesg = null;
    Common ObjComman = null;
    CountryMaster CountryMaster = null;
    ES_EmailMasterDetail objEmailDetail = null;
    ES_EmailMaster_Header objEmailHeader = null;
    PageControlCommon objPageCmn = null;

    //--------------- Start System ---------------

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/ERPLogin.aspx");
            }


            objCandidate = new HR_CandidateMaster(Session["DBConnection"].ToString());
            objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            CandidateMaster = new CandidateMaster(Session["DBConnection"].ToString());
            QualificationMaster = new QualificationMaster(Session["DBConnection"].ToString());
            objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
            objSys = new SystemParameter(Session["DBConnection"].ToString());
            objDesg = new DesignationMaster(Session["DBConnection"].ToString());
            ObjComman = new Common(Session["DBConnection"].ToString());
            CountryMaster = new CountryMaster(Session["DBConnection"].ToString());
            objEmailDetail = new ES_EmailMasterDetail(Session["DBConnection"].ToString());
            objEmailHeader = new ES_EmailMaster_Header(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

            Txt_Dob_CalendarExtender.Format = objSys.SetDateFormat();
            Page.Title = objSys.GetSysTitle();
            //AllPageCode();            


            if (!IsPostBack)
            {
                ServerDateTime.Value = DateTime.Today.ToString();
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/Candidate_Master.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
                if (clsPagePermission.bHavePermission == false)
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                AllPageCode(clsPagePermission);
                Bind_Country();
                Bind_DDL_Qualification();
                Fill_Grid_List();
            }
        }
        catch
        {
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Save.Visible = clsPagePermission.bEdit;
        Img_Emp_List_Active.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    public void DisplayMessage(string str,string color="orange")
    {
        try
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
        catch
        {
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

    //--------------- End System ---------------

    //--------------- Start List ---------------

    protected void Btn_List_Li_Click(object sender, EventArgs e)
    {
        try
        {
            Fill_Grid_List();
        }
        catch
        {
        }
    }

    protected void Fill_Grid_List()
    {
        try
        {
            Session["CM_CHECKED_ITEMS_LIST"] = null;
            Session["CM_Dt_Filter"] = null;
            DataTable Dt_Candi_Mstr = CandidateMaster.Get_Hr_Candidate_Master("0", "", "", "", "", "", "", "0", "", "", DateTime.Now.ToString("dd-MMM-yyyy"), "", "", "True", "True", "0", "True", "True", "0", "", "0", "", "", "", "", "", "", "", "", "", "", "True", "", "", "0");
            Session["CM_Dt_Candi_Mstr_List"] = Dt_Candi_Mstr;

            DataTable Dt_Candi_Mstr_List = new DataView(Session["CM_Dt_Candi_Mstr_List"] as DataTable, "Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();
            Session["CM_Dt_Candi_Mstr_List_Active"] = Dt_Candi_Mstr_List;
            if (Dt_Candi_Mstr_List.Rows.Count > 0)
            {
                Fill_Gv_Candi_Mstr(Dt_Candi_Mstr_List);
            }
            else
            {
                Fill_Gv_Candi_Mstr(Dt_Candi_Mstr_List);
            }
        }
        catch
        {
        }
    }

    protected void Fill_Gv_Candi_Mstr(DataTable Dt_Grid)
    {
        Lbl_TotalRecords.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();

        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Candi_Master_List.DataSource = Dt_Grid;
            Gv_Candi_Master_List.DataBind();
           
        }
        else
        {
            Gv_Candi_Master_List.DataSource = null;
            Gv_Candi_Master_List.DataBind();
           
        }
    }

    private void Save_Checked_Candi_Mstr_Master()
    {
        ArrayList Candi_Mstrs = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Candi_Master_List.Rows)
        {
            index = (int)Gv_Candi_Master_List.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select")).Checked;
            if (Session["CM_CHECKED_ITEMS_LIST"] != null)
                Candi_Mstrs = (ArrayList)Session["CM_CHECKED_ITEMS_LIST"];
            if (result)
            {
                if (!Candi_Mstrs.Contains(index))
                    Candi_Mstrs.Add(index);
            }
            else
                Candi_Mstrs.Remove(index);
        }
        if (Candi_Mstrs != null && Candi_Mstrs.Count > 0)
            Session["CM_CHECKED_ITEMS_LIST"] = Candi_Mstrs;
    }

    protected void Populate_Checked_Candi_Mstr_Master()
    {
        ArrayList Candi_Mstrs = (ArrayList)Session["CM_CHECKED_ITEMS_LIST"];
        if (Candi_Mstrs != null && Candi_Mstrs.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Candi_Master_List.Rows)
            {
                int index = (int)Gv_Candi_Master_List.DataKeys[gvrow.RowIndex].Value;
                if (Candi_Mstrs.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("Chk_Gv_Select");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["CM_Dt_Filter"] = null;
            Save_Checked_Candi_Mstr_Master();
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
                DataTable Dt_Candi_Mstr_List = (DataTable)Session["CM_Dt_Candi_Mstr_List_Active"];
                DataView view = new DataView(Dt_Candi_Mstr_List, condition, "", DataViewRowState.CurrentRows);
                Session["CM_Dt_Filter"] = view.ToTable();
                Fill_Gv_Candi_Mstr(view.ToTable());
                txtValue.Focus();
            }
            Populate_Checked_Candi_Mstr_Master();
        }
        catch
        {
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["CM_CHECKED_ITEMS_LIST"] = null;
            Session["CM_Dt_Filter"] = null;
            foreach (GridViewRow GR in Gv_Candi_Master_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = false;
            }
            Fill_Gv_Candi_Mstr(Session["CM_Dt_Candi_Mstr_List_Active"] as DataTable);
            txtValue.Text = "";
            ddlOption.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_List_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["CM_Dt_Candi_Mstr_List_Active"];
        ArrayList Candi_Mstrs = new ArrayList();
        Session["CM_CHECKED_ITEMS_LIST"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CM_CHECKED_ITEMS_LIST"] != null)
                {
                    Candi_Mstrs = (ArrayList)Session["CM_CHECKED_ITEMS_LIST"];
                }
                if (!Candi_Mstrs.Contains(dr["Candidate_Id"]))
                {
                    Candi_Mstrs.Add(dr["Candidate_Id"]);
                }
            }
            foreach (GridViewRow GR in Gv_Candi_Master_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = true;
            }
            if (Candi_Mstrs.Count > 0 && Candi_Mstrs != null)
            {
                Session["CM_CHECKED_ITEMS_LIST"] = Candi_Mstrs;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["CM_Dt_Candi_Mstr_List_Active"];
            objPageCmn.FillData((object)Gv_Candi_Master_List, dt, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
            
        }
    }

    protected void Img_Emp_List_Delete_All_Click(object sender, ImageClickEventArgs e)
    {
        int b = 0;
        ArrayList Candi_Mstr = new ArrayList();
        if (Gv_Candi_Master_List.Rows.Count > 0)
        {
            Save_Checked_Candi_Mstr_Master();
            if (Session["CM_CHECKED_ITEMS_LIST"] != null)
            {
                Candi_Mstr = (ArrayList)Session["CM_CHECKED_ITEMS_LIST"];
                if (Candi_Mstr.Count > 0)
                {
                    for (int j = 0; j < Candi_Mstr.Count; j++)
                    {
                        if (Candi_Mstr[j].ToString() != "")
                        {
                            b = CandidateMaster.Set_Hr_Candidate_Master(Candi_Mstr[j].ToString(), "", "", "", "", "", "", "0", "", "", DateTime.Now.ToString("dd-MMM-yyyy"), "", "", "True", "True", "0", "True", "True", "0", "", "0", "", "", "", "", "", "", "", "", "", "", "False", "", "", "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["CM_CHECKED_ITEMS_LIST"] = null;
                    Session["CM_Dt_Filter"] = null;
                    Fill_Grid_List();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Deleted");
                    Session["CM_CHECKED_ITEMS_LIST"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Candi_Master_List.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("Chk_Gv_Select");
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
                Gv_Candi_Master_List.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Candi_Master_List.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow gr in Gv_Candi_Master_List.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select")).Checked = false;
            }
        }
    }

    protected void Btn_Edit_Command(object sender, CommandEventArgs e)
    {
        string Fresher = "";
        string IsSms = "";
        string IsEmail = "";
        Edit_ID.Value = e.CommandArgument.ToString();
        DataTable dt = objCandidate.GetRecord_By_TransId(e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            Txt_CandidateName.Text = dt.Rows[0]["Candidate_Name"].ToString();
            Txt_Candidate_Local.Text = dt.Rows[0]["Candidate_Name_L"].ToString();
            Txt_Father.Text = dt.Rows[0]["F_H_Name"].ToString();
            if (dt.Rows[0]["Gender"].ToString() != "")
            {
                DDL_Gender.SelectedValue = dt.Rows[0]["Gender"].ToString();
            }
            Txt_Dob.Text = Convert.ToDateTime(dt.Rows[0]["DOb"].ToString()).ToString(objSys.SetDateFormat());
            Txt_CorrespondenceAddress.Text = dt.Rows[0]["Correspondence_Address"].ToString();
            Txt_PermanentAddress.Text = dt.Rows[0]["Permanent_Address"].ToString();

            if (Txt_CorrespondenceAddress.Text == Txt_PermanentAddress.Text)
                Chk_Address_Check.Checked = true;
            else
                Chk_Address_Check.Checked = false;


            Txt_MobileNo_1.Text = dt.Rows[0]["Mobile_No1"].ToString();
            Txt_MobileNo_2.Text = dt.Rows[0]["Mobile_No2"].ToString();
            Txt_Email_1.Text = dt.Rows[0]["Email_Id1"].ToString();
            Txt_Email_2.Text = dt.Rows[0]["Email_Id2"].ToString();
            Txt_Key_Skill.Text= dt.Rows[0]["Skill"].ToString();
            Fresher = dt.Rows[0]["Is_Fresher"].ToString();
            if (dt.Rows[0]["Country_Id"].ToString() != "")
            {
                DDL_CountryId_1.SelectedValue = dt.Rows[0]["Country_Id"].ToString();
                DDL_CountryId_2.SelectedValue = dt.Rows[0]["Country_Id"].ToString();
            }
            if (dt.Rows[0]["Source"].ToString() != "")
            {
                DDL_Source.SelectedValue = dt.Rows[0]["Source"].ToString();
            }

            if (Convert.ToBoolean(dt.Rows[0]["Martial_Status"].ToString()))
            {
                DDL_Marrital_Status.SelectedValue = "1";
            }
            else
            {
                DDL_Marrital_Status.SelectedValue = "0";
            }

            if (DDL_Source.SelectedValue == "3")
            {
                Div_Ref_Employee.Visible = true;
                Txt_Reference_Emp.Text = dt.Rows[0]["Emp_Name"].ToString();
            }
            else if (DDL_Source.SelectedValue == "4")
            {
                Div_Ref_Third_Party.Visible = true;
                Txt_Third_Party.Text = dt.Rows[0]["Third_Party"].ToString();
            }
            else
            {

            }

            if (Fresher == "False")
            {
                Div_Fresher.Visible = true;
                DDL_IsFresher.SelectedValue = "1";
                Txt_CurrentCompany.Text = dt.Rows[0]["Current_Company"].ToString();
                Txt_TotalExperince.Text = dt.Rows[0]["Total_Experience"].ToString();
                Txt_CurrentCTC.Text = dt.Rows[0]["Currenct_CTC"].ToString();
            }
            else
            {
                Div_Fresher.Visible = false;
                DDL_IsFresher.SelectedValue = "0";
                Txt_CurrentCompany.Text = "";
                Txt_TotalExperince.Text = "";
                Txt_CurrentCTC.Text = "";
            }
            Txt_Reference_1.Text = dt.Rows[0]["Ref_Detail1"].ToString();
            Txt_Reference_2.Text = dt.Rows[0]["Ref_Detail2"].ToString();
            IsEmail = dt.Rows[0]["IS_EMail"].ToString();
            if (IsEmail.ToString() == "False")
            {
                Chk_Email.Checked = false;
            }
            else
            {
                Chk_Email.Checked = true;
            }
            IsSms = dt.Rows[0]["Is_SMS"].ToString();
            if (IsSms.ToString() == "False")
            {
                Chk_Sms.Checked = false;
            }
            else
            {
                Chk_Sms.Checked = true;
            }
            DataTable DtQualification = objCandidate.GetQualification_By_CandidateId(Edit_ID.Value);
            if (DtQualification.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvQualification, DtQualification, "", "");
                Session["dtQualification"] = DtQualification;
            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    {
        string Candidate_Id = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = CandidateMaster.Set_Hr_Candidate_Master(Candidate_Id, "", "", "", "", "", "", "0", "", "", DateTime.Now.ToString("dd-MMM-yyyy"), "", "", "True", "True", "0", "True", "True", "0", "", "0", "", "", "", "", "", "", "", "", "", "", "False", "", "", "2");
        if (b != 0)
        {
            Fill_Grid_List();
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }

    protected void Gv_Candi_Master_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Candi_Mstr_Master();
            Gv_Candi_Master_List.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["CM_Dt_Filter"] != null)
                dt = (DataTable)Session["CM_Dt_Filter"];
            else if (Session["CM_Dt_Candi_Mstr_List_Active"] != null)
                dt = (DataTable)Session["CM_Dt_Candi_Mstr_List_Active"];
            objPageCmn.FillData((object)Gv_Candi_Master_List, dt, "", "");
            //AllPageCode();
            Gv_Candi_Master_List.HeaderRow.Focus();
            Populate_Checked_Candi_Mstr_Master();
        }
        catch
        {
        }
    }

    protected void Gv_Candi_Master_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Candi_Mstr_Master();
            DataTable dt = new DataTable();
            if (Session["CM_Dt_Filter"] != null)
                dt = (DataTable)Session["CM_Dt_Filter"];
            else if (Session["CM_Dt_Candi_Mstr_List_Active"] != null)
                dt = (DataTable)Session["CM_Dt_Candi_Mstr_List_Active"];

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
            Session["CM_Dt_Filter"] = dt;
            objPageCmn.FillData((object)Gv_Candi_Master_List, dt, "", "");
            //AllPageCode();
            Gv_Candi_Master_List.HeaderRow.Focus();
            Populate_Checked_Candi_Mstr_Master();
        }
        catch
        {
        }
    }

    //--------------- End List ---------------

    //--------------- Start Bin ---------------
    protected void Btn_Bin_Li_Click(object sender, EventArgs e)
    {
        try
        {
            Session["CM_CHECKED_ITEMS_BIN"] = null;
            Session["CM_Dt_Filter_Bin"] = null;
            Fill_Grid_Bin();
        }
        catch
        {

        }
    }

    protected void Fill_Grid_Bin()
    {
        try
        {
            DataTable Dt_Candi_Mstr_Bin = CandidateMaster.Get_Hr_Candidate_Master("0", "", "", "", "", "", "", "0", "", "", DateTime.Now.ToString("dd-MMM-yyyy"), "", "", "True", "True", "0", "True", "True", "0", "", "0", "", "", "", "", "", "", "", "", "", "", "False", "", "", "0");
            Dt_Candi_Mstr_Bin = new DataView(Dt_Candi_Mstr_Bin, "Is_Active='False'", "", DataViewRowState.CurrentRows).ToTable();
            Session["CM_Dt_Candi_Mstr_Bin_InActive"] = Dt_Candi_Mstr_Bin;
            if (Dt_Candi_Mstr_Bin.Rows.Count > 0)
            {
                Fill_Gv_Bin(Dt_Candi_Mstr_Bin);
            }
            else
            {
                Fill_Gv_Bin(Dt_Candi_Mstr_Bin);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Fill_Gv_Bin(DataTable Dt_Grid)
    {
        Lbl_TotalRecords_Bin.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();
        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Candi_Master_Bin.DataSource = Dt_Grid;
            Gv_Candi_Master_Bin.DataBind();
            //AllPageCode();
            
        }
        else
        {
            Gv_Candi_Master_Bin.DataSource = Dt_Grid;
            Gv_Candi_Master_Bin.DataBind();
            //AllPageCode();
            
        }
    }

    private void Save_Checked_Candi_Mstr_Master_Bin()
    {
        ArrayList Candi_Mstrs = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Candi_Master_Bin.Rows)
        {
            index = (int)Gv_Candi_Master_Bin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin")).Checked;
            if (Session["CM_CHECKED_ITEMS_BIN"] != null)
                Candi_Mstrs = (ArrayList)Session["CM_CHECKED_ITEMS_BIN"];
            if (result)
            {
                if (!Candi_Mstrs.Contains(index))
                    Candi_Mstrs.Add(index);
            }
            else
                Candi_Mstrs.Remove(index);
        }
        if (Candi_Mstrs != null && Candi_Mstrs.Count > 0)
            Session["CM_CHECKED_ITEMS_BIN"] = Candi_Mstrs;
    }

    protected void Populate_Checked_Candi_Mstr_Master_Bin()
    {
        ArrayList Candi_Mstrs = (ArrayList)Session["CM_CHECKED_ITEMS_BIN"];
        if (Candi_Mstrs != null && Candi_Mstrs.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Candi_Master_Bin.Rows)
            {
                int index = (int)Gv_Candi_Master_Bin.DataKeys[gvrow.RowIndex].Value;
                if (Candi_Mstrs.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["CM_Dt_Filter_Bin"] = null;
            Save_Checked_Candi_Mstr_Master_Bin();
            if (ddlOptionBin.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOptionBin.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
                }
                else if (ddlOptionBin.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text.Trim() + "%'";
                }
                DataTable Dt_Candi_Mstr_Bin = (DataTable)Session["CM_Dt_Candi_Mstr_Bin_InActive"];
                DataView view = new DataView(Dt_Candi_Mstr_Bin, condition, "", DataViewRowState.CurrentRows);
                Session["CM_Dt_Filter_Bin"] = view.ToTable();
                Fill_Gv_Bin(view.ToTable());
                txtValueBin.Focus();
            }
            Populate_Checked_Candi_Mstr_Master_Bin();
        }
        catch
        {
        }
    }

    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["CM_CHECKED_ITEMS_BIN"] = null;
            Session["CM_Dt_Filter_Bin"] = null;
            foreach (GridViewRow GR in Gv_Candi_Master_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
            Fill_Gv_Bin(Session["CM_Dt_Candi_Mstr_Bin_InActive"] as DataTable);
            txtValueBin.Text = "";
            ddlOptionBin.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_Bin_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["CM_Dt_Candi_Mstr_Bin_InActive"];
        ArrayList Candi_Mstrs = new ArrayList();
        Session["CM_CHECKED_ITEMS_BIN"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CM_CHECKED_ITEMS_BIN"] != null)
                {
                    Candi_Mstrs = (ArrayList)Session["CM_CHECKED_ITEMS_BIN"];
                }
                if (!Candi_Mstrs.Contains(dr["Candidate_Id"]))
                {
                    Candi_Mstrs.Add(dr["Candidate_Id"]);
                }
            }
            foreach (GridViewRow GR in Gv_Candi_Master_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            if (Candi_Mstrs.Count > 0 && Candi_Mstrs != null)
            {
                Session["CM_CHECKED_ITEMS_BIN"] = Candi_Mstrs;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["CM_Dt_Candi_Mstr_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Candi_Master_Bin, dt, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
            
        }
    }

    protected void Img_Emp_List_Active_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList Candi_Mstr = new ArrayList();
        if (Gv_Candi_Master_Bin.Rows.Count > 0)
        {
            Save_Checked_Candi_Mstr_Master_Bin();
            if (Session["CM_CHECKED_ITEMS_BIN"] != null)
            {
                Candi_Mstr = (ArrayList)Session["CM_CHECKED_ITEMS_BIN"];
                if (Candi_Mstr.Count > 0)
                {
                    for (int j = 0; j < Candi_Mstr.Count; j++)
                    {
                        if (Candi_Mstr[j].ToString() != "")
                        {
                            b = CandidateMaster.Set_Hr_Candidate_Master(Candi_Mstr[j].ToString(), "", "", "", "", "", "", "0", "", "", DateTime.Now.ToString("dd-MMM-yyyy"), "", "", "True", "True", "0", "True", "True", "0", "", "0", "", "", "", "", "", "", "", "", "", "", "True", "", "", "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["CM_CHECKED_ITEMS_BIN"] = null;
                    Session["CM_Dt_Filter_Bin"] = null;
                    Fill_Grid_List();
                    Fill_Grid_Bin();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CM_CHECKED_ITEMS_BIN"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Candi_Master_Bin.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("Chk_Gv_Select_Bin");
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
                Gv_Candi_Master_Bin.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_Bin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Candi_Master_Bin.HeaderRow.FindControl("Chk_Gv_Select_All_Bin"));
        foreach (GridViewRow gr in Gv_Candi_Master_Bin.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
        }
    }

    protected void IBtn_Active_Command(object sender, CommandEventArgs e)
    {
        string Candidate_Id = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = CandidateMaster.Set_Hr_Candidate_Master(Candidate_Id, "", "", "", "", "", "", "0", "", "", DateTime.Now.ToString("dd-MMM-yyyy"), "", "", "True", "True", "0", "True", "True", "0", "", "0", "", "", "", "", "", "", "", "", "", "", "True", "", "", "2");
        if (b != 0)
        {
            Fill_Grid_List();
            Fill_Grid_Bin();
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
    }

    protected void Gv_Candi_Master_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Candi_Mstr_Master_Bin();
            Gv_Candi_Master_Bin.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["CM_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["CM_Dt_Filter_Bin"];
            else if (Session["CM_Dt_Candi_Mstr_Bin_InActive"] != null)
                dt = (DataTable)Session["CM_Dt_Candi_Mstr_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Candi_Master_Bin, dt, "", "");
            //AllPageCode();
            Gv_Candi_Master_Bin.HeaderRow.Focus();
            Populate_Checked_Candi_Mstr_Master_Bin();
        }
        catch
        {
        }
    }

    protected void Gv_Candi_Master_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Candi_Mstr_Master_Bin();
            DataTable dt = new DataTable();
            if (Session["CM_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["CM_Dt_Filter_Bin"];
            else if (Session["CM_Dt_Candi_Mstr_Bin_InActive"] != null)
                dt = (DataTable)Session["CM_Dt_Candi_Mstr_Bin_InActive"];

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
            Session["CM_Dt_Filter_Bin"] = dt;
            objPageCmn.FillData((object)Gv_Candi_Master_Bin, dt, "", "");
            //AllPageCode();
            Gv_Candi_Master_Bin.HeaderRow.Focus();
            Populate_Checked_Candi_Mstr_Master_Bin();
        }
        catch
        {
        }
    }

    //--------------- End Bin ---------------


    //--------------- Start New ---------------

    protected void Btn_New_Li_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        int b = 0;
        bool MarritalStatus = false;
        bool IsFresher = false;
        bool IsEmail = false;
        bool IsSms = false;
        DataTable dtQualification = new DataTable();
        if (Txt_CandidateName.Text == "")
        {
            DisplayMessage("Enter Candidate Name");
            Txt_CandidateName.Focus();
            return;
        }
        if (Txt_Father.Text == "")
        {
            DisplayMessage("Enter Father/Husband Name");
            Txt_Father.Focus();
            return;
        }
        if (Txt_Dob.Text == "")
        {
            DisplayMessage("Enter Date of Birth");
            Txt_Dob.Focus();
            return;
        }

        if (Txt_PermanentAddress.Text == "")
        {
            DisplayMessage("Enter Permanent Address");
            Txt_PermanentAddress.Focus();
            return;
        }
        if (Txt_CorrespondenceAddress.Text == "")
        {
            DisplayMessage("Enter Correspondence Address");
            Txt_CorrespondenceAddress.Focus();
            return;
        }
        if (DDL_CountryId_1.SelectedIndex == 0)
        {
            DisplayMessage("Select Country");
            DDL_Source.Focus();
            return;
        }
        if (Txt_MobileNo_1.Text == "")
        {
            DisplayMessage("Enter Mobile No");
            Txt_MobileNo_1.Focus();
            return;
        }
        if (Txt_Email_1.Text == "")
        {
            DisplayMessage("Enter Email Address");
            Txt_Email_1.Focus();
            return;
        }
        if (DDL_Source.SelectedIndex == 0)
        {
            DisplayMessage("Select Source");
            DDL_Source.Focus();
            return;
        }

        if (DDL_Source.SelectedValue == "3")
        {
            if (Txt_Reference_Emp.Text == "")
            {
                DisplayMessage("Enter Employee Name");
                Txt_Reference_Emp.Focus();
                return;

            }
        }

        if (DDL_Source.SelectedValue == "4")
        {
            if (Txt_Third_Party.Text == "")
            {
                DisplayMessage("Enter Third Party Name");
                Txt_Third_Party.Focus();
                return;

            }
        }


        if (DDL_Marrital_Status.SelectedValue == "1")
        {
            MarritalStatus = true;
        }
        if (DDL_IsFresher.SelectedValue == "0")
        {
            IsFresher = true;
            Txt_CurrentCTC.Text = "0";
            Txt_TotalExperince.Text = "0";
        }
        else
        {
            IsFresher = false;
        }
        if (Chk_Email.Checked == true)
        {
            IsEmail = true;
        }
        if (Chk_Sms.Checked == true)
        {
            IsSms = true;
        }
        try
        {
            if (ViewState["Emp_Id"].ToString() == null)
            {
                ViewState["Emp_Id"] = "0";
            }
        }
        catch (Exception Ex)
        {
            ViewState["Emp_Id"] = "0";
        }
        
        if (Edit_ID.Value == "")
        {
            b = CandidateMaster.Set_Hr_Candidate_Master("0", Txt_CandidateName.Text, Txt_Candidate_Local.Text, Txt_Father.Text, Txt_Email_1.Text, Txt_Email_2.Text, DDL_Gender.SelectedValue, DDL_CountryId_1.SelectedValue, Txt_MobileNo_1.Text, Txt_MobileNo_2.Text, Convert.ToDateTime(Txt_Dob.Text).ToString(), Txt_CorrespondenceAddress.Text, Txt_PermanentAddress.Text, MarritalStatus.ToString(), IsFresher.ToString(), ViewState["Emp_Id"].ToString(), IsSms.ToString(), IsEmail.ToString(), Txt_TotalExperince.Text, Txt_CurrentCompany.Text, Txt_CurrentCTC.Text, DDL_Source.Text, Txt_Reference_1.Text, Txt_Reference_2.Text, "", "", "", "", "", Txt_Third_Party.Text, Txt_Key_Skill.Text, "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "1");
            if(b!=0)
            {
                if (Chk_Email.Checked == true)
                {
                    if (Txt_Email_1.Text != "")
                    {
                        int ref_id = objEmailHeader.ES_EmailMasterHeader_Insert(Txt_Email_1.Text, DDL_CountryId_1.SelectedItem.ToString(), "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objEmailDetail.ES_EmailMasterDetail_Insert(b.ToString(), "Candidate", ref_id.ToString(), "True", "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    if (Txt_Email_2.Text != "")
                    {
                        int ref_id = objEmailHeader.ES_EmailMasterHeader_Insert(Txt_Email_2.Text, DDL_CountryId_1.SelectedItem.ToString(), "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objEmailDetail.ES_EmailMasterDetail_Insert(b.ToString(), "Candidate", ref_id.ToString(), "False", "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
        }
        else
        {
            b = CandidateMaster.Set_Hr_Candidate_Master(Edit_ID.Value, Txt_CandidateName.Text, Txt_Candidate_Local.Text, Txt_Father.Text, Txt_Email_1.Text, Txt_Email_2.Text, DDL_Gender.SelectedValue, DDL_CountryId_1.SelectedValue, Txt_MobileNo_1.Text, Txt_MobileNo_2.Text, Convert.ToDateTime(Txt_Dob.Text).ToString(), Txt_CorrespondenceAddress.Text, Txt_PermanentAddress.Text, MarritalStatus.ToString(), IsFresher.ToString(), ViewState["Emp_Id"].ToString(), IsSms.ToString(), IsEmail.ToString(), Txt_TotalExperince.Text, Txt_CurrentCompany.Text, Txt_CurrentCTC.Text, DDL_Source.Text, Txt_Reference_1.Text, Txt_Reference_2.Text, "", "", "", "", "", Txt_Third_Party.Text, Txt_Key_Skill.Text, "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "3");            
        }

        if (b > 0)
        {
            try
            {
                dtQualification = (DataTable)Session["dtQualification"];

                if (dtQualification.Rows.Count > 0)
                {
                    if (Edit_ID.Value != "")
                    {
                        // Delete Previous Inserted Rows For Qualification,Skills and Insert New Rows...
                        objCandidate.DeleteRecord(Edit_ID.Value, true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                    }
                    for (int i = 0; i < dtQualification.Rows.Count; i++)
                    {
                        if (dtQualification.Rows[i]["QualficationId"].ToString() != "0" || dtQualification.Rows[i]["QualficationId"].ToString() != null)
                        {

                            int q = objCandidate.InsertQualificationRecord(b.ToString(), dtQualification.Rows[i]["QualficationId"].ToString(), dtQualification.Rows[i]["BoardName"].ToString(), dtQualification.Rows[i]["CollegeName"].ToString(), dtQualification.Rows[i]["Grade"].ToString(), "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                        }
                    }
                }
            }
            catch 
            {
            }
        }
        DisplayMessage("Record Saved", "green");
        Reset();
        //AllPageCode();
        Fill_Grid_List();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    protected void Reset()
    {
        try
        {
            Txt_CandidateName.Text = "";
            Txt_Candidate_Local.Text = "";
            Txt_Father.Text = "";
            Txt_Dob.Text = "";
            Txt_PermanentAddress.Text = "";
            Chk_Address_Check.Checked = false;
            Txt_CorrespondenceAddress.Text = "";
            DDL_CountryId_1.SelectedValue = "--Country--";
            Txt_MobileNo_1.Text = "";
            DDL_Marrital_Status.SelectedValue = "0";
            DDL_CountryId_2.SelectedValue = "--Country--";
            Txt_MobileNo_2.Text = "";
            Txt_Email_1.Text = "";
            Txt_Email_2.Text = "";
            Txt_Key_Skill.Text = "";
            GvQualification.DataSource = null;
            GvQualification.DataBind();
            DDL_IsFresher.SelectedValue = "0";
            DDL_Source.SelectedValue = "0";
            Txt_Reference_1.Text = "";
            Txt_Reference_2.Text = "";
            Chk_Email.Checked = false;
            Chk_Sms.Checked = false;
            Lbl_Tab_New.Text = Resources.Attendance.New;
            Txt_CurrentCompany.Text = "";
            Txt_TotalExperince.Text = "";
            Txt_CurrentCTC.Text = "";
            Div_Fresher.Visible = false;
            Txt_Reference_Emp.Text = "";
            Div_Ref_Employee.Visible = false;
            Txt_Third_Party.Text = "";
            Div_Ref_Third_Party.Visible = false;

            DDL_Qualification.SelectedValue = "--Select--";
            Txt_SchoolName.Text = "";
            Txt_BoardName.Text = "";
            Txt_Grade.Text = "";
            Edit_ID.Value = "";
            hdnQualificationId.Value = "";
            Session["dtQualification"] = null;
        }
        catch
        {
        }
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            Reset();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
        catch
        {
        }
    }

    protected void Btn_Reset_Click(object sender, EventArgs e)
    {
        try
        {
            Reset();
            Txt_CandidateName.Focus();
        }
        catch
        {
        }
    }

    //--------------- End New ---------------
        
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethod()]
    public static string[] Get_Skill(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        SkillMaster SkillMaster = new SkillMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(SkillMaster.GetAllRecord(), "Skill like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Skill"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Emp_Name_List(string prefixText, int count, string contextKey)
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

    protected void Btn_Qualification_Edit_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Qualification_Modal_Close()", true);
        hdnQualificationId.Value = e.CommandArgument.ToString();
        DataTable dt = CreateDataTable();
        foreach (GridViewRow gvrow in GvQualification.Rows)
        {
            Label lblquaName = (Label)gvrow.FindControl("Lbl_Qualification_Name");
            HiddenField hdnquaId = (HiddenField)gvrow.FindControl("Hdn_Lbl_Qualification_Name");
            Label lblcollegename = (Label)gvrow.FindControl("Lbl_College_Name");
            Label lblBoardName = (Label)gvrow.FindControl("Lbl_University_Name");
            Label lblGrade = (Label)gvrow.FindControl("Lbl_Grade");
            DataRow dr = dt.NewRow();
            dr["QualficationId"] = hdnquaId.Value;
            dr["QualficationName"] = lblquaName.Text;
            dr["CollegeName"] = lblcollegename.Text;
            dr["BoardName"] = lblBoardName.Text;
            dr["Grade"] = lblGrade.Text;
            dt.Rows.Add(dr);
        }
        dt = new DataView(dt, "QualficationId=" + hdnQualificationId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            DDL_Qualification.SelectedValue = hdnQualificationId.Value;
            Txt_SchoolName.Text = dt.Rows[0]["CollegeName"].ToString();
            Txt_BoardName.Text = dt.Rows[0]["BoardName"].ToString();
            Txt_Grade.Text = dt.Rows[0]["Grade"].ToString();
        }
    }

    protected void Btn_Qualification_Delete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = CreateDataTable();
        foreach (GridViewRow gvrow in GvQualification.Rows)
        {
            Label lblquaName = (Label)gvrow.FindControl("Lbl_Qualification_Name");
            HiddenField hdnquaId = (HiddenField)gvrow.FindControl("Hdn_Lbl_Qualification_Name");
            Label lblcollegename = (Label)gvrow.FindControl("Lbl_College_Name");
            Label lblBoardName = (Label)gvrow.FindControl("Lbl_University_Name");
            Label lblGrade = (Label)gvrow.FindControl("Lbl_Grade");
            DataRow dr = dt.NewRow();
            dr["QualficationId"] = hdnquaId.Value;
            dr["QualficationName"] = lblquaName.Text;
            dr["CollegeName"] = lblcollegename.Text;
            dr["BoardName"] = lblBoardName.Text;
            dr["Grade"] = lblGrade.Text;
            dt.Rows.Add(dr);
        }
        dt = new DataView(dt, "QualficationId<>'" + e.CommandArgument.ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
        Session["dtQualification"] = dt;
        objPageCmn.FillData((object)GvQualification, dt, "", "");
    }

    protected void DDL_IsFresher_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Fresher.Visible = false;
        if (DDL_IsFresher.SelectedValue == "0")
        {            
            Txt_CurrentCompany.Text = "";
            Txt_TotalExperince.Text = "";
            Txt_CurrentCTC.Text = "";            
            Req_CurrentCompany.ValidationGroup = "Not_Save";
            Req_TotalExperince.ValidationGroup = "Not_Save";
            Req_CurrentCTC.ValidationGroup = "Not_Save";
        }
        else
        {
            Div_Fresher.Visible = true;
            Txt_CurrentCompany.Text = "";
            Txt_TotalExperince.Text = "";
            Txt_CurrentCTC.Text = "";
            Req_CurrentCompany.ValidationGroup = "Save";
            Req_TotalExperince.ValidationGroup = "Save";
            Req_CurrentCTC.ValidationGroup = "Save";
        }
    }

    protected void DDL_Source_SelectedIndexChanged(object sender, EventArgs e)
    {
        Req_CompRefEmp.ValidationGroup = "Not_Save";
        Req_Third_Party.ValidationGroup = "Not_Save";
        Div_Ref_Employee.Visible = false;
        Div_Ref_Third_Party.Visible = false;
        if (DDL_Source.SelectedValue == "3")
        {
            Div_Ref_Employee.Visible = true;
            Req_CompRefEmp.ValidationGroup = "Save";
            if (Txt_Reference_Emp.Text == "")
            {
                Txt_Reference_Emp.Focus();
                return;
            }
        }
        if (DDL_Source.SelectedValue == "4")
        {
            Div_Ref_Third_Party.Visible = true;
            Req_Third_Party.ValidationGroup = "Save";
            if (Txt_Third_Party.Text == "")
            {
                Txt_Third_Party.Focus();
                return;
            }
        }
    }

    protected void Txt_Reference_Emp_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = objEmp.GetEmployeeMasterByEmpName(Session["CompId"].ToString().ToString(), Txt_Reference_Emp.Text.Trim());
        if (dt.Rows.Count > 0)
        {
            Txt_Reference_Emp.Text = dt.Rows[0]["Emp_Name"].ToString();
            ViewState["Emp_Id"] = dt.Rows[0]["Emp_Id"].ToString();
            Txt_Reference_1.Focus();
        }
        else
        {
            Txt_Reference_Emp.Text = "";
            DisplayMessage("Select Employee From Suggested List");
            Txt_Reference_Emp.Focus();
            return;
        }
        if (ViewState["Emp_Id"].ToString() == "")
        {
            ViewState["Emp_Id"] = "0";
        }
    }

    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }

    protected void Btn_Qualification_Save_Click(object sender, EventArgs e)
    {
        DataTable dt = CreateDataTable();
        foreach (GridViewRow gvrow in GvQualification.Rows)
        {
            Label lblquaName = (Label)gvrow.FindControl("Lbl_Qualification_Name");
            HiddenField hdnquaId = (HiddenField)gvrow.FindControl("Hdn_Lbl_Qualification_Name");
            Label lblcollegename = (Label)gvrow.FindControl("Lbl_College_Name");
            Label lblBoardName = (Label)gvrow.FindControl("Lbl_University_Name");
            Label lblGrade = (Label)gvrow.FindControl("Lbl_Grade");
            DataRow dr = dt.NewRow();
            dr["QualficationId"] = hdnquaId.Value;
            dr["QualficationName"] = lblquaName.Text;
            dr["CollegeName"] = lblcollegename.Text;
            dr["BoardName"] = lblBoardName.Text;
            dr["Grade"] = lblGrade.Text;
            dt.Rows.Add(dr);
        }
        if (hdnQualificationId.Value == "")
        {
            if (DDL_Qualification.SelectedIndex == 0)
            {
                DisplayMessage("Select Qualification");
                DDL_Qualification.Focus();
                return;
            }
            if (Txt_SchoolName.Text == "")
            {
                DisplayMessage("Type School/College Name");
                Txt_SchoolName.Focus();
                return;
            }
            if (Txt_BoardName.Text == "")
            {
                DisplayMessage("Type Board/University Name");
                Txt_BoardName.Focus();
                return;
            }
            if (Txt_Grade.Text == "")
            {
                DisplayMessage("Type Grade");
                Txt_Grade.Focus();
                return;
            }
            DataRow drNew = dt.NewRow();
            drNew["QualficationId"] = DDL_Qualification.SelectedValue;
            drNew["QualficationName"] = DDL_Qualification.SelectedItem.Text;
            drNew["CollegeName"] = Txt_SchoolName.Text;
            drNew["BoardName"] = Txt_BoardName.Text;
            drNew["Grade"] = Txt_Grade.Text;
            dt.Rows.Add(drNew);
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["QualficationId"].ToString() == hdnQualificationId.Value)
                {
                    dt.Rows[i]["QualficationId"] = DDL_Qualification.SelectedValue;
                    dt.Rows[i]["QualficationName"] = DDL_Qualification.SelectedItem.Text;
                    dt.Rows[i]["CollegeName"] = Txt_SchoolName.Text;
                    dt.Rows[i]["BoardName"] = Txt_BoardName.Text;
                    dt.Rows[i]["Grade"] = Txt_Grade.Text;
                }
            }
        }
        objPageCmn.FillData((object)GvQualification, dt, "", "");
        Session["dtQualification"] = dt;
        ResetQualification();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Qualification_Modal_Close()", true);
    }

    public DataTable CreateDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("QualficationId");
        dt.Columns.Add("QualficationName");
        dt.Columns.Add("CollegeName");
        dt.Columns.Add("BoardName");
        dt.Columns.Add("Grade");
        return dt;
    }

    public void ResetQualification()
    {
        Txt_SchoolName.Text = "";
        Txt_BoardName.Text = "";
        Txt_Grade.Text = "";
        Bind_DDL_Qualification();
        DDL_Qualification.SelectedIndex = 0;
        DDL_Qualification.Enabled = true;
        hdnQualificationId.Value = "";
    }

    private void Bind_DDL_Qualification()
    {
        DataTable dt = QualificationMaster.GetQualificationMaster();
        dt = new DataView(dt, "", "Qualification Asc", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)DDL_Qualification, dt, "Qualification", "Qualification_Id");
    }

    protected void Btn_Qualification_Reset_Click(object sender, EventArgs e)
    {
        ResetQualification();
    }

    private void Bind_Country()
    {
        DataTable dtCOuntry = CountryMaster.GetCountryMaster();

        dtCOuntry = new DataView(dtCOuntry, "", "Country_Code Asc", DataViewRowState.CurrentRows).ToTable();
        if (dtCOuntry.Rows.Count > 0)
        {
            DDL_CountryId_1.DataSource = null;
            DDL_CountryId_1.DataBind();
            objPageCmn.Fill_Country_Code((object)DDL_CountryId_1, dtCOuntry, "Country_Code", "Country_Id");

            DDL_CountryId_2.DataSource = null;
            DDL_CountryId_2.DataBind();
            objPageCmn.Fill_Country_Code((object)DDL_CountryId_2, dtCOuntry, "Country_Code", "Country_Id");
        }
        else
        {
            try
            {
                DDL_CountryId_1.Items.Clear();
                DDL_CountryId_1.DataSource = null;
                DDL_CountryId_1.DataBind();

                DDL_CountryId_2.Items.Clear();
                DDL_CountryId_2.DataSource = null;
                DDL_CountryId_2.DataBind();
            }
            catch
            {

            }
        }
    }
    
}