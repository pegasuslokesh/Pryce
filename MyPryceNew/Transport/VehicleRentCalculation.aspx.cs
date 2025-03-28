using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using PegasusDataAccess;
using System.Collections;

public partial class Transport_VehicleRentCalculation : System.Web.UI.Page
{
    tp_Vehicle_rent_trans objVehicleRent = null;
    EmployeeMaster objEmployee = null;
    DataAccessClass objda = null;
    Common ObjComman = null;
    SystemParameter ObjSys = null;
    LocationMaster objLocation = null;
    Inv_UnitMaster ObjUnit = null;
    Prj_VehicleMaster objVehicleMaster = null;
    Ems_ContactMaster objContact = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Set_DocNumber objDocNo = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    tp_Vehicle_Ledger ObjVehicleLedger = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CompId"] == null || Session["BrandId"] == null || Session["LocId"] == null || Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objVehicleRent = new tp_Vehicle_rent_trans(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        ObjVehicleLedger = new tp_Vehicle_Ledger(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
          
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../Transport/VehicleRentCalculation.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            FillGrid();
            //AllPageCode();
            Calender.Format = ObjSys.SetDateFormat();
            txttrnDate.Text = DateTime.Now.ToString(ObjSys.SetDateFormat());
            CustomValidator1.ValidationGroup = "";
            txtpaymentCreditaccount.Text = GetAccountNameByTransId(Ac_ParameterMaster.GetVehicleAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()));
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btn_Post.Visible = clsPagePermission.bAdd;
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
        double PaidTotaamount = 0;

        string Narration = "Vehicle rent payment for " + ddlToMonth.SelectedItem.Text + "-" + txttoYear.Text + " and Vehicle Name is ";

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        int TotalDaysInMonth = 0;
        try
        {
            TotalDaysInMonth = DateTime.DaysInMonth(Convert.ToInt32(txttoYear.Text), Convert.ToInt32(ddlToMonth.SelectedValue));
        }
        catch
        {
            DisplayMessage("Enter Valid Year");
            txttoYear.Focus();
            return;
        }


        foreach (GridViewRow gvrow in gvRentCalculation.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkgvselect")).Checked)
            {
                PaidTotaamount += Convert.ToDouble(((Label)gvrow.FindControl("lblNetAmount")).Text);
                Narration += " '" + ((Label)gvrow.FindControl("lblVehicleName")).Text + "' " + ",";

            }

        }


        DateTime DtFromdate = new DateTime();
        DateTime DtTodate = new DateTime();

        DtFromdate = new DateTime(Convert.ToInt32(txttoYear.Text), Convert.ToInt32(ddlToMonth.SelectedValue), 1);
        DtTodate = new DateTime(Convert.ToInt32(txttoYear.Text), Convert.ToInt32(ddlToMonth.SelectedValue), TotalDaysInMonth);

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {
            int MaxId = 0;


            if (PaidTotaamount != 0)
            {
                //For Bank Account
                string strAccountId = string.Empty;
                DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", ref trns);
                if (dtAccount.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAccount.Rows.Count; i++)
                    {
                        if (strAccountId == "")
                        {
                            strAccountId = dtAccount.Rows[i]["Param_Value"].ToString();
                        }
                        else
                        {
                            strAccountId = strAccountId + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                        }
                    }
                }
                else
                {
                    strAccountId = "0";
                }

                string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();


                //for Voucher Number
                string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                if (strVoucherNumber != "")
                {
                    int counter = objAcParameter.GetCounterforVoucherNumber1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "JV", Session["FinanceYearId"].ToString(), ref trns);


                    if (counter == 0)
                    {
                        strVoucherNumber = strVoucherNumber + "1";
                    }
                    else
                    {
                        strVoucherNumber = strVoucherNumber + (counter + 1);
                    }
                }

                MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", ObjSys.getDateForInput(txttrnDate.Text).ToString(), strVoucherNumber, ObjSys.getDateForInput(txttrnDate.Text).ToString(), "JV", "1/1/1800", "1/1/1800", "", Narration, strCurrencyId, "1", Narration, false.ToString(), false.ToString(), false.ToString(), "JV", "", "", "0", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strVMaxId = MaxId.ToString();


                string strCompAmount = PaidTotaamount.ToString();
                string strLocAmount = PaidTotaamount.ToString();
                string strForeignAmount = PaidTotaamount.ToString();
                string strForeignExchangerate = "1";


                //str for Employee Id
                //For Debit

                string strCompanyCrrValueDr = PaidTotaamount.ToString();
                string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "ID", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Vehicle Rent Payment", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "ID", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Vehicle Rent Payment", "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //For Credit
                string strCompanyCrrValueCr = strCompanyCrrValueDr;
                string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                foreach (GridViewRow gvrow in gvRentCalculation.Rows)
                {
                    if (((CheckBox)gvrow.FindControl("chkgvselect")).Checked && Convert.ToDouble(((Label)gvrow.FindControl("lblNetAmount")).Text) > 0)
                    {

                        if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
                        {
                            //objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), ((Label)gvrow.FindControl("lblContractorId")).Text, "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", ((Label)gvrow.FindControl("lblNetAmount")).Text, "Vehicle rent payment for " + ddlToMonth.SelectedItem.Text + "-" + txttoYear.Text + " and Vehicle Name is " + ((Label)gvrow.FindControl("lblVehicleName")).Text, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, ((Label)gvrow.FindControl("lblNetAmount")).Text, "0.00", ((Label)gvrow.FindControl("lblNetAmount")).Text, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), ((Label)gvrow.FindControl("lblvehicleId")).Text, "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", ((Label)gvrow.FindControl("lblNetAmount")).Text, "Vehicle rent payment for " + ddlToMonth.SelectedItem.Text + "-" + txttoYear.Text + " and Vehicle Name is " + ((Label)gvrow.FindControl("lblVehicleName")).Text, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, ((Label)gvrow.FindControl("lblNetAmount")).Text, "0.00", ((Label)gvrow.FindControl("lblNetAmount")).Text, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            //objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), ((Label)gvrow.FindControl("lblContractorId")).Text, "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", ((Label)gvrow.FindControl("lblNetAmount")).Text, "Vehicle rent payment for " + ddlToMonth.SelectedItem.Text + "-" + txttoYear.Text + " and Vehicle Name is " + ((Label)gvrow.FindControl("lblVehicleName")).Text, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, ((Label)gvrow.FindControl("lblNetAmount")).Text, "0.00", ((Label)gvrow.FindControl("lblNetAmount")).Text, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), ((Label)gvrow.FindControl("lblvehicleId")).Text, "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", ((Label)gvrow.FindControl("lblNetAmount")).Text, "Vehicle rent payment for " + ddlToMonth.SelectedItem.Text + "-" + txttoYear.Text + " and Vehicle Name is " + ((Label)gvrow.FindControl("lblVehicleName")).Text, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, ((Label)gvrow.FindControl("lblNetAmount")).Text, "0.00", ((Label)gvrow.FindControl("lblNetAmount")).Text, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }

                        objda.execute_Command("update tp_Vehicle_Log set Field1='True' where  trans_date between '" + DtFromdate.ToString() + "' and '" + DtTodate.ToString() + "' and Vehicle_Id = '" + ((Label)gvrow.FindControl("lblvehicleId")).Text + "' and IsActive='True'", ref trns);
                    }
                }

            }

            int refid = 0;
            foreach (GridViewRow gvrow in gvRentCalculation.Rows)
            {
                if (((CheckBox)gvrow.FindControl("chkgvselect")).Checked && Convert.ToDouble(((Label)gvrow.FindControl("lblNetAmount")).Text) > 0)
                {
                    refid = objVehicleRent.InsertRecord(Session["Compid"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((Label)gvrow.FindControl("lblvehicleId")).Text, ((Label)gvrow.FindControl("lblContractorId")).Text, ObjSys.getDateForInput(txttrnDate.Text).ToString(), DtFromdate.ToString(), DtTodate.ToString(), ((Label)gvrow.FindControl("lblrentAmount")).Text, ((Label)gvrow.FindControl("lblexcessAmount")).Text, ((Label)gvrow.FindControl("lbldeductionAmount")).Text, ((Label)gvrow.FindControl("lblNetAmount")).Text, MaxId.ToString(), ((Label)gvrow.FindControl("lblContractNo")).Text, ((Label)gvrow.FindControl("lblVehicleName")).Text, ((Label)gvrow.FindControl("lblContractorName")).Text, ddlToMonth.SelectedItem.Text, txttoYear.Text, false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    ObjVehicleLedger.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), ((Label)gvrow.FindControl("lblContractorId")).Text, ((Label)gvrow.FindControl("lblvehicleId")).Text, "Vehicle rent payment for " + ddlToMonth.SelectedItem.Text + "-" + txttoYear.Text + " and Vehicle Name is " + ((Label)gvrow.FindControl("lblVehicleName")).Text, "0", ((Label)gvrow.FindControl("lblNetAmount")).Text, ((Label)gvrow.FindControl("lblNetAmount")).Text, Session["LocCurrencyId"].ToString(), "1", "0", ((Label)gvrow.FindControl("lblNetAmount")).Text, "tp_Vehicle_rent_trans", refid.ToString(), MaxId.ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
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
        txttrnDate.Text = DateTime.Now.ToString(ObjSys.SetDateFormat());
        ddlToMonth.SelectedIndex = 0;
        txttoYear.Text = "";
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        Session["CHECKED_ITEMS"] = null;
        ddlToMonth.Enabled = true;
        txttoYear.Enabled = true;
        objPageCmn.FillData((object)gvRentCalculation, null, "", "");
        Lbl_Tab_New.Text = Resources.Attendance.New;
        CustomValidator1.ValidationGroup = "";
        btn_Post.Enabled = false;
        //AllPageCode();
    }
    #region List
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void FillGrid()
    {
        DataTable dtBrand = objVehicleRent.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());



        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtRentCalculation"] = dtBrand;
        Session["dtRentFilter"] = dtBrand;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dtBrand, "", "");
        //AllPageCode();

    }
    protected void GvsalaryPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvsalaryPlan.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtRentFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
        //AllPageCode();
        GvsalaryPlan.HeaderRow.Focus();
    }
    protected void GvsalaryPlan_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtRentFilter"];
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
        Session["dtRentFilter"] = dt;
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
            DataTable dtDeduction = (DataTable)Session["dtRentCalculation"];
            DataView view = new DataView(dtDeduction, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvsalaryPlan, view.ToTable(), "", "");
            Session["dtRentFilter"] = view.ToTable();
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

        DataTable dt = objVehicleRent.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["Is_Post"].ToString() == "True" && ((ImageButton)sender).ID == "btnEdit")
            {
                DisplayMessage("Record Posted , you can not edit");
                return;

            }
            hdnEditId.Value = e.CommandArgument.ToString();
            txttrnDate.Text = Convert.ToDateTime(dt.Rows[0]["Field7"].ToString()).ToString(ObjSys.SetDateFormat());

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

        dt = objVehicleRent.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {


            if (dt.Rows[0]["Is_Post"].ToString() == "True")
            {
                DisplayMessage("Record Posted , you can not delete");
                return;

            }
        }


        b = objVehicleRent.RestoreRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        dt = objVehicleRent.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlanBin, dt, "", "");
        Session["dtBinDeduction"] = dt;
        Session["dtBinFilter"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        Session["CHECKED_ITEMS"] = null;
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    protected void btnbinbind_Click(object sender, ImageClickEventArgs e)
    {
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
                ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void imgBtnRestore_Click(object sender, ImageClickEventArgs e)
    {
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
                        Msg = objVehicleRent.RestoreRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
    #region VehicleList
    protected void btnGetRentList_Click(object sender, EventArgs e)
    {
        objPageCmn.FillData((object)gvRentCalculation, null, "", "");

        int TotalDaysInMonth = 0;
        try
        {
            TotalDaysInMonth = DateTime.DaysInMonth(Convert.ToInt32(txttoYear.Text), Convert.ToInt32(ddlToMonth.SelectedValue));
        }
        catch
        {
            DisplayMessage("Enter Valid Year");
            txttoYear.Focus();
            return;
        }

        DateTime DtFromdate = new DateTime();
        DateTime DtTodate = new DateTime();

        DtFromdate = new DateTime(Convert.ToInt32(txttoYear.Text), Convert.ToInt32(ddlToMonth.SelectedValue), 1);
        DtTodate = new DateTime(Convert.ToInt32(txttoYear.Text), Convert.ToInt32(ddlToMonth.SelectedValue), TotalDaysInMonth);

        DataTable dt = new DataTable();
        string strsql = string.Empty;
        strsql = "select Contract_No,vehicle_id,Contractor_Id,Field2 as VehicleName,Field3 as ContractorName,Rate_Type,qty,Rate,Weight,Excess_Use_rate,0.000 as Rent_Amount,0.000 as Excess_Use_Amount,0.000 as NetPayable,0.000 as Total_Deduction from tp_Vehicle_contract where Field1='True' and (((From_Date  between '" + DtFromdate + "'  And '" + DtTodate + "') or (To_Date  between '" + DtFromdate + "'  And '" + DtTodate + "'))) and Vehicle_Id not in (select vehicle_id from tp_Vehicle_rent_trans where Field4='" + ddlToMonth.SelectedItem.Text + "' and Field5='" + txttoYear.Text + "')";
        dt = objda.return_DataTable(strsql);

        if (dt.Rows.Count > 0)
        {


            dt.AcceptChanges();
            dt.Columns["Rent_Amount"].ReadOnly = false;
            dt.Columns["Excess_Use_Amount"].ReadOnly = false;
            dt.Columns["NetPayable"].ReadOnly = false;
            dt.Columns["Total_Deduction"].ReadOnly = false;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string[] str = GetVehiclecalculation(dt.Rows[i]["Vehicle_Id"].ToString(), dt.Rows[i]["Rate_Type"].ToString(), dt.Rows[i]["Rate"].ToString(), dt.Rows[i]["qty"].ToString(), dt.Rows[i]["Excess_Use_rate"].ToString(), TotalDaysInMonth, dt.Rows[i]["Weight"].ToString(), DtFromdate, DtTodate);

                dt.Rows[i]["Rent_Amount"] = Common.GetAmountDecimal(str[0].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                dt.Rows[i]["Excess_Use_Amount"] = Common.GetAmountDecimal(str[1].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                dt.Rows[i]["NetPayable"] = Common.GetAmountDecimal(str[2].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                dt.Rows[i]["Total_Deduction"] = Common.GetAmountDecimal(str[3].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            }
        }
        Session["VRC_Rent_Cal"] = dt;
        objPageCmn.FillData((object)gvRentCalculation, dt, "", "");
        if (dt.Rows.Count > 0)
        {
            CustomValidator1.ValidationGroup = "Save";
            btn_Post.Enabled = true;
        }
        dt.Dispose();
        txttoYear.Enabled = false;
        ddlToMonth.Enabled = false;

    }

    public string[] GetVehiclecalculation(string strvehicleId, string rateType, string Rate, string Quantity, string ExcessUseRate, int TotalDayInMonth, string Weight, DateTime DtFromDate, DateTime dtTodate)
    {
        DataTable dt = new DataTable();
        string strsql = string.Empty;

        string[] strRent = new string[4];

        string RentAmount = "0";
        string ExcessUseAmt = "0";
        string NetAmt = "0";
        double TotalKM = 0;
        double TotalWeight = 0;
        double TotalPresentDay = 0;
        double totalAbsentDays = 0;
        string Totaldeduction = "0";
        //fix month calculation

        if (rateType.Trim() == "0")
        {
            TotalPresentDay = GetTotalPresentDay(strvehicleId, DtFromDate, dtTodate);
            totalAbsentDays = GetTotalAbsentDay(strvehicleId, DtFromDate, dtTodate);
            RentAmount = ((Convert.ToDouble(Rate) / TotalDayInMonth) * TotalPresentDay).ToString();
            Totaldeduction = ((Convert.ToDouble(Rate) / TotalDayInMonth) * totalAbsentDays).ToString();
            ExcessUseAmt = "0";
            NetAmt = RentAmount;
        }

        //month fix rate based on unit
        else if (rateType.Trim() == "1")
        {
            TotalPresentDay = GetTotalPresentDay(strvehicleId, DtFromDate, dtTodate);
            totalAbsentDays = GetTotalAbsentDay(strvehicleId, DtFromDate, dtTodate);
            TotalKM = Convert.ToDouble(GetVehicleUnit(strvehicleId, DtFromDate, dtTodate));
            if (TotalKM < Convert.ToDouble(Quantity))
            {
                RentAmount = ((Convert.ToDouble(Rate) / TotalDayInMonth) * TotalPresentDay).ToString();
                Totaldeduction = ((Convert.ToDouble(Rate) / TotalDayInMonth) * totalAbsentDays).ToString();
                ExcessUseAmt = "0";
                NetAmt = RentAmount;
            }
            else
            {
                RentAmount = Rate;
                ExcessUseAmt = ((TotalKM - Convert.ToDouble(Quantity)) * Convert.ToDouble(ExcessUseRate)).ToString();
                NetAmt = (Convert.ToDouble(RentAmount) + Convert.ToDouble(ExcessUseAmt)).ToString();

            }
        }


        else if (rateType.Trim() == "2")
        {
            dt = objda.return_DataTable("select isnull( sum( (End_Reading-Start_Reading)/" + Quantity + "* (cast(field3 as numeric(18,6)))/" + Weight + "*" + Rate + "),0) as Total from tp_Vehicle_Log where vehicle_id=" + strvehicleId + " and Trans_date between '" + DtFromDate + "' And '" + dtTodate + "' and isactive='True'");

            RentAmount = Common.GetAmountDecimal(dt.Rows[0][0].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ExcessUseAmt = "0";
            NetAmt = RentAmount;
        }


        strRent[0] = RentAmount;
        strRent[1] = ExcessUseAmt;
        strRent[2] = NetAmt;
        strRent[3] = Totaldeduction;
        return strRent;
    }


    public string GetVehicleUnit(string strvehicleId, DateTime dtFromdate, DateTime dtTodate)
    {
        string vehicleLog = string.Empty;

        string strsql = "select isnull( sum ((End_Reading-Start_Reading)),0) as Total from tp_Vehicle_Log where vehicle_id=" + strvehicleId + " and Trans_date between '" + dtFromdate + "' And '" + dtTodate + "' and isactive='True'";
        vehicleLog = objda.return_DataTable(strsql).Rows[0][0].ToString();
        return vehicleLog;

    }


    public string GetVehicleWeight(string strvehicleId, DateTime dtFromdate, DateTime dtTodate)
    {
        string vehicleLog = string.Empty;

        string strsql = "select ISNULL( SUM( CAST(Field3 as numeric(18,6))),0) as Total from tp_Vehicle_Log where vehicle_id=" + strvehicleId + " and Trans_date between '" + dtFromdate + "' And '" + dtTodate + "' and isactive='True'";
        vehicleLog = objda.return_DataTable(strsql).Rows[0][0].ToString();
        return vehicleLog;

    }


    public double GetTotalPresentDay(string strvehicleId, DateTime dtFromdate, DateTime dtTodate)
    {
        double totalDay = 0;
        string strsql = string.Empty;

        strsql = "select count(*) as Total from tp_Vehicle_Log where vehicle_id=" + strvehicleId + " and Trans_date between '" + dtFromdate + "' And '" + dtTodate + "' and Field4='Present' and IsActive='True'";

        totalDay = Convert.ToDouble(objda.return_DataTable(strsql).Rows[0][0].ToString());

        strsql = "select count(*) as Total from tp_Vehicle_Log where vehicle_id=" + strvehicleId + " and Trans_date between '" + dtFromdate + "' And '" + dtTodate + "' and Field4='Half Day' and IsActive='True'";

        totalDay += Convert.ToDouble(objda.return_DataTable(strsql).Rows[0][0].ToString()) / 2;


        return totalDay;

    }

    public double GetTotalAbsentDay(string strvehicleId, DateTime dtFromdate, DateTime dtTodate)
    {
        double totalDay = 0;
        string strsql = string.Empty;

        strsql = "select count(*) as Total from tp_Vehicle_Log where vehicle_id=" + strvehicleId + " and Trans_date between '" + dtFromdate + "' And '" + dtTodate + "' and Field4='Absent' and IsActive='True'";

        totalDay = Convert.ToDouble(objda.return_DataTable(strsql).Rows[0][0].ToString());

        strsql = "select count(*) as Total from tp_Vehicle_Log where vehicle_id=" + strvehicleId + " and Trans_date between '" + dtFromdate + "' And '" + dtTodate + "' and Field4='Half Day' and IsActive='True'";

        totalDay += Convert.ToDouble(objda.return_DataTable(strsql).Rows[0][0].ToString()) / 2;


        return totalDay;

    }

    protected void chkgvHeaderselect_OnCheckedChanged(object sender, EventArgs e)
    {
        bool IsCheck = ((CheckBox)(gvRentCalculation).HeaderRow.FindControl("chkgvHeaderselect")).Checked;

        foreach (GridViewRow gvrow in gvRentCalculation.Rows)
        {
            ((CheckBox)gvrow.FindControl("chkgvselect")).Checked = IsCheck;
        }
    }
    #endregion
    #region Account
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        string filtertext = "AccountName like '%" + prefixText + "%'";
        dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }

        return txt;
    }
    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True'";
                DataTable dtCOA = objda.return_DataTable(sql);

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose Account in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {
                DisplayMessage("Choose Account in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }
    }

    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id=" + strAccountNo + " and IsActive='True'";

            DataTable dtAccName = objda.return_DataTable(sql);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString() + "/" + strAccountNo;
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }

    #endregion

    protected void gvRentCalculation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRentCalculation.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["VRC_Rent_Cal"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvRentCalculation, dt, "", "");
        //AllPageCode();
        gvRentCalculation.HeaderRow.Focus();
    }

    protected void gvRentCalculation_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["VRC_Rent_Cal"];
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
        Session["VRC_Rent_Cal"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvRentCalculation, dt, "", "");
        //AllPageCode();
        gvRentCalculation.HeaderRow.Focus();
    }
}