using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class WebUserControl_AgeingScreen : System.Web.UI.UserControl
{
    DataAccessClass da = null;
    Common cmn = null;
    Set_Suppliers objSupplier = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    CurrencyMaster objCurrency = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        da = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            txtPaymentdate_CalendarExtender.Format = objsys.SetDateFormat();
            txtPaymentdate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            FillLocation();
        }



        try
        {
            if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Customer")
            {
                lblContactname.Text = Resources.Attendance.Customer_Name;

            }
            if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Supplier")
            {

                lblContactname.Text = Resources.Attendance.Supplier_Name;
            }
        }
        catch
        {
        }


    }






    #region systemLevel

    public string SetDecimal(string amount)
    {
        return objsys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
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


    #endregion



    #region PendingPaymentCode



    protected void btnGetPendingList_OnClick(object sender, EventArgs e)
    {
        string strAgeingType = string.Empty;


        string strOtherAccountNo = string.Empty;


        if (txtSupplierPendingPayment.Text == "")
        {
            strOtherAccountNo = "0";
            gvPendingPayment.Columns[1].Visible = true;
        }
        else
        {
            strOtherAccountNo = txtSupplierPendingPayment.Text.Split('/')[1].ToString();
            gvPendingPayment.Columns[1].Visible = false;
        }

        gvPendingPayment.Columns[gvPendingPayment.Columns.Count - 1].Visible = false;





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

                gvPendingPayment.Columns[gvPendingPayment.Columns.Count - 1].Visible = true;
            }
        }

        if (strLocationId.Trim() == "")
        {
            strLocationId = Session["LocId"].ToString();
        }
        else
        {

        }




        DataTable dtPendingPayment = new DataTable();



        if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Customer")
        {

            dtPendingPayment = objAgeingDetail.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "RV", strOtherAccountNo, "0", "", true);
        }
        else
        {
            dtPendingPayment = objAgeingDetail.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "PV", strOtherAccountNo, "0", "", true);
        }

        dtPendingPayment = new DataView(dtPendingPayment, "actual_balance_amt>0", "paymentDate", DataViewRowState.CurrentRows).ToTable();

        if (txtPaymentdate.Text != "")
        {
            dtPendingPayment = new DataView(dtPendingPayment, "paymentDate='" + Convert.ToDateTime(txtPaymentdate.Text) + "'", "paymentDate", DataViewRowState.CurrentRows).ToTable();
        }

        if (dtPendingPayment.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
        }


        objPageCmn.FillData((object)gvPendingPayment, dtPendingPayment, "", "");

    }


    protected void btnSIEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvPaymentRow = (GridViewRow)((Button)sender).Parent.Parent;

        string strFireignExchange = SystemParameter.GetCurrency(((HiddenField)gvPaymentRow.FindControl("hdnGvCurrencyId")).Value, ((Label)gvPaymentRow.FindControl("lbldueamount")).Text, Session["DBConnection"].ToString(),Session["CompId"].ToString(),Session["LocId"].ToString());

        if (strFireignExchange == "")
        {
            strFireignExchange = "0";
        }




        ((TextBox)Parent.FindControl("txtSupplierName")).Text = e.CommandName.ToString() + "/" + e.CommandArgument.ToString();


        ((Panel)Parent.FindControl("pnlMenuAgeingDetail")).BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        ((Panel)Parent.FindControl("pnlMenuSettle")).BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        ((Panel)Parent.FindControl("pnlMenuList")).BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        ((Panel)Parent.FindControl("pnlMenuNew")).BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        ((Panel)Parent.FindControl("pnlMenuBin")).BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        ((Panel)Parent.FindControl("pnlMenuPendingPayment")).BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        ((Panel)Parent.FindControl("pnlPendingPaymentDetail")).Visible = false;
        ((Panel)Parent.FindControl("PnlList")).Visible = false;
        ((Panel)Parent.FindControl("PnlNewEdit")).Visible = true;
        ((Panel)Parent.FindControl("PnlBin")).Visible = false;
        ((Button)Parent.FindControl("btnAddSupplier")).Visible = true;
        ((Panel)Parent.FindControl("PnlSettelment")).Visible = false;
        ((Panel)Parent.FindControl("PnlAgeingDetail")).Visible = false;




        //code start



        string sql = "select MAX(Currency_Id) as Currency_Id,ROW_NUMBER() over(order by max(Trans_id) asc) as Trans_Id,Invoice_No,Invoice_Date,MAX(Invoice_Amount) as Invoice_Amount,sum(Paid_Receive_Amount) as Paid_Receive_Amount,   max(Invoice_Amount)-sum(Paid_Receive_Amount) as Due_Amount,Ref_Type,Ref_Id,Company_Id,Brand_Id, Location_Id, IsActive  from ac_ageing_detail group by Company_Id,Brand_Id, Location_Id, Ref_Type,Ref_Id,Invoice_No,Invoice_Date,Other_Account_No,AgeingType,IsActive  having max(Invoice_Amount)-sum(Paid_Receive_Amount)>0 and other_account_no='" + e.CommandArgument.ToString() + "' and AgeingType='PV' and IsActive='True'";
        DataTable dtDetail = da.return_DataTable(sql);
        if (dtDetail.Rows.Count > 0)
        {
            dtDetail = new DataView(dtDetail, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }


        GridView GvPendingInvoice = ((GridView)Parent.FindControl("GvPendingInvoice"));
        GvPendingInvoice.DataSource = dtDetail;
        GvPendingInvoice.DataBind();
        ((DropDownList)Parent.FindControl("ddlForeginCurrency")).SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();


        if (((DropDownList)Parent.FindControl("ddlForeginCurrency")).SelectedIndex == 0)
        {
            ((HiddenField)Parent.FindControl("hdnFExchangeRate")).Value = "0";
            ((TextBox)Parent.FindControl("txtExchangeRate")).Text = "0";
        }
        else
        {

            if (((TextBox)Parent.FindControl("txtPaidLocalAmount")).Text != "")
            {
                strFireignExchange = SystemParameter.GetCurrency(((DropDownList)Parent.FindControl("ddlForeginCurrency")).SelectedValue, ((TextBox)Parent.FindControl("txtPaidLocalAmount")).Text, Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
                ((TextBox)Parent.FindControl("txtPaidForeignamount")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
                ((TextBox)Parent.FindControl("txtPaidForeignamount")).Text = SetDecimal(((TextBox)Parent.FindControl("txtPaidForeignamount")).Text);
                ((HiddenField)Parent.FindControl("hdnFExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
                ((TextBox)Parent.FindControl("txtExchangeRate")).Text = ((HiddenField)Parent.FindControl("hdnFExchangeRate")).Value;
                ((TextBox)Parent.FindControl("txtExchangeRate")).Text = SetDecimal(((TextBox)Parent.FindControl("txtExchangeRate")).Text);
            }
            else
            {
                strFireignExchange = SystemParameter.GetCurrency(((DropDownList)Parent.FindControl("ddlForeginCurrency")).SelectedValue, "0", Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
                ((HiddenField)Parent.FindControl("hdnFExchangeRate")).Value = strFireignExchange.Trim().Split('/')[1].ToString();
                ((TextBox)Parent.FindControl("txtExchangeRate")).Text = ((HiddenField)Parent.FindControl("hdnFExchangeRate")).Value;
                ((TextBox)Parent.FindControl("txtExchangeRate")).Text = SetDecimal(((TextBox)Parent.FindControl("txtExchangeRate")).Text);
            }
        }



        foreach (GridViewRow gvr in GvPendingInvoice.Rows)
        {
            HiddenField hdnRefId = (HiddenField)gvr.FindControl("hdnRefId");
            HiddenField hdnRefType = (HiddenField)gvr.FindControl("hdnRefType");
            Label lblInvoiceNo = (Label)gvr.FindControl("lblPONo");
            Label lblFregnAmt = (Label)gvr.FindControl("lblFregnamount");

            DataTable dtAge = objAgeingDetail.GetAgeingDetailAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (lblInvoiceNo.Text == "OpeningBalance")
            {
                dtAge = new DataView(dtAge, "Ref_Id='" + hdnRefId.Value + "' and Ref_Type='" + hdnRefType.Value + "' and Invoice_No='" + lblInvoiceNo.Text + "' and Field3=''", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAge.Rows.Count > 0)
                {
                    lblFregnAmt.Text = dtAge.Rows[0]["Foreign_Amount"].ToString();
                }
            }
            else
            {
                dtAge = new DataView(dtAge, "Ref_Id='" + hdnRefId.Value + "' and Ref_Type='" + hdnRefType.Value + "' and Invoice_No='" + lblInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAge.Rows.Count > 0)
                {
                    lblFregnAmt.Text = dtAge.Rows[0]["Foreign_Amount"].ToString();
                }
            }

            CheckBox chkSelect = (CheckBox)gvr.FindControl("chkTrandId");
            DropDownList ddlgvCurrency = (DropDownList)gvr.FindControl("ddlgvCurrency");

            chkSelect.Enabled = true;

            DataTable dsCurrency = null;
            dsCurrency = objCurrency.GetCurrencyMaster();
            if (dsCurrency.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlgvCurrency, dsCurrency, "Currency_Name", "Currency_ID");
                ddlgvCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();
            }
        }

        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        foreach (GridViewRow gvS in GvPendingInvoice.Rows)
        {
            Label lblInvAmt = (Label)gvS.FindControl("lblinvamount");
            Label lblPaidAmt = (Label)gvS.FindControl("lblpaidamount");
            Label lblDueAmt = (Label)gvS.FindControl("lbldueamount");
            Label lblForeignAmt = (Label)gvS.FindControl("lblFregnamount");

            lblInvAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblInvAmt.Text);
            lblPaidAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblPaidAmt.Text);
            lblDueAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblDueAmt.Text);
            lblForeignAmt.Text = objsys.GetCurencyConversionForInv(strCurrency, lblForeignAmt.Text);
        }



        //btnAddSupplier_OnClick(null, null);

        DataTable dt = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            ((TextBox)Parent.FindControl("txtSupplierName")).Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Supplier_Id"].ToString();
            Session["SupplierAccountId"] = dt.Rows[0]["Account_No"].ToString();
        }


        double sumForeignamt = 0;
        double sumLocal = 0;
        foreach (GridViewRow gvrow in GvPendingInvoice.Rows)
        {
            TextBox txtPayLocal = (TextBox)gvrow.FindControl("txtpayLocal");
            TextBox txtPayForeign = (TextBox)gvrow.FindControl("txtpayforeign");


            if (((HiddenField)gvPaymentRow.FindControl("hdnRefId")).Value == ((HiddenField)gvrow.FindControl("hdnRefId")).Value)
            {
                ((CheckBox)gvrow.FindControl("chkTrandId")).Checked = true;
            }

            if (((CheckBox)gvrow.FindControl("chkTrandId")).Checked)
            {
                ((TextBox)gvrow.FindControl("txtpayforeign")).Text = strFireignExchange.Trim().Split('/')[0].ToString();
                ((HiddenField)gvrow.FindControl("hdnExchangeRate")).Value = SetDecimal(strFireignExchange.Trim().Split('/')[1].ToString());
                ((TextBox)gvrow.FindControl("txtgvExcahangeRate")).Text = SetDecimal(strFireignExchange.Trim().Split('/')[1].ToString());


                txtPayLocal.Text = ((Label)gvrow.FindControl("lbldueamount")).Text;


                if (txtPayLocal.Text == "")
                {
                    txtPayLocal.Text = "0";
                }
                sumLocal += Convert.ToDouble(txtPayLocal.Text);

                if (txtPayForeign.Text == "")
                {
                    txtPayForeign.Text = "0";
                }
                sumForeignamt += Convert.ToDouble(txtPayForeign.Text);
            }
        }
        ((TextBox)Parent.FindControl("txtPaidLocalAmount")).Text = sumLocal.ToString();
        ((TextBox)Parent.FindControl("txtPaidForeignamount")).Text = sumForeignamt.ToString();
        ((TextBox)Parent.FindControl("txtPaidLocalAmount")).Text = SetDecimal(((TextBox)Parent.FindControl("txtPaidLocalAmount")).Text);
        ((TextBox)Parent.FindControl("txtPaidForeignamount")).Text = SetDecimal(((TextBox)Parent.FindControl("txtPaidForeignamount")).Text);
        GetNetAmount();


        //btnNew_Click(null, null);




    }


    public string GetNetAmount()
    {
        string strTotal = "0";
        double LocalPaid = 0;
        double BankCharges = 0;
        if (((TextBox)Parent.FindControl("txtPaidLocalAmount")).Text != "")
        {
            LocalPaid = Convert.ToDouble(((TextBox)Parent.FindControl("txtPaidLocalAmount")).Text);
        }
        else
        {
            LocalPaid = 0;
        }

        if (((TextBox)Parent.FindControl("txtBankCharges")).Text != "")
        {
            BankCharges = Convert.ToDouble(((TextBox)Parent.FindControl("txtBankCharges")).Text);
        }
        else
        {
            BankCharges = 0;
        }

        if (LocalPaid != 0)
        {
            strTotal = LocalPaid.ToString();
        }
        if (BankCharges != 0)
        {
            strTotal = (float.Parse(LocalPaid.ToString()) + float.Parse(BankCharges.ToString())).ToString();
        }

        ((TextBox)Parent.FindControl("txtNetAmount")).Text = strTotal;
        ((TextBox)Parent.FindControl("txtNetAmount")).Text = SetDecimal(((TextBox)Parent.FindControl("txtNetAmount")).Text);
        return strTotal;
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
            else if (RefType.Trim() == "SINV")
            {
                string strCmd = string.Format("window.open('../Sales/SalesInvoice.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
        }
        else
        {
            //myButton.Attributes.Add("
            DisplayMessage("No Data");
            return;
        }

    }


    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
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
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", Session["UserId"].ToString(), Session["CompId"].ToString(),Session["EmpId"].ToString());

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
            objPageCmn.FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
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



    #endregion
}