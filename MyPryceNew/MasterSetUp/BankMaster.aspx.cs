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
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;


public partial class MasterSetUp_BankMaster : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    Set_BankMaster objBank = null;
    Set_AddressMaster AM = null;
    Set_AddressCategory ObjAddressCat = null;
    Set_AddressChild objAddChild = null;
    CountryMaster ObjCountry = null;
    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    Ac_ChartOfAccount objCOA = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objBank = new Set_BankMaster(Session["DBConnection"].ToString());
        AM = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjCountry = new CountryMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/BankMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);


            txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;
            FillGridBin();
            FillGrid();

            btnList_Click(null, null);
            FillCountryCode();
        }
      
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }


    #region CountryCallingCode
    public void FillCountryCode()
    {
        try
        {
            string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

            ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
            ViewState["CountryCode"] = ObjCountry.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        }
        catch
        {

        }
    }
    #endregion
   
    protected void btnList_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        txtValue.Focus();
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //PnlList.Visible = true;
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = false;

        Lbl_Tab_New.Text = Resources.Attendance.New;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtBankCode.Focus();
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //PnlList.Visible = false;
        //PnlNewEdit.Visible = true;
        //PnlBin.Visible = false;
       
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = true;
        //PnlList.Visible = false;
        FillGridBin();
       
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        
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
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["Bank"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Bank__Master"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvBankMaster, view.ToTable(), "", "");
          
        }
        txtValue.Focus();
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
            DataTable dtCust = (DataTable)Session["dtbinBank"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvBankMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
              //  imgBtnRestore.Visible = false;
              //  ImgbtnSelectAll.Visible = false;
            }
            else
            {
              
            }
        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
       
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvBankMasterBin.Rows)
        {
            index = (int)gvBankMasterBin.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValuesemplog()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvBankMasterBin.Rows)
            {
                int index = (int)gvBankMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvBankMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvBankMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBankMasterBin, dt, "", "");
       
        PopulateCheckedValuesemplog();
    }
    protected void gvBankMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objBank.GetBankMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBankMasterBin, dt, "", "");
      
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());

        int b = 0;

        //if (txtBankCode.Text == "")
        //{
        //    DisplayMessage("Enter Bank Code");
        //    txtBankCode.Focus();
        //    return;
        //}

        if (txtBankName.Text == "")
        {
            DisplayMessage("Enter Bank Name");
            txtBankName.Focus();
            return;
        }

        //if (txtAccountName.Text == "")
        //{
        //    DisplayMessage("Fill Account Name");
        //    txtAccountName.Focus();
        //    return;
        //}
        //else
        //{
        //    if (GetAccountId() == "")
        //    {
        //        DisplayMessage("Please Choose Account Name In Suggestions Only");
        //        txtAccountName.Text = "";
        //        txtAccountName.Focus();
        //        return;
        //    }
        //}




        //added rollback transaction by nawaz ahmed and jitendra
        //added on 11-08-2016


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {

            if (editid.Value == "")
            {
                //DataTable dt = objBank.GetBankMaster(ref trns);

                //dt = new DataView(dt, "Bank_Code='" + txtBankCode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                //if (dt.Rows.Count > 0)
                //{
                //    DisplayMessage("Bank Code Already Exists");
                //    txtBankCode.Focus();
                //    return;

                //}
                DataTable dt1 = objBank.GetBankMaster(ref trns);

                dt1 = new DataView(dt1, "Bank_Name='" + txtBankName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    DisplayMessage("Bank Name Already Exists");
                    txtBankName.Focus();
                    return;
                }
                b = objBank.InsertBankMaster(txtBankCode.Text, txtBankName.Text, txtBankNameL.Text, "", GetAccountId(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    string strMaxId = string.Empty;
                    strMaxId = b.ToString();
                    bool Isdefault = false;
                    foreach (GridViewRow gvr in GvAddressName.Rows)
                    {
                        if (((CheckBox)gvr.FindControl("chkdefault")).Checked == true)
                        {
                            Isdefault = true;
                            break;
                        }

                    }

                    foreach (GridViewRow gvr in GvAddressName.Rows)
                    {
                        Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");
                        CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
                        if (GvAddressName.Rows.Count == 1)
                        {
                            chk.Checked = true;
                        }
                        else
                        {
                            if (Isdefault == false)
                            {
                                if (gvr.RowIndex == 0)
                                {
                                    chk.Checked = true;
                                }

                            }
                        }

                        if (lblGvAddressName.Text != "")
                        {
                            DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text, ref trns);
                            if (dtAddId.Rows.Count > 0)
                            {
                                string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                                objAddChild.InsertAddressChild(strAddressId, "Bank", strMaxId, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                    }

                    DisplayMessage("Record Saved","green");
                    btnList_Click(null, null);

                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }
            }
            else
            {

                //DataTable dt = objBank.GetBankMaster(ref trns);

                DataTable dt2 = objBank.GetBankMasterById(editid.Value, ref trns);
                string BankName = string.Empty;
                string BankCode = string.Empty;
                if (dt2.Rows.Count > 0)
                {
                    BankName = dt2.Rows[0]["Bank_Name"].ToString();
                    BankCode = dt2.Rows[0]["Bank_Code"].ToString();

                }

                //try
                //{
                //    BankCode = (new DataView(dt, "Bank_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Bank_Code"].ToString();
                //}
                //catch
                //{
                //    BankCode = "";
                //}

                //dt = new DataView(dt, "Bank_Code='" + txtBankCode.Text + "' and Bank_Code<>'" + BankCode + "'", "", DataViewRowState.CurrentRows).ToTable();

                //if (dt.Rows.Count > 0)
                //{
                //    DisplayMessage("Bank Code Already Exists");
                //    txtBankCode.Focus();
                //    return;
                //}
                DataTable dt1 = objBank.GetBankMaster(ref trns);

                try
                {
                    BankName = (new DataView(dt1, "Bank_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Bank_Name"].ToString();
                }
                catch
                {
                    BankName = "";
                }

                dt1 = new DataView(dt1, "Bank_Name='" + txtBankName.Text + "' and Bank_Name<>'" + BankName + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    DisplayMessage("Bank Name Already Exists");
                    txtBankName.Focus();
                    return;

                }

                b = objBank.UpdateBankMaster(editid.Value, txtBankCode.Text, "", txtBankName.Text, txtBankNameL.Text, GetAccountId(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (b != 0)
                {
                    objAddChild.DeleteAddressChild("Bank", editid.Value, ref trns);
                    bool Isdefault = false;
                    foreach (GridViewRow gvr in GvAddressName.Rows)
                    {
                        if (((CheckBox)gvr.FindControl("chkdefault")).Checked == true)
                        {
                            Isdefault = true;
                            break;
                        }
                    }

                    foreach (GridViewRow gvr in GvAddressName.Rows)
                    {
                        Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");
                        CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
                        if (GvAddressName.Rows.Count == 1)
                        {
                            chk.Checked = true;
                        }
                        else
                        {
                            if (Isdefault == false)
                            {
                                if (gvr.RowIndex == 0)
                                {
                                    chk.Checked = true;
                                }
                            }
                        }

                        if (lblGvAddressName.Text != "")
                        {
                            DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text, ref trns);
                            if (dtAddId.Rows.Count > 0)
                            {
                                string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                                objAddChild.InsertAddressChild(strAddressId, "Bank", editid.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                    }
                    btnList_Click(null, null);
                    DisplayMessage("Record Updated", "green");

                }
                else
                {
                    DisplayMessage("Record Not Updated");
                }
            }


            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
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
    private string GetAccountId()
    {
        string retval = string.Empty;
        if (txtAccountName.Text != "")
        {
            retval = (txtAccountName.Text.Split('/'))[txtAccountName.Text.Split('/').Length - 1];

            DataTable dtCOA = objCOA.GetCOAByTransId(Session["CompId"].ToString(), retval);
            if (dtCOA.Rows.Count > 0)
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
        return retval;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();

        DataTable dt = objBank.GetBankMasterById(editid.Value);
        if (dt.Rows.Count > 0)
        {
            txtBankCode.Text = dt.Rows[0]["Bank_Code"].ToString();
            txtBankName.Text = dt.Rows[0]["Bank_Name"].ToString();
            txtBankNameL.Text = dt.Rows[0]["Bank_Name_L"].ToString();

            string strAccountId = dt.Rows[0]["Field1"].ToString();
            DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
            if (dtAccount.Rows.Count > 0)
            {
                txtAccountName.Text = dtAccount.Rows[0]["AccountName"].ToString() + "/" + strAccountId;
            }

            // txtAddress.Text = dt.Rows[0]["Address"].ToString();

            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Bank", editid.Value);
            if (dtChild.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)GvAddressName, dtChild, "", "");

                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    Label lblAddressName = (Label)gvr.FindControl("lblgvAddressName");
                    Label lblAddress = (Label)gvr.FindControl("lblgvAddress");
                    lblAddress.Text = GetAddressByAddressName(lblAddressName.Text);
                }
            }

            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
        }
     


    }
    protected void txtAccountName_TextChanged(object sender, EventArgs e)
    {
        if (txtAccountName.Text != "")
        {
            DataTable dtAccountName = objCOA.GetCOAAll(Session["CompId"].ToString());
            string retval = string.Empty;
            if (txtAccountName.Text != "")
            {
                string strAccountName = txtAccountName.Text.Trim().Split('/')[0].ToString();
                dtAccountName = new DataView(dtAccountName, "AccountName='" + strAccountName + "' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccountName.Rows.Count > 0)
                {
                    retval = (txtAccountName.Text.Split('/'))[txtAccountName.Text.Split('/').Length - 1];
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
                string strTransId = GetAccountId();
                DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strTransId);
                if (dtAccount.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
                else
                {
                    txtAccountName.Text = "";
                    DisplayMessage("Choose In Suggestions Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
                    return;
                }
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAccountName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            }
        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable BankDt = new DataTable();
        BankDt = objBank.CheckBankExistance(e.CommandArgument.ToString(), "1");
        if (BankDt.Rows.Count > 0)
        {
            DisplayMessage("Record In Use");
            return;
        }
        else
        {
            BankDt = objBank.CheckBankExistance(e.CommandArgument.ToString(), "2");
            if (BankDt.Rows.Count > 0)
            {
                DisplayMessage("Record In Use");
                return;
            }
            else
            {
                b = objBank.DeleteBankMaster(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        }
       
    }
    protected void gvBankMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBankMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Bank__Master"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBankMaster, dt, "", "");

       
    }
    protected void gvBankMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Bank__Master"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Bank__Master"] = dt;
        gvBankMaster.DataSource = dt;
        gvBankMaster.DataBind();
        
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListBankCode(string prefixText, int count, string contextKey)
    {
        Set_BankMaster objBankMaster = new Set_BankMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objBankMaster.GetBankMasterBoth(), "Bank_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Bank_Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListBankName(string prefixText, int count, string contextKey)
    {
        Set_BankMaster objBankMaster = new Set_BankMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objBankMaster.GetBankMasterBoth(), "Bank_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Bank_Name"].ToString();
        }
        return txt;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        btnList_Click(null, null);
       
    }
    public void FillGrid()
    {
        DataTable dt = objBank.GetBankMaster();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBankMaster, dt, "", "");
        Session["dtFilter_Bank__Master"] = dt;
        Session["Bank"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objBank.GetBankMasterInactive();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBankMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinBank"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
           // imgBtnRestore.Visible = false;
           // ImgbtnSelectAll.Visible = false;
        }
        else
        {
        
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount objCOA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

        string filtertext = "AccountName like '" + prefixText + "%'";
        dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                txt = null;
            }
            else
            {
                dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dtCOA.Rows.Count > 0)
                {
                    txt = new string[dtCOA.Rows.Count];
                    for (int i = 0; i < dtCOA.Rows.Count; i++)
                    {
                        txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return txt;
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)gvBankMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvBankMasterBin.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Bank_Id"]))
                {
                    userdetails.Add(dr["Bank_Id"]);
                }
            }
            foreach (GridViewRow GR in gvBankMasterBin.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvBankMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
       
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvBankMasterBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = objBank.DeleteBankMaster(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

                        }
                    }
                }

                if (b != 0)
                {

                    FillGrid();
                    FillGridBin();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CHECKED_ITEMS"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in gvBankMasterBin.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
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
            else
            {
                DisplayMessage("Please Select Record");
                gvBankMasterBin.Focus();
                return;
            }
        }
       
    }


    public void Reset()
    {
        txtBankCode.Text = "";
        txtBankName.Text = "";
        txtBankNameL.Text = "";
        txtAccountName.Text = "";
        Session["CHECKED_ITEMS"] = null;
        txtAddressName.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
       // GvAddressName.DataSource = null;
       // GvAddressName.DataBind();

    }

    #region Add AddressName Concept
    protected void chkgvSelect_CheckedChangedDefault(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GvAddressName.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
            chk.Checked = false;
        }

        CheckBox chk1 = (CheckBox)sender;
        chk1.Checked = true;


    }
    protected void txtAddressName_TextChanged(object sender, EventArgs e)
    {
        if (txtAddressName.Text != "")
        {
            DataTable dtAM = AM.GetAddressDataByAddressName(txtAddressName.Text);
            if (dtAM.Rows.Count > 0)
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(imgAddAddressName);
            }
            else
            {
                txtAddressName.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
        }
    }
    protected void imgAddAddressName_Click(object sender, EventArgs e)
    {
        if (txtAddressName.Text != "")
        {
            string strA = "0";
            foreach (GridViewRow gve in GvAddressName.Rows)
            {
                Label lblCAddressName = (Label)gve.FindControl("lblgvAddressName");
                if (txtAddressName.Text == lblCAddressName.Text)
                {
                    strA = "1";
                }
            }


            if (hdnAddressId.Value == "")
            {
                if (strA == "0")
                {
                    FillAddressChidGird("Save");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
                else
                {
                    txtAddressName.Text = "";
                    DisplayMessage("Address Name Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
            }
            else
            {
                if (txtAddressName.Text == hdnAddressName.Value)
                {
                    FillAddressChidGird("Edit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
                else
                {
                    if (strA == "0")
                    {
                        FillAddressChidGird("Edit");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                    }
                    else
                    {
                        txtAddressName.Text = "";
                        DisplayMessage("Address Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
        }
        txtAddressName.Focus();
    }
    public void ResetAddressName()
    {
        txtAddressName.Text = "";
        hdnAddressId.Value = "";
        hdnAddressName.Value = "";
    }
    public DataTable CreateAddressDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Address_Name");
        dt.Columns.Add("Address");
        dt.Columns.Add("Is_Default", typeof(bool));
        return dt;
    }
    public DataTable FillAddressDataTabel()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateAddressDataTable();
        if (GvAddressName.Rows.Count > 0)
        {
            for (int i = 0; i < GvAddressName.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvAddressName.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
                    Label lblAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");

                    dt.Rows[i]["Trans_Id"] = lblSNo.Text;
                    strNewSNo = lblSNo.Text;
                    dt.Rows[i]["Address_Name"] = lblAddressName.Text;
                    try
                    {
                        dt.Rows[i]["Address"] = GetAddressByAddressName(lblAddressName.Text);
                    }
                    catch
                    {

                    }
                    dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;

                }
                else
                {
                    dt.Rows.Add(i);
                    dt.Rows[i]["Trans_Id"] = (float.Parse(strNewSNo) + 1).ToString();
                    dt.Rows[i]["Address_Name"] = txtAddressName.Text;
                    try
                    {
                        dt.Rows[i]["Address"] = GetAddressByAddressName(txtAddressName.Text);
                    }
                    catch
                    {

                    }
                    dt.Rows[i]["Is_Default"] = false.ToString();

                }
            }
        }
        else
        {
            dt.Rows.Add(0);
            dt.Rows[0]["Trans_Id"] = "1";
            dt.Rows[0]["Address_Name"] = txtAddressName.Text;
            try
            {
                dt.Rows[0]["Address"] = GetAddressByAddressName(txtAddressName.Text);
            }
            catch
            {

            }
            dt.Rows[0]["Is_Default"] = false.ToString();

        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressName, dt, "", "");
        }
        return dt;
    }

    public string GetAddressByAddressName(string AddressName)
    {
        string Address = string.Empty;
        DataTable dt = AM.GetAddressDataByAddressName(AddressName);

        if (dt.Rows.Count > 0)
        {

            Address = dt.Rows[0]["Address"].ToString();

            if (dt.Rows[0]["Street"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["Street"].ToString();

            }
            if (dt.Rows[0]["Block"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["Block"].ToString();

            }
            if (dt.Rows[0]["Avenue"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["Avenue"].ToString();

            }
            if (dt.Rows[0]["CityId"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["CityId"].ToString();

            }
            if (dt.Rows[0]["StateId"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["StateId"].ToString();

            }
            if (dt.Rows[0]["Country_Name"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["Country_Name"].ToString();

            }
            if (dt.Rows[0]["PinCode"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["PinCode"].ToString();

            }


        }


        return Address;
    }
    public string GetContactEmailId(string AddressName)
    {
        string ContactEmailId = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {

            if (DtAddress.Rows[0]["EmailId1"].ToString() != "")
            {
                ContactEmailId = DtAddress.Rows[0]["EmailId1"].ToString();
            }
        }
        return ContactEmailId;
    }
    public string GetContactFaxNo(string AddressName)
    {
        string ContactFaxNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {

            if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            {
                ContactFaxNo = DtAddress.Rows[0]["FaxNo"].ToString();
            }
        }
        return ContactFaxNo;
    }
    public string GetContactPhoneNo(string AddressName)
    {
        string ContactPhoneNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["PhoneNo1"].ToString() != "")
            {
                ContactPhoneNo = DtAddress.Rows[0]["PhoneNo1"].ToString();
            }
            if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
            {
                if (ContactPhoneNo != "")
                {
                    ContactPhoneNo = ContactPhoneNo + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
                }
                else
                {
                    ContactPhoneNo = DtAddress.Rows[0]["PhoneNo2"].ToString();
                }
            }


        }
        return ContactPhoneNo;
    }
    public string GetContactMobileNo(string AddressName)
    {
        string ContactPhoneNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
            {
                ContactPhoneNo = DtAddress.Rows[0]["MobileNo1"].ToString();
            }
            if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
            {
                if (ContactPhoneNo != "")
                {
                    ContactPhoneNo = ContactPhoneNo + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
                }
                else
                {
                    ContactPhoneNo = DtAddress.Rows[0]["MobileNo2"].ToString();
                }
            }
        }
        return ContactPhoneNo;
    }


    public DataTable FillAddressDataTabelDelete()
    {
        DataTable dt = CreateAddressDataTable();
        for (int i = 0; i < GvAddressName.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");


            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;

            dt.Rows[i]["Address"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;

        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id<>'" + hdnAddressId.Value + "'";
        dt = (DataTable)dv.ToTable();
        return dt;
    }
    protected void btnAddressEdit_Command(object sender, CommandEventArgs e)
    {
        hdnAddressId.Value = e.CommandArgument.ToString();
        FillAddressDataTabelEdit();
    }
    public DataTable FillAddressDataTabelEdit()
    {
        DataTable dt = CreateAddressDataTable();

        for (int i = 0; i < GvAddressName.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");

            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;
            dt.Rows[i]["Address"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;

        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id='" + hdnAddressId.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {
            txtAddressName.Text = dt.Rows[0]["Address_Name"].ToString();
            hdnAddressName.Value = dt.Rows[0]["Address_Name"].ToString();

        }
        return dt;
    }
    protected void btnAddressDelete_Command(object sender, CommandEventArgs e)
    {
        hdnAddressId.Value = e.CommandArgument.ToString();
        FillAddressChidGird("Del");
    }
    public void FillAddressChidGird(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillAddressDataTabelDelete();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillAddressDataTableUpdate();
        }
        else
        {
            dt = FillAddressDataTabel();
        }
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressName, dt, "", "");

        ResetAddressName();
    }
    public DataTable FillAddressDataTableUpdate()
    {
        DataTable dt = CreateAddressDataTable();
        for (int i = 0; i < GvAddressName.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");

            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;

            dt.Rows[i]["Address"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnAddressId.Value == dt.Rows[i]["Trans_Id"].ToString())
            {
                dt.Rows[i]["Address_Name"] = txtAddressName.Text;
                dt.Rows[i]["Address"] = GetAddressByAddressName(txtAddressName.Text);
            }
        }
        return dt;
    }
    #endregion

    #region Add New Address Concept
    protected void btnAddNewAddress_Click(object sender, EventArgs e)
    {
        pnlAddress1.Visible = true;
        pnlAddress2.Visible = true;
    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        pnlAddress1.Visible = false;
        pnlAddress2.Visible = false;
    }
    #endregion

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.GetDistinctAddressName(prefixText);

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Address_Name"].ToString();
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
                dt = AddressN.GetAddressAllData(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Address_Name"].ToString();
                    }
                }
            }
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListNewAddress(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster Address = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Address.GetDistinctAddressName(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Address_Name"].ToString();
        }
        return str;
    }


    protected void txtBankCode_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objBank.GetBankMaster();
            dt = new DataView(dt, "Bank_Code='" + txtBankCode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                txtBankCode.Text = "";
                DisplayMessage("Bank Code is Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankCode);
                return;
            }
            DataTable dt1 = objBank.GetBankMasterInactive();
            dt1 = new DataView(dt1, "Bank_Code='" + txtBankCode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtBankCode.Text = "";
                DisplayMessage("Bank Code is Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankCode);
                return;
            }
        }
        else
        {
            DataTable dt = objBank.GetBankMaster();

            DataTable dt2 = objBank.GetBankMasterById(editid.Value);
            string BankCode = string.Empty;
            try
            {
                BankCode = (new DataView(dt, "Bank_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Bank_Code"].ToString();
            }
            catch
            {
                BankCode = "";
            }

            dt = new DataView(dt, "Bank_Code='" + txtBankCode.Text + "' and Bank_Code<>'" + BankCode + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Bank Code Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankCode);
                return;
            }

            DataTable dt1 = objBank.GetBankMasterInactive();
            dt1 = new DataView(dt1, "Bank_Code='" + txtBankCode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Bank code Already Exists. - Go To Bin.");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankCode);
                return;
            }
        }
       
    }

    protected void txtBankName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objBank.GetBankMaster();
            dt = new DataView(dt, "Bank_Name='" + txtBankName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                txtBankName.Text = "";
                DisplayMessage("Bank Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankName);
                return;
            }
            DataTable dt1 = objBank.GetBankMasterInactive();
            dt1 = new DataView(dt1, "Bank_Name='" + txtBankName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtBankName.Text = "";
                DisplayMessage("Bank Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankCode);
                return;
            }
        }
        else
        {
            DataTable dt = objBank.GetBankMaster();

            DataTable dt2 = objBank.GetBankMasterById(editid.Value);
            string BankName = string.Empty;
            if (dt2.Rows.Count > 0)
            {
                BankName = dt2.Rows[0]["Bank_Name"].ToString();
            }

            try
            {
                BankName = (new DataView(dt, "Bank_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Bank_Name"].ToString();
            }
            catch
            {
                BankName = "";
            }

            dt = new DataView(dt, "Bank_Name='" + txtBankName.Text + "' and Bank_Name<>'" + BankName + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                txtBankName.Text = "";
                DisplayMessage("Bank Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankName.Text);
                return;
            }

            DataTable dt1 = objBank.GetBankMasterInactive();
            dt1 = new DataView(dt1, "Bank_Name='" + txtBankName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtBankName.Text = "";
                DisplayMessage("Bank Name Already Exists - Go To Bin.");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankName);
            }
        }
     
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListStateName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["AddCtrl_Country_Id"].ToString() == "")
        {
            return null;
        }
        StateMaster objStateMaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objStateMaster.GetAllStateByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_Country_Id"].ToString());
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["State_Name"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCityName(string prefixText, int count, string contextKey)
    {
        try
        {
            if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "" || HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "0")
            {
                return null;
            }
            CityMaster objCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objCityMaster.GetAllCityByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_State_Id"].ToString());
            string[] txt = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["City_Name"].ToString();
            }
            return txt;
        }
        catch
        {
            return null;
        }
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
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void resetAddress()
    {
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ddlCountry_IndexChanged(string CountryId)
    {
        CountryMaster ObjSysCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
        return "+" + ObjSysCountryMaster.GetCountryMasterById(CountryId).Rows[0]["Country_Code"].ToString();
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCity_TextChanged(string stateId, string cityName)
    {
        CityMaster ObjCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());

        string City_id = ObjCityMaster.GetCityIdFromStateIdNCityName(stateId, cityName);

        if (City_id != "")
        {
            return City_id;
        }
        else
        {
            return "0";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtState_TextChanged(string CountryId, string StateName)
    {
        StateMaster ObjStatemaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string stateId = ObjStatemaster.GetStateIdFromCountryIdNStateName(CountryId, StateName);
        if (stateId != "")
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = stateId;
            return stateId;
        }
        else
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
            return "0";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static int txtAddressNameNew_TextChanged(string AddressName, string addressId)
    {
        // return  1 when 'Address Name Already Exists' and 0 when not present
        Set_AddressMaster AM = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string data = AM.GetAddressDataExistOrNot(AddressName, addressId);
        if (data == "0")
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetDistinctEmailId(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        return str;
    }
}
