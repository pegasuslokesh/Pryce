using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

public partial class Purchase_PurchaseInvoice_Deduction : System.Web.UI.Page
{

    EmployeeMaster objEmployee = null;
    DataAccessClass objda = null;
    Common ObjComman = null;
    SystemParameter ObjSys = null;
    LocationMaster objLocation = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Set_DocNumber objDocNo = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_ChartOfAccount objCOA = null;
    Inv_ProductMaster ObjProductMaster = null;
    Ems_ContactMaster objContact = null;
    Inv_InvoiceDeductionInfo objInvoiceDeduction = null;
    PurchaseInvoice ObjPurchaseInvoice = null;
    Inv_SalesInvoiceHeader objSalesInvoiceHeader = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        EmployeeMaster objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        DataAccessClass objda = new DataAccessClass(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());
        SystemParameter ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        LocationMaster objLocation = new LocationMaster(Session["DBConnection"].ToString());
        Ac_Parameter_Location objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        Set_DocNumber objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        Ac_Voucher_Header objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        Ac_Voucher_Detail objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        Ac_ParameterMaster objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        Ac_ChartOfAccount objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        Inv_ProductMaster ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        Ems_ContactMaster objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        Inv_InvoiceDeductionInfo objInvoiceDeduction = new Inv_InvoiceDeductionInfo(Session["DBConnection"].ToString());
        PurchaseInvoice ObjPurchaseInvoice = new PurchaseInvoice(Session["DBConnection"].ToString());
        Inv_SalesInvoiceHeader objSalesInvoiceHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "383", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillGrid();
            AllPageCode();
            ddlrefType_OnSelectedIndexChanged(null, null);
            CalendarExtendertxtVoucherDate.Format = ObjSys.SetDateFormat();
        }
    }
    protected void rbtndeductionsystem_OnCheckedChanged(object sender, EventArgs e)
    {

        if (rbtndeductionsystem.Checked)
        {
            try
            {
                GvsalaryPlan.Columns[0].Visible = true;
                GvsalaryPlan.Columns[1].Visible = true;
                GvsalaryPlan.Columns[2].Visible = true;
                GvsalaryPlan.Columns[3].Visible = false;
            }
            catch
            {

            }
        }
        else
        {
            try
            {
                GvsalaryPlan.Columns[0].Visible = false;
                GvsalaryPlan.Columns[1].Visible = false;
                GvsalaryPlan.Columns[2].Visible = false;
                GvsalaryPlan.Columns[3].Visible = true;
            }
            catch
            {

            }

        }
        FillGrid();
        AllPageCode();
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("383", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code

        Page.Title = ObjSys.GetSysTitle();

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {
            btnRefundSave.Visible = true;
            btnrefundPost.Visible = true;
            if (Lbl_Tab_New.Text != Resources.Attendance.View)
            {
                Btn_Save.Visible = true;
                btn_Post.Visible = true;
               
            }
            else
            {
                Btn_Save.Visible = false;
                btn_Post.Visible = false;
            }
            foreach (GridViewRow Row in GvsalaryPlan.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                ((ImageButton)Row.FindControl("lnkViewDetail")).Visible = true;
            }
            imgBtnRestore.Visible = true;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {
            DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["userId"].ToString(), strModuleId, "383", HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {

                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            btnRefundSave.Visible = true;
                            btnrefundPost.Visible = true;
                            if (Lbl_Tab_New.Text != Resources.Attendance.View)
                            {
                                Btn_Save.Visible = true;
                                btn_Post.Visible = true;
                            }
                            else
                            {
                                Btn_Save.Visible = false;
                                btn_Post.Visible = false;
                            }

                        }
                        foreach (GridViewRow Row in GvsalaryPlan.Rows)
                        {
                            if (DtRow["Op_Id"].ToString() == "2")
                            {
                                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                            }
                            if (DtRow["Op_Id"].ToString() == "3")
                            {
                                ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                            }
                            if (DtRow["Op_Id"].ToString() == "5")
                            {
                                ((ImageButton)Row.FindControl("lnkViewDetail")).Visible = true;
                            }
                        }
                        if (DtRow["Op_Id"].ToString() == "4")
                        {
                            imgBtnRestore.Visible = true;
                            ImgbtnSelectAll.Visible = false;
                        }


                    }
                }
            }

            else
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
    }

    protected void Btn_List_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void Btn_New_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_Bin_Click(object sender, EventArgs e)
    {
        FillGridBin();
    }

    protected void Btn_Post_Click(object sender, EventArgs e)
    {

        Btn_Save_Click(sender, e);
        AllPageCode();
    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {


        DateTime dtVoucherdate = new DateTime();
        
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        bool IsPost = false;
        string strsql = string.Empty;
        double PaidTotaamount = Convert.ToDouble(txtdeductionAmount.Text);
        string Narration = ddlDeductionType.SelectedValue + " deduction for Invoice No. '" + hdnsupplierInvoiceNo.Value + "' ( " + ddlInvoice.SelectedItem.Text + " ) ";
        int MaxId = 0;
        string strAccountNo = string.Empty;

        if (ddlrefType.SelectedIndex == 0)
        {
            strAccountNo = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());


            dtVoucherdate = Convert.ToDateTime(ObjPurchaseInvoice.GetPurchaseInvoiceTrueAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlInvoice.SelectedValue).Rows[0]["InvoiceDate"].ToString());
        }
        else
        {
            strAccountNo = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            dtVoucherdate = Convert.ToDateTime(objSalesInvoiceHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlInvoice.SelectedValue).Rows[0]["Invoice_Date"].ToString());

        }

        if (((Button)sender).ID.Trim() == "btn_Post")
        {
            IsPost = true;
        }
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {
            if (((Button)sender).ID.Trim() == "btn_Post" && hdnInvoiceType.Value.Trim() == "Final")
            {


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
                    string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
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

                    MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0",dtVoucherdate.ToString(), strVoucherNumber, dtVoucherdate.ToString(), "JV", "1/1/1800", "1/1/1800", "", Narration, strCurrencyId, "1", Narration, false.ToString(), false.ToString(), false.ToString(), "JV", "", "", hdnsupplierInvoiceNo.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
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
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), txtCustomer.Text.Split('/')[1].ToString(), "0", "ID", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), txtCustomer.Text.Split('/')[1].ToString(), "0", "ID", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    //For Credit
                    string strCompanyCrrValueCr = strCompanyCrrValueDr;
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                    if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
            }

            if (hdnEditId.Value == "")
            {
                objInvoiceDeduction.InsertRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlrefType.SelectedValue, ddlInvoice.SelectedValue, strAccountNo, txtCustomer.Text.Split('/')[1].ToString(), ddlDeductionType.SelectedValue, ddldeductionMethod.SelectedValue, txtApplicableAmount.Text, txtdeductionValue.Text, txtdeductionAmount.Text, MaxId.ToString(), "0", "0", false.ToString(), dtVoucherdate.ToString(), IsPost.ToString(), ddlInvoice.SelectedItem.Text, txtCustomer.Text.Split('/')[0].ToString(), txtInvoiceAmount.Text, hdnInvoiceType.Value, hdnsupplierInvoiceNo.Value, DateTime.Now.ToString(), true.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), ref trns);
                //
            }
            else
            {
                objInvoiceDeduction.UpdateRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEditId.Value, ddlrefType.SelectedValue, ddlInvoice.SelectedValue, strAccountNo, txtCustomer.Text.Split('/')[1].ToString(), ddlDeductionType.SelectedValue, ddldeductionMethod.SelectedValue, txtApplicableAmount.Text, txtdeductionValue.Text, txtdeductionAmount.Text, MaxId.ToString(), "0", "0", false.ToString(), dtVoucherdate.ToString(), IsPost.ToString(), ddlInvoice.SelectedItem.Text, txtCustomer.Text.Split('/')[0].ToString(), txtInvoiceAmount.Text, hdnInvoiceType.Value, hdnsupplierInvoiceNo.Value, DateTime.Now.ToString(), true.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), ref trns);

                // objEmployeeResources.UpdateRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEditId.Value, strEmployeeId, ddlProductName.SelectedValue, txtSerialNo.Text, txtOrderQty.Text, ddlUnit.SelectedValue, chkIsReturnable.Checked.ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), strIssuedBy, ddlTransType.SelectedValue, txtPenaltyAmt.Text, IsPost.ToString(), txtRemarks.Text, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), ref trns);
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


                Btn_List_Click(null, null);
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
        FillInvoice();
        txtdeductionValue.Text = "0";
        txtdeductionAmount.Text = "0";
        txtInvoiceAmount.Text = "0";
        txtApplicableAmount.Text = "0";
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        Session["CHECKED_ITEMS"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        AllPageCode();
        hdnsupplierInvoiceNo.Value = "";
    }
    #region List

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void FillGrid()
    {
        DataTable dtBrand = objInvoiceDeduction.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        if (rbtnRefundsystem.Checked)
        {
            dtBrand = new DataView(dtBrand, "Field5='Final' and Is_Post='True' and Is_Refund='False' and Deduction_Type='SD'", "", DataViewRowState.CurrentRows).ToTable();
            ddlPosted.Visible = false;
        }
        else
        {
            ddlPosted.Visible = true;
            if (ddlPosted.SelectedIndex == 0)
            {
                dtBrand = new DataView(dtBrand, "Is_Post='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlPosted.SelectedIndex == 1)
            {
                dtBrand = new DataView(dtBrand, "Is_Post='False'", "", DataViewRowState.CurrentRows).ToTable();

            }
        }


        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtDeduction"] = dtBrand;
        Session["dtFilter_PurInv_Dedu"] = dtBrand;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dtBrand, "", "");
        AllPageCode();

    }


    protected void GvsalaryPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvsalaryPlan.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_PurInv_Dedu"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
        AllPageCode();
        GvsalaryPlan.HeaderRow.Focus();
    }

    protected void GvsalaryPlan_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_PurInv_Dedu"];
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
        Session["dtFilter_PurInv_Dedu"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
        AllPageCode();
        GvsalaryPlan.HeaderRow.Focus();
    }


    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {
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
            DataTable dtDeduction = (DataTable)Session["dtDeduction"];
            DataView view = new DataView(dtDeduction, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvsalaryPlan, view.ToTable(), "", "");
            Session["dtFilter_PurInv_Dedu"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            AllPageCode();
        }
        txtValue.Focus();
    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
        FillGridBin();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }


    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = objInvoiceDeduction.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["Is_Post"].ToString() == "True" && ((ImageButton)sender).ID == "btnEdit")
            {
                DisplayMessage("Record Posted , you can not edit");
                return;

            }
            hdnEditId.Value = e.CommandArgument.ToString();

            ddlrefType.SelectedValue = dt.Rows[0]["Ref_Type"].ToString();
            ddlrefType_OnSelectedIndexChanged(null, null);
            txtCustomer.Text = dt.Rows[0]["Field3"].ToString() + "/" + dt.Rows[0]["Other_Account_No"].ToString();
            ddlDeductionType.SelectedValue = dt.Rows[0]["Deduction_Type"].ToString();
            FillInvoice();
           
            ddlInvoice.SelectedValue = dt.Rows[0]["Ref_Id"].ToString();
            txtInvoiceAmount.Text = Common.GetAmountDecimal(dt.Rows[0]["Field4"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            hdnInvoiceType.Value = dt.Rows[0]["Field5"].ToString().Trim();
            hdnsupplierInvoiceNo.Value = dt.Rows[0]["Field6"].ToString().Trim();
            txtApplicableAmount.Text = Common.GetAmountDecimal(dt.Rows[0]["Applicable_Amount"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            ddldeductionMethod.SelectedValue = dt.Rows[0]["Deduction_Method"].ToString();
            txtdeductionValue.Text = Common.GetAmountDecimal(dt.Rows[0]["Deduction_Value"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            txtdeductionAmount.Text = Common.GetAmountDecimal(dt.Rows[0]["Deduction_Amount"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            //txtEmployee.Text = "";

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
        AllPageCode();
    }

   

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = 0;

        DataTable dt = new DataTable();

        dt = objInvoiceDeduction.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {


            if (dt.Rows[0]["Is_Post"].ToString() == "True")
            {
                DisplayMessage("Record Posted , you can not delete");
                return;

            }
        }


        b = objInvoiceDeduction.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        dt = objInvoiceDeduction.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
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
            AllPageCode();
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
                AllPageCode();
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
                        Msg = objInvoiceDeduction.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
        AllPageCode();

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

        AllPageCode();
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

        if (txtCustomer.Text != "")
        {
            string[] CustomerName = txtCustomer.Text.Split('/');

            DataTable DtCustomer = objContact.GetContactByContactName(CustomerName[0].ToString().Trim());

            if (DtCustomer.Rows.Count > 0)
            {

                strContactId = CustomerName[1].ToString().Trim();
            }
            else
            {

                DisplayMessage("Enter Customer Name in suggestion Only");
                txtCustomer.Text = "";
                txtCustomer.Focus();

            }
        }


        FillInvoice();
    }
    #region Invoice

    protected void ddlDeductionType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FillInvoice();

    }

    public void FillInvoice()
    {

        string strContactId = "0";


        try
        {
            strContactId = txtCustomer.Text.Split('/')[1].ToString();

        }
        catch
        {

        }
        ddlInvoice.Items.Clear();
        string strsql = string.Empty;

        ListItem Li = new ListItem();
        Li.Text = "--Select--";
        Li.Value = "0";



        DataTable dtInvoice = new DataTable();
        if (ddlrefType.SelectedIndex == 0)
        {
            if (hdnEditId.Value == "")
            {
                strsql = "select Inv_PurchaseInvoiceHeader.invoiceNo as Invoice_No,Inv_PurchaseInvoiceHeader.TransID  as Trans_Id from Inv_PurchaseInvoiceHeader where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Field4<>'Normal' and SupplierId=" + strContactId + " and TransId not in (select Inv_InvoiceDeductionInfo.Ref_id from Inv_InvoiceDeductionInfo  where Inv_InvoiceDeductionInfo.Ref_Type='Supplier' and Deduction_Type='" + ddlDeductionType.SelectedValue + "')";
            }
            else
            {
                strsql = "select Inv_PurchaseInvoiceHeader.invoiceNo as Invoice_No,Inv_PurchaseInvoiceHeader.TransID  as Trans_Id from Inv_PurchaseInvoiceHeader where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Field4<>'Normal'and  SupplierId=" + strContactId + " and TransId not in (select Inv_InvoiceDeductionInfo.Ref_id from Inv_InvoiceDeductionInfo  where Inv_InvoiceDeductionInfo.Ref_Type='Supplier' and Inv_InvoiceDeductionInfo.TRans_id<>" + hdnEditId.Value + " and Deduction_Type='" + ddlDeductionType.SelectedValue + "')";
            }

        }
        else
        {
            if (hdnEditId.Value == "")
            {
                strsql = "select Inv_SalesInvoiceHeader.Invoice_No,Inv_SalesInvoiceHeader.Trans_Id from Inv_SalesInvoiceHeader where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Supplier_Id=" + strContactId + " and Trans_Id not in (select Inv_InvoiceDeductionInfo.Ref_id from Inv_InvoiceDeductionInfo  where Inv_InvoiceDeductionInfo.Ref_Type='Customer' and Deduction_Type='" + ddlDeductionType.SelectedValue + "')";
            }
            else
            {
                strsql = "select Inv_SalesInvoiceHeader.Invoice_No,Inv_SalesInvoiceHeader.Trans_Id from Inv_SalesInvoiceHeader where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Supplier_Id=" + strContactId + " and Trans_Id not in (select Inv_InvoiceDeductionInfo.Ref_id from Inv_InvoiceDeductionInfo  where Inv_InvoiceDeductionInfo.Ref_Type='Customer' and Inv_InvoiceDeductionInfo.TRans_id<>" + hdnEditId.Value + " and Deduction_Type='" + ddlDeductionType.SelectedValue + "')";

            }

        }


        dtInvoice = objda.return_DataTable(strsql);

        ddlInvoice.DataSource = dtInvoice;
        ddlInvoice.DataTextField = "Invoice_No";
        ddlInvoice.DataValueField = "Trans_Id";
        ddlInvoice.DataBind();
        ddlInvoice.Items.Insert(0, Li);

        dtInvoice.Dispose();

        txtInvoiceAmount.Text = "0";
        txtApplicableAmount.Text = "0";
        txtdeductionAmount.Text = "0";
    }

    protected void ddlInvoice_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtInvoice = new DataTable();
        string strsql = string.Empty;

        txtApplicableAmount.Text = "0";
        txtdeductionValue.Text = "0";
        txtdeductionAmount.Text = "0";
        hdnsupplierInvoiceNo.Value = "";
        if (ddlrefType.SelectedIndex == 0)
        {
            strsql = "select Inv_PurchaseInvoiceHeader.GrandTotal,Inv_PurchaseInvoiceHeader.Field4 as BillType,Inv_PurchaseInvoiceHeader.SupInvoiceNo from Inv_PurchaseInvoiceHeader where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and TRansId=" + ddlInvoice.SelectedValue + "";
        }
        else
        {

            strsql = "select Inv_SalesInvoiceHeader.GrandTotal,'Running' as BillType,Inv_SalesInvoiceHeader.Invoice_Ref_No as  SupInvoiceNo  from Inv_SalesInvoiceHeader where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and Trans_Id=" + ddlInvoice.SelectedValue + " ";
        }

        dtInvoice = objda.return_DataTable(strsql);

        if (dtInvoice.Rows.Count > 0)
        {
            txtApplicableAmount.Text = Common.GetAmountDecimal(dtInvoice.Rows[0]["GrandTotal"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            txtInvoiceAmount.Text = Common.GetAmountDecimal(dtInvoice.Rows[0]["GrandTotal"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            hdnInvoiceType.Value = dtInvoice.Rows[0]["BillType"].ToString();
            hdnsupplierInvoiceNo.Value = dtInvoice.Rows[0]["SupInvoiceNo"].ToString();
        }

    }

    protected void CommonCalculation(object sender, EventArgs e)
    {
        double InvoiceAmount = 0;
        double ApplicableAmount = 0;
        double deductionValue = 0;
        double DeductionAmount = 0;



        try
        {
            InvoiceAmount = Convert.ToDouble(txtInvoiceAmount.Text);

        }
        catch
        {

        }
        try
        {
            ApplicableAmount = Convert.ToDouble(txtApplicableAmount.Text);
        }
        catch
        {
        }

        if (InvoiceAmount < ApplicableAmount)
        {
            DisplayMessage("Applicable amount should be less then or equal to invoice amount");
            txtApplicableAmount.Text = "0";
            txtApplicableAmount.Focus();
            ApplicableAmount = 0;
        }




        try
        {
            deductionValue = Convert.ToDouble(txtdeductionValue.Text);
        }
        catch
        {
        }

        if (ddldeductionMethod.SelectedIndex == 0)
        {
            DeductionAmount = (ApplicableAmount * deductionValue) / 100;
        }
        else
        {
            DeductionAmount = deductionValue;
        }


        if (DeductionAmount > ApplicableAmount)
        {
            DisplayMessage("deduction amount should be less then or equal to applicable amount");
            txtdeductionAmount.Text = "0";
            txtdeductionValue.Text = "0";
            txtdeductionValue.Focus();
            ApplicableAmount = 0;
        }
        else
        {

            txtdeductionAmount.Text = Common.GetAmountDecimal(DeductionAmount.ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
        }


    }

    protected void ddldeductionMethod_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtdeductionValue.Text = "0";
        txtdeductionAmount.Text = "0";
        txtdeductionValue.Focus();
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
    protected void ddlrefType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlrefType.SelectedIndex == 0)
        {
            txtpaymentdebitaccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers").Rows[0]["Param_Value"].ToString());
        }
        else
        {
            txtpaymentdebitaccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers").Rows[0]["Param_Value"].ToString());

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
    private string GetLocationCode(string strLocationId)
    {
        string strLocationCode = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocation = objLocation.GetLocationMasterByLocationId(strLocationId);
            if (dtLocation.Rows.Count > 0)
            {
                strLocationCode = dtLocation.Rows[0]["Location_Code"].ToString();
            }
        }
        return strLocationCode;
    }
    #endregion

    #region Refund
    protected void btnRefundSave_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        bool IsPost = false;
        string strsql = string.Empty;
        double PaidTotaamount = Convert.ToDouble(txtRefundAmount.Text);

        string Narration = " SD Refund for Invoice No. '" + hdnsupplierInvoiceNo.Value + "' ( " + lblInvoiceNoValue.Text + " ) ";
        
        int MaxId = 0;
      

       
        if (((Button)sender).ID.Trim() == "btnrefundPost")
        {
            IsPost = true;
        }
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        try
        {
            if (IsPost)
            {


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
                    string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns,HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
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

                    MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", ObjSys.getDateForInput(txtVoucherDate.Text).ToString(), strVoucherNumber, ObjSys.getDateForInput(txtVoucherDate.Text).ToString(), "JV", "1/1/1800", "1/1/1800", "", Narration, strCurrencyId, "1", Narration, false.ToString(), false.ToString(), false.ToString(), "JV", "", "",hdnsupplierInvoiceNo.Value, "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    string strVMaxId = MaxId.ToString();


                    string strCompAmount = PaidTotaamount.ToString();
                    string strLocAmount = PaidTotaamount.ToString();
                    string strForeignAmount = PaidTotaamount.ToString();
                    string strForeignExchangerate = "1";



                    //str for Employee Id
                    //For Debit

                    string strCompanyCrrValueDr = PaidTotaamount.ToString();
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                    if (strAccountId.Split(',').Contains(txtdebitRefundAccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtdebitRefundAccount.Text.Split('/')[1].ToString(), "0", "0", "ID", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtdebitRefundAccount.Text.Split('/')[1].ToString(), "0", "0", "ID", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    //For Credit
                    string strCompanyCrrValueCr = strCompanyCrrValueDr;
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                    if (strAccountId.Split(',').Contains(txtCreditRefundAccount.Text.Split('/')[1].ToString()))
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtCreditRefundAccount.Text.Split('/')[1].ToString(), hdnRefId.Value, "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtCreditRefundAccount.Text.Split('/')[1].ToString(), hdnRefId.Value, "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
            }

            objda.execute_Command("update Inv_InvoiceDeductionInfo set Refund_Amount ='" + txtRefundAmount.Text + "',Refund_Voucher_Id='"+MaxId.ToString()+"',Refund_Date='" + ObjSys.getDateForInput(txtVoucherDate.Text).ToString() + "',Is_refund='"+IsPost.ToString()+"'  where Trans_Id="+editid.Value+" ",ref trns);

            DisplayMessage("Record Updated Successfully", "green");
            
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            

            DisplayMessage("Record Saved Successfully","green");
              
            Reset();
            FillGrid();

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Purchase_Modal()", true);

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

    protected void btnrefundPost_Click(object sender, EventArgs e)
    {

        btnRefundSave_Click(sender, e);
    }

    protected void lnkRefundDetail_Command(object sender, CommandEventArgs e)
    {
        txtVoucherDate.Text = DateTime.Now.ToString(ObjSys.SetDateFormat());


        editid.Value = e.CommandArgument.ToString();

        GridViewRow Gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        hdnRefId.Value = ((Label)Gvrow.FindControl("lblRefidvalue")).Text;
        lblSupplierNameValue.Text = ((Label)Gvrow.FindControl("lblrefName")).Text;
        lblInvoiceNoValue.Text = ((Label)Gvrow.FindControl("lblInvoiceNo")).Text;
        lblInvoiceAmountValue.Text = ((Label)Gvrow.FindControl("lblInvoiceamt")).Text;
        lbldeductionAmount.Text = ((Label)Gvrow.FindControl("lbldeductionamt")).Text;
        txtRefundAmount.Text = ((Label)Gvrow.FindControl("lbldeductionamt")).Text;
        hdnsupplierInvoiceNo.Value = ((Label)Gvrow.FindControl("lblsupplierInvoiceNo")).Text;
        txtCreditRefundAccount.Text = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers").Rows[0]["Param_Value"].ToString());

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Purchase_Modal()", true);
    }
    #endregion


}