using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using PegasusDataAccess;
using System.Web.UI.WebControls;

public partial class ProjectManagement_VehicleMaster : System.Web.UI.Page
{
    Common cmn = null;
    Prj_VehicleMaster objVehicleMaster = null;
    VehicleMaster Vehicle_Master = null;
    IT_ObjectEntry objObjectEntry = null;
    SystemParameter objSys = null;
    DataAccessClass objDa = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_SubChartOfAccount objSubCOA = null;
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
            Btn_Save.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(Btn_Save, "").ToString());
            Page.Title = objSys.GetSysTitle();
          
            CE_Search_Date.Format = objSys.SetDateFormat();
            CE_Expiry_Date.Format = objSys.SetDateFormat();
            CE_PUC_Expiry_Date.Format = objSys.SetDateFormat();
            CE_Service_Due_Date.Format = objSys.SetDateFormat();
            CE_Search_Date_Bin.Format = objSys.SetDateFormat();

            cmn = new Common(Session["DBConnection"].ToString());
            objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
            Vehicle_Master = new VehicleMaster(Session["DBConnection"].ToString());
            objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
            objSys = new SystemParameter(Session["DBConnection"].ToString());
            objDa = new DataAccessClass(Session["DBConnection"].ToString());
            objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
            objSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

            if (!IsPostBack)
            {
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ProjectManagement/VehicleMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
                if (clsPagePermission.bHavePermission == false)
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                AllPageCode(clsPagePermission);
                Fill_Grid_List();
                ddlVehicleType_OnSelectedIndexChanged(null, null);
            }
        }
        catch
        {
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Save.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        Img_Emp_List_Delete_All.Visible = clsPagePermission.bDelete.ToString().ToLower() == "true" ? true : false;
        Img_Emp_List_Select_All.Visible = clsPagePermission.bDelete.ToString().ToLower() == "true" ? true : false;
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
            Session["VihicleM_CHECKED_ITEMS_LIST"] = null;
            Session["VihicleM_Dt_Filter"] = null;
            DataTable Dt_Vehicle_Master_List = Vehicle_Master.Get_Vehicle_Master(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "True", "2", Session["UserID"].ToString());
            Session["VihicleM_Dt_Vehicle_Master_List_Active"] = Dt_Vehicle_Master_List;
            if (Dt_Vehicle_Master_List.Rows.Count > 0)
            {
                Fill_Gv_Vehicle_Master(Dt_Vehicle_Master_List);
            }
            else
            {
                Fill_Gv_Vehicle_Master(Dt_Vehicle_Master_List);
            }
        }
        catch 
        {
        }
    }

    protected void Fill_Gv_Vehicle_Master(DataTable Dt_Grid)
    {
        if (Dt_Grid != null)
        {
            Lbl_TotalRecords.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();

            if (Dt_Grid.Rows.Count > 0)
            {
                Gv_Vehicle_Master_List.DataSource = Dt_Grid;
                Gv_Vehicle_Master_List.DataBind();
              
              
            }
            else
            {
                Gv_Vehicle_Master_List.DataSource = null;
                Gv_Vehicle_Master_List.DataBind();
                
            }
        }
        else
        {
            Lbl_TotalRecords.Text = "Total Records: 0";
            Gv_Vehicle_Master_List.DataSource = null;
            Gv_Vehicle_Master_List.DataBind();
          
        }
    }

    private void Save_Checked_Vehicle_Master_Master()
    {
        ArrayList Vehicle_Master_Delete_Alls = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Vehicle_Master_List.Rows)
        {
            index = (int)Gv_Vehicle_Master_List.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select")).Checked;
            if (Session["VihicleM_CHECKED_ITEMS_LIST"] != null)
                Vehicle_Master_Delete_Alls = (ArrayList)Session["VihicleM_CHECKED_ITEMS_LIST"];
            if (result)
            {
                if (!Vehicle_Master_Delete_Alls.Contains(index))
                    Vehicle_Master_Delete_Alls.Add(index);
            }
            else
                Vehicle_Master_Delete_Alls.Remove(index);
        }
        if (Vehicle_Master_Delete_Alls != null && Vehicle_Master_Delete_Alls.Count > 0)
            Session["VihicleM_CHECKED_ITEMS_LIST"] = Vehicle_Master_Delete_Alls;
    }

    protected void Populate_Checked_Vehicle_Master_Master()
    {
        ArrayList Vehicle_Master_Delete_Alls = (ArrayList)Session["VihicleM_CHECKED_ITEMS_LIST"];
        if (Vehicle_Master_Delete_Alls != null && Vehicle_Master_Delete_Alls.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Vehicle_Master_List.Rows)
            {
                int index = (int)Gv_Vehicle_Master_List.DataKeys[gvrow.RowIndex].Value;
                if (Vehicle_Master_Delete_Alls.Contains(index))
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

        if (Session["VihicleM_Dt_Vehicle_Master_List_Active"] != null)
        {
            try
            {
                Session["VihicleM_Dt_Filter"] = null;
                Save_Checked_Vehicle_Master_Master();
                if (ddlOption.SelectedIndex != 0)
                {
                    string condition = string.Empty;
                    if (ddlFieldName.SelectedValue == "Expiry_Date" || ddlFieldName.SelectedValue == "Pvc_Expiry_Date" || ddlFieldName.SelectedValue == "ServiceDueDate")
                    {
                        if (ddlOption.SelectedIndex == 1)
                        {
                            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + objSys.getDateForInput(Txt_Search_Date.Text.Trim()) + "'";
                        }
                        else if (ddlOption.SelectedIndex == 2)
                        {
                            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + objSys.getDateForInput(Txt_Search_Date.Text.Trim()) + "%'";
                        }
                        else
                        {
                            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + objSys.getDateForInput(Txt_Search_Date.Text.Trim()) + "%'";
                        }
                    }
                    else
                    {
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
                    }

                    DataTable Dt_Vehicle_Master_List = (DataTable)Session["VihicleM_Dt_Vehicle_Master_List_Active"];
                    DataView view = new DataView(Dt_Vehicle_Master_List, condition, "", DataViewRowState.CurrentRows);
                    Session["VihicleM_Dt_Filter"] = view.ToTable();
                    Fill_Gv_Vehicle_Master(view.ToTable());
                    txtValue.Focus();
                }
                Populate_Checked_Vehicle_Master_Master();
            }
            catch
            {
            }
        }
        else
        {
            Fill_Grid_List();
            btnbind_Click(null, null);
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (Session["VihicleM_Dt_Vehicle_Master_List_Active"] != null)
        {
            try
            {
                Session["VihicleM_CHECKED_ITEMS_LIST"] = null;
                Session["VihicleM_Dt_Filter"] = null;
                foreach (GridViewRow GR in Gv_Vehicle_Master_List.Rows)
                {
                    ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = false;
                }
                Fill_Gv_Vehicle_Master(Session["VihicleM_Dt_Vehicle_Master_List_Active"] as DataTable);
                txtValue.Text = "";
                ddlOption.SelectedIndex = 2;
            }
            catch
            {
            }
        }
        else
        {
            Fill_Grid_List();
            btnRefresh_Click(null, null);
        }
    }

    protected void Img_Emp_List_Select_All_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (Session["VihicleM_Dt_Vehicle_Master_List_Active"] != null)
        {
            DataTable dtUnit = (DataTable)Session["VihicleM_Dt_Vehicle_Master_List_Active"];
            ArrayList Vehicle_Master_Delete_Alls = new ArrayList();
            Session["VihicleM_CHECKED_ITEMS_LIST"] = null;
            if (ViewState["Select"] == null)
            {
                ViewState["Select"] = 1;
                foreach (DataRow dr in dtUnit.Rows)
                {
                    if (Session["VihicleM_CHECKED_ITEMS_LIST"] != null)
                    {
                        Vehicle_Master_Delete_Alls = (ArrayList)Session["VihicleM_CHECKED_ITEMS_LIST"];
                    }
                    if (!Vehicle_Master_Delete_Alls.Contains(dr["Vehicle_Id"]))
                    {
                        Vehicle_Master_Delete_Alls.Add(dr["Vehicle_Id"]);
                    }
                }
                foreach (GridViewRow GR in Gv_Vehicle_Master_List.Rows)
                {
                    ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = true;
                }
                if (Vehicle_Master_Delete_Alls.Count > 0 && Vehicle_Master_Delete_Alls != null)
                {
                    Session["VihicleM_CHECKED_ITEMS_LIST"] = Vehicle_Master_Delete_Alls;
                }
            }
            else
            {
                DataTable dt = (DataTable)Session["VihicleM_Dt_Vehicle_Master_List_Active"];
                objPageCmn.FillData((object)Gv_Vehicle_Master_List, dt, "", "");
              
                ViewState["Select"] = null;
               
            }
        }
        else
        {
            Fill_Grid_List();
            Img_Emp_List_Select_All_Click(null, null);
        }
    }

    protected void Img_Emp_List_Delete_All_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        string check = string.Empty;
        int b = 0;
        DataTable Dt_Check_Contract = Vehicle_Master.Get_Vehicle_Master(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", "True", "3", Session["UserId"].ToString());
        DataTable Dt_Check_Rent_Trans = Vehicle_Master.Get_Vehicle_Master(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", "True", "4", Session["UserId"].ToString());

        ArrayList Vehicle_Master_Delete_All = new ArrayList();
        if (Gv_Vehicle_Master_List.Rows.Count > 0)
        {
            Save_Checked_Vehicle_Master_Master();
            if (Session["VihicleM_CHECKED_ITEMS_LIST"] != null)
            {
                Vehicle_Master_Delete_All = (ArrayList)Session["VihicleM_CHECKED_ITEMS_LIST"];
                if (Vehicle_Master_Delete_All.Count > 0)
                {
                    for (int j = 0; j < Vehicle_Master_Delete_All.Count; j++)
                    {
                        if (Vehicle_Master_Delete_All[j].ToString() != "")
                        {
                            DataTable Dt_Contract = new DataView(Dt_Check_Contract, "Contract_Vehicle_Id = " + Vehicle_Master_Delete_All[j].ToString(), "", DataViewRowState.CurrentRows).ToTable();
                            DataTable Dt_Rent_Trans = new DataView(Dt_Check_Rent_Trans, "Rent_Trans_Vehicle_Id = " + Vehicle_Master_Delete_All[j].ToString(), "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Contract.Rows.Count > 0 && Dt_Rent_Trans.Rows.Count > 0)
                            {
                                if (Dt_Contract.Rows[0]["Contract_Vehicle_Count"].ToString() != Dt_Rent_Trans.Rows[0]["Rent_Trans_Vehicle_Count"].ToString())
                                    check = Dt_Contract.Rows[0]["Contract_Vehicle_Name"].ToString() + " \\n " + check;
                            }
                            else if (Dt_Contract.Rows.Count > 0 && Dt_Rent_Trans.Rows.Count == 0)
                            {
                                check = Dt_Contract.Rows[0]["Contract_Vehicle_Name"].ToString() + " \\n " + check;
                            }
                        }
                    }
                }
                if (check == "")
                {
                    if (Vehicle_Master_Delete_All.Count > 0)
                    {
                        for (int j = 0; j < Vehicle_Master_Delete_All.Count; j++)
                        {
                            if (Vehicle_Master_Delete_All[j].ToString() != "")
                            {
                                b = Vehicle_Master.Set_Vehicle_Master(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Vehicle_Master_Delete_All[j].ToString(), "False", "1", Session["UserId"].ToString());
                            }
                        }
                    }
                }
                else
                {
                    DisplayMessage("Following Vehicle already used in application \\n " + check + "");
                    return;
                }
                if (b != 0)
                {
                    Session["VihicleM_CHECKED_ITEMS_LIST"] = null;
                    Session["VihicleM_Dt_Filter"] = null;
                    Fill_Grid_List();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Deleted");
                    Session["VihicleM_CHECKED_ITEMS_LIST"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Vehicle_Master_List.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("Chk_Gv_Select");
                        if (chk.Checked)
                        {
                            fleg++;
                        }
                        else
                        {
                            fleg++;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
                    }
                    else
                    {
                        DisplayMessage("Selected Vehicle Reference already used in application");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                Gv_Vehicle_Master_List.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Vehicle_Master_List.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow gr in Gv_Vehicle_Master_List.Rows)
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
        if (Session["VihicleM_Dt_Vehicle_Master_List_Active"] != null)
        {
            GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
            string Vehicle_Id = e.CommandArgument.ToString();
            Edit_Id.Value = e.CommandArgument.ToString();
            DataTable Dt_Vehicle_Master_List = Session["VihicleM_Dt_Vehicle_Master_List_Active"] as DataTable;
            Dt_Vehicle_Master_List = new DataView(Dt_Vehicle_Master_List, "Vehicle_Id = " + Vehicle_Id, "", DataViewRowState.CurrentRows).ToTable();
            if (Dt_Vehicle_Master_List.Rows.Count > 0)
            {
                txtvehiclename.Text = Dt_Vehicle_Master_List.Rows[0]["Name"].ToString();
                txtplateno.Text = Dt_Vehicle_Master_List.Rows[0]["Plate_No"].ToString();
                txtModelNo.Text = Dt_Vehicle_Master_List.Rows[0]["Model_No"].ToString();
                txtregistrationNo.Text = Dt_Vehicle_Master_List.Rows[0]["Reg_No"].ToString();
                txtexpiryDate.Text = GetDate(Dt_Vehicle_Master_List.Rows[0]["Expiry_Date"].ToString());
                txtpvcexpirydate.Text = GetDate(Dt_Vehicle_Master_List.Rows[0]["Pvc_Expiry_Date"].ToString());
                ddlVehicleType.SelectedValue = Dt_Vehicle_Master_List.Rows[0]["Field1"].ToString();
                txtownername.Text = Dt_Vehicle_Master_List.Rows[0]["Owner"].ToString();
                txtProductName.Text = Dt_Vehicle_Master_List.Rows[0]["EProductName"].ToString() + "/" + Dt_Vehicle_Master_List.Rows[0]["Product_Id"].ToString();
                txtServiceDueDate.Text = GetDate(Dt_Vehicle_Master_List.Rows[0]["ServiceDueDate"].ToString());
                txtServiceDueReading.Text = Dt_Vehicle_Master_List.Rows[0]["ServiceDueReading"].ToString();
                txtRemarks.Text = Dt_Vehicle_Master_List.Rows[0]["Remarks"].ToString();
                if (String.IsNullOrEmpty(Dt_Vehicle_Master_List.Rows[0]["Field1"].ToString()) || Dt_Vehicle_Master_List.Rows[0]["Field1"].ToString() == "Own")
                {
                    lblProductName.Visible = true;
                    txtProductName.Visible = true;
                }
                else
                {
                    lblProductName.Visible = false;
                    txtProductName.Visible = false;
                }
                if (Dt_Vehicle_Master_List.Rows[0]["Emp_Id"].ToString() != "0")
                {
                    //txtEmpName.Text = Dt_Vehicle_Master_List.Rows[0]["Emp_Name"].ToString().Trim() + "/" + Dt_Vehicle_Master_List.Rows[0]["Emp_Id"].ToString().Trim();
                    txtEmpName.Text = Dt_Vehicle_Master_List.Rows[0]["Emp_Name"].ToString().Trim() + "/" + Dt_Vehicle_Master_List.Rows[0]["Emp_Code"].ToString().Trim();
                }
                else
                {
                    txtEmpName.Text = Dt_Vehicle_Master_List.Rows[0]["Field2"].ToString().Trim();
                }
                string VehicleAccountId = Ac_ParameterMaster.GetVehicleAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                if (!String.IsNullOrEmpty(VehicleAccountId))
                {
                    string VehicleQuery = "Select * from Ac_SubChartOfAccount where Company_Id = " + Session["CompId"].ToString() + " and Brand_Id = " + Session["BrandId"].ToString() + " and Location_Id = " + Session["LocId"].ToString() + " and FinancialYearId = " + Session["FinanceYearId"].ToString() + " and AccTransId = " + VehicleAccountId + " and Other_Account_No = " + Edit_Id.Value.ToString() + "";

                    DataTable Balancedt = objDa.return_DataTable(VehicleQuery);

                    if (Balancedt != null && Balancedt.Rows.Count > 0)
                    {
                        txtOpeningDebitAmt.Text = double.Parse(Balancedt.Rows[0]["LDr_Amount"].ToString()).ToString();
                        txtOpeningCreditAmt.Text = double.Parse(Balancedt.Rows[0]["LCr_Amount"].ToString()).ToString();
                    }
                }
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
        }
        else
        {
            Fill_Grid_List();
        }
    }

    protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    {
        string Vehicle_Id = e.CommandArgument.ToString();
        DataTable Dt_Check = Vehicle_Master.Get_Vehicle_Master(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Vehicle_Id, "True", "1", Session["UserID"].ToString());
        if (Dt_Check.Rows.Count > 0)
        {
            if (Dt_Check.Rows[0]["Vehicle_Contract"].ToString() != Dt_Check.Rows[0]["Vehicle_Rent_Trans"].ToString())
            {
                DisplayMessage("This Vehicle already used in application");
            }

            else
            {
                int b = 0;
                String CompanyId = Session["CompId"].ToString();
                b = Vehicle_Master.Set_Vehicle_Master(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Vehicle_Id, "False", "1", Session["UserId"].ToString());
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
        }
    }

    protected void Gv_Vehicle_Master_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["VihicleM_Dt_Vehicle_Master_List_Active"] != null)
        {
            try
            {
                Save_Checked_Vehicle_Master_Master();
                Gv_Vehicle_Master_List.PageIndex = e.NewPageIndex;
                DataTable dt = new DataTable();
                if (Session["VihicleM_Dt_Filter"] != null)
                    dt = (DataTable)Session["VihicleM_Dt_Filter"];
                else if (Session["VihicleM_Dt_Vehicle_Master_List_Active"] != null)
                    dt = (DataTable)Session["VihicleM_Dt_Vehicle_Master_List_Active"];
                objPageCmn.FillData((object)Gv_Vehicle_Master_List, dt, "", "");
           
                Gv_Vehicle_Master_List.HeaderRow.Focus();
                Populate_Checked_Vehicle_Master_Master();
            }
            catch
            {
            }
        }
        else
        {
            Fill_Grid_List();
        }
    }

    protected void Gv_Vehicle_Master_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (Session["VihicleM_Dt_Vehicle_Master_List_Active"] != null)
        {
            try
            {
                Save_Checked_Vehicle_Master_Master();
                DataTable dt = new DataTable();
                if (Session["VihicleM_Dt_Filter"] != null)
                    dt = (DataTable)Session["VihicleM_Dt_Filter"];
                else if (Session["VihicleM_Dt_Vehicle_Master_List_Active"] != null)
                    dt = (DataTable)Session["VihicleM_Dt_Vehicle_Master_List_Active"];

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
                Session["VihicleM_Dt_Filter"] = dt;
                objPageCmn.FillData((object)Gv_Vehicle_Master_List, dt, "", "");
               
                Gv_Vehicle_Master_List.HeaderRow.Focus();
                Populate_Checked_Vehicle_Master_Master();
            }
            catch
            {
            }
        }
        else
        {
            Fill_Grid_List();
        }
    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedValue == "Expiry_Date" || ddlFieldName.SelectedValue == "Pvc_Expiry_Date" || ddlFieldName.SelectedValue == "ServiceDueDate")
        {
            txtValue.Text = "";
            Txt_Search_Date.Text = "";
            txtValue.Visible = false;
            Txt_Search_Date.Visible = true;
        }
        else
        {
            txtValue.Text = "";
            Txt_Search_Date.Text = "";
            txtValue.Visible = true;
            Txt_Search_Date.Visible = false;
        }
    }

    //--------------- End List ---------------

    //--------------- Start Bin ---------------

    protected void Btn_Bin_Li_Click(object sender, EventArgs e)
    {
        try
        {            
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
                Session["VihicleM_CHECKED_ITEMS_BIN"] = null;
                Session["VihicleM_Dt_Filter_Bin"] = null;
                DataTable Dt_Vehicle_Master_Bin = Vehicle_Master.Get_Vehicle_Master(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "False", "2", Session["UserID"].ToString());
                Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"] = Dt_Vehicle_Master_Bin;
                if (Dt_Vehicle_Master_Bin.Rows.Count > 0)
                {
                    Fill_Gv_Bin(Dt_Vehicle_Master_Bin);
                }
                else
                {
                    Fill_Gv_Bin(Dt_Vehicle_Master_Bin);
                }
            }
            catch
            {
            }
        }

    protected void Fill_Gv_Bin(DataTable Dt_Grid)
    {
        if (Dt_Grid != null)
        {
            Lbl_TotalRecords_Bin.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();
            if (Dt_Grid.Rows.Count > 0)
            {
                Gv_Vehicle_Master_Bin.DataSource = Dt_Grid;
                Gv_Vehicle_Master_Bin.DataBind();
              
                Img_Emp_Bin_Select_All.Visible = true;
                Img_Emp_List_Active.Visible = true;
            }
            else
            {
                Gv_Vehicle_Master_Bin.DataSource = Dt_Grid;
                Gv_Vehicle_Master_Bin.DataBind();
                Img_Emp_Bin_Select_All.Visible = false;
                Img_Emp_List_Active.Visible = false;
            }
        }
        else
        {
            Lbl_TotalRecords_Bin.Text = "Total Records: 0";
            Gv_Vehicle_Master_Bin.DataSource = null;
            Gv_Vehicle_Master_Bin.DataBind();
            Img_Emp_Bin_Select_All.Visible = false;
            Img_Emp_List_Active.Visible = false;
        }
    }

    private void Save_Checked_Vehicle_Master_Master_Bin()
    {
        ArrayList Vehicle_Master_Delete_Alls = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Vehicle_Master_Bin.Rows)
        {
            index = (int)Gv_Vehicle_Master_Bin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin")).Checked;
            if (Session["VihicleM_CHECKED_ITEMS_BIN"] != null)
                Vehicle_Master_Delete_Alls = (ArrayList)Session["VihicleM_CHECKED_ITEMS_BIN"];
            if (result)
            {
                if (!Vehicle_Master_Delete_Alls.Contains(index))
                    Vehicle_Master_Delete_Alls.Add(index);
            }
            else
                Vehicle_Master_Delete_Alls.Remove(index);
        }
        if (Vehicle_Master_Delete_Alls != null && Vehicle_Master_Delete_Alls.Count > 0)
            Session["VihicleM_CHECKED_ITEMS_BIN"] = Vehicle_Master_Delete_Alls;
    }

    protected void Populate_Checked_Vehicle_Master_Master_Bin()
    {
        ArrayList Vehicle_Master_Delete_Alls = (ArrayList)Session["VihicleM_CHECKED_ITEMS_BIN"];
        if (Vehicle_Master_Delete_Alls != null && Vehicle_Master_Delete_Alls.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Vehicle_Master_Bin.Rows)
            {
                int index = (int)Gv_Vehicle_Master_Bin.DataKeys[gvrow.RowIndex].Value;
                if (Vehicle_Master_Delete_Alls.Contains(index))
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
        if (Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"] != null)
        {
            try
            {
                Session["VihicleM_Dt_Filter_Bin"] = null;
                Save_Checked_Vehicle_Master_Master_Bin();
                if (ddlOptionBin.SelectedIndex != 0)
                {
                    string condition = string.Empty;
                    if (ddlFieldNameBin.SelectedValue == "Expiry_Date" || ddlFieldNameBin.SelectedValue == "Pvc_Expiry_Date" || ddlFieldNameBin.SelectedValue == "ServiceDueDate")
                    {
                        if (ddlOptionBin.SelectedIndex == 1)
                        {
                            condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + objSys.getDateForInput(Txt_Search_Date_Bin.Text.Trim()) + "'";
                        }
                        else if (ddlOptionBin.SelectedIndex == 2)
                        {
                            condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + objSys.getDateForInput(Txt_Search_Date_Bin.Text.Trim()) + "%'";
                        }
                        else
                        {
                            condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + objSys.getDateForInput(Txt_Search_Date_Bin.Text.Trim()) + "%'";
                        }
                    }
                    else
                    {
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
                    }
                    DataTable Dt_Vehicle_Master_Bin = (DataTable)Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"];
                    DataView view = new DataView(Dt_Vehicle_Master_Bin, condition, "", DataViewRowState.CurrentRows);
                    Session["VihicleM_Dt_Filter_Bin"] = view.ToTable();
                    Fill_Gv_Bin(view.ToTable());
                    txtValueBin.Focus();
                }
                Populate_Checked_Vehicle_Master_Master_Bin();
            }
            catch
            {
            }
        }
        else
        {
            Fill_Grid_Bin();
            btnbindBin_Click(null, null);
        }
    }

    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"] != null)
        {
            try
            {
                Session["VihicleM_CHECKED_ITEMS_BIN"] = null;
                Session["VihicleM_Dt_Filter_Bin"] = null;
                foreach (GridViewRow GR in Gv_Vehicle_Master_Bin.Rows)
                {
                    ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = false;
                }
                Fill_Gv_Bin(Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"] as DataTable);
                txtValueBin.Text = "";
                ddlOptionBin.SelectedIndex = 2;
            }
            catch
            {
            }
        }
        else
        {
            Fill_Grid_Bin();
            btnRefreshBin_Click(null, null);
        }
    }

    protected void Img_Emp_Bin_Select_All_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"] != null)
        {
            DataTable dtUnit = (DataTable)Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"];
            ArrayList Vehicle_Master_Delete_Alls = new ArrayList();
            Session["VihicleM_CHECKED_ITEMS_BIN"] = null;
            if (ViewState["Select"] == null)
            {
                ViewState["Select"] = 1;
                foreach (DataRow dr in dtUnit.Rows)
                {
                    if (Session["VihicleM_CHECKED_ITEMS_BIN"] != null)
                    {
                        Vehicle_Master_Delete_Alls = (ArrayList)Session["VihicleM_CHECKED_ITEMS_BIN"];
                    }
                    if (!Vehicle_Master_Delete_Alls.Contains(dr["Vehicle_Id"]))
                    {
                        Vehicle_Master_Delete_Alls.Add(dr["Vehicle_Id"]);
                    }
                }
                foreach (GridViewRow GR in Gv_Vehicle_Master_Bin.Rows)
                {
                    ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = true;
                }
                if (Vehicle_Master_Delete_Alls.Count > 0 && Vehicle_Master_Delete_Alls != null)
                {
                    Session["VihicleM_CHECKED_ITEMS_BIN"] = Vehicle_Master_Delete_Alls;
                }
            }
            else
            {
                DataTable dt = (DataTable)Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"];
                objPageCmn.FillData((object)Gv_Vehicle_Master_Bin, dt, "", "");
            
                ViewState["Select"] = null;
                if (dt.Rows.Count > 0)
                {
                    Img_Emp_Bin_Select_All.Visible = true;
                    Img_Emp_List_Active.Visible = true;
                }
                else
                {
                    Img_Emp_Bin_Select_All.Visible = false;
                    Img_Emp_List_Active.Visible = false;
                }
            }
        }
        else
        {
            Fill_Grid_Bin();
            Img_Emp_Bin_Select_All_Click(null, null);
        }
    }

    protected void Img_Emp_List_Active_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList Vehicle_Master_Delete_All = new ArrayList();
        if (Gv_Vehicle_Master_Bin.Rows.Count > 0)
        {
            Save_Checked_Vehicle_Master_Master_Bin();
            if (Session["VihicleM_CHECKED_ITEMS_BIN"] != null)
            {
                Vehicle_Master_Delete_All = (ArrayList)Session["VihicleM_CHECKED_ITEMS_BIN"];
                if (Vehicle_Master_Delete_All.Count > 0)
                {
                    for (int j = 0; j < Vehicle_Master_Delete_All.Count; j++)
                    {
                        if (Vehicle_Master_Delete_All[j].ToString() != "")
                        {
                            b = Vehicle_Master.Set_Vehicle_Master(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Vehicle_Master_Delete_All[j].ToString(), "True", "1", Session["UserId"].ToString());
                        }
                    }
                }
                if (b != 0)
                {
                    Session["VihicleM_CHECKED_ITEMS_BIN"] = null;
                    Session["VihicleM_Dt_Filter_Bin"] = null;
                    Fill_Grid_List();
                    Fill_Grid_Bin();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["VihicleM_CHECKED_ITEMS_BIN"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Vehicle_Master_Bin.Rows)
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
                Gv_Vehicle_Master_Bin.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_Bin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Vehicle_Master_Bin.HeaderRow.FindControl("Chk_Gv_Select_All_Bin"));
        foreach (GridViewRow gr in Gv_Vehicle_Master_Bin.Rows)
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
        string Vehicle_Id = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = Vehicle_Master.Set_Vehicle_Master(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Vehicle_Id, "True", "1", Session["UserId"].ToString());
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

    protected void Gv_Vehicle_Master_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"] != null)
        {
            try
            {
                Save_Checked_Vehicle_Master_Master_Bin();
                Gv_Vehicle_Master_Bin.PageIndex = e.NewPageIndex;
                DataTable dt = new DataTable();
                if (Session["VihicleM_Dt_Filter_Bin"] != null)
                    dt = (DataTable)Session["VihicleM_Dt_Filter_Bin"];
                else if (Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"] != null)
                    dt = (DataTable)Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"];
                objPageCmn.FillData((object)Gv_Vehicle_Master_Bin, dt, "", "");
              
                Gv_Vehicle_Master_Bin.HeaderRow.Focus();
                Populate_Checked_Vehicle_Master_Master_Bin();
            }
            catch
            {
            }
        }
        else
        {
            Fill_Grid_Bin();
        }
    }

    protected void Gv_Vehicle_Master_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"] != null)
        {
            try
            {
                Save_Checked_Vehicle_Master_Master_Bin();
                DataTable dt = new DataTable();
                if (Session["VihicleM_Dt_Filter_Bin"] != null)
                    dt = (DataTable)Session["VihicleM_Dt_Filter_Bin"];
                else if (Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"] != null)
                    dt = (DataTable)Session["VihicleM_Dt_Vehicle_Master_Bin_InActive"];

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
                Session["VihicleM_Dt_Filter_Bin"] = dt;
                objPageCmn.FillData((object)Gv_Vehicle_Master_Bin, dt, "", "");
             
                Gv_Vehicle_Master_Bin.HeaderRow.Focus();
                Populate_Checked_Vehicle_Master_Master_Bin();
            }
            catch
            {
            }
        }
        else
        {
            Fill_Grid_Bin();
        }
    }

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.SelectedValue == "Expiry_Date" || ddlFieldNameBin.SelectedValue == "Pvc_Expiry_Date" || ddlFieldNameBin.SelectedValue == "ServiceDueDate")
        {
            txtValueBin.Text = "";
            Txt_Search_Date_Bin.Text = "";
            txtValueBin.Visible = false;
            Txt_Search_Date_Bin.Visible = true;
        }
        else
        {
            txtValueBin.Text = "";
            Txt_Search_Date_Bin.Text = "";
            txtValueBin.Visible = true;
            Txt_Search_Date_Bin.Visible = false;
        }
    }

    //--------------- End Bin ---------------


    //--------------- Start New ---------------

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] Get_Vehicle_Title(string prefixText, int count, string contextKey)
    //{
    //    //if (HttpContext.Current.Session["UserId"] == null)
    //    //{
    //    //    HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
    //    //}
    //    //VehicleMaster VehicleMaster = new VehicleMaster();
    //    //DataTable dt = new DataView(VehicleMaster.Get_Vehicle_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "", "", "", "True", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "2"), "Title like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
    //    //string[] txt = new string[dt.Rows.Count];
    //    //for (int i = 0; i < dt.Rows.Count; i++)
    //    //{
    //    //    txt[i] = dt.Rows[i]["Title"].ToString();
    //    //}
    //    return txt;
    //}

    protected void Btn_New_Li_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        string Driverid = string.Empty;
        string ProductId = string.Empty;
        string DriVerName = string.Empty;
        int b = 0;

        if (txtOpeningCreditAmt.Text == "")
            txtOpeningCreditAmt.Text = "0";

        if (txtOpeningDebitAmt.Text == "")
            txtOpeningDebitAmt.Text = "0";

        bool IsOpeningBal = false;
        if (int.Parse(txtOpeningCreditAmt.Text.ToString()) > 0)
        {
            IsOpeningBal = true;
        }
        else if (int.Parse(txtOpeningDebitAmt.Text.ToString()) > 0)
        {
            IsOpeningBal = true;
        }
        if (IsOpeningBal)
        {
            string VlcAcc = Ac_ParameterMaster.GetVehicleAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            if (VlcAcc == "0")
            {
                DisplayMessage("Please configure Vehicle Account in Finance");
                return;
            }
        }
        if (txtvehiclename.Text.Trim() == "")
        {
            DisplayMessage("Enter Vehicle Name");
            txtvehiclename.Focus();
            return;
        }
        if (txtexpiryDate.Text == "")
        {
            txtexpiryDate.Text = "1/1/1900";
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtexpiryDate.Text.Trim());
            }
            catch
            {
                txtexpiryDate.Text = "";
                DisplayMessage("Enter Valid Expiry Date");
                txtexpiryDate.Focus();
                return;
            }
        }
        if (txtEmpName.Text.Contains("/"))
        {
            //Driverid = txtEmpName.Text.Split('/')[1].ToString();
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
           string Emp_Code = txtEmpName.Text.Split('/')[1].ToString();
            Driverid = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            if (Driverid == "0" || Driverid == "")
            {
                DisplayMessage("Employee not exists");
                txtEmpName.Focus();
                txtEmpName.Text = "";
                return;
            }            
            //Driverid = txtEmpName.Text.Split('/')[1].ToString();
            DriVerName = txtEmpName.Text.Split('/')[0].ToString();
        }
        else
        {
            Driverid = "0";
            DriVerName = txtEmpName.Text.Trim();
        }
        if (ddlVehicleType.SelectedValue.ToString() == "Own")
        {
            if (txtProductName.Text != "")
            {
                ProductId = txtProductName.Text.Split('/')[1].ToString();
            }
            else
            {
                ProductId = "0";
            }
        }
        else
        {
            ProductId = "0";
        }

        if (txtServiceDueDate.Text == "")
        {
            txtServiceDueDate.Text = "1/1/1900";
        }
        if (txtServiceDueReading.Text == "")
        {
            txtServiceDueReading.Text = "0";
        }
        if (Edit_Id.Value == "")
        {
            b = objVehicleMaster.InsertRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ProductId, txtpvcexpirydate.Text, txtownername.Text, txtRemarks.Text, "", txtvehiclename.Text.Trim(), txtplateno.Text.Trim(), txtModelNo.Text, txtregistrationNo.Text, txtexpiryDate.Text, Driverid, ddlVehicleType.SelectedValue.ToString(), DriVerName, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtServiceDueDate.Text, txtServiceDueReading.Text);
            if (b != 0)
            {
                SaveOpeningBalace(b);
                DisplayMessage("Record Saved","green");
                Fill_Grid_List();
                Reset();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            b = objVehicleMaster.UpdateRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Edit_Id.Value, ProductId, txtpvcexpirydate.Text, txtownername.Text, txtRemarks.Text, "", txtvehiclename.Text.Trim(), txtplateno.Text, txtModelNo.Text, txtregistrationNo.Text, txtexpiryDate.Text, Driverid, ddlVehicleType.SelectedValue.ToString(), DriVerName, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtServiceDueDate.Text, txtServiceDueReading.Text);
            if (b != 0)
            {
                SaveOpeningBalace(int.Parse(Edit_Id.Value.ToString()));                
                DisplayMessage("Record Updated", "green");
                Fill_Grid_List();
                Reset();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
    }

    protected void SaveOpeningBalace(int VehicleId)
    {
        string OtherAccountId = VehicleId.ToString();

        string VehicleAccountId = Ac_ParameterMaster.GetVehicleAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        int DebAmt = int.Parse(txtOpeningDebitAmt.Text.ToString());
        int CreAmt = int.Parse(txtOpeningCreditAmt.Text.ToString());
        if (VehicleAccountId != "0" && (DebAmt > 0 || CreAmt > 0))
        {
            string VehicleQuery = "Select COUNT(*) Count,ISNULL(Trans_Id,0) Id from Ac_SubChartOfAccount where Company_Id = " + Session["CompId"].ToString() + " and Brand_Id = " + Session["BrandId"].ToString() + " and Location_Id = " + Session["LocId"].ToString() + " and FinancialYearId = " + Session["FinanceYearId"].ToString() + " and AccTransId = " + VehicleAccountId + " and Other_Account_No = " + VehicleId.ToString() + " group by Trans_Id";
            DataTable VehicleDt = objDa.return_DataTable(VehicleQuery);
            int VehicleCount = 0;
            string TransId = "0";
            if (VehicleDt != null && VehicleDt.Rows.Count > 0)
            {
                VehicleCount = int.Parse(VehicleDt.Rows[0][0].ToString());
                TransId = VehicleDt.Rows[0][1].ToString();
            }
            string CurrencyId = Session["LocCurrencyId"].ToString();
            double CompanyDrAmnt = 0;
            double CompanyCrAmnt = 0;
            double.TryParse(objAcParameter.GetCompanyCurrency(Session["CurrencyId"].ToString(), txtOpeningDebitAmt.Text.ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), out CompanyDrAmnt);
            double.TryParse(objAcParameter.GetCompanyCurrency(Session["CurrencyId"].ToString(), txtOpeningCreditAmt.Text.ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), out CompanyCrAmnt);

            string CompanyDrAmt = CompanyDrAmnt.ToString();
            string CompanyCrAmt = CompanyCrAmnt.ToString();

            if (VehicleCount == 0)
            {
                int vehicleinsert = objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), VehicleAccountId, OtherAccountId, txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, CurrencyId, CompanyDrAmt, CompanyCrAmt, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else if (VehicleCount == 1)
            {
                int vehicleupdate = objSubCOA.UpdateSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), TransId, VehicleAccountId, txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, CurrencyId, CompanyDrAmt, CompanyCrAmt, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
    }

    protected void Reset()
    {
        try
        {
            Edit_Id.Value = "";
            txtValue.Text = "";
            ddlOption.SelectedIndex = 2;
            ddlFieldName.SelectedIndex = 0;
            txtvehiclename.Text = "";
            txtplateno.Text = "";
            txtModelNo.Text = "";
            txtregistrationNo.Text = "";
            txtexpiryDate.Text = "";
            txtEmpName.Text = "";
            txtProductName.Text = "";
            txtvehiclename.Focus();
            CE_Search_Date.Format = objSys.SetDateFormat();
            CE_Expiry_Date.Format = objSys.SetDateFormat();
            CE_PUC_Expiry_Date.Format = objSys.SetDateFormat();
            CE_Service_Due_Date.Format = objSys.SetDateFormat();
            CE_Search_Date_Bin.Format = objSys.SetDateFormat();
            txtRemarks.Text = string.Empty;
            txtpvcexpirydate.Text = string.Empty;
            txtServiceDueDate.Text = string.Empty;
            txtServiceDueReading.Text = string.Empty;
            txtexpiryDate.Text = "";
            txtownername.Text = "";
            txtOpeningCreditAmt.Text = string.Empty;
            txtOpeningDebitAmt.Text = string.Empty;
            ddlVehicleType.SelectedValue = "Own";
            ddlVehicleType_OnSelectedIndexChanged(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.New;
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
        }
        catch
        {
        }
    }
    
    public string GetDate(object obj)
    {
        string strNewDate = string.Empty;
        if (obj.ToString() != "" && obj.ToString() != "1/1/1900 12:00:00 AM")
        {
            strNewDate = Convert.ToDateTime(obj.ToString()).ToString(objSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVehicleName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        Prj_VehicleMaster objVehicleMaster = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Name"].ToString();
        }
        return txt;
    }

    protected void ddlVehicleType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string typename = ddlVehicleType.SelectedValue.ToString();
        if (typename == "Own")
        {
            lblProductName.Visible = true;
            txtProductName.Visible = true;

            CompanyMaster companyMaster = new CompanyMaster(Session["DBConnection"].ToString());
            DataTable companydt = companyMaster.GetCompanyMaster();
            txtownername.Text = companydt.Rows[0]["Company_Name"].ToString();
        }
        else
        {
            lblProductName.Visible = false;
            txtProductName.Visible = false;
            txtownername.Text = string.Empty;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListOwnerName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        Prj_VehicleMaster ObjVehicle = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjVehicle.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        dt = new DataView(dt, "Owner like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        dt = dt.DefaultView.ToTable(true, "Owner");
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Owner"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        EmployeeMaster objemployeemaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objemployeemaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        try
        {
            dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LOcId"].ToString() + "'  and (Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString();
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());        
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["EProductName"].ToString() + "/" + dt.Rows[i]["ProductId"].ToString();
        }
        return txt;
    }

    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        string productname = string.Empty;
        Inv_ProductMaster objProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        if (txtProductName.Text != "")
        {
            try
            {
                productname = txtProductName.Text.Split('/')[0].ToString();
            }
            catch
            {
                DisplayMessage("Product Not Exists");
                txtProductName.Text = "";
                txtProductName.Focus();
                return;
            }

            DataTable dtEmp = objProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), productname);
            if (dtEmp != null)
            {
                if (dtEmp.Rows.Count == 0)
                {
                    DisplayMessage("Product Not Exists");
                    txtProductName.Text = "";
                    txtProductName.Focus();
                    return;
                }
            }
            else
            {
                DisplayMessage("Product Not Exists");
                txtProductName.Text = "";
                txtProductName.Focus();
                return;
            }
        }
    }
    
    protected void txtvehiclename_TextChanged(object sender, EventArgs e)
    {
        if (txtvehiclename.Text.Trim() != "")
        {
            if (Edit_Id.Value == "")
            {
                DataTable dt = Vehicle_Master.Get_Vehicle_Master(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", "True", "5", Session["UserID"].ToString());
                try
                {
                    dt = new DataView(dt, "Name='" + txtvehiclename.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dt.Rows.Count > 0)
                {
                    DisplayMessage("Vehicle Name is Already Exists");
                    txtvehiclename.Text = "";
                    txtvehiclename.Focus();
                }
                DataTable dt1 = Vehicle_Master.Get_Vehicle_Master(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", "False", "5", Session["UserID"].ToString());
                try
                {
                    dt1 = new DataView(dt1, "Name='" + txtvehiclename.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dt1.Rows.Count > 0)
                {

                    DisplayMessage("Vehicle Name Already Exists in Bin Section");
                    txtvehiclename.Text = "";
                    txtvehiclename.Focus();
                    return;
                }
            }
            else
            {
                DataTable dtTemp = objVehicleMaster.GetRecord_By_VehicleId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Edit_Id.Value);
                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["Name"].ToString() != txtvehiclename.Text)
                    {
                        DataTable dt = Vehicle_Master.Get_Vehicle_Master(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", "True", "5", Session["UserID"].ToString());
                        try
                        {
                            dt = new DataView(dt, "Name='" + txtvehiclename.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {
                        }
                        if (dt.Rows.Count > 0)
                        {
                            DisplayMessage("Vehicle Name is Already Exists");
                            txtvehiclename.Text = "";
                            txtvehiclename.Focus();
                            return;
                        }
                        DataTable dt1 = Vehicle_Master.Get_Vehicle_Master(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", "False", "5", Session["UserID"].ToString());
                        try
                        {
                            dt1 = new DataView(dt1, "Name='" + txtvehiclename.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {
                        }
                        dt1 = new DataView(dt1, "Name='" + txtvehiclename.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                            DisplayMessage("Vehicle Name Already Exists in Bin Section");
                            txtvehiclename.Text = "";
                            txtvehiclename.Focus();
                        }
                    }
                }
            }
        }
        txtplateno.Focus();
    }


    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["VihicleM_Dt_Vehicle_Master_List_Active"] != null)
        {
            DataTable dt_Filter = Session["VihicleM_Dt_Vehicle_Master_List_Active"] as DataTable;
            if (ddlFilter.SelectedValue == "Select")
            {
                btnRefreshBin_Click(null, null);
            }
            else if (ddlFilter.SelectedValue == "Insurance")
            {
                DataTable Dt_Vehicle = new DataView(dt_Filter, "Expiry_Date < #" + DateTime.Now.ToString() + "#", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Vehicle.Rows.Count > 0)
                {
                    Fill_Gv_Vehicle_Master(Dt_Vehicle);
                }
                else
                {
                    Fill_Gv_Vehicle_Master(Dt_Vehicle);
                }
            }
            else if (ddlFilter.SelectedValue == "PUC")
            {
                DataTable Dt_Vehicle = new DataView(dt_Filter, "Pvc_Expiry_Date < #" + DateTime.Now.ToString() + "#", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Vehicle.Rows.Count > 0)
                {
                    Fill_Gv_Vehicle_Master(Dt_Vehicle);
                }
                else
                {
                    Fill_Gv_Vehicle_Master(Dt_Vehicle);
                }
            }
            else if (ddlFilter.SelectedValue == "Service")
            {
                DataTable Dt_Vehicle = new DataView(dt_Filter, "ServiceDueDate < #" + DateTime.Now.ToString() + "#", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Vehicle.Rows.Count > 0)
                {
                    Fill_Gv_Vehicle_Master(Dt_Vehicle);
                }
                else
                {
                    Fill_Gv_Vehicle_Master(Dt_Vehicle);
                }
            }
        }
    }
    //--------------- End New ---------------
}