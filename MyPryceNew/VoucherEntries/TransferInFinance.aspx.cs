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
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using net.webservicex.www;
using System.Data.SqlClient;

public partial class VoucherEntries_TransferInFinance : System.Web.UI.Page
{
    Pay_Employee_Loan ObjLoan = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ChartOfAccount objCOA = null;
    SystemParameter ObjSysParam = null;
    Common cmn = null;
    CurrencyMaster objCurrency = null;
    LocationMaster ObjLocation = null;
    Set_DocNumber objDocNo = null;
    EmployeeMaster objEmployee = null;
    Ems_ContactMaster objContact = null;
    DepartmentMaster objDepartment = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    Ac_ParameterMaster objAccParameter = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Set_Suppliers objSupplier = null;
    Set_CustomerMaster ObjCoustmer = null;
    Ac_CashFlow_Detail objCashFlowDetail = null;
    PegasusDataAccess.DataAccessClass objDA = null;
    IT_ObjectEntry objObjectEntry = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;

    public const int grdDefaultColCount = 4;
    private const string strPageName = "TransferInFinance";

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    string strPost = string.Empty;
    string strCancel = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objDepartment = new DepartmentMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objAccParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objCashFlowDetail = new Ac_CashFlow_Detail(Session["DBConnection"].ToString());
        objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../VoucherEntries/TransferInFinance.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            AllPageCode(clsPagePermission);
            FillLocationList();
            ddlOption.SelectedIndex = 2;
            btnList_Click(null, null);
            FillGrid();
            CalendarExtender_txtChequeIssueDate.Format = Session["DateFormat"].ToString();
            CalendarExtender_ChequeClearDate.Format = Session["DateFormat"].ToString();
            //AllPageCode();
            txtToLocation.Text = ObjLocation.GetLocationMasterByLocationId(strLocationId).Rows[0]["Location_Name"].ToString() + "/" + ObjLocation.GetLocationMasterByLocationId(strLocationId).Rows[0]["Location_Id"].ToString();

            try
            {
                txtDepartment.Text = objDepartment.GetDepartmentMasterById(Session["DepartmentId"].ToString()).Rows[0]["Dep_Name"].ToString() + "/" + objDepartment.GetDepartmentMasterById(Session["DepartmentId"].ToString()).Rows[0]["Dep_Id"].ToString();
            }
            catch
            {
                txtDepartment.Text = "";
            }

            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "184", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
            //FillCurrency();
            string strCurrencyId = string.Empty;

            try
            {
                strCurrencyId = Session["LocCurrencyId"].ToString();
            }
            catch
            {

            }

            FillDetailCurrency();

            if (strCurrencyId != "0" && strCurrencyId != "")
            {
                //ddlCurrency.SelectedValue = strCurrencyId;
                ddlDCurrency.SelectedValue = strCurrencyId;
            }

            //txtExchangeRate.Text = "1";
            txtDExchangeRate.Text = "1";
            txtReference.Text = "0";
            rbCashPayment.Checked = true;
            ddlFieldName_SelectedIndexChanged(sender, e);
            getPageControlsVisibility();
        }
    }
    private void FillDetailCurrency()
    {
        DataTable dsDCurrency = null;
        dsDCurrency = objCurrency.GetCurrencyMaster();
        if (dsDCurrency.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlDCurrency, dsDCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlDCurrency.Items.Insert(0, "--Select--");
            ddlDCurrency.SelectedIndex = 0;
        }
    }
    protected void GvVoucher_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvVoucher.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Trans_InFinance"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");

        string temp = string.Empty;

        for (int i = 0; i < GvVoucher.Rows.Count; i++)
        {
            Label lblconid = (Label)GvVoucher.Rows[i].FindControl("lblgvTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvVoucher.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }

        string strCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), ddlLocationList.SelectedValue).Rows[0]["Currency_id"].ToString();
        foreach (GridViewRow gv in GvVoucher.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvExchangerate");
            lblgvVoucherAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
        //AllPageCode();
    }
    protected void GvVoucher_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = (DataTable)Session["dtFilter_Trans_InFinance"];
        //dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId);
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        //Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucher, dt, "", "");
        lblSelectedRecord.Text = "";
        string strCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), ddlLocationList.SelectedValue).Rows[0]["Currency_id"].ToString();
        foreach (GridViewRow gv in GvVoucher.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvExchangerate");

            lblgvVoucherAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
        //AllPageCode();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlVoucherValue.Visible == true)
            {
                if (ddlVoucherValue.SelectedValue != "--Select--")
                {
                    if (ddlOption.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + ddlVoucherValue.SelectedValue + "'";
                    }
                    else if (ddlOption.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + ddlVoucherValue.SelectedValue + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + ddlVoucherValue.SelectedValue + "%'";
                    }
                }
                else
                {
                    //DisplayMessage("Select any Voucher Type to Search");
                    //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlVoucherValue);
                    //return;
                }
            }
            else if (txtValue.Visible == true)
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

            string[] strDateValidation = getDateFilterCondition(txtFromDate, txtToDate);
            if (strDateValidation[0] == "true")
            {
                condition = condition != string.Empty ? condition + " and " + strDateValidation[1] : strDateValidation[1];
            }
            else if (strDateValidation[0] == "false")
            {
                DisplayMessage(strDateValidation[1]);
                return;
            }


            if (condition == string.Empty)
            {
                DisplayMessage("Select any Voucher Type to Search");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlVoucherValue);
                return;
            }

            DataTable dtVoucher = (DataTable)Session["dtVoucher"];
            DataView view = new DataView(dtVoucher, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            if (view.ToTable().Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvVoucher, view.ToTable(), "", "");
                Session["dtFilter_Trans_InFinance"] = view.ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
                //AllPageCode();
            }
            else
            {
                GvVoucher.DataSource = null;
                GvVoucher.DataBind();
                DisplayMessage("You have no record according to searching criteria");
            }
        }
        string strCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), ddlLocationList.SelectedValue).Rows[0]["Currency_id"].ToString();
        foreach (GridViewRow gv in GvVoucher.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvExchangerate");

            lblgvVoucherAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
        txtValue.Focus();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        ImgButtonDelete.Visible = clsPagePermission.bDelete;
        btnVoucherSave.Visible = btnPostInFinance.Visible = clsPagePermission.bAdd;
        GvVoucher.Columns[0].Visible = clsPagePermission.bEdit;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        btnRefresh.Visible = ImgbtnSelectAll.Visible = clsPagePermission.bRestore;
        btnPostInFinance.ValidationGroup = "Grid";
        btnControlsSetting.Visible = false;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    protected string[] getDateFilterCondition(TextBox txtFromDate, TextBox txtToDate)
    {
        string[] strResult = new string[2]; // here we use first element true-valid date,false-error in date,skip-need to skip this filter
        DateTime dtToDate = new DateTime();
        DateTime dtFromDate = new DateTime();
        if (txtFromDate.Text == "" || txtToDate.Text == "")
        {
            strResult[0] = "skip";
            return strResult;
        }
        try
        {
            ObjSysParam.getDateForInput(txtFromDate.Text);
            Convert.ToDateTime(txtFromDate.Text);
            dtFromDate = Convert.ToDateTime(txtFromDate.Text);
            dtFromDate = new DateTime(dtFromDate.Year, dtFromDate.Month, dtFromDate.Day, 23, 59, 1);
            ObjSysParam.getDateForInput(txtToDate.Text);
            Convert.ToDateTime(txtToDate.Text);
            dtToDate = Convert.ToDateTime(txtToDate.Text);
            dtToDate = new DateTime(dtToDate.Year, dtToDate.Month, dtToDate.Day, 23, 59, 1);

        }
        catch
        {
            strResult[0] = "false";
            strResult[1] = "Enter Voucher Date in format " + Session["DateFormat"].ToString() + "";
            return strResult;
        }


        //for Check Financial Year with FromDate
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()) || !Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtToDate.Text), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            strResult[0] = "false";
            strResult[1] = "Only Current financial year date is allowed";
            return strResult;
        }

        if (DateTime.Parse(txtFromDate.Text) >= dtToDate)
        {
            strResult[0] = "false";
            strResult[1] = "Form date should be greate then to date";
            return strResult;
        }

        strResult[0] = "true";
        strResult[1] = "Voucher_Date >= '" + txtFromDate.Text + "' and  Voucher_Date <= '" + dtToDate + "'";
        return strResult;
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        ddlFieldName_SelectedIndexChanged(sender, e);
        txtFromDate.Text = "";
        txtToDate.Text = "";
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedValue == "Voucher_Type")
        {
            ddlVoucherValue.SelectedValue = "--Select--";
            ddlVoucherValue.Visible = true;
            txtValue.Visible = false;
        }
        else
        {
            ddlVoucherValue.Visible = false;
            txtValue.Visible = true;
        }
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvVoucher.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvVoucher.Rows.Count; i++)
        {
            ((CheckBox)GvVoucher.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvVoucher.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvVoucher.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvVoucher.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
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
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvVoucher.Rows[index].FindControl("lblgvTransId");
        Label lblrefType = (Label)GvVoucher.Rows[index].FindControl("lblgvrefType");
        Label lblApprovalStatus = (Label)GvVoucher.Rows[index].FindControl("lblApprovalStatus");
        if (lblApprovalStatus.Text == "Pending" || lblApprovalStatus.Text == "Rejected")
        {
            string strMessage = lblApprovalStatus.Text == "Rejected" ? "Voucher has been rejected, so you can not post it" : "Voucher is peding for approval, so you can post it";
            DisplayMessage(strMessage);
            ((CheckBox)GvVoucher.Rows[index].FindControl("chkSelect")).Checked = false;
            return;
        }


        if (((CheckBox)GvVoucher.Rows[index].FindControl("chkSelect")).Checked)
        {
            if (lblrefType.Text.Trim() == "Loan_Deduction" || lblrefType.Text.Trim() == "Pay_Employe_Month")
            {
                empidlist += GetRelatedVoucherId(lb.Text.Trim().ToString()) + ",";
            }
            else
            {
                empidlist += lb.Text.Trim().ToString() + ",";
            }
            lblSelectedRecord.Text += empidlist;
        }
        else
        {

            if (lblrefType.Text.Trim() == "Loan_Deduction" || lblrefType.Text.Trim() == "Pay_Employe_Month")
            {
                empidlist = GetRelatedVoucherId(lb.Text.ToString().Trim());
            }
            else
            {
                empidlist = lb.Text.ToString().Trim();
            }

            if (!lblSelectedRecord.Text.Split(',').Contains(lb.Text.ToString().Trim()))
            {
                lblSelectedRecord.Text += empidlist;
            }

            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (!empidlist.Split(',').Contains(item))
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

        if (lblrefType.Text.Trim() == "Loan_Deduction" || lblrefType.Text.Trim() == "Pay_Employe_Month")
        {
            for (int i = 0; i < GvVoucher.Rows.Count; i++)
            {
                Label lblconid = (Label)GvVoucher.Rows[i].FindControl("lblgvTransId");
                ((CheckBox)GvVoucher.Rows[i].FindControl("chkSelect")).Checked = false;
                string[] split = lblSelectedRecord.Text.Split(',');

                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvVoucher.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }


    }

    public string GetRelatedVoucherId(string strId)
    {
        string strTransId = string.Empty;


        string strsql = "select Field5 from ac_voucher_header where Trans_Id=" + strId + "";
        strTransId = objDA.return_DataTable(strsql).Rows[0][0].ToString();

        if (strTransId != "")
        {
            strId = strTransId;
        }

        strsql = "(SELECT STUFF((SELECT DISTINCT ',' + RTRIM(ac_voucher_header.Trans_Id) FROM ac_voucher_header WHERE  IsActive='True' and  (Trans_id =" + strId + " or Field5=" + strId + ")  FOR xml PATH ('')), 1, 1, ''))";
        strTransId = objDA.return_DataTable(strsql).Rows[0][0].ToString();

        return strTransId;
    }
    public string GetVoucherAmount(string strVoucherId)
    {
        string strVoucherAmount = string.Empty;
        try
        {

            DataTable dtVoucherD = objVoucherDetail.GetVoucherDetailByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVoucherId);
            if (dtVoucherD.Rows.Count > 0)
            {
                try
                {
                    double debitamount = 0;

                    for (int i = 0; i < dtVoucherD.Rows.Count; i++)
                    {
                        if (dtVoucherD.Rows[i]["Ref_Type"].ToString() == "PR")
                        {
                            debitamount += Convert.ToDouble(dtVoucherD.Rows[i]["Debit_Amount"].ToString()) / Convert.ToDouble(dtVoucherD.Rows[i]["Exchange_Rate"].ToString());
                        }
                        else
                            debitamount += Convert.ToDouble(dtVoucherD.Rows[i]["Debit_Amount"].ToString());
                    }
                    if (debitamount != 0)
                    {
                        strVoucherAmount = debitamount.ToString();
                    }
                }
                catch
                {
                    strVoucherAmount = "0.00";
                }
            }
            else
            {
                strVoucherAmount = "0";
            }
            return strVoucherAmount;
        }
        catch (Exception ex)
        {
            strVoucherAmount = "0";
            return strVoucherAmount;
        }

    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
        }
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    public void FillLocationList()
    {
        DataTable dtLoc = ObjLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)ddlLocationList, dtLoc, "Location_Name", "Location_Id");
            ddlLocationList.SelectedValue = Session["LocId"].ToString();
        }
        else
        {
            ddlLocationList.Items.Insert(0, "--Select--");
            ddlLocationList.SelectedIndex = 0;
        }
    }
    private void FillGrid()
    {
        if (ddlLocationList.SelectedValue != "0" && ddlLocationList.SelectedValue != "")
        {
            strLocationId = ddlLocationList.SelectedValue;
        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }

        DataTable dtBrand = new DataView(objVoucherHeader.GetRecordforReconcileFinance(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString()), "", "Voucher_Date DESC, Trans_id DESC", DataViewRowState.CurrentRows).ToTable();
        //dtBrand = new DataView(dtBrand, "Field3='' or Field3='Approved'", "", DataViewRowState.CurrentRows).ToTable();

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";

        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            Session["dtVoucher"] = dtBrand;
            Session["dtFilter_Trans_InFinance"] = dtBrand;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucher, dtBrand, "", "");
        }
        else
        {
            GvVoucher.DataSource = null;
            GvVoucher.DataBind();
        }

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count.ToString() + "";
        //AllPageCode();
        string strCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), strLocationId).Rows[0]["Currency_Id"].ToString();
        foreach (GridViewRow gv in GvVoucher.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvExchangerate");
            lblgvVoucherAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
    }
    protected void btnBindLocationList_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
    }
    protected string GetEmployeeNameByEmpCode(string strEmployeeCode)
    {
        string strEmpName = string.Empty;
        if (strEmployeeCode != "0" && strEmployeeCode != "")
        {
            if (strEmployeeCode == "superadmin")
            {
                strEmpName = "Admin";
            }
            else
            {
                DataTable dtEmp = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeCode);
                if (dtEmp.Rows.Count > 0)
                {
                    strEmpName = dtEmp.Rows[0]["Emp_Name"].ToString();
                }
            }
        }
        else
        {
            strEmpName = "";
        }
        return strEmpName;
    }

    protected string Created_By_Emp_Name(string strEmployeeCode)
    {
        string strEmpName = string.Empty;
        if (strEmployeeCode != "0" && strEmployeeCode != "")
        {
            if (strEmployeeCode == "superadmin")
            {
                strEmpName = "Admin";
            }
            else
            {
                string strsql = "Select Case When Set_UserMaster.Emp_Id !='0' then Emp_Name else 'Superadmin' End as Emp_Name From Set_EmployeeMaster Right join Set_UserMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id Where Set_UserMaster.User_Id='" + strEmployeeCode + "'";
                DataTable dtEmp = objDA.return_DataTable(strsql);
                //DataTable dtEmp = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeCode);
                if (dtEmp.Rows.Count > 0)
                {
                    strEmpName = dtEmp.Rows[0]["Emp_Name"].ToString();
                }
            }
        }
        else
        {
            strEmpName = "";
        }
        return strEmpName;
    }

    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCName = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCName.Rows.Count > 0)
            {
                strCurrencyName = dtCName.Rows[0]["Currency_Name"].ToString();
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblSelectedRecord.Text = "";
        DataTable dtVoucher = (DataTable)Session["dtFilter_Trans_InFinance"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtVoucher.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvVoucher.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvVoucher.Rows[i].FindControl("lblgvTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvVoucher.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            //lblSelectedRecord.Text = "";
            //DataTable dtVocher = (DataTable)Session["dtInactive"];
            ////Common Function add By Lokesh on 23-05-2015
            //objPageCmn.FillData((object)GvVoucher, dtVocher, "", "");
            //ViewState["Select"] = null;


            // Code change by Ghanshyam Suthar on 25/07/2017
            ViewState["Select"] = null;
            foreach (DataRow dr in dtVoucher.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvVoucher.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvVoucher.Rows[i].FindControl("lblgvTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvVoucher.Rows[i].FindControl("chkSelect")).Checked = false;
                        }
                    }
                }
            }
        }
    }
    protected void btnPostInFinance_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    DataTable Dt_Sal_Loan = objVoucherHeader.Get_Relationship_Voucher_Header(lblSelectedRecord.Text.Split(',')[j].Trim(), "1");
                    if (Dt_Sal_Loan != null && Dt_Sal_Loan.Rows.Count > 0)
                    {
                        if (Dt_Sal_Loan.Rows.Count > 1)
                        {
                            foreach (DataRow Dr_Loan in Dt_Sal_Loan.Rows)
                            {
                                if (Convert.ToBoolean(Dr_Loan["IsActive"].ToString()) == false)
                                {
                                    if (Dr_Loan["Field5"].ToString() == "")
                                    {
                                        DisplayMessage("Salary Voucher is Deleted, So it cannot be Post !");
                                        return;
                                    }
                                    else
                                    {
                                        DisplayMessage("Loan Voucher is Deleted, So it cannot be Post !");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        string strVoucherDetailNumber = string.Empty;
        string strVoucherDetailNumberFYC = string.Empty;
        string strCashflowPosted = string.Empty;
        if (lblSelectedRecord.Text != "")
        {
            //for Detail Record Not Exists
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim());
                    if (dtVoucherHeader.Rows.Count > 0)
                    {
                        if (Common.IsFinancialyearAllow(Convert.ToDateTime(dtVoucherHeader.Rows[0]["Voucher_Date"].ToString()), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                        {
                            DataTable dtVoucherDetail = objVoucherDetail.GetSumRecordByVoucherNo(lblSelectedRecord.Text.Split(',')[j].Trim());
                            if (dtVoucherDetail.Rows.Count > 0)
                            {

                                double sumDebit = 0;
                                double sumCredit = 0;
                                try
                                {
                                    sumDebit = Convert.ToDouble(Common.GetAmountDecimal(dtVoucherDetail.Rows[0]["DebitTotal"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()));
                                }
                                catch
                                {

                                }


                                try
                                {
                                    sumCredit = Convert.ToDouble(Common.GetAmountDecimal(dtVoucherDetail.Rows[0]["CreditTotal"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()));
                                }
                                catch
                                {

                                }





                                if (sumDebit == sumCredit)
                                {

                                }
                                else
                                {
                                    if (strVoucherDetailNumber == "")
                                    {
                                        strVoucherDetailNumber = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                    }
                                    else
                                    {
                                        strVoucherDetailNumber = strVoucherDetailNumber + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (strVoucherDetailNumber == "")
                                {
                                    strVoucherDetailNumber = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                }
                                else
                                {
                                    strVoucherDetailNumber = strVoucherDetailNumber + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (strVoucherDetailNumberFYC == "")
                            {
                                strVoucherDetailNumberFYC = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                            }
                            else
                            {
                                strVoucherDetailNumberFYC = strVoucherDetailNumberFYC + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                            }
                        }
                    }
                }
            }

            if (strVoucherDetailNumberFYC != "")
            {
                DisplayMessage("Log In Financial year not allowing to perform this action Voucher Number is :- " + strVoucherDetailNumberFYC + "");
                return;
            }

            //for Cash flow Posted
            //For Cash flow Account

            string strAccountId = string.Empty;
            DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CashFlowAccount");
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


            for (int d = 0; d < lblSelectedRecord.Text.Split(',').Length; d++)
            {
                if (lblSelectedRecord.Text.Split(',')[d] != "")
                {
                    DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[d].Trim());
                    if (dtVoucherHeader.Rows.Count > 0)
                    {
                        DataTable dtVoucherDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[d].Trim());
                        if (dtVoucherDetail.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtVoucherDetail.Rows.Count; i++)
                            {

                                if (strAccountId.Split(',').Contains(dtVoucherDetail.Rows[i]["Account_No"].ToString()))
                                {
                                    DataTable dtCashflowDetail = objCashFlowDetail.GetCashFlowDetailForAcountsEntry(StrCompId, StrBrandId, strLocationId);
                                    if (dtCashflowDetail.Rows.Count > 0)
                                    {
                                        string strCashFinalDate = dtCashflowDetail.Rows[0][0].ToString();
                                        if (strCashFinalDate != "")
                                        {
                                            DateTime dtFinalDate = DateTime.Parse(strCashFinalDate);

                                            if (dtFinalDate >= DateTime.Parse(dtVoucherHeader.Rows[0]["Voucher_Date"].ToString()))
                                            {
                                                if (strCashflowPosted == "")
                                                {
                                                    strCashflowPosted = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                                }
                                                else
                                                {
                                                    strCashflowPosted = strCashflowPosted + "," + dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (strCashflowPosted != "")
        {
            DisplayMessage("Your Cashflow is Posted for That Voucher Numbers :- " + strCashflowPosted + "");
            return;
        }

        if (strVoucherDetailNumber != "")
        {
            DisplayMessage("Your Detail Record is Not Proper Please check that Records :- " + strVoucherDetailNumber + "");
            return;
        }


        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();
            try
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        b = objVoucherHeader.UpdateVoucherReconciledFinance(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        if (b != 0)
                        {
                            objAgeingDetail.insert_Ageing(StrCompId, StrBrandId, strLocationId, Session["EmpId"].ToString(), Session["UserId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), trns);

                        }
                    }
                }

                if (b != 0)
                {
                }

                trns.Commit();
                lblSelectedRecord.Text = "";
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();


                FillGrid();
                btnRefreshReport_Click(null, null);
                //AllPageCode();
                DisplayMessage("Record Posted Successfully");
            }
            catch (Exception ex)
            {
                DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
                lblSelectedRecord.Text = "";
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
        else
        {
            DisplayMessage("Please Select Record");
        }
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;
        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            strLocationId = dtVoucherEdit.Rows[0]["Location_id"].ToString();
            string strVoucherLocCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), strLocationId).Rows[0]["Currency_Id"].ToString();
            string strFinanceCode = dtVoucherEdit.Rows[0]["Finance_Code"].ToString();
            if (strFinanceCode != "0" && strFinanceCode != "")
            {
                DataTable dtFy = objFYI.GetInfoByTransId(StrCompId, strFinanceCode);
                if (dtFy.Rows.Count > 0)
                {
                    txtVFinanceCode.Text = dtFy.Rows[0]["Finance_Code"].ToString() + "/" + dtFy.Rows[0]["Trans_Id"].ToString();
                }
            }

            string strToLocationId = dtVoucherEdit.Rows[0]["Location_id"].ToString();
            if (strToLocationId != "0" && strToLocationId != "")
            {
                txtVToLocation.Text = GetLocationName(strToLocationId) + "/" + strToLocationId;
            }
            else
            {
                txtVToLocation.Text = "";
            }

            string strDepartmentId = dtVoucherEdit.Rows[0]["Department_Id"].ToString();
            if (strDepartmentId != "0" && strDepartmentId != "")
            {
                txtVDepartment.Text = GetDepartmentName(strDepartmentId) + "/" + strDepartmentId;
            }
            else
            {
                txtVDepartment.Text = "";
            }

            txtVVoucherNo.Text = dtVoucherEdit.Rows[0]["Voucher_No"].ToString();
            txtVVoucherDate.Text = Convert.ToDateTime(dtVoucherEdit.Rows[0]["Voucher_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

            strCashCheque = dtVoucherEdit.Rows[0]["Field2"].ToString();
            if (strCashCheque == "Cash")
            {
                rbVCashPayment.Checked = true;
                rbVChequePayment.Checked = false;
                rbVCashPayment_CheckedChanged(null, null);
            }
            else if (strCashCheque == "Cheque")
            {
                rbVChequePayment.Checked = true;
                rbVCashPayment.Checked = false;
                rbVCashPayment_CheckedChanged(null, null);
            }
            else if (strCashCheque == "")
            {
                rbVCashPayment.Checked = true;
                rbVCashPayment_CheckedChanged(null, null);
            }

            strVoucherType = dtVoucherEdit.Rows[0]["Voucher_Type"].ToString();
            if (strVoucherType != "0")
            {
                ddlVVoucherType.SelectedValue = strVoucherType;
                ddlVVoucherType.Enabled = false;
            }
            else
            {
                ddlVVoucherType.SelectedValue = "--Select--";
            }

            string strChequeIssueDate = dtVoucherEdit.Rows[0]["Cheque_Issue_Date"].ToString();
            if (strChequeIssueDate != "" && strChequeIssueDate != "1/1/1800")
            {
                txtVChequeIssueDate.Text = Convert.ToDateTime(strChequeIssueDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            string strChequeClearDate = dtVoucherEdit.Rows[0]["Cheque_Clear_Date"].ToString();
            if (strChequeClearDate != "" && strChequeClearDate != "1/1/1800")
            {
                txtVChequeClearDate.Text = Convert.ToDateTime(strChequeClearDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            txtVChequeNo.Text = dtVoucherEdit.Rows[0]["Cheque_No"].ToString();

            txtVReference.Text = dtVoucherEdit.Rows[0]["RefrenceNo"].ToString();
            string strCurrencyId = dtVoucherEdit.Rows[0]["Currency_Id"].ToString();
            //ddlCurrency.SelectedValue = strCurrencyId;

            //txtExchangeRate.Text = dtVoucherEdit.Rows[0]["Exchange_Rate"].ToString();
            //txtNarration.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();

            //Add Child Concept
            GvVDetail.DataSource = null;
            GvVDetail.DataBind();

            string strCurrency = Session["LocCurrencyId"].ToString();
            DataTable dtDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(e.CommandArgument.ToString());
            if (dtDetail.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GvVDetail, dtDetail, "", "");

                //For Total
                double sumDebit = 0;
                double sumCredit = 0;
                foreach (GridViewRow gvr in GvVDetail.Rows)
                {
                    Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
                    Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");
                    Label lblgvFrgnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
                    Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");

                    if (lblgvDebitAmt.Text == "")
                    {
                        lblgvDebitAmt.Text = "0";
                    }
                    sumDebit += Convert.ToDouble(lblgvDebitAmt.Text);

                    if (lblgvCreditAmt.Text == "")
                    {
                        lblgvCreditAmt.Text = "0";
                    }
                    sumCredit += Convert.ToDouble(lblgvCreditAmt.Text);

                    lblgvDebitAmt.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblgvDebitAmt.Text);
                    lblgvCreditAmt.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblgvCreditAmt.Text);
                    lblgvFrgnAmt.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblgvFrgnAmt.Text);
                    lblgvExchangeRate.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblgvExchangeRate.Text);
                }

                Label lblDebitTotal = (Label)(GvVDetail.FooterRow.FindControl("lblgvDebitTotal"));
                Label lblCreditTotal = (Label)(GvVDetail.FooterRow.FindControl("lblgvCreditTotal"));

                lblDebitTotal.Text = sumDebit.ToString();
                lblCreditTotal.Text = sumCredit.ToString();

                lblDebitTotal.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblDebitTotal.Text);
                lblCreditTotal.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblCreditTotal.Text);
            }
        }

        //pnl1.Visible = true;
        //pnl2.Visible = true;


        // ModalPopupExtender1.Show();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Voucher_Detail_Popup()", true);

    }

    protected string GetEmployeeName(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterById(StrCompId, strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
            }
        }
        else
        {
            strEmployeeName = "";
        }
        return strEmployeeName;
    }
    protected string GetLocationName(string strLocationId)
    {
        string strLocationName = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocName = ObjLocation.GetLocationMasterById(StrCompId, strLocationId);
            if (dtLocName.Rows.Count > 0)
            {
                strLocationName = dtLocName.Rows[0]["Location_Name"].ToString();
            }
        }
        else
        {
            strLocationName = "";
        }
        return strLocationName;
    }
    protected void btnCancelPopLeave_Click(object sender, EventArgs e)
    {
        //pnl1.Visible = false;
        //pnl2.Visible = false;
        Reset();
    }
    protected void btnClosePanel_Click1(object sender, EventArgs e)
    {
        //pnl1.Visible = false;
        //pnl2.Visible = false;
        Reset();
    }
    public void Reset()
    {
        FillGrid();
        txtVVoucherDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        ddlVVoucherType.Enabled = true;
        ddlVVoucherType.SelectedValue = "--Select--";
        txtVReference.Text = "0";
        PnlNewContant.Enabled = true;

        //txtExchangeRate.Text = "";
        txtVChequeIssueDate.Text = "";
        txtVChequeClearDate.Text = "";
        txtVChequeNo.Text = "";

        GvVDetail.DataSource = null;
        GvVDetail.DataBind();

        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";

        rbVCashPayment.Checked = true;
        rbVChequePayment.Checked = false;
        rbVCashPayment_CheckedChanged(null, null);

    }
    protected void rbVCashPayment_CheckedChanged(object sender, EventArgs e)
    {
        if (rbVCashPayment.Checked == true)
        {
            trVCheque1.Visible = false;
            trVCheque2.Visible = false;
            txtVChequeIssueDate.Text = "";
            txtVChequeClearDate.Text = "";
            txtVChequeNo.Text = "";
        }
        else if (rbVChequePayment.Checked == true)
        {
            trVCheque1.Visible = true;
            trVCheque2.Visible = true;
        }
    }
    protected string GetCustomerNameByContactId(string strContactId)
    {
        string strCustomerName = string.Empty;
        if (strContactId != "0" && strContactId != "")
        {
            DataTable dtAccName = objContact.GetContactTrueById(strContactId);

            if (dtAccName.Rows.Count > 0)
            {
                strCustomerName = dtAccName.Rows[0]["Name"].ToString();
            }
        }
        else
        {
            strCustomerName = "";
        }
        return strCustomerName;
    }

    protected string GetDepartmentName(string strDepartmentId)
    {
        string strDepartmentName = string.Empty;
        if (strDepartmentId != "0" && strDepartmentId != "")
        {
            DataTable dtDepartmentName = objDepartment.GetDepartmentMasterById(strDepartmentId);
            if (dtDepartmentName.Rows.Count > 0)
            {
                strDepartmentName = dtDepartmentName.Rows[0]["Dep_Name"].ToString();
            }
        }
        else
        {
            strDepartmentName = "";
        }
        return strDepartmentName;
    }
    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            DataTable dtAccName = objCOA.GetCOAByTransId(StrCompId, strAccountNo);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString();
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }

    #region New & Bin Section
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (hdnVoucherId.Value == "0" || hdnVoucherId.Value == "")
        {
            DisplayMessage("You Cant Enter New Record. Update Only");
            return;
        }

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        Calender_VoucherDate.Format = Session["DateFormat"].ToString();
        txtVoucherDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
    }





    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable Dt_Emp_Payment_Loan = ObjLoan.Get_Employee_Loan("0", "0", e.CommandArgument.ToString(), "True", "True", "1");
        if (Dt_Emp_Payment_Loan != null && Dt_Emp_Payment_Loan.Rows.Count > 0)
        {
            DisplayMessage("Loan Payment is Approved So It cannot be Edited !");
            return;
        }

        PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        string strVoucherType = string.Empty;
        string strCashCheque = string.Empty;

        hdnVoucherId.Value = e.CommandArgument.ToString();
        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(e.CommandArgument.ToString());
        DataTable dtDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(hdnVoucherId.Value);
        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (dtVoucherEdit.Rows[0]["Field3"].ToString() == "Approved" || dtVoucherEdit.Rows[0]["Field3"].ToString() == "Rejected")
            {
                DisplayMessage("Voucher has been " + dtVoucherEdit.Rows[0]["Field3"].ToString() + ", so you can not edit it");
                return;
            }

            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is Posted, So record cannot be Edited");
                return;
            }

            string strVoucherDate = Convert.ToDateTime(dtVoucherEdit.Rows[0]["Voucher_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            string strVoucherNo = dtVoucherEdit.Rows[0]["Voucher_No"].ToString();

            if (objAccParameterLocation.ValidateVoucherForCashFlow(Session["CompId"].ToString(), e.CommandArgument.ToString()) == false)
            {
                DisplayMessage("Cashflow has been posted so you can not edit it");
                return;
            }

            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

            string strFinanceCode = dtVoucherEdit.Rows[0]["Finance_Code"].ToString();
            if (strFinanceCode != "0" && strFinanceCode != "")
            {
                DataTable dtFy = objFYI.GetInfoByTransId(StrCompId, strFinanceCode);
                if (dtFy.Rows.Count > 0)
                {
                    txtFinanceCode.Text = dtFy.Rows[0]["Finance_Code"].ToString() + "/" + dtFy.Rows[0]["Trans_Id"].ToString();
                }
            }

            string strToLocationId = dtVoucherEdit.Rows[0]["Location_To"].ToString();
            if (strToLocationId != "0" && strToLocationId != "")
            {
                txtToLocation.Text = GetLocationName(strToLocationId) + "/" + strToLocationId;
            }
            else
            {
                txtToLocation.Text = "";
            }

            string strDepartmentId = dtVoucherEdit.Rows[0]["Department_Id"].ToString();
            if (strDepartmentId != "0" && strDepartmentId != "")
            {
                txtDepartment.Text = GetDepartmentName(strDepartmentId) + "/" + strDepartmentId;
            }
            else
            {
                txtDepartment.Text = "";
            }
            strLocationId = dtVoucherEdit.Rows[0]["Location_id"].ToString();
            string strVoucherLocCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), strLocationId).Rows[0]["Currency_Id"].ToString();
            txtVoucherNo.Text = dtVoucherEdit.Rows[0]["Voucher_No"].ToString();
            txtVoucherNo.ReadOnly = true;
            txtVoucherDate.Text = Convert.ToDateTime(dtVoucherEdit.Rows[0]["Voucher_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

            strCashCheque = dtVoucherEdit.Rows[0]["Field2"].ToString();
            if (strCashCheque == "Cash")
            {
                rbCashPayment.Checked = true;
                rbChequePayment.Checked = false;
                rbCashPayment_CheckedChanged(null, null);
            }
            else if (strCashCheque == "Cheque")
            {
                rbChequePayment.Checked = true;
                rbCashPayment.Checked = false;
                rbCashPayment_CheckedChanged(null, null);
            }
            else if (strCashCheque == "")
            {
                rbCashPayment.Checked = true;
                rbCashPayment_CheckedChanged(null, null);
            }

            strVoucherType = dtVoucherEdit.Rows[0]["Voucher_Type"].ToString();
            if (strVoucherType != "0")
            {
                ddlVoucherType.SelectedValue = strVoucherType;
                ddlVoucherType.Enabled = false;
            }
            else
            {
                ddlVoucherType.SelectedValue = "--Select--";
            }

            string strChequeIssueDate = dtVoucherEdit.Rows[0]["Cheque_Issue_Date"].ToString();
            if (strChequeIssueDate != "" && strChequeIssueDate != "1/1/1800")
            {
                txtChequeIssueDate.Text = Convert.ToDateTime(strChequeIssueDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            string strChequeClearDate = dtVoucherEdit.Rows[0]["Cheque_Clear_Date"].ToString();
            if (strChequeClearDate != "" && strChequeClearDate != "1/1/1800")
            {
                txtChequeClearDate.Text = Convert.ToDateTime(strChequeClearDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            txtChequeNo.Text = dtVoucherEdit.Rows[0]["Cheque_No"].ToString();

            txtReference.Text = dtVoucherEdit.Rows[0]["Narration"].ToString();
            hdnRef_Id.Value = dtVoucherEdit.Rows[0]["Ref_Id"].ToString();
            hdnRef_Type.Value = dtVoucherEdit.Rows[0]["Ref_Type"].ToString();
            hdnInvoiceNumber.Value = dtVoucherEdit.Rows[0]["Inv_Number"].ToString();
            hdnInvoiceDate.Value = dtVoucherEdit.Rows[0]["Inv_Date"].ToString();

            string strCurrencyId = dtVoucherEdit.Rows[0]["Currency_Id"].ToString();
            //for Reconciel Cheque
            if (strCashCheque == "Cheque")
            {
                //For Bank Account
                string strAccountId = objAccParameterLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), strToLocationId);


                DataTable dtReconcile = objVoucherDetail.GetVoucherDetailByVoucherNo(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
                if (dtReconcile.Rows.Count > 0)
                {
                    for (int i = 0; i < dtReconcile.Rows.Count; i++)
                    {
                        if (strAccountId.Split(',').Contains(dtReconcile.Rows[i]["Account_No"].ToString()))
                        {
                            string strStatus = dtReconcile.Rows[i]["Field2"].ToString();
                            if (strStatus == "False")
                            {
                                chkReconcile.Checked = false;
                                chkReconcile.Visible = true;
                            }
                            else if (strStatus == "True")
                            {
                                chkReconcile.Checked = true;
                                chkReconcile.Visible = true;
                            }
                            else
                            {
                                chkReconcile.Checked = false;
                                chkReconcile.Visible = true;
                            }
                        }
                        else
                        {
                            chkReconcile.Checked = false;
                            chkReconcile.Visible = true;
                        }
                    }
                }
                else
                {
                    chkReconcile.Checked = false;
                    chkReconcile.Visible = true;
                }
            }

            string strCurrency = Session["LocCurrencyId"].ToString();
            //Add Child Concept            
            if (dtDetail.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GvDetail, dtDetail, "", "");

                //For Total
                double sumDebit = 0;
                double sumCredit = 0;
                foreach (GridViewRow gvr in GvDetail.Rows)
                {
                    Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
                    Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");
                    Label lblgvFrgnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
                    Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");

                    if (((Label)gvr.FindControl("lblgvDebitAmount")).Text == "")
                    {
                        ((Label)gvr.FindControl("lblgvDebitAmount")).Text = "0";
                    }
                    sumDebit += Convert.ToDouble(((Label)gvr.FindControl("lblgvDebitAmount")).Text);

                    if (((Label)gvr.FindControl("lblgvCreditAmount")).Text == "")
                    {
                        ((Label)gvr.FindControl("lblgvCreditAmount")).Text = "0";
                    }
                    sumCredit += Convert.ToDouble(((Label)gvr.FindControl("lblgvCreditAmount")).Text);

                    lblgvDebitAmt.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblgvDebitAmt.Text);
                    lblgvCreditAmt.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblgvCreditAmt.Text);
                    lblgvFrgnAmt.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblgvFrgnAmt.Text);
                    lblgvExchangeRate.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblgvExchangeRate.Text);
                }

                Label lblDebitTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvDebitTotal"));
                Label lblCreditTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvCreditTotal"));

                lblDebitTotal.Text = sumDebit.ToString();
                lblCreditTotal.Text = sumCredit.ToString();
                lblDebitTotal.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblDebitTotal.Text);
                lblCreditTotal.Text = ObjSysParam.GetCurencyConversionForInv(strVoucherLocCurrency, lblCreditTotal.Text);
            }
        }
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherNo);
    }
    protected void rbCashPayment_CheckedChanged(object sender, EventArgs e)
    {
        if (rbCashPayment.Checked == true)
        {
            trCheque1.Visible = false;
            trCheque2.Visible = false;
            txtChequeIssueDate.Text = "";
            txtChequeClearDate.Text = "";
            txtChequeNo.Text = "";
        }
        else if (rbChequePayment.Checked == true)
        {
            trCheque1.Visible = true;
            trCheque2.Visible = true;
        }
    }


    protected void ImgButtonDelete_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ImageButton imgeditbutton = new ImageButton();
        imgeditbutton.ID = "imgBtnApprove";

        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    IbtnDelete_Command(imgeditbutton, new CommandEventArgs("commandName", lblSelectedRecord.Text.Split(',')[j].ToString()));
                }
            }

            DisplayMessage("Record deleted successfully");
            FillGridBin(); //Update grid view in bin tab
            FillGrid();
            VoucherReset();

        }
        else
        {
            DisplayMessage("Please select record");
            return;
        }
    }


    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        //create this validation by jitendra upadhyay on 04-02-2014 
        // here we set validation that the after post the record user can't delete the record
        strLocationId = ddlLocationList.SelectedValue;
        DataTable dtVoucherEdit = objVoucherHeader.GetVoucherHeaderByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());

        if (dtVoucherEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtVoucherEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Voucher is posted, So it cannot be Deleted !");
                return;
            }
        }

        DataTable Dt_Sal_Loan = objVoucherHeader.Get_Relationship_Voucher_Header(e.CommandArgument.ToString(), "1");
        if (Dt_Sal_Loan != null && Dt_Sal_Loan.Rows.Count > 0)
        {
            if (Dt_Sal_Loan.Rows.Count > 1)
            {
                foreach (DataRow Dr_Loan in Dt_Sal_Loan.Rows)
                {
                    if (Convert.ToBoolean(Dr_Loan["ReconciledFromFinance"].ToString()) == true)
                    {
                        if (Dr_Loan["Field5"].ToString() == "")
                        {
                            DisplayMessage("Salary Voucher is Posted, So it cannot be Deleted !");
                            return;
                        }
                        else
                        {
                            DisplayMessage("Loan Voucher is Posted, So it cannot be Deleted !");
                            return;
                        }
                    }
                }
            }
        }

        DataTable Dt_Emp_Payment_Loan = ObjLoan.Get_Employee_Loan("0", "0", e.CommandArgument.ToString(), "True", "True", "1");
        if (Dt_Emp_Payment_Loan != null && Dt_Emp_Payment_Loan.Rows.Count > 0)
        {
            DisplayMessage("Loan Payment is Approved So It cannot be Deleted !");
            return;
        }


        int b = 0;
        hdnVoucherId.Value = e.CommandArgument.ToString();
        b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DataTable dtAgeDetail = objAgeingDetail.GetAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value);
            if (dtAgeDetail.Rows.Count > 0)
            {
                objAgeingDetail.DeleteAgeingIsActive(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, "false", StrUserId, DateTime.Now.ToString());
            }
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }


    }
    protected void btnVoucherCancel_Click(object sender, EventArgs e)
    {
        VoucherReset();
        btnList_Click(null, null);
        FillGridBin();
        FillGrid();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void VoucherReset()
    {
        FillGrid();
        txtVoucherDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        ddlVoucherType.Enabled = true;
        ddlVoucherType.SelectedValue = "--Select--";
        txtReference.Text = "0";
        PnlNewContant.Enabled = true;
        btnVoucherSave.Visible = true;
        //FillCurrency();
        //txtExchangeRate.Text = "";
        txtChequeIssueDate.Text = "";
        txtChequeClearDate.Text = "";
        txtChequeNo.Text = "";

        GvDetail.DataSource = null;
        GvDetail.DataBind();
        hdnVoucherId.Value = "0";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "184", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        ViewState["DocNo"] = txtVoucherNo.Text;
        string strCurrencyId = string.Empty;

        try
        {
            strCurrencyId = Session["LocCurrencyId"].ToString();
        }
        catch
        {

        }

        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            //ddlCurrency.SelectedValue = strCurrencyId;
            ddlDCurrency.SelectedValue = strCurrencyId;
        }
        //txtExchangeRate.Text = "1";
        txtDExchangeRate.Text = "1";
        txtNarration.Text = "";

        rbCashPayment.Checked = true;
        rbChequePayment.Checked = false;
        rbCashPayment_CheckedChanged(null, null);
        hdnRef_Id.Value = "0";
        hdnRef_Type.Value = "0";
        hdnInvoiceNumber.Value = "0";
        hdnInvoiceDate.Value = "0";

        btnList_Click(null, null);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        VoucherReset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToLocation);
    }
    private string GetLocationId()
    {
        string retval = string.Empty;
        if (txtToLocation.Text != "")
        {
            DataTable dtLocation = ObjLocation.GetLocationMasterByLocationName(StrCompId.ToString(), txtToLocation.Text.Trim().Split('/')[0].ToString());
            if (dtLocation.Rows.Count > 0)
            {
                retval = (txtToLocation.Text.Split('/'))[txtToLocation.Text.Split('/').Length - 1];
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    private string GetDepartmentId()
    {
        string retval = string.Empty;
        if (txtDepartment.Text != "")
        {
            DataTable dtDepartment = objDepartment.GetDepartmentMasterByDepName(txtDepartment.Text.Trim().Split('/')[0].ToString());
            if (dtDepartment.Rows.Count > 0)
            {
                retval = (txtDepartment.Text.Split('/'))[txtToLocation.Text.Split('/').Length - 1];
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = Session["LocCurrencyId"].ToString();
        //try
        //{
        //updated on 30-11-2015 for currency conversion by jitendra upadhyay
        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());

        try
        {
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    protected void btnVoucherSave_Click(object sender, EventArgs e)
    {
        if (txtToLocation.Text == "")
        {
            DisplayMessage("Enter To Location");
            txtToLocation.Focus();
            return;
        }

        if (txtDepartment.Text == "")
        {
            DisplayMessage("Enter Department");
            txtDepartment.Focus();
            return;
        }
        string strVoucherLocCurrency = string.Empty;
        try
        {
            strLocationId = txtToLocation.Text.Split('/')[1].ToString();
            strVoucherLocCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), strLocationId).Rows[0]["Currency_Id"].ToString();
        }
        catch
        {

        }
        if (string.IsNullOrEmpty(strVoucherLocCurrency))
        {
            strVoucherLocCurrency = Session["CurrencyId"].ToString();
        }

        if (hdnVoucherId.Value == "0" || hdnVoucherId.Value == "")
        {
            DisplayMessage("You Have No Edited Record. You Cant Update it.");
            return;
        }

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtVoucherDate.Text), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        if (GvDetail.Rows.Count == 0)
        {
            DisplayMessage("Enter Detail");
            return;
        }

        string strAccountNos = string.Empty;
        foreach (GridViewRow gvr in GvDetail.Rows)
        {
            HiddenField hdngvAccountNo = (HiddenField)gvr.FindControl("hdngvAccountNo");
            strAccountNos = strAccountNos + "," + hdngvAccountNo.Value;
        }
        strAccountNos = strAccountNos.Substring(1, strAccountNos.Length - 1);
        if (!objAccParameterLocation.ValidateVoucherForCashFlow(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strAccountNos, txtVoucherDate.Text))
        {
            DisplayMessage("Your Cashflow has been posted for the date :- " + txtVoucherDate.Text + "");
            return;
        }


        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strFinanceCode = string.Empty;
        string strChequeIssueDate = string.Empty;
        string strChequeClearDate = string.Empty;
        string strCashCheque = string.Empty;

        string strCustomerId = string.Empty;
        string strVoucherType = string.Empty;

        if (txtFinanceCode.Text != "")
        {
            strFinanceCode = txtFinanceCode.Text.Trim().Split('/')[1].ToString();
        }
        else
        {
            strFinanceCode = "0";
        }



        if (txtVoucherNo.Text == "")
        {
            DisplayMessage("Enter Voucher No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherNo);
            return;
        }


        if (txtChequeIssueDate.Text == "")
        {
            strChequeIssueDate = "1/1/1800";
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtChequeIssueDate.Text);
                strChequeIssueDate = txtChequeIssueDate.Text;
            }
            catch
            {
                DisplayMessage("Enter Cheque Issue Date in format " + Session["DateFormat"].ToString() + "");
                txtChequeIssueDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtChequeIssueDate);
                return;
            }
        }

        if (txtChequeClearDate.Text == "")
        {
            strChequeClearDate = "1/1/1800";
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtChequeClearDate.Text);
                strChequeClearDate = txtChequeClearDate.Text;
            }
            catch
            {
                DisplayMessage("Enter Cheque Clear Date in format " + Session["DateFormat"].ToString() + "");
                txtChequeClearDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtChequeClearDate);
                return;
            }
        }

        if (txtVoucherDate.Text == "")
        {
            DisplayMessage("Enter Voucher Date.");
            txtVoucherDate.Focus();
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtVoucherDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Voucher Date in format " + Session["DateFormat"].ToString() + "");
                txtVoucherDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherDate);
                return;
            }
        }

        if (ddlVoucherType.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Voucher Type");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlVoucherType);
            return;
        }
        else if (ddlVoucherType.SelectedValue != "--Select--")
        {
            strVoucherType = ddlVoucherType.SelectedValue;
        }

        if (rbCashPayment.Checked == true)
        {
            strCashCheque = "Cash";
        }
        else if (rbChequePayment.Checked == true)
        {
            strCashCheque = "Cheque";
        }
        else
        {
            DisplayMessage("Choose Any Payment Type");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(rbCashPayment);
            return;
        }

        //For Bank Account
        string strAccountId = string.Empty;
        DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "BankAccount");
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

        string strReconcile = string.Empty;
        if (chkReconcile.Visible == true)
        {
            if (chkReconcile.Checked == true)
            {
                strReconcile = "True";
            }
            else if (chkReconcile.Checked == false)
            {
                strReconcile = "False";
            }
            else
            {
                strReconcile = "False";
            }
        }




        //For Total
        Label lblDebitTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvDebitTotal"));
        Label lblCreditTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvCreditTotal"));

        if (float.Parse(lblDebitTotal.Text) == float.Parse(lblCreditTotal.Text))
        {

        }
        else
        {
            DisplayMessage("Your Debit Amount & Credit Amount is Not Equal");
            return;
        }


        strPost = "False";
        strCancel = "False";

        //for Customer & Supplier Account
        string strReceiveVoucherAcc = string.Empty;
        DataTable dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
        if (dtParam.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strReceiveVoucherAcc = "0";
        }

        string strPaymentVoucherAcc = string.Empty;
        DataTable dtPaymentVoucher = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPaymentVoucherAcc = "0";
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            if (hdnVoucherId.Value != "0" && hdnVoucherId.Value != "")
            {
                b = objVoucherHeader.UpdateVoucherHeader(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, strFinanceCode, GetLocationId(), GetDepartmentId(), hdnRef_Id.Value, hdnRef_Type.Value, hdnInvoiceNumber.Value, hdnInvoiceDate.Value, txtVoucherNo.Text, ObjSysParam.getDateForInput(txtVoucherDate.Text).ToString(), strVoucherType, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, txtReference.Text, strVoucherLocCurrency, "1", txtReference.Text, strPost, strCancel, false.ToString(), "AV", strCashCheque, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //Add Detail Section.
                objVoucherDetail.DeleteVoucherDetail(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);
                //objAgeingDetail.DeleteAgeingDetailByVoucherId(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, ref trns);

                foreach (GridViewRow gvr in GvDetail.Rows)
                {
                    HiddenField hdngvAccountNo = (HiddenField)gvr.FindControl("hdngvAccountNo");
                    Label lblgvSerialNo = (Label)gvr.FindControl("lblSNo");
                    Label lblgvOtherAccountNo = (Label)gvr.FindControl("lblgvOtherAccountNo");
                    HiddenField hdnOtherAccountNo = (HiddenField)gvr.FindControl("hdnOtherAccountNo");
                    Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                    Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                    Label lblgvNarration = (Label)gvr.FindControl("lblgvNarration");
                    Label lblgvCostCenter = (Label)gvr.FindControl("lblgvCostCenter");
                    HiddenField hdngvEmployeeId = (HiddenField)gvr.FindControl("hdngvEmployeeId");
                    HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdngvCurrencyId");
                    Label lblgvForeignAmount = (Label)gvr.FindControl("lblgvForeignAmount");
                    Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");

                    //HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");
                    //HiddenField hdnRefType = (HiddenField)gvr.FindControl("hdnRefType");

                    //For Debit Entry
                    string strCompanyCrrValueDr = GetCurrency(strVoucherLocCurrency, lblgvDebitAmount.Text);
                    string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                    //For Credit Entry 
                    string strCompanyCrrValueCr = GetCurrency(strVoucherLocCurrency, lblgvCreditAmount.Text);
                    string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();

                    if (strCashCheque == "Cheque")
                    {
                        if (strAccountId.Split(',').Contains(hdngvAccountNo.Value))
                        {
                            objVoucherDetail.InsertVoucherDetail(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, lblgvSerialNo.Text, hdngvAccountNo.Value, hdnOtherAccountNo.Value, hdnRef_Id.Value, hdnRef_Type.Value, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, lblgvDebitAmount.Text, lblgvCreditAmount.Text, lblgvNarration.Text, lblgvCostCenter.Text, Session["EmpId"].ToString(), hdngvCurrencyId.Value, lblgvExchangeRate.Text, lblgvForeignAmount.Text, CompanyCurrDebit, CompanyCurrCredit, "", strReconcile, "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, lblgvSerialNo.Text, hdngvAccountNo.Value, hdnOtherAccountNo.Value, hdnRef_Id.Value, hdnRef_Type.Value, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, lblgvDebitAmount.Text, lblgvCreditAmount.Text, lblgvNarration.Text, lblgvCostCenter.Text, Session["EmpId"].ToString(), hdngvCurrencyId.Value, lblgvExchangeRate.Text, lblgvForeignAmount.Text, CompanyCurrDebit, CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    else
                    {
                        objVoucherDetail.InsertVoucherDetail(StrCompId, StrBrandId, strLocationId, hdnVoucherId.Value, lblgvSerialNo.Text, hdngvAccountNo.Value, hdnOtherAccountNo.Value, hdnRef_Id.Value, hdnRef_Type.Value, strChequeIssueDate, strChequeClearDate, txtChequeNo.Text, lblgvDebitAmount.Text, lblgvCreditAmount.Text, lblgvNarration.Text, lblgvCostCenter.Text, Session["EmpId"].ToString(), hdngvCurrencyId.Value, lblgvExchangeRate.Text, lblgvForeignAmount.Text, CompanyCurrDebit, CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                }
                //End  

                if (b != 0)
                {
                    if (strPost != "True")
                    {
                        DisplayMessage("Record Updated Successfully", "green");
                        Lbl_Tab_New.Text = Resources.Attendance.New;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    }
                    else
                    {
                        DisplayMessage("Voucher has posted");
                    }

                    btnList_Click(null, null);
                }
                else
                {
                    DisplayMessage("Record  Not Updated");
                }
            }
            else
            {

            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            trns.Dispose();
            con.Dispose();
            FillGrid();
            VoucherReset();
            btnList_Click(null, null);
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString().Replace("'", " ") + " Line Number : " + Common.GetLineNumber(ex));
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
    protected void txtToLocation_TextChanged(object sender, EventArgs e)
    {
        string strLocationId = string.Empty;
        if (txtToLocation.Text != "")
        {
            strLocationId = GetLocationId();
            if (strLocationId != "" && strLocationId != "0")
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDepartment);
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtToLocation.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToLocation);
            }
        }
    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtSupplierName.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtSupplierName.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        Session["SupplierAccountId"] = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                        txtCreditAmount.Focus();
                        return;
                    }
                }
            }
        }
        catch { }
        DisplayMessage("Supplier is not valid");
        txtSupplierName.Text = "";
        txtSupplierName.Focus();
    }
    protected void txtEmployee_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtEmployee.Text != "")
        {
            strEmployeeId = GetEmployeeId(txtEmployee.Text);
            if (strEmployeeId != "" && strEmployeeId != "0")
            {
                hdnEmployeeId.Value = strEmployeeId;
            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtEmployee.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEmployee);
            }
        }
    }
    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(StrCompId, strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(StrCompId, retval);
                if (dtEmp.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    protected void txtDepartment_TextChanged(object sender, EventArgs e)
    {
        string strDepartmentId = string.Empty;
        if (txtDepartment.Text != "")
        {
            strDepartmentId = GetDepartmentId();
            if (strDepartmentId != "" && strDepartmentId != "0")
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlVoucherType);
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtDepartment.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDepartment);
            }
        }
    }
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtCustomerName.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtCustomerName.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        Session["CustomerAccountId"] = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                        txtDebitAmount.Focus();
                        return;
                    }

                }
            }
        }
        catch
        { }
        DisplayMessage("Customer is not valid");
        txtCustomerName.Text = "";
        txtCustomerName.Focus();
    }
    protected void txtAccountNo_TextChanged(object sender, EventArgs e)
    {
        if (txtAccountNo.Text != "")
        {
            DataTable dtAccount = objCOA.GetCOAAll(StrCompId);
            string retval = string.Empty;
            if (txtAccountNo.Text != "")
            {
                string strAccountName = txtAccountNo.Text.Trim().Split('/')[0].ToString();
                dtAccount = new DataView(dtAccount, "AccountName='" + strAccountName + "' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccount.Rows.Count > 0)
                {
                    retval = (txtAccountNo.Text.Split('/'))[txtAccountNo.Text.Split('/').Length - 1];
                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }

            if (retval != "0" && retval != "")
            {
                if (dtAccount != null && dtAccount.Rows.Count > 0)
                {
                    //txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Account_No");
                    for (int i = 0; i < GvDetail.Rows.Count; i++)
                    {
                        dt.Rows.Add(i);
                        HiddenField hdngvAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdngvAccountNo");
                        dt.Rows[i]["Account_No"] = hdngvAccountNo.Value;
                    }

                    //for Customer & Supplier Account
                    string strReceiveVoucherAcc = string.Empty;
                    DataTable dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
                    if (dtParam.Rows.Count > 0)
                    {
                        strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
                    }
                    else
                    {
                        strReceiveVoucherAcc = "0";
                    }

                    string strPaymentVoucherAcc = string.Empty;
                    DataTable dtPaymentVoucher = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
                    if (dtPaymentVoucher.Rows.Count > 0)
                    {
                        strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
                    }
                    else
                    {
                        strPaymentVoucherAcc = "0";
                    }

                    if (txtAccountNo.Text.Split('/')[1].ToString() == strReceiveVoucherAcc)
                    {
                        trSupplier.Visible = false;
                        trCustomer.Visible = true;
                        txtSupplierName.Text = "";
                        txtCustomerName.Text = "";
                        txtCustomerName.Focus();
                    }
                    else if (txtAccountNo.Text.Split('/')[1].ToString() == strPaymentVoucherAcc)
                    {
                        trSupplier.Visible = true;
                        trCustomer.Visible = false;
                        txtSupplierName.Text = "";
                        txtCustomerName.Text = "";
                        txtSupplierName.Focus();
                    }
                    else
                    {
                        trSupplier.Visible = false;
                        trCustomer.Visible = false;
                        txtSupplierName.Text = "";
                        txtCustomerName.Text = "";
                        txtDebitAmount.Focus();
                    }
                }
                else
                {
                    txtAccountNo.Text = "";
                    DisplayMessage("No Account Found");
                    txtAccountNo.Focus();
                    return;
                }
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAccountNo.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
            }
        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
        }

    }
    protected void txtDExchangeRate_OnTextChanged(object sender, EventArgs e)
    {
        string strForienAmount = string.Empty;
        if (txtDExchangeRate.Text != "")
        {
            if (txtDebitAmount.Text != "0" && txtDebitAmount.Text != "")
            {
                strForienAmount = ObjSysParam.GetCurencyConversionForInv(ddlDCurrency.SelectedValue, (float.Parse(txtDExchangeRate.Text) * float.Parse(txtDebitAmount.Text)).ToString());
                strForienAmount = strForienAmount + "/" + txtDExchangeRate.Text;
                txtForeignAmount.Text = strForienAmount.Trim().Split('/')[0].ToString();
                txtDExchangeRate.Text = strForienAmount.Trim().Split('/')[1].ToString();
            }
            else if (txtCreditAmount.Text != "0" && txtCreditAmount.Text != "")
            {
                strForienAmount = ObjSysParam.GetCurencyConversionForInv(ddlDCurrency.SelectedValue, (float.Parse(txtDExchangeRate.Text) * float.Parse(txtCreditAmount.Text)).ToString());
                strForienAmount = strForienAmount + "/" + txtDExchangeRate.Text;
                txtForeignAmount.Text = strForienAmount.Trim().Split('/')[0].ToString();
                txtDExchangeRate.Text = strForienAmount.Trim().Split('/')[1].ToString();
            }

            //string strFireignExchange = GetCurrency(ddlDCurrency.SelectedValue, txtDebitAmount.Text);
            //txtForeignAmount.Text = strFireignExchange.Trim().Split('/')[0].ToString();
            //txtDExchangeRate.Text = strFireignExchange.Trim().Split('/')[1].ToString();
        }
    }
    protected void txtDebitAmount_OnTextChanged(object sender, EventArgs e)
    {
        double debitamount = Convert.ToDouble(txtDebitAmount.Text);
        if (debitamount != 0)
        {
            txtCreditAmount.Text = "0";
            txtCreditAmount.ReadOnly = true;
            txtDebitAmount.ReadOnly = false;

            string strFireignExchange = GetCurrency(ddlDCurrency.SelectedValue, txtDebitAmount.Text);
            txtForeignAmount.Text = strFireignExchange.Trim().Split('/')[0].ToString();
            txtDExchangeRate.Text = strFireignExchange.Trim().Split('/')[1].ToString();
        }
        else
        {
            txtCreditAmount.Text = "0";
            txtCreditAmount.ReadOnly = false;
            txtForeignAmount.Text = "0";
            txtDExchangeRate.Text = "0";
        }
    }
    protected void txtCreditAmount_OnTextChanged(object sender, EventArgs e)
    {
        double creditamount = Convert.ToDouble(txtCreditAmount.Text);
        if (creditamount != 0)
        {
            txtDebitAmount.Text = "0";
            txtDebitAmount.ReadOnly = true;
            txtCreditAmount.ReadOnly = false;

            string strFireignExchange = GetCurrency(ddlDCurrency.SelectedValue, txtCreditAmount.Text);
            txtForeignAmount.Text = strFireignExchange.Trim().Split('/')[0].ToString();
            txtDExchangeRate.Text = strFireignExchange.Trim().Split('/')[1].ToString();
        }
        else
        {
            txtDebitAmount.Text = "0";
            txtDebitAmount.ReadOnly = false;
            txtForeignAmount.Text = "0";
            txtDExchangeRate.Text = "0";
        }
    }
    protected void ddlVoucherType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVoucherType.SelectedValue == "JV")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "PV")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "RV")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "303", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "PI")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "308", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "PR")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "309", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "SI")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "310", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "SR")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "311", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "SPV")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "312", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "CRV")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "313", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "CDN")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "314", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "SDN")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "315", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "CCN")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "316", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "SCN")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "317", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "PDC")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "318", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else if (ddlVoucherType.SelectedValue == "PDS")
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "319", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
        else
        {
            txtVoucherNo.Text = objDocNo.GetDocumentNo(true, "0", false, "160", "184", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtVoucherNo.Text;
        }
    }
    #endregion

    #region Auto Complete
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "Emp_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //txt[i] = dt.Rows[i]["Emp_Name"].ToString();
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }
        else
        {
            if (dt1.Rows.Count > 0)
            {
                txt = new string[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    //txt[i] = dt1.Rows[i]["Emp_Name"].ToString();
                    txt[i] = dt1.Rows[i]["Emp_Name"].ToString() + "/" + dt1.Rows[i]["Emp_Id"].ToString() + "";
                }
            }
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objAcParamMaster.GetCustomerAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ac_ParameterMaster objAcParamMaster = new Ac_ParameterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = objAcParamMaster.GetSupplierAsPerSearchText(HttpContext.Current.Session["CompId"].ToString(), prefixText);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListLocation(string prefixText, int count, string contextKey)
    {
        LocationMaster objLoc = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objLoc.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Location_Name Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Location_Name"].ToString() + "/" + dt.Rows[i]["Location_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = objLoc.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Location_Name"].ToString() + "/" + dt.Rows[i]["Location_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartment(string prefixText, int count, string contextKey)
    {
        DepartmentMaster objDepartment = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objDepartment.GetDepartmentMaster();

        dt = new DataView(dt, "Dep_Name Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = objDepartment.GetDepartmentMaster();
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountNo(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount COA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "AccountName Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployee(string prefixText, int count, string contextKey)
    {
        EmployeeMaster objEmployee = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objEmployee.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Emp_Name Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = objEmployee.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    #endregion

    #region VoucherNumberCommon
    protected void lblgvVoucherNoCommon_Click(object sender, EventArgs e)
    {
        string strVoucherType = string.Empty;
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string RefId = arguments[0];
        string RefType = arguments[1];
        string Trans_Id = arguments[2].Trim();
        string LocationId = arguments[3].Trim();

        DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeader();
        if (dtVoucherHeader.Rows.Count > 0)
        {
            dtVoucherHeader = new DataView(dtVoucherHeader, "Trans_Id='" + Trans_Id + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtVoucherHeader.Rows.Count > 0)
            {
                strVoucherType = dtVoucherHeader.Rows[0]["Voucher_Type"].ToString();
            }
        }

        if (RefId == "0" && RefId != "")
        {
            if (IsObjectPermission("160", "184"))
            {
                string strCmd = string.Format("window.open('../VoucherEntries/VoucherDetail.aspx?Id=" + Trans_Id + "&LocId=" + LocationId + "','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
            else
            {
                DisplayMessage("You have no permission for view detail");
                return;
            }
        }
        else if (RefId != "0" && RefId != "")
        {
            if (RefType == "PINV" && strVoucherType == "PI")
            {
                if (IsObjectPermission("143", "48"))
                {
                    string strCmd = string.Format("window.open('../Purchase/PurchaseInvoice.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "SINV" && strVoucherType == "SI")
            {
                if (IsObjectPermission("144", "92"))
                {
                    string strCmd = string.Format("window.open('../Sales/SalesInvoice.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "PO" && strVoucherType == "PV")
            {
                if (IsObjectPermission("143", "45"))
                {
                    string strCmd = string.Format("window.open('../Purchase/PurchaseOrder.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "SO" && strVoucherType == "RV")
            {
                if (IsObjectPermission("144", "67"))
                {
                    string strCmd = string.Format("window.open('../Sales/SalesOrder.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "SR" && strVoucherType == "SR")
            {
                if (IsObjectPermission("144", "120"))
                {
                    string strCmd = string.Format("window.open('../Sales/SalesReturn.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
        }
        else
        {
            //myButton.Attributes.Add("
            DisplayMessage("No Data");
            return;
        }
        //AllPageCode();
    }
    public bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        bool Result = false;
        if (Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId, Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }
    #endregion

    #region Bin Section
    protected void GvVoucherBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvVoucherBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");

        string temp = string.Empty;

        for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvVoucherBin.Rows[i].FindControl("lblgvTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        string strCurrency = Session["LocCurrencyId"].ToString();
        foreach (GridViewRow gv in GvVoucherBin.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvExchangerate");

            lblgvVoucherAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
    }
    protected void GvVoucherBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        strLocationId = ddlLocationList.SelectedValue;
        dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");
        lblSelectedRecord.Text = "";

        string strCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), strLocationId).Rows[0]["Currency_Id"].ToString();
        foreach (GridViewRow gv in GvVoucherBin.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvExchangerate");

            lblgvVoucherAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
        //AllPageCode();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        strLocationId = ddlLocationList.SelectedValue;
        dt = objVoucherHeader.GetRecordforReconcileFinanceandFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");
        Session["dtVoucherBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";

        if (dt.Rows.Count == 0)
        {
            ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = false;
        }
        else
        {
            ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = true;
        }

        string strCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), strLocationId).Rows[0]["Currency_id"].ToString();
        foreach (GridViewRow gv in GvVoucherBin.Rows)
        {
            Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvExchangerate");

            lblgvVoucherAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
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


            DataTable dtCust = (DataTable)Session["dtVoucherBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucherBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);

            string strCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), ddlLocationList.SelectedValue).Rows[0]["Currency_Id"].ToString();
            foreach (GridViewRow gv in GvVoucherBin.Rows)
            {
                Label lblgvVoucherAmt = (Label)gv.FindControl("lblgvExchangerate");

                lblgvVoucherAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvVoucherAmt.Text);
            }
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        strLocationId = ddlLocationList.SelectedValue;
        DataTable dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());

        if (GvVoucherBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        //Msg = objTax.DeleteTaxMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                FillGrid();
                FillGridBin();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activate");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvVoucherBin.Rows)
                {
                    CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void chkBCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvVoucherBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
        {
            ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
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
    protected void chkBSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvVoucherBin.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvVoucherBin.Rows[index].FindControl("chkSelect")).Checked)
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
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgBbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtVoucher = (DataTable)Session["dtInactive"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtVoucher.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvVoucherBin.Rows[i].FindControl("lblgvTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtVocher = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucherBin, dtVocher, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        strLocationId = ddlLocationList.SelectedValue;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblSelectedRecord.Text.Split(',')[j].Trim() != "" && lblSelectedRecord.Text.Split(',')[j].Trim() != "0")
                    {
                        DataTable dtVoucherHeader = objVoucherHeader.GetRecordforReconcileFinanceandFalse(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, Session["FinanceYearId"].ToString());
                        if (dtVoucherHeader.Rows.Count > 0)
                        {
                            dtVoucherHeader = new DataView(dtVoucherHeader, "Trans_Id='" + lblSelectedRecord.Text.Split(',')[j].Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        if (dtVoucherHeader.Rows.Count > 0)
                        {
                            string strVoucherDate = dtVoucherHeader.Rows[0]["Voucher_Date"].ToString();
                            if (strVoucherDate != "")
                            {
                                if (!Common.IsFinancialyearAllow(Convert.ToDateTime(strVoucherDate), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                                {
                                    DisplayMessage("Log In Financial year not allowing to perform this action");
                                    return;
                                }
                                else
                                {
                                    b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    DataTable dtAgeDetail = objAgeingDetail.GetAgeingDetailAllTrueFalse(StrCompId, StrBrandId, strLocationId);
                                    dtAgeDetail = new DataView(dtAgeDetail, "Field3='" + hdnVoucherId.Value + "' and IsActive='False'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtAgeDetail.Rows.Count > 0)
                                    {
                                        objAgeingDetail.DeleteAgeingIsActive(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (b != 0)
        {
            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
        }
        else
        {
            int flag = 0;
            foreach (GridViewRow Gvr in GvVoucherBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                if (chk.Checked)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
            }
            if (flag == 0)
            {
                DisplayMessage("Please Select Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }
    }
    #endregion

    #region Detail Section
    protected void btnDetailSave_Click(object sender, EventArgs e)
    {
        if (ddlVoucherType.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Voucher Type");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlVoucherType);
            return;
        }
        else if (ddlVoucherType.SelectedValue != "--Select--")
        {
            ddlVoucherType.Enabled = false;
        }

        string Description = string.Empty;
        if (txtAccountNo.Text != "")
        {
            string strAccountName = txtAccountNo.Text.Trim().Split('/')[0].ToString();

            string strA = "0";
            foreach (GridViewRow gve in GvDetail.Rows)
            {
                Label lblgvAccountName = (Label)gve.FindControl("lblgvAccountName");
                if (strAccountName == lblgvAccountName.Text)
                {
                    strA = "1";
                }
            }

            if (hdnNewAccountNo.Value == "0")
            {
                if (txtAccountNo.Text != "")
                {
                    DataTable dt = objCOA.GetCOAAllTrue(StrCompId);
                    dt = new DataView(dt, "AccountName ='" + strAccountName + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        hdnNewAccountNo.Value = dt.Rows[0]["Trans_Id"].ToString();
                    }
                    else
                    {
                        hdnNewAccountNo.Value = "0";
                    }
                }
            }

            //here we are checking currency constrain for customer and supplier
            //--------start neelkanth purohit 23-08-2018
            int NewOtherAccount = 0;
            if (trCustomer.Visible == true)
            {
                Int32.TryParse(txtCustomerName.Text.Split('/')[1].ToString(), out NewOtherAccount);
            }
            if (trSupplier.Visible == true)
            {
                Int32.TryParse(txtSupplierName.Text.Split('/')[1].ToString(), out NewOtherAccount);
            }
            if (NewOtherAccount > 0)
            {
                if (ddlDCurrency.SelectedValue.ToString() != objAcAccountMaster.GetAc_AccountMasterByTransId(NewOtherAccount.ToString()).Rows[0]["Currency_id"].ToString())
                {
                    DisplayMessage("Account No and Currency are different, please select currency as per Account");
                    return;
                }
            }
            //------end----------------------

            if (txtDebitAmount.Text == "")
            {
                txtDebitAmount.Text = "0";
            }
            if (txtCreditAmount.Text != "")
            {

            }
            else
            {
                txtCreditAmount.Text = "0";
            }



            if (ddlDCurrency.SelectedValue != "--Select--")
            {
                hdnCurrencyId.Value = ddlDCurrency.SelectedValue;
            }
            else
            {
                DisplayMessage("Select Currency Name");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlDCurrency);
                return;
            }

            if (txtForeignAmount.Text != "")
            {

            }
            else
            {
                txtForeignAmount.Text = "0";
            }

            if (txtDExchangeRate.Text != "")
            {

            }
            else
            {
                txtDExchangeRate.Text = "0";
            }

            if (hdnAccountNo.Value == "")
            {
                FillProductChidGird("Save");
                //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddDetail);
            }
            else
            {
                if (txtAccountNo.Text == hdnAccountName.Value)
                {
                    FillProductChidGird("Edit");
                }
                else
                {
                    FillProductChidGird("Edit");
                }
            }

            //For Total
            string strCurrency = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), ddlLocationList.SelectedValue).Rows[0]["Currency_id"].ToString();
            double sumDebit = 0;
            double sumCredit = 0;
            foreach (GridViewRow gvr in GvDetail.Rows)
            {
                Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
                Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");
                Label lblgvFrgnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
                Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");

                if (((Label)gvr.FindControl("lblgvDebitAmount")).Text == "")
                {
                    ((Label)gvr.FindControl("lblgvDebitAmount")).Text = "0";
                }
                sumDebit += Convert.ToDouble(((Label)gvr.FindControl("lblgvDebitAmount")).Text);

                if (((Label)gvr.FindControl("lblgvCreditAmount")).Text == "")
                {
                    ((Label)gvr.FindControl("lblgvCreditAmount")).Text = "0";
                }
                sumCredit += Convert.ToDouble(((Label)gvr.FindControl("lblgvCreditAmount")).Text);

                lblgvDebitAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
                lblgvCreditAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditAmt.Text);
                lblgvFrgnAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvFrgnAmt.Text);
                lblgvExchangeRate.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvExchangeRate.Text);
            }

            Label lblDebitTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvDebitTotal"));
            Label lblCreditTotal = (Label)(GvDetail.FooterRow.FindControl("lblgvCreditTotal"));

            lblDebitTotal.Text = sumDebit.ToString();
            lblCreditTotal.Text = sumCredit.ToString();

            lblDebitTotal.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblDebitTotal.Text);
            lblCreditTotal.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblCreditTotal.Text);

        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
        }
        txtAccountNo.Focus();
    }
    protected void btnDetailCancel_Click(object sender, EventArgs e)
    {
        ResetDetail();
    }
    public void ResetDetail()
    {
        txtAccountNo.Text = "";
        txtCustomerName.Text = "";
        txtSupplierName.Text = "";
        trCustomer.Visible = false;
        trSupplier.Visible = false;
        txtDebitAmount.Text = "";
        txtCreditAmount.Text = "";
        txtCreditAmount.ReadOnly = false;
        txtDebitAmount.ReadOnly = false;
        txtCostCenter.Text = "";
        txtEmployee.Text = "";
        FillDetailCurrency();
        txtForeignAmount.Text = "";
        txtDExchangeRate.Text = "";
        hdnAccountNo.Value = "";
        hdnAccountName.Value = "";
        hdnEmployeeId.Value = "";
        hdnNewAccountNo.Value = "0";
        string strCurrencyId = string.Empty;
        try
        {
            strCurrencyId = ObjLocation.Get_Currency_By_Location_ID(Session["CompId"].ToString(), ddlLocationList.SelectedValue).Rows[0]["Company_id"].ToString();
        }
        catch
        {

        }
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            ddlDCurrency.SelectedValue = strCurrencyId;
        }
        txtDExchangeRate.Text = "1";

    }
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Account_No");
        dt.Columns.Add("Other_Account_No");
        dt.Columns.Add("Debit_Amount");
        dt.Columns.Add("Credit_Amount");
        dt.Columns.Add("Narration");
        dt.Columns.Add("CostCenter_ID");
        dt.Columns.Add("Emp_Id");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("Foreign_Amount");
        dt.Columns.Add("Exchange_Rate");
        dt.Columns.Add("Serial_No");
        return dt;
    }
    public DataTable FillProductDataTabel()
    {
        string strNewSNo = string.Empty;

        DataTable dt = CreateProductDataTable();
        if (GvDetail.Rows.Count > 0)
        {
            for (int i = 0; i < GvDetail.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvDetail.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblgvSNo = (Label)GvDetail.Rows[i].FindControl("lblSNo");
                    HiddenField hdngvAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdngvAccountNo");
                    Label lblgvOtherAccountNo = (Label)GvDetail.Rows[i].FindControl("lblgvOtherAccountNo");
                    HiddenField hdnOtherAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdnOtherAccountNo");
                    Label lblgvDebitAmount = (Label)GvDetail.Rows[i].FindControl("lblgvDebitAmount");
                    Label lblgvCreditAmount = (Label)GvDetail.Rows[i].FindControl("lblgvCreditAmount");
                    Label lblgvNarration = (Label)GvDetail.Rows[i].FindControl("lblgvNarration");
                    Label lblgvCostCenter = (Label)GvDetail.Rows[i].FindControl("lblgvCostCenter");
                    HiddenField hdngvEmployeeId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvEmployeeId");
                    HiddenField hdngvCurrencyId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvCurrencyId");
                    Label lblgvForeignAmount = (Label)GvDetail.Rows[i].FindControl("lblgvForeignAmount");
                    Label lblgvExchangeRate = (Label)GvDetail.Rows[i].FindControl("lblgvExchangeRate");

                    dt.Rows[i]["Trans_Id"] = lblgvSNo.Text;
                    strNewSNo = lblgvSNo.Text;
                    dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
                    dt.Rows[i]["Account_No"] = hdngvAccountNo.Value;
                    dt.Rows[i]["Other_Account_No"] = hdnOtherAccountNo.Value;
                    dt.Rows[i]["Debit_Amount"] = lblgvDebitAmount.Text;
                    dt.Rows[i]["Credit_Amount"] = lblgvCreditAmount.Text;
                    dt.Rows[i]["Narration"] = lblgvNarration.Text;
                    dt.Rows[i]["CostCenter_ID"] = lblgvCostCenter.Text;
                    dt.Rows[i]["Emp_Id"] = hdngvEmployeeId.Value;
                    dt.Rows[i]["Currency_Id"] = hdngvCurrencyId.Value;
                    dt.Rows[i]["Foreign_Amount"] = lblgvForeignAmount.Text;
                    dt.Rows[i]["Exchange_Rate"] = lblgvExchangeRate.Text;
                }
                else
                {
                    DataTable DtMaxSerial = new DataTable();
                    try
                    {
                        DtMaxSerial.Merge(dt);
                        DtMaxSerial = new DataView(DtMaxSerial, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }

                    dt.Rows.Add(i);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows[i]["Trans_Id"] = (float.Parse(DtMaxSerial.Rows[0]["Trans_Id"].ToString()) + 1).ToString();
                    }
                    else
                    {
                        dt.Rows[i]["Trans_Id"] = "1";
                    }
                    dt.Rows[i]["Serial_No"] = "1";
                    dt.Rows[i]["Account_No"] = hdnNewAccountNo.Value;
                    if (trCustomer.Visible == true)
                    {
                        dt.Rows[i]["Other_Account_No"] = txtCustomerName.Text.Split('/')[1].ToString();
                    }
                    else if (trSupplier.Visible == true)
                    {
                        dt.Rows[i]["Other_Account_No"] = txtSupplierName.Text.Split('/')[1].ToString();
                    }
                    else
                    {
                        dt.Rows[i]["Other_Account_No"] = "0";
                    }
                    dt.Rows[i]["Debit_Amount"] = txtDebitAmount.Text;
                    dt.Rows[i]["Credit_Amount"] = txtCreditAmount.Text;
                    dt.Rows[i]["Narration"] = txtNarration.Text;
                    dt.Rows[i]["CostCenter_ID"] = txtCostCenter.Text;
                    dt.Rows[i]["Emp_Id"] = hdnEmployeeId.Value;
                    dt.Rows[i]["Currency_Id"] = hdnCurrencyId.Value;
                    dt.Rows[i]["Foreign_Amount"] = txtForeignAmount.Text;
                    dt.Rows[i]["Exchange_Rate"] = txtDExchangeRate.Text;
                }
            }
        }
        else
        {

            dt.Rows.Add(0);
            dt.Rows[0]["Trans_Id"] = "1";
            dt.Rows[0]["Serial_No"] = "1";
            dt.Rows[0]["Account_No"] = hdnNewAccountNo.Value;

            if (trCustomer.Visible == true)
            {
                dt.Rows[0]["Other_Account_No"] = txtCustomerName.Text.Split('/')[1].ToString();
            }
            else if (trSupplier.Visible == true)
            {
                dt.Rows[0]["Other_Account_No"] = txtSupplierName.Text.Split('/')[1].ToString();
            }
            else
            {
                dt.Rows[0]["Other_Account_No"] = "0";
            }

            //dt.Rows[0]["Other_Account_No"] = txtOtherAccountNo.Text;
            dt.Rows[0]["Debit_Amount"] = txtDebitAmount.Text;
            dt.Rows[0]["Credit_Amount"] = txtCreditAmount.Text;
            dt.Rows[0]["Narration"] = txtNarration.Text;
            dt.Rows[0]["CostCenter_ID"] = txtCostCenter.Text;
            dt.Rows[0]["Emp_Id"] = hdnEmployeeId.Value;
            dt.Rows[0]["Currency_Id"] = hdnCurrencyId.Value;
            dt.Rows[0]["Foreign_Amount"] = txtForeignAmount.Text;
            dt.Rows[0]["Exchange_Rate"] = txtDExchangeRate.Text;
        }

        string strCurrency = Session["LocCurrencyId"].ToString();
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvDetail, dt, "", "");

            foreach (GridViewRow gvr in GvDetail.Rows)
            {
                Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
                Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");
                Label lblgvFrgnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
                Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");


                lblgvDebitAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
                lblgvCreditAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditAmt.Text);
                lblgvFrgnAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvFrgnAmt.Text);
                lblgvExchangeRate.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvExchangeRate.Text);
            }
        }
        return dt;
    }
    public DataTable FillProductDataTabelDelete()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvDetail.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvDetail.Rows[i].FindControl("lblSNo");
            HiddenField hdngvAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdngvAccountNo");
            Label lblgvOtherAccountNo = (Label)GvDetail.Rows[i].FindControl("lblgvOtherAccountNo");
            HiddenField hdnOtherAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdnOtherAccountNo");
            Label lblgvDebitAmount = (Label)GvDetail.Rows[i].FindControl("lblgvDebitAmount");
            Label lblgvCreditAmount = (Label)GvDetail.Rows[i].FindControl("lblgvCreditAmount");
            Label lblgvNarration = (Label)GvDetail.Rows[i].FindControl("lblgvNarration");
            Label lblgvCostCenter = (Label)GvDetail.Rows[i].FindControl("lblgvCostCenter");
            HiddenField hdngvEmployeeId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvEmployeeId");
            HiddenField hdngvCurrencyId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvCurrencyId");
            Label lblgvForeignAmount = (Label)GvDetail.Rows[i].FindControl("lblgvForeignAmount");
            Label lblgvExchangeRate = (Label)GvDetail.Rows[i].FindControl("lblgvExchangeRate");

            dt.Rows[i]["Trans_Id"] = lblgvSNo.Text;
            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["Account_No"] = hdngvAccountNo.Value;
            dt.Rows[i]["Other_Account_No"] = hdnOtherAccountNo.Value;
            dt.Rows[i]["Debit_Amount"] = lblgvDebitAmount.Text;
            dt.Rows[i]["Credit_Amount"] = lblgvCreditAmount.Text;
            dt.Rows[i]["Narration"] = lblgvNarration.Text;
            dt.Rows[i]["CostCenter_ID"] = lblgvCostCenter.Text;
            dt.Rows[i]["Emp_Id"] = hdngvEmployeeId.Value;
            dt.Rows[i]["Currency_Id"] = hdngvCurrencyId.Value;
            dt.Rows[i]["Foreign_Amount"] = lblgvForeignAmount.Text;
            dt.Rows[i]["Exchange_Rate"] = lblgvExchangeRate.Text;
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id<>'" + hdnAccountNo.Value + "'";
        dt = (DataTable)dv.ToTable();
        return dt;
    }
    protected void imgBtnDetailEdit_Command(object sender, CommandEventArgs e)
    {
        hdnAccountNo.Value = e.CommandArgument.ToString();
        FillProductDataTabelEdit();
    }
    public DataTable FillProductDataTabelEdit()
    {
        DataTable dt = CreateProductDataTable();

        for (int i = 0; i < GvDetail.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvDetail.Rows[i].FindControl("lblSNo");
            HiddenField hdngvAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdngvAccountNo");
            Label lblgvOtherAccountNo = (Label)GvDetail.Rows[i].FindControl("lblgvOtherAccountNo");
            HiddenField hdnOtherAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdnOtherAccountNo");
            Label lblgvDebitAmount = (Label)GvDetail.Rows[i].FindControl("lblgvDebitAmount");
            Label lblgvCreditAmount = (Label)GvDetail.Rows[i].FindControl("lblgvCreditAmount");
            Label lblgvNarration = (Label)GvDetail.Rows[i].FindControl("lblgvNarration");
            Label lblgvCostCenter = (Label)GvDetail.Rows[i].FindControl("lblgvCostCenter");
            HiddenField hdngvEmployeeId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvEmployeeId");
            HiddenField hdngvCurrencyId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvCurrencyId");
            Label lblgvForeignAmount = (Label)GvDetail.Rows[i].FindControl("lblgvForeignAmount");
            Label lblgvExchangeRate = (Label)GvDetail.Rows[i].FindControl("lblgvExchangeRate");

            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["Trans_Id"] = lblgvSNo.Text;
            dt.Rows[i]["Account_No"] = hdngvAccountNo.Value;
            dt.Rows[i]["Other_Account_No"] = hdnOtherAccountNo.Value;
            dt.Rows[i]["Debit_Amount"] = lblgvDebitAmount.Text;
            dt.Rows[i]["Credit_Amount"] = lblgvCreditAmount.Text;
            dt.Rows[i]["Narration"] = lblgvNarration.Text;
            dt.Rows[i]["CostCenter_ID"] = lblgvCostCenter.Text;
            dt.Rows[i]["Emp_Id"] = hdngvEmployeeId.Value;
            dt.Rows[i]["Currency_Id"] = hdngvCurrencyId.Value;
            dt.Rows[i]["Foreign_Amount"] = lblgvForeignAmount.Text;
            dt.Rows[i]["Exchange_Rate"] = lblgvExchangeRate.Text;
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id='" + hdnAccountNo.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Account_No"].ToString() != "0" && dt.Rows[0]["Account_No"].ToString() != "")
            {
                string strAccountNo = dt.Rows[0]["Account_No"].ToString();
                txtAccountNo.Text = GetAccountNameByTransId(strAccountNo) + "/" + strAccountNo;
                hdnAccountName.Value = GetAccountNameByTransId(strAccountNo);
            }
            else
            {
                txtAccountNo.Text = "";
            }

            string strOtherAccNo = dt.Rows[0]["Other_Account_No"].ToString();
            if (strOtherAccNo != "0" && strOtherAccNo != "")
            {
                //for Customer & Supplier Account
                string strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

                if (txtAccountNo.Text.Split('/')[1].ToString() == strReceiveVoucherAcc)
                {
                    trSupplier.Visible = false;
                    trCustomer.Visible = true;
                    txtSupplierName.Text = "";
                    txtCustomerName.Text = Ac_ParameterMaster.GetOtherAccountNameForDetail(strOtherAccNo, strReceiveVoucherAcc, Session["CompId"].ToString(), Session["DBConnection"].ToString()) + "/" + strOtherAccNo;
                    txtCustomerName.Focus();
                }
                else if (txtAccountNo.Text.Split('/')[1].ToString() == strPaymentVoucherAcc)
                {
                    trSupplier.Visible = true;
                    trCustomer.Visible = false;
                    txtSupplierName.Text = Ac_ParameterMaster.GetOtherAccountNameForDetail(strOtherAccNo, strReceiveVoucherAcc, Session["CompId"].ToString(), Session["DBConnection"].ToString()) + "/" + strOtherAccNo;
                    txtCustomerName.Text = "";
                    txtSupplierName.Focus();
                }
                else
                {
                    trSupplier.Visible = false;
                    trCustomer.Visible = false;
                    txtSupplierName.Text = "";
                    txtCustomerName.Text = "";
                }
            }
            else
            {
                trSupplier.Visible = false;
                trCustomer.Visible = false;
                txtSupplierName.Text = "";
                txtCustomerName.Text = "";
            }


            txtDebitAmount.Text = dt.Rows[0]["Debit_Amount"].ToString();
            txtCreditAmount.Text = dt.Rows[0]["Credit_Amount"].ToString();
            txtNarration.Text = dt.Rows[0]["Narration"].ToString();
            txtCostCenter.Text = dt.Rows[0]["CostCenter_ID"].ToString();



            if (dt.Rows[0]["Emp_Id"].ToString() != "0" && dt.Rows[0]["Emp_Id"].ToString() != "")
            {
                string strEmployeeId = dt.Rows[0]["Emp_Id"].ToString();
                txtEmployee.Text = GetEmployeeName(strEmployeeId) + "/" + strEmployeeId;
                hdnEmployeeId.Value = strEmployeeId;
            }
            else
            {
                txtEmployee.Text = "";
            }

            if (dt.Rows[0]["Currency_Id"].ToString() != "0" && dt.Rows[0]["Currency_Id"].ToString() != "")
            {
                FillDetailCurrency();
                string strCurrencyId = dt.Rows[0]["Currency_Id"].ToString();
                ddlDCurrency.SelectedValue = strCurrencyId;
                hdnCurrencyId.Value = strCurrencyId;
            }
            else
            {
                FillDetailCurrency();
            }

            txtForeignAmount.Text = dt.Rows[0]["Foreign_Amount"].ToString();
            txtDExchangeRate.Text = dt.Rows[0]["Exchange_Rate"].ToString();
        }
        return dt;
    }
    protected void imgBtnDetailDelete_Command(object sender, CommandEventArgs e)
    {
        hdnAccountNo.Value = e.CommandArgument.ToString();
        FillProductChidGird("Del");
    }
    public void FillProductChidGird(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillProductDataTabelDelete();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillProductDataTableUpdate();
        }
        else
        {
            dt = FillProductDataTabel();
        }
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvDetail, dt, "", "");

        string strCurrency = Session["LocCurrencyId"].ToString();
        foreach (GridViewRow gvr in GvDetail.Rows)
        {
            Label lblgvDebitAmt = (Label)gvr.FindControl("lblgvDebitAmount");
            Label lblgvCreditAmt = (Label)gvr.FindControl("lblgvCreditAmount");
            Label lblgvFrgnAmt = (Label)gvr.FindControl("lblgvForeignAmount");
            Label lblgvExchangeRate = (Label)gvr.FindControl("lblgvExchangeRate");


            lblgvDebitAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvDebitAmt.Text);
            lblgvCreditAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditAmt.Text);
            lblgvFrgnAmt.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvFrgnAmt.Text);
            lblgvExchangeRate.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvExchangeRate.Text);
        }
        ResetDetail();
    }
    public DataTable FillProductDataTableUpdate()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvDetail.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvDetail.Rows[i].FindControl("lblSNo");
            HiddenField hdngvAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdngvAccountNo");
            Label lblgvOtherAccountNo = (Label)GvDetail.Rows[i].FindControl("lblgvOtherAccountNo");
            HiddenField hdnOtherAccountNo = (HiddenField)GvDetail.Rows[i].FindControl("hdnOtherAccountNo");
            Label lblgvDebitAmount = (Label)GvDetail.Rows[i].FindControl("lblgvDebitAmount");
            Label lblgvCreditAmount = (Label)GvDetail.Rows[i].FindControl("lblgvCreditAmount");
            Label lblgvNarration = (Label)GvDetail.Rows[i].FindControl("lblgvNarration");
            Label lblgvCostCenter = (Label)GvDetail.Rows[i].FindControl("lblgvCostCenter");
            HiddenField hdngvEmployeeId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvEmployeeId");
            HiddenField hdngvCurrencyId = (HiddenField)GvDetail.Rows[i].FindControl("hdngvCurrencyId");
            Label lblgvForeignAmount = (Label)GvDetail.Rows[i].FindControl("lblgvForeignAmount");
            Label lblgvExchangeRate = (Label)GvDetail.Rows[i].FindControl("lblgvExchangeRate");

            dt.Rows[i]["Trans_Id"] = lblgvSNo.Text;

            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["Account_No"] = hdngvAccountNo.Value;
            dt.Rows[i]["Other_Account_No"] = hdnOtherAccountNo.Value;
            dt.Rows[i]["Debit_Amount"] = lblgvDebitAmount.Text;
            dt.Rows[i]["Credit_Amount"] = lblgvCreditAmount.Text;
            dt.Rows[i]["Narration"] = lblgvNarration.Text;
            dt.Rows[i]["CostCenter_ID"] = lblgvCostCenter.Text;
            dt.Rows[i]["Emp_Id"] = hdngvEmployeeId.Value;
            dt.Rows[i]["Currency_Id"] = hdngvCurrencyId.Value;
            dt.Rows[i]["Foreign_Amount"] = lblgvForeignAmount.Text;
            dt.Rows[i]["Exchange_Rate"] = lblgvExchangeRate.Text;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnAccountNo.Value == dt.Rows[i]["Trans_Id"].ToString())
            {
                dt.Rows[i]["Account_No"] = hdnNewAccountNo.Value;
                if (trCustomer.Visible == true)
                {
                    dt.Rows[i]["Other_Account_No"] = txtCustomerName.Text.Split('/')[1].ToString();
                }
                else if (trSupplier.Visible == true)
                {
                    dt.Rows[i]["Other_Account_No"] = txtSupplierName.Text.Split('/')[1].ToString();
                }
                else
                {
                    dt.Rows[i]["Other_Account_No"] = "0";
                }
                //dt.Rows[i]["Other_Account_No"] = txtOtherAccountNo.Text;
                dt.Rows[i]["Debit_Amount"] = txtDebitAmount.Text;
                dt.Rows[i]["Credit_Amount"] = txtCreditAmount.Text;
                dt.Rows[i]["Narration"] = txtNarration.Text;
                dt.Rows[i]["CostCenter_ID"] = txtCostCenter.Text;
                dt.Rows[i]["Emp_Id"] = hdnEmployeeId.Value;
                dt.Rows[i]["Currency_Id"] = hdnCurrencyId.Value;
                dt.Rows[i]["Foreign_Amount"] = txtForeignAmount.Text;
                dt.Rows[i]["Exchange_Rate"] = txtDExchangeRate.Text;
            }
        }
        return dt;
    }
    protected void ddlDCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDCurrency.SelectedValue != "--Select--")
        {
            hdnCurrencyId.Value = ddlDCurrency.SelectedValue;
        }
        else
        {
            hdnCurrencyId.Value = "0";
        }

        if (ddlDCurrency.SelectedIndex != 0)
        {
            //try
            //{
            //    txtDExchangeRate.Text = Convert.ToDouble(obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), ObjCurrencyMaster.GetCurrencyMasterById(ddlDCurrency.SelectedValue.ToString()).Rows[0]["Currency_Code"].ToString()), (Currency)System.Enum.Parse(Currency.GetType(), "KWD"))).ToString();
            //}
            //catch
            //{
            //    DataTable dt = new DataView(ObjCurrencyMaster.GetCurrencyMaster(), "Currency_Code='" + ddlDCurrency.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    if (dt.Rows.Count != 0)
            //    {
            //        txtDExchangeRate.Text = dt.Rows[0]["Currency_Value"].ToString();
            //    }
            //    else
            //    {
            //        txtDExchangeRate.Text = "0";
            //    }
            //}


            double DebitAmount = Convert.ToDouble(txtDebitAmount.Text);
            double CreditAmount = Convert.ToDouble(txtCreditAmount.Text);
            if (DebitAmount != 0)
            {
                string strFireignExchange = GetCurrency(ddlDCurrency.SelectedValue, DebitAmount.ToString());
                txtForeignAmount.Text = strFireignExchange.Trim().Split('/')[0].ToString();
                txtDExchangeRate.Text = strFireignExchange.Trim().Split('/')[1].ToString();
            }
            else if (CreditAmount != 0)
            {
                string strFireignExchange = GetCurrency(ddlDCurrency.SelectedValue, CreditAmount.ToString());
                txtForeignAmount.Text = strFireignExchange.Trim().Split('/')[0].ToString();
                txtDExchangeRate.Text = strFireignExchange.Trim().Split('/')[1].ToString();
            }
        }
    }
    #endregion

    protected void ddlLocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }

    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }

    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvVoucher, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvVoucher, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
}