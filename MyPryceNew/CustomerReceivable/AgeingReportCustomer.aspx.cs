using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using net.webservicex.www;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Threading;

public partial class CustomerReceivable_AgeingReportCustomer : System.Web.UI.Page
{
    DataAccessClass da = null;
    Set_CustomerMaster ObjCoustmer = null;
    Ems_ContactMaster ObjContactMaster = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    Ac_Ageing_Detail objAgeing = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Common cmn = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    Ac_Finance_Year_Info objFY = null;
    Set_ApplicationParameter objAppParam = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        da = new DataAccessClass(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAgeing = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objFY = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/ERPLogin.aspx");
            }

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../CustomerReceivable/AgeingReportCustomer.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "301").ToString() == "False")
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/ERPLogin.aspx");
            //}

            //AllPageCode();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            FillLocation();

            Calender_VoucherDate.Format = objsys.SetDateFormat();
            CalendarExtender1.Format = objsys.SetDateFormat();

            DataTable dtFY = objFY.GetInfoAllTrue(StrCompId);
            if (dtFY.Rows.Count > 0)
            {
                string strFromDate = dtFY.Rows[0]["From_Date"].ToString();
                txtFromDate.Text = DateTime.Parse(strFromDate).ToString(objsys.SetDateFormat());
                txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            }
            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString["SearchField"].ToString() != "")
                {

                    string[] strFields = Common.Decrypt(Request.QueryString["SearchField"].ToString()).Split(',');
                    txtToDate.Text = strFields[1].ToString();
                    strLocationId = strFields[0].ToString();
                    btnGetReport_Click(null, null);
                }
            }
        }
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }
        //For Comment
        //StrCompId = Session["CompId"].ToString();
        //StrBrandId = Session["BrandId"].ToString();
        //strLocationId = Session["LocId"].ToString();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnGetReport.Visible = clsPagePermission.bAdd;
        //IT_ObjectEntry objObjectEntry = new IT_ObjectEntry();
        //Common ObjComman = new Common();

        //txtFromDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
        //txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());

        ////New Code created by jitendra on 09-12-2014
        //string strModuleId = string.Empty;
        //string strModuleName = string.Empty;

        //DataTable dtModule = objObjectEntry.GetModuleIdAndName("301");
        //if (dtModule.Rows.Count > 0)
        //{
        //    strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
        //    strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        //}
        //else
        //{
        //    Session.Abandon();
        //    Response.Redirect("~/ERPLogin.aspx");
        //}

        //End Code
        //Page.Title = objsys.GetSysTitle();
        //StrUserId = Session["UserId"].ToString();
        //StrCompId = Session["CompId"].ToString();
        //StrBrandId = Session["BrandId"].ToString();
        //strLocationId = Session["LocId"].ToString();
        //Session["AccordianId"] = strModuleId;
        //Session["HeaderText"] = strModuleName;
        //if (Session["EmpId"].ToString() == "0")
        //{
        //    btnGetReport.Visible = true;
        //}
        //else
        //{
        //    DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "301");
        //    if (dtAllPageCode.Rows.Count == 0)
        //    {
        //        Session.Abandon();
        //        Response.Redirect("~/ERPLogin.aspx");
        //    }
        //    foreach (DataRow DtRow in dtAllPageCode.Rows)
        //    {
        //        if (DtRow["Op_Id"].ToString() == "1")
        //        {
        //            btnGetReport.Visible = true;
        //        }
        //    }
        //}
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
                        return;
                    }

                }
            }
        }
        catch
        {

        }
        DisplayMessage("Customer is not valid");
        txtCustomerName.Text = "";
        txtCustomerName.Focus();
        //if (txtCustomerName.Text != "")
        //{
        //    try
        //    {
        //        txtCustomerName.Text.Split('/')[1].ToString();
        //    }
        //    catch
        //    {
        //        DisplayMessage("Enter Customer Name");
        //        txtCustomerName.Text = "";
        //        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
        //        return;
        //    }

        //    DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerName.Text.Trim().Split('/')[0].ToString());
        //    if (dt.Rows.Count == 0)
        //    {
        //        DisplayMessage("Select Customer Name");
        //        txtCustomerName.Text = "";
        //        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
        //    }
        //    else
        //    {
        //        string strCustomerId = dt.Rows[0]["Trans_Id"].ToString();
        //        if (strCustomerId != "0" && strCustomerId != "")
        //        {
        //            DataTable dtCus = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerId);
        //            if (dtCus.Rows.Count > 0)
        //            {
        //                Session["CustomerAccountId"] = dtCus.Rows[0]["Account_No"].ToString();
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Select Customer Name");
        //    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
        //}
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    #region Auto Complete Method
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
    #endregion
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        string strCurrency = Session["LocCurrencyId"].ToString();

        //check from and to date
        if (txtFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Please enter valid date");
                txtFromDate.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Please enter valid date");
            txtFromDate.Focus();
            return;
        }

        if (txtToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Please enter valid date");
                txtToDate.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Please enter valid date");
            txtToDate.Focus();
            return;
        }

        //GVAgeingReport.Columns[GVAgeingReport.Columns.Count - 1].Visible = false;

        //for Selected Location
        // string strLocationId = string.Empty;
        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
        {
            if (strLocationId == "")
            {
                strLocationId = lstLocationSelect.Items[i].Value;
            }
            else
            {
                GVAgeingReport.Columns[GVAgeingReport.Columns.Count - 1].Visible = true;
                strLocationId = strLocationId + "," + lstLocationSelect.Items[i].Value;
            }
        }

        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }


        DataTable dtAgeing = new DataTable();


        if (txtCustomerName.Text == "")
        {

            dtAgeing = objAgeing.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "RV", "0", "0", "", true);

            GVAgeingReport.Columns[2].Visible = true;
        }
        else
        {

            GVAgeingReport.Columns[2].Visible = false;
            dtAgeing = objAgeing.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "RV", txtCustomerName.Text.Split('/')[1].ToString(), "0", "", true);
        }



        if (txtOverdueDays.Text != "")
        {
            dtAgeing = new DataView(dtAgeing, "Due_Days>='" + txtOverdueDays.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }


        if (chkAllInvoices.Checked == false)
        {
            dtAgeing = new DataView(dtAgeing, "actual_balance_amt<>0", "", DataViewRowState.CurrentRows).ToTable();
        }

        try
        {
            dtAgeing = new DataView(dtAgeing, "PaymentDate>='" + txtFromDate.Text + "' and PaymentDate<='" + txtToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        GVAgeingReport.DataSource = dtAgeing;
        GVAgeingReport.DataBind();
        Session["dtAgeingData"] = dtAgeing;


        ////string sql = "select  MAX(ac_ageing_detail.Currency_Id) as Currency_Id,max(Sys_CurrencyMaster.Currency_Name) as CurrencyName,'Net ' + cast(Credit_Days as varchar) as Payment_Terms,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,cast('' as varchar(100)) as Po_No,Invoice_Date,cast(DATEADD(D,Credit_Days,Invoice_Date)as date) as Due_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id, Location_Id,cast((DATEDIFF(d ,DATEADD(D,Credit_Days,Invoice_Date),GETDATE())) as varchar) as Days_Overdue from ac_ageing_detail inner join Set_CustomerMaster on Set_CustomerMaster.Customer_Id=ac_ageing_detail.Other_Account_No inner join Sys_CurrencyMaster on Ac_Ageing_Detail.Currency_Id=Sys_CurrencyMaster.Currency_ID where Set_CustomerMaster.Brand_Id='" + Session["BrandId"].ToString() + "' and Ac_Ageing_Detail.invoice_no<>'0' and Ac_Ageing_Detail.invoice_no<>'OpeningBalance' and invoice_date >='" + txtFromDate.Text + "' and Invoice_Date<='" + txtToDate.Text + "' group by Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,AgeingType,Location_Id,Credit_Days  having Location_Id in(" + strLocationId + ") and AgeingType='RV'";
        ////sql = sql + " " + where_sql;
        //DataTable dtAgeing = objAgeing.GetAgeingDetailforCustomerAgeing(Session["BrandId"].ToString(), strLocationId, txtFromDate.Text, txtToDate.Text);
        //if (dtAgeing.Rows.Count > 0 && dtAgeing != null)
        //{
        //    if (txtCustomerName.Text != "")
        //    {
        //        dtAgeing = new DataView(dtAgeing, "Other_Account_No='" + txtCustomerName.Text.Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }

        //    if (txtOverdueDays.Text != "")
        //    {
        //        dtAgeing = new DataView(dtAgeing, "Days_Overdue>='" + txtOverdueDays.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //}

        //if (dtAgeing.Rows.Count > 0 && dtAgeing != null)
        //{
        //    dtAgeing.Columns["Po_No"].ReadOnly = false;

        //    if (chkAllInvoices.Checked == false)
        //    {
        //        dtAgeing = new DataView(dtAgeing, "Due_Amount<>0", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //}

        //string po_number = "";
        //if (dtAgeing.Rows.Count > 0)
        //{
        //    foreach (DataRow dr in dtAgeing.Rows)
        //    {
        //        po_number = "";
        //        string sql = "select distinct (Case when Inv_SalesOrderHeader.field3='' or Inv_SalesOrderHeader.field3 is null then '0' else Inv_SalesOrderHeader.field3 end) as po_number from dbo.Inv_SalesOrderHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceDetail.SIFromTransNo=Inv_SalesOrderHeader.Trans_Id inner join Inv_SalesInvoiceHeader on Inv_SalesInvoiceDetail.Invoice_No=Inv_SalesInvoiceHeader.Trans_Id where Inv_SalesInvoiceHeader.Invoice_No='" + dr["Invoice_No"].ToString() + "'";
        //        DataTable dtPoNumber = da.return_DataTable(sql);
        //        if (dtPoNumber.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr_po in dtPoNumber.Rows)
        //            {
        //                if (po_number != "")
        //                {
        //                    po_number = po_number + "," + dr_po["po_number"].ToString();
        //                }
        //                else
        //                {
        //                    po_number = dr_po["po_number"].ToString();
        //                }
        //            }
        //        }
        //        dtPoNumber.Dispose();
        //        dr["Po_No"] = po_number;
        //    }

        //    GVAgeingReport.DataSource = dtAgeing;
        //    GVAgeingReport.DataBind();
        //}
        //else
        //{
        //    GVAgeingReport.DataSource = null;
        //    GVAgeingReport.DataBind();
        //    DisplayMessage("You have no record");
        //    return;
        //}


        // for total Record
        //string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        double InvoiceTotalAmount = 0;
        double DueTotalAmount = 0;
        foreach (GridViewRow gvr in GVAgeingReport.Rows)
        {
            Label lblInvoiceAmt = (Label)gvr.FindControl("lblInvoiceAmt");
            Label lblReceiveAmt = (Label)gvr.FindControl("lblReceiveAmt");
            Label lblDueAmt = (Label)gvr.FindControl("lblDueAmt");
            lblInvoiceAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblInvoiceAmt.Text);
            lblDueAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblDueAmt.Text);
            InvoiceTotalAmount += Convert.ToDouble(lblInvoiceAmt.Text);
            DueTotalAmount += Convert.ToDouble(lblDueAmt.Text);
        }
        if (GVAgeingReport.Rows.Count > 0)
        {
            Label lblgvInvoiceAmtTotal = (Label)GVAgeingReport.FooterRow.FindControl("lblgvInvoiceAmtTotal");
            Label lblgvDueAmtTotal = (Label)GVAgeingReport.FooterRow.FindControl("lblgvDueAmtTotal");

            lblgvInvoiceAmtTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, InvoiceTotalAmount.ToString());
            lblgvDueAmtTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, DueTotalAmount.ToString());
        }
    }
    protected void lblgvInvoiceNo_Click(object sender, EventArgs e)
    {
        string strVoucherType = string.Empty;
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string RefId = arguments[0];
        string RefType = arguments[1];
        string LocationId = arguments[2].Trim();

        if (RefId != "0" && RefId != "")
        {
            if (RefType.Trim() == "SINV")
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
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId,Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }
    protected void btnShowReport_Click(object sender, EventArgs e)
    {
        btnGetReport_Click(null, null);

        //for Selected Location
        string strLocationId = string.Empty;
        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
        {
            if (strLocationId == "")
            {
                strLocationId = lstLocationSelect.Items[i].Value;
            }
            else
            {
                strLocationId = strLocationId + "," + lstLocationSelect.Items[i].Value;
            }
        }

        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }

        string strInvoiceStatus = string.Empty;
        if (chkAllInvoices.Checked == true)
        {
            strInvoiceStatus = "True";
        }
        else if (chkAllInvoices.Checked == false)
        {
            strInvoiceStatus = "False";
        }

        string strCustomerId = string.Empty;
        if (txtCustomerName.Text != "")
        {
            strCustomerId = txtCustomerName.Text.Split('/')[1].ToString();
        }
        else
        {
            strCustomerId = "0";
        }

        ArrayList objArr = new ArrayList();
        objArr.Add(strCustomerId);
        objArr.Add(strLocationId);
        objArr.Add(txtFromDate.Text);
        objArr.Add(txtToDate.Text);
        objArr.Add(strInvoiceStatus);
        objArr.Add(txtOverdueDays.Text);
        objArr.Add("RV");
        objArr.Add(chkAllInvoices.Checked.ToString());

        Session["dtAcParam"] = objArr;
        string strCmd = string.Format("window.open('../Accounts_Report/CustomerAgeingPrint.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    public string GetDateFormat(string Value)
    {
        string newdate = string.Empty;
        try
        {
            newdate = Convert.ToDateTime(Value).ToString(objsys.SetDateFormat());
        }
        catch
        {

        }
        return newdate;
    }

    #region LocationWork
    public void FillLocation()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();
        //DataTable dtloc = new DataTable();
        //dtloc = ObjLocation.GetAllLocationMaster();

        DataTable dtLoc = ObjLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());


        if (!GetStatus(Session["RoleId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
    }
    public bool GetStatus(string RoleId)
    {
        bool status = false;
        DataTable dtRole = objRole.GetRoleMaster();
        dtRole = new DataView(dtRole, "Role_Id IN (" + RoleId + ")", "", DataViewRowState.CurrentRows).ToTable();
        if (dtRole.Rows.Count > 0)
        {
            string str = dtRole.Rows[0]["Role_Name"].ToString().Trim().ToLower();
            if (str == "super admin")
            {
                status = true;
            }
        }
        return status;
    }
    public string GetDeptIdbyTransId(string TransId)
    {
        string DepId = string.Empty;

        DataTable dt = objLocDept.GetDepartmentLocation();
        dt = new DataView(dt, "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            DepId = dt.Rows[0]["Dep_Id"].ToString();
        }
        return DepId;
    }
    protected void btnPushDept_Click(object sender, EventArgs e)
    {
        if (lstLocation.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocationSelect.Items)
            {
                lstLocation.Items.Remove(li);
            }
            lstLocationSelect.SelectedIndex = -1;
        }
        btnPushDept.Focus();
    }
    protected void btnPullDept_Click(object sender, EventArgs e)
    {
        if (lstLocationSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    lstLocation.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Remove(li);
                }
            }
            lstLocation.SelectedIndex = -1;
        }
        btnPullDept.Focus();
    }
    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllDept.Focus();
    }
    #endregion

    protected void btnsendMail_Click(object sender, EventArgs e)
    {
        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Enter Customer Name");
            txtCustomerName.Focus();
            return;
        }
        if (GVAgeingReport.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }

        DataTable dttemp = new DataTable();
        string strMailId = string.Empty;
        Session["RV_AGE_Message"] = GetmailContentForAgeingReport();
        DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(txtCustomerName.Text.Split('/')[1].ToString());
        string strCustomerId = _dtTemp.Rows[0]["Ref_Id"].ToString();
        _dtTemp.Dispose();
        Response.Redirect("../EmailSystem/ContactMailReference.aspx?Page=RV_AGE&&ConId=" + strCustomerId + "&&PageRefType=RV_AGE");

        //strMailId = da.return_DataTable("select Field1 from ems_contactmaster where Trans_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "").Rows[0][0].ToString().Trim();

        //if (strMailId == "")
        //{
        //    DisplayMessage("Email not configured for this customer");
        //    return;
        //}

        //if (!IsValidEmailId(strMailId))
        //{
        //    DisplayMessage("Customer Email-Id is invalid");
        //    return;
        //}


        //string strAppMailId = string.Empty;
        //string strAppPassword = string.Empty;
        //string strSmtpHost = string.Empty;
        //string strSmtpPort = string.Empty;

        //strAppMailId = objAppParam.GetApplicationParameterValueByParamName("Master_Email", Session["CompId"].ToString());
        //strAppPassword = objAppParam.GetApplicationParameterValueByParamName("Master_Email_Password", Session["CompId"].ToString());
        //strSmtpHost = objAppParam.GetApplicationParameterValueByParamName("Master_Email_SMTP", Session["CompId"].ToString());
        //strSmtpPort = objAppParam.GetApplicationParameterValueByParamName("Master_Email_Port", Session["CompId"].ToString());
        //string strBody = GetmailContentForAgeingReport();

        //string strSubject = "Account Statement from " + ((Label)Master.FindControl("Lbl_Company_name")).Text;
        
        //if (SendEmail(strMailId, "", "", strAppMailId.Trim(), strAppPassword.Trim(), strSmtpHost.Trim(), strSmtpPort.Trim(), strBody, strSubject))
        //{
        //    DisplayMessage("Mail Sent Successfully");
        //}
        //else
        //{
        //    DisplayMessage("Mail Failed due to week internet connection , please try after some time");
        //}

    }



    public bool SendEmail(string strTo, string strCC, string BCC, string strSenderEmail, string strSenderEmailPassword, string SenderHost, string Senderport, string strBody, string strSubject)
    {

        bool result = false;
        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();


        foreach (string str in strTo.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.To.Add(str);
        }
        foreach (string str in strCC.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.CC.Add(str);
        }
        foreach (string str in BCC.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.Bcc.Add(str);
        }

        message.From = new System.Net.Mail.MailAddress(strSenderEmail, "Account Statement");
        message.Subject = strSubject;
        message.IsBodyHtml = true;
        message.Body = strBody;
        SmtpClient smtp = new SmtpClient(SenderHost);
        NetworkCredential basiccr = new NetworkCredential(strSenderEmail, Common.Decrypt(strSenderEmailPassword));
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = basiccr;
        smtp.Port = Convert.ToInt32(Senderport);
        try
        {
            smtp.Send(message);
            result = true;
        }
        catch (Exception e)
        {

        }

        return result;

    }




    public string GetmailContentForAgeingReport()
    {
        double actual_Invoice_amt = 0;
        double actual_balance_amt = 0;
        int counter = 0;
        SystemParameter ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        DataAccessClass ObjDa = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        string strMailContent = string.Empty;
        //strMailContent = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4>Respected Sir,</h4> <hr /> Please find your payment details as follow's. <br /><br /> ";
        strMailContent += "</br></br></br></br>  <table  border='1px' cellpadding='5' cellspacing='0' style='border: solid 1px Silver; font-size: x-small; width: 100%;'> <tr> <td align='left'  bgcolor='#F79646'   style='font-weight:bold;color:White; font-size: 14px;' > SNo.</td> <td align='left'  bgcolor='#F79646'   style='font-weight:bold;color:White; font-size: 14px;' > Invoice No</td>  <td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > PO Number </td>  <td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Invoice Date </td><td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Due Date</td><td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Payment Terms</td><td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Days Overdue</td><td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Currency</td><td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Invoice Amount</td><td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' >Balance Remaining</td></tr>";
        foreach (GridViewRow gvrow in GVAgeingReport.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkgvSelect")).Checked == true)
            {
                counter++;
                actual_Invoice_amt += Convert.ToDouble(((Label)gvrow.FindControl("lblInvoiceAmt")).Text);
                actual_balance_amt += Convert.ToDouble(((Label)gvrow.FindControl("lblDueAmt")).Text);
                strMailContent += "<tr><td align='left'   style='font-weight:bold; font-size: 13px;'>" + counter + "</td><td align='left'  style='font-weight:bold; font-size: 13px;'>" + ((LinkButton)gvrow.FindControl("lblInvoiceNo")).Text + "</td><td align='left'  style='font-weight:bold; font-size: 13px;'>" + ((Label)gvrow.FindControl("lblPoNo")).Text + "</td><td align='center'  style='font-weight:bold; font-size: 13px;'>" + ((Label)gvrow.FindControl("lblInvoiceDate")).Text + "</td><td align='center'  style='font-weight:bold; font-size: 13px;'>" + ((Label)gvrow.FindControl("lblDueDate")).Text + "</td><td align='center'  style='font-weight:bold; font-size: 13px;'>" + ((Label)gvrow.FindControl("lblPaymentTerms")).Text + "</td><td align='right'  style='font-weight:bold; font-size: 13px;'>" + ((Label)gvrow.FindControl("lblDaysOverdue")).Text + "</td><td align='left'  style='font-weight:bold; font-size: 13px;'>" + ((Label)gvrow.FindControl("lblCurrency")).Text + "</td><td align='right'  style='font-weight:bold; font-size: 13px;'>" + ((Label)gvrow.FindControl("lblInvoiceAmt")).Text + "</td><td align='right'  style='font-weight:bold; font-size: 13px;'>" + ((Label)gvrow.FindControl("lblDueAmt")).Text + "</td></tr>";
            }
        }
        strMailContent += "<tr><td align = 'left' colspan='8'   style = 'font-weight:bold; font-size: 13px;' > Total </td ><td align ='right' style = 'font-weight:bold; font-size: 13px;' > " + Common.GetAmountDecimal(actual_Invoice_amt.ToString(), Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) + " </td ><td align = 'right'    style = 'font-weight:bold; font-size: 13px;' > " + Common.GetAmountDecimal(actual_balance_amt.ToString(), Session["DBConnection"].ToString(),Session["LocCurrencyId"].ToString()) + " </td></tr>";
        strMailContent += "</table><Center><h5><B>** This is system generate mail and Please ignore if already paid **</B> </h5></Center></div>";
        //strMailContent += "</body> </html>";
        return strMailContent;

    }


    private bool IsValidEmailId(string InputEmail)
    {
        //Regex To validate Email Address
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(InputEmail);
        if (match.Success)
            return true;
        else
            return false;
    }

    protected void lnkContact_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton myButton = (LinkButton)sender;
            string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
            DataTable _dtTemp = objAcAccountMaster.GetAc_AccountMasterByTransId(arguments[0]);
            string CustomerId = _dtTemp.Rows[0]["Ref_id"].ToString();
            DataTable dt = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), CustomerId);
            UcContactList.fillHeader(dt);
            UcContactList.fillFollowupList(CustomerId);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
        }
        catch
        {

        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        //DataTable dt = new EmployeeMaster().GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentListPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
        }
        return str;

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetEmailIdPreFixText(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDesignationMaster(string prefixText, int count, string contextKey)
    {

        DataTable dt = new DesignationMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDesignationDataPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Designation"].ToString() + "/" + dt.Rows[i]["Designation_Id"].ToString();
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    {
        ContactNoMaster objContactNumMaster = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Phone_no"].ToString();
        }
        return txt;
    }

    protected void GVAgeingReport_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_List_Sort = (DataTable)Session["dtAgeingData"];
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

        dt_List_Sort = (new DataView(dt_List_Sort, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        objPageCmn.FillData((object)GVAgeingReport, dt_List_Sort, "", "");
        Session["dtAgeingData"] = dt_List_Sort;
        //AllPageCode();
    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ChkHeader = (CheckBox)GVAgeingReport.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gvrow in GVAgeingReport.Rows)
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
}