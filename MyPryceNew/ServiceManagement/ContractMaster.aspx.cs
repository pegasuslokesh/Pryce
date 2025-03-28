using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using DevExpress.Web;
using PegasusDataAccess;
using System.Collections.Generic;

public partial class ServiceManagement_ContractMaster : System.Web.UI.Page
{
    SM_Contract_Master objContractMaster = null;
    Inv_ProductMaster objProductM = null;
    Set_CustomerMaster ObjCustomer = null;
    SystemParameter objSysParam = null;
    Set_DocNumber ObjDoc = null;
    Ems_ContactMaster objContact = null;
    CurrencyMaster objCurrency = null;
    Inv_SalesInvoiceHeader objSalesInvoiceheader = null;
    Inv_SalesInvoiceDetail objSalesInvoiceDetail = null;
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    Common objCmn = null;
    SM_Contract_Detail obj_contractDetail = null;
    Common cmn = null;
    DataAccessClass ObjDa = null;
    PageControlCommon objPageCmn = null;
    static string CustomerID;
    PageControlsSetting objPageCtlSettting = null;
    public const int grdDefaultColCount = 9;
    private const string strPageName = "ContractMaster";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objContractMaster = new SM_Contract_Master(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objSalesInvoiceheader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objSalesInvoiceDetail = new Inv_SalesInvoiceDetail(Session["DBConnection"].ToString());
        objCmn = new Common(Session["DBConnection"].ToString());
        obj_contractDetail = new SM_Contract_Detail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        Page.Title = objSysParam.GetSysTitle();
        Calender.Format = Session["DateFormat"].ToString();

        txtStartdate.Attributes.Add("readonly", "readonly");
        txtContractDate.Attributes.Add("readonly", "readonly");
        txtEnddate.Attributes.Add("readonly", "readonly");

        //txtScheduledDate.Attributes.Add("readonly", "readonly");


        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ServiceManagement/ContractMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);
            objPageCmn.fillLocation(ddlLoc);

            FillCurrency();
            Reset();
            //btnList_Click(null, null);
            Calender.Format = Session["DateFormat"].ToString();
            calenderStartdate.Format = Session["DateFormat"].ToString();
            calenderenddate.Format = Session["DateFormat"].ToString();
            txtContractDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtStartdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtEnddate.Text = DateTime.Now.AddYears(1).AddDays(-1).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtEnddate_TextChanged(null, null);
            //CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            FillGrid();
            if (Request.QueryString["SearchField"] != null)
            {
                ddlType.SelectedValue = "All";

            }
            getPageControlsVisibility();

        }

        btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
        GvContractMaster.DataSource = Session["dtContractData"] as DataTable;
        GvContractMaster.DataBind();

        //this.RegisterPostBackControl();
        //AllPageCode();
    }

    //private void RegisterPostBackControl()
    //{
    //    foreach (GridViewRow row in GvContractMaster.Rows)
    //    {
    //        ImageButton lnkFull = row.FindControl("ImgDownload") as ImageButton;
    //        ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkFull);
    //    }
    //}

    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlCurrency.Items.Add("--Select--");
            ddlCurrency.SelectedValue = "--Select--";
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        BtnExportExcel.Visible = false;
        BtnExportPDF.Visible = false;
        btnSave.Visible = clsPagePermission.bAdd;
        GvContractMaster.Columns[1].Visible = clsPagePermission.bEdit;
        btnsaveScheduleDtls.Visible = clsPagePermission.bReorderAllow;
        GvContractMaster.Columns[2].Visible = clsPagePermission.bDelete;
        GvContractMaster.Columns[0].Visible = clsPagePermission.bView;
        BtnExportExcel.Visible = clsPagePermission.bDownload;
        BtnExportPDF.Visible = clsPagePermission.bDownload;
        GvContractMaster.Columns[3].Visible = clsPagePermission.bUpload;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillBinGrid();
        ddlFieldNameBin.Focus();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string filepath = string.Empty;
        string FileName = string.Empty;
        string TransId = string.Empty;

        if (txtContractNo.Text == "")
        {
            btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            DisplayMessage("Enter Contract No");
            txtContractNo.Focus();
            return;
        }
        else
        {

            DataTable dt = objContractMaster.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue);


            dt = new DataView(dt, "Contract_No='" + txtContractNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                if (hdnValue.Value != "")
                {
                    if (hdnValue.Value != dt.Rows[0]["Trans_Id"].ToString())
                    {
                        btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                        DisplayMessage("Contract No. already exists");
                        txtContractNo.Focus();
                        txtContractNo.Text = "";
                        return;
                    }
                }
                else
                {
                    btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                    DisplayMessage("Contract No. already exists");
                    txtContractNo.Focus();
                    txtContractNo.Text = "";
                    return;
                }
            }
        }


        if (txtContractDate.Text == "")
        {
            btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            DisplayMessage("Enter Contract Date");
            txtContractDate.Focus();
            return;
        }

        if (txtECustomer.Text == "")
        {
            btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            txtECustomer.Focus();
            DisplayMessage("Enter Customer Name");
            return;
        }

        if (txtStartdate.Text == "")
        {
            btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            DisplayMessage("Enter Start Date");
            txtStartdate.Focus();
            return;
        }
        if (txtContractAmt.Text.Trim() == "")
        {
            btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            DisplayMessage("Enter Contract Amount");
            txtContractAmt.Focus();
            return;
        }

        if (txtEnddate.Text == "")
        {
            btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            DisplayMessage("Enter End Date");
            txtEnddate.Focus();
            return;
        }

        if (Convert.ToDateTime(txtEnddate.Text) < Convert.ToDateTime(txtStartdate.Text))
        {
            btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            DisplayMessage("Start Date should be less than or equal to end date");
            txtStartdate.Focus();
            return;
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }


        if (txtInvoiceNo.Text == "")
        {
            hdnInvoiceId.Value = "0";
        }

        if (txtgrossAmount.Text == "")
        {
            txtgrossAmount.Text = "0";
        }
        if (txtDiscountAmount.Text == "")
        {
            txtDiscountAmount.Text = "0";
        }
        if (txtNetAmount.Text == "")
        {
            txtNetAmount.Text = "0";
        }

        //if (UploadFile.HasFile)
        //{
        //    FileName = UploadFile.FileName;
        //}


        string CurrencyId = string.Empty;
        CurrencyId = "0";
        if (ddlCurrency.SelectedIndex != 0)
        {
            CurrencyId = ddlCurrency.SelectedValue;
        }

        int b = 0;

        //for new renewal contract 
        if (hdnIsRenewal.Value == "true")
        {
            if (objSysParam.getDateForInput(txtStartdate.Text) < objSysParam.getDateForInput(hdnImmidiateContractEndDate.Value))
            {
                DisplayMessage("Contract Start Date Must be Greater than " + hdnImmidiateContractEndDate.Value);
                txtStartdate.Text = "";
                txtStartdate.Focus();
                return;
            }

            b = objContractMaster.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtContractNo.Text, txtContractDate.Text, txtStartdate.Text, txtEnddate.Text, Session["ContactID"].ToString(), CurrencyId, txtOldContractNo.Text, hdnInvoiceId.Value, txtgrossAmount.Text, txtDiscountAmount.Text, txtNetAmount.Text, false.ToString(), txtTermsandconditon.Content, FileName, txtDesc.Text, txtContractName.Text, hdnRenewalImmidiateTransId.Value, hdnRenewalParentTransId.Value, false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtCost.Text, txtContractAmt.Text, hdnSalesOrderID.Value);
            if (txtContractNo.Text == ViewState["DocNo"].ToString())
            {
                string sql = "select count(*) from SM_Contract_Master where company_id='" + Session["CompId"].ToString() + "'";
                int recCount = int.Parse(ObjDa.get_SingleValue(sql));
                bool bFlag = false;
                while (bFlag == false)
                {
                    txtContractNo.Text = ViewState["DocNo"].ToString() + (recCount == 0 ? "1" : recCount.ToString());
                    sql = "SELECT count(*) FROM SM_Contract_Master WHERE Company_Id = '" + Session["CompId"].ToString() + "' and contract_no='" + txtContractNo.Text + "'";
                    string strInvCount = ObjDa.get_SingleValue(sql);
                    if (strInvCount == "0")
                    {
                        bFlag = true;
                    }
                    else
                    {
                        recCount++;
                    }
                }
                objContractMaster.Updatecode(b.ToString(), txtContractNo.Text);
                //DataTable dtCount = objContractMaster.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue);
                //if (dtCount.Rows.Count == 0)
                //{
                //    objContractMaster.Updatecode(b.ToString(), txtContractNo.Text + "1");
                //    txtContractNo.Text = txtContractNo.Text + "1";
                //}
                //else
                //{
                //    objContractMaster.Updatecode(b.ToString(), txtContractNo.Text + dtCount.Rows.Count);
                //    txtContractNo.Text = txtContractNo.Text + dtCount.Rows.Count;
                //}
            }
            InsertDetailData(false, b.ToString(), txtContractNo.Text, txtECustomer.Text.Split('/')[0].ToString());

            // set reminder when contract ends.
            string message = "Your Contract ('" + txtContractNo.Text + "') Generated For ('" + txtECustomer.Text.Split('/')[0].ToString() + "') has been Expired";
            setReminder(txtEnddate.Text, txtContractNo.Text, b.ToString(), message);

            btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            DisplayMessage("Record Saved", "green");
            Reset();
            //FillGrid();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            return;
        }

        // to create new contract
        if (hdnValue.Value == "")
        {


            b = objContractMaster.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtContractNo.Text, objSysParam.getDateForInput(txtContractDate.Text).ToString(), objSysParam.getDateForInput(txtStartdate.Text).ToString(), objSysParam.getDateForInput(txtEnddate.Text).ToString(), Session["ContactID"].ToString(), CurrencyId, txtOldContractNo.Text, hdnInvoiceId.Value, txtgrossAmount.Text, txtDiscountAmount.Text, txtNetAmount.Text, false.ToString(), txtTermsandconditon.Content, FileName, txtDesc.Text, txtContractName.Text, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtCost.Text, txtContractAmt.Text, hdnSalesOrderID.Value);

            string strMaxId = string.Empty;

            if (b != 0)
            {
                strMaxId = b.ToString();

                string dtCount = objContractMaster.getCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue);
                objContractMaster.Updatecode(b.ToString(), txtContractNo.Text + dtCount);
                txtContractNo.Text = txtContractNo.Text + dtCount;
                
                //add details
                InsertDetailData(false, b.ToString(), txtContractNo.Text, txtECustomer.Text.Split('/')[0].ToString());

                // set reminder when contract ends.
                string message = "Your Contract ('" + txtContractNo.Text + "') Generated For ('" + txtECustomer.Text.Split('/')[0].ToString() + "') has been Expired";
                setReminder(txtEnddate.Text, txtContractNo.Text, b.ToString(), message);



                btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                DisplayMessage("Record Saved", "green");
                Reset();
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
        }
        else
        {


            objContractMaster.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, txtContractNo.Text, txtContractDate.Text, txtStartdate.Text, txtEnddate.Text, Session["ContactID"].ToString(), CurrencyId, txtOldContractNo.Text, hdnInvoiceId.Value, txtgrossAmount.Text, txtDiscountAmount.Text, txtNetAmount.Text, false.ToString(), txtTermsandconditon.Content, FileName, txtDesc.Text, txtContractName.Text, hdnRenewalImmidiateTransId.Value, hdnRenewalParentTransId.Value, false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtCost.Text, txtContractAmt.Text, hdnSalesOrderID.Value);
            InsertDetailData(true, hdnValue.Value, txtContractNo.Text, txtECustomer.Text.Split('/')[0].ToString());


            btnSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            //btnList_Click(null, null);
            DisplayMessage("Record Updated", "green");
            Reset();
            FillGrid();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
    }

    public void InsertDetailData(bool isEdited, string contractID, string contractNo, string customerName)
    {
        SM_Contract_Detail objContract = new SM_Contract_Detail(Session["DBConnection"].ToString());

        if (isEdited == true)
        {
            objContract.DeleteDataByContractID(contractID);
            deleteReminder(contractID);
        }

        Label gvinvoiceDt, gvInvoiceAmt, gvDueAmt;
        TextBox gvScheduledDate;
        HiddenField gvHdnInvoiceID;
        for (int i = 0; i < GvScheduledData.Rows.Count; i++)
        {
            gvHdnInvoiceID = GvScheduledData.Rows[i].FindControl("gvHdnInvoiceID") as HiddenField;
            gvScheduledDate = GvScheduledData.Rows[i].FindControl("gvScheduledDate") as TextBox;
            if (gvHdnInvoiceID.Value.Trim() == "")
            {
                gvHdnInvoiceID.Value = "0";
            }
            objContract.InsertRecord(contractID, gvScheduledDate.Text, gvHdnInvoiceID.Value, Session["UserId"].ToString(), Session["UserId"].ToString());
            string message = "Your have to create Invoice for Contract No ('" + contractNo + "') Generated For ('" + customerName + "')";
            setReminder(gvScheduledDate.Text, contractNo, contractID, message);
        }
    }
    public void setReminder(string dueDate, string contract_no, string contract_Id, string message)
    {

        int reminder_id = new Reminder(Session["DBConnection"].ToString()).insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SM_Contract_Master", contract_Id, message, "", System.DateTime.Now.ToString(), "1", dueDate, "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
        new ReminderLogs(Session["DBConnection"].ToString()).insertLogData(reminder_id.ToString(), dueDate, "", Session["UserId"].ToString(), Session["UserId"].ToString());
        new Reminder(Session["DBConnection"].ToString()).Set_Url(reminder_id.ToString(), "../ServiceManagement/ContractMaster.aspx?SearchField=" + contract_no + "");
    }

    public void deleteReminder(string ContractID)
    {
        new Reminder(Session["DBConnection"].ToString()).setIsActiveFalseByRef_table_name_n_pk("SM_Contract_Master ", ContractID);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        //btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }


    protected void GvContractMaster_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtContractData"];
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
        Session["dtContractData"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvContractMaster, dt, "", "");

        //AllPageCode();
    }
    protected void GvContractMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvContractMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtContractData"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvContractMaster, dt, "", "");

        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        ddlLoc.Enabled = false;
        ddlLoc.SelectedValue = e.CommandName.ToString();
        DataTable dt = objContractMaster.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {

            LinkButton b = (LinkButton)sender;
            string objSenderID = b.ID;

            if (objSenderID == "lnkViewDetail")
            {

                div_edit.Style.Add("display", "block");
                div_insert.Style.Add("display", "none");

                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                txtContractNo.Text = dt.Rows[0]["Contract_No"].ToString();
                txtStartdate.Text = GetDate(dt.Rows[0]["Start_Date"].ToString());
                txtEnddate.Text = GetDate(dt.Rows[0]["End_Date"].ToString());
                txtContractDate.Text = Convert.ToDateTime(dt.Rows[0]["Contract_Date"].ToString()).ToString(objSysParam.SetDateFormat());
                txtOldContractNo.Text = dt.Rows[0]["Old_Contract_No"].ToString();
                txtContractDate.Text = Convert.ToDateTime(dt.Rows[0]["Contract_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                GvScheduledData.DataSource = null;
                GvScheduledData.DataBind();
                btnSave.Enabled = false;
                try
                {
                    DataTable dt_contractDetail = obj_contractDetail.GetAllDetailDataByContractID(e.CommandArgument.ToString());
                    GvScheduledData.DataSource = dt_contractDetail;
                    GvScheduledData.DataBind();
                }
                catch
                {

                }
            }
            else
            {
                btnSave.Enabled = true;
                int R_days = Convert.ToInt32(dt.Rows[0]["Remaining_Days"].ToString());
                // for renewal (will create new contract)
                if (R_days < 0)
                {
                    Lbl_Tab_New.Text = "Renewal";
                    div_insert.Style.Add("display", "block");
                    div_edit.Style.Add("display", "none");
                    ddlOperation.Items.Clear();
                    //check 
                    DataTable dtCheckRenewal = objContractMaster.CheckRenewalByTransId(e.CommandArgument.ToString());

                    if (dtCheckRenewal.Rows.Count == 1)
                    {
                        DisplayMessage("Renewed Contract Already Exists");
                        return;
                    }

                    txtContractDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

                    hdnIsRenewal.Value = "true";
                    hdnRenewalImmidiateTransId.Value = e.CommandArgument.ToString();

                    DateTime dt1 = Convert.ToDateTime(dt.Rows[0]["Start_Date"].ToString());
                    txtStartdate.Text = GetDate((dt1.AddYears(1)).ToString());

                    DateTime dt2 = Convert.ToDateTime(dt.Rows[0]["End_Date"].ToString());
                    txtEnddate.Text = GetDate((dt2.AddYears(1)).ToString());
                    hdnImmidiateContractEndDate.Value = GetDate(dt.Rows[0]["End_Date"].ToString());
                    txtEnddate_TextChanged(null, null);

                    if (dt.Rows[0]["Field5"].ToString() == "")
                    {
                        hdnRenewalParentTransId.Value = hdnRenewalImmidiateTransId.Value;
                    }
                    else
                    {
                        hdnRenewalParentTransId.Value = dt.Rows[0]["Field5"].ToString();
                    }

                    if (dt.Rows[0]["Field3"].ToString() == "")
                    {
                        txtContractName.Enabled = true;
                    }
                    else
                    {
                        txtContractName.Enabled = false;
                    }


                    txtECustomer.Enabled = false;
                    txtOldContractNo.Text = dt.Rows[0]["Contract_No"].ToString();
                    txtOldContractNo.Enabled = false;
                    GvScheduledData.DataSource = null;
                    GvScheduledData.DataBind();
                }
                // for currently running (will edit the selected contract)
                if (R_days >= 0)
                {
                    Lbl_Tab_New.Text = Resources.Attendance.Edit;
                    div_edit.Style.Add("display", "block");
                    div_insert.Style.Add("display", "none");
                    //edit general
                    if (dt.Rows[0]["Field4"].ToString() == "")
                    {
                        Lbl_Tab_New.Text = Resources.Attendance.Edit;
                        hdnIsRenewal.Value = "false";
                        txtContractName.Enabled = true;
                        txtECustomer.Enabled = true;
                        txtOldContractNo.Enabled = true;
                        txtOldContractNo.Enabled = true;
                        txtContractNo.Text = dt.Rows[0]["Contract_No"].ToString();
                        txtStartdate.Text = GetDate(dt.Rows[0]["Start_Date"].ToString());
                        txtEnddate.Text = GetDate(dt.Rows[0]["End_Date"].ToString());
                        txtContractDate.Text = Convert.ToDateTime(dt.Rows[0]["Contract_Date"].ToString()).ToString(objSysParam.SetDateFormat());
                        txtOldContractNo.Text = dt.Rows[0]["Old_Contract_No"].ToString();
                    }
                    //edit renewal
                    else
                    {
                        hdnRenewalParentTransId.Value = dt.Rows[0]["Field5"].ToString();
                        hdnRenewalImmidiateTransId.Value = dt.Rows[0]["Field4"].ToString();
                        if (dt.Rows[0]["Field3"].ToString() == "")
                        {
                            txtContractName.Enabled = true;
                        }
                        else
                        {
                            txtContractName.Enabled = false;
                        }
                        txtStartdate.Text = GetDate(dt.Rows[0]["Start_Date"].ToString());
                        txtEnddate.Text = GetDate(dt.Rows[0]["End_Date"].ToString());
                        txtContractNo.Text = dt.Rows[0]["Contract_No"].ToString();
                        hdnImmidiateContractEndDate.Value = GetDate(dt.Rows[0]["End_Date"].ToString());
                        txtContractDate.Text = Convert.ToDateTime(dt.Rows[0]["Contract_Date"].ToString()).ToString(objSysParam.SetDateFormat());
                        txtECustomer.Enabled = false;
                        txtOldContractNo.Enabled = false;
                        txtOldContractNo.Text = dt.Rows[0]["Old_Contract_No"].ToString();
                    }

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                    GvScheduledData.DataSource = null;
                    GvScheduledData.DataBind();
                    try
                    {
                        DataTable dt_contractDetail = obj_contractDetail.GetAllDetailDataByContractID(e.CommandArgument.ToString());
                        GvScheduledData.DataSource = dt_contractDetail;
                        GvScheduledData.DataBind();
                    }
                    catch
                    {

                    }
                }
            }

            //btnNew_Click(null, null);
            hdnValue.Value = e.CommandArgument.ToString();

            hdnInvoiceId.Value = dt.Rows[0]["Invoice_No"].ToString();
            Session["ContactID"] = dt.Rows[0]["Customer_Id"].ToString();
            txtECustomer.Text = dt.Rows[0]["CustomerName"].ToString() + "/" + dt.Rows[0]["Customer_Id"].ToString();
            txtContractName.Text = dt.Rows[0]["Field3"].ToString();
            txtDesc.Text = dt.Rows[0]["Field2"].ToString();
            txtTermsandconditon.Content = dt.Rows[0]["Terms_and_Condition"].ToString();
            txtCost.Text = SetDecimal(dt.Rows[0]["Cost"].ToString());
            txtContractAmt.Text = SetDecimal(dt.Rows[0]["Contract_Amount"].ToString());

            if (dt.Rows[0]["Sales_Order_Id"].ToString().Trim() != "" && dt.Rows[0]["Sales_Order_Id"].ToString().Trim() != "0")
            {
                DataTable dt_salesorderdata = new Inv_SalesOrderHeader(Session["DBConnection"].ToString()).getAllDataByCustomerID(dt.Rows[0]["Customer_Id"].ToString());
                dt_salesorderdata = new DataView(dt_salesorderdata, "Trans_Id='" + dt.Rows[0]["Sales_Order_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                txtSalesOrderID.Text = dt_salesorderdata.Rows[0]["SalesOrderNo"].ToString();
                txtSalesOrderID_TextChanged(null, null);
            }
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {

        btnEdit_Command(sender, e);

    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        hdnValue.Value = e.CommandArgument.ToString();
        ddlLoc.SelectedValue = e.CommandName.ToString();
        b = objContractMaster.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        //FillBinGrid(); //Update grid view in bin tab
        FillGrid();
        Reset();
        //AllPageCode();

    }
    protected void IbtnDelete_Command(string trans_Id)
    {


        int b = 0;
        hdnValue.Value = trans_Id;
        b = objContractMaster.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnValue.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillBinGrid(); //Update grid view in bin tab
        FillGrid();
        Reset();
        //AllPageCode();

    }
    protected void txtECustomer_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtECustomer.Text != "")
        {
            try
            {
                strCustomerId = txtECustomer.Text.Split('/')[1].ToString();
            }
            catch
            {
                strCustomerId = "0";

            }
            if (strCustomerId == "0")
            {
                DisplayMessage("Customer Not Exist");
                txtECustomer.Text = "";
                txtECustomer.Focus();
                return;
            }
            else
            {

                Session["ContactID"] = strCustomerId;
            }


        }
        else
        {
            Session["ContactID"] = "0";
        }
        // AllPageCode();
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtCon = objcustomer.GetCustomerRecAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);


//        string filtertext = "Name like '%" + prefixText + "%'";
 //       DataTable dtCon = new DataView(dtCustomer, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["trans_Id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DataTable();

        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = HttpContext.Current.Session["ContactID"].ToString();


        if (id == "0")
        {
            dt = ObjContactMaster.GetContactTrueAllData();
            string filtertext = "Name like '%" + prefixText + "%'";
            dt = new DataView(dt, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dt = ObjContactMaster.GetContactTrueAllData(id, "Individual");
        }



        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[0]["Field2"].ToString() + "/" + dt.Rows[0]["Field1"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
            }
            return filterlist;
        }
        else
        {
            DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
            string[] filterlistcon = new string[dtcon.Rows.Count];
            for (int i = 0; i < dtcon.Rows.Count; i++)
            {
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Field2"].ToString() + "/" + dtcon.Rows[i]["Field1"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            return filterlistcon;

        }

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInvoiceNo(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["ContactID"] == null)
        {
            HttpContext.Current.Session["ContactID"] = "0";
        }
        try
        {
            Inv_SalesInvoiceHeader objSinvoiceHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objSinvoiceHeader.GetSInvHeaderAllTrueByCustomerID(HttpContext.Current.Session["ContactID"].ToString());

            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["Invoice_No"].ToString();
                }
            }
            return str;
        }
        catch
        {
            string[] str = { "" };
            return str;
        }

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListOrderNo(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["ContactID"] == null)
        {
            HttpContext.Current.Session["ContactID"] = "0";
        }
        try
        {
            DataTable dt = new Inv_SalesOrderHeader(HttpContext.Current.Session["DBConnection"].ToString()).getAllDataByCustomerID(HttpContext.Current.Session["ContactID"].ToString());

            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["SalesOrderNo"].ToString();
                }
            }
            return str;
        }
        catch
        {
            string[] str = { "" };
            return str;
        }

    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContractNo(string prefixText, int count, string contextKey)
    {
        try
        {
            DataTable dt = new SM_Contract_Master(HttpContext.Current.Session["DBConnection"].ToString()).GetAllRecordByContractNo(prefixText);

            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["Contract_No"].ToString();
                }
            }
            return str;
        }
        catch
        {
            string[] str = { "" };
            return str;
        }

    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRefTo(string prefixText, int count, string contextKey)
    {

        EmployeeMaster ObjEmp = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());


        DataTable dtMain = new DataTable();
        dtMain = dt.Copy();


        string filtertext = "Emp_Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dt, filtertext, "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Emp_Name"].ToString() + "/" + dtCon.Rows[i]["Emp_Id"].ToString();
            }

        }
        return filterlist;

    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
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
        ddlLoc.Enabled = true;
        ddlLoc.SelectedValue = Session["LocId"].ToString();
        div_insert.Attributes["style"] = "display:block;";
        div_edit.Attributes["style"] = "display:none;";
        txtECustomer.Text = "";
        txtContractName.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtContractNo.Text = "";
        txtContractNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtContractNo.Text;
        hdnValue.Value = "";
        ViewState["Select"] = null;
        txtContractDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtStartdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtEnddate.Text = DateTime.Now.AddYears(1).AddDays(-1).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtOldContractNo.Text = "";
        txtInvoiceNo.Text = "";
        FillCurrency();
        gvProduct.DataSource = null;
        gvProduct.DataBind();
        txtgrossAmount.Text = "0";
        txtDiscountAmount.Text = "0";
        txtNetAmount.Text = "";
        txtContractNo.Focus();
        txtTermsandconditon.Content = null;
        txtContractNo.Focus();
        Session["FileName"] = "";
        txtDesc.Text = "";
        hdnRenewalParentTransId.Value = "";
        hdnRenewalImmidiateTransId.Value = "";
        hdnImmidiateContractEndDate.Value = "";
        txtSalesOrderID.Text = "";
        hdnIsRenewal.Value = "false";
    }
    public void FillGrid()
    {
        DataTable dt = objContractMaster.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);

        if (ddlType.SelectedValue == "Running")
        {
            dt = new DataView(dt, "Remaining_Days >='0'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlType.SelectedValue == "Closed")
        {
            dt = new DataView(dt, "Remaining_Days <'0'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlType.SelectedValue == "DueAmt")
        {
            dt = new DataView(dt, "balance <>'0'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlType.SelectedValue == "PendingInvoiceCount")
        {
            dt = new DataView(dt, "PendingInvoiceCount <>'0'", "", DataViewRowState.CurrentRows).ToTable();
        }

        GvContractMaster.DataSource = dt;
        GvContractMaster.DataBind();


        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //cmn.FillData((object)GvContractMaster, dt, "", "");


        //lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

        Session["dtContractData"] = dt;
        //AllPageCode();
    }
    public void FillBinGrid()
    {
        DataTable dt = objContractMaster.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvContractMasterBin, dt, "", "");


        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["dtContractBinData"] = dt;
        //AllPageCode();
    }
    protected void GvContractMasterBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["Select"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objContractMaster.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtContractBinData"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvContractMasterBin, dt, "", "");

        lblSelectedRecord.Text = "";
        //AllPageCode();

    }
    protected void GvContractMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvContractMasterBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtContractBinData"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvContractMasterBin, dt, "", "");


        string temp = string.Empty;

        for (int i = 0; i < GvContractMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvContractMasterBin.Rows[i].FindControl("lblgvInquiryId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvContractMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        //AllPageCode();
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlFieldNameBin.SelectedItem.Value == "Contract_Date")
        {
            if (txtValueDateBin.Text != "")
            {

                try
                {
                    objSysParam.getDateForInput(txtValueDateBin.Text);
                    txtValueBin.Text = Convert.ToDateTime(txtValueDateBin.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDateBin.Text = "";
                    txtValueBin.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDateBin);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDateBin.Focus();
                txtValueBin.Text = "";
                return;
            }
        }
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


            DataTable dtCust = (DataTable)Session["dtContractBinData"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtContractBinData"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvContractMasterBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            //if (view.ToTable().Rows.Count == 0)
            //{
            //    FillBinGrid();
            //}
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        //AllPageCode();
        txtValueBin.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillBinGrid();

        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";

        txtValueDateBin.Visible = false;
        txtValueDateBin.Text = "";
        txtValueBin.Visible = true;
        txtValueBin.Text = "";
        txtValueBin.Focus();

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);

    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        int b = 0;
        DataTable dt = objContractMaster.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        if (GvContractMasterBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        b = objContractMaster.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                FillGrid();
                FillBinGrid();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activated");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvContractMasterBin.Rows)
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);

    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        DataTable dtCustInq = (DataTable)Session["dtContractBinData"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtCustInq.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_ID"]))
                {
                    lblSelectedRecord.Text += dr["Trans_ID"] + ",";
                }
            }
            for (int i = 0; i < GvContractMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvContractMasterBin.Rows[i].FindControl("lblgvInquiryId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvContractMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtCustInqiury = (DataTable)Session["dtContractBinData"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvContractMasterBin, dtCustInqiury, "", "");

            ViewState["Select"] = null;
        }
        //AllPageCode();

    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvContractMasterBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvContractMasterBin.Rows.Count; i++)
        {
            ((CheckBox)GvContractMasterBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvContractMasterBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvContractMasterBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvContractMasterBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvContractMasterBin.Rows[index].FindControl("lblgvInquiryId");
        if (((CheckBox)GvContractMasterBin.Rows[index].FindControl("chkSelect")).Checked)
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
    }
    protected string GetDocumentNumber()
    {
        string s = ObjDoc.GetDocumentNo(true, Session["CompId"].ToString(), true, "158", "277", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
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
            ProductName = "0";
        }
        return ProductName;
    }


    //protected void Timer1_Tick(object sender, EventArgs e)
    //{

    //    txtCallTime.Text = DateTime.Now.ToString("hh:mm");
    //}

    protected void txtInvoiceNo_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dtInvoice = new DataTable();
        if (txtInvoiceNo.Text != "")
        {
            Btn_Add_Div1.Attributes.Add("Class", "fa fa-minus");
            Div_Box_Add1.Attributes.Add("Class", "box box-primary");

            try
            {
                dtInvoice = objSalesInvoiceheader.GetSInvHeaderAllByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInvoiceNo.Text);
            }
            catch
            {
            }


            if (dtInvoice.Rows.Count > 0)
            {
                hdnInvoiceId.Value = dtInvoice.Rows[0]["Trans_Id"].ToString();
                txtgrossAmount.Text = objSysParam.GetCurencyConversionForInv(dtInvoice.Rows[0]["Currency_Id"].ToString(), dtInvoice.Rows[0]["TotalAmount"].ToString());
                txtDiscountAmount.Text = objSysParam.GetCurencyConversionForInv(dtInvoice.Rows[0]["Currency_Id"].ToString(), dtInvoice.Rows[0]["NetDiscountV"].ToString());
                txtNetAmount.Text = objSysParam.GetCurencyConversionForInv(dtInvoice.Rows[0]["Currency_Id"].ToString(), dtInvoice.Rows[0]["GrandTotal"].ToString());
                ddlCurrency.SelectedValue = dtInvoice.Rows[0]["Currency_Id"].ToString();
                //cmn.FillData((object)gvProduct, objSalesInvoiceDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnInvoiceId.Value), "", "");
                //foreach (GridViewRow gvRow in gvProduct.Rows)
                //{

                //    ((Label)gvRow.FindControl("lblUnitPrice")).Text = objSysParam.GetCurencyConversionForInv(dtInvoice.Rows[0]["Currency_Id"].ToString(), ((Label)gvRow.FindControl("lblUnitPrice")).Text);
                //    ((Label)gvRow.FindControl("lblqty")).Text = objSysParam.GetCurencyConversionForInv(dtInvoice.Rows[0]["Currency_Id"].ToString(), ((Label)gvRow.FindControl("lblqty")).Text);

                //    ((Label)gvRow.FindControl("lblDiscountValue")).Text = objSysParam.GetCurencyConversionForInv(dtInvoice.Rows[0]["Currency_Id"].ToString(), ((Label)gvRow.FindControl("lblDiscountValue")).Text);

                //    ((Label)gvRow.FindControl("lblLineTotal")).Text = objSysParam.GetCurencyConversionForInv(dtInvoice.Rows[0]["Currency_Id"].ToString(), ((Label)gvRow.FindControl("lblLineTotal")).Text);



                //}

            }
            else
            {
                DisplayMessage("Invoice Not Found");
                txtInvoiceNo.Text = "";
                txtInvoiceNo.Focus();
                return;
            }
        }
    }

    //protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    //{
    //    if (UploadFile.HasFile)
    //    {

    //    }
    //}

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.SelectedItem.Value == "Contract_Date")
        {
            txtValueDateBin.Visible = true;
            txtValueBin.Visible = false;
            txtValueBin.Text = "";
            txtValueDateBin.Text = "";

        }
        else
        {
            txtValueDateBin.Visible = false;
            txtValueBin.Visible = true;
            txtValueBin.Text = "";
            txtValueDateBin.Text = "";

        }
        ddlFieldNameBin.Focus();
    }

    protected void lblorder_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openSalesOrder('" + e.CommandName.ToString() + "')", true);
    }

    protected void IbtnFileUpload_Command(object sender, CommandEventArgs e)
    {
        string id = "";
        //for parent
        if (e.CommandName.ToString() != "")
        {
            id = e.CommandName.ToString();
        }
        //for child
        else
        {
            id = e.CommandArgument.ToString();
        }

        string strContractNo = ObjDa.return_DataTable("select Contract_No from SM_Contract_Master where Trans_Id=" + id + "").Rows[0][0].ToString();
        try
        {
            FUpload1.setID(id, Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/Contract", "ServiceManagement", "Contract", strContractNo, strContractNo);
        }
        catch(Exception ex)
        {

        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    protected void IbtnFileUpload_Command(string name, string parentId, string childId)
    {
        string id = "";
        //for parent
        if (parentId != "")
        {
            id = parentId;
        }
        //for child
        else
        {
            id = childId;
        }

        FUpload1.setID(id, "Contract", "ServiceManagement", "Contract");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    public string SetDecimal(string amount)
    {
        try
        {
            if (amount == "")
            {
                return "";
            }
            return objSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), amount);
        }
        catch
        {
            return "0";
        }
    }


    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();

    }

    protected void lblAllContractHistory_Command(object sender, CommandEventArgs e)
    {
        string id = "";
        if (e.CommandName.ToString() != "")
        {
            id = e.CommandName.ToString();
        }
        else
        {
            id = e.CommandArgument.ToString();
        }

        DataTable dt_tree = objContractMaster.GetAllRecordByLocationIDNTransID(Session["LocId"].ToString(), id);

        if (dt_tree.Rows.Count > 0)
        {
            gvContractData.DataSource = dt_tree;
            gvContractData.DataBind();

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_Generated_Contracts();", true);
        }
        else
        {
            gvContractData.DataSource = null;
            gvContractData.DataBind();
        }

    }


    protected void btnAddDtls_Click(object sender, EventArgs e)
    {
        Btn_Add_Div1.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add1.Attributes.Add("Class", "box box-primary");

        if (txtScheduledDate.Text.Trim() == "")
        {
            DisplayMessage("Please Enter Scheduled Date");
            txtScheduledDate.Focus();
            return;
        }

        DataTable scheduledDt = new DataTable();
        scheduledDt.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_Id", typeof(int)), new DataColumn("Schedule_Date"), new DataColumn("Invoice_No"), new DataColumn("Invoice_Date"), new DataColumn("InvoiceAmt"), new DataColumn("DueAmt"), new DataColumn("Invoice_Id"), new DataColumn("Location_Id") });


        Label gvinvoiceDt, gvInvoiceAmt, gvDueAmt;
        HiddenField gvHdnInvoiceID, hdnLoc_Id;
        TextBox gvScheduledDate, txtInvNo;

        for (int i = 0; i < GvScheduledData.Rows.Count; i++)
        {
            gvScheduledDate = GvScheduledData.Rows[i].FindControl("gvScheduledDate") as TextBox;
            txtInvNo = GvScheduledData.Rows[i].FindControl("txtInvNo") as TextBox;
            gvinvoiceDt = GvScheduledData.Rows[i].FindControl("gvinvoiceDt") as Label;
            gvInvoiceAmt = GvScheduledData.Rows[i].FindControl("gvInvoiceAmt") as Label;
            gvDueAmt = GvScheduledData.Rows[i].FindControl("gvDueAmt") as Label;
            gvHdnInvoiceID = GvScheduledData.Rows[i].FindControl("gvHdnInvoiceID") as HiddenField;
            hdnLoc_Id = GvScheduledData.Rows[i].FindControl("hdnLoc_Id") as HiddenField;
            if (txtInvoiceNum.Text.Trim() != "" && txtInvoiceNum.Text.Trim() == txtInvNo.Text.Trim())
            {
                DisplayMessage("Invoice No Already Exists");
                txtInvoiceNum.Text = "";
                txtInvoiceNum.Focus();
                return;
            }
            else
            {
                scheduledDt.Rows.Add(i, gvScheduledDate.Text, txtInvNo.Text, gvinvoiceDt.Text, gvInvoiceAmt.Text, gvDueAmt.Text, gvHdnInvoiceID.Value, hdnLoc_Id.Value);
            }
        }



        DataTable dt_invoiceData = obj_contractDetail.GetAllDetailDataByInvoice_No(txtInvoiceNum.Text);
        if (GvScheduledData.Rows.Count > 0)
        {
            if (dt_invoiceData.Rows.Count > 0)
            {
                scheduledDt.Rows.Add(1, txtScheduledDate.Text, txtInvoiceNum.Text, dt_invoiceData.Rows[0]["Invoice_Date"].ToString(), SetDecimal(dt_invoiceData.Rows[0]["InvoiceAmt"].ToString()), SetDecimal(dt_invoiceData.Rows[0]["DueAmt"].ToString()), dt_invoiceData.Rows[0]["Invoice_Id"].ToString(), dt_invoiceData.Rows[0]["Location_Id"].ToString());
            }
            else
            {
                scheduledDt.Rows.Add(1, txtScheduledDate.Text, txtInvoiceNum.Text, "", "", "", "");
            }

        }
        else
        {
            if (dt_invoiceData.Rows.Count > 0)
            {
                scheduledDt.Rows.Add(scheduledDt.Rows.Count + 1, txtScheduledDate.Text, txtInvoiceNum.Text, dt_invoiceData.Rows[0]["Invoice_Date"].ToString(), SetDecimal(dt_invoiceData.Rows[0]["InvoiceAmt"].ToString()), SetDecimal(dt_invoiceData.Rows[0]["DueAmt"].ToString()), dt_invoiceData.Rows[0]["Invoice_Id"].ToString(), dt_invoiceData.Rows[0]["Location_Id"].ToString());
            }
            else
            {
                scheduledDt.Rows.Add(scheduledDt.Rows.Count + 1, txtScheduledDate.Text, txtInvoiceNum.Text, "", "", "", "");
            }
        }

        GvScheduledData.DataSource = scheduledDt;
        GvScheduledData.DataBind();
        txtInvoiceNum.Text = "";
        txtScheduledDate.Text = "";

    }

    protected void txtSalesOrderID_TextChanged(object sender, EventArgs e)
    {
        if (txtSalesOrderID.Text.Trim() != "")
        {
            DataTable dt_salesorderdata = new Inv_SalesOrderHeader(Session["DBConnection"].ToString()).getAllDataByCustomerID(Session["ContactID"].ToString());
            if (dt_salesorderdata.Rows.Count > 0)
            {
                dt_salesorderdata = new DataView(dt_salesorderdata, "SalesOrderNo='" + txtSalesOrderID.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt_salesorderdata.Rows.Count > 0)
                {
                    hdnSalesOrderID.Value = dt_salesorderdata.Rows[0]["Trans_Id"].ToString();
                    DataTable SOrderDetailData = new Inv_SalesOrderDetail(Session["DBConnection"].ToString()).GetSODetailBySOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnSalesOrderID.Value);
                    gvProduct.DataSource = SOrderDetailData;
                    gvProduct.DataBind();
                }
                else
                {
                    DisplayMessage("Sales Order Not Found");
                    txtSalesOrderID.Text = "";
                    txtSalesOrderID.Focus();
                    gvProduct.DataSource = null;
                    gvProduct.DataBind();
                }
            }
            else
            {
                DisplayMessage("Sales Order Not Found");
                txtSalesOrderID.Text = "";
                txtSalesOrderID.Focus();
                gvProduct.DataSource = null;
                gvProduct.DataBind();
            }


        }
        else
        {
            gvProduct.DataSource = null;
            gvProduct.DataBind();
        }
    }


    protected void GvScheduledData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Btn_Add_Div1.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add1.Attributes.Add("Class", "box box-primary");
        DataTable scheduledDt = new DataTable();
        scheduledDt.Columns.AddRange(new DataColumn[9] { new DataColumn("Trans_Id", typeof(int)), new DataColumn("Schedule_Date"), new DataColumn("Invoice_No"), new DataColumn("Invoice_Date"), new DataColumn("InvoiceAmt"), new DataColumn("Field3"), new DataColumn("Invoice_Id"), new DataColumn("Location_Id"), new DataColumn("DueAmt") });

        Label gvinvoiceDt, gvInvoiceAmt, gvDueAmt;
        TextBox gvScheduledDate, txtInvNo;
        ImageButton gvTrans_Id;
        HiddenField gvHdnInvoiceID, hdnLoc_Id;

        for (int i = 0; i < GvScheduledData.Rows.Count; i++)
        {
            gvHdnInvoiceID = GvScheduledData.Rows[i].FindControl("gvHdnInvoiceID") as HiddenField;
            gvTrans_Id = GvScheduledData.Rows[i].FindControl("gvTrans_Id") as ImageButton;
            gvScheduledDate = GvScheduledData.Rows[i].FindControl("gvScheduledDate") as TextBox;
            txtInvNo = GvScheduledData.Rows[i].FindControl("txtInvNo") as TextBox;
            gvinvoiceDt = GvScheduledData.Rows[i].FindControl("gvinvoiceDt") as Label;
            gvInvoiceAmt = GvScheduledData.Rows[i].FindControl("gvInvoiceAmt") as Label;
            gvDueAmt = GvScheduledData.Rows[i].FindControl("gvDueAmt") as Label;
            hdnLoc_Id = GvScheduledData.Rows[i].FindControl("hdnLoc_Id") as HiddenField;
            scheduledDt.Rows.Add(gvTrans_Id.CommandArgument.ToString(), gvScheduledDate.Text, txtInvNo.Text, gvinvoiceDt.Text, gvInvoiceAmt.Text, gvDueAmt.Text, gvHdnInvoiceID.Value, hdnLoc_Id.Value);
        }

        scheduledDt.Rows.RemoveAt(e.RowIndex);
        GvScheduledData.DataSource = scheduledDt;
        GvScheduledData.DataBind();
    }

    protected void lblInvoice_Amount_Command(object sender, CommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().Trim() != "")
                Session["ContactID"] = e.CommandName.ToString().Split('/')[1].ToString();
            hdnValue.Value = e.CommandArgument.ToString().Split('/')[0].ToString();
            hdnCustomerName.Value = e.CommandName.ToString().Split('/')[0].ToString();
            hdnContractNo.Value = e.CommandArgument.ToString().Split('/')[1].ToString();

            DataTable dt_scheduledDetails = obj_contractDetail.GetAllDetailDataByContractID(hdnValue.Value);
            if (dt_scheduledDetails.Rows.Count > 0)
            {
                gvScheduledDataEdit.DataSource = dt_scheduledDetails;
                gvScheduledDataEdit.DataBind();
            }
            else
            {
                gvScheduledDataEdit.DataSource = null;
                gvScheduledDataEdit.DataBind();
                return;
            }
        }
        catch (Exception err)
        {

        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_ScheduledDetails()", true);
    }


    protected void lblVisitHistory_Command(object sender, CommandEventArgs e)
    {

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "VisitHistory('" + e.CommandArgument.ToString() + "')", true);
    }

    protected void DocumentViewer1_CacheReportDocument(object sender, DevExpress.XtraReports.Web.CacheReportDocumentEventArgs e)
    {
        e.Key = Guid.NewGuid().ToString();
        Page.Session[e.Key] = e.SaveDocumentToMemoryStream();
    }

    protected void DocumentViewer1_RestoreReportDocumentFromCache(object sender, DevExpress.XtraReports.Web.RestoreReportDocumentFromCacheEventArgs e)
    {
        Stream stream = Page.Session[e.Key] as Stream;
        if (stream != null)
            e.RestoreDocumentFromStream(stream);
    }




    public string removeNegativeNo(string data)
    {
        int x = Convert.ToInt32(data);
        if (x > 0)
        {
            return data;
        }
        else
        {
            return "";
        }
    }

    protected void txtEnddate_TextChanged(object sender, EventArgs e)
    {
        if (txtStartdate.Text.Trim() == "")
        {
            DisplayMessage("Enter Contract Start Date");
            txtStartdate.Focus();
            return;
        }
        ddlOperation.Items.Clear();
        ddlOperation.Items.Insert(0, "Select");

        if (objSysParam.getDateForInput(txtStartdate.Text) > objSysParam.getDateForInput(txtEnddate.Text))
        {
            DisplayMessage("Contract End Date Must Be Greater then Start Date");
            txtEnddate.Text = "";
            txtEnddate.Focus();
            return;
        }

        if (hdnImmidiateContractEndDate.Value != "")
        {
            if (objSysParam.getDateForInput(hdnImmidiateContractEndDate.Value) > objSysParam.getDateForInput(txtEnddate.Text))
            {
                DisplayMessage("Contract End Date Must Be Greater from Previous Contract End Date");
                txtEnddate.Text = "";
                txtEnddate.Focus();
                return;
            }
        }

        ddlOperation.Items.Clear();
        ddlOperation.Items.Insert(0, "Select");
        DateTime dt1 = Convert.ToDateTime(txtStartdate.Text);
        DateTime dt2 = Convert.ToDateTime(txtEnddate.Text);

        TimeSpan ts = dt2 - dt1;

        int days = ts.Days;

        if (days > 30 || days > 31)
        {
            ddlOperation.Items.Insert(1, new ListItem("Monthly", "1"));
        }

        if (days > 90 || days > 93)
        {
            ddlOperation.Items.Insert(2, new ListItem("Quarterly", "2"));
        }

        if (days > 180 || days > 186)
        {
            ddlOperation.Items.Insert(3, new ListItem("Half - Yearly", "3"));
        }

        if (days >= 364)
        {
            ddlOperation.Items.Insert(4, new ListItem("Yearly", "4"));
        }
        ddlOperation.DataBind();
    }

    protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btn_Add_Div1.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add1.Attributes.Add("Class", "box box-primary");

        DataTable scheduledDt = new DataTable();
        scheduledDt.Columns.AddRange(new DataColumn[9] { new DataColumn("Trans_Id", typeof(int)), new DataColumn("Schedule_Date"), new DataColumn("Invoice_No"), new DataColumn("Invoice_Date"), new DataColumn("InvoiceAmt"), new DataColumn("Field3"), new DataColumn("Invoice_Id"), new DataColumn("Location_Id"), new DataColumn("DueAmt") });


        if (GvScheduledData.Rows.Count > 0)
        {
            GvScheduledData.DataSource = null;
            GvScheduledData.DataBind();
        }

        DateTime dt1 = Convert.ToDateTime(txtStartdate.Text);
        DateTime dt2 = Convert.ToDateTime(txtEnddate.Text);

        TimeSpan ts = dt2 - dt1;

        int days = ts.Days;


        if (ddlOperation.SelectedValue == "1")
        {
            days = days / 30;
        }

        if (ddlOperation.SelectedValue == "2")
        {
            days = days / 90;
        }

        if (ddlOperation.SelectedValue == "3")
        {
            days = days / 180;
        }

        if (ddlOperation.SelectedValue == "4")
        {
            days = days / 364;
        }



        string date = "";
        string date1 = txtStartdate.Text;

        for (int i = 1; i <= days; i++)
        {
            DateTime dt = Convert.ToDateTime(date1);

            if (ddlOperation.SelectedValue == "1")
            {
                date = GetDate((dt.AddMonths(1)).ToString());
            }


            if (ddlOperation.SelectedValue == "2")
            {
                date = GetDate((dt.AddMonths(3)).ToString());
            }



            if (ddlOperation.SelectedValue == "3")
            {
                date = GetDate((dt.AddMonths(6)).ToString());
            }

            if (ddlOperation.SelectedValue == "4")
            {
                date = GetDate((dt.AddYears(1)).ToString());
            }

            if (objSysParam.getDateForInput(txtEnddate.Text) >= objSysParam.getDateForInput(GetDate((dt.AddDays(-5)).ToString())))
            {
                scheduledDt.Rows.Add(i, date, "", "", "", "", "", "");
                date1 = date;
            }
            else
            {
                break;
            }


        }

        GvScheduledData.DataSource = scheduledDt;
        GvScheduledData.DataBind();

    }



    protected void txtStartdate_TextChanged(object sender, EventArgs e)
    {
        if (txtEnddate.Text.Trim() != "")
        {
            txtEnddate_TextChanged(sender, e);
        }
    }

    protected void txtInvNo_TextChanged(object sender, EventArgs e)
    {
        Btn_Add_Div1.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add1.Attributes.Add("Class", "box box-primary");

        TextBox invoiceNo = (TextBox)sender;
        GridViewRow row = invoiceNo.NamingContainer as GridViewRow;
        int rowIndex = row.RowIndex;

        for (int i = 0; i < GvScheduledData.Rows.Count; i++)
        {
            if (rowIndex != i)
            {
                if ((GvScheduledData.Rows[i].FindControl("txtInvNo") as TextBox).Text.Trim() == invoiceNo.Text.Trim() && (GvScheduledData.Rows[i].FindControl("txtInvNo") as TextBox).Text.Trim() != "")
                {
                    DisplayMessage("Invoice No Already Exist");
                    invoiceNo.Text = "";
                    invoiceNo.Focus();
                    return;
                }
            }

        }

        DataTable dt_invoiceData = obj_contractDetail.GetAllDetailDataByInvoice_No(invoiceNo.Text);

        if (dt_invoiceData.Rows.Count > 0)
        {
            (GvScheduledData.Rows[rowIndex].FindControl("gvHdnInvoiceID") as HiddenField).Value = dt_invoiceData.Rows[0]["Invoice_Id"].ToString();
            (GvScheduledData.Rows[rowIndex].FindControl("gvinvoiceDt") as Label).Text = GetDate(dt_invoiceData.Rows[0]["Invoice_Date"].ToString());
            (GvScheduledData.Rows[rowIndex].FindControl("gvInvoiceAmt") as Label).Text = SetDecimal(dt_invoiceData.Rows[0]["InvoiceAmt"].ToString());
            (GvScheduledData.Rows[rowIndex].FindControl("gvDueAmt") as Label).Text = SetDecimal(dt_invoiceData.Rows[0]["DueAmt"].ToString());
        }
        else
        {
            (GvScheduledData.Rows[rowIndex].FindControl("gvHdnInvoiceID") as HiddenField).Value = "";
            (GvScheduledData.Rows[rowIndex].FindControl("gvinvoiceDt") as Label).Text = "";
            (GvScheduledData.Rows[rowIndex].FindControl("gvInvoiceAmt") as Label).Text = "";
            (GvScheduledData.Rows[rowIndex].FindControl("gvDueAmt") as Label).Text = "";
        }

    }
    protected void grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
    {
        ASPxGridView ab = sender as ASPxGridView;

        string strValue = ab.GetRowValues(e.VisibleIndex, "Trans_Id").ToString();

        //var row = ab.GetRow(e.VisibleIndex);
        object b = ab.GetDataRow(e.VisibleIndex);
        DataRow dr = (System.Data.DataRow)b;

        if (e.ButtonID == "lnkViewDetail")
        {
            //btnEdit_Command("lnkViewDetail", strValue);
        }
        if (e.ButtonID == "btnEdit")
        {
            //btnEdit_Command("btnEdit",strValue);
        }
        if (e.ButtonID == "IbtnDelete")
        {
            IbtnDelete_Command(strValue);
        }
        if (e.ButtonID == "IbtnFileUpload")
        {
            IbtnFileUpload_Command("fileupload", dr["Field5"].ToString(), strValue);
        }

    }

    public string ChangeImage(string data)
    {
        int x = Convert.ToInt32(data);
        if (x >= 0)
        {
            return "~/Images/edit.png";
        }
        if (x < 0)
        {
            return "~/Images/contact_renewal.png";
        }
        return "~/Images/edit.png";
    }
    public string ChangeToolTip(string data)
    {
        int x = Convert.ToInt32(data);
        if (x >= 0)
        {
            return "Edit";
        }
        if (x < 0)
        {
            return "Renewal";
        }
        return "Edit";
    }


    protected void txtContractAmt_TextChanged(object sender, EventArgs e)
    {
        if (txtContractAmt.Text.Trim() != "")
        {
            int parsedValue;
            decimal parseddecimal;
            if (!int.TryParse(txtContractAmt.Text, out parsedValue))
            {

                if (!decimal.TryParse(txtContractAmt.Text, out parseddecimal))
                {
                    DisplayMessage("Amount entered was not in correct format");
                    txtContractAmt.Text = "";
                    txtContractAmt.Focus();
                    return;
                }
            }
        }
    }

    protected void txtCost_TextChanged(object sender, EventArgs e)
    {
        if (txtCost.Text.Trim() != "")
        {
            int parsedValue;
            decimal parseddecimal;
            if (!int.TryParse(txtCost.Text, out parsedValue))
            {

                if (!decimal.TryParse(txtCost.Text, out parseddecimal))
                {
                    DisplayMessage("Amount entered was not in correct format");
                    txtCost.Text = "";
                    txtCost.Focus();
                    return;
                }
            }
        }
    }

    protected void gvScheduledDate_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = sender as TextBox;

        try
        {
            Convert.ToDateTime(tb.Text);
        }
        catch
        {
            DisplayMessage("Entered Date is not in correct format");
            tb.Text = "";
            tb.Focus();
        }
    }

    protected void gvlbtnInvoiceNo_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openSalesInvoice('" + e.CommandArgument.ToString() + "','" + e.CommandName.ToString() + "')", true);
    }

    protected void lblCustomerStatement_Command(object sender, CommandEventArgs e)
    {
        var now = DateTime.Now;
        var first = new DateTime(now.Year, now.Month, 1);

        Session["To"] = GetDate(System.DateTime.Now.ToString());
        Session["From"] = GetDate(first.ToString());
        Session["CusterStatementFromDate"] = GetDate(first.ToString());
        Session["CustomerStatementToDate"] = GetDate(System.DateTime.Now.ToString());
        Session["CustomerStatementLocations"] = Session["LocId"].ToString();
        Session["CustomerId"] = Session["LocId"].ToString();

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openCustomerStatement('" + e.CommandArgument.ToString() + "','" + e.CommandName.ToString() + "')", true);
    }

    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.PaperKind = System.Drawing.Printing.PaperKind.A3;
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WritePdfToResponse();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }

    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WriteCsvToResponse();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvContractMaster, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvContractMaster, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

}