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
using System.Web.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using PegasusDataAccess;

public partial class Transport_VehicleExpenses : System.Web.UI.Page
{
    Common cmn = null;
    SystemParameter objSys = null;
    tp_Vehicle_Expenses objVehicleExpenses = null;
    IT_ObjectEntry objObjectEntry = null;
    EmployeeMaster objEmpmaster = null;
    Prj_VehicleMaster ObjVehicle = null;
    Inv_UnitMaster objUnit = null;
    Inv_ProductMaster ObjProductMaster = null;
    DataAccessClass objda = null;
    Ems_ContactMaster objContact = null;
    Inv_AdjustHeader objAdjustHeader = null;
    Inv_AdjustDetail objAdjustDetail = null;
    Inv_ProductLedger ObjProductledger = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    Set_DocNumber objDocNo = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    tp_Vehicle_Ledger ObjVehicleLedger = null;
    Set_Suppliers objSupplier = null;
    PageControlCommon objPageCmn = null;

    public bool IsEdit = false;
    public bool IsDelete = false;
    public bool IsEditInitialize = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CompId"] == null || Session["BrandId"] == null || Session["LocId"] == null || Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objVehicleExpenses = new tp_Vehicle_Expenses(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objEmpmaster = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjVehicle = new Prj_VehicleMaster(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objAdjustHeader = new Inv_AdjustHeader(Session["DBConnection"].ToString());
        objAdjustDetail = new Inv_AdjustDetail(Session["DBConnection"].ToString());
        ObjProductledger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        ObjVehicleLedger = new tp_Vehicle_Ledger(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Transport/VehicleExpenses.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            FillPageSizeddl();
            HttpContext.Current.Session["IndexNo"] = 1;
            txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;
            FillGridBin();
            gvExpensesMaster.PageSize = int.Parse(Session["GridSize"].ToString());
            FillGrid();
            btnList_Click(null, null);
            Calender.Format = objSys.SetDateFormat();
            txtTransdate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
            PopulateData();
            CompanyMaster companyMaster = new CompanyMaster(Session["DBConnection"].ToString());
            DataTable companydt = companyMaster.GetCompanyMaster();
            AddpagingButton();
        }
        else
        {
            int currentpagesize = int.Parse(ddlPageSize.SelectedItem.Text.ToString());
            if (int.Parse(Session["GridSize"].ToString()) == currentpagesize)
            {
                ViewState["NoOfRecord"] = int.Parse(Session["GridSize"].ToString());
            }
            else
            {
                ViewState["NoOfRecord"] = currentpagesize;
            }
            AddpagingButton();
            AddpagingButtonMileage();
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowDatatables();", true);
        //AllPageCode();
    }


    #region Calculation

    protected void txtQty_OnTextChanged(object sender, EventArgs e)
    {
        txtAmount.Text = "0";
        double currentPumpReading = 0;
        double.TryParse(txtPumpStartReading.Text, out currentPumpReading);
        try
        {
            if (ddlCategory.SelectedValue == "Diesel")
            {
                if (txtQty.Text != "0" && txtQty.Text != "")
                {
                    double qty = 0;
                    double pumpStartReading = 0;
                    double.TryParse(txtQty.Text, out qty);
                    double.TryParse(txtPumpStartReading.Text, out pumpStartReading);
                    txtPumpEndReading.Text = (qty + pumpStartReading).ToString("0.00");

                }
            }
            txtAmount.Text = Common.GetAmountDecimal((Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtRate.Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
        }
        catch
        {

        }


    }
    #endregion

    private void FillPageSizeddl()
    {
        ddlPageSize.Items.Add(new ListItem("10", "10"));
        ddlPageSize.Items.Add(new ListItem("20", "20"));
        ddlPageSize.Items.Add(new ListItem("30", "30"));
        ddlPageSize.Items.Add(new ListItem("40", "40"));
        ddlPageSize.Items.Add(new ListItem("50", "50"));
        ddlPageSize.Items.Add(new ListItem("60", "60"));
        ddlPageSize.Items.Add(new ListItem("70", "70"));
        ddlPageSize.Items.Add(new ListItem("80", "80"));
        ddlPageSize.Items.Add(new ListItem("90", "90"));
        ddlPageSize.Items.Add(new ListItem("100", "100"));
        int StrPageSize = (int)int.Parse(Session["GridSize"].ToString());
        if (String.IsNullOrEmpty(StrPageSize.ToString()))
        {
            StrPageSize = 1;
        }
        else
        {
            if (StrPageSize % 10 != 0)
            {
                StrPageSize = StrPageSize + (10 - (StrPageSize % 10));
            }
        }
        ddlPageSize.SelectedValue = StrPageSize.ToString();
    }

    protected void ddlCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtPreReading.Text = "0";
        txtCurrentReading.Text = "0";
        txtPumpStartReading.Text = "0";
        txtPumpEndReading.Text = "0";
        CategorySelection();
        refreshData();
    }

    protected void CategorySelection()
    {
        lblTankFull.Visible = true;
        ddlTankFull.Visible = true;
        lblPSReading.Visible = true;
        lblPEReading.Visible = true;
        PSReading.Visible = true;
        PEReading.Visible = true;
        txtPumpStartReading.Visible = true;
        txtPumpEndReading.Visible = true;
        PreReading.Visible = true;
        CurrentReading.Visible = true;
        RFVPreReading.Enabled = true;
        RFVCurrentReading.Enabled = true;
        Unit.Visible = true;
        Qty.Visible = true;
        RFVUnit.Enabled = true;
        RFVQty.Enabled = true;
        txtRate.Enabled = true;


        Rate.Visible = true;
        RFVRate.Enabled = true;
        txtAmount.Enabled = false;
        Amount.Visible = false;
        RFVAmount.Enabled = false;

        if (ddlCategory.SelectedValue == "Inventory")
        {
            lblTankFull.Visible = false;
            ddlTankFull.Visible = false;
            lblPSReading.Visible = false;
            lblPEReading.Visible = false;
            PSReading.Visible = false;
            PEReading.Visible = false;
            txtPumpStartReading.Visible = false;
            txtPumpEndReading.Visible = false;
            PreReading.Visible = false;
            CurrentReading.Visible = false;
            RFVPreReading.Enabled = false;
            RFVCurrentReading.Enabled = false;

            Unit.Visible = false;
            RFVUnit.Enabled = false;
        }
        else if (ddlCategory.SelectedValue == "Miscellaneous")
        {
            Unit.Visible = false;
            Qty.Visible = false;
            RFVUnit.Enabled = false;
            RFVQty.Enabled = false;

            lblTankFull.Visible = false;
            ddlTankFull.Visible = false;
            lblPSReading.Visible = false;
            lblPEReading.Visible = false;
            PSReading.Visible = false;
            PEReading.Visible = false;
            txtPumpStartReading.Visible = false;
            txtPumpEndReading.Visible = false;
            PreReading.Visible = false;
            CurrentReading.Visible = false;
            RFVPreReading.Enabled = false;
            RFVCurrentReading.Enabled = false;
            txtRate.Enabled = false;

            Rate.Visible = false;
            RFVRate.Enabled = false;
            txtAmount.Enabled = true;
            Amount.Visible = true;
            RFVAmount.Enabled = true;
        }
    }

    protected void ddlPageSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        int currentpagesize = int.Parse(ddlPageSize.SelectedItem.Text.ToString());
        DataTable dt = objVehicleExpenses.GetAllTrueRecordByIndex(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["IndexNo"].ToString(), currentpagesize.ToString(), ddlPosted.SelectedValue);
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString();
        //gvVehicleMaster.PageIndex = currentpagesize;
        gvExpensesMaster.PageSize = currentpagesize;
        gvExpensesMaster.DataSource = dt;
        gvExpensesMaster.DataBind();
        //AllPageCode();
        Session["dtFilter_Vehicle_Exp"] = dt;
        ViewState["NoOfRecord"] = currentpagesize;
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "SetPageSizeInDt();", true);
    }

    private void PopulateData()
    {
        DataTable dt = objVehicleExpenses.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ViewState["TotalRecord"] = dt.Rows.Count;
        int currentpagesize = int.Parse(ddlPageSize.SelectedItem.Text.ToString());
        if (int.Parse(Session["GridSize"].ToString()) == currentpagesize)
        {
            ViewState["NoOfRecord"] = int.Parse(Session["GridSize"].ToString());
        }
        else
        {
            ViewState["NoOfRecord"] = currentpagesize;
        }
    }

    private void AddpagingButton()
    {
        // this method for generate custom button for Custom paging in Gridview
        int totalRecord = 0;
        int noofRecord = 0;
        totalRecord = ViewState["TotalRecord"] != null ? (int)ViewState["TotalRecord"] : 0;
        noofRecord = ViewState["NoOfRecord"] != null ? (int)ViewState["NoOfRecord"] : 0;
        int pages = 0;
        if (totalRecord > 0 && noofRecord > 0)
        {
            // Count no of pages 
            pages = (totalRecord / noofRecord) + ((totalRecord % noofRecord) > 0 ? 1 : 0);
            //if (totalRecord < noofRecord)
            //{
            //    pages = pages + 1;
            //}

            for (int i = 0; i < pages; i++)
            {
                Button b = new Button();
                b.CssClass = "pagingbtn";
                b.Text = (i + 1).ToString();
                b.CommandArgument = (i + 1).ToString();
                b.ID = "Button_" + (i + 1).ToString();
                b.Click += new EventHandler(this.b_click);
                // b.OnClientClick = "RemoveSelectedCss();";
                PagingPanel.Controls.Add(b);
            }
        }

    }

    protected void b_click(object sender, EventArgs e)
    {
        // this is for Get data from Database on button (paging button) click
        string pageNo = ((Button)sender).CommandArgument;
        HttpContext.Current.Session["IndexNo"] = pageNo;
        FillGrid();
        PopulateData();
        Button btn = sender as Button;
        //btn.CssClass = "pagingbtn-sel";
        // Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "SetSelectedCss(" + btn.Text + ");", true);
        // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "SetSelectedCss(" + btn.Text + ");", true);
        // string functionname = "<script type='text/javascript'>SetSelectedCss("+btn.Text+") { alert(no); $('#ctl00_MainContent_PagingPanel').find('input[type='submit']').each(function () { alert($(this).val()); $(this).removeClass('pagingbtn-sel');}); }</script>";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "myfunction", "SetSelectedCss(" + btn.Text + ");", true);
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        btn_Post.Visible = clsPagePermission.bEdit;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        txtValue.Focus();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtvehiclename.Focus();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
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
        txtValue.Focus();
    }

    protected void txtvehiclename_TextChanged(object sender, EventArgs e)
    {
        refreshData();
    }

    protected void refreshData()
    {
        txtEmpName.Text = "";
        txtOtherAccountNo.Text = "";
        //string strTrnsDate=string.Empty
        if (IsEditInitialize)
        {
            return;
        }
        if (txtvehiclename.Text.Trim() != "")
        {


            DataTable dt = ObjVehicle.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                //dt = new DataView(dt, "Name='" + txtvehiclename.Text.Trim().Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                dt = new DataView(dt, "Vehicle_Id='" + Get_String_After_Char(txtvehiclename.Text.Trim(), '/') + "'", "", DataViewRowState.CurrentRows).ToTable();
                string driverdetail = dt.Rows[0]["Field2"].ToString() + "/0";
                txtEmpName.Text = driverdetail;
            }
            catch
            {
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Vehicle not found");
                txtvehiclename.Text = "";
                txtvehiclename.Focus();
                //txtPumpStartReading.Text = "0";
                //txtPumpEndReading.Text = "0";
                //txtCurrentReading.Text = "0";
                //txtPreReading.Text = "0";

                return;
            }
            else
            {
                if (dt.Rows[0]["Emp_id"].ToString() != "0")
                {
                    txtEmpName.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_id"].ToString();
                }

                //here we are getting other account no if vehicle hired according signed contract 


                //string strsql = "select Contractor_Id,Field3,Field4,From_Date,To_Date from tp_Vehicle_contract where Vehicle_Id='" + txtvehiclename.Text.Split('/')[1].ToString() + "' and Contract_Type='In' and Field1='True' and ('" + Convert.ToDateTime(txtTransdate.Text) + "' between From_Date And To_Date)";

                string strsql = "select Contractor_Id,Field3,Field4,From_Date,To_Date from tp_Vehicle_contract where Vehicle_Id='" + Get_String_After_Char(txtvehiclename.Text.Trim(), '/') + "' and Field1='True' and ('" + Convert.ToDateTime(txtTransdate.Text) + "' between From_Date And To_Date)";

                dt = objda.return_DataTable(strsql);
                DataTable dtTemp = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    txtOtherAccountNo.Text = dt.Rows[0]["Field3"].ToString().Trim() + "/" + dt.Rows[0]["Contractor_Id"].ToString().Trim();
                    dtTemp = objda.return_DataTable("select current_reading from tp_Vehicle_Expenses where Trans_date between '" + Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString()) + "' And '" + Convert.ToDateTime(dt.Rows[0]["To_Date"].ToString()) + "' and Field1='" + ddlCategory.SelectedValue + "' and Vehicle_Id='" + Get_String_After_Char(txtvehiclename.Text.Trim(), '/') + "' order by Trans_date,trans_id desc");
                    if (dtTemp.Rows.Count > 0)
                    {
                        txtPreReading.Text = dtTemp.Rows[0]["current_reading"].ToString().Trim();
                    }
                    else
                    {
                        txtPreReading.Text = dt.Rows[0]["Field4"].ToString().Trim();
                    }

                }
                else
                {
                    dtTemp = objda.return_DataTable("select top 1 current_reading from tp_Vehicle_Expenses where IsActive='true' and  Trans_date <= '" + Convert.ToDateTime(txtTransdate.Text) + "' And  Field1='" + ddlCategory.SelectedValue + "' and Vehicle_Id='" + Get_String_After_Char(txtvehiclename.Text.Trim(), '/') + "' order by Trans_date,trans_id desc");
                    if (dtTemp.Rows.Count > 0)
                    {
                        txtPreReading.Text = dtTemp.Rows[0]["current_reading"].ToString().Trim();
                    }
                    else
                    {
                        //txtPreReading.Text = dt.Rows[0]["Field4"].ToString().Trim();
                    }
                }

                // get diseal filling detail
                //if (ddlCategory.SelectedValue == "Diesel")
                //{
                //    dtTemp = objda.return_DataTable("select top 1 Pump_End_Reading from tp_Vehicle_Expenses where IsActive='true' and Trans_date <= '" + Convert.ToDateTime(txtTransdate.Text) + "' And  Field1='Diesel' and Vehicle_Id='" + Get_String_After_Char(txtvehiclename.Text.Trim(), '/') + "' order by Trans_date,trans_id desc");
                //    if (dtTemp.Rows.Count > 0)
                //    {
                //        txtPumpStartReading.Text = dtTemp.Rows[0]["Pump_End_Reading"].ToString().Trim();
                //    }
                //    else
                //    {
                //        txtPumpStartReading.Text = "0";
                //    }
                //    if (txtQty.Text != "0" && txtQty.Text != "")
                //    {
                //        double qty = 0;
                //        double pumpStartReading = 0;
                //        double.TryParse(txtQty.Text, out qty);
                //        double.TryParse(txtPumpStartReading.Text, out pumpStartReading);
                //        txtPumpEndReading.Text = (qty + pumpStartReading).ToString("0.00");

                //    }
                //}

                //dtTemp.Dispose();
                dt.Dispose();
            }

        }
        txtQty.Focus();
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string PostingType = string.Empty;
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
            //DataTable dtCust = (DataTable)Session["Country"];
            DataTable dtExpenses = objVehicleExpenses.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            DataView view = null;
            if (ddlPosted.SelectedIndex == 0)
            {
                PostingType = "Field4 = 'true'";
            }
            else if (ddlPosted.SelectedIndex == 1)
            {
                PostingType = "Field4 = 'false'";
            }
            else
            {
                PostingType = "Field4 in ('false','true')";
            }
            view = new DataView(dtExpenses, condition, "", DataViewRowState.CurrentRows);
            view = new DataView(view.ToTable(), PostingType, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Vehicle_Exp"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvExpensesMaster, view.ToTable(), "", "");
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
            DataTable dtCust = (DataTable)Session["dtbinCountry"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)gvExpensesMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                // imgBtnRestore.Visible = false;
                // ImgbtnSelectAll.Visible = false;
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
        FillGrid();
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvExpensesMasterBin.Rows)
        {
            index = (int)gvExpensesMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvExpensesMasterBin.Rows)
            {
                int index = (int)gvExpensesMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    //protected void gvExpensesMasterBin_OnPreRender(object sender, EventArgs e)
    //{
    //    // You only need the following 2 lines of code if you are not 
    //    // using an ObjectDataSource of SqlDataSource
    //    DataTable dt = (DataTable)Session["dtbinFilter"];
    //    gvExpensesMasterBin.DataSource = dt;
    //    gvExpensesMasterBin.DataBind();
    //    if (gvExpensesMasterBin.Rows.Count > 0)
    //    {
    //        //This replaces <td> with <th> and adds the scope attribute
    //        gvExpensesMasterBin.UseAccessibleHeader = true;

    //        //This will add the <thead> and <tbody> elements
    //        gvExpensesMasterBin.HeaderRow.TableSection = TableRowSection.TableHeader;

    //    }
    //}
    protected void gvExpensesMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvExpensesMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvExpensesMasterBin, dt, "", "");

        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void gvExpensesMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objVehicleExpenses.GetAllFalseRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvExpensesMasterBin, dt, "", "");

        //AllPageCode();
        gvExpensesMasterBin.HeaderRow.Focus();
    }
    protected void Btn_Post_Click(object sender, EventArgs e)
    {
        btnSave_Click(sender, e);
        //AllPageCode();
    }

    public static string Get_String_After_Char(string input, char pivot)
    {
        int index = input.IndexOf(pivot);
        if (index >= 0)
        {
            return input.Substring(index + 1);
        }
        return input;
    }

    public static string Get_String_Before_Char(string input, char pivot)
    {
        int index = input.IndexOf(pivot);
        if (index >= 0)
        {
            return input.Substring(0, index);
        }
        return input;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        string Dr_Account_No = "0";
        string Cr_Account_No = "0";
        string Dr_OtherAccount_No = "0";
        string Cr_OtherAccount_No = "0";
        string Millage = "0";
        string strOtherAccountNo = "0";

        bool IsPost = false;

        if (((Button)sender).ID.Trim() == "btn_Post")
        {
            IsPost = true;
        }
        if (ddlCategory.SelectedValue == "Diesel")
        {
            double ps = double.Parse(txtPumpStartReading.Text.ToString());
            double pe = double.Parse(txtPumpEndReading.Text.ToString());
            if (ps > pe)
            {
                DisplayMessage("Pump start reading can not be greater than end reading");
                return;
            }
            double cr = double.Parse(txtPreReading.Text.ToString());
            double pr = double.Parse(txtCurrentReading.Text.ToString());
            if (cr > pr)
            {
                DisplayMessage("Pre reading can not be greater than current reading");
                return;
            }
        }
        if (chkAccountEntry.Checked)
        {
            //if (txtpaymentdebitaccount.Text.Trim() == "")
            //{
            //    DisplayMessage("Enter Debit Account");
            //    txtpaymentdebitaccount.Focus();
            //    return;
            //}
            //else
            //{
            //    Dr_Account_No = txtpaymentdebitaccount.Text.Split('/')[1].ToString();
            //}

            if (txtpaymentCreditaccount.Text.Trim() == "")
            {
                DisplayMessage("Enter Credit Account");
                txtpaymentCreditaccount.Focus();
                return;
            }
            else
            {
                Cr_Account_No = Get_String_After_Char(txtpaymentCreditaccount.Text.Trim(), '/');
            }

            if (Get_String_After_Char(txtpaymentCreditaccount.Text.Trim(), '/') == Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) || Get_String_After_Char(txtpaymentCreditaccount.Text.Trim(), '/') == Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()))
            {
                if (txtOtherAccountNo.Text == "")
                {
                    DisplayMessage("Enter Other Account No");
                    txtOtherAccountNo.Focus();
                    return;
                }
                strOtherAccountNo = Get_String_After_Char(txtOtherAccountNo.Text.Trim(), '/');
                Cr_OtherAccount_No = Get_String_After_Char(txtOtherAccountNo.Text.Trim(), '/');

            }


        }
        if (ddlCategory.SelectedIndex != 2 && txtProductName.Text == "")
        {
            DisplayMessage("Enter Product Name");
            txtProductName.Focus();
            return;
        }

        if (trSupplier.Visible == true)
        {
            if (txtSupplierName.Text != "")
            {

            }
            else
            {
                DisplayMessage("Enter Supplier Name");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
                return;
            }
        }
        ///Millage calculation if tank full

        if (ddlTankFull.SelectedIndex == 1 && ddlCategory.SelectedIndex == 0)
        {
            double prereading = 0;
            double currentReading = 0;
            double TotalDiesel = 0;

            try
            {
                prereading = Convert.ToDouble(txtPreReading.Text);
            }
            catch
            {
            }
            try
            {
                currentReading = Convert.ToDouble(txtCurrentReading.Text);
            }
            catch
            {
            }
            try
            {
                TotalDiesel = Convert.ToDouble(txtQty.Text);
            }
            catch
            {

            }


            Millage = Common.GetAmountDecimal(((currentReading - prereading) / TotalDiesel).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());

        }
        string Driverid = string.Empty;
        string ProductId = string.Empty;
        string VehicleId = string.Empty;
        string DriverName = string.Empty;
        string TankFull = string.Empty;
        string strUnitId = "0";
        string Category = ddlCategory.SelectedValue.ToString();
        int b = 0;
        string Qty = string.Empty;
        string Rate = string.Empty;
        string Amount = string.Empty;
        string PreReading = string.Empty;
        string CurrReading = string.Empty;
        string PuStReading = string.Empty;
        string PuEnReading = string.Empty;
        string strTransId = string.Empty;
        if (txtvehiclename.Text.Trim() == "")
        {
            DisplayMessage("Enter Vehicle Name");
            txtvehiclename.Focus();
            return;
        }

        if (txtTransdate.Text == "")
        {
            txtTransdate.Text = "1/1/1900";
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtTransdate.Text.Trim());
            }
            catch
            {
                txtTransdate.Text = "";
                DisplayMessage("Enter Valid Trans Date");
                txtTransdate.Focus();
                return;
            }
        }
        if (txtEmpName.Text != "")
        {
            Driverid = Get_String_After_Char(txtEmpName.Text.Trim(), '/');
            DriverName = Get_String_Before_Char(txtEmpName.Text.Trim(), '/');
            //Driverid = txtEmpName.Text.Split('/')[1].ToString();
            //DriverName = txtEmpName.Text.Split('/')[0].ToString();
        }
        else
        {
            Driverid = "0";
            DriverName = "";
        }

        if (!String.IsNullOrEmpty(txtvehiclename.Text))
        {
            VehicleId = Get_String_After_Char(txtvehiclename.Text.Trim(), '/');
        }
        else
        {
            VehicleId = "0";
        }


        if (txtProductName.Text != "")
        {
            ProductId = hdnProductId.Value;
            strUnitId = ddlUnit.SelectedValue;
        }
        else
        {
            ProductId = "0";
            strUnitId = "0";
        }

        if (!String.IsNullOrEmpty(txtQty.Text))
            Qty = txtQty.Text;
        else
            Qty = "0";

        if (!String.IsNullOrEmpty(txtRate.Text))
            Rate = txtRate.Text;
        else
            Rate = "0";

        if (!String.IsNullOrEmpty(txtAmount.Text))
            Amount = txtAmount.Text;
        else
            Amount = "0";

        if (!String.IsNullOrEmpty(txtPreReading.Text))
            PreReading = txtPreReading.Text;
        else
            PreReading = "0";

        if (!String.IsNullOrEmpty(txtCurrentReading.Text))
            CurrReading = txtCurrentReading.Text;
        else
            CurrReading = "0";

        if (!String.IsNullOrEmpty(txtPumpStartReading.Text))
            PuStReading = txtPumpStartReading.Text;
        else
            PuStReading = "0";

        if (!String.IsNullOrEmpty(txtPumpEndReading.Text))
            PuEnReading = txtPumpEndReading.Text;
        else
            PuEnReading = "0";
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        string strRefId = string.Empty;
        string Narration = string.Empty;
        int MaxId = 0;
        try
        {
            double PaidTotaamount = 0;

            try
            {
                PaidTotaamount = Convert.ToDouble(txtAmount.Text);
            }
            catch
            {

            }
            btnSave.Enabled = false;
            Narration = "Vehicle Expenses Entry For vehicle = " + Get_String_Before_Char(txtvehiclename.Text.Trim(), '/');
            //accounts entry
            if (((Button)sender).ID.Trim() == "btn_Post" && PaidTotaamount != 0 && chkAccountEntry.Checked)
            {

                //For Bank Account
                string strAccountId = string.Empty;
                DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", ref trns);
                if (dtAccount.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAccount.Rows.Count; i++)
                    {
                        if (strAccountId == "")
                        {
                            strAccountId = dtAccount.Rows[i]["Param_Value"].ToString();
                        }
                        else
                        {
                            strAccountId = strAccountId + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                        }
                    }
                }
                else
                {
                    strAccountId = "0";
                }

                string strCurrencyId = Session["LocCurrencyId"].ToString();


                //for Voucher Number
                string strVoucherNumber = objDocNo.GetDocumentNo(true, "0", false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                if (strVoucherNumber != "")
                {
                    int counter = objAcParameter.GetCounterforVoucherNumber1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "JV", Session["FinanceYearId"].ToString(), ref trns);


                    if (counter == 0)
                    {
                        strVoucherNumber = strVoucherNumber + "1";
                    }
                    else
                    {
                        strVoucherNumber = strVoucherNumber + (counter + 1);
                    }
                }

                MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), "0", "0", "0", "0", objSys.getDateForInput(txtTransdate.Text).ToString(), strVoucherNumber, objSys.getDateForInput(txtTransdate.Text).ToString(), "JV", "1/1/1800", "1/1/1800", "", Narration, strCurrencyId, "1", Narration, false.ToString(), false.ToString(), false.ToString(), "JV", "", "", "0", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strVMaxId = MaxId.ToString();


                string strCompAmount = PaidTotaamount.ToString();
                string strLocAmount = PaidTotaamount.ToString();
                string strForeignAmount = PaidTotaamount.ToString();
                string strForeignExchangerate = "1";



                //str for Employee Id
                //For Debit
                string VehicleAccountName = GetAccountNameByTransId(objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Vehicle Account").Rows[0]["Param_Value"].ToString());
                string VehicleAccountNo = Get_String_After_Char(VehicleAccountName.Trim(), '/');
                string strCompanyCrrValueDr = PaidTotaamount.ToString();
                string CompanyCurrDebit = Get_String_Before_Char(strCompanyCrrValueDr.Trim(), '/');
                //if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
                //{
                //    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", VehicleAccountNo, txtvehiclename.Text.Split('/')[1].ToString(), "0", "ID", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                //}
                //else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", VehicleAccountNo, Get_String_After_Char(txtvehiclename.Text.Trim(), '/'), "0", "ID", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), CompanyCurrDebit, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                //For Credit
                string strCompanyCrrValueCr = strCompanyCrrValueDr;
                string CompanyCurrCredit = Get_String_Before_Char(strCompanyCrrValueCr.Trim(), '/');
                if (String.IsNullOrEmpty(txtSupplierName.Text))
                {
                    Cr_OtherAccount_No = "0";
                }
                else
                {
                    Cr_OtherAccount_No = Get_String_After_Char(txtSupplierName.Text.Trim(), '/');
                }
                if (strAccountId.Split(',').Contains(Get_String_After_Char(txtpaymentCreditaccount.Text.Trim(), '/')))
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", Get_String_After_Char(txtpaymentCreditaccount.Text.Trim(), '/'), Cr_OtherAccount_No, "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", CompanyCurrCredit, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else
                {
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", Get_String_After_Char(txtpaymentCreditaccount.Text.Trim(), '/'), Cr_OtherAccount_No, "0", "EAP", "1/1/1800", "1/1/1800", "", "0.00", strLocAmount, Narration, "", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), strForeignExchangerate, PaidTotaamount.ToString(), "0.00", CompanyCurrCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

            }


            if (editid.Value == "")
            {
                b = objVehicleExpenses.InsertRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), VehicleId, txtTransdate.Text, ProductId, Qty, Rate, Amount, Driverid, DriverName, CurrReading, PreReading, PuStReading, PuEnReading, txtRemarks.Text, "0", ddlTankFull.SelectedValue.ToString(), Millage, MaxId.ToString(), Dr_Account_No, Dr_OtherAccount_No, Cr_Account_No, Cr_OtherAccount_No, Category, chkAccountEntry.Checked.ToString(), strUnitId, IsPost.ToString(), "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                strRefId = b.ToString();
            }
            else
            {
                b = objVehicleExpenses.UpdateRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), editid.Value, VehicleId, txtTransdate.Text, ProductId, Qty, Rate, Amount, Driverid, DriverName, CurrReading, PreReading, PuStReading, PuEnReading, txtRemarks.Text, "0", ddlTankFull.SelectedValue.ToString(), Millage, MaxId.ToString(), Dr_Account_No, Dr_OtherAccount_No, Cr_Account_No, Cr_OtherAccount_No, Category, chkAccountEntry.Checked.ToString(), strUnitId, IsPost.ToString(), "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                strRefId = editid.Value;
            }

            //inventory ,account and ledger entry 
            //stock entry

            if (IsPost)
            {

                if (txtProductName.Text != "")
                {

                    string strSerial = "0";


                    string docNo = objDocNo.GetDocumentNo(true, Session["compId"].ToString(), true, "11", "131", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());


                    string sql = "select count(*)+1 from Inv_AdjustHeader where CompanyId=" + Session["CompId"].ToString() + " and BrandId=" + Session["BrandId"].ToString() + " and FromLocationID=" + Session["LocId"].ToString() + "";
                    docNo = docNo + objda.return_DataTable(sql).Rows[0][0].ToString();
                    if (txtRate.Text == "")
                        txtRate.Text = "0";
                    b = objAdjustHeader.InsertAdjustHeader(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["LocId"].ToString(), docNo, objSys.getDateForInput(txtTransdate.Text).ToString(), "Vehicle Expenses Stock Out Entry For vehicle = " + Get_String_Before_Char(txtvehiclename.Text.Trim(), '/'), txtAmount.Text, true.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    objAdjustDetail.InsertAdjustDetail(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), b.ToString(), docNo, strSerial, hdnProductId.Value, ddlUnit.SelectedValue, txtQty.Text, txtRate.Text, strRefId, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    ObjProductledger.InsertProductLedger(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", b.ToString(), Session["LocId"].ToString(), hdnProductId.Value, ddlUnit.SelectedValue, "O", "0", "0", "0", txtQty.Text, "1/1/1800", txtRate.Text, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), objSys.getDateForInput(txtTransdate.Text).ToString(), Session["UserId"].ToString().ToString(), objSys.getDateForInput(txtTransdate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);

                }

                //if (chkAccountEntry.Checked && PaidTotaamount != 0)
                //{
                //    //vehicle ledger entry
                //    if (strOtherAccountNo.Trim() != "0")
                //    {
                //        ObjVehicleLedger.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), objSys.getDateForInput(txtTransdate.Text).ToString(), strOtherAccountNo, txtvehiclename.Text.Split('/')[1].ToString(), Narration, PaidTotaamount.ToString(), "0", PaidTotaamount.ToString(), Session["LocCurrencyId"].ToString(), "1", PaidTotaamount.ToString(), "0", "tp_Vehicle_Expenses", strRefId, MaxId.ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                //    }
                //}
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            if (editid.Value == "")
            {
                if (((Button)sender).ID.Trim() == "btn_Post")
                {
                    DisplayMessage("Record posted successfully");
                }
                else
                {

                    DisplayMessage("Record Saved Successfully","green");
                }

            }
            else
            {
                if (((Button)sender).ID.Trim() == "btn_Post")
                {
                    DisplayMessage("Record posted successfully");
                }
                else
                {

                    DisplayMessage("Record Updated Successfully", "green");
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

                btnList_Click(null, null);
            }
            Reset();
            FillGrid();
            btnSave.Enabled = true;
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
            btnSave.Enabled = true;
            return;

        }
    }

    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id=" + strAccountNo + " and IsActive='True'";

            DataTable dtAccName = objda.return_DataTable(sql);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString() + "/" + strAccountNo;
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }

    #region otheraccountNo
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = ObjContactMaster.GetContactTrueAllData();
        try
        {
            dtCustomer = new DataView(dtCustomer, "Field6='True'  and (Status='Company' or Is_Reseller='True')", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
            dtCustomer = ObjContactMaster.GetContactTrueAllData();
        }
        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Filtertext like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {

        string strContactId = "0";

        if (txtOtherAccountNo.Text != "")
        {
            string[] CustomerName = txtOtherAccountNo.Text.Split('/');

            DataTable DtCustomer = objContact.GetContactByContactName(CustomerName[0].ToString().Trim());

            if (DtCustomer.Rows.Count > 0)
            {

                strContactId = CustomerName[1].ToString().Trim();
            }
            else
            {

                DisplayMessage("Enter Customer Name in suggestion Only");
                txtOtherAccountNo.Text = "";
                txtOtherAccountNo.Focus();

            }
        }

    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        if (txtSupplierName.Text != "")
        {
            try
            {
                Get_String_After_Char(txtSupplierName.Text.Trim(), '/');
            }
            catch
            {
                DisplayMessage("Enter Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
                return;
            }

            DataTable dt = objContact.GetContactByContactName(Get_String_Before_Char(txtSupplierName.Text.Trim(), '/'));
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
            }
            else
            {
                string strSupplierId = Get_String_After_Char(txtSupplierName.Text.Trim(), '/');
                if (strSupplierId != "0" && strSupplierId != "")
                {
                    DataTable dtSup = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strSupplierId);
                    if (dtSup.Rows.Count > 0)
                    {
                        Session["SupplierAccountId"] = dtSup.Rows[0]["Account_No"].ToString();
                        //txtDebitAmount.Focus();
                    }
                    else
                    {
                        DisplayMessage("First Set Supplier Details in Supplier Setup");
                        txtSupplierName.Text = "";
                        txtSupplierName.Focus();
                        return;
                    }

                    if (Session["SupplierAccountId"].ToString() == "0" && Session["SupplierAccountId"].ToString() == "")
                    {
                        DisplayMessage("First Set Supplier Account in Supplier Setup");
                        txtSupplierName.Text = "";
                        txtSupplierName.Focus();
                        return;
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Select Supplier Name");
            txtSupplierName.Focus();
        }
    }
    #endregion


    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        if (((Label)gvrow.FindControl("lblIsPost")).Text == "True" && ((ImageButton)sender).ID == "btnEdit")
        {
            DisplayMessage("Record Posted , you can not edit");
            return;
        }

        editid.Value = e.CommandArgument.ToString();

        DataTable dt = objVehicleExpenses.GetRecord_By_TransId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {
            IsEditInitialize = true;
            Prj_VehicleMaster vehicle = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtvehicle = vehicle.GetRecord_By_VehicleId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), dt.Rows[0]["Vehicle_Id"].ToString());
            txtvehiclename.Text = dtvehicle.Rows[0]["Name"].ToString() + "/" + dtvehicle.Rows[0]["Vehicle_Id"].ToString();
            txtvehiclename_TextChanged(null, null);
            txtQty.Text = Common.GetAmountDecimal(dt.Rows[0]["qty"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtRate.Text = Common.GetAmountDecimal(dt.Rows[0]["Rate"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtAmount.Text = Common.GetAmountDecimal(dt.Rows[0]["Amount"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtPreReading.Text = dt.Rows[0]["Pre_Reading"].ToString();
            txtCurrentReading.Text = dt.Rows[0]["Current_Reading"].ToString();
            txtTransdate.Text = GetDate(dt.Rows[0]["Trans_date"].ToString());
            txtPumpStartReading.Text = dt.Rows[0]["Pump_Start_Reading"].ToString();
            txtPumpEndReading.Text = dt.Rows[0]["Pump_End_Reading"].ToString();
            ddlTankFull.SelectedValue = dt.Rows[0]["Is_Tank_Full"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remark"].ToString();
            ddlCategory.SelectedValue = dt.Rows[0]["Field1"].ToString();
            chkAccountEntry.Checked = Convert.ToBoolean(dt.Rows[0]["Field2"].ToString());

            if (chkAccountEntry.Checked)
            {
                txtpaymentCreditaccount.Text = GetAccountInformation(dt.Rows[0]["Cr_Account_No"].ToString());
                txtpaymentdebitaccount.Text = GetAccountInformation(dt.Rows[0]["Dr_Account_No"].ToString());
            }


            if (dt.Rows[0]["Product_Id"].ToString() != "0")
            {
                txtProductName.Text = dt.Rows[0]["ProductCode"].ToString();
                txtProductName_OnTextChanged(null, null);
                ddlUnit.SelectedValue = dt.Rows[0]["Field3"].ToString();
                hdnProductId.Value = dt.Rows[0]["Product_Id"].ToString();
            }
            else
            {
                hdnProductId.Value = "0";
            }

            //if (dt.Rows[0]["Driver_Id"].ToString() != "0")
            //{
            txtEmpName.Text = dt.Rows[0]["Driver_Name"].ToString() + "/" + dt.Rows[0]["Driver_Id"].ToString();
            //}
            //else
            //{
            //    txtEmpName.Text = dt.Rows[0]["Driver_Name"].ToString();
            //}



            btnNew_Click(null, null);

            if (((LinkButton)sender).ID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
            }
            else
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
            }

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            txtvehiclename.Focus();
            CategorySelection();
            IsEditInitialize = false;
        }
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "1/1/1900 12:00:00 AM")
        {
            strNewDate = Convert.ToDateTime(strDate).ToString(objSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        if (((Label)gvrow.FindControl("lblIsPost")).Text == "True")
        {
            DisplayMessage("Record Posted , you can not delete");
            return;
        }

        int b = 0;
        b = objVehicleExpenses.RestoreRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        try
        {
            gvExpensesMaster.Rows[gvrow.RowIndex].Cells[1].Focus();
        }
        catch
        {
        }
    }

    protected void gvExpensesMaster_OnPreRender(object sender, EventArgs e)
    {
        // You only need the following 2 lines of code if you are not 
        // using an ObjectDataSource of SqlDataSource
        //int currentpagesize = int.Parse(ddlPageSize.SelectedValue.ToString());
        //DataTable dt = objVehicleExpenses.GetAllTrueRecordByIndex(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["IndexNo"].ToString(), currentpagesize.ToString());
        //gvExpensesMaster.DataSource = dt;
        //gvExpensesMaster.DataBind();
        if (gvExpensesMaster.Rows.Count > 0)
        {
            //This replaces <td> with <th> and adds the scope attribute
            gvExpensesMaster.UseAccessibleHeader = true;

            //This will add the <thead> and <tbody> elements
            gvExpensesMaster.HeaderRow.TableSection = TableRowSection.TableHeader;

        }
    }
    protected void gvExpensesMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvExpensesMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Vehicle_Exp"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvExpensesMaster, dt, "", "");
        //AllPageCode();
        gvExpensesMaster.HeaderRow.Focus();

    }
    protected void gvExpensesMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Vehicle_Exp"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Vehicle_Exp"] = dt;
        gvExpensesMaster.DataSource = dt;
        gvExpensesMaster.DataBind();
        //AllPageCode();
        gvExpensesMaster.HeaderRow.Focus();
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
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
                ArebicMessage = EnglishMessage;
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
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
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    public void FillGrid()
    {

        string strpostStatus = string.Empty;

        int currentpagesize = int.Parse(ddlPageSize.SelectedItem.Text.ToString());



        DataTable dt = objVehicleExpenses.GetAllTrueRecordByIndex(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["IndexNo"].ToString(), currentpagesize.ToString(), ddlPosted.SelectedValue);
        //Common Function add By Lokesh on 11-05-2015

        objPageCmn.FillData((object)gvExpensesMaster, dt, "", "");

        //AllPageCode();
        Session["dtFilter_Vehicle_Exp"] = dt;
        Session["Country"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objVehicleExpenses.GetAllFalseRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvExpensesMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinCountry"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            // imgBtnRestore.Visible = false;
            // ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvExpensesMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvExpensesMasterBin.Rows)
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

                if (!userdetails.Contains(dr["Trans_Id"]))
                {
                    userdetails.Add(dr["Trans_Id"]);
                }
            }

            foreach (GridViewRow GR in gvExpensesMasterBin.Rows)
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
            DataTable dtUnit1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)gvExpensesMasterBin, dtUnit1, "", "");

            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvExpensesMasterBin.Rows.Count > 0)
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
                            b = objVehicleExpenses.RestoreRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
                    foreach (GridViewRow Gvr in gvExpensesMasterBin.Rows)
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
                gvExpensesMasterBin.Focus();
                return;
            }
        }
    }
    public void Reset()
    {
        Session["CHECKED_ITEMS"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtvehiclename.Text = "";
        txtQty.Text = "";
        txtAmount.Text = "";
        txtPreReading.Text = "";
        txtProductName.Text = "";
        txtRate.Text = "";
        txtPumpStartReading.Text = "";
        txtPumpEndReading.Text = "";
        txtTransdate.Text = "";
        txtEmpName.Text = "";
        txtvehiclename.Focus();
        Calender.Format = objSys.SetDateFormat();
        txtRemarks.Text = string.Empty;
        txtCurrentReading.Text = string.Empty;
        ddlUnit.Items.Clear();
        chkAccountEntry.Checked = false;
        txtpaymentCreditaccount.Text = "";
        txtpaymentdebitaccount.Text = "";
        txtOtherAccountNo.Text = "";
        // txtownername.Text = string.Empty;
        // ddlVehicleType.SelectedValue = "Own";
        txtTransdate.Text = DateTime.Now.ToString(objSys.SetDateFormat());
        ddlCategory.SelectedValue = "Diesel";
        trSupplier.Visible = false;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {

        EmployeeMaster objemployeemaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = objemployeemaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        try
        {
            dt = new DataView(dt, "Emp_Name like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString();
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVehicleName(string prefixText, int count, string contextKey)
    {
        Prj_VehicleMaster objVehicleMaster = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = (dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Vehicle_Id"].ToString());
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(),HttpContext.Current.Session["LocId"].ToString());
        //if (dt.Rows.Count == 0)
        //{
        //    dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());

        //}

        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }


        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetSuppliersList(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = ObjSupplier.GetAutoCompleteSupplierAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);

        string[] filterlist = new string[dtSupplier.Rows.Count];
        if (dtSupplier.Rows.Count > 0)
        {
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                filterlist[i] = dtSupplier.Rows[i]["Name"].ToString() + "/" + dtSupplier.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }

    public void FillUnit(string ProductId)
    {
        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());


        if (ddlCategory.SelectedValue == "Diesel")
        {
            //DataTable Dt_Diesel = objda.return_DataTable("select top 1 Pump_End_Reading from tp_Vehicle_Expenses where IsActive='true' and Trans_date <= '" + Convert.ToDateTime(txtTransdate.Text) + "' And  Field1='Diesel' and Vehicle_Id='" + Get_String_After_Char(txtvehiclename.Text.Trim(), '/') + "' order by Trans_date,trans_id desc");
            DataTable Dt_Diesel = objda.return_DataTable("Select top 1 Product_Id,Pump_Start_Reading,Pump_End_Reading,qty,Case when (Select top 1 Quantity from Inv_StockDetail where ProductId='" + ProductId + "' and IsActive='True' and Inv_StockDetail.ModifiedDate>tp_Vehicle_Expenses.ModifiedDate order by ModifiedDate desc) IS NULL then '0' else (Select top 1 Quantity from Inv_StockDetail where ProductId='" + ProductId + "' and IsActive='True' and Inv_StockDetail.ModifiedDate>tp_Vehicle_Expenses.ModifiedDate order by ModifiedDate desc) end as Stock_Quantity from tp_Vehicle_Expenses where Product_Id='" + ProductId + "' and IsActive='True' order by trans_id desc");
            if (Dt_Diesel.Rows.Count > 0)
            {
                txtPumpStartReading.Text = (Convert.ToDouble(Dt_Diesel.Rows[0]["Pump_End_Reading"].ToString()) + Convert.ToDouble(Dt_Diesel.Rows[0]["Stock_Quantity"].ToString())).ToString();
            }
            else
            {
                txtPumpStartReading.Text = "0";
            }
            if (txtQty.Text != "0" && txtQty.Text != "")
            {
                double qty = 0;
                double pumpStartReading = 0;
                double.TryParse(txtQty.Text, out qty);
                double.TryParse(txtPumpStartReading.Text, out pumpStartReading);
                txtPumpEndReading.Text = (qty + pumpStartReading).ToString("0.00");
            }
            Dt_Diesel.Dispose();
        }
        else if (ddlCategory.SelectedValue == "Petrol")
        {
            //DataTable Dt_Diesel = objda.return_DataTable("select top 1 Pump_End_Reading from tp_Vehicle_Expenses where IsActive='true' and Trans_date <= '" + Convert.ToDateTime(txtTransdate.Text) + "' And  Field1='Diesel' and Vehicle_Id='" + Get_String_After_Char(txtvehiclename.Text.Trim(), '/') + "' order by Trans_date,trans_id desc");
            DataTable Dt_Diesel = objda.return_DataTable("Select top 1 Product_Id,Pump_Start_Reading,Pump_End_Reading,qty,Case when (Select top 1 Quantity from Inv_StockDetail where ProductId='" + ProductId + "' and IsActive='True' and Inv_StockDetail.ModifiedDate>tp_Vehicle_Expenses.ModifiedDate order by ModifiedDate desc) IS NULL then '0' else (Select top 1 Quantity from Inv_StockDetail where ProductId='" + ProductId + "' and IsActive='True' and Inv_StockDetail.ModifiedDate>tp_Vehicle_Expenses.ModifiedDate order by ModifiedDate desc) end as Stock_Quantity from tp_Vehicle_Expenses where Product_Id='" + ProductId + "' and IsActive='True' order by trans_id desc");
            if (Dt_Diesel.Rows.Count > 0)
            {
                txtPumpStartReading.Text = (Convert.ToDouble(Dt_Diesel.Rows[0]["Pump_End_Reading"].ToString()) + Convert.ToDouble(Dt_Diesel.Rows[0]["Stock_Quantity"].ToString())).ToString();
            }
            else
            {
                txtPumpStartReading.Text = "0";
            }
            if (txtQty.Text != "0" && txtQty.Text != "")
            {
                double qty = 0;
                double pumpStartReading = 0;
                double.TryParse(txtQty.Text, out qty);
                double.TryParse(txtPumpStartReading.Text, out pumpStartReading);
                txtPumpEndReading.Text = (qty + pumpStartReading).ToString("0.00");
            }
            Dt_Diesel.Dispose();
        }
    }

    protected void txtProductName_OnTextChanged(object sender, EventArgs e)
    {
        ddlUnit.Items.Clear();
        DataTable dt = new DataTable();

        if (txtProductName.Text != "")
        {



            dt = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductName.Text);

            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Select Product in suggestion only");
                    txtProductName.Focus();
                    txtProductName.Text = "";
                    return;
                }
                else
                {
                    hdnProductId.Value = dt.Rows[0]["ProductId"].ToString();
                    FillUnit(dt.Rows[0]["ProductId"].ToString());
                }
            }
            else
            {
                DisplayMessage("Select Product in suggestion only");
                txtProductName.Focus();
                txtProductName.Text = "";
                return;

            }
        }
        dt.Dispose();
    }


    #region Account
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        string filtertext = "AccountName like '%" + prefixText + "%'";
        dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }

        return txt;

        //Commented by Ghanshyam suthar on 13-Dec-2017

        //string StrSupplierAccountNo = Ac_ParameterMaster.GetSupplierAccountNo(HttpContext.Current.Session["CompId"].ToString());

        //string sql = "select Ac_ChartOfAccount.AccountName,Ac_ChartOfAccount.Trans_Id from Ac_ChartOfAccount left join Ac_Groups ON Ac_Groups.Ac_Group_Id = Ac_ChartOfAccount.Acc_Group_Id where Ac_ChartOfAccount.Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Ac_ChartOfAccount.IsActive='True'  and (Ac_ChartOfAccount.Trans_id = " + StrSupplierAccountNo + " or Ac_Groups.Ac_GroupName = 'Cash In Hand')";
        //DataAccessClass daclass = new DataAccessClass();
        //DataTable dtCOA = daclass.return_DataTable(sql);

        //string filtertext = "AccountName like '%" + prefixText + "%'";
        //dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        //string[] txt = new string[dtCOA.Rows.Count];

        //if (dtCOA.Rows.Count > 0)
        //{
        //    for (int i = 0; i < dtCOA.Rows.Count; i++)
        //    {
        //        txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
        //    }
        //}

        //return txt;
    }
    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True'";
                DataTable dtCOA = objda.return_DataTable(sql);

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose Account in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {
                DisplayMessage("Choose Account in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }

            //Commented by ghanshyam suthar on 13-12-2017

            //try
            //{
            //    Get_String_Before_Char(((TextBox)sender).Text.Trim(), '/');
            //    //((TextBox)sender).Text.Split('/')[0].ToString();

            //    string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + Get_String_Before_Char(((TextBox)sender).Text, '/') + "' and IsActive='True'";
            //    DataTable dtCOA = objda.return_DataTable(sql);

            //    if (dtCOA.Rows.Count == 0)
            //    {
            //        DisplayMessage("Choose Account in Suggestion Only");
            //        ((TextBox)sender).Text = "";
            //        ((TextBox)sender).Focus();
            //        return;
            //    }
            //}
            //catch
            //{
            //    DisplayMessage("Choose Account in Suggestion Only");
            //    ((TextBox)sender).Text = "";
            //    ((TextBox)sender).Focus();
            //    return;
            //}

            //for Customer & Supplier Account
            string strReceiveVoucherAcc = string.Empty;
            DataTable dtParam = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers");
            if (dtParam.Rows.Count > 0)
            {
                strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
            }
            else
            {
                strReceiveVoucherAcc = "0";
            }

            string strPaymentVoucherAcc = string.Empty;
            DataTable dtPaymentVoucher = objAcParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers");
            if (dtPaymentVoucher.Rows.Count > 0)
            {
                strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
            }
            else
            {
                strPaymentVoucherAcc = "0";
            }

            if (Get_String_After_Char(txtpaymentCreditaccount.Text.Trim(), '/') == strReceiveVoucherAcc)
            {
                trSupplier.Visible = false;
                txtSupplierName.Text = "";
            }
            else if (Get_String_After_Char(txtpaymentCreditaccount.Text.Trim(), '/') == strPaymentVoucherAcc)
            {
                trSupplier.Visible = true;
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
            }
            else
            {
                trSupplier.Visible = false;
                txtSupplierName.Text = "";
            }
        }
        else
        {
            trSupplier.Visible = false;
            txtSupplierName.Text = "";
            txtpaymentCreditaccount.Focus();
        }
    }
    public string GetAccountInformation(string strAccountId)
    {
        string strAcName = string.Empty;
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id='" + strAccountId + "'";
        DataTable dtCOA = objda.return_DataTable(sql);
        if (dtCOA.Rows.Count > 0)
        {
            strAcName = dtCOA.Rows[0]["AccountName"].ToString() + "/" + dtCOA.Rows[0]["Trans_Id"].ToString();
        }
        return strAcName;
    }
    #endregion

    protected void Btn_Mileage_Click(object sender, EventArgs e)
    {
        string MonthNumber = DateTime.Now.ToString("MM");
        ddlMonth.SelectedValue = MonthNumber;
        string Year = DateTime.Now.Year.ToString();
        txtYear.Text = Year;
        HttpContext.Current.Session["IndexNoMileage"] = "1";
        FillMileageGrid();
    }

    protected void btnBind_Mileage_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        string VehicleId = string.Empty;
        string PageSize = int.Parse(Session["GridSize"].ToString()).ToString();
        string Year = txtYear.Text;
        string Month = ddlMonth.SelectedValue;
        string IsLowMileage = string.Empty;
        if (ddlLowMileage.SelectedIndex == 1)
        {
            IsLowMileage = "true";
        }

        if (String.IsNullOrEmpty(txtVehicleSearch.Text))
            VehicleId = "0";
        else
            VehicleId = Get_String_After_Char(txtVehicleSearch.Text.Trim(), '/');

        if (String.IsNullOrEmpty(txtYear.Text))
            Year = DateTime.Now.Year.ToString();

        DataTable dtRecord = objVehicleExpenses.GetMileageRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), VehicleId, ddlMonth.SelectedValue, IsLowMileage, PageSize, "1", "true", Year);
        DataTable dt = objVehicleExpenses.GetMileageRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), VehicleId, ddlMonth.SelectedValue, IsLowMileage, PageSize, "1", "", Year);

        if (dt != null && dt.Rows.Count > 0)
        {
            gvMileage.DataSource = dt;
            gvMileage.DataBind();

            lblTotalRecordMileage.Text = Resources.Attendance.Total_Records + ": " + dtRecord.Rows[0][0].ToString() + "";

            ViewState["TotalRecordMileage"] = int.Parse(dtRecord.Rows[0][0].ToString());

            AddpagingButtonMileage();
        }
        else
        {
            gvMileage.DataSource = null;
            gvMileage.DataBind();

            lblTotalRecordMileage.Text = Resources.Attendance.Total_Records + ": " + "0" + "";

            PagingPanelMileage.Controls.Clear();
        }

    }

    protected void btnRefresh_Mileage_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        txtYear.Text = DateTime.Now.Year.ToString();
        string MonthNumber = DateTime.Now.ToString("MM");
        ddlMonth.SelectedValue = MonthNumber;
        FillMileageGrid();
        txtVehicleSearch.Text = string.Empty;
        ddlLowMileage.SelectedIndex = 0;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetVehicleList(string prefixText, int count, string contextKey)
    {
        Prj_VehicleMaster objVehicleMaster = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = (dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Vehicle_Id"].ToString());
        }
        return txt;
    }

    protected void gvMileage_PreRender(object sender, EventArgs e)
    {
        if (gvMileage.Rows.Count > 0)
        {
            //This replaces <td> with <th> and adds the scope attribute
            gvMileage.UseAccessibleHeader = true;

            //This will add the <thead> and <tbody> elements
            gvMileage.HeaderRow.TableSection = TableRowSection.TableHeader;

        }
    }

    private void FillMileageGrid()
    {
        string VehicleId = string.Empty;
        string PageSize = int.Parse(Session["GridSize"].ToString()).ToString();
        string BatchNo = HttpContext.Current.Session["IndexNoMileage"].ToString();
        string IsLowMileage = string.Empty;
        if (ddlLowMileage.SelectedIndex == 1)
        {
            IsLowMileage = "true";
        }
        VehicleId = "0";
        string Year = string.Empty;
        if (String.IsNullOrEmpty(txtYear.Text))
            Year = DateTime.Now.Year.ToString();
        else
            Year = txtYear.Text;

        DataTable dtRecord = objVehicleExpenses.GetMileageRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), VehicleId, ddlMonth.SelectedValue, IsLowMileage, PageSize, BatchNo, "true", Year);
        DataTable dt = objVehicleExpenses.GetMileageRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), VehicleId, ddlMonth.SelectedValue, IsLowMileage, PageSize, BatchNo, "", Year);

        gvMileage.DataSource = dt;
        gvMileage.DataBind();

        Session["dtMileageFilter"] = dt;

        lblTotalRecordMileage.Text = Resources.Attendance.Total_Records + ": " + dtRecord.Rows[0][0].ToString() + "";

        ViewState["TotalRecordMileage"] = int.Parse(dtRecord.Rows[0][0].ToString());

        AddpagingButtonMileage();
    }

    private void AddpagingButtonMileage()
    {
        // this method for generate custom button for Custom paging in Gridview
        int totalRecord = 0;
        int noofRecord = 0;
        totalRecord = ViewState["TotalRecordMileage"] != null ? (int)ViewState["TotalRecordMileage"] : 0;
        noofRecord = ViewState["NoOfRecord"] != null ? (int)ViewState["NoOfRecord"] : 0;
        int pages = 0;
        PagingPanelMileage.Controls.Clear();
        if (totalRecord > 0 && noofRecord > 0)
        {
            // Count no of pages 
            pages = (totalRecord / noofRecord) + ((totalRecord % noofRecord) > 0 ? 1 : 0);
            //if (totalRecord < noofRecord)
            //{
            //    pages = pages + 1;
            //}

            for (int i = 0; i < pages; i++)
            {
                Button b = new Button();
                b.CssClass = "pagingbtn";
                b.Text = (i + 1).ToString();
                b.CommandArgument = (i + 1).ToString();
                b.ID = "ButtonM_" + (i + 1).ToString();
                b.Click += new EventHandler(this.b_Mileage_click);
                // b.OnClientClick = "RemoveSelectedCss();";
                PagingPanelMileage.Controls.Add(b);
            }
        }

    }

    protected void b_Mileage_click(object sender, EventArgs e)
    {
        // this is for Get data from Database on button (paging button) click
        string pageNo = ((Button)sender).CommandArgument;
        HttpContext.Current.Session["IndexNoMileage"] = pageNo;
        FillGrid();
        PopulateData();
        Button btn = sender as Button;
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "myfunction", "SetSelectedCssMileage(" + btn.Text + ");", true);
    }

    protected void chkAccountEntry_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAccountEntry.Checked)
        {
            DivCreditAccount.Visible = true;
        }
        else
        {
            DivCreditAccount.Visible = false;
        }

    }

    protected void gvMileage_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtMileageFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtMileageFilter"] = dt;
        gvMileage.DataSource = dt;
        gvMileage.DataBind();
        //AllPageCode();
        gvMileage.HeaderRow.Focus();
    }
}