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
using System.Threading;

public partial class CustomerReceivable_EmailLogOfStatement : System.Web.UI.Page
{
    DataAccessClass da = null;
    Common cmn = null;
    LocationMaster ObjLocation = null;
    EmployeeMaster objEmployee = null;
    Set_CustomerMaster ObjCoustmer = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_EmailLog objEmailLog = null;
    SystemParameter objsys = null;
    CustomerAgeingEstatement objCustomerAgeingStatement = null;
    CompanyMaster ObjCompany = null;
    Ac_Parameter_Location objAcParamLocation = null;
    Ac_EmailLog objAcEmailLog = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    string strLocationCurrencyId = string.Empty;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        da = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objEmailLog = new Ac_EmailLog(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        objCustomerAgeingStatement = new CustomerAgeingEstatement(Session["DBConnection"].ToString());
        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        objAcParamLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objAcEmailLog = new Ac_EmailLog(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../CustomerReceivable/EmailLogOfStatement.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            StrUserId = Session["UserId"].ToString();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            btnList_Click(sender, e);
            FillGrid(true);
          
        }

        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        getAgeingStatement();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        GvList.Columns[1].Visible = clsPagePermission.bDelete;
        //hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
   
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlList.Visible = true;
        PnlBin.Visible = false;
    }
   
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
        
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (txtValue.Text==string.Empty)
        {
            return;
        }
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

            DataTable dtVoucher = (DataTable)Session["dtCustomerEmailLog"];
            dtVoucher = new DataView(dtVoucher, "Status='" + ddlStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            DataView view = new DataView(dtVoucher, condition, "", DataViewRowState.CurrentRows);
            
            objPageCmn.FillData((object)GvList, view.ToTable(), "", "");
            Session["dtFilter_dtCustomerEmailLog"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid(true);
        //FillGridBin();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
       
    }
   
  
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string _tranId = e.CommandArgument.ToString();
        try
        {
            if (objEmailLog.UpdateIsActiveAcEmailLog(strLocationId,_tranId,false.ToString(),Session["UserId"].ToString(),DateTime.Now.ToString())!=0)
            {
                FillGrid(true);
                //AllPageCode();
                DisplayMessage("Record has been deleted");
            }
        }
        catch
        {
            DisplayMessage("Record Not deleted");
        }

      
    }
protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(objsys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    private void FillGrid(bool GetUpdateRecord=false)
    {
        DataTable _dt = new DataTable();
        if (GetUpdateRecord == true)
        {
            _dt = objEmailLog.getCustomerStatementEmailLog(strLocationId, "RV");
            Session["dtCustomerEmailLog"] = _dt;
        }
        else
        {
            if (Session["dtCustomerEmailLog"] == null)
            {
                _dt = objEmailLog.getCustomerStatementEmailLog(strLocationId, "RV");
                Session["dtCustomerEmailLog"] = _dt;
            }
            else
            {
                _dt = (DataTable)Session["dtCustomerEmailLog"];
            }
        }
        _dt = new DataView(_dt, "Status='" + ddlStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (_dt != null && _dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)GvList, _dt, "", "");
        }
        else
        {
            GvList.DataSource = null;
            GvList.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + _dt.Rows.Count.ToString() + "";
        //AllPageCode();
        reset();
        if (ddlStatus.SelectedValue=="Pending")
        {
            btnsendMail.Enabled = true;
        }
        else
        {
            btnsendMail.Enabled = false;
        }
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
    protected void GvVoucherBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvVoucherBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCustomerEmailLogBin"];
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
        
        //AllPageCode();
    }
    protected void GvVoucherBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtCustomerEmailLogBin"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtCustomerEmailLogBin"] = dt;
        
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");

        lblSelectedRecord.Text = "";
        //AllPageCode();
    }
    public void FillGridBin()
        {
        DataTable dt = new DataTable();
        dt = objEmailLog.getInActiveCustomerStatementEmailLog(strLocationId, "RV");
        //dt = new DataView(dt, "Voucher_Type='PV'", "", DataViewRowState.CurrentRows).ToTable();

        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");
        Session["dtCustomerEmailLogBin"] = dt;
        //Session["dtCustomerEmailLogBin"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";

        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
        }
        else
        {
            imgBtnRestore.Visible = true;
        }

       
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (txtValueBin.Text==string.Empty)
        {
            return;
        }
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


            DataTable dtCust = (DataTable)Session["dtCustomerEmailLogBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            //Session["dtAfterInvoiceTaxDetailInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucherBin, view.ToTable(), "", "");
            
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
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
                    if (lblSelectedRecord.Text.Split(',')[j].Trim() != "" && lblSelectedRecord.Text.Split(',')[j].Trim() != "0")
                    {
                        string _transId= lblSelectedRecord.Text.Split(',')[j].Trim();
                       // b=objAfterInvoiceTaxDetail.restoreRecord(_transId);

                        b= objAcEmailLog.UpdateIsActiveAcEmailLog(strLocationId, _transId, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        
                       
                    }
                }
            }
        }

        if (b != 0)
        {
            FillGrid(true);
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
                CheckBox chk = (CheckBox)Gvr.FindControl("chkCurrentBin");
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

    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = (CheckBox)GvVoucherBin.HeaderRow.FindControl("chkSelectAllBin");
        for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
        {
            ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkCurrentBin")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvVoucherBin.Rows[i].FindControl("lblBinTransID"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvVoucherBin.Rows[i].FindControl("lblBinTransID"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvVoucherBin.Rows[i].FindControl("lblBinTransID"))).Text.Trim().ToString())
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
    
    protected void ddlRecType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid(false);
    }

  
    
    protected void GvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvList.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCustomerEmailLog"];
        objPageCmn.FillData((object)GvList, dt, "", "");
        //AllPageCode();
    }

    protected void GvList_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtCustomerEmailLog"];
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
        Session["dtCustomerEmailLog"] = dt;
        objPageCmn.FillData((object)GvList, dt, "", "");
        //AllPageCode();
    }

    
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkHeader = (CheckBox)GvList.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gvrow in GvList.Rows)
        {
            if (ChkHeader.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }

        }
    }

    protected void lnkInvoiceStatement_Click(object sender, EventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;
        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { '|' });
        HDFCustomerID.Value = arguments[0];
        HDFStatementDate.Value= arguments[1];
        HDFLocations.Value = arguments[2];
        HDFOtherAccountId.Value = arguments[3];
        ViewState["CustomerAddressForStatement"] = null;
        ViewState["EmailFooterForStatement"] = null;
        ViewState["DtRvInvoiceStatement"] = null;
        getAgeingStatement();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Invoice_Statement_Popup()", true);
    }
    protected void getAgeingStatement()
    {

        if (HDFCustomerID.Value == "0" || HDFStatementDate.Value=="0" || HDFLocations.Value=="0")
        {
            return;
        }

        DataTable dt = new DataTable();
        if (ViewState["DtRvInvoiceStatement"] == null)
        {
            AccountsDataset ObjAccountDataset = new AccountsDataset();
            ObjAccountDataset.EnforceConstraints = false;
            AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter adp = new AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement, Session["CompId"].ToString(), Session["BrandId"].ToString(), HDFLocations.Value, "RV", HDFOtherAccountId.Value, (Convert.ToDateTime(HDFStatementDate.Value)).Date);
            ViewState["DtRvInvoiceStatement"] = ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement;
        }
        
            dt = (DataTable)ViewState["DtRvInvoiceStatement"];

        Ac_Ageing_Detail.clsAgeingDaysDetail clsAging = new Ac_Ageing_Detail.clsAgeingDaysDetail();
        if (dt.Rows.Count>0)
        {
            int CurrencydecimalCount = 0;
            int.TryParse(ObjCurrencyMaster.GetCurrencyMasterById(dt.Rows[0]["Currency_Id"].ToString()).Rows[0]["decimal_count"].ToString(), out CurrencydecimalCount);

            //set ageing days detail in footer of the print
            clsAging = ObjAgeingDetail.getAgingDayDetail(dt, CurrencydecimalCount == 0 ? 2 : CurrencydecimalCount);
        }
        string strCompanyName = string.Empty;
        string strCompanyLogoUrl = Session["CompanyLogoUrl"].ToString();
        strCompanyName = Session["CompName"].ToString();
        objCustomerAgeingStatement.DataSource = dt;
        objCustomerAgeingStatement.DataMember = "sp_Ac_InvoiceAgeingStatement";
        objCustomerAgeingStatement.setcompanyname(strCompanyName);
        objCustomerAgeingStatement.SetImage(strCompanyLogoUrl);
        objCustomerAgeingStatement.setStatementDate(HDFStatementDate.Value);
        objCustomerAgeingStatement.setAgeingDaysDetail(clsAging);
        string strEmailFooter = string.Empty;
        string strCustomerAddress = string.Empty;
        try
        {
            if (ViewState["CustomerAddressForStatement"] == null)
            {
                ViewState["CustomerAddressForStatement"] = da.get_SingleValue("SELECT Set_AddressMaster.Address FROM Set_AddressMaster WHERE Set_AddressMaster.Trans_Id = (SELECT TOP 1 Set_AddressChild.Ref_Id FROM Set_AddressChild inner join ac_accountMaster on ac_accountMaster.ref_id=Set_AddressChild.Add_Ref_Id WHERE (Set_AddressChild.Add_Type = 'Contact' or Set_AddressChild.Add_Type = 'Customer') AND ac_accountMaster.trans_id = '" + HDFOtherAccountId.Value + "')");
            }
            if (ViewState["EmailFooterForStatement"] == null)
            {
                ViewState["EmailFooterForStatement"] = objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Statement_Footer").Rows[0]["Param_Value"].ToString();
            }
        }
        catch
        { }
        strCustomerAddress = ViewState["CustomerAddressForStatement"] == null ? "" : ViewState["CustomerAddressForStatement"].ToString();
        strEmailFooter = ViewState["EmailFooterForStatement"] == null ? "" : ViewState["EmailFooterForStatement"].ToString();
        strCustomerAddress = strCustomerAddress == "@NOTFOUND@" ? "" : strCustomerAddress;
        objCustomerAgeingStatement.setCustomerAddress(strCustomerAddress);
        objCustomerAgeingStatement.setFooterNote(strEmailFooter);
        objCustomerAgeingStatement.CreateDocument();
        ReportViewer1.OpenReport(objCustomerAgeingStatement);

    }
    protected void reset()
    {
        HDFCustomerID.Value = "0";
        HDFStatementDate.Value = "0";
        HDFLocations.Value = "0";
        HDFOtherAccountId.Value = "0";
    }
    protected void btnsendMail_Click(object sender, EventArgs e)
    {
        string strRecentInsertedRecord = string.Empty; //comma saperated value of tran_d field
        foreach (GridViewRow gvr in GvList.Rows)
        {
            CheckBox chkChecked = (CheckBox)gvr.FindControl("chkgvSelect");
            Label lblTransId = (Label)gvr.FindControl("lblTransId");
            if (chkChecked.Checked == true)
            {
                if (strRecentInsertedRecord == string.Empty)
                {
                    strRecentInsertedRecord = strRecentInsertedRecord + lblTransId.Text;
                }
                else
                {
                    strRecentInsertedRecord = strRecentInsertedRecord + "," + lblTransId.Text;
                }
            }
        }
        if (strRecentInsertedRecord != string.Empty)
        {
            DisplayMessage("Email send process for customer statement has been started in background, you will get notification after completion");
            ThreadStart ts = delegate () {new Ac_EmailSend(Session["DBConnection"].ToString()).sendEmail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["CompName"].ToString(), Server.MapPath(Session["CompanyLogoUrl"].ToString()), Server.MapPath("~/Temp"), Session["EmpId"].ToString(), Session["UserId"].ToString(), "RV", strRecentInsertedRecord, Session["DBConnection"].ToString()); };
            Thread t = new Thread(ts);
            t.Start();
            //objAcEmailLog.sendEmail(Session["LocId"].ToString(), "RV", strRecentInsertedRecord);
        }
    }


    protected void chkSelectBin_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvVoucherBin.Rows[index].FindControl("lblBinTransID");
        if (((CheckBox)GvVoucherBin.Rows[index].FindControl("chkSelectBin")).Checked)
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
}