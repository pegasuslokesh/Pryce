using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class WebUserControl_EditAgeing : System.Web.UI.UserControl
{
    Ems_ContactMaster ObjContactMaster = null;
    Set_Suppliers objSupplier = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    DataAccessClass da = null;
    CurrencyMaster objCurrency = null;
    Common cmn = null;
    Ac_AccountMaster objAcAccountMaster = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
            objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
            objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
            objsys = new SystemParameter(Session["DBConnection"].ToString());
            ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
            da = new DataAccessClass(Session["DBConnection"].ToString());
            objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());

            if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Customer")
            {
                Session["ParentPageId"] = "1";
                lblSettleSupplier.Text = Resources.Attendance.Customer;

            }
            if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Supplier")
            {
                Session["ParentPageId"] = "2";
                lblSettleSupplier.Text = Resources.Attendance.Supplier;
            }


        }
        catch
        {
        }

    }

    public string SetDecimal(string amount)
    {
        string strCurrencyId = Session["LocCurrencyId"].ToString();
        try
        {
            HiddenField hdnCurrency = (HiddenField)this.Parent.FindControl("hdnCurrencyId");
            strCurrencyId = hdnCurrency.Value;
        }
        catch (Exception ex)
        {

        }
        return objsys.GetCurencyConversionForInv(strCurrencyId, amount);
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

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    #region SettlemetView


    protected void txtSettleSupplier_TextChanged(object sender, EventArgs e)
    {
        try
        {
            int otherAccountId = 0;
            int.TryParse(txtSettleSupplier.Text.Split('/')[1].ToString(), out otherAccountId);
            if (otherAccountId > 0)
            {
                DataTable dt = objAcAccountMaster.GetAc_AccountMasterByTransId(otherAccountId.ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Name"].ToString().ToUpper() == txtSettleSupplier.Text.Trim().Split('/')[0].ToString().ToUpper())
                    {
                        Session["CustomerAgeingId"] = otherAccountId;
                        return;
                    }

                }
            }
        }
        catch { }
        DisplayMessage("Enter " + lblSettleSupplier.Text + " Name");
        txtSettleSupplier.Text = "";
        GVSettleMentDetail.DataSource = null;
        GVSettleMentDetail.DataBind();
        GVSettleMentDebit.DataSource = null;
        GVSettleMentDebit.DataBind();
        txtSettleSupplier.Focus();

        //if (txtSettleSupplier.Text != "")
        //{
        //    try
        //    {
        //        txtSettleSupplier.Text.Split('/')[1].ToString();
        //    }
        //    catch
        //    {
        //        DisplayMessage("Enter " + lblSettleSupplier.Text + " Name");
        //        txtSettleSupplier.Text = "";
        //        GVSettleMentDetail.DataSource = null;
        //        GVSettleMentDetail.DataBind();
        //        GVSettleMentDebit.DataSource = null;
        //        GVSettleMentDebit.DataBind();

        //        txtSettleSupplier.Focus();
        //        return;
        //    }

        //    DataTable dt = ObjContactMaster.GetContactByContactName(txtSettleSupplier.Text.Trim().Split('/')[0].ToString());
        //    if (dt.Rows.Count == 0)
        //    {
        //        DisplayMessage("Select " + lblSettleSupplier.Text + " Name");
        //        txtSettleSupplier.Text = "";
        //        GVSettleMentDetail.DataSource = null;
        //        GVSettleMentDetail.DataBind();
        //        GVSettleMentDebit.DataSource = null;
        //        GVSettleMentDebit.DataBind();

        //        txtSettleSupplier.Focus();
        //    }
        //    else
        //    {
        //        string strSupplierId = txtSettleSupplier.Text.Trim().Split('/')[1].ToString();
        //        if (strSupplierId != "0" && strSupplierId != "")
        //        {

        //            Session["CustomerAgeingId"] = strSupplierId;
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Select " + lblSettleSupplier.Text + " Name");
        //    GVSettleMentDetail.DataSource = null;
        //    GVSettleMentDetail.DataBind();
        //    GVSettleMentDebit.DataSource = null;
        //    GVSettleMentDebit.DataBind();

        //    txtSettleSupplier.Focus();
        //}
    }
    protected void btnSettleSupplierAdd_OnClick(object sender, EventArgs e)
    {

        if (txtSettleSupplier.Text == "")
        {
            DisplayMessage("Enter " + lblSettleSupplier.Text + " Name");
            txtSettleSupplier.Focus();
            return;
        }
        
        if (string.IsNullOrEmpty(txtAgeingInvoiceNo.Text) && string.IsNullOrEmpty(txtAgeingVoucherNo.Text))
        {
            DisplayMessage("Enter Invoice or Voucher No");
            txtAgeingInvoiceNo.Focus();
            return;
        }

        int voucherId = 0;
        if (!string.IsNullOrEmpty(txtAgeingVoucherNo.Text))
        {
            int.TryParse(txtAgeingVoucherNo.Text.Split('/')[1].ToString(), out voucherId);
        }

        DataTable dtVoucherdetail = new DataTable();

        string strLocationId = Session["LocId"].ToString();
        string strCurrencyId = Session["LocCurrencyId"].ToString();
        try
        {
            HiddenField hdnCurrency = (HiddenField)this.Parent.FindControl("hdnCurrencyId");
            HiddenField hdnLocation = (HiddenField)this.Parent.FindControl("hdnLocId");
            strCurrencyId = hdnCurrency.Value;
            strLocationId = hdnLocation.Value;
        }
        catch (Exception ex)
        {

        }

        dtVoucherdetail = objAgeingDetail.GetAgeingDetailAllWithVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId);



        string strCondition = string.Empty;
        //supplier
        string whereClause = !string.IsNullOrEmpty(txtAgeingInvoiceNo.Text)?"Invoice_No = '" + txtAgeingInvoiceNo.Text.Split('/')[0].ToString() + "'" : "Voucher_No = '" + txtAgeingVoucherNo.Text.Split('/')[0].ToString() + "'";
        if (Session["ParentPageId"].ToString() == "2")
        {
            dtVoucherdetail = new DataView(dtVoucherdetail, "Other_Account_No='" + txtSettleSupplier.Text.Split('/')[1].ToString() + "' and AgeingType='PV' and " + whereClause + " and Paid_receive_Amount>0", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtVoucherdetail = new DataView(dtVoucherdetail, "Other_Account_No='" + txtSettleSupplier.Text.Split('/')[1].ToString() + "' and AgeingType='RV' and " + whereClause + " and Paid_receive_Amount>0", "", DataViewRowState.CurrentRows).ToTable();

        }

        GVSettleMentDetail.DataSource = dtVoucherdetail;
        GVSettleMentDetail.DataBind();
        

        foreach (GridViewRow gvr in GVSettleMentDetail.Rows)
        {
            //((TextBox)gvr.FindControl("txtSettleAmount")).Enabled = false;

            Label lblBalanceAmt = (Label)gvr.FindControl("lblBalanceAmount");

            lblBalanceAmt.Text = SetDecimal(lblBalanceAmt.Text);

        }

        if (txtSettleSupplier.Text != "")
        {
            DataTable dtDetail = new DataTable();
            string strVoucherType = string.Empty;
            if (((HiddenField)Parent.FindControl("hdnPageType")).Value == "Customer")
            {
                strVoucherType = "RV";
            }
            else
            {
                strVoucherType = "PV";
            }
            string strInvoiceNo = string.Empty;
            if (!string.IsNullOrEmpty(txtAgeingInvoiceNo.Text))
            {
                strInvoiceNo = txtAgeingInvoiceNo.Text.Split('/')[0].ToString();
            }
            dtDetail = objAgeingDetail.getPaidAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strVoucherType, txtSettleSupplier.Text.Split('/')[1].ToString(),voucherId.ToString(), strInvoiceNo);

            //if (dtDetail.Rows.Count > 0)
            //{
            //    dtDetail = new DataView(dtDetail, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //}

            if (dtDetail.Rows.Count > 0)
            {
                GVSettleMentDebit.DataSource = dtDetail;
                GVSettleMentDebit.DataBind();

                //ddlForeginCurrency.SelectedValue = dtDetail.Rows[0]["currency_id"].ToString();
                //ddlForeginCurrency_SelectedIndexChanged(sender, e);
                foreach (GridViewRow gvr in GVSettleMentDebit.Rows)
                {
                    //((TextBox)gvr.FindControl("txtSettleAmount")).Enabled = false;
                    Label lblInvAmt = (Label)gvr.FindControl("lblinvamount");
                    Label lblBalanceAmt = (Label)gvr.FindControl("lblBalanceAmount");
                    lblInvAmt.Text = SetDecimal(lblInvAmt.Text);
                    lblBalanceAmt.Text = SetDecimal(lblBalanceAmt.Text);

                }
            }
            else
            {
                GVSettleMentDebit.DataSource = null;
                GVSettleMentDebit.DataBind();
                if (!string.IsNullOrEmpty(txtAgeingInvoiceNo.Text))
                {
                    DisplayMessage("No Record Available");
                }
                ((DropDownList)Parent.FindControl("ddlForeginCurrency")).SelectedIndex = 0;
            }
        }
        else
        {
            DisplayMessage("Fill Name");
            txtSettleSupplier.Focus();
        }
    }


    #endregion


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {


        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        Set_CustomerMaster ObjCustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());
        //for customer

        DataTable dtFilter = new DataTable();
        if (HttpContext.Current.Session["ParentPageId"].ToString() == "1")
        {
            dtFilter = ObjCustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


            string filtertext = "Name like '%" + prefixText + "%'";
            dtFilter = new DataView(dtFilter, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();
        }
        ///supplier
        if (HttpContext.Current.Session["ParentPageId"].ToString() == "2")
        {

            dtFilter = ObjSupplier.GetAutoCompleteSupplierAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);



        }



        string[] filterlist = new string[dtFilter.Rows.Count];
        if (dtFilter.Rows.Count > 0)
        {
            for (int i = 0; i < dtFilter.Rows.Count; i++)
            {
                if (HttpContext.Current.Session["ParentPageId"].ToString() == "1")
                {
                    filterlist[i] = dtFilter.Rows[i]["Name"].ToString() + "/" + dtFilter.Rows[i]["Customer_Id"].ToString();
                }
                if (HttpContext.Current.Session["ParentPageId"].ToString() == "2")
                {
                    filterlist[i] = dtFilter.Rows[i]["Name"].ToString() + "/" + dtFilter.Rows[i]["Supplier_Id"].ToString();
                }
            }
        }
        return filterlist;
    }



    #region Invoice

    protected void txtAgeingInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (txtAgeingInvoiceNo.Text != "")
        {
            try
            {
                hdnAgeingInvoiceNo.Value = txtAgeingInvoiceNo.Text.Trim().Split('/')[0].ToString();
                hdnAgeingInvoiceId.Value = txtAgeingInvoiceNo.Text.Trim().Split('/')[1].ToString();
            }
            catch
            {

                hdnAgeingInvoiceNo.Value = "0";
                hdnAgeingInvoiceId.Value = "0";
                DisplayMessage("Fill Invoice Number");
                txtAgeingInvoiceNo.Focus();
                return;
            }
        }
        else
        {
            hdnAgeingInvoiceNo.Value = "0";
            hdnAgeingInvoiceId.Value = "0";
            DisplayMessage("Fill Invoice Number");
            txtAgeingInvoiceNo.Focus();
            return;
        }
    }


    #endregion

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string strLocationId = Session["LocId"].ToString();
        try
        {
            HiddenField hdnLocation = (HiddenField)this.Parent.FindControl("hdnLocId");
            strLocationId = hdnLocation.Value;
        }
        catch (Exception ex)
        {

        }
        string strsql = "Update Ac_Ageing_Detail Set IsActive = 'False' ,ModifiedBy='" + Session["userId"].ToString() + "' ,ModifiedDate='" + DateTime.Now.ToString() + "' Where [Company_Id] = '" + Session["CompId"].ToString() + "' and [Brand_Id] = '" + Session["BrandId"].ToString() + "' and [Location_Id]='" + strLocationId + "' and [trans_Id]='" + e.CommandArgument.ToString() + "'";
        da.execute_Command(strsql);
        btnSettleSupplierAdd_OnClick(null, null);
    }


    protected void txtAgeingVoucherNo_TextChanged(object sender, EventArgs e)
    {
        if (txtAgeingVoucherNo.Text != "")
        {
            try
            {
                hdnAgeingVoucherId.Value = txtAgeingVoucherNo.Text.Trim().Split('/')[0].ToString();
                hdnAgeingVoucherNo.Value = txtAgeingVoucherNo.Text.Trim().Split('/')[1].ToString();
            }
            catch
            {

                hdnAgeingVoucherId.Value = "0";
                hdnAgeingVoucherNo.Value = "0";
                DisplayMessage("Fill Voucher Number");
                txtAgeingVoucherNo.Focus();
                return;
            }
        }
        else
        {
            hdnAgeingVoucherId.Value = "0";
            hdnAgeingVoucherNo.Value = "0";
            DisplayMessage("Fill Voucher Number");
            txtAgeingVoucherNo.Focus();
            return;
        }
    }
}