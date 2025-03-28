using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetUp_DepartmentManager : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    ReligionMaster objDesg = null;
    Set_Location_Department objLocDept = null;
    EmployeeMaster objEmp = null;
    IT_ObjectEntry objObjectEntry = null;
    LocationMaster objLocation = null;
    CountryMaster ObjCountryMaster = null;
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
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objDesg = new ReligionMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/DepartmentManager.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocation(ddlLocList);          

            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();            

            ddlOption.SelectedIndex = 2;
            FillGrid();
            ddlFieldName.SelectedIndex = 0;
            txtManagerName.Focus();
            try
            {
                string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

                ViewState["CountryCode"] = ObjCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
            }
            catch
            {

            }
            FillCountryCode();
        }

        //AllPageCode();
    }

    public void FillCountryCode()
    {
        DataTable dt = ObjCountryMaster.getCountryCallingCode();
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 27-05-2015
            objPageCmn.FillData((object)ddlCountryCode, dt, "CountryCodeName", "CountryCodeValue");

            //Common Function add By Lokesh on 27-05-2015
            objPageCmn.FillData((object)ddlFaxCountryCode, dt, "CountryCodeName", "CountryCodeValue");
            if (ViewState["CountryCode"] != null)
            {
                try
                {
                    ddlCountryCode.SelectedValue = "+" + ViewState["CountryCode"].ToString();
                    ddlFaxCountryCode.SelectedValue = "+" + ViewState["CountryCode"].ToString();
                }
                catch
                {
                }
            }
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnCSave.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    #region User Defined Funcation

    private void FillGrid()
    {
        DataTable dtBrand = objLocDept.GetDepartmentByLocationId(ddlLocation.SelectedValue);


        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtDepartment"] = dtBrand;
        Session["dtFilter_Deprt_Mgr"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 27-05-2015
            objPageCmn.FillData((object)GvDept, dtBrand, "", "");
            //AllPageCode();
        }
        else
        {
            GvDept.DataSource = null;
            GvDept.DataBind();
        }

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

        txtManagerName.Text = "";
        txtEmailId.Text = "";
        txtFaxNo.Text = "";
        txtPhoneNo.Text = "";
        txtDepCode.Text = "";
        txtDepName.Text = "";
        editid.Value = "";
        ddlLocList.SelectedValue = Session["LocId"].ToString();
        //Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        if (ViewState["CountryCode"] != null)
        {
            try
            {

                ddlCountryCode.SelectedValue = "+" + ViewState["CountryCode"].ToString();
                ddlFaxCountryCode.SelectedValue = "+" + ViewState["CountryCode"].ToString();
            }
            catch
            {
            }
        }

        txtHR1.Text = "";
        txtHR2.Text = "";
        txtHR3.Text = "";
        txtHR4.Text = "";
        txtHR5.Text = "";
        hdnHR1.Value = "";
        hdnHR2.Value = "";
        hdnHR3.Value = "";
        hdnHR4.Value = "";
        hdnHR5.Value = "";
        hdnEmpId.Value = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);

    }
    #endregion
    #region System Defined Funcation
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtManagerName);
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {



        if (txtEmailId.Text != "")
        {
            if (!IsValidEmail(txtEmailId.Text))
            {
                DisplayMessage("Enter Correct Email Id Format");
                txtEmailId.Focus();
                return;

            }

        }


        string CountryCodewithMobileNumber = string.Empty;
        string CountryCodewithFaxNumber = string.Empty;
        if (txtPhoneNo.Text != "")
        {
            CountryCodewithMobileNumber = ddlCountryCode.SelectedValue + "-" + txtPhoneNo.Text;
        }
        if (txtFaxNo.Text != "")
        {
            CountryCodewithFaxNumber = ddlFaxCountryCode.SelectedValue + "-" + txtFaxNo.Text;
        }

        int b = 0;
        if (editid.Value != "")
        {
            b = objLocDept.UpdateLocationDepartmentMaster(editid.Value, ddlLocList.SelectedValue, hdnDepId.Value, hdnEmpId.Value, CountryCodewithMobileNumber, CountryCodewithFaxNumber, txtEmailId.Text, hdnHR1.Value, hdnHR2.Value, hdnHR3.Value, hdnHR4.Value, hdnHR5.Value, true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

            editid.Value = "";
            if (b != 0)
            {
                Reset();
                FillGrid();
                DisplayMessage("Record Updated", "green");

            }
            else
            {
                DisplayMessage("Record  Not Updated");
            }
        }
        else
        {
            DisplayMessage("First click on edit button in below grid");
            Reset();
        }
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString().Split('/')[0].ToString();
        hdnDepId.Value = e.CommandName.ToString();
        DataTable dt = objLocDept.GetDepartmentLocation(editid.Value);
       ddlLocList.SelectedValue = e.CommandArgument.ToString().Split('/')[1].ToString();
            //   Lbl_Tab_New.Text = Resources.Attendance.Edit;
            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["Emp_Id"].ToString().Trim() != "0" || dt.Rows[0]["Emp_Id"].ToString().Trim() != "")
                {
                    txtManagerName.Text = cmn.GetEmpName(dt.Rows[0]["Emp_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    hdnEmpId.Value = dt.Rows[0]["Emp_Id"].ToString();
                }
                else
                {
                    txtManagerName.Text = "";

                }


                if (dt.Rows[0]["Field1"].ToString().Trim() != "0" || dt.Rows[0]["Field1"].ToString().Trim() != "")
                {
                    txtHR1.Text = cmn.GetEmpName(dt.Rows[0]["Field1"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    hdnHR1.Value = dt.Rows[0]["Field1"].ToString();
                }
                else
                {
                    txtHR1.Text = "";
                    hdnHR1.Value = "";

                }

                if (dt.Rows[0]["Field2"].ToString().Trim() != "0" || dt.Rows[0]["Field2"].ToString().Trim() != "")
                {
                    txtHR2.Text = cmn.GetEmpName(dt.Rows[0]["Field2"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    hdnHR2.Value = dt.Rows[0]["Field2"].ToString();
                }
                else
                {
                    txtHR2.Text = "";
                    hdnHR2.Value = "";
                }


                if (dt.Rows[0]["Field3"].ToString().Trim() != "0" || dt.Rows[0]["Field3"].ToString().Trim() != "")
                {
                    txtHR3.Text = cmn.GetEmpName(dt.Rows[0]["Field3"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    hdnHR3.Value = dt.Rows[0]["Field3"].ToString();
                }
                else
                {
                    txtHR3.Text = "";
                    hdnHR3.Value = "";

                }

                if (dt.Rows[0]["Field4"].ToString().Trim() != "0" || dt.Rows[0]["Field4"].ToString().Trim() != "")
                {
                    txtHR4.Text = cmn.GetEmpName(dt.Rows[0]["Field4"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    hdnHR4.Value = dt.Rows[0]["Field4"].ToString();
                }
                else
                {
                    txtHR4.Text = "";
                    hdnHR4.Value = "";

                }

                if (dt.Rows[0]["Field5"].ToString().Trim() != "0" || dt.Rows[0]["Field5"].ToString().Trim() != "")
                {
                    txtHR5.Text = cmn.GetEmpName(dt.Rows[0]["Field5"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    hdnHR5.Value = dt.Rows[0]["Field5"].ToString();
                }
                else
                {
                    txtHR5.Text = "";
                    hdnHR5.Value = "";

                }



                try
                {
                    string[] mobileNumber = dt.Rows[0]["Phone_No"].ToString().Split('-');

                    if (mobileNumber.Length == 1)
                    {
                        txtPhoneNo.Text = mobileNumber[0].ToString();
                    }
                    else
                    {
                        ddlCountryCode.SelectedValue = mobileNumber[0].ToString();

                        txtPhoneNo.Text = mobileNumber[1].ToString();
                    }

                }
                catch
                {

                }
                try
                {
                    string[] FaxNumber = dt.Rows[0]["FaxNo"].ToString().Split('-');

                    if (FaxNumber.Length == 1)
                    {
                        txtFaxNo.Text = FaxNumber[0].ToString();
                    }
                    else
                    {
                        ddlFaxCountryCode.SelectedValue = FaxNumber[0].ToString();

                        txtFaxNo.Text = FaxNumber[1].ToString();
                    }

                }
                catch
                {

                }
                txtEmailId.Text = dt.Rows[0]["EmailId"].ToString();

                txtDepCode.Text = dt.Rows[0]["Dep_Code"].ToString();
                txtDepName.Text = dt.Rows[0]["Dep_Name"].ToString();

            

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtManagerName);
        }
    }
    protected void GvDept_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvDept.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Deprt_Mgr"];
        //Common Function add By Lokesh on 27-05-2015
        objPageCmn.FillData((object)GvDept, dt, "", "");
        //AllPageCode();
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
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text + "%'";
            }
            DataTable dtCurrency = (DataTable)Session["dtDepartment"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 27-05-2015
            objPageCmn.FillData((object)GvDept, view.ToTable(), "", "");
            Session["dtFilter_Deprt_Mgr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void GvDept_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Deprt_Mgr"];
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
        Session["dtFilter_Deprt_Mgr"] = dt;
        //Common Function add By Lokesh on 27-05-2015
        objPageCmn.FillData((object)GvDept, dt, "", "");
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        hdnDepId.Value = e.CommandName.ToString();
        int b = 0;
        b = objLocDept.DeleteLocationMasterByTransId(editid.Value);
             

        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }

        FillGrid();
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();

        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
    }

    public bool IsValidEmail(string EmailAddress)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(EmailAddress,
                      "\\w+([-+.']\\+)*@\\w+([-.]\\+)*\\.\\w+([-.]\\+)*");
    }


    protected void txtManager_TextChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;

        if (editid.Value == "")
        {
            DisplayMessage("First click on edit button in below grid");
            Reset();

        }

        if (txtManagerName.Text != "")
        {
            empid = txtManagerName.Text.Split('/')[txtManagerName.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                txtEmailId.Text = dtEmp.Rows[0]["Email_Id"].ToString().Trim();
                hdnEmpId.Value = empid;
                txtEmailId.Focus();

            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtManagerName.Text = "";
                txtManagerName.Focus();
                txtEmailId.Text = "";
                hdnEmpId.Value = "";
                return;
            }



        }
    }

    protected void txtHr1_TextChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;
        if (editid.Value == "")
        {
            DisplayMessage("First click on edit button in below grid");
            Reset();
        }
        if (txtHR1.Text != "")
        {
            empid = txtHR1.Text.Split('/')[txtHR1.Text.Split('/').Length - 1];           
            DataTable dtEmp = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empid);           
            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hdnHR1.Value = empid;  
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtHR1.Text = "";
                txtHR1.Focus();
                hdnHR1.Value = "";
                return;
            }
        }
        else
        {
            hdnHR1.Value = "";
        }
    }
    protected void txtHr2_TextChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;
        if (editid.Value == "")
        {
            DisplayMessage("First click on edit button in below grid");
            Reset();
        }
        if (txtHR2.Text != "")
        {
            empid = txtHR2.Text.Split('/')[txtHR2.Text.Split('/').Length - 1];
            DataTable dtEmp = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empid);
            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hdnHR2.Value = empid;
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtHR2.Text = "";
                txtHR2.Focus();
                hdnHR2.Value = "";
                return;
            }
        }
        else
        {
            hdnHR2.Value = "";
        }
    }


    protected void txtHr3_TextChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;
        if (editid.Value == "")
        {
            DisplayMessage("First click on edit button in below grid");
            Reset();
        }
        if (txtHR3.Text != "")
        {
            empid = txtHR3.Text.Split('/')[txtHR3.Text.Split('/').Length - 1];
            DataTable dtEmp = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empid);
            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hdnHR3.Value = empid;
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtHR3.Text = "";
                txtHR3.Focus();
                hdnHR3.Value = "";
                return;
            }
        }
        else
        {
            hdnHR3.Value = "";
        }
    }
    protected void txtHr4_TextChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;
        if (editid.Value == "")
        {
            DisplayMessage("First click on edit button in below grid");
            Reset();
        }
        if (txtHR4.Text != "")
        {
            empid = txtHR4.Text.Split('/')[txtHR4.Text.Split('/').Length - 1];
            DataTable dtEmp = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empid);
            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hdnHR4.Value = empid;
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtHR4.Text = "";
                txtHR4.Focus();
                hdnHR4.Value = "";
                return;
            }
        }
        else
        {
            hdnHR4.Value = "";
        }
    }
    protected void txtHr5_TextChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;
        if (editid.Value == "")
        {
            DisplayMessage("First click on edit button in below grid");
            Reset();
        }
        if (txtHR5.Text != "")
        {
            empid = txtHR5.Text.Split('/')[txtHR5.Text.Split('/').Length - 1];
            DataTable dtEmp = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empid);
            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hdnHR5.Value = empid;
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtHR5.Text = "";
                txtHR5.Focus();
                hdnHR5.Value = "";
                return;
            }
        }
        else
        {
            hdnHR5.Value = "";
        }
    }
    #endregion


    public void fillLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 1)
                {
                    ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
                }
            }
            else
            {
                ddlLocation.Items.Insert(0, new ListItem("All", Session["LocId"].ToString()));
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            if (dtLoc.Rows.Count > 1 && LocIds != "")
            {
                ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        dtLoc = null;
    }



    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();        
    }
}
