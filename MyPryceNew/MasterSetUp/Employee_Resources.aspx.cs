using PegasusDataAccess;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetUp_Employee_Resources : System.Web.UI.Page
{
    Set_EmployeeResources objEmployeeResources = null;
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
    Inv_StockDetail ObjStockDetail = null;
    Ems_ContactMaster objContact = null;
    Inv_AdjustHeader objAdjustHeader = null;
    Inv_AdjustDetail objAdjustDetail = null;
    Inv_ProductLedger ObjProductledger = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmployeeResources = new Set_EmployeeResources(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objAdjustHeader = new Inv_AdjustHeader(Session["DBConnection"].ToString());
        objAdjustDetail = new Inv_AdjustDetail(Session["DBConnection"].ToString());
        ObjProductledger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../MasterSetup/Employee_Resources.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
            if (Session["EmpId"].ToString() != "0" && Request.QueryString["Id"] == null)
            {
                txtIssuedBy.Text = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()).Rows[0]["Emp_Name"].ToString() + "/" + Session["EmpId"].ToString();
            }
            Session["TransType"] = "I";
            if (Request.QueryString["Request_Id"] != null)
            {
                UpdatePageInformation_For_Party();
            }


            //Request_Id
            //FillProductDropDown();
        }
        if (Request.QueryString["Request_Id"] != null)
            Session["ER_Request_Id"] = Request.QueryString["Request_Id"];
        else
            Session["ER_Request_Id"] = "null";

        Session["ER_TransType"] = ddlTransType.SelectedValue;
    }

    #region calculation

    protected void txtOrderQty_OnTextChanged(object sender, EventArgs e)
    {
        if (ddlTransType.SelectedValue == "In")
        {
            if (float.Parse(txtOrderQty.Text) > float.Parse(txtRemainQty.Text))
            {
                DisplayMessage("In Quantity should be less then or equal to remaining quantity");
                txtOrderQty.Text = "";
                txtOrderQty.Focus();

                return;
            }
        }


        if (ddlTransType.SelectedValue == "Out" && Request.QueryString["Request_Id"] != null && hdnStockType.Value == "S" && (float.Parse(txtOrderQty.Text) > float.Parse(txtRemainQty.Text)))
        {
            DisplayMessage("Out Quantity should be less then or equal to System quantity");
            txtOrderQty.Text = "";
            txtOrderQty.Focus();
            return;
        }


        txtAmount.Text = Common.GetAmountDecimal((Convert.ToDouble(txtOrderQty.Text) * Convert.ToDouble(txtUnitCost.Text)).ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());

    }
    #endregion
    public void UpdatePageInformation_For_Party()
    {
        ddlFieldName.Items.FindByValue("Field4").Text = "Contractor";
        ddlbinFieldName.Items.FindByValue("Field4").Text = "Contractor";
        lblRefName.Text = "Contractor";
        GvsalaryPlan.Columns[4].HeaderText = "Contractor";
        GvsalaryPlanBin.Columns[2].HeaderText = "Contractor";
        lblsysqty.Text = "System Quantity";
        RequiredFieldValidator_txtEmployee.ErrorMessage = "Enter Contractor Name";
        txtSalesPerson_AutoCompleteExtender.Enabled = false;
        txtSupplierName_AutoCompleteExtender.Enabled = true;
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

        string strRef_Id = string.Empty;
        string strInqty = string.Empty;
        string strOutQty = string.Empty;
        string strsql = string.Empty;


        //here in str_refid variable we are saving 0 or 1 value
        //0 means employee and 1 means supplier

        if (Request.QueryString["Request_Id"] != null)
        {
            strRef_Id = "1";

        }
        else
        {
            strRef_Id = "0";
        }




        if (txtUnitCost.Text == "" || txtUnitCost.Text == "0")
        {
            txtUnitCost.Text = "1";
            txtAmount.Text = txtOrderQty.Text;
        }



        if (((Button)sender).ID.Trim() == "btn_Post")
        {

            if (ddlTransType.SelectedValue == "Out" && Request.QueryString["Request_Id"] != null && hdnStockType.Value == "S" && (float.Parse(txtOrderQty.Text) > float.Parse(txtRemainQty.Text)))
            {
                DisplayMessage("Out Quantity should be less then or equal to System quantity");
                txtOrderQty.Focus();
                return;
            }

            if (ddlTransType.SelectedValue == "In")
            {

                txtRemainQty.Text = GetRemainingQty(ddlProductId.SelectedValue);
                if (float.Parse(txtOrderQty.Text) > float.Parse(txtRemainQty.Text))
                {
                    DisplayMessage("In Quantity should be less then or equal to remaining quantity");
                    txtOrderQty.Focus();

                    return;
                }
            }

            IsPost = true;
        }
        //here we are checking that in qty should not be greater then out qty



        //here we are added validation that in case of in transaction we can not save new transaction untill post previous record


        if (hdnEditId.Value == "")
        {
            strsql = "select Trans_id from Set_EmployeeResources where Emp_Id=" + txtEmployee.Text.Split('/')[1].ToString() + " and Product_Id=" + ddlProductId.SelectedValue.Trim() + " and Field5='" + strRef_Id + "' and Field1='False' and IsActive='True'";
        }
        else
        {
            strsql = "select Trans_id from Set_EmployeeResources where Emp_Id=" + txtEmployee.Text.Split('/')[1].ToString() + " and Product_Id=" + ddlProductId.SelectedValue.Trim() + " and Field5='" + strRef_Id + "' and Field1='False' and IsActive='True' and Trans_id<>" + hdnEditId.Value + "";

        }
        if (objda.return_DataTable(strsql).Rows.Count > 0)
        {
            DisplayMessage("Post previous transaction then proceed next");
            return;
        }


        DateTime PreviousRehiringDate = new DateTime(1900, 1, 1);
        string strEmployeeId = GetEmployeeId(txtEmployee.Text);
        string strIssuedBy = GetEmployeeId(txtIssuedBy.Text);

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strTransId = string.Empty;
        int b = 0;
        try
        {

            if (hdnEditId.Value == "")
            {
                b = objEmployeeResources.InsertRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtEmployee.Text.Split('/')[1].ToString(), ddlProductId.SelectedValue, txtSerialNo.Text, txtOrderQty.Text, ddlUnit.SelectedValue, chkIsReturnable.Checked.ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), strIssuedBy, ddlTransType.SelectedValue, txtUnitCost.Text, IsPost.ToString(), txtRemarks.Text, txtAmount.Text, txtEmployee.Text.Split('/')[0].ToString(), strRef_Id, chkIsAdjusted.Checked.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), ref trns);
                strTransId = b.ToString();

            }
            else
            {
                objEmployeeResources.UpdateRecord(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEditId.Value, txtEmployee.Text.Split('/')[1].ToString(), ddlProductName.SelectedValue, txtSerialNo.Text, txtOrderQty.Text, ddlUnit.SelectedValue, chkIsReturnable.Checked.ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), strIssuedBy, ddlTransType.SelectedValue, txtUnitCost.Text, IsPost.ToString(), txtRemarks.Text, txtAmount.Text, txtEmployee.Text.Split('/')[0].ToString(), strRef_Id, chkIsAdjusted.Checked.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userid"].ToString(), DateTime.Now.ToString(), ref trns);
                strTransId = hdnEditId.Value;
            }


            //stock entry

            if (Request.QueryString["Request_Id"] != null && IsPost)
            {
                string strSerial = "0";


                if (txtSerialNo.Text.Trim() != "")
                {
                    strSerial = txtSerialNo.Text.Trim();
                }


                if (ddlTransType.SelectedIndex == 0)
                {
                    strRef_Id = "I";
                }
                else
                {
                    strRef_Id = "O";
                }

                string docNo = objDocNo.GetDocumentNo(true, Session["compId"].ToString(), true, "11", "131", "0", ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());


                string sql = "select count(*)+1 from Inv_AdjustHeader where CompanyId=" + Session["CompId"].ToString() + " and BrandId=" + Session["BrandId"].ToString() + " and FromLocationID=" + Session["LocId"].ToString() + "";
                docNo = docNo + objda.return_DataTable(sql).Rows[0][0].ToString();

                b = objAdjustHeader.InsertAdjustHeader(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["LocId"].ToString(), docNo, ObjSys.getDateForInput(txttrnDate.Text).ToString(), "Stock " + ddlTransType.SelectedValue + " Entry For " + txtEmployee.Text.Split('/')[0].ToString(), txtAmount.Text, true.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                objAdjustDetail.InsertAdjustDetail(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), b.ToString(), docNo, strSerial, ddlProductId.SelectedValue, ddlUnit.SelectedValue, txtOrderQty.Text, txtUnitCost.Text, strRef_Id, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (strRef_Id.Trim() == "I")
                {
                    ObjProductledger.InsertProductLedger(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", b.ToString(), Session["LocId"].ToString(), ddlProductId.SelectedValue, ddlUnit.SelectedValue, "I", "0", "0", txtOrderQty.Text, "0", "1/1/1800", txtUnitCost.Text, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), Session["UserId"].ToString().ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                }
                else
                {
                    ObjProductledger.InsertProductLedger(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", b.ToString(), Session["LocId"].ToString(), ddlProductId.SelectedValue, ddlUnit.SelectedValue, "O", "0", "0", "0", txtOrderQty.Text, "1/1/1800", txtUnitCost.Text, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), Session["UserId"].ToString().ToString(), ObjSys.getDateForInput(txttrnDate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
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
        try
        {
            ddlProductId.SelectedIndex = 0;
            ddlProductName.SelectedIndex = 0;
        }
        catch
        {

        }
        Txt_Product_Name.Text = "";
        ddlUnit.Items.Clear();
        txtUnitCost.Text = "1";
        txtAmount.Text = "1";
        hdnEditId.Value = "";
        lblSelectedRecord.Text = "";
        txtOrderQty.Text = "1";
        txtSerialNo.Text = "";
        ViewState["Select"] = null;
        Session["CHECKED_ITEMS"] = null;
        txtEmployee.Focus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtRemainQty.Text = "0";
        ddlProductId.Focus();
        FillProductDropDown();
        txtRemarks.Text = "";
        chkIsReturnable.Checked = false;
        chkIsAdjusted.Checked = false;
        if (ddlTransType.SelectedIndex == 1)
        {
            chkIsReturnable.Enabled = true;
            chkIsAdjusted.Enabled = true;
        }

        //AllPageCode();
    }
    #region List

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void FillGrid()
    {
        DataTable dtBrand = objEmployeeResources.GetTrueAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        if (ddlPosted.SelectedIndex == 0)
        {
            dtBrand = new DataView(dtBrand, "Is_Post='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlPosted.SelectedIndex == 1)
        {
            dtBrand = new DataView(dtBrand, "Is_Post='False'", "", DataViewRowState.CurrentRows).ToTable();

        }

        if (Request.QueryString["Request_Id"] != null)
        {
            dtBrand = new DataView(dtBrand, "Field5='1'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtBrand = new DataView(dtBrand, "Field5='0'", "", DataViewRowState.CurrentRows).ToTable();
        }

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtDeduction"] = dtBrand;
        Session["dtFilter_EmpResourc_Mstr"] = dtBrand;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dtBrand, "", "");
        //AllPageCode();

    }
    protected void GvsalaryPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvsalaryPlan.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_EmpResourc_Mstr"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvsalaryPlan, dt, "", "");
        //AllPageCode();
        GvsalaryPlan.HeaderRow.Focus();
    }
    protected void GvsalaryPlan_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_EmpResourc_Mstr"];
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
        Session["dtFilter_EmpResourc_Mstr"] = dt;
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
            DataTable dtDeduction = (DataTable)Session["dtDeduction"];
            DataView view = new DataView(dtDeduction, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvsalaryPlan, view.ToTable(), "", "");
            Session["dtFilter_EmpResourc_Mstr"] = view.ToTable();
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

        DataTable dt = objEmployeeResources.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {

            if (dt.Rows[0]["Is_Post"].ToString() == "True" && ((LinkButton)sender).ID == "btnEdit")
            {
                DisplayMessage("Record Posted , you can not edit");
                return;

            }
            hdnEditId.Value = e.CommandArgument.ToString();
            txttrnDate.Text = Convert.ToDateTime(dt.Rows[0]["Trn_Date"].ToString()).ToString(ObjSys.SetDateFormat());
            txtEmployee.Text = dt.Rows[0]["Field4"].ToString() + "/" + dt.Rows[0]["Emp_Id"].ToString();
            //txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
            //txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
            hdnProductId.Value = dt.Rows[0]["Product_Id"].ToString();
            FillUnit(hdnProductId.Value);
            ddlUnit.SelectedValue = dt.Rows[0]["Unit_Id"].ToString();
            txtOrderQty.Text = Common.GetAmountDecimal(dt.Rows[0]["Qty"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            txtUnitCost.Text = Common.GetAmountDecimal(dt.Rows[0]["Penalty_Amount"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            txtAmount.Text = dt.Rows[0]["Field3"].ToString();
            txtSerialNo.Text = dt.Rows[0]["Serial_No"].ToString();
            ddlTransType.SelectedValue = dt.Rows[0]["Trn_Type"].ToString();
            ddlTransType_OnSelectedIndexChanged(null, null);
            ddlProductId.SelectedValue = dt.Rows[0]["Product_Id"].ToString();
            ddlProductName.SelectedValue = dt.Rows[0]["Product_Id"].ToString();
            Txt_Product_Name.Text = ddlProductName.SelectedItem.ToString() + "/" + dt.Rows[0]["Product_Id"].ToString();
            txtUnitCost.Text = Common.GetAmountDecimal(dt.Rows[0]["Penalty_Amount"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            txtIssuedBy.Text = dt.Rows[0]["IssuedPersonName"].ToString() + "/" + dt.Rows[0]["Issued_By"].ToString();
            chkIsReturnable.Checked = Convert.ToBoolean(dt.Rows[0]["Is_Returnable"].ToString());
            chkIsAdjusted.Checked = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
            txtRemarks.Text = dt.Rows[0]["Field2"].ToString();
            //txtEmployee.Text = "";
            txtEmployee.Focus();
            //TabContainer1.ActiveTabIndex = 1;
            // Btn_New_Click(null, null);
            if (ddlProductId.SelectedIndex > 0 && ddlTransType.SelectedValue == "In")
            {
                txtRemainQty.Text = GetRemainingQty(ddlProductId.SelectedValue);

            }
            else if (Request.QueryString["Request_Id"] != null && ddlTransType.SelectedValue == "Out")
            {
                txtRemainQty.Text = GetStockDetail(ddlProductId.SelectedValue)[0].ToString();
            }
            else
            {
                txtRemainQty.Text = "0";
            }

            updateLabel();


            if (((LinkButton)sender).ID == "lnkViewDetail")
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

        dt = objEmployeeResources.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {


            if (dt.Rows[0]["Is_Post"].ToString() == "True")
            {
                DisplayMessage("Record Posted , you can not delete");
                return;

            }
        }


        b = objEmployeeResources.DeleteRecord(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        dt = objEmployeeResources.GetFalseAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //Common Function add By Lokesh on 23-05-2015

        if (Request.QueryString["Request_Id"] != null)
        {
            dt = new DataView(dt, "Field5='1'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dt = new DataView(dt, "Field5='0'", "", DataViewRowState.CurrentRows).ToTable();
        }
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
                ImgbtnSelectAll.Visible = false;
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
                        Msg = objEmployeeResources.DeleteRecord(Session["CompId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

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
    protected void txtSalesPerson_TextChanged(object sender, EventArgs e)
    {
        Session["EmployeeId"] = "0";
        Session["ER_Contractor"] = "";
        string strEmployeeId = string.Empty;
        if (((TextBox)sender).Text != "")
        {
            if (Request.QueryString["Request_Id"] != null && ((TextBox)sender).ID == "txtEmployee")
            {
                strEmployeeId = GetContactId(((TextBox)sender).Text);
                Session["ER_Contractor"] = ((TextBox)sender).Text;
            }
            else
            {
                strEmployeeId = GetEmployeeId(((TextBox)sender).Text);
                Session["ER_Contractor"] = ((TextBox)sender).Text;
            }

            if (strEmployeeId != "" && strEmployeeId != "0")
            {
                if (((TextBox)sender).ID == "txtEmployee")
                {
                    FillProductDropDown();
                }

                Session["EmployeeId"] = strEmployeeId;
            }
            else
            {
                DisplayMessage("Select Value In Suggestions Only");
                ((TextBox)sender).Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)sender));
            }
        }

        updateLabel();
    }
    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(Session["CompId"].ToString(), strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), retval);
                if (dtEmp.Rows.Count == 0)
                { retval = ""; }
            }
            else
            { retval = ""; }
        }
        else
        { retval = ""; }
        return retval;
    }
    private string GetContactId(string strContractorName)
    {
        DataTable DtCustomer = new DataTable();
        string retval = string.Empty;
        if (strContractorName != "")
        {

            DtCustomer = objContact.GetContactByContactName(strContractorName.Split('/')[0].ToString());

            if (DtCustomer.Rows.Count > 0)
            {

                retval = strContractorName.Split('/')[1].ToString().Trim();
            }
        }

        DtCustomer.Dispose();

        return retval;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "Emp_Name like '%" + prefixText.ToString() + "%'", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }

        return txt;
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
    #region Product
    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    //{
    //    DataAccessClass ObjDa = new DataAccessClass();
    //    DataTable dt = new DataTable();
    //    Inv_ProductMaster PM = new Inv_ProductMaster();

    //    if (HttpContext.Current.Session["EmployeeId"] == null)
    //    {
    //        HttpContext.Current.Session["EmployeeId"] = "0";
    //    }


    //    if (HttpContext.Current.Session["TransType"].ToString() == "I")
    //    {
    //        dt = ObjDa.return_DataTable("select Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where  Set_EmployeeResources.Emp_Id='" + HttpContext.Current.Session["EmployeeId"].ToString() + "' and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.IsActive='True' group by Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName having sum((case when Set_EmployeeResources.Trn_Type<>'In' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0");

    //    }
    //    else
    //    {
    //        dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString());
    //    }


    //    string[] str = new string[dt.Rows.Count];
    //    if (dt.Rows.Count > 0)
    //    {
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            str[i] = dt.Rows[i]["EProductName"].ToString();

    //        }
    //    }

    //    return str;
    //}
    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    //{
    //    DataAccessClass ObjDa = new DataAccessClass();
    //    DataTable dt = new DataTable();
    //    Inv_ProductMaster PM = new Inv_ProductMaster();

    //    if (HttpContext.Current.Session["EmployeeId"] == null)
    //    {
    //        HttpContext.Current.Session["EmployeeId"] = "0";
    //    }


    //    if (HttpContext.Current.Session["TransType"].ToString() == "I")
    //    {
    //        dt = ObjDa.return_DataTable("select Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where  Set_EmployeeResources.Emp_Id='" + HttpContext.Current.Session["EmployeeId"].ToString() + "' and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.IsActive='True' group by Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName having sum((case when Set_EmployeeResources.Trn_Type<>'In' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0");

    //    }
    //    else
    //    {
    //        dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString());
    //    }
    //    string[] txt = new string[dt.Rows.Count];


    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        txt[i] = dt.Rows[i]["ProductCode"].ToString();
    //    }


    //    return txt;
    //}

    //protected void txtProductCode_TextChanged(object sender, EventArgs e)
    //{
    //    if (((TextBox)sender).Text.Trim() != "")
    //    {
    //        try
    //        {
    //            DataTable dt = new DataTable();
    //            dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((TextBox)sender).Text.Trim());
    //            if (dt == null)
    //            {
    //                DisplayMessage("Product Not Found");
    //                txtProductcode.Text = "";
    //                txtProductcode.Focus();
    //                return;
    //            }

    //            if (dt.Rows.Count != 0)
    //            {


    //                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
    //                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
    //                hdnProductId.Value = dt.Rows[0]["ProductId"].ToString();
    //                FillUnit(dt.Rows[0]["ProductId"].ToString());

    //                txtOrderQty.Text = "1";

    //            }
    //            else
    //            {
    //                FillUnit("0");
    //                txtProductcode.Text = "";
    //                txtProductName.Text = "";
    //                txtProductName.Focus();
    //            }
    //            ddlUnit.Focus();
    //        }
    //        catch
    //        {

    //        }
    //    }
    //    else
    //    {
    //        DisplayMessage("Enter Product Id");
    //        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)sender));
    //    }
    //}
    protected void ddlProductId_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        txtRemainQty.Text = "0";
        txtOrderQty.Text = "0";
        DataTable dt = new DataTable();
        //if (((DropDownList)sender).ID == "ddlProductId")
        //{
        //    ddlProductName.SelectedValue = ddlProductId.SelectedValue;
        //}
        //else
        //{
        //    ddlProductId.SelectedValue = ddlProductName.SelectedValue;
        //}

        if (ddlProductId.SelectedIndex > 0)
        {
            FillUnit(ddlProductId.SelectedValue);
        }
        else
        {
            FillUnit("0");
        }


        txtUnitCost.Text = "1";
        txtAmount.Text = "0";
        txtRemainQty.Text = "0";


        string[] str = GetStockDetail(ddlProductId.SelectedValue);
        if (Request.QueryString["Request_Id"] != null && ddlTransType.SelectedValue == "Out")
        {
            txtRemainQty.Text = str[0].ToString();
        }

        txtUnitCost.Text = str[1].ToString();

        txtAmount.Text = Common.GetAmountDecimal((Convert.ToDouble(txtOrderQty.Text) * Convert.ToDouble(txtUnitCost.Text)).ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());


        if (ddlProductId.SelectedIndex > 0 && ddlTransType.SelectedValue == "In")
        {

            txtRemainQty.Text = GetRemainingQty(ddlProductId.SelectedValue);


        }

    }
    public void FillUnit(string ProductId)
    {
        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());

    }
    public string[] GetStockDetail(string strProductId)
    {
        string[] str = new string[2];


        DataTable dtStockDetail = ObjStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), strProductId);

        if (dtStockDetail.Rows.Count > 0)
        {
            str[0] = Common.GetAmountDecimal(dtStockDetail.Rows[0]["Quantity"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            str[1] = Common.GetAmountDecimal(dtStockDetail.Rows[0]["Field2"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            hdnStockType.Value = "S";
        }
        else
        {
            hdnStockType.Value = "NS";
            str[0] = "0";
            str[1] = "1";
        }

        return str;
    }
    public string GetRemainingQty(string strProductId)
    {

        string strref_Id = "0";

        if (Request.QueryString["Request_Id"] != null)
        {
            strref_Id = "1";
        }
        chkIsReturnable.Checked = false;
        string RemainQty = "0";

        DataTable dt = objda.return_DataTable("select Set_EmployeeResources.Is_Returnable, sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end)) as RemainQty from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where  Set_EmployeeResources.Emp_Id='" + txtEmployee.Text.Split('/')[1].ToString() + "' and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.Product_Id='" + strProductId + "' and Set_EmployeeResources.Field5='" + strref_Id + "' and Set_EmployeeResources.Is_Returnable='True' and Set_EmployeeResources.IsActive='True' group by Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName,Set_EmployeeResources.Is_Returnable having sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0");

        if (dt.Rows.Count > 0)
        {
            RemainQty = Common.GetAmountDecimal(dt.Rows[0]["RemainQty"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            chkIsReturnable.Checked = Convert.ToBoolean(dt.Rows[0]["Is_Returnable"].ToString());
        }

        dt.Dispose();

        return RemainQty;

    }
    public void FillProductDropDown()
    {
        Txt_Product_Name.Text = "";
        string strref_Id = "0";
        if (Request.QueryString["Request_Id"] != null)
        {
            strref_Id = "1";
        }
        DataTable dt = new DataTable();
        if (ddlTransType.SelectedValue == "In")
        {
            dt = objda.return_DataTable("select Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where  Set_EmployeeResources.Emp_Id='" + txtEmployee.Text.Split('/')[1].ToString() + "' and Set_EmployeeResources.Field5='" + strref_Id + "' and Set_EmployeeResources.Is_Returnable='True'   and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.IsActive='True' group by Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName having sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0");
            chkIsReturnable.Enabled = false;
            chkIsAdjusted.Enabled = false;
        }
        else
        {
            if (Request.QueryString["Request_Id"] != null)
            {
                dt = objda.return_DataTable("select ProductId as Product_Id,ProductCode,EProductName from Inv_ProductMaster where  Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True'");
            }
            else
            {
                dt = objda.return_DataTable("select ProductId as Product_Id,ProductCode,EProductName from Inv_ProductMaster where PartNo='True' and Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True'");
            }
            chkIsReturnable.Enabled = true;
            chkIsAdjusted.Enabled = true;
        }


        ddlProductId.Items.Clear();
        ddlProductName.Items.Clear();

        if (dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlProductId, dt, "ProductCode", "Product_Id");
            objPageCmn.FillData((object)ddlProductName, dt, "EProductName", "Product_Id");
        }
        else
        {
            ddlProductId.Items.Insert(0, "--Select--");
            ddlProductName.Items.Insert(0, "--Select--");
        }



        dt.Dispose();
    }
    #endregion
    protected void ddlTransType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTransType.SelectedItem.ToString() == "In")
            Session["ER_TransType"] = "In";
        else
            Session["ER_TransType"] = "Out";

        if (txtEmployee.Text.Trim() == "")
        {
            DisplayMessage("Enter " + lblRefName.Text + " Name");
            txtEmployee.Focus();
            return;
        }
        updateLabel();
        FillProductDropDown();
    }

    public void updateLabel()
    {
        if (Request.QueryString["Request_Id"] != null)
        {
            if (ddlTransType.SelectedIndex == 0)
            {
                lblsysqty.Text = "Remaining Quantity";
            }
            else
            {
                lblsysqty.Text = "System Quantity";
            }
        }


    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Product_Name(string prefixText, int count, string contextKey)
    {
        DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string strref_Id = "0";
        if (HttpContext.Current.Request.QueryString["Request_Id"] != null)
            strref_Id = "1";
        DataTable Dt_Product = new DataTable();
        if (HttpContext.Current.Session["ER_TransType"].ToString() == "In")
        {
            if (HttpContext.Current.Session["ER_Contractor"].ToString() != "")
                Dt_Product = objda.return_DataTable("select Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where  Set_EmployeeResources.Emp_Id='" + HttpContext.Current.Session["ER_Contractor"].ToString().Split('/')[1].ToString() + "' and Set_EmployeeResources.Field5='" + strref_Id + "' and Set_EmployeeResources.Is_Returnable='True'   and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.IsActive='True' group by Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName having sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0");
        }
        else
        {
            if (HttpContext.Current.Session["ER_Request_Id"].ToString() != "null")
                Dt_Product = objda.return_DataTable("select ProductId as Product_Id,ProductCode,EProductName from Inv_ProductMaster where  Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True'");
            else
                Dt_Product = objda.return_DataTable("select ProductId as Product_Id,ProductCode,EProductName from Inv_ProductMaster where PartNo='True' and Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True'");
        }
        Dt_Product = new DataView(Dt_Product, "EProductName like '%" + prefixText.ToString() + "%'", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[Dt_Product.Rows.Count];
        if (Dt_Product.Rows.Count > 0)
        {
            for (int i = 0; i < Dt_Product.Rows.Count; i++)
            {

                txt[i] = Dt_Product.Rows[i]["EProductName"].ToString() + "/" + Dt_Product.Rows[i]["Product_Id"].ToString() + "";
            }
        }
        return txt;
    }
    protected void Txt_Product_Name_TextChanged(object sender, EventArgs e)
    {
        string strref_Id = "0";
        if (Request.QueryString["Request_Id"] != null)
        {
            strref_Id = "1";
        }
        DataTable Dt_Product = new DataTable();
        if (ddlTransType.SelectedValue == "In")
        {
            Dt_Product = objda.return_DataTable("select Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName from Set_EmployeeResources inner join Inv_ProductMaster on Set_EmployeeResources.Product_Id= Inv_ProductMaster.ProductId where  Set_EmployeeResources.Emp_Id='" + txtEmployee.Text.Split('/')[1].ToString() + "' and Set_EmployeeResources.Field5='" + strref_Id + "' and Set_EmployeeResources.Is_Returnable='True'   and Set_EmployeeResources.Field1='True' and Set_EmployeeResources.IsActive='True' group by Set_EmployeeResources.Product_Id,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName having sum((case when Set_EmployeeResources.Trn_Type='Out' and Set_EmployeeResources.Field6='False' then Set_EmployeeResources.Qty else 0 end)-(case when Set_EmployeeResources.Trn_Type='In' then Set_EmployeeResources.Qty else 0 end))>0");
        }
        else
        {
            if (Request.QueryString["Request_Id"] != null)
            {
                Dt_Product = objda.return_DataTable("select ProductId as Product_Id,ProductCode,EProductName from Inv_ProductMaster where  Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True'");
            }
            else
            {
                Dt_Product = objda.return_DataTable("select ProductId as Product_Id,ProductCode,EProductName from Inv_ProductMaster where PartNo='True' and Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True'");
            }
        }
        if (Dt_Product.Rows.Count > 0)
        {
            Dt_Product = new DataView(Dt_Product, "EProductName = '" + Get_String_Before_Char(Txt_Product_Name.Text, '/') + "'", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();
            if (Dt_Product == null || Dt_Product.Rows.Count == 0)
            {
                DisplayMessage("Product name not exist in database");
                Txt_Product_Name.Text = "";
                Txt_Product_Name.Focus();
                return;
            }
            else if (Dt_Product.Rows.Count > 0)
            {
                Txt_Product_Name.Text = Dt_Product.Rows[0]["EProductName"].ToString() + "/" + Dt_Product.Rows[0]["Product_Id"].ToString();
                ddlProductName.SelectedValue = Dt_Product.Rows[0]["Product_Id"].ToString();
                ddlProductId.SelectedValue = Dt_Product.Rows[0]["Product_Id"].ToString();
                ddlProductId_OnSelectedIndexChanged(null, null);
            }
        }
        else
        {
            DisplayMessage("Product name not exist in database");
            Txt_Product_Name.Text = "";
            Txt_Product_Name.Focus();
            return;
        }
        Dt_Product.Dispose();
    }
    public static string Get_String_Before_Char(string input, char pivot)
    {
        int index = input.IndexOf(pivot);
        if (index >= 0)
        {
            return input.Substring(0, index);
        }
        return input;
    }
}