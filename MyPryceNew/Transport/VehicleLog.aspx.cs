using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Collections;

public partial class Transport_VehicleLog : System.Web.UI.Page
{
    VehicleMaster Vehicle_Master = null;
    tp_Vehicle_Log ObjVehicleLog = null;
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

    public bool IsLogDate = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CompId"] == null || Session["BrandId"] == null || Session["LocId"] == null || Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        Vehicle_Master = new VehicleMaster(Session["DBConnection"].ToString());
        ObjVehicleLog = new tp_Vehicle_Log(Session["DBConnection"].ToString());
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
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../Transport/VehicleLog.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtWeight.Text = "0";
            FillGrid();
            FillUnit();
            Calender.Format = ObjSys.SetDateFormat();
            txttrnDate.Text = DateTime.Now.ToString(ObjSys.SetDateFormat());
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
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
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
        string strsql = string.Empty;
        string strDriverId = "0";
        string strDriverName = string.Empty;
        string strStartReading = string.Empty;
        string strEndReading = string.Empty;
        string strWeightUnit = string.Empty;
        string strWeight = string.Empty;


        //server side validation for check that same day multiple entry allow or not 


        if (ddlVehicleStatus.SelectedValue.Trim() == "Absent")
        {
            strDriverId = "0";
            strDriverName = "";
            strStartReading = txtStartReading.Text;
            strEndReading = txtEndReading.Text;
            strWeightUnit = "0";
            strWeight = "0";
        }
        else
        {
            if (txtdrivername.Text.Trim() != "")
            {
                try
                {
                    strDriverId = txtdrivername.Text.Trim().Split('/')[1].ToString();
                }
                catch
                {
                    strDriverId = "0";
                }


                try
                {
                    strDriverName = txtdrivername.Text.Trim().Split('/')[0].ToString();
                }
                catch
                {
                    strDriverName = txtdrivername.Text.Trim();
                }
            }


            strStartReading = txtStartReading.Text;
            strEndReading = txtEndReading.Text;
            if (ddlWeightUnit.SelectedValue == "--Select--")
            {
                strWeightUnit = "0";
            }
            else
            {
                strWeightUnit = ddlWeightUnit.SelectedValue;
            }
            strWeight = txtWeight.Text;

        }



        if (float.Parse(strStartReading) > float.Parse(strEndReading))
        {
            DisplayMessage("End Reading should be greater then or equal to start reading");
            txtStartReading.Focus();
            return;
        }

        if (((Button)sender).ID.Trim() == "btn_Post")
        {
            IsPost = true;
        }

        //here we are checking that single vehicle should not signed another contract between specific date range

        DataTable dtContract = new DataTable();
        if (hdnEditId.Value == "")
        {
            strsql = "select rate_type from tp_Vehicle_Contract inner join tp_Vehicle_Log on tp_Vehicle_Contract.Vehicle_Id = tp_Vehicle_Log.Vehicle_Id and ('" + ObjSys.getDateForInput(txttrnDate.Text) + "' between tp_Vehicle_Contract.From_Date and tp_Vehicle_Contract.To_Date) and tp_Vehicle_Contract.Vehicle_Id='" + txtvehiclename.Text.Split('/')[1].ToString() + "' and tp_Vehicle_Log.Trans_date='" + ObjSys.getDateForInput(txttrnDate.Text) + "' and tp_Vehicle_Contract.Rate_Type in (0,1) and tp_Vehicle_Contract.Field1='True' and tp_Vehicle_Log.IsActive='True'";
        }
        else
        {
            strsql = "select rate_type from tp_Vehicle_Contract inner join tp_Vehicle_Log on tp_Vehicle_Contract.Vehicle_Id = tp_Vehicle_Log.Vehicle_Id and ('" + ObjSys.getDateForInput(txttrnDate.Text) + "' between tp_Vehicle_Contract.From_Date and tp_Vehicle_Contract.To_Date) and tp_Vehicle_Contract.Vehicle_Id='" + txtvehiclename.Text.Split('/')[1].ToString() + "' and tp_Vehicle_Log.Trans_date='" + ObjSys.getDateForInput(txttrnDate.Text) + "' and tp_Vehicle_Contract.Rate_Type in (0,1) and tp_Vehicle_Contract.Field1='True' and tp_Vehicle_Log.Trans_Id<>" + hdnEditId.Value + " and  tp_Vehicle_Log.IsActive='True'";
        }

        dtContract = objda.return_DataTable(strsql);

        if (dtContract.Rows.Count > 0)
        {
            DisplayMessage("Log Already exists for selected date");
            return;
        }
        //here we are checking that in qty should not be greater then out qty


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        Btn_Save.Enabled = false;
        try
        {
            if (hdnEditId.Value == "")
            {

                ObjVehicleLog.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtvehiclename.Text.Split('/')[1].ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), strDriverId, strDriverName, strStartReading, strEndReading, txtRemarks.Text, dddlShift.SelectedValue, IsPost.ToString(), strWeightUnit, strWeight, ddlVehicleStatus.SelectedValue.Trim(), "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);
                //ObjVehicleContract.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ViewState["DocNo"].ToString() + objda.return_DataTable("select COUNT( *)+1 from tp_Vehicle_Contract where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " ", ref trns).Rows[0][0].ToString(), txtvehiclename.Text.Split('/')[1].ToString(), txtContractorName.Text.Split('/')[1].ToString(), ObjSys.getDateForInput(txtFromdate.Text).ToString(), ObjSys.getDateForInput(txtToDate.Text).ToString(), txtOrderQty.Text, ddlUnit.SelectedValue, txtrate.Text, txtWeight.Text, ddlWeightUnit.SelectedValue, ddlRateType.SelectedValue, txtExcessuseRate.Text, txtMilage.Text, ddlContractType.SelectedValue, txtRemarks.Text, IsPost.ToString(), txtvehiclename.Text.Split('/')[0].ToString(), txtContractorName.Text.Split('/')[0].ToString(), "", "", false.ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);

            }
            else
            {
                ObjVehicleLog.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEditId.Value, txtvehiclename.Text.Split('/')[1].ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), strDriverId, strDriverName, strStartReading, strEndReading, txtRemarks.Text, dddlShift.SelectedValue, IsPost.ToString(), strWeightUnit, strWeight, ddlVehicleStatus.SelectedValue.Trim(), "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);

                // ObjVehicleContract.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEditId.Value, txtContractNo.Text, txtvehiclename.Text.Split('/')[1].ToString(), txtContractorName.Text.Split('/')[1].ToString(), ObjSys.getDateForInput(txtFromdate.Text).ToString(), ObjSys.getDateForInput(txtToDate.Text).ToString(), txtOrderQty.Text, ddlUnit.SelectedValue, txtrate.Text, txtWeight.Text, ddlWeightUnit.SelectedValue, ddlRateType.SelectedValue, txtExcessuseRate.Text, txtMilage.Text, ddlContractType.SelectedValue, txtRemarks.Text, IsPost.ToString(), txtvehiclename.Text.Split('/')[0].ToString(), txtContractorName.Text.Split('/')[0].ToString(), "", "", false.ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);

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
            Btn_Save.Enabled = true;
            Reset();
            FillGrid();

        }
        catch (Exception ex)
        {
            Btn_Save.Enabled = true;
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

        ViewState["DocNo"] = GetDocumentNumber();
        txttrnDate.Text = DateTime.Now.ToString(ObjSys.SetDateFormat());
        txtvehiclename.Text = "";
        txtdrivername.Text = "";
        txtStartReading.Text = "";
        txtEndReading.Text = "";
        ddlWeightUnit.SelectedIndex = 0;
        txtWeight.Text = "";
        txtRemarks.Text = "";
        lblSelectedRecord.Text = "";

        ViewState["Select"] = null;
        Session["CHECKED_ITEMS"] = null;


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
        DataTable dtBrand = ObjVehicleLog.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        if (ddlPosted.SelectedIndex == 0)
        {
            dtBrand = new DataView(dtBrand, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlPosted.SelectedIndex == 1)
        {
            dtBrand = new DataView(dtBrand, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();

        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtDeduction"] = dtBrand;
        Session["dtFilter_Vehicle_Log"] = dtBrand;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dtBrand, "", "");
        //AllPageCode();

    }
    protected void GvsalaryPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvsalaryPlan.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Vehicle_Log"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
        //AllPageCode();
        GvsalaryPlan.HeaderRow.Focus();
    }
    protected void GvsalaryPlan_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Vehicle_Log"];
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
        Session["dtFilter_Vehicle_Log"] = dt;
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
            if (ddlFieldName.SelectedValue == "Trans_date")
            {
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + ObjSys.getDateForInput(Txt_Log_Date.Text) + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + ObjSys.getDateForInput(Txt_Log_Date.Text) + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + ObjSys.getDateForInput(Txt_Log_Date.Text) + "%'";
                }
            }
            else
            {
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
            }

            DataTable dtDeduction = (DataTable)Session["dtDeduction"];
            DataView view = new DataView(dtDeduction, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvsalaryPlan, view.ToTable(), "", "");
            Session["dtFilter_Vehicle_Log"] = view.ToTable();
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
        Txt_Log_Date.Text = "";
        txtValue.Visible = true;
        Txt_Log_Date.Visible = false;
        txtValue.Focus();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = ObjVehicleLog.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["Field1"].ToString() == "True" && ((ImageButton)sender).ID == "btnEdit")
            {
                DisplayMessage("Record Posted , you can not edit");
                return;

            }
            hdnEditId.Value = e.CommandArgument.ToString();
            txttrnDate.Text = Convert.ToDateTime(dt.Rows[0]["Trans_date"].ToString()).ToString(ObjSys.SetDateFormat());
            txtvehiclename.Text = dt.Rows[0]["Vehicle_Name"].ToString().Trim() + "/" + dt.Rows[0]["Vehicle_Id"].ToString().Trim();
            if (dt.Rows[0]["Driver_Id"].ToString().Trim() != "0")
            {
                txtdrivername.Text = dt.Rows[0]["Driver_Name"].ToString().Trim() + "/" + dt.Rows[0]["Driver_Id"].ToString().Trim();
            }
            else
            {
                txtdrivername.Text = dt.Rows[0]["Driver_Name"].ToString().Trim();
            }


            try
            {
                ddlWeightUnit.SelectedValue = dt.Rows[0]["Field2"].ToString();
            }
            catch
            {
                ddlWeightUnit.SelectedIndex = 0;
            }

            txtWeight.Text = Common.GetAmountDecimal(dt.Rows[0]["Field3"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtRemarks.Text = dt.Rows[0]["Work_Detail"].ToString();
            dddlShift.SelectedValue = dt.Rows[0]["Shift"].ToString();
            ddlVehicleStatus.SelectedValue = dt.Rows[0]["Field4"].ToString().Trim();
            ddlVehicleStatus_OnSelectedIndexChanged(null, null);
            txtStartReading.Text = dt.Rows[0]["Start_Reading"].ToString();
            txtEndReading.Text = dt.Rows[0]["End_Reading"].ToString();
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

        dt = ObjVehicleLog.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {


            if (dt.Rows[0]["Field1"].ToString() == "True")
            {
                DisplayMessage("Record Posted , you can not delete");
                return;

            }
        }


        b = ObjVehicleLog.RestoreRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        dt = ObjVehicleLog.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
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

            if (ddlbinFieldName.SelectedValue == "Trans_date")
            {
                if (ddlbinOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + ObjSys.getDateForInput(Txt_Log_Date_Bin.Text) + "'";
                }
                else if (ddlbinOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + ObjSys.getDateForInput(Txt_Log_Date_Bin.Text) + "%'";
                }
                else
                {
                    condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + ObjSys.getDateForInput(Txt_Log_Date_Bin.Text) + "%'";
                }
            }
            else
            {
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
        lblSelectedRecord.Text = "";
        txtbinValue.Text = "";
        Txt_Log_Date_Bin.Text = "";
        txtbinValue.Visible = true;
        Txt_Log_Date_Bin.Visible = false;
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
                        Msg = ObjVehicleLog.RestoreRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
        DataTable dt = new DataTable();
        string strsql = string.Empty;
        txtStartReading.Text = "0";
        txtEndReading.Text = "0";
        txtdrivername.Text = "";
        if (txtvehiclename.Text.Trim() != "")
        {

            dt = objVehicleMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
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
            }
            else
            {

                if (dt.Rows[0]["Emp_id"].ToString() != "0")
                {
                    txtdrivername.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_Id"].ToString();
                }
                else
                {
                    txtdrivername.Text = dt.Rows[0]["Field2"].ToString();
                }
                strsql = "select DATEADD(DAY,1 ,Max(Trans_date)) as MaxDate from tp_Vehicle_Log where  Vehicle_Id= '" + txtvehiclename.Text.Trim().Split('/')[1].ToString() + "' and IsActive='True'";

                dt = objda.return_DataTable(strsql);

                if (dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "" && IsLogDate == false)
                {
                    txttrnDate.Text = Convert.ToDateTime(dt.Rows[0]["MaxDate"].ToString()).ToString(ObjSys.SetDateFormat());

                }
                DataTable Dt_Reading = Vehicle_Master.Get_Vehicle_Reading(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtvehiclename.Text.Trim().Split('/')[1].ToString(), ObjSys.getDateForInput(txttrnDate.Text.Trim()).ToString(), "True", "1", Session["UserId"].ToString());
                if (Dt_Reading != null)
                {

                    if (Dt_Reading.Rows.Count > 0)
                    {
                        if (Dt_Reading.Rows[0]["End_Reading"].ToString() == "" && Dt_Reading.Rows[0]["Start_Reading_Log"].ToString() == "")
                            txtStartReading.Text = Dt_Reading.Rows[0]["Start_Reading"].ToString();
                        else
                            txtStartReading.Text = Dt_Reading.Rows[0]["End_Reading"].ToString();

                        if (ddlVehicleStatus.SelectedIndex == 2)
                        {
                            if (Dt_Reading.Rows[0]["End_Reading"].ToString() == "" && Dt_Reading.Rows[0]["Start_Reading_Log"].ToString() == "")
                                txtEndReading.Text = Dt_Reading.Rows[0]["Start_Reading"].ToString();
                            else
                                txtEndReading.Text = Dt_Reading.Rows[0]["End_Reading"].ToString();
                        }
                        else
                        {
                            txtEndReading.Text = "0";
                        }
                    }
                    else
                    {
                        txtStartReading.Text = "0";
                    }
                }
                else
                {

                }
                //strsql = "select End_Reading  from tp_Vehicle_Log where Trans_date>='" + Convert.ToDateTime(txttrnDate.Text).AddDays(-1) + "' and Trans_date<='" + Convert.ToDateTime(txttrnDate.Text) + "' and Vehicle_Id= '" + txtvehiclename.Text.Trim().Split('/')[1].ToString() + "' and IsActive='True'";

                //dt = objda.return_DataTable(strsql);

                //if (dt.Rows.Count > 0)
                //{
                //    txtStartReading.Text = dt.Rows[0]["End_Reading"].ToString();

                //    if (ddlVehicleStatus.SelectedIndex == 2)
                //    {
                //        txtEndReading.Text = dt.Rows[0]["End_Reading"].ToString();
                //    }
                //}
                //else
                //{
                //    strsql = "select Contractor_Id,Field3,Field4,From_Date,To_Date from tp_Vehicle_contract where Vehicle_Id='" + txtvehiclename.Text.Split('/')[1].ToString() + "' and Contract_Type='In' and Field1='True' and ('" + Convert.ToDateTime(txttrnDate.Text) + "' between From_Date And To_Date)";

                //    dt = objda.return_DataTable(strsql);

                //    if (dt.Rows.Count > 0)
                //    {
                //        txtStartReading.Text = dt.Rows[0]["Field4"].ToString().Trim();

                //        if (ddlVehicleStatus.SelectedIndex == 2)
                //        {
                //            txtEndReading.Text = txtStartReading.Text;
                //        }
                //    }
                //}  

            }

            dt.Dispose();

        }

    }
    #endregion
    #region Driver

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDriverName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString();
        }
        return str;
    }

    #endregion
    protected void ddlVehicleStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtStartReading.Enabled = true;
        txtEndReading.Enabled = true;
        DataTable dt = new DataTable();
        string strsql = string.Empty;

        strsql = "select End_Reading  from tp_Vehicle_Log where Trans_date>='" + Convert.ToDateTime(txttrnDate.Text).AddDays(-1) + "' and Trans_date<='" + Convert.ToDateTime(txttrnDate.Text) + "' and Vehicle_Id= '" + txtvehiclename.Text.Trim().Split('/')[1].ToString() + "' and IsActive='True'";

        dt = objda.return_DataTable(strsql);

        RequiredFieldValidator_txtStartReading.Enabled = true;
        RequiredFieldValidator_txtEndReading.Enabled = true;
        //RequiredFieldValidator_ddlWeightUnit.Enabled = false;
        RequiredFieldValidator_txtWeight.Enabled = true;

        if (dt.Rows.Count > 0)
        {
            txtStartReading.Text = dt.Rows[0]["End_Reading"].ToString();
            txtEndReading.Text = "0";


        }

        if (ddlVehicleStatus.SelectedValue.Trim() == "Absent")
        {
            txtStartReading.Enabled = false;
            txtEndReading.Enabled = false;

            RequiredFieldValidator_txtStartReading.Enabled = false;
            RequiredFieldValidator_txtEndReading.Enabled = false;
            //RequiredFieldValidator_ddlWeightUnit.Enabled = false;
            RequiredFieldValidator_txtWeight.Enabled = false;


            if (dt.Rows.Count > 0)
            {
                txtStartReading.Text = dt.Rows[0]["End_Reading"].ToString();
                txtEndReading.Text = dt.Rows[0]["End_Reading"].ToString();
            }
        }

        if (ddlVehicleStatus.SelectedIndex == 2)
        {
            txtEndReading.Text = txtStartReading.Text;
        }
    }

    protected void txttrnDate_TextChanged(object sender, EventArgs e)
    {
        IsLogDate = true;
        txtvehiclename_TextChanged(null, null);
    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedValue == "Trans_date")
        {
            Txt_Log_Date.Visible = true;
            txtValue.Visible = false;
            Txt_Log_Date.Text = "";
            txtValue.Text = "";
        }
        else
        {
            Txt_Log_Date.Visible = false;
            txtValue.Visible = true;
            Txt_Log_Date.Text = "";
            txtValue.Text = "";
        }
    }

    protected void ddlbinFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinFieldName.SelectedValue == "Trans_date")
        {
            Txt_Log_Date_Bin.Visible = true;
            txtbinValue.Visible = false;
            Txt_Log_Date_Bin.Text = "";
            txtbinValue.Text = "";
        }
        else
        {
            Txt_Log_Date_Bin.Visible = false;
            txtbinValue.Visible = true;
            Txt_Log_Date_Bin.Text = "";
            txtbinValue.Text = "";
        }
    }
}