using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

public partial class Transport_VehicleContract : BasePage
{

    tp_Vehicle_Contract ObjVehicleContract = null;
    EmployeeMaster objEmployee = null;
    DataAccessClass objda = null;
    Common ObjComman = null;
    SystemParameter ObjSys = null;
    LocationMaster objLocation = null;
    Set_DocNumber objDocNo = null;
    Inv_UnitMaster ObjUnit = null;
    Prj_VehicleMaster objVehicleMaster = null;
    Ems_ContactMaster objContact = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CompId"] == null || Session["BrandId"] == null || Session["LocId"] == null || Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjVehicleContract = new tp_Vehicle_Contract(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Btn_Save.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(Btn_Save, "").ToString());
        //btn_Post.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btn_Post, "").ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../Transport/VehicleContract.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            FillGrid();
            txtContractNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = GetDocumentNumber();
            //AllPageCode();
            FillUnit();
            Calender.Format = ObjSys.SetDateFormat();
            txttrnDate.Text = DateTime.Now.ToString(ObjSys.SetDateFormat());
            CalendarExtender_txtFromdate.Format = ObjSys.SetDateFormat();
            CalendarExtender_txtToDate.Format = ObjSys.SetDateFormat();
        }
    }
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "172", "351", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Save.Visible = clsPagePermission.bAdd;
        btn_Post.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    protected void Btn_Bin_Click(object sender, EventArgs e)
    {
        FillGridBin();
    }
    protected void Btn_Post_Click(object sender, EventArgs e)
    {
        Btn_Save_Click(sender, e);
        //AllPageCode();
    }
    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        bool IsPost = false;

        string strInqty = string.Empty;
        string strOutQty = string.Empty;
        string strsql = string.Empty;

        DataTable dtContract = new DataTable();

        if (((Button)sender).ID.Trim() == "btn_Post")
        {
            IsPost = true;
        }

        string strUnitId = "0";
        string strWeightUnitId = "0";

        if (ddlUnit.SelectedIndex > 0)
        {
            strUnitId = ddlUnit.SelectedValue;
        }
        if (ddlWeightUnit.SelectedIndex > 0)
        {
            strWeightUnitId = ddlWeightUnit.SelectedValue;
        }

        if (hdnEditId.Value == "")
        {
            strsql = "select IsActive from tp_Vehicle_contract where Vehicle_Id='" + txtvehiclename.Text.Split('/')[1].ToString() + "' and (('" + Convert.ToDateTime(txtFromdate.Text) + "' between From_Date And To_Date) or ('" + Convert.ToDateTime(txtToDate.Text) + "' between From_Date And To_Date))";
        }
        else
        {
            strsql = "select IsActive from tp_Vehicle_contract where Vehicle_Id='" + txtvehiclename.Text.Split('/')[1].ToString() + "' and Trans_Id<>" + hdnEditId.Value + " and (('" + Convert.ToDateTime(txtFromdate.Text) + "' between From_Date And To_Date) or ('" + Convert.ToDateTime(txtToDate.Text) + "' between From_Date And To_Date))";
        }

        dtContract = objda.return_DataTable(strsql);

        if (dtContract.Rows.Count > 0)
        {
            if (dtContract.Rows[0]["IsActive"].ToString() == "True")
            {
                DisplayMessage("Vehicle is already used in another contract");
                return;
            }

            if (dtContract.Rows[0]["IsActive"].ToString() == "False")
            {
                DisplayMessage("Vehicle is already used in another contract,  check in bin section");
                return;
            }
        }


        //here we are checking that in qty should not be greater then out qty


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {
            if (hdnEditId.Value == "")
            {
                ObjVehicleContract.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ViewState["DocNo"].ToString() + objda.return_DataTable("select COUNT( *)+1 from tp_Vehicle_Contract where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " ", ref trns).Rows[0][0].ToString(), txtvehiclename.Text.Split('/')[1].ToString(), txtContractorName.Text.Split('/')[1].ToString(), ObjSys.getDateForInput(txtFromdate.Text).ToString(), ObjSys.getDateForInput(txtToDate.Text).ToString(), txtOrderQty.Text, strUnitId, txtrate.Text, txtWeight.Text, strWeightUnitId, ddlRateType.SelectedValue, txtExcessuseRate.Text, txtMilage.Text, ddlContractType.SelectedValue, txtRemarks.Text, IsPost.ToString(), txtvehiclename.Text.Split('/')[0].ToString(), txtContractorName.Text.Split('/')[0].ToString(), txtCurrentREading.Text, "", false.ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);

            }
            else
            {
                ObjVehicleContract.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEditId.Value, txtContractNo.Text, txtvehiclename.Text.Split('/')[1].ToString(), txtContractorName.Text.Split('/')[1].ToString(), ObjSys.getDateForInput(txtFromdate.Text).ToString(), ObjSys.getDateForInput(txtToDate.Text).ToString(), txtOrderQty.Text, strUnitId, txtrate.Text, txtWeight.Text, strWeightUnitId, ddlRateType.SelectedValue, txtExcessuseRate.Text, txtMilage.Text, ddlContractType.SelectedValue, txtRemarks.Text, IsPost.ToString(), txtvehiclename.Text.Split('/')[0].ToString(), txtContractorName.Text.Split('/')[0].ToString(), txtCurrentREading.Text, "", false.ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);

            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();


            if (hdnEditId.Value == "")
            {
                if (((Button)sender).ID.Trim() == "btn_Post")
                {
                    DisplayMessage("Record posted successfully");
                }
                else
                {

                    DisplayMessage("Record Saved Successfully","green");
                }

            }
            else
            {
                if (((Button)sender).ID.Trim() == "btn_Post")
                {
                    DisplayMessage("Record posted successfully");
                }
                else
                {

                    DisplayMessage("Record Updated Successfully", "green");
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);                
            }
            Reset();
            FillGrid();
        }
        catch (Exception ex)
        {

            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;

        }


    }
    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void Btn_Reset_Click(object sender, EventArgs e)
    {
        Reset();

    }
    public void Reset()
    {

        hdnEditId.Value = "";
        txtContractNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = GetDocumentNumber();
        txttrnDate.Text = DateTime.Now.ToString(ObjSys.SetDateFormat());
        txtvehiclename.Text = "";
        txtContractorName.Text = "";
        txtFromdate.Text = "";
        txtToDate.Text = "";
        ddlUnit.SelectedIndex = 0;
        txtOrderQty.Text = "0";
        txtrate.Text = "0";
        ddlWeightUnit.SelectedIndex = 0;
        ddlRateType.SelectedIndex = 0;
        txtWeight.Text = "0";
        txtExcessuseRate.Text = "0";
        txtMilage.Text = "0";
        txtRemarks.Text = "";
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        Session["CHECKED_ITEMS"] = null;
        txtCurrentREading.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        //AllPageCode();
    }
    #region List
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
    private void FillGrid()
    {
        DataTable dtBrand = ObjVehicleContract.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        if (ddlPosted.SelectedIndex == 0)
        {
            dtBrand = new DataView(dtBrand, "Is_Post='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlPosted.SelectedIndex == 1)
        {
            dtBrand = new DataView(dtBrand, "Is_Post='False'", "", DataViewRowState.CurrentRows).ToTable();

        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtContract"] = dtBrand;
        Session["dtContractFilter"] = dtBrand;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dtBrand, "", "");
        //AllPageCode();

    }
    protected void GvsalaryPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvsalaryPlan.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtContractFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
        //AllPageCode();
        GvsalaryPlan.HeaderRow.Focus();
    }
    protected void GvsalaryPlan_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtContractFilter"];
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
        Session["dtContractFilter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
        //AllPageCode();
        GvsalaryPlan.HeaderRow.Focus();
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
            DataTable dtDeduction = (DataTable)Session["dtContract"];
            DataView view = new DataView(dtDeduction, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvsalaryPlan, view.ToTable(), "", "");
            Session["dtContractFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = ObjVehicleContract.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["Is_Post"].ToString() == "True" && ((ImageButton)sender).ID == "btnEdit")
            {
                DisplayMessage("Record Posted , you can not edit");
                return;

            }

            ddlRateType.SelectedValue = dt.Rows[0]["Rate_Type"].ToString().Trim();
            ddlRateType_OnSelectedIndexChanged(null, null);

            hdnEditId.Value = e.CommandArgument.ToString();
            txttrnDate.Text = Convert.ToDateTime(dt.Rows[0]["Field7"].ToString()).ToString(ObjSys.SetDateFormat());
            txtContractNo.Text = dt.Rows[0]["Contract_No"].ToString();
            txtvehiclename.Text = dt.Rows[0]["Field2"].ToString() + "/" + dt.Rows[0]["Vehicle_Id"].ToString();
            txtContractorName.Text = dt.Rows[0]["Field3"].ToString() + "/" + dt.Rows[0]["Contractor_Id"].ToString();
            txtFromdate.Text = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString()).ToString(ObjSys.SetDateFormat());
            txtToDate.Text = Convert.ToDateTime(dt.Rows[0]["To_Date"].ToString()).ToString(ObjSys.SetDateFormat());
            txtOrderQty.Text = Common.GetAmountDecimal(dt.Rows[0]["Qty"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            try
            {
                ddlUnit.SelectedValue = dt.Rows[0]["Unit"].ToString();
            }
            catch
            {
                ddlUnit.SelectedIndex = 0;
            }

            txtrate.Text = Common.GetAmountDecimal(dt.Rows[0]["Rate"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtWeight.Text = Common.GetAmountDecimal(dt.Rows[0]["Weight"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            try
            {
                ddlWeightUnit.SelectedValue = dt.Rows[0]["Weight_Unit"].ToString();
            }
            catch
            {
                ddlWeightUnit.SelectedIndex = 0;
            }



            txtExcessuseRate.Text = Common.GetAmountDecimal(dt.Rows[0]["Excess_Use_rate"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtMilage.Text = Common.GetAmountDecimal(dt.Rows[0]["Millage"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ddlContractType.SelectedValue = dt.Rows[0]["Contract_Type"].ToString().Trim();
            txtRemarks.Text = dt.Rows[0]["Remark"].ToString();
            txtCurrentREading.Text = dt.Rows[0]["Field4"].ToString();

            if (((ImageButton)sender).ID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
            }
            else
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            //Txt_Plan_Name.Focus();
        }
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = 0;

        DataTable dt = new DataTable();

        dt = ObjVehicleContract.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {


            if (dt.Rows[0]["Is_Post"].ToString() == "True")
            {
                DisplayMessage("Record Posted , you can not delete");
                return;

            }
        }


        b = ObjVehicleContract.RestoreRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
    #endregion
    #region Bin
    public void FillGridBin()
    {

        DataTable dt = new DataTable();
        dt = ObjVehicleContract.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");
        Session["dtBinDeduction"] = dt;
        Session["dtBinFilter"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        Session["CHECKED_ITEMS"] = null;
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
        }
        else
        {
            //AllPageCode();
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
            DataTable dtCust = (DataTable)Session["dtBinDeduction"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvsalaryPlanBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
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

        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        int Msg = 0;
        if (GvsalaryPlanBin.Rows.Count != 0)
        {
            SaveCheckedValues();
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Please Select Record");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        Msg = ObjVehicleContract.RestoreRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }

                    if (Msg != 0)
                    {
                        FillGrid();
                        FillGridBin();
                        ViewState["Select"] = null;
                        lblSelectedRecord.Text = "";
                        DisplayMessage("Record Activated");
                        Session["CHECKED_ITEMS"] = null;
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
                return;
            }
        }
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
            {
                int index = (int)GvsalaryPlanBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
        {
            index = (int)GvsalaryPlanBin.DataKeys[gvrow.RowIndex].Value;
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
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        DataTable dtDEPARTMENT = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtDEPARTMENT.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Trans_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Trans_Id"]));

            }
            foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvsalaryPlanBin, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvsalaryPlanBin.HeaderRow.FindControl("chkgvSelectAll"));
        bool result = false;
        if (chkSelAll.Checked == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvsalaryPlanBin.Rows)
        {
            index = (int)GvsalaryPlanBin.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];


            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void GvsalaryPlanBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        GvsalaryPlanBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");

        PopulateCheckedValues();
        //AllPageCode();

    }
    protected void GvsalaryPlanBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        SaveCheckedValues();
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtbinFilter"];
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");

        //AllPageCode();
        PopulateCheckedValues();
    }
    #endregion
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "1/1/1900 12:00:00 AM")
        {
            strNewDate = DateTime.Parse(strDate).ToString(ObjSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
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
    public void FillUnit()
    {
        DataTable dtUnit = ObjUnit.GetUnitMaster(Session["CompId"].ToString());
        objPageCmn.FillData((object)ddlUnit, dtUnit, "Unit_Name", "Unit_Id");
        objPageCmn.FillData((object)ddlWeightUnit, dtUnit, "Unit_Name", "Unit_Id");
        dtUnit.Dispose();
    }
    #region Vehicle
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVehicleName(string prefixText, int count, string contextKey)
    {
        Prj_VehicleMaster objVehicleMaster = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Vehicle_Id"].ToString();
        }
        return txt;
    }
    protected void txtvehiclename_TextChanged(object sender, EventArgs e)
    {
        if (txtvehiclename.Text.Trim() != "")
        {

            DataTable dt = objVehicleMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                dt = new DataView(dt, "Name='" + txtvehiclename.Text.Trim().Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Vehicle not found");
                txtvehiclename.Text = "";
                txtvehiclename.Focus();
                return;
            }
            else
            {
                txtContractorName.Focus();
            }
        }
        txtContractorName.Focus();
    }
    #endregion
    #region Contact

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = ObjContactMaster.GetContactTrueAllData();
        try
        {
            dtCustomer = new DataView(dtCustomer, "Field6='True'  and (Status='Company' or Is_Reseller='True')", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
            dtCustomer = ObjContactMaster.GetContactTrueAllData();
        }
        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Filtertext like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        string strContactId = "0";
        if (txtContractorName.Text != "")
        {
            string[] CustomerName = txtContractorName.Text.Split('/');
            DataTable DtCustomer = objContact.GetContactByContactName(CustomerName[0].ToString().Trim());
            if (DtCustomer.Rows.Count > 0)
            {
                strContactId = CustomerName[1].ToString().Trim();
                txtFromdate.Focus();
            }
            else
            {
                DisplayMessage("Enter Customer Name in suggestion Only");
                txtContractorName.Text = "";
                txtContractorName.Focus();
            }
        }
    }
    #endregion
    protected void ddlRateType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        RequiredFieldValidator_ddlWeightUnit.Enabled = false;
        RequiredFieldValidator_txtWeight.Enabled = false;
        RequiredFieldValidator_ddlUnit.Enabled = false;
        tr_ddlUnit.Visible = false;
        if (ddlRateType.SelectedIndex == 0)
        {
            txtOrderQty.Text = "1";
            tr_Weight.Visible = false;
            txtWeight.Text = "0";
            tr_qty.Visible = false;
            txtExcessuseRate.Text = "0";
            tr_ExcessUseRate.Visible = false;

        }
        else if (ddlRateType.SelectedIndex == 1)
        {
            txtOrderQty.Text = "";
            tr_Weight.Visible = false;
            txtWeight.Text = "0";
            tr_qty.Visible = true;
            txtExcessuseRate.Text = "";
            tr_ExcessUseRate.Visible = true;
            tr_ddlUnit.Visible = true;
            RequiredFieldValidator_ddlUnit.Enabled = true;
        }
        else
        {
            txtOrderQty.Text = "";
            tr_qty.Visible = true;
            txtWeight.Text = "";
            tr_Weight.Visible = true;
            txtExcessuseRate.Text = "0";
            tr_ExcessUseRate.Visible = false;
            RequiredFieldValidator_ddlWeightUnit.Enabled = true;
            RequiredFieldValidator_txtWeight.Enabled = true;
            RequiredFieldValidator_ddlUnit.Enabled = true;
            tr_ddlUnit.Visible = true;
        }

    }
}