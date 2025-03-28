using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Data.SqlClient;
public partial class SystemSetUp_AddressMaster : BasePage
{
    #region defind Class Object
    Common cmn = null;
    Set_AddressMaster objAddressM = null;
    Set_AddressCategory ObjAddressCat = null;
    SystemParameter ObjSysParam = null;
    IT_ObjectEntry objObjectEntry = null;
    CountryMaster ObjCountry = null;
    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objAddressM = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjCountry = new CountryMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../systemSetup/addressmaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            Session["AddCtrl_Country_Id"] = "";
            Session["AddCtrl_State_Id"] = "";
            ddlOption.SelectedIndex = 2;
            FillGridBin();
            FillGrid();
            FillAddressCategory();
            FillCountry();
            Session["CHECKED_ITEMS"] = null;
            Session["Add"] = null;
            Session["Long"] = null;
            Session["Lati"] = null;
            FillCountryCode();
        }

    }
    public void FillCountryCode()
    {
        try
        {
            string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
            ddlCountry.SelectedValue = ViewState["Country_Id"].ToString();
            ViewState["CountryCode"] = ObjCountry.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
            txtCountryCode_Phoneno1.Text = "+" + ViewState["CountryCode"].ToString();
            txtCountryCode_Phoneno2.Text = "+" + ViewState["CountryCode"].ToString();
            txtCountryCode_MobileNo1.Text = "+" + ViewState["CountryCode"].ToString();
            txtCountryCode_MobileNo2.Text = "+" + ViewState["CountryCode"].ToString();
            Session["AddCtrl_Country_Id"] = ViewState["Country_Id"].ToString();
        }
        catch
        {
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnAddressSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    #region System defind Funcation    
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dtAddressEdit = objAddressM.GetAddressDataByTransId(editid.Value, Session["CompId"].ToString());
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
        if (dtAddressEdit.Rows.Count > 0)
        {
            ddlAddressCategory.SelectedValue = dtAddressEdit.Rows[0]["Address_Category_Id"].ToString();
            txtAddressName.Text = dtAddressEdit.Rows[0]["Address_Name"].ToString();
            txtAddress.Text = dtAddressEdit.Rows[0]["Address"].ToString();
            txtStreet.Text = dtAddressEdit.Rows[0]["Street"].ToString();
            txtBlock.Text = dtAddressEdit.Rows[0]["Block"].ToString();
            txtAvenue.Text = dtAddressEdit.Rows[0]["Avenue"].ToString();
            string strCountryId = dtAddressEdit.Rows[0]["CountryId"].ToString();
            Session["AddCtrl_Country_Id"] = strCountryId;
            hdnstateId.Value = dtAddressEdit.Rows[0]["State_Id"].ToString();
            Session["AddCtrl_State_Id"] = hdnstateId.Value;
            hdncityId.Value = dtAddressEdit.Rows[0]["City_Id"].ToString();
            if (strCountryId != "" && strCountryId != "0")
            {
                ddlCountry.SelectedValue = strCountryId;
            }
            else
            {
                FillCountry();
            }
            txtState.Text = dtAddressEdit.Rows[0]["State_Name"].ToString();
            txtCity.Text = dtAddressEdit.Rows[0]["City_Name"].ToString();
            txtPinCode.Text = dtAddressEdit.Rows[0]["PinCode"].ToString();
            txtPhoneNo1.Text = dtAddressEdit.Rows[0]["PhoneNo1"].ToString();
            txtCountryCode_Phoneno1.Text = dtAddressEdit.Rows[0]["Country_Code"].ToString();
            txtCountryCode_Phoneno2.Text = dtAddressEdit.Rows[0]["Country_Code"].ToString();
            txtCountryCode_MobileNo1.Text = dtAddressEdit.Rows[0]["Country_Code"].ToString();
            txtCountryCode_MobileNo2.Text = dtAddressEdit.Rows[0]["Country_Code"].ToString();
            try
            {
                string[] mobileNumber = dtAddressEdit.Rows[0]["PhoneNo1"].ToString().Split('-');
                if (mobileNumber.Length == 1)
                {
                    txtPhoneNo1.Text = mobileNumber[0].ToString();
                }
                else
                {
                    txtPhoneNo1.Text = mobileNumber[1].ToString();
                }
            }
            catch
            {
            }
            try
            {
                string[] mobileNumber = dtAddressEdit.Rows[0]["PhoneNo2"].ToString().Split('-');
                if (mobileNumber.Length == 1)
                {
                    txtPhoneNo2.Text = mobileNumber[0].ToString();
                }
                else
                {
                    txtPhoneNo2.Text = mobileNumber[1].ToString();
                }
            }
            catch
            {
            }
            try
            {
                string[] mobileNumber = dtAddressEdit.Rows[0]["MobileNo1"].ToString().Split('-');
                if (mobileNumber.Length == 1)
                {
                    txtMobileNo1.Text = mobileNumber[0].ToString();
                }
                else
                {
                    txtMobileNo1.Text = mobileNumber[1].ToString();
                }
            }
            catch
            {
            }
            try
            {
                string[] mobileNumber = dtAddressEdit.Rows[0]["MobileNo2"].ToString().Split('-');
                if (mobileNumber.Length == 1)
                {
                    txtMobileNo2.Text = mobileNumber[0].ToString();
                }
                else
                {
                    txtMobileNo2.Text = mobileNumber[1].ToString();
                }
            }
            catch
            {
            }
            txtEmailId1.Text = dtAddressEdit.Rows[0]["EmailId1"].ToString();
            txtEmailId2.Text = dtAddressEdit.Rows[0]["EmailId2"].ToString();
            txtFaxNo.Text = dtAddressEdit.Rows[0]["FaxNo"].ToString();
            txtWebsite.Text = dtAddressEdit.Rows[0]["WebSite"].ToString();
            txtLongitude.Text = dtAddressEdit.Rows[0]["Longitude"].ToString();
            txtLatitude.Text = dtAddressEdit.Rows[0]["Latitude"].ToString();
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlAddressCategory);
    }
    protected void GvAddress_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvAddress.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Address"];
        objPageCmn.FillData((object)GvAddress, dt, "", "");
    }
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
            objPageCmn.FillData((object)GvAddress, view.ToTable(), "", "");
            Session["dtFilter_Address"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
        txtValue.Focus();
    }
    protected void GvAddress_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Address"];
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
        Session["dtFilter_Address"] = dt;
        objPageCmn.FillData((object)GvAddress, dt, "", "");
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        DataTable dtCount = ObjAddressCat.CheckAddressExistance(editid.Value, "2");
        if (dtCount.Rows.Count > 0)
        {
            DisplayMessage("Record Used in other page, So it can not be deleted");
        }
        else
        {
            b = objAddressM.DeleteAddressMaster(Session["CompId"].ToString(), editid.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                DisplayMessage("Record Deleted");
            }
            else
            {
                DisplayMessage("Record  Not Deleted");
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

        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
    }
    protected void btnAddressCancel_Click(object sender, EventArgs e)
    {
        Reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlAddressCategory);
    }
    protected void btnGetLatLong_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        if (!txtLatitude.Text.Contains("0.0000") && !txtLatitude.Text.Contains("0.0000"))
        {
            string FullAddress = ddlCountry.SelectedItem.Text + "," + txtState.Text + "," + txtCity.Text + "," + txtAddress.Text + "," + txtPinCode.Text;
            Session["Add"] = FullAddress;
            Session["Long"] = txtLongitude.Text;
            Session["Lati"] = txtLatitude.Text;
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Additional_Info_Open()", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "nothing", "window.open('../SystemSetup/GoogleMap.aspx','window','width=1024')", true);
    }
    protected void btnAddressSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        if (ddlAddressCategory.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Address Category");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlAddressCategory);
            return;
        }
        if (txtAddressName.Text == "" || txtAddressName.Text == null)
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
            return;
        }
        if (txtAddress.Text == "" || txtAddress.Text == null)
        {
            DisplayMessage("Enter Address");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddress);
            return;
        }
        string strCountryId = string.Empty;
        if (ddlCountry.SelectedValue == "--Select--")
        {
            strCountryId = "0";
        }
        else
        {
            strCountryId = ddlCountry.SelectedValue;
        }
        if (txtEmailId1.Text != "")
        {
            if (!CheckEmailId(txtEmailId1.Text))
            {
                DisplayMessage("Email Id 1 is invalid");
                txtEmailId1.Text = "";
                txtEmailId1.Focus();
                return;
            }
        }
        if (txtEmailId2.Text != "")
        {
            if (!CheckEmailId(txtEmailId2.Text))
            {
                DisplayMessage("Email Id 2 is invalid");
                txtEmailId2.Text = "";
                txtEmailId2.Focus();
                return;
            }
        }
        string strLongitude = string.Empty;
        if (txtLongitude.Text != "")
        {
            strLongitude = txtLongitude.Text;
        }
        else
        {
            strLongitude = "0";
        }
        string strLatitude = string.Empty;
        if (txtLatitude.Text != "")
        {
            strLatitude = txtLatitude.Text;
        }
        else
        {
            strLatitude = "0";
        }
        string strPhoneNo1 = string.Empty;
        string strPhoneNo2 = string.Empty;
        string strMobile1 = string.Empty;
        string strMObile2 = string.Empty;
        if (txtPhoneNo1.Text != "")
        {
            strPhoneNo1 = txtCountryCode_Phoneno1.Text + "-" + txtPhoneNo1.Text;
        }
        if (txtPhoneNo2.Text != "")
        {
            strPhoneNo2 = txtCountryCode_Phoneno2.Text + "-" + txtPhoneNo2.Text;
        }
        if (txtMobileNo1.Text != "")
        {
            strMobile1 = txtCountryCode_MobileNo1.Text + "-" + txtMobileNo1.Text;
        }
        if (txtMobileNo2.Text != "")
        {
            strMObile2 = txtCountryCode_MobileNo2.Text + "-" + txtMobileNo2.Text;
        }
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            int b = 0;
            if (editid.Value != "")
            {
                b = objAddressM.UpdateAddressMaster(editid.Value, ddlAddressCategory.SelectedValue, txtAddressName.Text, txtAddress.Text, txtStreet.Text, txtBlock.Text, txtAvenue.Text, strCountryId, hdnstateId.Value, hdncityId.Value, txtPinCode.Text, strPhoneNo1.Trim(), strPhoneNo2.Trim(), strMobile1.Trim(), strMObile2.Trim(), txtEmailId1.Text, txtEmailId2.Text, txtFaxNo.Text, txtWebsite.Text, strLongitude, strLatitude, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                }
                else
                {
                    DisplayMessage("Record  Not Updated");
                }
            }
            else
            {
                b = objAddressM.InsertAddressMaster(ddlAddressCategory.SelectedValue, txtAddressName.Text, txtAddress.Text, txtStreet.Text, txtBlock.Text, txtAvenue.Text, strCountryId, hdnstateId.Value, hdncityId.Value, txtPinCode.Text, strPhoneNo1.Trim(), strPhoneNo2.Trim(), strMobile1.Trim(), strMObile2.Trim(), txtEmailId1.Text, txtEmailId2.Text, txtFaxNo.Text, txtWebsite.Text, strLongitude, strLatitude, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    DisplayMessage("Record Saved","green");
                }
                else
                {
                    DisplayMessage("Record  Not Saved");
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
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
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
    protected void BtnUpdateLatLong_Click(object sender, EventArgs e)
    {
        if (Session["Long"] != null && Session["Lati"] != null)
        {
            txtLongitude.Text = Session["Long"].ToString();
            txtLatitude.Text = Session["Lati"].ToString();
            Session["Long"] = null;
            Session["Lati"] = null;
        }
        else
        {
            try
            {
                if (txtLatitude.Text.Contains("0.0000") && txtLatitude.Text.Contains("0.0000"))
                {
                    string FullAddress = txtAddress.Text + "," + txtCity.Text + "," + txtState.Text + "," + ddlCountry.SelectedItem.Text + "," + txtPinCode.Text;
                    string url = "http://maps.google.com/maps/api/geocode/xml?address=" + FullAddress + "&sensor=false";
                    WebRequest request = WebRequest.Create(url);
                    using (WebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            DataSet dsResult = new DataSet();
                            dsResult.ReadXml(reader);
                            DataTable dtCoordinates = new DataTable();
                            dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                        new DataColumn("Address", typeof(string)),
                        new DataColumn("Latitude",typeof(string)),
                        new DataColumn("Longitude",typeof(string)) });
                            foreach (DataRow row in dsResult.Tables["result"].Rows)
                            {
                                string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                                DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                                dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                            }
                            txtLatitude.Text = dtCoordinates.Rows[0]["Latitude"].ToString();
                            txtLongitude.Text = dtCoordinates.Rows[0]["Longitude"].ToString();
                        }
                    }
                }
                else
                {
                    Session["Long"] = txtLongitude.Text;
                    Session["Lati"] = txtLatitude.Text;
                }
            }
            catch
            {
            }
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Additional_Info_Open()", true);
    }

    #region Bin Section
    protected void GvAddressBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        GvAddressBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtInactive"];
        objPageCmn.FillData((object)GvAddressBin, dt, "", "");
        PopulateCheckedValues();
    }
    protected void GvAddressBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objAddressM.GetAddressFalseAllData();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        objPageCmn.FillData((object)GvAddressBin, dt, "", "");
        Session["CHECKED_ITEMS"] = null;
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objAddressM.GetAddressFalseAllData();
        objPageCmn.FillData((object)GvAddressBin, dt, "", "");
        Session["dtPBrandBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["CHECKED_ITEMS"] = null;
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
            objPageCmn.FillData((object)GvAddressBin, view.ToTable(), "", "");
            Session["CHECKED_ITEMS"] = null;
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int Msg = 0;
        if (GvAddressBin.Rows.Count != 0)
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
                        Msg = objAddressM.DeleteAddressMaster(Session["CompId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    if (Msg != 0)
                    {
                        FillGrid();
                        FillGridBin();
                        ViewState["Select"] = null;
                        Session["CHECKED_ITEMS"] = null;
                        DisplayMessage("Record Activated");
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
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvAddressBin.HeaderRow.FindControl("chkCurrent"));
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
        foreach (GridViewRow gvrow in GvAddressBin.Rows)
        {
            index = (int)GvAddressBin.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkSelect")).Checked = false;
            }
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
      
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        Session["CHECKED_ITEMS"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        DataTable dtDEPARTMENT = (DataTable)Session["dtInactive"];
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
            foreach (GridViewRow gvrow in GvAddressBin.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressBin, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        if (GvAddressBin.Rows.Count != 0)
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
                        Msg = objAddressM.DeleteAddressMaster(Session["CompId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    if (Msg != 0)
                    {
                        FillGrid();
                        FillGridBin();
                        ViewState["Select"] = null;
                        Session["CHECKED_ITEMS"] = null;
                        DisplayMessage("Record Activated");
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
            foreach (GridViewRow gvrow in GvAddressBin.Rows)
            {
                int index = (int)GvAddressBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvAddressBin.Rows)
        {
            index = (int)GvAddressBin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
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
    #endregion
    #endregion
    #region User defind Funcation
    private void FillAddressCategory()
    {
        DataTable dsAddressCat = null;
        dsAddressCat = ObjAddressCat.GetAddressCategoryAll();
        if (dsAddressCat.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlAddressCategory, dsAddressCat, "Address_Name", "Address_Category_Id");
        }
        else
        {
            ddlAddressCategory.Items.Insert(0, "--Select--");
            ddlAddressCategory.SelectedIndex = 0;
        }
    }
    private void FillCountry()
    {
        DataTable dsCountry = null;
        dsCountry = ObjCountry.GetCountryMaster();
        if (dsCountry.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlCountry, dsCountry, "Country_Name", "Country_Id");
        }
        else
        {
            ddlCountry.Items.Insert(0, "--Select--");
            ddlCountry.SelectedIndex = 0;
        }
    }
    public bool CheckEmailId(string EmailAddress)
    {
        return Regex.IsMatch(EmailAddress, "\\w+([-+.']\\+)*@\\w+([-.]\\+)*\\.\\w+([-.]\\+)*");
    }
    private void FillGrid()
    {
        DataTable dtBrand = objAddressM.GetAddressAllData(Session["CompId"].ToString());
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        Session["dtCurrency"] = dtBrand;
        Session["dtFilter_Address"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            objPageCmn.FillData((object)GvAddress, dtBrand, "", "");
        }
        else
        {
            GvAddress.DataSource = null;
            GvAddress.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count.ToString() + "";
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
        FillAddressCategory();
        txtAddressName.Text = "";
        txtAddress.Text = "";
        txtStreet.Text = "";
        txtBlock.Text = "";
        txtAvenue.Text = "";
        FillCountry();
        hdncityId.Value = "";
        hdnstateId.Value = "";
        txtState.Text = "";
        txtCity.Text = "";
        txtPinCode.Text = "";
        txtPhoneNo1.Text = "";
        txtPhoneNo2.Text = "";
        txtMobileNo1.Text = "";
        txtMobileNo2.Text = "";
        txtEmailId1.Text = "";
        txtEmailId2.Text = "";
        txtFaxNo.Text = "";
        txtWebsite.Text = "";
        txtLongitude.Text = "";
        txtLatitude.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        Session["CHECKED_ITEMS"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        if (ViewState["CountryCode"] != null)
        {
            try
            {
                txtCountryCode_Phoneno1.Text = "+" + ViewState["CountryCode"].ToString();
                txtCountryCode_Phoneno2.Text = "+" + ViewState["CountryCode"].ToString();
                txtCountryCode_MobileNo1.Text = "+" + ViewState["CountryCode"].ToString();
                txtCountryCode_MobileNo2.Text = "+" + ViewState["CountryCode"].ToString();
            }
            catch
            {
            }
        }
        if (ViewState["Country_Id"] != null)
        {
            try
            {
                ddlCountry.SelectedValue = ViewState["Country_Id"].ToString();
                Session["AddCtrl_Country_Id"] = ViewState["Country_Id"].ToString();
            }
            catch
            {
            }
        }
    }
    #endregion
    #region Auto Complete Function
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAreaName(string prefixText, int count, string contextKey)
    {
        Sys_AreaMaster objAreaMaster = new Sys_AreaMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objAreaMaster.GetAreaMaster(), "Area_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Area_Name"].ToString();
        }
        return txt;
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
        if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "")
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.getAddressNamePreText(prefixText);
        string[] str = new string[0];
        if (dt != null)
        {
            str = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Address_Name"].ToString();
            }
        }
        else
        {
            str = null;
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
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ddlCountry_IndexChanged(string CountryId)
    {
        CountryMaster ObjSysCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
        return "+" + ObjSysCountryMaster.GetCountryMasterById(CountryId).Rows[0]["Country_Code"].ToString();
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCity_TextChanged(string stateId, string cityName)
    {
        CityMaster ObjCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string City_id = ObjCityMaster.GetCityIdFromStateIdNCityName(stateId, cityName);
        if (City_id != "" && City_id!= "@NOTFOUND@")
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
        if (stateId != "" && stateId!= "@NOTFOUND@")
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
    #endregion
}