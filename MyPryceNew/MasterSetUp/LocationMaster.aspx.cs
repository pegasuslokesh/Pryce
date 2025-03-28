using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using PegasusDataAccess;


public partial class MasterSetUp_LocationMaster : BasePage
{
    LocationMaster objLocation = null;
    Set_Location_Department objLocDept = null;
    Common cmn = null;
    DataAccessClass objda = null;
    Set_ApplicationParameter objAppParam = null;
    SystemParameter objSys = null;
    Set_AddressMaster AM = null;
    Set_AddressCategory ObjAddressCat = null;
    Set_AddressChild objAddChild = null;
    EmployeeMaster objEmp = null;
    UserDataPermission objUserDataPerm = null;
    Sys_LocationType_Master objLocationType = null;
    DepartmentMaster objDep = null;
    IT_ObjectEntry objObjectEntry = null;
    CurrencyMaster objCurrency = null;
    Inv_ParameterMaster objInvParam = null;
    CountryMaster ObjCountry = null;
    Ac_Finance_Year_Info ObjFinance = null;
    Ac_FinancialYear_Detail ObjFinancedetail = null;
    Country_Currency objCountryCurrency = null;
    hr_laborLaw_config ObjLabourLaw = null;
    ContactNoMaster objContactnoMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    BrandMaster ObjBrandMaster = null;
    public const int grdDefaultColCount = 4;
    PageControlCommon objPageCmn = null;
    private const string strPageName = "LocationMaster";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        AM = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objUserDataPerm = new UserDataPermission(Session["DBConnection"].ToString());
        objLocationType = new Sys_LocationType_Master(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjCountry = new CountryMaster(Session["DBConnection"].ToString());
        ObjFinance = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        ObjFinancedetail = new Ac_FinancialYear_Detail(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        ObjLabourLaw = new hr_laborLaw_config(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        hdntxtaddressid.Value = txtAddressName.ID;
        hdnEmployeeId.Value = txtManagerName.ID;
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/LocationMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            //code for configure gst and trn visibility on application basis

            if (Session["Application_Id"].ToString().Trim() == "1")
            {
                //for time man application , it should be false
                ctlGSTIN.Visible = false;
                ctlTRN_No.Visible = false;
            }
            else
            {
                ctlGSTIN.Visible = true;
                ctlTRN_No.Visible = true;
            }
            txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;
            Session["AddCtrl_State_Id"] = "";
            Session["AddCtrl_Country_Id"] = "";
            FillGridBin();
            FillBrand();
            FillGrid();
            FillddlParentLocationDDL();
            FillCurrencyDDL();
            ViewState["imgpath"] = null;
            FillLocationType();
            FillLabourLaw();
            getPageControlsVisibility();
            string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                ASPxFileManager1.SettingsUpload.ValidationSettings.MaxFileSize = int.Parse(ParmValue) * 1000;
            }
            catch(Exception ex)
            {

            }
            RootFolder();
        }
        TreeViewDepartment.Attributes.Add("onclick", "postBackByObject()");
        //AllPageCode();
    }


    public void FillBrand()
    {
        DataTable dtBrand = ObjBrandMaster.GetBrandMaster(Session["CompId"].ToString());


        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            string BrandIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "B", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (BrandIds != "")
            {
                dtBrand = new DataView(dtBrand, "Brand_Id in(" + BrandIds.Substring(0, BrandIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        ddlBrand.DataSource = dtBrand;
        ddlBrand.DataTextField = "Brand_Name";
        ddlBrand.DataValueField = "Brand_Id";
        ddlBrand.DataBind();
        ddlBrand.SelectedValue = Session["BrandId"].ToString();

    }
    public void FillLocationType()
    {
        ddlLocationType.Items.Clear();


        objPageCmn.FillData((object)ddlLocationType, new DataView(objLocationType.GetAllActiveRecord(), "", "Location_Type_Name", DataViewRowState.CurrentRows).ToTable(), "Location_Type_Name", "Trans_Id");
    }


    public void FillLabourLaw()
    {
        DataTable dt = ObjLabourLaw.GetAllTrueRecord(Session["CompId"].ToString());

        objPageCmn.FillData((object)ddlLabour, dt, "Laborlaw_Name", "Trans_Id");

    }

    public void FillCurrencyDDL()
    {
        DataTable dt = objCurrency.GetCurrencyMaster();
        if (dt.Rows.Count > 0)
        {
            ddlCurrency.DataSource = null;
            ddlCurrency.DataBind();
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
        }
        else
        {
            try
            {
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
            }
            catch
            {
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
            }
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        imgAddAddressName.Visible = clsPagePermission.bAdd;
        btnAddNewAddress.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }
    public void FillddlParentLocationDDL()
    {
        DataTable dt = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            ddlParentLocation.DataSource = null;
            ddlParentLocation.DataBind();
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)ddlParentLocation, dt, "Location_Name", "Location_Id");
        }
        else
        {
            try
            {
                ddlParentLocation.Items.Insert(0, "--Select--");
                ddlParentLocation.SelectedIndex = 0;
            }
            catch
            {
                ddlParentLocation.Items.Insert(0, "--Select--");
                ddlParentLocation.SelectedIndex = 0;
            }
        }
    }
    public void FillGrid()
    {
        DataTable dt = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLocationMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Loc_Mstr"] = dt;
        Session["Location"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objLocation.GetLocationMasterInactive(Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLocationMasterBin, dt, "", "");
        //AllPageCode();

        Session["dtbinFilter"] = dt;
        Session["dtbinLocation"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
        }
        else
        {

            //AllPageCode();
        }
    }
    protected void gvLocationMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLocationMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Loc_Mstr"];
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLocationMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvLocationMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Loc_Mstr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Loc_Mstr"] = dt;
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLocationMaster, dt, "", "");
        //AllPageCode();
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvLocationMasterBin.Rows)
        {
            index = (int)gvLocationMasterBin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;


            // Check in the Sessionfrestore

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
            foreach (GridViewRow gvrow in gvLocationMasterBin.Rows)
            {
                int index = (int)gvLocationMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvLocationMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvLocationMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLocationMasterBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }

    protected void gvLocationMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objLocation.GetLocationMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLocationMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvLocationMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvLocationMasterBin.Rows)
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

    //protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    string empidlist = string.Empty;
    //    string temp = string.Empty;
    //    int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
    //    Label lb = (Label)gvLocationMasterBin.Rows[index].FindControl("lblLocationId");
    //    if (((CheckBox)gvLocationMasterBin.Rows[index].FindControl("chkgvSelect")).Checked)
    //    {
    //        empidlist += lb.Text.Trim().ToString() + ",";
    //        lblSelectedRecord.Text += empidlist;

    //    }

    //    else
    //    {

    //        empidlist += lb.Text.ToString().Trim();
    //        lblSelectedRecord.Text += empidlist;
    //        string[] split = lblSelectedRecord.Text.Split(',');
    //        foreach (string item in split)
    //        {
    //            if (item != empidlist)
    //            {
    //                if (item != "")
    //                {
    //                    if (item != "")
    //                    {
    //                        temp += item + ",";
    //                    }
    //                }
    //            }
    //        }
    //        lblSelectedRecord.Text = temp;
    //    }
    //}

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        string strParentLocation = string.Empty;

        DataTable dt = objLocation.GetLocationMasterById(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {
            FillddlParentLocationDDL();

            try
            {
                ddlLocationType.SelectedValue = dt.Rows[0]["Location_Type"].ToString();
            }
            catch
            {

            }

            try
            {
                txtLocationCode.Text = dt.Rows[0]["Location_Code"].ToString();
                txtLocationName.Text = dt.Rows[0]["Location_Name"].ToString();
                txtLocationNameL.Text = dt.Rows[0]["Location_Name_L"].ToString();


                strParentLocation = dt.Rows[0]["Parent_Id"].ToString();
                if (strParentLocation != "0")
                {
                    ddlParentLocation.SelectedValue = strParentLocation;
                }
            }
            catch
            {

            }
            try
            {
                ddlCurrency.SelectedValue = dt.Rows[0]["Field1"].ToString();
            }
            catch
            {
                ddlCurrency.SelectedIndex = 0;
            }

            //for labour law

            try
            {
                ddlLabour.SelectedValue = dt.Rows[0]["Field3"].ToString();
            }
            catch
            {
                ddlLabour.SelectedIndex = 0;
            }

            txtGSTIN.Text = dt.Rows[0]["Field4"].ToString();
            Txt_TRN_No.Text = dt.Rows[0]["Field5"].ToString();

            if (dt.Rows[0]["Emp_Id"].ToString().Trim() != "0" || dt.Rows[0]["Emp_Id"].ToString().Trim() != "")
            {
                txtManagerName.Text = cmn.GetEmpName(dt.Rows[0]["Emp_Id"].ToString(),HttpContext.Current.Session["CompId"].ToString());
            }
            else
            {
                txtManagerName.Text = "";

            }


            Session["imgpath"] = dt.Rows[0]["Field2"].ToString();

            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Location", editid.Value);
            if (dtChild.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 13-05-2015
                objPageCmn.FillData((object)GvAddressName, dtChild, "", "");
                int Sr_No = 1;
                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    Label lblAddressName = (Label)gvr.FindControl("lblgvAddressName");
                    Label lblAddress = (Label)gvr.FindControl("lblgvAddress");
                    lblAddress.Text = GetAddressByAddressName(lblAddressName.Text);
                    Label lblSNo = (Label)gvr.FindControl("lblSNo");
                    lblSNo.Text = Sr_No.ToString();
                    Sr_No++;
                }
            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
            btnNew_Click(null, null);
            pnlLoc.Visible = true;
            pnlLocDept.Visible = false;
            try
            {
                // imgLogo.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"].ToString() + "/" + dt.Rows[0]["Field2"].ToString();
              
                string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                string Path = "~/CompanyResource/" + RegistrationCode + "/" + Session["CompId"].ToString() + "";
                imgLogo.ImageUrl = Path + "/" + dt.Rows[0]["Field2"].ToString();
            }
            catch
            {
                string Path = "~/CompanyResource/"+ Session["CompId"].ToString() + "";
                imgLogo.ImageUrl = Path + "/" + dt.Rows[0]["Field2"].ToString();

            }
        }
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        string LocationId = Session["LocId"].ToString();
        if (LocationId == "0")
        {
            b = objLocation.DeleteLocationMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterBy_Id(e.CommandArgument.ToString(), "12");
            if (dtEmp.Rows.Count > 0)
            {
                b = -11;
            }
            else
            {
                b = objLocation.DeleteLocationMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }

        if (b != 0)
        {
            if (b == -11)
            {
                DisplayMessage("Location is Used for Transaction so you can not Delete this Location");
            }
            else
            {
                DisplayMessage("Record Deleted");
                FillGridBin();
                FillGrid();
                Reset();
            }
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public void Reset()
    {
        txtLocationCode.Text = "";
        txtLocationName.Text = "";
        txtLocationNameL.Text = "";
        ddlLocationType.SelectedIndex = 0;
        txtManagerName.Text = "";
        Session["CHECKED_ITEMS"] = null;
        ddlParentLocation.SelectedValue = "--Select--";
        ddlBrand.SelectedValue = Session["BrandId"].ToString();
        try
        {
            imgLogo.ImageUrl = "";
        }
        catch
        {
        }
        Session["imgpath"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtAddressName.Text = "";
        GvAddressName.DataSource = null;
        GvAddressName.DataBind();
        ddlCurrency.SelectedIndex = 0;
        ddlLocationType.SelectedIndex = 0;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtGSTIN.Text = "";
        Txt_TRN_No.Text = "";
        chkSelectAll.Checked = false;
        //AllPageCode();
    }

    protected void txtLocation_OnTextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objLocation.GetLocationMasterByLocationName(Session["CompId"].ToString().ToString(), txtLocationName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtLocationName.Text = "";
                DisplayMessage("Location Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLocationName);
                return;
            }
            DataTable dt1 = objLocation.GetLocationMasterInactive(Session["CompId"].ToString().ToString());
            dt1 = new DataView(dt1, "Location_Name='" + txtLocationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtLocationName.Text = "";
                DisplayMessage("Location Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLocationName);
                return;
            }
            txtLocationNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objLocation.GetLocationMasterById(Session["CompId"].ToString().ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Location_Name"].ToString() != txtLocationName.Text)
                {
                    DataTable dt = objLocation.GetLocationMaster(Session["CompId"].ToString().ToString());
                    dt = new DataView(dt, "Location_Name='" + txtLocationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtLocationName.Text = "";
                        DisplayMessage("Location Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLocationName);
                        return;
                    }
                    DataTable dt1 = objLocation.GetLocationMasterInactive(Session["CompId"].ToString().ToString());
                    dt1 = new DataView(dt1, "Location_Name='" + txtLocationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtLocationName.Text = "";
                        DisplayMessage("Location Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLocationName);
                        return;
                    }
                }
            }
            txtLocationNameL.Focus();
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtLocationCode.Focus();
        pnlLoc.Visible = true;
        pnlLocDept.Visible = false;
        //AllPageCode();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
        //AllPageCode();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
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
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%  " + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["Location"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Loc_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)gvLocationMaster, view.ToTable(), "", "");
            //AllPageCode();
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
            DataTable dtCust = (DataTable)Session["dtbinLocation"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)gvLocationMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }

        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvLocationMasterBin.Rows.Count > 0)
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
                            b = objLocation.DeleteLocationMaster(Session["CompId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
                    foreach (GridViewRow Gvr in gvLocationMasterBin.Rows)
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
                gvLocationMasterBin.Focus();
                return;

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

                if (!userdetails.Contains(dr["Location_Id"]))
                {
                    userdetails.Add(dr["Location_Id"]);
                }
            }
            foreach (GridViewRow GR in gvLocationMasterBin.Rows)
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
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)gvLocationMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }



    //here we are inserting financial year entry on location level 



    public void insertFinancerecord(string strLocationId)
    {
        string FinanceId = ObjFinance.GetInfoByStatus(Session["CompId"].ToString()).Rows[0]["Trans_Id"].ToString();


        ObjFinancedetail.InsertFinancialYearDetail(Session["CompId"].ToString(), ddlBrand.SelectedValue.ToString(), strLocationId, FinanceId, "Open", "", "Open", "", DateTime.Now.ToString(), "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        string strLabourLaw = "0";


        if (ddlLabour.SelectedIndex > 0)
        {
            strLabourLaw = ddlLabour.SelectedValue;
        }
        int b = 0;
        string strParentLocation = string.Empty;

        if (txtLocationCode.Text == "")
        {
            DisplayMessage("Enter Location Code");
            txtLocationCode.Focus();
            return;
        }

        if (ddlLocationType.SelectedIndex == 0)
        {
            DisplayMessage("Select Location Type");
            ddlLocationType.Focus();
            return;
        }

        if (txtLocationName.Text == "")
        {
            DisplayMessage("Enter Location Name");
            txtLocationName.Focus();
            return;
        }
        if (Session["imgpath"] == null)
        {
            Session["imgpath"] = "";
        }


        string empid = string.Empty;
        if (txtManagerName.Text != "")
        {
            empid = txtManagerName.Text.Split('/')[txtManagerName.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Brand_Id='" + ddlBrand.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtManagerName.Text = "";
                txtManagerName.Focus();
                return;
            }
        }
        else
        {
            empid = "0";
        }

        if (ddlCurrency.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Currency in dropdownlist");
            ddlCurrency.Focus();
            return;
        }

        if (ddlParentLocation.SelectedValue == "--Select--")
        {
            strParentLocation = "0";
        }
        else
        {
            strParentLocation = ddlParentLocation.SelectedValue;
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }
        if (editid.Value == "")
        {
            string strFinancialMonth = string.Empty;
            string strworkdayMinute = string.Empty;
            string strYearlyHalfDay = string.Empty;
            string strweekoffday = string.Empty;
            if (ddlLabour.SelectedIndex > 0)
            {

                DataTable dtLabourlaw = ObjLabourLaw.GetRecordbyTRans_Id(Session["CompId"].ToString(), ddlLabour.SelectedValue);

                if (dtLabourlaw.Rows.Count > 0)
                {
                    strFinancialMonth = dtLabourlaw.Rows[0]["fy_start_month"].ToString();
                    strworkdayMinute = dtLabourlaw.Rows[0]["work_day_minutes"].ToString();
                    strYearlyHalfDay = dtLabourlaw.Rows[0]["yearly_halfday"].ToString();
                    strweekoffday = dtLabourlaw.Rows[0]["week_off_day"].ToString();

                }

            }




            DataTable dt = objLocation.GetLocationMaster(Session["CompId"].ToString());
            dt = new DataView(dt, "Brand_Id='" + ddlBrand.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            dt = new DataView(dt, "Location_Code='" + txtLocationCode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Location Code Already Exists");
                txtLocationCode.Focus();
                return;

            }
            DataTable dt1 = objLocation.GetLocationMaster(Session["CompId"].ToString());
            dt1 = new DataView(dt1, "Brand_Id='" + ddlBrand.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            dt1 = new DataView(dt1, "Location_Name='" + txtLocationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Location Name Already Exists");
                txtLocationName.Focus();
                return;

            }




            b = objLocation.InsertLocationMaster(Session["CompId"].ToString(), txtLocationName.Text, txtLocationNameL.Text, txtLocationCode.Text, ddlBrand.SelectedValue.ToString(), strParentLocation, ddlLocationType.SelectedValue, empid, ddlCurrency.SelectedValue, Session["imgpath"].ToString(), strLabourLaw, txtGSTIN.Text.Trim(), Txt_TRN_No.Text.Trim(), false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());



            if (b != 0)
            {
                //here we are giving default permission to admin user 

                if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
                {
                    objda.execute_Command("insert into  Set_DocNumber    SELECT " + Session["CompId"].ToString() + "     ," + ddlBrand.SelectedValue.ToString() + "      ," + b.ToString() + "     ,[Module_Id]      ,[Object_Id]      ,[Prefix]      ,[Suffix]      ,[CompId]      ,[BrandId]      ,[LocationId]      ,[DeptId]      ,[EmpId]      ,[Year]      ,[Month]      ,[Day]      ,[IsUse]      ,[Field1]      ,[Field2]      ,[Field3]      ,[Field4]      ,[Field5]      ,[Field6]      ,[Field7]      ,[IsActive]      ,[CreatedBy]      ,[CreatedDate]      ,[ModifiedBy]      ,[ModifiedDate] ,[FinancialYearValue],[AutogenerateNumber],[AutoGenerateNumberMonth]  FROM [dbo].[Set_DocNumber] where Location_Id = 1");
                    objda.execute_Command("insert into    Set_Payment_Mode_Master     SELECT " + Session["CompId"].ToString() + "     ," + ddlBrand.SelectedValue.ToString() + "      ," + b.ToString() + "     ,[Pay_Mod_Name]      ,[Pay_Mod_Name_L]      ,[Account_No]      ,[Field1]      ,[Field2]      ,[Field3]      ,[Field4]      ,[Field5]      ,[Field6]      ,[Field7]      ,[IsActive]      ,[CreatedBy]      ,[CreatedDate]      ,[ModifiedBy]      ,[ModifiedDate]  FROM [dbo].[Set_Payment_Mode_Master] where Location_Id = 1");
                }

                //financial year entry
                insertFinancerecord(b.ToString());
                // Nitin Jain  Save Location Parameter here 

                DataTable dtParam = objAppParam.GetApplicationParameterByCompanyId("", Session["CompId"].ToString());
                dtParam = new DataView(dtParam, "Brand_Id='" + ddlBrand.SelectedValue.ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtParam.Rows.Count == 0)
                {
                    dtParam = objAppParam.GetApplicationParameterByCompanyId("", "1");
                    dtParam = new DataView(dtParam, "Brand_Id='1'  and Location_Id='1'", "", DataViewRowState.CurrentRows).ToTable();
                }


                if (dtParam.Rows.Count > 0)
                {
                    string strParamValue = string.Empty;


                    for (int i = 0; i < dtParam.Rows.Count; i++)
                    {
                        if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "FinancialYearStartMonth" && ddlLabour.SelectedIndex > 0)
                        {
                            strParamValue = strFinancialMonth;
                        }
                        else if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "Work Day Min" && ddlLabour.SelectedIndex > 0)
                        {
                            strParamValue = strworkdayMinute;
                        }
                        else if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "Half_Day_Count" && ddlLabour.SelectedIndex > 0)
                        {
                            strParamValue = strYearlyHalfDay;
                        }
                        else if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "Week Off Days" && ddlLabour.SelectedIndex > 0)
                        {
                            strParamValue = strweekoffday;
                        }
                        else
                        {
                            strParamValue = dtParam.Rows[i]["Param_Value"].ToString();
                        }
                        objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), ddlBrand.SelectedValue.ToString(), b.ToString(), dtParam.Rows[i]["Param_Name"].ToString(), strParamValue, dtParam.Rows[i]["Param_Cat_Id"].ToString(), dtParam.Rows[i]["Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
                ///

                //this code is created by jitendra upadhyay on 07-01-2015
                //this code for insert the all inventory parameter acording new location id

                //code start

                if (Session["Application_Id"].ToString() != "1")
                {

                    DataTable dtInvParam = new DataTable();

                    //if (ddlParentLocation.SelectedValue == "--Select--")
                    //{


                    dtInvParam = objInvParam.GetParameterMasterAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

                    //}
                    //else
                    //{
                    //    dtInvParam = objInvParam.GetParameterMasterAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlParentLocation.SelectedValue);
                    //}
                    if (dtInvParam.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtInvParam.Rows.Count; i++)
                        {
                            //if (dtInvParam.Rows[i]["ParameterName"].ToString() == "Purchase Account Parameter" || dtInvParam.Rows[i]["ParameterName"].ToString() == "Sales Account Parameter")
                            //{
                            //    objInvParam.InsertParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), b.ToString(), dtInvParam.Rows[i]["ParameterName"].ToString(), "0", dtInvParam.Rows[i]["Field1"].ToString(), dtInvParam.Rows[i]["Field2"].ToString(), dtInvParam.Rows[i]["Field3"].ToString(), dtInvParam.Rows[i]["Field4"].ToString(), dtInvParam.Rows[i]["Field5"].ToString(), dtInvParam.Rows[i]["Field6"].ToString(), dtInvParam.Rows[i]["Field7"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            //}
                            //else
                            //{
                            objInvParam.InsertParameterMaster(Session["CompId"].ToString(), ddlBrand.SelectedValue.ToString(), b.ToString(), dtInvParam.Rows[i]["ParameterName"].ToString(), dtInvParam.Rows[i]["ParameterValue"].ToString(), dtInvParam.Rows[i]["Field1"].ToString(), dtInvParam.Rows[i]["Field2"].ToString(), dtInvParam.Rows[i]["Field3"].ToString(), dtInvParam.Rows[i]["Field4"].ToString(), dtInvParam.Rows[i]["Field5"].ToString(), dtInvParam.Rows[i]["Field6"].ToString(), dtInvParam.Rows[i]["Field7"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                            //}

                        }

                    }
                }


                //code end

                string strMaxId = string.Empty;
                strMaxId = b.ToString();
                editid.Value = b.ToString();

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
                        DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text);
                        if (dtAddId.Rows.Count > 0)
                        {
                            string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                            objAddChild.InsertAddressChild(strAddressId, "Location", strMaxId, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }

                DisplayMessage("Record Saved", "green");
                FillGrid();
                //Reset();
                //btnList_Click(null, null);
                lblLocationId1.Text = txtLocationCode.Text;
                lblLocationName1.Text = txtLocationName.Text;
                pnlLoc.Visible = false;
                pnlLocDept.Visible = true;
                chkSelectAll.Checked = false;
                lstDepartmentSelect.DataSource = null;
                lstDepartmentSelect.DataBind();

                FillDepartment();
            }
            else
            {
                DisplayMessage("Record Not Saved");
                return;
            }
        }
        else
        {

            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && objda.return_DataTable("select company_id from Set_DocNumber where Location_Id= " + editid.Value + "").Rows.Count == 0)
            {
                objda.execute_Command("insert into  Set_DocNumber    select " + Session["CompId"].ToString() + "     ," + ddlBrand.SelectedValue.ToString() + "      ," + editid.Value + "     ,[Module_Id]      ,[Object_Id]      ,[Prefix]      ,[Suffix]      ,[CompId]      ,[BrandId]      ,[LocationId]      ,[DeptId]      ,[EmpId]      ,[Year]      ,[Month]      ,[Day]      ,[IsUse]      ,[Field1]      ,[Field2]      ,[Field3]      ,[Field4]      ,[Field5]      ,[Field6]      ,[Field7]      ,[IsActive]      ,[CreatedBy]      ,[CreatedDate]      ,[ModifiedBy]      ,[ModifiedDate],[FinancialYearValue],[AutogenerateNumber],[AutoGenerateNumberMonth]  FROM [dbo].[Set_DocNumber] where Location_Id = 1");
            }


            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && objda.return_DataTable("select company_id from Set_Payment_Mode_Master where Location_Id= " + editid.Value + "").Rows.Count == 0)
            {
                objda.execute_Command("insert into    Set_Payment_Mode_Master     SELECT " + Session["CompId"].ToString() + "     ," + ddlBrand.SelectedValue.ToString() + "      ," + editid.Value + "     ,[Pay_Mod_Name]      ,[Pay_Mod_Name_L]      ,[Account_No]      ,[Field1]      ,[Field2]      ,[Field3]      ,[Field4]      ,[Field5]      ,[Field6]      ,[Field7]      ,[IsActive]      ,[CreatedBy]      ,[CreatedDate]      ,[ModifiedBy]      ,[ModifiedDate]  FROM [dbo].[Set_Payment_Mode_Master] where Location_Id = 1");
            }


            //this code is created by jitendra upadhyay on 07-01-2015
            //this code for insert the all inventory parameter acording new location id

            //code start


            if (Session["Application_Id"].ToString() != "1")
            {

                DataTable dtInvParam = objInvParam.GetParameterMasterAllTrue(Session["CompId"].ToString(), ddlBrand.SelectedValue.ToString(), editid.Value);
                if (dtInvParam.Rows.Count == 0)
                {
                    if (ddlParentLocation.SelectedValue == "--Select--")
                    {
                        dtInvParam = objInvParam.GetParameterMasterAllTrue("1", "1", "1");
                    }
                    else
                    {
                        dtInvParam = objInvParam.GetParameterMasterAllTrue("1", "1", ddlParentLocation.SelectedValue);
                    }

                    for (int i = 0; i < dtInvParam.Rows.Count; i++)
                    {
                        if (dtInvParam.Rows[i]["ParameterName"].ToString() == "Purchase Account Parameter" || dtInvParam.Rows[i]["ParameterName"].ToString() == "Sales Account Parameter")
                        {
                            objInvParam.InsertParameterMaster(Session["CompId"].ToString(), ddlBrand.SelectedValue.ToString(), editid.Value, dtInvParam.Rows[i]["ParameterName"].ToString(), "0", dtInvParam.Rows[i]["Field1"].ToString(), dtInvParam.Rows[i]["Field2"].ToString(), dtInvParam.Rows[i]["Field3"].ToString(), dtInvParam.Rows[i]["Field4"].ToString(), dtInvParam.Rows[i]["Field5"].ToString(), dtInvParam.Rows[i]["Field6"].ToString(), dtInvParam.Rows[i]["Field7"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                        else
                        {
                            objInvParam.InsertParameterMaster(Session["CompId"].ToString(), ddlBrand.SelectedValue.ToString(), editid.Value, dtInvParam.Rows[i]["ParameterName"].ToString(), dtInvParam.Rows[i]["ParameterValue"].ToString(), dtInvParam.Rows[i]["Field1"].ToString(), dtInvParam.Rows[i]["Field2"].ToString(), dtInvParam.Rows[i]["Field3"].ToString(), dtInvParam.Rows[i]["Field4"].ToString(), dtInvParam.Rows[i]["Field5"].ToString(), dtInvParam.Rows[i]["Field6"].ToString(), dtInvParam.Rows[i]["Field7"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        }

                    }

                }
            }


            //code end

            DataTable dt = objLocation.GetLocationMaster(Session["CompId"].ToString());


            string LocationCode = string.Empty;
            string LocationName = string.Empty;

            try
            {
                LocationCode = (new DataView(dt, "Location_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Location_Code"].ToString();
            }
            catch
            {
                LocationCode = "";
            }

            dt = new DataView(dt, "Location_Code='" + txtLocationCode.Text + "' and Location_Code<>'" + LocationCode + "' ", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Location Code Already Exists");
                txtLocationCode.Focus();
                return;

            }



            DataTable dt1 = objLocation.GetLocationMaster(Session["CompId"].ToString());
            try
            {
                LocationName = (new DataView(dt1, "Location_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Location_Name"].ToString();
            }
            catch
            {
                LocationName = "";
            }
            dt1 = new DataView(dt1, "Location_Name='" + txtLocationName.Text + "' and Location_Name<>'" + LocationName + "' ", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Location Name Already Exists");
                txtLocationName.Focus();
                return;

            }
            b = objLocation.UpdateLocationMaster(editid.Value, Session["CompId"].ToString(), txtLocationName.Text, txtLocationNameL.Text, txtLocationCode.Text, ddlBrand.SelectedValue.ToString(), strParentLocation, ddlLocationType.SelectedValue, empid, ddlCurrency.SelectedValue, Session["imgpath"].ToString(), strLabourLaw, txtGSTIN.Text.Trim(), Txt_TRN_No.Text.Trim(), false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                // ---------------------------------------------------
                DataTable dtParam = objAppParam.GetApplicationParameterByCompanyId("", Session["CompId"].ToString());
                dtParam = new DataView(dtParam, "Brand_Id='" + ddlBrand.SelectedValue.ToString() + "'  and Location_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtParam.Rows.Count == 0)
                {
                    dtParam = objAppParam.GetApplicationParameterByCompanyId("", "1");
                    dtParam = new DataView(dtParam, "Brand_Id='1'  and Location_Id='1'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtParam.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtParam.Rows.Count; i++)
                        {
                            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), ddlBrand.SelectedValue.ToString(), editid.Value.ToString(), dtParam.Rows[i]["Param_Name"].ToString(), dtParam.Rows[i]["Param_Value"].ToString(), dtParam.Rows[i]["Param_Cat_Id"].ToString(), dtParam.Rows[i]["Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                // -------------------------------------------------

                objAddChild.DeleteAddressChild("Location", editid.Value);
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
                        DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text);
                        if (dtAddId.Rows.Count > 0)
                        {
                            string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                            objAddChild.InsertAddressChild(strAddressId, "Location", editid.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }


                lblLocationId1.Text = txtLocationCode.Text;
                lblLocationName1.Text = txtLocationName.Text;
                //btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                //Reset();
                //Lbl_Tab_New.Text = Resources.Attendance.New;
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                chkSelectAll.Checked = false;
                FillGrid();
                pnlLoc.Visible = false;
                pnlLocDept.Visible = true;
                FillDepartment();
                lstDepartmentSelect.Items.Clear();
                lstDepartmentSelect.DataSource = null;
                lstDepartmentSelect.DataBind();


                DataTable dtDept = objLocDept.GetDepartmentByLocationId(editid.Value);
                for (int i = 0; i < dtDept.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Value = dtDept.Rows[i]["Dep_Id"].ToString();
                    try
                    {
                        li.Text = objDep.GetDepartmentMasterById(dtDept.Rows[i]["Dep_Id"].ToString()).Rows[0]["Dep_Name"].ToString();
                    }
                    catch
                    {
                    }
                    lstDepartmentSelect.Items.Add(li);
                    lstDepartment.Items.Remove(li);
                }


            }
            else
            {
                DisplayMessage("Record Not Updated");
                return;
            }

        }


        //here we inser record in inventory paerameter table 
        ddlLabour.SelectedIndex = 0;

        BindTreeView();
    }
    protected void btnSaveDept_Click(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && Session["UserId"].ToString().Trim().ToLower() == "admin")
        {
            objda.execute_Command("delete from Set_UserDataPermission where ((record_type='L' and record_id=" + editid.Value + ") or Field1='" + editid.Value + "') and User_id='" + Session["UserId"].ToString().Trim() + "' ");
            objUserDataPerm.InsertUserDataPermission(Session["UserId"].ToString().Trim(), Session["CompId"].ToString(), "L", editid.Value, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        objLocDept.DeleteLocationMaster(editid.Value);

        foreach (TreeNode ModuleNode in TreeViewDepartment.Nodes)
        {
            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && Session["UserId"].ToString().Trim().ToLower() == "admin")
            {
                objUserDataPerm.InsertUserDataPermission(Session["UserId"].ToString().Trim(), Session["CompId"].ToString(), "D", ModuleNode.Value, editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            if (ModuleNode.Checked)
            {
                objLocDept.InsertLocationDepartmentMaster(editid.Value, ModuleNode.Value, "0", "", "", "", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                childNodeSave(ModuleNode);
            }
        }

        Reset();
        DisplayMessage("Record Saved", "green");
        Lbl_Tab_New.Text = Resources.Attendance.New;

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    public void childNodeSave(TreeNode ModuleNode)
    {

        foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
        {
            if (ObjNode.Checked)
            {
                objLocDept.InsertLocationDepartmentMaster(editid.Value, ObjNode.Value, "0", "", "", "", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                childNodeSave(ObjNode);
            }
        }
    }
    protected void btnResetDept_Click(object sender, EventArgs e)
    {
        lstDepartment.Items.Clear();
        lstDepartment.DataSource = null;
        lstDepartment.DataBind();
        FillDepartment();
        lstDepartmentSelect.Items.Clear();
        lstDepartmentSelect.DataSource = null;
        lstDepartmentSelect.DataBind();
    }
    protected void btnCancelDept_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    public void FillDepartment()
    {
        lstDepartment.Items.Clear();
        lstDepartment.DataSource = null;
        lstDepartment.DataBind();
        DataTable dtDept = new DataTable();
        dtDept = objDep.GetDepartmentMaster();
        if (dtDept.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)lstDepartment, dtDept, "Dep_Name", "Dep_Id");
        }
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstDepartmentSelect.Items)
        {

            DataTable dtDeptEmp = objEmp.GetDepartmentofEmployeeByLocation(Session["CompId"].ToString(), Session["CompId"].ToString(), editid.Value.ToString());

            if (dtDeptEmp.Rows.Count > 0)
            {
                dtDeptEmp = new DataView(dtDeptEmp, "Department_Id =" + li.Value, "", DataViewRowState.CurrentRows).ToTable();
                if (dtDeptEmp.Rows.Count > 0)
                {

                }
                else
                {
                    lstDepartment.Items.Add(li);
                }
            }
            else
            {
                lstDepartment.Items.Add(li);
            }

        }
        foreach (ListItem li in lstDepartment.Items)
        {
            lstDepartmentSelect.Items.Remove(li);
        }
        btnPullAllDept.Focus();
    }
    protected void btnPushDept_Click(object sender, EventArgs e)
    {
        if (lstDepartment.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstDepartment.Items)
            {
                if (li.Selected)
                {
                    lstDepartmentSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstDepartmentSelect.Items)
            {
                lstDepartment.Items.Remove(li);

            }

            lstDepartmentSelect.SelectedIndex = -1;

        }
        btnPushDept.Focus();


    }
    protected void btnPullDept_Click(object sender, EventArgs e)
    {
        if (lstDepartmentSelect.SelectedIndex >= 0)
        {

            foreach (ListItem li in lstDepartmentSelect.Items)
            {
                if (li.Selected)
                {

                    DataTable dtDeptEmp = objEmp.GetDepartmentofEmployeeByLocation(Session["CompId"].ToString(), Session["CompId"].ToString(), editid.Value.ToString());

                    if (dtDeptEmp.Rows.Count > 0)
                    {
                        dtDeptEmp = new DataView(dtDeptEmp, "Department_Id =" + li.Value, "", DataViewRowState.CurrentRows).ToTable();
                        if (dtDeptEmp.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            lstDepartment.Items.Add(li);
                        }
                    }
                    else
                    {
                        lstDepartment.Items.Add(li);
                    }

                }
            }
            foreach (ListItem li in lstDepartment.Items)
            {
                lstDepartmentSelect.Items.Remove(li);

            }

        }
        btnPullDept.Focus();
    }

    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstDepartment.Items)
        {
            lstDepartmentSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstDepartmentSelect.Items)
        {
            lstDepartment.Items.Remove(DeptItem);


        }
        btnPushAllDept.Focus();
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
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }




    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListLocationCode(string prefixText, int count, string contextKey)
    {
        LocationMaster objLocationMaster = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objLocationMaster.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString()), "Location_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Location_Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListLocationName(string prefixText, int count, string contextKey)
    {
        LocationMaster objLocationMaster = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = new DataView(objLocationMaster.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString()), "Location_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Location_Name"].ToString();
        }
        return txt;
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
    public DataTable CreateAddressDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Address_Name");
        dt.Columns.Add("FullAddress");
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
                        dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblAddressName.Text);
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
                        dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressName.Text);
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
                dt.Rows[0]["FullAddress"] = GetAddressByAddressName(txtAddressName.Text);
            }
            catch
            {

            }
            dt.Rows[0]["Is_Default"] = false.ToString();

        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
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
            Address = dt.Rows[0]["FullAddress"].ToString();
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
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNoData.Rows)
                {
                    if (dr["Type"].ToString() == "Work Fax")
                    {
                        ContactFaxNo = ContactFaxNo + "Work: " + dr["Phone_no"].ToString() + " <br/> ";
                    }

                    if (dr["Type"].ToString() == "Home Fax")
                    {
                        ContactFaxNo = ContactFaxNo + "Home: " + dr["Phone_no"].ToString() + " <br/> ";
                    }
                }

            }

            //if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            //{
            //    ContactFaxNo = DtAddress.Rows[0]["FaxNo"].ToString();
            //}
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


            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");

            if (dt_ContactNoData.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNoData.Rows)
                {
                    if (dr["Type"].ToString() == "Work")
                    {
                        ContactPhoneNo = ContactPhoneNo + "Work: " + dr["Phone_no"].ToString() + " <br/> ";
                    }

                    if (dr["Type"].ToString() == "LandLine")
                    {
                        ContactPhoneNo = ContactPhoneNo + "LandLine:" + dr["Phone_no"].ToString() + " <br/> ";
                    }

                    if (dr["Type"].ToString() == "Home")
                    {
                        ContactPhoneNo = ContactPhoneNo + "Home:" + dr["Phone_no"].ToString() + " <br/> ";
                    }
                }
            }
            else
            {
                ContactPhoneNo = "";
            }


            //if (DtAddress.Rows[0]["PhoneNo1"].ToString() != "")
            //{
            //    ContactPhoneNo = DtAddress.Rows[0]["PhoneNo1"].ToString();
            //}
            //if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
            //{
            //    if (ContactPhoneNo != "")
            //    {
            //        ContactPhoneNo = ContactPhoneNo + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
            //    }
            //    else
            //    {
            //        ContactPhoneNo = DtAddress.Rows[0]["PhoneNo2"].ToString();
            //    }
            //}


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
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNoData.Rows)
                {
                    if (dr["Type"].ToString() == "Mobile" && dr["Is_default"].ToString() == "True")
                    {
                        ContactPhoneNo = ContactPhoneNo + dr["Phone_no"].ToString() + " ";
                    }
                }

            }


            //if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
            //{
            //    ContactPhoneNo = DtAddress.Rows[0]["MobileNo1"].ToString();
            //}
            //if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
            //{
            //    if (ContactPhoneNo != "")
            //    {
            //        ContactPhoneNo = ContactPhoneNo + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
            //    }
            //    else
            //    {
            //        ContactPhoneNo = DtAddress.Rows[0]["MobileNo2"].ToString();
            //    }
            //}
        }
        return ContactPhoneNo;
    }

    public string IsVisible(string data)
    {
        DataTable DtAddress = AM.GetAddressDataByAddressName(data);
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 1)
            {
                return "More";
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }

    }

    #endregion

    #region Add New Address Concept
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {

    }
    #endregion

    #region UploadConcept

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        imgLogo.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"].ToString() + "/" + FULogoPath.FileName;
        ViewState["imgpath"] = FULogoPath.FileName;
    }
    #endregion
    #region Filetobytearray
    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }
    #endregion
    #region Upload
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string ASPxFileManager1_SelectedFileOpened(string fullname, string name)
    {
        try
        {
            DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string Date = DateTime.Now.ToString("yyyyMMddHHss");
            string fileExtension = Path.GetExtension(name);

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name) + Date;
            name = fileNameWithoutExtension + fileExtension;
            CompanyMaster objComp = new CompanyMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string compid = (int.Parse(objComp.GetMaxCompanyId())).ToString();
          
            try
            {
                try
                {

                    string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                    fullname = fullname.Replace("Product_", "Product//Product_");
                    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                    Byte[] buffer = br.ReadBytes((int)numBytes);
                    string fullPath = "~/CompanyResource/" + RegistrationCode + "/" + compid + "";
                    string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                    if (Directory.Exists(MapfullPath))
                    {
                        if (RegistrationCode != "" && RegistrationCode != null)
                        {
                            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);

                        }
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(MapfullPath);
                            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
                catch
                {
                    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                    Byte[] buffer = br.ReadBytes((int)numBytes);
                    string fullPath = "~/CompanyResource/" + HttpContext.Current.Session["CompId"].ToString() +"";
                    string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                    if (Directory.Exists(MapfullPath))
                    {
                       
                       File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                       
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(MapfullPath);
                            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch
            {
            }


            HttpContext.Current.Session["imgpath"] = name;
            return "true";
        }
        catch (Exception err)
        {
            return "false";
        }
    }
    #region Filemanager
    public void RootFolder()
    {
        string User = HttpContext.Current.Session["UserId"].ToString();

        if (User == "superadmin")
        {
            ASPxFileManager1.Settings.RootFolder = "~\\Product";
            ASPxFileManager1.Settings.InitialFolder = "~\\Product";
            ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
        }
        else
        {
            string folderPath = "";
            try
            {
                string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                folderPath = "~\\Product\\Product_" + RegistrationCode + "";
                string fullPath = Server.MapPath(folderPath);
                if (Directory.Exists(fullPath))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(fullPath);
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (Directory.Exists(folderPath + "\\Thumbnail"))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath + "\\Thumbnail");
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                ASPxFileManager1.Settings.RootFolder = folderPath;
                ASPxFileManager1.Settings.InitialFolder = folderPath;
                ASPxFileManager1.Settings.ThumbnailFolder = folderPath + "\\Thumbnail";
            }
            catch(Exception ex)
            {
                ASPxFileManager1.Settings.RootFolder = "~\\Product";
                ASPxFileManager1.Settings.InitialFolder = "~\\Product";
                ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
            }
          
        }

    }
    #endregion
    #endregion

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

    #region BindDepartmentTreeview
    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {
        DataTable dtLocDept = new DataTable();

        if (editid.Value != "")
        {
            dtLocDept = objLocDept.GetDepartmentByLocationId(editid.Value);
        }


        TreeViewDepartment.Nodes.Clear();
        DataTable dt = new DataTable();

        string x = "Parent_Id=" + "0" + "";


        dt = objDep.GetDepartmentMaster();
        dt = new DataView(dt, x, "Dep_Name asc", DataViewRowState.OriginalRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.ShowCheckBox = true;
            tn.Text = dt.Rows[i]["Dep_Name"].ToString();
            tn.Value = dt.Rows[i]["Dep_Id"].ToString();
            if (editid.Value != "")
            {
                DataTable DtTemp = new DataView(dtLocDept, "Dep_Id=" + dt.Rows[i]["Dep_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                if (DtTemp.Rows.Count > 0)
                {
                    tn.Checked = true;
                }
            }
            TreeViewDepartment.Nodes.Add(tn);
            FillChild((dt.Rows[i]["Dep_Id"].ToString()), tn, dtLocDept);

            i++;
        }
        TreeViewDepartment.DataBind();
    }
    private void FillChild(string index, TreeNode tn, DataTable dtLocDept)//fill up child nodes and respective child nodes of them 
    {

        DataTable dt = new DataTable();
        dt = objDep.GetAllDepartmentMaster_By_ParentId(index);

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Dep_Name"].ToString();
            tn1.Value = dt.Rows[i]["Dep_Id"].ToString();
            tn1.ShowCheckBox = true;

            if (editid.Value != "")
            {
                DataTable DtTemp = new DataView(dtLocDept, "Dep_Id=" + dt.Rows[i]["Dep_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                if (DtTemp.Rows.Count > 0)
                {
                    tn1.Checked = true;
                }
            }
            tn.ChildNodes.Add(tn1);
            FillChild((dt.Rows[i]["Dep_Id"].ToString()), tn1, dtLocDept);
            i++;
        }
        TreeViewDepartment.DataBind();
    }
    protected void navTree_SelectedNodeChanged1(object sender, EventArgs e)
    {
        try
        {
            if (TreeViewDepartment.SelectedNode.Checked == true)
            {
                UnSelectChild(TreeViewDepartment.SelectedNode);
            }
            else
            {
                SelectChild(TreeViewDepartment.SelectedNode);
            }
        }
        catch (Exception)
        {
        }
    }
    private void UnSelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = false;
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = false;
            UnSelectChild(treeNode.ChildNodes[i]);
            i++;
        }

        TreeViewDepartment.DataBind();
    }

    private void SelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = true;
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = true;
            SelectChild(treeNode.ChildNodes[i]);
            i++;
        }
        try
        {
            if (treeNode.Parent != null)
            {
                treeNode.Parent.Checked = true;
                treeNode.Parent.Parent.Checked = true;
                treeNode.Parent.Parent.Parent.Checked = true;
                treeNode.Parent.Parent.Parent.Parent.Checked = true;
            }
        
        }
        catch
        { 
        
        }

        TreeViewDepartment.DataBind();

    }

    protected void navTree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        if (e != null && e.Node != null)
        {
            try
            {
                if (e.Node.Checked == true)
                {
                    CheckTreeNodeRecursive(e.Node, true);
                    try
                    {
                        SelectChild(e.Node);
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    CheckTreeNodeRecursive(e.Node, false);
                    UnSelectChild(e.Node);
                }
            }
            catch (Exception)
            {

            }
        }

    }

    private void CheckTreeNodeRecursive(TreeNode parent, bool fCheck)
    {
        foreach (TreeNode child in parent.ChildNodes)
        {
            if (child.Checked != fCheck)
            {
                child.Checked = fCheck;
            }

            if (child.ChildNodes.Count > 0)
            {
                CheckTreeNodeRecursive(child, fCheck);
            }
        }
    }

    protected void ChkSelectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        BindTreeView();
        if (chkSelectAll.Checked)
        {
            foreach (TreeNode Tn in TreeViewDepartment.Nodes)
            {
                SelectChild(Tn);
            }

        }
        else
        {
            foreach (TreeNode Tn in TreeViewDepartment.Nodes)
            {
                UnSelectChild(Tn);

            }

        }
    }

    #endregion
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            // string ext = FULogoPath.FileName.Substring(FULogoPath.FileName.Split('.')[0].Length);
            string ext = Path.GetExtension(FULogoPath.FileName);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpeg"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpeg extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"].ToString()))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"].ToString());
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"].ToString() + "/") + FULogoPath.FileName;
                FULogoPath.SaveAs(path);
                Session["imgpath"] = FULogoPath.FileName;
            }
        }
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
                        FillAddressChidGird("Edit");
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
    protected void btnAddNewAddress_Click(object sender, EventArgs e)
    {
        addaddress.Reset();
        Session["Add_Address_Popup"] = "";
        txtAddressName.Text = "";
        Hdn_Address_ID.Value = "";
        hdnAddressId.Value = "";
        hdnAddressName.Value = "";
        hdntxtaddressid.Value = txtAddressName.Text;

        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
        if (ViewState["Country_Id"] != null)
        {
            addaddress.BtnNew_click(ViewState["Country_Id"].ToString());
        }
        addaddress.fillHeader(txtLocationName.Text);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
    }
    protected void btnAddressEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        Label lblgvAddressName = gvRow.FindControl("lblgvAddressName") as Label;
        Hdn_Address_ID.Value = lblgvAddressName.Text;
        hdntxtaddressid.Value = lblgvAddressName.Text;
        txtAddressName.Text = lblgvAddressName.Text;
        Session["Add_Address_Popup"] = lblgvAddressName.Text;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);

        hdnAddressId.Value = e.CommandArgument.ToString();

        //user to fill address data on edit button click 
        addaddress.GetAddressInformationByAddressname(lblgvAddressName.Text);

        FillAddressDataTabelEdit();
        addaddress.fillHeader(txtLocationName.Text);
        addaddress.FillLocationNCode();
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
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
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
        //Update by Rahul Sharma when Automatic  Select Currency Select
        try
        {
            if (txtAddressName.Text != "")
            {
                ddlCurrency.SelectedValue = objda.get_SingleValue("Select Currency_Id from Sys_Country_Currency inner join Set_AddressMaster on Set_AddressMaster.CountryId=Sys_Country_Currency.Country_Id where Address_Name = '" + txtAddressName.Text + "' And Set_AddressMaster.IsActive = '1'");
            }
        }
        catch(Exception ex)
        {

        }



        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressName, dt, "", "");
        ResetAddressName();
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
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id<>'" + hdnAddressId.Value + "'";
        dt = (DataTable)dv.ToTable();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Trans_Id"] = i + 1;
        }
        return dt;
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
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnAddressId.Value == dt.Rows[i]["Trans_Id"].ToString())
            {
                dt.Rows[i]["Address_Name"] = txtAddressName.Text;
                dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressName.Text);
            }
        }
        return dt;
    }
    public void ResetAddressName()
    {
        txtAddressName.Text = "";
        hdnAddressId.Value = "";
        hdnAddressName.Value = "";
    }

    protected void BtnMoreNumber_Command(object sender, CommandEventArgs e)
    {

        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(e.CommandArgument.ToString());
        string data = "";
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNodata = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");

            if (dt_ContactNodata.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNodata.Rows)
                {
                    data = data + "<b>" + dr["Type"].ToString() + "</b>:" + dr["Phone_no"].ToString() + " <br/>";
                }
            }
        }

        if (data.Trim() != "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_Number_Open('" + data + "');", true);
        }
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
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, gvLocationMaster, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(gvLocationMaster, lstCls);
    }
    protected void btnNewEmployee_Click(object sender, EventArgs e)
    {
        addEmployee.Reset();
        //addEmployee.Reset();
        hdnEmployeeId.Value = txtManagerName.Text;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Employee_Open()", true);
    }
    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
}