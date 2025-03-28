using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Production_ProductionProcess : System.Web.UI.Page
{
    #region defined Class Object
    Production_Process_Detail objProcessdetail = null;
    ProductOptionCategoryDetail objProOpCatedetail = null;
    Common cmn = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Production_Process objProductionProcess = null;
    Production_BOM objProductionBom = null;
    Production_Employee objProductionEmployee = null;
    Ems_GroupMaster objGroupMaster = null;
    Inv_ProductionRequestDetail ObjProductionRequestDetail = null;
    Inv_ProductionRequestHeader ObjProductionReqestHeader = null;
    CountryMaster ObjCountry = null;
    Inv_ProductMaster objProductM = null;
    Inv_UnitMaster UM = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster objContact = null;
    LocationMaster objLocation = null;
    Inv_StockDetail objStockDetail = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    EmployeeMaster objEmployee = null;
    Inv_ParameterMaster objInvParameter = null;
    Inv_StockDetail objStock = null;
    DataAccessClass objDa = null;
    BillOfMaterial ObjInvBOM = null;
    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objProcessdetail = new Production_Process_Detail(Session["DBConnection"].ToString());
        objProOpCatedetail = new ProductOptionCategoryDetail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        objProductionProcess = new Production_Process(Session["DBConnection"].ToString());
        objProductionBom = new Production_BOM(Session["DBConnection"].ToString());
        objProductionEmployee = new Production_Employee(Session["DBConnection"].ToString());
        objGroupMaster = new Ems_GroupMaster(Session["DBConnection"].ToString());
        ObjProductionRequestDetail = new Inv_ProductionRequestDetail(Session["DBConnection"].ToString());
        ObjProductionReqestHeader = new Inv_ProductionRequestHeader(Session["DBConnection"].ToString());
        ObjCountry = new CountryMaster(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objInvParameter = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objStock = new Inv_StockDetail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjInvBOM = new BillOfMaterial(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Production/ProductionProcess.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            Session["DtBOM"] = null;
            Session["dtVisitTaskList"] = null;
            LoadVisitTask();
            ddlOption.SelectedIndex = 2;
            FillGrid();
            Session["DtDetail"] = null;
            FillRequestGrid();

            txtPINo.Text = GetDocumentNum();
            ViewState["DocNo"] = txtPINo.Text;


            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueRequestDate.Format = Session["DateFormat"].ToString();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            CalendarExtender2.Format = Session["DateFormat"].ToString();

            BtnReset_Click(null, null);
            Session["DtSearchProduct"] = null;
            ddlFieldName.Focus();
            FillRequestLocation();
        }
        //AllPageCode();
    }
    public void FillRequestLocation()
    {

        DataTable dtLocation = objLocation.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());
        dtLocation = new DataView(dtLocation, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();

        objPageCmn.FillData((object)ddlLocation, dtLocation, "Location_Name", "Location_Id");
        ddlLocation.Items.RemoveAt(0);
        ddlLocation.SelectedValue = Session["LocId"].ToString();
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }

    #region System defined Function

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;


        if (objSenderID == "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
        }
        else
        {
            if (e.CommandName.ToString() == "True")
            {
                DisplayMessage("Record posted , you can not edit !");
                return;
            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;

        }

        Lbl_Tab_New.Enabled = true;
        if (Lbl_Tab_New.Text == "View")
        {
            btnPISave.Visible = false;
            BtnReset.Visible = false;
        }
        else
        {

            BtnReset.Visible = true;
        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

        editid.Value = e.CommandArgument.ToString();

        DataTable dtProcess = objProductionProcess.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        if (dtProcess.Rows.Count > 0)
        {
            txtPINo.Enabled = false;
            txtPINo.Text = dtProcess.Rows[0]["Job_No"].ToString();
            hdnrequestid.Value = dtProcess.Rows[0]["Ref_Production_Req_No"].ToString();
            txtCustomer.Text = dtProcess.Rows[0]["Customername"].ToString();
            txtRequestNo.Text = dtProcess.Rows[0]["Request_No"].ToString();
            txtRequestDate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Request_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtSONo.Text = dtProcess.Rows[0]["Order_No"].ToString();
            if (dtProcess.Rows[0]["Order_Date"].ToString() != "1/1/1900 12:00:00 AM")
            {
                txtSODate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Order_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }

            hdnOrderId.Value = dtProcess.Rows[0]["SalesOrderId"].ToString();

            txtjobcreationdate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Job_Creation_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtjobstartdate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Job_Start"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtexpjobenddate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Exp_Job_End"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtDescription.Text = dtProcess.Rows[0]["Remarks"].ToString();
            chkCancel.Checked = Convert.ToBoolean(dtProcess.Rows[0]["Is_Cancel"].ToString());
            if (dtProcess.Rows[0]["Job_End"].ToString() != "1/1/1900 12:00:00 AM")
            {
                txtjobenddate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Job_End"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            ddlLocation.SelectedValue = dtProcess.Rows[0]["Req_For_Material"].ToString();
            ChkisQualitycheck.Checked = Convert.ToBoolean(dtProcess.Rows[0]["Field6"].ToString());
            //get record from bom table
            DataTable dtBom = objProductionBom.GetRecord_By_RefJobNo(editid.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                dtBom = dtBom.DefaultView.ToTable(true, "Id", "Item_Id", "Unit_Id", "Sys_qty", "Unit_Price", "Req_Qty", "Line_Total", "ShortDescription");
            }
            catch
            {
            }
            objPageCmn.FillData((object)gvBom, dtBom, "", "");
            Session["DtBOM"] = dtBom;
            //get record from production employee

            DataTable dtEmployee = objProductionEmployee.GetRecord_By_RefJobNo(editid.Value);
            try
            {
                dtEmployee = dtEmployee.DefaultView.ToTable(true, "Id", "Emp_Name", "Job_Date", "Start_Time", "Stop_Time", "Duration", "Machine_Name");
            }
            catch
            {
            }
            if (dtEmployee.Rows.Count > 0)
            {
                Session["dtVisitTaskList"] = dtEmployee;
                objPageCmn.FillData((object)gvVisitTask, dtEmployee, "", "");
            }
            //get request detail
            DataTable dtrequestDetail = objProcessdetail.GetRecord_By_RefJobNo(editid.Value);

            objPageCmn.FillData((object)GvProduct, dtrequestDetail, "", "");

            TabContainer2.ActiveTabIndex = 0;

            //for machine ionformation

            string strMachineInformation = dtProcess.Rows[0]["Field1"].ToString();


            foreach (ListItem item in chkMachineInformation.Items)
            {
                if (strMachineInformation.Trim() != "")
                {
                    if (strMachineInformation.Contains(item.Value))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                else
                {
                    item.Selected = false;
                }
            }
        }

        //AllPageCode();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);

    }
    protected void GvProductionProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvProductionProcess.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtPInquiry"];

        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcess, dt, "", "");
        //AllPageCode();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlFieldName.SelectedItem.Value == "Job_Creation_Date" || ddlFieldName.SelectedItem.Value == "Request_Date")
        {
            if (txtValueDate.Text != "")
            {

                try
                {
                    ObjSysParam.getDateForInput(txtValueDate.Text);
                    txtValue.Text = Convert.ToDateTime(txtValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDate.Text = "";
                    txtValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDate.Focus();
                txtValue.Text = "";
                return;
            }
        }
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

            DataTable dtPurchaseInquiry = (DataTable)Session["dtPInquiry"];
            DataView view = new DataView(dtPurchaseInquiry, condition, "", DataViewRowState.CurrentRows);

            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductionProcess, view.ToTable(), "", "");

            Session["dtPInquiry"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

            //AllPageCode();
            btnRefresh.Focus();
        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvProductionProcess_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtPInquiry"];
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
        Session["dtPInquiry"] = dt;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcess, dt, "", "");
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() == "True")
        {
            DisplayMessage("Record posted , you can not delete !");
            return;
        }


        int b = 0;
        b = objProductionProcess.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), "false", Session["UserId"].ToString(), DateTime.Now.ToString());
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
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueDate.Text = "";
        txtValue.Focus();
    }
    protected void btnPICancel_Click(object sender, EventArgs e)
    {
        Reset();
        FillGrid();
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        editid.Value = "";
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);

    }

    protected void btnPost_Click(object sender, EventArgs e)
    {
        chkpost.Checked = true;
        btnPISave_Click(null, null);
    }
    protected void btnPISave_Click(object sender, EventArgs e)
    {


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());



        string strSql = string.Empty;

        if (txtRequestNo.Text == "")
        {
            DisplayMessage("Request no. not found");
            return;
        }
        if (txtPINo.Text == "")
        {
            DisplayMessage("Enter Voucher No.");
            txtPINo.Focus();
            return;
        }

        if (txtjobcreationdate.Text == "")
        {
            DisplayMessage("Enter Job creation date");
            txtjobcreationdate.Focus();
            return;
        }


        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtjobcreationdate.Text), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }



        if (txtexpjobenddate.Text == "")
        {
            DisplayMessage("Enter expected job end date");
            txtexpjobenddate.Focus();
            return;
        }



        if (GvProduct.Rows.Count == 0)
        {
            DisplayMessage("Product not found");
            return;
        }

        string strMachineInformation = string.Empty;

        foreach (ListItem item in chkMachineInformation.Items)
        {
            if (item.Selected)
            {

                strMachineInformation += item.Text + " , ";
            }
        }

        //if machine information is not equal to blank then

        if (strMachineInformation != "")
        {
            strMachineInformation = strMachineInformation.Substring(0, strMachineInformation.Length - 2);
        }

        string strsql = string.Empty;

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();


        try
        {


            if (editid.Value == "")
            {
                //here we check that request number is already found or not
                int b = 0;
                b = objProductionProcess.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnrequestid.Value, txtPINo.Text, ObjSysParam.getDateForInput(txtjobcreationdate.Text).ToString(), ObjSysParam.getDateForInput(txtjobstartdate.Text).ToString(), "01/01/1900", ObjSysParam.getDateForInput(txtexpjobenddate.Text).ToString(), txtDescription.Text, "", ddlLocation.SelectedValue, chkCancel.Checked.ToString(), chkpost.Checked.ToString(), strMachineInformation.Trim(), "False", "", "", "", ChkisQualitycheck.Checked.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                string strMaxId = string.Empty;

                strMaxId = b.ToString();
                if (txtPINo.Text == ViewState["DocNo"].ToString())
                {
                    DataTable dtCount = objProductionProcess.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                    if (dtCount.Rows.Count == 0)
                    {
                        objProductionProcess.Updatecode(b.ToString(), txtPINo.Text + "1", ref trns);
                        txtPINo.Text = txtPINo.Text + "1";
                    }
                    else
                    {
                        objProductionProcess.Updatecode(b.ToString(), txtPINo.Text + dtCount.Rows.Count, ref trns);
                        txtPINo.Text = txtPINo.Text + dtCount.Rows.Count;
                    }
                }


                if (hdnOrderId.Value == "")
                {
                    hdnOrderId.Value = "0";
                }

                //update expected delivery date in sales order page from production 
                strsql = "update Inv_SalesOrderHeader set EstimateDeliveryDate ='" + ObjSysParam.getDateForInput(txtexpjobenddate.Text).ToString() + "' where Trans_Id=" + hdnOrderId.Value + "";
                objDa.execute_Command(strsql, ref trns);


                int i = 0;
                foreach (GridViewRow gvRow in gvBom.Rows)
                {
                    Label lblTransId = (Label)gvRow.FindControl("lblTransId");
                    HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
                    HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdngvUnitId");
                    Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
                    TextBox txtgvUnitPrice = (TextBox)gvRow.FindControl("txtgvUnitPrice");
                    TextBox txtRequestquantity = (TextBox)gvRow.FindControl("txtRequestquantity");
                    Label lblgvtotal = (Label)gvRow.FindControl("lblgvtotal");
                    Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
                    i++;

                    objProductionBom.InsertRecord(strMaxId, i.ToString(), hdngvProductId.Value, hdngvUnitId.Value, txtgvUnitPrice.Text, txtRequestquantity.Text, "0", "0", "0", "0", lblgvtotal.Text, lblgvProductName.Text, ref trns);

                }

                foreach (GridViewRow gvRow in GvProduct.Rows)
                {
                    if (chkpost.Checked)
                    {
                        objDa.execute_Command("update Inv_ProductionRequestDetail set Field2= CAST(Field2 as numeric(18,6))+" + ((TextBox)gvRow.FindControl("txtReqQty")).Text.Trim() + " where Request_No=" + hdnrequestid.Value + " and ProductId = " + ((Label)gvRow.FindControl("lblPID")).Text + "", ref trns);
                    }

                    objProcessdetail.InsertRecord(strMaxId, ((Label)gvRow.FindControl("lblPID")).Text, ((Label)gvRow.FindControl("lblUID")).Text, ((Label)gvRow.FindControl("lblunitPrice")).Text, ((Label)gvRow.FindControl("lblReqQty")).Text, ((Label)gvRow.FindControl("lblRemainQty")).Text, ((TextBox)gvRow.FindControl("txtReqQty")).Text, ((Label)gvRow.FindControl("lblLinetotal")).Text, "0", ref trns);
                }

                if (Session["dtVisitTaskList"] != null)
                {

                    foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                    {
                        objProductionEmployee.InsertRecord(strMaxId, dr["Emp_Name"].ToString(), dr["Job_Date"].ToString(), dr["Start_Time"].ToString(), dr["Stop_Time"].ToString(), dr["Duration"].ToString(), dr["Machine_Name"].ToString(), ref trns);
                    }
                }

                strSql = "update dbo.Inv_ProductionRequestHeader set Is_Production_Process='True',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id=" + hdnrequestid.Value + "";
                objDa.execute_Command(strSql, ref trns);
                //for update is production process flag when we post process
                //code start
                if (chkpost.Checked)
                {
                    DisplayMessage("Record has been posted");
                }
                else
                {
                    DisplayMessage("Record Saved", "green");
                }

                //code end
                //FillGrid();


            }
            else
            {
                objProductionProcess.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, hdnrequestid.Value, txtPINo.Text, ObjSysParam.getDateForInput(txtjobcreationdate.Text).ToString(), ObjSysParam.getDateForInput(txtexpjobenddate.Text).ToString(), txtDescription.Text, "", ddlLocation.SelectedValue, chkCancel.Checked.ToString(), chkpost.Checked.ToString(), strMachineInformation.Trim(), "False", "", "", "", ChkisQualitycheck.Checked.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                //update expected delivery date in sales order page from production 
                strsql = "update Inv_SalesOrderHeader set EstimateDeliveryDate ='" + ObjSysParam.getDateForInput(txtexpjobenddate.Text).ToString() + "' where Trans_Id=" + hdnOrderId.Value + "";
                objDa.execute_Command(strsql, ref trns);



                //delete from bom table and reinsert 
                objProductionBom.DeleteRecord_By_RefJobNo(editid.Value, ref trns);

                int i = 0;
                foreach (GridViewRow gvRow in gvBom.Rows)
                {
                    Label lblTransId = (Label)gvRow.FindControl("lblTransId");
                    HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
                    HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdngvUnitId");
                    Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
                    TextBox txtgvUnitPrice = (TextBox)gvRow.FindControl("txtgvUnitPrice");
                    TextBox txtRequestquantity = (TextBox)gvRow.FindControl("txtRequestquantity");
                    Label lblgvtotal = (Label)gvRow.FindControl("lblgvtotal");

                    Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
                    i++;
                    objProductionBom.InsertRecord(editid.Value, i.ToString(), hdngvProductId.Value, hdngvUnitId.Value, txtgvUnitPrice.Text, txtRequestquantity.Text, "0", "0", "0", "0", lblgvtotal.Text, lblgvProductName.Text, ref trns);

                }
                objProcessdetail.DeleteRecord_By_RefJobNo(editid.Value, ref trns);

                foreach (GridViewRow gvRow in GvProduct.Rows)
                {
                    if (chkpost.Checked)
                    {
                        objDa.execute_Command("update Inv_ProductionRequestDetail set Field2= CAST(Field2 as numeric(18,6))+" + ((TextBox)gvRow.FindControl("txtReqQty")).Text.Trim() + " where Request_No=" + hdnrequestid.Value + " and ProductId = " + ((Label)gvRow.FindControl("lblPID")).Text + "", ref trns);
                    }
                    objProcessdetail.InsertRecord(editid.Value, ((Label)gvRow.FindControl("lblPID")).Text, ((Label)gvRow.FindControl("lblUID")).Text, ((Label)gvRow.FindControl("lblunitPrice")).Text, ((Label)gvRow.FindControl("lblReqQty")).Text, ((Label)gvRow.FindControl("lblRemainQty")).Text, ((TextBox)gvRow.FindControl("txtReqQty")).Text, ((Label)gvRow.FindControl("lblLinetotal")).Text, "0", ref trns);
                }


                //delete from job detail table and reinsert
                objProductionEmployee.DeleteRecord_By_RefJobNo(editid.Value, ref trns);
                if (Session["dtVisitTaskList"] != null)
                {

                    foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                    {
                        objProductionEmployee.InsertRecord(editid.Value, dr["Emp_Name"].ToString(), dr["Job_Date"].ToString(), dr["Start_Time"].ToString(), dr["Stop_Time"].ToString(), dr["Duration"].ToString(), dr["Machine_Name"].ToString(), ref trns);

                    }
                }

                //for update is production process flag when we post process
                //code start
                if (chkpost.Checked)
                {
                    DisplayMessage("Record has been posted");
                }
                else
                {
                    DisplayMessage("Record Updated", "green");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                //code end

            }


            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            FillGrid();
            BtnReset_Click(null, null);

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
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {

        int b = 0;
        b = objProductionProcess.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
        FillGrid();
        FillGridBin();
        Reset();
    }

    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtValueBin.Text = "";
        txtValueBin.Focus();
    }
    protected void btnPRequest_Click(object sender, EventArgs e)
    {
        FillRequestGrid();
    }
    protected void GvProductionProcessBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvProductionProcessBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtPInquiryBin"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcessBin, dt, "", "");


        string temp = string.Empty;

        for (int i = 0; i < GvProductionProcessBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvProductionProcessBin.Rows[i].FindControl("hdnTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvProductionProcessBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
    }
    protected void GvProductionProcessBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objProductionProcess.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtPInquiryBin"] = dt;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcessBin, dt, "", "");

        lblSelectedRecord.Text = "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objProductionProcess.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcessBin, dt, "", "");
        Session["dtPInquiryBin"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            //ImgbtnSelectAll.Visible = false;
            //imgBtnRestore.Visible = false;
        }
        else
        {
            //ImgbtnSelectAll.Visible = false;
            //imgBtnRestore.Visible = true;
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "Job_Creation_Date" || ddlFieldNameBin.SelectedItem.Value == "Request_Date")
        {
            if (txtValueBinDate.Text != "")
            {

                try
                {
                    ObjSysParam.getDateForInput(txtValueBinDate.Text);
                    txtValueBin.Text = Convert.ToDateTime(txtValueBinDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueBinDate.Text = "";
                    txtValueBin.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBinDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueBinDate.Focus();
                txtValueBin.Text = "";
                return;
            }
        }
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


            DataTable dtCust = (DataTable)Session["dtPInquiryBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtPInquiryBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductionProcessBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }
            //AllPageCode();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
            //btnRefreshBin.Focus();

        }
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvProductionProcessBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvProductionProcessBin.Rows.Count; i++)
        {
            ((CheckBox)GvProductionProcessBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvProductionProcessBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvProductionProcessBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvProductionProcessBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        HiddenField lb = (HiddenField)GvProductionProcessBin.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)GvProductionProcessBin.Rows[index].FindControl("chkSelect")).Checked)
        {
            empidlist += lb.Value.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Value.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectedRecord.Text = temp;
        }
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;

        txtValueBin.Visible = true;
        txtValueBinDate.Visible = false;
        txtValueBinDate.Text = "";


        lblSelectedRecord.Text = "";
        //AllPageCode();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtPInquiryBin"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Id"]))
                {
                    lblSelectedRecord.Text += dr["Id"] + ",";
                }
            }
            for (int i = 0; i < GvProductionProcessBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvProductionProcessBin.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvProductionProcessBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtPInquiryBin"];
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductionProcessBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        bool Result = true;

        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(objProductionProcess.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].ToString()).Rows[0]["Job_Creation_Date"].ToString()), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                    {
                        Result = false;
                        break;
                    }
                }
            }
        }

        if (!Result)
        {
            DisplayMessage("You can not restore closed financial year record");
            return;
        }


        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objProductionProcess.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvProductionProcessBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
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
    #endregion

    #endregion

    #region User defined Function

    private void FillGrid()
    {
        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            PostStatus = " Is_Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            PostStatus = " Is_Post='False'";
        }
        DataTable dtPInquiry = new DataView(objProductionProcess.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()), PostStatus, "Id Desc", DataViewRowState.CurrentRows).ToTable();

        //DataTable dtPInquiry = objProductionProcess.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtPInquiry.Rows.Count + "";

        Session["dtPInquiry"] = dtPInquiry;
        if (dtPInquiry.Rows.Count > 0)
        {
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductionProcess, dtPInquiry, "", "");

        }
        else
        {
            GvProductionProcess.DataSource = null;
            GvProductionProcess.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtPInquiry.Rows.Count.ToString() + "";
        //AllPageCode();
    }

    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "1/1/1900 12:00:00 AM")
        {
            strNewDate = Convert.ToDateTime(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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

        Session["DtBOM"] = null;
        objPageCmn.FillData((object)gvBom, null, "", "");
        ddlProductSerach.SelectedIndex = 1;
        ddlProductSerach_SelectedIndexChanged(null, null);
        btnPICancel.Visible = true;
        txtPINo.ReadOnly = false;
        txtDescription.Text = "";
        GvProduct.DataSource = null;
        GvProduct.DataBind();
        Session["DtDetail"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        txtValueDate.Text = "";
        txtValueBinDate.Text = "";
        txtValueRequest.Text = "";
        txtValueRequestDate.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueBin.Visible = true;
        txtValueBinDate.Visible = false;
        txtValueRequest.Visible = true;
        txtValueRequestDate.Visible = false;
        lblSelectedRecord.Text = "";
        //txtPINo.Text = GetDocumentNumber();
        txtPINo.Text = GetDocumentNum();
        ViewState["DocNo"] = txtPINo.Text;
        Session["DtSearchProduct"] = null;
        ViewState["Dtproduct"] = null;
        Session["dtVisitTaskList"] = null;
        LoadVisitTask();
        hdnrequestid.Value = "";
        txtRequestNo.Text = "";
        txtRequestDate.Text = "";
        txtCustomer.Text = "";
        txtSONo.Text = "";
        txtSODate.Text = "";
        txtjobcreationdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtexpjobenddate.Text = "";
        txtjobstartdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtDescription.Text = "";
        chkpost.Checked = false;
        chkCancel.Checked = false;
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        txtPINo.Enabled = true;
        ChkisQualitycheck.Checked = false;
        foreach (ListItem item in chkMachineInformation.Items)
        {

            item.Selected = false;

        }
    }
    protected string GetDocumentNum()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "167", "321", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    #endregion


    #region Add Request Section
    protected void GvPurchaseRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseRequest.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtPRequest"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseRequest, dt, "", "");

        //AllPageCode();
    }
    protected void GvPurchaseRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtPRequest"];
        string sortdir = "DESC";
        if (ViewState["PRSortDir"] != null)
        {
            sortdir = ViewState["PRSortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["PRSortDir"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["PRSortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["PRSortDir"] = "DESC";
        }

        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["PRSortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtPRequest"] = dt;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseRequest, dt, "", "");
        //AllPageCode();
    }
    private void FillRequestGrid()
    {
        DataTable dtPRequest = null;

        dtPRequest = ObjProductionReqestHeader.GetRecord_For_ProductionProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        Session["dtPRequest"] = dtPRequest;
        if (dtPRequest != null && dtPRequest.Rows.Count > 0)
        {
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPurchaseRequest, dtPRequest, "", "");

        }
        else
        {
            GvPurchaseRequest.DataSource = null;
            GvPurchaseRequest.DataBind();
        }

        lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + ": " + dtPRequest.Rows.Count.ToString() + "";
        //AllPageCode();

    }
    protected void btnPREdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = "";

        DataTable dt = ObjProductionReqestHeader.GetRecord_By_TransId(Session["Compid"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            txtRequestDate.Text = Convert.ToDateTime(dt.Rows[0]["Request_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtRequestNo.Text = dt.Rows[0]["Request_No"].ToString();
            txtCustomer.Text = dt.Rows[0]["Customername"].ToString() + "/" + dt.Rows[0]["Customer_Id"].ToString();
            txtSONo.Text = dt.Rows[0]["Order_No"].ToString();
            hdnOrderId.Value = dt.Rows[0]["Field1"].ToString();
            if (dt.Rows[0]["Order_Date"].ToString() != "1/1/1900 12:00:00 AM")
            {
                txtSODate.Text = Convert.ToDateTime(dt.Rows[0]["Order_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            hdnrequestid.Value = dt.Rows[0]["Trans_Id"].ToString();
            txtjobstartdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtjobcreationdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtexpjobenddate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

            txtDescription.Text = dt.Rows[0]["Remarks"].ToString();
            ddlLocation.SelectedValue = e.CommandName.ToString();
            DataTable dtDetail = ObjProductionRequestDetail.GetRecord_By_RequestNo(Session["Compid"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
            dtDetail = new DataView(dtDetail, "Remain_Qty>0", "", DataViewRowState.CurrentRows).ToTable();
            objPageCmn.FillData((object)GvProduct, dtDetail, "", "");
            getBom();
        }
        //AllPageCode();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Request_Active()", true);
    }

    #endregion
    #region ViewOption
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    #endregion

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnPISave.Visible = clsPagePermission.bAdd;
        btnPost.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    #endregion
    protected void GvPurchaseInquiry_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public string ProductName(string ProductId)
    {
        string ProductName = string.Empty;


        DataTable dt = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["EProductName"].ToString();
        }
        else
        {
            ProductName = "0";

        }


        return ProductName;

    }

    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;


        DataTable dt = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["ProductCode"].ToString();
        }
        else
        {
            ProductName = "";


        }

        return ProductName;

    }

    public string ProductDescription(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["Description"].ToString();
        }
        else
        {
            ProductName = "0";


        }

        return ProductName;

    }

    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = UM.GetUnitMasterById(Session["CompId"].ToString().ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
    }

    #region Purchase Request Search
    protected void btnbindRequest_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if ((ddlFieldNameRequest.SelectedItem.Value == "Request_Date") || (ddlFieldNameRequest.SelectedItem.Value == "Order_Date"))
        {
            if (txtValueRequestDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueRequestDate.Text);
                    txtValueRequest.Text = Convert.ToDateTime(txtValueRequestDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueRequestDate.Text = "";
                    txtValueRequest.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueRequestDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueRequestDate.Focus();
                txtValueRequest.Text = "";
                return;
            }
        }


        if (ddlOptionRequest.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlOptionRequest.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameRequest.SelectedValue + ",System.String)='" + txtValueRequest.Text.Trim() + "'";
            }
            else if (ddlOptionRequest.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameRequest.SelectedValue + ",System.String) like '%" + txtValueRequest.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameRequest.SelectedValue + ",System.String) Like '" + txtValueRequest.Text.Trim() + "%'";
            }

            DataTable dtPurchaseRequest = (DataTable)Session["dtPRequest"];


            DataView view = new DataView(dtPurchaseRequest, condition, "", DataViewRowState.CurrentRows);
            Session["dtPRequest"] = view.ToTable();
            lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPurchaseRequest, view.ToTable(), "", "");

            //AllPageCode();

            // btnRefreshRequest.Focus();

        }
        if (txtValueRequest.Text != "")
            txtValueRequest.Focus();
        else if (txtValueRequestDate.Text != "")
            txtValueRequestDate.Focus();
    }
    protected void btnRefreshRequest_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        btnPRequest_Click(sender, e);

        ddlFieldNameRequest.SelectedIndex = 0;
        ddlOptionRequest.SelectedIndex = 2;
        txtValueRequest.Text = "";

        txtValueRequest.Visible = true;
        txtValueRequestDate.Visible = false;
        txtValueRequestDate.Text = "";

        txtValueRequest.Focus();
    }

    #endregion

    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Job_Creation_Date" || ddlFieldName.SelectedItem.Value == "Request_Date")
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
        else
        {
            txtValueDate.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
        ddlFieldName.Focus();
    }

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "Job_Creation_Date" || ddlFieldNameBin.SelectedItem.Value == "Request_Date")
        {
            txtValueBinDate.Visible = true;
            txtValueBin.Visible = false;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
        else
        {
            txtValueBinDate.Visible = false;
            txtValueBin.Visible = true;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
        ddlFieldNameBin.Focus();
    }
    protected void ddlFieldNameRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if ((ddlFieldNameRequest.SelectedItem.Value == "Request_Date") || (ddlFieldNameRequest.SelectedItem.Value == "Order_Date"))
        {
            txtValueRequestDate.Visible = true;
            txtValueRequest.Visible = false;
            txtValueRequest.Text = "";
            txtValueRequestDate.Text = "";

        }
        else
        {
            txtValueRequestDate.Visible = false;
            txtValueRequest.Visible = true;
            txtValueRequest.Text = "";
            txtValueRequestDate.Text = "";

        }
    }

    #endregion

    #region Bom
    protected void ddlProductSerach_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSearchProductName.Text = "";
        txtProductId.Text = "";
        if (ddlProductSerach.SelectedIndex == 0)
        {
            txtProductId.Visible = true;
            txtSearchProductName.Visible = false;
        }
        else
        {
            txtProductId.Visible = false;
            txtSearchProductName.Visible = true;
        }
    }

    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtSearchProductName.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSearchProductName.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtSearchProductName.Text = dt.Rows[0]["EProductName"].ToString();
                    txtProductId.Text = dt.Rows[0]["ProductCode"].ToString();

                    DataTable dtProduct = new DataTable();
                    if (Session["DtBOM"] == null)
                    {
                    }
                    else
                    {
                        dtProduct = (DataTable)Session["DtBOM"];
                    }
                    if (dtProduct.Rows.Count > 0)
                    {
                        dtProduct = new DataView(dtProduct, "Item_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtProduct.Rows.Count > 0)
                        {
                            DisplayMessage("Product Is already exists!");
                            txtSearchProductName.Focus();
                            txtSearchProductName.Text = "";
                            return;
                        }
                    }

                    btnProductSave_Click(null, null);
                }
                else
                {
                    DisplayMessage("No Product Found");
                    txtSearchProductName.Text = "";
                    txtSearchProductName.Focus();
                    return;
                }

            }
            catch
            { }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSearchProductName);
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSearchProductName);
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductId.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductId.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {

                    txtSearchProductName.Text = dt.Rows[0]["EProductName"].ToString();
                    txtProductId.Text = dt.Rows[0]["ProductCode"].ToString();
                    DataTable dtProduct = new DataTable();
                    if (Session["DtBOM"] == null)
                    {


                    }
                    else
                    {
                        dtProduct = (DataTable)Session["DtBOM"];
                    }
                    if (dtProduct.Rows.Count > 0)
                    {
                        dtProduct = new DataView(dtProduct, "Item_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtProduct.Rows.Count > 0)
                        {
                            DisplayMessage("Product Is already exists!");
                            txtProductId.Focus();
                            txtProductId.Text = "";
                            return;
                        }
                        else
                        {
                            txtSearchProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                        }
                    }


                    btnProductSave_Click(null, null);

                }
                else
                {
                    DisplayMessage("No Product Found");
                    txtProductId.Text = "";
                    txtProductId.Focus();
                    return;
                }
            }
            catch
            {
            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
    }
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("Item_Id");
        dt.Columns.Add("Unit_Id");
        dt.Columns.Add("Sys_qty");
        dt.Columns.Add("Unit_Price");
        dt.Columns.Add("Req_Qty");
        dt.Columns.Add("Line_Total");
        dt.Columns.Add("ShortDescription");
        return dt;
    }
    public DataTable SavedGridRecordindatatble()
    {
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvRow in gvBom.Rows)
        {
            DataRow dr = dt.NewRow();

            Label lblTransId = (Label)gvRow.FindControl("lblTransId");
            HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdngvUnitId");
            Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
            TextBox txtgvUnitPrice = (TextBox)gvRow.FindControl("txtgvUnitPrice");
            TextBox txtRequestquantity = (TextBox)gvRow.FindControl("txtRequestquantity");
            Label lblgvtotal = (Label)gvRow.FindControl("lblgvtotal");

            dr["Id"] = lblTransId.Text;
            dr["Item_Id"] = hdngvProductId.Value;
            dr["Unit_Id"] = hdngvUnitId.Value;
            dr["Sys_qty"] = SetDecimal(lblgvSystemQuantity.Text);
            dr["Unit_Price"] = SetDecimal(txtgvUnitPrice.Text);
            dr["Req_Qty"] = SetDecimal(txtRequestquantity.Text);
            dr["Line_Total"] = SetDecimal(lblgvtotal.Text);
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        DataTable DtProduct = CreateProductDataTable();
        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;
        string UnitCost = string.Empty;
        string SearchCriteria = string.Empty;
        if (ddlProductSerach.SelectedIndex == 0)
        {
            if (txtProductId.Text == "")
            {
                DisplayMessage("Enter Product Id");
                txtProductId.Focus();
                return;
            }
            SearchCriteria = txtProductId.Text;
        }
        if (ddlProductSerach.SelectedIndex == 1)
        {
            if (txtSearchProductName.Text == "")
            {
                DisplayMessage("Enter Product Name");
                txtSearchProductName.Focus();
                return;
            }
            SearchCriteria = txtSearchProductName.Text;
        }

        if (SearchCriteria != "")
        {
            DataTable dt = new DataTable();
            if (ddlProductSerach.SelectedIndex == 0)
            {
                dt = new DataView(objProductM.GetProductMasterAll(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductCode='" + txtProductId.Text.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (ddlProductSerach.SelectedIndex == 1)
            {
                dt = new DataView(objProductM.GetProductMasterAll(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "EProductName='" + txtSearchProductName.Text.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (dt.Rows.Count != 0)
            {
                ProductId = dt.Rows[0]["ProductId"].ToString();
            }
            else
            {
                ProductId = "0";
            }
            UnitId = dt.Rows[0]["UnitId"].ToString();
            try
            {
                UnitCost = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", txtCustomer.Text.Split('/')[1].ToString(), ProductId).Rows[0]["Sales_Price"].ToString())).ToString();

            }
            catch
            {
                UnitCost = "0";
            }
        }
        int SerialNO = 0;
        DataTable dtProduct = new DataTable();
        if (Session["DtBOM"] == null)
        {
            dtProduct = CreateProductDataTable();
        }
        else
        {
            dtProduct = (DataTable)Session["DtBOM"];
        }
        if (dtProduct.Rows.Count > 0)
        {
            dtProduct = new DataView(dtProduct, "", "Id Desc", DataViewRowState.CurrentRows).ToTable();
            SerialNO = Convert.ToInt32(dtProduct.Rows[0]["Id"].ToString());
            SerialNO += 1;
        }
        else
        {
            SerialNO = 1;
        }
        DataRow dr;
        if (Session["DtBOM"] == null)
        {
            dr = DtProduct.NewRow();
        }
        else
        {
            DtProduct = SavedGridRecordindatatble();
            dr = DtProduct.NewRow();
        }
        dr["Id"] = SerialNO.ToString();
        dr["Item_Id"] = ProductId;
        dr["Unit_Id"] = UnitId;
        dr["Sys_qty"] = "0";
        dr["Unit_Price"] = SetDecimal(UnitCost);
        dr["Req_Qty"] = "0";
        dr["Line_Total"] = "0";



        try
        {
            dr["Sys_qty"] = SetDecimal(objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ProductId).Rows[0]["Quantity"].ToString());
        }
        catch
        {
            dr["Sys_qty"] = "0";
        }

        DtProduct.Rows.Add(dr);
        txtSearchProductName.Text = "";
        txtProductId.Text = "";
        Session["DtBOM"] = DtProduct;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvBom, DtProduct, "", "");
        //AllPageCode();
    }
    protected void txtgvUnitPrice_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox b = (TextBox)sender;
        string objSenderID = b.ID;


        if (((TextBox)gvRow.FindControl("txtRequestquantity")).Text == "")
        {
            ((TextBox)gvRow.FindControl("txtRequestquantity")).Text = "0";
        }

        if (((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text == "")
        {
            ((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text = "0";
        }
        ((TextBox)gvRow.FindControl("txtRequestquantity")).Text = SetDecimal(((TextBox)gvRow.FindControl("txtRequestquantity")).Text);
        ((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text = SetDecimal(((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text);

        try
        {
            ((Label)gvRow.FindControl("lblgvtotal")).Text = SetDecimal((float.Parse(((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text) * float.Parse(((TextBox)gvRow.FindControl("txtRequestquantity")).Text)).ToString());
        }
        catch
        {
            ((Label)gvRow.FindControl("lblgvtotal")).Text = "0";
        }

        if (objSenderID == "txtRequestquantity")
        {
            ((TextBox)gvRow.FindControl("txtgvUnitPrice")).Focus();
        }

    }

    public string SetDecimal(string amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
    }

    protected void IbtnPDDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = SavedGridRecordindatatble();

        try
        {
            dt = new DataView(dt, "Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        Session["DtBOM"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvBom, dt, "", "");
        //AllPageCode();

    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster ObjInvProductMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvProductMaster.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["locId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        DataTable dtTemp = dt.Copy();
        dt = new DataView(dt, "ProductCode like '%" + prefixText.ToString() + "%'", "ProductCode Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["EProductName"].ToString();
            }
        }
        return str;
    }

    #endregion

    #region JobDetail

    public void LoadVisitTask()
    {
        DataTable dt = new DataTable();
        if (Session["dtVisitTaskList"] != null)
        {


            dt = new DataTable();
            dt = (DataTable)Session["dtVisitTaskList"];



            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvVisitTask, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();

                contacts.Columns.Add("Id", typeof(int));
                contacts.Columns.Add("Emp_Name", typeof(string));
                contacts.Columns.Add("Job_Date", typeof(DateTime));
                contacts.Columns.Add("Start_Time", typeof(string));
                contacts.Columns.Add("Stop_Time", typeof(string));
                contacts.Columns.Add("Duration", typeof(string));
                contacts.Columns.Add("Machine_Name", typeof(string));

                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvVisitTask, contacts, "", "");
                int TotalColumns = gvVisitTask.Rows[0].Cells.Count;
                gvVisitTask.Rows[0].Cells.Clear();
                gvVisitTask.Rows[0].Cells.Add(new TableCell());
                gvVisitTask.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvVisitTask.Rows[0].Visible = false;
            }

        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Id", typeof(int));
            contacts.Columns.Add("Emp_Name", typeof(string));
            contacts.Columns.Add("Job_Date", typeof(DateTime));
            contacts.Columns.Add("Start_Time", typeof(string));
            contacts.Columns.Add("Stop_Time", typeof(string));
            contacts.Columns.Add("Duration", typeof(string));
            contacts.Columns.Add("Machine_Name", typeof(string));

            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvVisitTask, contacts, "", "");
            int TotalColumns = gvVisitTask.Rows[0].Cells.Count;
            gvVisitTask.Rows[0].Cells.Clear();
            gvVisitTask.Rows[0].Cells.Add(new TableCell());
            gvVisitTask.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvVisitTask.Rows[0].Visible = false;
        }





    }

    protected void gvVisitTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {



        DataTable dt = new DataTable();
        string EmpId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Text == "")
            {
                DisplayMessage("Enter Employee Name");
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Focus();
                return;
            }

            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtstartdate")).Text == "")
            {
                DisplayMessage("Enter Job date");
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtstartdate")).Focus();
                return;
            }

            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtStarttime")).Text == "")
            {
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtStarttime")).Text = "00:00";

            }
            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtStoptime")).Text == "")
            {
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtStoptime")).Text = "00:00";

            }


            if (Session["dtVisitTaskList"] == null)
            {
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("Emp_Name", typeof(string));
                dt.Columns.Add("Job_Date", typeof(DateTime));
                dt.Columns.Add("Start_Time", typeof(string));
                dt.Columns.Add("Stop_Time", typeof(string));
                dt.Columns.Add("Duration", typeof(string));
                dt.Columns.Add("Machine_Name", typeof(string));

                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Text;
                dr[2] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtstartdate")).Text;
                dr[3] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtStarttime")).Text;
                dr[4] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtStoptime")).Text;
                dr[5] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtDuration")).Text;
                dr[6] = ((DropDownList)gvVisitTask.FooterRow.FindControl("ddlMachineName")).SelectedValue;
                dt.Rows.Add(dr);
            }
            else
            {
                string strTransid = string.Empty;
                dt = (DataTable)Session["dtVisitTaskList"];
                if (dt.Rows.Count > 0)
                {
                    DataTable dtCopy = dt.Copy();
                    dtCopy = new DataView(dtCopy, "", "Id desc", DataViewRowState.CurrentRows).ToTable();
                    strTransid = (float.Parse(dtCopy.Rows[0]["Id"].ToString()) + 1).ToString();
                }
                else
                {
                    strTransid = "1";
                }

                DataRow dr = dt.NewRow();
                dr[0] = strTransid;
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Text;
                dr[2] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtstartdate")).Text;
                dr[3] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtStarttime")).Text;
                dr[4] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtStoptime")).Text;
                dr[5] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtDuration")).Text;
                dr[6] = ((DropDownList)gvVisitTask.FooterRow.FindControl("ddlMachineName")).SelectedValue;
                dt.Rows.Add(dr);
            }
            Session["dtVisitTaskList"] = dt;
            gvVisitTask.EditIndex = -1;
            LoadVisitTask();
        }
        if (e.CommandName.Equals("Delete"))
        {

            if (Session["dtVisitTaskList"] != null)
            {
                dt = (DataTable)Session["dtVisitTaskList"];
                dt = new DataView(dt, "Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtVisitTaskList"] = dt;
            }
            gvVisitTask.EditIndex = -1;
            LoadVisitTask();
        }

        ((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Focus();



    }
    protected void gvVisitTask_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvVisitTask.EditIndex = e.NewEditIndex;
        LoadVisitTask();
    }
    protected void gvVisitTask_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvVisitTask.EditIndex = -1;
        LoadVisitTask();
    }
    protected void gvVisitTask_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtVisitTaskList"];

        GridViewRow row = gvVisitTask.Rows[e.RowIndex];

        dt.Rows[row.DataItemIndex]["Emp_Name"] = ((TextBox)row.FindControl("txtempName")).Text;
        dt.Rows[row.DataItemIndex]["Job_Date"] = ((TextBox)row.FindControl("txteditstartdate")).Text;

        dt.Rows[row.DataItemIndex]["Start_Time"] = ((TextBox)row.FindControl("txteditstartime")).Text;
        dt.Rows[row.DataItemIndex]["Stop_Time"] = ((TextBox)row.FindControl("txteditstoptime")).Text;
        dt.Rows[row.DataItemIndex]["Machine_Name"] = ((DropDownList)row.FindControl("ddlEditMachineName")).SelectedValue;

        Session["dtVisitTaskList"] = dt;
        gvVisitTask.EditIndex = -1;
        LoadVisitTask();
    }
    protected void gvVisitTask_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        dt = new DataView(dt, "Emp_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Name"].ToString();
        }
        return txt;
    }
    #endregion

    #region Post

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //AllPageCode();

    }
    #endregion
    protected void txtlRequestNo_TextChanged(object sender, EventArgs e)
    {
        if (txtPINo.Text != "")
        {
            DataTable dt = new DataView(objProductionProcess.GetAllRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString()), "Job_No='" + txtPINo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                {
                    DisplayMessage("Voucher No Already Exists");

                }
                else
                {

                    DisplayMessage("Voucher No Already Exists :- Go To Bin Tab");
                }
                txtPINo.Text = "";
                txtPINo.Focus();
            }
            else
            {

            }
        }
    }
    public void getBom()
    {
        string labelMeterialCateId = "0";
        string PartNumber = string.Empty;
        DataTable dtBomDetail = new DataTable();
        DataTable dtOpcatdetail = objProOpCatedetail.GetAllRecord(Session["CompId"].ToString());

        if (dtOpcatdetail.Rows.Count > 0)
        {
            labelMeterialCateId = dtOpcatdetail.Rows[0]["MaterialCategoryId"].ToString();
        }

        foreach (GridViewRow gvrow in GvProduct.Rows)
        {
            string ProductId = ((Label)gvrow.FindControl("lblPID")).Text;
            string ProductCode = ((Label)gvrow.FindControl("lblproductcode")).Text;
            string RequestQty = ((TextBox)gvrow.FindControl("txtReqQty")).Text;
            string ProductName = ((Label)gvrow.FindControl("lblProductId")).Text;
            string ModelNo = objDa.return_DataTable("select Inv_ModelMaster.Model_No as ModelNo from Inv_ProductMaster inner join Inv_ModelMaster on Inv_ProductMaster.ModelNo = Inv_ModelMaster.Trans_Id where Inv_ProductMaster.ProductId =" + ProductId + "").Rows[0]["ModelNo"].ToString();





            //DataTable dtProduct = objProductM.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            //try
            //{
            //    dtProduct = new DataView(dtProduct, "ProductId=" + ProductId + "", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //catch
            //{
            //}
            try
            {
                PartNumber = ProductCode.Split('-')[1].ToString();
            }
            catch
            {
                PartNumber = ProductCode;
            }

            DataTable dtBom = ObjInvBOM.BOM_All(Session["CompId"].ToString());
            try
            {
                dtBom = new DataView(dtBom, "ModelNo='" + ModelNo + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }

            if (dtBom.Rows.Count == 0)
            {
                DisplayMessage("Bom not found");
                return;
            }

            //  DataTable dtBom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), dtProduct.Rows[0]["ModelNo"].ToString());
            for (int i = 0; i < PartNumber.Length; i++)
            {
                string srno = (i + 1).ToString();
                string Charvalue = PartNumber[i].ToString();

                if (Charvalue != "0")
                {

                    if (Charvalue == "#")
                    {
                        break;
                    }

                    DataTable dtTemp = new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    DataRow dr;
                    if (Session["DtBOM"] == null)
                    {
                        dtBomDetail = CreateProductDataTable();
                        dr = dtBomDetail.NewRow();
                        dr["Id"] = Convert.ToInt32(srno);
                        dr["Item_Id"] = dtTemp.Rows[0]["SubProductID"].ToString();
                        DataTable dtitemdetail = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), dtTemp.Rows[0]["SubProductID"].ToString().Trim(), HttpContext.Current.Session["FinanceYearId"].ToString());

                        if (dtTemp.Rows[0]["SubProductID"].ToString().Trim() != "0")
                        {
                            dr["Unit_Id"] = dtitemdetail.Rows[0]["UnitId"].ToString();
                        }
                        else
                        {
                            dr["Unit_Id"] = "0";
                        }
                        try
                        {

                            dr["Sys_qty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dtTemp.Rows[0]["SubProductID"].ToString().Trim()).Rows[0]["Quantity"].ToString();
                        }
                        catch
                        {
                            dr["Sys_qty"] = "0";
                        }
                        if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == labelMeterialCateId)
                        {
                            try
                            {
                                dr["Unit_Price"] = new DataView(objStock.GetOpeningStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId=" + dtTemp.Rows[0]["SubProductID"].ToString().Trim() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["UnitPrice"].ToString();
                            }
                            catch
                            {
                                dr["Unit_Price"] = "0";
                            }


                            dr["Req_Qty"] = CalculateqtyforLabel(ModelNo, PartNumber, dtitemdetail.Rows[0]["UnitId"].ToString(), RequestQty);

                        }
                        else
                        {
                            dr["Unit_Price"] = SetDecimal(dtTemp.Rows[0]["UnitPrice"].ToString());


                            dr["Req_Qty"] = SetDecimal((float.Parse(dtTemp.Rows[0]["Quantity"].ToString()) * float.Parse(RequestQty)).ToString());

                        }

                        if (dr["Unit_Price"].ToString() == "")
                        {
                            dr["Unit_Price"] = "0";
                        }

                        dr["Line_Total"] = SetDecimal((float.Parse(dr["Unit_Price"].ToString()) * (float.Parse(dr["Req_Qty"].ToString()))).ToString());
                        //if it is inventory item then showing product name
                        if (dtTemp.Rows[0]["SubProductID"].ToString().Trim() != "0")
                        {
                            dr["ShortDescription"] = dtitemdetail.Rows[0]["EProductName"].ToString();
                        }
                        else
                        {
                            dr["ShortDescription"] = dtTemp.Rows[0]["ShortDescription"].ToString();
                        }

                        dtBomDetail.Rows.Add(dr);
                        Session["DtBOM"] = dtBomDetail;
                    }
                    else
                    {
                        dtBomDetail = (DataTable)Session["DtBOM"];
                        DataTable dtExistProductId = new DataTable();


                        if (dtTemp.Rows[0]["SubProductID"].ToString().Trim() != "0")
                        {
                            dtExistProductId = new DataView(dtBomDetail, "Item_Id=" + dtTemp.Rows[0]["SubProductID"].ToString().Trim() + "", "", DataViewRowState.CurrentRows).ToTable();

                        }
                        else
                        {
                            dtExistProductId = new DataView(dtBomDetail, "Item_Id=" + dtTemp.Rows[0]["SubProductID"].ToString().Trim() + " and ShortDescription='" + dtTemp.Rows[0]["ShortDescription"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

                        }

                        if (dtExistProductId.Rows.Count == 0)
                        {

                            dr = dtBomDetail.NewRow();
                            dr["Id"] = Convert.ToInt32(srno);
                            dr["Item_Id"] = dtTemp.Rows[0]["SubProductID"].ToString();

                            DataTable dtitemdetail = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), dtTemp.Rows[0]["SubProductID"].ToString().Trim(), HttpContext.Current.Session["FinanceYearId"].ToString());
                            if (dtTemp.Rows[0]["SubProductID"].ToString().Trim() != "0")
                            {
                                dr["Unit_Id"] = dtitemdetail.Rows[0]["UnitId"].ToString();
                            }
                            else
                            {
                                dr["Unit_Id"] = "0";
                            }
                            try
                            {

                                dr["Sys_qty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dtTemp.Rows[0]["SubProductID"].ToString().Trim()).Rows[0]["Quantity"].ToString();
                            }
                            catch
                            {
                                dr["Sys_qty"] = "0";
                            }

                            if (dtTemp.Rows[0]["OptionCategoryID"].ToString() == labelMeterialCateId)
                            {
                                try
                                {
                                    dr["Unit_Price"] = new DataView(objStock.GetOpeningStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId=" + dtTemp.Rows[0]["SubProductID"].ToString().Trim() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["UnitPrice"].ToString();
                                }
                                catch
                                {
                                    dr["Unit_Price"] = "0";
                                }

                                dr["Req_Qty"] = CalculateqtyforLabel(ModelNo, PartNumber, dtitemdetail.Rows[0]["UnitId"].ToString(), RequestQty);

                            }
                            else
                            {
                                dr["Unit_Price"] = SetDecimal(dtTemp.Rows[0]["UnitPrice"].ToString());
                                dr["Req_Qty"] = SetDecimal((float.Parse(dtTemp.Rows[0]["Quantity"].ToString()) * float.Parse(RequestQty)).ToString());
                            }

                            if (dr["Unit_Price"].ToString() == "")
                            {
                                dr["Unit_Price"] = "0";
                            }

                            dr["Line_Total"] = SetDecimal((float.Parse(dr["Unit_Price"].ToString()) * (float.Parse(dr["Req_Qty"].ToString()))).ToString());

                            if (dtTemp.Rows[0]["SubProductID"].ToString().Trim() != "0")
                            {
                                dr["ShortDescription"] = dtitemdetail.Rows[0]["EProductName"].ToString();
                            }
                            else
                            {
                                dr["ShortDescription"] = dtTemp.Rows[0]["ShortDescription"].ToString();
                            }

                            dtBomDetail.Rows.Add(dr);
                        }
                        else
                        {

                            foreach (DataRow drBom in dtBomDetail.Rows)
                            {
                                if (drBom["Item_Id"].ToString() == dtExistProductId.Rows[0]["Item_Id"].ToString() && drBom["ShortDescription"].ToString() == dtExistProductId.Rows[0]["ShortDescription"].ToString())
                                {
                                    drBom["Req_Qty"] = SetDecimal((float.Parse(dtExistProductId.Rows[0]["Req_Qty"].ToString()) + (float.Parse(dtTemp.Rows[0]["Quantity"].ToString()) * float.Parse(RequestQty))).ToString());

                                    drBom["Line_Total"] = SetDecimal((float.Parse(dtTemp.Rows[0]["UnitPrice"].ToString()) * (float.Parse(dtExistProductId.Rows[0]["Req_Qty"].ToString()) + (float.Parse(dtTemp.Rows[0]["Quantity"].ToString()) * float.Parse(RequestQty)))).ToString());

                                    break;
                                }
                            }

                        }


                        Session["DtBOM"] = dtBomDetail;
                    }
                }
            }
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvBom, dtBomDetail, "", "");

        }


    }
    public string CalculateqtyforLabel(string ModelId, string PartNumber, string UnitId, string RequestQty)
    {

        string Usedquantity = "0";
        try
        {
            DataTable dt = new DataTable();
            DataTable dtmodellabel = new DataTable();


            dt = objProOpCatedetail.GetAllRecord(Session["CompId"].ToString().ToString());
            string sql = "select Inv_Model_LabelSize.* from Inv_Model_LabelSize inner join Inv_ModelMaster on Inv_Model_LabelSize.Model_Id= Inv_ModelMaster.Trans_Id where Inv_ModelMaster.Model_No='" + ModelId + "'";
            dtmodellabel = objDa.return_DataTable(sql);


            DataTable dtBom = ObjInvBOM.BOM_All(Session["CompId"].ToString());
            try
            {
                dtBom = new DataView(dtBom, "ModelNo='" + ModelId + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }

            float labelQty = 0;
            float labelpacking = 0;
            float ColorPrice = 0;
            float MaterialPrice = 0;
            float PaperSize = 0;
            string labelQtyCateId = dt.Rows[0]["QtyCategoryId"].ToString();
            string labelMeterialCateId = dt.Rows[0]["MaterialCategoryId"].ToString();
            string labelColorCateId = dt.Rows[0]["ColorCategoryId"].ToString();
            string LabelSizeCatId = dt.Rows[0]["OptionCategoryId"].ToString();
            string strlPackingCategoryid = dt.Rows[0]["Field1"].ToString();

            for (int i = 0; i < PartNumber.Length; i++)
            {
                string srno = (i + 1).ToString();
                string Charvalue = PartNumber[i].ToString();

                if (Charvalue == "#")
                {
                    dtmodellabel = new DataView(dtmodellabel, "Field2='" + PartNumber.Split('#')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    PaperSize = float.Parse(dtmodellabel.Rows[0]["MMSize"].ToString()) / float.Parse(dt.Rows[0]["DefaultValue"].ToString());
                    break;
                }
                DataTable dtTemp = new DataView(dtBom, "Field1='" + srno + "' and OptionId='" + Charvalue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtTemp.Rows.Count > 0)
                {
                    if (labelQtyCateId.Trim() == dtTemp.Rows[0]["OptionCategoryID"].ToString())
                    {
                        labelQty = float.Parse(dtTemp.Rows[0]["Quantity"].ToString());

                    }
                    if (strlPackingCategoryid.Trim() == dtTemp.Rows[0]["OptionCategoryID"].ToString())
                    {
                        labelpacking = float.Parse(dtTemp.Rows[0]["Quantity"].ToString());

                    }
                }
            }

            float Price = PaperSize * labelQty * labelpacking * float.Parse(RequestQty);
            Usedquantity = Price.ToString();
        }
        catch
        {
            Usedquantity = "0";
        }

        return Usedquantity;
    }
    #region Print
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        string strCmd = string.Format("window.open('../Production_Report/ProductionFinishReport.aspx?Id=" + e.CommandArgument.ToString() + "&&Type=ORDER','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }


    protected void IbtnPrintVoucher_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        string strCmd = string.Format("window.open('../Production_Report/ProductionFinishReport.aspx?Id=" + e.CommandArgument.ToString() + "&&Type=VOUCHER','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion



    protected void txtReqQty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        try
        {
            Convert.ToDouble(((TextBox)gvRow.FindControl("txtReqQty")).Text);
        }
        catch
        {
            DisplayMessage("Invalid Quantity");
            ((TextBox)gvRow.FindControl("txtReqQty")).Text = ((Label)gvRow.FindControl("lblRemainQty")).Text;

        }

        if (Convert.ToDouble(((TextBox)gvRow.FindControl("txtReqQty")).Text) > Convert.ToDouble(((Label)gvRow.FindControl("lblRemainQty")).Text))
        {
            DisplayMessage("Production quantity can not greater then remain quantity");
            ((TextBox)gvRow.FindControl("txtReqQty")).Text = ((Label)gvRow.FindControl("lblRemainQty")).Text;

        }

        Session["DtBOM"] = null;
        getBom();

        ((Label)gvRow.FindControl("lblLinetotal")).Text = SetDecimal((Convert.ToDouble(((TextBox)gvRow.FindControl("txtReqQty")).Text) * Convert.ToDouble(((Label)gvRow.FindControl("lblunitPrice")).Text)).ToString());
    }

    protected void lblprocessedQty_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = objDa.return_DataTable("select Inv_Production_Process.job_no,Inv_Production_Process.Job_Creation_Date,Inv_ProductMaster.ProductCode,Inv_ProductMaster.eproductname,Inv_Production_Process_Detail.Production_Qty from Inv_Production_Process  inner join Inv_Production_Process_Detail on Inv_Production_Process.Id = Inv_Production_Process_Detail.Ref_Job_No inner join Inv_ProductMaster on Inv_Production_Process_Detail.ProductId =Inv_ProductMaster.ProductId where Inv_Production_Process.IsActive='True' and Inv_Production_Process.Ref_Production_Req_No=" + ((Label)((GridViewRow)((LinkButton)sender).Parent.Parent).FindControl("lblRequestId")).Text + " and Inv_Production_Process_Detail.ProductId=" + e.CommandArgument.ToString().Trim() + " order by Inv_Production_Process.Job_Creation_Date");

        objPageCmn.FillData((object)gvProcessedhistory, dt, "", "");

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }

    protected void btnBackOrder_Command(object sender, CommandEventArgs e)
    {
        hdnrequestid.Value = e.CommandArgument.ToString();
        DataTable dtrequestDetail = ObjProductionRequestDetail.GetRecord_By_RequestNo(Session["Compid"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        dtrequestDetail = new DataView(dtrequestDetail, "Remain_Qty>0", "", DataViewRowState.CurrentRows).ToTable();

        objPageCmn.FillData((object)gvBackorder, dtrequestDetail, "", "");
        objPageCmn.FillData((object)gvProcessedhistory, null, "", "");

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }



    protected void btnSettlementSave_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvBackorder.Rows)
        {
            objDa.execute_Command("update Inv_ProductionRequestDetail set Field2 = Quantity,Field3='" + ((Label)gvrow.FindControl("lblRemainQty")).Text + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate=GETDATE()  where Request_No = " + hdnrequestid.Value + " and ProductId =" + ((Label)gvrow.FindControl("lblPID")).Text + "");
        }


        FillRequestGrid();

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", " $('#PartialRequest').modal('hide');alert('Record saved successfully')", true);

        //DisplayMessage("Record saved successfully");
    }



    protected void btnAdjustbackOrder_Click(object sender, EventArgs e)
    {
        bool isselect = false;


        foreach (GridViewRow gvrow in GvPurchaseRequest.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkgvselect")).Checked)
            {
                isselect = true;
                objDa.execute_Command("update Inv_ProductionRequestDetail set Field2 = Quantity,Field3='" + ((Label)gvrow.FindControl("lblRemainQty")).Text + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate=GETDATE()  where Request_No = " + ((Label)gvrow.FindControl("lblRequestId")).Text + " and ProductId =" + ((Label)gvrow.FindControl("lblPID")).Text + "");
            }
        }

        if (!isselect)
        {
            DisplayMessage("Please select record");
        }
        else
        {
            FillRequestGrid();
            DisplayMessage("Back Order Adjusted successfully");
        }
    }

    protected void txteditstartime_TextChanged(object sender, EventArgs e)
    {
        if (((TextBox)gvVisitTask.FooterRow.FindControl("txtStarttime")).Text == "")
        {
            ((TextBox)gvVisitTask.FooterRow.FindControl("txtStarttime")).Text = "00:00";

        }
        if (((TextBox)gvVisitTask.FooterRow.FindControl("txtStoptime")).Text == "")
        {
            ((TextBox)gvVisitTask.FooterRow.FindControl("txtStoptime")).Text = "00:00";

        }


        if (((TextBox)gvVisitTask.FooterRow.FindControl("txtStoptime")).Text == "00:00" || ((TextBox)gvVisitTask.FooterRow.FindControl("txtStarttime")).Text == "00:00")
        {
            ((TextBox)gvVisitTask.FooterRow.FindControl("txtDuration")).Text = "00:00";
        }
        else
        {
            ((TextBox)gvVisitTask.FooterRow.FindControl("txtDuration")).Text = GetHours(GetMinuteDiff(((TextBox)gvVisitTask.FooterRow.FindControl("txtStoptime")).Text, ((TextBox)gvVisitTask.FooterRow.FindControl("txtStarttime")).Text));
        }



    }


    public string GetHours(object obj)
    {
        if (obj.ToString() == "")
        {
            return "";
        }
        string retval = string.Empty;
        retval = ((Convert.ToInt32(obj) / 60) < 10) ? "0" + (Convert.ToInt32(obj) / 60).ToString() : ((Convert.ToInt32(obj) / 60)).ToString();
        retval += ":" + (((Convert.ToInt32(obj) % 60) < 10) ? "0" + (Convert.ToInt32(obj) % 60) : (Convert.ToInt32(obj) % 60).ToString());

        return retval;
    }

    private int GetMinuteDiff(string greatertime, string lesstime)
    {

        if (greatertime == "__:__:__" || greatertime == "" || greatertime == "00:00:00")
        {
            return 0;
        }
        if (lesstime == "__:__:__" || lesstime == "" || lesstime == "00:00:00")
        {
            return 0;
        }
        int retval = 0;
        int actTimeHour = Convert.ToInt32(greatertime.Split(':')[0]);



        int ondutyhour = Convert.ToInt32(lesstime.Split(':')[0]);


        int actTimeMinute = Convert.ToInt32(greatertime.Split(':')[1]);
        int ondutyMinute = Convert.ToInt32(lesstime.Split(':')[1]);
        int totalActTimeMinute = actTimeHour * 60 + actTimeMinute;
        int totalOnDutyTimeMinute = ondutyhour * 60 + ondutyMinute;
        if (totalActTimeMinute - totalOnDutyTimeMinute < 0)
        {
            retval = 1440 + (totalActTimeMinute - totalOnDutyTimeMinute);
            //retval = 0;
        }
        else
        {
            retval = (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        return retval;
    }
}