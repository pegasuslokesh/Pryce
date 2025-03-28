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
using System.Data.SqlClient;
using PegasusDataAccess;

public partial class AccountSetup_FinancialYearInfo : BasePage
{
    #region defined Class Object
    Common cmn = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_FinancialYear_Detail ObjFDetail = null;
    Ac_FinancialYear_Closing_Detail ObjFClosingDetail = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_SubChartOfAccount objSubCOA = null;
    LocationMaster objLocationMaster = null;
    SystemParameter ObjSysParam = null;
    Set_DocNumber objDocNo = null;
    UserMaster ObjUser = null;
    EmployeeMaster objEmployee = null;
    Inv_StockDetail objstockDetail = null;
    Inv_ProductLedger ObjProductLadger = null;
    PegasusDataAccess.DataAccessClass objDA = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = "admin";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        cmn = new Common(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        ObjFDetail = new Ac_FinancialYear_Detail(Session["DBConnection"].ToString());
        ObjFClosingDetail = new Ac_FinancialYear_Closing_Detail(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
        objLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objstockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objDA = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {


            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../AccountSetup/FinancialYearInfo.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            ddlOption.SelectedIndex = 2;
            btnList_Click(null, null);
            FillGridBin();
            FillGrid();
            FillGridReOpen();
            txtFinanceNo.Text = GetDocumentNumber();
            DataTable dtFinance = new DataTable();
            dtFinance = objFYI.GetInfoByTransId(StrCompId, Session["FinanceYearId"].ToString());
            if (dtFinance.Rows.Count > 0)
            {
                txtCFinanceCode.Text = dtFinance.Rows[0]["Finance_Code"].ToString();
                txtCFromDate.Text = DateTime.Parse(dtFinance.Rows[0]["From_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtCToDate.Text = DateTime.Parse(dtFinance.Rows[0]["To_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    public string GetDocumentNumber()
    {
        string DocumentNo = string.Empty;

        DataTable Dt = objDocNo.GetDocumentNumberAll(StrCompId, "36", "185");

        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["Prefix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Prefix"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["CompId"].ToString()))
            {
                DocumentNo += StrCompId;
            }

            if (Convert.ToBoolean(Dt.Rows[0]["BrandId"].ToString()))
            {
                DocumentNo += StrBrandId;
            }

            if (Convert.ToBoolean(Dt.Rows[0]["LocationId"].ToString()))
            {
                DocumentNo += strLocationId;
            }



            if (Convert.ToBoolean(Dt.Rows[0]["DeptId"].ToString()))
            {
                DocumentNo += (string)Session["SessionDepId"];
            }

            if (Convert.ToBoolean(Dt.Rows[0]["EmpId"].ToString()))
            {
                DataTable Dtuser = ObjUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["LoginCompany"].ToString());
                DocumentNo += Dtuser.Rows[0]["Emp_Id"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Year"].ToString()))
            {
                DocumentNo += DateTime.Now.Year.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Month"].ToString()))
            {
                DocumentNo += DateTime.Now.Month.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Day"].ToString()))
            {
                DocumentNo += DateTime.Now.Day.ToString();
            }

            if (Dt.Rows[0]["Suffix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Suffix"].ToString();
            }

            if (DocumentNo != "")
            {
                DocumentNo += "-" + (Convert.ToInt32(objFYI.GetInfoAll(StrCompId.ToString()).Rows.Count) + 1).ToString();
            }
            else
            {
                DocumentNo += (Convert.ToInt32(objFYI.GetInfoAll(StrCompId.ToString()).Rows.Count) + 1).ToString();
            }
        }
        else
        {
            DocumentNo += (Convert.ToInt32(objFYI.GetInfoAll(StrCompId.ToString()).Rows.Count) + 1).ToString();
        }
        return DocumentNo;
    }

    #region System defined Function
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReOpen.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuClosing.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlReOpen.Visible = false;
        PnlClosing.Visible = false;
        FillGrid();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //Reset();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReOpen.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuClosing.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        PnlReOpen.Visible = false;
        PnlClosing.Visible = false;

        txtFinanceCode.MaxLength = 11;
        pnlMenuList.Visible = true;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dtFYIEdit = objFYI.GetInfoByTransId(StrCompId, e.CommandArgument.ToString());
        if (dtFYIEdit.Rows[0]["Status"].ToString() != "New")
        {
            DisplayMessage("You dont do any action with this Financial Year");
            return;
        }

        if (dtFYIEdit.Rows.Count > 0)
        {
            hdnFYIId.Value = e.CommandArgument.ToString();

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            txtFinanceNo.Text = dtFYIEdit.Rows[0]["Finance_No"].ToString();
            txtFinanceCode.Text = dtFYIEdit.Rows[0]["Finance_Code"].ToString();
            txtFromDate.Text = Convert.ToDateTime(dtFYIEdit.Rows[0]["From_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtToDate.Text = Convert.ToDateTime(dtFYIEdit.Rows[0]["To_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
            txtFinanceCode.Enabled = false;
            txtFinanceNo.Enabled = false;
            txtRemark.Text = dtFYIEdit.Rows[0]["Remark"].ToString();
        }
        else
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
            txtFinanceCode.Enabled = true;
            txtFinanceNo.Enabled = true;
        }
        btnNew_Click(null, null);
        BtnReset.Enabled = false;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFinanceNo);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }

    //Function to set new button enabled / disabled according to status (VS)
    //public string getNewBtnStatus()
    //{
    //    DataTable dtFYStatus = objFYI.GetInfoByStatus(StrCompId);
    //    string str1;
    //    if (dtFYStatus.Rows.Count > 0)
    //    {
    //        Lbl_Tab_New.Enabled = false;
    //        str1 = "False";
    //    }
    //    else
    //    {
    //        Lbl_Tab_New.Enabled = true;
    //        str1 = "True";

    //    }
    //    return str1;
    //}
    //End


    // Function to find Status for Gridview (VS)


    //End 


    protected void btnbindrpt_Click(object sender, EventArgs e)
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

            DataTable dtAdd = (DataTable)Session["dtCurrency"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvFYI, view.ToTable(), "", "");
            Session["dtFilter_Fin_Year_Info"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
        txtValue.Focus();
    }
    protected void GvFYI_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvFYI.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Fin_Year_Info"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvFYI, dt, "", "");
    }
    protected void GvFYI_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Fin_Year_Info"];
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
        Session["dtFilter_Fin_Year_Info"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvFYI, dt, "", "");
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;

        hdnFYIId.Value = e.CommandArgument.ToString();
        DataTable dtFYIEdit = objFYI.GetInfoByTransId(StrCompId, e.CommandArgument.ToString());
        if (dtFYIEdit.Rows[0]["Status"].ToString() != "New")
        {
            DisplayMessage("You cant delete that Record");
            return;
        }
        else
        {
            b = objFYI.DeleteInfo(StrCompId, hdnFYIId.Value, "false", StrUserId, DateTime.Now.ToString());

            if (b != 0)
            {
                DisplayMessage("Record Deleted Successfully");
            }
            else
            {
                DisplayMessage("Record  Not Deleted");
                return;
            }
        }

        FillGridBin(); //Update grid view in bin tab
        FillGrid();
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";
    }
    protected void btnFYICancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        FillGridBin();
        FillGrid();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFinanceNo);
    }
    protected void btnFYISave_Click(object sender, EventArgs e)
    {
        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strUserName = Session["UserId"].ToString();
        string strStatus = string.Empty;
        if (txtFinanceNo.Text == "")
        {
            DisplayMessage("Enter Finance No.");
            txtFinanceNo.Focus();
            return;
        }
        else
        {
            if (hdnFYIId.Value == "0")
            {
                DataTable dtFinanceNo = objFYI.GetInfoByFinanceNo(StrCompId, txtFinanceNo.Text);
                if (dtFinanceNo.Rows.Count > 0)
                {
                    DisplayMessage("Finance No. Already Exits");
                    txtFinanceNo.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFinanceNo);
                    return;
                }
                else
                {
                    DataTable dtmaxdate = objFYI.GetMaxToDate(StrCompId);
                    if (dtmaxdate.Rows.Count != 0)
                    {
                        if (dtmaxdate.Rows[0][0].ToString() != "")
                        {
                            DateTime dt1 = Convert.ToDateTime(txtFromDate.Text);
                            DateTime dt2 = Convert.ToDateTime(dtmaxdate.Rows[0][0].ToString());
                            int result1 = DateTime.Compare(dt1, dt2);
                            double diffdates1;
                            diffdates1 = (dt1 - dt2).TotalDays;
                            if (result1 < 0)
                            {
                                DisplayMessage("Financial Year For this Period Already Exists !");
                                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToDate);
                                return;
                            }
                        }
                    }
                }
            }
        }


        //for status check
        if (hdnFYIId.Value == "0")
        {
            DataTable dtStatus = objFYI.GetInfoAllTrue(StrCompId);
            if (dtStatus.Rows.Count == 0)
            {
                strStatus = "Open";
            }
            else if (dtStatus.Rows.Count > 0)
            {
                dtStatus = new DataView(dtStatus, "Status='Open'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtStatus.Rows.Count > 0)
                {
                    strStatus = "New";
                }
                else
                {
                    strStatus = "Open";
                }
            }
        }
        else
        {
            DataTable dtStatus = objFYI.GetInfoByTransId(StrCompId, hdnFYIId.Value);
            if (dtStatus.Rows.Count > 0)
            {
                strStatus = dtStatus.Rows[0]["Status"].ToString();
            }
            else
            {
                strStatus = "";
            }
        }

        if (txtFinanceCode.Text == "")
        {
            DisplayMessage("Fill Finance Code");
            txtFinanceCode.Focus();
            return;
        }

        if (txtFromDate.Text == "")
        {
            DisplayMessage("Enter From Date");
            txtFromDate.Focus();
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter From Date in format " + Session["DateFormat"].ToString() + "");
                txtFromDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
                return;
            }
        }

        if (txtToDate.Text == "")
        {
            DisplayMessage("Enter To Date");
            txtToDate.Focus();
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter To Date in format " + Session["DateFormat"].ToString() + "");
                txtToDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToDate);
                return;
            }
        }

        //Checks for dates (VS)
        DateTime date1 = Convert.ToDateTime(txtToDate.Text);
        DateTime date2 = Convert.ToDateTime(txtFromDate.Text);
        int result = DateTime.Compare(date1, date2);
        double diffdates;
        diffdates = (date1 - date2).TotalDays;
        if (result < 0)
        {
            DisplayMessage("ToDate Should be Greater than From Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToDate);
            return;
        }
        else if (result == 0)
        {
            DisplayMessage("ToDate and From Date Cannot be Equal");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
            return;
        }
        else
        {
            if (diffdates > 366)
            {
                DisplayMessage("Financial Period cannot  be more than one Year.");
                return;
            }
        }
        //End

        //Check Date Criteria
        if (hdnFYIId.Value == "0")
        {
            if (txtFromDate.Text != "")
            {
                DataTable dtmaxdate = objFYI.GetMaxToDate(StrCompId);
                if (dtmaxdate.Rows.Count > 0)
                {

                    if (dtmaxdate.Rows[0][0].ToString() != "")
                    {
                        DateTime dtLastDate = DateTime.Parse(dtmaxdate.Rows[0][0].ToString());
                        if (dtLastDate.ToString() != "")
                        {
                            DateTime dtFinalDate = DateTime.Parse(dtLastDate.AddDays(1).ToString());
                            if (DateTime.Parse(txtFromDate.Text) == DateTime.Parse(dtFinalDate.ToString()))
                            {

                            }
                            else
                            {
                                DisplayMessage("Your Entered from date Not According to Previous Financial Year");
                                return;
                            }
                        }
                    }
                }
            }
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            if (hdnFYIId.Value != "0")
            {
                b = objFYI.UpdateInfo(StrCompId, hdnFYIId.Value, txtFinanceNo.Text, txtFinanceCode.Text, ObjSysParam.getDateForInput(txtFromDate.Text).ToString(), ObjSysParam.getDateForInput(txtToDate.Text).ToString(), strStatus, "", "", "1/1/1800", txtRemark.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (b != 0)
                {
                    DisplayMessage("Record Updated Successfully !", "green");

                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
            }
            else
            {
                b = objFYI.InsertInfo(StrCompId, txtFinanceNo.Text, txtFinanceCode.Text, ObjSysParam.getDateForInput(txtFromDate.Text).ToString(), ObjSysParam.getDateForInput(txtToDate.Text).ToString(), strStatus, "", "", "1/1/1800", txtRemark.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    DataTable dtMaxId = objFYI.GetMaxIdByCompanyId(ref trns);
                    if (dtMaxId.Rows.Count > 0)
                    {
                        string strMaxId = dtMaxId.Rows[0][0].ToString();
                        if (strMaxId != "")
                        {
                            DataTable dtLocation = objLocationMaster.GetLocationMaster(StrCompId);
                            if (dtLocation.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtLocation.Rows.Count; i++)
                                {
                                    ObjFDetail.InsertFinancialYearDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), dtLocation.Rows[i]["Location_Id"].ToString(), strMaxId, strStatus, "", strStatus, "", "1/1/1800", txtRemark.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                            }
                        }
                    }

                    DisplayMessage("Record Saved Successfully !", "green");

                }
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            btnList_Click(null, null);
            FillGrid();
            Reset();
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
    protected void btnUpdate_Click(object sender, EventArgs e)
    {


        if (GvReOpen.Rows.Count == 0)
        {
            DisplayMessage("You have no record for Re Open");
            return;
        }


        bool Result = false;
        bool ResultChild = false;

        foreach (GridViewRow gv in GvReOpen.Rows)
        {
            CheckBox chkgvReopen = (CheckBox)gv.FindControl("chkgvReOpen");

            if (chkgvReopen.Checked)
            {
                Result = true;
            }
        }


        if (!Result)
        {
            DisplayMessage("Select atleast one record for ReOpen");
            return;
        }



        foreach (GridViewRow gv in gvReopenChild.Rows)
        {
            CheckBox chkgvReopen = (CheckBox)gv.FindControl("chkgvReOpen");

            if (chkgvReopen.Checked)
            {
                ResultChild = true;
            }
        }

        if (!ResultChild)
        {
            DisplayMessage("Select atleast one location for ReOpen");
            return;
        }

        string strSQL = string.Empty;

        foreach (GridViewRow gvr in GvReOpen.Rows)
        {

            CheckBox chkgvHeaderReopen = (CheckBox)gvr.FindControl("chkgvReOpen");

            if (!chkgvHeaderReopen.Checked)
            {
                continue;
            }

            Label lblgvStatus = (Label)gvr.FindControl("lblgvStatus");


            if (lblgvStatus.Text.Trim().ToUpper() == "CLOSE")
            {
                strSQL = "Update Ac_Finance_Year_Info set Status='ReOpen',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id='" + ((Label)gvr.FindControl("lblgvTransId")).Text + "'";
                objDA.execute_Command(strSQL);
            }

            foreach (GridViewRow gvrChild in gvReopenChild.Rows)
            {

                CheckBox chkgvReopen = (CheckBox)gvrChild.FindControl("chkgvReOpen");
                //in case current year is open

                if (lblgvStatus.Text.Trim().ToUpper() == "OPEN")
                {
                    if (chkgvReopen.Checked)
                    {
                        strSQL = "UPDATE Ac_FinancialYear_Detail SET STATUS='Open',Inv_Status='Open',Inv_Closed_By=' ',Inv_Closed_Date='1800-01-01 00:00:00.000',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' WHERE TRANS_ID=" + ((HiddenField)gvrChild.FindControl("hdnTransId")).Value + "";
                        objDA.execute_Command(strSQL);
                    }

                }

                //in case current year is close

                if (lblgvStatus.Text.Trim().ToUpper() == "CLOSE")
                {
                    if (chkgvReopen.Checked)
                    {

                        ObjFDetail.InsertFinancialYearDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrChild.FindControl("hdnLocationId")).Value, ((Label)gvr.FindControl("lblgvTransId")).Text, "ReOpen", "", "ReOpen", "", "1/1/1800", "Financial Year ReOpen", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }
                    else
                    {

                        ObjFDetail.InsertFinancialYearDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrChild.FindControl("hdnLocationId")).Value, ((Label)gvr.FindControl("lblgvTransId")).Text, "Close", "", "Close", "", "1/1/1800", " ", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }

                }

                //in case current year is ReOpen

                if (lblgvStatus.Text.Trim().ToUpper() == "REOPEN")
                {

                    if (chkgvReopen.Checked)
                    {

                        if (chkgvReopen.Checked)
                        {
                            strSQL = "UPDATE Ac_FinancialYear_Detail SET STATUS='ReOpen',Closing_Status=' ',Inv_Status='ReOpen',Inv_Closed_By=' ',Inv_Closed_Date='1800-01-01 00:00:00.000',Remark='Financial Year ReOpen',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "'  WHERE TRANS_ID=" + ((HiddenField)gvrChild.FindControl("hdnTransId")).Value + "";
                            objDA.execute_Command(strSQL);
                        }

                    }
                }
            }
        }


        DisplayMessage("Your Financial Year Successfully Re Open");
        btnReOpen_Click(null, null);
        return;

        //if (GvReOpen.Rows.Count > 0)
        //{
        //    int q = 0;
        //    foreach (GridViewRow gv in GvReOpen.Rows)
        //    {
        //        Label lblgvTrans_Id = (Label)gv.FindControl("lblgvTransId");
        //        CheckBox chkgvReopen = (CheckBox)gv.FindControl("chkgvReOpen");
        //        if (chkgvReopen.Checked == true)
        //        {

        //            if (q != 0)
        //            {
        //                DataTable dtLocation = objLocationMaster.GetLocationMaster(StrCompId);
        //                if (dtLocation.Rows.Count > 0)
        //                {
        //                    for (int i = 0; i < dtLocation.Rows.Count; i++)
        //                    {
        //                        ObjFDetail.InsertFinancialYearDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), dtLocation.Rows[i]["Location_Id"].ToString(), lblgvTrans_Id.Text, "ReOpen", "", "ReOpen", "", "1/1/1800", "Financial Year ReOpen", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        //                    }
        //                }
        //            }
        //            break;
        //        }
        //    }

        //    if (q != 0)
        //    {
        //        DisplayMessage("Your Financial Year Successfully Re Open");
        //        btnReOpen_Click(null, null);
        //        return;
        //    }
        //    else
        //    {
        //        DisplayMessage("Your Financial Year Not Re Open");
        //        return;
        //    }
        //}
        //else
        //{
        //    DisplayMessage("You have no record for Re Open");
        //    return;
        //}
    }
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        hdnFYIId.Value = e.CommandArgument.ToString();

        int b = 0;
        b = objFYI.DeleteInfo(StrCompId, hdnFYIId.Value, true.ToString(), StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
        FillGrid();
        FillGridBin();
        Reset();
    }

    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReOpen.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuClosing.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        PnlReOpen.Visible = false;
        PnlClosing.Visible = false;
        FillGridBin();
    }
    protected void btnReOpen_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReOpen.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuClosing.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlList.Visible = false;
        PnlReOpen.Visible = true;
        PnlClosing.Visible = false;
        FillGridReOpen();
        objPageCmn.FillData((object)gvReopenChild, null, "", "");
        Div_Re_Open.Visible = false;
    }
    protected void btnClosing_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReOpen.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuClosing.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        PnlList.Visible = false;
        PnlReOpen.Visible = false;
        PnlClosing.Visible = true;
        FillGridReOpen();
    }
    protected void GvFYIBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvFYIBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvFYIBin, dt, "", "");

        string temp = string.Empty;

        for (int i = 0; i < GvFYIBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvFYIBin.Rows[i].FindControl("lblgvTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvFYIBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
    }
    protected void GvFYIBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objFYI.GetInfoAllFalse(StrCompId);
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvFYIBin, dt, "", "");
        lblSelectedRecord.Text = "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objFYI.GetInfoAllFalse(StrCompId);
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvFYIBin, dt, "", "");
        Session["dtPBrandBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            //ImgbtnSelectAll.Visible = false;
            //imgBtnRestore.Visible = false;
        }
        else
        {
            //ImgbtnSelectAll.Visible = false;
            //imgBtnRestore.Visible = true;
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


            DataTable dtCust = (DataTable)Session["dtPBrandBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvFYIBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dt = objFYI.GetInfoAllFalse(StrCompId);

        if (GvFYIBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        //Msg = objTax.DeleteTaxMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        b = objFYI.DeleteInfo(StrCompId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
                foreach (GridViewRow Gvr in GvFYIBin.Rows)
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
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvFYIBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvFYIBin.Rows.Count; i++)
        {
            ((CheckBox)GvFYIBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvFYIBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvFYIBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvFYIBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvFYIBin.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvFYIBin.Rows[index].FindControl("chkSelect")).Checked)
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
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtInactive"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvFYIBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvFYIBin.Rows[i].FindControl("lblgvTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvFYIBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvFYIBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objFYI.DeleteInfo(StrCompId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            int fleg = 0;
            foreach (GridViewRow Gvr in GvFYIBin.Rows)
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
                DisplayMessage("Please Select at least one Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }
    }
    #endregion

    #endregion

    #region User defined Function
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
    private void FillGrid()
    {
        DataTable dtBrand = objFYI.GetInfoAllTrue(StrCompId);
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        Session["dtCurrency"] = dtBrand;
        Session["dtFilter_Fin_Year_Info"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvFYI, dtBrand, "", "");
        }
        else
        {
            GvFYI.DataSource = null;
            GvFYI.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count.ToString() + "";
    }
    private void FillGridReOpen()
    {
        DataTable dtBrand = objFYI.GetInfoAllTrue(StrCompId);
        dtBrand = new DataView(dtBrand, "Status='Open' or Status='Close' or Status='ReOpen'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtBrand.Rows.Count > 0)
        {
            objPageCmn.FillData((object)GvReOpen, dtBrand, "", "");
        }
        else
        {
            GvReOpen.DataSource = null;
            GvReOpen.DataBind();
        }

        string strId = string.Empty;
        if (dtBrand.Rows.Count > 0)
        {
            DataTable dtOpen = new DataView(dtBrand, "Status='Open'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtOpen.Rows.Count > 0)
            {
                DateTime dtFinalDate = DateTime.Parse(dtOpen.Rows[0]["From_Date"].ToString()).AddDays(-1);
                dtBrand = new DataView(dtBrand, "To_Date='" + dtFinalDate + "' and Status='Close'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtBrand.Rows.Count > 0)
                {
                    strId = dtBrand.Rows[0]["Trans_Id"].ToString();
                }
            }
        }

        foreach (GridViewRow gvr in GvReOpen.Rows)
        {
            Label lblGvTrans_Id = (Label)gvr.FindControl("lblgvTransId");
            CheckBox chkReOpen = (CheckBox)gvr.FindControl("chkgvReOpen");
            Label lblGvstatus = (Label)gvr.FindControl("lblgvStatus");

            if (lblGvstatus.Text.Trim() == "Open" || lblGvstatus.Text.Trim() == "ReOpen")
            {
                chkReOpen.Visible = true;
            }
            else
            {

                if (strId != "")
                {
                    if (lblGvTrans_Id.Text == strId)
                    {
                        chkReOpen.Visible = true;
                    }
                    else
                    {
                        chkReOpen.Visible = false;
                    }
                }
                else
                {
                    chkReOpen.Visible = false;
                }
            }
        }
    }

    protected void chkgvReOpen_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow Gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;
        gvReopenChild.DataSource = null;
        gvReopenChild.DataBind();
        Div_Re_Open.Visible = false;
        if (((CheckBox)Gvrow.FindControl("chkgvReOpen")).Checked)
        {
            string strSQL = "Select * from Ac_FinancialYear_Detail where Trans_Id in (Select Max(Trans_Id) from Ac_FinancialYear_Detail where Header_Trans_Id='" + ((Label)Gvrow.FindControl("lblgvTransId")).Text + "' Group by Location_Id)";
            DataTable dtDetail = objDA.return_DataTable(strSQL);
            //dtDetail = new DataView(dtDetail, "Field7='" + strMaxDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtDetail.Rows.Count > 0)
            {
                gvReopenChild.DataSource = dtDetail;
                gvReopenChild.DataBind();
                Div_Re_Open.Visible = true;
            }
            else
            {
                Div_Re_Open.Visible = false;
                DisplayMessage("You have no Arrenged Data");
                return;
            }
        }
        foreach (GridViewRow gvchildrow in GvReOpen.Rows)
        {
            if (((Label)Gvrow.FindControl("lblgvTransId")).Text != ((Label)gvchildrow.FindControl("lblgvTransId")).Text)
            {
                ((CheckBox)gvchildrow.FindControl("chkgvReOpen")).Checked = false;
            }
        }


    }



    protected void gvReopenChild_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((Label)e.Row.FindControl("lblgvFinanceStatus")).Text == "Close")
            {
                ((CheckBox)e.Row.FindControl("chkgvReOpen")).Enabled = true;
            }
            else
            {
                ((CheckBox)e.Row.FindControl("chkgvReOpen")).Enabled = false;
            }
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
    public void Reset()
    {
        FillGrid();
        txtFinanceNo.Text = GetDocumentNumber();
        txtFinanceCode.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";

        txtFromDate.Enabled = true;
        txtToDate.Enabled = true;
        txtFinanceCode.Enabled = true;
        txtFinanceNo.Enabled = true;

        txtRemark.Text = "";
        PnlNewContant.Enabled = true;
        hdnFYIId.Value = "0";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }
    #endregion

    #region CompanyClosingFY
    protected void btnExceute_Click(object sender, EventArgs e)
    {
        string strStatus = objFYI.GetInfoByTransId(Session["CompId"].ToString(), Session["FinanceYearId"].ToString()).Rows[0]["Status"].ToString();
        if (strStatus == "Open" || strStatus == "ReOpen")
        {
            DataTable dtDetail = ObjFDetail.GetAllDataByHeader_Id(Session["FinanceYearId"].ToString());
            if (dtDetail.Rows.Count > 0)
            {
                //string strSQL = "Select * from Ac_FinancialYear_Detail where Trans_Id in (Select Max(Trans_Id) from Ac_FinancialYear_Detail where Header_Trans_Id='" + Session["FinanceYearId"].ToString() + "' Group by Location_Id)";
                string strSQL = "Select * from Ac_FinancialYear_Detail where Trans_Id in (Select Max(Trans_Id) from Ac_FinancialYear_Detail where Header_Trans_Id='" + Session["FinanceYearId"].ToString() + "'  and Location_Id IN (Select Location_Id from Set_LocationMaster Where Isactive ='1') Group by Location_Id) and Company_Id='"+ Session["CompId"].ToString() + "'";
                dtDetail = objDA.return_DataTable(strSQL);
                //dtDetail = new DataView(dtDetail, "Field7='" + strMaxDate + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtDetail.Rows.Count > 0)
                {
                    GvClosingLocation.DataSource = dtDetail;
                    GvClosingLocation.DataBind();

                    //set visibile false inventory related column if inventory is not there
                }
                else
                {
                    DisplayMessage("You have no Arrenged Data");
                    return;
                }
            }
            else
            {
                DisplayMessage("You have no Data");
                return;
            }
        }
        else
        {
            DisplayMessage("You cant get data According to Login Financial Year");
            return;
        }
    }
    protected void btnCloseYear_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        string strFinancialYearStatus = objFYI.GetInfoByTransId(Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Status"].ToString();
        DataTable dtFinanceheader = objFYI.GetInfoAllTrue(Session["CompId"].ToString());

        //here we check that next financial year exist or not 
        ///code added by jitendra
        ///17-12-2016

        //code start
        if (new DataView(dtFinanceheader, "From_Date>='" + Convert.ToDateTime(Session["FinanceTodate"].ToString()).ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
        {
            DisplayMessage("Next Financial Year Not Found");
            return;
        }
        else
        {
            Session["NextFinancialYearId"] = new DataView(dtFinanceheader, "From_Date>='" + Convert.ToDateTime(Session["FinanceTodate"].ToString()).ToString() + "'", "Trans_Id", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString();
        }
        //code end

        //validation for check that all location of next financeial year i dshould be open status in case of current financial year id is Reopen

        //code start

        if (strFinancialYearStatus.Trim().ToUpper() == "REOPEN")
        {
            DataTable dtFinanceDetail = ObjFDetail.GetAllDataByHeader_Id(Session["NextFinancialYearId"].ToString());

            DataTable DtTemp = new DataView(dtFinanceDetail, "Status='Close' or Inv_Status='Close'", "", System.Data.DataViewRowState.CurrentRows).ToTable();

            if (DtTemp.Rows.Count > 0)
            {
                DisplayMessage("Next finacial year is closed on location level so you can not proceed for reopen");
                return;
            }
        }


        //code end

        Common.clsApplicationModules _clsAppModules = (Common.clsApplicationModules)Session["clsApplicationModule"];

        if (GvClosingLocation.Rows.Count > 0)
        {
            string strStatus = string.Empty;
            foreach (GridViewRow gvr in GvClosingLocation.Rows)
            {
                Label lblFinanceStatus = (Label)gvr.FindControl("lblgvFinanceStatus");
                Label lblInventoryStatus = (Label)gvr.FindControl("lblgvInventoyStatus");

                if (lblFinanceStatus.Text == "Close")
                {
                    if (_clsAppModules.isInventoryModule)
                    {
                        if (lblInventoryStatus.Text == "Close")
                        {
                            strStatus = "Close";
                        }
                        else
                        {
                            DisplayMessage("Need to Close Locations Financial Year");
                            strStatus = "";
                            return;
                        }
                    }
                    else
                    {
                        strStatus = "Close";
                    }
                }
                else
                {
                    DisplayMessage("Need to Close Locations Financial Year");
                    strStatus = "";
                    return;
                }
            }

            DataTable dtFilter = new DataTable();

            InventoryDataSet rptdata = new InventoryDataSet();

            rptdata.EnforceConstraints = false;
            InventoryDataSetTableAdapters.sp_Inv_StockDetail_SelectRow_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_StockDetail_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(rptdata.sp_Inv_StockDetail_SelectRow_Report);

            dtFilter = rptdata.sp_Inv_StockDetail_SelectRow_Report;

            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();

            try
            {
                int i = 0;
                if (strStatus == "Close")
                {
                    string strSQL = "Update Ac_Finance_Year_Info set Status='Close',ModifiedBy='" + Session["UserId"].ToString() + "', ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id='" + Session["FinanceYearId"].ToString() + "'";
                    i = objDA.execute_Command(strSQL, ref trns);
                    if (i != 0)
                    {
                        //check Entries First
                        objSubCOA.DeleteSubCOADetailByFYI(HttpContext.Current.Session["NextFinancialYearId"].ToString(), ref trns);
                        foreach (GridViewRow gvr in GvClosingLocation.Rows)
                        {
                            HiddenField hdnDetailId = (HiddenField)gvr.FindControl("hdnTransId");

                            if (hdnDetailId.Value != "0" && hdnDetailId.Value != "")
                            {
                                DataTable dtDetailClosing = ObjFClosingDetail.GetAllDataByDetailClosingByDetailTrans_Id(hdnDetailId.Value, ref trns);
                                if (dtDetailClosing.Rows.Count > 0)
                                {
                                    dtDetailClosing = new DataView(dtDetailClosing, "Type='F'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtDetailClosing.Rows.Count > 0)
                                    {
                                        for (int C = 0; C < dtDetailClosing.Rows.Count; C++)
                                        {
                                            string strDCCompanyId = dtDetailClosing.Rows[C]["Company_Id"].ToString();
                                            string strDCBrandId = dtDetailClosing.Rows[C]["Brand_Id"].ToString();
                                            string strDCLocationId = dtDetailClosing.Rows[C]["Location_Id"].ToString();
                                            string strDCAccountId = dtDetailClosing.Rows[C]["Account_No"].ToString();
                                            string strDCOtherAccountId = dtDetailClosing.Rows[C]["Other_Account_No"].ToString();
                                            string strDCAmount = dtDetailClosing.Rows[C]["Closing_Amount"].ToString();
                                            string strDCType = dtDetailClosing.Rows[C]["Field1"].ToString();
                                            string strDCForeignAmount = dtDetailClosing.Rows[C]["Field2"].ToString();
                                            string strDCCompanyAmount = dtDetailClosing.Rows[C]["Field3"].ToString();
                                            string strDCCurrencyId = dtDetailClosing.Rows[C]["Field4"].ToString();

                                            if (strDCCompanyAmount == "")
                                            {
                                                strDCCompanyAmount = "0";
                                            }
                                            else
                                            {
                                                strDCCompanyAmount = ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Math.Abs(double.Parse(strDCCompanyAmount)).ToString());
                                            }


                                            if (strDCForeignAmount == "")
                                            {
                                                strDCForeignAmount = "0";
                                            }
                                            else
                                            {
                                                strDCForeignAmount = ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Math.Abs(double.Parse(strDCForeignAmount)).ToString());
                                            }



                                            if (strDCType == "DR")
                                            {
                                                objSubCOA.InsertSubCOA(Session["NextFinancialYearId"].ToString(), strDCCompanyId, strDCBrandId, strDCLocationId, strDCAccountId, strDCOtherAccountId, strDCAmount, "0.00", strDCForeignAmount, "0.00", strDCCurrencyId, strDCCompanyAmount, "0.00", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                            else if (strDCType == "CR")
                                            {
                                                objSubCOA.InsertSubCOA(Session["NextFinancialYearId"].ToString(), strDCCompanyId, strDCBrandId, strDCLocationId, strDCAccountId, strDCOtherAccountId, "0.00", strDCAmount, "0.00", strDCForeignAmount, strDCCurrencyId, "0.00", strDCCompanyAmount, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                }


                                //code added by jitendra upadhyay for inventory closing 
                                //16-12-2016
                                DataTable dtStock = new DataView(dtFilter, "Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + " and Location_Id=" + ((HiddenField)gvr.FindControl("hdnLocationId")).Value + "", "", DataViewRowState.CurrentRows).ToTable();
                                string strBreak;
                                if (((HiddenField)gvr.FindControl("hdnLocationId")).Value.ToString() == "6")
                                {

                                    strBreak = "";
                                }

                                //foreach (DataRow dr in dtStock.Rows)
                                //{
                                //    if (strFinancialYearStatus.Trim().ToUpper() == "OPEN")
                                //    {
                                //        if (dr["ProductId"].ToString() == "3385")
                                //        {
                                //            strBreak = "";
                                //        }
                                //        objstockDetail.InsertStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvr.FindControl("hdnLocationId")).Value, dr["ProductId"].ToString(), dr["Quantity"].ToString(), "0", "0", "0", "0", "0", "0", "0", "0", dr["LastPrice"].ToString(), dr["UnitCost"].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["NextFinancialYearId"].ToString(), ref trns);
                                //        ObjProductLadger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvr.FindControl("hdnLocationId")).Value, "OP", "0", "0", dr["ProductId"].ToString(), dr["Unit_Id"].ToString(), "I", "0", "0", dr["Quantity"].ToString(), "0", "1/1/1800", dr["UnitCost"].ToString() == "" ? "0" : dr["UnitCost"].ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["NextFinancialYearId"].ToString(), ref trns);
                                //    }
                                //    else if (strFinancialYearStatus.Trim().ToUpper() == "REOPEN")
                                //    {
                                //        double NextFianceOpeningStock = 0;
                                //        double CurrentStock = 0;

                                //        double NextYearStock = 0;
                                //        try
                                //        {
                                //            NextFianceOpeningStock = Convert.ToDouble(objstockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvr.FindControl("hdnLocationId")).Value, HttpContext.Current.Session["NextFinancialYearId"].ToString(), dr["ProductId"].ToString(), ref trns).Rows[0]["OpeningBalance"].ToString());
                                //        }
                                //        catch
                                //        {

                                //        }
                                //        try
                                //        {
                                //            NextYearStock = Convert.ToDouble(objstockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvr.FindControl("hdnLocationId")).Value, HttpContext.Current.Session["NextFinancialYearId"].ToString(), dr["ProductId"].ToString(), ref trns).Rows[0]["Quantity"].ToString());
                                //        }
                                //        catch
                                //        {

                                //        }

                                //        try
                                //        {
                                //            CurrentStock = Convert.ToDouble(dr["Quantity"].ToString());
                                //        }
                                //        catch
                                //        {

                                //        }

                                //        //if ((0 - NextFianceOpeningStock + CurrentStock) != 0)
                                //        //{

                                //        objDA.execute_Command("delete from Inv_ProductLedger where TransType='OP' and Location_Id=" + ((HiddenField)gvr.FindControl("hdnLocationId")).Value + " and Finance_Year_Id=" + HttpContext.Current.Session["NextFinancialYearId"].ToString() + " and ProductId=" + dr["ProductId"].ToString() + "", ref trns);

                                //        string strsql = "select ISNULL( SUM(quantityin*unitprice)/SUM(quantityin),0) from Inv_ProductLedger where ProductId=" + dr["ProductId"].ToString() + " and location_id=" + ((HiddenField)gvr.FindControl("hdnLocationId")).Value + " and Finance_Year_Id=" + HttpContext.Current.Session["NextFinancialYearId"].ToString() + "";
                                //        double averageCost = 0;
                                //        try
                                //        {
                                //            averageCost = Convert.ToDouble(objDA.return_DataTable(strsql, ref trns).Rows[0][0].ToString());
                                //        }
                                //        catch
                                //        {
                                //            averageCost = 0;
                                //        }
                                //        objDA.execute_Command("update Inv_StockDetail set Quantity=" + (NextYearStock - NextFianceOpeningStock).ToString() + ",Field2='" + averageCost.ToString() + "' where Location_Id=" + ((HiddenField)gvr.FindControl("hdnLocationId")).Value + " and Finance_Year_Id=" + HttpContext.Current.Session["NextFinancialYearId"].ToString() + " and ProductId=" + dr["ProductId"].ToString() + "", ref trns);


                                //        //objDA.execute_Command("update Inv_StockDetail set Quantity=" + (NextYearStock - NextFianceOpeningStock).ToString() + " where Location_Id=" + ((HiddenField)gvr.FindControl("hdnLocationId")).Value + " and Finance_Year_Id=" + HttpContext.Current.Session["NextFinancialYearId"].ToString() + " and ProductId=" + dr["ProductId"].ToString() + "", ref trns);
                                //        ObjProductLadger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvr.FindControl("hdnLocationId")).Value, "OP", "0", "0", dr["ProductId"].ToString(), dr["Unit_Id"].ToString(), "I", "0", "0", (CurrentStock).ToString(), "0", "1/1/1800", dr["UnitCost"].ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["NextFinancialYearId"].ToString(), ref trns);

                                //        objDA.execute_Command("update Inv_StockDetail set OpeningBalance=" + (CurrentStock).ToString() + " where Location_Id=" + ((HiddenField)gvr.FindControl("hdnLocationId")).Value + " and Finance_Year_Id=" + HttpContext.Current.Session["NextFinancialYearId"].ToString() + " and ProductId=" + dr["ProductId"].ToString() + "", ref trns);
                                //        //}
                                //    }
                                //}

                            }

                        }
                        DisplayMessage("Closed Financial Year Successfully");
                        GvClosingLocation.DataSource = null;
                        GvClosingLocation.DataBind();

                    }


                    //here we update open status for next financial year in case of open
                    if (strFinancialYearStatus.Trim().ToUpper() == "OPEN")
                    {

                        strSQL = "Update Ac_Finance_Year_Info set Status='Open',ModifiedBy='" + Session["UserId"].ToString() + "', ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id='" + Session["NextFinancialYearId"].ToString() + "'";
                        objDA.execute_Command(strSQL, ref trns);
                        strSQL = "update Ac_FinancialYear_Detail set Status='Open',Inv_Status='Open',ModifiedBy='" + Session["UserId"].ToString() + "', ModifiedDate='" + DateTime.Now.ToString() + "' where header_trans_id='" + Session["NextFinancialYearId"].ToString() + "'";
                        objDA.execute_Command(strSQL, ref trns);

                    }



                }
                else
                {
                    DisplayMessage("Need to Close Locations Financial Year");

                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }

                    trns.Dispose();
                    con.Dispose();
                    strStatus = "";
                    return;
                }


                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }

                trns.Dispose();
                con.Dispose();
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
        else
        {
            DisplayMessage("You have no data to close");
            return;
        }

    }
    protected string GetLocationName(string strLocationId)
    {
        string strLocationName = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocName = objLocationMaster.GetLocationMasterById(StrCompId, strLocationId);
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
    #endregion





    #region Report

    protected void btnReport_Click(object sender, EventArgs e)
    {
        string strCmd = string.Format("window.open('../Accounts_Report/Financial_Year_Summary.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }


    #endregion
}
