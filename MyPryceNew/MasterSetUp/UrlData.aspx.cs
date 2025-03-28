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


public partial class MasterSetUp_UrlData : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    //Set_BankMaster objBank = null;
    Set_AddressMaster AM = null;
    Set_AddressCategory ObjAddressCat = null;
    Set_AddressChild objAddChild = null;
    CountryMaster ObjCountry = null;
    LocationMaster objLocation = null;

    Country_Currency objCountryCurrency = null;
    Ac_ChartOfAccount objCOA = null;
    PageControlCommon objPageCmn = null;
    EmployeeMaster objEmp = null;
    Common ObjComman = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        //objBank = new Set_BankMaster(Session["DBConnection"].ToString());
        AM = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjCountry = new CountryMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/UrlData.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);


            // txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;
            FillGridBin();
            FillGrid();

            btnList_Click(null, null);
            //FillCountryCode();
        }

    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }


    #region //CountryCallingCode
    //public void FillCountryCode()
    //{
    //    try
    //    {
    //        string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

    //        ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
    //        ViewState["CountryCode"] = ObjCountry.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
    //    }
    //    catch
    //    {

    //    }
    //}
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

    private void DepartmentList()
    {
        string StrCompId = HttpContext.Current.Session["CompId"].ToString();
        string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
        string StrLocId = HttpContext.Current.Session["LocId"].ToString();
        string UserId = HttpContext.Current.Session["UserId"].ToString();
        if (UserId != "superadmin")
        {
            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 4;
            command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId;
            command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = StrCompId;
            command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = StrLocId;
            command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = StrBrandId;
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            if (dt.Rows.Count > 0)
            {
                Department.DataSource = null;
                Department.DataBind();
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)Department, dt, "Dep_Name", "Dep_Id");
            }
            else
            {
                Department.Items.Insert(0, "--Select--");
                Department.SelectedIndex = 0;
            }
        }
        else
        {
            DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");
            dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                Department.DataSource = null;
                Department.DataBind();
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)Department, dt, "DeptName", "Dep_Id");
            }
            else
            {
                Department.Items.Insert(0, "--Select--");
                Department.SelectedIndex = 0;
            }
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        btnSave.Visible = true;
        btnReset.Visible = true;
        LinkHeader.Focus();
        string StrLocId = HttpContext.Current.Session["LocId"].ToString();
        SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        connection.Close();
        SqlCommand command = new SqlCommand();
        command = new SqlCommand("Select*from UrlData where LocationId='" + StrLocId + "'", connection);
        connection.Open();
        DataTable dt = new DataTable();
        dt.Load(command.ExecuteReader());
        connection.Close();
        if (LinkNumber.Text == "")
        {
            DepartmentList();
            LinkNumber.Text = (dt.Rows.Count + 1).ToString();

        }

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
            DataTable dtCust = (DataTable)Session["UrlGrid"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Url__Master"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            gvUrlMaster.DataSource = "";
            gvUrlMaster.DataSource = view;
            gvUrlMaster.DataBind();

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
            DataTable dtCust = (DataTable)Session["dtbinUrlData"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtUrlbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            // objPageCmn.FillData((object)gvBankMasterBin, view.ToTable(), "", "");
            gvUrlMasterBin.DataSource = view;
            gvUrlMasterBin.DataBind();
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
        foreach (GridViewRow gvrow in gvUrlMasterBin.Rows)
        {
            index = (int)gvUrlMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvUrlMasterBin.Rows)
            {
                int index = (int)gvUrlMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvUrlMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvUrlMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvUrlMasterBin, dt, "", "");

        PopulateCheckedValuesemplog();
    }
    protected void gvUrlMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = FillGridBin();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvUrlMasterBin, dt, "", "");

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string StrCompId = HttpContext.Current.Session["CompId"].ToString();
        string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
        string StrLocId = HttpContext.Current.Session["LocId"].ToString();
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        int b = 0;
        if (LinkDescription.Text == "")
        {
            DisplayMessage("Enter LinkDescription Name");
            LinkDescription.Focus();
            return;
        }
        if (LinkUrl.Text == "")
        {
            DisplayMessage("Enter LinkUrl Name");
            LinkDescription.Focus();
            return;
        }
        if (LinkHeader.Text == "")
        {
            DisplayMessage("Enter LinkHeader Name");
            LinkDescription.Focus();
            return;
        }
        if (LinkNumber.Text == "")
        {
            DisplayMessage("Enter LinkNumber Name");
            LinkDescription.Focus();
            return;
        }
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {

            if (Lbl_Tab_New.Text == "New")
            {

                SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
                connection.Close();
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("Select*from UrlData where LinkNumber='" + LinkNumber.Text + "' And LocationId='" + StrLocId + "'", connection);
                connection.Open();
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                connection.Close();
                if (dt.Rows.Count == 1)
                {
                    DisplayMessage("Link Number already Exist", "red");
                }
                else
                {
                    b = InsertUrlMaster(StrCompId, StrBrandId, StrLocId, LinkNumber.Text, LinkHeader.Text, LinkUrl.Text, LinkDescription.Text, Department.SelectedValue, "", "", "", "", "", "", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                if (b != 0)
                {

                    DisplayMessage("Record Saved", "green");
                    btnList_Click(null, null);
                    SelectRow();
                    // Response.Redirect(Page.Request.Path);
                    Reset();
                }
                else
                {
                    DisplayMessage("Record not Saved", "red");
                }
            }
            else
            {

                if (LinkNumber.Text == Link.Text)
                {
                    b = UpdateUrlMaster(Trans.Text, LinkNumber.Text, LinkHeader.Text, LinkUrl.Text, LinkDescription.Text, Department.SelectedValue, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    if (b != 0)
                    {
                        DisplayMessage("Record Update Successfull");
                        btnList_Click(null, null);
                        Reset();
                        SelectRow();
                    }
                    else
                    {
                        DisplayMessage("Record Not Update ");
                    }


                }
                else
                {
                    SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
                    connection.Close();
                    SqlCommand command = new SqlCommand();
                    command = new SqlCommand("Select LinkNumber from UrlData where LinkNumber='" + LinkNumber.Text + "'  And LocationId='" + StrLocId + "'", connection);
                    connection.Open();
                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());
                    connection.Close();
                    if (dt.Rows.Count > 0)
                    {
                        DisplayMessage("Link Number already Exist", "red");
                    }
                    else
                    {
                        b = UpdateUrlMaster(Trans.Text, LinkNumber.Text, LinkHeader.Text, LinkUrl.Text, LinkDescription.Text, Department.SelectedValue, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        if (b != 0)
                        {
                            DisplayMessage("Record Update Successfull");
                            btnList_Click(null, null);
                            Reset();
                        }
                        else
                        {
                            DisplayMessage("Record Not Update ");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }

    }
    public int UpdateUrlMaster(string TransId, string LinkNumber, string LinkHeader, string LinkUrl, string LinkDescription, string Department_Id, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        using (SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Set_UrlMaster_Update", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransId", SqlDbType.NVarChar).Value = TransId;
                cmd.Parameters.AddWithValue("@LinkNumber", SqlDbType.NVarChar).Value = LinkNumber;
                cmd.Parameters.AddWithValue("@LinkHeader", SqlDbType.NVarChar).Value = LinkHeader;
                cmd.Parameters.AddWithValue("@LinkUrl", SqlDbType.NVarChar).Value = LinkUrl;
                cmd.Parameters.AddWithValue("@LinkDescription", SqlDbType.NVarChar).Value = LinkDescription;
                cmd.Parameters.AddWithValue("@Department_Id", SqlDbType.NVarChar).Value = Department_Id;
                cmd.Parameters.AddWithValue("@Feild1", SqlDbType.NVarChar).Value = Field1;
                cmd.Parameters.AddWithValue("@Feild2", SqlDbType.NVarChar).Value = Field2;
                cmd.Parameters.AddWithValue("@Feild3", SqlDbType.NVarChar).Value = Field3;
                cmd.Parameters.AddWithValue("@Feild4", SqlDbType.NVarChar).Value = Field4;
                cmd.Parameters.AddWithValue("@Feild5", SqlDbType.NVarChar).Value = Field5;
                cmd.Parameters.AddWithValue("@Feild6", SqlDbType.NVarChar).Value = Field6;
                cmd.Parameters.AddWithValue("@Feild7", SqlDbType.NVarChar).Value = Field7;
                cmd.Parameters.AddWithValue("@IsActive", SqlDbType.Bit).Value = IsActive;
                cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.NVarChar).Value = ModifiedBy;
                cmd.Parameters.AddWithValue("@ModifiedDate", SqlDbType.DateTime).Value = ModifiedDate;
                cmd.Parameters.AddWithValue("@ReferenceID", SqlDbType.DateTime).Value = "1";
                con.Open();
                int i = cmd.ExecuteNonQuery();
                return i;
            }
        }

    }


    public int InsertUrlMaster(string CompanyId, string BrandId, string LocationId, string LinkNumber, string LinkHeader, string LinkUrl, string LinkDescription, string Department_Id, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        //SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        using (SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Set_UrlMaster_Insert", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompayId", SqlDbType.NVarChar).Value = CompanyId;
                cmd.Parameters.AddWithValue("@BrandId", SqlDbType.NVarChar).Value = BrandId;
                cmd.Parameters.AddWithValue("@LocationId", SqlDbType.NVarChar).Value = LocationId;
                cmd.Parameters.AddWithValue("@LinkNumber", SqlDbType.NVarChar).Value = LinkNumber;
                cmd.Parameters.AddWithValue("@LinkHeader", SqlDbType.NVarChar).Value = LinkHeader;
                cmd.Parameters.AddWithValue("@LinkUrl", SqlDbType.NVarChar).Value = LinkUrl;
                cmd.Parameters.AddWithValue("@LinkDescription", SqlDbType.NVarChar).Value = LinkDescription;
                cmd.Parameters.AddWithValue("@Department_Id", SqlDbType.NVarChar).Value = Department_Id;
                cmd.Parameters.AddWithValue("@Feild1", SqlDbType.NVarChar).Value = Field1;
                cmd.Parameters.AddWithValue("@Feild2", SqlDbType.NVarChar).Value = Field2;
                cmd.Parameters.AddWithValue("@Feild3", SqlDbType.NVarChar).Value = Field3;
                cmd.Parameters.AddWithValue("@Feild4", SqlDbType.NVarChar).Value = Field4;
                cmd.Parameters.AddWithValue("@Feild5", SqlDbType.NVarChar).Value = Field5;
                cmd.Parameters.AddWithValue("@Feild6", SqlDbType.NVarChar).Value = Field6;
                cmd.Parameters.AddWithValue("@Feild7", SqlDbType.NVarChar).Value = Field7;
                cmd.Parameters.AddWithValue("@IsActive", SqlDbType.Bit).Value = IsActive;
                cmd.Parameters.AddWithValue("@CreatedBy", SqlDbType.NVarChar).Value = CreatedBy;
                cmd.Parameters.AddWithValue("@CreatedDate", SqlDbType.DateTime).Value = CreatedDate;
                cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.NVarChar).Value = ModifiedBy;
                cmd.Parameters.AddWithValue("@ModifiedDate", SqlDbType.DateTime).Value = ModifiedDate;
                cmd.Parameters.AddWithValue("@ReferenceID", SqlDbType.DateTime).Value = "1";
                con.Open();
                int i = cmd.ExecuteNonQuery();
                return i;
            }
        }

    }
    //private string GetAccountId()
    //{
    //    string retval = string.Empty;
    //    if (txtAccountName.Text != "")
    //    {
    //        retval = (txtAccountName.Text.Split('/'))[txtAccountName.Text.Split('/').Length - 1];

    //        DataTable dtCOA = objCOA.GetCOAByTransId(Session["CompId"].ToString(), retval);
    //        if (dtCOA.Rows.Count > 0)
    //        {

    //        }
    //        else
    //        {
    //            retval = "";
    //        }
    //    }
    //    else
    //    {
    //        retval = "";
    //    }
    //    return retval;
    //}
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        string TransId = e.CommandArgument.ToString();
        SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        connection.Close();
        SqlCommand command = new SqlCommand();
        command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 2;
        command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = TransId;
        command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = 1;
        command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = 1;
        command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = 1;
        command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = 1;
        connection.Open();
        DataTable dt = new DataTable();
        dt.Load(command.ExecuteReader());
        DepartmentList();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            LinkNumber.Text = dt.Rows[i]["LinkNumber"].ToString();
            LinkHeader.Text = dt.Rows[i]["LinkHeader"].ToString();
            LinkUrl.Text = dt.Rows[i]["LinkUrl"].ToString();
            LinkDescription.Text = dt.Rows[i]["LinkDescription"].ToString();
            Trans.Text = dt.Rows[i]["TransId"].ToString();
            Link.Text = dt.Rows[i]["LinkNumber"].ToString();

            if (dt.Rows[i]["Department_Id"].ToString() != null && dt.Rows[i]["Department_Id"].ToString() != "" && dt.Rows[i]["Department_Id"].ToString() != "0")
            {
                Department.SelectedValue = dt.Rows[i]["Department_Id"].ToString();
            }
            else
            {
                DepartmentList();
            }
        }

        btnNew_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
    }
    protected void btnView_Command(object sender, CommandEventArgs e)
    {

        string TransId = e.CommandArgument.ToString();
        SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        connection.Close();
        SqlCommand command = new SqlCommand();
        command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 2;
        command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = TransId;
        command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = 1;
        command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = 1;
        command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = 1;
        command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = 1;
        connection.Open();

        DataTable dt = new DataTable();

        dt.Load(command.ExecuteReader());
        DepartmentList();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            LinkNumber.Text = dt.Rows[i]["LinkNumber"].ToString();
            LinkHeader.Text = dt.Rows[i]["LinkHeader"].ToString();
            LinkUrl.Text = dt.Rows[i]["LinkUrl"].ToString();
            LinkDescription.Text = dt.Rows[i]["LinkDescription"].ToString();
            Trans.Text = dt.Rows[i]["TransId"].ToString();
            Link.Text = dt.Rows[i]["LinkNumber"].ToString();
            Department.SelectedValue = dt.Rows[i]["Department_Id"].ToString();
        }
        btnNew_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.View;
        btnSave.Visible = false;
        btnReset.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
    }
    //protected void txtAccountName_TextChanged(object sender, EventArgs e)
    //{
    //    if (txtAccountName.Text != "")
    //    {
    //        DataTable dtAccountName = objCOA.GetCOAAll(Session["CompId"].ToString());
    //        string retval = string.Empty;
    //        if (txtAccountName.Text != "")
    //        {
    //            string strAccountName = txtAccountName.Text.Trim().Split('/')[0].ToString();
    //            dtAccountName = new DataView(dtAccountName, "AccountName='" + strAccountName + "' ", "", DataViewRowState.CurrentRows).ToTable();
    //            if (dtAccountName.Rows.Count > 0)
    //            {
    //                retval = (txtAccountName.Text.Split('/'))[txtAccountName.Text.Split('/').Length - 1];
    //            }
    //            else
    //            {
    //                retval = "";
    //            }
    //        }
    //        else
    //        {
    //            retval = "";
    //        }

    //        if (retval != "0" && retval != "")
    //        {
    //            string strTransId = GetAccountId();
    //            DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strTransId);
    //            if (dtAccount.Rows.Count > 0)
    //            {
    //                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
    //            }
    //            else
    //            {
    //                txtAccountName.Text = "";
    //                DisplayMessage("Choose In Suggestions Only");
    //                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            DisplayMessage("Select In Suggestions Only");
    //            txtAccountName.Text = "";
    //            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
    //        }
    //    }
    //    else
    //    {
    //        DisplayMessage("Enter Account Name");
    //        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
    //    }
    //}
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string TransId = e.CommandArgument.ToString();
        int b = 0;
        b = DeleteRecord(TransId, false, Session["UserId"].ToString(), DateTime.Now.ToString());

        if (b != 0)
        {
            DisplayMessage("Record Delete Successfull");
        }
        SelectRow();
        Reset();
        GetDeleteRecord();
    }
    public int DeleteRecord(string TransId, bool IsActive, string ModifiedBy, string ModifiedDate)
    {
        using (SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Set_UrlMaster_RowStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransId", SqlDbType.NVarChar).Value = TransId;
                cmd.Parameters.AddWithValue("@IsActive", SqlDbType.Bit).Value = IsActive;
                cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.NVarChar).Value = ModifiedBy;
                cmd.Parameters.AddWithValue("@ModifiedDate", SqlDbType.DateTime).Value = ModifiedDate;
                cmd.Parameters.AddWithValue("@ReferenceID", SqlDbType.DateTime).Value = "1";
                con.Open();
                int i = cmd.ExecuteNonQuery();
                return i;
            }
        }
    }
    public void GetDeleteRecord()
    {
        string StrCompId = HttpContext.Current.Session["CompId"].ToString();
        string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
        string StrLocId = HttpContext.Current.Session["LocId"].ToString();
        string UserId = HttpContext.Current.Session["UserId"].ToString();
        if (UserId == "superadmin")
        {
            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 3;
            command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = StrCompId;
            command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = StrLocId;
            command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = StrBrandId;
            connection.Open();
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
            Session["dtbinUrlData"] = dt;
            Session["dtUrlbinFilter"] = dt;
            gvUrlMasterBin.DataSource = dt;
            gvUrlMasterBin.DataBind();
            connection.Close();
        }
        else
        {
            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 6;
            command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId;
            command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = StrCompId;
            command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = StrLocId;
            command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = StrBrandId;
            connection.Open();
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
            Session["dtbinUrlData"] = dt;
            Session["dtUrlbinFilter"] = dt;
            gvUrlMasterBin.DataSource = dt;
            gvUrlMasterBin.DataBind();
            connection.Close();
        }

    }
    public void SelectRow()
    {
        string StrCompId = HttpContext.Current.Session["CompId"].ToString();
        string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
        string StrLocId = HttpContext.Current.Session["LocId"].ToString();
        string UserId = HttpContext.Current.Session["UserId"].ToString();
        if (UserId == "superadmin")
        {
            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = StrCompId;
            command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = StrLocId;
            command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = StrBrandId;

            connection.Open();

            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            Session["UrlGrid"] = dt;
            Session["dtFilter_Url__Master"] = dt;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
            gvUrlMaster.DataSource = dt;
            gvUrlMaster.DataBind();
            connection.Close();
        }
        else
        {
            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 5;
            command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId;
            command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = StrCompId;
            command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = StrLocId;
            command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = StrBrandId;

            connection.Open();

            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            Session["UrlGrid"] = dt;
            Session["dtFilter_Url__Master"] = dt;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
            gvUrlMaster.DataSource = dt;
            gvUrlMaster.DataBind();
            connection.Close();
        }
    }





    protected void gvUrlMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUrlMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Bank__Master"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvUrlMaster, dt, "", "");


    }
    protected void gvUrlMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Bank__Master"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Bank__Master"] = dt;
        gvUrlMaster.DataSource = dt;
        gvUrlMaster.DataBind();

    }

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListBankCode(string prefixText, int count, string contextKey)
    //{
    //    Set_BankMaster objBankMaster = new Set_BankMaster(HttpContext.Current.Session["DBConnection"].ToString());
    //    DataTable dt = new DataView(objBankMaster.GetBankMasterBoth(), "Bank_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
    //    string[] txt = new string[dt.Rows.Count];

    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        txt[i] = dt.Rows[i]["Bank_Code"].ToString();
    //    }
    //    return txt;
    //}
    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListBankName(string prefixText, int count, string contextKey)
    //{
    //    Set_BankMaster objBankMaster = new Set_BankMaster(HttpContext.Current.Session["DBConnection"].ToString());
    //    DataTable dt = new DataView(objBankMaster.GetBankMasterBoth(), "Bank_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
    //    string[] txt = new string[dt.Rows.Count];

    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        txt[i] = dt.Rows[i]["Bank_Name"].ToString();
    //    }
    //    return txt;
    //}
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
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
        string StrCompId = HttpContext.Current.Session["CompId"].ToString();
        string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
        string StrLocId = HttpContext.Current.Session["LocId"].ToString();
        string UserId = HttpContext.Current.Session["UserId"].ToString();
        if (UserId == "superadmin")
        {
            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = StrCompId;
            command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = StrLocId;
            command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = StrBrandId;

            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            Session["UrlGrid"] = dt;
            Session["dtFilter_Url__Master"] = dt;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
            gvUrlMaster.DataSource = dt;
            gvUrlMaster.DataBind();
            connection.Close();
        }
        else
        {
            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 5;
            command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId;
            command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = StrCompId;
            command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = StrLocId;
            command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = StrBrandId;

            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            Session["UrlGrid"] = dt;
            Session["dtFilter_Url__Master"] = dt;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
            gvUrlMaster.DataSource = dt;
            gvUrlMaster.DataBind();
            connection.Close();
        }

    }
    public DataTable FillGridBin()
    {
        string StrCompId = HttpContext.Current.Session["CompId"].ToString();
        string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
        string StrLocId = HttpContext.Current.Session["LocId"].ToString();
        string UserId = HttpContext.Current.Session["UserId"].ToString();
        if (UserId == "superadmin")
        {
            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 3;
            command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = StrCompId;
            command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = StrLocId;
            command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = StrBrandId;
            connection.Open();
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
            Session["dtbinUrlData"] = dt;
            Session["dtUrlbinFilter"] = dt;
            gvUrlMasterBin.DataSource = dt;
            gvUrlMasterBin.DataBind();
            connection.Close();

            return dt;
        }
        else
        {
            SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("sp_Set_UrlMaster_SelectRow", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 6;
            command.Parameters.Add("@TransId", SqlDbType.VarChar).Value = 1;
            command.Parameters.Add("@UserId", SqlDbType.VarChar).Value = UserId;
            command.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = StrCompId;
            command.Parameters.Add("@LocationId", SqlDbType.VarChar).Value = StrLocId;
            command.Parameters.Add("@BrandId", SqlDbType.VarChar).Value = StrBrandId;
            connection.Open();
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
            Session["dtbinUrlData"] = dt;
            Session["dtUrlbinFilter"] = dt;
            gvUrlMasterBin.DataSource = dt;
            gvUrlMasterBin.DataBind();
            connection.Close();

            return dt;

        }
    }

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    //{
    //    Ac_ChartOfAccount objCOA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
    //    DataTable dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

    //    string filtertext = "AccountName like '" + prefixText + "%'";
    //    dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

    //    string[] txt = new string[dtCOA.Rows.Count];

    //    if (dtCOA.Rows.Count > 0)
    //    {
    //        for (int i = 0; i < dtCOA.Rows.Count; i++)
    //        {
    //            txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
    //        }
    //    }
    //    else
    //    {
    //        if (prefixText.Length > 2)
    //        {
    //            txt = null;
    //        }
    //        else
    //        {
    //            dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
    //            if (dtCOA.Rows.Count > 0)
    //            {
    //                txt = new string[dtCOA.Rows.Count];
    //                for (int i = 0; i < dtCOA.Rows.Count; i++)
    //                {
    //                    txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
    //                }
    //            }
    //        }
    //    }
    //    return txt;
    //}
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)gvUrlMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvUrlMasterBin.Rows)
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

                if (!userdetails.Contains(dr["TransId"]))
                {
                    userdetails.Add(dr["TransId"]);
                }
            }
            foreach (GridViewRow GR in gvUrlMasterBin.Rows)
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
            objPageCmn.FillData((object)gvUrlMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }

    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvUrlMasterBin.Rows.Count > 0)
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
                            b = RestoreData(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
                    foreach (GridViewRow Gvr in gvUrlMasterBin.Rows)
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
                gvUrlMasterBin.Focus();
                return;
            }
        }

    }
    public int RestoreData(string TransId, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        using (SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand("sp_Set_UrlMaster_RowStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TransId", SqlDbType.NVarChar).Value = TransId;
                cmd.Parameters.AddWithValue("@IsActive", SqlDbType.Bit).Value = IsActive;
                cmd.Parameters.AddWithValue("@ModifiedBy", SqlDbType.NVarChar).Value = ModifiedBy;
                cmd.Parameters.AddWithValue("@ModifiedDate", SqlDbType.DateTime).Value = ModifiedDate;
                cmd.Parameters.AddWithValue("@ReferenceID", SqlDbType.DateTime).Value = "1";
                con.Open();
                int i = cmd.ExecuteNonQuery();
                return i;
            }
        }
    }

    public void Reset()
    {
        LinkNumber.Text = "";
        LinkHeader.Text = "";
        LinkUrl.Text = "";
        LinkDescription.Text = "";
        Trans.Text = "";
        Link.Text = "";
        Session["CHECKED_ITEMS"] = null;
        //txtAddressName.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        btnSave.Visible = true;
        btnReset.Visible = true;
        btnNew_Click(null, null);
        // GvAddressName.DataSource = null;
        // GvAddressName.DataBind();

    }

}
