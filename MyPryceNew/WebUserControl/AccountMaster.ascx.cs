using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.IO;
using System.Data.SqlClient;

public partial class WebUserControl_AccountMaster : System.Web.UI.UserControl
{
    Ac_AccountMaster objAccountMaster = null;
    CurrencyMaster objCurrency = null;
    Common objCommon = null;
    Set_DocNumber objDocNo = null;
    DataAccessClass objDa = null;
    Set_CustomerMaster_CreditParameter objCustomerCreditParam = null;
    SystemParameter ObjSysParam = null;
    Set_Approval_Employee objEmpApproval = null;
    NotificationMaster Obj_Notifiacation = null;
    EmployeeMaster objEmployee = null;
    Inv_ParameterMaster objInvParam = null;
    Check_Page_Permission ojbCheckPermission = null;
    LocationMaster objLocationMaster = null;
    Ac_Finance_Year_Info objFYI = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objCommon = new Common(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ojbCheckPermission = new Check_Page_Permission(Session["DBConnection"].ToString());
        objLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            fillDdlCurrecy();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_AccountMasterList('pnlListGrid')", true);
            //pnlCreditSetup.Visible = false;
            //pnlListGrid.Visible = true;

        }
    }
    protected void fillDdlCurrecy()
    {
        DataTable _dtCurrency = objCurrency.GetCurrencyMaster();
        objPageCmn.FillData((object)ddlCurrency, _dtCurrency, "Currency_Name", "Currency_Id");
    }

    protected void btnDeleteAcMaster_Command(object sender, CommandEventArgs e)
    {
        string strAccountId = e.CommandArgument.ToString();
        string strParentAccountNo = "0";
        if (hdnRefType.Value == "Customer")
        {
            strParentAccountNo = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        }
        else if (hdnRefType.Value == "Supplier")
        {
            strParentAccountNo = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        }
        using (DataTable dtLoc = objLocationMaster.GetLocationMaster(Session["CompId"].ToString()))
        {
            foreach (DataRow dr in dtLoc.Rows)
            {
                string sql = "select(dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "', '" + dr["Location_id"].ToString() + "', '" + DateTime.Now.ToString() + "', '0', '" + strParentAccountNo.ToString() + "', '" + strAccountId.ToString() + "', '3', '" + Session["FinanceYearId"].ToString() + "')) OpeningBalance";
                if (objDa.get_SingleValue(sql) != "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Sorry! you can not delete because this account have balance on " + dr["Location_name"].ToString() + "(Location)')", true);
                    return;
                }
            }
        }

        int i = objAccountMaster.DeleteAc_AccountMaster(e.CommandArgument.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (i != 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record deleted successfully')", true);
            fillListGrid();
            FillGridBin();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record not deleted')", true);
        }

    }

    protected void fillListGrid()
    {
        reset();
        if (hdnRefId.Value == string.Empty || hdnRefType.Value == string.Empty)
        {
            GvAcMasterList.DataSource = null;
            GvAcMasterList.DataBind();
            return;
        }

        DataTable _dtAcMaster = new DataTable();
        if (hdnRefType.Value == "Customer")
        {
            _dtAcMaster = objAccountMaster.GetAc_AccountMasterByCustomerId(hdnRefId.Value, Session["FinanceYearId"].ToString(), Session["LocId"].ToString());
        }
        else if (hdnRefType.Value == "Supplier")
        {
            _dtAcMaster = objAccountMaster.GetAc_AccountMasterBySupplierId(hdnRefId.Value, Session["FinanceYearId"].ToString(), Session["LocId"].ToString());
        }
        objPageCmn.FillData((object)GvAcMasterList, _dtAcMaster, "", "");
        if (GvAcMasterList.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in GvAcMasterList.Rows)
            {
                string strObType = ((HiddenField)gvr.FindControl("hdnObType")).Value;
                string strCurrencyId = ((HiddenField)gvr.FindControl("hdnCurrencyId")).Value;
                DropDownList ddl = (DropDownList)gvr.FindControl("ddlObType");
                if (!string.IsNullOrEmpty(strObType))
                {
                    ddl.SelectedValue = strObType;
                }
                TextBox txtOb = (TextBox)gvr.FindControl("txtOb");
                if (!string.IsNullOrEmpty(txtOb.Text))
                {
                    txtOb.Text= ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), txtOb.Text);
                }

                TextBox txtExchangeRate = (TextBox)gvr.FindControl("txtExchangeRate");
                double exchangeRate = 0;
                double.TryParse(txtExchangeRate.Text, out exchangeRate);
                txtExchangeRate.Text = exchangeRate.ToString("0.000000");
                LinkButton lnkUpdate = (LinkButton)gvr.FindControl("btnUpdateOb");
                if (!objFYI.isObUpdateAllow(Session["CompId"].ToString()))
                {
                    lnkUpdate.Enabled = ddl.Enabled = txtExchangeRate.Enabled = txtOb.Enabled = false;
                }
            }
        }
        AllPageCode();
        _dtAcMaster.Dispose();
    }
    public void setUcAcMasterValues(string strRefId, string strRefType, string strName)
    {
        lblHeaderR.Text = strName;
        hdnRefType.Value = strRefType;
        hdnRefId.Value = strRefId;
        fillListGrid();
        FillGridBin();
    }

    protected void btnSaveAcMaster_Command(object sender, CommandEventArgs e)
    {
        if (txtAccountNo.Text == string.Empty || txtAccountNo.Text == "0")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Please set Document No system, Before going with save')", true);
            return;
        }
        if (ddlCurrency.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Please select currency')", true);
            return;
        }

        bool isAccountExist = false;
        if (hdnRefType.Value == "Customer")
        {
            isAccountExist = objAccountMaster.isCustomerAccountExist(hdnRefId.Value, ddlCurrency.SelectedValue);
        }
        else if (hdnRefType.Value == "Supplier")
        {
            isAccountExist = objAccountMaster.isSupplierAccountExist(hdnRefId.Value, ddlCurrency.SelectedValue);
        }
        if (isAccountExist == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('" + ddlCurrency.SelectedItem.Text + " Account already exist for " + lblHeaderR.Text + "')", true);
            return;
        }
        int i = objAccountMaster.InsertAc_AccountMaster("0", hdnRefType.Value, hdnRefId.Value, txtAccountNo.Text, ddlCurrency.SelectedValue.ToString(), "", "", "", false.ToString(), "01-Jan-1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (i > 0)
        {
            string sql = "select count(*) from ac_accountMaster";
            string strRecCount = objDa.get_SingleValue(sql);
            txtAccountNo.Text = strRecCount == "0" ? txtAccountNo.Text + "1" : txtAccountNo.Text + strRecCount;
            objCommon.UpdateCodeForDocumentNo("ac_accountMaster", "Account_no", "Trans_Id", i.ToString(), txtAccountNo.Text);
            fillListGrid();
            reset();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record Successfully saved')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record Not Saved')", true);
        }
    }
    protected void FillGridBin()
    {
        if (hdnRefId.Value == string.Empty || hdnRefType.Value == string.Empty)
        {
            GvAcMasterBin.DataSource = null;
            GvAcMasterBin.DataBind();
            return;
        }

        DataTable _dtAcMaster = new DataTable();
        if (hdnRefType.Value == "Customer")
        {
            _dtAcMaster = objAccountMaster.GetAc_AccountMasterByCustomerIdInActive(hdnRefId.Value);
        }
        else if (hdnRefType.Value == "Supplier")
        {
            _dtAcMaster = objAccountMaster.GetAc_AccountMasterBySupplierIdInActive(hdnRefId.Value);
        }
        objPageCmn.FillData((object)GvAcMasterBin, _dtAcMaster, "", "");
        _dtAcMaster.Dispose();
    }
    protected void btnResetAcMaster_Command(object sender, CommandEventArgs e)
    {
        reset();
    }
    protected void reset()
    {
        txtAccountNo.Text = "";
        ddlCurrency.SelectedIndex = 0;
        try
        {
            if (ViewState["Uc_acMasterDocNo"] == null)
            {
                ViewState["Uc_acMasterDocNo"] = objDocNo.GetDocumentNo(true, "0", false, "150", hdnRefType.Value == "Customer" ? "399" : "400", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
            }
            txtAccountNo.Text = (string)ViewState["Uc_acMasterDocNo"];

        }
        catch { }

    }
    protected void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());
        if (Session["EmpId"].ToString() == "0")
        {
            GvAcMasterList.Columns[0].Visible = true; //delete button
            GvAcMasterList.Columns[1].Visible = true; //credit setup button
            btnSaveAcMaster.Enabled = true;
            GvAcMasterBin.Columns[0].Visible = true;
            return;
        }

        string strModuleId = "0";
        string strModuleName = string.Empty;
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("175", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        DataTable dtAllPageCode = null;
        Common.clsApplicationModules _cls = (Common.clsApplicationModules)Session["clsApplicationModule"];
        if (_cls.isFinanceModule)
        {
            dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "175", HttpContext.Current.Session["CompId"].ToString());
        }
        else
        {
            dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), "107", "20", HttpContext.Current.Session["CompId"].ToString());
        }
        if (dtAllPageCode.Rows.Count == 0)
        {
            GvAcMasterList.Columns[0].Visible = false;
            GvAcMasterList.Columns[1].Visible = false;
            btnSaveAcMaster.Enabled = false;
            GvAcMasterBin.Columns[0].Visible = false;
        }
        else
        {
            foreach (DataRow DtRow in dtAllPageCode.Rows)
            {
                if (DtRow["Op_Id"].ToString() == "3")
                {
                    GvAcMasterList.Columns[0].Visible = true; //delete button
                    GvAcMasterList.Columns[1].Visible = true; //credit setup button
                }
                if (DtRow["Op_Id"].ToString() == "1")
                {
                    btnSaveAcMaster.Enabled = true;
                }
                if (DtRow["Op_Id"].ToString() == "4") //restore
                {
                    GvAcMasterBin.Columns[0].Visible = true;
                }
            }
        }

        btnSaveAcMaster.Enabled = true;
    }

    protected void btnRestore_Command(object sender, CommandEventArgs e)
    {
        try
        {
            string strBinTransId = e.CommandArgument.ToString();
            int b = objAccountMaster.RestoreAc_AccountMaster(strBinTransId, Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Restore record successfully')", true);
                fillListGrid();
                FillGridBin();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Restore operation failed')", true);
            }

        }
        catch
        {

        }
    }

    protected void btnuploadFiancialstatement_Click(object sender, EventArgs e)
    {

        lnkDownloadFiancialstatement.Text = FileUploadFinancilaStatement.FileName;
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Credit_Approval_Div()", true);
    }
    protected void btnDownloadFiancialstatement_Click(object sender, EventArgs e)
    {

        if (Session["StatementPath"] == null)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Statement not found')", true);
            return;
        }
        try
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../CompanyResource/" + Session["CompId"].ToString() + "/" + Session["StatementPath"].ToString() + "')", true);
        }
        catch { }

        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Credit_Approval_Div()", true);
    }
    protected void btnCreditSetup_Command(object sender, CommandEventArgs e)
    {
        try
        {
            //args 0-trans_id,1-account_no,2-currency_name,3-currency_id
            string[] strArgs = e.CommandArgument.ToString().Split(',');
            lblCreditSetupTitle.Text = "Credit setup for - " + strArgs[1].ToString() + "(" + strArgs[2].ToString() + ")";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_AccountMasterList('pnlCreditSetup')", true);
            //pnlCreditSetup.Visible = true;
            //pnlListGrid.Visible = false;
            resetCreditSetupControl();
            hdn_ucAcMaster_OtherAccountId.Value = strArgs[0].ToString();
            hdn_ucAcMaster_CurrencyId.Value = strArgs[3].ToString();
            GetCreditInfo(strArgs[0].ToString(), strArgs[3].ToString());
        }
        catch { }
    }

    protected void btnSaveCreditDetail_Command(object sender, CommandEventArgs e)
    {
        if (UpdateCustomerCredit(hdnRefId.Value, hdn_ucAcMaster_OtherAccountId.Value, hdn_ucAcMaster_CurrencyId.Value) == true)
        {
            fillListGrid();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_AccountMasterList('pnlListGrid')", true);
            //pnlCreditSetup.Visible = false;
            //pnlListGrid.Visible = true;
        }
    }


    public void resetCreditSetupControl()
    {
        txtCreditLimit.Text = "";
        txtCreditDays.Text = "";
        rbtnNone.Checked = true;
        hdn_ucAcMaster_OtherAccountId.Value = "0";
        hdn_ucAcMaster_CurrencyId.Value = "0";
        Session["StatementPath"] = null;
        lnkDownloadFiancialstatement.Text = "Download";
    }
    public void GetCreditInfo(string strOtherAccountId, string strCurrencyId)
    {
        try
        {
            DataTable dtCreditParameter = new DataTable();
            if (hdnRefType.Value == "Customer")
            {
                dtCreditParameter = objCustomerCreditParam.GetCustomerRecord_By_OtherAccountId(strOtherAccountId);
            }
            else
            {
                dtCreditParameter = objCustomerCreditParam.GetSupplierRecord_By_OtherAccountId(strOtherAccountId);
            }

            if (dtCreditParameter.Rows.Count > 0)
            {
                txtCreditLimit.Text = SystemParameter.GetAmountWithDecimal(dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim(), Session["LoginLocDecimalCount"].ToString());
                txtCreditDays.Text = dtCreditParameter.Rows[0]["Credit_Days"].ToString().Trim();
                if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()))
                {
                    rbtnAdvanceCheque.Checked = true;
                }
                else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()))
                {
                    rbtnInvoicetoInvoice.Checked = true;
                }
                else if (Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim()))
                {
                    rbtnAdvanceHalfpayment.Checked = true;
                }
                else
                {
                    rbtnNone.Checked = true;
                }


                if (dtCreditParameter.Rows[0]["Financial_Statement_Name"].ToString().Trim() != "")
                {
                    Session["StatementPath"] = dtCreditParameter.Rows[0]["Financial_Statement_Name"].ToString().Trim();
                    lnkDownloadFiancialstatement.Text = dtCreditParameter.Rows[0]["Financial_Statement_Name"].ToString().Trim();
                }
                else
                {
                    Session["StatementPath"] = null;
                    lnkDownloadFiancialstatement.Text = "Download";
                }
            }
        }
        catch { }
    }

    public bool UpdateCustomerCredit(string strCustomerId, string strOtherAccountId, string strCurrencyId)
    {
        try
        {
            double creditLimit = 0;
            double.TryParse(txtCreditLimit.Text, out creditLimit);
            int creditDays = 0;
            int.TryParse(txtCreditDays.Text, out creditDays);
            creditDays = creditLimit == 0 ? 0 : creditDays;
            string strApprovalType = hdnRefType.Value == "Customer" ? "14" : "16";
            if (creditLimit > 0 && creditDays == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Credit Days should be there')", true);
                return false;
            }
            int CreditLimitThreshold = 0;

            using (DataTable _dt = objCurrency.GetCurrencyMasterById(strCurrencyId))
            {
                if (_dt.Rows.Count > 0)
                {
                    int.TryParse(_dt.Rows[0]["field4"].ToString(), out CreditLimitThreshold);
                }
            }


            if (hdnRefType.Value == "Customer" && CreditLimitThreshold > 0 && creditLimit > CreditLimitThreshold && Session["StatementPath"] == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Credit limit is greater than " + CreditLimitThreshold.ToString() + " so financial statement is required, Please upload it.')", true);
                return false;
            }

            DataTable dtApproval = new DataTable();
            string EmpPermission = string.Empty;

            if (hdnRefType.Value == "Customer")
            {
                EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name(hdnRefType.Value == "Customer" ? "Customer Credit" : "Supplier Credit").Rows[0]["Approval_Level"].ToString();
                if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CreditInvoiceApproval").Rows[0]["ParameterValue"]) == true)
                {
                    dtApproval = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), hdnRefType.Value == "Customer" ? "18" : "20", Session["EmpId"].ToString());
                    if (dtApproval.Rows.Count == 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Approval setup issue , please contact to your admin')", true);
                        return false;
                    }
                }
            }

            string Creditmethod = string.Empty;
            if (rbtnAdvanceCheque.Checked)
            {
                Creditmethod = "Advance Cheque basis";
            }
            else if (rbtnInvoicetoInvoice.Checked)
            {
                Creditmethod = "Invoice to invoice credit";
            }
            else if (rbtnAdvanceHalfpayment.Checked)
            {
                Creditmethod = "50% advance and 50% on delivery";
            }
            else
            {
                Creditmethod = "only credit limit and credit days basis";
            }


            string strLocalCreditLimit = txtCreditLimit.Text;
            double oldCreditLimit = 0;
            int oldCreditDays = 0;
            string strOldCreditMethod = string.Empty;
            DataTable dtParam = new DataTable();
            if (hdnRefType.Value == "Customer")
            {
                dtParam = objCustomerCreditParam.GetCustomerRecord_By_OtherAccountId(strOtherAccountId);
            }
            else
            {
                dtParam = objCustomerCreditParam.GetSupplierRecord_By_OtherAccountId(strOtherAccountId);
            }

            if (dtParam.Rows.Count > 0)
            {
                double.TryParse(dtParam.Rows[0]["Credit_Limit"].ToString(), out oldCreditLimit);
                int.TryParse(dtParam.Rows[0]["Credit_Days"].ToString().Trim(), out oldCreditDays);
                if (Convert.ToBoolean(dtParam.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim()))
                {
                    strOldCreditMethod = "Advance Cheque basis";
                }
                else if (Convert.ToBoolean(dtParam.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim()))
                {
                    strOldCreditMethod = "Invoice to invoice credit";
                }
                else if (Convert.ToBoolean(dtParam.Rows[0]["Is_Half_Advance"].ToString().Trim()))
                {
                    strOldCreditMethod = "50% advance and 50% on delivery";
                }
                else
                {
                    strOldCreditMethod = "only credit limit and credit days basis";
                }
            }
            if (creditDays == oldCreditDays && creditLimit == oldCreditLimit && Creditmethod == strOldCreditMethod)
            {
                return true;
            }

            //DataTable dtEmp = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString());
            DataTable dtEmp = new DataTable();
            if (hdnRefType.Value == "Customer")
            {
                dtEmp = objDa.return_DataTable("select * from Set_Approval_Transaction where Approval_Id = '" + strApprovalType + "' and Ref_Id = '" + strOtherAccountId + "' and isActive='true'");
            }
            SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
            SqlTransaction trns;
            con.Open();
            trns = con.BeginTransaction();
            try
            {
                if (dtParam.Rows.Count > 0)
                {
                    objCustomerCreditParam.DeleteRecord_By_CustomerId(strOtherAccountId, hdnRefType.Value == "Customer" ? "C" : "S");
                }
                //dtEmp = new DataView(dtEmp, "Approval_Id='14' and Ref_Id='" + strOtherAccountId + "'", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < dtEmp.Rows.Count; i++)
                {
                    objDa.execute_Command("update Set_Approval_Transaction set isActive='False',ModifiedDate='" + DateTime.Now.ToString() + "'  where Trans_Id=" + dtEmp.Rows[i]["Trans_Id"].ToString() + "", ref trns);
                }
                dtEmp.Dispose();

                if (dtApproval.Rows.Count > 0)
                {
                    for (int j = 0; j < dtApproval.Rows.Count; j++)
                    {
                        string PriorityEmpId = dtApproval.Rows[j]["Emp_Id"].ToString();
                        string IsPriority = dtApproval.Rows[j]["Priority"].ToString();
                        int cur_trans_id = 0;
                        if (EmpPermission == "1")
                        {
                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton(strApprovalType, Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), strOtherAccountId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                        }
                        else if (EmpPermission == "2")
                        {
                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton(strApprovalType, Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), strOtherAccountId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                        }
                        else if (EmpPermission == "3")
                        {
                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton(strApprovalType, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strOtherAccountId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton(strApprovalType, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strOtherAccountId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                        }

                        Session["PriorityEmpId"] = PriorityEmpId;
                        Session["cur_trans_id"] = cur_trans_id;
                        Session["Ref_ID"] = strOtherAccountId.ToString();
                        //Set_Notification();
                    }
                }
                if (Session["StatementPath"] == null)
                {
                    Session["StatementPath"] = "";
                }
                string strApprovalStatus = dtApproval.Rows.Count > 0 && hdnRefType.Value == "Customer" ? "Pending" : "Approved";
                objCustomerCreditParam.InsertRecord(strCustomerId, creditLimit.ToString(), strCurrencyId, creditDays.ToString(), rbtnAdvanceCheque.Checked.ToString(), rbtnInvoicetoInvoice.Checked.ToString(), rbtnAdvanceHalfpayment.Checked.ToString(), Session["StatementPath"].ToString(), creditLimit.ToString(), hdnRefType.Value == "Customer" ? "C" : "S", strOtherAccountId, strApprovalStatus, "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                trns.Commit();
                con.Close();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record updated successfully')", true);
                return true;
            }
            catch (Exception ex)
            {
                trns.Rollback();
                con.Close();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('" + ex.Message + "')", true);
                return false;
            }
        }
        catch (Exception ex)
        {

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('" + ex.Message + "')", true);
            return false;
        }


    }
    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/mastersetup"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);
        //string sql = "SELECT Trans_ID FROM set_notification_type WHERE Type = 'Customer Credit' AND IsActive = 'True'";
        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = GetEmployeeCode(Session["UserId"].ToString()) + " request for Customer Credit for " + lblHeaderR.Text + ". on " + System.DateTime.Now.ToString();
        //if (Hdn_Edit_ID.Value == "")
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["Ref_ID"].ToString(), "1");
        //else
        //    Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Hdn_Edit_ID.Value, "15");
    }
    protected string GetEmployeeCode(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
                ViewState["Emp_Img"] = "../CompanyResource/2/" + dtEName.Rows[0]["Emp_Image"].ToString();
            }
            else
            {
                ViewState["Emp_Img"] = "";
            }
        }
        else
        {
            strEmployeeName = "";
        }
        return strEmployeeName;
    }
    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FileUploadFinancilaStatement.HasFile)
        {
            if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"]))
            {
                Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"]);
            }
            string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/") + FileUploadFinancilaStatement.FileName;
            FileUploadFinancilaStatement.SaveAs(path);
            Session["StatementPath"] = FileUploadFinancilaStatement.FileName;
            // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Credit_Approval_Div()", true);
        }
    }

    protected void lnkCurrentBalance_Command(object sender, CommandEventArgs e)
    {
        if (Session["EmpId"].ToString() != "0")
        {
            if (ojbCheckPermission.CheckPagePermission(Session["UserId"].ToString(), hdnRefType.Value == "Customer" ? "300" : "308", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('You do not have permission to view statement')", true);
                return;
            }
        }
        if (hdnRefType.Value == "Customer")
        {
            Session["CusID"] = e.CommandArgument.ToString();
            Session["CusterStatementFromDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            Session["CustomerStatementToDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            Session["CustomerStatementLocations"] = Session["LocId"].ToString();
        }
        else
        {
            Session["SupID"] = e.CommandArgument.ToString();
            Session["From"] = DateTime.Now.ToString("dd-MMM-yyyy");
            Session["To"] = DateTime.Now.ToString("dd-MMM-yyyy");
            Session["SupLocId"] = Session["LocId"].ToString();
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "OpentStatementWindow('" + hdnRefType.Value.ToString() + "','" + e.CommandArgument.ToString() + "')", true);
    }
    protected string getCurrentBalance(string strOtherAccountId)
    {
        string _strBalance = "0";
        string _strAccountId = "0";
        _strAccountId = hdnRefType.Value == "Customer" ? Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) : Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        string sql = "select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "', '" + DateTime.Now.Date.ToString("dd-MMM-yyyy") + "', '0','" + _strAccountId + "','" + strOtherAccountId + "','3','" + Session["FinanceYearId"].ToString() + "')) OpeningBalance";
        _strBalance = SystemParameter.GetAmountWithDecimal(objDa.get_SingleValue(sql), Session["LoginLocDecimalCount"].ToString());
        return _strBalance;
    }



    protected void btnUpdateOb_Command(object sender, CommandEventArgs e)
    {
        try
        {
            if (!objFYI.isObUpdateAllow(Session["CompId"].ToString()))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('system does allow to update opening balance bcz of closing year exist", true);
                return;
            }

            string otherAccountId = e.CommandArgument.ToString();
            string strParentAccountNo = "0";
            if (hdnRefType.Value == "Customer")
            {
                strParentAccountNo = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            }
            else if (hdnRefType.Value == "Supplier")
            {
                strParentAccountNo = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            }

            GridViewRow gvRow = (GridViewRow)((LinkButton)sender).NamingContainer;

            string strOb = ((TextBox)gvRow.FindControl("txtOb")).Text;
            double ob = 0;
            double.TryParse(strOb, out ob);

            if (ob == 0)
            {
                return;
            }

            string strExchangeRate = ((TextBox)gvRow.FindControl("txtExchangeRate")).Text;
            double exchangeRate = 0;
            double.TryParse(strExchangeRate, out exchangeRate);
            exchangeRate = exchangeRate == 0 ? 1 : exchangeRate; //set default value as 1

            string strCurrencyId = ((HiddenField)gvRow.FindControl("hdnCurrencyId")).Value;

            string strBalType = ((DropDownList)gvRow.FindControl("ddlObType")).SelectedValue;
            double fDrAmt = strBalType.ToUpper() == "DR" ? ob : 0;
            double fCrAmt = strBalType.ToUpper() == "CR" ? ob : 0;
            double lDrAmt = fDrAmt * exchangeRate;
            double lCrAmt = fCrAmt * exchangeRate;

            Ac_SubChartOfAccount objSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
            SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();
            try
            {
                //delete existing data
                objSubCOA.deleteOtherAcIdAndFyId(Session["CompId"].ToString(), Session["LocId"].ToString(), otherAccountId.ToString(), Session["FinanceYearId"].ToString(), ref trns);
                int b = objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strParentAccountNo, otherAccountId.ToString(), lDrAmt.ToString(), lCrAmt.ToString(), fDrAmt.ToString(), fCrAmt.ToString(), strCurrencyId, "0", "0", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                trns.Commit();
                con.Close();
                fillListGrid();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record has been updated", true);
            }
            catch (Exception)
            {
                trns.Rollback();
                con.Close();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Sorry! Unable to update reocords ", true);
                return;
            }
        }
        catch (Exception ex)
        {

        }

    }
}