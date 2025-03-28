using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;

public partial class Production_ProductionRequest : System.Web.UI.Page
{
    #region Class Object
    Inv_SalesOrderDetail ObjSalesOrderDetail = null;
    DataAccessClass objDa = null;
    Set_Approval_Employee objEmpApproval = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_ProductionRequestDetail ObjProductionRequestDetail = null;
    Inv_ProductionRequestHeader ObjProductionReqestHeader = null;
    Inv_UnitMaster objUnit = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_StockDetail objStockDetail = null;
    LocationMaster objLocation = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Common cmn = null;
    Inv_PurchaseInquiryHeader objPIHeader = null;
    Inv_ParameterMaster objInvParam = null;
    DepartmentMaster ObjDept = null;
    EmployeeMaster objEmployee = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_SalesOrderDetail ObjSorderDetail = null;
    HR_EmployeeDetail HR_EmployeeDetail = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjSalesOrderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjProductionRequestDetail = new Inv_ProductionRequestDetail(Session["DBConnection"].ToString());
        ObjProductionReqestHeader = new Inv_ProductionRequestHeader(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        ObjSorderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Production/ProductionRequest.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtlRequestNo.Text = GetDocumentNum();
            txtCalenderExtender.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();

            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtRequestdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtSODate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtTo_CalendarExtender2.Format = Session["DateFormat"].ToString();
            Fillgrid();
            Reset();
            txtlRequestNo.Focus();
            txtValue.Focus();
            Session["DtSearchProduct"] = null;
            FillddlLocation();
            ddlFieldName.Focus();
            FillddlOrderLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            ddlorderlocation.SelectedValue = Session["LocId"].ToString();
           // FillSalesOrder();
            Fillcategory();
            GetPendingOrder();
        }

        //AllPageCode();
    }

    private void FillddlOrderLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");

        //Common Function add By jitendra on 06-02-2016
        ddlorderlocation.DataSource = dtLoc;
        ddlorderlocation.DataTextField = "Location_Name";
        ddlorderlocation.DataValueField = "Location_Id";
        ddlorderlocation.DataBind();
        //objPageCmn.FillData((object)ddlorderlocation, dtLoc, "Location_Name", "Location_Id");

    }



    public void GetPendingOrder()
    {

        DataTable dt = ObjSalesOrderDetail.getSODetailDataForProduction(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlorderlocation.SelectedValue, Session["FinanceYearId"].ToString());
        if (ddlCategory.SelectedIndex > 0)
        {
            dt = new DataView(dt, "Category_Id="+ddlCategory.SelectedValue.Trim()+"", "", DataViewRowState.CurrentRows).ToTable();
        }
        dt = dt.DefaultView.ToTable(true,"Name", "Location_Name", "location_id", "SalesOrderNo", "trans_id", "salesorderdate", "CustomerId", "ProductCode", "Product_Id", "UnitPrice", "EproductName", "Unit_name", "Unit_Id", "Quantity", "sysqty");
        gvPendingSalesOrder.DataSource = dt;
        gvPendingSalesOrder.DataBind();
        lblOrderTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
        Session["dtSorder"] = dt;
    }

    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    #region System Function :-

    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtbinValue.Focus();
        txtbinValue.Text = "";
    }
    protected void btnOrder_Click(object sender, EventArgs e)
    {

        txtbinValue.Focus();
        txtbinValue.Text = "";
        //FillSalesOrder();
    }
    protected void btnPost_Click(object sender, EventArgs e)
    {

        chkpost.Checked = true;
        btnSave_Click(null, null);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        if (txtRequestdate.Text == "")
        {
            DisplayMessage("Enter Request Date");
            txtRequestdate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtRequestdate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Request Date");
                txtRequestdate.Focus();
                return;
            }
        }

        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtRequestdate.Text), "I", Session["DBConnection"].ToString(),
Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }





        if (txtlRequestNo.Text == "")
        {
            DisplayMessage("Enter Request No.");
            txtlRequestNo.Focus();
            return;
        }

        if (txtCustomer.Text == "")
        {
            DisplayMessage("Enter Customer Name");
            txtCustomer.Focus();
            return;
        }


        string strSalesPersonId = string.Empty;
        if (txtSalesPerson.Text != "")
        {
            try
            {
                strSalesPersonId = HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString());
            }
            catch
            {
                strSalesPersonId = "0";
            }
        }
        else
        {
            strSalesPersonId = "0";
        }




        if (gvProductRequest.Rows.Count == 0)
        {
            ViewState["Return"] = 1;
            DisplayMessage("Enter Product Details");

            return;

        }
        if (txtTermCondition.Text == "")
        {
            DisplayMessage("Enter Description");

            txtTermCondition.Focus();
            return;
        }
        string sodate = string.Empty;
        if (txtSODate.Text == "")
        {
            sodate = "01/01/1900";
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtSODate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid order date");
                txtSODate.Focus();
                return;
            }
            sodate = ObjSysParam.getDateForInput(txtSODate.Text).ToString();
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();


        try
        {

            int b = 0;
            string sql = string.Empty;

            if (editid.Value == "")
            {
                b = ObjProductionReqestHeader.InsertRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), txtlRequestNo.Text, ObjSysParam.getDateForInput(txtRequestdate.Text).ToString(), txtCustomer.Text.Split('/')[1].ToString(), txtSONo.Text, sodate, strSalesPersonId, txtTermCondition.Text, true.ToString(), chkCancel.Checked.ToString(), chkpost.Checked.ToString(), false.ToString(), false.ToString(), hdnOrderId.Value, ddlLocation.SelectedValue, "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                //b = 1;

                editid.Value = b.ToString();
                if (txtlRequestNo.Text == ViewState["DocNo"].ToString())
                {
                    DataTable dtCount = ObjProductionReqestHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                    if (dtCount.Rows.Count == 0)
                    {
                        ObjProductionReqestHeader.Updatecode(b.ToString(), txtlRequestNo.Text + "1", ref trns);
                        txtlRequestNo.Text = txtlRequestNo.Text + "1";
                    }
                    else
                    {
                        ObjProductionReqestHeader.Updatecode(b.ToString(), txtlRequestNo.Text + dtCount.Rows.Count, ref trns);
                        txtlRequestNo.Text = txtlRequestNo.Text + dtCount.Rows.Count;
                    }

                }
                foreach (GridViewRow gvr in gvProductRequest.Rows)
                {
                    Label lblSerialNo = (Label)gvr.FindControl("lblSerialNO");
                    Label lblProductId = (Label)gvr.FindControl("lblPID");
                    Label lblUnitId = (Label)gvr.FindControl("lblUID");
                    Label lblReqQty = (Label)gvr.FindControl("lblReqQty");
                    Label lblunitPrice = (Label)gvr.FindControl("lblunitPrice");


                    ObjProductionRequestDetail.InsertRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), editid.Value, lblSerialNo.Text, lblProductId.Text, lblUnitId.Text, lblReqQty.Text, lblunitPrice.Text, lblunitPrice.Text, "0", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                }
                if (b != 0)
                {
                    DisplayMessage("Record Saved", "green");

                }
                //sqltrans.Commit();


            }
            else
            {

                b = ObjProductionReqestHeader.UpdateRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), editid.Value, txtlRequestNo.Text, ObjSysParam.getDateForInput(txtRequestdate.Text).ToString(), txtCustomer.Text.Split('/')[1].ToString(), txtSONo.Text, sodate, strSalesPersonId, txtTermCondition.Text, true.ToString(), chkCancel.Checked.ToString(), chkpost.Checked.ToString(), hdnOrderId.Value, ddlLocation.SelectedValue, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);


                ObjProductionRequestDetail.DeleteRecord_By_RequestNo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), editid.Value, ref trns);

                foreach (GridViewRow gvr in gvProductRequest.Rows)
                {
                    Label lblSerialNo = (Label)gvr.FindControl("lblSerialNO");
                    Label lblProductId = (Label)gvr.FindControl("lblPID");
                    Label lblUnitId = (Label)gvr.FindControl("lblUID");
                    Label lblReqQty = (Label)gvr.FindControl("lblReqQty");
                    Label lblunitPrice = (Label)gvr.FindControl("lblunitPrice");


                    ObjProductionRequestDetail.InsertRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), editid.Value, lblSerialNo.Text, lblProductId.Text, lblUnitId.Text, lblReqQty.Text, lblunitPrice.Text, lblunitPrice.Text, "0", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                }


                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }

            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Fillgrid();
            Reset();
            txtRequestdate.Focus();
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
    protected void btnReset_Click(object sender, EventArgs e)
    {

        Reset();
        txtlRequestNo.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;

        DataTable dt = ObjProductionReqestHeader.GetRecord_By_TransId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
            }
            else
            {
                if (Convert.ToBoolean(dt.Rows[0]["Is_Post"].ToString()))
                {
                    DisplayMessage("Recors is posted ,you can not edit");
                    return;
                }

                Lbl_Tab_New.Text = Resources.Attendance.Edit;


            }

            txtRequestdate.Focus();

            if (Lbl_Tab_New.Text == "View")
            {
                btnSave.Visible = false;
                btnReset.Visible = false;
                btnPost.Visible = false;
            }
            else
            {
                btnReset.Visible = true;
            }

            editid.Value = e.CommandArgument.ToString();
            txtRequestdate.Text = Convert.ToDateTime(dt.Rows[0]["Request_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtlRequestNo.Text = dt.Rows[0]["Request_No"].ToString();
            txtlRequestNo.Enabled = false;
            txtCustomer.Text = dt.Rows[0]["Customername"].ToString() + "/" + dt.Rows[0]["Customer_Id"].ToString();
            txtSONo.Text = dt.Rows[0]["Order_No"].ToString();
            if (dt.Rows[0]["Order_Date"].ToString() != "1/1/1900 12:00:00 AM")
            {
                txtSODate.Text = Convert.ToDateTime(dt.Rows[0]["Order_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            if (dt.Rows[0]["Sales_Person"].ToString() != "0")
            {
                txtSalesPerson.Text = dt.Rows[0]["SalesPersonName"].ToString() + "/" + HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Sales_Person"].ToString());
            }
            hdnOrderId.Value = dt.Rows[0]["Field1"].ToString();

            ddlLocation.SelectedValue = dt.Rows[0]["Field2"].ToString();

            txtTermCondition.Text = dt.Rows[0]["Remarks"].ToString();
            chkCancel.Checked = Convert.ToBoolean(dt.Rows[0]["Is_Cancel"].ToString());

            DataTable dtdetail = ObjProductionRequestDetail.GetRecord_By_RequestNo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString());

            dtdetail = dtdetail.DefaultView.ToTable(true, "Trans_Id", "Serial_No", "ProductId", "UnitId", "Quantity", "UnitPrice");
            Session["DtRequestProduct"] = dtdetail;
            objPageCmn.FillData((object)gvProductRequest, dtdetail, "", "");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 3;

        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;

        txtValue.Focus();
        Fillgrid();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() == "True")
        {
            DisplayMessage("Record is posted ,you can not delete");
            return;
        }

        ObjProductionReqestHeader.DeleteRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
        Fillgrid();
        DisplayMessage("Record Deleted");
        try
        {
            int i = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            ((LinkButton)gvPurchaseRequest.Rows[i].FindControl("IbtnDelete")).Focus();
        }
        catch
        {
            txtValue.Focus();
        }


    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Request_Date" || ddlFieldName.SelectedItem.Value == "Order_Date")
        {
            if (txtValueDate.Text != "")
            {

                try
                {

                    txtValue.Text = ObjSysParam.getDateForInput(txtValueDate.Text).ToString();
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
            DataTable dtPurchaseRequest = (DataTable)Session["DtPurchaseRequest"];


            DataView view = new DataView(dtPurchaseRequest, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Produ_Req"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";


            //bind gridview by function in common class

            objPageCmn.FillData((object)gvPurchaseRequest, view.ToTable(), "", "");

            //AllPageCode();

            // btnRefresh.Focus();

        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void btnClosePanel_Click(object sender, ImageClickEventArgs e)
    {
        pnlProduct1.Visible = false;
        txtTermCondition.Focus();
    }
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("Trans_Id", typeof(int));
        DtProduct.Columns.Add("Serial_No", typeof(int));
        DtProduct.Columns.Add("ProductId");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("Quantity");
        DtProduct.Columns.Add("UnitPrice");
        string PDiscription = string.Empty;

        if (txtProductName.Text == "")
        {
            DisplayMessage("Enter Product Name");
            txtProductName.Text = "";
            txtProductName.Focus();
            return;
        }

        if (ddlUnit == null)
        {
            DisplayMessage("Unit Not Found");
            ddlUnit.Focus();
            return;
        }
        else
        {
            if (ddlUnit.Items.Count == 0)
            {
                DisplayMessage("Unit Not Found");
                ddlUnit.Focus();
                return;
            }

        }
        //if (ddlUnit.SelectedIndex == 0)
        //{
        //    DisplayMessage("Select Unit");
        //    ddlUnit.SelectedIndex = 0;
        //    ddlUnit.Focus();
        //    return;

        //}
        if (txtRequestQty.Text == "")
        {
            txtRequestQty.Text = "1";
        }

        if (txtUnitPrice.Text == "")
        {
            DisplayMessage("Enter Unit Price");
            txtUnitPrice.Focus();
            return;
        }
        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;
        string SuggestedProductName = string.Empty;
        string ProductName = string.Empty;



        if (txtProductName.Text != "")
        {
            DataTable dt = new DataView(ObjProductMaster.GetProductMasterAll(Session["CompId"].ToString().ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "EProductName='" + txtProductName.Text.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                PDiscription = txtPDescription.Text;

                ProductId = dt.Rows[0]["ProductId"].ToString();
            }

        }

        UnitId = ddlUnit.SelectedValue.ToString();

        if (hidProduct.Value == "" || hidProduct.Value == "0")
        {



            if (Session["DtRequestProduct"] != null)
            {

                DtProduct = (DataTable)Session["DtRequestProduct"];

            }

            if (Session["DtRequestProduct"] == null)
            {
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                dr["Serial_No"] = "1";
                dr["ProductId"] = ProductId.ToString();
                dr["UnitId"] = UnitId.ToString();
                dr["Quantity"] = txtRequestQty.Text.ToString();
                dr["UnitPrice"] = txtUnitPrice.Text.ToString();
                DtProduct.Rows.Add(dr);
                Session["DtRequestProduct"] = (DataTable)DtProduct;
            }
            else
            {
                DtProduct = (DataTable)Session["DtRequestProduct"];
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                DataTable dtserialNo = new DataTable();
                try
                {
                    dtserialNo = new DataView(DtProduct, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                try
                {
                    dr["Serial_No"] = (float.Parse(dtserialNo.Rows[0]["Serial_No"].ToString()) + 1).ToString();
                }
                catch
                {
                    dr["Serial_No"] = 1;
                }

                dr["ProductId"] = ProductId.ToString();
                dr["UnitId"] = UnitId.ToString();
                dr["Quantity"] = txtRequestQty.Text.ToString();
                dr["UnitPrice"] = txtUnitPrice.Text.ToString();

                DtProduct.Rows.Add(dr);
                Session["DtRequestProduct"] = (DataTable)DtProduct;

            }

        }
        else
        {

            DataTable dt = (DataTable)Session["DtRequestProduct"];


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                DataRow dr = DtProduct.NewRow();
                if (dt.Rows[i]["Trans_Id"].ToString() == hidProduct.Value)
                {
                    dr["Trans_Id"] = hidProduct.Value;
                    dr["Serial_No"] = dt.Rows[i]["Serial_No"].ToString();
                    dr["ProductId"] = ProductId.ToString();
                    dr["UnitId"] = UnitId.ToString();
                    dr["Quantity"] = txtRequestQty.Text.ToString();
                    dr["UnitPrice"] = txtUnitPrice.Text.ToString();
                    DtProduct.Rows.Add(dr);

                }
                else
                {
                    dr["Trans_Id"] = dt.Rows[i]["Trans_Id"].ToString();
                    dr["Serial_No"] = dt.Rows[i]["Serial_No"].ToString();
                    dr["Product_Id"] = dt.Rows[i]["ProductId"].ToString();
                    dr["UnitId"] = dt.Rows[i]["UnitId"].ToString();
                    dr["Quantity"] = dt.Rows[i]["Quantity"].ToString();
                    dr["UnitPrice"] = dt.Rows[i]["UnitPrice"].ToString();
                    DtProduct.Rows.Add(dr);
                }
            }
        }

        //fillgridDetail();  //comment by jitendra
        Session["DtRequestProduct"] = (DataTable)DtProduct;


        //bind gridview by function in common class

        objPageCmn.FillData((object)gvProductRequest, DtProduct, "", "");

        //AllPageCode();
        ResetDetail();
        txtProductcode.Focus();

    }
    protected void btnEdit_Command1(object sender, CommandEventArgs e)
    {
        pnlProduct1.Visible = true;
        hidProduct.Value = e.CommandArgument.ToString();

        //updated by jitendra upadhyay on 26-oct-2013 after create the dynamic table for product gridview
        DataTable dtproduct = new DataTable();
        dtproduct = (DataTable)Session["DtRequestProduct"];

        DataTable dt = new DataView(dtproduct, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["ProductId"].ToString() != "0")
            {
                txtProductName.Text = ProductName(dt.Rows[0]["ProductId"].ToString());
                //when we edit the product then we showing the product code also with product name  so create new function as Product code(string productId)
                //this code is created by jitendra upadhyay on 24-03-2014
                txtProductcode.Text = ProductCode(dt.Rows[0]["ProductId"].ToString());
                pnlPDescription.Visible = true;
                txtPDesc.Visible = false;
                txtPDescription.Text = ProductDescription(dt.Rows[0]["ProductId"].ToString());
                FillUnit(dt.Rows[0]["ProductId"].ToString());
            }
            else
            {

                txtProductName.Text = dt.Rows[0]["SuggestedProductName"].ToString();
                txtPDesc.Visible = true;
                txtPDesc.Text = dt.Rows[0]["ProductDescription"].ToString();
                pnlPDescription.Visible = false;
                FillUnit("0");
            }

            ddlUnit.SelectedValue = dt.Rows[0]["UnitId"].ToString();
            txtRequestQty.Text = dt.Rows[0]["Quantity"].ToString();
            txtUnitPrice.Text = dt.Rows[0]["UnitPrice"].ToString();
            ViewState["SNO"] = dt.Rows[0]["Serial_No"].ToString();
            txtProductcode.Focus();
        }
    }
    protected void IbtnDelete_Command1(object sender, CommandEventArgs e)
    {


        DataTable dtproduct = (DataTable)Session["DtRequestProduct"];




        DataTable dt = new DataView(dtproduct, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Serial_No"] = i + 1;

        }
        //bind gridview by function in common class

        objPageCmn.FillData((object)gvProductRequest, dt, "", "");

        Session["DtRequestProduct"] = (DataTable)dt;
        //AllPageCode();


    }
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {

        ResetDetail();
        txtTermCondition.Focus();

    }
    protected void btnProductClose_Click(object sender, EventArgs e)
    {
        btnClosePanel_Click(null, null);
    }
    protected void txtlRequestNo_TextChanged(object sender, EventArgs e)
    {
        if (txtlRequestNo.Text != "")
        {
            DataTable dt = new DataView(ObjProductionReqestHeader.GetAllRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString()), "Request_No='" + txtlRequestNo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                {
                    DisplayMessage("Request No Already Exists");

                }
                else
                {

                    DisplayMessage("Request No Already Exists :- Go To Bin Tab");

                }
                txtlRequestNo.Text = "";
                txtlRequestNo.Focus();
            }
            else
            {

            }
        }
    }
    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = true;
        txtProductcode.Focus();
        ResetDetail();
        Session["DtSearchProduct"] = null;
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text != "")
        {
            DataTable dt = new DataTable();
            dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductName.Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["ModelNo"].ToString() == "0" || dt.Rows[0]["ModelNo"].ToString() == "")
                {
                    DisplayMessage("Product have no bom");
                    txtProductcode.Text = "";
                    txtProductcode.Focus();
                    txtProductName.Text = "";
                    return;
                }

                if (Session["DtRequestProduct"] != null)
                {
                    DataTable Dt = (DataTable)Session["DtRequestProduct"];
                    DataTable dtProduct = new DataTable();
                    try
                    {
                        dtProduct = new DataView(Dt, "ProductId=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtProduct.Rows.Count > 0)
                    {
                        DisplayMessage("Product Is already exists!");
                        txtProductName.Text = "";
                        txtProductcode.Text = "";
                        txtProductName.Focus();
                        return;

                    }
                }


                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                FillUnit(dt.Rows[0]["ProductId"].ToString());
                //string strUnitId = dt.Rows[0]["UnitId"].ToString();
                //if (strUnitId != "0" && strUnitId != "")
                //{
                //    ddlUnit.SelectedValue = strUnitId;
                //}
                //else
                //{
                //    FillUnit();
                //}
                txtPDescription.Text = dt.Rows[0]["Description"].ToString();

                txtPDesc.Visible = false;
                pnlPDescription.Visible = true;

            }
            else
            {
                DisplayMessage("Product Not exists");
                txtProductName.Text = "";
                txtProductcode.Text = "";
                txtProductName.Focus();
                return;
            }
            ddlUnit.Focus();
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductcode.Text != "")
        {
            DataTable dt = new DataTable();
            dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductcode.Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["ModelNo"].ToString() == "0" || dt.Rows[0]["ModelNo"].ToString() == "")
                {
                    DisplayMessage("Product have no bom");
                    txtProductcode.Text = "";
                    txtProductcode.Focus();
                    txtProductName.Text = "";
                    return;
                }

                if (Session["DtRequestProduct"] != null)
                {
                    DataTable Dt = (DataTable)Session["DtRequestProduct"];
                    DataTable dtProduct = new DataTable();
                    try
                    {
                        dtProduct = new DataView(Dt, "ProductId=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtProduct.Rows.Count > 0)
                    {
                        DisplayMessage("Product Is already exists!");
                        txtProductcode.Text = "";
                        txtProductcode.Focus();
                        txtProductName.Text = "";
                        return;

                    }
                }
                FillUnit(dt.Rows[0]["ProductId"].ToString());
                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                //string strUnitId = dt.Rows[0]["UnitId"].ToString();
                //if (strUnitId != "0" && strUnitId != "")
                //{
                //    ddlUnit.SelectedValue = strUnitId;
                //}
                //else
                //{
                //    FillUnit();
                //}
                txtPDescription.Text = dt.Rows[0]["Description"].ToString();

                txtPDesc.Visible = false;
                pnlPDescription.Visible = true;

            }
            else
            {

                DisplayMessage("Product Not exists");
                txtProductcode.Text = "";
                txtProductcode.Focus();
                txtProductName.Text = "";
                return;

            }
            ddlUnit.Focus();
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }



    }
    //Function to Check Whether there is Opening Stock for Particular Product or Not
    //created by Varsha Surana
    private bool CheckOpeningStockRow(string pid)
    {
        int st1;
        st1 = objStockDetail.GetProductOpeningStockStatus(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), pid);
        if (st1 > 0)
            return true;
        else
            return false;
    }
    //End Function
    protected void gvPurchaseRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Produ_Req"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Produ_Req"] = dt;
        //bind gridview by function in common class

        objPageCmn.FillData((object)gvPurchaseRequest, dt, "", "");

        //AllPageCode();
        gvPurchaseRequest.HeaderRow.Focus();

    }
    protected void gvPurchaseRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPurchaseRequest.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Produ_Req"];
        //bind gridview by function in common class
        objPageCmn.FillData((object)gvPurchaseRequest, dt, "", "");
        //AllPageCode();
    }
    #endregion
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        btnGeneraterequest.Visible = clsPagePermission.bAdd;
        btnProductSave.Visible = clsPagePermission.bAdd;
        btnPost.Visible = clsPagePermission.bAdd;

        try
        {
            GvSalesOrder.Columns[0].Visible = clsPagePermission.bEdit;
        }
        catch
        {
        }

        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        try
        {
            gvProductRequest.Columns[0].Visible = clsPagePermission.bEdit;
            gvProductRequest.Columns[1].Visible = clsPagePermission.bDelete;
        }
        catch
        {
        }

        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    #endregion

    #region User Defined Function

    public void ResetDetail()
    {
        txtProductName.Text = "";
        txtProductcode.Text = "";
        txtPDescription.Text = "";

        ddlUnit.Items.Clear();
        txtRequestQty.Text = "1";
        hidProduct.Value = "";
        txtPDesc.Text = "";
        txtProductcode.Focus();
        txtUnitPrice.Text = "1";
    }
    public void FillUnit(string ProductId)
    {

        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());

    }

    public string ProductName(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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

        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["ProductCode"].ToString();
        }
        else
        {
            ProductName = "0";


        }

        return ProductName;

    }

    public string ProductDescription(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
        DataTable dt = objUnit.GetUnitMasterById(Session["CompId"].ToString().ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
    }
    public void Fillgrid()
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

        DataTable dt = new DataView(ObjProductionReqestHeader.GetAllTrueRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString()), PostStatus, "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();

        //bind gridview by function in common class

        objPageCmn.FillData((object)gvPurchaseRequest, dt, "", "");

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count;
        Session["DtPurchaseRequest"] = dt;
        Session["dtFilter_Produ_Req"] = dt;
        //AllPageCode();

    }
    public void Reset()
    {
        txtlRequestNo.Text = "";
        txtlRequestNo.Text = GetDocumentNum();
        ViewState["DocNo"] = txtlRequestNo.Text;
        txtRequestdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtCustomer.Text = "";
        txtSONo.Text = "";
        txtSODate.Text = "";
        txtSalesPerson.Text = "";
        txtTermCondition.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        Session["DtSearchProduct"] = null;
        Session["DtRequestProduct"] = null;
        //GetDocumentNumber();
        gvProductRequest.DataSource = null;
        gvProductRequest.DataBind();
        txtbinValue.Text = "";
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueBinDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        txtRequestdate.Focus();
        txtValue.Focus();
        chkpost.Checked = false;
        chkCancel.Checked = false;
        hdnOrderId.Value = "0";
        txtlRequestNo.Enabled = true;
        ddlLocation.SelectedValue = Session["LocId"].ToString();
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
    protected string GetDocumentNum()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "167", "320", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }

    #endregion
    #region Bin Section
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinFieldName.SelectedItem.Value == "Request_Date" || ddlbinFieldName.SelectedItem.Value == "Order_Date")
        {
            if (txtValueBinDate.Text != "")
            {

                try
                {
                    ObjSysParam.getDateForInput(txtValueBinDate.Text);
                    txtbinValue.Text = ObjSysParam.getDateForInput(txtValueBinDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueBinDate.Text = "";
                    txtbinValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBinDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueBinDate.Focus();
                txtbinValue.Text = "";
                return;
            }
        }

        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtBinPurchaseRequest = (DataTable)Session["DtBinPurchaseRequest"];


            DataView view = new DataView(dtBinPurchaseRequest, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";


            //bind gridview by function in common class

            objPageCmn.FillData((object)gvBinPurchaseRequest, view.ToTable(), "", "");



            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
            //btnbinbind.Focus();
            //btnbinRefresh.Focus();

        }
        if (txtbinValue.Text != "")
            txtbinValue.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        ddlbinOption.SelectedIndex = 3;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtbinValue.Visible = true;
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void gvBinPurchaseRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBinPurchaseRequest.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {

            //bind gridview by function in common class

            objPageCmn.FillData((object)gvBinPurchaseRequest, (DataTable)Session["dtbinFilter"], "", "");

        }
        //AllPageCode();

        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvBinPurchaseRequest.Rows.Count; i++)
        {
            Label lblconid = (Label)gvBinPurchaseRequest.Rows[i].FindControl("lblReqId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvBinPurchaseRequest.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

        gvBinPurchaseRequest.BottomPagerRow.Focus();

    }
    protected void gvBinPurchaseRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtBinFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //bind gridview by function in common class

        objPageCmn.FillData((object)gvBinPurchaseRequest, (DataTable)Session["dtbinFilter"], "", "");

        //AllPageCode();
        gvBinPurchaseRequest.HeaderRow.Focus();

    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvBinPurchaseRequest.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvBinPurchaseRequest.Rows.Count; i++)
        {
            ((CheckBox)gvBinPurchaseRequest.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvBinPurchaseRequest.Rows[i].FindControl("lblReqId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvBinPurchaseRequest.Rows[i].FindControl("lblReqId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvBinPurchaseRequest.Rows[i].FindControl("lblReqId"))).Text.Trim().ToString())
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
        ((CheckBox)gvBinPurchaseRequest.HeaderRow.FindControl("chkgvSelectAll")).Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvBinPurchaseRequest.Rows[index].FindControl("lblReqId");
        if (((CheckBox)gvBinPurchaseRequest.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;

        }

        else
        {

            empidlist += lb.Text.ToString().Trim();
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
        ((CheckBox)gvBinPurchaseRequest.Rows[index].FindControl("chkgvSelect")).Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPr = (DataTable)Session["dtBinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPr.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < gvBinPurchaseRequest.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvBinPurchaseRequest.Rows[i].FindControl("lblReqId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvBinPurchaseRequest.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtPr1 = (DataTable)Session["dtBinFilter"];
            //bind gridview by function in common class

            objPageCmn.FillData((object)gvBinPurchaseRequest, (DataTable)Session["dtbinFilter"], "", "");

            //AllPageCode();
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
                    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(ObjProductionReqestHeader.GetRecord_By_TransId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), lblSelectedRecord.Text.Split(',')[j].ToString()).Rows[0]["Request_Date"].ToString()), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
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
                    b = ObjProductionReqestHeader.DeleteRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
                }
            }
        }

        if (b != 0)
        {
            Fillgrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activated");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvBinPurchaseRequest.Rows)
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
        txtbinValue.Focus();
    }


    public void FillGridBin()
    {

        DataTable dt = ObjProductionReqestHeader.GetAllFalseRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString());


        //bind gridview by function in common class

        objPageCmn.FillData((object)gvBinPurchaseRequest, dt, "", "");


        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count;
        Session["DtBinPurchaseRequest"] = dt;
        Session["DtBinFilter"] = dt;
        if (dt.Rows.Count != 0)
        {
            //AllPageCode();
        }
        else
        {
            imgBtnRestore.Visible = false;
        }
    }
    #endregion
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/PurchaseRequestPrint.aspx?RId=" + e.CommandArgument.ToString() + "','window','width=1024');", true);
    }

    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "1/1/1900 12:00:00 AM")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    public string GetDesc(string Desc)
    {
        string s = "";
        if (Desc.ToString().Length > 15)
        {
            s = Desc.ToString().Substring(0, 14);
        }
        else
        {
            s = Desc.ToString();
        }
        return s;
    }
    protected string GetEmployeeCode(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
            }
        }
        else
        {
            strEmployeeName = "";
        }
        return strEmployeeName;
    }
    public string GetDeptName(string DepId)
    {
        try
        {
            return ObjDept.GetDepartmentMasterById(DepId).Rows[0]["Dep_Name"].ToString();
        }
        catch
        {
            return "";

        }
    }
    #region View
    protected void btnCancelView_Click(object sender, EventArgs e)
    {
        ViewReset();
    }
    protected void btnCloseView_Click(object sender, EventArgs e)
    {
        ViewReset();
    }
    void ViewReset()
    {
        editid.Value = "";
        lblRequestNoView.Text = "";
        lblRequestdateView.Text = "";
        lblExpDelDateView.Text = "";
        txtDescriptionView.Text = "";
        GridView_ViewDetail.DataSource = null;
        GridView_ViewDetail.DataBind();
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }


    #endregion
    #region Date Search
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Request_Date" || ddlFieldName.SelectedItem.Value == "Order_Date")
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

        if (ddlbinFieldName.SelectedItem.Value == "Request_Date" || ddlbinFieldName.SelectedItem.Value == "Order_Date")
        {
            txtValueBinDate.Visible = true;
            txtbinValue.Visible = false;
            txtbinValue.Text = "";
            txtValueBinDate.Text = "";

        }
        else
        {
            txtValueBinDate.Visible = false;
            txtbinValue.Visible = true;
            txtbinValue.Text = "";
            txtValueBinDate.Text = "";

        }
        ddlbinFieldName.Focus();
    }

    #endregion

    #region AutocompleteRegion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {

        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        try
        {
            dt = new DataView(dt, "ModelNo<>'0' and ModelNo<>' '", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        try
        {
            dt = new DataView(dt, "ModelNo<>'0' and ModelNo<>' '", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }


        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Customer_Id"].ToString();
            }
        }
        return filterlist;

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "";
            }
        }

        return txt;
    }

    #endregion
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;
        if (txtCustomer.Text != "")
        {
            DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomer.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Customer Name");
                txtCustomer.Text = "";
                txtCustomer.Focus();

            }
            else
            {
                string strCustomerId = dt.Rows[0]["Trans_Id"].ToString();

            }

        }
        else
        {
            DisplayMessage("Select Customer Name");
            txtCustomer.Focus();

        }

    }
    protected void txtSalesPerson_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtSalesPerson.Text != "")
        {
            strEmployeeId = HR_EmployeeDetail.GetEmployeeId(txtSalesPerson.Text.Split('/')[1].ToString());
            if (strEmployeeId != "" && strEmployeeId != "0")
            {

            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtSalesPerson.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesPerson);
            }
        }
    }

    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(Session["CompId"].ToString(), strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), retval);
                if (dtEmp.Rows.Count == 0)
                { retval = ""; }
            }
            else
            { retval = ""; }
        }
        else
        { retval = ""; }
        return retval;
    }
    #region Post

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Fillgrid();
        //AllPageCode();

    }
    #endregion
    public string SetDecimal(string amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
    }
    #region SalesOrder

    private void FillSalesOrder()
    {
        DataTable dt = objSOrderHeader.GetSalesOrderForproduction(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        Session["dtSorder"] = dt;
        if (dt != null && dt.Rows.Count > 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesOrder, dt, "", "");
        }
        else
        {
            GvSalesOrder.DataSource = null;
            GvSalesOrder.DataBind();
        }
        lblOrderTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
    }
    protected void ddlOrderFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        if (ddlOrderFieldName.SelectedItem.Value == "SalesOrderDate")
        {
            txtOrderValueDate.Visible = true;
            txtOrderValue.Visible = false;
            txtOrderValue.Text = "";
            txtOrderValueDate.Text = "";

        }
        else
        {
            txtOrderValueDate.Visible = false;
            txtOrderValue.Visible = true;
            txtOrderValue.Text = "";
            txtOrderValueDate.Text = "";

        }
    }

    protected void btnbindOrderrpt_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlOrderFieldName.SelectedItem.Value == "SalesOrderDate")
        {
            if (txtOrderValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtOrderValueDate.Text);
                    txtOrderValue.Text = Convert.ToDateTime(txtOrderValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtOrderValueDate.Text = "";
                    txtOrderValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtOrderValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtOrderValueDate.Focus();
                return;
            }
        }
        if (ddlOrderOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOrderOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlOrderFieldName.SelectedValue + ",System.String)='" + txtOrderValue.Text.Trim() + "'";
            }
            else if (ddlOrderOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlOrderFieldName.SelectedValue + ",System.String) like '%" + txtOrderValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlOrderFieldName.SelectedValue + ",System.String) Like '" + txtOrderValue.Text.Trim() + "%'";

            }
            DataTable dtAdd = (DataTable)Session["dtSorder"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvPendingSalesOrder, view.ToTable(), "", "");
            Session["dtSorder"] = view.ToTable();
            lblOrderTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
    }


    protected void btnOrderRefreshReport_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        GetPendingOrder();
        ddlOrderFieldName.SelectedIndex = 1;
        ddlOrderOption.SelectedIndex = 2;
        txtOrderValue.Text = "";
        txtOrderValueDate.Text = "";
        txtOrderValueDate.Visible = false;
        txtOrderValue.Visible = true;
        //AllPageCode();
    }


    protected void GvSalesOrder_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtSorder"];
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
        Session["dtSorder"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesOrder, dt, "", "");

        //AllPageCode();
    }
    protected void GvSalesOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesOrder.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtSorder"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesOrder, dt, "", "");

        //AllPageCode();
    }


    protected void btnSIEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dtsalesorder = objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), e.CommandArgument.ToString());

        if (dtsalesorder.Rows.Count > 0)
        {
            txtCustomer.Text = dtsalesorder.Rows[0]["CustomerName"].ToString() + "/" + dtsalesorder.Rows[0]["CustomerId"].ToString();
            txtSONo.Text = dtsalesorder.Rows[0]["SalesOrderNo"].ToString();
            txtSODate.Text = Convert.ToDateTime(dtsalesorder.Rows[0]["SalesOrderDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

            hdnOrderId.Value = e.CommandArgument.ToString();
            DataTable dtDetail = ObjSorderDetail.GetSODetailBySOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), e.CommandArgument.ToString());
            try
            {
                dtDetail = new DataView(dtDetail, "Field2='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
            dtDetail = dtDetail.DefaultView.ToTable(true, "Trans_Id", "Serial_No", "Product_Id", "UnitId", "Quantity", "UnitPrice");

            dtDetail.Columns["Product_Id"].ColumnName = "ProductId";
            Session["DtRequestProduct"] = dtDetail;
            //bind gridview by function in common class
            objPageCmn.FillData((object)gvProductRequest, dtDetail, "", "");
            ddlLocation.SelectedValue = e.CommandName.ToString();
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSONo);
        hdnOrderId.Value = e.CommandArgument.ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Request_Active()", true);
    }

    #endregion
    #region Report

    protected void btnShowReport_Click(object sender, EventArgs e)
    {

        Session["DtProduction"] = null;
        DataTable DtFilter = new DataTable();
        ProductionDataset ObjProductionDataset = new ProductionDataset();
        ObjProductionDataset.EnforceConstraints = false;

        ProductionDatasetTableAdapters.sp_Inv_ProductionRequestHeader_SelectRow_ReportTableAdapter objAdp = new ProductionDatasetTableAdapters.sp_Inv_ProductionRequestHeader_SelectRow_ReportTableAdapter();
        objAdp.Connection.ConnectionString = Session["DBConnection"].ToString();
        objAdp.Fill(ObjProductionDataset.sp_Inv_ProductionRequestHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
        DtFilter = ObjProductionDataset.sp_Inv_ProductionRequestHeader_SelectRow_Report;

        if (DtFilter.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        DtFilter = new DataView(DtFilter, "", "Request_Date asc", DataViewRowState.CurrentRows).ToTable();



        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

            DtFilter = new DataView(DtFilter, "Request_Date>='" + txtFromDate.Text + "' and Request_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }



        if (ddlStatus.SelectedIndex == 1)
        {
            DtFilter = new DataView(DtFilter, "Is_Production_Process='False' and Is_Production_Finish='False'", "", DataViewRowState.CurrentRows).ToTable();

        }
        else if (ddlStatus.SelectedIndex == 2)
        {
            DtFilter = new DataView(DtFilter, "Is_Production_Process='True' and Is_Production_Finish='False'", "", DataViewRowState.CurrentRows).ToTable();


        }
        else if (ddlStatus.SelectedIndex == 3)
        {
            DtFilter = new DataView(DtFilter, "Is_Production_Finish='True'", "", DataViewRowState.CurrentRows).ToTable();


        }

        if (DtFilter.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }


        Session["DtProduction"] = DtFilter;

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Production_Report/ProductionReport.aspx?Type=" + ddlStatus.SelectedValue.Trim() + "','window','width=1024');", true);


    }
    #endregion

    #region GetProductionLocation


    private void FillddlLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");

        //Common Function add By jitendra on 06-02-2016
        objPageCmn.FillData((object)ddlLocation, dtLoc, "Location_Name", "Location_Id");

    }
    #endregion

    protected void ddlorderlocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        Fillcategory(); 
        GetPendingOrder();
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetPendingOrder();
    }

    public void Fillcategory()
    {
        DataTable dt = ObjSalesOrderDetail.getSODetailDataForProduction(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlorderlocation.SelectedValue, Session["FinanceYearId"].ToString());
        dt = new DataView(dt, "", "Category_Name", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "Category_Name", "Category_Id");
      
        ddlCategory.DataSource = dt;
        ddlCategory.DataTextField = "Category_Name";
        ddlCategory.DataValueField = "Category_Id";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, "--Category--");
    }

    protected void btnGeneraterequest_Click(object sender, EventArgs e)
    {
        int Counter = 0;


        string strOrderList = string.Empty;

        DataTable dtorderdetail = new DataTable();

        dtorderdetail.Columns.Add("Customer_id");
        dtorderdetail.Columns.Add("order_no");
        dtorderdetail.Columns.Add("order_date", typeof(DateTime));
        dtorderdetail.Columns.Add("Order_Id");
        dtorderdetail.Columns.Add("productid");
        dtorderdetail.Columns.Add("unitid");
        dtorderdetail.Columns.Add("quantity");
        dtorderdetail.Columns.Add("unitprice");
        dtorderdetail.Columns.Add("location_id");


        foreach (GridViewRow gvrow in gvPendingSalesOrder.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSO")).Checked)
            {
                DataRow dr = dtorderdetail.NewRow();
                dr["Customer_id"] = ((Label)gvrow.FindControl("lblCustomerId")).Text;
                dr["order_no"] = ((Label)gvrow.FindControl("lblOrderNo")).Text;
                dr["order_date"] = ((Label)gvrow.FindControl("lblOrderdate")).Text;
                dr["Order_Id"] = ((HiddenField)gvrow.FindControl("gvOrderId")).Value;
                dr["productid"] = ((HiddenField)gvrow.FindControl("gvhdnProductId")).Value;
                dr["unitid"] = ((HiddenField)gvrow.FindControl("gvhdnUnitId")).Value;
                dr["quantity"] = ((Label)gvrow.FindControl("lblQuantity")).Text;
                dr["unitprice"] = ((HiddenField)gvrow.FindControl("gvHdnUnitCost")).Value;
                dr["location_id"] = ((Label)gvrow.FindControl("lbllocId")).Text;
                dtorderdetail.Rows.Add(dr);
            }
        }


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        DataTable dtTemp = dtorderdetail.Copy();

        DataTable dtItemdetail = new DataTable();

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (strOrderList.Split(',').Contains(dr["Order_Id"].ToString()))
                {
                    continue;
                }

                Counter++;
                strOrderList += dr["Order_Id"].ToString().Trim() + ",";

                int b = 0;
                string sql = string.Empty;


                b = ObjProductionReqestHeader.InsertRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), ViewState["DocNo"].ToString(), ObjSysParam.getDateForInput(DateTime.Now.ToString("dd-MMM-yyyy")).ToString(), dr["Customer_id"].ToString(), dr["order_no"].ToString(), dr["order_date"].ToString(), "0", "Auto Generate Request", true.ToString(), false.ToString(), false.ToString(), false.ToString(), false.ToString(), dr["Order_Id"].ToString(), dr["location_id"].ToString(), "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                //b = 1;


                DataTable dtCount = ObjProductionReqestHeader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                if (dtCount.Rows.Count == 0)
                {
                    ObjProductionReqestHeader.Updatecode(b.ToString(), ViewState["DocNo"].ToString() + "1", ref trns);

                }
                else
                {
                    ObjProductionReqestHeader.Updatecode(b.ToString(), ViewState["DocNo"].ToString() + dtCount.Rows.Count, ref trns);

                }

                dtItemdetail = new DataView(dtorderdetail, "Order_Id='" + dr["Order_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                float srNo = 0;
                foreach (DataRow drdetail in dtItemdetail.Rows)
                {
                    srNo++;
                    ObjProductionRequestDetail.InsertRecord(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), b.ToString(), srNo.ToString(), drdetail["productid"].ToString(), drdetail["unitid"].ToString(), drdetail["quantity"].ToString(), drdetail["unitprice"].ToString(), drdetail["unitprice"].ToString(), "0", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                }

            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Fillgrid();
            Reset();
            txtRequestdate.Focus();
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

        if (Counter > 0)
        {
            DisplayMessage("Record Saved successfully");
        }
        else
        {
            DisplayMessage("Record not found");
        }
        GetPendingOrder();

    }

    protected void chkHeaderSO_CheckedChanged(object sender, EventArgs e)
    {
        bool Result = ((CheckBox)gvPendingSalesOrder.HeaderRow.FindControl("chkHeaderSO")).Checked;

        foreach (GridViewRow gvrow in gvPendingSalesOrder.Rows)
        {
            ((CheckBox)gvrow.FindControl("chkSO")).Checked = Result;
        }
    }
}