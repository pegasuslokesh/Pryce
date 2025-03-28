using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;

public partial class WebUserControl_AddressControl : System.Web.UI.UserControl
{
    public delegate void parentPageHandler(string strAddressName);
    public event parentPageHandler refreshControlsFromChild;

    
    Set_AddressMaster AM = null;
    Set_AddressCategory ObjAddressCat = null;
    Set_AddressChild objAddChild = null;
    Common ObjComman = null;
    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    CountryMaster ObjSysCountryMaster = null;
    ContactNoMaster objContactnoMaster = null;
    StateMaster ObjStatemaster = null;
    CityMaster ObjCityMaster = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        AM = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        ObjSysCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        ObjStatemaster = new StateMaster(Session["DBConnection"].ToString());
        ObjCityMaster = new CityMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            //FillCountryCode();
            FillAddressCategory();
            FillCountry();
            ResetAddress();
        }
        ContactNo.ExpandNumberDiv += new WebUserControl_ContactNo.parentHandler(ucContactNo_ExpandDiv);
    }
    public void Reset()
    {
        Session["Add_Address_Popup"] = txtAddressNameNew.Text;
        Hdn_Address_ID.Value = "";
        txtAddressNameNew.Text = "";
        Lbl_Tab_New.Text = "New";
        FillAddressCategory();
        txtAddress.Text = "";
        txtStreet.Text = "";
        txtBlock.Text = "";
        txtAvenue.Text = "";
        ContactNo.setNullToGV();
        //FillCountry();
        txtState.Text = "";
        txtCity.Text = "";
        txtPinCode.Text = "";
        txtEmailId1.Text = "";
        txtEmailId2.Text = "";
        //txtFaxNo.Text = "";
        txtWebsite.Text = "";
        txtLongitude.Text = "";
        txtLatitude.Text = "";
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
        ddlAddressCategory.Focus();
    }
    public void GetAddressInformationByAddressname(string strAddress)
    {
        DataTable dtAddressEdit = AM.GetAddressDataByAddressName(strAddress);
        if (dtAddressEdit.Rows.Count > 0)
        {
            ContactNo.setNullToGV();
            ContactNo.FillGridData(dtAddressEdit.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            Lbl_Tab_New.Text = "Edit";
            Hdn_Address_ID.Value = dtAddressEdit.Rows[0]["Trans_Id"].ToString();
            ddlAddressCategory.SelectedValue = dtAddressEdit.Rows[0]["Address_Category_Id"].ToString();
            txtAddressNameNew.Text = dtAddressEdit.Rows[0]["Address_Name"].ToString();
            txtAddress.Text = dtAddressEdit.Rows[0]["Address"].ToString();
            txtStreet.Text = dtAddressEdit.Rows[0]["Street"].ToString();
            txtBlock.Text = dtAddressEdit.Rows[0]["Block"].ToString();
            txtAvenue.Text = dtAddressEdit.Rows[0]["Avenue"].ToString();
            string strCountryId = dtAddressEdit.Rows[0]["CountryId"].ToString();
            if (strCountryId != "" && strCountryId != "0")
            {
                ddlCountry.SelectedValue = strCountryId;
                ContactNo.setCountryCode("+" + ObjSysCountryMaster.GetCountryMasterById(strCountryId).Rows[0]["Country_Code"].ToString());
                Session["AddCtrl_Country_Id"] = strCountryId;
            }
            else
            {
                FillCountry();
            }
            txtState.Text = dtAddressEdit.Rows[0]["State_Name"].ToString();
            txtCity.Text = dtAddressEdit.Rows[0]["City_Name"].ToString();
            hdnstateId.Value = dtAddressEdit.Rows[0]["StateId"].ToString();
            Session["AddCtrl_State_Id"] = dtAddressEdit.Rows[0]["StateId"].ToString();
            hdncityId.Value = dtAddressEdit.Rows[0]["CityId"].ToString();
            txtPinCode.Text = dtAddressEdit.Rows[0]["PinCode"].ToString();
            txtEmailId1.Text = dtAddressEdit.Rows[0]["EmailId1"].ToString();
            txtEmailId2.Text = dtAddressEdit.Rows[0]["EmailId2"].ToString();
            //txtFaxNo.Text = dtAddressEdit.Rows[0]["FaxNo"].ToString();
            txtWebsite.Text = dtAddressEdit.Rows[0]["WebSite"].ToString();
            txtLongitude.Text = dtAddressEdit.Rows[0]["Longitude"].ToString();
            txtLatitude.Text = dtAddressEdit.Rows[0]["Latitude"].ToString();
        }
        dtAddressEdit = null;
    }
    #region Add New Address Concept
    public void BtnNew_click(string country_id)
    {
        ddlCountry.SelectedValue = country_id;
        Session["AddCtrl_Country_Id"] = country_id;
        ContactNo.setCountryCode("+" + ObjSysCountryMaster.GetCountryMasterById(country_id).Rows[0]["Country_Code"].ToString());
    }
    public void ddlCountry_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ContactNo.setCountryCode("+" + ObjSysCountryMaster.GetCountryMasterById(ddlCountry.SelectedValue).Rows[0]["Country_Code"].ToString());
            Session["AddCtrl_Country_Id"] = ddlCountry.SelectedValue;
        }
        catch
        {
        }
    }
    protected void btnAddressSave_Click(object sender, EventArgs e)
    {
        if (!checkValidation())
        {
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
        string strLongitude = string.Empty;
        string strLatitude = string.Empty;
        string strPhoneNo1 = "";
        string strPhoneNo2 = "";
        string strMobile1 = "";
        string strMObile2 = "";
        if (txtLatitude.Text.Trim() == "")
        {
            strLatitude = "0";
        }
        else
        {
            strLatitude = txtLatitude.Text.Trim();
        }
        if (txtLongitude.Text.Trim() == "")
        {
            strLongitude = "0";
        }
        else
        {
            strLongitude = txtLongitude.Text.Trim();
        }
        int count = 0;
        using (DataTable dt_number = ContactNo.getDatatable())
        {
            for (int i = 0; i < dt_number.Rows.Count; i++)
            {
                if (dt_number.Rows[i]["Is_default"].ToString() == "True")
                {
                    count++;
                    strMobile1 = dt_number.Rows[i]["Country_code"].ToString() + "-" + dt_number.Rows[i]["Phone_no"].ToString();
                }
            }
            int b = 0;
            if (Hdn_Address_ID.Value == "")
            {
                b = AM.InsertAddressMaster(ddlAddressCategory.SelectedValue, txtAddressNameNew.Text, txtAddress.Text, txtStreet.Text, txtBlock.Text, txtAvenue.Text, strCountryId, hdnstateId.Value, hdncityId.Value, txtPinCode.Text, strPhoneNo1.Trim(), strPhoneNo2.Trim(), strMobile1.Trim(), strMObile2.Trim(), txtEmailId1.Text, txtEmailId2.Text, "", txtWebsite.Text, strLongitude, strLatitude, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                if (b != 0)
                {
                    for (int i = 0; i < dt_number.Rows.Count; i++)
                    {
                        if (count == 0 && i == 0)
                        {
                            objContactnoMaster.insertDate(Session["CompId"].ToString(), "Set_AddressMaster", b.ToString(), dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), "True", Session["UserId"].ToString(), Session["UserId"].ToString());
                        }
                        else
                        {
                            objContactnoMaster.insertDate(Session["CompId"].ToString(), "Set_AddressMaster", b.ToString(), dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), dt_number.Rows[i]["Is_default"].ToString(), Session["UserId"].ToString(), Session["UserId"].ToString());
                        }
                    }
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Record Saved')", true);
                    try
                    {
                        ((TextBox)Parent.FindControl(((HiddenField)Parent.FindControl("hdntxtaddressid")).Value)).Text = txtAddressNameNew.Text;
                    }
                    catch
                    {
                    }
                    if (Lbl_Tab_New.Text == "New")
                    {
                        if (hdnCustomerId.Value != "" && hdnCustomerId.Value != "NewCust")
                        {
                            //objAddChild.DeleteAddressChild("Contact", hdnCustomerId.Value.ToString());
                            objAddChild.InsertAddressChild(b.ToString(), "Contact", hdnCustomerId.Value, "False", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            fillGridAdd(hdnCustomerId.Value);
                        }
                    }
                    //call parent page event
                    string strAddressName = txtAddressNameNew.Text;
                    ResetAddress();
                    ddlAddressCategory.Focus();
                    if (this.refreshControlsFromChild != null)
                    {
                        refreshControlsFromChild(strAddressName);
                    }
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), UniqueID, "Modal_Address_Close()", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Record Not Saved')", true);
                }
            }
            else
            {
                DataTable dtadd2 = new DataTable();
                b = AM.UpdateAddressMaster(Hdn_Address_ID.Value, ddlAddressCategory.SelectedValue, txtAddressNameNew.Text, txtAddress.Text, txtStreet.Text, txtBlock.Text, txtAvenue.Text, strCountryId, hdnstateId.Value, hdncityId.Value, txtPinCode.Text, strPhoneNo1.Trim(), strPhoneNo2.Trim(), strMobile1.Trim(), strMObile2.Trim(), txtEmailId1.Text, txtEmailId2.Text, "", txtWebsite.Text, strLongitude, strLatitude, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                if (b != 0)
                {
                    objContactnoMaster.deteteDate("Set_AddressMaster", Hdn_Address_ID.Value);
                    for (int i = 0; i < dt_number.Rows.Count; i++)
                    {
                        if (count == 0 && i == 0)
                        {
                            objContactnoMaster.insertDate(Session["CompId"].ToString(), "Set_AddressMaster", Hdn_Address_ID.Value, dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), "True", Session["UserId"].ToString(), Session["UserId"].ToString());
                        }
                        else
                        {
                            objContactnoMaster.insertDate(Session["CompId"].ToString(), "Set_AddressMaster", Hdn_Address_ID.Value, dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), dt_number.Rows[i]["Is_default"].ToString(), Session["UserId"].ToString(), Session["UserId"].ToString());
                        }
                    }
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Record Updated')", true);
                    try
                    {
                        ((TextBox)Parent.FindControl(((HiddenField)Parent.FindControl("hdntxtaddressid")).Value)).Text = txtAddressNameNew.Text;
                    }
                    catch
                    {
                    }
                    try
                    {
                        fillGridAdd(hdnCustomerId.Value);
                    }
                    catch
                    { }
                    ResetAddress();
                    ddlAddressCategory.Focus();
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), UniqueID, "Modal_Address_Close()", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Record  Not Update')", true);
                }
            }
        }
    }
    protected void btnAddressReset_Click(object sender, EventArgs e)
    {
        ResetAddress();
        ddlAddressCategory.Focus();
        ContactNo.setNullToGV();
    }
    protected void btnAddressCancel_Click(object sender, EventArgs e)
    {
        ResetAddress();
        ((Panel)Parent.FindControl("pnlAddress1")).Visible = false;
        ((Panel)Parent.FindControl("pnlAddress2")).Visible = false;
    }
    public bool checkValidation()
    {
        if (ddlAddressCategory.SelectedValue == "--Select--")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Select Address Category')", true);
            ddlAddressCategory.Focus();
            return false;
        }
        if (txtAddressNameNew.Text == "" || txtAddressNameNew.Text == null)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Enter Address Name')", true);
            txtAddressNameNew.Focus();
            return false;
        }
        if (txtAddress.Text == "" || txtAddress.Text == null)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Enter Address')", true);
            txtAddress.Focus();
            return false;
        }
        if (txtEmailId1.Text != "")
        {
            if (!CheckEmailId(txtEmailId1.Text))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Enter Valid Email Address')", true);
                txtEmailId1.Text = "";
                txtEmailId1.Focus();
                return false;
            }
        }
        if (txtEmailId2.Text != "")
        {
            if (!CheckEmailId(txtEmailId2.Text))
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Enter Valid Email Address')", true);
                txtEmailId2.Text = "";
                txtEmailId2.Focus();
                return false;
            }
        }
        if (txtLongitude.Text != "")
        {
            float flTemp = 0;
            if (!float.TryParse(txtLongitude.Text, out flTemp))
            {
                txtLongitude.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Enter Numeric Value Only')", true);
                txtLongitude.Focus();
                return false;
            }
        }
        if (txtLatitude.Text != "")
        {
            float flTemp = 0;
            if (!float.TryParse(txtLatitude.Text, out flTemp))
            {
                txtLatitude.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Enter Numeric Value Only')", true);
                txtLatitude.Focus();
                return false;
            }
        }
        return true;
    }
    protected void ResetAddress()
    {
        FillAddressCategory();
        txtAddressNameNew.Text = "";
        txtAddress.Text = "";
        txtStreet.Text = "";
        txtBlock.Text = "";
        txtAvenue.Text = "";
        //FillCountry();
        txtState.Text = "";
        txtCity.Text = "";
        txtPinCode.Text = "";
        txtEmailId1.Text = "";
        txtEmailId2.Text = "";
        //txtFaxNo.Text = "";
        txtWebsite.Text = "";
        txtLongitude.Text = "";
        txtLatitude.Text = "";
        ddlAddressCategory.Focus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
    }
    public bool CheckEmailId(string EmailAddress)
    {
        return Regex.IsMatch(txtEmailId1.Text,
                      "\\w+([-+.']\\+)*@\\w+([-.]\\+)*\\.\\w+([-.]\\+)*");
    }
    private void FillAddressCategory()
    {
        DataTable dsAddressCat = null;
        dsAddressCat = ObjAddressCat.GetAddressCategoryAll();
        if (dsAddressCat.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
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
        CountryMaster objCountry = new CountryMaster(Session["DBConnection"].ToString());
        DataTable dsCountry = null;
        dsCountry = objCountry.getCountryListForDDL();
        if (dsCountry.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlCountry, dsCountry, "Country_Name", "Country_Id");
        }
        else
        {
            ddlCountry.Items.Insert(0, "--Select--");
            ddlCountry.SelectedIndex = 0;
        }
        if (ViewState["Country_Id"] != null)
        {
            try
            {
                ddlCountry.SelectedValue = ViewState["Country_Id"].ToString();
                Session["AddCtrl_Country_Id"] = ddlCountry.SelectedValue;
            }
            catch
            {
            }
        }
        dsCountry = null;
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
                    //string FullAddress = txtAddress.Text + "," + txtCity.Text + "," + txtState.Text + "," + ddlCountry.SelectedItem.Text + "," + txtPinCode.Text;
                    string FullAddress = ddlCountry.SelectedItem.Text + "," + txtState.Text + "," + txtCity.Text + "," + txtAddress.Text + "," + txtPinCode.Text;
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
                            Session["Long"] = null;
                            Session["Lati"] = null;
                            dtCoordinates = null;
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
    protected void btnGetLatLong_Click(object sender, EventArgs e)
    {
        if (txtLatitude.Text.Contains("0.0000") && txtLatitude.Text.Contains("0.0000"))
        {
            //string FullAddress = txtAddress.Text + "," + txtCity.Text + "," + txtState.Text + "," + ddlCountry.SelectedItem.Text + "," + txtPinCode.Text;
            string FullAddress = ddlCountry.SelectedItem.Text + "," + txtState.Text + "," + txtCity.Text + "," + txtAddress.Text + "," + txtPinCode.Text;
            Session["Add"] = FullAddress;
        }
        else
        {
            Session["Long"] = txtLongitude.Text;
            Session["Lati"] = txtLatitude.Text;
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Additional_Info_Open()", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "nothing", "window.open('../SystemSetup/GoogleMap.aspx','window','width=1024')", true);
    }
    #endregion
    public void FillLocationNCode()
    {
        //FillCountry();
        ddlCountry_OnSelectedIndexChanged(null, null);
    }
    public void fillGridAdd(string ContactTransID = "")
    {
        if (ContactTransID == "" || ContactTransID == "NewCust")
        {
            return;
        }
        DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Contact", ContactTransID);
        GvAddressList.DataSource = dtChild;
        GvAddressList.DataBind();
        dtChild = null;
    }
    protected void GvIBtnAddEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        Label lblgvAddressName = gvRow.FindControl("lblgvAddressName") as Label;
        Hdn_Address_ID.Value = lblgvAddressName.Text;
        CheckBox chkdefault = gvRow.FindControl("chkdefault") as CheckBox;
        hdnIsDefault.Value = chkdefault.Checked.ToString();
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Tab_AddList()", true);
        //hdnCustomerId.Value = e.CommandArgument.ToString();
        //user to fill address data on edit button click 
        FillLocationNCode();
        GetAddressInformationByAddressname(lblgvAddressName.Text);
        txtAddressNameNew.Enabled = false;
    }
    public void setCustomerID(string CustomerTrans_Id = "")
    {
        if (CustomerTrans_Id == "")
        {
            return;
        }
        hdnCustomerId.Value = CustomerTrans_Id;
    }
    protected void chkdefault_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        GridViewRow row = (GridViewRow)cb.NamingContainer;
        string SetTrans_idTrue = (GvAddressList.Rows[row.RowIndex].FindControl("GvIBtnAddEdit") as ImageButton).CommandArgument.ToString();
        string SetTrans_idFalse = "";
        if (!cb.Checked)
        {
            objAddChild.SetISDefaultAddressFalseByAddChildTransId(SetTrans_idTrue);
        }
        else
        {
            for (int i = 0; i < GvAddressList.Rows.Count; i++)
            {
                if (row.RowIndex != i)
                {
                    SetTrans_idFalse = (GvAddressList.Rows[i].FindControl("GvIBtnAddEdit") as ImageButton).CommandArgument.ToString();
                    objAddChild.SetISDefaultAddressFalseByAddChildTransId(SetTrans_idFalse);
                    break;
                }
            }
            objAddChild.SetISDefaultAddressTrueByAddChildTransId(SetTrans_idTrue);
        }
        if (hdnCustomerId.Value == "")
        {
            return;
        }
        fillGridAdd(hdnCustomerId.Value);
    }
    protected void imgBtnDelete_Command(object sender, CommandEventArgs e)
    {
        if (hdnCustomerId.Value == "")
        {
            return;
        }
        ImageButton ib = (ImageButton)sender;
        GridViewRow row = (GridViewRow)ib.NamingContainer;
        string trans_id = (GvAddressList.Rows[row.RowIndex].FindControl("imgBtnDelete") as ImageButton).CommandArgument.ToString();
        objAddChild.DeleteChildRecordByTrans_Id(trans_id);
        fillGridAdd(hdnCustomerId.Value);
    }
    public void showListOrNot(string data)
    {
        if (data == "Yes")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "displayList()", true);
        }
    }
    public void fillHeader(string PartyName)
    {
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtAddressNameNew.Enabled = true;
        lblHeaderL.Text = "New Address";
        lblHeaderR.Text = PartyName;
    }

    private void ucContactNo_ExpandDiv()
    {
        Btn_Div_Additional_Info.Attributes.Add("Class", "fa fa-minus");
        Div_Additional_Info.Attributes.Add("Class", "box box-primary");
    }
}
// should be added on parent pages
//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetCompletionListStateName(string prefixText, int count, string contextKey)
//{
//    if (HttpContext.Current.Session["AddCtrl_Country_Id"].ToString() == "")
//    {
//        return null;
//    }
//    StateMaster objStateMaster = new StateMaster();
//    DataTable dt = objStateMaster.GetAllStateByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_Country_Id"].ToString());
//    string[] txt = new string[dt.Rows.Count];
//    for (int i = 0; i < dt.Rows.Count; i++)
//    {
//        txt[i] = dt.Rows[i]["State_Name"].ToString();
//    }
//    return txt;
//}
//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetCompletionListCityName(string prefixText, int count, string contextKey)
//{
//    try
//    {
//        if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "" || HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "0")
//        {
//            return null;
//        }
//        CityMaster objCityMaster = new CityMaster();
//        DataTable dt = objCityMaster.GetAllCityByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_State_Id"].ToString());
//        string[] txt = new string[dt.Rows.Count];
//        for (int i = 0; i < dt.Rows.Count; i++)
//        {
//            txt[i] = dt.Rows[i]["City_Name"].ToString();
//        }
//        return txt;
//    }
//    catch
//    {
//        return null;
//    }
//}
//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
//{
//    Set_AddressMaster AddressN = new Set_AddressMaster();
//    DataTable dt = AddressN.GetDistinctAddressName1(prefixText);
//    string[] str = new string[0];
//    if (dt != null)
//    {
//        str = new string[dt.Rows.Count];
//        for (int i = 0; i < dt.Rows.Count; i++)
//        {
//            str[i] = dt.Rows[i]["Address_Name"].ToString();
//        }
//    }
//    else
//    {
//        str = null;
//    }
//    return str;
//}
//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
//{
//    ContactNoMaster objContactNumMaster = new ContactNoMaster();
//    DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
//    string[] txt = new string[dt.Rows.Count];
//    for (int i = 0; i < dt.Rows.Count; i++)
//    {
//        txt[i] = dt.Rows[i]["Phone_no"].ToString();
//    }
//    return txt;
//}
//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static string ddlCountry_IndexChanged(string CountryId)
//{
//    CountryMaster ObjSysCountryMaster = new CountryMaster();
//    HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
//    HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
//    return "+" + ObjSysCountryMaster.GetCountryMasterById(CountryId).Rows[0]["Country_Code"].ToString();
//}
//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static void resetAddress()
//{
//    HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
//}
//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static string txtCity_TextChanged(string stateId, string cityName)
//{
//    CityMaster ObjCityMaster = new CityMaster();
//    string City_id = ObjCityMaster.GetCityIdFromStateIdNCityName(stateId, cityName);
//    if (City_id != "")
//    {
//        return City_id;
//    }
//    else
//    {
//        return "0";
//    }
//}
//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static string txtState_TextChanged(string CountryId, string StateName)
//{
//    StateMaster ObjStatemaster = new StateMaster();
//    string stateId = ObjStatemaster.GetStateIdFromCountryIdNStateName(CountryId, StateName);
//    if (stateId != "")
//    {
//        HttpContext.Current.Session["AddCtrl_State_Id"] = stateId;
//        return stateId;
//    }
//    else
//    {
//        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
//        return "0";
//    }
//}
//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static int txtAddressNameNew_TextChanged(string AddressName, string addressId)
//{
//    // return  1 when 'Address Name Already Exists' and 0 when not present
//    Set_AddressMaster AM = new Set_AddressMaster();
//    string data = AM.GetAddressDataExistOrNot(AddressName, addressId);
//    if (data == "0")
//    {
//        return 0;
//    }
//    else
//    {
//        return 1;
//    }
//}