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

public partial class SuppliersPayable_AgeingReportSupplier : System.Web.UI.Page
{
    DataAccessClass da = null;
    Set_CustomerMaster ObjCoustmer = null;
    Set_Suppliers objSupplier = null;
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
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    string strdtTodate = string.Empty;
    string strdtFromdate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        da = new DataAccessClass(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
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
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SuppliersPayable/AgeingReportSupplier.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "309").ToString() == "False")
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


    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnGetReport.Visible = clsPagePermission.bPrint;
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

        ////End Code
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
                        return;
                    }

                }
            }
        }
        catch
        { }
        DisplayMessage("Supplier is not valid");
        txtSupplierName.Text = "";
        txtSupplierName.Focus();
        //if (txtSupplierName.Text != "")
        //{
        //    try
        //    {
        //        txtSupplierName.Text.Split('/')[1].ToString();
        //    }
        //    catch
        //    {
        //        DisplayMessage("Enter Supplier Name");
        //        txtSupplierName.Text = "";
        //        txtSupplierName.Focus();
        //        return;
        //    }

        //    DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierName.Text.Trim().Split('/')[0].ToString());
        //    if (dt.Rows.Count == 0)
        //    {
        //        DisplayMessage("Select Supplier Name");
        //        txtSupplierName.Text = "";
        //        txtSupplierName.Focus();
        //    }
        //    else
        //    {
        //        string strSupplierId = dt.Rows[0]["Trans_Id"].ToString();
        //        if (strSupplierId != "0" && strSupplierId != "")
        //        {
        //            DataTable dtSup = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strSupplierId);
        //            if (dtSup.Rows.Count > 0)
        //            {
        //                Session["SupplierAccountId"] = dtSup.Rows[0]["Account_No"].ToString();
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Select Supplier Name");
        //    txtSupplierName.Focus();
        //}
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    #region Auto Complete Method
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
        //Set_Suppliers ObjSupplier = new Set_Suppliers();
        //DataTable dtSupplier = ObjSupplier.GetAutoCompleteSupplierAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);

        //string[] filterlist = new string[dtSupplier.Rows.Count];
        //if (dtSupplier.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dtSupplier.Rows.Count; i++)
        //    {
        //        filterlist[i] = dtSupplier.Rows[i]["Name"].ToString() + "/" + dtSupplier.Rows[i]["Supplier_Id"].ToString();
        //    }
        //}
        //return filterlist;
    }
    #endregion
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

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

        GVAgeingReport.Columns[GVAgeingReport.Columns.Count - 1].Visible = false;




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
                strLocationId = strLocationId + "," + lstLocationSelect.Items[i].Value;


                GVAgeingReport.Columns[GVAgeingReport.Columns.Count - 1].Visible = true;
            }
        }

        //if (strLocationId != "")
        //{

        //}
        //else
        //{
        //    strLocationId = Session["LocId"].ToString();
        //}

        DataTable dtAgeing = new DataTable();

        if (strLocationId == string.Empty)
        {
            strLocationId = Session["LocId"].ToString();
        }

        if (txtSupplierName.Text == "")
        {

            dtAgeing = objAgeing.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "PV", "0", "0", "", true);


            GVAgeingReport.Columns[1].Visible = true;
        }
        else
        {
            dtAgeing = objAgeing.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "PV", txtSupplierName.Text.Split('/')[1].ToString(), "0", "", true);

            GVAgeingReport.Columns[1].Visible = false;
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
            dtAgeing = new DataView(dtAgeing, "PaymentDate=>'" + txtFromDate.Text + "' and PaymentDate<='" + txtToDate.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        GVAgeingReport.DataSource = dtAgeing;
        GVAgeingReport.DataBind();



        //DataTable dtAgeing = objAgeing.GetAgeingDetailforSupplierAgeing(Session["BrandId"].ToString(), strLocationId, txtFromDate.Text, txtToDate.Text);
        //if (dtAgeing.Rows.Count > 0 && dtAgeing != null)
        //{
        //    if (txtSupplierName.Text != "")
        //    {
        //        dtAgeing = new DataView(dtAgeing, "Other_Account_No='" + txtSupplierName.Text.Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }



        //if (dtAgeing.Rows.Count > 0 && dtAgeing != null)
        //{
        //    dtAgeing.Columns["Po_No"].ReadOnly = false;


        //}

        //string po_number = "";
        //if (dtAgeing.Rows.Count > 0)
        //{
        //    foreach (DataRow dr in dtAgeing.Rows)
        //    {
        //        po_number = "";
        //        string sql = "select distinct (Case when Inv_PurchaseInvoiceHeader.SupInvoiceNo='' or Inv_PurchaseInvoiceHeader.SupInvoiceNo is null then '0' else Inv_PurchaseInvoiceHeader.SupInvoiceNo end) as po_number from dbo.Inv_PurchaseInvoiceHeader where Inv_PurchaseInvoiceHeader.InvoiceNo='" + dr["Invoice_No"].ToString() + "'";
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
            if (RefType.Trim() == "PINV")
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

        string strSupplierId = string.Empty;
        if (txtSupplierName.Text != "")
        {
            strSupplierId = txtSupplierName.Text.Split('/')[1].ToString();
        }
        else
        {
            strSupplierId = "0";
        }

        ArrayList objArr = new ArrayList();
        objArr.Add(strSupplierId);
        objArr.Add(strLocationId);
        objArr.Add(txtFromDate.Text);
        objArr.Add(txtToDate.Text);
        objArr.Add(strInvoiceStatus);
        objArr.Add(txtOverdueDays.Text);
        objArr.Add("PV");
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
        if (RoleId.IndexOf(',') != -1)
        {
            string[] values = RoleId.Split(',');

            // Output each value
            foreach (string value in values)
            {
                DataTable dtRole = objRole.GetRoleMaster();
                dtRole = new DataView(dtRole, "Role_Id='" + value + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtRole.Rows.Count > 0)
                {
                    string str = dtRole.Rows[0]["Role_Name"].ToString().Trim().ToLower();
                    if (str == "super admin")
                    {
                        status = true;
                    }
                }
            }
        }
        else
        {
            DataTable dtRole = objRole.GetRoleMaster();
            dtRole = new DataView(dtRole, "Role_Id='" + RoleId + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtRole.Rows.Count > 0)
            {
                string str = dtRole.Rows[0]["Role_Name"].ToString().Trim().ToLower();
                if (str == "super admin")
                {
                    status = true;
                }
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
}